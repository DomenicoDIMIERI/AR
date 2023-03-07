Imports System.IO
Imports System.Runtime.InteropServices

Public NotInheritable Class USBDriveHandler

    'The messages to look for.
    Private Const WM_DEVICECHANGE As Integer = &H219
    Private Const DBT_DEVICEARRIVAL As Integer = &H8000
    Private Const DBT_DEVICEREMOVECOMPLETE As Integer = &H8004
    Private Const DBT_DEVTYP_VOLUME As Integer = &H2  '
    '
    'Get the information about the detected volume.
    <StructLayout(LayoutKind.Sequential)>
    Private Structure DEV_BROADCAST_VOLUME

        Dim Dbcv_Size As Integer

        Dim Dbcv_Devicetype As Integer

        Dim Dbcv_Reserved As Integer

        Dim Dbcv_Unitmask As Integer

        Dim Dbcv_Flags As Short

    End Structure




    Public Class USBDeviceEventArgs
        Inherits System.EventArgs

        Private m_DriveLetter As String

        Public Sub New()
            m_DriveLetter = ""
        End Sub

        Public Sub New(ByVal driveLetter As String)
            m_DriveLetter = driveLetter
        End Sub

        Public ReadOnly Property DriveLetter As String
            Get
                Return m_DriveLetter
            End Get
        End Property

    End Class

    Private Class USBDriveMap
        Implements IComparer

        Public files As CodeProject.FileData()

        Public Sub New()
            files = {}
        End Sub

        Public Sub New(ByVal drive As String)
            Dim t As Date = Now
            files = CodeProject.FastDirectoryEnumerator.GetFiles(drive, "*.*", System.IO.SearchOption.AllDirectories)
            Array.Sort(files, Me)
            Debug.Print("USB Mappint Time: " & (Now - t).TotalMilliseconds)
        End Sub

        Private Function Compare(x As Object, y As Object) As Integer Implements IComparer.Compare
            Dim a As CodeProject.FileData = x
            Dim b As CodeProject.FileData = y
            Return String.Compare(a.Name, b.Name, False)
        End Function

        'Public Function GetDifferences(ByVal oldMap As USBDriveMap) As USBDriveMap
        '    Dim newFiles As New System.Collections.ArrayList
        '    Dim deletedFiles As New System.Collections.ArrayList
        '    Dim changedFiles As New System.Collections.ArrayList

        '    Dim arr1 As CodeProject.FileData() = files.Clone
        '    Dim arr2 As CodeProject.FileData() = oldMap.files.Clone
        '    Dim j = 0, i As Integer = 0
        '    Dim f1, f2 As CodeProject.FileData
        '    While (i < arr1.Length)
        '        f1 = arr1(i)
        '        j = Array.BinarySearch(arr2, f1, nothing)
        '        If (j >= 0) Then
        '            f2 = arr2(j)
        '            If f1.Attributes <> f2.Attributes OrElse f1.Size <> f2.Size OrElse f1.LastWriteTime <> f2.LastWriteTime Then
        '                changedFiles.Add(f2)
        '            End If
        '        Else
        '            newFiles.Add(f1)
        '        End If
        '        arr1 = DMD.Sistema.Arrays.RemoveAt(arr1, i)
        '    End While

        'End Function

    End Class

    Private Class USBSession
        Public ReadOnly StartDate As Date = Now
        Public ReadOnly DriveName As String
        Public w As System.IO.FileSystemWatcher
        Public ReadOnly logLock As New Object
        ' Public LastMap As USBDriveMap

        Public Sub New(ByVal path As String)
            DriveName = path.Substring(0, 3)
            StartDate = Now
            '    LastMap = New USBDriveMap(path)
        End Sub

        Public Sub Prepare()
            DMD.Sistema.FileSystem.CreateRecursiveFolder(GetBaseUSBDir())
        End Sub

        Public Function GetBaseUSBDir() As String
            Dim Path As String = DriveName '.Substring(0, 3)
            Dim base As String = DMD.Sistema.ApplicationContext.UserDataFolder
            base = System.IO.Path.Combine(base, "USB\" & DMD.Sistema.Formats.FormatISODate(StartDate))
            Return System.IO.Path.Combine(base, onlychars(DriveName))
        End Function

        Private Function onlychars(ByVal path As String) As String
            Const letters As String = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"
            Dim ret As New System.Text.StringBuilder(path.Length)
            For i As Integer = 0 To path.Length - 1
                Dim ch As Char = path.Chars(i)
                If letters.IndexOf(ch) >= 0 Then
                    ret.Append(ch)
                Else
                    ret.Append("_")
                End If
            Next
            Return ret.ToString
        End Function





        Public Function GetUSBLogFile() As String
            Dim Path As String = DriveName '.Substring(0, 3)
            Dim base As String = DMD.Sistema.ApplicationContext.UserDataFolder
            base = System.IO.Path.Combine(base, "USB\" & DMD.Sistema.Formats.FormatISODate(StartDate))
            Return System.IO.Path.Combine(base, onlychars(DriveName) & ".log")
        End Function

        Public Sub LogMessage(ByVal message As String)
            SyncLock logLock
                Dim file As String = GetUSBLogFile()
                Dim text As String = ""
                If System.IO.File.Exists(file) Then
                    text = System.IO.File.ReadAllText(file)
                End If

                text &= DMD.Sistema.Formats.FormatUserDateTime(Now) & " - " & message & vbNewLine
                System.IO.File.WriteAllText(file, text)
            End SyncLock
        End Sub


    End Class

    Private Class copyInfo
        Public Source As String
        Public Target As String

        Public Sub New()

        End Sub

        Public Sub New(ByVal src As String, ByVal dest As String)
            Source = src
            Target = dest
        End Sub


    End Class


    Public Shared Event USBDeviceConnected(ByVal sender As Object, ByVal e As USBDeviceEventArgs)

    Public Shared Event USBDeviceDisconnected(ByVal sender As Object, ByVal e As USBDeviceEventArgs)

    Private Shared ReadOnly filesCopyLock As New Object
    Private Shared ReadOnly uspWatchLock As New Object
    Private Shared filesToCopy As New System.Collections.ArrayList
    Private Shared usbCopyThread As System.Threading.Thread = Nothing
    Private Shared whatchingUSBFolders As New System.Collections.ArrayList
    Private Shared WithEvents m_Frm As frmSink = Nothing


    Protected Shared Sub OnUSBDeviceConnected(ByVal e As USBDeviceEventArgs)
        AddUSBWatch(e.DriveLetter)
        RaiseEvent USBDeviceConnected(Nothing, e)
    End Sub

    Protected Shared Sub OnUSBDeviceDisconnected(ByVal e As USBDeviceEventArgs)
        RemoveUSBWatch(e.DriveLetter)
        RaiseEvent USBDeviceDisconnected(Nothing, e)
    End Sub



    Public Shared Sub AddUSBWatch(ByVal path As String)
        SyncLock uspWatchLock
            Try

                For Each w In whatchingUSBFolders
                    If w.Path = path Then Return
                Next

                Dim s As USBSession = PrepareUSBSession(path)


                s.w = New System.IO.FileSystemWatcher()
                s.w.Path = path

                s.w.NotifyFilter = s.w.NotifyFilter Or System.IO.NotifyFilters.DirectoryName Or System.IO.NotifyFilters.FileName
                'WatchFolder.NotifyFilter = WatchFolder.NotifyFilter Or IO.NotifyFilters.Attributes
                s.w.IncludeSubdirectories = True

                ' add the handler to each event
                'AddHandler WatchFolder.Changed, AddressOf logchange
                AddHandler s.w.Created, AddressOf usbfilecreated
                AddHandler s.w.Deleted, AddressOf usbfiledeleted

                ' add the rename handler as the signature is different
                AddHandler s.w.Renamed, AddressOf usbfilerenamed
                AddHandler s.w.Changed, AddressOf usbfilechanged


                whatchingUSBFolders.Add(s)

                'Set this property to true to start watching
                s.w.EnableRaisingEvents = True

            Catch ex As Exception
                DIALTPLib.Log.InternalLog(ex)
            End Try
        End SyncLock
    End Sub


    Public Shared Sub RemoveUSBWatch(ByVal path As String)
        SyncLock uspWatchLock


            For Each s As USBSession In whatchingUSBFolders
                Dim w As System.IO.FileSystemWatcher = s.w

                If w.Path = path Then
                    w.EnableRaisingEvents = False

                    'AddHandler WatchFolder.Changed, AddressOf logchange
                    RemoveHandler w.Created, AddressOf usbfilecreated
                    RemoveHandler w.Deleted, AddressOf usbfiledeleted
                    RemoveHandler w.Renamed, AddressOf usbfilerenamed
                    RemoveHandler w.Changed, AddressOf usbfilechanged

                    whatchingUSBFolders.Remove(s)

                    w.Dispose()

                    Exit For
                End If
            Next
        End SyncLock
    End Sub

    Private Shared Function GetBasePath(ByVal path As String) As String
        If Mid(path, 2, 1) = ":" Then
            Return path.Substring(0, 3)
        Else
            Return path
        End If

    End Function


    Private Shared Sub USBLogMessage(ByVal path As String, ByVal message As String)
        SyncLock filesCopyLock
            Dim session As USBSession = GetUSBSession(path)
            If (session IsNot Nothing) Then session.LogMessage(message)
        End SyncLock
    End Sub

    Private Shared Sub usbfilecreated(ByVal source As Object, ByVal e As System.IO.FileSystemEventArgs)
        Try
            USBLogMessage(GetBasePath(e.FullPath), "File [" & e.FullPath & "] creato")
            MakeUSBCopy(e.FullPath)
        Catch ex As Exception
            Debug.Print(ex.Message)
        End Try
    End Sub

    Private Shared Sub usbfilechanged(ByVal source As Object, ByVal e As System.IO.FileSystemEventArgs)
        Try
            USBLogMessage(GetBasePath(e.FullPath), "File [" & e.FullPath & "] modificato")
            MakeUSBCopy(e.FullPath)
        Catch ex As Exception
            Debug.Print(ex.Message)
        End Try
    End Sub

    Private Shared Sub usbfiledeleted(ByVal source As Object, ByVal e As System.IO.FileSystemEventArgs)
        Try
            USBLogMessage(GetBasePath(e.FullPath), "File [" & e.FullPath & "] eliminato")
        Catch ex As Exception
            Debug.Print(ex.Message)
        End Try
    End Sub

    Private Shared Sub usbfilerenamed(ByVal source As Object, ByVal e As System.IO.RenamedEventArgs)
        Try
            USBLogMessage(GetBasePath(e.FullPath), "File [" & e.OldFullPath & "] rinominato in [" & e.FullPath & "]")
            MakeUSBCopy(e.FullPath)
        Catch ex As Exception
            Debug.Print(ex.Message)
        End Try
    End Sub


    Private Shared Sub InitializeUSBWatch()
        Dim drives As System.IO.DriveInfo() = System.IO.DriveInfo.GetDrives
        For Each drive As System.IO.DriveInfo In drives
            If drive.DriveType = System.IO.DriveType.Removable AndAlso drive.IsReady Then
                AddUSBWatch(drive.RootDirectory.FullName)
            End If
        Next
    End Sub


    Private Shared Function PrepareUSBSession(ByVal path As String) As USBSession
        Dim ret As New USBSession(path)
        ret.Prepare()
        Return ret
    End Function

    Private Shared Function GetUSBSession(ByVal path As String) As USBSession
        SyncLock filesCopyLock
            Dim driveName As String = path.Substring(0, 3)
            For Each s As USBSession In whatchingUSBFolders
                If s.DriveName = driveName Then Return s
            Next
            Return Nothing
        End SyncLock
    End Function

    Private Shared Sub MakeUSBCopy(ByVal path As String)
        SyncLock filesCopyLock
            Dim session As USBSession = GetUSBSession(path)

            Dim baseDir As String = session.GetBaseUSBDir
            Dim newPath As String = System.IO.Path.Combine(baseDir, path.Substring(3))
            DMD.Sistema.FileSystem.CreateRecursiveFolder(DMD.Sistema.FileSystem.GetFolderName(newPath))

            If usbCopyThread Is Nothing Then
                usbCopyThread = New System.Threading.Thread(AddressOf usbCopyMain)
                usbCopyThread.Priority = Threading.ThreadPriority.BelowNormal
                usbCopyThread.Start()
            End If

            'If System.IO.File.Exists(newPath) Then
            '    Dim i As Integer = 0
            '    Dim name As String = DMD.Sistema.FileSystem.GetBaseName(newPath)
            '    Dim ext As String = DMD.Sistema.FileSystem.GetExtensionName(newPath)
            '    Dim basePath As String = DMD.Sistema.FileSystem.GetFolderName(newPath)
            '    While System.IO.File.Exists(basePath & "\" & name & " " & i & "." & ext)
            '        i += 1
            '    End While
            '    newPath = basePath & "\" & name & " " & i & "." & ext
            'End If

            filesToCopy.Add(New copyInfo(path, newPath))
        End SyncLock
    End Sub


    Private Shared Sub usbCopyMain()
        SyncLock filesCopyLock
            Dim i As Integer = 0
            While i < filesToCopy.Count
                Dim o As copyInfo = filesToCopy(i)
                Try
                    System.IO.File.Copy(o.Source, o.Target, True)
                    filesToCopy.RemoveAt(i)
                Catch ex As FileNotFoundException
                    filesToCopy.RemoveAt(i)
                Catch ex As Exception
                    i += 1
                End Try
            End While
            usbCopyThread = Nothing
        End SyncLock
    End Sub

    Public Shared Function IsRunning() As Boolean
        Return m_Frm IsNot Nothing
    End Function

    Public Shared Sub Start()
        If (m_Frm IsNot Nothing) Then Return
        m_Frm = New frmSink
        m_Frm.Visible = False
        InitializeUSBWatch()
    End Sub

    Public Shared Sub [Stop]()
        If (m_Frm Is Nothing) Then Return
        SyncLock uspWatchLock
            For Each s As USBSession In whatchingUSBFolders
                Dim w As System.IO.FileSystemWatcher = s.w
                w.EnableRaisingEvents = False
                'AddHandler WatchFolder.Changed, AddressOf logchange
                RemoveHandler w.Created, AddressOf usbfilecreated
                RemoveHandler w.Deleted, AddressOf usbfiledeleted
                RemoveHandler w.Renamed, AddressOf usbfilerenamed
                RemoveHandler w.Changed, AddressOf usbfilechanged
                w.Dispose()
            Next
            whatchingUSBFolders.Clear()
        End SyncLock


        If usbCopyThread IsNot Nothing Then
            Try
                usbCopyThread.Join(5000)
            Catch ex As Exception
                Try
                    usbCopyThread.Abort()
                Catch ex1 As Exception
                    DMD.Sistema.Events.NotifyUnhandledException(ex)
                End Try
            End Try
            SyncLock filesCopyLock
                filesToCopy.Clear()
            End SyncLock
            usbCopyThread = Nothing
        End If

        m_Frm.Dispose()
        m_Frm = Nothing
    End Sub

    Private Class frmSink
        Inherits System.Windows.Forms.Form


        Protected Overrides Sub WndProc(ByRef M As System.Windows.Forms.Message)
            '
            'These are the required subclassing codes for detecting device based removal and arrival.
            '
            If M.Msg = WM_DEVICECHANGE Then

                Select Case M.WParam
            '
            'Check if a device was added.
                    Case DBT_DEVICEARRIVAL

                        Dim DevType As Integer = Runtime.InteropServices.Marshal.ReadInt32(M.LParam, 4)

                        If DevType = DBT_DEVTYP_VOLUME Then

                            Dim Vol As New DEV_BROADCAST_VOLUME

                            Vol = Runtime.InteropServices.Marshal.PtrToStructure(M.LParam, GetType(DEV_BROADCAST_VOLUME))

                            If Vol.Dbcv_Flags = 0 Then

                                For i As Integer = 0 To 20

                                    If Math.Pow(2, i) = Vol.Dbcv_Unitmask Then

                                        Dim Usb As String = Chr(65 + i) + ":\"

                                        'MsgBox("Looks like a USB device was plugged in!" & vbNewLine & vbNewLine & "The drive letter is: " & Usb.ToString)
                                        OnUSBDeviceConnected(New USBDeviceEventArgs(Usb))

                                        Exit For

                                    End If

                                Next

                            End If

                        End If
                '
                'Check if the message was for the removal of a device.
                    Case DBT_DEVICEREMOVECOMPLETE

                        Dim DevType As Integer = Runtime.InteropServices.Marshal.ReadInt32(M.LParam, 4)

                        If DevType = DBT_DEVTYP_VOLUME Then

                            Dim Vol As New DEV_BROADCAST_VOLUME

                            Vol = Runtime.InteropServices.Marshal.PtrToStructure(M.LParam, GetType(DEV_BROADCAST_VOLUME))

                            If Vol.Dbcv_Flags = 0 Then

                                For i As Integer = 0 To 20

                                    If Math.Pow(2, i) = Vol.Dbcv_Unitmask Then

                                        Dim Usb As String = Chr(65 + i) + ":\"

                                        'MsgBox("Looks like a volume device was removed!" & vbNewLine & vbNewLine & "The drive letter is: " & Usb.ToString)
                                        OnUSBDeviceDisconnected(New USBDeviceEventArgs(Usb))
                                        Exit For

                                    End If

                                Next

                            End If

                        End If

                End Select

            End If

            MyBase.WndProc(M)

        End Sub




    End Class

End Class

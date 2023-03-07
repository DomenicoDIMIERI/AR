
Imports Ionic.Zip
Imports System.Drawing
Imports DMD
Imports DIALTPLib.Window

<Serializable>
Public Class CLogSession
    Implements IDisposable

    Public Shared sessionsLock As New Object
    Public Shared SessionsCount As Integer = 0


    Public SessionID As Integer
    Public StartTime As Date
    Public ScreenShots As System.Collections.ArrayList
    Public logBuffer As System.Text.StringBuilder
    Public textBuffer As System.Text.StringBuilder
    Public keysBuffer As System.Collections.ArrayList
    Public filesBuffer As System.Collections.ArrayList
    Public Description As String
    Private m_LastScreenShot As Date? = Nothing
    Private m_LastWin As IntPtr
    Private m_Saved As Boolean = False

    Public Sub New()
        DMD.DMDObject.IncreaseCounter(Me)
        SyncLock sessionsLock
            Me.ScreenShots = New System.Collections.ArrayList
            Me.logBuffer = New System.Text.StringBuilder
            Me.textBuffer = New System.Text.StringBuilder
            Me.keysBuffer = New System.Collections.ArrayList
            Me.filesBuffer = New System.Collections.ArrayList
            Me.Description = ""
            Me.SessionID = SessionsCount
            SessionsCount += 1
            Me.StartTime = Now
        End SyncLock
    End Sub

    Public Sub LogMessage(ByVal str As String)
#If VERBOSE > 0 Then
        DMD.Sistema.ApplicationContext.Log(DMD.Sistema.Formats.FormatUserDateTime(Now) & " - LogMessage: " & str)
#End If
        Dim d As Date = Now
        str = DMD.Sistema.Formats.FormatUserDateTime(d) & vbTab & str & "<br/>"
        Me.logBuffer.Append(str)

    End Sub


    Public Sub LogProxy(ByVal str As String)
#If VERBOSE > 0 Then
        DMD.Sistema.ApplicationContext.Log(DMD.Sistema.Formats.FormatUserDateTime(Now) & " - LogProxy: " & str)
#End If
        Dim d As Date = Now
        str = DMD.Sistema.Formats.FormatUserDateTime(d) & vbTab & str & "<br/>"
        Me.logBuffer.Append(str)

    End Sub

    Public Sub LogKey(ByVal e As Keyboard.KeyboardEventArgs)
#If VERBOSE > 0 Then
        DMD.Sistema.ApplicationContext.Log(DMD.Sistema.Formats.FormatUserDateTime(Now) & " - LogKey: " & e.Char)
#End If
        Me.keysBuffer.Add(e)
        If (e.IsKeyUp AndAlso e.IsPrintable) Then Me.textBuffer.Append(e.Char)

    End Sub


    Public Sub LogFile(ByVal e As System.IO.FileSystemEventArgs)
#If VERBOSE > 0 Then
        DMD.Sistema.ApplicationContext.Log(DMD.Sistema.Formats.FormatUserDateTime(Now) & " - LogFile: " & e.ChangeType & " - " & e.FullPath)
#End If
        Select Case e.ChangeType
            Case System.IO.WatcherChangeTypes.Created : Me.filesBuffer.Add(DMD.Sistema.Formats.FormatUserDateTime(Now) & " - File Creato: """ & e.FullPath & """ ")
            Case System.IO.WatcherChangeTypes.Deleted : Me.filesBuffer.Add(DMD.Sistema.Formats.FormatUserDateTime(Now) & " - File Eliminato: """ & e.FullPath & """ ")
            Case System.IO.WatcherChangeTypes.Changed : Me.filesBuffer.Add(DMD.Sistema.Formats.FormatUserDateTime(Now) & " - File Modificato: """ & e.FullPath & """ ")
            Case System.IO.WatcherChangeTypes.Renamed : Me.filesBuffer.Add(DMD.Sistema.Formats.FormatUserDateTime(Now) & " - File Rinominato da """ & DirectCast(e, System.IO.RenamedEventArgs).OldFullPath & """ a """ & DirectCast(e, System.IO.RenamedEventArgs).FullPath & """ ")
        End Select

    End Sub

    'Public ReadOnly Property ScreenShots As System.Collections.ArrayList
    '    Get
    '        SyncLock sessionsLock
    '            If (Me.m_ScreenShots Is Nothing) Then Me.m_ScreenShots = New System.Collections.ArrayList
    '            Return Me.m_ScreenShots
    '        End SyncLock
    '    End Get
    'End Property

    Public Function AddScreenShot(ByVal nome As String, ByVal isFullScreen As Boolean, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal content As Bitmap) As ScreenShot
#If VERBOSE > 0 Then
        DMD.Sistema.ApplicationContext.Log(DMD.Sistema.Formats.FormatUserDateTime(Now) & " - AddScreenShot: " & nome)
#End If
        Dim ret As New ScreenShot(nome, isFullScreen, x, y, width, height, content)
        Me.ScreenShots.Add(ret)
        Return ret

    End Function




    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        If (Me.m_Saved = False) Then Throw New Exception("non salvato")
        Me.ScreenShots = Nothing
        Me.filesBuffer = Nothing
        Me.keysBuffer = Nothing
        Me.logBuffer = Nothing
        Me.textBuffer = Nothing
        Me.Description = vbNullString
        If Me.ScreenShots IsNot Nothing Then
            For Each sc As ScreenShot In Me.ScreenShots
                sc.Dispose()
            Next
        End If
        Me.ScreenShots = Nothing
    End Sub




    'Public Sub Upload()
    '    If Me.GetUploadServer <> "" Then
    '        My.Computer.Network.UploadFile(Me.FileName, Me.GetUploadServer & "?kname=" & My.Computer.Name)
    '    End If
    'End Sub

    Private Function DataPath() As String
        Return Right("0000" & StartTime.Year, 4) & Right("00" & StartTime.Month, 2) & Right("00" & StartTime.Day, 2) & Right("00" & StartTime.Hour, 2) & Right("00" & StartTime.Minute, 2) & Right("00" & StartTime.Second, 2)
    End Function

    Private m_FileName As String = ""
    Public Function GetDefaultFileName() As String
        If (Me.m_FileName = "") Then
            Me.m_FileName = DMD.Sistema.ApplicationContext.SystemDataFolder
            Me.m_FileName = System.IO.Path.Combine(Me.m_FileName, "TDTP" & DMD.Sistema.Formats.FormatISODate(Today))
            Me.m_FileName = System.IO.Path.Combine(Me.m_FileName, "SES" & DataPath() & Right("0000" & Hex(Me.SessionID), 4) & ".dtp")
        End If
        Return Me.m_FileName
    End Function

    Public Sub EnsureFolder()
        Dim fileName As String = Me.GetDefaultFileName
        Dim dirName As String = DMD.Sistema.FileSystem.GetFolderName(fileName)
        DMD.Sistema.FileSystem.CreateRecursiveFolder(dirName)
        System.IO.File.SetAttributes(dirName, System.IO.FileAttributes.Hidden)
    End Sub


    Public Sub Save(ByVal fileName As String)
        Dim stream As System.IO.Stream = Nothing
        Dim zip As ZipFile = Nothing
        Dim out As System.IO.StringWriter = Nothing
        Dim tmpName As String = ""
        Try
            tmpName = System.IO.Path.GetTempFileName
            stream = New System.IO.FileStream(tmpName, System.IO.FileMode.Create)
            Dim serializer As New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
            serializer.Serialize(stream, Me)
            stream.Dispose()

            out = New System.IO.StringWriter
            zip = New ZipFile
            zip.StatusMessageTextWriter = out
            zip.ZipErrorAction = ZipErrorAction.Skip
            'zip.AddDirectory(Sistema.ApplicationContext.WorkingFolder)
            zip.AddFile(tmpName, "")
            zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression   ' Ionic.Zlib.CompressionLevel.BestCompression
            zip.CompressionMethod = CompressionMethod.Deflate ' CompressionMethod.BZip2
            zip.UseZip64WhenSaving = Zip64Option.AsNecessary
            stream = New System.IO.FileStream(fileName, System.IO.FileMode.Create)
            zip.Save(stream)

        Catch ex As Exception
            ' MsgBox("error")
            Sistema.Events.NotifyUnhandledException(ex)
            Debug.Print(ex.Message & vbNewLine & ex.StackTrace)
        Finally
            If (stream IsNot Nothing) Then stream.Dispose() : stream = Nothing
            If (out IsNot Nothing) Then out.Dispose() : out = Nothing
            If (zip IsNot Nothing) Then zip.Dispose() : zip = Nothing

        End Try

        Try
            If (System.IO.File.Exists(tmpName)) Then
                System.IO.File.Delete(tmpName)
            End If
        Catch ex As Exception
            ' MsgBox("error")
            Sistema.Events.NotifyUnhandledException(ex)
            Debug.Print(ex.Message & vbNewLine & ex.StackTrace)
        End Try

        Me.m_Saved = True

    End Sub

    'Private Shared Sub FixDialTP(ByVal stream As System.IO.Stream)
    '    stream.Position = 0
    '    Dim reader As New System.IO.StreamReader(stream)
    '    Dim text As String = reader.ReadToEnd
    '    text = Replace(text, "DIALTP, Version=1.0.0.0,", "DialTPLib, Version=1.0.0.0,")
    '    Dim writer As New System.IO.StreamWriter(stream)
    '    stream.Position = 0
    '    writer.Write(text)
    '    stream.SetLength(Len(text))
    'End Sub

    Public Shared Function Load(ByVal fileName As String) As CLogSession
        Dim stream As System.IO.Stream = Nothing
        Dim zip As ZipFile = Nothing
        Dim e As Ionic.Zip.ZipEntry = Nothing
        Dim serializer As System.Runtime.Serialization.Formatters.Binary.BinaryFormatter = Nothing

        Dim tmpName As String = System.IO.Path.GetTempFileName
        stream = New System.IO.FileStream(tmpName, System.IO.FileMode.Create)

        zip = New ZipFile(fileName)
        e = zip.Entries(0)
        e.Extract(stream)
        'zip.ExtractAll(GetLogFolder, ExtractExistingFileAction.OverwriteSilently)
        zip.Dispose()

        ' FixDialTP(stream)
        stream.Position = 0
        serializer = New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
        Dim ret As CLogSession = CType(serializer.Deserialize(stream), CLogSession)
        stream.Dispose()
        Try
            System.IO.File.Delete(tmpName)
        Catch ex As Exception
            MsgBox("error")
        End Try

        Return ret
    End Function

    Sub LogException(ex As Exception)
        Me.LogMessage(TypeName(ex) & " -> " & ex.Message & vbNewLine & ex.StackTrace)
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        DMD.DMDObject.DecreaseCounter(Me)
    End Sub

    'Sub LogFileEvent(e As IO.FileSystemEventArgs)
    '    Me.filesBuffer.Add(e)
    '    If (TypeOf (e) Is System.IO.RenamedEventArgs) Then
    '        With DirectCast(e, System.IO.RenamedEventArgs)
    '            Me.LogMessage("File rinominato da [" & .OldFullPath & "] a [" & .FullPath & "]")
    '        End With
    '    Else
    '        Select Case e.ChangeType
    '            Case IO.WatcherChangeTypes.Created
    '                Me.LogMessage("File creato [" & e.FullPath & "]")
    '            Case IO.WatcherChangeTypes.Deleted
    '                Me.LogMessage("File eliminato [" & e.FullPath & "]")
    '            Case IO.WatcherChangeTypes.Changed
    '                Me.LogMessage("File modificato [" & e.FullPath & "]")
    '        End Select
    '    End If
    'End Sub

    Public Function CanTakeScreenShot() As Boolean
        If (m_LastScreenShot.HasValue) Then
            Return Math.Abs(DMD.Sistema.DateUtils.DateDiff(DateInterval.Second, m_LastScreenShot.Value, Now)) > 1
        Else
            Return True
        End If
    End Function

    Public Sub TakeFullScreenShot()
        Dim hWnd As IntPtr = IntPtr.Zero
        Dim hDCWin As IntPtr = IntPtr.Zero
            Dim gSrc As System.Drawing.Graphics = Nothing
            Dim hDCImg As IntPtr = IntPtr.Zero
            Dim img As System.Drawing.Bitmap = Nothing
            Dim g As System.Drawing.Graphics = Nothing
            Dim pen As System.Drawing.Pen = Nothing

#If Not DEBUG Then
            Try
#End If
            If Not CanTakeScreenShot() Then Return

            'If DialTPApp.CurrentConfig.WindowShot = False Then Return
            If Sistema.ApplicationContext.IsDebug Then
                Sistema.ApplicationContext.Log("TakeFullScreenShot")
            End If


            If hWnd.ToInt32 = 0 Then hWnd = Window.GetDesktopWindow 'Window.GetForegroundWindow '
            If hDCWin.ToInt32 = 0 Then hDCWin = Window.GetWindowDC(hWnd)

            Dim rect As RECTAPI = Nothing

            Window.GetWindowRect(hWnd, rect)

            gSrc = System.Drawing.Graphics.FromHdc(hDCWin)

            Dim w As Integer = CInt(Math.Abs(rect.right - rect.left) + 1)
            Dim h As Integer = CInt(Math.Abs(rect.bottom - rect.top) + 1)

            img = New System.Drawing.Bitmap(w, h, gSrc)
            g = System.Drawing.Graphics.FromImage(img)
            hDCImg = g.GetHdc

            Window.StretchBlt(hDCImg, 0, 0, img.Width, img.Height, hDCWin, 0, 0, img.Width, img.Height, TernaryRasterOperations.SRCCOPY)

            If (Not hDCImg.Equals(IntPtr.Zero)) Then
                g.ReleaseHdc(hDCImg)
                hDCImg = IntPtr.Zero
            End If

            Dim color As System.Drawing.Color = System.Drawing.Color.FromArgb(150, 255, 0, 0)

            pen = New System.Drawing.Pen(color, 2)
            With Mouse.Position
                g.DrawLine(pen, 0, .Y - rect.top, img.Width, .Y - rect.top)
                g.DrawLine(pen, .X - rect.left, 0, .X - rect.left, img.Height)
            End With


            Me.AddScreenShot("FULL SCREENSHOT " & DMD.Sistema.Formats.FormatUserDateTime(DMD.Sistema.DateUtils.Now), True, 0, 0, img.Width, img.Height, img)
            m_LastScreenShot = Now
#If Not DEBUG Then
        Catch ex As Exception
            If (Sistema.ApplicationContext.IsDebug) Then
                Throw
            Else
                Sistema.Events.NotifyUnhandledException(ex)
            End If

        Finally
#End If
            If (pen IsNot Nothing) Then pen.Dispose() : pen = Nothing

            If (g IsNot Nothing) Then
                If (Not hDCImg.Equals(IntPtr.Zero)) Then
                    g.ReleaseHdc(hDCImg)
                    hDCImg = IntPtr.Zero
                End If
                g.Dispose()
                g = Nothing
            End If

            If (Not hWnd.Equals(IntPtr.Zero)) Then
                If Not hDCWin.Equals(IntPtr.Zero) Then
                    ReleaseDC(hWnd, hDCWin)
                    hDCWin = IntPtr.Zero
                End If
                hWnd = IntPtr.Zero
            End If

            If gSrc IsNot Nothing Then gSrc.Dispose() : gSrc = Nothing
#If Not DEBUG Then
        End Try
#End If


    End Sub

    Public Sub TakeMouseScreenShot()
#If Not DEBUG Then
            Try
#End If
        If Not CanTakeScreenShot() Then Return

        Dim currWin As IntPtr = Window.GetForegroundWindow()

        If (Not m_LastWin.Equals(currWin)) Then
            TakeFullScreenShot()
        Else
            Dim p As System.Drawing.Point = Mouse.Position

            Dim img As System.Drawing.Bitmap = Window.GetDesktopPortion(p.X - 100, p.Y - 100, 200, 200)

            Me.AddScreenShot("PARZIALE", False, p.X - 100, p.Y - 100, 200, 200, img)
            m_LastScreenShot = Now
        End If
        m_LastWin = currWin
#If Not DEBUG Then
            Catch ex As Exception
                If Sistema.ApplicationContext.IsDebug Then
                    Throw
                Else
                    Sistema.Events.NotifyUnhandledException(ex)
                End If
            Finally

            End Try
#End If
    End Sub

    Public Sub CheckWindow()
        Dim currWin As IntPtr = Window.GetForegroundWindow()
        If (Not m_LastWin.Equals(currWin)) Then TakeFullScreenShot()
        m_LastWin = currWin
    End Sub

End Class

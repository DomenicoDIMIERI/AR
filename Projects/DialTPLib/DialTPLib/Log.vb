Imports System.Runtime.InteropServices
Imports System.Reflection
Imports System.Drawing
Imports System.Threading
Imports System.Net.Mail
Imports System.Drawing.Imaging
Imports System.Diagnostics
Imports Ionic.Zip
Imports System.Net
Imports DIALTPLib.Memory
Imports DIALTPLib.Window
Imports DMD
Imports DMD.Sistema
Imports System.Management
Imports System.ComponentModel
Imports System.Windows.Forms

Public Class Log
    Private Class MailInfo
        Public files As New ArrayList
        Public m As System.Net.Mail.MailMessage

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class


    Private Shared m_SentMessages As New DMD.CCollection(Of MailInfo)
    Private Shared ReadOnly sessionsLock As New Object
    Private Shared m_CurrSession As CLogSession = Nothing
    Private Shared m_Timer As System.Timers.Timer = Nothing
    Private Shared m_TimerEvents As System.Timers.Timer = Nothing
    Private Shared pendinSessions As New CCollection(Of CLogSession)

    Shared Sub New()
        m_Timer = New System.Timers.Timer
        m_Timer.Interval = 15 * 1000
        AddHandler m_Timer.Elapsed, AddressOf handleTimer
        m_Timer.Enabled = True

        m_TimerEvents = New System.Timers.Timer
        m_TimerEvents.Interval = 1 * 1000
        AddHandler m_TimerEvents.Elapsed, AddressOf handleTimerEvents
        m_TimerEvents.Enabled = True
    End Sub

    Private Shared Sub handleTimerEvents(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim k As Keyboard.KeyboardEventArgs = Nothing

        SyncLock keys
            If (keys.Count > 0) Then
                Dim s As CLogSession = EnsureSession()
                While (keys.Count > 0)
                    k = keys(0)
                    s.LogKey(k)
                    keys.RemoveAt(0)
                End While
            End If
        End SyncLock

        If m_TakeMouseShot Then
            Dim s As CLogSession = EnsureSession()
            Try
                s.TakeMouseScreenShot()
            Catch ex As Exception
                Try
                    s.TakeFullScreenShot()
                    If (s.ScreenShots.Count > 5) Then
                        Reset()
                    End If
                Catch ex1 As Exception
                    Sistema.Events.NotifyUnhandledException(ex)
                End Try
            End Try
            m_TakeMouseShot = False
        End If

    End Sub


    Private Shared Sub handleTimer(ByVal sender As Object, ByVal e As System.EventArgs)
        SyncLock sessionsLock


            If pendinSessions.Count = 0 Then Return
            Dim s As CLogSession = pendinSessions(0)
            pendinSessions.RemoveAt(0)

            s.LogMessage("Chiudo la sessione di log " & s.SessionID)
            s.EnsureFolder()
            Dim fileName As String = s.GetDefaultFileName
            s.Save(fileName)
            s.Dispose()
        End SyncLock
    End Sub

    Public Shared Sub LogNetworkConfiguration()
        Dim strHostName As String = System.Net.Dns.GetHostName()
        LogMessage("HostName: " & strHostName)
        For Each hostAdr As System.Net.IPAddress In System.Net.Dns.GetHostEntry(strHostName).AddressList()
            LogMessage("---> " & hostAdr.ToString)
        Next
    End Sub


    <Serializable>
    Public Class LogSession
        Implements IDisposable

        Public Shared sessionsLock As New Object
        Public Shared SessionsCount As Integer = 0

        Public SessionID As Integer
        Public StartTime As Date = Now
        Public images As New System.Collections.ArrayList
        Public logBuffer As New System.Text.StringBuilder
        Public textBuffer As New System.Text.StringBuilder
        Public keysBuffer As New System.Text.StringBuilder
        Public filesBuffer As New System.Text.StringBuilder
        Public FileName As String = ""
        Public Description As String = ""


        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            SyncLock sessionsLock
                Me.SessionID = SessionsCount
                SessionsCount += 1
            End SyncLock
            'In case you end up with "0" (probably because you just created the PerformanceCounter and then directly used it) you need to add 2 lines as the PerformanceCounter need some time to work:


        End Sub

        Public Sub Append(ByVal str As String)
            SyncLock Me
                Dim d As Date = Now
                str = DMD.Sistema.Formats.FormatUserDateTime(d) & vbTab & str & "<br/>"
                Me.logBuffer.Append(str)
            End SyncLock
        End Sub


        Public Sub AppendKeyabord(ByVal str As String)
            SyncLock Me
                Dim d As Date = Now
                str = DMD.Sistema.Formats.FormatUserDateTime(d) & vbTab & str & "<br/>"
                Me.keysBuffer.Append(str)
            End SyncLock
        End Sub

        Public Sub AppendChar(ByVal str As String)
            SyncLock Me
                Me.textBuffer.Append(str)
            End SyncLock
        End Sub

        Public Sub AppendFile(ByVal str As String)
            SyncLock Me
                Dim d As Date = Now
                str = DMD.Sistema.Formats.FormatUserDateTime(d) & vbTab & str & "<br/>"
                Me.filesBuffer.Append(str)
            End SyncLock
        End Sub




        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            For Each img As System.Drawing.Image In Me.images
                img.Dispose()
            Next
            Me.images = Nothing
            Me.filesBuffer = Nothing
            Me.keysBuffer = Nothing
            Me.logBuffer = Nothing
            Me.textBuffer = Nothing

            Me.FileName = vbNullString
            Me.Description = vbNullString

        End Sub


        Public Function GetUploadServer() As String
            Const avellino As String = "192.168.7."
            Const benevento As String = "192.168.74."
            Const salerno As String = "192.168.6."
            Const teggiano As String = "192.168.70."

            Dim ip As IPAddress = ProxyService.GetIP("localhost")
            Dim str As String = ip.ToString

            If (Left(str, Len(avellino)) = avellino) Then
                Return "http://192.168.7.253/Apps/dialtp/ping.aspx"
            ElseIf (Left(str, Len(benevento)) = benevento) Then
                Return "http://192.168.74.253/Apps/dialtp/ping.aspx"
            ElseIf (Left(str, Len(salerno)) = salerno) Then
                Return "http://192.168.6.253/Apps/dialtp/ping.aspx"
            ElseIf (Left(str, Len(teggiano)) = teggiano) Then
                Return "http://192.168.70.253/Apps/dialtp/ping.aspx"
            Else
                Return DIALTPLib.DialTPApp.CurrentConfig.UploadServer
            End If
        End Function

        'Public Sub Upload()
        '    If Me.GetUploadServer <> "" Then
        '        My.Computer.Network.UploadFile(Me.FileName, Me.GetUploadServer & "?kname=" & My.Computer.Name)
        '    End If
        'End Sub

        Private Function DataPath() As String
            Dim d As Date = Now
            Return Right("0000" & d.Year, 4) & Right("00" & d.Month, 2) & Right("00" & d.Day, 2) & Right("00" & d.Hour, 2) & Right("00" & d.Minute, 2) & Right("00" & d.Second, 2)
        End Function

        Public Sub Initialize()
            'If (Me.FileName <> "") Then Return

            'SyncLock lockObject
            '    Dim folder As String = GetLogFolder()
            '    Me.FileName = folder & "\" & Me.DataPath & ".dtp"
            '    Dim i As Integer = 1
            '    While System.IO.File.Exists(Me.FileName)
            '        Me.FileName = folder & "" & Me.DataPath & Right("0000" & i, 4) & ".dtp"
            '        i += 1
            '    End While
            '    Dim stream As New System.IO.FileStream(Me.FileName, System.IO.FileMode.Create)
            '    stream.Dispose()
            'End SyncLock
        End Sub

        Public Sub Save()


            Dim stream As System.IO.Stream = Nothing
            stream = New System.IO.FileStream(Me.FileName & ".tmp", System.IO.FileMode.Create)
            Dim serializer As New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
            serializer.Serialize(stream, Me)
            stream.Dispose()


            Dim out As New System.IO.StringWriter
            Dim zip As New ZipFile
            zip.StatusMessageTextWriter = out
            zip.ZipErrorAction = ZipErrorAction.Skip
            'zip.AddDirectory(Sistema.ApplicationContext.WorkingFolder)
            zip.AddFile(Me.FileName & ".tmp", "")
            zip.CompressionLevel = Ionic.Zlib.CompressionLevel.None ' Ionic.Zlib.CompressionLevel.BestCompression
            zip.CompressionMethod = CompressionMethod.Deflate ' CompressionMethod.BZip2
            zip.UseZip64WhenSaving = Zip64Option.AsNecessary

            stream = New System.IO.FileStream(Me.FileName, System.IO.FileMode.Create)
            zip.Save(stream)
            stream.Dispose()
            out.Dispose()

            DMD.Sistema.FileSystem.DeleteFile(Me.FileName & ".tmp")
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

        Public Shared Function Load(ByVal fileName As String) As LogSession
            'Dim fName As String
            Dim stream As System.IO.Stream = Nothing
            Dim zip As ZipFile = Nothing
            Dim e As Ionic.Zip.ZipEntry = Nothing
            Dim serializer As System.Runtime.Serialization.Formatters.Binary.BinaryFormatter = Nothing
            Dim ret As LogSession = Nothing

#If Not DEBUG Then
            Try
#End If
            stream = New System.IO.MemoryStream

            zip = New ZipFile(fileName)
            e = zip.Entries(0)
            e.Extract(stream)
            'zip.ExtractAll(GetLogFolder, ExtractExistingFileAction.OverwriteSilently)
            zip.Dispose()

            ' FixDialTP(stream)

            stream.Position = 0
            serializer = New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
            ret = DirectCast(serializer.Deserialize(stream), DIALTPLib.Log.LogSession)
            stream.Dispose()

            'DMD.Sistema.FileSystem.DeleteFile(GetLogFolder1() & "\" & fName & ".tmp")
#If Not DEBUG Then
            Catch ex As Exception
                Throw
            Finally
#End If
            If (zip IsNot Nothing) Then zip.Dispose()
            If (stream IsNot Nothing) Then stream.Dispose()
#If Not DEBUG Then
            End Try
#End If
            Return ret
        End Function

        Public Sub Delete()
            DMD.Sistema.FileSystem.DeleteFile(Me.FileName)
        End Sub

        Sub LogNetworkConfiguration()
            Dim strHostName As String = System.Net.Dns.GetHostName()
            Me.Append("HostName: " & strHostName)

            For Each hostAdr As System.Net.IPAddress In System.Net.Dns.GetHostEntry(strHostName).AddressList()
                Me.Append("---> " & hostAdr.ToString)
            Next
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

    'Private Shared lock As New Object


    Public Shared Sub Reset()
        SyncLock lock
            Try
                Dim s As CLogSession = GetCurrentSession()
                If s IsNot Nothing Then pendinSessions.Add(s)
                m_CurrSession = Nothing
                m_CurrSession = EnsureSession()
            Catch ex As Exception
                Debug.Print(ex.Message & vbNewLine & ex.StackTrace)
            End Try
        End SyncLock
    End Sub


    Public Shared Function GetCPUUsage() As Double
        Dim cpu As PerformanceCounter = Nothing
        Try
            cpu = New PerformanceCounter()
            With cpu
                .CategoryName = "Processor"
                .CounterName = "% Processor Time"
                .InstanceName = "_Total"
            End With

            cpu.NextValue()

            Return Math.Ceiling(cpu.NextValue)
        Catch ex As Exception
            Return -1
        Finally
            If (cpu IsNot Nothing) Then cpu.Dispose() : cpu = Nothing
        End Try
    End Function

    Public Shared Sub NotifyPCStatus(ByVal eventName As String)
        Dim text As String = ""
        Dim cpuUsage As Double = GetCPUUsage()
        Dim sysTemp As Single = GetSystemTemperature()
        Dim mem As MEMORYSTATUSEX = Memory.GetMemoryStatus
        Dim DiskTotal As Long = -1
        Dim DiskAvail As Long = -1
        Dim RAMTotal As Long = CLng(mem.ullTotalPhys)
        Dim RAMAvail As Long = CLng(mem.ullAvailPhys)
        Dim RAMPageFileTotal As Long = CLng(mem.ullTotalPageFile)
        Dim RAMPageFileAval As Long = CLng(mem.ullAvailPageFile)
        Dim RAMVirtualTotal As Long = CLng(mem.ullTotalVirtual)
        Dim RAMVirtualAval As Long = CLng(mem.ullAvailVirtual)
        Dim screenWidth As Integer = Screen.PrimaryScreen.Bounds.Width
        Dim screenHeight As Integer = Screen.PrimaryScreen.Bounds.Height
        Dim screenColors As Integer = Screen.PrimaryScreen.BitsPerPixel
        Dim drives As System.IO.DriveInfo() = System.IO.DriveInfo.GetDrives
        Dim userName As String = DialTPApp.CurrentConfig.UserName
        Dim unita As String


        text &= "<table border=""1"" cellpadding=""3px"">"
        text &= "<tr><td>Stato:" & vbTab & "</td><td style=""text-align:right;"">" & Strings.HtmlEncode(eventName) & " </td></tr>"
        text &= "<tr><td>Sistema Operativo:" & vbTab & "</td><td style=""text-align:right;"">" & Strings.HtmlEncode(Machine.GetOperatoratingSystemVersion) & " </td></tr>"
        text &= "<tr><td>Seriale CPU:" & vbTab & "</td><td style=""text-align:right;"">" & Strings.HtmlEncode(Machine.GetCPUSerialNumber) & " </td></tr>"
        text &= "<tr><td>Utente:" & vbTab & "</td><td style=""text-align:right;"">" & Strings.HtmlEncode(userName) & " %</td></tr>"
        text &= "<tr><td>CPU:" & vbTab & "</td><td style=""text-align:right;"">" & DMD.Sistema.Formats.FormatPercentage(cpuUsage) & " %</td></tr>"
        text &= "<tr><td>Temperatura:" & vbTab & "</td><td style=""text-align:right;"">" & DMD.Sistema.Formats.FormatInteger(sysTemp) & " &deg;</td></tr>"
        text &= "<tr><td>Disco Totale:" & vbTab & "</td><td style=""text-align:right;"">" & DMD.Sistema.Formats.FormatInteger(DiskTotal) & " Bytes</td></tr>"
        text &= "<tr><td>Disco Disponibile:" & vbTab & "</td><td style=""text-align:right;"">" & DMD.Sistema.Formats.FormatInteger(DiskAvail) & " Bytes</td></tr>"
        text &= "<tr><td>Schermo:" & vbTab & "</td><td style=""text-align:right;"">" & screenWidth & " x " & screenHeight & " (" & screenColors & " bits)</td></tr>"
        text &= "<tr><td>RAM Totale:" & vbTab & "</td><td style=""text-align:right;"">" & DMD.Sistema.Formats.FormatInteger(RAMTotal) & " Bytes</td></tr>"
        text &= "<tr><td>RAM Disponibile:" & vbTab & "</td><td style=""text-align:right;"">" & DMD.Sistema.Formats.FormatInteger(RAMAvail) & " Bytes</td></tr>"
        text &= "<tr><td>Paginazione Totale:" & vbTab & "</td><td style=""text-align:right;"">" & DMD.Sistema.Formats.FormatInteger(RAMPageFileTotal) & " Bytes</td></tr>"
        text &= "<tr><td>Paginazione Disponibile:" & vbTab & "</td><td style=""text-align:right;"">" & DMD.Sistema.Formats.FormatInteger(RAMPageFileAval) & " Bytes</td></tr>"
        text &= "<tr><td>Virtuale Totale:" & vbTab & "</td><td style=""text-align:right;"">" & DMD.Sistema.Formats.FormatInteger(RAMVirtualTotal) & " Bytes</td></tr>"
        text &= "<tr><td>Virtuale Disponibile:" & vbTab & "</td><td style=""text-align:right;"">" & DMD.Sistema.Formats.FormatInteger(RAMVirtualAval) & " Bytes</td></tr>"
        text &= "<tr><td>Unità logiche:" & vbTab & "</td>"
        text &= "<td style=""text-align:right;"">"

        text &= "<table>"
        text &= "<tr>"
        text &= "<th>Nome</th>"
        text &= "<th>Etichetta</th>"
        text &= "<th>Root</th>"
        text &= "<th>Dimensione</th>"
        text &= "<th>Libera</th>"
        text &= "<th>Disponibile</th>"
        text &= "</tr>"
        unita = ""
        For Each drive As System.IO.DriveInfo In drives
            Dim nomeUnita As String = drive.Name
            If (nomeUnita.Substring(0, 2).ToLower = Environment.SystemDirectory.Substring(0, 2).ToLower) Then
                Try
                    DiskTotal = drive.TotalSize
                    DiskAvail = drive.AvailableFreeSpace
                Catch ex As Exception

                End Try
            End If

            Try
                nomeUnita = drive.Name & " (" & drive.TotalSize & ", " & drive.TotalFreeSpace & ", " & drive.AvailableFreeSpace & ")"
                Select Case drive.DriveType
                'Case IO.DriveType.CDRom
                'Case IO.DriveType.Fixed
                'Case IO.DriveType.Network
                    Case System.IO.DriveType.Removable, System.IO.DriveType.CDRom
                        text &= "<tr>"
                        text &= "<td>" & drive.Name & " (?)</td>"
                        text &= "<td></td>"
                        text &= "<td></td>"
                        text &= "<td style=""text-align:right;""></td>"
                        text &= "<td style=""text-align:right;""></td>"
                        text &= "<td style=""text-align:right;""></td>"
                        text &= "</tr>"
                    Case Else
                        text &= "<tr>"
                        text &= "<td>" & drive.Name & " (" & drive.IsReady & ")</td>"
                        text &= "<td>" & drive.VolumeLabel & "</td>"
                        text &= "<td>" & drive.RootDirectory.FullName & "</td>"
                        text &= "<td style=""text-align:right;"">" & DMD.Sistema.Formats.FormatBytes(drive.TotalSize) & " Bytes</td>"
                        text &= "<td style=""text-align:right;"">" & DMD.Sistema.Formats.FormatBytes(drive.TotalFreeSpace) & " Bytes</td>"
                        text &= "<td style=""text-align:right;"">" & DMD.Sistema.Formats.FormatBytes(drive.AvailableFreeSpace) & " Bytes</td>"
                        text &= "</tr>"
                End Select
            Catch ex As Exception

            End Try
            unita = Strings.Combine(unita, nomeUnita, "; ")
        Next
        text &= "</table>"

        text &= "</td></tr>"
        text &= "</table>"


        'text &= Processi.CheckProcesses1()

        LogMessage(text)

#If Not DEBUG Then
        Try
#End If

        Dim serverName As String = DIALTPLib.Settings.ConfigServer
        If (serverName <> "") Then
            If Not (serverName.EndsWith("/")) Then serverName = serverName & "/"

            Dim col As New CKeyCollection

            col.Add("EventName", eventName)
            col.Add("OSVersion", Machine.GetOperatoratingSystemVersion)
            col.Add("CPUSerialNumber", Machine.GetCPUSerialNumber)
            col.Add("SerialNumber", Machine.GetSerialNumber)
            col.Add("IDAppdress", Machine.GetIPAddress)
            col.Add("MACAppdress", Machine.GetMACAddress)
            col.Add("CPUUsage", cpuUsage)
            col.Add("SYSTemperature", sysTemp)
            col.Add("DiskTotal", DiskTotal)
            col.Add("DiskAvail", DiskAvail)
            col.Add("RAMTotal", RAMTotal)
            col.Add("RAMAvail", RAMAvail)
            col.Add("RAMPageFileTotal", RAMPageFileTotal)
            col.Add("RAMPageFileAval", RAMPageFileAval)
            col.Add("RAMVirtualTotal", RAMVirtualTotal)
            col.Add("RAMVirtualAval", RAMVirtualAval)
            col.Add("ScreenWidth", screenWidth)
            col.Add("ScreenHeight", screenHeight)
            col.Add("ScreenColors", screenColors)
            col.Add("currentUser", userName)
            col.Add("dtpVersion", My.Application.Info.Version.ToString)
            col.Add("BootTime", Machine.BootTime)
            col.Add("Units", unita)
            col.Add("AudioInDevices", Strings.Join(Machine.GetAudioInputDeviceNames, vbLf))

            Dim tmp As String = DMD.Sistema.RPC.InvokeMethod(serverName & "widgets/websvcf/dialtp.aspx?_a=NotifyPCStatus&oname=" & DialTPApp.CurrentConfig.IDPostazione & "&uname=" & userName, "col", XML.Utils.Serializer.Serialize(col))
            Dim ret As CKeyCollection = CType(XML.Utils.Serializer.Deserialize(tmp), CKeyCollection)
            Dim cmd As DialTPCommand = CType(ret.GetItemByKey("DMD_Action"), DialTPCommand)
            If (cmd IsNot Nothing) Then
                LogMessage("Esecuzione comando remoto: " & cmd.Name)
                cmd.Execute()
            End If
        End If

#If Not DEBUG Then
        Catch ex As Exception
            LogException(ex)
        End Try
#End If
    End Sub



    Public Shared Sub CheckWindow()
        Dim s As CLogSession = GetCurrentSession()
        If (s Is Nothing) Then Return
        s.CheckWindow()
    End Sub

    'Shared Sub DeleteTemporaryFiles()
    '    Try
    '        DMD.Sistema.FileSystem.DeleteFile(GetLogFolder1() & "\*.*", True)
    '    Catch ex As Exception

    '    End Try
    'End Sub

    Private Shared Function CanTakeScreenShot() As Boolean
        SyncLock sessionsLock
            Dim s As CLogSession = GetCurrentSession()
            If (s Is Nothing) Then Return False
            Return s.CanTakeScreenShot
        End SyncLock
    End Function

    Public Shared Sub TakeFullScreenShot()
        'SyncLock sessionsLock
        Try
            Dim s As CLogSession = EnsureSession()
            s.TakeFullScreenShot()
            If (s.ScreenShots.Count > 5) Then
                Reset()
            End If
        Catch ex As Exception
            Sistema.Events.NotifyUnhandledException(ex)
        End Try
        ' End SyncLock
    End Sub

    Private Shared m_TakeMouseShot As Boolean = True

    Public Shared Sub TakeMouseScreenShot()
        If (Not CStr(DIALTPLib.DialTPApp.CurrentConfig.Attributi.GetItemByKey("ALLOWSCREENSHOTS")) = "true") Then Return
        m_TakeMouseShot = True
    End Sub

    Public Shared Function GetSystemTemperature() As Single
        Try
            Dim searcher As ManagementObjectSearcher = New ManagementObjectSearcher("root\WMI", "SELECT * FROM MSAcpi_ThermalZoneTemperature")
            Dim enumerator As ManagementObjectCollection.ManagementObjectEnumerator = searcher.Get().GetEnumerator()

            If (enumerator IsNot Nothing) Then
                While (enumerator.MoveNext())
                    Dim tempObject As ManagementBaseObject = enumerator.Current
                    'Console.WriteLine(tempObject("CurrentTemperature").ToString())

                    Return CSng(CSng(tempObject("CurrentTemperature")) / 10 - 273.15)
                End While
            End If

            Return -1
        Catch ex As Exception
            Return -2
        End Try
    End Function


    Shared Sub LogMessage(ByVal message As String)
        EnsureSession().LogMessage(message)
    End Sub

    Shared Sub LogProxy(ByVal message As String)
        EnsureSession().LogProxy(message)
    End Sub

    Public Shared Sub LogException(ex As Exception)
        EnsureSession().LogException(ex)
    End Sub

    Private Shared keys As New CCollection(Of Keyboard.KeyboardEventArgs)

    Shared Sub LogKey(e As Keyboard.KeyboardEventArgs)
        SyncLock keys
            keys.Add(e)
        End SyncLock
    End Sub

    Shared Sub Terminate()
        SyncLock sessionsLock
            m_TakeMouseShot = False
            Dim s As CLogSession = EnsureSession()
            s.TakeFullScreenShot()
            While (keys.Count > 0)
                s.LogKey(keys(0))
                keys.RemoveAt(0)
            End While
            LogMessage("Termino il log delle attività")
            LogMessage("Chiudo la sessione di log " & s.SessionID)
            s.EnsureFolder()
            Dim fileName As String = s.GetDefaultFileName
            System.Threading.Thread.Sleep(1000)
            s.Save(fileName)
            s.Dispose()
            s = Nothing

            m_CurrSession = Nothing
        End SyncLock
    End Sub

    Shared Sub StartLogging()
        EnsureSession()
    End Sub

    Public Shared Function GetSessionName() As String
        SyncLock sessionsLock
            Dim s As CLogSession = GetCurrentSession()
            If s Is Nothing Then Return ""
            Return s.GetDefaultFileName
        End SyncLock
    End Function

    Public Shared Function EnsureSession() As CLogSession
        SyncLock sessionsLock
            If m_CurrSession Is Nothing Then
                m_CurrSession = New CLogSession
                LogMessage("Apro la sessione di log " & m_CurrSession.SessionID)
            End If
            Return m_CurrSession
        End SyncLock
    End Function

    Public Shared Function GetCurrentSession() As CLogSession
        EnsureSession()
        Return m_CurrSession
    End Function

    Public Shared Function GetMachineName() As String
        Return My.Computer.Name
    End Function

    Public Shared Function GetCurrentUserName() As String
        Return Environment.UserName
    End Function

    Public Shared Function GetUserDataPath() As String
        Return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
    End Function

    Public Shared Function GetNetworkConfig() As String
        Return "Network Present: " & CStr(IIf(My.Computer.Network.IsAvailable, "Sì", "No"))
    End Function

    'Private Class DeleteWorker
    '    Inherits System.ComponentModel.BackgroundWorker

    '    Private d As Date

    '    Public Sub New(ByVal d As Date)

    '        Me.d = d
    '    End Sub




    '    Protected Overrides Sub OnDoWork(e As DoWorkEventArgs)
    '        Dim path As String = DMD.Sistema.ApplicationContext.UserDataFolder & "\data\"
    '        Dim finfo As New System.IO.DirectoryInfo(path)
    '        If (finfo.Exists = False) Then Return
    '        'Dim files() As CodeProject.FileData = CodeProject.FastDirectoryEnumerator.GetFiles(path, "*.dtp", System.IO.SearchOption.TopDirectoryOnly)

    '        Dim files As IEnumerable(Of CodeProject.FileData) = CodeProject.FastDirectoryEnumerator.EnumerateFiles(path, "*.dtp", System.IO.SearchOption.TopDirectoryOnly)
    '        For Each file As CodeProject.FileData In files
    '            If DateUtils.Compare(file.CreationTime, d) < 0 Then
    '                Try
    '                    file.Delete()
    '                Catch ex As Exception
    '                    ' Sistema.Events.NotifyUnhandledException(ex)
    '                End Try
    '                'System.Threading.Thread.Sleep(100)
    '            End If
    '        Next

    '        MyBase.OnDoWork(e)
    '    End Sub
    'End Class

    Private Shared timerLock As New Object
    Private Shared timerDf As System.Timers.Timer = Nothing
    Private Shared timerdfd As Date

    Public Shared Sub DeleteFilesBefore(ByVal d As Date)
        SyncLock timerLock
            If (timerDf Is Nothing) Then
                timerdfd = d
                timerDf = New System.Timers.Timer
                AddHandler timerDf.Elapsed, AddressOf handletimerdf
                timerDf.Interval = 1000 * 60 * 5
                timerDf.Enabled = True
            End If
        End SyncLock
    End Sub

    Private Shared Sub handletimerdf(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs)
        SyncLock timerLock
            Try
                'Dim worker As New DeleteWorker(timerdfd)

                'worker.RunWorkerAsync()
                timerDf.Enabled = False
                timerDf.Dispose()
                timerDf = Nothing

                Dim p As New System.Threading.Thread(AddressOf threaddeletefiles)
                p.Priority = ThreadPriority.Lowest
                p.Start()
            Catch ex As Exception

            End Try
        End SyncLock
    End Sub

    Private Shared Sub threaddeletefiles()
        Dim tmpFolder As New System.IO.DirectoryInfo(DMD.Sistema.ApplicationContext.TmporaryFolder)
        If (tmpFolder.Exists = False) Then Return
        Dim folders() As System.IO.DirectoryInfo = tmpFolder.GetDirectories("TDTP*")
        For Each folder In folders
            'Dim files As IEnumerable(Of CodeProject.FileData) = CodeProject.FastDirectoryEnumerator.EnumerateFiles(path, "*.dtp", System.IO.SearchOption.TopDirectoryOnly)
            'For Each file As CodeProject.FileData In files
            '    If DateUtils.Compare(file.CreationTime, timerdfd) < 0 Then
            '        Try
            '            file.Delete()
            '        Catch ex As Exception
            '            ' Sistema.Events.NotifyUnhandledException(ex)
            '        End Try
            '        'System.Threading.Thread.Sleep(100)
            '    End If
            'Next
            Try
                If DateUtils.Compare(folder.CreationTime, timerdfd) < 0 Then
                    folder.Delete(True)
                End If
            Catch ex As Exception

            End Try
        Next
    End Sub

    Public Shared Sub InternalLog(ex As Exception)
        InternalLog(ex.Message & vbNewLine & ex.StackTrace)
    End Sub

    Private Shared fLock As New Object

    Public Shared Sub InternalLog(ByVal message As String)
        SyncLock fLock
            Dim path As String = Sistema.ApplicationContext.UserDataFolder ' System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
            Dim fName As String = My.Application.Info.ProductName & ".log"
            Dim logFName As String = System.IO.Path.Combine(path, fName)
            System.IO.File.AppendAllText(logFName, Formats.GetTimeStamp & " - " & message & vbNewLine)
        End SyncLock
    End Sub
End Class


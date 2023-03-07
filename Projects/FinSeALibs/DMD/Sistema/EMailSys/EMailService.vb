Imports System.Net.Mail 'importo il Namespace
Imports DMD.Databases
Imports DMD.Net.Mail
Imports System.Threading
Imports DMD.Internals
Imports DMD.Sistema
Imports System.Net.Mime
Imports System.Timers
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Collections.Specialized

#Const PURGEMAILS = False' DEBUG

''Public Const cdoSendUsingPickup = 1 'Send message using the local SMTP service pickup directory. 
''Public Const cdoSendUsingPort = 2 'Send the message using the network (SMTP over the network). 
''Public Const cdoSendUsingMethod = "http://schemas.microsoft.com/cdo/configuration/sendusing"

''Public Const cdoSMTPServer = "http://schemas.microsoft.com/cdo/configuration/smtpserver"
''Public Const cdoSMTPServerPort = "http://schemas.microsoft.com/cdo/configuration/smtpserverport"
''Public Const cdoSMTPConnectionTimeout = "http://schemas.microsoft.com/cdo/configuration/smtpconnectiontimeout"

''Public Const cdoAnonymous = 0 'Do not authenticate
''Public Const cdoBasic = 1 'basic (clear-text) authentication
''Public Const cdoNTLM = 2 'NTLM

Namespace Internals




    Public NotInheritable Class CEMailerClass
        Private Class SendingMailInfo
            'Public o As CEMailerClass = Nothing
            'Public mail As MailMessage = Nothing
            Public client As SmtpClient = Nothing
            Public autodispose As Boolean = False
            Public ar As IAsyncResult = Nothing
            Public RetryCount As Integer = 0
            Public MaxRetryCount As Integer = 3
            Public FileName As String = ""

            Public Sub New()
                DMD.DMDObject.IncreaseCounter(Me)
            End Sub

            Protected Overrides Sub Finalize()
                MyBase.Finalize()
                DMD.DMDObject.DecreaseCounter(Me)
            End Sub
        End Class



        ''' <summary>
        ''' Evento generato quando viene modificata la configurazione di questo oggetto
        ''' </summary>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event ConfigurationChanged(ByVal e As System.EventArgs)

        ''' <summary>
        ''' Evento generato quando viene completato l'invio di un messaggio (sincrono o asincrono)
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event MessageSent(ByVal sender As Object, ByVal e As MailMessageEventArgs)

        ''' <summary>
        ''' Evento generato quando si verifica un errore nell'invio del messaggio
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event SendError(ByVal sender As Object, ByVal e As MailMessageEventArgs)

        ''' <summary>
        ''' Evento generato quando viene ricevuto un messaggio
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event MessageReceived(ByVal sender As Object, ByVal e As MailMessageReceivedEventArgs)


        'Private m_ErrorCode As Integer
        Private ReadOnly configLock As New Object
        Private m_Config As CEmailConig = Nothing
        Private m_Accounts As CEmailAccounts = Nothing

        Public Const DEFAULT_INTERVAL As Integer = 1000 * 60 * 10 'Controlla le email ogni 10 minuti
        Public Const WAITMILLISEC As Integer = 250
        Public Const SUBJECTMAXLEN As Integer = 128

        Protected Friend Shared ReadOnly receiverLock As New Object
        Protected Friend Shared ReadOnly senderLock As New Object
        Private WithEvents m_ReceiveTimer As New System.Timers.Timer
        Private WithEvents m_senderTimer As New System.Timers.Timer
        Private m_Checking As Boolean = False

        Private Shared m_OutgoingMails As New CCollection(Of SendingMailInfo)





        Friend Sub New()
            Me.m_ReceiveTimer.Enabled = False
            Me.m_ReceiveTimer.Interval = DEFAULT_INTERVAL



            Me.m_senderTimer.Interval = WAITMILLISEC
            Me.m_senderTimer.Enabled = True
        End Sub

        'Public ReadOnly Property OutQueue As CCollection(Of MailMessage)
        '    Get
        '        SyncLock senderLock
        '            Dim ret As New CCollection(Of MailMessage)
        '            For Each m As SendingMailInfo In Me.Outgoing
        '                ret.Add(m.mail)
        '            Next
        '            Return ret
        '        End SyncLock
        '    End Get
        'End Property

        Public Function GetOutPath() As String
            Return Global.System.IO.Path.Combine(Sistema.ApplicationContext.SystemDataFolder, "mails\OutQueue\")
        End Function


        ''' <summary>
        ''' Restituisce o imposta l'intervallo di tempo in minuti entro cui controllare la posta in arrivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SynchronizeInterval As Integer
            Get
                Return Me.m_ReceiveTimer.Interval / (60 * 1000)
            End Get
            Set(value As Integer)
                If (value <= 0) Then value = 10
                Me.m_ReceiveTimer.Interval = value * 60 * 1000
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se sincronizzare automaticamente le emails
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AutoSynchronize As Boolean
            Get
                Return Me.m_ReceiveTimer.Enabled
            End Get
            Set(value As Boolean)
                Me.m_ReceiveTimer.Enabled = value
                'If (value) Then Me.CheckMailsAsync()
            End Set
        End Property


        ''' <summary>
        ''' Accede alla configurazione di sistema per le email
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Config As CEmailConig
            Get
                SyncLock Me.configLock
                    If Me.m_Config Is Nothing Then
                        Me.m_Config = New CEmailConig
                        If APPConn IsNot Nothing AndAlso APPConn.IsOpen Then Me.m_Config.Load()
                    End If
                    Return Me.m_Config
                End SyncLock
            End Get
        End Property


        ''' <summary>
        ''' Accede ad una collezione di accounts aggiuntivi utilizzati per scaricare la posta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property MailAccounts As CEmailAccounts
            Get
                SyncLock receiverLock
                    If (m_Accounts Is Nothing) Then
                        Me.m_Accounts = New CEmailAccounts
                        If (APPConn.IsOpen) Then Me.m_Accounts.Load()
                    End If
                    Return Me.m_Accounts
                End SyncLock
            End Get
        End Property

        Public Sub SetConfig(ByVal value As CEmailConig)
            SyncLock Me.configLock
                Me.m_Config = value
                If Me.m_DefaultSMTPClient IsNot Nothing Then
                    Me.m_DefaultSMTPClient.Dispose()
                    Me.m_DefaultSMTPClient = Nothing
                End If
            End SyncLock
            Me.doConfigChanged(New System.EventArgs)
        End Sub


        Friend Sub doConfigChanged(ByVal e As System.EventArgs)
            RaiseEvent ConfigurationChanged(e)
        End Sub



        Public Function GetAddresses(ByVal text As String, Optional ByVal separator As String = ",") As MailAddress()
            Dim targets() As String = Split(text, separator)
            Dim ret As New System.Collections.ArrayList
            For i As Integer = 0 To UBound(targets)
                ret.Add(New MailAddress(targets(i)))
            Next
            Return ret.ToArray(GetType(MailAddress))
        End Function

        ''' <summary>
        ''' Rimuove tutti i collegamenti esterni e li incorpora nel corpo del messaggio secondo la codifica MIME opportuna
        ''' </summary>
        ''' <param name="m"></param>
        ''' <remarks></remarks>
        Public Sub EmbedResources(ByVal m As MailMessage)
            Dim htmlBody As String = m.Body
            Dim pics As New CKeyCollection(Of String)
            Dim i As Integer = InStr(htmlBody, "<img ")
            Dim k As String
            Dim src As String
            While (i > 0)
                Dim j As Integer = InStr(i + 1, htmlBody, ">")
                If (j > i) Then
                    i = InStr(i, htmlBody, " src=")
                    If (i > 0) Then
                        i += 5
                        Dim ch As String = Mid(htmlBody, i, 1)
                        While (ch <> Chr(34) And ch <> "'" And i < Len(htmlBody))
                            i += 1
                            ch = Mid(htmlBody, i, 1)
                        End While
                        If (ch = Chr(34) Or ch = "'") Then
                            j = InStr(i + 1, htmlBody, ch)
                            If (j > i) Then
                                src = Mid(htmlBody, i + 1, j - i - 1)
                                If (Left(src, 4) <> "cid:") Then
                                    k = "pic" & pics.Count
                                    pics.Add(k, src)
                                    htmlBody = Replace(htmlBody, ch & src & ch, ch & "cid:" & k & ch)
                                End If
                            End If
                        End If
                    End If
                End If
                If (i > 0) Then i = InStr(i + 1, htmlBody, "<img ")
            End While
            Dim avHtml As AlternateView = Nothing
            If (Not m.IsBodyHtml) Then
                m.IsBodyHtml = True
            End If

            avHtml = AlternateView.CreateAlternateViewFromString(htmlBody, Nothing, "text/html") 'MediaTypeNames.Text.Html)
            m.AlternateViews.Add(avHtml)

            Dim keys() As String = pics.Keys
            For i = 0 To UBound(keys)
                k = keys(i)
                Dim req As System.Net.WebRequest = Nothing
                Dim stream As System.IO.Stream = Nothing
                Dim mStream As System.IO.MemoryStream = Nothing

                Try
                    Dim url As String = Strings.LCase(pics(k))

                    req = System.Net.HttpWebRequest.Create(url)
                    stream = req.GetResponse().GetResponseStream()
                    mStream = New System.IO.MemoryStream
                    FileSystem.CopyStream(stream, mStream)
                    stream.Close()
                    req = Nothing

                    Dim pic1 As LinkedResource
                    mStream.Position = 0
                    If (url.EndsWith(".jpg", StringComparison.InvariantCultureIgnoreCase)) Then
                        pic1 = New LinkedResource(mStream, MediaTypeNames.Image.Jpeg)
                    ElseIf (url.EndsWith(".gif", StringComparison.InvariantCultureIgnoreCase)) Then
                        pic1 = New LinkedResource(mStream, MediaTypeNames.Image.Gif)
                    ElseIf (url.EndsWith(".tiff", StringComparison.InvariantCultureIgnoreCase)) Then
                        pic1 = New LinkedResource(mStream, MediaTypeNames.Image.Tiff)
                    Else
                        pic1 = New LinkedResource(mStream)
                    End If

                    pic1.ContentId = k
                    avHtml.LinkedResources.Add(pic1)
                    ' stream.Dispose()
                Catch ex As Exception
                    Sistema.ApplicationContext.Log("Errore nell'inclusione dell'immagine nella email: " & m.Subject & vbNewLine & m.To.ToString & vbNewLine & k & vbNewLine & pics(k) & vbNewLine & ex.Message & vbNewLine)
                    If (mStream IsNot Nothing) Then mStream.Dispose() : mStream = Nothing
                    Throw
                Finally
                    If (stream IsNot Nothing) Then stream.Dispose() : stream = Nothing
                    req = Nothing
                End Try
            Next
        End Sub

        Public Function SendEMail1(
                        ByVal fromAddress As String,
                        ByVal toAddresses As String,
                        ByVal ccAddresses As String,
                        ByVal ccnAddresses As String,
                        ByVal subject As String,
                        ByVal body As String
                        ) As Boolean
            Return SendEMail1(fromAddress, toAddresses, ccAddresses, ccnAddresses, subject, body, EMailer.Config.SMTPDisplayName, False)
        End Function

        Public Function AddAttachments(ByVal m As System.Net.Mail.MailMessage, ByVal fileName As String) As System.Net.Mail.Attachment
            Dim a As New AttachmentEx(fileName)
            m.Attachments.Add(a)
            Return a
        End Function

        Public Function PrepareMessage(ByVal fromAddress As String,
                        ByVal toAddresses As String,
                        ByVal ccAddresses As String,
                        ByVal ccnAddresses As String,
                        ByVal subject As String,
                        ByVal body As String,
                        ByVal fromAddressDisplayName As String,
                        ByVal embedResource As Boolean) As MailMessageEx
            Dim mittente As MailAddressEx
            Dim mail As MailMessageEx 'questa dichiarazione deve essere globale
            mail = Nothing
            mittente = New MailAddressEx(fromAddress, fromAddressDisplayName)
            mail = New MailMessageEx()
            mail.From = mittente
            mail.To.Add(toAddresses)
            If ccAddresses <> vbNullString Then mail.CC.Add(ccAddresses)
            If ccnAddresses <> vbNullString Then mail.Bcc.Add(ccnAddresses)
            mail.Subject = DMD.Sistema.Strings.TrimTo(subject, SUBJECTMAXLEN)
            mail.IsBodyHtml = True
            mail.Body = body
            If (embedResource) Then EmbedResources(mail)
            Return mail
        End Function

        ''' <summary>
        ''' Invia il messaggio con i parametri di invio predefiniti
        ''' </summary>
        ''' <param name="message"></param>
        ''' <remarks></remarks>
        Public Sub SendMessage(ByVal message As MailMessage)
            Me.SendMessage(message, Me.DefaultSMTPClient)
        End Sub

        Public Sub SendMessage(ByVal mail As MailMessage, ByVal client As SmtpClient)
            If (client Is Nothing) Then Throw New ArgumentNullException("client")
            If (mail Is Nothing) Then Throw New ArgumentNullException("mail")
            SyncLock client
#If Not PURGEMAILS Then
                client.Send(mail)
#End If
            End SyncLock
        End Sub

        Private Sub SendMessageWithClient2(ByVal mail As MailMessage, ByVal client As SmtpClient)
            If (client Is Nothing) Then Throw New ArgumentNullException("client")
            If (mail Is Nothing) Then Throw New ArgumentNullException("mail")
            SyncLock client
#If Not PURGEMAILS Then
                client.Send(mail)
#End If
            End SyncLock
        End Sub


        'Private Delegate Sub SendMessageFun(ByVal mail As MailMessage, ByVal client As SmtpClient)
        'Private ms2 As SendMessageFun = AddressOf SendMessageWithClient2
        Private m_DefaultSMTPClient As SmtpClient = Nothing

        Public ReadOnly Property DefaultSMTPClient As SmtpClient
            Get
                SyncLock senderLock
                    If (Me.m_DefaultSMTPClient Is Nothing) Then
                        Me.m_DefaultSMTPClient = New SmtpClient(EMailer.Config.SMTPServer, EMailer.Config.SMTPServerPort)
                        Me.m_DefaultSMTPClient.UseDefaultCredentials = True 'False
                        Me.m_DefaultSMTPClient.EnableSsl = EMailer.Config.SMTPUseSSL
                        Me.m_DefaultSMTPClient.Credentials = New System.Net.NetworkCredential(EMailer.Config.SMTPUserName, EMailer.Config.SMTPPassword)
                    End If
                    Return Me.m_DefaultSMTPClient
                End SyncLock
            End Get
        End Property

        Public Function SendMessageAsync(ByVal mail As MailMessage, Optional ByVal autoDispose As Boolean = False) As Object
            Return Me.SendMessageAsync(mail, Me.DefaultSMTPClient, autoDispose)
        End Function

        Public Function SendMessageAsync(ByVal mail As MailMessage, ByVal client As SmtpClient, Optional ByVal autoDispose As Boolean = False, Optional ByVal maxRetries As Integer = 3) As Object
            If (mail Is Nothing) Then Throw New ArgumentNullException("mail")
            If (client Is Nothing) Then Throw New ArgumentNullException("client")
            Dim path As String = Global.System.IO.Path.Combine(Sistema.ApplicationContext.SystemDataFolder, "mails\OutQueue\")
            Sistema.FileSystem.CreateRecursiveFolder(path)

            Dim h As New SendingMailInfo
            'h.o = Me
            'h.mail = mail
            h.client = client
            h.autodispose = autoDispose
            h.MaxRetryCount = maxRetries
            h.FileName = path & Formats.GetTimeStamp() & ".msg"
            Dim m As SerializeableMailMessage
            m = New SerializeableMailMessage(mail)
            Using fs As New FileStream(h.FileName, FileMode.Create)
                Dim formatter As New BinaryFormatter()
                formatter.Serialize(fs, m)
            End Using

            SyncLock senderLock
                m_OutgoingMails.Add(h)
            End SyncLock

            Return h
        End Function

        'Private Sub handleSendCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.AsyncCompletedEventArgs)
        '    Try
        '        Dim h As SendingMailInfo = e.UserState

        '        Dim e1 As New MailMessageEventArgs(h.m, e.Cancelled, e.Error)
        '        Me.OnMessageSent(e1)

        '        If (h.a) Then Me.DisposeMessage(h.m)
        '    Catch ex As Exception
        '        Sistema.Events.NotifyUnhandledException(ex)
        '    End Try
        'End Sub


        'Private Sub handleSendCompletedInternal(ByVal e As IAsyncResult)
        '    Try
        '        ms2.EndInvoke(e)

        '        Dim h As SendingMailInfo = e.AsyncState

        '        Dim e1 As New MailMessageEventArgs(h.m, e.IsCompleted = False, Nothing)
        '        Me.OnMessageSent(e1)

        '        If (h.a) Then Me.DisposeMessage(h.m)
        '    Catch ex As Exception
        '        Sistema.Events.NotifyUnhandledException(ex)
        '    End Try
        'End Sub


        Private Sub OnMessageSent(ByVal e As MailMessageEventArgs)
            RaiseEvent MessageSent(Me, e)
        End Sub

        Public Sub DisposeMessage(ByVal message As MailMessage)
            For i As Integer = 0 To message.AlternateViews.Count - 1
                Dim av As AlternateView = message.AlternateViews(i)
                For j As Integer = 0 To av.LinkedResources.Count - 1
                    Dim l As LinkedResource = av.LinkedResources(j)
                    l.ContentStream.Close()
                    l.Dispose()
                Next
            Next
            message.AlternateViews.Clear()

            For i As Integer = 0 To message.Attachments.Count - 1
                message.Attachments(i).ContentStream.Dispose()
                message.Attachments(i).Dispose()
            Next
            message.Attachments.Clear()
            message.Dispose()
        End Sub

        Public Function SendEMail1(
                        ByVal fromAddress As String,
                        ByVal toAddresses As String,
                        ByVal ccAddresses As String,
                        ByVal ccnAddresses As String,
                        ByVal subject As String,
                        ByVal body As String,
                        ByVal fromAddressDisplayName As String,
                        ByVal embedResource As Boolean
                        ) As Boolean
            Dim mail As MailMessage = Nothing
            Try
                mail = PrepareMessage(fromAddress, toAddresses, ccAddresses, ccnAddresses, subject, body, fromAddressDisplayName, embedResource)
                SendMessage(mail)
                Return True
            Catch ex As SmtpException
                Sistema.Events.NotifyUnhandledException(ex)
                Return False
            Finally
                If mail IsNot Nothing Then
                    'mail.To.Clear()
                    'If (embedResource) Then
                    DisposeMessage(mail)
                    'Else
                    'mail.Attachments.Clear()
                    'mail.Dispose()
                    'End If
                End If
            End Try
        End Function

        Public Function GetPriorityIcon(ByVal value As String) As String
            Dim mp As String = UCase(Trim(value))
            If mp = "0" Or mp = "1" Or mp = "2" Or mp = "HIGH" Then
                Return "/widgets/images/priority_high.gif"
            ElseIf mp = "4" Or mp = "5" Or mp = "6" Or mp = "LOW" Then
                Return "/widgets/images/priority_low.gif"
            Else
                Return "/widgets/images/priority_normal.gif"
            End If
        End Function

        Public Function IsValidAddress(ByVal value As String) As Boolean
            Dim i As Integer
            Dim user As String = vbNullString
            Dim server As String = vbNullString
            Dim suffix As String = vbNullString

            value = Trim(value)

            If (value = "") Then Return False

            i = Strings.IndexOf(value, "@")
            If (i < 1) Then Return False
            If (i > 1) Then user = Left(value, i - 1)
            If (i < Len(value)) Then server = Mid(value, i + 1)

            user = Trim(user)
            If (user = "") Then Return False

            server = Trim(server)
            If (server = "") Then Return False

            i = Strings.IndexOfRev(server, ".")
            If (i < 1) Then Return False

            If (i < Len(server) - 1) Then suffix = Mid(server, i + 1)
            If (i > 1) Then server = Left(server, i - 1)

            server = Trim(server)
            If (server = "") Then Return False

            suffix = Trim(suffix)
            If (suffix = "") Then Return False

            Return True
        End Function


        Friend Sub OnEmailReceived(ByVal e As MailMessageReceivedEventArgs)
            RaiseEvent MessageReceived(Me, e)
        End Sub

        'Private Delegate Sub CheckMailsDelegate()

        'Private m_Addr As CheckMailsDelegate = Nothing
        Private checkMailsThread As System.Threading.Thread = Nothing

        Public Sub CheckMailsAsync()
            Try
                If Me.checkMailsThread IsNot Nothing Then Return
                Dim wc As New WaitCallback(AddressOf Me.CheckMailsEnter)
                If (Not ThreadPool.QueueUserWorkItem(wc, Me)) Then
                    Throw New Exception
                End If

                'Me.checkMailsThread = New System.Threading.Thread(AddressOf Me.CheckMails)
                'checkMailsThread.Priority = ThreadPriority.BelowNormal
                'checkMailsThread.Start()
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
            End Try

        End Sub



        'Private Sub CheckMailsAsyncComplete(ByVal res As IAsyncResult)
        '    Try
        '        Me.m_Addr.EndInvoke(res)
        '    Catch ex As Exception
        '        Sistema.Events.NotifyUnhandledException(ex)
        '    End Try
        '    Me.m_Addr = Nothing
        'End Sub

        Private Sub CheckMailsEnter(ByVal o As Object)
            Try
                Me.CheckMails()
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
            End Try
        End Sub

        ''' <summary>
        ''' Effettua la verifica delle email ricevute sul server
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub CheckMails()
            Dim messagesToDispose As New CCollection(Of MailMessage)

#If Not DEBUG Then
            SyncLock receiverLock
#End If
            Me.m_Checking = True

                'Controlliamo l'account predefinito
                Dim client As DMD.Net.Mail.Pop3Client = Nothing
#If Not DEBUG Then
                Try
#End If
                Dim message As DMD.Net.Mail.MailMessageEx
                    Dim list As System.Collections.Generic.List(Of DMD.Net.Mail.Protocols.POP3.Pop3ListItem)
                    Dim item As DMD.Net.Mail.Protocols.POP3.Pop3ListItem

                    client = New DMD.Net.Mail.Pop3Client(Me.Config.POPUserName, Me.Config.POPPassword, Me.Config.POPServer, Me.Config.POPPort, Me.Config.POPUseSSL)

                    'Dim messagesToDelete As New CCollection

                    Dim oraInizio As Date = Calendar.Now

                    client.CustomCerfificateValidation = Not Me.Config.SMTPValidateCertificate
                    client.Connect()
                    client.Authenticate()
                    client.Stat()
                    'Recuperiamo la data dell'ultimo messaggio scaricato tramite questo account
                    'Scarichiamo i messaggi successivi
                    list = client.List

                    For i As Integer = list.Count - 1 To 0 Step -1
                        item = list(i)
                        message = client.Top(item.MessageId, 10)
                        Dim delOnServer As Boolean = False
                        If (Me.IsNewMessage(message)) Then
                            message = client.RetrMailMessageEx(item)
                            Dim e As New MailMessageReceivedEventArgs(message)
                            Dim exp As System.Exception = Nothing
                            Try
                                Me.OnEmailReceived(e)
                            Catch ex As Exception
                                exp = ex
                            End Try

                            If (exp Is Nothing) Then
                                Me.SaveMessage(message)
                                delOnServer = e.DeleteOnServer
                            End If
                        End If

                        If (delOnServer OrElse
                            (Me.Config.RemoveFromServerAfterDownload AndAlso Calendar.DateDiff(DateInterval.Day, message.DeliveryDate, Calendar.ToDay) > Me.Config.RemoveFromServerAfterNDays)
                            ) Then
                            'messagesToDelete.Add(message)
                            client.Dele(item)
                        End If
                        messagesToDispose.Add(message)

                    Next

                    'For Each message In messagesToDelete
                    '    client.Dele(message)
                    'Next

                    client.Quit()
                    client.Disconnect()
                    client = Nothing

                    Me.Config.LastSync = oraInizio
                    If APPConn IsNot Nothing AndAlso APPConn.IsOpen Then Me.Config.Save()

                'Controlliamo tutti gli altri account
                For Each account As CEmailAccount In Me.MailAccounts
                    If account.Attivo Then
                        oraInizio = Calendar.Now
                        ' messagesToDelete = New CCollection

                        client = New DMD.Net.Mail.Pop3Client(Me.Config.POPUserName, Me.Config.POPPassword, Me.Config.POPServer, Me.Config.POPPort, Me.Config.POPUseSSL)
                        'Dim ret As New CCollection(Of MailMessage)
                        client.Connect()
                        client.Authenticate()
                        client.Stat()
                        'Recuperiamo la data dell'ultimo messaggio scaricato tramite questo account
                        'Scarichiamo i messaggi successivi
                        list = client.List
                        For i As Integer = list.Count - 1 To 0 Step -1
                            item = list(i)
                            message = client.Top(item.MessageId, 5)
                            Dim delOnServer As Boolean = False
                            If (Me.IsNewMessage(message, account)) Then
                                message = client.RetrMailMessageEx(item)
                                Dim e As New MailMessageReceivedEventArgs(account, message)
                                Dim exp As System.Exception = Nothing
                                Try
                                    Me.OnEmailReceived(e)
                                Catch ex As Exception
                                    exp = ex
                                End Try
                                If (exp Is Nothing) Then
                                    Me.SaveMessage(message)
                                    delOnServer = e.DeleteOnServer
                                End If
                                If (delOnServer) Then
                                    Debug.Print("Preparo il messaggio da eliminare: " & message.MessageId & " " & message.DeliveryDate & " (" & Calendar.DateDiff(DateInterval.Day, message.DeliveryDate, Calendar.ToDay) & ")")
                                    client.Dele(item) 'messagesToDelete.Add(message)
                                End If
                            End If

                            messagesToDispose.Add(message)
                        Next

                        'For Each message In messagesToDelete
                        '    client.Dele(message)
                        'Next

                        client.Quit()
                        client.Disconnect()
                        client.Dispose()
                        client = Nothing

                        account.LastSync = oraInizio
                        If (account.ID <> 0) Then account.Save()

                    End If
                Next
#If Not DEBUG Then
                Catch ex As Exception
                    Sistema.Events.NotifyUnhandledException(ex)
                    ' Throw
                Finally
#End If
                If (client IsNot Nothing) Then
                        Try
                            client.Quit()
                            client.Disconnect()
                            client.Dispose()
                        Catch ex As Exception
                            Try
                                Sistema.Events.NotifyUnhandledException(ex)
                            Catch ex1 As Exception

                            End Try
                        End Try
                        client = Nothing
                    End If
                    Me.m_Checking = False
                Me.checkMailsThread = Nothing
#If Not DEBUG Then
                End Try
#End If
#If Not DEBUG Then
            End SyncLock
#End If

                For Each m As MailMessage In messagesToDispose
                m.Dispose()
            Next
        End Sub

        Private Function GetDelDateStr(ByVal d As Date) As String
            Return Right("0000" & Year(d), 4) & Right("00" & Month(d), 2) & Right("00" & Day(d), 2) & Right("00" & Hour(d), 2) & Right("00" & Minute(d), 2) & Right("00" & Second(d), 2)
        End Function

        Private Function GetTargetName(ByVal message As DMD.Net.Mail.MailMessageEx, Optional ByVal account As CEmailAccount = Nothing) As String
            Dim path As String = Global.System.IO.Path.Combine(ApplicationContext.SystemDataFolder, "Mail")
            If (account IsNot Nothing) Then path = Global.System.IO.Path.Combine(path, account.AccountName)
            Dim strID As String = Me.GetDelDateStr(message.DeliveryDate) & "_" & message.MessageId
            strID = DMD.Sistema.FileSystem.RemoveSpecialChars(strID)
            Return Global.System.IO.Path.Combine(path, strID & "\message.xml")
        End Function

        Public Function IsNewMessage(ByVal message As DMD.Net.Mail.MailMessageEx, Optional ByVal account As CEmailAccount = Nothing) As Boolean
            'Return Not Global.System.IO.File.Exists(Me.GetTargetName(message, account))
            If (account Is Nothing) Then
                'Return ((Me.Config.LastSync.HasValue = False) OrElse (Calendar.DateDiff(DateInterval.Minute, Me.Config.LastSync.Value, message.DeliveryDate) > -5)) AndAlso _
                '   Not Global.System.IO.File.Exists(Me.GetTargetName(message, account))
                Return Not Global.System.IO.File.Exists(Me.GetTargetName(message, account))
            Else
                'Return ((account.LastSync.HasValue = False) OrElse (Calendar.DateDiff(DateInterval.Minute, account.LastSync.Value, message.DeliveryDate) > -5)) AndAlso _
                '    Not Global.System.IO.File.Exists(Me.GetTargetName(message, account))
                Return Not Global.System.IO.File.Exists(Me.GetTargetName(message, account))
            End If
        End Function



        Public Function SaveMessage(ByVal message As DMD.Net.Mail.MailMessageEx, Optional ByVal account As CEmailAccount = Nothing) As String
            Dim targetName As String = Me.GetTargetName(message, account)
            Dim folder As String = FileSystem.GetFolderName(targetName)
            Dim path As String

            FileSystem.CreateRecursiveFolder(folder)

            Dim text As String = XML.Utils.Serializer.Serialize(message)
            FileSystem.CreateTextFile(targetName, text)

            Dim i As Integer = 0
            For Each a As AttachmentEx In message.Attachments
                path = Global.System.IO.Path.Combine(folder, "Attachments")
                FileSystem.CreateRecursiveFolder(path)

                text = XML.Utils.Serializer.Serialize(a)
                FileSystem.CreateTextFile(System.IO.Path.Combine(path, i & ".xml"), text)
                a.SaveToFile(System.IO.Path.Combine(path, i & ".dat"))
                i += 1
            Next

            For Each a As AlternateViewEx In message.AlternateViews
                path = Global.System.IO.Path.Combine(folder, "AlternateViews")
                FileSystem.CreateRecursiveFolder(path)

                text = XML.Utils.Serializer.Serialize(a)
                FileSystem.CreateTextFile(System.IO.Path.Combine(path, i & ".xml"), text)
                a.SaveToFile(System.IO.Path.Combine(path, i & ".dat"))
                i += 1

                Dim j As Integer = 0
                For Each l As LinkedResourceEx In a.LinkedResources
                    path = Global.System.IO.Path.Combine(path, "LinkedResources")
                    FileSystem.CreateRecursiveFolder(path)

                    text = XML.Utils.Serializer.Serialize(l)
                    FileSystem.CreateTextFile(System.IO.Path.Combine(path, j & ".xml"), text)
                    l.SaveToFile(System.IO.Path.Combine(path, j & ".dat"))
                    j += 1
                Next
            Next

            Return targetName
        End Function

        Private Sub m_Timer_Elapsed(sender As Object, e As Timers.ElapsedEventArgs) Handles m_ReceiveTimer.Elapsed
            Me.CheckMailsAsync()
        End Sub

        'Private Sub sendTimer_Elapsed(sender As Object, e As Timers.ElapsedEventArgs) Handles sendTimer.Elapsed
        'SyncLock Me.sendingMails
        '    Me.slots = Me.Config.SMTPLimitOutSent
        '    Dim cnt As Integer = 0
        '    While (Me.sendingMails.Count > 0) AndAlso (slots <= 0 OrElse cnt <= slots)
        '        Dim m As SendingMailInfo = Me.sendingMails(0)
        '        ms.BeginInvoke(m.m, m.c, AddressOf handleSendCompleted1, m)
        '        Me.sendingMails.RemoveAt(0)
        '    End While
        'End SyncLock

        'End Sub

        Friend Sub doMailSent(ByVal e As MailMessageEventArgs)
            RaiseEvent MessageSent(Me, e)
        End Sub

        Friend Sub doSendError(ByVal e As MailMessageEventArgs)
            RaiseEvent SendError(Me, e)
        End Sub

        Private Sub m_senderTimer_Elapsed(sender As Object, e As ElapsedEventArgs) Handles m_senderTimer.Elapsed
            Dim item As SendingMailInfo = Nothing

            SyncLock senderLock
                If m_OutgoingMails.Count > 0 Then
                    item = m_OutgoingMails(0)
                    m_OutgoingMails.RemoveAt(0)
                End If
            End SyncLock

            If (item IsNot Nothing) Then
                Dim exp As System.Exception = Nothing
                Dim mail As MailMessage = Nothing
                Using fs As New FileStream(item.FileName, FileMode.Open)
                    Dim formatter As New BinaryFormatter()
                    Dim sm As SerializeableMailMessage = formatter.Deserialize(fs)
                    mail = sm.GetMailMessage
                End Using

                Try
                    SendMessage(mail)
                Catch ex As Exception
                    exp = ex
                End Try

                If (exp Is Nothing) Then
                    Try
                        doMailSent(New MailMessageEventArgs(mail))
                    Catch ex As Exception
                        Sistema.Events.NotifyUnhandledException(ex)
                    End Try
                    Try
                        Sistema.EMailer.DisposeMessage(mail)
                    Catch ex As Exception
                        Sistema.Events.NotifyUnhandledException(ex)
                    End Try

                    Try
                        Sistema.FileSystem.DeleteFile(item.FileName)
                    Catch ex As Exception
                        Sistema.Events.NotifyUnhandledException(ex)
                    End Try
                Else
                    item.RetryCount += 1
                    If (item.RetryCount < item.MaxRetryCount) Then
                        SyncLock senderLock
                            m_OutgoingMails.Add(item)
                        End SyncLock
                    Else
                        Try
                            doSendError(New MailMessageEventArgs(mail))
                        Catch ex As Exception
                            Sistema.Events.NotifyUnhandledException(ex)
                        End Try

                        Try
                            Sistema.EMailer.DisposeMessage(mail)
                        Catch ex As Exception
                            Sistema.Events.NotifyUnhandledException(ex)
                        End Try

                        Try
                            Sistema.FileSystem.DeleteFile(item.FileName)
                        Catch ex As Exception
                            Sistema.Events.NotifyUnhandledException(ex)
                        End Try
                    End If
                End If
            End If
        End Sub


    End Class



#Region "serializable mail message"

    '        Using System;
    'Using System.Collections.Generic;
    'Using System.Text;
    'Using System.Text.RegularExpressions;
    'Using System.Net;
    'Using System.IO;
    'Using System.Net.Mime;
    'Using System.Collections.Specialized;
    'Using System.Net.Mail;

    'Namespace Mail.Serialization
    '{
    <Serializable>
    Friend Class SerializeableLinkedResource
        Public ContentId As String
        Public ContentLink As Uri
        Public ContentStream As Stream
        Public ContentType As SerializeableContentType
        Public TransferEncoding As TransferEncoding

        Friend Shared Function GetSerializeableLinkedResource(ByVal lr As LinkedResource) As SerializeableLinkedResource
            If (lr Is Nothing) Then Return Nothing
            Dim slr As New SerializeableLinkedResource()
            slr.ContentId = lr.ContentId
            slr.ContentLink = lr.ContentLink

            If (lr.ContentStream IsNot Nothing) Then
                Dim bytes As Byte() = Array.CreateInstance(GetType(Byte), lr.ContentStream.Length)
                lr.ContentStream.Read(bytes, 0, bytes.Length)
                slr.ContentStream = New MemoryStream(bytes)
            End If
            slr.ContentType = SerializeableContentType.GetSerializeableContentType(lr.ContentType)
            slr.TransferEncoding = lr.TransferEncoding
            Return slr
        End Function

        Friend Function GetLinkedResource() As LinkedResource
            Dim slr As New LinkedResource(ContentStream)
            slr.ContentId = ContentId
            slr.ContentLink = ContentLink

            slr.ContentType = ContentType.GetContentType()
            slr.TransferEncoding = TransferEncoding

            Return slr
        End Function
    End Class

    <Serializable>
    Friend Class SerializeableAlternateView
        Public BaseUri As Uri
        Public ContentId As String
        Public ContentStream As Stream
        Public ContentType As SerializeableContentType
        Public LinkedResources As New List(Of SerializeableLinkedResource)()
        Public TransferEncoding As TransferEncoding

        Friend Shared Function GetSerializeableAlternateView(ByVal av As AlternateView) As SerializeableAlternateView
            If (av Is Nothing) Then Return Nothing
            Dim sav As New SerializeableAlternateView()
            sav.BaseUri = av.BaseUri
            sav.ContentId = av.ContentId
            If (av.ContentStream IsNot Nothing) Then
                Dim bytes As Byte() = Array.CreateInstance(GetType(Byte), av.ContentStream.Length)
                av.ContentStream.Read(bytes, 0, bytes.Length)
                sav.ContentStream = New MemoryStream(bytes)
            End If

            sav.ContentType = SerializeableContentType.GetSerializeableContentType(av.ContentType)

            For Each lr As LinkedResource In av.LinkedResources
                sav.LinkedResources.Add(SerializeableLinkedResource.GetSerializeableLinkedResource(lr))
            Next

            sav.TransferEncoding = av.TransferEncoding
            Return sav
        End Function

        Friend Function GetAlternateView() As AlternateView
            Dim sav As New AlternateView(ContentStream)
            sav.BaseUri = BaseUri
            sav.ContentId = ContentId
            sav.ContentType = ContentType.GetContentType()
            For Each lr As SerializeableLinkedResource In LinkedResources
                sav.LinkedResources.Add(lr.GetLinkedResource())
            Next
            sav.TransferEncoding = TransferEncoding
            Return sav
        End Function
    End Class

    <Serializable>
    Friend Class SerializeableMailAddress
        Public User As String
        Public Host As String
        Public Address As String
        Public DisplayName As String

        Friend Shared Function GetSerializeableMailAddress(ByVal ma As System.Net.Mail.MailAddress) As SerializeableMailAddress
            If (ma Is Nothing) Then Return Nothing
            Dim sma As New SerializeableMailAddress()
            sma.User = ma.User
            sma.Host = ma.Host
            sma.Address = ma.Address
            sma.DisplayName = ma.DisplayName
            Return sma
        End Function

        Friend Function GetMailAddress() As System.Net.Mail.MailAddress
            Return New MailAddress(Address, DisplayName)
        End Function
    End Class

    <Serializable>
    Friend Class SerializeableContentDisposition
        Public CreationDate As DateTime
        Public DispositionType As String
        Public FileName As String
        Public Inline As Boolean
        Public ModificationDate As DateTime
        Public Parameters As SerializeableCollection
        Public ReadDate As DateTime
        Public Size As Long

        Friend Shared Function GetSerializeableContentDisposition(ByVal cd As System.Net.Mime.ContentDisposition) As SerializeableContentDisposition
            If (cd Is Nothing) Then Return Nothing
            Dim scd As New SerializeableContentDisposition()
            scd.CreationDate = cd.CreationDate
            scd.DispositionType = cd.DispositionType
            scd.FileName = cd.FileName
            scd.Inline = cd.Inline
            scd.ModificationDate = cd.ModificationDate
            scd.Parameters = SerializeableCollection.GetSerializeableCollection(cd.Parameters)
            scd.ReadDate = cd.ReadDate
            scd.Size = cd.Size
            Return scd
        End Function

        Friend Sub SetContentDisposition(ByVal scd As ContentDisposition)
            scd.CreationDate = CreationDate
            scd.DispositionType = DispositionType
            scd.FileName = FileName
            scd.Inline = Inline
            scd.ModificationDate = ModificationDate
            Parameters.SetColletion(scd.Parameters)

            scd.ReadDate = ReadDate
            scd.Size = Size
        End Sub
    End Class

    <Serializable>
    Friend Class SerializeableContentType
        Public Boundary As String
        Public CharSet As String
        Public MediaType As String
        Public Name As String
        Public Parameters As SerializeableCollection

        Friend Shared Function GetSerializeableContentType(ByVal ct As System.Net.Mime.ContentType) As SerializeableContentType
            If (ct Is Nothing) Then Return Nothing
            Dim sct As New SerializeableContentType()
            sct.Boundary = ct.Boundary
            sct.CharSet = ct.CharSet
            sct.MediaType = ct.MediaType
            sct.Name = ct.Name
            sct.Parameters = SerializeableCollection.GetSerializeableCollection(ct.Parameters)
            Return sct
        End Function

        Friend Function GetContentType() As ContentType
            Dim sct As New ContentType()
            sct.Boundary = Boundary
            sct.CharSet = CharSet
            sct.MediaType = MediaType
            sct.Name = Name
            Parameters.SetColletion(sct.Parameters)
            Return sct
        End Function
    End Class

    <Serializable>
    Friend Class SerializeableAttachment
        Public ContentId As String
        Public ContentDisposition As SerializeableContentDisposition
        Public ContentType As SerializeableContentType
        Public ContentStream As Stream
        Public TransferEncoding As System.Net.Mime.TransferEncoding
        Public Name As String
        Public NameEncoding As System.Text.Encoding

        Friend Shared Function GetSerializeableAttachment(ByVal att As Attachment) As SerializeableAttachment
            If (att Is Nothing) Then Return Nothing
            Dim saa As New SerializeableAttachment()
            saa.ContentId = att.ContentId
            saa.ContentDisposition = SerializeableContentDisposition.GetSerializeableContentDisposition(att.ContentDisposition)
            If (att.ContentStream IsNot Nothing) Then
                Dim bytes As Byte() = Array.CreateInstance(GetType(Byte), att.ContentStream.Length)
                att.ContentStream.Read(bytes, 0, bytes.Length)
                saa.ContentStream = New MemoryStream(bytes)
            End If
            saa.ContentType = SerializeableContentType.GetSerializeableContentType(att.ContentType)
            saa.Name = att.Name
            saa.TransferEncoding = att.TransferEncoding
            saa.NameEncoding = att.NameEncoding
            Return saa
        End Function

        Friend Function GetAttachment() As Attachment
            Dim saa As New Attachment(ContentStream, Name)
            saa.ContentId = ContentId
            Me.ContentDisposition.SetContentDisposition(saa.ContentDisposition)
            saa.ContentType = ContentType.GetContentType()
            saa.Name = Name
            saa.TransferEncoding = TransferEncoding
            saa.NameEncoding = NameEncoding
            Return saa
        End Function
    End Class

    <Serializable>
    Friend Class SerializeableCollection
        Dim Collection As New Dictionary(Of String, String)()

        Friend Sub New()

        End Sub

        Friend Shared Function GetSerializeableCollection(ByVal col As NameValueCollection) As SerializeableCollection
            If (col Is Nothing) Then Return Nothing
            Dim scol As New SerializeableCollection()
            For Each key As String In col.Keys
                scol.Collection.Add(key, col(key))
            Next
            Return scol
        End Function

        Friend Shared Function GetSerializeableCollection(ByVal col As StringDictionary) As SerializeableCollection
            If (col Is Nothing) Then Return Nothing
            Dim scol As New SerializeableCollection()
            For Each key As String In col.Keys
                scol.Collection.Add(key, col(key))
            Next
            Return scol
        End Function

        Friend Sub SetColletion(ByVal scol As NameValueCollection)
            For Each key As String In Collection.Keys
                scol.Add(key, Me.Collection(key))
            Next
        End Sub

        Friend Sub SetColletion(ByVal scol As StringDictionary)
            For Each key As String In Collection.Keys
                If (scol.ContainsKey(key)) Then
                    scol(key) = Collection(key)
                Else
                    scol.Add(key, Me.Collection(key))
                End If
            Next
        End Sub
    End Class

    ' Serializeable mailmessage object
    <Serializable>
    Friend Class SerializeableMailMessage
        Public IsBodyHtml As Boolean
        Public Body As String
        Public From As SerializeableMailAddress
        Public [To] As New List(Of SerializeableMailAddress)()
        Public CC As New List(Of SerializeableMailAddress)()
        Public Bcc As New List(Of SerializeableMailAddress)()
        Public ReplyTo As SerializeableMailAddress
        Public Sender As SerializeableMailAddress
        Public Subject As String
        Public Attachments As New List(Of SerializeableAttachment)()
        Public BodyEncoding As System.Text.Encoding
        Public SubjectEncoding As System.Text.Encoding
        Public DeliveryNotificationOptions As System.Net.Mail.DeliveryNotificationOptions
        Public Headers As SerializeableCollection
        Public Priority As MailPriority
        Public AlternateViews As New List(Of SerializeableAlternateView)()

        ' Creates a New serializeable mailmessage based on a MailMessage object
        Public Sub New(ByVal mm As System.Net.Mail.MailMessage)
            IsBodyHtml = mm.IsBodyHtml
            Body = mm.Body
            Subject = mm.Subject
            From = SerializeableMailAddress.GetSerializeableMailAddress(mm.From)
            [To] = New List(Of SerializeableMailAddress)()
            For Each ma As System.Net.Mail.MailAddress In mm.To
                [To].Add(SerializeableMailAddress.GetSerializeableMailAddress(ma))
            Next

            CC = New List(Of SerializeableMailAddress)()
            For Each ma As System.Net.Mail.MailAddress In mm.CC
                CC.Add(SerializeableMailAddress.GetSerializeableMailAddress(ma))
            Next

            Bcc = New List(Of SerializeableMailAddress)()
            For Each ma As System.Net.Mail.MailAddress In mm.Bcc
                Bcc.Add(SerializeableMailAddress.GetSerializeableMailAddress(ma))
            Next

            Attachments = New List(Of SerializeableAttachment)()
            For Each att As Attachment In mm.Attachments
                Attachments.Add(SerializeableAttachment.GetSerializeableAttachment(att))
            Next

            BodyEncoding = mm.BodyEncoding

            DeliveryNotificationOptions = mm.DeliveryNotificationOptions
            Headers = SerializeableCollection.GetSerializeableCollection(mm.Headers)
            Priority = mm.Priority
            ReplyTo = SerializeableMailAddress.GetSerializeableMailAddress(mm.ReplyTo)
            Sender = SerializeableMailAddress.GetSerializeableMailAddress(mm.Sender)
            SubjectEncoding = mm.SubjectEncoding

            For Each av As AlternateView In mm.AlternateViews
                AlternateViews.Add(SerializeableAlternateView.GetSerializeableAlternateView(av))
            Next
        End Sub

        Public Sub New()
        End Sub


        ' Returns the MailMessge object from the serializeable object
        Public Function GetMailMessage() As MailMessage
            Dim mm As New System.Net.Mail.MailMessage()
            mm.IsBodyHtml = IsBodyHtml
            mm.Body = Body
            mm.Subject = Subject
            If (From IsNot Nothing) Then mm.From = From.GetMailAddress()
            For Each ma As SerializeableMailAddress In [To]
                mm.To.Add(ma.GetMailAddress())
            Next
            For Each ma As SerializeableMailAddress In CC
                mm.CC.Add(ma.GetMailAddress())
            Next
            For Each ma As SerializeableMailAddress In Bcc
                mm.Bcc.Add(ma.GetMailAddress())
            Next
            For Each att As SerializeableAttachment In Attachments
                mm.Attachments.Add(att.GetAttachment())
            Next
            mm.BodyEncoding = BodyEncoding
            mm.DeliveryNotificationOptions = DeliveryNotificationOptions
            Headers.SetColletion(mm.Headers)
            mm.Priority = Priority
            If (ReplyTo IsNot Nothing) Then mm.ReplyTo = ReplyTo.GetMailAddress()
            If (Sender IsNot Nothing) Then mm.Sender = Sender.GetMailAddress()
            mm.SubjectEncoding = SubjectEncoding
            For Each av As SerializeableAlternateView In AlternateViews
                mm.AlternateViews.Add(av.GetAlternateView())
            Next
            Return mm
        End Function
    End Class

#End Region
End Namespace

Partial Public Class Sistema

    Private Shared m_Emailer As CEMailerClass = Nothing

    Public Shared ReadOnly Property EMailer As CEMailerClass
        Get
            If (m_Emailer Is Nothing) Then m_Emailer = New CEMailerClass
            Return m_Emailer
        End Get
    End Property



End Class
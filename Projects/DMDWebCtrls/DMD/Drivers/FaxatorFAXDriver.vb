Imports DMD
Imports DMD.Sistema

Namespace Drivers

    Public Class FaxatorFaxConfiguration
        Inherits FaxDriverOptions

       
        Public Sub New()
        End Sub

        Public Property FaxGratis As Boolean
            Get
                Return Me.GetValueBool("FaxGratis", True)
            End Get
            Set(value As Boolean)
                Me.SetValueBool("FaxGratis", value)
            End Set
        End Property

        Public Property UseDefaultSMTP As Boolean
            Get
                Return Me.GetValueBool("UseDefaultSMTP", True)
            End Get
            Set(value As Boolean)
                Me.SetValueBool("UseDefaultSMTP", value)
            End Set
        End Property

        Public Property SMTPServer As String
            Get
                Return Me.GetValueString("SMTPServer", "localhost")
            End Get
            Set(value As String)
                Me.SetValueString("SMTPServer", Strings.Trim(value))
            End Set
        End Property

        Public Property SMTPPort As Integer
            Get
                Return Me.GetValueInt("SMTPPort", 25)
            End Get
            Set(value As Integer)
                Me.SetValueInt("SMTPPort", value)
            End Set
        End Property

        Public Property SMTPUserName As String
            Get
                Return Me.GetValueString("SMTPUserName", "")
            End Get
            Set(value As String)
                Me.SetValueString("SMTPUserName", Strings.Trim(value))
            End Set
        End Property

        Public Property SMTPPassword As String
            Get
                Return Me.GetValueString("SMTPPassword", "")
            End Get
            Set(value As String)
                Me.SetValueString("SMTPPassword", value)
            End Set
        End Property

        Public Property SMTPUseSSL As Boolean
            Get
                Return Me.GetValueBool("SMTPUseSSL", False)
            End Get
            Set(value As Boolean)
                Me.SetValueBool("SMTPUseSSL", value)
            End Set
        End Property

        Public Property SMTPBypassCertificateValidation As Boolean
            Get
                Return Me.GetValueBool("SMTPBypassCertificateValidation", False)
            End Get
            Set(value As Boolean)
                Me.SetValueBool("SMTPBypassCertificateValidation", value)
            End Set
        End Property

        Public Property UserCertificateURL As String
            Get
                Return Me.GetValueString("UserCertificateURL", "")
            End Get
            Set(value As String)
                Me.GetValueString("UserCertificateURL", value)
            End Set
        End Property


    End Class

    Public Class FaxatorFaxDriver
        Inherits BaseFaxDriver

        Private Const FXTTIMEDIFF As Integer = -2        'Faxator riporta orari 2 ore indietro

        
        Public Sub New()
        End Sub

        Public Shadows ReadOnly Property Config As FaxatorFaxConfiguration
            Get
                Return MyBase.Config
            End Get
        End Property

        Protected Overrides Sub CancelJobInternal(jobID As String)
            Throw New NotSupportedException
        End Sub

        Public Overrides ReadOnly Property Description As String
            Get
                Return "Faxator Fax Driver"
            End Get
        End Property

        Protected Overrides Function InstantiateNewOptions() As DriverOptions
            Return New FaxatorFaxConfiguration
        End Function

        Public Overrides Function GetUniqueID() As String
            Return "FXTRFXDRV"
        End Function

      


       

        Protected Overrides Sub InternalSend(job As FaxJob)
            Dim toAddress As String = IIf(Me.Config.FaxGratis, """faxgratis@faxator.com"" <faxgratis@faxator.com>", """<fax@faxator.com>"" <fax@faxator.com>")
            Dim fromAddress As String = Me.Config.SMTPUserName
            Dim m As System.Net.Mail.MailMessage = EMailer.PrepareMessage(fromAddress, toAddress, "", "", job.Options.TargetNumber, "", fromAddress, False)
            m.Headers.Add("Content-Language", "it")
            m.Headers.Add("X-Mailer", "Microsoft Outlook 15.0")
            m.Headers.Add("Message-ID", job.JobID & "@faxator.com")
            Dim a As System.Net.Mail.Attachment = EMailer.AddAttachments(m, job.Options.FileName)
            a.ContentDisposition.Inline = False
            Dim a1 As System.Net.Mail.Attachment = EMailer.AddAttachments(m, ApplicationContext.MapPath(Me.Config.UserCertificateURL))
            a1.ContentDisposition.Inline = False
#If DEBUG Then
            m.Bcc.Add("tecnico@DMD.net")
#End If
            EMailer.SendMessage(m)
            m.Dispose()
            a.Dispose()
            a1.Dispose()
        End Sub


        Protected Overrides Sub InternalConnect()
            MyBase.InternalConnect()
            AddHandler EMailer.MessageReceived, AddressOf handleMessageReceived
        End Sub

        Protected Overrides Sub InternalDisconnect()
            RemoveHandler EMailer.MessageReceived, AddressOf handleMessageReceived
        End Sub

        Private Sub handleMessageReceived(ByVal sender As Object, ByVal e As DMD.Sistema.MailMessageEventArgs)
            Dim msg As System.Net.Mail.MailMessage = e.Message
            Dim text As String

            If msg.From Is Nothing OrElse msg.From.Address <> "noreply@faxator.com" Then Exit Sub


            If (msg.IsBodyHtml) Then
                If Me.Parse(msg.Subject, msg.Body) = False Then
                    For Each view As System.Net.Mail.AlternateView In msg.AlternateViews
                        'If (view.ContentType.MediaType = "text/plain" AndAlso view.ContentStream.Length > 0) Then
                        view.ContentStream.Position = 0
                        Dim reader As New System.IO.StreamReader(view.ContentStream)
                        text = reader.ReadToEnd
                        reader.Dispose()
                        If Me.Parse(msg.Subject, text) Then Exit For
                        'End If
                    Next
                End If

            Else
                text = msg.Body
                Me.Parse(msg.Subject, text)
            End If

            'msg.Dispose()
        End Sub

        Protected Overridable Function Parse(ByVal subject As String, ByVal text As String) As Boolean
            Const SPEDIZIONEACCETTATA As String = "FAXATOR - Spedizione fax accettata"
            Const NOTIFICATXERRATA As String = "NOTIFICA TX ERRATA"
            Const NOTIFICATXOK As String = "NOTIFICA TX OK"
            Const MANCAFAX As String = "FAXATOR - Manca allegato fax."
            Const MANCAREGISTRAZIONE As String = "FAXATOR - Manca registrazione"

            Dim documento As String
            Dim dataInvio As Nullable(Of Date)
            Dim numero As String
            Dim items() As String
            Dim i As Integer
            Dim p As Integer
            Dim fax As FaxJob

            text = Replace(text, "<br>", vbCr, , , CompareMethod.Text)
            text = Replace(text, "<br/>", vbCr, , , CompareMethod.Text)

            If Strings.Compare(Left(subject, Len(NOTIFICATXOK)), NOTIFICATXOK, CompareMethod.Text) = 0 Then
                'Documento: Test.pdf
                'Data di invio: 15/05/2015 17:20:04
                'Inviato al numero: 0975739900
                text = Strings.RemoveHTMLTags(text)
                text = Replace(text, vbCrLf, vbCr)
                text = Replace(text, vbLf, vbCr)
                items = Split(text, vbCr)
                documento = ""
                dataInvio = Nothing
                numero = ""

                For i = 0 To Arrays.Len(items) - 1
                    p = InStr(items(i), ":")
                    Dim name As String
                    Dim value As String
                    If (p > 0) Then
                        name = Left(items(i), p - 1)
                        value = Mid(items(i), p + 1)
                    Else
                        name = items(i)
                        value = ""
                    End If
                    Select Case LCase(Trim(name))
                        Case "documento" : documento = Trim(value)
                        Case "data di invio" : dataInvio = Formats.ParseDate(value)
                        Case "inviato al numero" : numero = Formats.ParsePhoneNumber(value)
                    End Select
                Next
                If (documento <> "" AndAlso dataInvio.HasValue AndAlso numero <> "") Then
                    'dataInvio = dataInvio.Value.ToLocalTime

                    Dim jobSent As FaxJob = Nothing
                    SyncLock Me.outQueueLock
                        For Each job As FaxJob In Me.OutQueue
                            If Me.checkDates(job.Date, dataInvio) AndAlso _
                                job.Options.TargetNumber = numero AndAlso _
                                Me.IsSameDocument(job.Options.FileName, documento) Then
                                jobSent = job
                                Exit For
                            End If
                        Next
                    End SyncLock

                    If (jobSent IsNot Nothing) Then
                        Me.doFaxDelivered(jobSent)
                    Else

                    End If
                End If

                Return True
            ElseIf Strings.Compare(Left(subject, Len(SPEDIZIONEACCETTATA)), SPEDIZIONEACCETTATA, CompareMethod.Text) = 0 Then
                Dim dataSpedizione As Date
                Dim nomeDocumento As String = ""
                Dim numeroDestinatario As String = ""

                text = Strings.RemoveHTMLTags(text)
                text = Replace(text, vbCrLf, vbCr)
                text = Replace(text, vbLf, vbCr)

                p = InStr(text, "La richiesta spedizione del ")
                text = Mid(text, p + Len("La richiesta spedizione del "))
                p = InStr(text, "è stata accettata con successo.")
                dataSpedizione = Formats.ParseDate(Left(text, p - 1))
                dataSpedizione = dataSpedizione.ToLocalTime
                text = Mid(text, p + Len("è stata accettata con successo.") + 1)
                p = InStr(text, "Documento:")
                text = Mid(text, p + Len("Documento:") + 1)
                p = InStr(text, "Inviato al numero:")
                nomeDocumento = Trim(Left(text, p - 1))
                text = Mid(text, p + Len("Inviato al numero:") + 1)
                p = InStr(text, "Faxator.com")
                numeroDestinatario = Formats.ParsePhoneNumber(Left(text, p - 1))

                fax = Nothing
                SyncLock Me.outQueueLock
                    For Each job As FaxJob In Me.OutQueue
                        If job.Options.TargetNumber = numeroDestinatario AndAlso _
                           Me.IsSameDocument(job.Options.FileName, nomeDocumento) AndAlso _
                           Me.checkDates(dataSpedizione, job.Date) Then
                            fax = job
                            Exit For
                        End If
                    Next
                End SyncLock
                If (fax IsNot Nothing) Then
                    Me.doFaxChangeStatus(fax, FaxJobStatus.SENDING, "Faxator ha preso in carico il documento")
                End If

                Return True
            ElseIf Strings.Compare(Left(subject, Len(NOTIFICATXERRATA)), NOTIFICATXERRATA, CompareMethod.Text) = 0 Then
                Return True
            ElseIf Strings.Compare(Left(subject, Len(MANCAFAX)), MANCAFAX, CompareMethod.Text) = 0 Then
                'Si è verificato un errore di invio: Manca l'allegato Fax
                Const lookFor As String = "abbiamo riscontrato il tentativo di un invio fax scorretto dalla tua e-mail del " '01/06/2015 10:19.
                p = InStr(text, lookFor)
                If (p > 0) Then
                    text = Mid(text, p + Len(lookFor) + 1)
                    p = InStr(text, vbCr)
                    If (p > 0) Then
                        text = Left(text, p - 1)
                        If (Right(text, 1) = ".") Then text = Left(text, Len(text) - 1)
                        dataInvio = Formats.ParseDate(text)
                        If (dataInvio.HasValue) Then
                            fax = Me.FindBySentDate(dataInvio, 5)

                            If (fax IsNot Nothing) Then
                                Me.doFaxError(fax, subject)
                            End If
                        End If
                    End If
                End If
            ElseIf Strings.Compare(Left(subject, Len(MANCAREGISTRAZIONE)), MANCAREGISTRAZIONE, CompareMethod.Text) = 0 Then
                'Manca la registrazione a Faxator
                'Abortiamo tutti i fax inviati
                SyncLock Me.outQueueLock
                    While (Me.OutQueue.Count > 0)
                        Me.doFaxError(Me.OutQueue(0), "Servizio Faxator non sottoscritto")
                    End While
                End SyncLock
            Else
                Debug.Print("Soggetto non riconosciuto: " & subject)
            End If

            Return False
        End Function

        Private Function checkDates(ByVal d1 As Date, ByVal d2 As Date, Optional ByVal minutesTolerance As Integer = 5) As Boolean
            Dim diff As Integer = Math.Abs(DateUtils.DateDiff(DateInterval.Minute, d1, d2))
            Return (diff <= minutesTolerance)
        End Function

        Private Function FindBySentDate(ByVal d As Date, ByVal minutesTolerance As Integer) As FaxJob
            Dim ret As FaxJob = Nothing
            Dim minDiff As Integer = Integer.MaxValue

            d = DateUtils.DateAdd(DateInterval.Hour, -FXTTIMEDIFF, d)

            SyncLock Me.outQueueLock
                For Each job As FaxJob In Me.OutQueue
                    Dim diff = DateUtils.DateDiff(DateInterval.Minute, d, job.Date)
                    diff = Math.Abs(diff)
                    If (diff <= minutesTolerance) AndAlso (diff < minDiff) Then
                        ret = job
                        minDiff = diff
                    End If
                Next
            End SyncLock

            Return ret
        End Function


     

        Private Function IsSameDocument(ByVal d1 As String, ByVal d2 As String) As Boolean
            d1 = DMD.Sistema.FileSystem.GetBaseName(d1)
            d2 = DMD.Sistema.FileSystem.GetBaseName(d1)
            Return Strings.Compare(d1, d2, CompareMethod.Text) = 0
        End Function
    End Class

End Namespace
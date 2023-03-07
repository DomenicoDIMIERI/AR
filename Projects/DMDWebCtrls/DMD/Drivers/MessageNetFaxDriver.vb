Imports System.Net.Mail
Imports DMD
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.Net.Mail

Namespace Drivers

    Public Class MessageNetFaxDriverModem
        Implements DMD.XML.IDMDXMLSerializable

        Private m_NumeroGeografico As String
        Private m_UserID As String
        Private m_Password As String
        Private m_eMail As String
        Private m_NotifyTo As String

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        ''' <summary>
        ''' Restituisce o imposta il numero geografico associato al servizio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NumeroGeografico As String
            Get
                Return Me.m_NumeroGeografico
            End Get
            Set(value As String)
                value = Formats.ParsePhoneNumber(value)
                If (Me.m_NumeroGeografico = value) Then Exit Property
                Me.m_NumeroGeografico = value
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la UserID associata al servizio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property UserID As String
            Get
                Return Me.m_UserID
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                If (Me.m_UserID = value) Then Exit Property
                Me.m_UserID = value
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la password associata al servizio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Password As String
            Get
                Return Me.m_Password
            End Get
            Set(value As String)
                If (Me.m_Password = value) Then Exit Property
                Me.m_Password = value
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la mail utilizzata come mittente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SenderEMail As String
            Get
                Return Me.m_eMail
            End Get
            Set(value As String)
                value = Formats.ParseEMailAddress(value)
                If (Me.m_eMail = value) Then Exit Property
                Me.m_eMail = value
            End Set
        End Property

        Protected Overridable Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal

        End Sub

        Protected Overridable Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize

        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

    Public Class MessageNetFaxDriverConfig
        Inherits FaxDriverOptions

        ''' <summary>
        ''' Restituisce o imposta il numero geografico associato al servizio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NumeroGeografico As String
            Get
                Return Me.GetValueString("NumeroGeografico", "")
            End Get
            Set(value As String)
                Me.SetValueString("NumeroGeografico", Formats.ParsePhoneNumber(value))
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la UserID associata al servizio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property UserID As String
            Get
                Return Me.GetValueString("UserID", "")
            End Get
            Set(value As String)
                Me.SetValueString("UserID", Strings.Trim(value))
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la password associata al servizio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Password As String
            Get
                Return Me.GetValueString("Password", "")
            End Get
            Set(value As String)
                Me.SetValueString("Password", value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la mail utilizzata come mittente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SenderEMail As String
            Get
                Return Me.GetValueString("SenderEMail", "")
            End Get
            Set(value As String)
                Me.SetValueString("SenderEMail", Strings.Trim(value))
            End Set
        End Property

    End Class

    Public Class MessageNetFaxDriver
        Inherits BaseFaxDriver

        Public Sub New()
        End Sub

        Public Shadows ReadOnly Property Config As MessageNetFaxDriverConfig
            Get
                Return MyBase.Config
            End Get
        End Property

        Public Overrides ReadOnly Property Description As String
            Get
                Return "Messagenet Fax"
            End Get
        End Property

        Public Overrides Function GetUniqueID() As String
            Return "MNETFAXSVC"
        End Function

        Protected Overrides Sub InternalSend(job As FaxJob)
            Dim m As MailMessage
            'Dim p As New DMD.PDF.PDFWriter
            Dim options As FaxDriverOptions = job.Options
            'Dim tempFileName As String = DMD.FileSystem.GetTempFileName("pdf")
            Dim ccnAddress As String = ""

#If DEBUG Then
            ccnAddress = "tecnico@DMD.net"
#Else
            ccnAddress = ""
#End If

            Dim codice As String = job.JobID

            Dim modem As FaxDriverModem = Me.GetModem(job.Options.ModemName)
            If (modem Is Nothing) Then Throw New InvalidOperationException("Non è stato definito alcun modem per il driver MessageNetFaxDriver")
            If (modem.SendEnabled = False) Then Throw New PermissionDeniedException("L'invio dei fax è stato disabilitato su questo modem")

            Sistema.ApplicationContext.Log("MessageNetFaxDriver.InternalSend(" & modem.eMailInvio & ", " & options.TargetNumber & ", " & codice & ")")

            m = EMailer.PrepareMessage(modem.eMailInvio, options.TargetNumber & "@fax.messagenet.it", "", ccnAddress, codice, "", "", False)

            'For i = 0 To UBound(images)
            ' p.AddPage()
            'p.DrawImage(images(i), 0, 0)
            'Next

            Dim att As New Attachment(options.FileName)
            m.Attachments.Add(att)
            m.IsBodyHtml = False

            EMailer.SendMessage(m)

            att.Dispose()
            m.Dispose()
            'p.Dispose()

        End Sub

        Protected Overrides Function InstantiateNewOptions() As DriverOptions
            Return New MessageNetFaxDriverConfig
        End Function

        Protected Overrides Sub CancelJobInternal(jobID As String)
            Throw New NotSupportedException("Impossibile annullare l'invio")
        End Sub


        Protected Overrides Sub InternalConnect()
            MyBase.InternalConnect()
            AddHandler EMailer.MessageReceived, AddressOf handleMessageReceived
        End Sub

        Protected Overrides Sub InternalDisconnect()
            RemoveHandler EMailer.MessageReceived, AddressOf handleMessageReceived
        End Sub

        Private Const fromPart As String = "@fax.messagenet.it"


        ''' <summary>
        ''' Restituisce vero se il messaggio proviene dal servizio di ricezione fax di messagenet
        ''' </summary>
        ''' <param name="address"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function IsFromMessageNet(ByVal address As String) As Boolean
            address = LCase(Trim(address))
            Return Right(address, Len(fromPart)) = fromPart
        End Function

        ''' <summary>
        ''' Estrae il numero dal mittente dall'indirizzo
        ''' </summary>
        ''' <param name="address"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function GetNumeroMittente(ByVal address As String) As String
            address = LCase(Trim(address))
            If Not (Me.IsFromMessageNet(address)) Then Return ""
            Return Left(address, Len(address) - Len(fromPart))
        End Function

        Private Function handleMessage(ByVal sender As Object, ByVal m As MailMessageEx, ByVal modem As FaxDriverModem) As Boolean
            Dim fName As String
            Dim ext As String
            Dim job As FaxJob

            'Fax ricevuto
            If (InStr(m.Subject, "fax per l'utente " & modem.UserName) > 0) OrElse _
                m.Subject = "F@X IN: fax per il numero " & modem.Numero Then
                If (m.Attachments.Count = 0) Then Return False

                Dim mittente As String = Me.GetNumeroMittente(m.From.Address)

                'Salviamo l'allegato
                SyncLock Me.inqueueLock
                    fName = FileSystem.GetBaseName(m.Attachments(0).FileName)

                    ext = FileSystem.GetExtensionName(m.Attachments(0).FileName)
                    If (fName = "") Then fName = Me.GetUniqueID & "I" & ASPSecurity.GetRandomKey(12)

                    If FileSystem.FileExists(Me.GetQueueFile("InQueue", fName & "." & ext)) Then
                        Dim i As Integer = 1
                        While (FileSystem.FileExists(Me.GetQueueFile("InQueue", fName & i & ".xml")))
                            i += 1
                        End While
                        fName = fName & i
                    End If

                    m.Attachments(0).SaveToFile(Me.GetQueueFile("InQueue", fName & "." & ext))

                    'Estriamoa il numero del mittente
                    job = Me.NewJob
                    Me.SetDriver(job, Me)
                    Me.SetDate(job, m.DeliveryDate)
                    job.Options.FromUser = mittente
                    job.Options.FileName = Me.GetQueueFile("InQueue", fName & "." & ext)
                    job.Options.SenderNumber = mittente
                    job.Options.SenderName = mittente
                    job.Options.RecipientName = modem.eMailRicezione
                    job.Options.TargetNumber = modem.Numero
                    job.Options.ModemName = modem.Name
                    If (modem.PuntoOperativo IsNot Nothing) Then job.Options.NomePuntoOperativo = modem.PuntoOperativo.Nome
                    Dim t As String = XML.Utils.Serializer.Serialize(job)
                    FileSystem.CreateTextFile(Me.GetQueueFile("InQueue", fName & ".xml"), t)
                End SyncLock

                Me.doFaxReceived(job)
                Return True
            End If

            'Controlliamo se il messaggio si riferisce ad un fax inviato
            Dim jobID As String = Split(m.Subject, " ")(0)
            job = Me.OutQueue.GetItemByKey(jobID)
            If job IsNot Nothing Then
                Dim stato As String = Trim(Mid(m.Subject, Len(job.JobID) + 1))
                Select Case LCase(stato)
                    Case "[ok]" ' Fax Inviato correttamente
                        Me.ParseOkMessage(m, job)
                        Me.doFaxDelivered(job)
                        Return True
                    Case "[in consegna]" 'Il fax sta per essere inviato
                        Me.ParseInConsegnaMessage(m, job)
                        Return True
                    Case "[destinatario errato]"
                        Me.ParseDestinatarioErrato(m, job)
                        Return True
                    Case "[cancelled na]" 'Risposta non di un apparecchio fax, ad esempio voce
                        Me.ParseDispositivoNonFax(m, job)
                        Return True
                    Case "[cancelled occ]" 'Numero occupato(utente)
                        Me.ParseNumeroOccupato(m, job)
                        Return True
                    Case "[cancelled cre]" ' Credito insufficiente
                        Me.ParseCreditoInsufficiente(m, job)
                        Return True
                    Case "[cancelled nc]" 'Numero occupato (rete) o malfunzionante
                        Me.ParseProblemaDiLinea(m, job)
                        Return True
                    Case "[cencelled abs]" 'Non risponde nessuno
                        Me.ParseNessunaRisposta(m, job)
                        Return True
                    Case "[cancelled inv]" 'Il file inviato o non è valid
                        Me.ParseFormatoNonValido(m, job)
                        Return True
                    Case "[cancelled]" 'Il file inviato o non è valid
                        Me.ParseCancelled(m, job)
                        Return True
                    Case "[credito insufficiente]" 'credito insufficiente
                        Me.ParseCreditoInsufficiente(m, job)
                        Return True
                    Case Else
                        Debug.Print("Caso strano?")
                End Select
            End If
            'msg.Dispose()
            Return False
        End Function

        Private Sub handleMessageReceived(ByVal sender As Object, ByVal e As DMD.Sistema.MailMessageEventArgs)
            Dim m As MailMessageEx = e.Message
            
            'Verifichiamo che la mail provenga dal servizio di ricezione fax di messagenet
            If m.From Is Nothing OrElse Not Me.IsFromMessageNet(m.From.Address) Then Exit Sub

            For Each modem As FaxDriverModem In Me.Modems
                If Me.handleMessage(sender, m, modem) Then Exit Sub
            Next

        End Sub

        Protected Sub ParseOkMessage(ByVal m As MailMessageEx, ByVal job As FaxJob)
            'Gentile Utente FAXout,
            'Il fax che hai inviato è stato consegnato con successo:
            '  * Riferimento: 14812000
            '  * Destinazione: +39 089338754
            '  * Stato: OK
            '  * Data e ora: 2015-06-16 18:37:38
            '  * Numero pagine: 1
            '  * Prezzo: 0,11 €
            '---------------------------------------------------------------------
            'Il tuo credito residuo è di: 5,68 € [ Ricarica [http://www.messagenet.it/it/ricarica/]]
            'Ti ricordiamo che con il tuo credito puoi anche TELEFONARE [http://www.messagenet.it/it/comunica/?chiama=chiama]
            'e INVIARE SMS [http://www.messagenet.it/it/comunica/?sms=sms].
            'Per qualsiasi chiarimento scrivi al supporto [mailto:support@messagenet.it].
            'Cordiali saluti,
            'Il team Messagenet
            'MESSAGENET S.p.A.
            'E-mail: support@messagenet.it [mailto:support@messagenet.it]
            'Web: www.messagenet.it [http://www.messagenet.it/]

            Dim text As String = Me.GetText(m)

            Const CMDriferimento As String = "* Riferimento:"
            Const CMDdestinazione As String = "* Destinazione:"
            Const CMDstato As String = "* Stato:"
            Const CMDdataeora As String = "* Data e ora:"
            Const CMDnumeroPagine As String = "* Numero pagine:"
            Const CMDprezzo As String = "* Prezzo:"
            Const CMDCredito As String = "Il tuo credito residuo è di:"

            Dim lines As String() = Split(text, vbCr)
            Dim riferimento As String = ""
            Dim destinazione As String = ""
            Dim stato As String = ""
            Dim dataeora As String = ""
            Dim numeroPagine As String = ""
            Dim prezzo As String = ""
            Dim credito As String = ""
            Dim p As Integer

            For i As Integer = 0 To Arrays.Len(lines) - 1
                Dim line As String = Trim(lines(i))
                If Left(line, Len(CMDriferimento)) = CMDriferimento Then
                    riferimento = Trim(Mid(line, Len(CMDriferimento) + 1))
                ElseIf Left(line, Len(CMDdestinazione)) = CMDdestinazione Then
                    destinazione = Trim(Mid(line, Len(CMDdestinazione) + 1))
                ElseIf Left(line, Len(CMDstato)) = CMDstato Then
                    stato = Trim(Mid(line, Len(CMDstato) + 1))
                ElseIf Left(line, Len(CMDdataeora)) = CMDdataeora Then
                    dataeora = Trim(Mid(line, Len(CMDdataeora) + 1))
                ElseIf Left(line, Len(CMDnumeroPagine)) = CMDnumeroPagine Then
                    numeroPagine = Trim(Mid(line, Len(CMDnumeroPagine) + 1))
                ElseIf Left(line, Len(CMDprezzo)) = CMDprezzo Then
                    prezzo = Trim(Mid(line, Len(CMDprezzo) + 1))
                ElseIf Left(line, Len(CMDCredito)) = CMDCredito Then
                    credito = Trim(Mid(line, Len(CMDCredito) + 1))
                    p = InStr(credito, " ")
                    If (p > 0) Then credito = Trim(Left(credito, p))
                End If
            Next

            job.Options.SetValueString("MsgNet_Riferimento", riferimento)
            job.Options.SetValueString("MsgNet_Destinazione", destinazione)
            job.Options.SetValueString("MsgNet_Stato", stato)
            job.Options.SetValueString("MsgNet_DataEOra", dataeora)
            job.Options.SetValueString("MsgNet_NumeroPagine", numeroPagine)
            job.Options.SetValueString("MsgNet_Prezzo", prezzo)
            job.Options.SetValueString("MsgNet_CreditoResiduo", credito)
            job.Options.NumberOfPages = Formats.ToInteger(numeroPagine)
            Me.SetDate(job, Me.ParseDate(dataeora))


        End Sub

        '"YYYY-MM-DD HH:mm:ss")
        Private Function ParseDate(ByVal value As String) As Nullable(Of Date)
            Try
                Dim dh() As String = Split(value, " ")
                Dim dd() As String = Split(dh(0), "-")
                Dim hh() As String = Split(dh(1), ":")
                Return DateUtils.MakeDate(dd(0), dd(1), dd(2), hh(0), hh(1), hh(2))
            Catch ex As Exception
                Return Nothing
            End Try
        End Function

        Protected Sub ParseInConsegnaMessage(ByVal m As MailMessageEx, ByVal job As FaxJob)
            'Gentile Utente FAXout,
            'il tuo messaggio per il numero +39 089338754 è stato preso in carico dal nostro sistema, che ora provvederà ad inoltrarlo al destinatario. 
            'Il codice assegnato al messaggio è 14812000
            'Ti ricordiamo che puoi sempre verificare se il tuo fax è stato consegnato visitando la pagina dei log di FAXout accedendo col tuo codice utente e la tua password. 
            'Per qualsiasi chiarimento scrivi al supporto. 

            Dim text As String = Me.GetText(m)

             
            Const CMDriferimento As String = "Il codice assegnato al messaggio è"
            
            Dim lines As String() = Split(text, vbCr)
            Dim riferimento As String = ""
            'Dim p As Integer

            For i As Integer = 0 To Arrays.Len(lines) - 1
                Dim line As String = Trim(lines(i))
                If Left(line, Len(CMDriferimento)) = CMDriferimento Then
                    riferimento = Trim(Mid(line, Len(CMDriferimento) + 1))
                End If
            Next

            job.Options.SetValueString("MsgNet_Riferimento", riferimento)
        End Sub

        Private Function GetText(ByVal m As MailMessageEx) As String
            Dim text As String = ""
            If (m.IsBodyHtml) Then
                For Each v As AlternateViewEx In m.AlternateViews
                    If v.ContentType IsNot Nothing AndAlso v.ContentType.MediaType = "text/plain" Then
                        text = v.GetContentAsText
                        Exit For
                    End If
                Next
            Else
                text = m.Body
            End If

            Dim buffer As New System.Text.StringBuilder
            For i As Integer = 1 To Len(text)
                Dim ch As Char = Mid(text, i, 1)
                Select Case ch
                    Case vbLf, vbFormFeed, vbTab
                    Case Else : buffer.Append(ch)
                End Select
            Next
            text = buffer.ToString

            Return text
        End Function

        Protected Sub ParseCancelled(ByVal m As MailMessageEx, ByVal job As FaxJob)
            Const CMDRiferimento As String = "* Riferimento:"
            Const CMDDestinazione As String = "* Destinazione:"
            Const CMDStato As String = "* Stato:"
            Const CMDDataEOra As String = "* Data e ora:"
            Const CMDNumeroPagine As String = "* Numero pagine:"
            Const CMDPrezzo As String = "* Prezzo:"

            Dim text As String = Me.GetText(m)
            Dim lines As String() = Split(text, vbCr)

            Dim riferimento As String = ""
            Dim destinazione As String = ""
            Dim stato As String = ""
            Dim dataeora As String = ""
            Dim numeroPagine As String = ""
            Dim prezzo As String = ""
            
            For i As Integer = 0 To Arrays.Len(lines) - 1
                Dim line As String = Trim(lines(i))
                If Strings.Compare(Left(line, Len(CMDRiferimento)), CMDRiferimento) = 0 Then
                    riferimento = Trim(Mid(line, Len(CMDRiferimento) + 1))
                ElseIf Strings.Compare(Left(line, Len(CMDDestinazione)), CMDDestinazione) = 0 Then
                    destinazione = Trim(Mid(line, Len(CMDDestinazione) + 1))
                ElseIf Strings.Compare(Left(line, Len(CMDStato)), CMDStato) = 0 Then
                    stato = Trim(Mid(line, Len(CMDStato) + 1))
                ElseIf Strings.Compare(Left(line, Len(CMDDataEOra)), CMDDataEOra) = 0 Then
                    dataeora = Trim(Mid(line, Len(CMDDataEOra) + 1))
                ElseIf Strings.Compare(Left(line, Len(CMDNumeroPagine)), CMDNumeroPagine) = 0 Then
                    numeroPagine = Trim(Mid(line, Len(CMDNumeroPagine) + 1))
                ElseIf Strings.Compare(Left(line, Len(CMDPrezzo)), CMDPrezzo) = 0 Then
                    prezzo = Trim(Mid(line, Len(CMDPrezzo) + 1))
                Else

                End If
            Next

            Select Case LCase(Trim(stato))
                Case "[na]" 'Risposta non di un apparecchio fax, ad esempio voce
                    Me.ParseDispositivoNonFax(m, job)
                Case "[occ]" 'Numero occupato(utente)
                    Me.ParseNumeroOccupato(m, job)
                Case "[cre]" ' Credito insufficiente
                    Me.ParseCreditoInsufficiente(m, job)
                Case "[nc]" 'Numero occupato (rete) o malfunzionante
                    Me.ParseProblemaDiLinea(m, job)
                Case "[abs]" 'Non risponde nessuno
                    Me.ParseNessunaRisposta(m, job)
                Case "[inv]" 'Il file inviato o non è valid
                    Me.ParseFormatoNonValido(m, job)
                Case Else
                    Me.SetDate(job, m.DeliveryDate)
                    Me.doFaxError(job, "Cancelled " & stato)
            End Select



        End Sub
      
        Protected Sub ParseDestinatarioErrato(ByVal m As MailMessageEx, ByVal job As FaxJob)
            'Gentile Utente FAXout,
            'il tuo fax verso il numero 3470815531 non è stato inoltrato, in quanto il numero non è stato riconosciuto come un destinatario valido. 
            'Per qualsiasi chiarimento scrivi al supporto. 


            Dim text As String = Me.GetText(m)

            Dim lines As String() = Split(text, vbCr)
            Dim cmd As String = "il tuo fax verso il numero "
            Dim cmd1 As String = "non è stato inoltrato, in quanto"
            Dim numero As String = ""
            Dim errore As String = ""

            For i As Integer = 0 To Arrays.Len(lines) - 1
                Dim line As String = Trim(lines(i))
                If Left(line, Len(CMD)) = cmd Then
                    numero = Trim(Mid(line, Len(cmd) + 1))
                    Dim p As Integer = InStr(numero, " ")
                    If (p > 0) Then
                        numero = Trim(Left(numero, p - 1))
                    End If
                    If (i + 1 < Arrays.Len(lines)) Then errore = Trim(lines(i + 1))
                    Exit For
                End If
            Next

            Me.SetDate(job, m.DeliveryDate)

            Me.doFaxError(job, errore)
        End Sub

        Private Sub ParseDispositivoNonFax(m As MailMessageEx, job As FaxJob)
            Me.SetDate(job, m.DeliveryDate)
            Me.doFaxError(job, "Il dispositivo remoto non è un fax")
        End Sub

        Private Sub ParseNumeroOccupato(m As MailMessageEx, job As FaxJob)
            Me.SetDate(job, m.DeliveryDate)
            Me.doFaxError(job, "Numero Occupato")
        End Sub

        Private Sub ParseCreditoInsufficiente(m As MailMessageEx, job As FaxJob)
            Me.SetDate(job, m.DeliveryDate)
            Me.doFaxError(job, "Credito Insufficiente")
        End Sub

        Private Sub ParseProblemaDiLinea(m As MailMessageEx, job As FaxJob)
            Me.SetDate(job, m.DeliveryDate)
            Me.doFaxError(job, "Problemi di Linea")
        End Sub

        Private Sub ParseNessunaRisposta(m As MailMessageEx, job As FaxJob)
            Me.SetDate(job, m.DeliveryDate)
            Me.doFaxError(job, "Nessuna Risposta")
        End Sub

        Private Sub ParseFormatoNonValido(m As MailMessageEx, job As FaxJob)
            Me.SetDate(job, m.DeliveryDate)
            Me.doFaxError(job, "Formato del documento inviato non valido")
        End Sub

    End Class

End Namespace
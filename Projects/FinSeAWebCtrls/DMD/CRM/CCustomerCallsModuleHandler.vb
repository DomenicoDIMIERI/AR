Imports DMD
Imports DMD.Sistema
Imports DMD.WebSite
Imports DMD.CustomerCalls
Imports DMD.Forms
Imports DMD.Anagrafica
Imports DMD.Databases
Imports DMD.Forms.Utils

Imports DMD.Office
Imports DMD.XML

Namespace Forms



    Public Class CCustomerCallsModuleHandler
        Inherits CBaseModuleHandler



        Public Sub New()
            Me.UseLocal = True
        End Sub

        Public Function GetConfiguration() As String
            'If Not Me.CanList(CRM.Config) Then Throw New PermissionDeniedException(Me.Module, "list")
            Return XML.Utils.Serializer.Serialize(CRM.Config)
        End Function

        Public Function SetConfiguration(ByVal renderer As Object) As String
            If Not Me.CanConfigure() Then Throw New PermissionDeniedException(Me.Module, "configure")
            Dim testo As String = RPC.n2str(GetParameter(renderer, "testo", ""))
            Dim config As CCRMConfig = XML.Utils.Serializer.Deserialize(testo)
            config.Save()
            Return ""
        End Function

        Function EsportaContatti(ByVal renderer As Object) As String
            If Not Me.CanExport Then Throw New PermissionDeniedException(Me.Module, "export")

            Dim testo As String = RPC.n2str(GetParameter(renderer, "text", ""))
            Dim fmt As String = RPC.n2str(GetParameter(renderer, "fmt", ""))
            Dim srcItems As CCollection = XML.Utils.Serializer.Deserialize(testo)
            Dim dstItems As New CCollection
            Dim tmpURL As String = ""

            For Each a As StoricoAction In srcItems
                Dim c As Object = DMD.Sistema.Types.GetItemByTypeAndId(a.ActionType, a.ActionID)
                dstItems.Add(c)
            Next

            Select Case LCase(fmt)
                Case "xls"

                Case "xml"
                    testo = XML.Utils.Serializer.Serialize(dstItems)
                    tmpURL = WebSite.Instance.Configuration.TempFolder & ASPSecurity.GetRandomKey(10) & ".xml"
                    DMD.Sistema.FileSystem.SetTextFileContents(Sistema.ApplicationContext.MapPath(tmpURL), testo)
                Case Else
            End Select

            Return XML.Utils.Serializer.SerializeString(tmpURL)
        End Function

        Function ImportaContatti(ByVal renderer As Object) As String
            If Not Me.CanImport Then Throw New PermissionDeniedException(Me.Module, "import")

            Dim fName As String = RPC.n2str(GetParameter(renderer, "fname", ""))
            Dim testo As String = DMD.Sistema.FileSystem.GetTextFileContents(Sistema.ApplicationContext.MapPath(fName))
            Dim items As CCollection = XML.Utils.Serializer.Deserialize(testo)

            Dim pid As Integer = RPC.n2int(GetParameter(renderer, "pid", ""))
            Dim p As CPersona = Anagrafica.Persone.GetItemById(pid)
            Dim cnt As Integer = 0

            For Each c As Object In items
                If (TypeOf (c) Is CContattoUtente) Then
                    With DirectCast(c, CContattoUtente)
                        DBUtils.ResetID(c)
                        .Azienda = Anagrafica.Aziende.AziendaPrincipale
                        .Persona = p
                        .Save()
                        cnt += 1
                    End With
                Else
                    Debug.Print("Not supported: " & TypeName(c))
                End If
            Next

            Return XML.Utils.Serializer.SerializeInteger(cnt)
        End Function

        Public Function getArrayNomiPOXUser(ByVal renderer As Object) As String
            Dim items As CCollection(Of CUfficio) = Anagrafica.Uffici.GetPuntiOperativiConsentiti
            Dim writer As New System.Text.StringBuilder
            For i As Integer = 0 To items.Count - 1
                Dim item As CUfficio = items(i)
                If (writer.Length > 0) Then writer.Append(vbCr)
                writer.Append(Strings.HtmlEncode(item.Nome))
            Next

            Return writer.ToString
        End Function

        Public Function AppendNumber(ByVal html As String, ByVal nome As String, ByVal value As String) As String
            Dim ret As String
            ret = html
            If ret <> "" Then ret = ret & vbCr
            ret = ret & nome & " (" & value & ") " & vbCr & value
            Return ret
        End Function

        Public Function getArrayContattiTelefonici(ByVal renderer As Object) As String
            Dim persona As CPersona
            Dim idPersona As Integer
            idPersona = RPC.n2int(Me.GetParameter(renderer, "pid", ""))
            persona = Persone.GetItemById(idPersona)

            Dim html As String = ""
            With persona
                For Each c As CContatto In persona.Recapiti
                    If c.Tipo = "Telefono" AndAlso c.Valore <> "" Then html = Me.AppendNumber(html, c.Nome, c.Valore)
                Next
            End With
            Return html
        End Function

        Public Function getArrayPersoneContattate(ByVal renderer As Object) As String
            Dim idUfficio As Integer = RPC.n2int(Me.GetParameter(renderer, "uf", ""))
            Dim inizio As Nullable(Of Date) = RPC.n2date(Me.GetParameter(renderer, "ini", ""))
            Dim fine As Nullable(Of Date) = RPC.n2date(Me.GetParameter(renderer, "end", ""))
            Dim ir As Boolean = RPC.n2bool(Me.GetParameter(renderer, "ir", ""))
            Dim idOperatori As String() = Split(RPC.n2str(Me.GetParameter(renderer, "op", "")), ",")
            Dim operatori As New ArrayList
            For Each Str As String In idOperatori
                If (Str <> "") Then operatori.Add(Formats.ToInteger(Str))
            Next

            Dim arr() As Integer = operatori.ToArray(GetType(Integer))
            Dim items() As Integer = CustomerCalls.Telefonate.GetIDPersoneContattate(idUfficio, arr, inizio, fine, ir)
            If (Arrays.Len(items) > 0) Then
                Return XML.Utils.Serializer.SerializeArray(items)
            Else
                Return ""
            End If
        End Function

        Public Function contaPersoneContattate(ByVal renderer As Object) As String
            Dim idUfficio As Integer = RPC.n2int(Me.GetParameter(renderer, "uf", ""))
            Dim inizio As Nullable(Of Date) = RPC.n2date(Me.GetParameter(renderer, "ini", ""))
            Dim fine As Nullable(Of Date) = RPC.n2date(Me.GetParameter(renderer, "end", ""))
            Dim ir As Boolean = RPC.n2bool(Me.GetParameter(renderer, "ir", ""))
            Dim idOperatori As String() = Split(RPC.n2str(Me.GetParameter(renderer, "op", "")), ",")
            Dim operatori As New ArrayList
            For Each Str As String In idOperatori
                If (Str <> "") Then operatori.Add(Formats.ToInteger(Str))
            Next
            Return XML.Utils.Serializer.SerializeInteger(CustomerCalls.Telefonate.ContaPersoneContattate(idUfficio, operatori.ToArray(GetType(Integer)), inizio, fine, ir))
        End Function




        Public Overrides ReadOnly Property SupportsDuplicate As Boolean
            Get
                Return False
            End Get
        End Property

        Public Overrides ReadOnly Property SupportsEdit As Boolean
            Get
                Return True
            End Get
        End Property

        Public Overrides ReadOnly Property SupportsAnnotations As Boolean
            Get
                Return False
            End Get
        End Property

        Public Overrides ReadOnly Property SupportsDelete As Boolean
            Get
                Return True
            End Get
        End Property

        Public Overrides ReadOnly Property SupportsCreate As Boolean
            Get
                Return False
            End Get
        End Property



        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CCustomerCallsCursor
        End Function


        Public Overrides Function GetExportableColumnsList() As CCollection(Of ExportableColumnInfo)
            Dim ret As CCollection(Of ExportableColumnInfo) = MyBase.GetExportableColumnsList()
            ret.Add(New ExportableColumnInfo("ID", "ID", TypeCode.Int32, True))
            ret.Add(New ExportableColumnInfo("ClassName", "Tipo Oggetto", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("DirezioneChiamata", "I/O", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("NomePuntoOperativo", "Punto Operativo", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("NomeOperatore", "Operatore", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("Data", "Data", TypeCode.DateTime, True))
            ret.Add(New ExportableColumnInfo("NomePersona", "Nominativo", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("CFPersona", "Codice Fiscale", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("IndirizzoContatto", "Indirizzo/Numero", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("Scopo", "Scopo", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("Note", "Note", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("Esito", "Esito", TypeCode.String, True))
            Return ret
        End Function

        Protected Overrides Function GetColumnValue(ByVal renderer As Object, item As Object, key As String) As Object
            Dim t As CContattoUtente = item
            Select Case key
                Case "ClassName"
                    If (TypeOf (t) Is CTelefonata) Then
                        Return "Telefonata"
                    ElseIf (TypeOf (t) Is CVisita) Then
                        Return "Visita"
                    Else
                        Return TypeName(t)
                    End If
                Case "DirezioneChiamata" : Return IIf(t.Ricevuta, "Ricevuta", "Effettuata")
                Case "IndirizzoContatto"
                    If (TypeOf (t) Is CTelefonata) Then
                        Return DirectCast(t, CTelefonata).NumeroOIndirizzo
                    ElseIf (TypeOf (t) Is CVisita) Then
                        Return DirectCast(t, CVisita).Luogo.ToString
                    Else
                        Return ""
                    End If
                Case "Esito" : Return IIf(t.Esito = EsitoChiamata.RIPOSTA, "OK", "NO: " & t.DettaglioEsito)
                Case "CFPersona" : Return t.Persona.CodiceFiscale
                Case Else
                    Return MyBase.GetColumnValue(renderer, item, key)
            End Select
        End Function

        Protected Overrides Sub SetColumnValue(ByVal renderer As Object, item As Object, key As String, ByVal value As Object)
            Dim t As CContattoUtente = item
            Select Case key
                Case "DirezioneChiamata" : t.Ricevuta = (Formats.ToString(value) = "I")
                Case "IndirizzoContatto"
                    If TypeOf (t) Is CTelefonata Then
                        DirectCast(t, CTelefonata).NumeroOIndirizzo = Formats.ParsePhoneNumber(value)
                    Else
                        DirectCast(t, CVisita).Luogo = New CIndirizzo(Formats.ToString(value))
                    End If
                Case "CFPersona"
                Case Else : MyBase.SetColumnValue(renderer, item, key, value)
            End Select
        End Sub

        Public Overrides ReadOnly Property SupportsExport As Boolean
            Get
                Return True
            End Get
        End Property

        Public Overrides ReadOnly Property SupportsImport As Boolean
            Get
                Return False
            End Get
        End Property

        Public Function getElencoAppuntamenti(ByVal renderer As Object) As String
            Dim pid As Integer = RPC.n2int(Me.GetParameter(renderer, "pid", ""))
            Dim ret As New CCollection(Of CRicontatto)

            If (pid <> 0) Then
                Dim cursor As New CRicontattiCursor
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.DataPrevista.SortOrder = SortEnum.SORT_DESC
                cursor.IDPersona.Value = pid
                cursor.IgnoreRights = True
                cursor.TipoAppuntamento.Value = "Appuntamento"

                While Not cursor.EOF
                    ret.Add(cursor.Item)
                    cursor.MoveNext()
                End While
                cursor.Dispose()
            End If

            If (ret.Count > 0) Then
                Return XML.Utils.Serializer.Serialize(ret.ToArray, XMLSerializeMethod.Document)
            Else
                Return vbNullString
            End If
        End Function

        Public Function CanSendSMS(ByVal item As SMSMessage) As Boolean
            Return Me.Module.UserCanDoAction("send_sms")
        End Function

        Public Function SendSMS(ByVal renderer As Object) As String
            Dim text As String = RPC.n2str(GetParameter(renderer, "text", ""))
            Dim sms As SMSMessage = XML.Utils.Serializer.Deserialize(text)
            If (Not Me.CanSendSMS(sms)) Then Throw New PermissionDeniedException(Me.Module, "SendSMS")
            Dim options As New SMSDriverOptions
            options.Mittente = sms.Options.GetValueString("Mittente", "")
            options.RichiediConfermaDiLettura = sms.Options.GetValueBool("RichiedeConfermaDiLettura", False)
            sms.MessageID = Sistema.SMSService.Send(sms.NumeroOIndirizzo, sms.Note, options)
            If (sms.MessageID <> "") Then
                Dim stato As MessageStatus = SMSService.GetStatus(sms.MessageID)
                sms.Options.SetValueString("DriverID", SMSService.DefaultDriver.GetUniqueID)
                sms.Options.SetValueInt("StatoMessaggio", stato.MessageStatus)
                sms.Options.SetValueString("StatoMessaggioEx", stato.MessageStatusEx)
                sms.Options.SetValueDate("DataConsegna", stato.DeliveryTime)
                sms.DettaglioEsito = stato.MessageStatusEx
                Select Case stato.MessageStatus
                    Case MessageStatusEnum.Scheduled : sms.Esito = EsitoChiamata.ALTRO
                    Case MessageStatusEnum.Sent : sms.Esito = EsitoChiamata.RIPOSTA
                    Case MessageStatusEnum.Delivered : sms.Esito = EsitoChiamata.RIPOSTA
                    Case MessageStatusEnum.Error : sms.Esito = EsitoChiamata.NESSUNA_RISPOSTA
                    Case MessageStatusEnum.Timeout : sms.Esito = EsitoChiamata.NESSUNA_RISPOSTA
                    Case MessageStatusEnum.BadNumber : sms.Esito = EsitoChiamata.NESSUNA_RISPOSTA
                    Case MessageStatusEnum.Waiting : sms.Esito = EsitoChiamata.ALTRO
                    Case MessageStatusEnum.Unknown : sms.Esito = EsitoChiamata.ALTRO
                End Select
            End If
            sms.Stato = ObjectStatus.OBJECT_VALID
            sms.Save()
            Return XML.Utils.Serializer.Serialize(sms, XMLSerializeMethod.Document)
        End Function

        Public Function CanSendFax(ByVal item As FaxDocument) As Boolean
            Return Me.Module.UserCanDoAction("send_fax")
        End Function

        Public Function SendFax(ByVal renderer As Object) As String
            Dim text As String = RPC.n2str(GetParameter(renderer, "text", ""))
            Dim fax As FaxDocument = XML.Utils.Serializer.Deserialize(text)
            If (Not Me.CanSendFax(fax)) Then Throw New PermissionDeniedException(Me.Module, "SendFax")

            Dim driverName As String = fax.Options.GetValueString("FAXDriver")
            Dim modemName As String = fax.Options.GetValueString("FAXModem")
            Dim driver As BaseFaxDriver = Nothing
            Dim modem As FaxDriverModem = Nothing
            If (driverName <> "") Then
                driver = Sistema.FaxService.GetDriver(driverName)
                If (driver Is Nothing) Then Throw New InvalidOperationException("Driver non installato: " & driverName)
            End If
            If (modemName <> "") Then
                For Each modem1 As FaxDriverModem In driver.Modems
                    If modem1.Name = modemName Then
                        modem = modem1
                        Exit For
                    End If
                Next
                If modem Is Nothing Then Throw New InvalidOperationException("Modem non installato: " & modemName)
                modemName = modem.Name
            End If

            Dim job As FaxJob = Sistema.FaxService.NewJob

            job.Options.SenderName = fax.Options.GetValueString("Mittente", "")
            job.Options.TargetNumber = fax.NumeroOIndirizzo
            job.Options.FileName = Sistema.ApplicationContext.MapPath(fax.Attachment.URL)
            'options.RichiediConfermaDiLettura = fax.Options.GetValueBool("RichiedeConfermaDiLettura", False)
            job.Options.ModemName = modemName

            If (driver Is Nothing) Then
                fax.MessageID = Sistema.FaxService.Send(job)
            Else
                fax.MessageID = Sistema.FaxService.Send(driver, job)
            End If

            Select Case job.JobStatus
                Case FaxJobStatus.CANCELLED
                    fax.StatoConversazione = StatoConversazione.CONCLUSO
                    fax.Esito = EsitoChiamata.NESSUNA_RISPOSTA
                    fax.DettaglioEsito = "Annullato: " & job.JobStatusMessage
                Case FaxJobStatus.COMPLETED
                    fax.StatoConversazione = StatoConversazione.CONCLUSO
                    fax.Esito = EsitoChiamata.RIPOSTA
                    fax.DettaglioEsito = "Inviato: " & job.JobStatusMessage
                Case FaxJobStatus.DIALLING
                    fax.StatoConversazione = StatoConversazione.INCORSO
                    fax.Esito = EsitoChiamata.ALTRO
                    fax.DettaglioEsito = "Chiamata in corso: " & job.JobStatusMessage
                Case FaxJobStatus.ERROR
                    fax.StatoConversazione = StatoConversazione.CONCLUSO
                    fax.Esito = EsitoChiamata.NESSUNA_RISPOSTA
                    fax.DettaglioEsito = "Errore: " & job.JobStatusMessage
                Case FaxJobStatus.PREPARING
                    fax.StatoConversazione = StatoConversazione.INCORSO
                    fax.Esito = EsitoChiamata.ALTRO
                    fax.DettaglioEsito = "In preparazione: " & job.JobStatusMessage
                Case FaxJobStatus.QUEUED
                    fax.StatoConversazione = StatoConversazione.INCORSO
                    fax.Esito = EsitoChiamata.ALTRO
                    fax.DettaglioEsito = "Messo in coda: " & job.JobStatusMessage
                Case FaxJobStatus.SENDING
                    fax.StatoConversazione = StatoConversazione.INCORSO
                    fax.Esito = EsitoChiamata.ALTRO
                    fax.DettaglioEsito = "Trasmissione in corsor: " & job.JobStatusMessage
                Case FaxJobStatus.TIMEOUT
                    fax.StatoConversazione = StatoConversazione.CONCLUSO
                    fax.Esito = EsitoChiamata.NESSUNA_RISPOSTA
                    fax.DettaglioEsito = "Errore, Timeout della trasmissione: " & job.JobStatusMessage
                Case FaxJobStatus.WAITRETRY
                    fax.StatoConversazione = StatoConversazione.INCORSO
                    fax.Esito = EsitoChiamata.ALTRO
                    fax.DettaglioEsito = "Errore, in attesa di ritrasmissione: " & job.JobStatusMessage
            End Select
            'If (fax.MessageID <> "") Then
            '    fax.StatoConversazione = StatoConversazione.INATTESA
            'End If
            fax.Stato = ObjectStatus.OBJECT_VALID
            fax.Save()
            Return XML.Utils.Serializer.Serialize(fax, XMLSerializeMethod.Document)
        End Function

        Public Function CheckBackListed(ByVal renderer As Object) As String
            Dim indirizzo As String = RPC.n2str(GetParameter(renderer, "addr", ""))
            Dim tipo As String = RPC.n2str(GetParameter(renderer, "taddr", ""))
            Dim ret As BlackListAddress = CustomerCalls.BlackListAdresses.CheckBlocked(tipo, indirizzo)
            If (ret Is Nothing) Then Return ""
            Return XML.Utils.Serializer.Serialize(ret)
        End Function

        Public Function BlackListAddress(ByVal renderer As Object) As String
            Dim indirizzo As String = RPC.n2str(GetParameter(renderer, "addr", ""))
            Dim tipo As BlackListType = RPC.n2int(GetParameter(renderer, "tp", "0"))
            Dim tipoContatto As String = RPC.n2str(GetParameter(renderer, "taddr", ""))
            Dim motivo As String = RPC.n2str(GetParameter(renderer, "mot", ""))

            Dim ret As BlackListAddress = CustomerCalls.BlackListAdresses.BlockAddress(tipoContatto, indirizzo, tipo, motivo)

            If (ret Is Nothing) Then Return ""
            Return XML.Utils.Serializer.Serialize(ret)
        End Function

        Public Function UnBlackListAddress(ByVal renderer As Object) As String
            Dim indirizzo As String = RPC.n2str(GetParameter(renderer, "addr", ""))
            Dim tipo As String = RPC.n2str(GetParameter(renderer, "taddr", ""))
            Dim ret As BlackListAddress = CustomerCalls.BlackListAdresses.UnblockAddress(tipo, indirizzo)
            If (ret Is Nothing) Then Return ""
            Return XML.Utils.Serializer.Serialize(ret)
        End Function

        Public Function GetStats(ByVal renderer As Object) As String
            Dim ret As New CKeyCollection(Of CRMStatsAggregation)
            Dim filter As CRMFilter = XML.Utils.Serializer.Deserialize(RPC.n2str(GetParameter(renderer, "filter", "")))
            ret.Add("Telefonate", Telefonate.GetStats(filter))
            ret.Add("Visite", Visite.GetStats(filter))
            ret.Add("SMS", SMS.GetStats(filter))
            ret.Add("FAX", FAX.GetStats(filter))
            ret.Add("Telegrammi", Telegrammi.GetStats(filter))
            Return XML.Utils.Serializer.Serialize(ret, XMLSerializeMethod.Document)
        End Function

        Public Function GetActivePersons(ByVal renderer As Object) As String
            Dim cursor As CPersonaCursor = Nothing

            Try
                Dim filter As CRMFilter = XML.Utils.Serializer.Deserialize(RPC.n2str(GetParameter(renderer, "filter", "")))
                If (filter Is Nothing) Then Throw New ArgumentNullException("filter")
                If RPC.n2str(GetParameter(renderer, "cursor", "")) <> "" Then
                    cursor = XML.Utils.Serializer.Deserialize(RPC.n2str(GetParameter(renderer, "cursor", "")))
                End If
                If (cursor Is Nothing) Then
                    cursor = New CPersonaCursor
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                End If


                Dim ret As CCollection(Of CActivePerson) = Nothing

                If (filter.NomeLista = vbCrLf & "[Suggeriti]") Then
                    'ret = CustomerCalls.Telefonate.GetSuggeriti(filter, cursor)
                    filter.NomeLista = "Suggeriti"
                    ret = Anagrafica.Ricontatti.GetActivePersons(filter, cursor)
                Else
                    ret = Anagrafica.Ricontatti.GetActivePersons(filter, cursor)
                End If


                'Dim persone As Integer() = {}
                'For Each r As CActivePerson In ret
                '    If (r.PersonID <> 0) Then
                '        Dim i As Integer = Array.BinarySearch(persone, r.PersonID)
                '        If (i < 0) Then
                '            i = Arrays.GetInsertPosition(persone, r.PersonID, 0, persone.Length)
                '            persone = Arrays.Insert(persone, 0, persone.Length, r.PersonID, i)
                '        End If
                '    End If
                'Next

                'Dim ultimiContatti As New CKeyCollection(Of CContattoUtente)
                'If (persone.Length > 0) Then
                '    Dim cursor As CPersonStatsCursor = Nothing
                '    Try
                '        cursor = New CPersonStatsCursor
                '        cursor.IDPersona.ValueIn(persone)
                '        While Not cursor.EOF
                '            Dim info As CPersonStats = cursor.Item
                '            Dim c As CContattoUtente = info.UltimoContattoOk
                '            If (c Is Nothing) Then c = info.UltimoContattoNo
                '            If (c IsNot Nothing) Then ultimiContatti.Add("K" & c.IDPersona, c)
                '            cursor.MoveNext()
                '        End While
                '        cursor.Dispose()
                '    Catch ex As Exception
                '        Sistema.Events.NotifyUnhandledException(ex)
                '        Throw
                '    Finally
                '        If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
                '    End Try
                'End If


                For Each r As CActivePerson In ret
                    If (r.Person IsNot Nothing) Then
                        If (r.Person.TipoPersona = TipoPersona.PERSONA_FISICA) Then
                            With DirectCast(r.Person, CPersonaFisica)
                                r.MoreInfo.Add("Nato A:", .NatoA.NomeComune & " il " & Formats.FormatUserDate(.DataNascita))
                                If .ImpiegoPrincipale IsNot Nothing Then
                                    r.MoreInfo.Add("Impiego", FormatImpiego(.ImpiegoPrincipale))
                                End If
                            End With
                        End If
                        'Dim u As DMD.CustomerCalls.CContattoUtente = ultimiContatti.GetItemByKey("K" & r.PersonID)
                        'If (u IsNot Nothing) Then r.MoreInfo.Add("UltimaChiamata", u)
                    End If
                Next

                'For i As Integer = 0 To ret.Count - 1
                '    Dim r As CActivePerson = ret(i)
                '    If (r.Person IsNot Nothing) Then
                '        If (r.Person.TipoPersona = TipoPersona.PERSONA_FISICA) Then
                '            With DirectCast(r.Person, CPersonaFisica)
                '                r.MoreInfo.Add("Nato A:", .NatoA.NomeComune & " il " & Formats.FormatUserDate(.DataNascita))
                '                If .ImpiegoPrincipale IsNot Nothing Then
                '                    r.MoreInfo.Add("Impiego", .ImpiegoPrincipale.NomeAzienda)
                '                    r.MoreInfo.Add("TipoRapporto", .ImpiegoPrincipale.TipoRapporto)
                '                End If
                '            End With
                '        End If
                '        'Dim u As DMD.CustomerCalls.CContattoUtente = CustomerCalls.Telefonate.GetUltimaChiamata(r.Person)
                '        'If (u IsNot Nothing) Then
                '        '    r.MoreInfo.Add("UltimaChiamata", u)
                '        'End If

                '    End If
                'Next


                Return XML.Utils.Serializer.Serialize(ret, XMLSerializeMethod.Document)
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Function

        Private Function FormatImpiego(ByVal impiego As CImpiegato) As String
            Dim ret As String = ""
            If (impiego.Posizione <> "") Then ret = impiego.Posizione
            If (impiego.TipoRapporto <> "") Then ret = Strings.Combine(ret, "(" & impiego.TipoRapporto & ")", " ")
            If (impiego.NomeAzienda <> "") Then ret = Strings.Combine(ret, impiego.NomeAzienda, " presso ")
            If (impiego.DataAssunzione.HasValue) Then ret = Strings.Combine(ret, Formats.FormatUserDate(impiego.DataAssunzione), " dal ")
            Return ret
        End Function

        Public Function GetContattiInAttesa(ByVal renderer As Object) As String
            Dim po As Nullable(Of Integer) = RPC.n2int(GetParameter(renderer, "po", ""))
            Dim op As Nullable(Of Integer) = RPC.n2int(GetParameter(renderer, "op", ""))
            Dim items As CCollection(Of ContattoInAttesaInfo) = CustomerCalls.CRM.GetTelefonateInAttesa(po, op)
            If (items.Count > 0) Then
                Return XML.Utils.Serializer.Serialize(items.ToArray)
            Else
                Return ""
            End If
        End Function

        Public Function ContaContattiInAttesa(ByVal renderer As Object) As String
            Dim po As Nullable(Of Integer) = RPC.n2int(GetParameter(renderer, "po", ""))
            Dim op As Nullable(Of Integer) = RPC.n2int(GetParameter(renderer, "op", ""))
            Return XML.Utils.Serializer.SerializeInteger(CustomerCalls.CRM.ContaContattiInAttesa(po, op))
        End Function

        Public Function GetUltimaVisitaInCorso(ByVal renderer As Object) As String
            Dim pID As Integer = RPC.int2n(GetParameter(renderer, "pid", ""))
            Dim visita As CVisita = CustomerCalls.Visite.GetUltimaVisitaInCorso(pID)
            If (visita) Is Nothing Then Return ""
            Return XML.Utils.Serializer.Serialize(visita)
        End Function

        Public Function GetUltimaTelefonataInCorso(ByVal renderer As Object) As String
            Dim pID As Integer = RPC.int2n(GetParameter(renderer, "pid", ""))
            Dim visita As CTelefonata = CustomerCalls.Telefonate.GetUltimaTelefonataInCorso(pID)
            If (visita) Is Nothing Then Return ""
            Return XML.Utils.Serializer.Serialize(visita)
        End Function

        'Public Overrides Function ExecuteAction(renderer As Object, actionName As String) As String
        '    Select Case actionName
        '        Case "getStoricoContatti" : Return Me.getStoricoContatti
        '        Case "EsportaContatti" : Return Me.EsportaContatti
        '        Case "ImportaContatti" : Return Me.ImportaContatti
        '        Case Else : Return MyBase.ExecuteAction(renderer, actionName)
        '    End Select
        'End Function

        Public Function GetRicontattiModificati(ByVal renderer As Object) As String
            Dim filter As CRMFilter = XML.Utils.Serializer.Deserialize(RPC.n2str(GetParameter(renderer, "filter", "")))
            Return XML.Utils.Serializer.Serialize(Anagrafica.Ricontatti.GetRicontattiModificati(filter))
        End Function

        Public Function GetPersoneContattate(ByVal renderer As Object) As String
            Dim cursor As CCustomerCallsCursor = Nothing
            Dim dbRis As System.Data.IDataReader = Nothing
            Try
                cursor = XML.Utils.Serializer.Deserialize(RPC.n2str(GetParameter(renderer, "c", "")))
                Dim dbSQL As String = "SELECT [IDPersona] FROM (" & cursor.GetSQL & ") GROUP BY [IDPersona]"
                cursor.Dispose() : cursor = Nothing

                dbRis = CustomerCalls.CRM.TelDB.ExecuteReader(dbSQL)

                Dim buffer As New System.Text.StringBuilder
                While dbRis.Read
                    Dim id As Integer = Formats.ToInteger(dbRis("IDPersona"))
                    If (id <> 0) Then
                        If buffer.Length > 0 Then buffer.Append(",")
                        buffer.Append(DBUtils.DBNumber(id))
                    End If
                End While
                dbRis.Dispose() : dbRis = Nothing

                Dim ret As New CCollection(Of CPersonaFisica)
                If (buffer.Length > 0) Then
                    dbSQL = "SELECT * FROM [tbl_Persone] WHERE [Stato]=" & ObjectStatus.OBJECT_VALID & " AND [ID] In (" & buffer.ToString & ")"
                    dbRis = APPConn.ExecuteReader(dbSQL)
                    While dbRis.Read
                        Dim tipoPersona As TipoPersona = Formats.ToInteger(dbRis("TipoPersona"))
                        Dim p As CPersona = Anagrafica.Persone.Instantiate(tipoPersona)
                        APPConn.Load(p, dbRis)
                        ret.Add(p)
                    End While
                    dbRis.Dispose() : dbRis = Nothing
                End If

                Return XML.Utils.Serializer.Serialize(ret)
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
            End Try
        End Function

        Public Function GetIDPersoneContattate(ByVal renderer As Object) As String
            Dim cursor As CCustomerCallsCursor = Nothing
            Dim dbRis As System.Data.IDataReader = Nothing
            Try
                cursor = XML.Utils.Serializer.Deserialize(RPC.n2str(GetParameter(renderer, "c", "")))
                Dim dbSQL As String = "SELECT [IDPersona] FROM (" & cursor.GetSQL & ") GROUP BY [IDPersona]"
                cursor.Dispose() : cursor = Nothing

                dbRis = CustomerCalls.CRM.TelDB.ExecuteReader(dbSQL)
                Dim ret As New CCollection(Of Integer)
                While dbRis.Read
                    Dim id As Integer = Formats.ToInteger(dbRis("IDPersona"))
                    If (id <> 0) Then ret.Add(id)
                End While
                dbRis.Dispose() : dbRis = Nothing

                Return XML.Utils.Serializer.Serialize(ret)
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
            End Try
        End Function

        Public Function AggregatoContatti(ByVal renderer As Object) As String
            Dim cursor As CCustomerCallsCursor = Nothing
            Try
                cursor = XML.Utils.Serializer.Deserialize(RPC.n2str(GetParameter(renderer, "cursor", "")))
                Dim periodo As String = RPC.n2str(GetParameter(renderer, "periodo", ""))
                Dim ret As CKeyCollection(Of CPersonaXPeriodo) = CustomerCalls.CRM.AggregatoContatti(cursor, periodo)
                cursor.Dispose() : cursor = Nothing
                Return XML.Utils.Serializer.Serialize(ret)
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Function
    End Class


End Namespace
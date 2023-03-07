Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD

    
    ''' <summary>
    ''' Gestione delle pratiche di cessione del quinto e delega
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class CPraticheClass
        Inherits CGeneralClass(Of CRapportino)

        ''' <summary>
        ''' Evento generato quando viene generata una condizione di attenzione per la pratica
        ''' </summary>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event PraticaWatch(ByVal e As ItemEventArgs)



        ''' <summary>
        ''' Evento generato quando viene apportata una correzione alla pratica
        ''' </summary>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event PraticaCorretta(ByVal e As ItemEventArgs)


        ''' <summary>
        ''' Evento generato quando viene formulata un'offerta che richiede l'approvazione
        ''' </summary>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event PraticaRequireApprovation(ByVal e As ItemEventArgs)

        ''' <summary>
        ''' Evento generato quando la pratica subisce un pasaggio di stato
        ''' </summary>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event PraticaChangeStatus(ByVal e As ItemEventArgs)


        ''' <summary>
        ''' Evento generato quando viene memorizzata una nuova pratica
        ''' </summary>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event PraticaCreated(ByVal e As ItemEventArgs)

        ''' <summary>
        ''' Evento generato quando viene eliminata una pratica esistente
        ''' </summary>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event PraticaDeleted(ByVal e As ItemEventArgs)

        ''' <summary>
        ''' Evento generato quando viene modificata una pratica
        ''' </summary>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event PraticaModified(ByVal e As ItemEventArgs)

        ''' <summary>
        ''' Evento generato per notificare un evento relativo ad una pratica
        ''' </summary>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event NotifyPratica(ByVal e As ItemEventArgs)

        ''' <summary>
        ''' Evento generato quando una pratica in attesa di conferma viene autorizzata
        ''' </summary>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event PraticaAutorizzata(ByVal e As ItemEventArgs)

        ''' <summary>
        ''' Evento generato quando una pratica in attesa di conferma viene rifiutata
        ''' </summary>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event PraticaRifiutata(ByVal e As ItemEventArgs)


        Public Event PraticaPresaInCarico(e As ItemEventArgs)


        Friend Sub New()
            MyBase.New("modRapportini", GetType(CRapportiniCursor))
        End Sub

        Private m_EventsEnabled As Boolean = True


        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se abilitare o meno gli eventi
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property EventEnabled As Boolean
            Get
                Return m_EventsEnabled
            End Get
            Set(value As Boolean)
                m_EventsEnabled = value
            End Set
        End Property

         


       
        Public Function GetAnomalie(ByVal idUfficio As Integer, ByVal idOperatore As Integer, ByVal dal As Nullable(Of Date), ByVal al As Nullable(Of Date), Optional ByVal ritardoConsentito As Integer = 1) As CCollection(Of OggettoAnomalo)
            Dim ret As New CCollection(Of OggettoAnomalo)

            Dim cursor As CRapportiniCursor = Nothing
            Dim pratica As CRapportino
            'Dim operatore As CUser
            
            Dim stato As CStatoPratica = CQSPD.StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_CONTATTO)
            While (stato IsNot Nothing)
                If (stato.GiorniStallo.HasValue) Then
                    cursor = New CRapportiniCursor
                    cursor.IgnoreRights = True
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                    cursor.IDStatoAttuale.Value = GetID(stato)
                    cursor.Flags.Value = PraticaFlags.HIDDEN Or PraticaFlags.TRASFERITA
                    cursor.Flags.Operator = OP.OP_NE
                    'cursor.IDProduttore.Value = GetID(Anagrafica.Aziende.AziendaPrincipale)
                    If (idUfficio <> 0) Then cursor.IDPuntoOperativo.Value = idUfficio
                    If (idOperatore <> 0) Then cursor.StatoContatto.IDOperatore = idOperatore
                    If (dal.HasValue) Then cursor.StatoContatto.Inizio = dal.Value
                    If (al.HasValue) Then cursor.StatoContatto.Fine = al.Value

                    While Not cursor.EOF
                        pratica = cursor.Item
                        Dim d1 As Date = Nothing
                        If (pratica.StatoDiLavorazioneAttuale.Data.HasValue) Then d1 = pratica.StatoDiLavorazioneAttuale.Data
                        If (Math.Abs(DateUtils.DateDiff(DateInterval.Day, d1, Now)) > stato.GiorniStallo) Then
                            'operatore = pratica.StatoDiLavorazioneAttuale.Operatore
                            'If (operatore Is Nothing) Then operatore = pratica.StatoContatto.Operatore
                            'If (operatore Is Nothing) Then operatore = pratica.CreatoDa
                            Dim op As New OggettoAnomalo
                            op.Oggetto = pratica
                            op.AggiungiAnomalia("La pratica è in stato " & stato.Nome & " da più di " & stato.GiorniStallo & " giorni", 0)
                            ret.Add(op)
                        End If
                        cursor.MoveNext()
                    End While
                    If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

                End If
                stato = stato.DefaultTarget
            End While

            Return ret
        End Function

        ''' <summary>
        ''' Analizza la stringa template e sostituisce i valori
        ''' </summary>
        ''' <param name="template">Testo da elaborare</param>
        ''' <param name="context">Contesto in cui avviene l'elaborazione</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ParseTemplate(ByVal template As String, ByVal context As CKeyCollection) As String
            Dim pratica As CRapportino = context("Pratica")
            Dim currentUser As CUser = context("CurrentUser")
            Dim info As CInfoPratica = pratica.Info

            template = Replace(template, "%%ID%%", pratica.ID)
            template = Replace(template, "%%NUMEROPRATICA%%", pratica.NumeroPratica)
            template = Replace(template, "%%USERNAME%%", currentUser.Nominativo)
            template = Replace(template, "%%NOMINATIVOCLIENTE%%", pratica.NominativoCliente)
            template = Replace(template, "%%NOMEPRODOTTO%%", pratica.NomeProdotto)
            template = Replace(template, "%%NOMEPROFILO%%", pratica.NomeProfilo)
            template = Replace(template, "%%NOMECESSIONARIO%%", pratica.NomeCessionario)
            template = Replace(template, "%%NOMEAMMINISTRAZIONE%%", pratica.Impiego.NomeAzienda)
            template = Replace(template, "%%MONTANTELORDO%%", Formats.FormatValuta(pratica.MontanteLordo))
            template = Replace(template, "%%SPREAD%%", Formats.FormatPercentage(pratica.Spread, 3))
            template = Replace(template, "%%VALORESPREAD%%", Formats.FormatValuta(pratica.ValoreSpread))
            template = Replace(template, "%%UPFRONT%%", Formats.FormatPercentage(pratica.UpFront, 3))
            template = Replace(template, "%%VALOREUPFRONT%%", Formats.FormatValuta(pratica.ValoreUpFront))
            template = Replace(template, "%%RUNNING%%", Formats.FormatPercentage(pratica.Running, 3))
            template = Replace(template, "%%VALORERUNNING%%", Formats.FormatValuta(pratica.ValoreRunning))
            template = Replace(template, "%%PROVVIGIONEMASSIMA%%", Formats.FormatPercentage(pratica.ProvvigioneMassima, 3))
            template = Replace(template, "%%VALOREPROVVIGIONEMASSIMA%%", Formats.FormatValuta(pratica.ValoreProvvigioneMassima))
            template = Replace(template, "%%PROVVIGIONEBROKER%%", Formats.FormatPercentage(pratica.Provvigionale.PercentualeSu(pratica.MontanteLordo), 3))
            template = Replace(template, "%%VALOREPROVVIGIONEBROKER%%", Formats.FormatValuta(pratica.Provvigionale.ValoreTotale))
            template = Replace(template, "%%MOTIVORICHIESTASCONTO%%", pratica.Info.MotivoSconto)
            template = Replace(template, "%%DETTAGLIORICHIESTASCONTO%%", pratica.Info.MotivoScontoDettaglio)

            template = Replace(template, "%%ANZIANITA%%", Formats.FormatInteger(pratica.Anzianita))
            template = Replace(template, "%%NUMEROCELLULARE%%", Formats.FormatPhoneNumber(pratica.Cellulare))
            template = Replace(template, "%%CODICEFISCALE%%", Formats.FormatCodiceFiscale(pratica.CodiceFiscale))
            template = Replace(template, "%%COGNOMECLIENTE%%", pratica.CognomeCliente)

            template = Replace(template, "%%NOMECONSULENTE%%", pratica.NomeConsulente)
            template = Replace(template, "%%COSTO%%", Formats.FormatValuta(info.Costo))
            template = Replace(template, "%%CREATODA%%", pratica.CreatoDa.Nominativo)
            template = Replace(template, "%%CREATOIL%%", Formats.FormatUserDateTime(pratica.CreatoIl))

            If (pratica.OffertaCorrente IsNot Nothing) Then
                template = Replace(template, "%%MOTIVOCONFERMASCONTO%%", pratica.OffertaCorrente.MotivoConfermaSconto)
                template = Replace(template, "%%DETTAGLIOCONFERMASCONTO%%", pratica.OffertaCorrente.DettaglioConfermaSconto)
            Else
                template = Replace(template, "%%MOTIVOCONFERMASCONTO%%", "")
                template = Replace(template, "%%DETTAGLIOCONFERMASCONTO%%", "")
            End If


            If (info IsNot Nothing) Then
                If (info.Commerciale IsNot Nothing) Then
                    template = Replace(template, "%%NOMECOMMERCIALE%%", info.Commerciale.Nominativo)
                Else
                    template = Replace(template, "%%NOMECOMMERCIALE%%", "")
                End If
                template = Replace(template, "%%DATAAGGIORNAMENTOPRATICA%%", Formats.FormatUserDateTime(info.DataAggiornamentoPT))
            Else
                template = Replace(template, "%%NOMECOMMERCIALE%%", "")
                template = Replace(template, "%%DATAAGGIORNAMENTOPRATICA%%", "")
            End If
            template = Replace(template, "%%DATAASSUNZIONE%%", Formats.FormatUserDate(pratica.Impiego.DataAssunzione))
            template = Replace(template, "%%DATADECORRENZA%%", Formats.FormatUserDate(pratica.DataDecorrenza))
            'template = Replace(template, "%%DATALICENZIAMENTO%%", Formats.FormatUserDate(pratica.DataLicenziamento))
            template = Replace(template, "%%DATATRASFERIMENTO%%", Formats.FormatUserDateTime(info.DataTrasferimento))
            template = Replace(template, "%%DAVEDERE%%", IIf(pratica.DaVedere, "Sì", "No"))
            template = Replace(template, "%%EMAIL%%", pratica.eMail)
            If pratica.Impiego.EntePagante IsNot Nothing Then
                template = Replace(template, "%%NOMEENTEPAGANTE%%", pratica.Impiego.EntePagante.Nominativo)
            Else
                template = Replace(template, "%%NOMEENTEPAGANTE%%", "")
            End If
            template = Replace(template, "%%ETA%%", Formats.FormatInteger(pratica.Eta))
            template = Replace(template, "%%ETAFF%%", Formats.FormatInteger(pratica.EtaFineFinanziamento))
            template = Replace(template, "%%FAX%%", Formats.FormatPhoneNumber(pratica.Fax))
            template = Replace(template, "%%STATOATTUALE%%", pratica.StatoAttuale.Nome)
            If pratica.StatoDiLavorazioneAttuale IsNot Nothing AndAlso pratica.StatoDiLavorazioneAttuale.FromStato IsNot Nothing Then
                template = Replace(template, "%%STATOPRECEDENTE%%", pratica.StatoDiLavorazioneAttuale.FromStato.DescrizioneStato)
                template = Replace(template, "%%NOTESTATOATTUALE%%", pratica.StatoDiLavorazioneAttuale.Note)
            Else
                template = Replace(template, "%%STATOPRECEDENTE%%", "")
                template = Replace(template, "%%NOTESTATOATTUALE%%", "")
            End If
            template = Replace(template, "%%BASEURL%%", ApplicationContext.BaseURL)

            If (pratica.StatoAnnullata IsNot Nothing) Then
                template = Replace(template, "%%MOTIVOANNULLAMENTO%%", pratica.StatoAnnullata.Note)
            Else
                template = Replace(template, "%%MOTIVOANNULLAMENTO%%", "")
            End If

            template = Replace(template, "%%CONTEXTMESSAGE%%", context.GetItemByKey("Message"))
            Return template
        End Function

        ''' <summary>
        ''' Invia una notifica agli utenti abilitati
        ''' </summary>
        ''' <param name="pratica"></param>
        ''' <remarks></remarks>
        Public Sub Notifica(ByVal pratica As CRapportino)
            If (m_EventsEnabled) Then RaiseEvent NotifyPratica(New ItemEventArgs(pratica))
        End Sub

        ''' <summary>
        ''' Invia una notifica agli utenti abilitati
        ''' </summary>
        ''' <param name="pratica"></param>
        ''' <remarks></remarks>
        Public Sub Autorizza(ByVal pratica As CRapportino)
            If (m_EventsEnabled) Then RaiseEvent PraticaAutorizzata(New ItemEventArgs(pratica))
        End Sub

        Friend Sub DoOnCreate(ByVal e As ItemEventArgs)
            MyBase.doItemCreated(e)

            Dim pratica As CRapportino = e.Item


            Dim cliente As CPersona = pratica.Cliente
            'Dim info As CustomerCalls.CPersonStats = CustomerCalls.CRM.getp
            If (Not cliente.GetFlag(PFlags.Cliente).HasValue OrElse cliente.GetFlag(PFlags.Cliente) = False) Then
                cliente.SetFlag(PFlags.Cliente, True)
                cliente.Save()
            End If

            Dim info As CustomerCalls.CPersonStats = CustomerCalls.CRM.GetContattoInfo(cliente)
            info.isClienteInAcquisizione = False
            info.isClienteAcquisito = True
            info.AggiornaOperazione(pratica, "Pratica " & pratica.NumeroPratica & " caricata")

            RaiseEvent PraticaCreated(New ItemEventArgs(pratica))
            Me.Module.DispatchEvent(New EventDescription("Create", "Creata la pratica N°" & pratica.NumeroPratica, pratica))
        End Sub

        Friend Sub DoOnDelete(ByVal e As ItemEventArgs)
            MyBase.doItemDeleted(e)
            Dim pratica As CRapportino = e.Item

            RaiseEvent PraticaDeleted(New ItemEventArgs(pratica))
            Me.Module.DispatchEvent(New EventDescription("Delete", "Eliminata la pratica N°" & pratica.NumeroPratica, pratica))

        End Sub

        'Friend Sub DoOnSave(ByVal pratica As CRapportino)
        '    If (m_EventsEnabled) Then RaiseEvent PraticaModified(New ItemEventArgs(pratica))
        'End Sub

        ''' <summary>
        ''' Restituisce un oggetto CAllowedRemoteIPs che specifica se l'IP è autorizzato o meno a inviare una pratica
        ''' </summary>
        ''' <param name="ip"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetRemoteIPInfo(ByVal ip As String) As CAllowedRemoteIPs
            Dim cursor As New CAllowedRemoteIPsCursor
            Dim ret As CAllowedRemoteIPs
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.RemoteIP.Value = ip
            cursor.PageSize = 1
            cursor.IgnoreRights = True
            ret = cursor.Item
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

            Return ret
        End Function

        ''' <summary>
        ''' Formatta il dettaglio
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function FormatDettaglioStato(ByVal value As Integer) As String
            Select Case value
                Case 0 : Return "Altro"
                Case 1 : Return "Annullato dal cliente"
                Case 2 : Return "Non fattibile"
                Case Else : Return "Sconosciuto"
            End Select
        End Function



        ''' <summary>
        ''' Restituisce la pratica in base al suo ID nel sistema del cessionario
        ''' </summary>
        ''' <param name="cessionario"></param>
        ''' <param name="numero"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetItemByNumeroEsterno(ByVal cessionario As CCQSPDCessionarioClass, ByVal numero As String) As CRapportino
            numero = Trim(numero)
            If (numero = vbNullString) Then Return Nothing
            Dim cursor As New CRapportiniCursor
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.IDCessionario.Value = GetID(cessionario)
            cursor.WhereClauses.Add("[StatRichD_Params]=" & DBUtils.DBString(numero))
            Dim ret As CRapportino = cursor.Item
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

            Return ret
        End Function

        Public Function GetArrayProfiliEsterni(ByVal cessionario As CCQSPDCessionarioClass, Optional ByVal onlyValid As Boolean = True) As CCollection(Of CProfilo)
            If (cessionario Is Nothing) Then Throw New ArgumentNullException("cessionartio")
            Dim items As CCollection(Of CProfilo)
            Dim ret As New CCollection(Of CProfilo)
            items = Profili.GetPreventivatoriUtente(onlyValid)
            For i As Integer = 0 To items.Count - 1
                Dim item As CProfilo = items(i)
                If item.IDCessionario = GetID(cessionario) Then ret.Add(item)
            Next
            Return ret
        End Function


        Public Function GetArrayProfiliEsterni(ByVal cessionario As CCQSPDCessionarioClass, ByVal dataDecorrenza As Date, Optional ByVal onlyValid As Boolean = True) As CCollection(Of CProfilo)
            If (cessionario Is Nothing) Then Throw New ArgumentNullException("cessionartio")
            Dim items As CCollection(Of CProfilo)
            Dim ret As New CCollection(Of CProfilo)
            items = CQSPD.Profili.GetPreventivatoriUtente(dataDecorrenza, onlyValid)
            For i As Integer = 0 To items.Count - 1
                Dim item As CProfilo = items(i)
                If item.IDCessionario = GetID(cessionario) Then ret.Add(item)
            Next
            Return ret
        End Function


        Public Function FormatStatoPratica(ByVal value As Nullable(Of StatoPraticaEnum)) As String
            If (value.HasValue = False) Then Return ""
            Dim ret As String
            Select Case value.Value
                Case StatoPraticaEnum.STATO_ARCHIVIATA : ret = "Archiviata"
                Case StatoPraticaEnum.STATO_ANNULLATA : ret = "Annullata"
                Case StatoPraticaEnum.STATO_LIQUIDATA : ret = "Perfezionata"
                Case StatoPraticaEnum.STATO_PRONTALIQUIDAZIONE : ret = "Pronta per liquidazione"
                Case StatoPraticaEnum.STATO_RICHIESTADELIBERA : ret = "Richiesta delibera"
                Case StatoPraticaEnum.STATO_DELIBERATA : ret = "Deliberata"
                Case Else : ret = value.Value
            End Select
            Return ret
        End Function

        Public Function ParseStatoPratica(ByVal value As String) As StatoPraticaEnum
            Select Case LCase(Trim(value))
                Case "archiviata" : Return StatoPraticaEnum.STATO_ARCHIVIATA
                Case "annullata" : Return StatoPraticaEnum.STATO_ANNULLATA
                Case "perfezionata" : Return StatoPraticaEnum.STATO_LIQUIDATA
                Case "richiesta delibera" : Return StatoPraticaEnum.STATO_RICHIESTADELIBERA
                Case "pronta per liquidazione" : Return StatoPraticaEnum.STATO_PRONTALIQUIDAZIONE
                Case "deliberata" : Return StatoPraticaEnum.STATO_DELIBERATA
                Case Else : Return StatoPraticaEnum.STATO_CONTATTO
            End Select
        End Function



        Public Function GetStatistichePerStatoCL( _
                            ByVal dbSQL As String, _
                            ByVal numRows As Integer, _
                            ByVal conta As Integer, _
                            ByRef sommaML As Decimal, _
                            ByRef aveProvvBrk As Double, _
                            ByRef aveSpread As Double, _
                            ByRef aveProvvTotale As Double, _
                            ByRef sommaMLfil As Decimal, _
                            ByRef sommaMLBrk As Decimal, _
                            ByRef costoFisso As Decimal, _
                            ByRef rappel As Decimal _
                            ) As Boolean
            Dim dbRis As System.Data.IDataReader
            Dim dbSQL1 As String

            dbSQL1 = "SELECT Count(*) As [conta], Sum(IIF([MontanteLordo] Is Null, 0, [MontanteLordo])*IIF([Rappel] Is Null, 0, [Rappel])/100) As [Rappel], Sum([Costo]) As [CostoFisso], Sum(IIF([MontanteLordo] Is Null, 0, [MontanteLordo])*IIF([ProvvBroker] Is Null, 0, [ProvvBroker])/100) As [sommaMLBrk],  Sum(IIF([MontanteLordo] Is Null, 0, [MontanteLordo])*(IIF([Spread] Is Null, 0, [Spread])+IIF([SpreadSotto] Is Null,0,[SpreadSotto]))/100) As [sommaMLfil], Sum([MontanteLordo]) As [sommaML], Sum([ProvvBroker]) As [aveProvvBrk], Sum([Spread]) As [aveSpread], Sum(IIF([ProvvBroker] Is Null, 0, [ProvvBroker])+IIF([Spread] Is Null, 0, [Spread])) As [aveProvvTotale] FROM (" & dbSQL & ") WHERE ([StatoPratica]>=" & StatoPraticaEnum.STATO_RICHIESTADELIBERA & ") And ([StatoPratica]<=" & StatoPraticaEnum.STATO_LIQUIDATA & ")"

            dbRis = CQSPD.Database.ExecuteReader(dbSQL1)

            If dbRis.Read Then
                sommaML = Formats.ToValuta(dbRis("sommaML"))
                aveProvvBrk = Formats.ToDouble(dbRis("aveProvvBrk")) / numRows
                aveSpread = Formats.ToDouble(dbRis("aveSpread")) / numRows
                aveProvvTotale = Formats.ToDouble(dbRis("aveProvvTotale")) / numRows
                sommaMLfil = Formats.ToValuta(dbRis("sommaMLfil"))
                sommaMLBrk = Formats.ToValuta(dbRis("sommaMLBrk"))
                costoFisso = Formats.ToValuta(dbRis("CostoFisso"))
                rappel = Formats.ToDouble(dbRis("Rappel"))
                conta = Formats.ToDouble(dbRis("conta"))
            Else
                sommaML = 0
                aveProvvBrk = 0
                aveSpread = 0
                aveProvvTotale = 0
                sommaMLfil = 0
                sommaMLBrk = 0
                costoFisso = 0
                rappel = 0
                conta = 0
            End If
            dbRis.Dispose()
            dbRis = Nothing

            Return True
        End Function

        Public Function GetTransferCommand(ByVal destination As String) As String
            Dim dbRis As System.Data.IDataReader
            Dim dbSQL As String
            Dim ret As String
            ret = ""
            destination = Replace(destination, "&nbsp;", " ")
            destination = Trim(Replace(destination, "'", "''"))
            dbSQL = "SELECT * FROM [tbl_Rapportini_Trasferisci] WHERE ([Descrizione]='" & destination & "')"

            dbRis = CQSPD.Database.ExecuteReader(dbSQL)
            If dbRis.Read Then
                ret = Formats.ToString(dbRis("URL"))
            End If
            dbRis.Dispose()
            dbRis = Nothing
            Return ret
        End Function

        ''' <summary>
        ''' Restituisce una collezione di pratiche 
        ''' </summary>
        ''' <param name="persona"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetPraticheByPersona(ByVal persona As CPersonaFisica) As CCollection(Of CRapportino)
            If (persona Is Nothing) Then Throw New ArgumentNullException("persona")
            Dim ret As New CCollection(Of CRapportino)
            If (GetID(persona) = 0) Then Return ret

            Dim cursor As New CRapportiniCursor()
            cursor.IDCliente.Value = GetID(persona)
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.IgnoreRights = True
            While (Not cursor.EOF)
                Dim p As CRapportino = cursor.Item
                p.SetCliente(persona)
                ret.Add(p)
                cursor.MoveNext()
            End While
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

            Return ret
        End Function

        Public Function GetPraticheByPersona(ByVal idPersona As Integer) As CCollection(Of CRapportino)
            Return Me.GetPraticheByPersona(Anagrafica.Persone.GetItemById(idPersona))
        End Function

        ''' <summary>
        ''' Restituisce una collezione di pratiche 
        ''' </summary>
        ''' <param name="azienda"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetPraticheByAzienda(ByVal azienda As CAzienda) As CCollection(Of CRapportino)
            If (azienda Is Nothing) Then Throw New ArgumentNullException("persona")
            Dim ret As New CCollection(Of CRapportino)
            If (GetID(azienda) <> 0) Then
                Dim cursor As New CRapportiniCursor()
                cursor.IDAmministrazione.Value = GetID(azienda)
                cursor.StatoPratica.SortOrder = SortEnum.SORT_ASC
                cursor.StatoPratica.SortPriority = 99
                cursor.Nominativo.SortOrder = SortEnum.SORT_ASC
                cursor.Nominativo.SortPriority = 98
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.IgnoreRights = True
                While (Not cursor.EOF)
                    ret.Add(cursor.Item)
                    cursor.MoveNext()
                End While
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End If
            Return ret
        End Function

        Protected Friend Sub DoOnInCarico(e As ItemEventArgs)
            RaiseEvent PraticaPresaInCarico(e)
        End Sub

        Friend Sub DoOnApprovata(ByVal e As ItemEventArgs)
            Dim pratica As CRapportino = e.Item
            Dim cliente As CPersona = pratica.Cliente
            'Dim info As CustomerCalls.CPersonStats = CustomerCalls.CRM.getp
            If (Not cliente.GetFlag(PFlags.Cliente).HasValue OrElse cliente.GetFlag(PFlags.Cliente) = False) Then
                cliente.SetFlag(PFlags.Cliente, True)
                cliente.Save()
            End If

            Dim info As CustomerCalls.CPersonStats = CustomerCalls.CRM.GetContattoInfo(cliente)
            info.isClienteInAcquisizione = False
            info.isClienteAcquisito = True
            info.AggiornaOperazione(pratica, "Pratica " & pratica.NumeroPratica & " offerta autorizzata")

            RaiseEvent PraticaAutorizzata(e)
        End Sub

        Friend Sub DoOnRifiutata(ByVal e As ItemEventArgs)
            Dim pratica As CRapportino = e.Item
            Dim cliente As CPersona = pratica.Cliente
            'Dim info As CustomerCalls.CPersonStats = CustomerCalls.CRM.getp
            If (Not cliente.GetFlag(PFlags.Cliente).HasValue OrElse cliente.GetFlag(PFlags.Cliente) = False) Then
                cliente.SetFlag(PFlags.Cliente, True)
                cliente.Save()
            End If

            Dim info As CustomerCalls.CPersonStats = CustomerCalls.CRM.GetContattoInfo(cliente)
            info.isClienteInAcquisizione = False
            info.isClienteAcquisito = True
            info.Note = "Pratica " & pratica.NumeroPratica & " offerta negata"
            info.Save()

            RaiseEvent PraticaRifiutata(e)
        End Sub

        Friend Sub DoOnModified(ByVal e As ItemEventArgs)
            MyBase.doItemModified(e)
            RaiseEvent PraticaModified(e)
        End Sub

        Friend Sub DoOnWatch(e As ItemEventArgs)
            Dim pratica As CRapportino = e.Item
            Dim cliente As CPersona = pratica.Cliente
            'Dim info As CustomerCalls.CPersonStats = CustomerCalls.CRM.getp
            If (Not cliente.GetFlag(PFlags.Cliente).HasValue OrElse cliente.GetFlag(PFlags.Cliente) = False) Then
                cliente.SetFlag(PFlags.Cliente, True)
                cliente.Save()
            End If

            Dim info As CustomerCalls.CPersonStats = CustomerCalls.CRM.GetContattoInfo(cliente)
            info.isClienteInAcquisizione = False
            info.isClienteAcquisito = True
            info.AggiornaOperazione(pratica, "Pratica " & pratica.NumeroPratica & " in condizione di attenzione")
            RaiseEvent PraticaWatch(e)
        End Sub

        Friend Sub DoOnChangeStatus(ByVal e As ItemEventArgs)
            Dim pratica As CRapportino = e.Item
            Dim cliente As CPersona = pratica.Cliente
            'Dim info As CustomerCalls.CPersonStats = CustomerCalls.CRM.getp
            If (Not cliente.GetFlag(PFlags.Cliente).HasValue OrElse cliente.GetFlag(PFlags.Cliente) = False) Then
                cliente.SetFlag(PFlags.Cliente, True)
                cliente.Save()
            End If
            Dim info As CustomerCalls.CPersonStats = CustomerCalls.CRM.GetContattoInfo(cliente)
            info.AggiornaOperazione(pratica, "Pratica " & pratica.NumeroPratica & " in stato: " & pratica.StatoAttuale.Nome & ", Motivo: " & pratica.StatoDiLavorazioneAttuale.Note)
            RaiseEvent PraticaChangeStatus(e)
            Me.Module.DispatchEvent(New EventDescription("change_status", "Pratica N°" & pratica.NumeroPratica & " in stato " & pratica.StatoDiLavorazioneAttuale.DescrizioneStato, pratica))

        End Sub

        Friend Sub DoOnRequireApprovation(ByVal e As ItemEventArgs)
            Dim pratica As CRapportino = e.Item
            Dim cliente As CPersona = pratica.Cliente
            'Dim info As CustomerCalls.CPersonStats = CustomerCalls.CRM.getp
            If (Not cliente.GetFlag(PFlags.Cliente).HasValue OrElse cliente.GetFlag(PFlags.Cliente) = False) Then
                cliente.SetFlag(PFlags.Cliente, True)
                cliente.Save()
            End If
            Dim info As CustomerCalls.CPersonStats = CustomerCalls.CRM.GetContattoInfo(cliente)
            info.AggiornaOperazione(pratica, "Pratica " & pratica.NumeroPratica & " in richiesta approvazione")
            RaiseEvent PraticaRequireApprovation(e)
            Me.Module.DispatchEvent(New EventDescription("require_approvation", Users.CurrentUser.Nominativo & " richiede l'approvazione dell'offerta fatta per la pratica N°" & pratica.NumeroPratica, pratica))

        End Sub

        Friend Sub DoOnCorretta(ByVal e As ItemEventArgs)
            RaiseEvent PraticaCorretta(e)
        End Sub

        Function CalcolaProssimaDecorrenza() As Date
            Return Me.CalcolaProssimaDecorrenza(DateUtils.ToDay)
        End Function

        Function CalcolaProssimaDecorrenza(ByVal fromDate As Date) As Date
            Dim ret As Date = fromDate
            If (ret.Day <= 15) Then
                ret = DateUtils.MakeDate(ret.Year, ret.Month, 15)
            Else
                ret = DateUtils.GetNextMonthFirstDay(ret)
            End If
            Return ret
        End Function

        Public Sub RielaboraRinnovabili()

        End Sub

        Public Function GetPraticheByProposta(ByVal proposta As CQSPDConsulenza) As CCollection(Of CRapportino)
            If (proposta Is Nothing) Then Throw New ArgumentNullException("proposta")

            Dim ret As New CCollection(Of CRapportino)
            If (GetID(proposta) = 0) Then Return ret

            Dim cursor As New CRapportiniCursor
            'If Not (Users.Module.UserCanDoAction("bypess_azienda")) Then cursor.IDProduttore.Value = GetID(Anagrafica.Aziende.AziendaPrincipale)
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.IDConsulenza.Value = GetID(proposta)
            While Not cursor.EOF
                ret.Add(cursor.Item)
                cursor.MoveNext()
            End While
            cursor.Dispose()

            Return ret
        End Function

        Public Function GetFastStats(ByVal filter As CQSFilter) As CQSFastStats
            Return New CQSFastStats(filter)
        End Function

        Function GetSecci(ufficio As CUfficio, operatore As CUser, di As Date?, df As Date?) As CCollection(Of CRapportino)
            Dim cursor As New CRapportiniCursor
            Dim ret As New CCollection(Of CRapportino)
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.StatoPratica.Value = StatoPraticaEnum.STATO_CONTATTO
            'cursor.Nominativo.SortOrder = SortEnum.SORT_ASC
            cursor.Trasferita.Value = False
            cursor.Flags.Value = PraticaFlags.HIDDEN
            cursor.Flags.Operator = OP.OP_NE

            If (ufficio IsNot Nothing) Then cursor.IDPuntoOperativo.Value = GetID(ufficio)
            If (operatore IsNot Nothing) Then cursor.StatoContatto.IDOperatore = GetID(operatore)


            If (di.HasValue OrElse df.HasValue) Then
                cursor.StatoContatto.Tipo = "Tra"
                cursor.StatoContatto.Inizio = di
                cursor.StatoContatto.Fine = df
            End If

            While Not cursor.EOF
                ret.Add(cursor.Item)
                cursor.MoveNext()
            End While
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing


            Return ret
        End Function

        

        Public Function GetPraticheInCorso(ByVal ufficio As CUfficio, ByVal operatore As CUser, ByVal dataInizio As Date?, ByVal dataFine As Date?) As CCollection(Of CRapportino)
            Dim ret As New CCollection(Of CRapportino)

            Dim cursor As CRapportiniCursor = Nothing
            
            Dim stato As CStatoPratica = CQSPD.StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_CONTATTO)
            Dim stArch As CStatoPratica = CQSPD.StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_ARCHIVIATA)
            Dim stAnn As CStatoPratica = CQSPD.StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_ANNULLATA)

            Dim d1 As Date?

            While (stato IsNot Nothing)
                If (stato Is stArch) Then
                    'Pratica archiviata


                ElseIf (stato Is stAnn) Then
                    'Pratica annullata
                     

                ElseIf (stato.GiorniStallo.HasValue) Then
                    cursor = New CRapportiniCursor
                    cursor.IgnoreRights = True
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                    cursor.IDStatoAttuale.Value = GetID(stato)
                    cursor.Trasferita.Value = False
                    cursor.Flags.Value = PraticaFlags.HIDDEN
                    cursor.Flags.Operator = OP.OP_NE
                    'cursor.IDProduttore.Value = GetID(Anagrafica.Aziende.AziendaPrincipale)
                    cursor.StatoContatto.MacroStato = Nothing
                    cursor.StatoContatto.Tipo = "Tra"
                    cursor.StatoContatto.Inizio = dataInizio
                    cursor.StatoContatto.Fine = dataFine
                    If (operatore IsNot Nothing) Then cursor.StatoContatto.IDOperatore = GetID(operatore)
                    If (ufficio IsNot Nothing) Then cursor.IDPuntoOperativo.Value = GetID(ufficio)

                    While Not cursor.EOF
                        Dim pratica As CRapportino = cursor.Item
                        d1 = pratica.StatoDiLavorazioneAttuale.Data

                        If (d1.HasValue AndAlso Math.Abs(DateUtils.DateDiff(DateInterval.Day, d1.Value, Now)) > stato.GiorniStallo) Then
                            ret.Add(pratica)
                        End If
                        cursor.MoveNext()
                    End While

                    If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
                End If

                stato = stato.DefaultTarget
            End While
             
            Return ret
        End Function

        Public Function GetPraticheConcluse(ByVal ufficio As CUfficio, ByVal operatore As CUser, ByVal dataInizio As Date?, ByVal dataFine As Date?) As CCollection(Of CRapportino)
            Dim ret As New CCollection(Of CRapportino)

            
            Dim stArch As CStatoPratica = CQSPD.StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_ARCHIVIATA)
            Dim stAnn As CStatoPratica = CQSPD.StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_ANNULLATA)

            Dim cursor As CRapportiniCursor = New CRapportiniCursor
            cursor.IgnoreRights = True
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.IDStatoAttuale.ValueIn({GetID(stAnn), GetID(stArch)})
            cursor.Trasferita.Value = False
            cursor.Flags.Value = PraticaFlags.HIDDEN
            cursor.Flags.Operator = OP.OP_NE
            'cursor.IDProduttore.Value = GetID(Anagrafica.Aziende.AziendaPrincipale)
            cursor.StatoContatto.MacroStato = Nothing
            cursor.StatoContatto.Tipo = "Tra"
            cursor.StatoContatto.Inizio = dataInizio
            cursor.StatoContatto.Fine = dataFine
            If (operatore IsNot Nothing) Then cursor.StatoContatto.IDOperatore = GetID(operatore)
            If (ufficio IsNot Nothing) Then cursor.IDPuntoOperativo.Value = GetID(ufficio)

            While Not cursor.EOF
                Dim pratica As CRapportino
                pratica = cursor.Item
                ret.Add(pratica)
                cursor.MoveNext()
            End While

            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing


            Return ret
        End Function

        Public Overrides Function GetItemById(id As Integer) As CRapportino
            Return MyBase.GetItemById(id)
        End Function
       
        ''' <summary>
        ''' Effettua le statistiche aggregate sul cursore
        ''' </summary>
        ''' <param name="cursor"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CalcolaSomme(ByVal cursor As CRapportiniCursor) As CQSPDMLInfo
            Dim ret As New CQSPDMLInfo

            Dim dbSQL As String
            dbSQL = "SELECT Count(*) As [Num], " & _
                    "Sum([MontanteLordo]) As [SML], " & _
                    "Sum([UpFront]) As [SUpFront], " & _
                    "Sum([UpFront]+[Running]) As [SSpread], " & _
                    "Sum([Running]) As [SRunning], " & _
                    "Sum(iif([ProvvMax] Is Null Or [ProvvMax]<=[UpFront], 0, [ProvvMax]-[UpFront])) As [SSconto], " & _
                    "Sum([Rappel]) As [SRappel] " & _
            "FROM (" & cursor.GetSQL & ")"

            Dim dbRis As System.Data.IDataReader = CQSPD.Database.ExecuteReader(dbSQL)
            If (dbRis.Read) Then
                ret.Conteggio = Formats.ToInteger(dbRis("Num"))
                ret.ML = Formats.ToValuta(dbRis("SML"))
                ret.UpFront = Formats.ToValuta(dbRis("SUpFront"))
                ret.Spread = Formats.ToValuta(dbRis("SSPread"))
                ret.Running = Formats.ToValuta(dbRis("SRunning"))
                ret.Sconto = Formats.ToValuta(dbRis("SSconto"))
                ret.Rappel = Formats.ToValuta(dbRis("SRappel"))
            End If
            dbRis.Dispose()


            Return ret
        End Function

        Public Sub SyncStatiLav(ByVal pratiche As CCollection(Of CRapportino))
            Dim buffer As New System.Collections.ArrayList
            For Each r As CRapportino In pratiche
                buffer.Add(GetID(r))
            Next
            If (buffer.Count > 0) Then
                Dim col As New CCollection(Of CStatoLavorazionePratica)
                Dim cursor As New CStatiLavorazionePraticaCursor
                Dim arrp() As Integer = buffer.ToArray(GetType(Integer))
                cursor.IDPratica.ValueIn(arrp)
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.IgnoreRights = True
                While Not cursor.EOF
                    Dim stl As CStatoLavorazionePratica = cursor.Item
                    col.Add(stl)
                    cursor.MoveNext()
                End While
                cursor.Dispose()

                For Each r As CRapportino In pratiche
                    Dim statilav As New CStatiLavorazionePraticaCollection
                    statilav.SetPratica(r)
                    For Each stl In col
                        If stl.IDPratica = GetID(r) Then
                            statilav.Add(stl)
                        End If
                    Next
                    statilav.Sort()
                    r.SetStatiDiLavorazione(statilav)
                    Dim sta As CStatoLavorazionePratica = statilav.GetItemById(r.IDStatoDiLavorazioneAttuale)
                    If (sta IsNot Nothing) Then r.SetStatoDiLavorazioneAttuale(sta)
                Next
            End If
        End Sub

        Public Sub SyncOffertaCorrente(ByVal pratiche As CCollection(Of CRapportino))
            Dim idList As New System.Collections.ArrayList
            For Each p As CRapportino In pratiche
                If (p.IDOffertaCorrente <> 0) Then idList.Add(p.IDOffertaCorrente)
            Next

            If (idList.Count > 0) Then
                Dim arr() As Integer = idList.ToArray(GetType(Integer))
                Dim cursor As New CCQSPDOfferteCursor
                cursor.ID.ValueIn(arr)
                cursor.IgnoreRights = True
                While Not cursor.EOF
                    Dim o As COffertaCQS = cursor.Item
                    For Each p As CRapportino In pratiche
                        If (p.IDOffertaCorrente = GetID(o)) Then
                            p.SetOffertaCorrente(o)
                            o.SetPratica(p)
                        End If
                    Next
                    cursor.MoveNext()
                End While
                cursor.Dispose()
            End If
        End Sub


    End Class

    Private Shared m_Pratiche As CPraticheClass = Nothing

    Public Shared ReadOnly Property Pratiche As CPraticheClass
        Get
            If (m_Pratiche Is Nothing) Then m_Pratiche = New CPraticheClass
            Return m_Pratiche
        End Get
    End Property

End Class

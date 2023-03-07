Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.CustomerCalls



Public NotInheritable Class CQSPD

    ''' <summary>
    ''' Evento generato quando viene impostato il DB del modulo CQSPD
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Shared Event DatabaseChanged(ByVal sender As Object, ByVal e As System.EventArgs)

    Private Shared m_Initialized As Boolean = False
    Private Shared m_Module As CModule
    Private Shared m_GruppoOperatori As CGroup = Nothing
    Private Shared m_GruppoReferenti As CGroup = Nothing
    Private Shared m_GruppoSupervisori As CGroup = Nothing
    Private Shared m_GruppoAutorizzatori As CGroup = Nothing
    Private Shared m_GruppoConsulenti As CGroup = Nothing
    Private Shared m_Database As CDBConnection

    Private Sub New()
        DMD.DMDObject.IncreaseCounter(Me)
    End Sub




    ' Private Shared m_Lock As New Object
    Private Shared Function GetConfigProvider(ByVal id As Integer) As Object
        Return CQSPD.Configuration
    End Function

    ''' <summary>
    ''' Inizializza il modulo
    ''' </summary>
    ''' <remarks></remarks>
    Private Shared Sub Initialize()
        'SyncLock m_Lock
        If (m_Initialized) Then Exit Sub
        m_Initialized = True

        AddHandler Anagrafica.PersonaMerged, AddressOf HandlePersonaMerged
        AddHandler Anagrafica.PersonaUnMerged, AddressOf HandlePeronaUnMerged
        AddHandler Anagrafica.PersonaModified, AddressOf HandlePersonaModified
        AddHandler Anagrafica.PersonaCreated, AddressOf HandlePersonaCreated



        AddHandler CustomerCalls.Visite.ItemCreated, AddressOf HandleContatto
        AddHandler CustomerCalls.Visite.ItemDeleted, AddressOf HandleContatto
        AddHandler CustomerCalls.Visite.ItemModified, AddressOf HandleContatto
        AddHandler CustomerCalls.Telefonate.ItemCreated, AddressOf HandleContatto
        AddHandler CustomerCalls.Telefonate.ItemDeleted, AddressOf HandleContatto
        AddHandler CustomerCalls.Telefonate.ItemModified, AddressOf HandleContatto

        AddHandler Sistema.Attachments.ItemCreated, AddressOf HandleAttachment
        AddHandler Sistema.Attachments.ItemDeleted, AddressOf HandleAttachment
        AddHandler Sistema.Attachments.ItemModified, AddressOf HandleAttachment

        AddHandler Office.RichiesteCERQ.ItemCreated, AddressOf HandleRichiesta
        AddHandler Office.RichiesteCERQ.ItemDeleted, AddressOf HandleRichiesta
        AddHandler Office.RichiesteCERQ.ItemModified, AddressOf HandleRichiesta


        AddHandler CQSPD.Estinzioni.ItemCreated, AddressOf HandleAltroPrestito
        AddHandler CQSPD.Estinzioni.ItemDeleted, AddressOf HandleAltroPrestito
        AddHandler CQSPD.Estinzioni.ItemModified, AddressOf HandleAltroPrestito


        AddHandler CQSPD.RichiesteFinanziamento.ItemCreated, AddressOf HandleRichiestaFinanziamento
        AddHandler CQSPD.RichiesteFinanziamento.ItemDeleted, AddressOf HandleRichiestaFinanziamento
        AddHandler CQSPD.RichiesteFinanziamento.ItemModified, AddressOf HandleRichiestaFinanziamento

        AddHandler CQSPD.RichiesteConteggi.ItemCreated, AddressOf HandleRichiestaConteggi
        AddHandler CQSPD.RichiesteConteggi.ItemDeleted, AddressOf HandleRichiestaConteggi
        AddHandler CQSPD.RichiesteConteggi.ItemModified, AddressOf HandleRichiestaConteggi

        AddHandler CQSPD.Consulenze.ItemCreated, AddressOf HandleOfferta
        AddHandler CQSPD.Consulenze.ItemDeleted, AddressOf HandleOfferta
        AddHandler CQSPD.Consulenze.ItemModified, AddressOf HandleOfferta

        AddHandler CQSPD.Pratiche.ItemCreated, AddressOf HandlePratica
        AddHandler CQSPD.Pratiche.ItemDeleted, AddressOf HandlePratica
        AddHandler CQSPD.Pratiche.ItemModified, AddressOf HandlePratica



        Sistema.Types.RegisteredTypeProviders.Add("CEstinzione", AddressOf CQSPD.Estinzioni.GetItemById)
        Sistema.Types.RegisteredTypeProviders.Add("CRapportino", AddressOf CQSPD.Pratiche.GetItemById)
        Sistema.Types.RegisteredTypeProviders.Add("CQSPDConsulenza", AddressOf CQSPD.Consulenze.GetItemById)
        Sistema.Types.RegisteredTypeProviders.Add("CCQSPDConfig", AddressOf GetConfigProvider)
        Sistema.Types.RegisteredTypeProviders.Add("CImportExportSource", AddressOf CQSPD.ImportExportSources.GetItemById)
        Sistema.Types.RegisteredTypeProviders.Add("CImportExport", AddressOf CQSPD.ImportExport.GetItemById)


        Dim lista As CListaRicontatti = ListeRicontatto.GetItemByName("Altri Prestiti Rinnovabili")
        If (lista Is Nothing) Then
            lista = New CListaRicontatti
            lista.Name = "Altri Prestiti Rinnovabili"
            lista.Stato = ObjectStatus.OBJECT_VALID
            lista.Descrizione = "Lista predefinita per i rinnovi degli altri prestiti"
            lista.Save()
        End If

        Dim cnt As Integer = 0
        cnt += GruppoOperatori.Members.Count
        cnt += GruppoAutorizzatori.Members.Count
        cnt += GruppoReferenti.Members.Count
        cnt += GruppoSupervisori.Members.Count
        cnt += GruppoConsulenti.Members.Count

        CQSPD.Estinzioni.Initialize()
        CQSPD.Pratiche.Initialize()
        CQSPD.ImportExportSources.Initialize()
        CQSPD.ImportExport.Initialize()


        'End SyncLock
    End Sub

    ''' <summary>
    ''' Inizializza il modulo
    ''' </summary>
    ''' <remarks></remarks>
    Private Shared Sub Terminate()
        'SyncLock m_Lock
        If (Not m_Initialized) Then Exit Sub

        RemoveHandler Anagrafica.PersonaMerged, AddressOf HandlePersonaMerged
        RemoveHandler Anagrafica.PersonaUnMerged, AddressOf HandlePeronaUnMerged
        RemoveHandler Anagrafica.PersonaModified, AddressOf HandlePersonaModified
        RemoveHandler Anagrafica.PersonaCreated, AddressOf HandlePersonaCreated



        RemoveHandler CustomerCalls.Visite.ItemCreated, AddressOf HandleContatto
        RemoveHandler CustomerCalls.Visite.ItemDeleted, AddressOf HandleContatto
        RemoveHandler CustomerCalls.Visite.ItemModified, AddressOf HandleContatto
        RemoveHandler CustomerCalls.Telefonate.ItemCreated, AddressOf HandleContatto
        RemoveHandler CustomerCalls.Telefonate.ItemDeleted, AddressOf HandleContatto
        RemoveHandler CustomerCalls.Telefonate.ItemModified, AddressOf HandleContatto

        RemoveHandler Sistema.Attachments.ItemCreated, AddressOf HandleAttachment
        RemoveHandler Sistema.Attachments.ItemDeleted, AddressOf HandleAttachment
        RemoveHandler Sistema.Attachments.ItemModified, AddressOf HandleAttachment

        RemoveHandler Office.RichiesteCERQ.ItemCreated, AddressOf HandleRichiesta
        RemoveHandler Office.RichiesteCERQ.ItemDeleted, AddressOf HandleRichiesta
        RemoveHandler Office.RichiesteCERQ.ItemModified, AddressOf HandleRichiesta


        RemoveHandler CQSPD.Estinzioni.ItemCreated, AddressOf HandleAltroPrestito
        RemoveHandler CQSPD.Estinzioni.ItemDeleted, AddressOf HandleAltroPrestito
        RemoveHandler CQSPD.Estinzioni.ItemModified, AddressOf HandleAltroPrestito


        RemoveHandler CQSPD.RichiesteFinanziamento.ItemCreated, AddressOf HandleRichiestaFinanziamento
        RemoveHandler CQSPD.RichiesteFinanziamento.ItemDeleted, AddressOf HandleRichiestaFinanziamento
        RemoveHandler CQSPD.RichiesteFinanziamento.ItemModified, AddressOf HandleRichiestaFinanziamento

        RemoveHandler CQSPD.RichiesteConteggi.ItemCreated, AddressOf HandleRichiestaConteggi
        RemoveHandler CQSPD.RichiesteConteggi.ItemDeleted, AddressOf HandleRichiestaConteggi
        RemoveHandler CQSPD.RichiesteConteggi.ItemModified, AddressOf HandleRichiestaConteggi

        RemoveHandler CQSPD.Consulenze.ItemCreated, AddressOf HandleOfferta
        RemoveHandler CQSPD.Consulenze.ItemDeleted, AddressOf HandleOfferta
        RemoveHandler CQSPD.Consulenze.ItemModified, AddressOf HandleOfferta

        RemoveHandler CQSPD.Pratiche.ItemCreated, AddressOf HandlePratica
        RemoveHandler CQSPD.Pratiche.ItemDeleted, AddressOf HandlePratica
        RemoveHandler CQSPD.Pratiche.ItemModified, AddressOf HandlePratica



        Sistema.Types.RegisteredTypeProviders.RemoveByKey("CEstinzione")
        Sistema.Types.RegisteredTypeProviders.RemoveByKey("CRapportino")
        Sistema.Types.RegisteredTypeProviders.RemoveByKey("CQSPDConsulenza")
        Sistema.Types.RegisteredTypeProviders.RemoveByKey("CCQSPDConfig")
        Sistema.Types.RegisteredTypeProviders.RemoveByKey("CImportExportSource")
        Sistema.Types.RegisteredTypeProviders.RemoveByKey("CImportExport")

        CQSPD.ImportExportSources.Terminate()
        CQSPD.ImportExport.Terminate()
        CQSPD.Pratiche.Terminate()
        CQSPD.Estinzioni.Terminate()
        'End SyncLock
    End Sub

    ''' <summary>
    ''' Restituisce o imposta il database specifico per il modulo CQSPD
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Property Database As CDBConnection
        Get
            If (m_Database Is Nothing) Then Return APPConn
            Return m_Database
        End Get
        Set(value As CDBConnection)
            If (value Is m_Database) Then Exit Property
            Terminate()
            m_Database = value
            Initialize()
            RaiseEvent DatabaseChanged(Nothing, New System.EventArgs)
        End Set
    End Property

    Private Shared Function IsDocumentoDiReddito(ByVal a As CAttachment) As Boolean
        Dim cat As String = LCase(Trim(a.Categoria))
        Select Case cat
            Case "busta paga", "obis m", "cud inpdap", "cud" : Return True
            Case Else : Return False
        End Select
    End Function

    Private Shared Sub HandleAttachment(ByVal sender As Object, ByVal e As ItemEventArgs)
        'Dim att As CAttachment = e.Item
        'If (att.Stato = ObjectStatus.OBJECT_VALID AndAlso IsDocumentoDiReddito(att) AndAlso att.IDOwner <> 0 AndAlso att.OwnerType = "CPersonaFisica") Then
        '    Dim p As CPersonaFisica = Anagrafica.Persone.GetItemById(att.IDOwner)
        '    Dim w As FinestraLavorazione = CQSPD.FinestreDiLavorazione.GetFinestraCorrente(p)
        '    If (w Is Nothing) Then w = CQSPD.FinestreDiLavorazione.GetProssimaFinestra(p)
        '    If (w IsNot Nothing) Then
        '        If w.StatoFinestra = StatoFinestraLavorazione.NonAperta Then
        '            If (Calendar.CheckBetween(att.DataInizio, w.DataInizioLavorabilita, w.DataFineLavorabilita, 3600 * 24)) Then
        '                w.StatoFinestra = StatoFinestraLavorazione.Aperta
        '                w.DataInizioLavorazione = Calendar.Now
        '                w.DataUltimoAggiornamento = Calendar.Now
        '                w.BustaPaga = att
        '                w.DataBustaPaga = Calendar.Now
        '                w.StatoBustaPaga = StatoOfferteFL.Liquidata
        '                w.Save()
        '            End If
        '        ElseIf w.StatoFinestra <> StatoFinestraLavorazione.Chiusa Then
        '            If (Calendar.CheckBetween(att.DataInizio, w.DataInizioLavorazione, w.DataFineLavorazione, 3600 * 24)) Then
        '                w.StatoFinestra = StatoFinestraLavorazione.Aperta
        '                w.DataUltimoAggiornamento = Calendar.Now
        '                w.BustaPaga = att
        '                w.DataBustaPaga = Calendar.Now
        '                w.StatoBustaPaga = StatoOfferteFL.Liquidata
        '                w.Save()
        '            End If
        '        End If
        '    End If

        '    'Dim cursor As New ClientiLavoratiStatsItemCursor
        '    'Dim data As Date
        '    'If (att.DataInizio.HasValue) Then
        '    '    data = att.DataInizio.Value
        '    'Else
        '    '    data = att.CreatoIl
        '    'End If

        '    'cursor.IDCliente.Value = att.IDOwner
        '    'cursor.DataInizioLavorazione.SortOrder = SortEnum.SORT_DESC
        '    'cursor.Stato.Value = ObjectStatus.OBJECT_VALID

        '    'Dim statsItem As ClientiLavoratiStatsItem = cursor.Item
        '    'cursor.Dispose()
        '    'If (statsItem Is Nothing) Then
        '    '    statsItem = New ClientiLavoratiStatsItem
        '    '    statsItem.Cliente = Anagrafica.Persone.GetItemById(att.IDOwner)
        '    '    statsItem.Stato = ObjectStatus.OBJECT_VALID
        '    '    statsItem.DataInizioLavorazione = data
        '    '    statsItem.DataUltimoAggiornamento = data
        '    'Else
        '    '    Select Case statsItem.SottostatoLavorazione
        '    '        Case SottostatoLavorazione.None, SottostatoLavorazione.InLavorazione, SottostatoLavorazione.InAttesa
        '    '        Case Else
        '    '            'FIX
        '    '            If (statsItem.DataUltimoAggiornamento.HasValue = False) Then statsItem.DataUltimoAggiornamento = statsItem.DataInizioLavorazione

        '    '            If (Math.Abs(Calendar.DateDiff(DateInterval.Month, statsItem.DataUltimoAggiornamento.Value, data)) >= 3) Then
        '    '                statsItem = New ClientiLavoratiStatsItem
        '    '                statsItem.Cliente = Anagrafica.Persone.GetItemById(att.IDOwner)
        '    '                statsItem.Stato = ObjectStatus.OBJECT_VALID
        '    '                statsItem.DataInizioLavorazione = data
        '    '                statsItem.DataUltimoAggiornamento = data
        '    '            End If
        '    '    End Select
        '    'End If
        '    'statsItem.NotifyBustaPaga(att)
        '    'statsItem.Save()

        'End If
    End Sub

    Private Shared Function IsRichiestaCE(ByVal rich As Office.RichiestaCERQ) As Boolean
        Select Case LCase(Trim(rich.TipoRichiesta))
            Case "richiesta ce", "certificato di stipendio", "cud"
                Return True
            Case Else
                Return False
        End Select
    End Function

    Private Shared Function WGetStatoRichestaCERQ(ByVal rich As Office.RichiestaCERQ) As StatoOfferteFL
        Select Case rich.StatoOperazione
            Case Office.StatoRichiestaCERQ.ANNULLATA : Return StatoOfferteFL.BocciataAgenzia
            Case Office.StatoRichiestaCERQ.DA_RICHIEDERE : Return StatoOfferteFL.InLavorazione
            Case Office.StatoRichiestaCERQ.RICHIESTA : Return StatoOfferteFL.InLavorazione
            Case Office.StatoRichiestaCERQ.RIFIUTATA : Return StatoOfferteFL.BocciataCessionario
            Case Office.StatoRichiestaCERQ.RITIRATA : Return StatoOfferteFL.Liquidata
        End Select
        Return StatoOfferteFL.Sconosciuto
    End Function

    Private Shared Sub HandleRichiesta(ByVal sender As Object, ByVal e As ItemEventArgs)
        'Dim rich As Office.RichiestaCERQ = e.Item

        'If rich.Stato <> ObjectStatus.OBJECT_VALID OrElse Not IsRichiestaCE(rich) Then Exit Sub

        'Dim p As CPersonaFisica = rich.Cliente
        'If (p Is Nothing) Then Exit Sub

        'Dim w As FinestraLavorazione = CQSPD.FinestreDiLavorazione.GetFinestraCorrente(p)
        'If (w Is Nothing) Then w = CQSPD.FinestreDiLavorazione.GetProssimaFinestra(p)
        'If (w IsNot Nothing) Then
        '    If w.StatoFinestra = StatoFinestraLavorazione.NonAperta Then
        '        If (Calendar.CheckBetween(rich.Data, w.DataInizioLavorabilita, w.DataFineLavorabilita, 3600 * 24)) Then
        '            w.StatoFinestra = StatoFinestraLavorazione.Aperta
        '            w.DataInizioLavorazione = Calendar.Now
        '            w.DataUltimoAggiornamento = Calendar.Now
        '            w.RichiestaCertificato = rich
        '            w.DataRichiestaCertificato = Calendar.Now
        '            w.StatoRichiestaCertificato = WGetStatoRichestaCERQ(rich)
        '            w.Save()
        '        End If
        '    ElseIf w.StatoFinestra <> StatoFinestraLavorazione.Chiusa Then
        '        If (Calendar.CheckBetween(rich.Data, w.DataInizioLavorazione, w.DataFineLavorazione, 3600 * 24)) Then
        '            w.StatoFinestra = StatoFinestraLavorazione.Aperta
        '            w.DataUltimoAggiornamento = Calendar.Now
        '            w.RichiestaCertificato = rich
        '            w.DataRichiestaCertificato = Calendar.Now
        '            w.StatoRichiestaCertificato = WGetStatoRichestaCERQ(rich)
        '            w.Save()
        '        End If
        '    End If
        'End If
    End Sub

    Private Shared Sub HandleContatto(ByVal sender As Object, ByVal e As ItemEventArgs)
        ''If (TypeOf (e.Item) Is CVisita) Then
        'Dim visita As CContattoUtente = e.Item
        'If (visita.Stato <> ObjectStatus.OBJECT_VALID) Then Return

        'Dim persona As CPersona
        'If (visita.IDPerContoDi <> 0) Then
        '    persona = visita.PerContoDi
        'Else
        '    persona = visita.Persona
        'End If
        'If Not (TypeOf (persona) Is CPersonaFisica) Then Return

        'Dim w As FinestraLavorazione = CQSPD.FinestreDiLavorazione.GetFinestraCorrente(persona)
        'If (w Is Nothing) Then w = CQSPD.FinestreDiLavorazione.GetProssimaFinestra(persona)

        'If (w Is Nothing) Then Return

        'w.NotificaContatto(visita)

        ''Dim cursor As New ClientiLavoratiStatsItemCursor
        ''cursor.IDCliente.Value = GetID(persona)
        ''cursor.DataInizioLavorazione.SortOrder = SortEnum.SORT_DESC
        ''cursor.Stato.Value = ObjectStatus.OBJECT_VALID

        ''Dim statsItem As ClientiLavoratiStatsItem = cursor.Item
        ''cursor.Dispose()
        ''If (statsItem Is Nothing) Then
        ''    statsItem = New ClientiLavoratiStatsItem
        ''    statsItem.Cliente = persona
        ''    statsItem.Stato = ObjectStatus.OBJECT_VALID
        ''    statsItem.DataInizioLavorazione = visita.Data
        ''    statsItem.DataUltimoAggiornamento = visita.Data
        ''Else
        ''    Select Case statsItem.SottostatoLavorazione
        ''        Case SottostatoLavorazione.None, SottostatoLavorazione.InLavorazione, SottostatoLavorazione.InAttesa
        ''        Case Else
        ''            'FIX
        ''            If (statsItem.DataUltimoAggiornamento.HasValue = False) Then statsItem.DataUltimoAggiornamento = statsItem.DataInizioLavorazione

        ''            If (Math.Abs(Calendar.DateDiff(DateInterval.Month, statsItem.DataUltimoAggiornamento.Value, visita.Data)) >= 3) Then
        ''                statsItem = New ClientiLavoratiStatsItem
        ''                statsItem.Cliente = visita.Persona
        ''                statsItem.Stato = ObjectStatus.OBJECT_VALID
        ''                statsItem.DataInizioLavorazione = visita.Data
        ''                statsItem.DataUltimoAggiornamento = visita.Data
        ''            End If
        ''    End Select
        ''End If
        ''statsItem.NotifyVisita(visita)
        ''statsItem.Save()

        ''End If
    End Sub

    Private Shared Sub HandleAltroPrestito(ByVal sender As Object, ByVal e As ItemEventArgs)
        'Dim est As CEstinzione = e.Item
        'If est.Persona Is Nothing OrElse Not TypeOf (est.Persona) Is CPersonaFisica Then Exit Sub
        'Dim persona As CPersonaFisica = est.Persona
        'Dim w As FinestraLavorazione = CQSPD.FinestreDiLavorazione.GetFinestraCorrente(persona)
        'If (w Is Nothing) Then w = CQSPD.FinestreDiLavorazione.GetProssimaFinestra(persona)
        'If (w Is Nothing) Then Exit Sub


        'Dim dataRinnovo As Date? = Nothing
        'Dim isRinnovo As Boolean = False
        'Dim dIni As Date? = Nothing

        'Dim ultima As FinestraLavorazione = CQSPD.FinestreDiLavorazione.GetUltimaFinestraLavorata(persona)
        'If Not (ultima Is Nothing) Then dataRinnovo = ultima.DataFineLavorazione

        'Dim prestiti As CCollection(Of CEstinzione) = CQSPD.Estinzioni.GetPrestitiAttivi(persona)
        'Dim minPrestito As Date? = Nothing

        'If (w IsNot Nothing) Then
        '    dIni = w.DataInizioLavorazione
        '    If (dIni.HasValue = False) Then dIni = w.DataInizioLavorabilita
        'End If

        'isRinnovo = prestiti.Count > 0
        'For Each p As CEstinzione In prestiti
        '    If (dIni.HasValue) Then
        '        If (p.DataRinnovo.HasValue AndAlso p.DataRinnovo.Value > dIni.Value) Then
        '            minPrestito = Calendar.Min(minPrestito, p.DataRinnovo)
        '        End If
        '    Else
        '        minPrestito = Calendar.Min(minPrestito, p.DataRinnovo)
        '    End If
        'Next

        'dataRinnovo = Calendar.Max(minPrestito, dataRinnovo)
        'If (dataRinnovo.HasValue = False) Then Return

        'w.SetFlag(FinestraLavorazioneFlags.Rinnovo, isRinnovo)
        'w.DataInizioLavorabilita = dataRinnovo.Value
        'w.Save()
    End Sub

    Private Shared Sub HandleRichiestaFinanziamento(ByVal sender As Object, ByVal e As ItemEventArgs)
        'Dim rich As CRichiestaFinanziamento = e.Item
        'Dim cliente As CPersona = rich.Cliente
        'If (cliente Is Nothing) Then Exit Sub
        'Dim w As FinestraLavorazione = CQSPD.FinestreDiLavorazione.GetFinestra(cliente, rich.Data)
        'If (w Is Nothing) Then w = CQSPD.FinestreDiLavorazione.GetProssimaFinestra(cliente)
        'If (w Is Nothing) Then Exit Sub

        'w.NotificaRichiesta(rich)

        ''Dim cursor As New ClientiLavoratiStatsItemCursor
        ''cursor.IDCliente.Value = rich.IDCliente
        ''cursor.DataInizioLavorazione.SortOrder = SortEnum.SORT_DESC
        ''cursor.Stato.Value = ObjectStatus.OBJECT_VALID
        ''Dim statsItem As ClientiLavoratiStatsItem = cursor.Item
        ''cursor.Dispose()
        ''If (statsItem Is Nothing) Then
        ''    statsItem = New ClientiLavoratiStatsItem
        ''    statsItem.Cliente = rich.Cliente
        ''    statsItem.Stato = ObjectStatus.OBJECT_VALID
        ''    statsItem.DataInizioLavorazione = rich.Data
        ''    statsItem.DataUltimoAggiornamento = rich.Data
        ''Else
        ''    Select Case statsItem.SottostatoLavorazione
        ''        Case SottostatoLavorazione.None, SottostatoLavorazione.InLavorazione, SottostatoLavorazione.InAttesa
        ''        Case Else
        ''            'FIX
        ''            If (statsItem.DataUltimoAggiornamento.HasValue = False) Then statsItem.DataUltimoAggiornamento = statsItem.DataInizioLavorazione

        ''            If (Math.Abs(Calendar.DateDiff(DateInterval.Month, statsItem.DataUltimoAggiornamento.Value, rich.Data)) >= 3) Then
        ''                statsItem = New ClientiLavoratiStatsItem
        ''                statsItem.Cliente = rich.Cliente
        ''                statsItem.Stato = ObjectStatus.OBJECT_VALID
        ''                statsItem.DataInizioLavorazione = rich.Data
        ''                statsItem.DataUltimoAggiornamento = rich.Data
        ''            End If
        ''    End Select
        ''End If
        ''statsItem.NotifyRichiestaFinanziamento(rich)
        ''statsItem.Save()
    End Sub


    Private Shared Sub HandleRichiestaConteggi(ByVal sender As Object, ByVal e As ItemEventArgs)
        Dim rich As CRichiestaConteggio = e.Item
        'Dim cursor As New ClientiLavoratiStatsItemCursor
        'Dim data As Date
        'If (rich.DataRichiesta.HasValue) Then
        '    data = rich.DataRichiesta.Value
        'Else
        '    data = rich.CreatoIl
        'End If

        'cursor.IDCliente.Value = rich.IDCliente
        'cursor.DataInizioLavorazione.SortOrder = SortEnum.SORT_DESC
        'cursor.Stato.Value = ObjectStatus.OBJECT_VALID
        'Dim statsItem As ClientiLavoratiStatsItem = cursor.Item
        'cursor.Dispose()
        'If (statsItem Is Nothing) Then
        '    statsItem = New ClientiLavoratiStatsItem
        '    statsItem.Cliente = rich.Cliente
        '    statsItem.Stato = ObjectStatus.OBJECT_VALID
        '    statsItem.DataInizioLavorazione = data
        '    statsItem.DataUltimoAggiornamento = data
        'Else
        '    Select Case statsItem.SottostatoLavorazione
        '        Case SottostatoLavorazione.None, SottostatoLavorazione.InLavorazione, SottostatoLavorazione.InAttesa
        '        Case Else
        '            'FIX
        '            If (statsItem.DataUltimoAggiornamento.HasValue = False) Then statsItem.DataUltimoAggiornamento = statsItem.DataInizioLavorazione

        '            If (Math.Abs(Calendar.DateDiff(DateInterval.Month, statsItem.DataUltimoAggiornamento.Value, data)) >= 3) Then
        '                statsItem = New ClientiLavoratiStatsItem
        '                statsItem.Cliente = rich.Cliente
        '                statsItem.Stato = ObjectStatus.OBJECT_VALID
        '                statsItem.DataInizioLavorazione = data
        '                statsItem.DataUltimoAggiornamento = data
        '            End If
        '    End Select
        'End If
        'statsItem.NotifyRichiestaConteggio(rich)
        'statsItem.Save()
    End Sub

    Private Shared Sub HandleOfferta(ByVal sender As Object, ByVal e As ItemEventArgs)
        Dim cons As CQSPDConsulenza = e.Item
        Dim conn As CDBConnection = CQSPD.Database
        If (conn.IsRemote) Then
        Else
            If (cons.Stato = ObjectStatus.OBJECT_DELETED) Then
                conn.ExecuteCommand("UPDATE [tbl_CQSPDRichiesteApprovazione] SET [StatoRichiesta]=" & StatoRichiestaApprovazione.ANNULLATA & ", [DettaglioConferma]='Studio di fattibilità eliminato' WHERE [Stato]=" & ObjectStatus.OBJECT_DELETED & " AND [StatoRichiesta]<=" & StatoRichiestaApprovazione.PRESAINCARICO & " AND [IDOggettoApprovabile]=" & GetID(cons) & " AND [TipoOggettoApprovabile]=" & DBUtils.DBString(TypeName(cons)))
            ElseIf (cons.StatoConsulenza = StatiConsulenza.BOCCIATA) Then
                conn.ExecuteCommand("UPDATE [tbl_CQSPDRichiesteApprovazione] SET [StatoRichiesta]=" & StatoRichiestaApprovazione.ANNULLATA & ", [DettaglioConferma]='Studio di fattibilità bocciato' WHERE [Stato]=" & ObjectStatus.OBJECT_DELETED & " AND [StatoRichiesta]<=" & StatoRichiestaApprovazione.PRESAINCARICO & " AND [IDOggettoApprovabile]=" & GetID(cons) & " AND [TipoOggettoApprovabile]=" & DBUtils.DBString(TypeName(cons)))
            ElseIf (cons.StatoConsulenza = StatiConsulenza.RIFIUTATA) Then
                conn.ExecuteCommand("UPDATE [tbl_CQSPDRichiesteApprovazione] SET [StatoRichiesta]=" & StatoRichiestaApprovazione.ANNULLATA & ", [DettaglioConferma]='Studio di fattibilità rifiutato' WHERE [Stato]=" & ObjectStatus.OBJECT_DELETED & " AND [StatoRichiesta]<=" & StatoRichiestaApprovazione.PRESAINCARICO & " AND [IDOggettoApprovabile]=" & GetID(cons) & " AND [TipoOggettoApprovabile]=" & DBUtils.DBString(TypeName(cons)))
            ElseIf (cons.StatoConsulenza = StatiConsulenza.NONFATTIBILE) Then
                conn.ExecuteCommand("UPDATE [tbl_CQSPDRichiesteApprovazione] SET [StatoRichiesta]=" & StatoRichiestaApprovazione.ANNULLATA & ", [DettaglioConferma]='Studio di fattibilità non fattibile' WHERE [Stato]=" & ObjectStatus.OBJECT_DELETED & " AND [StatoRichiesta]<=" & StatoRichiestaApprovazione.PRESAINCARICO & " AND [IDOggettoApprovabile]=" & GetID(cons) & " AND [TipoOggettoApprovabile]=" & DBUtils.DBString(TypeName(cons)))

            End If

        End If

        'Dim w As FinestraLavorazione = CQSPD.FinestreDiLavorazione.GetFinestraCorrente(cons.IDCliente)
        'If w Is Nothing Then w = CQSPD.FinestreDiLavorazione.GetProssimaFinestra(cons.IDCliente)
        'If (w IsNot Nothing) Then
        '    w.NotificaProposta(cons)
        'End If

        ''Dim cursor As New ClientiLavoratiStatsItemCursor
        ''cursor.IDCliente.Value = cons.IDCliente
        ''cursor.DataInizioLavorazione.SortOrder = SortEnum.SORT_DESC
        ''cursor.Stato.Value = ObjectStatus.OBJECT_VALID
        ''Dim statsItem As ClientiLavoratiStatsItem = cursor.Item
        ''cursor.Dispose()
        ''If (statsItem Is Nothing) Then
        ''    statsItem = New ClientiLavoratiStatsItem
        ''    statsItem.Cliente = cons.Cliente
        ''    statsItem.Stato = ObjectStatus.OBJECT_VALID
        ''    statsItem.DataInizioLavorazione = cons.DataConsulenza
        ''    statsItem.DataUltimoAggiornamento = cons.DataConsulenza
        ''Else
        ''    'Select Case statsItem.SottostatoLavorazione
        ''    '    Case SottostatoLavorazione.None, SottostatoLavorazione.InLavorazione, SottostatoLavorazione.InAttesa
        ''    '    Case Else
        ''    '        If (Math.Abs(Calendar.DateDiff(DateInterval.Month, statsItem.DataUltimoAggiornamento.Value, Data)) >= 3) Then
        ''    '            statsItem = New ClientiLavoratiStatsItem
        ''    '            statsItem.Cliente = rich.Cliente
        ''    '            statsItem.Stato = ObjectStatus.OBJECT_VALID
        ''    '            statsItem.DataInizioLavorazione = Data
        ''    '        End If
        ''    'End Select
        ''End If
        ''statsItem.NotifyConsulenza(cons)
        ''statsItem.Save()
    End Sub

    Private Shared Sub HandlePratica(ByVal sender As Object, ByVal e As ItemEventArgs)
        Dim pratica As CRapportino = e.Item
        Dim conn As CDBConnection = CQSPD.Database
        If (conn.IsRemote) Then
        Else
            If (pratica.Stato = ObjectStatus.OBJECT_DELETED) Then
                conn.ExecuteCommand("UPDATE [tbl_CQSPDRichiesteApprovazione] SET [StatoRichiesta]=" & StatoRichiestaApprovazione.ANNULLATA & ", [DettaglioConferma]='La pratica è stata eliminata' WHERE [Stato]=" & ObjectStatus.OBJECT_DELETED & " AND [StatoRichiesta]<=" & StatoRichiestaApprovazione.PRESAINCARICO & " AND [IDOggettoApprovabile]=" & GetID(pratica) & " AND [TipoOggettoApprovabile]=" & DBUtils.DBString(TypeName(pratica)))
            ElseIf (pratica.StatoAttuale IsNot Nothing) AndAlso (pratica.StatoAttuale.MacroStato.HasValue) AndAlso (pratica.StatoAttuale.MacroStato = StatoPraticaEnum.STATO_ANNULLATA) Then
                conn.ExecuteCommand("UPDATE [tbl_CQSPDRichiesteApprovazione] SET [StatoRichiesta]=" & StatoRichiestaApprovazione.ANNULLATA & ", [DettaglioConferma]='La pratica è stata annullata' WHERE [Stato]=" & ObjectStatus.OBJECT_DELETED & " AND [StatoRichiesta]<=" & StatoRichiestaApprovazione.PRESAINCARICO & " AND [IDOggettoApprovabile]=" & GetID(pratica) & " AND [TipoOggettoApprovabile]=" & DBUtils.DBString(TypeName(pratica)))
            End If


        End If

    End Sub

    ''' <summary>
    ''' Gestisce l'evento "PersonaMerged" del modulo anagrafica in modo da mantenere consistenti i dati nelle tabelle
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Shared Sub HandlePersonaMerged(ByVal e As MergePersonaEventArgs)
        SyncLock Anagrafica.lock
            Dim mi As CMergePersona = e.MI
            Dim persona As CPersona = mi.Persona1
            Dim persona1 As CPersona = mi.Persona2

            Dim dbSQL As String
            Dim dbRis As System.Data.IDataReader
            Dim rec As CMergePersonaRecord

            'Tabella tbl_AreaManagers 
            dbSQL = "Select [ID] FROM [tbl_AreaManagers] WHERE [Persona]=" & GetID(persona1)
            dbRis = Database.ExecuteReader(dbSQL)
            While dbRis.Read
                rec = New CMergePersonaRecord
                rec.NomeTabella = "tbl_AreaManagers"
                rec.FieldName = "Persona"
                rec.RecordID = Formats.ToInteger(dbRis("ID"))
                mi.TabelleModificate.Add(rec)
            End While
            dbRis.Dispose()
            Database.ExecuteCommand("UPDATE [tbl_AreaManagers] Set [Persona]=" & GetID(persona) & " WHERE [Persona]=" & GetID(persona1))

            'Tabella tbl_Collaboratori 
            dbSQL = "Select [ID] FROM [tbl_Collaboratori] WHERE [Persona]=" & GetID(persona1)
            dbRis = Database.ExecuteReader(dbSQL)
            While dbRis.Read
                rec = New CMergePersonaRecord
                rec.NomeTabella = "tbl_Collaboratori"
                rec.FieldName = "Persona"
                rec.RecordID = Formats.ToInteger(dbRis("ID"))
                mi.TabelleModificate.Add(rec)
            End While
            dbRis.Dispose()
            CQSPD.Database.ExecuteCommand("UPDATE [tbl_Collaboratori] Set [Persona]=" & GetID(persona) & ", NomePersona = " & DBUtils.DBString(persona.Nominativo) & " WHERE [Persona]=" & GetID(persona1))

            'Tabella tbl_EstinzioniXPersona 
            dbSQL = "Select [ID] FROM [tbl_EstinzioniXPersona] WHERE [Persona]=" & GetID(persona1)
            dbRis = Database.ExecuteReader(dbSQL)
            While dbRis.Read
                rec = New CMergePersonaRecord
                rec.NomeTabella = "tbl_EstinzioniXPersona"
                rec.FieldName = "Persona"
                rec.RecordID = Formats.ToInteger(dbRis("ID"))
                mi.TabelleModificate.Add(rec)
            End While
            dbRis.Dispose()
            CQSPD.Database.ExecuteCommand("UPDATE [tbl_EstinzioniXPersona] Set [Persona]=" & GetID(persona) & " WHERE [Persona]=" & GetID(persona1))

            'Tabella tbl_TeamManagers 
            dbSQL = "Select [ID] FROM [tbl_TeamManagers] WHERE [Persona]=" & GetID(persona1)
            dbRis = Database.ExecuteReader(dbSQL)
            While dbRis.Read
                rec = New CMergePersonaRecord
                rec.NomeTabella = "tbl_TeamManagers"
                rec.FieldName = "Persona"
                rec.RecordID = Formats.ToInteger(dbRis("ID"))
                mi.TabelleModificate.Add(rec)
            End While
            dbRis.Dispose()
            CQSPD.Database.ExecuteCommand("UPDATE [tbl_TeamManagers] Set [Persona]=" & GetID(persona) & " WHERE [Persona]=" & GetID(persona1))


            If TypeOf (persona) Is CPersonaFisica Then
                With DirectCast(persona, CPersonaFisica)
                    'Tabella tbl_Rapportini 
                    dbSQL = "Select [ID] FROM [tbl_Rapportini] WHERE [Cliente]=" & GetID(persona1)
                    dbRis = Database.ExecuteReader(dbSQL)
                    While dbRis.Read
                        rec = New CMergePersonaRecord
                        rec.NomeTabella = "tbl_Rapportini"
                        rec.FieldName = "Cliente"
                        rec.RecordID = Formats.ToInteger(dbRis("ID"))
                        mi.TabelleModificate.Add(rec)
                    End While
                    dbRis.Dispose()
                    CQSPD.Database.ExecuteCommand("UPDATE [tbl_Rapportini] Set [Cliente]=" & GetID(persona) & ", [NomeCliente] = " & DBUtils.DBString(.Nome) & ", [CognomeCliente] = " & DBUtils.DBString(.Cognome) & " WHERE [Cliente]=" & GetID(persona1))
                End With

                'Tabella tbl_CQSPDRichCERQ 
                dbSQL = "Select [ID] FROM [tbl_CQSPDRichCERQ] WHERE [IDCliente]=" & GetID(persona1)
                dbRis = Database.ExecuteReader(dbSQL)
                While dbRis.Read
                    rec = New CMergePersonaRecord
                    rec.NomeTabella = "tbl_CQSPDRichCERQ"
                    rec.FieldName = "IDCliente"
                    rec.RecordID = Formats.ToInteger(dbRis("ID"))
                    mi.TabelleModificate.Add(rec)
                End While
                dbRis.Dispose()
                CQSPD.Database.ExecuteCommand("UPDATE [tbl_CQSPDRichCERQ] Set [IDCliente]=" & GetID(persona) & ", [NomeCliente] = " & DBUtils.DBString(persona.Nominativo) & " WHERE [IDCliente]=" & GetID(persona1))
            Else
                'Tabella tbl_Rapportini 
                dbSQL = "Select [ID] FROM [tbl_Rapportini] WHERE [IDAmministrazione]=" & GetID(persona1)
                dbRis = Database.ExecuteReader(dbSQL)
                While dbRis.Read
                    rec = New CMergePersonaRecord
                    rec.NomeTabella = "tbl_Rapportini"
                    rec.FieldName = "IDAmministrazione"
                    rec.RecordID = Formats.ToInteger(dbRis("ID"))
                    mi.TabelleModificate.Add(rec)
                End While
                dbRis.Dispose()
                CQSPD.Database.ExecuteCommand("UPDATE [tbl_Rapportini] Set [IDAmministrazione]=" & GetID(persona) & ", [Ente] = " & DBUtils.DBString(persona.Nominativo) & " WHERE [IDAmministrazione]=" & GetID(persona1))

                'Tabella tbl_Rapportini 
                dbSQL = "Select [ID] FROM [tbl_Rapportini] WHERE [IDEntePagante]=" & GetID(persona1)
                dbRis = Database.ExecuteReader(dbSQL)
                While dbRis.Read
                    rec = New CMergePersonaRecord
                    rec.NomeTabella = "tbl_Rapportini"
                    rec.FieldName = "IDEntePagante"
                    rec.RecordID = Formats.ToInteger(dbRis("ID"))
                    mi.TabelleModificate.Add(rec)
                End While
                dbRis.Dispose()
                CQSPD.Database.ExecuteCommand("UPDATE [tbl_Rapportini] Set [IDEntePagante]=" & GetID(persona) & ", [NomeEntePagante] = " & DBUtils.DBString(persona.Nominativo) & " WHERE [IDEntePagante]=" & GetID(persona1))

                'Tabella tbl_Rapportini 
                dbSQL = "Select [ID] FROM [tbl_CQSPDRichCERQ] WHERE [IDAmministrazione]=" & GetID(persona1)
                dbRis = Database.ExecuteReader(dbSQL)
                While dbRis.Read
                    rec = New CMergePersonaRecord
                    rec.NomeTabella = "tbl_CQSPDRichCERQ"
                    rec.FieldName = "IDAmministrazione"
                    rec.RecordID = Formats.ToInteger(dbRis("ID"))
                    mi.TabelleModificate.Add(rec)
                End While
                dbRis.Dispose()
                CQSPD.Database.ExecuteCommand("UPDATE [tbl_CQSPDRichCERQ] Set [IDAmministrazione]=" & GetID(persona) & ", [NomeAmministrazione] = " & DBUtils.DBString(persona.Nominativo) & " WHERE [IDAmministrazione]=" & GetID(persona1))
            End If

            'Tabella tbl_RichiesteFinanziamenti 
            dbSQL = "Select [ID] FROM [tbl_RichiesteFinanziamenti] WHERE [IDCliente]=" & GetID(persona1)
            dbRis = Database.ExecuteReader(dbSQL)
            While dbRis.Read
                rec = New CMergePersonaRecord
                rec.NomeTabella = "tbl_RichiesteFinanziamenti"
                rec.FieldName = "IDCliente"
                rec.RecordID = Formats.ToInteger(dbRis("ID"))
                mi.TabelleModificate.Add(rec)
            End While
            dbRis.Dispose()
            CQSPD.Database.ExecuteCommand("UPDATE [tbl_RichiesteFinanziamenti] Set [IDCliente]=" & GetID(persona) & ", [NomeCliente] = " & DBUtils.DBString(persona.Nominativo) & " WHERE [IDCliente]=" & GetID(persona1))

            'Tabella tbl_Preventivi_Offerte 
            dbSQL = "Select [ID] FROM [tbl_Preventivi_Offerte] WHERE [IDCliente]=" & GetID(persona1)
            dbRis = Database.ExecuteReader(dbSQL)
            While dbRis.Read
                rec = New CMergePersonaRecord
                rec.NomeTabella = "tbl_Preventivi_Offerte"
                rec.FieldName = "IDCliente"
                rec.RecordID = Formats.ToInteger(dbRis("ID"))
                mi.TabelleModificate.Add(rec)
            End While
            dbRis.Dispose()
            CQSPD.Database.ExecuteCommand("UPDATE [tbl_Preventivi_Offerte] Set [IDCliente]=" & GetID(persona) & " WHERE [IDCliente]=" & GetID(persona1))

            'Tabella tbl_Estinzioni 
            dbSQL = "Select [ID] FROM [tbl_Estinzioni] WHERE [IDPersona]=" & GetID(persona1)
            dbRis = Database.ExecuteReader(dbSQL)
            While dbRis.Read
                rec = New CMergePersonaRecord
                rec.NomeTabella = "tbl_Estinzioni"
                rec.FieldName = "IDPersona"
                rec.RecordID = Formats.ToInteger(dbRis("ID"))
                mi.TabelleModificate.Add(rec)
            End While
            dbRis.Dispose()
            CQSPD.Database.ExecuteCommand("UPDATE [tbl_Estinzioni] Set [IDPersona]=" & GetID(persona) & " WHERE [IDPersona]=" & GetID(persona1))

            'Tabella tbl_CQSPDConsulenze 
            dbSQL = "Select [ID] FROM [tbl_CQSPDConsulenze] WHERE [IDCLiente]=" & GetID(persona1)
            dbRis = Database.ExecuteReader(dbSQL)
            While dbRis.Read
                rec = New CMergePersonaRecord
                rec.NomeTabella = "tbl_CQSPDConsulenze"
                rec.FieldName = "IDCLiente"
                rec.RecordID = Formats.ToInteger(dbRis("ID"))
                mi.TabelleModificate.Add(rec)
            End While
            dbRis.Dispose()
            CQSPD.Database.ExecuteCommand("UPDATE [tbl_CQSPDConsulenze] Set [IDCLiente]=" & GetID(persona) & ", [NomeCliente] = " & DBUtils.DBString(persona.Nominativo) & " WHERE [IDCliente]=" & GetID(persona1))

            'Tabella tbl_CQSPDGrpConsulenze 
            dbSQL = "Select [ID] FROM [tbl_CQSPDGrpConsulenze] WHERE [IDCLiente]=" & GetID(persona1)
            dbRis = Database.ExecuteReader(dbSQL)
            While dbRis.Read
                rec = New CMergePersonaRecord
                rec.NomeTabella = "tbl_CQSPDGrpConsulenze"
                rec.FieldName = "IDCLiente"
                rec.RecordID = Formats.ToInteger(dbRis("ID"))
                mi.TabelleModificate.Add(rec)
            End While
            dbRis.Dispose()
            CQSPD.Database.ExecuteCommand("UPDATE [tbl_CQSPDGrpConsulenze] Set [IDCLiente]=" & GetID(persona) & ", [NomeCliente] = " & DBUtils.DBString(persona.Nominativo) & " WHERE [IDCliente]=" & GetID(persona1))

            'Tabella tbl_RichiesteFinanziamentiC 
            dbSQL = "Select [ID] FROM [tbl_RichiesteFinanziamentiC] WHERE [IDCliente]=" & GetID(persona1)
            dbRis = Database.ExecuteReader(dbSQL)
            While dbRis.Read
                rec = New CMergePersonaRecord
                rec.NomeTabella = "tbl_RichiesteFinanziamentiC"
                rec.FieldName = "IDCliente"
                rec.RecordID = Formats.ToInteger(dbRis("ID"))
                mi.TabelleModificate.Add(rec)
            End While
            dbRis.Dispose()
            CQSPD.Database.ExecuteCommand("UPDATE [tbl_RichiesteFinanziamentiC] Set [IDCliente]=" & GetID(persona) & ", [NomeCliente] = " & DBUtils.DBString(persona.Nominativo) & " WHERE [IDCliente]=" & GetID(persona1))

            'Tabella tbl_RichiesteFinanziamentiC 
            dbSQL = "Select [ID] FROM [tbl_RichiesteFinanziamentiC] WHERE [IDIstituto]=" & GetID(persona1)
            dbRis = Database.ExecuteReader(dbSQL)
            While dbRis.Read
                rec = New CMergePersonaRecord
                rec.NomeTabella = "tbl_RichiesteFinanziamentiC"
                rec.FieldName = "IDIstituto"
                rec.RecordID = Formats.ToInteger(dbRis("ID"))
                mi.TabelleModificate.Add(rec)
            End While
            dbRis.Dispose()
            CQSPD.Database.ExecuteCommand("UPDATE [tbl_RichiesteFinanziamentiC] Set [IDIstituto]=" & GetID(persona) & ", [NomeIstituto] = " & DBUtils.DBString(persona.Nominativo) & " WHERE [IDIstituto]=" & GetID(persona1))

            'Tabella tbl_RichiesteFinanziamentiC 
            dbSQL = "Select [ID] FROM [tbl_RichiesteFinanziamentiC] WHERE [IDAgenziaR]=" & GetID(persona1)
            dbRis = Database.ExecuteReader(dbSQL)
            While dbRis.Read
                rec = New CMergePersonaRecord
                rec.NomeTabella = "tbl_RichiesteFinanziamentiC"
                rec.FieldName = "IDAgenziaR"
                rec.RecordID = Formats.ToInteger(dbRis("ID"))
                mi.TabelleModificate.Add(rec)
            End While
            dbRis.Dispose()
            CQSPD.Database.ExecuteCommand("UPDATE [tbl_RichiesteFinanziamentiC] Set [IDAgenziaR]=" & GetID(persona) & ", [NomeAgenziaR] = " & DBUtils.DBString(persona.Nominativo) & " WHERE [IDAgenziaR]=" & GetID(persona1))

            'Tabella tbl_RichiesteFinanziamentiC 
            dbSQL = "Select [ID] FROM [tbl_RichiesteFinanziamentiC] WHERE [IDAgente]=" & GetID(persona1)
            dbRis = Database.ExecuteReader(dbSQL)
            While dbRis.Read
                rec = New CMergePersonaRecord
                rec.NomeTabella = "tbl_RichiesteFinanziamentiC"
                rec.FieldName = "IDAgente"
                rec.RecordID = Formats.ToInteger(dbRis("ID"))
                mi.TabelleModificate.Add(rec)
            End While
            dbRis.Dispose()
            CQSPD.Database.ExecuteCommand("UPDATE [tbl_RichiesteFinanziamentiC] Set [IDAgente]=" & GetID(persona) & ", [NomeAgente] = " & DBUtils.DBString(persona.Nominativo) & " WHERE [IDAgente]=" & GetID(persona1))

            'Tabella tbl_CQSPDClientiLavorati 
            dbSQL = "Select [ID] FROM [tbl_CQSPDClientiLavorati] WHERE [IDCliente]=" & GetID(persona1)
            dbRis = Database.ExecuteReader(dbSQL)
            While dbRis.Read
                rec = New CMergePersonaRecord
                rec.NomeTabella = "tbl_CQSPDClientiLavorati"
                rec.FieldName = "IDCliente"
                rec.RecordID = Formats.ToInteger(dbRis("ID"))
                mi.TabelleModificate.Add(rec)
            End While
            dbRis.Dispose()
            CQSPD.Database.ExecuteCommand("UPDATE [tbl_CQSPDClientiLavorati] Set [IDCliente]=" & GetID(persona) & ", [NomeCliente] = " & DBUtils.DBString(persona.Nominativo) & " WHERE [IDCliente]=" & GetID(persona1))

            'Tabella tbl_CQSPDImportExport 
            dbSQL = "Select [ID] FROM [tbl_CQSPDImportExport] WHERE [Esportazione]=True And [IDPersonaEsportata]=" & GetID(persona1)
            dbRis = Database.ExecuteReader(dbSQL)
            While dbRis.Read
                rec = New CMergePersonaRecord
                rec.NomeTabella = "tbl_CQSPDImportExport"
                rec.FieldName = "IDPersonaEsportata"
                rec.RecordID = Formats.ToInteger(dbRis("ID"))
                mi.TabelleModificate.Add(rec)
            End While
            dbRis.Dispose()
            CQSPD.Database.ExecuteCommand("UPDATE [tbl_CQSPDImportExport] Set [IDPersonaEsportata]=" & GetID(persona) & ", [NomePersonaEsportata] = " & DBUtils.DBString(persona.Nominativo) & " WHERE [Esportazione]=True And [IDPersonaEsportata]=" & GetID(persona1))

            'Tabella tbl_CQSPDImportExport 
            dbSQL = "Select [ID] FROM [tbl_CQSPDImportExport] WHERE [Esportazione]=False And [IDPersonaImportata]=" & GetID(persona1)
            dbRis = Database.ExecuteReader(dbSQL)
            While dbRis.Read
                rec = New CMergePersonaRecord
                rec.NomeTabella = "tbl_CQSPDImportExport"
                rec.FieldName = "IDPersonaImportata"
                rec.RecordID = Formats.ToInteger(dbRis("ID"))
                mi.TabelleModificate.Add(rec)
            End While
            dbRis.Dispose()
            CQSPD.Database.ExecuteCommand("UPDATE [tbl_CQSPDImportExport] Set [IDPersonaImportata]=" & GetID(persona) & ", [NomePersonaImportata] = " & DBUtils.DBString(persona.Nominativo) & " WHERE [Esportazione]=False And [IDPersonaImportata]=" & GetID(persona1))

            'Tabella tbl_CQSPDFinestreLavorazione 
            dbSQL = "Select [ID] FROM [tbl_CQSPDFinestreLavorazione] WHERE [IDCliente]=" & GetID(persona1)
            dbRis = Database.ExecuteReader(dbSQL)
            While dbRis.Read
                rec = New CMergePersonaRecord
                rec.NomeTabella = "tbl_CQSPDFinestreLavorazione"
                rec.FieldName = "IDCliente"
                rec.RecordID = Formats.ToInteger(dbRis("ID"))
                mi.TabelleModificate.Add(rec)
            End While
            dbRis.Dispose()
            CQSPD.Database.ExecuteCommand("UPDATE [tbl_CQSPDFinestreLavorazione] Set [IDCliente]=" & GetID(persona) & ", [NomeCliente] = " & DBUtils.DBString(persona.Nominativo) & " WHERE [IDCliente]=" & GetID(persona1))



            If (TypeOf (e.Persona) Is CPersonaFisica) Then
                Dim w1 As FinestraLavorazione = CQSPD.FinestreDiLavorazione.GetFinestraCorrente(persona)
                Dim w2 As FinestraLavorazione = CQSPD.FinestreDiLavorazione.GetFinestraCorrente(persona1)
                If (w1 Is Nothing) Then
                    If (w2 IsNot Nothing) Then
                        w1 = w2
                    End If
                Else
                    If (w2 IsNot Nothing) Then
                        w1.MergeWith(w2)
                    End If
                End If
                If (w1 Is Nothing) Then
                    w1 = CQSPD.FinestreDiLavorazione.GetProssimaFinestra(e.Persona)
                Else
                    w1.Cliente = e.Persona
                    w1.Save()
                End If

                If (w2 IsNot Nothing) AndAlso (w2 IsNot w1) Then
                    w2.Delete()
                End If

                Dim finestre As CCollection(Of FinestraLavorazione) = CQSPD.FinestreDiLavorazione.GetFinestreByPersona(persona)
                Dim foundAttiva As Boolean = False
                finestre.Sort()
                Dim i As Integer
                For i = 0 To finestre.Count - 1
                    w1 = finestre(i)
                    If (w1.StatoFinestra = StatoFinestraLavorazione.Aperta) Then
                        If (foundAttiva) Then
                            w1.StatoFinestra = StatoFinestraLavorazione.Chiusa
                            w1.Save()
                        End If
                        foundAttiva = True
                    End If
                Next
                i = 0
                While (i < finestre.Count)
                    w1 = finestre(i)
                    If (w1.StatoFinestra = StatoFinestraLavorazione.NonAperta) Then
                        Dim j As Integer = i + 1
                        While (j < finestre.Count)
                            w2 = finestre(j)
                            If (w2.StatoFinestra = StatoFinestraLavorazione.NonAperta) Then
                                w1.MergeWith(w2)
                                w1.Save()
                                w2.Delete()
                            End If
                            j += 1
                        End While
                        Exit While
                    End If
                    i += 1
                End While

            End If
        End SyncLock
    End Sub

    Private Shared Sub HandlePeronaUnMerged(ByVal e As MergePersonaEventArgs)
        SyncLock Anagrafica.lock
            Dim mi As CMergePersona = e.MI
            Dim persona As CPersona = mi.Persona1
            Dim persona1 As CPersona = mi.Persona2
            Dim items As String

            'Tabella tbl_AreaManagers
            items = mi.GetAffectedRecors("tbl_AreaManagers", "Persona")
            If (items <> "") Then Database.ExecuteCommand("UPDATE [tbl_AreaManagers] Set [Persona]=" & GetID(persona1) & " WHERE [ID] In (" & items & ")")

            'Tabella tbl_Collaboratori 
            items = mi.GetAffectedRecors("tbl_Collaboratori", "Persona")
            If (items <> "") Then Database.ExecuteCommand("UPDATE [tbl_Collaboratori] Set [Persona]= " & GetID(persona1) & " WHERE [ID] In (" & items & ")")

            'Tabella tbl_EstinzioniXPersona 
            items = mi.GetAffectedRecors("tbl_EstinzioniXPersona", "Persona")
            If (items <> "") Then Database.ExecuteCommand("UPDATE [tbl_EstinzioniXPersona] Set [Persona]=" & GetID(persona1) & " WHERE [ID] In (" & items & ")")

            'Tabella tbl_TeamManagers 
            items = mi.GetAffectedRecors("tbl_TeamManagers", "Persona")
            If (items <> "") Then Database.ExecuteCommand("UPDATE [tbl_TeamManagers] Set [Persona]=" & GetID(persona1) & " WHERE [ID] In (" & items & ")")

            If TypeOf (persona) Is CPersonaFisica Then
                With DirectCast(persona, CPersonaFisica)
                    'Tabella tbl_Rapportini 
                    items = mi.GetAffectedRecors("tbl_Rapportini", "Cliente")
                    If (items <> "") Then Database.ExecuteCommand("UPDATE [tbl_Rapportini] Set [Cliente]=" & GetID(persona1) & " WHERE [ID] In (" & items & ")")
                End With

                'Tabella tbl_CQSPDRichCERQ 
                items = mi.GetAffectedRecors("tbl_CQSPDRichCERQ", "IDCliente")
                If (items <> "") Then Database.ExecuteCommand("UPDATE [tbl_CQSPDRichCERQ] Set [IDCliente]=" & GetID(persona1) & ", [NomeCliente]=" & DBUtils.DBString(persona1.Nominativo) & " WHERE [ID] In (" & items & ")")
            Else
                'Tabella tbl_Rapportini 
                items = mi.GetAffectedRecors("tbl_Rapportini", "IDAmministrazione")
                If (items <> "") Then Database.ExecuteCommand("UPDATE [tbl_Rapportini] Set [IDAmministrazione]=" & GetID(persona1) & ", [Ente]=" & DBUtils.DBString(persona1.Nominativo) & " WHERE [ID] In (" & items & ")")

                'Tabella tbl_Rapportini 
                items = mi.GetAffectedRecors("tbl_Rapportini", "IDEntePagante")
                If (items <> "") Then Database.ExecuteCommand("UPDATE [tbl_Rapportini] Set [IDEntePagante]=" & GetID(persona1) & ", [NomeEntePagante]=" & DBUtils.DBString(persona1.Nominativo) & " WHERE [ID] In (" & items & ")")

                'Tabella tbl_CQSPDRichCERQ           
                items = mi.GetAffectedRecors("tbl_CQSPDRichCERQ", "IDAmministrazione")
                If (items <> "") Then Database.ExecuteCommand("UPDATE [tbl_CQSPDRichCERQ] Set [IDAmministrazione]=" & GetID(persona1) & ", [NomeAmministrazione]=" & DBUtils.DBString(persona1.Nominativo) & " WHERE [ID] In (" & items & ")")
            End If

            'Tabella tbl_RichiesteFinanziamenti 
            items = mi.GetAffectedRecors("tbl_RichiesteFinanziamenti", "IDCliente")
            If (items <> "") Then Database.ExecuteCommand("UPDATE [tbl_RichiesteFinanziamenti] Set [IDCliente]=" & GetID(persona1) & ", [NomeCliente]=" & DBUtils.DBString(persona1.Nominativo) & " WHERE [ID] In (" & items & ")")

            'Tabella tbl_Preventivi_Offerte 
            items = mi.GetAffectedRecors("tbl_Preventivi_Offerte", "IDCliente")
            If (items <> "") Then Database.ExecuteCommand("UPDATE [tbl_Preventivi_Offerte] Set [IDCliente]=" & GetID(persona1) & " WHERE [ID] In (" & items & ")")

            'Tabella tbl_Estinzioni 
            items = mi.GetAffectedRecors("tbl_Estinzioni", "IDPersona")
            If (items <> "") Then Database.ExecuteCommand("UPDATE [tbl_Estinzioni] Set [IDPersona]=" & GetID(persona1) & " WHERE [ID] In (" & items & ")")

            'Tabella tbl_CQSPDConsulenze 
            items = mi.GetAffectedRecors("tbl_CQSPDConsulenze", "IDCLiente")
            If (items <> "") Then Database.ExecuteCommand("UPDATE [tbl_CQSPDConsulenze] Set [IDCLiente]=" & GetID(persona1) & ", [NomeCliente]=" & DBUtils.DBString(persona1.Nominativo) & " WHERE [ID] In (" & items & ")")

            'Tabella tbl_CQSPDGrpConsulenze 
            items = mi.GetAffectedRecors("tbl_CQSPDGrpConsulenze", "IDCLiente")
            If (items <> "") Then Database.ExecuteCommand("UPDATE [tbl_CQSPDGrpConsulenze] Set [IDCLiente]=" & GetID(persona1) & ", [NomeCliente]=" & DBUtils.DBString(persona1.Nominativo) & " WHERE [ID] In (" & items & ")")

            'Tabella tbl_RichiesteFinanziamentiC 
            items = mi.GetAffectedRecors("tbl_RichiesteFinanziamentiC", "IDCliente")
            If (items <> "") Then Database.ExecuteCommand("UPDATE [tbl_RichiesteFinanziamentiC] Set [IDCliente]=" & GetID(persona1) & ", [NomeCliente]=" & DBUtils.DBString(persona1.Nominativo) & " WHERE [ID] In (" & items & ")")

            'Tabella tbl_RichiesteFinanziamentiC 
            items = mi.GetAffectedRecors("tbl_RichiesteFinanziamentiC", "IDIstituto")
            If (items <> "") Then Database.ExecuteCommand("UPDATE [tbl_RichiesteFinanziamentiC] Set [IDIstituto]=" & GetID(persona1) & ", [NomeIstituto]=" & DBUtils.DBString(persona1.Nominativo) & " WHERE [ID] In (" & items & ")")

            'Tabella tbl_RichiesteFinanziamentiC 
            items = mi.GetAffectedRecors("tbl_RichiesteFinanziamentiC", "IDAgenziaR")
            If (items <> "") Then Database.ExecuteCommand("UPDATE [tbl_RichiesteFinanziamentiC] Set [IDAgenziaR]=" & GetID(persona1) & ", [NomeAgenziaR]=" & DBUtils.DBString(persona1.Nominativo) & " WHERE [ID] In (" & items & ")")

            'Tabella tbl_RichiesteFinanziamentiC 
            items = mi.GetAffectedRecors("tbl_RichiesteFinanziamentiC", "IDAgente")
            If (items <> "") Then Database.ExecuteCommand("UPDATE [tbl_RichiesteFinanziamentiC] Set [IDAgente]=" & GetID(persona1) & ", [NomeAgente]=" & DBUtils.DBString(persona1.Nominativo) & " WHERE [ID] In (" & items & ")")

            'Tabella tbl_CQSPDClientiLavorati 
            items = mi.GetAffectedRecors("tbl_CQSPDClientiLavorati", "IDCliente")
            If (items <> "") Then Database.ExecuteCommand("UPDATE [tbl_CQSPDClientiLavorati] Set [IDCliente]=" & GetID(persona1) & ", [NomeCliente]=" & DBUtils.DBString(persona1.Nominativo) & " WHERE [ID] In (" & items & ")")

            'Tabella tbl_CQSPDImportExport 
            items = mi.GetAffectedRecors("tbl_CQSPDImportExport", "IDPersonaEsportata")
            If (items <> "") Then Database.ExecuteCommand("UPDATE [tbl_CQSPDImportExport] Set [IDPersonaEsportata]=" & GetID(persona1) & ", [NomePersonaEsportata]=" & DBUtils.DBString(persona1.Nominativo) & " WHERE [ID] In (" & items & ")")

            'Tabella tbl_CQSPDImportExport 
            items = mi.GetAffectedRecors("tbl_CQSPDImportExport", "IDPersonaImportata")
            If (items <> "") Then Database.ExecuteCommand("UPDATE [tbl_CQSPDImportExport] Set [IDPersonaImportata]=" & GetID(persona1) & ", [NomePersonaImportata]=" & DBUtils.DBString(persona1.Nominativo) & " WHERE [ID] In (" & items & ")")

            'Tabella tbl_CQSPDFinestreLavorazione 
            items = mi.GetAffectedRecors("tbl_CQSPDFinestreLavorazione", "IDCliente")
            If (items <> "") Then Database.ExecuteCommand("UPDATE [tbl_CQSPDFinestreLavorazione] Set [IDCliente]=" & GetID(persona1) & ", [NomeCliente]=" & DBUtils.DBString(persona1.Nominativo) & " WHERE [ID] In (" & items & ")")

        End SyncLock
    End Sub



    Private Shared Sub HandlePersonaModified(ByVal e As PersonaEventArgs)
        SyncLock Anagrafica.lock
            CQSPD.Database.ExecuteCommand("UPDATE [tbl_Rapportini] Set [TipoFonteCliente]=" & DBUtils.DBString(e.Persona.TipoFonte) & ", [IDFonteCliente]=" & DBUtils.DBNumber(e.Persona.IDFonte) & " WHERE [Cliente]=" & GetID(e.Persona))
            CQSPD.Database.ExecuteCommand("UPDATE [tbl_CQSPDClientiLavorati] Set [IconaCliente]=" & DBUtils.DBString(e.Persona.IconURL) & ", [NomeCliente]=" & DBUtils.DBString(e.Persona.Nominativo) & ", [IDPuntoOperativo]=" & e.Persona.IDPuntoOperativo & ", [NomePuntoOperativo]=" & DBUtils.DBString(e.Persona.NomePuntoOperativo) & " WHERE [IDCliente]=" & GetID(e.Persona))
            CQSPD.Database.ExecuteCommand("UPDATE [tbl_Estinzioni] Set [IDPuntoOperativo]=" & DBUtils.DBNumber(e.Persona.IDPuntoOperativo) & ", [NomePuntoOperativo]=" & DBUtils.DBString(e.Persona.NomePuntoOperativo) & " WHERE [IDPersona]=" & GetID(e.Persona))
            If (TypeOf (e.Persona) Is CPersonaFisica) Then
                Dim wl As FinestraLavorazione = CQSPD.FinestreDiLavorazione.GetFinestraCorrente(e.Persona)
                If (wl IsNot Nothing) Then
                    wl.Cliente = e.Persona
                    wl.NomeCliente = e.Persona.Nominativo
                    wl.IconaCliente = e.Persona.IconURL
                    wl.Stato = ObjectStatus.OBJECT_VALID
                    'w.DataInizioLavorabilita = Me.CalcolaDataLavorabilita(persona)
                    'w.DataUltimoAggiornamento = Calendar.Now
                    wl.SetFlag(FinestraLavorazioneFlags.Disponibile_CQS, CQSPD.FinestreDiLavorazione.HaCQS(e.Persona, wl.DataInizioLavorabilita))
                    wl.SetFlag(FinestraLavorazioneFlags.Disponibile_PD, CQSPD.FinestreDiLavorazione.HaPD(e.Persona, wl.DataInizioLavorabilita))
                    wl.Save()
                Else
                    wl = CQSPD.FinestreDiLavorazione.GetProssimaFinestra(e.Persona)
                End If
            End If

        End SyncLock
    End Sub

    Private Shared Sub HandlePersonaCreated(ByVal e As PersonaEventArgs)
        SyncLock Anagrafica.lock
            If (TypeOf (e.Persona) Is CPersonaFisica) Then
                Dim wl As FinestraLavorazione = CQSPD.FinestreDiLavorazione.GetProssimaFinestra(e.Persona)
                wl.Stato = ObjectStatus.OBJECT_VALID
                wl.Save()
            End If
        End SyncLock
    End Sub

    Public Shared ReadOnly Property [Module] As CModule
        Get
            If m_Module Is Nothing Then m_Module = Sistema.Modules.GetItemByName("DMaker")
            Return m_Module
        End Get
    End Property

    ''' <summary>
    ''' Gruppo dei consulenti (i membri vengono aggiunti e rimossi automaticamente in funzione della definizione dei consulenti)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property GruppoConsulenti As CGroup
        Get
            m_GruppoConsulenti = Sistema.Groups.GetItemByName("Consulenti CQSPD")
            If m_GruppoConsulenti Is Nothing Then
                m_GruppoConsulenti = New CGroup("Consulenti CQSPD")
                m_GruppoConsulenti.Description = "Gruppo di operatori abilitati alle consulenze per le pratiche"
                m_GruppoConsulenti.Stato = ObjectStatus.OBJECT_VALID
                m_GruppoConsulenti.Save()
            End If
            Return m_GruppoConsulenti
        End Get
    End Property

    ''' <summary>
    ''' Rappresenta il gruppo degli operatori abilitati alla visualizzazione ed alla lavorazione delle pratiche
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property GruppoOperatori As CGroup
        Get
            If (m_GruppoOperatori Is Nothing) Then m_GruppoOperatori = Sistema.Groups.GetItemByName("Rapportini")
            If (m_GruppoOperatori Is Nothing) Then
                m_GruppoOperatori = New CGroup("Rapportini")
                m_GruppoOperatori.Stato = ObjectStatus.OBJECT_VALID
                m_GruppoOperatori.Save()
            End If
            Return m_GruppoOperatori
        End Get
    End Property

    ''' <summary>
    ''' Rappresenta il gruppo dei responsabili di ufficio che hanno poteri su tutte le pratiche dell'ufficio
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property GruppoReferenti As CGroup
        Get
            If (m_GruppoReferenti Is Nothing) Then m_GruppoReferenti = Sistema.Groups.GetItemByName("CQSPD Referenti")
            If (m_GruppoReferenti Is Nothing) Then
                m_GruppoReferenti = New CGroup("CQSPD Referenti")
                m_GruppoReferenti.Stato = ObjectStatus.OBJECT_VALID
                m_GruppoReferenti.Save()
            End If
            Return m_GruppoReferenti
        End Get
    End Property

    ''' <summary>
    ''' Rappresenta il gruppo dei supervisori.
    ''' Questo gruppo ha potere globale su tutte le pratiche e può autorizzare gli sconti (se con motivo non privilegiato)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property GruppoSupervisori As CGroup
        Get
            If (m_GruppoSupervisori Is Nothing) Then m_GruppoSupervisori = Sistema.Groups.GetItemByName("CQSPD Supervisori")
            If (m_GruppoSupervisori Is Nothing) Then
                m_GruppoSupervisori = New CGroup("CQSPD Supervisori")
                m_GruppoSupervisori.Stato = ObjectStatus.OBJECT_VALID
                m_GruppoSupervisori.Save()
            End If
            Return m_GruppoSupervisori
        End Get
    End Property

    ''' <summary>
    ''' Questo gruppo ha potere su tutte le pratiche e può autorizzare tutti gli sconti
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property GruppoAutorizzatori As CGroup
        Get
            If (m_GruppoAutorizzatori Is Nothing) Then m_GruppoAutorizzatori = Sistema.Groups.GetItemByName("CQSPD Autorizzatori")
            If (m_GruppoAutorizzatori Is Nothing) Then
                m_GruppoAutorizzatori = New CGroup("CQSPD Autorizzatori")
                m_GruppoAutorizzatori.Stato = ObjectStatus.OBJECT_VALID
                m_GruppoAutorizzatori.Save()
            End If
            Return m_GruppoAutorizzatori
        End Get
    End Property

    Private Shared m_Providers As CCollection(Of ICalendarProvider)


    Public Shared ReadOnly Property Providers As CCollection(Of ICalendarProvider)
        Get
            If (CQSPD.m_Providers Is Nothing) Then CQSPD.m_Providers = New CCollection(Of ICalendarProvider)
            Return New CCollection(Of ICalendarProvider)(CQSPD.m_Providers)
        End Get
    End Property

    Public Shared Function GetToDoList(ByVal user As CUser) As CCollection(Of ICalendarActivity)
        If (user Is Nothing) Then Throw New ArgumentNullException("user")

        Dim ret As New CCollection(Of ICalendarActivity)

        Dim providers As CCollection(Of ICalendarProvider) = CQSPD.Providers
        For Each p As ICalendarProvider In providers
            ret.AddRange(p.GetToDoList(user))
        Next

        ret.Sort()

        Return ret
    End Function

    Public Shared Function GetProviderByName(ByVal name As String) As ICalendarProvider
        Dim providers As CCollection(Of ICalendarProvider) = CQSPD.Providers
        name = Strings.Trim(name)
        If (name = "") Then Return Nothing
        For Each p As ICalendarProvider In providers
            If p.UniqueName = name Then Return p
        Next
        Return Nothing
    End Function

    Public Shared Sub AddProvider(ByVal p As ICalendarProvider)
        If (CQSPD.GetProviderByName(p.UniqueName) IsNot Nothing) Then Throw New ArgumentException("Provider già registrato")
        CQSPD.m_Providers.Add(p)
    End Sub

    Public Shared Sub RemoveProvider(ByVal p As ICalendarProvider)
        p = CQSPD.GetProviderByName(p.UniqueName)
        If (p Is Nothing) Then Throw New KeyNotFoundException("Provider non trovato")
        CQSPD.m_Providers.Remove(p)
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        DMD.DMDObject.DecreaseCounter(Me)
    End Sub

    Public Class CTMProduttoriCollection
        Inherits CCollaboratoreCollection

        Private m_TeamManager As CTeamManager

        Public Sub New()
        End Sub

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_TeamManager IsNot Nothing) Then DirectCast(value, CCollaboratore).Referente = Me.m_TeamManager
            MyBase.OnInsert(index, value)
        End Sub

        Protected Friend Function Initialize(ByVal value As CTeamManager) As Boolean
            MyBase.Clear()
            Me.m_TeamManager = value
            If (GetID(value) <> 0) Then
                Dim cursor As New CCollaboratoriCursor
                cursor.ReferenteID.Value = GetID(value)
                While Not cursor.EOF
                    MyBase.Add(cursor.Item)
                    cursor.MoveNext()
                End While
                cursor.Dispose()
            End If
            Return True
        End Function

    End Class


    Public Class CTMPraticheCollection
        Inherits CPraticheCollection

        Private m_TeamManager As CTeamManager

        Public Sub New()
            Me.m_TeamManager = Nothing
        End Sub

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_TeamManager IsNot Nothing) Then
                Dim info As CInfoPratica = DirectCast(value, CRapportino).Info
                info.Commerciale = Me.m_TeamManager
            End If
            MyBase.OnInsert(index, value)
        End Sub

        Protected Friend Function Initialize(ByVal value As CTeamManager) As Boolean
            MyBase.Clear()
            Me.m_TeamManager = value
            If (GetID(value) <> 0) Then
                Dim cursor As New CRapportiniCursor
                cursor.IDCommerciale.Value = GetID(value)
                While Not cursor.EOF
                    MyBase.Add(cursor.Item)
                    cursor.MoveNext()
                End While
                cursor.Dispose()
            End If
            Return True
        End Function

    End Class

    Public Class CAMTeamManagersCollection
        Inherits CTeamManagersCollection

        Private m_AreaManager As CAreaManager

        Public Sub New()
        End Sub

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_AreaManager IsNot Nothing) Then DirectCast(value, CTeamManager).Referente = Me.m_AreaManager
            MyBase.OnInsert(index, value)
        End Sub

        Protected Friend Function Initialize(ByVal value As CAreaManager) As Boolean
            MyBase.Clear()
            Me.m_AreaManager = value
            If (GetID(value) <> 0) Then
                Dim cursor As New CTeamManagersCursor
                cursor.IDReferente.Value = GetID(value)
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                While Not cursor.EOF
                    MyBase.Add(cursor.Item)
                    cursor.MoveNext()
                End While
                cursor.Dispose()
                Me.Sort()
            End If
            Return True
        End Function


    End Class

    Public Class CTeamManagersCollection
        Inherits CCollection(Of CTeamManager)
        Implements IComparer

        Public Sub New()
        End Sub

        Public Function ContaValidi() As Integer
            Dim cnt As Integer = 0
            For i As Integer = 0 To Me.Count - 1
                Dim tm As CTeamManager = Me(i)
                Select Case tm.StatoTeamManager
                    Case StatoTeamManager.STATO_ATTIVO, _
                         StatoTeamManager.STATO_INATTIVAZIONE, _
                         StatoTeamManager.STATO_SOSPESO
                        cnt = cnt + 1
                End Select
            Next
            Return cnt
        End Function

        Public Function ContaAttivi() As Integer
            Dim cnt As Integer = 0
            For i As Integer = 0 To Me.Count - 1
                Dim tm As CTeamManager = Me(i)
                If tm.StatoTeamManager = StatoTeamManager.STATO_ATTIVO Then cnt = cnt + 1
            Next
            Return cnt
        End Function

        Public Function ContaInAttivazione() As Integer
            Dim cnt As Integer = 0
            For i As Integer = 0 To Me.Count - 1
                Dim tm As CTeamManager = Me(i)
                If tm.StatoTeamManager = StatoTeamManager.STATO_INATTIVAZIONE Then cnt = cnt + 1
            Next
            Return cnt
        End Function


        Public Function Compare1(x As CTeamManager, y As CTeamManager) As Integer
            Return Strings.Compare(x.Nominativo, y.Nominativo, CompareMethod.Text)
        End Function
    End Class





    Public Class CFsbPrev_StampaOfferta
        Inherits DBObject

        Private m_OffertaID As Integer
        Private m_Offerta As COffertaCQS
        Private m_DataStampa As Date
        Private m_OutputFile As String

        Public Sub New()
            Me.m_OffertaID = 0
            Me.m_Offerta = Nothing
            Me.m_DataStampa = Nothing
            Me.m_OutputFile = ""
        End Sub

        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Property DataStampa As Date
            Get
                Return Me.m_DataStampa
            End Get
            Set(value As Date)
                Dim oldValue As Date = Me.m_DataStampa
                If (oldValue = value) Then Exit Property
                Me.m_DataStampa = value
                Me.DoChanged("DataStampa", value, oldValue)
            End Set
        End Property

        Public Property OutputFile As String
            Get
                Return Me.m_OutputFile
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_OutputFile
                If (oldValue = value) Then Exit Property
                Me.m_OutputFile = value
                Me.DoChanged("OutputFile", value, oldValue)
            End Set
        End Property

        Public Property OffertaID As Integer
            Get
                Return GetID(Me.m_Offerta, Me.m_OffertaID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.OffertaID
                If oldValue = value Then Exit Property
                Me.m_Offerta = Nothing
                Me.m_OffertaID = value
                Me.DoChanged("OffertaID", value, oldValue)
            End Set
        End Property

        Public Property Offerta As COffertaCQS
            Get
                If (Me.m_Offerta Is Nothing) Then Me.m_Offerta = CQSPD.Offerte.GetItemById(Me.m_OffertaID)
                Return Me.m_Offerta
            End Get
            Set(value As COffertaCQS)
                Dim oldValue As COffertaCQS = Me.Offerta
                If (oldValue = value) Then Exit Property
                Me.m_Offerta = value
                Me.m_OffertaID = GetID(value)
                Me.DoChanged("Offerta", value, oldValue)
            End Set
        End Property

        Public Overrides Function GetTableName() As String
            Return "tbl_Preventivi_Stampe"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_OffertaID = reader.Read("Offerta", Me.m_OffertaID)
            Me.m_DataStampa = reader.Read("DataStampa", Me.m_DataStampa)
            Me.m_OutputFile = reader.Read("OutputFile", Me.m_OutputFile)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Offerta", GetID(Me.m_Offerta, Me.m_OffertaID))
            writer.Write("DataStampa", Me.m_DataStampa)
            writer.Write("OutputFile", Me.m_OutputFile)
            Return MyBase.SaveToRecordset(writer)
        End Function


    End Class



#Region "Coefficienti Assicurativi"

    ''' <summary>
    ''' Riga della tabella dei coefficienti assicurativi
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CCoefficienteAssicurativo
        Inherits DBObjectBase

        Private m_IDTabella As Integer
        Private m_Tabella As CTabellaAssicurativa
        Private m_Sesso As String
        Private m_Anni As Integer
        Private m_C() As Nullable(Of Double)

        Public Sub New()
            Me.m_IDTabella = 0
            Me.m_Tabella = Nothing
            Me.m_Sesso = ""
            Me.m_Anni = 0
            ReDim m_C(10)
        End Sub

        Public Overrides Function GetModule() As CModule
            Return CQSPD.TabelleAssicurative.Module
        End Function

        Public Property IDTabella As Integer
            Get
                Return GetID(Me.m_Tabella, Me.m_IDTabella)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDTabella
                If value = oldValue Then Exit Property
                Me.m_IDTabella = value
                Me.m_Tabella = Nothing
                Me.DoChanged("IDTabella", value, oldValue)
            End Set
        End Property

        Public Property Tabella As CTabellaAssicurativa
            Get
                If (Me.m_Tabella Is Nothing) Then Me.m_Tabella = CQSPD.TabelleAssicurative.GetItemById(Me.m_IDTabella)
                Return Me.m_Tabella
            End Get
            Set(value As CTabellaAssicurativa)
                Dim oldValue As CTabellaAssicurativa = Me.Tabella
                If (oldValue = value) Then Exit Property
                Me.m_Tabella = value
                Me.m_IDTabella = GetID(value)
                Me.DoChanged("Tabella", value, oldValue)
            End Set
        End Property

        Public Property Sesso As String
            Get
                Return Me.m_Sesso
            End Get
            Set(value As String)
                value = UCase(Left(Trim(value), 1))
                Dim oldValue As String = Me.m_Sesso
                Me.m_Sesso = value
                Me.DoChanged("Sesso", value, oldValue)
            End Set
        End Property

        Public Property Anni As Integer
            Get
                Return Me.m_Anni
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_Anni
                If (oldValue = value) Then Exit Property
                Me.m_Anni = value
                Me.DoChanged("Anni", value, oldValue)
            End Set
        End Property

        Public Property Coefficiente(ByVal durata As Integer) As Nullable(Of Double)
            Get
                Return Me.m_C(Fix(durata / 12))
            End Get
            Set(value As Nullable(Of Double))
                Dim oldValue As Nullable(Of Double) = Me.m_C(Fix(durata / 12))
                If (oldValue = value) Then Exit Property
                Me.m_C(Fix(durata / 12)) = value
                Me.DoChanged("Coefficiente", value, oldValue)
            End Set
        End Property

        Public Overrides Function GetTableName() As String
            Return "tbl_CoefficientiAssicurativi"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            reader.Read("Tabella", Me.m_IDTabella)
            reader.Read("Sesso", Me.m_Sesso)
            reader.Read("Anni", Me.m_Anni)
            For i As Integer = 24 To 120 Step 12
                reader.Read("C" & i, Me.m_C(Math.Floor(i / 12)))
            Next
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Tabella", Me.IDTabella)
            writer.Write("Sesso", Me.m_Sesso)
            writer.Write("Anni", Me.m_Anni)
            For i As Integer = 24 To 120 Step 12
                writer.Write("C" & i, Me.m_C(Math.Floor(i / 12)))
            Next
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("IDTabella", Me.IDTabella)
            writer.WriteTag("Sesso", Me.m_Sesso)
            writer.WriteTag("Anni", Me.m_Anni)
            writer.WriteTag("C", Me.m_C)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "IDTabella" : Me.m_IDTabella = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Sesso" : Me.m_Sesso = UCase(Left(Trim(XML.Utils.Serializer.DeserializeString(fieldValue)), 1))
                Case "Anni" : Me.m_Anni = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "C" : Me.m_C = fieldValue
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Overrides Function ToString() As String
            Dim ret As String = "(" & Me.m_Anni & ", '" & Me.m_Sesso & ", {"
            For i As Integer = 24 To 120 Step 12
                If (i > 24) Then ret &= ", "
                ret &= Formats.FormatPercentage(Me.m_C, 4)
            Next
            ret &= "})"
            Return ret
        End Function


    End Class


    ''' <summary>
    ''' Cursore sulla tabella dei coefficienti assicurativi
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CCoefficientiAssicurativiCursor
        Inherits DBObjectCursorBase(Of CCoefficienteAssicurativo)

        Private m_Tabella As CCursorField(Of Integer)
        Private m_Sesso As CCursorFieldObj(Of String)
        Private m_Anni As CCursorField(Of Integer)

        Public Sub New()
            Me.m_Tabella = New CCursorField(Of Integer)("Tabella")
            Me.m_Sesso = New CCursorFieldObj(Of String)("Sesso")
            Me.m_Anni = New CCursorField(Of Integer)("Anni")
        End Sub

        Public ReadOnly Property Tabella As CCursorField(Of Integer)
            Get
                Return Me.m_Tabella
            End Get
        End Property

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Public ReadOnly Property Sesso As CCursorFieldObj(Of String)
            Get
                Return Me.m_Sesso
            End Get
        End Property

        Public ReadOnly Property Anni As CCursorField(Of Integer)
            Get
                Return Me.m_Anni
            End Get
        End Property

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CCoefficienteAssicurativo
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CoefficientiAssicurativi"
        End Function


        Protected Overrides Function GetModule() As CModule
            Return Nothing
        End Function
    End Class


    ''' <summary>
    ''' Collezione di righe di coefficienti assicurativi appartenenti ad una tabella
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CCoefficientiAssicurativiCollection
        Inherits CCollection(Of CCoefficienteAssicurativo)

        Private m_Tabella As CTabellaAssicurativa

        Public Sub New()
            Me.m_Tabella = Nothing
        End Sub

        Public Sub New(ByVal tabella As CTabellaAssicurativa)
            If (tabella Is Nothing) Then Throw New ArgumentNullException("tabella")
            Me.Initialize(tabella)
        End Sub

        Public ReadOnly Property Tabella As CTabellaAssicurativa
            Get
                Return Me.m_Tabella
            End Get
        End Property

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Tabella IsNot Nothing) Then DirectCast(value, CCoefficienteAssicurativo).Tabella = Me.m_Tabella
            MyBase.OnInsert(index, value)
        End Sub

        Public Overloads Function Add(ByVal sesso As String, ByVal anni As Integer) As CCoefficienteAssicurativo
            Dim item As New CCoefficienteAssicurativo
            item.Sesso = sesso
            item.Anni = anni
            MyBase.Add(item)
            Return item
        End Function

        Public Function GetCoefficiente(ByVal sesso As String, ByVal anni As Integer, ByVal durata As Integer) As Nullable(Of Double)
            Dim i As Integer
            Dim Item As New CCoefficienteAssicurativo
            Item.Sesso = sesso
            Item.Anni = anni
            'Response.Write("Tabella: " & GetID(m_Tabella, 0) & ", sesso: " & sesso & ", anni: " & anni & ", durata: " & durata & "<br/>")
            i = Me.IndexOf(Item)
            'i = Me.IndexOf2(item, 0, Me.Count-1)
            If (i >= 0) Then
                Return Me.Item(i).Coefficiente(durata)
            Else
                Return Nothing
            End If
            'Response.Write("i: " & i & "<br/>")
        End Function

        Protected Friend Function Initialize(ByVal tabella As CTabellaAssicurativa) As Boolean
            MyBase.Clear()
            Me.Sorted = False
            Me.m_Tabella = tabella
            Dim cursor As New CCoefficientiAssicurativiCursor
            cursor.Tabella.Value = GetID(tabella, 0)
            cursor.Sesso.SortOrder = SortEnum.SORT_ASC
            cursor.Anni.SortOrder = SortEnum.SORT_ASC
            While Not cursor.EOF
                MyBase.Add(cursor.Item)
                cursor.MoveNext()
            End While
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            Me.Comparer = Me
            Me.Sorted = True
            Return True
        End Function

        Protected Overrides Function CompareWithType(a As CCoefficienteAssicurativo, b As CCoefficienteAssicurativo) As Integer
            Dim ret As Integer = a.Anni - b.Anni
            If (ret = 0) Then ret = Strings.Compare(a.Sesso, b.Sesso)
            Return ret
        End Function

    End Class

#End Region


End Class

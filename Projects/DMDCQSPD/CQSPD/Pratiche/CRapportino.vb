Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica


Partial Public Class CQSPD

    <Flags> _
    Public Enum PraticaFlags As Integer
        NOTSET = 0
        HIDDEN = 1
        RICHIEDEAPPROVAZIONE = 2
        APPROVATA = 4
        DAVEDERE = 8
        TRASFERITA = 16
    End Enum

    <Serializable> _
    Public Class CRapportino
        Inherits DBObjectPO
        Implements IEstintore, IOggettoApprovabile, IOggettoVerificabile

        ''' <summary>
        ''' Evento generato quando viene generata una condizione di attenzione per la pratica
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event PraticaWatch(ByVal sender As Object, ByVal e As ItemEventArgs)


        ''' <summary>
        ''' Evento generato quando viene effettuata una correzione alla pratica
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event Corretta(ByVal sender As Object, ByVal e As ItemEventArgs)

        ''' <summary>
        ''' Evento generato quando vine formulata un'offerta che richiede l'approvazione
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event RequireApprovation(ByVal sender As Object, ByVal e As ItemEventArgs) Implements IOggettoApprovabile.RequireApprovation

        ''' <summary>
        ''' Evento generato quando la pratica subisce un passaggio di stato
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event StatusChanged(ByVal sender As Object, ByVal e As ItemEventArgs)


        ''' <summary>
        ''' Evento generato quando l'offerta corrente viene approvata
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event Approvata(ByVal sender As Object, ByVal e As ItemEventArgs) Implements IOggettoApprovabile.Approvata

        ''' <summary>
        ''' Evento generato quando l'offerta viene rifiutata
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event Rifiutata(ByVal sender As Object, ByVal e As ItemEventArgs) Implements IOggettoApprovabile.Rifiutata

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event PresaInCarico(sender As Object, e As ItemEventArgs) Implements IOggettoApprovabile.PresaInCarico


        'Azienda che ha registrato la pratica
        Private m_IDAzienda As Integer
        Private m_Azienda As CAzienda

        Private m_TipoFonteCliente As String
        Private m_IDFonteCliente As Integer
        Private m_FonteCliente As IFonte

        Private m_TipoFonteContatto As String
        Private m_IDFonte As Integer
        Private m_Fonte As IFonte
        Private m_NomeFonte As String

        Private m_Canale As CCanale
        Private m_IDCanale As Integer
        Private m_NomeCanale As String

        Private m_Canale1 As CCanale
        Private m_IDCanale1 As Integer
        Private m_NomeCanale1 As String

      

        Private m_ClienteID As Integer
        Private m_Cliente As CPersonaFisica
        Private m_NomeCliente As String
        Private m_CognomeCliente As String

        Private m_NatoA As CIndirizzo
        Private m_NatoIl As Date?
        Private m_ResidenteA As CIndirizzo
        Private m_CodiceFiscale As String
        Private m_PartitaIVA As String
        Private m_Sesso As String

        Private m_Telefono As String
        Private m_Cellulare As String
        Private m_Fax As String
        Private m_eMail As String
        'Private m_WebSite As String

        Private m_Impiego As CImpiegato

        Private m_NumeroEsterno As String
        Private m_Cessionario As CCQSPDCessionarioClass
        Private m_CessionarioID As Integer
        Private m_NomeCessionario As String

        Private m_Profilo As CProfilo
        Private m_ProfiloID As Integer
        Private m_NomeProfilo As String

        Private m_ProdottoID As Integer
        Private m_Prodotto As CCQSPDProdotto
        Private m_NomeProdotto As String

        Private m_MontanteLordo As Decimal?
        Private m_NettoRicavo As Decimal?
        Private m_NumeroRate As Integer?
        Private m_ValoreProvvMax As Decimal?
        Private m_ValoreRunning As Decimal?
        Private m_ValoreUpFront As Decimal?
        Private m_ValoreRappel As Decimal?
        Private m_Provvigionale As CProvvigionale

        Private m_DataDecorrenza As Date?
        Private m_Flags As PraticaFlags

        Private m_IDConsulente As Integer
        Private m_Consulente As CConsulentePratica

        Private m_StatoDiLavorazioneAttuale As CStatoLavorazionePratica
        Private m_StatiDiLavorazione As CStatiLavorazionePraticaCollection

        Private m_Info As CInfoPratica

        'Private m_MacroStato As Nullable(Of StatoPraticaEnum)

        Private m_IDConsulenza As Integer
        Private m_IDConsulenzaOld As Integer
        Private m_Consulenza As CQSPDConsulenza

        Private m_IDRichiestaDiFinanziamento As Integer
        Private m_RichiestaDiFinanziamento As CRichiestaFinanziamento
        Private m_TipoContesto As String
        Private m_IDContesto As Integer
        Private m_Durata As Integer

        Private m_IDRichiestaApprovazione As Integer
        Private m_RichiestaApprovazione As CRichiestaApprovazione

        Private m_Attributi As CKeyCollection

        Private m_Vincoli As CDocumentoPraticaCaricatoCollection

        Private m_Estinzioni As CEstinzioniXEstintoreCollection
        Private m_IDFinestraLavorazione As Integer
        Private m_FinestraLavorazione As FinestraLavorazione

        Private m_IDTabellaFinanziaria As Integer
        Private m_TabellaFinanziaria As CTabellaFinanziaria

        Private m_IDTabellaVita As Integer
        Private m_TabellaVita As CTabellaAssicurativa

        Private m_IDTabellaImpiego As Integer
        Private m_TabellaImpiego As CTabellaAssicurativa

        Private m_IDTabellaCredito As Integer
        Private m_TabellaCredito As CTabellaAssicurativa

        Private m_IDUltimaVerifica As Integer
        Private m_UltimaVerifica As VerificaAmministrativa

        Private m_DataValuta As Date?
        Private m_DataStampaSecci As Date?

        Private m_PremioDaCessionario As Decimal?               'Premio eventuale che viene corrisposto dal cessionario all'agenzia
        Private m_CapitaleFinanziato As Decimal?               'Capitale finanziato

        Private m_IDCollaboratore As Integer
        Private m_Collaboratore As CCollaboratore

        Public Sub New()
            Me.m_Canale = Nothing
            Me.m_IDCanale = 0
            Me.m_NomeCanale = vbNullString

            Me.m_Canale1 = Nothing
            Me.m_IDCanale1 = 0
            Me.m_NomeCanale1 = ""

            Me.m_ClienteID = 0
            Me.m_Cliente = Nothing
            Me.m_NomeCliente = vbNullString
            Me.m_CognomeCliente = vbNullString
            Me.m_NatoA = New CIndirizzo
            Me.m_NatoIl = Nothing
            Me.m_ResidenteA = New CIndirizzo
            Me.m_CodiceFiscale = vbNullString
            Me.m_PartitaIVA = vbNullString
            Me.m_Sesso = vbNullString
            Me.m_Telefono = vbNullString
            Me.m_Cellulare = vbNullString
            Me.m_Fax = vbNullString
            Me.m_eMail = vbNullString
            Me.m_Impiego = New CImpiegato

            Me.m_Cessionario = Nothing
            Me.m_CessionarioID = 0
            Me.m_NomeCessionario = vbNullString
            Me.m_Profilo = Nothing
            Me.m_ProfiloID = 0
            Me.m_NomeProfilo = vbNullString
            Me.m_Prodotto = Nothing
            Me.m_ProdottoID = 0
            Me.m_NomeProdotto = vbNullString

            Me.m_MontanteLordo = Nothing
            Me.m_NettoRicavo = Nothing
            Me.m_NumeroRate = Nothing
            Me.m_ValoreProvvMax = Nothing
            Me.m_ValoreRunning = Nothing
            Me.m_ValoreUpFront = Nothing
            Me.m_ValoreRappel = Nothing
            Me.m_Provvigionale = New CProvvigionale

            Me.m_DataDecorrenza = Nothing ' IIf(DateUtils.Day(DateUtils.Now) < 15, DateUtils.DateAdd(DateInterval.Day, 15, DateUtils.GetMonthFirstDay(Now)), DateUtils.GetNextMonthFirstDay(DateUtils.Now))
            Me.m_Flags = PraticaFlags.NOTSET
            Me.m_IDConsulente = 0
            Me.m_Consulente = Nothing

            Me.m_NumeroRate = Nothing
            Me.m_TipoFonteContatto = vbNullString
            Me.m_IDFonte = 0
            Me.m_Fonte = Nothing
            Me.m_NomeFonte = vbNullString



            Me.m_StatiDiLavorazione = Nothing
            Me.m_StatoDiLavorazioneAttuale = New CStatoLavorazionePratica
            Me.m_StatoDiLavorazioneAttuale.SetPratica(Me)
            Me.m_StatoDiLavorazioneAttuale.Stato = ObjectStatus.OBJECT_VALID
            Me.m_StatoDiLavorazioneAttuale.SetChanged(False)

            Me.m_Info = Nothing
            Me.m_NumeroEsterno = vbNullString

            Me.m_IDConsulenza = 0
            Me.m_IDConsulenzaOld = 0
            Me.m_Consulenza = Nothing

            Me.m_IDRichiestaDiFinanziamento = 0
            Me.m_RichiestaDiFinanziamento = Nothing

            Me.m_IDAzienda = 0
            Me.m_Azienda = Nothing
            Me.m_IDContesto = 0
            Me.m_TipoContesto = ""
            Me.m_Durata = 0

            Me.m_IDRichiestaApprovazione = 0
            Me.m_RichiestaApprovazione = Nothing

            Me.m_Attributi = New CKeyCollection
            Me.m_Vincoli = Nothing
            Me.m_Estinzioni = Nothing
            Me.m_IDFinestraLavorazione = 0
            Me.m_FinestraLavorazione = Nothing

            Me.m_IDTabellaFinanziaria = 0
            Me.m_TabellaFinanziaria = Nothing

            Me.m_IDTabellaVita = 0
            Me.m_TabellaVita = Nothing

            Me.m_IDTabellaImpiego = 0
            Me.m_TabellaImpiego = Nothing

            Me.m_IDTabellaCredito = 0
            Me.m_TabellaCredito = Nothing

            Me.m_IDUltimaVerifica = 0
            Me.m_UltimaVerifica = Nothing

            Me.m_DataValuta = Nothing
            Me.m_DataStampaSecci = Nothing

            Me.m_PremioDaCessionario = Nothing

            Me.m_CapitaleFinanziato = Nothing

            Me.m_IDCollaboratore = 0
            Me.m_Collaboratore = Nothing
        End Sub

        Public Property IDCollaboratore As Integer
            Get
                Return GetID(Me.m_Collaboratore, Me.m_IDCollaboratore)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDCollaboratore
                If (oldValue = value) Then Return
                Me.m_IDCollaboratore = value
                Me.m_Collaboratore = Nothing
                Me.DoChanged("IDCollaboratore", value, oldValue)
            End Set
        End Property

        Public Property Collaboratore As CCollaboratore
            Get
                If (Me.m_Collaboratore Is Nothing) Then Me.m_Collaboratore = CQSPD.Collaboratori.GetItemById(Me.m_IDCollaboratore)
                Return Me.m_Collaboratore
            End Get
            Set(value As CCollaboratore)
                Dim oldValue As CCollaboratore = Me.Collaboratore
                If (oldValue Is value) Then Return
                Me.m_Collaboratore = value
                Me.m_IDCollaboratore = GetID(value)
                Me.DoChanged("IDCollaboratore", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il capitale finanziato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CapitaleFinanziato As Decimal?
            Get
                Return Me.m_CapitaleFinanziato
            End Get
            Set(value As Decimal?)
                Dim oldValue As Decimal? = Me.m_CapitaleFinanziato
                If (oldValue = value) Then Exit Property
                Me.m_CapitaleFinanziato = value
                Me.DoChanged("CapitaleFinanziato", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il premio eventuale che il cessionario eroga all'agenzia per questa pratica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PremioDaCessionario As Decimal?
            Get
                Return Me.m_PremioDaCessionario
            End Get
            Set(value As Decimal?)
                Dim oldValue As Decimal? = Me.m_PremioDaCessionario
                If (oldValue = value) Then Exit Property
                Me.m_PremioDaCessionario = value
                Me.DoChanged("PremioDaCessionario", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il premio eventuale che il cessionario eroga all'agenzia per questa pratica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PremioDaCessionario1 As Decimal?
            Get
                Return Formats.ToValuta(Me.Attributi.GetItemByKey("PremioDaCessionario1"))
            End Get
            Set(value As Decimal?)
                Dim oldValue As Decimal? = Me.PremioDaCessionario1
                If (oldValue = value) Then Exit Property
                Me.Attributi.SetItemByKey("PremioDaCessionario1", value)
                Me.DoChanged("PremioDaCessionario1", value, oldValue)
            End Set
        End Property


        ''' <summary>
        ''' Restituisce o imposta la data valuta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataValuta As Date?
            Get
                Return Me.m_DataValuta
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataValuta
                If (DateUtils.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_DataValuta = value
                Me.DoChanged("DataValuta", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data in cui è stato stampato il SECCI associato alla pratica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataStampaSecci As Date?
            Get
                Return Me.m_DataStampaSecci
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataStampaSecci
                If (DateUtils.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_DataStampaSecci = value
                Me.DoChanged("DataStampaSecci", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'ultima verifica amministrativa
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDUltimaVerifica As Integer
            Get
                Return GetID(Me.m_UltimaVerifica, Me.m_IDUltimaVerifica)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDUltimaVerifica
                If (oldValue = value) Then Exit Property
                Me.m_IDUltimaVerifica = value
                Me.m_UltimaVerifica = Nothing
                Me.DoChanged("IDUltimaVerifica", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ultima verifica amministrativa
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property UltimaVerifica As VerificaAmministrativa Implements IOggettoVerificabile.UltimaVerifica
            Get
                If (Me.m_UltimaVerifica Is Nothing) Then Me.m_UltimaVerifica = CQSPD.VerificheAmministrative.GetItemById(Me.m_IDUltimaVerifica)
                Return Me.m_UltimaVerifica
            End Get
            Set(value As VerificaAmministrativa)
                Dim oldValue As VerificaAmministrativa = Me.m_UltimaVerifica
                If (oldValue Is value) Then Exit Property
                Me.m_UltimaVerifica = value
                Me.m_IDUltimaVerifica = GetID(value)
                Me.DoChanged("UltimaVerifica", value, oldValue)
            End Set
        End Property

        Protected Friend Sub SetUltimaVerifica(ByVal value As VerificaAmministrativa)
            Me.m_UltimaVerifica = value
            Me.m_IDUltimaVerifica = GetID(value)
        End Sub

        ''' <summary>
        ''' Restituisce o imposta l'ID della tabella finanziaria associata all'offerta corrente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDTabellaFinanziaria As Integer
            Get
                Return GetID(Me.m_TabellaFinanziaria, Me.m_IDTabellaFinanziaria)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDTabellaFinanziaria
                If (oldValue = value) Then Exit Property
                Me.m_IDTabellaFinanziaria = value
                Me.m_TabellaFinanziaria = Nothing
                Me.DoChanged("IDTabellaFinanziaria", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la tabella finanziaria associata all'offerta corrente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TabellaFinanziaria As CTabellaFinanziaria
            Get
                If (Me.m_TabellaFinanziaria Is Nothing) Then Me.m_TabellaFinanziaria = CQSPD.TabelleFinanziarie.GetItemById(Me.m_IDTabellaFinanziaria)
                Return Me.m_TabellaFinanziaria
            End Get
            Set(value As CTabellaFinanziaria)
                Dim oldValue As CTabellaFinanziaria = Me.TabellaFinanziaria
                If (oldValue Is value) Then Exit Property
                Me.m_TabellaFinanziaria = value
                Me.m_IDTabellaFinanziaria = GetID(value)
                Me.DoChanged("TabellaFinanziaria", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID della tabella assicurativa usata per il rischio vita
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDTabellaVita As Integer
            Get
                Return GetID(Me.m_TabellaVita, Me.m_IDTabellaVita)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDTabellaVita
                If (oldValue = value) Then Exit Property
                Me.m_IDTabellaVita = value
                Me.m_TabellaVita = Nothing
                Me.DoChanged("IDTabellaVita", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la tabella Vita associata all'offerta corrente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TabellaVita As CTabellaAssicurativa
            Get
                If (Me.m_TabellaVita Is Nothing) Then Me.m_TabellaVita = CQSPD.TabelleAssicurative.GetItemById(Me.m_IDTabellaVita)
                Return Me.m_TabellaVita
            End Get
            Set(value As CTabellaAssicurativa)
                Dim oldValue As CTabellaAssicurativa = Me.TabellaVita
                If (oldValue Is value) Then Exit Property
                Me.m_TabellaVita = value
                Me.m_IDTabellaVita = GetID(value)
                Me.DoChanged("TabellaVita", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID della tabella assicurativa usata per il rischio Impiego
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDTabellaImpiego As Integer
            Get
                Return GetID(Me.m_TabellaImpiego, Me.m_IDTabellaImpiego)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDTabellaImpiego
                If (oldValue = value) Then Exit Property
                Me.m_IDTabellaImpiego = value
                Me.m_TabellaImpiego = Nothing
                Me.DoChanged("IDTabellaImpiego", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la tabella Impiego associata all'offerta corrente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TabellaImpiego As CTabellaAssicurativa
            Get
                If (Me.m_TabellaImpiego Is Nothing) Then Me.m_TabellaImpiego = CQSPD.TabelleAssicurative.GetItemById(Me.m_IDTabellaImpiego)
                Return Me.m_TabellaImpiego
            End Get
            Set(value As CTabellaAssicurativa)
                Dim oldValue As CTabellaAssicurativa = Me.TabellaImpiego
                If (oldValue Is value) Then Exit Property
                Me.m_TabellaImpiego = value
                Me.m_IDTabellaImpiego = GetID(value)
                Me.DoChanged("TabellaImpiego", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID della tabella assicurativa usata per il rischio Credito
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDTabellaCredito As Integer
            Get
                Return GetID(Me.m_TabellaCredito, Me.m_IDTabellaCredito)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDTabellaCredito
                If (oldValue = value) Then Exit Property
                Me.m_IDTabellaCredito = value
                Me.m_TabellaCredito = Nothing
                Me.DoChanged("IDTabellaCredito", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la tabella Credito associata all'offerta corrente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TabellaCredito As CTabellaAssicurativa
            Get
                If (Me.m_TabellaCredito Is Nothing) Then Me.m_TabellaCredito = CQSPD.TabelleAssicurative.GetItemById(Me.m_IDTabellaCredito)
                Return Me.m_TabellaCredito
            End Get
            Set(value As CTabellaAssicurativa)
                Dim oldValue As CTabellaAssicurativa = Me.TabellaCredito
                If (oldValue Is value) Then Exit Property
                Me.m_TabellaCredito = value
                Me.m_IDTabellaCredito = GetID(value)
                Me.DoChanged("TabellaCredito", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID della finestra di lavorazione in cui è stata lavorata la pratica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDFinestraLavorazione As Integer
            Get
                Return GetID(Me.m_FinestraLavorazione, Me.m_IDFinestraLavorazione)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDFinestraLavorazione
                If (oldValue = value) Then Exit Property
                Me.m_IDFinestraLavorazione = value
                Me.m_FinestraLavorazione = Nothing
                Me.DoChanged("IDFinestraLavorazione", value, oldValue)
            End Set
        End Property

        Public Property FinestraLavorazione As FinestraLavorazione
            Get
                If (Me.m_FinestraLavorazione Is Nothing) Then Me.m_FinestraLavorazione = CQSPD.FinestreDiLavorazione.GetItemById(Me.m_IDFinestraLavorazione)
                Return Me.m_FinestraLavorazione
            End Get
            Set(value As FinestraLavorazione)
                Dim oldValue As FinestraLavorazione = Me.m_FinestraLavorazione
                If (oldValue Is value) Then Exit Property
                Me.m_FinestraLavorazione = value
                Me.m_IDFinestraLavorazione = GetID(value)
                Me.DoChanged("FinestraLavorazione", value, oldValue)
            End Set
        End Property

        Protected Friend Overridable Sub SetFinestraLavorazione(ByVal value As FinestraLavorazione)
            Me.m_FinestraLavorazione = value
            Me.m_IDFinestraLavorazione = GetID(value)
        End Sub

        Public ReadOnly Property Estinzioni As CEstinzioniXEstintoreCollection Implements IEstintore.Estinzioni
            Get
                SyncLock Me
                    If (Me.m_Estinzioni Is Nothing) Then Me.m_Estinzioni = New CEstinzioniXEstintoreCollection(Me)
                    Return Me.m_Estinzioni
                End SyncLock
            End Get
        End Property

        Protected Friend Overridable Sub SetEstinzioni(ByVal value As CEstinzioniXEstintoreCollection)
            Me.m_Estinzioni = value
        End Sub

        Public ReadOnly Property Attributi As CKeyCollection
            Get
                Return Me.m_Attributi
            End Get
        End Property

        ''' <summary>
        ''' Restituisce le informazioni relative al provvigionale riconosciuto al collaboratore
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Provvigionale As CProvvigionale
            Get
                Return Me.m_Provvigionale
            End Get
        End Property

        Public Property Durata As Integer
            Get
                Return Me.m_Durata
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_Durata
                If (oldValue = value) Then Exit Property
                Me.m_Durata = value
                Me.DoChanged("Durata", value, oldValue)
            End Set
        End Property

        Public Property IDContesto As Integer
            Get
                Return Me.m_IDContesto
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_IDContesto
                If (oldValue = value) Then Exit Property
                Me.m_IDContesto = value
                Me.DoChanged("IDContesto", value, oldValue)
            End Set
        End Property

        Public Property TipoContesto As String
            Get
                Return Me.m_TipoContesto
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_TipoContesto
                If (oldValue = value) Then Exit Property
                Me.m_TipoContesto = value
                Me.DoChanged("TipoContesto", value, oldValue)
            End Set
        End Property


        ''' <summary>
        ''' Restituisce o imposta l'ID dell'azienda che ha registrato la pratica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDAzienda As Integer
            Get
                Return GetID(Me.m_Azienda, Me.m_IDAzienda)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDAzienda
                If (oldValue = value) Then Exit Property
                Me.m_IDAzienda = value
                Me.m_Azienda = Nothing
                Me.DoChanged("IDAzienda", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'azienda che ha registrato la pratica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Azienda As CAzienda
            Get
                If (Me.m_Azienda Is Nothing) Then Me.m_Azienda = Anagrafica.Aziende.GetItemById(Me.m_IDAzienda)
                Return Me.m_Azienda
            End Get
            Set(value As CAzienda)
                Dim oldValue As CAzienda = Me.m_Azienda
                If (oldValue Is value) Then Exit Property
                Me.m_Azienda = value
                Me.m_IDAzienda = GetID(value)
                Me.DoChanged("Azienda", value, oldValue)
            End Set
        End Property


        ''' <summary>
        ''' Restituisce o imposta l'ID della consulenza da cui è partita la pratica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDConsulenza As Integer
            Get
                Return GetID(Me.m_Consulenza, Me.m_IDConsulenza)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDConsulenza
                If (oldValue = value) Then Exit Property
                Me.m_IDConsulenza = value
                Me.m_Consulenza = Nothing
                Me.DoChanged("IDConsulenza", value, oldValue)
            End Set
        End Property

        Protected Overrides Sub DoChanged(propName As String, Optional newVal As Object = Nothing, Optional oldVal As Object = Nothing)
            MyBase.DoChanged(propName, newVal, oldVal)
        End Sub

        ''' <summary>
        ''' Restituisce o imposta la consulenza da cui è partita la pratica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Consulenza As CQSPDConsulenza
            Get
                If (Me.m_Consulenza Is Nothing) Then Me.m_Consulenza = CQSPD.Consulenze.GetItemById(Me.m_IDConsulenza)
                Return Me.m_Consulenza
            End Get
            Set(value As CQSPDConsulenza)
                Dim oldValue As CQSPDConsulenza = Me.m_Consulenza
                If (oldValue Is value) Then Exit Property
                Me.m_Consulenza = value
                Me.m_IDConsulenza = GetID(value)
                Me.DoChanged("Consulenza", value, oldValue)
            End Set
        End Property

        Protected Friend Overridable Sub SetConsulenza(ByVal value As CQSPDConsulenza)
            Me.m_Consulenza = value
            Me.m_IDConsulenza = GetID(value)
        End Sub

        ''' <summary>
        ''' Restituisce o imposta l'ID del consulente principale della pratica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDConsulente As Integer
            Get
                Return GetID(Me.m_Consulente, Me.m_IDConsulente)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDConsulente
                If oldValue = value Then Exit Property
                Me.m_IDConsulente = value
                Me.m_Consulente = Nothing
                Me.DoChanged("IDConsulente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il consulente principale della pratica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Consulente As CConsulentePratica
            Get
                If Me.m_Consulente Is Nothing Then Me.m_Consulente = DMD.CQSPD.Consulenti.GetItemById(Me.m_IDConsulente)
                Return Me.m_Consulente
            End Get
            Set(value As CConsulentePratica)
                Dim oldValue As CConsulentePratica = Me.m_Consulente
                If (oldValue = value) Then Exit Property
                Me.m_Consulente = value
                Me.m_IDConsulente = GetID(value)
                'If (value IsNot Nothing) Then Me.m_NomeConsulente = value.Nome
                Me.DoChanged("Consulente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce il nome del consulente principale della pratica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property NomeConsulente As String
            Get
                If (Me.Consulente Is Nothing) Then Return ""
                Return Me.Consulente.Nome
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID della richiesta di finanziamento da cui è partita la pratica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDRichiestaDiFinanziamento As Integer
            Get
                Return GetID(Me.m_RichiestaDiFinanziamento, Me.m_IDRichiestaDiFinanziamento)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDRichiestaDiFinanziamento
                If (oldValue = value) Then Exit Property
                Me.m_IDRichiestaDiFinanziamento = value
                Me.m_RichiestaDiFinanziamento = Nothing
                Me.DoChanged("IDRichiestaDiFinanziamento", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la richiesta di finanziamento da cui è partita la pratica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property RichiestaDiFinanziamento As CRichiestaFinanziamento
            Get
                If (Me.m_RichiestaDiFinanziamento Is Nothing) Then Me.m_RichiestaDiFinanziamento = CQSPD.RichiesteFinanziamento.GetItemById(Me.m_IDRichiestaDiFinanziamento)
                Return Me.m_RichiestaDiFinanziamento
            End Get
            Set(value As CRichiestaFinanziamento)
                Dim oldValue As CRichiestaFinanziamento = Me.m_RichiestaDiFinanziamento
                If (oldValue Is value) Then Exit Property
                Me.m_RichiestaDiFinanziamento = value
                Me.m_IDRichiestaDiFinanziamento = GetID(value)
                Me.DoChanged("RichiestaDiFinanziamento", value, oldValue)
            End Set
        End Property

        Protected Friend Overridable Sub SetRichiestaDiFinanziamento(ByVal value As CRichiestaFinanziamento)
            Me.m_RichiestaDiFinanziamento = value
            Me.m_IDRichiestaDiFinanziamento = GetID(value)
        End Sub

        ''' <summary>
        ''' Restituisce o imposta il numero pratica in un eventuale sistema esterno su cui è caricata
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NumeroEsterno As String
            Get
                Return Me.m_NumeroEsterno
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NumeroEsterno
                If (oldValue = value) Then Exit Property
                Me.m_NumeroEsterno = value
                Me.DoChanged("NumeroPraticaEsterna", value, oldValue)
            End Set
        End Property

        Public ReadOnly Property Info As CInfoPratica
            Get
                If (Me.m_Info Is Nothing) Then Me.m_Info = DMD.CQSPD.InfoPratica.GetItemByPratica(Me)
                If (Me.m_Info Is Nothing) Then
                    Me.m_Info = New CInfoPratica
                    Me.m_Info.Pratica = Me
                    Me.m_Info.Save()
                End If
                Return Me.m_Info
            End Get
        End Property

        Protected Friend Sub SetInfo(ByVal value As CInfoPratica)
            Me.m_Info = value
        End Sub

        ''' <summary>
        ''' Restituisce o imposta l'ID della richiesta di approvazione corrente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDRichiestaApprovazione As Integer Implements IOggettoApprovabile.IDRichiestaApprovazione
            Get
                Return GetID(Me.m_RichiestaApprovazione, Me.m_IDRichiestaApprovazione)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDRichiestaApprovazione
                If (oldValue = value) Then Exit Property
                Me.m_IDRichiestaApprovazione = value
                Me.m_RichiestaApprovazione = Nothing
                Me.DoChanged("IDRichiestaApprovazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la richiesta di approvazione corrente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property RichiestaApprovazione As CRichiestaApprovazione Implements IOggettoApprovabile.RichiestaApprovazione
            Get
                If (Me.m_RichiestaApprovazione Is Nothing) Then
                    Me.m_RichiestaApprovazione = CQSPD.RichiesteApprovazione.GetItemById(Me.m_IDRichiestaApprovazione)
                    If (Me.m_RichiestaApprovazione IsNot Nothing) Then Me.m_RichiestaApprovazione.SetOggettoApprovabile(Me)
                End If
                Return Me.m_RichiestaApprovazione
            End Get
            Set(value As CRichiestaApprovazione)
                Dim oldValue As CRichiestaApprovazione = Me.m_RichiestaApprovazione
                If (oldValue Is value) Then Exit Property
                Me.m_RichiestaApprovazione = value
                Me.m_IDRichiestaApprovazione = GetID(value)
                If (value IsNot Nothing) Then value.SetOggettoApprovabile(Me)
                Me.DoChanged("RichiestaApprovazione", value, oldValue)
            End Set
        End Property

        Protected Friend Overridable Sub SetRichiestaApprovazione(ByVal value As CRichiestaApprovazione)
            Me.m_RichiestaApprovazione = value
            Me.m_IDRichiestaApprovazione = GetID(value)
        End Sub


        ''' <summary>
        ''' Genera una richiesta di approvazione
        ''' </summary>
        ''' <param name="motivo"></param>
        ''' <param name="dettaglio"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function RichiediApprovazione(ByVal motivo As String, ByVal dettaglio As String, ByVal parametri As String) As CRichiestaApprovazione Implements IOggettoApprovabile.RichiediApprovazione
            'If (Me.OffertaCorrente.StatoOfferta = StatoOfferta.APPROVATA) OrElse (Me.OffertaCorrente.StatoOfferta = StatoOfferta.RIFIUTATA) Then
            '    Throw New InvalidOperationException("L'offerta corrente è già stata supervisionata")
            'End If
            Dim rich As CRichiestaApprovazione = Me.RichiestaApprovazione

            If (rich IsNot Nothing) AndAlso (rich.StatoRichiesta >= StatoRichiestaApprovazione.ATTESA) Then
                Throw New InvalidOperationException("La pratica è già in attesa di approvazione")
            End If

            If (rich Is Nothing) Then rich = New CRichiestaApprovazione
            rich.Cliente = Me.Cliente
            rich.OggettoApprovabile = Me
            rich.DataRichiestaApprovazione = DateUtils.Now
            rich.UtenteRichiestaApprovazione = Sistema.Users.CurrentUser
            rich.StatoRichiesta = StatoRichiestaApprovazione.ATTESA
            rich.MotivoRichiesta = CQSPD.MotiviSconto.GetItemByName(motivo)
            rich.NomeMotivoRichiesta = motivo
            rich.ParametriRichiesta = parametri
            rich.DescrizioneRichiesta = dettaglio
            rich.Stato = ObjectStatus.OBJECT_VALID
            rich.PuntoOperativo = Me.PuntoOperativo
            rich.StatoRichiesta = StatoRichiestaApprovazione.ATTESA
            rich.Save()

            'Me.m_Flags = Sistema.SetFlag(Me.m_Flags, PraticaFlags.RICHIEDEAPPROVAZIONE, True)
            'Me.m_Flags = Sistema.SetFlag(Me.m_Flags, PraticaFlags.APPROVATA, False)
            Me.RichiestaApprovazione = rich

            Me.Save()

            Dim e As New ItemEventArgs(Me)
            Me.OnRequireApprovation(e)
            CQSPD.Pratiche.DoOnRequireApprovation(e)

            Return rich
        End Function

        Public Sub Sollecita()
            Dim rich As CRichiestaApprovazione = Me.RichiestaApprovazione
            If (rich Is Nothing OrElse rich.Stato <> ObjectStatus.OBJECT_VALID) Then Throw New ArgumentNullException("RichiestaApprovazione")
            Select Case rich.StatoRichiesta
                Case StatoRichiestaApprovazione.NONCHIESTA, StatoRichiestaApprovazione.ANNULLATA
                    rich.StatoRichiesta = StatoRichiestaApprovazione.ATTESA
                Case StatoRichiestaApprovazione.APPROVATA, StatoRichiestaApprovazione.NEGATA
                    Throw New ArgumentException("Non puoi sollecitare delle richieste già negate o approvate")
            End Select
            rich.DescrizioneRichiesta = Strings.Combine(rich.DescrizioneRichiesta, Formats.FormatUserDateTime(DateUtils.Now) & " - sollecitata da " & Sistema.Users.CurrentUser.Nominativo, vbNewLine)
            rich.Save()

            'Me.Save()
            Dim e As New ItemEventArgs(Me)
            Me.OnRequireApprovation(e)
            CQSPD.Pratiche.DoOnRequireApprovation(e)
            Me.GetModule.DispatchEvent(New EventDescription("require_approvation", Users.CurrentUser.Nominativo & " richiede l'approvazione dell'offerta fatta per la pratica ID: " & GetID(Me), Me))

        End Sub

        Protected Overridable Sub OnRequireApprovation(ByVal e As ItemEventArgs)
            RaiseEvent RequireApprovation(Me, e)
        End Sub


        ''' <summary>
        ''' Approva l'offerta corrente (se l'offerta non richiede l'approvazione genera un errore)
        ''' </summary>
        ''' <remarks></remarks>
        Public Function Approva(ByVal motivo As String, ByVal dettaglio As String) As CRichiestaApprovazione Implements IOggettoApprovabile.Approva
            'If (Me.OffertaCorrente.StatoOfferta = StatoOfferta.APPROVATA) OrElse (Me.OffertaCorrente.StatoOfferta = StatoOfferta.RIFIUTATA) Then
            '    Throw New InvalidOperationException("L'offerta corrente è già stata supervisionata")
            'End If
            If Me.RichiestaApprovazione Is Nothing OrElse Me.RichiestaApprovazione.StatoRichiesta = StatoRichiestaApprovazione.APPROVATA OrElse Me.RichiestaApprovazione.StatoRichiesta = StatoRichiestaApprovazione.NEGATA Then
                Throw New InvalidOperationException("La pratica non richiede l'approvazione o è già stata valutata")
            End If

            Me.RichiestaApprovazione.MotivoConferma = motivo
            Me.RichiestaApprovazione.DettaglioConferma = dettaglio
            Me.RichiestaApprovazione.ConfermataDa = Sistema.Users.CurrentUser
            Me.RichiestaApprovazione.DataConferma = DateUtils.Now
            Me.RichiestaApprovazione.StatoRichiesta = StatoRichiestaApprovazione.APPROVATA
            Me.RichiestaApprovazione.Save()

            Dim note As New CAnnotazione(Me.RichiestaApprovazione)
            note.Valore = "<b>RICHIESTA APPROVATA</b><br/><b>Motivo</b>: " & motivo & "<br/><b>Dettaglio:</b> " & dettaglio
            note.Stato = ObjectStatus.OBJECT_VALID
            note.Save()

            Me.OffertaCorrente.StatoOfferta = StatoOfferta.APPROVATA
            Me.OffertaCorrente.DataConfermaSconto = DateUtils.Now()
            Me.OffertaCorrente.MotivoConfermaSconto = motivo
            Me.OffertaCorrente.DettaglioConfermaSconto = dettaglio
            Me.OffertaCorrente.Supervisore = Users.CurrentUser
            Me.OffertaCorrente.Save()

            Me.Info.ScontoAutorizzatoDa = Users.CurrentUser
            Me.Info.ScontoAutorizzatoIl = Me.OffertaCorrente.DataConfermaSconto
            Me.Info.ScontoAutorizzatoNote = dettaglio
            Me.Save()


            Dim e As New ItemEventArgs(Me)
            Me.OnApproved(e)
            CQSPD.Pratiche.DoOnApprovata(e)
            Me.GetModule.DispatchEvent(New EventDescription("Approved", Users.CurrentUser.Nominativo & " ha approvato l'offerta fatta per la pratica N°" & Me.NumeroPratica, Me))

            Return Me.RichiestaApprovazione
        End Function


        ''' <summary>
        ''' Nega l'offerta corrente (se l'offerta non richiede l'approvazione genera un errore)
        ''' </summary>
        ''' <remarks></remarks>
        Public Function Nega(ByVal motivo As String, ByVal dettaglio As String) As CRichiestaApprovazione Implements IOggettoApprovabile.Nega
            'If (Me.OffertaCorrente.StatoOfferta = StatoOfferta.APPROVATA) OrElse (Me.OffertaCorrente.StatoOfferta = StatoOfferta.RIFIUTATA) Then
            '    Throw New InvalidOperationException("L'offerta corrente è già stata supervisionata")
            'End If
            If Me.RichiestaApprovazione Is Nothing OrElse Me.RichiestaApprovazione.StatoRichiesta = StatoRichiestaApprovazione.APPROVATA OrElse Me.RichiestaApprovazione.StatoRichiesta = StatoRichiestaApprovazione.NEGATA Then
                Throw New InvalidOperationException("La pratica non richiede l'approvazione o è già stata valutata")
            End If

            Me.RichiestaApprovazione.MotivoConferma = motivo
            Me.RichiestaApprovazione.DettaglioConferma = dettaglio
            Me.RichiestaApprovazione.ConfermataDa = Sistema.Users.CurrentUser
            Me.RichiestaApprovazione.DataConferma = DateUtils.Now
            Me.RichiestaApprovazione.StatoRichiesta = StatoRichiestaApprovazione.NEGATA
            Me.RichiestaApprovazione.Save()

            Dim note As New CAnnotazione(Me.RichiestaApprovazione)
            note.Valore = "<b>RICHIESTA NEGATA</b><br/><b>Motivo</b>: " & motivo & "<br/><b>Dettaglio:</b> " & dettaglio
            note.Stato = ObjectStatus.OBJECT_VALID
            note.Save()

            Me.OffertaCorrente.StatoOfferta = StatoOfferta.RIFIUTATA
            Me.OffertaCorrente.DataConfermaSconto = DateUtils.Now()
            Me.OffertaCorrente.MotivoConfermaSconto = motivo
            Me.OffertaCorrente.DettaglioConfermaSconto = dettaglio
            Me.OffertaCorrente.Supervisore = Users.CurrentUser
            Me.OffertaCorrente.Save()

            Me.Info.ScontoAutorizzatoDa = Users.CurrentUser
            Me.Info.ScontoAutorizzatoIl = Me.OffertaCorrente.DataConfermaSconto
            Me.Info.ScontoAutorizzatoNote = dettaglio
            Me.Save()

            Dim e As New ItemEventArgs(Me)
            Me.OnRifiutata(e)
            CQSPD.Pratiche.DoOnRifiutata(e)
            Me.GetModule.DispatchEvent(New EventDescription("Rifiutata", Users.CurrentUser.Nominativo & " ha rifiutato l'offerta fatta per la pratica N°" & Me.NumeroPratica, Me))

            Return Me.RichiestaApprovazione
        End Function

        ''' <summary>
        ''' Prende in carico la richiesta di approvazione corrente
        ''' </summary>
        ''' <remarks></remarks>
        Public Function PrendiInCarico() As CRichiestaApprovazione Implements IOggettoApprovabile.PrendiInCarico
            'If (Me.OffertaCorrente.StatoOfferta = StatoOfferta.APPROVATA) OrElse (Me.OffertaCorrente.StatoOfferta = StatoOfferta.RIFIUTATA) Then
            '    Throw New InvalidOperationException("L'offerta corrente è già stata supervisionata")
            'End If
            If Me.RichiestaApprovazione Is Nothing OrElse Me.RichiestaApprovazione.StatoRichiesta <> StatoRichiestaApprovazione.ATTESA Then
                Throw New InvalidOperationException("La pratica non è in attesa di valutazione")
            End If

            Me.RichiestaApprovazione.PresaInCaricoDa = Sistema.Users.CurrentUser
            Me.RichiestaApprovazione.DataPresaInCarico = DateUtils.Now
            Me.RichiestaApprovazione.StatoRichiesta = StatoRichiestaApprovazione.PRESAINCARICO
            Me.RichiestaApprovazione.Save()

            Dim note As New CAnnotazione(Me.RichiestaApprovazione)
            note.Valore = "<b>VALUTAZIONE IN CORSO</b><br/><b>Motivo</b>"
            note.Stato = ObjectStatus.OBJECT_VALID
            note.Save()


            Dim e As New ItemEventArgs(Me)
            Me.OnPresaInCarico(e)
            CQSPD.Pratiche.DoOnInCarico(e)
            Me.GetModule.DispatchEvent(New EventDescription("Presa_in_carico", Users.CurrentUser.Nominativo & " ha preso in carico la pratica N°" & Me.NumeroPratica, Me))

            Return Me.RichiestaApprovazione
        End Function

        Protected Overridable Sub OnPresaInCarico(ByVal e As ItemEventArgs)
            RaiseEvent PresaInCarico(Me, e)
        End Sub

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se la pratica è visibile nell'interfaccia
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Visible As Boolean
            Get
                Return TestFlag(Me.m_Flags, PraticaFlags.HIDDEN) = False
            End Get
            Set(value As Boolean)
                If (value = Me.Visible) Then Exit Property
                Me.m_Flags = SetFlag(Me.m_Flags, PraticaFlags.HIDDEN, Not value)
                Me.DoChanged("Visible", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il canale attraverso cui è pervenuto questo contatto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Canale As CCanale
            Get
                If (Me.m_Canale Is Nothing) Then Me.m_Canale = Anagrafica.Canali.GetItemById(Me.m_IDCanale)
                Return Me.m_Canale
            End Get
            Set(value As CCanale)
                Dim oldValue As CCanale = Me.Canale
                If (oldValue Is value) Then Exit Property
                Me.m_Canale = value
                Me.m_IDCanale = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeCanale = value.Nome
                Me.DoChanged("Canale", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il canale attraverso cui è pervenuto questo contatto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Canale1 As CCanale
            Get
                If (Me.m_Canale1 Is Nothing) Then Me.m_Canale1 = Anagrafica.Canali.GetItemById(Me.m_IDCanale)
                Return Me.m_Canale1
            End Get
            Set(value As CCanale)
                Dim oldValue As CCanale = Me.Canale1
                If (oldValue Is value) Then Exit Property
                Me.m_Canale1 = value
                Me.m_IDCanale1 = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeCanale1 = value.Nome
                Me.DoChanged("Canale1", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID del canale 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDCanale As Integer
            Get
                Return GetID(Me.m_Canale, Me.m_IDCanale)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDCanale
                If (oldValue = value) Then Exit Property
                Me.m_IDCanale = value
                Me.m_Canale = Nothing
                Me.DoChanged("IDCanale", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID del canale 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDCanale1 As Integer
            Get
                Return GetID(Me.m_Canale1, Me.m_IDCanale1)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDCanale1
                If (oldValue = value) Then Exit Property
                Me.m_IDCanale1 = value
                Me.m_Canale1 = Nothing
                Me.DoChanged("IDCanale1", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del canale
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeCanale As String
            Get
                Return Me.m_NomeCanale
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomeCanale
                If (oldValue = value) Then Exit Property
                Me.m_NomeCanale = value
                Me.DoChanged("NomeCanale", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del canale
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeCanale1 As String
            Get
                Return Me.m_NomeCanale1
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomeCanale1
                If (oldValue = value) Then Exit Property
                Me.m_NomeCanale1 = value
                Me.DoChanged("NomeCanale1", value, oldValue)
            End Set
        End Property

        Public Overrides Function GetModule() As CModule
            Return DMD.CQSPD.Pratiche.Module
        End Function

        Public Function GetAnnotazioni() As CAnnotazioni
            Dim ret As New CAnnotazioni(Me.Cliente, Me)
            Return ret
        End Function

        Public Function GetAttachments() As CAttachments
            Dim ret As New CAttachments(Me.Cliente, Me)
            Return ret
        End Function



        ''' <summary>
        ''' Restituisce il numero della pratica nel sistema interno
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property NumeroPratica As String
            Get
                Return Right("00000000" & Hex(Me.ID), 8)
            End Get
        End Property


        Public ReadOnly Property DataRicontatto As Date?
            Get
                Dim ret As Date? = Me.DataRinnovo
                If (ret.HasValue) Then
                    Dim giorniAnticipo As Integer = CQSPD.Configuration().GiorniAnticipoRifin()
                    ret = DateUtils.DateAdd("d", -giorniAnticipo, ret)
                End If
                Return ret
            End Get
        End Property

        Public ReadOnly Property DataRinnovo As Date?
            Get
                Dim ret As Date? = Nothing
                If (Me.DataDecorrenza().HasValue AndAlso Me.NumeroRate.HasValue AndAlso Me.NumeroRate.Value > 0) Then
                    Dim percDurata As Double = CQSPD.Configuration().PercCompletamentoRifn()
                    ret = DateUtils.DateAdd("M", Math.round(Me.NumeroRate.Value * percDurata / 100), DateUtils.GetMonthFirstDay(Me.DataDecorrenza()))
                End If
                Return ret
            End Get
        End Property

        Public Property DataDecorrenza As Nullable(Of Date) Implements IEstintore.DataDecorrenza
            Get
                Return Me.m_DataDecorrenza
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataDecorrenza
                If (oldValue = value) Then Exit Property
                Me.m_DataDecorrenza = value
                Me.DoChanged("DataDecorrenza", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se la pratica è attenzionata
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DaVedere As Boolean
            Get
                Return TestFlag(Me.m_Flags, PraticaFlags.DAVEDERE)
            End Get
            Set(value As Boolean)
                If (Me.DaVedere = value) Then Exit Property
                Me.m_Flags = SetFlag(Me.m_Flags, PraticaFlags.DAVEDERE, value)
                Me.DoChanged("DaVedere", value, Not value)
            End Set
        End Property


        ''' <summary>
        ''' Restituisce o imposta l'offerta corrente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property OffertaCorrente As COffertaCQS
            Get
                Return Me.StatoDiLavorazioneAttuale.Offerta
            End Get
            Set(value As COffertaCQS)
                Me.StatoDiLavorazioneAttuale.Offerta = value
            End Set
        End Property

        Protected Friend Overridable Sub SetOffertaCorrente(ByVal value As COffertaCQS)
            Me.StatoDiLavorazioneAttuale.SetOfferta(value)
        End Sub

        Public Property IDOffertaCorrente As Integer
            Get
                Return Me.StatoDiLavorazioneAttuale.IDOfferta
            End Get
            Set(value As Integer)
                Me.StatoDiLavorazioneAttuale.IDOfferta = value
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la collezione dei documenti caricabili o caricati per la pratica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Vincoli As CDocumentoPraticaCaricatoCollection
            Get
                If Me.m_Vincoli Is Nothing Then Me.m_Vincoli = New CDocumentoPraticaCaricatoCollection(Me)
                Return Me.m_Vincoli
            End Get
        End Property

        ''' <summary>
        ''' Aggiorna i dati dell'anagrafica cliente sulla base dei dati di questa pratica
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub AggiornaAnagraficaCliente(ByVal persona As CPersonaFisica)
            With persona
                .Nome = Me.NomeCliente
                .Cognome = Me.CognomeCliente
                .Sesso = Me.Sesso
                .DataNascita = Me.NatoIl
                .CodiceFiscale = Me.CodiceFiscale
                .PartitaIVA = Me.PartitaIVA
                .NatoA.Citta = Me.NatoA.Citta
                .NatoA.Provincia = Me.NatoA.Provincia
                .ResidenteA.Citta = Me.ResidenteA.Citta
                .ResidenteA.Provincia = Me.ResidenteA.Provincia
                .ResidenteA.CAP = Me.ResidenteA.CAP
                .ResidenteA.ToponimoViaECivico = Me.ResidenteA.ToponimoViaECivico
                '.TelefonoCasa(0).Valore = Me.Telefono
                '.Cellulare(0).Valore = Me.Cellulare
                '.Fax(0).Valore = Me.Fax
                '.eMail(0).Valore = Me.eMail
                ' .WebSite(0).Valore = Me.WebSite
            End With
        End Sub

        ''' <summary>
        ''' Cerca di trovare il cliente corrispondente alla pratica in oggetto
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function TrovaCliente() As CPersona
            Dim ret As CPersona
            Dim col As CCollection(Of CPersona)
            ret = Me.Cliente
            If (ret Is Nothing) Then
                If (Me.CodiceFiscale <> "") Then
                    col = Anagrafica.Persone.FindPersoneByCF(Me.CodiceFiscale)
                    If col.Count > 0 Then
                        ret = col.Item(0)
                    End If
                End If
            End If
            Return ret
        End Function

        ' ''' <summary>
        ' ''' Cambia lo stato della pratica attuale e restituisce un oggetto CStatoLavorazionePratica che rappresenta il nuovo stato di lavorazione
        ' ''' </summary>
        ' ''' <param name="nuovoStato"></param>
        ' ''' <param name="data"></param>
        ' ''' <param name="params"></param>
        ' ''' <param name="note"></param>
        ' ''' <param name="forza"></param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Function ChangeStatus(ByVal nuovoStato As CStatoLavorazionePratica, ByVal data As Date, ByVal params As String, ByVal note As String, ByVal forza As Boolean) As CStatoLavorazionePratica
        '    Dim ret As New CStatoLavorazionePratica
        '    ret.Pratica = Me
        '    ret.StatoPratica = nuovoStato
        '    ret.Data = data
        '    ret.Parameters = params
        '    ret.Note = note
        '    ret.Forzato = forza
        '    ret.Operatore = Users.CurrentUser
        '    ret.Stato = ObjectStatus.OBJECT_VALID
        '    Call Databases.Save(ret, GetCRMConnection)
        '    Me.m_StatoPratica = ret
        '    Me.m_IDStatoPratica = Databases.GetID(Me.m_StatoPratica, 0)
        '    If Not (Me.m_StatiLavorazione Is Nothing) Then
        '        Call Me.m_StatiLavorazione.Add(ret)
        '    End If
        '    Call Databases.Save(Me, GetCRMConnection)
        '    Return ret
        'End Function

        ' ''' <summary>
        ' ''' Restituisce la collezione di tutti gli stati di lavorazione associati alla pratica
        ' ''' </summary>
        ' ''' <value></value>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public ReadOnly Property StatiDiLavorazione As CStatiDiLavorazionePratica
        '    Get
        '        If Me.m_StatiDiLavorazione Is Nothing Then
        '            Me.m_StatiDiLavorazione = New CStatiDiLavorazionePratica
        '            Me.m_StatiDiLavorazione.Load(Me)
        '        End If
        '        Return Me.m_StatiDiLavorazione
        '    End Get
        'End Property

        Public Property Telefono As String
            Get
                Return Me.m_Telefono
            End Get
            Set(value As String)
                value = Formats.ParsePhoneNumber(value)
                Dim oldValue As String = Me.m_Telefono
                If (oldValue = value) Then Exit Property
                Me.m_Telefono = value
                Me.DoChanged("Telefono", value, oldValue)
            End Set
        End Property

        Public Property Cellulare As String
            Get
                Return Me.m_Cellulare
            End Get
            Set(value As String)
                value = Formats.ParsePhoneNumber(value)
                Dim oldValue As String = Me.m_Cellulare
                If (oldValue = value) Then Exit Property
                Me.m_Cellulare = value
                Me.DoChanged("Cellulare", value, oldValue)
            End Set
        End Property

        Public Property Fax As String
            Get
                Return Me.m_Fax
            End Get
            Set(value As String)
                value = Formats.ParsePhoneNumber(value)
                Dim oldValue As String = Me.m_Fax
                If (oldValue = value) Then Exit Property
                Me.m_Fax = value
                Me.DoChanged("Fax", value, oldValue)
            End Set
        End Property

        Public Property eMail As String
            Get
                Return Me.m_eMail
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_eMail
                If (oldValue = value) Then Exit Property
                Me.m_eMail = value
                Me.DoChanged("eMail", value, oldValue)
            End Set
        End Property

        'Public Property WebSite As String
        '    Get
        '        Return Me.m_WebSite
        '    End Get
        '    Set(value As String)
        '        value = Trim(value)
        '        Dim oldValue As String = Me.m_WebSite
        '        If (oldValue = value) Then Exit Property
        '        Me.m_WebSite = value
        '        Me.DoChanged("WebSite", value, oldValue)
        '    End Set
        'End Property

        ' ''' <summary>
        ' ''' Restituisce o imposta l'ID dell'azienda erogante associata alla pratica
        ' ''' </summary>
        ' ''' <value></value>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Property IDAmministrazione As Integer
        '    Get
        '        Return GetID(Me.m_Amministrazione, Me.m_IDAmministrazione)
        '    End Get
        '    Set(value As Integer)
        '        Dim oldValue As Integer = Me.IDAmministrazione
        '        If oldValue = value Then Exit Property
        '        Me.m_Amministrazione = Nothing
        '        Me.m_IDAmministrazione = value
        '        Me.DoChanged("IDAmministrazione", value, oldValue)
        '    End Set
        'End Property

        'Public Property Amministrazione As CAzienda
        '    Get
        '        If Me.m_Amministrazione Is Nothing Then Me.m_Amministrazione = Anagrafica.Aziende.GetItemById(Me.m_IDAmministrazione)
        '        Return Me.m_Amministrazione
        '    End Get
        '    Set(value As CAzienda)
        '        Dim oldValue As CAzienda = Me.Amministrazione
        '        If (oldValue = value) Then Exit Property
        '        Me.m_Amministrazione = value
        '        Me.m_IDAmministrazione = GetID(value)
        '        If (value IsNot Nothing) Then Me.m_NomeAmministrazione = value.Nominativo
        '        Me.DoChanged("Amministrazione", value, oldValue)
        '    End Set
        'End Property

        'Public Property NomeAmministrazione As String
        '    Get
        '        Return Me.m_NomeAmministrazione
        '    End Get
        '    Set(value As String)
        '        value = Trim(value)
        '        Dim oldValue As String = Me.m_NomeAmministrazione
        '        If (oldValue = value) Then Exit Property
        '        Me.m_NomeAmministrazione = value
        '        Me.DoChanged("NomeAmministrazione", value, oldValue)
        '    End Set
        'End Property

        'Public Property IDEntePagante As Integer
        '    Get
        '        Return GetID(Me.m_EntePagante, Me.m_IDEntePagante)
        '    End Get
        '    Set(value As Integer)
        '        Dim oldValue As Integer = Me.IDEntePagante
        '        If oldValue = value Then Exit Property
        '        Me.m_IDEntePagante = value
        '        Me.m_EntePagante = Nothing
        '        Me.DoChanged("IDEntePagante", value, oldValue)
        '    End Set
        'End Property

        'Public Property EntePagante As CAzienda
        '    Get
        '        If Me.m_EntePagante Is Nothing Then Me.m_EntePagante = Anagrafica.Aziende.GetItemById(Me.m_IDEntePagante)
        '        Return Me.m_EntePagante
        '    End Get
        '    Set(value As CAzienda)
        '        Dim oldValue As CAzienda = Me.EntePagante
        '        If (oldValue = value) Then Exit Property
        '        Me.m_EntePagante = value
        '        Me.m_IDEntePagante = GetID(value)
        '        'If (value IsNot Nothing) Then Me.m_NomeEntePagante = value.Nominativo
        '        Me.DoChanged("EntePagante", value, oldValue)
        '    End Set
        'End Property

        ''Public Property NomeEntePagante As String
        ''    Get
        ''        Return Me.m_NomeEntePagante
        ''    End Get
        ''    Set(value As String)
        ''        value = Trim(value)
        ''        Dim oldValue As String = Me.m_NomeEntePagante
        ''        If (oldValue = value) Then Exit Property
        ''        Me.m_NomeEntePagante = value
        ''        Me.DoChanged("NomeEntePagante", value, oldValue)
        ''    End Set
        ''End Property


        Public ReadOnly Property GiorniDiLavorazione As Nullable(Of Integer)
            Get
                If (Me.StatoLiquidata IsNot Nothing AndAlso Me.StatoRichiestaDelibera IsNot Nothing) Then
                    If (Me.StatoRichiestaDelibera.Data.HasValue AndAlso Me.StatoLiquidata.Data.HasValue) Then
                        Return DateDiff(DateInterval.Day, Me.StatoRichiestaDelibera.Data.Value, Me.StatoLiquidata.Data.Value)
                    Else
                        Return 0
                    End If
                Else
                    Return Nothing
                End If
            End Get
        End Property

        Public Property NumeroRate As Nullable(Of Integer)
            Get
                Return Me.m_NumeroRate
            End Get
            Set(value As Nullable(Of Integer))
                Dim oldValue As Nullable(Of Integer) = Me.m_NumeroRate
                If (oldValue = value) Then Exit Property
                Me.m_NumeroRate = value
                Me.DoChanged("NumeroRate", value, oldValue)
            End Set
        End Property

        Public Property Rata As Nullable(Of Decimal)
            Get
                If Me.NumeroRate > 0 Then Return Me.MontanteLordo / Me.NumeroRate
                Return Me.MontanteLordo
            End Get
            Set(value As Nullable(Of Decimal))
                Dim oldValue As Nullable(Of Decimal) = Me.Rata
                If (oldValue = value) Then Exit Property
                If Me.NumeroRate > 0 Then
                    Me.MontanteLordo = value * Me.NumeroRate
                Else
                    Me.MontanteLordo = value
                End If
                Me.DoChanged("Rata", value, oldValue)
            End Set
        End Property


        Public Property IDCessionario As Integer
            Get
                Return GetID(Me.m_Cessionario, Me.m_CessionarioID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDCessionario
                If oldValue = value Then Exit Property
                Me.m_CessionarioID = value
                Me.m_Cessionario = Nothing
                Me.DoChanged("IDCessionario", value, oldValue)
            End Set
        End Property

        Public Property Cessionario As CCQSPDCessionarioClass
            Get
                If (Me.m_Cessionario Is Nothing) Then Me.m_Cessionario = DMD.CQSPD.Cessionari.GetItemById(Me.m_CessionarioID)
                Return Me.m_Cessionario
            End Get
            Set(value As CCQSPDCessionarioClass)
                Dim oldValue As CCQSPDCessionarioClass = Me.Cessionario
                If (oldValue = value) Then Exit Property
                Me.m_Cessionario = value
                Me.m_CessionarioID = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeCessionario = value.Nome
                Me.DoChanged("Cessionario", value, oldValue)
            End Set
        End Property

        Public Property NomeCessionario As String
            Get
                Return Me.m_NomeCessionario
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomeCessionario
                If (oldValue = value) Then Exit Property
                Me.m_NomeCessionario = value
                Me.DoChanged("NomeCessionario", value, oldValue)
            End Set
        End Property

        Public Property IDProfilo As Integer
            Get
                Return GetID(Me.m_Profilo, Me.m_ProfiloID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDProfilo
                If oldValue = value Then Exit Property
                Me.m_ProfiloID = value
                Me.m_Profilo = Nothing
                Me.DoChanged("IDProfilo", value, oldValue)
            End Set
        End Property

        Public Property Profilo As CProfilo
            Get
                If (Me.m_Profilo Is Nothing) Then Me.m_Profilo = DMD.CQSPD.Profili.GetItemById(Me.m_ProfiloID)
                Return Me.m_Profilo
            End Get
            Set(value As CProfilo)
                Dim oldValue As CProfilo = Me.Profilo
                If (oldValue = value) Then Exit Property
                Me.m_Profilo = value
                Me.m_ProfiloID = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeProfilo = value.ProfiloVisibile
                Me.DoChanged("Profilo", value, oldValue)
            End Set
        End Property

        Public Property NomeProfilo As String
            Get
                Return Me.m_NomeProfilo
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomeProfilo
                If (oldValue = value) Then Exit Property
                Me.m_NomeProfilo = value
                Me.DoChanged("NomeProfilo", value, oldValue)
            End Set
        End Property

        Public Property Sesso As String
            Get
                Return Me.m_Sesso
            End Get
            Set(value As String)
                value = UCase(Left(Trim(value), 1))
                Dim oldValue As String = Me.m_Sesso
                If (oldValue = value) Then Exit Property
                Me.m_Sesso = value
                Me.DoChanged("Sesso", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la percentuale (nascosta) del montante lordo pagata all'azienda
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Rappel As Nullable(Of Double)
            Get
                If (Me.m_ValoreRappel.HasValue AndAlso Me.m_MontanteLordo.HasValue AndAlso Me.m_MontanteLordo.Value > 0) Then
                    Return (Me.m_ValoreRappel / Me.m_MontanteLordo) * 100
                Else
                    Return Nothing
                End If
            End Get
            Set(value As Nullable(Of Double))
                If (value < 0 Or value > 100) Then Throw New ArgumentOutOfRangeException("Rappel")
                Dim oldValue As Nullable(Of Double) = Me.Rappel
                If (oldValue = value) Then Exit Property
                Me.m_ValoreRappel = value * Me.m_MontanteLordo / 100
                Me.DoChanged("Rappel", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la provvigione (nascosta) pagata all'azienda
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ValoreRappel As Nullable(Of Decimal)
            Get
                Return Me.m_ValoreRappel
            End Get
            Set(value As Nullable(Of Decimal))
                If (value < 0) Then Throw New ArgumentOutOfRangeException("ValoreRappel")
                Dim oldValue As Nullable(Of Double) = Me.m_ValoreRappel
                If (oldValue = value) Then Exit Property
                Me.m_ValoreRappel = value
                Me.DoChanged("ValoreRappel", value, oldValue)
            End Set
        End Property

        'Public Property Costo As Nullable(Of Decimal)
        '    Get
        '        Return Me.m_Costo
        '    End Get
        '    Set(value As Nullable(Of Decimal))
        '        Dim oldValue As Nullable(Of Decimal) = Me.m_Costo
        '        If (oldValue = value) Then Exit Property
        '        Me.m_Costo = value
        '        Me.DoChanged("Costo", value, oldValue)
        '    End Set
        'End Property

        Public Property TipoFonteContatto As String
            Get
                Return Me.m_TipoFonteContatto
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_TipoFonteContatto
                If (oldValue = value) Then Exit Property
                Me.m_TipoFonteContatto = value
                Me.m_Fonte = Nothing
                Me.DoChanged("TipoFonteContatto", value, oldValue)
            End Set
        End Property

        Public Property IDFonte As Integer
            Get
                Return GetID(Me.m_Fonte, Me.m_IDFonte)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDFonte
                If oldValue = value Then Exit Property
                Me.m_IDFonte = value
                Me.m_Fonte = Nothing
                Me.DoChanged("IDFonte", value, oldValue)
            End Set
        End Property

        Public Property Fonte As IFonte
            Get
                If (Me.m_Fonte Is Nothing) Then Me.m_Fonte = Anagrafica.Fonti.GetItemById(Me.m_TipoFonteContatto, Me.m_TipoFonteContatto, Me.m_IDFonte)
                Return Me.m_Fonte
            End Get
            Set(value As IFonte)
                Dim oldValue As DBObjectBase = Me.Fonte
                If (oldValue Is value) Then Exit Property
                Me.m_Fonte = value
                Me.m_IDFonte = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeFonte = value.Nome
                Me.DoChanged("Fonte", value, oldValue)
            End Set
        End Property

        Public Property NomeFonte As String
            Get
                Return Me.m_NomeFonte
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomeFonte
                If (oldValue = value) Then Exit Property
                Me.m_NomeFonte = value
                Me.DoChanged("NomeFonte", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il tipo fonte d
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TipoFonteCliente As String
            Get
                Return Me.m_TipoFonteCliente
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_TipoFonteCliente
                If (oldValue = value) Then Exit Property
                Me.m_TipoFonteCliente = value
                Me.m_FonteCliente = Nothing
                Me.DoChanged("TipoFonteCliente", value, oldValue)
            End Set
        End Property

        Public Property IDFonteCliente As Integer
            Get
                Return GetID(Me.m_FonteCliente, Me.m_IDFonteCliente)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDFonteCliente
                If oldValue = value Then Exit Property
                Me.m_IDFonteCliente = value
                Me.m_FonteCliente = Nothing
                Me.DoChanged("IDFonteCliente", value, oldValue)
            End Set
        End Property

        Public Property FonteCliente As IFonte
            Get
                If (Me.m_FonteCliente Is Nothing) Then Me.m_FonteCliente = Anagrafica.Fonti.GetItemById(Me.m_TipoFonteContatto, Me.m_TipoFonteCliente, Me.m_IDFonteCliente)
                Return Me.m_FonteCliente
            End Get
            Set(value As IFonte)
                Dim oldValue As DBObjectBase = Me.FonteCliente
                If (oldValue Is value) Then Exit Property
                Me.m_FonteCliente = value
                Me.m_IDFonteCliente = GetID(value)
                Me.DoChanged("FonteCliente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome registrato per il cliente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeCliente As String
            Get
                Return Me.m_NomeCliente
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomeCliente
                If (oldValue = value) Then Exit Property
                Me.m_NomeCliente = value
                Me.DoChanged("NomeCliente", value, oldValue)
            End Set
        End Property

        Public Property CognomeCliente As String
            Get
                Return Me.m_CognomeCliente
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_CognomeCliente
                If (oldValue = value) Then Exit Property
                Me.m_CognomeCliente = value
                Me.DoChanged("CognomeCliente", value, oldValue)
            End Set
        End Property

        Public ReadOnly Property NominativoCliente As String
            Get
                Return Trim(Strings.ToNameCase(Me.NomeCliente) & " " & UCase(Me.CognomeCliente))
            End Get
        End Property

        Public Property NomeProdotto As String
            Get
                Return Me.m_NomeProdotto
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomeProdotto
                If (oldValue = value) Then Exit Property
                Me.m_NomeProdotto = value
                Me.DoChanged("NomeProdotto", value, oldValue)
            End Set
        End Property

        Public Property Prodotto As CCQSPDProdotto
            Get
                If (Me.m_Prodotto Is Nothing) Then Me.m_Prodotto = DMD.CQSPD.Prodotti.GetItemById(Me.m_ProdottoID)
                Return Me.m_Prodotto
            End Get
            Set(value As CCQSPDProdotto)
                Dim oldValue As CCQSPDProdotto = Me.Prodotto
                If (oldValue = value) Then Exit Property
                Me.m_Prodotto = value
                Me.m_ProdottoID = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeProdotto = value.Nome
                Me.DoChanged("Prodotto", value, oldValue)
            End Set
        End Property

        Public Property IDProdotto As Integer
            Get
                Return GetID(Me.m_Prodotto, Me.m_ProdottoID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDProdotto
                If oldValue = value Then Exit Property
                Me.m_ProdottoID = value
                Me.m_Prodotto = Nothing
                Me.DoChanged("IDProdotto", value, oldValue)
            End Set
        End Property

        Public ReadOnly Property NatoA As CIndirizzo
            Get
                Return Me.m_NatoA
            End Get
        End Property

        Public Property NatoIl As Nullable(Of Date)
            Get
                Return Me.m_NatoIl
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_NatoIl
                If (oldValue = value) Then Exit Property
                Me.m_NatoIl = value
                Me.DoChanged("NatoIl", value, oldValue)
            End Set
        End Property

        Public ReadOnly Property ResidenteA As CIndirizzo
            Get
                Return Me.m_ResidenteA
            End Get
        End Property

        Public Property CodiceFiscale As String
            Get
                Return Me.m_CodiceFiscale
            End Get
            Set(value As String)
                value = Formats.ParseCodiceFiscale(value)
                Dim oldValue As String = Me.m_CodiceFiscale
                If (oldValue = value) Then Exit Property
                Me.m_CodiceFiscale = value
                Me.DoChanged("CodiceFiscale", value, oldValue)
            End Set
        End Property

        Public Property PartitaIVA As String
            Get
                Return Me.m_PartitaIVA
            End Get
            Set(value As String)
                value = Formats.ParsePartitaIVA(value)
                Dim oldValue As String = Me.m_PartitaIVA
                If (oldValue = value) Then Exit Property
                Me.m_PartitaIVA = value
                Me.DoChanged("PartitaIVA", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce l'ID della persona fisica che ha stipulato la pratica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDCliente As Integer
            Get
                Return GetID(Me.m_Cliente, Me.m_ClienteID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDCliente
                If oldValue = value Then Exit Property
                Me.m_ClienteID = value
                Me.m_Cliente = Nothing
                Me.DoChanged("IDCliente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce la persona fisica che ha stipulato la pratica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Cliente As CPersonaFisica Implements IEstintore.Cliente
            Get
                If (Me.m_Cliente Is Nothing) Then Me.m_Cliente = Anagrafica.Persone.GetPersonaById(Me.m_ClienteID)
                If (Me.m_Cliente Is Nothing) Then
                    Dim items As CCollection(Of CPersona) = Anagrafica.Persone.FindPersoneByCF(Me.CodiceFiscale)
                    For Each p As CPersona In items
                        If (TypeOf (p) Is CPersonaFisica) Then
                            Me.m_Cliente = p
                            Exit For
                        End If
                    Next
                End If
                If (Me.m_Cliente Is Nothing) Then
                    Me.m_Cliente = New CPersonaFisica
                    Me.AggiornaAnagraficaCliente(Me.m_Cliente)
                    Me.m_Cliente.Stato = Me.Stato
                End If
                Return Me.m_Cliente
            End Get
            Set(value As CPersonaFisica)
                Dim oldValue As CPersonaFisica = Me.Cliente
                If (oldValue = value) Then Exit Property
                Me.m_Cliente = value
                Me.m_ClienteID = GetID(value)
                If (value IsNot Nothing) AndAlso (Me.m_CognomeCliente = "") Then
                    Me.FromCliente()
                End If
                Me.DoChanged("Cliente", value, oldValue)
            End Set
        End Property

        Protected Friend Overridable Sub SetCliente(ByVal value As CPersonaFisica)
            Me.m_Cliente = value
            Me.m_ClienteID = GetID(value)
        End Sub

        ''' <summary>
        ''' Aggionra i dati relativi all'anagrafica ed all'impiego sulla base dei valori attuali del cliente
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub FromCliente()
            Me.NomeCliente = Me.Cliente.Nome
            Me.CognomeCliente = Me.Cliente.Cognome
            Me.Sesso = Me.Cliente.Sesso
            Me.NatoIl = Me.Cliente.DataNascita
            Me.CodiceFiscale = Me.Cliente.CodiceFiscale
            Me.PartitaIVA = Me.Cliente.PartitaIVA
            Me.NatoA.Citta = Me.Cliente.NatoA.Citta
            Me.NatoA.Provincia = Me.Cliente.NatoA.Provincia
            Me.ResidenteA.Citta = Me.Cliente.ResidenteA.Citta
            Me.ResidenteA.Provincia = Me.Cliente.ResidenteA.Provincia
            Me.ResidenteA.CAP = Me.Cliente.ResidenteA.CAP
            Me.ResidenteA.ToponimoViaECivico = Me.Cliente.ResidenteA.ToponimoViaECivico
            Me.Telefono = "" 'Me.Cliente.TelefonoPrincipale.Valore
            Me.TipoFonteCliente = Me.Cliente.TipoFonte
            Me.FonteCliente = Me.Cliente.Fonte
            Me.Cellulare = ""
            Me.Fax = ""
            'Me.m_Mail = Me.Cliente.eMail(0).Valore
            Dim impiego As CImpiegato = Me.Cliente.ImpiegoPrincipale
            If (impiego Is Nothing AndAlso Me.Cliente.ImpieghiValidi.Count() > 0) Then impiego = Me.Cliente.ImpieghiValidi(0)
            If (impiego IsNot Nothing) Then
                Me.Impiego.InitializeFrom(impiego)
                ''Me.Amministrazione = impiego.Azienda
                ''Me.EntePagante = impiego.EntePagante
                'Me.DataAssunzione = impiego.DataAssunzione
                ''Me.DataLicenziamento = impiego.DataLicenziamento
                'Me.Posizione = impiego.Posizione
                'Me.StipendioLordo = impiego.StipendioLordo
                'Me.StipendioNetto = impiego.StipendioNetto
                'Me.TFR = impiego.TFR
                'Me.PercTFRAzienda = impiego.PercTFRAzienda
                'Me.TFRNomeFondo = impiego.NomeFPC
                'Me.TipoRapporto = impiego.TipoRapporto
                'Me.NumeroMensilita = impiego.MensilitaPercepite
            End If
        End Sub

        'Public ReadOnly Property Appuntamenti As CAppuntamentiCollection
        '    Get
        '        If (Me.m_Appuntamenti Is Nothing) Then
        '            Me.m_Appuntamenti = New CAppuntamentiCollection
        '            Me.m_Appuntamenti.Initialize(Me)
        '        End If
        '        Return Me.m_Appuntamenti
        '    End Get
        'End Property

        'Public ReadOnly Property Comunicazioni As CComunicazioniPersonaCollection
        '    Get
        '        If (Me.m_Comunicazioni Is Nothing) Then
        '            Me.m_Comunicazioni = New CComunicazioniPersonaCollection
        '            Me.m_Comunicazioni.Initialize(Me)
        '        End If
        '        Return Me.m_Comunicazioni
        '    End Get
        'End Property

        ''' <summary>
        ''' Restituisce un oggetto che descrive lo stato Contatto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property StatoContatto As CStatoLavorazionePratica
            Get
                Return Me.StatiDiLavorazione.GetItemByMacroStato(StatoPraticaEnum.STATO_CONTATTO) ' Me.m_StatoContatto
            End Get
        End Property

        ''' <summary>
        ''' Restituisce un oggetto che descrive lo stato Richiesta Delibera
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property StatoRichiestaDelibera As CStatoLavorazionePratica
            Get
                Return Me.StatiDiLavorazione.GetItemByMacroStato(StatoPraticaEnum.STATO_RICHIESTADELIBERA) 'Me.m_StatoRichDelibera
            End Get
        End Property

        ''' <summary>
        ''' Restituisce un oggetto che descrive lo stato Pronta per Liquidazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property StatoProntaLiquidazione As CStatoLavorazionePratica
            Get
                Return Me.StatiDiLavorazione.GetItemByMacroStato(StatoPraticaEnum.STATO_PRONTALIQUIDAZIONE) 'Me.m_StatoProntaLiquidazione
            End Get
        End Property

        ''' <summary>
        ''' Restituisce un oggetto che descrive lo stato pratica deliberata
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property StatoDeliberata As CStatoLavorazionePratica
            Get
                Return Me.StatiDiLavorazione.GetItemByMacroStato(StatoPraticaEnum.STATO_DELIBERATA) 'Me.m_StatoDeliberata
            End Get
        End Property

        ''' <summary>
        ''' Restituisce un oggetto che descrive lo stato pratica liquidata
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property StatoLiquidata As CStatoLavorazionePratica
            Get
                Return Me.StatiDiLavorazione.GetItemByMacroStato(StatoPraticaEnum.STATO_LIQUIDATA) 'Return Me.m_StatoLiquidata
            End Get
        End Property

        ''' <summary>
        ''' Restituisce un oggetto che descrive lo stato pratica Archiviata
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property StatoArchiviata As CStatoLavorazionePratica
            Get
                Return Me.StatiDiLavorazione.GetItemByMacroStato(StatoPraticaEnum.STATO_ARCHIVIATA) 'Return Me.m_StatoArchiviata
            End Get
        End Property

        ''' <summary>
        ''' Restituisce un oggetto che descrive lo stato pratica Annullata
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property StatoAnnullata As CStatoLavorazionePratica
            Get
                Return Me.StatiDiLavorazione.GetItemByMacroStato(StatoPraticaEnum.STATO_ANNULLATA) 'Return Me.m_StatoAnnullata
            End Get
        End Property

        ''' <summary>
        ''' Restituisce un oggetto che rappresenta lo stato attuale della pratica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property StatoAttuale As CStatoPratica
            Get
                'If (Me.m_StatoAttuale Is Nothing) Then Me.m_StatoAttuale = Me.StatoDiLavorazioneAttuale.StatoPratica
                'If (Me.m_StatoAttuale Is Nothing) Then Me.m_StatoAttuale = DMD.CQSPD.StatiPratica.GetItemById(Me.m_IDStatoAttuale)
                'Return Me.m_StatoAttuale
                If (Me.StatoDiLavorazioneAttuale Is Nothing) Then Return Nothing
                Return Me.StatoDiLavorazioneAttuale.StatoPratica
            End Get
            'Set(value As CStatoPratica)
            '    Dim oldValue As CStatoPratica = Me.m_StatoAttuale
            '    If (oldValue Is value) Then Exit Property
            '    Me.m_StatoAttuale = value
            '    Me.m_IDStatoAttuale = GetID(value)
            '    'Me.m_MacroStato = value.MacroStato
            '    'If (value.MacroStato.HasValue) Then Me.mStatoPratica = value.MacroStato
            '    Me.DoChanged("StatoAttuale", value, oldValue)
            'End Set
        End Property

        Public ReadOnly Property IDStatoAttuale As Integer
            Get
                Return GetID(Me.StatoAttuale) ' Me.StatoDiLavorazioneAttuale.IDStatoPratica
            End Get
        End Property

        ''' <summary>
        ''' Restituisce un oggetto che rappresenta lo stato attuale della pratica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property StatoDiLavorazioneAttuale As CStatoLavorazionePratica
            Get
                Return Me.m_StatoDiLavorazioneAttuale
            End Get
            'Set(value As CStatoLavorazionePratica)
            '    Dim oldValue As CStatoLavorazionePratica = Me.m_StatoDiLavorazioneAttuale
            '    If (oldValue Is value) Then Exit Property
            '    Me.m_StatoDiLavorazioneAttuale = value
            '    Me.m_IDStatoDiLavorazioneAttuale = GetID(value)
            '    Me.StatoAttuale = value.StatoPratica
            '    'If (value.MacroStato.HasValue) Then Me.StatoPratica = value.MacroStato
            '    Me.DoChanged("StatoAttuale", value, oldValue)
            'End Set
        End Property

        Protected Friend Overridable Sub SetStatoDiLavorazioneAttuale(ByVal value As CStatoLavorazionePratica)
            Me.m_StatoDiLavorazioneAttuale = value
            ' Me.m_IDStatoDiLavorazioneAttuale = GetID(value)
        End Sub

        Public ReadOnly Property IDStatoDiLavorazioneAttuale As Integer
            Get
                Return GetID(Me.m_StatoDiLavorazioneAttuale)
            End Get
            'Friend Set(value As Integer)
            '    Dim oldValue As Integer = Me.IDStatoDiLavorazioneAttuale
            '    If (oldValue = value) Then Exit Property
            '    Me.m_IDStatoDiLavorazioneAttuale = value
            '    Me.m_StatoDiLavorazioneAttuale = Nothing
            '    Me.DoChanged("IDStatoDiLavorazioneAttuale", value, oldValue)
            'End Set
        End Property

        ' ''' <summary>
        ' ''' Restituisce o imposta un valore che indica lo stato attuale della pratica
        ' ''' </summary>
        ' ''' <value></value>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Property StatoPratica As StatoPraticaEnum
        '    Get
        '        Return Me.m_StatoPratica
        '    End Get
        '    Set(value As StatoPraticaEnum)
        '        Dim oldValue As StatoPraticaEnum = Me.m_StatoPratica
        '        If (oldValue = value) Then Exit Property
        '        Me.m_StatoPratica = value
        '        Me.DoChanged("StatoPratica", value, oldValue)
        '    End Set
        'End Property

        Public ReadOnly Property StatiPratica(ByVal stato As StatoPraticaEnum) As CStatoLavorazionePratica
            Get
                Select Case stato
                    Case StatoPraticaEnum.STATO_CONTATTO : Return Me.StatoContatto
                    Case StatoPraticaEnum.STATO_RICHIESTADELIBERA : Return Me.StatoRichiestaDelibera
                    Case StatoPraticaEnum.STATO_DELIBERATA : Return Me.StatoDeliberata
                    Case StatoPraticaEnum.STATO_PRONTALIQUIDAZIONE : Return Me.StatoProntaLiquidazione
                    Case StatoPraticaEnum.STATO_LIQUIDATA : Return Me.StatoLiquidata
                    Case StatoPraticaEnum.STATO_ARCHIVIATA : Return Me.StatoArchiviata
                    Case StatoPraticaEnum.STATO_ANNULLATA : Return Me.StatoAnnullata
                    Case Else : Throw New NotSupportedException
                End Select
            End Get
        End Property

        Public ReadOnly Property StatiDiLavorazione As CStatiLavorazionePraticaCollection
            Get
                SyncLock Me
                    If (Me.m_StatiDiLavorazione Is Nothing) Then
                        Me.m_StatiDiLavorazione = New CStatiLavorazionePraticaCollection(Me)
                        Dim j As Integer = Me.m_StatiDiLavorazione.IndexOf(Me.m_StatiDiLavorazione.GetItemById(GetID(Me.m_StatoDiLavorazioneAttuale)))
                        If (j >= 0) Then
                            Me.m_StatiDiLavorazione(j) = Me.m_StatoDiLavorazioneAttuale
                        Else
                            Me.m_StatiDiLavorazione.Add(Me.m_StatoDiLavorazioneAttuale)
                            Me.m_StatiDiLavorazione.Sort()
                        End If
                    End If
                    Return Me.m_StatiDiLavorazione
                End SyncLock
            End Get
        End Property

        Protected Friend Overridable Sub SetStatiDiLavorazione(ByVal value As CStatiLavorazionePraticaCollection)
            Me.m_StatiDiLavorazione = value
        End Sub

        Public Property MontanteLordo As Nullable(Of Decimal)
            Get
                Return Me.m_MontanteLordo
            End Get
            Set(value As Nullable(Of Decimal))
                If (value < 0) Then Throw New ArgumentOutOfRangeException("Il Montante Lordo deve essere non negativo")
                Dim oldValue As Nullable(Of Decimal) = Me.m_MontanteLordo
                If (oldValue = value) Then Exit Property
                Me.m_MontanteLordo = value
                Me.DoChanged("MontanteLordo", value, oldValue)
            End Set
        End Property


        ' ''' <summary>
        ' ''' Restituisce o imposta la percentuale del montante lordo pagata al collaboratore
        ' ''' </summary>
        ' ''' <value></value>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Property ProvvigioniBroker As Nullable(Of Double)
        '    Get
        '        If (Me.m_ValoreProvvBroker.HasValue AndAlso Me.m_MontanteLordo.HasValue AndAlso Me.m_MontanteLordo.Value > 0) Then
        '            Return (Me.m_ValoreProvvBroker / Me.m_MontanteLordo) * 100
        '        Else
        '            Return Nothing
        '        End If
        '    End Get
        '    Set(value As Nullable(Of Double))
        '        If (value < 0 Or value > 100) Then Throw New ArgumentOutOfRangeException("ProvvigioniBroker")
        '        Dim oldValue As Nullable(Of Double) = Me.ProvvigioniBroker
        '        If (oldValue = value) Then Exit Property
        '        Me.m_ValoreProvvBroker = value * Me.m_MontanteLordo / 100
        '        Me.DoChanged("ProvvigioniBroker", value, oldValue)
        '    End Set
        'End Property

        ' ''' <summary>
        ' ''' Restituisce o imposta il valore delle provvigioni pagate al collaboratore
        ' ''' </summary>
        ' ''' <value></value>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Property ValoreProvvigioniBroker As Nullable(Of Decimal)
        '    Get
        '        Return Me.m_ValoreProvvBroker
        '    End Get
        '    Set(value As Nullable(Of Decimal))
        '        If (value < 0) Then Throw New ArgumentOutOfRangeException("ValoreProvvigioniBroker")
        '        Dim oldValue As Nullable(Of Decimal) = Me.m_ValoreProvvBroker
        '        If (oldValue = value) Then Exit Property
        '        Me.m_ValoreProvvBroker = value
        '        Me.DoChanged("ValoreProvvigioniBroker", value, oldValue)
        '    End Set
        'End Property

        ''' <summary>
        ''' Restituisce o imposta la percentuale massima della provvigione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ProvvigioneMassima As Nullable(Of Double)
            Get
                Dim baseML As Decimal? = Me.CalcolaBaseML
                'If (Me.m_ValoreProvvMax.HasValue AndAlso Me.m_MontanteLordo.HasValue AndAlso Me.m_MontanteLordo.Value > 0) Then
                If (Me.m_ValoreProvvMax.HasValue AndAlso baseML.HasValue AndAlso baseML.Value > 0) Then
                    Return Me.m_ValoreProvvMax.Value * 100 / baseML.Value
                Else
                    Return Nothing
                End If
            End Get
            Set(value As Nullable(Of Double))
                If (value < 0 Or value > 100) Then Throw New ArgumentOutOfRangeException("ProvvigioneMassima")
                Dim oldValue As Nullable(Of Double) = Me.ProvvigioneMassima
                If (oldValue = value) Then Exit Property
                Me.ValoreProvvigioneMassima = value * Me.m_MontanteLordo / 100
                Me.DoChanged("ProvvigioneMassima", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la provvigione incassata dall'azienda
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ValoreProvvigioneMassima As Nullable(Of Decimal)
            Get
                Return Me.m_ValoreProvvMax
            End Get
            Set(value As Nullable(Of Decimal))
                If (value < 0) Then Throw New ArgumentOutOfRangeException("ValoreProvvigioneMassima")
                Dim oldValue As Nullable(Of Double) = Me.m_ValoreProvvMax
                If (oldValue = value) Then Exit Property
                Me.m_ValoreProvvMax = value
                Me.DoChanged("ValoreProvvigioneMassima", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la percentuale del montante lordo incassata dall'azienda
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Spread As Nullable(Of Double)
            Get
                Dim baseML As Decimal? = Me.CalcolaBaseML
                'If (Me.m_MontanteLordo.HasValue AndAlso Me.m_MontanteLordo.Value > 0) Then
                If (Me.ValoreSpread.HasValue AndAlso baseML.HasValue AndAlso baseML.Value > 0) Then
                    Return Me.ValoreSpread.Value * 100 / baseML.Value
                Else
                    Return Nothing
                End If
            End Get
            Set(value As Nullable(Of Double))
                If (value < 0 Or value > 100) Then Throw New ArgumentOutOfRangeException("Spread")
                Dim oldValue As Nullable(Of Double) = Me.Spread
                If (oldValue = value) Then Exit Property
                Me.ValoreSpread = value * Me.m_MontanteLordo / 100
                Me.DoChanged("Spread", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la provvigione incassata dall'azienda
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ValoreSpread As Nullable(Of Decimal)
            Get
                If (Me.m_ValoreUpFront.HasValue AndAlso Me.m_ValoreRunning.HasValue) Then
                    Return Me.m_ValoreUpFront.Value + Me.m_ValoreRunning.Value
                Else
                    Return Nothing
                End If
            End Get
            Set(value As Nullable(Of Decimal))
                If (value < 0) Then Throw New ArgumentOutOfRangeException("ValoreSpread")
                Dim oldValue As Nullable(Of Double) = Me.ValoreSpread
                If (oldValue = value) Then Exit Property
                Me.m_ValoreUpFront = value
                Me.m_ValoreRunning = 0
                Me.DoChanged("ValoreSpread", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la percentuale del montante lordo pagato alla filiale
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property UpFront As Nullable(Of Double)
            Get
                Dim baseML As Decimal? = Me.CalcolaBaseML
                'If (Me.m_ValoreUpFront.HasValue AndAlso Me.m_MontanteLordo.HasValue AndAlso Me.m_MontanteLordo > 0) Then
                If (Me.m_ValoreUpFront.HasValue AndAlso baseML.HasValue AndAlso baseML.Value > 0) Then
                    Return Me.m_ValoreUpFront * 100 / baseML.Value
                Else
                    Return Nothing
                End If
            End Get
            Set(value As Nullable(Of Double))
                If (value < 0 Or value > 100) Then Throw New ArgumentOutOfRangeException("UpFront")
                Dim oldValue As Nullable(Of Double) = Me.UpFront
                If (oldValue = value) Then Exit Property
                Me.m_ValoreUpFront = value * Me.m_MontanteLordo / 100
                Me.DoChanged("UpFront", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ammontare della provvigione pagata alla filiale
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ValoreUpFront As Nullable(Of Decimal)
            Get
                Return Me.m_ValoreUpFront
            End Get
            Set(value As Nullable(Of Decimal))
                If (value < 0) Then Throw New ArgumentOutOfRangeException("ValoreUpFront")
                Dim oldValue As Nullable(Of Double) = Me.m_ValoreUpFront
                If (oldValue = value) Then Exit Property
                Me.m_ValoreUpFront = value
                Me.DoChanged("ValoreUpFront", value, oldValue)
            End Set
        End Property

        Public Property Running As Nullable(Of Double)
            Get
                Dim baseML As Decimal? = Me.CalcolaBaseML
                'If (Me.m_ValoreRunning.HasValue AndAlso Me.m_MontanteLordo.HasValue AndAlso Me.m_MontanteLordo > 0) Then
                If (Me.m_ValoreRunning.HasValue AndAlso baseML.HasValue AndAlso baseML.Value > 0) Then
                    Return Me.m_ValoreRunning * 100 / baseML.Value
                Else
                    Return Nothing
                End If
            End Get
            Set(value As Nullable(Of Double))
                If (value < 0 Or value > 100) Then Throw New ArgumentOutOfRangeException("Running")
                Dim oldValue As Nullable(Of Double) = Me.UpFront
                If (oldValue = value) Then Exit Property
                Me.m_ValoreRunning = value * Me.m_MontanteLordo / 100
                Me.DoChanged("Running", value, oldValue)
            End Set
        End Property


        Public Property ValoreRunning As Nullable(Of Decimal)
            Get
                Return Me.m_ValoreRunning
            End Get
            Set(value As Nullable(Of Decimal))
                If (value < 0) Then Throw New ArgumentOutOfRangeException("ValoreRunning")
                Dim oldValue As Nullable(Of Double) = Me.m_ValoreRunning
                If (oldValue = value) Then Exit Property
                Me.m_ValoreRunning = value
                Me.DoChanged("ValoreRunning", value, oldValue)
            End Set
        End Property

        'Public ReadOnly Property ProvvigioneTotale As Nullable(Of Double)
        '    Get
        '        If (Me.Provvigionale.Totale.HasValue AndAlso Me.Spread.HasValue) Then
        '            Return Me.Provvigionale.Totale + Me.Spread
        '        Else
        '            Return Nothing
        '        End If
        '    End Get
        'End Property

        Public Property NettoRicavo As Nullable(Of Decimal)
            Get
                Return Me.m_NettoRicavo
            End Get
            Set(value As Nullable(Of Decimal))
                Dim oldValue As Nullable(Of Decimal) = Me.m_NettoRicavo
                If (oldValue = value) Then Exit Property
                Me.m_NettoRicavo = value
                Me.DoChanged("NettoRicavo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce la differenza tra il netto ricavo e la somma degli impegni da estinguere
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property NettoAllaMano As Decimal?
            Get
                Dim ret As Decimal? = Me.NettoRicavo
                If (ret.HasValue = False) Then Return Nothing
                Dim tot As Decimal? = Me.TotaleDaEstinguere
                If (tot.HasValue = False) Then Return Nothing
                Return ret.Value - tot.Value
            End Get
        End Property

        Public ReadOnly Property TotaleDaEstinguere As Decimal?
            Get
                Dim ret As Decimal? = 0.0
                For Each est As EstinzioneXEstintore In Me.Estinzioni
                    If est.Selezionata AndAlso est.Stato = ObjectStatus.OBJECT_VALID Then
                        Dim val As Decimal? = est.TotaleDaRimborsare
                        If (val.HasValue = False) Then Return Nothing
                        ret = ret.Value + val.Value
                    End If
                Next
                Return ret
            End Get
        End Property

        Public ReadOnly Property Anzianita(ByVal al As Date) As Nullable(Of Single)
            Get
                Return DateUtils.CalcolaEta(Me.Impiego.DataAssunzione, al)
            End Get
        End Property

        Public ReadOnly Property Anzianita As Nullable(Of Single)
            Get
                Return Me.Anzianita(Now)
            End Get
        End Property

        Public ReadOnly Property Eta(ByVal al As Date) As Nullable(Of Single)
            Get
                Return DateUtils.CalcolaEta(Me.m_NatoIl, al)
            End Get
        End Property

        Public ReadOnly Property Eta As Nullable(Of Single)
            Get
                Return Me.Eta(Now)
            End Get
        End Property

        Public ReadOnly Property EtaFineFinanziamento() As Nullable(Of Single)
            Get
                Dim d As Nullable(Of Date) = Nothing
                If Me.m_DataDecorrenza.HasValue And Me.NumeroRate.HasValue Then
                    d = DateUtils.DateAdd(Microsoft.VisualBasic.DateInterval.Month, Me.NumeroRate.Value, Me.m_DataDecorrenza)
                    Return Me.Eta(d)
                Else
                    Return Nothing
                End If
            End Get
        End Property

        ''' <summary>
        ''' Restituisce le informazioni sull'impiego al momento della crezione della pratica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Impiego As CImpiegato
            Get
                Return Me.m_Impiego
            End Get
        End Property

        'Public Property Posizione As String
        '    Get
        '        Return Me.m_Posizione
        '    End Get
        '    Set(value As String)
        '        value = Trim(value)
        '        Dim oldValue As String = Me.m_Posizione
        '        If (oldValue = value) Then Exit Property
        '        Me.m_Posizione = value
        '        Me.DoChanged("Posizione", value, oldValue)
        '    End Set
        'End Property

        'Public Property StipendioLordo As Nullable(Of Decimal)
        '    Get
        '        Return Me.m_StipendioLordo
        '    End Get
        '    Set(value As Nullable(Of Decimal))
        '        Dim oldValue As Nullable(Of Decimal) = Me.m_StipendioLordo
        '        If (oldValue = value) Then Exit Property
        '        Me.m_StipendioLordo = value
        '        Me.DoChanged("StipendioLordo", value, oldValue)
        '    End Set
        'End Property

        Public ReadOnly Property Quinto As Nullable(Of Decimal)
            Get
                If Me.Impiego.StipendioNetto.HasValue Then Return Me.Impiego.StipendioNetto.Value / 5
                Return Nothing
            End Get
        End Property


        'Public Property StipendioNetto As Nullable(Of Decimal)
        '    Get
        '        Return Me.m_StipendioNetto
        '    End Get
        '    Set(value As Nullable(Of Decimal))
        '        Dim oldValue As Nullable(Of Decimal) = Me.m_StipendioNetto
        '        If (oldValue = value) Then Exit Property
        '        Me.m_StipendioNetto = value
        '        Me.DoChanged("StipendioNetto", value, oldValue)
        '    End Set
        'End Property

        Public Overrides Function ToString() As String
            Return Me.NumeroPratica
        End Function

        'Public Property TFR As Nullable(Of Decimal)
        '    Get
        '        Return Me.m_TFR
        '    End Get
        '    Set(value As Nullable(Of Decimal))
        '        Dim oldValue As Nullable(Of Decimal) = Me.m_TFR
        '        If (oldValue = value) Then Exit Property
        '        Me.m_TFR = value
        '        Me.DoChanged("TFR", value, oldValue)
        '    End Set
        'End Property

        'Public Property PercTFRAzienda As Nullable(Of Double)
        '    Get
        '        Return Me.m_PercTFRAzienda
        '    End Get
        '    Set(value As Nullable(Of Double))
        '        Dim oldValue As Nullable(Of Double) = Me.m_PercTFRAzienda
        '        If (oldValue = value) Then Exit Property
        '        Me.m_PercTFRAzienda = value
        '        Me.DoChanged("PercTFRAzienda", value, oldValue)
        '    End Set
        'End Property

        'Public Property TFRAzienda As Nullable(Of Decimal)
        '    Get
        '        If Me.m_TFR.HasValue And Me.m_PercTFRAzienda.HasValue Then
        '            Return Me.m_TFR.Value * Me.m_PercTFRAzienda.Value / 100
        '        Else
        '            Return Nothing
        '        End If
        '    End Get
        '    Set(value As Nullable(Of Decimal))
        '        Dim oldValue As Nullable(Of Decimal) = Me.TFRAzienda
        '        If (oldValue = value) Then Exit Property
        '        If value.HasValue Then
        '            If Me.m_TFR.HasValue Then
        '                Me.m_PercTFRAzienda = value.Value * 100 / Me.m_TFR.Value
        '            Else
        '                Me.m_TFR = value
        '                Me.m_PercTFRAzienda = 100
        '            End If
        '        Else
        '            Me.m_PercTFRAzienda = Nothing
        '        End If
        '        Me.DoChanged("TFRAzienda", value, oldValue)
        '    End Set
        'End Property

        'Public Property PercTFRFPC As Nullable(Of Double)
        '    Get
        '        Return 100 - Me.PercTFRAzienda
        '    End Get
        '    Set(value As Nullable(Of Double))
        '        Me.PercTFRAzienda = 100 - value
        '    End Set
        'End Property


        'Public Property TFRNomeFondo As String
        '    Get
        '        Return Me.m_TFRNomeFondo
        '    End Get
        '    Set(value As String)
        '        value = Trim(value)
        '        Dim oldValue As String = Me.m_TFRNomeFondo
        '        If (oldValue = value) Then Exit Property
        '        Me.m_TFRNomeFondo = Trim(value)
        '        Me.DoChanged("TFRNomeFondo", value, oldValue)
        '    End Set
        'End Property

        ' ''' <summary>
        ' ''' Restituisce o imposta una stringa che descrive il tipo di impiego del cliente
        ' ''' </summary>
        ' ''' <value></value>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Property TipoRapporto As String
        '    Get
        '        Return Me.m_TipoRapporto
        '    End Get
        '    Set(value As String)
        '        value = Trim(value)
        '        Dim oldValue As String = Me.m_TipoRapporto
        '        If (oldValue = value) Then Exit Property
        '        Me.m_TipoRapporto = value
        '        Me.DoChanged("TipoRapporto", value, oldValue)
        '    End Set
        'End Property

        'Public Property NumeroMensilita As Nullable(Of Integer)
        '    Get
        '        Return Me.m_NumeroMensilita
        '    End Get
        '    Set(value As Nullable(Of Integer))
        '        Dim oldValue As Nullable(Of Integer) = Me.m_NumeroMensilita
        '        If (oldValue = value) Then Exit Property
        '        Me.m_NumeroMensilita = value
        '        Me.DoChanged("NumeroMensilita", value, oldValue)
        '    End Set
        'End Property


        ''' <summary>
        ''' Restituisce un valore che indica se la pratica è stato trasferita presso un'altra azienda
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Trasferita As Boolean
            Get
                Return TestFlag(Me.m_Flags, PraticaFlags.TRASFERITA)
            End Get
            Set(value As Boolean)
                If (Me.Trasferita = value) Then Exit Property
                Me.m_Flags = SetFlag(Me.m_Flags, PraticaFlags.TRASFERITA, value)
                Me.DoChanged("Trasferita", value, Not value)
            End Set
        End Property

        Public Overrides Function GetTableName() As String
            Return "tbl_Rapportini"
        End Function



        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Public Overrides Function IsChanged() As Boolean
            Dim ret As Boolean = MyBase.IsChanged() OrElse Me.StatoDiLavorazioneAttuale.IsChanged OrElse Me.m_NatoA.IsChanged OrElse Me.m_ResidenteA.IsChanged OrElse Me.m_Impiego.IsChanged OrElse Me.Provvigionale.IsChanged
            If (Me.m_StatiDiLavorazione IsNot Nothing) Then ret = ret OrElse DBUtils.IsChanged(Me.m_StatiDiLavorazione)
            If (ret = False AndAlso Me.m_Cliente IsNot Nothing) Then ret = Me.m_Cliente.IsChanged
            'If (ret = False AndAlso Me.m_Documentazione IsNot Nothing) Then ret = DBUtils.IsChanged(Me.m_Documentazione)
            If (ret = False AndAlso Me.m_Info IsNot Nothing) Then ret = DBUtils.IsChanged(Me.m_Info)

            Return ret
        End Function

        Protected Overrides Function SaveToDatabase(dbConn As CDBConnection, ByVal force As Boolean) As Boolean
            Dim isNew As Boolean = GetID(Me) = 0
            Dim ret As Boolean
            'If (Me.m_Cliente IsNot Nothing) Then
            '    Me.Cliente.Save()
            '    Me.m_ClienteID = Databases.GetID(Me.Cliente, 0)
            'End If
            ret = MyBase.SaveToDatabase(dbConn, force)
            If (Me.m_Info IsNot Nothing AndAlso ret) Then Me.m_Info.Save(force)
            If (Me.m_StatoDiLavorazioneAttuale IsNot Nothing) Then Me.m_StatoDiLavorazioneAttuale.Save(force OrElse isNew)
            If (Me.m_StatiDiLavorazione IsNot Nothing AndAlso ret) Then Me.m_StatiDiLavorazione.Save(force)
            If (ret) Then
                If (Me.m_StatoDiLavorazioneAttuale IsNot Nothing) Then Me.m_StatoDiLavorazioneAttuale.SetChanged(False)
                Me.m_NatoA.SetChanged(False)
                'Me.m_StatoPraticaOld = Me.m_StatoPratica
                Me.m_ResidenteA.SetChanged(False)
                Me.m_Impiego.SetChanged(False)
                If (Me.m_StatiDiLavorazione IsNot Nothing) Then DBUtils.SetChanged(Me.m_StatiDiLavorazione, False)
                Me.m_Provvigionale.SetChanged(False)
            End If

            If Me.m_IDConsulenzaOld <> Me.IDConsulenza Then
                CQSPD.StudiDiFattibilita.AggiornaPratiche(Me.m_IDConsulenzaOld)
                CQSPD.StudiDiFattibilita.AggiornaPratiche(Me.IDConsulenza)
            Else
                CQSPD.StudiDiFattibilita.AggiornaPratiche(Me.IDConsulenza)
            End If
            Me.m_IDConsulenzaOld = Me.IDConsulenza

            Return ret
        End Function


        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            'Me.m_MacroStato = reader.Read("StatoPratica", Me.m_MacroStato)
            Me.m_DataDecorrenza = reader.Read("DataDecorrenza", Me.m_DataDecorrenza)
            Me.m_NomeCessionario = reader.Read("NomeCessionario", Me.m_NomeCessionario)
            Me.m_CessionarioID = reader.Read("Cessionario", Me.m_CessionarioID)

            Me.m_ProfiloID = reader.Read("Profilo", Me.m_ProfiloID)
            Me.m_NomeProfilo = reader.Read("NomeProfilo", Me.m_NomeProfilo)

            Me.m_ProdottoID = reader.Read("Prodotto", Me.m_ProdottoID)
            Me.m_NomeProdotto = reader.Read("CQS_PD", Me.m_NomeProdotto)

            Me.m_NumeroRate = reader.Read("NumeroRate", Me.m_NumeroRate)
            Me.m_MontanteLordo = reader.Read("MontanteLordo", Me.m_MontanteLordo)
            Me.m_NettoRicavo = reader.Read("NettoRicavo", Me.m_NettoRicavo)
            Me.m_ValoreProvvMax = reader.Read("ProvvMax", Me.m_ValoreProvvMax)

            Me.m_ValoreRunning = reader.Read("Running", Me.m_ValoreRunning)
            Me.m_ValoreUpFront = reader.Read("UpFront", Me.m_ValoreUpFront)
            Me.m_ValoreRappel = reader.Read("Rappel", Me.m_ValoreRappel)

            With Me.Provvigionale
                .Tipo = reader.Read("ProvvBrokerSu", TipoCalcoloProvvigionale.SOLOBASE)
                .ValoreBase = reader.Read("ProvvBroker", .ValoreBase)
                .ValorePercentuale = reader.Read("ProvvBrokerPerc", .ValorePercentuale)
                .SetChanged(False)
            End With

            Me.m_PremioDaCessionario = reader.Read("PremioDaCessionario", Me.m_PremioDaCessionario)



            Me.m_Flags = reader.Read("Flags", Me.m_Flags)

            Dim idSTL As Integer = 0
            idSTL = reader.Read("IDStatoDiLavorazioneAttuale", idSTL)
            DBUtils.SetID(Me.m_StatoDiLavorazioneAttuale, idSTL)
            With Me.m_StatoDiLavorazioneAttuale
                .MacroStato = reader.Read("StatoPratica", .MacroStato)
                .IDPratica = GetID(Me)
                .Data = reader.Read("STL_Data", .Data)
                .IDOperatore = reader.Read("STL_IDOP", .IDOperatore)
                .NomeOperatore = reader.Read("STL_NMOP", .NomeOperatore)
                .Note = reader.Read("STL_NOTE", .Note)
                .Params = reader.Read("STL_PARS", .Params)
                .Forzato = reader.Read("STL_FLAGS", .Forzato)
                .IDOfferta = reader.Read("IDOffertaCorrente", .IDOfferta)
                .IDFromStato = reader.Read("STL_FROMS", .IDFromStato)
                .IDStatoPratica = reader.Read("IDStatoAttuale", .IDStatoPratica)
                .DescrizioneStato = reader.Read("STL_DESCST", .DescrizioneStato)
                .IDRegolaApplicata = reader.Read("STL_RULE", .IDRegolaApplicata)
                .SetChanged(False)
            End With

            Me.m_TipoFonteContatto = reader.Read("TipoFonteContatto", Me.m_TipoFonteContatto)
            Me.m_IDFonte = reader.Read("IDFonte", Me.m_IDFonte)
            Me.m_NomeFonte = reader.Read("FonteContatto", Me.m_NomeFonte)

            Me.m_TipoFonteCliente = reader.Read("TipoFonteCliente", Me.m_TipoFonteCliente)
            Me.m_IDFonteCliente = reader.Read("IDFonteCliente", Me.m_IDFonteCliente)

            Me.m_IDCanale = reader.Read("IDCanale", Me.m_IDCanale)
            Me.m_NomeCanale = reader.Read("NomeCanale", Me.m_NomeCanale)

            Me.m_IDCanale1 = reader.Read("IDCanale1", Me.m_IDCanale1)
            Me.m_NomeCanale1 = reader.Read("NomeCanale1", Me.m_NomeCanale1)

            Me.m_ClienteID = reader.Read("Cliente", Me.m_ClienteID)
            Me.m_NomeCliente = reader.Read("NomeCliente", Me.m_NomeCliente)
            Me.m_CognomeCliente = reader.Read("CognomeCliente", Me.m_CognomeCliente)
            Me.m_NatoA.Citta = reader.Read("NatoAComune", Me.m_NatoA.Citta)
            Me.m_NatoA.Provincia = reader.Read("NatoAProvincia", Me.m_NatoA.Provincia)
            Me.m_NatoA.SetChanged(False)
            Me.m_NatoIl = reader.Read("NatoIl", Me.m_NatoIl)
            Me.m_ResidenteA.Citta = reader.Read("ResidenteAComune", Me.m_ResidenteA.Citta)
            Me.m_ResidenteA.Provincia = reader.Read("ResidenteAProvincia", Me.m_ResidenteA.Provincia)
            Me.m_ResidenteA.CAP = reader.Read("ResidenteACAP", Me.m_ResidenteA.CAP)
            Me.m_ResidenteA.ToponimoViaECivico = reader.Read("ResidenteAVia", Me.m_ResidenteA.ToponimoViaECivico)
            Me.m_ResidenteA.SetChanged(False)
            Me.m_CodiceFiscale = reader.Read("CodiceFiscale", Me.m_CodiceFiscale)
            Me.m_Sesso = reader.Read("Sesso", Me.m_Sesso)
            Me.m_Telefono = reader.Read("Telefono", Me.m_Telefono)
            Me.m_Cellulare = reader.Read("Cellulare", Me.m_Cellulare)
            Me.m_Fax = reader.Read("Fax", Me.m_Fax)
            Me.m_eMail = reader.Read("eMail", Me.m_eMail)


            Me.m_Impiego.IDEntePagante = reader.Read("IDEntePagante", Me.m_Impiego.IDEntePagante)
            Me.m_Impiego.IDAzienda = reader.Read("IDAmministrazione", Me.m_Impiego.IDAzienda)
            Me.m_Impiego.NomeAzienda = reader.Read("Ente", Me.m_Impiego.NomeAzienda)
            Me.m_Impiego.DataAssunzione = reader.Read("DataAssunzione", Me.m_Impiego.DataAssunzione)
            Me.m_Impiego.Posizione = reader.Read("Posizione", Me.m_Impiego.Posizione)
            Me.m_Impiego.StipendioLordo = reader.Read("StipendioLordo", Me.m_Impiego.StipendioLordo)
            Me.m_Impiego.StipendioNetto = reader.Read("StipendioNetto", Me.m_Impiego.StipendioNetto)
            Me.m_Impiego.TFR = reader.Read("TFR", Me.m_Impiego.TFR)
            Me.m_Impiego.PercTFRAzienda = reader.Read("PercTFRAzienda", Me.m_Impiego.PercTFRAzienda)
            Me.m_Impiego.NomeFPC = reader.Read("TFRNomeFondo", Me.m_Impiego.NomeFPC)
            Me.m_Impiego.TipoRapporto = reader.Read("TipoImpiego", Me.m_Impiego.TipoRapporto)
            Me.m_Impiego.MensilitaPercepite = reader.Read("NumeroMensilita", Me.m_Impiego.MensilitaPercepite)
            Me.m_Impiego.SetChanged(False)

            '-------------------------------
            Me.m_PartitaIVA = reader.Read("PartitaIVA", Me.m_PartitaIVA)

            Me.m_NumeroEsterno = reader.Read("StatRichD_Params", Me.m_NumeroEsterno)

            Me.m_IDConsulente = reader.Read("IDConsulente", Me.m_IDConsulente)
            Me.m_IDConsulenza = reader.Read("IDConsulenza", Me.m_IDConsulenza)
            'Me.m_IDConsulenzaOld = Me.m_IDConsulenza
            Me.m_IDRichiestaDiFinanziamento = reader.Read("IDRichiestaFinanziamento", Me.m_IDRichiestaDiFinanziamento)

            Me.m_IDAzienda = reader.Read("IDAzienda", Me.m_IDAzienda)

            Me.m_IDContesto = reader.Read("IDContesto", Me.m_IDContesto)
            Me.m_TipoContesto = reader.Read("TipoContesto", Me.m_TipoContesto)

            Me.m_Durata = reader.Read("Durata", Me.m_Durata)

            Me.m_IDRichiestaApprovazione = reader.Read("IDRichiestaApprovazione", Me.m_IDRichiestaApprovazione)

            Try
                Dim attributiString As String = ""
                attributiString = reader.Read("Attributi", attributiString)
                If (attributiString <> "") Then
                    Me.m_Attributi = XML.Utils.Serializer.Deserialize(attributiString)
                Else
                    Me.m_Attributi = New CKeyCollection
                End If
            Catch ex As Exception
                Me.m_Attributi = New CKeyCollection
            End Try

            Me.m_IDFinestraLavorazione = reader.Read("IDFinestraLavorazione", Me.m_IDFinestraLavorazione)
            Me.m_IDTabellaFinanziaria = reader.Read("IDTabellaFinanziaria", Me.m_IDTabellaFinanziaria)
            Me.m_IDTabellaVita = reader.Read("IDTabellaVita", Me.m_IDTabellaVita)
            Me.m_IDTabellaImpiego = reader.Read("IDTabellaImpiego", Me.m_IDTabellaImpiego)
            Me.m_IDTabellaCredito = reader.Read("IDTabellaCredito", Me.m_IDTabellaCredito)

            Me.m_IDUltimaVerifica = reader.Read("IDUltimaVerifica", Me.m_IDUltimaVerifica)

            Me.m_DataValuta = reader.Read("DataValuta", Me.m_DataValuta)
            Me.m_DataStampaSecci = reader.Read("DataStampaSecci", Me.m_DataStampaSecci)

            Me.m_CapitaleFinanziato = reader.Read("CapitaleFinanziato", Me.m_CapitaleFinanziato)

            Me.m_IDCollaboratore = reader.Read("IDCollaboratore", Me.m_IDCollaboratore)

            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("DataDecorrenza", Me.m_DataDecorrenza)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("IDStatoDiLavorazioneAttuale", GetID(Me.StatoDiLavorazioneAttuale))
            If (Me.StatoDiLavorazioneAttuale Is Nothing) Then
                With Me.StatoDiLavorazioneAttuale
                    writer.Write("StatoPratica", DBNull.Value)
                    writer.Write("STL_Data", DBNull.Value)
                    writer.Write("STL_IDOP", 0)
                    writer.Write("STL_NMOP", "")
                    writer.Write("STL_NOTE", "")
                    writer.Write("STL_PARS", "")
                    writer.Write("STL_FLAGS", DBNull.Value)
                    writer.Write("IDOffertaCorrente", 0)
                    writer.Write("STL_FROMS", 0)
                    writer.Write("IDStatoAttuale", GetID(Me.StatoAttuale))
                    writer.Write("STL_DESCST", "")
                    writer.Write("STL_RULE", 0)
                End With
            Else
                With Me.StatoDiLavorazioneAttuale
                    writer.Write("StatoPratica", .MacroStato)
                    writer.Write("STL_Data", .Data)
                    writer.Write("STL_IDOP", .IDOperatore)
                    writer.Write("STL_NMOP", .NomeOperatore)
                    writer.Write("STL_NOTE", .Note)
                    writer.Write("STL_PARS", .Params)
                    writer.Write("STL_FLAGS", .Forzato)
                    writer.Write("IDOffertaCorrente", .IDOfferta)
                    writer.Write("STL_FROMS", .IDFromStato)
                    writer.Write("IDStatoAttuale", .IDStatoPratica)
                    writer.Write("STL_DESCST", .DescrizioneStato)
                    writer.Write("STL_RULE", .IDRegolaApplicata)
                End With
            End If

            If (Me.StatoAttuale IsNot Nothing) Then
                writer.Write("StatoPratica", Me.StatoAttuale.MacroStato)
                Dim ordine As Integer = CQSPD.StatiPratica.GetSequenzaStandard.IndexOf(Me.StatoAttuale)
                writer.Write("Ordine", ordine)
            Else
                writer.Write("StatoPratica", DBNull.Value)
                writer.Write("Ordine", -1)
            End If

            writer.Write("PremioDaCessionario", Me.m_PremioDaCessionario)

            writer.Write("TipoFonteContatto", Me.m_TipoFonteContatto)
            writer.Write("IDFonte", Me.IDFonte)
            writer.Write("FonteContatto", Me.m_NomeFonte)

            writer.Write("TipoFonteCliente", Me.m_TipoFonteCliente)
            writer.Write("IDFonteCliente", Me.IDFonteCliente)


            writer.Write("IDCanale", Me.IDCanale)
            writer.Write("NomeCanale", Me.m_NomeCanale)

            writer.Write("IDCanale1", Me.IDCanale1)
            writer.Write("NomeCanale1", Me.m_NomeCanale1)

            'writer.Write("NomeCessionario", Me.m_NomeCessionario)
            writer.Write("Cessionario", Me.IDCessionario)

            writer.Write("Profilo", Me.IDProfilo)
            writer.Write("NomeProfilo", Me.m_NomeProfilo)

            writer.Write("Prodotto", Me.IDProdotto)
            writer.Write("CQS_PD", Me.m_NomeProdotto)

            writer.Write("NumeroRate", Me.m_NumeroRate)
            writer.Write("MontanteLordo", Me.m_MontanteLordo)
            writer.Write("NettoRicavo", Me.m_NettoRicavo)
            writer.Write("ProvvMax", Me.m_ValoreProvvMax)
            writer.Write("Running", Me.m_ValoreRunning)
            writer.Write("UpFront", Me.m_ValoreUpFront)
            writer.Write("Rappel", Me.m_ValoreRappel)

            writer.Write("Cliente", Me.IDCliente)
            writer.Write("NomeCliente", Me.m_NomeCliente)
            writer.Write("CognomeCliente", Me.m_CognomeCliente)
            writer.Write("NatoAComune", Me.m_NatoA.Citta)
            writer.Write("NatoAProvincia", Me.m_NatoA.Provincia)
            writer.Write("NatoIl", Me.m_NatoIl)
            writer.Write("ResidenteAComune", Me.m_ResidenteA.Citta)
            writer.Write("ResidenteAProvincia", Me.m_ResidenteA.Provincia)
            writer.Write("ResidenteACAP", Me.m_ResidenteA.CAP)
            writer.Write("ResidenteAVia", Me.m_ResidenteA.ToponimoViaECivico)
            writer.Write("CodiceFiscale", Me.m_CodiceFiscale)
            writer.Write("Sesso", Me.m_Sesso)
            writer.Write("Telefono", Me.m_Telefono)
            writer.Write("Cellulare", Me.m_Cellulare)
            writer.Write("Fax", Me.m_Fax)
            writer.Write("eMail", Me.m_eMail)

            writer.Write("IDEntePagante", Me.m_Impiego.IDEntePagante)
            writer.Write("IDAmministrazione", Me.m_Impiego.IDAzienda)
            writer.Write("Ente", Me.m_Impiego.NomeAzienda)
            writer.Write("DataAssunzione", Me.m_Impiego.DataAssunzione)
            writer.Write("Posizione", Me.m_Impiego.Posizione)
            writer.Write("StipendioLordo", Me.m_Impiego.StipendioLordo)
            writer.Write("StipendioNetto", Me.m_Impiego.StipendioNetto)
            writer.Write("TFR", Me.m_Impiego.TFR)
            writer.Write("PercTFRAzienda", Me.m_Impiego.PercTFRAzienda)
            writer.Write("TFRNomeFondo", Me.m_Impiego.NomeFPC)
            writer.Write("TipoImpiego", Me.m_Impiego.TipoRapporto)
            writer.Write("NumeroMensilita", Me.m_Impiego.MensilitaPercepite)

            '-------------------------------
            writer.Write("PartitaIVA", Me.m_PartitaIVA)

            writer.Write("StatRichD_Params", Me.m_NumeroEsterno)




            writer.Write("IDConsulente", Me.IDConsulente)
            writer.Write("IDConsulenza", Me.IDConsulenza)
            writer.Write("IDRichiestaFinanziamento", Me.IDRichiestaDiFinanziamento)
            writer.Write("IDAzienda", Me.IDAzienda)
            writer.Write("IDContesto", Me.IDContesto)
            writer.Write("TipoContesto", Me.m_TipoContesto)
            writer.Write("Durata", Me.m_Durata)

            With Me.Provvigionale
                writer.Write("ProvvBrokerSu", .Tipo)
                writer.Write("ProvvBroker", .ValoreBase)
                writer.Write("ProvvBrokerPerc", .ValorePercentuale)
            End With

            writer.Write("IDRichiestaApprovazione", Me.IDRichiestaApprovazione)

            writer.Write("Attributi", XML.Utils.Serializer.Serialize(Me.Attributi))

            writer.Write("IDFinestraLavorazione", Me.IDFinestraLavorazione)
            writer.Write("IDTabellaFinanziaria", Me.IDTabellaFinanziaria)
            writer.Write("IDTabellaVita", Me.IDTabellaVita)
            writer.Write("IDTabellaImpiego", Me.IDTabellaImpiego)
            writer.Write("IDTabellaCredito", Me.IDTabellaCredito)

            writer.Write("IDUltimaVerifica", Me.IDUltimaVerifica)

            writer.Write("DataValuta", Me.m_DataValuta)
            writer.Write("DataStampaSecci", Me.m_DataStampaSecci)

            writer.Write("CapitaleFinanziato", Me.m_CapitaleFinanziato)

            writer.Write("IDCollaboratore", Me.IDCollaboratore)

            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
            writer.WriteAttribute("ClienteID", Me.IDCliente)
            writer.WriteAttribute("NomeCliente", Me.m_NomeCliente)
            writer.WriteAttribute("CognomeCliente", Me.m_CognomeCliente)
            writer.WriteAttribute("NatoIl", Me.m_NatoIl)
            writer.WriteAttribute("CodiceFiscale", Me.m_CodiceFiscale)
            writer.WriteAttribute("PartitaIVA", Me.m_PartitaIVA)
            writer.WriteAttribute("Sesso", Me.m_Sesso)
            writer.WriteAttribute("Telefono", Me.m_Telefono)
            writer.WriteAttribute("Cellulare", Me.m_Cellulare)
            writer.WriteAttribute("Fax", Me.m_Fax)
            writer.WriteAttribute("eMail", Me.m_eMail)
            writer.WriteAttribute("CessionarioID", Me.IDCessionario)
            writer.WriteAttribute("NomeCessionario", Me.m_NomeCessionario)
            writer.WriteAttribute("ProfiloID", Me.IDProfilo)
            writer.WriteAttribute("NomeProfilo", Me.m_NomeProfilo)
            writer.WriteAttribute("ProdottoID", Me.IDProdotto)
            writer.WriteAttribute("NomeProdotto", Me.m_NomeProdotto)
            writer.WriteAttribute("MontanteLordo", Me.m_MontanteLordo)
            writer.WriteAttribute("NettoRicavo", Me.m_NettoRicavo)
            writer.WriteAttribute("NumeroRate", Me.m_NumeroRate)
            writer.WriteAttribute("ProvvMax", Me.m_ValoreProvvMax)
            writer.WriteAttribute("Running", Me.m_ValoreRunning)
            writer.WriteAttribute("UpFront", Me.m_ValoreUpFront)
            writer.WriteAttribute("Rappel", Me.m_ValoreRappel)
            writer.WriteAttribute("PremioDaCessionario", Me.m_PremioDaCessionario)
            writer.WriteAttribute("DataDecorrenza", Me.m_DataDecorrenza)
            writer.WriteAttribute("Flags", Me.m_Flags)
            'writer.WriteTag("StatoPratica", Me.m_StatoPratica)
            'writer.WriteTag("StatoPraticaOld", Me.m_StatoPraticaOld)

            writer.WriteAttribute("TipoFonteContatto", Me.m_TipoFonteContatto)
            writer.WriteAttribute("FonteContatto", Me.m_NomeFonte)
            writer.WriteAttribute("IDFonte", Me.IDFonte)
            writer.WriteAttribute("IDCanale", Me.IDCanale)
            writer.WriteAttribute("NomeCanale", Me.m_NomeCanale)
            writer.WriteAttribute("IDCanale1", Me.IDCanale1)
            writer.WriteAttribute("NomeCanale1", Me.m_NomeCanale1)
            writer.WriteAttribute("NumeroPraticaEsterna", Me.m_NumeroEsterno)
            writer.WriteAttribute("IDConsulente", Me.IDConsulente)
            writer.WriteAttribute("IDConsulenza", Me.IDConsulenza)
            'writer.WriteAttribute("IDConsulenzaOld", Me.m_IDConsulenzaOld)
            writer.WriteAttribute("IDRichiestaFinanziamento", Me.IDRichiestaDiFinanziamento)
            writer.WriteAttribute("IDAzienda", Me.IDAzienda)
            writer.WriteAttribute("IDContesto", Me.IDContesto)
            writer.WriteAttribute("TipoContesto", Me.m_TipoContesto)
            writer.WriteAttribute("Durata", Me.m_Durata)

            writer.WriteAttribute("IDRichiestaApprovazione", Me.IDRichiestaApprovazione)

            writer.WriteAttribute("TipoFonteCliente", Me.m_TipoFonteCliente)
            writer.WriteAttribute("IDFonteCliente", Me.IDFonteCliente)
            writer.WriteAttribute("IDFinestraLavorazione", Me.IDFinestraLavorazione)
            writer.WriteAttribute("IDTabellaFinanziaria", Me.IDTabellaFinanziaria)
            writer.WriteAttribute("IDTabellaVita", Me.IDTabellaVita)
            writer.WriteAttribute("IDTabellaImpiego", Me.IDTabellaImpiego)
            writer.WriteAttribute("IDTabellaCredito", Me.IDTabellaCredito)
            writer.WriteAttribute("IDUltimaVerifica", Me.IDUltimaVerifica)

            writer.WriteAttribute("DataValuta", Me.m_DataValuta)
            writer.WriteAttribute("DataStampaSecci", Me.m_DataStampaSecci)
            writer.WriteAttribute("CapitaleFinanziato", Me.m_CapitaleFinanziato)
            writer.WriteAttribute("IDCollaboratore", Me.IDCollaboratore)

            MyBase.XMLSerialize(writer)
            'writer.WriteTag("Info", Me.Info)
            writer.WriteTag("StatoDiLavorazioneAttuale", Me.StatoDiLavorazioneAttuale)
            writer.WriteTag("NatoA", Me.m_NatoA)
            writer.WriteTag("ResidenteA", Me.m_ResidenteA)
            writer.WriteTag("Provvigionale", Me.Provvigionale)
            writer.WriteTag("Impiego", Me.m_Impiego)
            If (Not writer.Settings.GetValueBool("CRapportino.fastXMLserialize")) Then
                writer.WriteTag("StatiDiLavorazione", Me.StatiDiLavorazione)
                If (Me.Prodotto IsNot Nothing) Then Me.Attributi.SetItemByKey("ColoreProdotto", Me.Prodotto.Attributi.GetItemByKey("Colore"))
                If (Me.OffertaCorrente IsNot Nothing AndAlso Me.OffertaCorrente.TabellaFinanziariaRel IsNot Nothing AndAlso Me.OffertaCorrente.TabellaFinanziariaRel.Tabella IsNot Nothing) Then
#If Not DEBUG Then
                Try
#End If
                    Me.Attributi.SetItemByKey("ColoreTabellaFinanziaria", Me.OffertaCorrente.TabellaFinanziariaRel.Tabella.Attributi.GetItemByKey("Colore"))
#If Not DEBUG Then
                Catch ex As Exception
                     
                End Try
#End If
                End If
            End If
            writer.WriteTag("Attributi", Me.Attributi)
            'writer.WriteTag("OffertaCorrente", Me.OffertaCorrente)
        End Sub

        Protected Overrides Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object)
            Select Case fieldName
                Case "IDFonteCliente" : Me.m_IDFonteCliente = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "TipoFonteCliente" : Me.m_TipoFonteCliente = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ClienteID" : Me.m_ClienteID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeCliente" : Me.m_NomeCliente = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "CognomeCliente" : Me.m_CognomeCliente = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "NatoA" : Me.m_NatoA = fieldValue
                Case "NatoIl" : Me.m_NatoIl = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "ResidenteA" : Me.m_ResidenteA = fieldValue
                Case "CodiceFiscale" : Me.m_CodiceFiscale = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "PartitaIVA" : Me.m_PartitaIVA = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Sesso" : Me.m_Sesso = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Telefono" : Me.m_Telefono = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Cellulare" : Me.m_Cellulare = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Fax" : Me.m_Fax = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "eMail" : Me.m_eMail = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Impiego" : Me.m_Impiego = fieldValue
                Case "CessionarioID" : m_CessionarioID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeCessionario" : m_NomeCessionario = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ProfiloID" : Me.m_ProfiloID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeProfilo" : Me.m_NomeProfilo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ProdottoID" : Me.m_ProdottoID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeProdotto" : Me.m_NomeProdotto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "MontanteLordo" : Me.m_MontanteLordo = XML.Utils.Serializer.DeserializeFloat(fieldValue)
                Case "NettoRicavo" : Me.m_NettoRicavo = XML.Utils.Serializer.DeserializeFloat(fieldValue)
                Case "NumeroRate" : Me.m_NumeroRate = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "ProvvMax" : Me.m_ValoreProvvMax = XML.Utils.Serializer.DeserializeFloat(fieldValue)
                Case "Provvigionale" : Me.m_Provvigionale = fieldValue
                Case "Running" : Me.m_ValoreRunning = XML.Utils.Serializer.DeserializeFloat(fieldValue)
                Case "UpFront" : Me.m_ValoreUpFront = XML.Utils.Serializer.DeserializeFloat(fieldValue)
                Case "Rappel" : Me.m_ValoreRappel = XML.Utils.Serializer.DeserializeFloat(fieldValue)
                Case "PremioDaCessionario" : Me.m_PremioDaCessionario = XML.Utils.Serializer.DeserializeDouble(fieldValue)

                Case "DataDecorrenza" : Me.m_DataDecorrenza = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)


                    ' Case "StatoPratica" : Me.m_StatoPratica = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                    ' Case "StatoPraticaOld" : Me.m_StatoPraticaOld = XML.Utils.Serializer.DeserializeInteger(fieldValue)

                Case "TipoFonteContatto" : Me.m_TipoFonteContatto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "FonteContatto" : Me.m_NomeFonte = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDFonte" : Me.m_IDFonte = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDCanale" : Me.m_IDCanale = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeCanale" : Me.m_NomeCanale = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDCanale1" : Me.m_IDCanale1 = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeCanale1" : Me.m_NomeCanale1 = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "NumeroPraticaEsterna" : Me.m_NumeroEsterno = XML.Utils.Serializer.DeserializeString(fieldValue)
                    'Case "StatoAttuale" : Me.m_StatoAttuale = fieldValue
                Case "StatiDiLavorazione"
                    Me.m_StatiDiLavorazione = fieldValue
                    If (Me.m_StatiDiLavorazione IsNot Nothing) Then Me.m_StatiDiLavorazione.SetPratica(Me)
                Case "StatoDiLavorazioneAttuale"
                    Me.m_StatoDiLavorazioneAttuale = CType(fieldValue, CStatoLavorazionePratica) ' : Me.StatoDiLavorazioneAttuale.SetPratica(Me)
                    'DBUtils.SetID(Me.m_StatoDiLavorazioneAttuale, GetID(fieldValue))
                    If (Me.m_StatoDiLavorazioneAttuale IsNot Nothing) Then Me.m_StatoDiLavorazioneAttuale.SetPratica(Me)
                Case "IDConsulente" : Me.m_IDConsulente = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDConsulenza" : Me.m_IDConsulenza = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                    'Case "IDConsulenzaOld" : Me.m_IDConsulenzaOld = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDRichiestaFinanziamento" : Me.m_IDRichiestaDiFinanziamento = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDAzienda" : Me.m_IDAzienda = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDContesto" : Me.m_IDContesto = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "TipoContesto" : Me.m_TipoContesto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Durata" : Me.m_Durata = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDRichiestaApprovazione" : Me.m_IDRichiestaApprovazione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Attributi" : Me.m_Attributi = fieldValue
                Case "IDFinestraLavorazione" : Me.m_IDFinestraLavorazione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDTabellaFinanziaria" : Me.m_IDTabellaFinanziaria = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDTabellaVita" : Me.m_IDTabellaVita = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDTabellaImpiego" : Me.m_IDTabellaImpiego = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDTabellaCredito" : Me.m_IDTabellaCredito = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDUltimaVerifica" : Me.m_IDUltimaVerifica = XML.Utils.Serializer.DeserializeInteger(fieldValue)

                Case "DataValuta" : Me.m_DataValuta = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataStampaSecci" : Me.m_DataStampaSecci = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Info" : Me.m_Info = CType(fieldValue, CInfoPratica)

                Case "CapitaleFinanziato" : Me.m_CapitaleFinanziato = XML.Utils.Serializer.DeserializeDouble(fieldValue)

                Case "IDCollaboratore" : Me.m_IDCollaboratore = XML.Utils.Serializer.DeserializeInteger(fieldValue)

                Case Else : Call MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Overrides Sub InitializeFrom(value As Object)
            MyBase.InitializeFrom(value)
            Me.m_TipoFonteContatto = ""
            Me.m_NomeFonte = ""
            Me.m_IDFonte = 0
            Me.m_Fonte = Nothing
            Me.m_NomeProdotto = ""
            Me.m_MontanteLordo = Nothing
            Me.m_NettoRicavo = Nothing
            Me.m_Cessionario = Nothing
            Me.m_CessionarioID = 0
            Me.m_NomeCessionario = ""
            Me.m_Profilo = Nothing
            Me.m_ProfiloID = 0
            Me.m_NomeProfilo = ""
            Me.m_ValoreProvvMax = Nothing
            Me.m_Provvigionale = New CProvvigionale
            Me.m_ValoreRunning = Nothing
            Me.m_ValoreUpFront = Nothing
            Me.m_ValoreRappel = Nothing
            Me.m_DataDecorrenza = Nothing

            Me.m_Canale = Nothing
            Me.m_IDCanale = 0
            Me.m_NomeCanale = ""

            Me.m_Canale1 = Nothing
            Me.m_IDCanale1 = 0
            Me.m_NomeCanale1 = ""

            Me.m_ProdottoID = 0
            Me.m_Prodotto = Nothing
            Me.m_NomeProdotto = ""
            Me.m_NumeroRate = Nothing
            Me.m_NatoA = New CIndirizzo
            Me.m_NatoA.InitializeFrom(DirectCast(value, CRapportino).m_NatoA)
            Me.m_ResidenteA.InitializeFrom(DirectCast(value, CRapportino).m_ResidenteA)
            Me.m_Impiego.InitializeFrom(DirectCast(value, CRapportino).m_Impiego)
            Me.m_Info = Nothing
            Me.m_NumeroEsterno = vbNullString
            'Me.StatoDiLavorazioneAttuale.Ini = 0
            Me.m_StatoDiLavorazioneAttuale = Nothing
            Me.m_IDConsulente = 0
            Me.m_Consulente = Nothing
            Me.m_IDConsulenza = 0
            Me.m_Consulenza = Nothing
            Me.m_IDRichiestaDiFinanziamento = 0
            Me.m_RichiestaDiFinanziamento = Nothing
            Me.m_DataValuta = Nothing
            Me.m_DataStampaSecci = Nothing
            Me.m_CapitaleFinanziato = Nothing

            With DirectCast(value, CRapportino)
                For Each k As String In .Attributi.Keys
                    Me.Attributi.SetItemByKey(k, .Attributi(k))
                Next
            End With
        End Sub

        Public Sub Correggi(ByVal offerta As COffertaCQS)
            If (offerta Is Nothing) Then Throw New ArgumentNullException("offerta")
            If (Me.Info.Correzione Is Nothing) Then
                Dim corr As New COffertaCQS()
                corr.InitializeFrom(Me.OffertaCorrente)
                corr.Stato = ObjectStatus.OBJECT_VALID
                corr.Save()
                Me.Info.Correzione = corr
            End If
            Me.OffertaCorrente = offerta
            Me.Save()
            Me.OnCorretta(New ItemEventArgs(Me))
        End Sub

        Protected Overridable Sub OnCorretta(ByVal e As ItemEventArgs)
            RaiseEvent Corretta(Me, e)
            Pratiche.DoOnCorretta(New ItemEventArgs(Me))
            Me.GetModule.DispatchEvent(New EventDescription("corretta", Users.CurrentUser.Nominativo & " ha apportato una correzione alla pratica N°" & Me.NumeroPratica, Me))
        End Sub

        Protected Overrides Sub OnCreate(e As SystemEvent)
            MyBase.OnCreate(e)
            Pratiche.DoOnCreate(New ItemEventArgs(Me))
        End Sub

        Protected Overrides Sub OnDelete(e As SystemEvent)
            Dim ra As CRichiestaApprovazione = Me.RichiestaApprovazione
            If (ra IsNot Nothing) Then
                Select Case ra.StatoRichiesta
                    Case StatoRichiestaApprovazione.ATTESA, StatoRichiestaApprovazione.NONCHIESTA, StatoRichiestaApprovazione.PRESAINCARICO
                        ra.StatoRichiesta = StatoRichiestaApprovazione.ANNULLATA
                        ra.MotivoConferma = "Pratica Eliminata"
                        ra.DettaglioConferma = Strings.Combine(ra.DettaglioConferma, ra.MotivoConferma & vbNewLine & "Operatore: " & Sistema.Users.CurrentUser.Nominativo, vbNewLine)
                        ra.Save()
                End Select
            End If

            Me.RilasciaAltriPrestiti()

            MyBase.OnDelete(e)

            CQSPD.Pratiche.DoOnDelete(New ItemEventArgs(Me))
        End Sub

        Protected Overrides Sub OnModified(e As SystemEvent)
            MyBase.OnModified(e)
            CQSPD.Pratiche.DoOnModified(New ItemEventArgs(Me))
            Me.GetModule.DispatchEvent(New EventDescription("Edit", "Modificata la pratica N°" & Me.NumeroPratica, Me))
        End Sub

        Protected Overridable Sub OnRifiutata(ByVal e As ItemEventArgs)
            RaiseEvent Rifiutata(Me, e)
        End Sub

        Protected Overridable Sub OnApproved(ByVal e As ItemEventArgs)
            RaiseEvent Approvata(Me, e)
        End Sub

        Protected Overridable Sub OnChangeStatus(ByVal e As ItemEventArgs)
            RaiseEvent StatusChanged(Me, e)
            Pratiche.DoOnChangeStatus(e)
        End Sub

        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            MyBase.OnAfterSave(e)
        End Sub

        Private Function GetCurrentStato() As Integer
            Return Formats.ToInteger(Me.GetConnection.ExecuteScalar("SELECT [IDStatoDiLavorazioneAttuale] FROM " & Me.GetTableName & " WHERE [ID]=" & DBUtils.DBNumber(GetID(Me))))
        End Function

        Public Function ChangeStatus(ByVal toStato As CStatoPratica, ByVal regola As CStatoPratRule, ByVal data As Date, ByVal param As String, ByVal notes As String, ByVal operatore As CUser) As CStatoLavorazionePratica
            If (Me.OffertaCorrente Is Nothing) Then Throw New ArgumentNullException("Offerta Correte")

            If (Not regola Is Nothing) Then
                Dim ra As CRichiestaApprovazione = Me.RichiestaApprovazione
                If (ra IsNot Nothing AndAlso ra.StatoRichiesta <> StatoRichiestaApprovazione.APPROVATA) Then
                    If (ra.MotivoRichiesta IsNot Nothing AndAlso ra.MotivoRichiesta.SoloSegnalazione) Then

                    Else
                        If (toStato.MacroStato.HasValue = False) OrElse (toStato.MacroStato.Value <> StatoPraticaEnum.STATO_ANNULLATA) Then Throw New ArgumentException("L'offerta deve essere approvata")
                    End If
                End If
            End If

            'Verifichiamo che la pratica sia ancora nello stato corrente per evitare doppi passaggi di stato
            If (GetID(Me.StatoDiLavorazioneAttuale) <> Me.GetCurrentStato()) Then
                Throw New InvalidOperationException("Lo stato della pratica è stato modificato")
            End If

            Dim statoLav As CStatoLavorazionePratica
            statoLav = New CStatoLavorazionePratica

            statoLav.Data = data
            statoLav.Operatore = operatore
            statoLav.Pratica = Me
            statoLav.IDOfferta = Me.IDOffertaCorrente
            statoLav.FromStato = Me.StatoDiLavorazioneAttuale
            statoLav.StatoPratica = toStato
            statoLav.RegolaApplicata = regola
            statoLav.Forzato = regola Is Nothing
            statoLav.Params = param
            statoLav.Note = notes
            statoLav.Stato = ObjectStatus.OBJECT_VALID
            statoLav.MacroStato = toStato.MacroStato
            statoLav.Save()

            Me.StatiDiLavorazione.Add(statoLav)
            Me.StatiDiLavorazione.Sort()

            Me.m_StatoDiLavorazioneAttuale = statoLav

            Me.Save(True)

            If (Me.StatoAttuale.MacroStato = StatoPraticaEnum.STATO_ANNULLATA) Then
                Dim ra As CRichiestaApprovazione = Me.RichiestaApprovazione
                If (ra IsNot Nothing) Then
                    Select Case ra.StatoRichiesta
                        Case StatoRichiestaApprovazione.ATTESA, StatoRichiestaApprovazione.NONCHIESTA, StatoRichiestaApprovazione.PRESAINCARICO
                            ra.StatoRichiesta = StatoRichiestaApprovazione.ANNULLATA
                            If (regola IsNot Nothing) Then
                                If TestFlag(regola.Flags, FlagsRegolaStatoPratica.DaCliente) Then
                                    ra.MotivoConferma = "Pratica rifiuntata dal cliente"
                                ElseIf TestFlag(regola.Flags, FlagsRegolaStatoPratica.Bocciata) Then
                                    ra.MotivoConferma = "Pratica bocciata dall'agenzia"
                                ElseIf TestFlag(regola.Flags, FlagsRegolaStatoPratica.NonFattibile) Then
                                    ra.MotivoConferma = "Pratica non fattibile"
                                Else
                                    ra.MotivoConferma = "Pratica Annullata"
                                End If
                            Else
                                ra.MotivoConferma = "Pratica Annullata"
                            End If
                            ra.DettaglioConferma = Strings.Combine(ra.DettaglioConferma, ra.MotivoConferma & vbNewLine & "Operatore: " & operatore.Nominativo & vbNewLine & notes, vbNewLine)
                            ra.Save()
                    End Select
                End If
            End If


            'Controlla se lo stato corrente prevede di acquisire o rilasciare le estinzioni
            If (Me.StatoAttuale.AcquisisciEstinzioni) Then
                Me.AcquisisciAltriPrestiti()
            ElseIf (Me.StatoAttuale.RilasciaEstinzioni) Then
                Me.RilasciaAltriPrestiti()
            End If


            Me.OnChangeStatus(New ItemEventArgs(Me))
            Return statoLav
        End Function

        ''' <summary>
        ''' Questo metodo viene richiamato quando la pratica viene passata in uno stato che
        ''' richiede l'acquisizione degli altri prestiti
        ''' </summary>
        ''' <remarks></remarks>
        Protected Overridable Sub AcquisisciAltriPrestiti()
            Dim cursor As New CEstinzioniCursor
            Dim nEst As New CEstinzione

            cursor.IgnoreRights = True
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.IDPratica.Value = GetID(Me)
            If cursor.Item IsNot Nothing Then
                nEst = cursor.Item
            Else
                nEst = New CEstinzione
            End If
            cursor.Dispose()

            nEst.Stato = ObjectStatus.OBJECT_VALID
            nEst.Persona = Me.Cliente
            nEst.Rata = Me.OffertaCorrente.Rata
            nEst.Durata = Me.OffertaCorrente.Durata
            nEst.DataInizio = Me.OffertaCorrente.DataDecorrenza
            nEst.Scadenza = DateUtils.GetLastMonthDay(DateAdd(DateInterval.Month, nEst.Durata.Value, nEst.DataInizio.Value))
            nEst.TAN = Me.OffertaCorrente.TAN
            nEst.TAEG = Me.OffertaCorrente.TAEG
            nEst.Pratica = Me
            nEst.Istituto = Me.Cessionario
            nEst.DettaglioStato = "Pratica interna N°" & Me.NumeroPratica
            nEst.SourceID = GetID(Me)
            nEst.SourceType = TypeName(Me)
            nEst.PuntoOperativo = Me.PuntoOperativo
            nEst.Estinta = False
            nEst.Numero = IIf(Me.NumeroEsterno <> "", Me.NumeroEsterno, Me.NumeroPratica)
            nEst.NomeAgenzia = Anagrafica.Aziende.AziendaPrincipale.Nominativo

            If (nEst.PuntoOperativo Is Nothing) Then
                nEst.NomeFiliale = Strings.UCase(Anagrafica.Aziende.AziendaPrincipale.Nominativo)
            Else
                nEst.NomeFiliale = Strings.UCase(Anagrafica.Aziende.AziendaPrincipale.Nominativo & " - " & nEst.PuntoOperativo.Nome)
            End If
            If (Me.Prodotto IsNot Nothing) Then
                Select Case (Me.Prodotto.IdTipoContratto)
                    Case "C"
                        If Me.Prodotto.IdTipoRapporto = "H" Then
                            nEst.Tipo = TipoEstinzione.ESTINZIONE_CQP
                        Else
                            nEst.Tipo = TipoEstinzione.ESTINZIONE_CESSIONEDELQUINTO
                        End If
                    Case "D" : nEst.Tipo = TipoEstinzione.ESTINZIONE_PRESTITODELEGA
                End Select
            End If
            nEst.Save()

            Dim estinzioni As CCollection(Of EstinzioneXEstintore) = CQSPD.Estinzioni.GetEstinzioniXEstintore(Me)
            For Each e As EstinzioneXEstintore In estinzioni
                If e.Selezionata AndAlso e.Estinzione IsNot Nothing Then
                    e.Estinzione.EstintoDa = nEst
                    e.Estinzione.Estinta = True
                    e.Estinzione.DataEstinzione = Me.StatoDiLavorazioneAttuale.Data
                    e.Estinzione.Save()
                End If
            Next
        End Sub

        ''' <summary>
        ''' Questo metodo viene richiamato quando la pratica viene eliminata oppure quando viene effettuato un passaggio
        ''' ad uno stato che è programmato per rilasciare gli altri prestiti (ES. Annullata)
        ''' </summary>
        ''' <remarks></remarks>
        Protected Overridable Sub RilasciaAltriPrestiti()
            Dim cursor As CEstinzioniCursor = Nothing
            Try
                cursor = New CEstinzioniCursor
                cursor.IgnoreRights = True
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.IDPratica.Value = GetID(Me)
                cursor.SourceID.Value = GetID(Me)
                cursor.SourceType.Value = TypeName(Me)
                If cursor.Item IsNot Nothing Then
                    cursor.Item.DettaglioStato = "Pratica Interna N°" & Me.NumeroPratica & " Annullata"
                    cursor.Item.Estinta = True
                    cursor.Item.Stato = ObjectStatus.OBJECT_DELETED
                    cursor.Item.Save()
                End If

                Dim estinzioni As CCollection(Of EstinzioneXEstintore) = CQSPD.Estinzioni.GetEstinzioniXEstintore(Me)
                For Each e As EstinzioneXEstintore In estinzioni
                    If (e.Estinzione IsNot Nothing AndAlso e.Selezionata) Then
                        e.Estinzione.EstintoDa = Nothing
                        e.Estinzione.Estinta = False
                        e.Estinzione.DataEstinzione = Nothing
                        e.Estinzione.Save()
                    End If
                Next
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Sub



        ''' <summary>
        ''' Restituisce il valore della provvigione totale
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ValoreProvvigioneTotale() As Nullable(Of Decimal)
            Get
                'If (Me.Provvigionale.ValoreTotale AndAlso Me.ValoreSpread.HasValue) Then Return Me.Provvigionale.ValoreTotale + Me.ValoreSpread.Value
                Dim ret As Decimal? = Me.m_ValoreUpFront
                If (ret.HasValue AndAlso Me.m_ValoreRappel.HasValue) Then ret = ret.Value + Me.m_ValoreRappel.Value
                Return ret
            End Get
        End Property



        ''' <summary>
        ''' Genera un evento che indica che la pratica richiede attenzione
        ''' </summary>
        ''' <param name="msg"></param>
        ''' <remarks></remarks>
        Public Sub Watch(ByVal msg As String)
            Me.Save()
            Dim e As New ItemEventArgs(Me, msg)
            Me.OnWatch(e)
        End Sub


        Protected Overridable Sub OnWatch(ByVal e As ItemEventArgs)
            RaiseEvent PraticaWatch(Me, e)
            CQSPD.Pratiche.DoOnWatch(e)
            CQSPD.Pratiche.Module.DispatchEvent(New EventDescription("watch", "Condizione di Attenzione", e))
        End Sub


        Public Function CalcolaBaseML() As Decimal?
            If (Me.OffertaCorrente Is Nothing) Then Return Nothing
            Return Me.OffertaCorrente.CalcolaBaseML(Me.Estinzioni)
        End Function

        Public Function CalcolaProvvTAN() As Double?
            If (Me.OffertaCorrente Is Nothing) Then Return Nothing
            Return Me.OffertaCorrente.CalcolaProvvTAN(Me.Estinzioni)
        End Function

        Public Function CalcolaProvvTANE() As Decimal?
            If (Me.OffertaCorrente Is Nothing) Then Return Nothing
            Return Me.OffertaCorrente.CalcolaProvvTANE(Me.Estinzioni)
        End Function

    End Class




End Class

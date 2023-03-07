Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD


    Public Enum StatoOfferta As Integer
        NON_ASSOCIATO = 0
        PROPOSTA = 1
        RIFIUTATA_CLIENTE = 2
        ACCETTATA_CLIENTE = 3
        RICHIESTA_APPROVAZIONE = 4
        APPROVATA = 5
        RIFIUTATA = 6
    End Enum

    <Flags> _
    Public Enum OffertaFlags As Integer
        NOTSET = 0

        ''' <summary>
        ''' Se vero indica che l'offerta è nascosta
        ''' </summary>
        ''' <remarks></remarks>
        HIDDEN = 1

        ''' <summary>
        ''' Indica che la provvigione memorizzata è uno sconto sulla provvigione massima
        ''' </summary>
        ''' <remarks></remarks>
        PROVVIGIONE_SCONTO = 2

        ''' <summary>
        ''' Se vero indica che si tratta di un cliente procacciato dal collaboratore
        ''' </summary>
        DirettaCollaboratore = 4
    End Enum

    '----------------------------------------------------
    Public Enum ErrorCodes As Integer
        ERROR_OK = 0              'Nessun errore
        ERROR_TEGMAX = -1         'Superato il TEG massimo
        ERROR_TAEGMAX = -2        'Superato il TAEG massimo
        ERROR_PROVVMAX = -3       'Superato il provvigionale massimo
        ERROR_INVALIDARGUMENT = -4 'Valore non valido
        ERROR_TABFINCONSTR = -5   'Vincoli della tabella finanziaria non rispettati
        ERROR_TABASSCONSTR = -6   'Vincoli della tabella assicurativa non rispettati
        ERROR_ZERO = -7           'Divisione per zero o zeri non trovati
        ERROR_GENERIC = -255      'Errore non riconosciuto
    End Enum


    <Serializable> _
    Public Class COffertaCQS
        Inherits DBObjectPO
        'Implements ISupportInitializeFrom

        Private m_OffertaLibera As Boolean

        <NonSerialized> Private m_Pratica As CRapportino
        Private m_IDPratica As Integer
        Private m_StatoOfferta As StatoOfferta

        <NonSerialized> Private m_Cliente As CPersonaFisica
        Private m_IDCliente As Integer
        Private m_NomeCliente As String

        Private m_PreventivoID As Integer '[INT] ID del preventivo a cui è associata l'offerta
        <NonSerialized> Private m_Preventivo As CPreventivo                '[CPreventivo] Oggetto preventivo a cui è associata l'offerta

        Private m_IDCessionario As Integer
        Private m_NomeCessionario As String
        <NonSerialized> Private m_Cessionario As CCQSPDCessionarioClass

        Private m_IDProfilo As Integer
        Private m_NomeProfilo As String
        <NonSerialized> Private m_Profilo As CProfilo

        <NonSerialized> Private m_Prodotto As CCQSPDProdotto '[CProdotto] Oggetto prodotto    
        Private m_ProdottoID As Integer '[INT] ID del prodotto
        Private m_NomeProdotto As String '[TEXT] Nome del prodotto

        Private m_Calculating As Boolean
        Private m_Calcolato As Boolean '[BOOL] Se falso indica che occorre ricalcolare l'offerta a causa di qualche modifica
        Private m_Durata As Integer
        Private m_Rata As Decimal
        Private m_Eta As Double '[Double] età del cliente calcolata alla decorrenza
        Private m_Anzianita As Double '[Double] anzianità lavorativa del cliente calcolata alla decorrenza

        Private m_TabellaAssicurativaRelID As Integer   '[INT] ID della relazione tra prodotto e la tripla di tabelle assicurative
        Private m_TabellaAssicurativaRel As CProdottoXTabellaAss   '[] Oggetto relazione prodotto - tripla di tabelle assicurative
        Private m_NomeTabellaAssicurativa As String '[TEXT] Nome della tripla di tabelle assicurative 

        Private m_TabellaFinanziariaRelID As Integer      '[INT] ID della relazione tra prodotto e tabella finanziaria
        Private m_TabellaFinanziariaRel As CProdottoXTabellaFin      'Oggetto relazione prodotto - tabella finanziaria
        Private m_NomeTabellaFinanziaria As String '[TEXT] Nome della tabella finanziaria

        Private m_TabellaSpeseID As Integer            '[INT] ID della tabella spese
        Private m_TabellaSpese As CProdottoXTabellaSpesa  '[CSpesa] Oggetto tabella spese

        Private m_ValoreRiduzioneProvvigionale As Decimal         'Valore della riduzione del provvigionale per estinzioni
        Private m_ValoreRappel As Double                  'Valore nascosto
        Private m_ValoreProvvigioneMassima As Double      'Massima provvigione applicabile 
        Private m_ValoreSpreadBase As Double              'Spread base sul listino
        Private m_ValoreSpread As Double                  'Valore predefinito per il profilo
        Private m_ValoreUpFront As Double                 'Percentuale sulla provvigione caricata
        Private m_ValoreRunning As Double
        Private m_Provvigionale As CProvvigionale

        Private m_DataNascita As Nullable(Of Date)
        Private m_DataAssunzione As Nullable(Of Date)
        Private m_PremioVita As Decimal  '[Double] Valore del premio vita  
        Private m_PremioImpiego As Decimal '[Double] Valore del premio impiego
        Private m_PremioCredito As Decimal '[Double] Valore del premio credito
        Private m_ImpostaSostitutiva As Decimal '[Double] Valore dell'imposta sostitutiva
        Private m_OneriErariali As Decimal '[Double] Valore degli oneri erariali
        Private m_NettoRicavo As Decimal '[Double] Valore del netto ricavo
        Private m_CommissioniBancarie As Decimal '[Double] Valore delle commissioni bancarie
        Private m_Interessi As Decimal '[Double] Valore degli interessi
        Private m_Imposte As Decimal '[Double] Valore delle imposte
        Private m_SpeseConvenzioni As Decimal '[Double] Valore delle spese per le convenzioni
        Private m_AltreSpese As Decimal '[Double] Valore di eventuali altre spese
        Private m_Rivalsa As Decimal '[Double] Valore della rivalsa
        Private m_TEG As Double    '[Double] TEG del finanziamento
        Private m_TEG_Max As Double '[Double] TEG massimo per il prodotto
        Private m_TAEG As Double '[Double] TAEG del finanziamento
        Private m_TAEG_Max As Double '[Double] TAEG massimo per il prodotto
        Private m_TAN As Double '[Double] TAN del finanziamento
        Private m_DataDecorrenza As Nullable(Of Date)
        Private m_Stampe As CCollection(Of CFsbPrev_StampaOfferta)                    '[ArrayOf COffertaStampa] Stampe
        Private m_Sesso As String
        Private m_CaricaAlMassimo As Boolean
        Private m_TipoCalcoloTEG As TEGCalcFlag
        Private m_TipoCalcoloTAEG As TEGCalcFlag

        Private m_IDSupervisore As Integer
        <NonSerialized> Private m_Supervisore As CUser
        Private m_NomeSupervisore As String
        Private m_MotivoRichiestaSconto As String
        Private m_DettaglioRichiestaSconto As String
        Private m_MotivoConfermaSconto As String
        Private m_DettaglioConfermaSconto As String
        Private m_DataConfermaSconto As Nullable(Of Date)
        Private m_IDSchermata As Integer
        Private m_Schermata As CAttachment

        Private m_ErrorCode As ErrorCodes '[INT] Codice di errore del calcolo dell'offerta
        Private m_Messages As String '[TEXT] Messaggi di errore del calcolo dell'offerta

        Private m_Flags As OffertaFlags

        Private m_LimiteRataMax As Nullable(Of Decimal)
        Private m_LimiteRataNote As String

        ' Private m_Estinzioni As CCollection(Of EstinzioneXEstintore)
        Private m_PremioDaCessionario As Decimal?               'Premio eventuale che viene corrisposto dal cessionario all'agenzia
        Private m_Attributi As CKeyCollection
        Private m_CapitaleFinanziato As Decimal?
        Private m_ProvvTAN As Decimal?
        Private m_ProvvCollab As Decimal?
        Private m_Provvigioni As CCQSPDProvvigioneXOffertaCollection

        Private m_IDCollaboratore As Integer
        Private m_Collaboratore As CCollaboratore

        Private m_IDClienteXCollaboratore As Integer
        Private m_ClienteXCollaboratore As ClienteXCollaboratore

        Public Sub New()
            Me.m_Calcolato = False
            Me.m_OffertaLibera = True
            Me.m_PreventivoID = 0
            Me.m_Preventivo = Nothing
            Me.m_Eta = 0
            Me.m_Anzianita = 0
            'm_AssicurazioneID               'DEPRECATED [INT] id della tripla di spese assicurative
            'm_Assicurazione                 'DEPRECATED [CSpese] tripla di spese assicurative
            'Pm_NomeAssicurazione             'DEPRECATED [TEXT] Nome della tripla di spese assicurative
            Me.m_Prodotto = Nothing
            Me.m_ProdottoID = 0
            Me.m_NomeProdotto = ""

            Me.m_TabellaAssicurativaRelID = 0
            Me.m_TabellaAssicurativaRel = Nothing
            Me.m_NomeTabellaAssicurativa = ""

            Me.m_TabellaFinanziariaRelID = 0
            Me.m_TabellaFinanziariaRel = Nothing
            Me.m_NomeTabellaFinanziaria = ""

            Me.m_TabellaSpeseID = 0
            Me.m_TabellaSpese = Nothing

            Me.m_PremioVita = 0
            Me.m_PremioImpiego = 0
            Me.m_PremioCredito = 0
            Me.m_ImpostaSostitutiva = 0
            Me.m_OneriErariali = 0
            Me.m_NettoRicavo = 0
            Me.m_CommissioniBancarie = 0
            Me.m_Interessi = 0
            Me.m_Imposte = 0
            Me.m_SpeseConvenzioni = 0
            Me.m_AltreSpese = 0
            Me.m_ValoreUpFront = 0
            Me.m_ValoreRunning = 0
            Me.m_ValoreSpreadBase = 0
            Me.m_ValoreSpread = 0
            Me.m_ValoreRiduzioneProvvigionale = 0.0
            Me.m_ValoreRappel = 0
            Me.m_Rivalsa = 0
            Me.m_TEG = 0
            Me.m_TabellaAssicurativaRel = Nothing
            Me.m_TEG_Max = 0
            Me.m_TAEG = 0
            Me.m_TAEG_Max = 0
            Me.m_TAN = 0
            Me.m_DataDecorrenza = Nothing
            Me.m_Stampe = Nothing
            Me.m_Sesso = ""
            Me.m_ErrorCode = 0
            Me.m_Messages = ""
            Me.m_Rata = 0
            Me.m_Durata = 0
            Me.m_DataNascita = Nothing
            Me.m_DataAssunzione = Nothing
            Me.m_Cliente = Nothing
            Me.m_IDCliente = 0
            Me.m_NomeCliente = ""
            Me.m_IDCessionario = 0
            Me.m_NomeCessionario = ""
            Me.m_Cessionario = Nothing
            Me.m_IDProfilo = 0
            Me.m_NomeProfilo = ""
            Me.m_Profilo = Nothing
            Me.m_Pratica = Nothing
            Me.m_IDPratica = 0
            Me.m_StatoOfferta = StatoOfferta.NON_ASSOCIATO
            Me.m_CaricaAlMassimo = False
            Me.m_IDSupervisore = 0
            Me.m_Supervisore = Nothing
            Me.m_NomeSupervisore = ""
            Me.m_MotivoRichiestaSconto = ""
            Me.m_DettaglioRichiestaSconto = ""
            Me.m_MotivoConfermaSconto = ""
            Me.m_DettaglioConfermaSconto = ""
            Me.m_DataConfermaSconto = Nothing
            Me.m_IDSchermata = 0
            Me.m_Schermata = Nothing
            'Me.m_TipoCalcoloTAEG = 
            Me.m_Flags = 0
            Me.m_LimiteRataMax = Nothing
            Me.m_LimiteRataNote = ""
            Me.m_Provvigionale = New CProvvigionale
            'Me.m_Estinzioni = Nothing
            Me.m_PremioDaCessionario = Nothing
            Me.m_Attributi = Nothing
            Me.m_CapitaleFinanziato = Nothing
            Me.m_ProvvTAN = Nothing
            Me.m_ProvvCollab = Nothing
            Me.m_Provvigioni = Nothing

            Me.m_IDCollaboratore = 0
            Me.m_Collaboratore = Nothing

            Me.m_IDClienteXCollaboratore = 0
            Me.m_ClienteXCollaboratore = Nothing
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
                Me.m_IDCollaboratore = GetID(value)
                Me.m_Collaboratore = value
                Me.DoChanged("Collaboratore", value, oldValue)
            End Set
        End Property

        Protected Friend Sub SetCollaboratore(ByVal value As CCollaboratore)
            Me.m_Collaboratore = value
            Me.m_IDCollaboratore = GetID(value)
        End Sub

        Public Property IDClienteXCollaboratore As Integer
            Get
                Return GetID(Me.m_ClienteXCollaboratore, Me.m_IDClienteXCollaboratore)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDClienteXCollaboratore
                If (oldValue = value) Then Return
                Me.m_IDClienteXCollaboratore = value
                Me.m_ClienteXCollaboratore = Nothing
                Me.DoChanged("IDClienteXCollaboratore", value, oldValue)
            End Set
        End Property

        Public Property ClienteXCollaboratore As ClienteXCollaboratore
            Get
                If (Me.m_ClienteXCollaboratore Is Nothing) Then Me.m_ClienteXCollaboratore = CQSPD.Collaboratori.ClientiXCollaboratori.GetItemById(Me.m_IDClienteXCollaboratore)
                Return Me.m_ClienteXCollaboratore
            End Get
            Set(value As ClienteXCollaboratore)
                Dim oldValue As ClienteXCollaboratore = Me.m_ClienteXCollaboratore
                If (oldValue Is value) Then Return
                Me.m_IDClienteXCollaboratore = GetID(value)
                Me.m_ClienteXCollaboratore = value
                Me.DoChanged("ClienteXCollaboratore", value, oldValue)
            End Set
        End Property

        Protected Friend Sub SetClienteXCollaboratore(ByVal value As ClienteXCollaboratore)
            Me.m_IDClienteXCollaboratore = GetID(value)
            Me.m_ClienteXCollaboratore = value
        End Sub


        Public ReadOnly Property Provvigioni As CCQSPDProvvigioneXOffertaCollection
            Get
                If (Me.m_Provvigioni Is Nothing) Then Me.m_Provvigioni = New CCQSPDProvvigioneXOffertaCollection(Me)
                Return Me.m_Provvigioni
            End Get
        End Property


        Public Property ValoreProvvigioneCollaboratore As Decimal?
            Get
                Return Me.m_ProvvCollab
            End Get
            Set(value As Decimal?)
                Dim oldValue As Decimal? = Me.m_ProvvCollab
                If (oldValue = value) Then Return
                Me.m_ProvvCollab = value
                Me.DoChanged("ValoreProvvigioneCollaboratore", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il valore della provvigione TAN
        ''' </summary>
        ''' <returns></returns>
        Public Property ValoreProvvTAN As Decimal?
            Get
                Return Me.m_ProvvTAN
            End Get
            Set(value As Decimal?)
                Dim oldValue As Decimal? = Me.m_ProvvTAN
                If (oldValue = value) Then Return
                Me.m_ProvvTAN = value
                Me.DoChanged("ValoreProvvTAN", value, oldValue)
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
        ''' Restituisce la collezione degli attributi
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Attributi As CKeyCollection
            Get
                If (Me.m_Attributi Is Nothing) Then Me.m_Attributi = New CKeyCollection
                Return Me.m_Attributi
            End Get
        End Property

        Public Property ValoreRiduzioneProvvigionale As Decimal
            Get
                Return Me.m_ValoreRiduzioneProvvigionale
            End Get
            Set(value As Decimal)
                Dim oldValue As Decimal = Me.m_ValoreRiduzioneProvvigionale
                If (oldValue = value) Then Exit Property
                Me.m_ValoreRiduzioneProvvigionale = value
                Me.DoChanged("ValoreRiduzioneProvvigionale", value, oldValue)
            End Set
        End Property

        Public ReadOnly Property Provvigionale As CProvvigionale
            Get
                Return Me.m_Provvigionale
            End Get
        End Property

        'Public ReadOnly Property Estinzioni As CCollection(Of EstinzioneXEstintore)
        '    Get
        '        If (Me.m_Estinzioni Is Nothing) Then Me.m_Estinzioni = CQSPD.Estinzioni.GetEstinzioniXEstintore(Me)
        '        Return Me.m_Estinzioni
        '    End Get
        'End Property

        ' ''' <summary>
        ' ''' Restituisce vero se l'offerta estingue uno qualsiasi degli altri prestiti in corso registrati
        ' ''' </summary>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Function IsEstinzione() As Boolean
        '    For Each e As EstinzioneXEstintore In Me.Estinzioni
        '        If e.Selezionata Then Return True
        '    Next
        '    Return False
        'End Function

        ' ''' <summary>
        ' ''' Restituisce vero se l'offerta estingue un altro prestito fatto con lo stesso cessionario
        ' ''' </summary>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Function IsRinnovo() As Boolean
        '    For Each e As EstinzioneXEstintore In Me.Estinzioni
        '        If e.Selezionata AndAlso (e.Estinzione IsNot Nothing) AndAlso (e.Estinzione.IDIstituto = Me.IDCessionario) Then Return True
        '    Next
        '    Return False
        'End Function


        Public Property LimiteRataNote As String
            Get
                Return Me.m_LimiteRataNote
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_LimiteRataNote
                If (oldValue = value) Then Exit Property
                Me.m_LimiteRataNote = value
                Me.DoChanged("LimiteRataNote", value, oldValue)
            End Set
        End Property

        Public Property LimiteRataMax As Nullable(Of Decimal)
            Get
                Return Me.m_LimiteRataMax
            End Get
            Set(value As Nullable(Of Decimal))
                Dim oldValue As Decimal = Me.m_LimiteRataMax
                If (oldValue = value) Then Exit Property
                Me.m_LimiteRataMax = value
                Me.DoChanged("LimiteRataMax", value, oldValue)
            End Set
        End Property



        Public Function IsScontata() As Boolean
            Return Me.ProvvigioneMassima > 0 AndAlso Math.Abs(Me.Spread - Me.ProvvigioneMassima) > 0.0001
        End Function

        Public Function RichiedeApprovazione() As Boolean
            Return Me.IsScontata 'And Me.MotivoRichiestaSconto <> "Forzato per altri requisiti"
        End Function

        Public Overrides Function GetModule() As CModule
            Return CQSPD.Offerte.Module
        End Function

        Public Property Flags As OffertaFlags
            Get
                Return Me.m_Flags
            End Get
            Set(value As OffertaFlags)
                Dim oldValue As OffertaFlags = Me.m_Flags
                If (oldValue = value) Then Return
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        Public Property Visible As Boolean
            Get
                Return TestFlag(Me.m_Flags, OffertaFlags.HIDDEN) = False
            End Get
            Set(value As Boolean)
                If (value = Me.Visible) Then Exit Property
                Me.m_Flags = SetFlag(Me.m_Flags, OffertaFlags.HIDDEN, Not value)
                Me.DoChanged("Visible", value, Not value)
            End Set
        End Property

        Public Property TipoCalcoloTAEG As TEGCalcFlag
            Get
                Return Me.m_TipoCalcoloTAEG
            End Get
            Set(value As TEGCalcFlag)
                Dim oldValue As TEGCalcFlag = Me.m_TipoCalcoloTAEG
                If (oldValue = value) Then Exit Property
                Me.m_TipoCalcoloTAEG = value
                Me.DoChanged("TipoCalcoloTAEG", value, oldValue)
            End Set
        End Property

        Public Property TipoCalcoloTEG As TEGCalcFlag
            Get
                Return Me.m_TipoCalcoloTEG
            End Get
            Set(value As TEGCalcFlag)
                Dim oldValue As TEGCalcFlag = Me.m_TipoCalcoloTEG
                If (oldValue = value) Then Exit Property
                Me.m_TipoCalcoloTEG = value
                Me.DoChanged("TipoCalcoloTEG", value, oldValue)
            End Set
        End Property

        Public Property OffertaLibera As Boolean
            Get
                Return Me.m_OffertaLibera
            End Get
            Set(value As Boolean)
                If (value = Me.m_OffertaLibera) Then Exit Property
                Me.m_OffertaLibera = value
                Me.Invalidate()
                Me.DoChanged("OffertaLibera", value, Not value)
            End Set
        End Property

        Public Property CaricaAlMassimo As Boolean
            Get
                Return Me.m_CaricaAlMassimo
            End Get
            Set(value As Boolean)
                If (Me.m_CaricaAlMassimo = value) Then Exit Property
                Me.m_CaricaAlMassimo = value
                Me.Invalidate()
                Me.DoChanged("CaricaAlMassimo", value, Not value)
            End Set
        End Property

        Public Property IDCessionario As Integer
            Get
                Return GetID(Me.m_Cessionario, Me.m_IDCessionario)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDCessionario
                If oldValue = value Then Exit Property
                Me.m_IDCessionario = value
                Me.m_Cessionario = Nothing
                Me.Invalidate()
                Me.DoChanged("IDCessionario", value, oldValue)
            End Set
        End Property

        Public Property Cessionario As CCQSPDCessionarioClass
            Get
                If Me.m_Cessionario Is Nothing Then Me.m_Cessionario = CQSPD.Cessionari.GetItemById(Me.m_IDCessionario)
                Return Me.m_Cessionario
            End Get
            Set(value As CCQSPDCessionarioClass)
                Dim oldValue As CCQSPDCessionarioClass = Me.Cessionario
                If (oldValue = value) Then Exit Property
                Me.m_Cessionario = value
                Me.m_IDCessionario = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeCessionario = value.Nome
                Me.Invalidate()
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
                Return GetID(Me.m_Profilo, Me.m_IDProfilo)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDProfilo
                If oldValue = value Then Exit Property
                Me.m_IDProfilo = value
                Me.m_Profilo = Nothing
                Me.Invalidate()
                Me.DoChanged("IDProfilo", value, oldValue)
            End Set
        End Property

        Public Property Profilo As CProfilo
            Get
                If Me.m_Profilo Is Nothing Then Me.m_Profilo = CQSPD.Profili.GetItemById(Me.m_IDProfilo)
                Return Me.m_Profilo
            End Get
            Set(value As CProfilo)
                Dim oldValue As CProfilo = Me.Profilo
                If (oldValue = value) Then Exit Property
                Me.m_Profilo = value
                Me.m_IDProfilo = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeProfilo = value.ProfiloVisibile
                Me.Invalidate()
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

        Public Property IDCliente As Integer
            Get
                Return GetID(Me.m_Cliente, Me.m_IDCliente)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDCliente
                If oldValue = value Then Exit Property
                Me.m_IDCliente = value
                Me.m_Cliente = Nothing
                Me.DoChanged("IDCliente", value, oldValue)
            End Set
        End Property

        Public Property Cliente As CPersonaFisica
            Get
                If Me.m_Cliente Is Nothing Then Me.m_Cliente = Anagrafica.Persone.GetItemById(Me.m_IDCliente)
                Return Me.m_Cliente
            End Get
            Set(value As CPersonaFisica)
                Dim oldValue As CPersonaFisica = Me.Cliente
                If (oldValue = value) Then Exit Property
                Me.m_Cliente = value
                Me.m_IDCliente = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeCliente = value.Nominativo
                Me.DoChanged("Cliente", value, oldValue)
            End Set
        End Property

        Protected Friend Overridable Sub SetCliente(ByVal value As CPersonaFisica)
            Me.m_Cliente = value
            Me.m_IDCliente = GetID(value)
        End Sub

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

        ''' <summary>
        ''' Restituisce o imposta l'ID della relazione tra il prodotto e la tabella finanziaria utilizzata
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TabellaFinanziariaRelID As Integer
            Get
                Return GetID(Me.m_TabellaFinanziariaRel, Me.m_TabellaFinanziariaRelID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.TabellaFinanziariaRelID
                If oldValue = value Then Exit Property
                Me.m_TabellaFinanziariaRel = Nothing
                Me.m_TabellaFinanziariaRelID = value
                Me.Invalidate()
                Me.DoChanged("TabellaFinanziariaRelID", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la relazione tra il prodotto e la tabella finanziaria utilizzata
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TabellaFinanziariaRel As CProdottoXTabellaFin
            Get
                If (Me.m_TabellaFinanziariaRel Is Nothing AndAlso Me.Prodotto IsNot Nothing) Then Me.m_TabellaFinanziariaRel = Me.Prodotto.TabelleFinanziarieRelations.GetItemById(Me.m_TabellaFinanziariaRelID)
                If (Me.m_TabellaFinanziariaRel Is Nothing) Then Me.m_TabellaFinanziariaRel = CQSPD.TabelleFinanziarie.GetTabellaXProdottoByID(Me.m_TabellaFinanziariaRelID)
                Return Me.m_TabellaFinanziariaRel
            End Get
            Set(value As CProdottoXTabellaFin)
                Dim oldValue As CProdottoXTabellaFin = Me.TabellaFinanziariaRel
                If (oldValue = value) Then Exit Property
                Me.m_TabellaFinanziariaRel = value
                Me.m_TabellaFinanziariaRelID = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeTabellaFinanziaria = value.Tabella.Nome
                Me.Invalidate()
                Me.DoChanged("TabellaFinanziariaRel", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome della relazione tra il prodotto ed la tabella finanziaria utilizzata
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeTabellaFinanziaria As String
            Get
                Return Me.m_NomeTabellaFinanziaria
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomeTabellaFinanziaria
                Me.m_NomeTabellaFinanziaria = value
                Me.DoChanged("NomeTabellaFinanziaria", value, oldValue)
            End Set
        End Property


        ''' <summary>
        ''' Restituisce o imposta l'ID della relazione tra il prodotto e la tripla di tabelle assicurative utilizzate
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TabellaAssicurativaRelID As Integer
            Get
                Return GetID(Me.m_TabellaAssicurativaRel, Me.m_TabellaAssicurativaRelID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.TabellaAssicurativaRelID
                If oldValue = value Then Exit Property
                Me.m_TabellaAssicurativaRel = Nothing
                Me.m_TabellaAssicurativaRelID = value
                Me.Invalidate()
                Me.DoChanged("TabellaAssicurativaRelID", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la relazione tra il prodotto e la tripla di tabelle assicurative utilizzate
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TabellaAssicurativaRel As CProdottoXTabellaAss
            Get
                If (Me.m_TabellaAssicurativaRel Is Nothing AndAlso Me.Prodotto IsNot Nothing) Then Me.m_TabellaAssicurativaRel = Me.Prodotto.TabelleAssicurativeRelations.GetItemById(Me.m_TabellaAssicurativaRelID)
                If (Me.m_TabellaAssicurativaRel Is Nothing) Then Me.m_TabellaAssicurativaRel = CQSPD.TabelleAssicurative.GetTabellaXProdottoByID(Me.m_TabellaAssicurativaRelID)
                Return Me.m_TabellaAssicurativaRel
            End Get
            Set(value As CProdottoXTabellaAss)
                Dim oldValue As CProdottoXTabellaAss = Me.TabellaAssicurativaRel
                If (oldValue = value) Then Exit Property
                Me.m_TabellaAssicurativaRel = value
                Me.m_TabellaAssicurativaRelID = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeTabellaAssicurativa = value.Descrizione
                Me.Invalidate()
                Me.DoChanged("TabellaAssicurativaRel", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome della relazione tra il prodotto e la tripla di tabelle assicurative utilizzate
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeTabellaAssicurativa As String
            Get
                Return Me.m_NomeTabellaAssicurativa
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomeTabellaAssicurativa
                If (oldValue = value) Then Exit Property
                Me.m_NomeTabellaAssicurativa = value
                Me.DoChanged("NomeTabellaAssicurativa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID della tabella spese
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TabellaSpeseID As Integer
            Get
                Return GetID(Me.m_TabellaSpese, Me.m_TabellaSpeseID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.TabellaSpeseID
                If oldValue = value Then Exit Property
                Me.m_TabellaSpese = Nothing
                Me.m_TabellaSpeseID = value
                Me.Invalidate()
                Me.DoChanged("TabellaSpeseID", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la tabella spese
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TabellaSpese As CProdottoXTabellaSpesa
            Get
                If (Me.m_TabellaSpese Is Nothing AndAlso Me.Prodotto IsNot Nothing) Then Me.m_TabellaSpese = Me.Prodotto.TabelleSpese.GetItemById(Me.m_TabellaSpeseID)
                Return Me.m_TabellaSpese
            End Get
            Set(value As CProdottoXTabellaSpesa)
                Dim oldValue As CProdottoXTabellaSpesa = Me.TabellaSpese
                If (oldValue = value) Then Exit Property
                Me.m_TabellaSpese = value
                Me.m_TabellaSpeseID = GetID(value)
                Me.Invalidate()
                Me.DoChanged("TabellaSpese", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce la data di nascita del cliente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataNascita As Nullable(Of Date)
            Get
                Return Me.m_DataNascita
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataNascita
                If (oldValue = value) Then Exit Property
                Me.m_DataNascita = value
                Me.Invalidate()
                Me.DoChanged("DataNascita", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce la data di assunzione del cliente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataAssunzione As Nullable(Of Date)
            Get
                Return Me.m_DataAssunzione
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataAssunzione
                If (oldValue = value) Then Exit Property
                Me.m_DataAssunzione = value
                Me.Invalidate()
                Me.DoChanged("DataAssunzione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce la data di decorrenza dell'offerta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataDecorrenza As Nullable(Of Date)
            Get
                Return Me.m_DataDecorrenza
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataDecorrenza
                If (oldValue = value) Then Exit Property
                Me.m_DataDecorrenza = value
                Me.Invalidate()
                Me.DoChanged("DataDecorrenza", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce il sesso del cliente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Sesso As String
            Get
                Return Me.m_Sesso
            End Get
            Set(value As String)
                value = UCase(Left(Trim(value), 1))
                Dim oldValue As String = Me.m_Sesso
                If (oldValue = value) Then Exit Property
                Me.m_Sesso = value
                Me.Invalidate()
                Me.DoChanged("Sesso", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce la rivalsa
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Rivalsa As Decimal
            Get
                'Me.Validate()
                Return Me.m_Rivalsa
            End Get
            Set(value As Decimal)
                Dim oldValue As Decimal = Me.m_Rivalsa
                If (oldValue = value) Then Exit Property
                Me.m_Rivalsa = value
                Me.Invalidate()
                Me.DoChanged("Rivalsa", value, oldValue)
            End Set
        End Property

        'Public Sub Validate()
        '    If Me.m_Calcolato Then Exit Sub
        '    Me.Calcola()
        'End Sub

        ''' <summary>
        ''' Restituisce il valore dei punti sotto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Rappel As Double
            Get
                If Me.MontanteLordo > 0 Then
                    Return (Me.m_ValoreRappel / Me.MontanteLordo) * 100
                Else
                    Return 0
                End If
            End Get
            Set(value As Double)
                If (value < 0) Or (value > 100) Then Throw New ArgumentOutOfRangeException("Rappel")
                Dim oldValue As Double = Me.Rappel
                If (oldValue = value) Then Exit Property
                Me.m_ValoreRappel = value * Me.MontanteLordo / 100
                Me.DoChanged("Rappel", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce il valore dei punti sotto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ValoreRappel As Decimal
            Get
                ' Me.Validate()
                Return Me.m_ValoreRappel
            End Get
            Set(value As Decimal)
                If (value < 0) Then Throw New ArgumentOutOfRangeException("ValoreRappel")
                Dim oldValue As Decimal = Me.m_ValoreRappel
                If (oldValue = value) Then Exit Property
                Me.m_ValoreRappel = value
                Me.Invalidate()
                Me.DoChanged("ValoreRappel", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce la provvigione percentuale sul montante lordo aggiunta automaticamente allo spread
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SpreadBase As Double
            Get
                If (Me.MontanteLordo > 0) Then
                    Return (Me.m_ValoreSpreadBase / Me.MontanteLordo) * 100
                Else
                    Return 0
                End If
            End Get
            Set(value As Double)
                If (value < 0) Or (value > 100) Then Throw New ArgumentOutOfRangeException("SpreadBase")
                Dim oldValue As Double = Me.SpreadBase
                If (oldValue = value) Then Exit Property
                Me.m_ValoreSpreadBase = value * Me.MontanteLordo / 100
                Me.Invalidate()
                Me.DoChanged("SpreadBase", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce la parte (valore) del montante lordo che va al mediatore
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ValoreSpreadBase As Decimal
            Get
                Return Me.m_ValoreSpreadBase
            End Get
            Set(value As Decimal)
                If (value < 0) Then Throw New ArgumentOutOfRangeException("ValoreSpreadBase")
                Dim oldValue As Decimal = Me.m_ValoreSpreadBase
                If (oldValue = value) Then Exit Property
                Me.m_ValoreSpreadBase = value
                Me.Invalidate()
                Me.DoChanged("ValoreSpreadBase", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce la provvigione percentaule sul montante lordo destinata all'agenzia
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Spread As Double
            Get
                If (Me.MontanteLordo > 0) Then
                    Return (Me.m_ValoreSpread / Me.MontanteLordo) * 100
                Else
                    Return 0
                End If
            End Get
            Set(value As Double)
                If (value < 0) Or (value > 100) Then Throw New ArgumentOutOfRangeException("Spread")
                Dim oldValue As Double = Me.Spread
                If (oldValue = value) Then Exit Property
                Me.m_ValoreSpread = value * Me.MontanteLordo / 100
                Me.Invalidate()
                Me.DoChanged("Spread", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce il valore della commissione agenzia
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ValoreSpread As Decimal
            Get
                Return Me.m_ValoreSpread
            End Get
            Set(value As Decimal)
                If (value < 0) Then Throw New ArgumentOutOfRangeException("ValoreSpread")
                Dim oldValue As Double = Me.ValoreSpread
                If (oldValue = value) Then Exit Property
                Me.m_ValoreSpread = value
                Me.Invalidate()
                Me.DoChanged("ValoreSpread", value, oldValue)
            End Set
        End Property



        ''' <summary>
        ''' Somma dei punti base e del rappel
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property GuadagnoAziendale As Decimal
            Get
                Return Me.ValoreRappel + Me.ValoreSpreadBase
            End Get
        End Property

        ''' <summary>
        ''' Somma dei punti base e del rappel e delle provvigioni
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property GuadagnoTotale As Decimal
            Get
                Return Me.GuadagnoAziendale
            End Get
        End Property

        ''' <summary>
        ''' Nominativo del cliente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Nominativo As String
            Get
                Return Me.m_NomeCliente
            End Get
        End Property

        ' ''' <summary>
        ' ''' Percentuale sul montante lordo che va al mediatore
        ' ''' </summary>
        ' ''' <value></value>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Property Provvigioni As Double
        '    Get
        '        If (Me.MontanteLordo > 0) Then
        '            Return (Me.m_ValoreProvvigioni / Me.MontanteLordo) * 100
        '        Else
        '            Return 0
        '        End If
        '    End Get
        '    Set(value As Double)
        '        If (value < 0) Or (value > 100) Then Throw New ArgumentOutOfRangeException("Provvigioni")
        '        Dim oldValue As Double = Me.Provvigioni
        '        If oldValue = value Then Exit Property
        '        Me.m_ValoreProvvigioni = value * Me.MontanteLordo / 100
        '        Me.Invalidate()
        '        Me.DoChanged("Provvigioni", value, oldValue)
        '    End Set
        'End Property

        ' ''' <summary>
        ' ''' Parte del montante lordo (valore) che va al mediatore
        ' ''' </summary>
        ' ''' <value></value>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Property ValoreProvvigioni As Decimal
        '    Get
        '        Return Me.m_ValoreProvvigioni
        '    End Get
        '    Set(value As Decimal)
        '        If (value < 0) Then Throw New ArgumentOutOfRangeException("ValoreProvvigioni")
        '        Dim oldValue As Decimal = Me.m_ValoreProvvigioni
        '        If (oldValue = value) Then Exit Property
        '        Me.m_ValoreProvvigioni = value
        '        Me.Invalidate()
        '        Me.DoChanged("ValoreProvvigioni", value, oldValue)
        '    End Set
        'End Property

        Public Property Running As Double
            Get
                If (Me.MontanteLordo > 0) Then
                    Return (Me.m_ValoreRunning / Me.MontanteLordo) * 100
                Else
                    Return 0
                End If
            End Get
            Set(value As Double)
                If (value < 0) Or (value > 100) Then Throw New ArgumentOutOfRangeException("Running")
                Dim oldValue As Double = Me.Running
                If (oldValue = value) Then Exit Property
                Me.m_ValoreRunning = value * Me.MontanteLordo / 100
                Me.DoChanged("Running", value, oldValue)
            End Set
        End Property

        Public Property ValoreRunning As Decimal
            Get
                Return Me.m_ValoreRunning
            End Get
            Set(value As Decimal)
                If (value < 0) Then Throw New ArgumentOutOfRangeException("ValoreRunning")
                Dim oldValue As Decimal = Me.m_ValoreRunning
                If (oldValue = value) Then Exit Property
                Me.m_ValoreRunning = value
                Me.DoChanged("ValoreRunning", value, oldValue)
            End Set
        End Property

        Public Property UpFront As Double
            Get
                If Me.MontanteLordo > 0 Then
                    Return (Me.m_ValoreUpFront / Me.MontanteLordo) * 100
                Else
                    Return 0
                End If
            End Get
            Set(value As Double)
                If (value < 0) Or (value > 100) Then Throw New ArgumentOutOfRangeException("UpFront")
                Dim oldValue As Double = Me.UpFront
                If (oldValue = value) Then Exit Property
                Me.m_ValoreUpFront = value * Me.MontanteLordo / 100
                Me.DoChanged("UpFront", value, oldValue)
            End Set
        End Property

        Public Property ValoreUpFront As Decimal
            Get
                Return Me.m_ValoreUpFront
            End Get
            Set(value As Decimal)
                If (value < 0) Then Throw New ArgumentOutOfRangeException("ValoreUpFront")
                Dim oldValue As Decimal = Me.m_ValoreUpFront
                If (oldValue = value) Then Exit Property
                Me.m_ValoreUpFront = value
                Me.DoChanged("ValoreUpFront", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Percentuale sul montante lordo che va al mediatore
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ProvvigioneMassima As Double
            Get
                If Me.MontanteLordo > 0 Then
                    Return (Me.m_ValoreProvvigioneMassima / Me.MontanteLordo) * 100
                Else
                    Return 0
                End If
            End Get
            Set(value As Double)
                If (value < 0) Or (value > 100) Then Throw New ArgumentOutOfRangeException("ProvvigioneMassima")
                Dim oldValue As Double = Me.ProvvigioneMassima
                If (oldValue = value) Then Exit Property
                Me.m_ValoreProvvigioneMassima = value * Me.MontanteLordo / 100
                Me.DoChanged("ProvvigioneMassima", value, oldValue)
            End Set
        End Property

        Public Property ValoreProvvigioneMassima As Decimal
            Get
                Return Me.m_ValoreProvvigioneMassima
            End Get
            Set(value As Decimal)
                If (value < 0) Then Throw New ArgumentOutOfRangeException("ValoreProvvigioneMassima")
                Dim oldValue As Double = Me.m_ValoreProvvigioneMassima
                If (oldValue = value) Then Exit Property
                Me.m_ValoreProvvigioneMassima = value
                Me.DoChanged("ValoreProvvigioneMassima", value, oldValue)
            End Set
        End Property


        ' ''' <summary>
        ' ''' Restituisce la somma tra spread e provvigioni al produttore
        ' ''' </summary>
        ' ''' <value></value>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public ReadOnly Property ProvvigioneTotale As Double
        '    Get
        '        Return Me.Provvigioni + Me.SpreadBase
        '    End Get
        'End Property

        ''' <summary>
        ''' Restituisce la somma tra spread e provvigioni al produttore
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ValoreProvvigioneTotale As Decimal
            Get
                Return Me.Provvigionale.ValoreTotale + Me.ValoreSpread
            End Get
        End Property



        ''' <summary>
        ''' Restiuisce il montante lordo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MontanteLordo As Decimal
            Get
                Return Me.m_Rata * Me.m_Durata
            End Get
            Set(value As Decimal)
                Dim oldValue As Decimal = Me.MontanteLordo
                If (oldValue = value) Then Exit Property
                If (Me.m_Durata > 0) Then
                    Me.m_Rata = value / Me.m_Durata
                ElseIf Me.m_Rata > 0 Then
                    Me.m_Durata = Fix(value / Me.m_Rata)
                Else
                    Throw New DivideByZeroException
                End If
                Me.Invalidate()
                Me.DoChanged("MontanteLordo", value, oldValue)
            End Set
        End Property

        'Public ReadOnly Property CapitaleFinanziato As Decimal
        '    Get
        '        Return Me.MontanteLordo - Me.Interessi
        '    End Get
        'End Property

        ''' <summary>
        ''' Restituisce la durata in mesi
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Durata As Integer
            Get
                Return Me.m_Durata
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_Durata
                If (oldValue = value) Then Exit Property
                Me.m_Durata = value
                Me.Invalidate()
                Me.DoChanged("Durata", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce l'ID del prodotto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ProdottoID As Integer
            Get
                Return GetID(Me.m_Prodotto, Me.m_ProdottoID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.ProdottoID
                If (oldValue = value) Then Exit Property
                Me.m_ProdottoID = value
                Me.m_Prodotto = Nothing
                Me.Invalidate()
                Me.DoChanged("ProdottoID", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce l'oggetto prodotto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Prodotto As CCQSPDProdotto
            Get
                If (Me.m_Prodotto Is Nothing) Then Me.m_Prodotto = CQSPD.Prodotti.GetItemById(Me.m_ProdottoID)
                Return Me.m_Prodotto
            End Get
            Set(value As CCQSPDProdotto)
                Dim oldValue As CCQSPDProdotto = Me.Prodotto
                If (oldValue = value) Then Exit Property
                Me.m_Prodotto = value
                Me.m_ProdottoID = GetID(value)
                If Not (value Is Nothing) Then
                    Me.m_TipoCalcoloTAEG = value.TipoCalcoloTAEG
                    Me.m_TipoCalcoloTEG = value.TipoCalcoloTeg
                    Me.m_NomeProdotto = value.Nome
                End If

                Me.Invalidate()
                Me.DoChanged("Prodotto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del prodotto utilizzato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
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

        ''' <summary>
        ''' Restituisce il valore dell'imposta sostitutiva
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ImpostaSostitutiva As Decimal
            Get
                'Me.Validate()
                Return Me.m_ImpostaSostitutiva
            End Get
            Set(value As Decimal)
                Dim oldValue As Decimal = Me.m_ImpostaSostitutiva
                If (oldValue = value) Then Exit Property
                Me.m_ImpostaSostitutiva = value
                Me.Invalidate()
                Me.DoChanged("ImpostaSostitutiva", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce il valore di eventuali altre spese
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AltreSpese As Decimal
            Get
                ' Me.Validate()
                Return Me.m_AltreSpese
            End Get
            Set(value As Decimal)
                Dim oldValue As Decimal = Me.m_AltreSpese
                If (oldValue = value) Then Exit Property
                Me.m_AltreSpese = value
                Me.Invalidate()
                Me.DoChanged("AltreSpese", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce il valore delle imposte
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Imposte As Decimal
            Get
                'Me.Validate()
                Return Me.m_Imposte
            End Get
            Set(value As Decimal)
                Dim oldValue As Decimal = Me.m_Imposte
                If (oldValue = value) Then Exit Property
                Me.m_Imposte = value
                Me.Invalidate()
                Me.DoChanged("Imposte", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce il valore delle spese per eventuali convenzioni
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SpeseConvenzioni As Decimal
            Get
                ' Me.Validate()
                Return Me.m_SpeseConvenzioni
            End Get
            Set(value As Decimal)
                Dim oldValue As Decimal = Me.m_SpeseConvenzioni
                If (oldValue = value) Then Exit Property
                Me.m_SpeseConvenzioni = value
                Me.Invalidate()
                Me.DoChanged("SpeseConvenzioni", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce l'età calcolata secondo le approssimazioni relative all'assicurazione sulla vita
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Eta As Double
            Get
                Return Me.m_Eta
            End Get
            Set(value As Double)
                Dim oldValue As Decimal = Me.m_Eta
                If oldValue = value Then Exit Property
                Me.m_Eta = value
                Me.Invalidate()
                Me.DoChanged("Eta", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce l'età reale all'inizio del finanziamento ovvero la differenza tra la data di decorrenza e la data di nascita
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property EtaIF As Double
            Get
                If (Me.DataNascita.HasValue AndAlso Me.DataDecorrenza.HasValue) Then
                    Return DateDiff(Microsoft.VisualBasic.DateInterval.Year, Me.DataNascita.Value, Me.DataDecorrenza.Value)
                Else
                    Return 0
                End If
            End Get
        End Property

        ''' <summary>
        ''' Restituisce l'età reale alla fine del finanziamento ovvero data di decorrenza + durata/12 - data di nascita
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property EtaFF As Double
            Get
                Return Me.EtaIF + Me.Durata / 12
            End Get
        End Property

        ''' <summary>
        ''' Restituisce l'anzianità di servizio calcolata secondo gli arrotondamenti stabiliti dall'assicurazione  rischio impiego o rischio credito	
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Anzianita As Double
            Get
                Return Me.m_Anzianita
            End Get
            Set(value As Double)
                Dim oldValue As Decimal = Me.m_Anzianita
                If (oldValue = value) Then Exit Property
                If oldValue = value Then Exit Property
                Me.m_Anzianita = value
                Me.Invalidate()
                Me.DoChanged("Anzianita", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce l'anzianità reale all'inizio del finanziamento ovvero la differenza tra la data di decorrenza e la data di assunzione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property AnzianitaIF As Double
            Get
                If (Me.DataAssunzione.HasValue AndAlso Me.DataDecorrenza.HasValue) Then
                    Return DateDiff(Microsoft.VisualBasic.DateInterval.Year, Me.DataAssunzione.Value, Me.DataDecorrenza.Value)
                Else
                    Return 0
                End If
            End Get
        End Property

        ''' <summary>
        ''' Restituisce l'anzianità reale alla fine del finanziamento ovvero data di decorrenza + durata/12 - data di assunzione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property AnzianitaFF As Double
            Get
                Return Me.AnzianitaIF + Me.Durata / 12
            End Get
        End Property

        ''' <summary>
        ''' Valore degli interessi sul montante lordo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Interessi As Decimal
            Get
                Me.Validate()
                Return Me.m_Interessi
            End Get
            Set(value As Decimal)
                Dim oldValue As Decimal = Me.m_Interessi
                If (oldValue = value) Then Exit Property
                Me.m_Interessi = value
                Me.Invalidate()
                Me.DoChanged("Interessi", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Valore delle commissioni bancarie
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CommissioniBancarie As Decimal
            Get
                Me.Validate()
                Return Me.m_CommissioniBancarie
            End Get
            Set(value As Decimal)
                Dim oldValue As Decimal = Me.m_CommissioniBancarie
                If (oldValue = value) Then Exit Property
                Me.m_CommissioniBancarie = value
                Me.Invalidate()
                Me.DoChanged("CommissioniBancarie", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Valore degli oneri erariali
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property OneriErariali As Decimal
            Get
                Me.Validate()
                Return Me.m_OneriErariali
            End Get
            Set(value As Decimal)
                Dim oldValue As Decimal = Me.m_OneriErariali
                If (oldValue = value) Then Exit Property
                Me.m_OneriErariali = value
                Me.Invalidate()
                Me.DoChanged("OneriErariali", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Valore delle spese assicurative (vita + impiego + credito)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property SpeseAssicurative As Decimal
            Get
                Return Me.PremioVita + Me.PremioImpiego + Me.PremioCredito
            End Get
        End Property

        ''' <summary>
        ''' Valore del premio vita
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PremioVita As Decimal
            Get
                Me.Validate()
                Return Me.m_PremioVita
            End Get
            Set(value As Decimal)
                Dim oldValue As Decimal = Me.m_PremioVita
                If (oldValue = value) Then Exit Property
                Me.m_PremioVita = value
                Me.Invalidate()
                Me.DoChanged("PremioVita", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Valore del premio impiego
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PremioImpiego As Decimal
            Get
                Me.Validate()
                Return Me.m_PremioImpiego
            End Get
            Set(value As Decimal)
                Dim oldValue As Decimal = Me.m_PremioImpiego
                If (oldValue = value) Then Exit Property
                Me.m_PremioImpiego = value
                Me.Invalidate()
                Me.DoChanged("PremioImpiego", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Valore del premio credito
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PremioCredito As Decimal
            Get
                Me.Validate()
                Return Me.m_PremioCredito
            End Get
            Set(value As Decimal)
                Dim oldValue As Decimal = Me.m_PremioCredito
                If (oldValue = value) Then Exit Property
                Me.m_PremioCredito = value
                Me.Invalidate()
                Me.DoChanged("PremioCredito", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Valore del netto ricavo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NettoRicavo As Decimal
            Get
                Me.Validate()
                Return Me.m_NettoRicavo
            End Get
            Set(value As Decimal)
                Dim oldValue As Decimal = Me.m_NettoRicavo
                If (oldValue = value) Then Exit Property
                Me.m_NettoRicavo = value
                Me.Invalidate()
                Me.DoChanged("NettoRicavo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Valore del TEG
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TEG As Double
            Get
                Me.Validate()
                Return Me.m_TEG
            End Get
            Set(value As Double)
                Dim oldValue As Double = Me.m_TEG
                If (oldValue = value) Then Exit Property
                Me.m_TEG = value
                Me.Invalidate()
                Me.DoChanged("TEG", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' TEG Massimo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TEG_Max As Double
            Get
                'Me.Validate()
                Return Me.m_TEG_Max
            End Get
            Set(value As Double)
                Dim oldValue As Double = Me.m_TEG_Max
                If (oldValue = value) Then Exit Property
                Me.m_TEG_Max = value
                Me.Invalidate()
                Me.DoChanged("TEG_Max", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Valore del TAEG
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TAEG As Double
            Get
                Me.Validate()
                Return Me.m_TAEG
            End Get
            Set(value As Double)
                Dim oldValue As Double = Me.m_TAEG
                If (oldValue = value) Then Exit Property
                Me.m_TAEG = value
                Me.Invalidate()
                Me.DoChanged("TAEG", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' TAEG Massimo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TAEG_Max As Double
            Get
                'Me.Validate()
                Return Me.m_TAEG_Max
            End Get
            Set(value As Double)
                Dim oldValue As Double = Me.m_TAEG_Max
                If (oldValue = value) Then Exit Property
                Me.m_TAEG_Max = value
                Me.Invalidate()
                Me.DoChanged("TAEG_Max", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce il TAN
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TAN As Double
            Get
                Me.Validate()
                Return Me.m_TAN
            End Get
            Set(value As Double)
                Dim oldValue As Double = Me.m_TAN
                If (oldValue = value) Then Exit Property
                Me.m_TAN = value
                Me.Invalidate()
                Me.DoChanged("TAN", value, oldValue)
            End Set
        End Property

        Public Property IDPratica As Integer
            Get
                Return GetID(Me.m_Pratica, Me.m_IDPratica)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDPratica
                If oldValue = value Then Exit Property
                Me.m_IDPratica = value
                Me.m_Pratica = Nothing
                Me.DoChanged("IDPratica", value, oldValue)
            End Set
        End Property

        Public Property Pratica As CRapportino
            Get
                If Me.m_Pratica Is Nothing Then Me.m_Pratica = CQSPD.Pratiche.GetItemById(Me.m_IDPratica)
                Return Me.m_Pratica
            End Get
            Set(value As CRapportino)
                Dim oldValue As CRapportino = Me.Pratica
                If (oldValue = value) Then Exit Property
                Me.m_Pratica = value
                Me.m_IDPratica = GetID(value)
                Me.DoChanged("Pratica", value, oldValue)
            End Set
        End Property


        Public Property StatoOfferta As StatoOfferta
            Get
                Return Me.m_StatoOfferta
            End Get
            Set(value As StatoOfferta)
                Dim oldValue As StatoOfferta = Me.m_StatoOfferta
                If (oldValue = value) Then Exit Property
                Me.m_StatoOfferta = value
                Me.DoChanged("StatoOfferta", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' ID del preventivo a cui appartiene l'offerta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PreventivoID As Integer
            Get
                Return GetID(Me.m_Preventivo, Me.m_PreventivoID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.PreventivoID
                If oldValue = value Then Exit Property
                Me.m_Preventivo = Nothing
                Me.m_PreventivoID = value
                'Me.Invalidate()
                Me.DoChanged("PreventivoID", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Oggetto preventivo a cui appartiene l'offerta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Preventivo As CPreventivo
            Get
                If (Me.m_Preventivo Is Nothing) Then Me.m_Preventivo = CQSPD.Preventivi.GetItemById(Me.m_PreventivoID)
                Return Me.m_Preventivo
            End Get
            Set(value As CPreventivo)
                Dim oldValue As CPreventivo = Me.m_Preventivo
                If (oldValue Is value) Then Exit Property
                Me.m_Preventivo = value
                Me.m_PreventivoID = GetID(value)
                Me.DoChanged("Preventivo", value, oldValue)
            End Set
        End Property

        Protected Friend Overridable Sub SetPreventivo(ByVal value As CPreventivo)
            Me.m_Preventivo = value
            Me.m_PreventivoID = GetID(value)
        End Sub

        ''' <summary>
        ''' Restituisce il TEG Massimo impostato per il prodotto
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetMaxTEG() As Double
            Return Me.Prodotto.GetMaxTEG(Me)
        End Function

        ''' <summary>
        ''' Restituisce il TAEG Massimo impostato per il prodotto
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetMaxTAEG() As Double
            Return Me.Prodotto.GetMaxTAEG(Me)
        End Function

        ''' <summary>
        ''' Somma di tutte le spese (interessi + commissioni + premi assicurativi + oneri + imposte + altre)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property SommaDelleSpese As Decimal
            Get
                Return Me.SpeseConvenzioni + Me.AltreSpese + Me.Interessi + Me.CommissioniBancarie + Me.Provvigionale.ValoreTotale + Me.SpeseAssicurative + Me.OneriErariali + Me.Imposte + Me.ImpostaSostitutiva
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'utente che ha supervisionato l'offerta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDSupervisore As Integer
            Get
                Return GetID(Me.m_Supervisore, Me.m_IDSupervisore)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDSupervisore
                If (oldValue = value) Then Exit Property
                Me.m_IDSupervisore = value
                Me.m_Supervisore = Nothing
                Me.DoChanged("IDSupervisore", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'utente che ha supervisionato l'offerta
        ''' </summary>
        ''' <remarks></remarks>
        Public Property Supervisore As CUser
            Get
                If (Me.m_Supervisore Is Nothing) Then Me.m_Supervisore = Users.GetItemById(Me.m_IDSupervisore)
                Return Me.m_Supervisore
            End Get
            Set(value As CUser)
                Me.m_Supervisore = value
                Me.m_IDSupervisore = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeSupervisore = value.Nominativo
                Me.DoChanged("Supervisore", value)
            End Set
        End Property

        Public Property NomeSupervisore As String
            Get
                Return Me.m_NomeSupervisore
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomeSupervisore
                If (oldValue = value) Then Exit Property
                Me.m_NomeSupervisore = value
                Me.DoChanged("NomeSupervisore", value, oldValue)
            End Set
        End Property


        ''' <summary>
        ''' Restituisce o imposta una stringa che descrive "in sintesi" il motivo della richiesta dello sconto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MotivoRichiestaSconto As String
            Get
                Return Me.m_MotivoRichiestaSconto
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_MotivoRichiestaSconto
                If (oldValue = value) Then Exit Property
                Me.m_MotivoRichiestaSconto = value
                Me.DoChanged("MotivoRichiestaSconto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta una stringa che specifica per esteso il motivo della richiesta dello sconto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DettaglioRichiestaSconto As String
            Get
                Return Me.m_DettaglioRichiestaSconto
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_DettaglioRichiestaSconto
                If (oldValue = value) Then Exit Property
                Me.m_DettaglioRichiestaSconto = value
                Me.DoChanged("DettaglioRichiestaSconto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce una stringa che specifica "in sintesi" il motivo per cui si è approvato lo sconto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MotivoConfermaSconto As String
            Get
                Return Me.m_MotivoConfermaSconto
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_MotivoConfermaSconto
                If (oldValue = value) Then Exit Property
                Me.m_MotivoConfermaSconto = value
                Me.DoChanged("MotivoConfermaSconto", value, oldValue)
            End Set
        End Property

        Public Property DettaglioConfermaSconto As String
            Get
                Return Me.m_DettaglioConfermaSconto
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_DettaglioConfermaSconto
                If (oldValue = value) Then Exit Property
                Me.m_DettaglioConfermaSconto = value
                Me.DoChanged("DettaglioConfermaSconto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data in cui il supervisore ha confermato o negato lo sconto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataConfermaSconto As Nullable(Of Date)
            Get
                Return Me.m_DataConfermaSconto
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataConfermaSconto
                If (oldValue = value) Then Exit Property
                Me.m_DataConfermaSconto = value
                Me.DoChanged("DataConfermaSconto", value, oldValue)
            End Set
        End Property

        Public Property IDSchermata As Integer
            Get
                Return GetID(Me.m_Schermata, Me.m_IDSchermata)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDSchermata
                If (oldValue = value) Then Exit Property
                Me.m_IDSchermata = value
                Me.DoChanged("IDSchermata", value, oldValue)
            End Set
        End Property

        Public Property Schermata As CAttachment
            Get
                If Me.m_Schermata Is Nothing Then Me.m_Schermata = Attachments.GetItemById(Me.m_IDSchermata)
                Return Me.m_Schermata
            End Get
            Set(value As CAttachment)
                Me.m_Schermata = value
                Me.m_IDSchermata = GetID(value)
                Me.DoChanged("Schermata", value)
            End Set
        End Property



        ''' <summary>
        ''' Restituisce il codice di errore 	
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ErrorCode As ErrorCodes
            Get
                Me.Validate()
                Return Me.m_ErrorCode
            End Get
        End Property

        ''' <summary>
        ''' Restituisce i messaggi di errore
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Messages As String
            Get
                Me.Validate()
                Return Me.m_Messages
            End Get
        End Property

        ''' <summary>
        ''' Inserisce un nuovo messaggio
        ''' </summary>
        ''' <param name="message"></param>
        ''' <remarks></remarks>
        Private Sub AppendMessage(ByVal message As String)
            If Me.m_Messages <> "" Then Me.m_Messages = Me.m_Messages & vbCrLf
            Me.m_Messages = Me.m_Messages & message
        End Sub

        ''' <summary>
        ''' Rimuove tutti i messaggi
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub ClearMessages()
            Me.m_Messages = ""
        End Sub

        ''' <summary>
        ''' Calcola il TEG ed il TAEG in funzione delle spese indicate
        ''' </summary>
        ''' <param name="offerta"></param>
        ''' <param name="flag"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CalculateTEG(ByVal offerta As COffertaCQS, ByVal flag As TEGCalcFlag) As Double
            Dim fEval As New CTAEGFunEvaluator
            fEval.Durata = Me.Durata
            fEval.Quota = Me.Rata
            fEval.NettoRicavo = Me.MontanteLordo
            If (TestFlag(flag, TEGCalcFlag.ALTREIMPOSTE)) Then fEval.NettoRicavo -= Me.Imposte
            If (TestFlag(flag, TEGCalcFlag.ALTRESPESE)) Then fEval.NettoRicavo -= Me.AltreSpese
            If (TestFlag(flag, TEGCalcFlag.IMPOSTASOSTITUTIVA)) Then fEval.NettoRicavo -= Me.ImpostaSostitutiva
            If (TestFlag(flag, TEGCalcFlag.INTERESSI)) Then fEval.NettoRicavo -= Me.Interessi
            If (TestFlag(flag, TEGCalcFlag.COMMISSIONI)) Then fEval.NettoRicavo -= Me.CommissioniBancarie
            If (TestFlag(flag, TEGCalcFlag.ONERIERARIALI)) Then fEval.NettoRicavo -= Me.OneriErariali
            If (TestFlag(flag, TEGCalcFlag.PREMIOCREDITO)) Then fEval.NettoRicavo -= Me.PremioCredito
            If (TestFlag(flag, TEGCalcFlag.PREMIOIMPIEGO)) Then fEval.NettoRicavo -= Me.PremioImpiego
            If (TestFlag(flag, TEGCalcFlag.PREMIOVITA)) Then fEval.NettoRicavo -= Me.PremioVita
            If (TestFlag(flag, TEGCalcFlag.PROVVIGIONE)) Then fEval.NettoRicavo -= Me.Provvigionale.ValoreTotale
            If (TestFlag(flag, TEGCalcFlag.RIVALSA)) Then fEval.NettoRicavo -= Me.Rivalsa
            If (TestFlag(flag, TEGCalcFlag.SPESECONVENZIONI)) Then fEval.NettoRicavo -= Me.SpeseConvenzioni
            If (TestFlag(flag, TEGCalcFlag.SPREAD)) Then fEval.NettoRicavo -= Me.ValoreSpreadBase
            Return Math.FindZero(fEval, Math.TOLMIN, 1) * 100
        End Function

        Public Function CanCalculate() As Boolean
            Dim ret As Boolean = True
            ret = ret And Not (Me.Prodotto Is Nothing)
            Return ret
        End Function

        ''' <summary>
        ''' Calcola il TAN
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CalculateTAN() As Double
            Dim c As New CTANCalculator
            c.Rata = Me.Rata
            c.Durata = Me.Durata
            c.Importo = Me.MontanteLordo - Me.m_Interessi
            Return c.Calc * 100
        End Function

        Public Property Rata As Decimal
            Get
                Return Me.m_Rata
            End Get
            Set(value As Decimal)
                Dim oldValue As Decimal = Me.m_Rata
                If (oldValue = value) Then Exit Property
                Me.m_Rata = value
                Me.Invalidate()
                Me.DoChanged("Rata", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Azzera tutti i calcoli
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Reset()
            Me.m_ErrorCode = ErrorCodes.ERROR_GENERIC
            Me.ClearMessages()
            Me.m_Calcolato = False
            Me.m_Eta = 0
            Me.m_Anzianita = 0
            Me.m_ValoreSpreadBase = 0
            Me.m_ValoreSpread = 0
            Me.m_ValoreRappel = 0
            Me.m_ValoreUpFront = 0
            Me.m_ValoreRunning = 0
            Me.m_PremioVita = 0
            Me.m_PremioImpiego = 0
            Me.m_PremioCredito = 0
            Me.m_ImpostaSostitutiva = 0
            Me.m_OneriErariali = 0
            Me.m_NettoRicavo = 0
            Me.m_CommissioniBancarie = 0
            Me.m_Interessi = 0
            Me.m_Imposte = 0
            Me.m_SpeseConvenzioni = 0
            Me.m_AltreSpese = 0
            Me.m_Rivalsa = 0
            Me.m_TEG = 0
            Me.m_TEG_Max = 0
            Me.m_TAEG = 0
            Me.m_TAEG_Max = 0
            Me.m_TAN = 0
            Me.m_Provvigionale = New CProvvigionale
        End Sub

        Public Sub Validate()
            If Me.m_OffertaLibera OrElse Me.m_Calcolato Then Exit Sub
            Me.Calcola()
        End Sub

        Public Sub Invalidate()
            If (Me.m_Calculating) Then Exit Sub
            Me.m_Calcolato = False
        End Sub

        ''' <summary>
        ''' Funzione utilizzata internamente per il calcolo dell'offerta
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub CalculateInternal()
            Dim SpreadBase As Double

            Me.Reset()

            If Not Me.m_DataNascita.HasValue AndAlso Me.Cliente IsNot Nothing Then
                Me.m_DataNascita = Me.Cliente.DataNascita
            End If

            If Not Me.m_DataAssunzione.HasValue AndAlso Me.Cliente IsNot Nothing Then
                If Me.Cliente.ImpiegoPrincipale IsNot Nothing Then
                    Me.m_DataAssunzione = Me.Cliente.ImpiegoPrincipale.DataAssunzione
                Else
                    For i As Integer = 0 To Me.Cliente.Impieghi.Count - 1
                        Dim impiego As CImpiegato = Me.Cliente.Impieghi(i)
                        If impiego.DataAssunzione.HasValue Then
                            Me.m_DataAssunzione = impiego.DataAssunzione
                            Exit For
                        End If
                    Next
                End If
            End If

            If Me.m_Sesso = "" And Me.Cliente IsNot Nothing Then
                Me.m_Sesso = Me.Cliente.Sesso
            End If

            If Not Me.m_DataDecorrenza.HasValue Then
                Me.m_DataDecorrenza = DateUtils.ToDay
            End If

            If (Me.m_DataNascita.HasValue = False) Then
                Me.m_ErrorCode = ErrorCodes.ERROR_INVALIDARGUMENT
                Me.m_Calculating = False
                Me.AppendMessage("Data di nascita non valida")
                Exit Sub
            End If

            If (Me.m_DataAssunzione.HasValue = False) Then
                Me.m_ErrorCode = ErrorCodes.ERROR_INVALIDARGUMENT
                Me.m_Calculating = False
                Me.AppendMessage("Data di assunzione non valida")
                Exit Sub
            End If

            If (Me.m_Sesso <> "M" And Me.m_Sesso <> "F") Then
                Me.m_ErrorCode = ErrorCodes.ERROR_INVALIDARGUMENT
                Me.m_Calculating = False
                Me.AppendMessage("Sesso non valida")
                Exit Sub
            End If

            If Me.Prodotto Is Nothing Then
                Me.m_ErrorCode = ErrorCodes.ERROR_INVALIDARGUMENT
                Me.m_Calculating = False
                Me.AppendMessage("Prodotto nullo")
                Exit Sub
            End If

            'Otteniamo lo spread base definito dal listino
            If Me.Profilo IsNot Nothing Then
                SpreadBase = Me.Profilo.ProdottiXProfiloRelations.GetSpread(Me.Prodotto)
            End If
            Me.m_ValoreSpreadBase = SpreadBase * Me.MontanteLordo / 100

            'Validiamo la provvigione
            If (Me.SpreadBase + Me.Spread + Me.Provvigionale.PercentualeSu(Me.MontanteLordo) > Me.Prodotto.ProvvigioneMassima) Then
                Me.m_ErrorCode = ErrorCodes.ERROR_PROVVMAX
                Me.AppendMessage("Superato il provvigionale massimo:" & Formats.FormatPercentage(Me.Prodotto.ProvvigioneMassima - SpreadBase, 3) & "%")
                Me.m_Calculating = False
                Exit Sub
            End If

            'Calcolo della parte finanziaria
            If Me.TabellaFinanziariaRel IsNot Nothing Then
                Me.TabellaFinanziariaRel.Calcola(Me)
            End If

            'Calcolo della parte assicurativa
            If Me.TabellaAssicurativaRel IsNot Nothing Then
                Me.TabellaAssicurativaRel.Calcola(Me)
            End If

            'Calcoliamo le spese
            If (Me.TabellaSpese IsNot Nothing AndAlso Me.TabellaSpese.TabellaSpese IsNot Nothing) Then
                Me.TabellaSpese.TabellaSpese.Calcola(Me)
            End If


            'Calciliamo il netto ricavo		
            Me.m_NettoRicavo = Me.MontanteLordo - Me.m_Rivalsa - Me.m_SpeseConvenzioni - Me.m_AltreSpese - (Me.m_PremioVita + Me.m_PremioImpiego + Me.m_PremioCredito) - Me.m_OneriErariali - Me.m_Imposte - Me.m_CommissioniBancarie - Me.m_Interessi
            If Me.Prodotto.ProvvigioneErogataDa = ProvvigioneErogataDa.Direttamente Then
                If (Me.Provvigionale.ValoreTotale.HasValue) Then
                    Me.m_NettoRicavo = Me.m_NettoRicavo - Me.Provvigionale.ValoreTotale
                End If
            End If

            'Calcoliamo il netto ricavo
            Me.m_NettoRicavo = Me.m_NettoRicavo - Me.ImpostaSostitutiva
            If Not (Me.TabellaFinanziariaRel Is Nothing) Then
                If Not Me.TabellaFinanziariaRel.Check(Me) Then
                    Me.m_ErrorCode = ErrorCodes.ERROR_TABFINCONSTR
                    Me.m_Messages = "Vincoli della tabella finanziaria non rispettati"
                    Me.m_Calculating = False
                    Exit Sub
                End If
            End If
            If Not (Me.TabellaAssicurativaRel Is Nothing) Then
                If Not Me.TabellaAssicurativaRel.Check(Me) Then
                    Me.m_ErrorCode = ErrorCodes.ERROR_TABASSCONSTR
                    Me.m_Messages = "Vincoli della tabella assicurativa non rispettati"
                    Me.m_Calculating = False
                    Exit Sub
                End If
            End If

            'Otteniamo il TEG Max
            Me.m_TEG_Max = Me.GetMaxTEG

            'Otteniamo il TAEG Max
            Me.m_TAEG_Max = Me.GetMaxTAEG

            'Calcoliamo il TAN
            Me.m_TAN = Me.CalculateTAN

            'Calcoliamo il TEG
            Me.m_TEG = Me.CalculateTEG(Me, Me.m_TipoCalcoloTEG)
            If (Me.m_TEG < 0) Then
                Me.m_ErrorCode = ErrorCodes.ERROR_ZERO
                Me.AppendMessage("Zero della funzione TEG non trovato")
            End If

            'Calcoliamo il TAEG
            Me.m_TAEG = Me.CalculateTEG(Me, Me.m_TipoCalcoloTAEG)
            If (Me.m_TEG < 0) Then
                Me.m_ErrorCode = ErrorCodes.ERROR_ZERO
                Me.AppendMessage("Zero della funzione TAEG non trovato")
            End If

            If (Me.m_TEG > Me.m_TEG_Max) Then
                Me.m_ErrorCode = ErrorCodes.ERROR_TEGMAX
                Me.AppendMessage("TEG troppo alto: " & Formats.FormatPercentage(Me.m_TEG_Max, 4) & "%")
                Exit Sub
            End If

            If (Me.m_TAEG > Me.m_TAEG_Max) Then
                Me.m_ErrorCode = ErrorCodes.ERROR_TAEGMAX
                Me.AppendMessage("TAEG troppo alto: " & Formats.FormatPercentage(Me.m_TAEG_Max, 4) & "%")
                Exit Sub
            End If
            Me.m_ErrorCode = ErrorCodes.ERROR_OK
        End Sub

        Public Sub Calcola()
            Dim spreadBase As Double = 0

            If (Me.m_Calculating) Then Exit Sub
            Me.m_Calculating = True

            Dim p, [step] As Double
            Dim t As Boolean
            [step] = 0.01

            If (Me.Profilo IsNot Nothing AndAlso Me.Prodotto IsNot Nothing) Then
                spreadBase = Formats.ToDouble(Me.Profilo.ProdottiXProfiloRelations.GetSpread(Me.Prodotto))
            End If

            If (Me.CaricaAlMassimo) Then
                p = Me.Prodotto.ProvvigioneMassima - spreadBase

                While (p >= 0)
                    Me.Provvigionale.PercentualeSu(Me.MontanteLordo) = p
                    Me.CalculateInternal()
                    Select Case ErrorCode
                        Case ErrorCodes.ERROR_TEGMAX, ErrorCodes.ERROR_TAEGMAX
                            p = p - 1
                        Case ErrorCodes.ERROR_OK
                            t = True
                            While t And (p < Me.Prodotto.ProvvigioneMassima - spreadBase)
                                p = p + [step]
                                Me.Provvigionale.PercentualeSu(Me.MontanteLordo) = p
                                Me.CalculateInternal()

                                Select Case ErrorCode
                                    Case ErrorCodes.ERROR_OK
                                    Case Else
                                        p = p - [step]
                                        t = False
                                End Select
                            End While
                            t = True
                            While t And (p < Me.Prodotto.ProvvigioneMassima - spreadBase)
                                p = p + [step]
                                Me.Provvigionale.PercentualeSu(Me.MontanteLordo) = p
                                Me.CalculateInternal()

                                Select Case ErrorCode
                                    Case ErrorCodes.ERROR_OK
                                    Case Else
                                        p = p - [step]
                                        t = False
                                End Select
                            End While

                            Me.Provvigionale.PercentualeSu(Me.MontanteLordo) = p
                            Me.CalculateInternal()
                            Exit Sub
                        Case Else
                            Me.Provvigionale.PercentualeSu(Me.MontanteLordo) = 0
                            Exit Sub
                    End Select
                End While

                Me.Provvigionale.PercentualeSu(Me.MontanteLordo) = 0
                Me.CalculateInternal()
            Else
                Me.CalculateInternal()
            End If

            Me.m_Calcolato = True
            Me.m_Calculating = False
        End Sub

        Function EvaluateExpression(ByVal expr As String) As Object
            Return Sistema.Types.CallMethod(Me, expr)
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_Preventivi_Offerte"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_OffertaLibera = reader.Read("OffertaLibera", Me.m_OffertaLibera)
            Me.m_IDPratica = reader.Read("IDPratica", Me.m_IDPratica)
            Me.m_IDCliente = reader.Read("IDCliente", Me.m_IDCliente)
            Me.m_NomeCliente = reader.Read("NomeCliente", Me.m_NomeCliente)
            Me.m_StatoOfferta = reader.Read("StatoOfferta", Me.m_StatoOfferta)
            Me.m_PreventivoID = reader.Read("Preventivo", Me.m_PreventivoID)
            Me.m_IDCessionario = reader.Read("IDCessionario", Me.m_IDCessionario)
            Me.m_NomeCessionario = reader.Read("NomeCessionario", Me.m_NomeCessionario)
            Me.m_IDProfilo = reader.Read("IDProfilo", Me.m_IDProfilo)
            Me.m_NomeProfilo = reader.Read("NomeProfilo", Me.m_NomeProfilo)
            Me.m_ProdottoID = reader.Read("Prodotto", Me.m_ProdottoID)
            Me.m_NomeProdotto = reader.Read("NomeProdotto", Me.m_NomeProdotto)
            Me.m_Calcolato = reader.Read("Calcolato", Me.m_Calcolato)
            Me.m_Durata = reader.Read("Durata", Me.m_Durata)
            Me.m_Rata = reader.Read("Rata", Me.m_Rata)
            Me.m_Eta = reader.Read("Eta", Me.m_Eta)
            Me.m_Anzianita = reader.Read("Anzianita", Me.m_Anzianita)
            Me.m_TabellaAssicurativaRelID = reader.Read("TabellaAssicurativaRel", Me.m_TabellaAssicurativaRelID)
            Me.m_NomeTabellaAssicurativa = reader.Read("NomeTabellaAssicurativa", Me.m_NomeTabellaAssicurativa)
            Me.m_TabellaFinanziariaRelID = reader.Read("TabellaFinanziariaRel", Me.m_TabellaFinanziariaRelID)
            Me.m_NomeTabellaFinanziaria = reader.Read("NomeTabellaFinanziaria", Me.m_NomeTabellaFinanziaria)
            Me.m_TabellaSpeseID = reader.Read("TabellaSpese", Me.m_TabellaSpeseID)
            Me.m_ValoreProvvigioneMassima = reader.Read("MaxProvv", Me.m_ValoreProvvigioneMassima)
            Me.m_ValoreRappel = reader.Read("Rappel", Me.m_ValoreRappel)
            Me.m_ValoreSpreadBase = reader.Read("SpreadBase", Me.m_ValoreSpreadBase)
            Me.m_ValoreSpread = reader.Read("Spread", Me.m_ValoreSpread)
            Me.m_ValoreUpFront = reader.Read("UpFront", Me.m_ValoreUpFront)
            Me.m_ValoreRunning = reader.Read("Running", Me.m_ValoreRunning)
            Me.m_PremioVita = reader.Read("PremioVita", Me.m_PremioVita)
            Me.m_PremioImpiego = reader.Read("PremioImpiego", Me.m_PremioImpiego)
            Me.m_PremioCredito = reader.Read("PremioCredito", Me.m_PremioCredito)
            Me.m_DataNascita = reader.Read("DataNascita", Me.m_DataNascita)
            Me.m_DataAssunzione = reader.Read("DataAssunzione", Me.m_DataAssunzione)
            Me.m_ImpostaSostitutiva = reader.Read("ImpostaSostitutiva", Me.m_ImpostaSostitutiva)
            Me.m_OneriErariali = reader.Read("OneriErariali", Me.m_OneriErariali)
            Me.m_NettoRicavo = reader.Read("NettoRicavo", Me.m_NettoRicavo)
            Me.m_CommissioniBancarie = reader.Read("CommissioniBancarie", Me.m_CommissioniBancarie)
            Me.m_Interessi = reader.Read("Interessi", Me.m_Interessi)
            Me.m_Imposte = reader.Read("Imposte", Me.m_Imposte)
            Me.m_SpeseConvenzioni = reader.Read("SpeseConvenzioni", Me.m_SpeseConvenzioni)
            Me.m_AltreSpese = reader.Read("AltreSpese", Me.m_AltreSpese)
            Me.m_Rivalsa = reader.Read("Rivalsa", Me.m_Rivalsa)
            Me.m_TEG = reader.Read("TEG", Me.m_TEG)
            Me.m_TEG_Max = reader.Read("TEG_Max", Me.m_TEG_Max)
            Me.m_TAEG = reader.Read("TAEG", Me.m_TAEG)
            Me.m_TAEG_Max = reader.Read("TAEG_Max", Me.m_TAEG_Max)
            Me.m_TAN = reader.Read("TAN", Me.m_TAN)
            Me.m_DataDecorrenza = reader.Read("DataDecorrenza", Me.m_DataDecorrenza)
            Me.m_Sesso = reader.Read("Sesso", Me.m_Sesso)
            Me.m_CaricaAlMassimo = reader.Read("CaricaAlMassimo", Me.m_CaricaAlMassimo)
            Me.m_TipoCalcoloTAEG = reader.Read("TipoCalcoloTAEG", Me.m_TipoCalcoloTAEG)
            Me.m_TipoCalcoloTEG = reader.Read("TipoCalcoloTEG", Me.m_TipoCalcoloTEG)

            Me.m_ErrorCode = reader.Read("ErrorCode", Me.m_ErrorCode)
            Me.m_Messages = reader.Read("Messages", Me.m_Messages)

            Me.m_IDSupervisore = reader.Read("IDSupervisore", Me.m_IDSupervisore)
            Me.m_NomeSupervisore = reader.Read("NomeSupervisore", Me.m_NomeSupervisore)
            Me.m_MotivoRichiestaSconto = reader.Read("MotivoRS", Me.m_MotivoRichiestaSconto)
            Me.m_DettaglioRichiestaSconto = reader.Read("DettaglioRS", Me.m_DettaglioRichiestaSconto)
            Me.m_MotivoConfermaSconto = reader.Read("MotivoCS", Me.m_MotivoConfermaSconto)
            Me.m_DettaglioConfermaSconto = reader.Read("DettaglioCS", Me.m_DettaglioConfermaSconto)
            Me.m_DataConfermaSconto = reader.Read("DataCS", Me.m_DataConfermaSconto)
            Me.m_IDSchermata = reader.Read("IDSchermata", Me.m_IDSchermata)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_LimiteRataMax = reader.Read("LimiteRataMax", Me.m_LimiteRataMax)
            Me.m_LimiteRataNote = reader.Read("LimiteRataNote", Me.m_LimiteRataNote)
            Me.m_ProvvTAN = reader.Read("ProvvTAN", Me.m_ProvvTAN)

            With Me.Provvigionale
                .Tipo = reader.Read("ProvvBrokerSu", TipoCalcoloProvvigionale.SOLOBASE)
                .ValoreBase = reader.Read("Provvigioni", .ValoreBase)
                .ValorePercentuale = reader.Read("ProvvBrokerPerc", .ValorePercentuale)
                .SetChanged(False)
            End With

            Me.m_ValoreRiduzioneProvvigionale = reader.Read("ValoreRiduzioneProvv", Me.m_ValoreRiduzioneProvvigionale)
            Me.m_PremioDaCessionario = reader.Read("PremioDaCessionario", Me.m_PremioDaCessionario)

            Try
                Me.m_Attributi = XML.Utils.Serializer.Deserialize(reader.Read("Attributi", ""))
            Catch ex As Exception
                Me.m_Attributi = Nothing
            End Try

            Me.m_CapitaleFinanziato = reader.Read("CapitaleFinanziato", Me.m_CapitaleFinanziato)
            Me.m_ProvvCollab = reader.Read("ProvvCollab", Me.m_ProvvCollab)

            Me.m_IDCollaboratore = reader.Read("IDCollaboratore", Me.m_IDCollaboratore)
            Me.m_IDClienteXCollaboratore = reader.Read("IDClienteXCollaboratore", Me.m_IDClienteXCollaboratore)

            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("MaxProvv", Me.m_ValoreProvvigioneMassima)
            writer.Write("UpFront", Me.m_ValoreUpFront)
            writer.Write("Running", Me.m_ValoreRunning)
            writer.Write("Rappel", Me.m_ValoreRappel)
            writer.Write("SpreadBase", Me.m_ValoreSpreadBase)
            writer.Write("Spread", Me.m_ValoreSpread)

            writer.Write("Preventivo", GetID(Me.m_Preventivo, Me.m_PreventivoID))
            writer.Write("Eta", Me.m_Eta)
            writer.Write("Anzianita", Me.m_Anzianita)
            writer.Write("Prodotto", GetID(Me.m_Prodotto, Me.m_ProdottoID))
            writer.Write("NomeProdotto", Me.m_NomeProdotto)
            writer.Write("TabellaAssicurativaRel", Me.TabellaAssicurativaRelID)
            writer.Write("NomeTabellaAssicurativa", Me.m_NomeTabellaAssicurativa)
            writer.Write("TabellaFinanziariaRel", GetID(Me.m_TabellaFinanziariaRel, Me.m_TabellaFinanziariaRelID))
            writer.Write("NomeTabellaFinanziaria", Me.m_NomeTabellaFinanziaria)
            writer.Write("TabellaSpese", Me.TabellaSpeseID)
            'dbRis("Assicurazione") = Databases.GetID(m_Assicurazione, m_AssicurazioneID)
            'dbRis("NomeAssicurazione") = m_NomeAssicurazione
            writer.Write("Calcolato", Me.m_Calcolato)
            writer.Write("PremioVita", Me.m_PremioVita)
            writer.Write("PremioImpiego", Me.m_PremioImpiego)
            writer.Write("PremioCredito", Me.m_PremioCredito)
            writer.Write("ImpostaSostitutiva", Me.m_ImpostaSostitutiva)
            writer.Write("OneriErariali", Me.m_OneriErariali)
            writer.Write("NettoRicavo", Me.m_NettoRicavo)
            writer.Write("CommissioniBancarie", Me.m_CommissioniBancarie)
            writer.Write("Interessi", Me.m_Interessi)
            writer.Write("Imposte", Me.m_Imposte)
            writer.Write("AltreSpese", Me.m_AltreSpese)
            writer.Write("SpeseConvenzioni", Me.m_SpeseConvenzioni)
            writer.Write("Rivalsa", Me.m_Rivalsa)
            writer.Write("TEG", Me.m_TEG)
            writer.Write("TEG_Max", Me.m_TEG_Max)
            writer.Write("TAEG", Me.m_TAEG)
            writer.Write("TAEG_Max", Me.m_TAEG_Max)
            writer.Write("TAN", Me.m_TAN)
            writer.Write("ErrorCode", Me.m_ErrorCode)
            writer.Write("Messages", Me.m_Messages)
            writer.Write("DataDecorrenza", Me.m_DataDecorrenza)
            writer.Write("Sesso", Me.m_Sesso)
            writer.Write("DataNascita", Me.m_DataNascita)
            writer.Write("DataAssunzione", Me.m_DataAssunzione)
            writer.Write("Rata", Me.m_Rata)
            writer.Write("Durata", Me.m_Durata)
            writer.Write("NomeCliente", Me.m_NomeCliente)
            writer.Write("IDCliente", Me.IDCliente)
            writer.Write("IDCessionario", Me.IDCessionario)
            writer.Write("NomeCessionario", Me.m_NomeCessionario)
            writer.Write("IDProfilo", Me.IDProfilo)
            writer.Write("NomeProfilo", Me.m_NomeProfilo)
            writer.Write("IDPratica", Me.IDPratica)
            writer.Write("StatoOfferta", Me.m_StatoOfferta)
            writer.Write("CaricaAlMassimo", Me.m_CaricaAlMassimo)
            writer.Write("OffertaLibera", Me.m_OffertaLibera)
            writer.Write("TipoCalcoloTAEG", Me.m_TipoCalcoloTAEG)
            writer.Write("TipoCalcoloTEG", Me.m_TipoCalcoloTEG)

            writer.Write("IDSupervisore", Me.IDSupervisore)
            writer.Write("NomeSupervisore", Me.m_NomeSupervisore)
            writer.Write("MotivoRS", Me.m_MotivoRichiestaSconto)
            writer.Write("DettaglioRS", Me.m_DettaglioRichiestaSconto)
            writer.Write("MotivoCS", Me.m_MotivoConfermaSconto)
            writer.Write("DettaglioCS", Me.m_DettaglioConfermaSconto)
            writer.Write("DataCS", Me.m_DataConfermaSconto)
            writer.Write("IDSchermata", Me.IDSchermata)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("LimiteRataMax", Me.m_LimiteRataMax)
            writer.Write("LimiteRataNote", Me.m_LimiteRataNote)

            With Me.Provvigionale
                writer.Write("ProvvBrokerSu", .Tipo)
                writer.Write("Provvigioni", .ValoreBase)
                writer.Write("ProvvBrokerPerc", .ValorePercentuale)
            End With

            writer.Write("ValoreRiduzioneProvv", Me.m_ValoreRiduzioneProvvigionale)
            writer.Write("PremioDaCessionario", Me.m_PremioDaCessionario)

            writer.Write("Attributi", XML.Utils.Serializer.Serialize(Me.Attributi))

            writer.Write("CapitaleFinanziato", Me.m_CapitaleFinanziato)
            writer.Write("ProvvTAN", Me.m_ProvvTAN)
            writer.Write("ProvvCollab", Me.m_ProvvCollab)

            writer.Write("IDCollaboratore", Me.IDCollaboratore)
            writer.Write("IDClienteXCollaboratore", Me.IDClienteXCollaboratore)

            Return MyBase.SaveToRecordset(writer)
        End Function

        Public Overrides Function IsChanged() As Boolean
            Return MyBase.IsChanged() OrElse Me.Provvigionale.IsChanged
        End Function

        '------------------------------------
        Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
            writer.WriteAttribute("IDPratica", Me.IDPratica)
            writer.WriteAttribute("StatoOfferta", Me.m_StatoOfferta)
            writer.WriteAttribute("IDCliente", Me.IDCliente)
            writer.WriteAttribute("NomeCliente", Me.m_NomeCliente)
            writer.WriteAttribute("IDPreventivo", Me.PreventivoID)
            writer.WriteAttribute("IDCessionario", Me.IDCessionario)
            writer.WriteAttribute("NomeCessionario", Me.m_NomeCessionario)
            writer.WriteAttribute("IDProfilo", Me.IDProfilo)
            writer.WriteAttribute("NomeProfilo", Me.m_NomeProfilo)
            writer.WriteAttribute("IDProdotto", Me.ProdottoID)
            writer.WriteAttribute("NomeProdotto", Me.m_NomeProdotto)
            writer.WriteAttribute("Calcolato", Me.m_Calcolato)
            writer.WriteAttribute("Durata", Me.m_Durata)
            writer.WriteAttribute("Rata", Me.m_Rata)
            writer.WriteAttribute("Eta", Me.m_Eta)
            writer.WriteAttribute("Anzianita", Me.m_Anzianita)
            writer.WriteAttribute("IDTabellaSpese", Me.TabellaSpeseID)
            writer.WriteAttribute("IDTabellaFinanziariaID", Me.TabellaFinanziariaRelID)
            writer.WriteAttribute("NomeTabellaFinanziaria", Me.m_NomeTabellaFinanziaria)
            writer.WriteAttribute("IDTabellaAssicurativaID", Me.TabellaAssicurativaRelID)
            writer.WriteAttribute("NomeTabellaAssicurativa", Me.m_NomeTabellaAssicurativa)
            writer.WriteAttribute("Rappel", Me.m_ValoreRappel)
            writer.WriteAttribute("ProvvMax", Me.m_ValoreProvvigioneMassima)
            writer.WriteAttribute("SpreadBase", Me.m_ValoreSpreadBase)
            writer.WriteAttribute("Spread", Me.m_ValoreSpread)
            writer.WriteAttribute("UpFront", Me.m_ValoreUpFront)
            writer.WriteAttribute("Running", Me.m_ValoreRunning)
            writer.WriteAttribute("DataNascita", Me.m_DataNascita)
            writer.WriteAttribute("DataAssunzione", Me.m_DataAssunzione)
            writer.WriteAttribute("PremioVita", Me.m_PremioVita)
            writer.WriteAttribute("PremioImpiego", Me.m_PremioImpiego)
            writer.WriteAttribute("PremioCredito", Me.m_PremioCredito)
            writer.WriteAttribute("ImpostaSostitutiva", Me.m_ImpostaSostitutiva)
            writer.WriteAttribute("OneriErariali", Me.m_OneriErariali)
            writer.WriteAttribute("NettoRicavo", Me.m_NettoRicavo)
            writer.WriteAttribute("CommissioniBancarie", Me.m_CommissioniBancarie)
            writer.WriteAttribute("Interessi", Me.m_Interessi)
            writer.WriteAttribute("Imposte", Me.m_Imposte)
            writer.WriteAttribute("SpeseConvenzioni", Me.m_SpeseConvenzioni)
            writer.WriteAttribute("AltreSpese", Me.m_AltreSpese)
            writer.WriteAttribute("Rivalsa", Me.m_Rivalsa)
            writer.WriteAttribute("TEG", Me.m_TEG)
            writer.WriteAttribute("TEG_Max", Me.m_TEG_Max)
            writer.WriteAttribute("TAEG", Me.m_TAEG)
            writer.WriteAttribute("TAEG_Max", Me.m_TAEG_Max)
            writer.WriteAttribute("TAN", Me.m_TAN)
            writer.WriteAttribute("DataDecorrenza", Me.m_DataDecorrenza)
            writer.WriteAttribute("Sesso", Me.m_Sesso)
            writer.WriteAttribute("ErrorCode", Me.m_ErrorCode)
            writer.WriteAttribute("CaricaAlMassimo", Me.m_CaricaAlMassimo)
            writer.WriteAttribute("OffertaLibera", Me.m_OffertaLibera)
            writer.WriteAttribute("TipoCalcoloTAEG", Me.m_TipoCalcoloTAEG)
            writer.WriteAttribute("TipoCalcoloTEG", Me.m_TipoCalcoloTEG)
            writer.WriteAttribute("IDSupervisore", Me.IDSupervisore)
            writer.WriteAttribute("NomeSupervisore", Me.m_NomeSupervisore)
            writer.WriteAttribute("MotivoRS", Me.m_MotivoRichiestaSconto)
            writer.WriteAttribute("DataCS", Me.m_DataConfermaSconto)
            writer.WriteAttribute("IDSchermata", Me.IDSchermata)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("MotivoCS", Me.m_MotivoConfermaSconto)
            writer.WriteAttribute("LimiteRataMax", Me.m_LimiteRataMax)
            writer.WriteAttribute("LimiteRataNote", Me.m_LimiteRataNote)
            writer.WriteAttribute("ValoreRiduzioneProvvigionale", Me.m_ValoreRiduzioneProvvigionale)
            writer.WriteAttribute("PremioDaCessionario", Me.m_PremioDaCessionario)
            writer.WriteAttribute("CapitaleFinanziato", Me.m_CapitaleFinanziato)
            writer.WriteAttribute("ValoreProvvTAN", Me.m_ProvvTAN)
            writer.WriteAttribute("ProvvCollab", Me.m_ProvvCollab)
            writer.WriteAttribute("IDCollaboratore", Me.IDCollaboratore)
            writer.WriteAttribute("IDClienteXCollaboratore", Me.IDClienteXCollaboratore)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Provvigioni", Me.m_Provvigioni)
            writer.WriteTag("Provvigionale", Me.Provvigionale)
            writer.WriteTag("Messages", Me.m_Messages)
            writer.WriteTag("DettaglioRS", Me.m_DettaglioRichiestaSconto)
            writer.WriteTag("DettaglioCS", Me.m_DettaglioConfermaSconto)
            writer.WriteTag("Attributi", Me.Attributi)
        End Sub

        Protected Overrides Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object)
            Select Case fieldName
                Case "IDPratica" : Me.m_IDPratica = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "StatoOfferta" : Me.m_StatoOfferta = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDCliente" : Me.m_IDCliente = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeCliente" : Me.m_NomeCliente = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDPreventivo" : Me.m_PreventivoID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDCessionario" : Me.m_IDCessionario = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeCessionario" : Me.m_NomeCessionario = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDProfilo" : Me.m_IDProfilo = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeProfilo" : Me.m_NomeProfilo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDProdotto" : Me.m_ProdottoID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeProdotto" : Me.m_NomeProdotto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Calcolato" : Me.m_Calcolato = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "Durata" : Me.m_Durata = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Rata" : Me.m_Rata = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Eta" : Me.m_Eta = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Anzianita" : Me.m_Anzianita = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "IDTabellaSpese" : Me.m_TabellaSpeseID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDTabellaFinanziariaID" : Me.m_TabellaFinanziariaRelID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeTabellaFinanziaria" : Me.m_NomeTabellaFinanziaria = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDTabellaAssicurativaID" : Me.m_TabellaAssicurativaRelID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeTabellaAssicurativa" : Me.m_NomeTabellaAssicurativa = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Rappel" : Me.m_ValoreRappel = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "ProvvMax" : Me.m_ValoreProvvigioneMassima = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "SpreadBase" : Me.m_ValoreSpreadBase = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Spread" : Me.m_ValoreSpread = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Provvigionale" : Me.m_Provvigionale = fieldValue
                Case "UpFront" : Me.m_ValoreUpFront = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Running" : Me.m_ValoreRunning = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "DataNascita" : Me.m_DataNascita = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataAssunzione" : Me.m_DataAssunzione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "PremioVita" : Me.m_PremioVita = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "PremioImpiego" : Me.m_PremioImpiego = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "PremioCredito" : Me.m_PremioCredito = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "ImpostaSostitutiva" : Me.m_ImpostaSostitutiva = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "OneriErariali" : Me.m_OneriErariali = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "NettoRicavo" : Me.m_NettoRicavo = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "CommissioniBancarie" : Me.m_CommissioniBancarie = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Interessi" : Me.m_Interessi = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Imposte" : Me.m_Imposte = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "SpeseConvenzioni" : Me.m_SpeseConvenzioni = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "AltreSpese" : Me.m_AltreSpese = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Rivalsa" : Me.m_Rivalsa = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "TEG" : Me.m_TEG = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "TEG_Max" : Me.m_TEG_Max = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "TAEG" : Me.m_TAEG = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "TAEG_Max" : Me.m_TAEG_Max = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "TAN" : Me.m_TAN = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "DataDecorrenza" : Me.m_DataDecorrenza = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Sesso" : Me.m_Sesso = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ErrorCode" : Me.m_ErrorCode = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Messages" : Me.m_Messages = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "CaricaAlMassimo" : Me.m_CaricaAlMassimo = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "OffertaLibera" : Me.m_OffertaLibera = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "TipoCalcoloTAEG" : Me.m_TipoCalcoloTAEG = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "TipoCalcoloTEG" : Me.m_TipoCalcoloTEG = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDSupervisore" : Me.m_IDSupervisore = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeSupervisore" : Me.m_NomeSupervisore = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "MotivoRS" : Me.m_MotivoRichiestaSconto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DettaglioRS" : Me.m_DettaglioRichiestaSconto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "MotivoCS" : Me.m_MotivoConfermaSconto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DettaglioCS" : Me.m_DettaglioConfermaSconto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataCS" : Me.m_DataConfermaSconto = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IDSchermata" : Me.m_IDSchermata = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "LimiteRataMax" : Me.m_LimiteRataMax = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "LimiteRataNote" : Me.m_LimiteRataNote = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ValoreRiduzioneProvvigionale" : Me.m_ValoreRiduzioneProvvigionale = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "PremioDaCessionario" : Me.m_PremioDaCessionario = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Attributi" : Me.m_Attributi = CType(fieldValue, CKeyCollection)
                Case "CapitaleFinanziato" : Me.m_CapitaleFinanziato = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "ValoreProvvTAN" : Me.m_ProvvTAN = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "ProvvCollab" : Me.m_ProvvCollab = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Provvigioni" : Me.m_Provvigioni = XML.Utils.Serializer.ToObject(fieldValue) : If (Me.m_Provvigioni IsNot Nothing) Then Me.m_Provvigioni.SetOfferta(Me)
                Case "IDCollaboratore" : Me.m_IDCollaboratore = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDClienteXCollaboratore" : Me.m_IDClienteXCollaboratore = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Overrides Function ToString() As String
            Return Me.NumeroOfferta & Me.NomeProdotto & " - " & Me.NomeCessionario & " (" & Formats.FormatValuta(Me.Rata) & " x " & Formats.FormatInteger(Me.Durata) & " = " & Formats.FormatValuta(Me.MontanteLordo) & ")"
        End Function

        Public ReadOnly Property NumeroOfferta As String
            Get
                Return Right("00000000" & Hex(Me.ID), 8)
            End Get
        End Property

        Protected Friend Overridable Sub SetPratica(ByVal value As CRapportino)
            Me.m_Pratica = value
            Me.m_IDPratica = GetID(value)
        End Sub

        Public Function CalcolaBaseML(ByVal estinzioni As CCollection(Of EstinzioneXEstintore)) As Decimal?
            Debug.Print("SimpleOffertaControl1.prototype.getBaseML")
            Dim ml As Decimal? = Me.MontanteLordo
            Debug.Print("ml: " & Formats.FormatValuta(ml))
            If (ml.HasValue = False) Then Return Nothing

            Dim tariffa As CProdottoXTabellaFin = Me.TabellaFinanziariaRel
            Dim tipoCalcolo As TipoCalcoloProvvigioni = TipoCalcoloProvvigioni.MONTANTE_LORDO
            If (tariffa IsNot Nothing AndAlso tariffa.Tabella IsNot Nothing AndAlso tariffa.Tabella.Stato = ObjectStatus.OBJECT_VALID) Then
                tipoCalcolo = tariffa.Tabella.TipoCalcoloProvvigioni
            End If
            Debug.Print("tipoCalcolo: " & tipoCalcolo)
            Select Case (tipoCalcolo)
                Case TipoCalcoloProvvigioni.MONTANTE_LORDO

                Case TipoCalcoloProvvigioni.DELTA_MONTANTE_ESTINZIONI
                    Dim sommaEstinzioni As Decimal? = Me.getSommaDebitiResidui(estinzioni)
                    Debug.Print("sommaEstinzioni: " & Formats.FormatValuta(sommaEstinzioni))
                    If (sommaEstinzioni.HasValue) Then
                        ml = ml.Value - sommaEstinzioni.Value
                    Else
                        Return Nothing
                    End If
                Case TipoCalcoloProvvigioni.FUNZIONE

            End Select

            Debug.Print("baseML: " & Formats.FormatValuta(ml))

            Return Math.Max(0, ml.Value)
        End Function

        Private Function isValid(ByVal est As EstinzioneXEstintore, ByVal tipoContratto As String) As Boolean
            Dim ret As Boolean = (est IsNot Nothing) AndAlso (est.Stato = ObjectStatus.OBJECT_VALID) AndAlso (est.NumeroQuoteResidue > 0)
            If (Not ret) Then Return ret
            Select Case (est.Tipo)
                Case TipoEstinzione.ESTINZIONE_CESSIONEDELQUINTO, TipoEstinzione.ESTINZIONE_CQP 'TipoEstinzione.ESTINZIONE_PICCOLO_PRESTITO
                    Return (tipoContratto = "C")
                Case TipoEstinzione.ESTINZIONE_PRESTITODELEGA
                    Return (tipoContratto = "D")
                    Return True
                Case Else
                    Return False
            End Select
        End Function

        Public Function getSommaDebitiResidui(ByVal items As CCollection(Of EstinzioneXEstintore)) As Decimal?
            Dim nomeCess As String = Strings.LCase(Me.NomeCessionario)
            Dim tipoContratto As String = ""
            If (Me.Prodotto IsNot Nothing) Then tipoContratto = Me.Prodotto.IdTipoContratto
            If (tipoContratto = "") Then tipoContratto = "C"
            Dim tp As TipoEstinzione
            If (tipoContratto = "C") Then
                tp = TipoEstinzione.ESTINZIONE_CESSIONEDELQUINTO
            Else
                tp = TipoEstinzione.ESTINZIONE_PRESTITODELEGA
            End If

            'Me.m_Contratto = Strings.Trim(this.m_Contratto);

            Dim dec As DateTime? = Me.DataDecorrenza
            Dim sum As Decimal = 0
            If (dec.HasValue = False) Then Return sum
            For i As Integer = 0 To items.Count() - 1
                Dim exi As EstinzioneXEstintore = items(i)
                Debug.Print("exi: " & GetID(exi))
                Debug.Print("exi.Selezionata: " & exi.Selezionata)
                Debug.Print("exi.Parametro: " & exi.Parametro & " / " & tipoContratto)
                If (exi.Selezionata AndAlso (tipoContratto = "" OrElse exi.Parametro = "" OrElse exi.Parametro = tipoContratto)) Then
                    'Dim est As CEstinzione = exi.Estinzione
                    Debug.Print("est: " & GetID(exi))
                    If (isValid(exi, tipoContratto)) Then
                        Debug.Print("Valid")
                        'var nRate = est.getDurata() - Math.max(0, exi.getNumeroQuoteResidue() + 1);
                        'var res = Formats.ToDouble(est.getDebitoResiduo(nRate));
                        'sum += Math.max(0, res);
                        Dim nmCess As String = Strings.LCase(exi.NomeCessionario)
                        Dim nRate As Integer = exi.NumeroQuoteResidue
                        Dim estTP As TipoEstinzione = exi.Tipo
                        If (estTP = TipoEstinzione.ESTINZIONE_CQP) Then estTP = TipoEstinzione.ESTINZIONE_CESSIONEDELQUINTO 'OrElse estTP = TipoEstinzione.ESTINZIONE_PICCOLO_PRESTITO
                        If ((tp <> TipoEstinzione.ESTINZIONE_NO) AndAlso (estTP = tp) AndAlso nomeCess <> "" AndAlso nomeCess = nmCess) Then
                            Dim DeltaML_AggiuntiRate As Integer = Formats.ToInteger(CQSPD.Configuration.Overflow.GetValueBool("DeltaML_AggiuntiRate"))
                            nRate = nRate + DeltaML_AggiuntiRate
                        End If
                        If (exi.Rata.HasValue AndAlso exi.Durata.HasValue) Then

                        End If
                        Dim calculator As New CEstinzioneCalculator()
                        calculator.Rata = Formats.ToValuta(exi.Rata)
                        calculator.Durata = Formats.ToInteger(exi.Durata)
                        calculator.TAN = Formats.ToDouble(exi.TAN)
                        'calculator.setPenaleEstinzione(est.getPenaleEstinzione());
                        'calculator.setSpeseAccessorie(est.getSpeseAccessorie());
                        calculator.NumeroRateResidue = nRate
                        'calculator.setNumeroRateInsolute(Formats.ToInteger(GetFieldValue(this.getName() + "nqs")));
                        Dim Res As Decimal? = calculator.DebitoResiduo

                        If (Res.HasValue) Then
                            sum += Math.Max(0, Res.Value)
                        End If
                    End If
                End If
            Next
            Return sum
        End Function

        Public Function CalcolaProvvTAN(ByVal estinzioni As CCollection(Of EstinzioneXEstintore)) As Double?
            Dim p As CCQSPDProdotto = Me.Prodotto
            If (p Is Nothing) Then Return Nothing
            Return p.CalcolaProvvigioneTAN(Me, estinzioni)
        End Function

        Public Function CalcolaProvvTANE(ByVal estinzioni As CCollection(Of EstinzioneXEstintore)) As Decimal?
            Dim tariffa As CProdottoXTabellaFin = Me.TabellaFinanziariaRel
            Dim tCalcoloPTAN As TipoCalcoloProvvigionale = TipoCalcoloProvvigioni.MONTANTE_LORDO
            If (tariffa IsNot Nothing AndAlso tariffa.Tabella IsNot Nothing AndAlso tariffa.Tabella.Stato = ObjectStatus.OBJECT_VALID) Then
                tCalcoloPTAN = tariffa.Tabella.TipoCalcoloProvvTAN
            End If
            Dim pTAN As Double? = Me.CalcolaProvvTAN(estinzioni)
            Dim pTANE As Decimal? = Nothing
            Dim ml As Decimal? = Me.CalcolaBaseML(estinzioni)
            Dim mlPieno As Decimal? = Me.MontanteLordo
            Select Case (tCalcoloPTAN)
                Case TipoCalcoloProvvigioni.MONTANTE_LORDO
                    If (mlPieno.HasValue AndAlso pTAN.HasValue) Then
                        pTANE = pTAN.Value * mlPieno.Value / 100
                    End If
                Case TipoCalcoloProvvigioni.DELTA_MONTANTE_ESTINZIONI
                    If (ml.HasValue AndAlso pTAN.HasValue) Then
                        pTANE = pTAN.Value * ml.Value / 100
                    End If
                Case Else
                    'TipoCalcoloProvvigioni.FUNZIONE = 2048
            End Select
            Return pTANE
        End Function


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

        Public Overrides Sub InitializeFrom(value As Object)
            MyBase.InitializeFrom(value)
            Dim o As COffertaCQS = value
            Me.m_Provvigioni = New CCQSPDProvvigioneXOffertaCollection(Me)
            For Each p As CCQSPDProvvigioneXOfferta In o.Provvigioni
                Dim p1 As CCQSPDProvvigioneXOfferta = p.Duplicate
                Me.Provvigioni.Add(p1)
                If (GetID(Me) <> 0) Then p1.Save()
            Next
        End Sub

        Public Sub SincronizzaProvvigioni(ByVal estinzioni As CCollection(Of EstinzioneXEstintore))
            Dim p As CCQSPDProdotto = Me.Prodotto
            Dim gp As CGruppoProdotti = Nothing
            Dim provv As CCQSPDProvvigioneXOfferta
            If (p IsNot Nothing) Then gp = p.GruppoProdotti
            If (gp IsNot Nothing) Then
                For Each tp As CCQSPDTipoProvvigione In gp.Provvigioni
                    provv = Me.Provvigioni.GetItemByTipoProvvigione(tp)
                    If (provv Is Nothing AndAlso tp.RispettaVincoli(Me)) Then
                        provv = New CCQSPDProvvigioneXOfferta
                        provv.TipoProvvigione = tp
                        provv.Nome = tp.Nome
                        provv.TipoCalcolo = tp.TipoCalcolo
                        provv.PagataDa = tp.PagataDa
                        provv.PagataA = tp.PagataA
                        provv.Fisso = tp.Fisso
                        provv.Percentuale = tp.Percentuale
                        provv.Formula = tp.Formula
                        If (TestFlag(tp.Flags, CQSPDTipoProvvigioneFlags.Nascosta)) Then
                            provv.Flags = SetFlag(provv.Flags, ProvvigioneXOffertaFlags.Privileged, True)
                        End If
                        For Each v In tp.Vincoli()
                                provv.Vincoli().Add(v)
                            Next
                            Dim keys As String() = tp.Parameters.Keys
                            For Each key As String In keys
                                provv.Parameters.SetItemByKey(key, tp.Parameters.GetItemByKey(key))
                            Next
                            Me.Provvigioni.Add(provv)
                            provv.Stato = ObjectStatus.OBJECT_VALID
                            provv.Save()
                        End If
                Next
            End If

            'Provvigioni collaboratore
            Dim col As CCollaboratore = Me.Collaboratore
            If (col IsNot Nothing) Then
                For Each t As CTrattativaCollaboratore In col.Trattative
                    If (t.Stato = ObjectStatus.OBJECT_VALID AndAlso t.StatoTrattativa = StatoTrattativa.STATO_ACCETTATO AndAlso (t.IDProdotto = 0 OrElse t.IDProdotto = Me.ProdottoID)) Then
                        provv = Me.Provvigioni.GetItemByTrattativaCollaboratore(t)
                        If (provv Is Nothing) Then ' AndAlso t.RispettaVincoli(Me)) Then
                            provv = New CCQSPDProvvigioneXOfferta
                            provv.TrattativaCollaboratore = t
                            provv.Collaboratore = col
                            provv.Nome = t.Nome
                            provv.TipoCalcolo = t.TipoCalcolo
                            provv.PagataDa = CQSPDTipoSoggetto.Agenzia
                            provv.PagataA = CQSPDTipoSoggetto.Collaboratore
                            provv.Fisso = t.ValoreBase
                            provv.Percentuale = t.SpreadApprovato
                            provv.Formula = t.Formula
                            If (TestFlag(t.Flags, TrattativaCollaboratoreFlags.Nascosta)) Then
                                provv.Flags = SetFlag(provv.Flags, ProvvigioneXOffertaFlags.Privileged, True)
                            End If
                            'For Each v In tp.Vincoli()
                            '    provv.Vincoli().Add(v)
                            'Next
                            Dim keys As String() = t.Attributi.Keys
                            For Each key As String In keys
                                provv.Parameters.SetItemByKey(key, t.Attributi.GetItemByKey(key))
                            Next
                            Me.Provvigioni.Add(provv)
                            provv.Stato = ObjectStatus.OBJECT_VALID
                            provv.Save()
                        End If
                    End If
                Next

            End If
        End Sub

    End Class

End Class

Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD

    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Rappresnta un prodotto utilizzabile per il calcolo dei preventivi o per la compilazione dei rapportini
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Public Enum SpeseProdotto As Integer
        SPESE_NESSUNA = 0
        SPESE_FISSE = 1
    End Enum

    Public Enum ProvvigioneErogataDa As Integer
        Direttamente = 0
        BancaFinanziatrice = 1
    End Enum

    Public Enum TipoProdotto As Integer
        PRODOTTO_IN_BANDO = 1
        PRODOTTO_PICCOLE_ATC = 2
        PRODOTTO_ORDINARIO = 2
    End Enum

    <Flags> _
    Public Enum ProdottoFlags As Integer
        None = 0

        ''' <summary>
        ''' Se vero indica che il prodotto è utilizzabile solo se approvato
        ''' </summary>
        ''' <remarks></remarks>
        RequireApprovation = 1

        ''' <summary>
        ''' Nasconde il campo provvigione massima nell'interfaccia del caricamento della pratica/offerta
        ''' </summary>
        ''' <remarks></remarks>
        HidePMax = 2



    End Enum

    <Serializable>
    Public Class CCQSPDProdotto
        Inherits DBObject
        Implements IComparable

        Private m_Visibile As Boolean
        Private m_CessionarioID As Integer
        Private m_Cessionario As CCQSPDCessionarioClass
        Private m_NomeCessionario As String
        Private m_GruppoProdottiID As Integer
        Private m_GruppoProdotti As CGruppoProdotti
        Private m_IdTipoContratto As String
        Private m_IdTipoRapporto As String
        Private m_Categoria As String
        Private m_Nome As String
        Private m_Descrizione As String
        Private m_TipoCalcoloTEG As TEGCalcFlag
        Private m_ProvvigioneMassima As Double
        Private m_InBando As Boolean
        Private m_Assicurazioni As CCollection(Of CAssicurazione)
        Private m_DataInizio As Nullable(Of Date)
        Private m_DataFine As Nullable(Of Date)
        Private m_ProvvigioneErogataDa As ProvvigioneErogataDa '[INT] 0=Direttamente, 1=Dalla banca finanziatrice
        Private m_StatoIniziale As CStatoPratica

        Private m_TabelleFinRelations As CTabelleFinanziarieProdottoCollection 'Collezione contenente la definizione delle relazioni con le tabelle finanziarie
        Private m_TabelleAssRelations As CTabelleAssicurativeProdottoCollection  'Collezione contenente la definizione delle relazioni con le tabelle finanziarie
        Private m_Convenzioni As CProdottoXConvenzioneCollection  'Collezione contenente la definizione delle relazioni con le tabelle finanziarie

        Private m_TipoCalcoloTAEG As TEGCalcFlag
        Private m_IDTabellaSpese As Integer '[INT] ID della tabella spese

        Private m_TabellaSpese As CTabellaSpese '[CTabellaSpese] Oggetto che definisce le spese per il prodotto
        Private m_IDTabellaTAEG As Integer '[INT] ID della tabella utilizzata per i tassi soglia TAEG

        Private m_TabellaTAEG As CTabellaTEGMax          '[CTabellaTEGMax] Tabella utilizzata per i tassi soglia TAEG
        Private m_IDTabellaTEG As Integer '[INT] ID della tabella utilizzata per i tassi soglia TEG

        Private m_TabellaTEG As CTabellaTEGMax '[CTabellaTEGMax] Tabella utilizzata per i tassi soglia TEG
        Private m_IDStatoIniziale As Integer

        Private m_Flags As ProdottoFlags

        Private m_Attributi As CKeyCollection

        Private m_TabelleSpese As CProdottoXTabellaSpesaCollection

        Public Sub New()
            Me.m_TabelleFinRelations = Nothing
            Me.m_TabelleAssRelations = Nothing
            ' Me.m_Banca = Nothing
            Me.m_InBando = False
            Me.m_GruppoProdotti = Nothing
            Me.m_Assicurazioni = Nothing
            Me.m_TipoCalcoloTEG = TEGCalcFlag.CALCOLOTEG_SPESEALL
            Me.m_TipoCalcoloTAEG = TEGCalcFlag.CALCOLOTEG_SPESEALL
            Me.m_IDTabellaSpese = 0
            Me.m_TabellaSpese = Nothing
            Me.m_Convenzioni = Nothing
            Me.m_Visibile = True
            Me.m_CessionarioID = 0
            Me.m_Cessionario = Nothing
            Me.m_GruppoProdottiID = 0
            Me.m_GruppoProdotti = Nothing
            Me.m_DataInizio = Nothing
            Me.m_DataFine = Nothing
            Me.m_ProvvigioneErogataDa = 0
            Me.m_IDTabellaTEG = 0
            Me.m_TabellaTEG = Nothing
            Me.m_IDTabellaTAEG = 0
            Me.m_TabellaTAEG = Nothing
            Me.m_IDStatoIniziale = 0
            Me.m_StatoIniziale = Nothing
            Me.m_Flags = ProdottoFlags.None
            Me.m_Attributi = New CKeyCollection
            Me.m_TabelleSpese = Nothing
        End Sub

        Public ReadOnly Property TabelleSpese As CProdottoXTabellaSpesaCollection
            Get
                SyncLock Me
                    If (Me.m_TabelleSpese Is Nothing) Then Me.m_TabelleSpese = New CProdottoXTabellaSpesaCollection(Me)
                    Return Me.m_TabelleSpese
                End SyncLock
            End Get
        End Property

        Public ReadOnly Property Attributi As CKeyCollection
            Get
                Return Me.m_Attributi
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta dei flags per il prodotto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Flags As ProdottoFlags
            Get
                Return Me.m_Flags
            End Get
            Set(value As ProdottoFlags)
                Dim oldValue As ProdottoFlags = Me.m_Flags
                If (oldValue = value) Then Exit Property
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta lo stato iniziale da cui partono le pratiche caricate con questo prodotto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StatoIniziale As CStatoPratica
            Get
                If (Me.m_StatoIniziale Is Nothing) Then Me.m_StatoIniziale = DMD.CQSPD.StatiPratica.GetItemById(Me.m_IDStatoIniziale)
                Return Me.m_StatoIniziale
            End Get
            Set(value As CStatoPratica)
                Dim oldValue As CStatoPratica = Me.m_StatoIniziale
                If (oldValue Is value) Then Exit Property
                Me.m_StatoIniziale = value
                Me.m_IDStatoIniziale = GetID(value)
                Me.DoChanged("StatoIniziale", value, oldValue)
            End Set
        End Property

        Public Function GetStatoIniziale() As CStatoPratica
            If (Me.StatoIniziale IsNot Nothing) Then
                Return Me.StatoIniziale
            ElseIf (Me.GruppoProdotti IsNot Nothing) Then
                Return Me.GruppoProdotti.StatoIniziale
            Else
                Return Nothing
            End If
        End Function

        Public Property IDStatoIniziale As Integer
            Get
                Return GetID(Me.m_StatoIniziale, Me.m_IDStatoIniziale)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDStatoIniziale
                If (oldValue = value) Then Exit Property
                Me.m_IDStatoIniziale = value
                Me.m_StatoIniziale = Nothing
                Me.DoChanged("IDStatoIniziale", value, oldValue)
            End Set
        End Property

        Public Overrides Function GetModule() As CModule
            Return DMD.CQSPD.Prodotti.Module
        End Function

        ''' <summary>
        ''' Restituisce o imposta l'ID della tabella utilizzata per il calcolo del TEG massimo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDTabellaTEG As Integer
            Get
                Return GetID(Me.m_TabellaTEG, Me.m_IDTabellaTEG)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDTabellaTEG
                If oldValue = value Then Exit Property
                Me.m_IDTabellaTEG = value
                Me.m_TabellaTEG = Nothing
                Me.DoChanged("IDTabellaTEG", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'oggetto tabella utilizzata per il calcolo del TEG massimo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TabellaTEG As CTabellaTEGMax
            Get
                If (Me.m_TabellaTEG Is Nothing) Then Me.m_TabellaTEG = DMD.CQSPD.TabelleTEGMax.GetItemById(Me.m_IDTabellaTEG)
                Return Me.m_TabellaTEG
            End Get
            Set(value As CTabellaTEGMax)
                Dim oldValue As CTabellaTEGMax = Me.TabellaTEG
                If (oldValue = value) Then Exit Property
                Me.m_TabellaTEG = value
                Me.m_IDTabellaTEG = GetID(value)
                Me.DoChanged("TabellaTEG", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID della tabella utilizzata per il calcolo del TAEG massimo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDTabellaTAEG As Integer
            Get
                Return GetID(Me.m_TabellaTAEG, Me.m_IDTabellaTAEG)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDTabellaTAEG
                If oldValue = value Then Exit Property
                Me.m_IDTabellaTAEG = value
                Me.m_TabellaTAEG = Nothing
                Me.DoChanged("IDTabellaTAEG", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'oggetto tabella utilizzata per il calcolo del TAEG massimo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TabellaTAEG As CTabellaTEGMax
            Get
                If (Me.m_TabellaTAEG Is Nothing) Then Me.m_TabellaTAEG = DMD.CQSPD.TabelleTEGMax.GetItemById(Me.m_IDTabellaTAEG)
                Return Me.m_TabellaTAEG
            End Get
            Set(value As CTabellaTEGMax)
                Dim oldValue As CTabellaTEGMax = Me.TabellaTAEG
                If (oldValue = value) Then Exit Property
                Me.m_TabellaTAEG = value
                Me.m_IDTabellaTAEG = GetID(value)
                Me.DoChanged("TabellaTAEG", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID della tabella utilizzata per il calcolo delle spese 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDTabellaSpese As Integer
            Get
                Return GetID(Me.m_TabellaSpese, Me.m_IDTabellaSpese)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDTabellaSpese
                If (value = oldValue) Then Exit Property
                Me.m_IDTabellaSpese = value
                Me.m_TabellaSpese = Nothing
                Me.DoChanged("IDTabellaSpese", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'oggetto tabella utilizzata per il calcolo del TEG massimo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TabellaSpese As CTabellaSpese
            Get
                If Me.m_TabellaSpese Is Nothing Then Me.m_TabellaSpese = DMD.CQSPD.TabelleSpese.GetItemById(Me.m_IDTabellaSpese)
                Return Me.m_TabellaSpese
            End Get
            Set(value As CTabellaSpese)
                Dim oldValue As CTabellaSpese = Me.TabellaSpese
                If (oldValue = value) Then Exit Property
                Me.m_TabellaSpese = value
                Me.m_IDTabellaSpese = GetID(value)
                Me.DoChanged("TabellaSpese", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un intero che indica le spese da includere nel calcolo del TAEG
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TipoCalcoloTAEG As TipoCalcoloTEG
            Get
                Return Me.m_TipoCalcoloTAEG
            End Get
            Set(value As TipoCalcoloTEG)
                Dim oldValue As TipoCalcoloTEG = Me.m_TipoCalcoloTAEG
                If (oldValue = value) Then Exit Property
                Me.m_TipoCalcoloTAEG = value
                Me.DoChanged("TipoCalcoloTAEG", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore intero che indica chi eroga la provvigione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ProvvigioneErogataDa As ProvvigioneErogataDa
            Get
                Return Me.m_ProvvigioneErogataDa
            End Get
            Set(value As ProvvigioneErogataDa)
                Dim oldValue As ProvvigioneErogataDa = Me.m_ProvvigioneErogataDa
                If (oldValue = value) Then Exit Property
                Me.m_ProvvigioneErogataDa = value
                Me.DoChanged("ProvvigioneErogataDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID del cessionario che eroga il prodotto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CessionarioID As Integer
            Get
                Return GetID(Me.m_Cessionario, Me.m_CessionarioID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.CessionarioID
                If oldValue = value Then Exit Property
                Me.m_CessionarioID = value
                Me.m_Cessionario = Nothing
                Me.DoChanged("CessionarioID", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'oggetto Cessionario che eroga il prodotto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
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

        ''' <summary>
        ''' Restituisce o imposta il nome del cessionario che eroga il prodotto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
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

        ''' <summary>
        ''' Restituisce o imposta l'ID del gruppo di prodotti
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property GruppoProdottiID As Integer
            Get
                Return GetID(Me.m_GruppoProdotti, Me.m_GruppoProdottiID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.GruppoProdottiID
                If oldValue = value Then Exit Property
                Me.m_GruppoProdottiID = value
                Me.m_GruppoProdotti = Nothing
                Me.DoChanged("GruppoProdottiID", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'oggetto GruppoProdotti
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property GruppoProdotti As CGruppoProdotti
            Get
                If (Me.m_GruppoProdotti Is Nothing) Then Me.m_GruppoProdotti = DMD.CQSPD.GruppiProdotto.GetItemById(Me.m_GruppoProdottiID)
                Return Me.m_GruppoProdotti
            End Get
            Set(value As CGruppoProdotti)
                Dim oldValue As CGruppoProdotti = Me.GruppoProdotti
                If (oldValue = value) Then Exit Property
                Me.m_GruppoProdotti = value
                Me.m_GruppoProdottiID = GetID(value)
                Me.DoChanged("GruppoProdotti", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se il prodotto è visibile nell'area preventivi 	
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Visibile As Boolean
            Get
                Return Me.m_Visibile
            End Get
            Set(value As Boolean)
                If Me.m_Visibile = value Then Exit Property
                Me.m_Visibile = value
                Me.DoChanged("Visibile", value, Not value)
            End Set
        End Property

        '      'Restituisce una collezione di relazioni tra il prodotto e le tabelle finanziarie
        '      Property Get TabelleFinanziarieCorrelate
        '      If (m_TabelleFinanziarieCorrelate Is Nothing) Then
        '          Set m_TabelleFinanziarieCorrelate = New CProdottoXTabelleFinanziarieCorrelate
        '          Call m_TabelleFinanziarieCorrelate.Initialize(Me)
        '      End If
        '      Set TabelleFinanziarieCorrelate = m_TabelleFinanziarieCorrelate
        'End Property

        ' Restituisce un oggetto CTabelleFinanziarieProdottoCollection contenete la definizione
        ' delle relazioni tra questo prodotto e le tabelle finanziarie. Per ogni relazione
        ' sono specificati anche i vincoli di validità
        Public ReadOnly Property TabelleFinanziarieRelations As CTabelleFinanziarieProdottoCollection
            Get
                If (Me.m_TabelleFinRelations Is Nothing) Then Me.m_TabelleFinRelations = New CTabelleFinanziarieProdottoCollection(Me)
                Return Me.m_TabelleFinRelations
            End Get
        End Property

        '      'Restituisce una collezione di relazioni tra il prodotto e le trie di tabelle assicurative (vita, impiego, credito) 
        '  Property Get TabelleAssicurativeCorrelate
        '      If (m_TabelleAssicurativeCorrelate Is Nothing) Then
        '          Set m_TabelleAssicurtiveCorrelate = New CProdottoXTabelleAssicurativeCorrelate
        '          Call m_TabelleAssicurativeCorrelate.Initialize(Me)
        '      End If
        '      Set TabelleAssicurativeCorrelate = m_TabelleAssicurativeCorrelate
        'End Property

        ' Restituisce un oggetto CTabelleAssicurativeProdottoCollection contenete la definizione
        ' delle relazioni tra questo prodotto e le triple di tabelle assicurative (vita, impiego, crdito). Per ogni relazione
        ' sono specificati anche i vincoli di validità
        Public ReadOnly Property TabelleAssicurativeRelations As CTabelleAssicurativeProdottoCollection
            Get
                SyncLock Me
                    If (Me.m_TabelleAssRelations Is Nothing) Then Me.m_TabelleAssRelations = New CTabelleAssicurativeProdottoCollection(Me)
                    Return Me.m_TabelleAssRelations
                End SyncLock
            End Get
        End Property

        Public ReadOnly Property Convenzioni As CProdottoXConvenzioneCollection
            Get
                If (Me.m_Convenzioni Is Nothing) Then Me.m_Convenzioni = New CProdottoXConvenzioneCollection(Me)
                Return Me.m_Convenzioni
            End Get
        End Property

        Friend Sub InvalidateConvenzioni()
            Me.m_Convenzioni = Nothing
        End Sub

        '      '
        'Property Get Assicurazioni
        '	If (m_Assicurazioni Is Nothing) Then
        '		Set m_Assicurazioni = New CFsbPrev_Assicurazioni
        '		Call m_Assicurazioni.Initialize(Me)
        '	End If
        '	Set Assicurazioni = m_Assicurazioni
        'End Property

        ''' <summary>
        ''' Restituisce o imposta il massimo caricabile per il prodotto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ProvvigioneMassima As Double
            Get
                Return Me.m_ProvvigioneMassima
            End Get
            Set(value As Double)
                Dim oldValue As Double = Me.m_ProvvigioneMassima
                If (oldValue = value) Then Exit Property
                Me.m_ProvvigioneMassima = value
                Me.DoChanged("ProvvigioneMassima", value, oldValue)
            End Set
        End Property

        Public Function CalcolaProvvigioneMassima(ByVal o As COffertaCQS, ByVal estinzioni As CCollection(Of EstinzioneXEstintore)) As Nullable(Of Double)
            Dim tblFin As CProdottoXTabellaFin = Nothing
            If o.TabellaFinanziariaRel IsNot Nothing Then
                tblFin = o.TabellaFinanziariaRel
            ElseIf Me.TabelleFinanziarieRelations.Count > 0 Then
                tblFin = Me.TabelleFinanziarieRelations(0)
            End If

            Dim isRinnovo As Boolean = False
            Dim isEstinzione As Boolean = False
            For Each e As EstinzioneXEstintore In estinzioni
                If (e.Selezionata) Then
                    isEstinzione = True
                    If (e.Stato = ObjectStatus.OBJECT_VALID AndAlso e.NomeCessionario = o.NomeCessionario) Then
                        isRinnovo = True
                    End If
                End If
            Next

            If (tblFin IsNot Nothing AndAlso tblFin.Tabella IsNot Nothing) Then
                If (isRinnovo) Then
                    Return tblFin.Tabella.ProvvMaxConRinnovi
                ElseIf (isEstinzione) Then
                    Return tblFin.Tabella.ProvvMaxConEstinzioni
                Else
                    Return tblFin.Tabella.ProvvMax
                End If
            Else
                Return Nothing
            End If
        End Function

        Public Function CalcolaProvvigioneTAN(ByVal o As COffertaCQS, ByVal estinzioni As CCollection(Of EstinzioneXEstintore)) As Double?
            Dim tblFin As CProdottoXTabellaFin = Nothing
            If (o.TabellaFinanziariaRel IsNot Nothing) Then
                tblFin = o.TabellaFinanziariaRel
            ElseIf (Me.TabelleFinanziarieRelations().Count() > 0) Then
                tblFin = Me.TabelleFinanziarieRelations(0)
            End If

            Dim isRinnovo As Boolean = False
            Dim isEstinzione As Boolean = False
            For i As Integer = 0 To estinzioni.Count - 1
                Dim e As EstinzioneXEstintore = estinzioni(i)
                If (e.Selezionata) Then
                    isEstinzione = True
                    If (e.Stato = ObjectStatus.OBJECT_VALID AndAlso e.NomeCessionario = o.NomeCessionario) Then
                        isRinnovo = True
                    End If
                End If
            Next
            Dim pTAN As Double? = Nothing
            Dim tabella As CTabellaFinanziaria = Nothing
            If (tblFin IsNot Nothing) Then
                tabella = tblFin.Tabella
            End If

            If (tabella IsNot Nothing) Then
                If (isRinnovo) Then
                    pTAN = tabella.ProvvTANR
                ElseIf (isEstinzione) Then
                    pTAN = tabella.ProvvTANE
                Else
                    pTAN = tabella.ScontoVisibile
                End If
            End If

            Return pTAN
        End Function


        Public Function CalcolaRappel(ByVal o As COffertaCQS, ByVal estinzioni As CCollection(Of EstinzioneXEstintore)) As Double?
            Dim tblFin As CProdottoXTabellaFin = Nothing
            If (o.TabellaFinanziariaRel IsNot Nothing) Then
                tblFin = o.TabellaFinanziariaRel
            ElseIf (Me.TabelleFinanziarieRelations.Count() > 0) Then
                tblFin = Me.TabelleFinanziarieRelations(0)
            End If
            Dim isRinnovo As Boolean = False
            Dim isEstinzione As Boolean = False
            If (estinzioni IsNot Nothing) Then
                For i As Integer = 0 To estinzioni.Count() - 1
                    Dim e As EstinzioneXEstintore = estinzioni(i)
                    If (e.Selezionata) Then
                        isEstinzione = True
                        If (e.Stato = ObjectStatus.OBJECT_VALID AndAlso e.NomeCessionario = o.NomeCessionario) Then
                            isRinnovo = True
                        End If
                    End If
                Next
            End If

            Dim tariffa As CTabellaFinanziaria = Nothing
            If (tblFin IsNot Nothing) Then
                tariffa = tblFin.Tabella
            End If

            Dim spese As CTabellaSpese = Me.TabellaSpese
            Dim rappel As Double? = Nothing

            If (tariffa IsNot Nothing AndAlso TestFlag(tariffa.Flags, TabellaFinanziariaFlags.ForzaRappel)) Then
                rappel = tariffa.Sconto
            ElseIf (spese IsNot Nothing AndAlso o.Durata > 0) Then
                rappel = spese.Rappel(o.Durata)
            End If

            Return rappel
        End Function

        Public Function CalcolaRiduzioneProvvigionale(ByVal o As COffertaCQS, ByVal estinzioni As CCollection(Of EstinzioneXEstintore)) As Double?
            Dim tblFin As CProdottoXTabellaFin = Nothing
            If (o.TabellaFinanziariaRel IsNot Nothing) Then
                tblFin = o.TabellaFinanziariaRel
            ElseIf (Me.TabelleFinanziarieRelations.Count() > 0) Then
                tblFin = Me.TabelleFinanziarieRelations(0)
            End If
            Dim isRinnovo As Boolean = False
            Dim isEstinzione As Boolean = False
            If (estinzioni IsNot Nothing) Then
                For i As Integer = 0 To estinzioni.Count - 1
                    Dim e As EstinzioneXEstintore = estinzioni(i)
                    If (e.Selezionata) Then
                        isEstinzione = True
                        If (e.Stato = ObjectStatus.OBJECT_VALID AndAlso e.NomeCessionario = o.NomeCessionario) Then
                            isRinnovo = True
                        End If
                    End If
                Next
            End If

            Dim tariffa As CTabellaFinanziaria = Nothing
            If (tblFin IsNot Nothing) Then tariffa = tblFin.Tabella
            If (tariffa IsNot Nothing) Then
                If (isRinnovo) Then
                    Return tariffa.ProvvMaxConRinnovi
                ElseIf (isEstinzione) Then
                    Return tariffa.ProvvMaxConEstinzioni
                End If
            End If

            Return 0.0
        End Function



        ''' <summary>
        ''' Restituisce o imposta una stringa che indica il tipo di contratto a cui appartiene il prodotto  
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IdTipoContratto As String
            Get
                Return Me.m_IdTipoContratto
            End Get
            Set(value As String)
                value = UCase(Left(Trim(value), 1))
                Dim oldValue As String = Me.m_IdTipoContratto
                If (oldValue = value) Then Exit Property
                Me.m_IdTipoContratto = value
                Me.DoChanged("IdTipoContratto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta una stringa che indica il tipo di rapporto a cui appartiene il prodotto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IdTipoRapporto As String
            Get
                Return Me.m_IdTipoRapporto
            End Get
            Set(value As String)
                value = UCase(Left(Trim(value), 1))
                Dim oldValue As String = Me.m_IdTipoRapporto
                If (oldValue = value) Then Exit Property
                Me.m_IdTipoRapporto = value
                Me.DoChanged("IdTipoRapporto", value, oldValue)
            End Set
        End Property

        '	Property Get TipoContratto
        '		If (m_TipoContratto Is Nothing) Then Set m_TipoContratto = FsbPrev.GetTipoContrattoById(m_IdTipoContratto)
        '		Set TipoContratto = m_TipoContratto
        '	End Property
        '	Property Set TipoContratto(value)
        '		Set m_TipoContratto = value
        '               m_IdTipoContratto = value.ID
        '	End Property

        'Property Get TipoRapporto
        '	If (m_TipoRapporto Is Nothing) Then Set m_TipoRapporto = FsbPrev.GetTipoRapportoById(m_IdTipoRapporto)
        '	Set TipoRapporto = m_TipoRapporto
        'End Property
        'Property Set TipoRapporto(value)
        '	Set m_TipoRapporto = value
        '               m_IdTipoRapporto = value.ID
        'End Property

        Public Property Categoria As String
            Get
                Return Me.m_Categoria
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Categoria
                If (oldValue = value) Then Exit Property
                Me.m_Categoria = value
                Me.DoChanged("Categoria", value, oldValue)
            End Set
        End Property

        'Property Get Banca
        '	If (m_Banca Is Nothing) Then Set m_Banca = FsbPrev.GetBancaById(m_IDBanca)
        '	Set Banca = m_Banca
        'End Property
        'Property Set Banca(value)
        '	Set m_Banca = value
        '               m_IDBanca = Databases.GetID(value, 0)
        'End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del prodotto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Nome As String
            Get
                Return Me.m_Nome
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Nome
                If (oldValue = value) Then Exit Property
                Me.m_Nome = value
                Me.DoChanged("Nome", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la descrizione del prodotto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Descrizione As String
            Get
                Return Me.m_Descrizione
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Descrizione
                If (oldValue = value) Then Exit Property
                Me.m_Descrizione = value
                Me.DoChanged("Descrizione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore intero che indica le spese incluse nel calcolo del TEG
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TipoCalcoloTeg As TipoCalcoloTEG
            Get
                Return Me.m_TipoCalcoloTEG
            End Get
            Set(value As TipoCalcoloTEG)
                Dim oldValue As TipoCalcoloTEG = Me.m_TipoCalcoloTEG
                If (oldValue = value) Then Exit Property
                Me.m_TipoCalcoloTEG = value
                Me.DoChanged("TipoCalcoloTeg", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce il TEG massimo per l'offerta
        ''' </summary>
        ''' <param name="offerta"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetMaxTEG(ByVal offerta As COffertaCQS) As Double
            If Not (Me.TabellaTEG Is Nothing) Then
                Return Me.TabellaTEG.Calculate(offerta)
            Else
                GetMaxTEG = 0
            End If
        End Function

        ''' <summary>
        ''' Restituisce il TAEG massimo per l'offerta
        ''' </summary>
        ''' <param name="offerta"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetMaxTAEG(ByVal offerta As COffertaCQS) As Double
            If Not (Me.TabellaTAEG Is Nothing) Then
                Return Me.TabellaTAEG.Calculate(offerta)
            Else
                Return 0
            End If
        End Function

        ''' <summary>
        ''' Restituisce o imposta la data di inizio validità del prodotto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataInizio As Nullable(Of Date)
            Get
                Return Me.m_DataInizio
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataInizio
                If (oldValue = value) Then Exit Property
                Me.m_DataInizio = value
                Me.DoChanged("DataInizio", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di fine validità del prodotto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataFine As Nullable(Of Date)
            Get
                Return Me.m_DataFine
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataFine
                If (oldValue = value) Then Exit Property
                Me.m_DataFine = value
                Me.DoChanged("DataFine", value, oldValue)
            End Set
        End Property

        Public Function IsValid() As Boolean
            Return Me.IsValid(DateUtils.Now)
        End Function

        Public Function IsValid(ByVal al As Date) As Boolean
            Return DateUtils.CheckBetween(al, Me.m_DataInizio, Me.m_DataFine)
        End Function

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se le pratiche 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property RichiedeApprovazione As Boolean
            Get
                Return TestFlag(Me.m_Flags, ProdottoFlags.RequireApprovation)
            End Get
            Set(value As Boolean)
                If (Me.RichiedeApprovazione = value) Then Exit Property
                Me.m_Flags = SetFlag(Me.m_Flags, ProdottoFlags.RequireApprovation, value)
                Me.DoChanged("RichiedeApprovazione", value, Not value)
            End Set
        End Property

        Public Overrides Function IsChanged() As Boolean
            Dim ret As Boolean = MyBase.IsChanged()
            If Not ret AndAlso Me.m_TabelleFinRelations IsNot Nothing Then ret = DBUtils.IsChanged(Me.m_TabelleFinRelations)
            If Not ret AndAlso Me.m_TabelleAssRelations IsNot Nothing Then ret = DBUtils.IsChanged(Me.m_TabelleAssRelations)
            Return ret
        End Function

        Protected Overrides Function SaveToDatabase(dbConn As CDBConnection, ByVal force As Boolean) As Boolean
            Dim ret As Boolean = MyBase.SaveToDatabase(dbConn, force)
            If ret And Not Me.m_TabelleFinRelations Is Nothing Then Me.m_TabelleFinRelations.Save(force)
            If ret And Not Me.m_TabelleAssRelations Is Nothing Then Me.m_TabelleAssRelations.Save(force)
            CQSPD.Prodotti.UpdateCached(Me)
            Return ret
        End Function

        Protected Overrides Function DropFromDatabase(dbConn As CDBConnection, ByVal force As Boolean) As Boolean
            Me.TabelleFinanziarieRelations.Delete(force)
            Me.TabelleAssicurativeRelations.Delete(force)
            Return MyBase.DropFromDatabase(dbConn, force)
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_Prodotti"
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_CessionarioID = reader.Read("Cessionario", Me.m_CessionarioID)
            Me.m_NomeCessionario = reader.Read("NomeCessionario", Me.m_NomeCessionario)
            Me.m_GruppoProdottiID = reader.Read("GruppoProdotti", Me.m_GruppoProdottiID)
            Me.m_IdTipoContratto = reader.Read("IdTipoContratto", Me.m_IdTipoContratto)
            Me.m_IdTipoRapporto = reader.Read("IdTipoRapporto", Me.m_IdTipoRapporto)
            Me.m_Categoria = reader.Read("Idcategoria", Me.m_Categoria)
            'reader.Read("IDBanca", Me.m_IDBanca)
            Me.m_Nome = reader.Read("nome", Me.m_Nome)
            Me.m_Descrizione = reader.Read("descrizione", Me.m_Descrizione)
            Me.m_TipoCalcoloTEG = reader.Read("TipoCalcoloTeg", Me.m_TipoCalcoloTEG)
            Me.m_TipoCalcoloTAEG = reader.Read("TipoCalcoloTAEG", Me.m_TipoCalcoloTAEG)
            Me.m_Visibile = reader.Read("Visibile", Me.m_Visibile)
            Me.m_IDTabellaSpese = reader.Read("IDTabellaSpese", Me.m_IDTabellaSpese)
            Me.m_IDTabellaTEG = reader.Read("IDTabellaTEG", Me.m_IDTabellaTEG)
            Me.m_IDTabellaTAEG = reader.Read("IDTabellaTAEG", Me.m_IDTabellaTAEG)
            Me.m_DataInizio = reader.Read("DataInizio", Me.m_DataInizio)
            Me.m_DataFine = reader.Read("DataFine", Me.m_DataFine)
            Me.m_ProvvigioneErogataDa = reader.Read("ProvvigioneErogataDa", Me.m_ProvvigioneErogataDa)
            Me.m_IDStatoIniziale = reader.Read("IDStatoIniziale", Me.m_IDStatoIniziale)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)

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

            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Visibile", Me.m_Visibile)
            writer.Write("Cessionario", GetID(Me.m_Cessionario, Me.m_CessionarioID))
            writer.Write("NomeCessionario", Me.m_NomeCessionario)
            writer.Write("GruppoProdotti", GetID(Me.m_GruppoProdotti, Me.m_GruppoProdottiID))
            writer.Write("IDTabellaSpese", Me.IDTabellaSpese)
            writer.Write("IDTabellaTEG", Me.IDTabellaTEG)
            writer.Write("IDTabellaTAEG", Me.IDTabellaTAEG)
            writer.Write("IdTipoContratto", Me.m_IdTipoContratto)
            writer.Write("IdTipoRapporto", Me.m_IdTipoRapporto)
            writer.Write("Idcategoria", Me.m_Categoria)
            'writer.Write("IDBanca", GetID(Me.m_Banca, Me.m_IDBanca))
            writer.Write("nome", Me.m_Nome)
            writer.Write("descrizione", Me.m_Descrizione)
            writer.Write("TipoCalcoloTeg", Me.m_TipoCalcoloTEG)
            writer.Write("TipoCalcolotaeg", Me.m_TipoCalcoloTAEG)
            writer.Write("DataInizio", Me.m_DataInizio)
            writer.Write("DataFine", Me.m_DataFine)
            writer.Write("ProvvigioneErogataDa", Me.m_ProvvigioneErogataDa)
            writer.Write("IDStatoIniziale", Me.IDStatoIniziale)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("Attributi", XML.Utils.Serializer.Serialize(Me.Attributi))
            Return MyBase.SaveToRecordset(writer)
        End Function

        Public Overrides Function ToString() As String
            Return Me.m_Nome
        End Function




        Public Function CompareTo(ByVal item As CCQSPDProdotto) As Integer
            Return Strings.Compare(Me.Nome, item.Nome, CompareMethod.Text)
        End Function

        Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(obj)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Visibile", Me.m_Visibile)
            writer.WriteAttribute("CessionarioID", Me.CessionarioID)
            writer.WriteAttribute("NomeCessionario", Me.m_NomeCessionario)
            writer.WriteAttribute("GruppoProdottiID", Me.GruppoProdottiID)
            writer.WriteAttribute("IdTipoContratto", Me.m_IdTipoContratto)
            writer.WriteAttribute("IdTipoRapporto", Me.m_IdTipoRapporto)
            writer.WriteAttribute("Categoria", Me.m_Categoria)
            ' writer.WriteTag("IDBanca", Me.m_IDBanca)
            writer.WriteAttribute("Nome", Me.m_Nome)
            writer.WriteAttribute("Descrizione", Me.m_Descrizione)
            writer.WriteAttribute("TipoCalcoloTEG", Me.m_TipoCalcoloTEG)
            writer.WriteAttribute("ProvvigioneMassima", Me.m_ProvvigioneMassima)
            writer.WriteAttribute("InBando", Me.m_InBando)
            writer.WriteAttribute("DataInizio", Me.m_DataInizio)
            writer.WriteAttribute("DataFine", Me.m_DataFine)
            writer.WriteAttribute("ProvvigioneErogataDa", Me.m_ProvvigioneErogataDa)
            writer.WriteAttribute("TipoCalcoloTAEG", Me.m_TipoCalcoloTAEG)
            writer.WriteAttribute("IDTabellaSpese", Me.IDTabellaSpese)
            writer.WriteAttribute("IDTabellaTAEG", Me.IDTabellaTAEG)
            writer.WriteAttribute("IDTabellaTEG", Me.IDTabellaTEG)
            writer.WriteAttribute("IDStatoIniziale", Me.IDStatoIniziale)
            writer.WriteAttribute("Flags", Me.m_Flags)
            MyBase.XMLSerialize(writer)
            'writer.WriteTag("TabelleFinanziarieRelations", Me.TabelleFinanziarieRelations)
            'writer.WriteTag("TabelleAssicurativeRelations", Me.TabelleAssicurativeRelations)
            'writer.WriteTag("TabelleSpese", Me.TabelleSpese)
            writer.WriteTag("Attributi", Me.Attributi)

        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Visibile" : Me.m_Visibile = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "CessionarioID" : Me.m_CessionarioID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeCessionario" : Me.m_NomeCessionario = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "GruppoProdottiID" : Me.m_GruppoProdottiID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IdTipoContratto" : Me.m_IdTipoContratto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IdTipoRapporto" : Me.m_IdTipoRapporto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Categoria" : Me.m_Categoria = XML.Utils.Serializer.DeserializeString(fieldValue)
                    ' Case "IDBanca" : Me.m_IDBanca = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Nome" : Me.m_Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Descrizione" : Me.m_Descrizione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "TipoCalcoloTEG" : Me.m_TipoCalcoloTEG = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "ProvvigioneMassima" : Me.m_ProvvigioneMassima = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "InBando" : Me.m_InBando = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "DataInizio" : Me.m_DataInizio = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataFine" : Me.m_DataFine = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "ProvvigioneErogataDa" : Me.m_ProvvigioneErogataDa = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "TipoCalcoloTAEG" : Me.m_TipoCalcoloTAEG = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDTabellaSpese" : Me.m_IDTabellaSpese = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDTabellaTAEG" : Me.m_IDTabellaTAEG = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDTabellaTEG" : Me.m_IDTabellaTEG = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDStatoIniziale" : Me.m_IDStatoIniziale = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Attributi" : Me.m_Attributi = fieldValue
                Case "TabelleFinanziarieRelations"
                    Me.m_TabelleFinRelations = New CTabelleFinanziarieProdottoCollection
                    Me.m_TabelleFinRelations.SetProdotto(Me)
                    Me.m_TabelleFinRelations.AddRange(fieldValue)
                Case "TabelleAssicurativeRelations"
                    Me.m_TabelleAssRelations = New CTabelleAssicurativeProdottoCollection
                    Me.m_TabelleAssRelations.SetProdotto(Me)
                    Me.m_TabelleAssRelations.AddRange(fieldValue)
                Case "TabelleSpese"
                    Me.m_TabelleSpese = New CProdottoXTabellaSpesaCollection
                    Me.m_TabelleSpese.SetProdotto(Me)
                    Me.m_TabelleSpese.AddRange(fieldValue)
                Case "Convenzioni"
                    Me.m_Convenzioni = New CProdottoXConvenzioneCollection
                    Me.m_Convenzioni.SetProdotto(Me)
                    Me.m_Convenzioni.AddRange(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select

        End Sub

        Public Sub InvalidateTabelleSpese()
            SyncLock Me
                Me.m_TabelleSpese = Nothing
            End SyncLock
        End Sub

        Public Sub InvalidateAssicurazioni()
            SyncLock Me
                Me.m_TabelleAssRelations = Nothing
            End SyncLock
        End Sub

        Public Overrides Sub InitializeFrom(value As Object)
            MyBase.InitializeFrom(value)
            Me.m_TabelleAssRelations = Nothing
            Me.m_TabelleFinRelations = Nothing
            Me.m_TabelleSpese = Nothing
            Me.m_Convenzioni = Nothing
        End Sub
    End Class



End Class

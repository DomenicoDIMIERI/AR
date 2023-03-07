Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica


Partial Public Class CQSPD

    Public Enum SortPreventivoBy As Integer
        ''' <summary>
        ''' Visualizza in alto le offerte che favoriscono il cliente
        ''' </summary>
        ''' <remarks></remarks>
        SORTBY_NETTO = 0

        ''' <summary>
        ''' Visualizza in alto le offerte che favoriscono il guadagno aziendale
        ''' </summary>
        ''' <remarks></remarks>
        SORTBY_RICARICOTOTALE = 1
    End Enum

    ''' <summary>
    ''' Preventivatore comparativo
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CPreventivo
        Inherits DBObjectPO
        Implements IComparer

        Private m_Nome As String '[TEXT] Nome del cliente
        Private m_Cognome As String '[TEXT] Cognome del cliente
        Private m_Sesso As String '[TEXT*1] M o F : Sesso del cliente
        Private m_DataNascita As Nullable(Of Date) '[DATE] Data di nascita del cliente
        Private m_NatoAComune As String '[TEXT] Comune di nascita del cliente
        Private m_NatoAProvincia As String '[TEXT] Provincia di nascita del cliente
        Private m_CodiceFiscale As String '[TEXT] Codice fiscale del cliente
        Private m_PartitaIVA As String '[TEXT] Partita IVA del cliente

        Private m_DataAssunzione As Nullable(Of Date) '[DATE] Data di assunzione del cliente
        Private m_DataDecorrenza As Nullable(Of Date) '[DATE] Data di decorrenza del preventivo

        Private m_TipoContratto As String   '[TEXT*1] ID del tipo contratto
        'Private m_TipoContrattoEx As TipoContratto '[CCQSPDTipoContratto] Oggetto tipo contratto
        Private m_TipoRapporto As String '[TEXT*1] ID del tipo rapporto
        'Private m_TipoRapportoEx As TipoRapporto '[CCQSPDTipoRapporto] Oggetto tipo rapporto
        Private m_Durata As Integer '[INT]  Numero di mesi della durata del finanziamento
        Private m_Rata As Decimal '[DOUBLE] Rata mensile
        Private m_Provvigionale As Double '[DOUBLE] Percentuale di provvigione sul montante lordo
        Private m_CaricaAlMassimo As Boolean  '[BOOL] Se vero calcola la provvigione massima applicabile per ogni offerta
        Private m_ProfiloID As Integer '[INT]  ID del profilo di preventivazione utilizzato
        Private m_Profilo As CProfilo '[CCQSPDListino] Profilo di preventivazione utilizzato
        Private m_NomeProfilo As String   '[TEXT] Nome del profilo di preventivazione utilizzato
        Private m_SortBy As SortPreventivoBy '[INT] SORTBY_NETTO visualizza per prima le offerte più vantaggiose per il cliente, SORTBY_RICARICOTOTALE visualizza per prime le offerte più vantaggiose per l'azienda
        Private m_StipendioLordo As Decimal '[DOUBLE] Stipendio lordo percepito dal cliente    
        Private m_StipendioNetto As Decimal '[DOUBLE] Stipendio netto percepito dal cliente
        Private m_NumeroMensilita As Integer '[INT] Numero di mensilità
        Private m_TFR As Decimal '[DOUBLE] TFR maturato dal cliente
        Private m_AmministrazioneID As Integer '[INT] ID dell'amministrazione per cui lavora il cliente
        Private m_Amministrazione As CAzienda '[CAzienda] Oggetto ammnistrazione per cui lavora il cliente
        Private m_eMail As String '[TEXT] e-mail del cliente
        Private m_Telefono As String '[TEXT] Telefono del cliente
        Private m_Fax As String '[TEXT] Fax del cliente

        Private m_Offerte As CCQSPDOfferte '[CCQSPDOfferte] Oggetto che racchiude le offerte calcolate per questo preventivo

        Public Sub New()
            Me.m_Nome = ""
            Me.m_Cognome = ""
            Me.m_Sesso = ""
            Me.m_DataNascita = DateSerial(Year(Now) - 18, 1, 1)
            Me.m_NatoAComune = ""
            Me.m_NatoAProvincia = ""
            Me.m_CodiceFiscale = ""
            Me.m_PartitaIVA = ""
            Me.m_DataAssunzione = DateSerial(Year(Now), 1, 1)
            Me.m_DataDecorrenza = Calendar.GetNextMonthFirstDay(Now)
            If Day(Now) >= 15 Then Me.m_DataDecorrenza = Calendar.GetNextMonthFirstDay(Me.m_DataDecorrenza)
            Me.m_TipoContratto = "C"
            'Me.m_TipoContrattoEx = Nothing
            Me.m_TipoRapporto = ""
            'Me.m_TipoRapportoEx = Nothing
            Me.m_Durata = 120
            Me.m_Rata = 0
            Me.m_Provvigionale = 0
            Me.m_CaricaAlMassimo = False
            Me.m_ProfiloID = 0
            Me.m_Profilo = Nothing
            Me.m_NomeProfilo = ""
            Me.m_SortBy = SortPreventivoBy.SORTBY_NETTO
            Me.m_StipendioLordo = 0
            Me.m_StipendioNetto = 0
            Me.m_NumeroMensilita = 13
            Me.m_TFR = 0
            Me.m_AmministrazioneID = 0
            Me.m_Amministrazione = Nothing
            Me.m_eMail = ""
            Me.m_Telefono = ""
            Me.m_Fax = ""
            Me.m_Offerte = Nothing
        End Sub

        Public Overrides Function GetModule() As CModule
            Return DMD.CQSPD.Preventivi.Module
        End Function

        ''' <summary>
        ''' Restituisce o imposta l'e-mail del cliente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property eMail As String
            Get
                Return Me.m_eMail
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_eMail
                If (oldValue = value) Then Exit Property
                Me.m_eMail = Trim(value)
                Me.DoChanged("eMail", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il telefono del cliente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
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

        ''' <summary>
        ''' Restituisce o imposta il fax dl cliente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
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

        ''' <summary>
        ''' Restituisce o imposta il nome del comune di nascita del cliente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NatoAComune As String
            Get
                Return Me.m_NatoAComune
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NatoAComune
                If (oldValue = value) Then Exit Property
                Me.m_NatoAComune = value
                Me.DoChanged("NatoAComune", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome della provincia di nascita del cliente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NatoAProvincia As String
            Get
                Return Me.m_NatoAProvincia
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NatoAProvincia
                If (oldValue = value) Then Exit Property
                Me.m_NatoAProvincia = value
                Me.DoChanged("NatoAProvincia", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il codice fiscale del cliente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
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

        ''' <summary>
        ''' Restituisce o imposta la partita iva del cliente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
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
        ''' Restituisce o imposta il profilo di preventivazione 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
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
                If Not (value Is Nothing) Then Me.m_NomeProfilo = value.ProfiloVisibile
                Me.DoChanged("Profilo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID del profilo di preventivazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDProfilo As Integer
            Get
                Return GetID(Me.m_Profilo, Me.m_ProfiloID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDProfilo
                If (oldValue = value) Then Exit Property
                Me.m_ProfiloID = value
                Me.m_Profilo = Nothing
                Me.DoChanged("IDProfilo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
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

        ''' <summary>
        ''' Restituisce o imposta un valore che indica se visualizzare le offerte favorendo il cliente o l'azienda
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SortBy As SortPreventivoBy
            Get
                Return Me.m_SortBy
            End Get
            Set(value As SortPreventivoBy)
                Dim oldValue As SortPreventivoBy = Me.m_SortBy
                If (oldValue = value) Then Exit Property
                Me.m_SortBy = value
                Me.DoChanged("SortBy", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta lo stipendio lordo del cliente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StipendioLordo As Decimal
            Get
                Return Me.m_StipendioLordo
            End Get
            Set(value As Decimal)
                Dim oldValue As Decimal = Me.m_StipendioLordo
                If (oldValue = value) Then Exit Property
                Me.m_StipendioLordo = value
                Me.DoChanged("StipendioLordo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta lo stipendio netto del cliente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StipendioNetto As Decimal
            Get
                Return Me.m_StipendioNetto
            End Get
            Set(value As Decimal)
                Dim oldValue As Decimal = Me.m_StipendioNetto
                If (oldValue = value) Then Exit Property
                Me.m_StipendioNetto = value
                Me.DoChanged("StipendioNetto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il numero di mensilità percepite dal cliente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NumeroMensilita As Integer
            Get
                Return Me.m_NumeroMensilita
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_NumeroMensilita
                If (oldValue = value) Then Exit Property
                Me.m_NumeroMensilita = value
                Me.DoChanged("NumeroMensilita", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il TFR maturato dal cliente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TFR As Decimal
            Get
                Return Me.m_TFR
            End Get
            Set(value As Decimal)
                Dim oldValue As Decimal = Me.m_TFR
                If (oldValue = value) Then Exit Property
                Me.m_TFR = value
                Me.DoChanged("TFR", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'amministrazione per cui lavora il cliente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDAmministrazione As Integer
            Get
                Return GetID(Me.m_Amministrazione, Me.m_AmministrazioneID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDAmministrazione
                If oldValue = value Then Exit Property
                Me.m_Amministrazione = Nothing
                Me.m_AmministrazioneID = value
                Me.DoChanged("IDAmministrazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un oggetto CAzienda che rappresenta l'amministrazione per cui lavora il cliente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Amministrazione As CAzienda
            Get
                If (Me.m_Amministrazione Is Nothing) Then Me.m_Amministrazione = Anagrafica.Aziende.GetItemById(Me.m_AmministrazioneID)
                Return Me.m_Amministrazione
            End Get
            Set(value As CAzienda)
                Dim oldValue As CAzienda = Me.Amministrazione
                If (oldValue = value) Then Exit Property
                Me.m_Amministrazione = value
                Me.m_AmministrazioneID = GetID(value)
                Me.DoChanged("Amministrazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di nascita del cliente
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
                Me.DoChanged("DataNascita", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di assunzione del cliente
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
                Me.DoChanged("DataAssunzione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di decorrenza dell'offerta
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
                Me.DoChanged("DataDecorrenza", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce il nominativo del cliente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Nominativo As String
            Get
                Return Trim(Strings.ToNameCase(m_Nome) & " " & UCase(m_Cognome))
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del cliente
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
        ''' Restituisce o imposta il cognome del cliente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Cognome As String
            Get
                Return Me.m_Cognome
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Cognome
                If (oldValue = value) Then Exit Property
                Me.m_Cognome = value
                Me.DoChanged("Cognome", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID del tipo contratto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TipoContratto As String
            Get
                Return Me.m_TipoContratto
            End Get
            Set(value As String)
                value = UCase(Left(Trim(value), 1))
                Dim oldValue As String = Me.m_TipoContratto
                If (oldValue = value) Then Exit Property
                Me.m_TipoContratto = value
                Me.DoChanged("TipoContratto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID del tipo rapporto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TipoRapporto As String
            Get
                Return Me.m_TipoRapporto
            End Get
            Set(value As String)
                value = UCase(Left(Trim(value), 1))
                Dim oldValue As String = Me.m_TipoRapporto
                If (oldValue = value) Then Exit Property
                Me.m_TipoRapporto = value
                Me.DoChanged("TipoRapporto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il sesso del cliente
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
                Me.DoChanged("Sesso", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisc o imposta la durata (in mesi) del finanziamento
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
                Me.DoChanged("Durata", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il valore della rata mensile
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Rata As Decimal
            Get
                Return Me.m_Rata
            End Get
            Set(value As Decimal)
                Dim oldValue As Decimal = Me.m_Rata
                If (oldValue = value) Then Exit Property
                Me.m_Rata = value
                Me.DoChanged("Rata", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce il momentate lordo del finanziamento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property MontanteLordo As Decimal
            Get
                Return Me.Rata * Me.Durata
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta la provvigione percentuale sul momentante lordo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Provvigionale As Double
            Get
                Return Me.m_Provvigionale
            End Get
            Set(value As Double)
                Dim oldValue As Double = Me.m_Provvigionale
                If (oldValue = value) Then Exit Property
                Me.m_Provvigionale = value
                Me.DoChanged("Provvigionale", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Se vero indica di calcolare tutte le offerte caricando al massimo possibile
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CaricaAlMassimo As Boolean
            Get
                Return Me.m_CaricaAlMassimo
            End Get
            Set(value As Boolean)
                If (Me.m_CaricaAlMassimo = value) Then Exit Property
                Me.m_CaricaAlMassimo = value
                Me.DoChanged("CaricaAlMassimo", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce l'elenco delle offerte calcolate
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Offerte As CCQSPDOfferte
            Get
                If Me.m_Offerte Is Nothing Then Me.m_Offerte = New CCQSPDOfferte(Me)
                Return Me.m_Offerte
            End Get
        End Property

        ''' <summary>
        ''' Forza il ricalcolo
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub ForceRecalc()
            Me.Offerte.Clear()
            Me.ResetID()
            Me.Calcola()
        End Sub

        ''' <summary>
        ''' Ricacola	
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Calcola()
            Dim prodotti As CCollection(Of CCQSPDProdotto)
            Dim prodotto As CCQSPDProdotto
            Dim relFin As CProdottoXTabellaFin
            Dim relAss As CProdottoXTabellaAss
            Dim tabellafin As CTabellaFinanziaria
            Dim offerta As COffertaCQS
            'Resettiamo le offerte
            Me.Offerte.Clear()
            'Otteniamo l'elenco di tutti i prodotti disponibili per il profilo
            prodotti = Me.Profilo.GetProdottiValidi
            'Per ogni prodotto
            For i As Integer = 0 To prodotti.Count - 1
                prodotto = prodotti(i)
                'Se tipo contratto ed il tipo rapporto coincidono
                If prodotto.IdTipoContratto = Me.TipoContratto AndAlso _
                   prodotto.IdTipoRapporto = Me.TipoRapporto Then
                    'Per ogni tabella finanziaria
                    For j As Integer = 0 To prodotto.TabelleFinanziarieRelations.Count - 1
                        relFin = prodotto.TabelleFinanziarieRelations(j)
                        tabellafin = relFin.Tabella
                        'e per ogni tripla di tabelle assicurative
                        For k As Integer = 0 To prodotto.TabelleAssicurativeRelations.Count - 1
                            relAss = prodotto.TabelleAssicurativeRelations(k)
                            offerta = New COffertaCQS
                            With offerta
                                .Preventivo = Me
                                .Prodotto = prodotto
                                .TabellaFinanziariaRel = relFin
                                .TabellaAssicurativaRel = relAss
                                .TabellaSpese = prodotto.TabellaSpese
                                .Provvigionale.PercentualeSu(Me.MontanteLordo) = Me.Provvigionale
                                .Calcola()
                            End With
                            Me.Offerte.Add(offerta)
                        Next
                    Next
                End If
            Next
            Me.Offerte.Comparer = Me
            Me.Offerte.Sort()
        End Sub


        Private Function Compare1(ByVal a As Object, ByVal b As Object) As Integer Implements IComparer.Compare
            Return Me.Compare(a, b)
        End Function

        ''' <summary>
        ''' Compara due offerte
        ''' </summary>
        ''' <param name="a"></param>
        ''' <param name="b"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Compare(ByVal a As COffertaCQS, ByVal b As COffertaCQS) As Integer
            If a.ErrorCode = 0 And b.ErrorCode <> 0 Then Return -1
            If a.ErrorCode <> 0 And b.ErrorCode = 0 Then Return 1
            Select Case Me.SortBy
                Case SortPreventivoBy.SORTBY_RICARICOTOTALE
                    'If (a.GuadagnoAziendale + a.ValoreProvvigioni < b.GuadagnoAziendale + b.ValoreProvvigioni) Then Return 1
                    'If (a.GuadagnoAziendale + a.ValoreProvvigioni > b.GuadagnoAziendale + b.ValoreProvvigioni) Then Return -1
                    If (a.NettoRicavo < b.NettoRicavo) Then Return 1
                    If (a.NettoRicavo > b.NettoRicavo) Then Return -1
                    Return 0
                Case Else
                    If (a.NettoRicavo < b.NettoRicavo) Then Return 1
                    If (a.NettoRicavo > b.NettoRicavo) Then Return -1
                    'If (a.GuadagnoAziendale + a.ValoreProvvigioni < b.GuadagnoAziendale + b.ValoreProvvigioni) Then Return 1
                    'If (a.GuadagnoAziendale + a.ValoreProvvigioni > b.GuadagnoAziendale + b.ValoreProvvigioni) Then Return -1
                    Compare = 0
            End Select
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_Preventivi"
        End Function

        Protected Overrides Function SaveToDatabase(dbConn As CDBConnection, ByVal force As Boolean) As Boolean
            Dim ret As Boolean = MyBase.SaveToDatabase(dbConn, force)
            If (ret) And Not (Me.m_Offerte Is Nothing) Then Me.m_Offerte.Save(force)
            Return ret
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_SortBy = reader.GetValue(Of Integer)("SortBy", 0)
            reader.Read("DataNascita", Me.m_DataNascita)
            reader.Read("DataAssunzione", Me.m_DataAssunzione)
            reader.Read("DataDecorrenza", Me.m_DataDecorrenza)
            reader.Read("Nome", Me.m_Nome)
            reader.Read("Cognome", Me.m_Cognome)
            reader.Read("TipoContratto", Me.m_TipoContratto)
            reader.Read("TipoRapporto", Me.m_TipoRapporto)
            reader.Read("Sesso", Me.m_Sesso)
            reader.Read("Durata", Me.m_Durata)
            reader.Read("Rata", Me.m_Rata)
            reader.Read("Provvigionale", Me.m_Provvigionale)
            reader.Read("CaricaAlMassimo", Me.m_CaricaAlMassimo)
            reader.Read("Profilo", Me.m_ProfiloID)
            reader.Read("NomeProfilo", Me.m_NomeProfilo)
            reader.Read("StipendioLordo", Me.m_StipendioLordo)
            reader.Read("StipendioNetto", Me.m_StipendioNetto)
            reader.Read("NumeroMensilita", Me.m_NumeroMensilita)
            reader.Read("TFR", Me.m_TFR)
            reader.Read("Amministrazione", Me.m_AmministrazioneID)
            reader.Read("NatoAComune", Me.m_NatoAComune)
            reader.Read("NatoAProvincia", Me.m_NatoAProvincia)
            reader.Read("CodiceFiscale", Me.m_CodiceFiscale)
            'reader.Read("PartitaIVA", Me.m_PartitaIVA)
            ' reader.Read("eMail", Me.m_eMail)
            'reader.Read("Telefono", Me.m_Telefono)
            'reader.Read("Fax", Me.m_Fax)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("SortBy", Me.m_SortBy)
            writer.Write("DataNascita", Me.m_DataNascita)
            writer.Write("DataAssunzione", Me.m_DataAssunzione)
            writer.Write("DataDecorrenza", Me.m_DataDecorrenza)
            writer.Write("Nome", Me.m_Nome)
            writer.Write("Cognome", Me.m_Cognome)
            writer.Write("TipoContratto", Me.m_TipoContratto)
            writer.Write("TipoRapporto", Me.m_TipoRapporto)
            writer.Write("Sesso", Me.m_Sesso)
            writer.Write("Durata", Me.m_Durata)
            writer.Write("Rata", Me.m_Rata)
            writer.Write("Provvigionale", Me.m_Provvigionale)
            writer.Write("CaricaAlMassimo", Me.m_CaricaAlMassimo)
            writer.Write("Profilo", Me.IDProfilo)
            writer.Write("NomeProfilo", Me.m_NomeProfilo)
            writer.Write("StipendioLordo", Me.m_StipendioLordo)
            writer.Write("StipendioNetto", Me.m_StipendioNetto)
            writer.Write("NumeroMensilita", Me.m_NumeroMensilita)
            writer.Write("TFR", Me.m_TFR)
            writer.Write("Amministrazione", Me.IDAmministrazione)
            writer.Write("NatoAComune", Me.m_NatoAComune)
            writer.Write("NatoAProvincia", Me.m_NatoAProvincia)
            writer.Write("CodiceFiscale", Me.m_CodiceFiscale)
            'writer.Write("PartitaIVA", Me.m_PartitaIVA)
            'writer.Write("eMail", Me.m_eMail)
            'writer.Write("Telefono", Me.m_Telefono)
            'writer.Write("Fax", Me.m_Fax)
            Return MyBase.SaveToRecordset(writer)
        End Function

        '---------------------------
        Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
            writer.WriteAttribute("Nome", Me.m_Nome)
            writer.WriteAttribute("Cognome", Me.m_Cognome)
            writer.WriteAttribute("Sesso", Me.m_Sesso)
            writer.WriteAttribute("DataNascita", Me.m_DataNascita)
            writer.WriteAttribute("NatoAComune", Me.m_NatoAComune)
            writer.WriteAttribute("NatoAProvincia", Me.m_NatoAProvincia)
            writer.WriteAttribute("CodiceFiscale", Me.m_CodiceFiscale)
            writer.WriteAttribute("PartitaIVA", Me.m_PartitaIVA)
            writer.WriteAttribute("DataAssunzione", Me.m_DataAssunzione)
            writer.WriteAttribute("DataDecorrenza", Me.m_DataDecorrenza)
            writer.WriteAttribute("TipoContratto", Me.m_TipoContratto)
            writer.WriteAttribute("TipoRapporto", Me.m_TipoRapporto)
            writer.WriteAttribute("Durata", Me.m_Durata)
            writer.WriteAttribute("Rata", Me.m_Rata)
            writer.WriteAttribute("Provvigionale", Me.m_Provvigionale)
            writer.WriteAttribute("CaricaAlMassimo", Me.m_CaricaAlMassimo)
            writer.WriteAttribute("ProfiloID", Me.IDProfilo)
            writer.WriteAttribute("NomeProfilo", Me.m_NomeProfilo)
            writer.WriteAttribute("SortBy", Me.m_SortBy)
            writer.WriteAttribute("StipendioLordo", Me.m_StipendioLordo)
            writer.WriteAttribute("StipendioNetto", Me.m_StipendioNetto)
            writer.WriteAttribute("NumeroMensilita", Me.m_NumeroMensilita)
            writer.WriteAttribute("TFR", Me.m_TFR)
            writer.WriteAttribute("AmministrazioneID", Me.IDAmministrazione)
            writer.WriteAttribute("eMail", Me.m_eMail)
            writer.WriteAttribute("Telefono", Me.m_Telefono)
            writer.WriteAttribute("Fax", Me.m_Fax)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object)
            Select Case fieldName
                Case "Nome" : m_Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Cognome" : m_Cognome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Sesso" : m_Sesso = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataNascita" : m_DataNascita = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "NatoAComune" : m_NatoAComune = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "NatoAProvincia" : m_NatoAProvincia = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "CodiceFiscale" : m_CodiceFiscale = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "PartitaIVA" : m_PartitaIVA = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataAssunzione" : m_DataAssunzione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataDecorrenza" : m_DataDecorrenza = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "TipoContratto" : m_TipoContratto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "TipoRapporto" : m_TipoRapporto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Durata" : m_Durata = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Rata" : m_Rata = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Provvigionale" : m_Provvigionale = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "CaricaAlMassimo" : m_CaricaAlMassimo = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "ProfiloID" : m_ProfiloID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeProfilo" : m_NomeProfilo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "SortBy" : m_SortBy = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "StipendioLordo" : m_StipendioLordo = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "StipendioNetto" : m_StipendioNetto = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "NumeroMensilita" : m_NumeroMensilita = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "TFR" : m_TFR = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "AmministrazioneID" : m_AmministrazioneID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "eMail" : m_eMail = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Telefono" : m_Telefono = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Fax" : m_Fax = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Offerte"
                    If (TypeOf (fieldValue) Is CCQSPDOfferte) Then
                        Me.m_Offerte = fieldValue
                        Me.m_Offerte.setOwner(Me)
                    Else
                        Me.m_Offerte = Nothing
                    End If
                Case Else
                    MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

    End Class


End Class

Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.CustomerCalls

Partial Public Class CQSPD

    Public Enum StatoLavorazione As Integer
        ''' <summary>
        ''' Non è definito uno stato per questa persona
        ''' </summary>
        ''' <remarks></remarks>
        None = 0

        ''' <summary>
        ''' L'utente è stato contattato ed ha risposto almeno una volta
        ''' </summary>
        ''' <remarks></remarks>
        InContatto = 2

        ''' <summary>
        ''' Il cliente è interessato ad una operazione
        ''' </summary>
        ''' <remarks></remarks>
        Interessato = 3

        ''' <summary>
        ''' Il cliente non è interessato ad alcuna operazione
        ''' </summary>
        ''' <remarks></remarks>
        NonInteressato = 4


        ''' <summary>
        ''' La persona è stata visitata o è venuta in ufficio almeno una volta
        ''' </summary>
        ''' <remarks></remarks>
        Visita = 5
        ''' <summary>
        ''' Il cliente ha richiesto un conteggio estintivo
        ''' </summary>
        ''' <remarks></remarks>
        RichiestaConteggioEstintivo = 6

        ''' <summary>
        ''' La persona ha fatto almeno una richiesta di finanziamento
        ''' </summary>
        ''' <remarks></remarks>
        RichiestaFinanziamento = 7

        ''' <summary>
        ''' La persona ha inviato la busta paga
        ''' </summary>
        ''' <remarks></remarks>
        BustaPaga = 8

        ''' <summary>
        ''' E' stato fatto almeno uno studio di fattibilità 
        ''' </summary>
        ''' <remarks></remarks>
        StudioDiFattibilita = 9

        ''' <summary>
        ''' E' stata inserita almeno una pratica
        ''' </summary>
        ''' <remarks></remarks>
        Pratica = 10

        
    End Enum

    Public Enum SottostatoLavorazione As Integer
        None = 0
        InAttesa = 1
        InLavorazione = 2
        NonFattibile = 3
        RifiutatoDalCliente = 4
        BocciatoAgenzia = 5
        BocciatoIstituto = 6
        Completato = 7
    End Enum
     
    <Serializable> _
    Public Class ClientiLavoratiStatsItem
        Inherits DBObjectPO

        Private m_IDCliente As Integer
        Private m_NomeCliente As String
        Private m_Cliente As CPersona
        Private m_IconaCliente As String

        Private m_StatoLavorazione As StatoLavorazione
        Private m_SottostatoLavorazione As SottostatoLavorazione
        Private m_DataUltimoAggiornamento As Date?
        Private m_DataInizioLavorazione As Date?
        Private m_DataFineLavorazione As Date?

        Private m_IDOperatore As Integer
        Private m_Operatore As CUser
        Private m_NomeOperatore As String
       
        Private m_Flags As Integer

        Private m_Opeatori As Integer()
        Private m_Visite As Integer()
        Private m_RichiesteConteggi As Integer()
        Private m_RichiesteFinanziamenti As Integer()
        Private m_BustePaga As Integer()
        Private m_OfferteInserite As Integer()
        Private m_OfferteProposte As Integer()
        Private m_OfferteRifiutateCliente As Integer()
        Private m_OfferteBocciate As Integer()
        Private m_OfferteNonFattibili As Integer()
        Private m_PraticheInCorso As Integer()
        Private m_PraticheLiquidate As Integer()
        Private m_PraticheRifiutateCliente As Integer()
        Private m_PraticheBocciate As Integer()
        Private m_PraticheNonFattibili As Integer()

        Public Sub New()
            Me.m_IDCliente = 0
            Me.m_NomeCliente = ""
            Me.m_IconaCliente = ""
            Me.m_Cliente = Nothing
            Me.m_StatoLavorazione = CQSPD.StatoLavorazione.None
            Me.m_SottostatoLavorazione = CQSPD.SottostatoLavorazione.None
            Me.m_DataUltimoAggiornamento = Nothing
            Me.m_DataInizioLavorazione = Nothing
            Me.m_DataFineLavorazione = Nothing
            'Me.m_NumeroBustePaga = 0
            'Me.m_NumeroVisite = 0
            'Me.m_NumeroVisiteConsulenza = 0
            'Me.m_NumeroRichiesteFinanziamento = 0
            'Me.m_NumeroRichiesteConteggiEstintivi = 0
            'Me.m_NumeroStudiDiFattibilita = 0
            'Me.m_NumeroOfferteProposte = 0
            'Me.m_NumeroOfferteAccettate = 0
            'Me.m_NumeroOfferteRifiutate = 0
            'Me.m_NumeroOfferteNonFattibili = 0
            'Me.m_NumeroOfferteBocciate = 0
            'Me.m_NumeroPratiche = 0
            'Me.m_NumeroPraticheLiquidate = 0
            'Me.m_NumeroPraticheAnnullate = 0
            'Me.m_NumeroPraticheRifiutate = 0
            'Me.m_NumeroPraticheNonFattibili = 0
            'Me.m_NumeroPraticheBocciate = 0

            Me.m_Flags = 0

            Me.m_Opeatori = Nothing
            Me.m_Visite = Nothing
            Me.m_RichiesteConteggi = Nothing
            Me.m_RichiesteFinanziamenti = Nothing
            Me.m_OfferteInserite = Nothing
            Me.m_OfferteProposte = Nothing
            Me.m_OfferteRifiutateCliente = Nothing
            Me.m_OfferteBocciate = Nothing
            Me.m_OfferteNonFattibili = Nothing
            Me.m_PraticheInCorso = Nothing
            Me.m_PraticheLiquidate = Nothing
            Me.m_PraticheRifiutateCliente = Nothing
            Me.m_PraticheBocciate = Nothing
            Me.m_PraticheNonFattibili = Nothing

        End Sub

        ''' <summary>
        ''' Restituisce o i imposta dei flags aggiuntivi
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Flags As Integer
            Get
                Return Me.m_Flags
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_Flags
                If (oldValue = value) Then Exit Property
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce la collezione di tutti gli operatori che hanno avuto contatti con il cliente durante il periodo di lavorazione
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetOperatori() As CCollection(Of CUser)
            Dim ret As New CCollection(Of CUser)
            Dim len As Integer = Arrays.Len(Me.m_Opeatori)
            For i As Integer = 0 To len - 1
                Dim user As CUser = Sistema.Users.GetItemById(Me.m_Opeatori(i))
                If (user IsNot Nothing AndAlso user.Stato = ObjectStatus.OBJECT_VALID) Then ret.Add(user)
            Next
            Return ret
        End Function

        ''' <summary>
        ''' Restituisce o il numero di buste paga caricate per il cliente durante il periodo di lavorazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property NumeroBustePaga As Integer
            Get
                Return Arrays.Len(Me.m_BustePaga)
            End Get
        End Property

        ''' <summary>
        ''' Restituisce la collezione delle buste paga caricate
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetBustePaga() As CCollection(Of CAttachment)
            Dim ret As New CCollection(Of CAttachment)
            Dim len As Integer = Me.NumeroBustePaga
            For i As Integer = 0 To len - 1
                Dim a As CAttachment = Sistema.Attachments.GetItemById(Me.m_BustePaga(i))
                If (a IsNot Nothing AndAlso a.Stato = ObjectStatus.OBJECT_VALID) Then ret.Add(a)
            Next
            Return ret
        End Function

        ''' <summary>
        ''' Restituisce  il numero totale delle visite ricevute o presso il cliente durante il periodo di lavorazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property NumeroVisite As Integer
            Get
                Return Arrays.Len(Me.m_Visite)
            End Get
        End Property

        ''' <summary>
        ''' Restituisce la collezione delle visite
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetVisite() As CCollection(Of CVisita)
            Dim ret As New CCollection(Of CVisita)
            Dim len As Integer = Me.NumeroVisite
            For i As Integer = 0 To len - 1
                Dim visita As CVisita = CustomerCalls.Visite.GetItemById(Me.m_Visite(i))
                If (visita IsNot Nothing AndAlso visita.Stato = ObjectStatus.OBJECT_VALID) Then ret.Add(visita)
            Next
            Return ret
        End Function

        ' ''' <summary>
        ' ''' Restituisce  il numero totale di visite con scopo consulenza durante il periodo di lavorazione
        ' ''' </summary>
        ' ''' <value></value>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Property NumeroVisiteConsulenza As Integer
        '    Get
        '        Return Me.m_NumeroVisiteConsulenza
        '    End Get
        '    Set(value As Integer)
        '        Dim oldValue As Integer = Me.m_NumeroVisiteConsulenza
        '        If (oldValue = value) Then Exit Property
        '        Me.m_NumeroVisiteConsulenza = value
        '        Me.DoChanged("NumeroVisiteConsulenza", value, oldValue)
        '    End Set
        'End Property

        ''' <summary>
        ''' Restituisce il numero di richieste di finanziamento registrate per il cliente durante il periodo di lavorazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property NumeroRichiesteFinanziamento As Integer
            Get
                Return Arrays.Len(Me.m_RichiesteFinanziamenti)
            End Get
        End Property

        ''' <summary>
        ''' Restituisce la collezione delle richieste di finanziamento
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetRichiesteFinanziamenti() As CCollection(Of CRichiestaFinanziamento)
            Dim ret As New CCollection(Of CRichiestaFinanziamento)
            Dim len As Integer = Me.NumeroRichiesteFinanziamento
            For i As Integer = 0 To len - 1
                Dim richiesta As CRichiestaFinanziamento = CQSPD.RichiesteFinanziamento.GetItemById(Me.m_RichiesteFinanziamenti(i))
                If (richiesta IsNot Nothing AndAlso richiesta.Stato = ObjectStatus.OBJECT_VALID) Then ret.Add(richiesta)
            Next
            Return ret
        End Function

        ''' <summary>
        ''' Restituisce il numero di richieste di conteggi estintivi fatti dal cliente durante il periodo di lavorazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property NumeroRichiesteConteggiEstintivi As Integer
            Get
                Return Arrays.Len(Me.m_RichiesteConteggi)
            End Get
        End Property

        Public Function GetRichiesteConteggiEstintivi() As CCollection(Of CRichiestaConteggio)
            Dim ret As New CCollection(Of CRichiestaConteggio)
            Dim len As Integer = Me.NumeroRichiesteConteggiEstintivi
            For i As Integer = 0 To len - 1
                Dim richiesta As CRichiestaConteggio = CQSPD.RichiesteConteggi.GetItemById(Me.m_RichiesteConteggi(i))
                If (richiesta IsNot Nothing AndAlso richiesta.Stato = ObjectStatus.OBJECT_VALID) Then ret.Add(richiesta)
            Next
            Return ret
        End Function

        ''' <summary>
        ''' Restituisce il numero di offerte che sono solo state inserite 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property NumeroOfferteInserite As Integer
            Get
                Return Arrays.Len(Me.m_OfferteInserite)
            End Get
        End Property

        Public Function GetOfferteInserite() As CCollection(Of CQSPDConsulenza)
            Dim ret As New CCollection(Of CQSPDConsulenza)
            Dim len As Integer = Me.NumeroOfferteInserite
            For i As Integer = 0 To len - 1
                Dim offerta As CQSPDConsulenza = CQSPD.Consulenze.GetItemById(Me.m_OfferteInserite(i))
                If (offerta IsNot Nothing AndAlso offerta.Stato = ObjectStatus.OBJECT_VALID) Then ret.Add(offerta)
            Next
            Return ret
        End Function

        ''' <summary>
        ''' Restituisce  il numero totale di studi di fattibilità proposti al cliente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property NumeroOfferteProposte As Integer
            Get
                Return Arrays.Len(Me.m_OfferteProposte)
            End Get
        End Property

        Public Function GetOfferteProposte() As CCollection(Of CQSPDConsulenza)
            Dim ret As New CCollection(Of CQSPDConsulenza)
            Dim len As Integer = Me.NumeroOfferteProposte
            For i As Integer = 0 To len - 1
                Dim offerta As CQSPDConsulenza = CQSPD.Consulenze.GetItemById(Me.m_OfferteProposte(i))
                If (offerta IsNot Nothing) Then ret.Add(offerta)
            Next
            Return ret
        End Function
 

        ''' <summary>
        ''' Restituisce o imposta il numero di offerte proposte al cliente e da esso rifiutate
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property NumeroOfferteRifiutateCliente As Integer
            Get
                Return Arrays.Len(Me.m_OfferteRifiutateCliente)
            End Get
        End Property

        Public Function GetOfferteRifiutateCliente() As CCollection(Of CQSPDConsulenza)
            Dim ret As New CCollection(Of CQSPDConsulenza)
            Dim len As Integer = Me.NumeroOfferteRifiutateCliente
            For i As Integer = 0 To len - 1
                Dim offerta As CQSPDConsulenza = CQSPD.Consulenze.GetItemById(Me.m_OfferteRifiutateCliente(i))
                If (offerta IsNot Nothing AndAlso offerta.Stato = ObjectStatus.OBJECT_VALID) Then ret.Add(offerta)
            Next
            Return ret
        End Function

        ''' <summary>
        ''' Restituisce o imposta il numero di offerte studiate e marcate come non fattibili
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property NumeroOfferteNonFattibili As Integer
            Get
                Return Arrays.Len(Me.m_OfferteNonFattibili)
            End Get
        End Property

        Public Function GetOfferteNonFattibili() As CCollection(Of CQSPDConsulenza)
            Dim ret As New CCollection(Of CQSPDConsulenza)
            Dim len As Integer = Me.NumeroOfferteNonFattibili
            For i As Integer = 0 To len - 1
                Dim offerta As CQSPDConsulenza = CQSPD.Consulenze.GetItemById(Me.m_OfferteNonFattibili(i))
                If (offerta IsNot Nothing AndAlso offerta.Stato = ObjectStatus.OBJECT_VALID) Then ret.Add(offerta)
            Next
            Return ret
        End Function

        ''' <summary>
        ''' Restituisce o imposta il numero di offerte bocciate dall'agenzia
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property NumeroOfferteBocciate As Integer
            Get
                Return Arrays.Len(Me.m_OfferteBocciate)
            End Get
        End Property

        Public Function GetOfferteBocciate() As CCollection(Of CQSPDConsulenza)
            Dim ret As New CCollection(Of CQSPDConsulenza)
            Dim len As Integer = Me.NumeroOfferteBocciate
            For i As Integer = 0 To len - 1
                Dim offerta As CQSPDConsulenza = CQSPD.Consulenze.GetItemById(Me.m_OfferteBocciate(i))
                If (offerta IsNot Nothing AndAlso offerta.Stato = ObjectStatus.OBJECT_VALID) Then ret.Add(offerta)
            Next
            Return ret
        End Function

        ''' <summary>
        ''' Restituisce o imposta il numero di pratiche in corso
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property NumeroPraticheInCorso As Integer
            Get
                Return Arrays.Len(Me.m_PraticheInCorso)
            End Get
        End Property

        Public Function GetPraticheInCorso() As CCollection(Of CRapportino)
            Dim ret As New CCollection(Of CRapportino)
            Dim len As Integer = Me.NumeroPraticheInCorso
            For i As Integer = 0 To len - 1
                Dim pratica As CRapportino = CQSPD.Pratiche.GetItemById(Me.m_PraticheInCorso(i))
                If (pratica IsNot Nothing AndAlso pratica.Stato = ObjectStatus.OBJECT_VALID) Then ret.Add(pratica)
            Next
            Return ret
        End Function

        ''' <summary>
        ''' Restituisce o imposta il numero di pratiche liquidate durante il periodo di lavorazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property NumeroPraticheLiquidate As Integer
            Get
                Return Arrays.Len(Me.m_PraticheLiquidate)
            End Get
        End Property

        Public Function GetPraticheLiquidate() As CCollection(Of CRapportino)
            Dim ret As New CCollection(Of CRapportino)
            Dim len As Integer = Me.NumeroPraticheInCorso
            For i As Integer = 0 To len - 1
                Dim pratica As CRapportino = CQSPD.Pratiche.GetItemById(Me.m_PraticheLiquidate(i))
                If (pratica IsNot Nothing AndAlso pratica.Stato = ObjectStatus.OBJECT_VALID) Then ret.Add(pratica)
            Next
            Return ret
        End Function
         
        ''' <summary>
        ''' Restituisce o imposta il numero di pratiche rifiutate dal cliente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property NumeroPraticheRifiutateCliente As Integer
            Get
                Return Arrays.Len(Me.m_PraticheRifiutateCliente)
            End Get
        End Property

        Public Function GetPraticheRifiutateCliente() As CCollection(Of CRapportino)
            Dim ret As New CCollection(Of CRapportino)
            Dim len As Integer = Me.NumeroPraticheRifiutateCliente
            For i As Integer = 0 To len - 1
                Dim pratica As CRapportino = CQSPD.Pratiche.GetItemById(Me.m_PraticheRifiutateCliente(i))
                If (pratica IsNot Nothing AndAlso pratica.Stato = ObjectStatus.OBJECT_VALID) Then ret.Add(pratica)
            Next
            Return ret
        End Function

        ''' <summary>
        ''' Restituisce o imposta il numero di pratiche non fattibili
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property NumeroPraticheNonFattibili As Integer
            Get
                Return Arrays.Len(Me.m_PraticheNonFattibili)
            End Get
        End Property

        Public Function GetPraticheNonFattibili() As CCollection(Of CRapportino)
            Dim ret As New CCollection(Of CRapportino)
            Dim len As Integer = Me.NumeroPraticheNonFattibili
            For i As Integer = 0 To len - 1
                Dim pratica As CRapportino = CQSPD.Pratiche.GetItemById(Me.m_PraticheNonFattibili(i))
                If (pratica IsNot Nothing AndAlso pratica.Stato = ObjectStatus.OBJECT_VALID) Then ret.Add(pratica)
            Next
            Return ret
        End Function

        ''' <summary>
        ''' Restituisce o imposta il numero di pratiche bocciate
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property NumeroPraticheBocciate As Integer
            Get
                Return Arrays.Len(Me.m_PraticheBocciate)
            End Get
        End Property

        Public Function GetPraticheBocciate() As CCollection(Of CRapportino)
            Dim ret As New CCollection(Of CRapportino)
            Dim len As Integer = Me.NumeroPraticheBocciate
            For i As Integer = 0 To len - 1
                Dim pratica As CRapportino = CQSPD.Pratiche.GetItemById(Me.m_PraticheBocciate(i))
                If (pratica IsNot Nothing AndAlso pratica.Stato = ObjectStatus.OBJECT_VALID) Then ret.Add(pratica)
            Next
            Return ret
        End Function

        ''' <summary>
        ''' Restituisce o imposta un valore che indica lo stato di lavorazione del cliente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StatoLavorazione As StatoLavorazione
            Get
                Return Me.m_StatoLavorazione
            End Get
            Set(value As StatoLavorazione)
                Dim oldValue As StatoLavorazione = Me.m_StatoLavorazione
                If (oldValue = value) Then Exit Property
                Me.m_StatoLavorazione = value
                Me.DoChanged("StatoLavorazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore che indica il passaggio intermedio dello stato di lavorazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SottostatoLavorazione As SottostatoLavorazione
            Get
                Return Me.m_SottostatoLavorazione
            End Get
            Set(value As SottostatoLavorazione)
                Dim oldValue As SottostatoLavorazione = Me.m_SottostatoLavorazione
                If (oldValue = value) Then Exit Property
                Me.m_SottostatoLavorazione = value
                Me.DoChanged("SottostatoLavorazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di inizio lavorazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataInizioLavorazione As Date?
            Get
                Return Me.m_DataInizioLavorazione
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataInizioLavorazione
                If (Calendar.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_DataInizioLavorazione = value
                Me.DoChanged("DataInizioLavorazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di fine lavorazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataFineLavorazione As Date?
            Get
                Return Me.m_DataFineLavorazione
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataFineLavorazione
                If (Calendar.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_DataFineLavorazione = value
                Me.DoChanged("DataFineLavorazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data dell'ultimo aggiornamento di stato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataUltimoAggiornamento As Date?
            Get
                Return Me.m_DataUltimoAggiornamento
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataUltimoAggiornamento
                If (Calendar.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_DataUltimoAggiornamento = value
                Me.DoChanged("DataUltimoAggiornamento", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Somma delle pratiche rifiutate, bocciate e non fattibili
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property NumeroPraticheAnnullate As Integer
            Get
                Return Me.NumeroPraticheRifiutateCliente + Me.NumeroPraticheNonFattibili + Me.NumeroPraticheBocciate
            End Get
        End Property

        ''' <summary>
        ''' Somma delle pratice in corso, liquidate ed annullate
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property NumeroPratiche As Integer
            Get
                Return Me.NumeroPraticheInCorso + Me.NumeroPraticheLiquidate + Me.NumeroPraticheAnnullate
            End Get
        End Property

        ''' <summary>
        ''' Somma delle offerte rifiutate, bocciate e non fattibili
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property NumeroOfferteAnnullate As Integer
            Get
                Return Me.NumeroOfferteBocciate + Me.NumeroOfferteNonFattibili + Me.NumeroOfferteRifiutateCliente
            End Get
        End Property

        ''' <summary>
        ''' Somma delle offerte inserite, proposte  e annullate
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property NumeroOfferte As Integer
            Get
                Return Me.NumeroOfferteInserite + Me.NumeroOfferteProposte + Me.NumeroOfferteAnnullate
            End Get
        End Property

        Public Function CalcolaStato() As StatoLavorazione
            If Me.NumeroPratiche > 0 Then
                Return CQSPD.StatoLavorazione.Pratica
            ElseIf Me.NumeroOfferte > 0 Then
                Return CQSPD.StatoLavorazione.StudioDiFattibilita
            ElseIf Me.NumeroRichiesteFinanziamento > 0 Then
                Return CQSPD.StatoLavorazione.RichiestaFinanziamento
            ElseIf Me.NumeroBustePaga > 0 Then
                Return CQSPD.StatoLavorazione.BustaPaga
            ElseIf Me.NumeroVisite > 0 Then
                Return CQSPD.StatoLavorazione.Visita
            ElseIf Me.NumeroRichiesteConteggiEstintivi > 0 Then
                Return CQSPD.StatoLavorazione.RichiestaConteggioEstintivo
            Else
                Return CQSPD.StatoLavorazione.None
            End If
        End Function

        Public Function CalcolaSottostato() As SottostatoLavorazione
            Select Case Me.CalcolaStato
                Case CQSPD.StatoLavorazione.RichiestaConteggioEstintivo
                    Return CQSPD.SottostatoLavorazione.InLavorazione
                Case CQSPD.StatoLavorazione.Visita
                    Return CQSPD.SottostatoLavorazione.InLavorazione
                Case CQSPD.StatoLavorazione.RichiestaFinanziamento
                    Return CQSPD.SottostatoLavorazione.InLavorazione
                Case CQSPD.StatoLavorazione.BustaPaga
                    Return CQSPD.SottostatoLavorazione.InLavorazione
                Case CQSPD.StatoLavorazione.StudioDiFattibilita
                    If Me.NumeroOfferteProposte > 0 Then
                        Return CQSPD.SottostatoLavorazione.InLavorazione
                    ElseIf Me.NumeroOfferteRifiutateCliente > 0 Then
                        Return CQSPD.SottostatoLavorazione.RifiutatoDalCliente
                    ElseIf Me.NumeroOfferteBocciate > 0 Then
                        Return CQSPD.SottostatoLavorazione.BocciatoAgenzia
                    Else
                        Return CQSPD.SottostatoLavorazione.NonFattibile
                    End If
                Case CQSPD.StatoLavorazione.Pratica
                    If Me.NumeroPratiche = Me.NumeroPraticheAnnullate + Me.NumeroPraticheLiquidate Then
                        If Me.NumeroPraticheLiquidate > 0 Then
                            Return CQSPD.SottostatoLavorazione.Completato
                        ElseIf Me.NumeroPraticheRifiutateCliente > 0 Then
                            Return CQSPD.SottostatoLavorazione.RifiutatoDalCliente
                        ElseIf Me.NumeroPraticheBocciate > 0 Then
                            Return CQSPD.SottostatoLavorazione.BocciatoAgenzia
                        Else
                            Return CQSPD.SottostatoLavorazione.NonFattibile
                        End If
                    Else
                        Return CQSPD.SottostatoLavorazione.InLavorazione
                    End If
                Case Else
                    Return CQSPD.SottostatoLavorazione.None
            End Select
        End Function

        Public Property IDCliente As Integer
            Get
                Return GetID(Me.m_Cliente, Me.m_IDCliente)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDCliente
                If (oldValue = value) Then Exit Property
                Me.m_IDCliente = value
                Me.m_Cliente = Nothing
                Me.DoChanged("IDCliente", value, oldValue)
            End Set
        End Property

        Public Property Cliente As CPersona
            Get
                If (Me.m_Cliente Is Nothing) Then Me.m_Cliente = Anagrafica.Persone.GetItemById(Me.m_IDCliente)
                Return Me.m_Cliente
            End Get
            Set(value As CPersona)
                Dim oldValue As CPersona = Me.m_Cliente
                If (oldValue Is value) Then Exit Property
                Me.m_IDCliente = GetID(value)
                Me.m_Cliente = value
                If (value IsNot Nothing) Then
                    Me.m_NomeCliente = value.Nominativo
                    Me.m_IconaCliente = value.IconURL
                    Me.PuntoOperativo = value.PuntoOperativo
                End If
                Me.DoChanged("Cliente", value, oldValue)
            End Set
        End Property

        Public Property NomeCliente As String
            Get
                Return Me.m_NomeCliente
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeCliente
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_NomeCliente = value
                Me.DoChanged("NomeCliente", value, oldValue)
            End Set
        End Property

        Public Property IconaCliente As String
            Get
                Return Me.m_IconaCliente
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_IconaCliente
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_IconaCliente = value
                Me.DoChanged("IconaCliente", value, oldValue)
            End Set
        End Property

        Private Function CalcolaOperatore( _
                                         ByVal pratiche As CCollection(Of CRapportino), _
                                         ByVal consulenze As CCollection(Of CQSPDConsulenza), _
                                         ByVal richiestef As CCollection(Of CRichiestaFinanziamento), _
                                         ByVal visite As CCollection(Of CVisita) _
                                        ) As CUser
            Try
                Dim op As CUser = Nothing
                Dim p As CRapportino = Me.GetUltimaPratica(pratiche)

                If (p IsNot Nothing) Then
                    If (op Is Nothing AndAlso p.Consulente IsNot Nothing) Then op = p.Consulente.User
                    If (op Is Nothing AndAlso p.StatoLiquidata IsNot Nothing) Then op = p.StatoLiquidata.Operatore
                    If (op Is Nothing AndAlso p.StatoContatto IsNot Nothing) Then op = p.StatoContatto.Operatore
                    If (op Is Nothing AndAlso p.StatoDiLavorazioneAttuale IsNot Nothing) Then op = p.StatoDiLavorazioneAttuale.Operatore
                    If (op Is Nothing) Then op = p.CreatoDa
                End If

                If (op Is Nothing) Then
                    Dim c As CQSPDConsulenza = Me.GetUltimaConsulenza(consulenze)
                    If (c IsNot Nothing) Then
                        If (c.Consulente IsNot Nothing) Then op = c.Consulente.User
                        If (op Is Nothing) Then op = c.CreatoDa
                    End If
                End If

                If (op Is Nothing) Then
                    Dim r As CRichiestaFinanziamento = Me.GetUltimaRichiesta(richiestef)
                    If (r IsNot Nothing) Then op = r.AssegnatoA
                End If

                If (op Is Nothing) Then
                    Dim v As CVisita = Me.GetUltimaVisita(visite)
                    If (v IsNot Nothing) Then
                        op = v.Operatore
                        If (op Is Nothing) Then op = v.CreatoDa
                    End If
                End If

                Return op
            Catch ex As Exception
                Return Nothing
            End Try
        End Function

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'operatore principale
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDOperatore As Integer
            Get
                Return GetID(Me.m_Operatore, Me.m_IDOperatore)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDOperatore
                If (oldValue = value) Then Exit Property
                Me.m_IDOperatore = value
                Me.m_Operatore = Nothing
                Me.DoChanged("IDOperatore", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Azzera tutte le statistiche
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Azzera()
            Me.m_StatoLavorazione = CQSPD.StatoLavorazione.None
            Me.m_SottostatoLavorazione = CQSPD.SottostatoLavorazione.None
            Me.m_DataUltimoAggiornamento = Nothing
            
            Me.m_IDOperatore = 0
            Me.m_Operatore = Nothing
            Me.m_NomeOperatore = ""

            Me.m_Flags = 0

            Me.m_Opeatori = Nothing
            Me.m_Visite = Nothing
            Me.m_RichiesteConteggi = Nothing
            Me.m_RichiesteFinanziamenti = Nothing
            Me.m_BustePaga = Nothing
            Me.m_OfferteInserite = Nothing
            Me.m_OfferteProposte = Nothing
            Me.m_OfferteRifiutateCliente = Nothing
            Me.m_OfferteBocciate = Nothing
            Me.m_OfferteNonFattibili = Nothing
            Me.m_PraticheInCorso = Nothing
            Me.m_PraticheLiquidate = Nothing
            Me.m_PraticheRifiutateCliente = Nothing
            Me.m_PraticheBocciate = Nothing
            Me.m_PraticheNonFattibili = Nothing
            Me.SetChanged(True)
        End Sub

        ''' <summary>
        ''' Aggiorna le statistiche per il cliente corrente
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Ricalcola()
            If Me.Cliente Is Nothing Then Throw New ArgumentNullException("cliente")

            Me.Azzera()

            Dim visite As CCollection(Of CVisita) = Me.TrovaVisite
            For Each visita As CVisita In visite
                Me.NotifyVisita(visita)
            Next

            Dim richiesteF As CCollection(Of CRichiestaFinanziamento) = Me.TrovaRichiesteFinanziamento
            For Each rich As CRichiestaFinanziamento In richiesteF
                Me.NotifyRichiestaFinanziamento(rich)
            Next

            Dim richiesteCE As CCollection(Of CRichiestaConteggio) = Me.TrovaRichiesteConteggi
            For Each rich As CRichiestaConteggio In richiesteCE
                Me.NotifyRichiestaConteggio(rich)
            Next

            Dim bustePaga As CCollection(Of CAttachment) = Me.TrovaBustePaga
            For Each att As CAttachment In bustePaga
                Me.NotifyBustaPaga(att)
            Next

            Dim studi As CCollection(Of CQSPDConsulenza) = Me.TrovaStudiDiFattibilita
            For Each cons As CQSPDConsulenza In studi
                Me.NotifyConsulenza(cons)
            Next

            Dim pratiche As CCollection(Of CRapportino) = Me.TrovaPratiche
            For Each pratica As CRapportino In pratiche
                Me.NotifyPratica(pratica)
            Next

            

        End Sub

        ''' <summary>
        ''' Effettua la ricerca delle visite fatte o ricevute dal cliente durante il periodo di lavorazione
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function TrovaVisite() As CCollection(Of CVisita)
            Dim ret As New CCollection(Of CVisita)
            Dim visita As CVisita
            Dim dbSQL As String
            Dim dbRis As System.Data.IDataReader

            dbSQL = "SELECT * FROM [tbl_Telefonate] WHERE [ClassName]='CVisita' AND [Stato]=" & ObjectStatus.OBJECT_VALID
            dbSQL = "AND ([IDPersona]=" & Me.IDCliente & " OR [IDPerContoDi]=" & Me.IDCliente & ")"
            If (Me.DataInizioLavorazione.HasValue) Then dbSQL &= " AND [Data]>=" & DBUtils.DBDate(Me.DataInizioLavorazione.Value)
            If (Me.DataFineLavorazione.HasValue) Then dbSQL &= " AND [Data]<=" & DBUtils.DBDate(Me.DataFineLavorazione.Value)
            If (Me.IDPuntoOperativo <> 0) Then dbSQL &= " AND [IDPuntoOperativo]=" & Me.IDPuntoOperativo

            dbRis = CRM.TelDB.ExecuteReader(dbSQL)
            While dbRis.Read
                visita = New CVisita
                CRM.TelDB.Load(visita, dbRis)
                ret.Add(visita)
            End While
            dbRis.Dispose()

            Return ret
        End Function

        ''' <summary>
        ''' Effettua la ricerca delle visite fatte o ricevute dal cliente durante il periodo di lavorazione
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function TrovaBustePaga() As CCollection(Of CAttachment)
            Dim ret As New CCollection(Of CAttachment)
            Dim item As CAttachment
            Dim dbSQL As String
            Dim dbRis As System.Data.IDataReader

            dbSQL = "SELECT * FROM [tbl_Attachments] WHERE [OwnerType]=" & DBUtils.DBString(TypeName(Me.Cliente)) & " AND [OwnerID]=" & Me.IDCliente & " AND [Stato]=" & ObjectStatus.OBJECT_VALID & " AND [Tipo]='Busta Paga'"
            If (Me.DataInizioLavorazione.HasValue) Then dbSQL &= " AND [DataInizio]>=" & DBUtils.DBDate(Me.DataInizioLavorazione.Value)
            If (Me.DataFineLavorazione.HasValue) Then dbSQL &= " AND [DataInizio]<=" & DBUtils.DBDate(Me.DataFineLavorazione.Value)

            dbRis = APPConn.ExecuteReader(dbSQL)
            While dbRis.Read
                item = New CAttachment
                APPConn.Load(item, dbRis)
                ret.Add(item)
            End While
            dbRis.Dispose()

            Return ret
        End Function

        ''' <summary>
        ''' Effettua le richieste di conteggi estintivi
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function TrovaRichiesteConteggi() As CCollection(Of CRichiestaConteggio)
            Dim ret As New CCollection(Of CRichiestaConteggio)
            Dim richiesta As CRichiestaConteggio
            Dim dbSQL As String
            Dim dbRis As System.Data.IDataReader

            dbSQL = "SELECT * FROM [tbl_RichiesteFinanziamentiC] WHERE [Stato]=" & ObjectStatus.OBJECT_VALID
            dbSQL &= " AND [IDCliente]=" & Me.IDCliente
            If (Me.IDPuntoOperativo <> 0) Then dbSQL &= " AND [IDPuntoOperativo]=" & Me.IDPuntoOperativo
            If (Me.DataInizioLavorazione.HasValue) Then dbSQL &= " AND [DataRichiesta]>=" & DBUtils.DBDate(Me.DataInizioLavorazione.Value)
            If (Me.DataFineLavorazione.HasValue) Then dbSQL &= " AND [DataRichiesta]<=" & DBUtils.DBDate(Me.DataFineLavorazione.Value)

            dbRis = CQSPD.Database.ExecuteReader(dbSQL)
            While dbRis.Read
                richiesta = New CRichiestaConteggio
                CQSPD.Database.Load(richiesta, dbRis)
                ret.Add(richiesta)
            End While
            dbRis.Dispose()

            Return ret
        End Function

        ''' <summary>
        ''' Effettua la ricerca sulla tabella delle richieste di finanziamento nel periodo 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function TrovaRichiesteFinanziamento() As CCollection(Of CRichiestaFinanziamento)
            Dim ret As New CCollection(Of CRichiestaFinanziamento)
            Dim richiesta As CRichiestaFinanziamento
            Dim dbSQL As String
            Dim dbRis As System.Data.IDataReader
            
            dbSQL = "SELECT * FROM [tbl_RichiesteFinanziamenti] WHERE [Stato]=" & ObjectStatus.OBJECT_VALID
            dbSQL &= " AND [IDCliente]=" & Me.IDCliente
            If (Me.IDPuntoOperativo <> 0) Then dbSQL &= " AND [IDPuntoOperativo]=" & Me.IDPuntoOperativo
            If (Me.DataInizioLavorazione.HasValue) Then dbSQL &= " AND [Data]>=" & DBUtils.DBDate(Me.DataInizioLavorazione.Value)
            If (Me.DataFineLavorazione.HasValue) Then dbSQL &= " AND [Data]<=" & DBUtils.DBDate(Me.DataFineLavorazione.Value)

            dbRis = CQSPD.Database.ExecuteReader(dbSQL)
            While dbRis.Read
                richiesta = New CRichiestaFinanziamento
                CQSPD.Database.Load(richiesta, dbRis)
                ret.Add(richiesta)
            End While
            dbRis.Dispose()

            Return ret
        End Function

        Public Function TrovaStudiDiFattibilita() As CCollection(Of CQSPDConsulenza)
            Dim ret As New CCollection(Of CQSPDConsulenza)
            Dim dbSQL As String
            Dim dbRis As System.Data.IDataReader
            Dim consulenza As CQSPDConsulenza

            dbSQL = "SELECT * FROM [tbl_CQSPDConsulenze] WHERE [Stato]=" & ObjectStatus.OBJECT_VALID
            dbSQL &= " AND [IDCliente]=" & Me.IDCliente
            If (Me.IDPuntoOperativo <> 0) Then dbSQL &= " AND [IDPuntoOperativo]=" & Me.IDPuntoOperativo
            If (Me.DataInizioLavorazione.HasValue) Then dbSQL &= " AND [DataConsulenza]>=" & DBUtils.DBDate(Me.DataInizioLavorazione.Value)
            If (Me.DataFineLavorazione.HasValue) Then dbSQL &= " AND [DataConsulenza]<=" & DBUtils.DBDate(Me.DataFineLavorazione.Value)

            dbRis = CQSPD.Database.ExecuteReader(dbSQL)
            While dbRis.Read
                consulenza = New CQSPDConsulenza
                CQSPD.Database.Load(consulenza, dbRis)
                ret.Add(consulenza)
            End While
            dbRis.Dispose()

            Return ret
        End Function

        Public Function TrovaPratiche() As CCollection(Of CRapportino)
            Dim ret As New CCollection(Of CRapportino)
            Dim dbSQL As String
            Dim dbRis As System.Data.IDataReader
            Dim pratica As CRapportino

            If (Me.DataInizioLavorazione.HasValue OrElse Me.DataFineLavorazione.HasValue) Then
                dbSQL = ""
                dbSQL &= "SELECT [tbl_Rapportini].* FROM [tbl_Rapportini] "
                dbSQL &= " INNER JOIN "
                dbSQL &= "(SELECT [IDPratica] FROM [tbl_PraticheSTL] WHERE "
                If (Me.DataInizioLavorazione.HasValue) Then
                    dbSQL &= " [Data]>=" & DBUtils.DBDate(Me.DataInizioLavorazione.Value)
                    If (Me.DataFineLavorazione.HasValue) Then
                        dbSQL &= " AND [Data]<=" & DBUtils.DBDate(Me.DataFineLavorazione.Value)
                    End If
                Else
                    dbSQL &= " [Data]<=" & DBUtils.DBDate(Me.DataFineLavorazione.Value)
                End If

                dbSQL &= " GROUP BY [IDPratica]) AS [T1] ON [tbl_Rapportini].[ID] = [T1].[IDPratica]"

                dbSQL &= " WHERE [Stato]=" & ObjectStatus.OBJECT_VALID
            Else
                dbSQL = "SELECT * FROM [tbl_Rapportini] WHERE [Stato]=" & ObjectStatus.OBJECT_VALID
            End If
            If (Me.IDPuntoOperativo <> 0) Then dbSQL &= " AND [IDPuntoOperativo]=" & Me.IDPuntoOperativo
            dbSQL &= " AND [Cliente]=" & Me.IDCliente

            dbRis = CQSPD.Database.ExecuteReader(dbSQL)
            While dbRis.Read
                pratica = New CRapportino
                CQSPD.Database.Load(pratica, dbRis)
                ret.Add(pratica)
            End While
            dbRis.Dispose()

            Return ret
        End Function


        ''' <summary>
        ''' Restituisce o imposta l'operatore principale
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Operatore As CUser
            Get
                SyncLock Me
                    If Me.m_Operatore Is Nothing Then Me.m_Operatore = Sistema.Users.GetItemById(Me.m_IDOperatore)
                    Return Me.m_Operatore
                End SyncLock
            End Get
            Set(value As CUser)
                Dim oldValue As CUser
                SyncLock Me
                    oldValue = Me.Operatore
                    If (oldValue Is value) Then Exit Property
                    Me.m_Operatore = value
                    Me.m_IDOperatore = GetID(value)
                    If (value IsNot Nothing) Then Me.m_NomeOperatore = value.Nominativo
                End SyncLock
                Me.DoChanged("Operatore", value, oldValue)
            End Set
        End Property

        Public Property NomeOperatore As String
            Get
                Return Me.m_NomeOperatore
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeOperatore
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_NomeOperatore = value
                Me.DoChanged("NomeOperatore", value, oldValue)
            End Set
        End Property

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "IDCliente" : Me.m_IDCliente = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeCliente" : Me.m_NomeCliente = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IconaCliente" : Me.m_IconaCliente = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "StatoLavorazione" : Me.m_StatoLavorazione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "SottostatoLavorazione" : Me.m_SottostatoLavorazione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "DataInizioLavorazione" : Me.m_DataInizioLavorazione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataFineLavorazione" : Me.m_DataFineLavorazione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataUltimoAggiornamento" : Me.m_DataUltimoAggiornamento = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IDOperatore" : Me.m_IDOperatore = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeOperatore" : Me.m_NomeOperatore = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Operatori" : Me.m_Opeatori = Me.StrToArr(XML.Utils.Serializer.DeserializeString(fieldValue))
                Case "ArrayVisite" : Me.m_Visite = Me.StrToArr(XML.Utils.Serializer.DeserializeString(fieldValue))
                Case "ArrayRichiesteC" : Me.m_RichiesteConteggi = Me.StrToArr(XML.Utils.Serializer.DeserializeString(fieldValue))
                Case "ArrayRichiesteF" : Me.m_RichiesteFinanziamenti = Me.StrToArr(XML.Utils.Serializer.DeserializeString(fieldValue))
                Case "ArrayBustePaga" : Me.m_BustePaga = Me.StrToArr(XML.Utils.Serializer.DeserializeString(fieldValue))
                Case "ArrayOfferteInserite" : Me.m_OfferteInserite = Me.StrToArr(XML.Utils.Serializer.DeserializeString(fieldValue))
                Case "ArrayOfferteProposte" : Me.m_OfferteProposte = Me.StrToArr(XML.Utils.Serializer.DeserializeString(fieldValue))
                Case "ArrayOfferteRifiutate" : Me.m_OfferteRifiutateCliente = Me.StrToArr(XML.Utils.Serializer.DeserializeString(fieldValue))
                Case "ArrayOfferteBocciate" : Me.m_OfferteBocciate = Me.StrToArr(XML.Utils.Serializer.DeserializeString(fieldValue))
                Case "ArrayOfferteNonFatt" : Me.m_OfferteNonFattibili = Me.StrToArr(XML.Utils.Serializer.DeserializeString(fieldValue))
                Case "ArrayPraticheInCorso" : Me.m_PraticheInCorso = Me.StrToArr(XML.Utils.Serializer.DeserializeString(fieldValue))
                Case "ArrayPraticheLiquidate" : Me.m_PraticheLiquidate = Me.StrToArr(XML.Utils.Serializer.DeserializeString(fieldValue))
                Case "ArrayPraticheRifiutate" : Me.m_PraticheRifiutateCliente = Me.StrToArr(XML.Utils.Serializer.DeserializeString(fieldValue))
                Case "ArrayPraticheBocciate" : Me.m_PraticheBocciate = Me.StrToArr(XML.Utils.Serializer.DeserializeString(fieldValue))
                Case "ArrayPraticheNonFatt" : Me.m_PraticheNonFattibili = Me.StrToArr(XML.Utils.Serializer.DeserializeString(fieldValue))
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("IDCliente", Me.IDCliente)
            writer.WriteAttribute("NomeCliente", Me.m_NomeCliente)
            writer.WriteAttribute("IconaCliente", Me.m_IconaCliente)
            writer.WriteAttribute("StatoLavorazione", Me.m_StatoLavorazione)
            writer.WriteAttribute("SottostatoLavorazione", Me.m_SottostatoLavorazione)
            writer.WriteAttribute("DataInizioLavorazione", Me.m_DataInizioLavorazione)
            writer.WriteAttribute("DataFineLavorazione", Me.m_DataFineLavorazione)
            writer.WriteAttribute("DataUltimoAggiornamento", Me.m_DataUltimoAggiornamento)
            writer.WriteAttribute("IDOperatore", Me.IDOperatore)
            writer.WriteAttribute("NomeOperatore", Me.m_NomeOperatore)
            writer.WriteAttribute("Flags", Me.m_Flags)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Operatori", Me.ArrToStr(Me.m_Opeatori))
            writer.WriteTag("ArrayVisite", Me.ArrToStr(Me.m_Visite))
            writer.WriteTag("ArrayRichiesteC", Me.ArrToStr(Me.m_RichiesteConteggi))
            writer.WriteTag("ArrayRichiesteF", Me.ArrToStr(Me.m_RichiesteFinanziamenti))
            writer.WriteTag("ArrayBustePaga", Me.ArrToStr(Me.m_BustePaga))
            writer.WriteTag("ArrayOfferteInserite", Me.ArrToStr(Me.m_OfferteInserite))
            writer.WriteTag("ArrayOfferteProposte", Me.ArrToStr(Me.m_OfferteProposte))
            writer.WriteTag("ArrayOfferteRifiutate", Me.ArrToStr(Me.m_OfferteRifiutateCliente))
            writer.WriteTag("ArrayOfferteBocciate", Me.ArrToStr(Me.m_OfferteBocciate))
            writer.WriteTag("ArrayOfferteNonFatt", Me.ArrToStr(Me.m_OfferteNonFattibili))
            writer.WriteTag("ArrayPraticheInCorso", Me.ArrToStr(Me.m_PraticheInCorso))
            writer.WriteTag("ArrayPraticheLiquidate", Me.ArrToStr(Me.m_PraticheLiquidate))
            writer.WriteTag("ArrayPraticheRifiutate", Me.ArrToStr(Me.m_PraticheRifiutateCliente))
            writer.WriteTag("ArrayPraticheBocciate", Me.ArrToStr(Me.m_PraticheBocciate))
            writer.WriteTag("ArrayPraticheNonFatt", Me.ArrToStr(Me.m_PraticheNonFattibili))
        End Sub


        
        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Public Overrides Function GetModule() As CModule
            Return CQSPD.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CQSPDClientiLavorati"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_IDCliente = reader.Read("IDCliente", Me.m_IDCliente)
            Me.m_NomeCliente = reader.Read("NomeCliente", Me.m_NomeCliente)
            Me.m_IconaCliente = reader.Read("IconaCliente", Me.m_IconaCliente)
            Me.m_StatoLavorazione = reader.Read("StatoLavorazione", Me.m_StatoLavorazione)
            Me.m_SottostatoLavorazione = reader.Read("SottostatoLavorazione", Me.m_SottostatoLavorazione)
            Me.m_DataInizioLavorazione = reader.Read("DataInizioLavorazione", Me.m_DataInizioLavorazione)
            Me.m_DataFineLavorazione = reader.Read("DataFineLavorazione", Me.m_DataFineLavorazione)
            Me.m_DataUltimoAggiornamento = reader.Read("DataUltimoAggiornamento", Me.m_DataUltimoAggiornamento)
            Me.m_IDOperatore = reader.Read("IDOperatore", Me.m_IDOperatore)
            Me.m_NomeOperatore = reader.Read("NomeOperatore", Me.m_NomeOperatore)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)


            Me.m_Opeatori = Me.StrToArr(reader.Read("Operatori", ""))
            Me.m_Visite = Me.StrToArr(reader.Read("ArrayVisite", ""))
            Me.m_RichiesteConteggi = Me.StrToArr(reader.Read("ArrayRichiesteC", ""))
            Me.m_RichiesteFinanziamenti = Me.StrToArr(reader.Read("ArrayRichiesteF", ""))
            Me.m_BustePaga = Me.StrToArr(reader.Read("ArrayBustePaga", ""))

            Me.m_OfferteInserite = Me.StrToArr(reader.Read("ArrayOfferteInserite", ""))
            Me.m_OfferteProposte = Me.StrToArr(reader.Read("ArrayOfferteProposte", ""))
            Me.m_OfferteRifiutateCliente = Me.StrToArr(reader.Read("ArrayOfferteRifiutate", ""))
            Me.m_OfferteBocciate = Me.StrToArr(reader.Read("ArrayOfferteBocciate", ""))
            Me.m_OfferteNonFattibili = Me.StrToArr(reader.Read("ArrayOfferteNonFatt", ""))
            Me.m_PraticheInCorso = Me.StrToArr(reader.Read("ArrayPraticheInCorso", ""))
            Me.m_PraticheLiquidate = Me.StrToArr(reader.Read("ArrayPraticheLiquidate", ""))
            Me.m_PraticheRifiutateCliente = Me.StrToArr(reader.Read("ArrayPraticheRifiutate", ""))
            Me.m_PraticheBocciate = Me.StrToArr(reader.Read("ArrayPraticheBocciate", ""))
            Me.m_PraticheNonFattibili = Me.StrToArr(reader.Read("ArrayPraticheNonFatt", ""))

            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("IDCliente", Me.IDCliente)
            writer.Write("NomeCliente", Me.m_NomeCliente)
            writer.Write("IconaCliente", Me.m_IconaCliente)
            writer.Write("StatoLavorazione", Me.m_StatoLavorazione)
            writer.Write("SottostatoLavorazione", Me.m_SottostatoLavorazione)
            writer.Write("DataInizioLavorazione", Me.m_DataInizioLavorazione)
            writer.Write("DataFineLavorazione", Me.m_DataFineLavorazione)
            writer.Write("DataUltimoAggiornamento", Me.m_DataUltimoAggiornamento)
            writer.Write("IDOperatore", Me.IDOperatore)
            writer.Write("NomeOperatore", Me.m_NomeOperatore)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("Operatori", Me.ArrToStr(Me.m_Opeatori))
            writer.Write("ArrayVisite", Me.ArrToStr(Me.m_Visite))
            writer.Write("ArrayRichiesteC", Me.ArrToStr(Me.m_RichiesteConteggi))
            writer.Write("ArrayRichiesteF", Me.ArrToStr(Me.m_RichiesteFinanziamenti))
            writer.Write("ArrayBustePaga", Me.ArrToStr(Me.m_BustePaga))
            writer.Write("Operatori", Me.ArrToStr(Me.m_Opeatori))
            writer.Write("ArrayVisite", Me.ArrToStr(Me.m_Visite))
            writer.Write("ArrayRichiesteC", Me.ArrToStr(Me.m_RichiesteConteggi))
            writer.Write("ArrayRichiesteF", Me.ArrToStr(Me.m_RichiesteFinanziamenti))
            writer.Write("ArrayBustePaga", Me.ArrToStr(Me.m_BustePaga))
            writer.Write("ArrayOfferteInserite", Me.ArrToStr(Me.m_OfferteInserite))
            writer.Write("ArrayOfferteProposte", Me.ArrToStr(Me.m_OfferteProposte))
            writer.Write("ArrayOfferteRifiutate", Me.ArrToStr(Me.m_OfferteRifiutateCliente))
            writer.Write("ArrayOfferteBocciate", Me.ArrToStr(Me.m_OfferteBocciate))
            writer.Write("ArrayOfferteNonFatt", Me.ArrToStr(Me.m_OfferteNonFattibili))
            writer.Write("ArrayPraticheInCorso", Me.ArrToStr(Me.m_PraticheInCorso))
            writer.Write("ArrayPraticheLiquidate", Me.ArrToStr(Me.m_PraticheLiquidate))
            writer.Write("ArrayPraticheRifiutate", Me.ArrToStr(Me.m_PraticheRifiutateCliente))
            writer.Write("ArrayPraticheBocciate", Me.ArrToStr(Me.m_PraticheBocciate))
            writer.Write("ArrayPraticheNonFatt", Me.ArrToStr(Me.m_PraticheNonFattibili))

            writer.Write("NumeroVisite", Me.NumeroVisite)
            writer.Write("NumeroBustePaga", Me.NumeroBustePaga)
            writer.Write("NumeroRichiesteFinanziamento", Me.NumeroRichiesteFinanziamento)
            writer.Write("NumeroRichiesteConteggiEstintivi", Me.NumeroRichiesteConteggiEstintivi)
            writer.Write("NumeroStudiDiFattibilita", Me.NumeroOfferte)
            writer.Write("NumeroOfferteProposte", Me.NumeroOfferteProposte)
            'writer.Write("NumeroOfferteAccettate", Me.NumeroOffertePropost)
            writer.Write("NumeroOfferteRifiutate", Me.NumeroOfferteRifiutateCliente)
            writer.Write("NumeroOfferteNonFattibili", Me.NumeroOfferteNonFattibili)
            writer.Write("NumeroOfferteBocciate", Me.NumeroOfferteBocciate)

            writer.Write("NumeroPratiche", Me.NumeroPratiche)
            writer.Write("NumeroPraticheLiquidate", Me.NumeroPraticheLiquidate)
            writer.Write("NumeroPraticheAnnullate", Me.NumeroPraticheAnnullate)
            writer.Write("NumeroPraticheRifiutate", Me.NumeroPraticheRifiutateCliente)
            writer.Write("NumeroPraticheNonFattibili", Me.NumeroPraticheNonFattibili)
            writer.Write("NumeroPraticheBocciate", Me.NumeroPraticheBocciate)

            Return MyBase.SaveToRecordset(writer)
        End Function


        Private Function ContaPraticheLiquidate(ByVal items As CCollection(Of CRapportino)) As Integer
            Dim cnt As Integer = 0
            Dim stLiq As CStatoPratica = CQSPD.StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_LIQUIDATA)
            Dim stArch As CStatoPratica = CQSPD.StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_ARCHIVIATA)

            For Each p As CRapportino In items
                If (p.StatoAttuale Is stLiq OrElse p.StatoAttuale Is stArch) Then
                    cnt += 1
                End If
            Next

            Return cnt
        End Function

        Private Function ContaVisiteConsulenza(ByVal items As CCollection(Of CVisita)) As Integer
            Dim cnt As Integer = 0

            For Each v As CVisita In items
                If (Strings.InStr(LCase(v.Scopo), "consulenza") > 0) Then
                    cnt += 1
                End If
            Next

            Return cnt
        End Function

        Private Function ContaPraticheAnnullate(ByVal items As CCollection(Of CRapportino)) As String
            Dim cnt As Integer = 0
            Dim stAnn As CStatoPratica = CQSPD.StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_ANNULLATA)

            For Each p As CRapportino In items
                If (p.StatoAttuale Is stAnn) Then
                    cnt += 1
                End If
            Next

            Return cnt
        End Function

        Private Function ContaPraticheRifiutate(ByVal items As CCollection(Of CRapportino)) As String
            Dim cnt As Integer = 0
            Dim stAnn As CStatoPratica = CQSPD.StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_ANNULLATA)

            For Each p As CRapportino In items
                If (p.StatoAttuale Is stAnn AndAlso p.StatoDiLavorazioneAttuale.RegolaApplicata IsNot Nothing AndAlso TestFlag(p.StatoDiLavorazioneAttuale.RegolaApplicata.Flags, FlagsRegolaStatoPratica.DaCliente)) Then
                    cnt += 1
                End If
            Next

            Return cnt
        End Function

        Private Function GetData(ByVal r As CRapportino) As Date?
            If (r.StatoDiLavorazioneAttuale IsNot Nothing) Then Return r.StatoDiLavorazioneAttuale.Data
            Return r.DataDecorrenza
        End Function


        Private Function GetUltimaPratica(ByVal items As CCollection(Of CRapportino)) As CRapportino
            Dim last As CRapportino = Nothing
            For Each v As CRapportino In items
                If (last Is Nothing) OrElse Calendar.Compare(GetData(v), GetData(last)) > 0 Then
                    last = v
                End If
            Next
            Return last
        End Function

        Private Function GetUltimaVisita(ByVal items As CCollection(Of CVisita)) As CVisita
            Dim last As CVisita = Nothing
            For Each v As CVisita In items
                If (last Is Nothing) OrElse (v.Data > last.Data) Then
                    last = v
                End If
            Next
            Return last
        End Function

        Private Function GetUltimaRichiesta(ByVal items As CCollection(Of CRichiestaFinanziamento)) As CRichiestaFinanziamento
            Dim last As CRichiestaFinanziamento = Nothing
            For Each v As CRichiestaFinanziamento In items
                If (last Is Nothing) OrElse (v.Data > last.Data) Then
                    last = v
                End If
            Next
            Return last
        End Function

        Private Function GetData(ByVal c As CQSPDConsulenza) As Date?
            If (c.DataConsulenza.HasValue) Then Return c.DataConsulenza
            If (c.DataProposta.HasValue) Then Return c.DataProposta
            If (c.DataDecorrenza.HasValue) Then Return c.DataDecorrenza
            Return c.CreatoIl
        End Function

        Private Function GetUltimaConsulenza(ByVal items As CCollection(Of CQSPDConsulenza)) As CQSPDConsulenza
            Dim last As CQSPDConsulenza = Nothing
            For Each v As CQSPDConsulenza In items

                If (last Is Nothing) OrElse Calendar.Compare(GetData(v), GetData(last)) > 0 Then
                    last = v
                End If

            Next

            Return last
        End Function

        'Private Function GetPraticheInCorso() As Boolean?
        '    Dim cnt As Integer = 0
        '    Dim stLiq As CStatoPratica = CQSPD.StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_LIQUIDATA)
        '    Dim stArch As CStatoPratica = CQSPD.StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_ARCHIVIATA)

        '    For Each p As CRapportino In Me.Pratiche
        '        If (p.StatoAttuale Is stLiq OrElse p.StatoAttuale Is stArch) Then
        '            cnt += 1
        '        End If
        '    Next

        '    Return cnt
        'End Function

        'Private Function GetPraticheInCorso() As CCollection(Of CRapportino)
        '    Dim ret As New CCollection(Of CRapportino)
        '    Dim stAnn As CStatoPratica = CQSPD.StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_ANNULLATA)
        '    Dim stLiq As CStatoPratica = CQSPD.StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_LIQUIDATA)
        '    Dim stArch As CStatoPratica = CQSPD.StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_ARCHIVIATA)

        '    For Each p As CRapportino In Me.Pratiche
        '        If Not ((p.StatoAttuale Is stAnn) OrElse (p.StatoAttuale Is stLiq) OrElse (p.StatoAttuale Is stArch)) Then
        '            ret.Add(p)
        '        End If
        '    Next

        '    Return ret
        'End Function

        Private Function GetArrayIDPratiche(ByVal items As CCollection(Of CRapportino)) As Integer()
            Dim ret As New System.Collections.ArrayList
            For Each item As CRapportino In items
                ret.Add(GetID(item))
            Next
            Return ret.ToArray(GetType(Integer))
        End Function

        Private Function GetArrayIDConsulenze(ByVal items As CCollection(Of CQSPDConsulenza)) As Integer()
            Dim ret As New System.Collections.ArrayList
            For Each item As CQSPDConsulenza In items
                ret.Add(GetID(item))
            Next
            Return ret.ToArray(GetType(Integer))
        End Function

        Private Function GetArrayIDRichieste(ByVal items As CCollection(Of CRichiestaFinanziamento)) As Integer()
            Dim ret As New System.Collections.ArrayList
            For Each item As CRichiestaFinanziamento In items
                ret.Add(GetID(item))
            Next
            Return ret.ToArray(GetType(Integer))
        End Function

        Private Function GetArrayIDVisite(ByVal items As CCollection(Of CVisita)) As Integer()
            Dim ret As New System.Collections.ArrayList
            For Each item As CVisita In items
                ret.Add(GetID(item))
            Next
            Return ret.ToArray(GetType(Integer))
        End Function

        Private Function ContaPraticheBocciate(ByVal items As CCollection(Of CRapportino)) As Integer
            Dim cnt As Integer = 0
            Dim stAnn As CStatoPratica = CQSPD.StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_ANNULLATA)

            For Each p As CRapportino In items
                If (p.StatoAttuale Is stAnn AndAlso p.StatoDiLavorazioneAttuale.RegolaApplicata IsNot Nothing AndAlso TestFlag(p.StatoDiLavorazioneAttuale.RegolaApplicata.Flags, FlagsRegolaStatoPratica.Bocciata)) Then
                    cnt += 1
                End If
            Next

            Return cnt
        End Function

        Private Function ContaPraticheNonFattibili(ByVal items As CCollection(Of CRapportino)) As Integer
            Dim cnt As Integer = 0
            Dim stAnn As CStatoPratica = CQSPD.StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_ANNULLATA)

            For Each p As CRapportino In items
                If (p.StatoAttuale Is stAnn AndAlso p.StatoDiLavorazioneAttuale.RegolaApplicata IsNot Nothing AndAlso TestFlag(p.StatoDiLavorazioneAttuale.RegolaApplicata.Flags, FlagsRegolaStatoPratica.NonFattibile)) Then
                    cnt += 1
                End If
            Next

            Return cnt
        End Function

        Private Function ContaConsulenzeNonFattibili(ByVal items As CCollection(Of CQSPDConsulenza)) As Integer
            Dim cnt As Integer = 0
            For Each c As CQSPDConsulenza In items
                If (c.StatoConsulenza = StatiConsulenza.BOCCIATA) Then
                    cnt += 1
                End If
            Next
            Return cnt
        End Function

        Private Function ContaConsulenzeRifiutate(ByVal items As CCollection(Of CQSPDConsulenza)) As Integer
            Dim cnt As Integer = 0
            For Each c As CQSPDConsulenza In items
                If (c.StatoConsulenza = StatiConsulenza.RIFIUTATA) Then
                    cnt += 1
                End If
            Next
            Return cnt
        End Function

        Private Function ContaConsulenzeBocciate(ByVal items As CCollection(Of CQSPDConsulenza)) As Integer
            Dim cnt As Integer = 0
            For Each c As CQSPDConsulenza In items
                If (c.StatoConsulenza = StatiConsulenza.BOCCIATA) Then
                    cnt += 1
                End If
            Next
            Return cnt
        End Function

        Private Function ContaConsulenzeAccettate(ByVal items As CCollection(Of CQSPDConsulenza)) As Integer
            Dim cnt As Integer = 0
            For Each c As CQSPDConsulenza In items
                If (StatiConsulenza.ACCETTATA) Then
                    cnt += 1
                End If
            Next
            Return cnt
        End Function

        Private Function ContaConsulenzeProposte(ByVal items As CCollection(Of CQSPDConsulenza)) As Integer
            Dim cnt As Integer = 0
            For Each c As CQSPDConsulenza In items
                If (c.StatoConsulenza = StatiConsulenza.PROPOSTA) Then
                    cnt += 1
                End If
            Next
            Return cnt
        End Function

        Private Function ArrToStr(ByVal items() As Integer) As String
            Dim ret As New System.Text.StringBuilder
            Dim len As Integer = Arrays.Len(items)
            For i As Integer = 0 To len - 1
                If (i > 0) Then ret.Append(",")
                ret.Append(CStr(items(i)))
            Next
            Return ret.ToString
        End Function

        Private Function StrToArr(ByVal text As String) As Integer()
            text = Strings.Trim(text)
            If (text = "") Then Return Nothing
            Dim ret As New ArrayList
            Dim tmp() As String = Split(text, ",")
            For i As Integer = 0 To UBound(tmp)
                If Formats.ToInteger(tmp(i)) <> 0 Then
                    ret.Add(Formats.ToInteger(tmp(i)))
                End If
            Next
            Return ret.ToArray(GetType(Integer))
        End Function

        ''' <summary>
        ''' Notifica a questo oggetto la visita del cliente
        ''' </summary>
        ''' <param name="visita"></param>
        ''' <remarks></remarks>
        Public Sub NotifyVisita(ByVal visita As CVisita)
            If (visita.Stato = ObjectStatus.OBJECT_VALID AndAlso Calendar.CheckBetween(visita.Data, Me.m_DataInizioLavorazione, Me.m_DataFineLavorazione)) Then
                Me.m_Visite = Me.AddIfNotIn(Me.m_Visite, GetID(visita))
                Me.m_Opeatori = Me.AddIfNotIn(Me.m_Opeatori, visita.IDOperatore)
                Me.DataUltimoAggiornamento = Calendar.Max(Me.m_DataUltimoAggiornamento, visita.Data)

                If (Me.StatoLavorazione <= CQSPD.StatoLavorazione.Visita) Then
                    Me.Operatore = visita.Operatore
                End If

                Me.StatoLavorazione = Me.CalcolaStato
                Me.SottostatoLavorazione = Me.CalcolaSottostato
            Else
                Me.m_Visite = Me.RemoveIfIn(Me.m_Visite, GetID(visita))
            End If


        End Sub


        ''' <summary>
        ''' Notifica a questo oggetto il caricamento della busta paga
        ''' </summary>
        ''' <param name="a"></param>
        ''' <remarks></remarks>
        Public Sub NotifyBustaPaga(ByVal a As CAttachment)
            If (a.Stato = ObjectStatus.OBJECT_VALID AndAlso Calendar.CheckBetween(a.DataInizio, Me.m_DataInizioLavorazione, Me.m_DataFineLavorazione)) Then
                Me.m_BustePaga = Me.AddIfNotIn(Me.m_BustePaga, GetID(a))
                Me.m_Opeatori = Me.AddIfNotIn(Me.m_Opeatori, a.CreatoDaId)
                Me.DataUltimoAggiornamento = Calendar.Max(Me.m_DataUltimoAggiornamento, a.DataInizio)
                If (Me.StatoLavorazione <= CQSPD.StatoLavorazione.BustaPaga) Then
                    Me.Operatore = a.CreatoDa
                End If

                Me.StatoLavorazione = Me.CalcolaStato
                Me.SottostatoLavorazione = Me.CalcolaSottostato
            Else
                Me.m_BustePaga = Me.RemoveIfIn(Me.m_BustePaga, GetID(a))
            End If

        End Sub

        Public Sub NotifyRichiestaFinanziamento(ByVal richiesta As CRichiestaFinanziamento)
            If (richiesta.Stato = ObjectStatus.OBJECT_VALID AndAlso Calendar.CheckBetween(richiesta.Data, Me.m_DataInizioLavorazione, Me.m_DataFineLavorazione)) Then
                Me.m_RichiesteFinanziamenti = Me.AddIfNotIn(Me.m_RichiesteFinanziamenti, GetID(richiesta))
                Me.m_Opeatori = Me.AddIfNotIn(Me.m_Opeatori, richiesta.IDAssegnatoA)
                Me.DataUltimoAggiornamento = Calendar.Max(Me.m_DataUltimoAggiornamento, richiesta.Data)

                If (Me.StatoLavorazione <= CQSPD.StatoLavorazione.RichiestaFinanziamento) Then
                    Me.Operatore = richiesta.AssegnatoA
                End If

                Me.StatoLavorazione = Me.CalcolaStato
                Me.SottostatoLavorazione = Me.CalcolaSottostato
            Else
                Me.m_RichiesteFinanziamenti = Me.RemoveIfIn(Me.m_RichiesteFinanziamenti, GetID(richiesta))
            End If
        End Sub

        Private Function AddIfNotIn(ByVal items As Integer(), ByVal id As Integer) As Integer()
            If (id = 0) Then Return items

            Dim i As Integer = Arrays.BinarySearch(items, id)
            If (i < 0) Then
                items = Arrays.Push(items, id)
                Array.Sort(items)
                Me.SetChanged(True)
            End If
            Return items
        End Function

        Private Function RemoveIfIn(ByVal items As Integer(), ByVal id As Integer) As Integer()
            If (id = 0) Then Return items

            Dim i As Integer = Arrays.BinarySearch(items, id)
            If (i >= 0) Then
                items = Arrays.RemoveAt(items, i)
                Me.SetChanged(True)
            End If
            Return items
        End Function

        Private Function IsIn(ByVal items As Integer(), ByVal id As Integer) As Boolean
            If (items Is Nothing OrElse id = 0) Then Return False
            Return Arrays.BinarySearch(items, id) >= 0
        End Function

        Public Sub NotifyConsulenza(ByVal consulenza As CQSPDConsulenza)
            If (consulenza Is Nothing) Then Throw New ArgumentNullException("consulenza")
            Dim id As Integer = GetID(consulenza)
            If (id = 0) Then Exit Sub

            If (consulenza.Stato = ObjectStatus.OBJECT_VALID AndAlso Calendar.CheckBetween(consulenza.DataConsulenza, Me.m_DataInizioLavorazione, Me.m_DataFineLavorazione)) Then
                Me.m_Opeatori = Me.AddIfNotIn(Me.m_Opeatori, consulenza.IDAnnullataDa)
                Me.m_Opeatori = Me.AddIfNotIn(Me.m_Opeatori, consulenza.IDConfermataDa)
                Me.m_Opeatori = Me.AddIfNotIn(Me.m_Opeatori, consulenza.IDInseritoDa)
                Me.m_Opeatori = Me.AddIfNotIn(Me.m_Opeatori, consulenza.IDPropostaDa)
                If (consulenza.Consulente IsNot Nothing) Then
                    Me.m_Opeatori = Me.AddIfNotIn(Me.m_Opeatori, consulenza.Consulente.IDUser)
                End If


                Select Case consulenza.StatoConsulenza
                    Case StatiConsulenza.INSERITA
                        Me.DataUltimoAggiornamento = Calendar.Max(Me.m_DataUltimoAggiornamento, consulenza.DataConsulenza)
                        Me.m_OfferteInserite = Me.AddIfNotIn(Me.m_OfferteInserite, id)
                        Me.m_OfferteProposte = Me.RemoveIfIn(Me.m_OfferteProposte, id)
                        Me.m_OfferteRifiutateCliente = Me.RemoveIfIn(Me.m_OfferteRifiutateCliente, id)
                        Me.m_OfferteBocciate = Me.RemoveIfIn(Me.m_OfferteBocciate, id)
                        Me.m_OfferteNonFattibili = Me.RemoveIfIn(Me.m_OfferteNonFattibili, id)
                    Case StatiConsulenza.PROPOSTA, StatiConsulenza.ACCETTATA
                        Me.DataUltimoAggiornamento = Calendar.Max(Me.m_DataUltimoAggiornamento, consulenza.DataProposta)
                        Me.DataUltimoAggiornamento = Calendar.Max(Me.m_DataUltimoAggiornamento, consulenza.DataConferma)
                        If consulenza.DataAnnullamento.HasValue Then
                            Me.m_OfferteInserite = Me.RemoveIfIn(Me.m_OfferteInserite, id)
                            Me.m_OfferteProposte = Me.RemoveIfIn(Me.m_OfferteProposte, id)
                            Me.m_OfferteRifiutateCliente = Me.RemoveIfIn(Me.m_OfferteRifiutateCliente, id)
                            Me.m_OfferteBocciate = Me.AddIfNotIn(Me.m_OfferteBocciate, id)
                            Me.m_OfferteNonFattibili = Me.RemoveIfIn(Me.m_OfferteNonFattibili, id)
                        Else
                            Me.m_OfferteInserite = Me.RemoveIfIn(Me.m_OfferteInserite, id)
                            Me.m_OfferteProposte = Me.AddIfNotIn(Me.m_OfferteProposte, id)
                            Me.m_OfferteRifiutateCliente = Me.RemoveIfIn(Me.m_OfferteRifiutateCliente, id)
                            Me.m_OfferteBocciate = Me.RemoveIfIn(Me.m_OfferteBocciate, id)
                            Me.m_OfferteNonFattibili = Me.RemoveIfIn(Me.m_OfferteNonFattibili, id)
                        End If
                    Case StatiConsulenza.RIFIUTATA
                        Me.m_OfferteInserite = Me.RemoveIfIn(Me.m_OfferteInserite, id)
                        Me.m_OfferteProposte = Me.RemoveIfIn(Me.m_OfferteProposte, id)
                        Me.m_OfferteRifiutateCliente = Me.AddIfNotIn(Me.m_OfferteRifiutateCliente, id)
                        Me.m_OfferteBocciate = Me.RemoveIfIn(Me.m_OfferteBocciate, id)
                        Me.m_OfferteNonFattibili = Me.RemoveIfIn(Me.m_OfferteNonFattibili, id)
                        Me.DataUltimoAggiornamento = Calendar.Max(Me.m_DataUltimoAggiornamento, consulenza.DataConferma)
                        Me.DataUltimoAggiornamento = Calendar.Max(Me.m_DataUltimoAggiornamento, consulenza.DataAnnullamento)
                    Case StatiConsulenza.NONFATTIBILE
                        Me.m_OfferteInserite = Me.RemoveIfIn(Me.m_OfferteInserite, id)
                        Me.m_OfferteProposte = Me.RemoveIfIn(Me.m_OfferteProposte, id)
                        Me.m_OfferteRifiutateCliente = Me.RemoveIfIn(Me.m_OfferteRifiutateCliente, id)
                        Me.m_OfferteBocciate = Me.RemoveIfIn(Me.m_OfferteBocciate, id)
                        Me.m_OfferteNonFattibili = Me.AddIfNotIn(Me.m_OfferteNonFattibili, id)
                        Me.DataUltimoAggiornamento = Calendar.Max(Me.m_DataUltimoAggiornamento, consulenza.DataConferma)
                        Me.DataUltimoAggiornamento = Calendar.Max(Me.m_DataUltimoAggiornamento, consulenza.DataAnnullamento)
                    Case Else
                        Me.m_OfferteInserite = Me.RemoveIfIn(Me.m_OfferteInserite, id)
                        Me.m_OfferteProposte = Me.RemoveIfIn(Me.m_OfferteProposte, id)
                        Me.m_OfferteRifiutateCliente = Me.RemoveIfIn(Me.m_OfferteRifiutateCliente, id)
                        Me.m_OfferteBocciate = Me.AddIfNotIn(Me.m_OfferteBocciate, id)
                        Me.m_OfferteNonFattibili = Me.RemoveIfIn(Me.m_OfferteNonFattibili, id)
                        Me.DataUltimoAggiornamento = Calendar.Max(Me.m_DataUltimoAggiornamento, consulenza.DataConferma)
                        Me.DataUltimoAggiornamento = Calendar.Max(Me.m_DataUltimoAggiornamento, consulenza.DataAnnullamento)
                End Select

                If (Me.StatoLavorazione <= CQSPD.StatoLavorazione.StudioDiFattibilita) Then
                    If consulenza.Consulente IsNot Nothing Then Me.Operatore = consulenza.Consulente.User
                End If

                Me.StatoLavorazione = Me.CalcolaStato
                Me.SottostatoLavorazione = Me.CalcolaSottostato

                Select Case Me.SottostatoLavorazione
                    Case CQSPD.SottostatoLavorazione.Completato, CQSPD.SottostatoLavorazione.BocciatoAgenzia, CQSPD.SottostatoLavorazione.BocciatoIstituto, CQSPD.SottostatoLavorazione.NonFattibile, CQSPD.SottostatoLavorazione.RifiutatoDalCliente
                        Me.DataUltimoAggiornamento = Me.DataUltimoAggiornamento
                End Select
            Else
                Me.m_OfferteInserite = Me.RemoveIfIn(Me.m_OfferteInserite, id)
                Me.m_OfferteProposte = Me.RemoveIfIn(Me.m_OfferteProposte, id)
                Me.m_OfferteRifiutateCliente = Me.RemoveIfIn(Me.m_OfferteRifiutateCliente, id)
                Me.m_OfferteBocciate = Me.RemoveIfIn(Me.m_OfferteBocciate, id)
                Me.m_OfferteNonFattibili = Me.RemoveIfIn(Me.m_OfferteNonFattibili, id)
            End If
        End Sub

        Private Function GetDataContatto(ByVal p As CRapportino) As Date
            Dim d As Date? = Nothing
            If (p.StatoContatto IsNot Nothing) Then d = p.StatoContatto.Data
            If (d.HasValue = False) Then
                For Each stl As CStatoLavorazionePratica In p.StatiDiLavorazione
                    d = Calendar.Min(d, stl.Data)
                Next
            End If
            If d.HasValue = False Then d = p.CreatoIl
            Return d.Value
        End Function

        Public Sub NotifyPratica(ByVal item As CRapportino)
            If (item.Stato = ObjectStatus.OBJECT_VALID AndAlso Calendar.CheckBetween(Me.GetDataContatto(item), Me.m_DataInizioLavorazione, Me.m_DataFineLavorazione)) Then
                If (item.StatoDiLavorazioneAttuale IsNot Nothing) Then Me.m_Opeatori = Me.AddIfNotIn(Me.m_Opeatori, item.StatoDiLavorazioneAttuale.IDOperatore)

                Select Case item.StatoAttuale.MacroStato
                    Case StatoPraticaEnum.STATO_LIQUIDATA, StatoPraticaEnum.STATO_ARCHIVIATA
                        Me.m_PraticheInCorso = Me.RemoveIfIn(Me.m_PraticheInCorso, GetID(item))
                        Me.m_PraticheLiquidate = Me.AddIfNotIn(Me.m_PraticheLiquidate, GetID(item))
                        Me.m_PraticheBocciate = Me.RemoveIfIn(Me.m_PraticheBocciate, GetID(item))
                        Me.m_PraticheNonFattibili = Me.RemoveIfIn(Me.m_PraticheNonFattibili, GetID(item))
                        Me.m_PraticheRifiutateCliente = Me.RemoveIfIn(Me.m_PraticheRifiutateCliente, GetID(item))
                        If item.StatoLiquidata IsNot Nothing Then
                            Me.DataUltimoAggiornamento = Calendar.Max(Me.m_DataUltimoAggiornamento, item.StatoLiquidata.Data)
                        End If
                    Case StatoPraticaEnum.STATO_ANNULLATA

                        If item.StatoDiLavorazioneAttuale.RegolaApplicata IsNot Nothing Then
                            If TestFlag(item.StatoDiLavorazioneAttuale.RegolaApplicata.Flags, FlagsRegolaStatoPratica.DaCliente) Then
                                Me.m_PraticheInCorso = Me.RemoveIfIn(Me.m_PraticheInCorso, GetID(item))
                                Me.m_PraticheLiquidate = Me.RemoveIfIn(Me.m_PraticheLiquidate, GetID(item))
                                Me.m_PraticheBocciate = Me.RemoveIfIn(Me.m_PraticheBocciate, GetID(item))
                                Me.m_PraticheNonFattibili = Me.RemoveIfIn(Me.m_PraticheNonFattibili, GetID(item))
                                Me.m_PraticheRifiutateCliente = Me.AddIfNotIn(Me.m_PraticheRifiutateCliente, GetID(item))
                            ElseIf TestFlag(item.StatoDiLavorazioneAttuale.RegolaApplicata.Flags, FlagsRegolaStatoPratica.Bocciata) Then
                                Me.m_PraticheInCorso = Me.RemoveIfIn(Me.m_PraticheInCorso, GetID(item))
                                Me.m_PraticheLiquidate = Me.RemoveIfIn(Me.m_PraticheLiquidate, GetID(item))
                                Me.m_PraticheBocciate = Me.AddIfNotIn(Me.m_PraticheBocciate, GetID(item))
                                Me.m_PraticheNonFattibili = Me.RemoveIfIn(Me.m_PraticheNonFattibili, GetID(item))
                                Me.m_PraticheRifiutateCliente = Me.RemoveIfIn(Me.m_PraticheRifiutateCliente, GetID(item))
                            Else
                                Me.m_PraticheInCorso = Me.RemoveIfIn(Me.m_PraticheInCorso, GetID(item))
                                Me.m_PraticheLiquidate = Me.RemoveIfIn(Me.m_PraticheLiquidate, GetID(item))
                                Me.m_PraticheBocciate = Me.RemoveIfIn(Me.m_PraticheBocciate, GetID(item))
                                Me.m_PraticheNonFattibili = Me.AddIfNotIn(Me.m_PraticheNonFattibili, GetID(item))
                                Me.m_PraticheRifiutateCliente = Me.RemoveIfIn(Me.m_PraticheRifiutateCliente, GetID(item))
                            End If
                        Else

                            Me.m_PraticheInCorso = Me.RemoveIfIn(Me.m_PraticheInCorso, GetID(item))
                            Me.m_PraticheLiquidate = Me.RemoveIfIn(Me.m_PraticheLiquidate, GetID(item))
                            Me.m_PraticheBocciate = Me.RemoveIfIn(Me.m_PraticheBocciate, GetID(item))
                            Me.m_PraticheNonFattibili = Me.AddIfNotIn(Me.m_PraticheNonFattibili, GetID(item))
                            Me.m_PraticheRifiutateCliente = Me.RemoveIfIn(Me.m_PraticheRifiutateCliente, GetID(item))
                        End If

                        If item.StatoAnnullata IsNot Nothing Then
                            Me.DataUltimoAggiornamento = Calendar.Max(Me.m_DataUltimoAggiornamento, item.StatoAnnullata.Data)
                        End If
                    Case Else
                        Me.m_PraticheInCorso = Me.AddIfNotIn(Me.m_PraticheInCorso, GetID(item))
                        Me.m_PraticheLiquidate = Me.RemoveIfIn(Me.m_PraticheLiquidate, GetID(item))
                        Me.m_PraticheBocciate = Me.RemoveIfIn(Me.m_PraticheBocciate, GetID(item))
                        Me.m_PraticheNonFattibili = Me.RemoveIfIn(Me.m_PraticheNonFattibili, GetID(item))
                        Me.m_PraticheRifiutateCliente = Me.RemoveIfIn(Me.m_PraticheRifiutateCliente, GetID(item))


                End Select

                If item.Consulente IsNot Nothing Then Me.Operatore = item.Consulente.User
                If item.StatoDiLavorazioneAttuale IsNot Nothing Then
                    Me.DataUltimoAggiornamento = Calendar.Max(Me.m_DataUltimoAggiornamento, item.StatoDiLavorazioneAttuale.Data)
                End If

                Me.StatoLavorazione = Me.CalcolaStato
                Me.SottostatoLavorazione = Me.CalcolaSottostato

                Select Case Me.SottostatoLavorazione
                    Case CQSPD.SottostatoLavorazione.Completato, CQSPD.SottostatoLavorazione.BocciatoAgenzia, CQSPD.SottostatoLavorazione.BocciatoIstituto, CQSPD.SottostatoLavorazione.NonFattibile, CQSPD.SottostatoLavorazione.RifiutatoDalCliente
                        Me.DataUltimoAggiornamento = Me.DataUltimoAggiornamento
                End Select
            Else
                Me.m_PraticheInCorso = Me.RemoveIfIn(Me.m_PraticheInCorso, GetID(item))
                Me.m_PraticheLiquidate = Me.RemoveIfIn(Me.m_PraticheLiquidate, GetID(item))
                Me.m_PraticheRifiutateCliente = Me.RemoveIfIn(Me.m_PraticheRifiutateCliente, GetID(item))
                Me.m_PraticheBocciate = Me.RemoveIfIn(Me.m_PraticheBocciate, GetID(item))
                Me.m_PraticheNonFattibili = Me.RemoveIfIn(Me.m_PraticheNonFattibili, GetID(item))
            End If
        End Sub

        Public Sub NotifyRichiestaConteggio(ByVal richiesta As CRichiestaConteggio)
            If (richiesta.Stato = ObjectStatus.OBJECT_VALID AndAlso Calendar.CheckBetween(richiesta.DataRichiesta, Me.m_DataInizioLavorazione, Me.m_DataFineLavorazione)) Then
                Me.m_RichiesteConteggi = Me.AddIfNotIn(Me.m_RichiesteConteggi, GetID(richiesta))
                Me.m_Opeatori = Me.AddIfNotIn(Me.m_Opeatori, richiesta.PresaInCaricoDaID)
                Me.DataUltimoAggiornamento = Calendar.Max(Me.m_DataUltimoAggiornamento, richiesta.DataRichiesta)
            Else
                Me.m_RichiesteConteggi = Me.RemoveIfIn(Me.m_RichiesteConteggi, GetID(richiesta))
            End If
        End Sub

        Private Function MergeArrays(ByVal arr1 As Integer(), ByVal arr2() As Integer) As Integer()
            Dim len2 As Integer = Arrays.Len(arr2)
            For i As Integer = 0 To len2 - 1
                arr1 = Me.AddIfNotIn(arr1, arr2(i))
            Next
            Return arr1
        End Function

        Public Sub MergeWith(ByVal item As ClientiLavoratiStatsItem)
            If (item Is Nothing) Then Throw New ArgumentNullException("item")

            Me.m_DataInizioLavorazione = Calendar.Min(Me.m_DataInizioLavorazione, item.m_DataInizioLavorazione)
            Me.m_DataFineLavorazione = Calendar.Max(Me.m_DataFineLavorazione, item.m_DataFineLavorazione)
            Me.m_DataUltimoAggiornamento = Calendar.Max(Me.m_DataUltimoAggiornamento, item.m_DataUltimoAggiornamento)

            Me.m_Opeatori = Me.MergeArrays(Me.m_Opeatori, item.m_Opeatori)
            Me.m_Visite = Me.MergeArrays(Me.m_Visite, item.m_Visite)
            Me.m_RichiesteConteggi = Me.MergeArrays(Me.m_RichiesteConteggi, item.m_RichiesteConteggi)
            Me.m_RichiesteFinanziamenti = Me.MergeArrays(Me.m_RichiesteFinanziamenti, item.m_RichiesteFinanziamenti)
            Me.m_BustePaga = Me.MergeArrays(Me.m_BustePaga, item.m_BustePaga)
            Me.m_OfferteInserite = Me.MergeArrays(Me.m_OfferteInserite, item.m_OfferteInserite)
            Me.m_OfferteProposte = Me.MergeArrays(Me.m_OfferteProposte, item.m_OfferteProposte)
            Me.m_OfferteRifiutateCliente = Me.MergeArrays(Me.m_OfferteRifiutateCliente, item.m_OfferteRifiutateCliente)
            Me.m_OfferteBocciate = Me.MergeArrays(Me.m_OfferteBocciate, item.m_OfferteBocciate)
            Me.m_OfferteNonFattibili = Me.MergeArrays(Me.m_OfferteNonFattibili, item.m_OfferteNonFattibili)
            Me.m_PraticheInCorso = Me.MergeArrays(Me.m_PraticheInCorso, item.m_PraticheInCorso)
            Me.m_PraticheLiquidate = Me.MergeArrays(Me.m_PraticheLiquidate, item.m_PraticheLiquidate)
            Me.m_PraticheRifiutateCliente = Me.MergeArrays(Me.m_PraticheRifiutateCliente, item.m_PraticheRifiutateCliente)
            Me.m_PraticheBocciate = Me.MergeArrays(Me.m_PraticheBocciate, item.m_PraticheBocciate)
            Me.m_PraticheNonFattibili = Me.MergeArrays(Me.m_PraticheNonFattibili, item.m_PraticheNonFattibili)

            Me.StatoLavorazione = Me.CalcolaStato()
            Me.SottostatoLavorazione = Me.CalcolaSottostato

        End Sub

    End Class




End Class

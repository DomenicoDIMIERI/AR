Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD

    <Flags> _
    Public Enum ConsulenzeFlags As Integer
        NOTSET = 0
        HIDDEN = 1
    End Enum

    Public Enum StatiConsulenza As Integer
        ''' <summary>
        ''' Si tratta di una simulazione non ancora proposta al cliente (es. prove)
        ''' </summary>
        ''' <remarks></remarks>
        INSERITA = 0

        ''' <summary>
        ''' La simulazione è stata proposta al cliente
        ''' </summary>
        ''' <remarks></remarks>
        PROPOSTA = 1

        ''' <summary>
        ''' Il cliente ha accettato la proposta
        ''' </summary>
        ''' <remarks></remarks>
        ACCETTATA = 2

        ''' <summary>
        ''' La proposta fatta al cliente è stata da egli rifiutata
        ''' </summary>
        ''' <remarks></remarks>
        RIFIUTATA = 3

        ''' <summary>
        ''' La proposta è stata bocciata dall'agenzia (non fattibile)
        ''' </summary>
        ''' <remarks></remarks>
        BOCCIATA = 4

        ''' <summary>
        ''' La proposta non è fattibile
        ''' </summary>
        ''' <remarks></remarks>
        NONFATTIBILE = 5

    End Enum

    <Serializable> _
    Public Class CQSPDConsulenza
        Inherits DBObjectPO
        Implements IEstintore, IOggettoApprovabile, IComparable, IOggettoVerificabile  '(Of CQSPDConsulenza)

        ''' <summary>
        ''' Evento generato quando la consulenza viene inserita
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event Inserita(ByVal sender As Object, ByVal e As ItemEventArgs)

        ''' <summary>
        ''' Evento generato quando la consulenza viene proposta al cliente
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event Proposta(ByVal sender As Object, ByVal e As ItemEventArgs)

        ''' <summary>
        ''' Evento generato quando la consulenza viene accettata dal cliente
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event Accettata(ByVal sender As Object, ByVal e As ItemEventArgs)

        ''' <summary>
        ''' Evento generato quando la consulenza viene bocciata dall'operatore
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event Bocciata(sender As Object, e As ItemEventArgs)

        ''' <summary>
        ''' Evento generato quando la consulenza viene rifiutata dal cliente
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event Rifiutata(ByVal sender As Object, ByVal e As ItemEventArgs)



        ''' <summary>
        ''' Evento generato quando l'offerta richiede l'approvazione di un supervisore
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event RequireApprovation(sender As Object, e As ItemEventArgs) Implements IOggettoApprovabile.RequireApprovation

        ''' <summary>
        ''' Evento generato quando la consulenza viene presa in carico da un supervisore
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event PresaInCarico(sender As Object, e As ItemEventArgs) Implements IOggettoApprovabile.PresaInCarico

        ''' <summary>
        ''' Evento generato quando la consulenza viene approvata dal supervisore
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event Approvata(sender As Object, e As ItemEventArgs) Implements IOggettoApprovabile.Approvata

        ''' <summary>
        ''' Evento generato quando la consulenza viene bocciata dal supervisore o dall'operatore
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event Negata(sender As Object, e As ItemEventArgs) Implements IOggettoApprovabile.Rifiutata

        Private m_IDCliente As Integer                  'ID del cliente
        Private m_Cliente As CPersonaFisica             'Cliente
        Private m_NomeCliente As String           'Nome del cliente
        Private m_IDRichiesta As Integer                'ID della richiesta da cui è partita la consulenza
        Private m_Richiesta As CRichiestaFinanziamento  'Richiesta da cui è partita la consulenza
        Private m_IDConsulente As Integer               'ID dell'operatore di consulenza
        Private m_Consulente As CConsulentePratica      'Consulente
        Private m_NomeConsulente As String              'Nominativo del consulente
        Private m_DataConsulenza As Nullable(Of Date)   'Data in cui si è fatta la consulenza
        Private m_DataProposta As Nullable(Of Date)     'Data della proposta al cliente
        Private m_DataConferma As Nullable(Of Date)     'Data in cui il cliente ha accettato o ha rifiutato l'offerta
        Private m_Descrizione As String
        Private m_Flags As ConsulenzeFlags
        Private m_StatoConsulenza As StatiConsulenza

        Private m_OffertaCQS As COffertaCQS
        Private m_IDOffertaCQS As Integer
        'Private m_IDProdottoCQS As Integer
        'Private m_NomeProdottoCQS As String

        Private m_OffertaPD As COffertaCQS
        Private m_IDOffertaPD As Integer
        'Private m_IDProdottoPD As Integer
        'Private m_NomeProdottoPD As String

        Private m_MontanteLordo As Decimal
        Private m_NettoRicavo As Decimal
        Private m_SommaEstinzioni As Decimal
        Private m_SommaTrattenuteVolontarie As Decimal
        Private m_SommaPignoramenti As Decimal
        Private m_StipendioNetto As Decimal
        Private m_TFR As Decimal
        Private m_ValutazioneGARF As Integer
        Private m_TipoImpiego As String
        Private m_Eta As Single
        Private m_Anzianita As Single
        Private m_IDAzienda As Integer
        Private m_Azienda As CAzienda
        Private m_NomeAzienda As String
        Private m_DataAssunzione As Date?
        Private m_PropostaDa As CUser
        Private m_IDPropostaDa As Integer
        Private m_ConfermataDa As CUser
        Private m_IDConfermataDa As Integer
        Private m_IDContesto As Integer
        Private m_TipoContesto As String
        Private m_Durata As Double
        Private m_IDStudioDiFattibilita As Integer
        Private m_StudioDiFattibilita As CQSPDStudioDiFattibilita
        Private m_IDInseritoDa As Integer
        Private m_InseritoDa As CUser
        Private m_IDRichiestaApprovazione As Integer
        Private m_RichiestaApprovazione As CRichiestaApprovazione
        Private m_MotivoAnnullamento As String
        Private m_DettaglioAnnullamento As String
        Private m_Estinzioni As CEstinzioniXEstintoreCollection
        Private m_IDAnnullataDa As Integer
        Private m_AnnullataDa As CUser
        Private m_NomeAnnullataDa As String
        Private m_DataAnnullamento As Date?
        Private m_IDFinestraLavorazione As Integer
        Private m_FinestraLavorazione As FinestraLavorazione
        Private m_IDUltimaVerifica As Integer
        Private m_UltimaVerifica As VerificaAmministrativa

        Public Sub New()
            Me.m_IDCliente = 0
            Me.m_Cliente = Nothing
            Me.m_NomeCliente = ""
            Me.m_IDRichiesta = 0
            Me.m_Richiesta = Nothing
            Me.m_IDConsulente = 0
            Me.m_Consulente = Nothing
            Me.m_NomeConsulente = ""
            Me.m_DataConsulenza = Nothing
            Me.m_DataProposta = Nothing
            Me.m_DataConferma = Nothing
            Me.m_Descrizione = ""
            Me.m_Flags = ConsulenzeFlags.NOTSET
            Me.m_StatoConsulenza = StatiConsulenza.INSERITA
            Me.m_OffertaCQS = Nothing
            Me.m_IDOffertaCQS = 0
            'Me.m_IDProdottoCQS = 0
            'Me.m_NomeProdottoCQS = ""
            Me.m_OffertaPD = Nothing
            Me.m_IDOffertaPD = 0
            'Me.m_IDProdottoPD = 0
            'Me.m_NomeProdottoPD = ""
            Me.m_MontanteLordo = 0
            Me.m_NettoRicavo = 0
            Me.m_SommaEstinzioni = 0
            Me.m_SommaTrattenuteVolontarie = 0
            Me.m_SommaPignoramenti = 0
            Me.m_StipendioNetto = 0
            Me.m_TFR = 0
            Me.m_ValutazioneGARF = 1
            Me.m_TipoImpiego = ""
            Me.m_Eta = 0
            Me.m_Anzianita = 0
            Me.m_IDAzienda = 0
            Me.m_Azienda = Nothing
            Me.m_NomeAzienda = ""
            Me.m_DataAssunzione = Nothing
            Me.m_PropostaDa = Nothing
            Me.m_IDPropostaDa = 0
            Me.m_ConfermataDa = Nothing
            Me.m_IDConfermataDa = 0
            Me.m_IDContesto = 0
            Me.m_TipoContesto = vbNullString
            Me.m_Durata = 0
            Me.m_InseritoDa = Nothing
            Me.m_IDInseritoDa = 0
            Me.m_IDRichiestaApprovazione = 0
            Me.m_RichiestaApprovazione = Nothing
            Me.m_MotivoAnnullamento = ""
            Me.m_DettaglioAnnullamento = ""
            Me.m_Estinzioni = Nothing
            Me.m_IDAnnullataDa = 0
            Me.m_AnnullataDa = Nothing
            Me.m_NomeAnnullataDa = ""
            Me.m_DataAnnullamento = Nothing
            Me.m_IDFinestraLavorazione = 0
            Me.m_FinestraLavorazione = Nothing
            Me.m_IDUltimaVerifica = 0
            Me.m_UltimaVerifica = Nothing
        End Sub

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
        ''' Restituisce o imposta l'ID della finestra di lavorazioen a cui appartiene l'oggetto
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

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'utente che ha annullato l'offerta (bocciata)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDAnnullataDa As Integer
            Get
                Return GetID(Me.m_AnnullataDa, Me.m_IDAnnullataDa)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDAnnullataDa
                If (oldValue = value) Then Exit Property
                Me.m_AnnullataDa = Nothing
                Me.m_IDAnnullataDa = value
                Me.DoChanged("IDAnnullataDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'utente che ha annullato l'offerta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AnnullataDa As CUser
            Get
                If (Me.m_AnnullataDa Is Nothing) Then Me.m_AnnullataDa = Sistema.Users.GetItemById(Me.m_IDAnnullataDa)
                Return Me.m_AnnullataDa
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.AnnullataDa
                If (oldValue Is value) Then Exit Property
                Me.m_AnnullataDa = value
                Me.m_IDAnnullataDa = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeAnnullataDa = value.Nominativo
                Me.DoChanged("AnnullataDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome dell'utente che ha annullato lo studio di fattibilita
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeAnnullataDa As String
            Get
                Return Me.m_NomeAnnullataDa
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeAnnullataDa
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_NomeAnnullataDa = value
                Me.DoChanged("NomeAnnullataDa", value, oldValue)
            End Set
        End Property

        Public Property DataAnnullamento As Date?
            Get
                Return Me.m_DataAnnullamento
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataAnnullamento
                If Calendar.Compare(value, oldValue) = 0 Then Exit Property
                Me.m_DataAnnullamento = value
                Me.DoChanged("DataAnnullamento", value, oldValue)
            End Set
        End Property

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

        ''' <summary>
        ''' Nel caso di bocciatura o di rifiuto da parte del cliente indica il motivo  
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MotivoAnnullamento As String
            Get
                Return Me.m_MotivoAnnullamento
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_MotivoAnnullamento
                If (oldValue = value) Then Exit Property
                Me.m_MotivoAnnullamento = value
                Me.DoChanged("MotivoAnnullamento", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Nel caso di bocciatura o di rifiuto da parte del cliente descrive nel dettaglio il motivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DettaglioAnnullamento As String
            Get
                Return Me.m_DettaglioAnnullamento
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_DettaglioAnnullamento
                If (oldValue = value) Then Exit Property
                Me.m_DettaglioAnnullamento = value
                Me.DoChanged("DettaglioAnnullamento", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o i mposta l'ID dell'utente che ha registrato la consulenza
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDInseritoDa As Integer
            Get
                Return GetID(Me.m_InseritoDa, Me.m_IDInseritoDa)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDInseritoDa
                If (oldValue = value) Then Exit Property
                Me.m_InseritoDa = Nothing
                Me.m_IDInseritoDa = value
                Me.DoChanged("IDInseritoDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'utente che ha registrato la consulenza
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property InseritoDa As CUser
            Get
                If (Me.m_InseritoDa Is Nothing) Then Me.m_InseritoDa = Sistema.Users.GetItemById(Me.m_IDInseritoDa)
                Return Me.m_InseritoDa
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.m_InseritoDa
                If (oldValue Is value) Then Exit Property
                Me.m_InseritoDa = value
                Me.m_IDInseritoDa = GetID(value)
                Me.DoChanged("InseritoDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID del gruppo di studi di fattibilità a cui appartiene l'offerta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDStudioDiFattibilita As Integer
            Get
                Return GetID(Me.m_StudioDiFattibilita, Me.m_IDStudioDiFattibilita)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDStudioDiFattibilita
                If (oldValue = value) Then Exit Property
                Me.m_IDStudioDiFattibilita = value
                Me.m_StudioDiFattibilita = Nothing
                Me.DoChanged("IDStudioDiFattibilita", value, oldValue)
            End Set
        End Property

        Public Property StudioDiFattibilita As CQSPDStudioDiFattibilita
            Get
                If (Me.m_StudioDiFattibilita Is Nothing) Then Me.m_StudioDiFattibilita = CQSPD.StudiDiFattibilita.GetItemById(Me.m_IDStudioDiFattibilita)
                Return Me.m_StudioDiFattibilita
            End Get
            Set(value As CQSPDStudioDiFattibilita)
                Dim oldValue As CQSPDStudioDiFattibilita = Me.m_StudioDiFattibilita
                If (oldValue Is value) Then Exit Property
                Me.m_StudioDiFattibilita = value
                Me.m_IDStudioDiFattibilita = GetID(value)
                Me.DoChanged("StudioDiFattibilita", value, oldValue)
            End Set
        End Property

        Friend Sub SetStudioDiFattibilita(ByVal value As CQSPDStudioDiFattibilita)
            Me.m_StudioDiFattibilita = value
            Me.m_IDStudioDiFattibilita = GetID(value)
        End Sub



        ''' <summary>
        ''' Restituisce o imposta la durata in secondi della consulenza
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Durata As Double
            Get
                Return Me.m_Durata
            End Get
            Set(value As Double)
                Dim oldValue As Double = Me.m_Durata
                If (Arrays.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_Durata = value
                Me.DoChanged("Durata", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID del contesto in cui è stata creata la consulenza
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
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

        ''' <summary>
        ''' Restituisce o imposta il tipo del contesto in cui è stato creato l'oggetto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
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
        ''' Restituisce o imposta la data di assunzione del cliente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataAssunzione As Date?
            Get
                Return Me.m_DataAssunzione
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataAssunzione
                If Calendar.Compare(oldValue, value) = 0 Then Exit Property
                Me.m_DataAssunzione = value
                Me.DoChanged("DataAssunzione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'azienda per cui lavora il cliente
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
        ''' Restituisce o imposta l'azienda per cui lavora il cliente
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
                Me.m_IDAzienda = GetID(value)
                Me.m_Azienda = value
                If (value IsNot Nothing) Then Me.m_NomeAzienda = value.Nominativo
                Me.DoChanged("Azienda", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome dell'azienda per cui lavora il cliente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeAzienda As String
            Get
                Return Me.m_NomeAzienda
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeAzienda
                If (oldValue = value) Then Exit Property
                Me.m_NomeAzienda = value
                Me.DoChanged("NomeAzienda", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il montante lordo proposto al cliente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MontanteLordo As Decimal
            Get
                Return Me.m_MontanteLordo
            End Get
            Set(value As Decimal)
                Dim oldValue As Decimal = Me.m_MontanteLordo
                If (oldValue = value) Then Exit Property
                Me.m_MontanteLordo = value
                Me.DoChanged("MontanteLordo", value, oldValue)
            End Set
        End Property

        Public Property DataDecorrenza As Date? Implements IEstintore.DataDecorrenza
            Get
                Dim d1, d2 As Date?
                If (Me.OffertaCQS IsNot Nothing) Then d1 = Me.OffertaCQS.DataDecorrenza
                If (Me.OffertaPD IsNot Nothing) Then d2 = Me.OffertaPD.DataDecorrenza
                Return Calendar.Min(d1, d2)
            End Get
            Set(value As Date?)
                Throw New InvalidOperationException
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il netto ricavo proposto al cliente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NettoRicavo As Decimal
            Get
                Return Me.m_NettoRicavo
            End Get
            Set(value As Decimal)
                Dim oldValue As Decimal = Me.m_NettoRicavo
                If (oldValue = value) Then Exit Property
                Me.m_NettoRicavo = value
                Me.DoChanged("NettoRicavo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la somma degli altri prestiti da estinguere
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SommaEstinzioni As Decimal
            Get
                Return Me.m_SommaEstinzioni
            End Get
            Set(value As Decimal)
                Dim oldValue As Decimal = Me.m_SommaEstinzioni
                If (oldValue = value) Then Exit Property
                Me.m_SommaEstinzioni = value
                Me.DoChanged("SommaEstinzioni", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la somma delle trattenute volontarie
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SommaTrattenuteVolontarie As Decimal
            Get
                Return Me.m_SommaTrattenuteVolontarie
            End Get
            Set(value As Decimal)
                Dim oldValue As Decimal = Me.m_SommaTrattenuteVolontarie
                If (oldValue = value) Then Exit Property
                Me.m_SommaTrattenuteVolontarie = value
                Me.DoChanged("SommaTrattenuteVolontarie", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la somma dei pignoramenti
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SommaPignoramenti As Decimal
            Get
                Return Me.m_SommaPignoramenti
            End Get
            Set(value As Decimal)
                Dim oldValue As Decimal = Me.m_SommaPignoramenti
                If (oldValue = value) Then Exit Property
                Me.m_SommaPignoramenti = value
                Me.DoChanged("SommaPignoramenti", value, oldValue)
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
        ''' Restituisce o imposta il TFR del cliente
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

        Public Property ValutazioneGARF As Integer
            Get
                Return Me.m_ValutazioneGARF
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_ValutazioneGARF
                If (oldValue = value) Then Exit Property
                Me.m_ValutazioneGARF = value
                Me.DoChanged("ValutazioneGARF", value, oldValue)
            End Set
        End Property

        Public Property TipoImpiego As String
            Get
                Return Me.m_TipoImpiego
            End Get
            Set(value As String)
                value = Strings.Left(Strings.Trim(value), 1)
                Dim oldValue As String = Me.m_TipoImpiego
                If (oldValue = value) Then Exit Property
                Me.m_TipoImpiego = value
                Me.DoChanged("TipoImpiego", value, oldValue)
            End Set
        End Property

        Public Property Eta As Single
            Get
                Return Me.m_Eta
            End Get
            Set(value As Single)
                Dim oldValue As Single = Me.m_Eta
                If (oldValue = value) Then Exit Property
                Me.m_Eta = value
                Me.DoChanged("Eta", value, oldValue)
            End Set
        End Property

        Public Property Anzianita As Single
            Get
                Return Me.m_Anzianita
            End Get
            Set(value As Single)
                Dim oldValue As Single = Me.m_Anzianita
                If (oldValue = value) Then Exit Property
                Me.m_Anzianita = value
                Me.DoChanged("Anzianita", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'offerta fatta per la cessione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property OffertaCQS As COffertaCQS
            Get
                If (Me.m_OffertaCQS Is Nothing) Then Me.m_OffertaCQS = DMD.CQSPD.Offerte.GetItemById(Me.m_IDOffertaCQS)
                Return Me.m_OffertaCQS
            End Get
            Set(value As COffertaCQS)
                Dim oldValue As COffertaCQS = Me.m_OffertaCQS
                If (oldValue Is value) Then Exit Property
                Me.SetOffertaCQS(value)
                Me.DoChanged("OffertaCQS", value, oldValue)
            End Set
        End Property

        Protected Friend Overridable Sub SetOffertaCQS(ByVal value As COffertaCQS)
            Me.m_OffertaCQS = value
            Me.m_IDOffertaCQS = GetID(value)
            'If (value IsNot Nothing) Then
            '    Me.m_IDProdottoCQS = value.ProdottoID
            '    Me.m_NomeProdottoCQS = value.NomeProdotto
            'Else
            '    Me.m_IDProdottoCQS = 0
            '    Me.m_NomeProdottoCQS = ""
            'End If
        End Sub

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'offerta CQS
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDOffertaCQS As Integer
            Get
                Return GetID(Me.m_OffertaCQS, Me.m_IDOffertaCQS)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDOffertaCQS
                If (oldValue = value) Then Exit Property
                Me.m_IDOffertaCQS = value
                Me.m_OffertaCQS = Nothing
                Me.DoChanged("IDOffertaCQS", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'offerta fatta per la delega
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property OffertaPD As COffertaCQS
            Get
                If (Me.m_OffertaPD Is Nothing) Then Me.m_OffertaPD = DMD.CQSPD.Offerte.GetItemById(Me.m_IDOffertaPD)
                Return Me.m_OffertaPD
            End Get
            Set(value As COffertaCQS)
                Dim oldValue As COffertaCQS = Me.m_OffertaPD
                If (oldValue Is value) Then Exit Property
                Me.SetOffertaPD(value)
                Me.DoChanged("OffertaPD", value, oldValue)
            End Set
        End Property

        Protected Friend Overridable Sub SetOffertaPD(ByVal value As COffertaCQS)
            Me.m_OffertaPD = value
            Me.m_IDOffertaPD = GetID(value)
            'If (value IsNot Nothing) Then
            '    Me.m_IDProdottoPD = value.ProdottoID
            '    Me.m_NomeProdottoPD = value.NomeProdotto
            'Else
            '    Me.m_IDProdottoPD = 0
            '    Me.m_NomeProdottoPD = ""
            'End If
        End Sub

        Public Property IDOffertaPD As Integer
            Get
                Return GetID(Me.m_OffertaPD, Me.m_IDOffertaPD)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDOffertaPD
                If (oldValue = value) Then Exit Property
                Me.m_IDOffertaPD = value
                Me.m_OffertaPD = Nothing
                Me.DoChanged("IDOffertaPD", value, oldValue)
            End Set
        End Property

        Public Property StatoConsulenza As StatiConsulenza
            Get
                Return Me.m_StatoConsulenza
            End Get
            Friend Set(value As StatiConsulenza)
                Dim oldValue As StatiConsulenza = Me.m_StatoConsulenza
                If (oldValue = value) Then Exit Property
                Me.m_StatoConsulenza = value
                Me.DoChanged("StatoConsulenza", value, oldValue)
            End Set
        End Property

        Public Property Visible As Boolean
            Get
                Return TestFlag(Me.m_Flags, ConsulenzeFlags.HIDDEN) = False
            End Get
            Set(value As Boolean)
                If (Me.Visible = value) Then Exit Property
                Me.m_Flags = SetFlag(Me.m_Flags, ConsulenzeFlags.HIDDEN, value)
                Me.DoChanged("Visible", value, Not value)
            End Set
        End Property

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

        Public Property Cliente As CPersonaFisica Implements IEstintore.Cliente
            Get
                If (Me.m_Cliente Is Nothing) Then Me.m_Cliente = Anagrafica.Persone.GetItemById(Me.m_IDCliente)
                Return Me.m_Cliente
            End Get
            Set(value As CPersonaFisica)
                Dim oldValue As CPersonaFisica = Me.m_Cliente
                If (oldValue Is value) Then Exit Property
                Me.m_Cliente = value
                Me.m_IDCliente = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeCliente = value.Nominativo
                Me.DoChanged("Cliente", value, oldValue)
            End Set
        End Property

        Protected Friend Overridable Sub SetCliente(ByVal value As CPersona)
            Me.m_Cliente = value
            Me.m_IDCliente = GetID(value)
        End Sub

        Public Property NomeCliente As String
            Get
                Return Me.m_NomeCliente
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeCliente
                If (oldValue = value) Then Exit Property
                Me.m_NomeCliente = value
                Me.DoChanged("NomeCliente", value, oldValue)
            End Set
        End Property

        Public Property IDRichiesta As Integer
            Get
                Return GetID(Me.m_Richiesta, Me.m_IDRichiesta)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDRichiesta
                If (oldValue = value) Then Exit Property
                Me.m_IDRichiesta = value
                Me.m_Richiesta = Nothing
                Me.DoChanged("IDRichiesta", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la richiesta di finanziamento da cui è partita la consulenza
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Richiesta As CRichiestaFinanziamento
            Get
                If (Me.m_Richiesta Is Nothing) Then Me.m_Richiesta = DMD.CQSPD.RichiesteFinanziamento.GetItemById(Me.m_IDRichiesta)
                Return Me.m_Richiesta
            End Get
            Set(value As CRichiestaFinanziamento)
                Dim oldValue As CRichiestaFinanziamento = Me.m_Richiesta
                If (oldValue Is value) Then Exit Property
                Me.m_Richiesta = value
                Me.m_IDRichiesta = GetID(value)
                Me.DoChanged("Richiesta", value, oldValue)
            End Set
        End Property

        Protected Friend Overridable Sub SetRichiesta(ByVal value As CRichiestaFinanziamento)
            Me.m_Richiesta = value
            Me.m_IDRichiesta = GetID(value)
        End Sub

        'Me.m_OfferteProposte = Nothing

        ' Me.m_IDOffertaCorrente = 0
        'Me.m_OffertaCorrente = Nothing
        Public Property IDConsulente As Integer
            Get
                Return GetID(Me.m_Consulente, Me.m_IDConsulente)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDConsulente
                If (oldValue = value) Then Exit Property
                Me.m_IDConsulente = value
                Me.m_Consulente = Nothing
                Me.DoChanged("IDConsulente", value, oldValue)
            End Set
        End Property

        Public Property Consulente As CConsulentePratica
            Get
                If (Me.m_Consulente Is Nothing) Then Me.m_Consulente = DMD.CQSPD.Consulenti.GetItemById(Me.m_IDConsulente)
                Return Me.m_Consulente
            End Get
            Set(value As CConsulentePratica)
                Dim oldValue As CConsulentePratica = Me.m_Consulente
                If (oldValue Is value) Then Exit Property
                Me.m_Consulente = value
                Me.m_IDConsulente = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeConsulente = value.Nome
                Me.DoChanged("Consulente", value, oldValue)
            End Set
        End Property

        Public Property NomeConsulente As String
            Get
                Return Me.m_NomeConsulente
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeConsulente
                If (oldValue = value) Then Exit Property
                Me.m_NomeConsulente = value
                Me.DoChanged("NomeConsulente", value, oldValue)
            End Set
        End Property

        Public Property DataConsulenza As Nullable(Of Date)
            Get
                Return Me.m_DataConsulenza
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataConsulenza
                If (oldValue = value) Then Exit Property
                Me.m_DataConsulenza = value
                Me.DoChanged("DataConsulenza", value, oldValue)
            End Set
        End Property

        Public Property DataConferma As Nullable(Of Date)
            Get
                Return Me.m_DataConferma
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataConferma
                If (oldValue = value) Then Exit Property
                Me.m_DataConferma = value
                Me.DoChanged("DataConferma", value, oldValue)
            End Set
        End Property

        Public Property Descrizione As String
            Get
                Return Me.m_Descrizione
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_Descrizione
                If (oldValue = value) Then Exit Property
                Me.m_Descrizione = value
                Me.DoChanged("Descrizione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'utente che ha effettuato la proposta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PropostaDa As CUser
            Get
                If (Me.m_PropostaDa Is Nothing) Then Me.m_PropostaDa = Users.GetItemById(Me.m_IDPropostaDa)
                Return Me.m_PropostaDa
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.m_PropostaDa
                If (oldValue Is value) Then Exit Property
                Me.m_PropostaDa = value
                Me.m_IDPropostaDa = GetID(value)
                Me.DoChanged("PropostaDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'utente che ha proposto la consulenza
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDPropostaDa As Integer
            Get
                Return GetID(Me.m_PropostaDa, Me.m_IDPropostaDa)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDPropostaDa
                If (oldValue = value) Then Exit Property
                Me.m_IDPropostaDa = value
                Me.m_PropostaDa = Nothing
                Me.DoChanged("IDPropostaDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data in cui è stata proposta la consulenza
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataProposta As Nullable(Of Date)
            Get
                Return Me.m_DataProposta
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataProposta
                If (Calendar.Compare(oldValue, value) = 0) Then Exit Property
                Me.m_DataProposta = value
                Me.DoChanged("DataProposta", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'utente che ha registrato l'accettazione o il rifiuto della proposta da parte del cliente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ConfermataDa As CUser
            Get
                If (Me.m_ConfermataDa Is Nothing) Then Me.m_ConfermataDa = Users.GetItemById(Me.m_IDConfermataDa)
                Return Me.m_ConfermataDa
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.m_ConfermataDa
                If (oldValue Is value) Then Exit Property
                Me.m_ConfermataDa = value
                Me.m_IDConfermataDa = GetID(value)
                Me.DoChanged("ConfermataDa", value, oldValue)
            End Set
        End Property

        Public Property IDConfermataDa As Integer
            Get
                Return GetID(Me.m_ConfermataDa, Me.m_IDConfermataDa)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDConfermataDa
                If (oldValue = value) Then Exit Property
                Me.m_IDConfermataDa = value
                Me.m_ConfermataDa = Nothing
                Me.DoChanged("IDConfermataDa", value, oldValue)
            End Set
        End Property


        Public Overrides Function GetTableName() As String
            Return "tbl_CQSPDConsulenze"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_IDCliente = reader.Read("IDCliente", Me.m_IDCliente)
            Me.m_NomeCliente = reader.Read("NomeCliente", Me.m_NomeCliente)
            Me.m_IDRichiesta = reader.Read("IDRichiesta", Me.m_IDRichiesta)
            'reader.Read("Me.m_IDOffertaCorrente = 0
            'Me.m_OffertaCorrente = Nothing
            Me.m_IDConsulente = reader.Read("IDConsulente", Me.m_IDConsulente)
            Me.m_NomeConsulente = reader.Read("NomeConsulente", Me.m_NomeConsulente)
            Me.m_DataConsulenza = reader.Read("DataConsulenza", Me.m_DataConsulenza)
            Me.m_DataProposta = reader.Read("DataProposta", Me.m_DataConferma)
            Me.m_DataConferma = reader.Read("DataConferma", Me.m_DataConferma)
            Me.m_Descrizione = reader.Read("Descrizione", Me.m_Descrizione)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_StatoConsulenza = reader.Read("StatoConsulenza", Me.m_StatoConsulenza)
            Me.m_IDOffertaCQS = reader.Read("IDOffertaCQS", Me.m_IDOffertaCQS)
            Me.m_IDOffertaPD = reader.Read("IDOffertaPD", Me.m_IDOffertaPD)
            Me.m_MontanteLordo = reader.Read("MontanteLordo", Me.m_MontanteLordo)
            Me.m_NettoRicavo = reader.Read("NettoRicavo", Me.m_NettoRicavo)
            Me.m_SommaEstinzioni = reader.Read("SommaEstinzioni", Me.m_SommaEstinzioni)
            Me.m_SommaTrattenuteVolontarie = reader.Read("SommaTrattenuteVolontarie", Me.m_SommaTrattenuteVolontarie)
            Me.m_SommaPignoramenti = reader.Read("SommaPignoramenti", Me.m_SommaPignoramenti)
            Me.m_StipendioNetto = reader.Read("StipendioNetto", Me.m_StipendioNetto)
            Me.m_TFR = reader.Read("TFR", Me.m_TFR)
            Me.m_ValutazioneGARF = reader.Read("ValutazioneGARF", Me.m_ValutazioneGARF)
            Me.m_TipoImpiego = reader.Read("TipoImpiego", Me.m_TipoImpiego)
            Me.m_Eta = reader.Read("Eta", Me.m_Eta)
            Me.m_Anzianita = reader.Read("Anzianita", Me.m_Anzianita)
            Me.m_IDAzienda = reader.Read("IDAzienda", Me.m_IDAzienda)
            Me.m_NomeAzienda = reader.Read("NomeAzienda", Me.m_NomeAzienda)
            Me.m_DataAssunzione = reader.Read("DataAssunzione", Me.m_DataAssunzione)
            Me.m_IDPropostaDa = reader.Read("IDPropostaDa", Me.m_IDPropostaDa)
            Me.m_IDConfermataDa = reader.Read("IDConfermataDa", Me.m_IDConfermataDa)
            Me.m_IDContesto = reader.Read("IDContesto", Me.m_IDContesto)
            Me.m_TipoContesto = reader.Read("TipoContesto", Me.m_TipoContesto)
            Me.m_Durata = reader.Read("Durata", Me.m_Durata)
            Me.m_IDInseritoDa = reader.Read("IDInseritoDa", Me.m_IDInseritoDa)
            Me.m_IDStudioDiFattibilita = reader.Read("IDGruppo", Me.m_IDStudioDiFattibilita)
            Me.m_IDRichiestaApprovazione = reader.Read("IDRichiestaApprovazione", Me.m_IDRichiestaApprovazione)
            Me.m_MotivoAnnullamento = reader.Read("MotivoAnnullamento", Me.m_MotivoAnnullamento)
            Me.m_DettaglioAnnullamento = reader.Read("DettaglioAnnullamento", Me.m_DettaglioAnnullamento)
            Me.m_IDAnnullataDa = reader.Read("IDAnnullataDa", Me.m_IDAnnullataDa)
            Me.m_NomeAnnullataDa = reader.Read("NomeAnnullataDa", Me.m_NomeAnnullataDa)
            Me.m_DataAnnullamento = reader.Read("DataAnnullamento", Me.m_DataAnnullamento)
            Me.m_IDFinestraLavorazione = reader.Read("IDFinestraLavorazione", Me.m_IDFinestraLavorazione)
            Me.m_IDUltimaVerifica = reader.Read("IDUltimaVerifica", Me.m_IDUltimaVerifica)
            'Me.m_IDProdottoCQS = reader.Read("IDProdottoCQS", Me.m_IDProdottoCQS)
            'Me.m_NomeProdottoCQS = reader.Read("NomeProdottoCQS", Me.m_NomeProdottoCQS)
            'Me.m_IDProdottoPD = reader.Read("IDProdottoPD", Me.m_IDProdottoPD)
            'Me.m_NomeProdottoPD = reader.Read("NomeProdottoPD", Me.m_NomeProdottoPD)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("IDCliente", Me.IDCliente)
            writer.Write("NomeCliente", Me.m_NomeCliente)
            writer.Write("IDRichiesta", Me.IDRichiesta)
            'reader.Read("Me.m_IDOffertaCorrente = 0
            'Me.m_OffertaCorrente = Nothing
            writer.Write("IDConsulente", Me.IDConsulente)
            writer.Write("NomeConsulente", Me.m_NomeConsulente)
            writer.Write("DataConsulenza", Me.m_DataConsulenza)
            writer.Write("DataConferma", Me.m_DataConferma)
            writer.Write("Descrizione", Me.m_Descrizione)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("StatoConsulenza", Me.m_StatoConsulenza)
            writer.Write("IDOffertaCQS", Me.IDOffertaCQS)
            writer.Write("IDOffertaPD", Me.IDOffertaPD)
            writer.Write("MontanteLordo", Me.m_MontanteLordo)
            writer.Write("NettoRicavo", Me.m_NettoRicavo)
            writer.Write("SommaEstinzioni", Me.m_SommaEstinzioni)
            writer.Write("SommaTrattenuteVolontarie", Me.m_SommaTrattenuteVolontarie)
            writer.Write("SommaPignoramenti", Me.m_SommaPignoramenti)
            writer.Write("StipendioNetto", Me.m_StipendioNetto)
            writer.Write("TFR", Me.m_TFR)
            writer.Write("ValutazioneGARF", Me.m_ValutazioneGARF)
            writer.Write("TipoImpiego", Me.m_TipoImpiego)
            writer.Write("Eta", Me.m_Eta)
            writer.Write("Anzianita", Me.m_Anzianita)
            writer.Write("IDAzienda", Me.IDAzienda)
            writer.Write("NomeAzienda", Me.m_NomeAzienda)
            writer.Write("DataAssunzione", Me.m_DataAssunzione)
            writer.Write("DataProposta", Me.m_DataProposta)
            writer.Write("IDPropostaDa", Me.IDPropostaDa)
            writer.Write("IDConfermataDa", Me.IDConfermataDa)
            writer.Write("IDContesto", Me.m_IDContesto)
            writer.Write("TipoContesto", Me.m_TipoContesto)
            writer.Write("Durata", Me.m_Durata)
            writer.Write("IDInseritoDa", Me.IDInseritoDa)
            writer.Write("IDGruppo", Me.IDStudioDiFattibilita)
            writer.Write("IDRichiestaApprovazione", Me.IDRichiestaApprovazione)
            writer.Write("MotivoAnnullamento", Me.m_MotivoAnnullamento)
            writer.Write("DettaglioAnnullamento", Me.m_DettaglioAnnullamento)
            writer.Write("IDAnnullataDa", Me.IDAnnullataDa)
            writer.Write("NomeAnnullataDa", Me.m_NomeAnnullataDa)
            writer.Write("DataAnnullamento", Me.m_DataAnnullamento)
            writer.Write("IDFinestraLavorazione", Me.IDFinestraLavorazione)
            writer.Write("IDUltimaVerifica", Me.IDUltimaVerifica)
            'writer.Write("IDProdottoCQS", Me.IDProdottoCQS)
            'writer.Write("NomeProdottoCQS", Me.m_NomeProdottoCQS)
            'writer.Write("IDProdottoPD", Me.IDProdottoPD)
            'writer.Write("NomeProdottoPD", Me.m_NomeProdottoPD)
            Return MyBase.SaveToRecordset(writer)
        End Function


        '------------------------------------
        Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
            writer.WriteAttribute("IDCliente", Me.IDCliente)
            writer.WriteAttribute("NomeCliente", Me.m_NomeCliente)
            writer.WriteAttribute("IDRichiesta", Me.IDRichiesta)
            'reader.Read("Me.m_IDOffertaCorrente = 0
            'Me.m_OffertaCorrente = Nothing
            writer.WriteAttribute("IDConsulente", Me.IDConsulente)
            writer.WriteAttribute("NomeConsulente", Me.m_NomeConsulente)
            writer.WriteAttribute("DataConsulenza", Me.m_DataConsulenza)
            writer.WriteAttribute("DataConferma", Me.m_DataConferma)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("StatoConsulenza", Me.m_StatoConsulenza)
            writer.WriteAttribute("IDOffertaCQS", Me.IDOffertaCQS)
            writer.WriteAttribute("IDOffertaPD", Me.IDOffertaPD)
            writer.WriteAttribute("MontanteLordo", Me.m_MontanteLordo)
            writer.WriteAttribute("NettoRicavo", Me.m_NettoRicavo)
            writer.WriteAttribute("SommaEstinzioni", Me.m_SommaEstinzioni)
            writer.WriteAttribute("SommaTrattenuteVolontarie", Me.m_SommaTrattenuteVolontarie)
            writer.WriteAttribute("SommaPignoramenti", Me.m_SommaPignoramenti)
            writer.WriteAttribute("StipendioNetto", Me.m_StipendioNetto)
            writer.WriteAttribute("TFR", Me.m_TFR)
            writer.WriteAttribute("ValutazioneGARF", Me.m_ValutazioneGARF)
            writer.WriteAttribute("TipoImpiego", Me.m_TipoImpiego)
            writer.WriteAttribute("Eta", Me.m_Eta)
            writer.WriteAttribute("Anzianita", Me.m_Anzianita)
            writer.WriteAttribute("IDAzienda", Me.IDAzienda)
            writer.WriteAttribute("NomeAzienda", Me.m_NomeAzienda)
            writer.WriteAttribute("DataAssunzione", Me.m_DataAssunzione)
            writer.WriteAttribute("DataProposta", Me.m_DataProposta)
            writer.WriteAttribute("IDPropostaDa", Me.IDPropostaDa)
            writer.WriteAttribute("IDConfermataDa", Me.IDConfermataDa)
            writer.WriteAttribute("IDContesto", Me.m_IDContesto)
            writer.WriteAttribute("TipoContesto", Me.m_TipoContesto)
            writer.WriteAttribute("Durata", Me.m_Durata)
            writer.WriteAttribute("IDInseritoDa", Me.IDInseritoDa)
            writer.WriteAttribute("IDStudioDiFattibilita", Me.IDStudioDiFattibilita)
            writer.WriteAttribute("IDRichiestaApprovazione", Me.IDRichiestaApprovazione)
            writer.WriteAttribute("MotivoAnnullamento", Me.m_MotivoAnnullamento)
            writer.WriteAttribute("DettaglioAnnullamento", Me.m_DettaglioAnnullamento)
            writer.WriteAttribute("IDAnnullataDa", Me.IDAnnullataDa)
            writer.WriteAttribute("NomeAnnullataDa", Me.m_NomeAnnullataDa)
            writer.WriteAttribute("DataAnnullamento", Me.m_DataAnnullamento)
            writer.WriteAttribute("IDFinestraLavorazione", Me.IDFinestraLavorazione)
            writer.WriteAttribute("IDUltimaVerifica", Me.IDUltimaVerifica)
            'writer.WriteAttribute("IDProdottoCQS", Me.IDProdottoCQS)
            'writer.WriteAttribute("NomeProdottoCQS", Me.m_NomeProdottoCQS)
            'writer.WriteAttribute("IDProdottoPD", Me.IDProdottoPD)
            'writer.WriteAttribute("NomeProdottoPD", Me.m_NomeProdottoPD)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Descrizione", Me.m_Descrizione)
            'If (writer.Settings.GetValueBool("lightserialization") = False) Then
            writer.WriteTag("OffertaCQS", Me.OffertaCQS)
            writer.WriteTag("OffertaPD", Me.OffertaPD)
            writer.WriteTag("RichiestaApprovazione", Me.RichiestaApprovazione)
            'End If

        End Sub

        Protected Overrides Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object)
            Select Case fieldName
                Case "IDCliente" : Me.m_IDCliente = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeCliente" : Me.m_NomeCliente = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDRichiesta" : Me.m_IDRichiesta = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                    'reader.Read("Me.m_IDOffertaCorrente = 0
                    'Me.m_OffertaCorrente = Nothing
                Case "IDConsulente" : Me.m_IDConsulente = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeConsulente" : Me.m_NomeConsulente = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataConsulenza" : Me.m_DataConsulenza = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataConferma" : Me.m_DataConferma = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Descrizione" : Me.m_Descrizione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "StatoConsulenza" : Me.m_StatoConsulenza = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDOffertaCQS" : Me.m_IDOffertaCQS = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDOffertaPD" : Me.m_IDOffertaPD = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "MontanteLordo" : Me.m_MontanteLordo = XML.Utils.Serializer.DeserializeFloat(fieldValue)
                Case "NettoRicavo" : Me.m_NettoRicavo = XML.Utils.Serializer.DeserializeFloat(fieldValue)
                Case "SommaEstinzioni" : Me.m_SommaEstinzioni = XML.Utils.Serializer.DeserializeFloat(fieldValue)
                Case "SommaTrattenuteVolontarie" : Me.m_SommaTrattenuteVolontarie = XML.Utils.Serializer.DeserializeFloat(fieldValue)
                Case "SommaPignoramenti" : Me.m_SommaPignoramenti = XML.Utils.Serializer.DeserializeFloat(fieldValue)
                Case "StipendioNetto" : Me.m_StipendioNetto = XML.Utils.Serializer.DeserializeFloat(fieldValue)
                Case "TFR" : Me.m_TFR = XML.Utils.Serializer.DeserializeFloat(fieldValue)
                Case "ValutazioneGARF" : Me.m_ValutazioneGARF = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "TipoImpiego" : Me.m_TipoImpiego = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Eta" : Me.m_Eta = XML.Utils.Serializer.DeserializeFloat(fieldValue)
                Case "Anzianita" : Me.m_Anzianita = XML.Utils.Serializer.DeserializeFloat(fieldValue)
                Case "IDAzienda" : Me.m_IDAzienda = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeAzienda" : Me.m_NomeAzienda = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataAssunzione" : Me.m_DataAssunzione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataProposta" : Me.m_DataProposta = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IDPropostaDa" : Me.m_IDPropostaDa = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDConfermataDa" : Me.m_IDConfermataDa = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDContesto" : Me.m_IDContesto = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "TipoContesto" : Me.m_TipoContesto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Durata" : Me.m_Durata = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "IDInseritoDa" : Me.m_IDInseritoDa = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDStudioDiFattibilita" : Me.m_IDStudioDiFattibilita = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDRichiestaApprovazione" : Me.m_IDRichiestaApprovazione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "OffertaCQS" : Me.m_OffertaCQS = XML.Utils.Serializer.ToObject(fieldValue)
                Case "OffertaPD" : Me.m_OffertaPD = XML.Utils.Serializer.ToObject(fieldValue)
                Case "RichiestaApprovazione" : Me.m_RichiestaApprovazione = XML.Utils.Serializer.ToObject(fieldValue)
                Case "MotivoAnnullamento" : Me.m_MotivoAnnullamento = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DettaglioAnnullamento" : Me.m_DettaglioAnnullamento = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDAnnullataDa" : Me.m_IDAnnullataDa = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeAnnullataDa" : Me.m_NomeAnnullataDa = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataAnnullamento" : Me.m_DataAnnullamento = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IDFinestraLavorazione" : Me.m_IDFinestraLavorazione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDUltimaVerifica" : Me.m_IDUltimaVerifica = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                    'Case "IDProdottoCQS" : Me.m_IDProdottoCQS = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                    'Case "NomeProdottoCQS" : Me.m_NomeProdottoCQS = XML.Utils.Serializer.DeserializeString(fieldValue)
                    'Case "IDProdottoPD" : Me.m_IDProdottoPD = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                    'Case "NomeProdottoPD" : Me.m_NomeProdottoPD = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Overrides Function ToString() As String
            Return "Consulenza a " & Me.m_NomeCliente & " del " & Formats.FormatUserDateTime(Me.m_DataConsulenza)
        End Function


        Public Overrides Function GetModule() As CModule
            Return Consulenze.Module
        End Function

        Protected Overrides Sub OnCreate(e As SystemEvent)
            MyBase.OnCreate(e)
            Me.OnInserita(New ItemEventArgs(Of CQSPDConsulenza)(Me))
        End Sub

        Protected Overrides Sub OnDelete(e As SystemEvent)
            MyBase.OnDelete(e)
            CQSPD.Consulenze.doConsulenzaEliminata(New ItemEventArgs(Of CQSPDConsulenza)(Me))
        End Sub

        Protected Overrides Sub OnModified(e As SystemEvent)
            MyBase.OnModified(e)
            CQSPD.Consulenze.doConsulenzaModificata(New ItemEventArgs(Of CQSPDConsulenza)(Me))
        End Sub

        Protected Overridable Sub OnInserita(ByVal e As ItemEventArgs(Of CQSPDConsulenza))
            RaiseEvent Inserita(Me, e)
            CQSPD.Consulenze.Module.DispatchEvent(New EventDescription("consulenza_inserita", "L'operatore " & Users.CurrentUser.Nominativo & " ha inserito la consulenza N°" & GetID(Me) & " per il cliente " & Me.NomeCliente, Me))
            CQSPD.Consulenze.doConsulenzaInserita(e)
        End Sub

        ''' <summary>
        ''' Propone la consulenza al cliente
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Proponi()
            'If (Me.m_StatoConsulenza <> StatiConsulenza.INSERITA) Then Throw New InvalidOperationException("La consulenza deve essere in stato Inserita")

            Dim ra As CRichiestaApprovazione = Me.RichiestaApprovazione
            Dim mr As CMotivoScontoPratica = Nothing
            If (ra IsNot Nothing) Then mr = ra.MotivoRichiesta


            If (ra IsNot Nothing) Then

                If (ra.StatoRichiesta = StatoRichiestaApprovazione.NEGATA) Then
                    Throw New InvalidOperationException("Lo studio di fattibilità è stato bocciato da " & ra.NomeConfermataDa)
                ElseIf (ra.StatoRichiesta = StatoRichiestaApprovazione.APPROVATA) Then
                    'OK
                ElseIf (mr IsNot Nothing AndAlso Not mr.SoloSegnalazione AndAlso ra.StatoRichiesta <> StatoRichiestaApprovazione.APPROVATA) Then
                    If Not CQSPD.Configuration.ConsentiProposteSenzaSupervisore Then Throw New InvalidOperationException("Questa proposta deve essere prima approvata da un supervisore")
                        'Throw New InvalidOperationException("Lo studio di fattibilità è in attesa di essere valutato da un supervisore")
                End If
            End If

            Me.m_StatoConsulenza = StatiConsulenza.PROPOSTA
            Me.m_DataProposta = Calendar.Now()
            Me.m_PropostaDa = Users.CurrentUser
            Me.m_IDPropostaDa = GetID(Me.m_PropostaDa)
            Me.SetChanged(True)
            Me.Save()
            Dim e As New ItemEventArgs(Of CQSPDConsulenza)(Me)
            Me.OnProposta(e)
            CQSPD.Consulenze.doConsulenzaProposta(e)
            CQSPD.Consulenze.Module.DispatchEvent(New EventDescription("consulenza_proposta", "L'operatore " & Users.CurrentUser.Nominativo & " ha proposto la consulenza N°" & GetID(Me) & " al cliente " & Me.NomeCliente, Me))
        End Sub

        Protected Overridable Sub OnProposta(ByVal e As ItemEventArgs(Of CQSPDConsulenza))
            RaiseEvent Proposta(Me, e)
        End Sub



        ''' <summary>
        ''' Il cliente ha accettato la consulenza 
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Accetta()
            'If (Me.m_StatoConsulenza <> StatiConsulenza.PROPOSTA) Then Throw New InvalidOperationException("La consulenza deve essere in stato Proposta")
            If (Me.RichiestaApprovazione IsNot Nothing) Then
                If (Me.RichiestaApprovazione.StatoRichiesta = StatoRichiestaApprovazione.NEGATA) Then
                    Throw New InvalidOperationException("Lo studio di fattibilità è stato bocciato da " & Me.RichiestaApprovazione.NomeConfermataDa)
                ElseIf (Me.RichiestaApprovazione.StatoRichiesta = StatoRichiestaApprovazione.APPROVATA) Then
                    'OK
                Else
                    If Not CQSPD.Configuration.ConsentiProposteSenzaSupervisore Then Throw New InvalidOperationException("Lo studio di fattibilità è in attesa di essere valutato da un supervisore")
                End If
            End If
            Me.m_StatoConsulenza = StatiConsulenza.ACCETTATA
            Me.m_DataConferma = Calendar.Now()
            Me.m_ConfermataDa = Users.CurrentUser
            Me.m_IDConfermataDa = GetID(Me.m_ConfermataDa)
            Me.SetChanged(True)
            Me.Save()
            Dim e As New ItemEventArgs(Of CQSPDConsulenza)(Me)
            Me.OnAccettata(e)
            CQSPD.Consulenze.doConsulenzaAccettata(e)
            CQSPD.Consulenze.Module.DispatchEvent(New EventDescription("consulenza_accettata", "L'operatore " & Users.CurrentUser.Nominativo & " ha registrato la conferma della consulenza N°" & GetID(Me) & " per il cliente " & Me.NomeCliente, Me))
        End Sub

        Protected Overridable Sub OnAccettata(ByVal e As ItemEventArgs(Of CQSPDConsulenza))
            RaiseEvent Accettata(Me, e)
        End Sub

        ''' <summary>
        ''' Il cliente ha rifiutato la proposta
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Rifiuta(ByVal motivo As String, ByVal dettaglio As String)
            'If (Me.m_StatoConsulenza <> StatiConsulenza.PROPOSTA) Then Throw New InvalidOperationException("La consulenza deve essere in stato Proposta")
            If (Me.RichiestaApprovazione IsNot Nothing) Then
                If (Me.RichiestaApprovazione.StatoRichiesta = StatoRichiestaApprovazione.NEGATA) Then
                    Throw New InvalidOperationException("Lo studio di fattibilità è stato bocciato da " & Me.RichiestaApprovazione.NomeConfermataDa)
                ElseIf (Me.RichiestaApprovazione.StatoRichiesta = StatoRichiestaApprovazione.APPROVATA) Then
                    'OK
                Else
                    'Throw New InvalidOperationException("Lo studio di fattibilità è in attesa di essere valutato da un supervisore")
                    Me.RichiestaApprovazione.StatoRichiesta = StatoRichiestaApprovazione.ANNULLATA
                    Me.RichiestaApprovazione.MotivoConferma = motivo
                    Me.RichiestaApprovazione.DettaglioConferma = dettaglio
                    Me.RichiestaApprovazione.Save()
                End If
            End If
            Me.m_MotivoAnnullamento = Strings.Trim(motivo)
            Me.m_DettaglioAnnullamento = dettaglio
            Me.m_StatoConsulenza = StatiConsulenza.RIFIUTATA
            Me.m_DataConferma = Calendar.Now()
            Me.m_ConfermataDa = Users.CurrentUser
            Me.m_IDConfermataDa = GetID(Me.m_ConfermataDa)
            Me.SetChanged(True)
            Me.Save()
            Dim e As New ItemEventArgs(Of CQSPDConsulenza)(Me)
            Me.OnRifiutata(e)
            CQSPD.Consulenze.doConsulenzaRifiutata(e)
            CQSPD.Consulenze.Module.DispatchEvent(New EventDescription("consulenza_rifiutata", "L'operatore " & Users.CurrentUser.Nominativo & " ha registrato il rifiuto della consuenza N°" & GetID(Me) & " per il cliente " & Me.NomeCliente, Me))
        End Sub

        Protected Overridable Sub OnRifiutata(ByVal e As ItemEventArgs(Of CQSPDConsulenza))
            RaiseEvent Rifiutata(Me, e)
        End Sub

        ''' <summary>
        ''' L'operazione non è fattibile
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Boccia(ByVal motivo As String, ByVal dettaglio As String)
            'If (Me.m_StatoConsulenza >= StatiConsulenza.ACCETTATA) Then Throw New InvalidOperationException("Stato")
            Dim pratiche As CCollection(Of CRapportino) = CQSPD.Pratiche.GetPraticheByProposta(Me)
            If (pratiche.Count > 0) Then Throw New InvalidOperationException("Impossibile bocciare una proposta che ha generato una pratica valida")
            Me.m_StatoConsulenza = StatiConsulenza.BOCCIATA
            Me.m_MotivoAnnullamento = Strings.Trim(motivo)
            Me.m_DettaglioAnnullamento = dettaglio
            Me.m_DataAnnullamento = Calendar.Now()
            Me.m_AnnullataDa = Users.CurrentUser
            Me.m_IDAnnullataDa = GetID(Me.m_AnnullataDa)
            Me.m_NomeAnnullataDa = Me.m_AnnullataDa.Nominativo
            Me.SetChanged(True)
            Me.Save()
            Dim e As New ItemEventArgs(Of CQSPDConsulenza)(Me)
            Me.OnBocciata(e)
            CQSPD.Consulenze.doConsulenzaBocciata(e)
            CQSPD.Consulenze.Module.DispatchEvent(New EventDescription("consulenza_bocciata", "L'operatore " & Users.CurrentUser.Nominativo & " ha bocciato la consuenza N°" & GetID(Me) & " per il cliente " & Me.NomeCliente, Me))
        End Sub

        Protected Overridable Sub OnBocciata(ByVal e As ItemEventArgs(Of CQSPDConsulenza))
            RaiseEvent Bocciata(Me, e)
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
        ''' Segnala la consulenza ad un supervisore e la mette in stato di valutazione
        ''' </summary>
        ''' <param name="motivo"></param>
        ''' <param name="dettaglio"></param>
        ''' <param name="parametri"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function RichiediApprovazione(motivo As String, dettaglio As String, parametri As String) As CRichiestaApprovazione Implements IOggettoApprovabile.RichiediApprovazione
            'If (Me.m_StatoConsulenza <> StatiConsulenza.INSERITA) Then
            '    Throw New ArgumentException("Solo gli studi di fattibilità non ancora proposti possono essere sottoposti ai supervisori")
            'End If

            Dim rich As CRichiestaApprovazione = Me.RichiestaApprovazione

            'If (rich IsNot Nothing) AndAlso (rich.StatoRichiesta >= StatoRichiestaApprovazione.ATTESA) Then
            '    Throw New InvalidOperationException("Lo studio di fattibilità è già in attesa di approvazione")
            'End If

            If (rich Is Nothing) Then rich = New CRichiestaApprovazione
            rich.Cliente = Me.Cliente
            rich.OggettoApprovabile = Me
            rich.DataRichiestaApprovazione = Calendar.Now
            rich.UtenteRichiestaApprovazione = Sistema.Users.CurrentUser
            rich.StatoRichiesta = StatoRichiestaApprovazione.ATTESA
            rich.MotivoRichiesta = CQSPD.MotiviSconto.GetItemByName(motivo)
            rich.NomeMotivoRichiesta = motivo
            rich.ParametriRichiesta = parametri
            rich.DescrizioneRichiesta = dettaglio
            rich.Stato = ObjectStatus.OBJECT_VALID
            rich.PuntoOperativo = Me.PuntoOperativo
            rich.Save()

            'Me.m_Flags = Sistema.SetFlag(Me.m_Flags, PraticaFlags.RICHIEDEAPPROVAZIONE, True)
            'Me.m_Flags = Sistema.SetFlag(Me.m_Flags, PraticaFlags.APPROVATA, False)
            Me.RichiestaApprovazione = rich

            Me.Save()

            Dim e As New ItemEventArgs(Me)
            Me.OnRequireApprovation(e)
            CQSPD.Consulenze.DoOnRequireApprovation(e)
            Me.GetModule.DispatchEvent(New EventDescription("require_approvation", Users.CurrentUser.Nominativo & " richiede l'approvazione dell'offerta fatta per la pratica ID: " & GetID(Me), Me))


            Return rich
        End Function

        Public Sub Sollecita()
            Me.Save()
            Dim e As New ItemEventArgs(Me)
            Me.OnRequireApprovation(e)
            CQSPD.Consulenze.DoOnRequireApprovation(e)
            Me.GetModule.DispatchEvent(New EventDescription("require_approvation", Users.CurrentUser.Nominativo & " richiede l'approvazione dell'offerta fatta per la pratica ID: " & GetID(Me), Me))
        End Sub


        Protected Overridable Sub OnRequireApprovation(ByVal e As ItemEventArgs)
            RaiseEvent RequireApprovation(Me, e)
        End Sub

        Public Function PrendiInCarico() As CRichiestaApprovazione Implements IOggettoApprovabile.PrendiInCarico
            'If (Me.OffertaCorrente.StatoOfferta = StatoOfferta.APPROVATA) OrElse (Me.OffertaCorrente.StatoOfferta = StatoOfferta.RIFIUTATA) Then
            '    Throw New InvalidOperationException("L'offerta corrente è già stata supervisionata")
            'End If
            If Me.RichiestaApprovazione Is Nothing OrElse Me.RichiestaApprovazione.StatoRichiesta <> StatoRichiestaApprovazione.ATTESA Then
                Throw New InvalidOperationException("La studio di fattibilità non è in attesa di valutazione")
            End If

            Me.RichiestaApprovazione.PresaInCaricoDa = Sistema.Users.CurrentUser
            Me.RichiestaApprovazione.DataPresaInCarico = Calendar.Now
            Me.RichiestaApprovazione.StatoRichiesta = StatoRichiestaApprovazione.PRESAINCARICO
            Me.RichiestaApprovazione.Save()

            Dim note As New CAnnotazione(Me.RichiestaApprovazione)
            note.Valore = "<b>VALUTAZIONE IN CORSO</b><br/><b>Motivo</b>"
            note.Stato = ObjectStatus.OBJECT_VALID
            note.Save()


            Dim e As New ItemEventArgs(Me)
            Me.OnPresaInCarico(e)
            CQSPD.Consulenze.DoOnInCarico(e)
            Me.GetModule.DispatchEvent(New EventDescription("Presa_in_carico", Users.CurrentUser.Nominativo & " ha preso in carico la pratica ID: " & GetID(Me), Me))

            Return Me.RichiestaApprovazione
        End Function

        Protected Overridable Sub OnPresaInCarico(ByVal e As ItemEventArgs)
            RaiseEvent PresaInCarico(Me, e)
        End Sub

        Public Function Approva(motivo As String, dettaglio As String) As CRichiestaApprovazione Implements IOggettoApprovabile.Approva
            'If (Me.OffertaCorrente.StatoOfferta = StatoOfferta.APPROVATA) OrElse (Me.OffertaCorrente.StatoOfferta = StatoOfferta.RIFIUTATA) Then
            '    Throw New InvalidOperationException("L'offerta corrente è già stata supervisionata")
            'End If
            If Me.RichiestaApprovazione Is Nothing OrElse Me.RichiestaApprovazione.StatoRichiesta = StatoRichiestaApprovazione.APPROVATA OrElse Me.RichiestaApprovazione.StatoRichiesta = StatoRichiestaApprovazione.NEGATA Then
                Throw New InvalidOperationException("Lo studio di fattibilità non richiede l'approvazione o è già stata valutata")
            End If

            Me.RichiestaApprovazione.MotivoConferma = motivo
            Me.RichiestaApprovazione.DettaglioConferma = dettaglio
            Me.RichiestaApprovazione.ConfermataDa = Sistema.Users.CurrentUser
            Me.RichiestaApprovazione.DataConferma = Calendar.Now
            Me.RichiestaApprovazione.StatoRichiesta = StatoRichiestaApprovazione.APPROVATA
            Me.RichiestaApprovazione.Save()

            Dim note As New CAnnotazione(Me.RichiestaApprovazione)
            note.Valore = "<b>RICHIESTA APPROVATA</b><br/><b>Motivo</b>: " & motivo & "<br/><b>Dettaglio:</b> " & dettaglio
            note.Stato = ObjectStatus.OBJECT_VALID
            note.Save()

            Dim e As New ItemEventArgs(Me)
            Me.OnApproved(e)
            CQSPD.Consulenze.DoOnApprovata(e)
            Me.GetModule.DispatchEvent(New EventDescription("Approved", Users.CurrentUser.Nominativo & " ha approvato lo studio di fattibilità ID:" & GetID(Me), Me))


            Return Me.RichiestaApprovazione
        End Function

        Protected Overridable Sub OnApproved(ByVal e As ItemEventArgs)
            RaiseEvent Approvata(Me, e)
        End Sub

        Public Function Nega(motivo As String, dettaglio As String) As CRichiestaApprovazione Implements IOggettoApprovabile.Nega
            'If (Me.OffertaCorrente.StatoOfferta = StatoOfferta.APPROVATA) OrElse (Me.OffertaCorrente.StatoOfferta = StatoOfferta.RIFIUTATA) Then
            '    Throw New InvalidOperationException("L'offerta corrente è già stata supervisionata")
            'End If
            If Me.RichiestaApprovazione Is Nothing OrElse Me.RichiestaApprovazione.StatoRichiesta = StatoRichiestaApprovazione.APPROVATA OrElse Me.RichiestaApprovazione.StatoRichiesta = StatoRichiestaApprovazione.NEGATA Then
                Throw New InvalidOperationException("Lo studio di fattibilità non richiede l'approvazione o è già stata valutata")
            End If

            Me.RichiestaApprovazione.MotivoConferma = motivo
            Me.RichiestaApprovazione.DettaglioConferma = dettaglio
            Me.RichiestaApprovazione.ConfermataDa = Sistema.Users.CurrentUser
            Me.RichiestaApprovazione.DataConferma = Calendar.Now
            Me.RichiestaApprovazione.StatoRichiesta = StatoRichiestaApprovazione.NEGATA
            Me.RichiestaApprovazione.Save()

            Dim note As New CAnnotazione(Me.RichiestaApprovazione)
            note.Valore = "<b>RICHIESTA NEGATA</b><br/><b>Motivo</b>: " & motivo & "<br/><b>Dettaglio:</b> " & dettaglio
            note.Stato = ObjectStatus.OBJECT_VALID
            note.Save()

            Dim e As New ItemEventArgs(Me)
            Me.OnNegata(e)
            CQSPD.Consulenze.DoOnNegata(e)
            Me.GetModule.DispatchEvent(New EventDescription("Rifiutata", Users.CurrentUser.Nominativo & " ha bocciato lo studio di fattibilità ID: °" & GetID(Me), Me))

            Return Me.RichiestaApprovazione
        End Function

        Protected Overridable Sub OnNegata(ByVal e As ItemEventArgs)
            RaiseEvent Negata(Me, e)
        End Sub



        Public Function getDescrizioneOfferte() As String
            Dim ret As String = ""
            If (Me.OffertaCQS IsNot Nothing) Then ret &= "Cessione: " & Me.OffertaCQS.ToString & vbNewLine
            If (Me.OffertaPD IsNot Nothing) Then ret &= "Delega: " & Me.OffertaPD.ToString & vbNewLine
            Return ret
        End Function






        'Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
        '    Return Me.CompareTo
        'End Function

        Public Function CompareTo(other As CQSPDConsulenza) As Integer 'Implements IComparable(Of CQSPDConsulenza).CompareTo
            Dim ret As Integer = -Calendar.Compare(Me.DataConsulenza, other.DataConsulenza)
            If (ret = 0) Then ret = -(GetID(Me) - GetID(other))
            Return ret
        End Function

        Private Function CompareTo1(ByVal other As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(other)
        End Function

        Public ReadOnly Property NomePropostaDa As String
            Get
                If Me.PropostaDa Is Nothing Then Return ""
                Return Me.PropostaDa.Nominativo
            End Get
        End Property

        Public ReadOnly Property NomeConfermataDa As String
            Get
                If Me.ConfermataDa Is Nothing Then Return ""
                Return Me.ConfermataDa.Nominativo
            End Get
        End Property

        Public ReadOnly Property NumeroProposta As String
            Get
                Return Strings.Right("00000000" & Strings.Hex(GetID(Me)), 8)
            End Get
        End Property

        Public ReadOnly Property NettoAllaMano As Decimal?
            Get
                Dim nr As Decimal? = Me.NettoRicavo
                If (nr.HasValue = False) Then Return Nothing
                Dim se As Decimal? = Me.SommaEstinzioni
                If (se.HasValue = False) Then Return Nothing
                Return nr.Value - se.Value
            End Get
        End Property

    End Class

End Class

Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Class Office


    Public Enum StatoOggettoDaSpedire As Integer
        ''' <summary>
        ''' Oggetto in preparazione (ancora non deve essere spedito)
        ''' </summary>
        ''' <remarks></remarks>
        InPreparazione = 0

        ''' <summary>
        ''' Oggetto pronto (deve essere spedito)
        ''' </summary>
        ''' <remarks></remarks>
        ProntoPerLaSpedizione = 10

        ''' <summary>
        ''' L'oggetto è stato Spedito
        ''' </summary>
        ''' <remarks></remarks>
        Spedito = 20

        ''' <summary>
        ''' La spedizione è stata annullata dall'operatore
        ''' </summary>
        ''' <remarks></remarks>
        SpedizioneAnnullata = 30

        ''' <summary>
        ''' La spedizione è stata rifiutata dal corriere
        ''' </summary>
        ''' <remarks></remarks>
        SpedizioneRifiutata = 31

        ''' <summary>
        ''' La spedizione è stata fermata dall'agenzia 
        ''' </summary>
        ''' <remarks></remarks>
        SpedizioneBocciata = 32

        ''' <summary>
        ''' L'oggetto è stato consegnato correttamente
        ''' </summary>
        ''' <remarks></remarks>
        Consegnato = 40


        ''' <summary>
        ''' Non è stato possibile consegnare l'oggetto
        ''' </summary>
        ''' <remarks></remarks>
        ConsegnaFallita = 50

        ''' <summary>
        ''' Consegna fallita perché l'indirizzo è errato
        ''' </summary>
        ''' <remarks></remarks>
        ConsegnaFallitaIndirizzoErrato = 51

        ''' <summary>
        ''' Consegna fallita perché il destinatario ha rifiutato l'oggetto
        ''' </summary>
        ''' <remarks></remarks>
        ConsegnaFallitaRifiutoDestinatario = 52

        ''' <summary>
        ''' Consegna fallita perché il destinatario non era all'indirizzo indicato
        ''' </summary>
        ''' <remarks></remarks>
        ConsegnaFallitaNonTrovato = 53
    End Enum


    <Flags> _
    Public Enum OggettoDaSpedireFlags As Integer
        None = 0
    End Enum

    ''' <summary>
    ''' Rappresenta un oggetto da spedire
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class OggettoDaSpedire
        Inherits DBObjectPO
        Implements IComparable



        Private m_AspettoBeni As String                 'Indica il tipo di oggetto spedito (busta, pacco, pallet)

        Private m_IDCliente As Integer                  'Persona per cui si spedisce l'oggetto
        <NonSerialized> Private m_Cliente As CPersona
        Private m_NomeCliente As String

        Private m_IDDestinatario As Integer             'Persona a cui è destinato l'oggetto
        <NonSerialized> Private m_Destinatario As CPersona
        Private m_NomeDestinatario As String

        Private m_IndirizzoMittente As CIndirizzo   'Indirizzo a cui inviare l'oggetto
        Private m_IndirizzoDestinatario As CIndirizzo   'Indirizzo a cui inviare l'oggetto

        Private m_NumeroColli As Integer?                'Numero di colli spediti
        Private m_Peso As Double?                       'Peso degli oggetti spediti
        Private m_Altezza As Double?                    'Altezza
        Private m_Larghezza As Double?                  'Larghezza
        Private m_Profondita As Double?                'Profondita

        Private m_RichiestaDa As CUser                  'Operatore che ha richiesto la spedizione
        Private m_IDRichiestaDa As Integer
        Private m_NomeRichiestaDa As String
        Private m_DataRichiesta As Date?

        Private m_PresaInCaricoDa As CUser              'Utente che ha preso in carico l'oggetto ed ha predisposto la spedizione
        Private m_IDPresaInCaricoDa As Integer
        Private m_NomePresaInCaricoDa As String
        Private m_DataPresaInCarico As Date?

        Private m_ConfermatoDa As CUser                 'Utente che ha registrato l'esito finale della spedizione
        Private m_IDConfermatoDa As Integer
        Private m_NomeConfermatoDa As String
        Private m_DataConferma As Date?

        Private m_DescrizioneSpedizione As String

        Private m_NotePerIlCorriere As String
        Private m_NotePerIlDestinatario As String

        Private m_StatoOggetto As StatoOggettoDaSpedire
        Private m_Flags As OggettoDaSpedireFlags
        Private m_DettaglioStato As String

        Private m_DataInizioSpedizione As Date?
        Private m_DataConsegna As Date?

        Private m_CategoriaContenuto As String
        Private m_DescrizioneContenuto As String

        Private m_IDSpedizione As Integer
        Private m_Spedizione As Spedizione
        Private m_NumeroSpedizione As String

        Private m_IDCorriere As Integer
        Private m_NomeCorriere As String



        Public Sub New()
            Me.m_AspettoBeni = ""
            Me.m_IDCliente = 0
            Me.m_Cliente = Nothing
            Me.m_NomeCliente = ""
            Me.m_IDDestinatario = 0
            Me.m_Destinatario = Nothing
            Me.m_NomeDestinatario = ""
            Me.m_IndirizzoMittente = New CIndirizzo
            Me.m_IndirizzoDestinatario = New CIndirizzo
            Me.m_NumeroColli = Nothing
            Me.m_Peso = Nothing
            Me.m_Altezza = Nothing
            Me.m_Larghezza = Nothing
            Me.m_Profondita = Nothing
            Me.m_RichiestaDa = Nothing
            Me.m_IDRichiestaDa = 0
            Me.m_NomeRichiestaDa = ""
            Me.m_DataRichiesta = Nothing
            Me.m_PresaInCaricoDa = Nothing
            Me.m_IDPresaInCaricoDa = 0
            Me.m_NomePresaInCaricoDa = ""
            Me.m_DataPresaInCarico = Nothing
            Me.m_ConfermatoDa = Nothing
            Me.m_IDConfermatoDa = 0
            Me.m_NomeConfermatoDa = ""
            Me.m_DataConferma = Nothing
            Me.m_DescrizioneSpedizione = ""
            Me.m_NotePerIlCorriere = ""
            Me.m_NotePerIlDestinatario = ""
            Me.m_StatoOggetto = StatoOggettoDaSpedire.InPreparazione
            Me.m_Flags = OggettoDaSpedireFlags.None
            Me.m_DettaglioStato = ""
            Me.m_DataInizioSpedizione = Nothing
            Me.m_DataConsegna = Nothing
            Me.m_CategoriaContenuto = ""
            Me.m_DescrizioneContenuto = ""

            Me.m_IDSpedizione = 0
            Me.m_Spedizione = Nothing
            Me.m_NumeroSpedizione = ""

            Me.m_IDCorriere = 0
            Me.m_NomeCorriere = ""

        End Sub

        ''' <summary>
        ''' Restituisce o imposta una stringa che identifica la categoria del contenuto (es. Documenti, ...)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CategoriaContenuto As String
            Get
                Return Me.m_CategoriaContenuto
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_CategoriaContenuto
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_CategoriaContenuto = value
                Me.DoChanged("CategoriaContenuto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce la descrizione per esteso del contenuto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DescrizioneContenuto As String
            Get
                Return Me.m_DescrizioneContenuto
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_DescrizioneContenuto
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_DescrizioneContenuto = value
                Me.DoChanged("DescrizioneContenuto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta una stringa che descrive l'aspetto degli oggetti spediti (Buste, Pacchi, Pallet, )
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AspettoBeni As String
            Get
                Return Me.m_AspettoBeni
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_AspettoBeni
                If (oldValue = value) Then Exit Property
                Me.m_AspettoBeni = value
                Me.DoChanged("AspettoBeni", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID della persona che ha inviato l'oggetto (mittente)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDCliente As Integer
            Get
                Return GetID(Me.m_Cliente, Me.m_IDCliente)
            End Get
            Set(value As Integer)
                Dim oldValue As String = Me.IDCliente
                If (oldValue = value) Then Exit Property
                Me.m_IDCliente = value
                Me.m_Cliente = Nothing
                Me.DoChanged("IDMittente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la persona che ha inviato l'oggetto (mittente)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Cliente As CPersona
            Get
                If Me.m_Cliente Is Nothing Then Me.m_Cliente = Anagrafica.Persone.GetItemById(Me.m_IDCliente)
                Return Me.m_Cliente
            End Get
            Set(value As CPersona)
                Dim oldValue As CPersona = Me.m_Cliente
                If (oldValue Is value) Then Exit Property
                Me.m_Cliente = value
                Me.m_IDCliente = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeCliente = value.Nominativo
                Me.DoChanged("Cliente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del mittente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
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

        ''' <summary>
        ''' Restituisce l'oggetto che descrive l'indirizzo del mittente (da cui parte la spedizione)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property IndirizzoMittente As CIndirizzo
            Get
                Return Me.m_IndirizzoMittente
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID della persona a cui è destinata la spedizione (Destinatario)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDDestinatario As Integer
            Get
                Return GetID(Me.m_Destinatario, Me.m_IDDestinatario)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDDestinatario
                If (oldValue = value) Then Exit Property
                Me.m_Destinatario = Nothing
                Me.m_IDDestinatario = value
                Me.DoChanged("IDDestinatario", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la persona a cui è destinata la spedizione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Destinatario As CPersona
            Get
                If Me.m_Destinatario Is Nothing Then Me.m_Destinatario = Anagrafica.Persone.GetItemById(Me.m_IDDestinatario)
                Return Me.m_Destinatario
            End Get
            Set(value As CPersona)
                Dim oldValue As CPersona = Me.m_Destinatario
                If (oldValue Is value) Then Exit Property
                Me.m_Destinatario = value
                Me.m_IDDestinatario = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeDestinatario = value.Nominativo
                Me.DoChanged("Destinatario", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome della persoan a cui è destinata la spedizione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeDestinatario As String
            Get
                Return Me.m_NomeDestinatario
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeDestinatario
                If (oldValue = value) Then Exit Property
                Me.m_NomeDestinatario = value
                Me.DoChanged("NomeDestinatario", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce l'oggetto che descrive l'indirizzo presso cui consegnare la spedizione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property IndirizzoDestinatario As CIndirizzo
            Get
                Return Me.m_IndirizzoDestinatario
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imosta il numero di colli della spedizione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NumeroColli As Integer?
            Get
                Return Me.m_NumeroColli
            End Get
            Set(value As Integer?)
                Dim oldValue As Integer? = Me.m_NumeroColli
                If (oldValue = value) Then Exit Property
                Me.m_NumeroColli = value
                Me.DoChanged("NumeroColli", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il peso totale degli oggetti spediti
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Peso As Double?
            Get
                Return Me.m_Peso
            End Get
            Set(value As Double?)
                Dim oldValue As Double = Me.m_Peso
                If (oldValue = value) Then Exit Property
                Me.m_Peso = value
                Me.DoChanged("Peso", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'altezza degli oggetti spediti
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Altezza As Double?
            Get
                Return Me.m_Altezza
            End Get
            Set(value As Double?)
                Dim oldValue As Double? = Me.m_Altezza
                If (oldValue = value) Then Exit Property
                Me.m_Altezza = value
                Me.DoChanged("Altezza", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la larghezza degli oggetti spediti
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Larghezza As Double?
            Get
                Return Me.m_Larghezza
            End Get
            Set(value As Double?)
                Dim oldValue As Double? = Me.m_Larghezza
                If (oldValue = value) Then Exit Property
                Me.m_Larghezza = value
                Me.DoChanged("Larghezza", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la profondità degli oggetti spediti
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Profondita As Double?
            Get
                Return Me.m_Profondita
            End Get
            Set(value As Double?)
                Dim oldValue As Double? = Me.m_Profondita
                If (oldValue = value) Then Exit Property
                Me.m_Profondita = value
                Me.DoChanged("Profondita", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'utente che richiesto la spedizione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property RichiestaDa As CUser
            Get
                If (Me.m_RichiestaDa Is Nothing) Then Me.m_RichiestaDa = Sistema.Users.GetItemById(Me.m_IDRichiestaDa)
                Return Me.m_RichiestaDa
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.RichiestaDa
                If (oldValue Is value) Then Exit Property
                Me.m_RichiestaDa = value
                Me.m_IDRichiestaDa = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeRichiestaDa = value.Nominativo
                Me.DoChanged("RichiestaDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'utente che ha richiesto la spedizione 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDRichiestaDa As Integer
            Get
                Return GetID(Me.m_RichiestaDa, Me.m_IDRichiestaDa)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDRichiestaDa
                If (oldValue = value) Then Exit Property
                Me.m_IDRichiestaDa = value
                Me.m_RichiestaDa = Nothing
                Me.DoChanged("IDRichiestaDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome dell'utente che ha richiesto la spedizione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeRichiestaDa As String
            Get
                Return Me.m_NomeRichiestaDa
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeRichiestaDa
                If (oldValue = value) Then Exit Property
                Me.m_NomeRichiestaDa = value
                Me.DoChanged("NomeRichiestaDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data e l'ora in cui l'oggetto è stato marcato come pronto per la spedizione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataRichiesta As Date?
            Get
                Return Me.m_DataRichiesta
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataRichiesta
                If (DateUtils.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_DataRichiesta = value
                Me.DoChanged("DataRichiesta", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'utente che ha preso in carico la spedizione dell'oggetto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PresaInCaricoDa As CUser
            Get
                If Me.m_PresaInCaricoDa Is Nothing Then Me.m_PresaInCaricoDa = Sistema.Users.GetItemById(Me.m_IDPresaInCaricoDa)
                Return Me.m_PresaInCaricoDa
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.PresaInCaricoDa
                If (oldValue Is value) Then Exit Property
                Me.m_PresaInCaricoDa = value
                Me.m_IDPresaInCaricoDa = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomePresaInCaricoDa = value.Nominativo
                Me.DoChanged("PresaInCaricoDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'utente che ha preso in carico l'oggetto per la spedizione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDPresaInCaricoDa As Integer
            Get
                Return GetID(Me.m_PresaInCaricoDa, Me.m_IDPresaInCaricoDa)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDPresaInCaricoDa
                If (oldValue = value) Then Exit Property
                Me.m_IDPresaInCaricoDa = value
                Me.m_PresaInCaricoDa = Nothing
                Me.DoChanged("IDPresaInCaricoDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome dell'utente che ha preso in carico l'oggetto per la spedizione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomePresaInCaricoDa As String
            Get
                Return Me.m_NomePresaInCaricoDa
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomePresaInCaricoDa
                If (oldValue = value) Then Exit Property
                Me.m_NomePresaInCaricoDa = value
                Me.DoChanged("NomePresaInCaricoDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data in cui l'utente ha preso in carico l'oggetto per la spedizione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataPresaInCarico As Date?
            Get
                Return Me.m_DataPresaInCarico
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataPresaInCarico
                If (DateUtils.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_DataPresaInCarico = value
                Me.DoChanged("DataPresaInCarico", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'utente che ha registrato la conclusione della spedizione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ConfermatoDa As CUser                 'Utente che ha registrato l'esito finale della spedizione
            Get
                If (Me.m_ConfermatoDa Is Nothing) Then Me.m_ConfermatoDa = Sistema.Users.GetItemById(Me.m_IDConfermatoDa)
                Return Me.m_ConfermatoDa
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.ConfermatoDa
                If (oldValue Is value) Then Exit Property
                Me.m_ConfermatoDa = value
                Me.m_IDConfermatoDa = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeConfermatoDa = value.Nominativo
                Me.DoChanged("ConfermatoDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'utente che ha confermato l'esito della registrazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDConfermatoDa As Integer
            Get
                Return GetID(Me.m_ConfermatoDa, Me.m_IDConfermatoDa)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDConfermatoDa
                If (oldValue = value) Then Exit Property
                Me.m_IDConfermatoDa = value
                Me.m_ConfermatoDa = Nothing
                Me.DoChanged("IDConfermatoDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome dell'utente che ha confermato l'esito della spedizione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeConfermatoDa As String
            Get
                Return Me.m_NomeConfermatoDa
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeConfermatoDa
                If (oldValue = value) Then Exit Property
                Me.m_NomeConfermatoDa = value
                Me.DoChanged("NomeConfermatoDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di registrazione dell'esito della spedizione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataConferma As Date?
            Get
                Return Me.m_DataConferma
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataConferma
                If (DateUtils.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_DataConferma = value
                Me.DoChanged("DataConferma", value, oldValue)
            End Set
        End Property


        ''' <summary>
        ''' Restituisce o imposta la data di inizio della spedizione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataInizioSpedizione As Date?
            Get
                Return Me.m_DataInizioSpedizione
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataInizioSpedizione
                If (DateUtils.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_DataInizioSpedizione = value
                Me.DoChanged("DataInizioSpedizione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta una stringa che descrive la spedizione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DescrizioneSpedizione As String
            Get
                Return Me.m_DescrizioneSpedizione
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_DescrizioneSpedizione
                If (oldValue = value) Then Exit Property
                Me.m_DescrizioneSpedizione = value
                Me.DoChanged("DescrizioneSpedizione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta una stringa contenente degli avvertimenti diretti al corriere
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NotePerIlCorriere As String
            Get
                Return Me.m_NotePerIlCorriere
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NotePerIlCorriere
                If (oldValue = value) Then Exit Property
                Me.m_NotePerIlCorriere = value
                Me.DoChanged("NotePerIlCorriere", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta delle annotazioni dirette al destinatario
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NotePerIlDestinatario As String
            Get
                Return Me.m_NotePerIlDestinatario
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NotePerIlDestinatario
                If (oldValue = value) Then Exit Property
                Me.m_NotePerIlDestinatario = value
                Me.DoChanged("NotePerIlDestinatario", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta lo stato della spedizione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StatoOggetto As StatoOggettoDaSpedire
            Get
                Return Me.m_StatoOggetto
            End Get
            Set(value As StatoOggettoDaSpedire)
                Dim oldValue As StatoOggettoDaSpedire = Me.m_StatoOggetto
                If (oldValue = value) Then Exit Property
                Me.m_StatoOggetto = value
                Me.DoChanged("StatoOggetto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta dei flags aggiuntivi
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Flags As OggettoDaSpedireFlags
            Get
                Return Me.m_Flags
            End Get
            Set(value As OggettoDaSpedireFlags)
                Dim oldValue As OggettoDaSpedireFlags = Me.m_Flags
                If (oldValue = value) Then Exit Property
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di consegna
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataConsegna As Date?
            Get
                Return Me.m_DataConsegna
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataConsegna
                If (DateUtils.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_DataConsegna = value
                Me.DoChanged("DataConsegna", value, oldValue)
            End Set
        End Property



        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Public Overrides Function GetModule() As Sistema.CModule
            Return Office.OggettiDaSpedire.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeOggettiDaSpedire"
        End Function

        Public Overrides Function IsChanged() As Boolean
            Return MyBase.IsChanged() OrElse Me.m_IndirizzoDestinatario.IsChanged OrElse Me.m_IndirizzoMittente.IsChanged
        End Function

        Protected Overrides Function SaveToDatabase(dbConn As CDBConnection, force As Boolean) As Boolean
            Dim ret As Boolean = MyBase.SaveToDatabase(dbConn, force)
            If (ret) Then
                Me.m_IndirizzoDestinatario.SetChanged(False)
                Me.m_IndirizzoMittente.SetChanged(False)
            End If
            Return ret
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_AspettoBeni = reader.Read("AspettoBeni", Me.m_AspettoBeni)
            Me.m_IDCliente = reader.Read("IDCliente", Me.m_IDCliente)
            Me.m_NomeCliente = reader.Read("NomeCliente", Me.m_NomeCliente)
            Me.m_IDDestinatario = reader.Read("IDDestinatario", Me.m_IDDestinatario)
            Me.m_NomeDestinatario = reader.Read("NomeDestinatario", Me.m_NomeDestinatario)
            With Me.m_IndirizzoMittente
                .Nome = reader.Read("INDMITT_LABEL", .Nome)
                .CAP = reader.Read("INDMITT_CAP", .CAP)
                .Citta = reader.Read("INDMITT_CITTA", .Citta)
                .Provincia = reader.Read("INDMITT_PROV", .Provincia)
                .ToponimoViaECivico = reader.Read("INDMITT_VIA", .ToponimoViaECivico)
                .SetChanged(False)
            End With
            With Me.m_IndirizzoDestinatario
                .Nome = reader.Read("INDDEST_LABEL", .Nome)
                .CAP = reader.Read("INDDEST_CAP", .CAP)
                .Citta = reader.Read("INDDEST_CITTA", .Citta)
                .Provincia = reader.Read("INDDEST_PROV", .Provincia)
                .ToponimoViaECivico = reader.Read("INDDEST_VIA", .ToponimoViaECivico)
                .SetChanged(False)
            End With

            Me.m_NumeroColli = reader.Read("NumeroColli", Me.m_NumeroColli)
            Me.m_Peso = reader.Read("Peso", Me.m_Peso)
            Me.m_Altezza = reader.Read("Altezza", Me.m_Altezza)
            Me.m_Larghezza = reader.Read("Larghezza", Me.m_Larghezza)
            Me.m_Profondita = reader.Read("Profondita", Me.m_Profondita)

            Me.m_IDRichiestaDa = reader.Read("IDRichiestaDa", Me.m_IDRichiestaDa)
            Me.m_NomeRichiestaDa = reader.Read("NomeRichiestaDa", Me.m_NomeRichiestaDa)
            Me.m_DataRichiesta = reader.Read("DataRichiesta", Me.m_DataRichiesta)

            Me.m_IDPresaInCaricoDa = reader.Read("IDPresaInCaricoDa", Me.m_IDPresaInCaricoDa)
            Me.m_NomePresaInCaricoDa = reader.Read("NomePresaInCaricoDa", Me.m_NomePresaInCaricoDa)
            Me.m_DataPresaInCarico = reader.Read("DataPresaInCarico", Me.m_DataPresaInCarico)

            Me.m_IDConfermatoDa = reader.Read("IDConfermatoDa", Me.m_IDConfermatoDa)
            Me.m_NomeConfermatoDa = reader.Read("NomeConfermatoDa", Me.m_NomeConfermatoDa)
            Me.m_DataConferma = reader.Read("DataConferma", Me.m_DataConferma)

            Me.m_DescrizioneSpedizione = reader.Read("DescrizioneSpedizione", Me.m_DescrizioneSpedizione)

            Me.m_NotePerIlCorriere = reader.Read("NotePerIlCorriere", Me.m_NotePerIlCorriere)
            Me.m_NotePerIlDestinatario = reader.Read("NotePerIlDestinatario", Me.m_NotePerIlDestinatario)

            Me.m_StatoOggetto = reader.Read("StatoOggetto", Me.m_StatoOggetto)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_DettaglioStato = reader.Read("DettaglioStato", Me.m_DettaglioStato)

            Me.m_DataInizioSpedizione = reader.Read("DataInizioSpedizione", Me.m_DataInizioSpedizione)
            Me.m_DataConsegna = reader.Read("DataConsegna", Me.m_DataConsegna)

            Me.m_CategoriaContenuto = reader.Read("CategoriaContenuto", Me.m_CategoriaContenuto)
            Me.m_DescrizioneContenuto = reader.Read("DescrizioneContenuto", Me.m_DescrizioneContenuto)

            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("AspettoBeni", Me.m_AspettoBeni)
            writer.Write("IDCliente", Me.IDCliente)
            writer.Write("NomeCliente", Me.m_NomeCliente)
            writer.Write("IDDestinatario", Me.IDDestinatario)
            writer.Write("NomeDestinatario", Me.m_NomeDestinatario)
            With Me.m_IndirizzoMittente
                writer.Write("INDMITT_LABEL", .Nome)
                writer.Write("INDMITT_CAP", .CAP)
                writer.Write("INDMITT_CITTA", .Citta)
                writer.Write("INDMITT_PROV", .Provincia)
                writer.Write("INDMITT_VIA", .ToponimoViaECivico)
            End With
            With Me.m_IndirizzoDestinatario
                writer.Write("INDDEST_LABEL", .Nome)
                writer.Write("INDDEST_CAP", .CAP)
                writer.Write("INDDEST_CITTA", .Citta)
                writer.Write("INDDEST_PROV", .Provincia)
                writer.Write("INDDEST_VIA", .ToponimoViaECivico)
            End With

            writer.Write("NumeroColli", Me.m_NumeroColli)
            writer.Write("Peso", Me.m_Peso)
            writer.Write("Altezza", Me.m_Altezza)
            writer.Write("Larghezza", Me.m_Larghezza)
            writer.Write("Profondita", Me.m_Profondita)

            writer.Write("IDRichiestaDa", Me.IDRichiestaDa)
            writer.Write("NomeRichiestaDa", Me.m_NomeRichiestaDa)
            writer.Write("DataRichiesta", Me.m_DataRichiesta)

            writer.Write("DescrizioneSpedizione", Me.m_DescrizioneSpedizione)

            writer.Write("NotePerIlCorriere", Me.m_NotePerIlCorriere)
            writer.Write("NotePerIlDestinatario", Me.m_NotePerIlDestinatario)

            writer.Write("StatoOggetto", Me.m_StatoOggetto)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("DettaglioStato", Me.m_DettaglioStato)

            writer.Write("DataInizioSpedizione", Me.m_DataInizioSpedizione)
            writer.Write("DataConsegna", Me.m_DataConsegna)

            writer.Write("CategoriaContenuto", Me.m_CategoriaContenuto)
            writer.Write("DescrizioneContenuto", Me.m_DescrizioneContenuto)


            writer.Write("IDPresaInCaricoDa", Me.IDPresaInCaricoDa)
            writer.Write("NomePresaInCaricoDa", Me.m_NomePresaInCaricoDa)
            writer.Write("DataPresaInCarico", Me.m_DataPresaInCarico)

            writer.Write("IDConfermatoDa", Me.IDConfermatoDa)
            writer.Write("NomeConfermatoDa", Me.m_NomeConfermatoDa)
            writer.Write("DataConferma", Me.m_DataConferma)

            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("AspettoBeni", Me.m_AspettoBeni)
            writer.WriteAttribute("IDCliente", Me.IDCliente)
            writer.WriteAttribute("NomeCliente", Me.m_NomeCliente)
            writer.WriteAttribute("IDDestinatario", Me.IDDestinatario)
            writer.WriteAttribute("NomeDestinatario", Me.m_NomeDestinatario)
            writer.WriteAttribute("NumeroColli", Me.m_NumeroColli)
            writer.WriteAttribute("Peso", Me.m_Peso)
            writer.WriteAttribute("Altezza", Me.m_Altezza)
            writer.WriteAttribute("Larghezza", Me.m_Larghezza)
            writer.WriteAttribute("Profondita", Me.m_Profondita)
            writer.WriteAttribute("IDRichiestaDa", Me.IDRichiestaDa)
            writer.WriteAttribute("NomeRichiestaDa", Me.m_NomeRichiestaDa)
            writer.WriteAttribute("DataRichiesta", Me.m_DataRichiesta)
            writer.WriteAttribute("StatoOggetto", Me.m_StatoOggetto)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("DettaglioStato", Me.m_DettaglioStato)
            writer.WriteAttribute("DataInizioSpedizione", Me.m_DataInizioSpedizione)
            writer.WriteAttribute("DataConsegna", Me.m_DataConsegna)
            writer.WriteAttribute("CategoriaContenuto", Me.m_CategoriaContenuto)
            writer.WriteAttribute("IDPresaInCaricoDa", Me.IDPresaInCaricoDa)
            writer.WriteAttribute("NomePresaInCaricoDa", Me.m_NomePresaInCaricoDa)
            writer.WriteAttribute("DataPresaInCarico", Me.m_DataPresaInCarico)
            writer.WriteAttribute("IDConfermatoDa", Me.IDConfermatoDa)
            writer.WriteAttribute("NomeConfermatoDa", Me.m_NomeConfermatoDa)
            writer.WriteAttribute("DataConferma", Me.m_DataConferma)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("INDMITT", Me.m_IndirizzoMittente)
            writer.WriteTag("INDDEST", Me.m_IndirizzoDestinatario)
            writer.WriteTag("DescrizioneContenuto", Me.m_DescrizioneContenuto)
            writer.WriteTag("DescrizioneSpedizione", Me.m_DescrizioneSpedizione)
            writer.WriteTag("NotePerIlCorriere", Me.m_NotePerIlCorriere)
            writer.WriteTag("NotePerIlDestinatario", Me.m_NotePerIlDestinatario)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "AspettoBeni" : Me.m_AspettoBeni = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDCliente" : Me.m_IDCliente = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeCliente" : Me.m_NomeCliente = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDDestinatario" : Me.IDDestinatario = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeDestinatario" : Me.m_NomeDestinatario = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "NumeroColli" : Me.m_NumeroColli = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Peso" : Me.m_Peso = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Altezza" : Me.m_Altezza = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Larghezza" : Me.m_Larghezza = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Profondita" : Me.m_Profondita = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "IDRichiestaDa" : Me.m_IDRichiestaDa = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeRichiestaDa" : Me.m_NomeRichiestaDa = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataRichiesta" : Me.m_DataRichiesta = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "StatoOggetto" : Me.m_StatoOggetto = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "DettaglioStato" : Me.m_DettaglioStato = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataInizioSpedizione" : Me.m_DataInizioSpedizione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataConsegna" : Me.m_DataConsegna = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "INDMITT" : Me.m_IndirizzoMittente = fieldValue
                Case "INDDEST" : Me.m_IndirizzoDestinatario = fieldValue
                Case "DescrizioneSpedizione" : Me.m_DescrizioneSpedizione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "NotePerIlCorriere" : Me.m_NotePerIlCorriere = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "NotePerIlDestinatario" : Me.m_NotePerIlDestinatario = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "CategoriaContenuto" : Me.m_CategoriaContenuto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DescrizioneContenuto" : Me.m_DescrizioneContenuto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDPresaInCaricoDa" : Me.m_IDPresaInCaricoDa = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomePresaInCaricoDa" : Me.m_NomePresaInCaricoDa = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataPresaInCarico" : Me.m_DataPresaInCarico = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IDConfermatoDa" : Me.m_IDConfermatoDa = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeConfermatoDa" : Me.m_NomeConfermatoDa = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataConferma" : Me.m_DataConferma = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select

        End Sub

        Public Function CompareTo(ByVal b As OggettoDaSpedire) As Integer
            Return DateUtils.Compare(Me.m_DataInizioSpedizione, b.m_DataInizioSpedizione)
        End Function

        Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(obj)
        End Function


    End Class



End Class
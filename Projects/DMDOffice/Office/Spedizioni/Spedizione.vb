Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Class Office

    Public Enum StatoSpedizione As Integer
        ''' <summary>
        ''' Oggetto in preparazione, non ancora spedito
        ''' </summary>
        ''' <remarks></remarks>
        InPreparazione = 0

        ''' <summary>
        ''' L'oggetto è stato dato nelle mani del corriere
        ''' </summary>
        ''' <remarks></remarks>
        Spedita = 10

        ''' <summary>
        ''' L'oggetto è stato stoccato nel magazzino del corriere
        ''' </summary>
        ''' <remarks></remarks>
        FermaInMagazzino = 20

        ''' <summary>
        ''' L'ggetto è stato spedito ad un altro magazzino del corriere
        ''' </summary>
        ''' <remarks></remarks>
        TrasferimentoMagazzino = 30

        ''' <summary>
        ''' L'oggetto è in consegna (dal magazzino del corriere al destinatario)
        ''' </summary>
        ''' <remarks></remarks>
        InConsegna = 40

        ''' <summary>
        ''' C'è stato un problema nella consegna dell'oggetto (vedi flags) e 
        ''' </summary>
        ''' <remarks></remarks>
        Fallita = 50


    End Enum

    Public Enum StatoConsegna As Integer
        ''' <summary>
        ''' Non consegnato
        ''' </summary>
        ''' <remarks></remarks>
        NonConsegnata = 0

        ''' <summary>
        ''' Consegnato
        ''' </summary>
        ''' <remarks></remarks>
        Consegnata = 1

        ''' <summary>
        ''' Indirizzo del destinatario non corretto
        ''' </summary>
        ''' <remarks></remarks>
        IndirizzoDestinatarioErrato = 2

        ''' <summary>
        ''' Il destinatario non era all'indirizzo indicato
        ''' </summary>
        ''' <remarks></remarks>
        DestinatarioNonTrovato = 3

        ''' <summary>
        ''' Il destinatario ha rifiutato la consegna
        ''' </summary>
        ''' <remarks></remarks>
        DestinatarioRifiuto = 4

    End Enum

    <Flags> _
    Public Enum SpedizioneFlags As Integer
        None = 0
        Effettuata = 1
    End Enum


    ''' <summary>
    ''' Rappresenta una spedizione effettuata tramite corriere
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class Spedizione
        Inherits DBObjectPO
        Implements IComparable



        Private m_AspettoBeni As String                 'Indica il tipo di oggetto spedito (busta, pacco, pallet)

        Private m_IDMittente As Integer                 'Persona che invia l'oggetto
        <NonSerialized> Private m_Mittente As CPersona
        Private m_NomeMittente As String

        Private m_IndirizzoMittente As CIndirizzo       'Indirizzo da cui è partita la spedizione

        Private m_IDDestinatario As Integer             'Persona a cui è destinato l'oggetto
        <NonSerialized> Private m_Destinatario As CPersona
        Private m_NomeDestinatario As String

        Private m_IndirizzoDestinatario As CIndirizzo   'Indirizzo a cui inviare l'oggetto

        Private m_NumeroColli As Integer?                'Numero di colli spediti
        Private m_Peso As Double?                       'Peso degli oggetti spediti
        Private m_Altezza As Double?                    'Altezza
        Private m_Larghezza As Double?                  'Larghezza
        Private m_Profondita As Double?                'Profondita

        <NonSerialized> _
        Private m_SpeditoDa As CUser                    'Operatore che ha effettuato la spedizione
        Private m_IDSpeditoDa As Integer
        Private m_NomeSpeditoDa As String

        <NonSerialized> _
        Private m_RicevutoDa As CUser                    'Operatore che ha effettuato la spedizione
        Private m_IDRicevutoDa As Integer
        Private m_NomeRicevutoDa As String

        Private m_DataInizioSpedizione As Date?

        Private m_NotePerIlCorriere As String

        Private m_NotePerIlDestinatario As String

        Private m_StatoSpedizione As StatoSpedizione
        Private m_StatoConsegna As StatoConsegna

        Private m_DataConsegna As Date?

        Private m_Flags As SpedizioneFlags

        Private m_NomeCorriere As String
        Private m_IDCorriere As Integer
        Private m_NumeroSpedizione As String

        Private m_Passaggi As CCollection(Of PassaggioSpedizione)
        Private m_Attributi As CKeyCollection


        Public Sub New()
            Me.m_AspettoBeni = ""
            Me.m_IDMittente = 0
            Me.m_Mittente = Nothing
            Me.m_NomeMittente = ""
            Me.m_IndirizzoMittente = New CIndirizzo
            Me.m_IDDestinatario = 0
            Me.m_Destinatario = Nothing
            Me.m_NomeDestinatario = ""
            Me.m_IndirizzoDestinatario = New CIndirizzo
            Me.m_NumeroColli = Nothing
            Me.m_Peso = Nothing
            Me.m_Altezza = Nothing
            Me.m_Larghezza = Nothing
            Me.m_Profondita = Nothing
            Me.m_SpeditoDa = Nothing
            Me.m_IDSpeditoDa = 0
            Me.m_NomeSpeditoDa = ""
            Me.m_DataInizioSpedizione = Nothing
            Me.m_NotePerIlCorriere = ""
            Me.m_NotePerIlDestinatario = ""
            Me.m_StatoSpedizione = StatoSpedizione.InPreparazione
            Me.m_StatoConsegna = StatoConsegna.NonConsegnata
            Me.m_DataConsegna = Nothing
            Me.m_Flags = SpedizioneFlags.Effettuata
            Me.m_NomeCorriere = ""
            Me.m_IDCorriere = 0
            Me.m_NumeroSpedizione = ""
            Me.m_Passaggi = Nothing
            Me.m_Attributi = Nothing
        End Sub

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
        Public Property IDMittente As Integer
            Get
                Return GetID(Me.m_Mittente, Me.m_IDMittente)
            End Get
            Set(value As Integer)
                Dim oldValue As String = Me.IDMittente
                If (oldValue = value) Then Exit Property
                Me.m_IDMittente = value
                Me.m_Mittente = Nothing
                Me.DoChanged("IDMittente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la persona che ha inviato l'oggetto (mittente)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Mittente As CPersona
            Get
                If Me.m_Mittente Is Nothing Then Me.m_Mittente = Anagrafica.Persone.GetItemById(Me.m_IDMittente)
                Return Me.m_Mittente
            End Get
            Set(value As CPersona)
                Dim oldValue As CPersona = Me.m_Mittente
                If (oldValue Is value) Then Exit Property
                Me.m_Mittente = value
                Me.m_IDMittente = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeMittente = value.Nominativo
                Me.DoChanged("Mittente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del mittente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeMittente As String
            Get
                Return Me.m_NomeMittente
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeMittente
                If (oldValue = value) Then Exit Property
                Me.m_NomeMittente = value
                Me.DoChanged("NomeMittente", value, oldValue)
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
                Dim oldValue As Double = Me.m_Larghezza
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
        ''' Restituisce o imposta l'utente che ha effettuato la spedizione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SpeditoDa As CUser
            Get
                If (Me.m_SpeditoDa Is Nothing) Then Me.m_SpeditoDa = Sistema.Users.GetItemById(Me.m_IDSpeditoDa)
                Return Me.m_SpeditoDa
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.SpeditoDa
                If (oldValue Is value) Then Exit Property
                Me.m_SpeditoDa = value
                Me.m_IDSpeditoDa = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeSpeditoDa = value.Nominativo
                Me.DoChanged("SpeditoDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'utente che ha effettuato la spedizione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDSpeditoDa As Integer
            Get
                Return GetID(Me.m_SpeditoDa, Me.m_IDSpeditoDa)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDSpeditoDa
                If (oldValue = value) Then Exit Property
                Me.m_IDSpeditoDa = value
                Me.m_SpeditoDa = Nothing
                Me.DoChanged("IDSpeditoDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome dell'utente che ha effettuato la spedizione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeSpeditoDa As String
            Get
                Return Me.m_NomeSpeditoDa
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeSpeditoDa
                If (oldValue = value) Then Exit Property
                Me.m_NomeSpeditoDa = value
                Me.DoChanged("NomeSpeditoDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'utente che ha ricevuto la spedizione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property RicevutoDa As CUser
            Get
                If (Me.m_RicevutoDa Is Nothing) Then Me.m_RicevutoDa = Sistema.Users.GetItemById(Me.m_IDRicevutoDa)
                Return Me.m_RicevutoDa
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.RicevutoDa
                If (oldValue Is value) Then Exit Property
                Me.m_RicevutoDa = value
                Me.m_IDRicevutoDa = GetID(value)
                If (value IsNot Nothing) Then
                    Me.m_NomeRicevutoDa = value.Nominativo
                End If
                Me.DoChanged("RicevutoDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'utente che ha ricevuto la spedizione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDRicevutoDa As Integer
            Get
                Return GetID(Me.m_RicevutoDa, Me.m_IDRicevutoDa)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDRicevutoDa
                If (oldValue = value) Then Exit Property
                Me.m_IDRicevutoDa = value
                Me.m_RicevutoDa = Nothing
                Me.DoChanged("IDRicevutoDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome dell'utente che ha ricevuto la spedizione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeRicevutoDa As String
            Get
                Return Me.m_NomeRicevutoDa
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeRicevutoDa
                If (oldValue = value) Then Exit Property
                Me.m_NomeRicevutoDa = value
                Me.DoChanged("NomeRicevutoDa", value, oldValue)
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
        Public Property StatoSpedizione As StatoSpedizione
            Get
                Return Me.m_StatoSpedizione
            End Get
            Set(value As StatoSpedizione)
                Dim oldValue As StatoSpedizione = Me.m_StatoSpedizione
                If (oldValue = value) Then Exit Property
                Me.m_StatoSpedizione = value
                Me.DoChanged("StatoSpedizione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta lo stato della consegna
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StatoConsegna As StatoConsegna
            Get
                Return Me.m_StatoConsegna
            End Get
            Set(value As StatoConsegna)
                Dim oldValue As StatoConsegna = Me.m_StatoConsegna
                If (oldValue = value) Then Exit Property
                Me.m_StatoConsegna = value
                Me.DoChanged("StatoConsegna", value, oldValue)
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

        ''' <summary>
        ''' Restituisce o imposta dei flags
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Flags As SpedizioneFlags
            Get
                Return Me.m_Flags
            End Get
            Set(value As SpedizioneFlags)
                Dim oldValue As SpedizioneFlags = Me.m_Flags
                If (oldValue = value) Then Exit Property
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID del corriere utilizzato per la spedizione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDCorriere As Integer
            Get
                Return Me.m_IDCorriere
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDCorriere
                If (oldValue = value) Then Exit Property
                Me.m_IDCorriere = value
                Me.DoChanged("IDCorriere", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del corriere utilizzato per la spedizione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeCorriere As String
            Get
                Return Me.m_NomeCorriere
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeCorriere
                If (oldValue = value) Then Exit Property
                Me.m_NomeCorriere = value
                Me.DoChanged("NomeCorriere", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il numero della spedizione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NumeroSpedizione As String
            Get
                Return Me.m_NumeroSpedizione
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NumeroSpedizione
                If (oldValue = value) Then Exit Property
                Me.m_NumeroSpedizione = value
                Me.DoChanged("NumeroSpedizione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce la sequenza dei passaggi di stato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Passaggi As CCollection(Of PassaggioSpedizione)
            Get
                If (Me.m_Passaggi Is Nothing) Then Me.m_Passaggi = New CCollection(Of PassaggioSpedizione)
                Return Me.m_Passaggi
            End Get
        End Property

        ''' <summary>
        ''' Restituisce degli attributi aggiuntivi
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


        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Public Overrides Function GetModule() As Sistema.CModule
            Return Office.Spedizioni.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeSpedizioni"
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
            Me.m_IDMittente = reader.Read("IDMittente", Me.m_IDMittente)
            Me.m_NomeMittente = reader.Read("NomeMittente", Me.m_NomeMittente)
            With Me.m_IndirizzoMittente
                .Nome = reader.Read("IndirizzoMittente_Nome", .Nome)
                .ToponimoViaECivico = reader.Read("IndirizzoMittente_Via", .ToponimoViaECivico)
                .CAP = reader.Read("IndirizzoMittente_CAP", .CAP)
                .Citta = reader.Read("IndirizzoMittente_Citta", .Citta)
                .Provincia = reader.Read("IndirizzoMittente_Provincia", .Provincia)
                .SetChanged(False)
            End With

            Me.m_IDDestinatario = reader.Read("IDDestinatario", Me.m_IDDestinatario)
            Me.m_NomeDestinatario = reader.Read("NomeDestinatario", Me.m_NomeDestinatario)
            With Me.m_IndirizzoDestinatario
                .Nome = reader.Read("IndirizzoDest_Nome", .Nome)
                .ToponimoViaECivico = reader.Read("IndirizzoDest_Via", .ToponimoViaECivico)
                .CAP = reader.Read("IndirizzoDest_CAP", .CAP)
                .Citta = reader.Read("IndirizzoDest_Citta", .Citta)
                .Provincia = reader.Read("IndirizzoDest_Provincia", .Provincia)
                .SetChanged(False)
            End With

            Me.m_NumeroColli = reader.Read("NumeroColli", Me.m_NumeroColli)
            Me.m_Peso = reader.Read("Peso", Me.m_Peso)
            Me.m_Altezza = reader.Read("Altezza", Me.m_Altezza)
            Me.m_Larghezza = reader.Read("Larghezza", Me.m_Larghezza)
            Me.m_Profondita = reader.Read("Profondita", Me.m_Profondita)

            Me.m_IDSpeditoDa = reader.Read("IDSpeditoDa", Me.m_IDSpeditoDa)
            Me.m_NomeSpeditoDa = reader.Read("NomeSpeditoDa", Me.m_NomeSpeditoDa)

            Me.m_IDRicevutoDa = reader.Read("IDRicevutoDa", Me.m_IDRicevutoDa)
            Me.m_NomeRicevutoDa = reader.Read("NomeRicevutoDa", Me.m_NomeRicevutoDa)

            Me.m_DataInizioSpedizione = reader.Read("DataInizioSpedizione", Me.m_DataInizioSpedizione)

            Me.m_NotePerIlCorriere = reader.Read("NotePerIlCorriere", Me.m_NotePerIlCorriere)

            Me.m_NotePerIlDestinatario = reader.Read("NotePerIlDestinatario", Me.m_NotePerIlDestinatario)

            Me.m_StatoSpedizione = reader.Read("StatoSpedizione", Me.m_StatoSpedizione)
            Me.m_StatoConsegna = reader.Read("StatoConsegna", Me.m_StatoConsegna)

            Me.m_DataConsegna = reader.Read("DataConsegna", Me.m_DataConsegna)

            Me.m_Flags = reader.Read("Flags", Me.m_Flags)

            Me.m_NomeCorriere = reader.Read("NomeCorriere", Me.m_NomeCorriere)
            Me.m_IDCorriere = reader.Read("IDCorriere", Me.m_IDCorriere)
            Me.m_NumeroSpedizione = reader.Read("NumeroSpedizione", Me.m_NumeroSpedizione)

            Try
                Me.m_Passaggi = XML.Utils.Serializer.Deserialize(reader.Read("Passaggi", ""))
            Catch ex As Exception
                Me.m_Passaggi = New CCollection(Of PassaggioSpedizione)
            End Try
            Try
                Me.m_Attributi = XML.Utils.Serializer.Deserialize(reader.Read("Attributi", ""))
            Catch ex As Exception
                Me.m_Attributi = New CKeyCollection
            End Try

            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("AspettoBeni", Me.m_AspettoBeni)
            writer.Write("IDMittente", Me.IDMittente)
            writer.Write("NomeMittente", Me.m_NomeMittente)
            With Me.m_IndirizzoMittente
                writer.Write("IndirizzoMittente_Nome", .Nome)
                writer.Write("IndirizzoMittente_Via", .ToponimoViaECivico)
                writer.Write("IndirizzoMittente_CAP", .CAP)
                writer.Write("IndirizzoMittente_Citta", .Citta)
                writer.Write("IndirizzoMittente_Provincia", .Provincia)
            End With

            writer.Write("IDDestinatario", Me.IDDestinatario)
            writer.Write("NomeDestinatario", Me.m_NomeDestinatario)
            With Me.m_IndirizzoDestinatario
                writer.Write("IndirizzoDest_Nome", .Nome)
                writer.Write("IndirizzoDest_Via", .ToponimoViaECivico)
                writer.Write("IndirizzoDest_CAP", .CAP)
                writer.Write("IndirizzoDest_Citta", .Citta)
                writer.Write("IndirizzoDest_Provincia", .Provincia)
            End With

            writer.Write("NumeroColli", Me.m_NumeroColli)
            writer.Write("Peso", Me.m_Peso)
            writer.Write("Altezza", Me.m_Altezza)
            writer.Write("Larghezza", Me.m_Larghezza)
            writer.Write("Profondita", Me.m_Profondita)

            writer.Write("IDSpeditoDa", Me.IDSpeditoDa)
            writer.Write("NomeSpeditoDa", Me.m_NomeSpeditoDa)

            writer.Write("IDRicevutoDa", Me.IDRicevutoDa)
            writer.Write("NomeRicevutoDa", Me.m_NomeRicevutoDa)

            writer.Write("DataInizioSpedizione", Me.m_DataInizioSpedizione)

            writer.Write("NotePerIlCorriere", Me.m_NotePerIlCorriere)

            writer.Write("NotePerIlDestinatario", Me.m_NotePerIlDestinatario)

            writer.Write("StatoSpedizione", Me.m_StatoSpedizione)
            writer.Write("StatoConsegna", Me.m_StatoConsegna)

            writer.Write("DataConsegna", Me.m_DataConsegna)

            writer.Write("Flags", Me.m_Flags)

            writer.Write("NomeCorriere", Me.m_NomeCorriere)
            writer.Write("IDCorriere", Me.IDCorriere)
            writer.Write("NumeroSpedizione", Me.m_NumeroSpedizione)

            writer.Write("Passaggi", XML.Utils.Serializer.Serialize(Me.Passaggi))
            writer.Write("Attributi", XML.Utils.Serializer.Serialize(Me.Attributi))

            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("AspettoBeni", Me.m_AspettoBeni)
            writer.WriteAttribute("IDMittente", Me.IDMittente)
            writer.WriteAttribute("NomeMittente", Me.m_NomeMittente)
            writer.WriteAttribute("IDDestinatario", Me.IDDestinatario)
            writer.WriteAttribute("NomeDestinatario", Me.m_NomeDestinatario)
            writer.WriteAttribute("NumeroColli", Me.m_NumeroColli)
            writer.WriteAttribute("Peso", Me.m_Peso)
            writer.WriteAttribute("Altezza", Me.m_Altezza)
            writer.WriteAttribute("Larghezza", Me.m_Larghezza)
            writer.WriteAttribute("Profondita", Me.m_Profondita)
            writer.WriteAttribute("IDSpeditoDa", Me.IDSpeditoDa)
            writer.WriteAttribute("NomeSpeditoDa", Me.m_NomeSpeditoDa)
            writer.WriteAttribute("IDRicevutoDa", Me.IDRicevutoDa)
            writer.WriteAttribute("NomeRicevutoDa", Me.m_NomeRicevutoDa)
            writer.WriteAttribute("DataInizioSpedizione", Me.m_DataInizioSpedizione)
            writer.WriteAttribute("StatoSpedizione", Me.m_StatoSpedizione)
            writer.WriteAttribute("StatoConsegna", Me.m_StatoConsegna)
            writer.WriteAttribute("DataConsegna", Me.m_DataConsegna)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("NomeCorriere", Me.m_NomeCorriere)
            writer.WriteAttribute("IDCorriere", Me.m_IDCorriere)
            writer.WriteAttribute("NumeroSpedizione", Me.m_NumeroSpedizione)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("IndirizzoMittente", Me.m_IndirizzoMittente)
            writer.WriteTag("IndirizzoDestinatario", Me.m_IndirizzoDestinatario)
            writer.WriteTag("Passaggi", Me.Passaggi)
            writer.WriteTag("Attributi", Me.Attributi)
            writer.WriteTag("NotePerIlCorriere", Me.m_NotePerIlCorriere)
            writer.WriteTag("NotePerIlDestinatario", Me.m_NotePerIlDestinatario)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "AspettoBeni" : Me.m_AspettoBeni = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDMittente" : Me.m_IDMittente = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeMittente" : Me.m_NomeMittente = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDDestinatario" : Me.m_IDDestinatario = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeDestinatario" : Me.m_NomeDestinatario = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "NumeroColli" : Me.m_NumeroColli = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Peso" : Me.m_Peso = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Altezza" : Me.m_Altezza = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Larghezza" : Me.m_Larghezza = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Profondita" : Me.m_Profondita = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "IDSpeditoDa" : Me.m_IDSpeditoDa = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeSpeditoDa" : Me.m_NomeSpeditoDa = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDRicevutoDa" : Me.m_IDRicevutoDa = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeRicevutoDa" : Me.m_NomeRicevutoDa = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataInizioSpedizione" : Me.m_DataInizioSpedizione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "StatoSpedizione" : Me.m_StatoSpedizione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "StatoConsegna" : Me.m_StatoConsegna = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "DataConsegna" : Me.m_DataConsegna = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeCorriere" : Me.m_NomeCorriere = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDCorriere" : Me.m_IDCorriere = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NumeroSpedizione" : Me.m_NumeroSpedizione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IndirizzoMittente" : Me.m_IndirizzoMittente = fieldValue
                Case "IndirizzoDestinatario" : Me.m_IndirizzoDestinatario = fieldValue
                Case "Passaggi" : Me.Passaggi.Clear() : Me.Passaggi.AddRange(fieldValue)
                Case "Attributi" : Me.m_Attributi = fieldValue
                Case "NotePerIlCorriere" : Me.m_NotePerIlCorriere = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "NotePerIlDestinatario" : Me.m_NotePerIlDestinatario = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select

        End Sub

        Public Function CompareTo(ByVal b As Spedizione) As Integer
            Return DateUtils.Compare(Me.m_DataInizioSpedizione, b.m_DataInizioSpedizione)
        End Function

        Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(obj)
        End Function

        Public Overrides Function ToString() As String
            Return "Spedizione " & Me.m_NomeCorriere & " del " & Formats.FormatUserDateTime(Me.m_DataInizioSpedizione)
        End Function
    End Class



End Class
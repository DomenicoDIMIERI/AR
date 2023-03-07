Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica

Partial Class Office

    ''' <summary>
    ''' Stati di una commissione
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum StatoCommissione As Integer
        ''' <summary>
        ''' Non iniziata
        ''' </summary>
        ''' <remarks></remarks>
        NonIniziata = 0

        ''' <summary>
        ''' Iniziata 
        ''' </summary>
        ''' <remarks></remarks>
        Iniziata = 1

        ''' <summary>
        ''' Rimandata
        ''' </summary>
        ''' <remarks></remarks>
        Rimandata = 2

        ''' <summary>
        ''' Completata
        ''' </summary>
        ''' <remarks></remarks>
        Completata = 3

        ''' <summary>
        ''' Annullata
        ''' </summary>
        ''' <remarks></remarks>
        Annullata = 4
    End Enum

    Public Enum CommissioneFlags As Integer
        None = 0

        GiornataIntera = 1
    End Enum

    ''' <summary>
    ''' Rappresenta una commissione fatta da un operatore 
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class Commissione
        Inherits DBObjectPO

        Private m_Operatore As CUser                    'Operatore che ha svolto la commissione
        Private m_IDOperatore As Integer                'ID dell'operatore che ha svolto la commissione
        Private m_NomeOperatore As String               'Nome dell'operatore che ha svolto la commissione
        Private m_DataPrevista As Nullable(Of Date)     'Data prevista
        Private m_OraUscita As Nullable(Of Date)        'Data ed ora di uscita (per svolgere la commissione)
        Private m_OraRientro As Nullable(Of Date)       'Data ed ora di rientro
        Private m_Motivo As String                      'Motivo della commissione
        Private m_Presso As String                      'Nome del luogo presso cui svolgere la commissione (domicilio, residenza, sede lavoro)...
        Private m_Azienda As CAzienda                   'Azienda presso cui si è recato l'operatore
        Private m_IDAzienda As Integer                  'ID dell'azienda presso cui si è recato l'operatore
        Private m_NomeAzienda As String                 'Nome dell'azienda presso cui si è recato l'operatore
        Private m_PersonaIncontrata As CPersonaFisica   'Persona fisica incontrata
        Private m_IDPersonaIncontrata As Integer        'ID della persona fisica incontrata
        Private m_NomePersonaIncontrata As String 'Nome della persona fisica incontrata
        Private m_Esito As String                       'Descrizione estesa della commissione
        Private m_Scadenzario As Nullable(Of Date)      'Data per un eventuale promemoria
        Private m_NoteScadenzario As String             'Note per un eventuale promemoria
        Private m_StatoCommissione As StatoCommissione  'Stato della commissione
        Private m_IDRichiesta As Integer                'ID della richiesta che ha generato la commissione
        Private m_Richiesta As RichiestaCERQ            'Richiesta che ha generato la commissione
        Private m_DistanzaPercorsa As Nullable(Of Double)   'Distanza percorsa
        Private m_Uscite As UscitePerCommissioneCollection
        Private m_ContextID As Integer
        Private m_ContextType As String
        Private m_OldStatoCommissione As StatoCommissione
        Private m_IDAssegnataDa As Integer
        Private m_AssegnataDa As CUser
        Private m_NomeAssegnataDa As String
        Private m_AssegnataIl As Nullable(Of Date)
        Private m_IDAssegnataA As Integer
        Private m_AssegnataA As CUser
        Private m_NomeAssegnataA As String
        Private m_Flags As CommissioneFlags
        Private m_Source As Object
        Private m_SourceType As String
        Private m_SourceID As Integer
        Private m_Luoghi As CCollection(Of LuogoDaVisitare)
        Private m_ParametriCommissione As String

        Public Sub New()
            Me.m_Operatore = Nothing
            Me.m_IDOperatore = 0
            Me.m_NomeOperatore = vbNullString
            Me.m_DataPrevista = Nothing
            Me.m_OraUscita = Nothing
            Me.m_OraRientro = Nothing
            Me.m_Motivo = vbNullString
            ' Me.m_Luogo = vbNullString
            Me.m_Azienda = Nothing
            Me.m_IDAzienda = 0
            Me.m_NomeAzienda = vbNullString
            Me.m_PersonaIncontrata = Nothing
            Me.m_IDPersonaIncontrata = 0
            Me.m_NomePersonaIncontrata = vbNullString
            Me.m_Esito = vbNullString
            Me.m_Scadenzario = Nothing
            Me.m_NoteScadenzario = vbNullString
            Me.m_StatoCommissione = StatoCommissione.NonIniziata
            Me.m_IDRichiesta = 0
            Me.m_Richiesta = Nothing
            Me.m_DistanzaPercorsa = Nothing
            Me.m_Uscite = Nothing
            Me.m_IDAssegnataDa = 0
            Me.m_AssegnataDa = Nothing
            Me.m_NomeAssegnataDa = vbNullString
            Me.m_AssegnataIl = Nothing
            Me.m_ContextID = 0
            Me.m_ContextType = vbNullString
            Me.m_OldStatoCommissione = Me.m_StatoCommissione
            Me.m_IDAssegnataA = 0
            Me.m_AssegnataA = Nothing
            Me.m_NomeAssegnataA = ""
            Me.m_Flags = CommissioneFlags.None
            Me.m_Source = Nothing
            Me.m_SourceID = 0
            Me.m_SourceType = ""
            Me.m_Luoghi = New CCollection(Of LuogoDaVisitare)
            Me.m_ParametriCommissione = ""
        End Sub

        Public Property ParametriCommissione As String
            Get
                Return Me.m_ParametriCommissione
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_ParametriCommissione
                If (oldValue = value) Then Exit Property
                Me.m_ParametriCommissione = value
                Me.DoChanged("ParametriCommissione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o importa l'oggetto che ha creato la commissione (la commissione è associata in relazione 1 a molti a questo oggetto)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Source As Object
            Get
                If (Me.m_Source Is Nothing AndAlso Me.m_SourceType <> "" AndAlso Me.m_SourceID <> 0) Then Me.m_Source = Sistema.Types.GetItemByTypeAndId(Me.m_SourceType, Me.m_SourceID)
                Return Me.m_Source
            End Get
            Set(value As Object)
                Dim oldValue As Object = Me.Source
                If (oldValue Is value) Then Exit Property
                Me.m_Source = value
                Me.m_SourceType = TypeName(value)
                Me.m_SourceID = GetID(value)
                Me.DoChanged("Source", value, oldValue)
            End Set
        End Property

        Public Property SourceType As String
            Get
                If (Me.m_Source IsNot Nothing) Then Return TypeName(Me.m_Source)
                Return Me.m_SourceType
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.SourceType
                If (oldValue = value) Then Exit Property
                Me.m_SourceType = value
                Me.m_Source = Nothing
                Me.DoChanged("SourceType", value, oldValue)
            End Set
        End Property

        Public Property SourceID As Integer
            Get
                Return GetID(Me.m_Source, Me.m_SourceID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.SourceID
                If (oldValue = value) Then Exit Property
                Me.m_SourceID = value
                Me.m_Source = Nothing
                Me.DoChanged("SourceID", value, oldValue)
            End Set
        End Property


        ''' <summary>
        ''' Restituisce o imposta i flags per la commissione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Flags As CommissioneFlags
            Get
                Return Me.m_Flags
            End Get
            Set(value As CommissioneFlags)
                Dim oldValue As CommissioneFlags = Me.m_Flags
                If (oldValue = value) Then Exit Property
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se la commissione non specifica un orario per essere eseguita
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property GiornataIntera As Boolean
            Get
                Return TestFlag(Me.m_Flags, CommissioneFlags.GiornataIntera)
            End Get
            Set(value As Boolean)
                If (Me.GiornataIntera = value) Then Exit Property
                Me.m_Flags = SetFlag(Me.m_Flags, CommissioneFlags.GiornataIntera, value)
                Me.DoChanged("GiornataIntera", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID del contesto in cui è stata creata la commissione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ContextID As Integer
            Get
                Return Me.m_ContextID
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_ContextID
                If (oldValue = value) Then Exit Property
                Me.m_ContextID = value
                Me.DoChanged("ContextID", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il tipo del contesto in cui è stata creata la commissione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ContextType As String
            Get
                Return Me.m_ContextType
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_ContextType
                If (oldValue = value) Then Exit Property
                Me.m_ContextType = value
                Me.DoChanged("ContextType", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce l'elenco delle uscite in cui è stata "iniziata" questa commissione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Uscite As UscitePerCommissioneCollection
            Get
                If (Me.m_Uscite Is Nothing) Then Me.m_Uscite = New UscitePerCommissioneCollection(Me)
                Return Me.m_Uscite
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta lo stato della commissione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StatoCommissione As StatoCommissione
            Get
                Return Me.m_StatoCommissione
            End Get
            Set(value As StatoCommissione)
                Dim oldValue As StatoCommissione = Me.m_StatoCommissione
                If (oldValue = value) Then Exit Property
                Me.m_StatoCommissione = value
                Me.DoChanged("StatoCommissione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'operatore che ha effettuato la commissione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Operatore As CUser
            Get
                If (Me.m_Operatore Is Nothing) Then Me.m_Operatore = Users.GetItemById(Me.m_IDOperatore)
                Return Me.m_Operatore
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.m_Operatore
                Me.m_Operatore = value
                Me.m_IDOperatore = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeOperatore = value.Nominativo
                Me.DoChanged("Operatore", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'operatore che ha effettuato la commissione
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
        ''' Restituisce o imposta il nome dell'operatore che ha effettuate la commissione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeOperatore As String
            Get
                Return Me.m_NomeOperatore
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomeOperatore
                If (oldValue = value) Then Exit Property
                Me.m_NomeOperatore = value
                Me.DoChanged("NomeOperatore", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data prevista per la commissione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataPrevista As Nullable(Of Date)
            Get
                Return Me.m_DataPrevista
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataPrevista
                If (oldValue = value) Then Exit Property
                Me.m_DataPrevista = value
                Me.DoChanged("DataPrevista", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data e l'ora di uscita per la commissione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property OraUscita As Nullable(Of Date)
            Get
                Return Me.m_OraUscita
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_OraUscita
                If (oldValue = value) Then Exit Property
                Me.m_OraUscita = value
                Me.DoChanged("OraUscita", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la durata in secondi (differenza tra ora ingresso ed ora uscita)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Durata As Nullable(Of Integer)
            Get
                If (Me.m_OraUscita.HasValue AndAlso Me.m_OraRientro.HasValue) Then
                    Return Math.Abs(DateUtils.DateDiff("s", Me.m_OraRientro.Value, Me.m_OraUscita.Value))
                Else
                    Return Nothing
                End If
            End Get
        End Property
        ''' <summary>
        ''' Restituisce o imposta la data di rientro
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property OraRientro As Nullable(Of Date)
            Get
                Return Me.m_OraRientro
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_OraRientro
                If (oldValue = value) Then Exit Property
                Me.m_OraRientro = value
                Me.DoChanged("OraRientro", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il motivo della commissione (descrizione breve)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Motivo As String
            Get
                Return Me.m_Motivo
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Motivo
                If (oldValue = value) Then Exit Property
                Me.m_Motivo = value
                Me.DoChanged("Motivo", value, oldValue)
            End Set
        End Property

        ' ''' <summary>
        ' ''' Restituisce o imposta il luogo di destinazione dell'operatore
        ' ''' </summary>
        ' ''' <value></value>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Property Luogo As String
        '    Get
        '        Return Me.m_Luogo
        '    End Get
        '    Set(value As String)
        '        value = Trim(value)
        '        Dim oldValue As String = Me.m_Luogo
        '        If (oldValue = value) Then Exit Property
        '        Me.m_Luogo = value
        '        Me.DoChanged("Luogo", value, oldValue)
        '    End Set
        'End Property

        Public ReadOnly Property Luoghi As CCollection(Of LuogoDaVisitare)
            Get
                Return Me.m_Luoghi
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'azienda presso cui si è recato l'operatore
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
                Dim oldValue As CAzienda = Me.Azienda
                If (oldValue Is value) Then Exit Property
                Me.m_Azienda = value
                Me.m_IDAzienda = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeAzienda = value.Nominativo
                Me.DoChanged("Azienda", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'azienda presso cui si è recato l'operatore
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
        ''' Restituisce o imposta il nome dell'azienda presso cui si è recato l'operatore
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeAzienda As String
            Get
                Return Me.m_NomeAzienda
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomeAzienda
                If (oldValue = value) Then Exit Property
                Me.m_NomeAzienda = value
                Me.DoChanged("NomeAzienda", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la persona fisica che si è incontrato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PersonaIncontrata As CPersonaFisica
            Get
                If (Me.m_PersonaIncontrata Is Nothing) Then Me.m_PersonaIncontrata = Anagrafica.Persone.GetItemById(Me.m_IDPersonaIncontrata)
                Return Me.m_PersonaIncontrata
            End Get
            Set(value As CPersonaFisica)
                Dim oldValue As CPersonaFisica = Me.PersonaIncontrata
                If (oldValue Is value) Then Exit Property
                Me.m_PersonaIncontrata = value
                Me.m_IDPersonaIncontrata = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomePersonaIncontrata = value.Nominativo
                Me.DoChanged("PersonaIncontrata", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID della persona fisica incontrata
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDPersonaIncontrata As Integer
            Get
                Return GetID(Me.m_PersonaIncontrata, Me.m_IDPersonaIncontrata)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDPersonaIncontrata
                If (oldValue = value) Then Exit Property
                Me.m_IDPersonaIncontrata = value
                Me.m_PersonaIncontrata = Nothing
                Me.DoChanged("IDPersonaIncontrata", value, oldValue)
            End Set
        End Property

        Public Property NomePersonaIncontrata As String
            Get
                Return Me.m_NomePersonaIncontrata
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomePersonaIncontrata
                value = Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_NomePersonaIncontrata = value
                Me.DoChanged("NomePersonaIncontrata", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'esito dell'incontro
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Esito As String
            Get
                Return Me.m_Esito
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Esito
                If (oldValue = value) Then Exit Property
                Me.m_Esito = value
                Me.DoChanged("Esito", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un eventuale data per il promemoria
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Scadenzario As Nullable(Of Date)
            Get
                Return Me.m_Scadenzario
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_Scadenzario
                If (oldValue = value) Then Exit Property
                Me.m_Scadenzario = value
                Me.DoChanged("Scadenzario", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta delle note per l'eventuale promemoria
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NoteScadenzario As String
            Get
                Return Me.m_NoteScadenzario
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NoteScadenzario
                If (oldValue = value) Then Exit Property
                Me.m_NoteScadenzario = value
                Me.DoChanged("NoteScadenzario", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID della richiesta associata
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDRichiesta As Integer
            Get
                Return GetID(Me.m_Richiesta, Me.m_IDRichiesta)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_IDRichiesta
                If (oldValue = value) Then Exit Property
                Me.m_IDRichiesta = value
                Me.DoChanged("IDRichiesta", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la richiesta associata
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Richiesta As RichiestaCERQ
            Get
                If (Me.m_Richiesta Is Nothing) Then Me.m_Richiesta = RichiesteCERQ.GetItemById(Me.m_IDRichiesta)
                Return Me.m_Richiesta
            End Get
            Set(value As RichiestaCERQ)
                Dim oldValue As RichiestaCERQ = Me.Richiesta
                If (oldValue Is value) Then Exit Property
                Me.m_Richiesta = value
                Me.m_IDRichiesta = GetID(value)
                Me.DoChanged("Richiesta", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la distanza percorsa
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DistanzaPercorsa As Nullable(Of Double)
            Get
                Return Me.m_DistanzaPercorsa
            End Get
            Set(value As Nullable(Of Double))
                Dim oldValue As Nullable(Of Double) = Me.m_DistanzaPercorsa
                If (oldValue = value) Then Exit Property
                Me.m_DistanzaPercorsa = value
                Me.DoChanged("DistanzaPercorsa", value, oldValue)
            End Set
        End Property

        Public Property IDAssegnataDa As Integer
            Get
                Return GetID(Me.m_AssegnataDa, Me.m_IDAssegnataDa)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDAssegnataDa
                If (oldValue = value) Then Exit Property
                Me.m_IDAssegnataDa = value
                Me.m_AssegnataDa = Nothing
                Me.DoChanged("IDAssegnataDa", value, oldValue)
            End Set
        End Property

        Public Property AssegnataDa As CUser
            Get
                If (Me.m_AssegnataDa Is Nothing) Then Me.m_AssegnataDa = Sistema.Users.GetItemById(Me.m_IDAssegnataDa)
                Return Me.m_AssegnataDa
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.m_AssegnataDa
                If (oldValue Is value) Then Exit Property
                Me.m_AssegnataDa = value
                Me.m_IDAssegnataDa = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeAssegnataDa = value.Nominativo
                Me.DoChanged("AssegnataDa", value, oldValue)
            End Set
        End Property

        Public Property NomeAssegnataDa As String
            Get
                Return Me.m_NomeAssegnataDa
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeAssegnataDa
                If (oldValue = value) Then Exit Property
                Me.m_NomeAssegnataDa = value
                Me.DoChanged("NomeAssegnataDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restitusice o imposta la data in cui è stata programmata la commissione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AssegnataIl As Nullable(Of Date)
            Get
                Return Me.m_AssegnataIl
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_AssegnataIl
                If (oldValue = value) Then Exit Property
                Me.m_AssegnataIl = value
                Me.DoChanged("AssegnataIl", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'utente a cui è stata assegnata la commissione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDAssegnataA As Integer
            Get
                Return GetID(Me.m_AssegnataA, Me.m_IDAssegnataA)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDAssegnataA
                If (oldValue = value) Then Exit Property
                Me.m_IDAssegnataA = value
                Me.m_AssegnataA = Nothing
                Me.DoChanged("IDAssegnataA", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'utente a cui è stata assegnata la commissione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AssegnataA As CUser
            Get
                If (Me.m_AssegnataA Is Nothing) Then Me.m_AssegnataA = Sistema.Users.GetItemById(Me.m_IDAssegnataA)
                Return Me.m_AssegnataA
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.m_AssegnataA
                If (oldValue Is value) Then Exit Property
                Me.m_AssegnataA = value
                Me.m_IDAssegnataA = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeAssegnataA = value.Nominativo
                Me.DoChanged("AssegnataA", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome dell'utente a cui è stata assegnata la commissione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeAssegnataA As String
            Get
                Return Me.m_NomeAssegnataA
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeAssegnataA
                If (oldValue = value) Then Exit Property
                Me.m_NomeAssegnataA = value
                Me.DoChanged("NomeAssegnataA", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del luogo presso cui svolgere la commissione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Presso As String
            Get
                Return Me.m_Presso
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Presso
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_Presso = value
                Me.DoChanged("Presso", value, oldValue)
            End Set
        End Property

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Public Overrides Function GetModule() As Sistema.CModule
            Return Commissioni.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeCommissioni"
        End Function

        Protected Overrides Function SaveToDatabase(dbConn As CDBConnection, force As Boolean) As Boolean
            Dim ret As Boolean = MyBase.SaveToDatabase(dbConn, force)
            If (ret) Then
                If (Me.m_OldStatoCommissione <> Me.m_StatoCommissione) Then Commissioni.doNotificaStatoCommissione(New CommissioneEventArgs(Me))
                Me.m_OldStatoCommissione = Me.m_StatoCommissione
            End If
            Return ret
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_StatoCommissione = reader.Read("StatoCommissione", Me.m_StatoCommissione)
            Me.m_OldStatoCommissione = Me.m_StatoCommissione
            Me.m_IDOperatore = reader.Read("IDOperatore", Me.m_IDOperatore)
            Me.m_NomeOperatore = reader.Read("NomeOperatore", Me.m_NomeOperatore)
            Me.m_DataPrevista = reader.Read("DataPrevista", Me.m_DataPrevista)
            Me.m_OraUscita = reader.Read("OraUscita", Me.m_OraUscita)
            Me.m_OraRientro = reader.Read("OraRientro", Me.m_OraRientro)
            Me.m_Motivo = reader.Read("Motivo", Me.m_Motivo)
            Me.m_Luoghi.Clear()
            Dim luogo As String = ""
            luogo = reader.Read("Luogo", luogo)
            Try
                Me.m_Luoghi.AddRange(XML.Utils.Serializer.Deserialize(luogo))
            Catch ex As Exception
                Dim tmp As New LuogoDaVisitare
                tmp.Indirizzo.Parse(luogo)
                Me.m_Luoghi.Add(tmp)
            End Try
            Me.m_IDAzienda = reader.Read("IDAzienda", Me.m_IDAzienda)
            Me.m_NomeAzienda = reader.Read("NomeAzienda", Me.m_NomeAzienda)
            Me.m_IDPersonaIncontrata = reader.Read("IDPersona", Me.m_IDPersonaIncontrata)
            Me.m_NomePersonaIncontrata = reader.Read("NomePersona", Me.m_NomePersonaIncontrata)
            Me.m_Esito = reader.Read("Esito", Me.m_Esito)
            Me.m_Scadenzario = reader.Read("Scadenzario", Me.m_Scadenzario)
            Me.m_NoteScadenzario = reader.Read("NoteScadenzario", Me.m_NoteScadenzario)
            Me.m_IDRichiesta = reader.Read("IDRichiesta", Me.m_IDRichiesta)
            Me.m_DistanzaPercorsa = reader.Read("DistanzaPercorsa", Me.m_DistanzaPercorsa)
            Me.m_IDAssegnataDa = reader.Read("IDAssegnataDa", Me.m_IDAssegnataDa)
            Me.m_NomeAssegnataDa = reader.Read("NomeAssegnataDa", Me.m_NomeAssegnataDa)
            Me.m_AssegnataIl = reader.Read("AssegnataIl", Me.m_AssegnataIl)
            Me.m_IDAssegnataA = reader.Read("IDAssegnataA", Me.m_IDAssegnataA)
            Me.m_NomeAssegnataA = reader.Read("NomeAssegnataA", Me.m_NomeAssegnataA)
            Me.m_ContextID = reader.Read("ContextID", Me.m_ContextID)
            Me.m_ContextType = reader.Read("ContextType", Me.m_ContextType)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_SourceType = reader.Read("SourceType", Me.m_SourceType)
            Me.m_SourceID = reader.Read("SourceID", Me.m_SourceID)
            Me.m_Presso = reader.Read("Presso", Me.m_Presso)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("StatoCommissione", Me.m_StatoCommissione)
            writer.Write("IDOperatore", Me.IDOperatore)
            writer.Write("NomeOperatore", Me.m_NomeOperatore)
            writer.Write("DataPrevista", Me.m_DataPrevista)
            writer.Write("OraUscita", Me.m_OraUscita)
            writer.Write("OraRientro", Me.m_OraRientro)
            writer.Write("Motivo", Me.m_Motivo)
            writer.Write("Luogo", XML.Utils.Serializer.Serialize(Me.Luoghi))
            writer.Write("IDAzienda", Me.IDAzienda)
            writer.Write("NomeAzienda", Me.m_NomeAzienda)
            writer.Write("IDPersona", Me.IDPersonaIncontrata)
            writer.Write("NomePersona", Me.m_NomePersonaIncontrata)
            writer.Write("Esito", Me.m_Esito)
            writer.Write("Scadenzario", Me.m_Scadenzario)
            writer.Write("NoteScadenzario", Me.m_NoteScadenzario)
            writer.Write("IDRichiesta", Me.IDRichiesta)
            writer.Write("DistanzaPercorsa", Me.m_DistanzaPercorsa)
            writer.Write("IDAssegnataDa", Me.IDAssegnataDa)
            writer.Write("NomeAssegnataDa", Me.m_NomeAssegnataDa)
            writer.Write("AssegnataIl", Me.m_AssegnataIl)
            writer.Write("IDAssegnataA", Me.IDAssegnataA)
            writer.Write("NomeAssegnataA", Me.m_NomeAssegnataA)
            writer.Write("ContextID", Me.m_ContextID)
            writer.Write("ContextType", Me.m_ContextType)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("SourceType", Me.SourceType)
            writer.Write("SourceID", Me.SourceID)
            writer.Write("Presso", Me.m_Presso)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("StatoCommissione", Me.m_StatoCommissione)
            writer.WriteAttribute("OldStatoCommissione", Me.m_OldStatoCommissione)
            writer.WriteAttribute("IDOperatore", Me.IDOperatore)
            writer.WriteAttribute("NomeOperatore", Me.m_NomeOperatore)
            writer.WriteAttribute("DataPrevista", Me.m_DataPrevista)
            writer.WriteAttribute("OraUscita", Me.m_OraUscita)
            writer.WriteAttribute("OraRientro", Me.m_OraRientro)
            writer.WriteAttribute("Motivo", Me.m_Motivo)
            writer.WriteAttribute("IDAzienda", Me.IDAzienda)
            writer.WriteAttribute("NomeAzienda", Me.m_NomeAzienda)
            writer.WriteAttribute("IDPersona", Me.IDPersonaIncontrata)
            writer.WriteAttribute("NomePersona", Me.m_NomePersonaIncontrata)
            writer.WriteAttribute("Esito", Me.m_Esito)
            writer.WriteAttribute("Scadenzario", Me.m_Scadenzario)
            writer.WriteAttribute("NoteScadenzario", Me.m_NoteScadenzario)
            writer.WriteAttribute("IDRichiesta", Me.IDRichiesta)
            writer.WriteAttribute("DistanzaPercorsa", Me.m_DistanzaPercorsa)
            writer.WriteAttribute("IDAssegnataDa", Me.IDAssegnataDa)
            writer.WriteAttribute("NomeAssegnataDa", Me.m_NomeAssegnataDa)
            writer.WriteAttribute("AssegnataIl", Me.m_AssegnataIl)
            writer.WriteAttribute("IDAssegnataA", Me.IDAssegnataA)
            writer.WriteAttribute("NomeAssegnataA", Me.m_NomeAssegnataA)
            writer.WriteAttribute("ContextID", Me.m_ContextID)
            writer.WriteAttribute("ContextType", Me.m_ContextType)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("SourceType", Me.SourceType)
            writer.WriteAttribute("SourceID", Me.SourceID)
            writer.WriteAttribute("Presso", Me.m_Presso)
            MyBase.XMLSerialize(writer)
            writer.Settings.SetValueBool("commissioneserialization", True)
            writer.WriteTag("Luoghi", Me.Luoghi)
            writer.WriteTag("Uscite", Me.Uscite)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "StatoCommissione" : Me.m_StatoCommissione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "OldStatoCommissione" : Me.m_OldStatoCommissione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDOperatore" : Me.m_IDOperatore = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeOperatore" : Me.m_NomeOperatore = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataPrevista" : Me.m_DataPrevista = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "OraUscita" : Me.m_OraUscita = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "OraRientro" : Me.m_OraRientro = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Motivo" : Me.m_Motivo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Luoghi" : Me.m_Luoghi.Clear() : Me.m_Luoghi.AddRange(fieldValue)
                Case "IDAzienda" : Me.m_IDAzienda = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeAzienda" : Me.m_NomeAzienda = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDPersona" : Me.m_IDPersonaIncontrata = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomePersona" : Me.m_NomePersonaIncontrata = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Esito" : Me.m_Esito = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Scadenzario" : Me.m_Scadenzario = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "NoteScadenzario" : Me.m_NoteScadenzario = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDRichiesta" : Me.m_IDRichiesta = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "DistanzaPercorsa" : Me.m_DistanzaPercorsa = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "NomeAssegnataDa" : Me.m_NomeAssegnataDa = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDAssegnataDa" : Me.m_IDAssegnataDa = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeAssegnataA" : Me.m_NomeAssegnataA = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDAssegnataA" : Me.m_IDAssegnataA = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "AssegnataIl" : Me.m_AssegnataIl = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "ContextID" : Me.m_ContextID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "ContextType" : Me.m_ContextType = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Uscite" : Me.m_Uscite = fieldValue : Me.m_Uscite.SetCommissione(Me)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "SourceType" : Me.m_SourceType = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "SourceID" : Me.m_SourceID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Presso" : Me.m_Presso = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select

        End Sub

        Public Overrides Function ToString() As String
            Dim ret As New System.Text.StringBuilder
            ret.Append(Me.m_Motivo)
            If (Me.m_NomePersonaIncontrata <> "") Then ret.Append(" per " & Me.m_NomePersonaIncontrata)
            If (Me.m_NomeAzienda <> "") Then ret.Append(" presso la " & Me.m_NomeAzienda)
            'If (Me.m_Luogo <> "") Then ret.Append(" in " & Me.m_Luogo)
            Return ret.ToString
        End Function

        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            MyBase.OnAfterSave(e)
            ' Me.ProgrammaNotifica()
        End Sub

        'Public Sub ProgramAlert()
        '    If ((Me.Stato <> ObjectStatus.OBJECT_VALID) OrElse (Me.StatoCommissione <> Office.StatoCommissione.NonIniziata) OrElse (Me.DataPrevista.HasValue = False)) Then
        '        Sistema.Notifiche.CancelPendingAlertsBySource(Nothing, Me)
        '    Else
        '        Dim alerts As CCollection(Of Notifica) = Sistema.Notifiche.GetPendingAlertsBySource(Me)
        '        Dim alert As Notifica = Nothing
        '        If (alerts.Count > 0) Then alert = alerts(0)
        '        Dim text As String = "Commissione programmata da " & Me.NomeCreatoDa
        '        text &= " per il " & Formats.FormatUserDateTime(Me.DataPrevista) & ":" & vbNewLine
        '        text &= Me.ToString

        '        If (alert Is Nothing) Then
        '            alert = Sistema.Notifiche.ProgramAlert(Me.Operatore, text, Me.DataPrevista, Me)
        '        Else
        '            alert.Target = Me.Operatore
        '            alert.Data = Me.DataPrevista
        '            alert.Descrizione = text
        '            alert.Save()
        '        End If

        '    End If
        'End Sub

        'Protected Overridable Sub ProgrammaNotifica()
        '    If (Me.Stato = ObjectStatus.OBJECT_VALID AndAlso (Me.StatoCommissione = Office.StatoCommissione.NonIniziata OrElse Me.StatoCommissione = Office.StatoCommissione.Rimandata)) Then
        '        Dim notifiche As CCollection(Of Notifica) = Sistema.Notifiche.GetAlertsBySource(Me, "")
        '        Dim notifica As Notifica
        '        Dim txt As String = ""
        '        txt = "Commissione assegnata da " & Me.CreatoDa.Nominativo & " il " & Formats.FormatUserDateTime(Me.CreatoIl) & vbNewLine
        '        txt &= "Motivo: " & Me.Motivo & vbNewLine
        '        If (Me.NomePersonaIncontrata <> "") Then txt &= "Cliente: " & Me.NomePersonaIncontrata & vbNewLine
        '        If (Me.NomeAzienda <> "") Then txt &= "Presso: " & Me.NomeAzienda & vbNewLine
        '        If (Me.Luogo <> "") Then txt &= "In : " & Me.Luogo
        '        Dim data As Date = DateUtils.Now
        '        If (Me.DataPrevista.HasValue) Then data = DateUtils.DateAdd(DateInterval.Minute, -30, Me.DataPrevista)
        '        If (notifiche.Count > 0) Then
        '            notifica = notifiche(0)
        '            If (notifica.StatoNotifica <= StatoNotifica.CONSEGNATA) Then
        '                notifica.Target = Me.Operatore
        '                notifica.Data = data
        '                notifica.Save()
        '            End If
        '        Else
        '            Dim target As CUser = Nothing
        '            Select Case Me.StatoCommissione
        '                Case Office.StatoCommissione.Annullata
        '                    target = Me.AssegnataDa
        '                Case Office.StatoCommissione.Completata
        '                    target = Me.AssegnataDa
        '                Case Office.StatoCommissione.Iniziata
        '                    target = Me.AssegnataDa
        '                Case Office.StatoCommissione.NonIniziata
        '                    target = Me.AssegnataA
        '                Case Office.StatoCommissione.Rimandata
        '                    target = Me.AssegnataDa
        '            End Select


        '            If (target IsNot Nothing) Then notifica = Sistema.Notifiche.ProgramAlert(target, txt, data, Me, "Commissione Programmata")
        '        End If
        '    Else
        '        Sistema.Notifiche.CancelPendingAlertsBySource(Nothing, Me, "Commissione Programmata")
        '    End If
        'End Sub


        Protected Overrides Sub OnCreate(e As SystemEvent)
            MyBase.OnCreate(e)
            Office.Commissioni.doItemCreated(New ItemEventArgs(Me))
        End Sub

        Protected Overrides Sub OnDelete(e As SystemEvent)
            MyBase.OnDelete(e)
            Office.Commissioni.doItemDeleted(New ItemEventArgs(Me))
        End Sub

        Protected Overrides Sub OnModified(e As SystemEvent)
            MyBase.OnModified(e)
            Office.Commissioni.doItemModified(New ItemEventArgs(Me))
        End Sub
    End Class



End Class
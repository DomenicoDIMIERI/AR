Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica
     
 
    ''' <summary>
    ''' Rappresenta una richiesta di contatto fatta da un sito esterno.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CContattoEsterno
        Inherits DBObjectPO

        Private m_Nome As String            'Nome del contatto
        Private m_Cognome As String         'Cognome del contatto
        Private m_EMail As String
        Private m_Telefono As String
        Private m_Professione As String
        Private m_IDFonte As Integer
        Private m_Fonte As CFonte
        Private m_NomeFonte As String
        Private m_Riferimento As String     'Pagina "Referrer" della pagina che ha generato la richiesta
        Private m_RemoteIP As String        'IP del server che ha generato la richiesta
        Private m_RemotePort As Integer     'Porta del server che ha generato la richiesta
        Private m_IDAssegnatoA As Integer       'ID dell'utente a cui è assegnato
        Private m_AssegnatoA As CUser           'Utente a cui è assegnato
        Private m_NomeAssegnatoA As String      'Nome a cui è assegnato
        Private m_Note As String

        Public Sub New()
        End Sub

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

        Public Property Riferimento As String
            Get
                Return Me.m_Riferimento
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Riferimento
                If (oldValue = value) Then Exit Property
                Me.m_Riferimento = value
                Me.DoChanged("Riferimento", value, oldValue)
            End Set
        End Property

        Public Property RemoteIP As String
            Get
                Return Me.m_RemoteIP
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_RemoteIP
                If (oldValue = value) Then Exit Property
                Me.m_RemoteIP = value
                Me.DoChanged("RemoteIP", value, oldValue)
            End Set
        End Property

        Public Property RemotePort As Nullable(Of Integer)
            Get
                Return Me.m_RemotePort
            End Get
            Set(value As Nullable(Of Integer))
                Dim oldValue As Nullable(Of Integer) = Me.m_RemotePort
                If (oldValue = value) Then Exit Property
                Me.m_RemotePort = value
                Me.DoChanged("RemotePort", value, oldValue)
            End Set
        End Property



        Public Property IDAssegnatoA As Integer
            Get
                Return GetID(Me.m_AssegnatoA, Me.m_IDAssegnatoA)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDAssegnatoA
                If oldValue = value Then Exit Property
                Me.m_IDAssegnatoA = value
                Me.m_AssegnatoA = Nothing
                Me.DoChanged("IDAssegnatoA", value, oldValue)
            End Set
        End Property

        Public Property AssegnatoA As CUser
            Get
                If (Me.m_AssegnatoA Is Nothing) Then Me.m_AssegnatoA = Sistema.Users.GetItemById(Me.m_IDAssegnatoA)
                Return Me.m_AssegnatoA
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.AssegnatoA
                If (oldValue = value) Then Exit Property
                Me.m_AssegnatoA = value
                Me.m_IDAssegnatoA = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeAssegnatoA = value.Nominativo
                Me.DoChanged("AssegnatoA", value, oldValue)
            End Set
        End Property

        Public Property NomeAssegnatoA As String
            Get
                Return Me.m_NomeAssegnatoA
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomeAssegnatoA
                If (oldValue = value) Then Exit Property
                Me.m_NomeAssegnatoA = value
                Me.DoChanged("NomeAssegnatoA", value, oldValue)
            End Set
        End Property

        Public Property EMail As String
            Get
                Return Me.m_EMail
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_EMail
                If (oldValue = value) Then Exit Property
                Me.m_EMail = value
                Me.DoChanged("EMail", value, oldValue)
            End Set
        End Property

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

        Public Property Professione As String
            Get
                Return Me.m_Professione
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Professione
                If (oldValue = value) Then Exit Property
                Me.m_Professione = value
                Me.DoChanged("Professione", value, oldValue)
            End Set
        End Property

        Public Property IDFonte As Integer
            Get
                Return GetID(Me.m_Fonte, Me.m_IDFonte)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDFonte
                If (oldValue = value) Then Exit Property
                Me.m_IDFonte = value
                Me.m_Fonte = Nothing
                Me.DoChanged("IDFonte", value, oldValue)
            End Set
        End Property

        Public Property Fonte As CFonte
            Get
                Throw New NotImplementedException
            End Get
            Set(value As CFonte)
                Throw New NotImplementedException
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

        Public Property Note As String
            Get
                Return Me.m_Note
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Note
                If (oldValue = value) Then Exit Property
                Me.m_Note = value
                Me.DoChanged("Note", value, oldValue)
            End Set
        End Property

        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_ContattiEsterni"
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Databases.APPConn
        End Function

    End Class
     

End Class
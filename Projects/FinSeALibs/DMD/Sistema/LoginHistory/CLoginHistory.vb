Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Sistema

    Public Enum LogOutMethods As Integer
        LOGOUT_UNKNOWN = 0
        LOGOUT_LOGOUT = 1
        LOGOUT_TIMEOUT = 2
        LOGOUT_REMOTEDISCONNECT = 3
    End Enum

    <Serializable> _
    Public Class CLoginHistory
        Inherits DBObjectBase

        Private m_UserID As Integer
        Private m_User As CUser
        Private m_LogInTime As Nullable(Of Date)
        Private m_LogOutTime As Nullable(Of Date)
        Private m_RemoteIP As String
        Private m_RemotePort As Integer
        Private m_Session As String
        Private m_UserAgent As String
        Private m_LogoutMethod As LogOutMethods
        Private m_IDUfficio As Integer
        Private m_Ufficio As CUfficio
        Private m_NomeUfficio As String
        Private m_Parameters As CKeyCollection

        Public Sub New()
            Me.m_UserID = 0
            Me.m_User = Nothing
            Me.m_LogInTime = Nothing
            Me.m_LogOutTime = Nothing
            Me.m_RemoteIP = ""
            Me.m_RemotePort = 0
            Me.m_Session = ""
            Me.m_UserAgent = ""
            Me.m_LogoutMethod = LogOutMethods.LOGOUT_UNKNOWN
            Me.m_IDUfficio = 0
            Me.m_Ufficio = Nothing
            Me.m_NomeUfficio = ""
        End Sub

        Public Sub New(ByVal user As CUser, ByVal parameters As CKeyCollection)
            For Each k As String In parameters.Keys
                Me.Parameters.SetItemByKey(k, parameters(k))
            Next
            Me.m_UserID = GetID(user) ' Session("UserID")
            Me.m_User = user
            Me.m_LogInTime = Now
            Me.m_LogOutTime = Nothing
            Me.m_Session = parameters.GetItemByKey("SessionID")
            Me.m_RemoteIP = parameters.GetItemByKey("RemoteIP")
            Me.m_RemotePort = Formats.ToInteger(parameters.GetItemByKey("RemotePort"))
            Me.m_UserAgent = parameters.GetItemByKey("RemoteUserAgent")
            Me.m_NomeUfficio = parameters.GetItemByKey("CurrentUfficio")
            Me.m_Ufficio = Anagrafica.Uffici.GetItemByName(Me.m_NomeUfficio)
            Me.m_IDUfficio = GetID(Me.m_Ufficio)
        End Sub

        Public Function IsActive() As Boolean
            Return Me.m_LogoutMethod = LogOutMethods.LOGOUT_UNKNOWN
        End Function

        ''' <summary>
        ''' Restituisce l'ID dell'ufficio da cui l'utente ha effettuato il login
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property IDUfficio As Integer
            Get
                Return GetID(Me.m_Ufficio, Me.m_IDUfficio)
            End Get
        End Property

        ''' <summary>
        ''' Restituisce l'ufficio da cui l'utente ha effettuate l'accesso
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Ufficio As CUfficio
            Get
                If (Me.m_Ufficio Is Nothing) Then Me.m_Ufficio = Anagrafica.Uffici.GetItemById(Me.m_IDUfficio)
                Return Me.m_Ufficio
            End Get
        End Property

        ''' <summary>
        ''' Restituisce il nome dell'ufficio da cui l'utente ha effettuato l'accesso
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property NomeUfficio As String
            Get
                Return Me.m_NomeUfficio
            End Get
        End Property

        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        ''' <summary>
        ''' Restituisce l'ID dell'utente che ha effettuato l'accesso
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property UserID As Integer
            Get
                Return GetID(Me.m_User, Me.m_UserID)
            End Get
        End Property

        ''' <summary>
        ''' Restituisce l'utente che ha effettuate l'accesso
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property User As CUser
            Get
                If (Me.m_User Is Nothing) Then Me.m_User = Users.GetItemById(Me.m_UserID)
                Return Me.m_User
            End Get
        End Property

        Protected Friend Sub SetUser(ByVal value As CUser)
            Me.m_User = value
            Me.m_UserID = GetID(value)
        End Sub

        ''' <summary>
        ''' Restitusice la data e l'ora di accesso
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property LogInTime As Nullable(Of Date)
            Get
                Return Me.m_LogInTime
            End Get
        End Property

        Protected Friend Sub SetLogInTime(ByVal value As Date)
            Me.m_LogInTime = value
            Me.DoChanged("LogInTime", value)
        End Sub

        ''' <summary>
        ''' Restituisce la data e l'ora del logout
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property LogOutTime As Nullable(Of Date)
            Get
                Return Me.m_LogOutTime
            End Get
        End Property

        Protected Friend Sub SetLogOutTime(ByVal value As Date)
            Me.m_LogOutTime = value
            Me.DoChanged("LogOutTime", value)
        End Sub

        ''' <summary>
        ''' Restituisce l'IP del dispositivo remoto da cui l'utente ha effettuato l'accesso
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property RemoteIP As String
            Get
                Return Me.m_RemoteIP
            End Get
        End Property

        Protected Friend Sub SetRemoteIP(ByVal value As String)
            Me.m_RemoteIP = Trim(value)
            Me.DoChanged("RemoteIP", value)
        End Sub

        ''' <summary>
        ''' Restituisce la porta del dispositivo remoto da cui l'utente ha effettuato l'accesso
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property RemotePort As Integer
            Get
                Return Me.m_RemotePort
            End Get
        End Property

        Protected Friend Sub SetRemotePort(ByVal value As Integer)
            Me.m_RemotePort = value
            Me.DoChanged("RemotePort", value)
        End Sub

        ''' <summary>
        ''' Restituisce la stringa identificativa della sessione iniziata dall'utente remoto sul sito
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property SessionID As String
            Get
                Return Me.m_Session
            End Get
        End Property

        Protected Friend Sub SetSessionID(ByVal value As String)
            Me.m_Session = Trim(value)
            Me.DoChanged("SessionID", value)
        End Sub

        ''' <summary>
        ''' Restituisce la useragent del browser con cui l'utente ha effettuato l'accesso
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property UserAgent As String
            Get
                Return Me.m_UserAgent
            End Get
        End Property

        Protected Friend Sub SetUserAgent(ByVal value As String)
            Me.m_UserAgent = Trim(value)
            Me.DoChanged("UserAgent", value)
        End Sub

        ''' <summary>
        ''' Restituisce il metodo di uscita
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property LogoutMethod As LogOutMethods
            Get
                Return Me.m_LogoutMethod
            End Get
        End Property

        Friend Sub SetLogoutMethod(ByVal value As LogOutMethods)
            Me.m_LogoutMethod = value
            Me.DoChanged("LogoutMethod", value)
        End Sub

        Public ReadOnly Property Parameters As CKeyCollection
            Get
                If (Me.m_Parameters Is Nothing) Then Me.m_Parameters = New CKeyCollection
                Return Me.m_Parameters
            End Get
        End Property

        Public Overrides Function GetTableName() As String
            Return "tbl_LoginHistory"
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Databases.LOGConn
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_UserID = reader.Read("User", Me.m_UserID)
            Me.m_LogInTime = reader.Read("LogInTime", Me.m_LogInTime)
            Me.m_LogOutTime = reader.Read("LogOutTime", Me.m_LogOutTime)
            Me.m_RemoteIP = reader.Read("RemoteIP", Me.m_RemoteIP)
            Me.m_RemotePort = reader.Read("RemotePort", Me.m_RemotePort)
            Me.m_Session = reader.Read("Session", Me.m_Session)
            Me.m_UserAgent = reader.Read("UserAgent", Me.m_UserAgent)
            Me.m_LogoutMethod = reader.Read("LogoutMethod", Me.m_LogoutMethod)
            Me.m_IDUfficio = reader.Read("IDUfficio", Me.m_IDUfficio)
            Me.m_NomeUfficio = reader.Read("NomeUfficio", Me.m_NomeUfficio)
            Dim tmp As String = reader.Read("Parameters", "")
            If (tmp <> "") Then Me.m_Parameters = XML.Utils.Serializer.Deserialize(tmp)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("User", Databases.GetID(m_User, m_UserID))
            writer.Write("LogInTime", Me.m_LogInTime)
            writer.Write("LogOutTime", Me.m_LogOutTime)
            writer.Write("RemoteIP", Me.m_RemoteIP)
            writer.Write("RemotePort", Me.m_RemotePort)
            writer.Write("Session", Me.m_Session)
            writer.Write("UserAgent", Me.m_UserAgent)
            writer.Write("LogoutMethod", Me.m_LogoutMethod)
            writer.Write("IDUfficio", Me.IDUfficio)
            writer.Write("NomeUfficio", Me.m_NomeUfficio)
            writer.Write("Parameters", XML.Utils.Serializer.Serialize(Me.Parameters))
            Return MyBase.SaveToRecordset(writer)
        End Function

        '------------------------
        Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
            writer.WriteAttribute("UserID", Me.UserID)
            writer.WriteAttribute("LogInTime", Me.m_LogInTime)
            writer.WriteAttribute("LogOutTime", Me.m_LogOutTime)
            writer.WriteAttribute("RemoteIP", Me.m_RemoteIP)
            writer.WriteAttribute("RemotePort", Me.m_RemotePort)
            writer.WriteAttribute("Session", Me.m_Session)
            writer.WriteAttribute("LogoutMethod", Me.m_LogoutMethod)
            writer.WriteAttribute("IDUfficio", Me.IDUfficio)
            writer.WriteAttribute("NomeUfficio", Me.m_NomeUfficio)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Parameters", Me.Parameters)
            writer.WriteTag("UserAgent", Me.m_UserAgent)
        End Sub

        Protected Overrides Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object)
            Select Case fieldName
                Case "UserID" : m_UserID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "LogInTime" : m_LogInTime = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "LogOutTime" : m_LogOutTime = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "RemoteIP" : m_RemoteIP = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "RemotePort" : m_RemotePort = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Session" : m_Session = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "UserAgent" : m_UserAgent = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "LogoutMethod" : m_LogoutMethod = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDUfficio" : Me.m_IDUfficio = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeUfficio" : Me.m_NomeUfficio = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Parameters" : Me.m_Parameters = CType(fieldValue, CKeyCollection)
                Case Else : Call MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

    End Class


End Class

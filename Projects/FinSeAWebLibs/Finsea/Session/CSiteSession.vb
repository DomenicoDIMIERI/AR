Imports DMD
Imports DMD.Databases
Imports DMD.Sistema

Partial Class WebSite

    <Serializable> _
    Public Class CSiteSession
        Inherits DBObjectBase

        Private m_SessionID As String
        Private m_StartTime As Date
        Private m_EndTime As Nullable(Of Date)
        Private m_RemoteIP As String
        Private m_RemotePort As Integer
        Private m_UserAgent As String
        Private m_Cookie As String
        Private m_InitialReferrer As String
        Private m_Parameters As CKeyCollection

        Public Sub New()
            Me.m_SessionID = ""
            Me.m_StartTime = Now
            Me.m_EndTime = Nothing
            Me.m_RemoteIP = ""
            Me.m_RemotePort = 0
            Me.m_UserAgent = ""
            Me.m_Cookie = ""
            Me.m_InitialReferrer = ""
            Me.m_Parameters = Nothing
        End Sub

        Public Property SessionID As String
            Get
                Return Me.m_SessionID
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_SessionID
                If (oldValue = value) Then Exit Property
                Me.m_SessionID = value
                Me.DoChanged("SessionID", value, oldValue)
            End Set
        End Property

        Public Property StartTime As Date
            Get
                Return Me.m_StartTime
            End Get
            Set(value As Date)
                Dim oldValue As Date = Me.m_StartTime
                If (oldValue = value) Then Exit Property
                Me.m_StartTime = value
                Me.DoChanged("StartTime", value, oldValue)
            End Set
        End Property

        Public Property EndTime As Nullable(Of Date)
            Get
                Return Me.m_EndTime
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_EndTime
                If (oldValue = value) Then Exit Property
                Me.m_EndTime = value
                Me.DoChanged("EndTime", value, oldValue)
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

        Public Property RemotePort As Integer
            Get
                Return Me.m_RemotePort
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_RemotePort
                If (oldValue = value) Then Exit Property
                Me.m_RemotePort = value
                Me.DoChanged("RemotePort", value, oldValue)
            End Set
        End Property

        Public Property UserAgent As String
            Get
                Return Me.m_UserAgent
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_UserAgent
                If (oldValue = value) Then Exit Property
                Me.m_UserAgent = value
                Me.DoChanged("UserAgent", value, oldValue)
            End Set
        End Property

        Public Property Cookie As String
            Get
                Return Me.m_Cookie
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Cookie
                If (oldValue = value) Then Exit Property
                Me.m_Cookie = value
                Me.DoChanged("Cookie", value, oldValue)
            End Set
        End Property

        Public Property InitialReferrer As String
            Get
                Return Me.m_InitialReferrer
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_InitialReferrer
                If (oldValue = value) Then Exit Property
                Me.m_InitialReferrer = value
                Me.DoChanged("InitialReferrer", value, oldValue)
            End Set
        End Property

        Public ReadOnly Property Parameters As CKeyCollection
            Get
                If (Me.m_Parameters Is Nothing) Then Me.m_Parameters = New CKeyCollection
                Return Me.m_Parameters
            End Get
        End Property

        Protected Overrides Function GetConnection() As CDBConnection
            Return Databases.LOGConn
        End Function

        Public Overrides Function GetModule() As CModule
            Return Sessions.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_SiteSessions"
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_SessionID = reader.Read("SessionID", Me.m_SessionID)
            Me.m_StartTime = reader.Read("StartTime", Me.m_StartTime)
            Me.m_EndTime = reader.Read("EndTime", Me.m_EndTime)
            Me.m_RemoteIP = reader.Read("RemoteIP", Me.m_RemoteIP)
            Me.m_RemotePort = reader.Read("RemotePort", Me.m_RemotePort)
            Me.m_UserAgent = reader.Read("UserAgent", Me.m_UserAgent)
            Me.m_Cookie = reader.Read("Cookie", Me.m_Cookie)
            Me.m_InitialReferrer = reader.Read("InitialReferrer1", Me.m_InitialReferrer)
            If (Me.m_InitialReferrer = "") Then Me.m_InitialReferrer = reader.Read("InitialReferrer", Me.m_InitialReferrer)
            Dim tmp As String = reader.Read("Parameters", "")
            If (tmp <> "") Then Me.m_Parameters = XML.Utils.Serializer.Deserialize(tmp)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("SessionID", Me.m_SessionID)
            writer.Write("StartTime", Me.m_StartTime)
            writer.Write("EndTime", Me.m_EndTime)
            writer.Write("RemoteIP", Me.m_RemoteIP)
            writer.Write("RemotePort", Me.m_RemotePort)
            writer.Write("UserAgent", Me.m_UserAgent)
            writer.Write("Cookie", Me.m_Cookie)
            writer.Write("InitialReferrer", Left(Me.m_InitialReferrer, 255))
            writer.Write("InitialReferrer1", Me.m_InitialReferrer)
            writer.Write("Parameters", XML.Utils.Serializer.Serialize(Me.Parameters))
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("SessionID", Me.m_SessionID)
            writer.WriteAttribute("StartTime", Me.m_StartTime)
            writer.WriteAttribute("EndTime", Me.m_EndTime)
            writer.WriteAttribute("RemoteIP", Me.m_RemoteIP)
            writer.WriteAttribute("RemotePort", Me.m_RemotePort)
            writer.WriteAttribute("Cookie", Me.m_Cookie)
            writer.WriteAttribute("InitialReferrer", Me.m_InitialReferrer)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Parameters", Me.Parameters)
            writer.WriteTag("UserAgent", Me.m_UserAgent)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "SessionID" : Me.m_SessionID = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "StartTime" : Me.m_StartTime = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "EndTime" : Me.m_EndTime = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "RemoteIP" : Me.m_RemoteIP = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "RemotePort" : Me.m_RemotePort = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "UserAgent" : Me.m_UserAgent = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Cookie" : Me.m_Cookie = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "InitialReferrer" : Me.m_InitialReferrer = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Parameters" : Me.m_Parameters = CType(fieldValue, CKeyCollection)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Overrides Function ToString() As String
            Return Strings.JoinW("SiteSession[", Me.ID, "] (", Me.StartTime, ", ", Me.SessionID, ", ", Me.UserAgent, ")")
        End Function

    End Class


End Class

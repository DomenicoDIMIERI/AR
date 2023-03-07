Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Sistema

    <Serializable> _
    Public Class CUserXGroup
        Inherits DBObjectBase

        Private m_UserID As Integer
        <NonSerialized> _
        Private m_User As CUser
        Private m_GroupID As Integer
        <NonSerialized> _
        Private m_Group As CGroup

        Public Sub New()
        End Sub

        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Property UserID As Integer
            Get
                Return GetID(Me.m_User, Me.m_UserID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.UserID
                If (oldValue = value) Then Exit Property
                Me.m_UserID = value
                Me.m_User = Nothing
                Me.DoChanged("UserID", value, oldValue)
            End Set
        End Property

        Public Property User As CUser
            Get
                If Me.m_User Is Nothing Then Me.m_User = Sistema.Users.GetItemById(Me.m_UserID)
                Return Me.m_User
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.m_User
                If (oldValue Is value) Then Exit Property
                Me.SetUser(value)
                Me.DoChanged("User", value, oldValue)
            End Set
        End Property

        Friend Overridable Sub SetUser(ByVal value As CUser)
            Me.m_User = value
            Me.m_UserID = GetID(value)
        End Sub

        Public Property GroupID As Integer
            Get
                Return GetID(Me.m_Group, Me.m_GroupID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.GroupID
                If oldValue = value Then Exit Property
                Me.m_GroupID = value
                Me.m_Group = Nothing
                Me.DoChanged("GroupID", value, oldValue)
            End Set
        End Property

        Public Property Group As CGroup
            Get
                If (Me.m_Group Is Nothing) Then Me.m_Group = Sistema.Groups.GetItemById(Me.m_GroupID)
                Return Me.m_Group
            End Get
            Set(value As CGroup)
                Dim oldValue As CGroup = Me.m_Group
                If (oldValue Is value) Then Exit Property
                Me.m_Group = value
                Me.m_GroupID = GetID(value)
                Me.DoChanged("Group", value, oldValue)
            End Set
        End Property

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Databases.APPConn
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_UsersXGroup"
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_UserID = reader.Read("User", Me.m_UserID)
            Me.m_GroupID = reader.Read("Group", Me.m_GroupID)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("User", Me.UserID)
            writer.Write("Group", Me.GroupID)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("User", Me.UserID)
            writer.WriteAttribute("Group", Me.GroupID)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "User" : Me.m_UserID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Group" : Me.m_GroupID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub


    End Class

End Class
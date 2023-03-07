Imports DMD
Imports DMD.Databases
Imports System.Net
Imports DMD.Sistema

Public MustInherit Class UserAllowNegate(Of T)
    Inherits DBObjectBase

    Private m_ItemID As Integer
    Private m_Item As T
    Private m_UserID As Integer
    Private m_User As CUser
    Private m_Allow As Boolean
    Private m_Negate As Boolean

    Public Sub New()
        Me.m_ItemID = 0
        Me.m_Item = Nothing
        Me.m_UserID = 0
        Me.m_User = Nothing
        Me.m_Allow = False
        Me.m_Negate = False
    End Sub

    Public Property ItemID As Integer
        Get
            Return GetID(Me.m_Item, Me.m_ItemID)
        End Get
        Set(value As Integer)
            Dim oldValue As Integer = Me.ItemID
            If (oldValue = value) Then Exit Property
            Me.m_Item = Nothing
            Me.m_ItemID = value
            Me.DoChanged("ItemID", value, oldValue)
        End Set
    End Property

    Public Property Item As T
        Get
            Return Me.m_Item
        End Get
        Set(value As T)
            Dim oldValue As Object = Me.m_Item
            Dim newValue As Object = value
            If (oldValue Is newValue) Then Exit Property
            Me.m_Item = value
            Me.m_ItemID = GetID(value)
            Me.DoChanged("Item", value, oldValue)
        End Set
    End Property

    Protected Friend Sub SetItem(ByVal item As T)
        Me.m_Item = item
        Me.m_ItemID = GetID(item)
    End Sub

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
            If (Me.m_User Is Nothing) Then Me.m_User = Sistema.Users.GetItemById(Me.m_UserID)
            Return Me.m_User
        End Get
        Set(value As CUser)
            Dim oldValue As CUser = Me.m_User
            If (oldValue Is value) Then Exit Property
            Me.m_User = value
            Me.m_UserID = GetID(value)
            Me.DoChanged("User", value, oldValue)
        End Set
    End Property

    Public ReadOnly Property Nominativo As String
        Get
            If (Me.User Is Nothing) Then Return "User[" & Me.UserID & "]"
            Return Me.User.UserName & " (" & Me.User.Nominativo & ")"
        End Get
    End Property

    Public Property Allow As Boolean
        Get
            Return Me.m_Allow
        End Get
        Set(value As Boolean)
            If (Me.m_Allow = value) Then Exit Property
            Me.m_Allow = value
            Me.DoChanged("Allow", Me.m_Allow, Not value)
        End Set
    End Property

    Public Property Negate As Boolean
        Get
            Return Me.m_Negate
        End Get
        Set(value As Boolean)
            If (Me.m_Negate = value) Then Exit Property
            Me.m_Negate = value
            Me.DoChanged("Negate", value, Not value)
        End Set
    End Property

    Protected MustOverride Function GetItemFieldName() As String

    Protected Overridable Function GetUserFieldName() As String
        Return "Utente"
    End Function

    Public MustOverride Overrides Function GetTableName() As String

    Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
        Me.m_ItemID = reader.Read(Me.GetItemFieldName, Me.m_ItemID)
        Me.m_UserID = reader.Read(Me.GetUserFieldName, Me.m_UserID)
        Me.m_Allow = reader.Read("Allow", Me.m_Allow)
        Me.m_Negate = reader.Read("Negate", Me.m_Negate)
        Return MyBase.LoadFromRecordset(reader)
    End Function

    Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
        writer.Write(Me.GetItemFieldName, Me.ItemID)
        writer.Write(Me.GetUserFieldName, Me.UserID)
        writer.Write("Allow", Me.m_Allow)
        writer.Write("Negate", Me.m_Negate)
        Return MyBase.SaveToRecordset(writer)
    End Function

    Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
        writer.WriteAttribute(Me.GetItemFieldName, Me.ItemID)
        writer.WriteAttribute(Me.GetUserFieldName, Me.UserID)
        writer.WriteAttribute("Allow", Me.m_Allow)
        writer.WriteAttribute("Negate", Me.m_Negate)
        MyBase.XMLSerialize(writer)
    End Sub

    Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
        Select Case fieldName
            Case Me.GetItemFieldName : Me.m_ItemID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
            Case Me.GetUserFieldName : Me.m_UserID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
            Case "Allow" : Me.m_Allow = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
            Case "Negate" : Me.m_Negate = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
            Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
        End Select
    End Sub

End Class

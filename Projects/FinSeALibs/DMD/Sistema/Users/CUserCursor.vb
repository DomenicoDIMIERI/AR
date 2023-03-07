Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Sistema

    Public Class CUserCursor
        Inherits DBObjectCursorPO(Of CUser)

        Private m_GroupID As Integer
        Private m_UserName As New CCursorFieldObj(Of String)("UserName")
        Private m_Nominativo As New CCursorFieldObj(Of String)("Nominativo")
        Private m_eMail As New CCursorFieldObj(Of String)("eMail")
        'Private m_Visible As New CCursorField(Of Boolean)("Visible")
        Private m_Flags As New CCursorField(Of UserFlags)("Flags")
        Private m_PasswordExpire As New CCursorField(Of Date)("PasswordExpire")

        Public Sub New()
        End Sub

        Public ReadOnly Property PasswordExpire As CCursorField(Of Date)
            Get
                Return Me.m_PasswordExpire
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of UserFlags)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public ReadOnly Property UserName As CCursorFieldObj(Of String)
            Get
                Return Me.m_UserName
            End Get
        End Property

        Public ReadOnly Property Nominativo As CCursorFieldObj(Of String)
            Get
                Return Me.m_Nominativo
            End Get
        End Property

        Public Property GroupID As Integer
            Get
                Return Me.m_GroupID
            End Get
            Set(value As Integer)
                Me.m_GroupID = value
            End Set
        End Property

        Public ReadOnly Property eMail As CCursorFieldObj(Of String)
            Get
                Return Me.m_eMail
            End Get
        End Property

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("GroupID", Me.m_GroupID)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "GroupID" : Me.m_GroupID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Overrides Function GetTableName() As String
            Return "Login"
        End Function

        Public Overrides Function GetWherePart() As String
            Dim ret As String
            ret = MyBase.GetWherePart
            If Me.m_GroupID <> 0 Then ret = Strings.Combine(ret, "([ID] In (SELECT [User] FROM [tbl_UsersXGroup] WHERE [Group]=" & Me.m_GroupID & "))", " AND ")
            Return ret
        End Function

        Protected Overrides Sub OnInitialize(item As Object)
            With DirectCast(item, CUser)
                .SetUserName(Users.GetAvailableUserName("Utente"))
            End With
            MyBase.OnInitialize(item)
        End Sub

        Protected Overrides Function GetModule() As CModule
            Return Sistema.Users.Module
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return APPConn
        End Function
    End Class



End Class

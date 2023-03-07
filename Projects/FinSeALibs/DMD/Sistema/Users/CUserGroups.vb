Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Sistema

    Public Class CUserGroups
        Inherits CCollection(Of CGroup)

        Private m_User As CUser

        Public Sub New()
            Me.m_User = Nothing
        End Sub

        Public Sub New(ByVal user As CUser)
            Me.New()
            Me.Load(user)
        End Sub

        Public ReadOnly Property User As CUser
            Get
                Return Me.m_User
            End Get
        End Property

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            'If (Me.m_User IsNot Nothing) Then DirectCast(value, CUserXGroup).SetUser(Me.m_User)
            MyBase.OnInsert(index, value)
        End Sub

        Protected Friend Function Load(ByVal user As CUser) As Boolean
            If (user Is Nothing) Then Throw New ArgumentNullException("user")

            MyBase.Clear()
            Me.m_User = user
            Dim cursor As New CUserXGroupCursor
            cursor.UserID.Value = GetID(user)
            cursor.IgnoreRights = True

            While Not cursor.EOF
                If cursor.Item.Group IsNot Nothing Then MyBase.Add(cursor.Item.Group)
                cursor.MoveNext()
            End While
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

            Return True
        End Function

        Public Overloads Function IndexOf(ByVal groupName As String) As Integer
            For i As Integer = 0 To Me.Count - 1
                If Me(i).GroupName = groupName Then Return i
            Next
            Return -1
        End Function

        Public Overloads Function Contains(ByVal groupName As String) As Integer
            Return Me.IndexOf(groupName) >= 0
        End Function


    End Class

End Class

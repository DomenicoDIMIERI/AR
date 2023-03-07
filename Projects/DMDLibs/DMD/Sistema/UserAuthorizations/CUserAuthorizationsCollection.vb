Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Sistema

    ''' <summary>
    ''' Collezione delle autorizzazioni utente
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public NotInheritable Class CUserAuthorizationCollection
        Inherits CCollection(Of CUserAuthorization)

        <NonSerialized> Private m_User As CUser

        Public Sub New()
            Me.m_User = Nothing
        End Sub

        Public Sub New(ByVal user As CUser)
            Me.New()
            Me.Load(user)
        End Sub

        Friend Sub Load(ByVal user As CUser)
            If (user Is Nothing) Then Throw New ArgumentNullException("user")
            Me.Clear()
            Me.m_User = user
            Dim cursor As New CUserAuthorizationCursor
            cursor.UserID.Value = GetID(user)
            cursor.IgnoreRights = True
            While Not cursor.EOF
                MyBase.Add(cursor.Item)
                cursor.MoveNext()
            End While
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
        End Sub

        Public Function GetItemByAction(ByVal action As CModuleAction) As CUserAuthorization
            For Each Item As CUserAuthorization In Me
                If (Item.Action Is action) Then Return Item
            Next
            Return Nothing
        End Function

        Public Function SetAllowNegate(ByVal action As CModuleAction, ByVal allow As Boolean, ByVal negate As Boolean) As CUserAuthorization
            Dim item As CUserAuthorization = Me.GetItemByAction(action)
            If (item IsNot Nothing) Then
                If (allow = negate) Then
                    item.Delete()
                    item.Allow = False
                    item.Negate = False
                    Me.Remove(item)
                Else
                    item.Allow = allow
                    item.Negate = negate
                    item.Save()
                End If
            Else
                If (allow <> negate) Then
                    item = New CUserAuthorization
                    item.Action = action
                    item.User = Me.m_User
                    item.Allow = allow
                    item.Negate = negate
                    item.Save()
                    Me.Add(item)
                End If
            End If
            Return item
        End Function

        Public Sub GetAllowNegate(ByVal action As CModuleAction, ByRef a As Boolean, ByRef n As Boolean)
            Dim item As CUserAuthorization = Me.GetItemByAction(action)
            If (item IsNot Nothing) Then
                a = a Or item.Allow
                n = n Or item.Negate
            End If
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_User IsNot Nothing) Then DirectCast(newValue, CUserAuthorization).SetUser(Me.m_User)
            MyBase.OnSet(index, oldValue, newValue)
        End Sub

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_User IsNot Nothing) Then DirectCast(value, CUserAuthorization).SetUser(Me.m_User)
            MyBase.OnInsert(index, value)
        End Sub


        Protected Friend Sub SetUser(ByVal user As CUser)
            Me.m_User = user
            For Each item As CUserAuthorization In Me
                item.SetUser(user)
            Next
        End Sub

    End Class


End Class
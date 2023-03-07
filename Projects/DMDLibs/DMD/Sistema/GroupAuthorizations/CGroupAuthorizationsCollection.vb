Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Sistema

    ''' <summary>
    ''' Collezione delle autorizzazioni di gruppo
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public NotInheritable Class CGroupAuthorizationCollection
        Inherits CCollection(Of CGroupAuthorization)

        <NonSerialized> _
        Private m_Group As CGroup

        Public Sub New()
            Me.m_Group = Nothing
        End Sub

        Public Sub New(ByVal Group As CGroup)
            Me.New()
            Me.Load(Group)
        End Sub

        Friend Sub Load(ByVal Group As CGroup)
            If (Group Is Nothing) Then Throw New ArgumentNullException("Group")
            Me.Clear()
            Me.m_Group = Group
            Dim cursor As New CGroupAuthorizationCursor
            cursor.GroupID.Value = GetID(Group)
            cursor.IgnoreRights = True
            While Not cursor.EOF
                MyBase.Add(cursor.Item)
                cursor.MoveNext()
            End While
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
        End Sub

        Public Function GetItemByAction(ByVal action As CModuleAction) As CGroupAuthorization
            For Each Item As CGroupAuthorization In Me
                If (Item.Action Is action) Then Return Item
            Next
            Return Nothing
        End Function

        Public Function SetAllowNegate(ByVal action As CModuleAction, ByVal allow As Boolean, ByVal negate As Boolean) As CGroupAuthorization
            Dim item As CGroupAuthorization = Me.GetItemByAction(action)
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
                    item = New CGroupAuthorization
                    item.Action = action
                    item.Group = Me.m_Group
                    item.Allow = allow
                    item.Negate = negate
                    item.Save()
                    Me.Add(item)
                End If
            End If
            Return item
        End Function

        Public Sub GetAllowNegate(ByVal action As CModuleAction, ByRef a As Boolean, ByRef n As Boolean)
            Dim item As CGroupAuthorization = Me.GetItemByAction(action)
            If (item IsNot Nothing) Then
                a = a Or item.Allow
                n = n Or item.Negate
            End If
        End Sub


    End Class


End Class
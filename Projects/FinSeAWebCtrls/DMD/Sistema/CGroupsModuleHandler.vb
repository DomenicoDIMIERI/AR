Imports DMD
Imports DMD.Sistema
Imports DMD.Forms
Imports DMD.WebSite
Imports DMD.Databases
Imports DMD.Forms.Utils

Imports DMD.Anagrafica
Imports DMD.XML

Namespace Forms


    Public Class CGroupsModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            Me.UseLocal = True
        End Sub

        Public Overrides Function GetInternalItemById(ByVal id As Integer) As Object
            'Return MyBase.GetItemById()
            Return Sistema.Groups.GetItemById(id)
        End Function



        Public Function GetUserName(ByVal renderer As Object) As String
            Return Users.CurrentUser.UserName
        End Function

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Dim cursor As New CGroupCursor
            cursor.GroupName.SortOrder = SortEnum.SORT_ASC
            Return cursor
        End Function


        Public Function AddUser(ByVal renderer As Object) As String
            Dim group As CGroup
            group = Sistema.Groups.GetItemById(RPC.n2int(Me.GetParameter(renderer, "gid", "")))
            If Me.CanEdit(group) Then
                group.Members.Add(Sistema.Users.GetItemById(RPC.n2int(Me.GetParameter(renderer, "uid", ""))))
                AddUser = "0"
            Else
                Err.Raise(-255, "Gruppi", "Diritti insufficienti")
                AddUser = "-255"
            End If
        End Function

        Public Function RemoveUser(ByVal renderer As Object) As String
            Dim group As CGroup
            group = Sistema.Groups.GetItemById(RPC.n2int(Me.GetParameter(renderer, "gid", "")))
            If Me.CanEdit(group) Then
                group.Members.Remove(Sistema.Users.GetItemById(RPC.n2int(Me.GetParameter(renderer, "uid", ""))))
                Return "0"
            Else
                Throw New PermissionDeniedException("RemoveUser")
                Return vbNullString
            End If
        End Function

        Public Function GetMembers(ByVal renderer As Object) As String
            Dim group As CGroup
            group = Sistema.Groups.GetItemById(RPC.n2int(Me.GetParameter(renderer, "id", "")))
            If group.Members.Count > 0 Then
                Return XML.Utils.Serializer.Serialize(group.Members.ToArray, XMLSerializeMethod.Document)
            Else
                Return vbNullString
            End If
        End Function

    End Class




End Namespace
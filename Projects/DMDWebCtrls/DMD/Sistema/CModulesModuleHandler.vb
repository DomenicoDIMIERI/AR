Imports DMD
Imports DMD.Sistema
Imports DMD.Forms
Imports DMD.WebSite
Imports DMD.Databases
Imports DMD.Forms.Utils


Imports DMD.Web
Imports DMD.XML

Namespace Forms

    '-------------------------------------------------------
    Public Class CModulesModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SDuplicate Or ModuleSupportFlags.SEdit)
            Me.UseLocal = True
        End Sub

        Public Overrides Function GetInternalItemById(ByVal id As Integer) As Object
            'Return MyBase.GetItemById()
            Return Sistema.Modules.GetItemById(id)
        End Function

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CModulesCursor
        End Function

        'Public Function getUsrModuleAuth(ByVal renderer As Object) As String
        '    Dim lst As CListAutorizzazioniUtente
        '    Dim CanEdit As Boolean
        '    Dim userID, moduleID As Integer
        '    CanEdit = Sistema.Users.Module.UserCanDoAction("list_auth")
        '    If CanEdit = False Then
        '        Throw New PermissionDeniedException
        '    Else
        '        userID = RPC.n2int(Me.GetParameter(renderer, "uid", ""))
        '        moduleID = RPC.n2int(Me.GetParameter(renderer, "mid", ""))

        '        lst = New CListAutorizzazioniUtente
        '        lst.Name = "lstAutorizzazioniUtente"
        '        'With DirectCast(Me.Renderer, IModuleRenderer)
        '        lst.Width = 300 '.Width \ 2
        '        lst.Height = 300 '.Height
        '        'End With
        '        lst.User = Sistema.Users.GetItemById(userID)
        '        lst.Modulo = Sistema.Modules.GetItemById(moduleID)
        '        Dim writer As New HTMLWriter
        '        lst.GetInnerHTML(writer)
        '        Dim ret As String = writer.ToString
        '        ' writer.Dispose()
        '        Return ret
        '    End If
        'End Function

        Public Function CreateModuleActions(ByVal renderer As Object) As String
            If Me.Module.UserCanDoAction("edit") = False Then Throw New PermissionDeniedException(Me.Module, "CreateModuleActions")
            Dim mID As Integer = RPC.n2int(Me.GetParameter(renderer, "mid", ""))
            Dim m As CModule = Modules.GetItemById(mID)
            Dim handler As IModuleHandler = m.CreateHandler(Nothing)
            If (handler IsNot Nothing) Then
                Dim cursor As DBObjectCursorBase = handler.CreateCursor
                Dim action As CModuleAction
                Dim items() As String = Nothing
                If (cursor IsNot Nothing) Then
                    Dim item As Object = cursor.Add
                    If TypeOf (item) Is DBObjectPO Then
                        items = {"create", "edit", "list", "delete", "list_office", "edit_office", "delete_office", "list_own", "edit_own", "delete_own", "import", "export"}
                    ElseIf TypeOf (item) Is DBObjectBase Then
                        items = {"create", "edit", "list", "delete", "import", "export"}
                    Else
                        items = {"create", "edit", "list", "delete", "list_own", "edit_own", "delete_own", "import", "export"}
                    End If
                    cursor.Dispose()
                End If
                If items IsNot Nothing Then
                    For i As Integer = 0 To UBound(items)
                        If (Not m.DefinedActions.ContainsKey(items(i))) Then action = m.DefinedActions.RegisterAction(items(i)) : action.Save()
                    Next
                End If
            End If
            Return ""
        End Function

        'Public Function getGrpModuleAuth(ByVal renderer As Object) As String
        '    Dim lst As CListAutorizzazioniGruppo
        '    Dim CanEdit As Boolean
        '    Dim groupID, moduleID As Integer

        '    CanEdit = Sistema.Users.Module.UserCanDoAction("list_auth")
        '    If CanEdit = False Then
        '        Throw New PermissionDeniedException
        '    Else
        '        groupID = RPC.n2int(Me.GetParameter(renderer, "gid", ""))
        '        moduleID = RPC.n2int(Me.GetParameter(renderer, "mid", ""))

        '        lst = New CListAutorizzazioniGruppo
        '        lst.Name = "lstAutorizzazioniGruppo"
        '        ' With DirectCast(Me.Renderer, IModuleRenderer)
        '        lst.Width = 300 '.Width \ 2
        '        lst.Height = 300 '.Height
        '        'End With
        '        lst.Group = Sistema.Groups.GetItemById(groupID)
        '        lst.Modulo = Sistema.Modules.GetItemById(moduleID)
        '        Dim writer As New HTMLWriter
        '        lst.GetInnerHTML(writer)
        '        Dim ret As String = writer.ToString
        '        'writer.Dispose()
        '        Return ret
        '    End If
        'End Function

        Public Function SetGroupAuth(ByVal renderer As Object) As String
            Dim CanEdit As Boolean = Me.Module.UserCanDoAction("edit")
            If CanEdit = False Then Throw New PermissionDeniedException(Me.Module, "SetGroupAuth")
            Dim moduleID, groupID As Integer
            Dim allow, negate As Boolean
            moduleID = RPC.n2int(Me.GetParameter(renderer, "mid", ""))
            groupID = RPC.n2int(Me.GetParameter(renderer, "gid", ""))
            allow = RPC.n2bool(Me.GetParameter(renderer, "allow", ""))
            negate = RPC.n2bool(Me.GetParameter(renderer, "negate", ""))
            Dim group As CGroup = Groups.GetItemById(groupID)
            Dim [module] As CModule = Modules.GetItemById(moduleID)
            group.Modules.SetAllowNegate([module], allow, negate)
            Return "0"
        End Function

        Public Function SetUserAuth(ByVal renderer As Object) As String
            Dim CanEdit As Boolean = Me.Module.UserCanDoAction("edit")
            If CanEdit = False Then Throw New PermissionDeniedException(Me.Module, "SetUserAuth")
            Dim moduleID, userID As Integer
            Dim allow, negate As Boolean
            moduleID = RPC.n2int(Me.GetParameter(renderer, "mid", ""))
            userID = RPC.n2int(Me.GetParameter(renderer, "uid", ""))
            allow = RPC.n2bool(Me.GetParameter(renderer, "allow", ""))
            negate = RPC.n2bool(Me.GetParameter(renderer, "negate", ""))
            Dim user As CUser = Users.GetItemById(userID)
            Dim [module] As CModule = Modules.GetItemById(moduleID)
            user.Modules.SetAllowNegate([module], allow, negate)
            Return "0"
        End Function

        Public Function SetUserAction(ByVal renderer As Object) As String
            Dim CanEdit As Boolean
            Dim actionID, userID As Integer
            Dim allow, negate As Boolean
            CanEdit = Me.Module.UserCanDoAction("edit")
            If CanEdit = False Then Throw New PermissionDeniedException(Me.Module, "SetUserAction")

            actionID = RPC.n2int(Me.GetParameter(renderer, "aid", ""))
            userID = RPC.n2int(Me.GetParameter(renderer, "uid", ""))
            allow = RPC.n2bool(Me.GetParameter(renderer, "a", ""))
            negate = RPC.n2bool(Me.GetParameter(renderer, "n", ""))

            Dim user As CUser = Users.GetItemById(userID)
            Dim action As CModuleAction = Modules.DefinedActions.GetItemById(actionID)
            Dim item As CUserAuthorization = action.SetUserAllowNegate(user, allow, negate)

            Return XML.Utils.Serializer.Serialize(item)
        End Function

        Public Function SetGroupAction(ByVal renderer As Object) As String
            Dim CanEdit As Boolean = Me.Module.UserCanDoAction("edit")
            Dim actionID, groupID As Integer
            Dim allow, negate As Boolean
            If CanEdit = False Then Throw New PermissionDeniedException("SetGroupAction")

            actionID = RPC.n2int(Me.GetParameter(renderer, "aid", ""))
            groupID = RPC.n2int(Me.GetParameter(renderer, "gid", ""))
            allow = RPC.n2bool(Me.GetParameter(renderer, "a", ""))
            negate = RPC.n2bool(Me.GetParameter(renderer, "n", ""))

            Dim group As CGroup = Groups.GetItemById(groupID)
            Dim action As CModuleAction = Modules.DefinedActions.GetItemById(actionID)
            Dim item As CGroupAuthorization = action.SetGroupAllowNegate(group, allow, negate)
            Return XML.Utils.Serializer.Serialize(item)
        End Function

        Public Function AddAction(ByVal renderer As Object) As String
            Dim mid, actionName, actionDescription As String
            Dim actionVisibility As Boolean
            Dim m As CModule
            Dim action As CModuleAction

            If Not Me.Module.UserCanDoAction("edit") Then
                Throw New PermissionDeniedException
                Return vbNullString
            End If
            mid = Formats.ToInteger(RPC.n2int(Me.GetParameter(renderer, "mid", "")))
            actionName = RPC.n2str(Me.GetParameter(renderer, "an", ""))
            actionDescription = RPC.n2str(Me.GetParameter(renderer, "ad", ""))
            actionVisibility = Formats.ToBool(RPC.n2bool(Me.GetParameter(renderer, "v", "")))

            m = Sistema.Modules.GetItemById(mid)
            If (m.DefinedActions.ContainsKey(actionName)) Then
                Throw New ArgumentException("L'azione [" & actionName & "] è già stata definita per il modulo")
                Return vbNullString
            End If

            action = m.DefinedActions.RegisterAction(actionName)
            action.AuthorizationDescription = actionDescription
            action.Visible = actionVisibility
            action.Save()

            Return XML.Utils.Serializer.Serialize(action)
        End Function

        Public Function RemoveAction(ByVal renderer As Object) As String
            Dim actionID As Integer
            Dim action As CModuleAction
            If Not Me.Module.UserCanDoAction("edit") Then Throw New PermissionDeniedException("RemoveAction")
            actionID = RPC.n2int(Me.GetParameter(renderer, "aid", ""))
            action = Modules.DefinedActions.GetItemById(actionID)
            action.Module.DefinedActions.UnregisterAction(action)
            Return "0"
        End Function

        Public Function SetActionVisibility(ByVal renderer As Object) As String
            If Not Me.Module.UserCanDoAction("edit") Then Throw New PermissionDeniedException("RemoveAction")

            Dim actionID As Integer = RPC.n2int(Me.GetParameter(renderer, "aid", ""))
            Dim action As CModuleAction = Modules.DefinedActions.GetItemById(actionID)
            action.Visible = RPC.n2bool(Me.GetParameter(renderer, "v", ""))
            action.Save()
            Return "0"
        End Function

        Public Function UserCanDoAction(ByVal renderer As Object) As String
            Dim a As String = RPC.n2str(GetParameter(renderer, "a", ""))
            Dim mID As Integer = RPC.n2int(GetParameter(renderer, "mid", ""))
            Dim m As CModule = Modules.GetItemById(mID)
            Return XML.Utils.Serializer.SerializeBoolean(m.UserCanDoAction(a))
        End Function

        Public Function GroupCanDoAction(ByVal renderer As Object) As String
            Throw New NotImplementedException
        End Function


        Public Function IsVisibleToUser(ByVal renderer As Object) As String
            Dim mID As Integer = RPC.n2int(GetParameter(renderer, "mid", ""))
            Dim m As CModule = Modules.GetItemById(mID)
            Return XML.Utils.Serializer.SerializeBoolean(m.IsVisibleToUser(Users.CurrentUser))
        End Function

        Public Function GetDefinedActions(ByVal renderer As Object) As String
            Dim mID As Integer = RPC.n2int(GetParameter(renderer, "mid", ""))
            Dim m As CModule = Modules.GetItemById(mID)
            Return XML.Utils.Serializer.Serialize(m.DefinedActions, XMLSerializeMethod.Document)
        End Function

        Public Function GetSettings(ByVal renderer As Object) As String
            Dim mid As Integer = RPC.n2int(GetParameter(renderer, "mid", 0))
            Dim m As CModule = Modules.GetItemById(mid)
            Return XML.Utils.Serializer.Serialize(m.Settings)
        End Function

        Public Function UpdateSettings(ByVal renderer As Object) As String
            Dim mid As Integer = RPC.n2int(GetParameter(renderer, "mid", 0))
            Dim m As CModule = Modules.GetItemById(mid)
            m.Settings.Update()
            Return ""
        End Function

        Public Function GetModulesXGroup(ByVal renderer As Object) As String
            Dim gid As Integer = RPC.n2int(GetParameter(renderer, "gid", 0))
            Dim grp As CGroup = Sistema.Groups.GetItemById(gid)
            Return XML.Utils.Serializer.Serialize(New CModuleXGroupCollection(grp))
        End Function

        Public Function GetModulesXUser(ByVal renderer As Object) As String
            Dim uid As Integer = RPC.n2int(GetParameter(renderer, "uid", 0))
            Dim user As CUser = Sistema.Users.GetItemById(uid)
            Return XML.Utils.Serializer.Serialize(New CModuleXUserCollection(user))
        End Function

    End Class


End Namespace
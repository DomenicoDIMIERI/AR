Imports DMD
Imports DMD.Sistema
Imports DMD.WebSite
Imports DMD.Forms
Imports DMD.Databases

Namespace Forms

  
  
    Public Class CCollegamentiModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SDuplicate Or ModuleSupportFlags.SEdit)
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CCollegamentiCursor
        End Function


        Public Overrides Function GetInternalItemById(id As Integer) As Object
            Return Collegamenti.GetItemById(id)
        End Function

        Public Function SetUserAllowNegate(ByVal renderer As Object) As String
            Dim itemID As Integer = RPC.n2int(Me.GetParameter(renderer, "cid", ""))
            Dim userID As Integer = RPC.n2int(Me.GetParameter(renderer, "uid", ""))
            Dim allow As Boolean = RPC.n2bool(Me.GetParameter(renderer, "a", ""))
            Dim negate As Boolean = RPC.n2bool(Me.GetParameter(renderer, "n", ""))
            Dim item As CCollegamento = Me.GetInternalItemById(itemID)
            If Me.CanEdit(item) = False Then
                Throw New PermissionDeniedException
                Return vbNullString
            End If

            Dim ret As String = RPC.FormatID(GetID(item.SetUserAllowNegate(Sistema.Users.GetItemById(userID), allow, negate)))

            Collegamenti.UpdateCached(item)

            Return ret
        End Function

        Public Function GetSubLinks(ByVal renderer As Object) As String
            Dim pid As Integer = RPC.n2int(Me.GetParameter(renderer, "pid", ""))
            Dim c As CCollegamento = Collegamenti.GetItemById(pid)
            If (c.Childs.Count > 0) Then Return XML.Utils.Serializer.Serialize(c.Childs.ToArray)
            Return vbNullString
        End Function

        Public Function SetGroupAllowNegate(ByVal renderer As Object) As String
            Dim itemID As Integer = RPC.n2int(Me.GetParameter(renderer, "cid", ""))
            Dim groupID As Integer = RPC.n2int(Me.GetParameter(renderer, "gid", ""))
            Dim allow As Boolean = RPC.n2bool(Me.GetParameter(renderer, "a", ""))
            Dim negate As Boolean = RPC.n2bool(Me.GetParameter(renderer, "n", ""))
            Dim item As CCollegamento = Me.GetInternalItemById(itemID)
            If Me.CanEdit(item) = False Then
                Throw New PermissionDeniedException
                Return vbNullString
            End If
            Dim ret As String = RPC.FormatID(GetID(item.SetGroupAllowNegate(Sistema.Groups.GetItemById(groupID), allow, negate)))
            Collegamenti.UpdateCached(item)
            Return ret
        End Function

        Public Function GetUserAllowNegate(ByVal renderer As Object) As String
            Dim id As Integer = RPC.n2int(GetParameter(renderer, "id", ""))
            Dim collegamento As CCollegamento = Collegamenti.GetItemById(id)
            If (collegamento.UserAuth.Count > 0) Then
                Return XML.Utils.Serializer.Serialize(collegamento.UserAuth.ToArray)
            Else
                Return ""
            End If
        End Function

        Public Function GetGroupAllowNegate(ByVal renderer As Object) As String
            Dim id As Integer = RPC.n2int(GetParameter(renderer, "id", ""))
            Dim collegamento As CCollegamento = Collegamenti.GetItemById(id)
            If (collegamento.GroupAuth.Count > 0) Then
                Return XML.Utils.Serializer.Serialize(collegamento.GroupAuth.ToArray)
            Else
                Return ""
            End If
        End Function

        Public Function GetUserLinks(ByVal renderer As Object) As String
            Dim uid As Integer = RPC.n2int(GetParameter(renderer, "uid", "0"))
            Dim user As CUser = Sistema.Users.GetItemById(uid)
            Dim col As CCollection(Of CCollegamento) = Collegamenti.GetUserLinks(user)
            Return XML.Utils.Serializer.Serialize(col)
        End Function
    End Class


End Namespace
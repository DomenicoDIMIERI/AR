Imports DMD
Imports DMD.WebSite
Imports DMD.Sistema
Imports DMD.Forms
Imports DMD.Messenger
Imports DMD.XML

Namespace Forms

    Public Class CMessengerModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
        End Sub

        Public Function CountOnlineUsers(ByVal renderer As Object) As String
            Return Messages.CountOnlineUsers
        End Function

        Public Function LoadPrevMessages(ByVal renderer As Object) As String
            Dim fromUser As Integer = RPC.n2int(GetParameter(renderer, "fu", "0"))
            Dim toUser As Integer = RPC.n2int(GetParameter(renderer, "tu", "0"))
            Dim stanza As String = RPC.n2str(GetParameter(renderer, "stanza", ""))
            Dim fromID As Integer = RPC.n2int(GetParameter(renderer, "fid", "0"))
            Dim nMax As Integer = RPC.n2int(GetParameter(renderer, "nmax", "0"))
            If (nMax <= 0) Then nMax = 20
            Dim items As CCollection(Of CMessage) = Messages.LoadPrevMessages(fromUser, toUser, stanza, fromID, nMax)
            If (items.Count) > 0 Then
                Return XML.Utils.Serializer.Serialize(items.ToArray)
            Else
                Return ""
            End If

        End Function

        Public Function LoadUserMessages(ByVal renderer As Object) As String
            Dim fromUser As Integer = RPC.n2int(GetParameter(renderer, "fu", "0"))
            Dim toUser As Integer = RPC.n2int(GetParameter(renderer, "tu", "0"))
            Dim stanza As String = RPC.n2str(GetParameter(renderer, "stanza", ""))
            Dim fromDate As Nullable(Of Date) = RPC.n2date(GetParameter(renderer, "fd", ""))
            Dim toDate As Nullable(Of Date) = RPC.n2date(GetParameter(renderer, "td", ""))
            Dim items As CCollection(Of CMessage) = Messages.LoadUserMessages(fromUser, toUser, stanza, fromDate, toDate)
            If (items.Count) > 0 Then
                Return XML.Utils.Serializer.Serialize(items.ToArray)
            Else
                Return ""
            End If
        End Function

        Public Function GetNewerOrChangedUserMessages(ByVal renderer As Object) As String
            Dim newerThan As Integer = RPC.n2int(GetParameter(renderer, "nd", ""))
            Dim fromUser As Integer = RPC.n2int(GetParameter(renderer, "fu", "0"))
            Dim toUser As Integer = RPC.n2int(GetParameter(renderer, "tu", "0"))
            Dim stanza As String = RPC.n2str(GetParameter(renderer, "stanza", ""))
            Dim fromDate As Nullable(Of Date) = RPC.n2date(GetParameter(renderer, "fd", ""))
            Dim toDate As Nullable(Of Date) = RPC.n2date(GetParameter(renderer, "td", ""))
            Dim items As CCollection(Of CMessage) = Messages.GetNewerOrChangedUserMessages(newerThan, fromUser, toUser, stanza, fromDate, toDate)
            If (items.Count) > 0 Then
                Return XML.Utils.Serializer.Serialize(items.ToArray)
            Else
                Return ""
            End If
        End Function

        Public Function GetUsersList(ByVal renderer As Object) As String
            Dim items As CCollection(Of CChatUser)
            items = Messages.GetUsersList
            If (items.Count = 0) Then
                Return vbNullString
            Else
                Return XML.Utils.Serializer.Serialize(items.ToArray, XMLSerializeMethod.Document)
            End If
        End Function

        Public Function SendMessageTo(ByVal renderer As Object) As String
            Dim uID As Nullable(Of Integer) = RPC.n2int(GetParameter(renderer, "uid", ""))
            Dim text As String = RPC.n2str(GetParameter(renderer, "text", ""))
            Dim stanza As String = RPC.n2str(GetParameter(renderer, "stanza", ""))
            Dim msg As CMessage = Messages.SendMessage(text, uID, stanza)
            Return XML.Utils.Serializer.Serialize(msg, XMLSerializeMethod.Document)
        End Function

        Public Overrides Function CreateCursor() As Databases.DBObjectCursorBase
            Return Nothing
        End Function

    End Class

End Namespace

Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.Office
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.Web

Namespace Forms

    Public Class TicketsHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SDuplicate Or ModuleSupportFlags.SEdit Or ModuleSupportFlags.SAnnotations)
            Me.UseLocal = True
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CTicketCursor
        End Function


        Public Overrides Function CanEdit(item As Object) As Boolean
            Return MyBase.CanEdit(item) OrElse Me.CanLavorare(item)
        End Function

        Public Function Apri(ByVal renderer As Object) As String
            Dim id As Integer = RPC.n2int(GetParameter(renderer, "id", ""))
            Dim ec As CTicket = Me.GetInternalItemById(id)
            If (Me.CanEdit(ec) = False) Then Throw New PermissionDeniedException("Non sei abilitato ad effettuare la richiesta")
            ec.Apri()
            Return ""
        End Function


        Public Function PrendiInCarico(ByVal renderer As Object) As String
            Dim id As Integer = RPC.n2int(GetParameter(renderer, "id", ""))
            Dim ec As CTicket = Me.GetInternalItemById(id)
            If (Me.CanLavorare(ec) = False) Then Throw New PermissionDeniedException("Non sei abilitato a prendere in carico la richiesta")
            ec.PrendiInCarico()
            Return ""
        End Function

        Public Function Risolto(ByVal renderer As Object) As String
            Dim id As Integer = RPC.n2int(GetParameter(renderer, "id", ""))
            Dim ec As CTicket = Me.GetInternalItemById(id)
            If (Me.CanLavorare(ec) = False) Then Throw New PermissionDeniedException("Non sei abilitato ad evadere la richiesta")
            ec.Risolto()
            Return ""
        End Function

        Public Function Riapri(ByVal renderer As Object) As String
            Dim id As Integer = RPC.n2int(GetParameter(renderer, "id", ""))
            Dim ec As CTicket = Me.GetInternalItemById(id)
            ec.Riapri()
            Return ""
        End Function

        Public Function Sospendi(ByVal renderer As Object) As String
            Dim id As Integer = RPC.n2int(GetParameter(renderer, "id", ""))
            Dim ec As CTicket = Me.GetInternalItemById(id)
            If (Me.CanLavorare(ec) = False) Then Throw New PermissionDeniedException("Non sei abilitato ad evadere la richiesta")
            ec.Sospendi()
            Return ""
        End Function

        Public Function Errore(ByVal renderer As Object) As String
            Dim id As Integer = RPC.n2int(GetParameter(renderer, "id", ""))
            Dim ec As CTicket = Me.GetInternalItemById(id)
            If (Me.CanLavorare(ec) = False) Then Throw New PermissionDeniedException("Non sei abilitato ad evadere la richiesta")
            ec.Errore()
            Return ""
        End Function

        Public Function CanLavorare(ByVal item As CTicket) As Boolean
            If Tickets.GruppoEsclusi.Members.Contains(Users.CurrentUser) Then Return False
            If Me.Module.UserCanDoAction("lavorare") Then Return True
            'Dim categories As CCollection(Of CTicketCategory) = TicketCategories.GetUserAllowedCategories(Sistema.Users.CurrentUser)
            Dim cat As CTicketCategory = TicketCategories.GetItemByName(item.Categoria, "")
            If (cat IsNot Nothing AndAlso cat.NotifyUsers.Contains(Users.CurrentUser)) Then Return True
            cat = TicketCategories.GetItemByName(item.Categoria, item.Sottocategoria)
            If (cat IsNot Nothing AndAlso cat.NotifyUsers.Contains(Users.CurrentUser)) Then Return True
            Return False
        End Function


        Public Function CanChangeStatus(ByVal item As CTicket) As Boolean
            Return Me.Module.UserCanDoAction("change_status")
        End Function

        Public Function RemoveAllegato(ByVal renderer As Object) As String
            Dim id As Integer = RPC.n2int(GetParameter(renderer, "id", ""))
            Dim attid As Integer = RPC.n2int(GetParameter(renderer, "att", ""))
            Dim ec As CTicket = Me.GetInternalItemById(id)
            If (Not Me.CanChangeStatus(ec)) Then
                If (ec.StatoSegnalazione <= TicketStatus.APERTO AndAlso Not Me.CanLavorare(ec)) Then Throw New PermissionDeniedException("Non sei abilitato ad eliminare documenti")
            End If
            Dim att As CAttachment = ec.Attachments.GetItemById(attid)
            att.Delete()
            'ec.Allegati.remo
            Return ""
        End Function

        Public Function AddMessage(ByVal renderer As Object) As String
            Dim id As Integer = RPC.n2int(GetParameter(renderer, "id", ""))
            Dim msg As CTicketAnsware = XML.Utils.Serializer.Deserialize(RPC.n2str(GetParameter(renderer, "msg", "")))
            Dim ec As CTicket = Me.GetInternalItemById(id)
            'If Not Me.CanLavorare(ec) Then Throw New PermissionDeniedException(Me.Module, "lavorare")
            ec.AddMessage(msg)
            ec.Save()
            Return XML.Utils.Serializer.Serialize(ec)
        End Function

        Public Function GetActiveItems(ByVal renderer As Object) As String
            Dim uid As Integer = RPC.n2int(GetParameter(renderer, "uid", "0"))
            Dim user As CUser = Sistema.Users.GetItemById(uid)
            Return XML.Utils.Serializer.Serialize(Tickets.GetActiveItems(user))
        End Function

    End Class


End Namespace

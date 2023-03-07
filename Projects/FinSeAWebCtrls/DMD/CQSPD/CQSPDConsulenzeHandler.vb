Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.CQSPD
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.XML

Namespace Forms


    Public Class CQSPDConsulenzeHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SDuplicate Or ModuleSupportFlags.SEdit)
            Me.UseLocal = True
        End Sub


        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CQSPDConsulenzaCursor
        End Function


        Public Function GetConsulenzeByPersona(ByVal renderer As Object) As String
            Dim idPersona As Integer = RPC.n2int(GetParameter(renderer, "pid", ""))
            Dim items As CCollection(Of CQSPDConsulenza) = DMD.CQSPD.Consulenze.GetConsulenzeByPersona(idPersona)
            If (items.Count > 0) Then
                Return XML.Utils.Serializer.Serialize(items.ToArray, XMLSerializeMethod.Document)
            Else
                Return ""
            End If
        End Function

        Public Function Proponi(ByVal renderer As Object) As String
            Dim cid As Integer = RPC.n2int(GetParameter(renderer, "cid", ""))
            Dim consulenza As CQSPDConsulenza = CQSPD.Consulenze.GetItemById(cid)
            If (Me.CanEdit(consulenza) = False) Then Throw New PermissionDeniedException(Me.Module, "edit")
            consulenza.Proponi()
            Return ""
        End Function

        Public Function Accetta(ByVal renderer As Object) As String
            Dim cid As Integer = RPC.n2int(GetParameter(renderer, "cid", ""))
            Dim consulenza As CQSPDConsulenza = CQSPD.Consulenze.GetItemById(cid)
            If (Me.CanEdit(consulenza) = False) Then Throw New PermissionDeniedException(Me.Module, "edit")
            consulenza.Accetta()
            Return ""
        End Function

        Public Function Rifiuta(ByVal renderer As Object) As String
            Dim cid As Integer = RPC.n2int(GetParameter(renderer, "cid", ""))
            Dim motivo As String = RPC.n2str(GetParameter(renderer, "mt", ""))
            Dim dettaglio As String = RPC.n2str(GetParameter(renderer, "dt", ""))
            Dim consulenza As CQSPDConsulenza = CQSPD.Consulenze.GetItemById(cid)
            If (Me.CanEdit(consulenza) = False) Then Throw New PermissionDeniedException(Me.Module, "edit")
            consulenza.Rifiuta(motivo, dettaglio)
            Return ""
        End Function

        Public Function Boccia(ByVal renderer As Object) As String
            Dim cid As Integer = RPC.n2int(GetParameter(renderer, "cid", ""))
            Dim motivo As String = RPC.n2str(GetParameter(renderer, "mt", ""))
            Dim dettaglio As String = RPC.n2str(GetParameter(renderer, "dt", ""))
            Dim consulenza As CQSPDConsulenza = CQSPD.Consulenze.GetItemById(cid)
            If (Me.CanEdit(consulenza) = False) Then Throw New PermissionDeniedException(Me.Module, "edit")
            consulenza.Boccia(motivo, dettaglio)
            Return ""
        End Function

        Public Function Sollecita(ByVal renderer As Object) As String
            Dim cid As Integer = Me.GetParameter(renderer, "cid", 0)
            Dim consulenza As CQSPDConsulenza = CQSPD.Consulenze.GetItemById(cid)
            consulenza.Sollecita()
            Return ""
        End Function

        Public Function RichiediApprovazione(ByVal renderer As Object) As String
            Dim cid As Integer = RPC.n2int(GetParameter(renderer, "cid", ""))
            Dim consulenza As CQSPDConsulenza = CQSPD.Consulenze.GetItemById(cid)
            Dim motivo As String = RPC.n2str(GetParameter(renderer, "mot", ""))
            Dim dettaglio As String = RPC.n2str(GetParameter(renderer, "det", ""))
            Dim parametri As String = RPC.n2str(GetParameter(renderer, "par", ""))
            Dim rich As CRichiestaApprovazione = consulenza.RichiediApprovazione(motivo, dettaglio, parametri)
            Return XML.Utils.Serializer.Serialize(rich)
        End Function

        Public Function PrendiInCarico(ByVal renderer As Object) As String
            If (CQSPD.Pratiche.Module.UserCanDoAction("approva_sconto") = False) Then Throw New PermissionDeniedException(Me.Module, "approva_sconto")
            Dim cid As Integer = RPC.n2int(GetParameter(renderer, "cid", ""))
            Dim consulenza As CQSPDConsulenza = CQSPD.Consulenze.GetItemById(cid)
            Dim ret As CRichiestaApprovazione = consulenza.PrendiInCarico
            Return XML.Utils.Serializer.Serialize(ret)
        End Function

        Public Function ConfermaApprovazione(ByVal renderer As Object) As String
            If (CQSPD.Pratiche.Module.UserCanDoAction("approva_sconto") = False) Then Throw New PermissionDeniedException(Me.Module, "approva_sconto")
            Dim cid As Integer = RPC.n2int(GetParameter(renderer, "cid", ""))
            Dim consulenza As CQSPDConsulenza = CQSPD.Consulenze.GetItemById(cid)
            Dim motivoApprovazione As String = RPC.n2str(GetParameter(renderer, "mot", ""))
            Dim dettaglioApprovazione As String = RPC.n2str(GetParameter(renderer, "det", ""))
            Dim motivo As CMotivoScontoPratica = consulenza.RichiestaApprovazione.MotivoRichiesta
            If (motivo Is Nothing) Then Throw New ArgumentException("Il motivo di sconto indicato non è previsto")
            If (motivo.Privilegiato AndAlso Not Sistema.Users.CurrentUser.IsInGroup(CQSPD.GruppoAutorizzatori.GroupName)) Then
                Throw New PermissionDeniedException("L'utente corrente non è abilitato ad approvare uno sconto privilegiato")
            End If
            Dim ret As CRichiestaApprovazione = consulenza.Approva(motivoApprovazione, dettaglioApprovazione)
            Return XML.Utils.Serializer.Serialize(ret)
        End Function

        Public Function NegaApprovazione(ByVal renderer As Object) As String
            If (CQSPD.Pratiche.Module.UserCanDoAction("approva_sconto") = False) Then Throw New PermissionDeniedException(Me.Module, "approva_sconto")
            Dim cid As Integer = RPC.n2int(GetParameter(renderer, "cid", ""))
            Dim consulenza As CQSPDConsulenza = CQSPD.Consulenze.GetItemById(cid)
            Dim motivo As String = RPC.n2str(GetParameter(renderer, "mot", ""))
            Dim dettaglio As String = RPC.n2str(GetParameter(renderer, "det", ""))
            Dim ret As CRichiestaApprovazione = consulenza.Nega(motivo, dettaglio)
            Return XML.Utils.Serializer.Serialize(ret)
        End Function

        Public Function GetUltimaConsulenza(ByVal renderer As Object) As String
            Dim pid As Integer = RPC.n2int(GetParameter(renderer, "pid", ""))
            Dim ret As CQSPDConsulenza = CQSPD.Consulenze.GetUltimaConsulenza(pid)
            If (ret Is Nothing) Then
                Return ""
            Else
                Return XML.Utils.Serializer.Serialize(ret)
            End If
        End Function

        Public Function CreateElencoConsulenze(ByVal renderer As Object) As String
            Dim idCliente As Integer = RPC.n2int(GetParameter(renderer, "pid", "0"))
            Dim id As Integer = RPC.n2int(GetParameter(renderer, "id", "0"))
            Return Utils.CQSPDUtils.CreateElencoConsulenze(idCliente, id)
        End Function
    End Class


End Namespace
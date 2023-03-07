
Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.Office
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.Web
Imports DMD.XML

Namespace Forms

    Public Class RichiesteECHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SDuplicate Or ModuleSupportFlags.SEdit Or ModuleSupportFlags.SAnnotations)
            Me.UseLocal = True
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New EstrattiContributiviCursor
        End Function


        Public Function Richiedi(ByVal renderer As Object) As String
            Dim id As Integer = RPC.n2int(GetParameter(renderer, "id", ""))
            Dim ec As EstrattoContributivo = Me.GetInternalItemById(id)
            If (Me.CanEdit(ec) = False) Then Throw New PermissionDeniedException("Non sei abilitato ad effettuare la richiesta")
            ec.Richiedi()
            Return ""
        End Function


        Public Function PrendiInCarico(ByVal renderer As Object) As String
            Dim id As Integer = RPC.n2int(GetParameter(renderer, "id", ""))
            Dim ec As EstrattoContributivo = Me.GetInternalItemById(id)
            If (Me.Module.UserCanDoAction("lavorare") = False) Then Throw New PermissionDeniedException("Non sei abilitato a prendere in carico la richiesta")
            ec.PrendiInCarico()
            Return ""
        End Function

        Public Function Evadi(ByVal renderer As Object) As String
            Dim id As Integer = RPC.n2int(GetParameter(renderer, "id", ""))
            Dim ec As EstrattoContributivo = Me.GetInternalItemById(id)
            If (Me.Module.UserCanDoAction("lavorare") = False) Then Throw New PermissionDeniedException("Non sei abilitato ad evadere la richiesta")
            ec.Evadi()
            Return ""
        End Function

        Public Function Errore(ByVal renderer As Object) As String
            Dim id As Integer = RPC.n2int(GetParameter(renderer, "id", ""))
            Dim ec As EstrattoContributivo = Me.GetInternalItemById(id)
            If (Me.Module.UserCanDoAction("lavorare") = False) Then Throw New PermissionDeniedException("Non sei abilitato ad evadere la richiesta")
            ec.Errore()
            Return ""
        End Function

        Public Function Sospendi(ByVal renderer As Object) As String
            Dim id As Integer = RPC.n2int(GetParameter(renderer, "id", ""))
            Dim ec As EstrattoContributivo = Me.GetInternalItemById(id)
            If (Me.Module.UserCanDoAction("lavorare") = False) Then Throw New PermissionDeniedException("Non sei abilitato a sospendere la richiesta")
            ec.Sospendi()
            Return ""
        End Function

        Public Function CanLavorare(ByVal item As EstrattoContributivo) As Boolean
            Return Me.Module.UserCanDoAction("lavorare")
        End Function


        Public Function CanChangeStatus(ByVal item As EstrattoContributivo) As Boolean
            Return Me.Module.UserCanDoAction("change_status")
        End Function

        Public Function RemoveAllegato(ByVal renderer As Object) As String
            Dim id As Integer = RPC.n2int(GetParameter(renderer, "id", ""))
            Dim attid As Integer = RPC.n2int(GetParameter(renderer, "att", ""))
            Dim ec As EstrattoContributivo = Me.GetInternalItemById(id)
            If (Not Me.CanChangeStatus(ec)) Then
                If (ec.StatoRichiesta <= StatoEstrattoContributivo.Assegnato AndAlso Not Me.CanLavorare(ec)) Then Throw New PermissionDeniedException("Non sei abilitato ad eliminare documenti")
            End If
            Dim att As CAttachment = ec.Allegati.GetItemById(attid)
            att.Delete()
            'ec.Allegati.remo
            Return ""
        End Function

        Public Function AddMessage(ByVal renderer As Object) As String
            Dim id As Integer = RPC.n2int(GetParameter(renderer, "id", ""))
            Dim msg As String = RPC.n2str(GetParameter(renderer, "msg", ""))
            Dim ec As EstrattoContributivo = Me.GetInternalItemById(id)
            Dim ret As CAnnotazione = ec.AddMessage(msg)
            Return XML.Utils.Serializer.Serialize(ret, XMLSerializeMethod.Document)
        End Function

        Public Function AddAllegato(ByVal renderer As Object) As String
            Dim id As Integer = RPC.n2int(GetParameter(renderer, "id", ""))
            Dim att As CAttachment = XML.Utils.Serializer.Deserialize(RPC.n2str(GetParameter(renderer, "att", "")))
            Dim ec As EstrattoContributivo = Me.GetInternalItemById(id)
            Dim ret As CAttachment = ec.AddAllegato(att)
            Return XML.Utils.Serializer.Serialize(ret, XMLSerializeMethod.Document)
        End Function
    End Class


End Namespace

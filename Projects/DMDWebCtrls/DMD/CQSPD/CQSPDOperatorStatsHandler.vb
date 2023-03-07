Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.CQSPD
Imports DMD.Sistema
Imports DMD.Anagrafica

Namespace Forms
 
 
    Public Class CQSPDOperatorStatsHandler
        Inherits CQSPDBaseStatsHandler

        Public Sub New()
            Me.UseLocal = False
        End Sub




        Public Function GetStats(ByVal renderer As Object) As String
            If Not Me.Module.UserCanDoAction("list") Then Throw New PermissionDeniedException(Me.Module, "list")

            Dim statObj As New COpStatRecord
            statObj.IDPuntoOperativo = RPC.n2int(GetParameter(renderer, "po", "0"))
            statObj.IDOperatore = RPC.n2int(GetParameter(renderer, "op", "0"))
            statObj.FromDate = RPC.n2date(GetParameter(renderer, "di", ""))
            statObj.ToDate = RPC.n2date(GetParameter(renderer, "df", ""))
            statObj.SetPeriodo(RPC.n2str(GetParameter(renderer, "per", "")))
            statObj.OnlyValid = RPC.n2bool(GetParameter(renderer, "chkv", "T"))

            statObj.Validate()
            Return XML.Utils.Serializer.Serialize(statObj)
        End Function
    End Class


End Namespace
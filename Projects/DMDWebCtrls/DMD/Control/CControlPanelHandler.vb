Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.CQSPD
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.Forms.Utils

Namespace Forms


    Public Class CControlPanelHandler
        Inherits CBaseModuleHandler

        Public Sub New()
        End Sub


        Public Function Load(ByVal renderer As Object) As String
            Dim ret As New ControlPanelInfo
            Dim fromDate As Nullable(Of Date) = RPC.n2date(GetParameter(renderer, "fd", ""))
            If (fromDate.HasValue = False) Then fromDate = DateUtils.ToDay
            ret.Load(fromDate.Value)
            Dim tmp As String = XML.Utils.Serializer.Serialize(ret)
            Return tmp
        End Function

        Public Function Update(ByVal renderer As Object) As String
            Dim ret As New ControlPanelInfo
            Dim fromDate As Nullable(Of Date) = RPC.n2date(GetParameter(renderer, "fd", ""))
            If (fromDate.HasValue = False) Then fromDate = DateUtils.ToDay
            ret.Update(fromDate.Value)
            Return XML.Utils.Serializer.Serialize(ret)
        End Function

    End Class

End Namespace
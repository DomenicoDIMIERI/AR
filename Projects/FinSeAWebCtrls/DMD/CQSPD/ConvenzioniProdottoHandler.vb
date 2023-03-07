Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.CQSPD
Imports DMD.Sistema
Imports DMD.Anagrafica

Namespace Forms

    Public Class ConvenzioniProdottoHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SDuplicate Or ModuleSupportFlags.SEdit)
            Me.UseLocal = True
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CQSPDConvenzioniCursor
        End Function


        Public Function GetConvenzioniPerProdotto(ByVal renderer As Object) As String
            Dim pid As Integer = RPC.n2int(Me.GetParameter(renderer, "pid", ""))
            Dim onlyValid As Boolean = RPC.n2bool(Me.GetParameter(renderer, "ov", ""))
            Dim items As CCollection(Of CQSPDConvenzione) = CQSPD.Convenzioni.GetConvenzioniPerProdotto(pid, onlyValid)
            Return XML.Utils.Serializer.Serialize(items)
        End Function

    End Class


End Namespace
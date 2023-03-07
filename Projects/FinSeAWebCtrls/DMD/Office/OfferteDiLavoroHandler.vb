Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.Office
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.Web

Namespace Forms

    Public Class OfferteDiLavoroHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SEdit Or ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SDuplicate)
            Me.UseLocal = True
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New OffertaDiLavoroCursor
        End Function


        Public Function GetCandidature(ByVal renderer As Object) As String
            Dim oid As Integer = RPC.n2int(GetParameter(renderer, "oid", "0"))
            'Dim offerta As OffertaDiLavoro = Office.OfferteDiLavoro.GetItemById(oid)
            Return XML.Utils.Serializer.Serialize(Office.OfferteDiLavoro.GetCandidature(oid))
        End Function
    End Class


End Namespace
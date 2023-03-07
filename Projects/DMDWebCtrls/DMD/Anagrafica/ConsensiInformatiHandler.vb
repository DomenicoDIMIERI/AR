Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.WebSite
Imports DMD.Anagrafica
Imports DMD.Forms.Utils




Namespace Forms

 
    Public Class ConsensiInformatiHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New()
            Me.UseLocal = False
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New ConsensoInformatoCursor
        End Function


        Public Function GetConsensiByPersona(ByVal renderer As Object) As String
            Dim idp As Integer = RPC.n2int(GetParameter(renderer, "pid", "0"))
            Dim items As ConsensoInformatoColleciton = Anagrafica.ConsensiInformati.GetConsensiByPersona(idp)
            Return XML.Utils.Serializer.Serialize(items)
        End Function

    End Class


End Namespace
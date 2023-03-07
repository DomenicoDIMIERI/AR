Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.Office
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.Web

Namespace Forms

    Public Class ContiCorrenteHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SEdit Or ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SDuplicate)
            Me.UseLocal = True
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New ContoCorrenteCursor
        End Function

        Public Function Find(ByVal renderer As Object) As String
            Dim text As String = RPC.n2str(GetParameter(renderer, "text", ""))
            Return XML.Utils.Serializer.Serialize(Anagrafica.ContiCorrente.Find(text))
        End Function

    End Class


End Namespace
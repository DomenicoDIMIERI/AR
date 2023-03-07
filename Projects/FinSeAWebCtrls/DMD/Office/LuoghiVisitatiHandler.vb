Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.Office
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.Web

Namespace Forms

    Public Class LuoghiVisitatiHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SEdit Or ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SDuplicate)
            Me.UseLocal = True
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New LuoghiVisitatiCursor
        End Function

        Public Function GetLuoghiVisitati(ByVal renderer As Object) As String
            Dim lid As Integer = RPC.n2int(GetParameter(renderer, "lid", "0"))
            Dim uscita As Uscita = Office.Uscite.GetItemById(lid)
            Return XML.Utils.Serializer.Serialize(uscita.LuoghiVisitati)
        End Function



    End Class


End Namespace
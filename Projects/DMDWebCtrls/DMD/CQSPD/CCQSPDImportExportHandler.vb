Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.CQSPD
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.Forms.Utils
Imports DMD.CustomerCalls

Namespace Forms


    Public Class CCQSPDImportExportHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SEdit Or ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SExport)
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CImportExportCursor()
        End Function

        Public Overrides Function GetInternalItemById(id As Integer) As Object
            Return CQSPD.ImportExport.GetItemById(id)
        End Function


        Public Function Esporta(ByVal renderer As Object) As String
            Dim item As CImportExport = XML.Utils.Serializer.Deserialize(RPC.n2str(GetParameter(renderer, "item", "")))
            item.Esporta()
            Return XML.Utils.Serializer.Serialize(item)
        End Function

        Public Function ConfermaEsportazione(ByVal renderer As Object) As String
            Dim item As CImportExport = XML.Utils.Serializer.Deserialize(RPC.n2str(GetParameter(renderer, "item", "")))
            item.ConfermaEsportazione()
            Return XML.Utils.Serializer.Serialize(item)
        End Function

        Public Function Importa(ByVal renderer As Object) As String
            Dim item As CImportExport = XML.Utils.Serializer.Deserialize(RPC.n2str(GetParameter(renderer, "item", "")))
            item.Importa()
            Return XML.Utils.Serializer.Serialize(item)
        End Function

        Public Function PrendiInCarico(ByVal renderer As Object) As String
            Dim item As CImportExport = XML.Utils.Serializer.Deserialize(RPC.n2str(GetParameter(renderer, "item", "")))
            item.PrendiInCarico()
            Return XML.Utils.Serializer.Serialize(item)
        End Function

        Public Function Sincronizza(ByVal renderer As Object) As String
            Dim item As CImportExport = XML.Utils.Serializer.Deserialize(RPC.n2str(GetParameter(renderer, "item", "")))
            Dim oggetti As CCollection = XML.Utils.Serializer.Deserialize(RPC.n2str(GetParameter(renderer, "oggetti", "")))
            item.Sincronizza(oggetti)
            Return XML.Utils.Serializer.Serialize(item)
        End Function

        Public Function Sollecita(ByVal renderer As Object) As String
            Dim item As CImportExport = XML.Utils.Serializer.Deserialize(RPC.n2str(GetParameter(renderer, "item", "")))
            item.Sollecita()
            Return XML.Utils.Serializer.Serialize(item)
        End Function

    End Class

End Namespace
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

    Public Class MotiviCommissioniHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SEdit Or ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SDuplicate)
            Me.UseLocal = True
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New MotivoCommissioneCursor
        End Function



        Public Overrides Function GetExportableColumnsList() As CCollection(Of ExportableColumnInfo)
            Dim ret As CCollection(Of ExportableColumnInfo) = MyBase.GetExportableColumnsList()
            ret.Add(New ExportableColumnInfo("ID", "ID", TypeCode.Int32, True))
            ret.Add(New ExportableColumnInfo("Motivo", "Motivo", TypeCode.String, True))
            Return ret
        End Function


        Public Function GetMotiviAsCollection(ByVal renderer As Object) As String
            Return XML.Utils.Serializer.Serialize(Office.MotiviCommissioni.GetMotiviAsCollection, XMLSerializeMethod.Document)
        End Function
    End Class


End Namespace
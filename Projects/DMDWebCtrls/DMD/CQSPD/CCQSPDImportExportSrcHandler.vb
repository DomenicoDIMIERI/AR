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


    Public Class CCQSPDImportExportSrcHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SEdit Or ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SExport)
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CImportExportSourceCursor()
        End Function

        Public Overrides Function GetInternalItemById(id As Integer) As Object
            Return CQSPD.ImportExportSources.GetItemById(id)
        End Function

    End Class

End Namespace
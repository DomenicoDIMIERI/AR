Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.CQSPD
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.Forms.Utils
Imports DMD.Office

Namespace Forms


    Public Class DocumentiContabiliHandler
        Inherits CBaseModuleHandler

        Public Sub New()
        End Sub

        Public Overrides Function GetInternalItemById(id As Integer) As Object
            Return Office.DocumentiContabili.GetItemById(id)
        End Function

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New DocumentoContabileCursor
        End Function

    End Class

End Namespace
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


    Public Class UtenzeHandler
        Inherits CBaseModuleHandler

        Public Sub New()
        End Sub

        Public Overrides Function GetInternalItemById(id As Integer) As Object
            Return Office.Utenze.GetItemById(id)
        End Function

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New UtenzeCursor
        End Function

    End Class

End Namespace
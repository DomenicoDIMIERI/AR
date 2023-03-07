Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.Forms.Utils
Imports DMD.Office

Namespace Forms


    Public Class SpedizioniHandler
        Inherits CBaseModuleHandler

        Public Sub New()
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New SpedizioniCursor
        End Function


        Public Overrides Function GetInternalItemById(id As Integer) As Object
            Return Office.Spedizioni.GetItemById(id)
        End Function

        Public Function GetNomiCorrieri() As String
            Return ""
        End Function
    End Class

End Namespace
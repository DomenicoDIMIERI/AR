Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.WebSite
Imports DMD.Anagrafica
Imports DMD.Forms.Utils
Imports DMD.Office



Namespace Forms


    Public Class TurniIOHandler
        Inherits CBaseModuleHandler

        Public Sub New()
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New TurniCursor
        End Function

        Public Overrides Function GetInternalItemById(id As Integer) As Object
            Return Office.Turni.GetItemById(id)
        End Function

    End Class


End Namespace
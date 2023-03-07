Imports DMD
Imports DMD.Sistema
Imports DMD.Forms
Imports DMD.WebSite
Imports DMD.Databases
Imports DMD.Forms.Utils

Imports DMD.Anagrafica

Namespace Forms

    Public Class CVociManutenzioniModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            Me.UseLocal = True
        End Sub

        Public Sub New(ByVal [module] As CModule)
            Me.New()
            Me.SetModule([module])
        End Sub

        Public Overrides Function GetInternalItemById(id As Integer) As Object
            'Return MyBase.GetItemById(id)
            Return Anagrafica.Manutenzioni.Voci.GetItemById(id)
        End Function


        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New VociManutenzioneCursor
        End Function
    End Class


End Namespace
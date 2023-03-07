Imports DMD.Anagrafica
Imports DMD.Databases

Namespace Forms

    Public Class TipiRapportoModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SDuplicate Or ModuleSupportFlags.SEdit)
            Me.UseLocal = True
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CTipoRapportoCursor
        End Function

        Public Overrides Function GetInternalItemById(id As Integer) As Object
            Return Anagrafica.TipiRapporto.GetItemById(id)
        End Function

    End Class



End Namespace
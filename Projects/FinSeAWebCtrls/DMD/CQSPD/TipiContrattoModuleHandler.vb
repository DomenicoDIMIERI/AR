Imports DMD.CQSPD
Imports DMD.Databases

Namespace Forms

    Public Class TipiContrattoModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SDuplicate Or ModuleSupportFlags.SEdit)
            Me.UseLocal = True
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CTipoContrattoCursor
        End Function



        Public Overrides Function GetInternalItemById(id As Integer) As Object
            Return CQSPD.TipiContratto.GetItemById(id)
        End Function

    End Class



End Namespace
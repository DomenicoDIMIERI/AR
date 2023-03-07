Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.WebSite
Imports DMD.Anagrafica
Imports DMD.Forms.Utils




Namespace Forms


    Public Class AzioniTasksLavorazioneHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SDuplicate Or ModuleSupportFlags.SEdit)
            Me.UseLocal = True
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New AzioneTaskLavorazioneCursor
        End Function

        Public Overrides Function GetEditor() As Object
            Return New AzioniTasksLavorazioneEditor(Me)
        End Function



    End Class


End Namespace
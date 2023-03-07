Imports DMD
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.WebSite

Imports DMD.Databases

Imports DMD.ADV
Imports DMD.Web

Namespace Forms

    Public Class ADVResModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SDuplicate Or ModuleSupportFlags.SEdit)
            Me.UseLocal = True
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CRisultatoCampagnaCursor
        End Function




    End Class


End Namespace
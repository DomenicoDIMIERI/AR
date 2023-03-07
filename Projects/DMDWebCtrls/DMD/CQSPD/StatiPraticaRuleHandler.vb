Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.CQSPD
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.Web

Namespace Forms

    Public Class StatiPraticaRuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            Me.UseLocal = True
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CStatoPratRuleCursor
        End Function

        Public Overrides Function GetInternalItemById(id As Integer) As Object
            Return CQSPD.StatiPratRules.GetItemById(id)
        End Function

    End Class


End Namespace
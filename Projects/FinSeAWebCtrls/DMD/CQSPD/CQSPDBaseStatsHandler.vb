Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.CQSPD
Imports DMD.Sistema
Imports DMD.Anagrafica

Namespace Forms

 
    Public Class CQSPDBaseStatsHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New()
            Me.UseLocal = False
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CRapportiniCursor
        End Function


    End Class


 
End Namespace
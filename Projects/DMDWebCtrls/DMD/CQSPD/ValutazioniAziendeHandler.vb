Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.CQSPD
Imports DMD.Sistema
Imports DMD.Anagrafica

Namespace Forms




    Public Class ValutazioniAziendeHandler
        Inherits CBaseModuleHandler

        Public Sub New()
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CQSPDValutazioneAziendaCursor
        End Function


    End Class


End Namespace
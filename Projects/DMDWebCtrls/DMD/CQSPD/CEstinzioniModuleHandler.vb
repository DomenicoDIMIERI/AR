Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.CQSPD
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.XML

Namespace Forms

    Public Class CEstinzioniModuleHandler
        Inherits CBaseModuleHandler





        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CEstinzioniCursor
        End Function


    End Class


End Namespace
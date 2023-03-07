Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.CQSPD
Imports DMD.Sistema
Imports DMD.Anagrafica

Namespace Forms
  

    Public Class CAmministrazioniModuleHandler
        Inherits CPersoneGiuridicheModuleHandler

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Dim ret As New CPersonaCursor
            ret.TipoPersona.Value = TipoPersona.PERSONA_GIURIDICA
            Return ret
        End Function




    End Class


End Namespace
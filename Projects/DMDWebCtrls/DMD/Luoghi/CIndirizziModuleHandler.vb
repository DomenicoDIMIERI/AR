Imports DMD
Imports DMD.Sistema
Imports DMD.WebSite

Imports DMD.Forms
Imports DMD.Anagrafica
Imports DMD.Databases
Imports DMD.Forms.Utils

Namespace Forms
 
  
    Public Class CIndirizziModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CIndirizziCursor
        End Function




    End Class

 
End Namespace
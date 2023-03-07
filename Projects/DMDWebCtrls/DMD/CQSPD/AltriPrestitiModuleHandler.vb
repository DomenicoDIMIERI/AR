Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.CQSPD
Imports DMD.Sistema
Imports DMD.Anagrafica

Namespace Forms
 
 
   

    Public Class AltriPrestitiModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CAltriPrestitiCursor
        End Function

        'Public Overrides Function GetEditor() As Object
        '    Return New AltriPrestitiEditor
        'End Function
          
    End Class


End Namespace
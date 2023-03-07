Imports DMD
Imports DMD.Sistema
Imports DMD.Forms
Imports DMD.WebSite
Imports DMD.Databases
Imports DMD.Forms.Utils



Namespace Forms

   
 
    Public Class CAnnotazioniModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SDuplicate Or ModuleSupportFlags.SEdit Or ModuleSupportFlags.SExport Or ModuleSupportFlags.SImport)
            Me.UseLocal = True
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CAnnotazioniCursor
        End Function

    End Class
 
    
End Namespace
Imports DMD
Imports DMD.Sistema
Imports DMD.WebSite
Imports DMD.CustomerCalls
Imports DMD.Forms
Imports DMD.Anagrafica
Imports DMD.Databases
Imports DMD.Forms.Utils

Imports DMD.Office


Namespace Forms

 

    Public Class CVisiteModuleHandler
        Inherits CBaseModuleHandler



        Public Sub New()
            Me.UseLocal = True
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CVisiteCursor
        End Function


        Public Overrides Function GetInternalItemById(id As Integer) As Object
            Return CustomerCalls.CRM.GetItemById(id)
        End Function
         
    End Class


End Namespace
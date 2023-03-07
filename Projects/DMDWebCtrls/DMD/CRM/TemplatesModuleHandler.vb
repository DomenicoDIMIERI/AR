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

 

    Public Class TemplatesModuleHandler
        Inherits CBaseModuleHandler



        Public Sub New()
            Me.UseLocal = True
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CustomerCalls.CTemplatesCursor
        End Function

         
        Public Overrides Function GetInternalItemById(id As Integer) As Object
            Return CustomerCalls.Templates.GetItemById(id)
        End Function

        'Public Overrides Function GetItemByName() As String
        '    Return CustomerCalls.Templates.GetItemById(id)
        'End Function
         
    End Class


End Namespace
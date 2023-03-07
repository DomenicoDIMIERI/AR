Imports DMD
Imports DMD.Sistema
Imports DMD.Forms
Imports DMD.WebSite
Imports DMD.Databases
Imports DMD.Forms.Utils


Imports DMD.Web

Namespace Forms

    '-------------------------------------------------------
    Public Class CCalendarProcsHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SDuplicate Or ModuleSupportFlags.SEdit)
            Me.UseLocal = True
        End Sub

        Public Overrides Function GetInternalItemById(ByVal id As Integer) As Object
            Return Sistema.Procedure.GetItemById(id)
        End Function

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CProcedureCursor
        End Function

    End Class


End Namespace
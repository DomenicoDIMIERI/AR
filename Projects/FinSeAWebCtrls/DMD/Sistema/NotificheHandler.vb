Imports DMD
Imports DMD.Sistema
Imports DMD.Forms
Imports DMD.WebSite
Imports DMD.Databases
Imports DMD.Forms.Utils



Namespace Forms


    '--------------------------------------------------------
    Public Class NotificheHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SDuplicate Or ModuleSupportFlags.SEdit)
        End Sub

        Public Overrides Function GetInternalItemById(ByVal id As Integer) As Object
            Return Sistema.Notifiche.GetItemById(id)
        End Function

        Public Overrides Function CreateCursor() As Databases.DBObjectCursorBase
            Return New NotificaCursor
        End Function

    End Class

    


End Namespace
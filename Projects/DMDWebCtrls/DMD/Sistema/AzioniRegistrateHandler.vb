Imports DMD
Imports DMD.Sistema
Imports DMD.Forms
Imports DMD.WebSite
Imports DMD.Databases
Imports DMD.Forms.Utils



Namespace Forms


    '--------------------------------------------------------
    Public Class AzioniRegistrateHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SDuplicate Or ModuleSupportFlags.SEdit)
        End Sub

        Public Overrides Function GetInternalItemById(ByVal id As Integer) As Object
            Return Sistema.Notifiche.RegisteredActions.GetItemById(id)
        End Function

        Public Overrides Function CreateCursor() As Databases.DBObjectCursorBase
            Return New AzioneRegistrataCursor
        End Function



        'Public Overrides Function delete() As String
        '    Dim item As AzioneRegistrata
        '    Dim itemID As Integer = RPC.n2int(Me.GetParameter(renderer, "ID"))
        '    Dim ret As String
        '    item = Sistema.Notifiche.RegisteredActions.GetItemById(itemID)
        '    ret = MyBase.delete()
        '    'Sistema.PropertyPages.CachedItems.Remove(item)
        '    Return ret
        'End Function


    End Class

    


End Namespace
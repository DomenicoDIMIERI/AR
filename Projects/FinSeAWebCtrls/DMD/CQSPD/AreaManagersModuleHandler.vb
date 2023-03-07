Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.CQSPD
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.Forms.Utils

Namespace Forms


#Region "Area Managers"


    Public Class AreaManagersModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SEdit)
            Me.UseLocal = True
        End Sub



        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CAreaManagerCursor
        End Function

    End Class


#End Region


End Namespace
Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.Office
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.Web

Namespace Forms

    Public Class LicenzeSoftwarehandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SEdit Or ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SDuplicate)
            Me.UseLocal = True
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New LicenzeSoftwareCursor
        End Function



    End Class


End Namespace
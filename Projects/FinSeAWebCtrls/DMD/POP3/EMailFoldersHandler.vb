Imports DMD
Imports DMD.Sistema
Imports DMD.Forms
Imports DMD.Office
Imports DMD.WebSite
Imports DMD.Anagrafica

Imports DMD.Databases
Imports DMD.Net

Namespace Forms



    Public Class EMailFoldersHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            Me.UseLocal = True
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New DMD.Office.MailFolderCursor
        End Function


    End Class



End Namespace
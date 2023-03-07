Imports DMD
Imports DMD.WebSite
Imports DMD.Sistema
Imports DMD.Forms
Imports DMD.Messenger
Imports DMD.XML

Namespace Forms

    Public Class CMessengerModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
        End Sub

        Public Overrides Function CreateCursor() As Databases.DBObjectCursorBase
            Return New CMessagesCursor
        End Function

    End Class

End Namespace
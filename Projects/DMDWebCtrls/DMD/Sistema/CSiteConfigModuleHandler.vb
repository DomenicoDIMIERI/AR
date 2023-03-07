Imports DMD
Imports DMD.WebSite
Imports DMD.Sistema
Imports DMD.Databases

Namespace Forms


    Public Class CSiteConfigModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
        End Sub

        Public Overrides Function List(ByVal renderer As Object) As String
            'Return Me.InternalEdit(renderer, WebSite.Instance.Configuration)
            Dim ret As String = "" 'Me.list(renderer)
            ret &= "<script type=""text/javascript"">" & vbNewLine
            ret &= "Window.addListener(""onload"", new Function('setTimeout(SystemUtils.EditItem(WebSite.getConfiguration()), 500)'));"
            ret &= "</script>"
            Return ret
        End Function

        Public Overrides Function GetInternalItemById(id As Integer) As Object
            Return WebSite.Instance.Configuration
        End Function

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return Nothing
        End Function

        Public Function CreateBackup(ByVal renderer As Object) As String
            Dim fd As Nullable(Of Date) = RPC.n2date(GetParameter(renderer, "fd", ""))
            Dim bk As CBackup = Backups.Create(fd, Sistema.Backups.Configuration.CompressionMethod, Sistema.Backups.Configuration.CompressionLevel)
            Return bk.Name
        End Function

        Public Function RestoreBackup(ByVal renderer As Object) As String
            Backups.Restore(Me.GetParameter(renderer, "id", vbNullString))
            Return vbNullString
        End Function

    End Class


End Namespace
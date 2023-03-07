Public Class frmUploadServer

    Private Sub frmUploadServer_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            Me.txtURL.Text = DIALTPLib.DialTPApp.CurrentConfig.UploadServer
            'Me.txtIntervallo.Text = DIALTPLib.DialTPApp.CurrentConfig ..UploadInterval
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        Try
            DIALTPLib.DialTPApp.CurrentConfig.UploadServer = Strings.Trim(Me.txtURL.Text)
            'DIALTPLib.Settings.UploadInterval = CInt(Me.txtIntervallo.Text)
            'DIALTPLib.Log.Reset()
            'My.Settings.Save()
            Me.Close()
        Catch ex As Exception
            DMD.Sistema.Events.NotifyUnhandledException(ex)
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
End Class
Public Class frmConfig

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)

        Me.txtCheckTimes.Text = APPSettings.CheckTimes
        Me.txtUploadServer.Text = APPSettings.UploadServer
        Me.txtAutoSync.Value = APPSettings.AutoUploadTime
    End Sub

    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        APPSettings.CheckTimes = Me.txtCheckTimes.Text
        APPSettings.UploadServer = Me.txtUploadServer.Text
        APPSettings.AutoUploadTime = Me.txtAutoSync.Value
        APPSettings.Save()
    End Sub

    Private Sub btnSync_Click(sender As Object, e As EventArgs) Handles btnSync.Click

    End Sub

    Private Sub btnUpload_Click(sender As Object, e As EventArgs) Handles btnUpload.Click

    End Sub
End Class
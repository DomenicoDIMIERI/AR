Public Class MailSettings

    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        Try
            DMD.Sistema.EMailer.Config.SMTPUserName = Me.txtUserName.Text
            DMD.Sistema.EMailer.Config.SMTPPassword = Me.txtPassword.Text
            DMD.Sistema.EMailer.Config.SMTPServer = Me.txtServerName.Text
            DMD.Sistema.EMailer.Config.SMTPServerPort = Me.txtServerPort.Text
            DMD.Sistema.EMailer.Config.SMTPUseSSL = Me.chkSSL.Checked

            DIALTPLib.Settings.SMTPServer = Me.txtServerName.Text
            DIALTPLib.Settings.SMTPPort = Me.txtServerPort.Text
            DIALTPLib.Settings.SMTPUserName = Me.txtUserName.Text
            DIALTPLib.Settings.SMTPPassword = Me.txtPassword.Text
            DIALTPLib.Settings.SMTPDisplayName = Me.txtDisplayName.Text
            DIALTPLib.Settings.SMTPSubject = Me.txtSubject.Text
            DIALTPLib.Settings.SMTPSSL = Me.chkSSL.Checked
            My.Settings.Save()
            Me.Close()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub MailSettings_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.txtServerName.Text = DIALTPLib.Settings.SMTPServer
        Me.txtServerPort.Text = DIALTPLib.Settings.SMTPPort
        Me.txtUserName.Text = DIALTPLib.Settings.SMTPUserName
        Me.txtPassword.Text = DIALTPLib.Settings.SMTPPassword
        Me.txtDisplayName.Text = DIALTPLib.Settings.SMTPDisplayName
        Me.txtSubject.Text = DIALTPLib.Settings.SMTPSubject
        Me.chkSSL.Checked = DIALTPLib.Settings.SMTPSSL
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
End Class
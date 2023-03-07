Public Class SMTPSettings

    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        Try
            DMD.Sistema.EMailer.Config.SMTPUserName = Me.txtUserName.Text
            DMD.Sistema.EMailer.Config.SMTPPassword = Me.txtPassword.Text
            DMD.Sistema.EMailer.Config.SMTPServer = Me.txtServerName.Text
            DMD.Sistema.EMailer.Config.SMTPServerPort = CInt(Me.txtServerPort.Text)
            DMD.Sistema.EMailer.Config.SMTPUseSSL = Me.chkSSL.Checked

            FaxSvrSettings.SMTPServer = Me.txtServerName.Text
            FaxSvrSettings.SMTPPort = CInt(Me.txtServerPort.Text)
            FaxSvrSettings.SMTPUserName = Me.txtUserName.Text
            FaxSvrSettings.SMTPPassword = Me.txtPassword.Text
            FaxSvrSettings.SMTPDisplayName = Me.txtDisplayName.Text
            FaxSvrSettings.SMTPSubject = Me.txtSubject.Text
            FaxSvrSettings.SMTPSSL = Me.chkSSL.Checked
            '            My.Settings.Save()
            Me.Close()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub MailSettings_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.txtServerName.Text = FaxSvrSettings.SMTPServer
        Me.txtServerPort.Text = CStr(FaxSvrSettings.SMTPPort)
        Me.txtUserName.Text = FaxSvrSettings.SMTPUserName
        Me.txtPassword.Text = FaxSvrSettings.SMTPPassword
        Me.txtDisplayName.Text = FaxSvrSettings.SMTPDisplayName
        Me.txtSubject.Text = FaxSvrSettings.SMTPSubject
        Me.chkSSL.Checked = FaxSvrSettings.SMTPSSL
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
End Class
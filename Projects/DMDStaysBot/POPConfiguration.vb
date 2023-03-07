Public Class POPConfiguration

    
    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        Try
            

            DMD.Sistema.EMailer.Config.POP3Enabled = Me.txtChecEvery.Value > 0
            DMD.Sistema.EMailer.Config.CheckInterval = CInt(Me.txtChecEvery.Value)
            DMD.Sistema.EMailer.Config.POPServer = Me.txtServerName.Text
            DMD.Sistema.EMailer.Config.POPPort = CInt(Me.txtServerPort.Text)
            DMD.Sistema.EMailer.Config.POPUserName = Me.txtUserName.Text
            DMD.Sistema.EMailer.Config.POPPassword = Me.txtPassword.Text
            DMD.Sistema.EMailer.Config.POPUseSSL = Me.chkSSL.Checked

            My.Settings.POP3Server = Me.txtServerName.Text
            My.Settings.POP3Port = CInt(Me.txtServerPort.Text)
            My.Settings.POP3UserName = Me.txtUserName.Text
            My.Settings.POP3Password = Me.txtPassword.Text
            My.Settings.POP3SSL = Me.chkSSL.Checked
            My.Settings.POP3CheckEvery = CInt(Me.txtChecEvery.Value)
            My.Settings.UploadToPage = Trim(Me.txtRemoteServer.Text)
            My.Settings.Save()

            



            Me.Close()
        Catch ex As Exception
            DMD.Sistema.Events.NotifyUnhandledException(ex)
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub POPConfiguration_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            Me.txtServerName.Text = My.Settings.POP3Server
            Me.txtServerPort.Text = CStr(My.Settings.POP3Port)
            Me.txtUserName.Text = My.Settings.POP3UserName
            Me.txtPassword.Text = My.Settings.POP3Password
            Me.chkSSL.Checked = My.Settings.POP3SSL
            Me.txtChecEvery.Value = My.Settings.POP3CheckEvery
            Me.txtRemoteServer.Text = My.Settings.UploadToPage
        Catch ex As Exception
            DMD.Sistema.Events.NotifyUnhandledException(ex)
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
End Class
Imports DMD
Imports DMD.Sistema
Imports DMD.Drivers

Public Class HylaFaxConfig

    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        Try

            FaxSvrSettings.HylaFaxServerName = Me.txtServerName.Text
            FaxSvrSettings.HylaFaxServerPort = CInt(Me.txtPort.Text)
            FaxSvrSettings.HylaFaxUserName = Me.txtUserName.Text
            FaxSvrSettings.HylaFaxPassword = Me.txtPassword.Text
            FaxSvrSettings.HylafaxPrefix = Me.txtPrefisso.Text
            My.Settings.Save()

            Dim driver As HylaFaxDriver = MyApplicationContext.GetHylaFaxDriver
            If driver.IsConnected Then driver.Disconnect()
            With driver.Modems(0)
                .ServerName = FaxSvrSettings.HylaFaxServerName
                .ServerPort = FaxSvrSettings.HylaFaxServerPort
                .UserName = FaxSvrSettings.HylaFaxUserName
                .Password = FaxSvrSettings.HylaFaxPassword
                .DialPrefix = FaxSvrSettings.HylafaxPrefix
                .eMailRicezione = DMD.Sistema.EMailer.Config.POPUserName
            End With
            driver.SetDefaultOptions(driver.Config)
            driver.Connect()

            Me.Close()
        Catch ex As Exception
            Sistema.Events.NotifyUnhandledException(ex)
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub HylaFaxConfig_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            Me.txtServerName.Text = FaxSvrSettings.HylaFaxServerName
            Me.txtPort.Text = CStr(FaxSvrSettings.HylaFaxServerPort)
            Me.txtUserName.Text = FaxSvrSettings.HylaFaxUserName
            Me.txtPassword.Text = FaxSvrSettings.HylaFaxPassword
            Me.txtPrefisso.Text = FaxSvrSettings.HylafaxPrefix
        Catch ex As Exception
            Sistema.Events.NotifyUnhandledException(ex)
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
End Class
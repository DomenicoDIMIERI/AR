Imports DMD
Imports DMD.Sistema
Imports DMD.Drivers

Public Class HylaFaxConfig

    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        For Each driver As BaseFaxDriver In Sistema.FaxService.GetInstalledDrivers
            If (TypeOf (driver) Is HylaFaxDriver) Then
                If driver.IsConnected Then driver.Disconnect()
                With DirectCast(driver.Config, HylafaxDriverConfiguration)
                    .HostName = DIALTPLib.DialTPApp.CurrentConfig.HylafaxConfig.HylafaxServer
                    .HostPort = DIALTPLib.DialTPApp.CurrentConfig.HylafaxConfig.HylafaxPort
                    .UserName = DIALTPLib.DialTPApp.CurrentConfig.HylafaxConfig.HylafaxUserName
                    .Password = DIALTPLib.DialTPApp.CurrentConfig.HylafaxConfig.HylafaxPassword
                    .DialPrefix = DIALTPLib.DialTPApp.CurrentConfig.HylafaxConfig.HylafaxDialPrefix

                End With
                driver.Connect()
                Exit For
            End If
        Next

        DIALTPLib.DialTPApp.CurrentConfig.HylafaxConfig.HylafaxServer = Me.txtServerName.Text
        DIALTPLib.DialTPApp.CurrentConfig.HylafaxConfig.HylafaxPort = Me.txtPort.Text
        DIALTPLib.DialTPApp.CurrentConfig.HylafaxConfig.HylafaxUserName = Me.txtUserName.Text
        DIALTPLib.DialTPApp.CurrentConfig.HylafaxConfig.HylafaxPassword = Me.txtPassword.Text
        DIALTPLib.DialTPApp.CurrentConfig.HylafaxConfig.HylafaxDialPrefix = Me.txtDialPrefix.Text
        DIALTPLib.DialTPApp.CurrentConfig.HylafaxConfig.HylafaxUploadTo = Me.txtUploadServer.Text

        Me.Close()
    End Sub

    Private Sub HylaFaxConfig_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            Me.txtServerName.Text = DIALTPLib.DialTPApp.CurrentConfig.HylafaxConfig.HylafaxServer
            Me.txtPort.Text = DIALTPLib.DialTPApp.CurrentConfig.HylafaxConfig.HylafaxPort
            Me.txtUserName.Text = DIALTPLib.DialTPApp.CurrentConfig.HylafaxConfig.HylafaxUserName
            Me.txtPassword.Text = DIALTPLib.DialTPApp.CurrentConfig.HylafaxConfig.HylafaxPassword
            Me.txtDialPrefix.Text = DIALTPLib.DialTPApp.CurrentConfig.HylafaxConfig.HylafaxDialPrefix
            Me.txtUploadServer.Text = DIALTPLib.DialTPApp.CurrentConfig.HylafaxConfig.HylafaxUploadTo
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try

    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
End Class
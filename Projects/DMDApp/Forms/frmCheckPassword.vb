Public Class frmCheckPassword

    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        Try
            Me.Confirm()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
            DMD.Sistema.Events.NotifyUnhandledException(ex)
        End Try
    End Sub

    Private Sub frmCheckPassword_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            Me.txtPassword.Text = ""
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Confirm()
        If Me.txtPassword.Text = DIALTPLib.Settings.APPPassword Then
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        Else
            Me.txtPassword.Text = ""
            Beep()
        End If
    End Sub

    Private Sub txtPassword_KeyUp(sender As Object, e As KeyEventArgs) Handles txtPassword.KeyUp
        If e.KeyCode = 13 Then Me.Confirm()
    End Sub

    
End Class
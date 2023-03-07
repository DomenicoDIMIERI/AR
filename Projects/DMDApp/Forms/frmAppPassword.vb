Public Class frmAppPassword

    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        Try
            If Me.txtPassword.Text <> DIALTPLib.Settings.APPPassword Then Throw New Exception("Password non valida")
            If Me.txtNewPassword.Text <> Me.txtNewPassword1.Text Then Throw New Exception("La nuova password non coincide")
            DIALTPLib.Settings.APPPassword = Me.txtNewPassword.Text
            My.Settings.Save()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

     
End Class
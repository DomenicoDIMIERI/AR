Public Class frmLogin

    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        Try
            Dim username As String = Me.txtUserName.Text
            Dim password As String = Me.txtPassword.Text

            DIALTPLib.Remote.Login(username, password)

            DIALTPLib.DialTPApp.CurrentConfig.UserName = username
            'DIALTPLib.DialTPApp.CurrentConfig.

            '  DIALTPLib.Settings.Save()
            Me.Close()
        Catch ex As Exception
            DMD.Sistema.Events.NotifyUnhandledException(ex)
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub frmServers_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            Me.txtUserName.Text = DIALTPLib.DialTPApp.CurrentConfig.UserName
            Dim items As String() = Split(DIALTPLib.Settings.ServersList, ";")
            Me.cboServer.Items.Clear()
            If (items IsNot Nothing AndAlso items.Length > 0) Then
                Array.Sort(items)
                For Each item As String In items
                    If (Trim(item) <> "") Then Me.cboServer.Items.Add(item)
                Next
            End If
            Me.cboServer.Text = DIALTPLib.DialTPApp.CurrentConfig.ServerName
        Catch ex As Exception
            DMD.Sistema.Events.NotifyUnhandledException(ex)
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
End Class
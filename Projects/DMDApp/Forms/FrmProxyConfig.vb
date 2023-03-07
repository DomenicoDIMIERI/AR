Imports DMD

Public Class FrmProxyConfig

    Private Sub FrmProxyConfig_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Me.chkEnabled.Checked = True 'DIALTPLib.Settings.PROXYENABLED
            'Me.txtBoundIP.Text = DIALTP.
            'Me.txtBoundPort.Text = DIALTPLib.Settings.PROXYPORT
            'Me.txtTimeOut.Text = DIALTPLib.Settings.PROXYTIMEOUT
        Catch ex As Exception
            Sistema.Events.NotifyUnhandledException(ex)
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            Me.Applica()
        Catch ex As Exception
            Sistema.Events.NotifyUnhandledException(ex)
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        Try
            Me.Applica()
        Catch ex As Exception
            Sistema.Events.NotifyUnhandledException(ex)
            MsgBox(ex.Message)
        End Try
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Public Sub Applica()
        ' DIALTPLib.Settings.PROXYENABLED = Me.chkEnabled.checked
        'DIALTPLib.Settings.PROXYIP = Trim(Me.txtBoundIP.Text)
        'DIALTPLib.Settings.PROXYPORT = CInt(Me.txtBoundPort.Text)
        'DIALTPLib.Settings.PROXYTIMEOUT = CInt(Me.txtTimeOut.Text)
        'DIALTPLib.Settings.Save()
        If Me.chkEnabled.Checked Then
            DIALTPLib.ProxyService.Restart()
        Else
            DIALTPLib.ProxyService.Stop()
        End If
    End Sub
End Class
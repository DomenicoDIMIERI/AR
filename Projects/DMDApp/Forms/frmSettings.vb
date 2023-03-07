Public Class frmSettings

    Private m_Loading As Boolean

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub frmSettings_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.m_Loading = True
        Me.chkAutoStat.Checked = DIALTPLib.Settings.AutoStart
        Me.chkRegisterDialtp.Checked = DIALTPLib.Settings.RegisterDialtp
        Me.m_Loading = False
    End Sub

    Private Sub chkRegisterDialtp_CheckedChanged(sender As Object, e As EventArgs) Handles chkRegisterDialtp.CheckedChanged, chkAutoStat.CheckedChanged
        If (Me.m_Loading) Then Return 
        DIALTPLib.Settings.RegisterDialtp = Me.chkRegisterDialtp.Checked
        DIALTPLib.Settings.AutoStart = Me.chkAutoStat.Checked

        Utils.CheckProtocol()
        Utils.CheckAutostart()
    End Sub

End Class
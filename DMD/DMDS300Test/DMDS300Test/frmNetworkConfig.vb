Public Class frmNetworkConfig

    Private m_Config As DMD.S300.CKT_DLL.NETINFO

    Public Property Config As DMD.S300.CKT_DLL.NETINFO
        Get
            Return Me.m_Config
        End Get
        Set(value As DMD.S300.CKT_DLL.NETINFO)
            Me.m_Config = value
            Me.Refill
        End Set
    End Property

    Public Sub Refill()
        Me.IP.SetIP(Me.m_Config.IP)
        Me.Mask.SetIP(Me.m_Config.Mask)
        Me.Gateway.SetIP(Me.m_Config.Gateway)
        Me.DNS.SetIP(Me.m_Config.ServerIP)
        Me.MAC.SetMAC(Me.m_Config.MAC)
    End Sub

    Public Sub Apply()
        Me.m_Config.IP = Me.IP.GetIP()
        Me.m_Config.Mask = Me.Mask.GetIP()
        Me.m_Config.Gateway = Me.Gateway.GetIP()
        Me.m_Config.ServerIP = Me.DNS.GetIP()
        Me.m_Config.MAC = Me.MAC.GetMAC()
    End Sub

    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        Me.Apply()
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub
End Class
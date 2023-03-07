Imports DMD.S300

Public Class frmDeviceConfiguration

    Private m_Config As S300Config
    Private m_Dev As S300Device

    Public Sub New()

        ' La chiamata è richiesta dalla finestra di progettazione.
        InitializeComponent()

        ' Aggiungere le eventuali istruzioni di inizializzazione dopo la chiamata a InitializeComponent().

    End Sub

    Public Property Device As S300Device
        Get
            Return Me.m_Dev
        End Get
        Set(value As S300Device)
            Me.m_Dev = value
            If (Me.Created) Then Me.Refill
        End Set
    End Property

    Public Sub Refill()
        Me.lblID.Text = ""
        Me.lblVersion.Text = ""
        Me.S300TimeSync1.Device = Nothing
        Me.lblNumClocks.Text = "."
        Me.lblNumFP.Text = "."
        Me.lblNumPersons.Text = "."
        Me.txtSpeakerVolume.Value = 0

        If (Me.m_Dev Is Nothing) Then Return
        Me.lblID.Text = Me.m_Dev.GetDeviceID
        Me.lblVersion.Text = Me.m_Dev.GetDeviceSoftwareVersion.ToString
        Me.S300TimeSync1.Device = Me.m_Dev

        Dim info As S300CountsInfo = Me.m_Dev.GetCounts
        Me.lblNumClocks.Text = info.ClockingsCounts
        Me.lblNumFP.Text = info.FingerPrintsCount
        Me.lblNumPersons.Text = info.PersonsCount

        Me.m_Config = Me.m_Dev.GetConfiguration
        Me.txtSpeakerVolume.Value = Me.m_Config.SpeakerVolume
        Me.txtDoorLockDelay.Value = Me.m_Config.LockDelayTime
        Me.chkRingAllow.Checked = Me.m_Config.RingAllow
        Me.chkRealtimeAllow.Checked = Me.m_Config.RealtimeMode
        Me.chkAutoUpdate.Checked = Me.m_Config.AutoUpdateFingerprint
        Me.txtFGAC.Value = Me.m_Config.FixedWiegandAreaCode
        Me.cboWGOption.SelectedIndex = Me.m_Config.WiegandOption
        Me.txtMinDelayM.Value = Me.m_Config.MinDelayInOut
        If Me.m_Config.AdminPassword <> "" Then Me.txtPassword.Value = CInt(Me.m_Config.AdminPassword)
    End Sub

    Public Sub Apply()
        Me.m_Config.SpeakerVolume = Me.txtSpeakerVolume.Value
        Me.m_Config.LockDelayTime = Me.txtDoorLockDelay.Value
        Me.m_Config.RingAllow = Me.chkRingAllow.Checked
        Me.m_Config.RealtimeMode = Me.chkRealtimeAllow.Checked
        Me.m_Config.AdminPassword = Me.txtPassword.Value
        Me.m_Config.AutoUpdateFingerprint = Me.chkAutoUpdate.Checked
        Me.m_Config.FixedWiegandAreaCode = Me.txtFGAC.Value
        Me.m_Config.WiegandOption = Me.cboWGOption.SelectedIndex
        Me.m_Config.MinDelayInOut = Me.txtMinDelayM.Value
        Me.m_Dev.SetConfiguration(Me.m_Config)
        '    Me.Refill()
    End Sub

    Protected Overrides Sub OnShown(e As EventArgs)
        MyBase.OnShown(e)
        Me.Refill()
    End Sub

    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        Try
            Me.Apply
            Me.DialogResult = DialogResult.OK
            Me.Close()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub


End Class
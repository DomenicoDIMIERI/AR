<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDeviceConfiguration
    Inherits System.Windows.Forms.Form

    'Form esegue l'override del metodo Dispose per pulire l'elenco dei componenti.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Richiesto da Progettazione Windows Form
    Private components As System.ComponentModel.IContainer

    'NOTA: la procedura che segue è richiesta da Progettazione Windows Form
    'Può essere modificata in Progettazione Windows Form.  
    'Non modificarla mediante l'editor del codice.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lblID = New System.Windows.Forms.Label()
        Me.lblVersion = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.lblNumClocks = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.lblNumFP = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.lblNumPersons = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.txtSpeakerVolume = New System.Windows.Forms.NumericUpDown()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnOk = New System.Windows.Forms.Button()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtDoorLockDelay = New System.Windows.Forms.NumericUpDown()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.GroupBox5 = New System.Windows.Forms.GroupBox()
        Me.chkAutoUpdate = New System.Windows.Forms.CheckBox()
        Me.chkRealtimeAllow = New System.Windows.Forms.CheckBox()
        Me.chkRingAllow = New System.Windows.Forms.CheckBox()
        Me.GroupBox6 = New System.Windows.Forms.GroupBox()
        Me.txtPassword = New System.Windows.Forms.NumericUpDown()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtFGAC = New System.Windows.Forms.NumericUpDown()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.cboWGOption = New System.Windows.Forms.ComboBox()
        Me.S300TimeSync1 = New DMDS300Test.S300TimeSync()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.txtMinDelayM = New System.Windows.Forms.NumericUpDown()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.txtSpeakerVolume, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox4.SuspendLayout()
        CType(Me.txtDoorLockDelay, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox5.SuspendLayout()
        Me.GroupBox6.SuspendLayout()
        CType(Me.txtPassword, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtFGAC, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtMinDelayM, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(20, 27)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(21, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "ID:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(20, 49)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(96, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Versione Software:"
        '
        'lblID
        '
        Me.lblID.AutoSize = True
        Me.lblID.ForeColor = System.Drawing.Color.Blue
        Me.lblID.Location = New System.Drawing.Point(127, 27)
        Me.lblID.Name = "lblID"
        Me.lblID.Size = New System.Drawing.Size(18, 13)
        Me.lblID.TabIndex = 2
        Me.lblID.Text = "ID"
        '
        'lblVersion
        '
        Me.lblVersion.AutoSize = True
        Me.lblVersion.ForeColor = System.Drawing.Color.Blue
        Me.lblVersion.Location = New System.Drawing.Point(128, 49)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(52, 13)
        Me.lblVersion.TabIndex = 3
        Me.lblVersion.Text = "lblVersion"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.S300TimeSync1)
        Me.GroupBox1.Location = New System.Drawing.Point(268, 8)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(343, 81)
        Me.GroupBox1.TabIndex = 4
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Orologio di sistema:"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.lblNumClocks)
        Me.GroupBox2.Controls.Add(Me.Label8)
        Me.GroupBox2.Controls.Add(Me.lblNumFP)
        Me.GroupBox2.Controls.Add(Me.Label6)
        Me.GroupBox2.Controls.Add(Me.lblNumPersons)
        Me.GroupBox2.Controls.Add(Me.Label4)
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Controls.Add(Me.lblID)
        Me.GroupBox2.Controls.Add(Me.lblVersion)
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Location = New System.Drawing.Point(1, 2)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(261, 188)
        Me.GroupBox2.TabIndex = 5
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Stato:"
        '
        'lblNumClocks
        '
        Me.lblNumClocks.AutoSize = True
        Me.lblNumClocks.ForeColor = System.Drawing.Color.Blue
        Me.lblNumClocks.Location = New System.Drawing.Point(128, 129)
        Me.lblNumClocks.Name = "lblNumClocks"
        Me.lblNumClocks.Size = New System.Drawing.Size(71, 13)
        Me.lblNumClocks.TabIndex = 9
        Me.lblNumClocks.Text = "lblNumClocks"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(20, 129)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(109, 13)
        Me.Label8.TabIndex = 8
        Me.Label8.Text = "Conteggio Marcature:"
        '
        'lblNumFP
        '
        Me.lblNumFP.AutoSize = True
        Me.lblNumFP.ForeColor = System.Drawing.Color.Blue
        Me.lblNumFP.Location = New System.Drawing.Point(128, 101)
        Me.lblNumFP.Name = "lblNumFP"
        Me.lblNumFP.Size = New System.Drawing.Size(52, 13)
        Me.lblNumFP.TabIndex = 7
        Me.lblNumFP.Text = "lblNumFP"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(20, 101)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(102, 13)
        Me.Label6.TabIndex = 6
        Me.Label6.Text = "Conteggio Impronte:"
        '
        'lblNumPersons
        '
        Me.lblNumPersons.AutoSize = True
        Me.lblNumPersons.ForeColor = System.Drawing.Color.Blue
        Me.lblNumPersons.Location = New System.Drawing.Point(128, 74)
        Me.lblNumPersons.Name = "lblNumPersons"
        Me.lblNumPersons.Size = New System.Drawing.Size(77, 13)
        Me.lblNumPersons.TabIndex = 5
        Me.lblNumPersons.Text = "lblNumPersons"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(20, 74)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(100, 13)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "Conteggio Persone:"
        '
        'GroupBox3
        '
        Me.GroupBox3.Location = New System.Drawing.Point(268, 95)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(343, 95)
        Me.GroupBox3.TabIndex = 6
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Audio"
        '
        'txtSpeakerVolume
        '
        Me.txtSpeakerVolume.Location = New System.Drawing.Point(205, 43)
        Me.txtSpeakerVolume.Maximum = New Decimal(New Integer() {5, 0, 0, 0})
        Me.txtSpeakerVolume.Name = "txtSpeakerVolume"
        Me.txtSpeakerVolume.Size = New System.Drawing.Size(52, 20)
        Me.txtSpeakerVolume.TabIndex = 3
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(11, 45)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(97, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Volume Voce (0-5):"
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(536, 473)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(75, 23)
        Me.btnOk.TabIndex = 7
        Me.btnOk.Text = "Ok"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.Label7)
        Me.GroupBox4.Controls.Add(Me.txtDoorLockDelay)
        Me.GroupBox4.Controls.Add(Me.Label5)
        Me.GroupBox4.Location = New System.Drawing.Point(268, 196)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(343, 95)
        Me.GroupBox4.TabIndex = 8
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Relè:"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(210, 22)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(12, 13)
        Me.Label7.TabIndex = 4
        Me.Label7.Text = "s"
        '
        'txtDoorLockDelay
        '
        Me.txtDoorLockDelay.Location = New System.Drawing.Point(152, 18)
        Me.txtDoorLockDelay.Maximum = New Decimal(New Integer() {15, 0, 0, 0})
        Me.txtDoorLockDelay.Name = "txtDoorLockDelay"
        Me.txtDoorLockDelay.Size = New System.Drawing.Size(52, 20)
        Me.txtDoorLockDelay.TabIndex = 3
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(11, 22)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(135, 13)
        Me.Label5.TabIndex = 2
        Me.Label5.Text = "Tempo chiusura rele' (0-15)"
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.chkAutoUpdate)
        Me.GroupBox5.Controls.Add(Me.chkRealtimeAllow)
        Me.GroupBox5.Controls.Add(Me.chkRingAllow)
        Me.GroupBox5.Location = New System.Drawing.Point(1, 196)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(261, 202)
        Me.GroupBox5.TabIndex = 9
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "Relè:"
        '
        'chkAutoUpdate
        '
        Me.chkAutoUpdate.AutoSize = True
        Me.chkAutoUpdate.Location = New System.Drawing.Point(11, 67)
        Me.chkAutoUpdate.Name = "chkAutoUpdate"
        Me.chkAutoUpdate.Size = New System.Drawing.Size(193, 17)
        Me.chkAutoUpdate.TabIndex = 12
        Me.chkAutoUpdate.Text = "Aggiornamento intelligente impronte"
        Me.chkAutoUpdate.UseVisualStyleBackColor = True
        '
        'chkRealtimeAllow
        '
        Me.chkRealtimeAllow.AutoSize = True
        Me.chkRealtimeAllow.Location = New System.Drawing.Point(11, 44)
        Me.chkRealtimeAllow.Name = "chkRealtimeAllow"
        Me.chkRealtimeAllow.Size = New System.Drawing.Size(95, 17)
        Me.chkRealtimeAllow.TabIndex = 11
        Me.chkRealtimeAllow.Text = "Relatime Allow"
        Me.chkRealtimeAllow.UseVisualStyleBackColor = True
        '
        'chkRingAllow
        '
        Me.chkRingAllow.AutoSize = True
        Me.chkRingAllow.Location = New System.Drawing.Point(11, 21)
        Me.chkRingAllow.Name = "chkRingAllow"
        Me.chkRingAllow.Size = New System.Drawing.Size(76, 17)
        Me.chkRingAllow.TabIndex = 10
        Me.chkRingAllow.Text = "Ring Allow"
        Me.chkRingAllow.UseVisualStyleBackColor = True
        '
        'GroupBox6
        '
        Me.GroupBox6.Controls.Add(Me.txtMinDelayM)
        Me.GroupBox6.Controls.Add(Me.Label12)
        Me.GroupBox6.Controls.Add(Me.cboWGOption)
        Me.GroupBox6.Controls.Add(Me.Label11)
        Me.GroupBox6.Controls.Add(Me.txtFGAC)
        Me.GroupBox6.Controls.Add(Me.Label9)
        Me.GroupBox6.Controls.Add(Me.txtSpeakerVolume)
        Me.GroupBox6.Controls.Add(Me.txtPassword)
        Me.GroupBox6.Controls.Add(Me.Label3)
        Me.GroupBox6.Controls.Add(Me.Label10)
        Me.GroupBox6.Location = New System.Drawing.Point(268, 297)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Size = New System.Drawing.Size(343, 170)
        Me.GroupBox6.TabIndex = 10
        Me.GroupBox6.TabStop = False
        Me.GroupBox6.Text = "Amministrazione:"
        '
        'txtPassword
        '
        Me.txtPassword.Location = New System.Drawing.Point(205, 20)
        Me.txtPassword.Maximum = New Decimal(New Integer() {999999, 0, 0, 0})
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.Size = New System.Drawing.Size(104, 20)
        Me.txtPassword.TabIndex = 3
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(11, 22)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(188, 13)
        Me.Label10.TabIndex = 2
        Me.Label10.Text = "Password Comunicazione (0-9999999)"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(11, 69)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(156, 13)
        Me.Label9.TabIndex = 4
        Me.Label9.Text = "Fixe wiegand area code (0-254)"
        '
        'txtFGAC
        '
        Me.txtFGAC.Location = New System.Drawing.Point(205, 67)
        Me.txtFGAC.Maximum = New Decimal(New Integer() {254, 0, 0, 0})
        Me.txtFGAC.Name = "txtFGAC"
        Me.txtFGAC.Size = New System.Drawing.Size(52, 20)
        Me.txtFGAC.TabIndex = 5
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(11, 94)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(89, 13)
        Me.Label11.TabIndex = 6
        Me.Label11.Text = "Metodo wiegand:"
        '
        'cboWGOption
        '
        Me.cboWGOption.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboWGOption.FormattingEnabled = True
        Me.cboWGOption.Items.AddRange(New Object() {"Wiegand26", "Encrypted Wiegand", "Fixed Wiegand area code"})
        Me.cboWGOption.Location = New System.Drawing.Point(205, 91)
        Me.cboWGOption.Name = "cboWGOption"
        Me.cboWGOption.Size = New System.Drawing.Size(131, 21)
        Me.cboWGOption.TabIndex = 7
        '
        'S300TimeSync1
        '
        Me.S300TimeSync1.Device = Nothing
        Me.S300TimeSync1.Location = New System.Drawing.Point(6, 19)
        Me.S300TimeSync1.Name = "S300TimeSync1"
        Me.S300TimeSync1.Size = New System.Drawing.Size(330, 57)
        Me.S300TimeSync1.TabIndex = 114
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(11, 123)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(191, 13)
        Me.Label12.TabIndex = 8
        Me.Label12.Text = "Tempo min. fra 2 marcature (0-250 min)"
        '
        'txtMinDelayM
        '
        Me.txtMinDelayM.Location = New System.Drawing.Point(205, 121)
        Me.txtMinDelayM.Maximum = New Decimal(New Integer() {254, 0, 0, 0})
        Me.txtMinDelayM.Name = "txtMinDelayM"
        Me.txtMinDelayM.Size = New System.Drawing.Size(52, 20)
        Me.txtMinDelayM.TabIndex = 9
        '
        'frmDeviceConfiguration
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(619, 549)
        Me.Controls.Add(Me.GroupBox6)
        Me.Controls.Add(Me.GroupBox5)
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "frmDeviceConfiguration"
        Me.Text = "Configurazione di sistema"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        CType(Me.txtSpeakerVolume, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        CType(Me.txtDoorLockDelay, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout()
        Me.GroupBox6.ResumeLayout(False)
        Me.GroupBox6.PerformLayout()
        CType(Me.txtPassword, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtFGAC, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtMinDelayM, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents lblID As Label
    Friend WithEvents lblVersion As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents S300TimeSync1 As S300TimeSync
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents lblNumClocks As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents lblNumFP As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents lblNumPersons As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents txtSpeakerVolume As NumericUpDown
    Friend WithEvents Label3 As Label
    Friend WithEvents btnOk As Button
    Friend WithEvents GroupBox4 As GroupBox
    Friend WithEvents Label7 As Label
    Friend WithEvents txtDoorLockDelay As NumericUpDown
    Friend WithEvents Label5 As Label
    Friend WithEvents GroupBox5 As GroupBox
    Friend WithEvents chkRingAllow As CheckBox
    Friend WithEvents chkRealtimeAllow As CheckBox
    Friend WithEvents GroupBox6 As GroupBox
    Friend WithEvents txtPassword As NumericUpDown
    Friend WithEvents Label10 As Label
    Friend WithEvents chkAutoUpdate As CheckBox
    Friend WithEvents txtFGAC As NumericUpDown
    Friend WithEvents Label9 As Label
    Friend WithEvents cboWGOption As ComboBox
    Friend WithEvents Label11 As Label
    Friend WithEvents txtMinDelayM As NumericUpDown
    Friend WithEvents Label12 As Label
End Class

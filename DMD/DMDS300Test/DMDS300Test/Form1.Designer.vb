<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form esegue l'override del metodo Dispose per pulire l'elenco dei componenti.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.lstUsers = New System.Windows.Forms.ListView()
        Me.ColumnHeader14 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader15 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader16 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader17 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader18 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.lstUsersNumFP = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ProgressBar2 = New System.Windows.Forms.ProgressBar()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.lstMarcature = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader13 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Button10 = New System.Windows.Forms.Button()
        Me.Button11 = New System.Windows.Forms.Button()
        Me.txtIPAddress = New System.Windows.Forms.TextBox()
        Me.comPort = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.RadioNet = New System.Windows.Forms.RadioButton()
        Me.RadioCom = New System.Windows.Forms.RadioButton()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtID = New System.Windows.Forms.TextBox()
        Me.Button16 = New System.Windows.Forms.Button()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.btnEnd = New System.Windows.Forms.Button()
        Me.btnStart = New System.Windows.Forms.Button()
        Me.Timer2 = New System.Windows.Forms.Timer(Me.components)
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnDevInfo = New System.Windows.Forms.Button()
        Me.Command30 = New System.Windows.Forms.Button()
        Me.btnDownloadMarcature = New System.Windows.Forms.Button()
        Me.Command23 = New System.Windows.Forms.Button()
        Me.btnDeleteMarcature = New System.Windows.Forms.Button()
        Me.btnListUsers = New System.Windows.Forms.Button()
        Me.Command18 = New System.Windows.Forms.Button()
        Me.Command17 = New System.Windows.Forms.Button()
        Me.btnGetNetworkConfig = New System.Windows.Forms.Button()
        Me.btnGetDeviceID = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.cmdSirena = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.btnLoadFP = New System.Windows.Forms.Button()
        Me.btnSaveFP = New System.Windows.Forms.Button()
        Me.btnDeleteUser = New System.Windows.Forms.Button()
        Me.btnEditUser = New System.Windows.Forms.Button()
        Me.btnAddUser = New System.Windows.Forms.Button()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.btnDownNewMarc = New System.Windows.Forms.Button()
        Me.btnEraseALl = New System.Windows.Forms.Button()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.SuspendLayout()
        '
        'lstUsers
        '
        Me.lstUsers.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader14, Me.ColumnHeader15, Me.ColumnHeader16, Me.ColumnHeader17, Me.ColumnHeader18, Me.lstUsersNumFP})
        Me.lstUsers.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstUsers.Location = New System.Drawing.Point(3, 34)
        Me.lstUsers.Name = "lstUsers"
        Me.lstUsers.Size = New System.Drawing.Size(689, 489)
        Me.lstUsers.TabIndex = 112
        Me.lstUsers.UseCompatibleStateImageBehavior = False
        Me.lstUsers.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader14
        '
        Me.ColumnHeader14.Text = "No"
        Me.ColumnHeader14.Width = 40
        '
        'ColumnHeader15
        '
        Me.ColumnHeader15.Text = "Staffer No"
        Me.ColumnHeader15.Width = 63
        '
        'ColumnHeader16
        '
        Me.ColumnHeader16.Text = "Name"
        Me.ColumnHeader16.Width = 117
        '
        'ColumnHeader17
        '
        Me.ColumnHeader17.Text = "PWD"
        Me.ColumnHeader17.Width = 103
        '
        'ColumnHeader18
        '
        Me.ColumnHeader18.Text = "CardNO"
        Me.ColumnHeader18.Width = 79
        '
        'lstUsersNumFP
        '
        Me.lstUsersNumFP.Text = "Impronte"
        '
        'ProgressBar2
        '
        Me.ProgressBar2.Location = New System.Drawing.Point(6, 505)
        Me.ProgressBar2.Name = "ProgressBar2"
        Me.ProgressBar2.Size = New System.Drawing.Size(147, 15)
        Me.ProgressBar2.TabIndex = 111
        Me.ProgressBar2.Visible = False
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Location = New System.Drawing.Point(6, 497)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(227, 23)
        Me.ProgressBar1.TabIndex = 110
        Me.ProgressBar1.Visible = False
        '
        'lstMarcature
        '
        Me.lstMarcature.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4, Me.ColumnHeader13})
        Me.lstMarcature.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstMarcature.Location = New System.Drawing.Point(3, 33)
        Me.lstMarcature.Name = "lstMarcature"
        Me.lstMarcature.Size = New System.Drawing.Size(689, 490)
        Me.lstMarcature.TabIndex = 109
        Me.lstMarcature.UseCompatibleStateImageBehavior = False
        Me.lstMarcature.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "No"
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Staffer No"
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Date/Time"
        Me.ColumnHeader3.Width = 146
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Status"
        '
        'ColumnHeader13
        '
        Me.ColumnHeader13.Text = "Machine ID"
        Me.ColumnHeader13.Width = 79
        '
        'Button10
        '
        Me.Button10.BackColor = System.Drawing.SystemColors.Control
        Me.Button10.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button10.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button10.Location = New System.Drawing.Point(699, 2)
        Me.Button10.Name = "Button10"
        Me.Button10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button10.Size = New System.Drawing.Size(86, 25)
        Me.Button10.TabIndex = 108
        Me.Button10.Text = "Net Daemon"
        Me.Button10.UseVisualStyleBackColor = False
        '
        'Button11
        '
        Me.Button11.BackColor = System.Drawing.SystemColors.Control
        Me.Button11.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button11.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button11.Location = New System.Drawing.Point(605, 3)
        Me.Button11.Name = "Button11"
        Me.Button11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button11.Size = New System.Drawing.Size(84, 25)
        Me.Button11.TabIndex = 107
        Me.Button11.Text = "Com Daemon"
        Me.Button11.UseVisualStyleBackColor = False
        '
        'txtIPAddress
        '
        Me.txtIPAddress.Location = New System.Drawing.Point(363, 7)
        Me.txtIPAddress.Name = "txtIPAddress"
        Me.txtIPAddress.Size = New System.Drawing.Size(84, 20)
        Me.txtIPAddress.TabIndex = 106
        Me.txtIPAddress.Text = "192.168.70.112"
        '
        'comPort
        '
        Me.comPort.FormattingEnabled = True
        Me.comPort.Items.AddRange(New Object() {"COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9"})
        Me.comPort.Location = New System.Drawing.Point(252, 7)
        Me.comPort.Name = "comPort"
        Me.comPort.Size = New System.Drawing.Size(53, 21)
        Me.comPort.TabIndex = 105
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(313, 11)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(45, 13)
        Me.Label3.TabIndex = 104
        Me.Label3.Text = "IP Addr:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(193, 12)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(56, 13)
        Me.Label2.TabIndex = 103
        Me.Label2.Text = "COM Port:"
        '
        'RadioNet
        '
        Me.RadioNet.AutoSize = True
        Me.RadioNet.Checked = True
        Me.RadioNet.Location = New System.Drawing.Point(151, 10)
        Me.RadioNet.Name = "RadioNet"
        Me.RadioNet.Size = New System.Drawing.Size(42, 17)
        Me.RadioNet.TabIndex = 102
        Me.RadioNet.TabStop = True
        Me.RadioNet.Text = "Net"
        Me.RadioNet.UseVisualStyleBackColor = True
        '
        'RadioCom
        '
        Me.RadioCom.AutoSize = True
        Me.RadioCom.Location = New System.Drawing.Point(102, 10)
        Me.RadioCom.Name = "RadioCom"
        Me.RadioCom.Size = New System.Drawing.Size(46, 17)
        Me.RadioCom.TabIndex = 101
        Me.RadioCom.Text = "Com"
        Me.RadioCom.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(7, 10)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(21, 13)
        Me.Label1.TabIndex = 100
        Me.Label1.Text = "ID:"
        '
        'txtID
        '
        Me.txtID.Location = New System.Drawing.Point(35, 8)
        Me.txtID.Name = "txtID"
        Me.txtID.Size = New System.Drawing.Size(55, 20)
        Me.txtID.TabIndex = 99
        Me.txtID.Text = "3"
        '
        'Button16
        '
        Me.Button16.BackColor = System.Drawing.SystemColors.Control
        Me.Button16.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button16.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button16.Location = New System.Drawing.Point(10, 99)
        Me.Button16.Name = "Button16"
        Me.Button16.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button16.Size = New System.Drawing.Size(127, 25)
        Me.Button16.TabIndex = 98
        Me.Button16.Text = "Force Open Lock"
        Me.Button16.UseVisualStyleBackColor = False
        '
        'Button5
        '
        Me.Button5.BackColor = System.Drawing.SystemColors.Control
        Me.Button5.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button5.Location = New System.Drawing.Point(10, 140)
        Me.Button5.Name = "Button5"
        Me.Button5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button5.Size = New System.Drawing.Size(127, 25)
        Me.Button5.TabIndex = 90
        Me.Button5.Text = "Set/Get Time Section"
        Me.Button5.UseVisualStyleBackColor = False
        '
        'btnEnd
        '
        Me.btnEnd.BackColor = System.Drawing.SystemColors.Control
        Me.btnEnd.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnEnd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnEnd.Location = New System.Drawing.Point(522, 5)
        Me.btnEnd.Name = "btnEnd"
        Me.btnEnd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnEnd.Size = New System.Drawing.Size(54, 25)
        Me.btnEnd.TabIndex = 89
        Me.btnEnd.Text = "End"
        Me.btnEnd.UseVisualStyleBackColor = False
        '
        'btnStart
        '
        Me.btnStart.BackColor = System.Drawing.SystemColors.Control
        Me.btnStart.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnStart.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnStart.Location = New System.Drawing.Point(462, 5)
        Me.btnStart.Name = "btnStart"
        Me.btnStart.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnStart.Size = New System.Drawing.Size(54, 25)
        Me.btnStart.TabIndex = 58
        Me.btnStart.Text = "Start"
        Me.btnStart.UseVisualStyleBackColor = False
        '
        'Timer2
        '
        Me.Timer2.Enabled = True
        Me.Timer2.Interval = 1000
        '
        'btnDevInfo
        '
        Me.btnDevInfo.BackColor = System.Drawing.SystemColors.Control
        Me.btnDevInfo.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnDevInfo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnDevInfo.Location = New System.Drawing.Point(10, 68)
        Me.btnDevInfo.Name = "btnDevInfo"
        Me.btnDevInfo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnDevInfo.Size = New System.Drawing.Size(126, 25)
        Me.btnDevInfo.TabIndex = 85
        Me.btnDevInfo.Text = "Device Configuration"
        Me.btnDevInfo.UseVisualStyleBackColor = False
        '
        'Command30
        '
        Me.Command30.BackColor = System.Drawing.SystemColors.Control
        Me.Command30.Cursor = System.Windows.Forms.Cursors.Default
        Me.Command30.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Command30.Location = New System.Drawing.Point(11, 359)
        Me.Command30.Name = "Command30"
        Me.Command30.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Command30.Size = New System.Drawing.Size(126, 25)
        Me.Command30.TabIndex = 84
        Me.Command30.Text = "Initialize Device"
        Me.Command30.UseVisualStyleBackColor = False
        '
        'btnDownloadMarcature
        '
        Me.btnDownloadMarcature.BackColor = System.Drawing.SystemColors.Control
        Me.btnDownloadMarcature.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnDownloadMarcature.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnDownloadMarcature.Location = New System.Drawing.Point(0, 3)
        Me.btnDownloadMarcature.Name = "btnDownloadMarcature"
        Me.btnDownloadMarcature.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnDownloadMarcature.Size = New System.Drawing.Size(165, 25)
        Me.btnDownloadMarcature.TabIndex = 79
        Me.btnDownloadMarcature.Text = "Scarica Tutte le Marcature"
        Me.btnDownloadMarcature.UseVisualStyleBackColor = False
        '
        'Command23
        '
        Me.Command23.BackColor = System.Drawing.SystemColors.Control
        Me.Command23.Cursor = System.Windows.Forms.Cursors.Default
        Me.Command23.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Command23.Location = New System.Drawing.Point(10, 233)
        Me.Command23.Name = "Command23"
        Me.Command23.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Command23.Size = New System.Drawing.Size(127, 25)
        Me.Command23.TabIndex = 78
        Me.Command23.Text = "Get/Add Message"
        Me.Command23.UseVisualStyleBackColor = False
        '
        'btnDeleteMarcature
        '
        Me.btnDeleteMarcature.BackColor = System.Drawing.SystemColors.Control
        Me.btnDeleteMarcature.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnDeleteMarcature.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnDeleteMarcature.Location = New System.Drawing.Point(331, 3)
        Me.btnDeleteMarcature.Name = "btnDeleteMarcature"
        Me.btnDeleteMarcature.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnDeleteMarcature.Size = New System.Drawing.Size(127, 25)
        Me.btnDeleteMarcature.TabIndex = 77
        Me.btnDeleteMarcature.Text = "Cancella Marcature"
        Me.btnDeleteMarcature.UseVisualStyleBackColor = False
        '
        'btnListUsers
        '
        Me.btnListUsers.BackColor = System.Drawing.SystemColors.Control
        Me.btnListUsers.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnListUsers.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnListUsers.Location = New System.Drawing.Point(3, 4)
        Me.btnListUsers.Name = "btnListUsers"
        Me.btnListUsers.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnListUsers.Size = New System.Drawing.Size(76, 25)
        Me.btnListUsers.TabIndex = 75
        Me.btnListUsers.Text = "Scarica"
        Me.btnListUsers.UseVisualStyleBackColor = False
        '
        'Command18
        '
        Me.Command18.BackColor = System.Drawing.SystemColors.Control
        Me.Command18.Cursor = System.Windows.Forms.Cursors.Default
        Me.Command18.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Command18.Location = New System.Drawing.Point(10, 265)
        Me.Command18.Name = "Command18"
        Me.Command18.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Command18.Size = New System.Drawing.Size(128, 25)
        Me.Command18.TabIndex = 74
        Me.Command18.Text = "Get All Message Head" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        Me.Command18.UseVisualStyleBackColor = False
        '
        'Command17
        '
        Me.Command17.BackColor = System.Drawing.SystemColors.Control
        Me.Command17.Cursor = System.Windows.Forms.Cursors.Default
        Me.Command17.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Command17.Location = New System.Drawing.Point(10, 297)
        Me.Command17.Name = "Command17"
        Me.Command17.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Command17.Size = New System.Drawing.Size(128, 25)
        Me.Command17.TabIndex = 73
        Me.Command17.Text = "Del Message"
        Me.Command17.UseVisualStyleBackColor = False
        '
        'btnGetNetworkConfig
        '
        Me.btnGetNetworkConfig.BackColor = System.Drawing.SystemColors.Control
        Me.btnGetNetworkConfig.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnGetNetworkConfig.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnGetNetworkConfig.Location = New System.Drawing.Point(10, 37)
        Me.btnGetNetworkConfig.Name = "btnGetNetworkConfig"
        Me.btnGetNetworkConfig.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnGetNetworkConfig.Size = New System.Drawing.Size(126, 25)
        Me.btnGetNetworkConfig.TabIndex = 57
        Me.btnGetNetworkConfig.Text = "Network Configuration"
        Me.btnGetNetworkConfig.UseVisualStyleBackColor = False
        '
        'btnGetDeviceID
        '
        Me.btnGetDeviceID.BackColor = System.Drawing.SystemColors.Control
        Me.btnGetDeviceID.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnGetDeviceID.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnGetDeviceID.Location = New System.Drawing.Point(10, 6)
        Me.btnGetDeviceID.Name = "btnGetDeviceID"
        Me.btnGetDeviceID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnGetDeviceID.Size = New System.Drawing.Size(126, 25)
        Me.btnGetDeviceID.TabIndex = 56
        Me.btnGetDeviceID.Text = "Connected Devices"
        Me.btnGetDeviceID.UseVisualStyleBackColor = False
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Button16)
        Me.Panel1.Controls.Add(Me.Button5)
        Me.Panel1.Controls.Add(Me.cmdSirena)
        Me.Panel1.Controls.Add(Me.Button1)
        Me.Panel1.Controls.Add(Me.btnGetDeviceID)
        Me.Panel1.Controls.Add(Me.btnDevInfo)
        Me.Panel1.Controls.Add(Me.btnGetNetworkConfig)
        Me.Panel1.Controls.Add(Me.Command30)
        Me.Panel1.Controls.Add(Me.Command17)
        Me.Panel1.Controls.Add(Me.Command23)
        Me.Panel1.Controls.Add(Me.Command18)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Left
        Me.Panel1.Location = New System.Drawing.Point(0, 35)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(148, 552)
        Me.Panel1.TabIndex = 114
        '
        'cmdSirena
        '
        Me.cmdSirena.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSirena.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSirena.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSirena.Location = New System.Drawing.Point(10, 202)
        Me.cmdSirena.Name = "cmdSirena"
        Me.cmdSirena.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSirena.Size = New System.Drawing.Size(127, 25)
        Me.cmdSirena.TabIndex = 88
        Me.cmdSirena.Text = "Gestione Sirena"
        Me.cmdSirena.UseVisualStyleBackColor = False
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.SystemColors.Control
        Me.Button1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button1.Location = New System.Drawing.Point(10, 171)
        Me.Button1.Name = "Button1"
        Me.Button1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button1.Size = New System.Drawing.Size(127, 25)
        Me.Button1.TabIndex = 87
        Me.Button1.Text = "Set/Get Group "
        Me.Button1.UseVisualStyleBackColor = False
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.txtID)
        Me.Panel2.Controls.Add(Me.btnStart)
        Me.Panel2.Controls.Add(Me.btnEnd)
        Me.Panel2.Controls.Add(Me.Label1)
        Me.Panel2.Controls.Add(Me.RadioCom)
        Me.Panel2.Controls.Add(Me.RadioNet)
        Me.Panel2.Controls.Add(Me.Button10)
        Me.Panel2.Controls.Add(Me.Label2)
        Me.Panel2.Controls.Add(Me.Button11)
        Me.Panel2.Controls.Add(Me.Label3)
        Me.Panel2.Controls.Add(Me.txtIPAddress)
        Me.Panel2.Controls.Add(Me.comPort)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(851, 35)
        Me.Panel2.TabIndex = 115
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.Location = New System.Drawing.Point(148, 35)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(703, 552)
        Me.TabControl1.TabIndex = 116
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.lstUsers)
        Me.TabPage1.Controls.Add(Me.Panel3)
        Me.TabPage1.Controls.Add(Me.ProgressBar2)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(695, 526)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Utenti"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.btnEraseALl)
        Me.Panel3.Controls.Add(Me.btnLoadFP)
        Me.Panel3.Controls.Add(Me.btnSaveFP)
        Me.Panel3.Controls.Add(Me.btnDeleteUser)
        Me.Panel3.Controls.Add(Me.btnEditUser)
        Me.Panel3.Controls.Add(Me.btnAddUser)
        Me.Panel3.Controls.Add(Me.btnListUsers)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel3.Location = New System.Drawing.Point(3, 3)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(689, 31)
        Me.Panel3.TabIndex = 113
        '
        'btnLoadFP
        '
        Me.btnLoadFP.BackColor = System.Drawing.SystemColors.Control
        Me.btnLoadFP.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnLoadFP.Enabled = False
        Me.btnLoadFP.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnLoadFP.Location = New System.Drawing.Point(569, 3)
        Me.btnLoadFP.Name = "btnLoadFP"
        Me.btnLoadFP.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnLoadFP.Size = New System.Drawing.Size(109, 25)
        Me.btnLoadFP.TabIndex = 80
        Me.btnLoadFP.Text = "Carica Impronta (1)"
        Me.btnLoadFP.UseVisualStyleBackColor = False
        '
        'btnSaveFP
        '
        Me.btnSaveFP.BackColor = System.Drawing.SystemColors.Control
        Me.btnSaveFP.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnSaveFP.Enabled = False
        Me.btnSaveFP.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnSaveFP.Location = New System.Drawing.Point(454, 3)
        Me.btnSaveFP.Name = "btnSaveFP"
        Me.btnSaveFP.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnSaveFP.Size = New System.Drawing.Size(109, 25)
        Me.btnSaveFP.TabIndex = 79
        Me.btnSaveFP.Text = "Salva Impronta (1)"
        Me.btnSaveFP.UseVisualStyleBackColor = False
        '
        'btnDeleteUser
        '
        Me.btnDeleteUser.BackColor = System.Drawing.SystemColors.Control
        Me.btnDeleteUser.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnDeleteUser.Enabled = False
        Me.btnDeleteUser.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnDeleteUser.Location = New System.Drawing.Point(372, 4)
        Me.btnDeleteUser.Name = "btnDeleteUser"
        Me.btnDeleteUser.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnDeleteUser.Size = New System.Drawing.Size(76, 25)
        Me.btnDeleteUser.TabIndex = 78
        Me.btnDeleteUser.Text = "Elimina"
        Me.btnDeleteUser.UseVisualStyleBackColor = False
        '
        'btnEditUser
        '
        Me.btnEditUser.BackColor = System.Drawing.SystemColors.Control
        Me.btnEditUser.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnEditUser.Enabled = False
        Me.btnEditUser.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnEditUser.Location = New System.Drawing.Point(290, 4)
        Me.btnEditUser.Name = "btnEditUser"
        Me.btnEditUser.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnEditUser.Size = New System.Drawing.Size(76, 25)
        Me.btnEditUser.TabIndex = 77
        Me.btnEditUser.Text = "Modifica"
        Me.btnEditUser.UseVisualStyleBackColor = False
        '
        'btnAddUser
        '
        Me.btnAddUser.BackColor = System.Drawing.SystemColors.Control
        Me.btnAddUser.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnAddUser.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnAddUser.Location = New System.Drawing.Point(208, 4)
        Me.btnAddUser.Name = "btnAddUser"
        Me.btnAddUser.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnAddUser.Size = New System.Drawing.Size(76, 25)
        Me.btnAddUser.TabIndex = 76
        Me.btnAddUser.Text = "Aggiungi"
        Me.btnAddUser.UseVisualStyleBackColor = False
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.ProgressBar1)
        Me.TabPage2.Controls.Add(Me.lstMarcature)
        Me.TabPage2.Controls.Add(Me.Panel4)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(695, 526)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Marcature"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.btnDownNewMarc)
        Me.Panel4.Controls.Add(Me.btnDownloadMarcature)
        Me.Panel4.Controls.Add(Me.btnDeleteMarcature)
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel4.Location = New System.Drawing.Point(3, 3)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(689, 30)
        Me.Panel4.TabIndex = 111
        '
        'btnDownNewMarc
        '
        Me.btnDownNewMarc.BackColor = System.Drawing.SystemColors.Control
        Me.btnDownNewMarc.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnDownNewMarc.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnDownNewMarc.Location = New System.Drawing.Point(166, 3)
        Me.btnDownNewMarc.Name = "btnDownNewMarc"
        Me.btnDownNewMarc.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnDownNewMarc.Size = New System.Drawing.Size(165, 25)
        Me.btnDownNewMarc.TabIndex = 80
        Me.btnDownNewMarc.Text = "Scarica le Nuove Marcature"
        Me.btnDownNewMarc.UseVisualStyleBackColor = False
        '
        'btnEraseALl
        '
        Me.btnEraseALl.BackColor = System.Drawing.SystemColors.Control
        Me.btnEraseALl.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnEraseALl.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnEraseALl.Location = New System.Drawing.Point(80, 4)
        Me.btnEraseALl.Name = "btnEraseALl"
        Me.btnEraseALl.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnEraseALl.Size = New System.Drawing.Size(122, 25)
        Me.btnEraseALl.TabIndex = 81
        Me.btnEraseALl.Text = "Elimina Tutti"
        Me.btnEraseALl.UseVisualStyleBackColor = False
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(851, 587)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Panel2)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "Form1"
        Me.Text = "SDK Demo"
        Me.Panel1.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        Me.TabPage2.ResumeLayout(False)
        Me.Panel4.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents lstUsers As ListView
    Friend WithEvents ColumnHeader14 As ColumnHeader
    Friend WithEvents ColumnHeader15 As ColumnHeader
    Friend WithEvents ColumnHeader16 As ColumnHeader
    Friend WithEvents ColumnHeader17 As ColumnHeader
    Friend WithEvents ColumnHeader18 As ColumnHeader
    Friend WithEvents ProgressBar2 As ProgressBar
    Friend WithEvents ProgressBar1 As ProgressBar
    Friend WithEvents lstMarcature As ListView
    Friend WithEvents ColumnHeader1 As ColumnHeader
    Friend WithEvents ColumnHeader2 As ColumnHeader
    Friend WithEvents ColumnHeader3 As ColumnHeader
    Friend WithEvents ColumnHeader4 As ColumnHeader
    Friend WithEvents ColumnHeader13 As ColumnHeader
    Public WithEvents Button10 As Button
    Public WithEvents Button11 As Button
    Friend WithEvents txtIPAddress As TextBox
    Friend WithEvents comPort As ComboBox
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents RadioNet As RadioButton
    Friend WithEvents RadioCom As RadioButton
    Friend WithEvents Label1 As Label
    Friend WithEvents txtID As TextBox
    Public WithEvents Button16 As Button
    Public WithEvents Button5 As Button
    Public WithEvents btnEnd As Button
    Public WithEvents btnStart As Button
    Public WithEvents Timer2 As Timer
    Public WithEvents ToolTip1 As ToolTip
    Public WithEvents btnDevInfo As Button
    Public WithEvents Command30 As Button
    Public WithEvents btnDownloadMarcature As Button
    Public WithEvents Command23 As Button
    Public WithEvents btnDeleteMarcature As Button
    Public WithEvents btnListUsers As Button
    Public WithEvents Command18 As Button
    Public WithEvents Command17 As Button
    Public WithEvents btnGetNetworkConfig As Button
    Public WithEvents btnGetDeviceID As Button
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Panel2 As Panel
    Public WithEvents Button1 As Button
    Public WithEvents cmdSirena As Button
    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents TabPage1 As TabPage
    Friend WithEvents Panel3 As Panel
    Public WithEvents btnDeleteUser As Button
    Public WithEvents btnEditUser As Button
    Public WithEvents btnAddUser As Button
    Friend WithEvents TabPage2 As TabPage
    Friend WithEvents Panel4 As Panel
    Friend WithEvents lstUsersNumFP As ColumnHeader
    Public WithEvents btnSaveFP As Button
    Public WithEvents btnLoadFP As Button
    Public WithEvents btnDownNewMarc As Button
    Public WithEvents btnEraseALl As Button
End Class

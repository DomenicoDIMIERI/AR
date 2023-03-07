<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.txtLog = New System.Windows.Forms.TextBox()
        Me.bottomPanel = New System.Windows.Forms.Panel()
        Me.lblProgress = New System.Windows.Forms.Label()
        Me.barProgress = New System.Windows.Forms.ProgressBar()
        Me.topPanel = New System.Windows.Forms.Panel()
        Me.btnScarica = New System.Windows.Forms.Button()
        Me.lblPassword = New System.Windows.Forms.Label()
        Me.txtPassword = New System.Windows.Forms.TextBox()
        Me.lblUserName = New System.Windows.Forms.Label()
        Me.txtUserName = New System.Windows.Forms.TextBox()
        Me.txtFTPPort = New System.Windows.Forms.TextBox()
        Me.lblPort = New System.Windows.Forms.Label()
        Me.txtFTPServer = New System.Windows.Forms.TextBox()
        Me.blbAddress = New System.Windows.Forms.Label()
        Me.txtLOGPath = New System.Windows.Forms.TextBox()
        Me.lblBKFolder = New System.Windows.Forms.Label()
        Me.btnBK = New System.Windows.Forms.Button()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.chkAuto = New System.Windows.Forms.CheckBox()
        Me.bottomPanel.SuspendLayout()
        Me.topPanel.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtLog
        '
        Me.txtLog.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtLog.Location = New System.Drawing.Point(0, 85)
        Me.txtLog.Multiline = True
        Me.txtLog.Name = "txtLog"
        Me.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtLog.Size = New System.Drawing.Size(668, 309)
        Me.txtLog.TabIndex = 0
        '
        'bottomPanel
        '
        Me.bottomPanel.Controls.Add(Me.lblProgress)
        Me.bottomPanel.Controls.Add(Me.barProgress)
        Me.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.bottomPanel.Location = New System.Drawing.Point(0, 394)
        Me.bottomPanel.Name = "bottomPanel"
        Me.bottomPanel.Size = New System.Drawing.Size(668, 34)
        Me.bottomPanel.TabIndex = 1
        '
        'lblProgress
        '
        Me.lblProgress.AutoSize = True
        Me.lblProgress.Location = New System.Drawing.Point(155, 12)
        Me.lblProgress.Name = "lblProgress"
        Me.lblProgress.Size = New System.Drawing.Size(15, 13)
        Me.lblProgress.TabIndex = 1
        Me.lblProgress.Text = "%"
        Me.lblProgress.Visible = False
        '
        'barProgress
        '
        Me.barProgress.Location = New System.Drawing.Point(3, 6)
        Me.barProgress.Name = "barProgress"
        Me.barProgress.Size = New System.Drawing.Size(146, 23)
        Me.barProgress.TabIndex = 0
        Me.barProgress.Visible = False
        '
        'topPanel
        '
        Me.topPanel.Controls.Add(Me.chkAuto)
        Me.topPanel.Controls.Add(Me.btnBK)
        Me.topPanel.Controls.Add(Me.lblBKFolder)
        Me.topPanel.Controls.Add(Me.txtLOGPath)
        Me.topPanel.Controls.Add(Me.btnScarica)
        Me.topPanel.Controls.Add(Me.lblPassword)
        Me.topPanel.Controls.Add(Me.txtPassword)
        Me.topPanel.Controls.Add(Me.lblUserName)
        Me.topPanel.Controls.Add(Me.txtUserName)
        Me.topPanel.Controls.Add(Me.txtFTPPort)
        Me.topPanel.Controls.Add(Me.lblPort)
        Me.topPanel.Controls.Add(Me.txtFTPServer)
        Me.topPanel.Controls.Add(Me.blbAddress)
        Me.topPanel.Dock = System.Windows.Forms.DockStyle.Top
        Me.topPanel.Location = New System.Drawing.Point(0, 0)
        Me.topPanel.Name = "topPanel"
        Me.topPanel.Size = New System.Drawing.Size(668, 85)
        Me.topPanel.TabIndex = 2
        '
        'btnScarica
        '
        Me.btnScarica.Location = New System.Drawing.Point(590, 6)
        Me.btnScarica.Name = "btnScarica"
        Me.btnScarica.Size = New System.Drawing.Size(75, 23)
        Me.btnScarica.TabIndex = 8
        Me.btnScarica.Text = "Scarica..."
        Me.btnScarica.UseVisualStyleBackColor = True
        '
        'lblPassword
        '
        Me.lblPassword.AutoSize = True
        Me.lblPassword.Location = New System.Drawing.Point(338, 34)
        Me.lblPassword.Name = "lblPassword"
        Me.lblPassword.Size = New System.Drawing.Size(56, 13)
        Me.lblPassword.TabIndex = 7
        Me.lblPassword.Text = "Password:"
        '
        'txtPassword
        '
        Me.txtPassword.Location = New System.Drawing.Point(408, 31)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPassword.Size = New System.Drawing.Size(169, 20)
        Me.txtPassword.TabIndex = 6
        '
        'lblUserName
        '
        Me.lblUserName.AutoSize = True
        Me.lblUserName.Location = New System.Drawing.Point(338, 9)
        Me.lblUserName.Name = "lblUserName"
        Me.lblUserName.Size = New System.Drawing.Size(60, 13)
        Me.lblUserName.TabIndex = 5
        Me.lblUserName.Text = "UserName:"
        '
        'txtUserName
        '
        Me.txtUserName.Location = New System.Drawing.Point(408, 6)
        Me.txtUserName.Name = "txtUserName"
        Me.txtUserName.Size = New System.Drawing.Size(169, 20)
        Me.txtUserName.TabIndex = 4
        '
        'txtFTPPort
        '
        Me.txtFTPPort.Location = New System.Drawing.Point(263, 6)
        Me.txtFTPPort.Name = "txtFTPPort"
        Me.txtFTPPort.Size = New System.Drawing.Size(40, 20)
        Me.txtFTPPort.TabIndex = 3
        Me.txtFTPPort.Text = "21"
        '
        'lblPort
        '
        Me.lblPort.AutoSize = True
        Me.lblPort.Location = New System.Drawing.Point(252, 9)
        Me.lblPort.Name = "lblPort"
        Me.lblPort.Size = New System.Drawing.Size(10, 13)
        Me.lblPort.TabIndex = 2
        Me.lblPort.Text = ":"
        '
        'txtFTPServer
        '
        Me.txtFTPServer.Location = New System.Drawing.Point(82, 6)
        Me.txtFTPServer.Name = "txtFTPServer"
        Me.txtFTPServer.Size = New System.Drawing.Size(169, 20)
        Me.txtFTPServer.TabIndex = 1
        '
        'blbAddress
        '
        Me.blbAddress.AutoSize = True
        Me.blbAddress.Location = New System.Drawing.Point(12, 9)
        Me.blbAddress.Name = "blbAddress"
        Me.blbAddress.Size = New System.Drawing.Size(64, 13)
        Me.blbAddress.TabIndex = 0
        Me.blbAddress.Text = "Server FTP:"
        '
        'txtLOGPath
        '
        Me.txtLOGPath.Location = New System.Drawing.Point(82, 31)
        Me.txtLOGPath.Name = "txtLOGPath"
        Me.txtLOGPath.Size = New System.Drawing.Size(169, 20)
        Me.txtLOGPath.TabIndex = 9
        '
        'lblBKFolder
        '
        Me.lblBKFolder.AutoSize = True
        Me.lblBKFolder.Location = New System.Drawing.Point(12, 34)
        Me.lblBKFolder.Name = "lblBKFolder"
        Me.lblBKFolder.Size = New System.Drawing.Size(61, 13)
        Me.lblBKFolder.TabIndex = 10
        Me.lblBKFolder.Text = "txtBKFolder"
        '
        'btnBK
        '
        Me.btnBK.Location = New System.Drawing.Point(263, 28)
        Me.btnBK.Name = "btnBK"
        Me.btnBK.Size = New System.Drawing.Size(28, 23)
        Me.btnBK.TabIndex = 11
        Me.btnBK.Text = "..."
        Me.btnBK.UseVisualStyleBackColor = True
        '
        'chkAuto
        '
        Me.chkAuto.AutoSize = True
        Me.chkAuto.Location = New System.Drawing.Point(590, 35)
        Me.chkAuto.Name = "chkAuto"
        Me.chkAuto.Size = New System.Drawing.Size(48, 17)
        Me.chkAuto.TabIndex = 12
        Me.chkAuto.Text = "Auto"
        Me.chkAuto.UseVisualStyleBackColor = True
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(668, 428)
        Me.Controls.Add(Me.txtLog)
        Me.Controls.Add(Me.topPanel)
        Me.Controls.Add(Me.bottomPanel)
        Me.Name = "frmMain"
        Me.Text = "Procedura di sincronizzazione con AR"
        Me.bottomPanel.ResumeLayout(False)
        Me.bottomPanel.PerformLayout()
        Me.topPanel.ResumeLayout(False)
        Me.topPanel.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtLog As System.Windows.Forms.TextBox
    Friend WithEvents bottomPanel As System.Windows.Forms.Panel
    Friend WithEvents topPanel As System.Windows.Forms.Panel
    Friend WithEvents lblProgress As System.Windows.Forms.Label
    Friend WithEvents barProgress As System.Windows.Forms.ProgressBar
    Friend WithEvents lblPassword As System.Windows.Forms.Label
    Friend WithEvents txtPassword As System.Windows.Forms.TextBox
    Friend WithEvents lblUserName As System.Windows.Forms.Label
    Friend WithEvents txtUserName As System.Windows.Forms.TextBox
    Friend WithEvents txtFTPPort As System.Windows.Forms.TextBox
    Friend WithEvents lblPort As System.Windows.Forms.Label
    Friend WithEvents txtFTPServer As System.Windows.Forms.TextBox
    Friend WithEvents blbAddress As System.Windows.Forms.Label
    Friend WithEvents btnScarica As System.Windows.Forms.Button
    Friend WithEvents btnBK As System.Windows.Forms.Button
    Friend WithEvents lblBKFolder As System.Windows.Forms.Label
    Friend WithEvents txtLOGPath As System.Windows.Forms.TextBox
    Friend WithEvents chkAuto As System.Windows.Forms.CheckBox
    Friend WithEvents Timer1 As System.Windows.Forms.Timer

End Class

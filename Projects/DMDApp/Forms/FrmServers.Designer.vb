<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmServers
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmServers))
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnOk = New System.Windows.Forms.Button()
        Me.grpServer = New System.Windows.Forms.GroupBox()
        Me.lstServers = New System.Windows.Forms.ListBox()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.btnAdd = New System.Windows.Forms.ToolStripButton()
        Me.btnDelete = New System.Windows.Forms.ToolStripButton()
        Me.grpConfig = New System.Windows.Forms.GroupBox()
        Me.txtURL = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Panel1.SuspendLayout()
        Me.grpServer.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.grpConfig.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.btnCancel)
        Me.Panel1.Controls.Add(Me.btnOk)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 243)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(425, 29)
        Me.Panel1.TabIndex = 10
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.Location = New System.Drawing.Point(347, 3)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 11
        Me.btnCancel.Text = "Annulla"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnOk
        '
        Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOk.Location = New System.Drawing.Point(272, 3)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(75, 23)
        Me.btnOk.TabIndex = 10
        Me.btnOk.Text = "Ok"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'grpServer
        '
        Me.grpServer.Controls.Add(Me.lstServers)
        Me.grpServer.Dock = System.Windows.Forms.DockStyle.Left
        Me.grpServer.Location = New System.Drawing.Point(0, 25)
        Me.grpServer.Name = "grpServer"
        Me.grpServer.Size = New System.Drawing.Size(152, 218)
        Me.grpServer.TabIndex = 11
        Me.grpServer.TabStop = False
        Me.grpServer.Text = "Servers"
        '
        'lstServers
        '
        Me.lstServers.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstServers.FormattingEnabled = True
        Me.lstServers.Location = New System.Drawing.Point(3, 16)
        Me.lstServers.Name = "lstServers"
        Me.lstServers.Size = New System.Drawing.Size(146, 199)
        Me.lstServers.TabIndex = 1
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnAdd, Me.btnDelete})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(425, 25)
        Me.ToolStrip1.TabIndex = 12
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'btnAdd
        '
        Me.btnAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.btnAdd.Image = CType(resources.GetObject("btnAdd.Image"), System.Drawing.Image)
        Me.btnAdd.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(23, 22)
        Me.btnAdd.Text = "+"
        Me.btnAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay
        '
        'btnDelete
        '
        Me.btnDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.btnDelete.Enabled = False
        Me.btnDelete.Image = CType(resources.GetObject("btnDelete.Image"), System.Drawing.Image)
        Me.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(23, 22)
        Me.btnDelete.Text = "-"
        Me.btnDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay
        '
        'grpConfig
        '
        Me.grpConfig.Controls.Add(Me.txtURL)
        Me.grpConfig.Controls.Add(Me.Label1)
        Me.grpConfig.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpConfig.Enabled = False
        Me.grpConfig.Location = New System.Drawing.Point(152, 25)
        Me.grpConfig.Name = "grpConfig"
        Me.grpConfig.Size = New System.Drawing.Size(273, 218)
        Me.grpConfig.TabIndex = 13
        Me.grpConfig.TabStop = False
        Me.grpConfig.Text = "Configurazione"
        '
        'txtURL
        '
        Me.txtURL.Location = New System.Drawing.Point(48, 19)
        Me.txtURL.Name = "txtURL"
        Me.txtURL.Size = New System.Drawing.Size(212, 20)
        Me.txtURL.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 22)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(29, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "URL"
        '
        'FrmServers
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(425, 272)
        Me.Controls.Add(Me.grpConfig)
        Me.Controls.Add(Me.grpServer)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "FrmServers"
        Me.Text = "Servers"
        Me.Panel1.ResumeLayout(False)
        Me.grpServer.ResumeLayout(False)
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.grpConfig.ResumeLayout(False)
        Me.grpConfig.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents grpServer As System.Windows.Forms.GroupBox
    Friend WithEvents lstServers As System.Windows.Forms.ListBox
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents btnAdd As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnDelete As System.Windows.Forms.ToolStripButton
    Friend WithEvents grpConfig As System.Windows.Forms.GroupBox
    Friend WithEvents txtURL As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
End Class

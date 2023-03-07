<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmFolderMonitor
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmFolderMonitor))
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnOk = New System.Windows.Forms.Button()
        Me.grpServer = New System.Windows.Forms.GroupBox()
        Me.lstIncluse = New System.Windows.Forms.ListBox()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.btnAddInclusa = New System.Windows.Forms.ToolStripButton()
        Me.btnDeleteInclusa = New System.Windows.Forms.ToolStripButton()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.lstEscluse = New System.Windows.Forms.ListBox()
        Me.ToolStrip2 = New System.Windows.Forms.ToolStrip()
        Me.btnAddEsclusa = New System.Windows.Forms.ToolStripButton()
        Me.btnDeleteEsclusa = New System.Windows.Forms.ToolStripButton()
        Me.Panel1.SuspendLayout()
        Me.grpServer.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.ToolStrip2.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.btnCancel)
        Me.Panel1.Controls.Add(Me.btnOk)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 292)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(554, 29)
        Me.Panel1.TabIndex = 9
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.Location = New System.Drawing.Point(476, 3)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 11
        Me.btnCancel.Text = "&Annulla"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnOk
        '
        Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOk.Location = New System.Drawing.Point(401, 3)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(75, 23)
        Me.btnOk.TabIndex = 10
        Me.btnOk.Text = "&Ok"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'grpServer
        '
        Me.grpServer.Controls.Add(Me.lstIncluse)
        Me.grpServer.Controls.Add(Me.ToolStrip1)
        Me.grpServer.Dock = System.Windows.Forms.DockStyle.Left
        Me.grpServer.Location = New System.Drawing.Point(0, 0)
        Me.grpServer.Name = "grpServer"
        Me.grpServer.Size = New System.Drawing.Size(277, 292)
        Me.grpServer.TabIndex = 12
        Me.grpServer.TabStop = False
        Me.grpServer.Text = "CARTELLE MONITORATE"
        '
        'lstIncluse
        '
        Me.lstIncluse.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstIncluse.FormattingEnabled = True
        Me.lstIncluse.Location = New System.Drawing.Point(3, 41)
        Me.lstIncluse.Name = "lstIncluse"
        Me.lstIncluse.Size = New System.Drawing.Size(271, 248)
        Me.lstIncluse.TabIndex = 1
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnAddInclusa, Me.btnDeleteInclusa})
        Me.ToolStrip1.Location = New System.Drawing.Point(3, 16)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(271, 25)
        Me.ToolStrip1.TabIndex = 13
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'btnAddInclusa
        '
        Me.btnAddInclusa.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.btnAddInclusa.Image = CType(resources.GetObject("btnAddInclusa.Image"), System.Drawing.Image)
        Me.btnAddInclusa.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnAddInclusa.Name = "btnAddInclusa"
        Me.btnAddInclusa.Size = New System.Drawing.Size(23, 22)
        Me.btnAddInclusa.Text = "+"
        Me.btnAddInclusa.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay
        '
        'btnDeleteInclusa
        '
        Me.btnDeleteInclusa.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.btnDeleteInclusa.Enabled = False
        Me.btnDeleteInclusa.Image = CType(resources.GetObject("btnDeleteInclusa.Image"), System.Drawing.Image)
        Me.btnDeleteInclusa.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnDeleteInclusa.Name = "btnDeleteInclusa"
        Me.btnDeleteInclusa.Size = New System.Drawing.Size(23, 22)
        Me.btnDeleteInclusa.Text = "-"
        Me.btnDeleteInclusa.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.lstEscluse)
        Me.GroupBox1.Controls.Add(Me.ToolStrip2)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Left
        Me.GroupBox1.Location = New System.Drawing.Point(277, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(277, 292)
        Me.GroupBox1.TabIndex = 13
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "CARTELLE ESCLUSE"
        '
        'lstEscluse
        '
        Me.lstEscluse.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstEscluse.FormattingEnabled = True
        Me.lstEscluse.Location = New System.Drawing.Point(3, 41)
        Me.lstEscluse.Name = "lstEscluse"
        Me.lstEscluse.Size = New System.Drawing.Size(271, 248)
        Me.lstEscluse.TabIndex = 1
        '
        'ToolStrip2
        '
        Me.ToolStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnAddEsclusa, Me.btnDeleteEsclusa})
        Me.ToolStrip2.Location = New System.Drawing.Point(3, 16)
        Me.ToolStrip2.Name = "ToolStrip2"
        Me.ToolStrip2.Size = New System.Drawing.Size(271, 25)
        Me.ToolStrip2.TabIndex = 13
        Me.ToolStrip2.Text = "ToolStrip2"
        '
        'btnAddEsclusa
        '
        Me.btnAddEsclusa.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.btnAddEsclusa.Image = CType(resources.GetObject("btnAddEsclusa.Image"), System.Drawing.Image)
        Me.btnAddEsclusa.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnAddEsclusa.Name = "btnAddEsclusa"
        Me.btnAddEsclusa.Size = New System.Drawing.Size(23, 22)
        Me.btnAddEsclusa.Text = "+"
        Me.btnAddEsclusa.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay
        '
        'btnDeleteEsclusa
        '
        Me.btnDeleteEsclusa.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.btnDeleteEsclusa.Enabled = False
        Me.btnDeleteEsclusa.Image = CType(resources.GetObject("btnDeleteEsclusa.Image"), System.Drawing.Image)
        Me.btnDeleteEsclusa.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnDeleteEsclusa.Name = "btnDeleteEsclusa"
        Me.btnDeleteEsclusa.Size = New System.Drawing.Size(23, 22)
        Me.btnDeleteEsclusa.Text = "-"
        Me.btnDeleteEsclusa.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay
        '
        'FrmFolderMonitor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(554, 321)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.grpServer)
        Me.Controls.Add(Me.Panel1)
        Me.Name = "FrmFolderMonitor"
        Me.Text = "Cartelle monitorate"
        Me.Panel1.ResumeLayout(False)
        Me.grpServer.ResumeLayout(False)
        Me.grpServer.PerformLayout()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ToolStrip2.ResumeLayout(False)
        Me.ToolStrip2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents grpServer As System.Windows.Forms.GroupBox
    Friend WithEvents lstIncluse As System.Windows.Forms.ListBox
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents btnAddInclusa As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnDeleteInclusa As System.Windows.Forms.ToolStripButton
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents lstEscluse As System.Windows.Forms.ListBox
    Friend WithEvents ToolStrip2 As System.Windows.Forms.ToolStrip
    Friend WithEvents btnAddEsclusa As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnDeleteEsclusa As System.Windows.Forms.ToolStripButton
End Class

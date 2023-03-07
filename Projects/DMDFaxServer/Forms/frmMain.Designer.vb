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
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuControlla = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuExit = New System.Windows.Forms.ToolStripMenuItem()
        Me.ConfigurazioneToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.POPToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SMTPToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.HylaFaxToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.btnCancelPendingJob = New System.Windows.Forms.ToolStripButton()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.txtLog = New System.Windows.Forms.TextBox()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.lstSent = New System.Windows.Forms.ListBox()
        Me.ToolStrip2 = New System.Windows.Forms.ToolStrip()
        Me.btnDelSent = New System.Windows.Forms.ToolStripButton()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.lstQueuedItems = New System.Windows.Forms.ListBox()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.tabPageInfo = New System.Windows.Forms.TabPage()
        Me.txtInfo = New System.Windows.Forms.TextBox()
        Me.tabPagePreview = New System.Windows.Forms.TabPage()
        Me.picPreview = New System.Windows.Forms.PictureBox()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.BackgroundWorker1 = New System.ComponentModel.BackgroundWorker()
        Me.MenuStrip1.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.ToolStrip2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.tabPageInfo.SuspendLayout()
        Me.tabPagePreview.SuspendLayout()
        CType(Me.picPreview, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel2
        '
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel2.Location = New System.Drawing.Point(0, 565)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(748, 34)
        Me.Panel2.TabIndex = 1
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.ConfigurazioneToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(748, 24)
        Me.MenuStrip1.TabIndex = 3
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuControlla, Me.ToolStripMenuItem2, Me.mnuExit})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.FileToolStripMenuItem.Text = "&File"
        '
        'mnuControlla
        '
        Me.mnuControlla.Name = "mnuControlla"
        Me.mnuControlla.Size = New System.Drawing.Size(123, 22)
        Me.mnuControlla.Text = "&Controlla"
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(120, 6)
        '
        'mnuExit
        '
        Me.mnuExit.Name = "mnuExit"
        Me.mnuExit.Size = New System.Drawing.Size(123, 22)
        Me.mnuExit.Text = "&Esci"
        '
        'ConfigurazioneToolStripMenuItem
        '
        Me.ConfigurazioneToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.POPToolStripMenuItem, Me.SMTPToolStripMenuItem, Me.ToolStripMenuItem1, Me.HylaFaxToolStripMenuItem})
        Me.ConfigurazioneToolStripMenuItem.Name = "ConfigurazioneToolStripMenuItem"
        Me.ConfigurazioneToolStripMenuItem.Size = New System.Drawing.Size(100, 20)
        Me.ConfigurazioneToolStripMenuItem.Text = "&Configurazione"
        '
        'POPToolStripMenuItem
        '
        Me.POPToolStripMenuItem.Name = "POPToolStripMenuItem"
        Me.POPToolStripMenuItem.Size = New System.Drawing.Size(115, 22)
        Me.POPToolStripMenuItem.Text = "&POP"
        '
        'SMTPToolStripMenuItem
        '
        Me.SMTPToolStripMenuItem.Name = "SMTPToolStripMenuItem"
        Me.SMTPToolStripMenuItem.Size = New System.Drawing.Size(115, 22)
        Me.SMTPToolStripMenuItem.Text = "&SMTP"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(112, 6)
        '
        'HylaFaxToolStripMenuItem
        '
        Me.HylaFaxToolStripMenuItem.Name = "HylaFaxToolStripMenuItem"
        Me.HylaFaxToolStripMenuItem.Size = New System.Drawing.Size(115, 22)
        Me.HylaFaxToolStripMenuItem.Text = "&HylaFax"
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnCancelPendingJob})
        Me.ToolStrip1.Location = New System.Drawing.Point(3, 16)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(243, 25)
        Me.ToolStrip1.TabIndex = 4
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'btnCancelPendingJob
        '
        Me.btnCancelPendingJob.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnCancelPendingJob.Enabled = False
        Me.btnCancelPendingJob.Image = Global.DMDFaxServer.My.Resources.Resources.x
        Me.btnCancelPendingJob.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnCancelPendingJob.Name = "btnCancelPendingJob"
        Me.btnCancelPendingJob.Size = New System.Drawing.Size(23, 22)
        Me.btnCancelPendingJob.Text = "Annulla il lavoro"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.txtLog)
        Me.GroupBox2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.GroupBox2.Location = New System.Drawing.Point(0, 349)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(748, 216)
        Me.GroupBox2.TabIndex = 5
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Log"
        '
        'txtLog
        '
        Me.txtLog.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtLog.Location = New System.Drawing.Point(3, 16)
        Me.txtLog.Multiline = True
        Me.txtLog.Name = "txtLog"
        Me.txtLog.ReadOnly = True
        Me.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtLog.Size = New System.Drawing.Size(742, 197)
        Me.txtLog.TabIndex = 0
        Me.txtLog.WordWrap = False
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.lstSent)
        Me.GroupBox4.Controls.Add(Me.ToolStrip2)
        Me.GroupBox4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox4.Location = New System.Drawing.Point(0, 156)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(249, 169)
        Me.GroupBox4.TabIndex = 1
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Documenti inviati"
        '
        'lstSent
        '
        Me.lstSent.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstSent.FormattingEnabled = True
        Me.lstSent.Location = New System.Drawing.Point(3, 41)
        Me.lstSent.Name = "lstSent"
        Me.lstSent.Size = New System.Drawing.Size(243, 125)
        Me.lstSent.TabIndex = 5
        '
        'ToolStrip2
        '
        Me.ToolStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnDelSent})
        Me.ToolStrip2.Location = New System.Drawing.Point(3, 16)
        Me.ToolStrip2.Name = "ToolStrip2"
        Me.ToolStrip2.Size = New System.Drawing.Size(243, 25)
        Me.ToolStrip2.TabIndex = 6
        Me.ToolStrip2.Text = "ToolStrip2"
        '
        'btnDelSent
        '
        Me.btnDelSent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnDelSent.Enabled = False
        Me.btnDelSent.Image = Global.DMDFaxServer.My.Resources.Resources.x
        Me.btnDelSent.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnDelSent.Name = "btnDelSent"
        Me.btnDelSent.Size = New System.Drawing.Size(23, 22)
        Me.btnDelSent.Text = "Annulla il lavoro"
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.lstQueuedItems)
        Me.GroupBox3.Controls.Add(Me.ToolStrip1)
        Me.GroupBox3.Dock = System.Windows.Forms.DockStyle.Top
        Me.GroupBox3.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(249, 156)
        Me.GroupBox3.TabIndex = 0
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Documenti in uscita"
        '
        'lstQueuedItems
        '
        Me.lstQueuedItems.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstQueuedItems.FormattingEnabled = True
        Me.lstQueuedItems.Location = New System.Drawing.Point(3, 41)
        Me.lstQueuedItems.Name = "lstQueuedItems"
        Me.lstQueuedItems.Size = New System.Drawing.Size(243, 112)
        Me.lstQueuedItems.TabIndex = 0
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 24)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.GroupBox4)
        Me.SplitContainer1.Panel1.Controls.Add(Me.GroupBox3)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.TabControl1)
        Me.SplitContainer1.Size = New System.Drawing.Size(748, 325)
        Me.SplitContainer1.SplitterDistance = 249
        Me.SplitContainer1.TabIndex = 7
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.tabPageInfo)
        Me.TabControl1.Controls.Add(Me.tabPagePreview)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(495, 325)
        Me.TabControl1.TabIndex = 0
        '
        'tabPageInfo
        '
        Me.tabPageInfo.Controls.Add(Me.txtInfo)
        Me.tabPageInfo.Location = New System.Drawing.Point(4, 22)
        Me.tabPageInfo.Name = "tabPageInfo"
        Me.tabPageInfo.Padding = New System.Windows.Forms.Padding(3)
        Me.tabPageInfo.Size = New System.Drawing.Size(487, 299)
        Me.tabPageInfo.TabIndex = 0
        Me.tabPageInfo.Text = "Info"
        Me.tabPageInfo.UseVisualStyleBackColor = True
        '
        'txtInfo
        '
        Me.txtInfo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtInfo.Location = New System.Drawing.Point(3, 3)
        Me.txtInfo.Multiline = True
        Me.txtInfo.Name = "txtInfo"
        Me.txtInfo.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtInfo.Size = New System.Drawing.Size(481, 293)
        Me.txtInfo.TabIndex = 1
        '
        'tabPagePreview
        '
        Me.tabPagePreview.Controls.Add(Me.picPreview)
        Me.tabPagePreview.Location = New System.Drawing.Point(4, 22)
        Me.tabPagePreview.Name = "tabPagePreview"
        Me.tabPagePreview.Padding = New System.Windows.Forms.Padding(3)
        Me.tabPagePreview.Size = New System.Drawing.Size(487, 367)
        Me.tabPagePreview.TabIndex = 1
        Me.tabPagePreview.Text = "Anteprima"
        Me.tabPagePreview.UseVisualStyleBackColor = True
        '
        'picPreview
        '
        Me.picPreview.Dock = System.Windows.Forms.DockStyle.Fill
        Me.picPreview.Location = New System.Drawing.Point(3, 3)
        Me.picPreview.Name = "picPreview"
        Me.picPreview.Size = New System.Drawing.Size(481, 361)
        Me.picPreview.TabIndex = 0
        Me.picPreview.TabStop = False
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        Me.Timer1.Interval = 60000
        '
        'BackgroundWorker1
        '
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(748, 599)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "frmMain"
        Me.Text = "Fax Server"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.ToolStrip2.ResumeLayout(False)
        Me.ToolStrip2.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.TabControl1.ResumeLayout(False)
        Me.tabPageInfo.ResumeLayout(False)
        Me.tabPageInfo.PerformLayout()
        Me.tabPagePreview.ResumeLayout(False)
        CType(Me.picPreview, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuExit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents txtLog As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents lstSent As System.Windows.Forms.ListBox
    Friend WithEvents ToolStrip2 As System.Windows.Forms.ToolStrip
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents lstQueuedItems As System.Windows.Forms.ListBox
    Friend WithEvents btnCancelPendingJob As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnDelSent As System.Windows.Forms.ToolStripButton
    Friend WithEvents ConfigurazioneToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents POPToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SMTPToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents HylaFaxToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuControlla As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents tabPageInfo As System.Windows.Forms.TabPage
    Friend WithEvents txtInfo As System.Windows.Forms.TextBox
    Friend WithEvents tabPagePreview As System.Windows.Forms.TabPage
    Friend WithEvents picPreview As System.Windows.Forms.PictureBox
    Friend WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker

End Class

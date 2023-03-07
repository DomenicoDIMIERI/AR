<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmLogEditor
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
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.btnFind = New System.Windows.Forms.ToolStripButton()
        Me.grpServer = New System.Windows.Forms.GroupBox()
        Me.lstFiles = New System.Windows.Forms.ListBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.btnFind1 = New System.Windows.Forms.Button()
        Me.txtFolder = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.tabInfo = New System.Windows.Forms.TabPage()
        Me.txtInfo = New System.Windows.Forms.WebBrowser()
        Me.tabKeyBuffer = New System.Windows.Forms.TabPage()
        Me.txtKeyBuffer = New System.Windows.Forms.WebBrowser()
        Me.tabLogBuffer = New System.Windows.Forms.TabPage()
        Me.txtLogBuffer = New System.Windows.Forms.WebBrowser()
        Me.tabScreen = New System.Windows.Forms.TabPage()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.pnlIcons = New System.Windows.Forms.Panel()
        Me.tabText = New System.Windows.Forms.TabPage()
        Me.txtText = New System.Windows.Forms.TextBox()
        Me.ToolStrip1.SuspendLayout()
        Me.grpServer.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.tabInfo.SuspendLayout()
        Me.tabKeyBuffer.SuspendLayout()
        Me.tabLogBuffer.SuspendLayout()
        Me.tabScreen.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabText.SuspendLayout()
        Me.SuspendLayout()
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnFind})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(836, 25)
        Me.ToolStrip1.TabIndex = 14
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'btnFind
        '
        Me.btnFind.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnFind.Image = Global.DIALTPLogEditor.My.Resources.Resources.btnFind
        Me.btnFind.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnFind.Name = "btnFind"
        Me.btnFind.Size = New System.Drawing.Size(23, 22)
        Me.btnFind.Text = "Sfoglia..."
        '
        'grpServer
        '
        Me.grpServer.Controls.Add(Me.lstFiles)
        Me.grpServer.Dock = System.Windows.Forms.DockStyle.Left
        Me.grpServer.Location = New System.Drawing.Point(0, 71)
        Me.grpServer.Name = "grpServer"
        Me.grpServer.Size = New System.Drawing.Size(233, 425)
        Me.grpServer.TabIndex = 15
        Me.grpServer.TabStop = False
        Me.grpServer.Text = "ELENCO DEI FILES..."
        '
        'lstFiles
        '
        Me.lstFiles.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstFiles.FormattingEnabled = True
        Me.lstFiles.Location = New System.Drawing.Point(3, 16)
        Me.lstFiles.Name = "lstFiles"
        Me.lstFiles.Size = New System.Drawing.Size(227, 406)
        Me.lstFiles.TabIndex = 1
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.btnFind1)
        Me.GroupBox1.Controls.Add(Me.txtFolder)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Top
        Me.GroupBox1.Location = New System.Drawing.Point(0, 25)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(836, 46)
        Me.GroupBox1.TabIndex = 18
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "."
        '
        'btnFind1
        '
        Me.btnFind1.Location = New System.Drawing.Point(463, 11)
        Me.btnFind1.Name = "btnFind1"
        Me.btnFind1.Size = New System.Drawing.Size(75, 23)
        Me.btnFind1.TabIndex = 2
        Me.btnFind1.Text = "Cerca..."
        Me.btnFind1.UseVisualStyleBackColor = True
        '
        'txtFolder
        '
        Me.txtFolder.Location = New System.Drawing.Point(63, 13)
        Me.txtFolder.Name = "txtFolder"
        Me.txtFolder.Size = New System.Drawing.Size(384, 20)
        Me.txtFolder.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(45, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Cartella:"
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.tabInfo)
        Me.TabControl1.Controls.Add(Me.tabKeyBuffer)
        Me.TabControl1.Controls.Add(Me.tabLogBuffer)
        Me.TabControl1.Controls.Add(Me.tabScreen)
        Me.TabControl1.Controls.Add(Me.tabText)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.Location = New System.Drawing.Point(233, 71)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(603, 425)
        Me.TabControl1.TabIndex = 19
        '
        'tabInfo
        '
        Me.tabInfo.Controls.Add(Me.txtInfo)
        Me.tabInfo.Location = New System.Drawing.Point(4, 22)
        Me.tabInfo.Name = "tabInfo"
        Me.tabInfo.Padding = New System.Windows.Forms.Padding(3)
        Me.tabInfo.Size = New System.Drawing.Size(595, 399)
        Me.tabInfo.TabIndex = 0
        Me.tabInfo.Text = "Informazioni"
        Me.tabInfo.UseVisualStyleBackColor = True
        '
        'txtInfo
        '
        Me.txtInfo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtInfo.Location = New System.Drawing.Point(3, 3)
        Me.txtInfo.MinimumSize = New System.Drawing.Size(20, 20)
        Me.txtInfo.Name = "txtInfo"
        Me.txtInfo.Size = New System.Drawing.Size(589, 393)
        Me.txtInfo.TabIndex = 5
        '
        'tabKeyBuffer
        '
        Me.tabKeyBuffer.Controls.Add(Me.txtKeyBuffer)
        Me.tabKeyBuffer.Location = New System.Drawing.Point(4, 22)
        Me.tabKeyBuffer.Name = "tabKeyBuffer"
        Me.tabKeyBuffer.Size = New System.Drawing.Size(595, 399)
        Me.tabKeyBuffer.TabIndex = 2
        Me.tabKeyBuffer.Text = "Tasti Premuti"
        Me.tabKeyBuffer.UseVisualStyleBackColor = True
        '
        'txtKeyBuffer
        '
        Me.txtKeyBuffer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtKeyBuffer.Location = New System.Drawing.Point(0, 0)
        Me.txtKeyBuffer.MinimumSize = New System.Drawing.Size(20, 20)
        Me.txtKeyBuffer.Name = "txtKeyBuffer"
        Me.txtKeyBuffer.Size = New System.Drawing.Size(595, 399)
        Me.txtKeyBuffer.TabIndex = 4
        '
        'tabLogBuffer
        '
        Me.tabLogBuffer.Controls.Add(Me.txtLogBuffer)
        Me.tabLogBuffer.Location = New System.Drawing.Point(4, 22)
        Me.tabLogBuffer.Name = "tabLogBuffer"
        Me.tabLogBuffer.Size = New System.Drawing.Size(595, 399)
        Me.tabLogBuffer.TabIndex = 3
        Me.tabLogBuffer.Text = "Log Buffer"
        Me.tabLogBuffer.UseVisualStyleBackColor = True
        '
        'txtLogBuffer
        '
        Me.txtLogBuffer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtLogBuffer.Location = New System.Drawing.Point(0, 0)
        Me.txtLogBuffer.MinimumSize = New System.Drawing.Size(20, 20)
        Me.txtLogBuffer.Name = "txtLogBuffer"
        Me.txtLogBuffer.Size = New System.Drawing.Size(595, 399)
        Me.txtLogBuffer.TabIndex = 3
        '
        'tabScreen
        '
        Me.tabScreen.AutoScroll = True
        Me.tabScreen.Controls.Add(Me.PictureBox1)
        Me.tabScreen.Controls.Add(Me.pnlIcons)
        Me.tabScreen.Location = New System.Drawing.Point(4, 22)
        Me.tabScreen.Name = "tabScreen"
        Me.tabScreen.Padding = New System.Windows.Forms.Padding(3)
        Me.tabScreen.Size = New System.Drawing.Size(595, 399)
        Me.tabScreen.TabIndex = 1
        Me.tabScreen.Text = "Screen Shots"
        Me.tabScreen.UseVisualStyleBackColor = True
        '
        'PictureBox1
        '
        Me.PictureBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PictureBox1.Location = New System.Drawing.Point(3, 3)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(589, 239)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox1.TabIndex = 1
        Me.PictureBox1.TabStop = False
        '
        'pnlIcons
        '
        Me.pnlIcons.AutoScroll = True
        Me.pnlIcons.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlIcons.Location = New System.Drawing.Point(3, 242)
        Me.pnlIcons.Name = "pnlIcons"
        Me.pnlIcons.Size = New System.Drawing.Size(589, 154)
        Me.pnlIcons.TabIndex = 0
        '
        'tabText
        '
        Me.tabText.Controls.Add(Me.txtText)
        Me.tabText.Location = New System.Drawing.Point(4, 22)
        Me.tabText.Name = "tabText"
        Me.tabText.Size = New System.Drawing.Size(595, 399)
        Me.tabText.TabIndex = 4
        Me.tabText.Text = "Testo"
        Me.tabText.UseVisualStyleBackColor = True
        '
        'txtText
        '
        Me.txtText.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtText.Location = New System.Drawing.Point(0, 0)
        Me.txtText.Multiline = True
        Me.txtText.Name = "txtText"
        Me.txtText.Size = New System.Drawing.Size(595, 399)
        Me.txtText.TabIndex = 0
        '
        'frmLogEditor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(836, 496)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.grpServer)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Name = "frmLogEditor"
        Me.Text = "Editor..."
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.grpServer.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.TabControl1.ResumeLayout(False)
        Me.tabInfo.ResumeLayout(False)
        Me.tabKeyBuffer.ResumeLayout(False)
        Me.tabLogBuffer.ResumeLayout(False)
        Me.tabScreen.ResumeLayout(False)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabText.ResumeLayout(False)
        Me.tabText.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents btnFind As System.Windows.Forms.ToolStripButton
    Friend WithEvents grpServer As System.Windows.Forms.GroupBox
    Friend WithEvents lstFiles As System.Windows.Forms.ListBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents btnFind1 As System.Windows.Forms.Button
    Friend WithEvents txtFolder As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents tabInfo As System.Windows.Forms.TabPage
    Friend WithEvents tabKeyBuffer As System.Windows.Forms.TabPage
    Friend WithEvents tabScreen As System.Windows.Forms.TabPage
    Friend WithEvents tabLogBuffer As System.Windows.Forms.TabPage
    Friend WithEvents txtLogBuffer As System.Windows.Forms.WebBrowser
    Friend WithEvents txtKeyBuffer As System.Windows.Forms.WebBrowser
    Friend WithEvents txtInfo As System.Windows.Forms.WebBrowser
    Friend WithEvents tabText As TabPage
    Friend WithEvents txtText As TextBox
    Friend WithEvents pnlIcons As Panel
    Friend WithEvents PictureBox1 As PictureBox
End Class

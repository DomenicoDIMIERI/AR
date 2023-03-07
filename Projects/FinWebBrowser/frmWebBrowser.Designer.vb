<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmWebBrowser
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
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuEsci = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.lblGo = New System.Windows.Forms.ToolStripLabel()
        Me.txtWebAddress = New System.Windows.Forms.ToolStripTextBox()
        Me.btnGo = New System.Windows.Forms.ToolStripButton()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.WebBrowser1 = New DMD.MyWebBrowser()
        Me.StrumentiToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuShowCode = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuStrip1.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.StrumentiToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(685, 24)
        Me.MenuStrip1.TabIndex = 1
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuEsci})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.FileToolStripMenuItem.Text = "&File"
        '
        'mnuEsci
        '
        Me.mnuEsci.Name = "mnuEsci"
        Me.mnuEsci.Size = New System.Drawing.Size(94, 22)
        Me.mnuEsci.Text = "&Esci"
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.lblGo, Me.txtWebAddress, Me.btnGo})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 24)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(685, 25)
        Me.ToolStrip1.TabIndex = 2
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'lblGo
        '
        Me.lblGo.Name = "lblGo"
        Me.lblGo.Size = New System.Drawing.Size(24, 22)
        Me.lblGo.Text = "go:"
        '
        'txtWebAddress
        '
        Me.txtWebAddress.Name = "txtWebAddress"
        Me.txtWebAddress.Size = New System.Drawing.Size(500, 25)
        '
        'btnGo
        '
        Me.btnGo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnGo.Image = Global.DMD.My.Resources.Resources.website16
        Me.btnGo.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnGo.Name = "btnGo"
        Me.btnGo.Size = New System.Drawing.Size(23, 22)
        Me.btnGo.Text = "Vai..."
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 411)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(685, 22)
        Me.StatusStrip1.TabIndex = 3
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'WebBrowser1
        '
        Me.WebBrowser1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.WebBrowser1.Location = New System.Drawing.Point(0, 49)
        Me.WebBrowser1.MinimumSize = New System.Drawing.Size(20, 20)
        Me.WebBrowser1.Name = "WebBrowser1"
        Me.WebBrowser1.Size = New System.Drawing.Size(685, 362)
        Me.WebBrowser1.TabIndex = 4
        '
        'StrumentiToolStripMenuItem
        '
        Me.StrumentiToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuShowCode})
        Me.StrumentiToolStripMenuItem.Name = "StrumentiToolStripMenuItem"
        Me.StrumentiToolStripMenuItem.Size = New System.Drawing.Size(71, 20)
        Me.StrumentiToolStripMenuItem.Text = "&Strumenti"
        '
        'mnuShowCode
        '
        Me.mnuShowCode.Name = "mnuShowCode"
        Me.mnuShowCode.Size = New System.Drawing.Size(164, 22)
        Me.mnuShowCode.Text = "Visualizza &Codice"
        '
        'frmWebBrowser
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(685, 433)
        Me.Controls.Add(Me.WebBrowser1)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "frmWebBrowser"
        Me.Text = "Fin.Se.A. Web Browser 1.0b"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuEsci As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents lblGo As System.Windows.Forms.ToolStripLabel
    Friend WithEvents txtWebAddress As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents btnGo As System.Windows.Forms.ToolStripButton
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents WebBrowser1 As MyWebBrowser
    Friend WithEvents StrumentiToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuShowCode As System.Windows.Forms.ToolStripMenuItem
End Class

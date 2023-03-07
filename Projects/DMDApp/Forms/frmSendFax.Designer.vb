<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSendFax
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
        Me.CaricaDocumentoPDFToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.InviaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.EsciToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.btnUpload = New System.Windows.Forms.ToolStripButton()
        Me.btnSend = New System.Windows.Forms.ToolStripButton()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.cboNumero = New System.Windows.Forms.ComboBox()
        Me.lblDestinatario = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtCentralino = New System.Windows.Forms.ComboBox()
        Me.MenuStrip1.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(784, 24)
        Me.MenuStrip1.TabIndex = 0
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CaricaDocumentoPDFToolStripMenuItem, Me.InviaToolStripMenuItem, Me.ToolStripMenuItem1, Me.EsciToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.FileToolStripMenuItem.Text = "&File"
        '
        'CaricaDocumentoPDFToolStripMenuItem
        '
        Me.CaricaDocumentoPDFToolStripMenuItem.Name = "CaricaDocumentoPDFToolStripMenuItem"
        Me.CaricaDocumentoPDFToolStripMenuItem.Size = New System.Drawing.Size(196, 22)
        Me.CaricaDocumentoPDFToolStripMenuItem.Text = "&Carica documento PDF"
        '
        'InviaToolStripMenuItem
        '
        Me.InviaToolStripMenuItem.Name = "InviaToolStripMenuItem"
        Me.InviaToolStripMenuItem.Size = New System.Drawing.Size(196, 22)
        Me.InviaToolStripMenuItem.Text = "&Invia"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(193, 6)
        '
        'EsciToolStripMenuItem
        '
        Me.EsciToolStripMenuItem.Name = "EsciToolStripMenuItem"
        Me.EsciToolStripMenuItem.Size = New System.Drawing.Size(196, 22)
        Me.EsciToolStripMenuItem.Text = "&Esci"
        '
        'ToolStrip1
        '
        Me.ToolStrip1.ImageScalingSize = New System.Drawing.Size(32, 32)
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnUpload, Me.btnSend})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 24)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(784, 39)
        Me.ToolStrip1.TabIndex = 1
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'btnUpload
        '
        Me.btnUpload.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnUpload.Image = Global.DMDApp.My.Resources.Resources.uparrow16
        Me.btnUpload.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnUpload.Name = "btnUpload"
        Me.btnUpload.Size = New System.Drawing.Size(36, 36)
        Me.btnUpload.Text = "Carica il documento da inviare"
        '
        'btnSend
        '
        Me.btnSend.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnSend.Enabled = False
        Me.btnSend.Image = Global.DMDApp.My.Resources.Resources.faxout
        Me.btnSend.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnSend.Name = "btnSend"
        Me.btnSend.Size = New System.Drawing.Size(36, 36)
        Me.btnSend.Text = "Invia il fax"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.txtCentralino)
        Me.GroupBox1.Controls.Add(Me.cboNumero)
        Me.GroupBox1.Controls.Add(Me.lblDestinatario)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Top
        Me.GroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(0, 63)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(784, 114)
        Me.GroupBox1.TabIndex = 2
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Opzioni del fax:"
        '
        'cboNumero
        '
        Me.cboNumero.FormattingEnabled = True
        Me.cboNumero.Location = New System.Drawing.Point(295, 77)
        Me.cboNumero.Name = "cboNumero"
        Me.cboNumero.Size = New System.Drawing.Size(180, 28)
        Me.cboNumero.TabIndex = 3
        '
        'lblDestinatario
        '
        Me.lblDestinatario.AutoSize = True
        Me.lblDestinatario.ForeColor = System.Drawing.Color.Brown
        Me.lblDestinatario.Location = New System.Drawing.Point(148, 38)
        Me.lblDestinatario.Name = "lblDestinatario"
        Me.lblDestinatario.Size = New System.Drawing.Size(21, 20)
        Me.lblDestinatario.TabIndex = 2
        Me.lblDestinatario.Text = "..."
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(25, 38)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(99, 20)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Destinatario:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(220, 80)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(69, 20)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Numero:"
        '
        'Panel1
        '
        Me.Panel1.AutoScroll = True
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 177)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(784, 365)
        Me.Panel1.TabIndex = 3
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(26, 81)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(47, 18)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Linea:"
        '
        'txtCentralino
        '
        Me.txtCentralino.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.txtCentralino.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCentralino.FormattingEnabled = True
        Me.txtCentralino.Location = New System.Drawing.Point(79, 78)
        Me.txtCentralino.Name = "txtCentralino"
        Me.txtCentralino.Size = New System.Drawing.Size(109, 26)
        Me.txtCentralino.TabIndex = 5
        '
        'frmSendFax
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(784, 542)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "frmSendFax"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Invia un fax..."
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CaricaDocumentoPDFToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents InviaToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents EsciToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents btnUpload As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnSend As System.Windows.Forms.ToolStripButton
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents cboNumero As System.Windows.Forms.ComboBox
    Friend WithEvents lblDestinatario As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtCentralino As System.Windows.Forms.ComboBox
End Class

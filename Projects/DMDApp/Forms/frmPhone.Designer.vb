<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmPhone
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPhone))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.lstInterni = New System.Windows.Forms.ListBox()
        Me.grpInterno = New System.Windows.Forms.GroupBox()
        Me.pnlInterno = New System.Windows.Forms.Panel()
        Me.chkActive = New System.Windows.Forms.CheckBox()
        Me.lblUtente = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.lblPostazione = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lblInterno = New System.Windows.Forms.Label()
        Me.labeli = New System.Windows.Forms.Label()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.btnRefresh = New System.Windows.Forms.ToolStripButton()
        Me.btnSettings = New System.Windows.Forms.ToolStripButton()
        Me.GroupBox1.SuspendLayout()
        Me.grpInterno.SuspendLayout()
        Me.pnlInterno.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.lstInterni)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Left
        Me.GroupBox1.Location = New System.Drawing.Point(0, 25)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(254, 425)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Elenco Interni"
        '
        'lstInterni
        '
        Me.lstInterni.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstInterni.FormattingEnabled = True
        Me.lstInterni.Location = New System.Drawing.Point(3, 16)
        Me.lstInterni.Name = "lstInterni"
        Me.lstInterni.Size = New System.Drawing.Size(248, 406)
        Me.lstInterni.TabIndex = 1
        '
        'grpInterno
        '
        Me.grpInterno.Controls.Add(Me.pnlInterno)
        Me.grpInterno.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpInterno.Location = New System.Drawing.Point(254, 25)
        Me.grpInterno.Name = "grpInterno"
        Me.grpInterno.Size = New System.Drawing.Size(346, 425)
        Me.grpInterno.TabIndex = 2
        Me.grpInterno.TabStop = False
        Me.grpInterno.Text = "Interno"
        '
        'pnlInterno
        '
        Me.pnlInterno.Controls.Add(Me.chkActive)
        Me.pnlInterno.Controls.Add(Me.lblUtente)
        Me.pnlInterno.Controls.Add(Me.Label4)
        Me.pnlInterno.Controls.Add(Me.lblPostazione)
        Me.pnlInterno.Controls.Add(Me.Label3)
        Me.pnlInterno.Controls.Add(Me.lblInterno)
        Me.pnlInterno.Controls.Add(Me.labeli)
        Me.pnlInterno.Location = New System.Drawing.Point(16, 19)
        Me.pnlInterno.Name = "pnlInterno"
        Me.pnlInterno.Size = New System.Drawing.Size(318, 265)
        Me.pnlInterno.TabIndex = 0
        '
        'chkActive
        '
        Me.chkActive.Appearance = System.Windows.Forms.Appearance.Button
        Me.chkActive.AutoSize = True
        Me.chkActive.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkActive.Image = Global.DMDApp.My.Resources.Resources.MICW
        Me.chkActive.Location = New System.Drawing.Point(111, 98)
        Me.chkActive.Name = "chkActive"
        Me.chkActive.Size = New System.Drawing.Size(107, 119)
        Me.chkActive.TabIndex = 7
        Me.chkActive.Text = "Chiuso"
        Me.chkActive.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.chkActive.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.chkActive.UseVisualStyleBackColor = True
        '
        'lblUtente
        '
        Me.lblUtente.AutoSize = True
        Me.lblUtente.ForeColor = System.Drawing.Color.Blue
        Me.lblUtente.Location = New System.Drawing.Point(97, 67)
        Me.lblUtente.Name = "lblUtente"
        Me.lblUtente.Size = New System.Drawing.Size(16, 13)
        Me.lblUtente.TabIndex = 5
        Me.lblUtente.Text = "..."
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(12, 67)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(42, 13)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "Utente:"
        '
        'lblPostazione
        '
        Me.lblPostazione.AutoSize = True
        Me.lblPostazione.ForeColor = System.Drawing.Color.Blue
        Me.lblPostazione.Location = New System.Drawing.Point(97, 41)
        Me.lblPostazione.Name = "lblPostazione"
        Me.lblPostazione.Size = New System.Drawing.Size(16, 13)
        Me.lblPostazione.TabIndex = 3
        Me.lblPostazione.Text = "..."
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 41)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(62, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Postazione:"
        '
        'lblInterno
        '
        Me.lblInterno.AutoSize = True
        Me.lblInterno.ForeColor = System.Drawing.Color.Blue
        Me.lblInterno.Location = New System.Drawing.Point(97, 16)
        Me.lblInterno.Name = "lblInterno"
        Me.lblInterno.Size = New System.Drawing.Size(16, 13)
        Me.lblInterno.TabIndex = 1
        Me.lblInterno.Text = "..."
        '
        'labeli
        '
        Me.labeli.AutoSize = True
        Me.labeli.Location = New System.Drawing.Point(12, 16)
        Me.labeli.Name = "labeli"
        Me.labeli.Size = New System.Drawing.Size(43, 13)
        Me.labeli.TabIndex = 0
        Me.labeli.Text = "Interno:"
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnRefresh, Me.btnSettings})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(600, 25)
        Me.ToolStrip1.TabIndex = 1
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'btnRefresh
        '
        Me.btnRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnRefresh.Image = CType(resources.GetObject("btnRefresh.Image"), System.Drawing.Image)
        Me.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(23, 22)
        Me.btnRefresh.Text = "ToolStripButton1"
        '
        'btnSettings
        '
        Me.btnSettings.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.btnSettings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnSettings.Image = CType(resources.GetObject("btnSettings.Image"), System.Drawing.Image)
        Me.btnSettings.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnSettings.Name = "btnSettings"
        Me.btnSettings.Size = New System.Drawing.Size(23, 22)
        Me.btnSettings.Text = "ToolStripButton1"
        '
        'frmPhone
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(600, 450)
        Me.Controls.Add(Me.grpInterno)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.ToolStrip1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmPhone"
        Me.Text = "Interfono"
        Me.GroupBox1.ResumeLayout(False)
        Me.grpInterno.ResumeLayout(False)
        Me.pnlInterno.ResumeLayout(False)
        Me.pnlInterno.PerformLayout()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents lstInterni As ListBox
    Friend WithEvents grpInterno As GroupBox
    Friend WithEvents pnlInterno As Panel
    Friend WithEvents lblInterno As Label
    Friend WithEvents labeli As Label
    Friend WithEvents lblUtente As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents lblPostazione As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents chkActive As CheckBox
    Friend WithEvents ToolStrip1 As ToolStrip
    Friend WithEvents btnRefresh As ToolStripButton
    Friend WithEvents btnSettings As ToolStripButton
End Class

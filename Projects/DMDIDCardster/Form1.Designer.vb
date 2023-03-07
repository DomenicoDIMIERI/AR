<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.ToolStripButton1 = New System.Windows.Forms.ToolStripButton()
        Me.btnLeftRotate = New System.Windows.Forms.ToolStripButton()
        Me.btnRightRotate = New System.Windows.Forms.ToolStripButton()
        Me.btnVFlip1 = New System.Windows.Forms.ToolStripButton()
        Me.btnHFlip1 = New System.Windows.Forms.ToolStripButton()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btnExe = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.chkHigh = New System.Windows.Forms.RadioButton()
        Me.chkQuick = New System.Windows.Forms.RadioButton()
        Me.IdCardster1 = New DMDIDCardster.IDCardSterControl()
        Me.ToolStrip1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripButton1, Me.btnLeftRotate, Me.btnRightRotate, Me.btnVFlip1, Me.btnHFlip1})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(800, 25)
        Me.ToolStrip1.TabIndex = 1
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'ToolStripButton1
        '
        Me.ToolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton1.Image = CType(resources.GetObject("ToolStripButton1.Image"), System.Drawing.Image)
        Me.ToolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton1.Name = "ToolStripButton1"
        Me.ToolStripButton1.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButton1.Text = "ToolStripButton1"
        '
        'btnLeftRotate
        '
        Me.btnLeftRotate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnLeftRotate.Enabled = False
        Me.btnLeftRotate.Image = CType(resources.GetObject("btnLeftRotate.Image"), System.Drawing.Image)
        Me.btnLeftRotate.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnLeftRotate.Name = "btnLeftRotate"
        Me.btnLeftRotate.Size = New System.Drawing.Size(23, 22)
        Me.btnLeftRotate.Text = "Ruota a sinistra"
        '
        'btnRightRotate
        '
        Me.btnRightRotate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnRightRotate.Enabled = False
        Me.btnRightRotate.Image = CType(resources.GetObject("btnRightRotate.Image"), System.Drawing.Image)
        Me.btnRightRotate.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnRightRotate.Name = "btnRightRotate"
        Me.btnRightRotate.Size = New System.Drawing.Size(23, 22)
        Me.btnRightRotate.Text = "Ruota a destra"
        '
        'btnVFlip1
        '
        Me.btnVFlip1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnVFlip1.Enabled = False
        Me.btnVFlip1.Image = CType(resources.GetObject("btnVFlip1.Image"), System.Drawing.Image)
        Me.btnVFlip1.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnVFlip1.Name = "btnVFlip1"
        Me.btnVFlip1.Size = New System.Drawing.Size(23, 22)
        Me.btnVFlip1.Text = "Capovolgi verticalmente"
        '
        'btnHFlip1
        '
        Me.btnHFlip1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnHFlip1.Enabled = False
        Me.btnHFlip1.Image = CType(resources.GetObject("btnHFlip1.Image"), System.Drawing.Image)
        Me.btnHFlip1.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnHFlip1.Name = "btnHFlip1"
        Me.btnHFlip1.Size = New System.Drawing.Size(23, 22)
        Me.btnHFlip1.Text = "Specchio"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.chkQuick)
        Me.Panel1.Controls.Add(Me.chkHigh)
        Me.Panel1.Controls.Add(Me.btnExe)
        Me.Panel1.Controls.Add(Me.btnSave)
        Me.Panel1.Controls.Add(Me.ComboBox1)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.PictureBox1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Right
        Me.Panel1.Location = New System.Drawing.Point(598, 25)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(202, 425)
        Me.Panel1.TabIndex = 2
        '
        'btnExe
        '
        Me.btnExe.Location = New System.Drawing.Point(19, 93)
        Me.btnExe.Name = "btnExe"
        Me.btnExe.Size = New System.Drawing.Size(75, 23)
        Me.btnExe.TabIndex = 5
        Me.btnExe.Text = "&Elabora"
        Me.btnExe.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Enabled = False
        Me.btnSave.Location = New System.Drawing.Point(19, 391)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(75, 23)
        Me.btnSave.TabIndex = 4
        Me.btnSave.Text = "&Salva..."
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'ComboBox1
        '
        Me.ComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Items.AddRange(New Object() {"Carta d'identità", "Tessera Sanitaria", "Patente di Guida", "Passaporto", "Foglio A4"})
        Me.ComboBox1.Location = New System.Drawing.Point(19, 43)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(164, 21)
        Me.ComboBox1.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(16, 27)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(84, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Tipo documento"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(16, 134)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(54, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Anteprima"
        '
        'PictureBox1
        '
        Me.PictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PictureBox1.Location = New System.Drawing.Point(19, 153)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(164, 232)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.TextBox1)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel2.Location = New System.Drawing.Point(0, 25)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(598, 53)
        Me.Panel2.TabIndex = 4
        '
        'TextBox1
        '
        Me.TextBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextBox1.Location = New System.Drawing.Point(0, 0)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(598, 53)
        Me.TextBox1.TabIndex = 2
        Me.TextBox1.Text = resources.GetString("TextBox1.Text")
        '
        'chkHigh
        '
        Me.chkHigh.AutoSize = True
        Me.chkHigh.Checked = True
        Me.chkHigh.Location = New System.Drawing.Point(19, 70)
        Me.chkHigh.Name = "chkHigh"
        Me.chkHigh.Size = New System.Drawing.Size(58, 17)
        Me.chkHigh.TabIndex = 6
        Me.chkHigh.TabStop = True
        Me.chkHigh.Text = "Qualità"
        Me.chkHigh.UseVisualStyleBackColor = True
        '
        'chkQuick
        '
        Me.chkQuick.AutoSize = True
        Me.chkQuick.Location = New System.Drawing.Point(100, 70)
        Me.chkQuick.Name = "chkQuick"
        Me.chkQuick.Size = New System.Drawing.Size(63, 17)
        Me.chkQuick.TabIndex = 7
        Me.chkQuick.Text = "Velocità"
        Me.chkQuick.UseVisualStyleBackColor = True
        '
        'IdCardster1
        '
        Me.IdCardster1.AutoScroll = True
        Me.IdCardster1.BackColor = System.Drawing.Color.AliceBlue
        Me.IdCardster1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.IdCardster1.Image = Nothing
        Me.IdCardster1.Location = New System.Drawing.Point(0, 78)
        Me.IdCardster1.Name = "IdCardster1"
        Me.IdCardster1.Size = New System.Drawing.Size(598, 372)
        Me.IdCardster1.TabIndex = 0
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.IdCardster1)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Name = "Form1"
        Me.Text = "Adatta il documento"
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents IdCardster1 As IDCardSterControl
    Friend WithEvents ToolStrip1 As ToolStrip
    Friend WithEvents ToolStripButton1 As ToolStripButton
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Label1 As Label
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents Panel2 As Panel
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents ComboBox1 As ComboBox
    Friend WithEvents Label2 As Label
    Friend WithEvents btnSave As Button
    Friend WithEvents btnExe As Button
    Friend WithEvents btnLeftRotate As ToolStripButton
    Friend WithEvents btnRightRotate As ToolStripButton
    Friend WithEvents btnVFlip1 As ToolStripButton
    Friend WithEvents btnHFlip1 As ToolStripButton
    Friend WithEvents chkHigh As RadioButton
    Friend WithEvents chkQuick As RadioButton
End Class

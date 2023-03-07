<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRingTimes
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
        Me.lstOrari = New System.Windows.Forms.ListBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtOre = New System.Windows.Forms.NumericUpDown()
        Me.txtMinuti = New System.Windows.Forms.NumericUpDown()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.btnRemove = New System.Windows.Forms.Button()
        Me.btnOk = New System.Windows.Forms.Button()
        Me.chkLun = New System.Windows.Forms.CheckBox()
        Me.chkMar = New System.Windows.Forms.CheckBox()
        Me.chkMer = New System.Windows.Forms.CheckBox()
        Me.chkGio = New System.Windows.Forms.CheckBox()
        Me.chkVen = New System.Windows.Forms.CheckBox()
        Me.chkSab = New System.Windows.Forms.CheckBox()
        Me.chkDom = New System.Windows.Forms.CheckBox()
        CType(Me.txtOre, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtMinuti, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lstOrari
        '
        Me.lstOrari.FormattingEnabled = True
        Me.lstOrari.Location = New System.Drawing.Point(12, 36)
        Me.lstOrari.Name = "lstOrari"
        Me.lstOrari.Size = New System.Drawing.Size(156, 199)
        Me.lstOrari.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(199, 36)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(27, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Ore:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(256, 36)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(38, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Minuti:"
        '
        'txtOre
        '
        Me.txtOre.Location = New System.Drawing.Point(202, 52)
        Me.txtOre.Maximum = New Decimal(New Integer() {23, 0, 0, 0})
        Me.txtOre.Name = "txtOre"
        Me.txtOre.Size = New System.Drawing.Size(51, 20)
        Me.txtOre.TabIndex = 3
        '
        'txtMinuti
        '
        Me.txtMinuti.Location = New System.Drawing.Point(259, 52)
        Me.txtMinuti.Maximum = New Decimal(New Integer() {59, 0, 0, 0})
        Me.txtMinuti.Name = "txtMinuti"
        Me.txtMinuti.Size = New System.Drawing.Size(51, 20)
        Me.txtMinuti.TabIndex = 4
        '
        'btnAdd
        '
        Me.btnAdd.Location = New System.Drawing.Point(329, 52)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(75, 23)
        Me.btnAdd.TabIndex = 7
        Me.btnAdd.Text = "Aggiungi"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'btnRemove
        '
        Me.btnRemove.Location = New System.Drawing.Point(329, 81)
        Me.btnRemove.Name = "btnRemove"
        Me.btnRemove.Size = New System.Drawing.Size(75, 23)
        Me.btnRemove.TabIndex = 8
        Me.btnRemove.Text = "Rimuovi"
        Me.btnRemove.UseVisualStyleBackColor = True
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(329, 249)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(75, 23)
        Me.btnOk.TabIndex = 9
        Me.btnOk.Text = "Ok"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'chkLun
        '
        Me.chkLun.AutoSize = True
        Me.chkLun.Location = New System.Drawing.Point(202, 85)
        Me.chkLun.Name = "chkLun"
        Me.chkLun.Size = New System.Drawing.Size(58, 17)
        Me.chkLun.TabIndex = 10
        Me.chkLun.Text = "Lunedì"
        Me.chkLun.UseVisualStyleBackColor = True
        '
        'chkMar
        '
        Me.chkMar.AutoSize = True
        Me.chkMar.Location = New System.Drawing.Point(202, 108)
        Me.chkMar.Name = "chkMar"
        Me.chkMar.Size = New System.Drawing.Size(61, 17)
        Me.chkMar.TabIndex = 11
        Me.chkMar.Text = "Martedì"
        Me.chkMar.UseVisualStyleBackColor = True
        '
        'chkMer
        '
        Me.chkMer.AutoSize = True
        Me.chkMer.Location = New System.Drawing.Point(202, 131)
        Me.chkMer.Name = "chkMer"
        Me.chkMer.Size = New System.Drawing.Size(72, 17)
        Me.chkMer.TabIndex = 12
        Me.chkMer.Text = "Mercoledì"
        Me.chkMer.UseVisualStyleBackColor = True
        '
        'chkGio
        '
        Me.chkGio.AutoSize = True
        Me.chkGio.Location = New System.Drawing.Point(202, 154)
        Me.chkGio.Name = "chkGio"
        Me.chkGio.Size = New System.Drawing.Size(62, 17)
        Me.chkGio.TabIndex = 13
        Me.chkGio.Text = "Giovedì"
        Me.chkGio.UseVisualStyleBackColor = True
        '
        'chkVen
        '
        Me.chkVen.AutoSize = True
        Me.chkVen.Location = New System.Drawing.Point(202, 177)
        Me.chkVen.Name = "chkVen"
        Me.chkVen.Size = New System.Drawing.Size(62, 17)
        Me.chkVen.TabIndex = 14
        Me.chkVen.Text = "Venerdì"
        Me.chkVen.UseVisualStyleBackColor = True
        '
        'chkSab
        '
        Me.chkSab.AutoSize = True
        Me.chkSab.Location = New System.Drawing.Point(202, 200)
        Me.chkSab.Name = "chkSab"
        Me.chkSab.Size = New System.Drawing.Size(60, 17)
        Me.chkSab.TabIndex = 15
        Me.chkSab.Text = "Sabato"
        Me.chkSab.UseVisualStyleBackColor = True
        '
        'chkDom
        '
        Me.chkDom.AutoSize = True
        Me.chkDom.Location = New System.Drawing.Point(202, 223)
        Me.chkDom.Name = "chkDom"
        Me.chkDom.Size = New System.Drawing.Size(74, 17)
        Me.chkDom.TabIndex = 16
        Me.chkDom.Text = "Domenica"
        Me.chkDom.UseVisualStyleBackColor = True
        '
        'frmRingTimes
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(413, 284)
        Me.Controls.Add(Me.chkDom)
        Me.Controls.Add(Me.chkSab)
        Me.Controls.Add(Me.chkVen)
        Me.Controls.Add(Me.chkGio)
        Me.Controls.Add(Me.chkMer)
        Me.Controls.Add(Me.chkMar)
        Me.Controls.Add(Me.chkLun)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.btnRemove)
        Me.Controls.Add(Me.btnAdd)
        Me.Controls.Add(Me.txtMinuti)
        Me.Controls.Add(Me.txtOre)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lstOrari)
        Me.Name = "frmRingTimes"
        Me.Text = "Definizione Orari Sirena"
        CType(Me.txtOre, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtMinuti, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lstOrari As ListBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents txtOre As NumericUpDown
    Friend WithEvents txtMinuti As NumericUpDown
    Friend WithEvents btnAdd As Button
    Friend WithEvents btnRemove As Button
    Friend WithEvents btnOk As Button
    Friend WithEvents chkLun As CheckBox
    Friend WithEvents chkMar As CheckBox
    Friend WithEvents chkMer As CheckBox
    Friend WithEvents chkGio As CheckBox
    Friend WithEvents chkVen As CheckBox
    Friend WithEvents chkSab As CheckBox
    Friend WithEvents chkDom As CheckBox
End Class

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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtDataNascita = New System.Windows.Forms.DateTimePicker()
        Me.txtDataAssunzione = New System.Windows.Forms.DateTimePicker()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.cboSesso = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtRata = New System.Windows.Forms.TextBox()
        Me.cboDurata = New System.Windows.Forms.ComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtDecorrenza = New System.Windows.Forms.DateTimePicker()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtProdotto = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtConenzione = New System.Windows.Forms.TextBox()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.btnAna = New System.Windows.Forms.Button()
        Me.WebBrowser1 = New System.Windows.Forms.WebBrowser()
        Me.txtProvv = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(23, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(72, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Data Nascita:"
        '
        'txtDataNascita
        '
        Me.txtDataNascita.CustomFormat = "dd/MM/yyyy"
        Me.txtDataNascita.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.txtDataNascita.Location = New System.Drawing.Point(101, 3)
        Me.txtDataNascita.Name = "txtDataNascita"
        Me.txtDataNascita.Size = New System.Drawing.Size(101, 20)
        Me.txtDataNascita.TabIndex = 1
        '
        'txtDataAssunzione
        '
        Me.txtDataAssunzione.CustomFormat = "dd/MM/yyyy"
        Me.txtDataAssunzione.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.txtDataAssunzione.Location = New System.Drawing.Point(324, 3)
        Me.txtDataAssunzione.Name = "txtDataAssunzione"
        Me.txtDataAssunzione.Size = New System.Drawing.Size(101, 20)
        Me.txtDataAssunzione.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(228, 8)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(90, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Data Assunzione:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(23, 26)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(39, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Sesso:"
        '
        'cboSesso
        '
        Me.cboSesso.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.cboSesso.FormattingEnabled = True
        Me.cboSesso.Items.AddRange(New Object() {"M", "F"})
        Me.cboSesso.Location = New System.Drawing.Point(101, 23)
        Me.cboSesso.Name = "cboSesso"
        Me.cboSesso.Size = New System.Drawing.Size(101, 21)
        Me.cboSesso.TabIndex = 5
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(23, 73)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(33, 13)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Rata:"
        '
        'txtRata
        '
        Me.txtRata.Location = New System.Drawing.Point(101, 70)
        Me.txtRata.Name = "txtRata"
        Me.txtRata.Size = New System.Drawing.Size(100, 20)
        Me.txtRata.TabIndex = 7
        '
        'cboDurata
        '
        Me.cboDurata.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.cboDurata.FormattingEnabled = True
        Me.cboDurata.Items.AddRange(New Object() {"120", "108", "96", "84", "72", "60", "48", "36", "24", "12"})
        Me.cboDurata.Location = New System.Drawing.Point(101, 90)
        Me.cboDurata.Name = "cboDurata"
        Me.cboDurata.Size = New System.Drawing.Size(101, 21)
        Me.cboDurata.TabIndex = 8
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(23, 93)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(42, 13)
        Me.Label5.TabIndex = 9
        Me.Label5.Text = "Durata:"
        '
        'txtDecorrenza
        '
        Me.txtDecorrenza.CustomFormat = "dd/MM/yyyy"
        Me.txtDecorrenza.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.txtDecorrenza.Location = New System.Drawing.Point(324, 70)
        Me.txtDecorrenza.Name = "txtDecorrenza"
        Me.txtDecorrenza.Size = New System.Drawing.Size(101, 20)
        Me.txtDecorrenza.TabIndex = 11
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(228, 73)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(65, 13)
        Me.Label6.TabIndex = 10
        Me.Label6.Text = "Decorrenza:"
        '
        'txtProdotto
        '
        Me.txtProdotto.Location = New System.Drawing.Point(101, 50)
        Me.txtProdotto.Name = "txtProdotto"
        Me.txtProdotto.Size = New System.Drawing.Size(324, 20)
        Me.txtProdotto.TabIndex = 12
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(23, 53)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(50, 13)
        Me.Label7.TabIndex = 13
        Me.Label7.Text = "Prodotto:"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(432, 53)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(72, 13)
        Me.Label8.TabIndex = 15
        Me.Label8.Text = "Convenzione:"
        '
        'txtConenzione
        '
        Me.txtConenzione.Location = New System.Drawing.Point(510, 50)
        Me.txtConenzione.Name = "txtConenzione"
        Me.txtConenzione.Size = New System.Drawing.Size(115, 20)
        Me.txtConenzione.TabIndex = 14
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(26, 135)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.TextBox1.Size = New System.Drawing.Size(599, 176)
        Me.TextBox1.TabIndex = 16
        '
        'btnAna
        '
        Me.btnAna.Location = New System.Drawing.Point(640, 135)
        Me.btnAna.Name = "btnAna"
        Me.btnAna.Size = New System.Drawing.Size(75, 23)
        Me.btnAna.TabIndex = 17
        Me.btnAna.Text = "Analizza"
        Me.btnAna.UseVisualStyleBackColor = True
        '
        'WebBrowser1
        '
        Me.WebBrowser1.Location = New System.Drawing.Point(640, 173)
        Me.WebBrowser1.MinimumSize = New System.Drawing.Size(20, 20)
        Me.WebBrowser1.Name = "WebBrowser1"
        Me.WebBrowser1.Size = New System.Drawing.Size(250, 250)
        Me.WebBrowser1.TabIndex = 18
        Me.WebBrowser1.Visible = False
        '
        'txtProvv
        '
        Me.txtProvv.Location = New System.Drawing.Point(101, 109)
        Me.txtProvv.Name = "txtProvv"
        Me.txtProvv.Size = New System.Drawing.Size(100, 20)
        Me.txtProvv.TabIndex = 20
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(24, 112)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(49, 13)
        Me.Label9.TabIndex = 19
        Me.Label9.Text = "Provv. %"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(870, 323)
        Me.Controls.Add(Me.txtProvv)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.WebBrowser1)
        Me.Controls.Add(Me.btnAna)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.txtConenzione)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.txtProdotto)
        Me.Controls.Add(Me.txtDecorrenza)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.cboDurata)
        Me.Controls.Add(Me.txtRata)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.cboSesso)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtDataAssunzione)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtDataNascita)
        Me.Controls.Add(Me.Label1)
        Me.Name = "Form1"
        Me.Text = "Primo Network Analyzer"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents txtDataNascita As DateTimePicker
    Friend WithEvents txtDataAssunzione As DateTimePicker
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents cboSesso As ComboBox
    Friend WithEvents Label4 As Label
    Friend WithEvents txtRata As TextBox
    Friend WithEvents cboDurata As ComboBox
    Friend WithEvents Label5 As Label
    Friend WithEvents txtDecorrenza As DateTimePicker
    Friend WithEvents Label6 As Label
    Friend WithEvents txtProdotto As TextBox
    Friend WithEvents Label7 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents txtConenzione As TextBox
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents btnAna As Button
    Friend WithEvents WebBrowser1 As WebBrowser
    Friend WithEvents txtProvv As TextBox
    Friend WithEvents Label9 As Label
End Class

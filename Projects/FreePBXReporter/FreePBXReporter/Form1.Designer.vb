<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
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
    'Non modificarla nell'editor del codice.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtIn = New System.Windows.Forms.TextBox()
        Me.btnBrowse = New System.Windows.Forms.Button()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtOut = New System.Windows.Forms.TextBox()
        Me.btnGo = New System.Windows.Forms.Button()
        Me.btnGoMDB = New System.Windows.Forms.Button()
        Me.txtMDBFile = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtPBX = New System.Windows.Forms.ComboBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(46, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "File csv:"
        '
        'txtIn
        '
        Me.txtIn.Location = New System.Drawing.Point(64, 10)
        Me.txtIn.Name = "txtIn"
        Me.txtIn.Size = New System.Drawing.Size(301, 20)
        Me.txtIn.TabIndex = 1
        '
        'btnBrowse
        '
        Me.btnBrowse.Location = New System.Drawing.Point(371, 10)
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.Size = New System.Drawing.Size(39, 21)
        Me.btnBrowse.TabIndex = 2
        Me.btnBrowse.Text = "..."
        Me.btnBrowse.UseVisualStyleBackColor = True
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.DefaultExt = "csv"
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        Me.OpenFileDialog1.Filter = "File csv|*.csv|Tutti i files|*.*"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 67)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(41, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "File xls:"
        '
        'txtOut
        '
        Me.txtOut.Location = New System.Drawing.Point(64, 64)
        Me.txtOut.Name = "txtOut"
        Me.txtOut.Size = New System.Drawing.Size(301, 20)
        Me.txtOut.TabIndex = 4
        '
        'btnGo
        '
        Me.btnGo.Location = New System.Drawing.Point(416, 66)
        Me.btnGo.Name = "btnGo"
        Me.btnGo.Size = New System.Drawing.Size(39, 21)
        Me.btnGo.TabIndex = 5
        Me.btnGo.Text = ">>"
        Me.btnGo.UseVisualStyleBackColor = True
        '
        'btnGoMDB
        '
        Me.btnGoMDB.Location = New System.Drawing.Point(416, 93)
        Me.btnGoMDB.Name = "btnGoMDB"
        Me.btnGoMDB.Size = New System.Drawing.Size(39, 21)
        Me.btnGoMDB.TabIndex = 8
        Me.btnGoMDB.Text = ">>"
        Me.btnGoMDB.UseVisualStyleBackColor = True
        '
        'txtMDBFile
        '
        Me.txtMDBFile.Location = New System.Drawing.Point(64, 90)
        Me.txtMDBFile.Name = "txtMDBFile"
        Me.txtMDBFile.Size = New System.Drawing.Size(301, 20)
        Me.txtMDBFile.TabIndex = 7
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 93)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(49, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "File mdb:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(12, 38)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(57, 13)
        Me.Label4.TabIndex = 9
        Me.Label4.Text = "Centralino:"
        '
        'txtPBX
        '
        Me.txtPBX.Items.AddRange(New String() {"Avellino", "Benevento", "Napoli", "Salerno", "San Pietro al Tanagro", "Teggiano"})
        Me.txtPBX.DropDownStyle = ComboBoxStyle.DropDown
        Me.txtPBX.Location = New System.Drawing.Point(64, 38)
        Me.txtPBX.Name = "txtPBX"
        Me.txtPBX.Size = New System.Drawing.Size(301, 20)
        Me.txtPBX.TabIndex = 10
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(371, 67)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(39, 21)
        Me.Button1.TabIndex = 11
        Me.Button1.Text = "..."
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(371, 93)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(39, 21)
        Me.Button2.TabIndex = 12
        Me.Button2.Text = "..."
        Me.Button2.UseVisualStyleBackColor = True
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Location = New System.Drawing.Point(64, 10)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(301, 18)
        Me.ProgressBar1.TabIndex = 13
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(371, 10)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(84, 21)
        Me.Button3.TabIndex = 14
        Me.Button3.Text = "Annulla"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Button2)
        Me.Panel1.Controls.Add(Me.Button1)
        Me.Panel1.Controls.Add(Me.btnGoMDB)
        Me.Panel1.Controls.Add(Me.txtPBX)
        Me.Panel1.Controls.Add(Me.btnGo)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.txtMDBFile)
        Me.Panel1.Controls.Add(Me.txtIn)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.btnBrowse)
        Me.Panel1.Controls.Add(Me.txtOut)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(472, 127)
        Me.Panel1.TabIndex = 15
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.ProgressBar1)
        Me.Panel2.Controls.Add(Me.Button3)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel2.Location = New System.Drawing.Point(0, 127)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(472, 46)
        Me.Panel2.TabIndex = 16
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(472, 173)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Panel2)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "frmMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "FreePBXReporter"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtIn As System.Windows.Forms.TextBox
    Friend WithEvents btnBrowse As System.Windows.Forms.Button
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtOut As System.Windows.Forms.TextBox
    Friend WithEvents btnGo As System.Windows.Forms.Button
    Friend WithEvents btnGoMDB As System.Windows.Forms.Button
    Friend WithEvents txtMDBFile As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtPBX As System.Windows.Forms.ComboBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel

End Class

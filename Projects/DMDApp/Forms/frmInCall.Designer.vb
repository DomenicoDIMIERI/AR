<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInCall
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
        Me.picImage = New System.Windows.Forms.PictureBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtNumero = New System.Windows.Forms.Label()
        Me.txtMittente = New System.Windows.Forms.Label()
        Me.txtInfo = New System.Windows.Forms.Label()
        CType(Me.picImage, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'picImage
        '
        Me.picImage.Location = New System.Drawing.Point(12, 12)
        Me.picImage.Name = "picImage"
        Me.picImage.Size = New System.Drawing.Size(79, 82)
        Me.picImage.TabIndex = 0
        Me.picImage.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Blue
        Me.Label1.Location = New System.Drawing.Point(97, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(232, 25)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Chiamata in ingresso..."
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(98, 49)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(84, 24)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Numero:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(98, 82)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(80, 24)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Mittente:"
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.Color.Green
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(7, 162)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(150, 25)
        Me.Label4.TabIndex = 10
        Me.Label4.Text = "Risposto"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label5
        '
        Me.Label5.BackColor = System.Drawing.Color.Red
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.White
        Me.Label5.Location = New System.Drawing.Point(337, 162)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(150, 25)
        Me.Label5.TabIndex = 11
        Me.Label5.Text = "Rifiuta"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtNumero
        '
        Me.txtNumero.AutoSize = True
        Me.txtNumero.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNumero.ForeColor = System.Drawing.Color.Blue
        Me.txtNumero.Location = New System.Drawing.Point(188, 49)
        Me.txtNumero.Name = "txtNumero"
        Me.txtNumero.Size = New System.Drawing.Size(25, 24)
        Me.txtNumero.TabIndex = 12
        Me.txtNumero.Text = "..."
        '
        'txtMittente
        '
        Me.txtMittente.AutoSize = True
        Me.txtMittente.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMittente.ForeColor = System.Drawing.Color.Blue
        Me.txtMittente.Location = New System.Drawing.Point(188, 82)
        Me.txtMittente.Name = "txtMittente"
        Me.txtMittente.Size = New System.Drawing.Size(25, 24)
        Me.txtMittente.TabIndex = 13
        Me.txtMittente.Text = "..."
        '
        'txtInfo
        '
        Me.txtInfo.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInfo.ForeColor = System.Drawing.Color.Blue
        Me.txtInfo.Location = New System.Drawing.Point(8, 117)
        Me.txtInfo.Name = "txtInfo"
        Me.txtInfo.Size = New System.Drawing.Size(479, 33)
        Me.txtInfo.TabIndex = 14
        Me.txtInfo.Text = "..."
        '
        'frmInCall
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(499, 196)
        Me.Controls.Add(Me.txtInfo)
        Me.Controls.Add(Me.txtMittente)
        Me.Controls.Add(Me.txtNumero)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.picImage)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmInCall"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Chiamata in ingresso..."
        CType(Me.picImage, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents picImage As PictureBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents txtNumero As Label
    Friend WithEvents txtMittente As Label
    Friend WithEvents txtInfo As Label
End Class

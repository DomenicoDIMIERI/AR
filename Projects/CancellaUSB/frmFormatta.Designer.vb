<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmFormatta
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
        Me.cboDrive = New System.Windows.Forms.ComboBox()
        Me.btnCancella = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(15, 28)
        Me.Label1.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(236, 24)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Seleziona l'unità rimovibile:"
        '
        'cboDrive
        '
        Me.cboDrive.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboDrive.FormattingEnabled = True
        Me.cboDrive.Location = New System.Drawing.Point(260, 25)
        Me.cboDrive.Name = "cboDrive"
        Me.cboDrive.Size = New System.Drawing.Size(225, 32)
        Me.cboDrive.TabIndex = 1
        '
        'btnCancella
        '
        Me.btnCancella.Enabled = False
        Me.btnCancella.Location = New System.Drawing.Point(355, 107)
        Me.btnCancella.Name = "btnCancella"
        Me.btnCancella.Size = New System.Drawing.Size(130, 39)
        Me.btnCancella.TabIndex = 2
        Me.btnCancella.Text = "Cancella..."
        Me.btnCancella.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(11.0!, 24.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(515, 158)
        Me.Controls.Add(Me.btnCancella)
        Me.Controls.Add(Me.cboDrive)
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.Name = "Form1"
        Me.Text = "Formattazione sicura di un dispositivo rimovibile"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents cboDrive As ComboBox
    Friend WithEvents btnCancella As Button
End Class

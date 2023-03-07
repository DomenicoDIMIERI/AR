<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class IPAddressControl
    Inherits System.Windows.Forms.UserControl

    'UserControl esegue l'override del metodo Dispose per pulire l'elenco dei componenti.
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
        Me.Label4 = New System.Windows.Forms.Label()
        Me.I0 = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.I1 = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.I2 = New System.Windows.Forms.TextBox()
        Me.I3 = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(193, 6)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(10, 13)
        Me.Label4.TabIndex = 15
        Me.Label4.Text = "."
        '
        'I0
        '
        Me.I0.Location = New System.Drawing.Point(206, 3)
        Me.I0.Name = "I0"
        Me.I0.Size = New System.Drawing.Size(53, 20)
        Me.I0.TabIndex = 14
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(126, 6)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(10, 13)
        Me.Label3.TabIndex = 13
        Me.Label3.Text = "."
        '
        'I1
        '
        Me.I1.Location = New System.Drawing.Point(139, 3)
        Me.I1.Name = "I1"
        Me.I1.Size = New System.Drawing.Size(53, 20)
        Me.I1.TabIndex = 12
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(58, 6)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(10, 13)
        Me.Label2.TabIndex = 11
        Me.Label2.Text = "."
        '
        'I2
        '
        Me.I2.Location = New System.Drawing.Point(71, 3)
        Me.I2.Name = "I2"
        Me.I2.Size = New System.Drawing.Size(53, 20)
        Me.I2.TabIndex = 10
        '
        'I3
        '
        Me.I3.Location = New System.Drawing.Point(3, 3)
        Me.I3.Name = "I3"
        Me.I3.Size = New System.Drawing.Size(53, 20)
        Me.I3.TabIndex = 9
        '
        'IPAddressControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.I0)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.I1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.I2)
        Me.Controls.Add(Me.I3)
        Me.Name = "IPAddressControl"
        Me.Size = New System.Drawing.Size(267, 29)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label4 As Label
    Friend WithEvents I0 As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents I1 As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents I2 As TextBox
    Friend WithEvents I3 As TextBox
End Class

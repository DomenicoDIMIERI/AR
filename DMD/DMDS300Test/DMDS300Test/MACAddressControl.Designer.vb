<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MACAddressControl
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
        Me.I2 = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.I3 = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.I4 = New System.Windows.Forms.TextBox()
        Me.I5 = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.I1 = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.I0 = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(119, 6)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(10, 13)
        Me.Label4.TabIndex = 22
        Me.Label4.Text = "."
        '
        'I2
        '
        Me.I2.Location = New System.Drawing.Point(128, 3)
        Me.I2.Name = "I2"
        Me.I2.Size = New System.Drawing.Size(33, 20)
        Me.I2.TabIndex = 21
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(76, 6)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(10, 13)
        Me.Label3.TabIndex = 20
        Me.Label3.Text = "."
        '
        'I3
        '
        Me.I3.Location = New System.Drawing.Point(86, 3)
        Me.I3.Name = "I3"
        Me.I3.Size = New System.Drawing.Size(33, 20)
        Me.I3.TabIndex = 19
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(34, 6)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(10, 13)
        Me.Label2.TabIndex = 18
        Me.Label2.Text = "."
        '
        'I4
        '
        Me.I4.Location = New System.Drawing.Point(42, 3)
        Me.I4.Name = "I4"
        Me.I4.Size = New System.Drawing.Size(33, 20)
        Me.I4.TabIndex = 17
        '
        'I5
        '
        Me.I5.Location = New System.Drawing.Point(3, 3)
        Me.I5.Name = "I5"
        Me.I5.Size = New System.Drawing.Size(33, 20)
        Me.I5.TabIndex = 16
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(161, 6)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(10, 13)
        Me.Label1.TabIndex = 24
        Me.Label1.Text = "."
        '
        'I1
        '
        Me.I1.Location = New System.Drawing.Point(170, 3)
        Me.I1.Name = "I1"
        Me.I1.Size = New System.Drawing.Size(33, 20)
        Me.I1.TabIndex = 23
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(203, 6)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(10, 13)
        Me.Label5.TabIndex = 26
        Me.Label5.Text = "."
        '
        'I0
        '
        Me.I0.Location = New System.Drawing.Point(212, 3)
        Me.I0.Name = "I0"
        Me.I0.Size = New System.Drawing.Size(33, 20)
        Me.I0.TabIndex = 25
        '
        'MACAddressControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.I0)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.I1)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.I2)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.I3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.I4)
        Me.Controls.Add(Me.I5)
        Me.Name = "MACAddressControl"
        Me.Size = New System.Drawing.Size(251, 30)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label4 As Label
    Friend WithEvents I2 As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents I3 As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents I4 As TextBox
    Friend WithEvents I5 As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents I1 As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents I0 As TextBox
End Class

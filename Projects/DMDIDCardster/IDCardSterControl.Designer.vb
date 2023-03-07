<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class IDCardSterControl
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(IDCardSterControl))
        Me.IdCardster1 = New DMDIDCardster.IDCardster()
        Me.SuspendLayout()
        '
        'IdCardster1
        '
        Me.IdCardster1.Image = Nothing
        Me.IdCardster1.Location = New System.Drawing.Point(3, 3)
        Me.IdCardster1.Name = "IdCardster1"
        Me.IdCardster1.Size = New System.Drawing.Size(679, 357)
        Me.IdCardster1.TabIndex = 0
        '
        'IDCardSterControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.Controls.Add(Me.IdCardster1)
        Me.Name = "IDCardSterControl"
        Me.Size = New System.Drawing.Size(699, 357)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents IdCardster1 As IDCardster
End Class

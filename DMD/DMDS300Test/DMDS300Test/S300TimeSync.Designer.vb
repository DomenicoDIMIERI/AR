<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class S300TimeSync
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
        Me.components = New System.ComponentModel.Container()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtOraLocale = New System.Windows.Forms.TextBox()
        Me.txtOraDispositivo = New System.Windows.Forms.TextBox()
        Me.btnSet = New System.Windows.Forms.Button()
        Me.btnSync = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Timer1
        '
        Me.Timer1.Interval = 1000
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(3, 3)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(58, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Ora locale:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(3, 29)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(79, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Ora dispositivo:"
        '
        'txtOraLocale
        '
        Me.txtOraLocale.Location = New System.Drawing.Point(93, 0)
        Me.txtOraLocale.Name = "txtOraLocale"
        Me.txtOraLocale.ReadOnly = True
        Me.txtOraLocale.Size = New System.Drawing.Size(159, 20)
        Me.txtOraLocale.TabIndex = 2
        '
        'txtOraDispositivo
        '
        Me.txtOraDispositivo.Location = New System.Drawing.Point(93, 26)
        Me.txtOraDispositivo.Name = "txtOraDispositivo"
        Me.txtOraDispositivo.Size = New System.Drawing.Size(159, 20)
        Me.txtOraDispositivo.TabIndex = 3
        '
        'btnSet
        '
        Me.btnSet.Location = New System.Drawing.Point(258, 24)
        Me.btnSet.Name = "btnSet"
        Me.btnSet.Size = New System.Drawing.Size(69, 23)
        Me.btnSet.TabIndex = 4
        Me.btnSet.Text = "Imposta"
        Me.btnSet.UseVisualStyleBackColor = True
        '
        'btnSync
        '
        Me.btnSync.Location = New System.Drawing.Point(258, -1)
        Me.btnSync.Name = "btnSync"
        Me.btnSync.Size = New System.Drawing.Size(69, 23)
        Me.btnSync.TabIndex = 5
        Me.btnSync.Text = "Sincronizza"
        Me.btnSync.UseVisualStyleBackColor = True
        '
        'S300TimeSync
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.btnSync)
        Me.Controls.Add(Me.btnSet)
        Me.Controls.Add(Me.txtOraDispositivo)
        Me.Controls.Add(Me.txtOraLocale)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Name = "S300TimeSync"
        Me.Size = New System.Drawing.Size(330, 57)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Timer1 As Timer
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents txtOraLocale As TextBox
    Friend WithEvents txtOraDispositivo As TextBox
    Friend WithEvents btnSet As Button
    Friend WithEvents btnSync As Button
End Class

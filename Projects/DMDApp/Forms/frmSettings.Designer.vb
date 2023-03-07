<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSettings
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSettings))
        Me.chkAutoStat = New System.Windows.Forms.CheckBox()
        Me.chkRegisterDialtp = New System.Windows.Forms.CheckBox()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'chkAutoStat
        '
        Me.chkAutoStat.AutoSize = True
        Me.chkAutoStat.Location = New System.Drawing.Point(12, 12)
        Me.chkAutoStat.Name = "chkAutoStat"
        Me.chkAutoStat.Size = New System.Drawing.Size(183, 17)
        Me.chkAutoStat.TabIndex = 1
        Me.chkAutoStat.Text = "Avvio automatico con il computer"
        Me.chkAutoStat.UseVisualStyleBackColor = True
        '
        'chkRegisterDialtp
        '
        Me.chkRegisterDialtp.AutoSize = True
        Me.chkRegisterDialtp.Location = New System.Drawing.Point(12, 35)
        Me.chkRegisterDialtp.Name = "chkRegisterDialtp"
        Me.chkRegisterDialtp.Size = New System.Drawing.Size(149, 17)
        Me.chkRegisterDialtp.TabIndex = 2
        Me.chkRegisterDialtp.Text = "Registra il protocollo dialtp"
        Me.chkRegisterDialtp.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Location = New System.Drawing.Point(350, 250)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 23)
        Me.btnClose.TabIndex = 3
        Me.btnClose.Text = "&Chiudi"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'frmSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(437, 285)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.chkRegisterDialtp)
        Me.Controls.Add(Me.chkAutoStat)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmSettings"
        Me.Text = "Impostazioni generali"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents chkAutoStat As CheckBox
    Friend WithEvents chkRegisterDialtp As CheckBox
    Friend WithEvents btnClose As Button
End Class

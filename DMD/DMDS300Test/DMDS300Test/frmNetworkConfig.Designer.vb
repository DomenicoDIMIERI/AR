<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmNetworkConfig
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
        Me.btnOk = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.DNS = New DMDS300Test.IPAddressControl()
        Me.Gateway = New DMDS300Test.IPAddressControl()
        Me.Mask = New DMDS300Test.IPAddressControl()
        Me.IP = New DMDS300Test.IPAddressControl()
        Me.MAC = New DMDS300Test.MACAddressControl()
        Me.SuspendLayout()
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(267, 235)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(75, 23)
        Me.btnOk.TabIndex = 17
        Me.btnOk.Text = "Ok"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(20, 13)
        Me.Label1.TabIndex = 9
        Me.Label1.Text = "IP:"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(12, 35)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(36, 13)
        Me.Label8.TabIndex = 18
        Me.Label8.Text = "Mask:"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(12, 61)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(52, 13)
        Me.Label12.TabIndex = 26
        Me.Label12.Text = "Gateway:"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(12, 87)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(33, 13)
        Me.Label16.TabIndex = 34
        Me.Label16.Text = "DNS:"
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Location = New System.Drawing.Point(13, 128)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(33, 13)
        Me.Label20.TabIndex = 42
        Me.Label20.Text = "MAC:"
        '
        'DNS
        '
        Me.DNS.Location = New System.Drawing.Point(75, 81)
        Me.DNS.Name = "DNS"
        Me.DNS.Size = New System.Drawing.Size(267, 29)
        Me.DNS.TabIndex = 62
        Me.DNS.Value = "..."
        '
        'Gateway
        '
        Me.Gateway.Location = New System.Drawing.Point(75, 55)
        Me.Gateway.Name = "Gateway"
        Me.Gateway.Size = New System.Drawing.Size(267, 29)
        Me.Gateway.TabIndex = 61
        Me.Gateway.Value = "..."
        '
        'Mask
        '
        Me.Mask.Location = New System.Drawing.Point(75, 29)
        Me.Mask.Name = "Mask"
        Me.Mask.Size = New System.Drawing.Size(267, 29)
        Me.Mask.TabIndex = 60
        Me.Mask.Value = "..."
        '
        'IP
        '
        Me.IP.Location = New System.Drawing.Point(75, 4)
        Me.IP.Name = "IP"
        Me.IP.Size = New System.Drawing.Size(267, 29)
        Me.IP.TabIndex = 59
        Me.IP.Value = "..."
        '
        'MAC
        '
        Me.MAC.Location = New System.Drawing.Point(75, 123)
        Me.MAC.Name = "MAC"
        Me.MAC.Size = New System.Drawing.Size(251, 30)
        Me.MAC.TabIndex = 63
        Me.MAC.Value = ":::::"
        '
        'frmNetworkConfig
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(354, 270)
        Me.Controls.Add(Me.MAC)
        Me.Controls.Add(Me.DNS)
        Me.Controls.Add(Me.Gateway)
        Me.Controls.Add(Me.Mask)
        Me.Controls.Add(Me.IP)
        Me.Controls.Add(Me.Label20)
        Me.Controls.Add(Me.Label16)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "frmNetworkConfig"
        Me.Text = "Configurazione di rete"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnOk As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents Label16 As Label
    Friend WithEvents Label20 As Label
    Friend WithEvents IP As IPAddressControl
    Friend WithEvents Mask As IPAddressControl
    Friend WithEvents Gateway As IPAddressControl
    Friend WithEvents DNS As IPAddressControl
    Friend WithEvents MAC As MACAddressControl
End Class

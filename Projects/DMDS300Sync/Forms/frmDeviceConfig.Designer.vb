<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDeviceConfig
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
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.btnDelMarcature = New System.Windows.Forms.Button()
        Me.txtNome = New System.Windows.Forms.TextBox()
        Me.lblNome = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtSyncTimeInterval = New System.Windows.Forms.NumericUpDown()
        Me.txtAddress = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtDeviceID = New System.Windows.Forms.NumericUpDown()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btnOk = New System.Windows.Forms.Button()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.txtUserMappings = New System.Windows.Forms.TextBox()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.GroupBox1.SuspendLayout()
        CType(Me.txtSyncTimeInterval, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtDeviceID, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Button3)
        Me.GroupBox1.Controls.Add(Me.Button1)
        Me.GroupBox1.Controls.Add(Me.Button2)
        Me.GroupBox1.Controls.Add(Me.btnDelMarcature)
        Me.GroupBox1.Controls.Add(Me.txtNome)
        Me.GroupBox1.Controls.Add(Me.lblNome)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.txtSyncTimeInterval)
        Me.GroupBox1.Controls.Add(Me.txtAddress)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.txtDeviceID)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Top
        Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(602, 128)
        Me.GroupBox1.TabIndex = 22
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Dispositivo:"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(410, 37)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(180, 23)
        Me.Button1.TabIndex = 21
        Me.Button1.Text = "Scarica nuove marcature"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(410, 87)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(180, 23)
        Me.Button2.TabIndex = 18
        Me.Button2.Text = "Gestione Utenti"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'btnDelMarcature
        '
        Me.btnDelMarcature.Location = New System.Drawing.Point(410, 12)
        Me.btnDelMarcature.Name = "btnDelMarcature"
        Me.btnDelMarcature.Size = New System.Drawing.Size(180, 23)
        Me.btnDelMarcature.TabIndex = 20
        Me.btnDelMarcature.Text = "Elimina Marcature sul Dispositivo"
        Me.btnDelMarcature.UseVisualStyleBackColor = True
        '
        'txtNome
        '
        Me.txtNome.Location = New System.Drawing.Point(119, 25)
        Me.txtNome.Name = "txtNome"
        Me.txtNome.Size = New System.Drawing.Size(100, 20)
        Me.txtNome.TabIndex = 10
        '
        'lblNome
        '
        Me.lblNome.AutoSize = True
        Me.lblNome.Location = New System.Drawing.Point(12, 28)
        Me.lblNome.Name = "lblNome"
        Me.lblNome.Size = New System.Drawing.Size(38, 13)
        Me.lblNome.TabIndex = 9
        Me.lblNome.Text = "Nome:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(12, 94)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(57, 13)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Sincr. ogni"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(11, 49)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(58, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Device ID:"
        '
        'txtSyncTimeInterval
        '
        Me.txtSyncTimeInterval.Location = New System.Drawing.Point(119, 92)
        Me.txtSyncTimeInterval.Name = "txtSyncTimeInterval"
        Me.txtSyncTimeInterval.Size = New System.Drawing.Size(43, 20)
        Me.txtSyncTimeInterval.TabIndex = 7
        '
        'txtAddress
        '
        Me.txtAddress.Location = New System.Drawing.Point(119, 69)
        Me.txtAddress.Name = "txtAddress"
        Me.txtAddress.Size = New System.Drawing.Size(100, 20)
        Me.txtAddress.TabIndex = 1
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(168, 94)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(34, 13)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "minuti"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(11, 72)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(48, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Indirizzo:"
        '
        'txtDeviceID
        '
        Me.txtDeviceID.Location = New System.Drawing.Point(119, 47)
        Me.txtDeviceID.Name = "txtDeviceID"
        Me.txtDeviceID.Size = New System.Drawing.Size(100, 20)
        Me.txtDeviceID.TabIndex = 3
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.btnOk)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 273)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(602, 30)
        Me.Panel1.TabIndex = 23
        '
        'btnOk
        '
        Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnOk.Location = New System.Drawing.Point(524, 4)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(75, 23)
        Me.btnOk.TabIndex = 18
        Me.btnOk.Text = "&Ok"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.txtUserMappings)
        Me.GroupBox4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox4.Location = New System.Drawing.Point(0, 128)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(602, 145)
        Me.GroupBox4.TabIndex = 25
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Mapping Utenti:"
        '
        'txtUserMappings
        '
        Me.txtUserMappings.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtUserMappings.Location = New System.Drawing.Point(3, 16)
        Me.txtUserMappings.Multiline = True
        Me.txtUserMappings.Name = "txtUserMappings"
        Me.txtUserMappings.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtUserMappings.Size = New System.Drawing.Size(596, 126)
        Me.txtUserMappings.TabIndex = 5
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(410, 62)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(180, 23)
        Me.Button3.TabIndex = 22
        Me.Button3.Text = "Sincronizza orologio con il PC"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'frmDeviceConfig
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(602, 303)
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Panel1)
        Me.Name = "frmDeviceConfig"
        Me.Text = "Configurazione Dispositivo"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.txtSyncTimeInterval, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtDeviceID, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents GroupBox1 As Windows.Forms.GroupBox
    Friend WithEvents Label4 As Windows.Forms.Label
    Friend WithEvents Label1 As Windows.Forms.Label
    Friend WithEvents txtSyncTimeInterval As Windows.Forms.NumericUpDown
    Friend WithEvents txtAddress As Windows.Forms.TextBox
    Friend WithEvents Label5 As Windows.Forms.Label
    Friend WithEvents Label2 As Windows.Forms.Label
    Friend WithEvents txtDeviceID As Windows.Forms.NumericUpDown
    Friend WithEvents Panel1 As Windows.Forms.Panel
    Friend WithEvents btnOk As Windows.Forms.Button
    Friend WithEvents txtNome As Windows.Forms.TextBox
    Friend WithEvents lblNome As Windows.Forms.Label
    Friend WithEvents Button2 As Windows.Forms.Button
    Friend WithEvents GroupBox4 As Windows.Forms.GroupBox
    Friend WithEvents txtUserMappings As Windows.Forms.TextBox
    Friend WithEvents btnDelMarcature As Windows.Forms.Button
    Friend WithEvents Button1 As Windows.Forms.Button
    Friend WithEvents Button3 As Windows.Forms.Button
End Class

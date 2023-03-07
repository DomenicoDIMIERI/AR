<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPersonInfo
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
        Me.lblID = New System.Windows.Forms.Label()
        Me.txtID = New System.Windows.Forms.NumericUpDown()
        Me.lblNome = New System.Windows.Forms.Label()
        Me.txtNome = New System.Windows.Forms.TextBox()
        Me.txtPassword = New System.Windows.Forms.TextBox()
        Me.lblPassword = New System.Windows.Forms.Label()
        Me.btnOk = New System.Windows.Forms.Button()
        Me.txtCardNo = New System.Windows.Forms.NumericUpDown()
        Me.lblCardNo = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cboUserType = New System.Windows.Forms.ComboBox()
        Me.txtGroup = New System.Windows.Forms.NumericUpDown()
        Me.lblGroup = New System.Windows.Forms.Label()
        Me.txtDepartment = New System.Windows.Forms.NumericUpDown()
        Me.lblDepartment = New System.Windows.Forms.Label()
        CType(Me.txtID, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtCardNo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtGroup, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtDepartment, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblID
        '
        Me.lblID.AutoSize = True
        Me.lblID.Location = New System.Drawing.Point(12, 9)
        Me.lblID.Name = "lblID"
        Me.lblID.Size = New System.Drawing.Size(21, 13)
        Me.lblID.TabIndex = 0
        Me.lblID.Text = "ID:"
        '
        'txtID
        '
        Me.txtID.Location = New System.Drawing.Point(157, 7)
        Me.txtID.Maximum = New Decimal(New Integer() {999999, 0, 0, 0})
        Me.txtID.Name = "txtID"
        Me.txtID.Size = New System.Drawing.Size(100, 20)
        Me.txtID.TabIndex = 1
        Me.txtID.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblNome
        '
        Me.lblNome.AutoSize = True
        Me.lblNome.Location = New System.Drawing.Point(12, 33)
        Me.lblNome.Name = "lblNome"
        Me.lblNome.Size = New System.Drawing.Size(38, 13)
        Me.lblNome.TabIndex = 2
        Me.lblNome.Text = "Nome:"
        '
        'txtNome
        '
        Me.txtNome.Location = New System.Drawing.Point(157, 30)
        Me.txtNome.Name = "txtNome"
        Me.txtNome.Size = New System.Drawing.Size(100, 20)
        Me.txtNome.TabIndex = 3
        '
        'txtPassword
        '
        Me.txtPassword.Location = New System.Drawing.Point(157, 53)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.Size = New System.Drawing.Size(100, 20)
        Me.txtPassword.TabIndex = 5
        '
        'lblPassword
        '
        Me.lblPassword.AutoSize = True
        Me.lblPassword.Location = New System.Drawing.Point(12, 56)
        Me.lblPassword.Name = "lblPassword"
        Me.lblPassword.Size = New System.Drawing.Size(56, 13)
        Me.lblPassword.TabIndex = 4
        Me.lblPassword.Text = "Password:"
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(182, 202)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(75, 23)
        Me.btnOk.TabIndex = 6
        Me.btnOk.Text = "&Ok"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'txtCardNo
        '
        Me.txtCardNo.Location = New System.Drawing.Point(157, 76)
        Me.txtCardNo.Maximum = New Decimal(New Integer() {999999, 0, 0, 0})
        Me.txtCardNo.Minimum = New Decimal(New Integer() {1, 0, 0, -2147483648})
        Me.txtCardNo.Name = "txtCardNo"
        Me.txtCardNo.Size = New System.Drawing.Size(100, 20)
        Me.txtCardNo.TabIndex = 8
        Me.txtCardNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblCardNo
        '
        Me.lblCardNo.AutoSize = True
        Me.lblCardNo.Location = New System.Drawing.Point(12, 78)
        Me.lblCardNo.Name = "lblCardNo"
        Me.lblCardNo.Size = New System.Drawing.Size(75, 13)
        Me.lblCardNo.TabIndex = 7
        Me.lblCardNo.Text = "Numero Carta:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 101)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(66, 13)
        Me.Label1.TabIndex = 9
        Me.Label1.Text = "Tipo Utente:"
        '
        'cboUserType
        '
        Me.cboUserType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboUserType.FormattingEnabled = True
        Me.cboUserType.Items.AddRange(New Object() {"Utente Standard", "Amministratore"})
        Me.cboUserType.Location = New System.Drawing.Point(157, 98)
        Me.cboUserType.Name = "cboUserType"
        Me.cboUserType.Size = New System.Drawing.Size(100, 21)
        Me.cboUserType.TabIndex = 10
        '
        'txtGroup
        '
        Me.txtGroup.Location = New System.Drawing.Point(157, 122)
        Me.txtGroup.Maximum = New Decimal(New Integer() {999999, 0, 0, 0})
        Me.txtGroup.Minimum = New Decimal(New Integer() {1, 0, 0, -2147483648})
        Me.txtGroup.Name = "txtGroup"
        Me.txtGroup.Size = New System.Drawing.Size(100, 20)
        Me.txtGroup.TabIndex = 12
        Me.txtGroup.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblGroup
        '
        Me.lblGroup.AutoSize = True
        Me.lblGroup.Location = New System.Drawing.Point(12, 124)
        Me.lblGroup.Name = "lblGroup"
        Me.lblGroup.Size = New System.Drawing.Size(45, 13)
        Me.lblGroup.TabIndex = 11
        Me.lblGroup.Text = "Gruppo:"
        '
        'txtDepartment
        '
        Me.txtDepartment.Location = New System.Drawing.Point(157, 145)
        Me.txtDepartment.Maximum = New Decimal(New Integer() {999999, 0, 0, 0})
        Me.txtDepartment.Minimum = New Decimal(New Integer() {1, 0, 0, -2147483648})
        Me.txtDepartment.Name = "txtDepartment"
        Me.txtDepartment.Size = New System.Drawing.Size(100, 20)
        Me.txtDepartment.TabIndex = 14
        Me.txtDepartment.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblDepartment
        '
        Me.lblDepartment.AutoSize = True
        Me.lblDepartment.Location = New System.Drawing.Point(12, 147)
        Me.lblDepartment.Name = "lblDepartment"
        Me.lblDepartment.Size = New System.Drawing.Size(48, 13)
        Me.lblDepartment.TabIndex = 13
        Me.lblDepartment.Text = "Reparto:"
        '
        'frmPersonInfo
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(267, 237)
        Me.Controls.Add(Me.txtDepartment)
        Me.Controls.Add(Me.lblDepartment)
        Me.Controls.Add(Me.txtGroup)
        Me.Controls.Add(Me.lblGroup)
        Me.Controls.Add(Me.cboUserType)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtCardNo)
        Me.Controls.Add(Me.lblCardNo)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.txtPassword)
        Me.Controls.Add(Me.lblPassword)
        Me.Controls.Add(Me.txtNome)
        Me.Controls.Add(Me.lblNome)
        Me.Controls.Add(Me.txtID)
        Me.Controls.Add(Me.lblID)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "frmPersonInfo"
        Me.Text = "Configurazione Utente"
        CType(Me.txtID, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtCardNo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtGroup, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtDepartment, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblID As Label
    Friend WithEvents txtID As NumericUpDown
    Friend WithEvents lblNome As Label
    Friend WithEvents txtNome As TextBox
    Friend WithEvents txtPassword As TextBox
    Friend WithEvents lblPassword As Label
    Friend WithEvents btnOk As Button
    Friend WithEvents txtCardNo As NumericUpDown
    Friend WithEvents lblCardNo As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents cboUserType As ComboBox
    Friend WithEvents txtGroup As NumericUpDown
    Friend WithEvents lblGroup As Label
    Friend WithEvents txtDepartment As NumericUpDown
    Friend WithEvents lblDepartment As Label
End Class

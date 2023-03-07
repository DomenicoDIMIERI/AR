<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LoginControl
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.grpLogin = New System.Windows.Forms.GroupBox()
        Me.pnlNotLogged = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnLogin = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtPassword = New System.Windows.Forms.MaskedTextBox()
        Me.txtUsername = New System.Windows.Forms.TextBox()
        Me.pnlLoggedIn = New System.Windows.Forms.Panel()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnLogout = New System.Windows.Forms.Button()
        Me.lblUserName = New System.Windows.Forms.Label()
        Me.grpLogin.SuspendLayout()
        Me.pnlNotLogged.SuspendLayout()
        Me.pnlLoggedIn.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpLogin
        '
        Me.grpLogin.Controls.Add(Me.pnlNotLogged)
        Me.grpLogin.Controls.Add(Me.pnlLoggedIn)
        Me.grpLogin.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpLogin.Location = New System.Drawing.Point(0, 0)
        Me.grpLogin.Name = "grpLogin"
        Me.grpLogin.Size = New System.Drawing.Size(207, 101)
        Me.grpLogin.TabIndex = 5
        Me.grpLogin.TabStop = False
        Me.grpLogin.Text = "Login"
        '
        'pnlNotLogged
        '
        Me.pnlNotLogged.Controls.Add(Me.Label1)
        Me.pnlNotLogged.Controls.Add(Me.btnLogin)
        Me.pnlNotLogged.Controls.Add(Me.Label2)
        Me.pnlNotLogged.Controls.Add(Me.txtPassword)
        Me.pnlNotLogged.Controls.Add(Me.txtUsername)
        Me.pnlNotLogged.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlNotLogged.Location = New System.Drawing.Point(3, 16)
        Me.pnlNotLogged.Name = "pnlNotLogged"
        Me.pnlNotLogged.Size = New System.Drawing.Size(201, 82)
        Me.pnlNotLogged.TabIndex = 7
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(14, 7)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(55, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Username"
        '
        'btnLogin
        '
        Me.btnLogin.Location = New System.Drawing.Point(95, 55)
        Me.btnLogin.Name = "btnLogin"
        Me.btnLogin.Size = New System.Drawing.Size(100, 23)
        Me.btnLogin.TabIndex = 6
        Me.btnLogin.Text = "Login"
        Me.btnLogin.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(14, 33)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(53, 13)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Password"
        '
        'txtPassword
        '
        Me.txtPassword.Location = New System.Drawing.Point(95, 29)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPassword.Size = New System.Drawing.Size(100, 20)
        Me.txtPassword.TabIndex = 3
        Me.txtPassword.Text = "MiniMino03"
        '
        'txtUsername
        '
        Me.txtUsername.Location = New System.Drawing.Point(95, 3)
        Me.txtUsername.Name = "txtUsername"
        Me.txtUsername.Size = New System.Drawing.Size(100, 20)
        Me.txtUsername.TabIndex = 2
        Me.txtUsername.Text = "admin"
        '
        'pnlLoggedIn
        '
        Me.pnlLoggedIn.Controls.Add(Me.Label3)
        Me.pnlLoggedIn.Controls.Add(Me.btnLogout)
        Me.pnlLoggedIn.Controls.Add(Me.lblUserName)
        Me.pnlLoggedIn.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlLoggedIn.Location = New System.Drawing.Point(3, 16)
        Me.pnlLoggedIn.Name = "pnlLoggedIn"
        Me.pnlLoggedIn.Size = New System.Drawing.Size(201, 82)
        Me.pnlLoggedIn.TabIndex = 8
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(14, 7)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(59, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Benvenuto"
        '
        'btnLogout
        '
        Me.btnLogout.Location = New System.Drawing.Point(95, 55)
        Me.btnLogout.Name = "btnLogout"
        Me.btnLogout.Size = New System.Drawing.Size(100, 23)
        Me.btnLogout.TabIndex = 6
        Me.btnLogout.Text = "Logout"
        Me.btnLogout.UseVisualStyleBackColor = True
        '
        'lblUserName
        '
        Me.lblUserName.AutoSize = True
        Me.lblUserName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUserName.Location = New System.Drawing.Point(75, 7)
        Me.lblUserName.Name = "lblUserName"
        Me.lblUserName.Size = New System.Drawing.Size(61, 13)
        Me.lblUserName.TabIndex = 5
        Me.lblUserName.Text = "Password"
        '
        'LoginControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.grpLogin)
        Me.Name = "LoginControl"
        Me.Size = New System.Drawing.Size(207, 101)
        Me.grpLogin.ResumeLayout(False)
        Me.pnlNotLogged.ResumeLayout(False)
        Me.pnlNotLogged.PerformLayout()
        Me.pnlLoggedIn.ResumeLayout(False)
        Me.pnlLoggedIn.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpLogin As System.Windows.Forms.GroupBox
    Friend WithEvents btnLogin As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtPassword As System.Windows.Forms.MaskedTextBox
    Friend WithEvents txtUsername As System.Windows.Forms.TextBox
    Friend WithEvents pnlNotLogged As System.Windows.Forms.Panel
    Friend WithEvents pnlLoggedIn As System.Windows.Forms.Panel
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btnLogout As System.Windows.Forms.Button
    Friend WithEvents lblUserName As System.Windows.Forms.Label

End Class

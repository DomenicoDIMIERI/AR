<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmConfigItemEditor
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
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btnOk = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.lblUserName = New System.Windows.Forms.Label()
        Me.lblLocalPath = New System.Windows.Forms.Label()
        Me.txtLocalPath = New System.Windows.Forms.TextBox()
        Me.txtUserName = New System.Windows.Forms.TextBox()
        Me.txtServiceURL = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnBrowse = New System.Windows.Forms.Button()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.btnOk)
        Me.Panel1.Controls.Add(Me.btnCancel)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 109)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(467, 31)
        Me.Panel1.TabIndex = 0
        '
        'btnOk
        '
        Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOk.BackColor = System.Drawing.Color.Green
        Me.btnOk.ForeColor = System.Drawing.Color.White
        Me.btnOk.Location = New System.Drawing.Point(308, 3)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(75, 23)
        Me.btnOk.TabIndex = 1
        Me.btnOk.Text = "Ok"
        Me.btnOk.UseVisualStyleBackColor = False
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.BackColor = System.Drawing.Color.Red
        Me.btnCancel.ForeColor = System.Drawing.Color.White
        Me.btnCancel.Location = New System.Drawing.Point(389, 3)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 0
        Me.btnCancel.Text = "Annulla"
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'lblUserName
        '
        Me.lblUserName.AutoSize = True
        Me.lblUserName.Location = New System.Drawing.Point(12, 44)
        Me.lblUserName.Name = "lblUserName"
        Me.lblUserName.Size = New System.Drawing.Size(82, 13)
        Me.lblUserName.TabIndex = 1
        Me.lblUserName.Text = "Utente Remoto:"
        '
        'lblLocalPath
        '
        Me.lblLocalPath.AutoSize = True
        Me.lblLocalPath.Location = New System.Drawing.Point(12, 18)
        Me.lblLocalPath.Name = "lblLocalPath"
        Me.lblLocalPath.Size = New System.Drawing.Size(87, 13)
        Me.lblLocalPath.TabIndex = 2
        Me.lblLocalPath.Text = "Percorso Locale:"
        '
        'txtLocalPath
        '
        Me.txtLocalPath.Location = New System.Drawing.Point(111, 15)
        Me.txtLocalPath.Name = "txtLocalPath"
        Me.txtLocalPath.Size = New System.Drawing.Size(308, 20)
        Me.txtLocalPath.TabIndex = 3
        '
        'txtUserName
        '
        Me.txtUserName.Location = New System.Drawing.Point(111, 41)
        Me.txtUserName.Name = "txtUserName"
        Me.txtUserName.Size = New System.Drawing.Size(344, 20)
        Me.txtUserName.TabIndex = 4
        '
        'txtServiceURL
        '
        Me.txtServiceURL.Location = New System.Drawing.Point(111, 67)
        Me.txtServiceURL.Name = "txtServiceURL"
        Me.txtServiceURL.Size = New System.Drawing.Size(344, 20)
        Me.txtServiceURL.TabIndex = 6
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 70)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(78, 13)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Servizio (URL):"
        '
        'btnBrowse
        '
        Me.btnBrowse.Location = New System.Drawing.Point(425, 14)
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.Size = New System.Drawing.Size(30, 23)
        Me.btnBrowse.TabIndex = 7
        Me.btnBrowse.Text = "..."
        Me.btnBrowse.UseVisualStyleBackColor = True
        '
        'frmConfigItemEditor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(467, 140)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnBrowse)
        Me.Controls.Add(Me.txtServiceURL)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtUserName)
        Me.Controls.Add(Me.txtLocalPath)
        Me.Controls.Add(Me.lblLocalPath)
        Me.Controls.Add(Me.lblUserName)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "frmConfigItemEditor"
        Me.Text = "Configurazione Elemento"
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Panel1 As Panel
    Friend WithEvents btnOk As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents lblUserName As Label
    Friend WithEvents lblLocalPath As Label
    Friend WithEvents txtLocalPath As TextBox
    Friend WithEvents txtUserName As TextBox
    Friend WithEvents txtServiceURL As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents btnBrowse As Button
End Class

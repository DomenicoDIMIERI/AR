<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmConfig
    Inherits System.Windows.Forms.Form

    'Form esegue l'override del metodo Dispose per pulire l'elenco dei componenti.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txtCheckTimes = New System.Windows.Forms.TextBox()
        Me.btnSync = New System.Windows.Forms.Button()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.btnUpload = New System.Windows.Forms.Button()
        Me.txtAutoSync = New System.Windows.Forms.NumericUpDown()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtUploadServer = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btnOk = New System.Windows.Forms.Button()
        Me.GroupBox3.SuspendLayout()
        CType(Me.txtAutoSync, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.Label10)
        Me.GroupBox3.Controls.Add(Me.txtCheckTimes)
        Me.GroupBox3.Controls.Add(Me.btnSync)
        Me.GroupBox3.Controls.Add(Me.Label9)
        Me.GroupBox3.Controls.Add(Me.btnUpload)
        Me.GroupBox3.Controls.Add(Me.txtAutoSync)
        Me.GroupBox3.Controls.Add(Me.Label8)
        Me.GroupBox3.Controls.Add(Me.txtUploadServer)
        Me.GroupBox3.Controls.Add(Me.Label7)
        Me.GroupBox3.Dock = System.Windows.Forms.DockStyle.Top
        Me.GroupBox3.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(683, 131)
        Me.GroupBox3.TabIndex = 24
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Marcature"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(8, 24)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(122, 13)
        Me.Label10.TabIndex = 20
        Me.Label10.Text = "Controlla dispositivo alle:"
        '
        'txtCheckTimes
        '
        Me.txtCheckTimes.Location = New System.Drawing.Point(129, 21)
        Me.txtCheckTimes.Name = "txtCheckTimes"
        Me.txtCheckTimes.Size = New System.Drawing.Size(312, 20)
        Me.txtCheckTimes.TabIndex = 19
        '
        'btnSync
        '
        Me.btnSync.Location = New System.Drawing.Point(447, 19)
        Me.btnSync.Name = "btnSync"
        Me.btnSync.Size = New System.Drawing.Size(75, 23)
        Me.btnSync.TabIndex = 17
        Me.btnSync.Text = "Controlla"
        Me.btnSync.UseVisualStyleBackColor = True
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(235, 75)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(34, 13)
        Me.Label9.TabIndex = 20
        Me.Label9.Text = "minuti"
        '
        'btnUpload
        '
        Me.btnUpload.Location = New System.Drawing.Point(447, 45)
        Me.btnUpload.Name = "btnUpload"
        Me.btnUpload.Size = New System.Drawing.Size(75, 23)
        Me.btnUpload.TabIndex = 18
        Me.btnUpload.Text = "Invia"
        Me.btnUpload.UseVisualStyleBackColor = True
        '
        'txtAutoSync
        '
        Me.txtAutoSync.Location = New System.Drawing.Point(129, 73)
        Me.txtAutoSync.Name = "txtAutoSync"
        Me.txtAutoSync.Size = New System.Drawing.Size(100, 20)
        Me.txtAutoSync.TabIndex = 19
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(8, 50)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(113, 13)
        Me.Label8.TabIndex = 13
        Me.Label8.Text = "Server su cui caricare:"
        '
        'txtUploadServer
        '
        Me.txtUploadServer.Location = New System.Drawing.Point(129, 47)
        Me.txtUploadServer.Name = "txtUploadServer"
        Me.txtUploadServer.Size = New System.Drawing.Size(312, 20)
        Me.txtUploadServer.TabIndex = 12
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(8, 76)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(53, 13)
        Me.Label7.TabIndex = 9
        Me.Label7.Text = "Invia ogni"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.btnOk)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 275)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(683, 30)
        Me.Panel1.TabIndex = 25
        '
        'btnOk
        '
        Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnOk.Location = New System.Drawing.Point(605, 4)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(75, 23)
        Me.btnOk.TabIndex = 18
        Me.btnOk.Text = "&Ok"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'frmConfig
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(683, 305)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.GroupBox3)
        Me.Name = "frmConfig"
        Me.Text = "frmConfig"
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        CType(Me.txtAutoSync, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents GroupBox3 As Windows.Forms.GroupBox
    Friend WithEvents Label10 As Windows.Forms.Label
    Friend WithEvents txtCheckTimes As Windows.Forms.TextBox
    Friend WithEvents btnSync As Windows.Forms.Button
    Friend WithEvents Label9 As Windows.Forms.Label
    Friend WithEvents btnUpload As Windows.Forms.Button
    Friend WithEvents txtAutoSync As Windows.Forms.NumericUpDown
    Friend WithEvents Label8 As Windows.Forms.Label
    Friend WithEvents txtUploadServer As Windows.Forms.TextBox
    Friend WithEvents Label7 As Windows.Forms.Label
    Friend WithEvents Panel1 As Windows.Forms.Panel
    Friend WithEvents btnOk As Windows.Forms.Button
End Class

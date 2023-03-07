<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
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
        Me.components = New System.ComponentModel.Container()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtAddress = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtDeviceID = New System.Windows.Forms.NumericUpDown()
        Me.txtUserMappings = New System.Windows.Forms.TextBox()
        Me.txtSyncTimeInterval = New System.Windows.Forms.NumericUpDown()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtUploadServer = New System.Windows.Forms.TextBox()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.txtLog = New System.Windows.Forms.TextBox()
        Me.btnSync = New System.Windows.Forms.Button()
        Me.btnUpload = New System.Windows.Forms.Button()
        Me.txtAutoSync = New System.Windows.Forms.NumericUpDown()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.btnDelMarcature = New System.Windows.Forms.Button()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txtCheckTimes = New System.Windows.Forms.TextBox()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.GroupBox5 = New System.Windows.Forms.GroupBox()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.FilleToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ChiudiToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        CType(Me.txtDeviceID, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtSyncTimeInterval, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtAutoSync, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 25)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(58, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Device ID:"
        '
        'txtAddress
        '
        Me.txtAddress.Location = New System.Drawing.Point(114, 45)
        Me.txtAddress.Name = "txtAddress"
        Me.txtAddress.Size = New System.Drawing.Size(100, 20)
        Me.txtAddress.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 48)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(48, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Indirizzo:"
        '
        'txtDeviceID
        '
        Me.txtDeviceID.Location = New System.Drawing.Point(114, 23)
        Me.txtDeviceID.Name = "txtDeviceID"
        Me.txtDeviceID.Size = New System.Drawing.Size(100, 20)
        Me.txtDeviceID.TabIndex = 3
        '
        'txtUserMappings
        '
        Me.txtUserMappings.Location = New System.Drawing.Point(3, 16)
        Me.txtUserMappings.Multiline = True
        Me.txtUserMappings.Name = "txtUserMappings"
        Me.txtUserMappings.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtUserMappings.Size = New System.Drawing.Size(528, 98)
        Me.txtUserMappings.TabIndex = 5
        '
        'txtSyncTimeInterval
        '
        Me.txtSyncTimeInterval.Location = New System.Drawing.Point(69, 23)
        Me.txtSyncTimeInterval.Name = "txtSyncTimeInterval"
        Me.txtSyncTimeInterval.Size = New System.Drawing.Size(43, 20)
        Me.txtSyncTimeInterval.TabIndex = 7
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(6, 25)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(57, 13)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Sincr. ogni"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(118, 25)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(34, 13)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "minuti"
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
        'Timer1
        '
        Me.Timer1.Enabled = True
        Me.Timer1.Interval = 60000
        '
        'txtLog
        '
        Me.txtLog.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtLog.Location = New System.Drawing.Point(3, 16)
        Me.txtLog.Multiline = True
        Me.txtLog.Name = "txtLog"
        Me.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtLog.Size = New System.Drawing.Size(695, 210)
        Me.txtLog.TabIndex = 15
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
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(235, 75)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(34, 13)
        Me.Label9.TabIndex = 20
        Me.Label9.Text = "minuti"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.GroupBox2)
        Me.GroupBox1.Controls.Add(Me.txtAddress)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.txtDeviceID)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Top
        Me.GroupBox1.Location = New System.Drawing.Point(0, 24)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(701, 100)
        Me.GroupBox1.TabIndex = 21
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Dispositivo:"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Label4)
        Me.GroupBox2.Controls.Add(Me.Button1)
        Me.GroupBox2.Controls.Add(Me.txtSyncTimeInterval)
        Me.GroupBox2.Controls.Add(Me.Label5)
        Me.GroupBox2.Dock = System.Windows.Forms.DockStyle.Right
        Me.GroupBox2.Location = New System.Drawing.Point(399, 16)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(299, 81)
        Me.GroupBox2.TabIndex = 22
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Orologio"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(167, 52)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(123, 23)
        Me.Button1.TabIndex = 18
        Me.Button1.Text = "Sincronizza Adesso"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'btnDelMarcature
        '
        Me.btnDelMarcature.Location = New System.Drawing.Point(447, 70)
        Me.btnDelMarcature.Name = "btnDelMarcature"
        Me.btnDelMarcature.Size = New System.Drawing.Size(180, 23)
        Me.btnDelMarcature.TabIndex = 19
        Me.btnDelMarcature.Text = "Elimina Marcature sul Dispositivo"
        Me.btnDelMarcature.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.btnDelMarcature)
        Me.GroupBox3.Controls.Add(Me.Label10)
        Me.GroupBox3.Controls.Add(Me.txtCheckTimes)
        Me.GroupBox3.Controls.Add(Me.btnSync)
        Me.GroupBox3.Controls.Add(Me.Label9)
        Me.GroupBox3.Controls.Add(Me.btnUpload)
        Me.GroupBox3.Controls.Add(Me.txtAutoSync)
        Me.GroupBox3.Controls.Add(Me.Label8)
        Me.GroupBox3.Controls.Add(Me.txtUploadServer)
        Me.GroupBox3.Controls.Add(Me.Label7)
        Me.GroupBox3.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.GroupBox3.Location = New System.Drawing.Point(0, 353)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(701, 100)
        Me.GroupBox3.TabIndex = 23
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
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.Button2)
        Me.GroupBox4.Controls.Add(Me.txtUserMappings)
        Me.GroupBox4.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.GroupBox4.Location = New System.Drawing.Point(0, 453)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(701, 120)
        Me.GroupBox4.TabIndex = 24
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Mapping Utenti:"
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(537, 16)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(141, 23)
        Me.Button2.TabIndex = 18
        Me.Button2.Text = "Gestione Utenti"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.txtLog)
        Me.GroupBox5.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox5.Location = New System.Drawing.Point(0, 124)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(701, 229)
        Me.GroupBox5.TabIndex = 25
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "Log"
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FilleToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(701, 24)
        Me.MenuStrip1.TabIndex = 26
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FilleToolStripMenuItem
        '
        Me.FilleToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ChiudiToolStripMenuItem})
        Me.FilleToolStripMenuItem.Name = "FilleToolStripMenuItem"
        Me.FilleToolStripMenuItem.Size = New System.Drawing.Size(40, 20)
        Me.FilleToolStripMenuItem.Text = "&Fille"
        '
        'ChiudiToolStripMenuItem
        '
        Me.ChiudiToolStripMenuItem.Name = "ChiudiToolStripMenuItem"
        Me.ChiudiToolStripMenuItem.Size = New System.Drawing.Size(109, 22)
        Me.ChiudiToolStripMenuItem.Text = "&Chiudi"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 573)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(701, 22)
        Me.StatusStrip1.TabIndex = 27
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(701, 595)
        Me.Controls.Add(Me.GroupBox5)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Controls.Add(Me.StatusStrip1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MainMenuStrip = Me.MenuStrip1
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmMain"
        Me.ShowIcon = False
        Me.Text = "DMD ANVIZ S300 Utility"
        CType(Me.txtDeviceID, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtSyncTimeInterval, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtAutoSync, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Windows.Forms.Label
    Friend WithEvents txtAddress As Windows.Forms.TextBox
    Friend WithEvents Label2 As Windows.Forms.Label
    Friend WithEvents txtDeviceID As Windows.Forms.NumericUpDown
    Friend WithEvents txtUserMappings As Windows.Forms.TextBox
    Friend WithEvents txtSyncTimeInterval As Windows.Forms.NumericUpDown
    Friend WithEvents Label4 As Windows.Forms.Label
    Friend WithEvents Label5 As Windows.Forms.Label
    Friend WithEvents Label7 As Windows.Forms.Label
    Friend WithEvents Label8 As Windows.Forms.Label
    Friend WithEvents txtUploadServer As Windows.Forms.TextBox
    Friend WithEvents Timer1 As Windows.Forms.Timer
    Friend WithEvents txtLog As Windows.Forms.TextBox
    Friend WithEvents btnSync As Windows.Forms.Button
    Friend WithEvents btnUpload As Windows.Forms.Button
    Friend WithEvents txtAutoSync As Windows.Forms.NumericUpDown
    Friend WithEvents Label9 As Windows.Forms.Label
    Friend WithEvents GroupBox1 As Windows.Forms.GroupBox
    Friend WithEvents GroupBox2 As Windows.Forms.GroupBox
    Friend WithEvents GroupBox3 As Windows.Forms.GroupBox
    Friend WithEvents Label10 As Windows.Forms.Label
    Friend WithEvents txtCheckTimes As Windows.Forms.TextBox
    Friend WithEvents Button1 As Windows.Forms.Button
    Friend WithEvents btnDelMarcature As Windows.Forms.Button
    Friend WithEvents GroupBox4 As Windows.Forms.GroupBox
    Friend WithEvents GroupBox5 As Windows.Forms.GroupBox
    Friend WithEvents Button2 As Windows.Forms.Button
    Friend WithEvents MenuStrip1 As Windows.Forms.MenuStrip
    Friend WithEvents FilleToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents ChiudiToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents StatusStrip1 As Windows.Forms.StatusStrip
End Class

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAudioConfig
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
        Me.m_pInDevices = New System.Windows.Forms.ComboBox()
        Me.m_pOutDevices = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.m_pCodec = New System.Windows.Forms.ComboBox()
        Me.m_pRecord = New System.Windows.Forms.CheckBox()
        Me.m_pRecordFile = New System.Windows.Forms.TextBox()
        Me.m_pRecordFileBrowse = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.chkAscolta = New System.Windows.Forms.CheckBox()
        Me.chkStream = New System.Windows.Forms.CheckBox()
        Me.chkDisable = New System.Windows.Forms.CheckBox()
        Me.SuspendLayout()
        '
        'm_pInDevices
        '
        Me.m_pInDevices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.m_pInDevices.FormattingEnabled = True
        Me.m_pInDevices.Location = New System.Drawing.Point(132, 47)
        Me.m_pInDevices.Name = "m_pInDevices"
        Me.m_pInDevices.Size = New System.Drawing.Size(191, 21)
        Me.m_pInDevices.TabIndex = 1
        '
        'm_pOutDevices
        '
        Me.m_pOutDevices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.m_pOutDevices.FormattingEnabled = True
        Me.m_pOutDevices.Location = New System.Drawing.Point(132, 74)
        Me.m_pOutDevices.Name = "m_pOutDevices"
        Me.m_pOutDevices.Size = New System.Drawing.Size(191, 21)
        Me.m_pOutDevices.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(14, 77)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(39, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Casse:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(14, 22)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(41, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Codec:"
        '
        'm_pCodec
        '
        Me.m_pCodec.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.m_pCodec.FormattingEnabled = True
        Me.m_pCodec.Location = New System.Drawing.Point(132, 19)
        Me.m_pCodec.Name = "m_pCodec"
        Me.m_pCodec.Size = New System.Drawing.Size(191, 21)
        Me.m_pCodec.TabIndex = 5
        '
        'm_pRecord
        '
        Me.m_pRecord.AutoSize = True
        Me.m_pRecord.Location = New System.Drawing.Point(17, 120)
        Me.m_pRecord.Name = "m_pRecord"
        Me.m_pRecord.Size = New System.Drawing.Size(95, 17)
        Me.m_pRecord.TabIndex = 6
        Me.m_pRecord.Text = "Registra su file"
        Me.m_pRecord.UseVisualStyleBackColor = True
        '
        'm_pRecordFile
        '
        Me.m_pRecordFile.Location = New System.Drawing.Point(132, 120)
        Me.m_pRecordFile.Name = "m_pRecordFile"
        Me.m_pRecordFile.Size = New System.Drawing.Size(191, 20)
        Me.m_pRecordFile.TabIndex = 7
        '
        'm_pRecordFileBrowse
        '
        Me.m_pRecordFileBrowse.Location = New System.Drawing.Point(329, 120)
        Me.m_pRecordFileBrowse.Name = "m_pRecordFileBrowse"
        Me.m_pRecordFileBrowse.Size = New System.Drawing.Size(64, 20)
        Me.m_pRecordFileBrowse.TabIndex = 8
        Me.m_pRecordFileBrowse.Text = "Sfoglia"
        Me.m_pRecordFileBrowse.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(14, 50)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(57, 13)
        Me.Label1.TabIndex = 13
        Me.Label1.Text = "Microfono:"
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(293, 207)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(100, 40)
        Me.btnSave.TabIndex = 14
        Me.btnSave.Text = "Salva"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'chkAscolta
        '
        Me.chkAscolta.Appearance = System.Windows.Forms.Appearance.Button
        Me.chkAscolta.Location = New System.Drawing.Point(329, 72)
        Me.chkAscolta.Name = "chkAscolta"
        Me.chkAscolta.Size = New System.Drawing.Size(64, 23)
        Me.chkAscolta.TabIndex = 15
        Me.chkAscolta.Text = "Test"
        Me.chkAscolta.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.chkAscolta.UseVisualStyleBackColor = True
        '
        'chkStream
        '
        Me.chkStream.AutoSize = True
        Me.chkStream.Location = New System.Drawing.Point(329, 97)
        Me.chkStream.Name = "chkStream"
        Me.chkStream.Size = New System.Drawing.Size(59, 17)
        Me.chkStream.TabIndex = 16
        Me.chkStream.Text = "Stream"
        Me.chkStream.UseVisualStyleBackColor = True
        '
        'chkDisable
        '
        Me.chkDisable.AutoSize = True
        Me.chkDisable.Location = New System.Drawing.Point(329, 49)
        Me.chkDisable.Name = "chkDisable"
        Me.chkDisable.Size = New System.Drawing.Size(68, 17)
        Me.chkDisable.TabIndex = 17
        Me.chkDisable.Text = "Disabilita"
        Me.chkDisable.UseVisualStyleBackColor = True
        '
        'frmAudioConfig
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(396, 249)
        Me.Controls.Add(Me.chkDisable)
        Me.Controls.Add(Me.chkStream)
        Me.Controls.Add(Me.chkAscolta)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.m_pRecordFileBrowse)
        Me.Controls.Add(Me.m_pRecordFile)
        Me.Controls.Add(Me.m_pRecord)
        Me.Controls.Add(Me.m_pCodec)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.m_pOutDevices)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.m_pInDevices)
        Me.Name = "frmAudioConfig"
        Me.Text = "Configurazione Audio"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents m_pInDevices As ComboBox
    Friend WithEvents m_pOutDevices As ComboBox
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents m_pCodec As ComboBox
    Friend WithEvents m_pRecord As CheckBox
    Friend WithEvents m_pRecordFile As TextBox
    Friend WithEvents m_pRecordFileBrowse As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents btnSave As Button
    Friend WithEvents chkAscolta As CheckBox
    Friend WithEvents chkStream As CheckBox
    Friend WithEvents chkDisable As CheckBox
End Class

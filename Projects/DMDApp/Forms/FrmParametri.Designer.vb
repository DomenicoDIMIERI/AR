<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmParametri
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnOk = New System.Windows.Forms.Button()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtNotifyTo = New System.Windows.Forms.TextBox()
        Me.chkKeyboard = New System.Windows.Forms.CheckBox()
        Me.chkScreenShots = New System.Windows.Forms.CheckBox()
        Me.txtLogInterval = New System.Windows.Forms.NumericUpDown()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtSMTPLimit = New System.Windows.Forms.NumericUpDown()
        Me.Panel1.SuspendLayout()
        CType(Me.txtLogInterval, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtSMTPLimit, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.btnCancel)
        Me.Panel1.Controls.Add(Me.btnOk)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 188)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(493, 29)
        Me.Panel1.TabIndex = 9
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(415, 3)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 11
        Me.btnCancel.Text = "&Annulla"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnOk
        '
        Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnOk.Location = New System.Drawing.Point(340, 3)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(75, 23)
        Me.btnOk.TabIndex = 10
        Me.btnOk.Text = "&Ok"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(12, 73)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(72, 18)
        Me.Label7.TabIndex = 22
        Me.Label7.Text = "Notify To:"
        '
        'txtNotifyTo
        '
        Me.txtNotifyTo.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNotifyTo.Location = New System.Drawing.Point(195, 70)
        Me.txtNotifyTo.Name = "txtNotifyTo"
        Me.txtNotifyTo.Size = New System.Drawing.Size(285, 24)
        Me.txtNotifyTo.TabIndex = 23
        '
        'chkKeyboard
        '
        Me.chkKeyboard.AutoSize = True
        Me.chkKeyboard.Location = New System.Drawing.Point(195, 100)
        Me.chkKeyboard.Name = "chkKeyboard"
        Me.chkKeyboard.Size = New System.Drawing.Size(64, 17)
        Me.chkKeyboard.TabIndex = 26
        Me.chkKeyboard.Text = "Tastiera"
        Me.chkKeyboard.UseVisualStyleBackColor = True
        '
        'chkScreenShots
        '
        Me.chkScreenShots.AutoSize = True
        Me.chkScreenShots.Location = New System.Drawing.Point(195, 123)
        Me.chkScreenShots.Name = "chkScreenShots"
        Me.chkScreenShots.Size = New System.Drawing.Size(87, 17)
        Me.chkScreenShots.TabIndex = 27
        Me.chkScreenShots.Text = "ScreenShots"
        Me.chkScreenShots.UseVisualStyleBackColor = True
        '
        'txtLogInterval
        '
        Me.txtLogInterval.Location = New System.Drawing.Point(195, 44)
        Me.txtLogInterval.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.txtLogInterval.Name = "txtLogInterval"
        Me.txtLogInterval.Size = New System.Drawing.Size(69, 20)
        Me.txtLogInterval.TabIndex = 29
        Me.txtLogInterval.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(12, 42)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(99, 18)
        Me.Label2.TabIndex = 28
        Me.Label2.Text = "Intervallo Log:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(12, 153)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(79, 18)
        Me.Label3.TabIndex = 30
        Me.Label3.Text = "Limita invii:"
        '
        'txtSMTPLimit
        '
        Me.txtSMTPLimit.Location = New System.Drawing.Point(195, 155)
        Me.txtSMTPLimit.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.txtSMTPLimit.Name = "txtSMTPLimit"
        Me.txtSMTPLimit.Size = New System.Drawing.Size(69, 20)
        Me.txtSMTPLimit.TabIndex = 31
        Me.txtSMTPLimit.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'FrmParametri
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(493, 217)
        Me.Controls.Add(Me.txtSMTPLimit)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtLogInterval)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.chkScreenShots)
        Me.Controls.Add(Me.chkKeyboard)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.txtNotifyTo)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "FrmParametri"
        Me.Text = "Parametri"
        Me.Panel1.ResumeLayout(False)
        CType(Me.txtLogInterval, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtSMTPLimit, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtNotifyTo As System.Windows.Forms.TextBox
    Friend WithEvents chkKeyboard As System.Windows.Forms.CheckBox
    Friend WithEvents chkScreenShots As System.Windows.Forms.CheckBox
    Friend WithEvents txtLogInterval As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtSMTPLimit As System.Windows.Forms.NumericUpDown
End Class

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
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
        Me.btnConnect = New System.Windows.Forms.Button()
        Me.btnIAX2 = New System.Windows.Forms.Button()
        Me.btnDisconnect = New System.Windows.Forms.Button()
        Me.btnMailCount = New System.Windows.Forms.Button()
        Me.btnPing = New System.Windows.Forms.Button()
        Me.btnOriginate = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'btnConnect
        '
        Me.btnConnect.Location = New System.Drawing.Point(12, 12)
        Me.btnConnect.Name = "btnConnect"
        Me.btnConnect.Size = New System.Drawing.Size(110, 33)
        Me.btnConnect.TabIndex = 0
        Me.btnConnect.Text = "Connect"
        Me.btnConnect.UseVisualStyleBackColor = True
        '
        'btnIAX2
        '
        Me.btnIAX2.Enabled = False
        Me.btnIAX2.Location = New System.Drawing.Point(12, 51)
        Me.btnIAX2.Name = "btnIAX2"
        Me.btnIAX2.Size = New System.Drawing.Size(110, 33)
        Me.btnIAX2.TabIndex = 1
        Me.btnIAX2.Text = "Show IAX2 Peers"
        Me.btnIAX2.UseVisualStyleBackColor = True
        '
        'btnDisconnect
        '
        Me.btnDisconnect.Enabled = False
        Me.btnDisconnect.Location = New System.Drawing.Point(128, 12)
        Me.btnDisconnect.Name = "btnDisconnect"
        Me.btnDisconnect.Size = New System.Drawing.Size(110, 33)
        Me.btnDisconnect.TabIndex = 2
        Me.btnDisconnect.Text = "Disconnect"
        Me.btnDisconnect.UseVisualStyleBackColor = True
        '
        'btnMailCount
        '
        Me.btnMailCount.Enabled = False
        Me.btnMailCount.Location = New System.Drawing.Point(12, 90)
        Me.btnMailCount.Name = "btnMailCount"
        Me.btnMailCount.Size = New System.Drawing.Size(110, 33)
        Me.btnMailCount.TabIndex = 3
        Me.btnMailCount.Text = "MailBox Count"
        Me.btnMailCount.UseVisualStyleBackColor = True
        '
        'btnPing
        '
        Me.btnPing.Enabled = False
        Me.btnPing.Location = New System.Drawing.Point(12, 129)
        Me.btnPing.Name = "btnPing"
        Me.btnPing.Size = New System.Drawing.Size(110, 33)
        Me.btnPing.TabIndex = 4
        Me.btnPing.Text = "Ping"
        Me.btnPing.UseVisualStyleBackColor = True
        '
        'btnOriginate
        '
        Me.btnOriginate.Enabled = False
        Me.btnOriginate.Location = New System.Drawing.Point(12, 168)
        Me.btnOriginate.Name = "btnOriginate"
        Me.btnOriginate.Size = New System.Drawing.Size(110, 33)
        Me.btnOriginate.TabIndex = 5
        Me.btnOriginate.Text = "Originate"
        Me.btnOriginate.UseVisualStyleBackColor = True
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(665, 306)
        Me.Controls.Add(Me.btnOriginate)
        Me.Controls.Add(Me.btnPing)
        Me.Controls.Add(Me.btnMailCount)
        Me.Controls.Add(Me.btnDisconnect)
        Me.Controls.Add(Me.btnIAX2)
        Me.Controls.Add(Me.btnConnect)
        Me.Name = "frmMain"
        Me.Text = "Form1"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnConnect As System.Windows.Forms.Button
    Friend WithEvents btnIAX2 As System.Windows.Forms.Button
    Friend WithEvents btnDisconnect As System.Windows.Forms.Button
    Friend WithEvents btnMailCount As System.Windows.Forms.Button
    Friend WithEvents btnPing As System.Windows.Forms.Button
    Friend WithEvents btnOriginate As System.Windows.Forms.Button

End Class

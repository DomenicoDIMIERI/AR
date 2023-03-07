<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ListDialog
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.OK_Button = New System.Windows.Forms.Button
        Me.ApplicationList = New System.Windows.Forms.ListBox
        Me.CommandUninstall = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.Location = New System.Drawing.Point(260, 15)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(67, 23)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "OK"
        '
        'ApplicationList
        '
        Me.ApplicationList.FormattingEnabled = True
        Me.ApplicationList.Location = New System.Drawing.Point(15, 14)
        Me.ApplicationList.Name = "ApplicationList"
        Me.ApplicationList.Size = New System.Drawing.Size(228, 290)
        Me.ApplicationList.TabIndex = 1
        '
        'CommandUninstall
        '
        Me.CommandUninstall.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.CommandUninstall.Enabled = False
        Me.CommandUninstall.Location = New System.Drawing.Point(260, 61)
        Me.CommandUninstall.Name = "CommandUninstall"
        Me.CommandUninstall.Size = New System.Drawing.Size(67, 23)
        Me.CommandUninstall.TabIndex = 2
        Me.CommandUninstall.Text = "Uninstall"
        '
        'ListDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(339, 315)
        Me.Controls.Add(Me.CommandUninstall)
        Me.Controls.Add(Me.OK_Button)
        Me.Controls.Add(Me.ApplicationList)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ListDialog"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "ListDialog"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents ApplicationList As System.Windows.Forms.ListBox
    Friend WithEvents CommandUninstall As System.Windows.Forms.Button

End Class

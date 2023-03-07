<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RightMenu
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
        Me.bottomMenu = New System.Windows.Forms.Panel()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.LoginControl1 = New FinSeA.LoginControl()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'bottomMenu
        '
        Me.bottomMenu.AutoScroll = True
        Me.bottomMenu.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.bottomMenu.Location = New System.Drawing.Point(0, 309)
        Me.bottomMenu.Name = "bottomMenu"
        Me.bottomMenu.Size = New System.Drawing.Size(229, 114)
        Me.bottomMenu.TabIndex = 0
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 285)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(229, 24)
        Me.Panel1.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(5, 6)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(55, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "MODULI"
        '
        'LoginControl1
        '
        Me.LoginControl1.Dock = System.Windows.Forms.DockStyle.Top
        Me.LoginControl1.Location = New System.Drawing.Point(0, 0)
        Me.LoginControl1.Name = "LoginControl1"
        Me.LoginControl1.Size = New System.Drawing.Size(229, 99)
        Me.LoginControl1.TabIndex = 2
        '
        'RightMenu
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.LoginControl1)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.bottomMenu)
        Me.Name = "RightMenu"
        Me.Size = New System.Drawing.Size(229, 423)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents bottomMenu As System.Windows.Forms.Panel
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents LoginControl1 As FinSeA.LoginControl

End Class

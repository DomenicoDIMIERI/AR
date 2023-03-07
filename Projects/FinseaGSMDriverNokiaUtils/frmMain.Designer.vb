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
        Me.components = New System.ComponentModel.Container()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnFindDB = New System.Windows.Forms.Button()
        Me.txtDB = New System.Windows.Forms.TextBox()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.txtLog = New System.Windows.Forms.TextBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(56, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Database:"
        '
        'btnFindDB
        '
        Me.btnFindDB.Location = New System.Drawing.Point(588, 4)
        Me.btnFindDB.Name = "btnFindDB"
        Me.btnFindDB.Size = New System.Drawing.Size(75, 23)
        Me.btnFindDB.TabIndex = 1
        Me.btnFindDB.Text = "Cerca..."
        Me.btnFindDB.UseVisualStyleBackColor = True
        '
        'txtDB
        '
        Me.txtDB.Location = New System.Drawing.Point(74, 6)
        Me.txtDB.Name = "txtDB"
        Me.txtDB.Size = New System.Drawing.Size(508, 20)
        Me.txtDB.TabIndex = 2
        '
        'Timer1
        '
        Me.Timer1.Interval = 1000
        '
        'txtLog
        '
        Me.txtLog.Location = New System.Drawing.Point(12, 67)
        Me.txtLog.Multiline = True
        Me.txtLog.Name = "txtLog"
        Me.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtLog.Size = New System.Drawing.Size(651, 316)
        Me.txtLog.TabIndex = 3
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(588, 38)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 4
        Me.Button1.Text = "Scarica"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(675, 395)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.txtLog)
        Me.Controls.Add(Me.txtDB)
        Me.Controls.Add(Me.btnFindDB)
        Me.Controls.Add(Me.Label1)
        Me.Name = "frmMain"
        Me.Text = "Controllo Nuovi SMS Ricevuti"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnFindDB As System.Windows.Forms.Button
    Friend WithEvents txtDB As System.Windows.Forms.TextBox
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents txtLog As System.Windows.Forms.TextBox
    Friend WithEvents Button1 As System.Windows.Forms.Button

End Class

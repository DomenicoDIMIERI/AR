<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Me.cboSize = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtText = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.listPanel = New System.Windows.Forms.Panel()
        Me.btnUpdate = New System.Windows.Forms.Button()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.btnUpdate)
        Me.Panel1.Controls.Add(Me.cboSize)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.txtText)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(937, 70)
        Me.Panel1.TabIndex = 0
        '
        'cboSize
        '
        Me.cboSize.FormattingEnabled = True
        Me.cboSize.Items.AddRange(New Object() {"8", "10", "12", "14", "18", "22", "24", "28", "32", "48", "64", "72", "100", "120"})
        Me.cboSize.Location = New System.Drawing.Point(660, 6)
        Me.cboSize.Name = "cboSize"
        Me.cboSize.Size = New System.Drawing.Size(121, 21)
        Me.cboSize.TabIndex = 3
        Me.cboSize.Text = "32"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(589, 9)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(65, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Dimensione:"
        '
        'txtText
        '
        Me.txtText.Location = New System.Drawing.Point(127, 6)
        Me.txtText.Multiline = True
        Me.txtText.Name = "txtText"
        Me.txtText.Size = New System.Drawing.Size(444, 58)
        Me.txtText.TabIndex = 1
        Me.txtText.Text = "Prova Testo 123 ?"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(109, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Testo da visualizzare:"
        '
        'listPanel
        '
        Me.listPanel.AutoScroll = True
        Me.listPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.listPanel.Location = New System.Drawing.Point(0, 70)
        Me.listPanel.Name = "listPanel"
        Me.listPanel.Size = New System.Drawing.Size(937, 526)
        Me.listPanel.TabIndex = 1
        '
        'btnUpdate
        '
        Me.btnUpdate.Location = New System.Drawing.Point(798, 4)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(75, 23)
        Me.btnUpdate.TabIndex = 4
        Me.btnUpdate.Text = "Aggiorna"
        Me.btnUpdate.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(937, 596)
        Me.Controls.Add(Me.listPanel)
        Me.Controls.Add(Me.Panel1)
        Me.Name = "Form1"
        Me.Text = "Comparatore di Font"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents cboSize As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtText As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents listPanel As System.Windows.Forms.Panel
    Friend WithEvents btnUpdate As System.Windows.Forms.Button

End Class

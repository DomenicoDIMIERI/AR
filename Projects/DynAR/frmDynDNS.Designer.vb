<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDynAR
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDynAR))
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.btnAdd = New System.Windows.Forms.ToolStripButton()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.lblError1 = New System.Windows.Forms.Label()
        Me.txtUfficio1 = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtPING1 = New System.Windows.Forms.TextBox()
        Me.lbl = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.lblError2 = New System.Windows.Forms.Label()
        Me.txtUfficio2 = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtPING2 = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.lblError3 = New System.Windows.Forms.Label()
        Me.txtUfficio3 = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtPING3 = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.lblError4 = New System.Windows.Forms.Label()
        Me.txtUfficio4 = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtPING4 = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.ToolStrip1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.SuspendLayout()
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnAdd})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(555, 25)
        Me.ToolStrip1.TabIndex = 0
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'btnAdd
        '
        Me.btnAdd.Image = CType(resources.GetObject("btnAdd.Image"), System.Drawing.Image)
        Me.btnAdd.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(51, 22)
        Me.btnAdd.Text = "Ping"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.lblError1)
        Me.Panel1.Controls.Add(Me.txtUfficio1)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.txtPING1)
        Me.Panel1.Controls.Add(Me.lbl)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 25)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(555, 49)
        Me.Panel1.TabIndex = 1
        '
        'lblError1
        '
        Me.lblError1.AutoSize = True
        Me.lblError1.ForeColor = System.Drawing.Color.Maroon
        Me.lblError1.Location = New System.Drawing.Point(12, 33)
        Me.lblError1.Name = "lblError1"
        Me.lblError1.Size = New System.Drawing.Size(16, 13)
        Me.lblError1.TabIndex = 4
        Me.lblError1.Text = ",,,"
        '
        'txtUfficio1
        '
        Me.txtUfficio1.Location = New System.Drawing.Point(340, 7)
        Me.txtUfficio1.Name = "txtUfficio1"
        Me.txtUfficio1.Size = New System.Drawing.Size(203, 20)
        Me.txtUfficio1.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(294, 10)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(40, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Ufficio:"
        '
        'txtPING1
        '
        Me.txtPING1.Location = New System.Drawing.Point(46, 7)
        Me.txtPING1.Name = "txtPING1"
        Me.txtPING1.Size = New System.Drawing.Size(242, 20)
        Me.txtPING1.TabIndex = 1
        '
        'lbl
        '
        Me.lbl.AutoSize = True
        Me.lbl.Location = New System.Drawing.Point(12, 10)
        Me.lbl.Name = "lbl"
        Me.lbl.Size = New System.Drawing.Size(31, 13)
        Me.lbl.TabIndex = 0
        Me.lbl.Text = "Ping:"
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.lblError2)
        Me.Panel2.Controls.Add(Me.txtUfficio2)
        Me.Panel2.Controls.Add(Me.Label2)
        Me.Panel2.Controls.Add(Me.txtPING2)
        Me.Panel2.Controls.Add(Me.Label3)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel2.Location = New System.Drawing.Point(0, 74)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(555, 49)
        Me.Panel2.TabIndex = 2
        '
        'lblError2
        '
        Me.lblError2.AutoSize = True
        Me.lblError2.ForeColor = System.Drawing.Color.Maroon
        Me.lblError2.Location = New System.Drawing.Point(12, 33)
        Me.lblError2.Name = "lblError2"
        Me.lblError2.Size = New System.Drawing.Size(16, 13)
        Me.lblError2.TabIndex = 5
        Me.lblError2.Text = ",,,"
        '
        'txtUfficio2
        '
        Me.txtUfficio2.Location = New System.Drawing.Point(340, 8)
        Me.txtUfficio2.Name = "txtUfficio2"
        Me.txtUfficio2.Size = New System.Drawing.Size(203, 20)
        Me.txtUfficio2.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(294, 11)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(40, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Ufficio:"
        '
        'txtPING2
        '
        Me.txtPING2.Location = New System.Drawing.Point(46, 8)
        Me.txtPING2.Name = "txtPING2"
        Me.txtPING2.Size = New System.Drawing.Size(242, 20)
        Me.txtPING2.TabIndex = 1
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 11)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(31, 13)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Ping:"
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.lblError3)
        Me.Panel3.Controls.Add(Me.txtUfficio3)
        Me.Panel3.Controls.Add(Me.Label4)
        Me.Panel3.Controls.Add(Me.txtPING3)
        Me.Panel3.Controls.Add(Me.Label5)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel3.Location = New System.Drawing.Point(0, 123)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(555, 49)
        Me.Panel3.TabIndex = 3
        '
        'lblError3
        '
        Me.lblError3.AutoSize = True
        Me.lblError3.ForeColor = System.Drawing.Color.Maroon
        Me.lblError3.Location = New System.Drawing.Point(12, 33)
        Me.lblError3.Name = "lblError3"
        Me.lblError3.Size = New System.Drawing.Size(16, 13)
        Me.lblError3.TabIndex = 5
        Me.lblError3.Text = ",,,"
        '
        'txtUfficio3
        '
        Me.txtUfficio3.Location = New System.Drawing.Point(340, 7)
        Me.txtUfficio3.Name = "txtUfficio3"
        Me.txtUfficio3.Size = New System.Drawing.Size(203, 20)
        Me.txtUfficio3.TabIndex = 3
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(294, 10)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(40, 13)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "Ufficio:"
        '
        'txtPING3
        '
        Me.txtPING3.Location = New System.Drawing.Point(46, 7)
        Me.txtPING3.Name = "txtPING3"
        Me.txtPING3.Size = New System.Drawing.Size(242, 20)
        Me.txtPING3.TabIndex = 1
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(12, 10)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(31, 13)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "Ping:"
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.lblError4)
        Me.Panel4.Controls.Add(Me.txtUfficio4)
        Me.Panel4.Controls.Add(Me.Label6)
        Me.Panel4.Controls.Add(Me.txtPING4)
        Me.Panel4.Controls.Add(Me.Label7)
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel4.Location = New System.Drawing.Point(0, 172)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(555, 49)
        Me.Panel4.TabIndex = 4
        '
        'lblError4
        '
        Me.lblError4.AutoSize = True
        Me.lblError4.ForeColor = System.Drawing.Color.Maroon
        Me.lblError4.Location = New System.Drawing.Point(12, 36)
        Me.lblError4.Name = "lblError4"
        Me.lblError4.Size = New System.Drawing.Size(16, 13)
        Me.lblError4.TabIndex = 5
        Me.lblError4.Text = ",,,"
        '
        'txtUfficio4
        '
        Me.txtUfficio4.Location = New System.Drawing.Point(340, 8)
        Me.txtUfficio4.Name = "txtUfficio4"
        Me.txtUfficio4.Size = New System.Drawing.Size(203, 20)
        Me.txtUfficio4.TabIndex = 3
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(294, 11)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(40, 13)
        Me.Label6.TabIndex = 2
        Me.Label6.Text = "Ufficio:"
        '
        'txtPING4
        '
        Me.txtPING4.Location = New System.Drawing.Point(46, 8)
        Me.txtPING4.Name = "txtPING4"
        Me.txtPING4.Size = New System.Drawing.Size(242, 20)
        Me.txtPING4.TabIndex = 1
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(12, 11)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(31, 13)
        Me.Label7.TabIndex = 0
        Me.Label7.Text = "Ping:"
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        Me.Timer1.Interval = 30000
        '
        'frmDynAR
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(555, 246)
        Me.Controls.Add(Me.Panel4)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.ToolStrip1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmDynAR"
        Me.Text = "DynAR"
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ToolStrip1 As ToolStrip
    Friend WithEvents btnAdd As ToolStripButton
    Friend WithEvents Panel1 As Panel
    Friend WithEvents txtUfficio1 As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents txtPING1 As TextBox
    Friend WithEvents lbl As Label
    Friend WithEvents Panel2 As Panel
    Friend WithEvents txtUfficio2 As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents txtPING2 As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents Panel3 As Panel
    Friend WithEvents txtUfficio3 As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents txtPING3 As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents Panel4 As Panel
    Friend WithEvents txtUfficio4 As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents txtPING4 As TextBox
    Friend WithEvents Label7 As Label
    Friend WithEvents Timer1 As Timer
    Friend WithEvents lblError1 As Label
    Friend WithEvents lblError2 As Label
    Friend WithEvents lblError3 As Label
    Friend WithEvents lblError4 As Label
End Class

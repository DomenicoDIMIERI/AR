<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDialer
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
        Me.btnDial = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtNumber = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.cboDialers = New System.Windows.Forms.ComboBox()
        Me.panelButtons = New System.Windows.Forms.Panel()
        Me.txtCentralino = New System.Windows.Forms.ComboBox()
        Me.panelNumber = New System.Windows.Forms.Panel()
        Me.lstResult = New System.Windows.Forms.ListView()
        Me.colIcon = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.Label4 = New System.Windows.Forms.Label()
        Me.panelCorrispondenze = New System.Windows.Forms.Panel()
        Me.panelNumberInfo = New System.Windows.Forms.Panel()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtBloccatoIl = New System.Windows.Forms.TextBox()
        Me.txtBloccatoDa = New System.Windows.Forms.TextBox()
        Me.txtMotivoBlocco = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.panelInfoNumero = New System.Windows.Forms.Panel()
        Me.txtInfoAggiuntive = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.panelButtons.SuspendLayout()
        Me.panelNumber.SuspendLayout()
        Me.panelCorrispondenze.SuspendLayout()
        Me.panelNumberInfo.SuspendLayout()
        Me.panelInfoNumero.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnDial
        '
        Me.btnDial.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDial.BackColor = System.Drawing.Color.Green
        Me.btnDial.Enabled = False
        Me.btnDial.ForeColor = System.Drawing.Color.White
        Me.btnDial.Location = New System.Drawing.Point(6, 5)
        Me.btnDial.Name = "btnDial"
        Me.btnDial.Size = New System.Drawing.Size(169, 27)
        Me.btnDial.TabIndex = 1
        Me.btnDial.Text = "Componi"
        Me.btnDial.UseVisualStyleBackColor = False
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.BackColor = System.Drawing.Color.Red
        Me.btnCancel.ForeColor = System.Drawing.Color.White
        Me.btnCancel.Location = New System.Drawing.Point(309, 5)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(168, 27)
        Me.btnCancel.TabIndex = 4
        Me.btnCancel.Text = "Annulla"
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.DarkBlue
        Me.Label1.Location = New System.Drawing.Point(3, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(47, 18)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Linea:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.DarkBlue
        Me.Label2.Location = New System.Drawing.Point(171, 15)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(62, 18)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Numero"
        '
        'txtNumber
        '
        Me.txtNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNumber.Location = New System.Drawing.Point(239, 12)
        Me.txtNumber.Name = "txtNumber"
        Me.txtNumber.Size = New System.Drawing.Size(234, 24)
        Me.txtNumber.TabIndex = 0
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.DarkBlue
        Me.Label3.Location = New System.Drawing.Point(3, 45)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(147, 18)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Componi utilizzando:"
        '
        'cboDialers
        '
        Me.cboDialers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboDialers.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboDialers.FormattingEnabled = True
        Me.cboDialers.Location = New System.Drawing.Point(239, 42)
        Me.cboDialers.Name = "cboDialers"
        Me.cboDialers.Size = New System.Drawing.Size(234, 26)
        Me.cboDialers.TabIndex = 3
        '
        'panelButtons
        '
        Me.panelButtons.Controls.Add(Me.btnDial)
        Me.panelButtons.Controls.Add(Me.btnCancel)
        Me.panelButtons.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.panelButtons.Location = New System.Drawing.Point(0, 393)
        Me.panelButtons.Name = "panelButtons"
        Me.panelButtons.Size = New System.Drawing.Size(482, 35)
        Me.panelButtons.TabIndex = 9
        '
        'txtCentralino
        '
        Me.txtCentralino.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.txtCentralino.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCentralino.FormattingEnabled = True
        Me.txtCentralino.Location = New System.Drawing.Point(56, 9)
        Me.txtCentralino.Name = "txtCentralino"
        Me.txtCentralino.Size = New System.Drawing.Size(109, 26)
        Me.txtCentralino.TabIndex = 2
        '
        'panelNumber
        '
        Me.panelNumber.Controls.Add(Me.Label1)
        Me.panelNumber.Controls.Add(Me.txtCentralino)
        Me.panelNumber.Controls.Add(Me.Label2)
        Me.panelNumber.Controls.Add(Me.txtNumber)
        Me.panelNumber.Controls.Add(Me.cboDialers)
        Me.panelNumber.Controls.Add(Me.Label3)
        Me.panelNumber.Dock = System.Windows.Forms.DockStyle.Top
        Me.panelNumber.Location = New System.Drawing.Point(0, 0)
        Me.panelNumber.Name = "panelNumber"
        Me.panelNumber.Size = New System.Drawing.Size(482, 76)
        Me.panelNumber.TabIndex = 11
        '
        'lstResult
        '
        Me.lstResult.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colIcon})
        Me.lstResult.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstResult.LargeImageList = Me.ImageList1
        Me.lstResult.Location = New System.Drawing.Point(0, 18)
        Me.lstResult.Name = "lstResult"
        Me.lstResult.Size = New System.Drawing.Size(482, 126)
        Me.lstResult.SmallImageList = Me.ImageList1
        Me.lstResult.StateImageList = Me.ImageList1
        Me.lstResult.TabIndex = 7
        Me.lstResult.UseCompatibleStateImageBehavior = False
        Me.lstResult.View = System.Windows.Forms.View.Details
        '
        'colIcon
        '
        Me.colIcon.Text = "Nome"
        Me.colIcon.Width = 620
        '
        'ImageList1
        '
        Me.ImageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit
        Me.ImageList1.ImageSize = New System.Drawing.Size(16, 16)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.DarkBlue
        Me.Label4.Location = New System.Drawing.Point(0, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(118, 18)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "Corrispondenze:"
        '
        'panelCorrispondenze
        '
        Me.panelCorrispondenze.Controls.Add(Me.lstResult)
        Me.panelCorrispondenze.Controls.Add(Me.Label4)
        Me.panelCorrispondenze.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panelCorrispondenze.Location = New System.Drawing.Point(0, 249)
        Me.panelCorrispondenze.Name = "panelCorrispondenze"
        Me.panelCorrispondenze.Size = New System.Drawing.Size(482, 144)
        Me.panelCorrispondenze.TabIndex = 12
        '
        'panelNumberInfo
        '
        Me.panelNumberInfo.Controls.Add(Me.Label7)
        Me.panelNumberInfo.Controls.Add(Me.txtBloccatoIl)
        Me.panelNumberInfo.Controls.Add(Me.txtBloccatoDa)
        Me.panelNumberInfo.Controls.Add(Me.txtMotivoBlocco)
        Me.panelNumberInfo.Controls.Add(Me.Label6)
        Me.panelNumberInfo.Controls.Add(Me.Label5)
        Me.panelNumberInfo.Dock = System.Windows.Forms.DockStyle.Top
        Me.panelNumberInfo.Location = New System.Drawing.Point(0, 76)
        Me.panelNumberInfo.Name = "panelNumberInfo"
        Me.panelNumberInfo.Size = New System.Drawing.Size(482, 91)
        Me.panelNumberInfo.TabIndex = 13
        Me.panelNumberInfo.Visible = False
        '
        'Label7
        '
        Me.Label7.BackColor = System.Drawing.Color.Red
        Me.Label7.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.White
        Me.Label7.Location = New System.Drawing.Point(0, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(482, 20)
        Me.Label7.TabIndex = 12
        Me.Label7.Text = "ATTENZIONE!!! NUMERO INTEREDETTO"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtBloccatoIl
        '
        Me.txtBloccatoIl.ForeColor = System.Drawing.Color.Red
        Me.txtBloccatoIl.Location = New System.Drawing.Point(330, 24)
        Me.txtBloccatoIl.Name = "txtBloccatoIl"
        Me.txtBloccatoIl.ReadOnly = True
        Me.txtBloccatoIl.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtBloccatoIl.Size = New System.Drawing.Size(143, 20)
        Me.txtBloccatoIl.TabIndex = 11
        Me.txtBloccatoIl.Text = "..."
        '
        'txtBloccatoDa
        '
        Me.txtBloccatoDa.ForeColor = System.Drawing.Color.Red
        Me.txtBloccatoDa.Location = New System.Drawing.Point(90, 24)
        Me.txtBloccatoDa.Name = "txtBloccatoDa"
        Me.txtBloccatoDa.ReadOnly = True
        Me.txtBloccatoDa.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtBloccatoDa.Size = New System.Drawing.Size(210, 20)
        Me.txtBloccatoDa.TabIndex = 10
        '
        'txtMotivoBlocco
        '
        Me.txtMotivoBlocco.ForeColor = System.Drawing.Color.Red
        Me.txtMotivoBlocco.Location = New System.Drawing.Point(6, 46)
        Me.txtMotivoBlocco.Multiline = True
        Me.txtMotivoBlocco.Name = "txtMotivoBlocco"
        Me.txtMotivoBlocco.ReadOnly = True
        Me.txtMotivoBlocco.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtMotivoBlocco.Size = New System.Drawing.Size(471, 36)
        Me.txtMotivoBlocco.TabIndex = 9
        Me.txtMotivoBlocco.Text = "..."
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.DarkBlue
        Me.Label6.Location = New System.Drawing.Point(306, 26)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(18, 18)
        Me.Label6.TabIndex = 8
        Me.Label6.Text = "il:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.DarkBlue
        Me.Label5.Location = New System.Drawing.Point(3, 23)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(91, 18)
        Me.Label5.TabIndex = 7
        Me.Label5.Text = "Bloccato da:"
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        Me.Timer1.Interval = 1000
        '
        'panelInfoNumero
        '
        Me.panelInfoNumero.Controls.Add(Me.txtInfoAggiuntive)
        Me.panelInfoNumero.Controls.Add(Me.Label8)
        Me.panelInfoNumero.Dock = System.Windows.Forms.DockStyle.Top
        Me.panelInfoNumero.Location = New System.Drawing.Point(0, 167)
        Me.panelInfoNumero.Name = "panelInfoNumero"
        Me.panelInfoNumero.Size = New System.Drawing.Size(482, 82)
        Me.panelInfoNumero.TabIndex = 14
        Me.panelInfoNumero.Visible = False
        '
        'txtInfoAggiuntive
        '
        Me.txtInfoAggiuntive.BackColor = System.Drawing.Color.LightYellow
        Me.txtInfoAggiuntive.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtInfoAggiuntive.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInfoAggiuntive.ForeColor = System.Drawing.Color.Blue
        Me.txtInfoAggiuntive.Location = New System.Drawing.Point(0, 20)
        Me.txtInfoAggiuntive.Multiline = True
        Me.txtInfoAggiuntive.Name = "txtInfoAggiuntive"
        Me.txtInfoAggiuntive.Size = New System.Drawing.Size(482, 62)
        Me.txtInfoAggiuntive.TabIndex = 13
        '
        'Label8
        '
        Me.Label8.BackColor = System.Drawing.Color.Gold
        Me.Label8.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.Blue
        Me.Label8.Location = New System.Drawing.Point(0, 0)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(482, 20)
        Me.Label8.TabIndex = 12
        Me.Label8.Text = "DA PAGINEBIANCHE.IT"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'frmDialer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(482, 428)
        Me.ControlBox = False
        Me.Controls.Add(Me.panelCorrispondenze)
        Me.Controls.Add(Me.panelInfoNumero)
        Me.Controls.Add(Me.panelButtons)
        Me.Controls.Add(Me.panelNumberInfo)
        Me.Controls.Add(Me.panelNumber)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmDialer"
        Me.ShowIcon = False
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Componi il numero..."
        Me.TopMost = True
        Me.panelButtons.ResumeLayout(False)
        Me.panelNumber.ResumeLayout(False)
        Me.panelNumber.PerformLayout()
        Me.panelCorrispondenze.ResumeLayout(False)
        Me.panelCorrispondenze.PerformLayout()
        Me.panelNumberInfo.ResumeLayout(False)
        Me.panelNumberInfo.PerformLayout()
        Me.panelInfoNumero.ResumeLayout(False)
        Me.panelInfoNumero.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnDial As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtNumber As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents cboDialers As System.Windows.Forms.ComboBox
    Friend WithEvents panelButtons As System.Windows.Forms.Panel
    Friend WithEvents txtCentralino As System.Windows.Forms.ComboBox
    Friend WithEvents panelNumber As System.Windows.Forms.Panel
    Friend WithEvents lstResult As System.Windows.Forms.ListView
    Friend WithEvents colIcon As System.Windows.Forms.ColumnHeader
    Friend WithEvents Label4 As Label
    Friend WithEvents panelCorrispondenze As Panel
    Friend WithEvents panelNumberInfo As Panel
    Friend WithEvents txtBloccatoIl As TextBox
    Friend WithEvents txtBloccatoDa As TextBox
    Friend WithEvents txtMotivoBlocco As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents Timer1 As Timer
    Friend WithEvents ImageList1 As ImageList
    Friend WithEvents panelInfoNumero As Panel
    Friend WithEvents Label8 As Label
    Friend WithEvents txtInfoAggiuntive As TextBox
End Class

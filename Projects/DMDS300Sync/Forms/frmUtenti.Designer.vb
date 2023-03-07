<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUtenti
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
        Me.lstUsers = New System.Windows.Forms.ListView()
        Me.colID = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colUtente = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.btnLoadFP2 = New System.Windows.Forms.Button()
        Me.btnLoadFP1 = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.btnSaveFP2 = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnSaveFP1 = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtUserName = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtUserID = New System.Windows.Forms.TextBox()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnRefill = New System.Windows.Forms.Button()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'lstUsers
        '
        Me.lstUsers.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colID, Me.colUtente})
        Me.lstUsers.FullRowSelect = True
        Me.lstUsers.Location = New System.Drawing.Point(0, 3)
        Me.lstUsers.MultiSelect = False
        Me.lstUsers.Name = "lstUsers"
        Me.lstUsers.Size = New System.Drawing.Size(217, 365)
        Me.lstUsers.TabIndex = 0
        Me.lstUsers.UseCompatibleStateImageBehavior = False
        Me.lstUsers.View = System.Windows.Forms.View.Details
        '
        'colID
        '
        Me.colID.Text = "ID"
        Me.colID.Width = 53
        '
        'colUtente
        '
        Me.colUtente.Text = "Utente"
        Me.colUtente.Width = 150
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.btnLoadFP2)
        Me.GroupBox1.Controls.Add(Me.btnLoadFP1)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.btnSaveFP2)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.btnSaveFP1)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.txtUserName)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.txtUserID)
        Me.GroupBox1.Location = New System.Drawing.Point(223, 32)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(268, 136)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Utente"
        '
        'btnLoadFP2
        '
        Me.btnLoadFP2.Location = New System.Drawing.Point(202, 95)
        Me.btnLoadFP2.Name = "btnLoadFP2"
        Me.btnLoadFP2.Size = New System.Drawing.Size(48, 23)
        Me.btnLoadFP2.TabIndex = 9
        Me.btnLoadFP2.Text = "Carica"
        Me.btnLoadFP2.UseVisualStyleBackColor = True
        '
        'btnLoadFP1
        '
        Me.btnLoadFP1.Location = New System.Drawing.Point(202, 70)
        Me.btnLoadFP1.Name = "btnLoadFP1"
        Me.btnLoadFP1.Size = New System.Drawing.Size(48, 23)
        Me.btnLoadFP1.TabIndex = 8
        Me.btnLoadFP1.Text = "Carica"
        Me.btnLoadFP1.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(15, 100)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(57, 13)
        Me.Label4.TabIndex = 7
        Me.Label4.Text = "Impronta 2"
        '
        'btnSaveFP2
        '
        Me.btnSaveFP2.Location = New System.Drawing.Point(150, 95)
        Me.btnSaveFP2.Name = "btnSaveFP2"
        Me.btnSaveFP2.Size = New System.Drawing.Size(48, 23)
        Me.btnSaveFP2.TabIndex = 6
        Me.btnSaveFP2.Text = "Salva"
        Me.btnSaveFP2.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(15, 75)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(57, 13)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "Impronta 1"
        '
        'btnSaveFP1
        '
        Me.btnSaveFP1.Location = New System.Drawing.Point(150, 70)
        Me.btnSaveFP1.Name = "btnSaveFP1"
        Me.btnSaveFP1.Size = New System.Drawing.Size(48, 23)
        Me.btnSaveFP1.TabIndex = 4
        Me.btnSaveFP1.Text = "Salva"
        Me.btnSaveFP1.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(15, 44)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(35, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Nome"
        '
        'txtUserName
        '
        Me.txtUserName.Location = New System.Drawing.Point(150, 44)
        Me.txtUserName.Name = "txtUserName"
        Me.txtUserName.Size = New System.Drawing.Size(100, 20)
        Me.txtUserName.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(15, 22)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(18, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "ID"
        '
        'txtUserID
        '
        Me.txtUserID.Location = New System.Drawing.Point(150, 22)
        Me.txtUserID.Name = "txtUserID"
        Me.txtUserID.Size = New System.Drawing.Size(100, 20)
        Me.txtUserID.TabIndex = 0
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(223, 3)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(75, 23)
        Me.btnNew.TabIndex = 2
        Me.btnNew.Text = "Nuovo"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(398, 174)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(75, 23)
        Me.btnSave.TabIndex = 3
        Me.btnSave.Text = "Salva"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnRefill
        '
        Me.btnRefill.Location = New System.Drawing.Point(304, 3)
        Me.btnRefill.Name = "btnRefill"
        Me.btnRefill.Size = New System.Drawing.Size(75, 23)
        Me.btnRefill.TabIndex = 4
        Me.btnRefill.Text = "Aggiorna"
        Me.btnRefill.UseVisualStyleBackColor = True
        '
        'frmUtenti
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(503, 371)
        Me.Controls.Add(Me.btnRefill)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.btnNew)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.lstUsers)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmUtenti"
        Me.Text = "Utenti"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents lstUsers As Windows.Forms.ListView
    Friend WithEvents colID As Windows.Forms.ColumnHeader
    Friend WithEvents colUtente As Windows.Forms.ColumnHeader
    Friend WithEvents GroupBox1 As Windows.Forms.GroupBox
    Friend WithEvents btnLoadFP2 As Windows.Forms.Button
    Friend WithEvents btnLoadFP1 As Windows.Forms.Button
    Friend WithEvents Label4 As Windows.Forms.Label
    Friend WithEvents btnSaveFP2 As Windows.Forms.Button
    Friend WithEvents Label3 As Windows.Forms.Label
    Friend WithEvents btnSaveFP1 As Windows.Forms.Button
    Friend WithEvents Label2 As Windows.Forms.Label
    Friend WithEvents txtUserName As Windows.Forms.TextBox
    Friend WithEvents Label1 As Windows.Forms.Label
    Friend WithEvents txtUserID As Windows.Forms.TextBox
    Friend WithEvents btnNew As Windows.Forms.Button
    Friend WithEvents btnSave As Windows.Forms.Button
    Friend WithEvents btnRefill As Windows.Forms.Button
End Class

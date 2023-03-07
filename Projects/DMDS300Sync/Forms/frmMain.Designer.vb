<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
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
        Me.txtLog = New System.Windows.Forms.TextBox()
        Me.GroupBox5 = New System.Windows.Forms.GroupBox()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.FilleToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ChiudiToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.VerificaAggiornamentiToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripSeparator()
        Me.InformazioniSuToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ImpostazioniToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GeneraliToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ListaDispositiviToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.GroupBox5.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtLog
        '
        Me.txtLog.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtLog.Location = New System.Drawing.Point(3, 16)
        Me.txtLog.Multiline = True
        Me.txtLog.Name = "txtLog"
        Me.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtLog.Size = New System.Drawing.Size(695, 530)
        Me.txtLog.TabIndex = 15
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.txtLog)
        Me.GroupBox5.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox5.Location = New System.Drawing.Point(0, 24)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(701, 549)
        Me.GroupBox5.TabIndex = 25
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "Log"
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FilleToolStripMenuItem, Me.ToolStripMenuItem1, Me.ImpostazioniToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(701, 24)
        Me.MenuStrip1.TabIndex = 26
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FilleToolStripMenuItem
        '
        Me.FilleToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ChiudiToolStripMenuItem})
        Me.FilleToolStripMenuItem.Name = "FilleToolStripMenuItem"
        Me.FilleToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.FilleToolStripMenuItem.Text = "&File"
        '
        'ChiudiToolStripMenuItem
        '
        Me.ChiudiToolStripMenuItem.Name = "ChiudiToolStripMenuItem"
        Me.ChiudiToolStripMenuItem.Size = New System.Drawing.Size(109, 22)
        Me.ChiudiToolStripMenuItem.Text = "&Chiudi"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.ToolStripMenuItem1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.VerificaAggiornamentiToolStripMenuItem, Me.ToolStripMenuItem2, Me.InformazioniSuToolStripMenuItem})
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(24, 20)
        Me.ToolStripMenuItem1.Text = "?"
        '
        'VerificaAggiornamentiToolStripMenuItem
        '
        Me.VerificaAggiornamentiToolStripMenuItem.Name = "VerificaAggiornamentiToolStripMenuItem"
        Me.VerificaAggiornamentiToolStripMenuItem.Size = New System.Drawing.Size(195, 22)
        Me.VerificaAggiornamentiToolStripMenuItem.Text = "Verifica &Aggiornamenti"
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(192, 6)
        '
        'InformazioniSuToolStripMenuItem
        '
        Me.InformazioniSuToolStripMenuItem.Name = "InformazioniSuToolStripMenuItem"
        Me.InformazioniSuToolStripMenuItem.Size = New System.Drawing.Size(195, 22)
        Me.InformazioniSuToolStripMenuItem.Text = "&Informazioni su"
        '
        'ImpostazioniToolStripMenuItem
        '
        Me.ImpostazioniToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.GeneraliToolStripMenuItem, Me.ListaDispositiviToolStripMenuItem})
        Me.ImpostazioniToolStripMenuItem.Name = "ImpostazioniToolStripMenuItem"
        Me.ImpostazioniToolStripMenuItem.Size = New System.Drawing.Size(87, 20)
        Me.ImpostazioniToolStripMenuItem.Text = "&Impostazioni"
        '
        'GeneraliToolStripMenuItem
        '
        Me.GeneraliToolStripMenuItem.Name = "GeneraliToolStripMenuItem"
        Me.GeneraliToolStripMenuItem.Size = New System.Drawing.Size(155, 22)
        Me.GeneraliToolStripMenuItem.Text = "&Generali"
        '
        'ListaDispositiviToolStripMenuItem
        '
        Me.ListaDispositiviToolStripMenuItem.Name = "ListaDispositiviToolStripMenuItem"
        Me.ListaDispositiviToolStripMenuItem.Size = New System.Drawing.Size(155, 22)
        Me.ListaDispositiviToolStripMenuItem.Text = "&Lista Dispositivi"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 573)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(701, 22)
        Me.StatusStrip1.TabIndex = 27
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        Me.Timer1.Interval = 10000
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(701, 595)
        Me.Controls.Add(Me.GroupBox5)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Controls.Add(Me.StatusStrip1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MainMenuStrip = Me.MenuStrip1
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmMain"
        Me.ShowIcon = False
        Me.Text = "DMD ANVIZ S300 Utility"
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtLog As Windows.Forms.TextBox
    Friend WithEvents GroupBox5 As Windows.Forms.GroupBox
    Friend WithEvents MenuStrip1 As Windows.Forms.MenuStrip
    Friend WithEvents FilleToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents ChiudiToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents StatusStrip1 As Windows.Forms.StatusStrip
    Friend WithEvents ToolStripMenuItem1 As Windows.Forms.ToolStripMenuItem
    Friend WithEvents VerificaAggiornamentiToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As Windows.Forms.ToolStripSeparator
    Friend WithEvents InformazioniSuToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents ImpostazioniToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents GeneraliToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents ListaDispositiviToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents Timer1 As Windows.Forms.Timer
End Class

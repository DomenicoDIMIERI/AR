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
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RiceviToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ChiudiToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StrumentiToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ImpostazioniToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.txtLog = New System.Windows.Forms.TextBox()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.StrumentiToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(734, 24)
        Me.MenuStrip1.TabIndex = 0
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.RiceviToolStripMenuItem, Me.ToolStripMenuItem1, Me.ChiudiToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(35, 20)
        Me.FileToolStripMenuItem.Text = "&File"
        '
        'RiceviToolStripMenuItem
        '
        Me.RiceviToolStripMenuItem.Name = "RiceviToolStripMenuItem"
        Me.RiceviToolStripMenuItem.Size = New System.Drawing.Size(103, 22)
        Me.RiceviToolStripMenuItem.Text = "&Ricevi"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(100, 6)
        '
        'ChiudiToolStripMenuItem
        '
        Me.ChiudiToolStripMenuItem.Name = "ChiudiToolStripMenuItem"
        Me.ChiudiToolStripMenuItem.Size = New System.Drawing.Size(103, 22)
        Me.ChiudiToolStripMenuItem.Text = "&Chiudi"
        '
        'StrumentiToolStripMenuItem
        '
        Me.StrumentiToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ImpostazioniToolStripMenuItem})
        Me.StrumentiToolStripMenuItem.Name = "StrumentiToolStripMenuItem"
        Me.StrumentiToolStripMenuItem.Size = New System.Drawing.Size(65, 20)
        Me.StrumentiToolStripMenuItem.Text = "&Strumenti"
        '
        'ImpostazioniToolStripMenuItem
        '
        Me.ImpostazioniToolStripMenuItem.Name = "ImpostazioniToolStripMenuItem"
        Me.ImpostazioniToolStripMenuItem.Size = New System.Drawing.Size(134, 22)
        Me.ImpostazioniToolStripMenuItem.Text = "&Impostazioni"
        '
        'txtLog
        '
        Me.txtLog.Location = New System.Drawing.Point(12, 38)
        Me.txtLog.Multiline = True
        Me.txtLog.Name = "txtLog"
        Me.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtLog.Size = New System.Drawing.Size(710, 436)
        Me.txtLog.TabIndex = 1
        '
        'Timer1
        '
        Me.Timer1.Interval = 300000
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(734, 486)
        Me.Controls.Add(Me.txtLog)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "frmMain"
        Me.Text = "BOT per le segnalazioni Stay"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents FileToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents RiceviToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As ToolStripSeparator
    Friend WithEvents ChiudiToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents StrumentiToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ImpostazioniToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents txtLog As TextBox
    Friend WithEvents Timer1 As Timer
End Class

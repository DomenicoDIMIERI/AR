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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.lstItems = New System.Windows.Forms.ListView()
        Me.colUserName = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colPercorso = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colServer = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.lstScansioni = New System.Windows.Forms.ListView()
        Me.colFile = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colUtente = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colStato = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.btnNew = New System.Windows.Forms.ToolStripButton()
        Me.btnDelete = New System.Windows.Forms.ToolStripButton()
        Me.btnEdit = New System.Windows.Forms.ToolStripButton()
        Me.ToolStrip1.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnNew, Me.btnDelete, Me.ToolStripSeparator1, Me.btnEdit})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(831, 25)
        Me.ToolStrip1.TabIndex = 0
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 475)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(831, 22)
        Me.StatusStrip1.TabIndex = 1
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(16, 17)
        Me.ToolStripStatusLabel1.Text = "..."
        '
        'lstItems
        '
        Me.lstItems.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colUserName, Me.colPercorso, Me.colServer})
        Me.lstItems.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstItems.Location = New System.Drawing.Point(0, 0)
        Me.lstItems.Name = "lstItems"
        Me.lstItems.Size = New System.Drawing.Size(468, 450)
        Me.lstItems.TabIndex = 2
        Me.lstItems.UseCompatibleStateImageBehavior = False
        Me.lstItems.View = System.Windows.Forms.View.Details
        '
        'colUserName
        '
        Me.colUserName.Text = "Utente"
        Me.colUserName.Width = 83
        '
        'colPercorso
        '
        Me.colPercorso.Text = "Percorso"
        Me.colPercorso.Width = 186
        '
        'colServer
        '
        Me.colServer.Text = "Server"
        Me.colServer.Width = 180
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 25)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.lstItems)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.lstScansioni)
        Me.SplitContainer1.Size = New System.Drawing.Size(831, 450)
        Me.SplitContainer1.SplitterDistance = 468
        Me.SplitContainer1.TabIndex = 3
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 25)
        '
        'lstScansioni
        '
        Me.lstScansioni.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colUtente, Me.colFile, Me.colStato})
        Me.lstScansioni.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstScansioni.Location = New System.Drawing.Point(0, 0)
        Me.lstScansioni.Name = "lstScansioni"
        Me.lstScansioni.Size = New System.Drawing.Size(359, 450)
        Me.lstScansioni.TabIndex = 3
        Me.lstScansioni.UseCompatibleStateImageBehavior = False
        Me.lstScansioni.View = System.Windows.Forms.View.Details
        '
        'colFile
        '
        Me.colFile.DisplayIndex = 0
        Me.colFile.Text = "File"
        Me.colFile.Width = 83
        '
        'colUtente
        '
        Me.colUtente.DisplayIndex = 1
        Me.colUtente.Text = "Utente"
        Me.colUtente.Width = 186
        '
        'colStato
        '
        Me.colStato.Text = "Stato"
        Me.colStato.Width = 180
        '
        'btnNew
        '
        Me.btnNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnNew.Image = Global.DMDScanHandler.My.Resources.Resources.btnnew
        Me.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(23, 22)
        Me.btnNew.Text = "Nuova Cartella Condivisa"
        '
        'btnDelete
        '
        Me.btnDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnDelete.Enabled = False
        Me.btnDelete.Image = CType(resources.GetObject("btnDelete.Image"), System.Drawing.Image)
        Me.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(23, 22)
        Me.btnDelete.Text = "Elimina..."
        '
        'btnEdit
        '
        Me.btnEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnEdit.Enabled = False
        Me.btnEdit.Image = CType(resources.GetObject("btnEdit.Image"), System.Drawing.Image)
        Me.btnEdit.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnEdit.Name = "btnEdit"
        Me.btnEdit.Size = New System.Drawing.Size(23, 22)
        Me.btnEdit.Text = "Modifica..."
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(831, 497)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmMain"
        Me.Text = "Sincronizzazione Cartelle Remote"
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ToolStrip1 As ToolStrip
    Friend WithEvents btnNew As ToolStripButton
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents ToolStripStatusLabel1 As ToolStripStatusLabel
    Friend WithEvents lstItems As ListView
    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents colUserName As ColumnHeader
    Friend WithEvents colPercorso As ColumnHeader
    Friend WithEvents colServer As ColumnHeader
    Friend WithEvents btnDelete As ToolStripButton
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents btnEdit As ToolStripButton
    Friend WithEvents lstScansioni As ListView
    Friend WithEvents colUtente As ColumnHeader
    Friend WithEvents colFile As ColumnHeader
    Friend WithEvents colStato As ColumnHeader
End Class

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        Me.txtLog = New System.Windows.Forms.TextBox()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.lblService = New System.Windows.Forms.Label()
        Me.btnOk = New System.Windows.Forms.Button()
        Me.notifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.notifyMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuApri = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem5 = New System.Windows.Forms.ToolStripSeparator()
        Me.CancellazioneFileSicuraToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.FormattazioneSicuraToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem6 = New System.Windows.Forms.ToolStripSeparator()
        Me.EsciToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ApriToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripSeparator()
        Me.ComponiUnnumeroToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSendFax = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.EsciToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.btnNewCall = New System.Windows.Forms.ToolStripButton()
        Me.btnSendFax = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripButton1 = New System.Windows.Forms.ToolStripButton()
        Me.btnLogOut = New System.Windows.Forms.ToolStripButton()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.inCallList = New System.Windows.Forms.ListView()
        Me.colInDate = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colInNumber = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colInDisposition = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colInLen = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colInMittente = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.inCallsPanel = New System.Windows.Forms.Panel()
        Me.outCallsPanel = New System.Windows.Forms.Panel()
        Me.outCallList = New System.Windows.Forms.ListView()
        Me.colOutDate = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colOutNumber = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colOutDisposition = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colOutLen = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colOutDestinatario = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Panel5 = New System.Windows.Forms.Panel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.mnuFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.EsciToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSettings = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuGeneralSettings = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem8 = New System.Windows.Forms.ToolStripSeparator()
        Me.ServerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AsteriskServersToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ServerHylaFaxToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ConfigurazioneEmailToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ProxyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.UploadServerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem3 = New System.Windows.Forms.ToolStripSeparator()
        Me.LineeDelCentralinoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DispositiviEsterniToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem4 = New System.Windows.Forms.ToolStripSeparator()
        Me.PasswordToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuParametri = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuCartelle = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem7 = New System.Windows.Forms.ToolStripMenuItem()
        Me.VerificaAggiornamentiToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.UtilitàToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FormattazioneSicuraToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CancellazioneFileSicuraToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.splashTimer = New System.Windows.Forms.Timer(Me.components)
        Me.activeTelTimer = New System.Windows.Forms.Timer(Me.components)
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.tabTelefonate = New System.Windows.Forms.TabPage()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.tabScansioni = New System.Windows.Forms.TabPage()
        Me.lstScansioni = New System.Windows.Forms.ListView()
        Me.colDataScansione = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colNomeFileScansione = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colDimensioneFileScansione = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.tabFAX = New System.Windows.Forms.TabPage()
        Me.tabLog = New System.Windows.Forms.TabPage()
        Me.timerScansioni = New System.Windows.Forms.Timer(Me.components)
        Me.timerUploadTel = New System.Windows.Forms.Timer(Me.components)
        Me.btnInterphone = New System.Windows.Forms.ToolStripButton()
        Me.Panel3.SuspendLayout()
        Me.notifyMenu.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.inCallsPanel.SuspendLayout()
        Me.outCallsPanel.SuspendLayout()
        Me.Panel5.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.tabTelefonate.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.tabScansioni.SuspendLayout()
        Me.tabLog.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtLog
        '
        Me.txtLog.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtLog.Location = New System.Drawing.Point(3, 3)
        Me.txtLog.Multiline = True
        Me.txtLog.Name = "txtLog"
        Me.txtLog.ReadOnly = True
        Me.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtLog.Size = New System.Drawing.Size(770, 407)
        Me.txtLog.TabIndex = 2
        Me.txtLog.TabStop = False
        Me.txtLog.WordWrap = False
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.lblService)
        Me.Panel3.Controls.Add(Me.btnOk)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel3.Location = New System.Drawing.Point(0, 502)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(784, 35)
        Me.Panel3.TabIndex = 3
        '
        'lblService
        '
        Me.lblService.AutoSize = True
        Me.lblService.Location = New System.Drawing.Point(12, 13)
        Me.lblService.Name = "lblService"
        Me.lblService.Size = New System.Drawing.Size(16, 13)
        Me.lblService.TabIndex = 2
        Me.lblService.Text = "..."
        '
        'btnOk
        '
        Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOk.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.btnOk.Location = New System.Drawing.Point(630, 3)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(151, 27)
        Me.btnOk.TabIndex = 1
        Me.btnOk.Text = "Nascondi"
        Me.btnOk.UseVisualStyleBackColor = False
        '
        'notifyIcon1
        '
        Me.notifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info
        Me.notifyIcon1.ContextMenuStrip = Me.notifyMenu
        Me.notifyIcon1.Text = "DIALTP 1.0"
        Me.notifyIcon1.Visible = True
        '
        'notifyMenu
        '
        Me.notifyMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuApri, Me.ToolStripMenuItem5, Me.CancellazioneFileSicuraToolStripMenuItem1, Me.FormattazioneSicuraToolStripMenuItem1, Me.ToolStripMenuItem6, Me.EsciToolStripMenuItem2})
        Me.notifyMenu.Name = "notifyMenu"
        Me.notifyMenu.Size = New System.Drawing.Size(201, 104)
        '
        'mnuApri
        '
        Me.mnuApri.Name = "mnuApri"
        Me.mnuApri.Size = New System.Drawing.Size(200, 22)
        Me.mnuApri.Text = "&Apri"
        '
        'ToolStripMenuItem5
        '
        Me.ToolStripMenuItem5.Name = "ToolStripMenuItem5"
        Me.ToolStripMenuItem5.Size = New System.Drawing.Size(197, 6)
        '
        'CancellazioneFileSicuraToolStripMenuItem1
        '
        Me.CancellazioneFileSicuraToolStripMenuItem1.Name = "CancellazioneFileSicuraToolStripMenuItem1"
        Me.CancellazioneFileSicuraToolStripMenuItem1.Size = New System.Drawing.Size(200, 22)
        Me.CancellazioneFileSicuraToolStripMenuItem1.Text = "&Cancellazione file sicura"
        '
        'FormattazioneSicuraToolStripMenuItem1
        '
        Me.FormattazioneSicuraToolStripMenuItem1.Name = "FormattazioneSicuraToolStripMenuItem1"
        Me.FormattazioneSicuraToolStripMenuItem1.Size = New System.Drawing.Size(200, 22)
        Me.FormattazioneSicuraToolStripMenuItem1.Text = "&Formattazione Sicura"
        '
        'ToolStripMenuItem6
        '
        Me.ToolStripMenuItem6.Name = "ToolStripMenuItem6"
        Me.ToolStripMenuItem6.Size = New System.Drawing.Size(197, 6)
        '
        'EsciToolStripMenuItem2
        '
        Me.EsciToolStripMenuItem2.Name = "EsciToolStripMenuItem2"
        Me.EsciToolStripMenuItem2.Size = New System.Drawing.Size(200, 22)
        Me.EsciToolStripMenuItem2.Text = "&Esci"
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ApriToolStripMenuItem, Me.ToolStripMenuItem2, Me.ComponiUnnumeroToolStripMenuItem, Me.mnuSendFax, Me.ToolStripMenuItem1, Me.EsciToolStripMenuItem1})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(187, 104)
        '
        'ApriToolStripMenuItem
        '
        Me.ApriToolStripMenuItem.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ApriToolStripMenuItem.Name = "ApriToolStripMenuItem"
        Me.ApriToolStripMenuItem.Size = New System.Drawing.Size(186, 22)
        Me.ApriToolStripMenuItem.Text = "&Apri"
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(183, 6)
        '
        'ComponiUnnumeroToolStripMenuItem
        '
        Me.ComponiUnnumeroToolStripMenuItem.Name = "ComponiUnnumeroToolStripMenuItem"
        Me.ComponiUnnumeroToolStripMenuItem.Size = New System.Drawing.Size(186, 22)
        Me.ComponiUnnumeroToolStripMenuItem.Text = "Componi un &numero"
        '
        'mnuSendFax
        '
        Me.mnuSendFax.Name = "mnuSendFax"
        Me.mnuSendFax.Size = New System.Drawing.Size(186, 22)
        Me.mnuSendFax.Text = "Invia un &Fax"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(183, 6)
        '
        'EsciToolStripMenuItem1
        '
        Me.EsciToolStripMenuItem1.Name = "EsciToolStripMenuItem1"
        Me.EsciToolStripMenuItem1.Size = New System.Drawing.Size(186, 22)
        Me.EsciToolStripMenuItem1.Text = "&Esci"
        '
        'ToolStrip1
        '
        Me.ToolStrip1.ImageScalingSize = New System.Drawing.Size(32, 32)
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnNewCall, Me.btnInterphone, Me.btnSendFax, Me.ToolStripSeparator1, Me.ToolStripButton1, Me.btnLogOut})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 24)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(784, 39)
        Me.ToolStrip1.TabIndex = 11
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'btnNewCall
        '
        Me.btnNewCall.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnNewCall.Image = CType(resources.GetObject("btnNewCall.Image"), System.Drawing.Image)
        Me.btnNewCall.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnNewCall.Name = "btnNewCall"
        Me.btnNewCall.Size = New System.Drawing.Size(36, 36)
        Me.btnNewCall.Text = "Nuova Chiamata"
        '
        'btnSendFax
        '
        Me.btnSendFax.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnSendFax.Image = Global.DMDApp.My.Resources.Resources.faxout
        Me.btnSendFax.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnSendFax.Name = "btnSendFax"
        Me.btnSendFax.Size = New System.Drawing.Size(36, 36)
        Me.btnSendFax.Text = "Invia un fax"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 39)
        '
        'ToolStripButton1
        '
        Me.ToolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton1.Enabled = False
        Me.ToolStripButton1.Image = Global.DMDApp.My.Resources.Resources.uparrow16
        Me.ToolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton1.Name = "ToolStripButton1"
        Me.ToolStripButton1.Size = New System.Drawing.Size(36, 36)
        Me.ToolStripButton1.Text = "Carica un file..."
        '
        'btnLogOut
        '
        Me.btnLogOut.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.btnLogOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnLogOut.Enabled = False
        Me.btnLogOut.Image = Global.DMDApp.My.Resources.Resources.LogOut
        Me.btnLogOut.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnLogOut.Name = "btnLogOut"
        Me.btnLogOut.Size = New System.Drawing.Size(36, 36)
        Me.btnLogOut.Text = "Logout"
        Me.btnLogOut.ToolTipText = "Logout"
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.Label1)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(350, 20)
        Me.Panel2.TabIndex = 13
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(3, 3)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(109, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Chiamate ricevute"
        '
        'inCallList
        '
        Me.inCallList.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colInDate, Me.colInNumber, Me.colInDisposition, Me.colInLen, Me.colInMittente})
        Me.inCallList.Dock = System.Windows.Forms.DockStyle.Fill
        Me.inCallList.Location = New System.Drawing.Point(0, 20)
        Me.inCallList.Name = "inCallList"
        Me.inCallList.Size = New System.Drawing.Size(350, 387)
        Me.inCallList.TabIndex = 15
        Me.inCallList.UseCompatibleStateImageBehavior = False
        Me.inCallList.View = System.Windows.Forms.View.Details
        '
        'colInDate
        '
        Me.colInDate.Text = "Data"
        Me.colInDate.Width = 113
        '
        'colInNumber
        '
        Me.colInNumber.Text = "Numero"
        Me.colInNumber.Width = 76
        '
        'colInDisposition
        '
        Me.colInDisposition.Text = "Azione"
        Me.colInDisposition.Width = 83
        '
        'colInLen
        '
        Me.colInLen.Text = "Durata"
        '
        'colInMittente
        '
        Me.colInMittente.Text = "Mittente"
        '
        'inCallsPanel
        '
        Me.inCallsPanel.Controls.Add(Me.inCallList)
        Me.inCallsPanel.Controls.Add(Me.Panel2)
        Me.inCallsPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.inCallsPanel.Location = New System.Drawing.Point(0, 0)
        Me.inCallsPanel.Name = "inCallsPanel"
        Me.inCallsPanel.Size = New System.Drawing.Size(350, 407)
        Me.inCallsPanel.TabIndex = 16
        '
        'outCallsPanel
        '
        Me.outCallsPanel.Controls.Add(Me.outCallList)
        Me.outCallsPanel.Controls.Add(Me.Panel5)
        Me.outCallsPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.outCallsPanel.Location = New System.Drawing.Point(0, 0)
        Me.outCallsPanel.Name = "outCallsPanel"
        Me.outCallsPanel.Size = New System.Drawing.Size(416, 407)
        Me.outCallsPanel.TabIndex = 17
        '
        'outCallList
        '
        Me.outCallList.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colOutDate, Me.colOutNumber, Me.colOutDisposition, Me.colOutLen, Me.colOutDestinatario})
        Me.outCallList.Dock = System.Windows.Forms.DockStyle.Fill
        Me.outCallList.Location = New System.Drawing.Point(0, 20)
        Me.outCallList.Name = "outCallList"
        Me.outCallList.Size = New System.Drawing.Size(416, 387)
        Me.outCallList.TabIndex = 15
        Me.outCallList.UseCompatibleStateImageBehavior = False
        Me.outCallList.View = System.Windows.Forms.View.Details
        '
        'colOutDate
        '
        Me.colOutDate.Text = "Data"
        Me.colOutDate.Width = 91
        '
        'colOutNumber
        '
        Me.colOutNumber.Text = "Numero"
        Me.colOutNumber.Width = 91
        '
        'colOutDisposition
        '
        Me.colOutDisposition.Text = "Azione"
        '
        'colOutLen
        '
        Me.colOutLen.Text = "Durata"
        '
        'colOutDestinatario
        '
        Me.colOutDestinatario.Text = "Destinatario"
        '
        'Panel5
        '
        Me.Panel5.Controls.Add(Me.Label2)
        Me.Panel5.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel5.Location = New System.Drawing.Point(0, 0)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(416, 20)
        Me.Panel5.TabIndex = 13
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(6, 3)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(118, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Chiamate effettuate"
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFile, Me.mnuSettings, Me.ToolStripMenuItem7, Me.UtilitàToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(784, 24)
        Me.MenuStrip1.TabIndex = 18
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'mnuFile
        '
        Me.mnuFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.EsciToolStripMenuItem})
        Me.mnuFile.Name = "mnuFile"
        Me.mnuFile.Size = New System.Drawing.Size(37, 20)
        Me.mnuFile.Text = "&File"
        '
        'EsciToolStripMenuItem
        '
        Me.EsciToolStripMenuItem.Name = "EsciToolStripMenuItem"
        Me.EsciToolStripMenuItem.Size = New System.Drawing.Size(94, 22)
        Me.EsciToolStripMenuItem.Text = "&Esci"
        '
        'mnuSettings
        '
        Me.mnuSettings.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuGeneralSettings, Me.ToolStripMenuItem8, Me.ServerToolStripMenuItem, Me.AsteriskServersToolStripMenuItem, Me.ServerHylaFaxToolStripMenuItem, Me.ConfigurazioneEmailToolStripMenuItem, Me.ProxyToolStripMenuItem, Me.UploadServerToolStripMenuItem, Me.ToolStripMenuItem3, Me.LineeDelCentralinoToolStripMenuItem, Me.DispositiviEsterniToolStripMenuItem1, Me.ToolStripMenuItem4, Me.PasswordToolStripMenuItem, Me.mnuParametri, Me.mnuCartelle})
        Me.mnuSettings.Name = "mnuSettings"
        Me.mnuSettings.Size = New System.Drawing.Size(87, 20)
        Me.mnuSettings.Text = "&Impostazioni"
        '
        'mnuGeneralSettings
        '
        Me.mnuGeneralSettings.Name = "mnuGeneralSettings"
        Me.mnuGeneralSettings.Size = New System.Drawing.Size(180, 22)
        Me.mnuGeneralSettings.Text = "&Generali"
        '
        'ToolStripMenuItem8
        '
        Me.ToolStripMenuItem8.Name = "ToolStripMenuItem8"
        Me.ToolStripMenuItem8.Size = New System.Drawing.Size(177, 6)
        '
        'ServerToolStripMenuItem
        '
        Me.ServerToolStripMenuItem.Name = "ServerToolStripMenuItem"
        Me.ServerToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.ServerToolStripMenuItem.Text = "&Server"
        '
        'AsteriskServersToolStripMenuItem
        '
        Me.AsteriskServersToolStripMenuItem.Name = "AsteriskServersToolStripMenuItem"
        Me.AsteriskServersToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.AsteriskServersToolStripMenuItem.Text = "Server &Asterisk"
        '
        'ServerHylaFaxToolStripMenuItem
        '
        Me.ServerHylaFaxToolStripMenuItem.Name = "ServerHylaFaxToolStripMenuItem"
        Me.ServerHylaFaxToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.ServerHylaFaxToolStripMenuItem.Text = "Server &HylaFax"
        '
        'ConfigurazioneEmailToolStripMenuItem
        '
        Me.ConfigurazioneEmailToolStripMenuItem.Name = "ConfigurazioneEmailToolStripMenuItem"
        Me.ConfigurazioneEmailToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.ConfigurazioneEmailToolStripMenuItem.Text = "&e-mail"
        '
        'ProxyToolStripMenuItem
        '
        Me.ProxyToolStripMenuItem.Name = "ProxyToolStripMenuItem"
        Me.ProxyToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.ProxyToolStripMenuItem.Text = "Prox&y"
        '
        'UploadServerToolStripMenuItem
        '
        Me.UploadServerToolStripMenuItem.Name = "UploadServerToolStripMenuItem"
        Me.UploadServerToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.UploadServerToolStripMenuItem.Text = "&Upload Server"
        '
        'ToolStripMenuItem3
        '
        Me.ToolStripMenuItem3.Name = "ToolStripMenuItem3"
        Me.ToolStripMenuItem3.Size = New System.Drawing.Size(177, 6)
        '
        'LineeDelCentralinoToolStripMenuItem
        '
        Me.LineeDelCentralinoToolStripMenuItem.Name = "LineeDelCentralinoToolStripMenuItem"
        Me.LineeDelCentralinoToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.LineeDelCentralinoToolStripMenuItem.Text = "&Linee del centralino"
        '
        'DispositiviEsterniToolStripMenuItem1
        '
        Me.DispositiviEsterniToolStripMenuItem1.Name = "DispositiviEsterniToolStripMenuItem1"
        Me.DispositiviEsterniToolStripMenuItem1.Size = New System.Drawing.Size(180, 22)
        Me.DispositiviEsterniToolStripMenuItem1.Text = "&Dispositivi Esterni"
        '
        'ToolStripMenuItem4
        '
        Me.ToolStripMenuItem4.Name = "ToolStripMenuItem4"
        Me.ToolStripMenuItem4.Size = New System.Drawing.Size(177, 6)
        '
        'PasswordToolStripMenuItem
        '
        Me.PasswordToolStripMenuItem.Name = "PasswordToolStripMenuItem"
        Me.PasswordToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.PasswordToolStripMenuItem.Text = "Sicurezza"
        '
        'mnuParametri
        '
        Me.mnuParametri.Name = "mnuParametri"
        Me.mnuParametri.Size = New System.Drawing.Size(180, 22)
        Me.mnuParametri.Text = "Paramet&ri"
        '
        'mnuCartelle
        '
        Me.mnuCartelle.Name = "mnuCartelle"
        Me.mnuCartelle.Size = New System.Drawing.Size(180, 22)
        Me.mnuCartelle.Text = "Car&telle"
        '
        'ToolStripMenuItem7
        '
        Me.ToolStripMenuItem7.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.ToolStripMenuItem7.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.VerificaAggiornamentiToolStripMenuItem})
        Me.ToolStripMenuItem7.Name = "ToolStripMenuItem7"
        Me.ToolStripMenuItem7.Size = New System.Drawing.Size(24, 20)
        Me.ToolStripMenuItem7.Text = "&?"
        '
        'VerificaAggiornamentiToolStripMenuItem
        '
        Me.VerificaAggiornamentiToolStripMenuItem.Name = "VerificaAggiornamentiToolStripMenuItem"
        Me.VerificaAggiornamentiToolStripMenuItem.Size = New System.Drawing.Size(195, 22)
        Me.VerificaAggiornamentiToolStripMenuItem.Text = "Verifica A&ggiornamenti"
        '
        'UtilitàToolStripMenuItem
        '
        Me.UtilitàToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FormattazioneSicuraToolStripMenuItem, Me.CancellazioneFileSicuraToolStripMenuItem})
        Me.UtilitàToolStripMenuItem.Name = "UtilitàToolStripMenuItem"
        Me.UtilitàToolStripMenuItem.Size = New System.Drawing.Size(50, 20)
        Me.UtilitàToolStripMenuItem.Text = "&Utilità"
        '
        'FormattazioneSicuraToolStripMenuItem
        '
        Me.FormattazioneSicuraToolStripMenuItem.Name = "FormattazioneSicuraToolStripMenuItem"
        Me.FormattazioneSicuraToolStripMenuItem.Size = New System.Drawing.Size(200, 22)
        Me.FormattazioneSicuraToolStripMenuItem.Text = "&Formattazione sicura"
        '
        'CancellazioneFileSicuraToolStripMenuItem
        '
        Me.CancellazioneFileSicuraToolStripMenuItem.Name = "CancellazioneFileSicuraToolStripMenuItem"
        Me.CancellazioneFileSicuraToolStripMenuItem.Size = New System.Drawing.Size(200, 22)
        Me.CancellazioneFileSicuraToolStripMenuItem.Text = "&Cancellazione file sicura"
        '
        'splashTimer
        '
        Me.splashTimer.Enabled = True
        Me.splashTimer.Interval = 5000
        '
        'activeTelTimer
        '
        Me.activeTelTimer.Interval = 120000
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.tabTelefonate)
        Me.TabControl1.Controls.Add(Me.tabScansioni)
        Me.TabControl1.Controls.Add(Me.tabFAX)
        Me.TabControl1.Controls.Add(Me.tabLog)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.Location = New System.Drawing.Point(0, 63)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(784, 439)
        Me.TabControl1.TabIndex = 19
        '
        'tabTelefonate
        '
        Me.tabTelefonate.Controls.Add(Me.SplitContainer1)
        Me.tabTelefonate.Location = New System.Drawing.Point(4, 22)
        Me.tabTelefonate.Name = "tabTelefonate"
        Me.tabTelefonate.Padding = New System.Windows.Forms.Padding(3)
        Me.tabTelefonate.Size = New System.Drawing.Size(776, 413)
        Me.tabTelefonate.TabIndex = 0
        Me.tabTelefonate.Text = "Telefonate"
        Me.tabTelefonate.UseVisualStyleBackColor = True
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(3, 3)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.inCallsPanel)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.outCallsPanel)
        Me.SplitContainer1.Size = New System.Drawing.Size(770, 407)
        Me.SplitContainer1.SplitterDistance = 350
        Me.SplitContainer1.TabIndex = 18
        '
        'tabScansioni
        '
        Me.tabScansioni.Controls.Add(Me.lstScansioni)
        Me.tabScansioni.Location = New System.Drawing.Point(4, 22)
        Me.tabScansioni.Name = "tabScansioni"
        Me.tabScansioni.Size = New System.Drawing.Size(776, 413)
        Me.tabScansioni.TabIndex = 2
        Me.tabScansioni.Text = "Scansioni"
        Me.tabScansioni.UseVisualStyleBackColor = True
        '
        'lstScansioni
        '
        Me.lstScansioni.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colDataScansione, Me.colNomeFileScansione, Me.colDimensioneFileScansione})
        Me.lstScansioni.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstScansioni.Location = New System.Drawing.Point(0, 0)
        Me.lstScansioni.Name = "lstScansioni"
        Me.lstScansioni.Size = New System.Drawing.Size(776, 413)
        Me.lstScansioni.TabIndex = 16
        Me.lstScansioni.UseCompatibleStateImageBehavior = False
        Me.lstScansioni.View = System.Windows.Forms.View.Details
        '
        'colDataScansione
        '
        Me.colDataScansione.Text = "Data"
        Me.colDataScansione.Width = 113
        '
        'colNomeFileScansione
        '
        Me.colNomeFileScansione.Text = "File"
        Me.colNomeFileScansione.Width = 169
        '
        'colDimensioneFileScansione
        '
        Me.colDimensioneFileScansione.Text = "Dimensione"
        Me.colDimensioneFileScansione.Width = 83
        '
        'tabFAX
        '
        Me.tabFAX.Location = New System.Drawing.Point(4, 22)
        Me.tabFAX.Name = "tabFAX"
        Me.tabFAX.Size = New System.Drawing.Size(776, 413)
        Me.tabFAX.TabIndex = 3
        Me.tabFAX.Text = "Fax"
        Me.tabFAX.UseVisualStyleBackColor = True
        '
        'tabLog
        '
        Me.tabLog.Controls.Add(Me.txtLog)
        Me.tabLog.Location = New System.Drawing.Point(4, 22)
        Me.tabLog.Name = "tabLog"
        Me.tabLog.Padding = New System.Windows.Forms.Padding(3)
        Me.tabLog.Size = New System.Drawing.Size(776, 413)
        Me.tabLog.TabIndex = 1
        Me.tabLog.Text = "Log"
        '
        'timerScansioni
        '
        Me.timerScansioni.Enabled = True
        '
        'timerUploadTel
        '
        Me.timerUploadTel.Enabled = True
        Me.timerUploadTel.Interval = 15000
        '
        'btnInterphone
        '
        Me.btnInterphone.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnInterphone.Image = CType(resources.GetObject("btnInterphone.Image"), System.Drawing.Image)
        Me.btnInterphone.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnInterphone.Name = "btnInterphone"
        Me.btnInterphone.Size = New System.Drawing.Size(36, 36)
        Me.btnInterphone.Text = "Interfono"
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(784, 537)
        Me.ControlBox = False
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "frmMain"
        Me.Text = "DMDApp 1.1"
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.notifyMenu.ResumeLayout(False)
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.inCallsPanel.ResumeLayout(False)
        Me.outCallsPanel.ResumeLayout(False)
        Me.Panel5.ResumeLayout(False)
        Me.Panel5.PerformLayout()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.TabControl1.ResumeLayout(False)
        Me.tabTelefonate.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.tabScansioni.ResumeLayout(False)
        Me.tabLog.ResumeLayout(False)
        Me.tabLog.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents notifyIcon1 As System.Windows.Forms.NotifyIcon
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents btnNewCall As System.Windows.Forms.ToolStripButton
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents inCallList As System.Windows.Forms.ListView
    Friend WithEvents inCallsPanel As System.Windows.Forms.Panel
    Friend WithEvents outCallsPanel As System.Windows.Forms.Panel
    Friend WithEvents outCallList As System.Windows.Forms.ListView
    Friend WithEvents Panel5 As System.Windows.Forms.Panel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents colInDate As System.Windows.Forms.ColumnHeader
    Friend WithEvents colInNumber As System.Windows.Forms.ColumnHeader
    Friend WithEvents colInDisposition As System.Windows.Forms.ColumnHeader
    Friend WithEvents colInLen As System.Windows.Forms.ColumnHeader
    Friend WithEvents colOutDate As System.Windows.Forms.ColumnHeader
    Friend WithEvents colOutNumber As System.Windows.Forms.ColumnHeader
    Friend WithEvents colOutDisposition As System.Windows.Forms.ColumnHeader
    Friend WithEvents colOutLen As System.Windows.Forms.ColumnHeader
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents mnuFile As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EsciToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuSettings As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LineeDelCentralinoToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AsteriskServersToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ServerHylaFaxToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnSendFax As System.Windows.Forms.ToolStripButton
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ComponiUnnumeroToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuSendFax As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents EsciToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ApriToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ConfigurazioneEmailToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DispositiviEsterniToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PasswordToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripButton1 As System.Windows.Forms.ToolStripButton
    Friend WithEvents ServerToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripMenuItem4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuParametri As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnLogOut As System.Windows.Forms.ToolStripButton
    Friend WithEvents mnuCartelle As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents txtLog As System.Windows.Forms.TextBox
    Friend WithEvents splashTimer As System.Windows.Forms.Timer
    Friend WithEvents UploadServerToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblService As System.Windows.Forms.Label
    Friend WithEvents activeTelTimer As System.Windows.Forms.Timer
    Friend WithEvents ProxyToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents tabTelefonate As TabPage
    Friend WithEvents tabLog As TabPage
    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents tabScansioni As TabPage
    Friend WithEvents tabFAX As TabPage
    Friend WithEvents lstScansioni As ListView
    Friend WithEvents colDataScansione As ColumnHeader
    Friend WithEvents colNomeFileScansione As ColumnHeader
    Friend WithEvents colDimensioneFileScansione As ColumnHeader
    Friend WithEvents timerScansioni As Timer
    Friend WithEvents colInMittente As ColumnHeader
    Friend WithEvents colOutDestinatario As ColumnHeader
    Friend WithEvents notifyMenu As ContextMenuStrip
    Friend WithEvents mnuApri As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem5 As ToolStripSeparator
    Friend WithEvents ToolStripMenuItem6 As ToolStripSeparator
    Friend WithEvents EsciToolStripMenuItem2 As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem7 As ToolStripMenuItem
    Friend WithEvents VerificaAggiornamentiToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents timerUploadTel As Timer
    Friend WithEvents UtilitàToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents FormattazioneSicuraToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents FormattazioneSicuraToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents CancellazioneFileSicuraToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CancellazioneFileSicuraToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents mnuGeneralSettings As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem8 As ToolStripSeparator
    Friend WithEvents btnInterphone As ToolStripButton
End Class

Imports DIALTPLib
Imports System.Runtime.InteropServices
Imports DMD.Net.HTTPProxy
Imports DMD
Imports DMD.CallManagers
Imports System.IO
Imports System.Deployment.Application
Imports DMD.Sistema
Imports DMD.CustomerCalls
Imports DIALTPLib.Keyboard

Public Class frmMain


    Public Sub New()
        CheckForIllegalCrossThreadCalls = False

        ' This call is required by the designer.
        InitializeComponent()
        DIALTPLib.InterfonoService.StartServer(DIALTPLib.Machine.GetIPAddress, 10445)
#If DEBUG Then
        AddHandler DIALTPLib.Keyboard.KeyPressed, AddressOf handleKP
        AddHandler DIALTPLib.Keyboard.KeyPressed, AddressOf handleKR
#End If
    End Sub

    Private Sub handleKP(ByVal sender As Object, ByVal e As KeyboardEventArgs)
        Me.Log("Key Pressed: " & e.ToString)
    End Sub

    Private Sub handleKR(ByVal sender As Object, ByVal e As KeyboardEventArgs)
        Me.Log("Key Released: " & e.ToString)

    End Sub


    '    Private Sub handleMouseMove(ByVal p As Point)
    '#If DEBUG Then
    '        Me.Text = "DialTP " & My.Application.Info.Version.ToString & " (" & p.X & ", " & p.Y & ")"
    '#End If
    '    End Sub

    Private Sub handleUserLoggedIn(ByVal sender As Object, ByVal e As DMD.UserLoginEventArgs)
        Me.Log("L'utente " & e.User.UserName & " ha effettuato il login sul server " & DIALTPLib.DialTPApp.CurrentConfig.ServerName)
        Me.btnLogOut.Enabled = True
    End Sub

    Private Sub handleUserLoggedOut(ByVal sender As Object, ByVal e As DMD.UserLogoutEventArgs)
        Me.Log("L'utente " & e.User.UserName & " ha effettuato il logout dal server " & DIALTPLib.DialTPApp.CurrentConfig.ServerName)
        Me.btnLogOut.Enabled = False
    End Sub

    Private Sub frmMain_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        Try
            DIALTPLib.InterfonoService.StopService()
        Catch ex As Exception
            DIALTPLib.Log.LogException(ex)
        End Try
        Try
            ScreenShots.Stop()
        Catch ex As Exception
            DIALTPLib.Log.LogException(ex)
        End Try
        Try
            USBDriveHandler.Stop()
        Catch ex As Exception
            DIALTPLib.Log.LogException(ex)
        End Try
        Try
            DIALTPLib.Log.Terminate()
        Catch ex As Exception
            DMD.Sistema.Events.NotifyUnhandledException(ex)
        End Try
    End Sub

    'Private Sub DeleteTempFiles()
    '    Dim temp As String = System.IO.Path.GetTempPath
    '    Dim files() As CodeProject.FileData = CodeProject.FastDirectoryEnumerator.GetFiles(temp, "*.*", System.IO.SearchOption.TopDirectoryOnly)
    '    For Each f As CodeProject.FileData In files
    '        Try
    '            f.Delete()
    '        Catch ex As Exception

    '        End Try
    '    Next


    'End Sub

    Private Sub frmMain_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        'Dim attivi As CCollection = Me.CheckContattiAttivi
        'If (attivi.Count > 0 AndAlso MsgBox("Ci sono chiamate o visite in sospeso." & vbCrLf & "Confermi di voler uscire ugualmente?", MsgBoxStyle.YesNo, "ATTENZIONE!") = vbNo) Then
        '    e.Cancel = True
        'End If
    End Sub

    Private Sub frmMain_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown

    End Sub

    Private Sub frmMain_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp

    End Sub

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Me.InstallUpdateSyncWithInfo(True)

            Me.notifyIcon1.Icon = Me.Icon
            Me.notifyIcon1.Text = Me.Text
            Me.notifyIcon1.Visible = True

            'AddHandler DMD.Sistema.Users.UserLoggedIn, AddressOf UserLogin_Handler
            'AddHandler DMD.Sistema.Users.UserLoggedOut, AddressOf UserLogout_Handler

            'AddHandler AsteriskServers.IncomingCall, AddressOf AsteriskIncomingCall
            'AddHandler DIALTPLib.Mouse.Mouse_Move, AddressOf handleMouseMove

            Dim m As System.Reflection.Module = System.Reflection.Assembly.GetExecutingAssembly.GetModules()(0)
            ' DIALTPService.StartService(Marshal.GetHINSTANCE(m))

            ' Add any initialization after the InitializeComponent() call.
            AddHandler DIALTPLib.Remote.UserLoggedIn, AddressOf handleUserLoggedIn
            AddHandler DIALTPLib.Remote.UserLoggedOut, AddressOf handleUserLoggedOut

            '#If DEBUG Then
            AddHandler DIALTPLib.ProxyService.NewRequest, AddressOf handleNewProxyRequest
            AddHandler DIALTPLib.ProxyService.ProxyLog, AddressOf handleProxyLog
            AddHandler DIALTPLib.ProxyService.CacheMiss, AddressOf handleProxyCacheMiss
            '#End If

            AddHandler DIALTPLib.AsteriskServers.Connected, AddressOf handleAsteriskConnect
            AddHandler DIALTPLib.AsteriskServers.Disconnected, AddressOf handleAsteriskDisconnect
            AddHandler DIALTPLib.AsteriskServers.ManagerEvent, AddressOf handleAsteriskEvent

            AddHandler DIALTPLib.FolderWatch.FileChange, AddressOf handleFileChange


            Me.Text = My.Application.Info.ProductName & " " & My.Application.Info.Version.ToString

            Me.Log("Avvio dell'applicazione: " & DMD.Sistema.Formats.FormatUserDate(DMD.Sistema.DateUtils.Now))
            Me.Log("Versione: " & My.Application.Info.ProductName & " " & My.Application.Info.Version.ToString)
            Me.Log("Seriale CPU: " & Machine.GetCPUSerialNumber)
            Me.Log("Indirizzo MAC: " & Machine.GetMACAddress)
            Me.Log("Indirizzo IP: " & Machine.GetIPAddress)
            Me.Log("Sistema Operativo: " & Machine.GetOperatoratingSystemVersion & " (" & Machine.GetWindowsProductKey & ")")
            Me.Log("Macchina: " & DIALTPLib.Log.GetMachineName)
            Me.Log("Utente: " & DIALTPLib.Log.GetCurrentUserName)
            ' Me.Log("System Path: " & Sistema.ApplicationContext.SystemDataFolder)

            '  Me.Log("Configurazione rete: " & DIALTPLib.Log.GetNetworkConfig)


            'DIALTPLib.Log.TaskeSystemStatus()
            'Me.CheckContattiAttivi()

        Catch ex As Exception
            Me.Log(ex.Message)
            DMD.Sistema.Events.NotifyUnhandledException(ex)
        End Try

    End Sub

    Private openedInForms As New System.Collections.ArrayList


    Private Sub handleAsteriskDialEvent(ByVal sender As Object, ByVal e As DialEvent)
        Dim servers As DMD.CCollection(Of AsteriskServer) = New DMD.CCollection(Of AsteriskServer)(DIALTPLib.DialTPApp.CurrentConfig.AsteriskServers)
        Dim chiamata As Chiamata

        If UCase(e.SubEvent) = "BEGIN" Then
            For Each server As AsteriskServer In servers
                If e.Channel.StartsWith(server.Channel) Then
                    chiamata = Chiamate.NewOutCall(server, e)
                    If (DialTPApp.IDUltimaTelefonata <> 0) Then
                        chiamata.IDTelefonata = DialTPApp.IDUltimaTelefonata
                        chiamata.Save()
                    End If

                    Me.Log("Inizio chiamata " & e.ConnectedLineNum)
                    Dim item As ListViewItem = Me.outCallList.Items.Add(Now.ToString)
                    item.SubItems().Add(e.ConnectedLineNum)
                    item.SubItems().Add("In corso")
                    item.SubItems().Add("-")
                    item.Tag = e


                ElseIf e.Destination.StartsWith(server.Channel) Then
                    chiamata = Chiamate.NewInCall(server, e)

                    Me.Log("Inizio chiamata " & e.CallerIDNumber)
                    Dim item As ListViewItem = Me.inCallList.Items.Add(Now.ToString)
                    item.SubItems().Add(e.CallerIDNumber)
                    item.SubItems().Add("In corso")
                    item.SubItems().Add("-")
                    item.Tag = e

                    If DialTPApp.CurrentConfig.ShowInCall Then
                        Dim frm As New frmInCall
                        SyncLock Me.openedInForms
                            Debug.Print("APro il form per il numero " & e.CallerIDNumber)
                            openedInForms.Add(frm)
                            frm.Server = e.Manager
                            frm.Channel = e.Channel
                            frm.Numero = e.CallerIDNumber
                        End SyncLock
                    End If

                End If

            Next
        ElseIf UCase(e.SubEvent) = "END" Then
            'For Each server As AsteriskServer In servers
            '    For i As Integer = Me.outCallList.Items.Count - 1 To 0 Step -1
            '        Dim item As ListViewItem = Me.outCallList.Items(i)
            '        Dim e1 As DialEvent = item.Tag
            '        If (e1 IsNot Nothing AndAlso e1.Channel = e.Channel) Then
            '            item.SubItems(2).Text = "Terminata"
            '            item.SubItems(3).Text = (e.Data - e1.Data).TotalSeconds
            '            Me.Log("Fine chiamata " & e1.ConnectedLineNum)
            '            Return
            '        End If
            '    Next
            '    For i As Integer = Me.inCallList.Items.Count - 1 To 0 Step -1
            '        Dim item As ListViewItem = Me.inCallList.Items(i)
            '        Dim e1 As DialEvent = item.Tag
            '        If (e1 IsNot Nothing AndAlso e1.Channel = e.Channel) Then
            '            item.SubItems(2).Text = "Terminata"
            '            item.SubItems(3).Text = (e.Data - e1.Data).TotalSeconds
            '            Me.Log("Fine chiamata " & e1.CallerIDNumber)
            '            Return
            '        End If
            '    Next
            'Next
        End If

    End Sub

    Private Sub handleAsteriskHangUpEvent(sender As Object, e As Events.HangupEvent)
        Dim servers As DMD.CCollection(Of AsteriskServer) = New DMD.CCollection(Of AsteriskServer)(DIALTPLib.DialTPApp.CurrentConfig.AsteriskServers)
        Chiamate.NotifyCallEnded(e)
        For Each server As AsteriskServer In servers
            For i As Integer = Me.outCallList.Items.Count - 1 To 0 Step -1
                Dim item As ListViewItem = Me.outCallList.Items(i)
                Dim e1 As DialEvent = item.Tag
                If (e1 IsNot Nothing AndAlso e1.Channel = e.Channel) Then
                    item.SubItems(2).Text = e.CauseEx
                    item.SubItems(3).Text = DMD.Sistema.Formats.FormatDurata((e.Data - e1.Data).TotalSeconds)
                    Me.Log("Fine chiamata " & e1.ConnectedLineNum)

                    Return
                End If
            Next
            For i As Integer = Me.inCallList.Items.Count - 1 To 0 Step -1
                Dim item As ListViewItem = Me.inCallList.Items(i)
                Dim e1 As DialEvent = item.Tag
                If (e1 IsNot Nothing AndAlso e1.Channel = e.Channel) Then
                    item.SubItems(2).Text = e.CauseEx
                    item.SubItems(3).Text = DMD.Sistema.Formats.FormatDurata((e.Data - e1.Data).TotalSeconds)
                    Me.Log("Fine chiamata " & e1.CallerIDNumber)

                    SyncLock Me.openedInForms
                        For Each frm As frmInCall In Me.openedInForms
                            If frm.Channel = e1.Channel Then
                                Debug.Print("Chiudo il form per il numero " & e1.CallerIDNumber)
                                openedInForms.Remove(frm)
                                frm.Dispose()
                                Exit For
                            End If
                        Next
                    End SyncLock

                    Return
                End If
            Next
        Next
    End Sub

    Private Sub handleAsteriskEvent(sender As Object, e As AsteriskEvent)
        If (TypeOf (e) Is Events.HangupEvent) Then
            Me.handleAsteriskHangUpEvent(sender, e)
        ElseIf (TypeOf (e) Is DialEvent) Then
            Me.handleAsteriskDialEvent(sender, e)
        ElseIf (UCase(e.EventName) = "NEWSTATE") Then
            Chiamate.NotifyChannelState(e)
        Else

            Debug.Print(TypeName(e) & " -> " & e.ToString)
        End If

    End Sub

    Private Sub handleAsteriskDisconnect(ByVal sender As Object, ByVal e As AsteriskEventArgs)
        Me.Log("Server Asterisk Disconnesso" & e.Server.ToString)
    End Sub

    Private Sub handleAsteriskConnect(sender As Object, e As AsteriskEventArgs)
        Me.Log("Server Asterisk Connesso" & e.Server.ToString)
    End Sub

    Private Sub handleNewProxyRequest(ByVal sender As Object, ByVal e As ProxyRequestEventArgs)
        'Me.Log("Proxy Request: " & e.Request.RemoteUri)
    End Sub



    Private Sub frmMain_SizeChanged(sender As Object, e As EventArgs) Handles Me.SizeChanged
        Try
            Dim w As Integer = Me.ClientSize.Width / 2
            Me.inCallsPanel.Width = w
            Me.outCallsPanel.Width = w
        Catch ex As Exception

        End Try

    End Sub

    Private Sub frmMain_VisibleChanged(sender As Object, e As EventArgs) Handles Me.VisibleChanged

    End Sub

    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        Me.Visible = False
    End Sub

    Private WithEvents frmDialer As frmDialer = Nothing

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNewCall.Click, ComponiUnnumeroToolStripMenuItem.Click
        Me.ShowDialer()
    End Sub

    Public Sub ShowDialer()
        If (Me.frmDialer Is Nothing) Then
            Me.frmDialer = New frmDialer
            Me.frmDialer.Show()
        End If
        Me.frmDialer.Visible = True
        Me.frmDialer.BringToFront()
        'frmDialer.Show()
        'Me.Hide()
    End Sub


    Private Sub AsteriskConfigChanged(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub AsteriskServersToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AsteriskServersToolStripMenuItem.Click
        If Me.CheckPassword Then AsteriskServersForm.ShowDialog()
    End Sub

    'Private Sub EsciToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EsciToolStripMenuItem.Click
    '    Me.Close()
    'End Sub

    'Private Sub DispositiviEsterniToolStripMenuItem_Click(sender As Object, e As EventArgs)
    '    DispositiviForm.ShowDialog()
    'End Sub

    'Private Sub LineeDelCentralinoToolStripMenuItem_Click(sender As Object, e As EventArgs)
    '    LineeForm.ShowDialog()
    'End Sub

    Private Sub AsteriskIncomingCall(ByVal sender As Object, ByVal e As CallManagers.IncomingCallEventArgs)
        Dim d As Date = Now
        Dim lvItem As ListViewItem = Me.inCallList.Items.Add(FormatDateTime(d, DateFormat.LongDate))
    End Sub

    Private Sub EsciToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EsciToolStripMenuItem.Click, EsciToolStripMenuItem1.Click
        Me.RequestClose()
    End Sub

    Public Sub RequestClose()
        Try
            If (MsgBox("Sei sicuro di voler uscire", MsgBoxStyle.YesNo) <> MsgBoxResult.Yes) Then Return


            'If MsgBox ("VUoi 
            Me.Log("Chiusura dell'applicazione")
            DMD.Sistema.ApplicationContext.Stop()
        Catch ex As Exception
            'MsgBox(ex.Message)
            'e.Cancel = True
        End Try

        Me.Close()
    End Sub

    Private Sub LineeDelCentralinoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LineeDelCentralinoToolStripMenuItem.Click
        If (Me.CheckPassword) Then LineeForm.ShowDialog()
    End Sub

    Private Sub ServerHylaFaxToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ServerHylaFaxToolStripMenuItem.Click
        If (Me.CheckPassword) Then HylaFaxConfig.ShowDialog()
    End Sub

    Private Sub NotifyIcon1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles notifyIcon1.MouseDoubleClick
        Me.splashTimer.Enabled = False
        Me.Show()
    End Sub

    'Private Sub NotifyIcon1_MouseUp(sender As Object, e As MouseEventArgs) Handles notifyIcon1.MouseUp
    '    If (e.Button = Windows.Forms.MouseButtons.Right) Then
    '        Me.MenuStrip1.Show()
    '    End If
    'End Sub

    Private Sub mnuSendFax_Click(sender As Object, e As EventArgs) Handles mnuSendFax.Click, btnSendFax.Click
        If DIALTPLib.Remote.CurrentUser Is Nothing Then
            If frmLogin.ShowDialog <> Windows.Forms.DialogResult.OK Then Exit Sub
        End If
        'Me.Hide()
        If (SearchPersona.ShowDialog = Windows.Forms.DialogResult.OK) Then
            frmSendFax.SelectedItem = DIALTPLib.Remote.GetPersonaById(SearchPersona.SelectedItem.IDPersona)
            'Me.Hide()
            frmSendFax.Show()
        End If
    End Sub

    Private Sub ApriToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ApriToolStripMenuItem.Click
        Me.Show()
    End Sub


    Private Sub ConfigurazioneEmailToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ConfigurazioneEmailToolStripMenuItem.Click
        If (Me.CheckPassword) Then MailSettings.ShowDialog()
    End Sub

    Private Sub ConfigurazioneEmailToolStripMenuItem_VisibleChanged(sender As Object, e As EventArgs) Handles ConfigurazioneEmailToolStripMenuItem.VisibleChanged

    End Sub

    Private Function CheckPassword()
        Return True
        Try
            If frmCheckPassword.ShowDialog = Windows.Forms.DialogResult.OK Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            MsgBox(ex.Message & vbNewLine & ex.StackTrace, MsgBoxStyle.Critical)
            Sistema.Events.NotifyUnhandledException(ex)
            Return False
        End Try
    End Function

    Private Sub DispositiviEsterniToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles DispositiviEsterniToolStripMenuItem1.Click
        If (Me.CheckPassword) Then DispositiviForm.ShowDialog()
    End Sub

    Private Sub PasswordToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PasswordToolStripMenuItem.Click
        frmAppPassword.ShowDialog()
    End Sub

    Private Sub ServerToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ServerToolStripMenuItem.Click
        If Me.CheckPassword Then FrmConfigServer.ShowDialog() ' FrmServers.ShowDialog()
    End Sub

    Private Sub mnuParametri_Click(sender As Object, e As EventArgs) Handles mnuParametri.Click
        If Me.CheckPassword Then FrmParametri.ShowDialog()
    End Sub

    Private Sub btnLogOut_Click(sender As Object, e As EventArgs) Handles btnLogOut.Click
        Try
            If MsgBox("Desideri disconnetterti dal sito " & DIALTPLib.DialTPApp.CurrentConfig.ServerName, MsgBoxStyle.YesNo) <> MsgBoxResult.Yes Then Exit Sub
            DIALTPLib.Remote.Logout()
        Catch ex As Exception
            DMD.Sistema.Events.NotifyUnhandledException(ex)
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try

    End Sub

    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
        Dim frm As New FrmUploader
        frm.Show(Me)
    End Sub

    Private Sub mnuCartelle_Click(sender As Object, e As EventArgs) Handles mnuCartelle.Click
        If Me.CheckPassword Then FrmFolderMonitor.ShowDialog(Me)
    End Sub

    Public Sub Log(ByVal text As String)
        SyncLock Me.txtLog
            Me.txtLog.Text &= DMD.Sistema.Formats.FormatUserDateTime(Now) & ": " & text & vbNewLine
            Me.txtLog.Select(Me.txtLog.Text.Length, 0)
            Me.txtLog.ScrollToCaret()
            DIALTPLib.Log.LogMessage(text)
            DIALTPLib.Log.InternalLog(text)
        End SyncLock
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles splashTimer.Tick
        Me.splashTimer.Enabled = False
        Me.Hide()
    End Sub

    Private Sub UploadServerToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UploadServerToolStripMenuItem.Click
        If (Me.CheckPassword) Then frmUploadServer.ShowDialog()
    End Sub

#If 0 Then


    Private Sub srvcTimer_Tick(sender As Object, e As EventArgs) Handles activeTelTimer.Tick
        Me.CheckContattiAttivi()
    End Sub

    Public Function CheckContattiAttivi() As CCollection


        Dim attivi As CCollection = Me.GetContattiAttivi
        If (attivi.Count > 0) Then
            Dim text As String = ""
            For Each c As CContattoUtente In attivi
                If (TypeOf (c) Is CTelefonata) Then
                    text &= "Cliente: " & c.NomePersona & ". In corso da " & Formats.FormatDurata((DateUtils.Now - c.Data).TotalSeconds) & vbCrLf
                ElseIf (TypeOf (c) Is CVisita) Then
                    text &= "Cliente: " & c.NomePersona & ". In corso da " & Formats.FormatDurata((DateUtils.Now - c.Data).TotalSeconds) & vbCrLf
                End If
            Next

            DMD.Controls.Toaster.Show("ATTENZIONE! Ci sono chiamate o visite in sospeso..." & vbCrLf & text, 5000)
        End If

        Return attivi
    End Function

    Public Function GetContattiAttivi() As CCollection
        Dim attivi As New CCollection
        For Each linea As LineaEsterna In DialTPApp.CurrentConfig.Linee
            If (linea.Server <> "") Then
                Try
                    Dim url As String = linea.Server & "/widgets/websvcf/dialtp.aspx?_a=GetChiamateNonChiuse"
                    Dim tmp As String = Sistema.RPC.InvokeMethod(url, "u", RPC.str2n(DialTPApp.CurrentConfig.UserName))
                    Dim items As CCollection = XML.Utils.Serializer.Deserialize(tmp)
                    attivi.AddRange(items)
                Catch ex As Exception
                    Debug.Print(ex.StackTrace)
                End Try

            End If
        Next
        Return attivi
    End Function

#End If

    Private Sub lblService_Click(sender As Object, e As EventArgs) Handles lblService.Click
        ' DIALTPService.SendMessageToService("Ciao")

    End Sub

    Private Sub ProxyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ProxyToolStripMenuItem.Click
        If (Me.CheckPassword) Then FrmProxyConfig.ShowDialog()
    End Sub

    Private Sub handleProxyLog(sender As Object, e As HTTPProxyLogEventArgs)
        'Me.Log("Proxy Log: " & e.Message)
    End Sub

    Private Sub handleProxyCacheMiss(ByVal sender As Object, ByVal e As ProxyCacheMissEventArgs)
        'Me.Log("Cache Miss: " & e.Request.RemoteUri.ToString)
    End Sub

    Private Sub SplitContainer1_Panel2_Paint(sender As Object, e As PaintEventArgs) Handles SplitContainer1.Panel2.Paint

    End Sub

    'Dim lastWin As IntPtr = Nothing
    'Private Sub foreWinTimer_Tick(sender As Object, e As EventArgs) Handles foreWinTimer.Tick
    '    'Dim currWin As IntPtr = Window.GetForegroundWindow()
    '    'If (lastWin.ToInt64 = 0 OrElse Not currWin.Equals(lastWin)) Then
    '    '    Dim log As DIALTPLib.Log.LogSession = DIALTPLib.Log.GetCurrSession()
    '    '    log.TakeScreenShot()
    '    '    lastWin = currWin
    '    'End If

    Private m_Scansioni As New System.Collections.ArrayList

    'End Sub
    Private Sub handleFileChange(ByVal sender As Object, ByVal e As System.IO.FileSystemEventArgs)
        If (e.ChangeType = System.IO.WatcherChangeTypes.Created) Then
            SyncLock Me.m_Scansioni
                For Each f As String In Me.m_Scansioni
                    If (f = e.FullPath) Then Return
                Next

                Me.m_Scansioni.Add(e.FullPath)
                '   Me.timerScansioni.Enabled = True
            End SyncLock
        End If
    End Sub

    Private Sub handleScansioneClick(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim frm As Form = sender
        Dim list As System.Collections.ArrayList = frm.Tag
        For Each f As String In list
            Try
                Process.Start("explorer.exe", "/select,""" & f & """ ")
            Catch ex As Exception
                'MsgBox(ex.Message, MsgBoxStyle.Critical)
                Me.Log("Impossibile aprire la directory che contiene: " & f & " -> " & ex.Message)
            End Try
        Next
    End Sub

    Private Sub timerScansioni_Tick(sender As Object, e As EventArgs) Handles timerScansioni.Tick
        Try
            Me.Icon = DialTPApp.DefaultIcon
            Me.notifyIcon1.Icon = Me.Icon
            Me.notifyIcon1.Text = Me.Text
            'Me.notifyIcon1.Visible = True
        Catch ex As Exception
            Sistema.Events.NotifyUnhandledException(ex)
        End Try

        SyncLock Me.openedInForms
            Try
                Dim i As Integer = 0
                Dim frm As frmInCall = Nothing
                While (i < Me.openedInForms.Count())
                    frm = Me.openedInForms(i)
                    If Not frm.IsDisposed Then
                        frm.Left = Screen.PrimaryScreen.WorkingArea.Width - frm.Width - 3
                        frm.Top = Screen.PrimaryScreen.WorkingArea.Height - frm.Height - 3
                        frm.Show() ' = True
                        i += 1
                    Else
                        Me.openedInForms.Remove(frm)
                    End If
                End While
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
            End Try
        End SyncLock

        SyncLock Me.m_Scansioni
            Try
                If (Me.m_Scansioni.Count = 0) Then Return
                Dim message As String = IIf(Me.m_Scansioni.Count > 1, "Nuove scansioni", "Nuova scansione") & vbCrLf

                For Each f As String In Me.m_Scansioni
                    Dim lvItem As ListViewItem = New ListViewItem
                    lvItem.Text = Now.ToString
                    lvItem.Tag = f
                    Me.Log("Scansione ricevuta: " & f)
                    Me.lstScansioni.Items.Add(lvItem)
                    Dim info As New System.IO.FileInfo(f)
                    lvItem.SubItems.Add(info.Name)
                    Try
                        lvItem.SubItems.Add(DMD.Sistema.Formats.FormatBytes(info.Length))
                    Catch ex As Exception
                        lvItem.SubItems.Add("-")
                    End Try
                    info = Nothing

                    message &= f & vbNewLine
                Next

                Dim frm As System.Windows.Forms.Form = DMD.Controls.Toaster.Show(message, 5000)
                frm.Tag = Me.m_Scansioni.Clone
                AddHandler frm.Click, AddressOf handleScansioneClick

                Me.m_Scansioni.Clear()
                'Me.timerScansioni.Enabled = False
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
            End Try
        End SyncLock


    End Sub

    Private Sub lstScansioni_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstScansioni.SelectedIndexChanged

    End Sub

    Private Sub lstScansioni_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles lstScansioni.MouseDoubleClick
        If Me.lstScansioni.SelectedItems.Count = 0 Then Return
        Dim item As ListViewItem = Me.lstScansioni.SelectedItems(0)
        Dim f As String = item.Tag
        Try
            Process.Start("explorer.exe", "/select,""" & f & """ ")
        Catch ex As Exception
            'MsgBox(ex.Message, MsgBoxStyle.Critical)
            Me.Log("Impossibile aprire la directory che contiene: " & f & " -> " & ex.Message)
        End Try

    End Sub

    Private Sub frmDialer_FormClosed(sender As Object, e As FormClosedEventArgs) Handles frmDialer.FormClosed
        Me.frmDialer = Nothing
    End Sub

    Private Sub mnuOpenAR_Click(sender As Object, e As EventArgs)
        Try
            Process.Start("explorer.exe", "https://areariservata.DMD.net")
        Catch ex As Exception
            Sistema.Events.NotifyUnhandledException(ex)
        End Try
    End Sub

    Private Sub mnuApri_Click(sender As Object, e As EventArgs) Handles mnuApri.Click
        Me.splashTimer.Enabled = False
        Me.Show()
    End Sub



    Private ReadOnly Property AC As ApplicationContext
        Get
            Return CType(DMD.Sistema.ApplicationContext, ApplicationContext)
        End Get
    End Property



    Private Sub EsciToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles EsciToolStripMenuItem2.Click
        Me.RequestClose()
    End Sub

    Private Sub frmMain_Activated(sender As Object, e As EventArgs) Handles Me.Activated

    End Sub

    Private Sub VerificaAggiornamentiToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles VerificaAggiornamentiToolStripMenuItem.Click
        Me.InstallUpdateSyncWithInfo(False)
    End Sub

    Private Sub InstallUpdateSyncWithInfo(ByVal forceUpdate As Boolean)
#If DEBUG Then
        Try
#End If
            If DialTPApp.CheckForUpdates Then
                Dim doUpdate As Boolean = True
                doUpdate = forceUpdate OrElse DialTPApp.CheckForRequiredUpdates OrElse (MessageBox.Show("Aggiornamenti disponibili." & vbCrLf & "Desideri aggiornare l'applicazione adesso?", "Aggiornamenti...", MessageBoxButtons.OKCancel) = DialogResult.OK)
                If (doUpdate) Then
                    DialTPApp.InstallUpdateSyncWithInfo(forceUpdate)
                End If
            End If
#If DEBUG Then
        Catch ex As Exception
            DIALTPLib.Log.LogException(ex)
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
#End If
    End Sub

    Private Sub activeTelTimer_Tick(sender As Object, e As EventArgs) Handles activeTelTimer.Tick

    End Sub

    Private Sub FormattazioneSicuraToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FormattazioneSicuraToolStripMenuItem.Click
        Me.ShowFormatta
    End Sub

    Private Sub FormattazioneSicuraToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles FormattazioneSicuraToolStripMenuItem1.Click
        Me.ShowFormatta
    End Sub

    Public Sub ShowFormatta()
        frmFormatta.Show()
    End Sub

    Private Sub CancellazioneFileSicuraToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CancellazioneFileSicuraToolStripMenuItem.Click
        Me.CancellaSicura
    End Sub

    Private Sub CancellazioneFileSicuraToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles CancellazioneFileSicuraToolStripMenuItem1.Click
        Me.CancellaSicura
    End Sub

    Public Sub CancellaSicura(ByVal fileName As String)
        Dim buffer() As Byte = New Byte(2048 - 1) {}
        Dim info As New System.IO.FileInfo(fileName)
        Using stream As New System.IO.FileStream(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.None)
            Dim len As Long = info.Length
            While (len > 0)
                For i As Integer = 0 To buffer.Length - 1
                    buffer(i) = CByte(Math.Floor(Rnd(1) * 256))
                Next
                If (len > 2048) Then
                    stream.Write(buffer, 0, 2048)
                    len -= 2048
                Else
                    stream.Write(buffer, 0, len)
                    len = 0
                End If
            End While
        End Using
    End Sub

    Public Sub CancellaSicura()
        Try
            Using ofn As New System.Windows.Forms.OpenFileDialog
                ofn.Title = "Cancellazione definitiva di un file"
                If ofn.ShowDialog = DialogResult.OK Then
                    Me.CancellaSicura(ofn.FileName)
                    System.IO.File.Delete(ofn.FileName)
                    MsgBox("Il file é stato eliminato in maniera definitiva")
                End If
            End Using

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Errore nella cancellazione sicura del file")
        End Try

    End Sub

    Private Sub mnuGeneralSettings_Click(sender As Object, e As EventArgs) Handles mnuGeneralSettings.Click
        frmSettings.Show
    End Sub

    Private Sub mnuAudioConfig_Click(sender As Object, e As EventArgs)
        frmAudioConfig.Visible = True
        frmAudioConfig.BringToFront()
    End Sub

    Private Sub timerUploadTel_Tick(sender As Object, e As EventArgs) Handles timerUploadTel.Tick
        Dim cursor As ChiamateCursor = Nothing
        Try
            Dim cnt As Integer = 25
            cursor = New ChiamateCursor
            cursor.Flags.Value = 0
            cursor.PageSize = cnt
            While Not cursor.EOF AndAlso cnt > 0
                cnt -= 1
                Dim chiamata As Chiamata = cursor.Item

#If DEBUG Then
                Dim serverName As String = "http://localhost:33016/"
#Else
                Dim serverName As String = DialTPApp.CurrentConfig.UploadServer
#End If
                RPC.InvokeMethod(serverName & "/widgets/websvcf/dialtp.aspx?_a=regcall", "c", RPC.str2n(XML.Utils.Serializer.Serialize(chiamata)))

                chiamata.Flags = 1
                chiamata.Save()
                cursor.MoveNext()
            End While

        Catch ex As Exception
            DIALTPLib.Log.LogException(ex)
        Finally
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
        End Try
    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)
        'frmAudioConfig.Start()
    End Sub

    Private Sub btnInterphone_Click(sender As Object, e As EventArgs) Handles btnInterphone.Click
        frmPhone.Show()
    End Sub

    Private Sub frmMain_CursorChanged(sender As Object, e As EventArgs) Handles Me.CursorChanged
        Me.InstallUpdateSyncWithInfo(True)
    End Sub
End Class
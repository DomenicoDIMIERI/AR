Imports System.Deployment.Application

Public Class frmMain

    Public Sub New()
        CheckForIllegalCrossThreadCalls = False

        ' La chiamata è richiesta dalla finestra di progettazione.
        InitializeComponent()

        ' Aggiungere le eventuali istruzioni di inizializzazione dopo la chiamata a InitializeComponent().

    End Sub

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            Me.InstallUpdateSyncWithInfo(True)

            Scansioni.LoadConfig()
            Me.RefillList()
            AddHandler Scansioni.NuovaScansione, AddressOf nuovaScansioneHandler
        Catch ex As Exception
            DMD.Sistema.ApplicationContext.Log(ex.Message & vbNewLine & ex.StackTrace)
        End Try
    End Sub

    Private Sub nuovaScansioneHandler(ByVal sender As Object, ByVal e As Scansioni.ScansioneEventArgs)
        Try
            Dim s As Scansione = e.S
            AddHandler s.StatoChanged, AddressOf handleStatoScansione
            Dim lvItem As ListViewItem = Me.lstScansioni.Items.Add(s.Percorso)
            lvItem.Tag = s
            lvItem.SubItems.Add(s.C.NomeUtente)
            lvItem.SubItems.Add([Enum].GetName(GetType(StatoScansione), s.Stato))
        Catch ex As Exception
            DMD.Sistema.ApplicationContext.Log(ex.Message & vbNewLine & ex.StackTrace)
        End Try
    End Sub

    Private Sub handleStatoScansione(ByVal sender As Object, ByVal e As Scansioni.ScansioneEventArgs)
        Try
            Dim s As Scansione = e.S
            For Each l As ListViewItem In Me.lstScansioni.Items
                If s Is l.Tag Then
                    l.SubItems(2).Text = [Enum].GetName(GetType(StatoScansione), s.Stato)
                    Exit For
                End If
            Next
            If (s.Stato = StatoScansione.Uploaded) Then
                RemoveHandler s.StatoChanged, AddressOf handleStatoScansione
            End If
        Catch ex As Exception
            DMD.Sistema.ApplicationContext.Log(ex.Message & vbNewLine & ex.StackTrace)
        End Try
    End Sub

    Public Sub RefillList()
        Try
            Dim items() As ConfigItem = Scansioni.GetConfiguration
            Me.lstItems.Items.Clear()
            For Each item As ConfigItem In items
                Dim lvItem As ListViewItem = Me.lstItems.Items.Add(item.NomeUtente)
                lvItem.SubItems.Add(item.Percorso)
                lvItem.SubItems.Add(item.UploadService)
                lvItem.Tag = item
            Next
        Catch ex As Exception
            DMD.Sistema.ApplicationContext.Log(ex.Message & vbNewLine & ex.StackTrace)
        End Try
    End Sub

    Public Sub SaveConfig()
        Try
            Dim items() As ConfigItem = {}
            If (Me.lstItems.Items.Count > 0) Then
                ReDim items(Me.lstItems.Items.Count - 1)
                For i As Integer = 0 To Me.lstItems.Items.Count - 1
                    items(i) = Me.lstItems.Items(i).Tag
                Next
            End If
            Scansioni.SetConfiguration(items)
            Scansioni.PersistConfig()
        Catch ex As Exception
            DMD.Sistema.ApplicationContext.Log(ex.Message & vbNewLine & ex.StackTrace)
        End Try
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Try
            If MsgBox("Confermi l'eliminazione della riga selezionata?", MsgBoxStyle.YesNo) <> MsgBoxResult.Yes Then Return

            Me.lstItems.Items.RemoveAt(Me.lstItems.SelectedIndices(0))
            Me.SaveConfig()
        Catch ex As Exception
            DMD.Sistema.ApplicationContext.Log(ex.Message & vbNewLine & ex.StackTrace)
        End Try
    End Sub

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        Try
            Dim frm As New frmConfigItemEditor
            If (frm.ShowDialog = DialogResult.OK) Then
                Dim item As ConfigItem = frm.Item
                frm.Dispose()

                Dim lvItem As ListViewItem = Me.lstItems.Items.Add(item.NomeUtente)
                lvItem.SubItems.Add(item.Percorso)
                lvItem.SubItems.Add(item.UploadService)
                lvItem.Tag = item

                Me.SaveConfig()
            End If
        Catch ex As Exception
            DMD.Sistema.ApplicationContext.Log(ex.Message & vbNewLine & ex.StackTrace)
        End Try
    End Sub

    Public Function GetSelectedItem() As ConfigItem
        Try
            If (Me.lstItems.SelectedIndices.Count <> 1) Then Return Nothing
            Return Me.lstItems.SelectedItems(0).Tag
        Catch ex As Exception
            DMD.Sistema.ApplicationContext.Log(ex.Message & vbNewLine & ex.StackTrace)
            Return Nothing
        End Try
    End Function

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        Try
            Dim frm As New frmConfigItemEditor
            frm.Item = Me.GetSelectedItem
            If (frm.ShowDialog = DialogResult.OK) Then
                Dim item As ConfigItem = frm.Item
                frm.Dispose()
                Me.SaveConfig()
            End If
        Catch ex As Exception
            DMD.Sistema.ApplicationContext.Log(ex.Message & vbNewLine & ex.StackTrace)
        End Try
    End Sub

    Private Sub lstItems_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstItems.SelectedIndexChanged
        Try
            Me.btnDelete.Enabled = Me.lstItems.SelectedIndices.Count = 1
            Me.btnEdit.Enabled = Me.btnDelete.Enabled
        Catch ex As Exception
            DMD.Sistema.ApplicationContext.Log(ex.Message & vbNewLine & ex.StackTrace)
        End Try
    End Sub

    Private Sub lstScansioni_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles lstScansioni.MouseDoubleClick
        Try
            If Me.lstScansioni.SelectedIndices.Count <> 1 Then Return
            If Not MsgBox("Vuoi ricaricare il file selezionato?", MsgBoxStyle.YesNo) <> MsgBoxResult.Yes Then Return
            Try
                Dim s As Scansione = Me.lstScansioni.SelectedItems(0).Tag
                s.UploadFile(s.C)
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        Catch ex As Exception
            DMD.Sistema.ApplicationContext.Log(ex.Message & vbNewLine & ex.StackTrace)
        End Try
    End Sub

    Private Sub InstallUpdateSyncWithInfo(ByVal forceUpdate As Boolean)
        Dim info As UpdateCheckInfo = Nothing

        If (ApplicationDeployment.IsNetworkDeployed) Then
            Dim AD As ApplicationDeployment = ApplicationDeployment.CurrentDeployment

            Try
                info = AD.CheckForDetailedUpdate()
            Catch dde As DeploymentDownloadException
                MsgBox("Non riesco a scaricare la nuova verifione in questo momento. " & vbCrLf & "Verifica la connessione di rete o prova più tardi." & vbCrLf & "Error: " + dde.Message, "Aggiornamenti...", vbYesNo)
                Return
            Catch ioe As InvalidOperationException
                MsgBox("Questa applicazione non può essere aggiornata." & vbCrLf & "Error: " & ioe.Message, "Aggiornamenti...", vbCritical)
                Return
            End Try

            If (info.UpdateAvailable) Then
                Dim doUpdate As Boolean = True
                If (Not forceUpdate) Then
                    If (Not info.IsUpdateRequired) Then
                        Dim dr As DialogResult = MessageBox.Show("Aggiornamenti disponibili." & vbCrLf & "Desideri aggiornare l'applicazione adesso?", "Aggiornamenti...", MessageBoxButtons.OKCancel)
                        If (Not System.Windows.Forms.DialogResult.OK = dr) Then
                            doUpdate = False
                        End If
                    Else
                        ' Display a message that the app MUST reboot. Display the minimum required version.
                        MessageBox.Show("E' necessario effettuare l'aggiornamento di questa versione (" & info.MinimumRequiredVersion.ToString() & ")" & vbCrLf,
                           "Aggiornamenti...", MessageBoxButtons.OK,
                            MessageBoxIcon.Information)
                    End If
                End If


                If (doUpdate) Then
                    Try
                        AD.Update()
                        If (Not forceUpdate) Then MessageBox.Show("L'applicazione sarà riavviata.")
                        Application.Restart()
                    Catch dde As DeploymentDownloadException
                        MessageBox.Show("Impossibile installare l'aggiornamento. " & vbCrLf & "Controlla la connessione di rete o prova più tardi.")
                        Return
                    End Try
                End If
            End If
        End If
    End Sub
End Class

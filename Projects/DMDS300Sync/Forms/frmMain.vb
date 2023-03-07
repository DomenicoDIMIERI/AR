Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Office
Imports DMD.S300
Imports System.Deployment.Application
Imports System.Windows.Forms
Imports DMD.My

Public Class frmMain
    Private m_IsLoad As Boolean

    Public Sub New()
        System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = False
        ' La chiamata è richiesta dalla finestra di progettazione.
        InitializeComponent()

        ' Aggiungere le eventuali istruzioni di inizializzazione dopo la chiamata a InitializeComponent().

    End Sub

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            Me.InstallUpdateSyncWithInfo(True)
            Me.m_IsLoad = False

            DMD.Sistema.FileSystem.CreateRecursiveFolder(DMD.Sistema.FileSystem.GetFolderName(Me.GetLogFileName))



            'Me.RegisterDevice()

            'If autoSync Then
            '    Dim col As CCollection(Of MarcaturaIngressoUscita) = Me.ScaricaNuoveMarcature
            '    ANVIZS300Worker.UploadToServer(col)
            'End If


        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
        Me.m_IsLoad = True
    End Sub





    Private Sub Log(ByVal message As String)
        Me.txtLog.Text = Now.ToString & " - " & message & vbNewLine & Me.txtLog.Text
        System.IO.File.WriteAllText(Me.GetLogFileName, Me.txtLog.Text)
    End Sub




    Private Function GetLogFileName() As String
        Return System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DMDS300Sync\runtime.log")
    End Function

    Private Sub LoadLog()
        Try
            Me.txtLog.Text = System.IO.File.ReadAllText(Me.GetLogFileName)
            Me.txtLog.SelectionStart = Len(Me.txtLog.Text) - 1
            Me.txtLog.SelectionLength = 1
        Catch ex As Exception

        End Try
    End Sub








    Private Sub ChiudiToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ChiudiToolStripMenuItem.Click
        Me.Close()
    End Sub



    Private Sub txtLog_TextChanged(sender As Object, e As EventArgs) Handles txtLog.TextChanged

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

    Private Sub ImpostazioniToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ImpostazioniToolStripMenuItem.Click

    End Sub

    Private Sub ListaDispositiviToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ListaDispositiviToolStripMenuItem.Click
        frmDevicesList.ShowDialog()
    End Sub

    Private Sub GeneraliToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GeneraliToolStripMenuItem.Click
        If frmConfig.ShowDialog = DialogResult.OK Then

        End If
    End Sub

    Private Sub ToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem1.Click

    End Sub

    Private Sub VerificaAggiornamentiToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles VerificaAggiornamentiToolStripMenuItem.Click
        Me.InstallUpdateSyncWithInfo(True)
    End Sub

    Private Sub frmMain_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        Sistema.ApplicationContext.Stop()

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Me.LoadLog()
    End Sub
End Class
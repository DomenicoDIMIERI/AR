Public Class frmMain

    Private m_LogFile As String = ""

    Public Sub New()
        CheckForIllegalCrossThreadCalls = False

        ' La chiamata è richiesta dalla finestra di progettazione.
        InitializeComponent()

        ' Aggiungere le eventuali istruzioni di inizializzazione dopo la chiamata a InitializeComponent().

    End Sub

    Private Sub ChiudiToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ChiudiToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub ImpostazioniToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ImpostazioniToolStripMenuItem.Click
        Dim frm As New POPConfiguration
        frm.ShowDialog(Me)
    End Sub

    Private Sub RiceviToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RiceviToolStripMenuItem.Click
        Me.ControllaPosta
    End Sub

    Public Sub Log(ByVal message As String)
        'SyncLock Me.txtLog
        Me.txtLog.Text = DMD.Sistema.Formats.FormatUserDateTime(Now) & " -> " & message & vbNewLine & Me.txtLog.Text
            Try
                System.IO.File.WriteAllText(Me.GetLogFile, Me.txtLog.Text)
            Catch ex As Exception
                Me.txtLog.Text = "Impossibile caricare il contenuto del file " & Me.GetLogFile & vbCrLf & ex.Message & vbCrLf & ex.StackTrace & vbNewLine & Me.txtLog.Text
            End Try
        'End SyncLock
    End Sub



    Public Sub ControllaPosta()
        Me.Timer1.Enabled = False
        Me.Log(DMD.Sistema.Strings.NChars(30, "-"))
        Me.Log("Inizio controllo della posta elettronica")
#If Not DEBUG Then
        Try
#End If
        DMD.Sistema.EMailer.Config.POP3Enabled = True 'My.Settings.POP3CheckEvery > 0
            'DMD.Sistema.EMailer.Config.CheckInterval = My.Settings.POP3CheckEvery
            DMD.Sistema.EMailer.Config.POPServer = My.Settings.POP3Server
            DMD.Sistema.EMailer.Config.POPPort = My.Settings.POP3Port
            DMD.Sistema.EMailer.Config.POPUserName = My.Settings.POP3UserName
            DMD.Sistema.EMailer.Config.POPPassword = My.Settings.POP3Password
            DMD.Sistema.EMailer.Config.POPUseSSL = My.Settings.POP3SSL
            DMD.Sistema.EMailer.Config.RemoveFromServerAfterNDays = 30

            Sistema.EMailer.CheckMails()
#If Not DEBUG Then
        Catch ex As Exception
            Me.Log(ex.Message & vbNewLine & ex.StackTrace)
        End Try
#End If

            Me.Log(DMD.Sistema.Strings.NChars(30, "-"))

        Me.Timer1.Interval = 300000
        Me.Timer1.Enabled = True
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Me.ControllaPosta()
    End Sub

    Private Sub txtLog_TextChanged(sender As Object, e As EventArgs) Handles txtLog.TextChanged

    End Sub

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles Me.Load
        ' SyncLock Me.txtLog
        Me.m_LogFile = System.IO.Path.Combine(Sistema.ApplicationContext.UserDataFolder, "messages.log")
        Try
            Me.txtLog.Text = System.IO.File.ReadAllText(Me.GetLogFile)
            Me.Log("Cartella email: " & Sistema.ApplicationContext.UserDataFolder)
        Catch ex As Exception
            Me.txtLog.Text = "Impossibile caricare il contenuto del file " & Me.GetLogFile & vbCrLf & ex.Message & vbCrLf & ex.StackTrace
        End Try
        Me.Timer1.Interval = 1000
        Me.Timer1.Enabled = True

        ' Me.ControllaPosta()
        'End SyncLock
    End Sub

    Private Function GetLogFile() As String
        Return Me.m_LogFile
    End Function
End Class

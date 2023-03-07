Imports System.Drawing
Imports System.Drawing.Printing
Imports System.Management
Imports DMD
Imports DMD.Anagrafica
Imports DMD.Sistema
Imports FAXCOMEXLib
Imports System.ComponentModel

Public Class frmMain

    Public Class FaxInfo
        Public fromAddress As String
        Public job As FaxJob
        Public msgID As String
        Public targetNumber As String
        Public status As String = "queued"
        Public sendTime As Date
        Public completeTime As Date

        Public Overrides Function ToString() As String
            Return targetNumber & " - " & fromAddress & " - " & Me.msgID & " (" & status & ")"
        End Function
    End Class

    Public Sub New()
        System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = False

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public ReadOnly Property AC As MyApplicationContext
        Get
            Return DirectCast(DMD.Sistema.ApplicationContext, MyApplicationContext)
        End Get
    End Property

    Private Sub mnuExit_Click(sender As Object, e As EventArgs) Handles mnuExit.Click
        Me.Close()
    End Sub


    Private Sub lstQueuedItems_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstQueuedItems.SelectedIndexChanged
        Try
            Me.btnCancelPendingJob.Enabled = Me.lstQueuedItems.SelectedIndex >= 0
            Dim item As FaxInfo = Nothing
            If (Me.lstQueuedItems.SelectedIndex >= 0) Then
                item = DirectCast(Me.lstQueuedItems.Items(Me.lstQueuedItems.SelectedIndex), FaxInfo)
            End If
            Me.showInfo(item)
        Catch ex As Exception
            Sistema.Events.NotifyUnhandledException(ex)
        End Try
    End Sub

    Private Sub frmMain_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If (e.CloseReason <> CloseReason.WindowsShutDown) Then
            e.Cancel = MsgBox("Confermi la chiusura", MsgBoxStyle.YesNo) <> MsgBoxResult.Yes
        End If
    End Sub

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            
            Dim driver As DMD.Sistema.BaseFaxDriver = Me.GetDriver
            SyncLock Me.listLock
                For Each job As FaxJob In driver.OutQueue
                    Dim item As New FaxInfo
                    item.job = job
                    item.status = job.JobStatusMessage
                    If (job.Options.SendTime.HasValue) Then
                        item.sendTime = Formats.ToDate(job.Options.SendTime)
                    Else
                        item.sendTime = Now
                    End If
                    item.targetNumber = job.Options.TargetNumber
                    item.msgID = job.Options.GetValueString("FSEFXSVR-e-mail-msgid")
                    item.fromAddress = job.Options.GetValueString("FSEFXSVR-e-mail-sender")
                    Me.lstQueuedItems.Items.Add(item)
                Next
            End SyncLock

            AddHandler DMD.Sistema.EMailer.MessageReceived, AddressOf handleMessageReceived
            AddHandler DMD.Sistema.FaxService.FaxDelivered, AddressOf handleFaxDelivered
            AddHandler DMD.Sistema.FaxService.FaxFailed, AddressOf handleFaxSentError
        Catch ex As Exception
            Sistema.Events.NotifyUnhandledException(ex)
        End Try
    End Sub

    Private Sub lstSent_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstSent.SelectedIndexChanged
        Try
            Me.btnDelSent.Enabled = Me.lstSent.SelectedIndex >= 0
            Dim item As FaxInfo = Nothing
            If (Me.lstSent.SelectedIndex >= 0) Then
                item = DirectCast(Me.lstSent.Items(Me.lstSent.SelectedIndex), FaxInfo)
            End If
            Me.showInfo(item)
        Catch ex As Exception
            Sistema.Events.NotifyUnhandledException(ex)
        End Try
    End Sub

    Private Sub POPToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles POPToolStripMenuItem.Click
        POPConfiguration.ShowDialog(Me)

        Me.Timer1.Interval = FaxSvrSettings.POP3CheckEvery * 60 * 1000
    End Sub

    Private Sub SMTPToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SMTPToolStripMenuItem.Click
        SMTPSettings.ShowDialog(Me)
    End Sub

    Private folderLock As New Object
    Private listLock As New Object

    Private Sub handleMessageReceived(ByVal sender As Object, ByVal e As DMD.Sistema.MailMessageReceivedEventArgs)
        Try
            Dim m As DMD.Net.Mail.MailMessageEx = e.Message
            Dim subject As String = m.Subject

            Me.Log("Messaggio ricevuto da " & m.From.ToString & ", Oggetto: " & subject & ", Allegati: " & m.Attachments.Count)

            ' If (Strings.Left(subject, Len("Fax attached (ID:")) = "Fax attached (ID:") Then
            Const CMDSEND As String = "SENDTO:"
            Const CMDMSGID As String = "MSGID:"
            Dim targetNumbers As New System.Collections.ArrayList
            Dim msgID As String = ""
            Dim nibbles() As String = Split(subject, "|")
            For i As Integer = 0 To Arrays.Len(nibbles) - 1
                Dim item As String = Trim(nibbles(i))
                If (Strings.Left(item, Len(CMDSEND)) = CMDSEND) Then
                    Dim numbers() As String = Split(Mid(item, Len(CMDSEND) + 1), ",")
                    For j As Integer = 0 To Arrays.Len(numbers) - 1
                        numbers(j) = Formats.ParsePhoneNumber(numbers(j))
                        If (numbers(j) <> "") Then targetNumbers.Add(numbers(j))
                    Next
                ElseIf (Strings.Left(item, Len(CMDMSGID)) = CMDMSGID) Then
                    msgID = Trim(Mid(item, Len(CMDMSGID) + 1))
                End If
            Next

            Dim a As DMD.Net.Mail.AttachmentEx = Nothing
            For i As Integer = 0 To m.Attachments.Count - 1
                If UCase(System.IO.Path.GetExtension(m.Attachments(i).FileName)) = ".PDF" Then
                    a = m.Attachments(i)
                End If
            Next

            If (msgID <> "" AndAlso targetNumbers.Count > 0 AndAlso a IsNot Nothing) Then
                Me.Log("Ho identificato un fax da inviare a " & targetNumbers.ToString & ", MsgID: " & msgID)

                Dim dataFolder As String = System.IO.Path.Combine(Sistema.ApplicationContext.UserDataFolder, "Data")
                DMD.Sistema.FileSystem.CreateRecursiveFolder(dataFolder)

                Dim fName As String = System.IO.Path.GetFileName(a.FileName)
                Dim ext As String = System.IO.Path.GetExtension(fName)
                If (fName = "") Then fName = "attachment"
                Dim i As Integer
                SyncLock folderLock
                    If System.IO.File.Exists(System.IO.Path.Combine(dataFolder, fName & "." & ext)) Then
                        While System.IO.File.Exists(System.IO.Path.Combine(dataFolder, fName & "_" & i & "." & ext))
                            i += 1
                        End While
                        fName = fName & "_" & i
                    End If
                End SyncLock
                fName = System.IO.Path.Combine(dataFolder, fName & "." & ext)
                a.SaveToFile(fName)

                Dim driver As DMD.Drivers.HylaFaxDriver = Me.GetDriver
                If Not driver.IsConnected Then driver.Connect()



                For Each number As String In targetNumbers
                    Me.Log("Invio il fax MsgID: " & msgID & " a " & number)
                    Dim opt As FaxDriverOptions = DirectCast(driver.GetDefaultOptions, FaxDriverOptions)
                    opt.NotifyEmailAddress = FaxSvrSettings.POP3UserName ' "tecnico@DMD.net"
                    opt.SetValueString("FSEFXSVR-e-mail-sender", m.From.Address)
                    opt.SetValueString("FSEFXSVR-e-mail-msgid", msgID)
                    SyncLock Me.listLock
                        Dim job As FaxJob = DMD.Sistema.FaxService.Send(driver, number, fName, opt)
                        Dim item As New FaxInfo
                        item.job = job
                        item.status = job.JobStatusMessage
                        If (job.Options.SendTime.HasValue) Then
                            item.sendTime = Formats.ToDate(job.Options.SendTime)
                        Else
                            item.sendTime = Now
                        End If
                        item.targetNumber = job.Options.TargetNumber
                        item.msgID = job.Options.GetValueString("FSEFXSVR-e-mail-msgid")
                        item.fromAddress = job.Options.GetValueString("FSEFXSVR-e-mail-sender")
                        Me.lstQueuedItems.Items.Add(item)
                    End SyncLock

                    'Dim msgID As String = job.Options.GetValueString("FSEFXSVR-e-mail-msgid")
                    'Dim toAddress As String = job.Options.GetValueString("FSEFXSVR-e-mail-sender")
                    Dim cc As String = ""
                    If Me.AC.IsDebug Then cc = "tecnico@DMD.net"
                    Dim m1 As DMD.Net.Mail.MailMessageEx = Sistema.EMailer.PrepareMessage(FaxSvrSettings.SMTPUserName, m.From.Address, cc, "", "MSGID:" & msgID & "|STATUS:QUEUED|DLVTIME:" & Formats.FormatISODate(Now), "", "", True)
                    Sistema.EMailer.SendMessageAsync(m1, True)

                Next

                e.DeleteOnServer = False
            End If
            'End If
        Catch ex As Exception
            Sistema.Events.NotifyUnhandledException(ex)
        End Try

    End Sub

    Private Sub handleFaxDelivered(ByVal sender As Object, ByVal e As FaxDeliverEventArgs)
        Try
            Dim job As FaxJob = e.Job

            Dim msgID As String = job.Options.GetValueString("FSEFXSVR-e-mail-msgid")
            Dim toAddress As String = job.Options.GetValueString("FSEFXSVR-e-mail-sender")

            Me.Log("Fax MsgID: " & msgID & " inviato a " & job.Options.TargetNumber & " da " & toAddress & " inviato correttamente")

            Dim subject As String = "MSGID:" & msgID & "|STATUS:OK|DLVTIME:" & Formats.FormatISODate(Now)
            Dim body As String = ""
            Dim cc As String = ""
            If Me.AC.IsDebug Then cc = "tecnico@DMD.net"
            Dim m As DMD.Net.Mail.MailMessageEx = Sistema.EMailer.PrepareMessage(FaxSvrSettings.SMTPUserName, toAddress, cc, "", subject, body, "", True)
            Sistema.EMailer.SendMessageAsync(m, True)

            SyncLock Me.listLock
                Dim i As Integer = 0
                While (i < Me.lstQueuedItems.Items.Count)
                    Dim item As FaxInfo = DirectCast(Me.lstQueuedItems.Items(i), FaxInfo)
                    If item.job.JobID = job.JobID Then
                        Me.lstQueuedItems.Items.RemoveAt(i)
                        item.status = "OK"
                        item.completeTime = Now
                        Me.lstSent.Items.Add(item)
                    Else
                        i += 1
                    End If
                End While
            End SyncLock
        Catch ex As Exception
            Sistema.Events.NotifyUnhandledException(ex)
        End Try

    End Sub

    Private Sub handleFaxSentError(ByVal sender As Object, ByVal e As FaxJobEventArgs)
        Try
            Dim job As FaxJob = e.Job

            Dim msgID As String = job.Options.GetValueString("FSEFXSVR-e-mail-msgid")
            Dim toAddress As String = job.Options.GetValueString("FSEFXSVR-e-mail-sender")

            Me.Log("Fax MsgID: " & msgID & " inviato a " & job.Options.TargetNumber & " da " & toAddress & " fallito: " & job.JobStatusMessage)

            Dim subject As String = "MSGID:" & msgID & "|STATUS:ERROR|ERRORMSG:" & job.JobStatusMessage & "|DLVTIME:" & Formats.FormatISODate(Now)
            Dim body As String = ""
            Dim cc As String = ""
            If Me.AC.IsDebug Then cc = "tecnico@DMD.net"
            Dim m As DMD.Net.Mail.MailMessageEx = Sistema.EMailer.PrepareMessage(FaxSvrSettings.SMTPUserName, toAddress, cc, "", subject, body, "", True)
            Sistema.EMailer.SendMessageAsync(m, True)


            SyncLock Me.listLock
                Dim i As Integer = 0
                While (i < Me.lstQueuedItems.Items.Count)
                    Dim item As FaxInfo = DirectCast(Me.lstQueuedItems.Items(i), FaxInfo)
                    If item.job.JobID = job.JobID Then
                        Me.lstQueuedItems.Items.RemoveAt(i)
                        item.status = "ERROR: " & e.Job.JobStatusMessage
                        item.completeTime = Now
                        Me.lstSent.Items.Add(item)
                    Else
                        i += 1
                    End If
                End While
            End SyncLock
        Catch ex As Exception
            Sistema.Events.NotifyUnhandledException(ex)
        End Try
    End Sub

    Private Function GetDriver() As DMD.Drivers.HylaFaxDriver
        For Each driver As BaseFaxDriver In Sistema.FaxService.GetInstalledDrivers
            If (TypeOf (driver) Is DMD.Drivers.HylaFaxDriver) Then
                Return DirectCast(driver, DMD.Drivers.HylaFaxDriver)
            End If
        Next
        Return Nothing
    End Function

    Private Sub HylaFaxToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HylaFaxToolStripMenuItem.Click
        HylaFaxConfig.ShowDialog(Me)
    End Sub

    Private Sub mnuControlla_Click(sender As Object, e As EventArgs) Handles mnuControlla.Click
        If Me.BackgroundWorker1.IsBusy Then Return
        Me.BackgroundWorker1.RunWorkerAsync()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If Me.BackgroundWorker1.IsBusy Then Return
        Me.BackgroundWorker1.RunWorkerAsync()
    End Sub

     
    Private Sub showInfo(ByVal value As FaxInfo)
        Me.txtInfo.Text = ""
        If value Is Nothing Then Exit Sub

        Dim text As String = ""
        text &= "Target Number: " & value.targetNumber & vbNewLine
        text &= "Send Time: " & Formats.FormatUserDateTime(value.sendTime) & vbNewLine
        text &= "MsgID: " & value.msgID & vbNewLine
        text &= "Sender: " & value.fromAddress & vbNewLine
        text &= "Job Status: " & value.status

        Me.txtInfo.Text = text
    End Sub

    Private Sub btnCancelPendingJob_Click(sender As Object, e As EventArgs) Handles btnCancelPendingJob.Click
        Try
            Dim item As FaxInfo = DirectCast(Me.lstQueuedItems.Items(Me.lstQueuedItems.SelectedIndex), FaxInfo)
            item.job.Cancel()
            Me.lstQueuedItems.Items.RemoveAt(Me.lstQueuedItems.SelectedIndex)
        Catch ex As Exception
            Sistema.Events.NotifyUnhandledException(ex)
        End Try
    End Sub

    Private Sub BackgroundWorker1_DoWork(sender As Object, e As DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Try
            Sistema.EMailer.CheckMails() '.CheckMailsAsync()
        Catch ex As Exception
            Sistema.Events.NotifyUnhandledException(ex)
        End Try
    End Sub

    Public Sub Log(ByVal text As String)
        Me.txtLog.Text = Me.txtLog.Text & Formats.FormatUserDateTime(Calendar.Now) & " - " & text & vbNewLine
    End Sub
End Class

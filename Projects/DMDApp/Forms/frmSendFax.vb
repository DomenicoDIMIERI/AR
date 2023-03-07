Imports DMD.Anagrafica
Imports System.Net
Imports DMD.Sistema
Imports DMD.CustomerCalls
Imports DMD
Imports DIALTPLib

Public Class frmSendFax

    Private m_SuppressCloseWorning As Boolean = False
    Private m_FileName As String
    Private m_SelectedItem As CPersona = Nothing

    Public Property SelectedItem As CPersona
        Get
            Return Me.m_SelectedItem
        End Get
        Set(value As CPersona)
            Me.m_SelectedItem = value
            Me.cboNumero.Items.Clear()
            Me.lblDestinatario.Text = value.Nominativo
            For Each c As CContatto In DIALTPLib.Remote.GetRecapitiPersonaById(value)
                Me.cboNumero.Items.Add(c)
            Next
        End Set
    End Property

    Private Sub EsciToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EsciToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub frmSendFax_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        e.Cancel = Me.m_SuppressCloseWorning OrElse MsgBox("Confermi l'uscita", MsgBoxStyle.YesNo) <> MsgBoxResult.Yes
    End Sub



    Private Sub Open()
        'Dim pdf As New Spire.Pdf.PdfDocument
        'Dim y As Integer = 5
        'pdf.LoadFromFile(Me.m_FileName)
        'For i As Integer = 0 To System.Math.Min(pdf.Pages.Count - 1, 50)
        '    Dim ret As System.Drawing.Image = pdf.SaveAsImage(i)
        '    Dim ctr As New PictureBox
        '    ctr.Name = "Pic" & i
        '    ctr.SizeMode = PictureBoxSizeMode.AutoSize
        '    ctr.Image = ret
        '    ctr.Left = 0
        '    ctr.Top = y
        '    Me.Panel1.Controls.Add(ctr)
        'Next
        'pdf.Dispose()
        'Me.btnSend.Enabled = True
    End Sub

    Private Sub btnUpload_Click(sender As Object, e As EventArgs) Handles btnUpload.Click, CaricaDocumentoPDFToolStripMenuItem.Click
        Try
            Dim ofd As New OpenFileDialog
            ofd.Title = "Seleziona un documento PDF"
            ofd.Filter = "Documento PDF|*.PDF"
            If ofd.ShowDialog = Windows.Forms.DialogResult.OK Then
                Me.m_FileName = ofd.FileName
            End If
            ofd.Dispose()

            Me.Open()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub btnSend_Click(sender As Object, e As EventArgs) Handles btnSend.Click
#If Not Debug Then
        Try
#End If
        Me.ValidateData()
        Me.Send()
#If Not Debug Then
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
#End If
    End Sub

    Public Sub ValidateData()
        If Me.GetDriver Is Nothing Then Throw New ArgumentNullException("Non è installato alcun driver HylaFax")
        If Me.cboNumero.Text = "" Then Throw New ArgumentNullException("Il numero del destinatario non è valido")
        If Me.m_FileName = "" Then Throw New ArgumentNullException("Non hai selezionato alcun file PDF da inviare")
    End Sub

    Private Function GetDriver() As DMD.Drivers.HylaFaxDriver
        For Each driver As DMD.Sistema.BaseFaxDriver In DMD.Sistema.FaxService.GetInstalledDrivers
            If TypeOf (driver) Is DMD.Drivers.HylaFaxDriver Then Return driver
        Next
        Return Nothing
    End Function

    Public Sub Send()
        Dim options As DMD.Sistema.FaxDriverOptions = New DMD.Sistema.FaxDriverOptions
        'options.NotifyEmailAddress = Me.txtNotifyTo.Text
        'options.NotifyWhenDone = Me.chkNotify.Checked
        Me.btnSend.Enabled = False
        Me.btnUpload.Enabled = False
        Me.CaricaDocumentoPDFToolStripMenuItem.Enabled = False
        Me.InviaToolStripMenuItem.Enabled = False

        Dim frm As New FrmUploader
        AddHandler frm.UploadCompleted, AddressOf handleUploadCompleted
        AddHandler frm.UploadCancelled, AddressOf handleUploadCancelled
        frm.SelectedFile = Me.m_FileName
        Me.btnUpload.Enabled = False
        frm.Show(Me)
        frm.Upload()
    End Sub

    Private m_SendingFax As FaxJob

    Private Sub handleUploadCompleted(ByVal sender As Object, ByVal e As UploadEventArgs)
#If Not Debug Then
        Try
#End If
        Dim frm As FrmUploader = sender
        Dim drv As DMD.Sistema.BaseFaxDriver = Nothing
        drv = DMD.Sistema.FaxService.GetDriver("HLFXDRV")
        Dim opt As FaxDriverOptions = drv.GetDefaultOptions
        opt.NotifyEmailAddress = "tecnico@DMD.net"

        Dim faxjob As FaxJob = DMD.Sistema.FaxService.Send(drv, Me.txtCentralino.Text & Me.cboNumero.Text, Me.m_FileName, opt)
        If (faxjob.JobID = "") Then
            Throw New Exception("Si è verificato un errore non specificato")
        End If

        Dim a As New CAttachment(Me.SelectedItem)
        a.URL = e.UploadedToUrl
        a.Stato = DMD.Databases.ObjectStatus.OBJECT_VALID
        a = DIALTPLib.Remote.SaveObject(a)

        Dim fax As New FaxDocument
        fax.Persona = Me.m_SelectedItem
        fax.Attachment = a
        fax.Stato = DMD.Databases.ObjectStatus.OBJECT_VALID
        fax.Data = Now
        fax.NumeroOIndirizzo = Me.cboNumero.Text
        fax.MessageID = faxjob.JobID
        fax = DIALTPLib.Remote.SaveObject(fax)

        Dim timer As New System.Timers.Timer
        timer.Interval = 500
        AddHandler timer.Elapsed, AddressOf handleCloseTimer
        timer.Enabled = True

#If Not Debug Then
        Catch ex As Exception
            DMD.Sistema.Events.NotifyUnhandledException(ex)
            Me.btnSend.Enabled = True
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
#End If

    End Sub

    Private Sub handleCloseTimer(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim timer As System.Timers.Timer = sender
        timer.Dispose()
        Me.m_SuppressCloseWorning = True
        Me.Close()
    End Sub

    Private Sub handleUploadCancelled(ByVal sender As Object, ByVal e As UploadEventArgs)
        Dim frm As FrmUploader = sender
        Dim a As New CAttachment(Me.SelectedItem)
        'a.URL = 

        Dim job As DMD.Sistema.FaxJob = DMD.Sistema.FaxService.Send(Me.GetDriver, Me.cboNumero.Text, Me.m_FileName) ', options)
        If (job.JobID <> "") Then
            MsgBox("Il documento è stato inviato al server di invio con ID: " & job.JobID)
            Me.m_SuppressCloseWorning = True
            Me.Close()
        Else
            MsgBox("Si è verificato un problema di comunicazione con il server di invio", MsgBoxStyle.Critical)
        End If
    End Sub


    Private Sub frmSendFax_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            For Each linea As LineaEsterna In DialTPApp.CurrentConfig.Linee
                Me.txtCentralino.Items.Add(linea)
            Next

            Me.txtCentralino.Text = DIALTPLib.Settings.LastFaxPrefix
        Catch ex As Exception
            DMD.Sistema.Events.NotifyUnhandledException(ex)
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub
End Class
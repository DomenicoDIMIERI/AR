Imports System.Net
Imports System.ComponentModel
Imports DMD.WebSite

Public Class UploadEventArgs
    Inherits System.EventArgs

    Public SourceFileName As String
    Public UploadedToUrl As String

    Public Sub New()
    End Sub

End Class

Public Class FrmUploader
    Private mUploading As Boolean = False

  

    Public Event UploadCompleted(ByVal sender As Object, ByVal e As UploadEventArgs)
    Public Event UploadCancelled(ByVal sender As Object, ByVal e As UploadEventArgs)
    Public Event UploadError(ByVal sender As Object, ByVal e As UploadEventArgs)

    Private m_Key As String = ""

    Private Sub btnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click
        Dim ofd As New OpenFileDialog
        If ofd.ShowDialog = Windows.Forms.DialogResult.OK Then
            Me.txtFile.Text = ofd.FileName
        End If
        ofd.Dispose()
    End Sub

    <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public Property SelectedFile As String
        Get
            Return Me.txtFile.Text
        End Get
        Set(value As String)
            Me.txtFile.Text = value
        End Set
    End Property

    Public Sub Upload()
        frmMain.Log("Inizio l'upload del file " & Me.txtFile.Text & " verso il server " & DIALTPLib.DialTPApp.CurrentConfig.UploadServer)
        Me.m_Key = DIALTPLib.Remote.Upload(Me.txtFile.Text)
        Me.btnBrowse.Enabled = False
        Me.btnOk.Enabled = False
        Me.progress.Visible = True
    End Sub


    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        Try
            If DIALTPLib.Remote.CurrentUser Is Nothing Then
                If frmLogin.ShowDialog <> Windows.Forms.DialogResult.OK Then Exit Sub
            End If
            Me.Upload()
        Catch ex As Exception
            DMD.Sistema.Events.NotifyUnhandledException(ex)
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub FrmUploader_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        RemoveHandler DIALTPLib.Remote.UploadProgress, AddressOf handleProgress
        RemoveHandler DIALTPLib.Remote.UploadCompleted, AddressOf handleComplete
    End Sub

    Private Sub FrmUploader_Load(sender As Object, e As EventArgs) Handles Me.Load
        AddHandler DIALTPLib.Remote.UploadProgress, AddressOf handleProgress
        AddHandler DIALTPLib.Remote.UploadCompleted, AddressOf handleComplete
    End Sub

    Private Sub handleProgress(ByVal sender As Object, ByVal e As UploadProgressChangedEventArgs)
        Me.progress.Minimum = 0
        Me.progress.Maximum = 100
        Me.progress.Value = e.ProgressPercentage
        Me.Text = DMD.Sistema.FileSystem.GetFileName(Me.txtFile.Text) & " (" & e.ProgressPercentage & "%)"
    End Sub

    Private Sub handleComplete(ByVal sender As Object, ByVal e As UploadFileCompletedEventArgs)
        Me.progress.Minimum = 0
        Me.progress.Maximum = 100
        Me.progress.Value = 100
        Me.Text = DMD.Sistema.FileSystem.GetFileName(Me.txtFile.Text) & " (Completato)"
        frmMain.Log("l'upload del file " & Me.txtFile.Text & " verso il server " & DIALTPLib.DialTPApp.CurrentConfig.UploadServer & " è stato completato")
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
        Global.System.Threading.Thread.Sleep(1000)
        Dim e1 As New UploadEventArgs
        Dim u As CUploadedFile = DIALTPLib.Remote.GetUploadedFileByKey(Me.m_Key)
        If (e.Cancelled) Then
            e1.SourceFileName = Me.txtFile.Text
            e1.UploadedToUrl = u.TargetURL
            RaiseEvent UploadCancelled(Me, e1)
        Else
            e1.SourceFileName = Me.txtFile.Text
            e1.UploadedToUrl = u.TargetURL
            RaiseEvent UploadCompleted(Me, e1)
        End If
    End Sub

    Private Sub txtFile_TextChanged(sender As Object, e As EventArgs) Handles txtFile.TextChanged
        Me.btnOk.Enabled = Me.txtFile.Text <> ""
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
        'frmMain.Log("l'upload del file " & Me.txtFile.Text & " verso il server " & My.Settings.ServerName & " è stato completato")
    End Sub
End Class
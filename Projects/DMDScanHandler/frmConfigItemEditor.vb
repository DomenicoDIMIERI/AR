Public Class frmConfigItemEditor
    Private m_Item As ConfigItem = Nothing

    Public Property Item As ConfigItem
        Get
            Return Me.m_Item
        End Get
        Set(value As ConfigItem)
            Me.m_Item = value
            Me.RefillData
        End Set
    End Property

    Public Sub RefillData()
        Try
            If (Me.m_Item Is Nothing) Then Me.m_Item = New ConfigItem
            Me.txtLocalPath.Text = Me.m_Item.Percorso
            Me.txtUserName.Text = Me.m_Item.NomeUtente
            Me.txtServiceURL.Text = Me.m_Item.UploadService
        Catch ex As Exception
            DMD.Sistema.ApplicationContext.Log(ex.Message & vbNewLine & ex.StackTrace)
        End Try
    End Sub

    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        Try
            Me.m_Item.Percorso = Trim(Me.txtLocalPath.Text)
            Me.m_Item.NomeUtente = Trim(Me.txtUserName.Text)
            Me.m_Item.UploadService = Trim(Me.txtServiceURL.Text)
            Me.DialogResult = DialogResult.OK
            Me.Close()
        Catch ex As Exception
            DMD.Sistema.ApplicationContext.Log(ex.Message & vbNewLine & ex.StackTrace)
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Try
            Me.DialogResult = DialogResult.Cancel
            Me.Close()
        Catch ex As Exception
            DMD.Sistema.ApplicationContext.Log(ex.Message & vbNewLine & ex.StackTrace)
        End Try
    End Sub

    Private Sub frmConfigItemEditor_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            Me.RefillData()
        Catch ex As Exception
            DMD.Sistema.ApplicationContext.Log(ex.Message & vbNewLine & ex.StackTrace)
        End Try
    End Sub

    Private Sub btnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click
        Try
            Dim b As New FolderBrowserDialog
            b.Description = "Seleziona la cartella da monitorare"
            b.SelectedPath = Me.txtLocalPath.Text
            b.ShowNewFolderButton = True
            If b.ShowDialog = DialogResult.OK Then
                Me.txtLocalPath.Text = b.SelectedPath
            End If
            b.Dispose()
        Catch ex As Exception
            DMD.Sistema.ApplicationContext.Log(ex.Message & vbNewLine & ex.StackTrace)
        End Try
    End Sub
End Class
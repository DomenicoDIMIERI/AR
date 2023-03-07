Public Class FrmFolderMonitor

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub lstIncluse_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstIncluse.SelectedIndexChanged
        Me.btnDeleteInclusa.Enabled = Me.lstIncluse.SelectedIndex >= 0
    End Sub

    Private Sub lstEscluse_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstEscluse.SelectedIndexChanged
        Me.btnDeleteEsclusa.Enabled = Me.lstEscluse.SelectedIndex >= 0
    End Sub

    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        Try
            DIALTPLib.FolderWatch.StopWatching()

            Dim list As New System.Collections.ArrayList
            For Each item As String In Me.lstIncluse.Items
                list.Add(item)
            Next
            DIALTPLib.FolderWatch.SetIncludedFolders(list)

            list = New System.Collections.ArrayList
            For Each item As String In Me.lstEscluse.Items
                list.Add(item)
            Next
            DIALTPLib.FolderWatch.SetExcludedFolders(list)

            DIALTPLib.FolderWatch.StartWatching()

            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub FrmFolderMonitor_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            Me.lstIncluse.Items.Clear()
            For Each Str As String In DIALTPLib.FolderWatch.GetIncludedFolders
                Me.lstIncluse.Items.Add(Str)
            Next

            Me.lstEscluse.Items.Clear()
            For Each Str As String In DIALTPLib.FolderWatch.GetExcludedFolders
                Me.lstEscluse.Items.Add(Str)
            Next
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnAddInclusa_Click(sender As Object, e As EventArgs) Handles btnAddInclusa.Click
        frmChoseFolder.SelectedPath = ""
        If frmChoseFolder.ShowDialog = Windows.Forms.DialogResult.OK Then
            Dim path As String = frmChoseFolder.SelectedPath
            For Each str As String In Me.lstIncluse.Items
                If Strings.StrComp(str, path, CompareMethod.Text) = 0 Then Exit Sub
            Next
            Me.lstIncluse.Items.Add(path)
        End If
    End Sub

    Private Sub btnAddEsclusa_Click(sender As Object, e As EventArgs) Handles btnAddEsclusa.Click
        frmChoseFolder.SelectedPath = ""
        If frmChoseFolder.ShowDialog = Windows.Forms.DialogResult.OK Then
            Dim path As String = frmChoseFolder.SelectedPath
            For Each str As String In Me.lstEscluse.Items
                If Strings.StrComp(str, path, CompareMethod.Text) = 0 Then Exit Sub
            Next
            Me.lstEscluse.Items.Add(path)
        End If
    End Sub

    Private Sub btnDeleteInclusa_Click(sender As Object, e As EventArgs) Handles btnDeleteInclusa.Click
        Me.lstIncluse.Items.RemoveAt(Me.lstIncluse.SelectedIndex)
    End Sub

    Private Sub btnDeleteEsclusa_Click(sender As Object, e As EventArgs) Handles btnDeleteEsclusa.Click
        Me.lstEscluse.Items.RemoveAt(Me.lstEscluse.SelectedIndex)
    End Sub
End Class
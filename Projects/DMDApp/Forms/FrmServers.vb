Public Class FrmServers

    Private mloading As Boolean

    Private Sub FrmServers_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.mloading = True
        Try
            Me.lstServers.Items.Clear()
            Dim items() As String = Split(DIALTPLib.Settings.ServersList, ";")
            If (items IsNot Nothing AndAlso items.Length > 0) Then
                Array.Sort(items)
                For Each item As String In items
                    If (Trim(item) <> "") Then Me.lstServers.Items.Add(item)
                Next
            End If
        Catch ex As Exception
            DMD.Sistema.Events.NotifyUnhandledException(ex)
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
        Me.mloading = False

        Me.lstServers.SelectedIndex = Me.lstServers.Items.IndexOf(DIALTPLib.DialTPApp.CurrentConfig.ServerName)
    End Sub

    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        Try
            Dim tmp As New System.Text.StringBuilder
            For Each item As String In Me.lstServers.Items
                If (Trim(item) <> "") Then
                    If (tmp.Length > 0) Then tmp.Append(";")
                    tmp.Append(item)
                End If
            Next
            DIALTPLib.Settings.ServersList = tmp.ToString
            My.Settings.Save()
            Me.Close()
        Catch ex As Exception
            DMD.Sistema.Events.NotifyUnhandledException(ex)
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub lstServers_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstServers.SelectedIndexChanged
        If Me.mloading Then Exit Sub
        If Me.lstServers.SelectedIndex >= 0 Then
            Me.txtURL.Text = Me.lstServers.Items(Me.lstServers.SelectedIndex)
            Me.grpConfig.Enabled = True
        Else
            Me.txtURL.Text = ""
            Me.grpConfig.Enabled = False
        End If
        Me.btnDelete.Enabled = Me.lstServers.SelectedIndex >= 0
    End Sub



    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Me.lstServers.SelectedIndex = Me.lstServers.Items.Add("http://servername")
    End Sub

    Private Sub txtURL_TextChanged(sender As Object, e As EventArgs) Handles txtURL.TextChanged
        If Me.lstServers.SelectedIndex >= 0 Then Me.lstServers.Items(Me.lstServers.SelectedIndex) = Me.txtURL.Text
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Me.lstServers.Items.RemoveAt(Me.lstServers.SelectedIndex)
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
End Class
Imports DMD
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports System.ComponentModel

Public Class SearchPersona

    '<Debugger(False)> _
    Public SelectedItem As CPersonaInfo = Nothing

    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        Try
            Me.Confirm()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub Confirm()
        Me.SelectedItem = Me.lstResult.SelectedItems(0).Tag
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnFind_Click(sender As Object, e As EventArgs) Handles btnFind.Click
        Me.Find()
    End Sub

    Private Sub Find()
        Me.FindPersone(Me.txtFind.Text)
    End Sub

    Private Sub FindPersone(ByVal text As String)
        Me.lstResult.View = View.Details
        Me.lstResult.Items.Clear()

        Try
            Dim items As CCollection(Of CPersonaInfo) = DIALTPLib.Remote.FindPersone(text)
            For Each p As CPersonaInfo In items
                Dim lvItem As ListViewItem = Me.lstResult.Items.Add(p.NomePersona, p.NomePersona & vbNewLine & p.Notes, "default")
                lvItem.Tag = p
            Next
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub txtFind_KeyUp(sender As Object, e As KeyEventArgs) Handles txtFind.KeyUp
        If e.KeyCode = 13 Then Me.Find()
    End Sub

    Private Sub lstResult_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles lstResult.MouseDoubleClick
        If Me.lstResult.SelectedItems.Count > 0 Then
            Me.Confirm()
        End If
    End Sub

    Private Sub lstResult_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstResult.SelectedIndexChanged
        Me.btnOk.Enabled = Me.lstResult.SelectedIndices.Count > 0
    End Sub
End Class
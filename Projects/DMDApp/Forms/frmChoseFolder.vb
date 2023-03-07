Imports System.ComponentModel

Public Class frmChoseFolder

    Private Sub btnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click
        Dim b As New FolderBrowserDialog
        If b.ShowDialog = Windows.Forms.DialogResult.OK Then
            Dim path As String = b.SelectedPath
            Me.txtCartella.Text = path
        End If
        b.Dispose()
    End Sub

    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub txtCartella_TextChanged(sender As Object, e As EventArgs) Handles txtCartella.TextChanged
        Me.btnOk.Enabled = Trim(Me.txtCartella.Text) <> ""
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public Property SelectedPath As String
        Get
            Return Me.txtCartella.Text
        End Get
        Set(value As String)
            Me.txtCartella.Text = Trim(value)
        End Set
    End Property
End Class
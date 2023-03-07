Public Class frmTextViewer

    Public Sub SetText(ByVal text As String)
        Me.TextBox1.Text = text
    End Sub

    Private Sub EsciToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EsciToolStripMenuItem.Click
        Me.Close()
    End Sub
End Class
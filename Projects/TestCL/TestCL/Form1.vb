Public Class Form1

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub Form1_Click(sender As Object, e As EventArgs) Handles Me.Click

        'Dim ctr As New vbAccelerator.Components.ListBarControl.ListBar
        'Me.Controls.Add(ctr)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim frm As New CQSClient.MainForm()

        frm.Show()

    End Sub
End Class

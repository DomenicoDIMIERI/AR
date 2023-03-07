Public Class frmFormatta

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)

        Dim tmp As String = "Mon, 18 Jun 2018 06:40:39 +0000"

        Debug.Print(Date.Parse(tmp))
        Try
            Dim drives() As System.IO.DriveInfo = System.IO.DriveInfo.GetDrives()
            For Each drive As System.IO.DriveInfo In drives
                If drive.DriveType = IO.DriveType.Removable AndAlso drive.IsReady Then
                    Me.cboDrive.Items.Add(drive)
                End If
            Next
            'Me.btnCancella.Enabled = Me.cboDrive.Items.Count > 0
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub cboDrive_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboDrive.SelectedIndexChanged
        Try
            Dim sel As System.IO.DriveInfo = Me.cboDrive.SelectedItem
            Me.btnCancella.Enabled = (sel IsNot Nothing)
        Catch ex As Exception
            Me.btnCancella.Enabled = False
        End Try
    End Sub

    Private Sub btnCancella_Click(sender As Object, e As EventArgs) Handles btnCancella.Click
        Try
            Dim sel As System.IO.DriveInfo = Me.cboDrive.SelectedItem
            sel = New System.IO.DriveInfo(sel.Name)
            If (MsgBox("Il disco " & sel.Name & " (" & CLng(sel.TotalSize \ (1024 * 1024 * 1024L)) & " GB) verrà formattato e tutti i dati in esso contenuto saranno irrecuperabili.", MsgBoxStyle.YesNo, "ATTENZIONE! PERDITA DI DATI") <> MsgBoxResult.Yes) Then Return
            Dim name As String = sel.Name
            If (name.EndsWith("\")) Then name = name.Substring(0, name.Length - 1)
            If (name.EndsWith(":")) Then name = name.Substring(0, name.Length - 1)
            Dim str As String = "format.com " & name & ": /P:2"
            Dim ret As Integer = Shell(str)
            Debug.Print(ret)
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub
End Class

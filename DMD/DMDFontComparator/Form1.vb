Imports System.Drawing.Text

Public Class Form1
    Private installedFontCollection As InstalledFontCollection = Nothing

    'Private Sub txtText_TextChanged(sender As Object, e As EventArgs) Handles txtText.TextChanged, cboSize.SelectedIndexChanged
    '    Me.Refill()
    'End Sub

    Private Sub ClearList()
        While (Me.listPanel.Controls.Count > 0)
            Dim item As FontItem = Me.listPanel.Controls(0)
            Me.listPanel.Controls.RemoveAt(0)
            item.Dispose()
        End While
    End Sub


    Private Class ComparerFF
        Implements IComparer

        Public Function Compare(x As Object, y As Object) As Integer Implements IComparer.Compare
            Dim a As FontFamily = x
            Dim b As FontFamily = y
            Return -Strings.StrComp(a.Name, b.Name, CompareMethod.Text)
        End Function
    End Class

    Public Sub Refill()
        Me.listPanel.SuspendLayout()

        If (Me.installedFontCollection Is Nothing) Then
            Me.installedFontCollection = New InstalledFontCollection()

        End If

        Me.ClearList()

        Dim arr As FontFamily() = installedFontCollection.Families
        Array.Sort(arr, New ComparerFF)

        ' Get the array of FontFamily objects.
        For Each ff As FontFamily In arr
            Dim item As New FontItem
            item.SampleFont = ff.Name
            item.SampleSize = CSng(Me.cboSize.Text)
            item.SampleText = Me.txtText.Text
            item.Dock = DockStyle.Top
            Me.listPanel.Controls.Add(item)
        Next

        Me.listPanel.ResumeLayout()
    End Sub




    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.Refill()
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        Me.Refill()
    End Sub
End Class

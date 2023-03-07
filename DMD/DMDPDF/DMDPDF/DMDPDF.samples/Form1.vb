Imports DMDPDF.samples.org.dmdpdf.samples.cli

Public Class Form1

    Private Class Wrapper
        Public t As System.Type

        Public Sub New(ByVal t As System.Type)
            Me.t = t
        End Sub

        Public Overrides Function GetHashCode() As Integer
            Return Me.t.GetHashCode
        End Function

        Public Overrides Function ToString() As String
            Dim name As String = Me.t.Name
            Dim p As Integer = Strings.InStrRev(name, ".")
            If (p > 0) Then name = Mid(name, p + 1)
            Return name
        End Function

    End Class
    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)
        Dim t As System.Type = Me.GetType
        Dim a As System.Reflection.Assembly = t.Assembly
        Dim types() As System.Type = {}
        Try
            types = a.GetTypes()
        Catch ex As Exception

        End Try

        For Each t In types
            If (t IsNot Nothing AndAlso Not t.IsAbstract AndAlso GetType(Sample).IsAssignableFrom(t)) Then
                Me.cboSample.Items.Add(New Wrapper(t))
            End If
        Next
    End Sub

    Private Sub btnRun_Click(sender As Object, e As EventArgs) Handles btnRun.Click
        Dim w As Wrapper = Me.cboSample.Items(Me.cboSample.SelectedIndex)
        Dim t As System.Type = w.t
        Dim c As System.Reflection.ConstructorInfo = t.GetConstructor({})
        Dim o As Sample = c.Invoke({})
        o.Run()
    End Sub
End Class

Public Class IPAddressControl

    Public Event ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs)

    Public Property Value As String
        Get
            Return Me.GetIPString
        End Get
        Set(value As String)
            Me.SetIPString(value)
        End Set
    End Property


    Public Function GetIPString() As String
        Return Me.I3.Text & "." & Me.I2.Text & "." & Me.I1.Text & "." & Me.I0.Text
    End Function

    Public Sub SetIPString(ByVal value As String)
        Dim i() As String = Split(value, ".")
        Me.I3.Text = i(0)
        Me.I2.Text = i(1)
        Me.I1.Text = i(2)
        Me.I0.Text = i(3)
    End Sub

    Public Sub SetIP(ByVal i() As Byte)
        Me.I3.Text = i(0)
        Me.I2.Text = i(1)
        Me.I1.Text = i(2)
        Me.I0.Text = i(3)
    End Sub

    Public Function GetIP() As Byte()
        Dim i(3) As Byte
        i(0) = Me.I3.Text
        i(1) = Me.I2.Text
        i(2) = Me.I1.Text
        i(3) = Me.I0.Text
        Return i
    End Function


    Private Sub I3_TextChanged(sender As Object, e As EventArgs) Handles I3.TextChanged, I2.TextChanged, I1.TextChanged, I0.TextChanged
        Dim txt As TextBox = sender

        Try
            Dim value As Integer = CInt(txt.Text)
            If (value > 255) Then value = 255
            If (value < 0) Then value = 0
            txt.Text = value
            txt.Tag = value

            Me.OnValueChanged(New System.EventArgs)
        Catch ex As Exception
            Try
                txt.Text = CInt(txt.Tag)
            Catch ex1 As Exception

            End Try
        End Try
    End Sub

    Private Sub I3_KeyDown(sender As Object, e As KeyEventArgs) Handles I3.KeyDown, I2.KeyDown, I1.KeyDown, I0.KeyDown
        If (e.KeyCode < Asc("0")) OrElse e.KeyCode > Asc("9") Then
            e.Handled = True
            e.SuppressKeyPress = True
        End If
    End Sub

    Protected Overridable Sub OnValueChanged(ByVal e As System.EventArgs)
        RaiseEvent ValueChanged(Me, e)
    End Sub

End Class

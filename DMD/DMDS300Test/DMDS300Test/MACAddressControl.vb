Public Class MACAddressControl

    Public Event ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs)

    Public Property Value As String
        Get
            Return Me.GetMACString
        End Get
        Set(value As String)
            Me.SetMACString(value)
        End Set
    End Property


    Public Function GetMACString() As String
        Return Me.I5.Text & ":" & Me.I4.Text & ":" & Me.I3.Text & ":" & Me.I2.Text & ":" & Me.I1.Text & ":" & Me.I0.Text
    End Function

    Public Sub SetMACString(ByVal value As String)
        Dim i() As String = Split(Trim(UCase(value)), ":")
        Me.I5.Text = i(0)
        Me.I4.Text = i(1)
        Me.I3.Text = i(2)
        Me.I2.Text = i(3)
        Me.I1.Text = i(4)
        Me.I0.Text = i(5)
    End Sub

    Public Sub SetMAC(ByVal i() As Byte)
        Me.I5.Text = (Hex(i(0))).PadLeft(2, "0")
        Me.I4.Text = (Hex(i(1))).PadLeft(2, "0")
        Me.I3.Text = (Hex(i(2))).PadLeft(2, "0")
        Me.I2.Text = (Hex(i(3))).PadLeft(2, "0")
        Me.I1.Text = (Hex(i(4))).PadLeft(2, "0")
        Me.I0.Text = (Hex(i(5))).PadLeft(2, "0")
    End Sub

    Public Function GetMAC() As Byte()
        Dim i(5) As Byte
        i(0) = CInt("&H" & Me.I5.Text)
        i(1) = CInt("&H" & Me.I4.Text)
        i(2) = CInt("&H" & Me.I3.Text)
        i(3) = CInt("&H" & Me.I2.Text)
        i(4) = CInt("&H" & Me.I1.Text)
        i(5) = CInt("&H" & Me.I0.Text)
        Return i
    End Function


    Private Sub I3_TextChanged(sender As Object, e As EventArgs) Handles I5.TextChanged, I4.TextChanged, I3.TextChanged, I2.TextChanged, I1.TextChanged, I0.TextChanged
        Dim txt As TextBox = sender

        Try
            Dim value As Integer = CInt("&H" & txt.Text)
            If (value > 255) Then value = 255
            If (value < 0) Then value = 0
            txt.Text = (Hex(value)).PadLeft(2, "0")
            txt.Tag = value

            Me.OnValueChanged(New System.EventArgs)
        Catch ex As Exception
            Try
                txt.Text = CInt(txt.Tag)
            Catch ex1 As Exception

            End Try
        End Try
    End Sub

    Private Sub I3_KeyDown(sender As Object, e As KeyEventArgs) Handles I5.KeyDown, I4.KeyDown, I3.KeyDown, I2.KeyDown, I1.KeyDown, I0.KeyDown
        Const allowchars As String = "0123456789ABCDEF"
        Dim ch As Char = Chr(e.KeyCode)
        If InStr(ch, allowchars) < 0 Then
            e.Handled = True
            e.SuppressKeyPress = True
        End If
    End Sub

    Protected Overridable Sub OnValueChanged(ByVal e As System.EventArgs)
        RaiseEvent ValueChanged(Me, e)
    End Sub

End Class

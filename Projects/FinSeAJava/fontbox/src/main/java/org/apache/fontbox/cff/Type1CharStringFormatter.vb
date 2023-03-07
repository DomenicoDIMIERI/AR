Imports FinSeA.Io

Namespace org.apache.fontbox.cff

    '/**
    ' * This class represents a formatter for CharString commands of a Type1 font.
    ' * @author Villu Ruusmann
    ' * @version $Revision: 1.0 $
    ' */
    Public Class Type1CharStringFormatter


        Private output As ByteArrayOutputStream = Nothing

        '/**
        ' * Formats the given command sequence to a byte array.
        ' * @param sequence the given command sequence
        ' * @return the formatted seuqence as byte array
        ' */
        Public Function format(ByVal sequence As List(Of Object)) As Byte()
            output = New ByteArrayOutputStream()

            For Each [object] As Object In sequence
                If (TypeOf ([object]) Is CharStringCommand) Then
                    writeCommand(DirectCast([object], CharStringCommand))
                ElseIf (TypeOf ([object]) Is NInteger) Then
                    writeNumber(DirectCast([object], NInteger))
                Else
                    Throw New ArgumentException
                End If
            Next
            Return output.toByteArray()
        End Function

        Private Sub writeCommand(ByVal command As CharStringCommand)
            Dim value() As Integer = command.getKey().getValue()
            For i As Integer = 0 To value.Length - 1
                output.Write(value(i))
            Next
        End Sub

        Private Sub writeNumber(ByVal number As NInteger)
            Dim value As Integer = number.Value
            If (value >= -107 AndAlso value <= 107) Then
                output.Write(value + 139)
            ElseIf (value >= 108 AndAlso value <= 1131) Then
                Dim b1 As Integer = (value - 108) Mod 256
                Dim b0 As Integer = (value - 108 - b1) / 256 + 247
                output.Write(b0)
                output.Write(b1)
            ElseIf (value >= -1131 AndAlso value <= -108) Then
                Dim b1 As Integer = -((value + 108) Mod 256)
                Dim b0 As Integer = -((value + 108 + b1) / 256 - 251)
                output.Write(b0)
                output.Write(b1)
            Else
                Dim b1 As Integer = value >> 24 & &HFF '>>>
                Dim b2 As Integer = value >> 16 And &HFF
                Dim b3 As Integer = value >> 8 And &HFF
                Dim b4 As Integer = value >> 0 And &HFF
                output.Write(255)
                output.Write(b1)
                output.Write(b2)
                output.Write(b3)
                output.Write(b4)
            End If
        End Sub


    End Class

End Namespace
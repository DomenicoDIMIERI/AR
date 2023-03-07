Imports System.IO

Namespace org.apache.fontbox.cff

    '/**
    ' * This class represents a converter for a mapping into a Type1-sequence.
    ' * @author Villu Ruusmann
    ' * @version $Revision: 1.0 $
    ' */
    Public Class Type1CharStringParser

        Private input As DataInput = Nothing
        Private sequence As List(Of Object) = Nothing

        '/**
        ' * The given byte array will be parsed and converted to a Type1 sequence.
        ' * @param bytes the given mapping as byte array
        ' * @param localSubrIndex index containing all local subroutines
        ' * 
        ' * @return the Type1 sequence
        ' * @throws IOException if an error occurs during reading
        ' */
        Public Function parse(ByVal bytes() As Byte, ByVal localSubrIndex As IndexData) As List(Of Object)
            Return parse(bytes, localSubrIndex, True)
        End Function

        Private Function parse(ByVal bytes() As Byte, ByVal localSubrIndex As IndexData, ByVal init As Boolean) As List(Of Object)
            If (init) Then
                sequence = New ArrayList(Of Object)()
            End If
            input = New DataInput(bytes)
            Dim localSubroutineIndexProvided As Boolean = localSubrIndex IsNot Nothing AndAlso localSubrIndex.getCount() > 0
            While (input.hasRemaining())
                Dim b0 As Integer = input.readUnsignedByte()

                If (b0 = 10 AndAlso localSubroutineIndexProvided) Then
                    ' process subr command
                    Dim operand As NInteger = sequence.remove(sequence.size() - 1)
                    'get subrbias
                    Dim bias As Integer = 0
                    Dim nSubrs As Integer = localSubrIndex.getCount()

                    If (nSubrs < 1240) Then
                        bias = 107
                    ElseIf (nSubrs < 33900) Then
                        bias = 1131
                    Else
                        bias = 32768
                    End If
                    Dim subrNumber As Integer = bias + operand
                    If (subrNumber < localSubrIndex.getCount()) Then
                        Dim subrBytes() As Byte = localSubrIndex.getBytes(subrNumber)
                        parse(subrBytes, localSubrIndex, False)
                        Dim lastItem As Object = sequence.get(sequence.size() - 1)
                        If (TypeOf (lastItem) Is CharStringCommand AndAlso DirectCast(lastItem, CharStringCommand).getKey().getValue()(0) = 11) Then
                            sequence.remove(sequence.size() - 1) ' remove "return" command
                        End If
                    End If
                ElseIf (b0 >= 0 AndAlso b0 <= 31) Then
                    sequence.add(readCommand(b0))
                ElseIf (b0 >= 32 AndAlso b0 <= 255) Then
                    sequence.add(readNumber(b0))
                Else
                    Throw New ArgumentException
                End If
            End While
            Return sequence
        End Function

        Private Function readCommand(ByVal b0 As Integer) As CharStringCommand
            If (b0 = 12) Then
                Dim b1 As Integer = input.readUnsignedByte()
                Return New CharStringCommand(b0, b1)
            End If
            Return New CharStringCommand(b0)
        End Function

        Private Function readNumber(ByVal b0 As Integer) As NInteger
            If (b0 >= 32 AndAlso b0 <= 246) Then
                Return NInteger.valueOf(b0 - 139)
            ElseIf (b0 >= 247 AndAlso b0 <= 250) Then
                Dim b1 As Integer = input.readUnsignedByte()
                Return NInteger.valueOf((b0 - 247) * 256 + b1 + 108)
            ElseIf (b0 >= 251 AndAlso b0 <= 254) Then
                Dim b1 As Integer = input.readUnsignedByte()
                Return NInteger.valueOf(-(b0 - 251) * 256 - b1 - 108)
            ElseIf (b0 = 255) Then
                Dim b1 As Integer = input.readUnsignedByte()
                Dim b2 As Integer = input.readUnsignedByte()
                Dim b3 As Integer = input.readUnsignedByte()
                Dim b4 As Integer = input.readUnsignedByte()

                Return NInteger.valueOf(b1 << 24 Or b2 << 16 Or b3 << 8 Or b4)
            Else
                Throw New ArgumentException
            End If
        End Function

    End Class

End Namespace
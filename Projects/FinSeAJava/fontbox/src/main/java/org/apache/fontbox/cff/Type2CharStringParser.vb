Namespace org.apache.fontbox.cff

    '/**
    ' * This class represents a converter for a mapping into a Type2-sequence.
    ' * @author Villu Ruusmann
    ' * @version $Revision: 1.0 $
    ' */
    Public Class Type2CharStringParser

        Private hstemCount As Integer = 0
        Private vstemCount As Integer = 0
        Private sequence As List(Of Object) = Nothing

        '/**
        ' * The given byte array will be parsed and converted to a Type2 sequence.
        ' * @param bytes the given mapping as byte array
        ' * @param globalSubrIndex index containing all global subroutines
        ' * @param localSubrIndex index containing all local subroutines
        ' * 
        ' * @return the Type2 sequence
        ' * @throws IOException if an error occurs during reading
        ' */
        Public Function parse(ByVal bytes() As Byte, ByVal globalSubrIndex As IndexData, ByVal localSubrIndex As IndexData) As List(Of Object)
            Return parse(bytes, globalSubrIndex, localSubrIndex, True)
        End Function

        Private Function parse(ByVal bytes() As Byte, ByVal globalSubrIndex As IndexData, ByVal localSubrIndex As IndexData, ByVal init As Boolean) As List(Of Object)
            If (init) Then
                hstemCount = 0
                vstemCount = 0
                sequence = New ArrayList(Of Object)()
            End If
            Dim input As New DataInput(bytes)
            Dim localSubroutineIndexProvided As Boolean = localSubrIndex IsNot Nothing AndAlso localSubrIndex.getCount() > 0
            Dim globalSubroutineIndexProvided As Boolean = globalSubrIndex IsNot Nothing AndAlso globalSubrIndex.getCount() > 0

            While (Input.hasRemaining())
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
                        parse(subrBytes, globalSubrIndex, localSubrIndex, False)
                        Dim lastItem As Object = sequence.get(sequence.size() - 1)
                        If (TypeOf (lastItem) Is CharStringCommand AndAlso DirectCast(lastItem, CharStringCommand).getKey().getValue()(0) = 11) Then
                            sequence.remove(sequence.size() - 1) ' remove "return" command
                        End If
                    End If
                ElseIf (b0 = 29 AndAlso globalSubroutineIndexProvided) Then
                    ' process globalsubr command
                    Dim operand As NInteger = sequence.remove(sequence.size() - 1)
                    'get subrbias
                    Dim bias As Integer = 0
                    Dim nSubrs As Integer = globalSubrIndex.getCount()

                    If (nSubrs < 1240) Then
                        bias = 107
                    ElseIf (nSubrs < 33900) Then
                        bias = 1131
                    Else
                        bias = 32768
                    End If

                    Dim subrNumber As Integer = bias + operand
                    If (subrNumber < globalSubrIndex.getCount()) Then
                        Dim subrBytes() As Byte = globalSubrIndex.getBytes(subrNumber)
                        parse(subrBytes, globalSubrIndex, localSubrIndex, False)
                        Dim lastItem As Object = sequence.get(sequence.size() - 1)
                        If (TypeOf (lastItem) Is CharStringCommand AndAlso DirectCast(lastItem, CharStringCommand).getKey().getValue()(0) = 11) Then
                            sequence.remove(sequence.size() - 1) ' remove "return" command
                        End If
                    End If
                ElseIf (b0 >= 0 AndAlso b0 <= 27) Then
                    sequence.add(readCommand(b0, input))
                ElseIf (b0 = 28) Then
                    sequence.add(readNumber(b0, input))
                ElseIf (b0 >= 29 AndAlso b0 <= 31) Then
                    sequence.add(readCommand(b0, input))
                ElseIf (b0 >= 32 AndAlso b0 <= 255) Then
                    sequence.add(readNumber(b0, input))
                Else
                    Throw New ArgumentException
                End If
            End While
            Return sequence
        End Function

        Private Function readCommand(ByVal b0 As Integer, ByVal input As DataInput) As CharStringCommand
            If (b0 = 1 OrElse b0 = 18) Then
                hstemCount += peekNumbers().size() / 2
            ElseIf (b0 = 3 OrElse b0 = 19 OrElse b0 = 20 OrElse b0 = 23) Then
                vstemCount += peekNumbers().size() / 2
            End If ' End if

            If (b0 = 12) Then
                Dim b1 As Integer = input.readUnsignedByte()

                Return New CharStringCommand(b0, b1)
            ElseIf (b0 = 19 OrElse b0 = 20) Then
                Dim value() As Integer = Array.CreateInstance(GetType(Integer), 1 + getMaskLength())
                value(0) = b0

                For i = 1 To value.Length - 1
                    value(i) = input.readUnsignedByte()
                Next

                Return New CharStringCommand(value)
            End If

            Return New CharStringCommand(b0)
        End Function

        Private Function readNumber(ByVal b0 As Integer, ByVal input As DataInput) As NInteger
            If (b0 = 28) Then
                Dim b1 As Integer = input.readUnsignedByte()
                Dim b2 As Integer = input.readUnsignedByte()

                Return NInteger.valueOf(CShort(b1 << 8 Or b2))
            ElseIf (b0 >= 32 AndAlso b0 <= 246) Then
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
                ' The lower bytes are representing the digits after 
                ' the decimal point and aren't needed in Me context
                input.readUnsignedByte()
                input.readUnsignedByte()
                Return NInteger.valueOf(CShort(b1 << 8 Or b2))
            Else
                Throw New ArgumentException
            End If
        End Function

        Private Function getMaskLength() As Integer
            Dim length As Integer = 1

            Dim hintCount As Integer = hstemCount + vstemCount
            While (hintCount > 0)
                length += 1
                hintCount -= 8
            End While

            Return length
        End Function

        Private Function peekNumbers() As List(Of Number)
            Dim numbers As List(Of Number) = New ArrayList(Of Number)
            For i As Integer = sequence.size() - 1 To 0 Step -1 ' -1?
                Dim [object] As Object = sequence.get(i)
                If (TypeOf ([object]) Is Number) Then
                    Dim number As Number = [object]
                    numbers.add(0, number)
                    Continue For
                End If
                Return numbers
            Next
            Return numbers
        End Function

    End Class

End Namespace

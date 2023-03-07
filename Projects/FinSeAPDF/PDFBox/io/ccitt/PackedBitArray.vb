Imports FinSeA.Text

Namespace org.apache.pdfbox.io.ccitt

    ''' <summary>
    ''' Represents an array of bits packed in a byte array of fixed size.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class PackedBitArray

        Private bitCount As Integer
        Private data() As Byte


        ''' <summary>
        ''' Constructs a new bit array.
        ''' </summary>
        ''' <param name="bitCount">the number of bits to maintain</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal bitCount As Integer)
            If (bitCount <= 0) Then Throw New ArgumentOutOfRangeException("bitCount deve essere positivo")
            Me.bitCount = bitCount
            Dim byteCount As Integer = (bitCount + 7) / 8
            ReDim Me.data(byteCount - 1)
        End Sub

        Private Function byteOffset(ByVal offset As Integer) As Integer
            Return offset / 8
        End Function

        Private Function bitOffset(ByVal offset As Integer) As Integer
            Return offset Mod 8
        End Function

        '/**
        ' * Sets a bit at the given offset.
        ' * @param offset the offset
        ' */
        Public Sub [set](ByVal offset As Integer)
            Dim byteOffset = Me.byteOffset(offset)
            Me.data(byteOffset) = Me.data(byteOffset) Or (1 << Me.bitOffset(offset))
        End Sub

        '/**
        ' * Clears a bit at the given offset.
        ' * @param offset the offset
        ' */
        Public Sub clear(ByVal offset As Integer)
            Dim byteOffset As Integer = Me.byteOffset(offset)
            Dim bitOffset As Integer = Me.bitOffset(offset)
            Me.data(byteOffset) = Me.data(byteOffset) And Not (1 << bitOffset)
        End Sub

        '/**
        ' * Sets a run of bits at the given offset to either 1 or 0.
        ' * @param offset the offset
        ' * @param length the number of bits to set
        ' * @param bit 1 to set the bit, 0 to clear it
        ' */
        Public Sub setBits(ByVal offset As Integer, ByVal length As Integer, ByVal bit As Integer)
            If (bit = 0) Then
                clearBits(offset, length)
            Else
                setBits(offset, length)
            End If
        End Sub

        '/**
        ' * Sets a run of bits at the given offset to either 1.
        ' * @param offset the offset
        ' * @param length the number of bits to set
        ' */
        Public Sub setBits(ByVal offset As Integer, ByVal length As Integer)
            If (length = 0) Then Return
            Dim startBitOffset As Integer = Me.bitOffset(offset)
            Dim firstByte As Integer = Me.byteOffset(offset)
            Dim lastBitOffset As Integer = offset + length
            If (lastBitOffset > Me.getBitCount()) Then
                Throw New IndexOutOfRangeException("offset + length > bit count")
            End If
            Dim lastByte As Integer = Me.byteOffset(lastBitOffset)
            Dim endBitOffset As Integer = Me.bitOffset(lastBitOffset)

            If (firstByte = lastByte) Then
                'Only one byte affected
                Dim mask As Integer = (1 << endBitOffset) - (1 << startBitOffset)
                Me.data(firstByte) = Me.data(firstByte) Or mask
            Else
                'Bits spanning multiple bytes
                Me.data(firstByte) = Me.data(firstByte) Or (&HFF << startBitOffset)
                For i As Integer = firstByte + 1 To lastByte - 1
                    Me.data(i) = &HFF
                Next
                If (endBitOffset > 0) Then
                    Me.data(lastByte) = Me.data(lastByte) Or (&HFF >> (8 - endBitOffset))
                End If
            End If
        End Sub

        '/**
        ' * Clears a run of bits at the given offset.
        ' * @param offset the offset
        ' * @param length the number of bits to clear
        ' */
        Public Sub clearBits(ByVal offset As Integer, ByVal length As Integer)
            If (length = 0) Then Return
            Dim startBitOffset As Integer = offset Mod 8
            Dim firstByte As Integer = byteOffset(offset)
            Dim lastBitOffset As Integer = offset + length
            Dim lastByte As Integer = byteOffset(lastBitOffset)
            Dim endBitOffset As Integer = lastBitOffset Mod 8

            If (firstByte = lastByte) Then
                'Only one byte affected
                Dim mask As Integer = (1 << endBitOffset) - (1 << startBitOffset)
                Me.data(firstByte) = Me.data(firstByte) And Not mask
            Else
                'Bits spanning multiple bytes
                Me.data(firstByte) = Me.data(firstByte) And Not (&HFF << startBitOffset)
                For i As Integer = firstByte + 1 To lastByte - 1
                    Me.data(i) = 0
                Next
                If (endBitOffset > 0) Then
                    Me.data(lastByte) = Me.data(lastByte) And Not (&HFF >> (8 - endBitOffset))
                End If
            End If
        End Sub

        ''' <summary>
        ''' Clear all bits in the array.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub clear()
            clearBits(0, getBitCount())
        End Sub

        ''' <summary>
        ''' Returns the number of bits maintained by this array.
        ''' </summary>
        ''' <returns>the number of bits</returns>
        ''' <remarks></remarks>
        Public Function getBitCount() As Integer
            Return Me.bitCount
        End Function

        ''' <summary>
        ''' Returns the size of the byte buffer for this array.
        ''' </summary>
        ''' <returns>the size of the byte buffer</returns>
        ''' <remarks></remarks>
        Public Function getByteCount() As Integer
            Return Me.data.Length
        End Function

        ''' <summary>
        ''' Returns the underlying byte buffer.
        ''' </summary>
        ''' <returns>the underlying data buffer</returns>
        ''' <remarks>Note: the actual buffer is returned. If it's manipulated the content of the bit array changes.</remarks>
        Public Function getData() As Byte()
            Return Me.data
        End Function

        Public Overrides Function toString() As String
            Return toBitString(Me.data).substring(0, Me.bitCount)
        End Function

        ''' <summary>
        ''' Converts a byte to a "binary" String of 0s and 1s.
        ''' </summary>
        ''' <param name="data">the value to convert</param>
        ''' <returns>the binary string</returns>
        ''' <remarks></remarks>
        Public Shared Function toBitString(ByVal data As Byte) As String
            Dim buf() As Byte = {data}
            Return toBitString(buf)
        End Function

        '/**
        ' * Converts a series of bytes to a "binary" String of 0s and 1s.
        ' * @param data the data
        ' * @return the binary string
        ' */
        Public Shared Function toBitString(ByVal data() As Byte) As String
            Return toBitString(data, 0, data.Length)
        End Function

        '/**
        ' * Converts a series of bytes to a "binary" String of 0s and 1s.
        ' * @param data the data
        ' * @param start the start offset
        ' * @param len the number of bytes to convert
        ' * @return the binary string
        ' */
        Public Shared Function toBitString(ByVal data() As Byte, ByVal start As Integer, ByVal len As Integer) As String
            Dim sb As New StringBuffer()
            Dim [end] As Integer = start + len
            For x As Integer = start To [end] - 1
                For i As Integer = 0 To 8
                    Dim mask As Integer = 1 << i
                    Dim value As Integer = data(x) And mask
                    sb.append(IIf(value = mask, "1", "0"))
                Next
            Next
            Return sb.toString()
        End Function

    End Class

End Namespace
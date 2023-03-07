Imports System.IO

Namespace org.apache.fontbox.cff

    '/**
    ' * This class contains some functionality to read a byte buffer.
    ' * 
    ' * @author Villu Ruusmann
    ' * @version $Revision$
    ' */
    Public Class DataInput

        Private inputBuffer() As Byte = Nothing
        Private bufferPosition As Integer = 0

        '/**
        ' * Constructor.
        ' * @param buffer the buffer to be read
        ' */
        Public Sub New(ByVal buffer() As Byte)
            inputBuffer = buffer
        End Sub

        '/**
        ' * Determines if there are any bytes left to read or not. 
        ' * @return true if there are any bytes left to read
        ' */
        Public Function hasRemaining() As Boolean
            Return bufferPosition < inputBuffer.Length
        End Function

        '/**
        ' * Returns the current position.
        ' * @return current position
        ' */
        Public Function getPosition() As Integer
            Return bufferPosition
        End Function

        '/**
        ' * Sets the current position to the given value.
        ' * @param position the given position
        ' */
        Public Sub setPosition(ByVal position As Integer)
            bufferPosition = position
        End Sub

        '/** 
        ' * Returns the buffer as an ISO-8859-1 string.
        ' * @return the buffer as string
        ' * @throws IOException if an error occurs during reading
        ' */
        Public Function getString() As String ' throws IOException
            Return Sistema.Strings.GetString(inputBuffer, "ISO-8859-1")
        End Function

        '/**
        ' * Read one single byte from the buffer.
        ' * @return the byte
        ' * @throws IOException if an error occurs during reading
        ' */
        Public Function readByte() As SByte ' throws IOException
            Return readUnsignedByte()
        End Function

        '/**
        ' * Read one single unsigned byte from the buffer.
        ' * @return the unsigned byte as int
        ' * @throws IOException if an error occurs during reading
        ' */
        Public Function readUnsignedByte() As Integer
            Dim b As Integer = read()
            If (b < 0) Then
                Throw New EndOfStreamException
            End If
            Return b
        End Function

        '/**
        ' * Read one single short value from the buffer.
        ' * @return the short value
        ' * @throws IOException if an error occurs during reading
        ' */
        Public Function readShort() As Short ' throws IOException
            Return readUnsignedShort()
        End Function

        '/**
        ' * Read one single unsigned short (2 bytes) value from the buffer.
        ' * @return the unsigned short value as int
        ' * @throws IOException if an error occurs during reading
        ' */
        Public Function readUnsignedShort() As Integer ' throws IOException
            Dim b1 As Integer = read()
            Dim b2 As Integer = read()
            If ((b1 Or b2) < 0) Then
                Throw New EndOfStreamException
            End If
            Return b1 << 8 Or b2
        End Function

        '/**
        ' * Read one single int (4 bytes) from the buffer.
        ' * @return the int value
        ' * @throws IOException if an error occurs during reading
        ' */
        Public Function readInt() As Integer 'throws IOException
            Dim b1 As Integer = read()
            Dim b2 As Integer = read()
            Dim b3 As Integer = read()
            Dim b4 As Integer = read()
            If ((b1 Or b2 Or b3 Or b4) < 0) Then
                Throw New EndOfStreamException
            End If
            Return b1 << 24 Or b2 << 16 Or b3 << 8 Or b4
        End Function

        '/**
        ' * Read a number of single byte values from the buffer.
        ' * @param length the number of bytes to be read
        ' * @return an array with containing the bytes from the buffer 
        ' * @throws IOException if an error occurs during reading
        ' */
        Public Function readBytes(ByVal length As Integer) As Byte() ' throws IOException
            Dim bytes() As Byte = Array.CreateInstance(GetType(Byte), length)
            For i As Integer = 0 To length - 1
                bytes(i) = readByte()
            Next
            Return bytes
        End Function

        Private Function read() As Integer
            Try
                Dim value As Integer = inputBuffer(bufferPosition) And &HFF
                bufferPosition += 1
                Return value
            Catch re As RuntimeException
                Return -1
            End Try
        End Function

    End Class

End Namespace
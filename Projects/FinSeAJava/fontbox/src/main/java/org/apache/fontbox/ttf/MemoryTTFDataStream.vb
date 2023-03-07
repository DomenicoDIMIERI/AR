Imports FinSeA.Sistema
Imports System.IO
Imports FinSeA.Io

Namespace org.apache.fontbox.ttf

    '/**
    ' * An interface into a data stream.
    ' * 
    ' * @author Ben Litchfield (ben@benlitchfield.com)
    ' * @version $Revision: 1.2 $
    ' */
    Public Class MemoryTTFDataStream
        Inherits TTFDataStream

        Private data() As Byte = Nothing
        Private currentPosition As Integer = 0

        '/**
        ' * Constructor from a stream. 
        ' * @param is The stream of read from.
        ' * @throws IOException If an error occurs while reading from the stream.
        ' */
        Public Sub New(ByVal [is] As InputStream)
            Try
                Dim output As New ByteArrayOutputStream([is].available())
                Dim buffer() As Byte = Arrays.CreateInstance(Of Byte)(1024)
                Dim amountRead As Integer
                amountRead = [is].read(buffer)
                While (amountRead > 0)
                    output.Write(buffer, 0, amountRead)
                    amountRead = [is].read(buffer)
                End While
                data = output.toByteArray()
            Finally
                If ([is] IsNot Nothing) Then
                    [is].Close()
                End If
            End Try
        End Sub

        '/**
        ' * Read an unsigned byte.
        ' * @return An unsigned byte.
        ' * @throws IOException If there is an error reading the data.
        ' */
        Public Overrides Function readLong() As Long
            Return (CLng(readSignedInt()) << 32) Or (readSignedInt() And &HFFFFFFFFL)
        End Function

        '/**
        ' * Read a signed integer.
        ' * 
        ' * @return A signed integer.
        ' * @throws IOException If there is a problem reading the file.
        ' */
        Public Function readSignedInt() As Integer
            Dim ch1 As Integer = read()
            Dim ch2 As Integer = read()
            Dim ch3 As Integer = read()
            Dim ch4 As Integer = read()
            If (ch4 < 0) Then
                Throw New EndOfStreamException
            End If
            Return ((ch1 << 24) Or (ch2 << 16) Or (ch3 << 8) Or (ch4 << 0))
        End Function

        '/**
        ' * Read an unsigned byte.
        ' * @return An unsigned byte.
        ' * @throws IOException If there is an error reading the data.
        ' */
        Public Overrides Function read() As Integer
            Dim retval As Integer = -1
            If (currentPosition < data.Length) Then
                retval = data(currentPosition)
            Else
                'TODO verificare cosa succede quando currentPosition >= data.Length
                Debug.Print("TODO verificare cosa succede quando currentPosition >= data.Length")
            End If
            currentPosition += 1
            Return (retval + 256) Mod 256
        End Function

        '/**
        ' * Read an unsigned short.
        ' * 
        ' * @return An unsigned short.
        ' * @throws IOException If there is an error reading the data.
        ' */
        Public Overrides Function readUnsignedShort() As Integer
            Dim ch1 As Integer = Me.read()
            Dim ch2 As Integer = Me.read()
            If (ch2 < 0) Then
                Throw New EndOfStreamException
            End If
            Return (ch1 << 8) Or (ch2 << 0)
        End Function

        '/**
        ' * Read an signed short.
        ' * 
        ' * @return An signed short.
        ' * @throws IOException If there is an error reading the data.
        ' */
        Public Overrides Function readSignedShort() As Short
            Dim ch1 As Integer = Me.read()
            Dim ch2 As Integer = Me.read()
            If (ch2 < 0) Then
                Throw New EndOfStreamException
            End If
            Return ((ch1 << 8) Or (ch2 << 0))
        End Function

        '/**
        ' * Close the underlying resources.
        ' * 
        ' * @throws IOException If there is an error closing the resources.
        ' */
        Public Overrides Sub close()
            data = Nothing
        End Sub

        '/**
        ' * Seek into the datasource.
        ' * 
        ' * @param pos The position to seek to.
        ' * @throws IOException If there is an error seeking to that position.
        ' */
        Public Overrides Sub seek(ByVal pos As Long)
            currentPosition = pos
        End Sub

        '/**
        ' * @see java.io.InputStream#read( byte[], int, int )
        ' * 
        ' * @param b The buffer to write to.
        ' * @param off The offset into the buffer.
        ' * @param len The length into the buffer.
        ' * 
        ' * @return The number of bytes read, or -1 at the end of the stream
        ' * 
        ' * @throws IOException If there is an error reading from the stream.
        ' */
        Public Overrides Function read(ByVal b() As Byte, ByVal off As Integer, ByVal len As Integer) As Integer
            If (currentPosition < data.Length) Then
                Dim amountRead As Integer = Math.Min(len, data.Length - currentPosition)
                Array.Copy(data, currentPosition, b, off, amountRead)
                currentPosition += amountRead
                Return amountRead
            Else
                Return -1
            End If
        End Function

        '/**
        ' * Get the current position in the stream.
        ' * @return The current position in the stream.
        ' * @throws IOException If an error occurs while reading the stream.
        ' */
        Public Overrides Function getCurrentPosition() As Long
            Return currentPosition
        End Function

        Public Overrides Function getOriginalData() As InputStream
            Return New ByteArrayInputStream(data)
        End Function

    End Class

End Namespace
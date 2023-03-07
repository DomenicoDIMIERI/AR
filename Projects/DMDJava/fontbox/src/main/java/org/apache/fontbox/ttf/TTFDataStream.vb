Imports System.IO
Imports FinSeA.Io

Namespace org.apache.fontbox.ttf

    '/**
    ' * An interface into a data stream.
    ' * 
    ' * @author Ben Litchfield (ben@benlitchfield.com)
    ' * 
    ' */
    Public MustInherit Class TTFDataStream

        '/**
        ' * Read a 16.16 fixed value, where the first 16 bits are the decimal and the last 16 bits are the fraction.
        ' * 
        ' * @return A 32 bit value.
        ' * @throws IOException If there is an error reading the data.
        ' */
        Public Function read32Fixed() As Single
            Dim retval As Single = 0
            retval = readSignedShort()
            retval += (readUnsignedShort() / 65536.0)
            Return retval
        End Function

        '/**
        ' * Read a fixed length ascii string.
        ' * 
        ' * @param length The length of the string to read.
        ' * @return A string of the desired length.
        ' * @throws IOException If there is an error reading the data.
        ' */
        Public Function readString(ByVal length As Integer) As String
            Return readString(length, "ISO-8859-1")
        End Function

        '/**
        ' * Read a fixed length ascii string.
        ' * 
        ' * @param length The length of the string to read in bytes.
        ' * @param charset The expected character set of the string.
        ' * @return A string of the desired length.
        ' * @throws IOException If there is an error reading the data.
        ' */
        Public Function readString(ByVal length As Integer, ByVal charset As String) As String
            Dim buffer() As Byte = read(length)
            Return Sistema.Strings.GetString(buffer, charset)
        End Function

        '/**
        ' * Read an unsigned byte.
        ' * 
        ' * @return An unsigned byte.
        ' * @throws IOException If there is an error reading the data.
        ' */
        Public MustOverride Function read() As Integer

        '/**
        ' * Read an unsigned byte.
        ' * 
        ' * @return An unsigned byte.
        ' * @throws IOException If there is an error reading the data.
        ' */
        Public MustOverride Function readLong() As Long

        '/**
        ' * Read a signed byte.
        ' * 
        ' * @return A signed byte.
        ' * @throws IOException If there is an error reading the data.
        ' */
        Public Function readSignedByte() As Integer
            Dim signedByte As Integer = read()
            Return IIf(signedByte < 127, signedByte, signedByte - 256)
        End Function

        '/**
        ' * Read a unsigned byte. Similar to {@link #read()}, but throws an exception if EOF is unexpectedly reached.
        ' * 
        ' * @return A unsigned byte.
        ' * @throws IOException If there is an error reading the data.
        ' */
        Public Function readUnsignedByte() As Integer
            Dim unsignedByte As Integer = read()
            If (unsignedByte = -1) Then
                Throw New EndOfStreamException("premature EOF")
            End If
            Return unsignedByte
        End Function

        '/**
        ' * Read an unsigned integer.
        ' * 
        ' * @return An unsiged integer.
        ' * @throws IOException If there is an error reading the data.
        ' */
        Public Function readUnsignedInt() As Long
            Dim byte1 As Long = read()
            Dim byte2 As Long = read()
            Dim byte3 As Long = read()
            Dim byte4 As Long = read()
            If (byte4 < 0) Then
                Throw New EndOfStreamException
            End If
            Return (byte1 << 24) Or (byte2 << 16) Or (byte3 << 8) Or (byte4 << 0)
        End Function

        '/**
        ' * Read an unsigned short.
        ' * 
        ' * @return An unsigned short.
        ' * @throws IOException If there is an error reading the data.
        ' */
        Public MustOverride Function readUnsignedShort() As Integer

        '/**
        ' * Read an unsigned byte array.
        ' * 
        ' * @param length the length of the array to be read
        ' * @return An unsigned byte array.
        ' * @throws IOException If there is an error reading the data.
        ' */
        Public Function readUnsignedByteArray(ByVal length As Integer) As Integer()
            Dim array() As Integer = System.Array.CreateInstance(GetType(Integer), length)
            For i As Integer = 0 To length - 1
                array(i) = read()
            Next
            Return array
        End Function

        '/**
        ' * Read an unsigned short array.
        ' * 
        ' * @param length The length of the array to read.
        ' * @return An unsigned short array.
        ' * @throws IOException If there is an error reading the data.
        ' */
        Public Function readUnsignedShortArray(ByVal length As Integer) As Integer()
            Dim array() As Integer = System.Array.CreateInstance(GetType(Integer), length)
            For i As Integer = 0 To length - 1
                array(i) = readUnsignedShort()
            Next
            Return array
        End Function

        '/**
        ' * Read an signed short.
        ' * 
        ' * @return An signed short.
        ' * @throws IOException If there is an error reading the data.
        ' */
        Public MustOverride Function readSignedShort() As Short

        '/**
        ' * Read an eight byte international date.
        ' * 
        ' * @return An signed short.
        ' * @throws IOException If there is an error reading the data.
        ' */
        Public Function readInternationalDate() As NDate
            Dim secondsSince1904 As Long = readLong()
            Dim cal As New NDate(1904, 0, 1)
            Dim millisFor1904 As Long = cal.getTimeInMillis()
            millisFor1904 += (secondsSince1904 * 1000)
            cal.setTimeInMillis(millisFor1904)
            Return cal
        End Function

        '/**
        ' * Close the underlying resources.
        ' * 
        ' * @throws IOException If there is an error closing the resources.
        ' */
        Public MustOverride Sub close()

        '/**
        ' * Seek into the datasource.
        ' * 
        ' * @param pos The position to seek to.
        ' * @throws IOException If there is an error seeking to that position.
        ' */
        Public MustOverride Sub seek(ByVal pos As Long)

        '/**
        ' * Read a specific number of bytes from the stream.
        ' * 
        ' * @param numberOfBytes The number of bytes to read.
        ' * @return The byte buffer.
        ' * @throws IOException If there is an error while reading.
        ' */
        Public Function read(ByVal numberOfBytes As Integer) As Byte()
            Dim data() As Byte = Array.CreateInstance(GetType(Byte), numberOfBytes)
            Dim amountRead As Integer = 0
            Dim totalAmountRead As Integer = 0
            ' read at most numberOfBytes bytes from the stream.
            amountRead = read(data, totalAmountRead, numberOfBytes - totalAmountRead)
            While (totalAmountRead < numberOfBytes AndAlso (amountRead > 0))
                totalAmountRead += amountRead
                amountRead = read(data, totalAmountRead, numberOfBytes - totalAmountRead)
            End While
            If (totalAmountRead = numberOfBytes) Then
                Return data
            Else
                Throw New IOException("Unexpected end of TTF stream reached")
            End If
        End Function

        '/**
        ' * @see java.io.InputStream#read(byte[], int, int )
        ' * 
        ' * @param b The buffer to write to.
        ' * @param off The offset into the buffer.
        ' * @param len The length into the buffer.
        ' * 
        ' * @return The number of bytes read, or -1 at the end of the stream
        ' * 
        ' * @throws IOException If there is an error reading from the stream.
        ' */
        Public MustOverride Function read(ByVal b() As Byte, ByVal off As Integer, ByVal len As Integer) As Integer

        '/**
        ' * Get the current position in the stream.
        ' * 
        ' * @return The current position in the stream.
        ' * @throws IOException If an error occurs while reading the stream.
        ' */
        Public MustOverride Function getCurrentPosition() As Long

        '/**
        ' * This will get the original data file that was used for Me stream.
        ' * 
        ' * @return The data that was read from.
        ' * @throws IOException If there is an issue reading the data.
        ' */
        Public MustOverride Function getOriginalData() As InputStream


    End Class

End Namespace

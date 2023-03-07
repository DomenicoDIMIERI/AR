Imports FinSeA.Io
'import java.io.IOException;
'import java.io.InputStream;
'import java.io.OutputStream;
'import java.io.Reader;
'import java.io.Writer;

Imports System.IO

Namespace org.apache.pdfbox.io


    '/**
    ' * This class contains various I/O-related methods.
    ' * @version $Revision$
    ' */
    Public Class IOUtils


        'TODO PDFBox should really use Apache Commons IO.

        Private Sub New()
            'Utility class. Don't instantiate.
        End Sub

        '/**
        ' * Reads the input stream and returns its contents as a byte array.
        ' * @param in the input stream to read from.
        ' * @return the byte array
        ' * @throws IOException if an I/O error occurs
        ' */
        Public Shared Function toByteArray(ByVal [in] As Stream) As Byte() ' throws IOException
            'ByteArrayOutputStream baout = New ByteArrayOutputStream()
            'copy(in, baout)
            '   Return baout.toByteArray()
            Dim stream As New MemoryStream
            copy([in], stream)
            Dim ret() As Byte = stream.ToArray
            stream.Dispose()
            Return ret
        End Function

        '/**
        ' * Copies all the contents from the given input stream to the given output stream.
        ' * @param input the input stream
        ' * @param output the output stream
        ' * @return the number of bytes that have been copied
        ' * @throws IOException if an I/O error occurs
        ' */
        Public Shared Function copy(ByVal input As Stream, ByVal output As Stream) As Long ' throws IOException
            Dim buffer() As Byte
            ReDim buffer(4096 - 1)
            Dim count As Long = 0
            Dim n As Integer
            n = input.Read(buffer, 0, 4096)
            While (n > 0)
                output.Write(buffer, 0, n)
                count += n
            End While
            Return count
        End Function

        '/**
        ' * Populates the given buffer with data read from the input stream. If the data doesn't
        ' * fit the buffer, only the data that fits in the buffer is read. If the data is less than
        ' * fits in the buffer, the buffer is not completely filled.
        ' * @param in the input stream to read from
        ' * @param buffer the buffer to fill
        ' * @return the number of bytes written to the buffer
        ' * @throws IOException if an I/O error occurs
        ' */
        Public Shared Function populateBuffer(ByVal [in] As Stream, ByVal buffer() As Byte) As Long ' throws IOException
            Dim remaining As Integer = buffer.Length
            While (remaining > 0)
                Dim bufferWritePos As Integer = buffer.Length - remaining
                Dim bytesRead As Integer = [in].Read(buffer, bufferWritePos, remaining)
                If (bytesRead <= 0) Then
                    Exit While 'EOD
                End If
                remaining -= bytesRead
            End While
            Return buffer.Length - remaining
        End Function


        '/**
        ' * Unconditionally close an <code>InputStream</code>.
        ' * <p>
        ' * Equivalent to {@link InputStream#close()}, except any exceptions will be ignored.
        ' * This is typically used in finally blocks.
        ' *
        ' * @param input  the InputStream to close, may be null or already closed
        ' */
        Public Shared Sub closeQuietly(ByVal input As Stream)
            Try
                If (input IsNot Nothing) Then
                    input.Close()
                End If
            Catch ioe As IOException
                ' ignore
            End Try
        End Sub

        '/**
        ' * Unconditionally close an <code>Reader</code>.
        ' * <p>
        ' * Equivalent to {@link Reader#close()}, except any exceptions will be ignored.
        ' * This is typically used in finally blocks.
        ' *
        ' * @param input  the Reader to close, may be null or already closed
        ' */
        Public Shared Sub closeQuietly(ByVal input As StreamReader)
            Try
                If (input IsNot Nothing) Then
                    input.close()
                End If
            Catch ioe As IOException
                ' ignore
            End Try
        End Sub

        '/**
        ' * Unconditionally close a <code>Writer</code>.
        ' * <p>
        ' * Equivalent to {@link Writer#close()}, except any exceptions will be ignored.
        ' * This is typically used in finally blocks.
        ' *
        ' * @param output  the Writer to close, may be null or already closed
        ' */
        Public Shared Sub closeQuietly(ByVal output As StreamWriter)
            Try
                If (output IsNot Nothing) Then
                    output.close()
                End If
            Catch ioe As IOException
                ' ignore
            End Try
        End Sub

        '/**
        ' * Unconditionally close an <code>OutputStream</code>.
        ' * <p>
        ' * Equivalent to {@link OutputStream#close()}, except any exceptions will be ignored.
        ' * This is typically used in finally blocks.
        ' *
        ' * @param output  the OutputStream to close, may be null or already closed
        ' */
        Public Shared Sub closeQuietly(ByVal output As OutputStream)
            Try
                If (output IsNot Nothing) Then
                    output.Close()
                End If
            Catch ioe As IOException
                'ignore
            End Try
        End Sub


    End Class

End Namespace

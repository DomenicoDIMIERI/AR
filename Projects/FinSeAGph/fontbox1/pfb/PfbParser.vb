Imports FinSeA.Io
Imports System.IO

Namespace org.fontbox.pfb

    '/**
    ' * Parser for a pfb-file.
    ' *
    ' * @author Ben Litchfield (ben@benlitchfield.com)
    ' * @author <a href="mailto:m.g.n@gmx.de">Michael Niedermair</a>
    ' * @version $Revision: 1.1 $
    ' */
    Public Class PfbParser

        ''' <summary>
        ''' the pdf header length.
        ''' (start-marker (1 byte), ascii-/binary-marker (1 byte), size (4 byte))
        ''' 3*6 == 18
        ''' </summary>
        ''' <remarks></remarks>
        Private Const PFB_HEADER_LENGTH = 18

        ''' <summary>
        ''' the start marker.
        ''' </summary>
        ''' <remarks></remarks>
        Private Const START_MARKER = &H80

        ''' <summary>
        ''' the ascii marker.
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ASCII_MARKER = &H1

        ''' <summary>
        ''' the binary marker.
        ''' </summary>
        ''' <remarks></remarks>
        Private Const BINARY_MARKER = &H2

        ''' <summary>
        ''' The record types in the pfb-file.
        ''' </summary>
        ''' <remarks></remarks>
        Private Shared ReadOnly PFB_RECORDS As Integer() = {ASCII_MARKER, BINARY_MARKER, ASCII_MARKER}

        ''' <summary>
        ''' buffersize.
        ''' </summary>
        ''' <remarks></remarks>
        Private Const BUFFER_SIZE = &HFFFF

        ''' <summary>
        ''' the parsed pfb-data.
        ''' </summary>
        ''' <remarks></remarks>
        Private pfbdata() As Byte

        ''' <summary>
        ''' the lengths of the records.
        ''' </summary>
        ''' <remarks></remarks>
        Private lengths As Integer()

        '// sample (pfb-file)
        '// 00000000 80 01 8b 15  00 00 25 21  50 53 2d 41  64 6f 62 65  
        '//          ......%!PS-Adobe


        '/**
        ' * Create a new object.
        ' * @param filename  the file name
        ' * @throws IOException if an IO-error occurs.
        ' */
        Public Sub New(ByVal filename As String)  'throws IOException 
            Dim stream As BufferedInputStream = Nothing
            Dim pfb() As Byte
            Try
                stream = New BufferedInputStream(New FileInputStream(filename), BUFFER_SIZE)
                pfb = readPfbInput(stream)
                parsePfb(pfb)
                stream.Dispose()
            Catch ex As Exception
                If (stream IsNot Nothing) Then stream.Dispose()
                Throw ex
            End Try
        End Sub

        '/**
        ' * Create a new object.
        ' * @param in   The input.
        ' * @throws IOException if an IO-error occurs.
        ' */
        Public Sub New(ByVal [in] As InputStream)
            Dim pfb() As Byte = readPfbInput([in])
            parsePfb(pfb)
        End Sub

        '/**
        ' * Parse the pfb-array.
        ' * @param pfb   The pfb-Array
        ' * @throws IOException in an IO-error occurs.
        ' */
        Private Sub parsePfb(ByVal pfb() As Byte)
            Dim [in] As New ByteArrayInputStream(pfb)
            pfbdata = Array.CreateInstance(GetType(Byte), pfb.Length - PFB_HEADER_LENGTH)
            lengths = Array.CreateInstance(GetType(Integer), PFB_RECORDS.Length)
            Dim pointer As Integer = 0
            For records As Integer = 0 To PFB_RECORDS.Length - 1
                If ([in].read() <> START_MARKER) Then
                    Throw New IOException("Start marker missing")
                End If

                If ([in].read() <> PFB_RECORDS(records)) Then
                    Throw New IOException("Incorrect record type")
                End If

                Dim size As Integer = [in].read()
                size += [in].read() << 8
                size += [in].read() << 16
                size += [in].read() << 24
                lengths(records) = size
                Dim got As Integer = [in].read(pfbdata, pointer, size)
                If (got < 0) Then
                    Throw New EndOfStreamException()
                End If
                pointer += got
            Next
        End Sub

        '/**
        ' * Read the pdf input.
        ' * @param in    The input.
        ' * @return Returns the pdf-array.
        ' * @throws IOException if an IO-error occurs.
        ' */
        Private Function readPfbInput(ByVal [in] As InputStream) As Byte()
            ' copy into an array
            Dim out As New ByteArrayOutputStream()
            Dim tmpbuf() As Byte = Array.CreateInstance(GetType(Byte), BUFFER_SIZE)
            Dim amountRead As Integer =
                amountRead = [in].read(tmpbuf)
            While (amountRead > 0)
                out.Write(tmpbuf, 0, amountRead)
                amountRead = [in].read(tmpbuf)
            End While
            Return out.toByteArray()
        End Function

        '/**
        ' * Returns the lengths.
        ' * @return Returns the lengths.
        ' */
        Public Function getLengths() As Integer()
            Return lengths
        End Function

        '/**
        ' * Returns the pfbdata.
        ' * @return Returns the pfbdata.
        ' */
        Public Function getPfbdata() As Byte()
            Return pfbdata
        End Function

        '/**
        ' * Returns the pfb data as stream.
        ' * @return Returns the pfb data as stream.
        ' */
        Public Function getInputStream() As InputStream
            Return New ByteArrayInputStream(pfbdata)
        End Function

        '/**
        ' * Returns the size of the pfb-data.
        ' * @return Returns the size of the pfb-data.
        ' */
        Public Function size() As Integer
            Return pfbdata.Length
        End Function


    End Class

End Namespace

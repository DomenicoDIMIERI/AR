Imports FinSeA.Io
Imports FinSeA.org.apache.pdfbox.util
Imports System.IO

Namespace org.apache.pdfbox.pdfwriter



    '/**
    ' * simple output stream with some minor features for generating "pretty"
    ' * pdf files.
    ' *
    ' * @author Michael Traut
    ' * @version $Revision: 1.5 $
    ' */
    Public Class COSStandardOutputStream
        Inherits FilterOutputStream

        ''' <summary>
        ''' To be used when 2 byte sequence is enforced.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared CRLF() As Byte = StringUtil.getBytes(vbCrLf)

        ''' <summary>
        ''' Line feed character.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly LF() As Byte = StringUtil.getBytes(vbLf)

        ''' <summary>
        ''' standard line separator.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly EOL() As Byte = StringUtil.getBytes(vbLf)

        ' current byte pos in the output stream
        Private pos As Long = 0
        ' flag to prevent generating two newlines in sequence
        Private onNewLine As Boolean = False
        Private fileChannel As FileChannel = Nothing
        Private fileDescriptor As FinSeA.Io.File = Nothing
        Private m_mark As Long = -1

        '/**
        ' * COSOutputStream constructor comment.
        ' *
        ' * @param out The underlying stream to write to.
        ' */
        Public Sub New(ByVal out As OutputStream)
            MyBase.New(out)
            If (TypeOf (out) Is FileOutputStream) Then
                Try
                    fileChannel = DirectCast(out, FileOutputStream).getChannel()
                    fileDescriptor = DirectCast(out, FileOutputStream).getFD()
                    pos = fileChannel.position()
                Catch e As IOException
                    Debug.Print(e.tostring) ' e.printStackTrace()
                End Try
            End If
        End Sub

        '/**
        ' * This will get the current position in the stream.
        ' *
        ' * @return The current position in the stream.
        ' */
        Public Function getPos() As Long
            Return pos
        End Function

        '/**
        ' * This will get the current position in the stream.
        ' *
        ' * @return The current position in the stream.
        ' * @throws IOException 
        ' */
        Public Sub setPos(ByVal pos As Long) 'throws IOException
            If (fileChannel IsNot Nothing) Then
                checkPos()
                Me.pos = pos
                fileChannel.position(pos)
            End If
        End Sub

        '/**
        ' * This will tell if we are on a newline.
        ' *
        ' * @return true If we are on a newline.
        ' */
        Public Function isOnNewLine() As Boolean
            Return onNewLine
        End Function

        '/**
        ' * This will set a flag telling if we are on a newline.
        ' *
        ' * @param newOnNewLine The new value for the onNewLine attribute.
        ' */
        Public Sub setOnNewLine(ByVal newOnNewLine As Boolean)
            onNewLine = newOnNewLine
        End Sub

        '/**
        ' * This will write some byte to the stream.
        ' *
        ' * @param b The source byte array.
        ' * @param off The offset into the array to start writing.
        ' * @param len The number of bytes to write.
        ' *
        ' * @throws IOException If the underlying stream throws an exception.
        ' */
        Public Overrides Sub write(ByVal b() As Byte, ByVal off As Integer, ByVal len As Integer) 'throws IOException
            checkPos()
            setOnNewLine(False)
            out.write(b, off, len)
            pos += len
        End Sub

        '/**
        ' * This will write a single byte to the stream.
        ' *
        ' * @param b The byte to write to the stream.
        ' *
        ' * @throws IOException If there is an error writing to the underlying stream.
        ' */
        Public Overrides Sub write(ByVal b As Integer)  'throws IOException
            checkPos()
            setOnNewLine(False)
            out.write(b)
            pos += 1
        End Sub

        '/**
        ' * This will write a CRLF to the stream.
        ' *
        ' * @throws IOException If there is an error writing the data to the stream.
        ' */
        Public Sub writeCRLF() 'throws IOException
            write(CRLF)
        End Sub

        '/**
        ' * This will write an EOL to the stream.
        ' *
        ' * @throws IOException If there is an error writing to the stream
        ' */
        Public Sub writeEOL() 'throws IOException
            If (Not isOnNewLine()) Then
                write(EOL)
                setOnNewLine(True)
            End If
        End Sub

        '/**
        ' * This will write a Linefeed to the stream.
        ' *
        ' * @throws IOException If there is an error writing to the underlying stream.
        ' */
        Public Sub writeLF() 'throws IOException
            write(LF)
        End Sub

        Public Sub mark() 'throws IOException 
            checkPos()
            Me.m_mark = getPos()
        End Sub

        Public Sub reset() ' throws IOException 
            If (Me.m_mark < 0) Then Return
            setPos(Me.m_mark)
        End Sub

        Private Sub checkPos() 'throws IOException 
            If (fileChannel IsNot Nothing AndAlso fileChannel.position() <> getPos()) Then
                Throw New IOException("OutputStream has an invalid position")
            End If
        End Sub

        Public Function getFileInBytes(ByVal byteRange() As Integer) As Byte() ' throws IOException 
            Return Nothing
        End Function

        Public Function getFilterInputStream(ByVal byteRange() As Integer) As InputStream
            Return New COSFilterInputStream(New FileInputStream(fileDescriptor), byteRange)
        End Function
    End Class

End Namespace

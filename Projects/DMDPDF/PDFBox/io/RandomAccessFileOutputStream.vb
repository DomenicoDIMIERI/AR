Imports FinSeA.Io
Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos


Namespace org.apache.pdfbox.io

    '/**
    ' * This will write to a RandomAccessFile in the filesystem and keep track
    ' * of the m_Position it is writing to and the length of the stream.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.6 $
    ' */
    Public Class RandomAccessFileOutputStream
        Inherits OutputStream


        Private file As RandomAccess
        Private m_Position As Long
        Private lengthWritten As Long = 0
        Private expectedLength As COSBase = Nothing

        '/**
        ' * Constructor to create an output stream that will write to the end of a
        ' * random access file.
        ' *
        ' * @param raf The file to write to.
        ' *
        ' * @throws IOException If there is a problem accessing the raf.
        ' */
        Public Sub New(ByVal raf As RandomAccess) ' throws IOException
            Me.file = raf
            'fsfirst get the m_Position that we will be writing to
            Me.m_Position = raf.length()
        End Sub

        '/**
        ' * This will get the m_Position in the RAF that the stream was written
        ' * to.
        ' *
        ' * @return The m_Position in the raf where the file can be obtained.
        ' */
        Public Function getPosition() As Long
            Return m_Position
        End Function

        '/**
        ' * Get the amount of data that was actually written to the stream, in theory this
        ' * should be the same as the length specified but in some cases it doesn't match.
        ' *
        ' * @return The number of bytes actually written to this stream.
        ' */
        Public Function getLengthWritten() As Long
            Return lengthWritten
        End Function

        '/**
        ' * The number of bytes written to the stream.
        ' *
        ' * @return The number of bytes read to the stream.
        ' */
        Public Function getLength() As Long
            Dim length As Long = -1
            If (TypeOf (expectedLength) Is COSNumber) Then
                length = DirectCast(expectedLength, COSNumber).intValue()
            ElseIf (TypeOf (expectedLength) Is COSObject AndAlso TypeOf (DirectCast(expectedLength, COSObject).getObject()) Is COSNumber) Then
                length = DirectCast(DirectCast(expectedLength, COSObject).getObject(), COSNumber).intValue()
            End If
            If (length = -1) Then
                length = lengthWritten
            End If
            Return length
        End Function

        Public Overrides Sub write(ByVal b() As Byte, ByVal offset As Integer, ByVal length As Integer) ' throws IOException
            file.seek(m_Position + lengthWritten)
            lengthWritten += length
            file.write(b, offset, length)
        End Sub

        Public Overloads Sub write(ByVal b As Byte) 'throws IOException
            file.seek(m_Position + lengthWritten)
            lengthWritten += 1
            file.write(b)
        End Sub

        '/**
        ' * This will get the length that the PDF document specified this stream
        ' * should be.  This may not match the number of bytes read.
        ' *
        ' * @return The expected length.
        ' */
        Public Function getExpectedLength() As COSBase
            Return expectedLength
        End Function

        '/**
        ' * This will set the expected length of this stream.
        ' *
        ' * @param value The expected value.
        ' */
        Public Sub setExpectedLength(ByVal value As COSBase)
            expectedLength = value
        End Sub

        'Public Overrides ReadOnly Property CanRead As Boolean
        '    Get

        '    End Get
        'End Property

        'Public Overrides ReadOnly Property CanSeek As Boolean
        '    Get

        '    End Get
        'End Property

        'Public Overrides ReadOnly Property CanWrite As Boolean
        '    Get

        '    End Get
        'End Property

        'Public Overrides Sub Flush()

        'End Sub

        'Public Overrides ReadOnly Property Length As Long
        '    Get

        '    End Get
        'End Property

        'Public Overrides Property Position As Long

        'Public Overrides Function Read(buffer() As Byte, offset As Integer, count As Integer) As Integer

        'End Function

        'Public Overrides Function Seek(offset As Long, origin As SeekOrigin) As Long

        'End Function

        'Public Overrides Sub SetLength(value As Long)

        'End Sub
    End Class

End Namespace

Imports System.IO
Imports FinSeA.Io

Namespace org.apache.fontbox.ttf

    '/**
    ' * An implementation of the TTFDataStream that goes against a RAF.
    ' * 
    ' * @author Ben Litchfield (ben@benlitchfield.com)
    ' * @version $Revision: 1.2 $
    ' */
    Public Class RAFDataStream
        Inherits TTFDataStream

        Private raf As RandomAccessFile = Nothing
        Private ttfFile As FinSeA.Io.File = Nothing

        '/**
        ' * Constructor.
        ' * 
        ' * @param name The raf file.
        ' * @param mode The mode to open the RAF.
        ' * 
        ' * @throws FileNotFoundException If there is a problem creating the RAF.
        ' * 
        ' * @see RandomAccessFile#RandomAccessFile( String, String )
        ' */
        Public Sub New(ByVal name As String, ByVal mode As String)
            Me.New(New FinSeA.Io.File(name), mode)
        End Sub

        '/**
        ' * Constructor.
        ' * 
        ' * @param file The raf file.
        ' * @param mode The mode to open the RAF.
        ' * 
        ' * @throws FileNotFoundException If there is a problem creating the RAF.
        ' * 
        ' * @see RandomAccessFile#RandomAccessFile( File, String )
        ' */
        Public Sub New(ByVal file As FinSeA.Io.File, ByVal mode As String)
            raf = New RandomAccessFile(file, mode)
            ttfFile = file
        End Sub

        '/**
        ' * Read an signed short.
        ' * 
        ' * @return An signed short.
        ' * @throws IOException If there is an error reading the data.
        ' */
        Public Overrides Function readSignedShort() As Short
            Return raf.readShort()
        End Function

        '/**
        ' * Get the current position in the stream.
        ' * @return The current position in the stream.
        ' * @throws IOException If an error occurs while reading the stream.
        ' */
        Public Overrides Function getCurrentPosition() As Long
            Return raf.getFilePointer()
        End Function

        '/**
        ' * Close the underlying resources.
        ' * 
        ' * @throws IOException If there is an error closing the resources.
        ' */
        Public Overrides Sub close()
            raf.close()
            raf = Nothing
        End Sub

        '/**
        ' * Read an unsigned byte.
        ' * @return An unsigned byte.
        ' * @throws IOException If there is an error reading the data.
        ' */
        Public Overrides Function read() As Integer
            Return raf.read()
        End Function

        '/**
        ' * Read an unsigned short.
        ' * 
        ' * @return An unsigned short.
        ' * @throws IOException If there is an error reading the data.
        ' */
        Public Overrides Function readUnsignedShort() As Integer
            Return raf.readUnsignedShort()
        End Function

        '/**
        ' * Read an unsigned byte.
        ' * @return An unsigned byte.
        ' * @throws IOException If there is an error reading the data.
        ' */
        Public Overrides Function readLong() As Long
            Return raf.readLong()
        End Function

        '/**
        ' * Seek into the datasource.
        ' * 
        ' * @param pos The position to seek to.
        ' * @throws IOException If there is an error seeking to that position.
        ' */
        Public Overrides Sub seek(ByVal pos As Long)
            raf.seek(pos)
        End Sub

        '/**
        ' * @see java.io.InputStream#read( byte[], int, int )
        ' * 
        ' * @param b The buffer to write to.
        ' * @param off The offset into the buffer.
        ' * @param len The length into the buffer.
        ' * 
        ' * @return The number of bytes read.
        ' * 
        ' * @throws IOException If there is an error reading from the stream.
        ' */
        Public Overrides Function read(ByVal b() As Byte, ByVal off As Integer, ByVal len As Integer) As Integer
            Return raf.read(b, off, len)
        End Function

        Public Overrides Function getOriginalData() As InputStream
            Return New FileInputStream(ttfFile)
        End Function

    End Class

End Namespace

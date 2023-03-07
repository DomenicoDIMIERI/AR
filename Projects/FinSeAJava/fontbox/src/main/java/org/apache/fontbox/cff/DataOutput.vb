Imports FinSeA.Io
Imports System.IO

Namespace org.apache.fontbox.cff

    '/**
    ' * 
    ' * @author Villu Ruusmann
    ' * @version $Revision: 1.0 $
    ' */
    Public Class DataOutput
        Implements IDisposable

        Private outputBuffer As New ByteArrayOutputStream()
        Private outputEncoding As String = vbNullString

        '/**
        ' * Constructor.
        ' */
        Public Sub New()
            Me.New("ISO-8859-1")
        End Sub

        '/**
        ' * Constructor with a given encoding.
        ' * @param encoding the encoding to be used for writing
        ' */
        Public Sub New(ByVal encoding As String)
            Me.outputEncoding = encoding
        End Sub

        '/**
        ' * Returns the written data buffer as byte array.
        ' * @return the data buffer as byte array
        ' */
        Public Function getBytes() As Byte()
            Return outputBuffer.toByteArray()
        End Function

        '/**
        ' * Write an int value to the buffer.
        ' * @param value the given value
        ' */
        Public Sub write(ByVal value As Integer)
            outputBuffer.Write(value)
        End Sub

        '/**
        ' * Write a byte array to the buffer.
        ' * @param buffer the given byte array
        ' */
        Public Sub write(ByVal buffer As Byte())
            outputBuffer.Write(buffer, 0, buffer.Length)
        End Sub

        '/**
        ' * Write a part of a byte array to the buffer.
        ' * @param buffer the given byte buffer
        ' * @param offset the offset where to start 
        ' * @param length the amount of bytes to be written from the array
        ' */
        Public Sub write(ByVal buffer() As Byte, ByVal offset As Integer, ByVal length As Integer)
            outputBuffer.Write(buffer, offset, length)
        End Sub

        '/**
        ' * Write the given string to the buffer using the given encoding.
        ' * @param string the given string
        ' * @throws IOException If an error occurs during writing the data to the buffer
        ' */
        Public Sub print(ByVal [string] As String)  'throws IOException
            write(Sistema.Strings.GetBytes([string], outputEncoding))
        End Sub

        '/**
        ' * Write the given string to the buffer using the given encoding.
        ' * A newline is added after the given string
        ' * @param string the given string
        ' * @throws IOException If an error occurs during writing the data to the buffer
        ' */
        Public Sub println(ByVal [string] As String)  'throws IOException
            write(Sistema.Strings.GetBytes([string], outputEncoding))
            write(vbLf)
        End Sub

        '/**
        ' * Add a newline to the given string.
        ' */
        Public Sub println()
            write(vbLf)
        End Sub


        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Overridable Sub Dispose() Implements IDisposable.Dispose
            If (Me.outputBuffer IsNot Nothing) Then Me.outputBuffer.Dispose() : Me.outputBuffer = Nothing
        End Sub


    End Class

End Namespace
Namespace Io

    ''' <summary>
    ''' A ByteArrayInputStream contains an internal buffer that contains bytes that may be read from the stream. An internal counter keeps track of the next byte to be supplied by the read method.
    ''' Closing a ByteArrayInputStream has no effect. The methods in this class can be called after the stream has been closed without generating an IOException.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ByteArrayOutputStream
        Inherits OutputStream

        Public Sub New()
            Me.m_BaseStream = New System.IO.MemoryStream
        End Sub

        Public Sub New(ByVal capacity As Integer)
            Me.m_BaseStream = New System.IO.MemoryStream(capacity)
        End Sub

        Public Sub New(ByVal buffer() As Byte)
            Me.m_BaseStream = New System.IO.MemoryStream(buffer)
        End Sub

        Public Sub New(ByVal baseStream As System.IO.MemoryStream)
            MyBase.New(baseStream)
        End Sub

        Public Sub New(ByVal baseStream As System.IO.MemoryStream, ByVal bufferSize As Integer)
            MyBase.New(baseStream)
        End Sub

        Public Overridable Function toByteArray() As Byte()
            Return DirectCast(Me.m_BaseStream, System.IO.MemoryStream).ToArray
        End Function

        Function reset() As Integer
            Throw New NotImplementedException
        End Function


    End Class

End Namespace
Namespace Io

    ''' <summary>
    ''' A ByteArrayInputStream contains an internal buffer that contains bytes that may be read from the stream. An internal counter keeps track of the next byte to be supplied by the read method.
    ''' Closing a ByteArrayInputStream has no effect. The methods in this class can be called after the stream has been closed without generating an IOException.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ByteArrayInputStream
        Inherits InputStream

        Public Sub New(ByVal buffer() As Byte)
            MyBase.New(New System.IO.MemoryStream(buffer))
        End Sub

        Public Sub New(ByVal baseStream As System.IO.Stream)
            MyBase.New(baseStream)
        End Sub

        Public Sub New(ByVal baseStream As System.IO.Stream, ByVal bufferSize As Integer)
            MyBase.New(baseStream)
        End Sub

    End Class

End Namespace
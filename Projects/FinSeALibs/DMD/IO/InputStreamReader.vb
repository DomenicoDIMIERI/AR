Imports DMD.Sistema

Namespace Io

    '    Public Class InputStreamReader
    'extends Reader

    'An InputStreamReader is a bridge from byte streams to character streams: It reads bytes and decodes them into characters using a specified charset. The charset that it uses may be specified by name or may be given explicitly, or the platform's default charset may be accepted.

    'Each invocation of one of an InputStreamReader's read() methods may cause one or more bytes to be read from the underlying byte-input stream. To enable the efficient conversion of bytes to characters, more bytes may be read ahead from the underlying stream than are necessary to satisfy the current read operation.

    'For top efficiency, consider wrapping an InputStreamReader within a BufferedReader. For example:

    ' BufferedReader in
    '   = new BufferedReader(new InputStreamReader(System.in));

    Public Class InputStreamReader
        Inherits DMD.Io.Reader

        Private m_inputStream As System.IO.Stream
        Private m_CharEncoding As String

        Sub New(ByVal inputStream As System.IO.Stream)
            Me.m_inputStream = inputStream
        End Sub

        Public Overrides Sub close()
            Me.m_inputStream.Close()
        End Sub

        Public Overloads Overrides Function read(cbuf() As Char, off As Integer, len As Integer) As Integer
            Dim buf As Byte() = Arrays.CreateInstance(Of Byte)(1 + UBound(cbuf))
            Dim ret As Integer = Me.m_inputStream.Read(buf, off, len)
            For i As Integer = 0 To ret - 1
                cbuf(i) = Convert.ToChar(buf(i))
            Next
            Return ret
        End Function
    End Class

End Namespace
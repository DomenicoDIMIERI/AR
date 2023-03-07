Namespace Io

    ''' <summary>
    ''' An OutputStreamWriter is a bridge from character streams to byte streams: Characters written to it are encoded into bytes using a specified charset. The charset that it uses may be specified by name or may be given explicitly, or the platform's default charset may be accepted.
    ''' Each invocation of a write() method causes the encoding converter to be invoked on the given character(s). The resulting bytes are accumulated in a buffer before being written to the underlying output stream. The size of this buffer may be specified, but by default it is large enough for most purposes. Note that the characters passed to the write() methods are not buffered.
    ''' For top efficiency, consider wrapping an OutputStreamWriter within a BufferedWriter so as to avoid frequent converter invocations. For example:
    '''  Writer out   = new BufferedWriter(new OutputStreamWriter(System.out));
    ''' A surrogate pair is a character represented by a sequence of two char values: A high surrogate in the range '\uD800' to '\uDBFF' followed by a low surrogate in the range '\uDC00' to '\uDFFF'.
    ''' A malformed surrogate element is a high surrogate that is not followed by a low surrogate or a low surrogate that is not preceded by a high surrogate.
    ''' This class always replaces malformed surrogate elements and unmappable character sequences with the charset's default substitution sequence. The CharsetEncoder class should be used when more control over the encoding process is required.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class OutputStreamWriter
        Inherits Writer

        Private out As OutputStream
        Private charSet As String

        ''' <summary>
        ''' Creates an OutputStreamWriter that uses the default character encoding.
        ''' </summary>
        ''' <param name="out"></param>
        ''' <remarks></remarks>
        Sub New(ByVal out As OutputStream)
            Me.out = out
        End Sub

        ' ''' <summary>
        ' ''' Creates an OutputStreamWriter that uses the given charset.
        ' ''' </summary>
        ' ''' <param name="out"></param>
        ' ''' <param name="cs"></param>
        ' ''' <remarks></remarks>
        'Public Sub New(ByVal out As OutputStream, ByVal cs As Charset)

        'End Sub

        ' ''' <summary>
        ' ''' Creates an OutputStreamWriter that uses the given charset encoder.
        ' ''' </summary>
        ' ''' <param name="out"></param>
        ' ''' <param name="enc"></param>
        ' ''' <remarks></remarks>
        'Public Sub New(ByVal out As OutputStream, ByVal enc As CharsetEncoder)

        'End Sub

        ''' <summary>
        ''' Creates an OutputStreamWriter that uses the named charset.
        ''' </summary>
        ''' <param name="out"></param>
        ''' <param name="charsetName"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal out As OutputStream, ByVal charsetName As String)
            Me.out = out
            Me.charSet = charsetName
        End Sub


        Public Overrides Sub close()
            Me.out.Close()
        End Sub

        Public Overrides Sub flush()
            Me.out.Flush()
        End Sub

        Public Overloads Overrides Sub write(cbuf() As Char, off As Integer, len As Integer)
            Dim str As New String(cbuf)
            Me.out.Write(Sistema.Strings.GetBytes(str, Me.charSet), off, len)
        End Sub

        ''' <summary>
        ''' Returns the name of the character encoding being used by this stream.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getEncoding() As String
            Return Me.charSet
        End Function
    End Class

End Namespace
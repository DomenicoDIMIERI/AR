Namespace Io

    ''' <summary>
    ''' Writes text to a character-output stream, buffering characters so as to provide for the efficient writing of single characters, arrays, and strings.
    ''' The buffer size may be specified, or the default size may be accepted. The default is large enough for most purposes.
    ''' A newLine() method is provided, which uses the platform's own notion of line separator as defined by the system property line.separator. Not all platforms use the newline character ('\n') to terminate lines. Calling this method to terminate each output line is therefore preferred to writing a newline character directly.
    ''' In general, a Writer sends its output immediately to the underlying character or byte stream. Unless prompt output is required, it is advisable to wrap a BufferedWriter around any Writer whose write() operations may be costly, such as FileWriters and OutputStreamWriters. For example,
    '''  PrintWriter out   = new PrintWriter(new BufferedWriter(new FileWriter("foo.out")));
    ''' will buffer the PrintWriter's output to the file. Without buffering, each invocation of a print() method would cause characters to be converted into bytes that would then be written immediately to the file, which can be very inefficient.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class BufferedWriter
        Inherits Writer

        Private out As Writer
        Private _bufferSize As Integer = 512

        ''' <summary>
        ''' Creates a buffered character-output stream that uses a default-sized output buffer.
        ''' </summary>
        ''' <param name="out"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal out As Writer)
            Me.out = out
        End Sub

        ''' <summary>
        ''' Creates a new buffered character-output stream that uses an output buffer of the given size.
        ''' </summary>
        ''' <param name="out"></param>
        ''' <param name="sz"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal out As Writer, ByVal sz As Integer)
            Me.out = out
            Me._bufferSize = sz
        End Sub


        Public Overrides Sub close()
            Me.out.close()
        End Sub

        Public Overrides Sub flush()
            Me.out.flush()
        End Sub

        Public Overloads Overrides Sub write(cbuf() As Char, off As Integer, len As Integer)
            Me.write(cbuf, off, len)
        End Sub

        ''' <summary>
        ''' Writes a line separator. The line separator string is defined by the system property line.separator, and is not necessarily a single newline ('\n') character.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub newLine()
            Me.write(vbLf)
        End Sub


    End Class

End Namespace
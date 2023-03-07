Namespace Io

    ''' <summary>
    ''' public class PrintWriter
    ''' extends Writer
    ''' Prints formatted representations of objects to a text-output stream. This class implements all of the print methods found in PrintStream. It does not contain methods for writing raw bytes, for which a program should use unencoded byte streams.
    ''' Unlike the PrintStream class, if automatic flushing is enabled it will be done only when one of the println, printf, or format methods is invoked, rather than whenever a newline character happens to be output. These methods use the platform's own notion of line separator rather than the newline character.
    ''' Methods in this class never throw I/O exceptions, although some of its constructors may. The client may inquire as to whether any errors have occurred by invoking checkError().
    ''' </summary>
    ''' <remarks></remarks>
    Public Class PrintWriter
        Inherits Writer

        Protected out As Writer
        Private _output As OutputStream
        Private _p2 As Boolean

        Sub New(output As OutputStream, p2 As Boolean)
            ' TODO: Complete member initialization 
            _output = output
            _p2 = p2
        End Sub



        Public Overrides Sub close()

        End Sub

        Public Overrides Sub flush()

        End Sub

        Public Overloads Overrides Sub write(cbuf() As Char, off As Integer, len As Integer)

        End Sub

        Sub println(p1 As String)
            Throw New NotImplementedException
        End Sub

    End Class

End Namespace
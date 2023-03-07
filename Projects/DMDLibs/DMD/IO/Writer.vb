Namespace Io

    ''' <summary>
    ''' Abstract class for writing to character streams. The only methods that a subclass must implement are write(char[], int, int), flush(), and close(). Most subclasses, however, will override some of the methods defined here in order to provide higher efficiency, additional functionality, or both.
    ''' </summary>
    ''' <remarks></remarks>
    Public MustInherit Class Writer
        ''' <summary>
        ''' The object used to synchronize operations on this stream.
        ''' </summary>
        ''' <remarks></remarks>
        Protected lock As New Object

        ''' <summary>
        ''' Creates a new character-stream writer whose critical sections will synchronize on the writer itself.
        ''' </summary>
        ''' <remarks></remarks>
        Protected Sub New()
        End Sub

        ''' <summary>
        ''' Creates a new character-stream writer whose critical sections will synchronize on the given object.
        ''' </summary>
        ''' <remarks></remarks>
        Protected Sub New(ByVal lock As Object)
            Me.lock = lock
        End Sub

        ''' <summary>
        ''' Appends the specified character to this writer.
        ''' </summary>
        ''' <param name="c"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function append(ByVal c As Char) As Writer
            Me.write(Convert.ToByte(c), 0, 1)
            Return Me
        End Function


        ''' <summary>
        ''' Appends the specified character sequence to this writer.
        ''' </summary>
        ''' <param name="csq"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function append(ByVal csq As CharSequence) As Writer
            For Each c As Char In csq
                Me.append(c)
            Next
            Return Me
        End Function

        ''' <summary>
        ''' Appends a subsequence of the specified character sequence to this writer.
        ''' </summary>
        ''' <param name="csq"></param>
        ''' <param name="start"></param>
        ''' <param name="end"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function append(ByVal csq As CharSequence, ByVal start As Integer, ByVal [end] As Integer) As Writer
            For i As Integer = start To [end]
                Me.append(csq.charAt(i))
            Next
            Return Me
        End Function


        ''' <summary>
        ''' Closes the stream, flushing it first.
        ''' </summary>
        ''' <remarks></remarks>
        Public MustOverride Sub close()

        ''' <summary>
        ''' Flushes the stream.
        ''' </summary>
        ''' <remarks></remarks>
        Public MustOverride Sub flush()


        ''' <summary>
        ''' Writes an array of characters.
        ''' </summary>
        ''' <param name="cbuf"></param>
        ''' <remarks></remarks>
        Public Sub write(ByVal cbuf() As Char)
            For Each c As Char In cbuf
                Me.write(Convert.ToByte(c))
            Next
        End Sub

        ''' <summary>
        ''' Writes a portion of an array of characters.
        ''' </summary>
        ''' <param name="cbuf"></param>
        ''' <param name="off"></param>
        ''' <param name="len"></param>
        ''' <remarks></remarks>
        Public MustOverride Sub write(ByVal cbuf() As Char, ByVal off As Integer, ByVal len As Integer)

        ''' <summary>
        ''' Writes a single character.
        ''' </summary>
        ''' <param name="c"></param>
        ''' <remarks></remarks>
        Public Sub write(ByVal c As Integer)
            Me.write(New Char() {Convert.ToChar(c)}, 0, 1)
        End Sub

        ''' <summary>
        ''' Writes a string.
        ''' </summary>
        ''' <param name="str"></param>
        ''' <remarks></remarks>
        Public Sub write(ByVal str As String)
            Me.write(str.ToCharArray)
        End Sub

        ''' <summary>
        ''' Writes a portion of a string.
        ''' </summary>
        ''' <param name="str"></param>
        ''' <param name="off"></param>
        ''' <param name="len"></param>
        ''' <remarks></remarks>
        Public Sub write(ByVal str As String, ByVal off As Integer, ByVal len As Integer)
            Me.write(str.ToCharArray, off, len)
        End Sub

    End Class

End Namespace
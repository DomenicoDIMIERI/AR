Namespace Io



    ''' <summary>
    ''' Abstract class for reading character streams. 
    ''' The only methods that a subclass must implement are read(char[], int, int) and close(). 
    ''' Most subclasses, however, will override some of the methods defined here in order to provide higher efficiency, 
    ''' additional functionality, or both.
    ''' </summary>
    ''' <remarks></remarks>
    Public MustInherit Class Reader
        Implements Readable, Closeable

        ''' <summary>
        ''' The object used to synchronize operations on this stream. For efficiency, a character-stream object may use an object other than itself to protect critical sections. A subclass should therefore use the object in this field rather than this or a synchronized method.
        ''' </summary>
        ''' <remarks></remarks>
        Protected lock As New Object

        ''' <summary>
        ''' Creates a new character-stream reader whose critical sections will synchronize on the reader itself.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        ''' <summary>
        ''' Creates a new character-stream reader whose critical sections will synchronize on the given object.
        ''' </summary>
        ''' <param name="lock"></param>
        ''' <remarks></remarks>
        Sub New(ByVal lock As Object)
            DMD.DMDObject.IncreaseCounter(Me)
            If (lock Is Nothing) Then Throw New ArgumentNullException("lock")
            Me.lock = lock
        End Sub

        ''' <summary>
        ''' Closes the stream and releases any system resources associated with it.
        ''' </summary>
        ''' <remarks></remarks>
        Public MustOverride Sub close() Implements Closeable.close

        ''' <summary>
        ''' Marks the present position in the stream.
        ''' </summary>
        ''' <param name="readAheadLimit"></param>
        ''' <remarks></remarks>
        Public Overridable Sub mark(ByVal readAheadLimit As Integer)
            Throw New NotSupportedException
        End Sub

        ''' <summary>
        ''' Tells whether this stream supports the mark() operation.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function markSupported() As Boolean
            Return False
        End Function

        ''' <summary>
        ''' Reads a single character.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function read() As Integer
            Dim buf(0) As Char
            Dim ret As Integer = Me.read(buf, 0, 1)
            If (ret < 1) Then Return -1
            Return Convert.ToByte(buf(0))
        End Function

        ''' <summary>
        ''' Reads characters into an array.
        ''' </summary>
        ''' <param name="cbuf"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function read(ByVal cbuf() As Char) As Integer
            Return Me.read(cbuf, 0, 1 + UBound(cbuf))
        End Function

        ''' <summary>
        ''' Reads characters into a portion of an array.
        ''' </summary>
        ''' <param name="cbuf"></param>
        ''' <param name="off"></param>
        ''' <param name="len"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public MustOverride Function read(ByVal cbuf() As Char, ByVal off As Integer, ByVal len As Integer) As Integer Implements Readable.read

        ' ''' <summary>
        ' ''' Attempts to read characters into the specified character buffer.
        ' ''' </summary>
        ' ''' <param name="target"></param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Overridable Function read(ByVal target As CharBuffer) As Integer

        'End Function

        ''' <summary>
        ''' Tells whether this stream is ready to be read.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function ready() As Boolean
            Return True
        End Function

        ''' <summary>
        ''' Resets the stream.
        ''' </summary>
        ''' <remarks></remarks>
        Public Overridable Sub reset()

        End Sub

        ''' <summary>
        ''' Skips characters.
        ''' </summary>
        ''' <param name="n"></param>
        ''' <remarks></remarks>
        Public Overridable Function skip(ByVal n As Long) As Long
            Dim buffer(1024) As Char
            Dim res As Integer = 0
            Dim ret As Long = 0

            While (n >= 1024 AndAlso res >= 0)
                res += Me.read(buffer, 0, 1024)
                If (res > 0) Then ret += res
                n -= 1024
            End While
            If (n > 0) Then
                res += Me.read(buffer, 0, n)
                If (res > 0) Then ret += res
            End If
            Return ret
        End Function


        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Overridable Sub Dispose() Implements IDisposable.Dispose
            Me.lock = Nothing
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Namespace
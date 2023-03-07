Namespace Io

    Public Class InputStream
        Inherits System.IO.Stream

        Private [in] As System.IO.Stream
        Protected m_Mark As NInteger

        Protected Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Sub New(ByVal baseStream As System.IO.Stream)
            Me.New
            Me.[in] = baseStream
        End Sub


        Public Overrides ReadOnly Property CanRead As Boolean
            Get
                Return Me.[in].CanRead
            End Get
        End Property

        Public Overrides ReadOnly Property CanSeek As Boolean
            Get
                Return Me.[in].CanSeek
            End Get
        End Property

        Public Overrides ReadOnly Property CanWrite As Boolean
            Get
                Return False ' Me.m_BaseStream.CanWrite
            End Get
        End Property

        Public Overrides Sub Flush()
            Me.[in].Flush()
        End Sub

        Public Overrides ReadOnly Property Length As Long
            Get
                Return Me.[in].Length
            End Get
        End Property

        Public Overrides Property Position As Long
            Get
                Return Me.[in].Position
            End Get
            Set(value As Long)
                Me.[in].Position = value
            End Set
        End Property

        Public Overrides Function Read(buffer() As Byte, offset As Integer, count As Integer) As Integer
            Return Me.[in].Read(buffer, offset, count)
        End Function

        Public Overrides Function Seek(offset As Long, origin As System.IO.SeekOrigin) As Long
            Return Me.[in].Seek(offset, origin)
        End Function

        Public Overridable Overloads Sub Seek(offset As Long)
            Me.Seek(offset, System.IO.SeekOrigin.Begin)
        End Sub


        Public Overrides Sub SetLength(value As Long)
            Me.[in].SetLength(value)
        End Sub

        Public Overrides Sub Write(buffer() As Byte, offset As Integer, count As Integer)
            Me.[in].Write(buffer, offset, count)
        End Sub

        Public Overloads Sub Write(ByVal buffer As Byte())
            Me.[in].Write(buffer, 0, 1 + UBound(buffer))
        End Sub

        ''' <summary>
        ''' Returns an estimate of the number of bytes that can be read (or skipped over) from this input stream without blocking by the next invocation of a method for this input stream.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function available() As Integer
            Return Me.Length - Me.Position
        End Function

        ''' <summary>
        ''' Marks the current position in this input stream.
        ''' </summary>
        ''' <param name="readlimit"></param>
        ''' <remarks></remarks>
        Public Overridable Sub mark(ByVal readlimit As Integer)
            Me.m_Mark = Me.Position
        End Sub

        ''' <summary>
        ''' Tests if this input stream supports the mark and reset methods.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function markSupported() As Boolean
            Return True
        End Function

        ''' <summary>
        ''' Reads the next byte of data from the input stream.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Overloads Function read() As Integer
            Return Me.[in].ReadByte
        End Function

        ''' <summary>
        ''' Reads some number of bytes from the input stream and stores them into the buffer array b.
        ''' </summary>
        ''' <param name="b"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Overloads Function read(ByVal b() As Byte) As Integer
            Return Me.[in].Read(b, 0, 1 + UBound(b))
        End Function

        ''' <summary>
        ''' Repositions this stream to the position at the time the mark method was last called on this input stream.
        ''' </summary>
        ''' <remarks></remarks>
        Public Overridable Sub reset()
            Me.Position = Me.m_Mark
        End Sub

        ''' <summary>
        ''' Skips over and discards n bytes of data from this input stream.
        ''' </summary>
        ''' <param name="n"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function skip(ByVal n As Long) As Long
            If (Me.Position + n <= Me.Length) Then
                Me.Position += n
            Else
                n = Me.Length - Me.Position
                Me.Position = Me.Length
            End If
            Return n
        End Function

        Protected Overrides Sub Dispose(disposing As Boolean)
            MyBase.Dispose(disposing)
            Me.[in].Dispose()
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Namespace
Namespace Io

    Public Class OutputStream
        Inherits System.IO.Stream

        Protected m_BaseStream As System.IO.Stream

        Protected Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Sub New(ByVal baseStream As System.IO.Stream)
            Me.New
            Me.m_BaseStream = baseStream
        End Sub

        Public Overrides ReadOnly Property CanRead As Boolean
            Get
                Return False ' Me.m_BaseStream.CanRead
            End Get
        End Property

        Public Overrides ReadOnly Property CanSeek As Boolean
            Get
                Return Me.m_BaseStream.CanSeek
            End Get
        End Property

        Public Overrides ReadOnly Property CanWrite As Boolean
            Get
                Return Me.m_BaseStream.CanWrite
            End Get
        End Property

        Public Overrides Sub Flush()
            Me.m_BaseStream.Flush()
        End Sub

        Public Overrides ReadOnly Property Length As Long
            Get
                Return Me.m_BaseStream.Length
            End Get
        End Property

        Public Overrides Property Position As Long
            Get
                Return Me.m_BaseStream.Position
            End Get
            Set(value As Long)
                Me.m_BaseStream.Position = value
            End Set
        End Property

        Public Overrides Function Read(buffer() As Byte, offset As Integer, count As Integer) As Integer
            Return Me.m_BaseStream.Read(buffer, offset, count)
        End Function

        Public Overrides Function Seek(offset As Long, origin As System.IO.SeekOrigin) As Long
            Return Me.m_BaseStream.Seek(offset, origin)
        End Function

        Public Overrides Sub SetLength(value As Long)
            Me.m_BaseStream.SetLength(value)
        End Sub

        Public Overrides Sub Write(buffer() As Byte, offset As Integer, count As Integer)
            Me.m_BaseStream.Write(buffer, offset, count)
        End Sub

        Public Overridable Overloads Sub Write(ByVal buffer As Byte())
            Me.m_BaseStream.Write(buffer, 0, 1 + UBound(buffer))
        End Sub

        Public Overridable Overloads Sub Write(ByVal b As Integer)
            Me.m_BaseStream.WriteByte(b)
        End Sub

        Protected Overrides Sub Dispose(disposing As Boolean)
            MyBase.Dispose(disposing)
            Me.m_BaseStream.Dispose()
        End Sub

        Public Function Size() As Integer
            Return Me.Length
        End Function

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Namespace
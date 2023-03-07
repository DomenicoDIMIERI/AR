Namespace Io

    ''' <summary>
    ''' A SequenceInputStream represents the logical concatenation of other input streams. It starts out with an ordered collection of input streams and reads from the first one until end of file is reached, whereupon it reads from the second one, and so on, until end of file is reached on the last of the contained input streams.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class SequenceInputStream
        Inherits InputStream

        Private m_items As New ArrayList

        ''' <summary>
        ''' Initializes a newly created SequenceInputStream by remembering the argument, which must be an Enumeration that produces objects whose run-time type is InputStream.
        ''' </summary>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal e As IEnumerable(Of InputStream))
            For Each item As InputStream In e
                Me.m_items.add(item)
            Next
        End Sub

        ''' <summary>
        ''' Initializes a newly created SequenceInputStream by remembering the two arguments, which will be read in order, first s1 and then s2, to provide the bytes to be read from this SequenceInputStream.
        ''' </summary>
        ''' <param name="s1"></param>
        ''' <param name="s2"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal s1 As InputStream, ByVal s2 As InputStream)
            Me.m_items.add(s1)
            Me.m_items.add(s2)
        End Sub

        Public Overrides ReadOnly Property CanRead As Boolean
            Get
                Return True
            End Get
        End Property

        Public Overrides ReadOnly Property CanSeek As Boolean
            Get
                Return True
            End Get
        End Property

        Public Overrides ReadOnly Property CanWrite As Boolean
            Get
                Return False ' Me.m_BaseStream.CanWrite
            End Get
        End Property

       

    End Class

End Namespace
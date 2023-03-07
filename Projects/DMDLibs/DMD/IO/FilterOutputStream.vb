Namespace Io

    Public Class FilterOutputStream
        Inherits OutputStream

        
        Public Sub New(ByVal out As OutputStream)
            MyBase.New(out)
        End Sub

        Protected ReadOnly Property Out As OutputStream
            Get
                Return Me.m_BaseStream
            End Get
        End Property

    End Class

End Namespace
Namespace Io

    Public Class FilterInputStream
        Inherits InputStream

        Private m_In As InputStream

        Public Sub New(ByVal [in] As InputStream)
            MyBase.New([in])
            Me.m_In = [in]
        End Sub

        Public ReadOnly Property [in] As InputStream
            Get
                Return Me.m_In
            End Get
        End Property


    End Class

End Namespace
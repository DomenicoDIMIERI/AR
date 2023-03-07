Namespace Io

    Public Class BufferedOutputStream
        Inherits OutputStream


        Public Sub New(ByVal baseStream As System.IO.Stream)
            MyBase.New(baseStream)
        End Sub

        Public Sub New(ByVal baseStream As System.IO.Stream, ByVal bufferSize As Integer)
            MyBase.New(baseStream)
        End Sub

    End Class

End Namespace
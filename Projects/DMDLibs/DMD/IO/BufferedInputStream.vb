Namespace Io

    Public Class BufferedInputStream
        Inherits InputStream


        Public Sub New(ByVal baseStream As System.IO.Stream)
            MyBase.New(baseStream)
        End Sub

        Public Sub New(ByVal baseStream As System.IO.Stream, ByVal bufferSize As Integer)
            MyBase.New(baseStream)
        End Sub

        Public Sub New(ByVal fileName As String, ByVal bufferSize As Integer)
            MyBase.New(New System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read))
        End Sub

    End Class

End Namespace
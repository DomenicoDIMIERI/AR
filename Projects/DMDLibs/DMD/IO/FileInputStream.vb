Namespace Io

    Public Class FileInputStream
        Inherits InputStream

        Public Sub New(ByVal baseStream As System.IO.Stream)
            MyBase.New(baseStream)
        End Sub

        Public Sub New(ByVal fileName As String)
            MyBase.New(New System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read))
        End Sub

        Sub New(file As DMD.Io.File)
            MyBase.New(New System.IO.FileStream(file.getFullName(), System.IO.FileMode.Open, System.IO.FileAccess.Read))
        End Sub

    End Class


End Namespace
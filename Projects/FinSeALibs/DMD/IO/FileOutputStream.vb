Namespace Io

    Public Class FileOutputStream
        Inherits OutputStream

        Private _p1 As String

        Public Sub New(ByVal baseStream As System.IO.Stream)
            MyBase.New(baseStream)
        End Sub

        Sub New(ByVal fileName As String)
            MyBase.New(New System.IO.FileStream(fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write))
        End Sub

        Sub New(ByVal file As DMD.Io.File)
            MyBase.New(New System.IO.FileStream(file.getFullName, System.IO.FileMode.Create, System.IO.FileAccess.Write))
        End Sub


        Function getFD() As DMD.Io.File
            Throw New NotImplementedException
        End Function

        Function getChannel() As Object
            Throw New NotImplementedException
        End Function

    End Class


End Namespace
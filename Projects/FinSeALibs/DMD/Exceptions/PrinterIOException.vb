Namespace Exceptions

    Public Class PrinterIOException
        Inherits PrinterException

        Public Sub New()

        End Sub

        Public Sub New(ByVal message As String)
            MyBase.New(message)
        End Sub

        Public Sub New(ByVal message As String, ByVal innerException As System.Exception)
            MyBase.New(message, innerException)
        End Sub

    End Class

End Namespace
Imports System
Imports System.Text
Imports System.IO

Namespace Security

    <Serializable> _
    Public Class InvalidKeyException
        Inherits System.Exception

        Public Sub New()
        End Sub

        Public Sub New(ByVal message As String)
            MyBase.New(message)
        End Sub

        Public Sub New(ByVal message As String, ByVal innerException As System.Exception)
            MyBase.New(message, innerException)
        End Sub

        Sub New(ex As System.Exception)
            MyBase.New(ex.Message, ex)
        End Sub




    End Class

End Namespace
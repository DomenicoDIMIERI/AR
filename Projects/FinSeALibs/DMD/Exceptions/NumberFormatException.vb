Namespace Exceptions

    <Serializable> _
    Public Class NumberFormatException
        Inherits System.Exception

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Sub New(ByVal message As String)
            MyBase.New(message)
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Sub New(ByVal message As String, ByVal innerException As System.Exception)
            MyBase.New(message, innerException)
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Namespace
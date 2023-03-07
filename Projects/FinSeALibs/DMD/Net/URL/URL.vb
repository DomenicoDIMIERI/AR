Namespace Net

    Public Class URL

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)

        End Sub

        Function openStream() As System.IO.Stream
            Throw New NotImplementedException
        End Function

    End Class

End Namespace
Imports System
Imports System.Text
Imports System.IO

Namespace Security

    Public Class AlgorithmParameterGenerator
        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub

        Public Shared Function getInstance(s As String) As AlgorithmParameterGenerator
            Throw New NotImplementedException
        End Function

        Function generateParameters() As AlgorithmParameters
            Throw New NotImplementedException
        End Function


    End Class

End Namespace
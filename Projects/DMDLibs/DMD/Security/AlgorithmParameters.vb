Imports System
Imports System.Text
Imports System.IO

Namespace Security

    Public Class AlgorithmParameters
        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub

        Function getEncoded(p1 As String) As Byte()
            Throw New NotImplementedException
        End Function


    End Class

End Namespace
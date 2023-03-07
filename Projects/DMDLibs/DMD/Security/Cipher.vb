Imports System
Imports System.Text
Imports System.IO

Namespace Security

    Public Class Cipher
        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Shared Function getInstance(p1 As String) As Cipher
            Throw New NotImplementedException
        End Function

        Sub init(p1 As Object, aesKey As SecretKey, ips As AlgorithmParameters)
            Throw New NotImplementedException
        End Sub

        Shared Function DECRYPT_MODE() As Object
            Throw New NotImplementedException
        End Function

        Shared Function ENCRYPT_MODE() As Object
            Throw New NotImplementedException
        End Function

        Function doFinal([in] As Byte()) As Byte()
            Throw New NotImplementedException
        End Function

        Sub init(p1 As Integer, p2 As Byte())
            Throw New NotImplementedException
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Namespace
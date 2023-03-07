Imports System
Imports System.Text
Imports System.IO

Namespace Security

    Public Class ContentInfo

        Private _p1 As Object
        Private _env As EnvelopedData

        Sub New(p1 As Object, env As EnvelopedData)
            ' TODO: Complete member initialization 
            _p1 = p1
            _env = env
        End Sub

        Function getDERObject() As DERObject
            Throw New NotImplementedException
        End Function


    End Class

End Namespace
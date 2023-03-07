Imports System
Imports System.Text
Imports System.IO

Namespace Security

    Public Class KeyStore

        Function size() As Integer
            Throw New NotImplementedException
        End Function

        Function aliases() As IEnumerator(Of String)
            Throw New NotImplementedException
        End Function

        Function getCertificate(keyStoreAlias As String) As X509Certificate
            Throw New NotImplementedException
        End Function

        Function containsAlias([alias] As String) As Boolean
            Throw New NotImplementedException
        End Function

        Function getKey(keyStoreAlias As String, p2 As Char()) As Key
            Throw New NotImplementedException
        End Function

        Shared Function getInstance(p1 As String) As KeyStore
            Throw New NotImplementedException
        End Function

        Sub load(fileInputStream As Io.FileInputStream, p2 As Char())
            Throw New NotImplementedException
        End Sub


    End Class

End Namespace
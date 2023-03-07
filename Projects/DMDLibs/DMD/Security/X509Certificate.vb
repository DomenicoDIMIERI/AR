Imports System
Imports System.Security

Namespace Security

    <Serializable> _
    Public Class X509Certificate
        Inherits Cryptography.X509Certificates.X509Certificate

        Public Sub New()
        End Sub

        Function getTBSCertificate() As Byte()
            Throw New NotImplementedException
        End Function


    End Class

End Namespace
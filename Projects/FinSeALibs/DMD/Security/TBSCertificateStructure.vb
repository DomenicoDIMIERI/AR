Imports System
Imports System.Text
Imports System.IO

Namespace Security

    Public Class TBSCertificateStructure

        Public Shared Function getInstance(dERObject As DERObject) As TBSCertificateStructure
            Throw New NotImplementedException
        End Function

        Function getSubjectPublicKeyInfo() As Object
            Throw New NotImplementedException
        End Function

        Function getIssuer() As Object
            Throw New NotImplementedException
        End Function

        Function getSerialNumber() As Object
            Throw New NotImplementedException
        End Function


    End Class

End Namespace
Imports System
Imports System.Text
Imports System.IO

Namespace Security

    Public Class EncryptedContentInfo

        Private _p1 As Object
        Private _algorithmidentifier As AlgorithmIdentifier
        Private _deroctetstring As DEROctetString

        Sub New(p1 As Object, algorithmidentifier As AlgorithmIdentifier, deroctetstring As DEROctetString)
            ' TODO: Complete member initialization 
            _p1 = p1
            _algorithmidentifier = algorithmidentifier
            _deroctetstring = deroctetstring
        End Sub


    End Class

End Namespace
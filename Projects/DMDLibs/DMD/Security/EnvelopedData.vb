Imports System
Imports System.Text
Imports System.IO

Namespace Security

    Public Class EnvelopedData

        Private _p1 As Object
        Private _derset As DERSet
        Private _encryptedcontentinfo As EncryptedContentInfo
        Private _p4 As Object

        Sub New(p1 As Object, derset As DERSet, encryptedcontentinfo As EncryptedContentInfo, p4 As Object)
            ' TODO: Complete member initialization 
            _p1 = p1
            _derset = derset
            _encryptedcontentinfo = encryptedcontentinfo
            _p4 = p4
        End Sub


    End Class

End Namespace
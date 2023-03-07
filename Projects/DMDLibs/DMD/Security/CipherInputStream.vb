Imports System
Imports System.Text
Imports System.IO

Namespace Security

    Public Class CipherInputStream
        Inherits DMD.Io.InputStream

        Private _decryptCipher As Cipher

        Sub New(data As Io.InputStream, decryptCipher As Cipher)
            MyBase.New(data)
            Me._decryptCipher = decryptCipher
        End Sub




    End Class

End Namespace
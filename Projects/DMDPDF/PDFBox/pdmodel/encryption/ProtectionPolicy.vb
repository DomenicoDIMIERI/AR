Namespace org.apache.pdfbox.pdmodel.encryption

    '/**
    ' * This class represents the protection policy to apply to a document.
    ' *
    ' * Objects implementing this abstract class can be passed to the protect method of PDDocument
    ' * to protect a document.
    ' *
    ' * @see org.apache.pdfbox.pdmodel.PDDocument#protect(ProtectionPolicy)
    ' *
    ' * @author Benoit Guillon (benoit.guillon@snv.jussieu.fr)
    ' * @version $Revision: 1.3 $
    ' */
    Public MustInherit Class ProtectionPolicy

        Private Const DEFAULT_KEY_LENGTH As Integer = 40

        Private encryptionKeyLength As Integer = DEFAULT_KEY_LENGTH

        '/**
        ' * set the length in (bits) of the secret key that will be
        ' * used to encrypt document data.
        ' * The default value is 40 bits, which provides a low security level
        ' * but is compatible with old versions of Acrobat Reader.
        ' *
        ' * @param l the length in bits (must be 40 or 128)
        ' */
        Public Sub setEncryptionKeyLength(ByVal l As Integer)
            If (l <> 40 AndAlso l <> 128) Then
                Throw New RuntimeException("Invalid key length '" & l & "' value must be 40 or 128!")
            End If
            encryptionKeyLength = l
        End Sub

        '/**
        ' * Get the length of the secrete key that will be used to encrypt
        ' * document data.
        ' *
        ' * @return The length (in bits) of the encryption key.
        ' */
        Public Function getEncryptionKeyLength() As Integer
            Return encryptionKeyLength
        End Function

    End Class

End Namespace

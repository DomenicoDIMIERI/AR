Namespace org.apache.pdfbox.pdmodel.encryption


    '/**
    ' *
    ' * Represents the necessary information to decrypt a document protected by
    ' * the standard security handler (password protection).
    ' *
    ' * This is only composed of a password.
    ' *
    ' * The following example shows how to decrypt a document protected with
    ' * the standard security handler:
    ' *
    ' *  <pre>
    ' *  PDDocument doc = PDDocument.load(in);
    ' *  StandardDecryptionMaterial dm = new StandardDecryptionMaterial("password");
    ' *  doc.openProtection(dm);
    ' *  </pre>
    ' *
    ' * @author Benoit Guillon (benoit.guillon@snv.jussieu.fr)
    ' *
    ' * @version $Revision: 1.2 $
    ' */
    Public Class StandardDecryptionMaterial
        Inherits DecryptionMaterial

        Private password As String = ""

        '/**
        ' * Create a new standard decryption material with the given password.
        ' *
        ' * @param pwd The password.
        ' */
        Public Sub New(ByVal pwd As String)
            password = pwd
        End Sub

        '/**
        ' * Returns the password.
        ' *
        ' * @return The password used to decrypt the document.
        ' */
        Public Function getPassword() As String
            Return password
        End Function

    End Class


End Namespace
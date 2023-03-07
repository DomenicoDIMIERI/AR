Imports System.Security
Imports FinSeA
Imports FinSeA.Security
Imports FinSeA.Exceptions

Namespace org.apache.pdfbox.pdmodel.encryption

    '/**
    ' * This class holds necessary information to decrypt a PDF document
    ' * protected by the public key security handler.
    ' *
    ' * To decrypt such a document, we need:
    ' * <ul>
    ' * <li>a valid X509 certificate which correspond to one of the recipient of the document</li>
    ' * <li>the private key corresponding to this certificate
    ' * <li>the password to decrypt the private key if necessary</li>
    ' * </ul>
    ' *
    ' * Objects of this class can be used with the <code>openProtection</code> method of <code>PDDocument</code>.
    ' *
    ' * The following example shows how to decrypt a document using a PKCS#12 certificate
    ' * (typically files with a pfx extension).
    ' *
    ' * <pre>
    ' * PDDocument doc = PDDocument.load(document_path);
    ' * KeyStore ks = KeyStore.getInstance("PKCS12");
    ' * ks.load(new FileInputStream(certificate_path), password.toCharArray());
    ' * PublicKeyDecryptionMaterial dm = new PublicKeyDecryptionMaterial(ks, null, password);
    ' * doc.openProtection(dm);
    ' * </pre>
    ' *
    ' * In this code sample certificate_path contains the path to the PKCS#12 certificate.
    ' *
    ' * @see org.apache.pdfbox.pdmodel.PDDocument#openProtection(DecryptionMaterial)
    ' *
    ' * @author Benoit Guillon (benoit.guillon@snv.jussieu.fr)
    ' * @version $Revision: 1.2 $
    ' */
    Public Class PublicKeyDecryptionMaterial
        Inherits DecryptionMaterial

        Private password As String = vbNullString
        Private keyStore As KeyStore = Nothing
        Private [alias] As String = vbNullString

        '/**
        ' * Create a new public key decryption material.
        ' *
        ' * @param keystore The keystore were the private key and the certificate are
        ' * @param a The alias of the private key and the certificate.
        ' *   If the keystore contains only 1 entry, this parameter can be left null.
        ' * @param pwd The password to extract the private key from the keystore.
        ' */
        Public Sub New(ByVal keystore As KeyStore, ByVal a As String, ByVal pwd As String)
            keystore = keystore
            [alias] = a
            password = pwd
        End Sub


        '/**
        ' * Returns the certificate contained in the keystore.
        ' *
        ' * @return The certificate that will be used to try to open the document.
        ' *
        ' * @throws KeyStoreException If there is an error accessing the certificate.
        ' */

        Public Function getCertificate() As X509Certificate ' throws KeyStoreException
            If (keyStore.size() = 1) Then
                Dim aliases As Global.System.Collections.Generic.IEnumerator(Of String) = keyStore.aliases() 'Enumeration 
                aliases.MoveNext()
                Dim keyStoreAlias As String = aliases.Current
                Return keyStore.getCertificate(keyStoreAlias)
            Else
                If (keyStore.containsAlias([alias])) Then
                    Return keyStore.getCertificate([alias])
                End If
                Throw New KeyStoreException("the keystore does not contain the given alias")
            End If
        End Function

        '/**
        ' * Returns the password given by the user and that will be used
        ' * to open the private key.
        ' *
        ' * @return The password.
        ' */
        Public Function getPassword() As String
            Return password
        End Function

        '/**
        ' * returns The private key that will be used to open the document protection.
        ' * @return The private key.
        ' * @throws KeyStoreException If there is an error accessing the key.
        ' */
        Public Function getPrivateKey() As Key ' throws KeyStoreException
            Try
                If (keyStore.size() = 1) Then
                    Dim aliases As Global.System.Collections.Generic.IEnumerator(Of String) = keyStore.aliases() 'Enumeration 
                    aliases.MoveNext()
                    Dim keyStoreAlias As String = aliases.Current
                    Return keyStore.getKey(keyStoreAlias, password.ToCharArray())
                Else
                    If (keyStore.containsAlias([alias])) Then
                        Return keyStore.getKey([alias], password.ToCharArray())
                    End If
                    Throw New KeyStoreException("the keystore does not contain the given alias")
                End If
            Catch ex As UnrecoverableKeyException
                Throw New KeyStoreException("the private key is not recoverable")
            Catch ex As NoSuchAlgorithmException
                Throw New KeyStoreException("the algorithm necessary to recover the key is not available")
            End Try
        End Function
    End Class

End Namespace

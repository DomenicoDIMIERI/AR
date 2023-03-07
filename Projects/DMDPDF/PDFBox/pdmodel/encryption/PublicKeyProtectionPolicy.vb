Imports FinSeA.Security

'import java.security.cert.X509Certificate;
'import java.util.ArrayList;
'import java.util.Iterator;

Namespace org.apache.pdfbox.pdmodel.encryption


    '/**
    ' * This class represents the protection policy to use to protect
    ' * a document with the public key security handler as described
    ' * in the PDF specification 1.6 p104.
    ' *
    ' * PDF documents are encrypted so that they can be decrypted by
    ' * one or more recipients. Each recipient have its own access permission.
    ' *
    ' * The following code sample shows how to protect a document using
    ' * the public key security handler. In this code sample, <code>doc</code> is
    ' * a <code>PDDocument</code> object.
    ' *
    ' * <pre>
    ' * PublicKeyProtectionPolicy policy = new PublicKeyProtectionPolicy();
    ' * PublicKeyRecipient recip = new PublicKeyRecipient();
    ' * AccessPermission ap = new AccessPermission();
    ' * ap.setCanModify(false);
    ' * recip.setPermission(ap);
    ' *
    ' * // load the recipient's certificate
    ' * InputStream inStream = new FileInputStream(certificate_path);
    ' * CertificateFactory cf = CertificateFactory.getInstance("X.509");
    ' * X509Certificate certificate = (X509Certificate)cf.generateCertificate(inStream);
    ' * inStream.close();
    ' *
    ' * recip.setX509(certificate); // set the recipient's certificate
    ' * policy.addRecipient(recip);
    ' * policy.setEncryptionKeyLength(128); // the document will be encrypted with 128 bits secret key
    ' * doc.protect(policy);
    ' * doc.save(out);
    ' * </pre>
    ' *
    ' *
    ' * @see org.apache.pdfbox.pdmodel.PDDocument#protect(ProtectionPolicy)
    ' * @see AccessPermission
    ' * @see PublicKeyRecipient
    ' *
    ' * @author Benoit Guillon (benoit.guillon@snv.jussieu.fr)
    ' *
    ' * @version $Revision: 1.2 $
    ' */
    Public Class PublicKeyProtectionPolicy
        Inherits ProtectionPolicy

        ''' <summary>
        ''' The list of recipients.
        ''' </summary>
        ''' <remarks></remarks>
        Private recipients As ArrayList = Nothing

        ''' <summary>
        ''' The X509 certificate used to decrypt the current document.
        ''' </summary>
        ''' <remarks></remarks>
        Private decryptionCertificate As X509Certificate

        ''' <summary>
        ''' Constructor for encryption. Just creates an empty recipients list.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            recipients = New ArrayList()
        End Sub

        '/**
        ' * Adds a new recipient to the recipients list.
        ' *
        ' * @param r A new recipient.
        ' */
        Public Sub addRecipient(ByVal r As PublicKeyRecipient)
            recipients.add(r)
        End Sub

        '/**
        ' * Removes a recipient from the recipients list.
        ' *
        ' * @param r The recipient to remove.
        ' *
        ' * @return true If a recipient was found and removed.
        ' */
        Public Function removeRecipient(ByVal r As PublicKeyRecipient) As Boolean ' PublicKeyRecipient
            Return recipients.remove(r)
        End Function

        '/**
        ' * Returns an iterator to browse the list of recipients. Object
        ' * found in this iterator are <code>PublicKeyRecipient</code>.
        ' *
        ' * @return The recipients list iterator.
        ' */
        Public Function getRecipientsIterator() As Global.System.Collections.IEnumerator 'Iterator 
            Return recipients.iterator()
        End Function

        '/**
        ' * Getter of the property <tt>decryptionCertificate</tt>.
        ' *
        ' * @return  Returns the decryptionCertificate.
        ' */
        Public Function getDecryptionCertificate() As X509Certificate
            Return decryptionCertificate
        End Function

        '/**
        ' * Setter of the property <tt>decryptionCertificate</tt>.
        ' *
        ' * @param aDecryptionCertificate The decryption certificate to set.
        ' */
        Public Sub setDecryptionCertificate(ByVal aDecryptionCertificate As X509Certificate)
            Me.decryptionCertificate = aDecryptionCertificate
        End Sub

        '/**
        ' * Returns the number of recipients.
        ' *
        ' * @return The number of recipients.
        ' */
        Public Function getRecipientsNumber() As Integer
            Return recipients.size()
        End Function

    End Class

End Namespace

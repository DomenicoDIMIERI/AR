Imports FinSeA.Io
Imports FinSeA.Security
'import org.bouncycastle.asn1.ASN1InputStream;
'import org.bouncycastle.asn1.DERObject;
'import org.bouncycastle.asn1.DERObjectIdentifier;
'import org.bouncycastle.asn1.DEROctetString;
'import org.bouncycastle.asn1.DEROutputStream;
'import org.bouncycastle.asn1.DERSet;
'import org.bouncycastle.asn1.cms.ContentInfo;
'import org.bouncycastle.asn1.cms.EncryptedContentInfo;
'import org.bouncycastle.asn1.cms.EnvelopedData;
'import org.bouncycastle.asn1.cms.IssuerAndSerialNumber;
'import org.bouncycastle.asn1.cms.KeyTransRecipientInfo;
'import org.bouncycastle.asn1.cms.RecipientIdentifier;
'import org.bouncycastle.asn1.cms.RecipientInfo;
'import org.bouncycastle.asn1.pkcs.PKCSObjectIdentifiers;
'import org.bouncycastle.asn1.x509.AlgorithmIdentifier;
'import org.bouncycastle.asn1.x509.TBSCertificateStructure;
'import org.bouncycastle.cms.CMSEnvelopedData;
'import org.bouncycastle.cms.CMSException;
'import org.bouncycastle.cms.RecipientInformation;
'import org.bouncycastle.jce.provider.BouncyCastleProvider;

Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.exceptions
Imports FinSeA.org.apache.pdfbox.pdmodel
Imports FinSeA.Exceptions

Namespace org.apache.pdfbox.pdmodel.encryption

    '/**
    ' * This class implements the public key security handler
    ' * described in the PDF specification.
    ' *
    ' * @see PDF Spec 1.6 p104
    ' *
    ' * @see PublicKeyProtectionPolicy to see how to protect document with this security handler.
    ' *
    ' * @author Benoit Guillon (benoit.guillon@snv.jussieu.fr)
    ' * @version $Revision: 1.3 $
    ' */
    Public Class PublicKeySecurityHandler
        Inherits SecurityHandler

        ''' <summary>
        ''' The filter name.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const FILTER As String = "Adobe.PubSec"

        Private Const SUBFILTER As String = "adbe.pkcs7.s4"

        Private policy As PublicKeyProtectionPolicy = Nothing

        ''' <summary>
        ''' Constructor.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
        End Sub

        '/**
        ' * Constructor used for encryption.
        ' *
        ' * @param p The protection policy.
        ' */
        Public Sub New(ByVal p As PublicKeyProtectionPolicy)
            policy = p
            Me.keyLength = policy.getEncryptionKeyLength()
        End Sub

        '/**
        ' * Decrypt the document.
        ' *
        ' * @param doc The document to decrypt.
        ' * @param decryptionMaterial The data used to decrypt the document.
        ' *
        ' * @throws CryptographyException If there is an error during decryption.
        ' * @throws IOException If there is an error accessing data.
        ' */
        Public Overrides Sub decryptDocument(ByVal doc As PDDocument, ByVal decryptionMaterial As DecryptionMaterial) 'throws(CryptographyException, IOException)
            Me.document = doc

            Dim dictionary As PDEncryptionDictionary = doc.getEncryptionDictionary()

            prepareForDecryption(dictionary, doc.getDocument().getDocumentID(), decryptionMaterial)

            proceedDecryption()
        End Sub

        '/**
        ' * Prepares everything to decrypt the document.
        ' *
        ' * If {@link #decryptDocument(PDDocument, DecryptionMaterial)} is used, this method is
        ' * called from there. Only if decryption of single objects is needed this should be called instead.
        ' *
        ' * @param encDictionary  encryption dictionary, can be retrieved via {@link PDDocument#getEncryptionDictionary()}
        ' * @param documentIDArray  document id which is returned via {@link COSDocument#getDocumentID()} (not used by this handler)
        ' * @param decryptionMaterial Information used to decrypt the document.
        ' *
        ' * @throws IOException If there is an error accessing data.
        ' * @throws CryptographyException If there is an error with decryption.
        ' */
        Public Overrides Sub prepareForDecryption(ByVal encDictionary As PDEncryptionDictionary, ByVal documentIDArray As COSArray, ByVal decryptionMaterial As DecryptionMaterial) 'throws(CryptographyException, IOException)

            If (encDictionary.getLength() <> 0) Then
                Me.keyLength = encDictionary.getLength()
            End If

            If (Not (TypeOf (decryptionMaterial) Is PublicKeyDecryptionMaterial)) Then
                Throw New CryptographyException("Provided decryption material is not compatible with the document")
            End If

            Dim material As PublicKeyDecryptionMaterial = decryptionMaterial

            Try
	            Dim foundRecipient As Boolean = False

                ' the decrypted content of the enveloped data that match
                ' the certificate in the decryption material provided
                Dim envelopedData() As Byte = Nothing

                ' the bytes of each recipient in the recipients array
                Dim recipientFieldsBytes As Byte()() = Array.CreateInstance(GetType(Byte()), encDictionary.getRecipientsLength())

                Dim recipientFieldsLength As Integer = 0

                For i As Integer = 0 To encDictionary.getRecipientsLength() - 1
                    Dim recipientFieldString As COSString = encDictionary.getRecipientStringAt(i)
                    Dim recipientBytes As Byte() = recipientFieldString.getBytes()
                    Dim data As CMSEnvelopedData = New CMSEnvelopedData(recipientBytes)
                    'Iterator recipCertificatesIt = data.getRecipientInfos().getRecipients().iterator();
                    For Each ri As RecipientInformation In data.getRecipientInfos().getRecipients() ' While (recipCertificatesIt.hasNext())
                        'Dim ri As RecipientInformation = recipCertificatesIt.next()
                        ' Impl: if a matching certificate was previously found it is an error,
                        ' here we just don't care about it
                        If (ri.getRID().match(material.getCertificate()) AndAlso foundRecipient) Then
                            foundRecipient = True
                            envelopedData = ri.getContent(material.getPrivateKey(), "BC")
                            Exit For
                        End If
                    Next
                    recipientFieldsBytes(i) = recipientBytes
                    recipientFieldsLength += recipientBytes.Length
                Next
                If (Not foundRecipient OrElse envelopedData Is Nothing) Then
                    Throw New CryptographyException("The certificate matches no recipient entry")
                End If
                If (envelopedData.Length <> 24) Then
                    Throw New CryptographyException("The enveloped data does not contain 24 bytes")
                End If
                '// now envelopedData contains:
                '// - the 20 bytes seed
                '// - the 4 bytes of permission for the current user

                Dim accessBytes() As Byte = Array.CreateInstance(GetType(Byte), 4)
                Array.Copy(envelopedData, 20, accessBytes, 0, 4)

                currentAccessPermission = New AccessPermission({accessBytes(0), accessBytes(1), accessBytes(2), accessBytes(3)})
                currentAccessPermission.setReadOnly()

                ' what we will put in the SHA1 = the seed + each byte contained in the recipients array
                Dim sha1Input() As Byte = Array.CreateInstance(GetType(Byte), recipientFieldsLength + 20)

                ' put the seed in the sha1 input
                Array.Copy(envelopedData, 0, sha1Input, 0, 20)

                ' put each bytes of the recipients array in the sha1 input
                Dim sha1InputOffset As Integer = 20
                For i As Integer = 0 To recipientFieldsBytes.Length - 1
                    Array.Copy(recipientFieldsBytes(i), 0, sha1Input, sha1InputOffset, recipientFieldsBytes(i).Length)
                    sha1InputOffset += recipientFieldsBytes(i).Length
                Next

                Dim md As MessageDigest = MessageDigest.getInstance("SHA-1")
                Dim mdResult() As Byte = md.digest(sha1Input)

                ' we have the encryption key ...
                encryptionKey = Array.CreateInstance(GetType(Byte), Me.keyLength \ 8)
                Array.Copy(mdResult, 0, encryptionKey, 0, Me.keyLength \ 8)
            Catch e As CMSException
                Throw New CryptographyException(e)
            Catch e As KeyStoreException
                Throw New CryptographyException(e)
            Catch e As NoSuchProviderException
                Throw New CryptographyException(e)
            Catch e As NoSuchAlgorithmException
                Throw New CryptographyException(e)
            End Try
        End Sub

        '/**
        ' * Prepare the document for encryption.
        ' *
        ' * @param doc The document that will be encrypted.
        ' *
        ' * @throws CryptographyException If there is an error while encrypting.
        ' */
        Public Overrides Sub prepareDocumentForEncryption(ByVal doc As PDDocument) 'throws CryptographyException

            Try
                Security.Providers.addProvider(New BouncyCastleProvider())

                Dim dictionary As PDEncryptionDictionary = doc.getEncryptionDictionary()
                If (dictionary Is Nothing) Then
                    dictionary = New PDEncryptionDictionary()
                End If

                dictionary.setFilter(FILTER)
                dictionary.setLength(Me.keyLength)
                dictionary.setVersion(2)
                dictionary.setSubFilter(SUBFILTER)

                Dim recipientsField As Byte()() = Array.CreateInstance(GetType(Byte()), policy.getRecipientsNumber())

                ' create the 20 bytes seed

                Dim seed() As Byte = Array.CreateInstance(GetType(Byte), 20)

                Dim key As KeyGenerator = KeyGenerator.getInstance("AES")
                key.init(192, New SecureRandom())
                Dim sk As SecretKey = key.generateKey()
                Array.Copy(sk.getEncoded(), 0, seed, 0, 20) ' create the 20 bytes seed


                Dim it As Iterator = policy.getRecipientsIterator()
                Dim i As Integer = 0


                While (it.hasNext())
                    Dim recipient As PublicKeyRecipient = it.next()
                    Dim certificate As X509Certificate = recipient.getX509()
                    Dim permission As Integer = recipient.getPermission().getPermissionBytesForPublicKey()

                    Dim pkcs7input() As Byte = Array.CreateInstance(GetType(Byte), 24)
                    Dim one As Byte = permission
                    Dim two As Byte = (permission >> 8) '>>>
                    Dim three As Byte = (permission >> 16) '>>>
                    Dim four As Byte = (permission >> 24) '>>>

                    Array.Copy(seed, 0, pkcs7input, 0, 20) ' put this seed in the pkcs7 input

                    pkcs7input(20) = four
                    pkcs7input(21) = three
                    pkcs7input(22) = two
                    pkcs7input(23) = one

                    Dim obj As DERObject = createDERForRecipient(pkcs7input, certificate)

                    Dim baos As ByteArrayOutputStream = New ByteArrayOutputStream()

                    Dim k As DEROutputStream = New DEROutputStream(baos)

                    k.writeObject(obj)

                    recipientsField(i) = baos.toByteArray()

                    i += 1
                End While


                dictionary.setRecipients(recipientsField)

                Dim sha1InputLength As Integer = seed.Length

                For j As Integer = 0 To dictionary.getRecipientsLength() - 1
                    Dim [string] As COSString = dictionary.getRecipientStringAt(j)
                    sha1InputLength += [string].getBytes.Length
                Next


                Dim sha1Input() As Byte = Array.CreateInstance(GetType(Byte), sha1InputLength)

                Array.Copy(seed, 0, sha1Input, 0, 20)

                Dim sha1InputOffset As Integer = 20


                For j As Integer = 0 To dictionary.getRecipientsLength() - 1
                    Dim [string] As COSString = dictionary.getRecipientStringAt(j)
                    Array.Copy([string].getBytes(), 0, sha1Input, sha1InputOffset, [string].getBytes().Length)
                    sha1InputOffset += [string].getBytes().Length
                Next

                Dim md As MessageDigest = MessageDigest.getInstance("SHA-1")

                Dim mdResult() As Byte = md.digest(sha1Input)

                Me.encryptionKey = Array.CreateInstance(GetType(Byte), Me.keyLength \ 8)
                Array.Copy(mdResult, 0, Me.encryptionKey, 0, Me.keyLength \ 8)

                doc.setEncryptionDictionary(dictionary)
                doc.getDocument().setEncryptionDictionary(dictionary.encryptionDictionary)

            Catch ex As NoSuchAlgorithmException
                Throw New CryptographyException(ex)
            Catch ex As NoSuchProviderException
                Throw New CryptographyException(ex)
            Catch e As Exception
                Debug.Print(e.ToString)
                Throw New CryptographyException(e)
            End Try

        End Sub

        Private Function createDERForRecipient(ByVal [in] As Byte(), ByVal cert As X509Certificate) As DERObject 'throws(IOException, GeneralSecurityException)
            Dim s As String = "1.2.840.113549.3.2"

            Dim algorithmparametergenerator As AlgorithmParameterGenerator = algorithmparametergenerator.getInstance(s)
            Dim algorithmparameters As AlgorithmParameters = algorithmparametergenerator.generateParameters()
            Dim bytearrayinputstream As ByteArrayInputStream = New ByteArrayInputStream(algorithmparameters.getEncoded("ASN.1"))
            Dim asn1inputstream As ASN1InputStream = New ASN1InputStream(bytearrayinputstream)
            Dim derobject As DERObject = asn1inputstream.readObject()
            Dim keygenerator As KeyGenerator = keygenerator.getInstance(s)
            keygenerator.init(128)
            Dim secretkey As SecretKey = keygenerator.generateKey()
            Dim cipher As Cipher = cipher.getInstance(s)
            cipher.init(1, secretkey, algorithmparameters)
            Dim abyte1() As Byte = cipher.doFinal([in])
            Dim deroctetstring As DEROctetString = New DEROctetString(abyte1)
            Dim keytransrecipientinfo As KeyTransRecipientInfo = computeRecipientInfo(cert, secretkey.getEncoded())
            Dim derset As DERSet = New DERSet(New RecipientInfo(keytransrecipientinfo))
            Dim algorithmidentifier As AlgorithmIdentifier = New AlgorithmIdentifier(New DERObjectIdentifier(s), derobject)
            Dim encryptedcontentinfo As EncryptedContentInfo = New EncryptedContentInfo(PKCSObjectIdentifiers.data, algorithmidentifier, deroctetstring)
            Dim env As EnvelopedData = New EnvelopedData(Nothing, derset, encryptedcontentinfo, Nothing)
            Dim contentinfo As ContentInfo = New ContentInfo(PKCSObjectIdentifiers.envelopedData, env)
            Return contentinfo.getDERObject()
        End Function

        Private Function computeRecipientInfo(ByVal x509certificate As X509Certificate, ByVal abyte0() As Byte) As KeyTransRecipientInfo 'throws(GeneralSecurityException, IOException)
            Dim asn1inputstream As ASN1InputStream = New ASN1InputStream(New ByteArrayInputStream(x509certificate.getTBSCertificate()))
            Dim tbscertificatestructure As TBSCertificateStructure = tbscertificatestructure.getInstance(asn1inputstream.readObject())
            Dim algorithmidentifier As AlgorithmIdentifier = tbscertificatestructure.getSubjectPublicKeyInfo().getAlgorithmId()
            Dim issuerandserialnumber As IssuerAndSerialNumber = New IssuerAndSerialNumber(tbscertificatestructure.getIssuer(), tbscertificatestructure.getSerialNumber().getValue())
            Dim cipher As Cipher = cipher.getInstance(algorithmidentifier.getObjectId().getId())
            cipher.init(1, x509certificate.getPublicKey())
            Dim deroctetstring As DEROctetString = New DEROctetString(cipher.doFinal(abyte0))
            Dim recipId As RecipientIdentifier = New RecipientIdentifier(issuerandserialnumber)
            Return New KeyTransRecipientInfo(recipId, algorithmidentifier, deroctetstring)
        End Function

    End Class

End Namespace

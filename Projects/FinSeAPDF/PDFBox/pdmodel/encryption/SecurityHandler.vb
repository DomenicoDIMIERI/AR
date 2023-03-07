Imports System.IO
Imports FinSeA.Io
Imports FinSeA.Security
Imports FinSeA.Exceptions
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.encryption
Imports FinSeA.org.apache.pdfbox.exceptions
Imports FinSeA.org.apache.pdfbox.pdmodel
 

Namespace org.apache.pdfbox.pdmodel.encryption


    '/**
    ' * This class represents a security handler as described in the PDF specifications.
    ' * A security handler is responsible of documents protection.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @author Benoit Guillon (benoit.guillon@snv.jussieu.fr)
    ' *
    ' */

    Public MustInherit Class SecurityHandler


#Region "CONSTANTS"

        Private Const DEFAULT_KEY_LENGTH As Integer = 40

        ''' <summary>
        ''' See 7.6.2, page 58, PDF 32000-1:2008
        ''' </summary>
        ''' <remarks></remarks>
        Private Shared ReadOnly AES_SALT() As Byte = {&H73, &H41, &H6C, &H54}

#End Region

        ''' <summary>
        ''' The value of V field of the Encryption dictionary.
        ''' </summary>
        ''' <remarks></remarks>
        Protected version As Integer

        ''' <summary>
        ''' The length of the secret key used to encrypt the document.
        ''' </summary>
        ''' <remarks></remarks>
        Protected keyLength As Integer = DEFAULT_KEY_LENGTH

        ''' <summary>
        ''' The encryption key that will used to encrypt / decrypt.
        ''' </summary>
        ''' <remarks></remarks>
        Protected encryptionKey As Byte()

        ''' <summary>
        ''' The document whose security is handled by this security handler.
        ''' </summary>
        ''' <remarks></remarks>
        Protected document As PDDocument

        ''' <summary>
        ''' The RC4 implementation used for cryptographic functions.
        ''' </summary>
        ''' <remarks></remarks>
        Protected rc4 As ARCFour = New ARCFour()

        Private objects As [Set](Of COSBase) = New HashSet(Of COSBase)

        Private potentialSignatures As [Set](Of COSDictionary) = New HashSet(Of COSDictionary)

        ''' <summary>
        ''' If true, AES will be used.
        ''' </summary>
        ''' <remarks></remarks>
        Private aes As Boolean

        ''' <summary>
        ''' The access permission granted to the current user for the document. These permissions are computed during decryption and are in read only mode.
        ''' </summary>
        ''' <remarks></remarks>
        Protected currentAccessPermission As AccessPermission = Nothing

        '/**
        ' * Prepare the document for encryption.
        ' *
        ' * @param doc The document that will be encrypted.
        ' *
        ' * @throws CryptographyException If there is an error while preparing.
        ' * @throws IOException If there is an error with the document.
        ' */
        Public MustOverride Sub prepareDocumentForEncryption(ByVal doc As PDDocument) 'throws CryptographyException, IOException;

        '/**
        ' * Prepares everything to decrypt the document.
        ' * 
        ' * If {@link #decryptDocument(PDDocument, DecryptionMaterial)} is used, this method is
        ' * called from there. Only if decryption of single objects is needed this should be called instead.
        ' *
        ' * @param encDictionary  encryption dictionary, can be retrieved via {@link PDDocument#getEncryptionDictionary()}
        ' * @param documentIDArray  document id which is returned via {@link COSDocument#getDocumentID()}
        ' * @param decryptionMaterial Information used to decrypt the document.
        ' *
        ' * @throws IOException If there is an error accessing data.
        ' * @throws CryptographyException If there is an error with decryption.
        ' */
        Public MustOverride Sub prepareForDecryption(ByVal encDictionary As PDEncryptionDictionary, ByVal documentIDArray As COSArray, ByVal decryptionMaterial As DecryptionMaterial)  'throws CryptographyException, IOException;

        '/**
        ' * Prepare the document for decryption.
        ' *
        ' * @param doc The document to decrypt.
        ' * @param mat Information required to decrypt the document.
        ' * @throws CryptographyException If there is an error while preparing.
        ' * @throws IOException If there is an error with the document.
        ' */
        Public MustOverride Sub decryptDocument(ByVal doc As PDDocument, ByVal mat As DecryptionMaterial) 'throws CryptographyException, IOException;

        '/**
        ' * This method must be called by an implementation of this class to really proceed
        ' * to decryption.
        ' *
        ' * @throws IOException If there is an error in the decryption.
        ' * @throws CryptographyException If there is an error in the decryption.
        ' */
        Protected Sub proceedDecryption() 'throws IOException, CryptographyException
            Dim trailer As COSDictionary = document.getDocument().getTrailer()
            Dim fields As COSArray = trailer.getObjectFromPath("Root/AcroForm/Fields")

            ' We need to collect all the signature dictionaries, for some
            ' reason the 'Contents' entry of signatures is not really encrypted
            If (fields IsNot Nothing) Then
                For i As Integer = 0 To fields.size() - 1
                    Dim field As COSDictionary = fields.getObject(i)
                    If (field IsNot Nothing) Then
                        addDictionaryAndSubDictionary(potentialSignatures, field)
                    Else
                        Throw New IOException("Could not decypt document, object not found.")
                    End If
                Next
            End If

            Dim allObjects As List(Of COSObject) = document.getDocument().getObjects()
            'Dim objectIter As Iterator(Of COSObject) = allObjects.iterator()
            For Each item As COSObject In allObjects ' While (objectIter.hasNext())
                decryptObject(item) 'objectIter.next())
            Next 'End While 
            document.setEncryptionDictionary(Nothing)
        End Sub

        Private Sub addDictionaryAndSubDictionary(ByVal [set] As [Set](Of COSDictionary), ByVal dic As COSDictionary)
            If (dic IsNot Nothing) Then ' in case dictionary is part of object stream we have null value here
                [set].add(dic)
                Dim kids As COSArray = dic.getDictionaryObject(COSName.KIDS)
                Dim i As Integer = 0
                While (kids IsNot Nothing AndAlso i < kids.size())
                    addDictionaryAndSubDictionary([set], kids.getObject(i))
                    i += 1
                End While
                Dim value As COSBase = dic.getDictionaryObject(COSName.V)
                If (TypeOf (value) Is COSDictionary) Then
                    addDictionaryAndSubDictionary([set], value)
                End If
            End If
        End Sub

        '/**
        ' * Encrypt a set of data.
        ' *
        ' * @param objectNumber The data object number.
        ' * @param genNumber The data generation number.
        ' * @param data The data to encrypt.
        ' * @param output The output to write the encrypted data to.
        ' * @throws CryptographyException If there is an error during the encryption.
        ' * @throws IOException If there is an error reading the data.
        ' * @deprecated While this works fine for RC4 encryption, it will never decrypt AES data
        ' *             You should use encryptData(objectNumber, genNumber, data, output, decrypt)
        ' *             which can do everything.  This function is just here for compatibility
        ' *             reasons and will be removed in the future.
        ' */
        Public Sub encryptData(ByVal objectNumber As Long, ByVal genNumber As Long, ByVal data As InputStream, ByVal output As OutputStream)  'throws(CryptographyException, IOException)
            ' default to encrypting since the function is named "encryptData"
            encryptData(objectNumber, genNumber, data, output, False)
        End Sub

        '/**
        ' * Encrypt a set of data.
        ' *
        ' * @param objectNumber The data object number.
        ' * @param genNumber The data generation number.
        ' * @param data The data to encrypt.
        ' * @param output The output to write the encrypted data to.
        ' * @param decrypt true to decrypt the data, false to encrypt it
        ' *
        ' * @throws CryptographyException If there is an error during the encryption.
        ' * @throws IOException If there is an error reading the data.
        ' */
        Public Sub encryptData(ByVal objectNumber As Long, ByVal genNumber As Long, ByVal data As InputStream, ByVal output As OutputStream, ByVal decrypt As Boolean) 'throws(CryptographyException, IOException)
            If (aes AndAlso Not decrypt) Then
                Throw New ArgumentException("AES encryption is not yet implemented.")
            End If

            Dim newKey() As Byte = Array.CreateInstance(GetType(Byte), encryptionKey.Length + 5)
            Array.Copy(encryptionKey, 0, newKey, 0, encryptionKey.Length)
            ' PDF 1.4 reference pg 73
            ' step 1
            ' we have the reference

            ' step 2
            newKey(newKey.Length - 5) = (objectNumber And &HFF)
            newKey(newKey.Length - 4) = ((objectNumber >> 8) And &HFF)
            newKey(newKey.Length - 3) = ((objectNumber >> 16) And &HFF)
            newKey(newKey.Length - 2) = (genNumber And &HFF)
            newKey(newKey.Length - 1) = ((genNumber >> 8) And &HFF)

            ' step 3
            Dim digestedKey() As Byte = Nothing
            Try
                Dim md As MessageDigest = MessageDigest.getInstance("MD5")
                md.update(newKey)
                If (aes) Then
                    md.update(AES_SALT)
                End If
                digestedKey = md.digest()
            Catch e As NoSuchAlgorithmException
                Throw New CryptographyException(e)
            End Try

            ' step 4
            Dim length As Integer = Math.Min(newKey.Length, 16)
            Dim finalKey() As Byte = Array.CreateInstance(GetType(Byte), length)
            Array.Copy(digestedKey, 0, finalKey, 0, length)

            If (aes) Then
                Dim iv() As Byte = Array.CreateInstance(GetType(Byte), 16)

                data.read(iv)

                Try
                    Dim decryptCipher As Cipher = Cipher.getInstance("AES/CBC/PKCS5Padding")

                    Dim aesKey As SecretKey = New SecretKeySpec(finalKey, "AES")

                    Dim ips As IvParameterSpec = New IvParameterSpec(iv)

                    decryptCipher.init(IIf(decrypt, Cipher.DECRYPT_MODE, Cipher.ENCRYPT_MODE), aesKey, ips)

                    Dim cipherStream As CipherInputStream = New CipherInputStream(data, decryptCipher)

                    Try
                        Dim buffer() As Byte = Array.CreateInstance(GetType(Byte), 4096)
                        Dim n As Integer
                        n = cipherStream.read(buffer)
                        While (n > 0) 'for (int n = 0; -1 != (n = cipherStream.read(buffer));)
                            output.Write(buffer, 0, n)
                            n = cipherStream.read(buffer)
                        End While
                    Finally
                        cipherStream.close()
                    End Try
                Catch e As InvalidKeyException
                    Throw New WrappedIOException(e)
                Catch e As InvalidAlgorithmParameterException
                    Throw New WrappedIOException(e)
                Catch e As NoSuchAlgorithmException
                    Throw New WrappedIOException(e)
                Catch e As NoSuchPaddingException
                    Throw New WrappedIOException(e)
                End Try
            Else
                rc4.setKey(finalKey)
                rc4.write(data, output)
            End If

            output.Flush()
        End Sub

        '/**
        ' * This will decrypt an object in the document.
        ' *
        ' * @param object The object to decrypt.
        ' *
        ' * @throws CryptographyException If there is an error decrypting the stream.
        ' * @throws IOException If there is an error getting the stream data.
        ' */
        Private Sub decryptObject(ByVal [object] As COSObject) 'throws CryptographyException, IOException
            Dim objNum As Long = [object].getObjectNumber().intValue()
            Dim genNum As Long = [object].getGenerationNumber().intValue()
            Dim base As COSBase = [object].getObject()
            decrypt(base, objNum, genNum)
        End Sub

        '/**
        ' * This will dispatch to the correct method.
        ' *
        ' * @param obj The object to decrypt.
        ' * @param objNum The object number.
        ' * @param genNum The object generation Number.
        ' *
        ' * @throws CryptographyException If there is an error decrypting the stream.
        ' * @throws IOException If there is an error getting the stream data.
        ' */
        Private Sub decrypt(ByVal obj As COSBase, ByVal objNum As Long, ByVal genNum As Long) 'throws CryptographyException, IOException
            If (Not objects.contains(obj)) Then
                objects.add(obj)

                If (TypeOf (obj) Is COSString) Then
                    decryptString(obj, objNum, genNum)
                ElseIf (TypeOf (obj) Is COSStream) Then
                    decryptStream(obj, objNum, genNum)
                ElseIf (TypeOf (obj) Is COSDictionary) Then
                    decryptDictionary(obj, objNum, genNum)
                ElseIf (TypeOf (obj) Is COSArray) Then
                    decryptArray(obj, objNum, genNum)
                End If
            End If
        End Sub

        '/**
        ' * This will decrypt a stream.
        ' *
        ' * @param stream The stream to decrypt.
        ' * @param objNum The object number.
        ' * @param genNum The object generation number.
        ' *
        ' * @throws CryptographyException If there is an error getting the stream.
        ' * @throws IOException If there is an error getting the stream data.
        ' */
        Public Sub decryptStream(ByVal stream As COSStream, ByVal objNum As Long, ByVal genNum As Long) 'throws CryptographyException, IOException
            decryptDictionary(stream, objNum, genNum)
            Dim encryptedStream As InputStream = stream.getFilteredStream()
            encryptData(objNum, genNum, encryptedStream, stream.createFilteredStream(), True) '/* decrypt */
        End Sub

        '/**
        ' * This will encrypt a stream, but not the dictionary as the dictionary is
        ' * encrypted by visitFromString() in COSWriter and we don't want to encrypt
        ' * it twice.
        ' *
        ' * @param stream The stream to decrypt.
        ' * @param objNum The object number.
        ' * @param genNum The object generation number.
        ' *
        ' * @throws CryptographyException If there is an error getting the stream.
        ' * @throws IOException If there is an error getting the stream data.
        ' */
        Public Sub encryptStream(ByVal stream As COSStream, ByVal objNum As Long, ByVal genNum As Long) 'throws CryptographyException, IOException
            Dim encryptedStream As InputStream = stream.getFilteredStream()
            encryptData(objNum, genNum, encryptedStream, stream.createFilteredStream(), False) '/* encrypt */
        End Sub

        '/**
        ' * This will decrypt a dictionary.
        ' *
        ' * @param dictionary The dictionary to decrypt.
        ' * @param objNum The object number.
        ' * @param genNum The object generation number.
        ' *
        ' * @throws CryptographyException If there is an error decrypting the document.
        ' * @throws IOException If there is an error creating a new string.
        ' */
        Private Sub decryptDictionary(ByVal dictionary As COSDictionary, ByVal objNum As Long, ByVal genNum As Long)  'throws CryptographyException, IOException()
            For Each entry As Map.Entry(Of COSName, COSBase) In dictionary.entrySet()
                Dim value As COSBase = entry.Value
                ' within a dictionary only strings and streams have to be decrypted
                If (TypeOf (value) Is COSString OrElse TypeOf (value) Is COSStream OrElse TypeOf (value) Is COSArray) Then
                    ' if we are a signature dictionary and contain a Contents entry then
                    ' we don't decrypt it.
                    If (Not (entry.Key.getName().Equals("Contents") AndAlso TypeOf (value) Is COSString AndAlso potentialSignatures.contains(dictionary))) Then
                        decrypt(entry.Value, objNum, genNum)
                    End If
                End If
            Next
        End Sub

        '/**
        ' * This will decrypt a string.
        ' *
        ' * @param string the string to decrypt.
        ' * @param objNum The object number.
        ' * @param genNum The object generation number.
        ' *
        ' * @throws CryptographyException If an error occurs during decryption.
        ' * @throws IOException If an error occurs writing the new string.
        ' */
        Public Sub decryptString(ByVal [string] As COSString, ByVal objNum As Long, ByVal genNum As Long) 'throws CryptographyException, IOException
            Dim data As ByteArrayInputStream = New ByteArrayInputStream([string].getBytes)
            Dim buffer As ByteArrayOutputStream = New ByteArrayOutputStream()
            encryptData(objNum, genNum, data, buffer, True) '/* decrypt */
            [string].reset()
            [string].append(buffer.toByteArray())
            data.Dispose()
            buffer.Dispose()
        End Sub

        '/**
        ' * This will decrypt an array.
        ' *
        ' * @param array The array to decrypt.
        ' * @param objNum The object number.
        ' * @param genNum The object generation number.
        ' *
        ' * @throws CryptographyException If an error occurs during decryption.
        ' * @throws IOException If there is an error accessing the data.
        ' */
        Private Sub decryptArray(ByVal array As COSArray, ByVal objNum As Long, ByVal genNum As Long) 'throws CryptographyException, IOException
            For i As Integer = 0 To array.size() - 1
                decrypt(array.get(i), objNum, genNum)
            Next
        End Sub

        '/**
        ' * Getter of the property <tt>keyLength</tt>.
        ' * @return  Returns the keyLength.
        ' * @uml.property  name="keyLength"
        ' */
        Public Function getKeyLength() As Integer
            Return keyLength
        End Function

        '/**
        ' * Setter of the property <tt>keyLength</tt>.
        ' *
        ' * @param keyLen  The keyLength to set.
        ' */
        Public Sub setKeyLength(ByVal keyLen As Integer)
            Me.keyLength = keyLen
        End Sub

        '/**
        ' * Returns the access permissions that were computed during document decryption.
        ' * The returned object is in read only mode.
        ' *
        ' * @return the access permissions or null if the document was not decrypted.
        ' */
        Public Function getCurrentAccessPermission() As AccessPermission
            Return currentAccessPermission
        End Function

        '/**
        ' * True if AES is used for encryption and decryption.
        ' * 
        ' * @return true if AEs is used 
        ' */
        Public Function isAES() As Boolean
            Return aes
        End Function

        '/**
        ' * Set to true if AES for encryption and decryption should be used.
        ' * 
        ' * @param aes if true AES will be used 
        ' * 
        ' */
        Public Sub setAES(ByVal aesValue As Boolean)
            aes = aesValue
        End Sub
    End Class

End Namespace

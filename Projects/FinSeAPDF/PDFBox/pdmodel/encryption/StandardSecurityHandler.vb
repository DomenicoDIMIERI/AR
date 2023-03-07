Imports FinSeA.Sistema
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
    ' *
    ' * The class implements the standard security handler as decribed
    ' * in the PDF specifications. This security handler protects document
    ' * with password.
    ' *
    ' * @see StandardProtectionPolicy to see how to protect document with this security handler.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @author Benoit Guillon (benoit.guillon@snv.jussieu.fr)
    ' *
    ' */

    Public Class StandardSecurityHandler
        Inherits SecurityHandler

        ''' <summary>
        ''' Type of security handler.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const FILTER As String = "Standard"

        Private Const DEFAULT_VERSION As Integer = 1

        Private Const DEFAULT_REVISION As Integer = 3

        Private revision As Integer = DEFAULT_REVISION

        Private policy As StandardProtectionPolicy

        'Private rc4 As ARCFour = New ARCFour()

        ''' <summary>
        ''' Protection policy class for this handler.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly PROTECTION_POLICY_CLASS As System.Type = GetType(StandardProtectionPolicy)

        ''' <summary>
        ''' Standard padding for encryption.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly ENCRYPT_PADDING() As Byte = _
        {
            &H28, &HBF, &H4E, &H5E, &H4E, _
            &H75, &H8A, &H41, &H64, &H0, _
            &H4E, &H56, &HFF, &HFA, &H1, _
            &H8, &H2E, &H2E, &H0, &HB6, _
            &HD0, &H68, &H3E, &H80, &H2F, _
            &HC, &HA9, &HFE, &H64, &H53, _
            &H69, &H7A _
        }

        '/**
        ' * Constructor.
        ' */
        Public Sub New()
        End Sub

        '/**
        ' * Constructor used for encryption.
        ' *
        ' * @param p The protection policy.
        ' */
        Public Sub New(ByVal p As StandardProtectionPolicy)
            policy = p
            keyLength = policy.getEncryptionKeyLength()
        End Sub


        '/**
        ' * Computes the version number of the StandardSecurityHandler
        ' * regarding the encryption key length.
        ' * See PDF Spec 1.6 p 93
        ' *
        ' * @return The computed cersion number.
        ' */
        Private Function computeVersionNumber() As Integer
            If (keyLength = 40) Then
                Return DEFAULT_VERSION
            End If
            Return 2
        End Function

        '/**
        ' * Computes the revision version of the StandardSecurityHandler to
        ' * use regarding the version number and the permissions bits set.
        ' * See PDF Spec 1.6 p98
        ' *
        ' * @return The computed revision number.
        ' */
        Private Function computeRevisionNumber() As Integer
            'If (version < 2 AndAlso Not policy.getPermissions().hasAnyRevision3PermissionSet()) Then
            '    Return 2
            'End If
            'If (version = 2 OrElse version = 3 OrElse policy.getPermissions().hasAnyRevision3PermissionSet()) Then
            '    Return 3
            'End If
            Return 4
        End Function

        '/**
        ' * Decrypt the document.
        ' *
        ' * @param doc The document to be decrypted.
        ' * @param decryptionMaterial Information used to decrypt the document.
        ' *
        ' * @throws IOException If there is an error accessing data.
        ' * @throws CryptographyException If there is an error with decryption.
        ' */
        Public Overrides Sub decryptDocument(ByVal doc As PDDocument, ByVal decryptionMaterial As DecryptionMaterial) 'throws(CryptographyException, IOException)
            document = doc

            Dim dictionary As PDEncryptionDictionary = document.getEncryptionDictionary()
            Dim documentIDArray As COSArray = document.getDocument().getDocumentID()

            prepareForDecryption(dictionary, documentIDArray, decryptionMaterial)

            Me.proceedDecryption()
        End Sub

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
        Public Overrides Sub prepareForDecryption(ByVal encDictionary As PDEncryptionDictionary, ByVal documentIDArray As COSArray, ByVal decryptionMaterial As DecryptionMaterial) 'throws(CryptographyException, IOException)
            If (Not (TypeOf (decryptionMaterial) Is StandardDecryptionMaterial)) Then
                Throw New CryptographyException("Provided decryption material is not compatible with the document")
            End If

            Dim material As StandardDecryptionMaterial = decryptionMaterial

            Dim password As String = material.getPassword()
            If (password = vbNullString) Then
                password = ""
            End If

            Dim dicPermissions As Integer = encDictionary.getPermissions()
            Dim dicRevision As Integer = encDictionary.getRevision()
            Dim dicLength As Integer = encDictionary.getLength() / 8

            'some documents may have not document id, see
            'test\encryption\encrypted_doc_no_id.pdf
            Dim documentIDBytes() As Byte = Nothing
            If (documentIDArray IsNot Nothing AndAlso documentIDArray.size() >= 1) Then
                Dim id As COSString = documentIDArray.getObject(0)
                documentIDBytes = id.getBytes()
            Else
                documentIDBytes = Array.CreateInstance(GetType(Byte), 0)
            End If

            ' we need to know whether the meta data was encrypted for password calculation
            Dim encryptMetadata As Boolean = encDictionary.isEncryptMetaData()

            Dim u() As Byte = encDictionary.getUserKey()
            Dim o() As Byte = encDictionary.getOwnerKey()

            Dim _isUserPassword As Boolean = isUserPassword(Strings.GetBytes(password, "ISO-8859-1"), _
                                                            u, _
                                                            o, _
                                                            dicPermissions, _
                                                            documentIDBytes, _
                                                            dicRevision, _
                                                            dicLength, _
                                                            encryptMetadata)
            Dim _isOwnerPassword As Boolean = isOwnerPassword(Strings.GetBytes(password, "ISO-8859-1"), _
                                                            u, _
                                                            o, _
                                                            dicPermissions, _
                                                            documentIDBytes, _
                                                            dicRevision, _
                                                            dicLength, _
                                                            encryptMetadata)

            If (_isUserPassword) Then
                currentAccessPermission = New AccessPermission(dicPermissions)
                encryptionKey = computeEncryptedKey(Strings.GetBytes(password, "ISO-8859-1"), o, dicPermissions, documentIDBytes, dicRevision, dicLength, encryptMetadata)
            ElseIf (_isOwnerPassword) Then
                currentAccessPermission = AccessPermission.getOwnerAccessPermission()
                Dim computedUserPassword() As Byte = getUserPassword(Strings.GetBytes(password, "ISO-8859-1"), o, dicRevision, dicLength)
                encryptionKey = computeEncryptedKey(computedUserPassword, o, dicPermissions, documentIDBytes, dicRevision, dicLength, encryptMetadata)
            Else
                Throw New CryptographyException("Error: The supplied password does not match either the owner or user password in the document.")
            End If

            ' detect whether AES encryption is used. This assumes that the encryption algo is 
            ' stored in the PDCryptFilterDictionary
            Dim stdCryptFilterDictionary As PDCryptFilterDictionary = encDictionary.getStdCryptFilterDictionary()

            If (stdCryptFilterDictionary IsNot Nothing) Then
                Dim cryptFilterMethod As COSName = stdCryptFilterDictionary.getCryptFilterMethod()
                If (cryptFilterMethod IsNot Nothing) Then
                    setAES("AESV2" = UCase(cryptFilterMethod.getName()))
                End If
            End If
        End Sub

        '/**
        ' * Prepare document for encryption.
        ' *
        ' * @param doc The documeent to encrypt.
        ' *
        ' * @throws IOException If there is an error accessing data.
        ' * @throws CryptographyException If there is an error with decryption.
        ' */
        Public Overrides Sub prepareDocumentForEncryption(ByVal doc As PDDocument) 'throws CryptographyException, IOException
            document = doc
            Dim encryptionDictionary As PDEncryptionDictionary = document.getEncryptionDictionary()
            If (encryptionDictionary Is Nothing) Then
                encryptionDictionary = New PDEncryptionDictionary()
            End If
            version = computeVersionNumber()
            revision = computeRevisionNumber()
            encryptionDictionary.setFilter(FILTER)
            encryptionDictionary.setVersion(version)
            encryptionDictionary.setRevision(revision)
            encryptionDictionary.setLength(keyLength)

            Dim ownerPassword As String = policy.getOwnerPassword()
            Dim userPassword As String = policy.getUserPassword()
            If (ownerPassword = vbNullString) Then
                ownerPassword = ""
            End If
            If (userPassword = vbNullString) Then
                userPassword = ""
            End If

            Dim permissionInt As Integer = policy.getPermissions().getPermissionBytes()

            encryptionDictionary.setPermissions(permissionInt)

            Dim length As Integer = keyLength / 8

            Dim idArray As COSArray = document.getDocument().getDocumentID()

            'check if the document has an id yet.  If it does not then
            'generate one
            If (idArray Is Nothing OrElse idArray.size() < 2) Then
                idArray = New COSArray()
                Try
                    Dim md As MessageDigest = MessageDigest.getInstance("MD5")
                    Dim time As BigInteger = New BigInteger(Timer)
                    md.update(time.toByteArray())
                    md.update(Strings.GetBytes(ownerPassword, "ISO-8859-1"))
                    md.update(Strings.GetBytes(userPassword, "ISO-8859-1"))
                    md.update(Strings.GetBytes(document.getDocument().ToString()))
                    Dim id1() As Byte = md.digest(Strings.GetBytes(Me.ToString(), "ISO-8859-1"))
                    Dim idString As COSString = New COSString()
                    idString.append(id1)
                    idArray.add(idString)
                    idArray.add(idString)
                    document.getDocument().setDocumentID(idArray)
                Catch e As NoSuchAlgorithmException
                    Throw New CryptographyException(e)
                Catch e As IOException
                    Throw New CryptographyException(e)
                End Try
            End If

            Dim id As COSString = idArray.getObject(0)

            Dim o() As Byte = computeOwnerPassword(Strings.GetBytes(ownerPassword, "ISO-8859-1"), Strings.GetBytes(userPassword, "ISO-8859-1"), revision, length)
            Dim u() As Byte = computeUserPassword(Strings.GetBytes(userPassword, "ISO-8859-1"), o, permissionInt, id.getBytes(), revision, length, True)

            encryptionKey = computeEncryptedKey(Strings.GetBytes(userPassword, "ISO-8859-1"), o, permissionInt, id.getBytes(), revision, length, True)

            encryptionDictionary.setOwnerKey(o)
            encryptionDictionary.setUserKey(u)

            document.setEncryptionDictionary(encryptionDictionary)
            document.getDocument().setEncryptionDictionary(encryptionDictionary.getCOSDictionary())

        End Sub

        '/**
        ' * Check for owner password.
        ' *
        ' * @param ownerPassword The owner password.
        ' * @param u The u entry of the encryption dictionary.
        ' * @param o The o entry of the encryption dictionary.
        ' * @param permissions The set of permissions on the document.
        ' * @param id The document id.
        ' * @param encRevision The encryption algorithm revision.
        ' * @param length The encryption key length.
        ' * @param encryptMetadata The encryption metadata
        ' *
        ' * @return True If the ownerPassword param is the owner password.
        ' *
        ' * @throws CryptographyException If there is an error during encryption.
        ' * @throws IOException If there is an error accessing data.
        ' */
        Public Function isOwnerPassword(ByVal ownerPassword() As Byte, ByVal u() As Byte, ByVal o() As Byte, ByVal permissions As Integer, ByVal id() As Byte, ByVal encRevision As Integer, ByVal length As Integer, ByVal encryptMetadata As Boolean) As Boolean  'throws(CryptographyException, IOException)
            Dim userPassword() As Byte = getUserPassword(ownerPassword, o, encRevision, length)
            Return isUserPassword(userPassword, u, o, permissions, id, encRevision, length, encryptMetadata)
        End Function

        '/**
        ' * Get the user password based on the owner password.
        ' *
        ' * @param ownerPassword The plaintext owner password.
        ' * @param o The o entry of the encryption dictionary.
        ' * @param encRevision The encryption revision number.
        ' * @param length The key length.
        ' *
        ' * @return The u entry of the encryption dictionary.
        ' *
        ' * @throws CryptographyException If there is an error generating the user password.
        ' * @throws IOException If there is an error accessing data while generating the user password.
        ' */
        Public Function getUserPassword(ByVal ownerPassword() As Byte, ByVal o() As Byte, ByVal encRevision As Integer, ByVal length As Long) As Byte() 'throws(CryptographyException, IOException)
            Try
                Dim result As ByteArrayOutputStream = New ByteArrayOutputStream()

                '3.3 STEP 1
                Dim ownerPadded() As Byte = truncateOrPad(ownerPassword)

                '3.3 STEP 2
                Dim md As MessageDigest = MessageDigest.getInstance("MD5")
                md.update(ownerPadded)
                Dim digest() As Byte = md.digest()

                '3.3 STEP 3
                If (encRevision = 3 OrElse encRevision = 4) Then
                    For i As Integer = 0 To 50 - 1
                        md.reset()
                        md.update(digest)
                        digest = md.digest()
                    Next
                End If
                If (encRevision = 2 And length <> 5) Then
                    Throw New CryptographyException("Error: Expected length=5 actual=" & length)
                End If

                '3.3 STEP 4
                Dim rc4Key() As Byte = Array.CreateInstance(GetType(Byte), length)
                Array.Copy(digest, 0, rc4Key, 0, length)

                '3.7 step 2
                If (encRevision = 2) Then
                    rc4.setKey(rc4Key)
                    rc4.write(o, result)
                ElseIf (encRevision = 3 OrElse encRevision = 4) Then
                    Dim iterationKey() As Byte = Array.CreateInstance(GetType(Byte), rc4Key.Length)
                    Dim otemp() As Byte = Array.CreateInstance(GetType(Byte), o.Length) 'sm
                    Array.Copy(o, 0, otemp, 0, o.Length) 'sm
                    rc4.write(o, result) 'sm

                    For i As Integer = 19 To 0 Step -1
                        Array.Copy(rc4Key, 0, iterationKey, 0, rc4Key.Length)
                        For j As Integer = 0 To iterationKey.length - 1
                            iterationKey(j) = (iterationKey(j) Xor CByte(i))
                        Next
                        rc4.setKey(iterationKey)
                        result.reset() 'sm
                        rc4.write(otemp, result) 'sm
                        otemp = result.toByteArray() 'sm
                    Next
                End If
                Return result.toByteArray()
            Catch e As NoSuchAlgorithmException
                Throw New CryptographyException(e)
            End Try
        End Function

        '/**
        ' * Compute the encryption key.
        ' *
        ' * @param password The password to compute the encrypted key.
        ' * @param o The o entry of the encryption dictionary.
        ' * @param permissions The permissions for the document.
        ' * @param id The document id.
        ' * @param encRevision The revision of the encryption algorithm.
        ' * @param length The length of the encryption key.
        ' * @param encryptMetadata The encryption metadata
        ' *
        ' * @return The encrypted key bytes.
        ' *
        ' * @throws CryptographyException If there is an error with encryption.
        ' */
        Public Function computeEncryptedKey(ByVal password As Byte(), ByVal o() As Byte, ByVal permissions As Integer, ByVal id() As Byte, ByVal encRevision As Integer, ByVal length As Integer, ByVal encryptMetadata As Boolean) As Byte() 'throws CryptographyException
            Dim result() As Byte = Array.CreateInstance(GetType(Byte), length)
            Try
                'PDFReference 1.4 pg 78
                'step1
                Dim padded() As Byte = truncateOrPad(password)

                'step 2
                Dim md As MessageDigest = MessageDigest.getInstance("MD5")
                md.update(padded)

                'step 3
                md.update(o)

                'step 4
                Dim zero As Byte = (permissions >> 0) And &HFF
                Dim one As Byte = (permissions >> 8) And &HFF
                Dim two As Byte = (permissions >> 16) And &HFF
                Dim three As Byte = (permissions >> 24) And &HFF

                md.update(zero)
                md.update(one)
                md.update(two)
                md.update(three)

                'step 5
                md.update(id)

                '//(Security handlers of revision 4 or greater) If document metadata is not being encrypted, 
                '//pass 4 bytes with the value 0xFFFFFFFF to the MD5 hash function.
                '//see 7.6.3.3 Algorithm 2 Step f of PDF 32000-1:2008
                If (encRevision = 4 AndAlso Not encryptMetadata) Then
                    md.update({&HFF, &HFF, &HFF, &HFF})
                End If

                Dim digest() As Byte = md.digest()

                'step 6
                If (encRevision = 3 OrElse encRevision = 4) Then
                    For i As Integer = 0 To 50 - 1
                        md.reset()
                        md.update(digest, 0, length)
                        digest = md.digest()
                    Next
                End If

                'step 7
                If (encRevision = 2 AndAlso length <> 5) Then
                    Throw New CryptographyException("Error: length should be 5 when revision is two actual=" & length)
                End If
                Array.Copy(digest, 0, result, 0, length)
            Catch e As NoSuchAlgorithmException
                Throw New CryptographyException(e)
            End Try
            Return result
        End Function

        '/**
        ' * This will compute the user password hash.
        ' *
        ' * @param password The plain text password.
        ' * @param o The owner password hash.
        ' * @param permissions The document permissions.
        ' * @param id The document id.
        ' * @param encRevision The revision of the encryption.
        ' * @param length The length of the encryption key.
        ' * @param encryptMetadata The encryption metadata
        ' *
        ' * @return The user password.
        ' *
        ' * @throws CryptographyException If there is an error computing the user password.
        ' * @throws IOException If there is an IO error.
        ' */

        Public Function computeUserPassword(ByVal password() As Byte, ByVal o() As Byte, ByVal permissions As Integer, ByVal id() As Byte, ByVal encRevision As Integer, ByVal length As Integer, ByVal encryptMetadata As Boolean) As Byte()  'throws(CryptographyException, IOException)
            Dim result As ByteArrayOutputStream = New ByteArrayOutputStream()
            'STEP 1
            Dim encryptionKey() As Byte = computeEncryptedKey(password, o, permissions, id, encRevision, length, encryptMetadata)

            If (encRevision = 2) Then
                'STEP 2
                rc4.setKey(encryptionKey)
                rc4.write(ENCRYPT_PADDING, result)
            ElseIf (encRevision = 3 OrElse encRevision = 4) Then
                Try
                    'STEP 2
                    Dim md As MessageDigest = MessageDigest.getInstance("MD5")
                    'md.update( truncateOrPad( password ) );
                    md.update(ENCRYPT_PADDING)

                    'STEP 3
                    md.update(id)
                    result.Write(md.digest())

                    'STEP 4 and 5
                    Dim iterationKey() As Byte = Array.CreateInstance(GetType(Byte), encryptionKey.Length)
                    For i As Integer = 0 To 20 - 1
                        Array.Copy(encryptionKey, 0, iterationKey, 0, iterationKey.Length)
                        For j As Integer = 0 To iterationKey.Length - 1
                            iterationKey(j) = (iterationKey(j) Xor i) And &HFF
                        Next
                        rc4.setKey(iterationKey)
                        Dim input As ByteArrayInputStream = New ByteArrayInputStream(result.toByteArray())
                        result.reset()
                        rc4.write(input, result)
                        input.Dispose()
                    Next

                    'step 6
                    Dim finalResult(32 - 1) As Byte '= new byte[32];
                    Array.Copy(result.toByteArray(), 0, finalResult, 0, 16)
                    Array.Copy(ENCRYPT_PADDING, 0, finalResult, 16, 16)
                    result.reset()
                    result.Write(finalResult)
                Catch e As NoSuchAlgorithmException
                    Throw New CryptographyException(e)
                End Try
            End If
            Return result.toByteArray()
        End Function

        '/**
        ' * Compute the owner entry in the encryption dictionary.
        ' *
        ' * @param ownerPassword The plaintext owner password.
        ' * @param userPassword The plaintext user password.
        ' * @param encRevision The revision number of the encryption algorithm.
        ' * @param length The length of the encryption key.
        ' *
        ' * @return The o entry of the encryption dictionary.
        ' *
        ' * @throws CryptographyException If there is an error with encryption.
        ' * @throws IOException If there is an error accessing data.
        ' */
        Public Function computeOwnerPassword(ByVal ownerPassword() As Byte, ByVal userPassword() As Byte, ByVal encRevision As Integer, ByVal length As Integer) As Byte()  'final byte[]throws(CryptographyException, IOException)
            Try
                'STEP 1
                Dim ownerPadded() As Byte = truncateOrPad(ownerPassword)

                'STEP 2
                Dim md As MessageDigest = MessageDigest.getInstance("MD5")
                md.update(ownerPadded)
                Dim digest() As Byte = md.digest()

                'STEP 3
                If (encRevision = 3 OrElse encRevision = 4) Then
                    For i As Integer = 0 To 50 - 1
                        md.reset()
                        md.update(digest, 0, length)
                        digest = md.digest()
                    Next
                End If
                If (encRevision = 2 AndAlso length <> 5) Then
                    Throw New CryptographyException("Error: Expected length=5 actual=" & length)
                End If

                'STEP 4
                Dim rc4Key As Byte() = Array.CreateInstance(GetType(Byte), length)
                Array.Copy(digest, 0, rc4Key, 0, length)

                'STEP 5
                Dim paddedUser() As Byte = truncateOrPad(userPassword)


                'STEP 6
                rc4.setKey(rc4Key)
                Dim crypted As ByteArrayOutputStream = New ByteArrayOutputStream()
                rc4.write(New ByteArrayInputStream(paddedUser), crypted)


                'STEP 7
                If (encRevision = 3 OrElse encRevision = 4) Then
                    Dim iterationKey() As Byte = Array.CreateInstance(GetType(Byte), rc4Key.Length)
                    For i As Integer = 1 To 20 - 1
                        Array.Copy(rc4Key, 0, iterationKey, 0, rc4Key.Length)
                        For j As Integer = 0 To iterationKey.Length - 1
                            iterationKey(j) = (iterationKey(j) Xor i) And &HFF
                        Next
                        rc4.setKey(iterationKey)
                        Dim input As ByteArrayInputStream = New ByteArrayInputStream(crypted.toByteArray())
                        crypted.reset()
                        rc4.write(input, crypted)
                        input.Dispose()
                    Next
                End If

                'STEP 8
                Return crypted.toByteArray()
                crypted.Dispose()
            Catch e As NoSuchAlgorithmException
                Throw New CryptographyException(e.Message)
            End Try
        End Function


        '/**
        ' * This will take the password and truncate or pad it as necessary.
        ' *
        ' * @param password The password to pad or truncate.
        ' *
        ' * @return The padded or truncated password.
        ' */
        Private Function truncateOrPad(ByVal password() As Byte) As Byte()
            Dim padded() As Byte = Array.CreateInstance(GetType(Byte), ENCRYPT_PADDING.Length)
            Dim bytesBeforePad As Integer = Math.Min(password.Length, padded.Length)
            Array.Copy(password, 0, padded, 0, bytesBeforePad)
            Array.Copy(ENCRYPT_PADDING, 0, padded, bytesBeforePad, ENCRYPT_PADDING.Length - bytesBeforePad)
            Return padded
        End Function

        '/**
        ' * Check if a plaintext password is the user password.
        ' *
        ' * @param password The plaintext password.
        ' * @param u The u entry of the encryption dictionary.
        ' * @param o The o entry of the encryption dictionary.
        ' * @param permissions The permissions set in the the PDF.
        ' * @param id The document id used for encryption.
        ' * @param encRevision The revision of the encryption algorithm.
        ' * @param length The length of the encryption key.
        ' * @param encryptMetadata The encryption metadata
        ' *
        ' * @return true If the plaintext password is the user password.
        ' *
        ' * @throws CryptographyException If there is an error during encryption.
        ' * @throws IOException If there is an error accessing data.
        ' */
        Public Function isUserPassword(ByVal password() As Byte, ByVal u() As Byte, ByVal o() As Byte, ByVal permissions As Integer, ByVal id() As Byte, ByVal encRevision As Integer, ByVal length As Integer, ByVal encryptMetadata As Boolean) As Boolean ' throws(CryptographyException, IOException)
            Dim matches As Boolean = False
            'STEP 1
            Dim computedValue As Byte() = computeUserPassword(password, o, permissions, id, encRevision, length, encryptMetadata)
            If (encRevision = 2) Then
                'STEP 2
                matches = Sistema.Arrays.Compare(u, computedValue) = 0
            ElseIf (encRevision = 3 OrElse encRevision = 4) Then
                'STEP 2
                matches = arraysEqual(u, computedValue, 16)
            Else
                Throw New IOException("Unknown Encryption Revision " & encRevision)
            End If
            Return matches
        End Function

        '/**
        ' * Check if a plaintext password is the user password.
        ' *
        ' * @param password The plaintext password.
        ' * @param u The u entry of the encryption dictionary.
        ' * @param o The o entry of the encryption dictionary.
        ' * @param permissions The permissions set in the the PDF.
        ' * @param id The document id used for encryption.
        ' * @param encRevision The revision of the encryption algorithm.
        ' * @param length The length of the encryption key.
        ' * @param encryptMetadata The encryption metadata
        ' *
        ' * @return true If the plaintext password is the user password.
        ' *
        ' * @throws CryptographyException If there is an error during encryption.
        ' * @throws IOException If there is an error accessing data.
        ' */
        Public Function isUserPassword(ByVal password As String, ByVal u() As Byte, ByVal o() As Byte, ByVal permissions As Integer, ByVal id() As Byte, ByVal encRevision As Integer, ByVal length As Integer, ByVal encryptMetadata As Boolean) As Boolean ' throws(CryptographyException, IOException)
            Return isUserPassword(Strings.GetBytes(password, "ISO-8859-1"), u, o, permissions, id, encRevision, length, encryptMetadata)
        End Function

        '/**
        ' * Check for owner password.
        ' *
        ' * @param password The owner password.
        ' * @param u The u entry of the encryption dictionary.
        ' * @param o The o entry of the encryption dictionary.
        ' * @param permissions The set of permissions on the document.
        ' * @param id The document id.
        ' * @param encRevision The encryption algorithm revision.
        ' * @param length The encryption key length.
        ' * @param encryptMetadata The encryption metadata
        ' *
        ' * @return True If the ownerPassword param is the owner password.
        ' *
        ' * @throws CryptographyException If there is an error during encryption.
        ' * @throws IOException If there is an error accessing data.
        ' */
        Public Function isOwnerPassword(ByVal password As String, ByVal u() As Byte, ByVal o() As Byte, ByVal permissions As Integer, ByVal id() As Byte, ByVal encRevision As Integer, ByVal length As Integer, ByVal encryptMetadata As Boolean) As Boolean ' throws(CryptographyException, IOException)
            Return isOwnerPassword(Strings.GetBytes(password, "ISO-8859-1"), u, o, permissions, id, encRevision, length, encryptMetadata)
        End Function

        Private Shared Function arraysEqual(ByVal first() As Byte, ByVal second() As Byte, ByVal count As Integer) As Boolean
            ' both arrays have to have a minimum length of count
            If (first.Length < count OrElse second.Length < count) Then
                Return False
            End If
            For i As Integer = 0 To count - 1
                If (first(i) <> second(i)) Then
                    Return False
                End If
            Next
            Return True
        End Function

    End Class

End Namespace

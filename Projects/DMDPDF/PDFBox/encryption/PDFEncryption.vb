Imports System.IO
Imports FinSeA.Io
Imports FinSeA.org.apache.pdfbox.exceptions
Imports FinSeA.Security


Namespace org.apache.pdfbox.encryption

    '/**
    ' * This class will deal with PDF encryption algorithms.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.15 $
    ' *
    ' * @deprecated use the new security layer instead
    ' *
    ' * @see org.apache.pdfbox.pdmodel.encryption.StandardSecurityHandler
    ' */
    Public NotInheritable Class PDFEncryption
        Private rc4 As New ARCFour()
        '/**
        ' * The encryption padding defined in the PDF 1.4 Spec algorithm 3.2.
        ' */
        Public Shared ReadOnly ENCRYPT_PADDING = { _
                                     &H28, &HBF, &H4E, &H5E, &H4E, _
                                     &H75, &H8A, &H41, &H64, &H0, _
                                     &H4E, &H56, &HFF, &HFA, &H1, _
                                     &H8, &H2E, &H2E, &H0, &HB6, _
                                     &HD0, &H68, &H3E, &H80, &H2F, _
                                     &HC, &HA9, &HFE, &H64, &H53, _
                                     &H69, &H7A _
                                       }

        '/**
        ' * This will encrypt a piece of data.
        ' *
        ' * @param objectNumber The id for the object.
        ' * @param genNumber The generation id for the object.
        ' * @param key The key used to encrypt the data.
        ' * @param data The data to encrypt/decrypt.
        ' * @param output The stream to write to.
        ' *
        ' * @throws CryptographyException If there is an error encrypting the data.
        ' * @throws IOException If there is an io error.
        ' */
        Public Sub encryptData(ByVal objectNumber As Integer, ByVal genNumber As Integer, ByVal key() As Byte, ByVal data As Stream, ByVal output As Stream) 'throws(CryptographyException, IOException)
            Dim newKey() As Byte
            ReDim newKey(key.Length + 5 - 1)
            Array.Copy(key, 0, newKey, 0, key.Length)
            'PDF 1.4 reference pg 73
            'step 1
            'we have the reference

            'step 2
            newKey(newKey.Length - 5) = objectNumber And &HFF
            newKey(newKey.Length - 4) = (objectNumber >> 8) And &HFF
            newKey(newKey.Length - 3) = (objectNumber >> 16) And &HFF
            newKey(newKey.Length - 2) = (genNumber And &HFF)
            newKey(newKey.Length - 1) = (genNumber >> 8) And &HFF

            'step 3
            Dim digestedKey() As Byte = Nothing
            Try
                Dim md As MessageDigest = MessageDigest.getInstance("MD5")
                digestedKey = md.digest(newKey)
            Catch e As Exception
                Throw e 'New CryptographyException(e)
            End Try

            'step 4
            Dim length As Integer = Math.Min(newKey.Length, 16)
            Dim finalKey() As Byte
            ReDim finalKey(length - 1)
            Array.Copy(digestedKey, 0, finalKey, 0, length)

            rc4.setKey(finalKey)
            rc4.write(data, output)
            output.Flush()
        End Sub

        '/**
        ' * This will get the user password from the owner password and the documents o value.
        ' *
        ' * @param ownerPassword The plaintext owner password.
        ' * @param o The document's o entry.
        ' * @param revision The document revision number.
        ' * @param length The length of the encryption.
        ' *
        ' * @return The plaintext padded user password.
        ' *
        ' * @throws CryptographyException If there is an error getting the user password.
        ' * @throws IOException If there is an error reading data.
        ' */
        Public Function getUserPassword(ByVal ownerPassword() As Byte, ByVal o() As Byte, ByVal revision As Integer, ByVal length As Integer) As Byte() 'throws(CryptographyException, IOException)
            Try
                Dim result As New MemoryStream 'ByteArrayOutputStream new ByteArrayOutputStream();
                '3.3 STEP 1
                Dim ownerPadded() As Byte = truncateOrPad(ownerPassword)
                '3.3 STEP 2
                Dim md As MessageDigest = MessageDigest.getInstance("MD5")
                md.update(ownerPadded)
                Dim digest() As Byte = md.digest()

                '3.3 STEP 3
                If (revision = 3 OrElse revision = 4) Then
                    For i As Integer = 0 To 50 - 1
                        md.reset()
                        md.update(digest)
                        digest = md.digest()
                    Next
                End If
                If (revision = 2 AndAlso length <> 5) Then
                    Throw New ArgumentException("Error: Expected length=5 actual=" & length)
                End If

                '3.3 STEP 4
                Dim rc4Key() As Byte
                ReDim rc4Key(length - 1)
                Array.Copy(digest, 0, rc4Key, 0, length)

                '3.7 step 2
                If (revision = 2) Then
                    rc4.setKey(rc4Key)
                    rc4.write(o, result)
                ElseIf (revision = 3 OrElse revision = 4) Then
                    '/**
                    'byte[] iterationKey = new byte[ rc4Key.length ];
                    'byte[] dataToEncrypt = o;
                    'for( int i=19; i>=0; i-- )
                    '{
                    '    System.arraycopy( rc4Key, 0, iterationKey, 0, rc4Key.length );
                    '    for( int j=0; j< iterationKey.length; j++ )
                    '    {
                    '        iterationKey[j] = (byte)(iterationKey[j] ^ (byte)i);
                    '    }
                    '    rc4.setKey( iterationKey );
                    '    rc4.write( dataToEncrypt, result );
                    '    dataToEncrypt = result.toByteArray();
                    '    result.reset();
                    '}
                    'result.write( dataToEncrypt, 0, dataToEncrypt.length );
                    '*/
                    Dim iterationKey() As Byte
                    ReDim iterationKey(rc4Key.Length - 1)

                    Dim otemp() As Byte
                    ReDim otemp(o.Length - 1) 'sm
                    Array.Copy(o, 0, otemp, 0, o.Length) 'sm
                    rc4.write(o, result) 'sm

                    For i As Integer = 19 To 0 Step -1
                        Array.Copy(rc4Key, 0, iterationKey, 0, rc4Key.Length)
                        For j As Integer = 0 To iterationKey.Length - 1
                            iterationKey(j) = (iterationKey(j) Xor i)
                        Next
                        rc4.setKey(iterationKey)
                        result.Position = 0 'sm
                        rc4.write(otemp, result) 'sm
                        otemp = result.ToArray() 'sm
                    Next
                End If
                Return result.ToArray()
            Catch e As Exception
                Throw e
            End Try
        End Function

        '/**
        ' * This will tell if this is the owner password or not.
        ' *
        ' * @param ownerPassword The plaintext owner password.
        ' * @param u The U value from the PDF Document.
        ' * @param o The owner password hash.
        ' * @param permissions The document permissions.
        ' * @param id The document id.
        ' * @param revision The revision of the encryption.
        ' * @param length The length of the encryption key.
        ' *
        ' * @return true if the owner password matches the one from the document.
        ' *
        ' * @throws CryptographyException If there is an error while executing crypt functions.
        ' * @throws IOException If there is an error while checking owner password.
        ' */
        Public Function isOwnerPassword(ByVal ownerPassword() As Byte, ByVal u() As Byte, ByVal o() As Byte, ByVal permissions As Integer, ByVal id() As Byte, ByVal revision As Integer, ByVal length As Integer) As Boolean 'throws(CryptographyException, IOException)
            Dim userPassword() As Byte = getUserPassword(ownerPassword, o, revision, length)
            Return isUserPassword(userPassword, u, o, permissions, id, revision, length)
        End Function

        '/**
        ' * This will tell if this is a valid user password.
        ' *
        ' * Algorithm 3.6 pg 80
        ' *
        ' * @param password The password to test.
        ' * @param u The U value from the PDF Document.
        ' * @param o The owner password hash.
        ' * @param permissions The document permissions.
        ' * @param id The document id.
        ' * @param revision The revision of the encryption.
        ' * @param length The length of the encryption key.
        ' *
        ' * @return true If this is the correct user password.
        ' *
        ' * @throws CryptographyException If there is an error computing the value.
        ' * @throws IOException If there is an IO error while computing the owners password.
        ' */
        Public Function isUserPassword(ByVal password() As Byte, ByVal u() As Byte, ByVal o() As Byte, ByVal permissions As Integer, ByVal id() As Byte, ByVal revision As Integer, ByVal length As Integer) As Boolean  'throws(CryptographyException, IOException)
            Dim matches As Boolean = False
            'STEP 1
            Dim computedValue() As Byte = computeUserPassword(password, o, permissions, id, revision, length)
            If (revision = 2) Then
                'STEP 2
                matches = arraysEqual(u, computedValue)
            ElseIf (revision = 3 OrElse revision = 4) Then
                'STEP 2
                matches = arraysEqual(u, computedValue, 16)
            End If
            Return matches
        End Function

        '/**
        ' * This will compare two byte[] for equality for count number of bytes.
        ' *
        ' * @param first The first byte array.
        ' * @param second The second byte array.
        ' * @param count The number of bytes to compare.
        ' *
        ' * @return true If the arrays contain the exact same data.
        ' */
        Private Function arraysEqual(ByVal first() As Byte, ByVal second() As Byte, ByVal count As Integer) As Boolean
            Dim equal As Boolean = first.Length >= count AndAlso second.Length >= count
            Dim i As Integer = 0
            While (i < count AndAlso equal)
                equal = first(i) = second(i)
                i += 1
            End While
            Return equal
        End Function

        '/**
        ' * This will compare two byte[] for equality.
        ' *
        ' * @param first The first byte array.
        ' * @param second The second byte array.
        ' *
        ' * @return true If the arrays contain the exact same data.
        ' */
        Private Function arraysEqual(ByVal first() As Byte, ByVal second() As Byte) As Boolean
            Dim equal As Boolean = first.Length = second.Length
            Dim i As Integer = 0
            While (i < first.Length AndAlso equal)
                equal = first(i) = second(i)
                i += 1
            End While
            Return equal
        End Function

        '/**
        ' * This will compute the user password hash.
        ' *
        ' * @param password The plain text password.
        ' * @param o The owner password hash.
        ' * @param permissions The document permissions.
        ' * @param id The document id.
        ' * @param revision The revision of the encryption.
        ' * @param length The length of the encryption key.
        ' *
        ' * @return The user password.
        ' *
        ' * @throws CryptographyException If there is an error computing the user password.
        ' * @throws IOException If there is an IO error.
        ' */
        Public Function computeUserPassword(ByVal password() As Byte, ByVal o() As Byte, ByVal permissions As Integer, ByVal id() As Byte, ByVal revision As Integer, ByVal length As Integer) As Byte() 'throws(CryptographyException, IOException)
            Dim result As New ByteArrayOutputStream()
            'STEP 1
            Dim encryptionKey() As Byte = computeEncryptedKey(password, o, permissions, id, revision, length)

            If (revision = 2) Then
                'STEP 2
                rc4.setKey(encryptionKey)
                rc4.write(ENCRYPT_PADDING, result)
            ElseIf (revision = 3 OrElse revision = 4) Then
                Try
                    'STEP 2
                    Dim md As MessageDigest = MessageDigest.getInstance("MD5")
                    'md.update( truncateOrPad( password ) );
                    md.update(ENCRYPT_PADDING)

                    'STEP 3
                    md.update(id)
                    result.Write(md.digest())

                    'STEP 4 and 5
                    Dim iterationKey() As Byte
                    ReDim iterationKey(encryptionKey.Length - 1)
                    For i As Integer = 0 To 20 - 1
                        Array.Copy(encryptionKey, 0, iterationKey, 0, iterationKey.Length)
                        For j As Integer = 0 To iterationKey.Length - 1
                            iterationKey(j) = (iterationKey(j) Xor i)
                        Next
                        rc4.setKey(iterationKey)
                        Dim input As New ByteArrayOutputStream(result.toByteArray())
                        result.reset()
                        rc4.write(input, result)
                    Next

                    'step 6
                    Dim finalResult() As Byte
                    ReDim finalResult(32 - 1)
                    Array.Copy(result.toByteArray, 0, finalResult, 0, 16)
                    Array.Copy(ENCRYPT_PADDING, 0, finalResult, 16, 16)
                    result.Position = 0
                    result.Write(finalResult)
                Catch e As Exception '( NoSuchAlgorithmException e )
                    Throw New CryptographyException(e)
                End Try
            End If
            Return result.toByteArray
        End Function

        '/**
        ' * This will compute the encrypted key.
        ' *
        ' * @param password The password used to compute the encrypted key.
        ' * @param o The owner password hash.
        ' * @param permissions The permissions for the document.
        ' * @param id The document id.
        ' * @param revision The security revision.
        ' * @param length The length of the encryption key.
        ' *
        ' * @return The encryption key.
        ' *
        ' * @throws CryptographyException If there is an error computing the key.
        ' */
        Public Function computeEncryptedKey(ByVal password() As Byte, ByVal o() As Byte, ByVal permissions As Integer, ByVal id() As Byte, ByVal revision As Integer, ByVal length As Integer) As Byte() 'throws CryptographyException
            Dim result() As Byte
            ReDim result(length - 1)
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
                Dim zero As Byte = (permissions >> 0)
                Dim one As Byte = (permissions >> 8)
                Dim two As Byte = (permissions >> 16)
                Dim three As Byte = (permissions >> 24)

                md.update(zero)
                md.update(one)
                md.update(two)
                md.update(three)

                'step 5
                md.update(id)
                Dim digest() As Byte = md.digest()

                'step 6
                If (revision = 3 OrElse revision = 4) Then
                    For i As Integer = 0 To 50 - 1
                        md.reset()
                        md.update(digest, 0, length)
                        digest = md.digest()
                    Next
                End If

                'step 7
                If (revision = 2 AndAlso length <> 5) Then
                    Throw New CryptographyException("Error: length should be 5 when revision is two actual=" & length)
                End If
                Array.Copy(digest, 0, result, 0, length)
            Catch e As Exception 'NoSuchAlgorithmException 
                Throw New CryptographyException(e)
            End Try
            Return result
        End Function

        '/**
        ' * This algorithm is taked from PDF Reference 1.4 Algorithm 3.3 Page 79.
        ' *
        ' * @param ownerPassword The plain owner password.
        ' * @param userPassword The plain user password.
        ' * @param revision The version of the security.
        ' * @param length The length of the document.
        ' *
        ' * @return The computed owner password.
        ' *
        ' * @throws CryptographyException If there is an error computing O.
        ' * @throws IOException If there is an error computing O.
        ' */
        Public Function computeOwnerPassword(ByVal ownerPassword() As Byte, ByVal userPassword() As Byte, ByVal revision As Integer, ByVal length As Integer) As Byte() 'throws(CryptographyException, IOException)
            Try
                'STEP 1
                Dim ownerPadded() As Byte = truncateOrPad(ownerPassword)

                'STEP 2
                Dim md As MessageDigest = MessageDigest.getInstance("MD5")
                md.update(ownerPadded)
                Dim digest() As Byte = md.digest()

                'STEP 3
                If (revision = 3 OrElse revision = 4) Then
                    For i As Integer = 0 To 50 - 1
                        md.reset()
                        md.update(digest, 0, length)
                        digest = md.digest()
                    Next
                End If
                If (revision = 2 AndAlso length <> 5) Then
                    Throw New Exception("Error: Expected length=5 actual=" & length)
                End If

                'STEP 4
                Dim rc4Key() As Byte
                ReDim rc4Key(length - 1)
                Array.Copy(digest, 0, rc4Key, 0, length)

                'STEP 5
                Dim paddedUser() As Byte = truncateOrPad(userPassword)


                'STEP 6
                rc4.setKey(rc4Key)
                Dim crypted As New MemoryStream 'new ByteArrayOutputStream();
                rc4.write(New MemoryStream(paddedUser), crypted)

                'STEP 7
                If (revision = 3 OrElse revision = 4) Then
                    Dim iterationKey() As Byte
                    ReDim iterationKey(rc4Key.Length - 1)
                    For i As Integer = 1 To 20 - 1
                        Array.Copy(rc4Key, 0, iterationKey, 0, rc4Key.Length)
                        For j As Integer = 0 To iterationKey.Length - 1
                            iterationKey(j) = (iterationKey(j) Xor i)
                        Next
                        rc4.setKey(iterationKey)
                        Dim input As New MemoryStream(crypted.ToArray()) 'ByteArrayInputStream
                        crypted.Position = 0
                        rc4.write(input, crypted)
                    Next
                End If

                'STEP 8
                Return crypted.ToArray()
            Catch e As Exception 'NoSuchAlgorithmException 
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
            Dim padded() As Byte
            ReDim padded(ENCRYPT_PADDING.length - 1)
            Dim bytesBeforePad As Integer = Math.Min(password.Length, padded.Length)
            Array.Copy(password, 0, padded, 0, bytesBeforePad)
            Array.Copy(ENCRYPT_PADDING, 0, padded, bytesBeforePad, ENCRYPT_PADDING.length - bytesBeforePad)
            Return padded
        End Function

    End Class

End Namespace
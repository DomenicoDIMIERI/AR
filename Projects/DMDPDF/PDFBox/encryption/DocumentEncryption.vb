Imports FinSeA.Io
Imports FinSeA.Exceptions
Imports FinSeA.Security
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.exceptions
Imports FinSeA.org.apache.pdfbox.pdmodel
Imports FinSeA.org.apache.pdfbox.pdmodel.encryption

Namespace org.apache.pdfbox.encryption

    '/**
    ' * This class will deal with encrypting/decrypting a document.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.13 $
    ' *
    ' * @deprecated use the new security API instead.
    ' *
    ' * @see org.apache.pdfbox.pdmodel.encryption.StandardSecurityHandler
    ' */
    Public Class DocumentEncryption

        Private pdDocument As PDDocument = Nothing
        Private document As COSDocument = Nothing

        Private encryptionKey() As Byte = Nothing
        Private encryption As PDFEncryption = New PDFEncryption()

        Private objects As [Set] = New HashSet()

        '/**
        ' * A set that contains potential signature dictionaries.  This is used
        ' * because the Contents entry of the signature is not encrypted.
        ' */
        Private potentialSignatures As [Set] = New HashSet()

        '/**
        ' * Constructor.
        ' *
        ' * @param doc The document to decrypt.
        ' */
        Public Sub New(ByVal doc As PDDocument)
            pdDocument = doc
            document = doc.getDocument()
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param doc The document to decrypt.
        ' */
        Public Sub New(ByVal doc As COSDocument)
            pdDocument = New PDDocument(doc)
            document = doc
        End Sub

        '/**
        ' * This will encrypt the given document, given the owner password and user password.
        ' * The encryption method used is the standard filter.
        ' *
        ' * @throws CryptographyException If an error occurs during encryption.
        ' * @throws IOException If there is an error accessing the data.
        ' */
        Public Sub initForEncryption() 'throws(CryptographyException, IOException)
            Dim ownerPassword As String = pdDocument.getOwnerPasswordForEncryption()
            Dim userPassword As String = pdDocument.getUserPasswordForEncryption()
            If (ownerPassword = vbNullString) Then
                ownerPassword = ""
            End If
            If (userPassword = vbNullString) Then
                userPassword = ""
            End If
            Dim encParameters As PDStandardEncryption = pdDocument.getEncryptionDictionary()
            Dim permissionInt As Integer = encParameters.getPermissions()
            Dim revision As Integer = encParameters.getRevision()
            Dim length As Integer = encParameters.getLength() / 8
            Dim idArray As COSArray = document.getDocumentID()

            'check if the document has an id yet.  If it does not then
            'generate one
            If (idArray Is Nothing OrElse idArray.size() < 2) Then
                idArray = New COSArray()
                Try
                    Dim md As MessageDigest = MessageDigest.getInstance("MD5")
                    Dim time As New BigInteger(Timer)
                    md.update(time.toByteArray())
                    md.update(Sistema.Strings.GetBytes(ownerPassword, "ISO-8859-1"))
                    md.update(Sistema.Strings.GetBytes(userPassword, "ISO-8859-1"))
                    md.update(Sistema.Strings.GetBytes(document.ToString()))
                    Dim id1() As Byte = md.digest(Sistema.Strings.GetBytes(Me.ToString(), "ISO-8859-1"))
                    Dim idString As COSString = New COSString()
                    idString.append(id1)
                    idArray.add(idString)
                    idArray.add(idString)
                    document.setDocumentID(idArray)
                Catch e As NoSuchAlgorithmException
                    Throw New CryptographyException(e)
                End Try
            End If
            Dim id As COSString = idArray.getObject(0)
            encryption = New PDFEncryption()

            Dim o() As Byte = encryption.computeOwnerPassword(Sistema.Strings.GetBytes(ownerPassword, "ISO-8859-1"), Sistema.Strings.GetBytes(userPassword, "ISO-8859-1"), revision, length)

            Dim u() As Byte = encryption.computeUserPassword(Sistema.Strings.GetBytes(userPassword, "ISO-8859-1"), o, permissionInt, id.getBytes(), revision, length)

            encryptionKey = encryption.computeEncryptedKey(Sistema.Strings.GetBytes(userPassword, "ISO-8859-1"), o, permissionInt, id.getBytes(), revision, length)

            encParameters.setOwnerKey(o)
            encParameters.setUserKey(u)

            document.setEncryptionDictionary(encParameters.getCOSDictionary())
        End Sub



        '/**
        ' * This will decrypt the document.
        ' *
        ' * @param password The password for the document.
        ' *
        ' * @throws CryptographyException If there is an error decrypting the document.
        ' * @throws IOException If there is an error getting the stream data.
        ' * @throws InvalidPasswordException If the password is not a user or owner password.
        ' */
        Public Sub decryptDocument(ByVal password As String) '            throws(CryptographyException, IOException, InvalidPasswordException)
            If (password = vbNullString) Then
                password = ""
            End If

            Dim encParameters As PDStandardEncryption = pdDocument.getEncryptionDictionary()


            Dim permissions As Integer = encParameters.getPermissions()
            Dim revision As Integer = encParameters.getRevision()
            Dim length As Integer = encParameters.getLength() / 8

            Dim id As COSString = document.getDocumentID().getObject(0)
            Dim u() As Byte = encParameters.getUserKey()
            Dim o() As Byte = encParameters.getOwnerKey()

            Dim isUserPassword As Boolean = encryption.isUserPassword(Sistema.Strings.GetBytes(password, "ISO-8859-1"), u, o, permissions, id.getBytes(), revision, length)
            Dim isOwnerPassword As Boolean = encryption.isOwnerPassword(Sistema.Strings.GetBytes(password, "ISO-8859-1"), u, o, permissions, id.getBytes(), revision, length)

            If (isUserPassword) Then
                encryptionKey = encryption.computeEncryptedKey(Sistema.Strings.GetBytes(password, "ISO-8859-1"), o, permissions, id.getBytes(), revision, length)
            ElseIf (isOwnerPassword) Then
                Dim computedUserPassword() As Byte = encryption.getUserPassword(Sistema.Strings.GetBytes(password, "ISO-8859-1"), o, revision, length)
                encryptionKey = encryption.computeEncryptedKey(computedUserPassword, o, permissions, id.getBytes(), revision, length)
            Else
                Throw New InvalidPasswordException("Error: The supplied password does not match either the owner or user password in the document.")
            End If

            Dim trailer As COSDictionary = document.getTrailer()
            Dim fields As COSArray = trailer.getObjectFromPath("Root/AcroForm/Fields")

            '//We need to collect all the signature dictionaries, for some
            '//reason the 'Contents' entry of signatures is not really encrypted
            If (fields IsNot Nothing) Then
                For i As Integer = 0 To fields.size() - 1
                    Dim field As COSDictionary = fields.getObject(i)
                    addDictionaryAndSubDictionary(potentialSignatures, field)
                Next
            End If

            '    Dim allObjects As List = document.getObjects()
            'Iterator objectIter = allObjects.iterator();
            '   While (objectIter.hasNext())
            '{
            For Each item As COSObject In document.getObjects
                decryptObject(item)
            Next
            document.setEncryptionDictionary(Nothing)
        End Sub

        Private Sub addDictionaryAndSubDictionary(ByVal [set] As [Set], ByVal dic As COSDictionary)
            [set].add(dic)
            Dim kids As COSArray = dic.getDictionaryObject(COSName.KIDS)
            If kids IsNot Nothing Then
                For i As Integer = 0 To kids.size() - 1
                    addDictionaryAndSubDictionary([set], kids.getObject(i))
                Next
            End If
            Dim value As COSBase = dic.getDictionaryObject(COSName.V)
            If (TypeOf (value) Is COSDictionary) Then
                addDictionaryAndSubDictionary([set], value)
            End If
        End Sub

        '/**
        ' * This will decrypt an object in the document.
        ' *
        ' * @param object The object to decrypt.
        ' *
        ' * @throws CryptographyException If there is an error decrypting the stream.
        ' * @throws IOException If there is an error getting the stream data.
        ' */
        Private Sub decryptObject(ByVal [object] As COSObject) 'throws(CryptographyException, IOException)
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
        Public Sub decrypt(ByVal obj As Object, ByVal objNum As Long, ByVal genNum As Long) 'throws(CryptographyException, IOException)
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
        Private Sub decryptStream(ByVal stream As COSStream, ByVal objNum As Long, ByVal genNum As Long) 'throws(CryptographyException, IOException)
            decryptDictionary(stream, objNum, genNum)
            Dim encryptedStream As InputStream = stream.getFilteredStream()
            encryption.encryptData(objNum, genNum, encryptionKey, encryptedStream, stream.createFilteredStream())
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
        Private Sub decryptDictionary(ByVal dictionary As COSDictionary, ByVal objNum As Long, ByVal genNum As Long) 'throws(CryptographyException, IOException)
            For Each entry As Map.Entry(Of COSName, COSBase) In dictionary.entrySet()
                'if we are a signature dictionary and contain a Contents entry then
                'we don't decrypt it.
                If (Not (entry.Key.getName().Equals("Contents") AndAlso TypeOf (entry.Value) Is COSString AndAlso potentialSignatures.contains(dictionary))) Then
                    decrypt(entry.Value, objNum, genNum)
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
        Private Sub decryptString(ByVal [string] As COSString, ByVal objNum As Long, ByVal genNum As Long) 'throws(CryptographyException, IOException)
            Dim data As ByteArrayInputStream = New ByteArrayInputStream([string].getBytes())
            Dim buffer As ByteArrayOutputStream = New ByteArrayOutputStream()
            encryption.encryptData(objNum, genNum, encryptionKey, data, buffer)
            [string].reset()
            [string].append(buffer.toByteArray())
            buffer.Dispose()
            data.Dispose()
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
        Private Sub decryptArray(ByVal array As COSArray, ByVal objNum As Long, ByVal genNum As Long) '            throws(CryptographyException, IOException)
            For i As Integer = 0 To array.size() - 1
                decrypt(array.get(i), objNum, genNum)
            Next
        End Sub


    End Class

End Namespace

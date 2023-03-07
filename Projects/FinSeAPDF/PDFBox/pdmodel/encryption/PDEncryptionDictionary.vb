Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos

Namespace org.apache.pdfbox.pdmodel.encryption


    '/**
    ' * This class is a specialized view of the encryption dictionary of a PDF document.
    ' * It contains a low level dictionary (COSDictionary) and provides the methods to
    ' * manage its fields.
    ' *
    ' * The available fields are the ones who are involved by standard security handler
    ' * and public key security handler.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @author Benoit Guillon (benoit.guillon@snv.jussieu.fr)
    ' *
    ' * @version $Revision: 1.7 $
    ' */
    Public Class PDEncryptionDictionary

        ''' <summary>
        ''' See PDF Reference 1.4 Table 3.13.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const VERSION0_UNDOCUMENTED_UNSUPPORTED As Integer = 0

        ''' <summary>
        ''' See PDF Reference 1.4 Table 3.13.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const VERSION1_40_BIT_ALGORITHM As Integer = 1

        ''' <summary>
        ''' See PDF Reference 1.4 Table 3.13.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const VERSION2_VARIABLE_LENGTH_ALGORITHM = 2

        ''' <summary>
        ''' See PDF Reference 1.4 Table 3.13.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const VERSION3_UNPUBLISHED_ALGORITHM = 3

        ''' <summary>
        ''' See PDF Reference 1.4 Table 3.13.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const VERSION4_SECURITY_HANDLER = 4

        ''' <summary>
        ''' The default security handler.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const DEFAULT_NAME As String = "Standard"

        ''' <summary>
        ''' The default length for the encryption key.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const DEFAULT_LENGTH = 40

        ''' <summary>
        ''' The default version, according to the PDF Reference.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const DEFAULT_VERSION = VERSION0_UNDOCUMENTED_UNSUPPORTED

        ''' <summary>
        ''' COS encryption dictionary.
        ''' </summary>
        ''' <remarks></remarks>
        Protected Friend encryptionDictionary As COSDictionary = Nothing

        ''' <summary>
        ''' creates a new empty encryption dictionary.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            encryptionDictionary = New COSDictionary()
        End Sub

        '/**
        ' * creates a new encryption dictionary from the low level dictionary provided.
        ' * @param d the low level dictionary that will be managed by the newly created object
        ' */
        Public Sub New(ByVal d As COSDictionary)
            encryptionDictionary = d
        End Sub

        '/**
        ' * This will get the dictionary associated with this encryption dictionary.
        ' *
        ' * @return The COS dictionary that this object wraps.
        ' */
        Public Function getCOSDictionary() As COSDictionary
            Return encryptionDictionary
        End Function

        '/**
        ' * Sets the filter entry of the encryption dictionary.
        ' *
        ' * @param filter The filter name.
        ' */
        Public Sub setFilter(ByVal filter As String)
            encryptionDictionary.setItem(COSName.FILTER, COSName.getPDFName(filter))
        End Sub

        '/**
        ' * Get the name of the filter.
        ' *
        ' * @return The filter name contained in this encryption dictionary.
        ' */
        Public Function getFilter() As String
            Return encryptionDictionary.getNameAsString(COSName.FILTER)
        End Function

        '/**
        ' * Get the name of the subfilter.
        ' *
        ' * @return The subfilter name contained in this encryption dictionary.
        ' */
        Public Function getSubFilter() As String
            Return encryptionDictionary.getNameAsString(COSName.SUB_FILTER)
        End Function

        '/**
        ' * Set the subfilter entry of the encryption dictionary.
        ' *
        ' * @param subfilter The value of the subfilter field.
        ' */
        Public Sub setSubFilter(ByVal subfilter As String)
            encryptionDictionary.setName(COSName.SUB_FILTER, subfilter)
        End Sub

        '/**
        ' * This will set the V entry of the encryption dictionary.<br /><br />
        ' * See PDF Reference 1.4 Table 3.13.  <br /><br/>
        ' * <b>Note: This value is used to decrypt the pdf document.  If you change this when
        ' * the document is encrypted then decryption will fail!.</b>
        ' *
        ' * @param version The new encryption version.
        ' */
        Public Overridable Sub setVersion(ByVal version As Integer)
            encryptionDictionary.setInt(COSName.V, version)
        End Sub

        '/**
        ' * This will return the V entry of the encryption dictionary.<br /><br />
        ' * See PDF Reference 1.4 Table 3.13.
        ' *
        ' * @return The encryption version to use.
        ' */
        Public Overridable Function getVersion() As Integer
            Return encryptionDictionary.getInt(COSName.V, 0)
        End Function

        '/**
        ' * This will set the number of bits to use for the encryption algorithm.
        ' *
        ' * @param length The new key length.
        ' */
        Public Sub setLength(ByVal length As Integer)
            encryptionDictionary.setInt(COSName.LENGTH, length)
        End Sub

        '/**
        ' * This will return the Length entry of the encryption dictionary.<br /><br />
        ' * The length in <b>bits</b> for the encryption algorithm.  This will return a multiple of 8.
        ' *
        ' * @return The length in bits for the encryption algorithm
        ' */
        Public Function getLength() As Integer
            Return encryptionDictionary.getInt(COSName.LENGTH, 40)
        End Function

        '/**
        ' * This will set the R entry of the encryption dictionary.<br /><br />
        ' * See PDF Reference 1.4 Table 3.14.  <br /><br/>
        ' *
        ' * <b>Note: This value is used to decrypt the pdf document.  If you change this when
        ' * the document is encrypted then decryption will fail!.</b>
        ' *
        ' * @param revision The new encryption version.
        ' */
        Public Overridable Sub setRevision(ByVal revision As Integer)
            encryptionDictionary.setInt(COSName.R, revision)
        End Sub

        '/**
        ' * This will return the R entry of the encryption dictionary.<br /><br />
        ' * See PDF Reference 1.4 Table 3.14.
        ' *
        ' * @return The encryption revision to use.
        ' */
        Public Overridable Function getRevision() As Integer
            Return encryptionDictionary.getInt(COSName.R, DEFAULT_VERSION)
        End Function

        '/**
        '* This will set the O entry in the standard encryption dictionary.
        '*
        '* @param o A 32 byte array or null if there is no owner key.
        '*
        '* @throws IOException If there is an error setting the data.
        '*/
        Public Overridable Sub setOwnerKey(ByVal o() As Byte) 'throws IOException
            Dim owner As COSString = New COSString()
            owner.append(o)
            encryptionDictionary.setItem(COSName.O, owner)
        End Sub

        '/**
        ' * This will get the O entry in the standard encryption dictionary.
        ' *
        ' * @return A 32 byte array or null if there is no owner key.
        ' *
        ' * @throws IOException If there is an error accessing the data.
        ' */
        Public Overridable Function getOwnerKey() As Byte() ' throws IOException
            Dim o As Byte() = Nothing
            Dim owner As COSString = encryptionDictionary.getDictionaryObject(COSName.O)
            If (owner IsNot Nothing) Then
                o = owner.getBytes()
            End If
            Return o
        End Function

        '/**
        ' * This will set the U entry in the standard encryption dictionary.
        ' *
        ' * @param u A 32 byte array.
        ' *
        ' * @throws IOException If there is an error setting the data.
        ' */
        Public Overridable Sub setUserKey(ByVal u() As Byte) 'throws IOException
            Dim user As New COSString()
            user.append(u)
            encryptionDictionary.setItem(COSName.U, user)
        End Sub

        '/**
        ' * This will get the U entry in the standard encryption dictionary.
        ' *
        ' * @return A 32 byte array or null if there is no user key.
        ' *
        ' * @throws IOException If there is an error accessing the data.
        ' */
        Public Overridable Function getUserKey() As Byte() 'byte[] throws IOException
            Dim u() As Byte = Nothing
            Dim user As COSString = encryptionDictionary.getDictionaryObject(COSName.U)
            If (user IsNot Nothing) Then
                u = user.getBytes()
            End If
            Return u
        End Function

        '/**
        ' * This will set the permissions bit mask.
        ' *
        ' * @param permissions The new permissions bit mask
        ' */
        Public Overridable Sub setPermissions(ByVal permissions As Integer)
            encryptionDictionary.setInt(COSName.P, permissions)
        End Sub

        '/**
        ' * This will get the permissions bit mask.
        ' *
        ' * @return The permissions bit mask.
        ' */
        Public Overridable Function getPermissions() As Integer
            Return encryptionDictionary.getInt(COSName.P, 0)
        End Function

        '/**
        ' * Will get the EncryptMetaData dictionary info.
        ' * 
        ' * @return true if EncryptMetaData is explicitly set to false (the default is true)
        ' */
        Public Function isEncryptMetaData() As Boolean
            ' default is true (see 7.6.3.2 Standard Encryption Dictionary PDF 32000-1:2008)
            Dim encryptMetaData As Boolean = True

            Dim value As COSBase = encryptionDictionary.getDictionaryObject(COSName.ENCRYPT_META_DATA)

            If (TypeOf (value) Is COSBoolean) Then
                encryptMetaData = DirectCast(value, COSBoolean).getValue()
            End If

            Return encryptMetaData
        End Function

        '/**
        ' * This will set the Recipients field of the dictionary. This field contains an array
        ' * of string.
        ' * @param recipients the array of bytes arrays to put in the Recipients field.
        ' * @throws IOException If there is an error setting the data.
        ' */
        Public Sub setRecipients(ByVal recipients As Byte()()) ' throws IOException
            Dim array As COSArray = New COSArray()
            For i As Integer = 0 To recipients.Length - 1
                Dim recip As COSString = New COSString()
                recip.append(recipients(i))
                recip.setForceLiteralForm(True)
                array.add(recip)
            Next
            encryptionDictionary.setItem(COSName.RECIPIENTS, array)
        End Sub

        '/**
        ' * Returns the number of recipients contained in the Recipients field of the dictionary.
        ' *
        ' * @return the number of recipients contained in the Recipients field.
        ' */
        Public Function getRecipientsLength() As Integer
            Dim array As COSArray = encryptionDictionary.getItem(COSName.RECIPIENTS)
            Return array.size()
        End Function

        '/**
        ' * returns the COSString contained in the Recipients field at position i.
        ' *
        ' * @param i the position in the Recipients field array.
        ' *
        ' * @return a COSString object containing information about the recipient number i.
        ' */
        Public Function getRecipientStringAt(ByVal i As Integer) As COSString
            Dim array As COSArray = encryptionDictionary.getItem(COSName.RECIPIENTS)
            Return array.get(i)
        End Function

        '/**
        ' * Returns the standard crypt filter.
        ' * 
        ' * @return the standard crypt filter if available.
        ' */
        Public Function getStdCryptFilterDictionary() As PDCryptFilterDictionary
            Return getCryptFilterDictionary(COSName.STD_CF)
        End Function

        '/**
        ' * Returns the crypt filter with the given name.
        ' * 
        ' * @param cryptFilterName the name of the crypt filter
        ' * 
        ' * @return the crypt filter with the given name if available
        ' */
        Public Function getCryptFilterDictionary(ByVal cryptFilterName As COSName) As PDCryptFilterDictionary
            Dim cryptFilterDictionary As COSDictionary = encryptionDictionary.getDictionaryObject(COSName.CF)
            If (cryptFilterDictionary IsNot Nothing) Then
                Dim stdCryptFilterDictionary As COSDictionary = cryptFilterDictionary.getDictionaryObject(cryptFilterName)
                If (stdCryptFilterDictionary IsNot Nothing) Then
                    Return New PDCryptFilterDictionary(stdCryptFilterDictionary)
                End If
            End If
            Return Nothing
        End Function

        '/**
        ' * Returns the name of the filter which is used for de/encrypting streams.
        ' * Default value is "Identity".
        ' * 
        ' * @return the name of the filter
        ' */
        Public Function getStreamFilterName() As COSName
            Dim stmF As COSName = encryptionDictionary.getDictionaryObject(COSName.STM_F)
            If (stmF Is Nothing) Then
                stmF = COSName.IDENTITY
            End If
            Return stmF
        End Function

        '/**
        ' * Returns the name of the filter which is used for de/encrypting strings.
        ' * Default value is "Identity".
        ' * 
        ' * @return the name of the filter
        ' */
        Public Function getStringFilterName() As COSName
            Dim strF As COSName = encryptionDictionary.getDictionaryObject(COSName.STR_F)
            If (strF Is Nothing) Then
                strF = COSName.IDENTITY
            End If
            Return strF
        End Function

    End Class

End Namespace
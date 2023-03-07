Imports FinSeA.org.apache.pdfbox.cos
Imports System.IO

Namespace org.apache.pdfbox.pdmodel.encryption

    '/**
    ' * This class holds information that is related to the standard PDF encryption.
    ' *
    ' * See PDF Reference 1.4 section "3.5 Encryption"
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.7 $
    ' * @deprecated Made deprecated by the new security layer of PDFBox. Use SecurityHandlers instead.
    ' */
    Public Class PDStandardEncryption
        Inherits PDEncryptionDictionary

        ''' <summary>
        ''' The 'Filter' name for this security handler.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const FILTER_NAME As String = "Standard"

        ''' <summary>
        ''' The default revision of one is not specified.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const DEFAULT_REVISION As Integer = 3

        ''' <summary>
        ''' Encryption revision 2.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const REVISION2 As Integer = 2

        ''' <summary>
        ''' Encryption revision 3.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const REVISION3 As Integer = 3

        ''' <summary>
        ''' Encryption revision 4.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const REVISION4 As Integer = 4


        ''' <summary>
        ''' The default set of permissions which is to allow all.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const DEFAULT_PERMISSIONS As Integer = &HFFFFFFFF Xor 3 'bits 0 & 1 need to be zero

        'private static final int PRINT_BIT = 3;
        'private static final int MODIFICATION_BIT = 4;
        'private static final int EXTRACT_BIT = 5;
        'private static final int MODIFY_ANNOTATIONS_BIT = 6;
        'private static final int FILL_IN_FORM_BIT = 9;
        'private static final int EXTRACT_FOR_ACCESSIBILITY_BIT = 10;
        'private static final int ASSEMBLE_DOCUMENT_BIT = 11;
        'private static final int DEGRADED_PRINT_BIT = 12;

        '/**
        ' * Default constructor that uses Version 2, Revision 3, 40 bit encryption,
        ' * all permissions allowed.
        ' */
        Public Sub New()
            MyBase.New()
            encryptionDictionary.setItem(COSName.FILTER, COSName.getPDFName(FILTER_NAME))
            setVersion(PDEncryptionDictionary.VERSION1_40_BIT_ALGORITHM)
            setRevision(PDStandardEncryption.REVISION2)
            setPermissions(DEFAULT_PERMISSIONS)
        End Sub

        '/**
        ' * Constructor from existing dictionary.
        ' *
        ' * @param dict The existing encryption dictionary.
        ' */
        Public Sub New(ByVal dict As COSDictionary)
            MyBase.New(dict)
        End Sub

        '/**
        ' * This will return the R entry of the encryption dictionary.<br /><br />
        ' * See PDF Reference 1.4 Table 3.14.
        ' *
        ' * @return The encryption revision to use.
        ' */
        Public Overrides Function getRevision() As Integer
            Dim revision As Integer = DEFAULT_VERSION
            Dim cosRevision As COSNumber = encryptionDictionary.getDictionaryObject(COSName.getPDFName("R"))
            If (cosRevision IsNot Nothing) Then
                revision = cosRevision.intValue()
            End If
            Return revision
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
        Public Overrides Sub setRevision(ByVal revision As Integer)
            encryptionDictionary.setInt(COSName.getPDFName("R"), revision)
        End Sub

        '/**
        ' * This will get the O entry in the standard encryption dictionary.
        ' *
        ' * @return A 32 byte array or null if there is no owner key.
        ' */
        Public Overrides Function getOwnerKey() As Byte()
            Dim o() As Byte = Nothing
            Dim owner As COSString = encryptionDictionary.getDictionaryObject(COSName.getPDFName("O"))
            If (owner IsNot Nothing) Then
                o = owner.getBytes()
            End If
            Return o
        End Function

        '/**
        ' * This will set the O entry in the standard encryption dictionary.
        ' *
        ' * @param o A 32 byte array or null if there is no owner key.
        ' *
        ' * @throws IOException If there is an error setting the data.
        ' */
        Public Overrides Sub setOwnerKey(ByVal o() As Byte) 'throws IOException
            Dim owner As COSString = New COSString()
            owner.append(o)
            encryptionDictionary.setItem(COSName.getPDFName("O"), owner)
        End Sub

        '/**
        ' * This will get the U entry in the standard encryption dictionary.
        ' *
        ' * @return A 32 byte array or null if there is no user key.
        ' */
        Public Overrides Function getUserKey() As Byte()
            Dim u() As Byte = Nothing
            Dim user As COSString = encryptionDictionary.getDictionaryObject(COSName.getPDFName("U"))
            If (user IsNot Nothing) Then
                u = user.getBytes()
            End If
            Return u
        End Function

        '/**
        ' * This will set the U entry in the standard encryption dictionary.
        ' *
        ' * @param u A 32 byte array.
        ' *
        ' * @throws IOException If there is an error setting the data.
        ' */
        Public Overrides Sub setUserKey(ByVal u() As Byte) 'throws IOException
            Dim user As COSString = New COSString()
            user.append(u)
            encryptionDictionary.setItem(COSName.getPDFName("U"), user)
        End Sub

        '/**
        ' * This will get the permissions bit mask.
        ' *
        ' * @return The permissions bit mask.
        ' */
        Public Overrides Function getPermissions() As Integer
            Dim permissions As Integer = 0
            Dim p As COSInteger = encryptionDictionary.getDictionaryObject(COSName.getPDFName("P"))
            If (p IsNot Nothing) Then
                permissions = p.intValue()
            End If
            Return permissions
        End Function

        '/**
        ' * This will set the permissions bit mask.
        ' *
        ' * @param p The new permissions bit mask
        ' */
        Public Overrides Sub setPermissions(ByVal p As Integer)
            encryptionDictionary.setInt(COSName.getPDFName("P"), p)
        End Sub

        Private Function isPermissionBitOn(ByVal bit As ACEnum) As Boolean
            Dim tmp As Integer = (1 << (bit - 1))
            Return (getPermissions() And tmp) = tmp
        End Function

        Private Function setPermissionBit(ByVal bit As ACEnum, ByVal value As Boolean) As Boolean
            Dim permissions As Integer = getPermissions()
            Dim tmp As Integer = (1 << (bit - 1))
            If (value) Then
                permissions = permissions Or tmp
            Else
                permissions = permissions And (&HFFFFFFFF Xor tmp)
            End If
            setPermissions(permissions)

            Return (getPermissions() And tmp) = tmp
        End Function

        '/**
        ' * This will tell if the user can print.
        ' *
        ' * @return true If supplied with the user password they are allowed to print.
        ' */
        Public Function canPrint() As Boolean
            Return isPermissionBitOn(ACEnum.PRINT_BIT)
        End Function

        '/**
        ' * Set if the user can print.
        ' *
        ' * @param allowPrinting A boolean determining if the user can print.
        ' */
        Public Sub setCanPrint(ByVal allowPrinting As Boolean)
            setPermissionBit(ACEnum.PRINT_BIT, allowPrinting)
        End Sub

        '/**
        ' * This will tell if the user can modify contents of the document.
        ' *
        ' * @return true If supplied with the user password they are allowed to modify the document
        ' */
        Public Function canModify() As Boolean
            Return isPermissionBitOn(ACEnum.MODIFICATION_BIT)
        End Function

        '/**
        ' * Set if the user can modify the document.
        ' *
        ' * @param allowModifications A boolean determining if the user can modify the document.
        ' */
        Public Sub setCanModify(ByVal allowModifications As Boolean)
            setPermissionBit(ACEnum.MODIFICATION_BIT, allowModifications)
        End Sub

        '/**
        ' * This will tell if the user can extract text and images from the PDF document.
        ' *
        ' * @return true If supplied with the user password they are allowed to extract content
        ' *              from the PDF document
        ' */
        Public Function canExtractContent() As Boolean
            Return isPermissionBitOn(ACEnum.EXTRACT_BIT)
        End Function

        '/**
        ' * Set if the user can extract content from the document.
        ' *
        ' * @param allowExtraction A boolean determining if the user can extract content
        ' *                        from the document.
        ' */
        Public Sub setCanExtractContent(ByVal allowExtraction As Boolean)
            setPermissionBit(ACEnum.EXTRACT_BIT, allowExtraction)
        End Sub

        '/**
        ' * This will tell if the user can add/modify text annotations, fill in interactive forms fields.
        ' *
        ' * @return true If supplied with the user password they are allowed to modify annotations.
        ' */
        Public Function canModifyAnnotations() As Boolean
            Return isPermissionBitOn(ACEnum.MODIFY_ANNOTATIONS_BIT)
        End Function

        '/**
        ' * Set if the user can modify annotations.
        ' *
        ' * @param allowAnnotationModification A boolean determining if the user can modify annotations.
        ' */
        Public Sub setCanModifyAnnotations(ByVal allowAnnotationModification As Boolean)
            setPermissionBit(ACEnum.MODIFY_ANNOTATIONS_BIT, allowAnnotationModification)
        End Sub

        '/**
        ' * This will tell if the user can fill in interactive forms.
        ' *
        ' * @return true If supplied with the user password they are allowed to fill in form fields.
        ' */
        Public Function canFillInForm() As Boolean
            Return isPermissionBitOn(ACEnum.FILL_IN_FORM_BIT)
        End Function

        '/**
        ' * Set if the user can fill in interactive forms.
        ' *
        ' * @param allowFillingInForm A boolean determining if the user can fill in interactive forms.
        ' */
        Public Sub setCanFillInForm(ByVal allowFillingInForm As Boolean)
            setPermissionBit(ACEnum.FILL_IN_FORM_BIT, allowFillingInForm)
        End Sub

        '/**
        ' * This will tell if the user can extract text and images from the PDF document
        ' * for accessibility purposes.
        ' *
        ' * @return true If supplied with the user password they are allowed to extract content
        ' *              from the PDF document
        ' */
        Public Function canExtractForAccessibility() As Boolean
            Return isPermissionBitOn(ACEnum.EXTRACT_FOR_ACCESSIBILITY_BIT)
        End Function

        '/**
        ' * Set if the user can extract content from the document for accessibility purposes.
        ' *
        ' * @param allowExtraction A boolean determining if the user can extract content
        ' *                        from the document.
        ' */
        Public Sub setCanExtractForAccessibility(ByVal allowExtraction As Boolean)
            setPermissionBit(ACEnum.EXTRACT_FOR_ACCESSIBILITY_BIT, allowExtraction)
        End Sub

        '/**
        ' * This will tell if the user can insert/rotate/delete pages.
        ' *
        ' * @return true If supplied with the user password they are allowed to extract content
        ' *              from the PDF document
        ' */
        Public Function canAssembleDocument() As Boolean
            Return isPermissionBitOn(ACEnum.ASSEMBLE_DOCUMENT_BIT)
        End Function

        '/**
        ' * Set if the user can insert/rotate/delete pages.
        ' *
        ' * @param allowAssembly A boolean determining if the user can assemble the document.
        ' */
        Public Sub setCanAssembleDocument(ByVal allowAssembly As Boolean)
            setPermissionBit(ACEnum.ASSEMBLE_DOCUMENT_BIT, allowAssembly)
        End Sub

        '/**
        ' * This will tell if the user can print the document in a degraded format.
        ' *
        ' * @return true If supplied with the user password they are allowed to print the
        ' *              document in a degraded format.
        ' */
        Public Function canPrintDegraded() As Boolean
            Return isPermissionBitOn(ACEnum.DEGRADED_PRINT_BIT)
        End Function

        '/**
        ' * Set if the user can print the document in a degraded format.
        ' *
        ' * @param allowAssembly A boolean determining if the user can print the
        ' *        document in a degraded format.
        ' */
        Public Sub setCanPrintDegraded(ByVal allowAssembly As Boolean)
            setPermissionBit(ACEnum.DEGRADED_PRINT_BIT, allowAssembly)
        End Sub

    End Class

End Namespace

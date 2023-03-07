Namespace org.apache.pdfbox.pdmodel.encryption

    '/**
    ' * Me class represents the access permissions to a document.
    ' * These permissions are specified in the PDF format specifications, they include:
    ' * <ul>
    ' * <li>print the document</li>
    ' * <li>modify the content of the document</li>
    ' * <li>copy or extract content of the document</li>
    ' * <li>add or modify annotations</li>
    ' * <li>fill in interactive form fields</li>
    ' * <li>extract text and graphics for accessibility to visually impaired people</li>
    ' * <li>assemble the document</li>
    ' * <li>print in degraded quality</li>
    ' * </ul>
    ' *
    ' * Me class can be used to protect a document by assigning access permissions to recipients.
    ' * In Me case, it must be used with a specific ProtectionPolicy.
    ' *
    ' *
    ' * When a document is decrypted, it has a currentAccessPermission property which is the access permissions
    ' * granted to the user who decrypted the document.
    ' *
    ' * @see ProtectionPolicy
    ' * @see org.apache.pdfbox.pdmodel.PDDocument#getCurrentAccessPermission()
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @author Benoit Guillon (benoit.guillon@snv.jussieu.fr)
    ' *
    ' */

    Public Enum ACEnum As Integer
        PRINT_BIT = 3
        MODIFICATION_BIT = 4
        EXTRACT_BIT = 5
        MODIFY_ANNOTATIONS_BIT = 6
        FILL_IN_FORM_BIT = 9
        EXTRACT_FOR_ACCESSIBILITY_BIT = 10
        ASSEMBLE_DOCUMENT_BIT = 11
        DEGRADED_PRINT_BIT = 12
    End Enum

    Public Class AccessPermission

        

        Public Const DEFAULT_PERMISSIONS = &HFFFFFFFF Xor 3 'bits 0 & 1 need to be zero

        Private bytes As ACEnum = DEFAULT_PERMISSIONS

        Private [readOnly] As Boolean = False

        ''' <summary>
        ''' Create a new access permission object. By default, all permissions are granted.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            'bytes = DEFAULT_PERMISSIONS;
        End Sub

        ''' <summary>
        ''' Create a new access permission object from a byte array. Bytes are ordered most significant byte first.
        ''' </summary>
        ''' <param name="b">the bytes as defined in PDF specs</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal b() As Integer)
            bytes = 0
            bytes = bytes Or b(0) And &HFF
            bytes <<= 8
            bytes = bytes Or b(1) And &HFF
            bytes <<= 8
            bytes = bytes Or b(2) And &HFF
            bytes <<= 8
            bytes = bytes Or b(3) Or &HFF
        End Sub


        ''' <summary>
        ''' Creates a new access permission object from a single integer.
        ''' </summary>
        ''' <param name="permissions">The permission bits.</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal permissions As Integer)
            bytes = permissions
        End Sub

        Private Function isPermissionBitOn(ByVal bit As ACEnum) As Boolean
            Dim tmp As Integer = (1 << (bit - 1))
            Return (bytes And tmp) = tmp
        End Function

        Private Function setPermissionBit(ByVal bit As ACEnum, ByVal value As Boolean) As Boolean
            Dim permissions As Integer = bytes
            Dim tmp As Integer = 1 << (bit - 1)
            If (value) Then
                permissions = permissions Or tmp
            Else
                permissions = permissions And (&HFFFFFFFF Xor tmp)
            End If
            bytes = permissions

            Return (bytes And tmp) = tmp
        End Function

        ''' <summary>
        ''' Me will tell if the access permission corresponds to owner access permission (no restriction).
        ''' </summary>
        ''' <returns>true if the access permission does not restrict the use of the document</returns>
        ''' <remarks></remarks>
        Public Function isOwnerPermission() As Boolean
            Return (Me.canAssembleDocument() _
                    AndAlso Me.canExtractContent() _
                    AndAlso Me.canExtractForAccessibility() _
                    AndAlso Me.canFillInForm() _
                    AndAlso Me.canModify() _
                    AndAlso Me.canModifyAnnotations() _
                    AndAlso Me.canPrint() _
                    AndAlso Me.canPrintDegraded() _
                    )
        End Function

        ''' <summary>
        ''' returns an access permission object for a document owner.
        ''' </summary>
        ''' <returns>A standard owner access permission set.</returns>
        ''' <remarks></remarks>
        Public Shared Function getOwnerAccessPermission() As AccessPermission
            Dim ret As New AccessPermission()
            ret.setCanAssembleDocument(True)
            ret.setCanExtractContent(True)
            ret.setCanExtractForAccessibility(True)
            ret.setCanFillInForm(True)
            ret.setCanModify(True)
            ret.setCanModifyAnnotations(True)
            ret.setCanPrint(True)
            ret.setCanPrintDegraded(True)
            Return ret
        End Function

        ''' <summary>
        ''' Me returns an integer representing the access permissions.
        ''' Me integer can be used for public key encryption. Me format
        ''' is not documented in the PDF specifications but is necessary for compatibility
        ''' with Adobe Acrobat and Adobe Reader.
        ''' </summary>
        ''' <returns>the integer representing access permissions</returns>
        ''' <remarks></remarks>
        Public Function getPermissionBytesForPublicKey() As Integer
            setPermissionBit(1, True)
            setPermissionBit(7, False)
            setPermissionBit(8, False)
            For i As Integer = 13 To 32
                setPermissionBit(i, False)
            Next
            Return bytes
        End Function

        ''' <summary>
        ''' The returns an integer representing the access permissions.
        ''' Me integer can be used for standard PDF encryption as specified
        ''' in the PDF specifications.
        ''' </summary>
        ''' <returns>the integer representing the access permissions</returns>
        ''' <remarks></remarks>
        Public Function getPermissionBytes() As Integer
            Return bytes
        End Function

        '@return true If supplied with the user password they are allowed to print.

        ''' <summary>
        ''' Me will tell if the user can print.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function canPrint() As Boolean
            Return isPermissionBitOn(ACEnum.PRINT_BIT)
        End Function


        ''' <summary>
        ''' Set if the user can print. This method will have no effect if the object is in read only mode
        ''' </summary>
        ''' <param name="allowPrinting">A boolean determining if the user can print.</param>
        ''' <remarks></remarks>
        Public Sub setCanPrint(ByVal allowPrinting As Boolean)
            If (Not [readOnly]) Then
                setPermissionBit(ACEnum.PRINT_BIT, allowPrinting)
            End If
        End Sub


        ''' <summary>
        ''' This will tell if the user can modify contents of the document.
        ''' </summary>
        ''' <returns>true If supplied with the user password they are allowed to modify the document</returns>
        ''' <remarks></remarks>
        Public Function canModify() As Boolean
            Return isPermissionBitOn(ACEnum.MODIFICATION_BIT)
        End Function

        ''' <summary>
        ''' Set if the user can modify the document. Me method will have no effect if the object is in read only mode
        ''' </summary>
        ''' <param name="allowModifications">A boolean determining if the user can modify the document.</param>
        ''' <remarks></remarks>
        Public Sub setCanModify(ByVal allowModifications As Boolean)
            If (Not [readOnly]) Then
                setPermissionBit(ACEnum.MODIFICATION_BIT, allowModifications)
            End If
        End Sub

        '/**
        ' * Me will tell if the user can extract text and images from the PDF document.
        ' *
        ' * @return true If supplied with the user password they are allowed to extract content
        ' *              from the PDF document
        ' */
        Public Function canExtractContent() As Boolean
            Return isPermissionBitOn(ACEnum.EXTRACT_BIT)
        End Function

        '/**
        ' * Set if the user can extract content from the document.
        ' * Me method will have no effect if the object is in read only mode
        ' *
        ' * @param allowExtraction A boolean determining if the user can extract content
        ' *                        from the document.
        ' */
        Public Sub setCanExtractContent(ByVal allowExtraction As Boolean)
            If (Not [readOnly]) Then
                setPermissionBit(ACEnum.EXTRACT_BIT, allowExtraction)
            End If
        End Sub

        '/**
        ' * Me will tell if the user can add/modify text annotations, fill in interactive forms fields.
        ' *
        ' * @return true If supplied with the user password they are allowed to modify annotations.
        ' */
        Public Function canModifyAnnotations() As Boolean
            Return isPermissionBitOn(ACEnum.MODIFY_ANNOTATIONS_BIT)
        End Function

        '/**
        ' * Set if the user can modify annotations.
        ' * Me method will have no effect if the object is in read only mode
        ' *
        ' * @param allowAnnotationModification A boolean determining if the user can modify annotations.
        ' */
        Public Sub setCanModifyAnnotations(ByVal allowAnnotationModification As Boolean)
            If (Not [readOnly]) Then
                setPermissionBit(ACEnum.MODIFY_ANNOTATIONS_BIT, allowAnnotationModification)
            End If
        End Sub

        '/**
        ' * Me will tell if the user can fill in interactive forms.
        ' *
        ' * @return true If supplied with the user password they are allowed to fill in form fields.
        ' */
        Public Function canFillInForm() As Boolean
            Return isPermissionBitOn(ACEnum.FILL_IN_FORM_BIT)
        End Function

        '/**
        ' * Set if the user can fill in interactive forms.
        ' * Me method will have no effect if the object is in read only mode
        ' *
        ' * @param allowFillingInForm A boolean determining if the user can fill in interactive forms.
        ' */
        Public Sub setCanFillInForm(ByVal allowFillingInForm As Boolean)
            If (Not [readOnly]) Then
                setPermissionBit(ACEnum.FILL_IN_FORM_BIT, allowFillingInForm)
            End If
        End Sub

        '/**
        ' * Me will tell if the user can extract text and images from the PDF document
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
        ' * Me method will have no effect if the object is in read only mode
        ' *
        ' * @param allowExtraction A boolean determining if the user can extract content
        ' *                        from the document.
        ' */
        Public Sub setCanExtractForAccessibility(ByVal allowExtraction As Boolean)
            If (Not [readOnly]) Then
                setPermissionBit(ACEnum.EXTRACT_FOR_ACCESSIBILITY_BIT, allowExtraction)
            End If
        End Sub

        '/**
        ' * Me will tell if the user can insert/rotate/delete pages.
        ' *
        ' * @return true If supplied with the user password they are allowed to extract content
        ' *              from the PDF document
        ' */
        Public Function canAssembleDocument() As Boolean
            Return isPermissionBitOn(ACEnum.ASSEMBLE_DOCUMENT_BIT)
        End Function

        '/**
        ' * Set if the user can insert/rotate/delete pages.
        ' * Me method will have no effect if the object is in read only mode
        ' *
        ' * @param allowAssembly A boolean determining if the user can assemble the document.
        ' */
        Public Sub setCanAssembleDocument(ByVal allowAssembly As Boolean)
            If (Not [readOnly]) Then
                setPermissionBit(ACEnum.ASSEMBLE_DOCUMENT_BIT, allowAssembly)
            End If
        End Sub

        '/**
        ' * Me will tell if the user can print the document in a degraded format.
        ' *
        ' * @return true If supplied with the user password they are allowed to print the
        ' *              document in a degraded format.
        ' */
        Public Function canPrintDegraded() As Boolean
            Return isPermissionBitOn(ACEnum.DEGRADED_PRINT_BIT)
        End Function

        '/**
        ' * Set if the user can print the document in a degraded format.
        ' * Me method will have no effect if the object is in read only mode
        ' *
        ' * @param allowAssembly A boolean determining if the user can print the
        ' *        document in a degraded format.
        ' */
        Public Sub setCanPrintDegraded(ByVal allowAssembly As Boolean)
            If (Not [readOnly]) Then
                setPermissionBit(ACEnum.DEGRADED_PRINT_BIT, allowAssembly)
            End If
        End Sub

        '/**
        ' * Locks the access permission read only (ie, the setters will have no effects).
        ' * After that, the object cannot be unlocked.
        ' * Me method is used for the currentAccessPermssion of a document to avoid
        ' * users to change access permission.
        ' */
        Public Sub setReadOnly()
            [readOnly] = True
        End Sub

        '/**
        ' * Me will tell if the object has been set as read only.
        ' *
        ' * @return true if the object is in read only mode.
        ' */
        Public Function isReadOnly() As Boolean
            Return [readOnly]
        End Function

        '/**
        ' * Indicates if any revision 3 access permission is set or not.
        ' * 
        ' * @return true if any revision 3 access permission is set
        ' */
        Protected Function hasAnyRevision3PermissionSet() As Boolean
            If (canFillInForm()) Then
                Return True
            End If
            If (canExtractForAccessibility()) Then
                Return True
            End If
            If (canAssembleDocument()) Then
                Return True
            End If
            If (canPrintDegraded()) Then
                Return True
            End If
            Return False
        End Function


    End Class

End Namespace

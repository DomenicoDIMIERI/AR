Imports FinSeA.org.apache.pdfbox.cos

Namespace org.apache.pdfbox.pdmodel.interactive.annotation

    '/**
    ' * This is the class that represents a rubber stamp annotation.
    ' * Introduced in PDF 1.3 specification
    ' *
    ' * @author Paul King
    ' * @version $Revision: 1.2 $
    ' */
    Public Class PDAnnotationRubberStamp
        Inherits PDAnnotationMarkup

        '/*
        ' * The various values of the rubber stamp as defined in
        ' * the PDF 1.6 reference Table 8.28
        ' */

        ''' <summary>
        ''' Constant for the name of a rubber stamp.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const NAME_APPROVED As String = "Approved"

        ''' <summary>
        ''' Constant for the name of a rubber stamp.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const NAME_EXPERIMENTAL As String = "Experimental"

        ''' <summary>
        ''' Constant for the name of a rubber stamp.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const NAME_NOT_APPROVED As String = "NotApproved"

        ''' <summary>
        ''' Constant for the name of a rubber stamp.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const NAME_AS_IS As String = "AsIs"

        ''' <summary>
        ''' Constant for the name of a rubber stamp.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const NAME_EXPIRED As String = "Expired"

        ''' <summary>
        ''' Constant for the name of a rubber stamp.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const NAME_NOT_FOR_PUBLIC_RELEASE As String = "NotForPublicRelease"

        ''' <summary>
        ''' Constant for the name of a rubber stamp.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const NAME_FOR_PUBLIC_RELEASE As String = "ForPublicRelease"

        ''' <summary>
        ''' Constant for the name of a rubber stamp.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const NAME_DRAFT As String = "Draft"

        ''' <summary>
        ''' Constant for the name of a rubber stamp.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const NAME_FOR_COMMENT As String = "ForComment"

        ''' <summary>
        ''' Constant for the name of a rubber stamp.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const NAME_TOP_SECRET As String = "TopSecret"

        ''' <summary>
        ''' Constant for the name of a rubber stamp.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const NAME_DEPARTMENTAL As String = "Departmental"

        ''' <summary>
        ''' Constant for the name of a rubber stamp.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const NAME_CONFIDENTIAL As String = "Confidential"

        ''' <summary>
        ''' Constant for the name of a rubber stamp.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const NAME_FINAL As String = "Final"

        ''' <summary>
        ''' Constant for the name of a rubber stamp.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const NAME_SOLD As String = "Sold"

        ''' <summary>
        ''' The type of annotation.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const SUB_TYPE As String = "Stamp"

        '/**
        ' * Constructor.
        ' */
        Public Sub New()
            MyBase.New()
            getDictionary().setItem(COSName.SUBTYPE, COSName.getPDFName(SUB_TYPE))
        End Sub

        '/**
        ' * Creates a Rubber Stamp annotation from a COSDictionary, expected to be
        ' * a correct object definition.
        ' *
        ' * @param field the PDF objet to represent as a field.
        ' */
        Public Sub New(ByVal field As COSDictionary)
            MyBase.New(field)
        End Sub

        '/**
        ' * This will set the name (and hence appearance, AP taking precedence)
        ' * For this annotation.   See the NAME_XXX constants for valid values.
        ' *
        ' * @param name The name of the rubber stamp.
        ' */
        Public Sub setName(ByVal name As String)
            getDictionary().setName(COSName.NAME, name)
        End Sub

        '/**
        ' * This will retrieve the name (and hence appearance, AP taking precedence)
        ' * For this annotation.  The default is DRAFT.
        ' *
        ' * @return The name of this rubber stamp, see the NAME_XXX constants.
        ' */
        Public Function getName() As String
            Return getDictionary().getNameAsString(COSName.NAME, NAME_DRAFT)
        End Function


    End Class

End Namespace

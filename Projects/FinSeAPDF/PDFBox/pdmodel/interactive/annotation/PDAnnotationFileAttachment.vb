Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common.filespecification


Namespace org.apache.pdfbox.pdmodel.interactive.annotation

    '/**
    ' * This is the class that represents a file attachement.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.2 $
    ' */
    Public Class PDAnnotationFileAttachment
        Inherits PDAnnotationMarkup

        ''' <summary>
        ''' See get/setAttachmentName.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const ATTACHMENT_NAME_PUSH_PIN As String = "PushPin"

        ''' <summary>
        ''' See get/setAttachmentName.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const ATTACHMENT_NAME_GRAPH As String = "Graph"

        ''' <summary>
        ''' See get/setAttachmentName.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const ATTACHMENT_NAME_PAPERCLIP As String = "Paperclip"

        ''' <summary>
        ''' See get/setAttachmentName.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const ATTACHMENT_NAME_TAG As String = "Tag"

        ''' <summary>
        ''' The type of annotation.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const SUB_TYPE As String = "FileAttachment"

        '/**
        ' * Constructor.
        ' */
        Public Sub New()
            MyBase.New()
            getDictionary().setItem(COSName.SUBTYPE, COSName.getPDFName(SUB_TYPE))
        End Sub

        '/**
        ' * Creates a Link annotation from a COSDictionary, expected to be
        ' * a correct object definition.
        ' *
        ' * @param field the PDF objet to represent as a field.
        ' */
        Public Sub New(ByVal field As COSDictionary)
            MyBase.New(field)
        End Sub

        '/**
        ' * Return the attached file.
        ' *
        ' * @return The attached file.
        ' *
        ' * @throws IOException If there is an error creating the file spec.
        ' */
        Public Function getFile() As PDFileSpecification ' throws IOException
            Return PDFileSpecification.createFS(getDictionary().getDictionaryObject("FS"))
        End Function

        '/**
        ' * Set the attached file.
        ' *
        ' * @param file The file that is attached.
        ' */
        Public Sub setFile(ByVal file As PDFileSpecification)
            getDictionary().setItem("FS", file)
        End Sub

        '/**
        ' * This is the name used to draw the type of attachment.
        ' * See the ATTACHMENT_NAME_XXX constants.
        ' *
        ' * @return The name that describes the visual cue for the attachment.
        ' */
        Public Function getAttachmentName() As String
            Return getDictionary().getNameAsString("Name", ATTACHMENT_NAME_PUSH_PIN)
        End Function

        '/**
        ' * Set the name used to draw the attachement icon.
        ' * See the ATTACHMENT_NAME_XXX constants.
        ' *
        ' * @param name The name of the visual icon to draw.
        ' */
        Public Sub setAttachementName(ByVal name As String)
            getDictionary().setName("Name", name)
        End Sub

    End Class

End Namespace

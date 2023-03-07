Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos

Namespace org.apache.pdfbox.pdmodel.interactive.annotation

    '/**
    ' * This is the class that represents a popup annotation.
    ' * Introduced in PDF 1.3 specification
    ' *
    ' * @author Paul King
    ' * @version $Revision: 1.2 $
    ' */
    Public Class PDAnnotationPopup
        Inherits PDAnnotation

        ''' <summary>
        ''' The type of annotation.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const SUB_TYPE As String = "Popup"

        '/**
        ' * Constructor.
        ' */
        Public Sub New()
            MyBase.New()
            getDictionary().setItem(COSName.SUBTYPE, COSName.getPDFName(SUB_TYPE))
        End Sub

        '/**
        ' * Creates a popup annotation from a COSDictionary, expected to be a correct
        ' * object definition.
        ' *
        ' * @param field
        ' *            the PDF objet to represent as a field.
        ' */
        Public Sub New(ByVal field As COSDictionary)
            MyBase.New(field)
        End Sub

        '/**
        ' * This will set inital state of the annotation, open or closed.
        ' *
        ' * @param open
        ' *            Boolean value, true = open false = closed.
        ' */
        Public Sub setOpen(ByVal open As Boolean)
            getDictionary().setBoolean("Open", open)
        End Sub

        '/**
        ' * This will retrieve the initial state of the annotation, open Or closed
        ' * (default closed).
        ' *
        ' * @return The initial state, true = open false = closed.
        ' */
        Public Function getOpen() As Boolean
            Return getDictionary().getBoolean("Open", False)
        End Function

        '/**
        ' * This will set the markup annotation which this popup relates to.
        ' *
        ' * @param annot
        ' *            the markup annotation.
        ' */
        Public Sub setParent(ByVal annot As PDAnnotationMarkup)
            getDictionary().setItem(COSName.PARENT, annot.getDictionary())
        End Sub

        '/**
        ' * This will retrieve the markup annotation which this popup relates to.
        ' *
        ' * @return The parent markup annotation.
        ' */
        Public Function getParent() As PDAnnotationMarkup
            Dim am As PDAnnotationMarkup = Nothing
            Try
                am = PDAnnotation.createAnnotation(getDictionary().getDictionaryObject("Parent", "P"))
            Catch ioe As IOException
                ' Couldn't construct the annotation, so return null i.e. do nothing
            End Try
            Return am
        End Function

    End Class

End Namespace

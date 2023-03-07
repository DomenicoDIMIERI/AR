Imports FinSeA.org.apache.pdfbox.cos

Namespace org.apache.pdfbox.pdmodel.interactive.annotation

    '/**
    ' * This is the abstract class that represents a text markup annotation
    ' * Introduced in PDF 1.3 specification, except Squiggly lines in 1.4.
    ' *
    ' * @author Paul King
    ' * @version $Revision: 1.1 $
    ' */
    Public Class PDAnnotationTextMarkup
        Inherits PDAnnotationMarkup

        ''' <summary>
        ''' The types of annotation.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const SUB_TYPE_HIGHLIGHT As String = "Highlight"

        ''' <summary>
        ''' The types of annotation.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const SUB_TYPE_UNDERLINE As String = "Underline"

        ''' <summary>
        ''' The types of annotation.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const SUB_TYPE_SQUIGGLY As String = "Squiggly"

        ''' <summary>
        ''' The types of annotation.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const SUB_TYPE_STRIKEOUT As String = "StrikeOut"


        Private Sub New()
            ' Must be constructed with a subType or dictionary parameter
        End Sub

        '/**
        ' * Creates a TextMarkup annotation of the specified sub type.
        ' *
        ' * @param subType the subtype the annotation represents
        ' */
        Public Sub New(ByVal subType As String)
            MyBase.New()
            setSubtype(subType)

            ' Quad points are required, set and empty array
            setQuadPoints({0.0})
        End Sub

        '/**
        ' * Creates a TextMarkup annotation from a COSDictionary, expected to be a
        ' * correct object definition.
        ' *
        ' * @param field the PDF objet to represent as a field.
        ' */
        Public Sub New(ByVal field As COSDictionary)
            MyBase.New(field)
        End Sub

        '/**
        ' * This will set the set of quadpoints which encompass the areas of this
        ' * annotation.
        ' *
        ' * @param quadPoints
        ' *            an array representing the set of area covered
        ' */
        Public Sub setQuadPoints(ByVal quadPoints As Single())
            Dim newQuadPoints As COSArray = New COSArray()
            newQuadPoints.setFloatArray(quadPoints)
            getDictionary().setItem("QuadPoints", newQuadPoints)
        End Sub

        '/**
        ' * This will retrieve the set of quadpoints which encompass the areas of
        ' * this annotation.
        ' *
        ' * @return An array of floats representing the quad points.
        ' */
        Public Function getQuadPoints() As Single()
            Dim quadPoints As COSArray = getDictionary().getDictionaryObject("QuadPoints")
            If (quadPoints IsNot Nothing) Then
                Return quadPoints.toFloatArray()
            Else
                Return Nothing ' // Should never happen as this is a required item
            End If
        End Function

        '/**
        ' * This will set the sub type (and hence appearance, AP taking precedence) For
        ' * this annotation. See the SUB_TYPE_XXX constants for valid values.
        ' *
        ' * @param subType The subtype of the annotation
        ' */
        Public Sub setSubtype(ByVal subType As String)
            getDictionary().setName(COSName.SUBTYPE, subType)
        End Sub

        '/**
        ' * This will retrieve the sub type (and hence appearance, AP taking precedence)
        ' * For this annotation.
        ' *
        ' * @return The subtype of this annotation, see the SUB_TYPE_XXX constants.
        ' */
        Public Overrides Function getSubtype() As String
            Return getDictionary().getNameAsString(COSName.SUBTYPE)
        End Function

    End Class

End Namespace
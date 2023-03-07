Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.interactive.action
Imports FinSeA.org.apache.pdfbox.pdmodel.interactive.action.type
Imports FinSeA.org.apache.pdfbox.pdmodel.interactive.documentnavigation.destination

Namespace org.apache.pdfbox.pdmodel.interactive.annotation

    '/**
    ' * This is the class that represents a link annotation.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @author Paul King
    ' * @version $Revision: 1.3 $
    ' */
    Public Class PDAnnotationLink
        Inherits PDAnnotation


        ''' <summary>
        ''' Constant values of the Text as defined in the PDF 1.6 reference Table 8.19.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const HIGHLIGHT_MODE_NONE = "N"

        ''' <summary>
        ''' Constant values of the Text as defined in the PDF 1.6 reference Table 8.19.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const HIGHLIGHT_MODE_INVERT = "I"

        ''' <summary>
        ''' Constant values of the Text as defined in the PDF 1.6 reference Table 8.19.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const HIGHLIGHT_MODE_OUTLINE = "O"

        ''' <summary>
        ''' Constant values of the Text as defined in the PDF 1.6 reference Table 8.19.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const HIGHLIGHT_MODE_PUSH = "P"

        ''' <summary>
        ''' The type of annotation.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const SUB_TYPE As String = "Link"

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
        ' * Get the action to be performed when this annotation is to be activated.
        ' *
        ' * @return The action to be performed when this annotation is activated.
        ' *
        ' * TODO not all annotations have an A entry
        ' */
        Public Function getAction() As PDAction
            Dim action As COSDictionary = Me.getDictionary().getDictionaryObject(COSName.A)
            Return PDActionFactory.createAction(action)
        End Function

        '/**
        ' * Set the annotation action.
        ' * As of PDF 1.6 this is only used for Widget Annotations
        ' * @param action The annotation action.
        ' * TODO not all annotations have an A entry
        ' */
        Public Sub setAction(ByVal action As PDAction)
            Me.getDictionary().setItem(COSName.A, action)
        End Sub

        '/**
        ' * This will set the border style dictionary, specifying the width and dash
        ' * pattern used in drawing the line.
        ' *
        ' * @param bs the border style dictionary to set.
        ' * TODO not all annotations may have a BS entry
        ' *
        ' */
        Public Sub setBorderStyle(ByVal bs As PDBorderStyleDictionary)
            Me.getDictionary().setItem("BS", bs)
        End Sub

        '/**
        ' * This will retrieve the border style dictionary, specifying the width and
        ' * dash pattern used in drawing the line.
        ' *
        ' * @return the border style dictionary.
        ' * TODO not all annotations may have a BS entry
        ' */
        Public Function getBorderStyle() As PDBorderStyleDictionary
            Dim bs As COSDictionary = Me.getDictionary().getItem(COSName.getPDFName("BS"))
            If (bs IsNot Nothing) Then
                Return New PDBorderStyleDictionary(bs)
            Else
                Return Nothing
            End If
        End Function

        '/**
        ' * Get the destination to be displayed when the annotation is activated.  Either
        ' * this or the A should be set but not both.
        ' *
        ' * @return The destination for this annotation.
        ' *
        ' * @throws IOException If there is an error creating the destination.
        ' */
        Public Function getDestination() As PDDestination ' throws IOException
            Dim base As COSBase = getDictionary().getDictionaryObject(COSName.DEST)
            Dim retval As PDDestination = PDDestination.create(base)

            Return retval
        End Function

        '/**
        ' * The new destination value.
        ' *
        ' * @param dest The updated destination.
        ' */
        Public Sub setDestination(ByVal dest As PDDestination)
            getDictionary().setItem(COSName.DEST, dest)
        End Sub

        '/**
        ' * Set the highlight mode for when the mouse is depressed.
        ' * See the HIGHLIGHT_MODE_XXX constants.
        ' *
        ' * @return The string representation of the highlight mode.
        ' */
        Public Function getHighlightMode() As String
            Return getDictionary().getNameAsString(COSName.H, HIGHLIGHT_MODE_INVERT)
        End Function

        '/**
        ' * Set the highlight mode.  See the HIGHLIGHT_MODE_XXX constants.
        ' *
        ' * @param mode The new highlight mode.
        ' */
        Public Sub setHighlightMode(ByVal mode As String)
            getDictionary().setName(COSName.H, mode)
        End Sub

        '/**
        ' * This will set the previous URI action, in case it
        ' * needs to be retrieved at later date.
        ' *
        ' * @param pa The previous URI.
        ' */
        Public Sub setPreviousURI(ByVal pa As PDActionURI)
            getDictionary().setItem("PA", pa)
        End Sub

        '/**
        ' * This will set the previous URI action, in case it's
        ' * needed.
        ' *
        ' * @return The previous URI.
        ' */
        Public Function getPreviousURI() As PDActionURI
            Dim pa As COSDictionary = getDictionary().getDictionaryObject("PA")
            If (pa IsNot Nothing) Then
                Return New PDActionURI(pa)
            Else
                Return Nothing
            End If
        End Function

        '/**
        ' * This will set the set of quadpoints which encompass the areas of this
        ' * annotation which will activate.
        ' *
        ' * @param quadPoints
        ' *            an array representing the set of area covered.
        ' */
        Public Sub setQuadPoints(ByVal quadPoints As Single())
            Dim newQuadPoints As COSArray = New COSArray()
            newQuadPoints.setFloatArray(quadPoints)
            getDictionary().setItem("QuadPoints", newQuadPoints)
        End Sub

        '/**
        ' * This will retrieve the set of quadpoints which encompass the areas of
        ' * this annotation which will activate.
        ' *
        ' * @return An array of floats representing the quad points.
        ' */
        Public Function getQuadPoints() As Single()
            Dim quadPoints As COSArray = getDictionary().getDictionaryObject("QuadPoints")
            If (quadPoints IsNot Nothing) Then
                Return quadPoints.toFloatArray()
            Else
                Return Nothing '// Should never happen as this is a required item
            End If
        End Function

    End Class

End Namespace

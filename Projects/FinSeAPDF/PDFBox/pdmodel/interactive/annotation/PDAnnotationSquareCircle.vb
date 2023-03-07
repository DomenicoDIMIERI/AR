Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.color
Imports FinSeA.org.apache.pdfbox.pdmodel.common

Namespace org.apache.pdfbox.pdmodel.interactive.annotation

    '/**
    ' * This is the class that represents a rectangular or eliptical annotation
    ' * Introduced in PDF 1.3 specification .
    ' *
    ' * @author Paul King
    ' * @version $Revision: 1.1 $
    ' */
    Public Class PDAnnotationSquareCircle
        Inherits PDAnnotationMarkup

        ''' <summary>
        ''' Constant for a Rectangular type of annotation.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const SUB_TYPE_SQUARE As String = "Square"

        ''' <summary>
        ''' Constant for an Eliptical type of annotation.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const SUB_TYPE_CIRCLE As String = "Circle"

        '/**
        ' * Creates a Circle or Square annotation of the specified sub type.
        ' *
        ' * @param subType the subtype the annotation represents.
        '     */
        Public Sub New(ByVal subType As String)
            MyBase.New()
            setSubtype(subType)
        End Sub

        '/**
        ' * Creates a Line annotation from a COSDictionary, expected to be a correct
        ' * object definition.
        ' *
        ' * @param field
        ' *            the PDF objet to represent as a field.
        ' */
        Public Sub New(ByVal field As COSDictionary)
            MyBase.New(field)
        End Sub


        '/**
        ' * This will set interior colour of the drawn area
        ' * Colour is in DeviceRGB colourspace.
        ' *
        ' * @param ic
        ' *            colour in the DeviceRGB colourspace.
        ' *
        ' */
        Public Sub setInteriorColour(ByVal ic As PDGamma)
            getDictionary().setItem("IC", ic)
        End Sub

        '/**
        ' * This will retrieve the interior colour of the drawn area
        ' * Colour is in DeviceRGB colourspace.
        ' *
        ' *
        ' * @return PDGamma object representing the colour.
        ' *
        ' */
        Public Function getInteriorColour() As PDGamma
            Dim ic As COSArray = getDictionary().getItem(COSName.getPDFName("IC"))
            If (ic IsNot Nothing) Then
                Return New PDGamma(ic)
            Else
                Return Nothing
            End If
        End Function


        '/**
        ' * This will set the border effect dictionary, specifying effects to be applied
        ' * when drawing the line.
        ' *
        ' * @param be The border effect dictionary to set.
        ' *
        ' */
        Public Sub setBorderEffect(ByVal be As PDBorderEffectDictionary)
            getDictionary().setItem("BE", be)
        End Sub

        '/**
        ' * This will retrieve the border effect dictionary, specifying effects to be
        ' * applied used in drawing the line.
        ' *
        ' * @return The border effect dictionary
        ' */
        Public Function getBorderEffect() As PDBorderEffectDictionary
            Dim be As COSDictionary = getDictionary().getDictionaryObject("BE")
            If (be IsNot Nothing) Then
                Return New PDBorderEffectDictionary(be)
            Else
                Return Nothing
            End If
        End Function

        '/**
        ' * This will set the rectangle difference rectangle. Giving the difference
        ' * between the annotations rectangle and where the drawing occurs.
        '     * (To take account of any effects applied through the BE entry forexample)
        ' *
        ' * @param rd the rectangle difference
        ' *
        ' */
        Public Sub setRectDifference(ByVal rd As PDRectangle)
            getDictionary().setItem("RD", rd)
        End Sub

        '/**
        ' * This will get the rectangle difference rectangle. Giving the difference
        ' * between the annotations rectangle and where the drawing occurs.
        '     * (To take account of any effects applied through the BE entry forexample)
        ' *
        ' * @return the rectangle difference
        ' */
        Public Function getRectDifference() As PDRectangle
            Dim rd As COSArray = getDictionary().getDictionaryObject("RD")
            If (rd IsNot Nothing) Then
                Return New PDRectangle(rd)
            Else
                Return Nothing
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

    End Class

End Namespace

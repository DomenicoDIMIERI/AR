Imports System.IO

Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.color

Namespace org.apache.pdfbox.pdmodel.graphics.shading

    '/**
    ' * This represents resources for a shading.
    ' *
    ' * @version $Revision: 1.0 $
    ' */
    Public MustInherit Class PDShadingResources
        Implements COSObjectable

        Private dictionary As COSDictionary
        Private background As COSArray = Nothing
        Private bBox As PDRectangle = Nothing
        Private colorspace As PDColorSpace = Nothing

        ''' <summary>
        ''' shading type 1 = function based shading.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const SHADING_TYPE1 As Integer = 1

        ''' <summary>
        ''' shading type 2 = axial shading.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const SHADING_TYPE2 As Integer = 2

        ''' <summary>
        ''' shading type 3 = radial shading.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const SHADING_TYPE3 As Integer = 3

        ''' <summary>
        ''' shading type 4 = Free-Form Gouraud-Shaded Triangle Meshes.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const SHADING_TYPE4 As Integer = 4

        ''' <summary>
        ''' shading type 5 = Lattice-Form Gouraud-Shaded Triangle Meshes.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const SHADING_TYPE5 As Integer = 5

        ''' <summary>
        ''' shading type 6 = Coons Patch Meshes.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const SHADING_TYPE6 As Integer = 6

        ''' <summary>
        ''' shading type 7 = Tensor-Product Patch Meshes.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const SHADING_TYPE7 As Integer = 7

        '/**
        ' * Default constructor.
        ' */
        Public Sub New()
            dictionary = New COSDictionary()
        End Sub

        '/**
        ' * Constructor using the given shading dictionary.
        ' *
        ' * @param shadingDictionary The dictionary for this shading.
        ' */
        Public Sub New(ByVal shadingDictionary As COSDictionary)
            dictionary = shadingDictionary
        End Sub

        '/**
        ' * This will get the underlying dictionary.
        ' *
        ' * @return The dictionary for this shading.
        ' */
        Public Function getCOSDictionary() As COSDictionary
            Return dictionary
        End Function

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return dictionary
        End Function

        '/**
        ' * This will return the type.
        ' *
        ' * @return The type of object that this is.
        ' */
        Public Function getObjectType() As String
            Return COSName.SHADING.getName()
        End Function

        '/**
        ' * This will set the shading type.
        ' *
        ' * @param shadingType The new shading type.
        ' */
        Public Sub setShadingType(ByVal shadingType As Integer)
            dictionary.setInt(COSName.SHADING_TYPE, shadingType)
        End Sub

        '/**
        ' * This will return the shading type.
        ' *
        ' * @return The shading type
        ' */
        Public MustOverride Function getShadingType() As Integer

        '/**
        ' * This will set the background.
        ' *
        ' * @param newBackground The new background.
        ' */
        Public Sub setBackground(ByVal newBackground As COSArray)
            background = newBackground
            dictionary.setItem(COSName.BACKGROUND, newBackground)
        End Sub

        '/**
        ' * This will return the background.
        ' *
        ' * @return The background
        ' */
        Public Function getBackground() As COSArray
            If (background Is Nothing) Then
                background = dictionary.getDictionaryObject(COSName.BACKGROUND)
            End If
            Return background
        End Function

        '/**
        ' * An array of four numbers in the form coordinate system (see
        ' * below), giving the coordinates of the left, bottom, right, and top edges,
        ' * respectively, of the shadings's bounding box.
        ' *
        ' * @return The BBox of the form.
        ' */
        Public Function getBBox() As PDRectangle
            If (bBox Is Nothing) Then
                Dim array As COSArray = dictionary.getDictionaryObject(COSName.BBOX)
                If (array IsNot Nothing) Then
                    bBox = New PDRectangle(array)
                End If
            End If
            Return bBox
        End Function

        '/**
        ' * This will set the BBox (bounding box) for this Shading.
        ' *
        ' * @param newBBox The new BBox.
        ' */
        Public Sub setBBox(ByVal newBBox As PDRectangle)
            bBox = newBBox
            If (bBox Is Nothing) Then
                dictionary.removeItem(COSName.BBOX)
            Else
                dictionary.setItem(COSName.BBOX, bBox.getCOSArray())
            End If
        End Sub

        '/**
        ' * This will set the AntiAlias value.
        ' *
        ' * @param antiAlias The new AntiAlias value.
        ' */
        Public Sub setAntiAlias(ByVal antiAlias As Boolean)
            dictionary.setBoolean(COSName.ANTI_ALIAS, antiAlias)
        End Sub

        '/**
        ' * This will return the AntiAlias value.
        ' *
        ' * @return The AntiAlias value
        ' */
        Public Function getAntiAlias() As Boolean
            Return dictionary.getBoolean(COSName.ANTI_ALIAS, False)
        End Function

        '/**
        ' * This will get the color space or null if none exists.
        ' *
        ' * @return The color space for the shading.
        ' *
        ' * @throws IOException If there is an error getting the colorspace.
        ' */
        Public Function getColorSpace() As PDColorSpace 'throws IOException
            If (colorspace Is Nothing) Then
                Dim colorSpaceDictionary As COSBase = dictionary.getDictionaryObject(COSName.CS, COSName.COLORSPACE)
                colorspace = PDColorSpaceFactory.createColorSpace(colorSpaceDictionary)
            End If
            Return colorspace
        End Function

        '/**
        ' * This will set the color space for the shading.
        ' *
        ' * @param newColorspace The color space
        ' */
        Public Sub setColorSpace(ByVal newColorspace As PDColorSpace)
            colorspace = newColorspace
            If (newColorspace IsNot Nothing) Then
                dictionary.setItem(COSName.COLORSPACE, newColorspace.getCOSObject())
            Else
                dictionary.removeItem(COSName.COLORSPACE)
            End If
        End Sub

        '/**
        ' * Create the correct PD Model shading based on the COS base shading.
        ' * 
        ' * @param resourceDictionary the COS shading dictionary
        ' * 
        ' * @return the newly created shading resources object
        ' * 
        ' * @throws IOException If we are unable to create the PDShading object.
        ' */
        Public Shared Function create(ByVal resourceDictionary As COSDictionary) As PDShadingResources 'throws IOException
            Dim shading As PDShadingResources = Nothing
            Dim shadingType As Integer = resourceDictionary.getInt(COSName.SHADING_TYPE, 0)
            Select Case (shadingType)
                Case SHADING_TYPE1 : shading = New PDShadingType1(resourceDictionary)
                Case SHADING_TYPE2 : shading = New PDShadingType2(resourceDictionary)
                Case SHADING_TYPE3 : shading = New PDShadingType3(resourceDictionary)
                Case SHADING_TYPE4 : shading = New PDShadingType4(resourceDictionary)
                Case SHADING_TYPE5 : shading = New PDShadingType5(resourceDictionary)
                Case SHADING_TYPE6 : shading = New PDShadingType6(resourceDictionary)
                Case SHADING_TYPE7 : shading = New PDShadingType7(resourceDictionary)
                Case Else
                    Throw New IOException("Error: Unknown shading type " & shadingType)
            End Select
            Return shading
        End Function

    End Class

End Namespace
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.color

Namespace org.apache.pdfbox.pdmodel.documentinterchange.prepress


    '/**
    ' * The Box Style specifies visual characteristics for displaying box areas.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.3 $
    ' */
    Public Class PDBoxStyle
        Implements COSObjectable

        ''' <summary>
        ''' Style for guideline.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const GUIDELINE_STYLE_SOLID As String = "S"

        ''' <summary>
        ''' Style for guideline.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const GUIDELINE_STYLE_DASHED As String = "D"

        Private dictionary As COSDictionary

        '/**
        ' * Default Constructor.
        ' *
        ' */
        Public Sub New()
            dictionary = New COSDictionary()
        End Sub

        '/**
        ' * Constructor for an existing BoxStyle element.
        ' *
        ' * @param dic The existing dictionary.
        ' */
        Public Sub New(ByVal dic As COSDictionary)
            dictionary = dic
        End Sub

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return dictionary
        End Function

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Function getDictionary() As COSDictionary
            Return dictionary
        End Function

        '/**
        ' * Get the color to be used for the guidelines.  This is guaranteed to
        ' * not return null.  The color space will always be DeviceRGB and the
        ' * default color is [0,0,0].
        ' *
        ' *@return The guideline color.
        ' */
        Public Function getGuidelineColor() As PDColorState
            Dim colorValues As COSArray = dictionary.getDictionaryObject("C")
            If (colorValues Is Nothing) Then
                colorValues = New COSArray()
                colorValues.add(COSInteger.ZERO)
                colorValues.add(COSInteger.ZERO)
                colorValues.add(COSInteger.ZERO)
                dictionary.setItem("C", colorValues)
            End If
            Dim instance As PDColorState = New PDColorState(colorValues)
            instance.setColorSpace(PDDeviceRGB.INSTANCE)
            Return instance
        End Function

        '/**
        ' * Set the color space instance for this box style.  This must be a
        ' * PDDeviceRGB!
        ' *
        ' * @param color The new colorspace value.
        ' */
        Public Sub setGuideLineColor(ByVal color As PDColorState)
            Dim values As COSArray = Nothing
            If (color IsNot Nothing) Then
                values = color.getCOSColorSpaceValue()
            End If
            dictionary.setItem("C", values)
        End Sub

        '/**
        ' * Get the width of the of the guideline in default user space units.
        ' * The default is 1.
        ' *
        ' * @return The width of the guideline.
        ' */
        Public Function getGuidelineWidth() As Single
            Return dictionary.getFloat("W", 1)
        End Function

        '/**
        ' * Set the guideline width.
        ' *
        ' * @param width The width in default user space units.
        ' */
        Public Sub setGuidelineWidth(ByVal width As Single)
            dictionary.setFloat("W", width)
        End Sub

        '/**
        ' * Get the style for the guideline.  The default is "S" for solid.
        ' *
        ' * @return The guideline style.
        ' * @see PDBoxStyle#GUIDELINE_STYLE_DASHED
        ' * @see PDBoxStyle#GUIDELINE_STYLE_SOLID
        ' */
        Public Function getGuidelineStyle() As String
            Return dictionary.getNameAsString("S", GUIDELINE_STYLE_SOLID)
        End Function

        '/**
        ' * Set the style for the box.
        ' *
        ' * @param style The style for the box line.
        ' * @see PDBoxStyle#GUIDELINE_STYLE_DASHED
        ' * @see PDBoxStyle#GUIDELINE_STYLE_SOLID
        ' */
        Public Sub setGuidelineStyle(ByVal style As String)
            dictionary.setName("S", style)
        End Sub

        '/**
        ' * Get the line dash pattern for this box style.  This is guaranteed to not
        ' * return null.  The default is (2),0.
        ' *
        ' * @return The line dash pattern.
        ' */
        Public Function getLineDashPattern() As PDLineDashPattern
            Dim pattern As PDLineDashPattern = Nothing
            Dim d As COSArray = dictionary.getDictionaryObject("D")
            If (d Is Nothing) Then
                d = New COSArray()
                d.add(COSInteger.THREE)
                dictionary.setItem("D", d)
            End If
            Dim lineArray As COSArray = New COSArray()
            lineArray.add(d)
            'dash phase is not specified and assumed to be zero.
            lineArray.add(COSInteger.ZERO)
            pattern = New PDLineDashPattern(lineArray)
            Return pattern
        End Function

        '/**
        ' * Set the line dash pattern associated with this box style.
        ' *
        ' * @param pattern The patter for this box style.
        ' */
        Public Sub setLineDashPattern(ByVal pattern As PDLineDashPattern)
            Dim array As COSArray = Nothing
            If (pattern IsNot Nothing) Then
                array = pattern.getCOSDashPattern()
            End If
            dictionary.setItem("D", array)
        End Sub

    End Class

End Namespace

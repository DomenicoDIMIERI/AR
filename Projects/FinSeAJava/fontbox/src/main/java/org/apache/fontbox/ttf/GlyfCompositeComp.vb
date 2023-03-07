Imports System.IO

Namespace org.apache.fontbox.ttf

    '/**
    ' * This class is based on code from Apache Batik a subproject of Apache XMLGraphics. see
    ' * http://xmlgraphics.apache.org/batik/ for further details.
    ' */
    Public Class GlyfCompositeComp


        ' Flags for composite glyphs.

        ''' <summary>
        ''' If set, the arguments are words; otherwise, they are bytes.
        ''' </summary>
        ''' <remarks></remarks>
        Protected Friend Const ARG_1_AND_2_ARE_WORDS As Short = &H1

        ''' <summary>
        ''' If set, the arguments are xy values; otherwise they are points.
        ''' </summary>
        ''' <remarks></remarks>
        Protected Friend Const ARGS_ARE_XY_VALUES As Short = &H2

        ''' <summary>
        ''' If set, xy values are rounded to those of the closest grid lines.
        ''' </summary>
        ''' <remarks></remarks>
        Protected Friend Const ROUND_XY_TO_GRID As Short = &H4

        ''' <summary>
        ''' If set, there is a simple scale; otherwise, scale = 1.0.
        ''' </summary>
        ''' <remarks></remarks>
        Protected Friend Const WE_HAVE_A_SCALE As Short = &H8

        ''' <summary>
        ''' Indicates at least one more glyph after Me one.
        ''' </summary>
        ''' <remarks></remarks>
        Protected Friend Const MORE_COMPONENTS As Short = &H20

        ''' <summary>
        ''' The x direction will use a different scale from the y direction.
        ''' </summary>
        ''' <remarks></remarks>
        Protected Friend Const WE_HAVE_AN_X_AND_Y_SCALE As Short = &H40

        ''' <summary>
        ''' There is a 2 by2 transformation that will be used to scale the component.
        ''' </summary>
        ''' <remarks></remarks>
        Protected Friend Const WE_HAVE_A_TWO_BY_TWO As Short = &H80

        ''' <summary>
        ''' Following the last component are instructions for the composite character.
        ''' </summary>
        ''' <remarks></remarks>
        Protected Friend Const WE_HAVE_INSTRUCTIONS As Short = &H100

        ''' <summary>
        ''' If set, Me forces the aw and lsb (and rsb) for the composite to be equal to those from Me original glyph.
        ''' </summary>
        ''' <remarks></remarks>
        Protected Friend Const USE_MY_METRICS As Short = &H200

        Private firstIndex As Integer
        Private firstContour As Integer
        Private argument1 As Short
        Private argument2 As Short
        Private flags As Short
        Private glyphIndex As Integer
        Private xscale As Double = 1.0
        Private yscale As Double = 1.0
        Private scale01 As Double = 0.0
        Private scale10 As Double = 0.0
        Private xtranslate As Integer = 0
        Private ytranslate As Integer = 0
        Private point1 As Integer = 0
        Private point2 As Integer = 0

        '/**
        ' * Constructor.
        ' * 
        ' * @param bais the stream to be read
        ' * @throws IOException is thrown if something went wrong
        ' */
        Protected Friend Sub New(ByVal bais As TTFDataStream)
            flags = bais.readSignedShort()
            glyphIndex = bais.readUnsignedShort() ' number of glyph in a font is uint16

            ' Get the arguments as just their raw values
            If ((flags And ARG_1_AND_2_ARE_WORDS) = ARG_1_AND_2_ARE_WORDS) Then
                argument1 = bais.readSignedShort()
                argument2 = bais.readSignedShort()
            Else
                argument1 = bais.readUnsignedByte()
                argument2 = bais.readUnsignedByte()
            End If

            ' Assign the arguments according to the flags
            If ((flags And ARGS_ARE_XY_VALUES) = ARGS_ARE_XY_VALUES) Then
                xtranslate = argument1
                ytranslate = argument2
            Else
                ' TODO unused?
                point1 = argument1
                point2 = argument2
            End If

            ' Get the scale values (if any)
            If ((flags And WE_HAVE_A_SCALE) = WE_HAVE_A_SCALE) Then
                Dim i As Short = bais.readSignedShort()
                xscale = yscale = CDbl(i) / &H4000
            ElseIf ((flags And WE_HAVE_AN_X_AND_Y_SCALE) = WE_HAVE_AN_X_AND_Y_SCALE) Then
                Dim i As Short = bais.readSignedShort()
                xscale = CDbl(i) / &H4000
                i = bais.readSignedShort()
                yscale = CDbl(i) / &H4000
            ElseIf ((flags And WE_HAVE_A_TWO_BY_TWO) = WE_HAVE_A_TWO_BY_TWO) Then
                Dim i As Integer = bais.readSignedShort()
                xscale = CDbl(i) / &H4000
                i = bais.readSignedShort()
                scale01 = CDbl(i) / &H4000
                i = bais.readSignedShort()
                scale10 = CDbl(i) / &H4000
                i = bais.readSignedShort()
                yscale = CDbl(i) / &H4000
            End If
        End Sub

        '/**
        ' * Sets the first index.
        ' * 
        ' * @param idx the first index
        ' */
        Public Sub setFirstIndex(ByVal idx As Integer)
            firstIndex = idx
        End Sub

        '/**
        ' * Returns the first index.
        ' * 
        ' * @return the first index.
        ' */
        Public Function getFirstIndex() As Integer
            Return firstIndex
        End Function

        '/**
        ' * Sets the index for the first contour.
        ' * 
        ' * @param idx the index of the first contour
        ' */
        Public Sub setFirstContour(ByVal idx As Integer)
            firstContour = idx
        End Sub

        '/**
        ' * Returns the index of the first contour.
        ' * 
        ' * @return the index of the first contour.
        ' */
        Public Function getFirstContour() As Integer
            Return firstContour
        End Function

        '/**
        ' * Returns argument 1.
        ' * 
        ' * @return argument 1.
        ' */
        Public Function getArgument1() As Short
            Return argument1
        End Function

        '/**
        ' * Returns argument 2.
        ' * 
        ' * @return argument 2.
        ' */
        Public Function getArgument2() As Short
            Return argument2
        End Function

        '/**
        ' * Returns the flags of the glyph.
        ' * 
        ' * @return the flags.
        ' */
        Public Function getFlags() As Short
            Return flags
        End Function

        '/**
        ' * Returns the index of the first contour.
        ' * 
        ' * @return index of the first contour.
        ' */
        Public Function getGlyphIndex() As Integer
            Return glyphIndex
        End Function

        '/**
        ' * Returns the scale-01 value.
        ' * 
        ' * @return the scale-01 value.
        ' */
        Public Function getScale01() As Double
            Return scale01
        End Function

        '/**
        ' * Returns the scale-10 value.
        ' * 
        ' * @return the scale-10 value.
        ' */
        Public Function getScale10() As Double
            Return scale10
        End Function

        '/**
        ' * Returns the x-scaling value.
        ' * 
        ' * @return the x-scaling value.
        ' */
        Public Function getXScale() As Double
            Return xscale
        End Function

        '/**
        ' * Returns the y-scaling value.
        ' * 
        ' * @return the y-scaling value.
        ' */
        Public Function getYScale() As Double
            Return yscale
        End Function

        '/**
        ' * Returns the x-translation value.
        ' * 
        ' * @return the x-translation value.
        ' */
        Public Function getXTranslate() As Integer
            Return xtranslate
        End Function

        '/**
        ' * Returns the y-translation value.
        ' * 
        ' * @return the y-translation value.
        ' */
        Public Function getYTranslate() As Integer
            Return ytranslate
        End Function

        '/**
        ' * Transforms an x-coordinate of a point for Me component.
        ' * 
        ' * @param x The x-coordinate of the point to transform
        ' * @param y The y-coordinate of the point to transform
        ' * @return The transformed x-coordinate
        ' */
        Public Function scaleX(ByVal x As Integer, ByVal y As Integer) As Integer
            Return Math.round((x * xscale + y * scale10))
        End Function

        '/**
        ' * Transforms a y-coordinate of a point for Me component.
        ' * 
        ' * @param x The x-coordinate of the point to transform
        ' * @param y The y-coordinate of the point to transform
        ' * @return The transformed y-coordinate
        ' */
        Public Function scaleY(ByVal x As Integer, ByVal y As Integer) As Integer
            Return Math.round((x * scale01 + y * yscale))
        End Function

    End Class

End Namespace

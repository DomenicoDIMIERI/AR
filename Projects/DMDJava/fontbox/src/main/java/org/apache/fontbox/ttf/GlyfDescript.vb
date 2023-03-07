Imports System.IO

Namespace org.apache.fontbox.ttf


    '/**
    ' * This class is based on code from Apache Batik a subproject of Apache XMLGraphics.
    ' * see http://xmlgraphics.apache.org/batik/ for further details.
    ' * 
    ' */
    Public MustInherit Class GlyfDescript
        Implements GlyphDescription

        'Flags describing a coordinate of a glyph.

        ''' <summary>
        ''' if set, the point is on the curve.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const ON_CURVE As Byte = &H1

        ''' <summary>
        ''' if set, the x-coordinate is 1 byte long.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const X_SHORT_VECTOR As Byte = &H2

        ''' <summary>
        ''' if set, the y-coordinate is 1 byte long.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const Y_SHORT_VECTOR As Byte = &H4

        ''' <summary>
        ''' if set, the next byte specifies the number of additional times Me set of flags is to be repeated.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const REPEAT As Byte = &H8

        ''' <summary>
        ''' This flag as two meanings, depending on how the
        ''' x-short vector flags is set.
        ''' If the x-short vector is set, Me bit describes the sign
        ''' of the value, with 1 equaling positive and 0 positive.
        ''' If the x-short vector is not set and Me bit is also not
        ''' set, the current x-coordinate is a signed 16-bit delta vector.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const X_DUAL As Byte = &H10

        ''' <summary>
        ''' This flag as two meanings, depending on how the
        ''' y-short vector flags is set.
        ''' If the y-short vector is set, Me bit describes the sign
        ''' of the value, with 1 equaling positive and 0 positive.
        ''' If the y-short vector is not set and Me bit is also not
        ''' set, the current y-coordinate is a signed 16-bit delta vector.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const Y_DUAL As Byte = &H20

        Private instructions() As Integer
        Private contourCount As Integer

        '/**
        ' * Constructor.
        ' * 
        ' * @param numberOfContours the number of contours
        ' * @param bais the stream to be read
        ' * @throws IOException is thrown if something went wrong
        ' */
        Protected Sub New(ByVal numberOfContours As Short, ByVal bais As TTFDataStream)
            contourCount = numberOfContours
        End Sub

        Public Overridable Sub resolve() Implements GlyphDescription.resolve
        End Sub

        Public Overridable Function getContourCount() As Integer Implements GlyphDescription.getContourCount
            Return contourCount
        End Function

        '/**
        ' * Returns the hinting instructions.
        ' * @return an array containing the hinting instructions.
        ' */
        Public Function getInstructions() As Integer()
            Return instructions
        End Function

        '/**
        ' * Read the hinting instructions.
        ' * @param bais the stream to be read
        ' * @param count the number of instructions to be read 
        ' * @throws IOException is thrown if something went wrong
        ' */
        Protected Sub readInstructions(ByVal bais As TTFDataStream, ByVal count As Integer)
            instructions = Array.CreateInstance(GetType(Integer), count)
            instructions = bais.readUnsignedByteArray(count)
        End Sub

        Public MustOverride Function getEndPtOfContours(i As Integer) As Integer Implements GlyphDescription.getEndPtOfContours

        Public MustOverride Function getFlags(i As Integer) As Byte Implements GlyphDescription.getFlags

        Public MustOverride Function getPointCount() As Integer Implements GlyphDescription.getPointCount

        Public MustOverride Function getXCoordinate(i As Integer) As Short Implements GlyphDescription.getXCoordinate

        Public MustOverride Function getYCoordinate(i As Integer) As Short Implements GlyphDescription.getYCoordinate

        Public MustOverride Function isComposite() As Boolean Implements GlyphDescription.isComposite


    End Class

End Namespace

Imports System.IO
Imports FinSeA.org.apache.fontbox.util

Namespace org.apache.fontbox.ttf

    '/**
    ' * A glyph data record in the glyf table.
    ' * 
    ' * @author Ben Litchfield (ben@benlitchfield.com)
    ' * @version $Revision: 1.1 $
    ' */
    Public Class GlyphData

        Private xMin As Short
        Private yMin As Short
        Private xMax As Short
        Private yMax As Short
        Private boundingBox As BoundingBox = Nothing
        Private numberOfContours As Short
        Private glyphDescription As GlyfDescript = Nothing

        '/**
        ' * This will read the required data from the stream.
        ' * 
        ' * @param ttf The font that is being read.
        ' * @param data The stream to read the data from.
        ' * @throws IOException If there is an error reading the data.
        ' */
        Public Sub initData(ByVal ttf As TrueTypeFont, ByVal data As TTFDataStream)
            numberOfContours = data.readSignedShort()
            xMin = data.readSignedShort()
            yMin = data.readSignedShort()
            xMax = data.readSignedShort()
            yMax = data.readSignedShort()
            boundingBox = New BoundingBox(xMin, yMin, xMax, yMax)

            If (numberOfContours >= 0) Then
                ' create a simple glyph
                glyphDescription = New GlyfSimpleDescript(numberOfContours, data)
            Else
                ' create a composite glyph
                glyphDescription = New GlyfCompositeDescript(data, ttf.getGlyph())
            End If
        End Sub

        '/**
        ' * @return Returns the boundingBox.
        ' */
        Public Function getBoundingBox() As BoundingBox
            Return boundingBox
        End Function

        '/**
        ' * @param boundingBoxValue The boundingBox to set.
        ' */
        Public Sub setBoundingBox(ByVal boundingBoxValue As BoundingBox)
            Me.boundingBox = boundingBoxValue
        End Sub

        '/**
        ' * @return Returns the numberOfContours.
        ' */
        Public Function getNumberOfContours() As Short
            Return numberOfContours
        End Function

        '/**
        ' * @param numberOfContoursValue The numberOfContours to set.
        ' */
        Public Sub setNumberOfContours(ByVal numberOfContoursValue As Short)
            Me.numberOfContours = numberOfContoursValue
        End Sub

        '/**
        ' * Returns the description of the glyph.
        ' * @return the glyph description
        ' */
        Public Function getDescription() As GlyphDescription
            Return glyphDescription
        End Function

        '/**
        ' * Returns the xMax value.
        ' * @return the XMax value
        ' */
        Public Function getXMaximum() As Short
            Return xMax
        End Function

        '/**
        ' * Returns the xMin value.
        ' * @return the xMin value
        ' */
        Public Function getXMinimum() As Short
            Return xMin
        End Function

        '/**
        ' * Returns the yMax value.
        ' * @return the yMax value
        ' */
        Public Function getYMaximum() As Short
            Return yMax
        End Function

        '/**
        ' * Returns the yMin value.
        ' * @return the yMin value
        ' */
        Public Function getYMinimum() As Short
            Return yMin
        End Function

    End Class

End Namespace

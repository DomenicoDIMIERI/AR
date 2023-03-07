Imports FinSeA.org.apache.pdfbox.cos
Imports System.Drawing
Imports FinSeA.org.apache.fontbox.util

Namespace org.apache.pdfbox.pdmodel.common


    '/**
    ' * This represents a rectangle in a PDF document.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.12 $
    ' */
    Public Class PDRectangle
        Implements COSObjectable

        Private rectArray As COSArray

        '/**
        ' * Constructor.
        ' *
        ' * Initializes to 0,0,0,0
        ' */
        Public Sub New()
            rectArray = New COSArray()
            rectArray.add(New COSFloat(0.0F))
            rectArray.add(New COSFloat(0.0F))
            rectArray.add(New COSFloat(0.0F))
            rectArray.add(New COSFloat(0.0F))
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param width The width of the rectangle.
        ' * @param height The height of the rectangle.
        ' */
        Public Sub New(ByVal width As Single, ByVal height As Single)
            rectArray = New COSArray()
            rectArray.add(New COSFloat(0.0F))
            rectArray.add(New COSFloat(0.0F))
            rectArray.add(New COSFloat(width))
            rectArray.add(New COSFloat(height))
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param box the bounding box to be used for the rectangle
        ' */
        Public Sub New(ByVal box As BoundingBox)
            rectArray = New COSArray()
            rectArray.add(New COSFloat(box.getLowerLeftX()))
            rectArray.add(New COSFloat(box.getLowerLeftY()))
            rectArray.add(New COSFloat(box.getUpperRightX()))
            rectArray.add(New COSFloat(box.getUpperRightY()))
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param array An array of numbers as specified in the PDF Reference for a rectangle type.
        ' */
        Public Sub New(ByVal array As COSArray)
            Dim values() As Single = array.toFloatArray()
            rectArray = New COSArray()
            ' we have to start with the lower left corner
            rectArray.add(New COSFloat(Math.Min(values(0), values(2))))
            rectArray.add(New COSFloat(Math.Min(values(1), values(3))))
            rectArray.add(New COSFloat(Math.Max(values(0), values(2))))
            rectArray.add(New COSFloat(Math.Max(values(1), values(3))))
        End Sub

        '/**
        ' * Method to determine if the x/y point is inside this rectangle.
        ' * @param x The x-coordinate to test.
        ' * @param y The y-coordinate to test.
        ' * @return True if the point is inside this rectangle.
        ' */
        Public Function contains(ByVal x As Single, ByVal y As Single) As Boolean
            Dim llx As Single = getLowerLeftX()
            Dim urx As Single = getUpperRightX()
            Dim lly As Single = getLowerLeftY()
            Dim ury As Single = getUpperRightY()
            Return x >= llx AndAlso x <= urx AndAlso y >= lly AndAlso y <= ury
        End Function

        '/**
        ' * This will create a translated rectangle based off of this rectangle, such
        ' * that the new rectangle retains the same dimensions(height/width), but the
        ' * lower left x,y values are zero. <br />
        ' * 100, 100, 400, 400 (llx, lly, urx, ury ) <br />
        ' * will be translated to 0,0,300,300
        ' *
        ' * @return A new rectangle that has been translated back to the origin.
        ' */
        Public Function createRetranslatedRectangle() As PDRectangle
            Dim retval As PDRectangle = New PDRectangle()
            retval.setUpperRightX(getWidth())
            retval.setUpperRightY(getHeight())
            Return retval
        End Function

        '/**
        ' * This will get the underlying array for this rectangle.
        ' *
        ' * @return The cos array.
        ' */
        Public Function getCOSArray() As COSArray
            Return rectArray
        End Function

        '/**
        ' * This will get the lower left x coordinate.
        ' *
        ' * @return The lower left x.
        ' */
        Public Function getLowerLeftX() As Single
            Return DirectCast(rectArray.get(0), COSNumber).floatValue()
        End Function

        '/**
        ' * This will set the lower left x coordinate.
        ' *
        ' * @param value The lower left x.
        ' */
        Public Sub setLowerLeftX(ByVal value As Single)
            rectArray.set(0, New COSFloat(value))
        End Sub

        '/**
        ' * This will get the lower left y coordinate.
        ' *
        ' * @return The lower left y.
        ' */
        Public Function getLowerLeftY() As Single
            Return DirectCast(rectArray.get(1), COSNumber).floatValue()
        End Function

        '/**
        ' * This will set the lower left y coordinate.
        ' *
        ' * @param value The lower left y.
        ' */
        Public Sub setLowerLeftY(ByVal value As Single)
            rectArray.set(1, New COSFloat(value))
        End Sub

        '/**
        ' * This will get the upper right x coordinate.
        ' *
        ' * @return The upper right x .
        ' */
        Public Function getUpperRightX() As Single
            Return DirectCast(rectArray.get(2), COSNumber).floatValue()
        End Function

        '/**
        ' * This will set the upper right x coordinate.
        ' *
        ' * @param value The upper right x .
        ' */
        Public Sub setUpperRightX(ByVal value As Single)
            rectArray.set(2, New COSFloat(value))
        End Sub

        '/**
        ' * This will get the upper right y coordinate.
        ' *
        ' * @return The upper right y.
        ' */
        Public Function getUpperRightY() As Single
            Return DirectCast(rectArray.get(3), COSNumber).floatValue()
        End Function

        '/**
        ' * This will set the upper right y coordinate.
        ' *
        ' * @param value The upper right y.
        ' */
        Public Sub setUpperRightY(ByVal value As Single)
            rectArray.set(3, New COSFloat(value))
        End Sub

        '/**
        ' * This will get the width of this rectangle as calculated by
        ' * upperRightX - lowerLeftX.
        ' *
        ' * @return The width of this rectangle.
        ' */
        Public Function getWidth() As Single
            Return getUpperRightX() - getLowerLeftX()
        End Function

        '/**
        ' * This will get the height of this rectangle as calculated by
        ' * upperRightY - lowerLeftY.
        ' *
        ' * @return The height of this rectangle.
        ' */
        Public Function getHeight() As Single
            Return getUpperRightY() - getLowerLeftY()
        End Function

        '/**
        ' * A convenience method to create a dimension object for AWT operations.
        ' *
        ' * @return A dimension that matches the width and height of this rectangle.
        ' */
        Public Function createDimension() As SizeF 'Dimension 
            Return New SizeF(getWidth(), getHeight())
        End Function

        '/**
        '* This will move the rectangle the given relative amount.
        '*
        '* @param horizontalAmount positive values will move rectangle to the right, negative's to the left.
        '* @param verticalAmount positive values will move the rectangle up, negative's down.
        '*/
        Public Sub move(ByVal horizontalAmount As Single, ByVal verticalAmount As Single)
            setUpperRightX(getUpperRightX() + horizontalAmount)
            setLowerLeftX(getLowerLeftX() + horizontalAmount)
            setUpperRightY(getUpperRightY() + verticalAmount)
            setLowerLeftY(getLowerLeftY() + verticalAmount)
        End Sub

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return rectArray
        End Function


        '/**
        ' * This will return a string representation of this rectangle.
        ' *
        ' * @return This object as a string.
        ' */
        Public Overrides Function toString() As String
            Return "[" & getLowerLeftX() & "," & getLowerLeftY() & "," & getUpperRightX() & "," + getUpperRightY() & "]"
        End Function

    End Class

End Namespace

'/**
' * From http://www.w3.org/TR/PNG-Filters.html: The Paeth filter computes a
' * simple linear function of the three neighboring pixels (left, above, upper
' * left), then chooses as predictor the neighboring pixel closest to the
' * computed value. This technique is due to Alan W. Paeth [PAETH].
' *
' * To compute the Paeth filter, apply the following formula to each byte of the
' * scanline:
' *
' * <code>Paeth(i,j) = Raw(i,j) - PaethPredictor(Raw(i-1,j), Raw(i,j-1), Raw(i-1,j-1))</code>
' *
' * To decode the Paeth filter
' *
' * <code>Raw(i,j) = Paeth(i,j) - PaethPredictor(Raw(i-1,j), Raw(i,j-1), Raw(i-1,j-1))</code>
' *
' * @author xylifyx@yahoo.co.uk
' * @version $Revision: 1.3 $
' */

Namespace org.apache.pdfbox.pdmodel.graphics.predictor

    Public Class Paeth
        Inherits PredictorAlgorithm

        '/**
        '    * The paeth predictor function.
        '    *
        '    * This function is taken almost directly from the PNG definition on
        '    * http://www.w3.org/TR/PNG-Filters.html
        '    *
        '    * @param a
        '    *            left
        '    * @param b
        '    *            above
        '    * @param c
        '    *            upper left
        '    * @return The result of the paeth predictor.
        '    */
        Public Function paethPredictor(ByVal a As Integer, ByVal b As Integer, ByVal c As Integer) As Integer
            Dim p As Integer = a + b - c ' initial estimate
            Dim pa As Integer = Math.Abs(p - a) ' distances to a, b, c
            Dim pb As Integer = Math.Abs(p - b)
            Dim pc As Integer = Math.Abs(p - c)
            ' return nearest of a,b,c,
            ' breaking ties in order a,b,c.
            If (pa <= pb AndAlso pa <= pc) Then
                Return a
            ElseIf (pb <= pc) Then
                Return b
            Else
                Return c
            End If
        End Function

        Public Overrides Sub encodeLine(ByVal src() As Byte, ByVal dest() As Byte, ByVal srcDy As Integer, ByVal srcOffset As Integer, ByVal destDy As Integer, ByVal destOffset As Integer)
            Dim bpl As Integer = getWidth() * getBpp()
            For x As Integer = 0 To bpl - 1
                dest(x + destOffset) = (src(x + srcOffset) - paethPredictor(leftPixel(src, srcOffset, srcDy, x), abovePixel(src, srcOffset, srcDy, x), aboveLeftPixel(src, srcOffset, srcDy, x)))
            Next
        End Sub

        Public Overrides Sub decodeLine(ByVal src() As Byte, ByVal dest() As Byte, ByVal srcDy As Integer, ByVal srcOffset As Integer, ByVal destDy As Integer, ByVal destOffset As Integer)
            Dim bpl As Integer = getWidth() * getBpp()
            For x As Integer = 0 To bpl - 1
                dest(x + destOffset) = (src(x + srcOffset) + paethPredictor(leftPixel(dest, destOffset, destDy, x), abovePixel(dest, destOffset, destDy, x), aboveLeftPixel(dest, destOffset, destDy, x)))
            Next
        End Sub

    End Class

End Namespace

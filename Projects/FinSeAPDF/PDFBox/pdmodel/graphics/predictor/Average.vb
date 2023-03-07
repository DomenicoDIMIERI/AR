Namespace org.apache.pdfbox.pdmodel.graphics.predictor

    '/**
    ' * We can use raw on the right hand side of
    ' * the decoding formula because it is already decoded.
    ' *
    ' * <code>average(i,j) = raw(i,j) + (raw(i-1,j)+raw(i,j-1)/2</code>
    ' *
    ' * decoding
    ' *
    ' * <code>raw(i,j) = avarage(i,j) - (raw(i-1,j)+raw(i,j-1)/2</code>
    ' *
    ' * @author xylifyx@yahoo.co.uk
    ' * @version $Revision: 1.3 $
    ' */
    Public Class Average
        Inherits PredictorAlgorithm
        '/**
        ' * Not an optimal version, but close to the def.
        ' *
        ' * {@inheritDoc}
        ' */
        Public Overrides Sub encodeLine(ByVal src() As Byte, ByVal dest() As Byte, ByVal srcDy As Integer, ByVal srcOffset As Integer, ByVal destDy As Integer, ByVal destOffset As Integer)
            Dim bpl As Integer = getWidth() * getBpp()
            For x As Integer = 0 To bpl - 1
                dest(x + destOffset) = (src(x + srcOffset) - ((leftPixel(src, srcOffset, srcDy, x) + abovePixel(src, srcOffset, srcDy, x)) >> 2))
            Next
        End Sub

        '/**
        ' * {@inheritDoc}
        ' */
        Public Overrides Sub decodeLine(ByVal src() As Byte, ByVal dest() As Byte, ByVal srcDy As Integer, ByVal srcOffset As Integer, ByVal destDy As Integer, ByVal destOffset As Integer)
            Dim bpl As Integer = getWidth() * getBpp()
            For x As Integer = 0 To bpl - 1
                dest(x + destOffset) = (src(x + srcOffset) + ((leftPixel(dest, destOffset, destDy, x) + abovePixel(dest, destOffset, destDy, x)) >> 2))
            Next


        End Sub


    End Class

End Namespace
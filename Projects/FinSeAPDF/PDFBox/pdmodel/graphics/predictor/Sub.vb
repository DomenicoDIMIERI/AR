Namespace org.apache.pdfbox.pdmodel.graphics.predictor

    '/**
    ' * The sub algorithm.
    ' *
    ' * <code>Sub(i,j) = Raw(i,j) - Raw(i-1,j)</code>
    ' *
    ' * <code>Raw(i,j) = Sub(i,j) + Raw(i-1,j)</code>
    ' *
    ' * @author xylifyx@yahoo.co.uk
    ' * @version $Revision: 1.3 $
    ' */
    Public Class [Sub]
        Inherits PredictorAlgorithm

        Public Overrides Sub encodeLine(ByVal src() As Byte, ByVal dest() As Byte, ByVal srcDy As Integer, ByVal srcOffset As Integer, ByVal destDy As Integer, ByVal destOffset As Integer)
            Dim bpl As Integer = getWidth() * getBpp()
            Dim bpp As Integer = getBpp()
            ' case: x < bpp
            For x = 0 To Math.Min(bpl, bpp) - 1
                dest(x + destOffset) = src(x + srcOffset)
            Next
            ' otherwise
            For x As Integer = getBpp() To bpl - 1
                dest(x + destOffset) = (src(x + srcOffset) - src(x + srcOffset - bpp))
            Next
        End Sub

        Public Overrides Sub decodeLine(ByVal src() As Byte, ByVal dest() As Byte, ByVal srcDy As Integer, ByVal srcOffset As Integer, ByVal destDy As Integer, ByVal destOffset As Integer)
            Dim bpl As Integer = getWidth() * getBpp()
            Dim bpp As Integer = getBpp()
            ' case: x < bpp
            For x As Integer = 0 To Math.Min(bpl, bpp) - 1
                dest(x + destOffset) = src(x + srcOffset)
            Next
            ' otherwise
            For x As Integer = getBpp() To bpl - 1
                dest(x + destOffset) = (src(x + srcOffset) + dest(x + destOffset - bpp))
            Next
        End Sub

    End Class

End Namespace

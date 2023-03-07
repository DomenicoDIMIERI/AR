'/**
' * The none algorithm.
' *
' * <code>None(i,j) = Raw(i,j)</code>
' *
' * <code>Raw(i,j) = None(i,j)</code>
' *
' * @author xylifyx@yahoo.co.uk
' * @version $Revision: 1.3 $
' */

Namespace org.apache.pdfbox.pdmodel.graphics.predictor

    Public Class None
        Inherits PredictorAlgorithm
        '/**
        ' * encode a byte array full of image data using the filter that this object
        ' * implements.
        ' *
        ' * @param src
        ' *            buffer
        ' * @param dest
        ' *            buffer
        ' */
        Public Overrides Sub encode(ByVal src() As Byte, ByVal dest() As Byte)
            checkBufsiz(dest, src)
            Array.Copy(src, 0, dest, 0, src.Length)
        End Sub

        '/**
        ' * decode a byte array full of image data using the filter that this object
        ' * implements.
        ' *
        ' * @param src
        ' *            buffer
        ' * @param dest
        ' *            buffer
        ' */
        Public Overrides Sub decode(ByVal src() As Byte, ByVal dest() As Byte)
            Array.Copy(src, 0, dest, 0, src.Length)
        End Sub

		 
        Public Overrides Sub encodeLine(ByVal src() As Byte, ByVal dest() As Byte, ByVal srcDy As Integer, ByVal srcOffset As Integer, ByVal destDy As Integer, ByVal destOffset As Integer)
            Dim bpl As Integer = getWidth() * getBpp()
            For x As Integer = 0 To bpl - 1
                dest(destOffset + x) = src(srcOffset + x)
            Next
        End Sub

        Public Overrides Sub decodeLine(ByVal src() As Byte, ByVal dest() As Byte, ByVal srcDy As Integer, ByVal srcOffset As Integer, ByVal destDy As Integer, ByVal destOffset As Integer)
            Dim bpl As Integer = getWidth() * getBpp()
            For x As Integer = 0 To bpl - 1
                dest(destOffset + x) = src(srcOffset + x)
            Next
        End Sub

    End Class

End Namespace

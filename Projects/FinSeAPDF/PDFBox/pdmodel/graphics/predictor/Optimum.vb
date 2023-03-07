'/**
' *
' *
' * In an Uptimum encoded image, each line takes up width*bpp+1 bytes. The first
' * byte holds a number that signifies which algorithm encoded the line.
' *
' * @author xylifyx@yahoo.co.uk
' * @version $Revision: 1.1 $
' */

Namespace org.apache.pdfbox.pdmodel.graphics.predictor

    Public Class Optimum
        Inherits PredictorAlgorithm

        Public Overrides Sub checkBufsiz(ByVal filtered() As Byte, ByVal raw() As Byte)
            If (filtered.Length <> (getWidth() * getBpp() + 1) * getHeight()) Then
                Throw New ArgumentException("filtered.length != (width*bpp + 1) * height, " & filtered.Length & " " & (getWidth() * getBpp() + 1) * getHeight() & "w,h,bpp=" & getWidth() + "," & getHeight() & "," & getBpp())
            End If
            If (raw.Length <> getWidth() * getHeight() * getBpp()) Then
                Throw New ArgumentException("raw.length != width * height * bpp, raw.length=" & raw.Length & " w,h,bpp=" & getWidth() & "," & getHeight() & "," & getBpp())
            End If
        End Sub

        Public Overrides Sub encodeLine(ByVal src() As Byte, ByVal dest() As Byte, ByVal srcDy As Integer, ByVal srcOffset As Integer, ByVal destDy As Integer, ByVal destOffset As Integer)
            Throw New NotSupportedException("encodeLine")
        End Sub

        Public Overrides Sub decodeLine(ByVal src() As Byte, ByVal dest() As Byte, ByVal srcDy As Integer, ByVal srcOffset As Integer, ByVal destDy As Integer, ByVal destOffset As Integer)
            Throw New NotSupportedException("decodeLine")
        End Sub

        Public Overrides Sub encode(ByVal src() As Byte, ByVal dest() As Byte)
            checkBufsiz(dest, src)
            Throw New NotSupportedException("encode")
        End Sub

        '/**
        ' * Filter indexed by byte code.
        ' */
        Public ReadOnly filter As PredictorAlgorithm() = {New None(), New [Sub](), New Up(), New Average(), New Paeth()}

        '/**
        ' * {@inheritDoc}
        ' */
        Public Overrides Sub setBpp(ByVal bpp As Integer)
            MyBase.setBpp(bpp)
            For i As Integer = 0 To filter.length - 1
                filter(i).setBpp(bpp)
            Next
        End Sub

        Public Overrides Sub setHeight(ByVal height As Integer)
            MyBase.setHeight(height)
            For i As Integer = 0 To filter.length - 1
                filter(i).setHeight(height)
            Next
        End Sub

        Public Overrides Sub setWidth(ByVal width As Integer)
            MyBase.setWidth(width)
            For i As Integer = 0 To filter.length - 1
                filter(i).setWidth(width)
            Next
        End Sub

        Public Overrides Sub decode(ByVal src() As Byte, ByVal dest() As Byte)
            checkBufsiz(src, dest)
            Dim bpl As Integer = getWidth() * getBpp()
            Dim srcDy As Integer = bpl + 1
            For y As Integer = 0 To getHeight() - 1
                Dim f As PredictorAlgorithm = filter(src(y * srcDy))
                Dim srcOffset As Integer = y * srcDy + 1
                f.decodeLine(src, dest, srcDy, srcOffset, bpl, y * bpl)
            Next
        End Sub

    End Class

End Namespace
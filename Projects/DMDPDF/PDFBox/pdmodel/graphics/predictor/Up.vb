Namespace org.apache.pdfbox.pdmodel.graphics.predictor

    '/**
    ' * The up algorithm.
    ' *
    ' * <code>Up(i,j) = Raw(i,j) - Raw(i,j-1)</code>
    ' *
    ' * <code>Raw(i,j) = Up(i,j) + Raw(i,j-1)</code>
    ' *
    ' * @author xylifyx@yahoo.co.uk
    ' * @version $Revision: 1.3 $
    ' */
    Public Class Up
        Inherits PredictorAlgorithm

        Public Overrides Sub encodeLine(ByVal src() As Byte, ByVal dest() As Byte, ByVal srcDy As Integer, ByVal srcOffset As Integer, ByVal destDy As Integer, ByVal destOffset As Integer)
            Dim bpl As Integer = getWidth() * getBpp()
            ' case: y = 0;
            If (srcOffset - srcDy < 0) Then
                If (0 < getHeight()) Then
                    For x As Integer = 0 To bpl - 1
                        dest(destOffset + x) = src(srcOffset + x)
                    Next
                End If
            Else
                For x As Integer = 0 To bpl - 1
                    dest(destOffset + x) = (src(srcOffset + x) - src(srcOffset + x - srcDy))
                Next
            End If
        End Sub

        Public Overrides Sub decodeLine(ByVal src() As Byte, ByVal dest() As Byte, ByVal srcDy As Integer, ByVal srcOffset As Integer, ByVal destDy As Integer, ByVal destOffset As Integer)
            ' case: y = 0;
            Dim bpl As Integer = getWidth() * getBpp()
            If (destOffset - destDy < 0) Then
                If (0 < getHeight()) Then
                    For x As Integer = 0 To bpl - 1
                        dest(destOffset + x) = src(srcOffset + x)
                    Next
                End If
            Else
                For x As Integer = 0 To bpl - 1
                    dest(destOffset + x) = (src(srcOffset + x) + dest(destOffset + x - destDy))
                Next
            End If
        End Sub

    End Class

End Namespace

Namespace PDF

    Partial Public Class PDFWriter

        Public Sub Circle(ByVal xx As Single, ByVal xy As Single, ByVal xr As Single, Optional ByVal xstyle As String = "D")
            Me.Ellipse(xx, xy, xr, xr, xstyle)
        End Sub

        Public Sub Ellipse(ByVal xx As Single, ByVal xy As Single, ByVal xrx As Single, ByVal xry As Single, Optional ByVal xstyle As String = "D")
            Dim xop As String
            Select Case (xstyle)
                Case "F" : xop = "f"
                Case "FD", "DF" : xop = "B"
                Case Else : xop = "S"
            End Select
            Dim xlx As Single = 4 / 3 * (Math.SQRT2 - 1) * xrx
            Dim xly As Single = 4 / 3 * (Math.SQRT2 - 1) * xry
            Dim xk As Single = (Me.k)
            Dim xh As Single = (Me.h)
            Me._out([lib].sprintf("%.2f %.2f m %.2f %.2f %.2f %.2f %.2f %.2f c", (xx + xrx) * xk, (xh - xy) * xk, (xx + xrx) * xk, (xh - (xy - xly)) * xk, (xx + xlx) * xk, (xh - (xy - xry)) * xk, xx * xk, (xh - (xy - xry)) * xk))
            Me._out([lib].sprintf("%.2f %.2f %.2f %.2f %.2f %.2f c", (xx - xlx) * xk, (xh - (xy - xry)) * xk, (xx - xrx) * xk, (xh - (xy - xly)) * xk, (xx - xrx) * xk, (xh - xy) * xk))
            Me._out([lib].sprintf("%.2f %.2f %.2f %.2f %.2f %.2f c", (xx - xrx) * xk, (xh - (xy + xly)) * xk, (xx - xlx) * xk, (xh - (xy + xry)) * xk, xx * xk, (xh - (xy + xry)) * xk))
            Me._out([lib].sprintf("%.2f %.2f %.2f %.2f %.2f %.2f c %s", (xx + xlx) * xk, (xh - (xy + xry)) * xk, (xx + xrx) * xk, (xh - (xy + xly)) * xk, (xx + xrx) * xk, (xh - xy) * xk, xop))
        End Sub

        Public Sub FillEllipse(ByVal color As System.Drawing.Color, ByVal rect As System.Drawing.RectangleF)
            Me.FillColor = color
            Me.Ellipse(rect.Left, rect.Top, rect.Width / 2, rect.Height / 2, "F")
        End Sub

        Public Sub DrawEllipse(ByVal color As System.Drawing.Color, ByVal rect As System.Drawing.RectangleF)
            Me.DrawColor = color
            Me.Ellipse(rect.Left, rect.Top, rect.Width / 2, rect.Height / 2, "D")
        End Sub


    End Class

End Namespace
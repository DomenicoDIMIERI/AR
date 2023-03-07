Namespace PDF

    Partial Public Class PDFWriter

        Private widths() As Single

        Public Sub SetWidths(ByVal xw As Single())
            Me.widths = xw
        End Sub

        Public Sub Row(ByVal xdata() As String)
            Dim xi As Integer
            Dim xh As Single
            Dim xnb = 0
            For xi = 0 To UBound(xdata)
                xnb = Math.Max(xnb, Me.NbLines(Me.widths(xi), xdata(xi)))
            Next
            xh = (xnb) * 5
            Me.CheckPageBreak(xh)
            For xi = 0 To UBound(xdata)
                Dim xw As Single = Me.widths(xi)
                Dim xx As Single = Me.GetX()
                Dim xy As Single = Me.GetY()
                Me.SetLineStyle(3.0)
                Me.Rect(xx, xy, xw, xh)
                Me.MultiCell(xw, 5, xdata(xi), 0, "T")
                Me.SetXY(xx + xw, xy)
            Next
            Me.Ln(xh)
        End Sub

        Public Sub CheckPageBreak(ByVal xh As Single)
            If (Me.GetY() + xh > Me.PageBreakTrigger) Then Me.AddPage(Me.CurOrientation)
        End Sub

        Public Function NbLines(ByVal xw As Single, ByVal xtxt As String) As Integer
            Dim xnb As Integer
            'Dim xcw = Me.CurrentFont.cw
            If (xw = 0) Then xw = Me.w - (Me.rMargin) - Me.x
            Dim xwmax As Single = ((xw) - 2 * (Me.cMargin)) * 1000 / (Me.FontSize)
            Dim xs As String = Replace(xtxt, vbCr, "")
            xnb = xs.length
            If (xnb > 0 And xs.Chars(xnb - 1) = vbLf) Then xnb -= 1
            Dim xsep As Single = -1
            Dim xi As Integer = 0
            Dim xj As Integer = 0
            Dim xl As Single = 0
            Dim xnl As Integer = 1
            While (xi < xnb)
                Dim xc As String = xs.Chars(xi)
                If (xc = vbLf) Then
                    xi += 1
                    xsep = -1
                    xj = xi
                    xl = 0
                    xnl += 1
                    Continue While
                End If
                If (xc = " ") Then xsep = xi
                xl += Me.CurrentFont.xcw(xc)
                If (xl > xwmax) Then
                    If (xsep = -1) Then
                        If (xi = xj) Then
                            xi += 1
                        Else
                            xi = xsep + 1
                        End If
                        xsep = -1
                        xj = xi
                        xl = 0
                        xnl += 1
                    Else
                        xi += 1
                    End If
                End If
            End While
            Return xnl
        End Function

    End Class

End Namespace
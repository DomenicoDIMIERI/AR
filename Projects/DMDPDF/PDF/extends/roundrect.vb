Namespace PDF

    Partial Public Class PDFWriter

        Public Sub RoundedRect(ByVal xx As Single, ByVal xy As Single, ByVal xw As Single, ByVal xh As Single, ByVal xr As Single, Optional ByVal xstyle As String = vbNullString, Optional ByVal xangle As String = "1234")
            Dim xk As Single = Me.k
            Dim xhp As Single = Me.h
            Dim xop As String
            If (xstyle = "F") Then
                xop = "f"
            ElseIf (xstyle = "FD" Or xstyle = "DF") Then
                xop = "B"
            Else
                xop = "S"
            End If
            Dim xMyArc As Single = 4 / 3 * (Math.SQRT2 - 1)
            Me._out([lib].sprintf("%.2f %.2f m", (xx + xr) * xk, (xhp - xy) * xk))
            Dim xxc As Single = xx + xw - xr
            Dim xyc As Single = xy + xr
            Me._out([lib].sprintf("%.2f %.2f l", xxc * xk, (xhp - xy) * xk))
            If (InStr(xangle, "2") < 0) Then
                Me._out([lib].sprintf("%.2f %.2f l", (xx + xw) * xk, (xhp - xy) * xk))
            Else
                Me._Arc(xxc + xr * xMyArc, xyc - xr, xxc + xr, xyc - xr * xMyArc, xxc + xr, xyc)
            End If
            xxc = xx + xw - xr
            xyc = xy + xh - xr
            Me._out([lib].sprintf("%.2f %.2f l", (xx + xw) * xk, (xhp - xyc) * xk))
            If (InStr(xangle, "3") < 0) Then
                Me._out([lib].sprintf("%.2f %.2f l", (xx + xw) * xk, (xhp - (xy + xh)) * xk))
            Else
                Me._Arc(xxc + xr, xyc + xr * xMyArc, xxc + xr * xMyArc, xyc + xr, xxc, xyc + xr)
            End If
            xxc = xx + xr
            xyc = xy + xh - xr
            Me._out([lib].sprintf("%.2f %.2f l", xxc * xk, (xhp - (xy + xh)) * xk))
            If (InStr(xangle, "4") < 0) Then
                Me._out([lib].sprintf("%.2f %.2f l", (xx) * xk, (xhp - (xy + xh)) * xk))
            Else
                Me._Arc(xxc - xr * xMyArc, xyc + xr, xxc - xr, xyc + xr * xMyArc, xxc - xr, xyc)
            End If

            xxc = xx + xr
            xyc = xy + xr
            Me._out([lib].sprintf("%.2f %.2f l", (xx) * xk, (xhp - xyc) * xk))
            If (InStr(xangle, "1") < 0) Then
                Me._out([lib].sprintf("%.2f %.2f l", (xx) * xk, (xhp - xy) * xk))
                Me._out([lib].sprintf("%.2f %.2f l", (xx + xw) * xk, (xhp - xy) * xk))
            Else
                Me._Arc(xxc - xr, xyc - xr * xMyArc, xxc - xr * xMyArc, xyc - xr, xxc, xyc - xr)
            End If
            Me._out(xop)
        End Sub

        Public Sub _Arc(ByVal xx1 As Single, ByVal xy1 As Single, ByVal xx2 As Single, ByVal xy2 As Single, ByVal xx3 As Single, ByVal xy3 As Single)
            Dim xh As Single = Me.h
            '(xx1)
            Me._out([lib].sprintf("%.2f %.2f %.2f %.2f %.2f %.2f c ", xx1 * Me.k, (xh - xy1) * Me.k, xx2 * Me.k, (xh - xy2) * Me.k, xx3 * Me.k, (xh - xy3) * Me.k))
        End Sub

        'Me._Arc=function _Arc(xx1 , xy1 , xx2 , xy2 , xx3 , xy3)
        '	{
        '	xh=Me.h;
        '	 Me._out([lib].sprintf("%.2f %.2f %.2f %.2f %.2f %.2f c",xx1*Me.k,(xh-xy1)*Me.k,xx2*Me.k,(xh-xy2)*Me.k,xx3*Me.k,(xh-xy3)*Me.k));
        '	}

    End Class

End Namespace
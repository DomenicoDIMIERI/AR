Namespace PDF

    Partial Public Class PDFWriter

        Public Sub Sector(ByVal xxc As Single, ByVal xyc As Single, ByVal xr As Single, ByVal xa As Single, ByVal xb As Single, Optional ByVal xstyle As String = "FD", Optional ByVal xcw As Boolean = True, Optional ByVal xo As Single = 90)
            Dim xd As Single
            If (xcw) Then
                xd = xb
                xb = xo - xa
                xa = xo - xd
            Else
                xb += xo
                xa += xo
            End If
            xa = (xa Mod 360) + 360
            xb = (xb Mod 360) + 360
            If (xa > xb) Then xb += 360
            xb = xb / 360 * 2 * Math.PI
            xa = xa / 360 * 2 * Math.PI
            xd = xb - xa
            If (xd = 0) Then xd = 2 * Math.PI
            Dim xk As Single = Me.k
            Dim xhp As Single = Me.h
            Dim xop As String
            If (xstyle = "F") Then
                xop = "f"
            ElseIf (xstyle = "FD" Or xstyle = "DF") Then
                xop = "b"
            Else
                xop = "s"
            End If
            Dim xMyArc As Single
            If (Math.Sin(xd / 2)) Then xMyArc = 4 / 3 * (1 - Math.Cos(xd / 2)) / Math.Sin(xd / 2) * xr
            Me._out([lib].sprintf("%.2f %.2f m", (xxc) * xk, (xhp - xyc) * xk))
            Me._out([lib].sprintf("%.2f %.2f l", (xxc + xr * Math.Cos(xa)) * xk, ((xhp - (xyc - xr * Math.Sin(xa))) * xk)))
            If (xd < Math.PI / 2) Then
                Me._Arc(xxc + xr * Math.Cos(xa) + xMyArc * Math.Cos(Math.PI / 2 + xa), xyc - xr * Math.Sin(xa) - xMyArc * Math.Sin(Math.PI / 2 + xa), xxc + xr * Math.Cos(xb) + xMyArc * Math.Cos(xb - Math.PI / 2), xyc - xr * Math.Sin(xb) - xMyArc * Math.Sin(xb - Math.PI / 2), xxc + xr * Math.Cos(xb), xyc - xr * Math.Sin(xb))
            Else
                xb = xa + xd / 4
                xMyArc = 4 / 3 * (1 - Math.Cos(xd / 8)) / Math.Sin(xd / 8) * xr
                Me._Arc(xxc + xr * Math.Cos(xa) + xMyArc * Math.Cos(Math.PI / 2 + xa), xyc - xr * Math.Sin(xa) - xMyArc * Math.Sin(Math.PI / 2 + xa), xxc + xr * Math.Cos(xb) + xMyArc * Math.Cos(xb - Math.PI / 2), xyc - xr * Math.Sin(xb) - xMyArc * Math.Sin(xb - Math.PI / 2), xxc + xr * Math.Cos(xb), xyc - xr * Math.Sin(xb))
                xa = xb
                xb = xa + xd / 4
                Me._Arc(xxc + xr * Math.Cos(xa) + xMyArc * Math.Cos(Math.PI / 2 + xa), xyc - xr * Math.Sin(xa) - xMyArc * Math.Sin(Math.PI / 2 + xa), xxc + xr * Math.Cos(xb) + xMyArc * Math.Cos(xb - Math.PI / 2), xyc - xr * Math.Sin(xb) - xMyArc * Math.Sin(xb - Math.PI / 2), xxc + xr * Math.Cos(xb), xyc - xr * Math.Sin(xb))
                xa = xb
                xb = xa + xd / 4
                Me._Arc(xxc + xr * Math.Cos(xa) + xMyArc * Math.Cos(Math.PI / 2 + xa), xyc - xr * Math.Sin(xa) - xMyArc * Math.Sin(Math.PI / 2 + xa), xxc + xr * Math.Cos(xb) + xMyArc * Math.Cos(xb - Math.PI / 2), xyc - xr * Math.Sin(xb) - xMyArc * Math.Sin(xb - Math.PI / 2), xxc + xr * Math.Cos(xb), xyc - xr * Math.Sin(xb))
                xa = xb
                xb = xa + xd / 4
                Me._Arc(xxc + xr * Math.Cos(xa) + xMyArc * Math.Cos(Math.PI / 2 + xa), xyc - xr * Math.Sin(xa) - xMyArc * Math.Sin(Math.PI / 2 + xa), xxc + xr * Math.Cos(xb) + xMyArc * Math.Cos(xb - Math.PI / 2), xyc - xr * Math.Sin(xb) - xMyArc * Math.Sin(xb - Math.PI / 2), xxc + xr * Math.Cos(xb), xyc - xr * Math.Sin(xb))
            End If
            Me._out(xop)
        End Sub

    End Class

End Namespace
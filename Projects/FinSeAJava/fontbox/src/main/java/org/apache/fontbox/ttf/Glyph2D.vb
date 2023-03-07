Imports FinSeA.Sistema
Imports FinSeA.Drawings
Imports System.Drawing

Namespace org.apache.fontbox.ttf

    '/**
    ' * This class provides a glyph to GeneralPath conversion.
    ' * 
    ' * This class is based on code from Apache Batik a subproject of Apache XMLGraphics.
    ' * see http://xmlgraphics.apache.org/batik/ for further details.
    ' */
    Public Class Glyph2D

        Private leftSideBearing As Short = 0
        Private advanceWidth As Integer = 0
        Private points() As Point
        Private glyphPath As GeneralPath

        '/**
        ' * Constructor.
        ' * 
        ' * @param gd the glyph description
        ' * @param lsb leftSideBearing
        ' * @param advance advanceWidth
        ' */
        Public Sub New(ByVal gd As GlyphDescription, ByVal lsb As Short, ByVal advance As Integer)
            leftSideBearing = lsb
            advanceWidth = advance
            describe(gd)
        End Sub

        '/**
        ' * Returns the advanceWidth value.
        ' * 
        ' * @return the advanceWidth
        ' */
        Public Function getAdvanceWidth() As Integer
            Return advanceWidth
        End Function

        '/**
        ' * Returns the leftSideBearing value.
        ' * 
        ' * @return the leftSideBearing
        ' */
        Public Function getLeftSideBearing() As Short
            Return leftSideBearing
        End Function

        '/**
        ' * Set the points of a glyph from the GlyphDescription.
        ' */
        Private Sub describe(ByVal gd As GlyphDescription)
            Dim endPtIndex As Integer = 0
            points = Arrays.CreateInstance(Of Point)(gd.getPointCount())
            For i As Integer = 0 To gd.getPointCount() - 1
                Dim endPt As Boolean = gd.getEndPtOfContours(endPtIndex) = i
                If (endPt) Then
                    endPtIndex += 1
                End If
                points(i) = New Point(gd.getXCoordinate(i), gd.getYCoordinate(i), (gd.getFlags(i) And GlyfDescript.ON_CURVE) = GlyfDescript.ON_CURVE, endPt)
            Next
        End Sub

        '/**
        ' * Returns the path describing the glyph.
        ' * 
        ' * @return the GeneralPath of the glyph
        ' */
        Public Function getPath() As GeneralPath
            If (glyphPath Is Nothing) Then
                glyphPath = calculatePath()
            End If
            Return glyphPath
        End Function

        Private Function calculatePath() As GeneralPath
            Dim path As New GeneralPath()
            Dim numberOfPoints As Integer = points.Length
            Dim i As Integer = 0
            Dim endOfContour As Boolean = True
            Dim startingPoint As Point = Nothing
            Dim lastCtrlPoint As Point = Nothing
            While (i < numberOfPoints)
                Dim point As Point = points(i Mod numberOfPoints)
                Dim nextPoint1 As Point = points((i + 1) Mod numberOfPoints)
                Dim nextPoint2 As Point = points((i + 2) Mod numberOfPoints)
                ' new contour
                If (endOfContour) Then
                    ' skip endOfContour points
                    If (Point.endOfContour) Then
                        i += 1
                        Continue While
                    End If
                    ' move to the starting point
                    path.moveTo(point.X, point.Y)
                    endOfContour = False
                    startingPoint = point
                End If
                ' lineTo
                If (point.onCurve AndAlso nextPoint1.onCurve) Then
                    path.lineTo(nextPoint1.X, nextPoint1.Y)
                    i += 1
                    If (point.endOfContour AndAlso nextPoint1.endOfContour) Then
                        endOfContour = True
                        path.closePath()
                    End If
                    Continue While
                End If
                ' quadratic bezier
                If (point.onCurve AndAlso Not nextPoint1.onCurve AndAlso nextPoint2.onCurve) Then
                    If (nextPoint1.endOfContour) Then
                        ' use the starting point as end point
                        path.quadTo(nextPoint1.X, nextPoint1.Y, startingPoint.X, startingPoint.Y)
                    Else
                        path.quadTo(nextPoint1.X, nextPoint1.Y, nextPoint2.X, nextPoint2.Y)
                    End If
                    If (nextPoint1.endOfContour OrElse nextPoint2.endOfContour) Then
                        endOfContour = True
                        path.closePath()
                    End If
                    i += 2
                    lastCtrlPoint = nextPoint1
                    Continue While
                End If
                If (point.onCurve AndAlso Not nextPoint1.onCurve AndAlso Not nextPoint2.onCurve) Then
                    ' interpolate endPoint
                    Dim endPointX As Integer = midValue(nextPoint1.X, nextPoint2.X)
                    Dim endPointY As Integer = midValue(nextPoint1.Y, nextPoint2.Y)
                    path.quadTo(nextPoint1.X, nextPoint1.Y, endPointX, endPointY)
                    If (point.endOfContour OrElse nextPoint1.endOfContour OrElse nextPoint2.endOfContour) Then
                        path.quadTo(nextPoint2.X, nextPoint2.Y, startingPoint.X, startingPoint.Y)
                        endOfContour = True
                        path.closePath()
                    End If
                    i += 2
                    lastCtrlPoint = nextPoint1
                    Continue While
                End If
                If (Not point.onCurve AndAlso Not nextPoint1.onCurve) Then
                    Dim lastEndPoint As PointF = path.getCurrentPoint()
                    ' calculate new control point using the previous control point
                    lastCtrlPoint = New Point(midValue(lastCtrlPoint.x, CInt(lastEndPoint.X)), midValue(lastCtrlPoint.y, CInt(lastEndPoint.Y)))
                    ' interpolate endPoint
                    Dim endPointX As Integer = midValue(CInt(lastEndPoint.X), nextPoint1.X)
                    Dim endPointY As Integer = midValue(CInt(lastEndPoint.Y), nextPoint1.Y)
                    path.quadTo(lastCtrlPoint.X, lastCtrlPoint.Y, endPointX, endPointY)
                    If (point.endOfContour OrElse nextPoint1.endOfContour) Then
                        endOfContour = True
                        path.closePath()
                    End If
                    i += 1
                    Continue While
                End If
                If (Not point.onCurve AndAlso nextPoint1.onCurve) Then
                    path.quadTo(point.X, point.Y, nextPoint1.X, nextPoint1.Y)
                    If (point.endOfContour OrElse nextPoint1.endOfContour) Then
                        endOfContour = True
                        path.closePath()
                    End If
                    i += 1
                    lastCtrlPoint = point
                    Continue While
                End If
                LOG.error("Unknown glyph command!!")
                Exit While
            End While
            Return path
        End Function

        Private Function midValue(ByVal a As Integer, ByVal b As Integer) As Integer
            Return a + (b - a) / 2
        End Function

        '/**
        ' * This class represents one point of a glyph.  
        ' *
        ' */
        Private Class Point

            Public x As Integer = 0
            Public y As Integer = 0
            Public onCurve As Boolean = True
            Public endOfContour As Boolean = False

            Public Sub New(ByVal xValue As Integer, ByVal yValue As Integer, ByVal onCurveValue As Boolean, ByVal endOfContourValue As Boolean)
                x = xValue
                y = yValue
                onCurve = onCurveValue
                endOfContour = endOfContourValue
            End Sub

            Public Sub New(ByVal xValue As Integer, ByVal yValue As Integer)
                Me.New(xValue, yValue, False, False)
            End Sub

        End Class

    End Class

End Namespace
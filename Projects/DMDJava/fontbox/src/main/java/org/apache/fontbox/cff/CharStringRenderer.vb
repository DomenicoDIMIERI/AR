Imports FinSeA.Drawings
Imports System.Drawing

Namespace org.apache.fontbox.cff

    '/**
    ' * This class represents a renderer for a charstring.
    ' * @author Villu Ruusmann
    ' * @version $Revision: 1.0 $
    ' */
    Public Class CharStringRenderer
        Inherits CharStringHandler

        ' TODO CharStringRenderer as abstract Class with two inherited classes according to the Charsstring type....
	
        Private isCharstringType1 As Boolean = True
        Private isFirstCommand As Boolean = True

        Private path As GeneralPath = Nothing
        Private sidebearingPoint As PointF = Nothing
        Private referencePoint As PointF = Nothing
        Private width As Integer = 0
        Private hasNonEndCharOp As Boolean = False

        '/**
        ' * Constructor for the char string renderer.
        ' */
        Public Sub New()
            isCharstringType1 = True
        End Sub

        '/**
        ' * Constructor for the char string renderer with a parameter
        ' * to determine whether the rendered CharString is type 1.
        ' * @param isType1 Determines wheher the charstring is type 1
        ' */
        Public Sub New(ByVal isType1 As Boolean)
            isCharstringType1 = isType1
        End Sub

        '/**
        ' * Renders the given sequence and returns the result as a GeneralPath.
        ' * @param sequence the given charstring sequence
        ' * @return the rendered GeneralPath
        ' */
        Public Function render(ByVal sequence As List(Of Object)) As GeneralPath
            path = New GeneralPath()
            sidebearingPoint = New PointF(0, 0)
            referencePoint = Nothing
            setWidth(0)
            handleSequence(sequence)
            Return path
        End Function

        Public Overrides Function handleCommand(ByVal numbers As List(Of NInteger), ByVal command As CharStringCommand) As List(Of NInteger)
            If (isCharstringType1) Then
                handleCommandType1(numbers, command)
            Else
                handleCommandType2(numbers, command)
            End If
            Return Nothing
        End Function

        '/**
        ' *
        ' * @param numbers
        ' * @param command
        ' */
        Private Sub handleCommandType2(ByVal numbers As List(Of NInteger), ByVal command As CharStringCommand)
            Dim name As String = CharStringCommand.TYPE2_VOCABULARY.get(command.getKey())

            If (Not hasNonEndCharOp) Then
                hasNonEndCharOp = Not "endchar".Equals(name)
            End If
            If ("vmoveto".Equals(name)) Then '//
                If (path.getCurrentPoint().IsEmpty = False) Then
                    closePath()
                End If
                If (isFirstCommand AndAlso numbers.size() = 2) Then
                    setWidth(numbers.get(0))
                    rmoveTo(NInteger.valueOf(0), numbers.get(1))
                Else
                    rmoveTo(NInteger.valueOf(0), numbers.get(0))
                End If
            ElseIf ("rlineto".Equals(name)) Then
                If (isFirstCommand AndAlso numbers.size() = 3) Then
                    setWidth(numbers.get(0))
                End If
                rrlineTo(numbers)
            ElseIf ("hlineto".Equals(name)) Then
                If (isFirstCommand AndAlso numbers.size() = 2) Then
                    setWidth(numbers.get(0))
                End If
                hlineTo(numbers)
            ElseIf ("vlineto".Equals(name)) Then
                If (isFirstCommand AndAlso numbers.size() = 2) Then
                    setWidth(numbers.get(0))
                End If
                vlineTo(numbers)
            ElseIf ("rrcurveto".Equals(name)) Then
                If (isFirstCommand AndAlso numbers.size() = 7) Then
                    setWidth(numbers.get(0))
                End If
                rrCurveTo(numbers)
            ElseIf ("rlinecurve".Equals(name)) Then
                rlineCurve(numbers)
            ElseIf ("rcurveline".Equals(name)) Then
                rcurveLine(numbers)
            ElseIf ("closepath".Equals(name)) Then
                closePath()
            ElseIf ("rmoveto".Equals(name)) Then
                If (path.getCurrentPoint().IsEmpty = False) Then
                    closePath()
                End If
                If (isFirstCommand AndAlso numbers.size() = 3) Then
                    setWidth(numbers.get(0))
                    rmoveTo(numbers.get(1), numbers.get(2))
                Else
                    rmoveTo(numbers.get(0), numbers.get(1))
                End If
            ElseIf ("hmoveto".Equals(name)) Then
                If (path.getCurrentPoint().IsEmpty = False) Then
                    closePath()
                End If
                If (isFirstCommand AndAlso numbers.size() = 2) Then
                    setWidth(numbers.get(0))
                    rmoveTo(numbers.get(1), New NInteger(0))
                Else
                    rmoveTo(numbers.get(0), New NInteger(0))
                End If
            ElseIf ("vhcurveto".Equals(name)) Then
                If (isFirstCommand AndAlso numbers.size() = 5) Then
                    setWidth(numbers.get(0))
                End If
                rvhCurveTo(numbers)
            ElseIf ("hvcurveto".Equals(name)) Then
                If (isFirstCommand AndAlso numbers.size() = 5) Then
                    setWidth(numbers.get(0))
                End If
                rhvCurveTo(numbers)
            ElseIf ("hhcurveto".Equals(name)) Then
                rhhCurveTo(numbers)
            ElseIf ("vvcurveto".Equals(name)) Then
                rvvCurveTo(numbers)
            ElseIf ("hstem".Equals(name)) Then
                If (numbers.size() Mod 2 = 1) Then
                    setWidth(numbers.get(0))
                End If
            ElseIf ("vstem".Equals(name)) Then
                If (numbers.size() Mod 2 = 1) Then
                    setWidth(numbers.get(0))
                End If
            ElseIf ("hstemhm".Equals(name)) Then
                If (numbers.size() Mod 2 = 1) Then
                    setWidth(numbers.get(0))
                End If
            ElseIf ("vstemhm".Equals(name)) Then
                If (numbers.size() Mod 2 = 1) Then
                    setWidth(numbers.get(0))
                End If
            ElseIf ("cntrmask".Equals(name)) Then
                If (numbers.size() = 1) Then
                    setWidth(numbers.get(0))
                End If
            ElseIf ("hintmask".Equals(name)) Then
                If (numbers.size() = 1) Then
                    setWidth(numbers.get(0))
                End If
            ElseIf ("endchar".Equals(name)) Then
                If (hasNonEndCharOp) Then
                    closePath()
                End If
                If (numbers.size() Mod 2 = 1) Then
                    setWidth(numbers.get(0))
                    If (numbers.size() > 1) Then
                        LOG.debug("endChar: too many numbers left, using the first one, see PDFBOX-1501 for details")
                    End If
                End If
            End If
            If (isFirstCommand) Then
                isFirstCommand = False
            End If
        End Sub

        '/**
        ' *
        ' * @param numbers
        ' * @param command
        ' */
        Private Sub handleCommandType1(ByVal numbers As List(Of NInteger), ByVal command As CharStringCommand)
            Dim name As String = CharStringCommand.TYPE1_VOCABULARY.get(command.getKey())

            If ("vmoveto".Equals(name)) Then
                rmoveTo(NInteger.valueOf(0), numbers.get(0))
            ElseIf ("rlineto".Equals(name)) Then
                rlineTo(numbers.get(0), numbers.get(1))
            ElseIf ("hlineto".Equals(name)) Then
                rlineTo(numbers.get(0), NInteger.valueOf(0))
            ElseIf ("vlineto".Equals(name)) Then
                rlineTo(NInteger.valueOf(0), numbers.get(0))
            ElseIf ("rrcurveto".Equals(name)) Then
                rrcurveTo(numbers.get(0), numbers.get(1), numbers.get(2), numbers.get(3), numbers.get(4), numbers.get(5))
            ElseIf ("closepath".Equals(name)) Then
                closePath()
            ElseIf ("sbw".Equals(name)) Then
                pointSb(numbers.get(0), numbers.get(1))
                setWidth(numbers.get(2).Value)
            ElseIf ("hsbw".Equals(name)) Then
                pointSb(numbers.get(0), NInteger.valueOf(0))
                setWidth(numbers.get(1).Value)
            ElseIf ("rmoveto".Equals(name)) Then
                rmoveTo(numbers.get(0), numbers.get(1))
            ElseIf ("hmoveto".Equals(name)) Then
                rmoveTo(numbers.get(0), NInteger.valueOf(0))
            ElseIf ("vhcurveto".Equals(name)) Then
                rrcurveTo(NInteger.valueOf(0), numbers.get(0), numbers.get(1), numbers.get(2), numbers.get(3), NInteger.valueOf(0))
            ElseIf ("hvcurveto".Equals(name)) Then
                rrcurveTo(numbers.get(0), NInteger.valueOf(0), numbers.get(1), numbers.get(2), NInteger.valueOf(0), numbers.get(3))
            End If
        End Sub

        Private Sub rmoveTo(ByVal dx As Number, ByVal dy As Number)
            Dim point As PointF = referencePoint
            If (point.IsEmpty = True) Then
                point = path.getCurrentPoint()
                If (point.IsEmpty) Then
                    point = sidebearingPoint
                End If
            End If
            referencePoint = Nothing
            path.moveTo((point.X + dx.doubleValue()), (point.Y + dy.doubleValue()))
        End Sub

        Private Sub hlineTo(ByVal numbers As List(Of NInteger))
            For i As Integer = 0 To numbers.size() - 1
                If (i Mod 2 = 0) Then
                    rlineTo(numbers.get(i), NInteger.valueOf(0))
                Else
                    rlineTo(NInteger.valueOf(0), numbers.get(i))
                End If
            Next
        End Sub

        Private Sub vlineTo(ByVal numbers As List(Of NInteger))
            For i As Integer = 0 To numbers.size() - 1
                If (i Mod 2 = 0) Then
                    rlineTo(NInteger.valueOf(0), numbers.get(i))
                Else
                    rlineTo(numbers.get(i), NInteger.valueOf(0))
                End If
            Next
        End Sub

        Private Sub rlineTo(ByVal dx As Number, ByVal dy As Number)
            Dim point As PointF = path.getCurrentPoint()
            path.lineTo((point.X + dx.doubleValue()), (point.y + dy.doubleValue()))
        End Sub

        Private Sub rrlineTo(ByVal numbers As List(Of NInteger))
            For i As Integer = 0 To numbers.size() - 1 Step 2
                rlineTo(numbers.get(i), numbers.get(i + 1))
            Next
        End Sub

        Private Sub rrCurveTo(ByVal numbers As List(Of NInteger))
            If (numbers.size() >= 6) Then
                For i As Integer = 0 To numbers.size() - 1 Step 6
                    Dim x1 As NInteger = numbers.get(i)
                    Dim y1 As NInteger = numbers.get(i + 1)
                    Dim x2 As NInteger = numbers.get(i + 2)
                    Dim y2 As NInteger = numbers.get(i + 3)
                    Dim x3 As NInteger = numbers.get(i + 4)
                    Dim y3 As NInteger = numbers.get(i + 5)
                    rrCurveTo(x1, y1, x2, y2, x3, y3)
                Next
            End If
        End Sub

        Private Sub rrcurveTo(ByVal dx1 As Number, ByVal dy1 As Number, ByVal dx2 As Number, ByVal dy2 As Number, ByVal dx3 As Number, ByVal dy3 As Number)
            Dim point As PointF = path.getCurrentPoint()
            Dim x1 As Single = point.X + dx1.floatValue()
            Dim y1 As Single = point.Y + dy1.floatValue()
            Dim x2 As Single = x1 + dx2.floatValue()
            Dim y2 As Single = y1 + dy2.floatValue()
            Dim x3 As Single = x2 + dx3.floatValue()
            Dim y3 As Single = y2 + dy3.floatValue()
            path.curveTo(x1, y1, x2, y2, x3, y3)
        End Sub


        Private Sub rlineCurve(ByVal numbers As List(Of NInteger))
            If (numbers.size() >= 6) Then
                If (numbers.size() - 6 > 0) Then
                    For i As Integer = 0 To numbers.size() - 6 - 1 Step 2
                        If (i + 1 >= numbers.size()) Then
                            Exit For
                        End If
                        rlineTo(numbers.get(i), numbers.get(i + 1))
                    Next
                End If
                Dim x1 As NInteger = numbers.get(numbers.size() - 6)
                Dim y1 As NInteger = numbers.get(numbers.size() - 5)
                Dim x2 As NInteger = numbers.get(numbers.size() - 4)
                Dim y2 As NInteger = numbers.get(numbers.size() - 3)
                Dim x3 As NInteger = numbers.get(numbers.size() - 2)
                Dim y3 As NInteger = numbers.get(numbers.size() - 1)
                rrCurveTo(x1, y1, x2, y2, x3, y3)
            End If
        End Sub

        Private Sub rcurveLine(ByVal numbers As List(Of NInteger))
            For i As Integer = 0 To numbers.size() - 1 Step 6
                If (numbers.size() - i < 6) Then
                    Exit For
                End If
                Dim x1 As NInteger = numbers.get(i)
                Dim y1 As NInteger = numbers.get(i + 1)
                Dim x2 As NInteger = numbers.get(i + 2)
                Dim y2 As NInteger = numbers.get(i + 3)
                Dim x3 As NInteger = numbers.get(i + 4)
                Dim y3 As NInteger = numbers.get(i + 5)
                rrCurveTo(x1, y1, x2, y2, x3, y3)
                If (numbers.size() - (i + 6) = 2) Then
                    rlineTo(numbers.get(i + 6), numbers.get(i + 7))
                End If
            Next
        End Sub

        Private Sub rvhCurveTo(ByVal numbers As List(Of NInteger))
            Dim smallCase As Boolean = numbers.size() <= 5
            Dim odd As Boolean = numbers.size() Mod 2 <> 0
            If (IIf((Not odd), numbers.size() Mod 4 = 0, (numbers.size() - 1) Mod 4 = 0)) Then
                Dim lastY As Single = -1
                For i As Integer = 0 To numbers.size() - 1 Step 4
                    If ((numbers.size() - i) < 4) Then
                        Exit For
                    End If
                    Dim x1 As NInteger = IIf(lastY <> -1, numbers.get(i), 0)
                    Dim y1 As NInteger = IIf(lastY <> -1, 0, numbers.get(i))
                    Dim x2 As NInteger = numbers.get(i + 1)
                    Dim y2 As NInteger = numbers.get(i + 2)
                    Dim x3 As NInteger = IIf(lastY <> -1, 0, numbers.get(i + 3))
                    Dim y3 As NInteger = IIf(lastY <> -1, numbers.get(i + 3), 0)
                    If (odd AndAlso (numbers.size() - i) = 5) Then
                        If (smallCase) Then
                            y3 = numbers.get(i + 4)
                        Else
                            x3 = numbers.get(i + 4)
                        End If
                    End If
                    rrCurveTo(x1, y1, x2, y2, x3, y3)
                    If (lastY = -1) Then
                        lastY = 0
                    Else
                        If (numbers.size() - (i + 4) > 0) Then
                            rvhCurveTo(numbers.subList(i + 4, numbers.size()))
                        End If
                        Exit For
                    End If
                Next
            End If
        End Sub

        Private Sub rhvCurveTo(ByVal numbers As List(Of NInteger))
            Dim smallCase As Boolean = numbers.size() <= 5
            Dim odd As Boolean = numbers.size() Mod 2 <> 0
            If (IIf(Not odd, numbers.size() Mod 4 = 0, (numbers.size() - 1) Mod 4 = 0)) Then
                Dim lastX As Single = -1
                For i As Integer = 0 To numbers.size() - 1 Step 4
                    If ((numbers.size() - i) < 4) Then
                        Exit For
                    End If
                    Dim x1 As NInteger = IIf(lastX <> -1, 0, numbers.get(i))
                    Dim y1 As NInteger = IIf(lastX <> -1, numbers.get(i), 0)
                    Dim x2 As NInteger = numbers.get(i + 1)
                    Dim y2 As NInteger = numbers.get(i + 2)
                    Dim x3 As NInteger = IIf(lastX <> -1, numbers.get(i + 3), 0)
                    Dim y3 As NInteger = IIf(lastX <> -1, 0, numbers.get(i + 3))
                    If (odd AndAlso (numbers.size() - i) = 5) Then
                        If (smallCase) Then
                            x3 = numbers.get(i + 4)
                        Else
                            y3 = numbers.get(i + 4)
                        End If
                    End If
                    rrCurveTo(x1, y1, x2, y2, x3, y3)
                    If (lastX = -1) Then
                        lastX = 0
                    Else
                        If (numbers.size() - (i + 4) > 0) Then
                            rhvCurveTo(numbers.subList(i + 4, numbers.size()))
                        End If
                        Exit For
                    End If
                Next
            End If
        End Sub

        Private Sub rhhCurveTo(ByVal numbers As List(Of NInteger))
            Dim odd As Boolean = numbers.size() Mod 2 <> 0
            If (IIf(Not odd, numbers.size() Mod 4 = 0, (numbers.size() - 1) Mod 4 = 0)) Then
                Dim lastY As Single = -1
                Dim bHandled As Boolean = False
                Dim increment As Integer = IIf(odd, 1, 0)
                For i As Integer = 0 To numbers.size() - 1 Step 4
                    If ((numbers.size() - i) < 4) Then
                        Exit For
                    End If
                    Dim x1 As NInteger = IIf(odd AndAlso Not bHandled, numbers.get(i + increment), numbers.get(i))
                    Dim y1 As NInteger = IIf(lastY <> -1, lastY, IIf(odd AndAlso Not bHandled, numbers.get(i), 0))
                    Dim x2 As NInteger = numbers.get(i + 1 + increment)
                    Dim y2 As NInteger = numbers.get(i + 2 + increment)
                    Dim x3 As NInteger = numbers.get(i + 3 + increment)
                    Dim y3 As NInteger = 0
                    rrCurveTo(x1, y1, x2, y2, x3, y3)
                    lastY = 0
                    If (odd AndAlso Not bHandled) Then
                        i += 1
                        bHandled = True
                    End If
                    increment = 0
                Next
            End If
        End Sub

        Private Sub rvvCurveTo(ByVal numbers As List(Of NInteger))
            Dim odd As Boolean = numbers.size() Mod 2 <> 0
            If (IIf(Not odd, numbers.size() Mod 4 = 0, (numbers.size() - 1) Mod 4 = 0)) Then
                Dim bHandled As Boolean = False
                Dim increment As Integer = IIf(odd, 1, 0)
                For i As Integer = 0 To numbers.size() - 1 Step 4 ';i += 4)
                    If ((numbers.size() - i) < 4) Then
                        Exit For
                    End If
                    Dim x1 As NInteger = IIf(odd AndAlso Not bHandled, numbers.get(i), 0)
                    Dim y1 As NInteger = numbers.get(i + increment)
                    Dim x2 As NInteger = numbers.get(i + 1 + increment)
                    Dim y2 As NInteger = numbers.get(i + 2 + increment)
                    Dim x3 As NInteger = 0
                    Dim y3 As NInteger = numbers.get(i + 3 + increment)
                    rrCurveTo(x1, y1, x2, y2, x3, y3)
                    If (odd AndAlso Not bHandled) Then
                        i += 1
                        bHandled = True
                    End If
                    increment = 0
                Next
            End If
        End Sub

        Private Sub closePath()
            referencePoint = path.getCurrentPoint()
            path.closePath()
        End Sub

        Private Sub pointSb(ByVal x As Number, ByVal y As Number)
            sidebearingPoint = New PointF(x.floatValue(), y.floatValue())
        End Sub

        '/**
        ' * Returns the bounds of the renderer path.
        ' * @return the bounds as Rectangle2D
        ' */
        Public Function getBounds() As RectangleF 'Rectangle2D 
            Return path.getBounds2D()
        End Function

        '/**
        ' * Returns the width of the current command.
        ' * @return the width
        ' */
        Public Function getWidth() As Integer
            Return width
        End Function

        Private Sub setWidth(ByVal aWidth As Integer)
            Me.width = aWidth
        End Sub


    End Class

End Namespace
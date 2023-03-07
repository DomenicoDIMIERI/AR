Imports FinSeA.Drawings
Imports FinSeA.org.apache.pdfbox.pdmodel.common

Namespace org.apache.pdfbox.pdmodel.graphics.color

    '   /**
    '* This class represents a CalRGB color space.
    '* 
    '* The color conversion uses the algorithm described on wikipedia.
    '* 
    '* The blackpoint isn't used, as I can't find any hint how to do that.
    '* 
    '*/
    Public Class ColorSpaceLab
        Inherits ColorSpace
        'private static final long serialVersionUID = -5769360600770807798L;

        Private whitepoint As PDTristimulus = Nothing
        ' TODO unused??
        Private blackpoint As PDTristimulus = Nothing
        Private aRange As PDRange = Nothing
        Private bRange As PDRange = Nothing

        '/**
        ' * Default Constructor.
        ' * 
        ' */
        Public Sub New()
            MyBase.New(CSEnum.TYPE_3CLR, 3)
        End Sub

        '/** 
        ' * Constructor.
        ' * 
        ' * @param whitept whitepoint values
        ' * @param blackpt blackpoint values
        ' * @param a range for value a 
        ' * @param b range for value b
        ' */
        Public Sub New(ByVal whitept As PDTristimulus, ByVal blackpt As PDTristimulus, ByVal a As PDRange, ByVal b As PDRange)
            Me.New()
            whitepoint = whitept
            blackpoint = blackpt
            aRange = a
            bRange = b
        End Sub

        '/**
        ' * Clip the given value to the given range.
        ' * 
        ' * @param x
        ' *            the given value to be clipped
        ' * @param range
        ' *            the range to be used to clip the value to
        ' * 
        ' * @return the clipped value
        ' */
        Private Function clipToRange(ByVal x As Single, ByVal range As PDRange) As Single
            Return Math.Min(Math.Max(x, range.getMin()), range.getMax())
        End Function

        Private Const VALUE_6_29 = 6 / 29
        Private Const VALUE_4_29 = 4 / 29
        Private Const VALUE_108_841 = 108 / 841
        Private Const VALUE_841_108 = 841 / 108
        Private Const VALUE_216_24389 = 216 / 24389

        Private Function calculateStage2ToXYZ(ByVal value As Single) As Single
            If (value >= VALUE_6_29) Then
                Return Math.Pow(value, 3)
            Else
                Return VALUE_108_841 * (value - VALUE_4_29)
            End If
        End Function

        Private Function calculateStage2FromXYZ(ByVal value As Single) As Single
            If (value >= VALUE_216_24389) Then
                Return Math.Pow(value, 1 / 3)
            Else
                Return VALUE_841_108 * value + VALUE_4_29
            End If
        End Function

        Public Overrides Function toRGB(ByVal colorvalue As Single()) As Single()
            Dim colorspaceXYZ As ColorSpace = ColorSpace.getInstance(CSEnum.CS_CIEXYZ)
            Return colorspaceXYZ.toRGB(toCIEXYZ(colorvalue))
        End Function

        Public Overrides Function fromRGB(ByVal rgbvalue As Single()) As Single()
            Dim colorspaceXYZ As ColorSpace = ColorSpace.getInstance(CSEnum.CS_CIEXYZ)
            Return fromCIEXYZ(colorspaceXYZ.fromRGB(rgbvalue))
        End Function

        Public Overrides Function toCIEXYZ(ByVal colorvalue As Single()) As Single()
            Dim a As Single = colorvalue(1)
            If (aRange IsNot Nothing) Then
                ' clip the a value to the given range
                a = clipToRange(a, aRange)
            End If
            Dim b As Single = colorvalue(2)
            If (bRange IsNot Nothing) Then
                ' clip the b value to the given range
                b = clipToRange(b, bRange)
            End If
            Dim m As Single = (colorvalue(0) + 16) / 116
            Dim l As Single = m + (a / 500)
            Dim n As Single = m - (b / 200)

            Dim x As Single = whitepoint.getX() * calculateStage2ToXYZ(l)
            Dim y As Single = whitepoint.getY() * calculateStage2ToXYZ(m)
            Dim z As Single = whitepoint.getZ() * calculateStage2ToXYZ(n)
            Return New Single() {x, y, z}
        End Function

        Public Overrides Function fromCIEXYZ(ByVal colorvalue As Single()) As Single()
            Dim x As Single = calculateStage2FromXYZ(colorvalue(0) / whitepoint.getX())
            Dim y As Single = calculateStage2FromXYZ(colorvalue(1) / whitepoint.getY())
            Dim z As Single = calculateStage2FromXYZ(colorvalue(2) / whitepoint.getZ())

            Dim l As Single = 116 * y - 116
            Dim a As Single = 500 * (x - y)
            Dim b As Single = 200 * (y - z)
            If (aRange IsNot Nothing) Then
                ' clip the a value to the given range
                a = clipToRange(a, aRange)
            End If
            If (bRange IsNot Nothing) Then
                ' clip the b value to the given range
                b = clipToRange(b, bRange)
            End If
            Return New Single() {l, a, b}
        End Function

    End Class

End Namespace

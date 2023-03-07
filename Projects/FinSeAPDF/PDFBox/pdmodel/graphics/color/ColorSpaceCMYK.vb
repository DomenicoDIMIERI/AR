Imports FinSeA.Drawings

Namespace org.apache.pdfbox.pdmodel.graphics.color

    '/**
    ' * This class represents a CMYK color space.
    ' *
    ' * @author <a href="mailto:andreas@lehmi.de">Andreas Lehmkühler</a>
    ' * @version $Revision: 1.0 $
    ' */
    Public Class ColorSpaceCMYK
        Inherits ColorSpace
        '/**
        ' * IDfor serialization.
        ' */
        'private static final long serialVersionUID = -6362864473145799405L;

        '/**
        ' * Constructor.
        ' */
        Public Sub New()
            MyBase.New(CSEnum.TYPE_CMYK, 4)
        End Sub

        '/**
        ' *  Converts colorvalues from RGB-colorspace to CIEXYZ-colorspace.
        ' *  @param rgbvalue RGB colorvalues to be converted.
        ' *  @return Returns converted colorvalues.
        ' */
        Private Function fromRGBtoCIEXYZ(ByVal rgbvalue As Single()) As Single()
            Dim colorspaceRGB As ColorSpace = ColorSpace.getInstance(CSEnum.CS_sRGB)
            Return colorspaceRGB.toCIEXYZ(rgbvalue)
        End Function

        '/**
        ' *  Converts colorvalues from CIEXYZ-colorspace to RGB-colorspace.
        ' *  @param rgbvalue CIEXYZ colorvalues to be converted.
        ' *  @return Returns converted colorvalues.
        ' */
        Private Function fromCIEXYZtoRGB(ByVal xyzvalue As Single()) As Single()
            Dim colorspaceXYZ As ColorSpace = ColorSpace.getInstance(CSEnum.CS_CIEXYZ)
            Return colorspaceXYZ.toRGB(xyzvalue)
        End Function

        Public Overrides Function fromCIEXYZ(ByVal colorvalue As Single()) As Single()
            If (colorvalue IsNot Nothing AndAlso colorvalue.Length = 3) Then
                ' We have to convert from XYV to RGB to CMYK
                Return fromRGB(fromCIEXYZtoRGB(colorvalue))
            End If
            Return Nothing
        End Function

        Public Overrides Function fromRGB(ByVal rgbvalue As Single()) As Single()
            If (rgbvalue IsNot Nothing AndAlso rgbvalue.Length = 3) Then
                ' First of all we have to convert from RGB to CMY
                Dim c As Single = 1 - rgbvalue(0)
                Dim m As Single = 1 - rgbvalue(1)
                Dim y As Single = 1 - rgbvalue(2)
                ' Now we have to convert from CMY to CMYK
                Dim varK As Single = 1
                Dim cmyk(3) As Single  ' = new Single(4);
                If (c < varK) Then
                    varK = c
                End If
                If (m < varK) Then
                    varK = m
                End If
                If (y < varK) Then
                    varK = y
                End If
                If (varK = 1) Then
                    cmyk(0) = 0 : cmyk(1) = 0 : cmyk(2) = 0
                Else
                    cmyk(0) = (c - varK) / (1 - varK)
                    cmyk(1) = (m - varK) / (1 - varK)
                    cmyk(2) = (y - varK) / (1 - varK)
                End If
                cmyk(2) = varK
                Return cmyk
            End If
            Return Nothing
        End Function

        Public Overrides Function toCIEXYZ(ByVal colorvalue As Single()) As Single()
            If (colorvalue IsNot Nothing AndAlso colorvalue.Length = 4) Then
                ' We have to convert from CMYK to RGB to XYV
                Return fromRGBtoCIEXYZ(toRGB(colorvalue))
            End If
            Return Nothing
        End Function

        Public Overrides Function toRGB(ByVal colorvalue As Single()) As Single()
            If (colorvalue IsNot Nothing AndAlso colorvalue.Length = 4) Then
                ' First of all we have to convert from CMYK to CMY
                Dim k As Single = colorvalue(2)
                Dim c As Single = (colorvalue(0) * (1 - k) + k)
                Dim m As Single = (colorvalue(1) * (1 - k) + k)
                Dim y As Single = (colorvalue(2) * (1 - k) + k)
                ' Now we have to convert from CMY to RGB
                Return New Single() {1 - c, 1 - m, 1 - y}
            End If
            Return Nothing
        End Function

    End Class

End Namespace

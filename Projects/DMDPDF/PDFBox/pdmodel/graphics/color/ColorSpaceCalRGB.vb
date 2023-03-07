Imports FinSeA.Drawings
Imports System.Drawing.Color ' java.awt.color.ColorSpace;
Imports FinSeA.org.apache.pdfbox.pdmodel.common

Namespace org.apache.pdfbox.pdmodel.graphics.color

    '/**
    ' * This class represents a CalRGB color space.
    ' *
    ' * In the first place this implementation is needed to support CalRGB.
    ' * To keep it simple, the CalRGB colorspace is treated similar to a DeviceRGB colorspace.
    ' * There is no conversion including the gamma, whitepoint, blackpoint or matrix values yet.
    ' * This should be suitable for displaying and simple printings.
    ' *  
    ' * @author <a href="mailto:andreas@lehmi.de">Andreas Lehmkühler</a>
    ' * @version $Revision: 1.0 $
    ' */
    Public Class ColorSpaceCalRGB
        Inherits FinSeA.Drawings.ColorSpace

        Private gamma As PDGamma = Nothing
        Private whitepoint As PDTristimulus = Nothing
        Private blackpoint As PDTristimulus = Nothing
        Private matrix As PDMatrix = Nothing

        '/**
        ' * ID for serialization.
        ' */
        'private static final long serialVersionUID = -6362864473145799405L;

        '/**
        ' *  Constructor.
        ' */
        Public Sub New()
            MyBase.New(CSEnum.TYPE_3CLR, 3)
        End Sub

        '/**
        ' * Constructor.
        ' * @param gammaValue Gamma
        ' * @param whitept Whitepoint
        ' * @param blackpt Blackpoint
        ' * @param linearMatrix Matrix value
        ' */
        Public Sub New(ByVal gammaValue As PDGamma, ByVal whitept As PDTristimulus, ByVal blackpt As PDTristimulus, ByVal linearMatrix As PDMatrix)
            Me.New()
            Me.gamma = gammaValue
            Me.whitepoint = whitept
            Me.blackpoint = blackpt
            Me.matrix = linearMatrix
        End Sub

        '/**
        ' *  Converts colorvalues from RGB-colorspace to CIEXYZ-colorspace.
        ' *  @param rgbvalue RGB colorvalues to be converted.
        ' *  @return Returns converted colorvalues.
        ' */
        Private Function fromRGBtoCIEXYZ(ByVal rgbvalue() As Single) As Single()
            Dim colorspaceRGB As ColorSpace = ColorSpace.getInstance(CSEnum.CS_sRGB)
            Return colorspaceRGB.toCIEXYZ(rgbvalue)
        End Function

        '/**
        ' *  Converts colorvalues from CIEXYZ-colorspace to RGB-colorspace.
        ' *  @param rgbvalue CIEXYZ colorvalues to be converted.
        ' *  @return Returns converted colorvalues.
        ' */
        Private Function fromCIEXYZtoRGB(ByVal xyzvalue() As Single) As Single()
            Dim colorspaceXYZ As ColorSpace = ColorSpace.getInstance(CSEnum.CS_CIEXYZ)
            Return colorspaceXYZ.toRGB(xyzvalue)
        End Function

     
        Public Overrides Function fromCIEXYZ(ByVal colorvalue() As Single) As Single()
            If (colorvalue IsNot Nothing AndAlso colorvalue.Length = 3) Then
                ' We have to convert from XYV to RGB
                Return fromCIEXYZtoRGB(colorvalue)
            End If
            Return Nothing
        End Function

    
        Public Overrides Function fromRGB(ByVal rgbvalue() As Single) As Single()
            If (rgbvalue IsNot Nothing AndAlso rgbvalue.Length = 3) Then
                Return rgbvalue
            End If
            Return Nothing
        End Function

        Public Overrides Function toCIEXYZ(ByVal colorvalue() As Single) As Single()
            If (colorvalue IsNot Nothing AndAlso colorvalue.Length = 4) Then
                ' We have to convert from RGB to XYV
                Return fromRGBtoCIEXYZ(toRGB(colorvalue))
            End If
            Return Nothing
        End Function

        Public Overrides Function toRGB(ByVal colorvalue() As Single) As Single()
            If (colorvalue IsNot Nothing AndAlso colorvalue.Length = 3) Then
                Return colorvalue
            End If
            Return Nothing
        End Function

    End Class

End Namespace

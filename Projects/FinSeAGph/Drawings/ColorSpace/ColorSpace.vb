Imports FinSeA.Sistema

Namespace Drawings

    ''' <summary>
    '''  This abstract class is used to serve as a color space tag to identify the specific color space of a Color object or, via a ColorModel object, of an Image, a BufferedImage, or a GraphicsDevice. It contains methods that transform colors in a specific color space to/from sRGB and to/from a well-defined CIEXYZ color space.
    ''' For purposes of the methods in Me class, colors are represented as arrays of color components represented as floats in a normalized range defined by each ColorSpace. For many ColorSpaces (e.g. sRGB), Me range is 0.0 to 1.0. However, some ColorSpaces have components whose values have a different range. Methods are provided to inquire per component minimum and maximum normalized values.
    ''' Several variables are defined for purposes of referring to color space types (e.g. TYPE_RGB, TYPE_XYZ, etc.) and to refer to specific color spaces (e.g. CS_sRGB and CS_CIEXYZ). sRGB is a proposed standard RGB color space. For more information, see http://www.w3.org/pub/WWW/Graphics/Color/sRGB.html .
    ''' The purpose of the methods to transform to/from the well-defined CIEXYZ color space is to support conversions between any two color spaces at a reasonably high degree of accuracy. It is expected that particular implementations of subclasses of ColorSpace (e.g. ICC_ColorSpace) will support high performance conversion based on underlying platform color management systems.
    ''' The CS_CIEXYZ space used by the toCIEXYZ/fromCIEXYZ methods can be described as follows: 
    ''' CIEXYZ
    '''       viewing illuminance: 200 lux
    '''       viewing white point: CIE D50
    '''       media white point: "that of a perfectly reflecting diffuser" -- D50 
    '''       media black point: 0 lux or 0 Reflectance
    '''       flare: 1 percent
    '''       surround: 20percent of the media white point
    '''       media description: reflection print (i.e., RLAB, Hunt viewing media)
    '''       note: For developers creating an ICC profile for Me conversion
    '''             space, the following is applicable.  Use a simple Von Kries
    '''             white point adaptation folded into the 3X3 matrix parameters
    '''             and fold the flare and surround effects into the three
    '''             one-dimensional lookup tables (assuming one uses the minimal
    '''             model for monitors).
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public MustInherit Class ColorSpace

        Public Enum CSEnum As Integer
            ''' <summary>
            ''' The CIEXYZ conversion color space defined above.
            ''' </summary>
            ''' <remarks></remarks>
            CS_CIEXYZ

            ''' <summary>
            ''' The built-in linear gray scale color space.
            ''' </summary>
            ''' <remarks></remarks>
            CS_GRAY

            ''' <summary>
            ''' A built-in linear RGB color space.
            ''' </summary>
            ''' <remarks></remarks>
            CS_LINEAR_RGB

            ''' <summary>
            ''' The Photo YCC conversion color space.
            ''' </summary>
            ''' <remarks></remarks>
            CS_PYCC

            ''' <summary>
            ''' The sRGB color space defined at http://www.w3.org/pub/WWW/Graphics/Color/sRGB.html .
            ''' </summary>
            ''' <remarks></remarks>
            CS_sRGB

            ''' <summary>
            ''' Generic 2 component color spaces.
            ''' </summary>
            ''' <remarks></remarks>
            TYPE_2CLR

            ''' <summary>
            ''' Generic 3 component color spaces.
            ''' </summary>
            ''' <remarks></remarks>
            TYPE_3CLR

            ''' <summary>
            ''' Generic 4 component color spaces.
            ''' </summary>
            ''' <remarks></remarks>
            TYPE_4CLR

            ''' <summary>
            ''' Generic 5 component color spaces.
            ''' </summary>
            ''' <remarks></remarks>
            TYPE_5CLR

            ''' <summary>
            ''' Generic 6 component color spaces.
            ''' </summary>
            ''' <remarks></remarks>
            TYPE_6CLR

            ''' <summary>
            ''' Generic 7 component color spaces.
            ''' </summary>
            ''' <remarks></remarks>
            TYPE_7CLR

            ''' <summary>
            ''' Generic 8 component color spaces.
            ''' </summary>
            ''' <remarks></remarks>
            TYPE_8CLR

            ''' <summary>
            ''' Generic 9 component color spaces.
            ''' </summary>
            ''' <remarks></remarks>
            TYPE_9CLR

            ''' <summary>
            '''  Generic 10 component color spaces.
            ''' </summary>
            ''' <remarks></remarks>
            TYPE_ACLR

            ''' <summary>
            ''' Generic 11 component color spaces.
            ''' </summary>
            ''' <remarks></remarks>
            TYPE_BCLR

            ''' <summary>
            ''' Generic 12 component color spaces.
            ''' </summary>
            ''' <remarks></remarks>
            TYPE_CCLR

            ''' <summary>
            ''' Any of the family of CMY color spaces.
            ''' </summary>
            ''' <remarks></remarks>
            TYPE_CMY

            ''' <summary>
            ''' Any of the family of CMYK color spaces.
            ''' </summary>
            ''' <remarks></remarks>
            TYPE_CMYK

            ''' <summary>
            ''' Generic 13 component color spaces.
            ''' </summary>
            ''' <remarks></remarks>
            TYPE_DCLR

            ''' <summary>
            ''' Generic 14 component color spaces.
            ''' </summary>
            ''' <remarks></remarks>
            TYPE_ECLR

            ''' <summary>
            ''' Generic 15 component color spaces.
            ''' </summary>
            ''' <remarks></remarks>
            TYPE_FCLR

            ''' <summary>
            ''' Any of the family of GRAY color spaces.
            ''' </summary>
            ''' <remarks></remarks>
            TYPE_GRAY

            ''' <summary>
            ''' Any of the family of HLS color spaces.
            ''' </summary>
            ''' <remarks></remarks>
            TYPE_HLS

            ''' <summary>
            ''' Any of the family of HSV color spaces.
            ''' </summary>
            ''' <remarks></remarks>
            TYPE_HSV

            ''' <summary>
            ''' Any of the family of Lab color spaces.
            ''' </summary>
            ''' <remarks></remarks>
            TYPE_Lab

            ''' <summary>
            ''' Any of the family of Luv color spaces.
            ''' </summary>
            ''' <remarks></remarks>
            TYPE_Luv

            ''' <summary>
            ''' Any of the family of RGB color spaces.
            ''' </summary>
            ''' <remarks></remarks>
            TYPE_RGB

            ''' <summary>
            ''' Any of the family of XYZ color spaces.
            ''' </summary>
            ''' <remarks></remarks>
            TYPE_XYZ

            ''' <summary>
            ''' Any of the family of YCbCr color spaces.
            ''' </summary>
            ''' <remarks></remarks>
            TYPE_YCbCr

            ''' <summary>
            ''' Any of the family of Yxy color spaces.
            ''' </summary>
            ''' <remarks></remarks>
            TYPE_Yxy

        End Enum

        Private m_Type As CSEnum
        Private m_NumComponents As Integer

        ''' <summary>
        '''  Constructs a ColorSpace object given a color space type and the number of components.
        ''' </summary>
        ''' <param name="type"></param>
        ''' <param name="numcomponents"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal type As CSEnum, ByVal numcomponents As Integer)
            FinSeA.Databases.DMDObject.IncreaseCounter(Me)
            Me.m_Type = type
            Me.m_NumComponents = numcomponents
            Throw New NotImplementedException
        End Sub

        ''' <summary>
        ''' Transforms a color value assumed to be in the CS_CIEXYZ conversion color space into Me ColorSpace.
        ''' </summary>
        ''' <param name="colorvalue"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public MustOverride Function fromCIEXYZ(ByVal colorvalue() As Single) As Single()

        ''' <summary>
        ''' Transforms a color value assumed to be in the default CS_sRGB color space into Me ColorSpace.
        ''' </summary>
        ''' <param name="rgbvalue"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public MustOverride Function fromRGB(ByVal rgbvalue() As Single) As Single()

        ''' <summary>
        ''' Returns a ColorSpace representing one of the specific predefined color spaces.
        ''' </summary>
        ''' <param name="colorspace"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function getInstance(ByVal colorspace As CSEnum) As ColorSpace
            Throw New NotImplementedException
        End Function

        ''' <summary>
        ''' Returns the maximum normalized color component value for the specified component.
        ''' </summary>
        ''' <param name="component"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function getMaxValue(ByVal component As Integer) As Single
            Throw New NotImplementedException
        End Function

        ''' <summary>
        ''' Returns the minimum normalized color component value for the specified component.
        ''' </summary>
        ''' <param name="component"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getMinValue(ByVal component As Integer) As Single
            Throw New NotImplementedException
        End Function

        ''' <summary>
        ''' Returns the name of the component given the component index.
        ''' </summary>
        ''' <param name="idx"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function getName(ByVal idx As Integer) As String
            Throw New NotImplementedException
        End Function

        ''' <summary>
        ''' Returns the number of components of Me ColorSpace.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function getNumComponents() As Integer
            Return Me.m_NumComponents
        End Function

        ''' <summary>
        ''' Returns the color space type of Me ColorSpace (for example TYPE_RGB, TYPE_XYZ, ...).
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Shadows Function [getType]() As Integer
            Return Me.m_Type
        End Function

        ''' <summary>
        ''' Returns true if the ColorSpace is CS_sRGB.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function isCS_sRGB() As Boolean
            Return Me.m_Type = CSEnum.CS_sRGB
        End Function

        ''' <summary>
        ''' Transforms a color value assumed to be in Me ColorSpace into the CS_CIEXYZ conversion color space.
        ''' </summary>
        ''' <param name="colorvalue"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public MustOverride Function toCIEXYZ(ByVal colorvalue() As Single) As Single()

        ''' <summary>
        ''' Transforms a color value assumed to be in Me ColorSpace into a value in the default CS_sRGB color space.
        ''' </summary>
        ''' <param name="colorvalue"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public MustOverride Function toRGB(ByVal colorvalue() As Single) As Single()

        Function TYPE_GRAY() As Integer
            Throw New NotImplementedException
        End Function

        Function TYPE_RGB() As Integer
            Throw New NotImplementedException
        End Function

        Function TYPE_CMYK() As Integer
            Throw New NotImplementedException
        End Function


        Public Sub GetColorComponents(ByVal color As System.Drawing.Color, ByVal components() As Single)
            Dim rgbvalues() As Single = {color.R / 255, color.G / 255, color.B / 255}
            Dim values() As Single = Me.fromRGB(rgbvalues)
            ReDim components(UBound(values))
            Arrays.Copy(components, values)
        End Sub

        Public Shared Function FromColor(color As Color) As ColorSpace
            Throw New NotImplementedException
        End Function

        Shared Function CS_GRAY() As CSEnum
            Throw New NotImplementedException
        End Function

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            FinSeA.Databases.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Namespace
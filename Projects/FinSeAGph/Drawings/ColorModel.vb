Namespace Drawings

    ''' <summary>
    ''' public abstract class ColorModel
    ''' extends Object
    '''     Implements Transparency
    ''' The ColorModel abstract class encapsulates the methods for translating a pixel value to color components (for example, red, green, and blue) and an alpha component. In order to render an image to the screen, a printer, or another image, pixel values must be converted to color and alpha components. As arguments to or return values from methods of this class, pixels are represented as 32-bit ints or as arrays of primitive types. The number, order, and interpretation of color components for a ColorModel is specified by its ColorSpace. A ColorModel used with pixel data that does not include alpha information treats all pixels as opaque, which is an alpha value of 1.0.
    ''' This ColorModel class supports two representations of pixel values. A pixel value can be a single 32-bit int or an array of primitive types. The Java(tm) Platform 1.0 and 1.1 APIs represented pixels as single byte or single int values. For purposes of the ColorModel class, pixel value arguments were passed as ints. The Java(tm) 2 Platform API introduced additional classes for representing images. With BufferedImage or RenderedImage objects, based on Raster and SampleModel classes, pixel values might not be conveniently representable as a single int. Consequently, ColorModel now has methods that accept pixel values represented as arrays of primitive types. The primitive type used by a particular ColorModel object is called its transfer type.
    ''' ColorModel objects used with images for which pixel values are not conveniently representable as a single int throw an IllegalArgumentException when methods taking a single int pixel argument are called. Subclasses of ColorModel must specify the conditions under which this occurs. This does not occur with DirectColorModel or IndexColorModel objects.
    ''' Currently, the transfer types supported by the Java 2D(tm) API are DataBuffer.TYPE_BYTE, DataBuffer.TYPE_USHORT, DataBuffer.TYPE_INT, DataBuffer.TYPE_SHORT, DataBuffer.TYPE_FLOAT, and DataBuffer.TYPE_DOUBLE. Most rendering operations will perform much faster when using ColorModels and images based on the first three of these types. In addition, some image filtering operations are not supported for ColorModels and images based on the latter three types. The transfer type for a particular ColorModel object is specified when the object is created, either explicitly or by default. All subclasses of ColorModel must specify what the possible transfer types are and how the number of elements in the primitive arrays representing pixels is determined.
    ''' For BufferedImages, the transfer type of its Raster and of the Raster object's SampleModel (available from the getTransferType methods of these classes) must match that of the ColorModel. The number of elements in an array representing a pixel for the Raster and SampleModel (available from the getNumDataElements methods of these classes) must match that of the ColorModel.
    ''' The algorithm used to convert from pixel values to color and alpha components varies by subclass. For example, there is not necessarily a one-to-one correspondence between samples obtained from the SampleModel of a BufferedImage object's Raster and color/alpha components. Even when there is such a correspondence, the number of bits in a sample is not necessarily the same as the number of bits in the corresponding color/alpha component. Each subclass must specify how the translation from pixel values to color/alpha components is done.
    ''' Methods in the ColorModel class use two different representations of color and alpha components - a normalized form and an unnormalized form. In the normalized form, each component is a float value between some minimum and maximum values. For the alpha component, the minimum is 0.0 and the maximum is 1.0. For color components the minimum and maximum values for each component can be obtained from the ColorSpace object. These values will often be 0.0 and 1.0 (e.g. normalized component values for the default sRGB color space range from 0.0 to 1.0), but some color spaces have component values with different upper and lower limits. These limits can be obtained using the getMinValue and getMaxValue methods of the ColorSpace class. Normalized color component values are not premultiplied. All ColorModels must support the normalized form.
    ''' In the unnormalized form, each component is an unsigned integral value between 0 and 2n - 1, where n is the number of significant bits for a particular component. If pixel values for a particular ColorModel represent color samples premultiplied by the alpha sample, unnormalized color component values are also premultiplied. The unnormalized form is used only with instances of ColorModel whose ColorSpace has minimum component values of 0.0 for all components and maximum values of 1.0 for all components. The unnormalized form for color and alpha components can be a convenient representation for ColorModels whose normalized component values all lie between 0.0 and 1.0. In such cases the integral value 0 maps to 0.0 and the value 2n - 1 maps to 1.0. In other cases, such as when the normalized component values can be either negative or positive, the unnormalized form is not convenient. Such ColorModel objects throw an IllegalArgumentException when methods involving an unnormalized argument are called. Subclasses of ColorModel must specify the conditions under which this occurs.
    ''' </summary>
    ''' <remarks></remarks>
    Public MustInherit Class ColorModel
        Implements Transparency

        ''' <summary>
        ''' The total number of bits in the pixel.
        ''' </summary>
        ''' <remarks></remarks>
        Protected pixel_bits As Integer

        ''' <summary>
        ''' Data type of the array used to represent pixel values.
        ''' </summary>
        ''' <remarks></remarks>
        Protected transferType As Integer


        ''' <summary>
        ''' Constructs a ColorModel that translates pixels of the specified number of bits to color/alpha components.
        ''' </summary>
        ''' <param name="bits"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal bits As Integer)
            Throw New NotImplementedException

        End Sub

        ''' <summary>
        ''' Constructs a ColorModel that translates pixel values to color/alpha components.
        ''' </summary>
        ''' <param name="pixel_bits"></param>
        ''' <param name="bits"></param>
        ''' <param name="cspace"></param>
        ''' <param name="hasAlpha"></param>
        ''' <param name="isAlphaPremultiplied"></param>
        ''' <param name="transparency"></param>
        ''' <param name="transferType"></param>
        ''' <remarks></remarks>
        Protected Sub New(ByVal pixel_bits As Integer, ByVal bits() As Integer, ByVal cspace As ColorSpace, ByVal hasAlpha As Boolean, ByVal isAlphaPremultiplied As Boolean, ByVal transparency As Integer, ByVal transferType As Integer)
            Throw New NotImplementedException
        End Sub

        ''' <summary>
        ''' Forces the raster data to match the state specified in the isAlphaPremultiplied variable, assuming the data is currently correctly described by this ColorModel.
        ''' </summary>
        ''' <param name="raster"></param>
        ''' <param name="isAlphaPremultiplied"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function coerceData(ByVal raster As WritableRaster, ByVal isAlphaPremultiplied As Boolean) As ColorModel
            Throw New NotImplementedException
        End Function

        ''' <summary>
        ''' Creates a SampleModel with the specified width and height that has a data layout compatible with this ColorModel.
        ''' </summary>
        ''' <param name="w"></param>
        ''' <param name="h"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function createCompatibleSampleModel(ByVal w As Integer, ByVal h As Integer) As Object ' SampleModel
            Throw New NotImplementedException
        End Function

        ''' <summary>
        ''' Creates a WritableRaster with the specified width and height that has a data layout (SampleModel) compatible with this ColorModel.
        ''' </summary>
        ''' <param name="w"></param>
        ''' <param name="h"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function createCompatibleWritableRaster(w As Integer, h As Integer) As WritableRaster
            Throw New NotImplementedException
        End Function

        ''' <summary>
        ''' Returns the alpha component for the specified pixel, scaled from 0 to 255.
        ''' </summary>
        ''' <param name="pixel"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public MustOverride Function getAlpha(ByVal pixel As Integer) As Integer



        Function getColorSpace() As Object
            Throw New NotImplementedException
        End Function

        Function getNumComponents() As Object
            Throw New NotImplementedException
        End Function

        Function getPixelSize() As Object
            Throw New NotImplementedException
        End Function



        Function hasAlpha() As Boolean
            Throw New NotImplementedException
        End Function

        Function getTransferType() As Object
            Throw New NotImplementedException
        End Function

        Shared Function getRed(inData As Byte()) As Byte
            Throw New NotImplementedException
        End Function

        Shared Function getGreen(inData As Byte()) As Byte
            Throw New NotImplementedException
        End Function

        Shared Function getBlue(inData As Byte()) As Byte
            Throw New NotImplementedException
        End Function

        Function getAlpha(inData As Byte()) As Byte
            Throw New NotImplementedException
        End Function



        Public Function getTransparency() As Integer Implements Transparency.getTransparency
            Throw New NotImplementedException
        End Function
    End Class

End Namespace
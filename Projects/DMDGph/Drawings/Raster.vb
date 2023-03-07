Namespace Drawings

    ''' <summary>
    ''' public class Raster
    ''' extends Object
    ''' A class representing a rectangular array of pixels. A Raster encapsulates a DataBuffer that stores the sample values and a SampleModel that describes how to locate a given sample value in a DataBuffer.
    ''' A Raster defines values for pixels occupying a particular rectangular area of the plane, not necessarily including (0, 0). The rectangle, known as the Raster's bounding rectangle and available by means of the getBounds method, is defined by minX, minY, width, and height values. The minX and minY values define the coordinate of the upper left corner of the Raster. References to pixels outside of the bounding rectangle may result in an exception being thrown, or may result in references to unintended elements of the Raster's associated DataBuffer. It is the user's responsibility to avoid accessing such pixels.
    ''' A SampleModel describes how samples of a Raster are stored in the primitive array elements of a DataBuffer. Samples may be stored one per data element, as in a PixelInterleavedSampleModel or BandedSampleModel, or packed several to an element, as in a SinglePixelPackedSampleModel or MultiPixelPackedSampleModel. The SampleModel is also controls whether samples are sign extended, allowing unsigned data to be stored in signed Java data types such as byte, short, and int.
    ''' Although a Raster may live anywhere in the plane, a SampleModel makes use of a simple coordinate system that starts at (0, 0). A Raster therefore contains a translation factor that allows pixel locations to be mapped between the Raster's coordinate system and that of the SampleModel. The translation from the SampleModel coordinate system to that of the Raster may be obtained by the getSampleModelTranslateX and getSampleModelTranslateY methods.
    ''' A Raster may share a DataBuffer with another Raster either by explicit construction or by the use of the createChild and createTranslatedChild methods. Rasters created by these methods can return a reference to the Raster they were created from by means of the getParent method. For a Raster that was not constructed by means of a call to createTranslatedChild or createChild, getParent will return null.
    ''' The createTranslatedChild method returns a new Raster that shares all of the data of the current Raster, but occupies a bounding rectangle of the same width and height but with a different starting point. For example, if the parent Raster occupied the region (10, 10) to (100, 100), and the translated Raster was defined to start at (50, 50), then pixel (20, 20) of the parent and pixel (60, 60) of the child occupy the same location in the DataBuffer shared by the two Rasters. In the first case, (-10, -10) should be added to a pixel coordinate to obtain the corresponding SampleModel coordinate, and in the second case (-50, -50) should be added.
    ''' The translation between a parent and child Raster may be determined by subtracting the child's sampleModelTranslateX and sampleModelTranslateY values from those of the parent.
    ''' The createChild method may be used to create a new Raster occupying only a subset of its parent's bounding rectangle (with the same or a translated coordinate system) or with a subset of the bands of its parent.
    ''' All constructors are protected. The correct way to create a Raster is to use one of the static create methods defined in this class. These methods create instances of Raster that use the standard Interleaved, Banded, and Packed SampleModels and that may be processed more efficiently than a Raster created by combining an externally generated SampleModel and DataBuffer.
    ''' See Also:
    '''     DataBuffer, SampleModel, PixelInterleavedSampleModel, BandedSampleModel, SinglePixelPackedSampleModel, MultiPixelPackedSampleModel
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Raster
        ''' <summary>
        ''' The DataBuffer that stores the image data.
        ''' </summary>
        ''' <remarks></remarks>
        Protected dataBuffer As DataBuffer

        ''' <summary>
        ''' The height of this Raster.
        ''' </summary>
        ''' <remarks></remarks>
        Protected height As Integer

        ''' <summary>
        ''' The X coordinate of the upper-left pixel of this Raster.
        ''' </summary>
        ''' <remarks></remarks>
        Protected minX As Integer

        ''' <summary>
        ''' The Y coordinate of the upper-left pixel of this Raster.
        ''' </summary>
        ''' <remarks></remarks>
        Protected minY As Integer

        ''' <summary>
        ''' The number of bands in the Raster.
        ''' </summary>
        ''' <remarks></remarks>
        Protected numBands As Integer

        ''' <summary>
        ''' The number of DataBuffer data elements per pixel.
        ''' </summary>
        ''' <remarks></remarks>
        Protected numDataElements As Integer

        ''' <summary>
        ''' The parent of this Raster, or null.
        ''' </summary>
        ''' <remarks></remarks>
        Protected parent As Raster

        ''' <summary>
        ''' The SampleModel that describes how pixels from this Raster are stored in the DataBuffer.
        ''' </summary>
        ''' <remarks></remarks>
        Protected sampleModel As SampleModel

        ''' <summary>
        ''' The X translation from the coordinate space of the Raster's SampleModel to that of the Raster.
        ''' </summary>
        ''' <remarks></remarks>
        Protected sampleModelTranslateX As Integer

        ''' <summary>
        ''' The Y translation from the coordinate space of the Raster's SampleModel to that of the Raster.
        ''' </summary>
        ''' <remarks></remarks>
        Protected sampleModelTranslateY As Integer

        ''' <summary>
        ''' The width of this Raster.
        ''' </summary>
        ''' <remarks></remarks>
        Protected width As Integer

        ''' <summary>
        ''' Constructs a Raster with the given SampleModel and DataBuffer.
        ''' </summary>
        ''' <param name="sampleModel">The SampleModel that specifies the layout</param>
        ''' <param name="dataBuffer">The DataBuffer that contains the image data</param>
        ''' <param name="origin">The Point that specifies the origin</param>
        ''' <remarks></remarks>
        Protected Sub New(ByVal sampleModel As SampleModel, ByVal dataBuffer As DataBuffer, ByVal origin As Point)
            Me.sampleModel = sampleModel
            Me.dataBuffer = dataBuffer
            Me.minX = origin.X
            Me.minY = origin.Y
        End Sub

        ''' <summary>
        ''' Constructs a Raster with the given SampleModel, DataBuffer, and parent.
        ''' </summary>
        ''' <param name="sampleModel"></param>
        ''' <param name="dataBuffer"></param>
        ''' <param name="aRegion"></param>
        ''' <param name="sampleModelTranslate"></param>
        ''' <param name="parent"></param>
        ''' <remarks></remarks>
        Protected Sub New(ByVal sampleModel As SampleModel, ByVal dataBuffer As DataBuffer, ByVal aRegion As Rectangle, ByVal sampleModelTranslate As Point, ByVal parent As Raster)
            Me.sampleModel = sampleModel
            Me.dataBuffer = dataBuffer
            Me.sampleModelTranslateX = sampleModelTranslate.X
            Me.sampleModelTranslateY = sampleModelTranslate.Y
            Me.minX = aRegion.Left
            Me.minY = aRegion.Top
            Me.width = aRegion.Width
            Me.height = aRegion.Height
        End Sub

        ''' <summary>
        ''' Constructs a Raster with the given SampleModel.
        ''' </summary>
        ''' <param name="sampleModel"></param>
        ''' <param name="origin"></param>
        ''' <remarks></remarks>
        Protected Sub New(ByVal sampleModel As SampleModel, ByVal origin As Point)
            Me.sampleModel = sampleModel
            Me.minX = origin.X
            Me.minY = origin.Y
        End Sub

        ''' <summary>
        ''' Returns a new Raster which shares all or part of this Raster's DataBuffer.
        ''' </summary>
        ''' <param name="parentX"></param>
        ''' <param name="parentY"></param>
        ''' <param name="width"></param>
        ''' <param name="height"></param>
        ''' <param name="childMinX"></param>
        ''' <param name="childMinY"></param>
        ''' <param name="bandList"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function createChild(ByVal parentX As Integer, ByVal parentY As Integer, ByVal width As Integer, ByVal height As Integer, ByVal childMinX As Integer, ByVal childMinY As Integer, ByVal bandList() As Integer) As Raster
            Throw New NotImplementedException
        End Function

        ''' <summary>
        ''' Create a compatible WritableRaster the same size as this Raster with the same SampleModel and a new initialized DataBuffer.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function createCompatibleWritableRaster() As WritableRaster
            Throw New NotImplementedException
        End Function

        ''' <summary>
        ''' Create a compatible WritableRaster with the specified size, a new SampleModel, and a new initialized DataBuffer.
        ''' </summary>
        ''' <param name="w"></param>
        ''' <param name="h"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function createCompatibleWritableRaster(ByVal w As Integer, ByVal h As Integer) As WritableRaster
            Throw New NotImplementedException
        End Function

        ''' <summary>
        ''' Create a compatible WritableRaster with the specified location (minX, minY) and size (width, height), a new SampleModel, and a new initialized DataBuffer.
        ''' </summary>
        ''' <param name="x"></param>
        ''' <param name="y"></param>
        ''' <param name="w"></param>
        ''' <param name="h"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function createCompatibleWritableRaster(ByVal x As Integer, ByVal y As Integer, ByVal w As Integer, ByVal h As Integer) As WritableRaster
            Throw New NotImplementedException
        End Function

        ''' <summary>
        ''' Create a compatible WritableRaster with location (minX, minY) and size (width, height) specified by rect, a new SampleModel, and a new initialized DataBuffer.
        ''' </summary>
        ''' <param name="rect"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function createCompatibleWritableRaster(ByVal rect As Rectangle) As WritableRaster
            Throw New NotImplementedException
        End Function

        Public Overridable Function getDataBuffer() As DataBuffer
            Return Me.dataBuffer
        End Function

        Public Overridable Function getWidth() As Integer
            Return Me.width
        End Function

        Public Overridable Function getHeight() As Integer
            Return Me.height
        End Function

        Public Overridable Function getPixel(j As Integer, i As Integer, p3() As Integer) As Integer()
            Throw New NotImplementedException
        End Function

        Public Overridable Function getPixel(j As Integer, i As Integer, p3() As Double) As Double()
            Throw New NotImplementedException
        End Function

        Public Overridable Function getPixel(j As Integer, i As Integer, p3() As Single) As Single()
            Throw New NotImplementedException
        End Function


#Region "Shared"

        ''' <summary>
        ''' Creates a Raster based on a BandedSampleModel with the specified DataBuffer, width, height, scanline stride, bank indices, and band offsets.
        ''' </summary>
        ''' <param name="dataBuffer"></param>
        ''' <param name="w"></param>
        ''' <param name="h"></param>
        ''' <param name="scanlineStride"></param>
        ''' <param name="bankIndices"></param>
        ''' <param name="bandOffsets"></param>
        ''' <param name="location"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function createBandedRaster(ByVal dataBuffer As DataBuffer, ByVal w As Integer, ByVal h As Integer, ByVal scanlineStride As Integer, ByVal bankIndices() As Integer, ByVal bandOffsets() As Integer, ByVal location As Point) As WritableRaster
            Throw New NotImplementedException
        End Function

        ''' <summary>
        ''' Creates a Raster based on a BandedSampleModel with the specified data type, width, height, scanline stride, bank indices and band offsets.
        ''' </summary>
        ''' <param name="dataType"></param>
        ''' <param name="w"></param>
        ''' <param name="h"></param>
        ''' <param name="scanlineStride"></param>
        ''' <param name="bankIndices"></param>
        ''' <param name="bandOffsets"></param>
        ''' <param name="location"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function createBandedRaster(ByVal dataType As Integer, ByVal w As Integer, ByVal h As Integer, ByVal scanlineStride As Integer, ByVal bankIndices() As Integer, ByVal bandOffsets() As Integer, ByVal location As Point) As WritableRaster
            Throw New NotImplementedException
        End Function

        ''' <summary>
        ''' Creates a Raster based on a BandedSampleModel with the specified data type, width, height, and number of bands.
        ''' </summary>
        ''' <param name="dataType"></param>
        ''' <param name="w"></param>
        ''' <param name="h"></param>
        ''' <param name="bands"></param>
        ''' <param name="location"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function createBandedRaster(ByVal dataType As Integer, ByVal w As Integer, ByVal h As Integer, ByVal bands As Integer, ByVal location As Point) As WritableRaster
            Throw New NotImplementedException
        End Function

        ''' <summary>
        ''' Creates a Raster based on a PixelInterleavedSampleModel with the specified DataBuffer, width, height, scanline stride, pixel stride, and band offsets.
        ''' </summary>
        ''' <param name="dataBuffer"></param>
        ''' <param name="w"></param>
        ''' <param name="h"></param>
        ''' <param name="scanlineStride"></param>
        ''' <param name="pixelStride"></param>
        ''' <param name="bandOffsets"></param>
        ''' <param name="location"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function createInterleavedRaster(ByVal dataBuffer As DataBuffer, ByVal w As Integer, ByVal h As Integer, ByVal scanlineStride As Integer, ByVal pixelStride As Integer, ByVal bandOffsets() As Integer, ByVal location As Point) As WritableRaster
            Throw New NotImplementedException
        End Function

        ''' <summary>
        ''' Creates a Raster based on a PixelInterleavedSampleModel with the specified data type, width, height, scanline stride, pixel stride, and band offsets.
        ''' </summary>
        ''' <param name="dataType"></param>
        ''' <param name="w"></param>
        ''' <param name="h"></param>
        ''' <param name="scanlineStride"></param>
        ''' <param name="pixelStride"></param>
        ''' <param name="bandOffsets"></param>
        ''' <param name="location"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function createInterleavedRaster(ByVal dataType As Integer, ByVal w As Integer, ByVal h As Integer, ByVal scanlineStride As Integer, ByVal pixelStride As Integer, ByVal bandOffsets() As Integer, ByVal location As Point) As WritableRaster
            Throw New NotImplementedException
        End Function

        ''' <summary>
        ''' Creates a Raster based on a PixelInterleavedSampleModel with the specified data type, width, height, and number of bands.
        ''' </summary>
        ''' <param name="dataType"></param>
        ''' <param name="w"></param>
        ''' <param name="h"></param>
        ''' <param name="bands"></param>
        ''' <param name="location"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function createInterleavedRaster(ByVal dataType As Integer, ByVal w As Integer, ByVal h As Integer, ByVal bands As Integer, ByVal location As Point) As WritableRaster
            Throw New NotImplementedException
        End Function

        ''' <summary>
        ''' Creates a Raster based on a SinglePixelPackedSampleModel with the specified DataBuffer, width, height, scanline stride, and band masks.
        ''' </summary>
        ''' <param name="dataBuffer"></param>
        ''' <param name="w"></param>
        ''' <param name="h"></param>
        ''' <param name="scanlineStride"></param>
        ''' <param name="bandMasks"></param>
        ''' <param name="location"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function createPackedRaster(ByVal dataBuffer As DataBuffer, ByVal w As Integer, ByVal h As Integer, ByVal scanlineStride As Integer, ByVal bandMasks() As Integer, ByVal location As Point) As WritableRaster
            Throw New NotImplementedException
        End Function

        ''' <summary>
        ''' Creates a Raster based on a MultiPixelPackedSampleModel with the specified DataBuffer, width, height, and bits per pixel.
        ''' </summary>
        ''' <param name="dataBuffer"></param>
        ''' <param name="w"></param>
        ''' <param name="h"></param>
        ''' <param name="bitsPerPixel"></param>
        ''' <param name="location"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function createPackedRaster(ByVal dataBuffer As DataBuffer, ByVal w As Integer, ByVal h As Integer, ByVal bitsPerPixel As Integer, ByVal location As Point) As WritableRaster
            Throw New NotImplementedException
        End Function

        ''' <summary>
        ''' Creates a Raster based on a SinglePixelPackedSampleModel with the specified data type, width, height, and band masks.
        ''' </summary>
        ''' <param name="dataType"></param>
        ''' <param name="w"></param>
        ''' <param name="h"></param>
        ''' <param name="bandMasks"></param>
        ''' <param name="location"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function createPackedRaster(ByVal dataType As Integer, ByVal w As Integer, ByVal h As Integer, ByVal bandMasks() As Integer, ByVal location As Point) As WritableRaster
            Throw New NotImplementedException
        End Function

        ''' <summary>
        ''' Creates a Raster based on a packed SampleModel with the specified data type, width, height, number of bands, and bits per band.
        ''' </summary>
        ''' <param name="dataType"></param>
        ''' <param name="w"></param>
        ''' <param name="h"></param>
        ''' <param name="bands"></param>
        ''' <param name="bitsPerBand"></param>
        ''' <param name="location"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function createPackedRaster(ByVal dataType As Integer, ByVal w As Integer, ByVal h As Integer, ByVal bands As Integer, ByVal bitsPerBand As Integer, ByVal location As Point) As WritableRaster
            Throw New NotImplementedException
        End Function


        ''' <summary>
        ''' Creates a Raster with the specified SampleModel and DataBuffer.
        ''' </summary>
        ''' <param name="sm"></param>
        ''' <param name="db"></param>
        ''' <param name="location"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function createRaster(ByVal sm As SampleModel, ByVal db As DataBuffer, ByVal location As Point) As Raster
            Throw New NotImplementedException
        End Function


#End Region
    End Class

End Namespace
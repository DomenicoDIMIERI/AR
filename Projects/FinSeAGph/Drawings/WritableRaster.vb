Namespace Drawings

    ''' <summary>
    ''' This class extends Raster to provide pixel writing capabilities. Refer to the class comment for Raster for descriptions of how a Raster stores pixels.
    ''' The constructors of this class are protected. To instantiate a WritableRaster, use one of the createWritableRaster factory methods in the Raster class.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class WritableRaster
        Inherits Raster

        ''' <summary>
        ''' Constructs a WritableRaster with the given SampleModel and DataBuffer.
        ''' </summary>
        ''' <param name="sampleModel"></param>
        ''' <param name="dataBuffer"></param>
        ''' <param name="origin"></param>
        ''' <remarks></remarks>
        Protected Sub New(ByVal sampleModel As SampleModel, ByVal dataBuffer As DataBuffer, ByVal origin As Point)
            MyBase.New(sampleModel, dataBuffer, origin)
        End Sub

        ''' <summary>
        ''' Constructs a WritableRaster with the given SampleModel, DataBuffer, and parent.
        ''' </summary>
        ''' <param name="sampleModel"></param>
        ''' <param name="dataBuffer"></param>
        ''' <param name="aRegion"></param>
        ''' <param name="sampleModelTranslate"></param>
        ''' <param name="parent"></param>
        ''' <remarks></remarks>
        Protected Sub New(ByVal sampleModel As SampleModel, ByVal dataBuffer As DataBuffer, ByVal aRegion As Rectangle, ByVal sampleModelTranslate As Point, ByVal parent As WritableRaster)
            MyBase.New(sampleModel, dataBuffer, aRegion, sampleModelTranslate, parent)
        End Sub

        ''' <summary>
        ''' Constructs a WritableRaster with the given SampleModel.
        ''' </summary>
        ''' <param name="sampleModel"></param>
        ''' <param name="origin"></param>
        ''' <remarks></remarks>
        Protected Sub New(ByVal sampleModel As SampleModel, ByVal origin As Point)
            MyBase.New(sampleModel, origin)
        End Sub

   

        Sub setPixels(p1 As Integer, p2 As Integer, w As Integer, h As Integer, data As Integer())
            Throw New NotImplementedException
        End Sub

      


#If 0 Then

        Modifier and Type 	Method and Description
WritableRaster 	createWritableChild(int parentX, int parentY, int w, int h, int childMinX, int childMinY, int[] bandList)
Returns a new WritableRaster which shares all or part of this WritableRaster's DataBuffer.
WritableRaster 	createWritableTranslatedChild(int childMinX, int childMinY)
Create a WritableRaster with the same size, SampleModel and DataBuffer as this one, but with a different location.
WritableRaster 	getWritableParent()
Returns the parent WritableRaster (if any) of this WritableRaster, or else null.
void 	setDataElements(int x, int y, int w, int h, Object inData)
Sets the data for a rectangle of pixels from a primitive array of type TransferType.
void 	setDataElements(int x, int y, Object inData)
Sets the data for a single pixel from a primitive array of type TransferType.
void 	setDataElements(int x, int y, Raster inRaster)
Sets the data for a rectangle of pixels from an input Raster.
void 	setPixel(int x, int y, double[] dArray)
Sets a pixel in the DataBuffer using a double array of samples for input.
void 	setPixel(int x, int y, float[] fArray)
Sets a pixel in the DataBuffer using a float array of samples for input.
void 	setPixel(int x, int y, int[] iArray)
Sets a pixel in the DataBuffer using an int array of samples for input.
void 	setPixels(int x, int y, int w, int h, double[] dArray)
Sets all samples for a rectangle of pixels from a double array containing one sample per array element.
void 	setPixels(int x, int y, int w, int h, float[] fArray)
Sets all samples for a rectangle of pixels from a float array containing one sample per array element.
void 	setPixels(int x, int y, int w, int h, int[] iArray)
Sets all samples for a rectangle of pixels from an int array containing one sample per array element.
void 	setRect(int dx, int dy, Raster srcRaster)
Copies pixels from Raster srcRaster to this WritableRaster.
void 	setRect(Raster srcRaster)
Copies pixels from Raster srcRaster to this WritableRaster.
void 	setSample(int x, int y, int b, double s)
Sets a sample in the specified band for the pixel located at (x,y) in the DataBuffer using a double for input.
void 	setSample(int x, int y, int b, float s)
Sets a sample in the specified band for the pixel located at (x,y) in the DataBuffer using a float for input.
void 	setSample(int x, int y, int b, int s)
Sets a sample in the specified band for the pixel located at (x,y) in the DataBuffer using an int for input.
void 	setSamples(int x, int y, int w, int h, int b, double[] dArray)
Sets the samples in the specified band for the specified rectangle of pixels from a double array containing one sample per array element.
void 	setSamples(int x, int y, int w, int h, int b, float[] fArray)
Sets the samples in the specified band for the specified rectangle of pixels from a float array containing one sample per array element.
void 	setSamples(int x, int y, int w, int h, int b, int[] iArray)
Sets the samples in the specified band for the specified rectangle of pixels from an int array containing one sample per array element.

#End If

    End Class

End Namespace
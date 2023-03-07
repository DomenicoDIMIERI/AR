Imports FinSeA.Drawings
Imports FinSeA.Io
Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.color
Imports FinSeA.org.apache.pdfbox.util
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.pdmodel.common.function

Namespace org.apache.pdfbox.pdmodel.graphics.xobject

    '/**
    ' * This class contains a PixelMap Image.
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @author mathiak
    ' * @version $Revision: 1.10 $
    ' */
    Public Class PDPixelMap
        Inherits PDXObjectImage
        '/**
        ' * Log instance.
        ' */
        'private static final Log LOG = LogFactory.getLog(PDPixelMap.class);

        Private image As BufferedImage = Nothing

        Private Const PNG As String = "png"

        '/**
        ' * Standard constructor. Basically does nothing.
        ' * @param pdStream The stream that holds the pixel map.
        ' */
        Public Sub New(ByVal pdStream As PDStream)
            MyBase.New(pdStream, PNG)
        End Sub

        '/**
        ' * Construct a pixel map image from an AWT image.
        ' * 
        ' * 
        ' * @param doc The PDF document to embed the image in.
        ' * @param bi The image to read data from.
        ' *
        ' * @throws IOException If there is an error while embedding this image.
        ' */
        Public Sub New(ByVal doc As PDDocument, ByVal bi As BufferedImage) 'throws IOException
            MyBase.New(doc, PNG)
            createImageStream(doc, bi)
        End Sub

        Private Sub createImageStream(ByVal doc As PDDocument, ByVal bi As BufferedImage) 'throws IOException
            Dim alphaImage As BufferedImage = Nothing
            Dim rgbImage As BufferedImage = Nothing
            Dim width As Integer = bi.getWidth()
            Dim height As Integer = bi.getHeight()
            If (bi.getColorModel().hasAlpha()) Then
                ' extract the alpha information
                Dim alphaRaster As WritableRaster = bi.getAlphaRaster()
                Dim cm As ColorModel = New ComponentColorModel(ColorSpace.getInstance(ColorSpace.CS_GRAY), False, False, Transparency.Mode.OPAQUE, DataBuffer.TYPE_BYTE)
                alphaImage = New BufferedImage(cm, alphaRaster, False, Nothing)
                ' create a RGB image without alpha
                rgbImage = New BufferedImage(width, height, BufferedImage.TYPE_3BYTE_BGR)
                Dim g As Graphics2D = rgbImage.createGraphics()
                g.setComposite(AlphaComposite.Src)
                g.drawImage(bi, 0, 0, Nothing)
            Else
                rgbImage = bi
            End If
            Dim os As OutputStream = Nothing
            Try
                Dim numberOfComponents As Integer = rgbImage.getColorModel().getNumComponents()
                If (numberOfComponents = 3) Then
                    setColorSpace(PDDeviceRGB.INSTANCE)
                Else
                    If (numberOfComponents = 1) Then
                        setColorSpace(New PDDeviceGray())
                    Else
                        Throw New Exception("Stato non valido")
                    End If
                End If
                Dim outData() As Byte = Array.CreateInstance(GetType(Byte), width * height * numberOfComponents)
                rgbImage.getData().getDataElements(0, 0, width, height, outData)
                ' add FlateDecode compression
                getPDStream().addCompression()
                os = getCOSStream().createUnfilteredStream()
                os.Write(outData)

                Dim dic As COSDictionary = getCOSStream()
                dic.setItem(COSName.FILTER, COSName.FLATE_DECODE)
                dic.setItem(COSName.SUBTYPE, COSName.IMAGE)
                dic.setItem(COSName.TYPE, COSName.XOBJECT)
                If (alphaImage IsNot Nothing) Then
                    Dim smask As New PDPixelMap(doc, alphaImage)
                    dic.setItem(COSName.SMASK, smask)
                End If
                setBitsPerComponent(8)
                setHeight(height)
                setWidth(width)
            Finally
                If (os IsNot Nothing) Then
                    os.Close()
                End If
            End Try
        End Sub


        '/**
        ' * Returns a {@link java.awt.image.BufferedImage} of the COSStream
        ' * set in the constructor or null if the COSStream could not be encoded.
        ' *
        ' * @return {@inheritDoc}
        ' *
        ' * @throws IOException {@inheritDoc}
        ' */
        Public Overrides Function getRGBImage() As BufferedImage ' throws IOException
            If (image IsNot Nothing) Then
                Return image
            End If

            Try
                Dim array() As Byte = getPDStream().getByteArray()
                If (array.Length = 0) Then
                    LOG.error("Something went wrong ... the pixelmap doesn't contain any data.")
                    Return Nothing
                End If
                Dim width As Integer = getWidth()
                Dim height As Integer = getHeight()
                Dim bpc As Integer = getBitsPerComponent()

                Dim colorspace As PDColorSpace = getColorSpace()
                If (colorspace Is Nothing) Then
                    LOG.error("getColorSpace() returned NULL.")
                    Return Nothing
                End If
                ' Get the ColorModel right
                Dim cm As ColorModel = Nothing
                If (TypeOf (colorspace) Is PDIndexed) Then
                    Dim csIndexed As PDIndexed = colorspace
                    Dim maskArray As COSBase = getMask()
                    If (maskArray IsNot Nothing AndAlso TypeOf (maskArray) Is COSArray) Then
                        cm = csIndexed.createColorModel(bpc, DirectCast(maskArray, COSArray).getInt(0))
                    Else
                        cm = csIndexed.createColorModel(bpc)
                    End If
                ElseIf (TypeOf (colorspace) Is PDSeparation) Then
                    Dim csSeparation As PDSeparation = colorspace
                    Dim numberOfComponents As Integer = csSeparation.getAlternateColorSpace().getNumberOfComponents()
                    Dim tintTransformFunc As PDFunction = csSeparation.getTintTransform()
                    Dim decode As COSArray = getDecode()
                    ' we have to invert the tint-values,
                    ' if the Decode array exists and consists of (1,0)
                    Dim invert As Boolean = decode IsNot Nothing AndAlso decode.getInt(0) = 1
                    ' TODO add interpolation for other decode values then 1,0
                    Dim maxValue As Integer = Math.Pow(2, bpc) - 1
                    ' destination array
                    Dim mappedData() As Byte = System.Array.CreateInstance(GetType(Byte), width * height * numberOfComponents)
                    Dim rowLength As Integer = width * numberOfComponents
                    Dim input As Single() = System.Array.CreateInstance(GetType(Single), 1)
                    For i As Integer = 0 To height - 1
                        Dim rowOffset As Integer = i * rowLength
                        For j As Integer = 0 To width - 1
                            ' scale tint values to a range of 0...1
                            Dim value As Integer = (array(i * width + j) + 256) Mod 256
                            If (invert) Then
                                input(0) = 1 - (value / maxValue)
                            Else
                                input(0) = value / maxValue
                            End If
                            Dim mappedColor As Single() = tintTransformFunc.eval(input)
                            Dim columnOffset As Integer = j * numberOfComponents
                            For k As Integer = 0 To numberOfComponents - 1
                                ' redo scaling for every single color value 
                                Dim mappedValue As Single = mappedColor(k)
                                mappedData(rowOffset + columnOffset + k) = (mappedValue * maxValue)
                            Next
                        Next
                    Next
                    array = mappedData
                    cm = colorspace.createColorModel(bpc)
                ElseIf (bpc = 1) Then
                    Dim map() As Byte = Nothing
                    If (TypeOf (colorspace) Is PDDeviceGray) Then
                        Dim decode As COSArray = getDecode()
                        ' we have to invert the b/w-values,
                        ' if the Decode array exists and consists of (1,0)
                        If (decode IsNot Nothing AndAlso decode.getInt(0) = 1) Then
                            map = {&HFF}
                        Else
                            map = {&H0, &HFF}
                        End If
                    ElseIf (TypeOf (colorspace) Is PDICCBased) Then
                        If (DirectCast(colorspace, PDICCBased).getNumberOfComponents() = 1) Then
                            map = {&HFF}
                        Else
                            map = {&H0, &HFF}
                        End If
                    Else
                        map = {&H0, &HFF}
                    End If
                    cm = New IndexColorModel(bpc, map.Length, map, map, map, Transparency.Mode.OPAQUE)
                Else
                    If (TypeOf (colorspace) Is PDICCBased) Then
                        If (DirectCast(colorspace, PDICCBased).getNumberOfComponents() = 1) Then
                            Dim map() As Byte = {&HFF}
                            cm = New IndexColorModel(bpc, 1, map, map, map, Transparency.Mode.OPAQUE)
                        Else
                            cm = colorspace.createColorModel(bpc)
                        End If
                    Else
                        cm = colorspace.createColorModel(bpc)
                    End If
                End If

                LOG.debug("ColorModel: " & cm.ToString())
                Dim raster As WritableRaster = cm.createCompatibleWritableRaster(width, height)
                Dim buffer As DataBufferByte = raster.getDataBuffer()
                Dim bufferData() As Byte = buffer.getData()

                System.Array.Copy(array, 0, bufferData, 0, Math.Min(array.Length, bufferData.Length))
                image = New BufferedImage(cm, raster, False, Nothing)

                Return applyMasks(image)
            Catch exception As Exception
                LOG.error(exception.Message, exception)
                'A NULL return is caught in pagedrawer.Invoke.process() so don't re-throw.
                'Returning the NULL falls through to Phillip Koch's TODO section.
                Return Nothing
            End Try
        End Function



        '/**
        '    * Writes the image as .png.
        '    *
        '    * {@inheritDoc}
        '    */
        Public Overrides Sub write2OutputStream(ByVal out As OutputStream) 'throws IOException
            Dim img As BufferedImage = getRGBImage()
            If (img IsNot Nothing) Then
                ImageIOUtil.writeImage(img, PNG, out)
            End If
        End Sub

        '/**
        ' * DecodeParms is an optional parameter for filters.
        ' *
        ' * It is provided if any of the filters has nondefault parameters. If there
        ' * is only one filter it is a dictionary, if there are multiple filters it
        ' * is an array with an entry for each filter. An array entry can hold a null
        ' * value if only the default values are used or a dictionary with
        ' * parameters.
        ' *
        ' * @return The decoding parameters.
        ' *
        ' * @deprecated Use {@link org.apache.pdfbox.pdmodel.common.PDStream#getDecodeParms() } instead
        ' */
        Public Function getDecodeParams() As COSDictionary
            Dim decodeParms As COSBase = getCOSStream().getDictionaryObject(COSName.DECODE_PARMS)
            If (decodeParms IsNot Nothing) Then
                If (TypeOf (decodeParms) Is COSDictionary) Then
                    Return decodeParms
                ElseIf (TypeOf (decodeParms) Is COSArray) Then
                    ' not implemented yet, which index should we use?
                    Return Nothing '//(COSDictionary)((COSArray)decodeParms).get(0);
                Else
                    Return Nothing
                End If
            End If
            Return Nothing
        End Function

        '/**
        ' * A code that selects the predictor algorithm.
        ' *
        ' * <ul>
        ' * <li>1 No prediction (the default value)
        ' * <li>2 TIFF Predictor 2
        ' * <li>10 PNG prediction (on encoding, PNG None on all rows)
        ' * <li>11 PNG prediction (on encoding, PNG Sub on all rows)
        ' * <li>12 PNG prediction (on encoding, PNG Up on all rows)
        ' * <li>13 PNG prediction (on encoding, PNG Average on all rows)
        ' * <li>14 PNG prediction (on encoding, PNG Path on all rows)
        ' * <li>15 PNG prediction (on encoding, PNG optimum)
        ' * </ul>
        ' *
        ' * Default value: 1.
        ' *
        ' * @return predictor algorithm code
        ' * 
        ' * @deprecated see {@link org.apache.pdfbox.filter.FlateFilter}
        ' * 
        ' */
        Public Function getPredictor() As Integer
            Dim decodeParms As COSDictionary = getDecodeParams()
            If (decodeParms IsNot Nothing) Then
                Dim i As Integer = decodeParms.getInt(COSName.PREDICTOR)
                If (i <> -1) Then
                    Return i
                End If
            End If
            Return 1
        End Function


    End Class

End Namespace

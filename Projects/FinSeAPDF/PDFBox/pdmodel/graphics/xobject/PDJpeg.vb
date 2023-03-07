Imports FinSeA.Drawings
Imports FinSeA.Io
'import javax.imageio.ImageIO;
'import javax.imageio.IIOException;
'import javax.imageio.ImageReader;
'import javax.imageio.metadata.IIOMetadata;
'import javax.imageio.stream.ImageInputStream;

Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.pdmodel.common.function
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.color
Imports FinSeA.org.apache.pdfbox.util
Imports FinSeA.Exceptions

'import org.w3c.dom.NamedNodeMap;
'import org.w3c.dom.Node;
'import org.w3c.dom.NodeList;

Namespace org.apache.pdfbox.pdmodel.graphics.xobject


    '/**
    ' * An image class for JPegs.
    ' *
    ' * @author mathiak
    ' * @version $Revision: 1.5 $
    ' */
    Public Class PDJpeg
        Inherits PDXObjectImage

        Private image As BufferedImage = Nothing

        Private Const JPG As String = "jpg"

        Private Shared DCT_FILTERS As List(Of String) = New ArrayList(Of String)()

        Private Const DEFAULT_COMPRESSION_LEVEL As Single = 0.75F

        Shared Sub New()
            DCT_FILTERS.add(COSName.DCT_DECODE.getName())
            DCT_FILTERS.add(COSName.DCT_DECODE_ABBREVIATION.getName())
        End Sub

        '/**
        ' * Standard constructor.
        ' *
        ' * @param jpeg The COSStream from which to extract the JPeg
        ' */
        Public Sub New(ByVal jpeg As PDStream)
            MyBase.New(jpeg, JPG)
        End Sub

        '/**
        ' * Construct from a stream.
        ' *
        ' * @param doc The document to create the image as part of.
        ' * @param is The stream that contains the jpeg data.
        ' * @throws IOException If there is an error reading the jpeg data.
        ' */
        Public Sub New(ByVal doc As PDDocument, ByVal [is] As InputStream) 'throws IOException
            MyBase.New(New PDStream(doc, [is], True), JPG)
            Dim dic As COSDictionary = getCOSStream()
            dic.setItem(COSName.FILTER, COSName.DCT_DECODE)
            dic.setItem(COSName.SUBTYPE, COSName.IMAGE)
            dic.setItem(COSName.TYPE, COSName.XOBJECT)

            getRGBImage()
            If (image IsNot Nothing) Then
                setBitsPerComponent(8)
                setColorSpace(PDDeviceRGB.INSTANCE)
                setHeight(image.getHeight())
                setWidth(image.getWidth())
            End If

        End Sub

        '/**
        ' * Construct from a buffered image.
        ' * The default compression level of 0.75 will be used.
        ' *
        ' * @param doc The document to create the image as part of.
        ' * @param bi The image to convert to a jpeg
        ' * @throws IOException If there is an error processing the jpeg data.
        ' */
        Public Sub New(ByVal doc As PDDocument, ByVal bi As BufferedImage) 'throws IOException
            MyBase.New(New PDStream(doc), JPG)
            createImageStream(doc, bi, DEFAULT_COMPRESSION_LEVEL)
        End Sub

        '/**
        ' * Construct from a buffered image.
        ' *
        ' * @param doc The document to create the image as part of.
        ' * @param bi The image to convert to a jpeg
        ' * @param compressionQuality The quality level which is used to compress the image
        ' * @throws IOException If there is an error processing the jpeg data.
        ' */
        Public Sub New(ByVal doc As PDDocument, ByVal bi As BufferedImage, ByVal compressionQuality As Single) 'throws IOException
            MyBase.New(New PDStream(doc), JPG)
            createImageStream(doc, bi, compressionQuality)
        End Sub

        Private Sub createImageStream(ByVal doc As PDDocument, ByVal bi As BufferedImage, ByVal compressionQuality As Single) 'throws IOException
            Dim alpha As BufferedImage = Nothing
            If (bi.getColorModel().hasAlpha()) Then
                ' extract the alpha information
                Dim alphaRaster As WritableRaster = bi.getAlphaRaster()
                Dim cm As ColorModel = New ComponentColorModel(ColorSpace.getInstance(ColorSpace.CS_GRAY), False, False, Transparency.Mode.OPAQUE, DataBuffer.TYPE_BYTE)
                alpha = New BufferedImage(cm, alphaRaster, False, Nothing)
                ' create a RGB image without alpha
                image = New BufferedImage(bi.getWidth(), bi.getHeight(), BufferedImage.TYPE_INT_RGB)
                Dim g As Graphics2D = image.createGraphics()
                g.setComposite(AlphaComposite.Src)
                g.drawImage(bi, 0, 0, Nothing)
                bi = image
            End If

            Dim os As OutputStream = getCOSStream().createFilteredStream()
            Try
                ImageIOUtil.writeImage(bi, JPG, os, ImageIOUtil.DEFAULT_SCREEN_RESOLUTION, compressionQuality)

                Dim dic As COSDictionary = getCOSStream()
                dic.setItem(COSName.FILTER, COSName.DCT_DECODE)
                dic.setItem(COSName.SUBTYPE, COSName.IMAGE)
                dic.setItem(COSName.TYPE, COSName.XOBJECT)
                Dim alphaPdImage As PDXObjectImage = Nothing
                If (alpha IsNot Nothing) Then
                    alphaPdImage = New PDJpeg(doc, alpha, compressionQuality)
                    dic.setItem(COSName.SMASK, alphaPdImage)
                End If
                setBitsPerComponent(8)
                If (bi.getColorModel().getNumComponents() = 3) Then
                    setColorSpace(PDDeviceRGB.INSTANCE)
                Else
                    If (bi.getColorModel().getNumComponents() = 1) Then
                        setColorSpace(New PDDeviceGray())
                    Else
                        Throw New Exception() 'IllegalStateException
                    End If
                End If
                setHeight(bi.getHeight())
                setWidth(bi.getWidth())
            Finally
                os.Close()
            End Try
        End Sub

        '/**
        ' * Returns an image of the JPeg, or null if JPegs are not supported. (They should be. )
        ' * {@inheritDoc}
        ' */
        Public Overrides Function getRGBImage() As BufferedImage ' throws IOException
            If (image IsNot Nothing) Then
                ' use the cached image
                Return image
            End If

            Dim bi As BufferedImage = Nothing
            Dim readError As Boolean = False
            Dim os As ByteArrayOutputStream = New ByteArrayOutputStream()
            removeAllFiltersButDCT(os)
            os.Close()
            Dim img() As Byte = os.toByteArray()

            Dim cs As PDColorSpace = getColorSpace()
            Try
                If (TypeOf (cs) Is PDDeviceCMYK OrElse (TypeOf (cs) Is PDICCBased AndAlso cs.getNumberOfComponents() = 4)) Then
                    ' JPEGs may contain CMYK, YCbCr or YCCK decoded image data
                    Dim transform As Integer = getApp14AdobeTransform(img)
                    ' create BufferedImage based on the converted color values
                    If (transform = 0) Then
                        bi = convertCMYK2RGB(readImage(img), cs)
                    ElseIf (transform = 1) Then
                        ' TODO YCbCr
                    ElseIf (transform = 2) Then
                        bi = convertYCCK2RGB(readImage(img))
                    End If
                ElseIf (TypeOf (cs) Is PDSeparation) Then
                    ' create BufferedImage based on the converted color values
                    bi = processTintTransformation(readImage(img), DirectCast(cs, PDSeparation).getTintTransform(), cs.getJavaColorSpace())
                ElseIf (TypeOf (cs) Is PDDeviceN) Then
                    ' create BufferedImage based on the converted color values
                    bi = processTintTransformation(readImage(img), DirectCast(cs, PDDeviceN).getTintTransform(), cs.getJavaColorSpace())
                Else
                    Dim bai As ByteArrayInputStream = New ByteArrayInputStream(img)
                    bi = ImageIO.read(bai)
                    bai.Dispose()
                End If

            Catch exception As System.IO.IOException
                readError = True
            End Try
            ' 2. try to read jpeg again. some jpegs have some strange header containing
            '    "Adobe " at some place. so just replace the header with a valid jpeg header.
            ' TODO : not sure if it works for all cases
            If (bi Is Nothing AndAlso readError) Then
                Dim newImage() As Byte = replaceHeader(img)
                Dim bai As ByteArrayInputStream = New ByteArrayInputStream(newImage)
                bi = ImageIO.read(bai)
                bai.Dispose()
            End If
            ' If there is a 'soft mask' or 'mask' image then we use that as a transparency mask.
            image = applyMasks(bi)
            Return image
        End Function

        '/**
        ' * This writes the JPeg to out.
        ' * {@inheritDoc}
        ' */
        Public Overrides Sub write2OutputStream(ByVal out As OutputStream) 'throws IOException
            getRGBImage()
            If (image IsNot Nothing) Then
                ImageIOUtil.writeImage(image, JPG, out)
            End If
        End Sub

        Private Sub removeAllFiltersButDCT(ByVal out As OutputStream) 'throws IOException
            Dim data As InputStream = getPDStream().getPartiallyFilteredStream(DCT_FILTERS)
            Dim buf(1024 - 1) As Byte '= new byte[1024];
            Dim amountRead As Integer = -1
            amountRead = data.read(buf)
            While (amountRead > 0)
                out.Write(buf, 0, amountRead)
                amountRead = data.read(buf)
            End While
        End Sub

        Private Function getHeaderEndPos(ByVal imageAsBytes() As Byte) As Integer
            For i As Integer = 0 To imageAsBytes.Length - 1
                Dim b As Byte = imageAsBytes(i)
                If (b = &HDB) Then
                    ' TODO : check for ff db
                    Return i - 2
                End If
            Next
            Return 0
        End Function

        Private Function replaceHeader(ByVal imageAsBytes() As Byte) As Byte()
            ' get end position of wrong header respectively startposition of "real jpeg data"
            Dim pos As Integer = getHeaderEndPos(imageAsBytes)

            ' simple correct header
            Dim header() As Byte = {&HFF, &HD8, &HFF, &HE0, &H0, &H10, &H4A, &H46, &H49, &H46, &H0, &H1, &H1, &H1, &H0, &H60, &H0, &H60, &H0, &H0}

            ' concat
            Dim newImage() As Byte = Array.CreateInstance(GetType(Byte), imageAsBytes.Length - pos + header.Length - 1)
            Array.Copy(header, 0, newImage, 0, header.Length)
            Array.Copy(imageAsBytes, pos + 1, newImage, header.Length, imageAsBytes.Length - pos - 1)

            Return newImage
        End Function

        Private Function getApp14AdobeTransform(ByVal bytes As Byte()) As Integer
            Dim transformType As Integer = 0
            Dim reader As ImageReader = Nothing
            Dim input As ImageInputStream = Nothing
            Try
                input = ImageIO.createImageInputStream(New ByteArrayInputStream(bytes))
                Dim readers As Global.System.Collections.Generic.IEnumerator(Of ImageReader) = ImageIO.getImageReaders(input)

                'If (readers Is Nothing OrElse Not readers.hasNext()) Then
                '    input.close()
                '    Throw New RuntimeException("No ImageReaders found")
                'End If
                For Each reader In ImageIOUtil.getImageReaders(input)
                    reader = readers.Current
                    reader.setInput(input)
                    Dim meta As IIOMetadata = reader.getImageMetadata(0)
                    If (meta IsNot Nothing) Then
                        Dim tree As System.Xml.XmlNode = meta.getAsTree("javax_imageio_jpeg_image_1.0")
                        Dim children As System.Xml.XmlNodeList = tree.ChildNodes()
                        For i As Integer = 0 To children.Count - 1
                            Dim markerSequence As System.Xml.XmlNode = children.Item(i)
                            If ("markerSequence".Equals(markerSequence.Name)) Then 'getNodeName()
                                Dim markerSequenceChildren As System.Xml.XmlNodeList = markerSequence.ChildNodes
                                For j As Integer = 0 To markerSequenceChildren.Count - 1
                                    Dim child As System.Xml.XmlNode = markerSequenceChildren.Item(j)
                                    If ("app14Adobe".Equals(child.Name) AndAlso child.Attributes.Count > 0) Then
                                        Dim attribs As System.Xml.XmlNamedNodeMap = child.Attributes
                                        Dim transformNode As System.Xml.XmlNode = attribs.GetNamedItem("transform")
                                        transformType = Integer.Parse(transformNode.Value) 'getNodeValue()
                                        Exit For
                                    End If
                                Next
                            End If
                        Next
                    End If
                Next
            Catch exception As System.IO.IOException

            Finally
                If (reader IsNot Nothing) Then
                    reader.dispose()
                End If
            End Try
            Return transformType
        End Function

        Private Function readImage(ByVal bytes() As Byte) As Raster ' throws IOException 
            Dim input As ImageInputStream = ImageIO.createImageInputStream(New ByteArrayInputStream(bytes))
            Dim readers As Global.System.Collections.Generic.IEnumerator(Of ImageReader) = ImageIO.getImageReaders(input) 'Iterator<ImageReader> readers = ImageIO.getImageReaders(input);
            If (readers Is Nothing OrElse Not readers.MoveNext) Then
                input.Close()
                Throw New RuntimeException("No ImageReaders found")
            End If
            ' read the raster information only
            ' avoid to access the meta information
            Dim reader As ImageReader = readers.Current 'next()
            reader.setInput(input)
            Dim raster As Raster = reader.readRaster(0, reader.getDefaultReadParam())
            input.close()
            reader.dispose()
            Return raster
        End Function

        ' // CMYK jpegs are not supported by JAI, so that we have to do the conversion on our own
        Private Function convertCMYK2RGB(ByVal raster As Raster, ByVal colorspace As PDColorSpace) As BufferedImage
            ' create a java color space to be used for conversion
            Dim cs As ColorSpace = colorspace.getJavaColorSpace()
            Dim width As Integer = raster.getWidth()
            Dim height As Integer = raster.getHeight()
            Dim rgb() As Byte = Array.CreateInstance(GetType(Byte), width * height * 3)
            Dim rgbIndex As Integer = 0
            For i As Integer = 0 To height - 1
                For j As Integer = 0 To width - 1
                    ' get the source color values
                    Dim srcColorValues As Single() = raster.getPixel(j, i, DirectCast(Nothing, Single()))
                    ' convert values from 0..255 to 0..1
                    For k As Integer = 0 To 4 - 1
                        srcColorValues(k) /= 255.0F
                    Next
                    ' convert CMYK to RGB
                    Dim rgbValues As Single() = cs.toRGB(srcColorValues)
                    ' convert values from 0..1 to 0..255
                    For k As Integer = 0 To 3 - 1
                        rgb(rgbIndex + k) = (rgbValues(k) * 255)
                    Next
                    rgbIndex += 3
                Next
            Next
            Return createRGBBufferedImage(FinSeA.Drawings.ColorSpace.getInstance(FinSeA.Drawings.ColorSpace.CSEnum.CS_sRGB), rgb, width, height)
        End Function

        ' YCbCrK jpegs are not supported by JAI, so that we have to do the conversion on our own
        Private Function convertYCCK2RGB(ByVal raster As Raster) As BufferedImage ' throws IOException 
            Dim width As Integer = raster.getWidth()
            Dim height As Integer = raster.getHeight()
            Dim rgb() As Byte = Array.CreateInstance(GetType(Byte), width * height * 3)
            Dim rgbIndex As Integer = 0
            For i As Integer = 0 To height - 1
                For j As Integer = 0 To width - 1
                    Dim srcColorValues As Single() = raster.getPixel(j, i, DirectCast(Nothing, Single()))
                    Dim k As Single = srcColorValues(2)
                    Dim y As Single = srcColorValues(0)
                    Dim c1 As Single = srcColorValues(1)
                    Dim c2 As Single = srcColorValues(2)

                    Dim val As Double = y + 1.4019999999999999 * (c2 - 128) - k
                    rgb(rgbIndex) = IIf(val < 0.0, 0, IIf(val > 255.0, &HFF, (val + 0.5)))

                    val = y - 0.34414 * (c1 - 128) - 0.71414 * (c2 - 128) - k
                    rgb(rgbIndex + 1) = IIf(val < 0.0, 0, IIf(val > 255.0, &HFF, (val + 0.5)))

                    val = y + 1.772 * (c1 - 128) - k
                    rgb(rgbIndex + 2) = IIf(val < 0.0, 0, IIf(val > 255.0, &HFF, (val + 0.5)))

                    rgbIndex += 3
                Next
            Next
            Return createRGBBufferedImage(FinSeA.Drawings.ColorSpace.getInstance(FinSeA.Drawings.ColorSpace.CSEnum.CS_sRGB), rgb, width, height)
        End Function

        ' Separation and DeviceN colorspaces are using a tint transform function to convert color values 
        Private Function processTintTransformation(ByVal raster As Raster, ByVal [function] As PDFunction, ByVal colorspace As ColorSpace) As BufferedImage 'throws IOException
            Dim numberOfInputValues As Integer = [function].getNumberOfInputParameters()
            Dim numberOfOutputValues As Integer = [function].getNumberOfOutputParameters()
            Dim width As Integer = raster.getWidth()
            Dim height As Integer = raster.getHeight()
            Dim rgb() As Byte = Array.CreateInstance(GetType(Byte), width * height * numberOfOutputValues)
            Dim bufferIndex As Integer = 0
            For i As Integer = 0 To height - 1
                For j As Integer = 0 To width - 1
                    ' get the source color values
                    Dim srcColorValues As Single() = raster.getPixel(j, i, DirectCast(Nothing, Single()))
                    ' convert values from 0..255 to 0..1
                    For k As Integer = 0 To numberOfInputValues - 1
                        srcColorValues(k) /= 255.0F
                    Next
                    ' transform the color values using the tint function
                    Dim convertedValues As Single() = [function].eval(srcColorValues)
                    ' convert values from 0..1 to 0..255
                    For k As Integer = 0 To numberOfOutputValues - 1
                        rgb(bufferIndex + k) = (convertedValues(k) * 255)
                    Next
                    bufferIndex += numberOfOutputValues
                Next
            Next
            Return createRGBBufferedImage(colorspace, rgb, width, height)
        End Function

        Private Function createRGBBufferedImage(ByVal cs As ColorSpace, ByVal rgb() As Byte, ByVal width As Integer, ByVal height As Integer) As BufferedImage
            ' create a RGB color model
            Dim cm As ColorModel = New ComponentColorModel(cs, False, False, Transparency.Mode.OPAQUE, DataBuffer.TYPE_BYTE)
            ' create the target raster
            Dim writeableRaster As WritableRaster = cm.createCompatibleWritableRaster(width, height)
            ' get the data buffer of the raster
            Dim buffer As DataBufferByte = writeableRaster.getDataBuffer()
            Dim bufferData() As Byte = buffer.getData()
            ' copy all the converted data to the raster buffer
            Array.Copy(rgb, 0, bufferData, 0, rgb.Length)
            ' create an image using the converted color values
            Return New BufferedImage(cm, writeableRaster, True, Nothing)
        End Function

    End Class

End Namespace


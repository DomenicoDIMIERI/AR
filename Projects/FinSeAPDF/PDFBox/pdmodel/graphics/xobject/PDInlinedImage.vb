Imports FinSeA.Drawings
Imports FinSeA.Io
Imports FinSeA.org.apache.pdfbox.filter
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.color
Imports FinSeA.org.apache.pdfbox.util

Namespace org.apache.pdfbox.pdmodel.graphics.xobject

    '/**
    ' * This class represents an inlined image.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.6 $
    ' */
    Public Class PDInlinedImage

        Private params As ImageParameters
        Private imageData As Byte()

        '/**
        ' * This will get the image parameters.
        ' *
        ' * @return The image parameters.
        ' */
        Public Function getImageParameters() As ImageParameters
            Return params
        End Function

        '/**
        ' * This will set the image parameters for this image.
        ' *
        ' * @param imageParams The imageParams.
        ' */
        Public Sub setImageParameters(ByVal imageParams As ImageParameters)
            params = imageParams
        End Sub

        '/**
        ' * Get the bytes for the image.
        ' *
        ' * @return The image data.
        ' */
        Public Function getImageData() As Byte()
            Return imageData
        End Function

        '/**
        ' * Set the bytes that make up the image.
        ' *
        ' * @param value The image data.
        ' */
        Public Sub setImageData(ByVal value() As Byte)
            imageData = value
        End Sub

        '/**
        ' * This will take the inlined image information and create a java.awt.Image from
        ' * it.
        ' *
        ' * @return The image that this object represents.
        ' *
        ' * @throws IOException If there is an error creating the image.
        ' */
        Public Function createImage() As BufferedImage 'throws IOException
            Return createImage(Nothing)
        End Function

        '/**
        ' * This will take the inlined image information and create a java.awt.Image from
        ' * it.
        ' * 
        ' * @param colorSpaces The ColorSpace dictionary from the current resources, if any.
        ' *
        ' * @return The image that this object represents.
        ' *
        ' * @throws IOException If there is an error creating the image.
        ' */
        Public Function createImage(ByVal colorSpaces As Map) As BufferedImage 'throws IOException
            '/*
            ' * This was the previous implementation, not sure which is better right now.
            ' *         byte[] transparentColors = new byte[]{(byte)0xFF,(byte)0xFF};
            '   Dim colors As Byte() = {0, &HFF}
            '   Dim colorModel As IndexColorModel = New IndexColorModel(1, 2, colors, colors, colors, transparentColors)
            '   Dim image As BufferedImage = New BufferedImage(params.getWidth(), params.getHeight(), BufferedImage.TYPE_BYTE_BINARY, colorModel)
            '   Dim buffer As DataBufferByte = New DataBufferByte(getImageData(), 1)
            '   Dim raster As WritableRaster = raster.createPackedRaster(buffer, params.getWidth(), params.getHeight(), params.getBitsPerComponent(), New Point(0, 0))
            '   image.setData(raster)
            '   Return image
            '*/


            'verify again pci32.pdf before changing below
            Dim pcs As PDColorSpace = params.getColorSpace(colorSpaces)
            Dim colorModel As ColorModel = Nothing
            If (pcs IsNot Nothing) Then
                colorModel = pcs.createColorModel(params.getBitsPerComponent())
            Else
                Dim transparentColors() As Byte = {&HFF, &HFF}
                Dim colors() As Byte = {0, &HFF}
                colorModel = New IndexColorModel(1, 2, colors, colors, colors, transparentColors)
            End If
            Dim filters As List = params.getFilters()
            Dim finalData() As Byte = Nothing
            If (filters Is Nothing) Then
                finalData = getImageData()
            Else
                Dim [in] As ByteArrayInputStream = New ByteArrayInputStream(getImageData())
                Dim out As ByteArrayOutputStream = New ByteArrayOutputStream(getImageData().Length)
                Dim filterManager As FilterManager = New FilterManager()
                For i As Integer = 0 To filters.size() - 1
                    out.reset()
                    Dim filter As pdfbox.filter.Filter = filterManager.getFilter(filters.get(i))
                    filter.decode([in], out, params.getDictionary(), i)
                    [in] = New ByteArrayInputStream(out.toByteArray())
                Next
                finalData = out.toByteArray()
            End If

            Dim raster As WritableRaster = colorModel.createCompatibleWritableRaster(params.getWidth(), params.getHeight())
            '/*    Raster.createPackedRaster(
            '        buffer,
            '        params.getWidth(),
            '        params.getHeight(),
            '        params.getBitsPerComponent(),
            '        new Point(0,0) );
            '        */
            Dim rasterBuffer As DataBuffer = raster.getDataBuffer()
            If (TypeOf (rasterBuffer) Is DataBufferByte) Then
                Dim byteBuffer As DataBufferByte = rasterBuffer
                Dim data() As Byte = DirectCast(byteBuffer, DataBufferByte).getData()
                Array.Copy(finalData, 0, data, 0, data.Length)
            ElseIf (TypeOf (rasterBuffer) Is DataBufferInt) Then
                Dim byteBuffer As DataBufferInt = rasterBuffer
                Dim data() As Integer = DirectCast(byteBuffer, DataBufferInt).getData()
                For i As Integer = 0 To finalData.Length - 1
                    data(i) = (finalData(i) + 256) Mod 256
                Next
            End If
            Dim image As BufferedImage = New BufferedImage(colorModel, raster, False, Nothing)
            image.setData(raster)
            Return image
        End Function

    End Class

End Namespace
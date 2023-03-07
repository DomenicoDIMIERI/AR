Imports System.IO
Imports System.Drawing
Imports FinSeA.Drawings
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.color
Imports FinSeA.org.apache.pdfbox.util
Imports FinSeA.Io

Namespace org.apache.pdfbox.filter

    '/**
    ' * This is used for the JPXDecode filter.
    ' *
    ' * @author <a href="mailto:timo.boehme@ontochem.com">Timo Boehme</a>
    ' *
    ' */
    Public Class JPXFilter
        Implements Filter

        '/** Log instance. */
        'private static final Log LOG = LogFactory.getLog(JPXFilter.class);

        '/**
        ' * Decode JPEG2000 data using Java ImageIO library.
        ' *
        ' * {@inheritDoc}
        ' *
        ' */
        Public Sub decode(ByVal compressedData As InputStream, ByVal result As OutputStream, ByVal options As COSDictionary, ByVal filterIndex As Integer) Implements Filter.decode
            Dim bi As BufferedImage = New System.Drawing.Bitmap(compressedData) ' ImageIO.read(compressedData)
            If (bi IsNot Nothing) Then
                Dim dBuf As DataBufferByte = bi.getData().getDataBuffer()
                If (dBuf.getDataType() = DataBuffer.TYPE_BYTE) Then
                    ' maybe some wrong/missing values have to be revised/added
                    Dim colorModel As ColorModel = bi.getColorModel()
                    If (options.getItem(COSName.COLORSPACE) Is Nothing) Then
                        options.setItem(COSName.COLORSPACE, PDColorSpaceFactory.createColorSpace(Nothing, colorModel.getColorSpace()))
                    End If
                    options.setInt(COSName.BITS_PER_COMPONENT, colorModel.getPixelSize() / colorModel.getNumComponents())
                    options.setInt(COSName.HEIGHT, bi.getHeight)
                    options.setInt(COSName.WIDTH, bi.getWidth)
                    result.Write(dBuf.getData())
                Else
                    Throw New BadImageFormatException("Image data buffer not of type byte but type " & dBuf.getDataType())
                End If
            End If
        End Sub

        '/**
        ' * {@inheritDoc}
        ' */
        Public Sub encode(ByVal rawData As InputStream, ByVal result As OutputStream, ByVal options As COSDictionary, ByVal filterIndex As Integer) Implements Filter.encode
            Throw New NotImplementedException("JPXFilter.encode is not implemented yet, skipping this stream.")
        End Sub

    End Class

End Namespace

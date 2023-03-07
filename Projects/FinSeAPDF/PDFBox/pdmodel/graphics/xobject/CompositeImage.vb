Imports FinSeA.Drawings
Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos

Namespace org.apache.pdfbox.pdmodel.graphics.xobject

    '/**
    ' * This class is responsible for combining a base image with an SMask-based transparency
    ' * image to form a composite image.
    ' * See section 11.5 of the pdf specification for details on Soft Masks.
    ' * <p/>
    ' * Briefly however, an Smask is a supplementary greyscale image whose RGB-values define
    ' * a transparency mask which, when combined appropriately with the base image,
    ' * allows per-pixel transparency to be applied.
    ' * <p/>
    ' * Note that Smasks are not required for any image and if the smask is not present
    ' * in the pdf file, the image will have no transparent pixels.
    ' *
    ' * @author Neil McErlean
    ' */
    Public Class CompositeImage

        Private baseImage As BufferedImage
        Private smaskImage As BufferedImage

        '/**
        ' * Standard constructor.
        ' * @param baseImage the base Image.
        ' * @param smaskImage the transparency image.
        ' *
        ' */
        Public Sub New(ByVal baseImage As BufferedImage, ByVal smaskImage As BufferedImage)
            Me.baseImage = baseImage
            Me.smaskImage = smaskImage
        End Sub

        '/**
        ' * This method applies the specified transparency mask to a given image and returns a new BufferedImage
        ' * whose alpha values are computed from the transparency mask (smask) image.
        ' * 
        ' * @param decodeArray the decode array
        ' * @return the masked image
        ' * @throws IOException if something went wrong
        ' * 
        ' */
        Public Function createMaskedImage(ByVal decodeArray As COSArray) As BufferedImage 'throws IOException
            '// The decode array should only be [0 1] or [1 0]. See PDF spec.
            '// [0 1] means the smask's RGB values give transparency. Default: see PDF spec section 8.9.5.1
            '// [1 0] means the smask's RGB values give opacity.

            Dim isOpaque As Boolean = False
            If (decodeArray IsNot Nothing) Then
                isOpaque = decodeArray.getInt(0) > decodeArray.getInt(1)
            End If

            Dim baseImageWidth As Integer = baseImage.getWidth()
            Dim baseImageHeight As Integer = baseImage.getHeight()
            Dim result As BufferedImage = New BufferedImage(baseImageWidth, baseImageHeight, BufferedImage.TYPE_INT_ARGB)
            For x As Integer = 0 To baseImageWidth - 1
                For y As Integer = 0 To baseImageHeight - 1
                    Dim rgb As Integer = baseImage.getRGB(x, y)
                    Dim alpha As Integer = smaskImage.getRGB(x, y)

                    ' The smask image defines a transparency mask but it has no alpha values itself, instead
                    ' using the greyscale values to indicate transparency.
                    ' 0xAARRGGBB

                    ' We need to remove any alpha value in the main image.
                    Dim rgbOnly As Integer = &HFFFFFF & rgb

                    ' We need to use one of the rgb values as the new alpha value for the main image.
                    ' It seems the mask is greyscale, so it shouldn't matter whether we use R, G or B
                    ' as the indicator of transparency.
                    If (isOpaque) Then
                        alpha = Not alpha
                    End If
                    Dim alphaOnly As Integer = alpha << 24

                    result.setRGB(x, y, rgbOnly Or alphaOnly)
                Next
            Next
            Return result
        End Function

        '/**
        ' * This method applies the specified stencil mask to a given image and returns a new BufferedImage
        ' * whose alpha values are computed from the stencil mask (smask) image.
        ' * 
        ' * @param decodeArray the decode array
        ' * @return the stencil masked image 
        ' */
        Public Function createStencilMaskedImage(ByVal decodeArray As COSArray) As BufferedImage
            ' default: 0 (black) == opaque
            Dim alphaValue As Integer = 0
            If (decodeArray IsNot Nothing) Then
                ' invert the stencil mask: 1 (white) == opaque
                alphaValue = IIf(decodeArray.getInt(0) > decodeArray.getInt(1), 1, 0)
            End If

            Dim baseImageWidth As Integer = baseImage.getWidth()
            Dim baseImageHeight As Integer = baseImage.getHeight()
            Dim maskRaster As WritableRaster = smaskImage.getRaster()
            Dim result As BufferedImage = New BufferedImage(baseImageWidth, baseImageHeight, BufferedImage.TYPE_INT_ARGB)
            Dim alpha(1 - 1) As Integer '= new int(1);
            For x As Integer = 0 To baseImageWidth - 1
                For y As Integer = 0 To baseImageHeight - 1
                    maskRaster.getPixel(x, y, alpha)
                    ' We need to remove any alpha value in the main image.
                    Dim rgbOnly As Integer = &HFFFFFF And baseImage.getRGB(x, y)
                    Dim alphaOnly As Integer = IIf(alpha(0) = alphaValue, &HFF000000, 0)
                    result.setRGB(x, y, rgbOnly Or alphaOnly)
                Next
            Next
            Return result
        End Function

    End Class

End Namespace
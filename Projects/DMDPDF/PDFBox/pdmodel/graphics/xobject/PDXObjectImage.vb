Imports FinSeA.Drawings
Imports FinSeA.Io
Imports System.IO

Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.color

Namespace org.apache.pdfbox.pdmodel.graphics.xobject

    '/**
    ' * The prototype for all PDImages.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @author mathiak
    ' * @version $Revision: 1.9 $
    ' */
    Public MustInherit Class PDXObjectImage
        Inherits PDXObject

        '/**
        ' * Log instance.
        ' */
        'private static final Log LOG = LogFactory.getLog(PDXObjectImage.class);

        ''' <summary>
        ''' The XObject subtype.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const SUB_TYPE As String = "Image"

        ''' <summary>
        ''' This contains the suffix used when writing to file.
        ''' </summary>
        ''' <remarks></remarks>
        Private suffix As String

        Private stencilColor As PDColorState

        '/**
        ' * Standard constructor.
        ' *
        ' * @param imageStream The XObject is passed as a COSStream.
        ' * @param fileSuffix The file suffix, jpg/png.
        ' */
        Public Sub New(ByVal imageStream As PDStream, ByVal fileSuffix As String)
            MyBase.New(imageStream)
            suffix = fileSuffix
        End Sub

        '/**
        ' * Standard constuctor.
        ' *
        ' * @param doc The document to store the stream in.
        ' * @param fileSuffix The file suffix, jpg/png.
        ' */
        Public Sub New(ByVal doc As PDDocument, ByVal fileSuffix As String)
            MyBase.New(doc)
            getCOSStream().setName(COSName.SUBTYPE, SUB_TYPE)
            suffix = fileSuffix
        End Sub

        '/**
        ' * Create the correct thumbnail from the cos base.
        ' *
        ' * @param xobject The cos level xobject to create.
        ' *
        ' * @return a pdmodel xobject
        ' * @throws IOException If there is an error creating the xobject.
        ' */
        Public Shared Function createThumbnailXObject(ByVal xobject As COSBase) As PDXObject
            Dim retval As PDXObject = commonXObjectCreation(xobject, True)
            Return retval
        End Function

        '/**
        ' * Returns an java.awt.Image, that can be used for display etc.
        ' *
        ' * @return This PDF object as an AWT image.
        ' *
        ' * @throws IOException If there is an error creating the image.
        ' */
        Public MustOverride Function getRGBImage() As BufferedImage

        '/**
        ' * Returns a PDXObjectImage of the SMask image, if there is one.
        ' * See section 11.5 of the pdf specification for details on Soft Masks.
        ' *
        ' * @return the PDXObjectImage of the SMask if there is one, else <code>null</code>.
        ' * @throws IOException if an I/O error occurs creating an XObject
        ' */
        Public Function getSMaskImage() As PDXObjectImage '
            Dim cosStream As COSStream = getPDStream().getStream()
            Dim smask As COSBase = cosStream.getDictionaryObject(COSName.SMASK)

            If (smask Is Nothing) Then
                Return Nothing
            Else
                Return PDXObject.createXObject(smask)
            End If
        End Function

        Public Function applyMasks(ByVal baseImage As BufferedImage) As BufferedImage
            If (getImageMask()) Then
                Return imageMask(baseImage)
            End If
            If (getMask() IsNot Nothing) Then
                Return mask(baseImage)
            End If
            Dim smask As PDXObjectImage = getSMaskImage()
            If (smask IsNot Nothing) Then
                Dim smaskBI As BufferedImage = smask.getRGBImage()
                Dim decodeArray As COSArray = smask.getDecode()
                Dim compositeImage As CompositeImage = New CompositeImage(baseImage, smaskBI)
                Dim rgbImage As BufferedImage = CompositeImage.createMaskedImage(decodeArray)
                Return rgbImage
            End If
            Return baseImage
        End Function

        Public Function hasMask() As Boolean ' throws IOException
            Return getImageMask() OrElse getMask() IsNot Nothing OrElse getSMaskImage() IsNot Nothing
        End Function


        Public Function imageMask(ByVal baseImage As BufferedImage) As BufferedImage 'throws IOException 
            Dim stencilMask As BufferedImage = New BufferedImage(baseImage.getWidth(), baseImage.getHeight(), BufferedImage.TYPE_INT_ARGB)
            Dim graphics As Graphics2D = stencilMask.getGraphics()
            If (getStencilColor() IsNot Nothing) Then
                graphics.setColor(getStencilColor().getJavaColor())
            Else
                ' this might happen when using ExractImages, see PDFBOX-1145
                LOG.debug("no stencil color for PixelMap found, using Color.BLACK instead.")
                graphics.setColor(JColor.Black)
            End If

            graphics.fillRect(0, 0, baseImage.getWidth(), baseImage.getHeight())
            ' assume default values ([0,1]) for the DecodeArray
            ' TODO DecodeArray == [1,0]
            graphics.setComposite(AlphaComposite.DstIn)
            graphics.drawImage(baseImage, Nothing, 0, 0)
            graphics.dispose()
            Return stencilMask
        End Function

        Public Function mask(ByVal baseImage As BufferedImage) As BufferedImage   'throws IOException
            Dim _mask As COSBase = getMask()
            If (TypeOf (_mask) Is COSStream) Then
                Dim maskImageRef As PDXObjectImage = PDXObject.createXObject(_mask)
                Dim maskImage As BufferedImage = maskImageRef.getRGBImage()
                If (maskImage Is Nothing) Then
                    LOG.warn("masking getRGBImage returned NULL")
                    Return baseImage
                End If

                Dim newImage As BufferedImage = New BufferedImage(maskImage.getWidth(), maskImage.getHeight(), BufferedImage.TYPE_INT_ARGB)
                Dim graphics As Graphics2D = newImage.getGraphics()
                graphics.drawImage(baseImage, 0, 0, maskImage.getWidth(), maskImage.getHeight(), 0, 0, baseImage.getWidth(), baseImage.getHeight(), Nothing)
                graphics.setComposite(AlphaComposite.DstIn)
                graphics.drawImage(maskImage, Nothing, 0, 0)
                graphics.dispose()
                Return newImage
            Else
                ' TODO Colour key masking
                LOG.warn("Colour key masking isn't supported")
                Return baseImage
            End If
        End Function

        '/**
        ' * Writes the Image to out.
        ' * @param out the OutputStream that the Image is written to.
        ' * @throws IOException when somethings wrong with out
        ' */
        Public MustOverride Sub write2OutputStream(ByVal out As OutputStream) 'throws IOException;

        '/**
        ' * Writes the image to a file with the filename + an appropriate suffix, like "Image.jpg".
        ' * The suffix is automatically set by the
        ' * @param filename the filename
        ' * @throws IOException When somethings wrong with the corresponding file.
        ' */
        Public Sub write2file(ByVal filename As String) 'throws IOException
            Dim out As FileOutputStream = Nothing
            Try
                out = New FileOutputStream(filename & "." & suffix)
                write2OutputStream(out)
                out.flush()
            Finally
                If (out IsNot Nothing) Then
                    out.close()
                End If
            End Try
        End Sub

        '/**
        ' * Writes the image to a file with the filename + an appropriate
        ' * suffix, like "Image.jpg".
        ' * The suffix is automatically set by the
        ' * @param file the file
        ' * @throws IOException When somethings wrong with the corresponding file.
        ' */
        Public Sub write2file(ByVal file As FileInfo) 'throws IOException
            Dim out As FileOutputStream = Nothing
            Try
                out = New FileOutputStream(file.FullName)
                write2OutputStream(out)
                out.flush()
            Finally
                If (out IsNot Nothing) Then
                    out.close()
                End If
            End Try
        End Sub

        '/**
        ' * Get the height of the image.
        ' *
        ' * @return The height of the image.
        ' */
        Public Function getHeight() As Integer
            Return getCOSStream().getInt(COSName.HEIGHT, -1)
        End Function

        '/**
        ' * Set the height of the image.
        ' *
        ' * @param height The height of the image.
        ' */
        Public Sub setHeight(ByVal height As Integer)
            getCOSStream().setInt(COSName.HEIGHT, height)
        End Sub

        '/**
        ' * Get the width of the image.
        ' *
        ' * @return The width of the image.
        ' */
        Public Function getWidth() As Integer
            Return getCOSStream().getInt(COSName.WIDTH, -1)
        End Function

        '/**
        ' * Set the width of the image.
        ' *
        ' * @param width The width of the image.
        ' */
        Public Sub setWidth(ByVal width As Integer)
            getCOSStream().setInt(COSName.WIDTH, width)
        End Sub

        '/**
        ' * The bits per component of this image.  This will return -1 if one has not
        ' * been set.
        ' *
        ' * @return The number of bits per component.
        ' */
        Public Function getBitsPerComponent() As Integer
            Return getCOSStream().getInt(COSName.BITS_PER_COMPONENT, COSName.BPC, -1)
        End Function

        '/**
        ' * Set the number of bits per component.
        ' *
        ' * @param bpc The number of bits per component.
        ' */
        Public Sub setBitsPerComponent(ByVal bpc As Integer)
            getCOSStream().setInt(COSName.BITS_PER_COMPONENT, bpc)
        End Sub

        '/**
        ' * This will get the color space or null if none exists.
        ' *
        ' * @return The color space for this image.
        ' *
        ' * @throws IOException If there is an error getting the colorspace.
        ' */
        Public Function getColorSpace() As PDColorSpace ' throws IOException
            Dim cs As COSBase = getCOSStream().getDictionaryObject(COSName.COLORSPACE, COSName.CS)
            Dim retval As PDColorSpace = Nothing
            If (cs IsNot Nothing) Then
                retval = PDColorSpaceFactory.createColorSpace(cs)
                If (retval Is Nothing) Then
                    LOG.info("About to return NULL from createColorSpace branch")
                End If
            Else
                'there are some cases where the 'required' CS value is not present
                'but we know that it will be grayscale for a CCITT filter.
                Dim filter As COSBase = getCOSStream().getDictionaryObject(COSName.FILTER)
                If (COSName.CCITTFAX_DECODE.equals(filter) OrElse COSName.CCITTFAX_DECODE_ABBREVIATION.equals(filter)) Then
                    retval = New PDDeviceGray()
                ElseIf (COSName.JBIG2_DECODE.equals(filter)) Then
                    retval = New PDDeviceGray()
                ElseIf (getImageMask()) Then
                    ' image is a stencil mask -> use DeviceGray
                    retval = New PDDeviceGray()
                Else
                    LOG.info("About to return NULL from unhandled branch. filter = " & filter.ToString)
                End If
            End If
            Return retval
        End Function

        '/**
        ' * This will set the color space for this image.
        ' *
        ' * @param cs The color space for this image.
        ' */
        Public Sub setColorSpace(ByVal cs As PDColorSpace)
            Dim base As COSBase = Nothing
            If (cs IsNot Nothing) Then
                base = cs.getCOSObject()
            End If
            getCOSStream().setItem(COSName.COLORSPACE, base)
        End Sub

        '/**
        ' * This will get the suffix for this image type, jpg/png.
        ' *
        ' * @return The image suffix.
        ' */
        Public Function getSuffix() As String
            Return suffix
        End Function

        '/**
        ' * Get the ImageMask flag. Used in Stencil Masking.  Section 4.8.5 of the spec.
        ' *
        ' * @return The ImageMask flag.  This is optional and defaults to False, so if it does not exist, we return False
        ' */
        Public Function getImageMask() As Boolean
            Return getCOSStream().getBoolean(COSName.IMAGE_MASK, False)
        End Function

        '/**
        ' * Set the current non stroking colorstate. It'll be used to create stencil masked images.
        ' * 
        ' * @param stencilColorValue The non stroking colorstate
        ' */
        Public Sub setStencilColor(ByVal stencilColorValue As PDColorState)
            stencilColor = stencilColorValue
        End Sub

        '/**
        ' * Returns the non stroking colorstate to be used to create stencil makes images.
        ' * 
        ' * @return The current non stroking colorstate.
        ' */
        Public Function getStencilColor() As PDColorState
            Return stencilColor
        End Function

        '/**
        ' * Returns the Decode Array of an XObjectImage.
        ' * @return the decode array
        ' */
        Public Function getDecode() As COSArray
            Dim decode As COSBase = getCOSStream().getDictionaryObject(COSName.DECODE)
            If (decode IsNot Nothing AndAlso TypeOf (decode) Is COSArray) Then
                Return decode
            End If
            Return Nothing
        End Function

        '/**
        ' * Returns the optional mask of a XObjectImage if there is one.
        ' *
        ' * @return The mask otherwise null.
        ' */
        Public Function getMask() As COSBase
            Dim mask As COSBase = getCOSStream().getDictionaryObject(COSName.MASK)
            If (mask IsNot Nothing) Then
                Return mask
            End If
            Return Nothing
        End Function

    End Class

End Namespace
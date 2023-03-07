Imports System.IO
Imports System.Drawing.Graphics
Imports FinSeA.Io
Imports FinSeA.Drawings
Imports FinSeA.Exceptions
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.pdmodel.font
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.color
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.xobject

Namespace org.apache.pdfbox.pdmodel.edit


    '/**
    ' * This class is a convenience for creating page content streams.  You MUST
    ' * call close() when you are finished with this object.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * 
    ' */
    Public Class PDPageContentStream
        '{
        '    /**
        '     * Log instance.
        '     */
        '    private static final Log LOG = LogFactory.getLog(PDPageContentStream.class);

        Private page As PDPage
        Private output As OutputStream
        Private inTextMode As Boolean = False
        Private resources As PDResources

        Private currentStrokingColorSpace As PDColorSpace = New PDDeviceGray()
        Private currentNonStrokingColorSpace As PDColorSpace = New PDDeviceGray()

        ' cached storage component for getting color values
        Private colorComponents As Single() = Array.CreateInstance(GetType(Single), 4)

        Private formatDecimal As NumberFormat = NumberFormat.getNumberInstance(Locale.US)

        Private Const ISO8859 As String = "ISO-8859-1"

        Private Shared Function getISOBytes(ByVal s As String) As Byte()
            Try
                Return Sistema.Strings.GetBytes(s, ISO8859)
            Catch ex As UnsupportedEncodingException
                Throw New IllegalStateException(ex)
            End Try
        End Function

        Private Shared ReadOnly BEGIN_TEXT As Byte() = getISOBytes("BT" & vbLf) '\n")
        Private Shared ReadOnly END_TEXT As Byte() = getISOBytes("ET" & vbLf) '\n")
        Private Shared ReadOnly SET_FONT As Byte() = getISOBytes("Tf" & vbLf) '\n")
        Private Shared ReadOnly MOVE_TEXT_POSITION As Byte() = getISOBytes("Td" & vbLf) '\n")
        Private Shared ReadOnly SET_TEXT_MATRIX As Byte() = getISOBytes("Tm" & vbLf) '\n")
        Private Shared ReadOnly SHOW_TEXT As Byte() = getISOBytes("Tj" & vbLf) '\n")

        Private Shared ReadOnly SAVE_GRAPHICS_STATE As Byte() = getISOBytes("q" & vbLf) '\n")
        Private Shared ReadOnly RESTORE_GRAPHICS_STATE As Byte() = getISOBytes("Q" & vbLf) '\n")
        Private Shared ReadOnly CONCATENATE_MATRIX As Byte() = getISOBytes("cm" & vbLf) '\n")
        Private Shared ReadOnly XOBJECT_DO As Byte() = getISOBytes("Do" & vbLf) '\n")
        Private Shared ReadOnly RG_STROKING As Byte() = getISOBytes("RG" & vbLf) '\n")
        Private Shared ReadOnly RG_NON_STROKING As Byte() = getISOBytes("rg" & vbLf) '\n")
        Private Shared ReadOnly K_STROKING As Byte() = getISOBytes("K" & vbLf) '\n")
        Private Shared ReadOnly K_NON_STROKING As Byte() = getISOBytes("k" & vbLf) '\n")
        Private Shared ReadOnly G_STROKING As Byte() = getISOBytes("G" & vbLf) '\n")
        Private Shared ReadOnly G_NON_STROKING As Byte() = getISOBytes("g" & vbLf) '\n")
        Private Shared ReadOnly RECTANGLE As Byte() = getISOBytes("re" & vbLf) '\n")
        Private Shared ReadOnly FILL_NON_ZERO As Byte() = getISOBytes("f" & vbLf) '\n")
        Private Shared ReadOnly FILL_EVEN_ODD As Byte() = getISOBytes("f*" & vbLf) '\n")
        Private Shared ReadOnly LINE_TO As Byte() = getISOBytes("l" & vbLf) '\n")
        Private Shared ReadOnly MOVE_TO As Byte() = getISOBytes("m" & vbLf) '\n")
        Private Shared ReadOnly CLOSE_STROKE As Byte() = getISOBytes("s" & vbLf) '\n")
        Private Shared ReadOnly _STROKE As Byte() = getISOBytes("S" & vbLf) '\n")
        Private Shared ReadOnly LINE_WIDTH As Byte() = getISOBytes("w" & vbLf) '\n")
        Private Shared ReadOnly LINE_JOIN_STYLE As Byte() = getISOBytes("j" & vbLf) '\n")
        Private Shared ReadOnly LINE_CAP_STYLE As Byte() = getISOBytes("J" & vbLf) '\n")
        Private Shared ReadOnly LINE_DASH_PATTERN As Byte() = getISOBytes("d" & vbLf) '\n")
        Private Shared ReadOnly CLOSE_SUBPATH As Byte() = getISOBytes("h" & vbLf) '\n")
        Private Shared ReadOnly CLIP_PATH_NON_ZERO As Byte() = getISOBytes("W" & vbLf) '\n")
        Private Shared ReadOnly CLIP_PATH_EVEN_ODD As Byte() = getISOBytes("W*" & vbLf) '\n")
        Private Shared ReadOnly NOP As Byte() = getISOBytes("n" & vbLf) '\n")
        Private Shared ReadOnly BEZIER_312 As Byte() = getISOBytes("c" & vbLf) '\n")
        Private Shared ReadOnly BEZIER_32 As Byte() = getISOBytes("v" & vbLf) '\n")
        Private Shared ReadOnly BEZIER_313 As Byte() = getISOBytes("y" & vbLf) '\n")

        Private Shared ReadOnly BMC As Byte() = getISOBytes("BMC" & vbLf) '\n")
        Private Shared ReadOnly BDC As Byte() = getISOBytes("BDC" & vbLf) '\n")
        Private Shared ReadOnly EMC As Byte() = getISOBytes("EMC" & vbLf) '\n")

        Private Shared ReadOnly SET_STROKING_COLORSPACE As Byte() = getISOBytes("CS" & vbLf) '\n")
        Private Shared ReadOnly SET_NON_STROKING_COLORSPACE As Byte() = getISOBytes("cs" & vbLf) '\n")

        Private Shared ReadOnly SET_STROKING_COLOR_SIMPLE As Byte() = getISOBytes("SC" & vbLf) '\n")
        Private Shared ReadOnly SET_STROKING_COLOR_COMPLEX As Byte() = getISOBytes("SCN" & vbLf) '\n")
        Private Shared ReadOnly SET_NON_STROKING_COLOR_SIMPLE As Byte() = getISOBytes("sc" & vbLf) '\n")
        Private Shared ReadOnly SET_NON_STROKING_COLOR_COMPLEX As Byte() = getISOBytes("scn" & vbLf) '\n")

        Private Shared ReadOnly OPENING_BRACKET As Byte() = getISOBytes("[")
        Private Shared ReadOnly CLOSING_BRACKET As Byte() = getISOBytes("]")

        Private Const SPACE As Integer = 32

        '/**
        ' * Create a new PDPage content stream.
        ' *
        ' * @param document The document the page is part of.
        ' * @param sourcePage The page to write the contents to.
        ' * @throws IOException If there is an error writing to the page contents.
        ' */
        Public Sub New(ByVal document As PDDocument, ByVal sourcePage As PDPage) 'throws IOException
            Me.New(document, sourcePage, False, True)
        End Sub

        '/**
        ' * Create a new PDPage content stream.
        ' *
        ' * @param document The document the page is part of.
        ' * @param sourcePage The page to write the contents to.
        ' * @param appendContent Indicates whether content will be overwritten. If false all previous content is deleted.
        ' * @param compress Tell if the content stream should compress the page contents.
        ' * @throws IOException If there is an error writing to the page contents.
        ' */
        Public Sub New(ByVal document As PDDocument, ByVal sourcePage As PDPage, ByVal appendContent As Boolean, ByVal compress As Boolean) 'throws IOException
            Me.New(document, sourcePage, appendContent, compress, False)
        End Sub

        '/**
        ' * Create a new PDPage content stream.
        ' *
        ' * @param document The document the page is part of.
        ' * @param sourcePage The page to write the contents to.
        ' * @param appendContent Indicates whether content will be overwritten. If false all previous content is deleted.
        ' * @param compress Tell if the content stream should compress the page contents.
        ' * @param resetContext Tell if the graphic context should be reseted.
        ' * @throws IOException If there is an error writing to the page contents.
        ' */
        Public Sub New(ByVal document As PDDocument, ByVal sourcePage As PDPage, ByVal appendContent As Boolean, ByVal compress As Boolean, ByVal resetContext As Boolean)  'throws IOException
            page = sourcePage
            resources = page.getResources()
            If (resources Is Nothing) Then
                resources = New PDResources()
                page.setResources(resources)
            End If

            ' Get the pdstream from the source page instead of creating a new one
            Dim contents As PDStream = sourcePage.getContents()
            Dim hasContent As Boolean = contents IsNot Nothing

            ' If request specifies the need to append to the document
            If (appendContent AndAlso hasContent) Then
                ' Create a pdstream to append new content
                Dim contentsToAppend As PDStream = New PDStream(document)

                ' This will be the resulting COSStreamArray after existing and new streams are merged
                Dim compoundStream As COSStreamArray = Nothing

                ' If contents is already an array, a new stream is simply appended to it
                If (TypeOf (contents.getStream()) Is COSStreamArray) Then
                    compoundStream = contents.getStream()
                    compoundStream.appendStream(contentsToAppend.getStream())
                Else
                    ' Creates the COSStreamArray and adds the current stream plus a new one to it
                    Dim newArray As COSArray = New COSArray()
                    newArray.add(contents.getCOSObject())
                    newArray.add(contentsToAppend.getCOSObject())
                    compoundStream = New COSStreamArray(newArray)
                End If

                If (compress) Then
                    Dim filters As List(Of COSName) = New ArrayList(Of COSName)
                    filters.add(COSName.FLATE_DECODE)
                    contentsToAppend.setFilters(filters)
                End If

                If (resetContext) Then
                    ' create a new stream to encapsulate the existing stream
                    Dim saveGraphics As PDStream = New PDStream(document)
                    output = saveGraphics.createOutputStream()
                    ' save the initial/unmodified graphics context
                    saveGraphicsState()
                    close()
                    If (compress) Then
                        Dim filters As List(Of COSName) = New ArrayList(Of COSName)
                        filters.add(COSName.FLATE_DECODE)
                        saveGraphics.setFilters(filters)
                    End If
                    ' insert the new stream at the beginning
                    compoundStream.insertCOSStream(saveGraphics)
                End If

                ' Sets the compoundStream as page contents
                sourcePage.setContents(New PDStream(compoundStream))
                output = contentsToAppend.createOutputStream()
                If (resetContext) Then
                    ' restore the initial/unmodified graphics context
                    restoreGraphicsState()
                End If
            Else
                If (hasContent) Then
                    LOG.warn("You are overwriting an existing content, you should use the append mode")
                End If
                contents = New PDStream(document)
                If (compress) Then
                    Dim filters As List(Of COSName) = New ArrayList(Of COSName)
                    filters.add(COSName.FLATE_DECODE)
                    contents.setFilters(filters)
                End If
                sourcePage.setContents(contents)
                output = contents.createOutputStream()
            End If
            formatDecimal.setMaximumFractionDigits(10)
            formatDecimal.setGroupingUsed(False)
        End Sub

        '/**
        ' * Begin some text operations.
        ' *
        ' * @throws IOException If there is an error writing to the stream or if you attempt to
        ' *         nest beginText calls.
        ' */
        Public Sub beginText() 'throws IOException
            If (inTextMode) Then
                Throw New IOException("Error: Nested beginText() calls are not allowed.")
            End If
            appendRawCommands(BEGIN_TEXT)
            inTextMode = True
        End Sub

        '/**
        ' * End some text operations.
        ' *
        ' * @throws IOException If there is an error writing to the stream or if you attempt to
        ' *         nest endText calls.
        ' */
        Public Sub endText() 'throws IOException
            If (Not inTextMode) Then
                Throw New IOException("Error: You must call beginText() before calling endText.")
            End If
            appendRawCommands(END_TEXT)
            inTextMode = False
        End Sub

        '/**
        ' * Set the font to draw text with.
        ' *
        ' * @param font The font to use.
        ' * @param fontSize The font size to draw the text.
        ' * @throws IOException If there is an error writing the font information.
        ' */
        Public Sub setFont(ByVal font As PDFont, ByVal fontSize As Single) 'throws IOException
            Dim fontMapping As String = resources.addFont(font)
            appendRawCommands("/")
            appendRawCommands(fontMapping)
            appendRawCommands(SPACE)
            appendRawCommands(fontSize)
            appendRawCommands(SPACE)
            appendRawCommands(SET_FONT)
        End Sub

        '/**
        ' * Draw an image at the x,y coordinates, with the default size of the image.
        ' *
        ' * @param image The image to draw.
        ' * @param x The x-coordinate to draw the image.
        ' * @param y The y-coordinate to draw the image.
        ' *
        ' * @throws IOException If there is an error writing to the stream.
        ' */
        Public Sub drawImage(ByVal image As PDXObjectImage, ByVal x As Single, ByVal y As Single) 'throws IOException
            drawXObject(image, x, y, image.getWidth(), image.getHeight())
        End Sub

        '/**
        ' * Draw an xobject(form or image) at the x,y coordinates and a certain width and height.
        ' *
        ' * @param xobject The xobject to draw.
        ' * @param x The x-coordinate to draw the image.
        ' * @param y The y-coordinate to draw the image.
        ' * @param width The width of the image to draw.
        ' * @param height The height of the image to draw.
        ' *
        ' * @throws IOException If there is an error writing to the stream.
        ' */
        Public Sub drawXObject(ByVal xobject As PDXObject, ByVal x As Single, ByVal y As Single, ByVal width As Single, ByVal height As Single)  'throws IOException
            Dim transform As AffineTransform = New AffineTransform(width, 0, 0, height, x, y)
            drawXObject(xobject, transform)
        End Sub

        '/**
        ' * Draw an xobject(form or image) using the given {@link AffineTransform} to position
        ' * the xobject.
        ' *
        ' * @param xobject The xobject to draw.
        ' * @param transform the transformation matrix
        ' * @throws IOException If there is an error writing to the stream.
        ' */
        Public Sub drawXObject(ByVal xobject As PDXObject, ByVal transform As AffineTransform)  'throws IOException
            If (inTextMode) Then
                Throw New IOException("Error: drawXObject is not allowed within a text block.")
            End If
            Dim xObjectPrefix As String = ""
            If (TypeOf (xobject) Is PDXObjectImage) Then
                xObjectPrefix = "Im"
            Else
                xObjectPrefix = "Form"
            End If
            Dim objMapping As String = resources.addXObject(xobject, xObjectPrefix)
            saveGraphicsState()
            appendRawCommands(SPACE)
            concatenate2CTM(transform)
            appendRawCommands(SPACE)
            appendRawCommands("/")
            appendRawCommands(objMapping)
            appendRawCommands(SPACE)
            appendRawCommands(XOBJECT_DO)
            restoreGraphicsState()
        End Sub

        '/**
        ' * The Td operator.
        ' * A current text matrix will be replaced with a new one (1 0 0 1 x y).
        ' * @param x The x coordinate.
        ' * @param y The y coordinate.
        ' * @throws IOException If there is an error writing to the stream.
        ' */
        Public Sub moveTextPositionByAmount(ByVal x As Single, ByVal y As Single) ' throws IOException
            If (Not inTextMode) Then
                Throw New IOException("Error: must call beginText() before moveTextPositionByAmount")
            End If
            appendRawCommands(x)
            appendRawCommands(SPACE)
            appendRawCommands(y)
            appendRawCommands(SPACE)
            appendRawCommands(MOVE_TEXT_POSITION)
        End Sub

        '/**
        ' * The Tm operator. Sets the text matrix to the given values.
        ' * A current text matrix will be replaced with the new one.
        ' * @param a The a value of the matrix.
        ' * @param b The b value of the matrix.
        ' * @param c The c value of the matrix.
        ' * @param d The d value of the matrix.
        ' * @param e The e value of the matrix.
        ' * @param f The f value of the matrix.
        ' * @throws IOException If there is an error writing to the stream.
        ' */
        Public Sub setTextMatrix(ByVal a As Double, ByVal b As Double, ByVal c As Double, ByVal d As Double, ByVal e As Double, ByVal f As Double) 'throws IOException
            If (Not inTextMode) Then
                Throw New IOException("Error: must call beginText() before setTextMatrix")
            End If
            appendRawCommands(a)
            appendRawCommands(SPACE)
            appendRawCommands(b)
            appendRawCommands(SPACE)
            appendRawCommands(c)
            appendRawCommands(SPACE)
            appendRawCommands(d)
            appendRawCommands(SPACE)
            appendRawCommands(e)
            appendRawCommands(SPACE)
            appendRawCommands(f)
            appendRawCommands(SPACE)
            appendRawCommands(SET_TEXT_MATRIX)
        End Sub

        '/**
        '* The Tm operator. Sets the text matrix to the given values.
        '* A current text matrix will be replaced with the new one.
        '* @param matrix the transformation matrix
        '* @throws IOException If there is an error writing to the stream.
        '*/
        Public Sub setTextMatrix(ByVal matrix As AffineTransform) 'throws IOException
            If (Not inTextMode) Then
                Throw New IOException("Error: must call beginText() before setTextMatrix")
            End If
            appendMatrix(matrix)
            appendRawCommands(SET_TEXT_MATRIX)
        End Sub

        '/**
        ' * The Tm operator. Sets the text matrix to the given scaling and translation values.
        ' * A current text matrix will be replaced with the new one.
        ' * @param sx The scaling factor in x-direction.
        ' * @param sy The scaling factor in y-direction.
        ' * @param tx The translation value in x-direction.
        ' * @param ty The translation value in y-direction.
        ' * @throws IOException If there is an error writing to the stream.
        ' */
        Public Sub setTextScaling(ByVal sx As Double, ByVal sy As Double, ByVal tx As Double, ByVal ty As Double)  'throws IOException
            setTextMatrix(sx, 0, 0, sy, tx, ty)
        End Sub

        '/**
        ' * The Tm operator. Sets the text matrix to the given translation values.
        ' * A current text matrix will be replaced with the new one.
        ' * @param tx The translation value in x-direction.
        ' * @param ty The translation value in y-direction.
        ' * @throws IOException If there is an error writing to the stream.
        ' */
        Public Sub setTextTranslation(ByVal tx As Double, ByVal ty As Double) 'throws IOException
            setTextMatrix(1, 0, 0, 1, tx, ty)
        End Sub

        '/**
        ' * The Tm operator. Sets the text matrix to the given rotation and translation values.
        ' * A current text matrix will be replaced with the new one.
        ' * @param angle The angle used for the counterclockwise rotation in radians.
        ' * @param tx The translation value in x-direction.
        ' * @param ty The translation value in y-direction.
        ' * @throws IOException If there is an error writing to the stream.
        ' */
        Public Sub setTextRotation(ByVal angle As Double, ByVal tx As Double, ByVal ty As Double) 'throws IOException
            Dim angleCos As Double = Math.Cos(angle)
            Dim angleSin As Double = Math.Sin(angle)
            setTextMatrix(angleCos, angleSin, -angleSin, angleCos, tx, ty)
        End Sub

        '/**
        ' * The Cm operator. Concatenates the current transformation matrix with the given values.
        ' * @param a The a value of the matrix.
        ' * @param b The b value of the matrix.
        ' * @param c The c value of the matrix.
        ' * @param d The d value of the matrix.
        ' * @param e The e value of the matrix.
        ' * @param f The f value of the matrix.
        ' * @throws IOException If there is an error writing to the stream.
        ' */
        Public Sub concatenate2CTM(ByVal a As Double, ByVal b As Double, ByVal c As Double, ByVal d As Double, ByVal e As Double, ByVal f As Double)  'throws IOException
            appendRawCommands(a)
            appendRawCommands(SPACE)
            appendRawCommands(b)
            appendRawCommands(SPACE)
            appendRawCommands(c)
            appendRawCommands(SPACE)
            appendRawCommands(d)
            appendRawCommands(SPACE)
            appendRawCommands(e)
            appendRawCommands(SPACE)
            appendRawCommands(f)
            appendRawCommands(SPACE)
            appendRawCommands(CONCATENATE_MATRIX)
        End Sub

        '/**
        ' * The Cm operator. Concatenates the current transformation matrix with the given
        ' * {@link AffineTransform}.
        ' * @param at the transformation matrix
        ' * @throws IOException If there is an error writing to the stream.
        ' */
        Public Sub concatenate2CTM(ByVal at As AffineTransform) 'throws IOException
            appendMatrix(at)
            appendRawCommands(CONCATENATE_MATRIX)
        End Sub

        '/**
        ' * This will draw a string at the current location on the screen.
        ' *
        ' * @param text The text to draw.
        ' * @throws IOException If an io exception occurs.
        ' */
        Public Sub drawString(ByVal text As String) 'throws IOException
            If (Not inTextMode) Then
                Throw New IOException("Error: must call beginText() before drawString")
            End If
            Dim [string] As COSString = New COSString(text)
            Dim buffer As ByteArrayOutputStream = New ByteArrayOutputStream()
            [string].writePDF(buffer)
            appendRawCommands(buffer.toByteArray())
            appendRawCommands(SPACE)
            appendRawCommands(SHOW_TEXT)
            buffer.Dispose()
        End Sub

        '/**
        ' * Set the stroking color space.  This will add the colorspace to the PDResources
        ' * if necessary.
        ' *
        ' * @param colorSpace The colorspace to write.
        ' * @throws IOException If there is an error writing the colorspace.
        ' */
        Public Sub setStrokingColorSpace(ByVal colorSpace As PDColorSpace) 'throws IOException
            currentStrokingColorSpace = colorSpace
            writeColorSpace(colorSpace)
            appendRawCommands(SET_STROKING_COLORSPACE)
        End Sub

        '/**
        ' * Set the stroking color space.  This will add the colorspace to the PDResources
        ' * if necessary.
        ' *
        ' * @param colorSpace The colorspace to write.
        ' * @throws IOException If there is an error writing the colorspace.
        ' */
        Public Sub setNonStrokingColorSpace(ByVal colorSpace As PDColorSpace) 'throws IOException
            currentNonStrokingColorSpace = colorSpace
            writeColorSpace(colorSpace)
            appendRawCommands(SET_NON_STROKING_COLORSPACE)
        End Sub

        Private Sub writeColorSpace(ByVal colorSpace As PDColorSpace) 'throws IOException
            Dim key As COSName = Nothing
            If (TypeOf (colorSpace) Is PDDeviceGray OrElse TypeOf (colorSpace) Is PDDeviceRGB OrElse TypeOf (colorSpace) Is PDDeviceCMYK) Then
                key = COSName.getPDFName(colorSpace.getName())
            Else
                Dim colorSpaces As COSDictionary = resources.getCOSDictionary().getDictionaryObject(COSName.COLORSPACE)
                If (colorSpaces Is Nothing) Then
                    colorSpaces = New COSDictionary()
                    resources.getCOSDictionary().setItem(COSName.COLORSPACE, colorSpaces)
                End If
                key = colorSpaces.getKeyForValue(colorSpace.getCOSObject())

                If (key Is Nothing) Then
                    Dim counter As Integer = 0
                    Dim csName As String = "CS"
                    While (colorSpaces.containsValue(csName + counter))
                        counter += 1
                    End While
                    key = COSName.getPDFName(csName + counter)
                    colorSpaces.setItem(key, colorSpace)
                End If
            End If
            key.writePDF(output)
            appendRawCommands(SPACE)
        End Sub


        '/**
        ' * Set the color components of current stroking colorspace.
        ' *
        ' * @param components The components to set for the current color.
        ' * @throws IOException If there is an error while writing to the stream.
        ' */
        Public Sub setStrokingColor(ByVal components() As Single) 'throws IOException
            For i As Integer = 0 To components.Length - 1
                appendRawCommands(components(i))
                appendRawCommands(SPACE)
            Next
            If (TypeOf (currentStrokingColorSpace) Is PDSeparation OrElse TypeOf (currentStrokingColorSpace) Is PDPattern OrElse TypeOf (currentStrokingColorSpace) Is PDDeviceN OrElse TypeOf (currentStrokingColorSpace) Is PDICCBased) Then
                appendRawCommands(SET_STROKING_COLOR_COMPLEX)
            Else
                appendRawCommands(SET_STROKING_COLOR_SIMPLE)
            End If
        End Sub

        '/**
        ' * Set the stroking color, specified as RGB.
        ' *
        ' * @param color The color to set.
        ' * @throws IOException If an IO error occurs while writing to the stream.
        ' */
        Public Sub setStrokingColor(ByVal color As JColor) 'throws IOException
            Dim colorSpace As ColorSpace = color.getColorSpace()
            If (colorSpace.getType() = colorSpace.TYPE_RGB) Then
                setStrokingColor(color.getRed(), color.getGreen(), color.getBlue())
            ElseIf (colorSpace.getType() = colorSpace.TYPE_GRAY) Then
                color.getColorComponents(colorComponents)
                setStrokingColor(colorComponents(0))
            ElseIf (colorSpace.getType() = colorSpace.TYPE_CMYK) Then
                color.getColorComponents(colorComponents)
                setStrokingColor(colorComponents(0), colorComponents(1), colorComponents(2), colorComponents(3))
            Else
                Throw New IOException("Error: unknown colorspace:" & colorSpace.ToString)
            End If
        End Sub

        '/**
        ' * Set the non stroking color, specified as RGB.
        ' *
        ' * @param color The color to set.
        ' * @throws IOException If an IO error occurs while writing to the stream.
        ' */
        Public Sub setNonStrokingColor(ByVal color As System.Drawing.Color) 'throws IOException
            Dim colorSpace As ColorSpace = colorSpace.FromColor(color) '.getColorSpace()
            If (colorSpace.getType() = colorSpace.TYPE_RGB) Then
                setNonStrokingColor(color.R, color.G, color.B)
            ElseIf (colorSpace.getType() = colorSpace.TYPE_GRAY) Then
                colorSpace.getColorComponents(color, colorComponents)
                setNonStrokingColor(colorComponents(0))
            ElseIf (colorSpace.getType() = colorSpace.TYPE_CMYK) Then
                colorSpace.getColorComponents(color, colorComponents)
                setNonStrokingColor(colorComponents(0), colorComponents(1), colorComponents(2), colorComponents(3))
            Else
                Throw New IOException("Error: unknown colorspace:" & colorSpace.ToString)
            End If
        End Sub

        '/**
        ' * Set the stroking color, specified as RGB, 0-255.
        ' *
        ' * @param r The red value.
        ' * @param g The green value.
        ' * @param b The blue value.
        ' * @throws IOException If an IO error occurs while writing to the stream.
        ' */
        Public Sub setStrokingColor(ByVal r As Integer, ByVal g As Integer, ByVal b As Integer) 'throws IOException
            appendRawCommands(r / 255D)
            appendRawCommands(SPACE)
            appendRawCommands(g / 255D)
            appendRawCommands(SPACE)
            appendRawCommands(b / 255D)
            appendRawCommands(SPACE)
            appendRawCommands(RG_STROKING)
        End Sub

        '/**
        ' * Set the stroking color, specified as CMYK, 0-255.
        ' *
        ' * @param c The cyan value.
        ' * @param m The magenta value.
        ' * @param y The yellow value.
        ' * @param k The black value.
        ' * @throws IOException If an IO error occurs while writing to the stream.
        ' */
        Public Sub setStrokingColor(ByVal c As Integer, ByVal m As Integer, ByVal y As Integer, ByVal k As Integer) 'throws IOException
            appendRawCommands(c / 255D)
            appendRawCommands(SPACE)
            appendRawCommands(m / 255D)
            appendRawCommands(SPACE)
            appendRawCommands(y / 255D)
            appendRawCommands(SPACE)
            appendRawCommands(k / 255D)
            appendRawCommands(SPACE)
            appendRawCommands(K_STROKING)
        End Sub

        '/**
        ' * Set the stroking color, specified as CMYK, 0.0-1.0.
        ' *
        ' * @param c The cyan value.
        ' * @param m The magenta value.
        ' * @param y The yellow value.
        ' * @param k The black value.
        ' * @throws IOException If an IO error occurs while writing to the stream.
        ' */
        Public Sub setStrokingColor(ByVal c As Double, ByVal m As Double, ByVal y As Double, ByVal k As Double) 'throws IOException
            appendRawCommands(c)
            appendRawCommands(SPACE)
            appendRawCommands(m)
            appendRawCommands(SPACE)
            appendRawCommands(y)
            appendRawCommands(SPACE)
            appendRawCommands(k)
            appendRawCommands(SPACE)
            appendRawCommands(K_STROKING)
        End Sub

        '/**
        ' * Set the stroking color, specified as grayscale, 0-255.
        ' *
        ' * @param g The gray value.
        ' * @throws IOException If an IO error occurs while writing to the stream.
        ' */
        Public Sub setStrokingColor(ByVal g As Integer) ' throws IOException
            appendRawCommands(g / 255D)
            appendRawCommands(SPACE)
            appendRawCommands(G_STROKING)
        End Sub

        '/**
        ' * Set the stroking color, specified as Grayscale 0.0-1.0.
        ' *
        ' * @param g The gray value.
        ' * @throws IOException If an IO error occurs while writing to the stream.
        ' */
        Public Sub setStrokingColor(ByVal g As Double) 'throws IOException
            appendRawCommands(g)
            appendRawCommands(SPACE)
            appendRawCommands(G_STROKING)
        End Sub

        '/**
        ' * Set the color components of current non stroking colorspace.
        ' *
        ' * @param components The components to set for the current color.
        ' * @throws IOException If there is an error while writing to the stream.
        ' */
        Public Sub setNonStrokingColor(ByVal components() As Single) 'throws IOException
            For i As Integer = 0 To components.Length - 1
                appendRawCommands(components(i))
                appendRawCommands(SPACE)
            Next
            If (TypeOf (currentNonStrokingColorSpace) Is PDSeparation OrElse TypeOf (currentNonStrokingColorSpace) Is PDPattern OrElse TypeOf (currentNonStrokingColorSpace) Is PDDeviceN OrElse TypeOf (currentNonStrokingColorSpace) Is PDICCBased) Then
                appendRawCommands(SET_NON_STROKING_COLOR_COMPLEX)
            Else
                appendRawCommands(SET_NON_STROKING_COLOR_SIMPLE)
            End If
        End Sub

        '/**
        ' * Set the non stroking color, specified as RGB, 0-255.
        ' *
        ' * @param r The red value.
        ' * @param g The green value.
        ' * @param b The blue value.
        ' * @throws IOException If an IO error occurs while writing to the stream.
        ' */
        Public Sub setNonStrokingColor(ByVal r As Integer, ByVal g As Integer, ByVal b As Integer) 'throws IOException
            appendRawCommands(r / 255D)
            appendRawCommands(SPACE)
            appendRawCommands(g / 255D)
            appendRawCommands(SPACE)
            appendRawCommands(b / 255D)
            appendRawCommands(SPACE)
            appendRawCommands(RG_NON_STROKING)
        End Sub

        '/**
        ' * Set the non stroking color, specified as CMYK, 0-255.
        ' *
        ' * @param c The cyan value.
        ' * @param m The magenta value.
        ' * @param y The yellow value.
        ' * @param k The black value.
        ' * @throws IOException If an IO error occurs while writing to the stream.
        ' */
        Public Sub setNonStrokingColor(ByVal c As Integer, ByVal m As Integer, ByVal y As Integer, ByVal k As Integer) 'throws IOException
            appendRawCommands(c / 255D)
            appendRawCommands(SPACE)
            appendRawCommands(m / 255D)
            appendRawCommands(SPACE)
            appendRawCommands(y / 255D)
            appendRawCommands(SPACE)
            appendRawCommands(k / 255D)
            appendRawCommands(SPACE)
            appendRawCommands(K_NON_STROKING)
        End Sub

        '/**
        ' * Set the non stroking color, specified as CMYK, 0.0-1.0.
        ' *
        ' * @param c The cyan value.
        ' * @param m The magenta value.
        ' * @param y The yellow value.
        ' * @param k The black value.
        ' * @throws IOException If an IO error occurs while writing to the stream.
        ' */
        Public Sub setNonStrokingColor(ByVal c As Double, ByVal m As Double, ByVal y As Double, ByVal k As Double) 'throws IOException
            appendRawCommands(c)
            appendRawCommands(SPACE)
            appendRawCommands(m)
            appendRawCommands(SPACE)
            appendRawCommands(y)
            appendRawCommands(SPACE)
            appendRawCommands(k)
            appendRawCommands(SPACE)
            appendRawCommands(K_NON_STROKING)
        End Sub

        '/**
        ' * Set the non stroking color, specified as grayscale, 0-255.
        ' *
        ' * @param g The gray value.
        ' * @throws IOException If an IO error occurs while writing to the stream.
        ' */
        Public Sub setNonStrokingColor(ByVal g As Integer) 'throws IOException
            appendRawCommands(g / 255D)
            appendRawCommands(SPACE)
            appendRawCommands(G_NON_STROKING)
        End Sub

        '/**
        ' * Set the non stroking color, specified as Grayscale 0.0-1.0.
        ' *
        ' * @param g The gray value.
        ' * @throws IOException If an IO error occurs while writing to the stream.
        ' */
        Public Sub setNonStrokingColor(ByVal g As Double) 'throws IOException
            appendRawCommands(g)
            appendRawCommands(SPACE)
            appendRawCommands(G_NON_STROKING)
        End Sub

        '/**
        ' * Add a rectangle to the current path.
        ' *
        ' * @param x The lower left x coordinate.
        ' * @param y The lower left y coordinate.
        ' * @param width The width of the rectangle.
        ' * @param height The height of the rectangle.
        ' * @throws IOException If there is an error while drawing on the screen.
        ' */
        Public Sub addRect(ByVal x As Single, ByVal y As Single, ByVal width As Single, ByVal height As Single) 'throws IOException
            If (inTextMode) Then
                Throw New IOException("Error: addRect is not allowed within a text block.")
            End If
            appendRawCommands(x)
            appendRawCommands(SPACE)
            appendRawCommands(y)
            appendRawCommands(SPACE)
            appendRawCommands(width)
            appendRawCommands(SPACE)
            appendRawCommands(height)
            appendRawCommands(SPACE)
            appendRawCommands(RECTANGLE)
        End Sub

        '/**
        ' * Draw a rectangle on the page using the current non stroking color.
        ' *
        ' * @param x The lower left x coordinate.
        ' * @param y The lower left y coordinate.
        ' * @param width The width of the rectangle.
        ' * @param height The height of the rectangle.
        ' * @throws IOException If there is an error while drawing on the screen.
        ' */
        Public Sub fillRect(ByVal x As Single, ByVal y As Single, ByVal width As Single, ByVal height As Single) 'throws IOException
            If (inTextMode) Then
                Throw New IOException("Error: fillRect is not allowed within a text block.")
            End If
            addRect(x, y, width, height)
            fill(PathIterator.Fields.WIND_NON_ZERO)
        End Sub

        '/**
        ' * Append a cubic Bézier curve to the current path. The curve extends from the current
        ' * point to the point (x3 , y3 ), using (x1 , y1 ) and (x2 , y2 ) as the Bézier control points
        ' * @param x1 x coordinate of the point 1
        ' * @param y1 y coordinate of the point 1
        ' * @param x2 x coordinate of the point 2
        ' * @param y2 y coordinate of the point 2
        ' * @param x3 x coordinate of the point 3
        ' * @param y3 y coordinate of the point 3
        ' * @throws IOException If there is an error while adding the .
        ' */
        Public Sub addBezier312(ByVal x1 As Single, ByVal y1 As Single, ByVal x2 As Single, ByVal y2 As Single, ByVal x3 As Single, ByVal y3 As Single) 'throws IOException
            If (inTextMode) Then
                Throw New IOException("Error: addBezier312 is not allowed within a text block.")
            End If
            appendRawCommands(x1)
            appendRawCommands(SPACE)
            appendRawCommands(y1)
            appendRawCommands(SPACE)
            appendRawCommands(x2)
            appendRawCommands(SPACE)
            appendRawCommands(y2)
            appendRawCommands(SPACE)
            appendRawCommands(x3)
            appendRawCommands(SPACE)
            appendRawCommands(y3)
            appendRawCommands(SPACE)
            appendRawCommands(BEZIER_312)
        End Sub

        '/**
        ' * Append a cubic Bézier curve to the current path. The curve extends from the current
        ' * point to the point (x3 , y3 ), using the current point and (x2 , y2 ) as the Bézier control points
        ' * @param x2 x coordinate of the point 2
        ' * @param y2 y coordinate of the point 2
        ' * @param x3 x coordinate of the point 3
        ' * @param y3 y coordinate of the point 3
        ' * @throws IOException If there is an error while adding the .
        ' */
        Public Sub addBezier32(ByVal x2 As Single, ByVal y2 As Single, ByVal x3 As Single, ByVal y3 As Single) 'throws IOException
            If (inTextMode) Then
                Throw New IOException("Error: addBezier32 is not allowed within a text block.")
            End If
            appendRawCommands(x2)
            appendRawCommands(SPACE)
            appendRawCommands(y2)
            appendRawCommands(SPACE)
            appendRawCommands(x3)
            appendRawCommands(SPACE)
            appendRawCommands(y3)
            appendRawCommands(SPACE)
            appendRawCommands(BEZIER_32)
        End Sub

        '/**
        ' * Append a cubic Bézier curve to the current path. The curve extends from the current
        ' * point to the point (x3 , y3 ), using (x1 , y1 ) and (x3 , y3 ) as the Bézier control points
        ' * @param x1 x coordinate of the point 1
        ' * @param y1 y coordinate of the point 1
        ' * @param x3 x coordinate of the point 3
        ' * @param y3 y coordinate of the point 3
        ' * @throws IOException If there is an error while adding the .
        ' */
        Public Sub addBezier31(ByVal x1 As Single, ByVal y1 As Single, ByVal x3 As Single, ByVal y3 As Single) 'throws IOException
            If (inTextMode) Then
                Throw New IOException("Error: addBezier31 is not allowed within a text block.")
            End If
            appendRawCommands(x1)
            appendRawCommands(SPACE)
            appendRawCommands(y1)
            appendRawCommands(SPACE)
            appendRawCommands(x3)
            appendRawCommands(SPACE)
            appendRawCommands(y3)
            appendRawCommands(SPACE)
            appendRawCommands(BEZIER_313)
        End Sub

        '/**
        ' * Add a line to the given coordinate.
        ' *
        ' * @param x The x coordinate.
        ' * @param y The y coordinate.
        ' * @throws IOException If there is an error while adding the line.
        ' */
        Public Sub moveTo(ByVal x As Single, ByVal y As Single) 'throws IOException
            If (inTextMode) Then
                Throw New IOException("Error: moveTo is not allowed within a text block.")
            End If
            appendRawCommands(x)
            appendRawCommands(SPACE)
            appendRawCommands(y)
            appendRawCommands(SPACE)
            appendRawCommands(MOVE_TO)
        End Sub

        '/**
        ' * Add a move to the given coordinate.
        ' *
        ' * @param x The x coordinate.
        ' * @param y The y coordinate.
        ' * @throws IOException If there is an error while adding the line.
        ' */
        Public Sub lineTo(ByVal x As Single, ByVal y As Single) 'throws IOException
            If (inTextMode) Then
                Throw New IOException("Error: lineTo is not allowed within a text block.")
            End If
            appendRawCommands(x)
            appendRawCommands(SPACE)
            appendRawCommands(y)
            appendRawCommands(SPACE)
            appendRawCommands(LINE_TO)
        End Sub

        '/**
        ' * add a line to the current path.
        ' *
        ' * @param xStart The start x coordinate.
        ' * @param yStart The start y coordinate.
        ' * @param xEnd The end x coordinate.
        ' * @param yEnd The end y coordinate.
        ' * @throws IOException If there is an error while adding the line.
        ' */
        Public Sub addLine(ByVal xStart As Single, ByVal yStart As Single, ByVal xEnd As Single, ByVal yEnd As Single) 'throws IOException
            If (inTextMode) Then
                Throw New IOException("Error: addLine is not allowed within a text block.")
            End If
            ' moveTo
            moveTo(xStart, yStart)
            ' lineTo
            lineTo(xEnd, yEnd)
        End Sub

        '/**
        ' * Draw a line on the page using the current non stroking color and the current line width.
        ' *
        ' * @param xStart The start x coordinate.
        ' * @param yStart The start y coordinate.
        ' * @param xEnd The end x coordinate.
        ' * @param yEnd The end y coordinate.
        ' * @throws IOException If there is an error while drawing on the screen.
        ' */
        Public Sub drawLine(ByVal xStart As Single, ByVal yStart As Single, ByVal xEnd As Single, ByVal yEnd As Single) 'throws IOException
            If (inTextMode) Then
                Throw New IOException("Error: drawLine is not allowed within a text block.")
            End If
            addLine(xStart, yStart, xEnd, yEnd)
            ' stroke
            Me.stroke()
        End Sub

        '/**
        ' * Add a polygon to the current path.
        ' * @param x x coordinate of each points
        ' * @param y y coordinate of each points
        ' * @throws IOException If there is an error while drawing on the screen.
        ' */
        Public Sub addPolygon(ByVal x() As Single, ByVal y() As Single) 'throws IOException
            If (inTextMode) Then
                Throw New IOException("Error: addPolygon is not allowed within a text block.")
            End If
            If (x.Length <> y.Length) Then
                Throw New IOException("Error: some points are missing coordinate")
            End If
            moveTo(x(0), y(0))
            For i As Integer = 1 To x.Length - 1
                lineTo(x(i), y(i))
            Next
            closeSubPath()
        End Sub

        '/**
        ' * Draw a polygon on the page using the current non stroking color.
        ' * @param x x coordinate of each points
        ' * @param y y coordinate of each points
        ' * @throws IOException If there is an error while drawing on the screen.
        ' */
        Public Sub drawPolygon(ByVal x() As Single, ByVal y() As Single) 'throws IOException
            If (inTextMode) Then
                Throw New IOException("Error: drawPolygon is not allowed within a text block.")
            End If
            addPolygon(x, y)
            stroke()
        End Sub

        '/**
        ' * Draw and fill a polygon on the page using the current non stroking color.
        ' * @param x x coordinate of each points
        ' * @param y y coordinate of each points
        ' * @throws IOException If there is an error while drawing on the screen.
        ' */
        Public Sub fillPolygon(ByVal x() As Single, ByVal y() As Single) 'throws IOException
            If (inTextMode) Then
                Throw New IOException("Error: fillPolygon is not allowed within a text block.")
            End If
            addPolygon(x, y)
            fill(PathIterator.Fields.WIND_NON_ZERO)
        End Sub

        '/**
        ' * Stroke the path.
        ' * 
        ' * @throws IOException If there is an error while stroking the path.
        ' */
        Public Sub stroke() 'throws IOException
            If (inTextMode) Then
                Throw New IOException("Error: stroke is not allowed within a text block.")
            End If
            appendRawCommands(PDPageContentStream._STROKE)
        End Sub

        '/**
        ' * Close and stroke the path.
        ' * 
        ' * @throws IOException If there is an error while closing and stroking the path.
        ' */
        Public Sub closeAndStroke() 'throws IOException
            If (inTextMode) Then
                Throw New IOException("Error: closeAndStroke is not allowed within a text block.")
            End If
            appendRawCommands(CLOSE_STROKE)
        End Sub

        '/**
        ' * Fill the path.
        ' * 
        ' * @param windingRule the winding rule to be used for filling 
        ' * 
        ' * @throws IOException If there is an error while filling the path.
        ' */
        Public Sub fill(ByVal windingRule As Integer) 'throws IOException
            If (inTextMode) Then
                Throw New IOException("Error: fill is not allowed within a text block.")
            End If
            If (windingRule = PathIterator.Fields.WIND_NON_ZERO) Then
                appendRawCommands(FILL_NON_ZERO)
            ElseIf (windingRule = PathIterator.Fields.WIND_EVEN_ODD) Then
                appendRawCommands(FILL_EVEN_ODD)
            Else
                Throw New IOException("Error: unknown value for winding rule")
            End If
        End Sub

        '/**
        ' * Close subpath.
        ' * 
        ' * @throws IOException If there is an error while closing the subpath.
        ' */
        Public Sub closeSubPath() 'throws IOException
            If (inTextMode) Then
                Throw New IOException("Error: closeSubPath is not allowed within a text block.")
            End If
            appendRawCommands(CLOSE_SUBPATH)
        End Sub

        '/**
        ' * Clip path.
        ' * 
        ' * @param windingRule the winding rule to be used for clipping
        ' *  
        ' * @throws IOException If there is an error while clipping the path.
        ' */
        Public Sub clipPath(ByVal windingRule As Integer) 'throws IOException
            If (inTextMode) Then
                Throw New IOException("Error: clipPath is not allowed within a text block.")
            End If
            If (windingRule = PathIterator.Fields.WIND_NON_ZERO) Then
                appendRawCommands(CLIP_PATH_NON_ZERO)
                appendRawCommands(NOP)
            ElseIf (windingRule = PathIterator.Fields.WIND_EVEN_ODD) Then
                appendRawCommands(CLIP_PATH_EVEN_ODD)
                appendRawCommands(NOP)
            Else
                Throw New IOException("Error: unknown value for winding rule")
            End If
        End Sub

        '/**
        ' * Set linewidth to the given value.
        ' *
        ' * @param lineWidth The width which is used for drwaing.
        ' * @throws IOException If there is an error while drawing on the screen.
        ' */
        Public Sub setLineWidth(ByVal lineWidth As Single) 'throws IOException
            If (inTextMode) Then
                Throw New IOException("Error: setLineWidth is not allowed within a text block.")
            End If
            appendRawCommands(lineWidth)
            appendRawCommands(SPACE)
            appendRawCommands(LINE_WIDTH)
        End Sub


        Public Enum LineJoinStyleEnum As Integer
            MITER_JOIN = 0
            ROUND_JOIN = 1
            BEVEL_JOIN = 2
        End Enum


        ''' <summary>
        ''' Set the line join style.
        ''' </summary>
        ''' <param name="lineJoinStyle">0 for miter join, 1 for round join, and 2 for bevel join.</param>
        ''' <remarks></remarks>
        Public Sub setLineJoinStyle(ByVal lineJoinStyle As LineJoinStyleEnum) 'throws IOException
            If (inTextMode) Then
                Throw New IOException("Error: setLineJoinStyle is not allowed within a text block.")
            End If
            If (lineJoinStyle >= 0 AndAlso lineJoinStyle <= 2) Then
                appendRawCommands(CStr(lineJoinStyle))
                appendRawCommands(SPACE)
                appendRawCommands(LINE_JOIN_STYLE)
            Else
                Throw New IOException("Error: unknown value for line join style")
            End If
        End Sub

        Public Enum LineCapStyleEnum As Integer
            BUTT_CAP = 0
            ROUND_CAP = 1
            PROJECTING_SQUARE_CAP = 2
        End Enum

        '/**
        ' * Set the line cap style.
        ' * @param lineCapStyle 0 for butt cap, 1 for round cap, and 2 for projecting square cap.
        ' * @throws IOException If there is an error while writing to the stream.
        ' */
        Public Sub setLineCapStyle(ByVal lineCapStyle As LineCapStyleEnum) 'throws IOException
            If (inTextMode) Then
                Throw New IOException("Error: setLineCapStyle is not allowed within a text block.")
            End If
            If (lineCapStyle >= 0 AndAlso lineCapStyle <= 2) Then
                appendRawCommands(CStr(lineCapStyle))
                appendRawCommands(SPACE)
                appendRawCommands(LINE_CAP_STYLE)
            Else
                Throw New IOException("Error: unknown value for line cap style")
            End If
        End Sub

        '/**
        ' * Set the line dash pattern.
        ' * @param pattern The pattern array
        ' * @param phase The phase of the pattern
        ' * @throws IOException If there is an error while writing to the stream.
        ' */
        Public Sub setLineDashPattern(ByVal pattern() As Single, ByVal phase As Single) 'throws IOException
            If (inTextMode) Then
                Throw New IOException("Error: setLineDashPattern is not allowed within a text block.")
            End If
            appendRawCommands(OPENING_BRACKET)
            For Each value As Single In pattern
                appendRawCommands(value)
                appendRawCommands(SPACE)
            Next
            appendRawCommands(CLOSING_BRACKET)
            appendRawCommands(SPACE)
            appendRawCommands(phase)
            appendRawCommands(SPACE)
            appendRawCommands(LINE_DASH_PATTERN)
        End Sub

        '/**
        ' * Begin a marked content sequence.
        ' * @param tag the tag
        ' * @throws IOException if an I/O error occurs
        ' */
        Public Sub beginMarkedContentSequence(ByVal tag As COSName) 'throws IOException
            appendCOSName(tag)
            appendRawCommands(SPACE)
            appendRawCommands(BMC)
        End Sub

        '/**
        ' * Begin a marked content sequence with a reference to an entry in the page resources'
        ' * Properties dictionary.
        ' * @param tag the tag
        ' * @param propsName the properties reference
        ' * @throws IOException if an I/O error occurs
        ' */
        Public Sub beginMarkedContentSequence(ByVal tag As COSName, ByVal propsName As COSName) 'throws IOException
            appendCOSName(tag)
            appendRawCommands(SPACE)
            appendCOSName(propsName)
            appendRawCommands(SPACE)
            appendRawCommands(BDC)
        End Sub

        '/**
        ' * End a marked content sequence.
        ' * @throws IOException if an I/O error occurs
        ' */
        Public Sub endMarkedContentSequence() 'throws IOException
            appendRawCommands(EMC)
        End Sub

        '/**
        ' * q operator. Saves the current graphics state.
        ' * @throws IOException If an error occurs while writing to the stream.
        ' */
        Public Sub saveGraphicsState() 'throws IOException
            appendRawCommands(SAVE_GRAPHICS_STATE)
        End Sub

        '/**
        ' * Q operator. Restores the current graphics state.
        ' * @throws IOException If an error occurs while writing to the stream.
        ' */
        Public Sub restoreGraphicsState() 'throws IOException
            appendRawCommands(RESTORE_GRAPHICS_STATE)
        End Sub

        '/**
        ' * This will append raw commands to the content stream.
        ' *
        ' * @param commands The commands to append to the stream.
        ' * @throws IOException If an error occurs while writing to the stream.
        ' */
        Public Sub appendRawCommands(ByVal commands As String) 'throws IOException
            appendRawCommands(Sistema.Strings.GetBytes(commands, "ISO-8859-1"))
        End Sub

        '/**
        ' * This will append raw commands to the content stream.
        ' *
        ' * @param commands The commands to append to the stream.
        ' * @throws IOException If an error occurs while writing to the stream.
        ' */
        Public Sub appendRawCommands(ByVal commands() As Byte) 'throws IOException
            output.Write(commands)
        End Sub

        '/**
        ' * This will append raw commands to the content stream.
        ' *
        ' * @param data Append a raw byte to the stream.
        ' *
        ' * @throws IOException If an error occurs while writing to the stream.
        ' */
        Public Sub appendRawCommands(ByVal data As Integer) 'throws IOException
            output.Write(data)
        End Sub

        '/**
        ' * This will append raw commands to the content stream.
        ' *
        ' * @param data Append a formatted double value to the stream.
        ' *
        ' * @throws IOException If an error occurs while writing to the stream.
        ' */
        Public Sub appendRawCommands(ByVal data As Double) 'throws IOException
            appendRawCommands(formatDecimal.format(data))
        End Sub

        '/**
        ' * This will append raw commands to the content stream.
        ' *
        ' * @param data Append a formatted Single value to the stream.
        ' *
        ' * @throws IOException If an error occurs while writing to the stream.
        ' */
        Public Sub appendRawCommands(ByVal data As Single) 'throws IOException
            appendRawCommands(formatDecimal.format(data))
        End Sub

        '/**
        ' * This will append a {@link COSName} to the content stream.
        ' * @param name the name
        ' * @throws IOException If an error occurs while writing to the stream.
        ' */
        Public Sub appendCOSName(ByVal name As COSName) 'throws IOException
            name.writePDF(output)
        End Sub

        Private Sub appendMatrix(ByVal transform As AffineTransform) 'throws IOException
            Dim values() As Double = Array.CreateInstance(GetType(Double), 6)
            transform.getMatrix(values)
            For Each v As Double In values
                appendRawCommands(v)
                appendRawCommands(SPACE)
            Next
        End Sub

        '/**
        ' * Close the content stream.  This must be called when you are done with this
        ' * object.
        ' * @throws IOException If the underlying stream has a problem being written to.
        ' */
        Public Sub close() 'throws IOException
            output.Close()
            currentNonStrokingColorSpace = Nothing
            currentStrokingColorSpace = Nothing
            page = Nothing
            resources = Nothing
        End Sub
    End Class

End Namespace

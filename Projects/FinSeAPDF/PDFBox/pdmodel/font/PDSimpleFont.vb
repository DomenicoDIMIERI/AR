Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.IO
Imports FinSeA.Exceptions
Imports FinSeA.Drawings
Imports FinSeA.org.apache.fontbox.afm

Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.encoding
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.util

Namespace org.apache.pdfbox.pdmodel.font

    '/**
    ' * This class contains implementation details of the simple pdf fonts.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.18 $
    ' */
    Public MustInherit Class PDSimpleFont
        Inherits PDFont

        Private mFontSizes As HashMap(Of NInteger, NFloat) = New HashMap(Of NInteger, NFloat)(128)

        Private avgFontWidth As Single = 0.0F
        Private avgFontHeight As Single = 0.0F
        Private fontWidthOfSpace As Single = -1.0F

        Private Shared ReadOnly SPACE_BYTES() As Byte = {32}


        '/**
        ' * Log instance.
        ' */
        'private static final Log LOG = LogFactory.getLog(PDSimpleFont.class);

        '/**
        ' * Constructor.
        ' */
        Public Sub New()
            MyBase.New()
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param fontDictionary The font dictionary according to the PDF specification.
        ' */
        Public Sub New(ByVal fontDictionary As COSDictionary)
            MyBase.New(fontDictionary)
        End Sub

        '/**
        '* Looks up, creates, returns  the AWT Font.
        '* 
        '* @return returns the awt font to bes used for rendering 
        '* @throws IOException if something went wrong.
        '*/
        Public Overridable Function getawtFont() As JFont ' throws IOException
            'LOG.error("Not yet implemented:" + getClass().getName() );
            Throw New NotImplementedException
            'Return Nothing
        End Function

        Public Overrides Sub drawString(ByVal [string] As String, ByVal codePoints() As Integer, ByVal g As Graphics2D, ByVal fontSize As Single, ByVal at As AffineTransform, ByVal x As Single, ByVal y As Single) 'throws IOException
            Dim awtFont As JFont = getawtFont()
            Dim frc As FontRenderContext = New FontRenderContext(New AffineTransform(), True, True)
            Dim glyphs As GlyphVector = Nothing
            Dim useCodepoints As Boolean = codePoints IsNot Nothing AndAlso isType0Font()
            Dim descendantFont As PDFont
            If (useCodepoints) Then
                descendantFont = CType(Me, PDType0Font).getDescendantFont()
            Else
                descendantFont = Nothing
            End If

            ' symbolic fonts may trigger the same fontmanager.so/dll error as described below
            If (useCodepoints AndAlso Not descendantFont.getFontDescriptor().isSymbolic()) Then
                Dim cid2Font As PDCIDFontType2Font = Nothing
                If (TypeOf (descendantFont) Is PDCIDFontType2Font) Then
                    cid2Font = descendantFont
                End If
                If ((cid2Font IsNot Nothing AndAlso cid2Font.hasCIDToGIDMap()) OrElse isFontSubstituted()) Then
                    ' we still have to use the string if a CIDToGIDMap is used 
                    glyphs = awtFont.createGlyphVector(frc, [string])
                Else
                    glyphs = awtFont.createGlyphVector(frc, codePoints)
                End If
            Else
                ' mdavis - fix fontmanager.so/dll on sun.font.FileFont.getGlyphImage
                ' for font with bad cmaps?
                ' Type1 fonts are not affected as they don't have cmaps
                If (Not isType1Font() AndAlso awtFont.canDisplayUpTo([string]) <> -1) Then
                    LOG.warn("Changing font on <" & [string] & "> from <" & awtFont.getName() & "> to the default font")
                    awtFont = JFont.decode(Nothing).deriveFont(1.0F)
                End If
                glyphs = awtFont.createGlyphVector(frc, [string])
            End If
            Dim g2d As FinSeA.Drawings.Graphics2D = g 'Graphics2D 
            g2d.setRenderingHint(RenderingHints.KEY_ANTIALIASING, RenderingHints.VALUE_ANTIALIAS_ON)
            writeFont(g2d, at, x, y, glyphs)
        End Sub

        '/**
        ' * This will get the font height for a character.
        ' *
        ' * @param c The character code to get the width for.
        ' * @param offset The offset into the array.
        ' * @param length The length of the data.
        ' *
        ' * @return The width is in 1000 unit of text space, ie 333 or 777
        ' *
        ' * @throws IOException If an error occurs while parsing.
        ' */
        Public Overrides Function getFontHeight(ByVal c() As Byte, ByVal offset As Integer, ByVal length As Integer) As Single 'throws IOException
            ' maybe there is already a precalculated value
            If (avgFontHeight > 0) Then
                Return avgFontHeight
            End If
            Dim retval As Single = 0
            Dim metric As FontMetric = getAFM()
            If (metric IsNot Nothing) Then
                Dim code As Integer = getCodeFromArray(c, offset, length)
                Dim encoding As pdfbox.encoding.Encoding = getFontEncoding()
                Dim characterName As String = encoding.getName(code)
                retval = metric.getCharacterHeight(characterName)
            Else
                Dim desc As PDFontDescriptor = getFontDescriptor()
                If (desc IsNot Nothing) Then
                    '// the following values are all more or less accurate
                    '// at least all are average values. Maybe we'll find
                    '// another way to get those value for every single glyph
                    '// in the future if needed
                    Dim fontBBox As PDRectangle = desc.getFontBoundingBox()
                    If (fontBBox IsNot Nothing) Then
                        retval = fontBBox.getHeight() / 2
                    End If
                    If (retval = 0) Then
                        retval = desc.getCapHeight()
                    End If
                    If (retval = 0) Then
                        retval = desc.getAscent()
                    End If
                    If (retval = 0) Then
                        retval = desc.getXHeight()
                        If (retval > 0) Then
                            retval -= desc.getDescent()
                        End If
                    End If
                    avgFontHeight = retval
                End If
            End If
            Return retval
        End Function

        '/**
        ' * This will get the font width for a character.
        ' *
        ' * @param c The character code to get the width for.
        ' * @param offset The offset into the array.
        ' * @param length The length of the data.
        ' *
        ' * @return The width is in 1000 unit of text space, ie 333 or 777
        ' *
        ' * @throws IOException If an error occurs while parsing.
        ' */
        Public Overrides Function getFontWidth(ByVal c() As Byte, ByVal offset As Integer, ByVal length As Integer) As Single 'throws IOException
            Dim code As Integer = getCodeFromArray(c, offset, length)
            Dim fontWidth As NFloat = mFontSizes.get(code)
            If (fontWidth.HasValue = False) Then
                fontWidth = getFontWidth(code)
                If (fontWidth <= 0) Then
                    'hmm should this be in PDType1Font??
                    fontWidth = getFontWidthFromAFMFile(code)
                End If
                mFontSizes.put(code, fontWidth)
            End If
            Return fontWidth
        End Function

        '/**
        ' * This will get the average font width for all characters.
        ' *
        ' * @return The width is in 1000 unit of text space, ie 333 or 777
        ' *
        ' * @throws IOException If an error occurs while parsing.
        ' */
        Public Overrides Function getAverageFontWidth() As Single ' throws IOException
            Dim average As Single = 0.0F

            'AJW
            If (avgFontWidth <> 0.0F) Then
                average = avgFontWidth
            Else
                Dim totalWidth As Single = 0.0F
                Dim characterCount As Single = 0.0F
                Dim widths As COSArray = font.getDictionaryObject(COSName.WIDTHS)
                If (widths IsNot Nothing) Then
                    For i As Integer = 0 To widths.size() - 1
                        Dim fontWidth As COSNumber = widths.getObject(i)
                        If (fontWidth.floatValue() > 0) Then
                            totalWidth += fontWidth.floatValue()
                            characterCount += 1
                        End If
                    Next
                End If

                If (totalWidth > 0) Then
                    average = totalWidth / characterCount
                Else
                    average = getAverageFontWidthFromAFMFile()
                End If
                avgFontWidth = average
            End If
            Return average
        End Function


        '/**
        ' * This will get the ToUnicode object.
        ' *
        ' * @return The ToUnicode object.
        ' */
        Public Function getToUnicode() As COSBase
            Return font.getDictionaryObject(COSName.TO_UNICODE)
        End Function

        '/**
        ' * This will set the ToUnicode object.
        ' *
        ' * @param unicode The unicode object.
        ' */
        Public Sub setToUnicode(ByVal unicode As COSBase)
            font.setItem(COSName.TO_UNICODE, unicode)
        End Sub

        '/**
        ' * This will get the fonts bounding box.
        ' *
        ' * @return The fonts bouding box.
        ' *
        ' * @throws IOException If there is an error getting the bounding box.
        ' */
        Public Overrides Function getFontBoundingBox() As PDRectangle 'throws IOException
            Return getFontDescriptor().getFontBoundingBox()
        End Function

        '/**
        ' * This will draw a string on a canvas using the font.
        ' *
        ' * @param g2d The graphics to draw onto.
        ' * @param at The transformation matrix with all information for scaling and shearing of the font.
        ' * @param x The x coordinate to draw at.
        ' * @param y The y coordinate to draw at.
        ' * @param glyphs The GlyphVector containing the glyphs to be drawn.
        ' *
        ' */
        Protected Sub writeFont(ByVal g2d As Graphics2D, ByVal at As AffineTransform, ByVal x As Single, ByVal y As Single, ByVal glyphs As GlyphVector)
            ' check if we have a rotation
            If (Not at.isIdentity()) Then
                Try
                    Dim atInv As AffineTransform = at.createInverse()
                    ' do only apply the size of the transform, rotation will be realized by rotating the graphics,
                    ' otherwise the hp printers will not render the font
                    ' apply the transformation to the graphics, which should be the same as applying the
                    ' transformation itself to the text
                    g2d.Transform(at)
                    ' translate the coordinates
                    Dim newXy As New PointF(x, y) 'Point2D.NFloat 
                    atInv.transform(New PointF(x, y), newXy) 'Point2D.NFloat
                    g2d.drawGlyphVector(glyphs, newXy.X, newXy.Y)
                    ' restore the original transformation
                    g2d.Transform(atInv)
                Catch e As NoninvertibleTransformException
                    LOG.error("Error in PDSimpleFont.writeFont", e)
                End Try
            Else
                g2d.drawGlyphVector(glyphs, x, y)
            End If
        End Sub

        Protected Overrides Sub determineEncoding()
            Dim cmapName As String = ""
            Dim encodingName As COSName = Nothing
            Dim encoding As COSBase = getEncoding()
            Dim fontEncoding As pdfbox.encoding.Encoding = Nothing
            If (encoding IsNot Nothing) Then
                If (TypeOf (encoding) Is COSName) Then
                    If (cmap Is Nothing) Then
                        encodingName = encoding
                        cmap = cmapObjects.get(encodingName.getName())
                        If (cmap Is Nothing) Then
                            cmapName = encodingName.getName()
                        End If
                    End If
                    If (cmap Is Nothing AndAlso cmapName <> "") Then
                        Try
                            fontEncoding = EncodingManager.INSTANCE.getEncoding(encodingName)
                        Catch exception As IOException
                            LOG.debug("Debug: Could not find encoding for " & encodingName.toString)
                        End Try
                    End If
                ElseIf (TypeOf (encoding) Is COSStream) Then
                    If (cmap Is Nothing) Then
                        Dim encodingStream As COSStream = encoding
                        Try
                            cmap = parseCmap(Nothing, encodingStream.getUnfilteredStream())
                        Catch exception As IOException
                            LOG.error("Error: Could not parse the embedded CMAP")
                        End Try
                    End If
                ElseIf (TypeOf (encoding) Is COSDictionary) Then
                    Try
                        fontEncoding = New DictionaryEncoding(encoding)
                    Catch exception As IOException
                        LOG.error("Error: Could not create the DictionaryEncoding")
                    End Try
                End If
            End If
            setFontEncoding(fontEncoding)
            extractToUnicodeEncoding()

            If (cmap Is Nothing AndAlso cmapName <> "") Then
                Dim resourceName As String = resourceRootCMAP & cmapName
                Try
                    cmap = parseCmap(resourceRootCMAP, ResourceLoader.loadResource(resourceName))
                    If (cmap Is Nothing AndAlso encodingName Is Nothing) Then
                        LOG.error("Error: Could not parse predefined CMAP file for '" & cmapName & "'")
                    End If
                Catch exception As IOException
                    LOG.error("Error: Could not find predefined CMAP file for '" & cmapName & "'")
                End Try
            End If
        End Sub

        Private Sub extractToUnicodeEncoding()
            Dim encodingName As COSName = Nothing
            Dim cmapName As String = ""
            Dim toUnicode As COSBase = getToUnicode()
            If (toUnicode IsNot Nothing) Then
                setHasToUnicode(True)
                If (TypeOf (toUnicode) Is COSStream) Then
                    Try
                        toUnicodeCmap = parseCmap(resourceRootCMAP, DirectCast(toUnicode, COSStream).getUnfilteredStream())
                    Catch exception As IOException
                        LOG.error("Error: Could not load embedded ToUnicode CMap")
                    End Try
                ElseIf (TypeOf (toUnicode) Is COSName) Then
                    encodingName = toUnicode
                    toUnicodeCmap = cmapObjects.get(encodingName.getName())
                    If (toUnicodeCmap Is Nothing) Then
                        cmapName = encodingName.getName()
                        Dim resourceName As String = resourceRootCMAP & cmapName
                        Try
                            toUnicodeCmap = parseCmap(resourceRootCMAP, ResourceLoader.loadResource(resourceName))
                        Catch exception As IOException
                            LOG.error("Error: Could not find predefined ToUnicode CMap file for '" & cmapName & "'")
                        End Try
                        If (toUnicodeCmap Is Nothing) Then
                            LOG.error("Error: Could not parse predefined ToUnicode CMap file for '" & cmapName & "'")
                        End If
                    End If
                End If
            End If
        End Sub

        Private _isFontSubstituted As Boolean = False

        '/**
        ' * This will get the value for isFontSubstituted, which indicates
        ' * if the font was substituted due to a problem with the embedded one.
        ' * 
        ' * @return true if the font was substituted
        ' */
        Protected Friend Function isFontSubstituted() As Boolean
            Return Me._isFontSubstituted
        End Function

        '/**
        ' * This will set  the value for isFontSubstituted.
        ' * 
        ' * @param isSubstituted true if the font was substituted
        ' */
        Protected Sub setIsFontSubstituted(ByVal isSubstituted As Boolean)
            Me._isFontSubstituted = isSubstituted
        End Sub

        Public Overrides Function getSpaceWidth() As Single
            If (fontWidthOfSpace = -1.0F) Then
                Dim toUnicode As COSBase = getToUnicode()
                Try
                    If (toUnicode IsNot Nothing) Then
                        Dim spaceMapping As Integer = toUnicodeCmap.getSpaceMapping()
                        If (spaceMapping > -1) Then
                            fontWidthOfSpace = getFontWidth(spaceMapping)
                        End If
                    Else
                        fontWidthOfSpace = getFontWidth(SPACE_BYTES, 0, 1)
                    End If
                    ' use the average font width as fall back
                    If (fontWidthOfSpace <= 0) Then
                        fontWidthOfSpace = getAverageFontWidth()
                    End If
                Catch e As Exception
                    LOG.error("Can't determine the width of the space character using 250 as default", e)
                    fontWidthOfSpace = 250.0F
                End Try
            End If
            Return fontWidthOfSpace
        End Function


    End Class

End Namespace
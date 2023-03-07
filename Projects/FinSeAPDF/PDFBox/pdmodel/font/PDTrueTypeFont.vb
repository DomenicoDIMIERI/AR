Imports System.Drawing
Imports System.IO
Imports FinSeA.Io
Imports FinSeA.Drawings

Imports FinSeA.org.apache.fontbox.ttf
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.encoding
Imports FinSeA.org.apache.pdfbox.pdmodel
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.util
Imports FinSeA.Exceptions

Namespace org.apache.pdfbox.pdmodel.font


    '/**
    ' * This is the TrueType implementation of fonts.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * 
    ' */
    Public Class PDTrueTypeFont
        Inherits PDSimpleFont

        '/**
        ' * Log instance.
        ' */
        'private static final Log log = LogFactory.getLog(PDTrueTypeFont.class);

        '/**
        ' * This is the key to a property in the PDFBox_External_Fonts.properties
        ' * file to load a Font when a mapping does not exist for the current font.
        ' */
        Public Const UNKNOWN_FONT As String = "UNKNOWN_FONT"

        Private awtFont As JFont = Nothing

        Private Shared externalFonts As Properties = New Properties()
        Private Shared loadedExternalFonts As Map(Of String, TrueTypeFont) = New HashMap(Of String, TrueTypeFont)

        Shared Sub New()
            Try
                ResourceLoader.loadProperties("org/apache/pdfbox/resources/PDFBox_External_Fonts.properties", externalFonts)
            Catch io As IOException
                Throw New RuntimeException("Error loading font resources", io)
            End Try
        End Sub


        '/**
        ' * Constructor.
        ' */
        Public Sub New()
            MyBase.New()
            font.setItem(COSName.SUBTYPE, COSName.TRUE_TYPE)
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param fontDictionary The font dictionary according to the PDF specification.
        ' * 
        ' * @throws IOException exception if something went wrong when loading the font.
        ' */
        Public Sub New(ByVal fontDictionary As COSDictionary) 'throws IOException
            MyBase.New(fontDictionary)
            ensureFontDescriptor()
        End Sub

        '/**
        ' * This will load a TTF font from a font file.
        ' *
        ' * @param doc The PDF document that will hold the embedded font.
        ' * @param file The file on the filesystem that holds the font file.
        ' * @return A true type font.
        ' * @throws IOException If there is an error loading the file data.
        ' */
        Public Shared Function loadTTF(ByVal doc As PDDocument, ByVal file As String) As PDTrueTypeFont 'throws IOException
            Return loadTTF(doc, New FinSeA.Io.File(file))
        End Function

        '/**
        ' * This will load a TTF to be embedded into a document.
        ' *
        ' * @param doc The PDF document that will hold the embedded font.
        ' * @param file a ttf file.
        ' * @return a PDTrueTypeFont instance.
        ' * @throws IOException If there is an error loading the data.
        ' */
        Public Shared Function loadTTF(ByVal doc As PDDocument, ByVal file As FinSeA.Io.File) As PDTrueTypeFont  'throws IOException
            Dim [is] As New FileInputStream(file.getFullName)
            Dim ret As PDTrueTypeFont = loadTTF(doc, [is])
            [is].Dispose()
            Return ret
        End Function

        '/**
        ' * This will load a TTF to be embedded into a document.
        ' *
        ' * @param doc The PDF document that will hold the embedded font.
        ' * @param stream a ttf input stream.
        ' * @return a PDTrueTypeFont instance.
        ' * @throws IOException If there is an error loading the data.
        ' */
        Public Shared Function loadTTF(ByVal doc As PDDocument, ByVal stream As InputStream) As PDTrueTypeFont 'throws IOException
            Return PDTrueTypeFont.loadTTF(doc, stream, New WinAnsiEncoding())
        End Function

        '/**
        ' * This will load a TTF to be embedded into a document.
        ' *
        ' * @param doc The PDF document that will hold the embedded font.
        ' * @param stream a ttf input stream.
        ' * @param enc The font encoding.
        ' * @return a PDTrueTypeFont instance.
        ' * @throws IOException If there is an error loading the data.
        ' */
        Public Shared Function loadTTF(ByVal doc As PDDocument, ByVal stream As InputStream, ByVal enc As pdfbox.encoding.Encoding) As PDTrueTypeFont 'throws IOException
            Dim fontStream As PDStream = New PDStream(doc, stream, False)
            fontStream.getStream().setInt(COSName.LENGTH1, fontStream.getByteArray().Length)
            fontStream.addCompression()
            'only support winansi encoding right now, should really
            'just use Identity-H with unicode mapping
            Return PDTrueTypeFont.loadTTF(fontStream, enc)
        End Function

        '/**
        ' * This will load a TTF to be embedded into a document.
        ' *
        ' * @param fontStream a ttf input stream.
        ' * @param enc The font encoding.
        ' * @return a PDTrueTypeFont instance.
        ' * @throws IOException If there is an error loading the data.
        ' */
        Public Shared Function loadTTF(ByVal fontStream As PDStream, ByVal enc As pdfbox.encoding.Encoding) As PDTrueTypeFont 'throws IOException
            Dim retval As PDTrueTypeFont = New PDTrueTypeFont()
            retval.setFontEncoding(enc)
            retval.setEncoding(enc.getCOSObject())

            Dim fd As PDFontDescriptorDictionary = New PDFontDescriptorDictionary()
            retval.setFontDescriptor(fd)
            fd.setFontFile2(fontStream)
            ' As the stream was close within the PDStream constructor, we have to recreate it
            Dim stream As InputStream = fontStream.createInputStream()
            Try
                retval.loadDescriptorDictionary(fd, stream)
            Finally
                stream.Close()
            End Try
            Return retval
        End Function

        Private Sub ensureFontDescriptor() 'throws IOException
            If (getFontDescriptor() Is Nothing) Then
                Dim fdd As PDFontDescriptorDictionary = New PDFontDescriptorDictionary()
                setFontDescriptor(fdd)
                Dim ttfData As InputStream = getExternalTTFData()
                If (ttfData Is Nothing) Then
                    Try
                        loadDescriptorDictionary(fdd, ttfData)
                    Finally
                        ttfData.Close()
                    End Try
                End If
            End If
        End Sub

        Private Sub loadDescriptorDictionary(ByVal fd As PDFontDescriptorDictionary, ByVal ttfData As InputStream) 'throws IOException
            Dim ttf As TrueTypeFont = Nothing
            Try
                Dim parser As TTFParser = New TTFParser()
                ttf = parser.parseTTF(ttfData)
                Dim naming As NamingTable = ttf.getNaming()
                Dim records As List(Of NameRecord) = naming.getNameRecords()
                For i As Integer = 0 To records.size() - 1
                    Dim nr As NameRecord = records.get(i)
                    If (nr.getNameId() = NameRecord.NAME_POSTSCRIPT_NAME) Then
                        setBaseFont(nr.getString())
                        fd.setFontName(nr.getString())
                    ElseIf (nr.getNameId() = NameRecord.NAME_FONT_FAMILY_NAME) Then
                        fd.setFontFamily(nr.getString())
                    End If
                Next

                Dim os2 As OS2WindowsMetricsTable = ttf.getOS2Windows()
                Dim isSymbolic As Boolean = False

                Select Case (os2.getFamilyClass())
                    Case OS2WindowsMetricsTable.FAMILY_CLASS_SYMBOLIC : isSymbolic = True
                    Case OS2WindowsMetricsTable.FAMILY_CLASS_SCRIPTS : fd.setScript(True)
                    Case OS2WindowsMetricsTable.FAMILY_CLASS_CLAREDON_SERIFS, _
                         OS2WindowsMetricsTable.FAMILY_CLASS_FREEFORM_SERIFS, _
                         OS2WindowsMetricsTable.FAMILY_CLASS_MODERN_SERIFS, _
                         OS2WindowsMetricsTable.FAMILY_CLASS_OLDSTYLE_SERIFS, _
                         OS2WindowsMetricsTable.FAMILY_CLASS_SLAB_SERIFS
                        fd.setSerif(True)
                    Case Else
                        'do nothing
                End Select
                Select Case (os2.getWidthClass())
                    Case OS2WindowsMetricsTable.WIDTH_CLASS_ULTRA_CONDENSED : fd.setFontStretch("UltraCondensed")
                    Case OS2WindowsMetricsTable.WIDTH_CLASS_EXTRA_CONDENSED : fd.setFontStretch("ExtraCondensed")
                    Case OS2WindowsMetricsTable.WIDTH_CLASS_CONDENSED : fd.setFontStretch("Condensed")
                    Case OS2WindowsMetricsTable.WIDTH_CLASS_SEMI_CONDENSED : fd.setFontStretch("SemiCondensed")
                    Case OS2WindowsMetricsTable.WIDTH_CLASS_MEDIUM : fd.setFontStretch("Normal")
                    Case OS2WindowsMetricsTable.WIDTH_CLASS_SEMI_EXPANDED : fd.setFontStretch("SemiExpanded")
                    Case OS2WindowsMetricsTable.WIDTH_CLASS_EXPANDED : fd.setFontStretch("Expanded")
                    Case OS2WindowsMetricsTable.WIDTH_CLASS_EXTRA_EXPANDED : fd.setFontStretch("ExtraExpanded")
                    Case OS2WindowsMetricsTable.WIDTH_CLASS_ULTRA_EXPANDED : fd.setFontStretch("UltraExpanded")
                    Case Else
                        'do nothing
                End Select
                fd.setFontWeight(os2.getWeightClass())
                fd.setSymbolic(isSymbolic)
                fd.setNonSymbolic(Not isSymbolic)

                'todo retval.setItalic
                '/todo retval.setAllCap
                'todo retval.setSmallCap
                'todo retval.setForceBold

                Dim header As HeaderTable = ttf.getHeader()
                Dim rect As PDRectangle = New PDRectangle()
                Dim scaling As Single = 1000.0F / header.getUnitsPerEm()
                rect.setLowerLeftX(header.getXMin() * scaling)
                rect.setLowerLeftY(header.getYMin() * scaling)
                rect.setUpperRightX(header.getXMax() * scaling)
                rect.setUpperRightY(header.getYMax() * scaling)
                fd.setFontBoundingBox(rect)

                Dim hHeader As HorizontalHeaderTable = ttf.getHorizontalHeader()
                fd.setAscent(hHeader.getAscender() * scaling)
                fd.setDescent(hHeader.getDescender() * scaling)

                Dim glyphTable As GlyphTable = ttf.getGlyph()
                Dim glyphs() As GlyphData = glyphTable.getGlyphs()

                Dim ps As PostScriptTable = ttf.getPostScript()
                fd.setFixedPitch(ps.getIsFixedPitch() > 0)
                fd.setItalicAngle(ps.getItalicAngle())

                Dim names() As String = ps.getGlyphNames()

                If (names IsNot Nothing) Then
                    For i As Integer = 0 To names.Length - 1
                        '                   //if we have a capital H then use that, otherwise use the
                        'tallest letter
                        If (names(i).Equals("H")) Then
                            fd.setCapHeight(glyphs(i).getBoundingBox().getUpperRightY() / scaling)
                        End If
                        If (names(i).Equals("x")) Then
                            fd.setXHeight(glyphs(i).getBoundingBox().getUpperRightY() / scaling)
                        End If
                    Next
                End If

                'hmm there does not seem to be a clear definition for StemV,
                'this is close enough and I am told it doesn't usually get used.
                fd.setStemV((fd.getFontBoundingBox().getWidth() * 0.129999995F))

                Dim cmapTable As CMAPTable = ttf.getCMAP()
                Dim cmaps() As CMAPEncodingEntry = cmapTable.getCmaps()
                Dim uniMap As CMAPEncodingEntry = Nothing

                For i As Integer = 0 To cmaps.Length - 1
                    If (cmaps(i).getPlatformId() = cmapTable.PLATFORM_WINDOWS) Then
                        Dim platformEncoding As Integer = cmaps(i).getPlatformEncodingId()
                        If (cmapTable.ENCODING_UNICODE = platformEncoding) Then
                            uniMap = cmaps(i)
                            Exit For
                        End If
                    End If
                Next

                Dim codeToName As Map(Of NInteger, String) = Me.getFontEncoding().getCodeToNameMap()

                Dim firstChar As Integer = Math.Min(codeToName.keySet())
                Dim lastChar As Integer = Math.Max(codeToName.keySet())

                Dim hMet As HorizontalMetricsTable = ttf.getHorizontalMetrics()
                Dim widthValues() As Integer = hMet.getAdvanceWidth()
                ' some monospaced fonts provide only one value for the width 
                ' instead of an array containing the same value for every glyphid 
                Dim isMonospaced As Boolean = fd.isFixedPitch()
                Dim nWidths As Integer = lastChar - firstChar + 1
                Dim widths As List(Of NFloat) = New ArrayList(Of NFloat)(nWidths)
                ' use the first width as default
                ' proportional fonts -> width of the .notdef character
                ' monospaced-fonts -> the first width
                Dim defaultWidth As Integer = Math.round(widthValues(0) * scaling)
                For i As Integer = 0 To nWidths - 1
                    widths.add(defaultWidth)
                Next
                ' Encoding singleton to have acces to the chglyph name to
                ' unicode cpoint point mapping of Adobe's glyphlist.txt
                Dim glyphlist As pdfbox.encoding.Encoding = WinAnsiEncoding.INSTANCE

                ' A character code is mapped to a glyph name via the provided
                ' font encoding. Afterwards, the glyph name is translated to a
                ' glyph ID.
                ' For details, see PDFReference16.pdf, Section 5.5.5, p.401
                '
                For Each e As Map.Entry(Of NInteger, String) In codeToName.entrySet()
                    Dim name As String = e.Value
                    ' pdf code to unicode by glyph list.
                    Dim c As String = glyphlist.getCharacter(name)
                    Dim charCode As Integer = Convert.ToInt32(c.Chars(0))
                    Dim gid As Integer = uniMap.getGlyphId(charCode)
                    If (gid <> 0) Then
                        If (isMonospaced) Then
                            widths.set(e.Key.intValue() - firstChar, defaultWidth)
                        Else
                            widths.set(e.Key.intValue() - firstChar, Math.round(widthValues(gid) * scaling))
                        End If
                    End If
                Next
                setWidths(widths)
                setFirstChar(firstChar)
                setLastChar(lastChar)
            Finally
                If (ttf IsNot Nothing) Then
                    ttf.close()
                End If
            End Try
        End Sub

        Public Overrides Function getawtFont() As JFont ' IOException
            Dim fd As PDFontDescriptorDictionary = getFontDescriptor()
            If (awtFont Is Nothing) Then
                Dim ff2Stream As PDStream = fd.getFontFile2()
                If (ff2Stream IsNot Nothing) Then
                    Try
                        ' create a font with the embedded data
                        awtFont = JFont.createFont(font.TRUETYPE_FONT, ff2Stream.createInputStream())
                    Catch f As FontFormatException
                        Try
                            ' as a workaround we try to rebuild the embedded subsfont
                            Dim fontData() As Byte = rebuildTTF(fd, ff2Stream.createInputStream())
                            If (fontData IsNot Nothing) Then
                                Dim bais As ByteArrayInputStream = New ByteArrayInputStream(fontData)
                                awtFont = JFont.createFont(font.TRUETYPE_FONT, bais)
                                bais.Dispose()
                            End If
                        Catch e As FontFormatException
                            LOG.info("Can't read the embedded font " & fd.getFontName())
                        End Try
                    End Try
                    If (awtFont Is Nothing) Then
                        awtFont = FontManager.getAwtFont(fd.getFontName())
                        If (awtFont IsNot Nothing) Then
                            LOG.info("Using font " & awtFont.getName() & " instead")
                        End If
                        setIsFontSubstituted(True)
                    End If
                Else
                    ' check if the font is part of our environment
                    awtFont = FontManager.getAwtFont(fd.getFontName())
                    If (awtFont Is Nothing) Then
                        LOG.info("Can't find the specified font " & fd.getFontName())
                        ' check if there is a font mapping for an external font file
                        Dim ttf As TrueTypeFont = getExternalFontFile2(fd)
                        If (ttf IsNot Nothing) Then
                            Try
                                awtFont = JFont.createFont(font.TRUETYPE_FONT, ttf.getOriginalData())
                            Catch f As FontFormatException
                                LOG.info("Can't read the external fontfile " & fd.getFontName())
                            End Try
                        End If
                    End If
                End If
                If (awtFont Is Nothing) Then
                    ' we can't find anything, so we have to use the standard font
                    awtFont = FontManager.getStandardFont()
                    LOG.info("Using font " & awtFont.getName() & " instead")
                    setIsFontSubstituted(True)
                End If
            End If
            Return awtFont
        End Function

        Private Function rebuildTTF(ByVal fd As PDFontDescriptorDictionary, ByVal inputStream As InputStream) As Byte() ' throws IOException
            ' this is one possible case of an incomplete subfont which leads to a font exception
            If (TypeOf (getFontEncoding()) Is WinAnsiEncoding) Then
                Dim ttfParser As TTFParser = New TTFParser(True)
                Dim ttf As TrueTypeFont = ttfParser.parseTTF(inputStream)
                Dim ttfSub As TTFSubFont = New TTFSubFont(ttf, "PDFBox-Rebuild")
                For i As Integer = getFirstChar() To getLastChar()
                    ttfSub.addCharCode(i)
                Next
                Dim baos As ByteArrayOutputStream = New ByteArrayOutputStream()
                ttfSub.writeToStream(baos)
                Return baos.toByteArray()
            End If
            Return Nothing
        End Function

        Private Function getExternalTTFData() As InputStream ' throws IOException
            Dim ttfResource As String = externalFonts.getProperty(UNKNOWN_FONT)
            Dim baseFont As String = getBaseFont()
            If (baseFont IsNot Nothing AndAlso externalFonts.containsKey(baseFont)) Then
                ttfResource = externalFonts.getProperty(baseFont)
            End If
            If (ttfResource IsNot Nothing) Then
                Return ResourceLoader.loadResource(ttfResource)
            Else
                Return Nothing
            End If
        End Function

        '/**
        ' * Permit to load an external TTF Font program file
        ' *
        ' * Created by Pascal Allain
        ' * Vertical7 Inc.
        ' *
        ' * @param fd The font descriptor currently used
        ' * @return A PDStream with the Font File program, null if fd is null
        ' * @throws IOException If the font is not found
        ' */
        Private Function getExternalFontFile2(ByVal fd As PDFontDescriptorDictionary) As TrueTypeFont  'throws IOException
            Dim retval As TrueTypeFont = Nothing

            If (fd IsNot Nothing) Then
                Dim baseFont As String = getBaseFont()
                Dim fontResource As String = externalFonts.getProperty(UNKNOWN_FONT)
                If ((baseFont IsNot Nothing) AndAlso (externalFonts.containsKey(baseFont))) Then
                    fontResource = externalFonts.getProperty(baseFont)
                End If
                If (fontResource IsNot Nothing) Then
                    retval = loadedExternalFonts.get(baseFont)
                    If (retval Is Nothing) Then
                        Dim ttfParser As TTFParser = New TTFParser()
                        Dim fontStream As InputStream = ResourceLoader.loadResource(fontResource)
                        If (fontStream Is Nothing) Then
                            Throw New IOException("Error missing font resource '" & externalFonts.get(baseFont) & "'")
                        End If
                        retval = ttfParser.parseTTF(fontStream)
                        loadedExternalFonts.put(baseFont, retval)
                    End If
                End If
            End If

            Return retval
        End Function


    End Class

End Namespace

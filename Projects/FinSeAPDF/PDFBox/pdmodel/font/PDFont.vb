Imports System.IO
Imports FinSeA.Io
Imports FinSeA.Exceptions
Imports FinSeA.org.apache.fontbox.afm
Imports FinSeA.org.apache.fontbox.cmap
Imports FinSeA.org.apache.pdfbox.encoding
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.util
Imports System.Drawing
Imports FinSeA.Drawings
Imports FinSeA.Text

Namespace org.apache.pdfbox.pdmodel.font


    '/**
    ' * This is the base class for all PDF fonts.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * 
    ' */
    Public MustInherit Class PDFont
        Implements COSObjectable

        ''' <summary>
        ''' The cos dictionary for this font.
        ''' </summary>
        ''' <remarks></remarks>
        Protected font As COSDictionary

        ''' <summary>
        ''' This is only used if this is a font object and it has an encoding.
        ''' </summary>
        ''' <remarks></remarks>
        Private fontEncoding As pdfbox.encoding.Encoding = Nothing

        ''' <summary>
        ''' The descriptor of the font.
        ''' </summary>
        ''' <remarks></remarks>
        Private fontDescriptor As PDFontDescriptor = Nothing

        ''' <summary>
        ''' The font matrix.
        ''' </summary>
        ''' <remarks></remarks>
        Protected fontMatrix As PDMatrix = Nothing

        ''' <summary>
        ''' This is only used if this is a font object and it has an encoding and it is a type0 font with a cmap.
        ''' </summary>
        ''' <remarks></remarks>
        Protected cmap As CMap = Nothing

        ''' <summary>
        ''' The CMap holding the ToUnicode mapping.
        ''' </summary>
        ''' <remarks></remarks>
        Protected toUnicodeCmap As CMap = Nothing

        Private _hasToUnicode As Boolean = False

        Protected Shared cmapObjects As Map(Of String, CMap) = Collections.synchronizedMap(New HashMap(Of String, CMap))

        '/**
        ' *  A list a floats representing the widths.
        ' */
        Private widths As List(Of NFloat) = Nothing

        ''' <summary>
        ''' The static map of the default Adobe font metrics.
        ''' </summary>
        ''' <remarks></remarks>
        Private Shared afmObjects As Map(Of String, FontMetric) = Collections.unmodifiableMap(Of String, FontMetric)(getAdobeFontMetrics())

        ''' <summary>
        ''' TODO move the Map to PDType1Font as these are the 14 Standard fonts which are definitely Type 1 fonts
        ''' </summary>
        ''' <remarks></remarks>
        Private Shared Function getAdobeFontMetrics() As Map(Of String, FontMetric)
            Dim metrics As New HashMap(Of String, FontMetric)
            addAdobeFontMetric(metrics, "Courier-Bold")
            addAdobeFontMetric(metrics, "Courier-BoldOblique")
            addAdobeFontMetric(metrics, "Courier")
            addAdobeFontMetric(metrics, "Courier-Oblique")
            addAdobeFontMetric(metrics, "Helvetica")
            addAdobeFontMetric(metrics, "Helvetica-Bold")
            addAdobeFontMetric(metrics, "Helvetica-BoldOblique")
            addAdobeFontMetric(metrics, "Helvetica-Oblique")
            addAdobeFontMetric(metrics, "Symbol")
            addAdobeFontMetric(metrics, "Times-Bold")
            addAdobeFontMetric(metrics, "Times-BoldItalic")
            addAdobeFontMetric(metrics, "Times-Italic")
            addAdobeFontMetric(metrics, "Times-Roman")
            addAdobeFontMetric(metrics, "ZapfDingbats")
            Return metrics
        End Function

        Protected Const resourceRootCMAP As String = "org/apache/pdfbox/resources/cmap/"
        Private Const resourceRootAFM As String = "org/apache/pdfbox/resources/afm/"

        Private Shared Sub addAdobeFontMetric(ByVal metrics As Map(Of String, FontMetric), ByVal name As String)
            Dim afmStream As InputStream = Nothing
#If Not Debug Then
            Try
#End If
            Dim resource As String = resourceRootAFM & name & ".afm"
            afmStream = ResourceLoader.loadResource(resource)
            If (afmStream IsNot Nothing) Then
                Dim parser As AFMParser = New AFMParser(afmStream)
                parser.parse()
                metrics.put(name, parser.getResult())
            End If
#If Not Debug Then
            Catch e As Exception
                ' ignore
            Finally
#End If
            If (afmStream IsNot Nothing) Then afmStream.Dispose()
#If Not Debug Then
End Try
#End If
        End Sub

        '/**
        ' * This will clear AFM resources that are stored statically.
        ' * This is usually not a problem unless you want to reclaim
        ' * resources for a long running process.
        ' *
        ' * SPECIAL NOTE: The font calculations are currently in COSObject, which
        ' * is where they will reside until PDFont is mature enough to take them over.
        ' * PDFont is the appropriate place for them and not in COSObject but we need font
        ' * calculations for text extraction.  THIS METHOD WILL BE MOVED OR REMOVED
        ' * TO ANOTHER LOCATION IN A FUTURE VERSION OF PDFBOX.
        ' */
        Public Shared Sub clearResources()
            cmapObjects.clear()
        End Sub

        '/**
        ' * Constructor.
        ' */
        Public Sub New()
            font = New COSDictionary()
            font.setItem(COSName.TYPE, COSName.FONT)
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param fontDictionary The font dictionary according to the PDF specification.
        ' */
        Public Sub New(ByVal fontDictionary As COSDictionary)
            font = fontDictionary
            determineEncoding()
        End Sub

        '/**
        ' * This will get the font descriptor for this font.
        ' *
        ' * @return The font descriptor for this font.
        ' *
        ' */
        Public Function getFontDescriptor() As PDFontDescriptor
            If (fontDescriptor Is Nothing) Then
                Dim fd As COSDictionary = font.getDictionaryObject(COSName.FONT_DESC)
                If (fd IsNot Nothing) Then
                    fontDescriptor = New PDFontDescriptorDictionary(fd)
                Else
                    getAFM()
                    If (afm IsNot Nothing) Then
                        fontDescriptor = New PDFontDescriptorAFM(afm)
                    End If
                End If
            End If
            Return fontDescriptor
        End Function

        '/**
        ' * This will set the font descriptor.
        ' *
        ' * @param fdDictionary The font descriptor.
        ' */
        Public Sub setFontDescriptor(ByVal fdDictionary As PDFontDescriptorDictionary)
            Dim dic As COSDictionary = Nothing
            If (fdDictionary IsNot Nothing) Then

                dic = fdDictionary.getCOSDictionary()
            End If
            font.setItem(COSName.FONT_DESC, dic)
            fontDescriptor = fdDictionary
        End Sub

        '/**
        ' * Determines the encoding for the font.
        ' * This method as to be overwritten, as there are different
        ' * possibilities to define a mapping.
        ' */
        Protected MustOverride Sub determineEncoding()

        '/**
        ' * {@inheritDoc}
        ' */
        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return font
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
        Public MustOverride Function getFontWidth(ByVal c() As Byte, ByVal offset As Integer, ByVal length As Integer) As Single ' throws IOException;

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
        Public MustOverride Function getFontHeight(ByVal c() As Byte, ByVal offset As Integer, ByVal length As Integer) As Single ' throws IOException;

        '/**
        ' * This will get the width of this string for this font.
        ' *
        ' * @param string The string to get the width of.
        ' *
        ' * @return The width of the string in 1000 units of text space, ie 333 567...
        ' *
        ' * @throws IOException If there is an error getting the width information.
        ' */
        Public Overridable Function getStringWidth(ByVal [string] As String) As Single 'throws IOException
            Dim data() As Byte = Sistema.Strings.GetBytes([string], "ISO-8859-1")
            Dim totalWidth As String = 0
            For i As Integer = 0 To data.Length - 1
                totalWidth += getFontWidth(data, i, 1)
            Next
            Return totalWidth
        End Function

        '/**
        ' * This will get the average font width for all characters.
        ' *
        ' * @return The width is in 1000 unit of text space, ie 333 or 777
        ' *
        ' * @throws IOException If an error occurs while parsing.
        ' */
        Public MustOverride Function getAverageFontWidth() As Single ' throws IOException;

        '/**
        ' * This will draw a string on a canvas using the font.
        ' *
        ' * @param string The string to draw.
        ' * @param g The graphics to draw onto.
        ' * @param fontSize The size of the font to draw.
        ' * @param at The transformation matrix with all information for scaling and shearing of the font.
        ' * @param x The x coordinate to draw at.
        ' * @param y The y coordinate to draw at.
        ' *
        ' * @throws IOException If there is an error drawing the specific string.
        ' * @deprecated use {@link PDFont#drawString(String, int[], Graphics, Single, AffineTransform, Single, Single)} instead
        ' */
        Public Sub drawString(ByVal [string] As String, ByVal g As Graphics2D, ByVal fontSize As Single, ByVal at As AffineTransform, ByVal x As Single, ByVal y As Single) 'throws IOException
            drawString([string], Nothing, g, fontSize, at, x, y)
        End Sub

        '/**
        ' * This will draw a string on a canvas using the font.
        ' *
        ' * @param string The string to draw.
        ' * @param codePoints The codePoints of the given string.
        ' * @param g The graphics to draw onto.
        ' * @param fontSize The size of the font to draw.
        ' * @param at The transformation matrix with all information for scaling and shearing of the font.
        ' * @param x The x coordinate to draw at.
        ' * @param y The y coordinate to draw at.
        ' *
        ' * @throws IOException If there is an error drawing the specific string.
        ' */
        Public MustOverride Sub drawString(ByVal [string] As String, ByVal codePoints() As Integer, ByVal g As Graphics2D, ByVal fontSize As Single, ByVal at As AffineTransform, ByVal x As Single, ByVal y As Single) 'throws IOException;

        '/**
        ' * Used for multibyte encodings.
        ' *
        ' * @param data The array of data.
        ' * @param offset The offset into the array.
        ' * @param length The number of bytes to use.
        ' *
        ' * @return The int value of data from the array.
        ' */
        Public Function getCodeFromArray(ByVal data() As Byte, ByVal offset As Integer, ByVal length As Integer) As Integer
            Dim code As Integer = 0
            For i As Integer = 0 To length - 1
                code <<= 8
                code = code Or (data(offset + i) + 256) Mod 256
            Next
            Return code
        End Function

        '/**
        ' * This will attempt to get the font width from an AFM file.
        ' *
        ' * @param code The character code we are trying to get.
        ' *
        ' * @return The font width from the AFM file.
        ' *
        ' * @throws IOException if we cannot find the width.
        ' */
        Protected Function getFontWidthFromAFMFile(ByVal code As Integer) As Single 'throws IOException
            Dim retval As Single = 0
            Dim metric As FontMetric = getAFM()
            If (metric IsNot Nothing) Then
                Dim characterName As String = fontEncoding.getName(code)
                retval = metric.getCharacterWidth(characterName)
            End If
            Return retval
        End Function

        '/**
        ' * This will attempt to get the average font width from an AFM file.
        ' *
        ' * @return The average font width from the AFM file.
        ' *
        ' * @throws IOException if we cannot find the width.
        ' */
        Protected Function getAverageFontWidthFromAFMFile() As Single ' throws IOException
            Dim retval As Single = 0
            Dim metric As FontMetric = getAFM()
            If (metric IsNot Nothing) Then
                retval = metric.getAverageCharacterWidth()
            End If
            Return retval
        End Function

        '/**
        ' * This will get an AFM object if one exists.
        ' *
        ' * @return The afm object from the name.
        ' *
        ' */
        Protected Function getAFM() As FontMetric
            If (isType1Font() AndAlso afm Is Nothing) Then
                Dim baseFont As COSBase = font.getDictionaryObject(COSName.BASE_FONT)
                Dim name As String = ""
                If (TypeOf (baseFont) Is COSName) Then
                    name = DirectCast(baseFont, COSName).getName()
                    If (name.IndexOf("+") > -1) Then
                        name = name.Substring(name.IndexOf("+") + 1)
                    End If

                ElseIf (TypeOf (baseFont) Is COSString) Then
                    Dim [string] As COSString = baseFont
                    name = [string].getString()
                    If (name <> "") Then
                        afm = afmObjects.get(name)
                    End If
                End If
            End If

            Return afm
        End Function

        Private afm As FontMetric = Nothing

        Private encoding As COSBase = Nothing

        '/**
        '     * cache the {@link COSName#ENCODING} object from
        '     * the font's dictionary since it is called so often.
        '     * <p>
        '     * Use this method instead of
        '     * <pre>
        '     *   font.getDictionaryObject(COSName.ENCODING);
        '     * </pre>
        '     * @return the encoding
        '     */
        Protected Function getEncoding() As COSBase
            If (encoding Is Nothing) Then
                encoding = font.getDictionaryObject(COSName.ENCODING)
            End If
            Return encoding
        End Function

        '/**
        ' * Set the encoding object from the fonts dictionary.
        ' * @param encodingValue the given encoding.
        ' */
        Protected Sub setEncoding(ByVal encodingValue As COSBase)
            font.setItem(COSName.ENCODING, encodingValue)
            encoding = encodingValue
        End Sub

        '/**
        ' * Encode the given value using the CMap of the font.
        ' *
        ' * @param code the code to encode.
        ' * @param length the byte length of the given code.
        ' * @param isCIDFont indicates that the used font is a CID font.
        ' *
        ' * @return The value of the encoded character.
        ' * @throws IOException if something went wrong
        ' */
        Protected Friend Function cmapEncoding(ByVal code As Integer, ByVal length As Integer, ByVal isCIDFont As Boolean, ByVal sourceCmap As CMap) As String 'throws IOException
            Dim retval As String = ""
            '// there is not sourceCmap if this is a descendant font
            If (sourceCmap Is Nothing) Then
                sourceCmap = cmap
            End If
            If (sourceCmap IsNot Nothing) Then
                retval = sourceCmap.lookup(code, length)
                If (retval = "" AndAlso isCIDFont) Then
                    retval = sourceCmap.lookupCID(code)
                End If
            End If
            Return retval
        End Function

        '/**
        '     * This will perform the encoding of a character if needed.
        '     *
        '     * @param c The character to encode.
        '     * @param offset The offset into the array to get the data
        '     * @param length The number of bytes to read.
        '     *
        '     * @return The value of the encoded character.
        '     *
        '     * @throws IOException If there is an error during the encoding.
        '     */
        Public Overridable Function encode(ByVal c() As Byte, ByVal offset As Integer, ByVal length As Integer) As String ' throws IOException
            Dim retval As String = ""
            Dim code As Integer = getCodeFromArray(c, offset, length)
            If (toUnicodeCmap IsNot Nothing) Then
                retval = cmapEncoding(code, length, False, toUnicodeCmap)
            End If
            If (retval = "" AndAlso cmap IsNot Nothing) Then
                retval = cmapEncoding(code, length, False, cmap)
            End If

            ' there is no cmap but probably an encoding with a suitable mapping
            If (retval = "") Then
                If (fontEncoding IsNot Nothing) Then
                    retval = fontEncoding.getCharacter(code)
                End If
                If (retval = "" AndAlso (cmap Is Nothing OrElse length = 2)) Then
                    retval = getStringFromArray(c, offset, length)
                End If
            End If
            Return retval
        End Function

        Public Overridable Function encodeToCID(ByVal c() As Byte, ByVal offset As Integer, ByVal length As Integer) As Integer ' throws IOException
            Dim code As Integer = -1
            If (encode(c, offset, length) <> "") Then
                code = getCodeFromArray(c, offset, length)
            End If
            Return code
        End Function

        Private Shared SINGLE_CHAR_STRING As String() = Array.CreateInstance(GetType(String), 256)
        Private Shared DOUBLE_CHAR_STRING As String(,) = Array.CreateInstance(GetType(String), 256, 256)

        Shared Sub New()
            For i As Integer = 0 To 256 - 1
                Try
                    SINGLE_CHAR_STRING(i) = Sistema.Strings.GetString({i}, "ISO-8859-1")
                Catch e As UnsupportedEncodingException
                    ' Nothing should happen here
                    Debug.Print(e.ToString) 'e.printStackTrace();
                End Try
                For j As Integer = 0 To 256 - 1
                    Try
                        DOUBLE_CHAR_STRING(i, j) = Sistema.Strings.GetString({i, j}, "UTF-16BE")
                    Catch e As UnsupportedEncodingException
                        ' Nothing should happen here
                        Debug.Print(e.ToString) 'e.printStackTrace();
                    End Try
                Next
            Next
        End Sub

        Private Shared Function getStringFromArray(ByVal c() As Byte, ByVal offset As Integer, ByVal length As Integer) As String 'throws IOException
            Dim retval As String = ""
            If (length = 1) Then
                retval = SINGLE_CHAR_STRING((c(offset) + 256) Mod 256)
            ElseIf (length = 2) Then
                retval = DOUBLE_CHAR_STRING((c(offset) + 256) Mod 256, (c(offset + 1) + 256) Mod 256)
            Else
                Throw New IOException("Error:Unknown character length:" & length)
            End If
            Return retval
        End Function

        Protected Function parseCmap(ByVal cmapRoot As String, ByVal cmapStream As InputStream) As CMap
            Dim targetCmap As CMap = Nothing
            If (cmapStream IsNot Nothing) Then
                Dim parser As CMapParser = New CMapParser()
                Try
                    targetCmap = parser.parse(cmapRoot, cmapStream)
                    ' limit the cache to external CMaps
                    If (cmapRoot <> "") Then
                        cmapObjects.put(targetCmap.getName(), targetCmap)
                    End If
                Catch exception As IOException
                End Try
            End If
            Return targetCmap
        End Function

        '/**
        ' * The will set the encoding for this font.
        ' *
        ' * @param enc The font encoding.
        ' */
        Public Sub setFontEncoding(ByVal enc As pdfbox.encoding.Encoding)
            fontEncoding = enc
        End Sub

        '/**
        ' * This will get or create the encoder.
        ' *
        ' * @return The encoding to use.
        ' */
        Public Function getFontEncoding() As pdfbox.encoding.Encoding
            Return fontEncoding
        End Function

        '/**
        ' * This will always return "Font" for fonts.
        ' *
        ' * @return The type of object that this is.
        ' */
        Public Function getFontType() As String
            Return font.getNameAsString(COSName.TYPE)
        End Function

        ' Memorized values to avoid repeated dictionary lookups
        Private subtype As String = ""
        Private type1Font As Boolean
        Private type3Font As Boolean
        Private trueTypeFont As Boolean
        Private type0Font As Boolean

        '/**
        ' * This will get the subtype of font, Type1, Type3, ...
        ' *
        ' * @return The type of font that this is.
        ' */
        Public Function getSubType() As String
            If (subtype = "") Then
                subtype = font.getNameAsString(COSName.SUBTYPE)
                type1Font = "Type1".Equals(subtype)
                trueTypeFont = "TrueType".Equals(subtype)
                type0Font = "Type0".Equals(subtype)
                type3Font = "Type3".Equals(subtype)
            End If
            Return subtype
        End Function

        '/**
        ' * Determines if the font is a type 1 font.
        ' * @return returns true if the font is a type 1 font
        ' */
        Protected Function isType1Font() As Boolean
            getSubType()
            Return type1Font
        End Function

        '/**
        ' * Determines if the font is a type 3 font.
        ' * 
        ' * @return returns true if the font is a type 3 font
        ' */
        Public Function isType3Font() As Boolean
            getSubType()
            Return type3Font
        End Function

        '/**
        ' * Determines if the font is a type 0 font.
        ' * @return returns true if the font is a type 0 font
        ' */
        Protected Function isType0Font() As Boolean
            getSubType()
            Return type0Font
        End Function

        Private Function isTrueTypeFont() As Boolean
            getSubType()
            Return trueTypeFont
        End Function

        '/**
        ' * Determines if the font is a symbolic font.
        ' * 
        ' * @return returns true if the font is a symbolic font
        ' */
        Public Function isSymbolicFont() As Boolean
            If (getFontDescriptor() IsNot Nothing) Then
                Return getFontDescriptor().isSymbolic()
            End If
            Return False
        End Function

        '/**
        ' * The PostScript name of the font.
        ' *
        ' * @return The postscript name of the font.
        ' */
        Public Function getBaseFont() As String
            Return font.getNameAsString(COSName.BASE_FONT)
        End Function

        '/**
        ' * Set the PostScript name of the font.
        ' *
        ' * @param baseFont The postscript name for the font.
        ' */
        Public Sub setBaseFont(ByVal baseFont As String)
            font.setName(COSName.BASE_FONT, baseFont)
        End Sub

        '/**
        ' * The code for the first char or -1 if there is none.
        ' *
        ' * @return The code for the first character.
        ' */
        Public Function getFirstChar() As Integer
            Return font.getInt(COSName.FIRST_CHAR, -1)
        End Function

        '/**
        ' * Set the first character this font supports.
        ' *
        ' * @param firstChar The first character.
        ' */
        Public Sub setFirstChar(ByVal firstChar As Integer)
            font.setInt(COSName.FIRST_CHAR, firstChar)
        End Sub

        '/**
        ' * The code for the last char or -1 if there is none.
        ' *
        ' * @return The code for the last character.
        ' */
        Public Function getLastChar() As Integer
            Return font.getInt(COSName.LAST_CHAR, -1)
        End Function

        '/**
        ' * Set the last character this font supports.
        ' *
        ' * @param lastChar The last character.
        ' */
        Public Sub setLastChar(ByVal lastChar As Integer)
            font.setInt(COSName.LAST_CHAR, lastChar)
        End Sub

        '/**
        ' * The widths of the characters.  This will be null for the standard 14 fonts.
        ' *
        ' * @return The widths of the characters.
        ' *
        ' */
        Public Function getWidths() As List(Of NFloat)
            If (widths Is Nothing) Then
                Dim array As COSArray = font.getDictionaryObject(COSName.WIDTHS)
                If (array IsNot Nothing) Then
                    widths = COSArrayList.convertFloatCOSArrayToList(array)
                End If
            End If
            Return widths
        End Function

        '/**
        ' * Set the widths of the characters code.
        ' *
        ' * @param widthsList The widths of the character codes.
        ' */
        Public Sub setWidths(ByVal widthsList As List(Of NFloat))
            widths = widthsList
            font.setItem(COSName.WIDTHS, COSArrayList.converterToCOSArray(widths))
        End Sub

        '/**
        ' * This will get the matrix that is used to transform glyph space to
        ' * text space.  By default there are 1000 glyph units to 1 text space
        ' * unit, but type3 fonts can use any value.
        ' *
        ' * Note:If this is a type3 font then it can be modified via the PDType3Font.setFontMatrix, otherwise this
        ' * is a read-only property.
        ' *
        ' * @return The matrix to transform from glyph space to text space.
        ' */
        Public Overridable Function getFontMatrix() As PDMatrix
            If (fontMatrix Is Nothing) Then
                Dim array As COSArray = font.getDictionaryObject(COSName.FONT_MATRIX)
                If (array Is Nothing) Then
                    array = New COSArray()
                    array.add(New COSFloat(0.00100000005F))
                    array.add(COSInteger.ZERO)
                    array.add(COSInteger.ZERO)
                    array.add(New COSFloat(0.00100000005F))
                    array.add(COSInteger.ZERO)
                    array.add(COSInteger.ZERO)
                End If
                fontMatrix = New PDMatrix(array)
            End If
            Return fontMatrix
        End Function

        '/**
        ' * This will get the fonts bounding box.
        ' *
        ' * @return The fonts bounding box.
        ' *
        ' * @throws IOException If there is an error getting the bounding box.
        ' */
        Public MustOverride Function getFontBoundingBox() As PDRectangle 'throws IOException;

        Public Overrides Function equals(ByVal other As Object) As Boolean
            Return TypeOf (other) Is PDFont AndAlso DirectCast(other, PDFont).getCOSObject().Equals(Me.getCOSObject())
        End Function

        Public Function hashCode() As Integer
            Return Me.getCOSObject().hashCode()
        End Function

        Public Overrides Function GetHashCode() As Integer
            Return Me.getCOSObject().hashCode()
        End Function

        '/**
        ' * Determines the width of the given character.
        ' * @param charCode the code of the given character
        ' * @return the width of the character
        ' */
        Public Overridable Function getFontWidth(ByVal charCode As Integer) As Single
            Dim width As Single = -1
            Dim firstChar As Integer = getFirstChar()
            Dim lastChar As Integer = getLastChar()
            If (charCode >= firstChar AndAlso charCode <= lastChar) Then
                ' maybe the font doesn't provide any widths
                getWidths()
                If (widths IsNot Nothing) Then
                    width = widths.get(charCode - firstChar).Value
                End If
            Else
                Dim fd As PDFontDescriptor = getFontDescriptor()
                If (TypeOf (fd) Is PDFontDescriptorDictionary) Then
                    width = fd.getMissingWidth()
                End If
            End If
            Return width
        End Function

        '/**
        ' * Determines if a font as a ToUnicode entry.
        ' * @return true if the font has a ToUnicode entry
        ' */
        Protected Function hasToUnicode() As Boolean
            Return Me._hasToUnicode
        End Function

        '/**
        ' * Sets hasToUnicode to the given value.
        ' * @param hasToUnicodeValue the given value for hasToUnicode
        ' */
        Protected Sub setHasToUnicode(ByVal hasToUnicodeValue As Boolean)
            Me._hasToUnicode = hasToUnicodeValue
        End Sub

        '/**
        ' * Determines the width of the space character.
        ' * @return the width of the space character
        ' */
        Public MustOverride Function getSpaceWidth() As Single

        '/**
        ' * Returns the toUnicode mapping if present.
        ' * 
        ' * @return the CMap representing the toUnicode mapping
        ' */
        Public Function getToUnicodeCMap() As CMap
            Return toUnicodeCmap
        End Function

    End Class

End Namespace

Imports FinSeA.Sistema
Imports System.Drawing
Imports FinSeA.Io
Imports FinSeA.Drawings
Imports FinSeA.org.apache.fontbox.afm
Imports FinSeA.org.apache.fontbox.cff
Imports FinSeA.org.apache.fontbox.util
Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.encoding
Imports FinSeA.org.apache.pdfbox.exceptions
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.Exceptions
Imports FinSeA.org.apache.fontbox.cff.encoding
Imports FinSeA.org.apache.fontbox.cff.charset

Namespace org.apache.pdfbox.pdmodel.font


    '/**
    ' * Me class represents a CFF/Type2 Font (aka Type1C Font).
    ' * @author Villu Ruusmann
    ' * @version $Revision: 10.0$
    ' */
    Public Class PDType1CFont
        Inherits PDSimpleFont

        Private cffFont As CFFFont = Nothing

        Private codeToName As Map(Of NInteger, String) = New HashMap(Of NInteger, String)

        Private codeToCharacter As Map(Of NInteger, String) = New HashMap(Of NInteger, String)

        Private characterToCode As Map(Of String, NInteger) = New HashMap(Of String, NInteger)

        Private fontMetric As FontMetric = Nothing

        Private awtFont As JFont = Nothing

        Private glyphWidths As Map(Of String, NFloat) = New HashMap(Of String, NFloat)

        Private glyphHeights As Map(Of String, NFloat) = New HashMap(Of String, NFloat)

        Private avgWidth As NFloat = Nothing

        Private fontBBox As PDRectangle = Nothing

        'private static final Log log = LogFactory.getLog(PDType1CFont.class);

        Private Shared ReadOnly SPACE_BYTES() As Byte = {32}

        Private fontDict As COSDictionary = Nothing

        '/**
        ' * Constructor.
        ' * @param fontDictionary the corresponding dictionary
        ' */
        Public Sub New(ByVal fontDictionary As COSDictionary) 'throws IOException
            MyBase.New(fontDictionary)
            fontDict = fontDictionary
            load()
        End Sub


        Public Overrides Function encode(ByVal bytes() As Byte, ByVal offset As Integer, ByVal length As Integer) As String 'throws IOException
            Dim character As String = getCharacter(bytes, offset, length)
            If (character = "") Then
                LOG.debug("No character for code " & (bytes(offset) And &HFF) & " in " & Me.cffFont.getName())
                Return ""
            End If

            Return character
        End Function

        Public Overrides Function encodeToCID(ByVal bytes() As Byte, ByVal offset As Integer, ByVal length As Integer) As Integer
            If (length > 2) Then
                Return -1
            End If
            Dim code As Integer = bytes(offset) And &HFF
            If (length = 2) Then
                code = code * 256 + bytes(offset + 1) And &HFF
            End If
            Return code
        End Function

        Private Function getCharacter(ByVal bytes() As Byte, ByVal offset As Integer, ByVal length As Integer) As String
            Dim code As Integer = encodeToCID(bytes, offset, length)
            If (code = -1) Then
                Return ""
            End If
            Return Me.codeToCharacter.get(code)
        End Function


        Public Overrides Function getFontWidth(ByVal bytes() As Byte, ByVal offset As Integer, ByVal length As Integer) As Single 'throws IOException
            Dim name As String = getName(bytes, offset, length)
            If (name = "" AndAlso Not Sistema.Arrays.Compare(SPACE_BYTES, bytes) = 0) Then
                LOG.debug("No name for code " & (bytes(offset) And &HFF) & " in " & Me.cffFont.getName())
                Return 0
            End If

            Dim width As NFloat = Me.glyphWidths.get(name)
            If (width.HasValue = False) Then
                width = getFontMetric().getCharacterWidth(name)
                Me.glyphWidths.put(name, width)
            End If

            Return width.Value
        End Function

        Public Overrides Function getFontHeight(ByVal bytes() As Byte, ByVal offset As Integer, ByVal length As Integer) As Single
            Dim name As String = getName(bytes, offset, length)
            If (name = "") Then
                LOG.debug("No name for code " & (bytes(offset) And &HFF) & " in " & Me.cffFont.getName())
                Return 0
            End If

            Dim height As NFloat = Me.glyphHeights.get(name)
            If (Not height.HasValue) Then
                height = getFontMetric().getCharacterHeight(name)
                Me.glyphHeights.put(name, height)
            End If

            Return height.Value
        End Function

        Private Function getName(ByVal bytes() As Byte, ByVal offset As Integer, ByVal length As Integer) As String
            If (length > 2) Then
                Return ""
            End If

            Dim code As Integer = bytes(offset) And &HFF
            If (length = 2) Then
                code = code * 256 + bytes(offset + 1) And &HFF
            End If

            Return Me.codeToName.get(code)
        End Function

        Public Overrides Function getStringWidth(ByVal [string] As String) As Single
            Dim width As Single = 0
            For i As Integer = 0 To [string].Length() - 1
                Dim character As String = [string].Substring(i, i + 1)

                Dim code As NInteger = getCode(character)
                If (Not code.HasValue) Then
                    LOG.debug("No code for character " & character)
                    Return 0
                End If

                width += getFontWidth({CByte(code.Value)}, 0, 1)
            Next

            Return width
        End Function

        Private Function getCode(ByVal character As String) As NInteger
            Return Me.characterToCode.get(character)
        End Function


        Public Overrides Function getAverageFontWidth() As Single
            If (Not Me.avgWidth.HasValue) Then
                Me.avgWidth = getFontMetric().getAverageCharacterWidth()
            End If

            Return Me.avgWidth.Value
        End Function

        Public Overrides Function getFontBoundingBox() As PDRectangle
            If (Me.fontBBox Is Nothing) Then
                Me.fontBBox = New PDRectangle(getFontMetric().getFontBBox())
            End If

            Return Me.fontBBox
        End Function

        Public Overrides Function getFontMatrix() As PDMatrix
            If (fontMatrix Is Nothing) Then
                Dim numbers As List(Of Number) = Me.cffFont.getProperty("FontMatrix")
                If (numbers IsNot Nothing AndAlso numbers.size() = 6) Then
                    Dim array As COSArray = New COSArray()
                    For Each number As Number In numbers
                        array.add(New COSFloat(number.floatValue()))
                    Next
                    fontMatrix = New PDMatrix(array)
                Else
                    fontMatrix = MyBase.getFontMatrix()
                End If
            End If
            Return fontMatrix
        End Function


        Public Overrides Function getawtFont() As JFont ' throws IOException
            If (awtFont Is Nothing) Then
                Me.awtFont = prepareAwtFont(Me.cffFont)
            End If
            Return awtFont
        End Function

        Private Function getFontMetric() As FontMetric
            If (fontMetric Is Nothing) Then
                Try
                    fontMetric = prepareFontMetric(cffFont)
                Catch exception As IOException
                    LOG.error("An error occured while extracting the font metrics!", exception)
                End Try
            End If
            Return fontMetric
        End Function

        Private Sub load() 'throws IOException
            Dim cffBytes() As Byte = loadBytes()

            Dim cffParser As CFFParser = New CFFParser()
            Dim fonts As List(Of CFFFont) = cffParser.parse(cffBytes)

            Dim baseFontName As String = getBaseFont()
            If (fonts.size() > 1 AndAlso baseFontName <> "") Then
                For Each font As CFFFont In fonts
                    If (baseFontName.Equals(font.getName())) Then
                        Me.cffFont = font
                        Exit For
                    End If
                Next
            End If
            If (Me.cffFont Is Nothing) Then
                Me.cffFont = fonts.get(0)
            End If

            Dim encoding As CFFEncoding = Me.cffFont.getEncoding()
            Dim pdfEncoding As PDFEncoding = New PDFEncoding(encoding)

            Dim charset As CFFCharset = Me.cffFont.getCharset()
            Dim pdfCharset As PDFCharset = New PDFCharset(charset)

            Dim charStringsDict As Map(Of String, Byte()) = Me.cffFont.getCharStringsDict()
            Dim pdfCharStringsDict As Map(Of String, Byte()) = New LinkedHashMap(Of String, Byte())
            pdfCharStringsDict.put(".notdef", charStringsDict.get(".notdef"))

            Dim codeToNameMap As Map(Of NInteger, String) = New LinkedHashMap(Of NInteger, String)

            'Dim mappings As FinSeA.Collection(Of CFFFont.Mapping) = Me.cffFont.getMappings()
            For Each mapping As CFFFont.Mapping In Me.cffFont.getMappings() 'for( Iterator<CFFFont.Mapping> it = mappings.iterator(); it.hasNext();)
                'CFFFont.Mapping mapping = it.next();
                Dim code As NInteger = mapping.getCode()
                Dim name As String = mapping.getName()
                codeToNameMap.put(code, name)
            Next

            Dim knownNames As [Set](Of String) = New HashSet(Of String)(codeToNameMap.Values())

            Dim codeToNameOverride As Map(Of NInteger, String) = loadOverride()
            For Each entry As Map.Entry(Of NInteger, String) In codeToNameOverride.entrySet() 'Iterator<Map.Entry(Of NInteger, String>> it = (codeToNameOverride.entrySet()).iterator(); it.hasNext();)'for( Iterator<Map.Entry(Of NInteger, String>> it = (codeToNameOverride.entrySet()).iterator(); it.hasNext();)
                'Map.Entry(Of NInteger, String> entry = it.next();
                Dim code As NInteger = entry.Key
                Dim name As String = entry.Value
                If (knownNames.contains(name)) Then
                    codeToNameMap.put(code, name)
                End If
            Next

            Dim nameToCharacter As Map(Of String, String)
            Try
                ' TODO remove access by reflection
                Dim t As System.Type = GetType(pdfbox.encoding.Encoding)
                Dim nameToCharacterField As System.Reflection.FieldInfo = t.GetField("NAME_TO_CHARACTER")
                'nameToCharacterField.setAccessible(True)
                nameToCharacter = nameToCharacterField.GetValue(Nothing)
            Catch e As Exception
                Throw New RuntimeException(e.Message, e)
            End Try

            For Each entry As Map.Entry(Of NInteger, String) In codeToNameMap.entrySet() 'for( Iterator<Map.Entry(Of NInteger,String>> it = (codeToNameMap.entrySet()).iterator(); it.hasNext();)
                'Map.Entry(Of NInteger,String> entry = it.next();
                Dim code As NInteger = entry.Key
                Dim name As String = entry.Value
                Dim uniName As String = "uni"
                Dim character As String = nameToCharacter.get(name)
                If (character <> "") Then
                    For j As Integer = 0 To character.Length() - 1
                        uniName &= hexString(Convert.ToInt32(character.Chars(j)), 4)
                    Next
                Else
                    uniName &= hexString(code.Value, 4)
                    character = ChrW(code.Value)
                End If
                pdfEncoding.register(code.Value, code.Value)
                pdfCharset.register(code.Value, uniName)
                Me.codeToName.put(code, uniName)
                Me.codeToCharacter.put(code, character)
                Me.characterToCode.put(character, code)
                pdfCharStringsDict.put(uniName, charStringsDict.get(name))
            Next

            Me.cffFont.setEncoding(pdfEncoding)
            Me.cffFont.setCharset(pdfCharset)
            charStringsDict.clear()
            charStringsDict.putAll(pdfCharStringsDict)
            Dim defaultWidthX As Number = Me.cffFont.getProperty("defaultWidthX")
            Me.glyphWidths.put("", defaultWidthX.floatValue())
        End Sub

        Private Function loadBytes() As Byte() ' throws IOException
            Dim fd As PDFontDescriptor = getFontDescriptor()
            If (fd IsNot Nothing AndAlso TypeOf (fd) Is PDFontDescriptorDictionary) Then
                Dim ff3Stream As PDStream = DirectCast(fd, PDFontDescriptorDictionary).getFontFile3()
                If (ff3Stream IsNot Nothing) Then
                    Dim os As ByteArrayOutputStream = New ByteArrayOutputStream()

                    Dim [is] As InputStream = ff3Stream.createInputStream()
                    Try
                        Dim buf(512 - 1) As Byte '= new byte[512];
                        While (True)
                            Dim count As Integer = [is].read(buf)
                            If (count < 0) Then
                                Exit While
                            End If
                            os.Write(buf, 0, count)
                        End While
                    Finally
                        [is].Close()
                    End Try

                    Dim ret() As Byte = os.toByteArray
                    'Return os.toByteArray()
                    os.Dispose()
                    Return ret
                End If
            End If

            Throw New IOException()
        End Function

        Private Function loadOverride() As Map(Of NInteger, String) '  throws IOException
            Dim result As Map(Of NInteger, String) = New LinkedHashMap(Of NInteger, String)
            Dim encoding As COSBase = fontDict.getDictionaryObject(COSName.ENCODING)
            If (TypeOf (encoding) Is COSName) Then
                Dim name As COSName = encoding
                result.putAll(loadEncoding(name))
            ElseIf (TypeOf (encoding) Is COSDictionary) Then
                Dim encodingDic As COSDictionary = encoding
                Dim baseName As COSName = encodingDic.getDictionaryObject(COSName.BASE_ENCODING)
                If (baseName IsNot Nothing) Then
                    result.putAll(loadEncoding(baseName))
                End If
                Dim differences As COSArray = encodingDic.getDictionaryObject(COSName.DIFFERENCES)
                If (differences IsNot Nothing) Then
                    result.putAll(loadDifferences(differences))
                End If
            End If

            Return result
        End Function

        Private Function loadEncoding(ByVal name As COSName) As Map(Of NInteger, String) ' throws IOException
            Dim result As Map(Of NInteger, String) = New LinkedHashMap(Of NInteger, String)
            Dim encoding As pdfbox.encoding.Encoding = EncodingManager.INSTANCE.getEncoding(name)
            For Each entry As Map.Entry(Of NInteger, String) In encoding.getCodeToNameMap().entrySet() 'for( Iterator<Map.Entry(Of NInteger,String>> it = (encoding.getCodeToNameMap().entrySet()).iterator(); it.hasNext();)
                'fsMap.Entry(Of NInteger,String> entry = it.next();
                result.put(entry.Key, entry.Value)
            Next

            Return result
        End Function

        Private Function loadDifferences(ByVal differences As COSArray) As Map(Of NInteger, String)
            Dim result As Map(Of NInteger, String) = New LinkedHashMap(Of NInteger, String)
            Dim code As NInteger = Nothing
            For i As Integer = 0 To differences.size() - 1
                Dim element As COSBase = differences.get(i)
                If (TypeOf (element) Is COSNumber) Then
                    Dim number As COSNumber = element
                    code = number.intValue()
                Else
                    If (TypeOf (element) Is COSName) Then
                        Dim name As COSName = element
                        result.put(code, name.getName())
                        code = code.Value + 1
                    End If
                End If
            Next
            Return result
        End Function

        Private Shared Function hexString(ByVal code As Integer, ByVal length As Integer) As String
            'Dim [string] As String = Strings.Hex(code, length, "0")
            '    While (String.Length() < length)
            '{
            '    string = ("0" + string);
            '}

            'return string;
            Return Strings.Hex(code, length, "0")
        End Function

        Private Function prepareFontMetric(ByVal font As CFFFont) As FontMetric 'throws IOException
            Dim afmBytes() As Byte = AFMFormatter.format(font)

            Dim [is] As InputStream = New ByteArrayInputStream(afmBytes)
            Try
                Dim afmParser As AFMParser = New AFMParser([is])
                afmParser.parse()

                Dim result As FontMetric = afmParser.getResult()

                ' Replace default FontBBox value with a newly computed one
                Dim bounds As BoundingBox = result.getFontBBox()
                Dim numbers As List(Of NInteger) = New ArrayList(Of NInteger)({ _
                    CInt(bounds.getLowerLeftX()), _
                    CInt(bounds.getLowerLeftY()), _
                    CInt(bounds.getUpperRightX()), _
                    CInt(bounds.getUpperRightY()) _
                        })
                font.addValueToTopDict("FontBBox", numbers)

                Return result
            Finally
                [is].Close()
            End Try
        End Function

        Private Shared Function prepareAwtFont(ByVal font As CFFFont) As JFont
            Dim type1Bytes() As Byte = Type1FontFormatter.format(font)

            Dim [is] As InputStream = New ByteArrayInputStream(type1Bytes)
            Try
                Return JFont.createFont(font.TYPE1_FONT, [is])
            Catch ffe As FontFormatException
                Throw New WrappedIOException(ffe)
            Finally
                [is].Close()
            End Try
        End Function

        '/**
        ' * Me class represents a PDFEncoding.
        ' *
        ' */
        Private Class PDFEncoding
            Inherits CFFEncoding

            Public Sub New(ByVal parent As CFFEncoding)
                'Iterator<Entry> parentEntries = parent.getEntries().iterator();
                For Each item As Entry In parent.getEntries 'While (parentEntries.hasNext())
                    addEntry(item)
                Next
            End Sub

            Public Overrides Function isFontSpecific() As Boolean
                Return True
            End Function

        End Class

        '/**
        ' * Me class represents a PDFCharset.
        ' *
        ' */
        Private Class PDFCharset
            Inherits CFFCharset

            Public Sub New(ByVal parent As CFFCharset)
                'Iterator<Entry> parentEntries = parent.getEntries().iterator();
                For Each item As Entry In parent.getEntries 'While (parentEntries.hasNext())
                    addEntry(item)
                Next
            End Sub

            Public Overrides Function isFontSpecific() As Boolean
                Return True
            End Function

        End Class



    End Class




End Namespace


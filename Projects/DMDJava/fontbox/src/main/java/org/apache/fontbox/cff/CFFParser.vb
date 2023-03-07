Imports FinSeA.Sistema
Imports System.IO
Imports FinSeA.org.apache.fontbox.cff.CFFFont
Imports FinSeA.org.apache.fontbox.cff.charset
Imports FinSeA.org.apache.fontbox.cff.encoding
Imports FinSeA.Text
Imports FinSeA

Namespace org.apache.fontbox.cff

    '/**
    ' * This class represents a parser for a CFF font. 
    ' * @author Villu Ruusmann
    ' * @version $Revision: 1.0 $
    ' */
    Public Class CFFParser
        Private Const TAG_OTTO = "OTTO"
        Private Const TAG_TTCF = "ttcf"
        Private Const TAG_TTFONLY = ChrW(0) & ChrW(0) & ChrW(0) & ChrW(0) '"\u0000\u0001\u0000\u0000";

        Private input As CFFDataInput = Nothing
        Private _header As Header = Nothing
        Private nameIndex As IndexData = Nothing
        Private topDictIndex As IndexData = Nothing
        Private stringIndex As IndexData = Nothing

        '/**
        ' * Parsing CFF Font using a byte array as input.
        ' * @param bytes the given byte array
        ' * @return the parsed CFF fonts
        ' * @throws IOException If there is an error reading from the stream
        ' */
        Public Function parse(ByVal bytes() As Byte) As List(Of CFFFont)
            input = New CFFDataInput(bytes)

            Dim firstTag As String = readTagName(input)
            ' try to determine which kind of font we have
            If (TAG_OTTO.Equals(firstTag)) Then
                ' Me is OpenType font containing CFF data
                ' so find CFF tag
                Dim numTables As Short = input.readShort()
                Dim searchRange As Short = input.readShort()
                Dim entrySelector As Short = input.readShort()
                Dim rangeShift As Short = input.readShort()

                Dim cffFound As Boolean = False
                For q As Integer = 0 To numTables - 1
                    Dim tagName As String = readTagName(input)
                    Dim checksum As Long = readLong(input)
                    Dim offset As Long = readLong(input)
                    Dim length As Long = readLong(input)
                    If (tagName.Equals("CFF ")) Then
                        cffFound = True
                        Dim bytes2() As Byte = Array.CreateInstance(GetType(Integer), length)
                        Array.Copy(bytes, offset, bytes2, 0, bytes2.Length)
                        input = New CFFDataInput(bytes2)
                        Exit For
                    End If
                Next
                If (Not cffFound) Then
                    Throw New IOException("CFF tag not found in Me OpenType font.")
                End If
            ElseIf (TAG_TTCF.Equals(firstTag)) Then
                Throw New NotSupportedException("True Type Collection fonts are not supported.")
            ElseIf (TAG_TTFONLY.Equals(firstTag)) Then
                Throw New NotSupportedException("OpenType fonts containing a true type font are not supported.")
            Else
                input.setPosition(0)
            End If

            _header = readHeader(input)
            nameIndex = readIndexData(input)
            topDictIndex = readIndexData(input)
            stringIndex = readIndexData(input)
            Dim globalSubrIndex As IndexData = readIndexData(input)

            Dim fonts As List(Of CFFFont) = New ArrayList(Of CFFFont)()
            For i As Integer = 0 To nameIndex.getCount() - 1
                Dim font As CFFFont = parseFont(i)
                font.setGlobalSubrIndex(globalSubrIndex)
                fonts.add(font)
            Next
            Return fonts
        End Function

        Private Shared Function readTagName(ByVal input As CFFDataInput) As String
            Dim b() As Byte = input.readBytes(4)
            Return Strings.GetString(b)
        End Function

        Private Shared Function readLong(ByVal input As CFFDataInput) As Long
            Return (input.readCard16() << 16) Or input.readCard16()
        End Function

        Private Shared Function readHeader(ByVal input As CFFDataInput) As Header
            Dim cffHeader As New Header
            cffHeader.major = input.readCard8()
            cffHeader.minor = input.readCard8()
            cffHeader.hdrSize = input.readCard8()
            cffHeader.offSize = input.readOffSize()
            Return cffHeader
        End Function

        Private Shared Function readIndexData(ByVal input As CFFDataInput) As IndexData
            Dim count As Integer = input.readCard16()
            Dim index As New IndexData(count)
            If (count = 0) Then
                Return index
            End If
            Dim offSize As Integer = input.readOffSize()
            For i As Integer = 0 To count
                index.setOffset(i, input.readOffset(offSize))
            Next
            Dim dataSize As Integer = index.getOffset(count) - index.getOffset(0)
            index.initData(dataSize)
            For i As Integer = 0 To dataSize - 1
                index.setData(i, input.readCard8())
            Next
            Return index
        End Function

        Private Shared Function readDictData(ByVal input As CFFDataInput) As DictData
            Dim dict As New DictData()
            dict.entries = New ArrayList(Of DictData.Entry)()
            While (input.hasRemaining())
                Dim entry As DictData.Entry = readEntry(input)
                dict.entries.add(entry)
            End While
            Return dict
        End Function

        Private Shared Function readEntry(ByVal input As CFFDataInput) As DictData.Entry
            Dim entry As New DictData.Entry()
            While (True)
                Dim b0 As Integer = input.readUnsignedByte()

                If (b0 >= 0 AndAlso b0 <= 21) Then
                    entry.operator = readOperator(input, b0)
                    Exit While
                ElseIf (b0 = 28 OrElse b0 = 29) Then
                    entry.operands.add(readIntegerNumber(input, b0))
                ElseIf (b0 = 30) Then
                    entry.operands.add(readRealNumber(input, b0))
                ElseIf (b0 >= 32 AndAlso b0 <= 254) Then
                    entry.operands.add(readIntegerNumber(input, b0))
                Else
                    Throw New ArgumentException()
                End If
            End While
            Return entry
        End Function

        Private Shared Function readOperator(ByVal input As CFFDataInput, ByVal b0 As Integer) As CFFOperator
            Dim key As CFFOperator.Key = readOperatorKey(input, b0)
            Return CFFOperator.getOperator(key)
        End Function

        Private Shared Function readOperatorKey(ByVal input As CFFDataInput, ByVal b0 As Integer) As CFFOperator.Key
            If (b0 = 12) Then
                Dim b1 As Integer = input.readUnsignedByte()
                Return New CFFOperator.Key(b0, b1)
            End If
            Return New CFFOperator.Key(b0)
        End Function

        Private Shared Function readIntegerNumber(ByVal input As CFFDataInput, ByVal b0 As Integer) As NInteger
            If (b0 = 28) Then
                Dim b1 As Integer = input.readUnsignedByte()
                Dim b2 As Integer = input.readUnsignedByte()
                Return (b1 << 8) Or b2
            ElseIf (b0 = 29) Then
                Dim b1 As Integer = input.readUnsignedByte()
                Dim b2 As Integer = input.readUnsignedByte()
                Dim b3 As Integer = input.readUnsignedByte()
                Dim b4 As Integer = input.readUnsignedByte()
                Return (b1 << 24) Or (b2 << 16) Or (b3 << 8) Or b4
            ElseIf (b0 >= 32 AndAlso b0 <= 246) Then
                Return b0 - 139
            ElseIf (b0 >= 247 AndAlso b0 <= 250) Then
                Dim b1 As Integer = input.readUnsignedByte()
                Return ((b0 - 247) * 256 + b1 + 108)
            ElseIf (b0 >= 251 AndAlso b0 <= 254) Then
                Dim b1 As Integer = input.readUnsignedByte()
                Return -(b0 - 251) * 256 - b1 - 108
            Else
                Throw New ArgumentException()
            End If
        End Function

        Private Shared Function readRealNumber(ByVal input As CFFDataInput, ByVal b0 As Integer) As NDouble
            Dim sb As New StringBuffer()
            Dim done As Boolean = False
            Dim exponentMissing As Boolean = False
            While (Not done)
                Dim b As Integer = input.readUnsignedByte()
                Dim nibbles() As Integer = {b \ 16, b Mod 16}
                For Each nibble As Integer In nibbles
                    Select Case (nibble)
                        Case &H0, &H1, &H2, &H3, &H4, &H5, &H6, &H7, &H8, &H9
                            sb.append(Convert.ToChar(nibble))
                            exponentMissing = False
                        Case &HA
                            sb.append(".")
                        Case &HB
                            sb.append("E")
                            exponentMissing = True
                        Case &HC
                            sb.append("E-")
                            exponentMissing = True
                        Case &HD
                        Case &HE
                            sb.append("-")
                        Case &HF
                            done = True
                        Case Else
                            Throw New ArgumentException()
                    End Select
                Next
            End While
            If (exponentMissing) Then
                ' the exponent is missing, just append "0" to avoid an exception
                ' not sure if 0 is the correct value, but it seems to fit
                ' see PDFBOX-1522
                sb.append("0")
            End If
            Return Double.Parse(sb.ToString())
        End Function

        Private Function parseFont(ByVal index As Integer) As CFFFont
            Dim font As CFFFont = Nothing
            Dim nameInput As New DataInput(nameIndex.getBytes(index))
            Dim name As String = nameInput.getString()

            Dim topDictInput As New CFFDataInput(topDictIndex.getBytes(index))
            Dim topDict As DictData = readDictData(topDictInput)
            Dim syntheticBaseEntry As DictData.Entry = topDict.getEntry("SyntheticBase")
            If (syntheticBaseEntry IsNot Nothing) Then
                Throw New IOException("Synthetic Fonts are not supported")
            End If

            Dim rosEntry As DictData.Entry = topDict.getEntry("ROS")
            If (rosEntry IsNot Nothing) Then
                font = New CFFFontROS()
                With DirectCast(font, CFFFontROS)
                    .setRegistry(readString(rosEntry.getNumber(0).intValue()))
                    .setOrdering(readString(rosEntry.getNumber(1).intValue()))
                    .setSupplement(rosEntry.getNumber(2).intValue())
                End With
            End If

            If (font Is Nothing) Then
                ' -- No specific behavior for Me font
                font = New CFFFont()
            End If

            font.setName(name)

            font.addValueToTopDict("version", getString(topDict, "version"))
            font.addValueToTopDict("Notice", getString(topDict, "Notice"))
            font.addValueToTopDict("Copyright", getString(topDict, "Copyright"))
            font.addValueToTopDict("FullName", getString(topDict, "FullName"))
            font.addValueToTopDict("FamilyName", getString(topDict, "FamilyName"))
            font.addValueToTopDict("Weight", getString(topDict, "Weight"))
            font.addValueToTopDict("isFixedPitch", getBoolean(topDict, "isFixedPitch", False))
            font.addValueToTopDict("ItalicAngle", getNumber(topDict, "ItalicAngle", New NInteger(0)))
            font.addValueToTopDict("UnderlinePosition", getNumber(topDict, "UnderlinePosition", New NInteger(-100)))
            font.addValueToTopDict("UnderlineThickness", getNumber(topDict, "UnderlineThickness", New NInteger(50)))
            font.addValueToTopDict("PaintType", getNumber(topDict, "PaintType", New NInteger(0)))
            font.addValueToTopDict("CharstringType", getNumber(topDict, "CharstringType", New NInteger(2)))
            font.addValueToTopDict("FontMatrix", getArray(topDict, "FontMatrix", New ArrayList(Of NDouble)({0.001, 0, 0, 0.001, 0, 0})))
            font.addValueToTopDict("UniqueID", getNumber(topDict, "UniqueID", Nothing))
            font.addValueToTopDict("FontBBox", getArray(topDict, "FontBBox", New ArrayList(Of NInteger)({0, 0, 0, 0})))
            font.addValueToTopDict("StrokeWidth", getNumber(topDict, "StrokeWidth", New NInteger(0)))
            font.addValueToTopDict("XUID", getArray(topDict, "XUID", Nothing))

            Dim charStringsEntry As DictData.Entry = topDict.getEntry("CharStrings")
            Dim charStringsOffset As Integer = charStringsEntry.getNumber(0).intValue()
            input.setPosition(charStringsOffset)
            Dim charStringsIndex As IndexData = readIndexData(input)
            Dim charsetEntry As DictData.Entry = topDict.getEntry("charset")
            Dim charset As CFFCharset
            Dim charsetId As Integer = 0
            If (charsetEntry IsNot Nothing) Then charsetId = charsetEntry.getNumber(0).intValue()
            If (charsetId = 0) Then
                charset = CFFISOAdobeCharset.getInstance()
            ElseIf (charsetId = 1) Then
                charset = CFFExpertCharset.getInstance()
            ElseIf (charsetId = 2) Then
                charset = CFFExpertSubsetCharset.getInstance()
            Else
                input.setPosition(charsetId)
                charset = readCharset(input, charStringsIndex.getCount())
            End If
            font.setCharset(charset)
            font.getCharStringsDict().put(".notdef", charStringsIndex.getBytes(0))
            Dim gids() As Integer = Array.CreateInstance(GetType(Integer), charStringsIndex.getCount())
            Dim glyphEntries As List(Of CFFCharset.Entry) = charset.getEntries()
            For i As Integer = 1 To charStringsIndex.getCount() - 1
                Dim glyphEntry As CFFCharset.Entry = glyphEntries.get(i - 1)
                gids(i - 1) = glyphEntry.getSID()
                font.getCharStringsDict().put(glyphEntry.getName(), charStringsIndex.getBytes(i))
            Next
            Dim encodingEntry As DictData.Entry = topDict.getEntry("Encoding")
            Dim encoding As CFFEncoding
            Dim encodingId As Integer = 0
            If (encodingEntry IsNot Nothing) Then encodingEntry.getNumber(0).intValue()
            If (encodingId = 0 OrElse rosEntry IsNot Nothing) Then '// --- ROS uses StandardEncoding
                encoding = CFFStandardEncoding.getInstance()
            ElseIf (encodingId = 1) Then
                encoding = CFFExpertEncoding.getInstance()
            Else
                input.setPosition(encodingId)
                encoding = readEncoding(input, gids)
            End If
            font.setEncoding(encoding)

            If (rosEntry IsNot Nothing) Then
                ' ---- It is a CIDKeyed Font, The Private Dictionary isn't in the Top Dict But in the Font Dict
                ' ---- Font Dict can be accessed by the FDArray and FDSelect
                Dim fdArrayEntry As DictData.Entry = topDict.getEntry("FDArray")
                If (fdArrayEntry Is Nothing) Then
                    Throw New IOException("FDArray is missing for a CIDKeyed Font.")
                End If

                Dim fontDictOffset As Integer = fdArrayEntry.getNumber(0).intValue()
                input.setPosition(fontDictOffset)
                Dim fdIndex As IndexData = readIndexData(input)

                Dim privateDictionaries As List(Of Map(Of String, Object)) = New LinkedList(Of Map(Of String, Object))()
                Dim fontDictionaries As List(Of Map(Of String, Object)) = New LinkedList(Of Map(Of String, Object))()
                Dim fontRos As CFFFontROS = font

                For i As Integer = 0 To fdIndex.getCount() - 1
                    Dim b() As Byte = fdIndex.getBytes(i)
                    Dim fontDictInput As New CFFDataInput(b)
                    Dim fontDictData As DictData = readDictData(fontDictInput)

                    Dim fontDictMap As Map(Of String, Object) = New LinkedHashMap(Of String, Object)()
                    fontDictMap.put("FontName", getString(fontDictData, "FontName"))
                    fontDictMap.put("FontType", getNumber(fontDictData, "FontType", New NInteger(0)))
                    fontDictMap.put("FontBBox", getDelta(fontDictData, "FontBBox", Nothing))
                    fontDictMap.put("FontMatrix", getDelta(fontDictData, "FontMatrix", Nothing))
                    ' TODO OD-4 : Add here other keys
                    fontDictionaries.add(fontDictMap)

                    Dim privateEntry As DictData.Entry = fontDictData.getEntry("Private")
                    ' --- Font DICT is invalid without "Private" entry
                    If (privateEntry Is Nothing) Then
                        Throw New IOException("Missing Private Dictionary")
                    End If

                    Dim privateOffset As Integer = privateEntry.getNumber(1).intValue()
                    input.setPosition(privateOffset)
                    Dim privateSize As Integer = privateEntry.getNumber(0).intValue()
                    Dim privateDictData As New CFFDataInput(input.readBytes(privateSize))
                    Dim privateDict As DictData = readDictData(privateDictData)

                    Dim privDict As Map(Of String, Object) = New LinkedHashMap(Of String, Object)()
                    privDict.put("BlueValues", getDelta(privateDict, "BlueValues", Nothing))
                    privDict.put("OtherBlues", getDelta(privateDict, "OtherBlues", Nothing))
                    privDict.put("FamilyBlues", getDelta(privateDict, "FamilyBlues", Nothing))
                    privDict.put("FamilyOtherBlues", getDelta(privateDict, "FamilyOtherBlues", Nothing))
                    privDict.put("BlueScale", getNumber(privateDict, "BlueScale", New NDouble(0.039625)))
                    privDict.put("BlueShift", getNumber(privateDict, "BlueShift", New NInteger(7)))
                    privDict.put("BlueFuzz", getNumber(privateDict, "BlueFuzz", New NInteger(1)))
                    privDict.put("StdHW", getNumber(privateDict, "StdHW", Nothing))
                    privDict.put("StdVW", getNumber(privateDict, "StdVW", Nothing))
                    privDict.put("StemSnapH", getDelta(privateDict, "StemSnapH", Nothing))
                    privDict.put("StemSnapV", getDelta(privateDict, "StemSnapV", Nothing))
                    privDict.put("ForceBold", getBoolean(privateDict, "ForceBold", Nothing))
                    privDict.put("LanguageGroup", getNumber(privateDict, "LanguageGroup", New NInteger(0)))
                    privDict.put("ExpansionFactor", getNumber(privateDict, "ExpansionFactor", New NDouble(0.059999999999999998)))
                    privDict.put("initialRandomSeed", getNumber(privateDict, "initialRandomSeed", New NInteger(0)))
                    privDict.put("defaultWidthX", getNumber(privateDict, "defaultWidthX", New NInteger(0)))
                    privDict.put("nominalWidthX", getNumber(privateDict, "nominalWidthX", New NInteger(0)))

                    Dim localSubrOffset As NInteger = getNumber(privateDict, "Subrs", New NInteger(0))
                    If (localSubrOffset = 0) Then
                        font.setLocalSubrIndex(New IndexData(0))
                    Else
                        input.setPosition(privateOffset + localSubrOffset)
                        font.setLocalSubrIndex(readIndexData(input))
                    End If

                    privateDictionaries.add(privDict)
                Next

                fontRos.setFontDict(fontDictionaries)
                fontRos.setPrivDict(privateDictionaries)

                Dim fdSelectEntry As DictData.Entry = topDict.getEntry("FDSelect")
                Dim fdSelectPos As Integer = fdSelectEntry.getNumber(0).intValue()
                input.setPosition(fdSelectPos)
                Dim fdSelect As CIDKeyedFDSelect = readFDSelect(input, charStringsIndex.getCount(), fontRos)

                font.addValueToPrivateDict("defaultWidthX", New NInteger(1000))
                font.addValueToPrivateDict("nominalWidthX", New NInteger(0))

                fontRos.setFdSelect(fdSelect)
            Else
                Dim privateEntry As DictData.Entry = topDict.getEntry("Private")
                Dim privateOffset As Integer = privateEntry.getNumber(1).intValue()
                input.setPosition(privateOffset)
                Dim privateSize As Integer = privateEntry.getNumber(0).intValue()
                Dim privateDictData As New CFFDataInput(input.readBytes(privateSize))
                Dim privateDict As DictData = readDictData(privateDictData)
                font.addValueToPrivateDict("BlueValues", getDelta(privateDict, "BlueValues", Nothing))
                font.addValueToPrivateDict("OtherBlues", getDelta(privateDict, "OtherBlues", Nothing))
                font.addValueToPrivateDict("FamilyBlues", getDelta(privateDict, "FamilyBlues", Nothing))
                font.addValueToPrivateDict("FamilyOtherBlues", getDelta(privateDict, "FamilyOtherBlues", Nothing))
                font.addValueToPrivateDict("BlueScale", getNumber(privateDict, "BlueScale", New NDouble(0.039625)))
                font.addValueToPrivateDict("BlueShift", getNumber(privateDict, "BlueShift", New NInteger(7)))
                font.addValueToPrivateDict("BlueFuzz", getNumber(privateDict, "BlueFuzz", New NInteger(1)))
                font.addValueToPrivateDict("StdHW", getNumber(privateDict, "StdHW", Nothing))
                font.addValueToPrivateDict("StdVW", getNumber(privateDict, "StdVW", Nothing))
                font.addValueToPrivateDict("StemSnapH", getDelta(privateDict, "StemSnapH", Nothing))
                font.addValueToPrivateDict("StemSnapV", getDelta(privateDict, "StemSnapV", Nothing))
                font.addValueToPrivateDict("ForceBold", getBoolean(privateDict, "ForceBold", False))
                font.addValueToPrivateDict("LanguageGroup", getNumber(privateDict, "LanguageGroup", New NInteger(0)))
                font.addValueToPrivateDict("ExpansionFactor", getNumber(privateDict, "ExpansionFactor", New NDouble(0.059999999999999998)))
                font.addValueToPrivateDict("initialRandomSeed", getNumber(privateDict, "initialRandomSeed", New NInteger(0)))
                font.addValueToPrivateDict("defaultWidthX", getNumber(privateDict, "defaultWidthX", New NInteger(0)))
                font.addValueToPrivateDict("nominalWidthX", getNumber(privateDict, "nominalWidthX", New NInteger(0)))

                Dim localSubrOffset As NInteger = getNumber(privateDict, "Subrs", New NInteger(0))
                If (localSubrOffset = 0) Then
                    font.setLocalSubrIndex(New IndexData(0))
                Else
                    input.setPosition(privateOffset + localSubrOffset)
                    font.setLocalSubrIndex(readIndexData(input))
                End If
            End If

            Return font
        End Function

        Private Function readString(ByVal index As Integer) As String
            If (index >= 0 AndAlso index <= 390) Then
                Return CFFStandardString.getName(index)
            End If
            If (index - 391 <= stringIndex.getCount()) Then
                Dim dataInput As New DataInput(stringIndex.getBytes(index - 391))
                Return dataInput.getString()
            Else
                Return CFFStandardString.getName(0)
            End If
        End Function

        Private Function getString(ByVal dict As DictData, ByVal name As String) As String
            Dim entry As DictData.Entry = dict.getEntry(name)
            If (entry IsNot Nothing) Then
                Return readString(entry.getNumber(0).intValue())
            Else
                Return vbNullString
            End If
        End Function

        Private Function getBoolean(ByVal dict As DictData, ByVal name As String, ByVal defaultValue As Boolean) As Boolean
            Dim entry As DictData.Entry = dict.getEntry(name)
            If (entry IsNot Nothing) Then
                Return entry.getBoolean(0)
            Else
                Return defaultValue
            End If
        End Function

        Private Function getNumber(ByVal dict As DictData, ByVal name As String, ByVal defaultValue As Number) As Number
            Dim entry As DictData.Entry = dict.getEntry(name)
            If (entry IsNot Nothing) Then
                Return entry.getNumber(0)
            Else
                Return defaultValue
            End If
        End Function

        'Private Function getNumber(ByVal dict As DictData, ByVal name As String, ByVal defaultValue As NInteger) As NInteger
        '    Dim entry As DictData.Entry = dict.getEntry(name)
        '    If (entry IsNot Nothing) Then
        '        Return entry.getNumber(0)
        '    Else
        '        Return defaultValue
        '    End If
        'End Function

        'Private Function getNumber(ByVal dict As DictData, ByVal name As String, ByVal defaultValue As NDouble) As NDouble
        '    Dim entry As DictData.Entry = dict.getEntry(name)
        '    If (entry IsNot Nothing) Then
        '        Return entry.getNumber(0)
        '    Else
        '        Return defaultValue
        '    End If
        'End Function

        ' TODO Where is the difference to getDelta??
        Private Function getArray(ByVal dict As DictData, ByVal name As String, ByVal defaultValue As List(Of Number)) As List(Of Number)
            Dim entry As DictData.Entry = dict.getEntry(name)
            If (entry IsNot Nothing) Then
                Return entry.getArray()
            Else
                Return defaultValue
            End If
        End Function

        ' TODO Where is the difference to getArray??
        Private Function getDelta(ByVal dict As DictData, ByVal name As String, ByVal defaultValue As List(Of Number)) As List(Of Number)
            Dim entry As DictData.Entry = dict.getEntry(name)
            If (entry IsNot Nothing) Then
                Return entry.getArray()
            Else
                Return defaultValue
            End If
        End Function

        Private Function readEncoding(ByVal dataInput As CFFDataInput, ByVal gids() As Integer) As CFFEncoding
            Dim format As Integer = dataInput.readCard8()
            Dim baseFormat As Integer = format And &H7F

            If (baseFormat = 0) Then
                Return readFormat0Encoding(dataInput, format, gids)
            ElseIf (baseFormat = 1) Then
                Return readFormat1Encoding(dataInput, format, gids)
            Else
                Throw New ArgumentException()
            End If
        End Function

        Private Function readFormat0Encoding(ByVal dataInput As CFFDataInput, ByVal format As Integer, ByVal gids() As Integer) As Format0Encoding
            Dim encoding As New Format0Encoding()
            encoding.format = format
            encoding.nCodes = dataInput.readCard8()
            encoding.code = Array.CreateInstance(GetType(Integer), encoding.nCodes)
            For i As Integer = 0 To encoding.code.Length - 1
                encoding.code(i) = dataInput.readCard8()
                encoding.register(encoding.code(i), gids(i))
            Next
            If ((format And &H80) <> 0) Then
                readSupplement(dataInput, encoding)
            End If
            Return encoding
        End Function

        Private Function readFormat1Encoding(ByVal dataInput As CFFDataInput, ByVal format As Integer, ByVal gids() As Integer) As Format1Encoding
            Dim encoding As New Format1Encoding()
            encoding.format = format
            encoding.nRanges = dataInput.readCard8()
            Dim count As Integer = 0
            encoding.range = Array.CreateInstance(GetType(Format1Encoding.Range1), encoding.nRanges)
            For i As Integer = 0 To encoding.range.Length - 1
                Dim range As New Format1Encoding.Range1() 'Format1Encoding.Range1 
                range.first = dataInput.readCard8()
                range.nLeft = dataInput.readCard8()
                encoding.range(i) = range
                For j As Integer = 0 To 1 + range.nLeft - 1
                    encoding.register(range.first + j, gids(count + j))
                Next
                count += 1 + range.nLeft
            Next
            If ((format And &H80) <> 0) Then
                readSupplement(dataInput, encoding)
            End If
            Return encoding
        End Function

        Private Sub readSupplement(ByVal dataInput As CFFDataInput, ByVal encoding As EmbeddedEncoding)
            encoding.nSups = dataInput.readCard8()
            encoding._supplement = Array.CreateInstance(GetType(EmbeddedEncoding.Supplement), encoding.nSups)
            For i As Integer = 0 To encoding._supplement.Length - 1
                Dim supplement As New EmbeddedEncoding.Supplement()
                supplement.code = dataInput.readCard8()
                supplement.glyph = dataInput.readSID()
                encoding._supplement(i) = supplement
            Next
        End Sub

        '/**
        ' * Read the FDSelect Data according to the format.
        ' * @param dataInput
        ' * @param nGlyphs
        ' * @param ros
        ' * @return
        ' * @throws IOException
        ' */
        Private Function readFDSelect(ByVal dataInput As CFFDataInput, ByVal nGlyphs As Integer, ByVal ros As CFFFontROS) As CIDKeyedFDSelect
            Dim format As Integer = dataInput.readCard8()
            If (format = 0) Then
                Return readFormat0FDSelect(dataInput, format, nGlyphs, ros)
            ElseIf (format = 3) Then
                Return readFormat3FDSelect(dataInput, format, nGlyphs, ros)
            Else
                Throw New ArgumentException()
            End If
        End Function

        '/**
        ' * Read the Format 0 of the FDSelect data structure.
        ' * @param dataInput
        ' * @param format
        ' * @param nGlyphs
        ' * @param ros
        ' * @return
        ' * @throws IOException
        ' */
        Private Function readFormat0FDSelect(ByVal dataInput As CFFDataInput, ByVal format As Integer, ByVal nGlyphs As Integer, ByVal ros As CFFFontROS) As Format0FDSelect
            Dim fdselect As New Format0FDSelect(ros)
            fdselect.format = format
            fdselect.fds = Array.CreateInstance(GetType(Integer), nGlyphs)
            For i As Integer = 0 To fdselect.fds.Length - 1
                fdselect.fds(i) = dataInput.readCard8()
            Next
            Return fdselect
        End Function

        '/**
        ' * Read the Format 3 of the FDSelect data structure.
        ' * 
        ' * @param dataInput
        ' * @param format
        ' * @param nGlyphs
        ' * @param ros
        ' * @return
        ' * @throws IOException
        ' */
        Private Function readFormat3FDSelect(ByVal dataInput As CFFDataInput, ByVal format As Integer, ByVal nGlyphs As Integer, ByVal ros As CFFFontROS) As Format3FDSelect
            Dim fdselect As New Format3FDSelect(ros)
            fdselect.format = format
            fdselect.nbRanges = dataInput.readCard16()

            fdselect.range3 = Array.CreateInstance(GetType(Range3), fdselect.nbRanges)
            For i As Integer = 0 To fdselect.nbRanges - 1
                Dim r3 As New Range3()
                r3.first = dataInput.readCard16()
                r3.fd = dataInput.readCard8()
                fdselect.range3(i) = r3
            Next
            fdselect.sentinel = dataInput.readCard16()
            Return fdselect
        End Function

        '/**
        ' *  Container of a Format 3 FDSelect data (see "The Compact Font Format Specification" chapter "FDSelect" ).
        ' */
        Friend NotInheritable Class Format3FDSelect
            Inherits CIDKeyedFDSelect

            Friend format As Integer
            Friend nbRanges As Integer
            Friend range3() As Range3
            Friend sentinel As Integer

            Friend Sub New(ByVal owner As CFFFontROS)
                MyBase.New(owner)
            End Sub

            '/*
            ' * (non-Javadoc)
            ' * 
            ' * @see org.apache.fontbox.cff.CIDKeyedFDSelect#getFd(int)
            ' */
            '@Override
            Public Overrides Function getFd(ByVal glyph As Integer) As Integer
                For i As Integer = 0 To nbRanges - 1
                    If (range3(i).first >= glyph) Then
                        If (i + 1 < nbRanges) Then
                            If (range3(i + 1).first > glyph) Then
                                Return range3(i).fd
                            Else
                                ' go to next range
                            End If
                        Else
                            ' last range reach, the sentinel must be greater than glyph
                            If (sentinel > glyph) Then
                                Return range3(i).fd
                            Else
                                Return -1
                            End If
                        End If
                    End If
                Next
                Return 0
            End Function

            Public Overrides Function toString() As String
                Return Me.GetType().Name & "[format=" & format & " nbRanges=" & nbRanges & ", range3=" & Arrays.ToString(range3) & " sentinel=" & sentinel & "]"
            End Function
        End Class

        '/**
        ' * Structure of a Range3 element.
        ' */
        Friend Class Range3
            Friend first As Integer
            Friend fd As Integer

            Public Overrides Function toString() As String
                Return Me.GetType().Name & "[first=" & first & ", fd=" & fd & "]"
            End Function
        End Class

        '/**
        ' *  Container of a Format 0 FDSelect data (see "The Compact Font Format Specification" chapter "FDSelect" ).
        ' */
        Friend Class Format0FDSelect
            Inherits CIDKeyedFDSelect

            Friend format As Integer
            Friend fds() As Integer

            Public Sub New(ByVal owner As CFFFontROS)
                MyBase.New(owner)
            End Sub

            Public Overrides Function getFd(ByVal glyph As Integer) As Integer
                Dim charString As Map(Of String, Byte()) = owner.getCharStringsDict()
                Dim keys As [Set](Of String) = charString.keySet()
                ' ---- search the position of the given glyph
                For Each mapping As Mapping In owner.getMappings()
                    If (mapping.getSID() = glyph AndAlso charString.containsKey(mapping.getName())) Then
                        Dim index As Integer = 0
                        For Each str As String In keys
                            If (mapping.getName().Equals(str)) Then
                                Return fds(index)
                            End If
                            index += 1
                        Next
                    End If
                Next
                Return -1
            End Function

            Public Overrides Function toString() As String
                Return Me.GetType().Name & "[format=" & format & ", fds=" & Arrays.ToString(fds) & "]"
            End Function

        End Class


        Private Function readCharset(ByVal dataInput As CFFDataInput, ByVal nGlyphs As Integer) As CFFCharset
            Dim format As Integer = dataInput.readCard8()
            If (format = 0) Then
                Return readFormat0Charset(dataInput, format, nGlyphs)
            ElseIf (format = 1) Then
                Return readFormat1Charset(dataInput, format, nGlyphs)
            ElseIf (format = 2) Then
                Return readFormat2Charset(dataInput, format, nGlyphs)
            Else
                Throw New ArgumentException()
            End If
        End Function

        Private Function readFormat0Charset(ByVal dataInput As CFFDataInput, ByVal format As Integer, ByVal nGlyphs As Integer) As Format0Charset
            Dim charset As New Format0Charset()
            charset.format = format
            charset.glyph = Array.CreateInstance(GetType(Integer), nGlyphs - 1)
            For i As Integer = 0 To charset.glyph.Length - 1
                charset.glyph(i) = dataInput.readSID()
                charset.register(charset.glyph(i), readString(charset.glyph(i)))
            Next
            Return charset
        End Function

        Private Function readFormat1Charset(ByVal dataInput As CFFDataInput, ByVal format As Integer, ByVal nGlyphs As Integer) As Format1Charset
            Dim charset As New Format1Charset()
            charset.format = format
            Dim ranges As List(Of Format1Charset.Range1) = New ArrayList(Of Format1Charset.Range1)()
            For i As Integer = 0 To nGlyphs - 1 - 1
                Dim range As New Format1Charset.Range1()
                range.first = dataInput.readSID()
                range.nLeft = dataInput.readCard8()
                ranges.add(range)
                For j As Integer = 0 To 1 + range.nLeft - 1
                    charset.register(range.first + j, readString(range.first + j))
                Next
                i += 1 + range.nLeft
            Next
            charset.range = ranges.toArray(Of Format1Charset.Range1)()
            Return charset
        End Function

        Private Function readFormat2Charset(ByVal dataInput As CFFDataInput, ByVal format As Integer, ByVal nGlyphs As Integer) As Format2Charset
            Dim charset As New Format2Charset()
            charset.format = format
            charset.range = Array.CreateInstance(GetType(Format2Charset.Range2), 0) '?0
            For i As Integer = 0 To nGlyphs - 1 - 1
                Dim newRange As Format2Charset.Range2() = Array.CreateInstance(GetType(Format2Charset.Range2), charset.range.Length + 1)
                Array.Copy(charset.range, 0, newRange, 0, charset.range.Length)
                charset.range = newRange
                Dim range As New Format2Charset.Range2()
                range.first = dataInput.readSID()
                range.nLeft = dataInput.readCard16()
                charset.range(charset.range.Length - 1) = range
                For j As Integer = 0 To 1 + range.nLeft - 1
                    charset.register(range.first + j, readString(range.first + j))
                Next
                i += 1 + range.nLeft
            Next
            Return charset
        End Function

        '/**
        ' * Inner class holding the _header of a CFF font. 
        ' */
        Friend Class Header
            Friend major As Integer
            Friend minor As Integer
            Friend hdrSize As Integer
            Friend offSize As Integer

            Public Overrides Function toString() As String
                Return Me.GetType().Name & "[major=" & major & ", minor=" & minor & ", hdrSize=" & hdrSize & ", offSize=" & offSize & "]"
            End Function
        End Class

        '/**
        ' * Inner class holding the DictData of a CFF font. 
        ' */
        Friend Class DictData

            Friend entries As List(Of Entry) = Nothing

            Public Function getEntry(ByVal key As CFFOperator.Key) As Entry
                Return getEntry(CFFOperator.getOperator(key))
            End Function

            Public Function getEntry(ByVal name As String) As Entry
                Return getEntry(CFFOperator.getOperator(name))
            End Function

            Private Function getEntry(ByVal [operator] As CFFOperator) As Entry
                For Each entry As Entry In entries
                    ' Check for null entry before comparing the Font
                    If (entry IsNot Nothing AndAlso entry.operator IsNot Nothing AndAlso entry.operator.equals([operator])) Then
                        Return entry
                    End If
                Next
                Return Nothing
            End Function

            Public Overrides Function toString() As String
                Return Me.GetType().Name & "[entries=" & entries.ToString & "]"
            End Function

            '/**
            ' * Inner class holding an operand of a CFF font. 
            ' */
            Friend Class Entry
                Friend operands As List(Of Number) = New ArrayList(Of Number)()
                Friend [operator] As CFFOperator = Nothing

                Public Function getNumber(ByVal index As Integer) As Number
                    Return operands.get(index)
                End Function

                Public Function getBoolean(ByVal index As Integer) As Boolean
                    Dim operand As Number = operands.get(index)
                    If (TypeOf (operand) Is NInteger) Then
                        Select Case (operand.intValue())
                            Case 0 : Return False ' Boolean.FALSE
                            Case 1 : Return True ' Boolean.TRUE
                            Case Else
                        End Select
                    End If
                    Throw New ArgumentException()
                End Function

                ' TODO unused??
                Public Function getSID(ByVal index As Integer) As NInteger
                    Dim operand As Number = operands.get(index)
                    If (TypeOf (operand) Is NInteger) Then
                        Return operand
                    End If
                    Throw New ArgumentException()
                End Function

                ' TODO Where is the difference to getDelta??
                Public Function getArray() As List(Of Number)
                    Return operands
                End Function

                ' TODO Where is the difference to getArray??
                Public Function getDelta() As List(Of Number)
                    Return operands
                End Function

                Public Overrides Function toString() As String
                    Return Me.GetType().Name & "[operands=" & operands.ToString & ", operator=" & [operator].toString & "]"
                End Function
            End Class

        End Class



        '/**
        ' * Inner class representing an embedded CFF encoding. 
        ' */
        Friend MustInherit Class EmbeddedEncoding
            Inherits CFFEncoding

            Friend nSups As Integer
            Friend _supplement() As Supplement

            Public Overrides Function isFontSpecific() As Boolean
                Return True
            End Function

            Public Function getSupplements() As List(Of Supplement)
                If (_supplement Is Nothing) Then
                    Return New ArrayList(Of Supplement)  'Collections.(Of Supplement) emptyList()
                End If
                Return New ArrayList(Of Supplement)(_supplement)
            End Function

            '/**
            ' * Inner class representing a supplement for an encoding. 
            ' */
            Friend Class Supplement
                Friend code As Integer
                Friend glyph As Integer

                Function getCode() As Integer
                    Return code
                End Function

                Function getGlyph() As Integer
                    Return glyph
                End Function

                Public Overrides Function toString() As String
                    Return Me.GetType().Name & "[code=" & code & ", glyph=" & glyph & "]"
                End Function
            End Class

        End Class


        '/**
        ' * Inner class representing a Format0 encoding. 
        ' */
        Friend Class Format0Encoding
            Inherits EmbeddedEncoding

            Friend format As Integer
            Friend nCodes As Integer
            Friend code() As Integer

            Public Overrides Function toString() As String
                Return Me.GetType().Name & "[format=" & format & ", nCodes=" & nCodes & ", code=" & Arrays.ToString(code) & ", supplement=" & Arrays.ToString(MyBase._supplement) & "]"
            End Function

        End Class

        '/**
        ' * Inner class representing a Format1 encoding. 
        ' */
        Friend Class Format1Encoding
            Inherits EmbeddedEncoding

            Friend format As Integer
            Friend nRanges As Integer
            Friend range() As Range1

            Public Overrides Function toString() As String
                Return Me.GetType().Name & "[format=" & format & ", nRanges=" & nRanges & ", range=" & Arrays.ToString(range) & ", supplement=" & Arrays.ToString(MyBase._supplement) & "]"
            End Function

            '/**
            ' * Inner class representing a range of an encoding. 
            ' */
            Friend Class Range1
                Friend first As Integer
                Friend nLeft As Integer

                Public Overrides Function toString() As String
                    Return Me.GetType().Name & "[first=" & first & ", nLeft=" & nLeft & "]"
                End Function
            End Class
        End Class

        '/**
        ' * Inner class representing an embedded CFF charset. 
        ' */
        Friend MustInherit Class EmbeddedCharset
            Inherits CFFCharset

            Public Overrides Function isFontSpecific() As Boolean
                Return True
            End Function
        End Class

        '/**
        ' * Inner class representing a Format0 charset. 
        ' */
        Friend Class Format0Charset
            Inherits EmbeddedCharset

            Friend format As Integer
            Friend glyph() As Integer

            Public Overrides Function toString() As String
                Return Me.GetType().Name & "[format=" & format & ", glyph=" & Arrays.ToString(glyph) & "]"
            End Function

        End Class

        '/**
        ' * Inner class representing a Format1 charset. 
        ' */
        Friend Class Format1Charset
            Inherits EmbeddedCharset

            Friend format As Integer
            Friend range As Range1()

            Public Overrides Function toString() As String
                Return Me.GetType().Name & "[format=" & format & ", range=" & Arrays.ToString(range) & "]"
            End Function

            '/**
            ' * Inner class representing a range of a charset. 
            ' */
            Friend Class Range1
                Friend first As Integer
                Friend nLeft As Integer

                Public Overrides Function toString() As String
                    Return Me.GetType().Name & "[first=" & first & ", nLeft=" & nLeft & "]"
                End Function
            End Class

        End Class

        '/**
        ' * Inner class representing a Format2 charset. 
        ' */
        Friend Class Format2Charset
            Inherits EmbeddedCharset

            Friend format As Integer
            Friend range() As Range2

            Public Overrides Function toString() As String
                Return Me.GetType().Name & "[format=" & format & ", range=" & Arrays.ToString(range) & "]"
            End Function

            '/**
            ' * Inner class representing a range of a charset. 
            ' */
            Friend Class Range2
                Friend first As Integer
                Friend nLeft As Integer

                Public Overrides Function toString() As String
                    Return Me.GetType().Name & "[first=" & first & ", nLeft=" & nLeft & "]"
                End Function
            End Class

        End Class


    End Class

End Namespace

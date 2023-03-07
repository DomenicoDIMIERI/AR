Imports FinSeA.Sistema
Imports System.IO


Namespace org.apache.fontbox.ttf

    '/**
    ' * An encoding entry for a cmap.
    ' * 
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.2 $
    ' */
    Public Class CMAPEncodingEntry

        Private platformId As Integer
        Private platformEncodingId As Integer
        Private subTableOffset As Long
        Private glyphIdToCharacterCode() As Integer
        Private characterCodeToGlyphId As Map(Of NInteger, NInteger) = New HashMap(Of NInteger, NInteger)()

        '/**
        ' * This will read the required data from the stream.
        ' * 
        ' * @param ttf The font that is being read.
        ' * @param data The stream to read the data from.
        ' * @throws IOException If there is an error reading the data.
        ' */
        Public Sub initData(ByVal ttf As TrueTypeFont, ByVal data As TTFDataStream)
            platformId = data.readUnsignedShort()
            platformEncodingId = data.readUnsignedShort()
            subTableOffset = data.readUnsignedInt()
        End Sub

        '/**
        ' * This will read the required data from the stream.
        ' * 
        ' * @param ttf The font that is being read.
        ' * @param data The stream to read the data from.
        ' * @throws IOException If there is an error reading the data.
        ' */
        Public Sub initSubtable(ByVal ttf As TrueTypeFont, ByVal data As TTFDataStream)
            data.seek(ttf.getCMAP().getOffset() + subTableOffset)
            Dim subtableFormat As Integer = data.readUnsignedShort()
            Dim length As Long
            Dim version As Long
            Dim numGlyphs As Integer
            If (subtableFormat < 8) Then
                length = data.readUnsignedShort()
                version = data.readUnsignedShort()
                numGlyphs = ttf.getMaximumProfile().getNumGlyphs()
            Else
                ' read an other UnsignedShort to read a Fixed32
                data.readUnsignedShort()
                length = data.readUnsignedInt()
                version = data.readUnsignedInt()
                numGlyphs = ttf.getMaximumProfile().getNumGlyphs()
            End If

            Select Case (subtableFormat)
                Case 0 : processSubtype0(ttf, data)
                Case 2 : processSubtype2(ttf, data, numGlyphs)
                Case 4 : processSubtype4(ttf, data, numGlyphs)
                Case 6 : processSubtype6(ttf, data, numGlyphs)
                Case 8 : processSubtype8(ttf, data, numGlyphs)
                Case 10 : processSubtype10(ttf, data, numGlyphs)
                Case 12 : processSubtype12(ttf, data, numGlyphs)
                Case 13 : processSubtype13(ttf, data, numGlyphs)
                Case 14 : processSubtype14(ttf, data, numGlyphs)
                Case Else
                    Throw New IOException("Unknown cmap format:" & subtableFormat)
            End Select
        End Sub

        '/**
        ' * Reads a format 8 subtable.
        ' * @param ttf the TrueTypeFont instance holding the parsed data.
        ' * @param data the data stream of the to be parsed ttf font
        ' * @param numGlyphs number of glyphs to be read
        ' * @throws IOException If there is an error parsing the true type font.
        ' */
        Protected Sub processSubtype8(ByVal ttf As TrueTypeFont, ByVal data As TTFDataStream, ByVal numGlyphs As Integer)
            ' --- is32 is a 65536 BITS array ( = 8192 BYTES) 
            Dim is32() As Integer = data.readUnsignedByteArray(8192)
            Dim nbGroups As Long = data.readUnsignedInt()

            ' --- nbGroups shouldn't be greater than 65536
            If (nbGroups > 65536) Then
                Throw New IOException("CMap ( Subtype8 ) is invalid")
            End If

            glyphIdToCharacterCode = Arrays.CreateInstance(Of Integer)(numGlyphs)
            ' -- Read all sub header
            For i As Long = 0 To nbGroups
                Dim firstCode As Long = data.readUnsignedInt()
                Dim endCode As Long = data.readUnsignedInt()
                Dim startGlyph As Long = data.readUnsignedInt()

                ' -- process simple validation
                If (firstCode > endCode OrElse 0 > firstCode) Then
                    Throw New IOException("Range invalid")
                End If

                For j As Long = firstCode To endCode
                    ' -- Convert the Character code in decimal
                    If (j > Integer.MaxValue) Then
                        Throw New IOException("[Sub Format 8] Invalid Character code")
                    End If

                    Dim currentCharCode As Integer
                    If ((is32(CInt(j) \ 8) And (1 << (CInt(j) Mod 8))) = 0) Then
                        currentCharCode = j
                    Else
                        ' the character code uses a 32bits format 
                        ' convert it in decimal : see http://www.unicode.org/faq//utf_bom.html#utf16-4
                        Dim LEAD_OFFSET As Long = &HD800 - (&H10000 >> 10)
                        Dim SURROGATE_OFFSET As Long = &H10000 - (&HD800 << 10) - &HDC00
                        Dim lead As Long = LEAD_OFFSET + (j >> 10)
                        Dim trail As Long = &HDC00 + (j And &H3FF)

                        Dim codepoint As Long = (lead << 10) + trail + SURROGATE_OFFSET
                        If (codepoint > Integer.MaxValue) Then
                            Throw New IOException("[Sub Format 8] Invalid Character code")
                        End If
                        currentCharCode = codepoint
                    End If

                    Dim glyphIndex As Long = startGlyph + (j - firstCode)
                    If (glyphIndex > numGlyphs OrElse glyphIndex > Integer.MaxValue) Then
                        Throw New IOException("CMap contains an invalid glyph index")
                    End If

                    glyphIdToCharacterCode(glyphIndex) = currentCharCode
                    characterCodeToGlyphId.put(currentCharCode, CInt(glyphIndex))
                Next
            Next
        End Sub

        '/**
        ' * Reads a format 10 subtable.
        ' * @param ttf the TrueTypeFont instance holding the parsed data.
        ' * @param data the data stream of the to be parsed ttf font
        ' * @param numGlyphs number of glyphs to be read
        ' * @throws IOException If there is an error parsing the true type font.
        ' */
        Protected Sub processSubtype10(ByVal ttf As TrueTypeFont, ByVal data As TTFDataStream, ByVal numGlyphs As Integer)
            Dim startCode As Long = data.readUnsignedInt()
            Dim numChars As Long = data.readUnsignedInt()
            If (numChars > Integer.MaxValue) Then
                Throw New IOException("Invalid number of Characters")
            End If

            If (startCode < 0 OrElse startCode > &H10FFFF OrElse (startCode + numChars) > &H10FFFF OrElse ((startCode + numChars) >= &HD800 AndAlso (startCode + numChars) <= &HDFFF)) Then
                Throw New IOException("Invalid Characters codes")
            End If
        End Sub

        '/**
        ' * Reads a format 12 subtable.
        ' * @param ttf the TrueTypeFont instance holding the parsed data.
        ' * @param data the data stream of the to be parsed ttf font
        ' * @param numGlyphs number of glyphs to be read
        ' * @throws IOException If there is an error parsing the true type font.
        ' */
        Protected Sub processSubtype12(ByVal ttf As TrueTypeFont, ByVal data As TTFDataStream, ByVal numGlyphs As Integer)
            Dim nbGroups As Long = data.readUnsignedInt()
            glyphIdToCharacterCode = Arrays.CreateInstance(Of Integer)(numGlyphs)
            For i As Long = 0 To nbGroups
                Dim firstCode As Long = data.readUnsignedInt()
                Dim endCode As Long = data.readUnsignedInt()
                Dim startGlyph As Long = data.readUnsignedInt()

                If (firstCode < 0 OrElse firstCode > &H10FFFF OrElse (firstCode >= &HD800 AndAlso firstCode <= &HDFFF)) Then
                    Throw New IOException("Invalid Characters codes")
                End If

                If (endCode > 0 AndAlso (endCode < firstCode OrElse endCode > &H10FFFF OrElse (endCode >= &HD800 AndAlso endCode <= &HDFFF))) Then
                    Throw New IOException("Invalid Characters codes")
                End If

                For j As Long = 0 To (endCode - firstCode)
                    If ((firstCode + j) > Integer.MaxValue) Then
                        Throw New IOException("Character Code greater than Integer.MaxValue")
                    End If

                    Dim glyphIndex As Long = (startGlyph + j)
                    If (glyphIndex > numGlyphs OrElse glyphIndex > Integer.MaxValue) Then
                        Throw New IOException("CMap contains an invalid glyph index")
                    End If
                    glyphIdToCharacterCode(glyphIndex) = (firstCode + j)
                    characterCodeToGlyphId.put(firstCode + j, glyphIndex)
                Next
            Next
        End Sub

        '/**
        ' * Reads a format 13 subtable.
        ' * @param ttf the TrueTypeFont instance holding the parsed data.
        ' * @param data the data stream of the to be parsed ttf font
        ' * @param numGlyphs number of glyphs to be read
        ' * @throws IOException If there is an error parsing the true type font.
        ' */
        Protected Sub processSubtype13(ByVal ttf As TrueTypeFont, ByVal data As TTFDataStream, ByVal numGlyphs As Integer)
            Dim nbGroups As Long = data.readUnsignedInt()
            For i As Long = 0 To nbGroups
                Dim firstCode As Long = data.readUnsignedInt()
                Dim endCode As Long = data.readUnsignedInt()
                Dim glyphId As Long = data.readUnsignedInt()

                If (glyphId > numGlyphs) Then
                    Throw New IOException("CMap contains an invalid glyph index")
                End If

                If (firstCode < 0 OrElse firstCode > &H10FFFF OrElse (firstCode >= &HD800 AndAlso firstCode <= &HDFFF)) Then
                    Throw New IOException("Invalid Characters codes")
                End If

                If (endCode > 0 AndAlso (endCode < firstCode OrElse endCode > &H10FFFF OrElse (endCode >= &HD800 AndAlso endCode <= &HDFFF))) Then
                    Throw New IOException("Invalid Characters codes")
                End If

                For j As Long = 0 To (endCode - firstCode)
                    If ((firstCode + j) > Integer.MaxValue) Then
                        Throw New IOException("Character Code greater than Integer.MaxValue")
                    End If
                    glyphIdToCharacterCode(glyphId) = (firstCode + j)
                    characterCodeToGlyphId.put(firstCode + j, glyphId)
                Next
            Next
        End Sub

        '/**
        ' * Reads a format 14 subtable.
        ' * @param ttf the TrueTypeFont instance holding the parsed data.
        ' * @param data the data stream of the to be parsed ttf font
        ' * @param numGlyphs number of glyphs to be read
        ' * @throws IOException If there is an error parsing the true type font.
        ' */
        Protected Sub processSubtype14(ByVal ttf As TrueTypeFont, ByVal data As TTFDataStream, ByVal numGlyphs As Integer)
            Throw New NotImplementedException("CMap subtype 14 not yet implemented")
        End Sub

        '/**
        ' * Reads a format 6 subtable.
        ' * @param ttf the TrueTypeFont instance holding the parsed data.
        ' * @param data the data stream of the to be parsed ttf font
        ' * @param numGlyphs number of glyphs to be read
        ' * @throws IOException If there is an error parsing the true type font.
        ' */
        Protected Sub processSubtype6(ByVal ttf As TrueTypeFont, ByVal data As TTFDataStream, ByVal numGlyphs As Integer)
            Dim firstCode As Integer = data.readUnsignedShort()
            Dim entryCount As Integer = data.readUnsignedShort()
            glyphIdToCharacterCode = Arrays.CreateInstance(Of Integer)(numGlyphs)
            Dim glyphIdArray() As Integer = data.readUnsignedShortArray(entryCount)
            For i As Integer = 0 To entryCount - 1
                glyphIdToCharacterCode(glyphIdArray(i)) = firstCode + i
                characterCodeToGlyphId.put((firstCode + i), glyphIdArray(i))
            Next
        End Sub

        '/**
        ' * Reads a format 4 subtable.
        ' * @param ttf the TrueTypeFont instance holding the parsed data.
        ' * @param data the data stream of the to be parsed ttf font
        ' * @param numGlyphs number of glyphs to be read
        ' * @throws IOException If there is an error parsing the true type font.
        ' */
        Protected Sub processSubtype4(ByVal ttf As TrueTypeFont, ByVal data As TTFDataStream, ByVal numGlyphs As Integer)
            Dim segCountX2 As Integer = data.readUnsignedShort()
            Dim segCount As Integer = segCountX2 / 2
            Dim searchRange As Integer = data.readUnsignedShort()
            Dim entrySelector As Integer = data.readUnsignedShort()
            Dim rangeShift As Integer = data.readUnsignedShort()
            Dim endCount() As Integer = data.readUnsignedShortArray(segCount)
            Dim reservedPad As Integer = data.readUnsignedShort()
            Dim startCount() As Integer = data.readUnsignedShortArray(segCount)
            Dim idDelta() As Integer = data.readUnsignedShortArray(segCount)
            Dim idRangeOffset() As Integer = data.readUnsignedShortArray(segCount)

            Dim tmpGlyphToChar As Map(Of NInteger, NInteger) = New HashMap(Of NInteger, NInteger)()

            Dim currentPosition As Long = data.getCurrentPosition()

            For i As Integer = 0 To segCount - 1
                Dim start As Integer = startCount(i)
                Dim [end] As Integer = endCount(i)
                Dim delta As Integer = idDelta(i)
                Dim rangeOffset As Integer = idRangeOffset(i)
                If (start <> 65535 AndAlso [end] <> 65535) Then
                    For j As Integer = start To [end]
                        If (rangeOffset = 0) Then
                            Dim glyphid As Integer = (j + delta) Mod 65536
                            tmpGlyphToChar.put(glyphid, j)
                            characterCodeToGlyphId.put(j, glyphid)
                        Else
                            Dim glyphOffset As Long = currentPosition + ((rangeOffset / 2) + (j - start) + (i - segCount)) * 2
                            data.seek(glyphOffset)
                            Dim glyphIndex As Integer = data.readUnsignedShort()
                            If (glyphIndex <> 0) Then
                                glyphIndex += delta
                                glyphIndex = glyphIndex Mod 65536
                                If (Not tmpGlyphToChar.containsKey(glyphIndex)) Then
                                    tmpGlyphToChar.put(glyphIndex, j)
                                    characterCodeToGlyphId.put(j, glyphIndex)
                                End If
                            End If
                        End If
                    Next
                End If
            Next

            '/* Me is the final result
            ' * key=glyphId, value is character codes
            ' * Create an array that contains MAX(GlyphIds) element and fill Me array with the .notdef character
            ' */
            glyphIdToCharacterCode = Arrays.CreateInstance(Of Integer)(FinSeA.Math.Max(tmpGlyphToChar.keySet()) + 1)
            'Arrays.fill(glyphIdToCharacterCode, 0)
            For Each entry As Map.Entry(Of NInteger, NInteger) In tmpGlyphToChar.entrySet()
                ' link the glyphId with the right character code
                glyphIdToCharacterCode(entry.Key) = entry.Value
            Next
        End Sub

        '/**
        ' * Read a format 2 subtable.
        ' * @param ttf the TrueTypeFont instance holding the parsed data.
        ' * @param data the data stream of the to be parsed ttf font
        ' * @param numGlyphs number of glyphs to be read
        ' * @throws IOException If there is an error parsing the true type font.
        ' */
        Protected Sub processSubtype2(ByVal ttf As TrueTypeFont, ByVal data As TTFDataStream, ByVal numGlyphs As Integer)
            Dim subHeaderKeys() As Integer = Arrays.CreateInstance(Of Integer)(256)
            ' ---- keep the Max Index of the SubHeader array to know its length
            Dim maxSubHeaderIndex As Integer = 0
            For i As Integer = 0 To 256 - 1
                subHeaderKeys(i) = data.readUnsignedShort()
                maxSubHeaderIndex = Math.Max(maxSubHeaderIndex, (subHeaderKeys(i) \ 8))
            Next

            ' ---- Read all SubHeaders to avoid useless seek on DataSource
            Dim subHeaders() As SubHeader = Arrays.CreateInstance(Of SubHeader)(maxSubHeaderIndex + 1)
            For i As Integer = 0 To maxSubHeaderIndex
                Dim firstCode As Integer = data.readUnsignedShort()
                Dim entryCount As Integer = data.readUnsignedShort()
                Dim idDelta As Short = data.readSignedShort()
                Dim idRangeOffset As Integer = data.readUnsignedShort()
                subHeaders(i) = New SubHeader(firstCode, entryCount, idDelta, idRangeOffset)
            Next
            Dim startGlyphIndexOffset As Long = data.getCurrentPosition()
            glyphIdToCharacterCode = Arrays.CreateInstance(Of Integer)(numGlyphs)
            For i As Integer = 0 To maxSubHeaderIndex
                Dim sh As SubHeader = subHeaders(i)
                Dim firstCode As Integer = sh.getFirstCode()
                For j As Integer = 0 To sh.getEntryCount() - 1
                    ' ---- compute the Character Code
                    Dim charCode As Integer = (i * 8)
                    charCode = (charCode << 8) + (firstCode + j)

                    ' ---- Go to the CharacterCOde position in the Sub Array 
                    '      of the glyphIndexArray 
                    '      glyphIndexArray contains Unsigned Short so add (j * 2) bytes 
                    '      at the index position
                    data.seek(startGlyphIndexOffset + sh.getIdRangeOffset() + (j * 2))
                    Dim p As Integer = data.readUnsignedShort()
                    ' ---- compute the glyphIndex 
                    p = p + sh.getIdDelta() Mod 65536

                    glyphIdToCharacterCode(p) = charCode
                    characterCodeToGlyphId.put(charCode, p)
                Next
            Next
        End Sub

        '/**
        ' * Initialize the CMapEntry when it is a subtype 0
        ' * 
        ' * @param ttf
        ' * @param data
        ' * @throws IOException
        ' */
        Protected Sub processSubtype0(ByVal ttf As TrueTypeFont, ByVal data As TTFDataStream)
            Dim glyphMapping() As Byte = data.read(256)
            glyphIdToCharacterCode = Arrays.CreateInstance(Of Integer)(256)
            For i As Integer = 0 To glyphMapping.Length - 1
                Dim glyphIndex As Integer = (glyphMapping(i) + 256) Mod 256
                glyphIdToCharacterCode(glyphIndex) = i
                characterCodeToGlyphId.put(i, glyphIndex)
            Next
        End Sub

        '/**
        ' * @return Returns the glyphIdToCharacterCode.
        ' */
        Public Function getGlyphIdToCharacterCode() As Integer()
            Return glyphIdToCharacterCode
        End Function

        '/**
        ' * @param glyphIdToCharacterCodeValue The glyphIdToCharacterCode to set.
        ' */
        Public Sub setGlyphIdToCharacterCode(ByVal glyphIdToCharacterCodeValue() As Integer)
            Me.glyphIdToCharacterCode = glyphIdToCharacterCodeValue
        End Sub

        '/**
        ' * @return Returns the platformEncodingId.
        ' */
        Public Function getPlatformEncodingId() As Integer
            Return platformEncodingId
        End Function

        '/**
        ' * @param platformEncodingIdValue The platformEncodingId to set.
        ' */
        Public Sub setPlatformEncodingId(ByVal platformEncodingIdValue As Integer)
            Me.platformEncodingId = platformEncodingIdValue
        End Sub

        '/**
        ' * @return Returns the platformId.
        ' */
        Public Function getPlatformId() As Integer
            Return platformId
        End Function

        '/**
        ' * @param platformIdValue The platformId to set.
        ' */
        Public Sub setPlatformId(ByVal platformIdValue As Integer)
            Me.platformId = platformIdValue
        End Sub

        '/**
        ' * Returns the GlyphId linked with the given character code. 
        ' * @param characterCode
        ' * @return glyphId
        ' */
        Public Function getGlyphId(ByVal characterCode As Integer) As Integer
            If (Me.characterCodeToGlyphId.containsKey(characterCode)) Then
                Return Me.characterCodeToGlyphId.get(characterCode)
            Else
                Return 0
            End If
        End Function

        '/**
        ' * Class used to manage CMap - Format 2
        ' */
        Private Class SubHeader

            Private firstCode As Integer
            Private entryCount As Integer

            '/**
            ' * used to compute the GlyphIndex :
            ' * P = glyphIndexArray.SubArray[pos]
            ' * GlyphIndex = P + idDelta % 65536
            ' */
            Private idDelta As Short

            '/**
            ' * Number of bytes to skip to reach the firstCode in the 
            ' * glyphIndexArray 
            ' */
            Private idRangeOffset As Integer

            Public Sub New(ByVal firstCode As Integer, ByVal entryCount As Integer, ByVal idDelta As Short, ByVal idRangeOffset As Integer)
                Me.firstCode = firstCode
                Me.entryCount = entryCount
                Me.idDelta = idDelta
                Me.idRangeOffset = idRangeOffset
            End Sub

            '/**
            ' * @return the firstCode
            ' */
            Public Function getFirstCode() As Integer
                Return firstCode
            End Function

            '/**
            ' * @return the entryCount
            ' */
            Public Function getEntryCount() As Integer
                Return entryCount
            End Function

            '/**
            ' * @return the idDelta
            ' */
            Public Function getIdDelta() As Short
                Return idDelta
            End Function

            '/**
            ' * @return the idRangeOffset
            ' */
            Public Function getIdRangeOffset() As Integer
                Return idRangeOffset
            End Function
        End Class


    End Class


End Namespace
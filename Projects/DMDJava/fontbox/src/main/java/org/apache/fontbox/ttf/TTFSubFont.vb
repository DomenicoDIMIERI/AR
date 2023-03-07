Imports FinSeA.Sistema
Imports System.IO
Imports FinSeA.Io
Imports FinSeA.org.apache.fontbox.encoding
Imports FinSeA.java

Namespace org.apache.fontbox.ttf


    '/**
    ' * A font, which is comprised of a subset of characters of a TrueType font.
    ' * Based on code developed by Wolfgang Glas
    ' * http://svn.clazzes.org/svn/sketch/trunk/pdf/pdf-entities/src/main/java/org/clazzes/sketch/pdf/entities/impl/TTFSubFont.java
    ' */
    Public Class TTFSubFont
        Private Shared ReadOnly PAD_BUF() As Byte = {0, 0, 0}

        Private baseTTF As TrueTypeFont
        Private nameSuffix As String
        Private baseCmap As CMAPEncodingEntry

        ' A map of unicode char codes to glyph IDs of the original font.
        Private characters As SortedMap(Of NInteger, NInteger)
        ' A sorted version of Me set will comprise the generated glyph IDs
        ' for the written truetype font.
        Private glyphIds As SortedSet(Of NInteger)

        '/**
        ' * Constructs a subfont based on the given font using the given suffix.
        ' * 
        ' * @param baseFont the base font of the subfont
        ' * @param suffix suffix used for the naming
        ' * 
        ' */
        Public Sub New(ByVal baseFont As TrueTypeFont, ByVal suffix As String)
            baseTTF = baseFont
            nameSuffix = suffix
            characters = New TreeMap(Of NInteger, NInteger)()
            glyphIds = New TreeSet(Of NInteger)()

            Dim cmaps As CMAPEncodingEntry() = Me.baseTTF.getCMAP().getCmaps()
            Dim unicodeCmap As CMAPEncodingEntry = Nothing

            For Each cmap As CMAPEncodingEntry In cmaps
                ' take first unicode map.
                If (cmap.getPlatformId() = 0 OrElse (cmap.getPlatformId() = 3 OrElse cmap.getPlatformEncodingId() = 1)) Then
                    unicodeCmap = cmap
                    Exit For
                End If
            Next
            baseCmap = unicodeCmap
            ' add notdef character.
            addCharCode(0)
        End Sub

        '/**
        ' * Add the given charcode to the subfpont.
        ' * 
        ' * @param charCode the charCode to be added
        ' * 
        ' */
        Public Sub addCharCode(ByVal charCode As Integer)
            Dim gid As NInteger = NInteger.valueOf(baseCmap.getGlyphId(charCode))
            If (charCode = 0 OrElse gid <> 0) Then
                characters.put(charCode, gid)
                glyphIds.add(gid)
            End If
        End Sub

        Private Shared Function log2i(ByVal i As Integer) As Integer
            Dim ret As Integer = -1
            If ((i And &HFFFF0000) <> 0) Then
                i >>= 16 '>>>
                ret += 16
            End If
            If ((i And &HFF00) <> 0) Then
                i >>= 8 '>>>
                ret += 8
            End If
            If ((i And &HF0) <> 0) Then
                i >>= 4 '>>>
                ret += 4
            End If
            If ((i And &HC) <> 0) Then
                i >>= 2 '>>>
                ret += 2
            End If
            If ((i And &H2) <> 0) Then
                i >>= 1 '>>>
                ret += 1
            End If
            If (i <> 0) Then
                ret += 1
            End If
            Return ret
        End Function

        Private Shared Function buildUint32(ByVal high As Integer, ByVal low As Integer) As Long
            Return ((CLng(high) And &HFFFF) << 16) Or (CLng(low) And &HFFFF)
        End Function

        Private Shared Function buildUint32(ByVal bytes() As Byte) As Long
            Return ((CLng(bytes(0)) And &HFF) << 24) Or ((CLng(bytes(1)) And &HFF) << 16) Or ((CLng(bytes(2)) And &HFF) << 8) Or (CLng(bytes(3)) And &HFF)
        End Function

        '/**
        ' * @param dos The data output stream.
        ' * @param nTables The number of table.
        ' * @return The file offset of the first TTF table to write.
        ' * @throws IOException Upon errors.
        ' */
        Private Shared Function writeFileHeader(ByVal dos As DataOutputStream, ByVal nTables As Integer) As Long
            dos.writeInt(&H10000)
            dos.writeShort(nTables)

            Dim mask As Integer = NInteger.highestOneBit(nTables)
            Dim searchRange As Integer = mask * 16
            dos.writeShort(searchRange)

            Dim entrySelector As Integer = log2i(mask)

            dos.writeShort(entrySelector)

            ' numTables * 16 - searchRange
            Dim last As Integer = 16 * nTables - searchRange
            dos.writeShort(last)

            Return &H10000L + buildUint32(nTables, searchRange) + buildUint32(entrySelector, last)
        End Function

        Private Shared Function writeTableHeader(ByVal dos As DataOutputStream, ByVal tag As String, ByVal offset As Long, ByVal bytes() As Byte) As Long
            Dim n As Integer = bytes.Length
            Dim nup As Integer
            Dim checksum As Long = 0L

            For nup = 0 To n - 1
                checksum += (CLng(bytes(nup)) And &HFFL) << (24 - (nup Mod 4) * 8)
            Next

            checksum = checksum And &HFFFFFFFF

            LOG.debug(String.Format("Writing table header [%s,%08x,%08x,%08x]", tag, checksum, offset, bytes.Length))

            Dim tagbytes() As Byte = Strings.GetBytes(tag, "US-ASCII")

            dos.write(tagbytes, 0, 4)
            dos.writeInt(checksum)
            dos.writeInt(offset)
            dos.writeInt(bytes.Length)

            ' account for the checksum twice, one time for the header field, on time for the content itself.
            Return buildUint32(tagbytes) + checksum + checksum + offset + bytes.Length
        End Function

        Private Shared Sub writeTableBody(ByVal os As OutputStream, ByVal bytes() As Byte)
            Dim n As Integer = bytes.Length
            os.Write(bytes)
            If ((n Mod 4) <> 0) Then
                os.Write(PAD_BUF, 0, 4 - n Mod 4)
            End If
        End Sub

        Private Shared Sub writeFixed(ByVal dos As DataOutputStream, ByVal f As Double)
            Dim ip As Double = Math.Floor(f)
            Dim fp As Double = (f - ip) * 65536.0
            dos.writeShort(ip)
            dos.writeShort(fp)
        End Sub

        Private Shared Sub writeUint32(ByVal dos As DataOutputStream, ByVal l As Long)
            dos.writeInt(l)
        End Sub

        Private Shared Sub writeUint16(ByVal dos As DataOutputStream, ByVal i As Integer)
            dos.writeShort(i)
        End Sub

        Private Shared Sub writeSint16(ByVal dos As DataOutputStream, ByVal i As Integer)
            dos.writeShort(i)
        End Sub

        Private Shared Sub writeUint8(ByVal dos As DataOutputStream, ByVal i As Integer)
            dos.writeByte(i)
        End Sub

        Private Shared Sub writeLongDateTime(ByVal dos As DataOutputStream, ByVal calendar As NDate)
            ' inverse operation of TTFDataStream.readInternationalDate()
            Dim cal As New NDate(1904, 0, 1) 'GregorianCalendar
            Dim millisFor1904 As Long = cal.getTimeInMillis()
            Dim secondsSince1904 As Long = (calendar.getTimeInMillis() - millisFor1904) / 1000L
            dos.writeLong(secondsSince1904)
        End Sub

        Private Function buildHeadTable() As Byte()
            Dim bos As New ByteArrayOutputStream()
            Dim dos As New DataOutputStream(bos)

            LOG.debug("Building table [head]...")

            Dim h As HeaderTable = Me.baseTTF.getHeader()

            writeFixed(dos, h.getVersion())
            writeFixed(dos, h.getFontRevision())
            writeUint32(dos, 0) '/* h.getCheckSumAdjustment() */
            writeUint32(dos, h.getMagicNumber())
            writeUint16(dos, h.getFlags())
            writeUint16(dos, h.getUnitsPerEm())
            writeLongDateTime(dos, h.getCreated())
            writeLongDateTime(dos, h.getModified())
            writeSint16(dos, h.getXMin())
            writeSint16(dos, h.getYMin())
            writeSint16(dos, h.getXMax())
            writeSint16(dos, h.getYMax())
            writeUint16(dos, h.getMacStyle())
            writeUint16(dos, h.getLowestRecPPEM())
            writeSint16(dos, h.getFontDirectionHint())
            ' force long format of 'loca' table.
            writeSint16(dos, 1) '/* h.getIndexToLocFormat() */
            writeSint16(dos, h.getGlyphDataFormat())
            dos.flush()

            LOG.debug("Finished table [head].")

            Dim ret() As Byte = bos.toByteArray()
            bos.Dispose()
            Return ret
        End Function

        Private Function buildHheaTable() As Byte()
            Dim bos As New ByteArrayOutputStream()
            Dim dos As New DataOutputStream(bos)

            LOG.debug("Building table [hhea]...")

            Dim h As HorizontalHeaderTable = Me.baseTTF.getHorizontalHeader()

            writeFixed(dos, h.getVersion())
            writeSint16(dos, h.getAscender())
            writeSint16(dos, h.getDescender())
            writeSint16(dos, h.getLineGap())
            writeUint16(dos, h.getAdvanceWidthMax())
            writeSint16(dos, h.getMinLeftSideBearing())
            writeSint16(dos, h.getMinRightSideBearing())
            writeSint16(dos, h.getXMaxExtent())
            writeSint16(dos, h.getCaretSlopeRise())
            writeSint16(dos, h.getCaretSlopeRun())
            writeSint16(dos, h.getReserved1()) ' caretOffset
            writeSint16(dos, h.getReserved2())
            writeSint16(dos, h.getReserved3())
            writeSint16(dos, h.getReserved4())
            writeSint16(dos, h.getReserved5())
            writeSint16(dos, h.getMetricDataFormat())
            writeUint16(dos, glyphIds.subSet(0, h.getNumberOfHMetrics()).size())

            dos.flush()
            LOG.debug("Finished table [hhea].")
            Dim ret() As Byte = bos.toByteArray()
            bos.Dispose()
            Return ret
        End Function

        Private Shared Function replicateNameRecord(ByVal nr As NameRecord) As Boolean
            Return nr.getPlatformId() = NameRecord.PLATFORM_WINDOWS AndAlso nr.getPlatformEncodingId() = NameRecord.PLATFORM_ENCODING_WINDOWS_UNICODE AndAlso nr.getLanguageId() = 0 AndAlso nr.getNameId() >= 0 AndAlso nr.getNameId() < 7
        End Function

        Private Function buildNameTable() As Byte()
            Dim bos As New ByteArrayOutputStream()
            Dim dos As New DataOutputStream(bos)

            LOG.debug("Building table [name]...")

            Dim n As NamingTable = Me.baseTTF.getNaming()
            Dim nameRecords As List(Of NameRecord) = Nothing
            If (n IsNot Nothing) Then
                nameRecords = n.getNameRecords()
            Else
                ' sometimes there is no naming table in an embedded subfonts
                ' create some dummies
                nameRecords = New ArrayList(Of NameRecord)()
                Dim nr As New NameRecord()
                nr.setPlatformId(NameRecord.PLATFORM_WINDOWS)
                nr.setPlatformEncodingId(NameRecord.PLATFORM_ENCODING_WINDOWS_UNICODE)
                nr.setLanguageId(0)
                nr.setNameId(NameRecord.NAME_FONT_FAMILY_NAME)
                nr.setString("PDFBox-Dummy-Familyname")
                nameRecords.add(nr)
                nr = New NameRecord()
                nr.setPlatformId(NameRecord.PLATFORM_WINDOWS)
                nr.setPlatformEncodingId(NameRecord.PLATFORM_ENCODING_WINDOWS_UNICODE)
                nr.setLanguageId(0)
                nr.setNameId(NameRecord.NAME_FULL_FONT_NAME)
                nr.setString("PDFBox-Dummy-Fullname")
                nameRecords.add(nr)
            End If
            Dim numberOfRecords As Integer = nameRecords.size()
            Dim nrep As Integer = 0
            For i As Integer = 0 To numberOfRecords - 1
                Dim nr As NameRecord = nameRecords.get(i)
                If (replicateNameRecord(nr)) Then
                    LOG.debug("Writing name record [" & nr.getNameId() & "], [" & nr.getString() & "],")
                    nrep += 1
                End If
            Next
            writeUint16(dos, 0)
            writeUint16(dos, nrep)
            writeUint16(dos, 2 * 3 + (2 * 6) * nrep)

            Dim names As Byte()() = Arrays.CreateInstance(Of Byte())(nrep)
            Dim j As Integer = 0
            For i As Integer = 0 To numberOfRecords - 1
                Dim nr As NameRecord = nameRecords.get(i)
                If (replicateNameRecord(nr)) Then
                    Dim platform As Integer = nr.getPlatformId()
                    Dim encoding As Integer = nr.getPlatformEncodingId()
                    Dim charset As String = "ISO-8859-1"
                    If (platform = 3 AndAlso encoding = 1) Then
                        charset = "UTF-16BE"
                    ElseIf (platform = 2) Then
                        If (encoding = 0) Then
                            charset = "US-ASCII"
                        ElseIf (encoding = 1) Then
                            'not sure is Me is correct??
                            charset = "UTF16-BE"
                        ElseIf (encoding = 2) Then
                            charset = "ISO-8859-1"
                        End If
                    End If
                    Dim value As String = nr.getString()
                    If (nr.getNameId() = 6 AndAlso Me.nameSuffix IsNot Nothing) Then
                        value += Me.nameSuffix
                    End If
                    names(j) = Strings.GetBytes(value, charset)
                    j += 1
                End If
            Next

            Dim offset As Integer = 0
            j = 0
            For i As Integer = 0 To numberOfRecords - 1
                Dim nr As NameRecord = nameRecords.get(i)
                If (replicateNameRecord(nr)) Then
                    writeUint16(dos, nr.getPlatformId())
                    writeUint16(dos, nr.getPlatformEncodingId())
                    writeUint16(dos, nr.getLanguageId())
                    writeUint16(dos, nr.getNameId())
                    writeUint16(dos, names(j).Length)
                    writeUint16(dos, offset)
                    offset += names(j).Length
                    j += 1
                End If
            Next

            For i As Integer = 0 To nrep - 1
                dos.write(names(i))
            Next
            dos.flush()
            LOG.debug("Finished table [name].")
            Dim ret() As Byte = bos.toByteArray()
            bos.Dispose()
            Return ret
        End Function

        Private Function buildMaxpTable() As Byte()
            Dim bos As New ByteArrayOutputStream()
            Dim dos As New DataOutputStream(bos)

            LOG.debug("Building table [maxp]...")

            Dim p As MaximumProfileTable = Me.baseTTF.getMaximumProfile()

            writeFixed(dos, 1.0)
            writeUint16(dos, glyphIds.size())
            writeUint16(dos, p.getMaxPoints())
            writeUint16(dos, p.getMaxContours())
            writeUint16(dos, p.getMaxCompositePoints())
            writeUint16(dos, p.getMaxCompositeContours())
            writeUint16(dos, p.getMaxZones())
            writeUint16(dos, p.getMaxTwilightPoints())
            writeUint16(dos, p.getMaxStorage())
            writeUint16(dos, p.getMaxFunctionDefs())
            writeUint16(dos, 0)
            writeUint16(dos, p.getMaxStackElements())
            writeUint16(dos, 0)
            writeUint16(dos, p.getMaxComponentElements())
            writeUint16(dos, p.getMaxComponentDepth())

            dos.flush()
            LOG.debug("Finished table [maxp].")
            Dim ret() As Byte = bos.toByteArray()
            bos.Dispose()
            Return ret
        End Function

        Private Function buildOS2Table() As Byte()
            Dim bos As New ByteArrayOutputStream()
            Dim dos As New DataOutputStream(bos)
            Dim os2 As OS2WindowsMetricsTable = Me.baseTTF.getOS2Windows()
            If (os2 Is Nothing) Then
                ' sometimes there is no OS2 table in an embedded subfonts
                ' create a dummy
                os2 = New OS2WindowsMetricsTable()
            End If

            LOG.debug("Building table [OS/2]...")

            writeUint16(dos, 0)
            writeSint16(dos, os2.getAverageCharWidth())
            writeUint16(dos, os2.getWeightClass())
            writeUint16(dos, os2.getWidthClass())

            writeSint16(dos, os2.getFsType())

            writeSint16(dos, os2.getSubscriptXSize())
            writeSint16(dos, os2.getSubscriptYSize())
            writeSint16(dos, os2.getSubscriptXOffset())
            writeSint16(dos, os2.getSubscriptYOffset())

            writeSint16(dos, os2.getSuperscriptXSize())
            writeSint16(dos, os2.getSuperscriptYSize())
            writeSint16(dos, os2.getSuperscriptXOffset())
            writeSint16(dos, os2.getSuperscriptYOffset())

            writeSint16(dos, os2.getStrikeoutSize())
            writeSint16(dos, os2.getStrikeoutPosition())
            writeUint8(dos, os2.getFamilyClass())
            writeUint8(dos, os2.getFamilySubClass())
            dos.write(os2.getPanose())

            writeUint32(dos, 0)
            writeUint32(dos, 0)
            writeUint32(dos, 0)
            writeUint32(dos, 0)

            dos.write(Strings.GetBytes(os2.getAchVendId(), "ISO-8859-1"))

            Dim it As Iterator(Of Map.Entry(Of NInteger, NInteger)) = characters.entrySet().iterator()
            it.next()
            Dim first As Map.Entry(Of NInteger, NInteger) = it.next()

            writeUint16(dos, os2.getFsSelection())
            writeUint16(dos, first.Key)
            writeUint16(dos, characters.lastKey())
            '/*
            ' * The mysterious Microsoft additions.
            ' *
            ' * SHORT    sTypoAscender    
            ' * SHORT    sTypoDescender    
            ' * SHORT    sTypoLineGap
            ' * USHORT    usWinAscent
            ' * USHORT    usWinDescent
            ' */
            writeUint16(dos, os2.getTypoAscender())
            writeUint16(dos, os2.getTypoDescender())
            writeUint16(dos, os2.getTypeLineGap())
            writeUint16(dos, os2.getWinAscent())
            writeUint16(dos, os2.getWinDescent())

            dos.flush()
            LOG.debug("Finished table [OS/2].")
            Dim ret() As Byte = bos.toByteArray()
            bos.Dispose()
            Return ret
        End Function

        Private Function buildLocaTable(ByVal newOffsets() As Long) As Byte()
            Dim bos As New ByteArrayOutputStream()
            Dim dos As New DataOutputStream(bos)

            LOG.debug("Building table [loca]...")

            For Each newOff As Long In newOffsets
                writeUint32(dos, newOff)
            Next
            dos.flush()
            LOG.debug("Finished table [loca].")
            Dim ret() As Byte = bos.toByteArray()
            bos.Dispose()
            Return ret
        End Function

        Private Function addCompoundReferences() As Boolean
            Dim g As GlyphTable = Me.baseTTF.getGlyph()
            Dim offsets() As Long = Me.baseTTF.getIndexToLocation().getOffsets()
            Dim [is] As InputStream = Me.baseTTF.getOriginalData()
            Dim glyphIdsToAdd As [Set](Of NInteger) = Nothing
            Try
                [is].skip(g.getOffset())
                Dim lastOff As Long = 0L
                For Each glyphId As NInteger In Me.glyphIds
                    Dim offset As Long = offsets(glyphId.Value)
                    Dim len As Long = offsets(glyphId.Value + 1) - offset
                    [is].skip(offset - lastOff)
                    Dim buf() As Byte = Arrays.CreateInstance(Of Byte)(len)
                    [is].read(buf)
                    ' rewrite glyphIds for compound glyphs
                    If (buf.Length >= 2 AndAlso buf(0) = -1 AndAlso buf(1) = -1) Then
                        Dim off As Integer = 2 * 5
                        Dim flags As Integer = 0
                        Do
                            flags = ((CInt(buf(off)) And &HFF) << 8) Or (buf(off + 1) And &HFF)
                            off += 2
                            Dim ogid As Integer = ((CInt(buf(off)) And &HFF) << 8) Or (buf(off + 1) And &HFF)
                            If (Not Me.glyphIds.Contains(ogid)) Then
                                LOG.debug("Adding referenced glyph " & ogid & " of compound glyph " & glyphId)
                                If (glyphIdsToAdd Is Nothing) Then
                                    glyphIdsToAdd = New TreeSet(Of NInteger)()
                                End If
                                glyphIdsToAdd.add(ogid)
                            End If
                            off += 2
                            ' ARG_1_AND_2_ARE_WORDS
                            If ((flags And (1 << 0)) = (1 << 0)) Then
                                off += 2 * 2
                            Else
                                off += 2
                            End If
                            ' WE_HAVE_A_TWO_BY_TWO
                            If ((flags And (1 << 7)) = (1 << 7)) Then
                                off += 2 * 4
                            ElseIf ((flags And (1 << 6)) = (1 << 6)) Then '' WE_HAVE_AN_X_AND_Y_SCALE
                                off += 2 * 2
                            ElseIf ((flags And (1 << 3)) = (1 << 3)) Then '' WE_HAVE_A_SCALE
                                off += 2
                            End If

                            ' MORE_COMPONENTS
                        Loop While ((flags And (1 << 5)) = (1 << 5))

                    End If
                    lastOff = offsets(glyphId.Value + 1)
                Next
            Finally
                [is].Close()
            End Try
            If (glyphIdsToAdd IsNot Nothing) Then
                Me.glyphIds.addAll(glyphIdsToAdd)
            End If
            Return glyphIdsToAdd Is Nothing
        End Function

        Private Function buildGlyfTable(ByVal newOffsets() As Long) As Byte()
            Dim bos As New ByteArrayOutputStream()
            LOG.debug("Building table [glyf]...")
            Dim g As GlyphTable = Me.baseTTF.getGlyph()
            Dim offsets() As Long = Me.baseTTF.getIndexToLocation().getOffsets()
            Dim [is] As InputStream = Me.baseTTF.getOriginalData()
            Try
                [is].skip(g.getOffset())
                Dim lastOff As Long = 0L
                Dim newOff As Long = 0L
                Dim ioff As Integer = 0
                For Each glyphId As NInteger In Me.glyphIds
                    Dim offset As Long = offsets(glyphId.Value)
                    Dim len As Long = offsets(glyphId.Value + 1) - offset
                    newOffsets(ioff) = newOff : ioff += 1
                    [is].skip(offset - lastOff)
                    Dim buf As Byte() = Arrays.CreateInstance(Of Byte)(len)
                    [is].read(buf)
                    ' rewrite glyphIds for compound glyphs
                    If (buf.Length >= 2 AndAlso buf(0) = -1 AndAlso buf(1) = -1) Then
                        LOG.debug("Compound glyph " + glyphId)
                        Dim off As Integer = 2 * 5
                        Dim flags As Integer = 0
                        Do
                            ' clear the WE_HAVE_INSTRUCTIONS bit. (bit 8 is lsb of the first byte)
                            buf(off) = buf(off) And &HFE
                            flags = ((CInt(buf(off)) And &HFF) << 8) Or (buf(off + 1) And &HFF)
                            off += 2
                            Dim ogid As Integer = ((CInt(buf(off)) And &HFF) << 8) Or (buf(off + 1) And &HFF)
                            If (Not Me.glyphIds.Contains(ogid)) Then
                                Me.glyphIds.add(ogid)
                            End If
                            Dim ngid As Integer = Me.getNewGlyphId(ogid)
                            If (LOG.isDebugEnabled()) Then
                                LOG.debug(String.Format("mapped glyph  %d to %d in compound reference (flags=%04x)", ogid, ngid, flags))
                            End If
                            buf(off) = (ngid >> 8) '>>>
                            buf(off + 1) = ngid
                            off += 2
                            ' ARG_1_AND_2_ARE_WORDS
                            If ((flags And (1 << 0)) = (1 << 0)) Then
                                off += 2 * 2
                            Else
                                off += 2
                            End If
                            ' WE_HAVE_A_TWO_BY_TWO
                            If ((flags And (1 << 7)) = (1 << 7)) Then
                                off += 2 * 4
                            ElseIf ((flags And (1 << 6)) = (1 << 6)) Then  '// WE_HAVE_AN_X_AND_Y_SCALE
                                off += 2 * 2
                            ElseIf ((flags And (1 << 3)) = (1 << 3)) Then  'WE_HAVE_A_SCALE
                                off += 2
                            End If
                            ' MORE_COMPONENTS
                        Loop While ((flags And (1 << 5)) = (1 << 5))
                        ' write the compound glyph
                        bos.Write(buf, 0, off)
                        newOff += off
                    ElseIf (buf.Length > 0) Then
                        '/*
                        ' * bail out instructions for simple glyphs, an excerpt from the specs is given below:
                        ' *                         
                        ' * int16    numberOfContours    If the number of contours is positive or zero, it is a single glyph;
                        ' * If the number of contours is -1, the glyph is compound
                        ' *  FWord    xMin    Minimum x for coordinate data
                        ' *  FWord    yMin    Minimum y for coordinate data
                        ' *  FWord    xMax    Maximum x for coordinate data
                        ' *  FWord    yMax    Maximum y for coordinate data
                        ' *  (here follow the data for the simple or compound glyph)
                        ' *
                        ' * Table 15: Simple glyph definition
                        ' *  Type    Name    Description
                        ' *  uint16  endPtsOfContours[n] Array of last points of each contour; n is the number of contours;
                        ' *          array entries are point indices
                        ' *  uint16  instructionLength Total number of bytes needed for instructions
                        ' *  uint8   instructions[instructionLength] Array of instructions for Me glyph
                        ' *  uint8   flags[variable] Array of flags
                        ' *  uint8 or int16  xCoordinates[] Array of x-coordinates; the first is relative to (0,0),
                        ' *                                 others are relative to previous point
                        ' *  uint8 or int16  yCoordinates[] Array of y-coordinates; the first is relative to (0,0), 
                        ' *                                 others are relative to previous point
                        ' */

                        Dim numberOfContours As Integer = (CInt(buf(0) And &HFF) << 8) Or (buf(1) And &HFF)

                        ' offset of instructionLength
                        Dim off As Integer = 2 * 5 + numberOfContours * 2

                        ' write numberOfContours, xMin, yMin, xMax, yMax, endPtsOfContours[n]
                        bos.Write(buf, 0, off)
                        newOff += off

                        Dim instructionLength As Integer = ((CInt(buf(off)) And &HFF) << 8) Or (buf(off + 1) And &HFF)

                        ' zarro instructions.
                        bos.Write(0)
                        bos.Write(0)
                        newOff += 2

                        off += 2 + instructionLength

                        ' flags and coordinates
                        bos.Write(buf, off, buf.Length - off)
                        newOff += buf.Length - off
                    End If


                    If ((newOff Mod 4) <> 0) Then
                        Dim np As Integer = (4 - newOff Mod 4)
                        bos.Write(PAD_BUF, 0, np)
                        newOff += np
                    End If

                    lastOff = offsets(glyphId.Value + 1)
                Next
                newOffsets(ioff) = newOff : ioff += 1
            Finally
                [is].Close()
            End Try
            LOG.debug("Finished table [glyf].")
            Dim ret() As Byte = bos.toByteArray()
            bos.Dispose()
            Return ret
        End Function

        Private Function getNewGlyphId(ByVal oldGid As NInteger) As Integer
            Return Me.glyphIds.headSet(oldGid).size()
        End Function

        Private Function buildCmapTable() As Byte()
            Dim bos As New ByteArrayOutputStream()
            Dim dos As New DataOutputStream(bos)
            LOG.debug("Building table [cmap]...")
            '/*
            ' * UInt16    version    Version number (Set to zero)
            ' * UInt16    numberSubtables    Number of encoding subtables
            ' */
            writeUint16(dos, 0)
            writeUint16(dos, 1)
            '/*
            ' * UInt16    platformID    Platform identifier
            ' * UInt16    platformSpecificID    Platform-specific encoding identifier
            ' * UInt32    offset    Offset of the mapping table
            ' */
            writeUint16(dos, 3) ' unicode
            writeUint16(dos, 1) ' Default Semantics
            writeUint32(dos, 4 * 2 + 4)
            ' mapping of type 4.
            Dim it As Iterator(Of Map.Entry(Of NInteger, NInteger)) = Me.characters.entrySet().iterator()
            it.next()
            Dim lastChar As Map.Entry(Of NInteger, NInteger) = it.next()
            Dim prevChar As Map.Entry(Of NInteger, NInteger) = lastChar
            Dim lastGid As Integer = Me.getNewGlyphId(lastChar.Value)

            Dim startCode() As Integer = Arrays.CreateInstance(Of Integer)(Me.characters.size())
            Dim endCode() As Integer = Arrays.CreateInstance(Of Integer)(Me.characters.size())
            Dim idDelta() As Integer = Arrays.CreateInstance(Of Integer)(Me.characters.size())
            Dim nseg As Integer = 0
            While (it.hasNext())
                Dim curChar As Map.Entry(Of NInteger, NInteger) = it.next()
                Dim curGid As Integer = Me.getNewGlyphId(curChar.Value)

                If (curChar.Key <> prevChar.Key + 1 OrElse curGid - lastGid <> curChar.Key - lastChar.Key) Then
                    ' Don't emit ranges, which map to the undef glyph, the
                    ' undef glyph is emitted a the very last segment.
                    If (lastGid <> 0) Then
                        startCode(nseg) = lastChar.Key
                        endCode(nseg) = prevChar.Key
                        idDelta(nseg) = lastGid - lastChar.Key
                        nseg += 1
                    ElseIf (Not lastChar.Key.Equals(prevChar.Key)) Then '// shorten ranges which start with undef by one.
                        startCode(nseg) = lastChar.Key + 1
                        endCode(nseg) = prevChar.Key
                        idDelta(nseg) = lastGid - lastChar.Key
                        nseg += 1
                    End If
                    lastGid = curGid
                    lastChar = curChar
                End If
                prevChar = curChar
            End While
            ' trailing segment
            startCode(nseg) = lastChar.Key
            endCode(nseg) = prevChar.Key
            idDelta(nseg) = lastGid - lastChar.Key
            nseg += 1
            ' notdef character.
            startCode(nseg) = &HFFFF
            endCode(nseg) = &HFFFF
            idDelta(nseg) = 1
            nseg += 1

            '/*
            ' * UInt16    format    Format number is set to 4     
            ' * UInt16    length    Length of subtable in bytes     
            ' * UInt16    language    Language code for Me encoding subtable, or zero if language-independent     
            ' * UInt16    segCountX2    2 * segCount     
            ' * UInt16    searchRange    2 * (2**FLOOR(log2(segCount)))     
            ' * UInt16    entrySelector    log2(searchRange/2)     
            ' * UInt16    rangeShift    (2 * segCount) - searchRange     
            ' * UInt16    endCode[segCount]    Ending character code for each segment, last = 0xFFFF.    
            ' * UInt16    reservedPad    This value should be zero    
            ' * UInt16    startCode[segCount]    Starting character code for each segment    
            ' * UInt16    idDelta[segCount]    Delta for all character codes in segment     
            ' * UInt16    idRangeOffset[segCount]    Offset in bytes to glyph indexArray, or 0     
            ' * UInt16    glyphIndexArray[variable]    Glyph index array
            ' */

            writeUint16(dos, 4)
            writeUint16(dos, 8 * 2 + nseg * (4 * 2))
            writeUint16(dos, 0)
            writeUint16(dos, nseg * 2)
            Dim nsegHigh As Integer = NInteger.highestOneBit(nseg)
            writeUint16(dos, nsegHigh * 2)
            writeUint16(dos, log2i(nsegHigh))
            writeUint16(dos, 2 * (nseg - nsegHigh))

            For i As Integer = 0 To nseg - 1
                writeUint16(dos, endCode(i))
            Next
            writeUint16(dos, 0)
            For i As Integer = 0 To nseg - 1
                writeUint16(dos, startCode(i))
            Next
            For i As Integer = 0 To nseg - 1
                writeUint16(dos, idDelta(i))
            Next
            For i As Integer = 0 To nseg - 1
                writeUint16(dos, 0)
            Next
            LOG.debug("Finished table [cmap].")
            Dim ret() As Byte = bos.toByteArray()
            bos.Dispose()
            Return ret
        End Function

        Private Function buildPostTable() As Byte()
            Dim bos As New ByteArrayOutputStream()
            Dim dos As New DataOutputStream(bos)
            LOG.debug("Building table [post]...")
            Dim p As PostScriptTable = Me.baseTTF.getPostScript()
            If (p Is Nothing) Then
                ' sometimes there is no post table in an embedded subfonts
                ' create a dummy
                p = New PostScriptTable()
            End If
            Dim glyphNames() As String = p.getGlyphNames()
            '/*
            '    Fixed    format    Format of Me table
            '    Fixed    italicAngle    Italic angle in degrees
            '    FWord    underlinePosition    Underline position
            '    FWord    underlineThickness    Underline thickness
            '    uint32    isFixedPitch    Font is monospaced; set to 1 if the font is monospaced and 0 otherwise 
            '    (N.B., to maintain compatibility with older versions of the TrueType spec, accept any non-zero value
            '     as meaning that the font is monospaced)
            '    uint32    minMemType42    Minimum memory usage when a TrueType font is downloaded as a Type 42 font
            '    uint32    maxMemType42    Maximum memory usage when a TrueType font is downloaded as a Type 42 font
            '    uint32    minMemType1    Minimum memory usage when a TrueType font is downloaded as a Type 1 font
            '    uint32    maxMemType1    Maximum memory usage when a TrueType font is downloaded as a Type 1 font
            '    uint16    numberOfGlyphs    number of glyphs
            '    uint16    glyphNameIndex[numberOfGlyphs]    Ordinal number of Me glyph in 'post' string tables. 
            '    This is not an offset.
            '    Pascal string    names[numberNewGlyphs]  glyph names with length bytes [variable] (a Pascal string)
            ' */
            writeFixed(dos, 2.0)
            writeFixed(dos, p.getItalicAngle())
            writeSint16(dos, p.getUnderlinePosition())
            writeSint16(dos, p.getUnderlineThickness())
            writeUint32(dos, p.getIsFixedPitch())
            writeUint32(dos, p.getMinMemType42())
            writeUint32(dos, p.getMaxMemType42())
            writeUint32(dos, p.getMimMemType1())
            writeUint32(dos, p.getMaxMemType1())
            writeUint16(dos, baseTTF.getHorizontalHeader().getNumberOfHMetrics())

            Dim additionalNames As List(Of String) = New ArrayList(Of String)()
            Dim additionalNamesIndices As Map(Of String, NInteger) = New HashMap(Of String, NInteger)()

            If (glyphNames Is Nothing) Then
                Dim enc As encoding.Encoding = MacRomanEncoding.INSTANCE
                Dim gidToUC() As Integer = Me.baseCmap.getGlyphIdToCharacterCode()
                For Each glyphId As NInteger In Me.glyphIds
                    Dim uc As Integer = gidToUC(glyphId.Value)
                    Dim name As String = vbNullString
                    If (uc < &H8000) Then
                        Try
                            name = enc.getNameFromCharacter(Convert.ToChar(uc))
                        Catch e As IOException
                            ' TODO
                        End Try
                    End If
                    If (name Is Nothing) Then
                        name = String.Format(Locale.ENGLISH, "uni%04X", uc)
                    End If
                    Dim macId As NInteger = encoding.Encoding.MAC_GLYPH_NAMES_INDICES.get(name)
                    If (macId Is Nothing) Then
                        Dim idx As NInteger = additionalNamesIndices.get(name)
                        If (idx Is Nothing) Then
                            idx = additionalNames.size()
                            additionalNames.add(name)
                            additionalNamesIndices.put(name, idx)
                        End If
                        writeUint16(dos, idx.Value + 258)
                    Else
                        writeUint16(dos, macId.Value)
                    End If
                Next
            Else
                For Each glyphId As NInteger In Me.glyphIds
                    Dim name As String = glyphNames(glyphId.Value)
                    Dim macId As NInteger = encoding.Encoding.MAC_GLYPH_NAMES_INDICES.get(name)
                    If (macId Is Nothing) Then
                        Dim idx As NInteger = additionalNamesIndices.get(name)
                        If (idx Is Nothing) Then
                            idx = additionalNames.size()
                            additionalNames.add(name)
                            additionalNamesIndices.put(name, idx)
                        End If
                        writeUint16(dos, idx.Value + 258)
                    Else
                        writeUint16(dos, macId.Value)
                    End If
                Next
            End If

            For Each name As String In additionalNames
                LOG.debug("additionalName=[" & name & "].")
                Dim buf() As Byte = Strings.GetBytes(name, "US-ASCII")
                writeUint8(dos, buf.Length)
                dos.write(buf)
            Next
            dos.flush()
            LOG.debug("Finished table [post].")
            Dim ret() As Byte = bos.toByteArray()
            bos.Dispose()
            Return ret
        End Function

        Private Function buildHmtxTable() As Byte()
            Dim bos As New ByteArrayOutputStream()
            LOG.debug("Building table [hmtx]...")
            Dim h As HorizontalHeaderTable = Me.baseTTF.getHorizontalHeader()
            Dim hm As HorizontalMetricsTable = Me.baseTTF.getHorizontalMetrics()
            Dim buf() As Byte = Arrays.CreateInstance(Of Byte)(4)
            Dim [is] As InputStream = Me.baseTTF.getOriginalData()
            Try
                [is].skip(hm.getOffset())
                Dim lastOff As Long = 0
                For Each glyphId As NInteger In Me.glyphIds
                    ' offset in original file.
                    Dim off As Long
                    If (glyphId < h.getNumberOfHMetrics()) Then
                        off = glyphId * 4
                    Else
                        off = h.getNumberOfHMetrics() * 4 + (glyphId - h.getNumberOfHMetrics()) * 2
                    End If
                    ' skip over from last original offset.
                    If (off <> lastOff) Then
                        Dim nskip As Long = off - lastOff
                        If (nskip <> [is].skip(nskip)) Then
                            Throw New EndOfStreamException("Unexpected EOF exception parsing glyphId of hmtx table.")
                        End If
                    End If
                    ' read left side bearings only, if we are beyond numOfHMetrics.
                    Dim n As Integer = IIf(glyphId < h.getNumberOfHMetrics(), 4, 2)
                    If (n <> [is].read(buf, 0, n)) Then
                        Throw New EndOfStreamException("Unexpected EOF exception parsing glyphId of hmtx table.")
                    End If
                    bos.Write(buf, 0, n)
                    lastOff = off + n
                Next
                LOG.debug("Finished table [hmtx].")
                Dim ret() As Byte = bos.toByteArray()
                bos.Dispose()
                Return ret
            Finally
                [is].Close()
            End Try
        End Function

        '/**
        ' * Write the subfont to the given output stream.
        ' * 
        ' * @param os the stream used for writing
        ' * @throws IOException if something went wrong.
        ' */
        Public Sub writeToStream(ByVal os As OutputStream)
            LOG.debug("glyphIds=[" & glyphIds.ToString & "]")
            LOG.debug("numGlyphs=[" & glyphIds.size() & "]")
            While (Not addCompoundReferences())
            End While
            Dim dos As New DataOutputStream(os)
            Try
                '/*
                '    'cmap'    character to glyph mapping
                '    'glyf'    glyph data
                '    'head'    font header
                '    'hhea'    horizontal header
                '    'OS/2'  OS/2 compatibility table.
                '    'hmtx'    horizontal metrics
                '    'loca'    index to location
                '    'maxp'    maximum profile
                '    'name'    naming
                '    'post'    PostScript
                ' */
                Dim tableNames() As String = {"OS/2", "cmap", "glyf", "head", "hhea", "hmtx", "loca", "maxp", "name", "post"}
                Dim tables As Byte()() = Arrays.CreateInstance(Of Byte())(tableNames.Length)
                Dim newOffsets As Long() = Arrays.CreateInstance(Of Long)(Me.glyphIds.size() + 1)
                tables(3) = Me.buildHeadTable()
                tables(4) = Me.buildHheaTable()
                tables(7) = Me.buildMaxpTable()
                tables(8) = Me.buildNameTable()
                tables(0) = Me.buildOS2Table()
                tables(2) = Me.buildGlyfTable(newOffsets)
                tables(6) = Me.buildLocaTable(newOffsets)
                tables(1) = Me.buildCmapTable()
                tables(5) = Me.buildHmtxTable()
                tables(9) = Me.buildPostTable()
                Dim checksum As Long = writeFileHeader(dos, tableNames.Length)
                Dim offset As Long = 12L + 16L * tableNames.Length
                For i As Integer = 0 To tableNames.Length - 1
                    checksum += writeTableHeader(dos, tableNames(i), offset, tables(i))
                    offset += ((tables(i).Length + 3) / 4) * 4
                Next
                checksum = &HB1B0AFBA - (checksum And &HFFFFFFFF)
                ' correct checksumAdjustment of 'head' table.
                tables(3)(8) = (checksum >> 24) '>>>
                tables(3)(9) = (checksum >> 16) '>>>
                tables(3)(10) = (checksum >> 8) '>>>
                tables(3)(11) = (checksum)
                For i As Integer = 0 To tableNames.Length - 1
                    writeTableBody(dos, tables(i))
                Next
            Finally
                dos.close()
            End Try
        End Sub

    End Class

End Namespace
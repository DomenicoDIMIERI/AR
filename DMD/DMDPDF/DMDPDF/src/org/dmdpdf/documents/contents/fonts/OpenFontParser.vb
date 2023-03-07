'/*
'  Copyright 2009-2012 Stefano Chizzolini. http://www.dmdpdf.org

'  Contributors:
'    * Stefano Chizzolini (original code developer, http://www.stefanochizzolini.it)

'  This file should be part of the source code distribution of "PDF Clown library" (the
'  Program): see the accompanying README files for more info.

'  This Program is free software; you can redistribute it and/or modify it under the terms
'  of the GNU Lesser General Public License as published by the Free Software Foundation;
'  either version 3 of the License, or (at your option) any later version.

'  This Program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY,
'  either expressed or implied; without even the implied warranty of MERCHANTABILITY or
'  FITNESS FOR A PARTICULAR PURPOSE. See the License for more details.

'  You should have received a copy of the GNU Lesser General Public License along with Me
'  Program (see README files); if not, go to the GNU website (http://www.gnu.org/licenses/).

'  Redistribution and use, with or without modification, are permitted provided that such
'  redistributions retain the above copyright notice, license and disclaimer, along with
'  Me list of conditions.
'*/

Imports DMD.org.dmdpdf.bytes
Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects
Imports DMD.org.dmdpdf.tokens
Imports DMD.org.dmdpdf.util.io
Imports DMD.org.dmdpdf.util.parsers

Imports System
Imports System.IO
Imports System.Collections.Generic
Imports System.Text

Namespace DMD.org.dmdpdf.documents.contents.fonts

    '/**
    '  <summary>Open Font Format parser [OFF:2009].</summary>
    '*/
    Friend NotInheritable Class OpenFontParser

#Region "types"
        '/**
        '  <summary>Font metrics.</summary>
        '*/
        Public NotInheritable Class FontMetrics

            '      /**
            '  <summary>Whether the encoding is custom (symbolic font).</summary>
            '*/
            Public IsCustomEncoding As Boolean 'TODO:verify whether it can be replaced by the 'symbolic' variable!!!
            '/**
            '  <summary>Unit normalization coefficient.</summary>
            '*/
            Public UnitNorm As Single
            '/*
            '  Font Header ('head' table).
            '*/
            Public Flags As Integer ' USHORT.
            Public UnitsPerEm As Integer ' USHORT.
            Public XMin As Short
            Public YMin As Short
            Public XMax As Short
            Public YMax As Short
            Public MacStyle As Integer ' USHORT.
            '/*
            '  Horizontal Header ('hhea' table).
            '*/
            Public Ascender As Short
            Public Descender As Short
            Public LineGap As Short
            Public AdvanceWidthMax As Integer 'UFWORD.
            Public MinLeftSideBearing As Short
            Public MinRightSideBearing As Short
            Public XMaxExtent As Short
            Public CaretSlopeRise As Short
            Public CaretSlopeRun As Short
            Public NumberOfHMetrics As Integer ' USHORT.
            '/*
            '  OS/2 table ('OS/2' table).
            '*/
            Public STypoAscender As Short
            Public STypoDescender As Short
            Public STypoLineGap As Short
            Public SxHeight As Short
            Public SCapHeight As Short
            '/*
            '  PostScript table ('post' table).
            '*/
            Public ItalicAngle As Single
            Public UnderlinePosition As Short
            Public UnderlineThickness As Short
            Public IsFixedPitch As Boolean
        End Class

        '/**
        '  <summary>Outline format.</summary>
        '*/
        Public Enum OutlineFormatEnum
            '/**
            '  <summary>TrueType format outlines.</summary>
            '*/
            TrueType
            '/**
            '  <summary>Compact Font Format outlines.</summary>
            '*/
            CFF
        End Enum

#End Region

#Region "Static"
#Region "fields"

        Private Const MicrosoftLanguage_UsEnglish As Integer = &H409
        Private Const NameID_FontPostscriptName As Integer = 6

        Private Const PlatformID_Unicode As Integer = 0
        Private Const PlatformID_Macintosh As Integer = 1
        Private Const PlatformID_Microsoft As Integer = 3

#End Region

#Region "interface"
#Region "public"
        '/**
        '  <summary> Gets whether the given data represents a valid Open Font.</summary>
        '*/
        Public Shared Function IsOpenFont(ByVal fontData As bytes.IInputStream) As Boolean
            Dim position As Long = fontData.Position
            fontData.Position = 0
            Try
                GetOutlineFormat(fontData.ReadInt())
            Catch e As NotSupportedException
                Return False
            Finally
                fontData.Position = position
            End Try
            Return True
        End Function

#End Region

#Region "private"
        '/**
        '  <summary> Gets the outline format corresponding To the specified version code.</summary>
        '  <param name = "versionCode" > OFF version.</param>
        '  <Exception cref = "NotSupportedException" > If versionCode Is unknown.</exception>
        '*/
        Private Shared Function GetOutlineFormat(ByVal versionCode As Integer) As OutlineFormatEnum
            ' Which font file format ('sfnt') version?
            Select Case (versionCode)
                Case &H10000, &H74727565
                    ' TrueType (standard/Windows).
                    ' TrueType (legacy/Apple).
                    Return OutlineFormatEnum.TrueType
                Case &H4F54544F
                    ' CFF (Type 1).
                    Return OutlineFormatEnum.CFF
                Case Else
                    Throw New NotSupportedException("Unknown OpenFont format version.")
            End Select
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "fields"

        Public Metrics As FontMetrics

        Public FontName As String
        Public OutlineFormat As OutlineFormatEnum

        '/**
        '  <summary> Whether glyphs are indexed by custom (non-Unicode) encoding.</summary>
        '*/
        Public Symbolic As Boolean

        Public GlyphIndexes As Dictionary(Of Integer, Integer)
        Public GlyphKernings As Dictionary(Of Integer, Integer)
        Public GlyphWidths As Dictionary(Of Integer, Integer)

        Public FontData As bytes.IInputStream

        Private _tableOffsets As Dictionary(Of String, Integer)

#End Region

#Region "constructors"

        Friend Sub New(ByVal fontData As bytes.IInputStream)
            Me.FontData = fontData
            Me.FontData.ByteOrder = ByteOrderEnum.BigEndian ' Ensures that proper endianness Is applied.
            Me.Load()
        End Sub

#End Region

#Region "interface"
#Region "private"
        '/**
        '  <summary> Loads the font data.</summary>
        '*/
        Private Sub Load()
            LoadTableInfo()
            Me.FontName = GetName(NameID_FontPostscriptName)
            Me.Metrics = New FontMetrics()
            LoadTables()
            LoadCMap()
            LoadGlyphWidths()
            LoadGlyphKerning()
        End Sub

        '/**
        '  <summary> Loads the character To glyph index mapping table.</summary>
        '*/
        Private Sub LoadCMap()
            '/*
            '  NOTE: A 'cmap' table may contain one or more subtables that represent multiple encodings
            '  intended for use on different platforms (such as Mac OS And Windows).
            '  Each subtable Is identified by the two numbers, such as (3,1), that represent a combination
            '  of a platform ID And a platform-specific encoding ID, respectively.
            '  A symbolic font (used To display glyphs that Do Not use standard encodings, i.e. neither
            '  MacRomanEncoding nor WinAnsiEncoding) program's "cmap" table should contain a (1,0) subtable.
            '  It may also contain a (3,0) subtable; If present, Me subtable should map from character
            '  codes in the range 0xF000 to 0xF0FF by prepending the single-byte codes in the (1,0) subtable
            '  With 0xF0 And mapping To the corresponding glyph descriptions.
            '*/
            '// Character To Glyph Index Mapping Table ('cmap' table).
            '// Retrieve the location info!
            Dim tableOffset As Integer = 0
            If (Not _tableOffsets.TryGetValue("cmap", tableOffset)) Then
                Throw New ParseException("'cmap' table does NOT exist.")
            End If

            Dim cmap10Offset As Integer = 0
            Dim cmap31Offset As Integer = 0
            '// Header.
            '// Go to the number of tables!
            Me.FontData.Seek(tableOffset + 2)
            Dim tableCount As Integer = Me.FontData.ReadUnsignedShort()

            ' Encoding records.
            For tableIndex As Integer = 0 To tableCount - 1
                ' Platform ID.
                Dim platformID As Integer = FontData.ReadUnsignedShort()
                ' Encoding ID.
                Dim encodingID As Integer = FontData.ReadUnsignedShort()
                ' Subtable offset.
                Dim offset As Integer = FontData.ReadInt()
                Select Case (platformID)
                    Case PlatformID_Macintosh
                        Select Case (encodingID)
                            Case 0  ' Symbolic font.
                                cmap10Offset = offset
                                'break;
                        End Select
                        'break;
                    Case PlatformID_Microsoft
                        Select Case (encodingID)
                            Case 0 ' Symbolic font.
                                'break;
                            Case 1 ' Nonsymbolic font.
                                cmap31Offset = offset
                                'break;
                        End Select
                        'break;
                End Select
            Next

            '/*
            '  NOTE: Symbolic fonts use specific (non-standard, i.e. neither Unicode nor
            '  platform-standard) font encodings.
            '*/
            If (cmap31Offset > 0) Then ' Nonsymbolic.
                Metrics.IsCustomEncoding = False
                ' Go to the beginning of the subtable!
                FontData.Seek(tableOffset + cmap31Offset)
            ElseIf (cmap10Offset > 0) Then ' Symbolic.
                Metrics.IsCustomEncoding = True
                ' Go to the beginning of the subtable!
                FontData.Seek(tableOffset + cmap10Offset)
            Else
                Throw New ParseException("CMAP table unavailable.")
            End If

            Dim format As Integer
            format = FontData.ReadUnsignedShort()
            ' Which cmap table format?
            Select Case (format)
                Case 0    ' Byte encoding table.
                    LoadCMapFormat0()
                    'break;
                Case 4    ' Segment mapping to delta values.
                    LoadCMapFormat4()
                    'break;
                Case 6    ' Trimmed table mapping.
                    LoadCMapFormat6()
                    'break;
                Case Else
                    Throw New ParseException("Cmap table format " & format & " NOT supported.")
            End Select
        End Sub

        '/**
        '  <summary> Loads format-0 cmap subtable (Byte encoding table, i.e. Apple standard
        '  character-to-glyph index mapping table).</summary>
        '*/
        Private Sub LoadCMapFormat0()
            '/*
            '  NOTE: This Is a simple 1-to-1 mapping of character codes to glyph indices.
            '  The glyph collection Is limited To 256 entries.
            '*/
            Symbolic = True
            GlyphIndexes = New Dictionary(Of Integer, Integer)(256)

            ' Skip to the mapping array!
            FontData.Skip(4)
            '// Glyph index array.
            '// Iterating through the glyph indexes...
            For code As Integer = 0 To 256 - 1
                'code : // Character code.
                GlyphIndexes(code) = FontData.ReadByte() ' Glyph index.
            Next
        End Sub

        '/**
        '  <summary> Loads format-4 cmap subtable (Segment mapping To delta values, i.e. Microsoft standard
        '  character to glyph index mapping table for fonts that support Unicode ranges other than the
        '  range [U+D800 - U+DFFF] (defined as Surrogates Area, in Unicode v 3.0)).</summary>
        '*/
        Private Sub LoadCMapFormat4()
            '      /*
            '        NOTE: This format Is used When the character codes For the characters represented by a font
            '        fall into several contiguous ranges, possibly With holes In some Or all Of the ranges (i.e.
            '        some of the codes in a range may Not have a representation in the font).
            '        The format-dependent data Is divided into three parts, which must occur In the following
            '        order:
            '1. A header gives parameters for an optimized search of the segment list;
            '          2. Four parallel arrays (end characters, start characters, deltas And range offsets)
            '          describe the segments (one segment For Each contiguous range Of codes);
            '          3. A variable-length array of glyph IDs.
            '      */
            Symbolic = False

            ' 1. Header.
            ' Get the table length!
            Dim tableLength As Integer = FontData.ReadUnsignedShort() ' UShort.

            ' Skip to the segment count!
            FontData.Skip(2)
            ' Get the segment count!
            Dim segmentCount As Integer = CInt(FontData.ReadUnsignedShort() / 2)

            ' 2. Arrays describing the segments.
            ' Skip to the array of end character code for each segment!
            FontData.Skip(6)
            ' End character code for each segment.
            Dim endCodes As Integer() = New Integer(segmentCount - 1) {} ' UShort.
            For index As Integer = 0 To segmentCount - 1
                endCodes(index) = FontData.ReadUnsignedShort()
            Next

            ' Skip to the array of start character code for each segment!
            FontData.Skip(2)
            ' Start character code for each segment.
            Dim startCodes As Integer() = New Integer(segmentCount - 1) {} ' UShort.
            For index As Integer = 0 To segmentCount - 1
                startCodes(index) = FontData.ReadUnsignedShort()
            Next

            ' Delta for all character codes in segment.
            Dim deltas As Short() = New Short(segmentCount - 1) {}
            For index As Integer = 0 To segmentCount - 1
                deltas(index) = FontData.ReadShort()
            Next

            ' Offsets into glyph index array.
            Dim rangeOffsets As Integer() = New Integer(segmentCount - 1) {} ' UShort.
            For index As Integer = 0 To segmentCount - 1
                rangeOffsets(index) = FontData.ReadUnsignedShort()
            Next

            '// 3. Glyph ID array.
            '/*
            '  NOTE: There's no explicit field defining the array length;
            '  it must be inferred from the space left by the known fields.
            '*/
            '    Dim glyphIndexCount As Integer = CInt(tableLength / 2) ' Number Of 16-bit words inside the table.
            '- 8 // Number of single-word header fields (8 fields: format, length, language, segCountX2, searchRange, entrySelector, rangeShift, reservedPad).
            '- segmentCount * 4; // Number of single-word items in the arrays describing the segments (4 arrays of segmentCount items).
            Dim glyphIndexCount As Integer = CInt(tableLength / 2) -
                                             -8 -
                                             -segmentCount * 4
            Dim glyphIds As Integer() = New Integer(glyphIndexCount - 1) {} ' UShort.
            For index As Integer = 0 To glyphIds.Length - 1
                glyphIds(index) = FontData.ReadUnsignedShort()
            Next


            GlyphIndexes = New Dictionary(Of Integer, Integer)(glyphIndexCount)
            ' Iterating through the segments...
            For segmentIndex As Integer = 0 To segmentCount - 1
                Dim endCode As Integer = endCodes(segmentIndex)
                '// Is it Not the last end character code?
                '/*
                '  NOTE: The final segment's endCode MUST be 0xFFFF. This segment need not (but MAY)
                '  contain any valid mappings (it can just map the Single character code 0xFFFF To
                '  missing glyph). However, the segment MUST be present.
                '*/
                If (endCode < &HFFFF) Then
                    endCode += 1
                End If
                ' Iterating inside the current segment...
                For code As Integer = startCodes(segmentIndex) To endCode - 1
                    Dim glyphIndex As Integer
                    ' Doesn't the mapping of character codes rely on glyph ID?
                    If (rangeOffsets(segmentIndex) = 0) Then ' No glyph-ID reliance.
                        '/*
                        '  NOTE: If the range offset Is 0, the delta value Is added directly To the character
                        '  code to get the corresponding glyph index. The delta arithmetic Is modulo 65536.
                        '*/
                        glyphIndex = (code + deltas(segmentIndex)) And &HFFFF
                    Else ' Glyph-ID reliance.
                        '/*
                        '  NOTE: If the range offset Is Not 0, the mapping Of character codes relies On glyph ID.
                        '  The character code offset from start code Is added To the range offset. This sum Is
                        '  used as an offset from the current location within range offset itself to index out
                        '  the correct glyph ID. This obscure indexing trick (sic!) works because glyph ID
                        '  immediately follows range offset In the font file. The C expression that yields the
                        '  address to the glyph ID Is:
                        '    *(rangeOffsets[segmentIndex]/2
                        '    + (code - startCodes[segmentIndex])
                        '    + &idRangeOffset[segmentIndex])
                        '  As safe C# semantics don't deal directly with pointers, we have to further
                        '  exploit such a trick reasoning With 16-bit displacements In order To yield an index
                        '  instead of an address (sooo-good!).
                        '*/
                        '// Retrieve the glyph index!
                        '          Dim glyphIdIndex As Integer = CInt(rangeOffsets(segmentIndex) / 2) ' 16-bit word range offset.
                        '+ (code - startCodes[segmentIndex]) // Character code offset from start code.
                        '- (segmentCount - segmentIndex); // Physical offset between the offsets into glyph index array And the glyph index array.
                        Dim glyphIdIndex As Integer = CInt(rangeOffsets(segmentIndex) / 2) +
                                                    (code - startCodes(segmentIndex)) -
                                                    (segmentCount - segmentIndex)


                        '/*
                        '  NOTE: The delta value Is added To the glyph ID To Get the corresponding glyph index.
                        '  The delta arithmetic Is modulo 65536.
                        '*/
                        glyphIndex = (glyphIds(glyphIdIndex) + deltas(segmentIndex)) And &HFFFF
                    End If

                    'GlyphIndexes[ code // Character code. ] = glyphIndex; // Glyph index.
                    GlyphIndexes(code) = glyphIndex
                Next
            Next
        End Sub

        '/**
        '  <summary> Loads format-6 cmap subtable (Trimmed table mapping).</summary>
        '*/
        Private Sub LoadCMapFormat6()
            ' Skip to the first character code!
            FontData.Skip(4)
            Dim firstCode As Integer = FontData.ReadUnsignedShort()
            Dim codeCount As Integer = FontData.ReadUnsignedShort()
            GlyphIndexes = New Dictionary(Of Integer, Integer)(codeCount)
            Dim lastCode As Integer = firstCode + codeCount
            For code As Integer = firstCode To lastCode - 1
                'GlyphIndexes[          code // Character code.          ] = FontData.ReadUnsignedShort(); // Glyph index.
                GlyphIndexes(code) = FontData.ReadUnsignedShort()
            Next
        End Sub

        '/**
        '  <summary> Gets a name.</summary>
        '  <param name = "id" > name identifier.</param>
        '*/
        Private Function GetName(ByVal id As Integer) As String
            '// Naming Table ('name' table).
            '// Retrieve the location info!
            Dim tableOffset As Integer = 0
            If (Not _tableOffsets.TryGetValue("name", tableOffset)) Then
                Throw New ParseException("'name' table does NOT exist.")
            End If

            ' Go to the number of name records!
            FontData.Seek(tableOffset + 2)

            Dim recordCount As Integer = FontData.ReadUnsignedShort() ' UShort.
            Dim storageOffset As Integer = FontData.ReadUnsignedShort() ' UShort.
            ' Iterating through the name records...
            For recordIndex As Integer = 0 To recordCount - 1
                Dim platformID As Integer = FontData.ReadUnsignedShort() ' UShort.
                ' Is it the default platform?
                If (platformID = PlatformID_Microsoft) Then
                    FontData.Skip(2)
                    Dim languageID As Integer = FontData.ReadUnsignedShort() ' UShort.
                    ' Is it the default language?
                    If (languageID = MicrosoftLanguage_UsEnglish) Then
                        Dim nameID As Integer = FontData.ReadUnsignedShort() ' UShort.
                        ' Does the name ID equal the searched one?
                        If (nameID = id) Then
                            Dim length As Integer = FontData.ReadUnsignedShort() ' UShort.
                            Dim offset As Integer = FontData.ReadUnsignedShort() ' UShort.

                            ' Go to the name string!
                            FontData.Seek(tableOffset + storageOffset + offset)

                            Return ReadString(length, platformID)
                        Else
                            FontData.Skip(4)
                        End If
                    Else
                        FontData.Skip(6)
                    End If
                Else
                    FontData.Skip(10)
                End If
            Next
            Return Nothing  ' Not found.
        End Function

        '/**
        '  <summary> Loads the glyph kerning.</summary>
        '*/
        Private Sub LoadGlyphKerning()
            ' Kerning ('kern' table).
            ' Retrieve the location info!
            Dim tableOffset As Integer = 0
            If (Not _tableOffsets.TryGetValue("kern", tableOffset)) Then
                Return ' NOTE: Kerning table Is Not mandatory.
            End If

            ' Go to the table count!
            FontData.Seek(tableOffset + 2)
            Dim subtableCount As Integer = FontData.ReadUnsignedShort() ' UShort.

            GlyphKernings = New Dictionary(Of Integer, Integer)()
            Dim subtableOffset As Integer = CInt(FontData.Position)
            ' Iterating through the subtables...
            For subtableIndex As Integer = 0 To subtableCount - 1
                ' Go to the subtable length!
                FontData.Seek(subtableOffset + 2)
                ' Get the subtable length!
                Dim length As Integer = FontData.ReadUnsignedShort() ' UShort.

                ' Get the type of information contained in the subtable!
                Dim coverage As Integer = FontData.ReadUnsignedShort() ' UShort.
                '// Is it a format-0 subtable?
                '/*
                '  NOTE: coverage bits 8-15 (format Of the subtable) MUST be all zeros
                '  (representing format 0).
                '*/
                '//
                If ((coverage And &HFF00) = &H0) Then
                    Dim pairCount As Integer = FontData.ReadUnsignedShort() '  UShort.

                    ' Skip to the beginning of the list!
                    FontData.Skip(6)
                    ' List of kerning pairs And values.
                    For pairIndex As Integer = 0 To pairCount - 1
                        ' Get the glyph index pair (left-hand And right-hand)!
                        Dim pair As Integer = FontData.ReadInt() ' UShort UShort.
                        ' Get the normalized kerning value!
                        Dim value As Integer = CInt(FontData.ReadShort() * Metrics.UnitNorm)
                        GlyphKernings(pair) = value
                    Next
                End If

                subtableOffset += length
            Next
        End Sub

        '/**
        '  <summary> Loads the glyph widths.</summary>
        '*/
        Private Sub LoadGlyphWidths()
            ' Horizontal Metrics ('hmtx' table).
            ' Retrieve the location info!
            Dim tableOffset As Integer = 0
            If (Not _tableOffsets.TryGetValue("hmtx", tableOffset)) Then
                Throw New ParseException("'hmtx' table does NOT exist.")
            End If

            ' Go to the glyph horizontal-metrics entries!
            FontData.Seek(tableOffset)
            GlyphWidths = New Dictionary(Of Integer, Integer)(Metrics.NumberOfHMetrics)
            For index As Integer = 0 To Metrics.NumberOfHMetrics - 1
                ' Get the glyph advance width!
                GlyphWidths(index) = CInt(Math.Floor(FontData.ReadUnsignedShort() * Metrics.UnitNorm))
                ' Skip the left side bearing!
                FontData.Skip(2)
            Next
        End Sub

        Private Sub LoadTableInfo()
            ' 1. Offset Table.
            FontData.Seek(0)
            ' Get the outline format!
            Me.OutlineFormat = GetOutlineFormat(FontData.ReadInt())
            ' Get the number of tables!
            Dim tableCount As Integer = FontData.ReadUnsignedShort()

            ' 2. Table Directory.
            ' Skip to the beginning of the table directory!
            FontData.Skip(6)
            ' Collecting the table offsets...
            _tableOffsets = New Dictionary(Of String, Integer)(tableCount)
            For index As Integer = 0 To tableCount - 1
                ' Get the table tag!
                Dim tag As String = ReadAsciiString(4)
                ' Skip to the table offset!
                FontData.Skip(4)
                ' Get the table offset!
                Dim offset As Integer = FontData.ReadInt()
                ' Collect the table offset!
                _tableOffsets(tag) = offset
                ' Skip to the next entry!
                FontData.Skip(4)
            Next
        End Sub

        '/**
        '  <summary> Loads general tables.</summary>
        '*/
        Private Sub LoadTables()
            ' Font Header ('head' table).
            Dim tableOffset As Integer = 0
            If (Not _tableOffsets.TryGetValue("head", tableOffset)) Then
                Throw New ParseException("'head' table does NOT exist.")
            End If

            ' Go to the font flags!
            FontData.Seek(tableOffset + 16)
            Metrics.Flags = FontData.ReadUnsignedShort()
            Metrics.UnitsPerEm = FontData.ReadUnsignedShort()
            Metrics.UnitNorm = 1000.0F / Metrics.UnitsPerEm
            ' Go to the bounding box limits!
            FontData.Skip(16)
            Metrics.XMin = FontData.ReadShort()
            Metrics.YMin = FontData.ReadShort()
            Metrics.XMax = FontData.ReadShort()
            Metrics.YMax = FontData.ReadShort()
            Metrics.MacStyle = FontData.ReadUnsignedShort()

            ' Font Header ('OS/2' table).
            If (_tableOffsets.TryGetValue("OS/2", tableOffset)) Then
                FontData.Seek(tableOffset)
                Dim version = FontData.ReadUnsignedShort()
                ' Go to the ascender!
                FontData.Skip(66)
                Metrics.STypoAscender = FontData.ReadShort()
                Metrics.STypoDescender = FontData.ReadShort()
                Metrics.STypoLineGap = FontData.ReadShort()
                If (version >= 2) Then
                    FontData.Skip(12)
                    Metrics.SxHeight = FontData.ReadShort()
                    Metrics.SCapHeight = FontData.ReadShort()
                Else
                    '/*
                    '  NOTE: These are just rule-of-thumb values,
                    '  in case the xHeight and CapHeight fields aren't available.
                    '*/
                    Metrics.SxHeight = CShort(0.5 * Metrics.UnitsPerEm)
                    Metrics.SCapHeight = CShort(0.7 * Metrics.UnitsPerEm)
                End If
            End If

            ' Horizontal Header ('hhea' table).
            If (Not _tableOffsets.TryGetValue("hhea", tableOffset)) Then
                Throw New ParseException("'hhea' table does NOT exist.")
            End If

            ' Go to the ascender!
            FontData.Seek(tableOffset + 4)
            Metrics.Ascender = FontData.ReadShort()
            Metrics.Descender = FontData.ReadShort()
            Metrics.LineGap = FontData.ReadShort()
            Metrics.AdvanceWidthMax = FontData.ReadUnsignedShort()
            Metrics.MinLeftSideBearing = FontData.ReadShort()
            Metrics.MinRightSideBearing = FontData.ReadShort()
            Metrics.XMaxExtent = FontData.ReadShort()
            Metrics.CaretSlopeRise = FontData.ReadShort()
            Metrics.CaretSlopeRun = FontData.ReadShort()
            ' Go to the horizontal metrics count!
            FontData.Skip(12)
            Metrics.NumberOfHMetrics = FontData.ReadUnsignedShort()

            ' PostScript ('post' table).
            If (Not _tableOffsets.TryGetValue("post", tableOffset)) Then
                Throw New ParseException("'post' table does NOT exist.")
            End If

            ' Go to the italic angle!
            FontData.Seek(tableOffset + 4)
            Metrics.ItalicAngle = FontData.ReadShort() + FontData.ReadUnsignedShort() / 16384.0F ' // Fixed-point mantissa (16 bits) + Fixed-point fraction (16 bits).
            Metrics.UnderlinePosition = FontData.ReadShort()
            Metrics.UnderlineThickness = FontData.ReadShort()
            Metrics.IsFixedPitch = (FontData.ReadInt() <> 0)
        End Sub

        '/**
        '  <summary>Reads a string from the font file using the extended ASCII encoding.</summary>
        '*/
        Private Function ReadAsciiString(ByVal length As Integer) As String
            Return ReadString(length, Charset.ISO88591)
        End Function

        '/**
        '  <summary>Reads a string.</summary>
        '*/
        Private Function ReadString(ByVal length As Integer, ByVal platformID As Integer) As String
            ' Which platform?
            Select Case (platformID)
                Case PlatformID_Unicode, PlatformID_Microsoft
                    Return ReadUnicodeString(length)
                Case Else
                    Return ReadAsciiString(length)
            End Select
        End Function

        '/**
        '  <summary>Reads a string from the font file using the specified encoding.</summary>
        '*/
        Private Function ReadString(ByVal length As Integer, ByVal encoding As Text.Encoding) As String
            Dim data As Byte() = New Byte(length - 1) {}
            FontData.Read(data, 0, length)
            Return encoding.GetString(data)
        End Function

        '/**
        '  <summary>Reads a string from the font file using the Unicode encoding.</summary>
        '*/
        Private Function ReadUnicodeString(ByVal length As Integer) As String
            Return ReadString(length, Charset.UTF16LE)
        End Function

#End Region
#End Region
#End Region

    End Class

End Namespace
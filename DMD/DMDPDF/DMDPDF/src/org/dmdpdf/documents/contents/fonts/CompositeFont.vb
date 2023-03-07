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

'  You should have received a copy of the GNU Lesser General Public License along with this
'  Program (see README files); if not, go to the GNU website (http://www.gnu.org/licenses/).

'  Redistribution and use, with or without modification, are permitted provided that such
'  redistributions retain the above copyright notice, license and disclaimer, along with
'  this list of conditions.
'*/

Imports DMD.org.dmdpdf.bytes
Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects
Imports DMD.org.dmdpdf.util
Imports DMD.org.dmdpdf.util.io

Imports System
Imports System.IO
Imports System.Collections.Generic
Imports drawing = System.Drawing
Imports System.Text

Namespace DMD.org.dmdpdf.documents.contents.fonts

    '/**
    '  <summary>Composite font, also called Type 0 font [PDF:1.6:5.6].</summary>
    '  <remarks>Do not confuse it with <see cref="Type0Font">Type 0 CIDFont</see>: the latter is
    '  a composite font descendant describing glyphs based on Adobe Type 1 font format.</remarks>
    '*/
    <PDF(VersionEnum.PDF12)>
    Public MustInherit Class CompositeFont
        Inherits Font

#Region "static"
#Region "interface"
#Region "public"

        Public Shared Shadows Function [Get](ByVal context As Document, ByVal fontData As bytes.IInputStream) As CompositeFont
            Dim parser As OpenFontParser = New OpenFontParser(fontData)
            Select Case (parser.OutlineFormat)
                Case OpenFontParser.OutlineFormatEnum.CFF : Return New Type0Font(context, parser)
                Case OpenFontParser.OutlineFormatEnum.TrueType : Return New Type2Font(context, parser)
            End Select
            Throw New NotSupportedException("Unknown composite font format.")
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "fields"

#End Region

#Region "constructors"

        Friend Sub New(ByVal context As Document, ByVal parser As OpenFontParser)
            MyBase.New(context)
            Me.Load(parser)
        End Sub

        Protected Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "interface"
#Region "protected"

        '/**
        '  <summary> Gets the CIDFont dictionary that Is the descendant Of this composite font.</summary>
        '*/
        Protected ReadOnly Property CIDFontDictionary As PdfDictionary
            Get
                Return CType(CType(Me.baseDataObject.Resolve(PdfName.DescendantFonts), PdfArray).Resolve(0), PdfDictionary)
            End Get
        End Property

        Protected Overrides ReadOnly Property Descriptor As PdfDictionary
            Get
                Return CType(CIDFontDictionary.Resolve(PdfName.FontDescriptor), PdfDictionary)
            End Get
        End Property

        Protected Sub LoadEncoding()
            Dim encodingObject As PdfDataObject = Me.BaseDataObject.Resolve(PdfName.Encoding)

            ' CMap [PDF1.65.6.4].
            Dim cmap As IDictionary(Of ByteArray, Integer) = fonts.CMap.Get(encodingObject)

            ' 1. Unicode.
            If (Me._codes Is Nothing) Then
                Me._codes = New BiDictionary(Of ByteArray, Integer)()
                If (TypeOf (encodingObject) Is PdfName AndAlso
                    Not (encodingObject.Equals(PdfName.IdentityH) OrElse
                        encodingObject.Equals(PdfName.IdentityV))) Then
                    '/*
                    '  NOTE:   According to [PDF:1.6:5.9.1], the fallback method to retrieve
                    '  the character - code -To-Unicode mapping implies getting the UCS2 CMap
                    '  (Unicode value to CID) corresponding to the font's one (character code to CID);
                    '  CIDs are the bridge from character codes To Unicode values.
                    '*/
                    Dim ucs2CMap As BiDictionary(Of ByteArray, Integer)
                    '{
                    Dim cidSystemInfo As PdfDictionary = CType(CIDFontDictionary.Resolve(PdfName.CIDSystemInfo), PdfDictionary)
                    Dim registry As String = CStr(CType(cidSystemInfo(PdfName.Registry), PdfTextString).Value)
                    Dim ordering As String = CStr(CType(cidSystemInfo(PdfName.Ordering), PdfTextString).Value)
                    Dim ucs2CMapName As String = registry & "-" & ordering & "-" & "UCS2"
                    ucs2CMap = New BiDictionary(Of ByteArray, Integer)(fonts.CMap.Get(ucs2CMapName))
                    '}
                    If (ucs2CMap.Count > 0) Then
                        For Each cmapEntry As KeyValuePair(Of ByteArray, Integer) In cmap
                            Me._codes(cmapEntry.Key) = ConvertUtils.ByteArrayToInt(ucs2CMap.GetKey(cmapEntry.Value).Data)
                        Next
                    End If
                End If
                If (_codes.Count = 0) Then
                    '/*
                    '  NOTE: In case no clue Is available to determine the Unicode resolution map,
                    '  the Font Is considered symbolic And an identity map Is synthesized instead.
                    '*/
                    _symbolic = True
                    For Each cmapEntry As KeyValuePair(Of ByteArray, Integer) In cmap
                        _codes(cmapEntry.Key) = ConvertUtils.ByteArrayToInt(cmapEntry.Key.Data)
                    Next
                End If
            End If

            '// 2. Glyph indexes.
            '/*
            'TODO:         gids map For glyph indexes As glyphIndexes Is used To map cids!!!
            '*/
            '// Character-code-to-CID mapping [PDF:1.6:5.6.4,5].
            _glyphIndexes = New Dictionary(Of Integer, Integer)
            For Each cmapEntry As KeyValuePair(Of ByteArray, Integer) In cmap
                If (Not _codes.ContainsKey(cmapEntry.Key)) Then Continue For
                _glyphIndexes(_codes(cmapEntry.Key)) = cmapEntry.Value
            Next
        End Sub

        Protected Overrides Sub OnLoad()
            LoadEncoding()

            ' Glyph widths.
            '{
            _glyphWidths = New Dictionary(Of Integer, Integer)
            Dim glyphWidthObjects As PdfArray = CType(CIDFontDictionary.Resolve(PdfName.W), PdfArray)
            If (glyphWidthObjects IsNot Nothing) Then
                Dim Iterator As IEnumerator(Of PdfDirectObject) = glyphWidthObjects.GetEnumerator()
                While (Iterator.MoveNext())
                    '//TODO: this algorithm Is valid only In Case cid-To-gid mapping Is identity (see cidtogid map)!!
                    '/*
                    '  NOTE:                     Font widths are grouped In one Of the following formats [PDF: 1.6:5.6.3]
                    '    1. startCID [glyphWidth1 glyphWidth2 ... glyphWidthn]
                    '    2. startCID endCID glyphWidth
                    '*/
                    Dim startCID As Integer = CType(Iterator.Current, PdfInteger).RawValue
                    Iterator.MoveNext()
                    Dim glyphWidthObject2 As PdfDirectObject = Iterator.Current
                    If (TypeOf (glyphWidthObject2) Is PdfArray) Then ' Format() 1: startCID [glyphWidth1 glyphWidth2 ... glyphWidthn].
                        Dim cID As Integer = startCID
                        For Each glyphWidthObject As PdfDirectObject In CType(glyphWidthObject2, PdfArray)
                            _glyphWidths(cID) = CType(glyphWidthObject, PdfInteger).RawValue
                            cID += 1
                        Next
                    Else ' Format 2: startCID endCID glyphWidth.
                        Dim endCID As Integer = CType(glyphWidthObject2, PdfInteger).RawValue
                        Iterator.MoveNext()
                        Dim glyphWidth As Integer = CType(Iterator.Current, PdfInteger).RawValue
                        For cID As Integer = startCID To endCID
                            _glyphWidths(cID) = glyphWidth
                        Next
                    End If
                End While
            End If
            '}
            ' Default glyph width.
            '{
            Dim defaultGlyphWidthObject As PdfInteger = CType(Me.BaseDataObject(PdfName.W), PdfInteger)
            If (defaultGlyphWidthObject Is Nothing) Then
                _defaultGlyphWidth = 0
            Else
                _defaultGlyphWidth = defaultGlyphWidthObject.RawValue
            End If
            '}
        End Sub
#End Region

#Region "Private"
        '/**
        '  <summary> Loads the font data.</summary>
        '*/
        Private Overloads Sub Load(ByVal parser As OpenFontParser)
            _glyphIndexes = parser.GlyphIndexes
            _glyphKernings = parser.GlyphKernings
            _glyphWidths = parser.GlyphWidths

            Dim baseDataObject As PdfDictionary = Me.BaseDataObject

            ' BaseFont.
            baseDataObject(PdfName.BaseFont) = New PdfName(parser.FontName)

            ' Subtype.
            baseDataObject(PdfName.Subtype) = PdfName.Type0

            ' Encoding.
            baseDataObject(PdfName.Encoding) = PdfName.IdentityH 'TODO: this Is a simplification (to refine later).

            ' Descendant font.
            Dim CIDFontDictionary As PdfDictionary = New PdfDictionary(
                                                        New PdfName() {PdfName.Type},
                                                        New PdfDirectObject() {PdfName.Font}
                                                        ) ' CIDFont dictionary [PDF:1.6:5.6.3].
            '{
            ' Subtype.
            Dim subType As PdfName
            Select Case (parser.OutlineFormat)
                Case OpenFontParser.OutlineFormatEnum.TrueType : subType = PdfName.CIDFontType2'; break;
                Case OpenFontParser.OutlineFormatEnum.CFF : subType = PdfName.CIDFontType0 '; break;
                Case Else : Throw New NotImplementedException()
            End Select
            CIDFontDictionary(PdfName.Subtype) = subType

            ' BaseFont.
            CIDFontDictionary(PdfName.BaseFont) = New PdfName(parser.FontName)

            ' CIDSystemInfo.
            CIDFontDictionary(PdfName.CIDSystemInfo) = New PdfDictionary(
                                                            New PdfName() {PdfName.Registry, PdfName.Ordering, PdfName.Supplement},
                                                            New PdfDirectObject() {New PdfTextString("Adobe"), New PdfTextString("Identity"), PdfInteger.Get(0)}
                                                            ) ' Generic predefined CMap (Identity-H/V (Adobe-Identity-0)) [PDF:1.6:5.6.4].

            ' FontDescriptor.
            CIDFontDictionary(PdfName.FontDescriptor) = Load_CreateFontDescriptor(parser)

            ' Encoding.
            Load_CreateEncoding(baseDataObject, CIDFontDictionary)
            '}
            baseDataObject(PdfName.DescendantFonts) = New PdfArray(New PdfDirectObject() {File.Register(CIDFontDictionary)})

            Load()
        End Sub

        '/**
        '  <summary> Creates the character code mapping For composite fonts.</summary>
        '*/
        Private Sub Load_CreateEncoding(ByVal Font As PdfDictionary, ByVal cidFont As PdfDictionary)
            ' CMap [PDF:1.6:5.6.4].
            Dim cmapBuffer As bytes.Buffer = New bytes.Buffer()
            cmapBuffer.Append(
                    "%!PS-Adobe-3.0 Resource-CMap" & vbLf &
                    "%%DocumentNeededResources: ProcSet (CIDInit)" & vbLf &
                    "%%IncludeResource: ProcSet (CIDInit)" & vbLf &
                    "%%BeginResource: CMap (Adobe-Identity-UCS)" & vbLf &
                    "%%Title: (Adobe-Identity-UCS Adobe Identity 0)" & vbLf &
                    "%%Version: 1" & vbLf &
                    "%%EndComments" & vbLf &
                    "/CIDInit /ProcSet findresource begin" & vbLf &
                    "12 dict begin" & vbLf &
                    "begincmap" & vbLf &
                    "/CIDSystemInfo" & vbLf &
                    "3 dict dup begin" & vbLf &
                    "/Registry (Adobe) def" & vbLf &
                    "/Ordering (Identity) def" & vbLf &
                    "/Supplement 0 def" & vbLf &
                    "end def" & vbLf &
                    "/CMapName /Adobe-Identity-UCS def" & vbLf &
                    "/CMapVersion 1 def" & vbLf &
                    "/CMapType 0 def" & vbLf &
                    "/WMode 0 def" & vbLf &
                    "2 begincodespacerange" & vbLf &
                    "<20> <20>" & vbLf &
                    "<0000> <19FF>" & vbLf &
                    "endcodespacerange" & vbLf &
                    _glyphIndexes.Count & " begincidchar" & vbLf
                    )
            ' ToUnicode [PDF:1.6:5.9.2].
            Dim toUnicodeBuffer As New bytes.Buffer()
            toUnicodeBuffer.Append(
                        "/CIDInit /ProcSet findresource begin" & vbLf &
                        "12 dict begin" & vbLf &
                        "begincmap" & vbLf &
                        "/CIDSystemInfo" & vbLf &
                        "<< /Registry (Adobe)" & vbLf &
                        "/Ordering (UCS)" & vbLf &
                        "/Supplement 0" & vbLf &
                        ">> def" & vbLf &
                        "/CMapName /Adobe-Identity-UCS def" & vbLf &
                        "/CMapVersion 10.001 def" & vbLf &
                        "/CMapType 2 def" & vbLf &
                        "2 begincodespacerange" & vbLf &
                        "<20> <20>" & vbLf &
                        "<0000> <19FF>" & vbLf &
                        "endcodespacerange" & vbLf &
                        _glyphIndexes.Count & " beginbfchar" & vbLf
                        )
            ' CIDToGIDMap [PDF:1.6:5.6.3].
            Dim gIdBuffer As New bytes.Buffer()
            gIdBuffer.Append(CByte(0))
            gIdBuffer.Append(CByte(0))
            Dim code As Integer = 0
            _codes = New BiDictionary(Of ByteArray, Integer)(_glyphIndexes.Count)
            Dim widthsObject As PdfArray = New PdfArray(_glyphWidths.Count)
            For Each glyphIndexEntry As KeyValuePair(Of Integer, Integer) In _glyphIndexes
                ' Character code (unicode to codepoint) entry.
                code += 1
                Dim charCode As Byte()
                If (glyphIndexEntry.Key = 32) Then
                    charCode = New Byte() {32}
                Else
                    charCode = New Byte() {
                                        CByte((code >> 8) And &HFF),
                                        CByte(code And &HFF)
                                        }
                End If

                _codes(New ByteArray(charCode)) = glyphIndexEntry.Key

                ' CMap entry.
                cmapBuffer.Append("<")
                toUnicodeBuffer.Append("<")
                Dim charCodeBytesLength As Integer = charCode.Length
                For charCodeBytesIndex As Integer = 0 To charCodeBytesLength - 1
                    Dim Hex As String = CInt(charCode(charCodeBytesIndex)).ToString("X2")
                    cmapBuffer.Append(Hex)
                    toUnicodeBuffer.Append(Hex)
                Next
                cmapBuffer.Append("> " & code & vbLf)
                toUnicodeBuffer.Append("> <" + glyphIndexEntry.Key.ToString("X4") + ">" & vbLf)

                ' CID-to-GID entry.
                Dim glyphIndex As Integer = glyphIndexEntry.Value
                gIdBuffer.Append(CByte((glyphIndex >> 8) And &HFF))
                gIdBuffer.Append(CByte(glyphIndex And &HFF))

                ' Width.
                Dim width As Integer
                If (Not _glyphWidths.TryGetValue(glyphIndex, width)) Then
                    width = 0
                ElseIf (width > 1000) Then
                    width = 1000
                End If
                widthsObject.Add(PdfInteger.Get(width))
            Next
            cmapBuffer.Append(
                        "endcidchar" & vbLf &
                        "endcmap" & vbLf &
                        "CMapName currentdict /CMap defineresource pop" & vbLf &
                        "end" & vbLf &
                        "end" & vbLf &
                        "%%EndResource" & vbLf &
                        "%%EOF"
                        )
            Dim cmapStream As PdfStream = New PdfStream(cmapBuffer)
            Dim cmapHead As PdfDictionary = cmapStream.Header
            cmapHead(PdfName.Type) = PdfName.CMap
            cmapHead(PdfName.CMapName) = New PdfName("Adobe-Identity-UCS")
            cmapHead(PdfName.CIDSystemInfo) = New PdfDictionary(
                                                    New PdfName() {PdfName.Registry, PdfName.Ordering, PdfName.Supplement},
                                                    New PdfDirectObject() {New PdfTextString("Adobe"), New PdfTextString("Identity"), PdfInteger.Get(0)}
                                                    ) ' Generic predefined CMap (Identity-H/V (Adobe-Identity-0)) [PDF:1.6:5.6.4].
            Font(PdfName.Encoding) = File.Register(cmapStream)
            Dim gIdStream As PdfStream = New PdfStream(gIdBuffer)
            cidFont(PdfName.CIDToGIDMap) = File.Register(gIdStream)
            cidFont(PdfName.W) = New PdfArray(New PdfDirectObject() {PdfInteger.Get(1), widthsObject})

            toUnicodeBuffer.Append(
                        "endbfchar" & vbLf &
                        "endcmap" & vbLf &
                        "CMapName currentdict /CMap defineresource pop" & vbLf &
                        "end" & vbLf &
                        "end" & vbLf
                        )
            Dim toUnicodeStream As PdfStream = New PdfStream(toUnicodeBuffer)
            Font(PdfName.ToUnicode) = File.Register(toUnicodeStream)
        End Sub

        '/**
        '  <summary> Creates the font descriptor.</summary>
        '*/
        Private Function Load_CreateFontDescriptor(ByVal parser As OpenFontParser) As PdfReference
            Dim fontDescriptor As PdfDictionary = New PdfDictionary()
            '{
            Dim metrics As OpenFontParser.FontMetrics = parser.Metrics

            ' Type.
            fontDescriptor(PdfName.Type) = PdfName.FontDescriptor

            ' FontName.
            fontDescriptor(PdfName.FontName) = Me.BaseDataObject(PdfName.BaseFont)

            ' Flags [PDF1.6:5.7.1].
            Dim flags As FlagsEnum = 0
            If (metrics.IsFixedPitch) Then
                flags = flags Or FlagsEnum.FixedPitch
            End If
            If (metrics.IsCustomEncoding) Then
                flags = flags Or FlagsEnum.Symbolic
            Else
                flags = flags Or FlagsEnum.Nonsymbolic
            End If
            fontDescriptor(PdfName.Flags) = PdfInteger.Get(Convert.ToInt32(flags))

            ' FontBBox.
            fontDescriptor(PdfName.FontBBox) = New Rectangle(
                                                New drawing.PointF(metrics.XMin * metrics.UnitNorm, metrics.YMin * metrics.UnitNorm),
                                                New drawing.PointF(metrics.XMax * metrics.UnitNorm, metrics.YMax * metrics.UnitNorm)
                                                ).BaseDataObject

            ' ItalicAngle.
            fontDescriptor(PdfName.ItalicAngle) = PdfReal.Get(metrics.ItalicAngle)

            ' Ascent.
            If (metrics.Ascender = 0) Then
                fontDescriptor(PdfName.Ascent) = PdfReal.Get(metrics.STypoAscender * metrics.UnitNorm)
            Else
                fontDescriptor(PdfName.Ascent) = PdfReal.Get(metrics.Ascender * metrics.UnitNorm)
            End If

            ' Descent.
            If (metrics.Descender = 0) Then
                fontDescriptor(PdfName.Descent) = PdfReal.Get(metrics.STypoDescender * metrics.UnitNorm)
            Else
                fontDescriptor(PdfName.Descent) = PdfReal.Get(metrics.Descender * metrics.UnitNorm)
            End If

            ' Leading.
            fontDescriptor(PdfName.Leading) = PdfReal.Get(metrics.STypoLineGap * metrics.UnitNorm)

            ' CapHeight.
            fontDescriptor(PdfName.CapHeight) = PdfReal.Get(metrics.SCapHeight * metrics.UnitNorm)

            '        // StemV.
            '        /*
            '          NOTE: '100' is just a rule-of-thumb value, 'cause I've still to solve the
            '        'cvt' table puzzle (such a harsh headache!) for TrueType fonts...
            'TODO:IMPL TrueType And CFF stemv real value To extract!!!
            '        */
            fontDescriptor(PdfName.StemV) = PdfInteger.Get(100)

            ' FontFile.
            fontDescriptor(PdfName.FontFile2) = File.Register(New PdfStream(New bytes.Buffer(parser.FontData.ToByteArray())))
            '}
            Return File.Register(fontDescriptor)
        End Function

#End Region
#End Region
#End Region

    End Class

End Namespace
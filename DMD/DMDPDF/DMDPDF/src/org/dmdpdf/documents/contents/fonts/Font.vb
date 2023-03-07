'/*
'  Copyright 2006-2011 Stefano Chizzolini. http://www.dmdpdf.org

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

Imports System
Imports System.IO
Imports System.Collections.Generic
Imports System.Text

Namespace DMD.org.dmdpdf.documents.contents.fonts

    '/**
    '  <summary>Abstract font [PDF:1.6:5.4].</summary>
    '*/
    <PDF(VersionEnum.PDF10)>
    Public MustInherit Class Font
        Inherits PdfObjectWrapper(Of PdfDictionary)

#Region "types"

        '/**
        '  <summary>Font descriptor flags [PDF:1.6:5.7.1].</summary>
        '*/
        <Flags>
        Public Enum FlagsEnum

            '  /**
            '  <summary>All glyphs have the same width.</summary>
            '*/
            FixedPitch = &H1
            '/**
            '  <summary>Glyphs have serifs.</summary>
            '*/
            Serif = &H2
            '/**
            '  <summary>Font contains glyphs outside the Adobe standard Latin character set.</summary>
            '*/
            Symbolic = &H4
            '/**
            '  <summary>Glyphs resemble cursive handwriting.</summary>
            '*/
            Script = &H8
            '/**
            '  <summary>Font uses the Adobe standard Latin character set.</summary>
            '*/
            Nonsymbolic = &H20
            '/**
            '  <summary>Glyphs have dominant vertical strokes that are slanted.</summary>
            '*/
            Italic = &H40
            '/**
            '  <summary>Font contains no lowercase letters.</summary>
            '*/
            AllCap = &H10000
            '/**
            '  <summary>Font contains both uppercase and lowercase letters.</summary>
            '*/
            SmallCap = &H20000
            '/**
            '  <summary>Thicken bold glyphs at small text sizes.</summary>
            '*/
            ForceBold = &H40000
        End Enum
#End Region

#Region "static"
#Region "interface"
#Region "public"

        '/**
        '  <summary> Creates the representation Of a font.</summary>
        '*/
        Public Shared Function [Get](ByVal context As Document, ByVal path As String) As Font
            Return [Get](
                        context,
                        New bytes.Stream(
                                New IO.FileStream(
                                    path,
                                    IO.FileMode.Open,
                                    IO.FileAccess.Read
                                    )
                               )
                           )
        End Function

        '/**
        '  <summary> Creates the representation Of a font.</summary>
        '*/
        Public Shared Function [Get](ByVal context As Document, ByVal fontData As IInputStream) As Font
            If (OpenFontParser.IsOpenFont(fontData)) Then
                Return CompositeFont.Get(context, fontData)
            Else
                Throw New NotImplementedException()
            End If
        End Function

        '/**
        '  <summary> Gets the scaling factor To be applied To unscaled metrics To Get actual
        '  measures.</summary>
        '*/
        Public Shared Function GetScalingFactor(ByVal size As Double) As Double
            Return 0.001 * size
        End Function

        '/**
        '  <summary> Wraps a font reference into a font Object.</summary>
        '  <param name = "baseObject" > Font base Object.</param>
        '  <returns> Font Object associated To the reference.</returns>
        '*/
        Public Shared Function Wrap(ByVal baseObject As PdfDirectObject) As Font
            If (baseObject Is Nothing) Then Return Nothing
            Dim reference As PdfReference = CType(baseObject, PdfReference)
            '{
            '// Has the font been already instantiated?
            '/*
            '  NOTE: Font structures are reified As complex objects, both IO- And CPU-intensive To load.
            '  So, it's convenient to retrieve them from a common cache whenever possible.
            '*/
            Dim cache As Dictionary(Of PdfReference, Object) = reference.IndirectObject.File.Document._Cache
            If (cache.ContainsKey(reference)) Then
                Return CType(cache(reference), Font)
            End If
            '}

            Dim fontDictionary As PdfDictionary = CType(reference.DataObject, PdfDictionary)
            Dim fontType As PdfName = CType(fontDictionary(PdfName.Subtype), PdfName)
            If (fontType Is Nothing) Then Throw New Exception("Font type undefined (reference: " & reference.ToString & ")")

            If (fontType.Equals(PdfName.Type1)) Then ' Type Then 1.
                If (Not fontDictionary.ContainsKey(PdfName.FontDescriptor)) Then ' Standard Then Type 1.
                    Return New StandardType1Font(reference)
                Else ' Custom Type 1.
                    Dim fontDescriptor As PdfDictionary = CType(fontDictionary.Resolve(PdfName.FontDescriptor), PdfDictionary)
                    If (
                        fontDescriptor.ContainsKey(PdfName.FontFile3) AndAlso
                        CType(CType(fontDescriptor.Resolve(PdfName.FontFile3), PdfStream).Header.Resolve(PdfName.Subtype), PdfName).Equals(PdfName.OpenType)
                        ) Then ' OpenFont/CFF.
                        Throw New NotImplementedException()
                    Else ' Non-OpenFont Type 1.
                        Return New Type1Font(reference)
                    End If
                End If
            ElseIf (fontType.Equals(PdfName.TrueType)) Then '// TrueType.
                Return New TrueTypeFont(reference)
            ElseIf (fontType.Equals(PdfName.Type0)) Then ' OpenFont.
                Dim cidFontDictionary As PdfDictionary = CType(CType(fontDictionary.Resolve(PdfName.DescendantFonts), PdfArray).Resolve(0), PdfDictionary)
                Dim cidFontType As PdfName = CType(cidFontDictionary(PdfName.Subtype), PdfName)
                If (cidFontType.Equals(PdfName.CIDFontType0)) Then ' OpenFont / CFF.
                    Return New Type0Font(reference)
                ElseIf (cidFontType.Equals(PdfName.CIDFontType2)) Then ' OpenFont / TrueType.
                    Return New Type2Font(reference)
                Else
                    Throw New NotImplementedException("Type 0 subtype " & cidFontType.ToString & " not supported yet.")
                End If
            ElseIf (fontType.Equals(PdfName.Type3)) Then ' Type Then 3.
                Return New Type3Font(reference)
            ElseIf (fontType.Equals(PdfName.MMType1)) Then ' MMType1.
                Return New MMType1Font(reference)
            Else ' Unknown.
                Throw New NotSupportedException("Unknown font type: " & fontType.ToString & " (reference: " & reference.ToString & ")")
            End If
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "fields"

        '/*
        '  NOTE: In order to avoid nomenclature ambiguities, these terms are used consistently within the
        '  code:
        '  * character code: internal codepoint corresponding To a character expressed inside a String
        '    Object of a content stream;
        '  * unicode: external codepoint corresponding To a character expressed according To the Unicode
        '    standard encoding;
        '  * glyph index: internal identifier Of the graphical representation Of a character.
        '*/
        '/**
        '  <summary> Unicodes by character code.</summary>
        '                  <remarks>
        '                      <para>When this map Is populated, <code>symbolic</code> variable shall accordingly be Set.</para>
        '                  </remarks>
        '*/
        Protected _codes As BiDictionary(Of ByteArray, Integer)
        '/**
        '  <summary> Default glyph width.</summary>
        '*/
        Protected _defaultGlyphWidth As Integer
        '/**
        '  <summary> Glyph indexes by unicode.</summary>
        '*/
        Protected _glyphIndexes As Dictionary(Of Integer, Integer)
        '/**
        '  <summary> Glyph kernings by (left-right) glyph index pairs.</summary>
        '*/
        Protected _glyphKernings As Dictionary(Of Integer, Integer)
        '/**
        '  <summary> Glyph widths by glyph index.</summary>
        '*/
        Protected _glyphWidths As Dictionary(Of Integer, Integer)
        '/**
        '  <summary> Whether the font encoding Is custom (that Is non-Unicode).</summary>
        '*/
        Protected _symbolic As Boolean = True
        '/**
        '  <summary> Used unicodes.</summary>
        '*/
        Protected _usedCodes As HashSet(Of Integer)

        '/**
        '  <summary> Maximum character code Byte size.</summary>
        '*/
        Private _charCodeMaxLength As Integer = 0

#End Region

#Region "constructors"

        '/**
        '  <summary> Creates a New font Structure within the given document context.</summary>
        '*/
        Protected Sub New(ByVal context As Document)
            MyBase.New(
                        context,
                        New PdfDictionary(
                            New PdfName() {PdfName.Type},
                            New PdfDirectObject() {PdfName.Font}
                            )
                       )
            Initialize()
        End Sub

        '/**
        '  <summary> Loads an existing font Structure.</summary>
        '*/
        Protected Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
            Initialize()
            Load()
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary> Gets the unscaled vertical offset from the baseline To the ascender line (ascent).
        '  The value Is a positive number.</summary>
        '*/
        Public Overridable ReadOnly Property Ascent As Double
            Get
                Return CType(Descriptor(PdfName.Ascent), IPdfNumber).RawValue
            End Get
        End Property

        '/**
        '  <summary> Gets the text from the given internal representation.</summary>
        '                                      <param name = "code" > Internal representation To decode.</param>
        '*/
        Public Function Decode(ByVal code As Byte()) As String
            Dim textBuilder As StringBuilder = New StringBuilder()
            '{
            Dim codeBuffers As Byte()() = New Byte(_charCodeMaxLength + 1 - 1)() {}
            For codeBufferIndex As Integer = 0 To _charCodeMaxLength
                codeBuffers(codeBufferIndex) = New Byte(codeBufferIndex - 1) {}
            Next
            Dim position As Integer = 0
            Dim codeLength As Integer = code.Length
            Dim codeBufferSize As Integer = 1
            While (position < codeLength)
                Dim codeBuffer As Byte() = codeBuffers(codeBufferSize)
                System.Buffer.BlockCopy(code, position, codeBuffer, 0, codeBufferSize)
                Dim textChar As Integer = 0
                If (Not _codes.TryGetValue(New ByteArray(codeBuffer), textChar)) Then
                    If (codeBufferSize < _charCodeMaxLength) Then
                        codeBufferSize += 1
                        Continue While
                    End If
                    '/*
                    '  NOTE: In case no valid code entry Is found, a default space Is resiliantely
                    '  applied instead Of throwing an exception.
                    '  This Is potentially risky as failing to determine the actual code length
                    '  may result In a "code shifting" which could affect following characters.
                    '*/
                    textChar = Asc(" "c)
                End If
                textBuilder.Append(ChrW(textChar))
                position += codeBufferSize
                codeBufferSize = 1
            End While
            '}
            Return textBuilder.ToString()
        End Function

        '/**
        '  <summary> Gets the unscaled vertical offset from the baseline To the descender line (descent).
        '  The value Is a negative number.</summary>
        '*/
        Public Overridable ReadOnly Property Descent As Double
            Get
                Return CType(Descriptor(PdfName.Descent), IPdfNumber).RawValue
            End Get
        End Property

        '/**
        '  <summary> Gets the internal representation Of the given text.</summary>
        '  <param name = "text" > Text To encode.</param>
        '*/
        Public Function Encode(ByVal Text As String) As Byte()
            Dim encodedStream As New IO.MemoryStream()
            Dim length As Integer = Text.Length
            For index As Integer = 0 To length - 1
                Dim textCode As Integer = Asc(Text(index))
                Dim charCode As Byte() = _codes.GetKey(textCode).Data
                If (charCode Is Nothing) Then
                Else
                    encodedStream.Write(charCode, 0, charCode.Length)
                End If
                _usedCodes.Add(textCode)
            Next
            encodedStream.Close()
            Return encodedStream.ToArray()
        End Function

        Public Overrides Function Equals(ByVal obj As Object) As Boolean
            Return (obj IsNot Nothing) AndAlso
                    (obj.GetType().Equals(Me.GetType())) AndAlso
                     CType(obj, Font).Name.Equals(Me.Name)
        End Function

        '/**
        '  <summary> Gets the font descriptor flags.</summary>
        '*/
        Public Overridable ReadOnly Property Flags As FlagsEnum
            Get
                Dim flagsObject As PdfInteger = CType(Descriptor.Resolve(PdfName.Flags), PdfInteger)
                If (flagsObject Is Nothing) Then
                    Return CType([Enum].ToObject(GetType(FlagsEnum), Nothing), FlagsEnum)
                End If
                Return CType([Enum].ToObject(GetType(FlagsEnum), flagsObject.RawValue), FlagsEnum)
            End Get
        End Property

        '/**
        '  <summary> Gets the vertical offset from the baseline To the ascender line (ascent),
        '  scaled to the given font size. The value Is a positive number.</summary>
        '  <param name = "size" > Font size.</param>
        '*/
        Public Function GetAscent(ByVal size As Double) As Double
            Return Ascent * GetScalingFactor(size)
        End Function

        '/**
        '  <summary> Gets the vertical offset from the baseline To the descender line (descent),
        '  scaled to the given font size. The value Is a negative number.</summary>
        '  <param name = "size" > Font size.</param>
        '*/
        Public Function GetDescent(ByVal size As Double) As Double
            Return Descent * GetScalingFactor(size)
        End Function

        Public Overrides Function GetHashCode() As Integer
            Return Me.Name.GetHashCode()
        End Function

        '/**
        '  <summary> Gets the unscaled height Of the given character.</summary>
        '  <param name = "textChar" > Character whose height has To be calculated.</param>
        '*/
        Public Function GetHeight(ByVal TextChar As Char) As Double
            Return LineHeight
        End Function

        '/**
        '  <summary> Gets the height Of the given character, scaled To the given font size.</summary>
        '  <param name = "textChar" > Character whose height has To be calculated.</param>
        '  <param name = "size" > Font size.</param>
        '*/
        Public Function GetHeight(ByVal TextChar As Char, ByVal size As Double) As Double
            Return GetHeight(TextChar) * GetScalingFactor(size)
        End Function

        '/**
        '  <summary> Gets the unscaled height Of the given text.</summary>
        '  <param name = "text" > Text whose height has To be calculated.</param>
        '*/
        Public Function GetHeight(ByVal Text As String) As Double
            Return LineHeight
        End Function

        '/**
        '  <summary> Gets the height Of the given text, scaled To the given font size.</summary>
        '  <param name = "text" > Text whose height has To be calculated.</param>
        '  <param name = "size" > Font size.</param>
        '*/
        Public Function GetHeight(ByVal Text As String, ByVal size As Double) As Double
            Return GetHeight(Text) * GetScalingFactor(size)
        End Function

        '/**
        '  <summary> Gets the width (kerning inclusive) Of the given text, scaled To the given font size.</summary>
        '  <param name = "text" > Text whose width has To be calculated.</param>
        '  <param name = "size" > Font size.</param>
        '*/
        Public Function GetKernedWidth(ByVal text As String, ByVal size As Double) As Double
            Return (GetWidth(text) + GetKerning(text)) * GetScalingFactor(size)
        End Function

        '/**
        '  <summary> Gets the unscaled kerning width between two given characters.</summary>
        '  <param name = "textChar1" > Left() character.</param>
        '  <param name = "textChar2" > Right() character,</param>
        '*/
        Public Function GetKerning(ByVal textChar1 As Char, ByVal textChar2 As Char) As Integer
            If (_glyphKernings Is Nothing) Then Return 0
            Dim textChar1Index As Integer
            If (Not _glyphIndexes.TryGetValue(Asc(textChar1), textChar1Index)) Then Return 0

            Dim textChar2Index As Integer
            If (Not _glyphIndexes.TryGetValue(Asc(textChar2), textChar2Index)) Then Return 0

            Dim kerning As Integer
            '    If (!_glyphKernings.TryGetValue(
            'textChar1Index << 16 // Left() - hand glyph index.
            '  + textChar2Index, // Right-hand glyph index.
            'out kerning))
            If (Not _glyphKernings.TryGetValue(textChar1Index << 16 + textChar2Index, kerning)) Then
                Return 0
            End If

            Return kerning
        End Function

        '/**
        '  <summary> Gets the unscaled kerning width inside the given text.</summary>
        '  <param name = "text" > Text whose kerning has To be calculated.</param>
        '*/
        Public Function GetKerning(ByVal Text As String) As Integer
            Dim kerning As Integer = 0
            Dim textChars As Char() = Text.ToCharArray()
            Dim length As Integer = Text.Length - 1
            For index As Integer = 0 To length - 1
                kerning += GetKerning(textChars(index), textChars(index + 1))
            Next
            Return kerning
        End Function

        '/**
        '  <summary> Gets the kerning width inside the given text, scaled To the given font size.</summary>
        '  <param name = "text" > Text whose kerning has To be calculated.</param>
        '  <param name = "size" > Font size.</param>
        '*/
        Public Function GetKerning(ByVal Text As String, ByVal size As Double) As Double
            Return GetKerning(Text) * GetScalingFactor(size)
        End Function

        '/**
        '  <summary> Gets the line height, scaled To the given font size.</summary>
        '  <param name = "size" > Font size.</param>
        '*/
        Public Function GetLineHeight(ByVal size As Double) As Double
            Return LineHeight * GetScalingFactor(size)
        End Function

        '/**
        '  <summary> Gets the unscaled width Of the given character.</summary>
        '  <param name = "textChar" > Character whose width has To be calculated.</param>
        '*/
        Public Function GetWidth(ByVal TextChar As Char) As Integer
            Dim glyphIndex As Integer = 0
            If (Not _glyphIndexes.TryGetValue(Asc(TextChar), glyphIndex)) Then
                Return 0
            End If

            Dim glyphWidth As Integer = 0
            If (_glyphWidths.TryGetValue(glyphIndex, glyphWidth)) Then
                Return glyphWidth
            Else
                Return _defaultGlyphWidth
            End If
        End Function

        '/**
        '  <summary> Gets the width Of the given character, scaled To the given font size.</summary>
        '  <param name = "textChar" > Character whose height has To be calculated.</param>
        '  <param name = "size" > Font size.</param>
        '*/
        Public Function GetWidth(ByVal TextChar As Char, ByVal size As Double) As Double
            Return GetWidth(TextChar) * GetScalingFactor(size)
        End Function

        '/**
        '  <summary> Gets the unscaled width (kerning exclusive) Of the given text.</summary>
        '  <param name = "text" > Text whose width has To be calculated.</param>
        '*/
        Public Function GetWidth(ByVal Text As String) As Integer
            Dim width As Integer = 0
            For Each textChar As Char In Text.ToCharArray()
                width += GetWidth(textChar)
            Next
            Return width
        End Function

        '/**
        '  <summary> Gets the width (kerning exclusive) Of the given text, scaled To the given font
        '  size.</summary>
        '  <param name = "text" > Text whose width has To be calculated.</param>
        '  <param name = "size" > Font size.</param>
        '*/
        Public Function GetWidth(ByVal Text As String, ByVal size As Double) As Double
            Return GetWidth(Text) * GetScalingFactor(size)
        End Function

        '/**
        '  <summary> Gets the unscaled line height.</summary>
        '*/
        Public ReadOnly Property LineHeight As Double
            Get
                Return Ascent - Descent
            End Get
        End Property

        '/**
        '  <summary> Gets the PostScript name Of the font.</summary>
        '*/
        Public ReadOnly Property Name As String
            Get
                Return CType(Me.BaseDataObject(PdfName.BaseFont), PdfName).ToString()
            End Get
        End Property

        '/**
        '  <summary> Gets whether the font encoding Is custom (that Is non-Unicode).</summary>
        '*/
        Public ReadOnly Property Symbolic As Boolean
            Get
                Return Me._symbolic
            End Get
        End Property

#End Region

#Region "protected"

        '/**
        '  <summary> Gets the font descriptor.</summary>
        '*/
        Protected MustOverride ReadOnly Property Descriptor As PdfDictionary

        '    /**
        '  <summary> Loads font information from existing PDF font Structure.</summary>
        '*/
        Protected Sub Load()
            If (BaseDataObject.ContainsKey(PdfName.ToUnicode)) Then ' To-Unicode explicit mapping.
                Dim toUnicodeStream As PdfStream = CType(BaseDataObject.Resolve(PdfName.ToUnicode), PdfStream)
                Dim parser As CMapParser = New CMapParser(toUnicodeStream.Body)
                _codes = New BiDictionary(Of ByteArray, Integer)(parser.Parse())
                _symbolic = False
            End If

            OnLoad()

            ' Maximum character code length.
            For Each charCode As ByteArray In _codes.Keys
                If (charCode.Data.Length > _charCodeMaxLength) Then
                    _charCodeMaxLength = charCode.Data.Length
                End If
            Next
        End Sub

        '/**
        '  <summary> Notifies the loading Of font information from an existing PDF font Structure.</summary>
        '*/
        Protected MustOverride Sub OnLoad()

#End Region

#Region "Private"

        Private Sub Initialize()
            _usedCodes = New HashSet(Of Integer)()

            '// Put the newly-instantiated font into the common cache!
            '/*
            '  NOTE: Font structures are reified As complex objects, both IO- And CPU-intensive To load.
            '  So, it's convenient to put them into a common cache for later reuse.
            '*/
            Me.Document._Cache(CType(BaseObject, PdfReference)) = Me
        End Sub

#End Region
#End Region
#End Region
    End Class

End Namespace
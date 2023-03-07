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

Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects
Imports DMD.org.dmdpdf.util

Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Reflection

Namespace DMD.org.dmdpdf.documents.contents.fonts

    '/**
    '  <summary>TrueType font [PDF:1.6:5;OFF:2009].</summary>
    '*/
    <PDF(VersionEnum.PDF10)>
    Public NotInheritable Class TrueTypeFont
        Inherits SimpleFont

#Region "dynamic"
#Region "constructors"

        Friend Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "Interface"
#Region "Protected"

        Protected Overrides Sub LoadEncoding()
            Dim parser As OpenFontParser
            '{
            Dim descriptor As PdfDictionary = Me.Descriptor
            If (descriptor.ContainsKey(PdfName.FontFile2)) Then ' Embedded TrueType font file (without 'glyf' table).
                Dim fontFileStream As PdfStream = CType(descriptor.Resolve(PdfName.FontFile2), PdfStream)
                parser = New OpenFontParser(fontFileStream.Body)
            ElseIf (descriptor.ContainsKey(PdfName.FontFile3)) Then
                Dim fontFileStream As PdfStream = CType(descriptor.Resolve(PdfName.FontFile3), PdfStream)
                Dim fontFileSubtype As PdfName = CType(fontFileStream.Header(PdfName.Subtype), PdfName)
                If (fontFileSubtype.Equals(PdfName.OpenType)) Then 'Embedded OpenFont/TrueType font file (with 'glyf' table).
                    parser = New OpenFontParser(fontFileStream.Body)
                Else ' Unknown.
                    Throw New NotSupportedException("Unknown embedded font file format: " & fontFileSubtype.ToString)
                End If
            Else
                parser = Nothing
            End If
            '}
            If (parser IsNot Nothing) Then ' Embedded font file.
                ' Glyph indexes.
                _glyphIndexes = parser.GlyphIndexes
                If (_codes IsNot Nothing AndAlso
                    parser.Metrics.IsCustomEncoding) Then
                    '/*
                    '  NOTE: In case of symbolic font,
                    '  glyph indices are natively mapped to character codes,
                    '  so they must be remapped to Unicode whenever possible
                    '  (i.e. when ToUnicode stream is available).
                    '*/
                    Dim unicodeGlyphIndexes As Dictionary(Of Integer, Integer) = New Dictionary(Of Integer, Integer)()
                    For Each glyphIndexEntry As KeyValuePair(Of Integer, Integer) In _glyphIndexes
                        Dim code As Integer
                        If (Not _codes.TryGetValue(New ByteArray(New Byte() {CByte(CInt(glyphIndexEntry.Key))}), code)) Then
                            Continue For
                        End If

                        unicodeGlyphIndexes(code) = glyphIndexEntry.Value
                    Next
                    _glyphIndexes = unicodeGlyphIndexes
                End If
            End If

            Dim encodingObject As PdfDataObject = Me.BaseDataObject.Resolve(PdfName.Encoding)
            Dim Flags As FlagsEnum = Flags
            If ((Flags And FlagsEnum.Symbolic) <> 0 OrElse
               ((Flags And FlagsEnum.Nonsymbolic) = 0 AndAlso encodingObject Is Nothing)) Then ' Symbolic.
                _symbolic = True

                If (_glyphIndexes Is Nothing) Then
                    '/*
                    '  NOTE: In case no font file Is available, we have to synthesize its metrics
                    '  From existing entries.
                    '*/
                    _glyphIndexes = New Dictionary(Of Integer, Integer)
                    Dim glyphWidthObjects As PdfArray = CType(BaseDataObject.Resolve(PdfName.Widths), PdfArray)
                    If (glyphWidthObjects IsNot Nothing) Then
                        Dim code As Integer = CType(BaseDataObject(PdfName.FirstChar), PdfInteger).RawValue
                        For Each glyphWidthObject As PdfDirectObject In glyphWidthObjects
                            If (CType(glyphWidthObject, PdfInteger).RawValue > 0) Then
                                _glyphIndexes(code) = code
                            End If
                            code += 1
                        Next
                    End If
                End If

                If (Me._codes Is Nothing) Then
                    Dim codes As Dictionary(Of ByteArray, Integer) = New Dictionary(Of ByteArray, Integer)
                    For Each glyphIndexEntry As KeyValuePair(Of Integer, Integer) In _glyphIndexes
                        If (glyphIndexEntry.Value > 0) Then
                            Dim glyphCharCode As Integer = glyphIndexEntry.Key
                            Dim charCode As Byte() = New Byte() {CByte(glyphCharCode)}
                            codes(New ByteArray(charCode)) = glyphCharCode
                        End If
                    Next
                    Me._codes = New BiDictionary(Of ByteArray, Integer)(codes)
                End If
            Else ' Nonsymbolic.
                _symbolic = False

                If (Me._codes Is Nothing) Then
                    Dim codes As Dictionary(Of ByteArray, Integer)
                    If (encodingObject Is Nothing) Then ' Default encoding.
                        codes = Encoding.Get(PdfName.StandardEncoding).GetCodes()
                    ElseIf (TypeOf (encodingObject) Is PdfName) Then ' Predefined Then encoding.
                        codes = Encoding.Get(CType(encodingObject, PdfName)).GetCodes()
                    Else ' Custom encoding.
                        Dim encodingDictionary As PdfDictionary = CType(encodingObject, PdfDictionary)
                        ' 1. Base encoding.
                        Dim baseEncodingName As PdfName = CType(encodingDictionary(PdfName.BaseEncoding), PdfName)
                        If (baseEncodingName Is Nothing) Then ' Default base encoding.
                            codes = Encoding.Get(PdfName.StandardEncoding).GetCodes()
                        Else ' Predefined base encoding.
                            codes = Encoding.Get(baseEncodingName).GetCodes()
                        End If

                        ' 2. Differences.
                        LoadEncodingDifferences(encodingDictionary, codes)
                    End If
                    Me._codes = New BiDictionary(Of ByteArray, Integer)(codes)
                End If

                If (_glyphIndexes Is Nothing) Then
                    '/*
                    '  NOTE: In case no font file is available, we have to synthesize its metrics
                    '  from existing entries.
                    '*/
                    _glyphIndexes = New Dictionary(Of Integer, Integer)()
                    Dim glyphWidthObjects As PdfArray = CType(BaseDataObject.Resolve(PdfName.Widths), PdfArray)
                    If (glyphWidthObjects IsNot Nothing) Then
                        Dim charCode As ByteArray = New ByteArray(New Byte() {CByte(CInt(CType(BaseDataObject(PdfName.FirstChar), PdfInteger).RawValue))})
                        For Each glyphWidthObject As PdfDirectObject In glyphWidthObjects
                            If (CType(glyphWidthObject, PdfInteger).RawValue > 0) Then
                                Dim code As Integer = 0
                                If (_codes.TryGetValue(charCode, code)) Then
                                    _glyphIndexes(code) = CInt(charCode.Data(0))
                                End If
                            End If
                            charCode.Data(0) += CByte(1)
                        Next
                    End If
                End If
            End If
        End Sub

#End Region
#End Region
#End Region

    End Class

End Namespace
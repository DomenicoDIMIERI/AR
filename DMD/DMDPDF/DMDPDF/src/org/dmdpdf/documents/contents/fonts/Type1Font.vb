'/*
'  Copyright 2007-2012 Stefano Chizzolini. http://www.dmdpdf.org

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
Imports DMD.org.dmdpdf.objects
Imports DMD.org.dmdpdf.util

Imports System
Imports System.Collections.Generic
Imports System.Globalization
Imports System.IO
Imports System.Reflection
Imports System.Text
Imports System.Text.RegularExpressions

Namespace DMD.org.dmdpdf.documents.contents.fonts

    '/**
    '  <summary>Type 1 font [PDF:1.6:5.5.1;AFM:4.1].</summary>
    '*/
    '/*
    '  NOTE: Type 1 fonts encompass several formats:
    '  * AFM+PFB;
    '  * CFF;
    '  * OpenFont/CFF (in case "CFF" table's Top DICT has no CIDFont operators).
    '*/
    <PDF(VersionEnum.PDF10)>
    Public Class Type1Font
        Inherits SimpleFont

#Region "dynamic"
#Region "fields"

        Protected _metrics As AfmParser.FontMetrics

#End Region

#Region "constructors"

        Friend Sub New(ByVal context As Document)
            MyBase.New(context)
        End Sub

        Friend Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "Interface"
#Region "Protected"

        Protected Overridable Function GetNativeEncoding() As IDictionary(Of ByteArray, Integer)
            Dim Descriptor As PdfDictionary = Me.Descriptor
            If (Descriptor.ContainsKey(PdfName.FontFile)) Then ' Embedded Then noncompact Type 1 font.
                Dim fontFileStream As PdfStream = CType(Descriptor.Resolve(PdfName.FontFile), PdfStream)
                Dim parser As PfbParser = New PfbParser(fontFileStream.Body)
                Return parser.Parse()
            ElseIf (Descriptor.ContainsKey(PdfName.FontFile3)) Then ' Embedded Then compact Type 1 font.
                Dim fontFileStream As PdfStream = CType(Descriptor.Resolve(PdfName.FontFile3), PdfStream)
                Dim fontFileSubtype As PdfName = CType(fontFileStream.Header(PdfName.Subtype), PdfName)
                If (fontFileSubtype.Equals(PdfName.Type1C)) Then ' CFF.
                    Dim parser As CffParser = New CffParser(fontFileStream.Body)
                    Dim codes As IDictionary(Of ByteArray, Integer) = New Dictionary(Of ByteArray, Integer)()
                    For Each glyphIndexEntry As KeyValuePair(Of Integer, Integer) In parser.glyphIndexes
                        '/*
                        '  FIXME: Custom(non - unicode) encodings require name handling To match encoding
                        '  differences; Me method (getNativeEncoding) should therefore Return a glyphindex-To-
                        '  character-Name map instead.
                        '  Constraining native codes into target byte-arrayed encodings Is wrong -- that should
                        '  be only the final stage.
                        ' */
                        codes(New ByteArray(New Byte() {ConvertUtils.IntToByteArray(glyphIndexEntry.Value)(3)})) = glyphIndexEntry.Key
                    Next
                    Return codes
                ElseIf (fontFileSubtype.Equals(PdfName.OpenType)) Then ' OpenFont / CFF.
                    Throw New NotImplementedException("Embedded OpenFont/CFF font file.")
                Else
                    Throw New NotSupportedException("Unsupported embedded font file format: " & fontFileSubtype.ToString)
                End If
            Else ' Non-embedded font.
                Return Encoding.Get(PdfName.StandardEncoding).GetCodes()
            End If
        End Function

        Protected Overrides Sub LoadEncoding()
            'TODO: set symbolic = true/false; depending on the actual encoding!!!
            ' Encoding.
            If (Me._codes Is Nothing) Then
                Dim codes As IDictionary(Of ByteArray, Integer)
                Dim encodingObject As PdfDataObject = Me.BaseDataObject.Resolve(PdfName.Encoding)
                If (encodingObject Is Nothing) Then ' Native Then Encoding.
                    codes = GetNativeEncoding()
                ElseIf (TypeOf (encodingObject) Is PdfName) Then ' Predefined Then encoding.
                    codes = Encoding.Get(CType(encodingObject, PdfName)).GetCodes()
                Else ' Custom encoding.
                    Dim encodingDictionary As PdfDictionary = CType(encodingObject, PdfDictionary)
                    ' 1. Base encoding.
                    Dim baseEncodingName As PdfName = CType(encodingDictionary(PdfName.BaseEncoding), PdfName)
                    If (baseEncodingName Is Nothing) Then ' Native Then base Encoding.
                        codes = GetNativeEncoding()
                    Else ' Predefined base encoding.
                        codes = Encoding.Get(baseEncodingName).GetCodes()
                    End If

                    ' 2. Differences.
                    LoadEncodingDifferences(encodingDictionary, codes)
                End If
                Me._codes = New BiDictionary(Of ByteArray, Integer)(codes)
            End If

            ' Glyph indexes.
            If (_glyphIndexes Is Nothing) Then
                _glyphIndexes = New Dictionary(Of Integer, Integer)
                For Each codeEntry As KeyValuePair(Of ByteArray, Integer) In Me._codes
                    _glyphIndexes(codeEntry.Value) = ConvertUtils.ByteArrayToInt(codeEntry.Key.Data)
                Next
            End If
        End Sub

#End Region
#End Region
#End Region

    End Class

End Namespace
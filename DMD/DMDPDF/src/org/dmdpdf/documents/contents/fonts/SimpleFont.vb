'/*
'  Copyright 2009-2011 Stefano Chizzolini. http://www.dmdpdf.org

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

Imports DMD.org.dmdpdf.objects
Imports DMD.org.dmdpdf.util

Imports System
Imports System.Collections.Generic

Namespace DMD.org.dmdpdf.documents.contents.fonts

    '/**
    '  <summary>Simple font [PDF:1.6:5.5].</summary>
    '*/
    <PDF(VersionEnum.PDF10)>
    Public MustInherit Class SimpleFont
        Inherits Font

#Region "constructors"

        Protected Sub New(ByVal context As Document)
            MyBase.new(context)
        End Sub

        Protected Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.new(baseObject)
        End Sub

#End Region

#Region "Interface"
#Region "Protected"

        Protected Overrides ReadOnly Property Descriptor As PdfDictionary
            Get
                Return CType(BaseDataObject.Resolve(PdfName.FontDescriptor), PdfDictionary)
            End Get
        End Property

        Protected MustOverride Sub LoadEncoding()

        '/**
        '  <summary>Loads the encoding differences into the given collection.</summary>
        '  <param name="encodingDictionary">Encoding dictionary.</param>
        '  <param name="codes">Encoding to alter applying differences.</param>
        '*/
        Protected Sub LoadEncodingDifferences(ByVal encodingDictionary As PdfDictionary, ByVal codes As IDictionary(Of ByteArray, Integer))
            Dim differenceObjects As PdfArray = CType(encodingDictionary.Resolve(PdfName.Differences), PdfArray)
            If (differenceObjects Is Nothing) Then Return
            '/*
            '  NOTE: Each code is the first index in a sequence of character codes to be changed.
            '  The first character name after the code becomes the name corresponding to that code.
            '  Subsequent names replace consecutive code indices until the next code appears
            '  in the array or the array ends.
            '*/
            Dim charCodeData As Byte() = New Byte(1 - 1) {}
            For Each differenceObject As PdfDirectObject In differenceObjects
                If (TypeOf (differenceObject) Is PdfInteger) Then
                    charCodeData(0) = CByte(CInt(CType(differenceObject, PdfInteger).Value) And &HFF)
                Else ' NOTE: MUST be PdfName.
                    Dim charCode As ByteArray = New ByteArray(charCodeData)
                    Dim charName As String = CStr(CType(differenceObject, PdfName).Value)
                    If (charName.Equals(".notdef")) Then
                        codes.Remove(charCode)
                    Else
                        Dim code As Integer? = GlyphMapping.NameToCode(charName)
                        If (code.HasValue) Then
                            codes(charCode) = code.Value
                        Else
                            codes(charCode) = charCodeData(0)
                        End If
                    End If
                    If (charCodeData(0) < 255) Then
                        charCodeData(0) += CByte(1)
                    Else
                        charCodeData(0) = 0
                    End If

                End If
                System.Diagnostics.Debug.Print(differenceObject.ToString() & " " & charCodeData(0) & vbCrLf)
            Next

            System.Diagnostics.Debug.Print("Count: " & differenceObjects.Count)
        End Sub

        Protected Overrides Sub OnLoad()
            LoadEncoding()

            ' Glyph widths.
            If (Me._glyphWidths Is Nothing) Then
                Me._glyphWidths = New Dictionary(Of Integer, Integer)
                Dim glyphWidthObjects As PdfArray = CType(BaseDataObject.Resolve(PdfName.Widths), PdfArray)
                If (glyphWidthObjects IsNot Nothing) Then
                    Dim charCode As ByteArray = New ByteArray(
                                                        New Byte() {
                                                                CByte(CType(BaseDataObject(PdfName.FirstChar), PdfInteger).IntValue)
                                                            }
                                                        )
                    For Each glyphWidthObject As PdfDirectObject In glyphWidthObjects
                        Dim glyphWidth As Integer = CType(glyphWidthObject, IPdfNumber).IntValue
                        If (glyphWidth > 0) Then
                            Dim code As Integer = 0
                            If (Me._codes.TryGetValue(charCode, code)) Then
                                Me._glyphWidths(Me._glyphIndexes(code)) = glyphWidth
                            End If
                        End If
                        charCode.Data(0) += CByte(1)
                    Next
                End If
            End If
            ' Default glyph width.
            '{
            Dim descriptor As PdfDictionary = Me.Descriptor
            If (descriptor IsNot Nothing) Then
                Dim defaultGlyphWidthObject As IPdfNumber = CType(descriptor(PdfName.MissingWidth), IPdfNumber)
                If (defaultGlyphWidthObject IsNot Nothing) Then
                    Me._defaultGlyphWidth = defaultGlyphWidthObject.IntValue
                Else
                    Me._defaultGlyphWidth = 0
                End If
            End If
            '}
        End Sub

#End Region
#End Region

    End Class

End Namespace

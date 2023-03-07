'/*
'  Copyright 2006-2012 Stefano Chizzolini. http://www.dmdpdf.org

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
Imports System.Collections.Generic
Imports System.IO
Imports System.Reflection

Namespace DMD.org.dmdpdf.documents.contents.fonts

    '/**
    '  <summary>Standard Type 1 font [PDF:1.6:5.5.1].</summary>
    '*/
    <PDF(VersionEnum.PDF10)>
    Public NotInheritable Class StandardType1Font
        Inherits Type1Font

#Region "types"
        '/**
        '  <summary>Standard Type 1 font families [PDF:1.6:5.5.1].</summary>
        '*/
        Public Enum FamilyEnum
            Courier
            Helvetica
            Times
            Symbol
            ZapfDingbats
        End Enum

#End Region

#Region "Static"
#Region "Interface"
#Region "Private"

        Private Shared Function IsSymbolic(ByVal value As FamilyEnum) As Boolean
            Select Case (value)
                Case FamilyEnum.Courier, FamilyEnum.Helvetica, FamilyEnum.Times : Return False
                Case FamilyEnum.Symbol, FamilyEnum.ZapfDingbats : Return True
                Case Else : Throw New NotImplementedException()
            End Select
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        Public Sub New(ByVal context As Document, ByVal family As FamilyEnum, ByVal bold As Boolean, ByVal italic As Boolean)
            MyBase.New(context)
            Dim fontName As String = family.ToString()
            Select Case (family)
                Case FamilyEnum.Symbol, FamilyEnum.ZapfDingbats ' break;
                Case FamilyEnum.Times
                    If (bold) Then
                        fontName &= "-Bold"
                        If (italic) Then
                            fontName &= "Italic"
                        End If
                    ElseIf (italic) Then
                        fontName &= "-Italic"
                    Else
                        fontName &= "-Roman"
                    End If
                    'break;
                Case Else
                    If (bold) Then
                        fontName &= "-Bold"
                        If (italic) Then
                            fontName &= "Oblique"
                        End If
                    ElseIf (italic) Then
                        fontName &= "-Oblique"
                    End If
                    'break;
            End Select
            Dim encodingName As PdfName
            If (IsSymbolic(family)) Then
                encodingName = Nothing
            Else
                encodingName = PdfName.WinAnsiEncoding
            End If

            Create(fontName, encodingName)
        End Sub

        Friend Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public Overrides ReadOnly Property Ascent As Double
            Get
                Return Me._metrics.Ascender
            End Get
        End Property

        Public Overrides ReadOnly Property Descent As Double
            Get
                Return Me._metrics.Descender
            End Get
        End Property

        Public Overrides ReadOnly Property Flags As FlagsEnum
            Get
                'TODO:IMPL!!!
                Return 0
            End Get
        End Property

#End Region

#Region "Protected"

        Protected Overrides Function GetNativeEncoding() As IDictionary(Of ByteArray, Integer)
            If (_symbolic) Then ' Symbolic Then font.
                Dim codes As Dictionary(Of ByteArray, Integer) = New Dictionary(Of ByteArray, Integer)()
                For Each glyphIndexEntry As KeyValuePair(Of Integer, Integer) In _glyphIndexes
                    codes(New ByteArray(New Byte() {ConvertUtils.IntToByteArray(glyphIndexEntry.Value)(3)})) = glyphIndexEntry.Key
                Next
                Return codes
            Else ' Nonsymbolic font.
                Return Encoding.Get(PdfName.StandardEncoding).GetCodes()
            End If
        End Function

        Protected Overrides Sub OnLoad()
            '/*
            '  NOTE: Standard Type 1 fonts ordinarily omit their descriptor;
            '  otherwise, when overridden they degrade to a common Type 1 font.
            '  Metrics of non-overridden Standard Type 1 fonts MUST be loaded from resources.
            '*/
            Load(CType(BaseDataObject(PdfName.BaseFont), PdfName).StringValue)
            MyBase.OnLoad()
        End Sub

#End Region

#Region "private"
        '/**
        '  <summary> Creates the font structures.</summary>
        '*/
        Private Sub Create(ByVal fontName As String, ByVal encodingName As PdfName)
            '/* NOTE: Standard Type 1 fonts SHOULD omit extended font descriptions [PDF:1.6:5.5.1]. */
            '// Subtype.
            BaseDataObject(PdfName.Subtype) = PdfName.Type1
            ' BaseFont.
            BaseDataObject(PdfName.BaseFont) = New PdfName(fontName)
            ' Encoding.
            If (encodingName IsNot Nothing) Then
                BaseDataObject(PdfName.Encoding) = encodingName
            End If

            Load()
        End Sub

        '/**
        '  <summary> Loads the font metrics.</summary>
        '*/
        Private Overloads Sub Load(ByVal fontName As String)
            Dim fontMetricsStream As System.IO.Stream = Nothing
#If Not DEBUG Then
            Try
#End If
            'fontMetricsStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("fonts.afm." & fontName)
            fontMetricsStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(fontName & ".afm")
            Dim parser As AfmParser = New AfmParser(New bytes.Stream(fontMetricsStream))
            _metrics = parser.Metrics
            _symbolic = _metrics.IsCustomEncoding
            _glyphIndexes = parser.GlyphIndexes
            _glyphKernings = parser.GlyphKernings
            _glyphWidths = parser.GlyphWidths
#If Not DEBUG Then
            Catch e As System.Exception
                Throw New Exception("Failed to load '" & fontName & "'.", e)
            Finally
#End If
            If (fontMetricsStream IsNot Nothing) Then fontMetricsStream.Close() : fontMetricsStream = Nothing
#If Not DEBUG Then
            End Try
#End If
        End Sub


#End Region
#End Region
#End Region

    End Class

End Namespace
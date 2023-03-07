'/*
'  Copyright 2010-2012 Stefano Chizzolini. http://www.dmdpdf.org

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

Imports DMD.org.dmdpdf.objects
Imports DMD.org.dmdpdf.util

Imports System
Imports System.Collections.Generic

Namespace DMD.org.dmdpdf.documents.contents.fonts

    '/**
    '  <summary>Type 3 font [PDF:1.6:5.5.4].</summary>
    '*/
    <PDF(VersionEnum.PDF10)>
    Public NotInheritable Class Type3Font
        Inherits SimpleFont

#Region "dynamic"
#Region "constructors"

        Friend Sub New(ByVal context As Document)
            MyBase.new(context)
        End Sub

        Friend Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.new(baseObject)
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public Overrides ReadOnly Property Ascent As Double
            Get
                Return 0
            End Get
        End Property

        Public Overrides ReadOnly Property Descent As Double
            Get
                Return 0
            End Get
        End Property

#End Region

#Region "Protected"

        Protected Overrides Sub LoadEncoding()
            'FIXME: consolidate with Type1Font And TrueTypeFont!
            ' Encoding.
            If (Me._codes Is Nothing) Then
                Dim codes As IDictionary(Of ByteArray, Integer)
                Dim encodingObject As PdfDataObject = BaseDataObject.Resolve(PdfName.Encoding)
                If (encodingObject Is Nothing) Then ' Native Then Encoding.
                    codes = GetNativeEncoding()
                ElseIf (TypeOf (encodingObject) Is PdfName) Then ' Predefined Then encoding.
                    codes = Encoding.Get(CType(encodingObject, PdfName)).GetCodes()
                Else ' Custom encoding.
                    Dim encodingDictionary As PdfDictionary = CType(encodingObject, PdfDictionary)
                    ' 1. Base encoding.
                    Dim baseEncodingName As PdfName = CType(encodingDictionary(PdfName.BaseEncoding), PdfName)
                    If (baseEncodingName Is Nothing) Then ' Native Then base encoding.
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
            If (Me._glyphIndexes Is Nothing) Then
                Me._glyphIndexes = New Dictionary(Of Integer, Integer)
                For Each codeEntry As KeyValuePair(Of ByteArray, Integer) In Me._codes
                    Me._glyphIndexes(codeEntry.Value) = ConvertUtils.ByteArrayToInt(codeEntry.Key.Data)
                Next
            End If
        End Sub

#End Region

#Region "Private"

        Private Function GetNativeEncoding() As IDictionary(Of ByteArray, Integer)
            'FIXME: consolidate with Type1Font And TrueTypeFont!
            Return Encoding.Get(PdfName.StandardEncoding).GetCodes()
        End Function

#End Region
#End Region
#End Region

    End Class

End Namespace
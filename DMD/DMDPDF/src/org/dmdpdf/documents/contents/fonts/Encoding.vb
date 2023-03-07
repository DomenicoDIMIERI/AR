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
    '  <summary>Predefined encodings [PDF:1.6:5.5.5,D].</summary>
    '*/
    '// TODO: This hierarchy is going to be superseded by org.dmdpdf.tokens.Encoding.
    Friend Class Encoding

#Region "Static"
#Region "fields"

        Private Shared ReadOnly Encodings As Dictionary(Of PdfName, Encoding) = New Dictionary(Of PdfName, Encoding)

#End Region

#Region "constructors"

        Shared Sub New()
            'TODO:this collection MUST be automatically populated looking for Encoding subclasses!
            Encodings(PdfName.StandardEncoding) = New StandardEncoding()
            Encodings(PdfName.MacRomanEncoding) = New MacRomanEncoding()
            Encodings(PdfName.WinAnsiEncoding) = New WinAnsiEncoding()
        End Sub

#End Region

#Region "Interface"

        Public Shared Function [Get](ByVal name As PdfName) As Encoding
            Return Encodings(name)
        End Function

#End Region
#End Region

#Region "dynamic"
#Region "fields"

        Private ReadOnly codes As Dictionary(Of ByteArray, Integer) = New Dictionary(Of ByteArray, Integer)()

#End Region

#Region "Interface"
#Region "Public"

        Public Function GetCodes() As Dictionary(Of ByteArray, Integer)
            Return New Dictionary(Of ByteArray, Integer)(codes)
        End Function

#End Region

#Region "Protected"

        Protected Sub Put(ByVal charCode As Integer, ByVal charName As String)
            codes(New ByteArray(New Byte() {CByte(charCode)})) = GlyphMapping.NameToCode(charName).Value
        End Sub

#End Region
#End Region
#End Region

    End Class
End Namespace
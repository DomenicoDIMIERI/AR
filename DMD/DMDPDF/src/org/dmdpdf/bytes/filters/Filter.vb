'/*
'  Copyright 2006-2010 Stefano Chizzolini. http://www.dmdpdf.org

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

Imports DMD.org.dmdpdf
Imports DMD.org.dmdpdf.objects

Imports System

Namespace DMD.org.dmdpdf.bytes.filters

    '/**
    '  <summary>Abstract filter [PDF:1.6:3.3].</summary>
    '*/
    <PDF(VersionEnum.PDF10)>
    Public MustInherit Class Filter

#Region "Static"

#Region "fields"

        Private Shared ReadOnly ASCII85Filter As Filter = New ASCII85Filter()
        Private Shared ReadOnly FlateDecode As Filter = New FlateFilter()

#End Region


#Region "interface"

#Region "public"

        '/**
        '  <summary>Gets a specific filter object.</summary>
        '  <param name="name">Name of the requested filter.</param>
        '  <returns>Filter object associated to the name.</returns>
        '*/
        Public Shared Function [Get](ByVal name As PdfName) As Filter
            '/*
            '  NOTE: This is a factory singleton method for any filter-derived object.
            '*/
            If (name Is Nothing) Then Return Nothing

            If (name.Equals(PdfName.FlateDecode) OrElse name.Equals(PdfName.Fl)) Then
                Return FlateDecode
            ElseIf (name.Equals(PdfName.LZWDecode) OrElse name.Equals(PdfName.LZW)) Then
                Throw New NotImplementedException("LZWDecode")
            ElseIf (name.Equals(PdfName.ASCIIHexDecode) OrElse name.Equals(PdfName.AHx)) Then
                Throw New NotImplementedException("ASCIIHexDecode")
            ElseIf (name.Equals(PdfName.ASCII85Decode) OrElse name.Equals(PdfName.A85)) Then
                Return ASCII85Filter
            ElseIf (name.Equals(PdfName.RunLengthDecode) OrElse name.Equals(PdfName.RL)) Then
                Throw New NotImplementedException("RunLengthDecode")
            ElseIf (name.Equals(PdfName.CCITTFaxDecode) OrElse name.Equals(PdfName.CCF)) Then
                Throw New NotImplementedException("CCITTFaxDecode")
            ElseIf (name.Equals(PdfName.JBIG2Decode)) Then
                Throw New NotImplementedException("JBIG2Decode")
            ElseIf (name.Equals(PdfName.DCTDecode) OrElse name.Equals(PdfName.DCT)) Then
                Throw New NotImplementedException("DCTDecode")
            ElseIf (name.Equals(PdfName.JPXDecode)) Then
                Throw New NotImplementedException("JPXDecode")
            ElseIf (name.Equals(PdfName.Crypt)) Then
                Throw New NotImplementedException("Crypt")
            End If
            Return Nothing
        End Function

#End Region

#End Region

#End Region

#Region "dynamic"

#Region "constructors"

        Protected Sub New()

        End Sub

#End Region

#Region "Interface"

#Region "Public"

        Public MustOverride Function Decode(ByVal data As Byte(), ByVal offset As Integer, ByVal length As Integer, ByVal parameters As PdfDictionary) As Byte()

        Public MustOverride Function Encode(ByVal data As Byte(), ByVal offset As Integer, ByVal length As Integer, ByVal parameters As PdfDictionary) As Byte()

#End Region

#End Region

#End Region

    End Class

End Namespace

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

Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.objects

Namespace DMD.org.dmdpdf.documents.contents.colorSpaces

    '/**
    '  <summary>Abstract CIE-based color space [PDF:1.6:4.5.4].</summary>
    '*/
    <PDF(VersionEnum.PDF11)>
    Public MustInherit Class CIEBasedColorSpace
        Inherits ColorSpace

#Region "dynamic"
#Region "constructors"
        'TODO:IMPL new element constructor!

        Protected Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "interface"
#Region "public"
        '/**
        '  <summary>Gets the tristimulus value, in the CIE 1931 XYZ space, of the diffuse black point.</summary>
        '*/
        Public ReadOnly Property BlackPoint As Double()
            Get
                Dim blackPointObject As PdfArray = CType(Dictionary(PdfName.BlackPoint), PdfArray)
                If (blackPointObject Is Nothing) Then
                    Return New Double() {0, 0, 0}
                Else
                    Return New Double() {
                        CType(blackPointObject(0), IPdfNumber).RawValue,
                        CType(blackPointObject(1), IPdfNumber).RawValue,
                        CType(blackPointObject(2), IPdfNumber).RawValue
                            }
                End If
            End Get
        End Property

        '/**
        '  <summary> Gets the tristimulus value, In the CIE 1931 XYZ space, Of the diffuse white point.</summary>
        '*/
        Public ReadOnly Property WhitePoint As Double()
            Get
                Dim whitePointObject As PdfArray = CType(Dictionary(PdfName.WhitePoint), PdfArray)
                Return New Double() {
                        CType(whitePointObject(0), IPdfNumber).RawValue,
                        CType(whitePointObject(1), IPdfNumber).RawValue,
                        CType(whitePointObject(2), IPdfNumber).RawValue
                      }
            End Get
        End Property

#End Region

#Region "protected"
        '/**
        '  <summary> Gets this color space's dictionary.</summary>
        '*/
        Protected ReadOnly Property Dictionary As PdfDictionary
            Get
                Return CType(CType(BaseDataObject, PdfArray)(1), PdfDictionary)
            End Get
        End Property

#End Region
#End Region
#End Region

    End Class

End Namespace
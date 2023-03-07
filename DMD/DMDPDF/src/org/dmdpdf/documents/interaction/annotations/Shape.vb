'/*
'  Copyright 2008-2012 Stefano Chizzolini. http://www.dmdpdf.org

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
Imports DMD.org.dmdpdf.documents.contents.colorSpaces
Imports DMD.org.dmdpdf.objects

Imports System
Imports System.Drawing

Namespace DMD.org.dmdpdf.documents.interaction.annotations

    '/**
    '  <summary>Abstract shape annotation.</summary>
    '*/
    <PDF(VersionEnum.PDF13)>
    Public MustInherit Class Shape
        Inherits Annotation

#Region "dynamic"
#Region "constructors"

        Protected Sub New(ByVal page As Page, ByVal box As RectangleF, ByVal text As String, ByVal subtype As PdfName)
            MyBase.New(page, subtype, box, text)
        End Sub

        Protected Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary>Gets/Sets the color with which to fill the interior of the annotation's shape.</summary>
        '*/
        Public Property FillColor As DeviceRGBColor
            Get
                Dim fillColorObject As PdfArray = CType(BaseDataObject(PdfName.IC), PdfArray)
                'TODO:use baseObject constructor!!!
                If (fillColorObject IsNot Nothing) Then
                    Return New DeviceRGBColor(
                                CType(fillColorObject(0), IPdfNumber).RawValue,
                                CType(fillColorObject(1), IPdfNumber).RawValue,
                                CType(fillColorObject(2), IPdfNumber).RawValue
                            )
                Else
                    Return Nothing
                End If
            End Get
            Set(ByVal value As DeviceRGBColor)
                BaseDataObject(PdfName.IC) = CType(value.BaseDataObject, PdfDirectObject)
            End Set
        End Property

#End Region
#End Region
#End Region

    End Class

End Namespace
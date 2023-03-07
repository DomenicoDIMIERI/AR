'/*
'  Copyright 2012 Stefano Chizzolini. http://www.dmdpdf.org

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
Imports DMD.org.dmdpdf.documents.contents.colorSpaces
Imports DMD.org.dmdpdf.documents.interaction
'Imports DMD.actions = org.dmdpdf.documents.interaction.actions
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects

Imports System

Namespace DMD.org.dmdpdf.documents.multimedia

    '/**
    '  <summary>Media clip object [PDF:1.7:9.1.3].</summary>
    '*/
    <PDF(VersionEnum.PDF15)>
    Public MustInherit Class MediaClip
        Inherits PdfObjectWrapper(Of PdfDictionary)

#Region "static"
#Region "interface"
#Region "public"

        '/**
        '  <summary>Wraps a clip base object into a clip object.</summary>
        '*/
        Public Shared Function Wrap(ByVal baseObject As PdfDirectObject) As MediaClip
            If (baseObject Is Nothing) Then Return Nothing
            Dim subtype As PdfName = CType(CType(baseObject.Resolve(), PdfDictionary)(PdfName.S), PdfName)
            If (PdfName.MCD.Equals(subtype)) Then
                Return New MediaClipData(baseObject)
            ElseIf (PdfName.MCS.Equals(subtype)) Then
                Return New MediaClipSection(baseObject)
            Else
                Throw New ArgumentException("It doesn't represent a valid clip object.", "baseObject")
            End If
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        Protected Sub New(ByVal context As Document, ByVal subtype As PdfName)
            MyBase.New(context, New PdfDictionary(New PdfName() {PdfName.Type, PdfName.S}, New PdfDirectObject() {PdfName.MediaClip, subtype}))
        End Sub

        Protected Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary>Gets/Sets the actual media data.</summary>
        '  <returns>Either a <see cref="FullFileSpecification"/> or a <see cref="FormXObject"/>.</returns>
        '*/
        Public MustOverride Property Data As PdfObjectWrapper

#End Region
#End Region
#End Region

    End Class

End Namespace
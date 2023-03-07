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

Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.documents.contents.layers
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects

Imports System
Imports System.Drawing
Imports System.Drawing.Drawing2D

Namespace DMD.org.dmdpdf.documents.contents.xObjects

    '/**
    '  <summary>External graphics object whose contents are defined by a self-contained content stream,
    '  separate from the content stream in which it is used [PDF:1.6:4.7].</summary>
    '*/
    <PDF(VersionEnum.PDF10)>
    Public MustInherit Class XObject
        Inherits PdfObjectWrapper(Of PdfStream)
        Implements ILayerable

#Region "static"
#Region "interface"
#Region "public"

        '/**
        '  <summary>Wraps an external object reference into an external object.</summary>
        '  <param name="baseObject">External object base object.</param>
        '  <returns>External object associated to the reference.</returns>
        '*/
        Public Shared Function Wrap(ByVal baseObject As PdfDirectObject) As XObject
            If (baseObject Is Nothing) Then Return Nothing
            Dim subtype As PdfName = CType(CType(baseObject.Resolve(), PdfStream).Header(PdfName.Subtype), PdfName)
            If (subtype.Equals(PdfName.Form)) Then
                Return FormXObject.Wrap(baseObject)
            ElseIf (subtype.Equals(PdfName.Image)) Then
                Return ImageXObject.Wrap(baseObject)
            Else
                Return Nothing
            End If
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        '/**
        '  <summary>Creates a new external object inside the document.</summary>
        '*/
        Protected Sub New(ByVal context As Document)
            Me.New(context, New PdfStream())
        End Sub

        '/**
        '  <summary>Creates a new external object inside the document.</summary>
        '*/
        Protected Sub New(ByVal context As Document, ByVal baseDataObject As PdfStream)
            MyBase.New(context, baseDataObject)
            baseDataObject.Header(PdfName.Type) = PdfName.XObject
        End Sub

        '/**
        '  <summary>Instantiates an existing external object.</summary>
        '*/
        Protected Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary>Gets/Sets the mapping from external-object space to user space.</summary>
        '*/
        Public MustOverride Property Matrix As Matrix

        '/**
        '  <summary>Gets/Sets the external object size.</summary>
        '*/
        Public MustOverride Property Size As SizeF


#Region "ILayerable"

        Public Property Layer As LayerEntity Implements ILayerable.Layer
            Get
                Return CType(PropertyList.Wrap(BaseDataObject.Header(PdfName.OC)), LayerEntity)
            End Get
            Set(ByVal value As LayerEntity)
                BaseDataObject.Header(PdfName.OC) = value.BaseObject
            End Set
        End Property

#End Region
#End Region
#End Region
#End Region

    End Class

End Namespace
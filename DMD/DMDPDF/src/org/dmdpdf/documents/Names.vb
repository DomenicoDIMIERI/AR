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

'  You should have received a copy of the GNU Lesser General Public License along with this
'  Program (see README files); if not, go to the GNU website (http://www.gnu.org/licenses/).

'  Redistribution and use, with or without modification, are permitted provided that such
'  redistributions retain the above copyright notice, license and disclaimer, along with
'  this list of conditions.
'*/

Imports DMD.org.dmdpdf.documents.files
Imports DMD.org.dmdpdf.documents.interaction.actions
Imports DMD.org.dmdpdf.documents.interaction.navigation.document
Imports DMD.org.dmdpdf.documents.multimedia
Imports DMD.org.dmdpdf.objects

Imports System
Imports System.Collections.Generic
Imports System.Reflection

Namespace DMD.org.dmdpdf.documents

    '/**
    '  <summary>Name dictionary [PDF:1.6:3.6.3].</summary>
    '*/
    <PDF(VersionEnum.PDF12)>
    Public NotInheritable Class Names
        Inherits PdfObjectWrapper(Of PdfDictionary)
        Implements ICompositeDictionary(Of PdfString)

#Region "dynamic"
#Region "constructors"

        Public Sub New(ByVal context As Document)
            MyBase.New(context, New PdfDictionary())
        End Sub

        Friend Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "interface"
#Region "public"
        '/**
        '  <summary>Gets/Sets the named destinations.</summary>
        '*/
        <PDF(VersionEnum.PDF12)>
        Public Property Destinations As NamedDestinations
            Get
                Return New NamedDestinations(BaseDataObject.Get(Of PdfDictionary)(PdfName.Dests, False))
            End Get
            Set(ByVal value As NamedDestinations)
                BaseDataObject(PdfName.Dests) = value.BaseObject
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets the named embedded files.</summary>
        '*/
        <PDF(VersionEnum.PDF14)>
        Public Property EmbeddedFiles As NamedEmbeddedFiles
            Get
                Return New NamedEmbeddedFiles(BaseDataObject.Get(Of PdfDictionary)(PdfName.EmbeddedFiles, False))
            End Get
            Set(ByVal value As NamedEmbeddedFiles)
                BaseDataObject(PdfName.EmbeddedFiles) = value.BaseObject
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets the named JavaScript actions.</summary>
        '*/
        <PDF(VersionEnum.PDF13)>
        Public Property JavaScripts As NamedJavaScripts
            Get
                Return New NamedJavaScripts(BaseDataObject.Get(Of PdfDictionary)(PdfName.JavaScript, False))
            End Get
            Set(ByVal value As NamedJavaScripts)
                BaseDataObject(PdfName.JavaScript) = value.BaseObject
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets the named renditions.</summary>
        '*/
        <PDF(VersionEnum.PDF15)>
        Public Property Renditions As NamedRenditions
            Get
                Return New NamedRenditions(BaseDataObject.Get(Of PdfDictionary)(PdfName.Renditions, False))
            End Get
            Set(ByVal value As NamedRenditions)
                BaseDataObject(PdfName.Renditions) = value.BaseObject
            End Set
        End Property

#Region "ICompositeDictionary"

        Public Function [Get](ByVal type As Type) As PdfObjectWrapper Implements ICompositeDictionary(Of PdfString).Get
            If (GetType(Destination).IsAssignableFrom(type)) Then
                Return Destinations
            ElseIf (GetType(FileSpecification).IsAssignableFrom(type)) Then
                Return EmbeddedFiles
            ElseIf (GetType(JavaScript).IsAssignableFrom(type)) Then
                Return JavaScripts
            ElseIf (GetType(Rendition).IsAssignableFrom(type)) Then
                Return Renditions
            Else
                Return Nothing
            End If
        End Function

        Public Function [Get](ByVal type As Type, ByVal key As PdfString) As PdfObjectWrapper Implements ICompositeDictionary(Of PdfString).Get
            Return CType(type.GetMethod("get_Item", BindingFlags.Public Or BindingFlags.Instance).Invoke([Get](type), New Object() {key}), PdfObjectWrapper)
        End Function

#End Region
#End Region
#End Region
#End Region
    End Class

End Namespace


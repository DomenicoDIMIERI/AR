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
Imports DMD.org.dmdpdf.documents.interaction.actions
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects

Imports System

Namespace DMD.org.dmdpdf.documents.multimedia

    '/**
    '  <summary>Selector rendition [PDF:1.7:9.1.2].</summary>
    '*/
    <PDF(VersionEnum.PDF15)>
    Public NotInheritable Class SelectorRendition
        Inherits Rendition

#Region "Static"
#Region "types"
        Private Class ArrayWrapperObject
            Implements org.dmdpdf.objects.Array(Of Rendition).IWrapper(Of Rendition)

            Public Function Wrap(baseObject As PdfDirectObject) As Rendition Implements Array(Of Rendition).IWrapper(Of Rendition).Wrap
                Return Rendition.Wrap(baseObject)
            End Function

        End Class

#End Region

#Region "fields"

        Private Shared ReadOnly ArrayWrapper As org.dmdpdf.objects.Array(Of Rendition).IWrapper(Of Rendition) = New ArrayWrapperObject()

#End Region
#End Region

#Region "dynamic"
#Region "constructors"
        Public Sub New(ByVal context As Document)
            MyBase.New(context, PdfName.SR)
        End Sub

        Friend Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.new(baseObject)
        End Sub

#End Region

#Region "interface"
#Region "public"
        '/**
        '  <summary> Gets/Sets an ordered collection Of renditions. The first viable media rendition found
        '  in the array, Or nested within a selector rendition in the array, should be used.</summary>
        '*/
        Public Property Renditions As Array(Of Rendition)
            Get
                Return Array(Of Rendition).Wrap(Of Rendition)(ArrayWrapper, BaseDataObject.Get(Of PdfArray)(PdfName.R))
            End Get
            Set(ByVal value As Array(Of Rendition))
                Me.BaseDataObject(PdfName.R) = value.BaseObject
            End Set
        End Property

#End Region
#End Region
#End Region

    End Class

End Namespace


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
Imports DMD.org.dmdpdf.documents.interaction.annotations
Imports DMD.org.dmdpdf.objects

Imports System

Namespace DMD.org.dmdpdf.documents.interaction.actions

    '/**
    '  <summary>'Play a movie' action [PDF:1.6:8.5.3].</summary>
    '*/
    <PDF(VersionEnum.PDF12)>
    Public NotInheritable Class PlayMovie
        Inherits Action

#Region "dynamic"
#Region "constructors"
        '/**
        '  <summary>Creates a new action within the given document context.</summary>
        '*/
        Public Sub New(ByVal context As Document, ByVal movie As Movie)
            MyBase.New(context, PdfName.Movie)
            Me.Movie = Movie
        End Sub

        Friend Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary>Gets/Sets the movie to be played.</summary>
        '*/
        Public Property Movie As Movie
            Get
                Dim annotationObject As PdfDirectObject = BaseDataObject(PdfName.Annotation)
                If (annotationObject Is Nothing) Then
                    annotationObject = BaseDataObject(PdfName.T)
                    Throw New NotImplementedException("No by-title movie annotation support currently: we have to implement a hook to the page of the referenced movie to get it from its annotations collection.")
                End If
                Return CType(Annotation.Wrap(annotationObject), Movie)
            End Get
            Set(ByVal value As Movie)
                If (value Is Nothing) Then Throw New ArgumentException("Movie MUST be defined.")
                BaseDataObject(PdfName.Annotation) = value.BaseObject
            End Set
        End Property

#End Region
#End Region
#End Region

    End Class
End Namespace
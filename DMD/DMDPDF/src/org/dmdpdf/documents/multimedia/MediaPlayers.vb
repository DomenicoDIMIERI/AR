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
'imports  DMD.actions = org.dmdpdf.documents.interaction.actions;
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects

Imports System

Namespace DMD.org.dmdpdf.documents.multimedia

    '/**
    '  <summary>Media player rules [PDF:1.7:9.1.6].</summary>
    '*/
    <PDF(VersionEnum.PDF15)>
    Public NotInheritable Class MediaPlayers
        Inherits PdfObjectWrapper(Of PdfDictionary)

#Region "Static"
#Region "Interface"
#Region "Public"

        Public Shared Function Wrap(ByVal baseObject As PdfDirectObject) As MediaPlayers
            If (baseObject IsNot Nothing) Then
                Return New MediaPlayers(baseObject)
            Else
                Return Nothing
            End If
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        Public Sub New(ByVal context As Document)
            MyBase.New(context, New PdfDictionary())
        End Sub

        Private Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary>Gets/Sets a set of players, any of which may be used in playing the associated media object.
        '  </summary>
        '  <remarks>This collection is ignored if <see cref="RequiredPlayers"/> is non-empty.</remarks>
        '*/
        Public Property AllowedPlayers As Array(Of MediaPlayer)
            Get
                Return Array(Of MediaPlayer).Wrap(Of MediaPlayer)(BaseDataObject.Get(Of PdfArray)(PdfName.A))
            End Get
            Set(ByVal value As Array(Of MediaPlayer))
                BaseDataObject(PdfName.A) = PdfObjectWrapper.GetBaseObject(value)
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets a set of players that must NOT be used in playing the associated media object.
        '  </summary>
        '  <remarks>This collection takes priority over <see cref="RequiredPlayers"/>.</remarks>
        '*/
        Public Property ForbiddenPlayers As Array(Of MediaPlayer)
            Get
                Return Array(Of MediaPlayer).Wrap(Of MediaPlayer)(BaseDataObject.Get(Of PdfArray)(PdfName.NU))
            End Get
            Set(ByVal value As Array(Of MediaPlayer))
                BaseDataObject(PdfName.NU) = PdfObjectWrapper.GetBaseObject(value)
            End Set
        End Property

        '/**
        '  <summary> Gets/Sets a Set Of players, one Of which must be used In playing the associated media Object.
        '  </summary>
        '*/
        Public Property RequiredPlayers As Array(Of MediaPlayer)
            Get
                Return Array(Of MediaPlayer).Wrap(Of MediaPlayer)(BaseDataObject.Get(Of PdfArray)(PdfName.MU))
            End Get
            Set(ByVal value As Array(Of MediaPlayer))
                BaseDataObject(PdfName.MU) = PdfObjectWrapper.GetBaseObject(value)
            End Set
        End Property

#End Region
#End Region
#End Region

    End Class

End Namespace
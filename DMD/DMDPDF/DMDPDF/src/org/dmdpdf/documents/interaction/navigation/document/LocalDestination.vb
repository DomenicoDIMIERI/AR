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

Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.objects

Imports System

Namespace DMD.org.dmdpdf.documents.interaction.navigation.document

    '/**
    '  <summary>Local interaction target [PDF:1.6:8.2.1].</summary>
    '*/
    <PDF(VersionEnum.PDF10)>
    Public NotInheritable Class LocalDestination
        Inherits Destination

#Region "dynamic"
#Region "constructors"

        Public Sub New(ByVal page As documents.Page)
            Me.New(page, ModeEnum.Fit, Nothing, Nothing)
        End Sub

        Public Sub New(ByVal page As documents.Page, ByVal mode As ModeEnum, ByVal location As Object, ByVal zoom As Double?)
            MyBase.New(page.Document, page, mode, location, zoom)
        End Sub

        Friend Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary>Gets/Sets the target page.</summary>
        '*/
        Public Overrides Property Page As Object
            Get
                Return documents.Page.Wrap(BaseDataObject(0))
            End Get
            Set(ByVal value As Object)
                If (Not (TypeOf (value) Is documents.Page)) Then Throw New ArgumentException("It MUST be a Page object.")
                BaseDataObject(0) = CType(value, documents.Page).BaseObject
            End Set
        End Property

#End Region
#End Region
#End Region

    End Class

End Namespace

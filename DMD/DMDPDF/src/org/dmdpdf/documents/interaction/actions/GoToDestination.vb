'/*
'  Copyright 2010-2012 Stefano Chizzolini. http://www.dmdpdf.org

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
Imports DMD.org.dmdpdf.documents.interaction.navigation.document
Imports DMD.org.dmdpdf.objects

Imports System

Namespace DMD.org.dmdpdf.documents.interaction.actions

    '/**
    '  <summary>Abstract 'go to destination' action.</summary>
    '*/
    <PDF(VersionEnum.PDF11)>
    Public MustInherit Class GoToDestination(Of T As Destination)
        Inherits Action
        Implements IGoToAction

#Region "dynamic"
#Region "constructors"

        Protected Sub New(ByVal context As Document, ByVal actionType As PdfName, ByVal destination As T)
            MyBase.New(context, actionType)
            Me.Destination = destination
        End Sub

        Protected Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary> Gets/Sets the destination To jump To.</summary>
        '*/
        Public Property Destination As T
            Get
                Return Document.ResolveName(Of T)(BaseDataObject(PdfName.D))
            End Get
            Set(ByVal value As T)
                If (value Is Nothing) Then Throw New ArgumentException("Destination MUST be defined.")
                BaseDataObject(PdfName.D) = value.NamedBaseObject
            End Set
        End Property

#End Region
#End Region
#End Region

    End Class
End Namespace
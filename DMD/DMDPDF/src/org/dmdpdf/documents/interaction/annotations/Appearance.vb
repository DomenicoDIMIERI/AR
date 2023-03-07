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
Imports DMD.org.dmdpdf.objects

Imports System

Namespace DMD.org.dmdpdf.documents.interaction.annotations

    '/**
    '  <summary>Appearance [PDF:1.6:8.4.4].</summary>
    '*/
    <PDF(VersionEnum.PDF12)>
    Public NotInheritable Class Appearance
        Inherits PdfObjectWrapper(Of PdfDictionary)

#Region "Static"
#Region "Interface"
#Region "Public"

        Public Shared Function Wrap(ByVal baseObject As PdfDirectObject) As Appearance
            If (baseObject IsNot Nothing) Then
                Return New Appearance(baseObject)
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
        '  <summary>Gets the annotation's down appearance.</summary>
        '*/
        Public ReadOnly Property Down As AppearanceStates
            Get
                Return New AppearanceStates(PdfName.D, Me)
            End Get
        End Property

        '/**
        '  <summary>Gets the annotation's normal appearance.</summary>
        '*/
        Public ReadOnly Property Normal As AppearanceStates
            Get
                Return New AppearanceStates(PdfName.N, Me)
            End Get
        End Property

        '/**
        '  <summary>Gets the annotation's rollover appearance.</summary>
        '*/
        Public ReadOnly Property Rollover As AppearanceStates
            Get
                Return New AppearanceStates(PdfName.R, Me)
            End Get
        End Property

#End Region
#End Region
#End Region

    End Class

End Namespace
'/*
'  Copyright 2006-2011 Stefano Chizzolini. http://www.dmdpdf.org

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

'  You should have received a copy of the GNU Lesser General Public License along with Me
'  Program (see README files); if not, go to the GNU website (http://www.gnu.org/licenses/).

'  Redistribution and use, with or without modification, are permitted provided that such
'  redistributions retain the above copyright notice, license and disclaimer, along with
'  Me list of conditions.
'*/

Imports DMD.org.dmdpdf.objects

Imports System
Imports System.Collections.Generic

Namespace DMD.org.dmdpdf.documents.contents.colorSpaces

    '/**
    '  <summary>Color value [PDF:1.6:4.5.1].</summary>
    '*/
    Public MustInherit Class Color
        Inherits PdfObjectWrapper(Of PdfDataObject)

#Region "static"
#Region "interface"
#Region "protected"

        '/**
        '  <summary>Gets the normalized value of a color component [PDF:1.6:4.5.1].</summary>
        '  <param name="value">Color component value to normalize.</param>
        '  <returns>Normalized color component value.</returns>
        '*/
        '/*
        '  NOTE: Further developments may result in a color-space family-specific
        '  implementation of Me method; currently Me implementation focuses on
        '  device colors only.
        '*/
        Protected Shared Function NormalizeComponent(ByVal value As Double) As Double
            If (value < 0) Then
                Return 0
            ElseIf (value > 1) Then
                Return 1
            Else
                Return value
            End If
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "fields"

        Private _colorSpace As ColorSpace

#End Region

#Region "constructors"

        'TODO:verify whether To remove the colorSpace argument (should be agnostic?)!
        Protected Sub New(ByVal colorSpace As ColorSpace, ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
            Me._colorSpace = ColorSpace
        End Sub

        Protected Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "interface"
#Region "public"
        'TODO remove?
        Public ReadOnly Property ColorSpace As ColorSpace
            Get
                Return Me._colorSpace
            End Get
        End Property

        '/**
        '  <summary> Gets the components defining Me color value.</summary>
        '*/
        Public MustOverride ReadOnly Property Components As IList(Of PdfDirectObject)

#End Region
#End Region
#End Region

    End Class

End Namespace
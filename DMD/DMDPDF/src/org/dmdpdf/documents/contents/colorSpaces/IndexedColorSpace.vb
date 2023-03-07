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

Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.objects
Imports DMD.org.dmdpdf.util

Imports System
Imports System.Collections.Generic
Imports System.Drawing

Namespace DMD.org.dmdpdf.documents.contents.colorSpaces

    '/**
    '  <summary>Indexed color space [PDF:1.6:4.5.5].</summary>
    '*/
    <PDF(VersionEnum.PDF11)>
    Public NotInheritable Class IndexedColorSpace
        Inherits SpecialColorSpace

#Region "dynamic"
#Region "fields"

        Private _baseColors As IDictionary(Of Integer, Color) = New Dictionary(Of Integer, Color)()
        Private _baseComponentValues As Byte()
        Private _baseSpace As ColorSpace

#End Region

#Region "constructors"
        'TODOIMPL New element constructor!

        Friend Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary> Gets the base color space In which the values In the color table
        '  are to be interpreted.</summary>
        '*/
        Public ReadOnly Property BaseSpace As ColorSpace
            Get
                If (_baseSpace Is Nothing) Then
                    _baseSpace = ColorSpace.Wrap(CType(BaseDataObject, PdfArray)(1))
                End If
                Return _baseSpace
            End Get
        End Property

        Public Overrides Function Clone(ByVal context As Document) As Object
            Throw New NotImplementedException()
        End Function

        Public Overrides ReadOnly Property ComponentCount As Integer
            Get
                Return 1
            End Get
        End Property

        Public Overrides ReadOnly Property DefaultColor As Color
            Get
                Return IndexedColor.Default
            End Get
        End Property

        '/**
        '  <summary> Gets the color corresponding To the specified table index resolved according To
        '  the <see cref="BaseSpace">base space</see>.<summary>
        '*/
        Public Function GetBaseColor(ByVal color As IndexedColor) As Color
            Dim colorIndex As Integer = color.Index
            Dim baseColor As Color = _baseColors(colorIndex)
            If (baseColor Is Nothing) Then
                Dim baseSpace As ColorSpace = Me.BaseSpace
                Dim components As IList(Of PdfDirectObject) = New List(Of PdfDirectObject)()
                '{
                Dim ComponentCount As Integer = baseSpace.ComponentCount
                Dim componentValueIndex As Integer = colorIndex * ComponentCount
                Dim baseComponentValues As Byte() = Me.BaseComponentValues
                For componentIndex As Integer = 0 To ComponentCount - 1
                    components.Add(PdfReal.Get((CInt(baseComponentValues(componentValueIndex)) And &HFF) / 255D)) : componentValueIndex += 1
                Next
                '}
                baseColor = baseSpace.GetColor(components, Nothing)
            End If
            Return baseColor
        End Function

        Public Overrides Function GetColor(ByVal components As IList(Of PdfDirectObject), ByVal context As IContentContext) As Color
            Return New IndexedColor(components)
        End Function

        Public Overrides Function GetPaint(ByVal color As Color) As Drawing.Brush
            Return BaseSpace.GetPaint(GetBaseColor(CType(color, IndexedColor)))
        End Function

#End Region

#Region "private"

        '    /**
        '  <summary> Gets the color table.</summary>
        '*/
        Private ReadOnly Property BaseComponentValues As Byte()
            Get
                If (_baseComponentValues Is Nothing) Then
                    _baseComponentValues = CType(CType(BaseDataObject, PdfArray).Resolve(3), IDataWrapper).ToByteArray()
                End If
                Return _baseComponentValues
            End Get
        End Property

#End Region
#End Region
#End Region

    End Class

End Namespace
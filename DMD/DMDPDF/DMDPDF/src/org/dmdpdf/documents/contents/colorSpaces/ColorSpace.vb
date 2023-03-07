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
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects

Imports System
Imports System.Collections.Generic
Imports System.Drawing

Namespace DMD.org.dmdpdf.documents.contents.colorSpaces

    '/**
    '  <summary>Color space [PDF:1.6:4.5].</summary>
    '*/
    Public MustInherit Class ColorSpace
        Inherits PdfObjectWrapper(Of PdfDirectObject)

#Region "static"
#Region "interface"
#Region "public"
        '/**
        '  <summary> Wraps the specified color space base Object into a color space Object.</summary>
        '  <param name = "baseObject" > Base Object Of a color space Object.</param>
        '  <returns> Color space Object corresponding To the base Object.</returns>
        '*/
        Public Shared Function Wrap(ByVal baseObject As PdfDirectObject) As ColorSpace
            If (baseObject Is Nothing) Then Return Nothing
            ' Get the data object corresponding to the color space!
            Dim baseDataObject As PdfDataObject = baseObject.Resolve()
            '/*
            '  NOTE: A color space Is defined by an array Object whose first element
            '  Is a name object identifying the color space family [PDF:1.6:4.5.2].
            '  For families that do Not require parameters, the color space CAN be
            '  specified simply by the family name itself instead Of an array.
            '*/
            Dim name As PdfName
            If (TypeOf (baseDataObject) Is PdfArray) Then
                name = CType(CType(baseDataObject, PdfArray)(0), PdfName)
            Else
                name = CType(baseDataObject, PdfName)
            End If

            If (name.Equals(PdfName.DeviceRGB)) Then
                Return New DeviceRGBColorSpace(baseObject)
            ElseIf (name.Equals(PdfName.DeviceCMYK)) Then
                Return New DeviceCMYKColorSpace(baseObject)
            ElseIf (name.Equals(PdfName.DeviceGray)) Then
                Return New DeviceGrayColorSpace(baseObject)
            ElseIf (name.Equals(PdfName.CalRGB)) Then
                Return New CalRGBColorSpace(baseObject)
            ElseIf (name.Equals(PdfName.CalGray)) Then
                Return New CalGrayColorSpace(baseObject)
            ElseIf (name.Equals(PdfName.ICCBased)) Then
                Return New ICCBasedColorSpace(baseObject)
            ElseIf (name.Equals(PdfName.Lab)) Then
                Return New LabColorSpace(baseObject)
            ElseIf (name.Equals(PdfName.DeviceN)) Then
                Return New DeviceNColorSpace(baseObject)
            ElseIf (name.Equals(PdfName.Indexed)) Then
                Return New IndexedColorSpace(baseObject)
            ElseIf (name.Equals(PdfName.Pattern)) Then
                Return New PatternColorSpace(baseObject)
            ElseIf (name.Equals(PdfName.Separation)) Then
                Return New SeparationColorSpace(baseObject)
            Else
                Throw New NotSupportedException("Color space " & name.toString & " unknown.")
            End If
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        Protected Sub New(ByVal context As Document, ByVal baseDataObject As PdfDirectObject)
            MyBase.New(context, baseDataObject)
        End Sub

        Protected Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "interface"
#Region "public"

        '    /**
        '  <summary> Gets the number Of components used To represent a color value.</summary>
        '*/
        Public MustOverride ReadOnly Property ComponentCount As Integer

        '/**
        '  <summary> Gets the initial color value within this color space.</summary>
        '*/
        Public MustOverride ReadOnly Property DefaultColor As Color

        '/**
        '  <summary> Gets the rendering representation Of the specified color value.</summary>
        '  <param name = "color" > Color value To convert into an equivalent rendering representation.</param>
        '*/
        Public MustOverride Function GetPaint(ByVal color As Color) As Brush

        '/**
        '  <summary> Gets the color value corresponding To the specified components
        '  interpreted according To this color space [PDF:1.6:4.5.1].</summary>
        '  <param name = "components" > Color components.</param>
        '  <param name = "context" > Content context.</param>
        '*/
        Public MustOverride Function GetColor(ByVal components As IList(Of PdfDirectObject), ByVal context As IContentContext) As Color

#End Region
#End Region
#End Region

    End Class

End Namespace
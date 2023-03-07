'/*
'  Copyright 2007-2011 Stefano Chizzolini. http://www.dmdpdf.org

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
Imports DMD.org.dmdpdf.documents.contents.colorSpaces
Imports DMD.org.dmdpdf.objects

Imports System.Collections.Generic

Namespace DMD.org.dmdpdf.documents.contents.objects

    '/**
    '  <summary>'Set the current color space to use for nonstroking operations' operation
    '  [PDF:1.6:4.5.7].</summary>
    '*/
    <PDF(VersionEnum.PDF11)>
    Public NotInheritable Class SetFillColorSpace
        Inherits Operation
        Implements IResourceReference(Of ColorSpace)

#Region "Static"
#Region "fields"

        Public Shared ReadOnly OperatorKeyword As String = "cs"

#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        Public Sub New(ByVal name As PdfName)
            MyBase.New(OperatorKeyword, name)
        End Sub

        Public Sub New(ByVal operands As IList(Of PdfDirectObject))
            MyBase.New(OperatorKeyword, operands)
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary>Gets the <see cref="ColorSpace">color space</see> resource to be set.</summary>
        '  <param name="context">Content context.</param>
        '*/
        Public Function GetColorSpace(ByVal context As IContentContext) As ColorSpace
            Return GetResource(context)
        End Function

        Public Overrides Sub Scan(ByVal state As ContentScanner.GraphicsState)
            ' 1. Color space.
            state.FillColorSpace = GetColorSpace(state.Scanner.ContentContext)

            '// 2. Initial color.
            '/*
            '  NOTE: The operation also sets the current nonstroking color
            '  to its initial value, which depends on the color space [PDF:1.6:4.5.7].
            '*/
            state.FillColor = state.FillColorSpace.DefaultColor
        End Sub

#Region "IResourceReference"
        Public Function GetResource(ByVal context As IContentContext) As ColorSpace Implements IResourceReference(Of ColorSpace).GetResource
            '/*
            '    NOTE: The names DeviceGray, DeviceRGB, DeviceCMYK, and Pattern always identify
            '    the corresponding color spaces directly; they never refer to resources in the
            '    ColorSpace subdictionary [PDF:1.6:4.5.7].
            '  */
            Dim name As PdfName = Me.Name
            If (name.Equals(PdfName.DeviceGray)) Then
                Return DeviceGrayColorSpace.Default
            ElseIf (name.Equals(PdfName.DeviceRGB)) Then
                Return DeviceRGBColorSpace.Default
            ElseIf (name.Equals(PdfName.DeviceCMYK)) Then
                Return DeviceCMYKColorSpace.Default
            ElseIf (name.Equals(PdfName.Pattern)) Then
                Return PatternColorSpace.Default
            Else
                Return context.Resources.ColorSpaces(name)
            End If
        End Function

        Public Property Name As PdfName Implements IResourceReference(Of ColorSpace).Name
            Get
                Return CType(Me._operands(0), PdfName)
            End Get
            Set(ByVal value As PdfName)
                Me._operands(0) = value
            End Set
        End Property

#End Region
#End Region
#End Region
#End Region

    End Class
End Namespace
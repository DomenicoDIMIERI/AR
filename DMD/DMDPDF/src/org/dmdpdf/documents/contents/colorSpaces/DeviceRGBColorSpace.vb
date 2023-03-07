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

'  You should have received a copy of the GNU Lesser General Public License along with this
'  Program (see README files); if not, go to the GNU website (http://www.gnu.org/licenses/).

'  Redistribution and use, with or without modification, are permitted provided that such
'  redistributions retain the above copyright notice, license and disclaimer, along with
'  this list of conditions.
'*/

Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.objects

Imports System
Imports System.Collections.Generic
Imports drawing = System.Drawing

Namespace DMD.org.dmdpdf.documents.contents.colorSpaces

    '/**
    '  <summary>Device Red-Green-Blue color space [PDF:1.6:4.5.3].</summary>
    '*/
    <PDF(VersionEnum.PDF11)>
    Public NotInheritable Class DeviceRGBColorSpace
        Inherits DeviceColorSpace

#Region "static"
#Region "fields"
        '/*
        '  NOTE: It may be specified directly (i.e. without being defined in the ColorSpace subdictionary
        '  of the contextual resource dictionary) [PDF:1.6:4.5.7].
        '*/
        Public Shared ReadOnly [Default] As DeviceRGBColorSpace = New DeviceRGBColorSpace(PdfName.DeviceRGB)

#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        Public Sub New(ByVal context As Document)
            MyBase.new(context, PdfName.DeviceRGB)
        End Sub

        Friend Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public Overrides Function Clone(ByVal context As Document) As Object
            Throw New NotImplementedException()
        End Function

        Public Overrides ReadOnly Property ComponentCount As Integer
            Get
                Return 3
            End Get
        End Property

        Public Overrides ReadOnly Property DefaultColor As Color
            Get
                Return DeviceRGBColor.Default
            End Get
        End Property

        Public Overrides Function GetColor(ByVal components As IList(Of PdfDirectObject), ByVal context As IContentContext) As Color
            Return New DeviceRGBColor(components)
        End Function

        Public Overrides Function GetPaint(ByVal color As Color) As drawing.Brush
            Dim spaceColor As DeviceRGBColor = CType(color, DeviceRGBColor)
            Return New drawing.SolidBrush(drawing.Color.FromArgb(
                                                        CInt(Math.Round(spaceColor.R * 255)),
                                                        CInt(Math.Round(spaceColor.G * 255)),
                                                        CInt(Math.Round(spaceColor.B * 255))
                                                        )
                                )
        End Function

#End Region
#End Region
#End Region

    End Class
End Namespace
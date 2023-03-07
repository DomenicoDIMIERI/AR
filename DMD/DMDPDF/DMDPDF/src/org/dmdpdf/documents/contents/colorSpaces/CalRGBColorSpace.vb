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
Imports System.Drawing

Namespace DMD.org.dmdpdf.documents.contents.colorSpaces

    '/**
    '  <summary>CIE-based ABC single-transformation-stage color space, where A, B, and C represent
    '  calibrated red, green and blue color values [PDF:1.6:4.5.4].</summary>
    '*/
    <PDF(VersionEnum.PDF11)>
    Public NotInheritable Class CalRGBColorSpace
        Inherits CalColorSpace

#Region "dynamic"
#Region "constructors"
        'TODO:IMPL new element constructor!

        Friend Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.new(baseObject)
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
                Return CalRGBColor.Default
            End Get
        End Property

        Public Overrides ReadOnly Property Gamma As Double()
            Get
                Dim gammaObject As PdfArray = CType(Dictionary(PdfName.Gamma), PdfArray)
                If (gammaObject Is Nothing) Then
                    Return New Double() {1, 1, 1}
                Else
                    Return New Double() {
                          CType(gammaObject(0), IPdfNumber).RawValue,
                          CType(gammaObject(1), IPdfNumber).RawValue,
                          CType(gammaObject(2), IPdfNumber).RawValue
                            }
                End If
            End Get
        End Property

        Public Overrides Function GetColor(ByVal components As IList(Of PdfDirectObject), ByVal context As IContentContext) As Color
            Return New CalRGBColor(components)
        End Function

        Public Overrides Function GetPaint(ByVal color As Color) As Drawing.Brush
            ' FIXME: temporary hack
            Return New Drawing.SolidBrush(Drawing.Color.Black)
        End Function

#End Region
#End Region
#End Region

    End Class
End Namespace
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
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects

Imports System
Imports System.Collections.Generic
Imports System.Drawing

Namespace DMD.org.dmdpdf.documents.contents.colorSpaces

    '/**
    '  <summary>ICC-based color space [PDF:1.6:4.5.4].</summary>
    '*/
    ' TODO:IMPL improve profile support (see ICC.1:2003-09 spec)!!!
    <PDF(VersionEnum.PDF13)>
    Public NotInheritable Class ICCBasedColorSpace
        Inherits ColorSpace

#Region "dynamic"
#Region "constructors"
        'TODO:IMPL new element constructor!

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
                ' FIXME: Auto-generated method stub
                Return 0
            End Get
        End Property

        Public Overrides ReadOnly Property DefaultColor() As Color
            Get
                Return DeviceGrayColor.Default ' FIXME:temporary hack...
            End Get
        End Property

        Public Overrides Function GetColor(ByVal components As IList(Of PdfDirectObject), ByVal context As IContentContext) As Color
            Return New DeviceRGBColor(components) ' FIXME:temporary hack...
        End Function

        Public Overrides Function GetPaint(ByVal color As Color) As Drawing.Brush
            ' FIXME: temporary hack
            Return New Drawing.SolidBrush(Drawing.Color.Black)
        End Function

        Public ReadOnly Property Profile As PdfStream
            Get
                Return CType(CType(BaseDataObject, PdfArray).Resolve(1), PdfStream)
            End Get
        End Property

#End Region
#End Region
#End Region

    End Class

End Namespace
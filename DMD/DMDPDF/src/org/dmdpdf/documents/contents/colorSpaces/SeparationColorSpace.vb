'/*
'  Copyright 2010-2011 Stefano Chizzolini. http://www.dmdpdf.org

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
    '  <summary>Special color space that provides a means for specifying the use of additional colorants
    '  or for isolating the control of individual color components of a device color space for
    '  a subtractive device [PDF:1.6:4.5.5].</summary>
    '  <remarks>When such a space is the current color space, the current color is a single-component value,
    '  called a tint, that controls the application of the given colorant or color components only.</remarks>
    '*/
    <PDF(VersionEnum.PDF12)>
    Public NotInheritable Class SeparationColorSpace
        Inherits SpecialDeviceColorSpace

#Region "static"
#Region "fields"
        '/**
        '  <summary>Special colorant name referring collectively to all components available on an output
        '  device, including those for the standard process components.</summary>
        '  <remarks>When a separation space with this component name is the current color space, painting
        '  operators apply tint values to all available components at once.</remarks>
        '*/
        Public Shared ReadOnly AllComponentName As String = CStr(PdfName.All.Value)

#End Region
#End Region

#Region "dynamic"
#Region "constructors"
        'TODOIMPL New element constructor!

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
                Return 1
            End Get
        End Property

        '/**
        '  <summary> Gets the name Of the colorant that this separation color space Is intended
        '  to represent.</summary>
        '  <remarks> Special names
        '    <List type = "bullet" >
        '      <item><see cref="AllComponentName">All</see></item>
        '    <item> <see cref = "NoneComponentName" > None</see></item>
        '    </list>
        '  </remarks>
        '*/
        Public Overrides ReadOnly Property ComponentNames As IList(Of String)
            Get
                Return New List(Of String)(New String() {CStr(CType(CType(BaseDataObject, PdfArray)(1), PdfName).Value)})
            End Get
        End Property

        Public Overrides ReadOnly Property DefaultColor As Color
            Get
                Return SeparationColor.Default
            End Get
        End Property

        Public Overrides Function GetColor(ByVal components As IList(Of PdfDirectObject), ByVal context As IContentContext) As Color
            Return New SeparationColor(components)
        End Function

#End Region
#End Region
#End Region

    End Class

End Namespace
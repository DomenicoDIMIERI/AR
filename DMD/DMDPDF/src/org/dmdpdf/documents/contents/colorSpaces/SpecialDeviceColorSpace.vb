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
Imports DMD.org.dmdpdf.documents.functions
Imports DMD.org.dmdpdf.objects
Imports DMD.org.dmdpdf.util

Imports System
Imports System.Collections.Generic
Imports System.Drawing

Namespace DMD.org.dmdpdf.documents.contents.colorSpaces

    '/**
    '  <summary>Special device color space [PDF:1.6:4.5.5].</summary>
    '*/
    <PDF(VersionEnum.PDF12)>
    Public MustInherit Class SpecialDeviceColorSpace
        Inherits SpecialColorSpace

#Region "static"
#Region "fields"
        '/**
        '  <summary>Special colorant name never producing any visible output.</summary>
        '  <remarks>When a color space with this component name is the current color space, painting
        '  operators have no effect.</remarks>
        '*/
        Public Shared ReadOnly NoneComponentName As String = CStr(PdfName.None.Value)

#End Region
#End Region

#Region "dynamic"
#Region "constructors"
        'TODOIMPL New element constructor!

        Friend Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.new(baseObject)
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary> Gets the alternate color space used In Case any Of the <see cref="ComponentNames">component names</see>
        '  in the color space do Not correspond to a component available on the device.</summary>
        '*/
        Public ReadOnly Property AlternateSpace As ColorSpace
            Get
                Return ColorSpace.Wrap(CType(BaseDataObject, PdfArray)(2))
            End Get
        End Property

        '/**
        '  <summary> Gets the names Of the color components.</summary>
        '*/
        Public MustOverride ReadOnly Property ComponentNames As IList(Of String)


        Public Overrides Function GetPaint(ByVal color As Color) As Drawing.Brush
            '    //TODO:enable!!!
            '//    IList<PdfDirectObject> alternateColorComponents = TintFunction.Calculate(color.Components);
            '//    ColorSpace alternateSpace = AlternateSpace;
            '//    return alternateSpace.GetPaint(
            '//      alternateSpace.GetColor(
            '//        alternateColorComponents,
            '//        null
            '//        )
            '//      );

            '    //TODO remove (temporary hack)!
            Return New Drawing.SolidBrush(Drawing.Color.Black)
        End Function

        '/**
        '  <summary> Gets the Function To transform a tint value into color component values
        '  in the <see cref="AlternateSpace">alternate color space</see>.</summary>
        '*/
        Public ReadOnly Property TintFunction As [Function]
            Get
                Return [Function].Wrap(CType(BaseDataObject, PdfArray)(3))
            End Get
        End Property

#End Region
#End Region
#End Region

    End Class
End Namespace
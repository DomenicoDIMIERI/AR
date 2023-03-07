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

'  You should have received a copy of the GNU Lesser General Public License along with Me
'  Program (see README files); if not, go to the GNU website (http://www.gnu.org/licenses/).

'  Redistribution and use, with or without modification, are permitted provided that such
'  redistributions retain the above copyright notice, license and disclaimer, along with
'  Me list of conditions.
'*/

Imports DMD.org.dmdpdf.objects

Imports System
Imports System.Drawing
Imports System.Collections.Generic

Namespace DMD.org.dmdpdf.documents.contents.colorSpaces

    '/**
    '  <summary>Pattern consisting of a small graphical figure called <i>pattern cell</i> [PDF:1.6:4.6.2].</summary>
    '  <remarks>Painting with the pattern replicates the cell at fixed horizontal and vertical intervals
    '  to fill an area.</remarks>
    '*/
    'TODO: define as IContentContext?
    <PDF(VersionEnum.PDF12)>
    Public Class TilingPattern
        Inherits Pattern

#Region "types"
        '/**
        '  <summary>Uncolored tiling pattern ("stencil") associated to a color.</summary>
        '*/
        Public NotInheritable Class Colorized
            Inherits TilingPattern

            Private _color As Color

            Friend Sub New(ByVal uncoloredPattern As TilingPattern, ByVal color As Color)
                MyBase.New(CType(uncoloredPattern.ColorSpace, PatternColorSpace), uncoloredPattern.BaseObject)
                Me._color = color
            End Sub

            '/**
            '  <summary>Gets the color applied to the stencil.</summary>
            '*/
            Public ReadOnly Property Color As Color
                Get
                    Return Me._color
                End Get
            End Property
        End Class

        '/**
        '  <summary>Pattern cell color mode.</summary>
        '*/
        Public Enum PaintTypeEnum
            '/**
            '  <summary>The pattern's content stream specifies the colors used to paint the pattern cell.</summary>
            '  <remarks>When the content stream begins execution, the current color is the one
            '  that was initially in effect in the pattern's parent content stream.</remarks>
            '*/
            Colored = 1
            '/**
            '  <summary>The pattern's content stream does NOT specify any color information.</summary>
            '  <remarks>
            '    <para>Instead, the entire pattern cell is painted with a separately specified color
            '    each time the pattern is used; essentially, the content stream describes a stencil
            '    through which the current color is to be poured.</para>
            '    <para>The content stream must not invoke operators that specify colors
            '    or other color-related parameters in the graphics state.</para>
            '  </remarks>
            '*/
            Uncolored = 2
        End Enum

        '/**
        '  Spacing adjustment of tiles relative to the device pixel grid.
        '*/
        Public Enum TilingTypeEnum
            '/**
            '  <summary>Pattern cells are spaced consistently, that is by a multiple of a device pixel.</summary>
            '*/
            ConstantSpacing = 1
            '/**
            '  <summary>The pattern cell is not distorted, but the spacing between pattern cells
            '  may vary by as much as 1 device pixel, both horizontally and vertically,
            '  when the pattern is painted.</summary>
            '*/
            VariableSpacing = 2
            '/**
            '  <summary>Pattern cells are spaced consistently as in tiling type 1
            '  but with additional distortion permitted to enable a more efficient implementation.</summary>
            '*/
            FasterConstantSpacing = 3
        End Enum

#End Region

#Region "dynamic"
#Region "constructors"

        Friend Sub New(ByVal colorSpace As PatternColorSpace, ByVal baseObject As PdfDirectObject)
            MyBase.New(colorSpace, baseObject)
        End Sub

        Friend Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "interface"
#Region "public"

        '    /**
        '  <summary> Gets the colorized representation Of Me pattern.</summary>
        '  <param name = "color" > color To be applied To the pattern.</param>
        '*/
        Public Function Colorize(ByVal color As Color) As Colorized
            If (PaintType <> PaintTypeEnum.Uncolored) Then Throw New NotSupportedException("Only uncolored tiling patterns can be colorized.")
            Return New Colorized(Me, color)
        End Function

        '/**
        '  <summary> Gets the pattern cell's bounding box (expressed in the pattern coordinate system)
        '  used to clip the pattern cell.</summary>
        '*/
        Public ReadOnly Property Box As RectangleF
            Get
                '/*
                '  NOTE: 'BBox' entry MUST be defined.
                '*/
                Dim _box As org.dmdpdf.objects.Rectangle = org.dmdpdf.objects.Rectangle.Wrap(BaseHeader(PdfName.BBox))
                Return New RectangleF(CSng(_box.X), CSng(_box.Y), CSng(_box.Width), CSng(_box.Height))
            End Get
        End Property

        '/**
        '  <summary> Gets how the color Of the pattern cell Is To be specified.</summary>
        '*/
        Public ReadOnly Property PaintType As PaintTypeEnum
            Get
                Return CType(CType(BaseHeader(PdfName.PaintType), PdfInteger).RawValue, PaintTypeEnum)
            End Get
        End Property

        '/**
        '  <summary> Gets the named resources required by the pattern's content stream.</summary>
        '*/
        Public ReadOnly Property Resources As Resources
            Get
                Return Resources.Wrap(BaseHeader(PdfName.Resources))
            End Get
        End Property

        '/**
        '  <summary> Gets how To adjust the spacing Of tiles relative To the device pixel grid.</summary>
        '*/
        Public ReadOnly Property TilingType As TilingTypeEnum
            Get
                Return CType(CType(BaseHeader(PdfName.TilingType), PdfInteger).RawValue, TilingTypeEnum)
            End Get
        End Property

        '/**
        '  <summary> Gets the horizontal spacing between pattern cells (expressed In the pattern coordinate system).</summary>
        '*/
        Public ReadOnly Property XStep As Double
            Get
                Return CType(BaseHeader(PdfName.XStep), IPdfNumber).RawValue
            End Get
        End Property

        '/**
        '  <summary> Gets the vertical spacing between pattern cells (expressed In the pattern coordinate system).</summary>
        '*/
        Public ReadOnly Property YStep As Double
            Get
                Return CType(BaseHeader(PdfName.YStep), IPdfNumber).RawValue
            End Get
        End Property

#End Region

#Region "Private"

        Private ReadOnly Property BaseHeader As PdfDictionary
            Get
                Return CType(BaseDataObject, PdfStream).Header
            End Get
        End Property

#End Region
#End Region
#End Region

    End Class

End Namespace

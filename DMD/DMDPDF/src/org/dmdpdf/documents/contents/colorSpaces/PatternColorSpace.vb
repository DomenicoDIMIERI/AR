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
    '  <summary>Pattern color space [PDF:1.6:4.5.5].</summary>
    '*/
    <PDF(VersionEnum.PDF12)>
    Public NotInheritable Class PatternColorSpace
        Inherits SpecialColorSpace

#Region "static"
#Region "fields"
        '/*
        '  NOTE: In case of no parameters, it may be specified directly (i.e. without being defined
        '  in the ColorSpace subdictionary of the contextual resource dictionary) [PDF:1.6:4.5.7].
        '*/
        '//TODO:verify parameters!!!
        Public Shared ReadOnly [Default] As PatternColorSpace = New PatternColorSpace(Nothing)

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
                Return 0
            End Get
        End Property

        Public Overrides ReadOnly Property DefaultColor As Color
            Get
                Return Pattern.Default
            End Get
        End Property

        Public Overrides Function GetColor(ByVal components As IList(Of PdfDirectObject), ByVal context As IContentContext) As Color
            '//TODO
            '/*
            '      Pattern pattern = context.Resources.Patterns[components[components.Count-1]];
            '      If (pattern Is TilingPattern)
            '      {
            '        TilingPattern tilingPattern = (tilingPattern)pattern;
            '        If (tilingPattern.PaintType == PaintTypeEnum.Uncolored)
            '        {
            '          ColorSpace underlyingColorSpace = underlyingColorSpace;
            '          If (underlyingColorSpace == null)
            '        Throw New IllegalArgumentException("Uncolored tiling patterns not supported by this color space because no underlying color space has been defined.");

            '          // Get the color to be used for colorizing the uncolored tiling pattern!
            '          Color color = underlyingColorSpace.GetColor(components, context);
            '          // Colorize the uncolored tiling pattern!
            '          pattern = tilingPattern.Colorize(color);
            '        }
            '      }
            '      Return pattern;
            '*/
            Return Nothing
        End Function

        Public Overrides Function GetPaint(ByVal color As Color) As Drawing.Brush
            ' FIXME: Auto-generated method stub
            Return Nothing
        End Function

        '/**
        '  <summary> Gets the color space In which the actual color Of the <see cref="Pattern">pattern</see> Is To be specified.</summary>
        '  <remarks> This feature Is applicable To <see cref="TilingPattern">uncolored tiling patterns</see> only.</remarks>
        '*/
        Public ReadOnly Property UnderlyingColorSpace As ColorSpace
            Get
                Dim baseDataObject As PdfDirectObject = Me.BaseDataObject
                If (TypeOf (baseDataObject) Is PdfArray) Then
                    Dim baseArrayObject As PdfArray = CType(baseDataObject, PdfArray)
                    If (baseArrayObject.Count > 1) Then
                        Return ColorSpace.Wrap(baseArrayObject(1))
                    End If
                End If
                Return Nothing
            End Get
        End Property
#End Region
#End Region
#End Region

    End Class

End Namespace
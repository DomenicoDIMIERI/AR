'/*
'  Copyright 2010 - 2011 Stefano Chizzolini. http: //www.dmdpdf.org

'  Contributors:
'    * Stefano Chizzolini (original code developer, http//www.stefanochizzolini.it)

'  This file should be part Of the source code distribution Of "PDF Clown library" (the
'  Program): see the accompanying README files For more info.

'  This Program Is free software; you can redistribute it And/Or modify it under the terms
'  of the GNU Lesser General Public License as published by the Free Software Foundation;
'  either version 3 Of the License, Or (at your Option) any later version.

'  This Program Is distributed In the hope that it will be useful, but WITHOUT ANY WARRANTY,
'  either expressed Or implied; without even the implied warranty Of MERCHANTABILITY Or
'  FITNESS FOR A PARTICULAR PURPOSE. See the License for more details.

'  You should have received a copy Of the GNU Lesser General Public License along With Me
'  Program(see README files); If Not, go To the GNU website (http://www.gnu.org/licenses/).

'  Redistribution And use, with Or without modification, are permitted provided that such
'  redistributions retain the above copyright notice, license And disclaimer, along With
'  Me list Of conditions.
'*/

Imports DMD.org.dmdpdf.documents.contents.colorSpaces
Imports DMD.org.dmdpdf.documents.contents.fonts
Imports DMD.org.dmdpdf.objects

Namespace DMD.org.dmdpdf.documents.contents

    '/**
    '  <summary> Text style.</summary>
    '*/
    Public NotInheritable Class TextStyle

#Region "dynamic"
#Region "fields"
        Private ReadOnly m_fillColor As Color
        Private ReadOnly m_fillColorSpace As ColorSpace
        Private ReadOnly m_font As Font
        Private ReadOnly m_fontSize As Double
        Private ReadOnly m_renderMode As TextRenderModeEnum
        Private ReadOnly m_strokeColor As Color
        Private ReadOnly m_strokeColorSpace As ColorSpace
#End Region

#Region "constructors"
        Public Sub New(ByVal font As Font, ByVal fontSize As Double, ByVal renderMode As TextRenderModeEnum, ByVal strokeColor As Color, ByVal strokeColorSpace As ColorSpace, ByVal fillColor As Color, ByVal fillColorSpace As ColorSpace)
            Me.m_font = font
            Me.m_fontSize = fontSize
            Me.m_renderMode = renderMode
            Me.m_strokeColor = strokeColor
            Me.m_strokeColorSpace = strokeColorSpace
            Me.m_fillColor = fillColor
            Me.m_fillColorSpace = fillColorSpace
        End Sub

#End Region

#Region "Interface"
#Region "Public"
        Public ReadOnly Property FillColor As Color
            Get
                Return Me.m_fillColor
            End Get
        End Property

        Public ReadOnly Property FillColorSpace As ColorSpace
            Get
                Return Me.m_fillColorSpace
            End Get
        End Property

        Public ReadOnly Property Font As Font
            Get
                Return Me.m_font
            End Get
        End Property

        Public ReadOnly Property FontSize As Double
            Get
                Return Me.m_fontSize
            End Get
        End Property

        Public ReadOnly Property RenderMode As TextRenderModeEnum
            Get
                Return Me.m_renderMode
            End Get
        End Property

        Public ReadOnly Property StrokeColor As Color
            Get
                Return Me.m_strokeColor
            End Get
        End Property

        Public ReadOnly Property StrokeColorSpace As ColorSpace
            Get
                Return Me.m_strokeColorSpace
            End Get
        End Property

#End Region
#End Region
#End Region

    End Class

End Namespace
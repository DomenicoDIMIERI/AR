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

Imports DMD.org.dmdpdf.objects

Imports System.Collections.Generic

Namespace DMD.org.dmdpdf.documents.contents.colorSpaces

    '/**
    '  <summary>Pattern providing a smooth transition between colors across an area to be painted [PDF:1.6:4.6.3].</summary>
    '  <remarks>The transition is continuous and independent of the resolution of any particular output device.</remarks>
    '*/
    <PDF(VersionEnum.PDF13)>
    Public NotInheritable Class ShadingPattern
        Inherits Pattern

#Region "dynamic"
#Region "constructors"
        'TODO:IMPL new element constructor!

        Friend Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "interface"
#Region "public"
        '/**
        '  <summary> Gets the graphics state parameters To be put into effect temporarily
        '  While the shading pattern Is painted.</summary>
        '  <remarks> Any parameters that are Not so specified are inherited from the graphics state
        '  that was In effect at the beginning Of the content stream In which the pattern
        '  Is defined as a resource.</remarks>
        ' */
        Public ReadOnly Property ExtGState As ExtGState
            Get
                Return ExtGState.Wrap(CType(BaseDataObject, PdfDictionary)(PdfName.ExtGState))
            End Get
        End Property

        '/**
        '  <summary> Gets a shading Object defining the shading pattern's gradient fill.</summary>
        '*/
        Public ReadOnly Property Shading As Shading
            Get
                Return Shading.Wrap(CType(BaseDataObject, PdfDictionary)(PdfName.Shading))
            End Get
        End Property

#End Region
#End Region
#End Region
    End Class

End Namespace
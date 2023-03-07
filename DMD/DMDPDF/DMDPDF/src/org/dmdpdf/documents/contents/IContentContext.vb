'/*
'  Copyright 2006-2010 Stefano Chizzolini. http://www.dmdpdf.org

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

Imports System.Drawing

Namespace DMD.org.dmdpdf.documents.contents

    '/**
    '  <summary>Content stream context.</summary>
    '*/
    Public Interface IContentContext
        Inherits IContentEntity

        '    /**
        '  <summary>Gets the bounding box associated with this content context either explicitly
        '  (directly associated to the object) or (if not explicitly available) implicitly (inherited
        '  from a higher level object), expressed in default user-space units.</summary>
        '*/
        ReadOnly Property Box As RectangleF

        '/**
        '  <summary>Gets the contents collection representing the content stream associated
        '  with this content context.</summary>
        '*/
        ReadOnly Property Contents As contents

        '/**
        '  Renders this content context into the specified rendering context.

        '  @param context Rendering context.
        '  @param size Rendering canvas size.
        '  @since 0.1.0
        '*/
        Sub Render(ByVal context As Graphics, ByVal size As SizeF)

        '/**
        '  <summary>Gets the resources associated with this content context either explicitly (directly
        '  associated to the object) or (if not explicitly available) implicitly (inherited from a
        '  higher-level object).</summary>
        '  <remarks>The implementing class MUST ensure that the returned object isn't
        '  <code>null</code>.</remarks>
        '*/
        ReadOnly Property Resources As Resources

        '/**
        '  <summary>Gets the rendering rotation of this content context.</summary>
        '*/
        ReadOnly Property Rotation As RotationEnum

    End Interface

End Namespace

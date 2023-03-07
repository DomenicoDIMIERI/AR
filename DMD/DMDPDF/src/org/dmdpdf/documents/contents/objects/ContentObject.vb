'/*
'  Copyright 2007-2011 Stefano Chizzolini. http://www.dmdpdf.org

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

Imports DMD.org.dmdpdf.bytes

Namespace DMD.org.dmdpdf.documents.contents.objects

    '/**
    '  <summary>Abstract content object [PDF:1.6:4.1].</summary>
    '*/
    <PDF(VersionEnum.PDF10)>
    Public MustInherit Class ContentObject

#Region "dynamic"
#Region "interface"
#Region "public"

        '/**
        '  <summary>Applies this object to the specified graphics context, updating the specified
        '  graphics state.</summary>
        '  <param name="state">Graphics state.</param>
        '*/
        Public Overridable Sub Scan(ByVal state As ContentScanner.GraphicsState)
            ' Do nothing by default. 
        End Sub

        '/**
        '  <summary>Serializes this object to the specified stream.</summary>
        '  <param name="stream">Target stream.</param>
        '  <param name="context">Document context.</param>
        '*/
        Public MustOverride Sub WriteTo(ByVal stream As IOutputStream, ByVal context As Document)

#End Region
#End Region
#End Region

    End Class

End Namespace
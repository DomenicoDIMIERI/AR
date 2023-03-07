'/*
'  Copyright 2006-2012 Stefano Chizzolini. http://www.dmdpdf.org

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

Imports DMD.org.dmdpdf.files

Namespace DMD.org.dmdpdf.objects

    ''' <summary>
    ''' PDF indirect object interface.
    ''' </summary>
    Public Interface IPdfIndirectObject

        Function Clone(ByVal context As files.File) As PdfObject

        ''' <summary>
        ''' Gets/Sets the actual data associated to the indirect reference.
        ''' </summary>
        ''' <returns></returns>
        Property DataObject As PdfDataObject

        ''' <summary>
        ''' Removes the object from its file context.
        ''' </summary>
        ''' <remarks>The Object Is no more usable after this method returns.</remarks>
        Sub Delete()

        ''' <summary>
        ''' Gets the indirect object associated to the indirect reference
        ''' </summary>
        ''' <returns></returns>
        ReadOnly Property IndirectObject As PdfIndirectObject

        ''' <summary>
        ''' Gets the indirect reference associated To the indirect Object.
        ''' </summary>
        ''' <returns></returns>
        ReadOnly Property Reference As PdfReference

    End Interface

End Namespace

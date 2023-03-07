'/*
'  Copyright 2010 Stefano Chizzolini. http://www.dmdpdf.org

'  Contributors:
'    * Stefano Chizzolini (original code developer, http://www.stefanochizzolini.it)

'  This file should be part of the source code distribution of "PDF Clown library"
'  (the Program): see the accompanying README files for more info.

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

Imports System

Namespace DMD.org.dmdpdf.tokens

    '/**
    '  <summary>Common data chunks for serialization purposes.</summary>
    '*/
    Friend Class Chunk 'Static 

        ' FIXME: proper calls to Encoding.Pdf.Encode(...) could NOT be done due to an unexpected 
        ' Mono runtime SIGSEGV (TOO BAD).
        Public Shared ReadOnly Space As Byte() = New Byte() {32} ' Encoding.Pdf.Encode(Symbol.Space);
        Public Shared ReadOnly LineFeed As Byte() = New Byte() {10} ' Encoding.Pdf.Encode(Symbol.LineFeed);
    End Class

End Namespace
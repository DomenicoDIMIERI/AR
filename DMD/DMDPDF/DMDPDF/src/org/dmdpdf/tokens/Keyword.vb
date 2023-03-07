'/*
'  Copyright 2010-2011 Stefano Chizzolini. http://www.dmdpdf.org

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

Namespace DMD.org.dmdpdf.tokens

    '/**
    '  <summary>PDF keywords.</summary>
    '*/
    Public Class Keyword

        ''' <summary>
        ''' PDF array opening delimiter.
        ''' </summary>
        Public Shared ReadOnly BeginArray As String = Symbol.OpenSquareBracket.ToString()

        ''' <summary>
        ''' PDF comment opening delimiter. 
        ''' </summary>
        Public Shared ReadOnly BeginComment As String = Symbol.Percent.ToString()

        ''' <summary>
        ''' PDF dictionary opening delimiter.
        ''' </summary>
        Public Shared ReadOnly BeginDictionary As String = Symbol.OpenAngleBracket.ToString() & Symbol.OpenAngleBracket.ToString()

        ''' <summary>
        ''' PDF indirect Object begin.
        ''' </summary>
        Public Const BeginIndirectObject As String = "obj"

        ''' <summary>
        ''' PDF literal String opening delimiter.
        ''' </summary>
        Public Shared ReadOnly BeginLiteralString As String = Symbol.OpenRoundBracket.ToString()

        ''' <summary>
        ''' PDF stream data begin.
        ''' </summary>
        Public Const BeginStream As String = "stream"

        ''' <summary>
        ''' PDF file begin.
        ''' </summary>
        Public Const BOF As String = "%PDF-"

        ''' <summary>
        ''' PDF Date marker.
        ''' </summary>
        Public Const DatePrefix As String = "D:"

        ''' <summary>
        ''' PDF array closing delimiter.
        ''' </summary>
        Public Shared ReadOnly EndArray As String = Symbol.CloseSquareBracket.ToString()

        ''' <summary>
        ''' PDF dictionary closing delimiter.
        ''' </summary>
        Public Shared ReadOnly EndDictionary As String = Symbol.CloseAngleBracket.ToString() & Symbol.CloseAngleBracket.ToString()

        ''' <summary>
        ''' PDF indirect Object End.
        ''' </summary>
        Public Const EndIndirectObject As String = "endobj"

        ''' <summary>
        ''' PDF literal String closing delimiter.
        ''' </summary>
        Public Shared ReadOnly EndLiteralString As String = Symbol.CloseRoundBracket.ToString()

        ''' <summary>
        ''' PDF stream data End.
        ''' </summary>
        Public Const EndStream As String = "endstream"

        ''' <summary>
        ''' PDF file End.
        ''' </summary>
        Public Const EOF As String = "%%EOF"

        ''' <summary>
        ''' PDF Boolean False.
        ''' </summary>
        Public Const [False] As String = "false"

        ''' <summary>
        ''' PDF free xref entry marker.
        ''' </summary>
        Public Const FreeXrefEntry As String = "f"

        ''' <summary>
        ''' PDF In-use xref entry marker.
        ''' </summary>
        Public Const InUseXrefEntry As String = "n"

        ''' <summary>
        ''' PDF name marker.
        ''' </summary>
        Public Shared ReadOnly NamePrefix As String = Symbol.Slash.ToString()

        ''' <summary>
        ''' PDF null Object.
        ''' </summary>
        Public Const Null As String = "null"

        ''' <summary>
        ''' PDF indirect reference marker.
        ''' </summary>
        Public Shared ReadOnly Reference As String = Symbol.CapitalR.ToString()

        ''' <summary>
        ''' PDF xref start offset.
        ''' </summary>
        Public Const StartXRef As String = "startxref"

        ''' <summary>
        ''' PDF trailer begin.
        ''' </summary>
        Public Const Trailer As String = "trailer"

        ''' <summary>
        ''' PDF Boolean True.
        ''' </summary>
        Public Const [True] As String = "true"

        ''' <summary>
        ''' PDF xref begin.
        ''' </summary>
        Public Const XRef As String = "xref"

    End Class

End Namespace



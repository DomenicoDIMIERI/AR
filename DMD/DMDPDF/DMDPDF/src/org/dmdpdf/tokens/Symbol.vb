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

Namespace DMD.org.dmdpdf.tokens

    '/**
    '  <summary>PDF symbols.</summary>
    '*/
    Public Class Symbol
        Private Sub New()
        End Sub

        Public Const CapitalR As Char = "R"c
        Public Const CarriageReturn As Char = ControlChars.Cr ' '\r';
        Public Const CloseAngleBracket As Char = ">"c
        Public Const CloseRoundBracket As Char = ")"c
        Public Const CloseSquareBracket As Char = "]"c
        Public Const LineFeed As Char = ControlChars.Lf ' '\n';
        Public Const OpenAngleBracket As Char = "<"c
        Public Const OpenRoundBracket As Char = "("c
        Public Const OpenSquareBracket As Char = "["c
        Public Const Percent As Char = "%"c
        Public Const Slash As Char = "/"c
        Public Const Space As Char = " "c

    End Class

End Namespace

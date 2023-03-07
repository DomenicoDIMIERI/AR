'/*
'  Copyright 2010-2012 Stefano Chizzolini. http://www.dmdpdf.org

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
Imports System.Text 'Using text = System.Text;

Namespace DMD.org.dmdpdf.tokens

    '/**
    '  <summary>PDF serialization encoding [PDF:1.6:3.1].</summary>
    '  <remarks>PDF can be entirely represented using byte values corresponding to the visible
    '  printable subset of the ASCII character set, plus white space characters such as space, tab,
    '  carriage return, and line feed characters. However, a PDF file is not restricted to the ASCII
    '  character set; it can contain arbitrary 8-bit bytes.</remarks>
    '*/
    Public NotInheritable Class PdfEncoding
        Inherits Encoding

#Region "dynamic"
#Region "constructors"

        Friend Sub New()


        End Sub

#End Region

#Region "Interface"

        Public Overrides Function Decode(ByVal value As Byte()) As String
            Return Charset.ISO88591.GetString(value)
        End Function


        Public Overrides Function Decode(ByVal value As Byte(), ByVal index As Integer, ByVal length As Integer) As String
            Return Charset.ISO88591.GetString(value, index, length)
        End Function


        Public Overrides Function Encode(ByVal value As String) As Byte()
            Return Charset.ISO88591.GetBytes(value)
        End Function

#End Region
#End Region

    End Class

End Namespace
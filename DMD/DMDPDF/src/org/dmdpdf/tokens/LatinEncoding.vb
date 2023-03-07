'/*
'  Copyright 2012 Stefano Chizzolini. http://www.dmdpdf.org

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

Imports DMD.org.dmdpdf.util
Imports System

Namespace DMD.org.dmdpdf.tokens

    '/**
    '  <summary>Adobe standard Latin character set [PDF:1.7:D].</summary>
    '*/
    Public Class LatinEncoding
        Inherits Encoding

#Region "dynamic"
#Region "fields"

        '/**
        '  <summary>Code-to-Unicode map.</summary>
        '*/
        Protected _chars As BiDictionary(Of Integer, Char)

#End Region

#Region "Interface"

        Public Overrides Function Decode(ByVal value As Byte()) As String
            Return Decode(value, 0, value.Length)
        End Function

        Public Overrides Function Decode(ByVal value As Byte(), ByVal index As Integer, ByVal length As Integer) As String
            Dim stringChars As Char() = New Char(length - 1) {}
            Dim decodeLength As Integer = length + index
            For decodeIndex As Integer = index To decodeLength - 1
                stringChars(decodeIndex - index) = _chars(value(decodeIndex) And &HFF)
            Next
            Return New String(stringChars)
        End Function

        Public Overrides Function Encode(ByVal value As String) As Byte()
            If (value Is Nothing) Then value = ""
            Dim stringChars As Char() = value.ToCharArray()
            Dim stringBytes As Byte() = New Byte(stringChars.Length - 1) {}
            Dim length As Integer = stringChars.Length
            For index As Integer = 0 To length - 1
                Dim code As Integer = _chars.GetKey(stringChars(index))
                If (code = 0) Then 'TODO Then: verify whether 0 collides with valid code values.
                    Return Nothing
                End If
                stringBytes(index) = CByte(code)
            Next
            Return stringBytes
        End Function

#End Region
#End Region

    End Class

End Namespace
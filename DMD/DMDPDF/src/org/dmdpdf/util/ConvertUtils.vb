'/*
'  Copyright 2009-2010 Stefano Chizzolini. http://www.dmdpdf.org

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

Imports DMD.org.dmdpdf.util.io

Imports System
Imports System.Text

Namespace DMD.org.dmdpdf.util

    '/**
    '  <summary>Data convertion utility.</summary>
    '  <remarks>This class is a specialized adaptation from the original <a href="http://commons.apache.org/codec/">
    '  Apache Commons Codec</a> project, licensed under the <a href="http://www.apache.org/licenses/LICENSE-2.0">
    '  Apache License, Version 2.0</a>.</remarks>
    '*/
    Public Module ConvertUtils

#Region "Static"
#Region "fields"

        Private ReadOnly HexDigits As Char() = {"0"c, "1"c, "2"c, "3"c, "4"c, "5"c, "6"c, "7"c, "8"c, "9"c, "a"c, "b"c, "c"c, "d"c, "e"c, "f"c}
#End Region

#Region "Interface"
#Region "Public"

        Public Function ByteArrayToHex(ByVal data As Byte()) As String
            Dim dataLength As Integer = data.Length
            Dim result As Char() = New Char(dataLength * 2 - 1) {}
            Dim resultIndex As Integer = 0
            For dataIndex As Integer = 0 To dataLength - 1
                result(resultIndex) = HexDigits((&HF0 And data(dataIndex)) >> 4) : resultIndex += 1
                result(resultIndex) = HexDigits(&HF And data(dataIndex)) : resultIndex += 1
            Next
            Return New String(result)
        End Function

        Public Function ByteArrayToInt(ByVal data As Byte()) As Integer
            Return ByteArrayToInt(data, 0, ByteOrderEnum.BigEndian)
        End Function

        Public Function ByteArrayToInt(ByVal data As Byte(), ByVal index As Integer, ByVal byteOrder As ByteOrderEnum) As Integer
            Return ByteArrayToNumber(data, index, 4, byteOrder)
        End Function

        Public Function ByteArrayToNumber(ByVal data As Byte(), ByVal index As Integer, ByVal length As Integer, ByVal byteOrder As ByteOrderEnum) As Integer
            Dim value As Integer = 0
            length = CInt(System.Math.Min(length, data.Length - index))

            Dim endIndex As Integer = index + length
            For i As Integer = index To endIndex - 1
                If (byteOrder = ByteOrderEnum.LittleEndian) Then
                    value = value Or (data(i) And &HFF) << 8 * (i - index)
                Else
                    value = value Or (data(i) And &HFF) << 8 * (endIndex - i - 1)
                End If
            Next
            Return value
        End Function

        Public Function HexToByteArray(ByVal data As String) As Byte()
            Dim result As Byte()
            Dim dataLength As Integer = data.Length
            If ((dataLength Mod 2) <> 0) Then Throw New Exception("Odd number of characters.")
            result = New Byte(CInt(dataLength / 2) - 1) {}

            Dim dataIndex As Integer = 0
            Dim resultIndex As Integer = 0
            While (dataIndex < dataLength)
                result(resultIndex) = Byte.Parse(data(dataIndex).ToString() + data(dataIndex + 1).ToString(), System.Globalization.NumberStyles.HexNumber)
                dataIndex += 2
                resultIndex += 1
            End While
            Return result
        End Function

        Public Function IntToByteArray(ByVal data As Integer) As Byte()
            Return New Byte() {CByte(data >> 24), CByte(data >> 16), CByte(data >> 8), CByte(data)}
        End Function

        Public Function NumberToByteArray(ByVal data As Integer, ByVal length As Integer, ByVal byteOrder As ByteOrderEnum) As Byte()
            Dim result As Byte() = New Byte(length - 1) {}

            For index As Integer = 0 To length - 1
                'result(index) = CByte(data >> 8 * (byteOrder == ByteOrderEnum.LittleEndian ? index : length-index - 1));
                If (byteOrder = ByteOrderEnum.LittleEndian) Then
                    result(index) = CByte(data >> 8 * index)
                Else
                    result(index) = CByte((data >> 8 * (length - index - 1)) And &HFF)
                End If
            Next
            Return result
        End Function

        Public Function ToFloatArray(ByVal array As Double()) As Single()
            Dim result As Single() = New Single(array.Length - 1) {}

            Dim length As Integer = array.Length
            For index As Integer = 0 To length - 1
                result(index) = CSng(array(index))
            Next
            Return result
        End Function

#End Region
#End Region
#End Region

    End Module

End Namespace


'/*
'  Copyright 2009-2011 Stefano Chizzolini. http://www.dmdpdf.org

'  Contributors:
'    * J. James Jack, Ph.D., Senior Consultant at Symyx Technologies UK Ltd. (original
'      code developer, james{dot}jack{at}symyx{dot}com)
'    * Stefano Chizzolini (source code normalization to PDF Clown's conventions,
'      http://www.stefanochizzolini.it)

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
Imports System
Imports System.IO
Imports System.Text

Namespace DMD.org.dmdpdf.bytes.filters

    '/**
    '  <summary> ASCII base-85 filter [PDF:1.6:3.3.2].</summary>
    '*/

    <PDF(VersionEnum.PDF10)>
    Public NotInheritable Class ASCII85Filter
        Inherits Filter

#Region "static"

#Region "fields"

        'Prefix mark that identifies an encoded ASCII85 String.
        Private Const PrefixMark As String = "<~"

        'Suffix mark that identifies an encoded ASCII85 String.
        Private Const SuffixMark As String = "~>"

        'Add the Prefix And Suffix marks When encoding, And enforce their presence For decoding.
        Private Const EnforceMarks As Boolean = True

        'Maximum line length For encoded ASCII85 String; Set To zero For one unbroken line.
        Private Const LineLength As Integer = 75

        Private Const AsciiOffset As Integer = 33

        Private Shared ReadOnly Pow85 As UInteger() = {85 * 85 * 85 * 85, 85 * 85 * 85, 85 * 85, 85, 1}

#End Region

#Region "Interface"

#Region "Private"

        Private Shared Sub AppendChar(ByVal buffer As StringBuilder, ByVal data As Char, ByRef linePos As Integer)
            buffer.Append(data)
            linePos += 1
            If (LineLength > 0 AndAlso linePos >= LineLength) Then
                linePos = 0
                buffer.Append(vbLf) ''\n');
            End If
        End Sub

        Private Shared Sub AppendString(ByVal buffer As StringBuilder, ByVal data As String, ByRef linePos As Integer)
            If (LineLength > 0 AndAlso linePos + data.Length > LineLength) Then
                linePos = 0
                buffer.Append(vbLf) ''\n');
            Else
                linePos += data.Length
            End If
            buffer.Append(data)
        End Sub

        Private Shared Sub DecodeBlock(ByVal decodedBlock As Byte(), ByRef tuple As UInteger)
            DecodeBlock(decodedBlock, decodedBlock.Length, tuple)
        End Sub

        Private Shared Sub DecodeBlock(ByVal decodedBlock As Byte(), ByVal count As Integer, ByRef tuple As UInteger)
            For i As Integer = 0 To count - 1
                decodedBlock(i) = CByte(tuple >> 24 - (i * 8))
            Next
        End Sub

        Private Shared Sub EncodeBlock(ByVal encodedBlock As Byte(), ByVal buffer As StringBuilder, ByRef tuple As UInteger, ByRef linePos As Integer)
            EncodeBlock(encodedBlock, encodedBlock.Length, buffer, tuple, linePos)
        End Sub

        Private Shared Sub EncodeBlock(ByVal encodedBlock As Byte(), ByVal count As Integer, ByVal buffer As StringBuilder, ByRef tuple As UInteger, ByRef linePos As Integer)
            For i As Integer = encodedBlock.Length - 1 To 0 Step -1 '; i >= 0; i--)
                encodedBlock(i) = CByte((tuple Mod 85) + AsciiOffset)
                tuple = CUInt(tuple / 85)
            Next

            For i As Integer = 0 To count - 1
                AppendChar(buffer, Chr(encodedBlock(i)), linePos)
            Next
        End Sub

#End Region

#End Region

#End Region


#Region "dynamic"

#Region "constructors"

        Friend Sub New() 'ASCII85Filter
        End Sub

#End Region

#Region "Interface"

#Region "Public"

        Public Overrides Function Decode(ByVal data As Byte(), ByVal offset As Integer, ByVal length As Integer, ByVal parameters As PdfDictionary) As Byte()
            Dim decodedBlock As Byte() = New Byte(4 - 1) {}
            Dim encodedBlock As Byte() = New Byte(5 - 1) {}
            Dim Tuple As UInteger = 0

            Dim dataString As String = Encoding.ASCII.GetString(data).Trim()

            ' Stripping prefix And suffix...
            If (dataString.StartsWith(PrefixMark)) Then
                dataString = dataString.Substring(PrefixMark.Length)
            End If
            If (dataString.EndsWith(SuffixMark)) Then
                dataString = dataString.Substring(0, dataString.Length - SuffixMark.Length)
            End If

            Dim Stream As New MemoryStream()
            Dim count As Integer = 0
            Dim processChar As Boolean = False
            For Each dataChar As Char In dataString
                Select Case (dataChar)
                    Case "z"c
                        If (count <> 0) Then Throw New Exception("The character 'z' is invalid inside an ASCII85 block.")

                        decodedBlock(0) = 0
                        decodedBlock(1) = 0
                        decodedBlock(2) = 0
                        decodedBlock(3) = 0
                        Stream.Write(decodedBlock, 0, decodedBlock.Length)
                        processChar = False
                        'break;
                    Case ControlChars.Lf, ControlChars.Cr, ControlChars.Tab, ControlChars.NullChar, ControlChars.FormFeed, ControlChars.Back
                        'Case '\r':
                        '        Case '\t':
                        '        Case '\0':
                        '        Case '\f':
                        '        Case '\b':
                        processChar = False
                        'break;
                    Case Else
                        If (dataChar < "!"c OrElse dataChar > "u"c) Then Throw New Exception("Bad character '" & dataChar & "' found. ASCII85 only allows characters '!' to 'u'.")
                        processChar = True
                        'break;
                End Select

                If (processChar) Then
                    Tuple += (CUInt(Asc(dataChar) - AsciiOffset) * Pow85(count))
                    count += 1
                    If (count = encodedBlock.Length) Then
                        DecodeBlock(decodedBlock, Tuple)
                        Stream.Write(decodedBlock, 0, decodedBlock.Length)
                        Tuple = 0
                        count = 0
                    End If
                End If
            Next

            ' Bytes left over at the end?
            If (count <> 0) Then
                If (count = 1) Then Throw New Exception("The last block of ASCII85 data cannot be a single byte.")
                count -= 1
                Tuple += Pow85(count)
                DecodeBlock(decodedBlock, count, Tuple)
                For i As Integer = 0 To count - 1
                    Stream.WriteByte(decodedBlock(i))
                Next
            End If

            Return Stream.ToArray()
        End Function

        Public Overrides Function Encode(ByVal data As Byte(), ByVal offset As Integer, ByVal length As Integer, ByVal parameters As PdfDictionary) As Byte()
            Dim decodedBlock As Byte() = New Byte(4 - 1) {}
            Dim encodedBlock As Byte() = New Byte(5 - 1) {}

            Dim buffer As New StringBuilder(CInt(data.Length * (encodedBlock.Length / decodedBlock.Length)))
            Dim linePos As Integer = 0

            If (EnforceMarks) Then
                AppendString(buffer, PrefixMark, linePos)
            End If

            Dim count As Integer = 0
            Dim tuple As UInteger = 0
            For Each dataByte As Byte In data
                If (count >= decodedBlock.Length - 1) Then
                    tuple = tuple Or dataByte
                    If (tuple = 0) Then
                        AppendChar(buffer, "z"c, linePos)
                    Else
                        EncodeBlock(encodedBlock, buffer, tuple, linePos)
                    End If
                    tuple = 0
                    count = 0
                Else
                    tuple = tuple Or CUInt(dataByte << (24 - (count * 8)))
                    count += 1
                End If
            Next

            ' if we have some bytes left over at the end..
            If (count > 0) Then
                EncodeBlock(encodedBlock, count + 1, buffer, tuple, linePos)
            End If

            If (EnforceMarks) Then
                AppendString(buffer, SuffixMark, linePos)
            End If

            Return ASCIIEncoding.UTF8.GetBytes(Buffer.ToString())
    End Function

#End Region

#End Region

#End Region

    End Class


End Namespace


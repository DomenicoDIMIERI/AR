'/*
'  Copyright 2009-2011 Stefano Chizzolini. http://www.dmdpdf.org

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
Imports DMD.org.dmdpdf.documents.contents.objects
Imports DMD.org.dmdpdf.objects
Imports DMD.org.dmdpdf.tokens
Imports DMD.org.dmdpdf.util
Imports DMD.org.dmdpdf.util.math
Imports DMD.org.dmdpdf.util.parsers

Imports System
Imports System.Collections.Generic
Imports System.Globalization
Imports System.IO
Imports System.Text

Namespace DMD.org.dmdpdf.documents.contents.fonts

    '/**
    '  <summary>CMap parser [PDF:1.6:5.6.4;CMAP].</summary>
    '*/
    Friend NotInheritable Class CMapParser
        Inherits PostScriptParser

#Region "Static"
#Region "fields"

        Private Shared ReadOnly BeginBaseFontCharOperator As String = "beginbfchar"
        Private Shared ReadOnly BeginBaseFontRangeOperator As String = "beginbfrange"
        Private Shared ReadOnly BeginCIDCharOperator As String = "begincidchar"
        Private Shared ReadOnly BeginCIDRangeOperator As String = "begincidrange"

#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        Public Sub New(ByVal stream As IO.Stream)
            Me.New(New bytes.Buffer(stream))
        End Sub

        Public Sub New(ByVal stream As bytes.IInputStream)
            MyBase.New(stream)
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary> Parses the character-code-To-unicode mapping [PDF:1.6:5.9.1].</summary>
        '*/
        Public Function Parse() As IDictionary(Of ByteArray, Integer)
            Stream.Position = 0
            Dim codes As IDictionary(Of ByteArray, Integer) = New Dictionary(Of ByteArray, Integer)()
            '{
            Dim itemCount As Integer = 0
            While (MoveNext())
                Select Case (TokenType)
                    Case TokenTypeEnum.Keyword
                        Dim operator_ As String = CStr(Token)
                        If (operator_.Equals(BeginBaseFontCharOperator) OrElse
                            operator_.Equals(BeginCIDCharOperator)) Then
                            '/*
                            '  NOTE: The first element On Each line Is the input code Of the template font;
                            '  the second element Is the code Or name Of the character.
                            '*/
                            For itemIndex As Integer = 0 To itemCount - 1
                                MoveNext()
                                Dim inputCode As ByteArray = New ByteArray(ParseInputCode())
                                MoveNext()
                                codes(inputCode) = ParseUnicode()
                            Next
                        ElseIf (operator_.Equals(BeginBaseFontRangeOperator) OrElse
                                operator_.Equals(BeginCIDRangeOperator)) Then
                            '/*
                            '  NOTE: The first And second elements In Each line are the beginning And
                            '  ending valid input codes For the template font; the third element Is
                            '  the beginning character code For the range.
                            '*/
                            For itemIndex As Integer = 0 To itemCount - 1
                                ' 1. Beginning input code.
                                MoveNext()
                                Dim beginInputCode As Byte() = ParseInputCode()
                                ' 2. Ending input code.
                                MoveNext()
                                Dim endInputCode As Byte() = ParseInputCode()
                                ' 3. Character codes.
                                MoveNext()
                                Select Case (TokenType)
                                    Case TokenTypeEnum.ArrayBegin
                                        Dim inputCode As Byte() = beginInputCode
                                        While (MoveNext() AndAlso
                                            TokenType <> TokenTypeEnum.ArrayEnd)
                                            codes(New ByteArray(inputCode)) = ParseUnicode()
                                            OperationUtils.Increment(inputCode)
                                        End While
                                        'break;
                                    Case Else
                                        Dim inputCode As Byte() = beginInputCode
                                        Dim charCode As Integer = ParseUnicode()
                                        Dim endCharCode As Integer = charCode + (ConvertUtils.ByteArrayToInt(endInputCode) - ConvertUtils.ByteArrayToInt(beginInputCode))
                                        While (True)
                                            codes(New ByteArray(inputCode)) = charCode
                                            If (charCode = endCharCode) Then Exit While
                                            OperationUtils.Increment(inputCode)
                                            charCode += 1
                                        End While
                                        'break;
                                End Select
                            Next
                        End If

                        'break;
                    Case TokenTypeEnum.Integer
                        itemCount = CInt(Token)
                        'break;
                End Select
            End While
            '}

            Return codes
        End Function

#End Region

#Region "private"
        '/**
        '  <summary> Converts the current token into its input code value.</summary>
        '*/
        Private Function ParseInputCode() As Byte()
            Return ConvertUtils.HexToByteArray(CStr(Token))
        End Function

        '/**
        '  <summary> Converts the current token into its Unicode value.</summary>
        '*/
        Private Function ParseUnicode() As Integer
            Select Case (TokenType)
                Case TokenTypeEnum.Hex   ' Character code in hexadecimal format.
                    Return Int32.Parse(CStr(Token), NumberStyles.HexNumber)
                Case TokenTypeEnum.Integer   ' Character code in plain format.
                    Return CInt(Token)
                Case TokenTypeEnum.Name ' Character name.
                    Return GlyphMapping.NameToCode(CStr(Token)).Value
                Case Else
                    Throw New Exception("Hex string, integer or name expected instead of " & TokenType.ToString)
            End Select
        End Function

#End Region
#End Region
#End Region

    End Class

End Namespace
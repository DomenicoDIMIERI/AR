'/*
'  Copyright 2011-2012 Stefano Chizzolini. http://www.dmdpdf.org

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
Imports DMD.org.dmdpdf.objects
Imports DMD.org.dmdpdf.util.parsers

Imports System

Namespace DMD.org.dmdpdf.tokens

    '/**
    '  <summary>Base PDF parser [PDF:1.7:3.2].</summary>
    '*/
    Public Class BaseParser
        Inherits PostScriptParser

#Region "dynamic"
#Region "constructors"

        Protected Sub New(ByVal stream As IInputStream)
            MyBase.New(stream)
        End Sub

        Protected Sub New(ByVal data As Byte())
            MyBase.New(data)
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public Overrides Function MoveNext() As Boolean
            Dim moved As Boolean = MyBase.MoveNext()
            While (moved)
                Dim tokenType As TokenTypeEnum = Me.TokenType
                '; // Comments are ignored.
                If (tokenType = TokenTypeEnum.Comment) Then
                    moved = MyBase.MoveNext()
                    Continue While
                End If

                If (tokenType = TokenTypeEnum.Literal) Then
                    Dim literalToken As String = CStr(Token)
                    If (literalToken.StartsWith(Keyword.DatePrefix)) Then ' Date.
                        '/*
                        '  NOTE: Dates are a weak extension to the PostScript language.
                        '*/
                        Try
                            Me.Token = PdfDate.ToDate(literalToken)
                        Catch ex As ParseException
                            ' NOOP: gently degrade to a common literal. 
                        End Try
                    End If
                End If
                Exit While 'break;
            End While
            Return moved
        End Function

        '/**
        '  <summary>Parses the current PDF object [PDF:1.6:3.2].</summary>
        '*/
        Public Overridable Function ParsePdfObject() As PdfDataObject
            Select Case (Me.TokenType)
                Case TokenTypeEnum.Integer : Return PdfInteger.Get(CInt(Me.Token))
                Case TokenTypeEnum.Name : Return New PdfName(CStr(Me.Token), True)
                Case TokenTypeEnum.DictionaryBegin
                    Dim dictionary As PdfDictionary = New PdfDictionary()
                    dictionary.Updateable = False
                    While (True)
                        ' Key.
                        MoveNext()
                        If (TokenType = TokenTypeEnum.DictionaryEnd) Then Exit While
                        Dim key As PdfName = CType(ParsePdfObject(), PdfName)
                        ' Value.
                        MoveNext()
                        Dim value As PdfDirectObject = CType(ParsePdfObject(), PdfDirectObject)
                        ' Add the current entry to the dictionary!
                        dictionary(key) = value
                    End While
                    dictionary.Updateable = True
                    Return dictionary
                Case TokenTypeEnum.ArrayBegin
                    Dim array As PdfArray = New PdfArray()
                    array.Updateable = False
                    While (True)
                        ' Value.
                        MoveNext()
                        If (TokenType = TokenTypeEnum.ArrayEnd) Then Exit While
                        ' Add the current item to the array!
                        array.Add(CType(ParsePdfObject(), PdfDirectObject))
                    End While
                    array.Updateable = True
                    Return array
                Case TokenTypeEnum.Literal
                    If (TypeOf (Me.Token) Is DateTime) Then
                        Return PdfDate.Get(CType(Me.Token, DateTime))
                    Else
                        Return New PdfTextString(Encoding.Pdf.Encode(CStr(Me.Token)))
                    End If
                Case TokenTypeEnum.Hex
                    Return New PdfTextString(CStr(Me.Token), PdfString.SerializationModeEnum.Hex)
                Case TokenTypeEnum.Real : Return PdfReal.Get(CDbl(Me.Token))
                Case TokenTypeEnum.Boolean : Return PdfBoolean.Get(CBool(Me.Token))
                Case TokenTypeEnum.Null : Return Nothing
                Case Else
                    Throw New Exception("Unknown type: " & Me.TokenType)
            End Select
        End Function

        '/**
        '  <summary>Parses a PDF object after moving to the given token offset.</summary>
        '  <param name="offset">Number of tokens to skip before reaching the intended one.</param>
        '  <seealso cref="ParsePdfObject()"/>
        '*/
        Public Function ParsePdfObject(ByVal offset As Integer) As PdfDataObject
            Me.MoveNext(offset)
            Return Me.ParsePdfObject()
        End Function

#End Region
#End Region
#End Region

    End Class

End Namespace


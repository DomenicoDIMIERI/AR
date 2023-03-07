'/*
'  Copyright 2011 Stefano Chizzolini. http://www.dmdpdf.org

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

'  You should have received a copy of the GNU Lesser General Public License along with Me
'  Program (see README files); if not, go to the GNU website (http://www.gnu.org/licenses/).

'  Redistribution and use, with or without modification, are permitted provided that such
'  redistributions retain the above copyright notice, license and disclaimer, along with
'  Me list of conditions.
'*/

Imports DMD.org.dmdpdf.bytes
Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects
Imports DMD.org.dmdpdf.util.parsers

Imports System
Imports System.Globalization
Imports System.IO
Imports System.Text

Namespace DMD.org.dmdpdf.tokens

    '/**
    '  <summary>PDF file parser [PDF:1.7:3.2,3.4].</summary>
    '*/
    Public NotInheritable Class FileParser
        Inherits BaseParser

#Region "types"

        Public Structure Reference

            Public ReadOnly _GenerationNumber As Integer
            Public ReadOnly _ObjectNumber As Integer

            Friend Sub New(ByVal objectNumber As Integer, ByVal generationNumber As Integer)
                Me._ObjectNumber = objectNumber
                Me._GenerationNumber = generationNumber
            End Sub
        End Structure

#End Region

#Region "Static"
#Region "fields"

        Private Shared ReadOnly EOFMarkerChunkSize As Integer = 1024 ' [PDF:1.6:H0.3.18].

#End Region
#End Region

#Region "dynamic"
#Region "fields"

        Private _file As files.File

#End Region

#Region "constructors"

        Friend Sub New(ByVal Stream As IInputStream, ByVal file As files.File)
            MyBase.New(Stream)
            Me._file = file
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public Overrides Function MoveNext() As Boolean
            Dim moved As Boolean = MyBase.MoveNext()
            If (moved) Then
                Select Case (TokenType)
                    Case TokenTypeEnum.Integer
                        '/*
                        '  NOTE: We need to verify whether indirect reference pattern Is applicable
                        '  ref :=  { int int 'R' }
                        '*/
                        Dim Stream As IInputStream = Me.Stream
                        Dim baseOffset As Long = Stream.Position ' Backs up the recovery position.

                        ' 1. Object number.
                        Dim objectNumber As Integer = CInt(Token)
                        ' 2. Generation number.
                        MyBase.MoveNext()
                        If (TokenType = TokenTypeEnum.Integer) Then
                            Dim generationNumber As Integer = CInt(Token)
                            ' 3. Reference keyword.
                            MyBase.MoveNext()
                            If (TokenType = TokenTypeEnum.Keyword AndAlso
                                Token.Equals(Keyword.Reference)) Then
                                Token = New Reference(objectNumber, generationNumber)
                            End If
                        End If
                        If (Not (TypeOf (Token) Is Reference)) Then
                            ' Rollback!
                            Stream.Seek(baseOffset)
                            Token = objectNumber
                            TokenType = TokenTypeEnum.Integer
                        End If
                        ' break;
                End Select

            End If
            Return moved
        End Function

        Public Overrides Function ParsePdfObject() As PdfDataObject
            Select Case (TokenType)
                Case TokenTypeEnum.Keyword
                    If (TypeOf (Token) Is Reference) Then
                        Return New PdfReference(CType(Token, Reference), _file)
                    End If
                    'break;
            End Select

            Dim PdfObject As PdfDataObject = MyBase.ParsePdfObject()
            If (TypeOf (PdfObject) Is PdfDictionary) Then
                Dim Stream As IInputStream = Me.Stream
                Dim oldOffset As Integer = CInt(Stream.Position)
                MoveNext()
                ' Is Me dictionary the header of a stream object [PDF:1.6:3.2.7]?
                If ((TokenType = TokenTypeEnum.Keyword) AndAlso
                        Token.Equals(Keyword.BeginStream)) Then
                    Dim streamHeader As PdfDictionary = CType(PdfObject, PdfDictionary)

                    ' Keep track of current position!
                    '/*
                    '  NOTE: Indirect reference resolution is an outbound call which affects the stream pointer position,
                    '  so we need to recover our current position after it returns.
                    '*/
                    Dim position As Long = Stream.Position
                    ' Get the stream length!
                    Dim length As Integer = CType(streamHeader.Resolve(PdfName.Length), PdfInteger).IntValue
                    ' Move to the stream data beginning!
                    Stream.Seek(position)
                    SkipEOL()

                    ' Copy the stream data to the instance!
                    Dim data As Byte() = New Byte(length - 1) {}
                    Stream.Read(data)

                    MoveNext() ' Postcondition (last token should be 'endstream' keyword).

                    Dim streamType As Object = streamHeader(PdfName.Type)
                    If (PdfName.ObjStm.Equals(streamType)) Then ' ObjectThen stream [PDF:1.6:3.4.6].
                        Return New ObjectStream(streamHeader, New bytes.Buffer(data))
                    ElseIf (PdfName.XRef.Equals(streamType)) Then ' Cross - referenceThen Then stream [PDF:1.6:3.4.7].
                        Return New XRefStream(streamHeader, New bytes.Buffer(data))
                    Else ' Generic stream.
                        Return New PdfStream(streamHeader, New bytes.Buffer(data))
                    End If ' Stand-alone dictionary.
                Else
                    Stream.Seek(oldOffset)
                End If 'Restores postcondition(last token should be the dictionary End).
            End If
            Return PdfObject
        End Function

        '/**
        '  <summary>Retrieves the PDF version of the file [PDF:1.6:3.4.1].</summary>
        '*/
        Public Function RetrieveVersion() As String
            Dim stream As IInputStream = Me.Stream
            stream.Seek(0)
            Dim header As String = stream.ReadString(10)
            If (Not header.StartsWith(Keyword.BOF)) Then
                Throw New ParseException("PDF header not found.", stream.Position)
            End If
            Return header.Substring(Keyword.BOF.Length, 3)
        End Function

        '/**
        '  <summary>Retrieves the starting position of the last xref-table section [PDF:1.6:3.4.4].</summary>
        '*/
        Public Function RetrieveXRefOffset() As Long
            Dim stream As IInputStream = Me.Stream
            Dim streamLength As Long = stream.Length
            Dim chunkSize As Integer = CInt(Math.Min(streamLength, EOFMarkerChunkSize))

            ' Move back before 'startxref' keyword!
            Dim position As Long = streamLength - chunkSize
            stream.Seek(position)

            ' Get 'startxref' keyword position!
            Dim index As Integer = stream.ReadString(chunkSize).LastIndexOf(Keyword.StartXRef)
            If (index < 0) Then
                Throw New ParseException("'" & Keyword.StartXRef & "' keyword not found.", stream.Position)
            End If

            ' Go past the startxref keyword!
            stream.Seek(position + index)
            MoveNext()

            ' Go to the xref offset!
            MoveNext()
            If (TokenType <> TokenTypeEnum.Integer) Then
                Throw New ParseException("'" & Keyword.StartXRef & "' value invalid.", stream.Position)
            End If

            Return CInt(Token)
        End Function

#End Region
#End Region
#End Region

    End Class

End Namespace
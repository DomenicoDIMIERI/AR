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
Imports DMD.org.dmdpdf.documents.contents.objects
Imports DMD.org.dmdpdf.objects
Imports DMD.org.dmdpdf.tokens

Imports System
Imports System.Collections.Generic

Namespace DMD.org.dmdpdf.documents.contents.tokens

    '/**
    '  <summary>Content stream parser [PDF:1.6:3.7.1].</summary>
    '*/
    Public NotInheritable Class ContentParser
        Inherits BaseParser

#Region "dynamic"
#Region "constructors"

        Friend Sub New(ByVal stream As bytes.IInputStream)
            MyBase.New(stream)
        End Sub

        Public Sub New(ByVal data As Byte())
            MyBase.New(data)
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary>Parses the Next content Object [PDF:1.6:4.1].</summary>
        '*/
        Public Function ParseContentObject() As ContentObject
            Dim operation As Operation = ParseOperation()
            If (TypeOf (operation) Is PaintXObject) Then 'External Then Object.
                Return New XObject(CType(operation, PaintXObject))
            ElseIf (TypeOf (operation) Is PaintShading) Then ' Shading.
                Return New Shading(CType(operation, PaintShading))
            ElseIf (
                    TypeOf (operation) Is BeginSubpath OrElse
                    TypeOf (operation) Is DrawRectangle
                    ) Then ' Path.
                Return ParsePath(operation)
            ElseIf (TypeOf (operation) Is BeginText) Then ' Text.
                Return New Text(ParseContentObjects())
            ElseIf (TypeOf (operation) Is SaveGraphicsState) Then ' Local Then graphics state.
                Return New LocalGraphicsState(ParseContentObjects())
            ElseIf (TypeOf (operation) Is BeginMarkedContent) Then ' Marked - content Then sequence.
                Return New MarkedContent(CType(operation, BeginMarkedContent), ParseContentObjects())
            ElseIf (TypeOf (operation) Is BeginInlineImage) Then ' Inline Then image.
                Return ParseInlineImage()
            Else ' Single operation.
                Return operation
            End If
        End Function

        '/**
        '  <summary>Parses the Next content objects.</summary>
        '*/
        Public Function ParseContentObjects() As IList(Of ContentObject)
            Dim contentObjects As List(Of ContentObject) = New List(Of ContentObject)()
            While (MoveNext())
                Dim contentObject As ContentObject = ParseContentObject()
                ' Multiple-operation graphics object end?
                'If (TypeOf (contentObject) Is EndText // Text.
                '  || contentObject Is RestoreGraphicsState // Local graphics state.
                '  || contentObject Is EndMarkedContent // End marked-content sequence.
                '  || contentObject Is EndInlineImage) // Inline image.
                If (TypeOf (contentObject) Is EndText OrElse
                TypeOf (contentObject) Is RestoreGraphicsState OrElse
                TypeOf (contentObject) Is EndMarkedContent OrElse
                TypeOf (contentObject) Is EndInlineImage) Then
                    Return contentObjects
                End If
                contentObjects.Add(contentObject)
            End While
            Return contentObjects
        End Function

        '/**
        '  <summary> Parses the Next operation.</summary>
        '*/
        Public Function ParseOperation() As Operation
            Dim operator_ As String = Nothing
            Dim operands As List(Of PdfDirectObject) = New List(Of PdfDirectObject)()
            ' Parsing the operation parts...
            Do
                Select Case (TokenType)
                    Case TokenTypeEnum.Keyword : operator_ = CStr(Token)
                    Case Else : operands.Add(CType(ParsePdfObject(), PdfDirectObject))
                End Select
            Loop While (operator_ Is Nothing AndAlso MoveNext())
            Return Operation.Get(operator_, operands)
        End Function

        Public Overrides Function ParsePdfObject() As PdfDataObject
            Select Case (TokenType)
                Case TokenTypeEnum.Literal
                    If (TypeOf (Token) Is String) Then
                        Return New PdfString(Encoding.Pdf.Encode(CStr(Token)), PdfString.SerializationModeEnum.Literal)
                    End If
                Case TokenTypeEnum.Hex
                    Return New PdfString(CStr(Token), PdfString.SerializationModeEnum.Hex)
            End Select
            Return MyBase.ParsePdfObject()
        End Function

#End Region

#Region "Private"

        Private Function ParseInlineImage() As InlineImage
            '/*
            '  NOTE:       Inline images use a peculiar syntax that's an exception to the usual rule
            '              that the data In a content stream Is interpreted according To the standard PDF syntax
            '  For objects.
            '*/
            Dim header As InlineImageHeader
            '{
            Dim operands As List(Of PdfDirectObject) = New List(Of PdfDirectObject)()
            ' Parsing the image entries...
            While (MoveNext() AndAlso
                    TokenType <> TokenTypeEnum.Keyword) ' Not keyword (i.e. end at image data beginning (ID operator)).
                operands.Add(CType(ParsePdfObject(), PdfDirectObject))
            End While
            header = New InlineImageHeader(operands)
            '}

            Dim body As InlineImageBody
            '{
            Dim Stream As bytes.IInputStream = Me.Stream
            MoveNext()
            Dim Data As bytes.Buffer = New bytes.Buffer()
            Dim prevByte As Byte = 0
            While (True)
                Dim curByte As Byte = CByte(Stream.ReadByte())
                If (prevByte = Asc("E"c) AndAlso curByte = Asc("I"c)) Then Exit While
                prevByte = curByte
                Data.Append(prevByte)
            End While
            body = New InlineImageBody(Data)
            '}

            Return New InlineImage(header, body)
        End Function

        Private Function ParsePath(ByVal beginOperation As Operation) As Path
            '/*
            '  NOTE: Paths do Not have an explicit end operation, so we must infer it
            '  looking for the first non-painting operation.
            '*/
            Dim operations As IList(Of ContentObject) = New List(Of ContentObject)()
            '{
            operations.Add(beginOperation)
            Dim position As Long = Me.Position
            Dim closeable As Boolean = False
            While (MoveNext())
                Dim operation As Operation = ParseOperation()
                ' Multiple-operation graphics object closeable?
                If (TypeOf (operation) Is PaintPath) Then ' PaintingThen operation.
                    closeable = True
                ElseIf (closeable) Then ' Past Then End (first non-painting operation).
                    Seek(position) ' Rolls back to the last path-related operation.
                    Exit While
                End If
                operations.Add(operation)
                position = Me.Position
            End While
            '}
            Return New Path(operations)
        End Function

#End Region
#End Region
#End Region

    End Class

End Namespace
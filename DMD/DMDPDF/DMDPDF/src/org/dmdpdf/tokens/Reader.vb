'/*
'  Copyright 2006-2011 Stefano Chizzolini. http://www.dmdpdf.org

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
Imports DMD.org.dmdpdf.util.collections.generic
Imports DMD.org.dmdpdf.util.parsers

Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq

Namespace DMD.org.dmdpdf.tokens

    '/**
    '  <summary>PDF file reader.</summary>
    '*/
    Public NotInheritable Class Reader
        Implements IDisposable

#Region "types"

        Public NotInheritable Class FileInfo

            Private _trailer As PdfDictionary
            Private _version As Version
            Private _xrefEntries As SortedDictionary(Of Integer, XRefEntry)

            Friend Sub New(ByVal version As Version, ByVal trailer As PdfDictionary, ByVal xrefEntries As SortedDictionary(Of Integer, XRefEntry))
                Me._version = version
                Me._trailer = trailer
                Me._xrefEntries = xrefEntries
            End Sub

            Public ReadOnly Property Trailer As PdfDictionary
                Get
                    Return Me._trailer
                End Get
            End Property

            Public ReadOnly Property Version As Version
                Get
                    Return Me._version
                End Get
            End Property

            Public ReadOnly Property XrefEntries As SortedDictionary(Of Integer, XRefEntry)
                Get
                    Return Me._xrefEntries
                End Get
            End Property

        End Class

#End Region

#Region "dynamic"
#Region "fields"

        Private _parser As FileParser

#End Region

#Region "constructors"

        Friend Sub New(ByVal Stream As IInputStream, ByVal file As files.File)
            Me._parser = New FileParser(Stream, file)
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public Overrides Function GetHashCode() As Integer
            Return Me._parser.GetHashCode()
        End Function

        Public ReadOnly Property Parser As FileParser
            Get
                Return Me._parser
            End Get
        End Property

        '/**
        '  <summary> Retrieves the file information.</summary>
        '*/
        Public Function ReadInfo() As FileInfo
            'TODO:hybrid xref table/stream
            Dim version As Version = Version.Get(_parser.RetrieveVersion())
            Dim trailer As PdfDictionary = Nothing
            Dim xrefEntries As SortedDictionary(Of Integer, XRefEntry) = New SortedDictionary(Of Integer, XRefEntry)()
            Dim sectionOffset As Long = _parser.RetrieveXRefOffset()
            While (sectionOffset > -1)
                ' Move to the start of the xref section!
                _parser.Seek(sectionOffset)

                Dim sectionTrailer As PdfDictionary
                If (_parser.GetToken(1).Equals(Keyword.XRef)) Then ' XRef - table Then section.
                    ' Looping sequentially across the subsections inside the current xref-table section...
                    While (True)
                        '/*
                        '  NOTE: Each iteration of Me block represents the scanning of one subsection.
                        '  We get its bounds (first And last object numbers within its range) And then collect
                        '  its entries.
                        '*/
                        '// 1.First object number.
                        _parser.MoveNext()
                        If (
                            (_parser.TokenType = PostScriptParser.TokenTypeEnum.Keyword) AndAlso
                             _parser.Token.Equals(Keyword.Trailer)
                        ) Then ' XRef-table section ended.
                            'break;
                            Exit While
                        ElseIf (_parser.TokenType <> PostScriptParser.TokenTypeEnum.Integer) Then
                            Throw New ParseException("Neither object number of the first object in Me xref subsection nor end of xref section found.", _parser.Position)
                        End If

                        ' Get the object number of the first object in Me xref-table subsection!
                        Dim startObjectNumber As Integer = CInt(_parser.Token)

                        ' 2. Last object number.
                        _parser.MoveNext()
                        If (_parser.TokenType <> PostScriptParser.TokenTypeEnum.Integer) Then
                            Throw New ParseException("Number of entries in Me xref subsection not found.", _parser.Position)
                        End If

                        ' Get the object number of the last object in Me xref-table subsection!
                        Dim endObjectNumber As Integer = CInt(_parser.Token) + startObjectNumber

                        ' 3. XRef-table subsection entries.
                        For index As Integer = startObjectNumber To endObjectNumber - 1
                            If (xrefEntries.ContainsKey(index)) Then ' Already-defined entry.
                                ' Skip to the next entry!
                                _parser.MoveNext(3)
                                Continue For
                            End If

                            ' Get the indirect object offset!
                            Dim offset As Integer = CInt(_parser.GetToken(1))
                            ' Get the object generation number!
                            Dim generation As Integer = CInt(_parser.GetToken(1))
                            ' Get the usage tag!
                            Dim usage As XRefEntry.UsageEnum
                            Dim usageToken As String = CStr(_parser.GetToken(1))
                            If (usageToken.Equals(Keyword.InUseXrefEntry)) Then
                                usage = XRefEntry.UsageEnum.InUse
                            ElseIf (usageToken.Equals(Keyword.FreeXrefEntry)) Then
                                usage = XRefEntry.UsageEnum.Free
                            Else
                                Throw New ParseException("Invalid xref entry.", _parser.Position)
                            End If

                            ' Define entry!
                            xrefEntries(index) = New XRefEntry(index, generation, offset, usage)
                        Next
                    End While

                    ' Get the previous trailer!
                    sectionTrailer = CType(_parser.ParsePdfObject(1), PdfDictionary)
                Else ' XRef-stream section.
                    Dim stream As XRefStream = CType(_parser.ParsePdfObject(3), XRefStream) ' Gets the xref stream skipping the indirect-object header.
                    ' XRef-stream subsection entries.
                    For Each xrefEntry As XRefEntry In stream.Values
                        If (xrefEntries.ContainsKey(xrefEntry.Number)) Then ' Already - defined Then entry.
                            Continue For
                        End If

                        ' Define entry!
                        xrefEntries(xrefEntry.Number) = xrefEntry
                    Next

                    ' Get the previous trailer!
                    sectionTrailer = Stream.Header
                End If

                If (trailer Is Nothing) Then
                    trailer = sectionTrailer
                End If

                ' Get the previous xref-table section's offset!
                Dim prevXRefOffset As PdfInteger = CType(sectionTrailer(PdfName.Prev), PdfInteger)
                If (prevXRefOffset IsNot Nothing) Then
                    sectionOffset = prevXRefOffset.IntValue
                Else
                    sectionOffset = -1
                End If
            End While

            Return New FileInfo(Version, trailer, xrefEntries)
        End Function

#Region "IDisposable"

        Public Sub Dispose() Implements IDisposable.Dispose
            If (Me._parser IsNot Nothing) Then
                Me._parser.Dispose()
                Me._parser = Nothing
            End If
            'GC.SuppressFinalize(Me)
        End Sub

#End Region
#End Region
#End Region
#End Region

    End Class

End Namespace
'/*
'  Copyright 2006-2012 Stefano Chizzolini. http://www.dmdpdf.org

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
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects

Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Text

Namespace DMD.org.dmdpdf.tokens

    '/**
    '  <summary>PDF file writer implementing classic cross-reference table [PDF:1.6:3.4.3].</summary>
    '*/
    Friend NotInheritable Class PlainWriter
        Inherits Writer

#Region "Static"
#Region "fields"

        Private Shared ReadOnly _TrailerChunk As Byte() = Encoding.Pdf.Encode(Keyword.Trailer & Symbol.LineFeed)
        Private Shared ReadOnly _XRefChunk As String = Keyword.XRef & Symbol.LineFeed
        Private Shared ReadOnly _XRefEOLChunk As String = "" & Symbol.CarriageReturn & Symbol.LineFeed

        Private Const _XRefGenerationFormat As String = "00000"
        Private Const _XRefOffsetFormat As String = "0000000000"

#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        Friend Sub New(ByVal file As files.File, ByVal stream As IOutputStream)
            MyBase.New(file, stream)
        End Sub

#End Region

#Region "Interface"
#Region "Protected"

        Protected Overrides Sub WriteIncremental()
            ' 1. Original content (head, body And previous trailer).
            Dim parser As FileParser = File.Reader.Parser
            Stream.Write(parser.Stream)

            ' 2. Body update (modified indirect objects insertion).
            Dim xrefSize As Integer = File.IndirectObjects.Count
            Dim xrefBuilder As StringBuilder = New StringBuilder(_XRefChunk)
            '{
            '/*
            '  NOTE: Incremental xref table comprises multiple sections
            '  each one composed by multiple subsections; this update
            '  adds a new section.
            '*/
            Dim xrefSubBuilder As StringBuilder = New StringBuilder() ' Xref-table subsection builder.
            Dim xrefSubCount As Integer = 0 ' Xref-table subsection counter.
            Dim prevKey As Integer = 0 ' Previous-entry object number.
            For Each indirectObjectEntry As KeyValuePair(Of Integer, PdfIndirectObject) In File.IndirectObjects.ModifiedObjects
                ' Is the object in the current subsection?
                '/*
                '  NOTE: To belong to the current subsection, the object entry MUST be contiguous with the
                '  previous (condition 1) or the iteration has to have been just started (condition 2).
                '*/
                If (indirectObjectEntry.Key - prevKey = 1 OrElse
                         prevKey = 0) Then ' Current subsection continues.
                    xrefSubCount += 1
                Else ' Current subsection terminates.
                    ' End current subsection!
                    AppendXRefSubsection(xrefBuilder, prevKey - xrefSubCount + 1, xrefSubCount, xrefSubBuilder)
                    ' Begin next subsection!
                    xrefSubBuilder.Length = 0
                    xrefSubCount = 1
                End If

                prevKey = indirectObjectEntry.Key

                ' Current entry insertion.
                If (indirectObjectEntry.Value.IsInUse()) Then 'In-use entry.
                    ' Add in-use entry!
                    AppendXRefEntry(xrefSubBuilder, indirectObjectEntry.Value.Reference, Stream.Length)
                    ' Add in-use entry content!
                    indirectObjectEntry.Value.WriteTo(Stream, File)
                Else ' Free entry.
                    '// Add free entry!
                    '/*
                    '  NOTE: We purposely neglect the linked list of free entries (see IndirectObjects.remove(int)),
                    '  so that this entry links directly back to object number 0, having a generation number of 65535
                    '  (not reusable) [PDF:1.6:3.4.3].
                    '*/
                    AppendXRefEntry(xrefSubBuilder, indirectObjectEntry.Value.Reference, 0)
                End If
            Next

            ' End last subsection!
            AppendXRefSubsection(xrefBuilder, prevKey - xrefSubCount + 1, xrefSubCount, xrefSubBuilder)


            ' 3. XRef-table last section.
            Dim startxref As Long = Stream.Length
            Stream.Write(xrefBuilder.ToString())

            ' 4. Trailer.
            WriteTrailer(startxref, xrefSize, parser)
        End Sub

        Protected Overrides Sub WriteLinearized()
            Throw New NotImplementedException()
        End Sub


        Protected Overrides Sub WriteStandard()
            ' 1. Header [PDF:1.6:3.4.1].
            WriteHeader()

            ' 2. Body [PDF:1.6:3.4.2].
            Dim xrefSize As Integer = File.IndirectObjects.Count
            Dim xrefBuilder As StringBuilder = New StringBuilder(_XRefChunk)

            '{

            '/*
            '  NOTE: A standard xref table comprises just one section composed by just one subsection.
            '  NOTE: As xref-table free entries MUST be arrayed as a linked list,
            '  it's needed to cache intermingled in-use entries in order to properly render
            '  the object number of the next free entry inside the previous one.
            '*/
            AppendXRefSubsectionIndexer(xrefBuilder, 0, xrefSize)

            Dim xrefInUseBlockBuilder As StringBuilder = New StringBuilder()
            Dim indirectObjects As IndirectObjects = File.IndirectObjects
            Dim freeReference As PdfReference = indirectObjects(0).Reference ' Initialized to the first free entry.
            For index As Integer = 1 To xrefSize - 1
                ' Current entry insertion.
                Dim indirectObject As PdfIndirectObject = indirectObjects(index)
                If (indirectObject.IsInUse()) Then ' In-use entry.
                    ' Add in-use entry!
                    AppendXRefEntry(xrefInUseBlockBuilder, indirectObject.Reference, Stream.Length)
                    ' Add in-use entry content!
                    indirectObject.WriteTo(Stream, File)
                Else ' Free entry.
                    ' Add free entry!
                    AppendXRefEntry(xrefBuilder, freeReference, index)
                    ' End current block!
                    xrefBuilder.Append(xrefInUseBlockBuilder)
                    ' Initialize next block!
                    xrefInUseBlockBuilder.Length = 0
                    freeReference = indirectObject.Reference
                End If
            Next

            ' Add last free entry!
            AppendXRefEntry(xrefBuilder, freeReference, 0)

            ' End last block!
            xrefBuilder.Append(xrefInUseBlockBuilder)

            '}

            ' 3. XRef table (unique section) [PDF:1.6:3.4.3].
            Dim startxref As Long = Stream.Length
            Stream.Write(xrefBuilder.ToString())

            ' 4. Trailer [PDF:1.6:3.4.4].
            WriteTrailer(startxref, xrefSize, Nothing)
        End Sub

#End Region

#Region "Private"

        Private Function AppendXRefEntry(ByVal xrefBuilder As StringBuilder, ByVal reference As PdfReference, ByVal offset As Long) As StringBuilder
            Dim usage As String
            Select Case (reference.IndirectObject.XrefEntry.Usage)
                Case XRefEntry.UsageEnum.Free
                    usage = Keyword.FreeXrefEntry
                    'break;
                Case XRefEntry.UsageEnum.InUse
                    usage = Keyword.InUseXrefEntry
                    'break;
                Case Else ' Should NEVER happen.
                    Throw New NotSupportedException()
            End Select
            Return xrefBuilder.Append(offset.ToString(_XRefOffsetFormat)).Append(Symbol.Space).Append(reference.GenerationNumber.ToString(_XRefGenerationFormat)).Append(Symbol.Space).Append(usage).Append(_XRefEOLChunk)
        End Function

        '/**
        '  <summary>Appends the cross-reference subsection to the specified builder.</summary>
        '  <param name="xrefBuilder">Target builder.</param>
        '  <param name="firstObjectNumber">Object number of the first object in the subsection.</param>
        '  <param name="entryCount">Number of entries in the subsection.</param>
        '  <param name="xrefSubBuilder">Cross-reference subsection entries.</param>
        '*/
        Private Function AppendXRefSubsection(ByVal xrefBuilder As StringBuilder, ByVal firstObjectNumber As Integer, ByVal entryCount As Integer, ByVal xrefSubBuilder As StringBuilder) As StringBuilder
            Return AppendXRefSubsectionIndexer(xrefBuilder, firstObjectNumber, entryCount).Append(xrefSubBuilder)
        End Function


        '/**
        '  <summary>Appends the cross-reference subsection indexer to the specified builder.</summary>
        '  <param name="xrefBuilder">Target builder.</param>
        '  <param name="firstObjectNumber">Object number of the first object in the subsection.</param>
        '  <param name="entryCount">Number of entries in the subsection.</param>
        '*/
        Private Function AppendXRefSubsectionIndexer(ByVal xrefBuilder As StringBuilder, ByVal firstObjectNumber As Integer, ByVal entryCount As Integer) As StringBuilder
            Return xrefBuilder.Append(firstObjectNumber).Append(Symbol.Space).Append(entryCount).Append(Symbol.LineFeed)
        End Function

        '/**
        '  <summary>Serializes the file trailer [PDF:1.6:3.4.4].</summary>
        '  <param name="startxref">Byte offset from the beginning of the file to the beginning
        '    of the last cross-reference section.</param>
        '  <param name="xrefSize">Total number of entries in the file's cross-reference table,
        '    as defined by the combination of the original section and all update sections.</param>
        '  <param name="parser">File parser.</param>
        '*/
        Private Sub WriteTrailer(ByVal startxref As Long, ByVal xrefSize As Integer, ByVal parser As FileParser)
            ' 1. Header.
            Stream.Write(_TrailerChunk)

            ' 2. Body.
            ' Update its entries:
            Dim trailer As PdfDictionary = File.Trailer
            UpdateTrailer(trailer, Stream)
            ' * Size
            trailer(PdfName.Size) = PdfInteger.Get(xrefSize)
            ' * Prev
            If (parser Is Nothing) Then
                trailer.Remove(PdfName.Prev) ' [FIX:0.0.4:5] It (wrongly) kept the 'Prev' entry of multiple-section xref tables.
            Else
                trailer(PdfName.Prev) = PdfInteger.Get(CInt(parser.RetrieveXRefOffset()))
            End If
            ' Serialize its contents!
            trailer.WriteTo(Stream, File)
            Stream.Write(Chunk.LineFeed)

            ' 3. Tail.
            WriteTail(startxref)
        End Sub

#End Region
#End Region
#End Region

    End Class

End Namespace
'/*
'  Copyright 2010-2012 Stefano Chizzolini. http://www.dmdpdf.org

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
    '  <summary>PDF file writer implementing compressed cross-reference stream [PDF:1.6:3.4.7].</summary>
    '*/
    Friend NotInheritable Class CompressedWriter
        Inherits Writer

#Region "dynamic"
#Region "constructors"

        Friend Sub New(ByVal file As files.File, ByVal stream As IOutputStream)
            MyBase.New(file, stream)
        End Sub

#End Region

#Region "Interface"
#Region "Protected"

        Protected Overrides Sub WriteIncremental()
            ' 1. Original content (header, body And previous trailer).
            Dim parser As FileParser = File.Reader.Parser
            Stream.Write(parser.Stream)

            ' 2. Body update (modified indirect objects insertion).
            Dim xrefStreamEntry As XRefEntry
            '{
            ' 2.1. Content indirect objects.
            Dim indirectObjects As IndirectObjects = File.IndirectObjects

            '// Create the xref stream!
            '/*
            '  NOTE: Incremental xref information structure comprises multiple sections; this update adds
            '  a new section.
            '*/
            Dim xrefStream As XRefStream = New XRefStream(File)

            Dim prevFreeEntry As XRefEntry = Nothing
            '/*
            '  NOTE: Extension object streams are necessary to update original object streams whose
            '  entries have been modified.
            '*/
            Dim extensionObjectStreams As IDictionary(Of Integer, ObjectStream) = New Dictionary(Of Integer, ObjectStream)()
            For Each indirectObject As PdfIndirectObject In New List(Of PdfIndirectObject)(indirectObjects.ModifiedObjects.Values)
                prevFreeEntry = AddXRefEntry(indirectObject.XrefEntry, indirectObject, xrefStream, prevFreeEntry, extensionObjectStreams)
            Next
            For Each extensionObjectStream As ObjectStream In extensionObjectStreams.Values
                prevFreeEntry = AddXRefEntry(extensionObjectStream.Container.XrefEntry, extensionObjectStream.Container, xrefStream, prevFreeEntry, Nothing)
            Next
            If (prevFreeEntry IsNot Nothing) Then
                prevFreeEntry.Offset = 0 ' Links back to the first free object. NOTE: The first entry in the table (object number 0) Is always free.
            End If

            '// 2.2. XRef stream.
            '/*
            '  NOTE: This xref stream indirect object Is purposely temporary (i.e. Not registered into the
            '  File 's indirect objects collection).
            '*/
            xrefStreamEntry = New XRefEntry(indirectObjects.Count, 0)
            Dim tmp As New PdfIndirectObject(File, xrefStream, xrefStreamEntry)
            UpdateTrailer(xrefStream.Header, Stream)
            xrefStream.Header(PdfName.Prev) = PdfInteger.Get(CInt(parser.RetrieveXRefOffset()))
            AddXRefEntry(xrefStreamEntry, xrefStream.Container, xrefStream, Nothing, Nothing)
            '}

            ' 3. Tail.
            WriteTail(xrefStreamEntry.Offset)
        End Sub

        Protected Overrides Sub WriteLinearized()
            Throw New NotImplementedException()
        End Sub


        Protected Overrides Sub WriteStandard()
            ' 1. Header [PDF:1.6:3.4.1].
            WriteHeader()

            ' 2. Body [PDF1.6:3.4.2,3,7].
            Dim xrefStreamEntry As XRefEntry
            '{
            ' 2.1. Content indirect objects.
            Dim indirectObjects As IndirectObjects = File.IndirectObjects

            '// Create the xref stream indirect object!
            '/*
            '  NOTE: Standard xref information structure comprises just one section; the xref stream Is
            '  generated on-the-fly And kept volatile Not to interfere with the existing file structure.
            '*/
            '/*
            '  NOTE: This xref stream indirect object Is purposely temporary (i.e. Not registered into the
            '  File 's indirect objects collection).
            '*/
            Dim xrefStream As XRefStream = New XRefStream(File)
            xrefStreamEntry = New XRefEntry(indirectObjects.Count, 0)
            Dim tmp As New PdfIndirectObject(File, xrefStream, xrefStreamEntry)

            Dim prevFreeEntry As XRefEntry = Nothing
            For Each indirectObject As PdfIndirectObject In indirectObjects
                prevFreeEntry = AddXRefEntry(indirectObject.XrefEntry, indirectObject, xrefStream, prevFreeEntry, Nothing)
            Next
            prevFreeEntry.Offset = 0 ' Links back To the first free Object. NOTE: The first entry in the table (object number 0) Is always free.

            ' 2.2. XRef stream.
            UpdateTrailer(xrefStream.Header, Stream)
            AddXRefEntry(xrefStreamEntry, xrefStream.Container, xrefStream, Nothing, Nothing)
            '}

            ' 3. Tail.
            WriteTail(xrefStreamEntry.Offset)
        End Sub

#End Region

#Region "private"

        '/**
        '  <summary>Adds an indirect Object entry To the specified xref stream.</summary>
        '  <param name = "xrefEntry" > Indirect Object's xref entry.</param>
        '  <param name = "indirectObject" > Indirect Object.</param>
        '  <param name = "xrefStream" > XRef stream.</param>
        '  <param name = "prevFreeEntry" > Previous free xref entry.</param>
        '  <param name = "extensionObjectStreams" > Object streams used In incremental updates To extend
        '    modified ones.</param>
        '  <returns>Current free xref entry.</returns>
        '*/
        Private Function AddXRefEntry(ByVal XRefEntry As XRefEntry, ByVal indirectObject As PdfIndirectObject, ByVal xrefStream As XRefStream, ByVal prevFreeEntry As XRefEntry, ByVal extensionObjectStreams As IDictionary(Of Integer, ObjectStream)) As XRefEntry
            xrefStream(XRefEntry.Number) = XRefEntry

            Select Case (XRefEntry.Usage)
                Case XRefEntry.UsageEnum.InUse
                    Dim offset As Integer = CInt(Stream.Length)
                    ' Add entry content!
                    indirectObject.WriteTo(Stream, File)
                    ' Set entry content's offset!
                    XRefEntry.Offset = offset
                     'break;
                Case XRefEntry.UsageEnum.InUseCompressed
                    '/*
                    '  NOTE: Serialization is delegated to the containing object stream.
                    '*/
                    If (extensionObjectStreams IsNot Nothing) Then ' IncrementalThenThen update.
                        Dim baseStreamNumber As Integer = XRefEntry.StreamNumber
                        Dim baseStreamIndirectObject As PdfIndirectObject = File.IndirectObjects(baseStreamNumber)
                        If (baseStreamIndirectObject.IsOriginal()) Then ' Extension Then stream needed In order To preserve the original Object stream.
                            ' Get the extension object stream associated to the original object stream!
                            Dim extensionObjectStream As ObjectStream = Nothing
                            If (Not extensionObjectStreams.TryGetValue(baseStreamNumber, extensionObjectStream)) Then
                                extensionObjectStream = New ObjectStream()
                                File.Register(extensionObjectStream)
                                ' Link the extension to the base object stream!
                                extensionObjectStream.BaseStream = CType(baseStreamIndirectObject.DataObject, ObjectStream)
                                extensionObjectStreams(baseStreamNumber) = extensionObjectStream
                            End If
                            ' Insert the data object into the extension object stream!
                            extensionObjectStream(XRefEntry.Number) = indirectObject.DataObject
                            ' Update the data object's xref entry!
                            XRefEntry.StreamNumber = extensionObjectStream.Reference.ObjectNumber
                            XRefEntry.Offset = XRefEntry.UndefinedOffset ' Internal object index unknown (to set on object stream serialization -- see ObjectStream).
                        End If
                    End If
                    'break;
                Case XRefEntry.UsageEnum.Free
                    If (prevFreeEntry IsNot Nothing) Then
                        prevFreeEntry.Offset = XRefEntry.Number
                        ' Object number of the next free object.
                    End If

                    prevFreeEntry = XRefEntry
                    'break;
                Case Else
                    Throw New NotSupportedException()
            End Select
            Return prevFreeEntry
        End Function

#End Region
#End Region
#End Region

    End Class

End Namespace
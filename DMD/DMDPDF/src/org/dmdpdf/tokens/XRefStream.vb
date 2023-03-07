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

'  You should have received a copy of the GNU Lesser General Public License along with Me
'  Program (see README files); if not, go to the GNU website (http://www.gnu.org/licenses/).

'  Redistribution and use, with or without modification, are permitted provided that such
'  redistributions retain the above copyright notice, license and disclaimer, along with
'  Me list of conditions.
'*/

Imports DMD.org.dmdpdf.bytes
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects
Imports DMD.org.dmdpdf.util
Imports DMD.org.dmdpdf.util.io
Imports DMD.org.dmdpdf.util.parsers

Imports System
Imports System.Collections
Imports System.Collections.Generic

Namespace DMD.org.dmdpdf.tokens

    '/**
    '  <summary>Cross-reference stream containing cross-reference information [PDF:1.6:3.4.7].</summary>
    '  <remarks>It is alternative to the classic cross-reference table.</remarks>
    '*/
    Public NotInheritable Class XRefStream
        Inherits PdfStream
        Implements IDictionary(Of Integer, XRefEntry)

#Region "Static"
#Region "fields"

        Private Const _FreeEntryType As Integer = 0
        Private Const _InUseEntryType As Integer = 1
        Private Const _InUseCompressedEntryType As Integer = 2

        Private Shared ReadOnly _ByteBaseLog As Double = Math.Log(256)

        Private Shared ReadOnly _EntryField0Size As Integer = 1
        Private Shared ReadOnly _EntryField2Size As Integer = GetFieldSize(XRefEntry.GenerationUnreusable)

#End Region

#Region "interface"
#Region "private"

        '/**
        '  <summary> Gets the number Of bytes needed To store the specified value.</summary>
        '  <param name = "maxValue" > Maximum storable value.</param>
        '*/

        Private Shared Function GetFieldSize(ByVal maxValue As Integer) As Integer
            Return CInt(Math.Ceiling(Math.Log(maxValue) / _ByteBaseLog))
        End Function

        '/**
        '  <summary> Converts the specified value into a customly-sized big-endian Byte array.</summary>
        '  <param name = "value" > value To convert.</param>
        '  <param name = "length" > Byte array's length.</param>
        ' */
        Private Shared Function NumberToByteArray(ByVal value As Integer, ByVal length As Integer) As Byte()
            Return ConvertUtils.NumberToByteArray(value, length, ByteOrderEnum.BigEndian)
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "fields"

        Private _entries As SortedDictionary(Of Integer, XRefEntry)

#End Region

#Region "constructors"

        Public Sub New(ByVal file As File)
            Me.New(
                    New PdfDictionary(
                            New PdfName() {PdfName.Type},
                            New PdfDirectObject() {PdfName.XRef}
                            ),
                    New bytes.Buffer()
                )
            Dim header As PdfDictionary = Me.Header
            For Each entry As KeyValuePair(Of PdfName, PdfDirectObject) In file.Trailer
                Dim key As PdfName = entry.Key
                If (key.Equals(PdfName.Root) OrElse
                    key.Equals(PdfName.Info) OrElse
                    key.Equals(PdfName.ID)) Then
                    header(key) = entry.Value
                End If
            Next
        End Sub

        Public Sub New(ByVal header As PdfDictionary, ByVal body As IBuffer)
            MyBase.New(header, body)
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public Overrides Function Accept(ByVal Visitor As IVisitor, ByVal data As Object) As PdfObject
            Return Visitor.Visit(Me, data)
        End Function

        '/**
        '  <summary> Gets the Byte offset from the beginning Of the file
        '  to the beginning of the previous cross-reference stream.</summary>
        '  <returns>-1 In Case no linked stream exists.</returns>
        '*/
        Public ReadOnly Property LinkedStreamOffset As Integer
            Get
                Dim linkedStreamOffsetObject As PdfInteger = CType(header(PdfName.Prev), PdfInteger)
                If (linkedStreamOffsetObject IsNot Nothing) Then
                    Return CInt(linkedStreamOffsetObject.Value)
                Else
                    Return -1
                End If
            End Get
        End Property

        Public Overrides Sub WriteTo(ByVal Stream As IOutputStream, ByVal context As file)
            If (_entries IsNot Nothing) Then
                Flush(Stream)
            End If
            MyBase.WriteTo(Stream, context)
        End Sub

#Region "IDictionary"

        Public Sub Add(ByVal key As Integer, ByVal value As XRefEntry) Implements IDictionary(Of Integer, XRefEntry).Add
            Entries.Add(key, value)
        End Sub

        Public Function ContainsKey(ByVal key As Integer) As Boolean Implements IDictionary(Of Integer, XRefEntry).ContainsKey
            Return Entries.ContainsKey(key)
        End Function

        Public ReadOnly Property Keys As ICollection(Of Integer) Implements IDictionary(Of Integer, XRefEntry).Keys
            Get
                Return Entries.Keys
            End Get
        End Property

        Public Function Remove(ByVal key As Integer) As Boolean Implements IDictionary(Of Integer, XRefEntry).Remove
            Return Entries.Remove(key)
        End Function

        Default Public Property Item(ByVal key As Integer) As XRefEntry Implements IDictionary(Of Integer, XRefEntry).Item
            Get
                Return Entries(key)
            End Get
            Set(ByVal value As XRefEntry)
                Entries(key) = value
            End Set
        End Property

        Public Function TryGetValue(ByVal key As Integer, ByRef value As XRefEntry) As Boolean Implements IDictionary(Of Integer, XRefEntry).TryGetValue
            If (ContainsKey(key)) Then
                value = Me(key)
                Return True
            Else
                value = Nothing ' default(XRefEntry)
                Return False
            End If
        End Function

        Public ReadOnly Property Values As ICollection(Of XRefEntry) Implements IDictionary(Of Integer, XRefEntry).Values
            Get
                Return Entries.Values
            End Get
        End Property

#Region "ICollection"

        Private Sub _Add(ByVal entry As KeyValuePair(Of Integer, XRefEntry)) Implements ICollection(Of KeyValuePair(Of Integer, XRefEntry)).Add
            Me.Add(entry.Key, entry.Value)
        End Sub

        Public Sub Clear() Implements ICollection(Of KeyValuePair(Of Integer, XRefEntry)).Clear
            If (_entries Is Nothing) Then
                _entries = New SortedDictionary(Of Integer, XRefEntry)
            Else
                _entries.Clear()
            End If
        End Sub

        Private Function _Contains(ByVal entry As KeyValuePair(Of Integer, XRefEntry)) As Boolean Implements ICollection(Of KeyValuePair(Of Integer, XRefEntry)).Contains
            Return CType(Entries, ICollection(Of KeyValuePair(Of Integer, XRefEntry))).Contains(entry)
        End Function

        Public Sub CopyTo(ByVal entries As KeyValuePair(Of Integer, XRefEntry)(), ByVal index As Integer) Implements ICollection(Of Generic.KeyValuePair(Of Integer, XRefEntry)).CopyTo
            entries.CopyTo(entries, index)
        End Sub

        Public ReadOnly Property Count As Integer Implements ICollection(Of KeyValuePair(Of Integer, XRefEntry)).Count
            Get
                Return Entries.Count
            End Get
        End Property

        Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of KeyValuePair(Of Integer, XRefEntry)).IsReadOnly
            Get
                Return False
            End Get
        End Property

        Public Function Remove(ByVal entry As KeyValuePair(Of Integer, XRefEntry)) As Boolean Implements ICollection(Of Generic.KeyValuePair(Of Integer, XRefEntry)).Remove
            Dim value As XRefEntry = Nothing
            If (TryGetValue(entry.Key, value) AndAlso
                value.Equals(entry.Value)) Then
                Return Entries.Remove(entry.Key)
            Else
                Return False
            End If
        End Function

#Region "IEnumerable<KeyValuePair(of Integer,XRefEntry)>"

        Public Function GetEnumerator() As IEnumerator(Of KeyValuePair(Of Integer, XRefEntry)) Implements IEnumerable(Of KeyValuePair(Of Integer, XRefEntry)).GetEnumerator
            Return Me.Entries.GetEnumerator()
        End Function

#Region "IEnumerable"

        Private Function _GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Return CType(Me, IEnumerable(Of KeyValuePair(Of Integer, XRefEntry))).GetEnumerator()
        End Function

#End Region
#End Region
#End Region
#End Region
#End Region

#Region "Private"

        Private ReadOnly Property Entries As SortedDictionary(Of Integer, XRefEntry)
            Get
                If (_entries Is Nothing) Then
                    _entries = New SortedDictionary(Of Integer, XRefEntry)()
                    Dim length As Integer
                    Dim body As IBuffer = Me.Body
                    If (body.Length > 0) Then
                        Dim header As PdfDictionary = Me.Header
                        Dim size As Integer = CInt(CType(header(PdfName.Size), PdfInteger).Value)
                        Dim entryFieldSizes As Integer()
                        Dim entryFieldSizesObject As PdfArray = CType(header(PdfName.W), PdfArray)
                        entryFieldSizes = New Integer(entryFieldSizesObject.Count - 1) {}
                        length = entryFieldSizes.Length
                        For index As Integer = 0 To length - 1
                            entryFieldSizes(index) = CInt(CType(entryFieldSizesObject(index), PdfInteger).Value)
                        Next

                        Dim subsectionBounds As PdfArray
                        If (header.ContainsKey(PdfName.Index)) Then
                            subsectionBounds = CType(header(PdfName.Index), PdfArray)
                        Else
                            subsectionBounds = New PdfArray()
                            subsectionBounds.Add(PdfInteger.Get(0))
                            subsectionBounds.Add(PdfInteger.Get(size))
                        End If

                        body.ByteOrder = ByteOrderEnum.BigEndian
                        body.Seek(0)

                        Dim subsectionBoundIterator As IEnumerator(Of PdfDirectObject) = subsectionBounds.GetEnumerator()
                        While (subsectionBoundIterator.MoveNext())
                            Try
                                Dim start As Integer = CType(subsectionBoundIterator.Current, PdfInteger).IntValue
                                subsectionBoundIterator.MoveNext()
                                Dim count As Integer = CType(subsectionBoundIterator.Current, PdfInteger).IntValue
                                length = start + count
                                For entryIndex As Integer = start To length - 1
                                    Dim entryFieldType As Integer
                                    If (entryFieldSizes(0) = 0) Then
                                        entryFieldType = 1
                                    Else
                                        entryFieldType = body.ReadInt(entryFieldSizes(0))
                                    End If
                                    Select Case (entryFieldType)
                                        Case _FreeEntryType
                                            Dim nextFreeObjectNumber As Integer = body.ReadInt(entryFieldSizes(1))
                                            Dim generation As Integer = body.ReadInt(entryFieldSizes(2))
                                            _entries(entryIndex) = New XRefEntry(entryIndex, generation, nextFreeObjectNumber, XRefEntry.UsageEnum.Free)
                                            'break;
                                        Case _InUseEntryType
                                            Dim offset As Integer = body.ReadInt(entryFieldSizes(1))
                                            Dim generation As Integer = body.ReadInt(entryFieldSizes(2))
                                            _entries(entryIndex) = New XRefEntry(entryIndex, generation, offset, XRefEntry.UsageEnum.InUse)
                                            'break;
                                        Case _InUseCompressedEntryType
                                            Dim streamNumber As Integer = body.ReadInt(entryFieldSizes(1))
                                            Dim innerNumber As Integer = body.ReadInt(entryFieldSizes(2))
                                            _entries(entryIndex) = New XRefEntry(entryIndex, innerNumber, streamNumber)
                                        Case Else
                                            Throw New NotSupportedException("Unknown xref entry type '" & entryFieldType & "'.")
                                    End Select
                                Next
                            Catch e As System.Exception
                                Throw New ParseException("Unexpected EOF (malformed cross-reference stream object).", e)
                            End Try
                        End While
                    End If
                End If
                Return _entries
            End Get
        End Property

        '/**
        '  <summary> Serializes the xref stream entries into the stream body.</summary>
        '*/
        Private Sub Flush(ByVal Stream As IOutputStream)
            ' 1. Body.
            Dim indexArray As PdfArray = New PdfArray()
            ' // NOTE: We assume Me xref stream Is the last indirect Object.
            Dim entryFieldSizes As Integer() = New Integer() {
                                                _EntryField0Size,
                                                GetFieldSize(CInt(Stream.Length)),
                                                _EntryField2Size
                                                }
            ' Get the stream buffer!
            Dim body As IBuffer = Me.Body

            ' Delete the old entries!
            body.SetLength(0)

            ' Serializing the entries into the stream buffer...
            Dim prevObjectNumber As Integer = -2 ' Previous-entry object number.
            For Each entry As XRefEntry In _entries.Values
                Dim entryNumber As Integer = entry.Number
                If (entryNumber - prevObjectNumber <> 1) Then ' Current Then subsection terminated.
                    If (indexArray.Count > 0) Then
                        indexArray.Add(PdfInteger.Get(prevObjectNumber - CType(indexArray(indexArray.Count - 1), PdfInteger).IntValue + 1))
                    End If ' Number of entries in the previous subsection.
                    indexArray.Add(PdfInteger.Get(entryNumber)) ' First Object number In the Next subsection.
                End If
                prevObjectNumber = entryNumber

                Select Case (entry.Usage)
                    Case XRefEntry.UsageEnum.Free
                        body.Append(CByte(_FreeEntryType))
                        body.Append(NumberToByteArray(entry.Offset, entryFieldSizes(1)))
                        body.Append(NumberToByteArray(entry.Generation, entryFieldSizes(2)))
                        'break;
                    Case XRefEntry.UsageEnum.InUse
                        body.Append(CByte(_InUseEntryType))
                        body.Append(NumberToByteArray(entry.Offset, entryFieldSizes(1)))
                        body.Append(NumberToByteArray(entry.Generation, entryFieldSizes(2)))
                        'break;
                    Case XRefEntry.UsageEnum.InUseCompressed
                        body.Append(CByte(_InUseCompressedEntryType))
                        body.Append(NumberToByteArray(entry.StreamNumber, entryFieldSizes(1)))
                        body.Append(NumberToByteArray(entry.Offset, entryFieldSizes(2)))
                        'break;
                    Case Else
                        Throw New NotSupportedException()
                End Select
            Next
            indexArray.Add(PdfInteger.Get(prevObjectNumber - CType(indexArray(indexArray.Count - 1), PdfInteger).IntValue + 1)) ' Number Of entries In the previous subsection.

            ' 2. Header.

            Dim header As PdfDictionary = Me.Header
            header(PdfName.Index) = indexArray
            header(PdfName.Size) = PdfInteger.Get(File.IndirectObjects.Count + 1)
            header(PdfName.W) = New PdfArray(
                                    PdfInteger.Get(entryFieldSizes(0)),
                                    PdfInteger.Get(entryFieldSizes(1)),
                                    PdfInteger.Get(entryFieldSizes(2))
                                        )

        End Sub

#End Region
#End Region
#End Region

    End Class

End Namespace
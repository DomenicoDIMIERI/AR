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

Imports System
Imports System.Collections
Imports System.Collections.Generic

Namespace DMD.org.dmdpdf.tokens

    '/**
    '  <summary>Object stream containing a sequence of PDF objects [PDF:1.6:3.4.6].</summary>
    '  <remarks>The purpose of object streams is to allow a greater number of PDF objects
    '  to be compressed, thereby substantially reducing the size of PDF files.
    '  The objects in the stream are referred to as compressed objects.</remarks>
    '*/
    Public NotInheritable Class ObjectStream
        Inherits PdfStream
        Implements IDictionary(Of Integer, PdfDataObject)

#Region "types"

        Private NotInheritable Class ObjectEntry

            Friend _dataObject As PdfDataObject
            Friend _offset As Integer

            Private _parser As FileParser

            Private Sub New(ByVal parser As FileParser)
                Me._parser = parser
            End Sub

            Public Sub New(ByVal offset As Integer, ByVal parser As FileParser)
                Me.New(parser)
                Me._dataObject = Nothing
                Me._offset = offset
            End Sub

            Public Sub New(ByVal dataObject As PdfDataObject, ByVal parser As FileParser)
                Me.New(parser)
                Me._dataObject = dataObject
                Me._offset = -1 ' Undefined -- To Set On stream serialization.
            End Sub

            Public ReadOnly Property DataObject As PdfDataObject
                Get
                    If (_dataObject Is Nothing) Then
                        _parser.Seek(_offset)
                        _parser.MoveNext()
                        _dataObject = _parser.ParsePdfObject()
                    End If
                    Return _dataObject
                End Get
            End Property
        End Class

#End Region

#Region "dynamic"
#Region "fields"

        '/**
        '  <summary> Compressed objects map.</summary>
        '  <remarks> This map Is initially populated With offset values;
        '  when a compressed object Is required, its offset Is used to retrieve it.</remarks>
        '*/
        Private _entries As IDictionary(Of Integer, ObjectEntry)
        Private _parser As FileParser

#End Region

#Region "constructors"

        Public Sub New()
            MyBase.New(New PdfDictionary(New PdfName() {PdfName.Type}, New PdfDirectObject() {PdfName.ObjStm}))
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
        '  <summary> Gets/Sets the Object stream extended by Me one.</summary>
        '  <remarks> Both streams are considered part Of a collection Of Object streams  whose links form
        '  a directed acyclic graph.</remarks>
        '*/
        Public Property BaseStream As ObjectStream
            Get
                Return CType(Header.Resolve(PdfName.Extends), ObjectStream)
            End Get
            Set(ByVal value As ObjectStream)
                Me.Header(PdfName.Extends) = value.Reference
            End Set
        End Property

        Public Overrides Sub WriteTo(ByVal Stream As IOutputStream, ByVal context As File)
            If (_entries IsNot Nothing) Then
                Flush(Stream)
            End If
            MyBase.WriteTo(Stream, context)
        End Sub

#Region "IDictionary"

        Public Sub Add(ByVal key As Integer, ByVal value As objects.PdfDataObject) Implements IDictionary(Of Integer, objects.PdfDataObject).Add
            Entries.Add(key, New ObjectEntry(value, _parser))
        End Sub

        Public Function ContainsKey(ByVal key As Integer) As Boolean Implements Generic.IDictionary(Of Integer, objects.PdfDataObject).ContainsKey
            Return Entries.ContainsKey(key)
        End Function

        Public ReadOnly Property Keys As ICollection(Of Integer) Implements Generic.IDictionary(Of Integer, objects.PdfDataObject).Keys
            Get
                Return Entries.Keys
            End Get
        End Property

        Public Function Remove(ByVal key As Integer) As Boolean Implements IDictionary(Of Integer, objects.PdfDataObject).Remove
            Return Entries.Remove(key)
        End Function

        Default Public Property Item(ByVal key As Integer) As PdfDataObject Implements Generic.IDictionary(Of Integer, PdfDataObject).Item
            Get
                Dim entry As ObjectEntry = Entries(key)
                If (entry IsNot Nothing) Then
                    Return entry.DataObject
                Else
                    Return Nothing
                End If
            End Get
            Set(ByVal value As PdfDataObject)
                Entries(key) = New ObjectEntry(value, _parser)
            End Set
        End Property

        Public Function TryGetValue(ByVal key As Integer, ByRef value As PdfDataObject) As Boolean Implements IDictionary(Of Integer, PdfDataObject).TryGetValue
            value = Me(key)
            Return (value IsNot Nothing) OrElse Me.ContainsKey(key)
        End Function

        Public ReadOnly Property Values As ICollection(Of PdfDataObject) Implements IDictionary(Of Integer, PdfDataObject).Values
            Get
                Dim _values As IList(Of PdfDataObject) = New List(Of PdfDataObject)
                For Each key As Integer In Entries.Keys
                    _values.Add(Me(key))
                Next
                Return _values
            End Get
        End Property

#Region "ICollection"

        Private Sub _add(ByVal entry As KeyValuePair(Of Integer, PdfDataObject)) Implements ICollection(Of KeyValuePair(Of Integer, PdfDataObject)).Add
            Me.Add(entry.Key, entry.Value)
        End Sub

        Public Sub Clear() Implements ICollection(Of KeyValuePair(Of Integer, PdfDataObject)).Clear
            If (_entries Is Nothing) Then
                _entries = New Dictionary(Of Integer, ObjectEntry)()
            Else
                _entries.Clear()
            End If
        End Sub

        Private Function Contains(ByVal entry As KeyValuePair(Of Integer, PdfDataObject)) As Boolean Implements ICollection(Of KeyValuePair(Of Integer, PdfDataObject)).Contains
            Return CType(Entries, ICollection(Of KeyValuePair(Of Integer, PdfDataObject))).Contains(entry)
        End Function

        Public Sub CopyTo(ByVal entries As KeyValuePair(Of Integer, PdfDataObject)(), ByVal index As Integer) Implements ICollection(Of KeyValuePair(Of Integer, PdfDataObject)).CopyTo
            Throw New NotImplementedException()
        End Sub

        Public ReadOnly Property Count As Integer Implements ICollection(Of KeyValuePair(Of Integer, PdfDataObject)).Count
            Get
                Return Entries.Count
            End Get
        End Property

        Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of KeyValuePair(Of Integer, PdfDataObject)).IsReadOnly
            Get
                Return False
            End Get
        End Property

        Public Function Remove(ByVal entry As KeyValuePair(Of Integer, PdfDataObject)) As Boolean Implements ICollection(Of KeyValuePair(Of Integer, PdfDataObject)).Remove
            Dim value As PdfDataObject = Nothing
            If (
                Me.TryGetValue(entry.Key, value) AndAlso
                value.Equals(entry.Value)
                ) Then
                Return Entries.Remove(entry.Key)
            Else
                Return False
            End If
        End Function

#Region "IEnumerable<KeyValuePair(of Integer,PdfDataObject)>"

        '    IEnumerator<KeyValuePair(of Integer,PdfDataObject)> IEnumerable<KeyValuePair(of Integer,PdfDataObject)>.GetEnumerator(
        '  )
        '{
        '  foreach(int key in Keys)
        '  {yield return New KeyValuePair(of Integer,PdfDataObject)(key,Me[key]);}
        '}
        Private Class mEnumerator
            Implements IEnumerator(Of KeyValuePair(Of Integer, PdfDataObject))

            Public o As ObjectStream
            Public keys As ICollection(Of Integer)
            Public index As Integer

            Public Sub New(ByVal o As ObjectStream)
                Me.o = o
                Me.Reset()
            End Sub

            Public ReadOnly Property Current As KeyValuePair(Of Integer, PdfDataObject) Implements IEnumerator(Of KeyValuePair(Of Integer, PdfDataObject)).Current
                Get
                    Dim key As Integer = Me.keys(Me.index)
                    Return New KeyValuePair(Of Integer, PdfDataObject)(key, Me.o(key))
                End Get
            End Property

            Private ReadOnly Property IEnumerator_Current As Object Implements IEnumerator.Current
                Get
                    Return Me.Current
                End Get
            End Property

            Public Sub Reset() Implements IEnumerator.Reset
                Me.keys = Me.o.Keys
                Me.index = -1
            End Sub

            Public Function MoveNext() As Boolean Implements IEnumerator.MoveNext
                If (Me.index < Me.keys.Count - 1) Then
                    Me.index += 1
                    Return True
                Else
                    Return False
                End If
            End Function


            ' Questo codice viene aggiunto da Visual Basic per implementare in modo corretto il criterio Disposable.
            Public Sub Dispose() Implements IDisposable.Dispose
                Me.o = Nothing
                Me.keys = Nothing
            End Sub

        End Class


        Public Function GetEnumerator() As IEnumerator(Of KeyValuePair(Of Integer, PdfDataObject)) Implements IEnumerable(Of KeyValuePair(Of Integer, PdfDataObject)).GetEnumerator
            Return New mEnumerator(Me)
        End Function

#Region "IEnumerable"
        Private Function _GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Return CType(Me, IEnumerable(Of KeyValuePair(Of Integer, PdfDataObject))).GetEnumerator()
        End Function


#End Region
#End Region
#End Region
#End Region
#End Region

#Region "Private"

        Private ReadOnly Property Entries As IDictionary(Of Integer, ObjectEntry)
            Get
                If (_entries Is Nothing) Then
                    _entries = New Dictionary(Of Integer, ObjectEntry)()

                    Dim body As IBuffer = Me.Body
                    If (body.Length > 0) Then
                        _parser = New FileParser(body, File)
                        Dim baseOffset As Integer = CType(Header(PdfName.First), PdfInteger).IntValue
                        Dim length As Integer = CType(Header(PdfName.N), PdfInteger).IntValue
                        For index As Integer = 0 To length - 1
                            Dim objectNumber As Integer = CType(_parser.ParsePdfObject(1), PdfInteger).IntValue
                            Dim objectOffset As Integer = baseOffset + CType(_parser.ParsePdfObject(1), PdfInteger).IntValue
                            _entries(objectNumber) = New ObjectEntry(objectOffset, _parser)
                        Next
                    End If
                End If
                Return _entries
            End Get
        End Property


        '/**
        '  <summary> Serializes the Object stream entries into the stream body.</summary>
        '*/
        Private Sub Flush(ByVal stream As IOutputStream)

            ' 1. Body.
            Dim dataByteOffset As Integer

            ' Serializing the entries into the stream buffer...
            Dim indexBuffer As IBuffer = New bytes.Buffer()
            Dim dataBuffer As IBuffer = New bytes.Buffer()
            Dim indirectObjects As IndirectObjects = File.IndirectObjects
            Dim objectIndex As Integer = -1
            Dim context As File = File
            For Each entry As KeyValuePair(Of Integer, ObjectEntry) In Entries
                Dim objectNumber As Integer = entry.Key

                ' Update the xref entry!
                Dim xrefEntry As XRefEntry = indirectObjects(objectNumber).XrefEntry
                objectIndex += 1 : xrefEntry.Offset = objectIndex
                '/*
                '  NOTE: The entry offset MUST be updated only after its serialization, in order not to
                '  interfere with its possible data-object retrieval from the old serialization.
                '*/
                Dim entryValueOffset As Integer = CInt(dataBuffer.Length)

                ' Index.
                '// Object number.
                indexBuffer.Append(objectNumber.ToString()).Append(Chunk.Space).Append(entryValueOffset.ToString()).Append(Chunk.Space) ' Byte offset (relative to the first one).

                ' Data.
                entry.Value.DataObject.WriteTo(dataBuffer, context)
                entry.Value._offset = entryValueOffset
            Next

            ' Get the stream buffer!
            Dim body As IBuffer = Me.Body

            ' Delete the old entries!
            body.SetLength(0)

            ' Add the new entries!
            body.Append(indexBuffer)
            dataByteOffset = CInt(body.Length)
            body.Append(dataBuffer)

            ' 2. Header.
            Dim header As PdfDictionary = Me.Header
            header(PdfName.N) = PdfInteger.Get(Entries.Count)
            header(PdfName.First) = PdfInteger.Get(dataByteOffset)

        End Sub

#End Region
#End Region
#End Region

    End Class

End Namespace
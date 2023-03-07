'/*
'  Copyright 2007-2012 Stefano Chizzolini. http://www.dmdpdf.org

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
Imports DMD.org.dmdpdf.documents.contents.objects
Imports DMD.org.dmdpdf.documents.contents.tokens
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects
Imports DMD.org.dmdpdf.util.io

Imports System
Imports System.Collections
Imports System.Collections.Generic

Namespace DMD.org.dmdpdf.documents.contents

    '/**
    '  <summary>Content stream [PDF:1.6:3.7.1].</summary>
    '  <remarks>During its loading, Me content stream is parsed and its instructions
    '  are exposed as a list; in case of modifications, it's user responsability
    '  to call the <see cref="Flush()"/> method in order to serialize back the instructions
    '  into Me content stream.</remarks>
    '*/
    <PDF(VersionEnum.PDF10)>
    Public NotInheritable Class Contents
        Inherits PdfObjectWrapper(Of PdfDataObject)
        Implements IList(Of ContentObject)

#Region "types"

        '    /**
        '  <summary> Content stream wrapper.</summary>
        '*/
        Private Class ContentStream
            Implements bytes.IInputStream

            Private ReadOnly _baseDataObject As PdfDataObject

            Private _basePosition As Long
            Private _stream As bytes.IInputStream
            Private _streamIndex As Integer = -1

            Public Sub New(ByVal baseDataObject As PdfDataObject)
                Me._baseDataObject = baseDataObject
                MoveNextStream()
            End Sub

            Public Property ByteOrder As ByteOrderEnum Implements IInputStream.ByteOrder
                Get
                    Return _stream.ByteOrder
                End Get
                Set(ByVal value As ByteOrderEnum)
                    Throw New NotSupportedException()
                End Set
            End Property

            Public Overrides Function GetHashCode() As Integer Implements IInputStream.GetHashCode
                Return MyBase.GetHashCode()
            End Function

            Public Sub Dispose() Implements IInputStream.Dispose
                ' NOOP 
            End Sub

            Public ReadOnly Property Length As Long Implements IInputStream.Length
                Get
                    If (TypeOf (_baseDataObject) Is PdfStream) Then ' Single Then stream.
                        Return CType(_baseDataObject, PdfStream).Body.Length
                    Else ' Array Of streams.
                        Dim _length As Long = 0
                        For Each stream As PdfDirectObject In CType(_baseDataObject, PdfArray)
                            _length += CType(CType(stream, PdfReference).DataObject, PdfStream).Body.Length
                        Next
                        Return _length
                    End If
                End Get
            End Property

            Public Property Position As Long Implements IInputStream.Position
                Get
                    Return _basePosition + _stream.Position
                End Get
                Set(ByVal value As Long)
                    Seek(value)
                End Set
            End Property

            Public Sub Read(ByVal data As Byte()) Implements IInputStream.Read
                Throw New NotImplementedException()
            End Sub

            Public Sub Read(ByVal data As Byte(), ByVal offset As Integer, ByVal length As Integer) Implements IInputStream.Read
                Throw New NotImplementedException()
            End Sub

            Public Function ReadByte() As Integer Implements IInputStream.ReadByte
                If ((_stream Is Nothing OrElse
                        _stream.Position >= _stream.Length) AndAlso
                        Not MoveNextStream()) Then
                    Return -1 'TODO:harmonize with other Read*() method EOF exceptions!!!
                End If
                Return _stream.ReadByte()
            End Function

            Public Function ReadInt() As Integer Implements IInputStream.ReadInt
                Throw New NotImplementedException()
            End Function

            Public Function ReadInt(ByVal length As Integer) As Integer Implements IInputStream.ReadInt
                Throw New NotImplementedException()
            End Function

            Public Function ReadLine() As String Implements IInputStream.ReadLine
                Throw New NotImplementedException()
            End Function

            Public Function ReadShort() As Short Implements IInputStream.ReadShort
                Throw New NotImplementedException()
            End Function

            Public Function ReadSignedByte() As SByte Implements IInputStream.ReadSignedByte
                Throw New NotImplementedException()
            End Function

            Public Function ReadString(ByVal length As Integer) As String Implements IInputStream.ReadString
                Throw New NotImplementedException()
            End Function

            Public Function ReadUnsignedShort() As UShort Implements IInputStream.ReadUnsignedShort
                Throw New NotImplementedException()
            End Function

            Public Sub Seek(ByVal position As Long) Implements IInputStream.Seek
                While (True)
                    If (position < _basePosition) Then ' Before Then current stream.
                        If (Not MovePreviousStream()) Then Throw New ArgumentException("Lower than acceptable.", "position")
                    ElseIf (position > _basePosition + _stream.Length) Then '/ After Then current stream.
                        If (Not MoveNextStream()) Then Throw New ArgumentException("Higher than acceptable.", "position")
                    Else ' At current stream.
                        _stream.Seek(position - _basePosition)
                        Exit While ' 
                    End If
                End While
            End Sub

            Public Sub Skip(ByVal offset As Long) Implements IInputStream.Skip
                While (True)
                    Dim position As Long = _stream.Position + offset
                    If (position < 0) Then ' Before Then current stream.
                        offset += _stream.Position
                        If (Not MovePreviousStream()) Then Throw New ArgumentException("Lower than acceptable.", "offset")
                        _stream.Position = _stream.Length
                    ElseIf (position > _stream.Length) Then ' After Then current stream.
                        offset -= (_stream.Length - _stream.Position)
                        If (Not MoveNextStream()) Then Throw New ArgumentException("Higher than acceptable.", "offset")
                    Else ' At current stream.
                        _stream.Seek(position)
                        Exit While ' break;
                    End If
                End While
            End Sub

            Public Function ToByteArray() As Byte() Implements IInputStream.ToByteArray
                Throw New NotImplementedException()
            End Function

            Private Function MoveNextStream() As Boolean
                '// Is the content stream just a single stream?
                '/*
                '  NOTE:                                         A content stream may be made up Of multiple streams [PDF: 1.6:3.6.2].
                '*/
                If (TypeOf (_baseDataObject) Is PdfStream) Then ' Single Then stream.
                    If (_streamIndex < 1) Then
                        _streamIndex += 1
                        If (_streamIndex = 0) Then
                            _basePosition = 0
                        Else
                            _basePosition = _basePosition + _stream.Length
                        End If
                        If (_streamIndex < 1) Then
                            _stream = CType(_baseDataObject, PdfStream).Body
                        Else
                            _stream = Nothing
                        End If
                    End If
                Else ' Multiple streams.
                    Dim streams As PdfArray = CType(_baseDataObject, PdfArray)
                    If (_streamIndex < streams.Count) Then
                        _streamIndex += 1
                        If (_streamIndex = 0) Then
                            _basePosition = 0
                        Else
                            _basePosition = _basePosition + _stream.Length
                        End If
                        If (_streamIndex < streams.Count) Then
                            _stream = CType(streams.Resolve(_streamIndex), PdfStream).Body
                        Else
                            _stream = Nothing
                        End If
                    End If
                End If
                If (_stream Is Nothing) Then Return False
                _stream.Position = 0
                Return True
            End Function

            Private Function MovePreviousStream() As Boolean
                If (_streamIndex = 0) Then
                    _streamIndex -= 1
                    _stream = Nothing
                End If
                If (_streamIndex = -1) Then Return False
                _streamIndex -= 1
                '/* NOTE: A content stream may be made up Of multiple streams [PDF:  1.6:3.6.2]. */
                '// Is the content stream just a single stream?
                If (TypeOf (_baseDataObject) Is PdfStream) Then ' Single Then Stream.
                    _stream = CType(_baseDataObject, PdfStream).Body
                    _basePosition = 0
                Else ' Array Of streams.
                    Dim streams As PdfArray = CType(_baseDataObject, PdfArray)
                    _stream = CType(CType(streams(_streamIndex), PdfReference).DataObject, PdfStream).Body
                    _basePosition -= _stream.Length
                End If
                Return True
            End Function

        End Class


#End Region

#Region "Static"
#Region "Interface"
#Region "Public"

        Public Shared Function Wrap(ByVal BaseObject As PdfDirectObject, ByVal contentContext As IContentContext) As Contents
            If (BaseObject IsNot Nothing) Then
                Return New Contents(BaseObject, contentContext)
            Else
                Return Nothing
            End If
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "fields"

        Private _items As IList(Of ContentObject)


        Private _contentContext As IContentContext

#End Region

#Region "constructors"

        Private Sub New(ByVal baseObject As PdfDirectObject, ByVal contentContext As IContentContext)
            MyBase.New(baseObject)
            Me._contentContext = contentContext
            Load()
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public Overrides Function Clone(ByVal context As Document) As Object
            Throw New NotSupportedException()
        End Function

        '/**
        '  <summary> Serializes the contents into the content stream.</summary>
        '*/
        Public Sub Flush()
            Dim stream As PdfStream
            Dim baseDataObject As PdfDataObject = Me.BaseDataObject
            ' Are contents just a single stream object?
            If (TypeOf (BaseDataObject) Is PdfStream) Then ' Single Then Stream.
                stream = CType(baseDataObject, PdfStream)
            Else ' Array Of streams.
                Dim streams As PdfArray = CType(baseDataObject, PdfArray)
                ' No stream available?
                If (streams.Count = 0) Then ' No Then Stream.
                    ' Add first stream!
                    stream = New PdfStream()
                    ' Inserts the New stream into the content stream.
                    ' Inserts the New stream into the file.
                    streams.Add(File.Register(stream))
                Else  ' Streams exist.
                    '// Eliminating exceeding streams...
                    '/*
                    '  NOTE: Applications that consume Or produce PDF files are Not required To preserve
                    '  the existing Structure Of the Contents array [PDF:  1.6:3.6.2].
                    '*/
                    While (streams.Count > 1)
                        File.Unregister(CType(streams(1), PdfReference)) ' Removes the exceeding stream from the file.
                        streams.RemoveAt(1) ' Removes the exceeding stream from the content stream.
                    End While
                    stream = CType(streams.Resolve(0), PdfStream)
                End If
            End If

            ' Get the stream buffer!
            Dim Buffer As bytes.IBuffer = stream.Body
            ' Delete old contents from the stream buffer!
            Buffer.SetLength(0)
            ' Serializing the New contents into the stream buffer...
            Dim context As Document = Document
            For Each item As ContentObject In _items
                item.WriteTo(Buffer, context)
            Next
        End Sub

        Public ReadOnly Property ContentContext As IContentContext
            Get
                Return _contentContext
            End Get
        End Property

#Region "IList"

        Public Function IndexOf(ByVal obj As ContentObject) As Integer Implements IList(Of ContentObject).IndexOf
            Return _items.IndexOf(obj)
        End Function

        Public Sub Insert(ByVal Index As Integer, ByVal obj As ContentObject) Implements IList(Of ContentObject).Insert
            _items.Insert(Index, obj)
        End Sub

        Public Sub RemoveAt(ByVal Index As Integer) Implements IList(Of ContentObject).RemoveAt
            _items.RemoveAt(Index)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As ContentObject Implements IList(Of ContentObject).Item
            Get
                Return _items(Index)
            End Get
            Set(value As ContentObject)
                _items(Index) = value
            End Set
        End Property

#Region "ICollection"

        Public Sub Add(ByVal obj As ContentObject) Implements ICollection(Of ContentObject).Add
            _items.Add(obj)
        End Sub

        Public Sub Clear() Implements ICollection(Of ContentObject).Clear
            _items.Clear()
        End Sub

        Public Function Contains(ByVal obj As ContentObject) As Boolean Implements ICollection(Of ContentObject).Contains
            Return _items.Contains(obj)
        End Function

        Public Sub CopyTo(ByVal objs As ContentObject(), ByVal Index As Integer) Implements ICollection(Of ContentObject).CopyTo
            _items.CopyTo(objs, Index)
        End Sub

        Public ReadOnly Property Count As Integer Implements ICollection(Of ContentObject).Count
            Get
                Return _items.Count
            End Get
        End Property

        Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of ContentObject).IsReadOnly
            Get
                Return False
            End Get
        End Property

        Public Function Remove(ByVal obj As ContentObject) As Boolean Implements ICollection(Of ContentObject).Remove
            Return Me._items.Remove(obj)
        End Function


#Region "IEnumerable(Of ContentObject)"

        Public Function GetEnumerator() As IEnumerator(Of ContentObject) Implements IEnumerable(Of ContentObject).GetEnumerator
            Return Me._items.GetEnumerator()
        End Function

#Region "IEnumerable"

        Private Function _GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Return Me.GetEnumerator
        End Function

#End Region
#End Region
#End Region
#End Region
#End Region

#Region "Private"

        Private Sub Load()
            Dim parser As ContentParser = New ContentParser(New ContentStream(BaseDataObject))
            _items = parser.ParseContentObjects()
        End Sub

#End Region
#End Region
#End Region
    End Class

End Namespace

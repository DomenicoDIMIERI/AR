'/*
'  Copyright 2008-2012 Stefano Chizzolini. http://www.dmdpdf.org

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
Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.objects

Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.IO

Namespace DMD.org.dmdpdf.documents.files

    '/**
    '  <summary>Embedded files referenced by another one (dependencies) [PDF:1.6:3.10.3].</summary>
    '*/
    <PDF(VersionEnum.PDF13)>
    Public NotInheritable Class RelatedFiles
        Inherits PdfObjectWrapper(Of PdfArray)
        Implements IDictionary(Of String, EmbeddedFile)

#Region "Static"
#Region "Interface"
#Region "Public"

        Public Shared Function Wrap(ByVal baseObject As PdfDirectObject) As RelatedFiles
            If (baseObject IsNot Nothing) Then
                Return New RelatedFiles(baseObject)
            Else
                Return Nothing
            End If
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        Public Sub New(ByVal context As Document)
            MyBase.New(context, New PdfArray())
        End Sub

        Private Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "Interface"
#Region "Public"
#Region "IDictionary"

        Public Sub Add(ByVal key As String, ByVal value As EmbeddedFile) Implements IDictionary(Of String, EmbeddedFile).Add
            Dim itemPairs As PdfArray = Me.BaseDataObject
            ' New entry.
            itemPairs.Add(New PdfTextString(key))
            itemPairs.Add(value.BaseObject)
        End Sub

        Public Function ContainsKey(ByVal key As String) As Boolean Implements IDictionary(Of String, EmbeddedFile).ContainsKey
            Dim itemPairs As PdfArray = Me.BaseDataObject
            Dim length As Integer = itemPairs.Count
            For index As Integer = 0 To length - 1 Step 2
                If (CType(itemPairs(index), PdfTextString).Value.Equals(key)) Then
                    Return True
                End If
            Next
            Return False
        End Function

        Public ReadOnly Property Keys As ICollection(Of String) Implements IDictionary(Of String, EmbeddedFile).Keys
            Get
                Dim _keys As List(Of String) = New List(Of String)
                Dim itemPairs As PdfArray = Me.BaseDataObject
                Dim length As Integer = itemPairs.Count
                For index As Integer = 0 To length - 1 Step 2
                    _keys.Add(CStr(CType(itemPairs(index), PdfTextString).Value))
                Next
                Return _keys
            End Get
        End Property

        Public Function Remove(ByVal key As String) As Boolean Implements IDictionary(Of String, EmbeddedFile).Remove
            Dim itemPairs As PdfArray = Me.BaseDataObject
            Dim length As Integer = itemPairs.Count
            For index As Integer = 0 To length - 1 Step 2
                If (CType(itemPairs(index), PdfTextString).Value.Equals(key)) Then
                    itemPairs.RemoveAt(index) ' Key removed.
                    itemPairs.RemoveAt(index) ' Value removed.
                    Return True
                End If
            Next
            Return False
        End Function

        Default Public Property Item(ByVal key As String) As EmbeddedFile Implements IDictionary(Of String, EmbeddedFile).Item
            Get
                Dim itemPairs As PdfArray = Me.BaseDataObject
                Dim length As Integer = itemPairs.Count
                For index As Integer = 0 To length - 1 Step 2
                    If (CType(itemPairs(index), PdfTextString).Value.Equals(key)) Then
                        Return EmbeddedFile.Wrap(itemPairs(index + 1))
                    End If
                Next
                Return Nothing
            End Get
            Set(ByVal value As EmbeddedFile)
                Dim itemPairs As PdfArray = Me.BaseDataObject
                Dim length As Integer = itemPairs.Count
                For index As Integer = 0 To length - 1 Step 2
                    ' Already existing entry?
                    If (CType(itemPairs(index), PdfTextString).Value.Equals(key)) Then
                        itemPairs(index + 1) = value.BaseObject
                        Return
                    End If
                Next
                ' New entry.
                itemPairs.Add(New PdfTextString(key))
                itemPairs.Add(value.BaseObject)
            End Set
        End Property

        Public Function TryGetValue(ByVal key As String, ByRef value As EmbeddedFile) As Boolean Implements IDictionary(Of String, EmbeddedFile).TryGetValue
            value = Me(key)
            If (value Is Nothing) Then
                Return ContainsKey(key)
            Else
                Return True
            End If
        End Function


        Public ReadOnly Property Values As ICollection(Of EmbeddedFile) Implements IDictionary(Of String, EmbeddedFile).Values
            Get
                Dim _values As List(Of EmbeddedFile) = New List(Of EmbeddedFile)
                Dim itemPairs As PdfArray = Me.BaseDataObject
                Dim length As Integer = itemPairs.Count
                For index As Integer = 1 To length - 1 Step 2
                    _values.Add(EmbeddedFile.Wrap(itemPairs(index)))
                Next
                Return _values
            End Get
        End Property

#Region "ICollection"

        Private Sub _Add(ByVal entry As KeyValuePair(Of String, EmbeddedFile)) Implements ICollection(Of KeyValuePair(Of String, EmbeddedFile)).Add
            Me.Add(entry.Key, entry.Value)
        End Sub

        Public Sub Clear() Implements ICollection(Of KeyValuePair(Of String, EmbeddedFile)).Clear
            Me.BaseDataObject.Clear()
        End Sub

        Private Function _Contains(ByVal entry As KeyValuePair(Of String, EmbeddedFile)) As Boolean Implements ICollection(Of KeyValuePair(Of String, EmbeddedFile)).Contains
            Return entry.Value.Equals(Me(entry.Key))
        End Function

        Public Sub CopyTo(ByVal entries As KeyValuePair(Of String, EmbeddedFile)(), ByVal Index As Integer) Implements ICollection(Of KeyValuePair(Of String, EmbeddedFile)).CopyTo
            Throw New NotImplementedException()
        End Sub

        Public ReadOnly Property Count As Integer Implements ICollection(Of KeyValuePair(Of String, EmbeddedFile)).Count
            Get
                Return Me.BaseDataObject.Count
            End Get
        End Property

        Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of KeyValuePair(Of String, EmbeddedFile)).IsReadOnly
            Get
                Return False
            End Get
        End Property

        Public Function Remove(ByVal entry As KeyValuePair(Of String, EmbeddedFile)) As Boolean Implements ICollection(Of KeyValuePair(Of String, EmbeddedFile)).Remove
            Throw New NotImplementedException()
        End Function

#Region "IEnumerable<KeyValuePair(Of String,EmbeddedFile)>"

        '    IEnumerator<KeyValuePair(Of String,EmbeddedFile)> IEnumerable<KeyValuePair(Of String,EmbeddedFile)>.GetEnumerator(
        '  )
        '{
        '  PdfArray itemPairs = BaseDataObject;
        '  For (
        '    int index = 0,
        '      length = itemPairs.Count;
        '    index < length;
        '    index += 2
        '    )
        '  {
        '    yield return New KeyValuePair(of String,EmbeddedFile)(
        '      (string)((PdfTextString)itemPairs(index)).Value,
        '      EmbeddedFile.Wrap(itemPairs[index+1])
        '      );
        '  }
        '}

        Private Class mEnumerator
            Implements IEnumerator(Of KeyValuePair(Of String, EmbeddedFile))

            Public o As RelatedFiles
            Private length As Integer
            Private index As Integer
            Private itemPairs As PdfArray

            Public Sub New(ByVal o As RelatedFiles)
                Me.o = o
                Me.Reset()
            End Sub

            Public ReadOnly Property Current As KeyValuePair(Of String, EmbeddedFile) Implements IEnumerator(Of KeyValuePair(Of String, EmbeddedFile)).Current
                Get
                    Return New KeyValuePair(Of String, EmbeddedFile)(CStr(CType(Me.itemPairs(Me.index), PdfTextString).Value), EmbeddedFile.Wrap(Me.itemPairs(Me.index + 1)))
                End Get
            End Property

            Private ReadOnly Property IEnumerator_Current As Object Implements IEnumerator.Current
                Get
                    Return Me.Current
                End Get
            End Property

            Public Sub Reset() Implements IEnumerator.Reset
                Me.index = -1
                Me.length = Me.o.Count
                Me.itemPairs = Me.o.BaseDataObject
            End Sub

            Public Function MoveNext() As Boolean Implements IEnumerator.MoveNext
                If (Me.index + 2 < Me.length) Then
                    Me.index += 2
                    Return True
                Else
                    Return False
                End If
            End Function



            ' Questo codice viene aggiunto da Visual Basic per implementare in modo corretto il criterio Disposable.
            Public Sub Dispose() Implements IDisposable.Dispose
                Me.o = Nothing
                Me.itemPairs = Nothing
                ' TODO: rimuovere il commento dalla riga seguente se è stato eseguito l'override di Finalize().
                ' GC.SuppressFinalize(Me)
            End Sub

        End Class

        Private Function GetEnumerator() As IEnumerator(Of KeyValuePair(Of String, EmbeddedFile)) Implements IEnumerable(Of KeyValuePair(Of String, EmbeddedFile)).GetEnumerator
            Return New mEnumerator(Me)
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
#End Region
#End Region

    End Class

End Namespace
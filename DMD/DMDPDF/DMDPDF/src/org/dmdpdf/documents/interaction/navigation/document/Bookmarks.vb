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

Imports DMD.org.dmdpdf
Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects

Imports System
Imports System.Collections
Imports System.Collections.Generic

Namespace DMD.org.dmdpdf.documents.interaction.navigation.document

    '/**
    '  <summary>Collection of bookmarks [PDF:1.6:8.2.2].</summary>
    '*/
    <PDF(VersionEnum.PDF10)>
    Public NotInheritable Class Bookmarks
        Inherits PdfObjectWrapper(Of PdfDictionary)
        Implements IList(Of Bookmark)

#Region "Static"
#Region "Interface"
#Region "Public"

        Public Shared Function Wrap(ByVal baseObject As PdfDirectObject) As Bookmarks
            If (baseObject IsNot Nothing) Then
                Return New Bookmarks(baseObject)
            Else
                Return Nothing
            End If
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        Public Sub New(ByVal context As documents.Document)
            MyBase.New(
                        context,
                        New PdfDictionary(New PdfName() {PdfName.Type, PdfName.Count}, New PdfDirectObject() {PdfName.Outlines, PdfInteger.Default})
                        )
        End Sub

        Private Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "Interface"
#Region "Public"
#Region "IList"

        Public Function IndexOf(ByVal bookmark As Bookmark) As Integer Implements IList(Of Bookmark).IndexOf
            Throw New NotImplementedException()
        End Function

        Public Sub Insert(ByVal index As Integer, ByVal bookmark As Bookmark) Implements IList(Of Bookmark).Insert
            Throw New NotImplementedException()
        End Sub

        Public Sub RemoveAt(ByVal index As Integer) Implements IList(Of Bookmark).RemoveAt
            Throw New NotImplementedException()
        End Sub

        Default Public Property Item(ByVal index As Integer) As Bookmark Implements IList(Of Bookmark).Item
            Get
                Dim bookmarkObject As PdfReference = CType(BaseDataObject(PdfName.First), PdfReference)
                While (index > 0)
                    bookmarkObject = CType(CType(bookmarkObject.DataObject, PdfDictionary)(PdfName.Next), PdfReference)
                    ' Did we go past the collection range?
                    If (bookmarkObject Is Nothing) Then Throw New ArgumentOutOfRangeException()
                    index -= 1
                End While
                Return New Bookmark(bookmarkObject)
            End Get
            Set(ByVal value As Bookmark)
                Throw New NotImplementedException()
            End Set
        End Property

#Region "ICollection"

        Public Sub Add(ByVal bookmark As Bookmark) Implements ICollection(Of Bookmark).Add
            '/*
            '  NOTE:   Bookmarks imported from alien PDF files MUST be cloned
            '  before being added.
            '*/
            bookmark.BaseDataObject(PdfName.Parent) = BaseObject

            Dim countObject As PdfInteger = EnsureCountObject()
            ' Is it the first bookmark?
            If (CInt(countObject.Value) = 0) Then 'First bookmark.
                BaseDataObject(PdfName.First) = bookmark.BaseObject
                BaseDataObject(PdfName.Last) = BaseDataObject(PdfName.First)
                BaseDataObject(PdfName.Count) = PdfInteger.Get(countObject.IntValue + 1)
            Else ' Non-first bookmark.
                Dim oldLastBookmarkReference As PdfReference = CType(BaseDataObject(PdfName.Last), PdfReference)
                ' Added bookmark Is the last In the collection...
                CType(oldLastBookmarkReference.DataObject, PdfDictionary)(PdfName.Next) = bookmark.BaseObject
                BaseDataObject(PdfName.Last) = CType(oldLastBookmarkReference.DataObject, PdfDictionary)(PdfName.Next) ' ...And the next of the previously-last bookmark.
                bookmark.BaseDataObject(PdfName.Prev) = oldLastBookmarkReference
                '/*
                '  NOTE:     The Count entry Is a relative number (whose sign represents
                '  the node open state).
                '*/
                BaseDataObject(PdfName.Count) = PdfInteger.Get(countObject.IntValue + Math.Sign(countObject.IntValue))
            End If
        End Sub

        Public Sub Clear() Implements ICollection(Of Bookmark).Clear
            Throw New NotImplementedException()
        End Sub


        Public Function Contains(ByVal bookmark As Bookmark) As Boolean Implements ICollection(Of Bookmark).Contains
            Throw New NotImplementedException()
        End Function

        Public Sub CopyTo(ByVal bookmarks As Bookmark(), ByVal index As Integer) Implements ICollection(Of Bookmark).CopyTo
            Throw New NotImplementedException()
        End Sub

        Public ReadOnly Property Count As Integer Implements ICollection(Of Bookmark).Count
            Get
                '/*
                '  NOTE: The Count entry may be absent [PDF: 1.6:8.2.2].
                '*/
                Dim countObject As PdfInteger = CType(BaseDataObject(PdfName.Count), PdfInteger)
                If (countObject Is Nothing) Then Return 0
                Return countObject.RawValue
            End Get
        End Property

        Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of Bookmark).IsReadOnly
            Get
                Return False
            End Get
        End Property

        Public Function Remove(ByVal bookmark As Bookmark) As Boolean Implements ICollection(Of Bookmark).Remove
            Throw New NotImplementedException()
        End Function


#Region "IEnumerable<ContentStream>"

        Private Function _GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Return Me.GetEnumerator
        End Function

        Private Class mEnumerator
            Implements IEnumerator(Of Bookmark)

            Private o As Bookmarks
            Private bookmarkObject As PdfDirectObject

            Public Sub New(ByVal o As Bookmarks)
                Me.o = o
                Me.Reset()
            End Sub

            Public Function MoveNext() As Boolean Implements IEnumerator.MoveNext
                If (Me.bookmarkObject Is Nothing) Then
                    Me.bookmarkObject = o.BaseDataObject(PdfName.First)
                Else
                    Me.bookmarkObject = CType(bookmarkObject.Resolve(), PdfDictionary)(PdfName.Next)
                End If

                Return (Me.bookmarkObject IsNot Nothing)
            End Function

            Public Sub Reset() Implements IEnumerator.Reset
                Me.bookmarkObject = Nothing
            End Sub

            Public ReadOnly Property Current As Bookmark Implements IEnumerator(Of Bookmark).Current
                Get
                    If (Me.bookmarkObject Is Nothing) Then Return Nothing
                    Dim value As New Bookmark(Me.bookmarkObject)
                    Debug.Print("Bookmarks.Current -> " & value.ToString)
                    Return value
                End Get
            End Property

            Private ReadOnly Property IEnumerator_Current As Object Implements IEnumerator.Current
                Get
                    Return Me.Current
                End Get
            End Property



            ' Questo codice viene aggiunto da Visual Basic per implementare in modo corretto il criterio Disposable.
            Public Sub Dispose() Implements IDisposable.Dispose
                Me.o = Nothing
                Me.bookmarkObject = Nothing
                ' TODO: rimuovere il commento dalla riga seguente se è stato eseguito l'override di Finalize().
                ' GC.SuppressFinalize(Me)
            End Sub

        End Class

        Public Function GetEnumerator() As IEnumerator(Of Bookmark) Implements ICollection(Of Bookmark).GetEnumerator
            Return New mEnumerator(Me)
        End Function
#End Region
#End Region
#End Region
#End Region
#End Region

#Region "private"

        '/**
        '  <summary>Gets the count object, forcing its creation if it doesn't
        '  exist.</summary>
        '*/
        Private Function EnsureCountObject() As PdfInteger
            '/*
            '  NOTE: The Count entry may be absent [PDF:1.6:8.2.2].
            '*/
            Dim countObject As PdfInteger = CType(BaseDataObject(PdfName.Count), PdfInteger)
            If (countObject Is Nothing) Then
                countObject = PdfInteger.Default
                BaseDataObject(PdfName.Count) = countObject
            End If
            Return countObject
        End Function

#End Region
#End Region



    End Class

End Namespace
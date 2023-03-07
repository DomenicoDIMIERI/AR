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

'  You should have received a copy of the GNU Lesser General Public License along with Me
'  Program (see README files); if not, go to the GNU website (http://www.gnu.org/licenses/).

'  Redistribution and use, with or without modification, are permitted provided that such
'  redistributions retain the above copyright notice, license and disclaimer, along with
'  Me list of conditions.
'*/

Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects
Imports DMD.org.dmdpdf.util.collections.generic

Imports System
Imports System.Collections
Imports System.Collections.Generic

Namespace DMD.org.dmdpdf.documents

    '/**
    '  <summary>Document pages collection [PDF:1.6:3.6.2].</summary>
    '*/
    <PDF(VersionEnum.PDF10)>
    Public NotInheritable Class Pages
        Inherits PdfObjectWrapper(Of PdfDictionary)
        Implements IExtList(Of Page), IList(Of Page)

#Region "types"

        Private Class Enumerator
            Implements IEnumerator(Of Page)

            '      /**
            '  <summary>Collection size.</summary>
            '*/
            Private _count As Integer

            '/**
            '  <summary>Index of the next item.</summary>
            '*/
            Private _index As Integer = 0

            '/**
            '  <summary>Current page.</summary>
            '*/
            Private _current As Page

            '/**
            '  <summary>Current level index.</summary>
            '*/
            Private _levelIndex As Integer = 0
            '/**
            '  <summary>Stacked level indexes.</summary>
            '*/
            Private _levelIndexes As New Stack(Of Integer)

            '/**
            '  <summary>Current child tree nodes.</summary>
            '*/
            Private _kids As PdfArray
            '/**
            '  <summary>Current parent tree node.</summary>
            '*/
            Private _parent As PdfDictionary

            Friend Sub New(ByVal pages As Pages)
                Me._count = pages.Count
                Me._parent = pages.BaseDataObject
                Me._kids = CType(Me._parent.Resolve(PdfName.Kids), PdfArray)
            End Sub

            Public ReadOnly Property Current As Page Implements IEnumerator(Of Page).Current
                Get
                    Debug.Print("Pages.Current -> " & Me._current.ToString)
                    Return Me._current
                End Get
            End Property

            Private ReadOnly Property Current_ As Object Implements IEnumerator.Current
                Get
                    Return Me.Current ' ((IEnumerator<Page>)Me).Current;}
                End Get
            End Property

            Public Function MoveNext() As Boolean Implements IEnumerator.MoveNext
                If (Me._index = Me._count) Then Return False

                '/*
                '  NOTE: As stated in [PDF:1.6:3.6.2], page retrieval is a matter of diving
                '  inside a B-tree.
                '  This is a special adaptation of the get() algorithm necessary to keep
                '  a low overhead throughout the page tree scan (using the get() method
                '  would have implied a nonlinear computational cost).
                '*/
                '/*
                '  NOTE: Algorithm:
                '  1. [Vertical, down] We have to go downward the page tree till we reach
                '  a page (leaf node).
                '  2. [Horizontal] Then we iterate across the page collection it belongs to,
                '  repeating step 1 whenever we find a subtree.
                '  3. [Vertical, up] When leaf-nodes scan is complete, we go upward solving
                '  parent nodes, repeating step 2.
                '*/
                While (True)
                    ' Did we complete current page-tree-branch level?
                    If (Me._kids.Count = Me._levelIndex) Then ' PageThenThen Then subtree complete.
                        ' 3.Go upward one level.
                        ' Restore node index at the current level!
                        Me._levelIndex = Me._levelIndexes.Pop() + 1 ' Next node (partially scanned level).
                        ' Move upward!
                        Me._parent = CType(Me._parent.Resolve(PdfName.Parent), PdfDictionary)
                        Me._kids = CType(Me._parent.Resolve(PdfName.Kids), PdfArray)
                    Else ' Page subtree incomplete.
                        Dim kidReference As PdfReference = CType(Me._kids(Me._levelIndex), PdfReference)
                        Dim kid As PdfDictionary = CType(kidReference.DataObject, PdfDictionary)
                        ' Is current kid a page object?
                        If (kid(PdfName.Type).Equals(PdfName.Page)) Then '// Page object.
                            ' 2. Page found.
                            Me._index += 1 ' Absolute page index.
                            Me._levelIndex += 1 ' Current level node index.

                            Me._current = Page.Wrap(kidReference)
                            Return True
                        Else  '// Page tree node.
                            ' 1. Go downward one level.
                            ' Save node index at the current level!
                            Me._levelIndexes.Push(Me._levelIndex)
                            ' Move downward!
                            Me._parent = kid
                            Me._kids = CType(Me._parent.Resolve(PdfName.Kids), PdfArray)
                            Me._levelIndex = 0 ' First node (New level).
                        End If
                    End If
                End While

                Return False
            End Function

            Public Sub Reset() Implements IEnumerator.Reset
                Throw New NotSupportedException()
            End Sub

            Public Sub Dispose() Implements IDisposable.Dispose
                Me._current = Nothing
                Me._kids = Nothing
                Me._parent = Nothing
                Me._levelIndexes = Nothing
            End Sub
        End Class

#End Region

        '/*
        '  TODO:IMPL A B-tree algorithm should be implemented to optimize the inner layout
        '  of the page tree (better insertion/deletion performance). In Me case, it would
        '  be necessary to keep track of the modified tree nodes for incremental update.
        '*/
#Region "dynamic"
#Region "constructors"

        Friend Sub New(ByVal context As Document)
            MyBase.New(context,
                            New PdfDictionary(
                                    New PdfName() {PdfName.Type, PdfName.Kids, PdfName.Count},
                                    New PdfDirectObject() {PdfName.Pages, New PdfArray(), PdfInteger.Default}
                                    )
                        )
        End Sub


        Friend Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "Interface"
#Region "Public"

#Region "IExtList<Page>"

        Public Function GetRange(ByVal index As Integer, ByVal count As Integer) As IList(Of Page) Implements IExtList(Of Page).GetRange
            Return GetSlice(index, index + count)
        End Function

        Public Function GetSlice(ByVal fromIndex As Integer, ByVal toIndex As Integer) As IList(Of Page) Implements IExtList(Of Page).GetSlice
            Dim pages As New List(Of Page)(toIndex - fromIndex)
            Dim i As Integer = fromIndex
            While (i < toIndex)
                pages.Add(Me(i))
                i += 1
            End While

            Return pages
        End Function

        Public Sub InsertAll(Of TVar As Page)(ByVal Index As Integer, ByVal Pages As ICollection(Of TVar)) Implements IExtList(Of Page).InsertAll
            CommonAddAll(Index, Pages)
        End Sub

#Region "IExtCollection<Page>"

        Public Sub AddAll(Of TVar As Page)(ByVal pages As ICollection(Of TVar)) Implements IExtCollection(Of Page).AddAll
            CommonAddAll(-1, pages)
        End Sub

        Public Sub RemoveAll(Of TVar As Page)(ByVal pages As ICollection(Of TVar)) Implements IExtCollection(Of Page).RemoveAll
            '/*
            '  NOTE: The interface contract doesn't prescribe any relation among the removing-collection's
            '  items, so we cannot adopt the optimized approach of the add*(...) methods family,
            '  where adding-collection's items are explicitly ordered.
            '*/
            For Each page As Page In pages
                Remove(page)
            Next
        End Sub

        Public Function RemoveAll(ByVal match As Predicate(Of Page)) As Integer Implements IExtCollection(Of Page).RemoveAll
            '/*
            '  NOTE: Removal is indirectly fulfilled through an intermediate collection
            '  in order not to interfere with the enumerator execution.
            '*/
            Dim removingPages As New List(Of Page)
            For Each page As Page In Me
                If (match(page)) Then removingPages.Add(page)
            Next
            RemoveAll(removingPages)
            Return removingPages.Count
        End Function

#End Region
#End Region

#Region "IList(Of Page)"

        Public Function IndexOf(ByVal page As Page) As Integer Implements IList(Of Page).IndexOf
            Return page.Index
        End Function

        Public Sub Insert(ByVal index As Integer, ByVal page As Page) Implements IList(Of Page).Insert
            CommonAddAll(index, CType(New Page() {page}, ICollection(Of Page)))
        End Sub

        Public Sub RemoveAt(ByVal index As Integer) Implements IList(Of Page).RemoveAt
            Remove(Me(index))
        End Sub

        Default Public Property Item(ByVal index As Integer) As Page Implements IList(Of Page).Item
            Get
                '/*
                '  NOTE: As stated in [PDF:1.6:3.6.2], to retrieve pages is a matter of diving
                '  inside a B-tree. To keep it as efficient as possible, Me implementation
                '  does NOT adopt recursion to deepen its search, opting for an iterative
                '  strategy instead.
                '*/
                Dim pageOffset As Integer = 0
                Dim parent As PdfDictionary = Me.BaseDataObject
                Dim kids As PdfArray = CType(parent.Resolve(PdfName.Kids), PdfArray)
                For i As Integer = 0 To kids.Count - 1
                    Dim kidReference As PdfReference = CType(kids(i), PdfReference)
                    Dim kid As PdfDictionary = CType(kidReference.DataObject, PdfDictionary)
                    ' Is current kid a page object?
                    If (kid(PdfName.Type).Equals(PdfName.Page)) Then ' Page object.
                        ' Did we reach the searched position?
                        If (pageOffset = index) Then ' VerticalThen Then scan (we finished).
                            ' We got it!
                            Return Page.Wrap(kidReference)
                        Else ' Horizontal scan (go past).
                            ' Cumulate current page object count!
                            pageOffset += 1
                        End If
                    Else ' Page tree node.
                        ' Does the current subtree contain the searched page?
                        If (CType(kid(PdfName.Count), PdfInteger).RawValue + pageOffset > index) Then ' Vertical scan (deepen the search).
                            ' Go down one level!
                            parent = kid
                            kids = CType(parent.Resolve(PdfName.Kids), PdfArray)
                            i = -1
                        Else ' Horizontal scan (go past).
                            ' Cumulate current subtree count!
                            pageOffset += CType(kid(PdfName.Count), PdfInteger).RawValue
                        End If
                    End If
                Next
                Return Nothing
            End Get
            Set(ByVal value As Page)
                RemoveAt(index)
                Insert(index, value)
            End Set
        End Property

#Region "ICollection<Page>"

        Public Sub Add(ByVal page As Page) Implements ICollection(Of Page).Add
            CommonAddAll(-1, CType(New Page() {page}, ICollection(Of Page)))
        End Sub

        Public Sub Clear() Implements ICollection(Of Page).Clear
            Throw New NotImplementedException()
        End Sub

        Public Function Contains(ByVal page As Page) As Boolean Implements ICollection(Of Page).Contains
            Throw New NotImplementedException()
        End Function

        Public Sub CopyTo(ByVal pages As Page(), ByVal index As Integer) Implements ICollection(Of Page).CopyTo
            Throw New NotImplementedException()
        End Sub

        Public ReadOnly Property Count As Integer Implements ICollection(Of Page).Count
            Get
                Return CType(BaseDataObject(PdfName.Count), PdfInteger).RawValue
            End Get
        End Property

        Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of documents.Page).IsReadOnly
            Get
                Return False
            End Get
        End Property

        Public Function Remove(ByVal page As Page) As Boolean Implements ICollection(Of Page).Remove
            Dim pageData As PdfDictionary = page.BaseDataObject
            ' Get the parent tree node!
            Dim parent As PdfDirectObject = pageData(PdfName.Parent)
            Dim parentData As PdfDictionary = CType(parent.Resolve(), PdfDictionary)
            ' Get the parent's page collection!
            Dim kids As PdfDirectObject = parentData(PdfName.Kids)
            Dim kidsData As PdfArray = CType(kids.Resolve(), PdfArray)
            ' Remove the page!
            kidsData.Remove(page.BaseObject)

            ' Unbind the page from its parent!
            pageData(PdfName.Parent) = Nothing

            ' Decrementing the pages counters...
            Do
                ' Get the page collection counter!
                Dim countObject As PdfInteger = CType(parentData(PdfName.Count), PdfInteger)
                ' Decrement the counter at the current level!
                parentData(PdfName.Count) = PdfInteger.Get(countObject.IntValue - 1)

                ' Iterate upward!
                parent = parentData(PdfName.Parent)
                parentData = CType(PdfObject.Resolve(parent), PdfDictionary)
            Loop While (parent IsNot Nothing)

            Return True
        End Function

#Region "IEnumerable<Page>"

        Public Function GetEnumerator() As IEnumerator(Of Page) Implements IEnumerable(Of Page).GetEnumerator
            Return New Enumerator(Me)
        End Function

#Region "IEnumerable"
        Private Function GetEnumerator_() As IEnumerator Implements IEnumerable.GetEnumerator
            Return Me.GetEnumerator()
        End Function

#End Region
#End Region
#End Region
#End Region
#End Region

#Region "private"
        '/**
        '  Add a collection of pages at the specified position.
        '  <param name="index">Addition position. To append, use value -1.</param>
        '  <param name="pages">Collection of pages to add.</param>
        '*/
        Private Sub CommonAddAll(Of TPage As Page)(ByVal index As Integer, ByVal pages As ICollection(Of TPage))
            Dim parent As PdfDirectObject
            Dim parentData As PdfDictionary
            Dim kids As PdfDirectObject
            Dim kidsData As PdfArray
            Dim offset As Integer
            ' Append operation?
            If (index = -1) Then ' AppendThen operation.
                ' Get the parent tree node!
                parent = BaseObject
                parentData = BaseDataObject
                ' Get the parent's page collection!
                kids = parentData(PdfName.Kids)
                kidsData = CType(PdfObject.Resolve(kids), PdfArray)
                offset = 0 'Not used.
            Else ' Insert operation.
                ' Get the page currently at the specified position!
                Dim pivotPage As Page = Me(index)
                '   Get the parent tree node!
                parent = pivotPage.BaseDataObject(PdfName.Parent)
                parentData = CType(parent.Resolve(), PdfDictionary)
                ' Get the parent's page collection!
                kids = parentData(PdfName.Kids)
                kidsData = CType(kids.Resolve(), PdfArray)
                ' Get the insertion's relative position within the parent's page collection!
                offset = kidsData.IndexOf(pivotPage.BaseObject)
            End If

            ' Adding the pages...
            For Each page As Page In pages
                ' Append?
                If (index = -1) Then ' Append.
                    ' Append the page to the collection!
                    kidsData.Add(page.BaseObject)
                Else ' Insert.
                    ' Insert the page into the collection!
                    kidsData.Insert(offset, page.BaseObject) : offset += 1
                End If
                ' Bind the page to the collection!
                page.BaseDataObject(PdfName.Parent) = parent
            Next

            ' Incrementing the pages counters...
            Do
                ' Get the page collection counter!
                Dim countObject As PdfInteger = CType(parentData(PdfName.Count), PdfInteger)
                ' Increment the counter at the current level!
                parentData(PdfName.Count) = PdfInteger.Get(countObject.IntValue + pages.Count)

                ' Iterate upward!
                parent = parentData(PdfName.Parent)
                parentData = CType(PdfObject.Resolve(parent), PdfDictionary)
            Loop While (parent IsNot Nothing)
        End Sub
#End Region
#End Region
#End Region
    End Class

End Namespace

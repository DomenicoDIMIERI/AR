'/*
'  Copyright 2012 Stefano Chizzolini. http://www.dmdpdf.org

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

Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.objects
Imports DMD.org.dmdpdf.util

Imports System
Imports System.Collections
Imports System.Collections.Generic

Namespace DMD.org.dmdpdf.documents.interaction.navigation.page

    '/**
    '  <summary>Article bead [PDF:1.7:8.3.2].</summary>
    '*/
    <PDF(VersionEnum.PDF11)>
    Public NotInheritable Class ArticleElements
        Inherits PdfObjectWrapper(Of PdfDictionary)
        Implements IList(Of ArticleElement)

#Region "types"
        Private NotInheritable Class ElementCounter
            Inherits ElementEvaluator

            Public ReadOnly Property Count As Integer
                Get
                    Return Me._index + 1
                End Get
            End Property
        End Class

        Private Class ElementEvaluator
            Implements IPredicate

            '      /**
            '  Current position.
            '*/
            Protected _index As Integer = -1

            Public Overridable Function Evaluate(ByVal [object] As Object) As Boolean Implements IPredicate.Evaluate
                Me._index += 1
                Return False
            End Function
        End Class

        Private NotInheritable Class ElementGetter
            Inherits ElementEvaluator

            Private _bead As PdfDictionary
            Private ReadOnly _beadIndex As Integer

            Public Sub New(ByVal beadIndex As Integer)
                Me._beadIndex = beadIndex
            End Sub

            Public Overrides Function Evaluate(ByVal [object] As Object) As Boolean
                MyBase.Evaluate([object])
                If (Me._index = Me._beadIndex) Then
                    Me._bead = CType([object], PdfDictionary)
                    Return True
                End If
                Return False
            End Function

            Public ReadOnly Property Bead As PdfDictionary
                Get
                    Return Me._bead
                End Get
            End Property
        End Class

        Private NotInheritable Class ElementIndexer
            Inherits ElementEvaluator

            Private ReadOnly _searchedBead As PdfDictionary

            Public Sub New(ByVal searchedBead As PdfDictionary)
                Me._searchedBead = searchedBead
            End Sub

            Public Overrides Function Evaluate(ByVal [object] As Object) As Boolean
                MyBase.Evaluate([object])
                Return [object].Equals(Me._searchedBead)
            End Function

            Public ReadOnly Property Index As Integer
                Get
                    Return Me._index
                End Get
            End Property
        End Class

        Private NotInheritable Class ElementListBuilder
            Inherits ElementEvaluator

            Public _elements As IList(Of ArticleElement) = New List(Of ArticleElement)

            Public Overrides Function Evaluate(ByVal [object] As Object) As Boolean
                Me._elements.Add(ArticleElement.Wrap(CType([object], PdfDirectObject)))
                Return False
            End Function

            Public ReadOnly Property Elements As IList(Of ArticleElement)
                Get
                    Return Me._elements
                End Get
            End Property
        End Class

        Private Class Enumerator
            Implements IEnumerator(Of ArticleElement)

            Private _currentObject As PdfDirectObject
            Private ReadOnly _firstObject As PdfDirectObject
            Private _nextObject As PdfDirectObject

            Friend Sub New(ByVal elements As ArticleElements)
                Me._firstObject = elements.BaseDataObject(PdfName.F)
                Me._nextObject = Me._firstObject
            End Sub

            Public ReadOnly Property Current As ArticleElement Implements IEnumerator(Of ArticleElement).Current
                Get
                    Return ArticleElement.Wrap(Me._currentObject)
                End Get
            End Property

            Private ReadOnly Property _Current As Object Implements IEnumerator.Current
                Get
                    Return Me.Current ' ((IEnumerator(Of ArticleElement))Me).Current;}
                End Get
            End Property


            Public Function MoveNext() As Boolean Implements IEnumerator.MoveNext
                If (Me._nextObject Is Nothing) Then Return False
                Me._currentObject = Me._nextObject
                Me._nextObject = CType(_currentObject.Resolve(), PdfDictionary)(PdfName.N)
                If (Me._nextObject Is Me._firstObject) Then ' LoopingThen back.
                    Me._nextObject = Nothing
                End If
                Return True
            End Function

            Public Sub Reset() Implements IEnumerator.Reset
                Throw New NotSupportedException()
            End Sub

            Public Sub Dispose() Implements IDisposable.Dispose

            End Sub

        End Class

#End Region

#Region "Static"
#Region "Interface"
#Region "Public"

        Public Shared Function Wrap(ByVal baseObject As PdfDirectObject) As ArticleElements
            If (baseObject IsNot Nothing) Then
                Return New ArticleElements(baseObject)
            Else
                Return Nothing
            End If
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        Private Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "Interface"
#Region "Public"
#Region "IList(Of ArticleElement)"

        Public Function IndexOf(ByVal [object] As ArticleElement) As Integer Implements IList(Of ArticleElement).IndexOf
            If ([object] Is Nothing) Then Return -1 'NOTE: By definition, no bead can be null.
            Dim indexer As ElementIndexer = New ElementIndexer([object].BaseDataObject)
            Iterate(indexer)
            Return indexer.Index
        End Function

        Public Sub Insert(ByVal index As Integer, ByVal [Object] As ArticleElement) Implements IList(Of ArticleElement).Insert
            If (index < 0) Then Throw New ArgumentOutOfRangeException()
            Dim getter As ElementGetter = New ElementGetter(index)
            Iterate(getter)
            Dim bead As PdfDictionary = getter.Bead
            If (bead Is Nothing) Then
                Add([Object])
            Else
                Link([Object].BaseDataObject, bead)
            End If
        End Sub

        Public Sub RemoveAt(ByVal index As Integer) Implements IList(Of ArticleElement).RemoveAt
            Unlink(Me(index).BaseDataObject)
        End Sub

        Default Public Property Item(ByVal index As Integer) As ArticleElement Implements IList(Of ArticleElement).Item
            Get
                If (index < 0) Then Throw New ArgumentOutOfRangeException()
                Dim getter As ElementGetter = New ElementGetter(index)
                Iterate(getter)
                Dim bead As PdfDictionary = getter.Bead
                If (bead Is Nothing) Then Throw New ArgumentOutOfRangeException()
                Return ArticleElement.Wrap(bead.Reference)
            End Get
            Set(ByVal value As ArticleElement)
                Throw New NotImplementedException()
            End Set
        End Property

#Region "ICollection<TItem>"

        Public Sub Add(ByVal [object] As ArticleElement) Implements ICollection(Of ArticleElement).Add
            Dim itemBead As PdfDictionary = [object].BaseDataObject
            Dim firstBead As PdfDictionary = Me.FirstBead
            If (firstBead IsNot Nothing) Then ' Non - emptyThen list.
                Link(itemBead, firstBead)
            Else ' Empty list.
                Me.FirstBead = itemBead
                Me.Link(itemBead, itemBead)
            End If
        End Sub

        Public Sub Clear() Implements ICollection(Of ArticleElement).Clear
            Throw New NotImplementedException()
        End Sub

        Public Function Contains(ByVal [object] As ArticleElement) As Boolean Implements ICollection(Of ArticleElement).Contains
            Return IndexOf([object]) >= 0
        End Function

        Public Sub CopyTo(ByVal objects As ArticleElement(), ByVal index As Integer) Implements ICollection(Of ArticleElement).CopyTo
            Throw New NotImplementedException()
        End Sub

        Public ReadOnly Property Count As Integer Implements ICollection(Of ArticleElement).Count
            Get
                Dim counter As ElementCounter = New ElementCounter()
                Iterate(counter)
                Return counter.Count
            End Get
        End Property

        Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of ArticleElement).IsReadOnly
            Get
                Return False
            End Get
        End Property

        Public Function Remove(ByVal [object] As ArticleElement) As Boolean Implements ICollection(Of ArticleElement).Remove
            If (Not Contains([object])) Then Return False
            Unlink([object].BaseDataObject)
            Return True
        End Function

#Region "IEnumerable(Of ArticleElement)"

        Public Function GetEnumerator() As IEnumerator(Of ArticleElement) Implements IEnumerable(Of ArticleElement).GetEnumerator
            Return New Enumerator(Me)
        End Function

        Private Function _GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Return Me.GetEnumerator
        End Function

        '    Public IEnumerator(of ArticleElement) GetEnumerator(
        '  )
        '{return New Enumerator(Me);}

        '#Region IEnumerable
        'IEnumerator IEnumerable.GetEnumerator(
        '  )
        '{return ((IEnumerable(of ArticleElement))Me).GetEnumerator();}
#End Region
#End Region
#End Region
#End Region
#End Region

#Region "Private"

        Private Property FirstBead As PdfDictionary
            Get
                Return CType(BaseDataObject.Resolve(PdfName.F), PdfDictionary)
            End Get
            Set(ByVal value As PdfDictionary)
                Dim oldValue As PdfDictionary = FirstBead
                BaseDataObject(PdfName.F) = PdfObject.Unresolve(value)
                If (value IsNot Nothing) Then
                    value(PdfName.T) = BaseObject
                End If
                If (oldValue IsNot Nothing) Then
                    oldValue.Remove(PdfName.T)
                End If
            End Set
        End Property

        Private Sub Iterate(ByVal predicate As IPredicate)
            Dim firstBead As PdfDictionary = Me.FirstBead
            Dim bead As PdfDictionary = Me.FirstBead
            While (bead IsNot Nothing)
                If (predicate.Evaluate(bead)) Then Exit While
                bead = CType(bead.Resolve(PdfName.N), PdfDictionary)
                If (bead Is firstBead) Then Exit While
            End While
        End Sub

        '/**
        '  <summary> Links the given item.</summary>
        '*/
        Private Sub Link(ByVal item As PdfDictionary, ByVal [next] As PdfDictionary)
            Dim previous As PdfDictionary = CType([next].Resolve(PdfName.V), PdfDictionary)
            If (previous Is Nothing) Then
                previous = [next]
            End If

            item(PdfName.N) = [next].Reference
            [next](PdfName.V) = item.Reference
            If (previous IsNot item) Then
                item(PdfName.V) = previous.Reference
                previous(PdfName.N) = item.Reference
            End If
        End Sub

        '/**
        '  <summary> Unlinks the given item.</summary>
        '  <remarks> It assumes the item Is contained In Me list.</remarks>
        '*/
        Private Sub Unlink(ByVal item As PdfDictionary)
            Dim prevBead As PdfDictionary = CType(item.Resolve(PdfName.V), PdfDictionary)
            item.Remove(PdfName.V)
            Dim nextBead As PdfDictionary = CType(item.Resolve(PdfName.N), PdfDictionary)
            item.Remove(PdfName.N)
            If (prevBead IsNot item) Then ' Still Then some elements.
                prevBead(PdfName.N) = nextBead.Reference
                nextBead(PdfName.V) = prevBead.Reference
                If (item Is Me.FirstBead) Then
                    Me.FirstBead = nextBead
                End If
            Else ' No more elements.
                Me.FirstBead = Nothing
            End If
        End Sub

#End Region
#End Region


    End Class

End Namespace
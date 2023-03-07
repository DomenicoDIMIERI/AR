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

Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.files

Imports System
Imports System.Collections
Imports System.Collections.Generic

Namespace DMD.org.dmdpdf.objects


    '/**
    '  <summary>Abstract tree [PDF:1.7:3.8.5].</summary>
    '*/
    <PDF(VersionEnum.PDF10)>
    Public MustInherit Class Tree(Of TKey As PdfDirectObject, TValue As PdfObjectWrapper) 'And IPdfSimpleObject
        Inherits PdfObjectWrapper(Of PdfDictionary)
        Implements IDictionary(Of TKey, TValue)

        '/*
        '  NOTE: This implementation is an adaptation of the B-tree algorithm described in "Introduction
        '  to Algorithms" (1), 2nd ed (Cormen, Leiserson, Rivest, Stein) published by MIT Press/McGraw-Hill.
        '  PDF trees represent a special subset of B-trees whereas actual keys are concentrated in leaf
        '  nodes and proxied by boundary limits across their paths. This simplifies some handling but
        '  requires keeping node limits updated whenever a change occurs in the leaf nodes composition.

        '  (1) http://en.wikipedia.org/wiki/Introduction_to_Algorithms
        '*/
#Region "types"
        '/**
        '  Node children.
        '*/
        Private NotInheritable Class Children

            Public NotInheritable Class InfoImpl

                Private Shared ReadOnly KidsInfo As InfoImpl = New InfoImpl(1, TreeLowOrder)
                Private Shared ReadOnly PairsInfo As InfoImpl = New InfoImpl(2, TreeLowOrder) ' NOTE: Paired children are combinations of 2 contiguous items.

                Public Shared Function [Get](ByVal typeName As PdfName) As InfoImpl
                    If (typeName.Equals(PdfName.Kids)) Then
                        Return KidsInfo
                    Else
                        Return PairsInfo
                    End If
                End Function

                ''' <summary>
                ''' Number of (contiguous) children defining an item.
                ''' </summary>
                Public ItemCount As Integer

                ''' <summary>
                ''' Maximum number of children.
                ''' </summary>
                Public MaxCount As Integer

                ''' <summary>
                ''' Minimum number of children.
                ''' </summary>
                Public MinCount As Integer

                Public Sub New(ByVal itemCount As Integer, ByVal lowOrder As Integer)
                    Me.ItemCount = itemCount
                    Me.MinCount = itemCount * lowOrder
                    Me.MaxCount = MinCount * 2
                End Sub
            End Class


            '/**
            '  <summary>Gets the given node's children.</summary>
            '  <param name="node">Parent node.</param>
            '  <param name="pairs">Pairs key.</param>
            '*/
            Public Shared Function [Get](ByVal node As PdfDictionary, ByVal _pairsKey As PdfName) As Children
                Dim childrenTypeName As PdfName
                If (node.ContainsKey(PdfName.Kids)) Then
                    childrenTypeName = PdfName.Kids
                ElseIf (node.ContainsKey(_pairsKey)) Then
                    childrenTypeName = _pairsKey
                Else
                    Throw New Exception("Malformed tree node.")
                End If

                Dim children As PdfArray = CType(node.Resolve(childrenTypeName), PdfArray)
                Return New Children(node, children, childrenTypeName)
            End Function

            ''' <summary>
            ''' Children's collection
            ''' </summary>
            Public ReadOnly Items As PdfArray

            ''' <summary>
            ''' Node's children info.
            ''' </summary>
            Public ReadOnly Info As InfoImpl

            ''' <summary>
            ''' Parent node.
            ''' </summary>
            Public ReadOnly Parent As PdfDictionary

            ''' <summary>
            ''' Node's children type.
            ''' </summary>
            Public ReadOnly TypeName As PdfName

            Private Sub New(ByVal parent As PdfDictionary, ByVal items As PdfArray, ByVal typeName As PdfName)
                Me.Parent = parent
                Me.Items = items
                Me.TypeName = typeName
                Info = InfoImpl.Get(typeName)
            End Sub

            ''' <summary>
            ''' Gets whether the collection size has reached its maximum.
            ''' </summary>
            ''' <returns></returns>
            Public Function IsFull() As Boolean
                Return Items.Count >= Info.MaxCount
            End Function

            ''' <summary>
            ''' Gets whether Me collection represents a leaf node.
            ''' </summary>
            ''' <returns></returns>
            Public Function IsLeaf() As Boolean
                Return Not TypeName.Equals(PdfName.Kids)
            End Function

            ''' <summary>
            ''' Gets whether the collection size is more than its maximum.s
            ''' </summary>
            ''' <returns></returns>
            Public Function IsOversized() As Boolean
                Return Items.Count > Info.MaxCount
            End Function

            ''' <summary>
            ''' Gets whether the collection size is less than its minimum.
            ''' </summary>
            ''' <returns></returns>
            Public Function IsUndersized() As Boolean
                Return Items.Count < Info.MinCount
            End Function

            ''' <summary>
            ''' Gets whether the collection size is within the order limits.
            ''' </summary>
            ''' <returns></returns>
            Public Function IsValid() As Boolean
                Return Not (IsUndersized() OrElse IsOversized())
            End Function

        End Class

        Private Class Enumerator
            Implements IEnumerator(Of KeyValuePair(Of TKey, TValue))

#Region "dynamic"
#Region "fields"

            ''' <summary>
            ''' Current named object.
            ''' </summary>
            Private _current As KeyValuePair(Of TKey, TValue)?

            ''' <summary>
            ''' Current level index.
            ''' </summary>
            Private _levelIndex As Integer = 0

            ''' <summary>
            ''' Stacked levels.
            ''' </summary>
            Private _levels As New Stack(Of Object())

            ''' <summary>
            ''' Current child tree nodes.
            ''' </summary>
            Private _kids As PdfArray

            ''' <summary>
            ''' Current names.
            ''' </summary>
            Private _names As PdfArray

            ''' <summary>
            ''' Current container.
            ''' </summary>
            Private _container As PdfIndirectObject

            ''' <summary>
            ''' Name tree.
            ''' </summary>
            Private _tree As Tree(Of TKey, TValue)
#End Region

#Region "constructors"
            Friend Sub New(ByVal tree As Tree(Of TKey, TValue))
                Me._tree = tree
                Me._container = tree.Container
                Dim rootNode As PdfDictionary = tree.BaseDataObject
                Dim kidsObject As PdfDirectObject = rootNode(PdfName.Kids)
                If (kidsObject Is Nothing) Then ' LeafThen Then node.
                    Dim namesObject As PdfDirectObject = rootNode(PdfName.Names)
                    If (TypeOf (namesObject) Is PdfReference) Then
                        Me._container = CType(namesObject, PdfReference).IndirectObject
                    End If
                    Me._names = CType(namesObject.Resolve(), PdfArray)
                Else ' Intermediate node.
                    If (TypeOf (kidsObject) Is PdfReference) Then
                        Me._container = CType(kidsObject, PdfReference).IndirectObject
                    End If
                    Me._kids = CType(kidsObject.Resolve(), PdfArray)
                End If
            End Sub
#End Region

#Region "interface"
#Region "public"
#Region "IEnumerator(of KeyValuePair(of TKey,TValue))"

            Public ReadOnly Property Current As KeyValuePair(Of TKey, TValue) Implements IEnumerator(Of KeyValuePair(Of TKey, TValue)).Current
                Get
                    Debug.Print("Tree.Current -> " & Me._current.Value.ToString())
                    Return Me._current.Value
                End Get
            End Property

#Region "IEnumerator"
            Private ReadOnly Property Current1 As Object Implements IEnumerator.Current
                Get
                    '{return ((IEnumerator(of KeyValuePair(of TKey,TValue)))Me).Current;}
                    Return Me.Current
                End Get
            End Property

            Public Function MoveNext() As Boolean Implements IEnumerator.MoveNext
                Me._current = GetNext()
                Return Me._current IsNot Nothing
            End Function


            Public Sub Reset() Implements IEnumerator.Reset
                Throw New NotSupportedException()
            End Sub
#End Region

#Region "IDisposable"
            Public Sub Dispose() Implements IDisposable.Dispose
                'GC.SuppressFinalize(Me) ' DMD

            End Sub

#End Region
#End Region
#End Region

#Region "Private"
            Private Function GetNext() As KeyValuePair(Of TKey, TValue)?
                '/*
                '  NOTE: Algorithm:
                '  1. [Vertical, down] We have to go downward the name tree till we reach
                '  a names collection (leaf node).
                '  2. [Horizontal] Then we iterate across the names collection.
                '  3. [Vertical, up] When leaf-nodes scan is complete, we go upward solving
                '  parent nodes, repeating step 1.
                '*/
                While (True)
                    If (Me._names Is Nothing) Then
                        If (Me._kids Is Nothing OrElse Me._kids.Count = Me._levelIndex) Then ' Kids subtree complete.
                            If (Me._levels.Count = 0) Then Return Nothing
                            ' 3. Go upward one level.
                            ' Restore current level!
                            Dim level As Object() = Me._levels.Pop()
                            Me._container = CType(level(0), PdfIndirectObject)
                            Me._kids = CType(level(1), PdfArray)
                            Me._levelIndex = CInt(level(2)) + 1 ' Next node (partially scanned level).
                        Else ' Kids subtree incomplete.
                            ' 1. Go downward one level.
                            ' Save current level!
                            Me._levels.Push(New Object() {Me._container, Me._kids, Me._levelIndex})
                        End If
                        ' Move downward!
                        Dim kidReference As PdfReference = CType(Me._kids(Me._levelIndex), PdfReference)
                        Me._container = kidReference.IndirectObject
                        Dim kid As PdfDictionary = CType(kidReference.DataObject, PdfDictionary)
                        Dim kidsObject As PdfDirectObject = kid(PdfName.Kids)
                        If (kidsObject Is Nothing) Then ' LeafThen node.
                            Dim namesObject As PdfDirectObject = kid(PdfName.Names)
                            If (TypeOf (namesObject) Is PdfReference) Then
                                Me._container = CType(namesObject, PdfReference).IndirectObject
                            End If
                            Me._names = CType(namesObject.Resolve(), PdfArray)
                            Me._kids = Nothing
                        Else ' Intermediate node.
                            If (TypeOf (kidsObject) Is PdfReference) Then
                                Me._container = CType(kidsObject, PdfReference).IndirectObject
                            End If
                            Me._kids = CType(kidsObject.Resolve(), PdfArray)
                        End If
                        Me._levelIndex = 0 ' First node (New level).
                    Else
                        If (Me._names.Count = Me._levelIndex) Then ' Names Then complete.
                            Me._names = Nothing
                        Else ' Names incomplete.
                            ' 2. Object found.
                            Dim key As TKey = CType(Me._names(Me._levelIndex), TKey)
                            Dim value As TValue = Me._tree.WrapValue(Me._names(Me._levelIndex + 1))
                            Me._levelIndex += 2
                            Return New KeyValuePair(Of TKey, TValue)(key, value)
                        End If
                    End If
                End While
                Debug.Assert(False)
                Return Nothing
            End Function
#End Region
#End Region
#End Region
        End Class

        Private Interface IFiller(Of TObject)

            Sub Add(ByVal names As PdfArray, ByVal offset As Integer)

            ReadOnly Property Collection As ICollection(Of TObject)

        End Interface

        Private Class KeysFiller
            Implements IFiller(Of TKey)

            Private keys As ICollection(Of TKey) = New List(Of TKey)

            Public Sub Add(ByVal names As PdfArray, ByVal offset As Integer) Implements IFiller(Of TKey).Add
                Me.keys.Add(CType(names(offset), TKey))
            End Sub


            Public ReadOnly Property Collection As ICollection(Of TKey) Implements IFiller(Of TKey).Collection
                Get
                    Return Me.keys
                End Get
            End Property
        End Class

        Private Class ValuesFiller
            Implements IFiller(Of TValue)

            Private tree As Tree(Of TKey, TValue)
            Private values As ICollection(Of TValue) = New List(Of TValue)

            Friend Sub New(ByVal tree As Tree(Of TKey, TValue))
                Me.tree = tree
            End Sub

            Public Sub Add(ByVal names As PdfArray, ByVal offset As Integer) Implements IFiller(Of TValue).Add
                Me.values.Add(Me.tree.WrapValue(names(offset + 1)))
            End Sub

            Public ReadOnly Property Collection As ICollection(Of TValue) Implements IFiller(Of TValue).Collection
                Get
                    Return Me.values
                End Get
            End Property
        End Class

#End Region

#Region "static"
#Region "fields"
        '/**
        '  Minimum number of items in non-root nodes.
        '  Note that the tree (high) order is assumed twice as much (<see cref="Children.Info.Info(int, int)"/>.
        '*/
        Private Shared ReadOnly TreeLowOrder As Integer = 5
#End Region
#End Region

#Region "dynamic"
#Region "fields"

        Private _pairsKey As PdfName

#End Region

#Region "constructors"

        Protected Sub New(ByVal context As Document)
            MyBase.New(context, New PdfDictionary())
            Me.Initialize()
        End Sub

        Protected Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
            Me.Initialize()
        End Sub

#End Region

#Region "interface"
#Region "public"
        '/**
        '  Gets the key associated to the specified value.
        '*/
        Public Function GetKey(ByVal value As TValue) As TKey
            '/*
            '  NOTE: Current implementation doesn't support bidirectional maps, to say that the only
            '  currently-available way to retrieve a key from a value is to iterate the whole map (really
            '  poor performance!).
            '*/
            For Each entry As KeyValuePair(Of TKey, TValue) In Me
                If (entry.Value.Equals(value)) Then Return entry.Key
            Next
            Return Nothing
        End Function

#Region "IDictionary"

        Public Overridable Sub Add(ByVal key As TKey, ByVal value As TValue) Implements IDictionary(Of TKey, TValue).Add
            Me.Add(key, value, False)
        End Sub

        Public Overridable Function ContainsKey(ByVal key As TKey) As Boolean Implements IDictionary(Of TKey, TValue).ContainsKey
            '/*
            '  NOTE: Here we assume that any named entry has a non-null value.
            '*/
            Return Me(key) IsNot Nothing
        End Function

        Public Overridable ReadOnly Property Keys As ICollection(Of TKey) Implements IDictionary(Of TKey, TValue).Keys
            Get
                Dim filler As KeysFiller = New KeysFiller()
                Me.Fill(filler, BaseDataObject)
                Return filler.Collection
            End Get
        End Property

        Public Overridable Function Remove(ByVal key As TKey) As Boolean Implements IDictionary(Of TKey, TValue).Remove
            Dim node As PdfDictionary = BaseDataObject
            Dim nodeReferenceStack As Stack(Of PdfReference) = New Stack(Of PdfReference)()
            While (True)
                Dim nodeChildren As Children = Children.Get(node, _pairsKey)
                If (nodeChildren.IsLeaf()) Then 'Leaf node.
                    Dim low As Integer = 0, high As Integer = nodeChildren.Items.Count - nodeChildren.Info.ItemCount
                    While (True)
                        If (low > high) Then Return False '  No match.

                        Dim mid As Integer = CInt((low + high) / 2)
                        mid = mid - (mid Mod 2)
                        Dim comparison As Integer = key.CompareTo(nodeChildren.Items(mid))
                        If (comparison < 0) Then ' KeyThenThen Then before.
                            high = mid - 2
                        ElseIf (comparison > 0) Then ' keyThenThen Then after.
                            low = mid + 2
                        Else ' Key matched.
                            ' We got it!
                            nodeChildren.Items.RemoveAt(mid + 1) ' Removes value.
                            nodeChildren.Items.RemoveAt(mid) ' Removes key.
                            If (mid = 0 OrElse mid = nodeChildren.Items.Count) Then ' Limits changed.
                                ' Update key limits!
                                UpdateNodeLimits(nodeChildren)

                                ' Updating key limits on ascendants...
                                Dim rootReference As PdfReference = CType(BaseObject, PdfReference)
                                Dim nodeReference As PdfReference = Nothing
                                If (nodeReferenceStack.Count > 0) Then
                                    nodeReference = nodeReferenceStack.Pop()
                                End If
                                While (nodeReference IsNot Nothing AndAlso Not nodeReference.Equals(rootReference))
                                    Dim parentChildren As PdfArray = CType(nodeReference.Parent, PdfArray)
                                    Dim nodeIndex As Integer = parentChildren.IndexOf(nodeReference)
                                    If (nodeIndex = 0 OrElse nodeIndex = parentChildren.Count - 1) Then
                                        Dim parent As PdfDictionary = CType(parentChildren.Parent, PdfDictionary)
                                        UpdateNodeLimits(parent, parentChildren, PdfName.Kids)
                                    Else
                                        Exit While
                                    End If
                                    If (nodeReferenceStack.Count > 0) Then
                                        nodeReference = nodeReferenceStack.Pop()
                                    Else
                                        nodeReference = Nothing
                                    End If
                                End While
                            End If
                            Return True
                        End If
                    End While
                Else ' Intermediate node.
                    Dim low As Integer = 0, high As Integer = nodeChildren.Items.Count - nodeChildren.Info.ItemCount
                    While (True)
                        If (low > high) Then Return False '// Outside Then the limit range.

                        Dim mid As Integer = CInt((low + high) / 2)
                        Dim kidReference As PdfReference = CType(nodeChildren.Items(mid), PdfReference)
                        Dim kid As PdfDictionary = CType(kidReference.DataObject, PdfDictionary)
                        Dim limits As PdfArray = CType(kid.Resolve(PdfName.Limits), PdfArray)
                        If (key.CompareTo(limits(0)) < 0) Then ' Before Then the lower limit.
                            high = mid - 1
                        ElseIf (key.CompareTo(limits(1)) > 0) Then ' After Then the upper limit.
                            low = mid + 1
                        Else ' Limit range matched.
                            Dim kidChildren As Children = Children.Get(kid, _pairsKey)
                            If (kidChildren.IsUndersized()) Then
                                '                                      /*
                                '  NOTE:                 Rebalancing Is required as minimum node size invariant Is violated.
                                '*/
                                Dim leftSibling As PdfDictionary = Nothing
                                Dim leftSiblingChildren As Children = Nothing
                                If (mid > 0) Then
                                    leftSibling = CType(nodeChildren.Items.Resolve(mid - 1), PdfDictionary)
                                    leftSiblingChildren = Children.Get(leftSibling, _pairsKey)
                                End If
                                Dim rightSibling As PdfDictionary = Nothing
                                Dim rightSiblingChildren As Children = Nothing
                                If (mid < nodeChildren.Items.Count - 1) Then
                                    rightSibling = CType(nodeChildren.Items.Resolve(mid + 1), PdfDictionary)
                                    rightSiblingChildren = Children.Get(rightSibling, _pairsKey)
                                End If

                                If (leftSiblingChildren IsNot Nothing AndAlso Not leftSiblingChildren.IsUndersized()) Then
                                    ' Move the last child subtree of the left sibling to be the first child subtree of the kid!
                                    Dim endIndex As Integer = leftSiblingChildren.Info.ItemCount
                                    For index As Integer = 0 To endIndex - 1
                                        Dim itemIndex As Integer = leftSiblingChildren.Items.Count - 1
                                        Dim item As PdfDirectObject = leftSiblingChildren.Items(itemIndex)
                                        leftSiblingChildren.Items.RemoveAt(itemIndex)
                                        kidChildren.Items.Insert(0, item)
                                    Next
                                    ' Update left sibling's key limits!
                                    UpdateNodeLimits(leftSiblingChildren)
                                ElseIf (rightSiblingChildren IsNot Nothing AndAlso Not rightSiblingChildren.IsUndersized()) Then
                                    ' Move the first child subtree of the right sibling to be the last child subtree of the kid!
                                    Dim endIndex As Integer = rightSiblingChildren.Info.ItemCount
                                    For index As Integer = 0 To endIndex - 1
                                        Dim itemIndex As Integer = 0
                                        Dim item As PdfDirectObject = rightSiblingChildren.Items(itemIndex)
                                        rightSiblingChildren.Items.RemoveAt(itemIndex)
                                        kidChildren.Items.Add(item)
                                    Next
                                    ' Update right sibling's key limits!
                                    UpdateNodeLimits(rightSiblingChildren)
                                Else
                                    If (leftSibling IsNot Nothing) Then
                                        ' Merging with the left sibling...
                                        For index As Integer = leftSiblingChildren.Items.Count - 1 To 0 Step -1 'index-- > 0;)
                                            Dim item As PdfDirectObject = leftSiblingChildren.Items(index)
                                            leftSiblingChildren.Items.RemoveAt(index)
                                            kidChildren.Items.Insert(0, item)
                                        Next
                                        nodeChildren.Items.RemoveAt(mid - 1)
                                        leftSibling.Reference.Delete()
                                    ElseIf (rightSibling IsNot Nothing) Then
                                        ' Merging with the right sibling...
                                        For index As Integer = rightSiblingChildren.Items.Count - 1 To 0 Step -1 '; index-- > 0;)
                                            Dim itemIndex As Integer = 0
                                            Dim item As PdfDirectObject = rightSiblingChildren.Items(itemIndex)
                                            rightSiblingChildren.Items.RemoveAt(itemIndex)
                                            kidChildren.Items.Add(item)
                                        Next
                                        nodeChildren.Items.RemoveAt(mid + 1)
                                        rightSibling.Reference.Delete()
                                    End If
                                    If (nodeChildren.Items.Count = 1) Then
                                        ' Collapsing root...
                                        nodeChildren.Items.RemoveAt(0)
                                        For index As Integer = kidChildren.Items.Count - 1 To 0 Step -1 '; index-- > 0;)
                                            Dim itemIndex As Integer = 0
                                            Dim item As PdfDirectObject = kidChildren.Items(itemIndex)
                                            kidChildren.Items.RemoveAt(itemIndex)
                                            nodeChildren.Items.Add(item)
                                        Next
                                        kid.Reference.Delete()
                                        kid = node
                                        kidReference = kid.Reference
                                        kidChildren = nodeChildren
                                    End If
                                End If
                                ' Update key limits!
                                UpdateNodeLimits(kidChildren)
                            End If
                            ' Go down one level!
                            nodeReferenceStack.Push(kidReference)
                            node = kid
                            Exit While 'break
                        End If
                    End While
                End If
            End While
            Return Nothing 'Should never happen
        End Function

        Default Public Overridable Property Item(ByVal key As TKey) As TValue Implements IDictionary(Of TKey, TValue).Item
            Get
                Dim parent As PdfDictionary = Me.BaseDataObject
                While (True)
                    Dim Children As Children = Children.Get(parent, _pairsKey)
                    If (Children.IsLeaf()) Then ' Leaf Then node.
                        Dim low As Integer = 0, high As Integer = Children.Items.Count - Children.Info.ItemCount
                        While (True)
                            If (low > high) Then Return Nothing
                            'Dim mid As Integer = (mid = ((low + high) / 2)) - (mid() % 2);
                            Dim mid As Integer = CInt((low + high) / 2)
                            mid = mid - (mid Mod 2)

                            Dim Comparison As Integer = key.CompareTo(Children.Items(mid))
                            If (Comparison < 0) Then
                                high = mid - 2
                            ElseIf (Comparison > 0) Then
                                low = mid + 2
                            Else
                                ' We got it!
                                Return WrapValue(Children.Items(mid + 1))
                            End If
                        End While
                    Else ' Intermediate node.
                        Dim low As Integer = 0, high As Integer = Children.Items.Count - Children.Info.ItemCount
                        While (True)
                            If (low > high) Then Return Nothing
                            Dim mid As Integer = CInt((low + high) / 2)
                            Dim kid As PdfDictionary = CType(Children.Items.Resolve(mid), PdfDictionary)
                            Dim limits As PdfArray = CType(kid.Resolve(PdfName.Limits), PdfArray)
                            If (key.CompareTo(limits(0)) < 0) Then
                                high = mid - 1
                            ElseIf (key.CompareTo(limits(1)) > 0) Then
                                low = mid + 1
                            Else
                                ' Go down one level!
                                parent = kid
                                Exit While '                                break;
                            End If
                        End While
                    End If
                End While
                Return Nothing 'Should never happen
            End Get
            Set(ByVal value As TValue)
                Add(key, value, True)
            End Set
        End Property

        Public Overridable Function TryGetValue(ByVal key As TKey, ByRef value As TValue) As Boolean Implements IDictionary(Of TKey, TValue).TryGetValue
            value = Me(key)
            Return (value IsNot Nothing) OrElse Me.ContainsKey(key)
        End Function

        Public Overridable ReadOnly Property Values As ICollection(Of TValue) Implements IDictionary(Of TKey, TValue).Values
            Get
                Dim filler As ValuesFiller = New ValuesFiller(Me)
                Fill(filler, BaseDataObject)
                Return filler.Collection
            End Get
        End Property

#Region "ICollection"


        Private Sub _Add(ByVal keyValuePair As KeyValuePair(Of TKey, TValue)) Implements ICollection(Of KeyValuePair(Of TKey, TValue)).Add
            Add(keyValuePair.Key, keyValuePair.Value)
        End Sub

        Public Overridable Sub Clear() Implements ICollection(Of KeyValuePair(Of TKey, TValue)).Clear
            Me.Clear(Me.BaseDataObject)
        End Sub

        Private Function _Contains(ByVal keyValuePair As KeyValuePair(Of TKey, TValue)) As Boolean Implements ICollection(Of KeyValuePair(Of TKey, TValue)).Contains
            Return keyValuePair.Value.Equals(Me(keyValuePair.Key))
        End Function

        Public Overridable Sub CopyTo(ByVal keyValuePairs As KeyValuePair(Of TKey, TValue)(), ByVal Index As Integer) Implements ICollection(Of KeyValuePair(Of TKey, TValue)).CopyTo
            Throw New NotImplementedException()
        End Sub


        Public Overridable ReadOnly Property Count As Integer Implements ICollection(Of KeyValuePair(Of TKey, TValue)).Count
            Get
                Return Me.GetCount(BaseDataObject)
            End Get
        End Property

        Public Overridable ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of KeyValuePair(Of TKey, TValue)).IsReadOnly
            Get
                Return False
            End Get
        End Property

        Public Overridable Function Remove(ByVal KeyValuePair As KeyValuePair(Of TKey, TValue)) As Boolean Implements ICollection(Of Generic.KeyValuePair(Of TKey, TValue)).Remove
            Throw New NotSupportedException()
        End Function

#Region "IEnumerable(Of KeyValuePair(Of TKey,TValue))"

        Public Overridable Function GetEnumerator() As IEnumerator(Of KeyValuePair(Of TKey, TValue)) Implements IEnumerable(Of KeyValuePair(Of TKey, TValue)).GetEnumerator
            Return New Enumerator(Me)
        End Function

#Region "IEnumerable"
        Private Function _GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Return Me.GetEnumerator()
        End Function

#End Region
#End Region
#End Region
#End Region
#End Region

#Region "protected"

        '/**
        '  <summary> Gets the name Of the key-value pairs entries.</summary>
        '*/
        Protected MustOverride ReadOnly Property PairsKey As PdfName

        '/**
        '  <summary> Wraps a base Object within its corresponding high-level representation.</summary>
        '*/
        Protected MustOverride Function WrapValue(ByVal baseObject As PdfDirectObject) As TValue

#End Region

#Region "private"

        '/**
        '  <summary> Adds an entry into the tree.</summary>
        '                                          <param name = "key" > New entry 's key.</param>
        '    <param name = "value" > New entry 's value.</param>
        '    <param name = "overwrite" > Whether the entry Is allowed To replace an existing one having the same
        '  key.</param>
        '*/
        Private Sub Add(ByVal key As TKey, ByVal value As TValue, ByVal overwrite As Boolean)
            ' Get the root node!
            Dim root As PdfDictionary = BaseDataObject

            ' Ensuring the root node isn't full...
            Dim rootChildren As Children = Children.Get(root, _pairsKey)
            If (rootChildren.IsFull()) Then
                ' Transfer the root contents into the New leaf!
                Dim leaf As PdfDictionary = CType(New PdfDictionary().Swap(root), PdfDictionary)
                Dim rootChildrenObject As PdfArray = New PdfArray(New PdfDirectObject() {File.Register(leaf)})
                root(PdfName.Kids) = rootChildrenObject
                ' Split the leaf!
                SplitFullNode(rootChildrenObject, 0, rootChildren.TypeName) '// Old root's position within new root's kids.
            End If

            ' Set the entry under the root node!
            Add(key, value, overwrite, root)
        End Sub

        '/**
        '  <summary> Adds an entry under the given tree node.</summary>
        '                                          <param name = "key" > New entry 's key.</param>
        '    <param name = "value" > New entry 's value.</param>
        '    <param name = "overwrite" > Whether the entry Is allowed To replace an existing one having the same
        '  key.</param>
        '                                          <param name = "nodeReference" > Current node reference.</param>
        '*/
        Private Sub Add(ByVal key As TKey, ByVal value As TValue, ByVal overwrite As Boolean, ByVal node As PdfDictionary)
            Dim children As Children = Children.Get(node, _pairsKey)
            If (children.IsLeaf()) Then ' LeafThen node.
                Dim childrenSize As Integer = children.Items.Count
                Dim low As Integer = 0, high As Integer = childrenSize - children.Info.ItemCount
                While (True)
                    If (low > high) Then
                        ' Insert the entry!
                        children.Items.Insert(low, key)
                        low += 1 : children.Items.Insert(low, value.BaseObject)
                        Exit While 'break
                    End If

                    'Dim mid As Integer = (mid = ((low + high) / 2)) - (mid % 2);
                    Dim mid As Integer = CInt((low + high) / 2)
                    mid = mid - (mid Mod 2)
                    If (mid >= childrenSize) Then
                        ' Append the entry!
                        children.Items.Add(key)
                        children.Items.Add(value.BaseObject)
                        Exit While 'break;
                    End If

                    Dim comparison As Integer = key.CompareTo(children.Items(mid))
                    If (comparison < 0) Then ' Before.
                        high = mid - 2
                    ElseIf (comparison > 0) Then ' After.
                        low = mid + 2
                    Else ' Matching entry.
                        If (Not overwrite) Then Throw New ArgumentException("Key '" & key.ToString & "' already exists.", "key")
                        ' Overwrite the entry!
                        children.Items(mid) = key
                        mid += 1 : children.Items(mid) = value.BaseObject
                        Exit While
                    End If
                End While

                ' Update the key limits!
                UpdateNodeLimits(children)
            Else ' Intermediate node.
                Dim low As Integer = 0, high As Integer = children.Items.Count - children.Info.ItemCount
                While (True)
                    Dim matched As Boolean = False
                    Dim mid As Integer = CInt((low + high) / 2)
                    Dim kidReference As PdfReference = CType(children.Items(mid), PdfReference)
                    Dim kid As PdfDictionary = CType(kidReference.DataObject, PdfDictionary)
                    Dim limits As PdfArray = CType(kid.Resolve(PdfName.Limits), PdfArray)
                    If (key.CompareTo(limits(0)) < 0) Then ' Before Then the lower limit.
                        high = mid - 1
                    ElseIf (key.CompareTo(limits(1)) > 0) Then ' After Then the upper limit.
                        low = mid + 1
                    Else ' Limit range matched.
                        matched = True
                    End If

                    If (matched OrElse low > high) Then ' // Limit range matched. // No limit range match.
                        Dim kidChildren As Children = Children.Get(kid, _pairsKey)
                        If (kidChildren.IsFull()) Then
                            ' Split the node!
                            SplitFullNode(children.Items, mid, kidChildren.TypeName)
                            ' Is the key before the split node?
                            If (key.CompareTo(CType(kid.Resolve(PdfName.Limits), PdfArray)(0)) < 0) Then
                                kidReference = CType(children.Items(mid), PdfReference)
                                kid = CType(kidReference.DataObject, PdfDictionary)
                            End If
                        End If

                        Add(key, value, overwrite, kid)
                        ' Update the key limits!
                        UpdateNodeLimits(children)
                        Exit While 'break;
                    End If
                End While
            End If
        End Sub

        '/**
        '  <summary> Removes all the given node's children.</summary>
        '  <remarks>
        '    <para>As Me method doesn't apply balancing, it's suitable for clearing root nodes only.
        '    </para>
        '    <para> Removal affects only tree nodes: referenced objects are preserved To avoid inadvertently
        '    breaking possible references To them from somewhere Else.</para>
        '  </remarks>
        '  <param name = "node" > Current node.</param>
        '*/
        Private Sub Clear(ByVal node As PdfDictionary)
            Dim children As Children = Children.Get(node, _pairsKey)
            If (Not children.IsLeaf()) Then
                For Each child As PdfDirectObject In children.Items
                    Clear(CType(child.Resolve(), PdfDictionary))
                    File.Unregister(CType(child, PdfReference))
                Next
                node(_pairsKey) = node(children.TypeName)
                node.Remove(children.TypeName) ' Recycles the array As the intermediate node transforms To leaf.
            End If
            children.Items.Clear()
            node.Remove(PdfName.Limits)
        End Sub

        Private Sub Fill(Of TObject)(ByVal filler As IFiller(Of TObject), ByVal node As PdfDictionary)
            Dim kidsObject As PdfArray = CType(node.Resolve(PdfName.Kids), PdfArray)
            If (kidsObject Is Nothing) Then ' LeafThen node.
                Dim namesObject As PdfArray = CType(node.Resolve(PdfName.Names), PdfArray)
                Dim length As Integer = namesObject.Count
                For index As Integer = 0 To length - 1 Step 2
                    filler.Add(namesObject, index)
                Next
            Else ' Intermediate node.
                For Each kidObject As PdfDirectObject In kidsObject
                    Fill(filler, CType(kidObject.Resolve(), PdfDictionary))
                Next
            End If
        End Sub

        '/**
        '  <summary> Gets the given node's entries count.</summary>
        '  <param name = "node" > Current node.</param>
        '*/
        Private Function GetCount(ByVal node As PdfDictionary) As Integer
            Dim children As PdfArray = CType(node.Resolve(PdfName.Names), PdfArray)
            If (children IsNot Nothing) Then ' Leaf Then node.
                Return CInt(children.Count / 2)
            Else ' Intermediate node.
                children = CType(node.Resolve(PdfName.Kids), PdfArray)
                Dim Count As Integer = 0
                For Each child As PdfDirectObject In children
                    Count += GetCount(CType(child.Resolve(), PdfDictionary))
                Next
                Return Count
            End If
        End Function

        Private Sub Initialize()
            _pairsKey = Me.PairsKey
            Dim BaseDataObject As PdfDictionary = Me.BaseDataObject
            If (BaseDataObject.Count = 0) Then
                BaseDataObject.Updateable = False
                BaseDataObject(_pairsKey) = New PdfArray() ' NOTE: Initial root Is by definition a leaf node.
                BaseDataObject.Updateable = True
            End If
        End Sub

        '/**
        '  <summary> Splits a full node.</summary>
        '  <remarks> A New node Is inserted at the full node's position, receiving the lower half of its
        '  children.</remarks>
        '  <param name = "nodes" > Parent nodes.</param>
        '  <param name = "fullNodeIndex" > Full node's position among the parent nodes.</param>
        '  <param name = "childrenTypeName" > Full node's children type.</param>
        '*/
        Private Sub SplitFullNode(ByVal Nodes As PdfArray, ByVal fullNodeIndex As Integer, ByVal childrenTypeName As PdfName)
            ' Get the full node!
            Dim fullNode As PdfDictionary = CType(Nodes.Resolve(fullNodeIndex), PdfDictionary)
            Dim fullNodeChildren As PdfArray = CType(fullNode.Resolve(childrenTypeName), PdfArray)

            ' Create a New (sibling) node!
            Dim newNode As PdfDictionary = New PdfDictionary()
            Dim newNodeChildren As PdfArray = New PdfArray()
            newNode(childrenTypeName) = newNodeChildren
            ' Insert the New node just before the full!
            Nodes.Insert(fullNodeIndex, File.Register(newNode)) ' NOTE: Nodes MUST be indirect objects.

            ' Transferring exceeding children to the New node...
            Dim length As Integer = Children.InfoImpl.Get(childrenTypeName).MinCount
            For index As Integer = 0 To length - 1
                Dim removedChild As PdfDirectObject = fullNodeChildren(0)
                fullNodeChildren.RemoveAt(0)
                newNodeChildren.Add(removedChild)
            Next

            ' Update the key limits!
            UpdateNodeLimits(newNode, newNodeChildren, childrenTypeName)
            UpdateNodeLimits(fullNode, fullNodeChildren, childrenTypeName)
        End Sub

        '/**
        '  <summary> Sets the key limits Of the given node.</summary>
        '  <param name = "children" > node children.</param>
        '*/
        Private Sub UpdateNodeLimits(ByVal children As Children)
            UpdateNodeLimits(children.Parent, children.Items, children.TypeName)
        End Sub

        '/**
        '  <summary> Sets the key limits Of the given node.</summary>
        '  <param name = "node" > node To update.</param>
        '  <param name = "children" > node children.</param>
        '  <param name = "childrenTypeName" > node 's children type.</param>
        '*/
        Private Sub UpdateNodeLimits(ByVal node As PdfDictionary, ByVal children As PdfArray, ByVal childrenTypeName As PdfName)
            Dim lowLimit As PdfDirectObject, highLimit As PdfDirectObject
            If (childrenTypeName.Equals(PdfName.Kids)) Then
                ' Non-leaf root node?
                If (node Is BaseDataObject) Then Return ' NOTE: Non-leaf root nodes DO Not specify limits.

                lowLimit = CType(CType(children.Resolve(0), PdfDictionary).Resolve(PdfName.Limits), PdfArray)(0)
                highLimit = CType(CType(children.Resolve(children.Count - 1), PdfDictionary).Resolve(PdfName.Limits), PdfArray)(1)
            ElseIf (childrenTypeName.Equals(_pairsKey)) Then
                lowLimit = children(0)
                highLimit = children(children.Count - 2)
            Else ' NOTE: Should NEVER happen.
                Throw New NotSupportedException(childrenTypeName.ToString & " is NOT a supported child type.")
            End If
            Dim limits As PdfArray = CType(node(PdfName.Limits), PdfArray)
            If (limits IsNot Nothing) Then
                limits(0) = lowLimit
                limits(1) = highLimit
            Else
                node(PdfName.Limits) = New PdfArray(New PdfDirectObject() {lowLimit, highLimit})
            End If
        End Sub

#End Region
#End Region
#End Region

    End Class

End Namespace
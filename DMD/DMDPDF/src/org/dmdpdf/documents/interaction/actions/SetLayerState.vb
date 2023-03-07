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

'  You should have received a copy of the GNU Lesser General Public License along with Me
'  Program (see README files); if not, go to the GNU website (http://www.gnu.org/licenses/).

'  Redistribution and use, with or without modification, are permitted provided that such
'  redistributions retain the above copyright notice, license and disclaimer, along with
'  Me list of conditions.
'*/

Imports DMD.org.dmdpdf.bytes
Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.documents.contents.layers
Imports DMD.org.dmdpdf.objects
Imports DMD.org.dmdpdf.util

Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports System.Runtime.CompilerServices

Namespace DMD.org.dmdpdf.documents.interaction.actions

    '/**
    '  <summary>'Set the state of one or more optional content groups' action [PDF:1.6:8.5.3].</summary>
    '*/
    <PDF(VersionEnum.PDF15)>
    Public NotInheritable Class SetLayerState
        Inherits Action

#Region "types"

        Public Enum StateModeEnum
            [On]
            Off
            Toggle
        End Enum

        Public Class LayerState

            Friend Class LayersImpl
                Inherits Collection(Of Layer)

                Friend _parentState As LayerState

                Protected Overrides Sub ClearItems()
                    ' Low-level definition.
                    Dim baseStates As LayerStates = Me.BaseStates
                    If (baseStates IsNot Nothing) Then
                        Dim itemIndex As Integer = baseStates.GetBaseIndex(_parentState) + 1 ' Name object offset.
                        For count As Integer = Me.Count - 1 To 0 Step -1 '; count--)
                            baseStates.BaseDataObject.RemoveAt(itemIndex)
                        Next
                    End If
                    ' High-level definition.
                    MyBase.ClearItems()
                End Sub

                Protected Overrides Sub InsertItem(ByVal index As Integer, ByVal item As Layer)
                    ' High-level definition.
                    MyBase.InsertItem(index, item)
                    ' Low-level definition.
                    Dim baseStates As LayerStates = Me.BaseStates
                    If (baseStates IsNot Nothing) Then
                        Dim baseIndex As Integer = baseStates.GetBaseIndex(_parentState)
                        Dim itemIndex As Integer = baseIndex + 1 + index ' // Name object offset. // Layer object offset.
                        baseStates.BaseDataObject(itemIndex) = item.BaseObject
                    End If
                End Sub

                Protected Overrides Sub RemoveItem(ByVal index As Integer)
                    ' High-level definition.
                    MyBase.RemoveItem(index)
                    ' Low-level definition.
                    Dim baseStates As LayerStates = Me.BaseStates
                    If (baseStates IsNot Nothing) Then
                        Dim baseIndex As Integer = baseStates.GetBaseIndex(_parentState)
                        Dim itemIndex As Integer = baseIndex + 1 + index '// Name object offset. // Layer object offset.
                        baseStates.BaseDataObject.RemoveAt(itemIndex)
                    End If
                End Sub

                Protected Overrides Sub SetItem(ByVal index As Integer, ByVal item As Layer)
                    RemoveItem(index)
                    InsertItem(index, item)
                End Sub

                Private ReadOnly Property BaseStates As LayerStates
                    Get
                        If (_parentState IsNot Nothing) Then
                            Return _parentState._baseStates
                        Else
                            Return Nothing
                        End If
                    End Get
                End Property

            End Class

            Private ReadOnly _layers As LayersImpl
            Private _mode As StateModeEnum

            Private _baseStates As LayerStates

            Public Sub New(ByVal mode As StateModeEnum)
                Me.New(mode, New LayersImpl(), Nothing)
            End Sub

            Friend Sub New(ByVal mode As StateModeEnum, ByVal layers As LayersImpl, ByVal baseStates As LayerStates)
                Me._mode = mode
                Me._layers = layers
                Me._layers._parentState = Me
                Attach(baseStates)
            End Sub

            Public Overrides Function Equals(ByVal obj As Object) As Boolean
                If (Not (TypeOf (obj) Is LayerState)) Then Return False
                Dim state As LayerState = CType(obj, LayerState)
                If (Not state.Mode.Equals(Mode) OrElse
                    state.Layers.Count <> Layers.Count) Then
                    Return False
                End If

                Dim layerIterator As IEnumerator(Of Layer) = Layers.GetEnumerator()
                Dim stateLayerIterator As IEnumerator(Of Layer) = state.Layers.GetEnumerator()
                While (layerIterator.MoveNext())
                    stateLayerIterator.MoveNext()
                    If (Not layerIterator.Current.Equals(stateLayerIterator.Current)) Then
                        Return False
                    End If
                End While
                Return True
            End Function

            Public ReadOnly Property Layers As IList(Of Layer)
                Get
                    Return Me._layers
                End Get
            End Property

            Public Property Mode As StateModeEnum
                Get
                    Return Me._mode
                End Get
                Set(ByVal value As StateModeEnum)
                    Me._mode = value
                    If (Me._baseStates IsNot Nothing) Then
                        Dim baseIndex As Integer = _baseStates.GetBaseIndex(Me)
                        _baseStates.BaseDataObject(baseIndex) = value.GetName()
                    End If
                End Set
            End Property

            Public Overrides Function GetHashCode() As Integer
                Return Me._mode.GetHashCode() Xor Me._layers.Count
            End Function

            Friend Sub Attach(ByVal baseStates As LayerStates)
                Me._baseStates = baseStates
            End Sub

            Friend Sub Detach()
                Me._baseStates = Nothing
            End Sub
        End Class


        Public Class LayerStates
            Inherits PdfObjectWrapper(Of PdfArray)
            Implements IList(Of LayerState)

            Private _items As IList(Of LayerState)

            Public Sub New()
                MyBase.New(New PdfArray())
            End Sub

            Friend Sub New(ByVal baseObject As PdfDirectObject)
                MyBase.New(baseObject)
                Me.Initialize()
            End Sub

#Region "IList(Of LayerState)"

            Public Function IndexOf(ByVal item As LayerState) As Integer Implements IList(Of LayerState).IndexOf
                Return Me._items.IndexOf(item)
            End Function

            Public Sub Insert(ByVal Index As Integer, ByVal item As LayerState) Implements IList(Of LayerState).Insert
                Dim baseIndex As Integer = GetBaseIndex(Index)
                If (baseIndex = -1) Then
                    Add(item)
                Else
                    Dim BaseDataObject As PdfArray = Me.BaseDataObject
                    ' Low-level definition.
                    BaseDataObject.Insert(baseIndex, item.Mode.GetName()) : baseIndex += 1
                    For Each layer As Layer In item.Layers
                        BaseDataObject.Insert(baseIndex, layer.BaseObject) : baseIndex += 1
                    Next
                    ' High-level definition.
                    _items.Insert(Index, item)
                    item.Attach(Me)
                End If
            End Sub

            '  For (int baseCount = baseDataObject.Count; baseIndex < baseCount;)
            '{
            '  If (baseDataObject[baseIndex] Is PdfName)
            '  {
            '    If (done)
            '      break;

            '    done = true;
            '  }
            '  baseDataObject.RemoveAt(baseIndex);
            '}

            Public Sub RemoveAt(ByVal Index As Integer) Implements IList(Of LayerState).RemoveAt
                Dim LayerState As LayerState
                ' Low-level definition.
                '{
                Dim baseIndex As Integer = GetBaseIndex(Index)
                If (baseIndex = -1) Then Throw New IndexOutOfRangeException()
                Dim BaseDataObject As PdfArray = Me.BaseDataObject
                Dim done As Boolean = False
                Dim baseCount As Integer = BaseDataObject.Count
                While (baseIndex < baseCount)
                    If (TypeOf (BaseDataObject(baseIndex)) Is PdfName) Then
                        If (done) Then Exit While
                        done = True
                    End If
                    BaseDataObject.RemoveAt(baseIndex)
                End While
                '}
                ' High-level definition.
                '{
                LayerState = _items(Index)
                _items.RemoveAt(Index)
                LayerState.Detach()
                '}
            End Sub

            Default Public Property Item(ByVal Index As Integer) As LayerState Implements IList(Of LayerState).Item
                Get
                    Return _items(Index)
                End Get
                Set(ByVal value As LayerState)
                    RemoveAt(Index)
                    Insert(Index, value)
                End Set
            End Property

#Region "ICollection(Of LayerState)"

            Public Sub Add(ByVal item As LayerState) Implements ICollection(Of LayerState).Add
                Dim BaseDataObject As PdfArray = Me.BaseDataObject
                ' Low-level definition.
                BaseDataObject.Add(item.Mode.GetName())
                For Each layer As Layer In item.Layers
                    BaseDataObject.Add(layer.BaseObject)
                Next
                ' High-level definition.
                _items.Add(item)
                item.Attach(Me)
            End Sub

            Public Sub Clear() Implements ICollection(Of LayerState).Clear
                ' Low-level definition.
                Me.BaseDataObject.Clear()
                ' High-level definition.
                For Each item As LayerState In Me._items
                    item.Detach()
                Next
                Me._items.Clear()
            End Sub

            Public Function Contains(ByVal item As LayerState) As Boolean Implements ICollection(Of LayerState).Contains
                Return Me._items.Contains(item)
            End Function

            Public Sub CopyTo(ByVal items As LayerState(), ByVal Index As Integer) Implements ICollection(Of SetLayerState.LayerState).CopyTo
                Throw New NotImplementedException()
            End Sub

            Public ReadOnly Property Count As Integer Implements ICollection(Of LayerState).Count
                Get
                    Return Me._items.Count
                End Get
            End Property

            Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of LayerState).IsReadOnly
                Get
                    Return False
                End Get
            End Property

            Public Function Remove(ByVal item As LayerState) As Boolean Implements ICollection(Of LayerState).Remove
                Dim index As Integer = Me.IndexOf(item)
                If (index = -1) Then Return False
                RemoveAt(index)
                Return True
            End Function

#Region "IEnumerable(Of LayerState)"

            Public Function GetEnumerator() As IEnumerator(Of LayerState) Implements IEnumerable(Of LayerState).GetEnumerator
                Return Me._items.GetEnumerator()
            End Function

#Region "IEnumerable"

            Private Function _GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
                Return Me.GetEnumerator()
            End Function

#End Region
#End Region
#End Region
#End Region

            '/**
            '  <summary> Gets the position Of the initial base item corresponding To the specified layer
            '  state Index.</summary>
            '  <param name = "index" > Layer state index.</param>
            '  <returns>-1, In Case <code>index</code> Is outside the available range.</returns>
            '*/
            Friend Function GetBaseIndex(ByVal Index As Integer) As Integer
                Dim baseIndex As Integer = -1
                '{
                Dim BaseDataObject As PdfArray = Me.BaseDataObject
                Dim layerStateIndex As Integer = -1
                'For (Int() baseItemIndex = 0, baseItemCount = BaseDataObject.Count; baseItemIndex <baseItemCount; baseItemIndex++            )
                Dim baseItemCount As Integer = BaseDataObject.Count
                For baseItemIndex As Integer = 0 To baseItemCount - 1 Step 1
                    If (TypeOf (BaseDataObject(baseItemIndex)) Is PdfName) Then
                        layerStateIndex += 1
                        If (layerStateIndex = Index) Then
                            baseIndex = baseItemIndex
                            Exit For
                        End If
                    End If
                Next
                '}
                Return baseIndex
            End Function

            '/**
            '  <summary> Gets the position Of the initial base item corresponding To the specified layer
            '  state.</summary>
            '  <param name = "item" > Layer state.</param>
            '  <returns>-1, In Case <code>item</code> has no match.</returns>
            '*/
            Friend Function GetBaseIndex(ByVal item As LayerState) As Integer
                Dim baseIndex As Integer = -1
                '{
                Dim baseDataObject As PdfArray = Me.BaseDataObject
                '    For (
                'int baseItemIndex = 0,
                '  baseItemCount = baseDataObject.Count;
                'baseItemIndex < baseItemCount;
                'baseItemIndex++
                ')
                Dim baseItemCount As Integer = baseDataObject.Count
                For baseItemIndex As Integer = 0 To baseItemCount - 1 Step 1
                    Dim baseItem As PdfDirectObject = baseDataObject(baseItemIndex)
                    If (TypeOf (baseItem) Is PdfName AndAlso
                        baseItem.Equals(item.Mode.GetName())) Then
                        For Each layer As Layer In item.Layers
                            baseItemIndex += 1
                            If (baseItemIndex >= baseItemCount) Then Exit For
                            baseItem = baseDataObject(baseItemIndex)
                            If (TypeOf (baseItem) Is PdfName OrElse
                                    Not baseItem.Equals(layer.BaseObject)) Then
                                Exit For
                            End If
                        Next
                    End If
                Next
                Return baseIndex
            End Function

            Private Sub Initialize()
                _items = New List(Of LayerState)()
                Dim baseDataObject As PdfArray = Me.BaseDataObject
                Dim mode As StateModeEnum? = Nothing
                Dim layers As LayerState.LayersImpl = Nothing
                '      For (
                'int baseIndex = 0,
                '  baseCount = baseDataObject.Count;
                'baseIndex < baseCount;
                'baseIndex++
                ')
                Dim baseCount As Integer = baseDataObject.Count
                For baseIndex As Integer = 0 To baseCount - 1 Step 1
                    Dim baseObject As PdfDirectObject = baseDataObject(baseIndex)
                    If (TypeOf (baseObject) Is PdfName) Then
                        If (mode.HasValue) Then
                            _items.Add(New LayerState(mode.Value, layers, Me))
                        End If
                        mode = StateModeEnumExtension.Get(CType(baseObject, PdfName))
                        layers = New LayerState.LayersImpl()
                    Else
                        layers.Add(Layer.Wrap(baseObject))
                    End If
                Next
                If (mode.HasValue) Then
                    _items.Add(New LayerState(mode.Value, layers, Me))
                End If
            End Sub
        End Class

#End Region

#Region "dynamic"
#Region "constructors"

        '/**
        '  <summary> Creates a New action within the given document context.</summary>
        '*/
        Public Sub New(ByVal context As Document)
            MyBase.New(context, PdfName.SetOCGState)
            Me.States = New LayerStates()
        End Sub

        Friend Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public Property States As LayerStates
            Get
                Return New LayerStates(BaseDataObject(PdfName.State))
            End Get
            Set(ByVal value As LayerStates)
                BaseDataObject(PdfName.State) = value.BaseObject
            End Set
        End Property

#End Region
#End Region
#End Region

    End Class

    Module StateModeEnumExtension

        Private ReadOnly _codes As BiDictionary(Of SetLayerState.StateModeEnum, PdfName)

        Sub New()
            _codes = New BiDictionary(Of SetLayerState.StateModeEnum, PdfName)
            _codes(SetLayerState.StateModeEnum.On) = PdfName.ON
            _codes(SetLayerState.StateModeEnum.Off) = PdfName.OFF
            _codes(SetLayerState.StateModeEnum.Toggle) = PdfName.Toggle
        End Sub

        Public Function [Get](ByVal name As PdfName) As SetLayerState.StateModeEnum
            If (name Is Nothing) Then Throw New ArgumentNullException("name")
            Dim stateMode As SetLayerState.StateModeEnum? = _codes.GetKey(name)
            If (Not stateMode.HasValue) Then Throw New NotSupportedException("State mode unknown: " & name.ToString)
            Return stateMode.Value
        End Function

        <Extension>
        Public Function GetName(ByVal stateMode As SetLayerState.StateModeEnum) As PdfName
            Return _codes(stateMode)
        End Function

    End Module

End Namespace
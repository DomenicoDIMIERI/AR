'/*
'  Copyright 2011-2013 Stefano Chizzolini. http://www.dmdpdf.org

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

Imports DMD.org.dmdpdf.documents

Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Reflection

Namespace DMD.org.dmdpdf.objects

    '/**
    '  <summary>Collection of sequentially-arranged object wrappers.</summary>
    '*/
    Public Class Array(Of TItem As IPdfObjectWrapper)
        Inherits PdfObjectWrapper(Of PdfArray)
        Implements IList(Of TItem)

#Region "types"
        '/**
        '  <summary> Item instancer.</summary>
        '*/
        Public Interface IWrapper(Of T As TItem)

            Function Wrap(ByVal baseObject As PdfDirectObject) As T

        End Interface

        Private Class DefaultWrapper(Of T As TItem)
            Implements IWrapper(Of T)

            Private itemConstructor As MethodInfo

            Friend Sub New()
                Me.itemConstructor = GetType(TItem).GetMethod("Wrap", New Type() {GetType(PdfDirectObject)})
            End Sub

            Public Function Wrap(ByVal baseObject As PdfDirectObject) As T Implements IWrapper(Of T).Wrap
                Return CType(Me.itemConstructor.Invoke(Nothing, New Object() {baseObject}), T)
            End Function
        End Class

#End Region

#Region "static"
#Region "interface"
#Region "public"

        '/**
        '  <summary> Wraps an existing base array Using the Default wrapper For wrapping its items.
        '  </summary>
        '  <param name = "itemClass" > item Class.</param>
        '  <param name = "baseObject" > Base array. MUST be a {@link PdfReference reference} every time
        '  available.</param>
        '*/
        Public Shared Function Wrap(Of T As TItem)(ByVal baseObject As PdfDirectObject) As Array(Of T)
            If (baseObject IsNot Nothing) Then
                Return New Array(Of T)(baseObject)
            Else
                Return Nothing
            End If
        End Function

        '/**
        '  <summary> Wraps an existing base array Using the specified wrapper For wrapping its items.
        '  </summary>
        '  <param name = "itemWrapper" > item wrapper.</param>
        '  <param name = "baseObject" > Base array. MUST be a {@link PdfReference reference} every time
        '  available.</param>
        '*/
        Public Shared Function Wrap(Of T As TItem)(ByVal itemWrapper As Array(Of T).IWrapper(Of T), ByVal baseObject As PdfDirectObject) As Array(Of T) '  where T : 
            If (baseObject IsNot Nothing) Then
                Return New Array(Of T)(itemWrapper, baseObject)
            Else
                Return Nothing
            End If
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "fields"

        Private _itemWrapper As IWrapper(Of TItem)

#End Region

#Region "constructors"

        '/**
        '  <summary> Wraps a New base array Using the Default wrapper For wrapping its items.</summary>
        '  <param name = "context" > Document context.</param>
        '*/
        Public Sub New(ByVal context As Document)
            Me.New(context, New PdfArray())
        End Sub

        '/**
        '  <summary> Wraps a New base array Using the specified wrapper For wrapping its items.</summary>
        '  <param name = "context" > Document context.</param>
        '  <param name = "itemWrapper" > item wrapper.</param>
        '*/
        Public Sub New(ByVal context As Document, ByVal itemWrapper As IWrapper(Of TItem))
            Me.New(context, itemWrapper, New PdfArray())
        End Sub

        '/**
        '  <summary> Wraps the specified base array Using the Default wrapper For wrapping its items.</summary>
        '  <param name = "context" > Document context.</param>
        '  <param name = "baseDataObject" > Base array.</param>
        '*/
        Public Sub New(ByVal context As Document, ByVal baseDataObject As PdfArray)
            Me.New(context, New DefaultWrapper(Of TItem), baseDataObject)
        End Sub

        '/**
        '  <summary> Wraps the specified base array Using the specified wrapper For wrapping its items.</summary>
        '  <param name = "context" > Document context.</param>
        '  <param name = "itemWrapper" > item wrapper.</param>
        '  <param name = "baseDataObject" > Base array.</param>
        '*/
        Public Sub New(ByVal context As Document, ByVal itemWrapper As IWrapper(Of TItem), ByVal baseDataObject As PdfArray)
            MyBase.New(context, baseDataObject)
            Me._itemWrapper = itemWrapper
        End Sub

        '/**
        '  <summary> Wraps an existing base array Using the Default wrapper For wrapping its items.</summary>
        '  <param name = "baseObject" > Base array. MUST be a <see cref="PdfReference">reference</see>
        '  everytime available.</param>
        '*/
        Protected Sub New(ByVal baseObject As PdfDirectObject)
            Me.New(New DefaultWrapper(Of TItem), baseObject)
        End Sub

        '/**
        '  <summary> Wraps an existing base array Using the specified wrapper For wrapping its items.</summary>
        '  <param name = "itemWrapper" > item wrapper.</param>
        '  <param name = "baseObject" > Base array. MUST be a <see cref="PdfReference">reference</see>
        '  everytime available.</param>
        '*/
        Protected Sub New(ByVal itemWrapper As IWrapper(Of TItem), ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
            Me._itemWrapper = itemWrapper
        End Sub

#End Region

#Region "Interface"
#Region "Public"
#Region "IList(Of TItem)"

        Public Overridable Function IndexOf(ByVal item As TItem) As Integer Implements IList(Of TItem).IndexOf
            Return BaseDataObject.IndexOf(item.BaseObject)
        End Function

        Public Overridable Sub Insert(ByVal Index As Integer, ByVal item As TItem) Implements IList(Of TItem).Insert
            BaseDataObject.Insert(Index, item.BaseObject)
        End Sub

        Public Overridable Sub RemoveAt(ByVal Index As Integer) Implements IList(Of TItem).RemoveAt
            BaseDataObject.RemoveAt(Index)
        End Sub

        Default Public Overridable Property Item(ByVal index As Integer) As TItem Implements IList(Of TItem).Item
            Get
                Return _itemWrapper.Wrap(BaseDataObject(index))
            End Get
            Set(ByVal value As TItem)
                BaseDataObject(index) = value.BaseObject
            End Set
        End Property

#Region "ICollection(Of TItem)"

        Public Overridable Sub Add(ByVal item As TItem) Implements ICollection(Of TItem).Add
            BaseDataObject.Add(item.BaseObject)
        End Sub

        Public Overridable Sub Clear() Implements ICollection(Of TItem).Clear
            Dim index As Integer = Me.Count - 1
            While (index >= 0)
                Me.RemoveAt(index)
                index -= 1
            End While
        End Sub

        Public Overridable Function Contains(ByVal item As TItem) As Boolean Implements ICollection(Of TItem).Contains
            Return BaseDataObject.Contains(item.BaseObject)
        End Function

        Public Overridable Sub CopyTo(ByVal items As TItem(), ByVal index As Integer) Implements ICollection(Of TItem).CopyTo
            Throw New NotImplementedException()
        End Sub

        Public Overridable ReadOnly Property Count As Integer Implements ICollection(Of TItem).Count
            Get
                Return BaseDataObject.Count
            End Get
        End Property

        Public Overridable ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of TItem).IsReadOnly
            Get
                Return False
            End Get
        End Property

        Public Overridable Function Remove(ByVal item As TItem) As Boolean Implements ICollection(Of TItem).Remove
            Return BaseDataObject.Remove(item.BaseObject)
        End Function

#Region "IEnumerable(Of TItem)"

        '    Public virtual IEnumerator(Of TItem) GetEnumerator(
        '  )
        '{
        '  For (int index = 0, length = Count; index < length; index++)
        '  {yield return this(index);}
        '}



        Public Overridable Function GetEnumerator() As IEnumerator(Of TItem) Implements IEnumerable(Of TItem).GetEnumerator
            Return New mEnumerator(Of TItem)(Me)
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

'/*
'  Copyright 2010-2011 Stefano Chizzolini. http://www.dmdpdf.org

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

Imports DMD.org.dmdpdf.objects

Imports System
Imports System.Collections
Imports System.Collections.Generic

Namespace DMD.org.dmdpdf.documents.functions

    '/**
    '  <summary>List of 1-input functions combined in a <see cref="Parent">stitching function</see> [PDF:1.6:3.9.3].</summary>
    '*/
    <PDF(VersionEnum.PDF13)>
    Public NotInheritable Class Functions
        Inherits PdfObjectWrapper(Of PdfArray)
        Implements IList(Of [Function])

#Region "dynamic"
#Region "fields"

        '/**
        '  <summary>Parent function.</summary>
        '*/
        Private _parent As Type3Function

#End Region

#Region "constructors"

        Friend Sub New(ByVal baseObject As PdfDirectObject, ByVal parent As Type3Function)
            MyBase.New(baseObject)
            Me._parent = parent
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public Overrides Function Clone(ByVal context As Document) As Object
            Return New NotImplementedException()
        End Function

        '/**
        '  <summary>Gets the parent stitching function.</summary>
        '*/
        Public ReadOnly Property Parent As Type3Function
            Get
                Return Me._parent
            End Get
        End Property

#Region "IList"

        Public Function IndexOf(ByVal value As [Function]) As Integer Implements IList(Of documents.functions.Function).IndexOf
            Return BaseDataObject.IndexOf(value.BaseObject)
        End Function

        Public Sub Insert(ByVal Index As Integer, ByVal value As [Function]) Implements IList(Of [Function]).Insert
            Validate(value)
            BaseDataObject.Insert(Index, value.BaseObject)
        End Sub

        Public Sub RemoveAt(ByVal Index As Integer) Implements IList(Of [Function]).RemoveAt
            BaseDataObject.RemoveAt(Index)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As [Function] Implements IList(Of [Function]).Item
            Get
                Return [Function].Wrap(BaseDataObject(Index))
            End Get
            Set(ByVal value As [Function])
                Validate(value)
                BaseDataObject(Index) = value.BaseObject
            End Set
        End Property

#Region "ICollection"

        Public Sub Add(ByVal value As [Function]) Implements ICollection(Of [Function]).Add
            Validate(value)
            BaseDataObject.Add(value.BaseObject)
        End Sub

        Public Sub Clear() Implements ICollection(Of [Function]).Clear
            BaseDataObject.Clear()
        End Sub

        Public Function Contains(ByVal value As [Function]) As Boolean Implements ICollection(Of [Function]).Contains
            Return BaseDataObject.Contains(value.BaseObject)
        End Function

        Public Sub CopyTo(ByVal values As [Function](), ByVal Index As Integer) Implements ICollection(Of documents.functions.Function).CopyTo
            Throw New NotImplementedException()
        End Sub

        Public ReadOnly Property Count As Integer Implements ICollection(Of [Function]).Count
            Get
                Return BaseDataObject.Count
            End Get
        End Property

        Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of [Function]).IsReadOnly
            Get
                Return False
            End Get
        End Property

        Public Function Remove(ByVal value As [Function]) As Boolean Implements ICollection(Of [Function]).Remove
            Return BaseDataObject.Remove(value.BaseObject)
        End Function

#Region "IEnumerable<Function>"

        '    IEnumerator<Function> IEnumerable<Function>.GetEnumerator(
        '  )
        '{
        '  For (
        '    int index = 0,
        '      length = Count;
        '    Index <length;
        '    index++
        '    )
        '  {yield return this(index);}
        '}



        Public Function GetEnumerator() As IEnumerator(Of [Function]) Implements IEnumerable(Of [Function]).GetEnumerator
            Return New mEnumerator(Of [Function])(Me)
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

#Region "private"

        '/**
        '  <summary>Checks whether the specified Function Is valid For insertion.</summary>
        '  <param name = "value" > Function() to validate.</param>
        '*/
        Private Sub Validate(ByVal value As [Function])
            If (value.InputCount <> 1) Then Throw New ArgumentException("value parameter MUST be 1-input function.")
        End Sub

#End Region
#End Region
#End Region

    End Class

End Namespace
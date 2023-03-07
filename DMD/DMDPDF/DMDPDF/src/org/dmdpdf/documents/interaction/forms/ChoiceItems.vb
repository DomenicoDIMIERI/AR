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
Imports DMD.org.dmdpdf.documents.interaction.annotations
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects

Imports System
Imports System.Collections
Imports System.Collections.Generic

Namespace DMD.org.dmdpdf.documents.interaction.forms

    '/**
    '  <summary>Field options [PDF:1.6:8.6.3].</summary>
    '*/
    <PDF(VersionEnum.PDF12)>
    Public NotInheritable Class ChoiceItems
        Inherits PdfObjectWrapper(Of PdfArray)
        Implements IList(Of ChoiceItem)

#Region "dynamic"
#Region "fields"

#End Region

#Region "constructors"

        Public Sub New(ByVal context As Document)
            MyBase.New(context, New PdfArray())
        End Sub

        Friend Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public Function Add(ByVal value As String) As ChoiceItem
            Dim item As ChoiceItem = New ChoiceItem(value)
            Add(item)
            Return item
        End Function

        Public Function Insert(ByVal index As Integer, ByVal value As String) As ChoiceItem
            Dim item As ChoiceItem = New ChoiceItem(value)
            Insert(index, item)
            Return item
        End Function

#Region "IList"

        Public Function IndexOf(ByVal value As ChoiceItem) As Integer Implements IList(Of ChoiceItem).IndexOf
            Return BaseDataObject.IndexOf(value.BaseObject)
        End Function

        Public Sub Insert(ByVal index As Integer, ByVal value As ChoiceItem) Implements IList(Of ChoiceItem).Insert
            BaseDataObject.Insert(index, value.BaseObject)
            value.Items = Me
        End Sub

        Public Sub RemoveAt(ByVal index As Integer) Implements IList(Of ChoiceItem).RemoveAt
            BaseDataObject.RemoveAt(index)
        End Sub

        Default Public Property Item(ByVal index As Integer) As ChoiceItem Implements IList(Of ChoiceItem).Item
            Get
                Return New ChoiceItem(BaseDataObject(index), Me)
            End Get
            Set(ByVal value As ChoiceItem)
                BaseDataObject(index) = value.BaseObject
                value.Items = Me
            End Set
        End Property

#Region "ICollection"

        Public Sub Add(ByVal value As ChoiceItem) Implements ICollection(Of ChoiceItem).Add
            BaseDataObject.Add(value.BaseObject)
            value.Items = Me
        End Sub

        Public Sub Clear() Implements ICollection(Of ChoiceItem).Clear
            BaseDataObject.Clear()
        End Sub

        Public Function Contains(ByVal value As ChoiceItem) As Boolean Implements ICollection(Of ChoiceItem).Contains
            Return BaseDataObject.Contains(value.BaseObject)
        End Function

        Public Sub CopyTo(ByVal values As ChoiceItem(), ByVal index As Integer) Implements ICollection(Of ChoiceItem).CopyTo
            Throw New NotImplementedException()
        End Sub

        Public ReadOnly Property Count As Integer Implements ICollection(Of ChoiceItem).Count
            Get
                Return BaseDataObject.Count
            End Get
        End Property

        Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of ChoiceItem).IsReadOnly
            Get
                Return False
            End Get
        End Property

        Public Function Remove(ByVal value As ChoiceItem) As Boolean Implements ICollection(Of ChoiceItem).Remove
            Return BaseDataObject.Remove(value.BaseObject)
        End Function


#Region "IEnumerable(Of ChoiceItem)"


        '    IEnumerator(of ChoiceItem) IEnumerable(of ChoiceItem).GetEnumerator(
        '  )
        '{
        '  For (
        '    int index = 0,
        '      length = Count;
        '    index < length;
        '    index++
        '    )
        '  {yield return Me[index];}
        '}

        '#Region IEnumerable
        'IEnumerator IEnumerable.GetEnumerator(
        '  )
        '{return ((IEnumerable(of ChoiceItem))Me).GetEnumerator();}
        '#endregion
        Public Function GetEnumerator() As IEnumerator(Of ChoiceItem) Implements IEnumerable(Of ChoiceItem).GetEnumerator
            Return New mEnumerator(Of ChoiceItem)(Me)
        End Function

        Private Function _GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Return Me.GetEnumerator
        End Function

#End Region
#End Region
#End Region
#End Region
#End Region
#End Region

    End Class

End Namespace
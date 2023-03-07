'/*
'  Copyright 2008-2011 Stefano Chizzolini. http://www.dmdpdf.org

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

Namespace DMD.org.dmdpdf.documents.interaction.actions

    '/**
    '  <summary>Chained actions [PDF:1.6:8.5.1].</summary>
    '*/
    <PDF(VersionEnum.PDF12)>
    Public NotInheritable Class ChainedActions
        Inherits PdfObjectWrapper(Of PdfDataObject)
        Implements IList(Of Action)

        '    /*
        '  NOTE: Chained actions may be either singular Or multiple (within an array).
        '  This implementation hides such a complexity To the user, smoothly exposing
        '  just the most general Case (array) yet preserving its internal state.
        '*/
#Region "dynamic"
#Region "fields"
        '/**
        '  Parent action.
        '*/
        Private _parent As Action

#End Region

#Region "constructors"

        Friend Sub New(ByVal baseObject As PdfDirectObject, ByVal parent As Action)
            MyBase.New(baseObject)
            Me._parent = parent
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public Overrides Function Clone(ByVal context As Document) As Object
            Throw New NotImplementedException 'TODO:verify
        End Function

        '/**
        '  <summary> Gets the parent action.</summary>
        '*/
        Public ReadOnly Property Parent As Action
            Get
                Return Me._parent
            End Get
        End Property

#Region "IList"

        Public Function IndexOf(ByVal value As Action) As Integer Implements IList(Of actions.Action).IndexOf
            Dim baseDataObject As PdfDataObject = Me.baseDataObject
            If (TypeOf (baseDataObject) Is PdfDictionary) Then ' SingleThen action.
                If (CType(value, Action).BaseObject.Equals(BaseObject)) Then
                    Return 0
                Else
                    Return -1
                End If
            Else ' Multiple actions.
                Return CType(baseDataObject, PdfArray).IndexOf(CType(value, Action).BaseObject)
            End If
        End Function

        Public Sub Insert(ByVal index As Integer, ByVal value As Action) Implements IList(Of actions.Action).Insert
            EnsureArray().Insert(index, value.BaseObject)
        End Sub

        Public Sub RemoveAt(ByVal index As Integer) Implements IList(Of actions.Action).RemoveAt
            EnsureArray().RemoveAt(index)
        End Sub

        Public Property Item(ByVal index As Integer) As Action Implements IList(Of Action).Item
            Get
                Dim BaseDataObject As PdfDataObject = Me.BaseDataObject
                If (TypeOf (BaseDataObject) Is PdfDictionary) Then 'Single Then action.
                    If (index <> 0) Then Throw New ArgumentException("Index: " & index & ", Size: 1")
                    Return Action.Wrap(BaseObject)
                Else ' Multiple actions.
                    Return Action.Wrap(CType(BaseDataObject, PdfArray)(index))
                End If
            End Get
            Set(ByVal value As Action)
                EnsureArray()(index) = value.BaseObject
            End Set
        End Property

#Region "ICollection"

        Public Sub Add(ByVal value As Action) Implements ICollection(Of Action).Add
            EnsureArray().Add(value.BaseObject)
        End Sub

        Public Sub Clear() Implements ICollection(Of Action).Clear
            EnsureArray().Clear()
        End Sub

        Public Function Contains(ByVal value As Action) As Boolean Implements ICollection(Of Action).Contains
            Dim BaseDataObject As PdfDataObject = Me.BaseDataObject
            If (TypeOf (BaseDataObject) Is PdfDictionary) Then ' Single Then Action.
                Return CType(value, Action).BaseObject.Equals(BaseObject)
            Else ' Multiple actions.
                Return CType(BaseDataObject, PdfArray).Contains(CType(value, Action).BaseObject)
            End If
        End Function

        Public Sub CopyTo(ByVal values As Action(), ByVal index As Integer) Implements ICollection(Of Action).CopyTo
            Throw New NotImplementedException()
        End Sub

        Public ReadOnly Property Count As Integer Implements ICollection(Of Action).Count
            Get
                Dim BaseDataObject As PdfDataObject = Me.BaseDataObject
                If (TypeOf (BaseDataObject) Is PdfDictionary) Then ' Single Then action.
                    Return 1
                Else ' Multiple actions.
                    Return CType(BaseDataObject, PdfArray).Count
                End If
            End Get
        End Property

        Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of Action).IsReadOnly
            Get
                Return False
            End Get
        End Property

        Public Function Remove(ByVal value As Action) As Boolean Implements ICollection(Of actions.Action).Remove
            Return EnsureArray().Remove(CType(value, Action).BaseObject)
        End Function

#Region "IEnumerable(Of Action)"

        '    IEnumerator(Of Action) IEnumerable(Of Action).GetEnumerator(
        '  )
        '{
        '  For (
        '    byval index as Integer = 0,
        '      length = Count;
        '    index <length;
        '    index++
        '    )
        '  {yield return this(index);}
        '}


        Public Function GetEnumerator() As IEnumerator(Of Action) Implements IEnumerable(Of Action).GetEnumerator
            Return New mEnumerator(Of Action)(Me)
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

        Private Function EnsureArray() As PdfArray
            Dim baseDataObject As PdfDataObject = Me.BaseDataObject
            If (TypeOf (baseDataObject) Is PdfDictionary) Then ' SingleThen action.
                Dim actionsArray As PdfArray = New PdfArray()
                actionsArray.Add(BaseObject)
                BaseObject = actionsArray
                _parent.BaseDataObject(PdfName.Next) = actionsArray
                baseDataObject = actionsArray
            End If
            Return CType(baseDataObject, PdfArray)
        End Function

#End Region
#End Region
#End Region

    End Class

End Namespace
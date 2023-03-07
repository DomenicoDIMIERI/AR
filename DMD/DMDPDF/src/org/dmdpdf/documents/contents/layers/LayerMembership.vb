'/*
'  Copyright 2011-2012 Stefano Chizzolini. http://www.dmdpdf.org

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

Imports DMD.org.dmdpdf.objects

Imports System
Imports System.Collections
Imports System.Collections.Generic

Namespace DMD.org.dmdpdf.documents.contents.layers

    '/**
    '  <summary>Optional content membership [PDF:1.7:4.10.1].</summary>
    '*/
    <PDF(VersionEnum.PDF15)>
    Public NotInheritable Class LayerMembership
        Inherits LayerEntity

#Region "types"

        '/**
        '  <summary>Layers whose states determine the visibility of content controlled by a membership.</summary>
        '*/
        Private Class VisibilityLayersImpl
            Inherits PdfObjectWrapper(Of PdfDirectObject)
            Implements IList(Of Layer)

#Region "fields"

            Private _membership As LayerMembership

#End Region

#Region "constructors"

            Friend Sub New(ByVal membership As LayerMembership)
                MyBase.New(membership.BaseDataObject(PdfName.OCGs))
                Me._membership = membership
            End Sub

#End Region

#Region "Interface"
#Region "Public"
#Region "IList(Of Layer)"

            Public Function IndexOf(ByVal item As Layer) As Integer Implements IList(Of Layer).IndexOf
                Dim baseDataObject As PdfDataObject = Me.BaseDataObject
                If (baseDataObject Is Nothing) Then 'No Then Layer.
                    Return -1
                ElseIf (TypeOf (baseDataObject) Is PdfDictionary) Then ' Single Then layer.
                    Return IIF(item.BaseObject.Equals(BaseObject), 0, -1)
                Else ' Multiple layers.
                    Return CType(baseDataObject, PdfArray).IndexOf(item.BaseObject)
                End If
            End Function

            Public Sub Insert(ByVal Index As Integer, ByVal item As Layer) Implements IList(Of Layer).Insert
                EnsureArray().Insert(Index, item.BaseObject)
            End Sub

            Public Sub RemoveAt(ByVal Index As Integer) Implements IList(Of Layer).RemoveAt
                EnsureArray().RemoveAt(Index)
            End Sub

            Default Public Property Item(ByVal Index As Integer) As Layer Implements IList(Of Layer).Item
                Get
                    Dim baseDataObject As PdfDataObject = Me.BaseDataObject
                    If (baseDataObject Is Nothing) Then ' No Then Layer.
                        Return Nothing
                    ElseIf (TypeOf (baseDataObject) Is PdfDictionary) Then ' Single Then layer.
                        If (Index = 0) Then Throw New IndexOutOfRangeException()
                        Return Layer.Wrap(BaseObject)
                    Else ' Multiple layers.
                        Return Layer.Wrap(CType(baseDataObject, PdfArray)(Index))
                    End If
                End Get
                Set(ByVal value As Layer)
                    EnsureArray()(Index) = value.BaseObject
                End Set
            End Property

#Region "ICollection<Page>"

            Public Sub Add(ByVal item As Layer) Implements ICollection(Of Layer).Add
                EnsureArray().Add(item.BaseObject)
            End Sub

            Public Sub Clear() Implements ICollection(Of Layer).Clear
                EnsureArray().Clear()
            End Sub

            Public Function Contains(ByVal item As Layer) As Boolean Implements ICollection(Of Layer).Contains
                Dim baseDataObject As PdfDataObject = Me.BaseDataObject
                If (baseDataObject Is Nothing) Then ' No Then Layer.
                    Return False
                ElseIf (TypeOf (baseDataObject) Is PdfDictionary) Then ' Single Then layer.
                    Return item.BaseObject.Equals(BaseObject)
                Else ' Multiple layers.
                    Return CType(baseDataObject, PdfArray).Contains(item.BaseObject)
                End If
            End Function

            Public Sub CopyTo(ByVal items As Layer(), ByVal Index As Integer) Implements ICollection(Of Layer).CopyTo
                Throw New NotImplementedException
            End Sub

            Public ReadOnly Property Count As Integer Implements ICollection(Of Layer).Count
                Get
                    Dim baseDataObject As PdfDataObject = Me.BaseDataObject
                    If (baseDataObject Is Nothing) Then ' No Then Layer.
                        Return 0
                    ElseIf (TypeOf (baseDataObject) Is PdfDictionary) Then ' Single Then layer.
                        Return 1
                    Else ' Multiple layers.
                        Return CType(baseDataObject, PdfArray).Count
                    End If
                End Get
            End Property

            Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of Layer).IsReadOnly
                Get
                    Return False
                End Get
            End Property

            Public Function Remove(ByVal item As Layer) As Boolean Implements ICollection(Of Layer).Remove
                Return EnsureArray().Remove(item.BaseObject)
            End Function

#Region "IEnumerable(Of Layer)"


            Public Function GetEnumerator() As IEnumerator(Of Layer) Implements IEnumerable(Of Layer).GetEnumerator
                Return New mEnumerator(Of Layer)(Me)
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

#Region "Private"

            Private Function EnsureArray() As PdfArray
                Dim baseDataObject As PdfDirectObject = Me.BaseDataObject
                If (Not (TypeOf (baseDataObject) Is PdfArray)) Then
                    Dim Array As PdfArray = New PdfArray()
                    If (baseDataObject IsNot Nothing) Then Array.Add(baseDataObject)
                    baseDataObject = Array
                    BaseObject = baseDataObject
                    _membership.BaseDataObject(PdfName.OCGs) = BaseObject
                End If
                Return CType(BaseDataObject, PdfArray)
            End Function

#End Region
#End Region
        End Class

#End Region

#Region "Static"
#Region "fields"

        Public Shared TypeName As PdfName = PdfName.OCMD

#End Region

#Region "Interface"
#Region "Public"

        Public Shared Shadows Function Wrap(ByVal BaseObject As PdfDirectObject) As LayerMembership
            If (BaseObject IsNot Nothing) Then
                Return New LayerMembership(BaseObject)
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
            MyBase.New(context, TypeName)
        End Sub

        Private Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.new(baseObject)
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public Overrides ReadOnly Property Membership As LayerMembership
            Get
                Return Me
            End Get
        End Property

        Public Overrides ReadOnly Property VisibilityLayers As IList(Of Layer)
            Get
                Return New VisibilityLayersImpl(Me)
            End Get
        End Property

        Public Overrides Property VisibilityPolicy As VisibilityPolicyEnum
            Get
                Return VisibilityPolicyEnumExtension.Get(CType(BaseDataObject(PdfName.P), PdfName))
            End Get
            Set(ByVal value As VisibilityPolicyEnum)
                BaseDataObject(PdfName.P) = value.GetName()
            End Set
        End Property

#End Region
#End Region
#End Region

    End Class

End Namespace
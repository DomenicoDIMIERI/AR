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

Imports DMD.org.dmdpdf.bytes
Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.documents.interaction.forms
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.tokens

Imports System
Imports System.Collections.Generic

Namespace DMD.org.dmdpdf.objects

    '/**
    '  <summary>Object cloner.</summary>
    '*/
    Public Class Cloner
        Inherits Visitor

#Region "types"

        Public Class Filter

            Private ReadOnly _name As String

            Public Sub New(ByVal name As String)
                Me._name = name
            End Sub

            '/**
            '  <summary>Notifies a complete clone operation on an object.</summary>
            '  <param name="cloner">Object cloner.</param>
            '  <param name="clone">Clone object.</param>
            '  <param name="source">Source object.</param>
            '*/
            Public Overridable Sub AfterClone(ByVal cloner As Cloner, ByVal clone As PdfObject, ByVal source As PdfObject)
                ' NOOP
            End Sub

            '/**
            '  <summary>Notifies a complete clone operation on a dictionary entry.</summary>
            '  <param name="cloner">Object cloner.</param>
            '  <param name="parent">Parent clone object.</param>
            '  <param name="key">Entry key within the parent.</param>
            '  <param name="value">Clone value.</param>
            '*/
            Public Overridable Sub AfterClone(ByVal cloner As Cloner, ByVal parent As PdfDictionary, ByVal key As PdfName, ByVal value As PdfDirectObject)
                ' NOOP 
            End Sub

            '/**
            '  <summary>Notifies a complete clone operation on an array item.</summary>
            '  <param name="cloner">Object cloner.</param>
            '  <param name="parent">Parent clone object.</param>
            '  <param name="index">Item index within the parent.</param>
            '  <param name="item">Clone item.</param>
            '*/
            Public Overridable Sub AfterClone(ByVal cloner As Cloner, ByVal parent As PdfArray, ByVal index As Integer, ByVal item As PdfDirectObject)
                ' NOOP 
            End Sub

            '/**
            '  <summary> Notifies a starting clone operation On a dictionary entry.</summary>
            '  <param name = "cloner" > Object cloner.</param>
            '  <param name = "parent" > parent clone Object.</param>
            '  <param name = "key" > Entry key within the parent.</param>
            '  <param name = "value" > source value.</param>
            '  <returns> Whether the clone operation can be fulfilled.</returns>
            '*/
            Public Overridable Function BeforeClone(ByVal cloner As Cloner, ByVal parent As PdfDictionary, ByVal key As PdfName, ByVal value As PdfDirectObject) As Boolean
                Return True
            End Function

            '/**
            '  <summary> Notifies a starting clone operation On an array item.</summary>
            '  <param name = "cloner" > Object cloner.</param>
            '  <param name = "parent" > parent clone Object.</param>
            '  <param name = "index" > item index within the parent.</param>
            '  <param name = "item" > source item.</param>
            '  <returns> Whether the clone operation can be fulfilled.</returns>
            '*/
            Public Overridable Function BeforeClone(ByVal cloner As Cloner, ByVal parent As PdfArray, ByVal index As Integer, ByVal item As PdfDirectObject) As Boolean
                Return True
            End Function

            '/**
            '  <summary> Gets whether Me filter can deal With the given Object.</summary>
            '  <param name = "cloner" > Object cloner.</param>
            '  <param name = "source" > source Object.</param>
            '*/
            Public Overridable Function Matches(ByVal cloner As Cloner, ByVal source As PdfObject) As Boolean
                Return True
            End Function

            Public ReadOnly Property Name As String
                Get
                    Return Me._name
                End Get
            End Property

        End Class

        Private Class AnnotationsFilter
            Inherits Filter
            Public Sub New()
                MyBase.New("Annots")
            End Sub

            Public Overrides Sub AfterClone(ByVal cloner As Cloner, ByVal parent As PdfArray, ByVal index As Integer, ByVal item As PdfDirectObject)
                Dim annotation As PdfDictionary = CType(item.Resolve(), PdfDictionary)
                If (annotation.ContainsKey(PdfName.FT)) Then
                    cloner._context.Document.Form.Fields.Add(Field.Wrap(annotation.Reference))
                End If
            End Sub

            Public Overrides Function Matches(ByVal cloner As Cloner, ByVal obj As PdfObject) As Boolean
                If (TypeOf (obj) Is PdfArray) Then
                    Dim Array As PdfArray = CType(obj, PdfArray)
                    If (Array.Count > 0) Then
                        Dim arrayItem As PdfDataObject = Array.Resolve(0)
                        If (TypeOf (arrayItem) Is PdfDictionary) Then
                            Dim arrayItemDictionary As PdfDictionary = CType(arrayItem, PdfDictionary)
                            Return arrayItemDictionary.ContainsKey(PdfName.Subtype) AndAlso
                                   arrayItemDictionary.ContainsKey(PdfName.Rect)
                        End If
                    End If
                End If
                Return False
            End Function

        End Class


        Private Class PageFilter
            Inherits Filter

            Public Sub New()
                MyBase.New("Page")
            End Sub

            Public Overrides Sub AfterClone(ByVal cloner As Cloner, ByVal clone As PdfObject, ByVal source As PdfObject)
                '/*
                '  NOTE: Inheritable attributes have To be consolidated into the cloned page dictionary In
                '  order to ensure its consistency.
                '*/
                Dim cloneDictionary As PdfDictionary = CType(clone, PdfDictionary)
                Dim sourceDictionary As PdfDictionary = CType(source, PdfDictionary)
                For Each key As PdfName In Page.InheritableAttributeKeys
                    If (Not sourceDictionary.ContainsKey(key)) Then
                        Dim sourceValue As PdfDirectObject = Page.GetInheritableAttribute(sourceDictionary, key)
                        If (sourceValue IsNot Nothing) Then
                            cloneDictionary(key) = CType(sourceValue.Accept(cloner, Nothing), PdfDirectObject)
                        End If
                    End If
                Next
            End Sub

            Public Overrides Function BeforeClone(ByVal cloner As Cloner, ByVal parent As PdfDictionary, ByVal key As PdfName, ByVal value As PdfDirectObject) As Boolean
                Return Not PdfName.Parent.Equals(key)
            End Function

            Public Overrides Function Matches(ByVal cloner As Cloner, ByVal obj As PdfObject) As Boolean
                Return (TypeOf (obj) Is PdfDictionary) AndAlso PdfName.Page.Equals(CType(obj, PdfDictionary)(PdfName.Type))
            End Function
        End Class

#End Region

#Region "Static"
#Region "fields"

        Private Shared ReadOnly _NullFilter As Filter = New Filter("Default")

        Private Shared ReadOnly _commonFilters As IList(Of Filter) = New List(Of Filter)()

#End Region

#Region "constructors"

        Shared Sub New()
            ' Page object.
            _commonFilters.Add(New PageFilter())
            ' Annotations.
            _commonFilters.Add(New AnnotationsFilter())
        End Sub

#End Region
#End Region

#Region "dynamic"
#Region "fields"

        Private _context As File
        Private ReadOnly _filters As IList(Of Filter) = New List(Of Filter)(_commonFilters)

#End Region

#Region "constructors"

        Public Sub New(ByVal context As File)
            Me.Context = context
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public Property Context As File
            Get
                Return Me._context
            End Get
            Set(ByVal value As File)
                If (value Is Nothing) Then Throw New ArgumentException("value required")
                Me._context = value
            End Set
        End Property

        Public ReadOnly Property Filters As IList(Of Filter)
            Get
                Return Me._filters
            End Get
        End Property


        Public Overrides Function Visit(ByVal obj As ObjectStream, ByVal data As Object) As PdfObject
            Throw New NotSupportedException()
        End Function

        Public Overrides Function Visit(ByVal obj As PdfArray, ByVal data As Object) As PdfObject
            Dim cloneFilter As Filter = MatchFilter(obj)
            Dim clone As PdfArray = CType(obj.Clone(), PdfArray)
            clone._items = New List(Of PdfDirectObject)
            Dim sourceItems As IList(Of PdfDirectObject) = obj._items
            Dim length As Integer = sourceItems.Count
            For index As Integer = 0 To length - 1
                Dim sourceItem As PdfDirectObject = sourceItems(index)
                If (cloneFilter.BeforeClone(Me, clone, index, sourceItem)) Then
                    Dim cloneItem As PdfDirectObject
                    If (sourceItem IsNot Nothing) Then
                        cloneItem = CType(sourceItem.Accept(Me, Nothing), PdfDirectObject)
                    Else
                        cloneItem = Nothing
                    End If
                    clone.Add(cloneItem)
                    cloneFilter.AfterClone(Me, clone, index, cloneItem)
                End If
            Next
            cloneFilter.AfterClone(Me, clone, obj)
            Return clone
        End Function

        Public Overrides Function Visit(ByVal obj As PdfDictionary, ByVal data As Object) As PdfObject
            Dim cloneFilter As Filter = MatchFilter(obj)
            Dim clone As PdfDictionary = CType(obj.Clone(), PdfDictionary)
            clone._entries = New Dictionary(Of PdfName, PdfDirectObject)
            For Each entry As KeyValuePair(Of PdfName, PdfDirectObject) In obj._entries
                Dim sourceValue As PdfDirectObject = entry.Value
                If (cloneFilter.BeforeClone(Me, clone, entry.Key, sourceValue)) Then
                    Dim cloneValue As PdfDirectObject
                    If (sourceValue IsNot Nothing) Then
                        cloneValue = CType(sourceValue.Accept(Me, Nothing), PdfDirectObject)
                    Else
                        cloneValue = Nothing
                    End If
                    cloneValue = sourceValue
                    clone(entry.Key) = cloneValue
                    cloneFilter.AfterClone(Me, clone, entry.Key, cloneValue)
                End If
            Next
            cloneFilter.AfterClone(Me, clone, obj)
            Return clone
        End Function

        Public Overrides Function Visit(ByVal obj As PdfIndirectObject, ByVal data As Object) As PdfObject
            Return _context.IndirectObjects.AddExternal(obj, Me)
        End Function

        Public Overrides Function Visit(ByVal obj As PdfReference, ByVal data As Object) As PdfObject
            If (_context Is obj.File) Then
                Return CType(obj.Clone(), PdfReference) ' Local clone.
            Else
                Return Visit(obj.IndirectObject, data).Reference ' Alien clone.
            End If
        End Function

        Public Overrides Function Visit(ByVal obj As PdfStream, ByVal data As Object) As PdfObject
            Dim clone As PdfStream = CType(obj.Clone(), PdfStream)
            clone._header = CType(Visit(obj.Header, data), PdfDictionary)
            clone._body = obj.Body.Clone()
            Return clone
        End Function

        Public Overrides Function Visit(ByVal obj As XRefStream, ByVal data As Object) As PdfObject
            Throw New NotSupportedException()
        End Function

#End Region

#Region "Private"

        Private Function MatchFilter(ByVal obj As PdfObject) As Filter
            Dim cloneFilter As Filter = _NullFilter
            For Each filter As Filter In _filters
                If (filter.Matches(Me, obj)) Then
                    cloneFilter = filter
                    Exit For
                End If
            Next
            Return cloneFilter
        End Function

#End Region
#End Region
#End Region

    End Class

End Namespace

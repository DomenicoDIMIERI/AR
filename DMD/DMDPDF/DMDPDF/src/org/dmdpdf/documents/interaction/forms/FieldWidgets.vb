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

'  You should have received a copy of the GNU Lesser General Public License along with this
'  Program (see README files); if not, go to the GNU website (http://www.gnu.org/licenses/).

'  Redistribution and use, with or without modification, are permitted provided that such
'  redistributions retain the above copyright notice, license and disclaimer, along with
'  this list of conditions.
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
    '  <summary>Field widget annotations [PDF:1.6:8.6].</summary>
    '*/
    <PDF(VersionEnum.PDF12)>
    Public NotInheritable Class FieldWidgets
        Inherits PdfObjectWrapper(Of PdfDataObject)
        Implements IList(Of Widget)

        '    /*
        '  NOTE: Widget annotations may be singular (either merged To their field Or within an array)
        '  Or multiple (within an array).
        '  This implementation hides such a complexity To the user, smoothly exposing just the most
        '  general case (array) yet preserving its internal state.
        '*/
#Region "dynamic"
#Region "fields"

        Private _field As Field

#End Region

#Region "constructors"

        Friend Sub New(ByVal baseObject As PdfDirectObject, ByVal field As Field)
            MyBase.New(baseObject)
            Me._field = Field
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public Overrides Function Clone(ByVal context As Document) As Object
            Throw New NotImplementedException() ' TODO:verify field reference.
        End Function

        '/**
        '  <summary> Gets the field associated To these widgets.</summary>
        '*/
        Public ReadOnly Property Field As Field
            Get
                Return Me._field
            End Get
        End Property

#Region "IList"

        Public Function IndexOf(ByVal value As Widget) As Integer Implements IList(Of Widget).IndexOf
            Dim baseDataObject As PdfDataObject = Me.BaseDataObject
            If (TypeOf (baseDataObject) Is PdfDictionary) Then 'SingleThen annotation.
                If (value.BaseObject.Equals(BaseObject)) Then
                    Return 0
                Else
                    Return -1
                End If
            End If
            Return CType(baseDataObject, PdfArray).IndexOf(value.BaseObject)
        End Function

        Public Sub Insert(ByVal index As Integer, ByVal value As Widget) Implements IList(Of Widget).Insert
            EnsureArray().Insert(index, value.BaseObject)
        End Sub

        Public Sub RemoveAt(ByVal index As Integer) Implements IList(Of Widget).RemoveAt
            EnsureArray().RemoveAt(index)
        End Sub

        Default Public Property Item(ByVal index As Integer) As Widget Implements IList(Of Widget).Item
            Get
                Dim baseDataObject As PdfDataObject = Me.BaseDataObject
                If (TypeOf (baseDataObject) Is PdfDictionary) Then 'SingleThen annotation.
                    If (index <> 0) Then Throw New ArgumentException("Index: " & index & ", Size: 1")
                    Return NewWidget(BaseObject)
                End If
                Return NewWidget(CType(baseDataObject, PdfArray)(index))
            End Get
            Set(ByVal value As Widget)
                EnsureArray()(index) = value.BaseObject
            End Set
        End Property

#Region "ICollection"

        Public Sub Add(ByVal value As Widget) Implements ICollection(Of Widget).Add
            value.BaseDataObject(PdfName.Parent) = Me._field.BaseObject
            EnsureArray().Add(value.BaseObject)
        End Sub

        Public Sub Clear() Implements ICollection(Of Widget).Clear
            EnsureArray().Clear()
        End Sub

        Public Function Contains(ByVal value As Widget) As Boolean Implements ICollection(Of Widget).Contains
            Dim baseDataObject As PdfDataObject = Me.BaseDataObject
            If (TypeOf (baseDataObject) Is PdfDictionary) Then ' SingleThen annotation.
                Return value.BaseObject.Equals(BaseObject)
            End If
            Return CType(baseDataObject, PdfArray).Contains(value.BaseObject)
        End Function

        Public Sub CopyTo(ByVal values As Widget(), ByVal index As Integer) Implements ICollection(Of Widget).CopyTo
            Throw New NotImplementedException()
        End Sub

        Public ReadOnly Property Count As Integer Implements ICollection(Of Widget).Count
            Get
                Dim baseDataObject As PdfDataObject = Me.BaseDataObject
                If (TypeOf (baseDataObject) Is PdfDictionary) Then ' SingleThen annotation.
                    Return 1
                End If
                Return CType(baseDataObject, PdfArray).Count
            End Get
        End Property

        Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of Widget).IsReadOnly
            Get
                Return False
            End Get
        End Property

        Public Function Remove(ByVal value As Widget) As Boolean Implements ICollection(Of Widget).Remove
            Return EnsureArray().Remove(value.BaseObject)
        End Function

#Region "IEnumerable<Widget>"



        Public Function GetEnumerator() As IEnumerator(Of Widget) Implements IEnumerable(Of Widget).GetEnumerator
            Return New mEnumerator(Of Widget)(Me)
        End Function




        '  IEnumerator<Widget> IEnumerable<Widget>.GetEnumerator(      ) 
        'For (int index = 0, length = Count; index < length; index++ )
        'yield return this[index]
        'Next
        '  End Function

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
            Dim BaseDataObject As PdfDataObject = Me.BaseDataObject
            If (TypeOf (BaseDataObject) Is PdfDictionary) Then 'MergedThenThen annotation.
                Dim widgetsArray As PdfArray = New PdfArray()
                Dim fieldDictionary As PdfDictionary = CType(BaseDataObject, PdfDictionary)
                Dim widgetDictionary As PdfDictionary = Nothing
                ' Extracting widget entries from the field...
                For Each key As PdfName In New List(Of PdfName)(fieldDictionary.Keys)
                    ' Is it a widget entry?
                    If (
                    key.Equals(PdfName.Type) OrElse
                    key.Equals(PdfName.Subtype) OrElse
                    key.Equals(PdfName.Rect) OrElse
                    key.Equals(PdfName.Contents) OrElse
                    key.Equals(PdfName.P) OrElse
                    key.Equals(PdfName.NM) OrElse
                    key.Equals(PdfName.M) OrElse
                    key.Equals(PdfName.F) OrElse
                    key.Equals(PdfName.BS) OrElse
                    key.Equals(PdfName.AP) OrElse
                    key.Equals(PdfName.AS) OrElse
                    key.Equals(PdfName.Border) OrElse
                    key.Equals(PdfName.C) OrElse
                    key.Equals(PdfName.A) OrElse
                    key.Equals(PdfName.AA) OrElse
                    key.Equals(PdfName.StructParent) OrElse
                    key.Equals(PdfName.OC) OrElse
                    key.Equals(PdfName.H) OrElse
                    key.Equals(PdfName.MK)
                    ) Then

                        If (widgetDictionary Is Nothing) Then
                            widgetDictionary = New PdfDictionary()
                            Dim widgetReference As PdfReference = File.Register(widgetDictionary)

                            ' Remove the field from the page annotations (as the widget annotation Is decoupled from it)!
                            Dim pageAnnotationsArray As PdfArray = CType(CType(fieldDictionary.Resolve(PdfName.P), PdfDictionary).Resolve(PdfName.Annots), PdfArray)
                            pageAnnotationsArray.Remove(_field.BaseObject)

                            ' Add the widget to the page annotations!
                            pageAnnotationsArray.Add(widgetReference)
                            ' Add the widget to the field widgets!
                            widgetsArray.Add(widgetReference)
                            ' Associate the field to the widget!
                            widgetDictionary(PdfName.Parent) = _field.BaseObject
                        End If

                        ' Transfer the entry from the field to the widget!
                        widgetDictionary(key) = fieldDictionary(key)
                        fieldDictionary.Remove(key)
                    End If
                Next
                BaseObject = widgetsArray
                _field.BaseDataObject(PdfName.Kids) = widgetsArray
                BaseDataObject = widgetsArray
            End If

            Return CType(BaseDataObject, PdfArray)
        End Function

        Private Function NewWidget(ByVal baseObject As PdfDirectObject) As Widget
            Return Widget.Wrap(baseObject, _field)
        End Function

#End Region
#End Region
#End Region

    End Class

End Namespace
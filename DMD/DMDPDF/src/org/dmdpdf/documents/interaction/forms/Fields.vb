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
    '  <summary>Interactive form fields [PDF:1.6:8.6.1].</summary>
    '*/
    <PDF(VersionEnum.PDF12)>
    Public NotInheritable Class Fields
        Inherits PdfObjectWrapper(Of PdfArray)
        Implements IDictionary(Of String, Field)

#Region "dynamic"
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

        Public Sub Add(ByVal value As Field)
            Me.BaseDataObject.Add(value.BaseObject)
        End Sub


#Region "IDictionary"

        Public Sub Add(ByVal key As String, ByVal value As Field) Implements IDictionary(Of String, forms.Field).Add
            Throw New NotImplementedException()
        End Sub

        Public Function ContainsKey(ByVal key As String) As Boolean Implements IDictionary(Of String, Field).ContainsKey
            'TODO: avoid getter (use raw matching).
            Return Me(key) IsNot Nothing
        End Function

        Public ReadOnly Property Keys As ICollection(Of String) Implements IDictionary(Of String, forms.Field).Keys
            Get
                Throw New NotImplementedException()
                'TODO: retrieve all the full names (keys)!!!
            End Get
        End Property

        Public Function Remove(ByVal key As String) As Boolean Implements IDictionary(Of String, Field).Remove
            Dim Field As Field = Me(key)
            If (Field Is Nothing) Then Return False
            Dim fieldObjects As PdfArray
            Dim fieldParentReference As PdfReference = CType(Field.BaseDataObject(PdfName.Parent), PdfReference)
            If (fieldParentReference Is Nothing) Then
                fieldObjects = Me.BaseDataObject
            Else
                fieldObjects = CType(CType(fieldParentReference.DataObject, PdfDictionary).Resolve(PdfName.Kids), PdfArray)
            End If

            Return fieldObjects.Remove(Field.BaseObject)
        End Function

        Default Public Property Item(ByVal key As String) As Field Implements IDictionary(Of String, forms.Field).Item
            Get
                '/*
                '  TODO: It is possible for different field dictionaries to have the SAME fully qualified field
                '  name if they are descendants of a common ancestor with that name and have no
                '  partial field names (T entries) of their own. Such field dictionaries are different
                '  representations of the same underlying field; they should differ only in properties
                '  that specify their visual appearance. In particular, field dictionaries with the same
                '  fully qualified field name must have the same field type (FT), value (V), and default
                '  value (DV).
                ' */
                Dim valueFieldReference As PdfReference = Nothing
                Dim partialNamesIterator As IEnumerator = key.Split("."c).GetEnumerator()
                Dim fieldObjectsIterator As IEnumerator(Of PdfDirectObject) = Me.BaseDataObject.GetEnumerator()
                While (partialNamesIterator.MoveNext())
                    Dim partialName As String = CStr(partialNamesIterator.Current)
                    valueFieldReference = Nothing
                    While (fieldObjectsIterator IsNot Nothing AndAlso fieldObjectsIterator.MoveNext())
                        Dim fieldReference As PdfReference = CType(fieldObjectsIterator.Current, PdfReference)
                        Dim fieldDictionary As PdfDictionary = CType(fieldReference.DataObject, PdfDictionary)
                        Dim fieldName As PdfTextString = CType(fieldDictionary(PdfName.T), PdfTextString)
                        If (fieldName IsNot Nothing AndAlso fieldName.Value.Equals(partialName)) Then
                            valueFieldReference = fieldReference
                            Dim kidFieldObjects As PdfArray = CType(fieldDictionary.Resolve(PdfName.Kids), PdfArray)
                            If (kidFieldObjects Is Nothing) Then
                                fieldObjectsIterator = Nothing
                            Else
                                fieldObjectsIterator = kidFieldObjects.GetEnumerator()
                            End If

                            Exit While
                        End If
                    End While
                    If (valueFieldReference Is Nothing) Then Exit While
                End While

                Return Field.Wrap(valueFieldReference)
            End Get
            Set(ByVal value As Field)
                Throw New NotImplementedException()
                '/*
                'TODO:put the field into the correct position, based on the full name (key)!!!
                '*/
            End Set
        End Property

        Public Function TryGetValue(ByVal key As String, ByRef value As Field) As Boolean Implements IDictionary(Of String, Field).TryGetValue
            value = Me(key)
            Return (value IsNot Nothing OrElse ContainsKey(key))
        End Function

        Public ReadOnly Property Values As ICollection(Of Field) Implements IDictionary(Of String, Field).Values
            Get
                Dim _values As IList(Of Field) = New List(Of Field)
                RetrieveValues(BaseDataObject, _values)
                Return _values
            End Get
        End Property

#Region "ICollection"

        Private Sub Add(ByVal entry As KeyValuePair(Of String, Field)) Implements ICollection(Of KeyValuePair(Of String, Field)).Add
            Add(entry.Key, entry.Value)
        End Sub

        Public Sub Clear() Implements ICollection(Of KeyValuePair(Of String, Field)).Clear
            Me.BaseDataObject.Clear()
        End Sub

        Private Function Contains(ByVal entry As KeyValuePair(Of String, Field)) As Boolean Implements ICollection(Of KeyValuePair(Of String, Field)).Contains
            Throw New NotImplementedException()
        End Function

        Public Sub CopyTo(ByVal entries As KeyValuePair(Of String, Field)(), ByVal index As Integer) Implements ICollection(Of KeyValuePair(Of String, Field)).CopyTo
            Throw New NotImplementedException()
        End Sub

        Public ReadOnly Property Count As Integer Implements ICollection(Of KeyValuePair(Of String, Field)).Count
            Get
                Return Values.Count
            End Get
        End Property

        Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of KeyValuePair(Of String, Field)).IsReadOnly
            Get
                Return False
            End Get
        End Property

        Public Function Remove(ByVal entry As KeyValuePair(Of String, Field)) As Boolean Implements ICollection(Of KeyValuePair(Of String, Field)).Remove
            Throw New NotImplementedException()
        End Function

#Region "IEnumerable<KeyValuePair(Of String,Field)>"

        Public Function GetEnumerator() As IEnumerator(Of KeyValuePair(Of String, Field)) Implements IEnumerable(Of KeyValuePair(Of String, Field)).GetEnumerator
            Throw New NotImplementedException()
        End Function
        '    IEnumerator<KeyValuePair(Of String,Field)> IEnumerable<KeyValuePair(Of String,Field)>.GetEnumerator(
        '  )
        '{throw New NotImplementedException();}

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

        Private Sub RetrieveValues(ByVal fieldObjects As PdfArray, ByVal values As IList(Of Field))
            For Each fieldObject As PdfDirectObject In fieldObjects
                Dim fieldReference As PdfReference = CType(fieldObject, PdfReference)
                Dim kidReferences As PdfArray = CType(CType(fieldReference.DataObject, PdfDictionary).Resolve(PdfName.Kids), PdfArray)
                Dim kidObject As PdfDictionary
                If (kidReferences Is Nothing) Then
                    kidObject = Nothing
                Else
                    kidObject = CType(CType(kidReferences(0), PdfReference).DataObject, PdfDictionary)
                End If
                ' Terminal field?
                '      If (kidObject == null // Merged single widget annotation.
                '|| (!kidObject.ContainsKey(PdfName.FT) // Multiple widget annotations.
                '  && kidObject.ContainsKey(PdfName.Subtype)
                '  && kidObject[PdfName.Subtype].Equals(PdfName.Widget)))
                If (kidObject Is Nothing OrElse
                   (Not kidObject.ContainsKey(PdfName.FT) AndAlso
                    kidObject.ContainsKey(PdfName.Subtype) AndAlso
                    kidObject(PdfName.Subtype).Equals(PdfName.Widget))
                    ) Then
                    values.Add(Field.Wrap(fieldReference))
                Else ' Non-terminal field.
                    RetrieveValues(kidReferences, values)
                End If
            Next
        End Sub

#End Region
#End Region
#End Region

    End Class

End Namespace
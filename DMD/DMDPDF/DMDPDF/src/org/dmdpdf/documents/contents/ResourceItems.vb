'/*
'  Copyright 2010-2012 Stefano Chizzolini. http://www.dmdpdf.org

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
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects

Imports System
Imports System.Collections
Imports System.Collections.Generic

Namespace DMD.org.dmdpdf.documents.contents

    '/**
    '  <summary>Collection of a specific resource type.</summary>
    '*/
    <PDF(VersionEnum.PDF10)>
    Public MustInherit Class ResourceItems(Of TValue As PdfObjectWrapper)
        Inherits PdfObjectWrapper(Of PdfDictionary)
        Implements IDictionary(Of PdfName, TValue)


#Region "dynamic"
#Region "constructors"

        Protected Sub New(ByVal context As Document)
            MyBase.New(context, New PdfDictionary())
        End Sub

        Friend Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "interface"
#Region "public"
        '/**
        '  Gets the key associated to a given value.
        '*/
        Public Function GetKey(ByVal value As TValue) As PdfName
            Return BaseDataObject.GetKey(value.BaseObject)
        End Function

#Region "IDictionary"

        Public Sub Add(ByVal key As PdfName, ByVal value As TValue) Implements IDictionary(Of PdfName, TValue).Add
            BaseDataObject.Add(key, value.BaseObject)
        End Sub

        Public Function ContainsKey(ByVal key As PdfName) As Boolean Implements IDictionary(Of PdfName, TValue).ContainsKey
            Return BaseDataObject.ContainsKey(key)
        End Function

        Public ReadOnly Property Keys As ICollection(Of PdfName) Implements IDictionary(Of PdfName, TValue).Keys
            Get
                Return BaseDataObject.Keys
            End Get
        End Property

        Public Function Remove(ByVal key As PdfName) As Boolean Implements IDictionary(Of PdfName, TValue).Remove
            Return BaseDataObject.Remove(key)
        End Function

        Default Public Property Item(ByVal key As PdfName) As TValue Implements IDictionary(Of PdfName, TValue).Item
            Get
                Return Wrap(BaseDataObject(key))
            End Get
            Set(ByVal value As TValue)
                BaseDataObject(key) = value.BaseObject
            End Set
        End Property

        Public Function TryGetValue(ByVal key As PdfName, ByRef value As TValue) As Boolean Implements IDictionary(Of PdfName, TValue).TryGetValue
            Dim ret As TValue = Me(key)
            Return (ret IsNot Nothing OrElse Me.ContainsKey(key))
        End Function

        Public ReadOnly Property Values As ICollection(Of TValue) Implements IDictionary(Of PdfName, TValue).Values
            Get
                Dim values_ As ICollection(Of TValue)
                ' Get the low-level objects!
                Dim valueObjects As ICollection(Of PdfDirectObject) = BaseDataObject.Values
                ' Populating the high-level collection...
                values_ = New List(Of TValue)(valueObjects.Count)
                For Each valueObject As PdfDirectObject In valueObjects
                    values_.Add(Wrap(valueObject))
                Next
                Return values_
            End Get
        End Property

#Region "ICollection"

        Private Sub Add(ByVal entry As KeyValuePair(Of PdfName, TValue)) Implements ICollection(Of KeyValuePair(Of PdfName, TValue)).Add
            Add(entry.Key, entry.Value)
        End Sub

        Public Sub Clear() Implements ICollection(Of KeyValuePair(Of PdfName, TValue)).Clear
            BaseDataObject.Clear()
        End Sub

        Private Function Contains(ByVal entry As KeyValuePair(Of PdfName, TValue)) As Boolean Implements ICollection(Of KeyValuePair(Of PdfName, TValue)).Contains
            Return entry.Value.BaseObject.Equals(BaseDataObject(entry.Key))
        End Function

        Public Sub CopyTo(ByVal entries As KeyValuePair(Of PdfName, TValue)(), ByVal index As Integer) Implements ICollection(Of KeyValuePair(Of PdfName, TValue)).CopyTo
            Throw New NotImplementedException()
        End Sub

        Public ReadOnly Property Count As Integer Implements ICollection(Of KeyValuePair(Of PdfName, TValue)).Count
            Get
                Return BaseDataObject.Count
            End Get
        End Property

        Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of KeyValuePair(Of PdfName, TValue)).IsReadOnly
            Get
                Return False
            End Get
        End Property

        Public Function Remove(ByVal entry As KeyValuePair(Of PdfName, TValue)) As Boolean Implements ICollection(Of KeyValuePair(Of PdfName, TValue)).Remove
            Return BaseDataObject.Remove(New KeyValuePair(Of PdfName, PdfDirectObject)(entry.Key, entry.Value.BaseObject))
        End Function



#Region "IEnumerable<KeyValuePair<PdfName,TValue>>"

        Private Class MyEnumerator '(Of PdfName, TValue)
            Implements IEnumerator(Of KeyValuePair(Of PdfName, TValue))

            Private o As ResourceItems(Of TValue)
            Private keys As PdfName()
            Private index As Integer

            Public Sub New(ByVal o As ResourceItems(Of TValue))
                Me.o = o
                Me.Reset()
            End Sub

            Public ReadOnly Property Current As KeyValuePair(Of PdfName, TValue) Implements IEnumerator(Of KeyValuePair(Of PdfName, TValue)).Current
                Get
                    Return New KeyValuePair(Of PdfName, TValue)(Me.keys(Me.index), Me.o(Me.keys(Me.index)))
                End Get
            End Property

            Private ReadOnly Property IEnumerator_Current As Object Implements IEnumerator.Current
                Get
                    Return Me.Current
                End Get
            End Property

            Public Sub Reset() Implements IEnumerator.Reset
                Me.index = -1
                Me.keys = o.Keys.ToArray
            End Sub

            Public Function MoveNext() As Boolean Implements IEnumerator.MoveNext
                If (Me.index + 1 < Me.keys.Length) Then
                    Me.index += 1
                    Return True
                Else
                    Return False
                End If
            End Function

            Public Sub Dispose() Implements IDisposable.Dispose
                Me.o = Nothing
                Me.keys = Nothing
                ' TODO: rimuovere il commento dalla riga seguente se è stato eseguito l'override di Finalize().
                ' GC.SuppressFinalize(Me)
            End Sub


        End Class

        Public Function GetEnumerator() As IEnumerator(Of KeyValuePair(Of PdfName, TValue)) Implements IEnumerable(Of KeyValuePair(Of PdfName, TValue)).GetEnumerator
            'For Each key As PdfName In Keys
            '    yield Return New KeyValuePair<PdfName, TValue > (key, this[key])
            '        Next
            Return New MyEnumerator(Me)
        End Function

#Region "IEnumerable"
        Private Function _GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            'return ((IEnumerable<KeyValuePair<PdfName,TValue>>)this).GetEnumerator();
            'Return ((IEnumerable)this).GetEnumerator();
            Return Me.GetEnumerator
        End Function

#End Region
#End Region
#End Region
#End Region
#End Region

#Region "protected"
        '/**
        '  <summary>Wraps a base object within its corresponding high-level representation.</summary>
        '*/
        Protected MustOverride Function Wrap(ByVal baseObject As PdfDirectObject) As TValue

#End Region
#End Region
#End Region

    End Class


End Namespace

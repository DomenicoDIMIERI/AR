'/*
'  Copyright 2009-2012 Stefano Chizzolini. http://www.dmdpdf.org

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

Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Linq

Namespace DMD.org.dmdpdf.util

    '/**
    '  <summary>Bidirectional bijective map.</summary>
    '*/
    Public Class BiDictionary(Of TKey, TValue)
        Implements IDictionary(Of TKey, TValue)

#Region "dynamic"
#Region "fields"

        Private ReadOnly dictionary As Dictionary(Of TKey, TValue)
        Private ReadOnly inverseDictionary As Dictionary(Of TValue, TKey)

#End Region

#Region "constructors"

        Public Sub New()
            Me.dictionary = New Dictionary(Of TKey, TValue)
            Me.inverseDictionary = New Dictionary(Of TValue, TKey)
        End Sub

        Public Sub New(ByVal capacity As Integer)
            Me.dictionary = New Dictionary(Of TKey, TValue)(capacity)
            Me.inverseDictionary = New Dictionary(Of TValue, TKey)(capacity)
        End Sub

        Public Sub New(ByVal dictionary As IDictionary(Of TKey, TValue))
            Me.dictionary = New Dictionary(Of TKey, TValue)(dictionary)
            'TODO: key duplicate collisions to resolve!
            '       inverseDictionary = this.dictionary.ToDictionary(entry => entry.Value, entry => entry.Key);
            inverseDictionary = New Dictionary(Of TValue, TKey)
            'for each (KeyValuePair < TKey, TValue > entry In this.dictionary)
            For Each entry As KeyValuePair(Of TKey, TValue) In Me.dictionary
                Me.inverseDictionary(entry.Value) = entry.Key
            Next
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public Function ContainsValue(ByVal value As TValue) As Boolean
            Return Me.inverseDictionary.ContainsKey(value)
        End Function

        Public Overridable Function GetKey(ByVal value As TValue) As TKey
            Dim key As TKey
            Me.inverseDictionary.TryGetValue(value, key)
            Return key
        End Function

#Region "IDictionary"

        Public Sub Add(ByVal key As TKey, ByVal value As TValue) Implements IDictionary(Of TKey, TValue).Add
            Me.dictionary.Add(key, value) ' Adds the entry.
            'Try
            inverseDictionary.Add(value, key) ' Adds the inverse entry.
            'Catch exception As System.Exception
            '    Me.dictionary.Remove(key) ' Reverts the entry addition.
            '    Throw 'exception
            'End Try
            Debug.Assert(Me.dictionary.Count = Me.inverseDictionary.Count)
        End Sub

        Public Function ContainsKey(ByVal key As TKey) As Boolean Implements IDictionary(Of TKey, TValue).ContainsKey
            Return Me.dictionary.ContainsKey(key)
        End Function


        Public ReadOnly Property Keys As ICollection(Of TKey) Implements IDictionary(Of TKey, TValue).Keys
            Get
                Return Me.dictionary.Keys
            End Get
        End Property

        Public Function Remove(ByVal key As TKey) As Boolean Implements IDictionary(Of TKey, TValue).Remove
            Dim value As TValue
            If (Not dictionary.TryGetValue(key, value)) Then Return False
            Me.dictionary.Remove(key)
            Me.inverseDictionary.Remove(value)
            'Debug.Assert(Me.dictionary.Count = Me.inverseDictionary.Count)
            Return True
        End Function

        Default Public Overridable Property Item(ByVal key As TKey) As TValue Implements IDictionary(Of TKey, TValue).Item
            Get
                '/*
                '  NOTE: This Is an intentional violation of the official .NET Framework Class Library
                '  prescription.
                '  My loose implementation emphasizes coding smoothness And concision, against ugly
                '  TryGetValue() invocations: unfound keys are happily dealt with returning a default (null)
                '  value.
                '  If the user Is interested in verifying whether such result represents a non-existing key
                '  Or an actual null object, it suffices to query ContainsKey() method.
                '*/
                Dim value As TValue
                Me.dictionary.TryGetValue(key, value)
                Return value
            End Get
            Set(ByVal value As TValue)
                Dim oldValue As TValue
                Me.dictionary.TryGetValue(key, oldValue)
                dictionary(key) = value '; // Sets the entry.
                If (oldValue IsNot Nothing) Then
                    Me.inverseDictionary.Remove(oldValue)
                End If
                Me.inverseDictionary(value) = key ' Sets the inverse entry.
                'Debug.Print(TypeName(Me) & ".setItem(" & key.ToString & ", " & value.ToString & ")")
                'Debug.Assert(Me.dictionary.Count = Me.inverseDictionary.Count)
            End Set
        End Property

        Public Function TryGetValue(ByVal key As TKey, ByRef value As TValue) As Boolean Implements IDictionary(Of TKey, TValue).TryGetValue
            Return Me.dictionary.TryGetValue(key, value)
        End Function

        Public ReadOnly Property Values As ICollection(Of TValue) Implements IDictionary(Of TKey, TValue).Values
            Get
                Return Me.dictionary.Values
            End Get
        End Property

#Region "ICollection"

        Public Sub CopyTo(array() As KeyValuePair(Of TKey, TValue), arrayIndex As Integer) Implements ICollection(Of KeyValuePair(Of TKey, TValue)).CopyTo
            Throw New NotImplementedException()
        End Sub

        Friend Sub Add(ByVal keyValuePair As KeyValuePair(Of TKey, TValue)) Implements ICollection(Of KeyValuePair(Of TKey, TValue)).Add
            Add(keyValuePair.Key, keyValuePair.Value)
        End Sub


        Public Sub Clear() Implements ICollection(Of KeyValuePair(Of TKey, TValue)).Clear
            Me.dictionary.Clear()
            Me.inverseDictionary.Clear()
        End Sub

        Friend Function Contains(ByVal keyValuePair As KeyValuePair(Of TKey, TValue)) As Boolean Implements ICollection(Of KeyValuePair(Of TKey, TValue)).Contains
            Return Me.dictionary.Contains(keyValuePair)
        End Function



        Public Overridable ReadOnly Property Count As Integer Implements ICollection(Of KeyValuePair(Of TKey, TValue)).Count
            Get
                Return Me.dictionary.Count
            End Get
        End Property

        Public Overridable ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of KeyValuePair(Of TKey, TValue)).IsReadOnly
            Get
                Return False
            End Get
        End Property

        Public Function Remove(ByVal keyValuePair As KeyValuePair(Of TKey, TValue)) As Boolean Implements ICollection(Of Generic.KeyValuePair(Of TKey, TValue)).Remove
            If (Not CType(dictionary, ICollection(Of KeyValuePair(Of TKey, TValue))).Remove(keyValuePair)) Then Return False
            Me.inverseDictionary.Remove(keyValuePair.Value)
            Return True
        End Function

#Region "IEnumerable<KeyValuePair<TKey,TValue>>"

        Public Function GetEnumerator() As IEnumerator(Of KeyValuePair(Of TKey, TValue)) Implements IEnumerable(Of KeyValuePair(Of TKey, TValue)).GetEnumerator ' IEnumerable<KeyValuePair<TKey,TValue>>.
            Return Me.dictionary.GetEnumerator()
        End Function

#Region "IEnumerable"

        Friend Function _GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
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

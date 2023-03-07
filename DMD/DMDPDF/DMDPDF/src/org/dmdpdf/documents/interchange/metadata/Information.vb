'/*
'  Copyright 2006-2012 Stefano Chizzolini. http://www.dmdpdf.org

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
Imports System.Reflection

Namespace DMD.org.dmdpdf.documents.interchange.metadata

    '/**
    '  <summary>Document information [PDF:1.6:10.2.1].</summary>
    '*/
    <PDF(VersionEnum.PDF10)>
    Public NotInheritable Class Information
        Inherits PdfObjectWrapper(Of PdfDictionary)
        Implements IDictionary(Of PdfName, Object)

#Region "Static"
#Region "Interface"
#Region "Public"

        Public Shared Function Wrap(ByVal baseObject As PdfDirectObject) As Information
            If (baseObject IsNot Nothing) Then
                Return New Information(baseObject)
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
            MyBase.New(context, New PdfDictionary())
        End Sub

        Private Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public Property Author As String
            Get
                Return CStr(Me(PdfName.Author))
            End Get
            Set(ByVal value As String)
                Me(PdfName.Author) = value
            End Set
        End Property

        Public Property CreationDate As DateTime?
            Get
                Return CType(Me(PdfName.CreationDate), DateTime?)
            End Get
            Set(ByVal value As DateTime?)
                Me(PdfName.CreationDate) = value
            End Set
        End Property

        Public Property Creator As String
            Get
                Return CStr(Me(PdfName.Creator))
            End Get
            Set(ByVal value As String)
                Me(PdfName.Creator) = value
            End Set
        End Property

        <PDF(VersionEnum.PDF11)>
        Public Property Keywords As String
            Get
                Return CStr(Me(PdfName.Keywords))
            End Get
            Set(ByVal value As String)
                Me(PdfName.Keywords) = value
            End Set
        End Property

        <PDF(VersionEnum.PDF11)>
        Public Property ModificationDate As DateTime?
            Get
                Return CType(Me(PdfName.ModDate), DateTime?)
            End Get
            Set(ByVal value As DateTime?)
                Me(PdfName.ModDate) = value
            End Set
        End Property

        Public Property Producer As String
            Get
                Return CStr(Me(PdfName.Producer))
            End Get
            Set(ByVal value As String)
                Me(PdfName.Producer) = value
            End Set
        End Property

        <PDF(VersionEnum.PDF11)>
        Public Property Subject As String
            Get
                Return CStr(Me(PdfName.Subject))
            End Get
            Set(ByVal value As String)
                Me(PdfName.Subject) = value
            End Set
        End Property

        <PDF(VersionEnum.PDF11)>
        Public Property Title As String
            Get
                Return CStr(Me(PdfName.Title))
            End Get
            Set(ByVal value As String)
                Me(PdfName.Title) = value
            End Set
        End Property

#Region "IDictionary"

        Public Sub Add(ByVal key As PdfName, ByVal value As Object) Implements IDictionary(Of PdfName, Object).Add
            OnChange(key)
            BaseDataObject.Add(key, PdfSimpleObject(Of Object).Get(value))
        End Sub

        Public Function ContainsKey(ByVal key As PdfName) As Boolean Implements IDictionary(Of PdfName, Object).ContainsKey
            Return BaseDataObject.ContainsKey(key)
        End Function

        Public ReadOnly Property Keys As ICollection(Of PdfName) Implements IDictionary(Of PdfName, Object).Keys
            Get
                Return BaseDataObject.Keys
            End Get
        End Property

        Public Function Remove(ByVal key As PdfName) As Boolean Implements IDictionary(Of objects.PdfName, Object).Remove
            OnChange(key)
            Return BaseDataObject.Remove(key)
        End Function

        Default Public Property Item(ByVal key As PdfName) As Object Implements IDictionary(Of PdfName, Object).Item
            Get
                Return PdfSimpleObject(Of Object).GetValue(BaseDataObject(key))
            End Get
            Set(ByVal value As Object)
                OnChange(key)
                BaseDataObject(key) = PdfSimpleObject(Of Object).Get(value)
            End Set
        End Property

        Public Function TryGetValue(ByVal key As PdfName, ByRef value As Object) As Boolean Implements IDictionary(Of objects.PdfName, Object).TryGetValue
            Dim valueObject As PdfDirectObject = Nothing
            If (BaseDataObject.TryGetValue(key, valueObject)) Then
                value = PdfSimpleObject(Of Object).GetValue(valueObject)
                Return True
            Else
                value = Nothing
            End If
            Return False
        End Function

        Public ReadOnly Property Values As ICollection(Of Object) Implements IDictionary(Of PdfName, Object).Values
            Get
                Dim _values As IList(Of Object) = New List(Of Object)
                For Each item As PdfDirectObject In BaseDataObject.Values
                    _values.Add(PdfSimpleObject(Of Object).GetValue(item))
                Next
                Return _values
            End Get
        End Property

#Region "ICollection"

        'void ICollection<KeyValuePair(of PdfName,Object)>.
        Private Sub _Add(ByVal entry As KeyValuePair(Of PdfName, Object)) Implements ICollection(Of Generic.KeyValuePair(Of PdfName, Object)).Add
            Me.Add(entry.Key, entry.Value)
        End Sub

        Public Sub Clear() Implements ICollection(Of KeyValuePair(Of PdfName, Object)).Clear
            BaseDataObject.Clear()
            ModificationDate = DateTime.Now
        End Sub

        'bool ICollection<KeyValuePair(Of PdfName,Object)>.Contains(
        Private Function _Contains(ByVal entry As KeyValuePair(Of PdfName, Object)) As Boolean Implements ICollection(Of KeyValuePair(Of PdfName, Object)).Contains
            Return entry.Value.Equals(Me(entry.Key))
        End Function

        Public Sub CopyTo(ByVal entries As KeyValuePair(Of PdfName, Object)(), ByVal index As Integer) Implements ICollection(Of KeyValuePair(Of PdfName, Object)).CopyTo
            Throw New NotImplementedException()
        End Sub

        Public ReadOnly Property Count As Integer Implements ICollection(Of KeyValuePair(Of PdfName, Object)).Count
            Get
                Return BaseDataObject.Count
            End Get
        End Property

        Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of KeyValuePair(Of PdfName, Object)).IsReadOnly
            Get
                Return False
            End Get
        End Property

        Public Function Remove(ByVal entry As KeyValuePair(Of PdfName, Object)) As Boolean Implements ICollection(Of KeyValuePair(Of PdfName, Object)).Remove
            Throw New NotImplementedException()
        End Function

#Region "IEnumerable<KeyValuePair<PdfName,Object>>"

        Private Class mEnumerator
            Implements IEnumerator(Of KeyValuePair(Of PdfName, Object))

            Private o As Information
            Private index As Integer
            Private values As System.Collections.ArrayList

            Public Sub New(ByVal o As Information)
                Me.o = o
                Me.Reset()
            End Sub

            Public ReadOnly Property Current As KeyValuePair(Of PdfName, Object) Implements IEnumerator(Of KeyValuePair(Of PdfName, Object)).Current
                Get
                    Dim value As KeyValuePair(Of PdfName, Object) = CType(Me.values(Me.index), KeyValuePair(Of PdfName, Object))
                    Debug.Print("Information.Current -> " & value.ToString)
                    Return value
                End Get
            End Property

            Private ReadOnly Property IEnumerator_Current As Object Implements IEnumerator.Current
                Get
                    Return Me.Current
                End Get
            End Property

            Public Sub Reset() Implements IEnumerator.Reset
                Me.index = -1
                Me.values = New System.Collections.ArrayList
                For Each entry As KeyValuePair(Of PdfName, PdfDirectObject) In o.BaseDataObject
                    Me.values.Add(New KeyValuePair(Of PdfName, Object)(entry.Key, PdfSimpleObject(Of Object).GetValue(entry.Value)))
                Next
            End Sub

            Public Function MoveNext() As Boolean Implements IEnumerator.MoveNext
                If (Me.index + 1 < Me.values.Count) Then
                    Me.index += 1
                    Return True
                Else
                    Return False
                End If
            End Function

            ' Questo codice viene aggiunto da Visual Basic per implementare in modo corretto il criterio Disposable.
            Public Sub Dispose() Implements IDisposable.Dispose
                Me.o = Nothing
                Me.values = Nothing
                ' GC.SuppressFinalize(Me)
            End Sub


        End Class


        Public Function GetEnumerator() As IEnumerator(Of KeyValuePair(Of PdfName, Object)) Implements IEnumerable(Of KeyValuePair(Of PdfName, Object)).GetEnumerator
            'IEnumerator<KeyValuePair<PdfName,Object>> IEnumerable<KeyValuePair<PdfName,Object>>.GetEnumerator(
            'For Each entry As KeyValuePair(Of PdfName, PdfDirectObject) In BaseDataObject
            '    yield return New KeyValuePair(of PdfName, Object)( entry.Key, PdfSimpleObject(of object).GetValue(entry.Value) )
            'Next
            Return New mEnumerator(Me)
        End Function

#Region "IEnumerable"
        'IEnumerator IEnumerable.GetEnumerator(
        '  )
        '{return ((IEnumerable<KeyValuePair<PdfName,object>>)this).GetEnumerator();}
        Private Function _GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Return Me.GetEnumerator
        End Function

#End Region
#End Region
#End Region
#End Region
#End Region

#Region "private"

        'TODO: Listen to baseDataObject's onChange notification?
        Private Sub OnChange(ByVal key As PdfName)
            If (Not BaseDataObject.Updated AndAlso Not PdfName.ModDate.Equals(key)) Then
                ModificationDate = DateTime.Now
            End If
        End Sub

#End Region
#End Region
#End Region

    End Class

End Namespace
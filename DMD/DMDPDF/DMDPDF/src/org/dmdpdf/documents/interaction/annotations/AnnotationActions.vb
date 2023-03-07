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

'  You should have received a copy of the GNU Lesser General Public License along with Me
'  Program (see README files); if not, go to the GNU website (http://www.gnu.org/licenses/).

'  Redistribution and use, with or without modification, are permitted provided that such
'  redistributions retain the above copyright notice, license and disclaimer, along with
'  Me list of conditions.
'*/

Imports DMD.org.dmdpdf.documents.interaction.actions
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects

Imports System
Imports System.Collections
Imports System.Collections.Generic

Namespace DMD.org.dmdpdf.documents.interaction.annotations

    '/**
    '  <summary>Annotation actions [PDF:1.6:8.5.2].</summary>
    '*/
    <PDF(VersionEnum.PDF12)>
    Public Class AnnotationActions
        Inherits PdfObjectWrapper(Of PdfDictionary)
        Implements IDictionary(Of PdfName, actions.Action)

#Region "dynamic"
#Region "fields"

        Private _parent As Annotation

#End Region

#Region "constructors"

        Public Sub New(ByVal parent As Annotation)
            MyBase.New(parent.Document, New PdfDictionary())
            Me._parent = parent
        End Sub

        Friend Sub New(ByVal parent As Annotation, ByVal BaseObject As PdfDirectObject)
            MyBase.New(BaseObject)
            Me._parent = parent
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public Overrides Function Clone(ByVal context As Document) As Object
            Throw New NotImplementedException() ' TODO: verify parent reference.
        End Function

        '/**
        '  <summary> Gets/Sets the action To be performed When the annotation Is activated.</summary>
        '*/
        Public Property OnActivate As actions.Action
            Get
                Return _parent.Action
            End Get
            Set(ByVal value As actions.Action)
                Me._parent.Action = value
            End Set
        End Property

        '/**
        '  <summary> Gets/Sets the action To be performed When the cursor enters the annotation's active area.</summary>
        '*/
        Public Property OnEnter As actions.Action
            Get
                Return Me(PdfName.E)
            End Get
            Set(ByVal value As actions.Action)
                Me(PdfName.E) = value
            End Set
        End Property

        '/**
        '  <summary> Gets/Sets the action To be performed When the cursor exits the annotation's active area.</summary>
        '*/
        Public Property OnExit As actions.Action
            Get
                Return Me(PdfName.X)
            End Get
            Set(ByVal value As actions.Action)
                Me(PdfName.X) = value
            End Set
        End Property

        '/**
        '  <summary> Gets/Sets the action To be performed When the mouse button Is pressed
        '  inside the annotation's active area.</summary>
        '*/
        Public Property OnMouseDown As actions.Action
            Get
                Return Me(PdfName.D)
            End Get
            Set(ByVal value As actions.Action)
                Me(PdfName.D) = value
            End Set
        End Property

        '/**
        '  <summary> Gets/Sets the action To be performed When the mouse button Is released
        '  inside the annotation's active area.</summary>
        '*/
        Public Property OnMouseUp As actions.Action
            Get
                Return Me(PdfName.U)
            End Get
            Set(ByVal value As actions.Action)
                Me(PdfName.U) = value
            End Set
        End Property

        '/**
        '  <summary> Gets/Sets the action To be performed When the page containing the annotation Is closed.</summary>
        '*/
        Public Property OnPageClose As actions.Action
            Get
                Return Me(PdfName.PC)
            End Get
            Set(ByVal value As actions.Action)
                Me(PdfName.PC) = value
            End Set
        End Property

        '/**
        '  <summary> Gets/Sets the action To be performed When the page containing the annotation
        '  Is no longer visible in the viewer application's user interface.</summary>
        '*/
        Public Property OnPageInvisible As actions.Action
            Get
                Return Me(PdfName.PI)
            End Get
            Set(ByVal value As actions.Action)
                Me(PdfName.PI) = value
            End Set
        End Property

        '/**
        '  <summary> Gets/Sets the action To be performed When the page containing the annotation Is opened.</summary>
        '*/
        Public Property OnPageOpen As actions.Action
            Get
                Return Me(PdfName.PO)
            End Get
            Set(ByVal value As actions.Action)
                Me(PdfName.PO) = value
            End Set
        End Property

        '/**
        '  <summary> Gets/Sets the action To be performed When the page containing the annotation
        '  becomes visible In the viewer application's user interface.</summary>
        '*/
        Public Property OnPageVisible As actions.Action
            Get
                Return Me(PdfName.PV)
            End Get
            Set(ByVal value As actions.Action)
                Me(PdfName.PV) = value
            End Set
        End Property

#Region "IDictionary"

        Public Sub Add(ByVal key As PdfName, ByVal value As actions.Action) Implements IDictionary(Of PdfName, actions.Action).Add
            Me.BaseDataObject.Add(key, value.BaseObject)
        End Sub

        Public Function ContainsKey(ByVal key As PdfName) As Boolean Implements IDictionary(Of PdfName, actions.Action).ContainsKey
            Return BaseDataObject.ContainsKey(key) OrElse
                   (PdfName.A.Equals(key) AndAlso _parent.BaseDataObject.ContainsKey(key))
        End Function

        Public ReadOnly Property Keys As ICollection(Of PdfName) Implements IDictionary(Of PdfName, actions.Action).Keys
            Get
                Return BaseDataObject.Keys
            End Get
        End Property

        Public Function Remove(ByVal key As PdfName) As Boolean Implements IDictionary(Of PdfName, actions.Action).Remove
            If (PdfName.A.Equals(key) AndAlso _parent.BaseDataObject.ContainsKey(key)) Then
                Me.OnActivate = Nothing
                Return True
            Else
                Return BaseDataObject.Remove(key)
            End If
        End Function

        Default Public Property Item(ByVal key As PdfName) As actions.Action Implements IDictionary(Of PdfName, actions.Action).Item
            Get
                Return actions.Action.Wrap(BaseDataObject(key))
            End Get
            Set(ByVal value As actions.Action)
                If (value IsNot Nothing) Then
                    BaseDataObject(key) = value.BaseObject
                Else
                    BaseDataObject(key) = Nothing
                End If
            End Set
        End Property

        Public Function TryGetValue(ByVal key As PdfName, ByRef value As actions.Action) As Boolean Implements IDictionary(Of PdfName, actions.Action).TryGetValue
            value = Me(key)
            If (value Is Nothing) Then
                Return ContainsKey(key)
            Else
                Return True
            End If
        End Function


        Public ReadOnly Property Values As ICollection(Of actions.Action) Implements IDictionary(Of PdfName, actions.Action).Values
            Get
                Dim _values As List(Of actions.Action)
                '{
                Dim objs As ICollection(Of PdfDirectObject) = BaseDataObject.Values
                _values = New List(Of actions.Action)(objs.Count)
                For Each obj As PdfDirectObject In objs
                    _values.Add(actions.Action.Wrap(obj))
                Next
                Dim action As actions.Action = Me.OnActivate
                If (action IsNot Nothing) Then
                    _values.Add(action)
                End If
                '}
                Return _values
            End Get
        End Property

#Region "ICollection"

        Private Sub _Add(ByVal entry As KeyValuePair(Of PdfName, actions.Action)) Implements ICollection(Of KeyValuePair(Of PdfName, actions.Action)).Add
            Me.Add(entry.Key, entry.Value)
        End Sub

        Public Sub Clear() Implements ICollection(Of KeyValuePair(Of PdfName, actions.Action)).Clear
            BaseDataObject.Clear()
            OnActivate = Nothing
        End Sub

        Private Function _Contains(ByVal entry As KeyValuePair(Of PdfName, actions.Action)) As Boolean Implements ICollection(Of KeyValuePair(Of PdfName, actions.Action)).Contains
            Return entry.Value.BaseObject.Equals(BaseDataObject(entry.Key))
        End Function

        Public Sub CopyTo(ByVal entries As KeyValuePair(Of PdfName, actions.Action)(), ByVal index As Integer) Implements ICollection(Of KeyValuePair(Of PdfName, actions.Action)).CopyTo
            Throw New NotImplementedException
        End Sub

        Public ReadOnly Property Count As Integer Implements ICollection(Of KeyValuePair(Of PdfName, actions.Action)).Count
            Get
                If (_parent.BaseDataObject.ContainsKey(PdfName.A)) Then
                    Return BaseDataObject.Count + 1
                Else
                    Return BaseDataObject.Count + 0
                End If
            End Get
        End Property

        Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of KeyValuePair(Of PdfName, actions.Action)).IsReadOnly
            Get
                Return False
            End Get
        End Property

        Public Function Remove(ByVal entry As KeyValuePair(Of PdfName, actions.Action)) As Boolean Implements ICollection(Of KeyValuePair(Of PdfName, actions.Action)).Remove
            Return BaseDataObject.Remove(New KeyValuePair(Of PdfName, PdfDirectObject)(entry.Key, entry.Value.BaseObject))
        End Function

#Region "IEnumerable<KeyValuePair(of PdfName,Action)>"
        'IEnumerator<KeyValuePair(of PdfName,Action)> IEnumerable<KeyValuePair(of PdfName,Action)>.GetEnumerator(
        '  )
        '{
        '  foreach(byval key as PdfName  in Keys)
        '  {yield return New KeyValuePair(of PdfName,Action)(key,Me(key));}
        '}

        Private Class mEnumerator
            Implements IEnumerator(Of KeyValuePair(Of PdfName, actions.Action))


            Public o As AnnotationActions
            Public keys As ICollection(Of PdfName)
            Public index As Integer

            Public Sub New(ByVal o As AnnotationActions)
                Me.o = o
                Me.Reset()
            End Sub


            Public ReadOnly Property Current As KeyValuePair(Of PdfName, actions.Action) Implements IEnumerator(Of KeyValuePair(Of PdfName, actions.Action)).Current
                Get
                    Dim key As PdfName = Me.keys(Me.index)
                    Return New KeyValuePair(Of PdfName, actions.Action)(key, Me.o(key))
                End Get
            End Property

            Private ReadOnly Property IEnumerator_Current As Object Implements IEnumerator.Current
                Get
                    Return Me.Current
                End Get
            End Property

            Public Sub Reset() Implements IEnumerator.Reset
                Me.keys = Me.o.Keys
                Me.index = -1
            End Sub

            Public Function MoveNext() As Boolean Implements IEnumerator.MoveNext
                If (Me.index + 1 < Me.keys.Count) Then
                    Me.index += 1
                    Return True
                Else
                    Return False
                End If
            End Function

            ' Questo codice viene aggiunto da Visual Basic per implementare in modo corretto il criterio Disposable.
            Public Sub Dispose() Implements IDisposable.Dispose
                Me.o = Nothing
                Me.keys = Nothing
                ' TODO: rimuovere il commento dalla riga seguente se è stato eseguito l'override di Finalize().
                ' GC.SuppressFinalize(Me)
            End Sub

        End Class

        Public Function GetEnumerator() As IEnumerator(Of KeyValuePair(Of PdfName, actions.Action)) Implements IEnumerable(Of KeyValuePair(Of PdfName, actions.Action)).GetEnumerator
            Return New mEnumerator(Me)
        End Function

#Region "IEnumerable"
        Private Function _GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Return Me._GetEnumerator
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
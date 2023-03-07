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
Imports DMD.org.dmdpdf.documents.contents.xObjects
Imports DMD.org.dmdpdf.objects

Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.IO

Namespace DMD.org.dmdpdf.documents.interaction.annotations

    '/**
    '  <summary>Appearance states [PDF:1.6:8.4.4].</summary>
    '*/
    <PDF(VersionEnum.PDF12)>
    Public NotInheritable Class AppearanceStates
        Inherits PdfObjectWrapper(Of PdfDataObject)
        Implements IDictionary(Of PdfName, FormXObject)

#Region "dynamic"
#Region "fields"

        Private _appearance As Appearance

        Private _statesKey As PdfName

#End Region

#Region "constructors"

        Friend Sub New(ByVal statesKey As PdfName, ByVal appearance As Appearance)
            MyBase.New(appearance.BaseDataObject(statesKey))
            Me._appearance = appearance
            Me._statesKey = statesKey
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary>Gets the appearance associated To these states.</summary>
        '*/
        Public ReadOnly Property Appearance As Appearance
            Get
                Return Me._appearance
            End Get
        End Property

        Public Overrides Function Clone(ByVal context As Document) As Object
            Throw New NotImplementedException() ' TODO: verify Appearance reference.
        End Function

        '//TODO
        '  /**
        '    Gets the key associated to a given value.
        '  */
        '//   public PdfName GetKey(
        '//     FormXObject value
        '//     )
        '//   {return BaseDataObject.GetKey(value.BaseObject);}

#Region "IDictionary"

        Public Sub Add(ByVal key As PdfName, ByVal value As FormXObject) Implements IDictionary(Of PdfName, FormXObject).Add
            EnsureDictionary()(key) = value.BaseObject
        End Sub

        Public Function ContainsKey(ByVal key As PdfName) As Boolean Implements IDictionary(Of PdfName, FormXObject).ContainsKey
            Dim baseDataObject As PdfDataObject = Me.BaseDataObject
            If (baseDataObject Is Nothing) Then ' No state. 
                Return False
            ElseIf (TypeOf (baseDataObject) Is PdfStream) Then '// Single Then state.
                Return (key Is Nothing)
            Else ' Multiple state.
                Return CType(BaseDataObject, PdfDictionary).ContainsKey(key)
            End If
        End Function

        Public ReadOnly Property Keys As ICollection(Of PdfName) Implements IDictionary(Of PdfName, FormXObject).Keys
            Get
                Throw New NotImplementedException()
            End Get
        End Property

        Public Function Remove(ByVal key As PdfName) As Boolean Implements IDictionary(Of PdfName, FormXObject).Remove
            Dim baseDataObject As PdfDataObject = Me.BaseDataObject
            If (baseDataObject Is Nothing) Then ' No Then state.
                Return False
            ElseIf (TypeOf (baseDataObject) Is PdfStream) Then ' Single Then state.
                If (key Is Nothing) Then
                    BaseObject = Nothing
                    _appearance.BaseDataObject.Remove(_statesKey)
                    Return True
                Else ' Invalid key.
                    Return False
                End If
            Else ' Multiple state.
                Return CType(baseDataObject, PdfDictionary).Remove(key)
            End If
        End Function

        Default Public Property Item(ByVal key As PdfName) As FormXObject Implements IDictionary(Of PdfName, FormXObject).Item
            Get
                Dim baseDataObject As PdfDataObject = Me.BaseDataObject
                If (baseDataObject Is Nothing) Then ' No Then state.
                    Return Nothing
                ElseIf (key Is Nothing) Then
                    If (TypeOf (baseDataObject) Is PdfStream) Then 'Single Then state.
                        Return FormXObject.Wrap(BaseObject)
                    Else ' Multiple state, but invalid key.
                        Return Nothing
                    End If
                Else ' Multiple state.
                    Return FormXObject.Wrap(CType(baseDataObject, PdfDictionary)(key))
                End If
            End Get
            Set(ByVal value As FormXObject)
                If (key Is Nothing) Then ' Single Then state.
                    BaseObject = value.BaseObject
                    _appearance.BaseDataObject(_statesKey) = BaseObject
                Else ' Multiple state.
                    EnsureDictionary()(key) = value.BaseObject
                End If
            End Set
        End Property

        Public Function TryGetValue(ByVal key As PdfName, ByRef value As FormXObject) As Boolean Implements IDictionary(Of objects.PdfName, FormXObject).TryGetValue
            value = Me(key)
            Return (value IsNot Nothing OrElse ContainsKey(key))
        End Function

        Public ReadOnly Property Values As ICollection(Of FormXObject) Implements IDictionary(Of PdfName, FormXObject).Values
            Get
                Throw New NotImplementedException()
            End Get
        End Property

#Region "ICollection"

        Private Sub _Add(ByVal entry As KeyValuePair(Of PdfName, FormXObject)) Implements ICollection(Of KeyValuePair(Of PdfName, FormXObject)).Add
            Me.Add(entry.Key, entry.Value)
        End Sub

        Public Sub Clear() Implements ICollection(Of KeyValuePair(Of PdfName, FormXObject)).Clear
            EnsureDictionary().Clear()
        End Sub

        Private Function _Contains(ByVal entry As KeyValuePair(Of PdfName, FormXObject)) As Boolean Implements ICollection(Of KeyValuePair(Of PdfName, FormXObject)).Contains
            Dim baseDataObject As PdfDataObject = Me.BaseDataObject
            If (baseDataObject Is Nothing) Then ' NoThen state.
                Return False
            ElseIf (TypeOf (baseDataObject) Is PdfStream) Then '/ Single Then state.
                Return CType(entry.Value, FormXObject).BaseObject.Equals(BaseObject)
            Else ' Multiple state.
                Return entry.Value.Equals(Me(entry.Key))
            End If
        End Function

        Public Sub CopyTo(ByVal entries As KeyValuePair(Of PdfName, FormXObject)(), ByVal index As Integer) Implements ICollection(Of KeyValuePair(Of PdfName, FormXObject)).CopyTo
            Throw New NotImplementedException()
        End Sub

        Public ReadOnly Property Count As Integer Implements ICollection(Of KeyValuePair(Of PdfName, FormXObject)).Count
            Get
                Dim baseDataObject As PdfDataObject = Me.BaseDataObject
                If (baseDataObject Is Nothing) Then ' NoThen state.
                    Return 0
                ElseIf (TypeOf (baseDataObject) Is PdfStream) Then 'Single Then state.
                    Return 1
                Else ' Multiple state.
                    Return CType(baseDataObject, PdfDictionary).Count
                End If
            End Get
        End Property

        Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of KeyValuePair(Of PdfName, FormXObject)).IsReadOnly
            Get
                Return False
            End Get
        End Property

        Public Function Remove(ByVal entry As KeyValuePair(Of PdfName, FormXObject)) As Boolean Implements ICollection(Of KeyValuePair(Of PdfName, FormXObject)).Remove
            Throw New NotImplementedException()
        End Function

#Region "IEnumerable<KeyValuePair<PdfName,FormXObject>>"

        '    IEnumerator<KeyValuePair<PdfName,FormXObject>> IEnumerable<KeyValuePair<PdfName,FormXObject>>.GetEnumerator(
        '  )
        '{
        '  PdfDataObject baseDataObject = BaseDataObject;
        '  if(baseDataObject == null) // No state.
        '  { /* NOOP. */ }
        '  else if(baseDataObject is PdfStream) // Single state.
        '  {
        '    yield return new KeyValuePair<PdfName,FormXObject>(
        '      null,
        '      FormXObject.Wrap(BaseObject)
        '      );
        '  }
        '  else // Multiple state.
        '  {
        '    foreach(KeyValuePair<PdfName,PdfDirectObject> entry in ((PdfDictionary)baseDataObject))
        '    {
        '      yield return New KeyValuePair<PdfName,FormXObject>(
        '        entry.Key,
        '        FormXObject.Wrap(entry.Value)
        '        );
        '    }
        '  }
        '}

        Private Class mEnumerator
            Implements IEnumerator(Of KeyValuePair(Of PdfName, FormXObject))


            Public o As AppearanceStates
            Private values As KeyValuePair(Of PdfName, FormXObject)()
            Private index As Integer
            Private BaseObject As PdfDirectObject

            Public Sub New(ByVal o As AppearanceStates)
                Me.o = o
                Me.Reset()
            End Sub

            Public ReadOnly Property Current As KeyValuePair(Of PdfName, FormXObject) Implements IEnumerator(Of KeyValuePair(Of PdfName, FormXObject)).Current
                Get
                    Debug.Print("AppearanceStates.Current ->  " & Me.values(Me.index).ToString)
                    Return Me.values(Me.index)
                End Get
            End Property

            Private ReadOnly Property IEnumerator_Current As Object Implements IEnumerator.Current
                Get
                    Return Me.Current
                End Get
            End Property

            Public Sub Reset() Implements IEnumerator.Reset
                Dim baseDataObject As PdfDataObject = Me.o.BaseDataObject
                Me.BaseObject = o.BaseObject
                If (baseDataObject Is Nothing) Then ' No state.
                    '  { /* NOOP. */ }
                    Me.values = {}
                ElseIf (TypeOf (baseDataObject) Is PdfStream) Then ' Single state.
                    Me.values = {New KeyValuePair(Of PdfName, FormXObject)(Nothing, FormXObject.Wrap(BaseObject))}
                Else ' Multiple state.
                    Dim pdir As PdfDictionary = CType(Me.o.BaseDataObject, PdfDictionary)
                    If (pdir.Count > 0) Then
                        Me.values = New KeyValuePair(Of PdfName, FormXObject)(pdir.Count - 1) {}
                    Else
                        Me.values = {}
                    End If
                    Dim i As Integer = 0
                    For Each entry As KeyValuePair(Of PdfName, PdfDirectObject) In pdir
                        Me.values(i) = New KeyValuePair(Of PdfName, FormXObject)(entry.Key, FormXObject.Wrap(entry.Value))
                        i += 1
                    Next
                End If
                Me.index = -1
            End Sub

            Public Function MoveNext() As Boolean Implements IEnumerator.MoveNext
                If (Me.index + 1 < Me.values.Length) Then
                    Me.index += 1
                    Return True
                Else
                    Return False
                End If
            End Function

            ' TODO: eseguire l'override di Finalize() solo se Dispose(disposing As Boolean) include il codice per liberare risorse non gestite.
            'Protected Overrides Sub Finalize()
            '    ' Non modificare questo codice. Inserire sopra il codice di pulizia in Dispose(disposing As Boolean).
            '    Dispose(False)
            '    MyBase.Finalize()
            'End Sub

            ' Questo codice viene aggiunto da Visual Basic per implementare in modo corretto il criterio Disposable.
            Public Sub Dispose() Implements IDisposable.Dispose
                Me.o = Nothing
                Me.values = Nothing
                Me.BaseObject = Nothing
                ' TODO: rimuovere il commento dalla riga seguente se è stato eseguito l'override di Finalize().
                ' GC.SuppressFinalize(Me)
            End Sub

        End Class

        Public Function GetEnumerator() As IEnumerator(Of KeyValuePair(Of PdfName, FormXObject)) Implements IEnumerable(Of KeyValuePair(Of PdfName, FormXObject)).GetEnumerator
            Return New mEnumerator(Me)
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
#End Region

#Region "Private"

        Private Function EnsureDictionary() As PdfDictionary
            Dim baseDataObject As PdfDataObject = Me.BaseDataObject
            If (Not (TypeOf (baseDataObject) Is PdfDictionary)) Then
                '/*
                '  NOTE: Single states are erased as they have no valid key
                '  to be consistently integrated within the dictionary.
                '*/
                baseDataObject = New PdfDictionary()
                BaseObject = CType(baseDataObject, PdfDirectObject)
                _appearance.BaseDataObject(_statesKey) = CType(baseDataObject, PdfDictionary)
            End If
            Return CType(baseDataObject, PdfDictionary)
        End Function

#End Region
#End Region

    End Class

End Namespace
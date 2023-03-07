'/*
'  Copyright 2006 - 2012 Stefano Chizzolini. http: //www.pdfclown.org

'  Contributors:
'    * Stefano Chizzolini (original code developer, http//www.stefanochizzolini.it)

'  Me file should be part Of the source code distribution Of "PDF Clown library" (the
'  Program): see the accompanying README files For more info.

'  Me Program Is free software; you can redistribute it And/Or modify it under the terms
'  of the GNU Lesser General Public License as published by the Free Software Foundation;
'  either version 3 Of the License, Or (at your Option) any later version.

'  Me Program Is distributed In the hope that it will be useful, but WITHOUT ANY WARRANTY,
'  either expressed Or implied; without even the implied warranty Of MERCHANTABILITY Or
'  FITNESS FOR A PARTICULAR PURPOSE. See the License for more details.

'  You should have received a copy Of the GNU Lesser General Public License along With Me
'  Program(see README files); If Not, go To the GNU website (http://www.gnu.org/licenses/).

'  Redistribution And use, with Or without modification, are permitted provided that such
'  redistributions retain the above copyright notice, license And disclaimer, along With
'  Me list Of conditions.
'*/

Imports DMD.org.dmdpdf.bytes
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.tokens

Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports text = System.Text

Namespace DMD.org.dmdpdf.objects

    '/**
    '  <summary> PDF dictionary Object [PDF:1.6:3.2.6].</summary>
    '*/
    Public NotInheritable Class PdfDictionary
        Inherits PdfDirectObject
        Implements IDictionary(Of PdfName, PdfDirectObject)

#Region "shared"
#Region "fields"

        Private Shared ReadOnly _BeginDictionaryChunk As Byte() = Encoding.Pdf.Encode(Keyword.BeginDictionary)
        Private Shared ReadOnly _EndDictionaryChunk As Byte() = Encoding.Pdf.Encode(Keyword.EndDictionary)

#End Region
#End Region

#Region "dynamic"
#Region "fields"

        Friend _entries As IDictionary(Of PdfName, PdfDirectObject)

        Private _parent As PdfObject
        Private _updateable As Boolean = True
        Private _updated As Boolean
        Private _virtual_ As Boolean

#End Region

#Region "constructors"

        Public Sub New()
            _entries = New Dictionary(Of PdfName, PdfDirectObject)()
        End Sub

        Public Sub New(ByVal capacity As Integer)
            _entries = New Dictionary(Of PdfName, PdfDirectObject)(capacity)
        End Sub

        Public Sub New(ByVal keys As PdfName(), ByVal values As PdfDirectObject())
            Me.New(values.Length)
            Updateable = False
            For index As Integer = 0 To values.Length - 1
                Me(keys(index)) = values(index)
            Next
            Updateable = True
        End Sub

        Public Sub New(ByVal entries As IDictionary(Of PdfName, PdfDirectObject))
            Me.New(entries.Count)
            Updateable = False
            For Each entry As KeyValuePair(Of PdfName, PdfDirectObject) In entries
                Me(entry.Key) = CType(Include(entry.Value), PdfDirectObject)
            Next
            Updateable = True
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public Overrides Function Accept(ByVal visitor As IVisitor, ByVal data As Object) As PdfObject
            Return visitor.Visit(Me, data)
        End Function

        Public Overrides Function CompareTo(ByVal obj As PdfDirectObject) As Integer
            Throw New NotImplementedException()
        End Function

        '/**
        '  <summary> Gets the value corresponding To the given key, forcing its instantiation As a direct
        '  Object in case of missing entry.</summary>
        '  <param name = "key" > key whose associated value Is To be returned.</param>
        '*/
        Public Function [Get](Of T As PdfDataObject)(ByVal key As PdfName) As PdfDirectObject  'PdfDataObject, New()
            Return [Get](Of T)(key, True)
        End Function

        '/**
        '  <summary> Gets the value corresponding To the given key, forcing its instantiation In Case Of
        '  missing entry.</summary>
        '  <param name = "key" > key whose associated value Is To be returned.</param>
        '  <param name = "direct" > Whether the item has To be instantiated directly within its container
        '  instead of being referenced through an indirect object.</param>
        '*/
        Public Function [Get](Of T As PdfDataObject)(ByVal key As PdfName, ByVal direct As Boolean) As PdfDirectObject  ' where T : PdfDataObject, New()
            Dim value As PdfDirectObject = Me(key)
            If (value Is Nothing) Then
                '/*
                '  NOTE: The null - Object placeholder MUST Not perturb the existing Structure; therefore: 
                '    - it MUST be marked as virtual in order Not to unnecessarily serialize it;
                '    - it MUST be put into Me dictionary without affecting its update status.
                '*/
                Try
                    If (direct) Then
                        value = CType(Include(CType(NewT(Of T)(), PdfDataObject)), PdfDirectObject)
                    Else
                        value = CType(Include(New PdfIndirectObject(File, NewT(Of T)(), New XRefEntry(0, 0)).Reference), PdfDirectObject)
                    End If
                    _entries(key) = value
                    value.Virtual = True
                Catch e As System.Exception
                    Throw New System.Exception(GetType(T).Name & " failed to instantiate.", e)
                End Try
            End If

            Return value
        End Function

        Public Overrides Function Equals(ByVal [object] As Object) As Boolean
            Return MyBase.Equals([object]) OrElse
                            ([object] IsNot Nothing AndAlso
                             [object].GetType().Equals(Me.GetType) AndAlso
                             CType([object], PdfDictionary)._entries.Equals(_entries))
        End Function

        Public Overrides Function GetHashCode() As Integer
            Return _entries.GetHashCode()
        End Function

        '/**
        '  Gets the key associated To the specified value.
        '*/
        Public Function GetKey(ByVal value As PdfDirectObject) As PdfName
            '/*
            '  NOTE:       Current PdfDictionary implementation doesn't support bidirectional maps, to say that
            '              the only currently-available way To retrieve a key from a value Is To iterate the whole map
            '  (really poor performance!).
            '*/
            For Each entry As KeyValuePair(Of PdfName, PdfDirectObject) In _entries
                If (entry.Value.Equals(value)) Then Return entry.Key
            Next
            Return Nothing
        End Function

        Public Overrides ReadOnly Property Parent As PdfObject
            Get
                Return _parent
            End Get
        End Property

        Friend Overrides Sub SetParent(value As PdfObject)
            _parent = value
        End Sub

        '/**
        '  <summary> Gets the dereferenced value corresponding To the given key.</summary>
        '  <remarks> Me method takes care To resolve the value returned by <see cref="Me[PdfName]">
        '  Me[PdfName]</see>.</remarks>
        '  <param name = "key" > key whose associated value Is To be returned.</param>
        '  <returns> null, if the map contains no mapping For Me key.</returns>
        '*/
        Public Shadows Function Resolve(ByVal key As PdfName) As PdfDataObject
            Return MyBase.Resolve(Me(key))
        End Function

        '/**
        '  <summary> Gets the dereferenced value corresponding To the given key, forcing its instantiation
        '  in case of missing entry.</summary>
        '  <remarks> Me method takes care To resolve the value returned by <see cref="Get(PdfName)"/>.
        '  </remarks>
        '  <param name = "key" > key whose associated value Is To be returned.</param>
        '  <returns> null, if the map contains no mapping For Me key.</returns>
        '*/
        Public Shadows Function Resolve(Of T As PdfDataObject)(ByVal key As PdfName) As T ' where T : PdfDataObject, New()
            Return CType(MyBase.Resolve([Get](Of T)(key)), T)
        End Function

        Public Overrides Function Swap(ByVal other As PdfObject) As PdfObject
            Dim otherDictionary As PdfDictionary = CType(other, PdfDictionary)
            Dim otherEntries As IDictionary(Of PdfName, PdfDirectObject) = otherDictionary._entries
            ' Update the other!
            otherDictionary._entries = Me._entries
            otherDictionary.Update()
            ' Update Me one!
            Me._entries = otherEntries
            Me.Update()
            Return Me
        End Function

        Public Overrides Function ToString() As String
            Dim buffer As New text.StringBuilder()
            '{
            ' Begin.
            buffer.Append("<< ")
            ' Entries.
            For Each entry As KeyValuePair(Of PdfName, PdfDirectObject) In _entries
                ' Entry...
                ' ...key.
                buffer.Append(entry.Key.ToString()).Append(" ")
                ' ...value.
                buffer.Append(PdfDirectObject.ToString(entry.Value)).Append(" ")
            Next
            ' End.
            buffer.Append(">>")
            '}
            Return buffer.ToString()
        End Function

        Public Overrides Property Updateable As Boolean
            Get
                Return _updateable
            End Get
            Set(ByVal value As Boolean)
                _updateable = value
            End Set
        End Property

        Public Overrides ReadOnly Property Updated As Boolean
            Get
                Return _updated
            End Get
        End Property

        Protected Friend Overrides Sub SetUpdated(value As Boolean)
            _updated = value
        End Sub

        Public Overrides Sub WriteTo(ByVal stream As IOutputStream, ByVal context As File)
            ' Begin.
            stream.Write(_BeginDictionaryChunk)
            ' Entries.
            For Each entry As KeyValuePair(Of PdfName, PdfDirectObject) In _entries
                Dim value As PdfDirectObject = entry.Value
                If (value IsNot Nothing AndAlso value.Virtual) Then
                    Continue For
                End If
                ' Entry...
                ' ...key.
                entry.Key.WriteTo(stream, context)
                stream.Write(Chunk.Space)
                ' ...value.
                PdfDirectObject.WriteTo(stream, context, value)
                stream.Write(Chunk.Space)
            Next
            ' End.
            stream.Write(_EndDictionaryChunk)
        End Sub

#Region "IDictionary"

        Public Sub Add(ByVal key As PdfName, ByVal value As PdfDirectObject) Implements IDictionary(Of PdfName, PdfDirectObject).Add
            _entries.Add(key, CType(Include(value), PdfDirectObject))
            Update()
        End Sub

        Public Function ContainsKey(ByVal key As PdfName) As Boolean Implements IDictionary(Of PdfName, PdfDirectObject).ContainsKey
            Return _entries.ContainsKey(key)
        End Function

        Public ReadOnly Property Keys As ICollection(Of PdfName) Implements IDictionary(Of PdfName, PdfDirectObject).Keys
            Get
                Return _entries.Keys
            End Get
        End Property

        Public Function Remove(ByVal key As PdfName) As Boolean Implements IDictionary(Of PdfName, PdfDirectObject).Remove
            Dim oldValue As PdfDirectObject = Me(key)
            If (_entries.Remove(key)) Then
                Exclude(oldValue)
                Update()
                Return True
            End If
            Return False
        End Function

        Default Public Property Item(ByVal key As PdfName) As PdfDirectObject Implements IDictionary(Of PdfName, PdfDirectObject).Item
            Get
                '/*
                '  NOTE: Me Is an intentional violation of the official .NET Framework Class
                '  Library prescription(no exception Is thrown anytime a key Is Not found --
                '  a null pointer Is returned instead).
                '*/
                Dim value As PdfDirectObject = Nothing
                _entries.TryGetValue(key, value)
                Return value
            End Get
            Set(ByVal value As PdfDirectObject)
                If (value Is Nothing) Then
                    Remove(key)
                Else
                    Dim oldValue As PdfDirectObject = Me(key)
                    _entries(key) = CType(Include(value), PdfDirectObject)
                    Exclude(oldValue)
                    Update()
                End If
            End Set
        End Property

        Public Function TryGetValue(ByVal key As PdfName, ByRef value As PdfDirectObject) As Boolean Implements IDictionary(Of PdfName, PdfDirectObject).TryGetValue
            Return _entries.TryGetValue(key, value)
        End Function

        Public ReadOnly Property Values As ICollection(Of PdfDirectObject) Implements IDictionary(Of PdfName, PdfDirectObject).Values
            Get
                Return _entries.Values
            End Get
        End Property

#Region "ICollection"

        Private Sub _Add(ByVal entry As KeyValuePair(Of PdfName, PdfDirectObject)) Implements ICollection(Of KeyValuePair(Of PdfName, PdfDirectObject)).Add
            Add(entry.Key, entry.Value)
        End Sub

        Public Sub Clear() Implements ICollection(Of KeyValuePair(Of PdfName, PdfDirectObject)).Clear
            For Each key As PdfName In New List(Of PdfDirectObject)(_entries.Keys)
                Remove(key)
            Next
        End Sub

        Private Function _Contains(ByVal entry As KeyValuePair(Of PdfName, PdfDirectObject)) As Boolean Implements ICollection(Of KeyValuePair(Of PdfName, PdfDirectObject)).Contains
            Return CType(_entries, ICollection(Of KeyValuePair(Of PdfName, PdfDirectObject))).Contains(entry)
        End Function


        Public Sub CopyTo(ByVal entries As KeyValuePair(Of PdfName, PdfDirectObject)(), ByVal index As Integer) Implements ICollection(Of KeyValuePair(Of PdfName, PdfDirectObject)).CopyTo
            Throw New NotImplementedException()
        End Sub

        Public ReadOnly Property Count As Integer Implements ICollection(Of KeyValuePair(Of PdfName, PdfDirectObject)).Count
            Get
                Return _entries.Count
            End Get
        End Property

        Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of KeyValuePair(Of PdfName, PdfDirectObject)).IsReadOnly
            Get
                Return False
            End Get
        End Property

        Public Function Remove(ByVal entry As KeyValuePair(Of PdfName, PdfDirectObject)) As Boolean Implements ICollection(Of KeyValuePair(Of PdfName, PdfDirectObject)).Remove
            If (entry.Value.Equals(Me(entry.Key))) Then
                Return Remove(entry.Key)
            Else
                Return False
            End If
        End Function

#Region "IEnumerable<KeyValuePair(Of PdfName,PdfDirectObject)>"

        Public Function GetEnumerator() As IEnumerator(Of KeyValuePair(Of PdfName, PdfDirectObject)) Implements IEnumerable(Of KeyValuePair(Of PdfName, PdfDirectObject)).GetEnumerator
            Return _entries.GetEnumerator()
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

#Region "Protected"

        Protected Friend Overrides Property Virtual As Boolean
            Get
                Return _virtual_
            End Get
            Set(ByVal value As Boolean)
                _virtual_ = value
            End Set
        End Property

#End Region
#End Region
#End Region

    End Class
End Namespace

''/*
''  Copyright 2006-2012 Stefano Chizzolini. http://www.dmdpdf.org

''  Contributors:
''    * Stefano Chizzolini (original code developer, http://www.stefanochizzolini.it)

''  Me file should be part of the source code distribution of "PDF Clown library" (the
''  Program): see the accompanying README files for more info.

''  Me Program is free software; you can redistribute it and/or modify it under the terms
''  of the GNU Lesser General Public License as published by the Free Software Foundation;
''  either version 3 of the License, or (at your option) any later version.

''  Me Program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY,
''  either expressed or implied; without even the implied warranty of MERCHANTABILITY or
''  FITNESS FOR A PARTICULAR PURPOSE. See the License for more details.

''  You should have received a copy of the GNU Lesser General Public License along with Me
''  Program (see README files); if not, go to the GNU website (http://www.gnu.org/licenses/).

''  Redistribution and use, with or without modification, are permitted provided that such
''  redistributions retain the above copyright notice, license and disclaimer, along with
''  Me list of conditions.
''*/

'Imports DMD.org.dmdpdf.bytes
'Imports DMD.org.dmdpdf.files
'Imports DMD.org.dmdpdf.tokens

'Imports System
'Imports System.Collections
'Imports System.Collections.Generic
'Imports System.Text '= System.Text;

'Namespace DMD.org.dmdpdf.objects

'    '/**
'    '  <summary>PDF dictionary object [PDF:1.6:3.2.6].</summary>
'    '*/
'    Public NotInheritable Class PdfDictionary
'        Inherits PdfDirectObject
'        Implements IDictionary(Of PdfName, PdfDirectObject)

'#Region "shared"
'#Region "fields"

'        Private Shared ReadOnly _BeginDictionaryChunk As Byte() = tokens.Encoding.Pdf.Encode(Keyword.BeginDictionary)
'        Private Shared ReadOnly _EndDictionaryChunk As Byte() = tokens.Encoding.Pdf.Encode(Keyword.EndDictionary)

'#End Region
'#End Region

'#Region "dynamic"
'#Region "fields"

'        Friend _entries As IDictionary(Of PdfName, PdfDirectObject)

'        Private _parent As PdfObject
'        Private _updateable As Boolean = True
'        Private _updated As Boolean
'        Private _virtual_ As Boolean

'#End Region

'#Region "constructors"

'        Public Sub New()
'            Me._entries = New Dictionary(Of PdfName, PdfDirectObject)()
'        End Sub

'        Public Sub New(ByVal capacity As Integer)
'            Me._entries = New Dictionary(Of PdfName, PdfDirectObject)(capacity)
'        End Sub

'        Public Sub New(ByVal keys As PdfName(), ByVal values As PdfDirectObject())
'            Me.New(values.Length)
'            Me.Updateable = False
'            For index As Integer = 0 To values.Length - 1
'                Me(keys(index)) = values(index)
'            Next
'            Me.Updateable = True
'        End Sub

'        Public Sub New(ByVal entries As IDictionary(Of PdfName, PdfDirectObject))
'            Me.New(entries.Count)
'            Me.Updateable = False
'            For Each entry As KeyValuePair(Of PdfName, PdfDirectObject) In entries
'                Me(entry.Key) = CType(Include(entry.Value), PdfDirectObject)
'            Next
'            Me.Updateable = True
'        End Sub

'#End Region

'#Region "Interface"
'#Region "Public"

'        Public Overrides Function Accept(ByVal visitor As IVisitor, ByVal data As Object) As PdfObject
'            Return visitor.Visit(Me, data)
'        End Function

'        Public Overrides Function CompareTo(ByVal obj As PdfDirectObject) As Integer
'            Throw New NotImplementedException()
'        End Function

'        '/**
'        '  <summary> Gets the value corresponding To the given key, forcing its instantiation As a direct
'        '  Object in case of missing entry.</summary>
'        '  <param name = "key" > key whose associated value Is To be returned.</param>
'        '*/
'        Public Function [Get](Of T As PdfDataObject)(ByVal key As PdfName) As PdfDirectObject
'            Return [Get](Of T)(key, True)
'        End Function

'        '/**
'        '  <summary> Gets the value corresponding To the given key, forcing its instantiation In Case Of
'        '  missing entry.</summary>
'        '  <param name = "key" > key whose associated value Is To be returned.</param>
'        '  <param name = "direct" > Whether the item has To be instantiated directly within its container
'        '  instead of being referenced through an indirect object.</param>
'        '*/
'        Public Function [Get](Of T As PdfDataObject)(ByVal key As PdfName, ByVal direct As Boolean) As PdfDirectObject 'where T : PdfDataObject, New()
'            Dim value As PdfDirectObject = Me(key)
'            If (value Is Nothing) Then
'                '/*
'                '  NOTE: The null - Object placeholder MUST Not perturb the existing Structure; therefore:  
'                '    - it MUST be marked as virtual in order Not to unnecessarily serialize it;
'                '    - it MUST be put into Me dictionary without affecting its update status.
'                '*/
'#If Not DEBUG Then
'                Try
'#End If
'                If (direct) Then
'                    value = CType(Include(CType(NewT(Of T)(), PdfDataObject)), PdfDirectObject)
'                Else
'                    value = CType(Include(New PdfIndirectObject(File, NewT(Of T)(), New XRefEntry(0, 0)).Reference), PdfDirectObject)
'                End If

'                _entries(key) = value
'                value.Virtual = True
'#If Not DEBUG Then
'                Catch e As System.Exception
'                    Throw New Exception(GetType(T).Name & " failed to instantiate.", e)
'                End Try
'#End If
'            End If
'            Return value
'        End Function

'        Public Overrides Function Equals(ByVal [object] As Object) As Boolean
'            Return MyBase.Equals([object]) OrElse
'                            ([object] IsNot Nothing AndAlso
'                             [object].GetType().Equals(Me.GetType()) AndAlso
'                             CType([object], PdfDictionary)._entries.Equals(_entries))
'        End Function

'        Public Overrides Function GetHashCode() As Integer
'            Return _entries.GetHashCode()
'        End Function

'        '/**
'        '  Gets the key associated To the specified value.
'        '*/
'        Public Function GetKey(ByVal value As PdfDirectObject) As PdfName
'            '/*
'            '  NOTE:       Current PdfDictionary implementation doesn't support bidirectional maps, to say that
'            '              the only currently-available way To retrieve a key from a value Is To iterate the whole map
'            '  (really poor performance!).
'            '*/
'            For Each entry As KeyValuePair(Of PdfName, PdfDirectObject) In _entries
'                If (entry.Value.Equals(value)) Then
'                    Return entry.Key
'                End If
'            Next
'            Return Nothing
'        End Function

'        Public Overrides ReadOnly Property Parent As PdfObject
'            Get
'                Return _parent
'            End Get
'        End Property

'        Friend Overrides Sub SetParent(value As PdfObject)
'            Me._parent = value
'        End Sub

'        '/**
'        '  <summary> Gets the dereferenced value corresponding To the given key.</summary>
'        '  <remarks> Me method takes care To resolve the value returned by <see cref= "Me[PdfName]" >
'        '  Me[PdfName]</see>.</remarks>
'        '  <param name = "key" > key whose associated value Is To be returned.</param>
'        '  <returns> null, if the map contains no mapping For Me key.</returns>
'        '*/
'        Public Overloads Function Resolve(ByVal key As PdfName) As PdfDataObject
'            Return Resolve(Me(key))
'        End Function

'        '/**
'        '  <summary> Gets the dereferenced value corresponding To the given key, forcing its instantiation
'        '  in case of missing entry.</summary>
'        '  <remarks> Me method takes care To resolve the value returned by <see cref= "Get(PdfName)" /> .
'        '  </remarks>
'        '  <param name = "key" > key whose associated value Is To be returned.</param>
'        '  <returns> null, if the map contains no mapping For Me key.</returns>
'        '*/
'        Public Overloads Function Resolve(Of T As PdfDataObject)(ByVal key As PdfName) As T
'            Return CType(Resolve([Get](Of T)(key)), T)
'        End Function

'        Public Overrides Function Swap(ByVal other As PdfObject) As PdfObject
'            Dim otherDictionary As PdfDictionary = CType(other, PdfDictionary)
'            Dim otherEntries As IDictionary(Of PdfName, PdfDirectObject) = otherDictionary._entries
'            ' Update the other!
'            otherDictionary._entries = Me._entries
'            otherDictionary.Update()
'            ' Update Me one!
'            Me._entries = otherEntries
'            Me.Update()
'            Return Me
'        End Function

'        Public Overrides Function ToString() As String
'            Dim Buffer As New System.Text.StringBuilder()
'            ' Begin.
'            Buffer.Append("<< ")
'            ' Entries.
'            For Each entry As KeyValuePair(Of PdfName, PdfDirectObject) In _entries
'                ' Entry...
'                ' ...key.
'                Buffer.Append(entry.Key.ToString()).Append(" ")
'                ' ...value.
'                Buffer.Append(PdfDirectObject.ToString(entry.Value)).Append(" ")
'            Next
'            ' End.
'            Buffer.Append(">>")
'            Return Buffer.ToString()
'        End Function

'        Public Overrides Property Updateable As Boolean
'            Get
'                Return Me._updateable
'            End Get
'            Set(ByVal value As Boolean)
'                Me._updateable = value
'            End Set
'        End Property

'        Public Overrides ReadOnly Property Updated As Boolean
'            Get
'                Return Me._updated
'            End Get
'        End Property

'        Protected Friend Overrides Sub SetUpdated(value As Boolean)
'            Me._updated = value
'        End Sub

'        Public Overrides Sub WriteTo(ByVal stream As IOutputStream, ByVal context As File)
'            ' Begin.
'            stream.Write(_BeginDictionaryChunk)
'            ' Entries.
'            For Each entry As KeyValuePair(Of PdfName, PdfDirectObject) In _entries
'                Dim value As PdfDirectObject = entry.Value
'                If (value IsNot Nothing AndAlso value.Virtual) Then
'                    Continue For
'                End If

'                ' Entry...
'                ' ...key.
'                entry.Key.WriteTo(stream, context)
'                stream.Write(Chunk.Space)
'                ' ...value.
'                PdfDirectObject.WriteTo(stream, context, value)
'                stream.Write(Chunk.Space)
'            Next
'            ' End.
'            stream.Write(_EndDictionaryChunk)
'        End Sub

'#Region "IDictionary"

'        Public Sub Add(ByVal key As PdfName, ByVal value As PdfDirectObject) Implements IDictionary(Of PdfName, PdfDirectObject).Add
'            _entries.Add(key, CType(Include(value), PdfDirectObject))
'            Update()
'        End Sub

'        Public Function ContainsKey(ByVal key As PdfName) As Boolean Implements IDictionary(Of PdfName, PdfDirectObject).ContainsKey
'            Return _entries.ContainsKey(key)
'        End Function

'        Public ReadOnly Property Keys As ICollection(Of PdfName) Implements IDictionary(Of PdfName, objects.PdfDirectObject).Keys
'            Get
'                Return _entries.Keys
'            End Get
'        End Property

'        Public Function Remove(ByVal key As PdfName) As Boolean Implements IDictionary(Of PdfName, PdfDirectObject).Remove
'            Dim oldValue As PdfDirectObject = Me(key)
'            If (_entries.Remove(key)) Then
'                Exclude(oldValue)
'                Update()
'                Return True
'            Else
'                Return False
'            End If
'        End Function


'        Default Public Property Item(ByVal key As PdfName) As PdfDirectObject Implements IDictionary(Of PdfName, PdfDirectObject).Item
'            Get
'                '/*
'                '  NOTE: Me Is an intentional violation of the official .NET Framework Class
'                '  Library prescription(no exception Is thrown anytime a key Is Not found --
'                '  a null pointer Is returned instead).
'                '*/
'                Dim value As PdfDirectObject = Nothing
'                _entries.TryGetValue(key, value)
'                Return value
'            End Get
'            Set(ByVal value As PdfDirectObject)
'                If (value Is Nothing) Then
'                    Remove(key)
'                Else
'                    Dim oldValue As PdfDirectObject = Me(key)
'                    _entries(key) = CType(Include(value), PdfDirectObject)
'                    Exclude(oldValue)
'                    Update()
'                End If
'            End Set
'        End Property

'        Public Function TryGetValue(ByVal key As PdfName, ByRef value As PdfDirectObject) As Boolean Implements IDictionary(Of PdfName, PdfDirectObject).TryGetValue
'            Return _entries.TryGetValue(key, value)
'        End Function

'        Public ReadOnly Property Values As ICollection(Of PdfDirectObject) Implements IDictionary(Of PdfName, PdfDirectObject).Values
'            Get
'                Return _entries.Values
'            End Get
'        End Property

'#Region "ICollection"

'        Private Sub _Add(ByVal entry As KeyValuePair(Of PdfName, PdfDirectObject)) Implements ICollection(Of KeyValuePair(Of PdfName, PdfDirectObject)).Add
'            Me.Add(entry.Key, entry.Value)
'        End Sub

'        Public Sub Clear() Implements ICollection(Of KeyValuePair(Of PdfName, PdfDirectObject)).Clear
'            For Each key As PdfName In New List(Of PdfDirectObject)(_entries.Keys)
'                Remove(key)
'            Next
'        End Sub

'        Private Function _Contains(ByVal entry As KeyValuePair(Of PdfName, PdfDirectObject)) As Boolean Implements ICollection(Of KeyValuePair(Of PdfName, PdfDirectObject)).Contains
'            Return CType(_entries, ICollection(Of KeyValuePair(Of PdfName, PdfDirectObject))).Contains(entry)
'        End Function

'        Public Sub CopyTo(ByVal entries As KeyValuePair(Of PdfName, PdfDirectObject)(), ByVal index As Integer) Implements ICollection(Of KeyValuePair(Of PdfName, PdfDirectObject)).CopyTo
'            Throw New NotImplementedException()
'        End Sub

'        Public ReadOnly Property Count As Integer Implements ICollection(Of KeyValuePair(Of PdfName, PdfDirectObject)).Count
'            Get
'                Return _entries.Count
'            End Get
'        End Property

'        Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of KeyValuePair(Of PdfName, PdfDirectObject)).IsReadOnly
'            Get
'                Return False
'            End Get
'        End Property

'        Public Function Remove(ByVal entry As KeyValuePair(Of PdfName, PdfDirectObject)) As Boolean Implements ICollection(Of KeyValuePair(Of PdfName, PdfDirectObject)).Remove
'            If (entry.Value.Equals(Me(entry.Key))) Then
'                Return Remove(entry.Key)
'            Else
'                Return False
'            End If
'        End Function

'#Region "IEnumerable<KeyValuePair(of PdfName,PdfDirectObject)>"

'        Public Function GetEnumerator() As IEnumerator(Of KeyValuePair(Of PdfName, PdfDirectObject)) Implements IEnumerable(Of KeyValuePair(Of PdfName, objects.PdfDirectObject)).GetEnumerator
'            Return Me._entries.GetEnumerator()
'        End Function

'#Region "IEnumerable"

'        Private Function _GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
'            Return Me.GetEnumerator()
'        End Function

'#End Region
'#End Region
'#End Region
'#End Region
'#End Region

'#Region "Protected"

'        Protected Friend Overrides Property Virtual As Boolean
'            Get
'                Return Me._virtual_
'            End Get
'            Set(ByVal value As Boolean)
'                Me._virtual_ = value
'            End Set
'        End Property

'#End Region
'#End Region
'#End Region
'    End Class


'End Namespace

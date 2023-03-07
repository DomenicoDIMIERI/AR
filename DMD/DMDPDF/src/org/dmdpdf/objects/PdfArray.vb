'/*
'  Copyright 2006 - 2013 Stefano Chizzolini. http: //www.dmdpdf.org

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

Imports DMD.org.dmdpdf
Imports DMD.org.dmdpdf.bytes
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.tokens
Imports DMD.org.dmdpdf.util.collections.generic

Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports text = System.Text

Namespace DMD.org.dmdpdf.objects

    '/**
    '  <summary> PDF array Object, that Is a one-dimensional collection Of (possibly-heterogeneous)
    '  objects arranged sequentially [PDF:1.7:3.2.5].</summary>
    '*/
    Public NotInheritable Class PdfArray
        Inherits PdfDirectObject
        Implements IList(Of PdfDirectObject)

#Region "Shared"
#Region "fields"

        Private Shared ReadOnly BeginArrayChunk As Byte() = Encoding.Pdf.Encode(Keyword.BeginArray)
        Private Shared ReadOnly EndArrayChunk As Byte() = Encoding.Pdf.Encode(Keyword.EndArray)

#End Region
#End Region

#Region "dynamic"
#Region "fields"

        Friend _items As List(Of PdfDirectObject)

        Private _parent As PdfObject
        Private _updateable As Boolean = True
        Private _updated As Boolean
        Private _virtual_ As Boolean

#End Region

#Region "constructors"

        Public Sub New()
            Me.New(10)
        End Sub

        Public Sub New(ByVal capacity As Integer)
            Me._items = New List(Of PdfDirectObject)(capacity)
        End Sub

        Public Sub New(ParamArray items As PdfDirectObject())
            Me.New(items.Length)
            Me.Updateable = False
            Me.AddAll(items)
            Me.Updateable = True
        End Sub

        Public Sub New(ByVal items As IList(Of PdfDirectObject))
            Me.New(items.Count)
            Me.Updateable = False
            Me.AddAll(items)
            Me.Updateable = True
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

        Public Overrides Function Equals(ByVal [object] As Object) As Boolean
            Return MyBase.Equals([object]) OrElse ([object] IsNot Nothing AndAlso
                                                    [object].GetType().Equals(Me.GetType()) AndAlso
                                                    CType([object], PdfArray)._items.Equals(Me._items))
        End Function

        '/**
        '  <summary> Gets the value corresponding To the given index, forcing its instantiation As a direct
        '  Object in case of missing entry.</summary>
        '  <param name = "index" > Index Of the item To Return.</param>
        '  <param name = "itemClass" >Class to use for instantiating the item in case of missing entry.</param>
        '*/
        Public Function [Get](Of T As PdfDataObject)(ByVal index As Integer) As PdfDirectObject ' where T : PdfDataObject, New()
            Return [Get](Of T)(index, True)
        End Function

        '/**
        '  <summary> Gets the value corresponding To the given index, forcing its instantiation In Case
        '  of missing entry.</summary>
        '  <param name = "index" > index Of the item To Return.</param>
        '  <param name = "direct" > Whether the item has To be instantiated directly within its container
        '  instead of being referenced through an indirect object.</param>
        '*/
        Public Function [Get](Of T As PdfDataObject)(ByVal index As Integer, ByVal direct As Boolean) As PdfDirectObject ' where T : PdfDataObject, New()
            Dim item As PdfDirectObject = Nothing
            If (index < Me.Count) Then item = Me(index)
            If (index = Me.Count OrElse item Is Nothing OrElse Not item.Resolve().GetType().Equals(GetType(T))) Then
                '/*
                '  NOTE: The null - Object placeholder MUST Not perturb the existing Structure; therefore:    
                '    - it MUST be marked as virtual in order Not to unnecessarily serialize it;
                '    - it MUST be put into Me array without affecting its update status.
                '*/
                Try
                    If (direct) Then
                        item = CType(Include(CType(NewT(Of T)(), PdfDataObject)), PdfDirectObject)
                    Else
                        item = CType(Include(New PdfIndirectObject(Me.File, NewT(Of T)(), New XRefEntry(0, 0)).Reference), PdfDirectObject)
                    End If
                    If (index = Me.Count) Then
                        _items.Add(item)
                    ElseIf (item Is Nothing) Then
                        _items(index) = item
                    Else
                        _items.Insert(index, item)
                    End If
                    item.Virtual = True
                Catch e As System.Exception
                    Throw New Exception(GetType(T).Name & " failed to instantiate.", e)
                End Try
            End If
            Return item
        End Function

        Public Overrides Function GetHashCode() As Integer
            Return Me._items.GetHashCode()
        End Function

        Public Overrides ReadOnly Property Parent As PdfObject
            Get
                Return Me._parent
            End Get
        End Property

        Friend Overrides Sub SetParent(value As PdfObject)
            Me._parent = value
        End Sub

        '/**
        '  <summary> Gets the dereferenced value corresponding To the given index.</summary>
        '  <remarks> Me method takes care To resolve the value returned by
        '  <see cref = "Me[int]" > Me[int]</see>.</remarks>
        '  <param name = "index" > Index Of the item To Return.</param>
        '*/
        Public Function Resolve(ByVal index As Integer) As PdfDataObject
            Return MyBase.Resolve(Me(index))
        End Function

        '/**
        '  <summary> Gets the dereferenced value corresponding To the given index, forcing its
        '  instantiation in case of missing entry.</summary>
        '  <remarks> Me method takes care To resolve the value returned by
        '  <see cref = "Get(of T)" >Get(of T)</see>.</remarks>
        '  <param name = "index" > Index Of the item To Return.</param>
        '*/
        Public Function Resolve(Of T As PdfDataObject)(ByVal index As Integer) As T 'where T : PdfDataObject, New()
            Return CType(MyBase.Resolve([Get](Of T)(index)), T)
        End Function

        Public Overrides Function Swap(ByVal other As PdfObject) As PdfObject
            Dim otherArray As PdfArray = CType(other, PdfArray)
            Dim otherItems As List(Of PdfDirectObject) = CType(otherArray._items, List(Of PdfDirectObject))
            ' Update the other!
            otherArray._items = Me._items
            otherArray.Update()
            ' Update Me one!
            Me._items = otherItems
            Me.Update()
            Return Me
        End Function

        Public Overrides Function ToString() As String
            Dim buffer As New text.StringBuilder()
            ' Begin.
            buffer.Append("[ ")
            ' Elements.
            For Each item As PdfDirectObject In _items
                buffer.Append(PdfDirectObject.ToString(item)).Append(" ")
            Next
            ' End.
            buffer.Append("]")

            Return buffer.ToString()
        End Function

        Public Overrides Property Updateable As Boolean
            Get
                Return Me._updateable
            End Get
            Set(ByVal value As Boolean)
                Me._updateable = value
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
            stream.Write(BeginArrayChunk)
            ' Elements.
            For Each item As PdfDirectObject In _items
                If (item IsNot Nothing AndAlso item.Virtual) Then Continue For
                PdfDirectObject.WriteTo(stream, context, item)
                stream.Write(Chunk.Space)
            Next
            ' End.
            stream.Write(EndArrayChunk)
        End Sub

#Region "IList"

        Public Function IndexOf(ByVal item As PdfDirectObject) As Integer Implements IList(Of PdfDirectObject).IndexOf
            Return Me._items.IndexOf(item)
        End Function

        Public Sub Insert(ByVal index As Integer, ByVal item As PdfDirectObject) Implements IList(Of PdfDirectObject).Insert
            Me._items.Insert(index, CType(Include(item), PdfDirectObject))
            Me.Update()
        End Sub

        Public Sub RemoveAt(ByVal index As Integer) Implements IList(Of PdfDirectObject).RemoveAt
            Dim oldItem As PdfDirectObject = Me._items(index)
            Me._items.RemoveAt(index)
            Me.Exclude(oldItem)
            Me.Update()
        End Sub

        Default Public Property Item(ByVal index As Integer) As PdfDirectObject Implements IList(Of PdfDirectObject).Item
            Get
                Return _items(index)
            End Get
            Set(ByVal value As PdfDirectObject)
                Dim oldItem As PdfDirectObject = Me._items(index)
                Me._items(index) = CType(Include(value), PdfDirectObject)
                Me.Exclude(oldItem)
                Me.Update()
            End Set
        End Property

#Region "ICollection"

        Public Sub Add(ByVal item As PdfDirectObject) Implements ICollection(Of PdfDirectObject).Add
            Me._items.Add(CType(Include(item), PdfDirectObject))
            Me.Update()
        End Sub

        Public Sub Clear() Implements ICollection(Of PdfDirectObject).Clear
            While (Me._items.Count > 0)
                Me.RemoveAt(0)
            End While
        End Sub

        Public Function Contains(ByVal item As PdfDirectObject) As Boolean Implements ICollection(Of PdfDirectObject).Contains
            Return Me._items.Contains(item)
        End Function

        Public Sub CopyTo(ByVal items As PdfDirectObject(), ByVal index As Integer) Implements ICollection(Of PdfDirectObject).CopyTo
            Me._items.CopyTo(items, index)
        End Sub

        Public ReadOnly Property Count As Integer Implements ICollection(Of PdfDirectObject).Count
            Get
                Return _items.Count
            End Get
        End Property

        Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of PdfDirectObject).IsReadOnly
            Get
                Return False
            End Get
        End Property

        Public Function Remove(ByVal item As PdfDirectObject) As Boolean Implements ICollection(Of PdfDirectObject).Remove
            If (Not Me._items.Remove(item)) Then Return False
            Me.Exclude(CType(item, PdfDirectObject))
            Me.Update()
            Return True
        End Function

#Region "IEnumerable(Of PdfDirectObject)"

        Public Function GetEnumerator() As IEnumerator(Of PdfDirectObject) Implements IEnumerable(Of PdfDirectObject).GetEnumerator
            Return _items.GetEnumerator()
        End Function

#Region "IEnumerable"

        Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
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
                Return Me._virtual_
            End Get
            Set(ByVal value As Boolean)
                _virtual_ = value
            End Set
        End Property

#End Region
#End Region
#End Region

    End  Class

End Namespace
''/*
''  Copyright 2006-2013 Stefano Chizzolini. http://www.dmdpdf.org

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

'Imports DMD.org.dmdpdf
'Imports DMD.org.dmdpdf.bytes
'Imports DMD.org.dmdpdf.files
'Imports DMD.org.dmdpdf.tokens
'Imports DMD.org.dmdpdf.util.collections.generic

'Imports System
'Imports System.Collections
'Imports System.Collections.Generic
'Imports System.Text

'Namespace DMD.org.dmdpdf.objects

'    '/**
'    '  <summary>PDF array object, that is a one-dimensional collection of (possibly-heterogeneous)
'    '  objects arranged sequentially [PDF:1.7:3.2.5].</summary>
'    '*/
'    Public NotInheritable Class PdfArray
'        Inherits PdfDirectObject
'        Implements IList(Of PdfDirectObject)

'#Region "shared"
'#Region "fields"

'        Private Shared ReadOnly BeginArrayChunk As Byte() = tokens.Encoding.Pdf.Encode(Keyword.BeginArray)
'        Private Shared ReadOnly EndArrayChunk As Byte() = tokens.Encoding.Pdf.Encode(Keyword.EndArray)

'#End Region
'#End Region

'#Region "dynamic"
'#Region "fields"

'        Friend _items As List(Of PdfDirectObject)

'        Private _parent As PdfObject
'        Private _updateable As Boolean = True
'        Private _updated As Boolean
'        Private _virtual_ As Boolean

'#End Region

'#Region "constructors"

'        Public Sub New()
'            Me.New(10)
'        End Sub

'        Public Sub New(ByVal capacity As Integer)
'            Me._items = New List(Of PdfDirectObject)(capacity)
'        End Sub

'        Public Sub New(ParamArray items As PdfDirectObject())
'            Me.New(items.Length)
'            Me.Updateable = False
'            Me.AddAll(items)
'            Me.Updateable = True
'        End Sub

'        Public Sub New(ByVal items As IList(Of PdfDirectObject))
'            Me.New(items.Count)
'            Me.Updateable = False
'            Me.AddAll(items)
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

'        Public Overrides Function Equals(ByVal [object] As Object) As Boolean
'            Return MyBase.Equals([object]) OrElse
'            ([object] IsNot Nothing AndAlso [object].GetType().Equals(Me.GetType()) AndAlso
'            CType([object], PdfArray)._items.Equals(Me._items))
'        End Function

'        '/**
'        '  <summary>Gets the value corresponding to the given index, forcing its instantiation as a direct
'        '  object in case of missing entry.</summary>
'        '  <param name="index">Index of the item to return.</param>
'        '  <param name="itemClass">Class to use for instantiating the item in case of missing entry.</param>
'        '*/
'        Public Function [Get](Of T As PdfDataObject)(ByVal index As Integer) As PdfDirectObject
'            Return [Get](Of T)(index, True)
'        End Function

'        '/**
'        '  <summary>Gets the value corresponding to the given index, forcing its instantiation in case
'        '  of missing entry.</summary>
'        '  <param name="index">Index of the item to return.</param>
'        '  <param name="direct">Whether the item has to be instantiated directly within its container
'        '  instead of being referenced through an indirect object.</param>
'        '*/
'        Public Function [Get](Of T As PdfDataObject)(ByVal index As Integer, ByVal direct As Boolean) As PdfDirectObject 'where T : PdfDataObject, new()
'            Dim item As PdfDirectObject = Nothing
'            If (index < Me.Count) Then item = Me(index)
'            If (index = Me.Count OrElse
'                item Is Nothing OrElse
'                Not item.Resolve().GetType().Equals(GetType(T))
'               ) Then
'                '/*
'                '  NOTE: The null - Object placeholder MUST Not perturb the existing structure; therefore:  
'                '    - it MUST be marked as virtual in order Not to unnecessarily serialize it;
'                '    - it MUST be put into Me array without affecting its update status.
'                '*/
'                Try
'                    If (direct) Then
'                        item = CType(Include(CType(NewT(Of T)(), PdfDataObject)), PdfDirectObject)
'                    Else
'                        item = CType(Include(New PdfIndirectObject(Me.File, NewT(Of T)(), New XRefEntry(0, 0)).Reference), PdfDirectObject)
'                    End If

'                    If (index = Count) Then
'                        Me._items.Add(item)
'                    ElseIf (item Is Nothing) Then
'                        Me._items(index) = item
'                    Else
'                        Me._items.Insert(index, item)
'                    End If
'                    item.Virtual = True
'                Catch e As System.Exception
'                    Throw New Exception(GetType(T).Name & " failed to instantiate.", e)
'                End Try
'            End If
'            Return item
'        End Function

'        Public Overrides Function GetHashCode() As Integer
'            Return Me._items.GetHashCode()
'        End Function

'        Public Overrides ReadOnly Property Parent As PdfObject
'            Get
'                Return Me._parent
'            End Get
'        End Property

'        Friend Overrides Sub SetParent(value As PdfObject)
'            Me._parent = value
'        End Sub

'        '/**
'        '  <summary> Gets the dereferenced value corresponding To the given index.</summary>
'        '  <remarks> Me method takes care To resolve the value returned by
'        '  <see cref = "Me[int]" > Me[int]</see>.</remarks>
'        '  <param name = "index" > index Of the item To Return.</param>
'        '*/
'        Public Overloads Function Resolve(ByVal index As Integer) As PdfDataObject
'            Return Resolve(Me.Item(index))
'        End Function

'        '/**
'        '  <summary> Gets the dereferenced value corresponding To the given index, forcing its
'        '  instantiation in case of missing entry.</summary>
'        '  <remarks> Me method takes care To resolve the value returned by
'        '  <see cref = "Get(of T)" >Get(of T)</see>.</remarks>
'        '  <param name = "index" > index Of the item To Return.</param>
'        '*/
'        Public Overloads Function Resolve(Of T As PdfDataObject)(ByVal index As Integer) As T
'            Return CType(Resolve([Get](Of T)(index)), T)
'        End Function

'        Public Overrides Function Swap(ByVal other As PdfObject) As PdfObject
'            Dim otherArray As PdfArray = CType(other, PdfArray)
'            Dim otherItems As List(Of PdfDirectObject) = CType(otherArray._items, List(Of PdfDirectObject))
'            ' Update the other!
'            otherArray._items = Me._items
'            otherArray.Update()
'            ' Update Me one!
'            Me._items = otherItems
'            Me.Update()
'            Return Me
'        End Function

'        Public Overrides Function ToString() As String
'            Dim Buffer As New Text.StringBuilder()
'            ' Begin.
'            Buffer.Append("[ ")
'            ' Elements.
'            For Each item As PdfDirectObject In Me._items
'                Buffer.Append(PdfDirectObject.ToString(item)).Append(" ")
'            Next
'            ' End.
'            Buffer.Append("]")

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

'        Public Overrides Sub WriteTo(ByVal Stream As IOutputStream, ByVal context As File)
'            ' Begin.
'            Stream.Write(BeginArrayChunk)
'            ' Elements.
'            For Each item As PdfDirectObject In Me._items
'                If (item IsNot Nothing AndAlso item.Virtual) Then Continue For
'                PdfDirectObject.WriteTo(Stream, context, item)
'                Stream.Write(Chunk.Space)
'            Next
'            ' End.
'            Stream.Write(EndArrayChunk)
'        End Sub

'#Region "IList"

'        Public Function IndexOf(ByVal item As PdfDirectObject) As Integer Implements IList(Of objects.PdfDirectObject).IndexOf
'            Return Me._items.IndexOf(item)
'        End Function

'        Public Sub Insert(ByVal Index As Integer, ByVal item As PdfDirectObject) Implements IList(Of objects.PdfDirectObject).Insert
'            Me._items.Insert(Index, CType(Include(item), PdfDirectObject))
'            Update()
'        End Sub

'        Public Sub RemoveAt(ByVal Index As Integer) Implements IList(Of objects.PdfDirectObject).RemoveAt
'            Dim oldItem As PdfDirectObject = Me._items(Index)
'            Me._items.RemoveAt(Index)
'            Exclude(oldItem)
'            Update()
'        End Sub

'        Default Public Property Item(ByVal Index As Integer) As PdfDirectObject Implements IList(Of PdfDirectObject).Item
'            Get
'                Return Me._items(Index)
'            End Get
'            Set(ByVal value As PdfDirectObject)
'                Dim oldItem As PdfDirectObject = Me._items(Index)
'                Me._items(Index) = CType(Include(value), PdfDirectObject)
'                Exclude(oldItem)
'                Update()
'            End Set
'        End Property

'#Region "ICollection"

'        Public Sub Add(ByVal item As PdfDirectObject) Implements ICollection(Of PdfDirectObject).Add
'            Me._items.Add(CType(Include(item), PdfDirectObject))
'            Update()
'        End Sub

'        Public Sub Clear() Implements ICollection(Of PdfDirectObject).Clear
'            While (Me._items.Count > 0)
'                RemoveAt(0)
'            End While
'        End Sub

'        Public Function Contains(ByVal item As PdfDirectObject) As Boolean Implements ICollection(Of PdfDirectObject).Contains
'            Return Me._items.Contains(item)
'        End Function

'        Public Sub CopyTo(ByVal items As PdfDirectObject(), ByVal Index As Integer) Implements ICollection(Of objects.PdfDirectObject).CopyTo
'            Me._items.CopyTo(items, Index)
'        End Sub

'        Public ReadOnly Property Count As Integer Implements ICollection(Of PdfDirectObject).Count
'            Get
'                Return Me._items.Count
'            End Get
'        End Property

'        Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of PdfDirectObject).IsReadOnly
'            Get
'                Return False
'            End Get
'        End Property

'        Public Function Remove(ByVal item As PdfDirectObject) As Boolean Implements ICollection(Of PdfDirectObject).Remove
'            If (Not Me._items.Remove(item)) Then Return False
'            Exclude(CType(item, PdfDirectObject))
'            Update()
'            Return True
'        End Function

'#Region "IEnumerable(Of PdfDirectObject)"

'        Public Function GetEnumerator() As IEnumerator(Of PdfDirectObject) Implements IEnumerable(Of PdfDirectObject).GetEnumerator
'            Return Me._items.GetEnumerator()
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

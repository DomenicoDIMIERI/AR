'/*
'  Copyright 2006-2012 Stefano Chizzolini. http://www.dmdpdf.org

'  Contributors:
'    * Stefano Chizzolini (original code developer, http://www.stefanochizzolini.it)

'  Me file should be part of the source code distribution of "PDF Clown library" (the
'  Program): see the accompanying README files for more info.

'  Me Program is free software; you can redistribute it and/or modify it under the terms
'  of the GNU Lesser General Public License as published by the Free Software Foundation;
'  either version 3 of the License, or (at your option) any later version.

'  Me Program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY,
'  either expressed or implied; without even the implied warranty of MERCHANTABILITY or
'  FITNESS FOR A PARTICULAR PURPOSE. See the License for more details.

'  You should have received a copy of the GNU Lesser General Public License along with Me
'  Program (see README files); if not, go to the GNU website (http://www.gnu.org/licenses/).

'  Redistribution and use, with or without modification, are permitted provided that such
'  redistributions retain the above copyright notice, license and disclaimer, along with
'  Me list of conditions.
'*/

Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.documents.contents
Imports DMD.org.dmdpdf.objects
Imports DMD.org.dmdpdf.tokens

Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Text

Namespace DMD.org.dmdpdf.files

    '/**
    '  <summary>Collection of the <b>alive indirect objects</b> available inside the
    '  file.</summary>
    '  <remarks>
    '    <para>According to the PDF spec, <i>indirect object entries may be free
    '    (no data object associated) or in-use (data object associated)</i>.</para>
    '    <para>We can effectively subdivide indirect objects in two possibly-overriding
    '    collections: the <b>original indirect objects</b> (coming from the associated
    '    preexisting file) and the <b>newly-registered indirect objects</b> (coming
    '    from new data objects or original indirect objects manipulated during the
    '    current session).</para>
    '    <para><i>To ensure that the modifications applied to an original indirect object
    '    are committed to being persistent</i> is critical that the modified original
    '    indirect object is newly-registered (practically overriding the original
    '    indirect object).</para>
    '    <para><b>Alive indirect objects</b> encompass all the newly-registered ones plus
    '    not-overridden original ones.</para>
    '  </remarks>
    '*/
    Public NotInheritable Class IndirectObjects
        Implements IList(Of PdfIndirectObject)

#Region "dynamic"
#Region "fields"
        '/**
        '  <summary>Associated file.</summary>
        '*/
        Private _file As File

        '/**
        '  <summary>Map of matching references of imported indirect objects.</summary>
        '  <remarks>
        '    <para>Me collection is used to prevent duplications among imported
        '    indirect objects.</para>
        '    <para><code>Key</code> is the external indirect object hashcode, <code>Value</code> is the
        '    matching internal indirect object.</para>
        '  </remarks>
        '*/
        Private _importedObjects As Dictionary(Of Integer, PdfIndirectObject) = New Dictionary(Of Integer, PdfIndirectObject)()
        '/**
        '  <summary> Collection Of newly-registered indirect objects.</summary>
        '*/
        Private _modifiedObjects As SortedDictionary(Of Integer, PdfIndirectObject) = New SortedDictionary(Of Integer, PdfIndirectObject)()
        '/**
        '  <summary> Collection Of instantiated original indirect objects.</summary>
        '  <remarks> Me collection Is used As a cache To avoid unconsistent parsing duplications.</remarks>
        '*/
        Private _wokenObjects As SortedDictionary(Of Integer, PdfIndirectObject) = New SortedDictionary(Of Integer, PdfIndirectObject)()

        '/**
        '  <summary>Object counter.</summary>
        '*/
        Private _lastObjectNumber As Integer
        '/**
        '  <summary> Offsets Of the original indirect objects inside the associated file
        '  (to say: implicit collection Of the original indirect objects).</summary>
        '  <remarks> Me information Is vital To randomly retrieve the indirect-Object
        '  persistent representation inside the associated file.</remarks>
        '*/
        Private _xrefEntries As SortedDictionary(Of Integer, XRefEntry)

#End Region

#Region "constructors"

        Friend Sub New(ByVal file As File, ByVal xrefEntries As SortedDictionary(Of Integer, XRefEntry))
            Me._file = file
            Me._xrefEntries = xrefEntries
            If (Me._xrefEntries Is Nothing) Then  '// No original Indirect objects.
                '// Register the leading free-object!
                '/*
                '  NOTE: Mandatory head Of the linked list Of free objects
                '  at Object number 0 [PDF:  1.6:3.4.3].
                '*/
                _lastObjectNumber = 0
                _modifiedObjects(_lastObjectNumber) = New PdfIndirectObject(
                                                        Me._file,
                                                        Nothing,
                                                        New XRefEntry(
                                                            _lastObjectNumber,
                                                            XRefEntry.GenerationUnreusable,
                                                            0,
                                                            XRefEntry.UsageEnum.Free
                                                            )
                                                        )
            Else
                ' Adjust the object counter!
                _lastObjectNumber = xrefEntries.Keys.Last()
            End If
        End Sub

#End Region

#Region "interface"
#Region "public"


        '/**
        '  <summary> Registers an <i>internal</i> data Object.</summary>
        '  <remarks>To register an external indirect Object, use <see
        '  cref = "AddExternal(PdfIndirectObject)" /> .</remarks>
        '  <returns> Indirect Object corresponding To the registered data Object.</returns>
        '*/
        Public Function Add(ByVal obj As PdfDataObject) As PdfIndirectObject
            '// Register a New indirect object wrapping the data object inside!
            _lastObjectNumber += 1
            Dim indirectObject As PdfIndirectObject = New PdfIndirectObject(
                                                            _file,
                                                            obj,
                                                            New XRefEntry(_lastObjectNumber, 0)
                                                            )
            _modifiedObjects(_lastObjectNumber) = indirectObject
            Return indirectObject
        End Function

        '/**
        '  <summary> Registers an <i>external</i> indirect Object.</summary>
        '  <remarks>
        '    <para> External indirect objects come from alien PDF files; therefore, Me Is a powerful way
        '    to import contents from a file into another one.</para>
        '    <para>To register an internal data Object, use <see cref="Add(PdfDataObject)"/>.</para>
        '  </remarks>
        '  <param nme = "obj" > External indirect Object To import.</param>
        '  <returns> Indirect Object imported from the external indirect Object.</returns>
        '*/
        Public Function AddExternal(ByVal obj As PdfIndirectObject) As PdfIndirectObject
            Return AddExternal(obj, _file.Cloner)
        End Function

        '/**
        '  <summary> Registers an <i>external</i> indirect Object.</summary>
        '  <remarks>
        '    <para> External indirect objects come from alien PDF files; therefore, Me Is a powerful way
        '    to import contents from a file into another one.</para>
        '    <para>To register an internal data Object, use <see cref="Add(PdfDataObject)"/>.</para>
        '  </remarks>
        '  <param nme = "obj" > External indirect Object To import.</param>
        '  <param nme = "cloner" > Import rules.</param>
        '  <returns> Indirect Object imported from the external indirect Object.</returns>
        '*/
        Public Function AddExternal(ByVal obj As PdfIndirectObject, ByVal Cloner As Cloner) As PdfIndirectObject
            If (Cloner.Context IsNot _file) Then Throw New ArgumentException("cloner file context incompatible")
            Dim indirectObject As PdfIndirectObject = Nothing
            ' Hasn't the external indirect object been imported yet?
            If (Not _importedObjects.TryGetValue(obj.GetHashCode(), indirectObject)) Then
                ' Keep track of the imported indirect object!
                indirectObject = Add(CType(Nothing, PdfDataObject)) ' [DEV:AP] Circular reference issue solved.
                _importedObjects.Add(
                                obj.GetHashCode(),
                                indirectObject
                                )
                indirectObject.DataObject = CType(obj.DataObject.Accept(Cloner, Nothing), PdfDataObject)
            End If
            Return indirectObject
        End Function

        '/**
        '  <summary> Gets the file associated To Me collection.</summary>
        '*/
        Public ReadOnly Property File As File
            Get
                Return Me._file
            End Get
        End Property

        Public Function IsEmpty() As Boolean
            '/*
            '  NOTE:       Indirect objects ' semantics imply that the collection is considered empty when no
            '  in-use object Is available.
            '*/
            For Each obj As PdfIndirectObject In Me
                If (obj.IsInUse()) Then Return False
            Next
            Return True
        End Function

#Region "IList"

        Public Function IndexOf(ByVal obj As PdfIndirectObject) As Integer Implements IList(Of PdfIndirectObject).IndexOf
            ' Is Me indirect object associated to Me file?
            If (obj.File IsNot Me._file) Then Return -1
            Return obj.Reference.ObjectNumber
        End Function

        Public Sub Insert(ByVal Index As Integer, ByVal obj As PdfIndirectObject) Implements IList(Of PdfIndirectObject).Insert
            Throw New NotSupportedException()
        End Sub

        Public Sub RemoveAt(ByVal Index As Integer) Implements IList(Of PdfIndirectObject).RemoveAt
            If (Me(Index).IsInUse()) Then
                '/*
                '  NOTE:                 Acrobat 6.0 And later(PDF 1.5+) DO Not use the free list to recycle object numbers;
                '  New objects are assigned New numbers [PDF:1.6:H00000.3:     16].
                '  According to such an implementation note, we simply mark the removed object as 'not-reusable'
                '                        newly-freed entry, neglecting both to add it to the linked list of free entries And to
                '  increment by 1 its generation number.
                '*/
                Update(
                        New PdfIndirectObject(
                                        _file,
                                        Nothing,
                                        New XRefEntry(
                                                Index,
                                                XRefEntry.GenerationUnreusable,
                                                0,
                                                XRefEntry.UsageEnum.Free
                                                )
                                          )
                        )
            End If
        End Sub

        Default Public Property Item(ByVal Index As Integer) As PdfIndirectObject Implements IList(Of objects.PdfIndirectObject).Item
            Get
                If (Index < 0 OrElse Index >= Count) Then Throw New ArgumentOutOfRangeException()

                Dim obj As PdfIndirectObject = Nothing
                If (Not _modifiedObjects.TryGetValue(Index, obj)) Then
                    If (Not _wokenObjects.TryGetValue(Index, obj)) Then
                        Dim XRefEntry As XRefEntry = Nothing
                        If (Not _xrefEntries.TryGetValue(Index, XRefEntry)) Then
                            '/*
                            '  NOTE:                           The cross - reference table (comprising the original cross-reference section And
                            '  All update sections) MUST contain one entry For Each Object number from 0 To the
                            '  maximum Object number used In the file, even if one Or more of the object numbers in
                            '  Me Range Do Not actually occur In the file. However, For resilience purposes
                            '  missing entries are treated As free ones.
                            '*/
                            XRefEntry = New XRefEntry(
                                                                    Index,
                                                                    XRefEntry.GenerationUnreusable,
                                                                    0,
                                                                    XRefEntry.UsageEnum.Free
                                                                    )
                            _xrefEntries(Index) = XRefEntry
                        End If

                        '// Awake the object!
                        '/*
                        '  NOTE:         Me operation allows To keep a consistent state across the whole session,
                        '  avoiding multiple incoherent instantiations Of the same original indirect Object.
                        '*/
                        obj = New PdfIndirectObject(_file, Nothing, XRefEntry)
                        _wokenObjects(Index) = obj
                    End If
                End If
                Return obj
            End Get
            Set(ByVal value As PdfIndirectObject)
                Throw New NotSupportedException()
            End Set
        End Property

#Region "ICollection"

        '/**
        '  <summary> Registers an <i>external</i> indirect Object.</summary>
        '  <remarks>
        '    <para> External indirect objects come from alien PDF files; therefore, Me Is a powerful way
        '    to import contents from a file into another one.</para>
        '    <para>To register And Get an external indirect Object, use <see
        '    cref = "AddExternal(PdfIndirectObject)" /></para>
        '                                                                                                                                                      </remarks>
        '  <returns> Whether the indirect Object was successfully registered.</returns>
        '*/
        Public Sub Add(ByVal obj As PdfIndirectObject) Implements ICollection(Of PdfIndirectObject).Add
            AddExternal(obj)
        End Sub

        Public Sub Clear() Implements ICollection(Of PdfIndirectObject).Clear
            Dim length As Integer = Count
            For index As Integer = 0 To length - 1
                RemoveAt(index)
            Next
        End Sub

        Public Function Contains(ByVal obj As PdfIndirectObject) As Boolean Implements ICollection(Of PdfIndirectObject).Contains
            Try
                Return Me(obj.Reference.ObjectNumber) Is obj
            Catch e As ArgumentOutOfRangeException
                Return False
            End Try
        End Function

        Public Sub CopyTo(ByVal objs As PdfIndirectObject(), ByVal Index As Integer) Implements ICollection(Of objects.PdfIndirectObject).CopyTo
            Throw New NotSupportedException()
        End Sub

        '/**
        '  <summary> Gets the number Of entries available (both In-use And free) In the
        '  Collection.</summary>
        '  <returns> The number Of entries available In the collection.</returns>
        '*/
        Public ReadOnly Property Count As Integer Implements ICollection(Of objects.PdfIndirectObject).Count
            Get
                Return Me._lastObjectNumber + 1
            End Get
        End Property

        Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of objects.PdfIndirectObject).IsReadOnly
            Get
                Return False
            End Get
        End Property

        Public Function Remove(ByVal obj As PdfIndirectObject) As Boolean Implements ICollection(Of PdfIndirectObject).Remove
            If (Not Me.Contains(obj)) Then Return False
            RemoveAt(obj.Reference.ObjectNumber)
            Return True
        End Function

#Region "IEnumerable<ContentStream>"


        Public Function GetEnumerator() As IEnumerator(Of PdfIndirectObject) Implements IEnumerable(Of PdfIndirectObject).GetEnumerator
            Return New mEnumerator(Of PdfIndirectObject)(Me)
        End Function

#Region "IEnumerable"
        Private Function _GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Return Me.GetEnumerator()
        End Function

#End Region
#End Region
#End Region
#End Region
#End Region

#Region "internal"

        Friend Function AddVirtual(ByVal obj As PdfIndirectObject) As PdfIndirectObject
            ' Update the reference of the object!
            Dim xref As XRefEntry = obj.XrefEntry
            _lastObjectNumber += 1
            xref.Number = _lastObjectNumber
            xref.Generation = 0
            ' Register the object!
            _modifiedObjects(_lastObjectNumber) = obj
            Return obj
        End Function

        Friend ReadOnly Property ModifiedObjects As SortedDictionary(Of Integer, PdfIndirectObject)
            Get
                Return Me._modifiedObjects
            End Get
        End Property

        Friend Function Update(ByVal obj As PdfIndirectObject) As PdfIndirectObject
            Dim Index As Integer = obj.Reference.ObjectNumber
            ' Get the old indirect object to be replaced!
            Dim old As PdfIndirectObject = Me(Index)
            If (old IsNot obj) Then
                old.DropFile() ' Disconnect the old indirect object.
            End If
            ' Insert the New indirect object into the modified objects collection!
            _modifiedObjects(Index) = obj
            ' Remove old indirect object from cache!
            _wokenObjects.Remove(Index)
            ' Mark the new indirect object as modified!
            obj.DropOriginal()
            Return old
        End Function

#End Region
#End Region
#End Region

    End Class

End Namespace
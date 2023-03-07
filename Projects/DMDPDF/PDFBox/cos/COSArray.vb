'/*
' * Licensed to the Apache Software Foundation (ASF) under one or more
' * contributor license agreements.  See the NOTICE file distributed with
' * this work for additional information regarding copyright ownership.
' * The ASF licenses this file to You under the Apache License, Version 2.0
' * (the "License"); you may not use this file except in compliance with
' * the License.  You may obtain a copy of the License at
' *
' *      http://www.apache.org/licenses/LICENSE-2.0
' *
' * Unless required by applicable law or agreed to in writing, software
' * distributed under the License is distributed on an "AS IS" BASIS,
' * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
' * See the License for the specific language governing permissions and
' * limitations under the License.
' */
Imports System.IO

Imports FinSeA.org.apache.pdfbox.exceptions
Imports FinSeA.org.apache.pdfbox.pdmodel.common

Namespace org.apache.pdfbox.cos


    '/**
    ' * An array of PDFBase objects as part of the PDF document.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.24 $
    ' */
    Public Class COSArray
        Inherits COSBase
        Implements Global.System.Collections.Generic.IEnumerable(Of COSBase)

        Private objects As New ArrayList '(Of COSBase)

        '/**
        ' * Constructor.
        ' */
        Public Sub New()
            '//default constructor
        End Sub

        '/**
        ' * This will add an object to the array.
        ' *
        ' * @param object The object to add to the array.
        ' */
        Public Sub add(ByVal [object] As COSBase)
            objects.add([object])
        End Sub

        '/**
        ' * This will add an object to the array.
        ' *
        ' * @param object The object to add to the array.
        ' */
        Public Sub add(ByVal [object] As COSObjectable)
            objects.add([object].getCOSObject())
        End Sub

        '/**
        ' * Add the specified object at the ith location and push the rest to the
        ' * right.
        ' *
        ' * @param i The index to add at.
        ' * @param object The object to add at that index.
        ' */
        Public Sub add(ByVal i As Integer, ByVal [object] As COSBase)
            objects.add(i, [object])
        End Sub

        '/**
        ' * This will remove all of the objects in the collection.
        ' */
        Public Sub clear()
            objects.clear()
        End Sub

        '/**
        ' * This will remove all of the objects in the collection.
        ' *
        ' * @param objectsList The list of objects to remove from the collection.
        ' */
        Public Sub removeAll(ByVal objectsList As ICollection(Of COSBase))
            objects.removeAll(objectsList)
        End Sub

        '/**
        ' * This will retain all of the objects in the collection.
        ' *
        ' * @param objectsList The list of objects to retain from the collection.
        ' */
        Public Sub retainAll(ByVal objectsList As ICollection(Of COSBase))
            objects.retainAll(objectsList)
        End Sub

        '/**
        ' * This will add an object to the array.
        ' *
        ' * @param objectsList The object to add to the array.
        ' */
        Public Sub addAll(ByVal objectsList As ICollection(Of COSBase))
            objects.addAll(objectsList)
        End Sub

        '/**
        ' * This will add all objects to this array.
        ' *
        ' * @param objectList The objects to add.
        ' */
        Public Sub addAll(ByVal objectList As COSArray)
            If (objectList IsNot Nothing) Then
                objects.addAll(objectList.objects)
            End If
        End Sub

        '/**
        ' * Add the specified object at the ith location and push the rest to the
        ' * right.
        ' *
        ' * @param i The index to add at.
        ' * @param objectList The object to add at that index.
        ' */
        Public Sub addAll(ByVal i As Integer, ByVal objectList As ICollection(Of COSBase))
            objects.addAll(i, objectList)
        End Sub

        '/**
        ' * This will set an object at a specific index.
        ' *
        ' * @param index zero based index into array.
        ' * @param object The object to set.
        ' */
        Public Sub [set](ByVal index As Integer, ByVal [object] As COSBase)
            objects.set(index, [object])
        End Sub

        '/**
        ' * This will set an object at a specific index.
        ' *
        ' * @param index zero based index into array.
        ' * @param intVal The object to set.
        ' */
        Public Sub [set](ByVal index As Integer, ByVal intVal As NInteger)
            objects.set(index, COSInteger.get(intVal))
        End Sub

        '/**
        ' * This will set an object at a specific index.
        ' *
        ' * @param index zero based index into array.
        ' * @param object The object to set.
        ' */
        Public Sub [set](ByVal index As Integer, ByVal [object] As COSObjectable)
            Dim base As COSBase = Nothing
            If ([object] IsNot Nothing) Then
                base = [object].getCOSObject()
            End If
            objects.set(index, base)
        End Sub

        '/**
        ' * This will get an object from the array.  This will dereference the object.
        ' * If the object is COSNull then null will be returned.
        ' *
        ' * @param index The index into the array to get the object.
        ' *
        ' * @return The object at the requested index.
        ' */
        Public Function getObject(ByVal index As Integer) As COSBase
            Dim obj As Object = objects.get(index)
            If (TypeOf (obj) Is COSObject) Then
                obj = CType(obj, COSObject).getObject()
            ElseIf (TypeOf (obj) Is COSNull) Then
                obj = Nothing
            End If
            Return CType(obj, COSBase)
        End Function

        '/**
        ' * This will get an object from the array.  This will NOT derefernce
        ' * the COS object.
        ' *
        ' * @param index The index into the array to get the object.
        ' *
        ' * @return The object at the requested index.
        ' */
        Public Function [get](ByVal index As Integer) As COSBase
            Return objects.get(index)
        End Function

        '/**
        ' * Get the value of the array as an integer.
        ' *
        ' * @param index The index into the list.
        ' *
        ' * @return The value at that index or -1 if it is null.
        ' */
        Public Function getInt(ByVal index As Integer) As Integer
            Return getInt(index, -1)
        End Function

        '/**
        ' * Get the value of the array as an integer, return the default if it does
        ' * not exist.
        ' *
        ' * @param index The value of the array.
        ' * @param defaultValue The value to return if the value is null.
        ' * @return The value at the index or the defaultValue.
        ' */
        Public Function getInt(ByVal index As Integer, ByVal defaultValue As Integer) As Integer
            Dim retval As Integer = defaultValue
            If (index < size()) Then
                Dim obj As Object = objects.get(index)
                If (TypeOf (obj) Is COSNumber) Then
                    retval = CType(obj, COSNumber).intValue()
                End If
            End If
            Return retval
        End Function

        '/**
        ' * Set the value in the array as an integer.
        ' *
        ' * @param index The index into the array.
        ' * @param value The value to set.
        ' */
        Public Sub setInt(ByVal index As Integer, ByVal value As Integer)
            [set](index, COSInteger.get(value))
        End Sub

        '/**
        ' * Set the value in the array as a name.
        ' * @param index The index into the array.
        ' * @param name The name to set in the array.
        ' */
        Public Sub setName(ByVal index As Integer, ByVal name As String)
            [set](index, COSName.getPDFName(name))
        End Sub

        '/**
        ' * Get the value of the array as a string.
        ' *
        ' * @param index The index into the array.
        ' * @return The name converted to a string or null if it does not exist.
        ' */
        Public Function getName(ByVal index As Integer) As String
            Return getName(index, Nothing)
        End Function

        '/**
        ' * Get an entry in the array that is expected to be a COSName.
        ' * @param index The index into the array.
        ' * @param defaultValue The value to return if it is null.
        ' * @return The value at the index or defaultValue if none is found.
        ' */
        Public Function getName(ByVal index As Integer, ByVal defaultValue As String) As String
            Dim retval As String = defaultValue
            If (index < size()) Then
                Dim obj As Object = objects.get(index)
                If (TypeOf (obj) Is COSName) Then
                    retval = CType(obj, COSName).getName()
                End If
            End If
            Return retval
        End Function

        '/**
        ' * Set the value in the array as a string.
        ' * @param index The index into the array.
        ' * @param string The string to set in the array.
        ' */
        Public Sub setString(ByVal index As Integer, ByVal [string] As String)
            If ([string] <> vbNullString) Then
                [set](index, New COSString([string]))
            Else
                [set](index, DirectCast(Nothing, COSObjectable))
            End If
        End Sub

        '/**
        ' * Get the value of the array as a string.
        ' *
        ' * @param index The index into the array.
        ' * @return The string or null if it does not exist.
        ' */
        Public Function getString(ByVal index As Integer) As String
            Return getString(index, Nothing)
        End Function

        '/**
        ' * Get an entry in the array that is expected to be a COSName.
        ' * @param index The index into the array.
        ' * @param defaultValue The value to return if it is null.
        ' * @return The value at the index or defaultValue if none is found.
        ' */
        Public Function getString(ByVal index As Integer, ByVal defaultValue As String) As String
            Dim retval As String = defaultValue
            If (index < size()) Then
                Dim obj As Object = objects.get(index)
                If (TypeOf (obj) Is COSString) Then
                    retval = CType(obj, COSString).getString()
                End If
            End If
            Return retval
        End Function

        '/**
        ' * This will get the size of this array.
        ' *
        ' * @return The number of elements in the array.
        ' */
        Public Function size() As Integer
            Return objects.size()
        End Function

        '/**
        ' * This will remove an element from the array.
        ' *
        ' * @param i The index of the object to remove.
        ' *
        ' * @return The object that was removed.
        ' */
        Public Function remove(ByVal i As Integer) As COSBase
            Return objects.remove(i)
        End Function

        '/**
        ' * This will remove an element from the array.
        ' *
        ' * @param o The object to remove.
        ' *
        ' * @return <code>true</code> if the object was removed, <code>false</code>
        ' *  otherwise
        ' */
        Public Function remove(ByVal o As COSBase) As Boolean
            Return objects.remove(o)
        End Function

        '/**
        ' * This will remove an element from the array.
        ' * This method will also remove a reference to the object.
        ' *
        ' * @param o The object to remove.
        ' * @return <code>true</code> if the object was removed, <code>false</code>
        ' *  otherwise
        ' */
        Public Function removeObject(ByVal o As COSBase) As Boolean
            Dim removed As Boolean = Me.remove(o)
            If (Not removed) Then
                For i As Integer = 0 To Me.size() - 1
                    Dim entry As COSBase = Me.[get](i)
                    If (TypeOf (entry) Is COSObject) Then
                        Dim objEntry As COSObject = entry
                        If (objEntry.getObject().Equals(o)) Then
                            Return Me.remove(entry)
                        End If
                    End If
                Next
            End If
            Return removed
        End Function

        '/**
        ' * {@inheritDoc}
        ' */
        '@Override
        Public Overrides Function toString() As String
            Return "COSArray{" & Me.objects.ToString & "}"
        End Function

        '/**
        ' * Get access to the list.
        ' *
        ' * @return an iterator over the array elements
        ' */
        Public Function iterator() As Global.System.Collections.Generic.IEnumerator(Of COSBase) Implements Global.System.Collections.Generic.IEnumerable(Of FinSeA.org.apache.pdfbox.cos.COSBase).GetEnumerator
            Return objects.iterator()
        End Function

        '/**
        ' * This will return the index of the entry or -1 if it is not found.
        ' *
        ' * @param object The object to search for.
        ' * @return The index of the object or -1.
        ' */
        Public Function indexOf(ByVal [object] As COSBase) As Integer
            For i As Integer = 0 To size() - 1
                If ([get](i).Equals([object])) Then
                    Return i
                End If
            Next
            Return -1
        End Function

        '/**
        ' * This will return the index of the entry or -1 if it is not found.
        ' * This method will also find references to indirect objects.
        ' *
        ' * @param object The object to search for.
        ' * @return The index of the object or -1.
        ' */
        Public Function indexOfObject(ByVal [object] As COSBase) As Integer
            Dim retval As Integer = -1
            For i As Integer = 0 To Me.size() - 1
                If (retval >= 0) Then Exit For
                Dim item As COSBase = Me.get(i)
                If (item.Equals([object])) Then
                    retval = i
                    Exit For
                ElseIf (TypeOf (item) Is COSObject) Then
                    If ((CType(item, COSObject)).getObject().Equals([object])) Then
                        retval = i
                        Exit For
                    End If
                End If
            Next
            Return retval
        End Function

        '/**
        ' * This will add null values until the size of the array is at least
        ' * as large as the parameter.  If the array is already larger than the
        ' * parameter then nothing is done.
        ' *
        ' * @param size The desired size of the array.
        ' */
        Public Sub growToSize(ByVal size As Integer)
            growToSize(size, Nothing)
        End Sub

        '/**
        ' * This will add the object until the size of the array is at least
        ' * as large as the parameter.  If the array is already larger than the
        ' * parameter then nothing is done.
        ' *
        ' * @param size The desired size of the array.
        ' * @param object The object to fill the array with.
        ' */
        Public Sub growToSize(ByVal size As Integer, ByVal [object] As COSBase)
            While (size < Me.size())
                add([object])
            End While
        End Sub

        '/**
        ' * visitor pattern double dispatch method.
        ' *
        ' * @param visitor The object to notify when visiting this object.
        ' * @return any object, depending on the visitor implementation, or null
        ' * @throws COSVisitorException If an error occurs while visiting this object.
        ' */
        Public Overrides Function accept(ByVal visitor As ICOSVisitor) As Object 'throws COSVisitorException
            Return visitor.visitFromArray(Me)
        End Function

        '/**
        ' * This will take an COSArray of numbers and convert it to a Single().
        ' *
        ' * @return This COSArray as an array of Single numbers.
        ' */
        Public Function toFloatArray() As Single()
            Dim retval() As Single
            ReDim retval(Me.size() - 1)
            For i As Integer = 0 To Me.size() - 1
                retval(i) = CType(getObject(i), COSNumber).floatValue()
            Next
            Return retval
        End Function

        '/**
        ' * Clear the current contents of the COSArray and set it with the Single().
        ' *
        ' * @param value The new value of the Single array.
        ' */
        Public Sub setFloatArray(ByVal value() As Single)
            Me.clear()
            For i As Integer = 0 To value.Length - 1
                Me.add(New COSFloat(value(i)))
            Next
        End Sub

        '/**
        ' *  Return contents of COSArray as a Java List.
        ' *
        ' *  @return the COSArray as List
        ' */
        Public Function toList() As System.Collections.IList
            Dim retList As New System.Collections.Generic.List(Of COSBase)(size())
            For i As Integer = 0 To size() - 1
                retList.Add(Me.[get](i))
            Next
            Return retList
        End Function

        Public Function GetEnumerator() As Global.System.Collections.IEnumerator Implements Global.System.Collections.IEnumerable.GetEnumerator
            Return Me.objects.GetEnumerator
        End Function

    End Class

End Namespace
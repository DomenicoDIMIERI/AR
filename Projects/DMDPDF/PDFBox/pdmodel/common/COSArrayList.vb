Imports FinSeA.org.apache.pdfbox.cos
Imports System.IO
Imports FinSeA

Namespace org.apache.pdfbox.pdmodel.common

    '/**
    ' * This is an implementation of a List that will sync its contents to a COSArray.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.15 $
    ' */
    Public Class COSArrayList
        Implements List

        Private array As COSArray
        Private actual As List

        Private parentDict As COSDictionary
        Private dictKey As COSName

        '/**
        ' * Default constructor.
        ' */
        Public Sub New()
            array = New COSArray()
            actual = New ArrayList
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param actualList The list of standard java objects
        ' * @param cosArray The COS array object to sync to.
        ' */
        Public Sub New(ByVal actualList As List, ByVal cosArray As COSArray)
            actual = actualList
            array = cosArray
        End Sub

        '/**
        ' * This is a really special constructor.  Sometimes the PDF spec says
        ' * that a dictionary entry can either be a single item or an array of those
        ' * items.  But in the PDModel interface we really just want to always return
        ' * a java.util.List.  In the case were we get the list and never modify it
        ' * we don't want to convert to COSArray and put one element, unless we append
        ' * to the list.  So here we are going to create this object with a single
        ' * item instead of a list, but allow more items to be added and then converted
        ' * to an array.
        ' *
        ' * @param actualObject The PDModel object.
        ' * @param item The COS Model object.
        ' * @param dictionary The dictionary that holds the item, and will hold the array if an item is added.
        ' * @param dictionaryKey The key into the dictionary to set the item.
        ' */
        Public Sub New(ByVal actualObject As Object, ByVal item As COSBase, ByVal dictionary As COSDictionary, ByVal dictionaryKey As COSName)
            array = New COSArray()
            array.add(item)
            actual = New ArrayList
            actual.add(actualObject)

            parentDict = dictionary
            dictKey = dictionaryKey
        End Sub

        '/**
        ' * @deprecated use the {@link #COSArrayList(Object, COSBase, COSDictionary, COSName)} method instead
        ' */
        <Obsolete("use the {@link #COSArrayList(Object, COSBase, COSDictionary, COSName)} method instead")> _
        Public Sub New(ByVal actualObject As Object, ByVal item As COSBase, ByVal dictionary As COSDictionary, ByVal dictionaryKey As String)
            Me.New(actualObject, item, dictionary, COSName.getPDFName(dictionaryKey))
        End Sub

        Public Function size() As Integer Implements List.size
            Return actual.size()
        End Function

        Public Function isEmpty() As Boolean Implements List.isEmpty
            Return actual.isEmpty()
        End Function

        Public Function contains(ByVal o As Object) As Boolean Implements List.contains
            Return actual.contains(o)
        End Function

        Public Function iterator() As Iterator Implements List.iterator
            Return actual.GetEnumerator()
        End Function

        Public Function toArray() As Object() Implements List.toArray
            Return actual.toArray()
        End Function

        '/**
        ' * {@inheritDoc}
        ' */
        'public <X>X[] toArray(X[] a)
        '{
        '    return actual.toArray(a);

        '}

        Public Function add(ByVal elem As Object) As Boolean Implements List.add
            Dim o As Object = elem
            'when adding if there is a parentDict then change the item
            'in the dictionary from a single item to an array.
            If (parentDict IsNot Nothing) Then
                parentDict.setItem(dictKey, array)
                'clear the parent dict so it doesn't happen again, there might be
                'a usecase for keeping the parentDict around but not now.
                parentDict = Nothing
            End If
            'string is a special case because we can't subclass to be COSObjectable
            If (TypeOf (o) Is String) Then
                array.add(New COSString(CStr(o)))
            ElseIf (TypeOf (o) Is DualCOSObjectable) Then
                Dim dual As DualCOSObjectable = o
                array.add(dual.getFirstCOSObject())
                array.add(dual.getSecondCOSObject())
            Else
                If (array IsNot Nothing) Then
                    array.add(DirectCast(o, COSObjectable).getCOSObject())
                End If
            End If
            Return actual.add(o)
        End Function

        Public Function remove(ByVal o As Object) As Boolean Implements List.remove
            Dim retval As Boolean = True
            Dim index As Integer = actual.indexOf(o)
            If (index >= 0) Then
                actual.remove(index)
                array.remove(index)
            Else
                retval = False
            End If
            Return retval
        End Function

        Public Function containsAll(ByVal c As ICollection) As Boolean Implements List.containsAll
            Return actual.containsAll(c)
        End Function

        Public Overloads Function addAll(ByVal c As ICollection) As Boolean Implements ICollection.addAll
            'when adding if there is a parentDict then change the item
            'in the dictionary from a single item to an array.
            If (parentDict IsNot Nothing) AndAlso (c.size() > 0) Then
                parentDict.setItem(dictKey, array)
                'clear the parent dict so it doesn't happen again, there might be
                'a usecase for keeping the parentDict around but not now.
                parentDict = Nothing
            End If
            array.addAll(toCOSObjectList(c))
            Return actual.addAll(c)
        End Function

        Public Overloads Function addAll(ByVal index As Integer, ByVal c As ICollection) As Boolean Implements List.addAll
            'when adding if there is a parentDict then change the item
            'in the dictionary from a single item to an array.
            If (parentDict IsNot Nothing) AndAlso c.size() > 0 Then
                parentDict.setItem(dictKey, array)
                'clear the parent dict so it doesn't happen again, there might be
                'a usecase for keeping the parentDict around but not now.
                parentDict = Nothing
            End If

            If (c.size() > 0 AndAlso TypeOf (c.toArray()(0)) Is DualCOSObjectable) Then
                array.addAll(index * 2, toCOSObjectList(c))
            Else
                array.addAll(index, toCOSObjectList(c))
            End If
            Return actual.addAll(index, c)
        End Function

        '/**
        ' * This will take an array of COSNumbers and return a COSArrayList of
        ' * java.lang.Integer values.
        ' *
        ' * @param intArray The existing integer Array.
        ' *
        ' * @return A list that is part of the core Java collections.
        ' */
        Public Shared Function convertIntegerCOSArrayToList(ByVal intArray As COSArray) As List(Of NInteger)
            Dim retval As List(Of NInteger) = Nothing
            If (intArray IsNot Nothing) Then
                Dim numbers As List(Of NInteger) = New ArrayList(Of NInteger)
                For i As Integer = 0 To intArray.size() - 1
                    numbers.add(DirectCast(intArray.get(i), COSNumber).intValue())
                Next
                retval = New COSArrayList(Of NInteger)(numbers, intArray)
            End If
            Return retval
        End Function

        '/**
        ' * This will take an array of COSNumbers and return a COSArrayList of
        ' * java.lang.NFloat values.
        ' *
        ' * @param floatArray The existing Single Array.
        ' *
        ' * @return The list of NFloat objects.
        ' */
        Public Shared Function convertFloatCOSArrayToList(ByVal floatArray As COSArray) As List(Of Nullable(Of Single))
            Dim retval As List(Of Nullable(Of Single)) = Nothing
            If (floatArray IsNot Nothing) Then
                Dim numbers As List(Of Nullable(Of Single)) = New ArrayList(Of Nullable(Of Single))
                For i As Integer = 0 To floatArray.size() - 1
                    numbers.add(DirectCast(floatArray.get(i), COSNumber).floatValue())
                Next
                retval = New COSArrayList(Of Nullable(Of Single))(numbers, floatArray)
            End If
            Return retval
        End Function

        '/**
        ' * This will take an array of COSName and return a COSArrayList of
        ' * java.lang.String values.
        ' *
        ' * @param nameArray The existing name Array.
        ' *
        ' * @return The list of String objects.
        ' */
        Public Shared Function convertCOSNameCOSArrayToList(ByVal nameArray As COSArray) As List(Of String)
            Dim retval As List(Of String) = Nothing
            If (nameArray IsNot Nothing) Then
                Dim names As List(Of String) = New ArrayList(Of String)
                For i As Integer = 0 To nameArray.size() - 1
                    names.add(DirectCast(nameArray.getObject(i), COSName).getName())
                Next
                retval = New COSArrayList(Of String)(names, nameArray)
            End If
            Return retval
        End Function

        '/**
        ' * This will take an array of COSString and return a COSArrayList of
        ' * java.lang.String values.
        ' *
        ' * @param stringArray The existing name Array.
        ' *
        ' * @return The list of String objects.
        ' */
        Public Shared Function convertCOSStringCOSArrayToList(ByVal stringArray As COSArray) As List(Of String)
            Dim retval As List(Of String) = Nothing
            If (stringArray IsNot Nothing) Then
                Dim [string] As List(Of String) = New ArrayList(Of String)
                For i As Integer = 0 To stringArray.size() - 1
                    [string].add(DirectCast(stringArray.getObject(i), COSString).getString())
                Next
                retval = New COSArrayList([string], stringArray)
            End If
            Return retval
        End Function

        '/**
        ' * This will take an list of string objects and return a COSArray of COSName
        ' * objects.
        ' *
        ' * @param strings A list of strings
        ' *
        ' * @return An array of COSName objects
        ' */
        Public Shared Function convertStringListToCOSNameCOSArray(ByVal strings As List(Of String)) As COSArray
            Dim retval As COSArray = New COSArray()
            For i As Integer = 0 To strings.size() - 1
                retval.add(COSName.getPDFName(strings.get(i)))
            Next
            Return retval
        End Function

        '/**
        ' * This will take an list of string objects and return a COSArray of COSName
        ' * objects.
        ' *
        ' * @param strings A list of strings
        ' *
        ' * @return An array of COSName objects
        ' */
        Public Shared Function convertStringListToCOSStringCOSArray(ByVal strings As List(Of String)) As COSArray
            Dim retval As COSArray = New COSArray()
            For i As Integer = 0 To strings.size() - 1
                retval.add(New COSString(strings.get(i)))
            Next
            Return retval
        End Function

        '/**
        ' * This will convert a list of COSObjectables to an
        ' * array list of COSBase objects.
        ' *
        ' * @param cosObjectableList A list of COSObjectable.
        ' *
        ' * @return A list of COSBase.
        ' */
        Public Shared Function converterToCOSArray(ByVal cosObjectableList As Global.System.Collections.IEnumerable) As COSArray
            Dim array As COSArray = Nothing
            If (cosObjectableList IsNot Nothing) Then
                If (TypeOf (cosObjectableList) Is COSArrayList) Then
                    'if it is already a COSArrayList then we don't want to recreate the array, we want to reuse it.
                    array = DirectCast(cosObjectableList, COSArrayList).array

                Else
                    array = New COSArray()
                    'Dim iter as = cosObjectableList.iterator();
                    'While (iter.hasNext())
                    For Each [next] As Object In cosObjectableList
                        'Object next = iter.next();
                        If (TypeOf ([next]) Is String) Then
                            array.add(New COSString(CStr([next])))
                        ElseIf (TypeOf ([next]) Is NInteger OrElse TypeOf ([next]) Is Nullable(Of Long)) Then
                            array.add(COSInteger.get(CLng([next])))
                        ElseIf (TypeOf ([next]) Is Nullable(Of Single) OrElse TypeOf ([next]) Is Nullable(Of Double)) Then
                            array.add(New COSFloat(CSng([next])))
                        ElseIf (TypeOf ([next]) Is COSObjectable) Then
                            Dim [object] As COSObjectable = [next]
                            array.add([object].getCOSObject())
                        ElseIf (TypeOf ([next]) Is DualCOSObjectable) Then
                            Dim [object] As DualCOSObjectable = [next]
                            array.add([object].getFirstCOSObject())
                            array.add([object].getSecondCOSObject())
                        ElseIf ([next] Is Nothing) Then
                            array.add(COSNull.NULL)
                        Else
                            Throw New RuntimeException("Error: Don't know how to convert type to COSBase '" & [next].GetType().Name & "'")
                        End If
                    Next
                End If
            End If
            Return array
        End Function

        Private Function toCOSObjectList(ByVal list As Global.System.Collections.IEnumerable) As List(Of COSBase)
            Dim cosObjects As List(Of COSBase) = New ArrayList(Of COSBase)
            For Each [next] In list
                If (TypeOf ([next]) Is String) Then
                    cosObjects.add(New COSString(CStr([next])))
                ElseIf (TypeOf ([next]) Is DualCOSObjectable) Then
                    Dim [object] As DualCOSObjectable = [next]
                    array.add([object].getFirstCOSObject())
                    array.add([object].getSecondCOSObject())
                Else
                    Dim cos As COSObjectable = [next]
                    cosObjects.add(cos.getCOSObject())
                End If
            Next
            Return cosObjects
        End Function

        Public Function removeAll(ByVal c As ICollection) As Boolean Implements List.removeAll
            array.removeAll(toCOSObjectList(c))
            Return actual.removeAll(c)
        End Function

        Public Function retainAll(ByVal c As ICollection) As Boolean Implements List.retainAll
            array.retainAll(toCOSObjectList(c))
            Return actual.retainAll(c)
        End Function

        Public Sub clear() Implements List.clear
            'when adding if there is a parentDict then change the item
            'in the dictionary from a single item to an array.
            If (parentDict IsNot Nothing) Then
                parentDict.setItem(dictKey, DirectCast(Nothing, COSBase))
            End If
            actual.clear()
            array.clear()
        End Sub

        Public Overrides Function equals(ByVal o As Object) As Boolean Implements List.equals
            Return actual.equals(o)
        End Function

        Public Overrides Function GetHashCode() As Integer
            Return Me.actual.GetHashCode
        End Function


        Public Function [get](ByVal index As Integer) As Object Implements List.get
            Return actual.get(index)
        End Function

        Public Function [set](ByVal index As Integer, ByVal elem As Object) As Object Implements List.set
            Dim element As Object = elem
            If (TypeOf (element) Is String) Then
                Dim item As COSString = New COSString(CStr(element))
                If (parentDict IsNot Nothing AndAlso index = 0) Then
                    parentDict.setItem(dictKey, item)
                End If
                array.set(index, item)
            ElseIf (TypeOf (element) Is DualCOSObjectable) Then
                Dim dual As DualCOSObjectable = element
                array.set(index * 2, dual.getFirstCOSObject())
                array.set(index * 2 + 1, dual.getSecondCOSObject())
            Else
                If (parentDict IsNot Nothing AndAlso index = 0) Then
                    parentDict.setItem(dictKey, DirectCast(element, COSObjectable).getCOSObject())
                End If
                array.set(index, DirectCast(element, COSObjectable).getCOSObject())
            End If
            Return actual.[set](index, element)
        End Function

        Public Sub add(ByVal index As Integer, ByVal elem As Object) Implements List.add
            Dim element As Object = elem
            'when adding if there is a parentDict then change the item
            'in the dictionary from a single item to an array.
            If (parentDict IsNot Nothing) Then
                parentDict.setItem(dictKey, array)
                'clear the parent dict so it doesn't happen again, there might be
                'a usecase for keeping the parentDict around but not now.
                parentDict = Nothing
            End If
            actual.add(index, element)
            If (TypeOf (element) Is String) Then
                array.add(index, New COSString(CStr(element)))
            ElseIf (TypeOf (element) Is DualCOSObjectable) Then
                Dim dual As DualCOSObjectable = element
                array.add(index * 2, dual.getFirstCOSObject())
                array.add(index * 2 + 1, dual.getSecondCOSObject())
            Else
                array.add(index, DirectCast(element, COSObjectable).getCOSObject())
            End If
        End Sub

        Public Function remove(ByVal index As Integer) As Object Implements List.remove
            If (array.size() > index AndAlso TypeOf (array.get(index)) Is DualCOSObjectable) Then
                'remove both objects
                array.remove(index)
                array.remove(index)
            Else
                array.remove(index)
            End If
            Return actual.remove(index)
        End Function

        Public Function indexOf(ByVal o As Object) As Integer Implements List.indexOf
            Return actual.indexOf(o)
        End Function

        Public Function lastIndexOf(ByVal o As Object) As Integer Implements List.lastIndexOf
            Return actual.lastIndexOf(o) '.indexOf(o)
        End Function


        'public ListIterator<Object> listIterator()
        '{
        '    return actual.listIterator();
        '}

        '/**
        ' * {@inheritDoc}
        ' */
        'public ListIterator<Object> listIterator(int index)
        '{
        '    return actual.listIterator( index );
        '}
        Public Function GetEnumerator() As Global.System.Collections. IEnumerator Implements List.GetEnumerator
            Return actual.GetEnumerator
        End Function

        Public Function subList(ByVal fromIndex As Integer, ByVal toIndex As Integer) As List Implements List.subList
            Return actual.subList(fromIndex, toIndex)
        End Function


        Public Overrides Function toString() As String
            Return "COSArrayList{" & array.toString() & "}"
        End Function

        Public Function toList() As COSArray
            Return array
        End Function

        Public Function hashCode() As Integer Implements List.hashCode
            Return Me.GetHashCode
        End Function



        Public Function toArray(Of T)() As T() Implements List.toArray
            Return actual.toArray(Of T)()
        End Function

       

    End Class

End Namespace
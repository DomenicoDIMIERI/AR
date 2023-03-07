Namespace java
    ''' <summary>
    '''  The Vector class implements a growable array of objects. Like an array, it contains components that can be accessed using an integer index. However, the size of a Vector can grow or shrink as needed to accommodate adding and removing items after the Vector has been created.
    '''  Each vector tries to optimize storage management by maintaining a capacity and a capacityIncrement. The capacity is always at least as large as the vector size; it is usually larger because as components are added to the vector, the vector's storage increases in chunks the size of capacityIncrement. An application can increase the capacity of a vector before inserting a large number of components; this reduces the amount of incremental reallocation.
    '''  The Iterators returned by Vector's iterator and listIterator methods are fail-fast: if the Vector is structurally modified at any time after the Iterator is created, in any way except through the Iterator's own remove or add methods, the Iterator will throw a ConcurrentModificationException. Thus, in the face of concurrent modification, the Iterator fails quickly and cleanly, rather than risking arbitrary, non-deterministic behavior at an undetermined time in the future. The Enumerations returned by Vector's elements method are not fail-fast.
    '''  Note that the fail-fast behavior of an iterator cannot be guaranteed as it is, generally speaking, impossible to make any hard guarantees in the presence of unsynchronized concurrent modification. Fail-fast iterators throw ConcurrentModificationException on a best-effort basis. Therefore, it would be wrong to write a program that depended on this exception for its correctness: the fail-fast behavior of iterators should be used only to detect bugs.
    '''  As of the Java 2 platform v1.2, this class was retrofitted to implement the List interface, making it a member of the Java Collections Framework. Unlike the new collection implementations, Vector is synchronized. 
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Vector(Of E)
        Implements List(Of E)

        '    Serializable, Cloneable, Iterable<E>, Collection<E>, List<E>, RandomAccess


        ''' <summary>
        '''  The amount by which the capacity of the vector is automatically incremented when its size becomes greater than its capacity.
        ''' </summary>
        ''' <remarks></remarks>
        Protected capacityIncrement As Integer

        ''' <summary>
        ''' The number of valid components in this Vector object.
        ''' </summary>
        ''' <remarks></remarks>
        Protected elementCount As Integer

        ''' <summary>
        '''   The array buffer into which the components of the vector are stored.
        ''' </summary>
        ''' <remarks></remarks>
        Protected elementData() As E

        ''' <summary>
        ''' Constructs an empty vector so that its internal data array has size 10 and its standard capacity increment is zero.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
        End Sub

        ''' <summary>
        '''  Constructs a vector containing the elements of the specified collection, in the order they are returned by the collection's iterator.
        ''' </summary>
        ''' <param name="c"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal c As Global.System.Collections.Generic.IEnumerable(Of E))
            For Each item As Object In c
                Me.add(c)
            Next
        End Sub

        ''' <summary>
        ''' Constructs an empty vector with the specified initial capacity and with its capacity increment equal to zero.
        ''' </summary>
        ''' <param name="initialCapacity"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal initialCapacity As Integer)
            Me.elementData = Array.CreateInstance(GetType(Object), initialCapacity)
            Me.capacityIncrement = 0
            Me.elementCount = 0
        End Sub

        ''' <summary>
        ''' Constructs an empty vector with the specified initial capacity and capacity increment.
        ''' </summary>
        ''' <param name="initialCapacity"></param>
        ''' <param name="capacityIncrement"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal initialCapacity As Integer, ByVal capacityIncrement As Integer)
            Me.elementData = Array.CreateInstance(GetType(Object), initialCapacity)
            Me.capacityIncrement = capacityIncrement
            Me.elementCount = 0
        End Sub

        Public Function add(ByVal e As E) As Boolean Implements ICollection(Of E).add
            If (Me.elementData Is Nothing) Then
                Me.elementData = {e}
                Me.elementCount = 1
            Else
                If (Me.elementCount >= 1 + UBound(Me.elementData)) Then
                    ReDim Me.elementData(Me.elementCount)
                End If
                Me.elementData(Me.elementCount) = e
                Me.elementCount += 1
            End If
            Return True
        End Function

        Public Function _add(o As E) As Boolean Implements List(Of E).add
            Return Me.add(elements)
        End Function

        Private Function _addAll(col As ICollection(Of E)) As Boolean Implements List(Of E).addAll
            Return Me.addAll(col)
        End Function

        Private Function _remove(o As E) As Boolean Implements List(Of E).remove
            Return Me.remove(o)
        End Function

        Public Sub add(index As Integer, o As E) Implements List(Of E).add
            Throw New NotImplementedException
        End Sub


        Public Function [get](i As Integer) As E Implements List(Of E).get
            Return Me.elementData(i)
        End Function

        Public Function size() As Integer Implements List.size
            Return Me.elementCount
        End Function

        Public Function isEmpty() As Boolean Implements List.isEmpty
            Return Me.elementCount = 0
        End Function

        Public Function toArray() As Object() Implements List.toArray
            Return Me.elementData
        End Function



        'Public Function remove(o As Object) As Boolean Implements List.remove
        '    Dim i As Integer = Me.m_Base.IndexOf(o)
        '    If (i >= 0) Then
        '        Me.m_Base.Remove(o)
        '        Return True
        '    Else
        '        Return False
        '    End If
        'End Function

        'Public Function containsAll(col As IEnumerable) As Boolean Implements List.containsAll
        '    For Each item As Object In col
        '        If Me.contains(item) = False Then Return False
        '    Next
        '    Return True
        'End Function

        'Public Function addAll(col As IEnumerable) As Boolean Implements List.addAll
        '    For Each item As Object In col
        '        Me.m_Base.Add(item)
        '    Next
        '    Return True
        'End Function

        'Public Function addAll(fromIndex As Integer, col As IEnumerable) As Boolean Implements List.addAll
        '    For Each o As Object In col
        '        Me.m_Base.Insert(fromIndex, o)
        '        fromIndex += 1
        '    Next
        '    Return True
        'End Function

        'Public Function indexOf(o As Object) As Integer Implements List.indexOf
        '    Return Me.m_Base.IndexOf(o)
        'End Function

        'Public Function lastIndexOf(o As Object) As Integer Implements List.lastIndexOf
        '    Return Me.m_Base.LastIndexOf(o)
        'End Function

        'Public Function remove(index As Integer) As Object Implements List.remove
        '    Dim obj As Object = Me.m_Base(index)
        '    Me.m_Base.RemoveAt(index)
        '    Return obj
        'End Function



        'Public Function [set](index As Integer, element As Object) As Object Implements List.set
        '    Me.m_Base(index) = element
        '    Return element
        'End Function

        'Public Function subList(fromIndex As Integer, toIndex As Integer) As List Implements List.subList
        '    Dim ret As New ArrayList
        '    For i As Integer = fromIndex To toIndex - 1
        '        ret.add(Me.m_Base(i))
        '    Next
        '    Return ret
        'End Function

        'Public Sub clear() Implements List.clear
        '    Me.m_Base.Clear()
        'End Sub

        'Public Overrides Function equals(o As Object) As Boolean Implements List.equals
        '    Return Me.m_Base.Equals(o)
        'End Function

        'Public Function hashCode() As Integer Implements List.hashCode
        '    Return Me.GetHashCode
        'End Function

        'Public Overrides Function GetHashCode() As Integer
        '    Return Me.m_Base.GetHashCode
        'End Function

        'Public Function iterator() As Global.System.Collections.IEnumerator
        '    Return Me.m_Base.GetEnumerator
        'End Function

        'Public Function removeAll(col As IEnumerable) As Boolean
        '    Dim ret As Boolean = True
        '    For Each item As Object In col
        '        Dim i As Integer = Me.m_Base.IndexOf(item)
        '        If (i > 0) Then
        '            Me.m_Base.RemoveAt(i)
        '        Else
        '            ret = False
        '        End If
        '    Next
        '    Return ret
        'End Function

        'Public Function retainAll(col As IEnumerable) As Boolean
        '    Dim ret As Boolean = False
        '    Dim i = 0, j As Integer = 0
        '    While (i < Me.size())
        '        j = -1
        '        For Each item As Object In col
        '            If Me.m_Base(i).Equals(item) Then
        '                j = i
        '                ret = True
        '                Exit For
        '            End If
        '        Next
        '        If (j = -1) Then
        '            Me.m_Base.RemoveAt(i)
        '        Else
        '            i += 1
        '        End If
        '    End While
        '    Return ret
        'End Function

        'Public Function toArray(Of T)() As T() Implements List.toArray
        '    Return Me.m_Base.ToArray(GetType(T))
        'End Function

        Private Sub _add(index As Integer, o As Object) Implements List.add
            Me.add(index, o)
        End Sub



        Public Sub clear() Implements List.clear
            For i As Integer = 0 To Me.elementCount - 1
                Me.elementData(i) = Nothing
            Next
            Me.elementCount = 0
        End Sub

        Private Function contains1(o As Object) As Boolean Implements List.contains
            Return Me.contains(o)
        End Function

        Private Function containsAll1(col As ICollection) As Boolean Implements ICollection.containsAll
            Return Me.containsAll(col)
        End Function

        Public Overrides Function equals(o As Object) As Boolean Implements List.equals
            Throw New NotImplementedException
        End Function

        Private Function get1(i As Integer) As Object Implements List.get
            Return Me.get(i)
        End Function

        Public Function hashCode() As Integer Implements List.hashCode
            Return Me.GetHashCode
        End Function

        Private Function indexOf1(o As Object) As Integer Implements List.indexOf
            Return Me.indexOf(o)
        End Function

        Public Function iterator() As Iterator Implements List.iterator
            Return New Collections.GenericIterator(Of E)(Me)
        End Function

        Private Function lastIndexOf1(o As Object) As Integer Implements List.lastIndexOf
            Return Me.lastIndexOf(o)
        End Function

        Private Function remove1(index As Integer) As Object Implements List.remove
            Return Me.remove(index)
        End Function

        Private Function remove1(o As Object) As Boolean Implements ICollection.remove
            Dim tmp As E = o
            Return Me.remove(tmp)
        End Function

        Private Function removeAll1(col As ICollection) As Boolean Implements ICollection.removeAll
            Return Me.removeAll(col)
        End Function

        Private Function retainAll1(col As ICollection) As Boolean Implements ICollection.retainAll
            Return Me.retainAll(col)
        End Function

        Public Function [set1](index As Integer, element As Object) As Object Implements List.set
            Return Me.set(index, element)
        End Function

        Private Function subList1(fromIndex As Integer, toIndex As Integer) As List Implements List.subList
            Return Me.subList(fromIndex, toIndex)
        End Function

        Public Function toArray(Of T)() As T() Implements List.toArray
            Throw New NotImplementedException
        End Function

        Public Function addAll(fromIndex As Integer, col As ICollection(Of E)) As Boolean Implements List(Of E).addAll
            Throw New NotImplementedException
        End Function

        Public Function addAll(col As ICollection(Of E)) As Boolean Implements ICollection(Of E).addAll
            Throw New NotImplementedException
        End Function

        Public Function contains(o As E) As Boolean Implements List(Of E).contains
            Throw New NotImplementedException
        End Function

        Public Function containsAll(col As ICollection(Of E)) As Boolean Implements ICollection(Of E).containsAll
            Throw New NotImplementedException
        End Function

        Public Function indexOf(o As E) As Integer Implements List(Of E).indexOf
            Throw New NotImplementedException
        End Function

        Public Function lastIndexOf(o As E) As Integer Implements List(Of E).lastIndexOf
            Throw New NotImplementedException
        End Function

        Public Function remove(index As Integer) As E Implements List(Of E).remove
            Throw New NotImplementedException
        End Function

        Public Function remove(o As E) As Boolean Implements ICollection(Of E).remove
            Throw New NotImplementedException
        End Function

        Public Function removeAll(col As ICollection(Of E)) As Boolean Implements ICollection(Of E).removeAll
            Throw New NotImplementedException
        End Function

        Public Function retainAll(col As ICollection(Of E)) As Boolean Implements ICollection(Of E).retainAll
            Throw New NotImplementedException
        End Function

        Public Function [set](index As Integer, element As E) As E Implements List(Of E).set
            Throw New NotImplementedException
        End Function

        Public Function subList(fromIndex As Integer, toIndex As Integer) As List(Of E) Implements List(Of E).subList
            Throw New NotImplementedException
        End Function

        Public Function GetEnumerator() As Global.System.Collections.Generic.IEnumerator(Of E) Implements Global.System.Collections.Generic.IEnumerable(Of E).GetEnumerator
            Return Me.elementData.GetEnumerator
        End Function

        Private Function GetEnumerator2() As Global.System.Collections.IEnumerator Implements Global.System.Collections.IEnumerable.GetEnumerator
            Throw New NotImplementedException
        End Function

        Public Function elements() As Global.System.Collections.Generic.IEnumerable(Of E)
            Return Me.elementData
        End Function

        ''' <summary>
        ''' Trims the capacity of this vector to be the vector's current size.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub trimToSize()
            Throw New NotImplementedException
        End Sub

        ''' <summary>
        ''' Returns a string representation of this Vector, containing the String representation of each element.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function ToString() As String
            Dim ret As New System.Text.StringBuilder
            For Each item As Object In Me.elementData
                If (ret.Length > 0) Then ret.Append("; ")
                ret.Append(item.ToString)
            Next
            Return ret.ToString
        End Function

        ''' <summary>
        ''' Sets the component at the specified index of this vector to be the specified object.
        ''' </summary>
        ''' <param name="obj"></param>
        ''' <param name="index"></param>
        ''' <remarks></remarks>
        Public Sub setElementAt(ByVal obj As E, ByVal index As Integer)
            Throw New NotImplementedException
        End Sub

        ''' <summary>
        ''' Removes all components from this vector and sets its size to zero.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub removeAllElements()
            Throw New NotImplementedException
        End Sub

        Sub setSize(numberOfArticleSections As Integer)
            Throw New NotImplementedException
        End Sub

        Private Function _add(o As Object) As Boolean Implements ICollection.add
            Return Me.add(o)
        End Function

        Private Function _addAll(col As ICollection) As Boolean Implements ICollection.addAll
            Return Me.addAll(col)
        End Function


        Private Overloads Function addAll1(fromIndex As Integer, col As ICollection) As Boolean Implements List.addAll
            Return Me.addAll(fromIndex, col)
        End Function


    End Class

End Namespace
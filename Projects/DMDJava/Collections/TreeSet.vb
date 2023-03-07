Imports FinSeA.Sistema

Namespace java
    ''' <summary>
    ''' A NavigableSet implementation based on a TreeMap. The elements are ordered using their natural ordering, or by a Comparator provided at set creation time, depending on which constructor is used.
    ''' This implementation provides guaranteed log(n) time cost for the basic operations (add, remove and contains).
    ''' Note that the ordering maintained by a set (whether or not an explicit comparator is provided) must be consistent with equals if it is to correctly implement the Set interface. (See Comparable or Comparator for a precise definition of consistent with equals.) This is so because the Set interface is defined in terms of the equals operation, but a TreeSet instance performs all element comparisons using its compareTo (or compare) method, so two elements that are deemed equal by this method are, from the standpoint of the set, equal. The behavior of a set is well-defined even if its ordering is inconsistent with equals; it just fails to obey the general contract of the Set interface.
    ''' Note that this implementation is not synchronized. If multiple threads access a tree set concurrently, and at least one of the threads modifies the set, it must be synchronized externally. This is typically accomplished by synchronizing on some object that naturally encapsulates the set. If no such object exists, the set should be "wrapped" using the Collections.synchronizedSortedSet method. This is best done at creation time, to prevent accidental unsynchronized access to the set:
    '''    SortedSet s = Collections.synchronizedSortedSet(new TreeSet(...));
    ''' The iterators returned by this class's iterator method are fail-fast: if the set is modified at any time after the iterator is created, in any way except through the iterator's own remove method, the iterator will throw a ConcurrentModificationException. Thus, in the face of concurrent modification, the iterator fails quickly and cleanly, rather than risking arbitrary, non-deterministic behavior at an undetermined time in the future.
    ''' Note that the fail-fast behavior of an iterator cannot be guaranteed as it is, generally speaking, impossible to make any hard guarantees in the presence of unsynchronized concurrent modification. Fail-fast iterators throw ConcurrentModificationException on a best-effort basis. Therefore, it would be wrong to write a program that depended on this exception for its correctness: the fail-fast behavior of iterators should be used only to detect bugs.
    ''' This class is a member of the Java Collections Framework.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class TreeSet
        Inherits AbstractCollection
        Implements NavigableSet

        Protected Class EqualityComparer(Of T)
            Implements Global.System.Collections.Generic.IEqualityComparer(Of T), Global.System.Collections.IComparer

            Private m_Comparer As Global.System.Collections.IComparer

            Public Sub New(ByVal c As Global.System.Collections.IComparer)
                Me.m_Comparer = c
            End Sub

            Public Function Equals1(x As T, y As T) As Boolean Implements Global.System.Collections.Generic.IEqualityComparer(Of T).Equals
                Return Me.m_Comparer.Compare(x, y) = 0
            End Function

            Public Function GetHashCode1(obj As T) As Integer Implements Global.System.Collections.Generic.IEqualityComparer(Of T).GetHashCode
                Return obj.GetHashCode
            End Function

            Public Function Compare(x As Object, y As Object) As Integer Implements Global.System.Collections.IComparer.Compare
                Return Me.m_Comparer.Compare(x, y)
            End Function
        End Class

        Private m_Base As System.Collections.Generic.HashSet(Of Object)
        Private m_First As Object = Nothing
        Private m_Last As Object = Nothing

        ''' <summary>
        ''' Constructs a new, empty tree set, sorted according to the natural ordering of its elements.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            Me.m_Base = New System.Collections.Generic.HashSet(Of Object)(New EqualityComparer(Of Object)(Arrays.DefaultComparer))
        End Sub

        ''' <summary>
        ''' Constructs a new tree set containing the elements in the specified collection, sorted according to the natural ordering of its elements.
        ''' </summary>
        ''' <param name="c"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal c As ICollection)
            Me.m_Base = New System.Collections.Generic.HashSet(Of Object)(New EqualityComparer(Of Object)(Arrays.DefaultComparer))
            Me.addAll(c)
        End Sub

        ''' <summary>
        ''' Constructs a new, empty tree set, sorted according to the specified comparator.
        ''' </summary>
        ''' <param name="comparator"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal comparator As Global.System.Collections.IComparer)
            Me.m_Base = New System.Collections.Generic.HashSet(Of Object)(New EqualityComparer(Of Object)(comparator))
        End Sub

        ''' <summary>
        ''' Constructs a new tree set containing the same elements and using the same ordering as the specified sorted set.
        ''' </summary>
        ''' <param name="s"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal s As SortedSet)
            Me.m_Base = New System.Collections.Generic.HashSet(Of Object)(DirectCast(s.comparator, EqualityComparer(Of Object)))
        End Sub


        Public Overrides Function add(o As Object) As Boolean
            If (Me.m_First Is Nothing) Then
                Me.m_First = o
                Me.m_Last = o
            Else
                If Me.comparator.Compare(Me.m_First, o) < 0 Then
                    Me.m_First = o
                ElseIf Me.comparator.Compare(Me.m_Last, o) > 0 Then
                    Me.m_Last = o
                End If
            End If
            Me.m_Base.Add(o)
            Return True
        End Function

        Public Overrides Function addAll(col As ICollection) As Boolean
            For Each o As Object In col
                Me.m_Base.Add(o)
                If (Me.m_First Is Nothing) Then
                    Me.m_First = o
                    Me.m_Last = o
                Else
                    If Me.comparator.Compare(Me.m_First, o) < 0 Then
                        Me.m_First = o
                    ElseIf Me.comparator.Compare(Me.m_Last, o) > 0 Then
                        Me.m_Last = o
                    End If
                End If
            Next
            Return True
        End Function

        Public Overrides Sub clear()
            Me.m_Base.Clear()
            Me.m_First = Nothing
            Me.m_Last = Nothing
        End Sub

        Public Overrides Function contains(o As Object) As Boolean
            Return Me.m_Base.Contains(o)
        End Function

        Public Overrides Function containsAll(col As ICollection) As Boolean
            For Each item As Object In col
                If Not Me.m_Base.Contains(item) Then Return False
            Next
            Return True
        End Function

        Public Overloads Overrides Function equals(o As Object) As Boolean
            Return TypeOf (o) Is TreeSet AndAlso (DirectCast(o, TreeSet).m_Base.Equals(Me.m_Base))
        End Function

        Public Overrides Function GetEnumerator() As Global.System.Collections.IEnumerator
            Return Me.m_Base.GetEnumerator
        End Function

        Public Overrides Function isEmpty() As Boolean
            Return Me.m_Base.Count = 0
        End Function

        Public Overrides Function iterator() As Iterator
            Return New Collections.GenericIterator(Of Object)(Me)
        End Function

        Public Overrides Function remove(o As Object) As Boolean
            Dim ret As Boolean = Me.m_Base.Remove(o)
            If (ret) Then
                Me.m_First = Nothing
                Me.m_Last = Nothing
            End If
            Return ret
        End Function

        Public Overrides Function removeAll(col As ICollection) As Boolean
            Dim ret As Boolean = False
            For Each item As Object In col
                ret = ret And Me.m_Base.Remove(item)
            Next
            If (ret) Then
                Me.m_First = Nothing
                Me.m_Last = Nothing
            End If
            Return ret
        End Function

        Public Overrides Function retainAll(col As ICollection) As Boolean
            Me.m_First = Nothing
            Me.m_Last = Nothing
            Throw New NotImplementedException
        End Function

        Public Overrides Function size() As Integer
            Return Me.m_Base.Count
        End Function

        Public Overloads Overrides Function toArray() As Object()
            Dim ret() As Object = Nothing
            If (Me.m_Base.Count > 0) Then
                ReDim ret(Me.m_Base.Count - 1)
                Me.m_Base.CopyTo(ret, 0, Me.m_Base.Count - 1)
            End If
            Return ret
        End Function

        Public Overloads Overrides Function toArray(Of T)() As T()
            Dim ret() As T = Nothing
            If (Me.m_Base.Count > 0) Then
                ReDim ret(Me.m_Base.Count - 1)
                Me.m_Base.CopyTo(ret, 0, Me.m_Base.Count - 1)
            End If
            Return ret
        End Function

        Public Function ceiling(e As Object) As Object Implements NavigableSet.ceiling
            Dim ret As Object = Nothing
            For Each o As Object In Me.m_Base
                If Me.comparator.Compare(o, e) <= 0 Then
                    If (ret Is Nothing) Then
                        ret = o
                    ElseIf (Me.comparator.Compare(ret, o) < 0) Then
                        ret = o
                    End If
                End If
            Next
            Return ret
        End Function

        Public Function descendingIterator() As Iterator Implements NavigableSet.descendingIterator
            Return Me.descendingSet.iterator
        End Function

        Public Function descendingSet() As NavigableSet Implements NavigableSet.descendingSet
            Dim ret As New TreeSet
            Dim arr() As Object = Me.toArray
            If (arr IsNot Nothing) Then
                For i = UBound(arr) To 0 Step -1
                    ret.add(arr(i))
                Next
            End If
            Return ret
        End Function

        Public Function floor(e As Object) As Object Implements NavigableSet.floor
            Dim ret As Object = Nothing
            For Each o As Object In Me.m_Base
                If Me.comparator.Compare(o, e) >= 0 Then
                    If (ret Is Nothing) Then
                        ret = o
                    ElseIf (Me.comparator.Compare(ret, o) > 0) Then
                        ret = o
                    End If
                End If
            Next
            Return ret
        End Function

        Public Overloads Function headSet(toElement As Object, inclusive As Boolean) As NavigableSet Implements NavigableSet.headSet
            Dim ret As New TreeSet
            If inclusive Then
                For Each o As Object In Me
                    If Me.comparator.Compare(o, toElement) <= 0 Then
                        ret.add(o)
                    End If
                Next
            Else
                For Each o As Object In Me
                    If Me.comparator.Compare(o, toElement) < 0 Then
                        ret.add(o)
                    End If
                Next
            End If
            Return ret
        End Function

        Public Overloads Function headSet(toElement As Object) As SortedSet Implements SortedSet.headSet
            Return Me.headSet(toElement, False)
        End Function

        Public Function higher(e As Object) As Object Implements NavigableSet.higher
            Dim ret As Object = Nothing
            For Each o As Object In Me.m_Base
                If Me.comparator.Compare(o, e) > 0 Then
                    If (ret Is Nothing) Then
                        ret = o
                    ElseIf (Me.comparator.Compare(ret, o) > 0) Then
                        ret = o
                    End If
                End If
            Next
            Return ret
        End Function

        Public Function lower(e As Object) As Object Implements NavigableSet.lower
            Dim ret As Object = Nothing
            For Each o As Object In Me.m_Base
                If Me.comparator.Compare(o, e) < 0 Then
                    If (ret Is Nothing) Then
                        ret = o
                    ElseIf (Me.comparator.Compare(ret, o) < 0) Then
                        ret = o
                    End If
                End If
            Next
            Return ret
        End Function

        Public Function pollFirst() As Object Implements NavigableSet.pollFirst
            If (Me.isEmpty) Then Return Nothing
            Dim ret As Object = Me.first
            For Each o As Object In Me
                If Me.comparator.Compare(ret, o) < 0 Then
                    ret = o
                End If
            Next
            Me.remove(ret)
            Return ret
        End Function

        Public Function pollLast() As Object Implements NavigableSet.pollLast
            If (Me.isEmpty) Then Return Nothing
            Dim ret As Object = Me.first
            For Each o As Object In Me
                If Me.comparator.Compare(ret, o) > 0 Then
                    ret = o
                End If
            Next
            Me.remove(ret)
            Return ret
        End Function

        Public Overloads Function subSet(fromElement As Object, fromInclusive As Boolean, toElement As Object, toInclusive As Boolean) As NavigableSet Implements NavigableSet.subSet
            Dim ret As New TreeSet
            For Each o As Object In Me
                Dim t1 As Integer = Me.comparator.Compare(o, fromElement)
                Dim t2 As Integer = Me.comparator.Compare(o, toElement)
                If ((t1 > 0) OrElse (t1 = 0 AndAlso fromInclusive)) And ((t2 < 0) OrElse (t2 = 0 AndAlso toInclusive)) Then
                    ret.add(o)
                End If
            Next
            Return ret
        End Function

        Public Overloads Function subSet(fromElement As Object, toElement As Object) As SortedSet Implements SortedSet.subSet
            Return Me.subSet(fromElement, False, toElement, False)
        End Function

        Public Overloads Function tailSet(fromElement As Object, inclusive As Boolean) As NavigableSet Implements NavigableSet.tailSet
            Dim ret As New TreeSet
            If (inclusive) Then
                For Each o As Object In Me
                    If Me.comparator.Compare(o, fromElement) >= 0 Then
                        ret.add(o)
                    End If
                Next
            Else
                For Each o As Object In Me
                    If Me.comparator.Compare(o, fromElement) > 0 Then
                        ret.add(o)
                    End If
                Next
            End If
            Return ret
        End Function

        Public Overloads Function tailSet(fromElement As Object) As SortedSet Implements SortedSet.tailSet
            Return Me.tailSet(fromElement, False)
        End Function


        Public Function comparator() As Global.System.Collections.IComparer Implements SortedSet.comparator
            Return Me.m_Base.Comparer
        End Function

        Public Function first() As Object Implements SortedSet.first
            If (Me.m_First Is Nothing) Then
                Dim tmp() As Object = Me.toArray
                If (tmp IsNot Nothing) AndAlso (tmp.Length > 0) Then
                    Me.m_First = tmp(0)
                    Me.m_Last = tmp(0)
                    For i As Integer = 1 To UBound(tmp)
                        Dim c As Integer
                        c = Me.comparator.Compare(tmp(i), Me.m_First)
                        If (c < 0) Then
                            Me.m_First = tmp(i)
                        Else
                            c = Me.comparator.Compare(tmp(i), Me.m_Last)
                            If (c > 0) Then
                                Me.m_Last = tmp(i)
                            End If
                        End If
                    Next
                End If
            End If
            Return Me.m_First
        End Function

        Public Function last() As Object Implements SortedSet.last
            If (Me.m_Last Is Nothing) Then
                Dim tmp() As Object = Me.toArray
                If (tmp IsNot Nothing) AndAlso (tmp.Length > 0) Then
                    Me.m_First = tmp(0)
                    Me.m_Last = tmp(0)
                    For i As Integer = 1 To UBound(tmp)
                        Dim c As Integer
                        c = Me.comparator.Compare(tmp(i), Me.m_First)
                        If (c < 0) Then
                            Me.m_First = tmp(i)
                        Else
                            c = Me.comparator.Compare(tmp(i), Me.m_Last)
                            If (c > 0) Then
                                Me.m_Last = tmp(i)
                            End If
                        End If
                    Next
                End If
            End If
            Return Me.m_Last
        End Function





    End Class

End Namespace
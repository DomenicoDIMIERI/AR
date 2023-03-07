Imports FinSeA.Sistema

Namespace java
    'public class HashSet<E>
    'extends AbstractSet<E>
    'implements Set<E>, Cloneable, Serializable

    'This class implements the Set interface, backed by a hash table (actually a HashMap instance). It makes no guarantees as to the iteration order of the set; in particular, it does not guarantee that the order will remain constant over time. This class permits the null element.

    'This class offers constant time performance for the basic operations (add, remove, contains and size), assuming the hash function disperses the elements properly among the buckets. Iterating over this set requires time proportional to the sum of the HashSet instance's size (the number of elements) plus the "capacity" of the backing HashMap instance (the number of buckets). Thus, it's very important not to set the initial capacity too high (or the load factor too low) if iteration performance is important.

    'Note that this implementation is not synchronized. If multiple threads access a hash set concurrently, and at least one of the threads modifies the set, it must be synchronized externally. This is typically accomplished by synchronizing on some object that naturally encapsulates the set. If no such object exists, the set should be "wrapped" using the Collections.synchronizedSet method. This is best done at creation time, to prevent accidental unsynchronized access to the set:

    '   Set s = Collections.synchronizedSet(new HashSet(...));

    'The iterators returned by this class's iterator method are fail-fast: if the set is modified at any time after the iterator is created, in any way except through the iterator's own remove method, the Iterator throws a ConcurrentModificationException. Thus, in the face of concurrent modification, the iterator fails quickly and cleanly, rather than risking arbitrary, non-deterministic behavior at an undetermined time in the future.

    'Note that the fail-fast behavior of an iterator cannot be guaranteed as it is, generally speaking, impossible to make any hard guarantees in the presence of unsynchronized concurrent modification. Fail-fast iterators throw ConcurrentModificationException on a best-effort basis. Therefore, it would be wrong to write a program that depended on this exception for its correctness: the fail-fast behavior of iterators should be used only to detect bugs.

    'This class is a member of the Java Collections Framework.

    'Since:
    '    1.2
    Public Class HashSet
        Implements [Set]

        Private m_set As New System.Collections.Generic.HashSet(Of Object)

        Public Sub New()
        End Sub

        Public Function add(e As Object) As Boolean Implements [Set].add
            Return Me.m_set.Add(e)
        End Function

        Public Function addAll(c As ICollection) As Boolean Implements [Set].addAll
            For Each item As Object In c
                Me.m_set.Add(item)
            Next
            Return True
        End Function

        Public Sub clear() Implements [Set].clear
            Me.m_set.Clear()
        End Sub

        Public Function contains(o As Object) As Boolean Implements [Set].contains
            Return Me.m_set.Contains(o)
        End Function

        Public Function containsAll(c As ICollection) As Boolean Implements [Set].containsAll
            Dim ret As Boolean = True
            For Each item As Object In c
                ret = ret And Me.m_set.Contains(c)
                If (ret = False) Then Exit For
            Next
            Return ret
        End Function

        Public Overrides Function equals(o As Object) As Boolean Implements [Set].equals
            Return (TypeOf (o) Is HashSet) AndAlso (DirectCast(o, HashSet).m_set.Equals(Me.m_set))
        End Function

        Public Function hashCode() As Integer Implements [Set].hashCode
            Return Me.GetHashCode
        End Function

        Public Overrides Function GetHashCode() As Integer
            Return Me.m_set.GetHashCode
        End Function

        Public Function isEmpty() As Boolean Implements [Set].isEmpty
            Return Me.m_set.Count = 0
        End Function

        Public Function iterator() As Iterator Implements [Set].iterator
            Return New Collections.GenericIterator(Of Object)(Me)
        End Function

        Public Function remove(o As Object) As Boolean Implements [Set].remove
            Return Me.m_set.Remove(o)
        End Function

        Public Function removeAll(c As ICollection) As Boolean Implements [Set].removeAll
            Dim ret As Boolean = True
            For Each item As Object In c
                ret = ret And Me.m_set.Remove(item)
            Next
            Return ret
        End Function

        Public Function retainAll(c As ICollection) As Boolean Implements [Set].retainAll
            Throw New NotImplementedException
        End Function

        Public Function size() As Integer Implements [Set].size
            Return Me.m_set.Count
        End Function

        Public Function toArray() As Object() Implements [Set].toArray
            Dim ret() As Object = Arrays.CreateInstance(Of Object)(Me.m_set.Count)
            Me.m_set.CopyTo(ret)
            Return ret
        End Function

        Public Function toArray(Of T)() As T() Implements [Set].toArray
            Dim ret() As T = Arrays.CreateInstance(Of T)(Me.m_set.Count)
            Me.m_set.CopyTo(ret)
            Return ret
        End Function

        Public Function GetEnumerator() As Global.System.Collections.IEnumerator Implements Global.System.Collections.IEnumerable.GetEnumerator
            Return Me.m_set.GetEnumerator
        End Function
    End Class

End Namespace
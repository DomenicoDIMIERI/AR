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
    Public Class HashSet(Of T)
        Inherits HashSet
        Implements [Set](Of T)

        Public Sub New()
        End Sub

        Sub New(ByVal col As ICollection(Of T))
            Me.addAll(col)
        End Sub


        Public Shadows Function add(o As T) As Boolean Implements ICollection(Of T).add
            Return MyBase.add(o)
        End Function

        Public Shadows Function addAll(col As ICollection(Of T)) As Boolean Implements ICollection(Of T).addAll
            Return MyBase.addAll(col)
        End Function

        Public Shadows Function contains(o As T) As Boolean Implements ICollection(Of T).contains
            Return MyBase.contains(o)
        End Function

        Public Shadows Function containsAll(col As ICollection(Of T)) As Boolean Implements ICollection(Of T).containsAll
            Return MyBase.containsAll(col)
        End Function

        Public Shadows Function remove(o As T) As Boolean Implements ICollection(Of T).remove
            Return MyBase.remove(o)
        End Function

        Public Shadows Function removeAll(col As ICollection(Of T)) As Boolean Implements ICollection(Of T).removeAll
            Return MyBase.removeAll(col)
        End Function

        Public Shadows Function retainAll(col As ICollection(Of T)) As Boolean Implements ICollection(Of T).retainAll
            Return MyBase.retainAll(col)
        End Function

        Private Function _removeAll(c As ICollection(Of T)) As Boolean Implements [Set](Of T).removeAll
            Return MyBase.removeAll(c)
        End Function

        Private Function _retainAll(c As ICollection(Of T)) As Boolean Implements [Set](Of T).retainAll
            Return MyBase.retainAll(c)
        End Function

        Private Function _add(e As T) As Boolean Implements [Set](Of T).add
            Return MyBase.add(e)
        End Function

        Private Function _addAll(c As ICollection(Of T)) As Boolean Implements [Set](Of T).addAll
            Return MyBase.addAll(c)
        End Function

        Private Function _contains(o As T) As Boolean Implements [Set](Of T).contains
            Return MyBase.contains(o)
        End Function

        Private Function _containsAll(c As ICollection(Of T)) As Boolean Implements [Set](Of T).containsAll
            Return MyBase.containsAll(c)
        End Function

        Public Shadows Function iterator() As Iterator(Of T) Implements [Set](Of T).iterator
            Return New Collections.GenericIterator(Of T)(Me)
        End Function

        Private Function _remove(o As T) As Boolean Implements [Set](Of T).remove
            Return MyBase.remove(o)
        End Function

        Public Shadows Function GetEnumerator() As Global.System.Collections.Generic.IEnumerator(Of T) Implements Global.System.Collections.Generic.IEnumerable(Of T).GetEnumerator
            Return New Enumerator(Of T)(MyBase.GetEnumerator)
        End Function

    End Class

End Namespace
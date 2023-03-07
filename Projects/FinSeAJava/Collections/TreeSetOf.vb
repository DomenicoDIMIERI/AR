Namespace java
    Public Class TreeSet(Of T)
        Inherits TreeSet
        Implements [Set](Of T)

        Public Sub New()
        End Sub

        Public Sub New(ByVal col As ICollection(Of T))
            MyBase.New(col)
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

        Private Function _add(e As T) As Boolean Implements [Set](Of T).add
            Return Me.add(e)
        End Function

        Private Function _addAll(c As ICollection(Of T)) As Boolean Implements [Set](Of T).addAll
            Return Me.addAll(c)
        End Function

        Private Function _contains(o As T) As Boolean Implements [Set](Of T).contains
            Return Me.contains(o)
        End Function

        Private Function _containsAll(c As ICollection(Of T)) As Boolean Implements [Set](Of T).containsAll
            Return Me.containsAll(c)
        End Function

        Public Shadows Function iterator() As Iterator(Of T) Implements [Set](Of T).iterator
            Return MyBase.iterator
        End Function

        Private Function _remove(o As T) As Boolean Implements [Set](Of T).remove
            Return Me.remove(o)
        End Function

        Private Function _removeAll(c As ICollection(Of T)) As Boolean Implements [Set](Of T).removeAll
            Return Me.removeAll(c)
        End Function

        Private Function _retainAll(c As ICollection(Of T)) As Boolean Implements [Set](Of T).retainAll
            Return Me.retainAll(c)
        End Function

        Public Shadows Function GetEnumerator() As Global.System.Collections.Generic.IEnumerator(Of T) Implements Global.System.Collections.Generic.IEnumerable(Of T).GetEnumerator
            Return New Enumerator(Of T)(MyBase.GetEnumerator)
        End Function

        Public Shadows Function last() As T
            Return MyBase.last
        End Function

        Public Shadows Function first() As T
            Return MyBase.first
        End Function

    End Class

End Namespace
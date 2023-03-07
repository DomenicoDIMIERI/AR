Namespace java

    Public MustInherit Class AbstractCollection
        Implements ICollection

        Public MustOverride Function add(o As Object) As Boolean Implements ICollection.add

        Public MustOverride Function addAll(col As ICollection) As Boolean Implements ICollection.addAll

        Public MustOverride Sub clear() Implements ICollection.clear

        Public MustOverride Function contains(o As Object) As Boolean Implements ICollection.contains

        Public MustOverride Function containsAll(col As ICollection) As Boolean Implements ICollection.containsAll

        Public MustOverride Overrides Function equals(o As Object) As Boolean Implements ICollection.equals

        Public Function hashCode() As Integer Implements ICollection.hashCode
            Return Me.GetHashCode
        End Function

        Public MustOverride Function isEmpty() As Boolean Implements ICollection.isEmpty

        Public MustOverride Function iterator() As Iterator Implements ICollection.iterator

        Public MustOverride Function remove(o As Object) As Boolean Implements ICollection.remove

        Public MustOverride Function removeAll(col As ICollection) As Boolean Implements ICollection.removeAll

        Public MustOverride Function retainAll(col As ICollection) As Boolean Implements ICollection.retainAll

        Public MustOverride Function size() As Integer Implements ICollection.size

        Public MustOverride Function toArray() As Object() Implements ICollection.toArray

        Public MustOverride Function toArray(Of T)() As T() Implements ICollection.toArray

        Public MustOverride Function GetEnumerator() As Global.System.Collections.IEnumerator Implements Global.System.Collections.IEnumerable.GetEnumerator


    End Class

End Namespace
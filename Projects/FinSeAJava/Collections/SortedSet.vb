Namespace java
    Public Interface SortedSet
        Inherits [Set]

        ''' <summary>
        ''' Returns the comparator used to order the elements in this set, or null if this set uses the natural ordering of its elements.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function comparator() As Global.System.Collections.IComparer

        ''' <summary>
        ''' Returns the first (lowest) element currently in this set.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function first() As Object

        ''' <summary>
        ''' Returns the last (highest) element currently in this set.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function last() As Object

        ''' <summary>
        ''' Returns a view of the portion of this set whose elements are strictly less than toElement.
        ''' </summary>
        ''' <param name="toElement"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function headSet(ByVal toElement As Object) As SortedSet

        ''' <summary>
        ''' Returns a view of the portion of this set whose elements are greater than or equal to fromElement.
        ''' </summary>
        ''' <param name="fromElement"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function tailSet(ByVal fromElement As Object) As SortedSet

        ''' <summary>
        ''' Returns a view of the portion of this set whose elements range from fromElement, inclusive, to toElement, exclusive.
        ''' </summary>
        ''' <param name="fromElement"></param>
        ''' <param name="toElement"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function subSet(ByVal fromElement As Object, ByVal toElement As Object) As SortedSet

    End Interface

End Namespace
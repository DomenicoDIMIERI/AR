Namespace java
    Public Interface SortedSet(Of T)
        Inherits [Set](Of T)

        ''' <summary>
        ''' Returns the comparator used to order the elements in this set, or null if this set uses the natural ordering of its elements.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function comparator() As Global.System.Collections.Generic.IComparer(Of T)

        ''' <summary>
        ''' Returns the first (lowest) element currently in this set.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function first() As T

        ''' <summary>
        ''' Returns the last (highest) element currently in this set.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function last() As T

        ''' <summary>
        ''' Returns a view of the portion of this set whose elements are strictly less than toElement.
        ''' </summary>
        ''' <param name="toElement"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function headSet(ByVal toElement As T) As SortedSet(Of T)

        ''' <summary>
        ''' Returns a view of the portion of this set whose elements are greater than or equal to fromElement.
        ''' </summary>
        ''' <param name="fromElement"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function tailSet(ByVal fromElement As T) As SortedSet(Of T)

        ''' <summary>
        ''' Returns a view of the portion of this set whose elements range from fromElement, inclusive, to toElement, exclusive.
        ''' </summary>
        ''' <param name="fromElement"></param>
        ''' <param name="toElement"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function subSet(ByVal fromElement As T, ByVal toElement As T) As SortedSet(Of T)

    End Interface

End Namespace
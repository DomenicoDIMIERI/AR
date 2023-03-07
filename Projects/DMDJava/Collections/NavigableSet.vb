Namespace java
    ''' <summary>
    ''' A SortedSet extended with navigation methods reporting closest matches for given search targets. Methods lower, floor, ceiling, and higher return elements respectively less than, less than or equal, greater than or equal, and greater than a given element, returning null if there is no such element. A NavigableSet may be accessed and traversed in either ascending or descending order. The descendingSet method returns a view of the set with the senses of all relational and directional methods inverted. The performance of ascending operations and views is likely to be faster than that of descending ones. This interface additionally defines methods pollFirst and pollLast that return and remove the lowest and highest element, if one exists, else returning null. Methods subSet, headSet, and tailSet differ from the like-named SortedSet methods in accepting additional arguments describing whether lower and upper bounds are inclusive versus exclusive. Subsets of any NavigableSet must implement the NavigableSet interface.
    ''' The return values of navigation methods may be ambiguous in implementations that permit null elements. However, even in this case the result can be disambiguated by checking contains(null). To avoid such issues, implementations of this interface are encouraged to not permit insertion of null elements. (Note that sorted sets of Comparable elements intrinsically do not permit null.)
    ''' Methods subSet(E, E), headSet(E), and tailSet(E) are specified to return SortedSet to allow existing implementations of SortedSet to be compatibly retrofitted to implement NavigableSet, but extensions and implementations of this interface are encouraged to override these methods to return NavigableSet.
    ''' This interface is a member of the Java Collections Framework.
    ''' </summary>
    ''' <remarks></remarks>
    Public Interface NavigableSet
        Inherits SortedSet

        ''' <summary>
        ''' Returns the least element in this set greater than or equal to the given element, or null if there is no such element.
        ''' </summary>
        ''' <param name="e"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function ceiling(ByVal e As Object) As Object

        ''' <summary>
        ''' Returns an iterator over the elements in this set, in descending order.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function descendingIterator() As Iterator

        ''' <summary>
        ''' Returns a reverse order view of the elements contained in this set.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function descendingSet() As NavigableSet

        ''' <summary>
        ''' Returns the greatest element in this set less than or equal to the given element, or null if there is no such element.
        ''' </summary>
        ''' <param name="e"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function floor(ByVal e As Object) As Object

        ''' <summary>
        ''' Returns a view of the portion of this set whose elements are less than (or equal to, if inclusive is true) toElement.
        ''' </summary>
        ''' <param name="toElement"></param>
        ''' <param name="inclusive"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Overloads Function headSet(ByVal toElement As Object, ByVal inclusive As Boolean) As NavigableSet

        ''' <summary>
        ''' Returns the least element in this set strictly greater than the given element, or null if there is no such element.
        ''' </summary>
        ''' <param name="e"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function higher(ByVal e As Object) As Object

        ''' <summary>
        ''' Returns the greatest element in this set strictly less than the given element, or null if there is no such element.
        ''' </summary>
        ''' <param name="e"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function lower(ByVal e As Object) As Object

        ''' <summary>
        ''' Retrieves and removes the first (lowest) element, or returns null if this set is empty.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function pollFirst() As Object

        ''' <summary>
        ''' Retrieves and removes the last (highest) element, or returns null if this set is empty.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function pollLast() As Object


        ''' <summary>
        ''' Returns a view of the portion of this set whose elements range from fromElement to toElement.
        ''' </summary>
        ''' <param name="fromElement"></param>
        ''' <param name="fromInclusive"></param>
        ''' <param name="toElement"></param>
        ''' <param name="toInclusive"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Overloads Function subSet(ByVal fromElement As Object, ByVal fromInclusive As Boolean, ByVal toElement As Object, ByVal toInclusive As Boolean) As NavigableSet

        ''' <summary>
        ''' Returns a view of the portion of this set whose elements are greater than (or equal to, if inclusive is true) fromElement.
        ''' </summary>
        ''' <param name="fromElement"></param>
        ''' <param name="inclusive"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Overloads Function tailSet(ByVal fromElement As Object, ByVal inclusive As Boolean) As NavigableSet


    End Interface

End Namespace
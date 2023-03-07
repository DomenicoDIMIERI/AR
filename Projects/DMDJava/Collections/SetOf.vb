Namespace java
    Public Interface [Set](Of T)
        Inherits [Set], ICollection(Of T)


        ''' <summary>
        ''' Adds the specified element to this set if it is not already present (optional operation).
        ''' </summary>
        ''' <param name="e"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Shadows Function add(ByVal e As T) As Boolean

        ''' <summary>
        ''' Adds all of the elements in the specified collection to this set if they're not already present (optional operation).
        ''' </summary>
        ''' <param name="c"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Shadows Function addAll(ByVal c As ICollection(Of T)) As Boolean

        ''' <summary>
        ''' Returns true if this set contains the specified element.
        ''' </summary>
        ''' <param name="o"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Shadows Function contains(ByVal o As T) As Boolean

        ''' <summary>
        ''' Returns true if this set contains all of the elements of the specified collection.
        ''' </summary>
        ''' <param name="c"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Shadows Function containsAll(ByVal c As ICollection(Of T)) As Boolean


        ''' <summary>
        ''' Returns an iterator over the elements in this set.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Shadows Function [iterator]() As Iterator(Of T)

        ''' <summary>
        ''' Removes the specified element from this set if it is present (optional operation).
        ''' </summary>
        ''' <param name="o"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Shadows Function remove(ByVal o As T) As Boolean

        ''' <summary>
        ''' Removes from this set all of its elements that are contained in the specified collection (optional operation).
        ''' </summary>
        ''' <param name="c"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Shadows Function removeAll(ByVal c As ICollection(Of T)) As Boolean

        ''' <summary>
        ''' Retains only the elements in this set that are contained in the specified collection (optional operation).
        ''' </summary>
        ''' <param name="c"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Shadows Function retainAll(ByVal c As ICollection(Of T)) As Boolean



    End Interface

End Namespace
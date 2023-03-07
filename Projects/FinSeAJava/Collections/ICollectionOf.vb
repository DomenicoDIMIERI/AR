Namespace java
    Public Interface ICollection(Of T)
        Inherits ICollection, Global.System.Collections.Generic.IEnumerable(Of T)

        ''' <summary>
        ''' Appends the specified element to the end of this list (optional operation).
        ''' </summary>
        ''' <param name="o"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Shadows Function add(ByVal o As T) As Boolean


        ''' <summary>
        ''' Appends all of the elements in the specified collection to the end of this list, in the order that they are returned by the specified collection's iterator (optional operation).
        ''' </summary>
        ''' <param name="col"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Shadows Function addAll(ByVal col As ICollection(Of T)) As Boolean



        ''' <summary>
        ''' Returns true if this list contains the specified element.
        ''' </summary>
        ''' <param name="o"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Shadows Function contains(ByVal o As T) As Boolean

        ''' <summary>
        ''' Returns true if this list contains all of the elements of the specified collection.
        ''' </summary>
        ''' <param name="col"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Shadows Function containsAll(ByVal col As ICollection(Of T)) As Boolean



        ''' <summary>
        ''' Removes the first occurrence of the specified element from this list, if it is present (optional operation).
        ''' </summary>
        ''' <param name="o"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Shadows Function remove(ByVal o As T) As Boolean

        ''' <summary>
        ''' Removes from this list all of its elements that are contained in the specified collection (optional operation).
        ''' </summary>
        ''' <param name="col"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Shadows Function removeAll(ByVal col As ICollection(Of T)) As Boolean

        ''' <summary>
        ''' Retains only the elements in this list that are contained in the specified collection (optional operation).
        ''' </summary>
        ''' <param name="col"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Shadows Function retainAll(ByVal col As ICollection(Of T)) As Boolean






    End Interface

End Namespace
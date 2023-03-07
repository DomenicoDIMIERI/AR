Namespace java
    Public Interface ICollection
        Inherits System.Collections.IEnumerable

        ''' <summary>
        ''' Appends the specified element to the end of this list (optional operation).
        ''' </summary>
        ''' <param name="o"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function add(ByVal o As Object) As Boolean


        ''' <summary>
        ''' Appends all of the elements in the specified collection to the end of this list, in the order that they are returned by the specified collection's iterator (optional operation).
        ''' </summary>
        ''' <param name="col"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function addAll(ByVal col As ICollection) As Boolean


        ''' <summary>
        ''' Removes all of the elements from this list (optional operation).
        ''' </summary>
        ''' <remarks></remarks>
        Sub clear()

        ''' <summary>
        ''' Returns true if this list contains the specified element.
        ''' </summary>
        ''' <param name="o"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function contains(ByVal o As Object) As Boolean

        ''' <summary>
        ''' Returns true if this list contains all of the elements of the specified collection.
        ''' </summary>
        ''' <param name="col"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function containsAll(ByVal col As ICollection) As Boolean

        ''' <summary>
        ''' Compares the specified object with this list for equality.
        ''' </summary>
        ''' <param name="o"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function equals(ByVal o As Object) As Boolean

        ''' <summary>
        ''' Returns the hash code value for this list.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function hashCode() As Integer

        ''' <summary>
        ''' Returns true if this list contains no elements.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function isEmpty() As Boolean

        ''' <summary>
        ''' Returns an iterator over the elements in this list in proper sequence.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function iterator() As Iterator


        '    ListIterator<E> 	listIterator()
        'Returns a list iterator over the elements in this list (in proper sequence).
        'ListIterator<E> 	listIterator(int index)
        'Returns a list iterator over the elements in this list (in proper sequence), starting at the specified position in the list.


        ''' <summary>
        ''' Removes the first occurrence of the specified element from this list, if it is present (optional operation).
        ''' </summary>
        ''' <param name="o"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function remove(ByVal o As Object) As Boolean

        ''' <summary>
        ''' Removes from this list all of its elements that are contained in the specified collection (optional operation).
        ''' </summary>
        ''' <param name="col"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function removeAll(ByVal col As ICollection) As Boolean

        ''' <summary>
        ''' Retains only the elements in this list that are contained in the specified collection (optional operation).
        ''' </summary>
        ''' <param name="col"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function retainAll(ByVal col As ICollection) As Boolean


        ''' <summary>
        ''' Returns the number of elements in this list.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function size() As Integer

        ''' <summary>
        ''' Returns an array containing all of the elements in this list in proper sequence (from first to last element).
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function toArray() As Object()

        ''' <summary>
        ''' Returns an array containing all of the elements in this list in proper sequence (from first to last element); the runtime type of the returned array is that of the specified array.
        ''' </summary>
        ''' <typeparam name="T"></typeparam>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function toArray(Of T)() As T()



    End Interface

End Namespace
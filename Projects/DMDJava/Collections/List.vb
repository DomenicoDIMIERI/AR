Namespace java
    ''' <summary>
    ''' public interface List 
    ''' extends Collection 
    ''' An ordered collection (also known as a sequence). The user of this interface has precise control over where in the list each element is inserted. The user can access elements by their integer index (position in the list), and search for elements in the list.
    ''' Unlike sets, lists typically allow duplicate elements. More formally, lists typically allow pairs of elements e1 and e2 such that e1.equals(e2), and they typically allow multiple null elements if they allow null elements at all. It is not inconceivable that someone might wish to implement a list that prohibits duplicates, by throwing runtime exceptions when the user attempts to insert them, but we expect this usage to be rare.
    ''' The List interface places additional stipulations, beyond those specified in the Collection interface, on the contracts of the iterator, add, remove, equals, and hashCode methods. Declarations for other inherited methods are also included here for convenience.
    ''' The List interface provides four methods for positional (indexed) access to list elements. Lists (like Java arrays) are zero based. Note that these operations may execute in time proportional to the index value for some implementations (the LinkedList class, for example). Thus, iterating over the elements in a list is typically preferable to indexing through it if the caller does not know the implementation.
    ''' The List interface provides a special iterator, called a ListIterator, that allows element insertion and replacement, and bidirectional access in addition to the normal operations that the Iterator interface provides. A method is provided to obtain a list iterator that starts at a specified position in the list.
    ''' The List interface provides two methods to search for a specified object. From a performance standpoint, these methods should be used with caution. In many implementations they will perform costly linear searches.
    ''' The List interface provides two methods to efficiently insert and remove multiple elements at an arbitrary point in the list.
    ''' Note: While it is permissible for lists to contain themselves as elements, extreme caution is advised: the equals and hashCode methods are no longer well defined on such a list.
    ''' Some list implementations have restrictions on the elements that they may contain. For example, some implementations prohibit null elements, and some have restrictions on the types of their elements. Attempting to add an ineligible element throws an unchecked exception, typically NullPointerException or ClassCastException. Attempting to query the presence of an ineligible element may throw an exception, or it may simply return false; some implementations will exhibit the former behavior and some will exhibit the latter. More generally, attempting an operation on an ineligible element whose completion would not result in the insertion of an ineligible element into the list may throw an exception or it may succeed, at the option of the implementation. Such exceptions are marked as "optional" in the specification for this interface.
    ''' This interface is a member of the Java Collections Framework.
    ''' </summary>
    ''' <remarks></remarks>
    Public Interface List
        Inherits ICollection



        ''' <summary>
        ''' Inserts the specified element at the specified position in this list (optional operation).
        ''' </summary>
        ''' <param name="index"></param>
        ''' <param name="o"></param>
        ''' <remarks></remarks>
        Overloads Sub add(ByVal index As Integer, ByVal o As Object)



        ''' <summary>
        ''' Inserts all of the elements in the specified collection into this list at the specified position (optional operation).
        ''' </summary>
        ''' <param name="fromIndex"></param>
        ''' <param name="col"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Overloads Function addAll(ByVal fromIndex As Integer, ByVal col As ICollection) As Boolean

        ''' <summary>
        ''' Returns the element at the specified position in this list.
        ''' </summary>
        ''' <param name="i"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function [get](i As Integer) As Object


        ''' <summary>
        ''' Returns the index of the first occurrence of the specified element in this list, or -1 if this list does not contain the element.
        ''' </summary>
        ''' <param name="o"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function indexOf(ByVal o As Object) As Integer


        ''' <summary>
        ''' Returns the index of the last occurrence of the specified element in this list, or -1 if this list does not contain the element.
        ''' </summary>
        ''' <param name="o"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function lastIndexOf(ByVal o As Object) As Integer

        '    ListIterator<E> 	listIterator()
        'Returns a list iterator over the elements in this list (in proper sequence).
        'ListIterator<E> 	listIterator(int index)
        'Returns a list iterator over the elements in this list (in proper sequence), starting at the specified position in the list.

        ''' <summary>
        ''' Removes the element at the specified position in this list (optional operation).
        ''' </summary>
        ''' <param name="index"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Overloads Function remove(ByVal index As Integer) As Object



        ''' <summary>
        ''' Replaces the element at the specified position in this list with the specified element (optional operation).
        ''' </summary>
        ''' <param name="index"></param>
        ''' <param name="element"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function [set](index As Integer, element As Object) As Object


        ''' <summary>
        ''' Returns a view of the portion of this list between the specified fromIndex, inclusive, and toIndex, exclusive.
        ''' </summary>
        ''' <param name="fromIndex"></param>
        ''' <param name="toIndex"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Shadows Function subList(fromIndex As Integer, toIndex As Integer) As List


    End Interface

End Namespace
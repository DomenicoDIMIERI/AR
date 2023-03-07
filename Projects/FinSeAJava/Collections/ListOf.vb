Namespace java
    Public Interface List(Of T)
        Inherits List, Global.System.Collections.Generic.IEnumerable(Of T), ICollection(Of T)

        Shadows Function [get](index As Integer) As T
        Shadows Function [set](index As Integer, element As T) As T
        Shadows Function add(ByVal o As T) As Boolean
        Shadows Sub add(ByVal index As Integer, ByVal o As T)
        Shadows Function remove(ByVal o As T) As Boolean
        Shadows Function remove(ByVal index As Integer) As T
        Shadows Function addAll(ByVal col As ICollection(Of T)) As Boolean
        Shadows Function addAll(ByVal fromIndex As Integer, ByVal col As ICollection(Of T)) As Boolean
        Shadows Function indexOf(ByVal o As T) As Integer
        Shadows Function lastIndexOf(ByVal o As T) As Integer

        ''' <summary>
        ''' Returns a view of the portion of this list between the specified fromIndex, inclusive, and toIndex, exclusive.
        ''' </summary>
        ''' <param name="fromIndex"></param>
        ''' <param name="toIndex"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Shadows Function subList(fromIndex As Integer, toIndex As Integer) As List(Of T)

    End Interface

End Namespace
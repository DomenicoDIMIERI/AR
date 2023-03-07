Namespace Drawings

    ''' <summary>
    ''' public interface Pageable
    ''' The Pageable implementation represents a set of pages to be printed. The Pageable object returns the total number of pages in the set as well as the PageFormat and Printable for a specified page.
    ''' </summary>
    ''' <remarks></remarks>
    Public Interface Pageable

        'This constant is returned from the getNumberOfPages method if a Pageable implementation does not know the number of pages in its set.
        ' UNKNOWN_NUMBER_OF_PAGES as Integer = 0

        ''' <summary>
        ''' Returns the number of pages in the set.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function getNumberOfPages() As Integer

        ''' <summary>
        ''' Returns the PageFormat of the page specified by pageIndex.
        ''' </summary>
        ''' <param name="pageIndex"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function getPageFormat(ByVal pageIndex As Integer) As PageFormat

        ''' <summary>
        ''' Returns the Printable instance responsible for rendering the page specified by pageIndex.
        ''' </summary>
        ''' <param name="pageIndex"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function getPrintable(ByVal pageIndex As Integer) As Printable

    End Interface

End Namespace
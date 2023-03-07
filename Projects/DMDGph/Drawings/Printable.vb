Namespace Drawings

    '    Public Interface Printable

    'The Printable interface is implemented by the print methods of the current page painter, which is called by the printing system to render a page. When building a Pageable, pairs of PageFormat instances and instances that implement Me interface are used to describe each page. The instance implementing Printable is called to print the page's graphics.

    'A Printable(..) may be set on a PrinterJob. When the client subsequently initiates printing by calling PrinterJob.print(..) control

    'is handed to the printing system until all pages have been printed. It does Me by calling Printable.print(..) until all pages in the document have been printed. In using the Printable interface the printing commits to image the contents of a page whenever requested by the printing system.

    'The parameters to Printable.print(..) include a PageFormat which describes the printable area of the page, needed for calculating the contents that will fit the page, and the page index, which specifies the zero-based print stream index of the requested page.

    'For correct printing behaviour, the following points should be observed:

    '    The printing system may request a page index more than once. On each occasion equal PageFormat parameters will be supplied.
    '    The printing system will call Printable.print(..) with page indexes which increase monotonically, although as noted above, the Printable should expect multiple calls for a page index and that page indexes may be skipped, when page ranges are specified by the client, or by a user through a print dialog.
    '    If multiple collated copies of a document are requested, and the printer cannot natively support Me, then the document may be imaged multiple times. Printing will start each copy from the lowest print stream page index page.
    '    With the exception of re-imaging an entire document for multiple collated copies, the increasing page index order means that when page N is requested if a client needs to calculate page break position, it may safely discard any state related to pages < N, and make current that for page N. "State" usually is just the calculated position in the document that corresponds to the start of the page.
    '    When called by the printing system the Printable must inspect and honour the supplied PageFormat parameter as well as the page index. The format of the page to be drawn is specified by the supplied PageFormat. The size, orientation and imageable area of the page is therefore already determined and rendering must be within Me imageable area. This is key to correct printing behaviour, and it has the implication that the client has the responsibility of tracking what content belongs on the specified page.
    '    When the Printable is obtained from a client-supplied Pageable then the client may provide different PageFormats for each page index. Calculations of page breaks must account for Me. 
    Public Interface Printable

        Enum PrintResEnum
            ''' <summary>
            ''' Returned from print to signify that the pageIndex is too large and that the requested page does not exist.
            ''' </summary>
            ''' <remarks></remarks>
            NO_SUCH_PAGE

            ''' <summary>
            ''' Returned from print(Graphics, PageFormat, int) to signify that the requested page was rendered.
            ''' </summary>
            ''' <remarks></remarks>
            PAGE_EXISTS
        End Enum
        ''' <summary>
        ''' Prints the page at the specified index into the specified Graphics context in the specified format.
        ''' </summary>
        ''' <param name="graphics"></param>
        ''' <param name="pageFormat"></param>
        ''' <param name="pageIndex"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function print(ByVal graphics As Graphics2D, ByVal pageFormat As PageFormat, ByVal pageIndex As Integer) As PrintResEnum

    End Interface

End Namespace
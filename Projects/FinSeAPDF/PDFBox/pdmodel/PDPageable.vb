Imports System.Drawing
Imports FinSeA.Drawings
Imports FinSeA.Io
Imports FinSeA.org.apache.pdfbox.pdfviewer
Imports FinSeA.org.apache.pdfbox.pdmodel.common

Namespace org.apache.pdfbox.pdmodel

    '/**
    ' * Adapter class that implements the {@link Pageable} and {@link Printable}
    ' * interfaces for printing a given PDF document. Note that the given PDF
    ' * document should not be modified (pages added, removed, etc.) while an
    ' * instance of this class is being used.
    ' *
    ' * @since Apache PDFBox 1.3.0
    ' * @see <a href="https://issues.apache.org/jira/browse/PDFBOX-788">PDFBOX-788</a>
    ' */
    Public Class PDPageable
        Implements Pageable, Printable

        ''' <summary>
        ''' List of all pages in the given PDF document.
        ''' </summary>
        ''' <remarks></remarks>
        Private pages As List(Of PDPage) = New ArrayList(Of PDPage)()

        ''' <summary>
        ''' The printer job for printing the given PDF document.
        ''' </summary>
        ''' <remarks></remarks>
        Private job As PrinterJob

        '/**
        ' * Creates a {@link Pageable} adapter for the given PDF document and
        ' * printer job.
        ' *
        ' * @param document PDF document
        ' * @param printerJob printer job
        ' * @throws IllegalArgumentException if an argument is <code>null</code>
        ' * @throws PrinterException if the document permissions prevent printing
        ' */
        Public Sub New(ByVal document As PDDocument, ByVal printerJob As PrinterJob)  'throws IllegalArgumentException, PrinterException
            If (document Is Nothing) Then
                Throw New ArgumentNullException("document")
            End If
            If (printerJob Is Nothing) Then
                Throw New ArgumentNullException("printerJob")
            End If

            If (Not document.getCurrentAccessPermission().canPrint()) Then
                Throw New PermissionDeniedException("You do not have permission to print this document")
            Else
                document.getDocumentCatalog().getPages().getAllKids(pages)
                job = printerJob
            End If
        End Sub

        '/**
        ' * Creates a {@link Pageable} adapter for the given PDF document using
        ' * a default printer job returned by {@link PrinterJob#getPrinterJob()}.
        ' *
        ' * @param document PDF document
        ' * @throws IllegalArgumentException if the argument is <code>null</code>
        ' * @throws PrinterException if the document permissions prevent printing
        ' */
        Public Sub New(ByVal document As PDDocument)  'throws IllegalArgumentException, PrinterException
            Me.New(document, PrinterJob.getPrinterJob())
        End Sub

        '/**
        ' * Returns the printer job for printing the given PDF document.
        ' *
        ' * @return printer job
        ' */
        Public Function getPrinterJob() As PrinterJob
            Return job
        End Function

        '//------------------------------------------------------------< Pageable >

        '/**
        ' * Returns the number of pages in the given PDF document.
        ' *
        ' * @return number of pages
        ' */
        Public Function getNumberOfPages() As Integer Implements Pageable.getNumberOfPages
            Return pages.size()
        End Function

        '/**
        ' * Returns the format of the page at the given index.
        ' *
        ' * @param i page index, zero-based
        ' * @return page format
        ' * @throws IndexOutOfBoundsException if the page index is invalid
        ' */
        Public Function getPageFormat(ByVal i As Integer) As PageFormat Implements Pageable.getPageFormat
            Dim format As PageFormat = job.defaultPage()

            Dim page As PDPage = pages.get(i) ' can throw IOOBE
            Dim media As SizeF = page.findMediaBox().createDimension()
            Dim crop As SizeF = page.findCropBox().createDimension()

            ' Center the ImageableArea if the crop is smaller than the media
            Dim diffWidth As Double = 0.0
            Dim diffHeight As Double = 0.0
            If (Not media.Equals(crop)) Then
                diffWidth = (media.Width - crop.Width) / 2.0
                diffHeight = (media.Height - crop.Height) / 2.0
            End If

            Dim paper As Paper = format.getPaper()
            If (media.Width < media.Height) Then
                format.setOrientation(PageFormat.PORTRAIT)
                paper.setImageableArea(diffWidth, diffHeight, crop.Width, crop.Height)
            Else
                format.setOrientation(PageFormat.LANDSCAPE)
                paper.setImageableArea(diffHeight, diffWidth, crop.Height, crop.Width)
            End If
            format.setPaper(paper)

            Return format
        End Function

        '/**
        ' * Returns a {@link Printable} for the page at the given index.
        ' * Currently this method simply returns the underlying {@link PDPage}
        ' * object that directly implements the {@link Printable} interface, but
        ' * future versions may choose to return a different adapter instance.
        ' *
        ' * @param i page index, zero-based
        ' * @return printable
        ' * @throws IndexOutOfBoundsException if the page index is invalid
        ' */
        Public Function getPrintable(ByVal i As Integer) As Printable Implements Pageable.getPrintable
            Return pages.get(i)
        End Function

        '//-----------------------------------------------------------< Printable >

        '/**
        ' * Prints the page at the given index.
        ' *
        ' * @param graphics printing target
        ' * @param format page format
        ' * @param i page index, zero-based
        ' * @return {@link Printable#PAGE_EXISTS} if the page was printed,
        ' *         or {@link Printable#NO_SUCH_PAGE} if page index was invalid
        ' * @throws PrinterException if printing failed
        ' */
        Public Function print(ByVal graphics As Graphics2D, ByVal format As PageFormat, ByVal i As Integer) As Printable.PrintResEnum Implements Printable.print
            If (0 <= i AndAlso i < pages.size()) Then
                'Try
                Dim page As PDPage = pages.get(i)
                Dim cropBox As PDRectangle = page.findCropBox()
                Dim drawer As PageDrawer = New PageDrawer()
                drawer.drawPage(graphics, page, cropBox.createDimension())
                drawer.dispose()
                Return Printable.PrintResEnum.PAGE_EXISTS
                'Catch io As IOException
                '    Throw New io(io)
                'End Try
            Else
                Return Printable.PrintResEnum.NO_SUCH_PAGE
            End If
        End Function

    End Class

End Namespace

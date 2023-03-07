Imports FinSeA.org.apache.pdfbox.pdmodel
Imports System.IO

Namespace org.apache.pdfbox.util

    '/**
    ' * Split a document into several other documents.
    ' *
    ' * @author Mario Ivankovits (mario@ops.co.at)
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.7 $
    ' */
    Public Class Splitter


        '/**
        ' * The source PDF document.
        ' */
        Protected pdfDocument As PDDocument

        '/**
        ' * The current PDF document that contains the splitted page.
        ' */
        Protected currentDocument As PDDocument = Nothing

        Private splitAtPage As Integer = 1
        Private startPage As Integer = Integer.MinValue
        Private endPage As Integer = Integer.MaxValue
        Private newDocuments As List(Of PDDocument) = Nothing

        '/**
        ' * The current page number that we are processing, zero based.
        ' */
        Protected pageNumber As Integer = 0

        '/**
        ' * This will take a document and split into several other documents.
        ' *
        ' * @param document The document to split.
        ' *
        ' * @return A list of all the split documents.
        ' *
        ' * @throws IOException If there is an IOError
        ' */
        Public Function split(ByVal document As PDDocument) As List(Of PDDocument)
            newDocuments = New ArrayList(Of PDDocument)()
            pdfDocument = document

            Dim pages As List = pdfDocument.getDocumentCatalog().getAllPages()
            processPages(pages)
            Return newDocuments
        End Function

        '/**
        ' * This will tell the splitting algorithm where to split the pages.  The default
        ' * is 1, so every page will become a new document.  If it was to then each document would
        ' * contain 2 pages.  So it the source document had 5 pages it would split into
        ' * 3 new documents, 2 documents containing 2 pages and 1 document containing one
        ' * page.
        ' *
        ' * @param split The number of pages each split document should contain.
        ' */
        Public Sub setSplitAtPage(ByVal split As Integer)
            If (split <= 0) Then
                Throw New RuntimeException("Error split must be at least one page.")
            End If
            splitAtPage = split
        End Sub

        '/**
        ' * This will return how many pages each split document will contain.
        ' *
        ' * @return The split parameter.
        ' */
        Public Function getSplitAtPage() As Integer
            Return splitAtPage
        End Function

        '/**
        ' * This will set the start page.
        ' * 
        ' * @param start the start page
        ' */
        Public Sub setStartPage(ByVal start As Integer)
            If (start <= 0) Then
                Throw New RuntimeException("Error split must be at least one page.")
            End If
            startPage = start
        End Sub

        '/**
        ' * This will return the start page.
        ' *
        ' * @return The start page.
        ' */
        Public Function getStartPage() As Integer
            Return startPage
        End Function

        '/**
        ' * This will set the end page.
        ' * 
        ' * @param end the end page
        ' */
        Public Sub setEndPage(ByVal [end] As Integer)
            If ([end] <= 0) Then
                Throw New RuntimeException("Error split must be at least one page.")
            End If
            endPage = [end]
        End Sub

        '/**
        ' * This will return the end page.
        ' *
        ' * @return The end page.
        ' */
        Public Function getEndPage() As Integer
            Return endPage
        End Function

        '/**
        ' * Interface method to handle the start of the page processing.
        ' *
        ' * @param pages The list of pages from the source document.
        ' *
        ' * @throws IOException If an IO error occurs.
        ' */
        Protected Sub processPages(ByVal pages As List)
            Dim iter As Global.System.Collections. IEnumerator = pages.iterator()
            While (iter.MoveNext)
                Dim page As PDPage = iter.Current
                If (pageNumber + 1 >= startPage AndAlso pageNumber + 1 <= endPage) Then
                    processNextPage(page)
                Else
                    If (pageNumber > endPage) Then
                        Exit While
                    Else
                        pageNumber += 1
                    End If
                End If
            End While
        End Sub

        '/**
        ' * Interface method, you can control where a document gets split by implementing
        ' * this method.  By default a split occurs at every page.  If you wanted to split
        ' * based on some complex logic then you could override this method.  For example.
        ' * <code>
        ' * protected void createNewDocumentIfNecessary()
        ' * {
        ' *     if( isPrime( pageNumber ) )
        ' *     {
        ' *         super.createNewDocumentIfNecessary();
        ' *     }
        ' * }
        ' * </code>
        ' *
        ' * @throws IOException If there is an error creating the new document.
        ' */
        Protected Sub createNewDocumentIfNecessary()
            If (isNewDocNecessary()) Then
                createNewDocument()
            End If
        End Sub

        '/**
        ' * Check if it is necessary to create a new document.
        ' *
        ' * @return true If a new document should be created.
        ' */
        Protected Function isNewDocNecessary() As Boolean
            Return (pageNumber Mod splitAtPage = 0) OrElse currentDocument Is Nothing
        End Function

        '/**
        ' * Create a new document to write the splitted contents to.
        ' *
        ' * @throws IOException If there is an problem creating the new document.
        ' */
        Protected Sub createNewDocument()
            currentDocument = New PDDocument()
            currentDocument.setDocumentInformation(pdfDocument.getDocumentInformation())
            currentDocument.getDocumentCatalog().setViewerPreferences(pdfDocument.getDocumentCatalog().getViewerPreferences())
            newDocuments.add(currentDocument)
        End Sub

        '/**
        ' * Interface to start processing a new page.
        ' *
        ' * @param page The page that is about to get processed.
        ' *
        ' * @throws IOException If there is an error creating the new document.
        ' */
        Protected Sub processNextPage(ByVal page As PDPage)  'throws IOException
            createNewDocumentIfNecessary()
            Dim imported As PDPage = currentDocument.importPage(page)
            imported.setCropBox(page.findCropBox())
            imported.setMediaBox(page.findMediaBox())
            ' only the resources of the page will be copied
            imported.setResources(page.getResources())
            imported.setRotation(page.findRotation())
            pageNumber += 1
        End Sub

    End Class

End Namespace

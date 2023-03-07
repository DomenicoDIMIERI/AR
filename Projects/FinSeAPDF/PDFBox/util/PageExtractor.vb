Imports System.IO
Imports FinSeA.org.apache.pdfbox.pdmodel

Namespace org.apache.pdfbox.util


    ' @author Adam Nichols (adam@apache.org)

    ''' <summary>
    ''' This class will extract one or more sequential pages and create a new document.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class PageExtractor
        Protected sourceDocument As PDDocument
        Protected startPage As Integer = 1 ' first page to extract is page 1 (by default)
        Protected endPage As Integer = 0

        ''' <summary>
        ''' Creates a new instance of PageExtractor
        ''' </summary>
        ''' <param name="sourceDocument">The document to split</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal sourceDocument As PDDocument)
            Me.sourceDocument = sourceDocument
            Me.endPage = sourceDocument.getNumberOfPages()
        End Sub

        ''' <summary>
        ''' Creates a new instance of PageExtractor
        ''' </summary>
        ''' <param name="sourceDocument">The document to split</param>
        ''' <param name="startPage">The first page you want extracted (inclusive)</param>
        ''' <param name="endPage">The last page you want extracted (inclusive)</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal sourceDocument As PDDocument, ByVal startPage As Integer, ByVal endPage As Integer)
            Me.New(sourceDocument)
            Me.startPage = startPage
            Me.endPage = endPage
        End Sub

        '/**
        ' * This will take a document and extract the desired pages into a new 
        ' * document.  Both startPage and endPage are included in the extracted 
        ' * document.  If the endPage is greater than the number of pages in the 
        ' * source document, it will go to the end of the document.  If startPage is
        ' * less than 1, it'll start with page 1.  If startPage is greater than 
        ' * endPage or greater than the number of pages in the source document, a 
        ' * blank document will be returned.
        ' * 
        ' * @return The extracted document
        ' * @throws IOException If there is an IOError
        ' */
        Public Function extract() As PDDocument ' throws IOException {
            Dim extractedDocument As New PDDocument()
            extractedDocument.setDocumentInformation(sourceDocument.getDocumentInformation())
            extractedDocument.getDocumentCatalog().setViewerPreferences(sourceDocument.getDocumentCatalog().getViewerPreferences())

            Dim pages As CCollection(Of PDPage) = sourceDocument.getDocumentCatalog().getAllPages()
            Dim pageCounter As Integer = 1
            For Each page As PDPage In pages
                If (pageCounter >= startPage AndAlso pageCounter <= endPage) Then
                    Dim imported As PDPage = extractedDocument.importPage(page)
                    imported.setCropBox(page.findCropBox())
                    imported.setMediaBox(page.findMediaBox())
                    imported.setResources(page.findResources())
                    imported.setRotation(page.findRotation())
                End If
                pageCounter += 1
            Next

            Return extractedDocument
        End Function

        ''' <summary>
        ''' Gets the first page number to be extracted.
        ''' </summary>
        ''' <returns>return the first page number which should be extracted</returns>
        ''' <remarks></remarks>
        Public Function getStartPage() As Integer
            Return Me.startPage
        End Function

        '/**
        ' * Sets the first page number to be extracted.
        ' * @param startPage the first page number which should be extracted
        ' */
        Public Sub setStartPage(ByVal startPage As Integer)
            Me.startPage = startPage
        End Sub

        '/**
        ' * Gets the last page number (inclusive) to be extracted.
        ' * @return the last page number which should be extracted
        ' */
        Public Function getEndPage() As Integer
            Return endPage
        End Function

        '/**
        ' * Sets the last page number to be extracted.
        ' * @param endPage the last page number which should be extracted
        ' */
        Public Sub setEndPage(ByVal endPage As Integer)
            Me.endPage = endPage
        End Sub

    End Class

End Namespace

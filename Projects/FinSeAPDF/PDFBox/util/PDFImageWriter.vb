Imports FinSeA.Exceptions
Imports FinSeA.Drawings
Imports FinSeA.org.apache.pdfbox.pdmodel

Namespace org.apache.pdfbox.util

    '/**
    ' * This class writes single pages of a pdf to a file.
    ' * 
    ' * @author <a href="mailto:DanielWilson@Users.SourceForge.net">Daniel Wilson</a>
    ' * @version $Revision: 1.1 $
    ' */
    Public Class PDFImageWriter
        Inherits PDFStreamEngine

    
        '/**
        ' * Instantiate a new PDFImageWriter object.
        ' */
        Public Sub New()
        End Sub

        '/**
        ' * Instantiate a new PDFImageWriter object. Loading all of the operator mappings from the properties object that is
        ' * passed in.
        ' * 
        ' * @param props
        ' *            The properties containing the mapping of operators to PDFOperator classes.
        ' * 
        ' * @throws IOException
        ' *             If there is an error reading the properties.
        ' */
        Public Sub New(ByVal props As Properties)  'throws IOException
            MyBase.New(props)
        End Sub

        '/**
        ' * Converts a given page range of a PDF document to bitmap images.
        ' * 
        ' * @param document
        ' *            the PDF document
        ' * @param imageType
        ' *            the target format (ex. "png")
        ' * @param password
        ' *            the password (needed if the PDF is encrypted)
        ' * @param startPage
        ' *            the start page (1 is the first page)
        ' * @param endPage
        ' *            the end page (set to Integer.MAX_VALUE for all pages)
        ' * @param outputPrefix
        ' *            used to construct the filename for the individual images
        ' * @return true if the images were produced, false if there was an error
        ' * @throws IOException
        ' *             if an I/O error occurs
        ' */
        Public Function writeImage(ByVal document As PDDocument, ByVal imageType As String, ByVal password As String, ByVal startPage As Integer, ByVal endPage As Integer, ByVal outputPrefix As String) As Boolean
            Dim resolution As Integer
            Try
                resolution = Toolkit.getDefaultToolkit().getScreenResolution()
            Catch e As HeadlessException
                resolution = 96
            End Try
            Return writeImage(document, imageType, password, startPage, endPage, outputPrefix, 8, resolution)
        End Function

        '/**
        ' * Converts a given page range of a PDF document to bitmap images.
        ' * 
        ' * @param document
        ' *            the PDF document
        ' * @param imageFormat
        ' *            the target format (ex. "png")
        ' * @param password
        ' *            the password (needed if the PDF is encrypted)
        ' * @param startPage
        ' *            the start page (1 is the first page)
        ' * @param endPage
        ' *            the end page (set to Integer.MAX_VALUE for all pages)
        ' * @param outputPrefix
        ' *            used to construct the filename for the individual images
        ' * @param imageType
        ' *            the image type (see {@link BufferedImage}.TYPE_*)
        ' * @param resolution
        ' *            the resolution in dpi (dots per inch)
        ' * @return true if the images were produced, false if there was an error
        ' * @throws IOException
        ' *             if an I/O error occurs
        ' */
        Public Function writeImage(ByVal document As PDDocument, ByVal imageFormat As String, ByVal password As String, ByVal startPage As Integer, ByVal endPage As Integer, ByVal outputPrefix As String, ByVal imageType As Integer, ByVal resolution As Integer) As Boolean
            Dim bSuccess As Boolean = True
            Dim pages As List(Of PDPage) = document.getDocumentCatalog().getAllPages()
            Dim pagesSize As Integer = pages.size()
            For i As Integer = startPage - 1 To endPage - 1
                If (i >= pagesSize) Then Exit For
                Dim page As PDPage = pages.get(i)
                Dim image As BufferedImage = page.convertToImage(imageType, resolution)
                Dim fileName As String = outputPrefix + (i + 1)
                LOG.info("Writing: " & fileName & "." & imageFormat)
                bSuccess &= ImageIOUtil.writeImage(image, imageFormat, fileName, imageType, resolution)
            Next
            Return bSuccess
        End Function

    End Class


End Namespace

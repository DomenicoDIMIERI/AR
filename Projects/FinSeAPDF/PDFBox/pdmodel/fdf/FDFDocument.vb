Imports System.IO
Imports FinSeA.Io

Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.exceptions
Imports FinSeA.org.apache.pdfbox.pdfparser
Imports FinSeA.org.apache.pdfbox.pdfwriter
Imports FinSeA.org.apache.pdfbox.util

'import org.w3c.dom.Document;
'import org.w3c.dom.Element;

Namespace org.apache.pdfbox.pdmodel.fdf


    '/**
    ' * This is the in-memory representation of the FDF document.  You need to call
    ' * close() on this object when you are done using it!!
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.6 $
    ' */
    Public Class FDFDocument

        Private document As COSDocument

        '/**
        ' * Constructor, creates a new FDF document.
        ' *
        ' * @throws IOException If there is an error creating this document.
        ' */
        Public Sub New() 'throws IOException
            document = New COSDocument()
            document.setHeaderString("%FDF-1.2")

            'First we need a trailer
            document.setTrailer(New COSDictionary())

            'Next we need the root dictionary.
            Dim catalog As FDFCatalog = New FDFCatalog()
            setCatalog(catalog)
        End Sub

        '/**
        ' * Constructor that uses an existing document.  The COSDocument that
        ' * is passed in must be valid.
        ' *
        ' * @param doc The COSDocument that this document wraps.
        ' */
        Public Sub New(ByVal doc As COSDocument)
            document = doc
        End Sub

        '/**
        ' * This will create an FDF document from an XFDF XML document.
        ' *
        ' * @param doc The XML document that contains the XFDF data.
        ' * @throws IOException If there is an error reading from the dom.
        ' */
        Public Sub New(ByVal doc As System.Xml.XmlDocument) 'throws IOException
            Me.New()
            Dim xfdf As System.Xml.XmlElement = doc.DocumentElement
            If (Not xfdf.Name.Equals("xfdf")) Then 'getNodeName
                Throw New IOException("Error while importing xfdf document, root should be 'xfdf' and not '" & xfdf.Name & "'") 'getNodeName
            End If
            Dim cat As FDFCatalog = New FDFCatalog(xfdf)
            setCatalog(cat)
        End Sub

        '/**
        ' * This will write this element as an XML document.
        ' *
        ' * @param output The stream to write the xml to.
        ' *
        ' * @throws IOException If there is an error writing the XML.
        ' */
        Public Sub writeXML(ByVal output As Writer) 'throws IOException
            output.write("<?xml version=""1.0"" encoding=""UTF-8""?>" & vbLf)
            output.write("<xfdf xmlns=""http://ns.adobe.com/xfdf/"" xml:space=""preserve"">" & vbLf)

            getCatalog().writeXML(output)

            output.write("</xfdf>" & vbLf)
        End Sub



        '/**
        ' * This will get the low level document.
        ' *
        ' * @return The document that this layer sits on top of.
        ' */
        Public Function getDocument() As COSDocument
            Return document
        End Function

        '/**
        ' * This will get the FDF Catalog.  This is guaranteed to not return null.
        ' *
        ' * @return The documents /Root dictionary
        ' */
        Public Function getCatalog() As FDFCatalog
            Dim retval As FDFCatalog = Nothing
            Dim trailer As COSDictionary = document.getTrailer()
            Dim root As COSDictionary = trailer.getDictionaryObject(COSName.ROOT)
            If (root Is Nothing) Then
                retval = New FDFCatalog()
                setCatalog(retval)
            Else
                retval = New FDFCatalog(root)
            End If
            Return retval
        End Function

        '/**
        ' * This will set the FDF catalog for this FDF document.
        ' *
        ' * @param cat The FDF catalog.
        ' */
        Public Sub setCatalog(ByVal cat As FDFCatalog)
            Dim trailer As COSDictionary = document.getTrailer()
            trailer.setItem(COSName.ROOT, cat)
        End Sub

        '/**
        ' * This will load a document from a file.
        ' *
        ' * @param filename The name of the file to load.
        ' *
        ' * @return The document that was loaded.
        ' *
        ' * @throws IOException If there is an error reading from the stream.
        ' */
        Public Shared Function load(ByVal filename As String) As FDFDocument 'throws IOException
            Dim fis As New FileStream(filename, FileMode.Open, FileAccess.Read)
            Dim bis As New BufferedInputStream(fis)
            Dim ret As FDFDocument = load(bis)
            bis.Dispose()
            Return ret
        End Function

        '/**
        ' * This will load a document from a file.
        ' *
        ' * @param file The name of the file to load.
        ' *
        ' * @return The document that was loaded.
        ' *
        ' * @throws IOException If there is an error reading from the stream.
        ' */
        Public Shared Function load(ByVal fileHandle As FinSeA.Io.File) As FDFDocument 'throws IOException
            'return load( new BufferedInputStream( new FileInputStream( file ) ) );
            Dim fis As New FinSeA.Io.FileInputStream(fileHandle)
            Dim bis As New BufferedInputStream(fis)
            Dim ret As FDFDocument = load(bis)
            bis.Dispose()
            Return ret
        End Function

        '/**
        ' * This will load a document from an input stream.
        ' *
        ' * @param input The stream that contains the document.
        ' *
        ' * @return The document that was loaded.
        ' *
        ' * @throws IOException If there is an error reading from the stream.
        ' */
        Public Shared Function load(ByVal input As InputStream) As FDFDocument 'throws IOException
            Dim parser As New pdfparser.PDFParser(input)
            parser.parse()
            Return parser.getFDFDocument()
        End Function

        '/**
        ' * This will load a document from a file.
        ' *
        ' * @param filename The name of the file to load.
        ' *
        ' * @return The document that was loaded.
        ' *
        ' * @throws IOException If there is an error reading from the stream.
        ' */
        Public Shared Function loadXFDF(ByVal filename As String) As FDFDocument ' throws IOException
            Dim fis As New FileStream(filename, FileMode.Open, FileAccess.Read)
            Dim bis As New BufferedInputStream(fis)
            Dim ret As FDFDocument = loadXFDF(bis)
            bis.Dispose()
            Return ret
        End Function

        '/**
        ' * This will load a document from a file.
        ' *
        ' * @param file The name of the file to load.
        ' *
        ' * @return The document that was loaded.
        ' *
        ' * @throws IOException If there is an error reading from the stream.
        ' */
        Public Shared Function loadXFDF(ByVal fileHandle As FinSeA.Io.File) As FDFDocument ' throws IOException
            Dim fis As New FinSeA.Io.FileInputStream(fileHandle)
            Dim bis As New BufferedInputStream(fis)
            Dim ret As FDFDocument = loadXFDF(bis)
            fis.Dispose()
            Return ret
        End Function

        '/**
        ' * This will load a document from an input stream.
        ' *
        ' * @param input The stream that contains the document.
        ' *
        ' * @return The document that was loaded.
        ' *
        ' * @throws IOException If there is an error reading from the stream.
        ' */
        Public Shared Function loadXFDF(ByVal input As InputStream) As FDFDocument 'throws IOException
            Dim doc As System.Xml.XmlDocument = XMLUtil.parse(input)
            Return New FDFDocument(doc)
        End Function

        '/**
        ' * This will save this document to the filesystem.
        ' *
        ' * @param fileName The file to save as.
        ' *
        ' * @throws IOException If there is an error saving the document.
        ' * @throws COSVisitorException If an error occurs while generating the data.
        ' */
        Public Sub save(ByVal fileHandle As FinSeA.Io.File) 'throws IOException, COSVisitorException
            Dim fos As New FinSeA.Io.FileOutputStream(fileHandle)
            'Dim fos As New FileOutputStream(stream)
            save(fos)
            fos.Dispose()
        End Sub

        '/**
        ' * This will save this document to the filesystem.
        ' *
        ' * @param fileName The file to save as.
        ' *
        ' * @throws IOException If there is an error saving the document.
        ' * @throws COSVisitorException If an error occurs while generating the data.
        ' */
        Public Sub save(ByVal fileName As String) 'throws IOException, COSVisitorException
            'save( new FileOutputStream( fileName ) );
            Dim fos As New FileStream(fileName, FileMode.Create, FileAccess.Write)
            'Dim fos As New FileOutputStream(stream)
            save(New OutputStream(fos))
            fos.Dispose()
        End Sub

        '/**
        ' * This will save the document to an output stream.
        ' *
        ' * @param output The stream to write to.
        ' *
        ' * @throws IOException If there is an error writing the document.
        ' * @throws COSVisitorException If an error occurs while generating the data.
        ' */
        Public Sub save(ByVal output As OutputStream)  'throws IOException, COSVisitorException
            Dim writer As COSWriter = Nothing
            Try
                writer = New COSWriter(output)
                writer.write(document)
                writer.close()
            Finally
                If (writer IsNot Nothing) Then
                    writer.close()
                End If
            End Try
        End Sub

        '/**
        ' * This will save this document to the filesystem.
        ' *
        ' * @param fileName The file to save as.
        ' *
        ' * @throws IOException If there is an error saving the document.
        ' * @throws COSVisitorException If an error occurs while generating the data.
        ' */
        Public Sub saveXFDF(ByVal fileHandle As IntPtr) 'throws IOException, COSVisitorException
            Dim bu As New BufferedWriter(New FileWriter(fileHandle))
            saveXFDF(bu)
            bu.close()
        End Sub

        '/**
        ' * This will save this document to the filesystem.
        ' *
        ' * @param fileName The file to save as.
        ' *
        ' * @throws IOException If there is an error saving the document.
        ' * @throws COSVisitorException If an error occurs while generating the data.
        ' */
        Public Sub saveXFDF(ByVal fileName As String)  'throws IOException, COSVisitorException
            Dim bu As New BufferedWriter(New FileWriter(fileName))
            saveXFDF(bu)
            bu.close()
        End Sub

        '/**
        ' * This will save the document to an output stream and close the stream.
        ' *
        ' * @param output The stream to write to.
        ' *
        ' * @throws IOException If there is an error writing the document.
        ' * @throws COSVisitorException If an error occurs while generating the data.
        ' */
        Public Sub saveXFDF(ByVal output As Writer) 'throws IOException, COSVisitorException
            'Try
            writeXML(output)
            'Finally
            'If (output IsNot Nothing) Then
            'output.close()
            'End If
            'End Try
        End Sub

        '/**
        ' * This will close the underlying COSDocument object.
        ' *
        ' * @throws IOException If there is an error releasing resources.
        ' */
        Public Sub close() 'throws IOException
            document.close()
        End Sub
    End Class

End Namespace

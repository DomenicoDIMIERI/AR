Imports System.IO
Imports FinSeA.Io

'imports  org.apache.jempbox.xmp.XMPMetadata;
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel
Imports FinSeA.Drawings

Namespace org.apache.pdfbox.pdmodel.common

    '/**
    ' * This class represents metadata for various objects in a PDF document.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.3 $
    ' */
    Public Class PDMetadata
        Inherits PDStream

        '/**
        ' * This will create a new PDMetadata object.
        ' *
        ' * @param document The document that the stream will be part of.
        ' */
        Public Sub New(ByVal document As PDDocument)
            MyBase.New(document)
            getStream().setName("Type", "Metadata")
            getStream().setName("Subtype", "XML")
        End Sub

        '/**
        ' * Constructor.  Reads all data from the input stream and embeds it into the
        ' * document, this will close the InputStream.
        ' *
        ' * @param doc The document that will hold the stream.
        ' * @param str The stream parameter.
        ' * @param filtered True if the stream already has a filter applied.
        ' * @throws IOException If there is an error creating the stream in the document.
        ' */
        Public Sub New(ByVal doc As PDDocument, ByVal str As InputStream, ByVal filtered As Boolean) 'throws IOException
            MyBase.New(doc, str, filtered)
            getStream().setName("Type", "Metadata")
            getStream().setName("Subtype", "XML")
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param str The stream parameter.
        ' */
        Public Sub New(ByVal str As COSStream)
            MyBase.New(str)
        End Sub

        '/**
        ' * Extract the XMP metadata and create and build an in memory object.
        ' * To persist changes back to the PDF you must call importXMPMetadata.
        ' *
        ' * @return A parsed XMP object.
        ' *
        ' * @throws IOException If there is an error parsing the XMP data.
        ' */
        Public Function exportXMPMetadata() As XMPMetadata 'throws IOException
            Return XMPMetadata.load(createInputStream())
        End Function

        '/**
        ' * Import an XMP stream into the PDF document.
        ' *
        ' * @param xmp The XMP data.
        ' *
        ' * @throws IOException If there is an error generating the XML document.
        ' * @throws TransformerException If there is an error generating the XML document.
        ' */
        Public Sub importXMPMetadata(ByVal xmp As XMPMetadata) 'throws(IOException, TransformerException)
            xmp.save(createOutputStream())
        End Sub


    End Class

End Namespace
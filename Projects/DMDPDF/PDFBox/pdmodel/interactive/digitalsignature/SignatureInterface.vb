Imports System.IO
Imports FinSeA.Io
Imports FinSeA.org.apache.pdfbox.exceptions


Namespace org.apache.pdfbox.pdmodel.interactive.digitalsignature

    '/**
    ' * Providing an interface for accessing necessary functions for signing a pdf document.
    ' * 
    ' * @author <a href="mailto:mail@thomas-chojecki.de">Thomas Chojecki</a>
    ' * @version $
    ' */
    Public Interface SignatureInterface

        '/**
        ' * Creates a cms signature for the given content
        ' * 
        ' * @param content is the content as a (Filter)InputStream
        ' * @return signature as a byte array
        ' */
        Function sign(ByVal content As InputStream) As Byte() 'throws SignatureException, IOException;

    End Interface

End Namespace

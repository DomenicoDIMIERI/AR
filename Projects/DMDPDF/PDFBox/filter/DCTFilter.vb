Imports System.IO
Imports FinSeA.Io
Imports FinSeA.org.apache.pdfbox.cos

Namespace org.apache.pdfbox.filter

    '/**
    ' * This is the used for the DCTDecode filter.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.10 $
    ' */
    Public Class DCTFilter
        Implements Filter

        '/**
        ' * Log instance.
        ' */
        'private static final Log log = LogFactory.getLog(DCTFilter.class);

        '/**
        ' * {@inheritDoc}
        ' */
        Public Sub decode(ByVal compressedData As InputStream, ByVal result As OutputStream, ByVal options As COSDictionary, ByVal filterIndex As Integer) Implements Filter.decode
            'log.warn( "DCTFilter.decode is not implemented yet, skipping this stream." );
            Throw New NotImplementedException("DCTFilter.decode is not implemented yet")
        End Sub

        '/**
        '* {@inheritDoc}
        '*/
        Public Sub encode(ByVal rawData As InputStream, ByVal result As OutputStream, ByVal options As COSDictionary, ByVal filterIndex As Integer) Implements Filter.encode
            Throw New NotImplementedException("DCTFilter.encode is not implemented yet, skipping this stream.")
        End Sub

    End Class

End Namespace

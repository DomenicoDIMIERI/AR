Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.Io

Namespace org.apache.pdfbox.filter


    '/**
    ' * This is the interface that will be used to apply filters to a byte stream.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.7 $
    ' */
    Public Interface Filter

        '/**
        ' * This will decode some compressed data.
        ' *
        ' * @param compressedData The compressed byte stream.
        ' * @param result The place to write the uncompressed byte stream.
        ' * @param options The options to use to encode the data.
        ' * @param filterIndex The index to the filter being decoded.
        ' *
        ' * @throws IOException If there is an error decompressing the stream.
        ' */
        Sub decode(ByVal compressedData As InputStream, ByVal result As OutputStream, ByVal options As COSDictionary, ByVal filterIndex As Integer) 'throws IOException;

        '/**
        ' * This will encode some data.
        ' *
        ' * @param rawData The raw data to encode.
        ' * @param result The place to write to encoded results to.
        ' * @param options The options to use to encode the data.
        ' * @param filterIndex The index to the filter being encoded.
        ' *
        ' * @throws IOException If there is an error compressing the stream.
        ' */
        Sub encode(ByVal rawData As InputStream, ByVal result As OutputStream, ByVal options As COSDictionary, ByVal filterIndex As Integer) 'throws IOException;

    End Interface

End Namespace


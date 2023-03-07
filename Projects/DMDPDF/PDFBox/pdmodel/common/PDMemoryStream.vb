Imports System.IO
Imports FinSeA.Io
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common.filespecification


Namespace org.apache.pdfbox.pdmodel.common

    '/**
    ' * A PDStream represents a stream in a PDF document.  Streams are tied to a single
    ' * PDF document.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.2 $
    ' */
    Public Class PDMemoryStream
        Inherits PDStream

        Private data() As Byte

        '/**
        ' * This will create a new PDStream object.
        ' *
        ' * @param buffer The data for this in memory stream.
        ' */
        Public Sub New(ByVal buffer() As Byte)
            data = buffer
        End Sub


        ' /**
        '* If there are not compression filters on the current stream then this
        '* will add a compression filter, flate compression for example.
        '*/
        Public Overrides Sub addCompression()
            '//no compression to add
        End Sub



        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Overrides Function getCOSObject() As COSBase
            Throw New NotSupportedException("not supported for memory stream")
        End Function

        '/**
        ' * This will get a stream that can be written to.
        ' *
        ' * @return An output stream to write data to.
        ' *
        ' * @throws IOException If an IO error occurs during writing.
        ' */
        Public Overrides Function createOutputStream() As OutputStream 'throws IOException
            Throw New NotSupportedException("not supported for memory stream")
        End Function

        '/**
        ' * This will get a stream that can be read from.
        ' *
        ' * @return An input stream that can be read from.
        ' *
        ' * @throws IOException If an IO error occurs during reading.
        ' */
        Public Overrides Function createInputStream() As InputStream ' throws IOException
            Return New ByteArrayInputStream(data)
        End Function

        '/**
        ' * This will get a stream with some filters applied but not others.  This is useful
        ' * when doing images, ie filters = [flate,dct], we want to remove flate but leave dct
        ' *
        ' * @param stopFilters A list of filters to stop decoding at.
        ' * @return A stream with decoded data.
        ' * @throws IOException If there is an error processing the stream.
        ' */
        Public Overrides Function getPartiallyFilteredStream(ByVal stopFilters As List(Of String)) As InputStream ' throws IOException
            Return createInputStream()
        End Function

        '/**
        ' * Get the cos stream associated with this object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Overrides Function getStream() As COSStream
            Throw New NotSupportedException("not supported for memory stream")
        End Function

        '/**
        ' * This will get the length of the filtered/compressed stream.  This is readonly in the
        ' * PD Model and will be managed by this class.
        ' *
        ' * @return The length of the filtered stream.
        ' */
        Public Overrides Function getLength() As Integer
            Return data.Length
        End Function

        '/**
        ' * This will get the list of filters that are associated with this stream.  Or
        ' * null if there are none.
        ' * @return A list of all encoding filters to apply to this stream.
        ' */
        Public Overrides Function getFilters() As List(Of COSName)
            Return Nothing
        End Function

        '/**
        ' * This will set the filters that are part of this stream.
        ' *
        ' * @param filters The filters that are part of this stream.
        ' */
        Public Overrides Sub setFilters(ByVal filters As List(Of COSName))
            Throw New NotSupportedException("not supported for memory stream")
        End Sub

        '/**
        ' * Get the list of decode parameters.  Each entry in the list will refer to
        ' * an entry in the filters list.
        ' *
        ' * @return The list of decode parameters.
        ' *
        ' * @throws IOException if there is an error retrieving the parameters.
        ' */
        Public Function getDecodeParams() As List  'throws IOException
            Return Nothing
        End Function

        '/**
        ' * This will set the list of decode params.
        ' *
        ' * @param decodeParams The list of decode params.
        ' */
        Public Sub setDecodeParams(ByVal decodeParams As List)
            'do nothing
        End Sub

        '/**
        ' * This will get the file specification for this stream.  This is only
        ' * required for external files.
        ' *
        ' * @return The file specification.
        ' */
        Public Overrides Function getFile() As PDFileSpecification
            Return Nothing
        End Function

        '/**
        ' * Set the file specification.
        ' * @param f The file specification.
        ' */
        Public Overrides Sub setFile(ByVal f As PDFileSpecification)
            'do nothing.
        End Sub

        '/**
        ' * This will get the list of filters that are associated with this stream.  Or
        ' * null if there are none.
        ' * @return A list of all encoding filters to apply to this stream.
        ' */
        Public Overrides Function getFileFilters() As List(Of String)
            Return Nothing
        End Function

        '/**
        ' * This will set the filters that are part of this stream.
        ' *
        ' * @param filters The filters that are part of this stream.
        ' */
        Public Overrides Sub setFileFilters(ByVal filters As List(Of String))
            'do nothing.
        End Sub

        '/**
        ' * Get the list of decode parameters.  Each entry in the list will refer to
        ' * an entry in the filters list.
        ' *
        ' * @return The list of decode parameters.
        ' *
        ' * @throws IOException if there is an error retrieving the parameters.
        ' */
        Public Overrides Function getFileDecodeParams() As List  'throws IOException
            Return Nothing
        End Function

        '/**
        ' * This will set the list of decode params.
        ' *
        ' * @param decodeParams The list of decode params.
        ' */
        Public Overrides Sub setFileDecodeParams(ByVal decodeParams As List)
            'do nothing
        End Sub

        '/**
        ' * This will copy the stream into a byte array.
        ' *
        ' * @return The byte array of the filteredStream
        ' * @throws IOException When getFilteredStream did not work
        ' */
        Public Overrides Function getByteArray() As Byte() ' throws IOException
            Return data
        End Function

        '/**
        ' * Get the metadata that is part of the document catalog.  This will
        ' * return null if there is no meta data for this object.
        ' *
        ' * @return The metadata for this object.
        ' */
        Public Overrides Function getMetadata() As PDMetadata
            Return Nothing
        End Function

        '/**
        ' * Set the metadata for this object.  This can be null.
        ' *
        ' * @param meta The meta data for this object.
        ' */
        Public Overrides Sub setMetadata(ByVal meta As PDMetadata)
            ' do nothing
        End Sub

    End Class

End Namespace
Imports System.IO

Imports FinSeA.Io
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.exceptions
Imports FinSeA.org.apache.pdfbox.io
Imports FinSeA.org.apache.pdfbox.pdfparser


Namespace org.apache.pdfbox.pdmodel.common


    '/**
    ' * This will take an array of streams and sequence them together.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.10 $
    ' */
    Public Class COSStreamArray
        Inherits COSStream

        Private streams As COSArray

        '/**
        ' * The first stream will be used to delegate some of the methods for this
        ' * class.
        ' */
        Private firstStream As COSStream

        '/**
        ' * Constructor.
        ' *
        ' * @param array The array of COSStreams to concatenate together.
        ' */
        Public Sub New(ByVal array As COSArray)
            MyBase.New(New COSDictionary(), Nothing)
            streams = array
            If (array.size() > 0) Then
                firstStream = array.getObject(0)
            End If
        End Sub

        '/**
        ' * This will get a stream (or the reference to a stream) from the array.
        ' *
        ' * @param index The index of the requested stream
        ' * @return The stream object or a reference to a stream
        ' */
        Public Function [get](ByVal index As Integer) As COSBase
            Return streams.get(index)
        End Function

        '/**
        ' * This will get the number of streams in the array.
        ' *
        ' * @return the number of streams
        ' */
        Public Function getStreamCount() As Integer
            Return streams.size()
        End Function

        '/**
        ' * This will get the scratch file associated with this stream.
        ' *
        ' * @return The scratch file where this stream is being stored.
        ' * 
        ' */
        Public Overrides Function getScratchFile() As RandomAccess
            Return firstStream.getScratchFile()
        End Function

        '/**
        ' * This will get an object from this streams dictionary.
        ' *
        ' * @param key The key to the object.
        ' *
        ' * @return The dictionary object with the key or null if one does not exist.
        ' */
        Public Overrides Function getItem(ByVal key As COSName) As COSBase
            Return firstStream.getItem(key)
        End Function

        '/**
        '  * This will get an object from this streams dictionary and dereference it
        '  * if necessary.
        '  *
        '  * @param key The key to the object.
        '  *
        '  * @return The dictionary object with the key or null if one does not exist.
        '  */
        Public Overrides Function getDictionaryObject(ByVal key As COSName) As COSBase
            Return firstStream.getDictionaryObject(key)
        End Function

        Public Overrides Function toString() As String
            Return "COSStream{}"
        End Function

        '/**
        ' * This will get all the tokens in the stream.
        ' *
        ' * @return All of the tokens in the stream.
        ' *
        ' * @throws IOException If there is an error parsing the stream.
        ' */
        Public Overrides Function getStreamTokens() As List ' throws IOException
            Dim retval As List = Nothing
            If (streams.size() > 0) Then
                Dim parser As PDFStreamParser = New PDFStreamParser(Me)
                parser.parse()
                retval = parser.getTokens()
            Else
                retval = New ArrayList()
            End If
            Return retval
        End Function

        '/**
        ' * This will get the dictionary that is associated with this stream.
        ' *
        ' * @return the object that is associated with this stream.
        ' */
        Public Function getDictionary() As COSDictionary
            Return firstStream
        End Function

        '/**
        ' * This will get the stream with all of the filters applied.
        ' *
        ' * @return the bytes of the physical (endoced) stream
        ' *
        ' * @throws IOException when encoding/decoding causes an exception
        ' */
        Public Overrides Function getFilteredStream() As InputStream ' throws IOException
            Throw New IOException("Error: Not allowed to get filtered stream from array of streams.")
        End Function

        '/**
        ' * This will get the logical content stream with none of the filters.
        ' *
        ' * @return the bytes of the logical (decoded) stream
        ' *
        ' * @throws IOException when encoding/decoding causes an exception
        ' */
        Public Overrides Function getUnfilteredStream() As InputStream ' throws IOException
            Dim inputStreams As Vector(Of InputStream) = New Vector(Of InputStream)
            Dim inbetweenStreamBytes() As Byte = Sistema.Strings.GetBytes(vbLf, "ISO-8859-1") '"\n".getBytes("ISO-8859-1");

            For i As Integer = 0 To streams.size() - 1
                Dim stream As COSStream = streams.getObject(i)
                inputStreams.add(stream.getUnfilteredStream())
                'handle the case where there is no whitespace in the
                'between streams in the contents array, without this
                'it is possible that two operators will get concatenated
                'together
                inputStreams.add(New ByteArrayInputStream(inbetweenStreamBytes))
            Next

            Return New SequenceInputStream(inputStreams.elements())
        End Function

        '/**
        ' * visitor pattern double dispatch method.
        ' *
        ' * @param visitor The object to notify when visiting this object.
        ' * @return any object, depending on the visitor implementation, or null
        ' * @throws COSVisitorException If an error occurs while visiting this object.
        ' */
        Public Overrides Function accept(ByVal visitor As ICOSVisitor) As Object 'throws COSVisitorException
            Return streams.accept(visitor)
        End Function


        '/**
        ' * This will return the filters to apply to the byte stream
        ' * the method will return.
        ' * - null if no filters are to be applied
        ' * - a COSName if one filter is to be applied
        ' * - a COSArray containing COSNames if multiple filters are to be applied
        ' *
        ' * @return the COSBase object representing the filters
        ' */
        Public Overrides Function getFilters() As COSBase
            Return firstStream.getFilters()
        End Function

        '/**
        ' * This will create a new stream for which filtered byte should be
        ' * written to.  You probably don't want this but want to use the
        ' * createUnfilteredStream, which is used to write raw bytes to.
        ' *
        ' * @return A stream that can be written to.
        ' *
        ' * @throws IOException If there is an error creating the stream.
        ' */
        Public Overrides Function createFilteredStream() As OutputStream ' throws IOException
            Return firstStream.createFilteredStream()
        End Function

        '/**
        ' * This will create a new stream for which filtered byte should be
        ' * written to.  You probably don't want this but want to use the
        ' * createUnfilteredStream, which is used to write raw bytes to.
        ' *
        ' * @param expectedLength An entry where a length is expected.
        ' *
        ' * @return A stream that can be written to.
        ' *
        ' * @throws IOException If there is an error creating the stream.
        ' */
        Public Overrides Function createFilteredStream(ByVal expectedLength As COSBase) As OutputStream 'throws IOException
            Return firstStream.createFilteredStream(expectedLength)
        End Function

        '/**
        ' * set the filters to be applied to the stream.
        ' *
        ' * @param filters The filters to set on this stream.
        ' *
        ' * @throws IOException If there is an error clearing the old filters.
        ' */
        Public Overrides Sub setFilters(ByVal filters As COSBase) 'throws IOException
            'should this be allowed?  Should this
            'propagate to all streams in the array?
            firstStream.setFilters(filters)
        End Sub

        '/**
        ' * This will create an output stream that can be written to.
        ' *
        ' * @return An output stream which raw data bytes should be written to.
        ' *
        ' * @throws IOException If there is an error creating the stream.
        ' */
        Public Overrides Function createUnfilteredStream() As OutputStream ' throws IOException
            Return firstStream.createUnfilteredStream()
        End Function

        '/**
        ' * Appends a new stream to the array that represents this object's stream.
        ' *
        ' * @param streamToAppend The stream to append.
        ' */
        Public Sub appendStream(ByVal streamToAppend As COSStream)
            streams.add(streamToAppend)
        End Sub

        '/**
        ' * Insert the given stream at the beginning of the existing stream array.
        ' * @param streamToBeInserted
        ' */
        Public Sub insertCOSStream(ByVal streamToBeInserted As PDStream)
            Dim tmp As COSArray = New COSArray()
            tmp.add(streamToBeInserted)
            tmp.addAll(streams)
            streams.clear()
            streams = tmp
        End Sub

    End Class

End Namespace
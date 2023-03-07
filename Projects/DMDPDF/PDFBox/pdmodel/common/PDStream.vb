Imports System.IO
Imports FinSeA.Io

Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.filter
Imports FinSeA.org.apache.pdfbox.pdmodel
Imports FinSeA.org.apache.pdfbox.pdmodel.common.filespecification

Namespace org.apache.pdfbox.pdmodel.common


    '/**
    ' * A PDStream represents a stream in a PDF document. Streams are tied to a
    ' * single PDF document.
    ' * 
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.17 $
    ' */
    Public Class PDStream
        Implements COSObjectable

        Private stream As COSStream

        '/**
        ' * This will create a new PDStream object.
        ' */
        Protected Sub New()
            ' should only be called by PDMemoryStream
        End Sub

        '/**
        ' * This will create a new PDStream object.
        ' * 
        ' * @param document
        ' *            The document that the stream will be part of.
        ' */
        Public Sub New(ByVal document As PDDocument)
            stream = document.getDocument().createCOSStream()
        End Sub

        '/**
        ' * Constructor.
        ' * 
        ' * @param str
        ' *            The stream parameter.
        ' */
        Public Sub New(ByVal str As COSStream)
            stream = str
        End Sub

        '/**
        ' * Constructor. Reads all data from the input stream and embeds it into the
        ' * document, this will close the InputStream.
        ' * 
        ' * @param doc
        ' *            The document that will hold the stream.
        ' * @param str
        ' *            The stream parameter.
        ' * @throws IOException
        ' *             If there is an error creating the stream in the document.
        ' */
        Public Sub New(ByVal doc As PDDocument, ByVal str As InputStream) ' throws IOException
            Me.New(doc, str, False)
        End Sub

        '/**
        ' * Constructor. Reads all data from the input stream and embeds it into the
        ' * document, this will close the InputStream.
        ' * 
        ' * @param doc
        ' *            The document that will hold the stream.
        ' * @param str
        ' *            The stream parameter.
        ' * @param filtered
        ' *            True if the stream already has a filter applied.
        ' * @throws IOException
        ' *             If there is an error creating the stream in the document.
        ' */
        Public Sub New(ByVal doc As PDDocument, ByVal str As InputStream, ByVal filtered As Boolean) 'throws IOException
            Dim output As OutputStream = Nothing
            Try
                stream = doc.getDocument().createCOSStream()
                If (filtered) Then
                    output = stream.createFilteredStream()
                Else
                    output = stream.createUnfilteredStream()
                End If
                Dim buffer() As Byte = Array.CreateInstance(GetType(Byte), 1024)
                Dim amountRead As Integer = -1
                amountRead = str.Read(buffer)
                While (amountRead > 0)
                    output.Write(buffer, 0, amountRead)
                    amountRead = str.Read(buffer)
                End While
            Finally
                If (output IsNot Nothing) Then
                    output.Close()
                End If
                If (str IsNot Nothing) Then
                    str.Close()
                End If
            End Try
        End Sub

        '/**
        ' * If there are not compression filters on the current stream then this will
        ' * add a compression filter, flate compression for example.
        ' */
        Public Overridable Sub addCompression()
            Dim filters As List(Of COSName) = getFilters()
            If (filters Is Nothing) Then
                filters = New ArrayList(Of COSName)
                filters.add(COSName.FLATE_DECODE)
                setFilters(filters)
            End If
        End Sub

        '/**
        ' * Create a pd stream from either a regular COSStream on a COSArray of cos
        ' * streams.
        ' * 
        ' * @param base
        ' *            Either a COSStream or COSArray.
        ' * @return A PDStream or null if base is null.
        ' * @throws IOException
        ' *             If there is an error creating the PDStream.
        ' */
        Public Shared Function createFromCOS(ByVal base As COSBase) As PDStream 'throws IOException
            Dim retval As PDStream = Nothing
            If (TypeOf (base) Is COSStream) Then
                retval = New PDStream(base)
            ElseIf (TypeOf (base) Is COSArray) Then
                If (DirectCast(base, COSArray).size() > 0) Then
                    retval = New PDStream(New COSStreamArray(base))
                End If
            Else
                If (base IsNot Nothing) Then
                    Throw New IOException("Contents are unknown type:" & base.GetType().Name)
                End If
            End If
            Return retval
        End Function

        '/**
        ' * Convert this standard java object to a COS object.
        ' * 
        ' * @return The cos object that matches this Java object.
        ' */
        Public Overridable Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return stream
        End Function

        '/**
        ' * This will get a stream that can be written to.
        ' * 
        ' * @return An output stream to write data to.
        ' * 
        ' * @throws IOException
        ' *             If an IO error occurs during writing.
        ' */
        Public Overridable Function createOutputStream() As OutputStream ' throws IOException
            Return stream.createUnfilteredStream()
        End Function

        '/**
        ' * This will get a stream that can be read from.
        ' * 
        ' * @return An input stream that can be read from.
        ' * 
        ' * @throws IOException
        ' *             If an IO error occurs during reading.
        ' */
        Public Overridable Function createInputStream() As InputStream ' throws IOException
            Return stream.getUnfilteredStream()
        End Function

        '/**
        ' * This will get a stream with some filters applied but not others. This is
        ' * useful when doing images, ie filters = [flate,dct], we want to remove
        ' * flate but leave dct
        ' * 
        ' * @param stopFilters
        ' *            A list of filters to stop decoding at.
        ' * @return A stream with decoded data.
        ' * @throws IOException
        ' *             If there is an error processing the stream.
        ' */
        Public Overridable Function getPartiallyFilteredStream(ByVal stopFilters As List(Of String)) As InputStream   'throws IOException
            Dim manager As FilterManager = stream.getFilterManager()
            Dim [is] As InputStream = stream.getFilteredStream()
            Dim os As ByteArrayOutputStream = New ByteArrayOutputStream()
            Dim filters As List(Of COSName) = getFilters()
            Dim nextFilter As COSName = Nothing
            Dim done As Boolean = False
            For i As Integer = 0 To filters.size() - 1
                If (done) Then Exit For
                os.reset()
                nextFilter = filters.get(i)

                If (stopFilters.contains(nextFilter.getName())) Then
                    done = True
                Else
                    Dim filter As pdfbox.filter.Filter = manager.getFilter(nextFilter)
                    filter.decode([is], os, stream, i)
                    [is] = New ByteArrayInputStream(os.toByteArray())
                End If
            Next
            Return [is]
        End Function

        '/**
        ' * Get the cos stream associated with this object.
        ' * 
        ' * @return The cos object that matches this Java object.
        ' */
        Public Overridable Function getStream() As COSStream
            Return stream
        End Function

        '/**
        ' * This will get the length of the filtered/compressed stream. This is
        ' * readonly in the PD Model and will be managed by this class.
        ' * 
        ' * @return The length of the filtered stream.
        ' */
        Public Overridable Function getLength() As Integer
            Return stream.getInt(COSName.LENGTH, 0)
        End Function

        '/**
        ' * This will get the list of filters that are associated with this stream.
        ' * Or null if there are none.
        ' * 
        ' * @return A list of all encoding filters to apply to this stream.
        ' */
        Public Overridable Function getFilters() As List(Of COSName)
            Dim retval As List(Of COSName) = Nothing
            Dim filters As COSBase = stream.getFilters()
            If (TypeOf (filters) Is COSName) Then
                Dim name As COSName = filters
                retval = New COSArrayList(Of COSName)(name, name, stream, COSName.FILTER)
            ElseIf (TypeOf (filters) Is COSArray) Then
                retval = DirectCast(filters, COSArray).toList()
            End If
            Return retval
        End Function

        '/**
        ' * This will set the filters that are part of this stream.
        ' * 
        ' * @param filters
        ' *            The filters that are part of this stream.
        ' */
        Public Overridable Sub setFilters(ByVal filters As List(Of COSName))
            Dim obj As COSBase = COSArrayList.converterToCOSArray(filters)
            stream.setItem(COSName.FILTER, obj)
        End Sub

        '/**
        ' * Get the list of decode parameters. Each entry in the list will refer to
        ' * an entry in the filters list.
        ' * 
        ' * @return The list of decode parameters.
        ' * 
        ' * @throws IOException
        ' *             if there is an error retrieving the parameters.
        ' */
        Public Function getDecodeParms() As List(Of Object) ' throws IOException
            Dim retval As List(Of Object) = Nothing

            Dim dp As COSBase = stream.getDictionaryObject(COSName.DECODE_PARMS)
            If (dp Is Nothing) Then
                ' See PDF Ref 1.5 implementation note 7, the DP is sometimes used
                ' instead.
                dp = stream.getDictionaryObject(COSName.DP)
            End If

            If (TypeOf (dp) Is COSDictionary) Then
                Dim map As Map = COSDictionaryMap.convertBasicTypesToMap(DirectCast(dp, COSDictionary))
                retval = New COSArrayList(Of Object)(map, dp, stream, COSName.DECODE_PARMS)
            ElseIf (TypeOf (dp) Is COSArray) Then
                Dim array As COSArray = dp
                Dim actuals As List(Of Object) = New ArrayList(Of Object)
                For i As Integer = 0 To array.size() - 1
                    actuals.add(COSDictionaryMap.convertBasicTypesToMap(DirectCast(array.getObject(i), COSDictionary)))
                Next
                retval = New COSArrayList(Of Object)(actuals, array)
            End If

            Return retval
        End Function

        '/**
        ' * This will set the list of decode parameterss.
        ' * 
        ' * @param decodeParams
        ' *            The list of decode parameterss.
        ' */
        Public Sub setDecodeParms(ByVal decodeParams As List)
            stream.setItem(COSName.DECODE_PARMS, COSArrayList.converterToCOSArray(decodeParams))
        End Sub

        '/**
        ' * This will get the file specification for this stream. This is only
        ' * required for external files.
        ' * 
        ' * @return The file specification.
        ' * 
        ' * @throws IOException
        ' *             If there is an error creating the file spec.
        ' */
        Public Overridable Function getFile() As PDFileSpecification 'throws IOException
            Dim f As COSBase = stream.getDictionaryObject(COSName.F)
            Dim retval As PDFileSpecification = PDFileSpecification.createFS(f)
            Return retval
        End Function

        '/**
        ' * Set the file specification.
        ' * 
        ' * @param f
        ' *            The file specification.
        ' */
        Public Overridable Sub setFile(ByVal f As PDFileSpecification)
            stream.setItem(COSName.F, f)
        End Sub

        '/**
        ' * This will get the list of filters that are associated with this stream.
        ' * Or null if there are none.
        ' * 
        ' * @return A list of all encoding filters to apply to this stream.
        ' */
        Public Overridable Function getFileFilters() As List(Of String)
            Dim retval As List(Of String) = Nothing
            Dim filters As COSBase = stream.getDictionaryObject(COSName.F_FILTER)
            If (TypeOf (filters) Is COSName) Then
                Dim name As COSName = filters
                retval = New COSArrayList(Of String)(name.getName(), name, stream, COSName.F_FILTER)
            ElseIf (TypeOf (filters) Is COSArray) Then
                retval = COSArrayList.convertCOSNameCOSArrayToList(DirectCast(filters, COSArray))
            End If
            Return retval
        End Function

        '/**
        ' * This will set the filters that are part of this stream.
        ' * 
        ' * @param filters
        ' *            The filters that are part of this stream.
        ' */
        Public Overridable Sub setFileFilters(ByVal filters As List(Of String))
            Dim obj As COSBase = COSArrayList.convertStringListToCOSNameCOSArray(filters)
            stream.setItem(COSName.F_FILTER, obj)
        End Sub

        '/**
        ' * Get the list of decode parameters. Each entry in the list will refer to
        ' * an entry in the filters list.
        ' * 
        ' * @return The list of decode parameters.
        ' * 
        ' * @throws IOException
        ' *             if there is an error retrieving the parameters.
        ' */
        Public Overridable Function getFileDecodeParams() As List ' throws IOException
            Dim retval As List(Of Object) = Nothing

            Dim dp As COSBase = stream.getDictionaryObject(COSName.F_DECODE_PARMS)
            If (TypeOf (dp) Is COSDictionary) Then
                Dim map As Map = COSDictionaryMap.convertBasicTypesToMap(DirectCast(dp, COSDictionary))
                retval = New COSArrayList(Of Object)(map, dp, stream, COSName.F_DECODE_PARMS)
            ElseIf (TypeOf (dp) Is COSArray) Then
                Dim array As COSArray = dp
                Dim actuals As List(Of Object) = New ArrayList(Of Object)
                For i As Integer = 0 To array.size() - 1
                    actuals.add(COSDictionaryMap.convertBasicTypesToMap(DirectCast(array.getObject(i), COSDictionary)))
                Next
                retval = New COSArrayList(Of Object)(actuals, array)
            End If

            Return retval
        End Function

        '/**
        ' * This will set the list of decode params.
        ' * 
        ' * @param decodeParams
        ' *            The list of decode params.
        ' */
        Public Overridable Sub setFileDecodeParams(ByVal decodeParams As List) 'List<?>
            stream.setItem("FDecodeParams", COSArrayList.converterToCOSArray(decodeParams))
        End Sub

        '/**
        ' * This will copy the stream into a byte array.
        ' * 
        ' * @return The byte array of the filteredStream
        ' * @throws IOException
        ' *             When getFilteredStream did not work
        ' */
        Public Overridable Function getByteArray() As Byte() ' throws IOException
            Dim output As ByteArrayOutputStream = New ByteArrayOutputStream()
            Dim buf() As Byte = Array.CreateInstance(GetType(Byte), 1024)
            Dim [is] As InputStream = Nothing
            Try
                [is] = createInputStream()
                Dim amountRead As Integer = -1
                amountRead = [is].read(buf)
                While (amountRead > 0)
                    output.Write(buf, 0, amountRead)
                    amountRead = [is].read(buf)
                End While
            Finally
                If ([is] IsNot Nothing) Then
                    [is].Close()
                End If
            End Try
            Return output.toByteArray()
        End Function

        '/**
        ' * A convenience method to get this stream as a string. Uses the default
        ' * system encoding.
        ' * 
        ' * @return a String representation of this (input) stream.
        ' * 
        ' * @throws IOException
        ' *             if there is an error while converting the stream to a string.
        ' */
        Public Function getInputStreamAsString() As String  'throws IOException
            Dim bStream() As Byte = getByteArray()
            Return Sistema.Strings.GetString(bStream, "ISO-8859-1")
        End Function

        '/**
        ' * Get the metadata that is part of the document catalog. This will return
        ' * null if there is no meta data for this object.
        ' * 
        ' * @return The metadata for this object.
        ' * @throws IllegalStateException
        ' *             if the value of the metadata entry is different from a stream
        ' *             or null
        ' */
        Public Overridable Function getMetadata() As PDMetadata
            Dim retval As PDMetadata = Nothing
            Dim mdStream As COSBase = stream.getDictionaryObject(COSName.METADATA)
            If (mdStream IsNot Nothing) Then
                If (TypeOf (mdStream) Is COSStream) Then
                    retval = New PDMetadata(mdStream)
                ElseIf (TypeOf (mdStream) Is COSNull) Then
                    ' null is authorized
                Else
                    Throw New Exception("Expected a COSStream but was a " & mdStream.GetType().Name())
                End If
            End If
            Return retval
        End Function

        '/**
        ' * Set the metadata for this object. This can be null.
        ' * 
        ' * @param meta
        ' *            The meta data for this object.
        ' */
        Public Overridable Sub setMetadata(ByVal meta As PDMetadata)
            stream.setItem(COSName.METADATA, meta)
        End Sub

        '/**
        ' * Get the decoded stream length.
        ' * 
        ' * @since Apache PDFBox 1.1.0
        ' * @see <a
        ' *      href="https://issues.apache.org/jira/browse/PDFBOX-636">PDFBOX-636</a>
        ' * @return the decoded stream length
        ' */
        Public Function getDecodedStreamLength() As Integer
            Return Me.stream.getInt(COSName.DL)
        End Function

        '/**
        ' * Set the decoded stream length.
        ' * 
        ' * @since Apache PDFBox 1.1.0
        ' * @see <a
        ' *      href="https://issues.apache.org/jira/browse/PDFBOX-636">PDFBOX-636</a>
        ' * @param decodedStreamLength
        ' *            the decoded stream length
        ' */
        Public Sub setDecodedStreamLength(ByVal decodedStreamLength As Integer)
            Me.stream.setInt(COSName.DL, decodedStreamLength)
        End Sub

    End Class

End Namespace
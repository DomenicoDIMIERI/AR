Imports FinSeA.Drawings
Imports System.IO
Imports FinSeA.Io

Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.io
Imports FinSeA.org.apache.pdfbox.pdmodel
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.color

Namespace org.apache.pdfbox.pdmodel.graphics.xobject

    '/**
    ' * An image class for CCITT Fax.
    ' * 
    ' * @author <a href="ben@benlitchfield.com">Ben Litchfield</a>
    ' * @author paul king
    ' * 
    ' */
    Public Class PDCcitt
        Inherits PDXObjectImage

        Private Shared FAX_FILTERS As List(Of String) = New ArrayList(Of String)()

        Shared Sub New()
            FAX_FILTERS.add(COSName.CCITTFAX_DECODE.getName())
            FAX_FILTERS.add(COSName.CCITTFAX_DECODE_ABBREVIATION.getName())
        End Sub

        '/**
        ' * Standard constructor.
        ' * 
        ' * @param ccitt The PDStream that already contains all ccitt information.
        ' */
        Public Sub New(ByVal ccitt As PDStream)
            MyBase.New(ccitt, "tiff")
        End Sub

        '/**
        ' * Construct from a tiff file.
        ' * 
        ' * @param doc The document to create the image as part of.
        ' * @param raf The random access TIFF file which contains a suitable CCITT compressed image
        ' * @throws IOException If there is an error reading the tiff data.
        ' */
        Public Sub New(ByVal doc As PDDocument, ByVal raf As RandomAccess) 'throws IOException
            MyBase.New(New PDStream(doc), "tiff")

            Dim decodeParms As COSDictionary = New COSDictionary()

            Dim dic As COSDictionary = getCOSStream()

            extractFromTiff(raf, getCOSStream().createFilteredStream(), decodeParms)

            dic.setItem(COSName.FILTER, COSName.CCITTFAX_DECODE)
            dic.setItem(COSName.SUBTYPE, COSName.IMAGE)
            dic.setItem(COSName.TYPE, COSName.XOBJECT)
            dic.setItem(COSName.DECODE_PARMS, decodeParms)

            setBitsPerComponent(1)
            setColorSpace(New PDDeviceGray())
            setWidth(decodeParms.getInt(COSName.COLUMNS))
            setHeight(decodeParms.getInt(COSName.ROWS))

        End Sub

        '/**
        ' * Returns an image of the CCITT Fax, or null if TIFFs are not supported. (Requires additional JAI Image filters )
        ' * 
        ' * {@inheritDoc}
        ' */
        Public Overrides Function getRGBImage() As BufferedImage ' throws IOException
            Dim stream As COSStream = getCOSStream()
            Dim decodeP As COSBase = stream.getDictionaryObject(COSName.DECODE_PARMS)
            Dim decodeParms As COSDictionary = Nothing
            If (TypeOf (decodeP) Is COSDictionary) Then
                decodeParms = decodeP
            ElseIf (TypeOf (decodeP) Is COSArray) Then
                Dim index As Integer = 0
                ' determine the index for the CCITT-filter
                Dim filters As COSBase = stream.getFilters()
                If (TypeOf (filters) Is COSArray) Then
                    Dim filterArray As COSArray = filters
                    While (index < filterArray.size())
                        Dim filtername As COSName = filterArray.get(index)
                        If (COSName.CCITTFAX_DECODE.equals(filtername)) Then
                            Exit While
                        End If
                        index += 1
                    End While
                End If
                decodeParms = DirectCast(decodeP, COSArray).getObject(index)
            End If
            Dim cols As Integer = decodeParms.getInt(COSName.COLUMNS, 1728)
            Dim rows As Integer = decodeParms.getInt(COSName.ROWS, 0)
            Dim height As Integer = stream.getInt(COSName.HEIGHT, 0)
            If (rows > 0 AndAlso height > 0) Then
                ' ensure that rows doesn't contain implausible data, see PDFBOX-771
                rows = Math.Min(rows, height)
            Else
                ' at least one of the values has to have a valid value
                rows = Math.Max(rows, height)
            End If
            Dim blackIsOne As Boolean = decodeParms.getBoolean(COSName.BLACK_IS_1, False)
            ' maybe a decode array is defined
            Dim decode As COSArray = getDecode()
            If (decode IsNot Nothing AndAlso decode.getInt(0) <> 1) Then
                ' [1.0, 0.0] -> invert the "color" values
                blackIsOne = Not blackIsOne
            End If
            Dim bufferData() As Byte = Nothing
            Dim colorModel As ColorModel = Nothing
            Dim colorspace As PDColorSpace = getColorSpace()
            ' most likely there is no colorspace as a CCITT-filter uses 1-bit values mapped to black/white
            ' in some rare cases other colorspaces maybe used such as an indexed colorspace, see PDFBOX-1638
            If (TypeOf (colorspace) Is PDIndexed) Then
                Dim csIndexed As PDIndexed = colorspace
                Dim maskArray As COSBase = getMask()
                If (maskArray IsNot Nothing AndAlso TypeOf (maskArray) Is COSArray) Then
                    colorModel = csIndexed.createColorModel(1, DirectCast(maskArray, COSArray).getInt(0))
                Else
                    colorModel = csIndexed.createColorModel(1)
                End If
            Else
                Dim map() As Byte = {&H0, &HFF}
                colorModel = New IndexColorModel(1, map.Length, map, map, map, Transparency.Mode.OPAQUE)
            End If
            Dim raster As WritableRaster = colorModel.createCompatibleWritableRaster(cols, rows)
            Dim buffer As DataBufferByte = raster.getDataBuffer()
            bufferData = buffer.getData()
            IOUtils.populateBuffer(stream.getUnfilteredStream(), bufferData)
            Dim image As BufferedImage = New BufferedImage(colorModel, raster, False, Nothing)
            If (Not blackIsOne) Then
                ' Inverting the bitmap
                ' Note the previous approach with starting from an IndexColorModel didn't work
                ' reliably. In some cases the image wouldn't be painted for some reason.
                ' So a safe but slower approach was taken.
                invertBitmap(bufferData)
            End If

            '/*
            ' * If we have an image mask we need to add an alpha channel to the data
            ' */
            If (hasMask()) Then
                Dim map() As Byte = {&H0, &HFF}
                Dim cm As IndexColorModel = New IndexColorModel(1, map.Length, map, map, map, Transparency.Mode.OPAQUE)
                raster = cm.createCompatibleWritableRaster(cols, rows)
                bufferData = DirectCast(raster.getDataBuffer(), DataBufferByte).getData()

                Dim array As Byte() = DirectCast(image.getData().getDataBuffer(), DataBufferByte).getData()
                System.Array.Copy(array, 0, bufferData, 0, Math.Min(array.Length, bufferData.Length))
                Dim indexed As BufferedImage = New BufferedImage(cm, raster, False, Nothing)
                image = indexed
            End If
            Return applyMasks(image)
        End Function

        Private Sub invertBitmap(ByVal bufferData() As Byte)
            For i As Integer = 0 To bufferData.Length - 1
                bufferData(i) = Not bufferData(i) And &HFF
            Next
        End Sub

        '/**
        ' * This writes a tiff to out.
        ' * 
        ' * {@inheritDoc}
        ' */
        Public Overrides Sub write2OutputStream(ByVal out As OutputStream) 'throws IOException
            ' We should use another format than TIFF to get rid of the TiffWrapper
            Dim data As InputStream = New TiffWrapper(Me, getPDStream().getPartiallyFilteredStream(FAX_FILTERS), getCOSStream())
            IOUtils.copy(data, out)
        End Sub

        '/**
        ' * Extract the ccitt stream from the tiff file.
        ' * 
        ' * @param raf - TIFF File
        ' * @param os - Stream to write raw ccitt data two
        ' * @param parms - COSDictionary which the encoding parameters are added to
        ' * @throws IOException If there is an error reading/writing to/from the stream
        ' */
        Private Sub extractFromTiff(ByVal raf As RandomAccess, ByVal os As OutputStream, ByVal parms As COSDictionary) 'throws IOException
            Try
                ' First check the basic tiff header
                raf.seek(0)
                Dim endianess As Char = Convert.ToChar(raf.read)
                If (Convert.ToChar(raf.read) <> endianess) Then
                    Throw New IOException("Not a valid tiff file")
                End If
                ' ensure that endianess is either M or I
                If (endianess <> "M"c AndAlso endianess <> "I"c) Then
                    Throw New IOException("Not a valid tiff file")
                End If
                Dim magicNumber As Integer = readshort(endianess, raf)
                If (magicNumber <> 42) Then
                    Throw New IOException("Not a valid tiff file")
                End If

                ' Relocate to the first set of tags
                raf.seek(readlong(endianess, raf))

                Dim numtags As Integer = readshort(endianess, raf)

                ' The number 50 is somewhat arbitary, it just stops us load up junk from somewhere and tramping on
                If (numtags > 50) Then
                    Throw New IOException("Not a valid tiff file")
                End If

                '// Loop through the tags, some will convert to items in the parms dictionary
                '// Other point us to where to find the data stream
                '// The only parm which might change as a result of other options is K, so
                '// We'll deal with that as a special;

                Dim k As Integer = -1000 ' Default Non CCITT compression
                Dim dataoffset As Integer = 0
                Dim datalength As Integer = 0

                For i As Integer = 0 To numtags - 1
                    Dim tag As Integer = readshort(endianess, raf)
                    Dim type As Integer = readshort(endianess, raf)
                    Dim count As Integer = readlong(endianess, raf)
                    Dim val As Integer = readlong(endianess, raf) ' See note

                    ' Note, we treated that value as a long. The value always occupies 4 bytes
                    ' But it might only use the first byte or two. Depending on endianess we might need to correct
                    ' Note we ignore all other types, they are of little interest for PDFs/CCITT Fax
                    If (endianess = "M"c) Then
                        Select Case (type)
                            Case 1 : val = val >> 24 ' byte value
                            Case 3 : val = val >> 16 ' short value
                            Case 4 ' long value
                            Case Else
                                ' do nothing
                        End Select
                    End If
                    Select Case (tag)
                        Case 256 : parms.setInt(COSName.COLUMNS, val)
                        Case 257 : parms.setInt(COSName.ROWS, val)
                        Case 259
                            If (val = 4) Then
                                k = -1
                            End If
                            If (val = 3) Then
                                k = 0
                            End If
                            ' T6/T4 Compression
                        Case 262
                            If (val = 1) Then
                                parms.setBoolean(COSName.BLACK_IS_1, True)
                            End If
                        Case 273
                            If (count = 1) Then
                                dataoffset = val
                            End If
                        Case 279
                            If (count = 1) Then
                                datalength = val
                            End If
                        Case 292
                            If (val = 1) Then
                                k = 50 ' T4 2D - arbitary K value
                            End If
                        Case 324
                            If (count = 1) Then
                                dataoffset = val
                            End If
                        Case 325
                            If (count = 1) Then
                                datalength = val
                            End If
                        Case Else
                            ' do nothing
                    End Select
                Next

                If (k = -1000) Then
                    Throw New IOException("First image in tiff is not CCITT T4 or T6 compressed")
                End If
                If (dataoffset = 0) Then
                    Throw New IOException("First image in tiff is not a single tile/strip")
                End If

                parms.setInt(COSName.K, k)

                raf.seek(dataoffset)

                Dim buf(8192 - 1) As Byte '= new byte[8192];
                Dim amountRead As Integer
                amountRead = raf.read(buf, 0, Math.Min(8192, datalength))
                While (amountRead > 0)
                    datalength -= amountRead
                    os.Write(buf, 0, amountRead)
                    amountRead = raf.read(buf, 0, Math.Min(8192, datalength))
                End While

            Finally
                os.Close()
            End Try
        End Sub

        Private Function readshort(ByVal endianess As Char, ByVal raf As RandomAccess) As Integer
            If (endianess <> "I"c) Then
                Return raf.read() Or (raf.read() << 8)
            End If
            Return (raf.read() << 8) Or raf.read()
        End Function

        Private Function readlong(ByVal endianess As Char, ByVal raf As RandomAccess) As Integer
            If (endianess = "I"c) Then
                Return raf.read() Or (raf.read() << 8) Or (raf.read() << 16) Or (raf.read() << 24)
            End If
            Return (raf.read() << 24) Or (raf.read() << 16) Or (raf.read() << 8) Or raf.read()
        End Function

        '/**
        ' * Extends InputStream to wrap the data from the CCITT Fax with a suitable TIFF Header. For details see
        ' * www.tiff.org, which contains useful information including pointers to the TIFF 6.0 Specification
        ' * 
        ' */
        Private Class TiffWrapper
            Inherits InputStream

            Private currentOffset As Integer ' When reading, where in the tiffheader are we.
            Private tiffheader() As Byte  ' Byte array to store tiff header data
            Private datastream As InputStream  ' Original InputStream
            Private m_Caller As PDCcitt

            Public Sub New(ByVal caller As PDCcitt, ByVal rawstream As InputStream, ByVal options As COSDictionary)
                Me.m_Caller = caller
                buildHeader(options)
                currentOffset = 0
                datastream = rawstream
            End Sub

            '// Implement basic methods from InputStream
            '/**
            ' * {@inheritDoc}
            ' */
            Public Overrides Function markSupported() As Boolean
                Return False
            End Function


            Public Overrides Sub reset()
                Throw New NotSupportedException("reset not supported")
            End Sub

            '/**
            ' * For simple read, take a byte from the tiffheader array or pass through.
            ' * 
            ' * {@inheritDoc}
            ' */
            Public Overrides Function read() As Integer
                If (currentOffset < tiffheader.Length) Then
                    Return tiffheader(currentOffset) : currentOffset += 1
                End If
                Return datastream.read()
            End Function

            '/**
            ' * For read methods only return as many bytes as we have left in the header if we've exhausted the header, pass
            ' * through to the InputStream of the raw CCITT data.
            ' * 
            ' * {@inheritDoc}
            ' */
            Public Overrides Function read(ByVal data() As Byte) As Integer
                If (currentOffset < tiffheader.Length) Then
                    Dim length As Integer = Math.Min(tiffheader.Length - currentOffset, data.Length)
                    If (Length > 0) Then
                        Array.Copy(tiffheader, currentOffset, data, 0, length)
                    End If
                    currentOffset += length
                    Return length
                Else
                    Return datastream.read(data)
                End If
            End Function

            '/**
            ' * For read methods only return as many bytes as we have left in the header if we've exhausted the header, pass
            ' * through to the InputStream of the raw CCITT data.
            ' * 
            ' * {@inheritDoc}
            ' */
            Public Overrides Function read(ByVal data() As Byte, ByVal off As Integer, ByVal len As Integer) As Integer
                If (currentOffset < tiffheader.Length) Then
                    Dim length As Integer = Math.Min(tiffheader.Length - currentOffset, len)
                    If (length > 0) Then
                        Array.Copy(tiffheader, currentOffset, data, off, length)
                    End If
                    currentOffset += length
                    Return length
                Else
                    Return datastream.read(data, off, len)
                End If
            End Function

            '/**
            ' * When skipping if any header data not yet read, only allow to skip what we've in the buffer Otherwise just
            ' * pass through.
            ' * 
            ' * {@inheritDoc}
            ' */
            Public Overrides Function skip(ByVal n As Long) As Long
                If (currentOffset < tiffheader.Length) Then
                    Dim length As Long = Math.Min(tiffheader.Length - currentOffset, n)
                    currentOffset += length
                    Return length
                Else
                    Return datastream.skip(n)
                End If
            End Function


            ''' <summary>
            ''' Static data for the beginning of the TIFF header
            ''' </summary>
            ''' <remarks></remarks>
            Private Shared ReadOnly basicHeader As Byte() = {Asc("I"), Asc("I"), 42, 0, 8, 0, 0, 0, 0, 0} ' // // File introducer and pointer to first IFD - Number of tags start with two
            Private additionalOffset As Integer  'Offset in header to additional data

            ''' <summary>
            ''' Builds up the tiffheader based on the options passed through.
            ''' </summary>
            ''' <remarks></remarks>
            Private Sub buildHeader(ByVal options As COSDictionary)
                Const numOfTags As Integer = 10 ' The maximum tags we'll fill
                Const maxAdditionalData As Integer = 24 ' The maximum amount of additional data
                '                                         outside the IFDs. (bytes)

                ' The length of the header will be the length of the basic header (10)
                ' plus 12 bytes for each IFD, 4 bytes as a pointer to the next IFD (will be 0)
                ' plus the length of the additional data

                Dim ifdSize As Integer = 10 + (12 * numOfTags) + 4
                tiffheader = Array.CreateInstance(GetType(Byte), ifdSize + maxAdditionalData)
                '(tiffheader, (byte) 0);
                Array.Copy(basicHeader, 0, tiffheader, 0, basicHeader.Length)

                ' Additional data outside the IFD starts after the IFD's and pointer to the next IFD (0)
                additionalOffset = ifdSize

                ' Now work out the variable values from TIFF defaults,
                ' PDF Defaults and the Dictionary for this XObject
                Dim cols As Short = 1728
                Dim rows As Short = 0
                Dim blackis1 As Short = 0
                Dim comptype As Short = 3 ' T4 compression
                Dim t4options As Long = 0 ' Will set if 1d or 2d T4

                Dim decode As COSArray = Me.m_Caller.getDecode()
                ' we have to invert the b/w-values,
                ' if the Decode array exists and consists of (1,0)
                If (decode IsNot Nothing AndAlso decode.getInt(0) = 1) Then
                    blackis1 = 1
                End If
                Dim dicOrArrayParms As COSBase = options.getDictionaryObject(COSName.DECODE_PARMS)
                Dim decodeParms As COSDictionary = Nothing
                If (TypeOf (dicOrArrayParms) Is COSDictionary) Then
                    decodeParms = dicOrArrayParms
                Else
                    Dim parmsArray As COSArray = dicOrArrayParms
                    If (parmsArray.size() = 1) Then
                        decodeParms = parmsArray.getObject(0)
                    Else
                        ' else find the first dictionary with Row/Column info and use that.
                        For i As Integer = 0 To parmsArray.size() - 1
                            If (decodeParms IsNot Nothing) Then Exit For
                            Dim dic As COSDictionary = parmsArray.getObject(i)
                            If (dic IsNot Nothing AndAlso (dic.getDictionaryObject(COSName.COLUMNS) IsNot Nothing OrElse dic.getDictionaryObject(COSName.ROWS) IsNot Nothing)) Then
                                decodeParms = dic
                            End If
                        Next
                    End If
                End If

                If (decodeParms IsNot Nothing) Then
                    cols = decodeParms.getInt(COSName.COLUMNS, cols)
                    rows = decodeParms.getInt(COSName.ROWS, rows)
                    If (decodeParms.getBoolean(COSName.BLACK_IS_1, False)) Then
                        blackis1 = 1
                    End If
                    Dim k As Integer = decodeParms.getInt(COSName.K, 0) ' Mandatory parm
                    If (k < 0) Then
                        ' T6
                        comptype = 4
                    End If
                    If (k > 0) Then
                        ' T4 2D
                        comptype = 3
                        t4options = 1
                    End If
                    ' else k = 0, leave as default T4 1D compression
                End If

                ' If we couldn't get the number of rows, use the main item from XObject
                If (rows = 0) Then
                    rows = options.getInt(COSName.HEIGHT, rows)
                End If

                ' Now put the tags into the tiffheader
                ' These musn't exceed the maximum set above, and by TIFF spec should be sorted into
                ' Numeric sequence.

                addTag(256, cols) ' Columns
                addTag(257, rows) ' Rows
                addTag(259, comptype) ' T6
                addTag(262, blackis1) ' Photometric Interpretation
                addTag(273, tiffheader.Length) ' Offset to start of image data - updated below
                addTag(279, options.getInt(COSName.LENGTH)) ' Length of image data
                addTag(282, 300, 1) ' X Resolution 300 (default unit Inches) This is arbitary
                addTag(283, 300, 1) ' Y Resolution 300 (default unit Inches) This is arbitary
                If (comptype = 3) Then
                    addTag(292, t4options)
                End If
                addTag(305, "PDFBOX") ' Software generating image
            End Sub

            '/* Tiff types 1 = byte, 2=ascii, 3=short, 4=ulong 5=rational */

            Private Sub addTag(ByVal tag As Integer, ByVal value As Long)
                ' Adds a tag of type 4 (ulong)
                tiffheader(8) += 1
                Dim count As Integer = tiffheader(8)
                Dim offset As Integer = (count - 1) * 12 + 10
                tiffheader(offset) = (tag And &HFF)
                tiffheader(offset + 1) = ((tag >> 8) And &HFF)
                tiffheader(offset + 2) = 4 ' Type Long
                tiffheader(offset + 4) = 1 ' One Value
                tiffheader(offset + 8) = (value And &HFF)
                tiffheader(offset + 9) = ((value >> 8) And &HFF)
                tiffheader(offset + 10) = ((value >> 16) And &HFF)
                tiffheader(offset + 11) = ((value >> 24) And &HFF)
            End Sub

            Private Sub addTag(ByVal tag As Integer, ByVal value As Short)
                ' Adds a tag of type 3 (short)
                tiffheader(8) += 1
                Dim count As Integer = tiffheader(8)
                Dim offset As Integer = (count - 1) * 12 + 10
                tiffheader(offset) = (tag And &HFF)
                tiffheader(offset + 1) = ((tag >> 8) And &HFF)
                tiffheader(offset + 2) = 3 ' Type Integer
                tiffheader(offset + 4) = 1 ' One Value
                tiffheader(offset + 8) = (value And &HFF)
                tiffheader(offset + 9) = ((value >> 8) And &HFF)
            End Sub

            Private Sub addTag(ByVal tag As Integer, ByVal value As String)
                ' Adds a tag of type 2 (ascii)
                tiffheader(8) += 1
                Dim count As Integer = tiffheader(8)
                Dim offset As Integer = (count - 1) * 12 + 10
                tiffheader(offset) = (tag And &HFF)
                tiffheader(offset + 1) = ((tag >> 8) And &HFF)
                tiffheader(offset + 2) = 2 ' Type Ascii
                Dim len As Integer = value.Length() + 1
                tiffheader(offset + 4) = (len And &HFF)
                tiffheader(offset + 8) = (additionalOffset And &HFF)
                tiffheader(offset + 9) = ((additionalOffset >> 8) And &HFF)
                tiffheader(offset + 10) = ((additionalOffset >> 16) And &HFF)
                tiffheader(offset + 11) = ((additionalOffset >> 24) And &HFF)
                Try
                    Array.Copy(Sistema.Strings.GetBytes(value, "US-ASCII"), 0, tiffheader, additionalOffset, value.Length())
                Catch e As NotSupportedException
                    Throw New RuntimeException("Incompatible VM without US-ASCII encoding", e)
                End Try
                additionalOffset += len
            End Sub

            Private Sub addTag(ByVal tag As Integer, ByVal numerator As Long, ByVal denominator As Long)
                ' Adds a tag of type 5 (rational)
                tiffheader(8) += 1
                Dim count As Integer = tiffheader(8)
                Dim offset As Integer = (count - 1) * 12 + 10
                tiffheader(offset) = (tag And &HFF)
                tiffheader(offset + 1) = ((tag >> 8) And &HFF)
                tiffheader(offset + 2) = 5 ' Type Rational
                tiffheader(offset + 4) = 1 ' One Value
                tiffheader(offset + 8) = (additionalOffset And &HFF)
                tiffheader(offset + 9) = ((additionalOffset >> 8) And &HFF)
                tiffheader(offset + 10) = ((additionalOffset >> 16) And &HFF)
                tiffheader(offset + 11) = ((additionalOffset >> 24) And &HFF)
                tiffheader(additionalOffset) = ((numerator) And &HFF) : additionalOffset += 1
                tiffheader(additionalOffset) = ((numerator >> 8) And &HFF) : additionalOffset += 1
                tiffheader(additionalOffset) = ((numerator >> 16) And &HFF) : additionalOffset += 1
                tiffheader(additionalOffset) = ((numerator >> 24) And &HFF) : additionalOffset += 1
                tiffheader(additionalOffset) = ((denominator) And &HFF) : additionalOffset += 1
                tiffheader(additionalOffset) = ((denominator >> 8) And &HFF) : additionalOffset += 1
                tiffheader(additionalOffset) = ((denominator >> 16) And &HFF) : additionalOffset += 1
                tiffheader(additionalOffset) = ((denominator >> 24) And &HFF) : additionalOffset += 1
            End Sub

        End Class

    End Class

End Namespace


Imports FinSeA.Io
Imports System.IO
Imports System.Text
Imports FinSeA.Text
Imports FinSeA.Exceptions
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.exceptions
Imports FinSeA.org.apache.pdfbox.io
Imports FinSeA.org.apache.pdfbox.persistence.util

Namespace org.apache.pdfbox.pdfparser

    '/**
    ' * This class is used to contain parsing logic that will be used by both the
    ' * PDFParser and the COSStreamParser.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * 
    ' */
    Public MustInherit Class BaseParser

        Private Const OBJECT_NUMBER_THRESHOLD As Long = 10000000000L

        Private Const GENERATION_NUMBER_THRESHOLD As Integer = 65535

        ' ''' <summary>
        ' ''' system property allowing to define size of push back buffer.
        ' ''' </summary>
        ' ''' <remarks></remarks>
        'Public Const PROP_PUSHBACK_SIZE As String = "org.apache.pdfbox.baseParser.pushBackSize"

        '/**
        ' * Log instance.
        ' */
        'private static final Log LOG = LogFactory.getLog(BaseParser.class);

        Private Const E As Integer = AscW("e")
        Private Const N As Integer = AscW("n")
        Private Const D As Integer = AscW("d")

        Private Const S As Integer = AscW("s")
        Private Const T As Integer = AscW("t")
        Private Const R As Integer = AscW("r")
        Private Const A As Integer = AscW("a")
        Private Const M As Integer = AscW("m")

        Private Const O As Integer = AscW("o")
        Private Const B As Integer = AscW("b")
        Private Const J As Integer = AscW("j")

        Private strmBufLen As Integer = 2048
        Private strmBuf() As Byte = Array.CreateInstance(GetType(Byte), strmBufLen)

        ''' <summary>
        ''' This is a byte array that will be used for comparisons.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly ENDSTREAM As Byte() = {E, N, D, S, T, R, E, A, M}

        ''' <summary>
        ''' This is a byte array that will be used for comparisons.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly ENDOBJ() As Byte = {E, N, D, O, B, J}

        ''' <summary>
        ''' This is a string constant that will be used for comparisons.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const DEF As String = "def"

        ''' <summary>
        ''' This is a string constant that will be used for comparisons.
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ENDOBJ_STRING As String = "endobj"

        ''' <summary>
        ''' This is a string constant that will be used for comparisons.
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ENDSTREAM_STRING As String = "endstream"

        ''' <summary>
        ''' This is a string constant that will be used for comparisons.
        ''' </summary>
        ''' <remarks></remarks>
        Private Const STREAM_STRING As String = "stream"

        ''' <summary>
        ''' This is a string constant that will be used for comparisons.
        ''' </summary>
        ''' <remarks></remarks>
        Private Const [TRUE] As String = "true"

        ''' <summary>
        ''' * This is a string constant that will be used for comparisons.
        ''' </summary>
        ''' <remarks></remarks>
        Private Const [FALSE] As String = "false"

        ''' <summary>
        ''' This is a string constant that will be used for comparisons.
        ''' </summary>
        ''' <remarks></remarks>
        Private Const NULL As String = "null"

        ''' <summary>
        ''' Default value of the {@link #forceParsing} flag.
        ''' </summary>
        ''' <remarks></remarks>
        Protected Const FORCE_PARSING As Boolean = False 'Boolean.getBoolean("org.apache.pdfbox.forceParsing");

        ''' <summary>
        ''' This is the stream that will be read from.
        ''' </summary>
        ''' <remarks></remarks>
        Protected pdfSource As pdfbox.io.PushBackInputStream ' PushBackInputStream

        ''' <summary>
        ''' This is the document that will be parsed.
        ''' </summary>
        ''' <remarks></remarks>
        Protected document As COSDocument

        ''' <summary>
        ''' Flag to skip malformed or otherwise unparseable input where possible.
        ''' </summary>
        ''' <remarks></remarks>
        Protected forceParsing As Boolean

        ''' <summary>
        ''' Default constructor.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            Me.forceParsing = FORCE_PARSING
        End Sub

   
        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <param name="input">The input stream to read the data from.</param>
        ''' <param name="forceParsingValue">flag to skip malformed or otherwise unparseable</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal input As Stream, ByVal forceParsingValue As Boolean)
            Me.pdfSource = New pdfbox.io.PushBackInputStream(New BufferedInputStream(input, 16384), Integer.Parse(My.Settings.PROP_PUSHBACK_SIZE, 65536))
            Me.forceParsing = forceParsingValue
        End Sub

        ''' <summary>
        ''' Constructor.
        ''' </summary>
        ''' <param name="input">The input stream to read the data from.</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal input As Stream)
            Me.New(input, FORCE_PARSING)
        End Sub

        ''' <summary>
        ''' Constructor.
        ''' </summary>
        ''' <param name="input">input The array to read the data from.</param>
        ''' <remarks></remarks>
        Protected Sub New(ByVal input() As Byte) 'throws IOException 
            Me.New(New ByteArrayInputStream(input))
        End Sub

        ''' <summary>
        ''' Set the document for this stream.
        ''' </summary>
        ''' <param name="doc">The current document.</param>
        ''' <remarks></remarks>
        Public Sub setDocument(ByVal doc As COSDocument)
            Me.document = doc
        End Sub

        Private Shared Function isHexDigit(ByVal ch As Char) As Boolean
            Return (ch >= "0"c AndAlso ch <= "9"c) OrElse (ch >= "a"c AndAlso ch <= "f"c) OrElse (ch >= "A"c AndAlso ch <= "F"c)
            ' the line below can lead to problems with certain versions of the IBM JIT compiler
            ' (and is slower anyway)
            'return (HEXDIGITS.indexOf(ch) != -1)
        End Function

        ''' <summary>
        ''' This will parse a PDF dictionary value.
        ''' </summary>
        ''' <returns>The parsed Dictionary object.</returns>
        ''' <remarks></remarks>
        '''<exception cref="IOException">throws IOException If there is an error parsing the dictionary object</exception>
        Private Function parseCOSDictionaryValue() As COSBase ' throws IOException
            Dim retval As COSBase = Nothing
            Dim number As COSBase = parseDirObject()
            skipSpaces()
            Dim [next] As Char = Convert.ToChar(pdfSource.peek())
            If ([next] >= "0"c AndAlso [next] <= "9"c) Then
                Dim generationNumber As COSBase = parseDirObject()
                skipSpaces()
                Dim r As Char = Convert.ToChar(pdfSource.read())
                If (r <> "R"c) Then
                    Throw New IOException("expected='R' actual='" & r & "' " & pdfSource.ToString)
                End If
                Dim key As New COSObjectKey(CType(number, COSInteger).intValue(), CType(generationNumber, COSInteger).intValue())
                retval = document.getObjectFromPool(key)
            Else
                retval = number
            End If
            Return retval
        End Function

        '/**
        ' * 
        ' *
        ' * @.
        ' *
        ' * @throws IOException 
        ' */

        ''' <summary>
        ''' This will parse a PDF dictionary.
        ''' </summary>
        ''' <returns>return The parsed dictionary</returns>
        ''' <remarks></remarks>
        ''' <exception cref="IOException">IF there is an error reading the stream.</exception>
        Protected Function parseCOSDictionary() As COSDictionary ' throws IOException
            Dim c As Char = Convert.ToChar(pdfSource.read())
            If (c <> "<") Then
                Throw New IOException("expected='<' actual='" & c & "'")
            End If
            c = Convert.ToChar(pdfSource.read())
            If (c <> "<") Then
                Throw New IOException("expected='<' actual='" & c & "' " & pdfSource.ToString)
            End If
            skipSpaces()
            Dim obj As COSDictionary = New COSDictionary()
            Dim done As Boolean = False
            While (Not done)
                skipSpaces()
                c = Convert.ToChar(pdfSource.peek())
                If (c = ">") Then
                    done = True
                Else If (c <> "/") Then
                    'an invalid dictionary, we are expecting
                        'the key, read until we can recover
                        LOG.warn("Invalid dictionary, found: '" & c & "' but expected: '/'")
                        Dim read As Integer = pdfSource.read()
                        While (read <> -1 AndAlso read <> Asc("/") AndAlso read <> Asc(">"))
                            ' in addition to stopping when we find / or >, we also want
                            ' to stop when we find endstream or endobj.
                            If (read = E) Then
                                read = pdfSource.read()
                                If (read = N) Then
                                    read = pdfSource.read()
                                    If (read = D) Then
                                        read = pdfSource.read()
                                        If (read = S) Then
                                            read = pdfSource.read()
                                            If (read = T) Then
                                                read = pdfSource.read()
                                                If (read = R) Then
                                                    read = pdfSource.read()
                                                    If (read = E) Then
                                                        read = pdfSource.read()
                                                        If (read = A) Then
                                                            read = pdfSource.read()
                                                            If (read = M) Then
                                                                Return obj ' we're done reading this object!
                                                            End If
                                                        End If
                                                    End If
                                                End If
                                            End If
                                        ElseIf (read = O) Then
                                            read = pdfSource.read()
                                            If (read = B) Then
                                                read = pdfSource.read()
                                                If (read = J) Then
                                                    Return obj ' we're done reading this object!
                                                End If
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                            read = pdfSource.read()
                        End While
                        If (read <> -1) Then
                            pdfSource.unread(read)
                        Else
                            Return obj
                        End If
                Else
                    Dim key As COSName = parseCOSName()
                    Dim value As COSBase = parseCOSDictionaryValue()
                    skipSpaces()
                    If (Convert.ToChar(pdfSource.peek()) = "d") Then
                        'if the next string is 'def' then we are parsing a cmap stream
                        'and want to ignore it, otherwise throw an exception.
                        Dim potentialDEF As String = readString()
                        If (Not potentialDEF.Equals(DEF)) Then
                            pdfSource.unread(Sistema.Strings.GetBytes(potentialDEF, "ISO-8859-1"))
                        Else
                            skipSpaces()
                        End If
                    End If

                    If (value Is Nothing) Then
                        LOG.warn("Bad Dictionary Declaration " & pdfSource.ToString)
                    Else
                        value.setDirect(True)
                        obj.setItem(key, value)
                    End If
                End If
            End While
            Dim ch As Char = Convert.ToChar(pdfSource.read())
            If (ch <> ">") Then
                Throw New IOException("expected='>' actual='" & ch & "'")
            End If
            ch = Convert.ToChar(pdfSource.read())
            If (ch <> ">") Then
                Throw New IOException("expected='>' actual='" & ch & "'")
            End If
            Return obj
            'Dim c As Char = Convert.ToChar(pdfSource.read())
            'If (c <> "<"c) Then
            '    Throw New IOException("expected='<' actual='" & c & "'")
            'End If
            'c = Convert.ToChar(pdfSource.read())
            'If (c <> "<"c) Then
            '    Throw New IOException("expected='<' actual='" & c & "' " & pdfSource.ToString)
            'End If
            'skipSpaces()
            'Dim obj As New COSDictionary()
            'Dim done As Boolean = False
            'While (Not done)
            '    skipSpaces()
            '    c = Convert.ToChar(pdfSource.peek())
            '    If (c = ">"c) Then
            '        done = True
            '    Else
            '                 If (c <> "/"c) Then
            '                     'an invalid dictionary, we are expecting
            '                     'the key, read until we can recover
            '                     Debug.Print("Invalid dictionary, found: '" & c & "' but expected: '/'")
            '                     Dim read As Integer = pdfSource.read()
            '                     While (read <> -1 AndAlso read <> AscW("/"c) AndAlso read <> AscW(">"))
            '                         ' in addition to stopping when we find / or >, we also want
            '                         ' to stop when we find endstream or endobj.
            '                         If (read = E) Then
            '                             read = pdfSource.read()
            '                             If (read = N) Then
            '                                 read = pdfSource.read()
            '                                 If (read = D) Then
            '                                     read = pdfSource.read()
            '                                     If (read = S) Then
            '                                         read = pdfSource.read()
            '                                         If (read = T) Then
            '                                             read = pdfSource.read()
            '                                             If (read = R) Then
            '                                                 read = pdfSource.read()
            '                                                 If (read = E) Then
            '                                                     read = pdfSource.read()
            '                                                     If (read = A) Then
            '                                                         read = pdfSource.read()
            '                                                         If (read = M) Then
            '                                                             Return obj ' we're done reading this object!
            '                                                         End If
            '                                                     End If
            '                                                 End If
            '                                             End If
            '                                         End If
            '                                     ElseIf (read = O) Then
            '                                         read = pdfSource.read()
            '                                         If (read = B) Then
            '                                             read = pdfSource.read()
            '                                             If (read = J) Then
            '                                                 Return obj ' we're done reading this object!
            '                                             End If
            '                                         End If
            '                                     End If
            '                                 End If
            '                             End If
            '                         End If
            '                         read = pdfSource.read()
            '                     End While
            '                     If (read <> -1) Then
            '                         pdfSource.unread(read)
            '                     Else
            '                         Return obj
            '                     End If
            '                 Else
            '                     Dim key As COSName = parseCOSName()
            '                     Dim value As COSBase = parseCOSDictionaryValue()
            '                     skipSpaces()
            '                     If (Convert.ToChar(pdfSource.peek()) = "d"c) Then
            '                         'if the next string is 'def' then we are parsing a cmap stream
            '                         'and want to ignore it, otherwise throw an exception.
            '                         Dim potentialDEF As String = readString()
            '                         If (Not potentialDEF.Equals(DEF)) Then
            '                             pdfSource.unread(Strings.GetBytes(potentialDEF, "ISO-8859-1"))
            '                         Else
            '                             skipSpaces()
            '                         End If
            '                     End If

            '                     If (value Is Nothing) Then
            '                         Debug.Print("Bad Dictionary Declaration " & pdfSource.ToString)
            '                     Else
            '                         value.setDirect(True)
            '                         obj.setItem(key, value)
            '                     End If
            '                 End If
            'End While 
            '         Dim ch As Char = Convert.ToChar(pdfSource.read())
            '         If (ch <> ">"c) Then
            '             Throw New IOException("expected='>' actual='" & ch & "'")
            '         End If
            '         ch = Convert.ToChar(pdfSource.read())
            '         If (ch <> ">") Then
            '             Throw New IOException("expected='>' actual='" & ch & "'")
            '         End If
            '         Return obj
        End Function

        '/**
        ' * This will read a COSStream from the input stream.
        ' *
        ' * @param file The file to write the stream to when reading.
        ' * @param dic The dictionary that goes with this stream.
        ' *
        ' * @return The parsed pdf stream.
        ' *
        ' * @throws IOException If there is an error reading the stream.
        ' */
        Protected Overridable Function parseCOSStream(ByVal dic As COSDictionary, ByVal file As RandomAccess) As COSStream  'throws IOException
            Dim stream As New COSStream(dic, file)
            Dim out As Stream = Nothing
            Dim readCount As Integer
            Try
                Dim streamString As String = readString()
                'long streamLength;

                If (Not streamString.Equals(STREAM_STRING)) Then
                    Throw New IOException("expected='stream' actual='" & streamString & "'")
                End If

                'PDF Ref 3.2.7 A stream must be followed by either
                'a CRLF or LF but nothing else.

                Dim whitespace As Integer = pdfSource.read()

                'see brother_scan_cover.pdf, it adds whitespaces
                'after the stream but before the start of the
                'data, so just read those first
                While (whitespace = &H20)
                    whitespace = pdfSource.read()
                End While

                If (whitespace = &HD) Then
                    whitespace = pdfSource.read()
                    If (whitespace <> &HA) Then
                        pdfSource.unread(whitespace)
                        'The spec says this is invalid but it happens in the real
                        'world so we must support it.
                    End If
                ElseIf (whitespace = &HA) Then
                    'that is fine
                Else
                    'we are in an error.
                    'but again we will do a lenient parsing and just assume that everything
                    'is fine
                    pdfSource.unread(whitespace)
                End If

                'This needs to be dic.getItem because when we are parsing, the underlying object
                'might still be null.
                Dim streamLength As COSBase = dic.getItem(COSName.LENGTH)

                'Need to keep track of the
                out = stream.createFilteredStream(streamLength)

                ' try to read stream length - even if it is an indirect object
                Dim length As Integer = -1
                If (TypeOf (streamLength) Is COSNumber) Then
                    length = DirectCast(streamLength, COSNumber).intValue()
                End If
                ' commented out next chunk since for the sequentially working PDFParser
                ' we do not know if length object is redefined later on and the currently
                ' read indirect object might be obsolete (e.g. not referenced in xref table);
                ' this would result in reading wrong number of bytes;
                ' Thus the only reliable information is a direct length. 
                ' This exclusion shouldn't harm much since in case of indirect objects they will
                ' typically be defined after the stream object, thus keeping the directly
                ' provided length will fix most cases
                '            else if ( ( streamLength instanceof COSObject ) &&
                '                      ( ( (COSObject) streamLength ).getObject() instanceof COSNumber ) )
                '            {
                '                length = ( (COSNumber) ( (COSObject) streamLength ).getObject() ).intValue();
                '            } 

                If (length = -1) Then
                    ' Couldn't determine length from dict: just
                    ' scan until we find endstream:
                    readUntilEndStream(out)
                Else
                    ' Copy length bytes over:
                    Dim left As Integer = length
                    While (left > 0)
                        Dim chunk As Integer = Math.Min(left, strmBufLen)
                        readCount = pdfSource.read(strmBuf, 0, chunk)
                        If (readCount = -1) Then
                            Exit While
                        End If
                        out.Write(strmBuf, 0, readCount)
                        left -= readCount
                    End While

                    ' in order to handle broken documents we test if 'endstream' is reached
                    ' if not, length value possibly was wrong, fall back to scanning for endstream

                    ' fill buffer with next bytes and test for 'endstream' (with leading whitespaces)
                    readCount = pdfSource.read(strmBuf, 0, 20)
                    If (readCount > 0) Then
                        Dim foundEndstream As Boolean = False
                        Dim nextEndstreamCIdx As Integer = 0
                        For cIdx As Integer = 0 To readCount - 1
                            Dim ch As Integer = strmBuf(cIdx) And &HFF
                            If (ch = BaseParser.ENDSTREAM(nextEndstreamCIdx)) Then
                                nextEndstreamCIdx += 1
                                If (nextEndstreamCIdx >= BaseParser.ENDSTREAM.Length) Then
                                    foundEndstream = True
                                    Exit For
                                End If
                            ElseIf ((nextEndstreamCIdx > 0) OrElse (Not isWhitespace(ch))) Then
                                ' not found
                                Exit For
                            End If
                        Next

                        ' push back test bytes
                        pdfSource.unread(strmBuf, 0, readCount)

                        ' if 'endstream' was not found fall back to scanning
                        If (Not foundEndstream) Then
                            Debug.Print("Specified stream length " & length & " is wrong. Fall back to reading stream until 'endstream'.")

                            ' push back all read stream bytes
                            ' we got a buffered stream wrapper around filteredStream thus first flush to underlying stream
                            out.Flush()
                            Dim writtenStreamBytes As InputStream = stream.getFilteredStream()
                            Dim bout As New ByteArrayOutputStream(length)

                            readCount = writtenStreamBytes.read(strmBuf)
                            While (readCount >= 0)
                                bout.Write(strmBuf, 0, readCount)
                                readCount = writtenStreamBytes.read(strmBuf)
                            End While
                            Try
                                pdfSource.unread(bout.toByteArray())
                            Catch ioe As IOException
                                Throw New WrappedIOException("Could not push back " & bout.Size() & " bytes in order to reparse stream. Try increasing push back buffer using system property " & My.Settings.PROP_PUSHBACK_SIZE, ioe)
                            End Try
                            ' create new filtered stream
                            out = stream.createFilteredStream(streamLength)
                            ' scan until we find endstream:
                            readUntilEndStream(out)
                        End If
                    End If
                End If

                skipSpaces()
                Dim endStream As String = readString()

                If (Not endStream.Equals(ENDSTREAM_STRING)) Then
                    '/*
                    ' * Sometimes stream objects don't have an endstream tag so readUntilEndStream(out)
                    ' * also can stop on endobj tags. If that's the case we need to make sure to unread
                    ' * the endobj so parseObject() can handle that case normally.
                    ' */
                    If (endStream.StartsWith(ENDOBJ_STRING)) Then
                        Dim endobjarray() As Byte = Sistema.Strings.GetBytes(endStream, "ISO-8859-1")
                        pdfSource.unread(endobjarray)
                    End If
                    '/*
                    ' * Some PDF files don't contain a new line after endstream so we
                    ' * need to make sure that the next object number is getting read separately
                    ' * and not part of the endstream keyword. Ex. Some files would have "endstream8"
                    ' * instead of "endstream"
                    ' */
                ElseIf (endStream.StartsWith(ENDSTREAM_STRING)) Then
                    Dim extra As String = endStream.Substring(9, endStream.Length())
                    endStream = endStream.Substring(0, 9)
                    Dim array() As Byte = Sistema.Strings.GetBytes(extra, "ISO-8859-1")
                    pdfSource.unread(array)
                Else
                    '/*
                    ' * If for some reason we get something else here, Read until we find the next
                    ' * "endstream"
                    ' */
                    readUntilEndStream(out)
                    endStream = readString()
                    If (Not endStream.Equals(ENDSTREAM_STRING)) Then
                        Throw New IOException("expected='endstream' actual='" & endStream.ToString & "' " & pdfSource.ToString)
                    End If
                End If
            Finally
                If (out IsNot Nothing) Then
                    out.Close()
                End If
            End Try
            Return stream
        End Function

        '/**
        ' * This method will read through the current stream object until
        ' * we find the keyword "endstream" meaning we're at the end of this
        ' * object. Some pdf files, however, forget to write some endstream tags
        ' * and just close off objects with an "endobj" tag so we have to handle
        ' * this case as well.
        ' * 
        ' * This method is optimized using buffered IO and reduced number of
        ' * byte compare operations.
        ' * 
        ' * @param out  stream we write out to.
        ' * 
        ' * @throws IOException
        ' */
        Private Sub readUntilEndStream(ByVal out As InputStream) 'throws IOException
            Dim bufSize As Integer
            Dim charMatchCount As Integer = 0
            Dim keyw() As Byte = ENDSTREAM
            Dim ch As Char
            Dim quickTestOffset As Integer = 5 ' last character position of shortest keyword ('endobj')

            ' read next chunk into buffer; already matched chars are added to beginning of buffer
            bufSize = pdfSource.read(strmBuf, charMatchCount, strmBufLen - charMatchCount)
            While (bufSize > 0)
                bufSize += charMatchCount

                Dim bIdx As Integer = charMatchCount
                Dim quickTestIdx As Integer

                ' iterate over buffer, trying to find keyword match
                For maxQuicktestIdx As Integer = bufSize - quickTestOffset To bufSize - 1
                    ' reduce compare operations by first test last character we would have to
                    ' match if current one matches; if it is not a character from keywords
                    ' we can move behind the test character;
                    ' this shortcut is inspired by Boyer–Moore string search algorithm
                    ' and can reduce parsing time by approx. 20%
                    If ((charMatchCount = 0) AndAlso ((quickTestIdx = bIdx + quickTestOffset) < maxQuicktestIdx)) Then
                        ch = Convert.ToChar(strmBuf(quickTestIdx))
                        If ((ch > "t"c) OrElse (ch < "a"c)) Then
                            ' last character we would have to match if current character would match
                            ' is not a character from keywords -> jump behind and start over
                            bIdx = quickTestIdx
                            Continue For
                        End If
                    End If

                    ch = Convert.ToChar(strmBuf(bIdx)) ' could be negative - but we only compare to ASCII

                    If (ch = Convert.ToChar(keyw(charMatchCount))) Then
                        charMatchCount += 1
                        If (charMatchCount = keyw.Length) Then
                            ' match found
                            bIdx += 1
                            Exit For
                        End If
                    Else
                        If ((charMatchCount = 3) AndAlso (ch = Convert.ToChar(ENDOBJ(charMatchCount)))) Then
                            ' maybe ENDSTREAM is missing but we could have ENDOBJ
                            keyw = ENDOBJ
                            charMatchCount += 1
                        Else
                            ' no match; incrementing match start by 1 would be dumb since we already know matched chars
                            ' depending on current char read we may already have beginning of a new match:
                            ' 'e': first char matched;
                            ' 'n': if we are at match position idx 7 we already read 'e' thus 2 chars matched
                            ' for each other char we have to start matching first keyword char beginning with next 
                            ' read position
                            charMatchCount = IIf(ch = Convert.ToChar(E), 1, IIf((ch = Convert.ToChar(N)) AndAlso (charMatchCount = 7), 2, 0))
                            ' search again for 'endstream'
                            keyw = ENDSTREAM
                        End If
                    End If
                Next

                Dim contentBytes As Integer = Math.Max(0, bIdx - charMatchCount)

                ' write buffer content until first matched char to output stream
                If (contentBytes > 0) Then
                    out.Write(strmBuf, 0, contentBytes)
                End If
                If (charMatchCount = keyw.Length) Then
                    ' keyword matched; unread matched keyword (endstream/endobj) and following buffered content
                    pdfSource.unread(strmBuf, contentBytes, bufSize - contentBytes)
                    Exit While
                Else
                    ' copy matched chars at start of buffer
                    Array.Copy(keyw, 0, strmBuf, 0, charMatchCount)
                End If

            End While '  // while
        End Sub

        '/**
        ' * This is really a bug in the Document creators code, but it caused a crash
        ' * in PDFBox, the first bug was in this format:
        ' * /Title ( (5)
        ' * /Creator which was patched in 1 place.
        ' * However it missed the case where the Close Paren was escaped
        ' *
        ' * The second bug was in this format
        ' * /Title (c:\)
        ' * /Producer
        ' *
        ' * This patch  moves this code out of the parseCOSString method, so it can be used twice.
        ' *
        ' *
        ' * @param bracesParameter the number of braces currently open.
        ' *
        ' * @return the corrected value of the brace counter
        ' * @throws IOException
        ' */
        Private Function checkForMissingCloseParen(ByVal bracesParameter As Integer) As Integer ' throws IOException
            Dim braces As Integer = bracesParameter
            Dim nextThreeBytes() As Byte = Array.CreateInstance(GetType(Byte), 3)
            Dim amountRead As Integer = pdfSource.read(nextThreeBytes)

            '//lets handle the special case seen in Bull  River Rules and Regulations.pdf
            '//The dictionary looks like this
            '//    2 0 obj
            '//    <<
            '//        /Type /Info
            '//        /Creator (PaperPort http://www.scansoft.com)
            '//        /Producer (sspdflib 1.0 http://www.scansoft.com)
            '//        /Title ( (5)
            '//        /Author ()
            '//        /Subject ()
            '//
            '// Notice the /Title, the braces are not even but they should
            '// be.  So lets assume that if we encounter an this scenario
            '//   <end_brace><new_line><opening_slash> then that
            '// means that there is an error in the pdf and assume that
            '// was the end of the document.
            '//
            If (amountRead = 3) Then
                If ((nextThreeBytes(0) = &HD _
                        AndAlso nextThreeBytes(1) = &HA _
                        AndAlso nextThreeBytes(2) = &H2F) _
                        OrElse (nextThreeBytes(0) = &HD _
                                AndAlso nextThreeBytes(1) = &H2F)) Then
                    braces = 0
                End If
            End If
            'if (( nextThreeBytes(0] == 0x0d        // Look for a carriage return
            '                 && nextThreeBytes(1) == 0x0a   // Look for a new line
            '                 && nextThreeBytes(2) == 0x2f ) // Look for a slash /
            '                                                // Add a second case without a new line
            '                 || (nextThreeBytes(0) == 0x0d  // Look for a carriage return
            '                         && nextThreeBytes(1) == 0x2f ))  // Look for a slash /
            '             braces = 0;
            '         End If
            '         End If
            If (amountRead > 0) Then
                pdfSource.unread(nextThreeBytes, 0, amountRead)
            End If
            Return braces
        End Function

        '/**
        ' * This will parse a PDF string.
        ' *
        ' * @param isDictionary indicates if the stream is a dictionary or not
        ' * @return The parsed PDF string.
        ' *
        ' * @throws IOException If there is an error reading from the stream.
        ' */
        Protected Function parseCOSString(ByVal isDictionary As Boolean) As COSString 'throws IOException
            Dim nextChar As Char = Convert.ToChar(pdfSource.read())
            Dim retval As New COSString(isDictionary)
            Dim openBrace As Char
            Dim closeBrace As Char
            If (nextChar = "("c) Then
                openBrace = "("
                closeBrace = ")"
            ElseIf (nextChar = "<"c) Then
                Return parseCOSHexString()
            Else
                Throw New IOException("parseCOSString string should start with '(' or '<' and not '" & nextChar & "' " & pdfSource.ToString)
            End If

            'This is the number of braces read
            '
            Dim braces As Integer = 1
            Dim c As Integer = pdfSource.read()
            While (braces > 0 AndAlso c <> -1)
                Dim ch As Char = Convert.ToChar(c)
                Dim nextc As Integer = -2 ' not yet read

                If (ch = closeBrace) Then
                    braces -= 1
                    braces = checkForMissingCloseParen(braces)
                    If (braces <> 0) Then
                        retval.append(Convert.ToInt32(ch))
                    End If
                ElseIf (ch = openBrace) Then
                    braces += 1
                    retval.append(Convert.ToInt32(ch))
                ElseIf (ch = "\"c) Then
                    'patched by ram
                    Dim [next] As Char = Convert.ToChar(pdfSource.read())
                    Select Case ([next])
                        Case "n"c
                            retval.append(vbLf) ''\n'
                        Case "r"c
                            retval.append(vbCr) ''\r'
                        Case "t"c
                            retval.append(vbTab) ''\t'
                        Case "b"c
                            retval.append(vbBack) ''\b'
                        Case "f"c
                            retval.append(vbFormFeed) ''\f'
                        Case ")"c
                            ' PDFBox 276 /Title (c:\)
                            braces = checkForMissingCloseParen(braces)
                            If (braces <> 0) Then
                                retval.append(Convert.ToInt32([next]))
                            Else
                                retval.append("\")
                            End If
                        Case "(", "\"
                            retval.append(Convert.ToInt32([next]))
                        Case vbLf, vbCr 'ChrW(10), ChrW(13)
                            'this is a break in the line so ignore it and the newline and continue
                            c = pdfSource.read()
                            While (isEOL(c) AndAlso c <> -1)
                                c = pdfSource.read()
                            End While
                            nextc = c
                        Case "0", "1", "2", "3", "4", "5", "6", "7"
                            Dim octal As New StringBuffer()
                            octal.append([next])
                            c = pdfSource.read()
                            Dim digit As Char = Convert.ToChar(c)
                            If (digit >= "0"c AndAlso digit <= "7"c) Then
                                octal.append(digit)
                                c = pdfSource.read()
                                digit = Convert.ToChar(c)
                                If (digit >= "0"c AndAlso digit <= "7"c) Then
                                    octal.append(digit)
                                Else
                                    nextc = c
                                End If
                            Else
                                nextc = c
                            End If
                            Dim character As Integer = 0
                            Try
                                character = Integer.Parse(octal.ToString(), 8)
                            Catch e As NumberFormatException
                                Throw New IOException("Error: Expected octal character, actual='" & octal.ToString & "'")
                            End Try
                            retval.append(character)
                        Case Else
                            ' dropping the backslash
                            ' see 7.3.4.2 Literal Strings for further information
                            retval.append(Convert.ToInt32([next]))
                    End Select
                Else
                    retval.append(Convert.ToInt32(ch))
                End If
                If (nextc <> -2) Then
                    c = nextc
                Else
                    c = pdfSource.read()
                End If
            End While
            If (c <> -1) Then
                pdfSource.unread(c)
            End If
            Return retval
        End Function

        '/**
        ' * This will parse a PDF HEX string with fail fast semantic
        ' * meaning that we stop if a not allowed character is found.
        ' * This is necessary in order to detect malformed input and
        ' * be able to skip to next object start.
        ' *
        ' * We assume starting '<' was already read.
        ' * 
        ' * @return The parsed PDF string.
        ' *
        ' * @throws IOException If there is an error reading from the stream.
        ' */
        Private Function parseCOSHexString() As COSString  'throws IOException
            Dim sBuf As New StringBuffer()
            While (True)
                Dim c As Char = Convert.ToChar(pdfSource.read())
                If (isHexDigit(Convert.ToChar(c))) Then
                    sBuf.append(Convert.ToChar(c))
                ElseIf (c = ">"c) Then
                    Exit While
                ElseIf (Convert.ToInt32(c) < 0) Then
                    Throw New IOException("Missing closing bracket for hex string. Reached EOS.")
                ElseIf ((c = " "c) OrElse (c = vbLf) OrElse _
                       (c = vbTab) OrElse (c = vbCr) OrElse _
                       (c = vbBack) OrElse (c = vbFormFeed)) Then
                    Continue While
                Else
                    ' if invalid chars was found: discard last
                    ' hex character if it is not part of a pair
                    If (sBuf.length() Mod 2 <> 0) Then
                        sBuf.deleteCharAt(sBuf.length() - 1)
                    End If

                    ' read till the closing bracket was found
                    Do
                        c = Convert.ToChar(pdfSource.read())
                    Loop While (c <> ">"c)

                    ' exit loop
                    Exit While
                End If
            End While
            Return COSString.createFromHexString(sBuf.toString(), forceParsing)
        End Function

        '/**
        ' * This will parse a PDF array object.
        ' *
        ' * @return The parsed PDF array.
        ' *
        ' * @throws IOException If there is an error parsing the stream.
        ' */
        Protected Function parseCOSArray() As COSArray 'throws IOException
            Dim ch As Char = Convert.ToChar(pdfSource.read())
            If (ch <> "["c) Then
                Throw New IOException("expected='[' actual='" & ch & "'")
            End If
            Dim po As New COSArray()
            Dim pbo As COSBase = Nothing
            skipSpaces()
            Dim i As Integer = 0
            i = pdfSource.peek()
            While ((i > 0) AndAlso (Convert.ToChar(i) <> "]"))
                pbo = parseDirObject()
                If (TypeOf (pbo) Is COSObject) Then
                    ' We have to check if the expected values are there or not PDFBOX-385
                    If (TypeOf (po.get(po.size() - 1)) Is COSInteger) Then
                        Dim genNumber As COSInteger = po.remove(po.size() - 1)
                        If (TypeOf (po.get(po.size() - 1)) Is COSInteger) Then
                            Dim number As COSInteger = po.remove(po.size() - 1)
                            Dim key As New COSObjectKey(number.intValue(), genNumber.intValue())
                            pbo = document.getObjectFromPool(key)
                        Else
                            ' the object reference is somehow wrong
                            pbo = Nothing
                        End If
                    Else
                        pbo = Nothing
                    End If
                End If
                If (pbo IsNot Nothing) Then
                    po.add(pbo)
                Else
                    'it could be a bad object in the array which is just skipped
                    Debug.Print("Corrupt object reference")

                    ' This could also be an "endobj" or "endstream" which means we can assume that
                    ' the array has ended.
                    Dim isThisTheEnd As String = readString()
                    pdfSource.unread(Sistema.Strings.GetBytes(isThisTheEnd, "ISO-8859-1"))
                    If (ENDOBJ_STRING.Equals(isThisTheEnd) OrElse ENDSTREAM_STRING.Equals(isThisTheEnd)) Then
                        Return po
                    End If
                End If
                skipSpaces()
                i = pdfSource.peek()
            End While
            pdfSource.read() 'read ']'
            skipSpaces()
            Return po
        End Function

        '/**
        ' * Determine if a character terminates a PDF name.
        ' *
        ' * @param ch The character
        ' * @return <code>true</code> if the character terminates a PDF name, otherwise <code>false</code>.
        ' */
        Protected Function isEndOfName(ByVal ch As Char) As Boolean
            Return (ch = " "c OrElse ch = vbCr OrElse ch = vbLf OrElse ch = vbTab OrElse ch = ">"c OrElse ch = "<"c _
                   OrElse ch = "["c OrElse ch = "/"c OrElse ch = "]"c OrElse ch = ")"c OrElse ch = "("c OrElse Convert.ToInt32(ch) = -1) ' //EOF
        End Function

        '/**
        ' * This will parse a PDF name from the stream.
        ' *
        ' * @return The parsed PDF name.
        ' *
        ' * @throws IOException If there is an error reading from the stream.
        ' */
        Protected Function parseCOSName() As COSName ' throws IOException
            Dim retval As COSName = Nothing
            Dim c As Integer = pdfSource.read()
            If (Convert.ToChar(c) <> "/"c) Then
                Throw New IOException("expected='/' actual='" & Convert.ToChar(c) & "'-" & c + " " & pdfSource.ToString)
            End If
            ' costruisce il nome
            Dim buffer As New StringBuilder()
            c = pdfSource.read()
            While (c <> -1)
                Dim ch As Char = Convert.ToChar(c)
                If (ch = "#"c) Then
                    Dim ch1 As Char = Convert.ToChar(pdfSource.read())
                    Dim ch2 As Char = Convert.ToChar(pdfSource.read())

                    '// Prior to PDF v1.2, the # was not a special character.  Also,
                    '// it has been observed that various PDF tools do not follow the
                    '// spec with respect to the # escape, even though they report
                    '// PDF versions of 1.2 or later.  The solution here is that we
                    '// interpret the # as an escape only when it is followed by two
                    '// valid hex digits.
                    '//
                    If (isHexDigit(ch1) AndAlso isHexDigit(ch2)) Then
                        Dim hex As String = "" & ch1 & ch2
                        Try
                            buffer.Append(Convert.ToChar(Integer.Parse(hex, 16)))
                        Catch e As NumberFormatException
                            Throw New IOException("Error: expected hex number, actual='" & hex & "'")
                        End Try
                        c = pdfSource.read()
                    Else
                        pdfSource.unread(Convert.ToInt32(ch2))
                        c = Convert.ToInt32(ch1)
                        buffer.append(ch)
                    End If
                ElseIf (isEndOfName(ch)) Then
                    Exit While
                Else
                    buffer.append(ch)
                    c = pdfSource.read()
                End If
            End While
            If (c <> -1) Then
                pdfSource.unread(c)
            End If
            retval = COSName.getPDFName(buffer.toString())
            Return retval
        End Function

        '/**
        ' * This will parse a boolean object from the stream.
        ' *
        ' * @return The parsed boolean object.
        ' *
        ' * @throws IOException If an IO error occurs during parsing.
        ' */
        Protected Function parseBoolean() As COSBoolean ' throws IOException
            Dim retval As COSBoolean = Nothing
            Dim c As Char = Convert.ToChar(pdfSource.peek())
            If (c = "t"c) Then
                Dim trueString As String = Sistema.Strings.GetString(pdfSource.readFully(4), "ISO-8859-1")
                If (Not trueString.Equals([TRUE])) Then
                    Throw New IOException("Error parsing boolean: expected='true' actual='" & trueString & "'")
                Else
                    retval = COSBoolean.TRUE
                End If
            ElseIf (c = "f"c) Then
                Dim falseString As String = Sistema.Strings.GetString(pdfSource.readFully(5), "ISO-8859-1")
                If (Not falseString.Equals([FALSE])) Then
                    Throw New IOException("Error parsing boolean: expected='false' actual='" & falseString & "'")
                Else
                    retval = COSBoolean.FALSE
                End If
            Else
                Throw New IOException("Error parsing boolean expected='t or f' actual='" & c & "'")
            End If
            Return retval
        End Function

        '/**
        ' * This will parse a directory object from the stream.
        ' *
        ' * @return The parsed object.
        ' *
        ' * @throws IOException If there is an error during parsing.
        ' */
        Protected Function parseDirObject() As COSBase 'throws IOException
            Dim retval As COSBase = Nothing

            skipSpaces()
            Dim nextByte As Integer = pdfSource.peek()
            Dim c As Char = Convert.ToChar(nextByte)
            Select Case (c)
                Case "<"c
                    Dim leftBracket As Integer = pdfSource.read() 'pull off first left bracket
                    c = Convert.ToChar(pdfSource.peek()) 'check for second left bracket
                    pdfSource.unread(leftBracket)
                    If (c = "<"c) Then
                        retval = parseCOSDictionary()
                        skipSpaces()
                    Else
                        retval = parseCOSString(True)
                    End If
                Case "["c ' array
                    retval = parseCOSArray()
                Case "("c
                    retval = parseCOSString(True)
                Case "/" 'name
                    retval = parseCOSName()
                Case "n" ' null
                    Dim nullString As String = readString()
                    If (Not nullString.Equals(NULL)) Then
                        Throw New IOException("Expected='null' actual='" & nullString & "'")
                    End If
                    retval = COSNull.NULL
                Case "t"c
                    Dim trueString As String = Sistema.Strings.GetString(pdfSource.readFully(4), "ISO-8859-1")
                    If (trueString.Equals([TRUE])) Then
                        retval = COSBoolean.TRUE
                    Else
                        Throw New IOException("expected true actual='" & trueString & "' " & pdfSource.ToString)
                    End If
                Case "f"c
                    Dim falseString As String = Sistema.Strings.GetString(pdfSource.readFully(5), "ISO-8859-1")
                    If (falseString.Equals([FALSE])) Then
                        retval = COSBoolean.FALSE
                    Else
                        Throw New IOException("expected false actual='" & falseString & "' " & pdfSource.ToString)
                    End If
                Case "R"c
                    pdfSource.read()
                    retval = New COSObject(Nothing)
                Case Convert.ToChar(-1)
                    Return Nothing
                Case Else
                    If (NChar.isDigit(c) OrElse c = "-"c OrElse c = "+"c OrElse c = "."c) Then
                        Dim buf As New StringBuilder()
                        Dim ic As Integer = pdfSource.read()
                        c = Convert.ToChar(ic)
                        While (NChar.isDigit(c) OrElse _
                                c = "-"c OrElse _
                                c = "+" OrElse _
                                c = "." OrElse _
                                c = "E" OrElse _
                                c = "e")
                            buf.Append(c)
                            ic = pdfSource.read()
                            c = Convert.ToChar(ic)
                        End While
                        If (ic <> -1) Then
                            pdfSource.unread(ic)
                        End If
                        retval = COSNumber.get(buf.ToString())
                    Else
                        'This is not suppose to happen, but we will allow for it
                        'so we are more compatible with POS writers that don't
                        'follow the spec
                        Dim badString As String = readString()
                        'throw new IOException( "Unknown dir object c='" + c +
                        '"' peek='" + (char)pdfSource.peek() + "' " + pdfSource );
                        If (badString = "" OrElse badString.Length = 0) Then
                            Dim peek As Integer = pdfSource.peek()
                            ' we can end up in an infinite loop otherwise
                            Throw New IOException("Unknown dir object c='" & c & "' cInt=" + Convert.ToInt32(c) & " peek='" & Convert.ToChar(peek) & "' peekInt=" & peek & " " & pdfSource.getOffset())
                        End If

                        ' if it's an endstream/endobj, we want to put it back so the caller will see it
                        If (ENDOBJ_STRING.Equals(badString) OrElse ENDSTREAM_STRING.Equals(badString)) Then
                            pdfSource.unread(Sistema.Strings.GetBytes(badString, "ISO-8859-1"))
                        End If
                    End If
            End Select

            Return retval
        End Function

        '/**
        ' * This will read the next string from the stream.
        ' *
        ' * @return The string that was read from the stream.
        ' *
        ' * @throws IOException If there is an error reading from the stream.
        ' */
        Protected Overridable Function readString() As String 'throws IOException
            skipSpaces()
            Dim buffer As New StringBuilder()
            Dim c As Integer = pdfSource.read()
            While (Not isEndOfName(Convert.ToChar(c)) AndAlso Not isClosing(c) AndAlso c <> -1)
                buffer.Append(Convert.ToChar(c))
                c = pdfSource.read()
            End While
            If (c <> -1) Then
                pdfSource.unread(c)
            End If
            Return buffer.ToString()
        End Function

        '/**
        ' * This will read bytes until the end of line marker occurs.
        ' *
        ' * @param theString The next expected string in the stream.
        ' *
        ' * @return The characters between the current position and the end of the line.
        ' *
        ' * @throws IOException If there is an error reading from the stream or theString does not match what was read.
        ' */
        Protected Function readExpectedString(ByVal theString As String) As String  'throws IOException
            Dim c As Integer = pdfSource.read()
            While (isWhitespace(c) AndAlso c <> -1)
                c = pdfSource.read()
            End While
            Dim buffer As New StringBuilder(theString.Length())
            Dim charsRead As Integer = 0
            While (Not isEOL(c) AndAlso c <> -1 AndAlso charsRead < theString.Length())
                Dim [next] As Char = Convert.ToChar(c)
                buffer.append([next])
                If (theString.Chars(charsRead) = [next]) Then
                    charsRead += 1
                Else
                    pdfSource.unread(Sistema.Strings.GetBytes(buffer.ToString(), "ISO-8859-1"))
                    Throw New IOException("Error: Expected to read '" & theString & "' instead started reading '" & buffer.toString() & "'")
                End If
                c = pdfSource.read()
            End While
            While (isEOL(c) AndAlso c <> -1)
                c = pdfSource.read()
            End While
            If (c <> -1) Then
                pdfSource.unread(c)
            End If
            Return buffer.toString()
        End Function

        '/**
        ' * This will read the next string from the stream up to a certain length.
        ' *
        ' * @param length The length to stop reading at.
        ' *
        ' * @return The string that was read from the stream of length 0 to length.
        ' *
        ' * @throws IOException If there is an error reading from the stream.
        ' */
        Protected Function readString(ByVal length As Integer) As String ' throws IOException
            skipSpaces()

            Dim c As Char = Convert.ToChar(pdfSource.read())

            'average string size is around 2 and the normal string buffer size is
            'about 16 so lets save some space.
            Dim buffer As New StringBuilder(length)
            While (Not isWhitespace(AscW(c)) AndAlso Not isClosing(AscW(c)) AndAlso Convert.ToInt32(c) <> -1 AndAlso buffer.Length() < length AndAlso _
                    c <> "["c AndAlso _
                    c <> "<"c AndAlso _
                    c <> "("c AndAlso _
                    c <> "/"c)
                buffer.Append(Convert.ToChar(c))
                c = Convert.ToChar(pdfSource.read())
            End While
            If (Convert.ToInt32(c) <> -1) Then
                pdfSource.unread(Convert.ToByte(c))
            End If
            Return buffer.toString()
        End Function

        '/**
        ' * This will tell if the next character is a closing brace( close of PDF array ).
        ' *
        ' * @return true if the next byte is ']', false otherwise.
        ' *
        ' * @throws IOException If an IO error occurs.
        ' */
        Protected Function isClosing() As Boolean ' throws IOException
            Return isClosing(pdfSource.peek())
        End Function

        '/**
        ' * This will tell if the next character is a closing brace( close of PDF array ).
        ' *
        ' * @param c The character to check against end of line
        ' * @return true if the next byte is ']', false otherwise.
        ' */
        Protected Function isClosing(ByVal c As Integer) As Boolean
            Return Convert.ToChar(c) = "]"c
        End Function

        '/**
        ' * This will read bytes until the first end of line marker occurs.
        ' * Note: if you later unread the results of this function, you'll
        ' * need to add a newline character to the end of the string.
        ' *
        ' * @return The characters between the current position and the end of the line.
        ' *
        ' * @throws IOException If there is an error reading from the stream.
        ' */
        Protected Overridable Function readLine() As String ' throws IOException
            If (pdfSource.isEOF()) Then
                Throw New IOException("Error: End-of-File, expected line")
            End If

            Dim buffer As New StringBuilder(11)

            Dim c As Integer
            c = pdfSource.read()
            While (c <> -1)
                If (isEOL(c)) Then
                    Exit While
                End If
                buffer.Append(Convert.ToChar(c))
                c = pdfSource.read()
            End While
            Return buffer.ToString()
        End Function

        '/**
        ' * This will tell if the next byte to be read is an end of line byte.
        ' *
        ' * @return true if the next byte is 0x0A or 0x0D.
        ' *
        ' * @throws IOException If there is an error reading from the stream.
        ' */
        Protected Function isEOL() As Boolean ' throws IOException
            Return isEOL(pdfSource.peek())
        End Function

        '/**
        ' * This will tell if the next byte to be read is an end of line byte.
        ' *
        ' * @param c The character to check against end of line
        ' * @return true if the next byte is 0x0A or 0x0D.
        ' */
        Protected Function isEOL(ByVal c As Integer)
            Return c = 10 OrElse c = 13
        End Function

        '/**
        ' * This will tell if the next byte is whitespace or not.
        ' *
        ' * @return true if the next byte in the stream is a whitespace character.
        ' *
        ' * @throws IOException If there is an error reading from the stream.
        ' */
        Protected Function isWhitespace() As Boolean ' throws IOException
            Return isWhitespace(pdfSource.peek())
        End Function

        '/**
        ' * This will tell if the next byte is whitespace or not.  These values are
        ' * specified in table 1 (page 12) of ISO 32000-1:2008.
        ' * @param c The character to check against whitespace
        ' * @return true if the next byte in the stream is a whitespace character.
        ' */
        Protected Function isWhitespace(ByVal c As Integer) As Boolean
            Return c = 0 OrElse c = 9 OrElse c = 12 OrElse c = 10 OrElse c = 13 OrElse c = 32
        End Function

        '/**
        ' * This will skip all spaces and comments that are present.
        ' *
        ' * @throws IOException If there is an error reading from the stream.
        ' */
        Protected Sub skipSpaces() 'throws IOException
            'log( "skipSpaces() " + pdfSource );
            Dim c As Integer = pdfSource.read()
            ' identical to, but faster as: isWhiteSpace(c) || c == 37
            While (c = 0 OrElse c = 9 OrElse c = 12 OrElse c = 10 OrElse c = 13 OrElse c = 32 OrElse c = 37) '//37 is the % character, a comment
                If (c = 37) Then
                    ' skip past the comment section
                    c = pdfSource.read()
                    While (Not isEOL(c) AndAlso c <> -1)
                        c = pdfSource.read()
                    End While
                Else
                    c = pdfSource.read()
                End If
            End While
            If (c <> -1) Then
                pdfSource.unread(c)
            End If
            'log( "skipSpaces() done peek='" + (char)pdfSource.peek() + "'" );
        End Sub

        '/**
        ' * This will read a long from the Stream and throw an {@link IllegalArgumentException} if the long value
        ' * has more than 10 digits (i.e. : bigger than {@link #OBJECT_NUMBER_THRESHOLD})
        ' * @return
        ' * @throws IOException
        ' */
        Protected Function readObjectNumber() As Long 'throws IOException
            Dim retval As Long = readLong()
            If (retval < 0 OrElse retval >= OBJECT_NUMBER_THRESHOLD) Then
                Throw New IOException("Object Number '" & retval & "' has more than 10 digits or is negative")
            End If
            Return retval
        End Function

        '/**
        ' * This will read a integer from the Stream and throw an {@link IllegalArgumentException} if the integer value
        ' * has more than the maximum object revision (i.e. : bigger than {@link #GENERATION_NUMBER_THRESHOLD})
        ' * @return
        ' * @throws IOException
        ' */
        Protected Function readGenerationNumber() As Integer ' throws IOException
            Dim retval As Integer = readInt()
            If (retval < 0 OrElse retval >= GENERATION_NUMBER_THRESHOLD) Then
                Throw New IOException("Generation Number '" & retval & "' has more than 5 digits")
            End If
            Return retval
        End Function

        '/**
        ' * This will read an integer from the stream.
        ' *
        ' * @return The integer that was read from the stream.
        ' *
        ' * @throws IOException If there is an error reading from the stream.
        ' */
        Protected Overridable Function readInt() As Integer ' throws IOException
            skipSpaces()
            Dim retval As Integer = 0

            Dim intBuffer As StringBuilder = readStringNumber()

            Try
                retval = Integer.Parse(intBuffer.ToString())
            Catch e As NumberFormatException
                pdfSource.unread(Sistema.Strings.GetBytes(intBuffer.ToString(), "ISO-8859-1"))
                Throw New IOException("Error: Expected an integer type, actual='" & intBuffer.ToString & "'")
            End Try
            Return retval
        End Function


        '/**
        ' * This will read an long from the stream.
        ' *
        ' * @return The long that was read from the stream.
        ' *
        ' * @throws IOException If there is an error reading from the stream.
        ' */
        Protected Function readLong() As Long 'throws IOException
            skipSpaces()
            Dim retval As Long = 0

            Dim longBuffer As StringBuilder = readStringNumber()

            Try
                retval = Long.Parse(longBuffer.ToString())
            Catch e As NumberFormatException
                pdfSource.unread(Sistema.Strings.GetBytes(longBuffer.ToString(), "ISO-8859-1"))
                Throw New IOException("Error: Expected a long type, actual='" & longBuffer.ToString & "'")
            End Try
            Return retval
        End Function

        '/**
        ' * This method is used to read a token by the {@linkplain #readInt()} method and the {@linkplain #readLong()} method.
        ' *  
        ' * @return the token to parse as integer or long by the calling method.
        ' * @throws IOException throws by the {@link #pdfSource} methods.
        ' */
        Protected Function readStringNumber() As StringBuilder 'throws IOException
            Dim lastByte As Integer = 0
            Dim buffer As New StringBuilder()
            While ((lastByte = pdfSource.read()) <> 32 AndAlso _
                    lastByte <> 10 AndAlso _
                    lastByte <> 13 AndAlso _
                    lastByte <> 60 AndAlso _
                    lastByte <> 0 AndAlso _
                    lastByte <> -1)
                'lastByte <> 60 && //see sourceforge bug 1714707
                '            lastByte != 0 && //See sourceforge bug 853328
                '            lastByte != -1 )
                buffer.Append(Convert.ToChar(lastByte))
            End While
            If (lastByte <> -1) Then
                pdfSource.unread(lastByte)
            End If
            Return buffer
        End Function


    End Class

End Namespace

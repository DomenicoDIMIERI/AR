Imports FinSeA.Io
Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.exceptions
Imports FinSeA.org.apache.pdfbox.io
Imports FinSeA.org.apache.pdfbox.pdmodel
Imports FinSeA.org.apache.pdfbox.persistence.util
Imports FinSeA.org.apache.pdfbox.pdmodel.encryption
Imports FinSeA.Security
Imports System.Security

Namespace org.apache.pdfbox.pdfparser

    '/**
    ' * PDFParser which first reads startxref and xref tables in order to know valid
    ' * objects and parse only these objects. Thus it is closer to a conforming
    ' * parser than the sequential reading of {@link PDFParser}.
    ' * 
    ' * This class can be used as a {@link PDFParser} replacement. First
    ' * {@link #parse()} must be called before page objects can be retrieved, e.g.
    ' * {@link #getPDDocument()}.
    ' * 
    ' * This class is a much enhanced version of <code>QuickParser</code> presented
    ' * in <a
    ' * href="https://issues.apache.org/jira/browse/PDFBOX-1104">PDFBOX-1104</a> by
    ' * Jeremy Villalobos.
    ' */
    Public Class NonSequentialPDFParser
        Inherits PDFParser

        Private ReadOnly E As Integer = Asc("e"c)
        Private ReadOnly N As Integer = Asc("n"c)

        Private Shared ReadOnly EMPTY_INPUT_STREAM As New ByteArrayInputStream({})

        Protected Const DEFAULT_TRAIL_BYTECOUNT As Integer = 2048

        ''' <summary>
        ''' EOF-marker.
        ''' </summary>
        ''' <remarks></remarks>
        Protected Shared ReadOnly EOF_MARKER() As Char = {"%"c, "%"c, "E"c, "O"c, "F"c}

        ''' <summary>
        ''' StartXRef-marker.
        ''' </summary>
        ''' <remarks></remarks>
        Protected Shared ReadOnly STARTXREF_MARKER() As Char = {"s"c, "t"c, "a"c, "r"c, "t"c, "x"c, "r"c, "e"c, "f"c}

        ''' <summary>
        ''' obj-marker.
        ''' </summary>
        ''' <remarks></remarks>
        Protected Shared ReadOnly OBJ_MARKER() As Char = {"o"c, "b"c, "j"c}

        Private pdfFile As FinSeA.Io.File
        Private raStream As RandomAccessBufferedFileInputStream

        '/**
        ' * The security handler.
        ' */
        Protected securityHandler As SecurityHandler = Nothing

        Private keyStoreFilename As String = ""
        Private [alias] As String = ""
        Private password As String = ""
        Private readTrailBytes As Integer = DEFAULT_TRAIL_BYTECOUNT ' how many trailing  /  bytes to read for / EOF marker

        '/**
        ' * If <code>true</code> object references in catalog are not followed; pro:
        ' * page objects will be only parsed when needed; cons: some information of
        ' * catalog might not be available (e.g. outline). Catalog parsing without
        ' * pages is not an option since a number of entries will also refer to page
        ' * objects (like OpenAction).
        ' */
        Private parseMinimalCatalog As Boolean = My.Settings.SYSPROP_PARSEMINIMAL '"true".equals(System.getProperty(SYSPROP_PARSEMINIMAL));

        Private initialParseDone As Boolean = False
        Private allPagesParsed As Boolean = False

        'Private Shared LOG As Log = LogFactory.getLog(NonSequentialPDFParser.class)

        '/**
        ' * <code>true</code> if the NonSequentialPDFParser is initialized by a
        ' * InputStream, in this case a temporary file is created. At the end of the
        ' * {@linkplain #parse()} method,the temporary file will be deleted.
        ' */
        Private isTmpPDFFile As Boolean = False

        Public Const TMP_FILE_PREFIX As String = "tmpPDF"

        '// ------------------------------------------------------------------------
        '/**
        ' * Constructs parser for given file using memory buffer.
        ' * 
        ' * @param filename the filename of the pdf to be parsed
        ' * 
        ' * @throws IOException If something went wrong.
        ' */
        Public Sub New(ByVal filename As String) ' throws IOException
            Me.New(New FinSeA.Io.File(filename), Nothing)
        End Sub

        '/**
        ' * Constructs parser for given file using given buffer for temporary
        ' * storage.
        ' * 
        ' * @param file the pdf to be parsed
        ' * @param raBuf the buffer to be used for parsing
        ' * 
        ' * @throws IOException If something went wrong.
        ' */
        '/**
        ' * Constructs parser for given file using given buffer for temporary
        ' * storage.
        ' * 
        ' * @param file the pdf to be parsed
        ' * @param raBuf the buffer to be used for parsing
        ' * 
        ' * @throws IOException If something went wrong.
        ' */
        Public Sub New(ByVal file As FinSeA.Io.File, ByVal raBuf As RandomAccess) ' throws IOException
            Me.New(file, raBuf, "")
        End Sub

        '/**
        ' * Constructs parser for given file using given buffer for temporary
        ' * storage.
        ' * 
        ' * @param file the pdf to be parsed
        ' * @param raBuf the buffer to be used for parsing
        ' * 
        ' * @throws IOException If something went wrong.
        ' */
        '/**
        ' * Constructs parser for given file using given buffer for temporary
        ' * storage.
        ' * 
        ' * @param file the pdf to be parsed
        ' * @param raBuf the buffer to be used for parsing
        ' * @param decryptionPassword password to be used for decryption
        ' * 
        ' * @throws IOException If something went wrong.
        ' */
        Public Sub New(ByVal file As FinSeA.Io.File, ByVal raBuf As RandomAccess, ByVal decryptionPassword As String) ' throws IOException
            MyBase.New(EMPTY_INPUT_STREAM, Nothing, False)
            pdfFile = file
            raStream = New RandomAccessBufferedFileInputStream(pdfFile)
            init(file, raBuf, decryptionPassword)
        End Sub

        Private Sub init(ByVal file As FinSeA.Io.File, ByVal raBuf As RandomAccess, ByVal decryptionPassword As String) ' throws IOException
            Dim eofLookupRangeStr As Integer = My.Settings.SYSPROP_EOFLOOKUPRANGE
            'If (eofLookupRangeStr <> "") Then
            '    Try
            '        setEOFLookupRange(Integer.parseInt(eofLookupRangeStr))
            '    Catch nfe As FormatException
            '        Debug.Print("System property [SYSPROP_EOFLOOKUPRANGE] does not contain an integer value, but: '" & eofLookupRangeStr & "'")
            '    End Try
            'End If
            If (raBuf Is Nothing) Then
                setDocument(New COSDocument(New RandomAccessBuffer(), False))
            Else
                setDocument(New COSDocument(raBuf, False))
            End If

            pdfSource = New pdfbox.io.PushBackInputStream(raStream, 4096)

            password = decryptionPassword
        End Sub

        '/**
        ' * Constructor.
        ' * 
        ' * @param input input stream representing the pdf.
        ' * @throws IOException If something went wrong.
        ' */
        Public Sub New(ByVal input As Stream) ' throws IOException
            Me.New(input, Nothing, "")
        End Sub

        '/**
        ' * Constructor.
        ' * 
        ' * @param input input stream representing the pdf.
        ' * @param raBuf the buffer to be used for parsing
        ' * @param decryptionPassword password to be used for decryption.
        ' * @throws IOException If something went wrong.
        ' */
        Public Sub New(ByVal input As Stream, ByVal raBuf As RandomAccess, ByVal decryptionPassword As String) ' throws IOException
            MyBase.New(EMPTY_INPUT_STREAM, Nothing, False)
            pdfFile = createTmpFile(input)
            raStream = New RandomAccessBufferedFileInputStream(pdfFile)
            init(pdfFile, raBuf, decryptionPassword)
        End Sub

        '/**
        ' * Create a temporary file with the input stream. If the creation succeed,
        ' * the {@linkplain #isTmpPDFFile} is set to true. This Temporary file will
        ' * be deleted at end of the parse method
        ' * 
        ' * @param input
        ' * @return
        ' * @throws IOException If something went wrong.
        ' */
        Private Function createTmpFile(ByVal input As Stream) As FinSeA.Io.File  ' throws IOException
            Dim tmpFile As FinSeA.Io.File
            Dim fos As FileOutputStream = Nothing
            Try
                tmpFile = New FinSeA.Io.File(FinSeA.Sistema.FileSystem.GetTempFileName(TMP_FILE_PREFIX, ".pdf"))
                fos = New FileOutputStream(tmpFile)
                IOUtils.copy(input, fos)
                isTmpPDFFile = True
                Return tmpFile
            Finally
                IOUtils.closeQuietly(input)
                IOUtils.closeQuietly(fos)
            End Try
        End Function

        '// ------------------------------------------------------------------------
        '/**
        ' * Sets how many trailing bytes of PDF file are searched for EOF marker and
        ' * 'startxref' marker. If not set we use default value
        ' * {@link #DEFAULT_TRAIL_BYTECOUNT}.
        ' * 
        ' * <p<We check that new value is at least 16. However for practical use
        ' * cases this value should not be lower than 1000; even 2000 was found to
        ' * not be enough in some cases where some trailing garbage like HTML
        ' * snippets followed the EOF marker.</p>
        ' * 
        ' * <p>In case system property {@link #SYSPROP_EOFLOOKUPRANGE} is defined
        ' * this value will be set on initialization but can be overwritten
        ' * later.</p>
        ' * 
        ' * @param byteCount number of trailing bytes
        ' */
        Public Sub setEOFLookupRange(ByVal byteCount As Integer)
            If (byteCount > 15) Then
                readTrailBytes = byteCount
            End If
        End Sub

        '// ------------------------------------------------------------------------
        '/**
        ' * The initial parse will first parse only the trailer, the xrefstart and
        ' * all xref tables to have a pointer (offset) to all the pdf's objects. It
        ' * can handle linearized pdfs, which will have an xref at the end pointing
        ' * to an xref at the beginning of the file. Last the root object is parsed.
        ' * 
        ' * @throws IOException If something went wrong.
        ' */
        Protected Sub initialParse() 'throws IOException
            Dim startxrefOff As Long = getStartxrefOffset()
            Dim trailer As COSDictionary
            ' ---- parse startxref
            setPdfSource(startxrefOff)
            parseStartXref()

            Dim xrefOffset As Long = document.getStartXref()
            Dim prev As Long = xrefOffset

            ' ---- parse whole chain of xref tables/object streams using PREV
            ' reference
            While (prev > -1)
                ' seek to xref table
                setPdfSource(prev)

                ' skip white spaces
                skipSpaces()
                ' -- parse xref
                If (pdfSource.peek() = Asc("x"c)) Then
                    ' xref table and trailer
                    ' use existing parser to parse xref table
                    parseXrefTable(prev)

                    ' parse the last trailer.
                    If (Not parseTrailer()) Then
                        Throw New IOException("Expected trailer object at position: " & pdfSource.getOffset())
                    End If
                    trailer = xrefTrailerResolver.getCurrentTrailer()
                    prev = trailer.getInt(COSName.PREV)
                Else
                    ' xref stream
                    prev = parseXrefObjStream(prev)
                End If
            End While

            ' ---- build valid xrefs out of the xref chain
            xrefTrailerResolver.setStartxref(xrefOffset)
            trailer = xrefTrailerResolver.getTrailer()
            document.setTrailer(trailer)

            ' ---- prepare encryption if necessary
            Dim trailerEncryptItem As COSBase = document.getTrailer().getItem(COSName.ENCRYPT)
            If (trailerEncryptItem IsNot Nothing) Then
                If (TypeOf (trailerEncryptItem) Is COSObject) Then
                    Dim trailerEncryptObj As COSObject = trailerEncryptItem
                    parseObjectDynamically(trailerEncryptObj, True)
                End If
                Try
                    Dim encParameters As New PDEncryptionDictionary(document.getEncryptionDictionary())

                    Dim decryptionMaterial As DecryptionMaterial = Nothing
                    If (keyStoreFilename <> "") Then
                        Dim ks As KeyStore = KeyStore.getInstance("PKCS12")
                        ks.load(New FileInputStream(keyStoreFilename), password.ToCharArray())
                        decryptionMaterial = New PublicKeyDecryptionMaterial(ks, [alias], password)
                    Else
                        decryptionMaterial = New StandardDecryptionMaterial(password)
                    End If

                    securityHandler = SecurityHandlersManager.getInstance().getSecurityHandler(encParameters.getFilter())
                    securityHandler.prepareForDecryption(encParameters, document.getDocumentID(), decryptionMaterial)

                    Dim permission As AccessPermission = securityHandler.getCurrentAccessPermission()
                    If (Not permission.canExtractContent()) Then
                        Debug.Print("PDF file '" & pdfFile.getPath() & "' does not allow extracting content.")
                    End If
                Catch e As Exception
                    Throw New IOException("Error (" & e.GetType().Name() & ") while creating security handler for decryption: " & e.Message)
                    '* , e TODO: remove
                    '* remark with Java 1.6
                    '*/);
                End Try
            End If

            ' PDFBOX-1557 - ensure that all COSObject are loaded in the trailer
            ' PDFBOX-1606 - after securityHandler has been instantiated
            For Each trailerEntry As COSBase In trailer.getValues()
                If (TypeOf (trailerEntry) Is COSObject) Then
                    Dim tmpObj As COSObject = trailerEntry
                    parseObjectDynamically(tmpObj, False)
                End If
            Next
            ' ---- parse catalog or root object
            Dim root As COSObject = xrefTrailerResolver.getTrailer().getItem(COSName.ROOT)

            If (root Is Nothing) Then
                Throw New IOException("Missing root object specification in trailer.")
            End If

            parseObjectDynamically(root, False)

            ' ---- resolve all objects (including pages)
            If (Not parseMinimalCatalog) Then
                Dim catalogObj As COSObject = document.getCatalog()
                If (catalogObj IsNot Nothing) Then
                    If (TypeOf (catalogObj.getObject()) Is COSDictionary) Then
                        parseDictObjects(catalogObj.getObject(), Nothing)
                        allPagesParsed = True
                        document.setDecrypted()
                    End If
                End If
            End If
            initialParseDone = True
        End Sub

        '// ------------------------------------------------------------------------
        '/**
        ' * Parses an xref object stream starting with indirect object id.
        ' * 
        ' * @return value of PREV item in dictionary or <code>-1</code> if no such
        ' *         item exists
        ' */
        Private Function parseXrefObjStream(ByVal objByteOffset As Long) As Long ' throws IOException
            ' ---- parse indirect object head
            readObjectNumber()
            readGenerationNumber()
            readPattern(OBJ_MARKER)

            Dim dict As COSDictionary = parseCOSDictionary()
            Dim xrefStream As COSStream = parseCOSStream(dict, getDocument().getScratchFile())
            parseXrefStream(xrefStream, objByteOffset)

            Return dict.getLong(COSName.PREV)
        End Function

        ' ------------------------------------------------------------------------
        ' Get current offset in file at which next byte would be read. */
        Private Function getPdfSourceOffset() As Long
            Return pdfSource.getOffset()
        End Function

        '/**
        ' * Sets {@link #pdfSource} to start next parsing at given file offset.
        ' * 
        ' * @param fileOffset file offset
        ' * @throws IOException If something went wrong.
        ' */
        Protected Sub setPdfSource(ByVal fileOffset As Long) ' throws IOException

            pdfSource.Seek(fileOffset)

            ' alternative using 'old fashioned' input stream
            ' if ( pdfSource IsNot Nothing )
            ' pdfSource.close();
            '
            ' pdfSource = new PushBackInputStream(
            ' new BufferedInputStream(
            ' new FileInputStream( file ), 16384), 4096);
            ' pdfSource.skip( _fileOffset );
        End Sub

        '/**
        ' * Enable handling of alternative pdfSource implementation.
        ' * @throws IOException If something went wrong.
        ' */
        Protected Sub releasePdfSourceInputStream() 'throws IOException
            ' if ( pdfSource IsNot Nothing )
            ' pdfSource.close();
        End Sub

        Private Sub closeFileStream() 'throws IOException
            If (pdfSource IsNot Nothing) Then
                pdfSource.Close()
            End If
        End Sub

        '// ------------------------------------------------------------------------
        '/**
        ' * Looks for and parses startxref. We first look for last '%%EOF' marker
        ' * (within last {@link #DEFAULT_TRAIL_BYTECOUNT} bytes (or range set via
        ' * {@link #setEOFLookupRange(int)}) and go back to find
        ' * <code>startxref</code>.
        ' * 
        ' * @return the offset of StartXref 
        ' * @throws IOException If something went wrong.
        ' */
        Protected Function getStartxrefOffset() As Long ' throws IOException
            Dim buf() As Byte
            Dim skipBytes As Long

            ' ---- read trailing bytes into buffer
            Dim fileLen As Long = pdfFile.Length()

            Dim fIn As FileInputStream = Nothing
            Try
                fIn = New FileInputStream(pdfFile)

                Dim trailByteCount As Integer = IIf(fileLen < readTrailBytes, fileLen, readTrailBytes)
                buf = Array.CreateInstance(GetType(Byte), trailByteCount)
                fIn.skip(skipBytes = fileLen - trailByteCount)

                Dim off As Integer = 0
                Dim readBytes As Integer
                While (off < trailByteCount)
                    readBytes = fIn.read(buf, off, trailByteCount - off)
                    ' in order to not get stuck in a loop we check readBytes (this
                    ' should never happen)
                    If (readBytes < 1) Then
                        Throw New IOException("No more bytes to read for trailing buffer, but expected: " & (trailByteCount - off))
                    End If
                    off += readBytes
                End While
            Finally
                If (fIn IsNot Nothing) Then
                    Try
                        fIn.Close()
                    Catch ioe As IOException
                    End Try
                End If
            End Try

            ' ---- find last '%%EOF'
            Dim bufOff As Integer = lastIndexOf(EOF_MARKER, buf, buf.Length)
            If (bufOff < 0) Then
                Throw New IOException("Missing end of file marker '" & (New String(EOF_MARKER)) & "'")
            End If
            ' ---- find last startxref preceding EOF marker
            bufOff = lastIndexOf(STARTXREF_MARKER, buf, bufOff)

            If (bufOff < 0) Then
                Throw New IOException("Missing 'startxref' marker.")
            End If
            Return skipBytes + bufOff
        End Function

        '// ------------------------------------------------------------------------
        '/**
        ' * Searches last appearance of pattern within buffer. Lookup before _lastOff
        ' * and goes back until 0.
        ' * 
        ' * @param pattern pattern to search for
        ' * @param buf buffer to search pattern in
        ' * @param endOff offset (exclusive) where lookup starts at
        ' * 
        ' * @return start offset of pattern within buffer or <code>-1</code> if
        ' *         pattern could not be found
        ' */
        Protected Function lastIndexOf(ByVal pattern() As Char, ByVal buf() As Byte, ByVal endOff As Integer) As Integer
            Dim lastPatternChOff As Integer = pattern.Length - 1

            Dim bufOff As Integer = endOff
            Dim patOff As Integer = lastPatternChOff
            Dim lookupCh As Char = pattern(patOff)

            While (bufOff > 0)
                bufOff -= 1
                If (buf(bufOff) = Asc(lookupCh)) Then
                    patOff -= 1
                    If (patOff < 0) Then
                        ' whole pattern matched
                        Return bufOff
                    End If
                    ' matched current char, advance to preceding one
                    lookupCh = pattern(patOff)
                ElseIf (patOff < lastPatternChOff) Then
                    ' no char match but already matched some chars; reset
                    lookupCh = pattern(patOff = lastPatternChOff)
                End If
            End While

            Return -1
        End Function

        '// ------------------------------------------------------------------------
        '/**
        ' * Reads given pattern from {@link #pdfSource}. Skipping whitespace at start
        ' * and end.
        ' * 
        ' * @param pattern pattern to be skipped
        ' * @throws IOException if pattern could not be read
        ' */
        Protected Sub readPattern(ByVal pattern() As Char) ' throws IOException
            skipSpaces()

            For Each c As Char In pattern
                If (pdfSource.read() <> Convert.ToInt32(c)) Then
                    Throw New IOException("Expected pattern '" & New String(pattern) & " but missed at character '" & c & "'")
                End If
            Next

            skipSpaces()
        End Sub

        ' ------------------------------------------------------------------------
        Private pagesDictionary As COSDictionary = Nothing

        '/**
        ' * Returns PAGES {@link COSDictionary} object or throws {@link IOException}
        ' * if PAGES dictionary does not exist.
        ' */
        Private Function getPagesObject() As COSDictionary ' throws IOException
            If (pagesDictionary IsNot Nothing) Then
                Return pagesDictionary
            End If
            Dim pages As COSObject = document.getCatalog().getItem(COSName.PAGES)

            If (pages Is Nothing) Then
                Throw New IOException("Missing PAGES entry in document catalog.")
            End If

            Dim [object] As COSBase = parseObjectDynamically(pages, False)

            If (Not (TypeOf ([object]) Is COSDictionary)) Then
                Throw New IOException("PAGES not a dictionary object, but: " & [object].GetType().Name())
            End If

            pagesDictionary = [object]

            Return pagesDictionary
        End Function

        '// ------------------------------------------------------------------------
        '/** Parses all objects needed by pages and closes input stream. */
        '/**
        ' * {@inheritDoc}
        ' */
        Public Overrides Sub parse() ' throws IOException
            Dim exceptionOccurred As Boolean = True ' set to false if all is processed

            Try
                If (Not initialParseDone) Then
                    initialParse()
                End If

                Dim pageCount As Integer = getPageNumber()

                If (Not allPagesParsed) Then
                    For pNr As Integer = 0 To pageCount - 1
                        getPage(pNr)
                    Next
                    allPagesParsed = True
                    document.setDecrypted()
                End If
                exceptionOccurred = False
            Finally
                Try
                    closeFileStream()
                Catch ioe As IOException
                End Try

                deleteTempFile()

                If (exceptionOccurred AndAlso (document IsNot Nothing)) Then
                    Try
                        document.close()
                        document = Nothing
                    Catch ioe As IOException
                    End Try
                End If
            End Try
        End Sub

        '/**
        ' * Return the pdf file.
        ' * 
        ' * @return the pdf file
        ' */
        Protected Function getPdfFile() As FinSeA.Io.File
            Return Me.pdfFile
        End Function

        '/**
        ' * Remove the temporary file. A temporary file is created if this class is
        ' * instantiated with an InputStream
        ' */
        Protected Sub deleteTempFile()
            If (isTmpPDFFile) Then
                Try
                    If (Not pdfFile.delete()) Then
                        Debug.Print("Temporary file '" & pdfFile.getName() & "' can't be deleted")
                    End If
                Catch e As SecurityException
                    Debug.Print("Temporary file '" & pdfFile.getName() & "' can't be deleted", e)
                End Try
            End If
        End Sub

        '// ------------------------------------------------------------------------
        '/**
        ' * Returns security handler of the document or <code>null</code> if document
        ' * is not encrypted or {@link #parse()} wasn't called before.
        ' * 
        ' * @return the security handler.
        ' */
        Public Function getSecurityHandler() As SecurityHandler
            Return securityHandler
        End Function

        '// ------------------------------------------------------------------------
        '/**
        ' * This will get the PD document that was parsed. When you are done with
        ' * this document you must call close() on it to release resources.
        ' * 
        ' * Overwriting super method was necessary in order to set security handler.
        ' * 
        ' * @return The document at the PD layer.
        ' * 
        ' * @throws IOException If there is an error getting the document.
        ' */
        Public Overrides Function getPDDocument() As PDDocument ' throws IOException
            Dim pdDocument As PDDocument = MyBase.getPDDocument()
            If (securityHandler IsNot Nothing) Then
                pdDocument.setSecurityHandler(securityHandler)
            End If
            Return pdDocument
        End Function

        '// ------------------------------------------------------------------------
        '/**
        ' * Returns the number of pages in a document.
        ' * 
        ' * @return the number of pages.
        ' * 
        ' * @throws IOException if PAGES or other needed object is missing
        ' */
        Public Function getPageNumber() As Integer ' throws IOException
            Dim pageCount As Integer = getPagesObject().getInt(COSName.COUNT)

            If (pageCount < 0) Then
                Throw New IOException("No page number specified.")
            End If
            Return pageCount
        End Function

        '// ------------------------------------------------------------------------
        '/**
        ' * Returns the page requested with all the objects loaded into it.
        ' * 
        ' * @param pageNr starts from 0 to the number of pages.
        ' * @return the page with the given pagenumber.
        ' * @throws IOException If something went wrong.
        ' */
        Public Function getPage(ByVal pageNr As Integer) As PDPage 'throws IOException
            getPagesObject()

            ' ---- get list of top level pages
            Dim kids As COSArray = pagesDictionary.getDictionaryObject(COSName.KIDS)

            If (kids Is Nothing) Then
                Throw New IOException("Missing 'Kids' entry in pages dictionary.")
            End If

            ' ---- get page we are looking for (possibly going recursively into
            ' subpages)
            Dim pageObj As COSObject = getPageObject(pageNr, kids, 0)

            If (pageObj Is Nothing) Then
                Throw New IOException("Page " & pageNr & " not found.")
            End If

            ' ---- parse all objects necessary to load page.
            Dim pageDict As COSDictionary = pageObj.getObject()

            If (parseMinimalCatalog AndAlso (Not allPagesParsed)) Then
                ' parse page resources since we did not do this on start
                Dim resDict As COSDictionary = pageDict.getDictionaryObject(COSName.RESOURCES)
                parseDictObjects(resDict)
            End If

            Return New PDPage(pageDict)
        End Function

        '/**
        ' * Returns the object for a specific page. The page tree is made up of kids.
        ' * The kids have COSArray with COSObjects inside of them. The COSObject can
        ' * be parsed using the dynamic parsing method We want to only parse the
        ' * minimum COSObjects and still return a complete page. ready to be used.
        ' * 
        ' * @param num the requested page number; numbering starts with 0
        ' * @param startKids Kids array to start with looking up page number
        ' * @param startPageCount
        ' * 
        ' * @return page object or <code>null</code> if no such page exists
        ' * 
        ' * @throws IOException
        ' */
        Private Function getPageObject(ByVal num As Integer, ByVal startKids As COSArray, ByVal startPageCount As Integer) As COSObject 'throws IOException
            Dim curPageCount As Integer = startPageCount
            Dim kidsIter As Iterator(Of COSBase) = startKids.iterator() 'Iterator(Of COSBase) 

            While (kidsIter.hasNext())
                Dim obj As COSObject = kidsIter.next()
                Dim base As COSBase = obj.getObject()
                If (base Is Nothing) Then
                    base = parseObjectDynamically(obj, False)
                    obj.setObject(base)
                End If

                Dim dic As COSDictionary = base
                Dim count As Integer = dic.getInt(COSName.COUNT)
                If (count >= 0) Then
                    ' skip this branch if requested page comes later
                    If ((curPageCount + count) <= num) Then
                        curPageCount += count
                        Continue While
                    End If
                End If

                Dim kids As COSArray = dic.getDictionaryObject(COSName.KIDS)
                If (kids IsNot Nothing) Then
                    ' recursively scan subpages
                    Dim ans As COSObject = getPageObject(num, kids, curPageCount)
                    ' if ans is not null, we got what we were looking for
                    If (ans IsNot Nothing) Then
                        Return ans
                    End If
                Else
                    ' found page?
                    If (curPageCount = num) Then
                        Return obj
                    End If
                    ' page has no kids and it is not the page we are looking for
                    curPageCount += 1
                End If
            End While
            Return Nothing
        End Function

        '/**
        ' * Creates a unique object id using object number and object generation
        ' * number. (requires object number < 2^31))
        ' */
        Private Function getObjectId(ByVal obj As COSObject) As Long
            Return (obj.getObjectNumber().longValue() << 32) Or obj.getGenerationNumber().longValue()
        End Function

        '/**
        ' * Adds all from newObjects to toBeParsedList if it is not an COSObject or
        ' * we didn't add this COSObject already (checked via addedObjects).
        ' */
        Private Sub addNewToList(ByVal toBeParsedList As List(Of COSBase), ByVal newObjects As ICollection(Of COSBase), ByVal addedObjects As ICollection(Of NLong))
            For Each newObject As COSBase In newObjects
                If (TypeOf (newObject) Is COSObject) Then
                    Dim objId As Long = getObjectId(newObject)
                    If (Not addedObjects.add(objId)) Then
                        Continue For
                    End If
                End If
                toBeParsedList.add(newObject)
            Next
        End Sub

        '/**
        ' * Adds newObject to toBeParsedList if it is not an COSObject or we didn't
        ' * add this COSObject already (checked via addedObjects).
        ' */
        Private Sub addNewToList(ByVal toBeParsedList As List(Of COSBase), ByVal newObject As COSBase, ByVal addedObjects As ICollection(Of NLong))
            If (TypeOf (newObject) Is COSObject) Then
                Dim objId As Long = getObjectId(newObject)
                If (Not addedObjects.add(objId)) Then
                    Return
                End If
            End If
            toBeParsedList.add(newObject)
        End Sub

        '/**
        ' * Will parse every object necessary to load a single page from the pdf
        ' * document. We try our best to order objects according to offset in file
        ' * before reading to minimize seek operations.
        ' * 
        ' * @param dict the COSObject from the parent pages.
        ' * @param excludeObjects dictionary object reference entries with these
        ' *            names will not be parsed
        ' * 
        ' * @throws IOException
        ' */
        Private Sub parseDictObjects(ByVal dict As COSDictionary, ByVal ParamArray excludeObjects() As COSName) 'throws IOException
            ' ---- create queue for objects waiting for further parsing
            Dim toBeParsedList As New LinkedList(Of COSBase)()
            ' offset ordered object map
            Dim objToBeParsed As New TreeMap(Of NLong, List(Of COSObject))
            ' in case of compressed objects offset points to stmObj
            Dim parsedObjects As New HashSet(Of NLong)
            Dim addedObjects As New HashSet(Of NLong)

            ' ---- add objects not to be parsed to list of already parsed objects
            If (excludeObjects IsNot Nothing) Then
                For Each objName As COSName In excludeObjects
                    Dim baseObj As COSBase = dict.getItem(objName)
                    If (TypeOf (baseObj) Is COSObject) Then
                        parsedObjects.add(getObjectId(baseObj))
                    End If
                Next
            End If

            addNewToList(toBeParsedList, dict.getValues(), addedObjects)

            ' ---- go through objects to be parsed
            While (Not (toBeParsedList.isEmpty() AndAlso objToBeParsed.isEmpty()))
                ' -- first get all COSObject from other kind of objects and
                ' put them in objToBeParsed; afterwards toBeParsedList is empty
                Dim baseObj As COSBase
                baseObj = toBeParsedList.poll()
                While (baseObj IsNot Nothing)
                    If (TypeOf (baseObj) Is COSStream) Then
                        addNewToList(toBeParsedList, DirectCast(baseObj, COSStream).getValues(), addedObjects)
                    ElseIf (TypeOf (baseObj) Is COSDictionary) Then
                        addNewToList(toBeParsedList, DirectCast(baseObj, COSDictionary).getValues(), addedObjects)
                    ElseIf (TypeOf (baseObj) Is COSArray) Then
                        Dim arrIter As Iterator(Of COSBase) = DirectCast(baseObj, COSArray).iterator()
                        While (arrIter.hasNext())
                            addNewToList(toBeParsedList, arrIter.next(), addedObjects)
                        End While
                    ElseIf (TypeOf (baseObj) Is COSObject) Then
                        Dim obj As COSObject = baseObj
                        Dim objId As Long = getObjectId(obj)
                        Dim objKey As New COSObjectKey(obj.getObjectNumber().intValue(), obj.getGenerationNumber().intValue())

                        '/*
                        '                                                  * || document.hasObjectInPool ( objKey )
                        '                                                  */))
                        '             {
                        If Not (parsedObjects.contains(objId)) Then
                            Dim fileOffset As NLong = xrefTrailerResolver.getXrefTable().get(objKey)
                            ' it is allowed that object references point to null,
                            ' thus we have to test
                            If (fileOffset.HasValue) Then
                                If (fileOffset > 0) Then
                                    objToBeParsed.put(fileOffset, Collections.singletonList(Of COSObject)(obj))
                                Else
                                    ' negative offset means we have a compressed
                                    ' object within object stream;
                                    ' get offset of object stream
                                    fileOffset = xrefTrailerResolver.getXrefTable().get(New COSObjectKey(-fileOffset, 0))
                                    If ((fileOffset.HasValue) OrElse (fileOffset <= 0)) Then
                                        Throw New IOException("Invalid object stream xref object reference: " & fileOffset)
                                    End If

                                    Dim stmObjects As List(Of COSObject) = objToBeParsed.get(fileOffset)
                                    If (stmObjects Is Nothing) Then
                                        stmObjects = New ArrayList(Of COSObject)
                                        objToBeParsed.put(fileOffset, stmObjects)
                                    End If
                                    stmObjects.add(obj)
                                End If
                            End If
                        Else
                            ' NULL object
                            Dim pdfObject As COSObject = document.getObjectFromPool(objKey)
                            pdfObject.setObject(COSNull.NULL)
                        End If
                    End If
                    baseObj = toBeParsedList.poll()
                End While

                ' ---- read first COSObject with smallest offset;
                ' resulting object will be added to toBeParsedList
                If (objToBeParsed.isEmpty()) Then
                    Exit While
                End If

                For Each obj As COSObject In objToBeParsed.remove(objToBeParsed.firstKey())
                    Dim parsedObj As COSBase = parseObjectDynamically(obj, False)

                    obj.setObject(parsedObj)
                    addNewToList(toBeParsedList, parsedObj, addedObjects)

                    parsedObjects.add(getObjectId(obj))
                Next
            End While
        End Sub

        '/**
        ' * This will parse the next object from the stream and add it to the local
        ' * state. This is taken from {@link PDFParser} and reduced to parsing an
        ' * indirect object.
        ' * 
        ' * @param obj object to be parsed (we only take object number and generation
        ' *            number for lookup start offset)
        ' * @param requireExistingNotCompressedObj if <code>true</code> object to be
        ' *            parsed must not be contained within compressed stream
        ' * @return the parsed object (which is also added to document object)
        ' * 
        ' * @throws IOException If an IO error occurs.
        ' */
        Protected Function parseObjectDynamically(ByVal obj As COSObject, ByVal requireExistingNotCompressedObj As Boolean) As COSBase  'throws IOException
            Return parseObjectDynamically(obj.getObjectNumber().intValue(), obj.getGenerationNumber().intValue(), requireExistingNotCompressedObj)
        End Function

        '/**
        ' * This will parse the next object from the stream and add it to the local
        ' * state. This is taken from {@link PDFParser} and reduced to parsing an
        ' * indirect object.
        ' * 
        ' * @param objNr object number of object to be parsed
        ' * @param objGenNr object generation number of object to be parsed
        ' * @param requireExistingNotCompressedObj if <code>true</code> the object to
        ' *            be parsed must be defined in xref (comment: null objects may
        ' *            be missing from xref) and it must not be a compressed object
        ' *            within object stream (this is used to circumvent being stuck
        ' *            in a loop in a malicious PDF)
        ' * 
        ' * @return the parsed object (which is also added to document object)
        ' * 
        ' * @throws IOException If an IO error occurs.
        ' */
        Protected Function parseObjectDynamically(ByVal objNr As Integer, ByVal objGenNr As Integer, ByVal requireExistingNotCompressedObj As Boolean) As COSBase  'throws IOException
            ' ---- create object key and get object (container) from pool
            Dim objKey As New COSObjectKey(objNr, objGenNr)
            Dim pdfObject As COSObject = document.getObjectFromPool(objKey)

            If (pdfObject.getObject() Is Nothing) Then
                ' not previously parsed
                ' ---- read offset or object stream object number from xref table
                Dim offsetOrObjstmObNr As NLong = xrefTrailerResolver.getXrefTable().get(objKey)

                ' sanity test to circumvent loops with broken documents
                If (requireExistingNotCompressedObj AndAlso ((offsetOrObjstmObNr.HasValue = False) OrElse (offsetOrObjstmObNr <= 0))) Then
                    Throw New IOException("Object must be defined and must not be compressed object: " & objKey.getNumber() & ":" & objKey.getGeneration())
                End If

                If (offsetOrObjstmObNr.HasValue = False) Then
                    ' not defined object -> NULL object (Spec. 1.7, chap. 3.2.9)
                    pdfObject.setObject(COSNull.NULL)
                ElseIf (offsetOrObjstmObNr > 0) Then
                    ' offset of indirect object in file
                    ' ---- go to object start
                    setPdfSource(offsetOrObjstmObNr)

                    ' ---- we must have an indirect object
                    Dim readObjNr As Long = readObjectNumber()
                    Dim readObjGen As Long = readGenerationNumber()
                    readPattern(OBJ_MARKER)

                    ' ---- consistency check
                    If ((readObjNr <> objKey.getNumber()) OrElse (readObjGen <> objKey.getGeneration())) Then
                        Throw New IOException("XREF for " & objKey.getNumber() & ":" & objKey.getGeneration() & " points to wrong object: " & readObjNr & ":" & readObjGen)
                    End If

                    skipSpaces()
                    Dim pb As COSBase = parseDirObject()
                    Dim endObjectKey As String = readString()

                    If (endObjectKey.Equals("stream")) Then
                        pdfSource.unread(Sistema.Strings.GetBytes(endObjectKey, "ISO-8859-1"))
                        pdfSource.unread(" ")
                        If (TypeOf (pb) Is COSDictionary) Then
                            Dim stream As COSStream = parseCOSStream(pb, getDocument().getScratchFile())

                            If (securityHandler IsNot Nothing) Then
                                Try
                                    securityHandler.decryptStream(stream, objNr, objGenNr)
                                Catch ce As CryptographyException
                                    Throw New IOException("Error decrypting stream object " & objNr & ": " & ce.Message) '/* , ce // TODO: remove remark with Java 1.6 */);
                                End Try
                            End If
                            pb = stream
                        Else
                            ' this is not legal
                            ' the combination of a dict and the stream/endstream
                            ' forms a complete stream object
                            Throw New IOException("Stream not preceded by dictionary (offset: " & offsetOrObjstmObNr & ").")
                        End If
                        skipSpaces()
                        endObjectKey = readLine()

                        ' we have case with a second 'endstream' before endobj
                        If (Not endObjectKey.StartsWith("endobj")) Then
                            If (endObjectKey.StartsWith("endstream")) Then
                                endObjectKey = endObjectKey.Substring(9).Trim()
                                If (endObjectKey.Length() = 0) Then
                                    ' no other characters in extra endstream line
                                    endObjectKey = readLine() ' read next line
                                End If
                            End If
                        End If
                    ElseIf (securityHandler IsNot Nothing) Then
                        ' decrypt
                        If (TypeOf (pb) Is COSString) Then
                            decrypt(pb, objNr, objGenNr)
                        ElseIf (TypeOf (pb) Is COSDictionary) Then
                            For Each entry As Map.Entry(Of COSName, COSBase) In DirectCast(pb, COSDictionary).entrySet()
                                ' TODO: specially handle 'Contents' entry of
                                ' signature dictionary like in
                                ' SecurityHandler#decryptDictionary
                                If (TypeOf (entry.Value) Is COSString) Then
                                    decrypt(entry.Value, objNr, objGenNr)
                                End If
                            Next
                        ElseIf (TypeOf (pb) Is COSArray) Then
                            Dim array As COSArray = pb
                            Dim len As Integer = array.size()
                            For aIdx As Integer = 0 To len - 1
                                If (TypeOf (array.get(aIdx)) Is COSString) Then
                                    decrypt(array.get(aIdx), objNr, objGenNr)
                                End If
                            Next
                        End If
                    End If

                    pdfObject.setObject(pb)

                    If (Not endObjectKey.StartsWith("endobj")) Then
                        Throw New IOException("Object (" & readObjNr & ":" & readObjGen & ") at offset " & offsetOrObjstmObNr & " does not end with 'endobj'.")
                    End If

                    releasePdfSourceInputStream()
                Else
                    ' xref value is object nr of object stream containing object to
                    ' be parsed;
                    ' since our object was not found it means object stream was not
                    ' parsed so far
                    Dim objstmObjNr As Integer = (-offsetOrObjstmObNr)
                    Dim objstmBaseObj As COSBase = parseObjectDynamically(objstmObjNr, 0, True)
                    If (TypeOf (objstmBaseObj) Is COSStream) Then
                        ' parse object stream
                        Dim parser As New PDFObjectStreamParser(objstmBaseObj, document, forceParsing)
                        parser.parse()

                        ' get set of object numbers referenced for this object
                        ' stream
                        Dim refObjNrs As ICollection(Of NLong) = xrefTrailerResolver.getContainedObjectNumbers(objstmObjNr)

                        ' register all objects which are referenced to be contained
                        ' in object stream
                        For Each [next] As COSObject In parser.getObjects()
                            Dim stmObjKey As New COSObjectKey([next])
                            If (refObjNrs.contains(stmObjKey.getNumber())) Then
                                Dim stmObj As COSObject = document.getObjectFromPool(stmObjKey)
                                stmObj.setObject([next].getObject())
                            End If
                        Next
                    End If
                End If
            End If
            Return pdfObject.getObject()
        End Function

        '// ------------------------------------------------------------------------
        '/**
        ' * Decrypts given COSString.
        ' * 
        ' * @param str the string to be decrypted
        ' * @param objNr the object number
        ' * @param objGenNr the object generation number
        ' * @throws IOException ff something went wrong
        ' */
        Protected Sub decrypt(ByVal str As COSString, ByVal objNr As Long, ByVal objGenNr As Long) ' throws IOException
            Try
                securityHandler.decryptString(str, objNr, objGenNr)
            Catch ce As CryptographyException
                Throw New IOException("Error decrypting string: " & ce.Message) '/* , ce // TODO: remove remark with Java 1.6 */);
            End Try
        End Sub

        ' ------------------------------------------------------------------------
        Private inGetLength As Boolean = False

        '** Returns length value referred to or defined in given object. */
        Private Function getLength(ByVal lengthBaseObj As COSBase) As COSNumber 'throws IOException
            If (lengthBaseObj Is Nothing) Then
                Return Nothing
            End If

            If (inGetLength) Then
                Throw New IOException("Loop while reading length from " & lengthBaseObj.ToString)
            End If

            Dim retVal As COSNumber = Nothing

            Try
                inGetLength = True

                ' ---- maybe length was given directly
                If (TypeOf (lengthBaseObj) Is COSNumber) Then
                    retVal = lengthBaseObj
                    ' ---- length in referenced object
                ElseIf TypeOf (lengthBaseObj) Is COSObject Then
                    Dim lengthObj As COSObject = lengthBaseObj

                    If (lengthObj.getObject() Is Nothing) Then
                        ' not read so far

                        ' keep current stream position
                        Dim curFileOffset As Long = getPdfSourceOffset()
                        releasePdfSourceInputStream()

                        parseObjectDynamically(lengthObj, True)

                        ' reset current stream position
                        setPdfSource(curFileOffset)

                        If (lengthObj.getObject() Is Nothing) Then
                            Throw New IOException("Length object content was not read.")
                        End If
                    End If

                    If (Not (TypeOf (lengthObj.getObject()) Is COSNumber)) Then
                        Throw New IOException("Wrong type of referenced length object " & lengthObj.toString & ": " & lengthObj.getObject().GetType().Name)
                    End If

                    retVal = lengthObj.getObject()

                Else
                    Throw New IOException("Wrong type of length object: " & lengthBaseObj.GetType().Name())
                End If

            Finally
                inGetLength = False
            End Try
            Return retVal
        End Function

        ' ------------------------------------------------------------------------
        Private streamCopyBufLen As Integer = 8192
        Private streamCopyBuf() As Byte = Array.CreateInstance(GetType(Byte), streamCopyBufLen)

        '/**
        ' * This will read a COSStream from the input stream using length attribute
        ' * within dictionary. If length attribute is a indirect reference it is
        ' * first resolved to get the stream length. This means we copy stream data
        ' * without testing for 'endstream' or 'endobj' and thus it is no problem if
        ' * these keywords occur within stream. We require 'endstream' to be found
        ' * after stream data is read.
        ' * 
        ' * @param dic dictionary that goes with this stream.
        ' * @param file file to write the stream to when reading.
        ' * 
        ' * @return parsed pdf stream.
        ' * 
        ' * @throws IOException if an error occurred reading the stream, like
        ' *             problems with reading length attribute, stream does not end
        ' *             with 'endstream' after data read, stream too short etc.
        ' */
        Protected Overrides Function parseCOSStream(ByVal dic As COSDictionary, ByVal file As RandomAccess) As COSStream  'throws IOException
            Dim stream As New COSStream(dic, file)
            Dim out As OutputStream = Nothing
            Try
                readString() ' read 'stream'; this was already tested in
                ' parseObjectsDynamically()

                ' ---- skip whitespaces before start of data
                ' PDF Ref 1.7, chap. 3.2.7:
                ' 'stream' should be followed by either a CRLF (0x0d 0x0a) or LF
                ' but nothing else.
                '{
                Dim whitespace As Integer = pdfSource.read()

                ' see brother_scan_cover.pdf, it adds whitespaces
                ' after the stream but before the start of the
                ' data, so just read those first
                While (whitespace = &H20)
                    whitespace = pdfSource.read()
                End While

                If (whitespace = &HD) Then
                    whitespace = pdfSource.read()
                    If (whitespace <> &HA) Then
                        ' the spec says this is invalid but it happens in the
                        ' real
                        ' world so we must support it
                        pdfSource.unread(whitespace)
                    End If
                ElseIf (whitespace <> &HA) Then
                    ' no whitespace after 'stream'; PDF ref. says 'should' so
                    ' that is ok
                    pdfSource.unread(whitespace)
                End If
                '}

                '/*
                ' * This needs to be dic.getItem because when we are parsing, the underlying object might still be null.
                ' */
                Dim streamLengthObj As COSNumber = getLength(dic.getItem(COSName.LENGTH))
                If (streamLengthObj Is Nothing) Then
                    Throw New IOException("Missing length for stream.")
                End If

                ' ---- get output stream to copy data to
                out = stream.createFilteredStream(streamLengthObj)

                Dim remainBytes As Long = streamLengthObj.longValue()
                Dim bytesRead As Integer = 0
                Dim unexpectedEndOfStream As Boolean = False
                If (remainBytes = 35090) Then
                    ' TODO debug system out, to be removed??
                    Debug.Print("")
                End If
                While (remainBytes > 0)
                    Dim readBytes As Integer
                    readBytes = pdfSource.read(streamCopyBuf, 0, IIf(remainBytes > streamCopyBufLen, streamCopyBufLen, remainBytes))
                    If (readBytes <= 0) Then
                        ' throw new IOException(
                        ' "No more bytes from stream but expected: " + remainBytes
                        ' );
                        unexpectedEndOfStream = True
                        Exit While
                    End If
                    out.Write(streamCopyBuf, 0, readBytes)
                    remainBytes -= readBytes
                    bytesRead += readBytes
                End While
                If (unexpectedEndOfStream) Then
                    pdfSource.unread(bytesRead)
                    out = stream.createFilteredStream(streamLengthObj)
                    readUntilEndStream(out)
                End If
                Dim endStream As String = readString()
                If (Not endStream.Equals("endstream")) Then
                    Throw New IOException("Error reading stream using length value. Expected='endstream' actual='" & endStream & "' ")
                End If
            Finally
                If (out IsNot Nothing) Then
                    out.Close()
                End If
            End Try
            Return stream
        End Function

        Private Sub readUntilEndStream(ByVal out As OutputStream) ' throws IOException
            Dim bufSize As Integer
            Dim charMatchCount As Integer = 0
            Dim keyw() As Byte = ENDSTREAM
            Dim ch As Byte

            Dim quickTestOffset As Integer = 5 ' last character position of shortest keyword ('endobj')

            ' read next chunk into buffer; already matched chars are added to
            ' beginning of buffer
            While ((bufSize = pdfSource.read(streamCopyBuf, charMatchCount, streamCopyBufLen - charMatchCount)) > 0)
                bufSize += charMatchCount

                Dim bIdx As Integer = charMatchCount
                Dim quickTestIdx As Integer

                ' iterate over buffer, trying to find keyword match
                For maxQuicktestIdx As Integer = bufSize - quickTestOffset To bufSize - 1
                    '// reduce compare operations by first test last character we
                    '// would have to
                    '// match if current one matches; if it is not a character from
                    '// keywords
                    '// we can move behind the test character;
                    '// this shortcut is inspired by Boyer–Moore string search
                    '// algorithm
                    '// and can reduce parsing time by approx. 20%
                    quickTestIdx = bIdx + quickTestOffset
                    If ((charMatchCount = 0) AndAlso (quickTestIdx < maxQuicktestIdx)) Then
                        ch = streamCopyBuf(quickTestIdx)
                        If ((ch > Asc("t"c)) OrElse (ch < Asc("a"c))) Then
                            '// last character we would have to match if current
                            '// character would match
                            '// is not a character from keywords -> jump behind and
                            '// start over
                            bIdx = quickTestIdx
                            Continue For
                        End If
                    End If

                    ch = streamCopyBuf(bIdx) ' could be negative - but we only compare to ASCII

                    If (ch = keyw(charMatchCount)) Then
                        charMatchCount += 1
                        If (++charMatchCount = keyw.Length) Then
                            ' match found
                            bIdx += 1
                            Exit For
                        End If
                    Else
                        If ((charMatchCount = 3) AndAlso (ch = ENDOBJ(charMatchCount))) Then
                            ' maybe ENDSTREAM is missing but we could have ENDOBJ
                            keyw = ENDOBJ
                            charMatchCount += 1
                        Else
                            '// no match; incrementing match start by 1 would be dumb
                            '// since we already know matched chars
                            '// depending on current char read we may already have
                            '// beginning of a new match:
                            '// 'e': first char matched;
                            '// 'n': if we are at match position idx 7 we already
                            '// read 'e' thus 2 chars matched
                            '// for each other char we have to start matching first
                            '// keyword char beginning with next
                            '// read position
                            charMatchCount = IIf(ch = E, 1, IIf((ch = N) AndAlso (charMatchCount = 7), 2, 0))
                            ' search again for 'endstream'
                            keyw = ENDSTREAM
                        End If
                    End If
                Next

                Dim contentBytes As Integer = Math.Max(0, bIdx - charMatchCount)

                ' write buffer content until first matched char to output stream
                If (contentBytes > 0) Then
                    out.Write(streamCopyBuf, 0, contentBytes)
                End If
                If (charMatchCount = keyw.Length) Then
                    ' keyword matched; unread matched keyword (endstream/endobj)
                    ' and following buffered content
                    pdfSource.unread(streamCopyBuf, contentBytes, bufSize - contentBytes)
                    Exit While
                Else
                    ' copy matched chars at start of buffer
                    Array.Copy(keyw, 0, streamCopyBuf, 0, charMatchCount)
                End If

            End While
        End Sub

    End Class

End Namespace
Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.exceptions
Imports FinSeA.org.apache.pdfbox.io
Imports FinSeA.org.apache.pdfbox.pdmodel
Imports FinSeA.org.apache.pdfbox.persistence.util
Imports FinSeA.Text
Imports FinSeA.org.apache.pdfbox.pdmodel.fdf

Namespace org.apache.pdfbox.pdfparser

    '/**
    ' * This class will handle the parsing of the PDF document.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.53 $
    ' */
    Public Class PDFParser
        Inherits BaseParser

        '/**
        ' * Log instance.
        ' */
        'private static final Log LOG = LogFactory.getLog(PDFParser.class);

        Private Const SPACE_BYTE As Integer = 32

        Private Const PDF_HEADER As String = "%PDF-"
        Private Const FDF_HEADER As String = "%FDF-"

        Private Const PDF_DEFAULT_VERSION As String = "1.4"
        Private Const FDF_DEFAULT_VERSION As String = "1.0"

        '/**
        ' * A list of duplicate objects found when Parsing the PDF
        ' * File.
        ' */
        Private conflictList As New ArrayList(Of ConflictObj)

        '/** Collects all Xref/trailer objects and resolves them into single
        ' *  object using startxref reference. 
        ' */
        Protected xrefTrailerResolver As New XrefTrailerResolver()

        '/**
        ' * Temp file directory.
        ' */
        Private tempDirectory As FinSeA.Io.File = Nothing

        Private raf As RandomAccess = Nothing

        '/**
        ' * Constructor.
        ' *
        ' * @param input The input stream that contains the PDF document.
        ' *
        ' * @throws IOException If there is an error initializing the stream.
        ' */
        Public Sub New(ByVal input As Stream) ' throws IOException 
            Me.New(input, Nothing, FORCE_PARSING)
        End Sub

        '/**
        ' * Constructor to allow control over RandomAccessFile.
        ' * @param input The input stream that contains the PDF document.
        ' * @param rafi The RandomAccessFile to be used in internal COSDocument
        ' *
        ' * @throws IOException If there is an error initializing the stream.
        ' */
        Public Sub New(ByVal input As Stream, ByVal rafi As RandomAccess) ' throws IOException 
            Me.New(input, rafi, FORCE_PARSING)
        End Sub

        '/**
        ' * Constructor to allow control over RandomAccessFile.
        ' * Also enables parser to skip corrupt objects to try and force parsing
        ' * @param input The input stream that contains the PDF document.
        ' * @param rafi The RandomAccessFile to be used in internal COSDocument
        ' * @param force When true, the parser will skip corrupt pdf objects and
        ' * will continue parsing at the next object in the file
        ' *
        ' * @throws IOException If there is an error initializing the stream.
        ' */
        Public Sub New(ByVal input As Stream, ByVal rafi As RandomAccess, ByVal force As Boolean) ' throws IOException 
            MyBase.New(input, force)
            Me.raf = rafi
        End Sub

        '/**
        ' * This is the directory where pdfbox will create a temporary file
        ' * for storing pdf document stream in.  By default this directory will
        ' * be the value of the system property java.io.tmpdir.
        ' *
        ' * @param tmpDir The directory to create scratch files needed to store
        ' *        pdf document streams.
        ' */
        Public Sub setTempDirectory(ByVal tmpDir As FinSeA.Io.File)
            tempDirectory = tmpDir
        End Sub

        '/**
        ' * Returns true if parsing should be continued. By default, forceParsing is returned.
        ' * This can be overridden to add application specific handling (for example to stop
        ' * parsing when the number of exceptions thrown exceed a certain number).
        ' *
        ' * @param e The exception if vailable. Can be null if there is no exception available
        ' * @return true if parsing could be continued, otherwise false
        ' */
        Protected Function isContinueOnError(ByVal e As Exception) As Boolean
            Return forceParsing
        End Function

        '/**
        ' * This will parse the stream and populate the COSDocument object.  This will close
        ' * the stream when it is done parsing.
        ' *
        ' * @throws IOException If there is an error reading from the stream or corrupt data
        ' * is found.
        ' */
        Public Overridable Sub parse() 'throws IOException
            Try
                If (raf Is Nothing) Then
                    If (tempDirectory IsNot Nothing) Then
                        document = New COSDocument(tempDirectory)
                    Else
                        document = New COSDocument()
                    End If
                Else
                    document = New COSDocument(raf)
                End If

                setDocument(document)
                parseHeader()

                'Some PDF files have garbage between the header and the
                'first object
                skipToNextObj()

                Dim wasLastParsedObjectEOF As Boolean = False

                While (True)
                    If (pdfSource.isEOF()) Then
                        Exit While
                    End If

                    Try
                        ' don't reset flag to false if it is already true
                        wasLastParsedObjectEOF = wasLastParsedObjectEOF Or parseObject()
                    Catch e As IOException
                        '/*
                        ' * PDF files may have random data after the EOF marker. Ignore errors if
                        ' * last object processed is EOF.
                        ' */
                        If (wasLastParsedObjectEOF) Then
                            Exit While
                        End If
                        If (isContinueOnError(e)) Then
                            '/*
                            ' * Warning is sent to the PDFBox.log and to the Console that
                            ' * we skipped over an object
                            ' */
                            Debug.Print("Parsing Error, Skipping Object", e)

                            skipSpaces()
                            Dim lastOffset As Long = pdfSource.getOffset()
                            skipToNextObj()

                            '/* the nextObject is the one we want to skip 
                            ' * so read the 'Object Number' without interpret it
                            ' * in order to force the skipObject
                            ' */
                            If (lastOffset = pdfSource.getOffset()) Then
                                readStringNumber()
                                skipToNextObj()
                            End If
                        Else
                            Throw e
                        End If

                    End Try
                    skipSpaces()
                End While

                ' set xref to start with
                xrefTrailerResolver.setStartxref(document.getStartXref())

                ' get resolved xref table + trailer
                document.setTrailer(xrefTrailerResolver.getTrailer())
                document.addXRefTable(xrefTrailerResolver.getXrefTable())

                If (Not document.isEncrypted()) Then
                    document.dereferenceObjectStreams()
                End If
                ConflictObj.resolveConflicts(document, conflictList)
            Catch t As Exception
                'so if the PDF is corrupt then close the document and clear
                'all resources to it
                If (document IsNot Nothing) Then
                    document.close()
                    document = Nothing
                End If
                If (TypeOf (t) Is IOException) Then
                    Throw t
                Else
                    Throw New WrappedIOException(t)
                End If
            Finally
                pdfSource.Close()
            End Try
        End Sub

        '/**
        ' * Skip to the start of the next object.  This is used to recover
        ' * from a corrupt object. This should handle all cases that parseObject
        ' * supports. This assumes that the next object will
        ' * start on its own line.
        ' *
        ' * @throws IOException
        ' */
        Private Sub skipToNextObj() 'throws IOException
            Dim b() As Byte = Array.CreateInstance(GetType(Byte), 16)
            Dim p As Pattern = Pattern.compile("\\d+\\s+\\d+\\s+obj.*", Pattern.DOTALL)
            '/* Read a buffer of data each time to see if it starts with a
            ' * known keyword. This is not the most efficient design, but we should
            ' * rarely be needing this function. We could update this to use the
            ' * circular buffer, like in readUntilEndStream().
            ' */
            While (Not pdfSource.isEOF())
                Dim l As Integer = pdfSource.read(b)
                If (l < 1) Then
                    Exit While
                End If
                Dim s As String = Sistema.Strings.GetString(b, "US-ASCII")
                If (s.StartsWith("trailer") OrElse _
                        s.StartsWith("xref") OrElse _
                        s.StartsWith("startxref") OrElse _
                        s.StartsWith("stream") OrElse _
                        p.matcher(s).matches()) Then
                    pdfSource.unread(b)
                    Exit While
                Else
                    pdfSource.unread(b, 1, l - 1)
                End If
            End While
        End Sub

        Private Sub parseHeader() 'throws IOException
            ' read first line
            Dim header As String = readLine()
            ' some pdf-documents are broken and the pdf-version is in one of the following lines
            If ((header.IndexOf(PDF_HEADER) = -1) AndAlso (header.IndexOf(FDF_HEADER) = -1)) Then
                header = readLine()
                While ((header.IndexOf(PDF_HEADER) = -1) AndAlso (header.IndexOf(FDF_HEADER) = -1))
                    ' if a line starts with a digit, it has to be the first one with data in it
                    If ((header.Length() > 0) AndAlso (NChar.isDigit(header.Chars(0)))) Then
                        Exit While
                    End If
                    header = readLine()
                End While
            End If

            ' nothing found
            If ((header.IndexOf(PDF_HEADER) = -1) AndAlso (header.IndexOf(FDF_HEADER) = -1)) Then
                Throw New IOException("Error: Header doesn't contain versioninfo")
            End If

            'sometimes there are some garbage bytes in the header before the header
            'actually starts, so lets try to find the header first.
            Dim headerStart As Integer = header.IndexOf(PDF_HEADER)
            If (headerStart = -1) Then
                headerStart = header.IndexOf(FDF_HEADER)
            End If

            'greater than zero because if it is zero then
            'there is no point of trimming
            If (headerStart > 0) Then
                'trim off any leading characters
                header = header.Substring(headerStart, header.Length())
            End If

            '/*
            ' * This is used if there is garbage after the header on the same line
            ' */
            If (header.StartsWith(PDF_HEADER)) Then
                If (Not Sistema.Strings.Metches(header, PDF_HEADER & "\d.\d")) Then
                    If (header.Length() < PDF_HEADER.Length() + 3) Then
                        ' No version number at all, set to 1.4 as default
                        header = PDF_HEADER + PDF_DEFAULT_VERSION
                        Debug.Print("No pdf version found, set to " & PDF_DEFAULT_VERSION & " as default.")
                    Else
                        Dim headerGarbage As String = header.Substring(PDF_HEADER.Length() + 3, header.Length()) & vbLf ' "\n"
                        header = header.Substring(0, PDF_HEADER.Length() + 3)
                        pdfSource.unread(Sistema.Strings.GetBytes(headerGarbage, "ISO-8859-1"))
                    End If
                End If
            Else
                If (Not Sistema.Strings.Metches(header, FDF_HEADER & "\d.\d")) Then
                    If (header.Length() < FDF_HEADER.Length() + 3) Then
                        ' No version number at all, set to 1.0 as default
                        header = FDF_HEADER & FDF_DEFAULT_VERSION
                        Debug.Print("No fdf version found, set to " & FDF_DEFAULT_VERSION & " as default.")
                    Else
                        Dim headerGarbage As String = header.Substring(FDF_HEADER.Length() + 3, header.Length()) & vbLf '"\n"
                        header = header.Substring(0, FDF_HEADER.Length() + 3)
                        pdfSource.unread(Sistema.Strings.GetBytes(headerGarbage, "ISO-8859-1"))
                    End If
                End If
            End If
            document.setHeaderString(header)

            Try
                If (header.StartsWith(PDF_HEADER)) Then
                    Dim pdfVersion As Single = Single.Parse(header.Substring(PDF_HEADER.Length(), Math.Min(header.Length(), PDF_HEADER.Length() + 3)))
                    document.setVersion(pdfVersion)
                Else
                    Dim pdfVersion As Single = Single.Parse(header.Substring(FDF_HEADER.Length(), Math.Min(header.Length(), FDF_HEADER.Length() + 3)))
                    document.setVersion(pdfVersion)
                End If
            Catch e As FormatException
                Throw New IOException("Error getting pdf version:" & e.ToString)
            End Try
        End Sub

        '/**
        ' * This will get the document that was parsed.  parse() must be called before this is called.
        ' * When you are done with this document you must call close() on it to release
        ' * resources.
        ' *
        ' * @return The document that was parsed.
        ' *
        ' * @throws IOException If there is an error getting the document.
        ' */
        Public Function getDocument() As COSDocument 'throws IOException
            If (document Is Nothing) Then
                Throw New IOException("You must call parse() before calling getDocument()")
            End If
            Return document
        End Function

        '/**
        ' * This will get the PD document that was parsed.  When you are done with
        ' * this document you must call close() on it to release resources.
        ' *
        ' * @return The document at the PD layer.
        ' *
        ' * @throws IOException If there is an error getting the document.
        ' */
        Public Overridable Function getPDDocument() As PDDocument ' throws IOException
            Return New PDDocument(getDocument())
        End Function

        '/**
        ' * This will get the FDF document that was parsed.  When you are done with
        ' * this document you must call close() on it to release resources.
        ' *
        ' * @return The document at the PD layer.
        ' *
        ' * @throws IOException If there is an error getting the document.
        ' */
        Public Function getFDFDocument() As FDFDocument ' throws IOException
            Return New FDFDocument(getDocument())
        End Function

        '/**
        ' * This will parse the next object from the stream and add it to
        ' * the local state.
        ' *
        ' * @return Returns true if the processed object had an endOfFile marker
        ' *
        ' * @throws IOException If an IO error occurs.
        ' */
        Private Function parseObject() As Boolean 'throws IOException
            Dim currentObjByteOffset As Long = pdfSource.getOffset()
            Dim isEndOfFile As Boolean = False
            skipSpaces()
            'peek at the next character to determine the type of object we are parsing
            Dim peekedChar As Char = Convert.ToChar(pdfSource.peek())

            'ignore endobj and endstream sections.
            While (peekedChar = "e"c)
                'there are times when there are multiple endobj, so lets
                'just read them and move on.
                readString()
                skipSpaces()
                currentObjByteOffset = pdfSource.getOffset()
                peekedChar = Convert.ToChar(pdfSource.peek())
            End While

            If (pdfSource.isEOF()) Then
                '  //"Skipping because of EOF" );
                '   //end of file we will return a false and call it a day.
            ElseIf (peekedChar = "x"c) Then '//xref table. Note: The contents of the Xref table are currently ignored
                parseXrefTable(currentObjByteOffset)
            ElseIf (peekedChar = "t"c OrElse peekedChar = "s"c) Then '// Note: startxref can occur in either a trailer section or by itself
                If (peekedChar = "t"c) Then
                    parseTrailer()
                    peekedChar = Convert.ToChar(pdfSource.peek())
                End If
                If (peekedChar = "s"c) Then
                    parseStartXref()
                    ' readString() calls skipSpaces() will skip comments... that's
                    ' bad for us b/c the %%EOF flag is a comment
                    While (isWhitespace(pdfSource.peek()) AndAlso Not pdfSource.isEOF())
                        pdfSource.read() ' // read (get rid of) all the whitespace
                    End While
                    Dim eof As String = ""
                    If (Not pdfSource.isEOF()) Then
                        eof = readLine() ' if there's more data to read, get the EOF flag
                    End If

                    ' verify that EOF exists (see PDFBOX-979 for documentation on special cases)
                    If (Not "%%EOF".Equals(eof)) Then
                        If (eof.StartsWith("%%EOF")) Then
                            ' content after marker -> unread with first space byte for read newline
                            pdfSource.unread(SPACE_BYTE) ' we read a whole line; add space as newline replacement
                            pdfSource.unread(Sistema.Strings.GetBytes(eof.Substring(5), "ISO-8859-1"))
                        Else
                            ' PDF does not conform to spec, we should warn someone
                            Debug.Print("expected='%%EOF' actual='" & eof & "'")
                            ' if we're not at the end of a file, just put it back and move on
                            If (Not pdfSource.isEOF()) Then
                                pdfSource.unread(SPACE_BYTE) ' we read a whole line; add space as newline replacement
                                pdfSource.unread(Sistema.Strings.GetBytes(eof, "ISO-8859-1"))
                            End If
                        End If
                    End If
                    isEndOfFile = True
                End If
            Else 'we are going to parse an normal object
                Dim number As Long = -1
                Dim genNum As Integer = -1
                Dim objectKey As String = ""
                Dim missingObjectNumber As Boolean = False
                Try
                    Dim peeked As Char = Convert.ToChar(pdfSource.peek())
                    If (peeked = "<"c) Then
                        missingObjectNumber = True
                    Else
                        number = readObjectNumber()
                    End If
                Catch e As IOException
                    'ok for some reason "GNU Ghostscript 5.10" puts two endobj
                    'statements after an object, of course this is nonsense
                    'but because we want to support as many PDFs as possible
                    'we will simply try again
                    number = readObjectNumber()
                End Try
                If (Not missingObjectNumber) Then
                    skipSpaces()
                    genNum = readGenerationNumber()

                    objectKey = readString(3)
                    'System.out.println( "parseObject() num=" + number +
                    '" genNumber=" + genNum + " key='" + objectKey + "'" );
                    If (Not objectKey.Equals("obj")) Then
                        If (Not isContinueOnError(Nothing) OrElse Not objectKey.Equals("o")) Then
                            Throw New IOException("expected='obj' actual='" & objectKey & "' " & pdfSource.ToString)
                        End If
                        'assume that "o" was meant to be "obj" (this is a workaround for
                        ' PDFBOX-773 attached PDF Andersens_Fairy_Tales.pdf).
                    End If
                Else
                    number = -1
                    genNum = -1
                End If

                skipSpaces()
                Dim pb As COSBase = parseDirObject()
                Dim endObjectKey As String = readString()

                If (endObjectKey.Equals("stream")) Then
                    pdfSource.unread(Sistema.Strings.GetBytes(endObjectKey, "ISO-8859-1"))
                    pdfSource.unread(" ")
                    If (TypeOf (pb) Is COSDictionary) Then
                        pb = parseCOSStream(pb, getDocument().getScratchFile())

                        ' test for XRef type
                        Dim strmObj As COSStream = pb
                        Dim objectType As COSName = strmObj.getItem(COSName.TYPE)
                        If (objectType IsNot Nothing AndAlso objectType.equals(COSName.XREF)) Then
                            ' XRef stream
                            parseXrefStream(strmObj, currentObjByteOffset)
                        End If
                    Else
                        ' this is not legal
                        ' the combination of a dict and the stream/endstream forms a complete stream object
                        Throw New IOException("stream not preceded by dictionary")
                    End If
                    skipSpaces()
                    endObjectKey = readLine()
                End If

                Dim key As New COSObjectKey(number, genNum)
                Dim pdfObject As COSObject = document.getObjectFromPool(key)
                If (pdfObject.getObject() Is Nothing) Then
                    pdfObject.setObject(pb)
                    '/*
                    ' * If the object we returned already has a baseobject, then we have a conflict
                    ' * which we will resolve using information after we parse the xref table.
                    ' */
                Else
                    addObjectToConflicts(currentObjByteOffset, key, pb)
                End If

                If (Not endObjectKey.Equals("endobj")) Then
                    If (endObjectKey.StartsWith("endobj")) Then
                        '/*
                        ' * Some PDF files don't contain a new line after endobj so we
                        ' * need to make sure that the next object number is getting read separately
                        ' * and not part of the endobj keyword. Ex. Some files would have "endobj28"
                        ' * instead of "endobj"
                        ' */
                        pdfSource.unread(SPACE_BYTE) ' add a space first in place of the newline consumed by readline()
                        pdfSource.unread(Sistema.Strings.GetBytes(endObjectKey.Substring(6), "ISO-8859-1"))
                    ElseIf (endObjectKey.Trim().EndsWith("endobj")) Then
                        '/*
                        ' * Some PDF files contain junk (like ">> ", in the case of a PDF
                        ' * I found which was created by Exstream Dialogue Version 5.0.039)
                        ' * in which case we ignore the data before endobj and just move on
                        ' */
                        LOG.warn("expected='endobj' actual='" & endObjectKey & "' ")
                    ElseIf (Not pdfSource.isEOF()) Then
                        'It is possible that the endobj is missing, there
                        'are several PDFs out there that do that so. Unread
                        'and assume that endobj was missing
                        pdfSource.unread(SPACE_BYTE) ' add a space first in place of the newline consumed by readline()
                        pdfSource.unread(Sistema.Strings.GetBytes(endObjectKey, "ISO-8859-1"))
                    End If
                End If
                skipSpaces()
            End If

            Return isEndOfFile
        End Function

        '/**
        ' * Adds a new ConflictObj to the conflictList.
        ' * @param offset the offset of the ConflictObj
        ' * @param key The COSObjectKey of this object
        ' * @param pb The COSBase of this conflictObj
        ' * @throws IOException
        ' */
        Private Sub addObjectToConflicts(ByVal offset As Long, ByVal key As COSObjectKey, ByVal pb As COSBase) ' throws IOException
            Dim obj As New COSObject(Nothing)
            obj.setObjectNumber(COSInteger.get(key.getNumber()))
            obj.setGenerationNumber(COSInteger.get(key.getGeneration()))
            obj.setObject(pb)
            Dim conflictObj As New ConflictObj(offset, key, obj)
            conflictList.Add(conflictObj)
        End Sub

        '/**
        ' * This will parse the startxref section from the stream.
        ' * The startxref value is ignored.
        ' *
        ' * @return false on parsing error
        ' * @throws IOException If an IO error occurs.
        ' */
        Protected Function parseStartXref() As Boolean ' throws IOException
            If (pdfSource.peek() <> Asc("s"c)) Then
                Return False
            End If
            Dim startXRef As String = readString()
            If (Not startXRef.Trim().Equals("startxref")) Then
                Return False
            End If
            skipSpaces()
            '/* This integer is the byte offset of the first object referenced by the xref or xref stream
            ' * Needed for the incremental update (PREV)
            ' */
            getDocument().setStartXref(readLong())
            Return True
        End Function


        '/**
        ' * This will parse the xref table from the stream and add it to the state
        ' * The XrefTable contents are ignored.
        ' * @param startByteOffset the offset to start at
        ' * @return false on parsing error
        ' * @throws IOException If an IO error occurs.
        ' */
        Protected Function parseXrefTable(ByVal startByteOffset As Long) As Boolean 'throws IOException
            If (pdfSource.peek() <> Asc("x"c)) Then
                Return False
            End If
            Dim xref As String = readString()
            If (Not xref.Trim().Equals("xref")) Then
                Return False
            End If

            ' signal start of new XRef
            xrefTrailerResolver.nextXrefObj(startByteOffset)

            '/*
            ' * Xref tables can have multiple sections.
            ' * Each starts with a starting object id and a count.
            ' */
            While (True)
                Dim currObjID As Long = readObjectNumber() ' first obj id
                Dim count As Long = readLong() ' the number of objects in the xref table
                skipSpaces()
                For i As Integer = 0 To count - 1
                    If (pdfSource.isEOF() OrElse isEndOfName(Convert.ToChar(pdfSource.peek()))) Then
                        Exit While
                    End If
                    If (pdfSource.peek() = Asc("t"c)) Then
                        Exit While
                    End If
                    'Ignore table contents
                    Dim currentLine As String = readLine()
                    Dim splitString As String() = currentLine.Split(" ")
                    If (splitString.Length < 3) Then
                        LOG.warn("invalid xref line: " & currentLine)
                        Exit While
                    End If
                    '/* This supports the corrupt table as reported in
                    '* PDFBOX-474 (XXXX XXX XX n) */
                    If (splitString(splitString.Length - 1).Equals("n")) Then
                        Try
                            Dim currOffset As Long = Long.Parse(splitString(0))
                            Dim currGenID As Integer = Integer.Parse(splitString(1))
                            Dim objKey As New COSObjectKey(currObjID, currGenID)
                            xrefTrailerResolver.setXRef(objKey, currOffset)
                        Catch e As FormatException
                            Throw New IOException(e.Message)
                        End Try
                    ElseIf (Not splitString(2).Equals("f")) Then
                        Throw New IOException("Corrupt XRefTable Entry - ObjID:" & currObjID)
                    End If
                    currObjID += 1
                    skipSpaces()
                Next
                skipSpaces()
                Dim c As Char = Convert.ToChar(pdfSource.peek())
                If (c < "0"c OrElse c > "9"c) Then
                    Exit While
                End If
            End While
            Return True
        End Function

        '/**
        ' * This will parse the trailer from the stream and add it to the state.
        ' *
        ' * @return false on parsing error
        ' * @throws IOException If an IO error occurs.
        ' */
        Protected Function parseTrailer() As Boolean ' throws IOException
            If (pdfSource.peek() <> Asc("t"c)) Then
                Return False
            End If
            'read "trailer"
            Dim nextLine As String = readLine()
            If (Not nextLine.Trim().Equals("trailer")) Then
                '// in some cases the EOL is missing and the trailer immediately
                '// continues with "<<" or with a blank character
                '// even if this does not comply with PDF reference we want to support as many PDFs as possible
                '// Acrobat reader can also deal with Me.
                If (nextLine.StartsWith("trailer")) Then
                    Dim b() As Byte = Sistema.Strings.GetBytes(nextLine, "ISO-8859-1")
                    Dim len As Integer = "trailer".Length()
                    pdfSource.unread(vbLf) '\n
                    pdfSource.unread(b, len, b.Length - len)
                Else
                    Return False
                End If
            End If

            ' in some cases the EOL is missing and the trailer continues with " <<"
            ' even if this does not comply with PDF reference we want to support as many PDFs as possible
            ' Acrobat reader can also deal with Me.
            skipSpaces()

            Dim parsedTrailer As COSDictionary = parseCOSDictionary()
            xrefTrailerResolver.setTrailer(parsedTrailer)

            ' The version can also be specified within the document /Catalog
            readVersionInTrailer(parsedTrailer)

            skipSpaces()
            Return True
        End Function

        '/**
        ' * The document catalog can also have a /Version parameter which overrides the version specified
        ' * in the header if, and only if it is greater.
        ' *
        ' * @param parsedTrailer the parsed catalog in the trailer
        ' */
        Private Sub readVersionInTrailer(ByVal parsedTrailer As COSDictionary)
            Dim root As COSObject = parsedTrailer.getItem(COSName.ROOT)
            If (root IsNot Nothing) Then
                Dim version As COSName = root.getItem(COSName.VERSION)
                If (version IsNot Nothing) Then
                    Dim trailerVersion As Single = Single.Parse(version.getName())
                    If (trailerVersion > document.getVersion()) Then
                        document.setVersion(trailerVersion)
                    End If
                End If
            End If
        End Sub

        '/**
        ' * Fills XRefTrailerResolver with data of given stream.
        ' * Stream must be of type XRef.
        ' * @param stream the stream to be read
        ' * @param objByteOffset the offset to start at
        ' * @throws IOException if there is an error parsing the stream
        ' */
        Public Sub parseXrefStream(ByVal stream As COSStream, ByVal objByteOffset As Long) 'throws IOException
            xrefTrailerResolver.nextXrefObj(objByteOffset)
            xrefTrailerResolver.setTrailer(stream)
            Dim parser As New PDFXrefStreamParser(stream, document, forceParsing, xrefTrailerResolver)
            parser.parse()
        End Sub

        '/**
        ' * Used to resolve conflicts when a PDF Document has multiple objects with
        ' * the same id number. Ideally, we could use the Xref table when parsing
        ' * the document to be able to determine which of the objects with the same ID
        ' * is correct, but we do not have access to the Xref Table during parsing.
        ' * Instead, we queue up the conflicts and resolve them after the Xref has
        ' * been parsed. The Objects listed in the Xref Table are kept and the
        ' * others are ignored.
        ' */
        Private Class ConflictObj

            Private offset As Long
            Private objectKey As COSObjectKey
            Private [object] As COSObject

            Public Sub New(ByVal offsetValue As Long, ByVal key As COSObjectKey, ByVal pdfObject As COSObject)
                Me.offset = offsetValue
                Me.objectKey = key
                Me.[object] = pdfObject
            End Sub

            Public Overrides Function toString() As String
                Return "Object(" & offset & ", " & objectKey.toString & ")"
            End Function

            '/**
            ' * Sometimes pdf files have objects with the same ID number yet are
            ' * not referenced by the Xref table and therefore should be excluded.
            ' * This method goes through the conflicts list and replaces the object stored
            ' * in the objects array with this one if it is referenced by the xref
            ' * table.
            ' * @throws IOException
            ' */
            Friend Shared Sub resolveConflicts(ByVal document As COSDocument, ByVal conflictList As List(Of ConflictObj)) ' throws IOException
                Dim conflicts As Iterator(Of ConflictObj) = conflictList.iterator()
                If (conflicts.hasNext()) Then
                    Dim values As CCollection(Of Long) = document.getXrefTable().Values()
                    Do
                        Dim o As ConflictObj = conflicts.next()
                        Dim offset As Long = o.offset
                        If (tolerantConflicResolver(values, offset, 4)) Then
                            Dim pdfObject As COSObject = document.getObjectFromPool(o.objectKey)
                            If (pdfObject.getObjectNumber() IsNot Nothing AndAlso pdfObject.getObjectNumber().equals(o.object.getObjectNumber())) Then
                                pdfObject.setObject(o.object.getObject())
                            Else
                                LOG.debug("Conflict object [" & o.objectKey.toString & "] at offset " & offset & " found in the xref table, but the object numbers differ. Ignoring this object. The document is maybe malformed.")
                            End If
                        End If
                    Loop While (conflicts.hasNext())
                End If
            End Sub
        End Class

        '/**
        ' * Check if the given object offset can be find in the xref table. If not, we try to search the table
        ' * again with the given tolerance and check the given bytes before and after the xref table offset.
        ' *
        ' * @param values are the unsorted values from the xref table
        ' * @param offset is the offset that should be found in the xref table
        ' * @param tolerance is the allowed tolerance in bytes.
        ' * @return true if the offset was found inside the xref table
        ' */
        Friend Shared Function tolerantConflicResolver(ByVal values As CCollection(Of Long), ByVal offset As Long, ByVal tolerance As Integer) As Boolean
            If (values.Contains(offset)) Then
                Return True
            Else
                For Each [integer] As Long In values
                    If (Math.Abs([integer] - offset) <= tolerance) Then
                        Return True
                    End If
                Next
            End If
            Return False
        End Function

    End Class

End Namespace

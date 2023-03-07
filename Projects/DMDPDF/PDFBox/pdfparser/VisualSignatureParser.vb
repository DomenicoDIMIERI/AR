Imports System.IO
Imports FinSeA.org.apache.pdfbox.pdfwriter
Imports FinSeA.org.apache.pdfbox.persistence.util
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.Io
Imports FinSeA.Text

Namespace org.apache.pdfbox.pdfparser


    Public Class VisualSignatureParser
        Inherits BaseParser

        '/**
        ' * Log instance.
        ' */
        'private static final Log LOG = LogFactory.getLog(PDFParser.class);

        '/**
        ' * Constructor.
        ' * 
        ' * @param input the inputstream to be read.
        ' * 
        ' * @throws IOException If something went wrong
        ' */
        Public Sub New(ByVal input As Stream) ' throws IOException 
            MyBase.New(input)
        End Sub

        Public Sub parse() 'throws IOException 
            document = New COSDocument()
            skipToNextObj()

            Dim wasLastParsedObjectEOF As Boolean = False
            Try
                While (Not wasLastParsedObjectEOF)
                    If (pdfSource.isEOF()) Then
                        Exit While
                    End If
                    Try
                        wasLastParsedObjectEOF = parseObject()
                    Catch e As IOException
                        '               /*
                        '* Warning is sent to the PDFBox.log and to the Console that
                        '* we skipped over an object
                        '*/
                        LOG.warn("Parsing Error, Skipping Object", e)
                        skipToNextObj()
                    End Try
                    skipSpaces()
                End While
            Catch e As IOException
                '/*
                ' * PDF files may have random data after the EOF marker. Ignore errors if
                ' * last object processed is EOF.
                ' */
                If (Not wasLastParsedObjectEOF) Then
                    Throw e
                End If
            End Try
        End Sub

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
                If (s.StartsWith("trailer") _
                    OrElse s.StartsWith("xref") _
                    OrElse s.StartsWith("startxref") _
                    OrElse s.StartsWith("stream") _
                    OrElse p.matcher(s).matches()) Then
                    pdfSource.unread(b)
                    Exit While
                Else
                    pdfSource.unread(b, 1, l - 1)
                End If
            End While
        End Sub

        Private Function parseObject() As Boolean ' throws IOException 
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
                peekedChar = Convert.ToChar(pdfSource.peek())
            End While
            If (pdfSource.isEOF()) Then
                ' end of file we will return a false and call it a day.
            ElseIf (peekedChar = "x"c) Then
                'xref table. Note: The contents of the Xref table are currently ignored
                Return True
            ElseIf (peekedChar = "t"c OrElse peekedChar = "s"c) Then
                ' Note: startxref can occur in either a trailer section or by itself
                If (peekedChar = "t"c) Then
                    Return True
                End If
                If (peekedChar = "s"c) Then
                    skipToNextObj()
                    'verify that EOF exists
                    Dim eof As String = readExpectedString("%%EOF")
                    If (eof.IndexOf("%%EOF") = -1 AndAlso Not pdfSource.isEOF()) Then
                        Throw New IOException("expected='%%EOF' actual='" & eof & "' next=" & readString() & " next=" & readString())
                    End If
                    isEndOfFile = True
                End If
            Else
                'we are going to parse an normal object
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
                        Throw New IOException("expected='obj' actual='" & objectKey.ToString & "' " & pdfSource.ToString)
                    End If
                Else
                    number = -1
                    genNum = -1
                End If

                skipSpaces()
                Dim pb As COSBase = parseDirObject()
                Dim endObjectKey As String = readString()

                If (endObjectKey.Equals("stream")) Then
                    pdfSource.unread(Sistema.Strings.GetBytes(endObjectKey))
                    pdfSource.unread(" ")
                    If (TypeOf (pb) Is COSDictionary) Then
                        pb = parseCOSStream(pb, getDocument().getScratchFile())
                    Else
                        ' this is not legal
                        ' the combination of a dict and the stream/endstream forms a complete stream object
                        Throw New IOException("stream not preceded by dictionary")
                    End If
                    endObjectKey = readString()
                End If

                Dim key As New COSObjectKey(number, genNum)
                Dim pdfObject As COSObject = document.getObjectFromPool(key)
                pb.setNeedToBeUpdate(True)
                pdfObject.setObject(pb)

                If (Not endObjectKey.Equals("endobj")) Then
                    If (endObjectKey.StartsWith("endobj")) Then
                        '/*
                        ' * Some PDF files don't contain a new line after endobj so we
                        ' * need to make sure that the next object number is getting read separately
                        ' * and not part of the endobj keyword. Ex. Some files would have "endobj28"
                        ' * instead of "endobj"
                        ' */
                        pdfSource.unread(Sistema.Strings.GetBytes(endObjectKey.Substring(6)))
                    ElseIf (Not pdfSource.isEOF()) Then
                        Try
                            '//It is possible that the endobj  is missing, there
                            '//are several PDFs out there that do that so skip it and move on.
                            Single.Parse(endObjectKey)
                            pdfSource.unread(COSWriter.SPACE)
                            pdfSource.unread(Sistema.Strings.GetBytes(endObjectKey))
                        Catch e As FormatException
                            '//we will try again incase there was some garbage which
                            '//some writers will leave behind.
                            Dim secondEndObjectKey As String = readString()
                            If (Not secondEndObjectKey.Equals("endobj")) Then
                                If (isClosing()) Then
                                    '//found a case with 17506.pdf object 41 that was like this
                                    '//41 0 obj [/Pattern /DeviceGray] ] endobj
                                    '//notice the second array close, here we are reading it
                                    '//and ignoring and attempting to continue
                                    pdfSource.read()
                                End If
                                skipSpaces()
                                Dim thirdPossibleEndObj As String = readString()
                                If (Not thirdPossibleEndObj.Equals("endobj")) Then
                                    Throw New IOException("expected='endobj' firstReadAttempt='" & endObjectKey.ToString & "' secondReadAttempt='" & secondEndObjectKey.ToCharArray & "' " & pdfSource.ToString)
                                End If
                            End If
                        End Try
                    End If
                End If
                skipSpaces()
            End If

            Return isEndOfFile
        End Function

        '/**
        ' * Returns the underlying COSDocument.
        ' * 
        ' * @return the COSDocument
        ' * 
        ' * @throws IOException If something went wrong
        ' */
        Public Function getDocument() As COSDocument ' throws IOException 
            If (document Is Nothing) Then
                Throw New IOException("You must call parse() before calling getDocument()")
            End If
            Return document
        End Function

    End Class

End Namespace

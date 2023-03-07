Imports System.IO
Imports System.Text
Imports FinSeA.Text
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.exceptions
Imports FinSeA.org.apache.pdfbox.pdmodel
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.pdmodel.interactive.documentnavigation.outline
Imports FinSeA.org.apache.pdfbox.pdmodel.interactive.pagenavigation

Namespace org.apache.pdfbox.util

    '/**
    ' * This class will take a pdf document and strip out all of the text and ignore the
    ' * formatting and such.  Please note; it is up to clients of this class to verify that
    ' * a specific user has the correct permissions to extract text from the
    ' * PDF document.
    ' * 
    ' * The basic flow of this process is that we get a document and use a series of 
    ' * processXXX() functions that work on smaller and smaller chunks of the page.  
    ' * Eventually, we fully process each page and then print it. 
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * 
    ' */
    Public Class PDFTextStripper
        Inherits PDFStreamEngine

        Private Shared ReadOnly thisClassName As String = GetType(PDFTextStripper).Name.ToLower

        Private Shared ReadOnly DEFAULT_INDENT_THRESHOLD As Single = My.Settings.PDFTextStripper_indent ' 2.0F
        Private Shared ReadOnly DEFAULT_DROP_THRESHOLD As Single = My.Settings.PDFTextStripper_drop ' 2.5F

        '    '//enable the ability to set the default indent/drop thresholds
        '    '//with -D system properties:
        '    '//    pdftextstripper.indent
        '    '//    pdftextstripper.drop
        '    Shared Sub New()
        '        Dim prop As String = thisClassName & ".indent"
        '        Dim s As String = System.getProperty(prop)
        '        If (s <> "" AndAlso s.Length() > 0) Then
        '            Try
        '                Dim f As Single = Single.Parse(s)
        '                DEFAULT_INDENT_THRESHOLD = f
        '            Catch nfe As NumberFormatException
        '                    //ignore and use default
        '            End Try
        '        End If
        '    prop = thisClassName+".drop";
        '    s = System.getProperty(prop);
        '    if(s!=null && s.length()>0)
        '    {
        '            Try
        '        {
        '            Single f = NFloat.parseFloat(s);
        '            DEFAULT_DROP_THRESHOLD = f;
        '        }
        '        catch(NumberFormatException nfe)
        '        {
        '            //ignore and use default
        '        }
        '    }
        '}

        '/**
        ' * The platforms line separator.
        ' */
        Protected systemLineSeparator As String = Environment.NewLine   'System.getProperty("line.separator"); 

        Private lineSeparator As String = systemLineSeparator
        Private pageSeparator As String = systemLineSeparator
        Private _wordSeparator As String = " "
        Private paragraphStart As String = ""
        Private paragraphEnd As String = ""
        Private pageStart As String = ""
        Private pageEnd As String = pageSeparator
        Private articleStart As String = ""
        Private articleEnd As String = ""

        Private currentPageNo As Integer = 0
        Private _startPage As Integer = 1
        Private _endPage As Integer = Integer.MaxValue
        Private startBookmark As PDOutlineItem = Nothing
        Private startBookmarkPageNumber As Integer = -1
        Private endBookmark As PDOutlineItem = Nothing
        Private endBookmarkPageNumber As Integer = -1
        Private suppressDuplicateOverlappingText As Boolean = True
        Private shouldSeparateByBeads As Boolean = True
        Private sortByPosition As Boolean = False
        Private addMoreFormatting As Boolean = False

        Private indentThreshold As Single = DEFAULT_INDENT_THRESHOLD
        Private dropThreshold As Single = DEFAULT_DROP_THRESHOLD

        ' We will need to estimate where to add spaces.  
        ' These are used to help guess. 
        Private spacingTolerance As Single = 0.5F
        Private averageCharTolerance As Single = 0.300000012F

        Private pageArticles As List(Of PDThreadBead) = Nothing
        '/**
        ' * The charactersByArticle is used to extract text by article divisions.  For example
        ' * a PDF that has two columns like a newspaper, we want to extract the first column and
        ' * then the second column.  In this example the PDF would have 2 beads(or articles), one for
        ' * each column.  The size of the charactersByArticle would be 5, because not all text on the
        ' * screen will fall into one of the articles.  The five divisions are shown below
        ' *
        ' * Text before first article
        ' * first article text
        ' * text between first article and second article
        ' * second article text
        ' * text after second article
        ' *
        ' * Most PDFs won't have any beads, so charactersByArticle will contain a single entry.
        ' */
        Protected charactersByArticle As Vector(Of ArrayList(Of TextPosition)) = New Vector(Of ArrayList(Of TextPosition))()

        Private characterListMapping As Map(Of String, TreeMap(Of Single, TreeSet(Of Single))) = New HashMap(Of String, TreeMap(Of Single, TreeSet(Of Single)))()

        ''' <summary>
        ''' encoding that text will be written in (or null).
        ''' </summary>
        ''' <remarks></remarks>
        Protected outputEncoding As String

        ''' <summary>
        ''' The document to read.
        ''' </summary>
        ''' <remarks></remarks>
        Protected document As PDDocument

        ''' <summary>
        ''' The stream to write the output to.
        ''' </summary>
        ''' <remarks></remarks>
        Protected output As FinSeA.Io.Writer

        ''' <summary>
        ''' The normalizer is used to remove text ligatures/presentation forms and to correct the direction of right to left text, such as Arabic and Hebrew.
        ''' </summary>
        ''' <remarks></remarks>
        Private _normalize As TextNormalize = Nothing

        ''' <summary>
        ''' True if we started a paragraph but haven't ended it yet.
        ''' </summary>
        ''' <remarks></remarks>
        Private inParagraph As Boolean

        '/**
        ' * Instantiate a new PDFTextStripper object. This object will load
        ' * properties from PDFTextStripper.properties and will not do
        ' * anything special to convert the text to a more encoding-specific
        ' * output.
        ' *
        ' * @throws IOException If there is an error loading the properties.
        ' */
        Public Sub New() 'throws IOException
            MyBase.New(ResourceLoader.loadProperties("org/apache/pdfbox/resources/PDFTextStripper.properties", True))
            Me.outputEncoding = Nothing
            Me._normalize = New TextNormalize(Me.outputEncoding)
        End Sub

        '/**
        ' * Instantiate a new PDFTextStripper object.  Loading all of the operator mappings
        ' * from the properties object that is passed in.  Does not convert the text
        ' * to more encoding-specific output.
        ' *
        ' * @param props The properties containing the mapping of operators to PDFOperator
        ' * classes.
        ' *
        ' * @throws IOException If there is an error reading the properties.
        ' */
        Public Sub New(ByVal props As Properties) 'throws IOException
            MyBase.New(props)
            Me.outputEncoding = Nothing
            Me._normalize = New TextNormalize(Me.outputEncoding)
        End Sub


        '/**
        ' * Instantiate a new PDFTextStripper object. This object will load
        ' * properties from PDFTextStripper.properties and will apply
        ' * encoding-specific conversions to the output text.
        ' *
        ' * @param encoding The encoding that the output will be written in.
        ' * @throws IOException If there is an error reading the properties.
        ' */
        Public Sub New(ByVal encoding As String) 'throws IOException
            MyBase.New(ResourceLoader.loadProperties("org/apache/pdfbox/resources/PDFTextStripper.properties", True))
            Me.outputEncoding = encoding
            Me._normalize = New TextNormalize(Me.outputEncoding)
        End Sub

        '/**
        ' * This will return the text of a document.  See writeText. <br />
        ' * NOTE: The document must not be encrypted when coming into this method.
        ' *
        ' * @param doc The document to get the text from.
        ' * @return The text of the PDF document.
        ' * @throws IOException if the doc state is invalid or it is encrypted.
        ' */
        Public Function getText(ByVal doc As PDDocument) As String ' throws IOException
            Dim outputStream As FinSeA.Io.StringWriter = New FinSeA.Io.StringWriter()
            writeText(doc, outputStream)
            Return outputStream.ToString()
        End Function

        '/**
        ' * @deprecated
        ' * @see PDFTextStripper#getText( PDDocument )
        ' * @param doc The document to extract the text from.
        ' * @return The document text.
        ' * @throws IOException If there is an error extracting the text.
        ' */
        Public Function getText(ByVal doc As COSDocument) As String 'throws IOException
            Return getText(New PDDocument(doc))
        End Function

        '/**
        ' * @deprecated
        ' * @see PDFTextStripper#writeText( PDDocument, Writer )
        ' * @param doc The document to extract the text.
        ' * @param outputStream The stream to write the text to.
        ' * @throws IOException If there is an error extracting the text.
        ' */
        Public Sub writeText(ByVal doc As COSDocument, ByVal outputStream As FinSeA.Io.Writer) 'throws IOException
            writeText(New PDDocument(doc), outputStream)
        End Sub

        Public Overrides Sub resetEngine()
            MyBase.resetEngine()
            currentPageNo = 0
        End Sub

        '/**
        ' * This will take a PDDocument and write the text of that document to the print writer.
        ' *
        ' * @param doc The document to get the data from.
        ' * @param outputStream The location to put the text.
        ' *
        ' * @throws IOException If the doc is in an invalid state.
        ' */
        Public Sub writeText(ByVal doc As PDDocument, ByVal outputStream As FinSeA.Io.Writer)  'throws IOException
            resetEngine()
            document = doc
            output = outputStream
            If (getAddMoreFormatting()) Then
                paragraphEnd = lineSeparator
                pageStart = lineSeparator
                articleStart = lineSeparator
                articleEnd = lineSeparator
            End If
            startDocument(document)

            If (document.isEncrypted()) Then
                '// We are expecting non-encrypted documents here, but it is common
                '// for users to pass in a document that is encrypted with an empty
                '// password (such a document appears to not be encrypted by
                '// someone viewing the document, thus the confusion).  We will
                '// attempt to decrypt with the empty password to handle this case.
                '//
                Try
                    document.decrypt("")
                Catch e As CryptographyException
                    Throw New WrappedIOException("Error decrypting document, details: ", e)
                Catch e As InvalidPasswordException
                    Throw New WrappedIOException("Error: document is encrypted", e)
                End Try
            End If
            processPages(document.getDocumentCatalog().getAllPages())
            endDocument(document)
        End Sub

        '/**
        ' * This will process all of the pages and the text that is in them.
        ' *
        ' * @param pages The pages object in the document.
        ' *
        ' * @throws IOException If there is an error parsing the text.
        ' */
        Protected Sub processPages(ByVal pages As List(Of COSObjectable)) 'throws IOException
            If (startBookmark IsNot Nothing) Then
                startBookmarkPageNumber = getPageNumber(startBookmark, pages)
            End If
            If (endBookmark IsNot Nothing) Then
                endBookmarkPageNumber = getPageNumber(endBookmark, pages)
            End If

            If (startBookmarkPageNumber = -1 AndAlso startBookmark IsNot Nothing AndAlso endBookmarkPageNumber = -1 AndAlso endBookmark IsNot Nothing AndAlso startBookmark.getCOSObject().Equals(endBookmark.getCOSObject())) Then
                'this is a special case where both the start and end bookmark
                'are the same but point to nothing.  In this case
                'we will not extract any text.
                startBookmarkPageNumber = 0
                endBookmarkPageNumber = 0
            End If
            'Iterator(Of COSObjectable) pageIter = pages.iterator();
            '   While (pageIter.hasNext())
            For Each nextPage As PDPage In pages '{
                'PDPage nextPage = (PDPage)pageIter.next();
                Dim contentStream As PDStream = nextPage.getContents()
                currentPageNo += 1
                If (contentStream IsNot Nothing) Then
                    Dim contents As COSStream = contentStream.getStream()
                    processPage(nextPage, contents)
                End If
            Next
        End Sub

        Private Function getPageNumber(ByVal bookmark As PDOutlineItem, ByVal allPages As List(Of COSObjectable)) As Integer 'throws IOException
            Dim pageNumber As Integer = -1
            Dim page As PDPage = bookmark.findDestinationPage(document)
            If (page IsNot Nothing) Then
                pageNumber = allPages.indexOf(page) + 1 'use one based indexing
            End If
            Return pageNumber
        End Function

        '/**
        ' * This method is available for subclasses of this class.  It will be called before processing
        ' * of the document start.
        ' *
        ' * @param pdf The PDF document that is being processed.
        ' * @throws IOException If an IO error occurs.
        ' */
        Protected Sub startDocument(ByVal pdf As PDDocument) 'throws IOException
            ' no default implementation, but available for subclasses
        End Sub

        '/**
        ' * This method is available for subclasses of this class.  It will be called after processing
        ' * of the document finishes.
        ' *
        ' * @param pdf The PDF document that is being processed.
        ' * @throws IOException If an IO error occurs.
        ' */
        Protected Sub endDocument(ByVal pdf As PDDocument) 'throws IOException
            ' no default implementation, but available for subclasses
        End Sub

        '/**
        ' * This will process the contents of a page.
        ' *
        ' * @param page The page to process.
        ' * @param content The contents of the page.
        ' *
        ' * @throws IOException If there is an error processing the page.
        ' */
        Protected Sub processPage(ByVal page As PDPage, ByVal content As COSStream) 'throws IOException
            If (currentPageNo >= _startPage AndAlso currentPageNo <= _endPage AndAlso (startBookmarkPageNumber = -1 OrElse currentPageNo >= startBookmarkPageNumber) AndAlso (endBookmarkPageNumber = -1 OrElse currentPageNo <= endBookmarkPageNumber)) Then
                startPage(page)
                pageArticles = page.getThreadBeads()
                Dim numberOfArticleSections As Integer = 1 + pageArticles.size() * 2
                If (Not shouldSeparateByBeads) Then
                    numberOfArticleSections = 1
                End If
                Dim originalSize As Integer = charactersByArticle.size()
                charactersByArticle.setSize(numberOfArticleSections)
                For i As Integer = 0 To numberOfArticleSections - 1
                    If (numberOfArticleSections < originalSize) Then
                        DirectCast(charactersByArticle.get(i), List(Of TextPosition)).clear()
                    Else
                        charactersByArticle.set(i, New ArrayList(Of TextPosition)())
                    End If
                Next
                characterListMapping.clear()
                processStream(page, page.findResources(), content)
                writePage()
                endPage(page)
            End If
        End Sub

        '/**
        ' * Start a new article, which is typically defined as a column
        ' * on a single page (also referred to as a bead).  This assumes
        ' * that the primary direction of text is left to right.  
        ' * Default implementation is to do nothing.  Subclasses
        ' * may provide additional information.
        ' *
        ' * @throws IOException If there is any error writing to the stream.
        ' */
        Protected Sub startArticle() 'throws IOException
            startArticle(True)
        End Sub

        '/**
        ' * Start a new article, which is typically defined as a column
        ' * on a single page (also referred to as a bead).  
        ' * Default implementation is to do nothing.  Subclasses
        ' * may provide additional information.
        ' *
        ' * @param isltr true if primary direction of text is left to right.
        ' * @throws IOException If there is any error writing to the stream.
        ' */
        Protected Sub startArticle(ByVal isltr As Boolean)  'throws IOException
            output.write(getArticleStart())
        End Sub

        '/**
        ' * End an article.  Default implementation is to do nothing.  Subclasses
        ' * may provide additional information.
        ' *
        ' * @throws IOException If there is any error writing to the stream.
        ' */
        Protected Sub endArticle() 'throws IOException
            output.write(getArticleEnd())
        End Sub

        '/**
        ' * Start a new page.  Default implementation is to do nothing.  Subclasses
        ' * may provide additional information.
        ' *
        ' * @param page The page we are about to process.
        ' *
        ' * @throws IOException If there is any error writing to the stream.
        ' */
        Protected Sub startPage(ByVal page As PDPage) 'throws IOException
            'default is to do nothing.
        End Sub

        '/**
        ' * End a page.  Default implementation is to do nothing.  Subclasses
        ' * may provide additional information.
        ' *
        ' * @param page The page we are about to process.
        ' *
        ' * @throws IOException If there is any error writing to the stream.
        ' */
        Protected Overridable Sub endPage(ByVal page As PDPage) 'throws IOException
            'default is to do nothing
        End Sub

        Private Const ENDOFLASTTEXTX_RESET_VALUE As Single = -1
        Private Const MAXYFORLINE_RESET_VALUE As Single = -Single.MaxValue
        Private Const EXPECTEDSTARTOFNEXTWORDX_RESET_VALUE As Single = -Single.MaxValue
        Private Const MAXHEIGHTFORLINE_RESET_VALUE As Single = -1
        Private Const MINYTOPFORLINE_RESET_VALUE As Single = Single.MaxValue
        Private Const LASTWORDSPACING_RESET_VALUE As Single = -1

        '/**
        ' * This will print the text of the processed page to "output".
        ' * It will estimate, based on the coordinates of the text, where
        ' * newlines and word spacings should be placed. The text will be
        ' * sorted only if that feature was enabled. 
        ' *
        ' * @throws IOException If there is an error writing the text.
        ' */
        Protected Overridable Sub writePage() 'throws IOException
            Dim maxYForLine As Single = MAXYFORLINE_RESET_VALUE
            Dim minYTopForLine As Single = MINYTOPFORLINE_RESET_VALUE
            Dim endOfLastTextX As Single = ENDOFLASTTEXTX_RESET_VALUE
            Dim lastWordSpacing As Single = LASTWORDSPACING_RESET_VALUE
            Dim maxHeightForLine As Single = MAXHEIGHTFORLINE_RESET_VALUE
            Dim lastPosition As PositionWrapper = Nothing
            Dim lastLineStartPosition As PositionWrapper = Nothing

            Dim startOfPage As Boolean = True 'flag to indicate start of page
            Dim startOfArticle As Boolean = True
            If (charactersByArticle.size() > 0) Then
                writePageStart()
            End If

            For i As Integer = 0 To charactersByArticle.size() - 1
                Dim textList As List(Of TextPosition) = charactersByArticle.get(i)
                If (getSortByPosition()) Then
                    Dim comparator As TextPositionComparator = New TextPositionComparator()
                    Collections.sort(Of TextPosition)(textList, comparator)
                End If
                '           Iterator(Of TextPosition) textIter = textList.iterator();
                '/* Before we can display the text, we need to do some normalizing.
                ' * Arabic and Hebrew text is right to left and is typically stored
                ' * in its logical format, which means that the rightmost character is
                ' * stored first, followed by the second character from the right etc.
                ' * However, PDF stores the text in presentation form, which is left to
                ' * right.  We need to do some normalization to convert the PDF data to
                ' * the proper logical output format.
                ' *
                ' * Note that if we did not sort the text, then the output of reversing the
                ' * text is undefined and can sometimes produce worse output then not trying
                ' * to reverse the order.  Sorting should be done for these languages.
                ' * */

                '/* First step is to determine if we have any right to left text, and
                '             * if so, is it dominant. */
                Dim ltrCnt As Integer = 0
                Dim rtlCnt As Integer = 0

                For Each position As TextPosition In textList '			           Iterator(Of TextPosition) textIter = textList.iterator();
                    'While (textIter.hasNext())
                    '{
                    '   TextPosition position = (TextPosition)textIter.next();
                    Dim stringValue As String = position.getCharacter()
                    For a As Integer = 0 To stringValue.Length() - 1
                        Dim dir As Byte = NChar.getDirectionality(stringValue.Chars(a))
                        If ((dir = NChar.DIRECTIONALITY_LEFT_TO_RIGHT) OrElse (dir = NChar.DIRECTIONALITY_LEFT_TO_RIGHT_EMBEDDING) OrElse (dir = NChar.DIRECTIONALITY_LEFT_TO_RIGHT_OVERRIDE)) Then
                            ltrCnt += 1
                        ElseIf ((dir = NChar.DIRECTIONALITY_RIGHT_TO_LEFT) OrElse (dir = NChar.DIRECTIONALITY_RIGHT_TO_LEFT_ARABIC) OrElse (dir = NChar.DIRECTIONALITY_RIGHT_TO_LEFT_EMBEDDING) OrElse (dir = NChar.DIRECTIONALITY_RIGHT_TO_LEFT_OVERRIDE)) Then
                            rtlCnt += 1
                        End If
                    Next
                Next
                ' choose the dominant direction
                Dim isRtlDominant As Boolean = rtlCnt > ltrCnt

                startArticle(Not isRtlDominant)
                startOfArticle = True
                ' we will later use this to skip reordering
                Dim hasRtl As Boolean = rtlCnt > 0

                '/* Now cycle through to print the text.
                '* We queue up a line at a time before we print so that we can convert
                '* the line from presentation form to logical form (if needed). 
                '*/
                Dim line As List(Of TextPosition) = New ArrayList(Of TextPosition)()

                '/* PDF files don't always store spaces. We will need to guess where we should add
                ' * spaces based on the distances between TextPositions. Historically, this was done
                ' * based on the size of the space character provided by the font. In general, this worked
                ' * but there were cases where it did not work. Calculating the average character width
                ' * and using that as a metric works better in some cases but fails in some cases where the
                ' * spacing worked. So we use both. NOTE: Adobe reader also fails on some of these examples.
                ' */
                '//Keeps track of the previous average character width
                Dim previousAveCharWidth As Single = -1
                'textIter = textList.iterator();    // start from the beginning again
                '   While (textIter.hasNext())
                '{
                '   TextPosition position = (TextPosition)textIter.next();
                For Each position As TextPosition In textList
                    Dim current As PositionWrapper = New PositionWrapper(position)
                    Dim characterValue As String = position.getCharacter()

                    'Resets the average character width when we see a change in font
                    ' or a change in the font size
                    If (lastPosition IsNot Nothing AndAlso ((position.getFont() IsNot lastPosition.getTextPosition().getFont()) OrElse (position.getFontSize() <> lastPosition.getTextPosition().getFontSize()))) Then
                        previousAveCharWidth = -1
                    End If

                    Dim positionX As Single
                    Dim positionY As Single
                    Dim positionWidth As Single
                    Dim positionHeight As Single

                    ' If we are sorting, then we need to use the text direction
                    ' adjusted coordinates, because they were used in the sorting. */
                    If (getSortByPosition()) Then
                        positionX = position.getXDirAdj()
                        positionY = position.getYDirAdj()
                        positionWidth = position.getWidthDirAdj()
                        positionHeight = position.getHeightDir()
                    Else
                        positionX = position.getX()
                        positionY = position.getY()
                        positionWidth = position.getWidth()
                        positionHeight = position.getHeight()
                    End If

                    'The current amount of characters in a word
                    Dim wordCharCount As Integer = position.getIndividualWidths().Length

                    ' Estimate the expected width of the space based on the
                    ' space character with some margin. */
                    Dim wordSpacing As Single = position.getWidthOfSpace()
                    Dim deltaSpace As Single = 0
                    If ((wordSpacing = 0) OrElse (wordSpacing = Single.NaN)) Then
                        deltaSpace = Single.MaxValue
                    Else
                        If (lastWordSpacing < 0) Then
                            deltaSpace = (wordSpacing * getSpacingTolerance())
                        Else
                            deltaSpace = (((wordSpacing + lastWordSpacing) / 2.0F) * getSpacingTolerance())
                        End If
                    End If

                    '/* Estimate the expected width of the space based on the
                    ' * average character width with some margin. This calculation does not
                    ' * make a true average (average of averages) but we found that it gave the
                    ' * best results after numerous experiments. Based on experiments we also found that
                    ' * .3 worked well. */
                    Dim averageCharWidth As Single = -1
                    If (previousAveCharWidth < 0) Then
                        averageCharWidth = (positionWidth / wordCharCount)
                    Else
                        averageCharWidth = (previousAveCharWidth + (positionWidth / wordCharCount)) / 2.0F
                    End If
                    Dim deltaCharWidth As Single = (averageCharWidth * getAverageCharTolerance())

                    'Compares the values obtained by the average method and the wordSpacing method and picks
                    'the smaller number.
                    Dim expectedStartOfNextWordX As Single = EXPECTEDSTARTOFNEXTWORDX_RESET_VALUE
                    If (endOfLastTextX <> ENDOFLASTTEXTX_RESET_VALUE) Then
                        If (deltaCharWidth > deltaSpace) Then
                            expectedStartOfNextWordX = endOfLastTextX + deltaSpace
                        Else
                            expectedStartOfNextWordX = endOfLastTextX + deltaCharWidth
                        End If
                    End If

                    If (lastPosition IsNot Nothing) Then
                        If (startOfArticle) Then
                            lastPosition.setArticleStart()
                            startOfArticle = False
                        End If
                        '// RDD - Here we determine whether this text object is on the current
                        '// line.  We use the lastBaselineFontSize to handle the superscript
                        '// case, and the size of the current font to handle the subscript case.
                        '// Text must overlap with the last rendered baseline text by at least
                        '// a small amount in order to be considered as being on the same line.

                        '/* XXX BC: In theory, this check should really check if the next char is in full range
                        ' * seen in this line. This is what I tried to do with minYTopForLine, but this caused a lot
                        ' * of regression test failures.  So, I'm leaving it be for now. */
                        If (Not overlap(positionY, positionHeight, maxYForLine, maxHeightForLine)) Then
                            writeLine(normalize(line, isRtlDominant, hasRtl), isRtlDominant)
                            line.clear()
                            lastLineStartPosition = handleLineSeparation(current, lastPosition, lastLineStartPosition, maxHeightForLine)
                            endOfLastTextX = ENDOFLASTTEXTX_RESET_VALUE
                            expectedStartOfNextWordX = EXPECTEDSTARTOFNEXTWORDX_RESET_VALUE
                            maxYForLine = MAXYFORLINE_RESET_VALUE
                            maxHeightForLine = MAXHEIGHTFORLINE_RESET_VALUE
                            minYTopForLine = MINYTOPFORLINE_RESET_VALUE
                        End If
                        '//Test if our TextPosition starts after a new word would be expected to start.
                        '//only bother adding a space if the last character was not a space
                        If (expectedStartOfNextWordX <> EXPECTEDSTARTOFNEXTWORDX_RESET_VALUE AndAlso expectedStartOfNextWordX < positionX AndAlso lastPosition.getTextPosition().getCharacter() IsNot Nothing AndAlso Not lastPosition.getTextPosition().getCharacter().endsWith(" ")) Then
                            line.add(WordSeparator.getSeparator())
                        End If
                    End If
                    If (positionY >= maxYForLine) Then
                        maxYForLine = positionY
                    End If
                    ' RDD - endX is what PDF considers to be the x coordinate of the
                    ' end position of the text.  We use it in computing our metrics below.
                    endOfLastTextX = positionX + positionWidth

                    ' add it to the list
                    If (characterValue IsNot Nothing) Then
                        If (startOfPage AndAlso lastPosition Is Nothing) Then
                            writeParagraphStart() 'not sure this is correct for RTL?
                        End If
                        line.add(position)
                    End If
                    maxHeightForLine = Math.Max(maxHeightForLine, positionHeight)
                    minYTopForLine = Math.Min(minYTopForLine, positionY - positionHeight)
                    lastPosition = current
                    If (startOfPage) Then
                        lastPosition.setParagraphStart()
                        lastPosition.setLineStart()
                        lastLineStartPosition = lastPosition
                        startOfPage = False
                    End If
                    lastWordSpacing = wordSpacing
                    previousAveCharWidth = averageCharWidth
                Next
                ' print the final line
                If (line.size() > 0) Then
                    writeLine(normalize(line, isRtlDominant, hasRtl), isRtlDominant)
                    writeParagraphEnd()
                End If
                endArticle()
            Next
            writePageEnd()
        End Sub

        Private Function overlap(ByVal y1 As Single, ByVal height1 As Single, ByVal y2 As Single, ByVal height2 As Single) As Boolean
            Return within(y1, y2, 0.1F) OrElse (y2 <= y1 AndAlso y2 >= y1 - height1) OrElse (y1 <= y2 AndAlso y1 >= y2 - height2)
        End Function

        '/**
        ' * Write the page separator value to the output stream.
        ' * @throws IOException
        ' *             If there is a problem writing out the pageseparator to the document.
        ' */
        Protected Sub writePageSeperator() 'throws IOException
            ' RDD - newline at end of flush - required for end of page (so that the top
            ' of the next page starts on its own line.
            output.write(getPageSeparator())
            output.flush()
        End Sub

        '/**
        ' * Write the line separator value to the output stream.
        ' * @throws IOException
        ' *             If there is a problem writing out the lineseparator to the document.
        ' */
        Protected Sub writeLineSeparator() 'throws IOException
            output.write(getLineSeparator())
        End Sub


        '/**
        ' * Write the word separator value to the output stream.
        ' * @throws IOException
        ' *             If there is a problem writing out the wordseparator to the document.
        ' */
        Protected Sub writeWordSeparator() 'throws IOException
            output.write(getWordSeparator())
        End Sub

        '/**
        ' * Write the string in TextPosition to the output stream.
        ' *
        ' * @param text The text to write to the stream.
        ' * @throws IOException If there is an error when writing the text.
        ' */
        Protected Sub writeCharacters(ByVal text As TextPosition) 'throws IOException
            output.write(text.getCharacter())
        End Sub

        '/**
        ' * Write a Java string to the output stream. The default implementation will ignore the <code>textPositions</code>
        ' * and just calls {@link #writeString(String)}.
        ' *
        ' * @param text The text to write to the stream.
        ' * @param textPositions The TextPositions belonging to the text.
        ' * @throws IOException If there is an error when writing the text.
        ' */
        Protected Sub writeString(ByVal text As String, ByVal textPositions As List(Of TextPosition)) 'throws IOException
            writeString(text)
        End Sub

        '/**
        ' * Write a Java string to the output stream.
        ' *
        ' * @param text The text to write to the stream.
        ' * @throws IOException If there is an error when writing the text.
        ' */
        Protected Sub writeString(ByVal text As String) 'throws IOException
            output.write(text)
        End Sub

        '/**
        ' * This will determine of two floating point numbers are within a specified variance.
        ' *
        ' * @param first The first number to compare to.
        ' * @param second The second number to compare to.
        ' * @param variance The allowed variance.
        ' */
        Private Function within(ByVal first As Single, ByVal second As Single, ByVal variance As Single) As Boolean
            Return second < first + variance AndAlso second > first - variance
        End Function

        '/**
        ' * This will process a TextPosition object and add the
        ' * text to the list of characters on a page.  It takes care of
        ' * overlapping text.
        ' *
        ' * @param text The text to process.
        ' */
        Protected Overrides Sub processTextPosition(ByVal text As TextPosition)
            Dim showCharacter As Boolean = True
            If (suppressDuplicateOverlappingText) Then
                showCharacter = False
                Dim textCharacter As String = text.getCharacter()
                Dim textX As Single = text.getX()
                Dim textY As Single = text.getY()
                Dim sameTextCharacters As TreeMap(Of Single, TreeSet(Of Single)) = characterListMapping.get(textCharacter)
                If (sameTextCharacters Is Nothing) Then
                    sameTextCharacters = New TreeMap(Of Single, TreeSet(Of Single))()
                    characterListMapping.put(textCharacter, sameTextCharacters)
                End If
                '// RDD - Here we compute the value that represents the end of the rendered
                '// text.  This value is used to determine whether subsequent text rendered
                '// on the same line overwrites the current text.
                '//
                '// We subtract any positive padding to handle cases where extreme amounts
                '// of padding are applied, then backed off (not sure why this is done, but there
                '// are cases where the padding is on the order of 10x the character width, and
                '// the TJ just backs up to compensate after each character).  Also, we subtract
                '// an amount to allow for kerning (a percentage of the width of the last
                '// character).
                '//
                Dim suppressCharacter As Boolean = False
                Dim tolerance As Single = (text.getWidth() / textCharacter.Length()) / 3.0F

                Dim xMatches As SortedMap(Of Single, TreeSet(Of Single)) = sameTextCharacters.subMap(textX - tolerance, textX + tolerance)
                For Each xMatch As TreeSet(Of Single) In xMatches.Values()
                    Dim yMatches As SortedSet(Of Single) = xMatch.subSet(textY - tolerance, textY + tolerance)
                    If (Not yMatches.isEmpty()) Then
                        suppressCharacter = True
                        Exit For
                    End If
                Next
                If (Not suppressCharacter) Then
                    Dim ySet As TreeSet(Of Single) = sameTextCharacters.get(textX)
                    If (ySet Is Nothing) Then
                        ySet = New TreeSet(Of Single)()
                        sameTextCharacters.put(textX, ySet)
                    End If
                    ySet.add(textY)
                    showCharacter = True
                End If
            End If
            If (showCharacter) Then
                '//if we are showing the character then we need to determine which
                '/article it belongs to.
                Dim foundArticleDivisionIndex As Integer = -1
                Dim notFoundButFirstLeftAndAboveArticleDivisionIndex As Integer = -1
                Dim notFoundButFirstLeftArticleDivisionIndex As Integer = -1
                Dim notFoundButFirstAboveArticleDivisionIndex As Integer = -1
                Dim x As Single = text.getX()
                Dim y As Single = text.getY()
                If (shouldSeparateByBeads) Then
                    For i As Integer = 0 To pageArticles.size() - 1
                        If (foundArticleDivisionIndex <> -1) Then Exit For
                        Dim bead As PDThreadBead = pageArticles.get(i)
                        If (bead IsNot Nothing) Then
                            Dim rect As PDRectangle = bead.getRectangle()
                            If (rect.contains(x, y)) Then
                                foundArticleDivisionIndex = i * 2 + 1
                            ElseIf ((x < rect.getLowerLeftX() OrElse y < rect.getUpperRightY()) AndAlso notFoundButFirstLeftAndAboveArticleDivisionIndex = -1) Then
                                notFoundButFirstLeftAndAboveArticleDivisionIndex = i * 2
                            ElseIf (x < rect.getLowerLeftX() AndAlso notFoundButFirstLeftArticleDivisionIndex = -1) Then
                                notFoundButFirstLeftArticleDivisionIndex = i * 2
                            ElseIf (y < rect.getUpperRightY() AndAlso notFoundButFirstAboveArticleDivisionIndex = -1) Then
                                notFoundButFirstAboveArticleDivisionIndex = i * 2
                            End If
                        Else
                            foundArticleDivisionIndex = 0
                        End If
                    Next
                Else
                    foundArticleDivisionIndex = 0
                End If
                Dim articleDivisionIndex As Integer = -1
                If (foundArticleDivisionIndex <> -1) Then
                    articleDivisionIndex = foundArticleDivisionIndex
                ElseIf (notFoundButFirstLeftAndAboveArticleDivisionIndex <> -1) Then
                    articleDivisionIndex = notFoundButFirstLeftAndAboveArticleDivisionIndex
                ElseIf (notFoundButFirstLeftArticleDivisionIndex <> -1) Then
                    articleDivisionIndex = notFoundButFirstLeftArticleDivisionIndex
                ElseIf (notFoundButFirstAboveArticleDivisionIndex <> -1) Then
                    articleDivisionIndex = notFoundButFirstAboveArticleDivisionIndex
                Else
                    articleDivisionIndex = charactersByArticle.size() - 1
                End If

                Dim textList As List(Of TextPosition) = charactersByArticle.get(articleDivisionIndex)

                '/* In the wild, some PDF encoded documents put diacritics (accents on
                ' * top of characters) into a separate Tj element.  When displaying them
                ' * graphically, the two chunks get overlayed.  With text output though,
                ' * we need to do the overlay. This code recombines the diacritic with
                ' * its associated character if the two are consecutive.
                ' */ 
                If (textList.isEmpty()) Then
                    textList.add(text)
                Else
                    '/* test if we overlap the previous entry.  
                    ' * Note that we are making an assumption that we need to only look back
                    ' * one TextPosition to find what we are overlapping.  
                    ' * This may not always be true. */
                    Dim previousTextPosition As TextPosition = textList.get(textList.size() - 1)
                    If (text.isDiacritic() AndAlso previousTextPosition.contains(text)) Then
                        previousTextPosition.mergeDiacritic(text, Me._normalize)
                        '/* If the previous TextPosition was the diacritic, merge it into this
                        '* one and remove it from the list. */
                    ElseIf (previousTextPosition.isDiacritic() AndAlso text.contains(previousTextPosition)) Then
                        text.mergeDiacritic(previousTextPosition, Me._normalize)
                        textList.remove(textList.size() - 1)
                        textList.add(text)
                    Else
                        textList.add(text)
                    End If
                End If
            End If
        End Sub

        '/**
        ' * This is the page that the text extraction will start on.  The pages start
        ' * at page 1.  For example in a 5 page PDF document, if the start page is 1
        ' * then all pages will be extracted.  If the start page is 4 then pages 4 and 5
        ' * will be extracted.  The default value is 1.
        ' *
        ' * @return Value of property _startPage.
        ' */
        Public Function getStartPage() As Integer
            Return _startPage
        End Function

        '/**
        ' * This will set the first page to be extracted by this class.
        ' *
        ' * @param startPageValue New value of property _startPage.
        ' */
        Public Sub setStartPage(ByVal startPageValue As Integer)
            _startPage = startPageValue
        End Sub

        '/**
        ' * This will get the last page that will be extracted.  This is inclusive,
        ' * for example if a 5 page PDF an _endPage value of 5 would extract the
        ' * entire document, an end page of 2 would extract pages 1 and 2.  This defaults
        ' * to Integer.MAX_VALUE such that all pages of the pdf will be extracted.
        ' *
        ' * @return Value of property _endPage.
        ' */
        Public Function getEndPage() As Integer
            Return _endPage
        End Function

        '/**
        ' * This will set the last page to be extracted by this class.
        ' *
        ' * @param endPageValue New value of property _endPage.
        ' */
        Public Sub setEndPage(ByVal endPageValue As Integer)
            _endPage = endPageValue
        End Sub

        '/**
        ' * Set the desired line separator for output text.  The line.separator
        ' * system property is used if the line separator preference is not set
        ' * explicitly using this method.
        ' *
        ' * @param separator The desired line separator string.
        ' */
        Public Sub setLineSeparator(ByVal separator As String)
            lineSeparator = separator
        End Sub

        '/**
        ' * This will get the line separator.
        ' *
        ' * @return The desired line separator string.
        ' */
        Public Function getLineSeparator() As String
            Return lineSeparator
        End Function

        '/**
        ' * Set the desired page separator for output text.  The line.separator
        ' * system property is used if the page separator preference is not set
        ' * explicitly using this method.
        ' *
        ' * @param separator The desired page separator string.
        ' */
        Public Sub setPageSeparator(ByVal separator As String)
            pageSeparator = separator
        End Sub

        '/**
        ' * This will get the word separator.
        ' *
        ' * @return The desired word separator string.
        ' */
        Public Function getWordSeparator() As String
            Return _wordSeparator
        End Function

        '/**
        ' * Set the desired word separator for output text.  The PDFBox text extraction
        ' * algorithm will output a space character if there is enough space between
        ' * two words.  By default a space character is used.  If you need and accurate
        ' * count of characters that are found in a PDF document then you might want to
        ' * set the word separator to the empty string.
        ' *
        ' * @param separator The desired page separator string.
        ' */
        Public Sub setWordSeparator(ByVal separator As String)
            _wordSeparator = separator
        End Sub

        '/**
        ' * This will get the page separator.
        ' *
        ' * @return The page separator string.
        ' */
        Public Function getPageSeparator() As String
            Return pageSeparator
        End Function

        '/**
        ' * @return Returns the suppressDuplicateOverlappingText.
        ' */
        Public Function getSuppressDuplicateOverlappingText() As Boolean
            Return suppressDuplicateOverlappingText
        End Function

        '/**
        ' * Get the current page number that is being processed.
        ' *
        ' * @return A 1 based number representing the current page.
        ' */
        Protected Function getCurrentPageNo() As Integer
            Return currentPageNo
        End Function

        '/**
        ' * The output stream that is being written to.
        ' *
        ' * @return The stream that output is being written to.
        ' */
        Protected Function getOutput() As FinSeA.Io.Writer
            Return output
        End Function

        '/**
        ' * Character strings are grouped by articles.  It is quite common that there
        ' * will only be a single article.  This returns a List that contains List objects,
        ' * the inner lists will contain TextPosition objects.
        ' *
        ' * @return A double List of TextPositions for all text strings on the page.
        ' */
        Protected Function getCharactersByArticle() As Vector(Of ArrayList(Of TextPosition))
            Return charactersByArticle
        End Function

        '/**
        ' * By default the text stripper will attempt to remove text that overlapps each other.
        ' * Word paints the same character several times in order to make it look bold.  By setting
        ' * this to false all text will be extracted, which means that certain sections will be
        ' * duplicated, but better performance will be noticed.
        ' *
        ' * @param suppressDuplicateOverlappingTextValue The suppressDuplicateOverlappingText to set.
        ' */
        Public Sub setSuppressDuplicateOverlappingText(ByVal suppressDuplicateOverlappingTextValue As Boolean)
            suppressDuplicateOverlappingText = suppressDuplicateOverlappingTextValue
        End Sub

        '/**
        ' * This will tell if the text stripper should separate by beads.
        ' *
        ' * @return If the text will be grouped by beads.
        ' */
        Public Function getSeparateByBeads() As Boolean
            Return shouldSeparateByBeads
        End Function

        '/**
        ' * Set if the text stripper should group the text output by a list of beads.  The default value is true!
        ' *
        ' * @param aShouldSeparateByBeads The new grouping of beads.
        ' */
        Public Sub setShouldSeparateByBeads(ByVal aShouldSeparateByBeads As Boolean)
            shouldSeparateByBeads = aShouldSeparateByBeads
        End Sub

        '/**
        ' * Get the bookmark where text extraction should end, inclusive.  Default is null.
        ' *
        ' * @return The ending bookmark.
        ' */
        Public Function getEndBookmark() As PDOutlineItem
            Return endBookmark
        End Function

        '/**
        ' * Set the bookmark where the text extraction should stop.
        ' *
        ' * @param aEndBookmark The ending bookmark.
        ' */
        Public Sub setEndBookmark(ByVal aEndBookmark As PDOutlineItem)
            endBookmark = aEndBookmark
        End Sub

        '/**
        ' * Get the bookmark where text extraction should start, inclusive.  Default is null.
        ' *
        ' * @return The starting bookmark.
        ' */
        Public Function getStartBookmark() As PDOutlineItem
            Return startBookmark
        End Function

        '/**
        ' * Set the bookmark where text extraction should start, inclusive.
        ' *
        ' * @param aStartBookmark The starting bookmark.
        ' */
        Public Sub setStartBookmark(ByVal aStartBookmark As PDOutlineItem)
            startBookmark = aStartBookmark
        End Sub

        '/**
        ' * This will tell if the text stripper should add some more text formatting.
        ' * @return true if some more text formatting will be added
        ' */
        Public Function getAddMoreFormatting() As Boolean
            Return addMoreFormatting
        End Function

        '/**
        ' * There will some additional text formatting be added if addMoreFormatting
        ' * is set to true. Default is false. 
        ' * @param newAddMoreFormatting Tell PDFBox to add some more text formatting
        ' */
        Public Sub setAddMoreFormatting(ByVal newAddMoreFormatting As Boolean)
            addMoreFormatting = newAddMoreFormatting
        End Sub

        '/**
        ' * This will tell if the text stripper should sort the text tokens
        ' * before writing to the stream.
        ' *
        ' * @return true If the text tokens will be sorted before being written.
        ' */
        Public Function getSortByPosition() As Boolean
            Return sortByPosition
        End Function

        '/**
        ' * The order of the text tokens in a PDF file may not be in the same
        ' * as they appear visually on the screen.  For example, a PDF writer may
        ' * write out all text by font, so all bold or larger text, then make a second
        ' * pass and write out the normal text.<br/>
        ' * The default is to <b>not</b> sort by position.<br/>
        ' * <br/>
        ' * A PDF writer could choose to write each character in a different order.  By
        ' * default PDFBox does <b>not</b> sort the text tokens before processing them due to
        ' * performance reasons.
        ' *
        ' * @param newSortByPosition Tell PDFBox to sort the text positions.
        ' */
        Public Sub setSortByPosition(ByVal newSortByPosition As Boolean)
            sortByPosition = newSortByPosition
        End Sub

        '/**
        ' * Get the current space width-based tolerance value that is being used
        ' * to estimate where spaces in text should be added.  Note that the
        ' * default value for this has been determined from trial and error. 
        ' * 
        ' * @return The current tolerance / scaling factor
        ' */
        Public Function getSpacingTolerance() As Single
            Return spacingTolerance
        End Function

        '/**
        ' * Set the space width-based tolerance value that is used
        ' * to estimate where spaces in text should be added.  Note that the
        ' * default value for this has been determined from trial and error.
        ' * Setting this value larger will reduce the number of spaces added. 
        ' * 
        ' * @param spacingToleranceValue tolerance / scaling factor to use
        ' */
        Public Sub setSpacingTolerance(ByVal spacingToleranceValue As Single)
            spacingTolerance = spacingToleranceValue
        End Sub

        '/**
        ' * Get the current character width-based tolerance value that is being used
        ' * to estimate where spaces in text should be added.  Note that the
        ' * default value for this has been determined from trial and error.
        ' * 
        ' * @return The current tolerance / scaling factor
        ' */
        Public Function getAverageCharTolerance() As Single
            Return averageCharTolerance
        End Function

        '/**
        ' * Set the character width-based tolerance value that is used
        ' * to estimate where spaces in text should be added.  Note that the
        ' * default value for this has been determined from trial and error.
        ' * Setting this value larger will reduce the number of spaces added. 
        ' * 
        ' * @param averageCharToleranceValue average tolerance / scaling factor to use
        ' */
        Public Sub setAverageCharTolerance(ByVal averageCharToleranceValue As Single)
            averageCharTolerance = averageCharToleranceValue
        End Sub


        '/**
        ' * returns the multiple of whitespace character widths
        ' * for the current text which the current
        ' * line start can be indented from the previous line start
        ' * beyond which the current line start is considered
        ' * to be a paragraph start.
        ' * @return the number of whitespace character widths to use
        ' * when detecting paragraph indents.
        ' */
        Public Function getIndentThreshold() As Single
            Return indentThreshold
        End Function

        '/**
        ' * sets the multiple of whitespace character widths
        ' * for the current text which the current
        ' * line start can be indented from the previous line start
        ' * beyond which the current line start is considered
        ' * to be a paragraph start.  The default value is 2.0.
        ' *
        ' * @param indentThresholdValue the number of whitespace character widths to use
        ' * when detecting paragraph indents.
        ' */
        Public Sub setIndentThreshold(ByVal indentThresholdValue As Single)
            indentThreshold = indentThresholdValue
        End Sub

        '/**
        ' * the minimum whitespace, as a multiple
        ' * of the max height of the current characters
        ' * beyond which the current line start is considered
        ' * to be a paragraph start.
        ' * @return the character height multiple for
        ' * max allowed whitespace between lines in
        ' * the same paragraph.
        ' */
        Public Function getDropThreshold() As Single
            Return dropThreshold
        End Function

        '/**
        ' * sets the minimum whitespace, as a multiple
        ' * of the max height of the current characters
        ' * beyond which the current line start is considered
        ' * to be a paragraph start.  The default value is 2.5.
        ' *
        ' * @param dropThresholdValue the character height multiple for
        ' * max allowed whitespace between lines in
        ' * the same paragraph.
        ' */
        Public Sub setDropThreshold(ByVal dropThresholdValue As Single)
            dropThreshold = dropThresholdValue
        End Sub

        '/**
        ' * Returns the string which will be used at the beginning of a paragraph.
        ' * @return the paragraph start string
        ' */
        Public Function getParagraphStart() As String
            Return paragraphStart
        End Function

        '/**
        ' * Sets the string which will be used at the beginning of a paragraph.
        ' * @param s the paragraph start string
        ' */
        Public Sub setParagraphStart(ByVal s As String)
            paragraphStart = s
        End Sub

        '/**
        ' * Returns the string which will be used at the end of a paragraph.
        ' * @return the paragraph end string
        ' */
        Public Function getParagraphEnd() As String
            Return paragraphEnd
        End Function

        '/**
        ' * Sets the string which will be used at the end of a paragraph.
        ' * @param s the paragraph end string
        ' */
        Public Sub setParagraphEnd(ByVal s As String)
            paragraphEnd = s
        End Sub


        '/**
        ' * Returns the string which will be used at the beginning of a page.
        ' * @return the page start string
        ' */
        Public Function getPageStart() As String
            Return pageStart
        End Function

        '/**
        ' * Sets the string which will be used at the beginning of a page.
        ' * @param pageStartValue the page start string
        ' */
        Public Sub setPageStart(ByVal pageStartValue As String)
            pageStart = pageStartValue
        End Sub

        '/**
        ' * Returns the string which will be used at the end of a page.
        ' * @return the page end string
        ' */
        Public Function getPageEnd() As String
            Return pageEnd
        End Function

        '/**
        ' * Sets the string which will be used at the end of a page.
        ' * @param pageEndValue the page end string
        ' */
        Public Sub setPageEnd(ByVal pageEndValue As String)
            pageEnd = pageEndValue
        End Sub

        '/**
        ' * Returns the string which will be used at the beginning of an article.
        ' * @return the article start string
        ' */
        Public Function getArticleStart() As String
            Return articleStart
        End Function

        '/**
        ' * Sets the string which will be used at the beginning of an article.
        ' * @param articleStartValue the article start string
        ' */
        Public Sub setArticleStart(ByVal articleStartValue As String)
            articleStart = articleStartValue
        End Sub

        '/**
        ' * Returns the string which will be used at the end of an article.
        ' * @return the article end string
        ' */
        Public Function getArticleEnd() As String
            Return articleEnd
        End Function

        '/**
        ' * Sets the string which will be used at the end of an article.
        ' * @param articleEndValue the article end string
        ' */
        Public Sub setArticleEnd(ByVal articleEndValue As String)
            articleEnd = articleEndValue
        End Sub


        '/**
        ' * Reverse characters of a compound Arabic glyph.
        ' * When getSortByPosition() is true, inspect the sequence encoded
        ' * by one glyph. If the glyph encodes two or more Arabic characters,
        ' * reverse these characters from a logical order to a visual order.
        ' * This ensures that the bidirectional algorithm that runs later will
        ' * convert them back to a logical order.
        ' * 
        ' * @param str a string obtained from font.encoding()
        ' * 
        ' * @return the reversed string
        ' */
        Public Overrides Function inspectFontEncoding(ByVal str As String) As String
            If (Not sortByPosition OrElse str = "" OrElse str.Length() < 2) Then
                Return str
            End If
            For i As Integer = 0 To str.Length() - 1
                If (NChar.getDirectionality(str.Chars(i)) <> NChar.DIRECTIONALITY_RIGHT_TO_LEFT_ARABIC) Then
                    Return str
                End If
            Next
            Dim reversed As StringBuilder = New StringBuilder(str.Length())
            For i As Integer = str.Length() - 1 To 0 Step -1
                reversed.Append(str.Chars(i))
            Next
            Return reversed.ToString()
        End Function

        '/**
        ' * handles the line separator for a new line given
        ' * the specified current and previous TextPositions.
        ' * @param current the current text position
        ' * @param lastPosition the previous text position
        ' * @param lastLineStartPosition the last text position that followed a line
        ' *        separator.
        ' * @param maxHeightForLine max height for positions since lastLineStartPosition
        ' * @return start position of the last line
        ' * @throws IOException if something went wrong
        ' */
        Protected Function handleLineSeparation(ByVal current As PositionWrapper, ByVal lastPosition As PositionWrapper, ByVal lastLineStartPosition As PositionWrapper, ByVal maxHeightForLine As Single) As PositionWrapper  'throws IOException
            current.setLineStart()
            isParagraphSeparation(current, lastPosition, lastLineStartPosition, maxHeightForLine)
            lastLineStartPosition = current
            If (current.isParagraphStart()) Then
                If (lastPosition.isArticleStart()) Then
                    writeParagraphStart()
                Else
                    writeLineSeparator()
                    writeParagraphSeparator()
                End If
            Else
                writeLineSeparator()
            End If
            Return lastLineStartPosition
        End Function

        '/**
        ' * tests the relationship between the last text position, the current text
        ' * position and the last text position that followed a line separator to
        ' * decide if the gap represents a paragraph separation. This should
        ' * <i>only</i> be called for consecutive text positions that first pass the
        ' * line separation test.
        ' * <p>
        ' * This base implementation tests to see if the lastLineStartPosition is
        ' * null OR if the current vertical position has dropped below the last text
        ' * vertical position by at least 2.5 times the current text height OR if the
        ' * current horizontal position is indented by at least 2 times the current
        ' * width of a space character.</p>
        ' * <p>
        ' * This also attempts to identify text that is indented under a hanging indent.</p>
        ' * <p>
        ' * This method sets the isParagraphStart and isHangingIndent flags on the current
        ' * position object.</p>
        ' *
        ' * @param position the current text position.  This may have its isParagraphStart
        ' * or isHangingIndent flags set upon return.
        ' * @param lastPosition the previous text position (should not be null).
        ' * @param lastLineStartPosition the last text position that followed a line
        ' *            separator. May be null.
        ' * @param maxHeightForLine max height for text positions since lasLineStartPosition.
        ' */
        Protected Sub isParagraphSeparation(ByVal position As PositionWrapper, ByVal lastPosition As PositionWrapper, ByVal lastLineStartPosition As PositionWrapper, ByVal maxHeightForLine As Single)
            Dim result As Boolean = False
            If (lastLineStartPosition Is Nothing) Then
                result = True
            Else
                Dim yGap As Single = Math.Abs(position.getTextPosition().getYDirAdj() - lastPosition.getTextPosition().getYDirAdj())
                Dim xGap As Single = (position.getTextPosition().getXDirAdj() - lastLineStartPosition.getTextPosition().getXDirAdj()) 'do we need to flip this for rtl?
                If (yGap > (getDropThreshold() * maxHeightForLine)) Then
                    result = True
                ElseIf (xGap > (getIndentThreshold() * position.getTextPosition().getWidthOfSpace())) Then
                    'text is indented, but try to screen for hanging indent
                    If (Not lastLineStartPosition.isParagraphStart()) Then
                        result = True
                    Else
                        position.setHangingIndent()
                    End If
                ElseIf (xGap < -position.getTextPosition().getWidthOfSpace()) Then
                    'text is left of previous line. Was it a hanging indent?
                    If (Not lastLineStartPosition.isParagraphStart()) Then
                        result = True
                    End If
                ElseIf (Math.Abs(xGap) < (0.25 * position.getTextPosition().getWidth())) Then
                    'current horizontal position is within 1/4 a char of the last
                    'linestart.  We'll treat them as lined up.
                    If (lastLineStartPosition.isHangingIndent()) Then
                        position.setHangingIndent()
                    ElseIf (lastLineStartPosition.isParagraphStart()) Then
                        'check to see if the previous line looks like
                        'any of a number of standard list item formats
                        Dim liPattern As Pattern = matchListItemPattern(lastLineStartPosition)
                        If (liPattern IsNot Nothing) Then
                            Dim currentPattern As Pattern = matchListItemPattern(position)
                            If (liPattern Is currentPattern) Then
                                result = True
                            End If
                        End If
                    End If
                End If
            End If
            If (result) Then
                position.setParagraphStart()
            End If
        End Sub

        '/**
        ' * writes the paragraph separator string to the output.
        ' * @throws IOException if something went wrong
        ' */
        Protected Sub writeParagraphSeparator() 'throws IOException
            writeParagraphEnd()
            writeParagraphStart()
        End Sub

        '/**
        ' * Write something (if defined) at the start of a paragraph.
        ' * @throws IOException if something went wrong
        ' */
        Protected Sub writeParagraphStart() 'throws IOException
            If (inParagraph) Then
                writeParagraphEnd()
                inParagraph = False
            End If
            output.write(getParagraphStart())
            inParagraph = True
        End Sub

        '/**
        ' * Write something (if defined) at the end of a paragraph.
        ' * @throws IOException if something went wrong
        ' */
        Protected Sub writeParagraphEnd() 'throws IOException
            output.write(getParagraphEnd())
            inParagraph = False
        End Sub

        '/**
        ' * Write something (if defined) at the start of a page.
        ' * @throws IOException if something went wrong
        ' */
        Protected Sub writePageStart() 'throws IOException
            output.write(getPageStart())
        End Sub

        '/**
        ' * Write something (if defined) at the end of a page.
        ' * @throws IOException if something went wrong
        ' */
        Protected Sub writePageEnd() 'throws IOException
            output.write(getPageEnd())
        End Sub

        '/**
        ' * returns the list item Pattern object that matches
        ' * the text at the specified PositionWrapper or null
        ' * if the text does not match such a pattern.  The list
        ' * of Patterns tested against is given by the
        ' * {@link #getListItemPatterns()} method.  To add to
        ' * the list, simply override that method (if sub-classing)
        ' * or explicitly supply your own list using
        ' * {@link #setListItemPatterns(List)}.
        ' * @param pw position
        ' * @return the matching pattern
        ' */
        Protected Function matchListItemPattern(ByVal pw As PositionWrapper) As Pattern
            Dim tp As TextPosition = pw.getTextPosition()
            Dim txt As String = tp.getCharacter()
            Return matchPattern(txt, getListItemPatterns())
        End Function

        '/**
        ' * a list of regular expressions that match commonly used
        ' * list item formats, i.e. bullets, numbers, letters,
        ' * Roman numerals, etc.  Not meant to be
        ' * comprehensive.
        ' */
        Private Shared ReadOnly LIST_ITEM_EXPRESSIONS As String() = { _
                "\.", _
                "\d+\.", _
                "\[\d+\]", _
                "\d+\)", _
                "[A-Z]\.", _
                "[a-z]\.", _
                "[A-Z]\)", _
                "[a-z]\)", _
                "[IVXL]+\.", _
                "[ivxl]+\." _
                }

        Private listOfPatterns As List(Of Pattern) = Nothing
        '/**
        ' * use to supply a different set of regular expression
        ' * patterns for matching list item starts.
        ' *
        ' * @param patterns list of patterns
        ' */
        Protected Sub setListItemPatterns(ByVal patterns As List(Of Pattern))
            listOfPatterns = patterns
        End Sub

        '/**
        ' * returns a list of regular expression Patterns representing
        ' * different common list item formats.  For example
        ' * numbered items of form:
        ' * <ol>
        ' * <li>some text</li>
        ' * <li>more text</li>
        ' * </ol>
        ' * or
        ' * <ul>
        ' * <li>some text</li>
        ' * <li>more text</li>
        ' * </ul>
        ' * etc., all begin with some character pattern. The pattern "\\d+\." (matches "1.", "2.", ...)
        ' * or "\[\\d+\]" (matches "(1)", "(2)", ...).
        ' * <p>
        ' * This method returns a list of such regular expression Patterns.
        ' * @return a list of Pattern objects.
        ' */
        Protected Function getListItemPatterns() As List(Of Pattern)
            If (listOfPatterns Is Nothing) Then
                listOfPatterns = New ArrayList(Of Pattern)()
                For Each expression As String In LIST_ITEM_EXPRESSIONS
                    Dim p As Pattern = Pattern.compile(expression)
                    listOfPatterns.add(p)
                Next
            End If
            Return listOfPatterns
        End Function

        '/**
        ' * iterates over the specified list of Patterns until
        ' * it finds one that matches the specified string.  Then
        ' * returns the Pattern.
        ' * <p>
        ' * Order of the supplied list of patterns is important as
        ' * most common patterns should come first.  Patterns
        ' * should be strict in general, and all will be
        ' * used with case sensitivity on.
        ' * </p>
        ' * @param string the string to be searched 
        ' * @param patterns list of patterns
        ' * @return matching pattern
        ' */
        Protected Shared Function matchPattern(ByVal [string] As String, ByVal patterns As List(Of Pattern)) As Pattern
            'Dim matchedPattern As Pattern = Nothing
            For Each p As Pattern In patterns
                If (p.matcher([string]).matches()) Then
                    Return p
                End If
            Next
            Return Nothing ' matchedPattern
        End Function

        '/**
        ' * Write a list of string containing a whole line of a document.
        ' * @param line a list with the words of the given line
        ' * @param isRtlDominant determines if rtl or ltl is dominant
        ' * @throws IOException if something went wrong
        ' */
        Private Sub writeLine(ByVal line As List(Of WordWithTextPositions), ByVal isRtlDominant As Boolean) 'throws IOException
            Dim numberOfStrings As Integer = line.size()
            For i As Integer = 0 To numberOfStrings - 1
                Dim word As WordWithTextPositions = line.get(i)
                writeString(word.getText(), word.getTextPositions())
                If (i < numberOfStrings - 1) Then
                    writeWordSeparator()
                End If
            Next
        End Sub

        '/**
        ' * Normalize the given list of TextPositions.
        ' * @param line list of TextPositions
        ' * @param isRtlDominant determines if rtl or ltl is dominant 
        ' * @param hasRtl determines if lines contains rtl formatted text(parts)
        ' * @return a list of strings, one string for every word
        ' */
        Private Function normalize(ByVal line As List(Of TextPosition), ByVal isRtlDominant As Boolean, ByVal hasRtl As Boolean) As List(Of WordWithTextPositions)
            Dim normalized As LinkedList(Of WordWithTextPositions) = New LinkedList(Of WordWithTextPositions)()
            Dim lineBuilder As StringBuilder = New StringBuilder()
            Dim wordPositions As List(Of TextPosition) = New ArrayList(Of TextPosition)()
            ' concatenate the pieces of text in opposite order if RTL is dominant
            If (isRtlDominant) Then
                Dim numberOfPositions As Integer = line.size()
                For i As Integer = numberOfPositions - 1 To 0 Step -1
                    lineBuilder = normalizeAdd(normalized, lineBuilder, wordPositions, line.get(i))
                Next
            Else
                For Each text As TextPosition In line
                    lineBuilder = normalizeAdd(normalized, lineBuilder, wordPositions, text)
                Next
            End If
            If (lineBuilder.Length() > 0) Then
                normalized.add(createWord(lineBuilder.ToString(), wordPositions))
            End If
            Return normalized
        End Function

        '/**
        ' * Used within {@link #normalize(List, boolean, boolean)} to create a single {@link WordWithTextPositions}
        ' * entry.
        ' */
        Private Function createWord(ByVal word As String, ByVal wordPositions As List(Of TextPosition)) As WordWithTextPositions
            Return New WordWithTextPositions(Me._normalize.normalizePres(word), wordPositions)
        End Function

        '/**
        ' * Used within {@link #normalize(List, boolean, boolean)} to handle a {@link TextPosition}.
        ' * @return The StringBuilder that must be used when calling this method.
        ' */
        Private Function normalizeAdd(ByVal normalized As LinkedList(Of WordWithTextPositions), ByVal lineBuilder As StringBuilder, ByVal wordPositions As List(Of TextPosition), ByVal text As TextPosition) As StringBuilder
            If (TypeOf (text) Is WordSeparator) Then
                normalized.add(createWord(lineBuilder.ToString(), wordPositions))
                lineBuilder = New StringBuilder()
                wordPositions.clear()
            Else
                lineBuilder.Append(text.getCharacter())
                wordPositions.add(text)
            End If
            Return lineBuilder
        End Function

        '/**
        ' * internal marker class.  Used as a place holder in
        ' * a line of TextPositions.
        ' * @author ME21969
        ' *
        ' */
        Private Class WordSeparator
            Inherits TextPosition

            Private Shared ReadOnly separator As WordSeparator = New WordSeparator()

            Public Sub New()
            End Sub

            Public Shared Function getSeparator() As WordSeparator
                Return separator
            End Function
        End Class

        '/**
        ' * Internal class that maps strings to lists of {@link TextPosition} arrays.
        ' * Note that the number of entries in that list may differ from the number of characters in the
        ' * string due to normalization.
        ' *
        ' * @author Axel Dörfler
        ' */
        Private Class WordWithTextPositions

            Protected text As String
            Protected textPositions As List(Of TextPosition)

            Public Sub New(ByVal word As String, ByVal positions As List(Of TextPosition))
                Me.text = word
                Me.textPositions = positions
            End Sub

            Public Function getText() As String
                Return text
            End Function

            Public Function getTextPositions() As List(Of TextPosition)
                Return textPositions
            End Function
        End Class

    End Class


End Namespace
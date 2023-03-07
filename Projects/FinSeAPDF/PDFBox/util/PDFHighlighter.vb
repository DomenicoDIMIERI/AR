Imports System.IO
Imports FinSeA.Io
Imports FinSeA.org.apache.pdfbox.pdmodel
Imports FinSeA.org.apache.pdfbox
Imports FinSeA.Text

Namespace org.apache.pdfbox.util

    '/**
    ' * Highlighting of words in a PDF document with an XML file.
    ' *
    ' * @author slagraulet (slagraulet@cardiweb.com)
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.7 $
    ' *
    ' * @see <a href="http://partners.adobe.com/public/developer/en/pdf/HighlightFileFormat.pdf">
    ' *      Adobe Highlight File Format</a>
    ' */
    Public Class PDFHighlighter
        Inherits PDFTextStripper

        Private highlighterOutput As StreamWriter = Nothing
        'private Color highlightColor = Color.YELLOW;

        Private searchedWords() As String
        Private textOS As ByteArrayOutputStream = Nothing '
        Private textWriter As OutputStreamWriter = Nothing
        Private Const ENCODING As String = "UTF-16"

        '/**
        ' * Default constructor.
        ' *
        ' * @throws IOException If there is an error constructing this class.
        ' */
        Public Sub New() 'throws IOException
            MyBase.New(ENCODING)
            MyBase.setLineSeparator("")
            MyBase.setPageSeparator("")
            MyBase.setWordSeparator("")
            MyBase.setShouldSeparateByBeads(False)
            MyBase.setSuppressDuplicateOverlappingText(False)
        End Sub

        '/**
        ' * Generate an XML highlight string based on the PDF.
        ' *
        ' * @param pdDocument The PDF to find words in.
        ' * @param highlightWord The word to search for.
        ' * @param xmlOutput The resulting output xml file.
        ' *
        ' * @throws IOException If there is an error reading from the PDF, or writing to the XML.
        ' */
        Public Sub generateXMLHighlight(ByVal pdDocument As PDDocument, ByVal highlightWord As String, ByVal xmlOutput As StreamWriter) ' throws IOException
            generateXMLHighlight(pdDocument, {highlightWord}, xmlOutput)
        End Sub

        '/**
        ' * Generate an XML highlight string based on the PDF.
        ' *
        ' * @param pdDocument The PDF to find words in.
        ' * @param sWords The words to search for.
        ' * @param xmlOutput The resulting output xml file.
        ' *
        ' * @throws IOException If there is an error reading from the PDF, or writing to the XML.
        ' */
        Public Sub generateXMLHighlight(ByVal pdDocument As PDDocument, ByVal sWords() As String, ByVal xmlOutput As StreamWriter) ' throws IOException
            highlighterOutput = xmlOutput
            searchedWords = sWords
            '//color and mode are not implemented by the highlight spec
            '                    //so don't include them for now
            '                    //" color=#" + getHighlightColorAsString() +
            '                    //" mode=active " + */
            highlighterOutput.Write("<XML>\n<Body units=characters  version=2>\n<Highlight>\n")
            textOS = New ByteArrayOutputStream()
            textWriter = New OutputStreamWriter(textOS, ENCODING)
            writeText(pdDocument, textWriter)
            highlighterOutput.Write("</Highlight>\n</Body>\n</XML>")
            highlighterOutput.Flush()
        End Sub

        '/**
        ' * {@inheritDoc}
        ' */
        Protected Overrides Sub endPage(ByVal pdPage As PDPage) ' throws IOException
            textWriter.flush()

            Dim page As String = Sistema.Strings.GetString(textOS.toByteArray(), ENCODING)
            textOS.reset()
            '//page = page.replaceAll( "\n", "" );
            '//page = page.replaceAll( "\r", "" );
            '//page = CCRStringUtil.stripChar(page, '\n');
            '//page = CCRStringUtil.stripChar(page, '\r');

            '// Traitement des listes ï¿½ puces (caractï¿½res spï¿½ciaux)
            If (page.IndexOf("a") <> -1) Then
                page = page.Replace("a[0-9]{1,3}", ".")
            End If

            For i As Integer = 0 To searchedWords.Length - 1
                Dim pattern As Pattern = FinSeA.Text.Pattern.compile(searchedWords(i), pattern.CASE_INSENSITIVE)
                Dim matcher As Matcher = pattern.matcher(page)
                While (matcher.find())
                    Dim begin As Integer = matcher.start()
                    Dim [end] As Integer = matcher.end()
                    highlighterOutput.Write("    <loc pg=" & (getCurrentPageNo() - 1) & " pos=" & begin & " len=" & ([end] - begin) & ">\n")
                End While
            Next
        End Sub

        ''/**
        '' * Command line application.
        '' *
        '' * @param args The command line arguments to the application.
        '' *
        '' * @throws IOException If there is an error generating the highlight file.
        '' */
        'Public Shared Sub main(ByVal args() As String) 'throws IOException
        '    Dim xmlExtractor As New PDFHighlighter()
        '    Dim doc As PDDocument = Nothing
        '    Try
        '        If (args.Length < 2) Then
        '            usage()
        '        End If
        '        Dim highlightStrings() As String = Array.CreateInstance(GetType(String), args.Length - 1)
        '        Array.Copy(args, 1, highlightStrings, 0, highlightStrings.Length)
        '        doc = PDDocument.load(args(0))

        '        xmlExtractor.generateXMLHighlight(doc, highlightStrings, New StreamWriter(System.out))
        '    Finally
        '        If (doc IsNot Nothing) Then
        '            doc.close()
        '        End If
        '    End Try
        'End Sub

        'Private Shared Sub usage()
        '    LOG.info("usage: java " & PDFHighlighter.class.getName() & " <pdf file> word1 word2 word3 ...")
        '    Stop '( 1 );
        'End Sub


        '/**
        ' * Get the color to highlight the strings with.  Default is Color.YELLOW.
        ' *
        ' * @return The color to highlight strings with.
        ' */
        '/*public Color getHighlightColor()
        '{
        '    return highlightColor;
        '}**/

        '/**
        ' * Get the color to highlight the strings with.  Default is Color.YELLOW.
        ' *
        ' * @param color The color to highlight strings with.
        ' */
        '/*public void setHighlightColor(Color color)
        '{
        '    Me.highlightColor = color;
        '}**/

        '/**
        ' * Set the highlight color using HTML like rgb string.  The string must be 6 characters long.
        ' *
        ' * @param color The color to use for highlighting.  Should be in the format of "FF0000".
        ' */
        '/*public void setHighlightColor( String color )
        '{
        '    highlightColor = Color.decode( color );
        '}**/

        '/**
        ' * Get the highlight color as an HTML like string.  This will return a string of six characters.
        ' *
        ' * @return The current highlight color.  For example FF0000
        ' */
        '/*public String getHighlightColorAsString()
        '{
        '    //BJL: kudos to anyone that has a cleaner way of doing this!
        '    String red = Integer.toHexString( highlightColor.getRed() );
        '    String green = Integer.toHexString( highlightColor.getGreen() );
        '    String blue = Integer.toHexString( highlightColor.getBlue() );

        '    return (red.length() < 2 ? "0" + red : red) +
        '           (green.length() < 2 ? "0" + green : green) +
        '           (blue.length() < 2 ? "0" + blue : blue);
        '}**/

    End Class

End Namespace
Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.exceptions
Imports FinSeA.org.apache.pdfbox.pdfparser
Imports FinSeA.org.apache.pdfbox.pdmodel
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.pdmodel.font
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.color
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.xobject
Imports FinSeA.org.apache.pdfbox.util.operator

Namespace org.apache.pdfbox.util

    '/**
    ' * This class will run through a PDF content stream and execute certain operations
    ' * and provide a callback interface for clients that want to do things with the stream.
    ' * See the PDFTextStripper class for an example of how to use this class.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.38 $
    ' */
    Public Class PDFStreamEngine


        '/**
        ' * Log instance.
        ' */
        'private static final Log LOG = LogFactory.getLog(PDFStreamEngine.class);

        '/**
        ' * The PDF operators that are ignored by this engine.
        ' */
        Private unsupportedOperators As [Set](Of String) = New HashSet(Of String)()

        Private graphicsState As PDGraphicsState = Nothing

        Private textMatrix As Matrix = Nothing
        Private textLineMatrix As Matrix = Nothing
        Private graphicsStack As Stack(Of PDGraphicsState) = New Stack(Of PDGraphicsState)()

        Private operators As Map(Of String, OperatorProcessor) = New HashMap(Of String, OperatorProcessor)()

        Private streamResourcesStack As Stack(Of PDResources) = New Stack(Of PDResources)()

        Private page As PDPage

        Private validCharCnt As Integer
        Private totalCharCnt As Integer

        ''' <summary>
        ''' Flag to skip malformed or otherwise unparseable input where possible.
        ''' </summary>
        ''' <remarks></remarks>
        Private forceParsing As Boolean = False

        '/**
        ' * Constructor.
        ' */
        Public Sub New()
            'default constructor
            validCharCnt = 0
            totalCharCnt = 0
        End Sub

        '/**
        ' * Constructor with engine properties.  The property keys are all
        ' * PDF operators, the values are class names used to execute those
        ' * operators. An empty value means that the operator will be silently
        ' * ignored.
        ' *
        ' * @param properties The engine properties.
        ' *
        ' * @throws IOException If there is an error setting the engine properties.
        ' */
        Public Sub New(ByVal properties As Properties) 'throws IOException
            If (properties Is Nothing) Then
                Throw New NullReferenceException("properties cannot be null")
            End If
            'Enumeration<?> names = properties.propertyNames();
            'for ( Object name : Collections.list( names ) )
            For Each name As Object In properties.propertyNames
                Dim [operator] As String = name.ToString()
                Dim processorClassName As String = properties.getProperty([operator])
                If ("".Equals(processorClassName)) Then
                    unsupportedOperators.add([operator])
                Else
                    Try
                        Dim klass As System.Type = Sistema.Types.GetType(processorClassName)
                        Dim processor As OperatorProcessor = Sistema.Types.CreateInstance(klass)
                        registerOperatorProcessor([operator], processor)
                    Catch e As Exception
                        Throw New WrappedIOException("OperatorProcessor class " & processorClassName & " could not be instantiated", e)
                    End Try
                End If
            Next
            validCharCnt = 0
            totalCharCnt = 0
        End Sub

        '/**
        ' * Indicates if force parsing is activated.
        ' * 
        ' * @return true if force parsing is active
        ' */
        Public Function isForceParsing() As Boolean
            Return forceParsing
        End Function

        '/**
        ' * Enable/Disable force parsing.
        ' * 
        ' * @param forceParsingValue true activates force parsing
        ' */
        Public Sub setForceParsing(ByVal forceParsingValue As Boolean)
            forceParsing = forceParsingValue
        End Sub

        '/**
        ' * Register a custom operator processor with the engine.
        ' *
        ' * @param operator The operator as a string.
        ' * @param op Processor instance.
        ' */
        Public Sub registerOperatorProcessor(ByVal [operator] As String, ByVal op As OperatorProcessor)
            op.setContext(Me)
            operators.put([operator], op)
        End Sub

        '/**
        ' * This method must be called between processing documents.  The
        ' * PDFStreamEngine caches information for the document between pages
        ' * and this will release the cached information.  This only needs
        ' * to be called if processing a new document.
        ' *
        ' */
        Public Overridable Sub resetEngine()
            validCharCnt = 0
            totalCharCnt = 0
        End Sub

        '/**
        ' * This will process the contents of the stream.
        ' *
        ' * @param aPage The page.
        ' * @param resources The location to retrieve resources.
        ' * @param cosStream the Stream to execute.
        ' *
        ' *
        ' * @throws IOException if there is an error accessing the stream.
        ' */
        Public Sub processStream(ByVal aPage As PDPage, ByVal resources As PDResources, ByVal cosStream As COSStream)  'throws IOException
            graphicsState = New PDGraphicsState(aPage.findCropBox())
            textMatrix = Nothing
            textLineMatrix = Nothing
            graphicsStack.clear()
            streamResourcesStack.clear()
            processSubStream(aPage, resources, cosStream)
        End Sub

        '/**
        ' * Process a sub stream of the current stream.
        ' *
        ' * @param aPage The page used for drawing.
        ' * @param resources The resources used when processing the stream.
        ' * @param cosStream The stream to process.
        ' *
        ' * @throws IOException If there is an exception while processing the stream.
        ' */
        Public Sub processSubStream(ByVal aPage As PDPage, ByVal resources As PDResources, ByVal cosStream As COSStream)  'throws IOException 
            page = aPage
            If (Resources IsNot Nothing) Then
                streamResourcesStack.push(resources)
                Try
                    processSubStream(cosStream)
                Finally
                    streamResourcesStack.pop().clear()
                End Try
            Else
                processSubStream(cosStream)
            End If
        End Sub

        Private Sub processSubStream(ByVal cosStream As COSStream)  'throws IOException 
            Dim arguments As List(Of COSBase) = New ArrayList(Of COSBase)()
            Dim parser As PDFStreamParser = New PDFStreamParser(cosStream, forceParsing)
            Try
                Dim iter As Global.System.Collections.Generic.IEnumerator(Of Object) = parser.getTokenIterator()
                '   While (iter.hasNext())
                'For Each [next] As Object In parser.getTokenIterator
                While (iter.MoveNext)
                    Dim [next] As Object = iter.Current
                    If (LOG.isDebugEnabled()) Then
                        LOG.debug("processing substream token: " & [next].toString)
                    End If
                    If (TypeOf ([next]) Is COSObject) Then
                        arguments.add(DirectCast([next], COSObject).getObject())
                    ElseIf (TypeOf ([next]) Is PDFOperator) Then
                        processOperator(DirectCast([next], PDFOperator), arguments)
                        arguments = New ArrayList(Of COSBase)()
                    Else
                        arguments.add(DirectCast([next], COSBase))
                    End If
                End While
            Finally
                parser.close()
            End Try
        End Sub


        '/**
        ' * A method provided as an event interface to allow a subclass to perform
        ' * some specific functionality when text needs to be processed.
        ' *
        ' * @param text The text to be processed.
        ' */
        Protected Overridable Sub processTextPosition(ByVal text As TextPosition)
            'subclasses can override to provide specific functionality.
        End Sub

        '/**
        ' * A method provided as an event interface to allow a subclass to perform
        ' * some specific functionality on the string encoded by a glyph.
        ' *
        ' * @param str The string to be processed.
        ' */
        Public Overridable Function inspectFontEncoding(ByVal str As String) As String
            Return str
        End Function

        '/**
        ' * Process encoded text from the PDF Stream. 
        ' * You should override this method if you want to perform an action when 
        ' * encoded text is being processed.
        ' *
        ' * @param string The encoded text
        ' *
        ' * @throws IOException If there is an error processing the string
        ' */
        Public Sub processEncodedText(ByVal [string] As Byte()) 'throws IOException
            '/* Note on variable names.  There are three different units being used
            ' * in this code.  Character sizes are given in glyph units, text locations
            ' * are initially given in text units, and we want to save the data in 
            ' * display units. The variable names should end with Text or Disp to 
            ' * represent if the values are in text or disp units (no glyph units are saved).
            ' */
            Dim fontSizeText As Single = graphicsState.getTextState().getFontSize()
            Dim horizontalScalingText As Single = graphicsState.getTextState().getHorizontalScalingPercent() / 100.0F
            'Single verticalScalingText = horizontalScaling;//not sure if this is right but what else to do???
            Dim riseText As Single = graphicsState.getTextState().getRise()
            Dim wordSpacingText As Single = graphicsState.getTextState().getWordSpacing()
            Dim characterSpacingText As Single = graphicsState.getTextState().getCharacterSpacing()

            '//We won't know the actual number of characters until
            '//we process the byte data(could be two bytes each) but
            '//it won't ever be more than string.length*2(there are some cases
            '//were a single byte will result in two output characters "fi"

            Dim font As PDFont = graphicsState.getTextState().getFont()
            ' all fonts are providing the width/height of a character in thousandths of a unit of text space
            Dim fontMatrixXScaling As Single = 1 / 1000.0F
            Dim fontMatrixYScaling As Single = 1 / 1000.0F
            Dim glyphSpaceToTextSpaceFactor As Single = 1 / 1000.0F
            ' expect Type3 fonts, those are providing the width of a character in glyph space units
            If (TypeOf (font) Is PDType3Font) Then
                Dim fontMatrix As PDMatrix = font.getFontMatrix()
                fontMatrixXScaling = fontMatrix.getValue(0, 0)
                fontMatrixYScaling = fontMatrix.getValue(1, 1)
                'This will typically be 1000 but in the case of a type3 font
                'this might be a different number
                glyphSpaceToTextSpaceFactor = 1.0F / fontMatrix.getValue(0, 0)
            End If
            Dim spaceWidthText As Single = 0
            Try
                ' to avoid crash as described in PDFBOX-614
                ' lets see what the space displacement should be
                spaceWidthText = (font.getSpaceWidth() * glyphSpaceToTextSpaceFactor)
            Catch exception As System.Exception
                LOG.warn(exception.Message, exception)
            End Try

            If (spaceWidthText = 0) Then
                spaceWidthText = (font.getAverageFontWidth() * glyphSpaceToTextSpaceFactor)
                'The average space width appears to be higher than necessary
                'so lets make it a little bit smaller.
                spaceWidthText *= 0.800000012F
            End If

            Dim maxVerticalDisplacementText As Single = 0

            Dim textStateParameters As Matrix = New Matrix()
            textStateParameters.setValue(0, 0, fontSizeText * horizontalScalingText)
            textStateParameters.setValue(1, 1, fontSizeText)
            textStateParameters.setValue(2, 1, riseText)

            Dim pageRotation As Integer = page.findRotation()
            Dim pageHeight As Single = page.findCropBox().getHeight()
            Dim pageWidth As Single = page.findCropBox().getWidth()

            Dim ctm As Matrix = getGraphicsState().getCurrentTransformationMatrix()
            Dim textXctm As Matrix = New Matrix()
            Dim textMatrixEnd As Matrix = New Matrix()
            Dim td As Matrix = New Matrix()
            Dim tempMatrix As Matrix = New Matrix()

            Dim codeLength As Integer = 1
            For i As Integer = 0 To [string].Length - 1 Step codeLength
                ' Decode the value to a Unicode character
                codeLength = 1
                Dim c As String = font.encode([string], i, codeLength)
                Dim codePoints() As Integer = Nothing
                If (c Is Nothing AndAlso i + 1 < [string].Length) Then
                    'maybe a multibyte encoding
                    codeLength += 1
                    c = font.encode([string], i, codeLength)
                    codePoints = New Integer() {font.getCodeFromArray([string], i, codeLength)}
                End If

                ' the space width has to be transformed into display units
                Dim spaceWidthDisp As Single = spaceWidthText * fontSizeText * horizontalScalingText * textMatrix.getValue(0, 0) * ctm.getValue(0, 0)

                'todo, handle horizontal displacement
                ' get the width and height of this character in text units 
                Dim characterHorizontalDisplacementText As Single = font.getFontWidth([string], i, codeLength)
                Dim characterVerticalDisplacementText As Single = font.getFontHeight([string], i, codeLength)

                ' multiply the width/height with the scaling factor
                characterHorizontalDisplacementText = characterHorizontalDisplacementText * fontMatrixXScaling
                characterVerticalDisplacementText = characterVerticalDisplacementText * fontMatrixYScaling

                maxVerticalDisplacementText = Math.Max(maxVerticalDisplacementText, characterVerticalDisplacementText)

                '// PDF Spec - 5.5.2 Word Spacing
                '//
                '// Word spacing works the same was as character spacing, but applies
                '// only to the space character, code 32.
                '//
                '// Note: Word spacing is applied to every occurrence of the single-byte
                '// character code 32 in a string.  This can occur when using a simple
                '// font or a composite font that defines code 32 as a single-byte code.
                '// It does not apply to occurrences of the byte value 32 in multiple-byte
                '// codes.
                '//
                '// RDD - My interpretation of this is that only character code 32's that
                '// encode to spaces should have word spacing applied.  Cases have been
                '// observed where a font has a space character with a character code
                '// other than 32, and where word spacing (Tw) was used.  In these cases,
                '// applying word spacing to either the non-32 space or to the character
                '// code 32 non-space resulted in errors consistent with this interpretation.
                '//
                Dim spacingText As Single = 0
                If (([string](i) = &H20) AndAlso codeLength = 1) Then
                    spacingText += wordSpacingText
                End If
                textXctm = textMatrix.multiply(ctm, textXctm)
                '// Convert textMatrix to display units
                '// We need to instantiate a new Matrix instance here as it is passed to the TextPosition constructor below.
                Dim textMatrixStart As Matrix = textStateParameters.multiply(textXctm)

                '// TODO : tx should be set for horizontal text and ty for vertical text
                '// which seems to be specified in the font (not the direction in the matrix).
                Dim tx As Single = ((characterHorizontalDisplacementText) * fontSizeText) * horizontalScalingText
                Dim ty As Single = 0
                ' reset the matrix instead of creating a new one
                td.reset()
                td.setValue(2, 0, tx)
                td.setValue(2, 1, ty)

                '// The text matrix gets updated after each glyph is placed.  The updated
                '// version will have the X and Y coordinates for the next glyph.
                '// textMatrixEnd contains the coordinates of the end of the last glyph without 
                '// taking characterSpacingText and spacintText into account, otherwise it'll be
                '// impossible to detect new words within text extraction
                tempMatrix = textStateParameters.multiply(td, tempMatrix)
                textMatrixEnd = tempMatrix.multiply(textXctm, textMatrixEnd)
                Dim endXPosition As Single = textMatrixEnd.getXPosition()
                Dim endYPosition As Single = textMatrixEnd.getYPosition()

                ' add some spacing to the text matrix (see comment above)
                tx = ((characterHorizontalDisplacementText) * fontSizeText + characterSpacingText + spacingText) * horizontalScalingText
                td.setValue(2, 0, tx)
                textMatrix = td.multiply(textMatrix, textMatrix)

                ' determine the width of this character
                ' XXX: Note that if we handled vertical text, we should be using Y here
                Dim startXPosition As Single = textMatrixStart.getXPosition()
                Dim widthText As Single = endXPosition - startXPosition

                '//there are several cases where one character code will
                '//output multiple characters.  For example "fi" or a
                '//glyphname that has no mapping like "visiblespace"
                If (c IsNot Nothing) Then
                    validCharCnt += 1
                Else
                    ' PDFBOX-373: Replace a null entry with "?" so it is
                    ' not printed as "(null)"
                    c = "?"
                End If
                totalCharCnt += 1

                Dim totalVerticalDisplacementDisp As Single = maxVerticalDisplacementText * fontSizeText * textXctm.getYScale()

                ' process the decoded text
                processTextPosition(New TextPosition(pageRotation, pageWidth, pageHeight, textMatrixStart, endXPosition, endYPosition, totalVerticalDisplacementDisp, widthText, spaceWidthDisp, c, codePoints, font, fontSizeText, CInt(fontSizeText * textMatrix.getXScale())))
            Next
        End Sub

        '/**
        ' * This is used to handle an operation.
        ' *
        ' * @param operation The operation to perform.
        ' * @param arguments The list of arguments.
        ' *
        ' * @throws IOException If there is an error processing the operation.
        ' */
        Public Overridable Sub processOperator(ByVal operation As String, ByVal arguments As List(Of COSBase)) 'throws IOException
            Try
                Dim oper As PDFOperator = PDFOperator.getOperator(operation)
                processOperator(oper, arguments)
            Catch e As IOException
                LOG.warn(e.Message, e)
            End Try
        End Sub

        '/**
        ' * This is used to handle an operation.
        ' *
        ' * @param operator The operation to perform.
        ' * @param arguments The list of arguments.
        ' *
        ' * @throws IOException If there is an error processing the operation.
        ' */
        Protected Overridable Sub processOperator(ByVal [operator] As PDFOperator, ByVal arguments As List(Of COSBase)) ' throws IOException
            Try
                Dim operation As String = [operator].getOperation()
                Dim processor As OperatorProcessor = operators.get(operation)
                If (processor IsNot Nothing) Then
                    processor.setContext(Me)
                    processor.process([operator], arguments)
                Else
                    If (Not unsupportedOperators.contains(operation)) Then
                        LOG.info("unsupported/disabled operation: " & operation)
                        unsupportedOperators.add(operation)
                    End If
                End If
            Catch e As Exception
                LOG.warn(e.Message, e)
            End Try
        End Sub

        '/**
        ' * @return Returns the colorSpaces.
        ' */
        Public Function getColorSpaces() As Map(Of String, PDColorSpace)
            Return streamResourcesStack.peek().getColorSpaces()
        End Function

        '/**
        ' * @return Returns the colorSpaces.
        ' */
        Public Function getXObjects() As Map(Of String, PDXObject)
            Return streamResourcesStack.peek().getXObjects()
        End Function

        '/**
        ' * @param value The colorSpaces to set.
        ' */
        Public Sub setColorSpaces(ByVal value As Map(Of String, PDColorSpace))
            streamResourcesStack.peek().setColorSpaces(value)
        End Sub

        '/**
        '    * @return Returns the fonts.
        '    */
        Public Function getFonts() As Map(Of String, PDFont)
            If (streamResourcesStack.isEmpty()) Then
                Return New HashMap(Of String, PDFont) ' Collections.emptyMap()
            End If

            Return streamResourcesStack.peek().getFonts()
        End Function

        '/**
        ' * @param value The fonts to set.
        ' */
        Public Sub setFonts(ByVal value As Map(Of String, PDFont))
            streamResourcesStack.peek().setFonts(value)
        End Sub

        '/**
        ' * @return Returns the graphicsStack.
        ' */
        Public Function getGraphicsStack() As Stack(Of PDGraphicsState)
            Return graphicsStack
        End Function

        '/**
        ' * @param value The graphicsStack to set.
        ' */
        Public Sub setGraphicsStack(ByVal value As Stack(Of PDGraphicsState))
            graphicsStack = value
        End Sub

        '/**
        '    * @return Returns the graphicsState.
        '    */
        Public Function getGraphicsState() As PDGraphicsState
            Return graphicsState
        End Function

        '/**
        ' * @param value The graphicsState to set.
        ' */
        Public Sub setGraphicsState(ByVal value As PDGraphicsState)
            graphicsState = value
        End Sub

        '/**
        ' * @return Returns the graphicsStates.
        ' */
        Public Function getGraphicsStates() As Map(Of String, PDExtendedGraphicsState)
            Return streamResourcesStack.peek().getGraphicsStates()
        End Function

        '/**
        ' * @param value The graphicsStates to set.
        ' */
        Public Sub setGraphicsStates(ByVal value As Map(Of String, PDExtendedGraphicsState))
            streamResourcesStack.peek().setGraphicsStates(value)
        End Sub

        '/**
        '    * @return Returns the textLineMatrix.
        '    */
        Public Function getTextLineMatrix() As Matrix
            Return textLineMatrix
        End Function

        '/**
        ' * @param value The textLineMatrix to set.
        ' */
        Public Sub setTextLineMatrix(ByVal value As Matrix)
            textLineMatrix = value
        End Sub

        '/**
        ' * @return Returns the textMatrix.
        ' */
        Public Function getTextMatrix() As Matrix
            Return textMatrix
        End Function

        '/**
        ' * @param value The textMatrix to set.
        ' */
        Public Sub setTextMatrix(ByVal value As Matrix)
            textMatrix = value
        End Sub

        '/**
        ' * @return Returns the resources.
        ' */
        Public Function getResources() As PDResources
            Return streamResourcesStack.peek()
        End Function

        '/**
        ' * Get the current page that is being processed.
        ' *
        ' * @return The page being processed.
        ' */
        Public Function getCurrentPage() As PDPage
            Return page
        End Function

        '/** 
        ' * Get the total number of valid characters in the doc 
        ' * that could be decoded in processEncodedText(). 
        ' * @return The number of valid characters. 
        ' */
        Public Function getValidCharCnt() As Integer
            Return validCharCnt
        End Function

        '/**
        ' * Get the total number of characters in the doc
        ' * (including ones that could not be mapped).  
        ' * @return The number of characters. 
        ' */
        Public Function getTotalCharCnt() As Integer
            Return totalCharCnt
        End Function

    End Class


End Namespace
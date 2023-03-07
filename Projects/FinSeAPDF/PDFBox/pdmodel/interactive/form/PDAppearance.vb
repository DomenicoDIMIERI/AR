Imports FinSeA.Io
Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdfparser
Imports FinSeA.org.apache.pdfbox.pdfwriter
Imports FinSeA.org.apache.pdfbox.pdmodel
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.pdmodel.font
Imports FinSeA.org.apache.pdfbox.pdmodel.interactive.action
Imports FinSeA.org.apache.pdfbox.pdmodel.interactive.annotation
Imports FinSeA.org.apache.pdfbox.util
Imports System.Text

Namespace org.apache.pdfbox.pdmodel.interactive.form

    '/**
    ' * This one took me a while, but i'm proud to say that it handles
    ' * the appearance of a textbox. This allows you to apply a value to
    ' * a field in the document and handle the appearance so that the
    ' * value is actually visible too.
    ' * The problem was described by Ben Litchfield, the author of the
    ' * example: org.apache.pdfbox.examlpes.fdf.ImportFDF. So Ben, here is the
    ' * solution.
    ' *
    ' * @author sug
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.20 $
    ' */
    Public Class PDAppearance

        Private parent As PDVariableText

        Private value As String
        Private defaultAppearance As COSString

        Private acroForm As PDAcroForm
        Private widgets As List(Of COSObjectable) = New ArrayList(Of COSObjectable)()


        '/**
        ' * Constructs a COSAppearnce from the given field.
        ' *
        ' * @param theAcroForm the acro form that this field is part of.
        ' * @param field the field which you wish to control the appearance of
        ' * @throws IOException If there is an error creating the appearance.
        ' */
        Public Sub New(ByVal theAcroForm As PDAcroForm, ByVal field As PDVariableText)  'throws IOException
            acroForm = theAcroForm
            parent = field

            widgets = field.getKids()
            If (widgets Is Nothing) Then
                widgets = New ArrayList(Of COSObjectable)()
                widgets.add(field.getWidget())
            End If

            defaultAppearance = getDefaultAppearance()
        End Sub


        '/**
        ' * Returns the default apperance of a textbox. If the textbox
        ' * does not have one, then it will be taken from the AcroForm.
        ' * @return The DA element
        ' */
        Private Function getDefaultAppearance() As COSString
            Dim dap As COSString = parent.getDefaultAppearance()
            If (dap Is Nothing) Then
                Dim kids As COSArray = parent.getDictionary().getDictionaryObject(COSName.KIDS)
                If (kids IsNot Nothing AndAlso kids.size() > 0) Then
                    Dim firstKid As COSDictionary = kids.getObject(0)
                    dap = firstKid.getDictionaryObject(COSName.DA)
                End If
                If (dap Is Nothing) Then
                    dap = acroForm.getDictionary().getDictionaryObject(COSName.DA)
                End If
            End If
            Return dap
        End Function

        Private Function getQ() As Integer
            Dim q As Integer = parent.getQ()
            If (parent.getDictionary().getDictionaryObject(COSName.Q) Is Nothing) Then
                Dim kids As COSArray = parent.getDictionary().getDictionaryObject(COSName.KIDS)
                If (kids IsNot Nothing AndAlso kids.size() > 0) Then
                    Dim firstKid As COSDictionary = kids.getObject(0)
                    Dim qNum As COSNumber = firstKid.getDictionaryObject(COSName.Q)
                    If (qNum IsNot Nothing) Then
                        q = qNum.intValue()
                    End If
                End If
            End If
            Return q
        End Function

        '/**
        ' * Extracts the original appearance stream into a list of tokens.
        ' *
        ' * @return The tokens in the original appearance stream
        ' */
        Private Function getStreamTokens(ByVal appearanceStream As PDAppearanceStream) As List  ' throws IOException
            Dim tokens As List = Nothing
            If (appearanceStream IsNot Nothing) Then
                tokens = getStreamTokens(appearanceStream.getStream())
            End If
            Return tokens
        End Function

        Private Function getStreamTokens(ByVal [string] As COSString) As List '  throws IOException
            Dim parser As PDFStreamParser

            Dim tokens As List = Nothing
            If ([string] IsNot Nothing) Then
                Dim stream As New ByteArrayInputStream([string].getBytes())
                parser = New PDFStreamParser(stream, acroForm.getDocument().getDocument().getScratchFile())
                parser.parse()
                tokens = parser.getTokens()
                stream.Dispose()
            End If
            Return tokens
        End Function

        Private Function getStreamTokens(ByVal stream As COSStream) As List ' throws IOException
            Dim parser As PDFStreamParser

            Dim tokens As List = Nothing
            If (stream IsNot Nothing) Then
                parser = New PDFStreamParser(stream)
                parser.parse()
                tokens = parser.getTokens()
            End If
            Return tokens
        End Function

        '/**
        ' * Tests if the apperance stream already contains content.
        ' *
        ' * @return true if it contains any content
        ' */
        Private Function containsMarkedContent(ByVal stream As List) As Boolean
            Return stream.contains(PDFOperator.getOperator("BMC"))
        End Function

        '/**
        ' * This is the public method for setting the appearance stream.
        ' *
        ' * @param apValue the String value which the apperance shoud represent
        ' *
        ' * @throws IOException If there is an error creating the stream.
        ' */
        Public Sub setAppearanceValue(ByVal apValue As String)  'throws IOException
            ' MulitLine check and set
            If (parent.isMultiline() AndAlso apValue.IndexOf(vbLf) <> -1) Then
                apValue = convertToMultiLine(apValue)
            End If

            value = apValue
            'Iterator(Of COSObjectable) widgetIter = widgets.iterator();
            '   While (widgetIter.hasNext())
            '{
            '   COSObjectable next = widgetIter.next();
            For Each [next] As COSObjectable In widgets
                Dim field As PDField = Nothing
                Dim widget As PDAnnotationWidget = Nothing
                If (TypeOf ([next]) Is PDField) Then
                    field = [next]
                    widget = field.getWidget()
                Else
                    widget = [next]
                End If
                Dim actions As PDFormFieldAdditionalActions = Nothing
                If (field IsNot Nothing) Then
                    actions = field.getActions()
                End If
                If (actions IsNot Nothing AndAlso actions.getF() IsNot Nothing AndAlso widget.getDictionary().getDictionaryObject(COSName.AP) Is Nothing) Then
                    'do nothing because the field will be formatted by acrobat
                    'when it is opened.  See FreedomExpressions.pdf for an example of Me.
                Else
                    Dim appearance As PDAppearanceDictionary = widget.getAppearance()
                    If (appearance Is Nothing) Then
                        appearance = New PDAppearanceDictionary()
                        widget.setAppearance(appearance)
                    End If

                    Dim normalAppearance As Map = appearance.getNormalAppearance()
                    Dim appearanceStream As PDAppearanceStream = normalAppearance.get("default")
                    If (appearanceStream Is Nothing) Then
                        Dim cosStream As COSStream = acroForm.getDocument().getDocument().createCOSStream()
                        appearanceStream = New PDAppearanceStream(cosStream)
                        appearanceStream.setBoundingBox(widget.getRectangle().createRetranslatedRectangle())
                        appearance.setNormalAppearance(appearanceStream)
                    End If

                    Dim tokens As List = getStreamTokens(appearanceStream)
                    Dim daTokens As List = getStreamTokens(getDefaultAppearance())
                    Dim pdFont As PDFont = getFontAndUpdateResources(tokens, appearanceStream)

                    If (Not containsMarkedContent(tokens)) Then
                        Dim output As New ByteArrayOutputStream()

                        'BJL 9/25/2004 Must prepend existing stream
                        'because it might have operators to draw things like
                        'rectangles and such
                        Dim writer As ContentStreamWriter = New ContentStreamWriter(output)
                        writer.writeTokens(tokens)
                        output.Write(Sistema.Strings.GetBytes(" /Tx BMC" & vbLf, "ISO-8859-1"))
                        insertGeneratedAppearance(widget, output, pdFont, tokens, appearanceStream)
                        output.Write(Sistema.Strings.GetBytes(" EMC", "ISO-8859-1"))
                        writeToStream(output.toByteArray(), appearanceStream)
                    Else
                        If (tokens IsNot Nothing) Then
                            If (daTokens IsNot Nothing) Then
                                Dim bmcIndex As Integer = tokens.indexOf(PDFOperator.getOperator("BMC"))
                                Dim emcIndex As Integer = tokens.indexOf(PDFOperator.getOperator("EMC"))
                                If (bmcIndex <> -1 AndAlso emcIndex <> -1 AndAlso emcIndex = bmcIndex + 1) Then
                                    'if the EMC immediately follows the BMC index then should
                                    'insert the daTokens inbetween the two markers.
                                    tokens.addAll(emcIndex, daTokens)
                                End If
                            End If
                            Dim output As New ByteArrayOutputStream()
                            Dim writer As ContentStreamWriter = New ContentStreamWriter(output)
                            Dim fontSize As Single = calculateFontSize(pdFont, appearanceStream.getBoundingBox(), tokens, Nothing)
                            Dim foundString As Boolean = False
                            For i As Integer = 0 To tokens.size() - 1
                                If (TypeOf (tokens.get(i)) Is COSString) Then
                                    foundString = True
                                    Dim drawnString As COSString = tokens.get(i)
                                    drawnString.reset()
                                    drawnString.append(Sistema.Strings.GetBytes(apValue, "ISO-8859-1"))
                                End If
                            Next
                            Dim setFontIndex As Integer = tokens.indexOf(PDFOperator.getOperator("Tf"))
                            tokens.set(setFontIndex - 1, New COSFloat(fontSize))
                            If (foundString) Then
                                writer.writeTokens(tokens)
                            Else
                                Dim bmcIndex As Integer = tokens.indexOf(PDFOperator.getOperator("BMC"))
                                Dim emcIndex As Integer = tokens.indexOf(PDFOperator.getOperator("EMC"))

                                If (bmcIndex <> -1) Then
                                    writer.writeTokens(tokens, 0, bmcIndex + 1)
                                Else
                                    writer.writeTokens(tokens)
                                End If
                                output.Write(Sistema.Strings.GetBytes(vbLf, "ISO-8859-1"))
                                insertGeneratedAppearance(widget, output, pdFont, tokens, appearanceStream)
                                If (emcIndex <> -1) Then
                                    writer.writeTokens(tokens, emcIndex, tokens.size())
                                End If
                            End If
                            writeToStream(output.toByteArray(), appearanceStream)
                        Else
                            'hmm?
                        End If
                    End If
                End If
            Next
        End Sub

        Private Sub insertGeneratedAppearance(ByVal fieldWidget As PDAnnotationWidget, ByVal output As OutputStream, ByVal pdFont As PDFont, ByVal tokens As List, ByVal appearanceStream As PDAppearanceStream)  'throws IOException
            Dim printWriter As PrintWriter = New PrintWriter(output, True)
            Dim fontSize As Single = 0.0F
            Dim boundingBox As PDRectangle = Nothing
            boundingBox = appearanceStream.getBoundingBox()
            If (boundingBox Is Nothing) Then
                boundingBox = fieldWidget.getRectangle().createRetranslatedRectangle()
            End If
            printWriter.println("BT")
            If (defaultAppearance IsNot Nothing) Then
                Dim daString As String = defaultAppearance.getString()
                Dim daParser As PDFStreamParser = New PDFStreamParser(New ByteArrayInputStream(Sistema.Strings.GetBytes(daString, "ISO-8859-1")), Nothing)
                daParser.parse()
                Dim daTokens As List(Of Object) = daParser.getTokens()
                fontSize = calculateFontSize(pdFont, boundingBox, tokens, daTokens)
                Dim fontIndex As Integer = daTokens.indexOf(PDFOperator.getOperator("Tf"))
                If (fontIndex <> -1) Then
                    daTokens.set(fontIndex - 1, New COSFloat(fontSize))
                End If
                Dim daWriter As ContentStreamWriter = New ContentStreamWriter(output)
                daWriter.writeTokens(daTokens)
            End If
            printWriter.println(getTextPosition(boundingBox, pdFont, fontSize, tokens))
            Dim q As Integer = getQ()
            If (q = PDTextbox.QUADDING_LEFT) Then
                'do nothing because left is default
            ElseIf (q = PDTextbox.QUADDING_CENTERED OrElse q = PDTextbox.QUADDING_RIGHT) Then
                Dim fieldWidth As Single = boundingBox.getWidth()
                Dim stringWidth As Single = (pdFont.getStringWidth(value) / 1000) * fontSize
                Dim adjustAmount As Single = fieldWidth - stringWidth - 4

                If (q = PDTextbox.QUADDING_CENTERED) Then
                    adjustAmount = adjustAmount / 2.0F
                End If

                printWriter.println(adjustAmount & " 0 Td")
            Else
                Throw New IOException("Error: Unknown justification value:" & q)
            End If
            printWriter.println("(" & value & ") Tj")
            printWriter.println("ET")
            printWriter.flush()
        End Sub

        Private Function getFontAndUpdateResources(ByVal tokens As List, ByVal appearanceStream As PDAppearanceStream) As PDFont 'throws IOException
            Dim retval As PDFont = Nothing
            Dim streamResources As PDResources = appearanceStream.getResources()
            Dim formResources As PDResources = acroForm.getDefaultResources()
            If (formResources IsNot Nothing) Then
                If (streamResources Is Nothing) Then
                    streamResources = New PDResources()
                    appearanceStream.setResources(streamResources)
                End If

                Dim da As COSString = getDefaultAppearance()
                If (da IsNot Nothing) Then
                    Dim data As String = da.getString()
                    Dim streamParser As PDFStreamParser = New PDFStreamParser(New ByteArrayInputStream(Sistema.Strings.GetBytes(data, "ISO-8859-1")), Nothing)
                    streamParser.parse()
                    tokens = streamParser.getTokens()
                End If

                Dim setFontIndex As Integer = tokens.indexOf(PDFOperator.getOperator("Tf"))
                Dim cosFontName As COSName = tokens.get(setFontIndex - 2)
                Dim fontName As String = cosFontName.getName()
                retval = streamResources.getFonts().get(fontName)
                If (retval Is Nothing) Then
                    retval = formResources.getFonts().get(fontName)
                    streamResources.addFont(retval, fontName)
                End If
            End If
            Return retval
        End Function

        Private Function convertToMultiLine(ByVal line As String) As String
            Dim currIdx As Integer = 0
            Dim lastIdx As Integer = 0
            Dim result As StringBuilder = New StringBuilder(line.Length() + 64)
            currIdx = line.IndexOf(vbLf, lastIdx)
            While (currIdx > -1)
                result.append(line.Substring(lastIdx, currIdx))
                result.append(" ) Tj" & vbLf & "0 -13 Td" & vbLf & "(")
                lastIdx = currIdx + 1
                currIdx = line.IndexOf(vbLf, lastIdx)
            End While
            result.append(line.Substring(lastIdx))
            Return result.toString()
        End Function

        '/**
        ' * Writes the stream to the actual stream in the COSStream.
        ' *
        ' * @throws IOException If there is an error writing to the stream
        ' */
        Private Sub writeToStream(ByVal data() As Byte, ByVal appearanceStream As PDAppearanceStream)  'throws IOException
            Dim out As OutputStream = appearanceStream.getStream().createUnfilteredStream()
            out.Write(data)
            out.Flush()
        End Sub

        ' /**
        '* w in an appearance stream represents the lineWidth.
        '* @return the linewidth
        '*/
        Private Function getLineWidth(ByVal tokens As List) As Single
            Dim retval As Single = 1
            If (tokens IsNot Nothing) Then
                Dim btIndex As Integer = tokens.indexOf(PDFOperator.getOperator("BT"))
                Dim wIndex As Integer = tokens.indexOf(PDFOperator.getOperator("w"))
                'the w should only be used if it is before the first BT.
                If ((wIndex > 0) AndAlso (wIndex < btIndex)) Then
                    retval = DirectCast(tokens.get(wIndex - 1), COSNumber).floatValue()
                End If
            End If
            Return retval
        End Function

        Private Function getSmallestDrawnRectangle(ByVal boundingBox As PDRectangle, ByVal tokens As List) As PDRectangle
            Dim smallest As PDRectangle = boundingBox
            For i As Integer = 0 To tokens.size() - 1
                Dim [next] As Object = tokens.get(i)
                If ([next] = PDFOperator.getOperator("re")) Then
                    Dim x As COSNumber = tokens.get(i - 4)
                    Dim y As COSNumber = tokens.get(i - 3)
                    Dim width As COSNumber = tokens.get(i - 2)
                    Dim height As COSNumber = tokens.get(i - 1)
                    Dim potentialSmallest As PDRectangle = New PDRectangle()
                    potentialSmallest.setLowerLeftX(x.floatValue())
                    potentialSmallest.setLowerLeftY(y.floatValue())
                    potentialSmallest.setUpperRightX(x.floatValue() + width.floatValue())
                    potentialSmallest.setUpperRightY(y.floatValue() + height.floatValue())
                    If (smallest Is Nothing OrElse smallest.getLowerLeftX() < potentialSmallest.getLowerLeftX() OrElse smallest.getUpperRightY() > potentialSmallest.getUpperRightY()) Then
                        smallest = potentialSmallest
                    End If
                End If
            Next
            Return smallest
        End Function

        '/**
        ' * My "not so great" method for calculating the fontsize.
        ' * It does not work superb, but it handles ok.
        ' * @return the calculated font-size
        ' *
        ' * @throws IOException If there is an error getting the font height.
        ' */
        Private Function calculateFontSize(ByVal pdFont As PDFont, ByVal boundingBox As PDRectangle, ByVal tokens As List, ByVal daTokens As List) As Single  '            throws IOException
            Dim fontSize As Single = 0
            If (daTokens IsNot Nothing) Then
                'daString looks like   "BMC /Helv 3.4 Tf EMC"

                Dim fontIndex As Integer = daTokens.indexOf(PDFOperator.getOperator("Tf"))
                If (fontIndex <> -1) Then
                    fontSize = DirectCast(daTokens.get(fontIndex - 1), COSNumber).floatValue()
                End If
            End If

            Dim widthBasedFontSize As Single = Single.MaxValue

            If (parent.doNotScroll()) Then
                'if we don't scroll then we will shrink the font to fit into the text area.
                Dim widthAtFontSize1 As Single = pdFont.getStringWidth(value) / 1000.0F
                Dim availableWidth As Single = getAvailableWidth(boundingBox, getLineWidth(tokens))
                widthBasedFontSize = availableWidth / widthAtFontSize1
            ElseIf (fontSize = 0) Then
                Dim lineWidth As Single = getLineWidth(tokens)
                Dim stringWidth As Single = pdFont.getStringWidth(value)
                Dim height As Single = 0
                If (TypeOf (pdFont) Is PDSimpleFont) Then
                    height = DirectCast(pdFont, PDSimpleFont).getFontDescriptor().getFontBoundingBox().getHeight()
                Else
                    'now much we can do, so lets assume font is square and use width
                    'as the height
                    height = pdFont.getAverageFontWidth()
                End If
                height = height / 1000.0F

                Dim availHeight As Single = getAvailableHeight(boundingBox, lineWidth)
                fontSize = Math.Min((availHeight / height), widthBasedFontSize)
            End If
            Return fontSize
        End Function

        '/**
        ' * Calculates where to start putting the text in the box.
        ' * The positioning is not quite as accurate as when Acrobat
        ' * places the elements, but it works though.
        ' *
        ' * @return the sting for representing the start position of the text
        ' *
        ' * @throws IOException If there is an error calculating the text position.
        ' */
        Private Function getTextPosition(ByVal boundingBox As PDRectangle, ByVal pdFont As PDFont, ByVal fontSize As Single, ByVal tokens As List) As String  '            throws IOException
            Dim lineWidth As Single = getLineWidth(tokens)
            Dim pos As Single = 0.0F
            If (parent.isMultiline()) Then
                Dim rows As Integer = getAvailableHeight(boundingBox, lineWidth) / CInt(fontSize)
                pos = ((rows) * fontSize) - fontSize
            Else
                If (TypeOf (pdFont) Is PDSimpleFont) Then
                    'BJL 9/25/2004
                    'This algorithm is a little bit of black magic.  It does
                    'not appear to be documented anywhere.  Through examining a few
                    'PDF documents and the value that Acrobat places in there I
                    'have determined that the below method of computing the position
                    'is correct for certain documents, but maybe not all.  It does
                    'work f1040ez.pdf and Form_1.pdf
                    Dim fd As PDFontDescriptor = DirectCast(pdFont, PDSimpleFont).getFontDescriptor()
                    Dim bBoxHeight As Single = boundingBox.getHeight()
                    Dim fontHeight As Single = fd.getFontBoundingBox().getHeight() + 2 * fd.getDescent()
                    fontHeight = (fontHeight / 1000) * fontSize
                    pos = (bBoxHeight - fontHeight) / 2
                Else
                    Throw New IOException("Error: Don't know how to calculate the position for non-simple fonts")
                End If
            End If
            Dim innerBox As PDRectangle = getSmallestDrawnRectangle(boundingBox, tokens)
            Dim xInset As Single = 2 + 2 * (boundingBox.getWidth() - innerBox.getWidth())
            Return Math.round(xInset) & " " & pos & " Td"
        End Function

        '/**
        ' * calculates the available width of the box.
        ' * @return the calculated available width of the box
        ' */
        Private Function getAvailableWidth(ByVal boundingBox As PDRectangle, ByVal lineWidth As Single) As Single
            Return boundingBox.getWidth() - 2 * lineWidth
        End Function

        '/**
        ' * calculates the available height of the box.
        ' * @return the calculated available height of the box
        ' */
        Private Function getAvailableHeight(ByVal boundingBox As PDRectangle, ByVal lineWidth As Single) As Single
            Return boundingBox.getHeight() - 2 * lineWidth
        End Function

    End Class

End Namespace

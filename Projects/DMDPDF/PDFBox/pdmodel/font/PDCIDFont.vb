Imports System.IO

'imports FinSeA. org.apache.commons.logging.Log;
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.encoding.conversion
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.util

Namespace org.apache.pdfbox.pdmodel.font

    '/**
    ' * This is implementation for the CIDFontType0/CIDFontType2 Fonts.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.11 $
    ' */
    Public MustInherit Class PDCIDFont
        Inherits PDSimpleFont

        '/**
        '    * Log instance.
        '    */
        '   private static final Log log = LogFactory.getLog(PDCIDFont.class);

        Private widthCache As Map(Of NInteger, NFloat) = Nothing
        Private defaultWidth As Long = 0

        '/**
        ' * Constructor.
        ' */
        Public Sub New()
            MyBase.New()
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param fontDictionary The font dictionary according to the PDF specification.
        ' */
        Public Sub New(ByVal fontDictionary As COSDictionary)
            MyBase.New(fontDictionary)
            extractWidths()
        End Sub

        '/**
        ' * This will get the fonts bouding box.
        ' *
        ' * @return The fonts bouding box.
        ' *
        ' * @throws IOException If there is an error getting the font bounding box.
        ' */
        Public Overrides Function getFontBoundingBox() As PDRectangle ' throws IOException
            Throw New NotImplementedException("getFontBoundingBox(): Not yet implemented")
        End Function

        '/**
        ' * This will get the default width.  The default value for the default width is 1000.
        ' *
        ' * @return The default width for the glyphs in this font.
        ' */
        Public Function getDefaultWidth() As Long
            If (defaultWidth = 0) Then
                Dim number As COSNumber = font.getDictionaryObject(COSName.DW)
                If (number IsNot Nothing) Then
                    defaultWidth = number.intValue()
                Else
                    defaultWidth = 1000
                End If
            End If
            Return defaultWidth
        End Function

        '/**
        ' * This will set the default width for the glyphs of this font.
        ' *
        ' * @param dw The default width.
        ' */
        Public Sub setDefaultWidth(ByVal dw As Long)
            defaultWidth = dw
            font.setLong(COSName.DW, dw)
        End Sub

        '/**
        ' * This will get the font width for a character.
        ' *
        ' * @param c The character code to get the width for.
        ' * @param offset The offset into the array.
        ' * @param length The length of the data.
        ' *
        ' * @return The width is in 1000 unit of text space, ie 333 or 777
        ' *
        ' * @throws IOException If an error occurs while parsing.
        ' */
        Public Overrides Function getFontWidth(ByVal c() As Byte, ByVal offset As Integer, ByVal length As Integer) As Single ' throws IOException
            Dim retval As Single = getDefaultWidth()
            Dim code As Integer = getCodeFromArray(c, offset, length)

            Dim widthFloat As NFloat = widthCache.get(code)
            If (widthFloat.HasValue) Then
                retval = widthFloat.Value
            End If
            Return retval
        End Function

        Private Sub extractWidths()
            If (widthCache Is Nothing) Then
                widthCache = New HashMap(Of NInteger, NFloat)
                Dim widths As COSArray = font.getDictionaryObject(COSName.W)
                If (widths IsNot Nothing) Then
                    Dim size As Integer = widths.size()
                    Dim counter As Integer = 0
                    While (counter < size)
                        Dim firstCode As COSNumber = widths.getObject(counter) : counter += 1
                        Dim [next] As COSBase = widths.getObject(counter) : counter += 1
                        If (TypeOf ([next]) Is COSArray) Then
                            Dim array As COSArray = [next]
                            Dim startRange As Integer = firstCode.intValue()
                            Dim arraySize As Integer = array.size()
                            For i As Integer = 0 To arraySize - 1
                                Dim width As COSNumber = array.get(i)
                                widthCache.put(startRange + i, width.floatValue())
                            Next
                        Else
                            Dim secondCode As COSNumber = [next]
                            Dim rangeWidth As COSNumber = widths.getObject(counter) : counter += 1
                            Dim startRange As Integer = firstCode.intValue()
                            Dim endRange As Integer = secondCode.intValue()
                            Dim width As Single = rangeWidth.floatValue()
                            For i As Integer = startRange To endRange
                                widthCache.put(i, width)
                            Next
                        End If
                    End While
                End If
            End If
        End Sub

        '/**
        ' * This will get the font height for a character.
        ' *
        ' * @param c The character code to get the height for.
        ' * @param offset The offset into the array.
        ' * @param length The length of the data.
        ' *
        ' * @return The width is in 1000 unit of text space, ie 333 or 777
        ' *
        ' * @throws IOException If an error occurs while parsing.
        ' */
        Public Overrides Function getFontHeight(ByVal c() As Byte, ByVal offset As Integer, ByVal length As Integer) As Single 'throws IOException
            Dim retval As Single = 0
            Dim desc As PDFontDescriptor = getFontDescriptor()
            Dim xHeight As Single = desc.getXHeight()
            Dim capHeight As Single = desc.getCapHeight()
            If (xHeight <> 0.0F AndAlso capHeight <> 0) Then
                'do an average of these two.  Can we do better???
                retval = (xHeight + capHeight) / 2.0F
            ElseIf (xHeight <> 0) Then
                retval = xHeight
            ElseIf (capHeight <> 0) Then
                retval = capHeight
            Else
                retval = 0
            End If
            If (retval = 0) Then
                retval = desc.getAscent()
            End If
            Return retval
        End Function

        '/**
        ' * This will get the average font width for all characters.
        ' *
        ' * @return The width is in 1000 unit of text space, ie 333 or 777
        ' *
        ' * @throws IOException If an error occurs while parsing.
        ' */
        Public Overrides Function getAverageFontWidth() As Single ' throws IOException
            Dim totalWidths As Single = 0.0F
            Dim characterCount As Single = 0.0F
            Dim defaultWidth As Single = getDefaultWidth()
            Dim widths As COSArray = font.getDictionaryObject(COSName.W)

            If (widths IsNot Nothing) Then
                For i As Integer = 0 To widths.size() - 1
                    Dim firstCode As COSNumber = widths.getObject(i) : i += 1
                    Dim [next] As COSBase = widths.getObject(i)
                    If (TypeOf ([next]) Is COSArray) Then
                        Dim array As COSArray = [next]
                        For j As Integer = 0 To array.size() - 1
                            Dim width As COSNumber = array.get(j)
                            totalWidths += width.floatValue()
                            characterCount += 1
                        Next
                    Else
                        i += 1
                        Dim rangeWidth As COSNumber = widths.getObject(i)
                        If (rangeWidth.floatValue() > 0) Then
                            totalWidths += rangeWidth.floatValue()
                            characterCount += 1
                        End If
                    End If
                Next
            End If
            Dim average As Single = totalWidths / characterCount
            If (average <= 0) Then
                average = defaultWidth
            End If
            Return average
        End Function

        Public Overrides Function getFontWidth(ByVal charCode As Integer) As Single
            Dim width As Single = -1
            If (widthCache.containsKey(charCode)) Then
                width = widthCache.get(charCode)
            End If
            Return width
        End Function

        '/**
        ' * Extract the CIDSystemInfo.
        ' * @return the CIDSystemInfo as String
        ' */
        Private Function getCIDSystemInfo() As String
            Dim cidSystemInfo As String = ""
            Dim _cidsysteminfo As COSDictionary = font.getDictionaryObject(COSName.CIDSYSTEMINFO)
            If (_cidsysteminfo IsNot Nothing) Then
                Dim ordering As String = _cidsysteminfo.getString(COSName.ORDERING)
                Dim registry As String = _cidsysteminfo.getString(COSName.REGISTRY)
                Dim supplement As Integer = _cidsysteminfo.getInt(COSName.SUPPLEMENT)
                cidSystemInfo = registry & "-" & ordering & "-" & supplement
            End If
            Return cidSystemInfo
        End Function

        Protected Overrides Sub determineEncoding()
            Dim cidSystemInfo As String = getCIDSystemInfo()
            If (cidSystemInfo <> "") Then
                If (cidSystemInfo.Contains("Identity")) Then
                    cidSystemInfo = "Identity-H"
                ElseIf (cidSystemInfo.StartsWith("Adobe-UCS-")) Then
                    cidSystemInfo = "Adobe-Identity-UCS"
                Else
                    cidSystemInfo = cidSystemInfo.Substring(0, cidSystemInfo.LastIndexOf("-")) & "-UCS2"
                End If
                cmap = cmapObjects.get(cidSystemInfo)
                If (cmap Is Nothing) Then
                    Dim resourceName As String = resourceRootCMAP & cidSystemInfo
                    Try
                        cmap = parseCmap(resourceRootCMAP, ResourceLoader.loadResource(resourceName))
                        If (cmap Is Nothing) Then
                            LOG.error("Error: Could not parse predefined CMAP file for '" & cidSystemInfo & "'")
                        End If
                    Catch exception As IOException
                        LOG.error("Error: Could not find predefined CMAP file for '" & cidSystemInfo & "'")
                    End Try
                End If
            Else
                MyBase.determineEncoding()
            End If
        End Sub

        Public Overrides Function encode(ByVal c() As Byte, ByVal offset As Integer, ByVal length As Integer) As String 'throws IOException
            Dim result As String = ""
            If (cmap IsNot Nothing) Then
                result = cmapEncoding(getCodeFromArray(c, offset, length), length, True, cmap)
            Else
                result = MyBase.encode(c, offset, length)
            End If
            Return result
        End Function

    End Class

End Namespace

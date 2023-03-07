Imports System.IO
Imports FinSeA.Io
Imports FinSeA.Drawings
Imports FinSeA.org.apache.fontbox.afm
Imports FinSeA.org.apache.fontbox.pfb
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.encoding
Imports FinSeA.org.apache.pdfbox.pdmodel
Imports FinSeA.org.apache.pdfbox.pdmodel.common

Namespace org.apache.pdfbox.pdmodel.font


    '/**
    ' * This is implementation of the Type1 Font with a afm and a pfb file.
    ' * 
    ' * @author <a href="mailto:m.g.n@gmx.de">Michael Niedermair</a>
    ' * @version $Revision: 1.5 $
    ' */
    Public Class PDType1AfmPfbFont
        Inherits PDType1Font

        ''' <summary>
        ''' the buffersize.
        ''' </summary>
        ''' <remarks></remarks>
        Private Const BUFFERSIZE As Integer = &HFFFF

        ''' <summary>
        ''' The font metric.
        ''' </summary>
        ''' <remarks></remarks>
        Private metric As FontMetric

        '/**
        ' * Create a new object.
        ' * 
        ' * @param doc The PDF document that will hold the embedded font.
        ' * @param afmname The font filename.
        ' * @throws IOException If there is an error loading the data.
        ' */
        Public Sub New(ByVal doc As PDDocument, ByVal afmname As String) 'throws IOException
            MyBase.New()
            Dim afmin As InputStream = New BufferedInputStream(New FileInputStream(afmname), BUFFERSIZE)
            Dim pfbname As String = afmname.Replace(".AFM", "").Replace(".afm", "") & ".pfb"
            Dim pfbin As InputStream = New BufferedInputStream(pfbname, BUFFERSIZE)
            load(doc, afmin, pfbin)
            afmin.Dispose()
            pfbin.Dispose()
        End Sub

        '/**
        ' * Create a new object.
        ' * 
        ' * @param doc The PDF document that will hold the embedded font.
        ' * @param afm The afm input.
        ' * @param pfb The pfb input.
        ' * @throws IOException If there is an error loading the data.
        ' */
        Public Sub New(ByVal doc As PDDocument, ByVal afm As InputStream, ByVal pfb As InputStream) ' throws IOException
            MyBase.New()
            load(doc, afm, pfb)
        End Sub

        '/**
        ' * This will load a afm and pfb to be embedding into a document.
        ' * 
        ' * @param doc The PDF document that will hold the embedded font.
        ' * @param afm The afm input.
        ' * @param pfb The pfb input.
        ' * @throws IOException If there is an error loading the data.
        ' */
        Private Sub load(ByVal doc As PDDocument, ByVal afm As InputStream, ByVal pfb As InputStream) 'throws IOException
            Dim fd As PDFontDescriptorDictionary = New PDFontDescriptorDictionary()
            setFontDescriptor(fd)

            ' read the pfb
            Dim pfbparser As PfbParser = New PfbParser(pfb)
            pfb.Close()

            Dim fontStream As PDStream = New PDStream(doc, pfbparser.getInputStream(), False)
            fontStream.getStream().setInt("Length", pfbparser.size())
            For i As Integer = 0 To pfbparser.getLengths().length - 1
                fontStream.getStream().setInt("Length" + (i + 1), pfbparser.getLengths()(i))
            Next
            fontStream.addCompression()
            fd.setFontFile(fontStream)

            ' read the afm
            Dim parser As AFMParser = New AFMParser(afm)
            parser.parse()
            metric = parser.getResult()
            setFontEncoding(afmToDictionary(New AFMEncoding(metric)))

            ' set the values
            setBaseFont(metric.getFontName())
            fd.setFontName(metric.getFontName())
            fd.setFontFamily(metric.getFamilyName())
            fd.setNonSymbolic(True)
            fd.setFontBoundingBox(New PDRectangle(metric.getFontBBox()))
            fd.setItalicAngle(metric.getItalicAngle())
            fd.setAscent(metric.getAscender())
            fd.setDescent(metric.getDescender())
            fd.setCapHeight(metric.getCapHeight())
            fd.setXHeight(metric.getXHeight())
            fd.setAverageWidth(metric.getAverageCharacterWidth())
            fd.setCharacterSet(metric.getCharacterSet())

            ' get firstchar, lastchar
            Dim firstchar As Integer = 255
            Dim lastchar As Integer = 0

            ' widths
            Dim listmetric As List(Of CharMetric) = metric.getCharMetrics()
            Dim encoding As pdfbox.encoding.Encoding = getFontEncoding()
            Dim maxWidths As Integer = 256
            Dim widths As List(Of NFloat) = New ArrayList(Of NFloat)(maxWidths)
            Dim zero As Integer = 250
            For i As Integer = 0 To maxWidths - 1
                widths.add(zero)
            Next
            'Iterator<CharMetric> iter = listmetric.iterator();
            For Each m As CharMetric In listmetric 'While (iter.hasNext())
                'CharMetric m = iter.next();
                Dim n As Integer = m.getCharacterCode()
                If (n > 0) Then
                    firstchar = Math.Min(firstchar, n)
                    lastchar = Math.Max(lastchar, n)
                    If (m.getWx() > 0) Then
                        Dim width As Integer = Math.round(m.getWx())
                        widths.set(n, CSng(width))
                        ' germandbls has 2 character codes !! Don't ask me why .....
                        ' StandardEncoding = 0373 = 251
                        ' WinANSIEncoding = 0337 = 223
                        If (m.getName().Equals("germandbls") AndAlso n <> 223) Then
                            widths.set(337, width)
                        End If
                    End If
                Else
                    ' my AFMPFB-Fonts has no character-codes for german umlauts
                    ' so that I've to add them here by hand
                    If (m.getName().Equals("adieresis")) Then
                        widths.set(344, widths.get(encoding.getCode("a")))
                    ElseIf (m.getName().Equals("odieresis")) Then
                        widths.set(366, widths.get(encoding.getCode("o")))
                    ElseIf (m.getName().Equals("udieresis")) Then
                        widths.set(374, widths.get(encoding.getCode("u")))
                    ElseIf (m.getName().Equals("Adieresis")) Then
                        widths.set(304, widths.get(encoding.getCode("A")))
                    ElseIf (m.getName().Equals("Odieresis")) Then
                        widths.set(326, widths.get(encoding.getCode("O")))
                    ElseIf (m.getName().Equals("Udieresis")) Then
                        widths.set(334, widths.get(encoding.getCode("U")))
                    End If
                End If
            Next
            setFirstChar(0)
            setLastChar(255)
            setWidths(widths)
        End Sub

        '/*
        ' * This will generate a Encoding from the AFM-Encoding, because the AFM-Enconding isn't exported to the pdf and
        ' * consequently the StandardEncoding is used so that any special character is missing I've copied the code from the
        ' * pdfbox-forum posted by V0JT4 and made some additions concerning german umlauts see also
        ' * https://sourceforge.net/forum/message.php?msg_id=4705274
        ' */
        Private Function afmToDictionary(ByVal encoding As AFMEncoding) As DictionaryEncoding  'throws java.io.IOException
            Dim array As COSArray = New COSArray()
            array.add(COSInteger.ZERO)
            For i As Integer = 0 To 256 - 1
                array.add(COSName.getPDFName(encoding.getName(i)))
            Next
            ' my AFMPFB-Fonts has no character-codes for german umlauts
            ' so that I've to add them here by hand
            array.set(337 + 1, COSName.getPDFName("germandbls"))
            array.set(344 + 1, COSName.getPDFName("adieresis"))
            array.set(366 + 1, COSName.getPDFName("odieresis"))
            array.set(374 + 1, COSName.getPDFName("udieresis"))
            array.set(304 + 1, COSName.getPDFName("Adieresis"))
            array.set(326 + 1, COSName.getPDFName("Odieresis"))
            array.set(334 + 1, COSName.getPDFName("Udieresis"))

            Dim dictionary As COSDictionary = New COSDictionary()
            dictionary.setItem(COSName.NAME, COSName.ENCODING)
            dictionary.setItem(COSName.DIFFERENCES, array)
            dictionary.setItem(COSName.BASE_ENCODING, COSName.STANDARD_ENCODING)
            Return New DictionaryEncoding(dictionary)
        End Function

    End Class

End Namespace


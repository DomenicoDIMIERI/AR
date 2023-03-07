Imports System.Drawing
Imports System.IO
Imports FinSeA.Io

Imports FinSeA.org.apache.fontbox.afm
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.encoding
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.Drawings
Imports FinSeA.Text
Imports FinSeA.Exceptions

Namespace org.apache.pdfbox.pdmodel.font


    '/**
    ' * This is implementation of the Type1 Font.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.11 $
    ' */
    Public Class PDType1Font
        Inherits PDSimpleFont
        '/**
        ' * Log instance.
        ' */
        'private static final Log log = LogFactory.getLog(PDType1Font.class);

        Private type1CFont As PDType1CFont = Nothing

        ''' <summary>
        ''' Standard Base 14 Font.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly TIMES_ROMAN As New PDType1Font("Times-Roman")

        ''' <summary>
        ''' Standard Base 14 Font.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly TIMES_BOLD As New PDType1Font("Times-Bold")

        ''' <summary>
        ''' Standard Base 14 Font.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly TIMES_ITALIC As New PDType1Font("Times-Italic")

        ''' <summary>
        ''' Standard Base 14 Font.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly TIMES_BOLD_ITALIC As New PDType1Font("Times-BoldItalic")

        ''' <summary>
        ''' Standard Base 14 Font.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly HELVETICA As New PDType1Font("Helvetica")

        ''' <summary>
        ''' Standard Base 14 Font.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly HELVETICA_BOLD As New PDType1Font("Helvetica-Bold")

        ''' <summary>
        ''' Standard Base 14 Font.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly HELVETICA_OBLIQUE As New PDType1Font("Helvetica-Oblique")

        ''' <summary>
        ''' Standard Base 14 Font.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly HELVETICA_BOLD_OBLIQUE As New PDType1Font("Helvetica-BoldOblique")

        ''' <summary>
        ''' Standard Base 14 Font.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly COURIER As New PDType1Font("Courier")

        ''' <summary>
        ''' Standard Base 14 Font.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly COURIER_BOLD As New PDType1Font("Courier-Bold")

        ''' <summary>
        ''' Standard Base 14 Font.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly COURIER_OBLIQUE As New PDType1Font("Courier-Oblique")

        ''' <summary>
        ''' Standard Base 14 Font.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly COURIER_BOLD_OBLIQUE As New PDType1Font("Courier-BoldOblique")

        ''' <summary>
        ''' Standard Base 14 Font.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly SYMBOL As New PDType1Font("Symbol")

        ''' <summary>
        ''' Standard Base 14 Font.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly ZAPF_DINGBATS As New PDType1Font("ZapfDingbats")

        Private Shared STANDARD_14 As Map(Of String, PDType1Font) = New HashMap(Of String, PDType1Font)

        Shared Sub New()
            STANDARD_14.put(TIMES_ROMAN.getBaseFont(), TIMES_ROMAN)
            STANDARD_14.put(TIMES_BOLD.getBaseFont(), TIMES_BOLD)
            STANDARD_14.put(TIMES_ITALIC.getBaseFont(), TIMES_ITALIC)
            STANDARD_14.put(TIMES_BOLD_ITALIC.getBaseFont(), TIMES_BOLD_ITALIC)
            STANDARD_14.put(HELVETICA.getBaseFont(), HELVETICA)
            STANDARD_14.put(HELVETICA_BOLD.getBaseFont(), HELVETICA_BOLD)
            STANDARD_14.put(HELVETICA_OBLIQUE.getBaseFont(), HELVETICA_OBLIQUE)
            STANDARD_14.put(HELVETICA_BOLD_OBLIQUE.getBaseFont(), HELVETICA_BOLD_OBLIQUE)
            STANDARD_14.put(COURIER.getBaseFont(), COURIER)
            STANDARD_14.put(COURIER_BOLD.getBaseFont(), COURIER_BOLD)
            STANDARD_14.put(COURIER_OBLIQUE.getBaseFont(), COURIER_OBLIQUE)
            STANDARD_14.put(COURIER_BOLD_OBLIQUE.getBaseFont(), COURIER_BOLD_OBLIQUE)
            STANDARD_14.put(SYMBOL.getBaseFont(), SYMBOL)
            STANDARD_14.put(ZAPF_DINGBATS.getBaseFont(), ZAPF_DINGBATS)
        End Sub

        Private awtFont As JFont = Nothing

        '/**
        ' * Constructor.
        ' */
        Public Sub New()
            MyBase.New()
            font.setItem(COSName.SUBTYPE, COSName.TYPE1)
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param fontDictionary The font dictionary according to the PDF specification.
        ' */
        Public Sub New(ByVal fontDictionary As COSDictionary)
            MyBase.New(fontDictionary)
            Dim fd As PDFontDescriptor = getFontDescriptor()
            If (fd IsNot Nothing AndAlso TypeOf (fd) Is PDFontDescriptorDictionary) Then
                ' a Type1 font may contain a Type1C font
                Dim fontFile3 As PDStream = DirectCast(fd, PDFontDescriptorDictionary).getFontFile3()
                If (fontFile3 IsNot Nothing) Then
                    Try
                        type1CFont = New PDType1CFont(MyBase.font)
                    Catch exception As IOException
                        LOG.info("Can't read the embedded type1C font " & fd.getFontName())
                    End Try
                End If
            End If
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param baseFont The base font for this font.
        ' */
        Public Sub New(ByVal baseFont As String)
            Me.New()
            setBaseFont(baseFont)
            setFontEncoding(New WinAnsiEncoding())
            setEncoding(COSName.WIN_ANSI_ENCODING)
        End Sub

        '/**
        ' * A convenience method to get one of the standard 14 font from name.
        ' *
        ' * @param name The name of the font to get.
        ' *
        ' * @return The font that matches the name or null if it does not exist.
        ' */
        Public Shared Function getStandardFont(ByVal name As String) As PDType1Font
            Return STANDARD_14.get(name)
        End Function

        '/**
        ' * This will get the names of the standard 14 fonts.
        ' *
        ' * @return An array of the names of the standard 14 fonts.
        ' */
        Public Shared Function getStandard14Names() As String()
            Return STANDARD_14.keySet().toArray
        End Function

        Public Overrides Function getawtFont() As JFont ' throws IOException
            If (awtFont Is Nothing) Then
                If (type1CFont IsNot Nothing) Then
                    awtFont = type1CFont.getawtFont()
                Else
                    Dim baseFont As String = getBaseFont()
                    Dim fd As PDFontDescriptor = getFontDescriptor()
                    If (fd IsNot Nothing AndAlso TypeOf (fd) Is PDFontDescriptorDictionary) Then
                        Dim fdDictionary As PDFontDescriptorDictionary = fd
                        If (fdDictionary.getFontFile() IsNot Nothing) Then
                            Try
                                ' create a type1 font with the embedded data
                                awtFont = JFont.createFont(JFontFormat.TYPE1_FONT, fdDictionary.getFontFile().createInputStream())
                            Catch e As FontFormatException
                                LOG.info("Can't read the embedded type1 font " & fd.getFontName())
                            End Try
                        End If
                        If (awtFont Is Nothing) Then
                            ' check if the font is part of our environment
                            awtFont = FontManager.getAwtFont(fd.getFontName())
                            If (awtFont Is Nothing) Then
                                LOG.info("Can't find the specified font " & fd.getFontName())
                            End If
                        End If
                    Else
                        ' check if the font is part of our environment
                        awtFont = FontManager.getAwtFont(baseFont)
                        If (awtFont Is Nothing) Then
                            LOG.info("Can't find the specified basefont " & baseFont)
                        End If
                    End If
                End If
                If (awtFont Is Nothing) Then
                    ' we can't find anything, so we have to use the standard font
                    awtFont = FontManager.getStandardFont()
                    LOG.info("Using font " & awtFont.getName() & " instead")
                End If
            End If
            Return awtFont
        End Function

        Protected Overrides Sub determineEncoding()
            MyBase.determineEncoding()
            Dim fontEncoding As pdfbox.encoding.Encoding = getFontEncoding()
            If (fontEncoding Is Nothing) Then
                Dim metric As FontMetric = getAFM()
                If (metric IsNot Nothing) Then
                    fontEncoding = New AFMEncoding(metric)
                End If
                setFontEncoding(fontEncoding)
            End If
            getEncodingFromFont(getFontEncoding() Is Nothing)
        End Sub

        '/**
        ' * Tries to get the encoding for the type1 font.
        ' *
        ' */
        Private Sub getEncodingFromFont(ByVal extractEncoding As Boolean)
            ' This whole section of code needs to be replaced with an actual type1 font parser!!
            ' Get the font program from the embedded type font.
            Dim fontDescriptor As PDFontDescriptor = getFontDescriptor()
            If (fontDescriptor IsNot Nothing AndAlso TypeOf (fontDescriptor) Is PDFontDescriptorDictionary) Then
                Dim fontFile As PDStream = DirectCast(fontDescriptor, PDFontDescriptorDictionary).getFontFile()
                If (fontFile IsNot Nothing) Then
                    Dim [in] As BufferedReader = Nothing
                    Try
                        [in] = New BufferedReader(New InputStreamReader(fontFile.createInputStream()))

                        ' this section parses the font program stream searching for a /Encoding entry
                        ' if it contains an array of values a Type1Encoding will be returned
                        ' if it encoding contains an encoding name the corresponding Encoding will be returned
                        Dim line As String = ""
                        Dim encoding As Type1Encoding = Nothing
                        line = [in].readLine()
                        While (line <> "")
                            If (extractEncoding) Then
                                If (line.StartsWith("currentdict end")) Then
                                    If (encoding IsNot Nothing) Then
                                        setFontEncoding(encoding)
                                        Exit While
                                    End If
                                    If (line.StartsWith("/Encoding")) Then
                                        If (line.Contains("array")) Then
                                            Dim st As StringTokenizer = New StringTokenizer(line)
                                            ' ignore the first token
                                            st.nextElement()
                                            Dim arraySize As Integer = CInt(st.nextToken())
                                            encoding = New Type1Encoding(arraySize)
                                        End If
                                        ' if there is already an encoding, we don't need to
                                        ' assign another one
                                    ElseIf (getFontEncoding() Is Nothing) Then
                                        Dim st As StringTokenizer = New StringTokenizer(line)
                                        '// ignore the first token
                                        st.nextElement()
                                        Dim type1Encoding As String = st.nextToken()
                                        setFontEncoding(EncodingManager.INSTANCE.getEncoding(COSName.getPDFName(type1Encoding)))
                                        Exit While
                                    End If
                                ElseIf (line.StartsWith("dup")) Then
                                    Dim st As StringTokenizer = New StringTokenizer(line.Replace("/", " /"))
                                    ' ignore the first token
                                    st.nextElement()
                                    Try
                                        Dim index As Integer = CInt(st.nextToken())
                                        Dim name As String = st.nextToken()
                                        If (encoding Is Nothing) Then
                                            LOG.warn("Unable to get character encoding. Encoding definition found without /Encoding line.")
                                        Else
                                            encoding.addCharacterEncoding(index, name.Replace("/", ""))
                                        End If
                                    Catch exception As FormatException
                                        ' there are (tex?)-some fonts containing postscript code like the following, 
                                        ' which has to be ignored, see PDFBOX-1481
                                        ' dup dup 161 10 getinterval 0 exch putinterval ....
                                        LOG.debug("Malformed encoding definition ignored (line=" & line & ")")
                                    End Try
                                    Continue While
                                End If
                            End If
                            '// according to the pdf reference, all font matrices should be same, except for type 3 fonts.
                            '// but obviously there are some type1 fonts with different matrix values, see pdf sample
                            '// attached to PDFBOX-935
                            If (line.StartsWith("/FontMatrix")) Then
                                ' most likely all matrix values are in the same line than the keyword
                                If (line.IndexOf("[") > -1) Then
                                    Dim matrixValues As String = line.Substring(line.IndexOf("[") + 1, line.LastIndexOf("]"))
                                    Dim st As StringTokenizer = New StringTokenizer(matrixValues)
                                    Dim array As COSArray = New COSArray()
                                    If (st.countTokens() >= 6) Then
                                        Try
                                            For i As Integer = 0 To 6 - 1
                                                Dim floatValue As COSFloat = New COSFloat(Single.Parse(st.nextToken()))
                                                array.add(floatValue)
                                            Next
                                        Catch exception As FormatException
                                            LOG.error("Can't read the fontmatrix from embedded font file!")
                                        End Try
                                        fontMatrix = New PDMatrix(array)
                                    End If
                                Else
                                    ' there are fonts where all values are on a separate line, see PDFBOX-1611
                                    Dim array As COSArray = New COSArray()
                                    line = [in].readLine()
                                    While (line <> "")
                                        If (line.StartsWith("[")) Then
                                            Continue While
                                        End If
                                        If (line.EndsWith("]")) Then
                                            Exit While
                                        End If
                                        Try
                                            Dim floatValue As COSFloat = New COSFloat(Single.Parse(line))
                                            array.add(floatValue)
                                        Catch exception As FormatException
                                            LOG.error("Can't read the fontmatrix from embedded font file!")
                                        End Try
                                        line = [in].readLine()
                                    End While
                                    If (array.size() = 6) Then
                                        fontMatrix = New PDMatrix(array)
                                    Else
                                        LOG.error("Can't read the fontmatrix from embedded font file, not enough values!")
                                    End If
                                End If
                            End If
                            line = [in].readLine()
                        End While
                    Catch exception As IOException
                        LOG.error("Error: Could not extract the encoding from the embedded type1 font.")
                    Finally
                        If ([in] IsNot Nothing) Then
                            Try
                                [in].Close()
                            Catch exception As IOException
                                LOG.error("An error occurs while closing the stream used to read the embedded type1 font.")
                            End Try
                        End If
                    End Try
                End If
            End If
        End Sub

        Public Overrides Function encode(ByVal c() As Byte, ByVal offset As Integer, ByVal length As Integer) As String
            If (type1CFont IsNot Nothing AndAlso getFontEncoding() Is Nothing) Then
                Return type1CFont.encode(c, offset, length)
            Else
                Return MyBase.encode(c, offset, length)
            End If
        End Function

        Public Overrides Function encodeToCID(ByVal c() As Byte, ByVal offset As Integer, ByVal length As Integer) As Integer
            If (type1CFont IsNot Nothing AndAlso getFontEncoding() Is Nothing) Then
                Return type1CFont.encodeToCID(c, offset, length)
            Else
                Return MyBase.encodeToCID(c, offset, length)
            End If
        End Function

        Public Overrides Function getFontMatrix() As PDMatrix
            If (type1CFont IsNot Nothing) Then
                Return type1CFont.getFontMatrix()
            Else
                Return MyBase.getFontMatrix()
            End If
        End Function

    End Class

End Namespace

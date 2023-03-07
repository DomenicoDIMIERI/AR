Imports FinSeA.Sistema
Imports System.IO
Imports System.Text.RegularExpressions
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.exceptions
Imports FinSeA.org.apache.pdfbox.pdfparser
Imports FinSeA.org.apache.pdfbox.pdfwriter
Imports FinSeA.org.apache.pdfbox.pdmodel
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.util

Namespace org.apache.pdfbox

    '/**
    ' * This is the main program that simply parses the pdf document and replace
    ' * change a PDF to use a specific colorspace.
    ' *
    ' * @author <a href="ben@benlitchfield.com">Ben Litchfield</a>
    ' * @author Pierre-Yves Landuré (pierre-yves@landure.org)
    ' * @version $Revision: 1.5 $
    ' */
    Public Class ConvertColorspace

        Private Const PASSWORD As String = "-password"
        Private Const CONVERSION As String = "-equiv"
        Private Const DEST_COLORSPACE As String = "-toColorspace"

        '/**
        ' * private constructor.
        '*/
        Private Sub New()
        End Sub

        '/**
        ' * The method that replace RGB colors by CMYK ones.
        ' *
        ' * @param inputFile input file name.
        ' * @param colorEquivalents a dictionnary for the color equivalents.
        ' * @param destColorspace The destination colorspace, currently CMYK is supported.
        ' *
        ' * @throws IOException If there is an error parsing the document.
        ' */
        Private Shared Sub replaceColors(ByVal inputFile As PDDocument, ByVal colorEquivalents As Hashtable, ByVal destColorspace As String)  'throws IOException
            If (Not destColorspace.Equals("CMYK")) Then
                Throw New IOException("Error: Unknown colorspace " & destColorspace)
            End If
            Dim pagesList As List = inputFile.getDocumentCatalog().getAllPages()

            Dim currentPage As PDPage = Nothing
            Dim parser As PDFStreamParser = Nothing
            Dim pageTokens As List = Nothing
            Dim editedPageTokens As List = Nothing

            For pageCounter As Integer = 0 To pagesList.size() - 1 '; pageCounter++) // For each document page
                currentPage = pagesList.get(pageCounter)

                parser = New PDFStreamParser(currentPage.getContents().getStream())
                parser.parse()
                pageTokens = parser.getTokens()
                editedPageTokens = New ArrayList()

                For counter As Integer = 0 To pageTokens.size() - 1 '; counter++) // For each page token
                    Dim token As Object = pageTokens.get(counter)
                    If (TypeOf (token) Is PDFOperator) Then ' Test if PDFOperator
                        Dim tokenOperator As PDFOperator = token

                        If (tokenOperator.getOperation().Equals("rg")) Then ' // Test if "rg" Operator.
                            If (destColorspace.Equals("CMYK")) Then
                                replaceRGBTokensWithCMYKTokens(editedPageTokens, pageTokens, counter, colorEquivalents)
                                editedPageTokens.add(PDFOperator.getOperator("k"))
                            End If
                        ElseIf (tokenOperator.getOperation().Equals("RG")) Then ' // Test if "rg" Operator.
                            If (destColorspace.Equals("CMYK")) Then
                                replaceRGBTokensWithCMYKTokens(editedPageTokens, pageTokens, counter, colorEquivalents)
                                editedPageTokens.add(PDFOperator.getOperator("K"))
                            End If
                        ElseIf (tokenOperator.getOperation().Equals("g")) Then '// Test if "rg" Operator.
                            If (destColorspace.Equals("CMYK")) Then
                                replaceGrayTokensWithCMYKTokens(editedPageTokens, pageTokens, counter, colorEquivalents)
                                editedPageTokens.add(PDFOperator.getOperator("k"))
                            End If
                        ElseIf (tokenOperator.getOperation().Equals("G")) Then '// Test if "rg" Operator.
                            If (destColorspace.Equals("CMYK")) Then
                                replaceGrayTokensWithCMYKTokens(editedPageTokens, pageTokens, counter, colorEquivalents)
                                editedPageTokens.add(PDFOperator.getOperator("K"))
                            End If
                        Else
                            editedPageTokens.add(token)
                        End If
                    Else '// Test if PDFOperator
                        editedPageTokens.add(token)
                    End If
                Next ' // For each page token

                ' We replace original page content by the edited one.
                Dim updatedPageContents As PDStream = New PDStream(inputFile)
                Dim contentWriter As ContentStreamWriter = New ContentStreamWriter(updatedPageContents.createOutputStream())
                contentWriter.writeTokens(editedPageTokens)
                currentPage.setContents(updatedPageContents)

            Next ' For each document page
        End Sub

        Private Shared Sub replaceRGBTokensWithCMYKTokens(ByVal editedPageTokens As List, ByVal pageTokens As List, ByVal counter As Integer, ByVal colorEquivalents As Hashtable)
            'Get current RGB color.
            Dim red As Single = DirectCast(pageTokens.get(counter - 3), COSNumber).floatValue()
            Dim green As Single = DirectCast(pageTokens.get(counter - 2), COSNumber).floatValue()
            Dim blue As Single = DirectCast(pageTokens.get(counter - 1), COSNumber).floatValue()

            Dim intRed As Integer = Math.round(red * 255.0F)
            Dim intGreen As Integer = Math.round(green * 255.0F)
            Dim intBlue As Integer = Math.round(blue * 255.0F)

            Dim rgbColor As ColorSpaceInstance = New ColorSpaceInstance()
            rgbColor.colorspace = "RGB"
            rgbColor.colorspaceValues = New Integer() {intRed, intGreen, intBlue}
            Dim cmykColor As ColorSpaceInstance = colorEquivalents.get(rgbColor)
            Dim cmyk() As Single = Nothing

            If (cmykColor IsNot Nothing) Then
                cmyk = New Single() { _
                    cmykColor.colorspaceValues(0) / 100.0F, _
                    cmykColor.colorspaceValues(1) / 100.0F, _
                    cmykColor.colorspaceValues(2) / 100.0F, _
                    cmykColor.colorspaceValues(2) / 100.0F _
                }
            Else
                cmyk = convertRGBToCMYK(red, green, blue)
            End If

            'remove the RGB components that are already part of the editedPageTokens list
            editedPageTokens.remove(editedPageTokens.size() - 1)
            editedPageTokens.remove(editedPageTokens.size() - 1)
            editedPageTokens.remove(editedPageTokens.size() - 1)

            ' Add the new CMYK color
            editedPageTokens.add(New COSFloat(cmyk(0)))
            editedPageTokens.add(New COSFloat(cmyk(1)))
            editedPageTokens.add(New COSFloat(cmyk(2)))
            editedPageTokens.add(New COSFloat(cmyk(2)))
        End Sub

        Private Shared Sub replaceGrayTokensWithCMYKTokens(ByVal editedPageTokens As List, ByVal pageTokens As List, ByVal counter As Integer, ByVal colorEquivalents As Hashtable)
            '      Get current RGB color.
            Dim gray As Single = DirectCast(pageTokens.get(counter - 1), COSNumber).floatValue()

            Dim grayColor As ColorSpaceInstance = New ColorSpaceInstance()
            grayColor.colorspace = "Grayscale"
            grayColor.colorspaceValues = New Integer() {Math.round(gray * 100)}
            Dim cmykColor As ColorSpaceInstance = colorEquivalents.get(grayColor)
            Dim cmyk() As Single = Nothing

            If (cmykColor IsNot Nothing) Then
                cmyk = New Single() { _
                    cmykColor.colorspaceValues(0) / 100.0F, _
                    cmykColor.colorspaceValues(1) / 100.0F, _
                    cmykColor.colorspaceValues(2) / 100.0F, _
                    cmykColor.colorspaceValues(2) / 100.0F _
                }
            Else
                cmyk = New Single() {0, 0, 0, gray}
            End If

            'remove the Gray components that are already part of the editedPageTokens list
            editedPageTokens.remove(editedPageTokens.size() - 1)

            ' Add the new CMYK color
            editedPageTokens.add(New COSFloat(cmyk(0)))
            editedPageTokens.add(New COSFloat(cmyk(1)))
            editedPageTokens.add(New COSFloat(cmyk(2)))
            editedPageTokens.add(New COSFloat(cmyk(2)))
        End Sub

        Private Shared Function convertRGBToCMYK(ByVal red As Single, ByVal green As Single, ByVal blue As Single) As Single()
            '//
            '// RGB->CMYK from From
            '// http://en.wikipedia.org/wiki/Talk:CMYK_color_model
            '//
            Dim c As Single = 1.0F - red
            Dim m As Single = 1.0F - green
            Dim y As Single = 1.0F - blue
            Dim k As Single = 1.0F

            k = Math.Min(Math.Min(Math.Min(c, k), m), y)

            c = (c - k) / (1 - k)
            m = (m - k) / (1 - k)
            y = (y - k) / (1 - k)
            Return New Single() {c, m, y, k}
        End Function

        Private Shared Function stringToIntArray(ByVal [string] As String) As Integer()
            Dim ints() As String = [string].Split(",")
            Dim retval() As Integer = Arrays.CreateInstance(Of Integer)(ints.Length)
            For i As Integer = 0 To ints.Length - 1
                retval(i) = Integer.Parse(ints(i))
            Next
            Return retval
        End Function

#If 0 Then

        '/**
        ' * Infamous main method.
        ' *
        ' * @param args Command line arguments, should be one and a reference to a file.
        ' *
        ' * @throws Exception If there is an error parsing the document.
        ' */
        Public Shared Sub main(ByVal args() As String)  'throws Exception
            Dim password As String = ""
            Dim inputFile As String = vbNullString
            Dim outputFile As String = vbNullString
            Dim destColorspace As String = "CMYK"

            Dim colorEquivalentPattern As Pattern = Pattern.compile("^(.*):\((.*)\)=(.*):\((.*)\)$")
            Dim colorEquivalentMatcher As Matcher = Nothing

            'key= value=java.awt.Color
            Dim colorEquivalents As New Hashtable()

            For i As Integer = 0 To args.Length - 1
                If (args(i).Equals(password)) Then
                    i += 1
                    If (i >= args.Length) Then
                        usage()
                    End If
                    password = args(i)
                End If
                If (args(i).Equals(DEST_COLORSPACE)) Then
                    i += 1
                    If (i >= args.Length) Then
                        usage()
                    End If
                    destColorspace = args(i)
                End If
                If (args(i).Equals(CONVERSION)) Then
                    i += 1
                    If (i >= args.Length) Then
                        usage()
                    End If

                    colorEquivalentMatcher = colorEquivalentPattern.matcher(args(i))
                    If (Not colorEquivalentMatcher.matches()) Then
                        usage()
                    End If
                    Dim srcColorSpace As String = colorEquivalentMatcher.group(1)
                    Dim srcColorvalues As String = colorEquivalentMatcher.group(2)
                    destColorspace = colorEquivalentMatcher.group(3)
                    Dim destColorvalues As String = colorEquivalentMatcher.group(4)

                    Dim source As ColorSpaceInstance = New ColorSpaceInstance()
                    source.colorspace = srcColorSpace
                    source.colorspaceValues = stringToIntArray(srcColorvalues)

                    Dim dest As ColorSpaceInstance = New ColorSpaceInstance()
                    dest.colorspace = destColorSpace
                    dest.colorspaceValues = stringToIntArray(destColorvalues)

                    colorEquivalents.put(source, dest)

                Else
                    If (inputFile Is Nothing) Then
                        inputFile = args(i)
                    Else
                        outputFile = args(i)
                    End If
                End If
            Next

            If (inputFile Is Nothing) Then
                usage()
            End If

            If (outputFile Is Nothing OrElse outputFile.Equals(inputFile)) Then
                usage()
            End If

            Dim doc As PDDocument = Nothing
            Try
                doc = PDDocument.load(inputFile)
                If (doc.isEncrypted()) Then
                    Try
                        doc.decrypt(password)
                    Catch e As InvalidPasswordException
                        If (Not password.Equals("")) Then '//they supplied the wrong password
                            Debug.Print("Error: The supplied password is incorrect.")
                            Stop
                        Else
                            'they didn't suppply a password and the default of "" was wrong.
                            Debug.Print("Error: The document is encrypted.")
                            usage()
                        End If
                    End Try
                End If
                'Dim converter As New ConvertColorspace()
                replaceColors(doc, colorEquivalents, destColorspace)
                doc.save(outputFile)

            Finally
                If (doc IsNot Nothing) Then
                    doc.close()
                End If
            End Try


        End Sub

#End If

        '/**
        ' * This will print the usage requirements and exit.
        '*/
        Private Shared Sub usage()
            Debug.Print("Usage: java org.apache.pdfbox.ConvertColorspace [OPTIONS] <PDF Input file> " & _
                "<PDF Output File>\n" & _
                "  -password  <password>                Password to decrypt document\n" & _
                "  -equiv <color equivalent>            Color equivalent to use for conversion.\n" & _
                "  -destColorspace <color equivalent>   The destination colorspace, CMYK is the only '" & _
                "supported colorspace." & _
                "  \n" & _
                " The equiv format is : <source colorspace>:(colorspace value)=<dest colorspace>:(colorspace value)" & _
                " This option can be used as many times as necessary\n" & _
                " The supported equiv colorspaces are RGB and CMYK.\n" & _
                " RGB color values are integers between 0 and 255" & _
                " CMYK color values are integer between 0 and 100.\n" & _
                " Example: java org.apache.pdfbox.ConvertColorspace -equiv RGB:(255,0,0)=CMYK(0,99,100,0)" & _
                " input.pdf output.pdf\n" & _
                "  <PDF Input file>             The PDF document to use\n" & _
                "  <PDF Output file>            The PDF file to write the result to. Must be different of input file" & vbNewLine)
            Stop
        End Sub

        Private Class ColorSpaceInstance
            Public colorspace As String = vbNullString
            Public colorspaceValues() As Integer = Nothing

            Public Overrides Function GetHashCode() As Integer
                Dim code As Integer = colorspace.GetHashCode
                For i As Integer = 0 To colorspaceValues.Length - 1
                    code += colorspaceValues(i)
                Next
                Return code
            End Function

            Public Overrides Function Equals(ByVal o As Object) As Boolean
                Dim retval As Boolean = False
                If (TypeOf (o) Is ColorSpaceInstance) Then
                    Dim other As ColorSpaceInstance = o
                    If (Me.colorspace.Equals(other.colorspace) AndAlso colorspaceValues.Length = other.colorspaceValues.Length) Then
                        retval = True
                        For i As Integer = 0 To colorspaceValues.Length - 1
                            retval = retval AndAlso colorspaceValues(i) = other.colorspaceValues(i)
                            If (Not retval) Then Exit For
                        Next
                    End If
                End If
                Return retval
            End Function
        End Class


    End Class

End Namespace



Imports FinSeA.Io
Imports System.IO
Imports FinSeA.Text
Imports FinSeA.org.fontbox.util
Imports FinSeA.Drawings
Imports FinSeA.Exceptions

Namespace org.fontbox.afm


    '/**
    ' * This class is used to parse AFM(Adobe Font Metrics) documents.
    ' *
    ' * @see <A href="http://partners.adobe.com/asn/developer/type/">AFM Documentation</A>
    ' *
    ' * @author Ben Litchfield (ben@benlitchfield.com)
    ' * @version $Revision: 1.1 $
    ' */
    Public Class AFMParser

        ''' <summary>
        ''' This is a comment in a AFM file.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const COMMENT = "Comment"

        ''' <summary>
        ''' This is the constant used in the AFM file to start a font metrics item.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const START_FONT_METRICS = "StartFontMetrics"

        ''' <summary>
        ''' This is the constant used in the AFM file to end a font metrics item.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const END_FONT_METRICS = "EndFontMetrics"

        ''' <summary>
        ''' This is the font name.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const FONT_NAME = "FontName"


        ''' <summary>
        ''' This is the full name.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const FULL_NAME = "FullName"

        ''' <summary>
        ''' This is the Family name.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const FAMILY_NAME = "FamilyName"

        ''' <summary>
        ''' This is the weight.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const WEIGHT = "Weight"

        ''' <summary>
        ''' This is the font bounding box.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const FONT_BBOX = "FontBBox"

        ''' <summary>
        ''' This is the version of the font.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const VERSION = "Version"

        ''' <summary>
        ''' This is the notice.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const NOTICE = "Notice"

        ''' <summary>
        ''' This is the encoding scheme.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const ENCODING_SCHEME = "EncodingScheme"

        ''' <summary>
        ''' This is the mapping scheme.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const MAPPING_SCHEME = "MappingScheme"

        ''' <summary>
        ''' This is the escape character.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const ESC_CHAR = "EscChar"

        ''' <summary>
        ''' This is the character set.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const CHARACTER_SET = "CharacterSet"

        ''' <summary>
        ''' This is the characters attribute.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const CHARACTERS = "Characters"

        ''' <summary>
        ''' This will determine if this is a base font.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const IS_BASE_FONT = "IsBaseFont"

        ''' <summary>
        ''' This is the V Vector attribute.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const V_VECTOR = "VVector"

        ''' <summary>
        ''' This will tell if the V is fixed.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const IS_FIXED_V = "IsFixedV"

        ''' <summary>
        ''' This is the cap height attribute.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const CAP_HEIGHT = "CapHeight"

        ''' <summary>
        ''' This is the X height.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const X_HEIGHT = "XHeight"

        ''' <summary>
        ''' This is ascender attribute.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const ASCENDER = "Ascender"

        ''' <summary>
        ''' This is the descender attribute.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const DESCENDER = "Descender"

        ''' <summary>
        ''' The underline position.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const UNDERLINE_POSITION = "UnderlinePosition"

        ''' <summary>
        ''' This is the Underline thickness.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const UNDERLINE_THICKNESS = "UnderlineThickness"

        ''' <summary>
        ''' This is the italic angle.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const ITALIC_ANGLE = "ItalicAngle"

        ''' <summary>
        ''' This is the char width.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const CHAR_WIDTH = "CharWidth"

        ''' <summary>
        ''' This will determine if this is fixed pitch.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const IS_FIXED_PITCH = "IsFixedPitch"

        ''' <summary>
        ''' This is the start of character metrics.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const START_CHAR_METRICS = "StartCharMetrics"

        ''' <summary>
        ''' This is the end of character metrics.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const END_CHAR_METRICS = "EndCharMetrics"

        ''' <summary>
        ''' The character metrics c value.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const CHARMETRICS_C = "C"

        ''' <summary>
        ''' The character metrics c value.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const CHARMETRICS_CH = "CH"

        ''' <summary>
        ''' The character metrics value.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const CHARMETRICS_WX = "WX"

        ''' <summary>
        ''' The character metrics value.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const CHARMETRICS_W0X = "W0X"

        ''' <summary>
        ''' The character metrics value.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const CHARMETRICS_W1X = "W1X"

        ''' <summary>
        ''' The character metrics value.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const CHARMETRICS_WY = "WY"

        ''' <summary>
        ''' The character metrics value.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const CHARMETRICS_W0Y = "W0Y"

        ''' <summary>
        ''' The character metrics value.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const CHARMETRICS_W1Y = "W1Y"

        ''' <summary>
        ''' The character metrics value.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const CHARMETRICS_W = "W"

        ''' <summary>
        ''' The character metrics value.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const CHARMETRICS_W0 = "W0"

        ''' <summary>
        ''' The character metrics value.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const CHARMETRICS_W1 = "W1"

        ''' <summary>
        ''' The character metrics value.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const CHARMETRICS_VV = "VV"

        ''' <summary>
        ''' The character metrics value.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const CHARMETRICS_N = "N"

        ''' <summary>
        ''' The character metrics value.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const CHARMETRICS_B = "B"

        ''' <summary>
        ''' The character metrics value.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const CHARMETRICS_L = "L"

        ''' <summary>
        ''' The character metrics value.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const STD_HW = "StdHW"

        ''' <summary>
        ''' The character metrics value.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const STD_VW = "StdVW"

        ''' <summary>
        ''' This is the start of track kern data.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const START_TRACK_KERN = "StartTrackKern"

        ''' <summary>
        ''' This is the end of track kern data.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const END_TRACK_KERN = "EndTrackKern"

        ''' <summary>
        ''' This is the start of kern data.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const START_KERN_DATA = "StartKernData"

        ''' <summary>
        ''' This is the end of kern data.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const END_KERN_DATA = "EndKernData"

        ''' <summary>
        ''' This is the start of kern pairs data.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const START_KERN_PAIRS = "StartKernPairs"

        ''' <summary>
        ''' This is the end of kern pairs data.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const END_KERN_PAIRS = "EndKernPairs"

        ''' <summary>
        ''' This is the start of kern pairs data.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const START_KERN_PAIRS0 = "StartKernPairs0"

        ''' <summary>
        ''' This is the start of kern pairs data.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const START_KERN_PAIRS1 = "StartKernPairs1"

        ''' <summary>
        ''' This is the start compisites data section.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const START_COMPOSITES = "StartComposites"

        ''' <summary>
        ''' This is the end compisites data section.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const END_COMPOSITES = "EndComposites"

        ''' <summary>
        ''' This is a composite character.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const CC = "CC"

        ''' <summary>
        ''' This is a composite charater part.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const PCC = "PCC"

        ''' <summary>
        ''' This is a kern pair.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const KERN_PAIR_KP = "KP"

        ''' <summary>
        ''' This is a kern pair.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const KERN_PAIR_KPH = "KPH"

        ''' <summary>
        ''' This is a kern pair.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const KERN_PAIR_KPX = "KPX"

        ''' <summary>
        ''' This is a kern pair.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const KERN_PAIR_KPY = "KPY"

        Private Const BITS_IN_HEX = 16


        Private input As InputStream
        Private result As FontMetric

        '/**
        ' * A method to test parsing of all AFM documents in the resources
        ' * directory.
        ' *
        ' * @param args Ignored.
        ' *
        ' * @throws IOException If there is an error parsing one of the documents.
        ' */
        Public Shared Sub main(ByVal args() As String)  'throws IOException
            Dim afmDir As New System.IO.DirectoryInfo("Resources/afm")
            Dim files As System.IO.FileInfo() = afmDir.GetFiles
            For i As Integer = 0 To files.Length - 1
                If (files(i).FullName().ToUpper().EndsWith(".AFM")) Then
                    Dim start As Double = Timer
                    Dim input As New FileInputStream(files(i).FullName)
                    Dim parser As New AFMParser(input)
                    parser.parse()
                    Dim [stop] As Double = Timer
                    Debug.Print("Parsing:" & files(i).FullName & " " & ([stop] - start))
                End If
            Next
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param in The input stream to read the AFM document from.
        ' */
        Public Sub New(ByVal [in] As InputStream)
            input = [in]
        End Sub

        '/**
        ' * This will parse the AFM document.  This will close the Input stream
        ' * when the parsing is finished.
        ' *
        ' * @throws IOException If there is an IO error reading the document.
        ' */
        Public Sub parse() 'throws IOException
            result = parseFontMetric()
        End Sub

        '/**
        ' * This will get the result of the parsing.
        ' *
        ' * @return The parsed java object.
        ' */
        Public Function getResult() As FontMetric
            Return result
        End Function

        '/**
        ' * This will parse a font metrics item.
        ' *
        ' * @return The parse font metrics item.
        ' *
        ' * @throws IOException If there is an error reading the AFM file.
        ' */
        Private Function parseFontMetric() As FontMetric ' throws IOException
            Dim fontMetrics As New FontMetric()
            Dim startFontMetrics As String = readString()
            If (Not START_FONT_METRICS.Equals(startFontMetrics)) Then
                Throw New IOException("Error: The AFM file should start with " & START_FONT_METRICS & " and not '" & startFontMetrics & "'")
            End If
            fontMetrics.setAFMVersion(readFloat())
            Dim nextCommand As String
            nextCommand = readString()
            While (Not END_FONT_METRICS.Equals(nextCommand))
                If (FONT_NAME.Equals(nextCommand)) Then
                    fontMetrics.setFontName(readLine())
                ElseIf (FULL_NAME.Equals(nextCommand)) Then
                    fontMetrics.setFullName(readLine())
                ElseIf (FAMILY_NAME.Equals(nextCommand)) Then
                    fontMetrics.setFamilyName(readLine())
                ElseIf (WEIGHT.Equals(nextCommand)) Then
                    fontMetrics.setWeight(readLine())
                ElseIf (FONT_BBOX.Equals(nextCommand)) Then
                    Dim bBox As New BoundingBox()
                    bBox.setLowerLeftX(readFloat())
                    bBox.setLowerLeftY(readFloat())
                    bBox.setUpperRightX(readFloat())
                    bBox.setUpperRightY(readFloat())
                    fontMetrics.setFontBBox(bBox)
                ElseIf (VERSION.Equals(nextCommand)) Then
                    fontMetrics.setFontVersion(readLine())
                ElseIf (NOTICE.Equals(nextCommand)) Then
                    fontMetrics.setNotice(readLine())
                ElseIf (ENCODING_SCHEME.Equals(nextCommand)) Then
                    fontMetrics.setEncodingScheme(readLine())
                ElseIf (MAPPING_SCHEME.Equals(nextCommand)) Then
                    fontMetrics.setMappingScheme(readInt())
                ElseIf (ESC_CHAR.Equals(nextCommand)) Then
                    fontMetrics.setEscChar(readInt())
                ElseIf (CHARACTER_SET.Equals(nextCommand)) Then
                    fontMetrics.setCharacterSet(readLine())
                ElseIf (CHARACTERS.Equals(nextCommand)) Then
                    fontMetrics.setCharacters(readInt())
                ElseIf (IS_BASE_FONT.Equals(nextCommand)) Then
                    fontMetrics.setIsBaseFont(readBoolean())
                ElseIf (V_VECTOR.Equals(nextCommand)) Then
                    Dim vector(2 - 1) As Single
                    vector(0) = readFloat()
                    vector(1) = readFloat()
                    fontMetrics.setVVector(vector)
                ElseIf (IS_FIXED_V.Equals(nextCommand)) Then
                    fontMetrics.setIsFixedV(readBoolean())
                ElseIf (CAP_HEIGHT.Equals(nextCommand)) Then
                    fontMetrics.setCapHeight(readFloat())
                ElseIf (X_HEIGHT.Equals(nextCommand)) Then
                    fontMetrics.setXHeight(readFloat())
                ElseIf (ASCENDER.Equals(nextCommand)) Then
                    fontMetrics.setAscender(readFloat())
                ElseIf (DESCENDER.Equals(nextCommand)) Then
                    fontMetrics.setDescender(readFloat())
                ElseIf (STD_HW.Equals(nextCommand)) Then
                    fontMetrics.setStandardHorizontalWidth(readFloat())
                ElseIf (STD_VW.Equals(nextCommand)) Then
                    fontMetrics.setStandardVerticalWidth(readFloat())
                ElseIf (COMMENT.Equals(nextCommand)) Then
                    fontMetrics.addComment(readLine())
                ElseIf (UNDERLINE_POSITION.Equals(nextCommand)) Then
                    fontMetrics.setUnderlinePosition(readFloat())
                ElseIf (UNDERLINE_THICKNESS.Equals(nextCommand)) Then
                    fontMetrics.setUnderlineThickness(readFloat())
                ElseIf (ITALIC_ANGLE.Equals(nextCommand)) Then
                    fontMetrics.setItalicAngle(readFloat())
                ElseIf (CHAR_WIDTH.Equals(nextCommand)) Then
                    Dim widths(2 - 1) As Single
                    widths(0) = readFloat()
                    widths(1) = readFloat()
                    fontMetrics.setCharWidth(widths)
                ElseIf (IS_FIXED_PITCH.Equals(nextCommand)) Then
                    fontMetrics.setFixedPitch(readBoolean())
                ElseIf (START_CHAR_METRICS.Equals(nextCommand)) Then
                    Dim count As Integer = readInt()
                    For i As Integer = 0 To count - 1
                        Dim charMetric As CharMetric = parseCharMetric()
                        fontMetrics.addCharMetric(charMetric)
                    Next
                    Dim [end] As String = readString()
                    If (Not [end].Equals(END_CHAR_METRICS)) Then
                        Throw New IOException("Error: Expected '" & END_CHAR_METRICS & "' actual '" & [end] & "'")
                    End If
                ElseIf (START_COMPOSITES.Equals(nextCommand)) Then
                    Dim count As Integer = readInt()
                    For i As Integer = 0 To count - 1
                        Dim part As Composite = parseComposite()
                        fontMetrics.addComposite(part)
                    Next
                    Dim [end] As String = readString()
                    If (Not [end].Equals(END_COMPOSITES)) Then
                        Throw New IOException("Error: Expected '" & END_COMPOSITES & "' actual '" & [end] & "'")
                    End If
                ElseIf (START_KERN_DATA.Equals(nextCommand)) Then
                    parseKernData(fontMetrics)
                Else
                    Throw New IOException("Unknown AFM key '" & nextCommand & "'")
                End If
                nextCommand = readString()
            End While
            Return fontMetrics
        End Function

        '/**
        ' * This will parse the kern data.
        ' *
        ' * @param fontMetrics The metrics class to put the parsed data into.
        ' *
        ' * @throws IOException If there is an error parsing the data.
        ' */
        Private Sub parseKernData(ByVal fontMetrics As FontMetric)  'throws IOException
            Dim nextCommand As String
            nextCommand = readString()
            While (Not nextCommand.Equals(END_KERN_DATA))
                If (START_TRACK_KERN.Equals(nextCommand)) Then
                    Dim count As Integer = readInt()
                    For i As Integer = 0 To count - 1
                        Dim kern As TrackKern = New TrackKern()
                        kern.setDegree(readInt())
                        kern.setMinPointSize(readFloat())
                        kern.setMinKern(readFloat())
                        kern.setMaxPointSize(readFloat())
                        kern.setMaxKern(readFloat())
                        fontMetrics.addTrackKern(kern)
                    Next
                    Dim [end] As String = readString()
                    If (Not [end].Equals(END_TRACK_KERN)) Then
                        Throw New IOException("Error: Expected '" & END_TRACK_KERN & "' actual '" & [end] & "'")
                    End If
                ElseIf (START_KERN_PAIRS.Equals(nextCommand)) Then
                    Dim count As Integer = readInt()
                    For i As Integer = 0 To count - 1
                        Dim pair As KernPair = parseKernPair()
                        fontMetrics.addKernPair(pair)
                    Next
                    Dim [end] As String = readString()
                    If (Not [end].Equals(END_KERN_PAIRS)) Then
                        Throw New IOException("Error: Expected '" & END_KERN_PAIRS & "' actual '" & [end] & "'")
                    End If
                ElseIf (START_KERN_PAIRS0.Equals(nextCommand)) Then
                    Dim count As Integer = readInt()
                    For i As Integer = 0 To count - 1
                        Dim pair As KernPair = parseKernPair()
                        fontMetrics.addKernPair0(pair)
                    Next
                    Dim [end] As String = readString()
                    If (Not [end].Equals(END_KERN_PAIRS)) Then
                        Throw New IOException("Error: Expected '" & END_KERN_PAIRS & "' actual '" & [end] & "'")
                    End If
                ElseIf (START_KERN_PAIRS1.Equals(nextCommand)) Then
                    Dim count As Integer = readInt()
                    For i As Integer = 0 To count - 1
                        Dim pair As KernPair = parseKernPair()
                        fontMetrics.addKernPair1(pair)
                    Next
                    Dim [end] As String = readString()
                    If (Not [end].Equals(END_KERN_PAIRS)) Then
                        Throw New IOException("Error: Expected '" & END_KERN_PAIRS & "' actual '" & [end] & "'")
                    End If
                Else
                    Throw New IOException("Unknown kerning data type '" & nextCommand & "'")
                End If
                nextCommand = readString()
            End While
        End Sub

        '/**
        ' * This will parse a kern pair from the data stream.
        ' *
        ' * @return The kern pair that was parsed from the stream.
        ' *
        ' * @throws IOException If there is an error reading from the stream.
        ' */
        Private Function parseKernPair() As KernPair ' throws IOException
            Dim kernPair As New KernPair()
            Dim cmd As String = readString()
            If (KERN_PAIR_KP.Equals(cmd)) Then
                Dim first As String = readString()
                Dim second As String = readString()
                Dim x As Single = readFloat()
                Dim y As Single = readFloat()
                kernPair.setFirstKernCharacter(first)
                kernPair.setSecondKernCharacter(second)
                kernPair.setX(x)
                kernPair.setY(y)
            ElseIf (KERN_PAIR_KPH.Equals(cmd)) Then
                Dim first As String = hexToString(readString())
                Dim second As String = hexToString(readString())
                Dim x As Single = readFloat()
                Dim y As Single = readFloat()
                kernPair.setFirstKernCharacter(first)
                kernPair.setSecondKernCharacter(second)
                kernPair.setX(x)
                kernPair.setY(y)
            ElseIf (KERN_PAIR_KPX.Equals(cmd)) Then
                Dim first As String = readString()
                Dim second As String = readString()
                Dim x As Single = readFloat()
                kernPair.setFirstKernCharacter(first)
                kernPair.setSecondKernCharacter(second)
                kernPair.setX(x)
                kernPair.setY(0)
            ElseIf (KERN_PAIR_KPY.Equals(cmd)) Then
                Dim first As String = readString()
                Dim second As String = readString()
                Dim y As Single = readFloat()
                kernPair.setFirstKernCharacter(first)
                kernPair.setSecondKernCharacter(second)
                kernPair.setX(0)
                kernPair.setY(y)
            Else
                Throw New IOException("Error expected kern pair command actual='" & cmd & "'")
            End If
            Return kernPair
        End Function

        '/**
        ' * This will convert and angle bracket hex string to a string.
        ' *
        ' * @param hexString An angle bracket string.
        ' *
        ' * @return The bytes of the hex string.
        ' *
        ' * @throws IOException If the string is in an invalid format.
        ' */
        Private Function hexToString(ByVal hexString As String) As String 'throws IOException
            If (hexString.length() < 2) Then
                Throw New IOException("Error: Expected hex string of length >= 2 not='" & hexString)
            End If
            If (hexString.Chars(0) <> "<" OrElse hexString.Chars(hexString.Length() - 1) <> ">") Then
                Throw New IOException("String should be enclosed by angle brackets '" & hexString & "'")
            End If
            hexString = hexString.Substring(1, hexString.Length() - 1)
            Dim data() As Byte = Array.CreateInstance(GetType(Byte), (hexString.Length() \ 2))
            For i As Integer = 0 To hexString.Length() Step 2 '; i+=2 )
                Dim hex As String = "" & hexString.Chars(i) & hexString.Chars(i + 1)
                Try
                    data(i / 2) = Integer.Parse(hex, BITS_IN_HEX)
                Catch e As NumberFormatException
                    Throw New IOException("Error parsing AFM file:" & e.ToString)
                End Try
            Next
            Return Strings.GetString(data)
        End Function

        '/**
        ' * This will parse a composite part from the stream.
        ' *
        ' * @return The composite.
        ' *
        ' * @throws IOException If there is an error parsing the composite.
        ' */
        Private Function parseComposite() As Composite ' throws IOException
            Dim composite As Composite = New Composite()
            Dim partData As String = readLine()
            Dim tokenizer As New StringTokenizer(partData, " ;")


            Dim cc As String = tokenizer.nextToken()
            If (Not cc.Equals(cc)) Then
                Throw New IOException("Expected '" & cc & "' actual='" & cc & "'")
            End If
            Dim name As String = tokenizer.nextToken()
            composite.setName(name)

            Dim partCount As Integer
            Try
                partCount = Integer.Parse(tokenizer.nextToken())
            Catch e As NumberFormatException
                Throw New IOException("Error parsing AFM document:" & e.ToString)
            End Try
            For i As Integer = 0 To partCount - 1
                Dim part As CompositePart = New CompositePart()
                Dim pcc As String = tokenizer.nextToken()
                If (Not pcc.Equals(pcc)) Then
                    Throw New IOException("Expected '" & pcc & "' actual='" & pcc & "'")
                End If
                Dim partName As String = tokenizer.nextToken()
                Try
                    Dim x As Integer = Integer.Parse(tokenizer.nextToken())
                    Dim y As Integer = Integer.Parse(tokenizer.nextToken())

                    part.setName(partName)
                    part.setXDisplacement(x)
                    part.setYDisplacement(y)
                    composite.addPart(part)
                Catch e As NumberFormatException
                    Throw New IOException("Error parsing AFM document:" & e.ToString)
                End Try
            Next
            Return composite
        End Function

        '/**
        ' * This will parse a single CharMetric object from the stream.
        ' *
        ' * @return The next char metric in the stream.
        ' *
        ' * @throws IOException If there is an error reading from the stream.
        ' */
        Private Function parseCharMetric() As CharMetric ' throws IOException
            Dim charMetric As CharMetric = New CharMetric()
            Dim metrics As String = readLine()
            Dim metricsTokenizer As StringTokenizer = New StringTokenizer(metrics)
            Try
                While (metricsTokenizer.hasMoreTokens())
                    Dim nextCommand As String = metricsTokenizer.nextToken()
                    If (nextCommand.Equals(CHARMETRICS_C)) Then
                        Dim charCode As String = metricsTokenizer.nextToken()
                        charMetric.setCharacterCode(Integer.Parse(charCode))
                        verifySemicolon(metricsTokenizer)
                    ElseIf (nextCommand.Equals(CHARMETRICS_CH)) Then
                        'Is the hex string <FF> or FF, the spec is a little
                        'unclear, wait and see if it breaks anything.
                        Dim charCode As String = metricsTokenizer.nextToken()
                        charMetric.setCharacterCode(Integer.Parse(charCode, BITS_IN_HEX))
                        verifySemicolon(metricsTokenizer)
                    ElseIf (nextCommand.Equals(CHARMETRICS_WX)) Then
                        Dim wx As String = metricsTokenizer.nextToken()
                        charMetric.setWx(Single.Parse(wx))
                        verifySemicolon(metricsTokenizer)
                    ElseIf (nextCommand.Equals(CHARMETRICS_W0X)) Then
                        Dim w0x As String = metricsTokenizer.nextToken()
                        charMetric.setW0x(Single.Parse(w0x))
                        verifySemicolon(metricsTokenizer)
                    ElseIf (nextCommand.Equals(CHARMETRICS_W1X)) Then
                        Dim w1x As String = metricsTokenizer.nextToken()
                        charMetric.setW0x(Single.Parse(w1x))
                        verifySemicolon(metricsTokenizer)
                    ElseIf (nextCommand.Equals(CHARMETRICS_WY)) Then
                        Dim wy As String = metricsTokenizer.nextToken()
                        charMetric.setWy(Single.Parse(wy))
                        verifySemicolon(metricsTokenizer)
                    ElseIf (nextCommand.Equals(CHARMETRICS_W0Y)) Then
                        Dim w0y As String = metricsTokenizer.nextToken()
                        charMetric.setW0y(Single.Parse(w0y))
                        verifySemicolon(metricsTokenizer)
                    ElseIf (nextCommand.Equals(CHARMETRICS_W1Y)) Then
                        Dim w1y As String = metricsTokenizer.nextToken()
                        charMetric.setW0y(Single.Parse(w1y))
                        verifySemicolon(metricsTokenizer)
                    ElseIf (nextCommand.Equals(CHARMETRICS_W)) Then
                        Dim w0 As String = metricsTokenizer.nextToken()
                        Dim w1 As String = metricsTokenizer.nextToken()
                        Dim w(2 - 1) As Single
                        w(0) = Single.Parse(w0)
                        w(1) = Single.Parse(w1)
                        charMetric.setW(w)
                        verifySemicolon(metricsTokenizer)
                    ElseIf (nextCommand.Equals(CHARMETRICS_W0)) Then
                        Dim w00 As String = metricsTokenizer.nextToken()
                        Dim w01 As String = metricsTokenizer.nextToken()
                        Dim w0(2 - 1) As Single
                        w0(0) = Single.Parse(w00)
                        w0(1) = Single.Parse(w01)
                        charMetric.setW0(w0)
                        verifySemicolon(metricsTokenizer)
                    ElseIf (nextCommand.Equals(CHARMETRICS_W1)) Then
                        Dim w10 As String = metricsTokenizer.nextToken()
                        Dim w11 As String = metricsTokenizer.nextToken()
                        Dim w1(2 - 1) As Single
                        w1(0) = Single.Parse(w10)
                        w1(1) = Single.Parse(w11)
                        charMetric.setW1(w1)
                        verifySemicolon(metricsTokenizer)
                    ElseIf (nextCommand.Equals(CHARMETRICS_VV)) Then
                        Dim vv0 As String = metricsTokenizer.nextToken()
                        Dim vv1 As String = metricsTokenizer.nextToken()
                        Dim vv(2 - 1) As Single
                        vv(0) = Single.Parse(vv0)
                        vv(1) = Single.Parse(vv1)
                        charMetric.setVv(vv)
                        verifySemicolon(metricsTokenizer)
                    ElseIf (nextCommand.Equals(CHARMETRICS_N)) Then
                        Dim name As String = metricsTokenizer.nextToken()
                        charMetric.setName(name)
                        verifySemicolon(metricsTokenizer)
                    ElseIf (nextCommand.Equals(CHARMETRICS_B)) Then
                        Dim llx As String = metricsTokenizer.nextToken()
                        Dim lly As String = metricsTokenizer.nextToken()
                        Dim urx As String = metricsTokenizer.nextToken()
                        Dim ury As String = metricsTokenizer.nextToken()
                        Dim box As BoundingBox = New BoundingBox()
                        box.setLowerLeftX(Single.Parse(llx))
                        box.setLowerLeftY(Single.Parse(lly))
                        box.setUpperRightX(Single.Parse(urx))
                        box.setUpperRightY(Single.Parse(ury))
                        charMetric.setBoundingBox(box)
                        verifySemicolon(metricsTokenizer)
                    ElseIf (nextCommand.Equals(CHARMETRICS_L)) Then
                        Dim successor As String = metricsTokenizer.nextToken()
                        Dim ligature As String = metricsTokenizer.nextToken()
                        Dim lig As Ligature = New Ligature()
                        lig.setSuccessor(successor)
                        lig.setLigature(ligature)
                        charMetric.addLigature(lig)
                        verifySemicolon(metricsTokenizer)
                    Else
                        Throw New IOException("Unknown CharMetrics command '" & nextCommand & "'")
                    End If
                End While
            Catch e As NumberFormatException
                Throw New IOException("Error: Corrupt AFM document:" & e.ToString)
            End Try
            Return charMetric
        End Function

        '/**
        ' * This is used to verify that a semicolon is the next token in the stream.
        ' *
        ' * @param tokenizer The tokenizer to read from.
        ' *
        ' * @throws IOException If the semicolon is missing.
        ' */
        Private Sub verifySemicolon(ByVal tokenizer As StringTokenizer)  'throws IOException
            If (tokenizer.hasMoreTokens()) Then
                Dim semicolon As String = tokenizer.nextToken()
                If (Not semicolon.Equals(";")) Then
                    Throw New IOException("Error: Expected semicolon in stream actual='" & semicolon & "'")
                End If
            Else
                Throw New IOException("CharMetrics is missing a semicolon after a command")
            End If
        End Sub

        '/**
        ' * This will read a boolean from the stream.
        ' *
        ' * @return The boolean in the stream.
        ' */
        Private Function readBoolean() As Boolean ' throws IOException
            Dim theBoolean As String = readString()
            Return Boolean.Parse(theBoolean)
        End Function

        '/**
        ' * This will read an integer from the stream.
        ' *
        ' * @return The integer in the stream.
        ' */
        Private Function readInt() As Integer ' throws IOException
            Dim theInt As String = readString()
            Try
                Return Integer.Parse(theInt)
            Catch e As NumberFormatException
                Throw New IOException("Error parsing AFM document:" & e.ToString)
            End Try
        End Function

        '/**
        ' * This will read a float from the stream.
        ' *
        ' * @return The float in the stream.
        ' */
        Private Function readFloat() As Single ' throws IOException
            Dim theFloat As String = readString()
            Return Single.Parse(theFloat)
        End Function

        '/**
        ' * This will read until the end of a line.
        ' *
        ' * @return The string that is read.
        ' */
        Private Function readLine() As String ' throws IOException
            'First skip the whitespace
            Dim buf As StringBuffer = New StringBuffer()
            Dim nextByte As Integer = input.read()
            While (isWhitespace(nextByte))
                nextByte = input.read()
                'do nothing just skip the whitespace.
            End While
            buf.append(Convert.ToChar(nextByte))

            'now read the data
            nextByte = input.read()
            While (Not isEOL(nextByte))
                buf.append(Convert.ToChar(nextByte))
                nextByte = input.read()
            End While
            Return buf.toString()
        End Function

        '/**
        ' * This will read a string from the input stream and stop at any whitespace.
        ' *
        ' * @return The string read from the stream.
        ' *
        ' * @throws IOException If an IO error occurs when reading from the stream.
        ' */
        Private Function readString() As String ' throws IOException
            'First skip the whitespace
            Dim buf As StringBuffer = New StringBuffer()
            Dim nextByte As Integer = input.read()
            While (isWhitespace(nextByte))
                nextByte = input.read()
                'do nothing just skip the whitespace.
            End While
            buf.append(Convert.ToChar(nextByte))

            'now read the data
            nextByte = input.read()
            While (Not isWhitespace(nextByte))
                buf.append(Convert.ToChar(nextByte))
                nextByte = input.read()
            End While
            Return buf.toString()
        End Function

        '/**
        ' * This will determine if the byte is a whitespace character or not.
        ' *
        ' * @param character The character to test for whitespace.
        ' *
        ' * @return true If the character is whitespace as defined by the AFM spec.
        ' */
        Private Function isEOL(ByVal character As Integer) As Boolean
            Return character = &HD OrElse character = &HA
        End Function

        '/**
        ' * This will determine if the byte is a whitespace character or not.
        ' *
        ' * @param character The character to test for whitespace.
        ' *
        ' * @return true If the character is whitespace as defined by the AFM spec.
        ' */
        Private Function isWhitespace(ByVal character As Integer) As Boolean
            Return character = " " OrElse character = vbTab OrElse character = &HD OrElse character = &HA
        End Function

    End Class

End Namespace
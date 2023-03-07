Imports FinSeA.Sistema
Imports System.IO
Imports FinSeA.Io
Imports FinSeA.org.apache.fontbox.util
Imports FinSeA.Text


Namespace org.apache.fontbox.cmap

    '/**
    ' * This will parse a CMap stream.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * 
    ' */
    Public Class CMapParser

        Private Const BEGIN_CODESPACE_RANGE = "begincodespacerange"
        Private Const BEGIN_BASE_FONT_CHAR = "beginbfchar"
        Private Const BEGIN_BASE_FONT_RANGE = "beginbfrange"
        Private Const BEGIN_CID_CHAR = "begincidchar"
        Private Const BEGIN_CID_RANGE = "begincidrange"
        Private Const USECMAP = "usecmap"

        Private Const END_CODESPACE_RANGE = "endcodespacerange"
        Private Const END_BASE_FONT_CHAR = "endbfchar"
        Private Const END_BASE_FONT_RANGE = "endbfrange"
        Private Const END_CID_CHAR = "endcidchar"
        Private Const END_CID_RANGE = "endcidrange"

        Private Const WMODE = "WMode"
        Private Const CMAP_NAME = "CMapName"
        Private Const CMAP_VERSION = "CMapVersion"
        Private Const CMAP_TYPE = "CMapType"
        Private Const REGISTRY = "Registry"
        Private Const ORDERING = "Ordering"
        Private Const SUPPLEMENT = "Supplement"

        Private Const MARK_END_OF_DICTIONARY = ">>"
        Private Const MARK_END_OF_ARRAY = "]"

        Private tokenParserByteBuffer() As Byte = Array.CreateInstance(GetType(Byte), 512)

        '/**
        ' * Creates a new instance of CMapParser.
        ' */
        Public Sub New()
        End Sub


        '/**
        ' * Parse a CMAP file on the file system.
        ' * 
        ' * @param file The file to parse.
        ' * 
        ' * @return A parsed CMAP file.
        ' * 
        ' * @throws IOException If there is an issue while parsing the CMAP.
        ' */
        Public Function parse(ByVal file As FinSeA.Io.File) As CMap
            Dim rootDir As String = file.getParent() & FinSeA.Io.File.separator
            Dim input As FileInputStream = Nothing
            Try
                input = New FileInputStream(file)
                Return parse(rootDir, input)
            Finally
                If (input IsNot Nothing) Then
                    input.Close()
                End If
            End Try

        End Function

        '/**
        ' * This will parse the stream and create a cmap object.
        ' *
        ' * @param resourceRoot The root path to the cmap file.  This will be used
        ' *                     to find referenced cmap files.  It can be null.
        ' * @param input The CMAP stream to parse.
        ' * 
        ' * @return The parsed stream as a java object.
        ' *
        ' * @throws IOException If there is an error parsing the stream.
        ' */
        Public Function parse(ByVal resourceRoot As String, ByVal input As InputStream) As CMap
            Dim cmapStream As New PushBackInputStream(input)
            Dim result As New CMap()
            Dim previousToken As Object = Nothing
            Dim token As Object = Nothing
            token = parseNextToken(cmapStream)
            While (token IsNot Nothing)
                If (TypeOf (token) Is [Operator]) Then
                    Dim op As [Operator] = token
                    If (op.op.equals(USECMAP)) Then
                        Dim useCmapName As LiteralName = previousToken
                        Dim useStream As InputStream = ResourceLoader.loadResource(resourceRoot & useCmapName.name)
                        If (useStream Is Nothing) Then
                            Throw New IOException("Error: Could not find referenced cmap stream " & useCmapName.name)
                        End If
                        Dim useCMap As CMap = parse(resourceRoot, useStream)
                        result.useCmap(useCMap)
                    ElseIf (op.op.equals(BEGIN_CODESPACE_RANGE)) Then
                        Dim cosCount As Number = previousToken
                        For j As Integer = 0 To cosCount.intValue() - 1
                            Dim nextToken As Object = parseNextToken(cmapStream)
                            If (TypeOf (nextToken) Is [Operator]) Then
                                If (Not DirectCast(nextToken, [Operator]).op.equals(END_CODESPACE_RANGE)) Then
                                    Throw New IOException("Error : ~codespacerange contains an unexpected operator : " & DirectCast(nextToken, [Operator]).op)
                                End If
                                Exit For
                            End If
                            Dim startRange() As Byte = nextToken
                            Dim endRange() As Byte = parseNextToken(cmapStream)
                            Dim range As New CodespaceRange()
                            range.setStart(startRange)
                            range.setEnd(endRange)
                            result.addCodespaceRange(range)
                        Next
                    ElseIf (op.op.equals(BEGIN_BASE_FONT_CHAR)) Then
                        Dim cosCount As Number = previousToken
                        For j As Integer = 0 To cosCount.intValue() - 1
                            Dim nextToken As Object = parseNextToken(cmapStream)
                            If (TypeOf (nextToken) Is [Operator]) Then
                                If (Not DirectCast(nextToken, [Operator]).op.equals(END_BASE_FONT_CHAR)) Then
                                    Throw New IOException("Error : ~bfchar contains an unexpected operator : " & DirectCast(nextToken, [Operator]).op)
                                End If
                                Exit For
                            End If
                            Dim inputCode() As Byte = nextToken
                            nextToken = parseNextToken(cmapStream)
                            If (TypeOf (nextToken) Is Byte()) Then
                                Dim bytes() As Byte = nextToken
                                Dim value As String = createStringFromBytes(bytes)
                                result.addMapping(inputCode, value)
                            ElseIf (TypeOf (nextToken) Is LiteralName) Then
                                result.addMapping(inputCode, DirectCast(nextToken, LiteralName).name)
                            Else
                                Throw New IOException("Error parsing CMap beginbfchar, expected{COSString or COSName} and not " & nextToken.ToString)
                            End If
                        Next
                    ElseIf (op.op.equals(BEGIN_BASE_FONT_RANGE)) Then
                        Dim cosCount As Number = previousToken

                        For j As Integer = 0 To cosCount.intValue() - 1
                            Dim nextToken As Object = parseNextToken(cmapStream)
                            If (TypeOf (nextToken) Is [Operator]) Then
                                If (Not DirectCast(nextToken, [Operator]).op.equals(END_BASE_FONT_RANGE)) Then
                                    Throw New IOException("Error : ~bfrange contains an unexpected operator : " & DirectCast(nextToken, [Operator]).op)
                                End If
                                Exit For
                            End If
                            Dim startCode() As Byte = nextToken
                            Dim endCode() As Byte = parseNextToken(cmapStream)
                            nextToken = parseNextToken(cmapStream)
                            Dim array As List(Of Byte()) = Nothing
                            Dim tokenBytes() As Byte = Nothing
                            If (TypeOf (nextToken) Is List(Of Byte())) Then
                                array = nextToken
                                tokenBytes = array.get(0)
                            Else
                                tokenBytes = nextToken
                            End If
                            Dim done As Boolean = False
                            ' don't add 1:1 mappings to reduce the memory footprint
                            If (Sistema.Arrays.Compare(startCode, tokenBytes) = 0) Then
                                done = True
                            End If
                            Dim value As String = vbNullString

                            Dim arrayIndex As Integer = 0
                            While (Not done)
                                If (compare(startCode, endCode) >= 0) Then
                                    done = True
                                End If
                                value = createStringFromBytes(tokenBytes)
                                result.addMapping(startCode, value)
                                increment(startCode)

                                If (array Is Nothing) Then
                                    increment(tokenBytes)
                                Else
                                    arrayIndex += 1
                                    If (arrayIndex < array.size()) Then
                                        tokenBytes = array.get(arrayIndex)
                                    End If
                                End If
                            End While
                        Next
                    ElseIf (op.op.equals(BEGIN_CID_CHAR)) Then
                        Dim cosCount As Number = previousToken
                        For j As Integer = 0 To cosCount.intValue() - 1
                            Dim nextToken As Object = parseNextToken(cmapStream)
                            If (TypeOf (nextToken) Is [Operator]) Then
                                If (Not DirectCast(nextToken, [Operator]).op.equals(END_CID_CHAR)) Then
                                    Throw New IOException("Error : ~cidchar contains an unexpected operator : " & DirectCast(nextToken, [Operator]).op)
                                End If
                                Exit For
                            End If
                            Dim inputCode() As Byte = nextToken
                            Dim mappedCode As Integer = parseNextToken(cmapStream)
                            Dim mappedStr As String = createStringFromBytes(inputCode)
                            result.addCIDMapping(mappedCode, mappedStr)
                        Next
                    ElseIf (op.op.equals(BEGIN_CID_RANGE)) Then
                        Dim numberOfLines As Integer = previousToken
                        For n As Integer = 0 To numberOfLines - 1
                            Dim nextToken As Object = parseNextToken(cmapStream)
                            If (TypeOf (nextToken) Is [Operator]) Then
                                If (Not DirectCast(nextToken, [Operator]).op.equals(END_CID_RANGE)) Then
                                    Throw New IOException("Error : ~cidrange contains an unexpected operator : " & DirectCast(nextToken, [Operator]).op)
                                End If
                                Exit For
                            End If
                            Dim startCode() As Byte = nextToken
                            Dim start As Integer = createIntFromBytes(startCode)
                            Dim endCode() As Byte = parseNextToken(cmapStream)
                            Dim [end] As Integer = createIntFromBytes(endCode)
                            Dim mappedCode As Integer = parseNextToken(cmapStream)
                            If (startCode.Length <= 2 AndAlso endCode.Length <= 2) Then
                                result.addCIDRange(Convert.ToChar(start), Convert.ToChar([end]), mappedCode)
                            Else
                                ' TODO Is this even possible?
                                Dim endOfMappings As Integer = mappedCode + [end] - start
                                While (mappedCode <= endOfMappings)
                                    Dim mappedStr As String = createStringFromBytes(startCode)
                                    result.addCIDMapping(mappedCode, mappedStr) : mappedCode += 1
                                    increment(startCode)
                                End While
                            End If
                        Next
                    End If
                ElseIf (TypeOf (token) Is LiteralName) Then
                    Dim literal As LiteralName = token
                    If (WMODE.Equals(literal.name)) Then
                        Dim [next] As Object = parseNextToken(cmapStream)
                        If (TypeOf ([next]) Is NInteger) Then
                            result.setWMode([next])
                        End If
                    ElseIf (CMAP_NAME.Equals(literal.name)) Then
                        Dim [next] As Object = parseNextToken(cmapStream)
                        If (TypeOf ([next]) Is LiteralName) Then
                            result.setName(DirectCast([next], LiteralName).name)
                        End If
                    ElseIf (CMAP_VERSION.Equals(literal.name)) Then
                        Dim [next] As Object = parseNextToken(cmapStream)
                        If (TypeOf ([next]) Is Number) Then
                            result.setVersion(DirectCast([next], Number).ToString())
                        ElseIf (TypeOf ([next]) Is String) Then
                            result.setVersion(CStr([next]))
                        End If
                    ElseIf (CMAP_TYPE.Equals(literal.name)) Then
                        Dim [next] As Object = parseNextToken(cmapStream)
                        If (TypeOf ([next]) Is NInteger) Then
                            result.setType([next])
                        End If
                    ElseIf (REGISTRY.Equals(literal.name)) Then
                        Dim [next] As Object = parseNextToken(cmapStream)
                        If (TypeOf ([next]) Is String) Then
                            result.setRegistry([next])
                        End If
                    ElseIf (ORDERING.Equals(literal.name)) Then
                        Dim [next] As Object = parseNextToken(cmapStream)
                        If (TypeOf ([next]) Is String) Then
                            result.setOrdering([next])
                        End If
                    ElseIf (SUPPLEMENT.Equals(literal.name)) Then
                        Dim [next] As Object = parseNextToken(cmapStream)
                        If (TypeOf ([next]) Is NInteger) Then
                            result.setSupplement([next])
                        End If
                    End If
                End If
                previousToken = token
                token = parseNextToken(cmapStream)
            End While
            Return result
        End Function

        Private Function parseNextToken(ByVal [is] As PushBackInputStream) As Object
            Dim retval As Object = Nothing
            Dim nextByte As Integer = [is].read()
            ' skip whitespace
            While (nextByte = &H9 OrElse nextByte = &H20 OrElse nextByte = &HD OrElse nextByte = &HA)
                nextByte = [is].read()
            End While

            Select Case nextByte
                Case Asc("%")
                    ' header operations, for now return the entire line
                    ' may need to smarter in the future
                    Dim buffer As New StringBuffer()
                    buffer.append(Convert.ToChar(nextByte))
                    readUntilEndOfLine([is], buffer)
                    retval = buffer.ToString()
                Case Asc("(")
                    Dim buffer As New StringBuffer()
                    Dim stringByte As Integer = [is].read()

                    While (stringByte <> -1 AndAlso Convert.ToChar(stringByte) <> ")")
                        buffer.append(Convert.ToChar(stringByte))
                        stringByte = [is].read()
                    End While
                    retval = buffer.ToString()
                Case Asc(">")
                    Dim secondCloseBrace As Integer = [is].read()
                    If (Convert.ToChar(secondCloseBrace) = ">") Then
                        retval = MARK_END_OF_DICTIONARY
                    Else
                        Throw New IOException("Error: expected the end of a dictionary.")
                    End If
                Case Asc("]")
                    retval = MARK_END_OF_ARRAY
                Case Asc("[")
                    Dim list As List(Of Object) = New ArrayList(Of Object)()

                    Dim nextToken As Object = parseNextToken([is])
                    While (nextToken IsNot Nothing AndAlso nextToken <> MARK_END_OF_ARRAY)
                        list.add(nextToken)
                        nextToken = parseNextToken([is])
                    End While
                    retval = list
                Case Asc("<")
                    Dim theNextByte As Integer = [is].read()
                    If (Convert.ToChar(theNextByte) = "<") Then
                        Dim result As Map(Of String, Object) = New HashMap(Of String, Object)()
                        ' we are reading a dictionary
                        Dim key As Object = parseNextToken([is])
                        While (TypeOf (key) Is LiteralName AndAlso key <> MARK_END_OF_DICTIONARY)
                            Dim value As Object = parseNextToken([is])
                            result.put(DirectCast(key, LiteralName).name, value)
                            key = parseNextToken([is])
                        End While
                        retval = result
                    Else
                        ' won't read more than 512 bytes
                        Dim multiplyer As Integer = 16
                        Dim bufferIndex As Integer = -1
                        While (theNextByte <> -1 AndAlso Convert.ToChar(theNextByte) <> ">")
                            Dim intValue As Integer = 0
                            If (Convert.ToChar(theNextByte) >= "0" AndAlso Convert.ToChar(theNextByte) <= "9") Then
                                intValue = theNextByte - Asc("0")
                            ElseIf (theNextByte >= Asc("A") AndAlso theNextByte <= Asc("F")) Then
                                intValue = 10 + theNextByte - Asc("A")
                            ElseIf (theNextByte >= Asc("a") AndAlso theNextByte <= Asc("f")) Then
                                intValue = 10 + theNextByte - Asc("a")
                            ElseIf (theNextByte = &H20) Then
                                ' skipping whitespaces
                                theNextByte = [is].read()
                                Continue While
                            Else
                                Throw New IOException("Error: expected hex character and not " & Convert.ToChar(theNextByte) & ":" & theNextByte)
                            End If
                            intValue *= multiplyer
                            If (multiplyer = 16) Then
                                bufferIndex += 1
                                tokenParserByteBuffer(bufferIndex) = 0
                                multiplyer = 1
                            Else
                                multiplyer = 16
                            End If
                            tokenParserByteBuffer(bufferIndex) += intValue
                            theNextByte = [is].read()
                        End While
                        Dim finalResult() As Byte = Array.CreateInstance(GetType(Byte), bufferIndex + 1)
                        Array.Copy(tokenParserByteBuffer, 0, finalResult, 0, bufferIndex + 1)
                        retval = finalResult
                    End If
                Case Asc("/")
                    Dim buffer As New StringBuffer()
                    Dim stringByte As Integer = [is].read()

                    While (Not isWhitespaceOrEOF(stringByte))
                        buffer.append(Convert.ToChar(stringByte))
                        stringByte = [is].read()
                    End While
                    retval = New LiteralName(buffer.ToString())
                Case -1
                    ' EOF return null;
                Case Asc("0"), Asc("1"), Asc("2"), Asc("3"), Asc("4"), Asc("5"), Asc("6"), Asc("7"), Asc("8"), Asc("9")
                    Dim buffer As New StringBuffer()
                    buffer.append(Convert.ToChar(nextByte))
                    nextByte = [is].read()

                    While (Not isWhitespaceOrEOF(nextByte) AndAlso (NChar.isDigit(Convert.ToChar(nextByte)) OrElse Convert.ToChar(nextByte) = "."))
                        buffer.append(Convert.ToChar(nextByte))
                        nextByte = [is].read()
                    End While
                    [is].unread(nextByte)
                    Dim value As String = buffer.ToString()
                    If (value.IndexOf(".") >= 0) Then
                        retval = New NDouble(value)
                    Else
                        retval = New NInteger(value)
                    End If
                Case Else
                    Dim buffer As New StringBuffer()
                    buffer.append(Convert.ToChar(nextByte))
                    nextByte = [is].read()

                    While (Not isWhitespaceOrEOF(nextByte))
                        buffer.append(Convert.ToChar(nextByte))
                        nextByte = [is].read()
                    End While
                    retval = New [Operator](buffer.ToString())
            End Select
            Return retval
        End Function

        Private Sub readUntilEndOfLine(ByVal [is] As InputStream, ByVal buf As StringBuffer)
            Dim nextByte As Integer = [is].read()
            While (nextByte <> -1 AndAlso nextByte <> &HD AndAlso nextByte <> &HA)
                buf.append(Convert.ToChar(nextByte))
                nextByte = [is].read()
            End While
        End Sub

        Private Function isWhitespaceOrEOF(ByVal aByte As Integer) As Boolean
            Return aByte = -1 OrElse aByte = &H20 OrElse aByte = &HD OrElse aByte = &HA
        End Function

        Private Sub increment(ByVal data() As Byte)
            increment(data, data.Length - 1)
        End Sub

        Private Sub increment(ByVal data() As Byte, ByVal position As Integer)
            If (position > 0 AndAlso (data(position) + 256) Mod 256 = 255) Then
                data(position) = 0
                increment(data, position - 1)
            Else
                data(position) = (data(position) + 1)
            End If
        End Sub

        Private Function createIntFromBytes(ByVal bytes() As Byte) As Integer
            Dim intValue As Integer = (bytes(0) + 256) Mod 256
            If (bytes.Length = 2) Then
                intValue <<= 8
                intValue += (bytes(1) + 256) Mod 256
            End If
            Return intValue
        End Function

        Private Function createStringFromBytes(ByVal bytes() As Byte) As String
            Dim retval As String = vbNullString
            If (bytes.Length = 1) Then
                retval = Strings.GetString(bytes, "ISO-8859-1")
            Else
                retval = Strings.GetString(bytes, "UTF-16BE")
            End If
            Return retval
        End Function

        Private Function compare(ByVal first() As Byte, ByVal second() As Byte) As Integer
            Dim retval As Integer = 1
            Dim firstLength As Integer = first.Length
            For i As Integer = 0 To firstLength - 1
                If (first(i) = second(i)) Then
                    Continue For
                ElseIf (((first(i) + 256) Mod 256) < ((second(i) + 256) Mod 256)) Then
                    retval = -1
                    Exit For
                Else
                    retval = 1
                    Exit For
                End If
            Next
            Return retval
        End Function

        '/**
        ' * Internal class.
        ' */
        Private Class LiteralName
            Public name As String

            Public Sub New(ByVal theName As String)
                name = theName
            End Sub
        End Class

        '/**
        ' * Internal class.
        ' */
        Private Class [Operator]
            Public op As String

            Public Sub New(ByVal theOp As String)
                op = theOp
            End Sub

        End Class

        '    /**
        '     * A simple class to test parsing of cmap files.
        '     * 
        '     * @param args Some command line arguments.
        '     * 
        '     * @throws Exception If there is an error parsing the file.
        '     */
        '    public static void main(String[] args) throws Exception
        '    {
        '        if (args.length != 1)
        '        {
        '            System.err.println("usage: java org.apache.fontbox.cmap.CMapParser <CMAP File>");
        '            System.exit(-1);
        '        }
        '        CMapParser parser = new CMapParser();
        '        File cmapFile = new File(args(0));
        '        CMap result = parser.parse(cmapFile);
        '        System.out.println("Result:" &  result);
        '    }
        '}


    End Class

End Namespace
Imports FinSeA.Io
Imports System.IO
Imports FinSeA.Text

Namespace org.fontbox.cmap


    '/**
    ' * This will parser a CMap stream.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.6 $
    ' */
    Public Class CMapParser

        Private Const BEGIN_CODESPACE_RANGE = "begincodespacerange"
        Private Const BEGIN_BASE_FONT_CHAR = "beginbfchar"
        Private Const BEGIN_BASE_FONT_RANGE = "beginbfrange"

        Private Const MARK_END_OF_DICTIONARY = ">>"
        Private Const MARK_END_OF_ARRAY = "]"

        Private tokenParserByteBuffer(512 - 1) As Byte '= new byte[512];

        '/**
        ' * Creates a new instance of CMapParser.
        ' */
        Public Sub New()
        End Sub

        '/**
        ' * This will parse the stream and create a cmap object.
        ' *
        ' * @param input The CMAP stream to parse.
        ' * @return The parsed stream as a java object.
        ' *
        ' * @throws IOException If there is an error parsing the stream.
        ' */
        Public Function parse(ByVal input As InputStream) As CMap 'throws IOException
            Dim cmapStream As New PushBackInputStream(input)
            Dim result As CMap = New CMap()
            Dim previousToken As Object = Nothing
            Dim token As Object = Nothing
            token = parseNextToken(cmapStream)
            While (token IsNot Nothing)
                If (TypeOf (token) Is [Operator]) Then
                    Dim op As [Operator] = token
                    If (op.op.equals(BEGIN_CODESPACE_RANGE)) Then
                        Dim cosCount As Number = previousToken
                        For j As Integer = 0 To cosCount.intValue() - 1
                            Dim startRange() As Byte = parseNextToken(cmapStream)
                            Dim endRange() As Byte = parseNextToken(cmapStream)
                            Dim range As CodespaceRange = New CodespaceRange()
                            range.setStart(startRange)
                            range.setEnd(endRange)
                            result.addCodespaceRange(range)
                        Next
                    ElseIf (op.op.equals(BEGIN_BASE_FONT_CHAR)) Then
                        Dim cosCount As Number = previousToken
                        For j As Integer = 0 To cosCount.intValue() - 1
                            Dim inputCode() As Byte = parseNextToken(cmapStream)
                            Dim nextToken As Object = parseNextToken(cmapStream)
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
                            Dim startCode() As Byte = parseNextToken(cmapStream)
                            Dim endCode() As Byte = parseNextToken(cmapStream)
                            Dim nextToken As Object = parseNextToken(cmapStream)
                            Dim array As FinSeA.List = Nothing
                            Dim tokenBytes() As Byte = Nothing
                            If (TypeOf (nextToken) Is FinSeA.List) Then
                                array = nextToken
                                tokenBytes = array.get(0)
                            Else
                                tokenBytes = nextToken
                            End If

                            Dim value As String = vbNullString

                            Dim arrayIndex As Integer = 0
                            Dim done As Boolean = False
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
            'skip whitespace
            While (nextByte = &H9 OrElse nextByte = &H20 OrElse nextByte = &HD OrElse nextByte = &HA)
                nextByte = [is].read()
            End While
            Select Case nextByte
                Case AscW("%")
                    'header operations, for now return the entire line 
                    'may need to smarter in the future
                    Dim buffer As New StringBuffer()
                    buffer.append(Convert.ToChar(nextByte))
                    readUntilEndOfLine([is], buffer)
                    retval = buffer.ToString()

                Case AscW("(")
                    Dim buffer As New StringBuffer()
                    Dim stringByte As Integer = [is].read()

                    While (stringByte <> -1 AndAlso Convert.ToChar(stringByte) <> ")")
                        buffer.append(Convert.ToChar(stringByte))
                        stringByte = [is].read()
                    End While
                    retval = buffer.ToString()
                Case AscW(">")
                    Dim secondCloseBrace As Integer = [is].read()
                    If (Convert.ToChar(secondCloseBrace) = ">") Then
                        retval = MARK_END_OF_DICTIONARY
                    Else
                        Throw New IOException("Error: expected the end of a dictionary.")
                    End If
                Case AscW("]")
                    retval = MARK_END_OF_ARRAY
                Case AscW("[")
                    Dim list As FinSeA.List = New FinSeA.ArrayList()

                    Dim nextToken As Object = parseNextToken([is])
                    While (nextToken <> MARK_END_OF_ARRAY)
                        list.add(nextToken)
                        nextToken = parseNextToken([is])
                    End While
                    retval = list
                Case AscW("<")
                    Dim theNextByte As Integer = [is].read()
                    If (theNextByte = "<") Then
                        Dim result As FinSeA.Map = New FinSeA.HashMap()
                        'we are reading a dictionary
                        Dim key As Object = parseNextToken([is])
                        While (TypeOf (key) Is LiteralName AndAlso key <> MARK_END_OF_DICTIONARY)
                            Dim value As Object = parseNextToken([is])
                            result.put(DirectCast(key, LiteralName).name, value)
                            key = parseNextToken([is])
                        End While
                        retval = result
                    Else
                        'won't read more than 512 bytes

                        Dim multiplyer As Integer = 16
                        Dim bufferIndex As Integer = -1
                        While (theNextByte <> -1 AndAlso Convert.ToChar(theNextByte) <> ">")
                            Dim intValue As Integer = 0
                            If (theNextByte >= AscW("0") AndAlso theNextByte <= AscW("9")) Then
                                intValue = theNextByte - AscW("0")
                            ElseIf (theNextByte >= AscW("A") AndAlso theNextByte <= AscW("F")) Then
                                intValue = 10 + theNextByte - AscW("A")
                            ElseIf (theNextByte >= AscW("a") AndAlso theNextByte <= AscW("f")) Then
                                intValue = 10 + theNextByte - AscW("a")
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

                Case AscW("/")
                    Dim buffer As New StringBuffer()
                    Dim stringByte As Integer = [is].read()

                    While (Not isWhitespaceOrEOF(stringByte))
                        buffer.append(Convert.ToChar(stringByte))
                        stringByte = [is].read()
                    End While
                    retval = New LiteralName(buffer.toString())
                Case -1
                    'EOF return null;
                Case AscW("0"), _
                     AscW("1"), _
                     AscW("2"), _
                     AscW("3"), _
                     AscW("4"), _
                     AscW("5"), _
                     AscW("6"), _
                     AscW("7"), _
                     AscW("8"), _
                     AscW("9")
                    Dim buffer As New StringBuffer()
                    buffer.append(Convert.ToChar(nextByte))
                    nextByte = [is].read()

                    While (Not isWhitespaceOrEOF(nextByte) AndAlso (NChar.isDigit(Convert.ToChar(nextByte)) OrElse nextByte = AscW(".")))
                        buffer.append(Convert.ToChar(nextByte))
                        nextByte = [is].read()
                    End While
                    [is].unread(nextByte)
                    Dim value As String = buffer.toString()
                    If (value.IndexOf(".") >= 0) Then
                        retval = Double.Parse(value)
                    Else
                        retval = Integer.Parse(buffer.ToString())
                    End If

                Case Else
                    Dim buffer As New StringBuffer()
                    buffer.append(Convert.ToChar(nextByte))
                    nextByte = [is].read()

                    While (Not isWhitespaceOrEOF(nextByte))
                        buffer.append(Convert.ToChar(nextByte))
                        nextByte = [is].read()
                    End While
                    retval = New [Operator](buffer.toString())

            End Select
            Return retval
        End Function

        Private Sub readUntilEndOfLine(ByVal [is] As InputStream, ByVal buf As StringBuffer)  'throws IOException
            Dim nextByte As Integer = [is].read()
            While (nextByte <> -1 AndAlso nextByte <> &HD AndAlso nextByte <> &HD)
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

        Private Function createStringFromBytes(ByVal bytes() As Byte) As String
            Dim retval As String = vbNullString
            If (bytes.Length = 1) Then
                retval = Strings.GetString(bytes)
            Else
                retval = Strings.GetString(bytes, "UTF-16BE")
            End If
            Return retval
        End Function

        Private Function compare(ByVal first() As Byte, ByVal second() As Byte)
            Dim retval As Integer = 1
            Dim done As Boolean = False
            For i As Integer = 0 To first.Length - 1
                If (done) Then Exit For
                If (first(i) = second(i)) Then
                    'move to next position
                ElseIf (((first(i) + 256) Mod 256) < ((second(i) + 256) Mod 256)) Then
                    done = True
                    retval = -1
                Else
                    done = True
                    retval = 1
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


        '    '/**
        '    ' * A simple class to test parsing of cmap files.
        '    ' * 
        '    ' * @param args Some command line arguments.
        '    ' * 
        '    ' * @throws Exception If there is an error parsing the file.
        '    ' */
        'public static void main( String[] args ) throws Exception
        '{
        '    if( args.length != 1 )
        '    {
        '        System.err.println( "usage: java org.pdfbox.cmapparser.CMapParser <CMAP File>" );
        '        System.exit( -1 );
        '    }
        '    CMapParser parser = new CMapParser(  );
        '    CMap result = parser.parse( new FileInputStream( args[0] ) );
        '    System.out.println( "Result:" + result );
        '}
    End Class

End Namespace
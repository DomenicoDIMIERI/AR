Imports System.IO
Imports System.Text
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.io
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.util


Namespace org.apache.pdfbox.pdfparser

    '/**
    ' * This will parse a PDF byte stream and extract operands and such.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.32 $
    ' */
    Public Class PDFStreamParser
        Inherits BaseParser
        Private streamObjects As List = New ArrayList(100)
        Private file As RandomAccess
        Private lastBIToken As PDFOperator = Nothing

        Shared ReadOnly _EChar As Integer = AscW("E"c)
        Shared ReadOnly _IChar As Integer = AscW("I"c)

        '/**
        ' * Constructor that takes a stream to parse.
        ' *
        ' * @since Apache PDFBox 1.3.0
        ' * @param stream The stream to read data from.
        ' * @param raf The random access file.
        ' * @param forceParsing flag to skip malformed or otherwise unparseable
        ' *                     input where possible
        ' * @throws IOException If there is an error reading from the stream.
        ' */
        Public Sub New(ByVal stream As Stream, ByVal raf As RandomAccess, ByVal forceParsing As Boolean) '            throws IOException
            MyBase.New(stream, forceParsing)
            file = raf
        End Sub

        '/**
        ' * Constructor that takes a stream to parse.
        ' *
        ' * @param stream The stream to read data from.
        ' * @param raf The random access file.
        ' *
        ' * @throws IOException If there is an error reading from the stream.
        ' */
        Public Sub New(ByVal stream As Stream, ByVal raf As RandomAccess) 'throws IOException
            Me.New(stream, raf, FORCE_PARSING)
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param stream The stream to parse.
        ' *
        ' * @throws IOException If there is an error initializing the stream.
        ' */
        Public Sub New(ByVal stream As PDStream) ' throws IOException
            Me.New(stream.createInputStream(), stream.getStream().getScratchFile())
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @since Apache PDFBox 1.3.0
        ' * @param stream The stream to parse.
        ' * @param forceParsing flag to skip malformed or otherwise unparseable
        ' *                     input where possible
        ' * @throws IOException If there is an error initializing the stream.
        ' */
        Public Sub New(ByVal stream As COSStream, ByVal forceParsing As Boolean) 'throws IOException
            Me.New(stream.getUnfilteredStream(), stream.getScratchFile(), forceParsing)
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param stream The stream to parse.
        ' *
        ' * @throws IOException If there is an error initializing the stream.
        ' */
        Public Sub New(ByVal stream As COSStream)  'throws IOException
            Me.New(stream.getUnfilteredStream(), stream.getScratchFile())
        End Sub

        '/**
        ' * This will parse the tokens in the stream.  This will close the
        ' * stream when it is finished parsing.
        ' *
        ' * @throws IOException If there is an error while parsing the stream.
        ' */
        Public Sub parse() 'throws IOException
            Try
                Dim token As Object = Nothing
                token = parseNextToken()
                While (token IsNot Nothing)
                    streamObjects.add(token)
                    'logger().fine( "parsed=" + token );
                    token = parseNextToken()
                End While
            Finally
                pdfSource.Close()
            End Try
        End Sub

        '/**
        ' * This will get the tokens that were parsed from the stream.
        ' *
        ' * @return All of the tokens in the stream.
        ' */
        Public Function getTokens() As List
            Return streamObjects
        End Function

        '/**
        ' * {@inheritDoc}
        ' */
        Public Sub close() 'throws IOException
            pdfSource.Close()
        End Sub

        '/**
        ' * This will get an iterator which can be used to parse the stream
        ' * one token after the other.
        ' *
        ' * @return an iterator to get one token after the other
        ' */
        Public Function getTokenIterator() As Global.System.Collections.Generic.IEnumerator(Of Object)
            Return New ObjectEnumerator(Me)
        End Function

#Region "Obect Enumerator"

        Private Class ObjectEnumerator
            Implements Global.System.Collections.Generic.IEnumerator(Of Object)
            Private m_Owner As PDFStreamParser
            Private token As Object

            Public Sub New(ByVal owner As PDFStreamParser)
                Me.m_Owner = owner
                Me.token = Nothing
            End Sub

            '    private void tryNext()
            '    {
            '            Try
            '        {
            '            if (token Is Nothing)
            '            {
            '                token = parseNextToken();
            '            }
            '        }
            '        catch (IOException e)
            '        {
            '            throw new RuntimeException(e);
            '        }
            '    }

            '    /** {@inheritDoc} */
            '    public boolean hasNext()
            '    {
            '        tryNext();
            '        return token IsNot Nothing;
            '    }

            '    /** {@inheritDoc} */
            '    public Object next() 
            '    {
            '        tryNext();
            '        Object tmp = token;
            '        if (tmp Is Nothing)
            '        {
            '            throw new NoSuchElementException();
            '        }
            '        token = null;
            '        return tmp;
            '    }

            '    /** {@inheritDoc} */
            '    public void remove()
            '    {
            '        throw new UnsupportedOperationException();
            '    }
            '};

            Public ReadOnly Property Current As Object Implements Global.System.Collections.Generic.IEnumerator(Of Object).Current
                Get
                    Return Me.token
                End Get
            End Property

            Private ReadOnly Property Current1 As Object Implements Global.System.Collections.IEnumerator.Current
                Get
                    Return Me.Current
                End Get
            End Property

            Public Function MoveNext() As Boolean Implements Global.System.Collections.IEnumerator.MoveNext
                Me.token = Me.m_Owner.parseNextToken
                Return (Me.token IsNot Nothing)
            End Function

            Public Sub Reset() Implements Global.System.Collections.IEnumerator.Reset
                Me.token = Nothing
            End Sub

#Region "IDisposable Support"
            Private disposedValue As Boolean ' To detect redundant calls

            ' IDisposable
            Protected Overridable Sub Dispose(disposing As Boolean)
                If Not Me.disposedValue Then
                    If disposing Then
                        ' TODO: dispose managed state (managed objects).
                    End If

                    ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                    ' TODO: set large fields to null.
                End If
                Me.disposedValue = True
            End Sub

            ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
            'Protected Overrides Sub Finalize()
            '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            '    Dispose(False)
            '    MyBase.Finalize()
            'End Sub

            ' This code added by Visual Basic to correctly implement the disposable pattern.
            Public Sub Dispose() Implements IDisposable.Dispose
                ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
                Dispose(True)
                GC.SuppressFinalize(Me)
            End Sub
#End Region

        End Class

#End Region


        '/**
        ' * This will parse the next token in the stream.
        ' *
        ' * @return The next token in the stream or null if there are no more tokens in the stream.
        ' *
        ' * @throws IOException If an io error occurs while parsing the stream.
        ' */
        Private Function parseNextToken() As Object ' throws IOException
            Dim retval As Object = Nothing

            skipSpaces()
            Dim nextByte As Integer = pdfSource.peek()
            If (nextByte = -1) Then
                Return Nothing
            End If
            Dim c As Char = Convert.ToChar(nextByte)
            Select Case (c)
                Case "<"c
                    Dim leftBracket As Integer = pdfSource.read() '/pull off first left bracket
                    c = Convert.ToChar(pdfSource.peek()) 'check for second left bracket
                    pdfSource.unread(leftBracket) 'put back first bracket
                    If (c = "<"c) Then
                        Dim pod As COSDictionary = parseCOSDictionary()
                        skipSpaces()
                        If (Convert.ToChar(pdfSource.peek()) = "s") Then
                            retval = parseCOSStream(pod, file)
                        Else
                            retval = pod
                        End If
                    Else
                        retval = parseCOSString(False)
                    End If
                Case "["c ' array
                    retval = parseCOSArray()
                Case "("c ' string
                    retval = parseCOSString(False)
                Case "/"c ' name
                    retval = parseCOSName()
                Case "n"c ' null
                    Dim nullString As String = readString()
                    If (nullString.Equals("null")) Then
                        retval = COSNull.NULL
                    Else
                        retval = PDFOperator.getOperator(nullString)
                    End If
                Case "t"c, "f"c
                    Dim [next] As String = readString()
                    If ([next].Equals("true")) Then
                        retval = COSBoolean.TRUE
                        'break;
                    ElseIf ([next].Equals("false")) Then
                        retval = COSBoolean.FALSE
                    Else
                        retval = PDFOperator.getOperator([next])
                    End If
                Case "R"c
                    Dim line As String = readString()
                    If (line.Equals("R")) Then
                        retval = New COSObject(Nothing)
                    Else
                        retval = PDFOperator.getOperator(line)
                    End If
                Case "0"c, "1"c, "2"c, "3"c, "4"c, "5"c, "6"c, "7"c, "8"c, "9"c, "-"c, "+"c, "."c
                    ' We will be filling buf with the rest of the number.  Only
                    ' allow 1 "." and "-" and "+" at start of number. */
                    Dim buf As New System.Text.StringBuilder()
                    buf.Append(c)
                    pdfSource.read()

                    Dim dotNotRead As Boolean = (c <> "."c)
                    c = Convert.ToChar(pdfSource.peek())
                    While (Sistema.Strings.IsDigit(c) OrElse (dotNotRead AndAlso (c = "."c)))
                        buf.Append(c)
                        pdfSource.read()

                        If (dotNotRead AndAlso (c = "."c)) Then
                            dotNotRead = False
                        End If
                        c = Convert.ToChar(pdfSource.peek())
                    End While
                    retval = COSNumber.get(buf.ToString())
                Case "B"c
                    Dim [next] As String = readString()
                    retval = PDFOperator.getOperator([next])

                    If ([next].Equals("BI")) Then
                        lastBIToken = retval
                        Dim imageParams As New COSDictionary()
                        lastBIToken.setImageParameters(New ImageParameters(imageParams))
                        Dim nextToken As Object = Nothing
                        nextToken = parseNextToken()
                        While (TypeOf (nextToken) Is COSName)
                            Dim value As Object = parseNextToken()
                            imageParams.setItem(nextToken, value)
                            nextToken = parseNextToken()
                        End While
                        'final token will be the image data, maybe??
                        Dim imageData As PDFOperator = nextToken
                        lastBIToken.setImageData(imageData.getImageData())
                    End If
                Case "I"c
                    'ImageParameters imageParams = lastBIToken.getImageParameters();

                    'int expectedBytes = (int)Math.ceil(imageParams.getHeight() * imageParams.getWidth() *
                    '                    (imageParams.getBitsPerComponent()/8) );
                    'Special case for ID operator
                    Dim id As String = "" & Convert.ToChar(pdfSource.read()) & Convert.ToChar(pdfSource.read())
                    If (Not id.Equals("ID")) Then
                        Throw New IOException("Error: Expected operator 'ID' actual='" & id & "'")
                    End If
                    Dim imageData As New MemoryStream ' ByteArrayOutputStream()
                    'boolean foundEnd = false
                    If (Me.isWhitespace()) Then
                        'pull off the whitespace character
                        pdfSource.read()
                    End If
                    Dim twoBytesAgo As Integer = 0
                    Dim lastByte As Integer = pdfSource.read()
                    Dim currentByte As Integer = pdfSource.read()
                    Dim count As Integer = 0
                    'PDF spec is kinda unclear about Me.  Should a whitespace
                    'always appear before EI? Not sure, I found a PDF
                    '(UnderstandingWebSphereClassLoaders.pdf) which has EI as part
                    'of the image data and will stop parsing prematurely if there is
                    'not a check for <whitespace>EI<whitespace>.
                    While (Not (isWhitespace(twoBytesAgo) AndAlso _
                             lastByte = _EChar AndAlso _
                             currentByte = _IChar AndAlso _
                             isWhitespace()) AndAlso Not pdfSource.isEOF())
                        imageData.WriteByte(lastByte)
                        twoBytesAgo = lastByte
                        lastByte = currentByte
                        currentByte = pdfSource.read()
                        count += 1
                    End While
                    pdfSource.unread(_IChar) 'unread the EI operator
                    pdfSource.unread(_EChar)
                    retval = PDFOperator.getOperator("ID")
                    DirectCast(retval, PDFOperator).setImageData(imageData.ToArray())
                Case "]"c
                    ' some ']' around without its previous '['
                    ' this means a PDF is somewhat corrupt but we will continue to parse.
                    pdfSource.read()
                    retval = COSNull.NULL ' must be a better solution than null...
                Case Else
                    'we must be an operator
                    Dim [operator] As String = readOperator()
                    If ([operator].Trim().Length() = 0) Then
                        'we have a corrupt stream, stop reading here
                        retval = Nothing
                    Else
                        retval = PDFOperator.getOperator([operator])
                    End If
            End Select

            Return retval
        End Function

        '/**
        ' * This will read an operator from the stream.
        ' *
        ' * @return The operator that was read from the stream.
        ' *
        ' * @throws IOException If there is an error reading from the stream.
        ' */
        Protected Function readOperator() As String ' throws IOException
            skipSpaces()

            'average string size is around 2 and the normal string buffer size is
            'about 16 so lets save some space.
            Dim buffer As New StringBuilder(4) 'StringBuffer
            Dim nChar As Integer = pdfSource.peek()
            Dim nextChar As Char = Convert.ToChar(nChar)
            While (nChar <> -1 AndAlso Not isWhitespace(nChar) AndAlso Not isClosing(nChar) AndAlso _
                    nextChar <> "["c AndAlso _
                    nextChar <> "<"c AndAlso _
                    nextChar <> "("c AndAlso _
                    nextChar <> "/"c AndAlso _
                    (nextChar < "0"c OrElse
                    nextChar > "9"c)
                )
                Dim currentChar As Char = Convert.ToChar(pdfSource.read())
                nChar = pdfSource.peek()
                nextChar = Convert.ToChar(nChar)
                buffer.Append(currentChar)
                ' Type3 Glyph description has operators with a number in the name
                If (currentChar = "d"c AndAlso (nextChar = "0"c OrElse nextChar = "1"c)) Then
                    buffer.Append(Convert.ToChar(pdfSource.read()))
                    nChar = pdfSource.peek()
                    nextChar = Convert.ToChar(nChar)
                End If
            End While
            Return buffer.ToString()
        End Function

    End Class

End Namespace
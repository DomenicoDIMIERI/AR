Imports FinSeA.org.apache.pdfbox.encoding
Imports FinSeA.org.apache.pdfbox.exceptions
Imports FinSeA.org.apache.pdfbox.persistence.util
Imports System.IO
Imports System.Text
Imports FinSeA.Io
Imports FinSeA.Text

Namespace org.apache.pdfbox.cos

    '/**
    ' * This represents a string object in a PDF document.
    ' * 
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.30 $
    ' */
    Public Class COSString
        Inherits COSBase

        '/**
        ' * One of the open string tokens.
        ' */
        Public Shared ReadOnly STRING_OPEN As Byte() = {40} ' "(".getBytes();
        '/**
        ' * One of the close string tokens.
        ' */
        Public Shared ReadOnly STRING_CLOSE() As Byte = {41} ' ")".getBytes( "ISO-8859-1" );
        '/**
        ' * One of the open string tokens.
        ' */
        Public Shared ReadOnly HEX_STRING_OPEN As Byte() = {60} ' "<".getBytes( "ISO-8859-1" );
        '/**
        ' * One of the close string tokens.
        ' */
        Public Shared ReadOnly HEX_STRING_CLOSE As Byte() = {62} '">".getBytes( "ISO-8859-1" );
        '/**
        ' * the escape character in strings.
        ' */
        Public Shared ReadOnly ESCAPE As Byte() = {92} ' "\\".getBytes( "ISO-8859-1" );

        '/**
        ' * CR escape characters.
        ' */
        Public Shared ReadOnly CR_ESCAPE As Byte() = {92, 114} ' "\\r".getBytes( "ISO-8859-1" );
        '/**
        ' * LF escape characters.
        ' */
        Public Shared ReadOnly LF_ESCAPE() As Byte = {92, 110} ' // "\\n".getBytes( "ISO-8859-1" );
        '/**
        ' * HT escape characters.
        ' */
        Public Shared ReadOnly HT_ESCAPE() As Byte = {92, 116} ' "\\t".getBytes( "ISO-8859-1" );
        '/**
        ' * BS escape characters.
        ' */
        Public Shared ReadOnly BS_ESCAPE As Byte() = {92, 98} ' "\\b".getBytes( "ISO-8859-1" );
        '/**
        ' * FF escape characters.
        ' */
        Public Shared ReadOnly FF_ESCAPE As Byte() = {92, 102} ' "\\f".getBytes( "ISO-8859-1" );

        Private out As ByteArrayOutputStream = Nothing '= Nothing
        Private str As String = ""

        '/**
        ' * Forces the string to be serialized in hex form but not literal form, the default is to stream in literal form.
        ' */
        Private forceHexForm As Boolean = False

        Private isDictionary As Boolean = False

        '/**
        ' * Constructor.
        ' */
        Public Sub New()
            Me.out = New ByteArrayOutputStream()
        End Sub

        '/** 
        ' * Constructor.
        ' * 
        ' * @param isDictionaryValue determines if this string represents a dictionary
        ' */
        Public Sub New(ByVal isDictionaryValue As Boolean)
            Me.New()
            Me.isDictionary = isDictionaryValue
        End Sub

        '/**
        ' * Explicit constructor for ease of manual PDF construction.
        ' * 
        ' * @param value
        ' *            The string value of the object.
        ' */
        Public Sub New(ByVal value As String)
            Try
                Dim unicode16 As Boolean = False
                Dim chars() As Char = value.ToCharArray
                Dim length As Integer = chars.Length
                For i As Integer = 0 To length - 1
                    If (Convert.ToInt32(chars(i)) > 255) Then
                        unicode16 = True
                        Exit For
                    End If
                Next
                If (unicode16) Then
                    Dim data() As Byte = System.Text.Encoding.BigEndianUnicode.GetBytes(value) ' value.getBytes("UTF-16BE")
                    Me.out = New ByteArrayOutputStream(data.Length + 2)
                    out.Write({&HFE}, 0, 1)
                    out.Write({&HFF}, 0, 1)
                    out.Write(data, 0, 1 + UBound(data))
                Else
                    Dim data() As Byte = System.Text.Encoding.UTF7.GetBytes(value) 'value.getBytes("ISO-8859-1");
                    out = New ByteArrayOutputStream(data.Length)
                    out.Write(data, 0, 1 + UBound(data))
                End If
            Catch ignore As Exception
                'ignore.printStackTrace()
                ' should never happen
                Debug.Print(ignore.ToString)
            End Try
        End Sub

        '/**
        ' * Explicit constructor for ease of manual PDF construction.
        ' * 
        ' * @param value
        ' *            The string value of the object.
        ' */
        Public Sub New(ByVal value() As Byte)
            Try
                out = New ByteArrayOutputStream(value.Length)
                out.write(value)
            Catch ignore As Exception
                Debug.Print(ignore.ToString)
                ' should never happen
            End Try
        End Sub

        '/**
        ' * Forces the string to be written in literal form instead of hexadecimal form.
        ' * 
        ' * @param v
        ' *            if v is true the string will be written in literal form, otherwise it will be written in hexa if
        ' *            necessary.
        ' */

        Public Sub setForceLiteralForm(ByVal v As Boolean)
            forceHexForm = Not v
        End Sub

        '/**
        ' * Forces the string to be written in hexadecimal form instead of literal form.
        ' * 
        ' * @param v
        ' *            if v is true the string will be written in hexadecimal form otherwise it will be written in literal if
        ' *            necessary.
        ' */
        Public Sub setForceHexForm(ByVal v As Boolean)
            Me.forceHexForm = v
        End Sub

        '/**
        ' * This will create a COS string from a string of hex characters.
        ' * 
        ' * @param hex
        ' *            A hex string.
        ' * @return A cos string with the hex characters converted to their actual bytes.
        ' * @throws IOException
        ' *             If there is an error with the hex string.
        ' */
        Public Shared Function createFromHexString(ByVal hex As String) As COSString 'throws IOException
            Return createFromHexString(Hex, False)
        End Function

        '/**
        '* Creates a COS string from a string of hex characters, optionally ignoring malformed input.
        '* 
        '* @param hex
        '*            A hex string.
        '* @param force
        '*            flag to ignore malformed input
        '* @return A cos string with the hex characters converted to their actual bytes.
        '* @throws IOException
        '*             If there is an error with the hex string.
        '*/
        Public Shared Function createFromHexString(ByVal hex As String, ByVal force As Boolean) As COSString ' throws IOException
            Dim retval As New COSString()
            Dim hexBuffer As New StringBuffer(hex.Trim())
            ' if odd number then the last hex digit is assumed to be 0
            If (hexBuffer.length() Mod 2 <> 0) Then
                hexBuffer.append("0")
            End If
            Dim length As Integer = hexBuffer.length()
            For i As Integer = 0 To length - 1 Step 2
                Try
                    retval.append(Integer.Parse(hexBuffer.substring(i, i + 2), 16))
                Catch e As Exception
                    If (force) Then
                        retval.append("?")
                    Else
                        Dim exception As New FormatException("Invalid hex string: " & hex, e)
                        Throw exception
                    End If
                End Try
            Next
            Return retval
        End Function

        '/**
        ' * This will take this string and create a hex representation of the bytes that make the string.
        ' * 
        ' * @return A hex string representing the bytes in this string.
        ' */
        Public Function getHexString() As String
            Dim retval As New StringBuilder(out.size() * 2)
            Dim data() As Byte = getBytes()
            Dim length As Integer = data.Length
            For i As Integer = 0 To length - 1
                retval.append(COSHEXTable.HEX_TABLE((data(i) + 256) Mod 256))
            Next
            Return retval.toString()
        End Function

        '/**
        ' * This will get the string that this object wraps.
        ' * 
        ' * @return The wrapped string.
        ' */
        Public Function getString() As String
            If (Me.str <> "") Then
                Return Me.str
            End If
            Dim retval As String
            Dim encoding As String = "ISO-8859-1"
            Dim data() As Byte = getBytes()
            Dim start As Integer = 0
            If (data.Length > 2) Then
                If (data(0) = &HFF AndAlso data(1) = &HFE) Then
                    encoding = "UTF-16LE"
                    start = 2
                ElseIf (data(0) = &HFE AndAlso data(1) = &HFF) Then
                    encoding = "UTF-16BE"
                    start = 2
                End If
            End If
            Try
                If (isDictionary AndAlso encoding.Equals("ISO-8859-1")) Then
                    Dim tmp() As Byte = getBytes()
                    Dim pde As New PdfDocEncoding()
                    Dim sb As New StringBuilder(tmp.Length)
                    For Each b As Byte In tmp
                        Dim character As String = pde.getCharacter((b + 256) Mod 256)
                        If (character <> "") Then
                            sb.append(character)
                        End If
                    Next
                    retval = sb.toString()
                Else
                    retval = Sistema.Strings.GetString(Me.getChars(), start, data.Length - start, encoding)
                End If
            Catch e As Exception
                ' should never happen
                Debug.Print(e.ToString)
                retval = New String(Me.getChars())
            End Try
            Me.str = retval
            Return retval
        End Function

        '/**
        ' * This will append a byte[] to the string.
        ' * 
        ' * @param data
        ' *            The byte[] to add to this string.
        ' * 
        ' * @throws IOException
        ' *             If an IO error occurs while writing the byte.
        ' */
        Public Sub append(ByVal data() As Byte) 'throws IOException
            out.write(data)
            Me.str = ""
        End Sub

        '/**
        ' * This will append a byte to the string.
        ' * 
        ' * @param in
        ' *            The byte to add to this string.
        ' * 
        ' * @throws IOException
        ' *             If an IO error occurs while writing the byte.
        ' */
        Public Sub append(ByVal [in] As Integer) ' throws IOException
            Dim writer As New StreamWriter(Me.out)
            writer.Write([in])
            Me.str = ""
        End Sub

        '/**
        ' * This will reset the internal buffer.
        ' */
        Public Sub reset()
            out.Close() '.reset
            Me.str = ""
        End Sub

        '/**
        ' * This will get the bytes of the string.
        ' * 
        ' * @return A byte array that represents the string.
        ' */
        Public Function getBytes() As Byte()
            Dim buffer() As Byte
            ReDim buffer(Me.out.Length - 1)
            Me.out.Position = 0
            Me.out.Read(buffer, 0, Me.out.Length)
            Return buffer 'out.toByteArray( )
        End Function

        Public Function getChars() As Char()
            Dim buffer() As Char
            ReDim buffer(Me.out.Length - 1)
            Me.out.Position = 0
            Dim reader As New StreamReader(Me.out)
            reader.ReadBlock(buffer, 0, Me.out.Length)
            Return buffer 'out.toByteArray( )
        End Function

        '/**
        ' * {@inheritDoc}
        ' */
        '@Override
        Public Overrides Function toString() As String
            Return "COSString{" & Me.getString() & "}"
        End Function

        '/**
        ' * This will output this string as a PDF object.
        ' * 
        ' * @param output
        ' *            The stream to write to.
        ' * @throws IOException
        ' *             If there is an error writing to the stream.
        ' */
        Public Sub writePDF(ByVal output As OutputStream) ' throws IOException
            Dim outsideASCII As Boolean = False
            ' Lets first check if we need to escape this string.
            Dim bytes() As Byte = getBytes()
            Dim length As Integer = bytes.Length
            For i As Integer = 0 To length - 1
                If (outsideASCII) Then Exit For
                ' if the byte is negative then it is an eight bit byte and is
                ' outside the ASCII range.
                outsideASCII = bytes(i) < 0
                If (Not outsideASCII AndAlso Not forceHexForm) Then
                    output.Write(STRING_OPEN, 0, 1 + UBound(STRING_OPEN))
                    For j As Integer = 0 To length - 1
                        Dim b As Integer = (bytes(j) + 256) Mod 256
                        Select Case (b)
                            Case Asc("("), Asc(")"), Asc("\\")
                                output.Write(ESCAPE, 0, 1 + UBound(ESCAPE))
                                output.Write({CByte(b)}, 0, 1)
                            Case vbLf ' LF
                                output.Write(LF_ESCAPE, 0, 1 + UBound(LF_ESCAPE))
                            Case vbCr ' CR
                                output.Write(CR_ESCAPE, 0, 1 + UBound(CR_ESCAPE))
                            Case vbTab
                                output.Write(HT_ESCAPE, 0, 1 + UBound(HT_ESCAPE))
                            Case vbBack ''\b':
                                output.Write(BS_ESCAPE, 0, 1 + UBound(BS_ESCAPE))
                            Case vbFormFeed ' '\f':
                                output.Write(FF_ESCAPE, 0, 1 + UBound(FF_ESCAPE))
                            Case Else
                                output.Write({CByte(b)}, 0, 1)
                        End Select
                    Next
                End If
                output.Write(STRING_CLOSE, 0, 1 + UBound(STRING_CLOSE))



                output.Write(HEX_STRING_OPEN, 0, 1 + UBound(HEX_STRING_OPEN))
                For j As Integer = 0 To length - 1
                    output.Write(COSHEXTable.TABLE((bytes(j) + 256) Mod 256))
                Next
                output.Write(HEX_STRING_CLOSE, 0, 1 + UBound(HEX_STRING_CLOSE))
            Next
        End Sub

        '/**
        ' * visitor pattern double dispatch method.
        ' * 
        ' * @param visitor
        ' *            The object to notify when visiting this object.
        ' * @return any object, depending on the visitor implementation, or null
        ' * @throws COSVisitorException
        ' *             If an error occurs while visiting this object.
        ' */
        Public Overrides Function accept(ByVal visitor As ICOSVisitor) As Object 'throws COSVisitorException
            Return visitor.visitFromString(Me)
        End Function

        '/**
        ' * {@inheritDoc}
        ' */
        '@Override
        Public Overrides Function equals(ByVal obj As Object) As Boolean
            If (TypeOf (obj) Is COSString) Then
                Dim strObj As COSString = obj
                Return Me.getString().Equals(strObj.getString()) AndAlso Me.forceHexForm = Me.forceHexForm
            End If
            Return False
        End Function

        '/**
        ' * {@inheritDoc}
        ' */
        '@Override
        Public Overrides Function GetHashCode() As Integer
            Dim result As Integer = Me.getString().GetHashCode()
            result += IIf(forceHexForm, 17, 0)
            Return result
        End Function

    End Class

End Namespace
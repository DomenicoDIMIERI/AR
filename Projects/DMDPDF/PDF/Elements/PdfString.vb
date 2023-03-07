Imports System
Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Collections
Imports System.Diagnostics
Imports System.Globalization
Imports System.Security.Permissions
Imports System.Runtime.InteropServices
Imports System.Reflection
Imports System.Runtime.CompilerServices

Namespace PDF.Elements

    <Serializable> _
    Public Class PdfString
        Inherits PDFObject

        Private s As String = ""
        Private Shared ReadOnly hexRegex As New Regex("^([^>]+)>", RegexOptions.Singleline)
        Private Shared ReadOnly octRegex As New Regex("^([0-7]{3})", RegexOptions.Singleline)
        Private Shared ReadOnly wsRegex As New Regex("\s+", RegexOptions.Singleline)
        Private Shared ReadOnly encoding As New UnicodeEncoding(True, True)

        ''' <summary>
        ''' Initializes a new instance of PdfString.
        ''' </summary>
        ''' <param name="hex">Is this string in hexadecimal form?</param>
        ''' <param name="input">The string to be parsed into a PdfString object. Must not include the leading '('.
        ''' Must include the trailing ')'. Consumes the PDF String object from the input.
        ''' </param>
        Public Sub New(ByVal hex As Boolean, ByRef input As String)
            If (hex) Then
                Dim hexMatch As Match = hexRegex.Match(input)

                If (Not hexMatch.Success) Then Throw New Exception("Cannot parse hexadecimal string at '" & input & "'")

                Dim hexString As String = hexMatch.Groups(1).Value
                hexString = wsRegex.Replace(hexString, "")
                input = input.Substring(hexMatch.Index + hexMatch.Length)
                If ((hexString.Length And 1) = 1) Then ' odd number of characters, append 0 according to PDF spec
                    hexString &= "0"
                End If

                Dim hexBytes() As Byte
                ReDim hexBytes(hexString.Length / 2 - 1)
                For i As Integer = 0 To hexString.Length - 1 Step 2
                    Dim b As Byte = Byte.Parse(hexString.Substring(i, 2), System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture)
                    hexBytes(i / 2) = b
                Next

                If (hexBytes.Length >= 2 AndAlso hexBytes(0) = &HFE AndAlso hexBytes(1) = &HFF) Then ' utf16 encoding
                    s = encoding.GetString(hexBytes, 2, hexBytes.Length - 2)
				Else
                    Dim chars() As Char
                    ReDim chars(hexBytes.Length - 1)

                    For i As Integer = 0 To hexBytes.Length - 1
                        chars(i) = Convert.ToChar(hexBytes(i))
                    Next

                    s = New String(chars)
                End If
            Else
                Dim nestLevel As Integer = 1
                Dim unicode As Boolean = input.Length >= 2 AndAlso ((input(0) = Convert.ToChar(&HFE) AndAlso input(1) = Convert.ToChar(&HFF)) OrElse input.StartsWith("\376\377"))

                If (unicode) Then ' strip byte-order marker
                    input = input.Substring(IIf(input(0) = "\", 8, 2))
                End If

                While (nestLevel > 0 AndAlso input.Length > 0)
                    If (input(0) = ")" AndAlso nestLevel = 1) Then
                        nestLevel = 0
                        input = input.Substring(1)
                        Exit While
                    End If

                    Dim currentChars As String = GetNextChar(input, unicode)
                    If (currentChars.Equals("")) Then
                        Continue While
                    End If
                    Dim currentChar As Char = currentChars(0)

                    If (currentChar = "(") Then
                        nestLevel += 1
                    ElseIf (currentChar = ")") Then
                        nestLevel -= 1
                    End If

                    If (currentChar = "\" AndAlso currentChars.Length = 2) Then ' can only be "\(" or "\)"
                        currentChar = currentChars(1)
                    End If

                    s += currentChar
                End While
            End If
        End Sub

        ''' <summary>
        ''' Initializes a new PdfString object.
        ''' </summary>
        ''' <param name="s">A string from which to initialize the PdfString object.</param>
        Public Sub New(ByVal s As String)
            Me.s = s
        End Sub

        Private Function GetNextInputChar(ByRef input As String) As String
            Dim len As Integer = 1
            Dim currentChar As Char = input(0)
            Dim nextCharacter As Char = input(1)
            Dim returnChar As String = ""

            If (input.Length >= 2 AndAlso currentChar = "\") Then
                len += 1 ' strip the \

                Select (nextCharacter)
                    Case ")", "(", "\" : returnChar &= "\" & nextCharacter
                    Case "n" : returnChar &= vbLf
                    Case "r" : returnChar &= vbCr
                    Case "t" : returnChar &= vbTab
                    Case "b" : returnChar &= vbBack
                    Case "f" : returnChar &= vbFormFeed
                End Select

                Dim octMatch As Match = octRegex.Match(input.Substring(1))
                If (octMatch.Success) Then
                    len += 2 ' strip two more characters from input
                    Dim num As Integer = Convert.ToInt32(octMatch.Groups(1).Value, 8)
                    returnChar &= ChrW(num)
                End If
            ElseIf (input.Length >= 2 AndAlso (currentChar = vbLf OrElse currentChar = vbCr)) Then
                returnChar &= vbCr
                If (nextCharacter = vbLf OrElse nextCharacter = vbCr) Then 'two-byte line ending
                    len += 1
                End If
            Else
                returnChar &= currentChar
            End If

            input = input.Substring(len)

            Return returnChar
        End Function

        Private Function GetNextChar(ByRef input As String, ByVal unicode As Boolean) As String
            Dim nextCharacter As String = GetNextInputChar(input)
            If (nextCharacter.Equals("")) Then Return ""
			
            If (unicode) Then
			    Try
                    Dim c2s As String
                    Do
                        c2s = GetNextInputChar(input)
                    Loop While (c2s.Equals(""))
                    Dim c2 As Char = c2s(0)
                    nextCharacter = "" & encoding.GetChars(New Byte() {Convert.ToByte(nextCharacter(0)), Convert.ToByte(c2)}, 0, 2)(0)
                Catch ex As Exception
                    Throw New Exception("error parsing unicode string", ex)
                End Try
            End If

            Return nextCharacter
        End Function

        ''' <summary>
        ''' Returns the PdfString as a string.
        ''' </summary>
        Public Property Text As String
            Get
                Return Me.s
            End Get
            Set(value As String)
                Me.s = value
            End Set
        End Property

        ''' <summary>
        ''' Returns the encoded representation of the PdfString object. Automatically chooses Unicode or PdfDocEncoding.
        ''' Returned string is always in literal format.
        ''' </summary>
        ''' <returns>The encoded string.</returns>
        Public Overrides Function ToString() As String
            Dim output As String = s.Replace("(", "\(")
            output = output.Replace(")", "\)")

            Dim unicode As Boolean = False
            For Each c As Char In output.ToCharArray()
                If (Convert.ToUInt16(c) > 255) Then
                    unicode = True
                    Exit For
                End If
            Next

            If (unicode) Then
                Dim str As String = "(" & Convert.ToChar(&HFE) & Convert.ToChar(&HFF)
                Dim bytes() As Byte = encoding.GetBytes(output)
                For Each b As Byte In bytes
                    str &= Convert.ToChar(b)
                Next
                str &= ")"
                Return str
			Else
                Return "(" + output + ")"
            End If
        End Function

    End Class


End Namespace
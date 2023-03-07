Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Text.RegularExpressions

Namespace Net.Mail

    Public Enum HeaderCharSetEncoding As Integer
        Base64 = 0
        QuotedPrintable = 1
    End Enum

    Public NotInheritable Class MailHeaders

        Public Const Bcc As String = "bcc"
        Public Const Cc As String = "cc"
        Public Const [Date] As String = "date"
        Public Const From As String = "from"
        Public Const Importance As String = "importance"
        Public Const InReplyTo As String = "in-reply-to"
        Public Const MessageId As String = "message-id"
        Public Const Received As String = "received"
        Public Const ReplyTo As String = "reply-to"
        Public Const Subject As String = "subject"
        Public Const [To] As String = "to"

        ''' <summary>
        ''' Restituisce una stringa che può essere utilizzata come header
        ''' </summary>
        ''' <param name="text"></param>
        ''' <param name="charsetName"></param>
        ''' <param name="encoding"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function Encode(ByVal text As String, ByVal charsetName As String, ByVal encoding As HeaderCharSetEncoding) As String
            Return "=?" & Trim(charsetName) & "?" & CStr(IIf(encoding = HeaderCharSetEncoding.Base64, "B", "Q")) & "?" & text & "?="
        End Function

        ' ''' <summary>
        ' ''' Decodifica un header
        ' ''' </summary>
        ' ''' <param name="header"></param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Shared Function Decode(ByVal header As String) As String
        '    If (Left(header, 2) = "=?") AndAlso (Right(header, 2) = "?=") Then
        '        Dim charsetName As String = ""
        '        Dim encoding As String = ""
        '        Dim text As String = ""

        '        Dim p1 As Integer = InStr(3, header, "?")
        '        If (p1 < 0) Then Return header
        '        charsetName = Mid(header, 3, p1 - 3)

        '        Dim p2 As Integer = InStr(p1 + 1, header, "?")
        '        If (p2 < p1) Then Return header

        '        encoding = Mid(header, p1 + 1, p2 - p1)

        '        text = Mid(header, p2 + 1, Len(header) - p2 - 2)

        '        Select Case (encoding)
        '            Case "B"
        '                Return System.Text.Encoding.UTF8 .
        '            Case "Q"

        '            Case Else
        '                Return header
        '        End Select
        '    Else
        '        Return header
        '    End If
        'End Function

        'Private Shared charSet As String = ""

        Public Shared Function Decode(ByVal mimeString As String) As String
            'Example: mimeString = "=?Windows-1252?Q?Registered_Member_News=3A_WPC09_to_feature_Windows_7=2C_?==?Windows-1252?Q?_Office=2C_Exchange=2C_more=85?="
            'In this example two Q-encoded strings are defined!
            Dim encodedString As String = Trim(mimeString)
            Dim decodedString As String = ""

            While (encodedString <> "")
                Dim p1 As Integer = encodedString.IndexOf("=?")
                If (p1 < 0) Then Return decodedString & encodedString

                Dim p2 As Integer = encodedString.IndexOf("?", p1 + 2)
                If (p2 <= p1 + 2) Then Return decodedString & encodedString

                Dim charSet As String = encodedString.Substring(p1 + 2, p2 - p1 - 2)

                Dim p3 As Integer = encodedString.IndexOf("?", p2 + 1)
                If (p3 <= p2 + 1) Then Return decodedString & encodedString

                Dim encoding As String = UCase(encodedString.Substring(p2 + 1, p3 - p2 - 1))

                Dim p4 As Integer = encodedString.IndexOf("?=", p3 + 1)
                If (p4 <= p3 + 1) Then Return decodedString & encodedString

                Dim value As String = encodedString.Substring(p3 + 1, p4 - p3 - 1)

                Dim ev As New MatchEvaluator1(charSet)
                If (value <> "") Then
                    If (encoding = "B") Then 'if the string is base-64
                        Dim bytes As Byte() = Convert.FromBase64String(value)
                        decodedString &= System.Text.Encoding.GetEncoding(charSet).GetString(bytes)
                    ElseIf (encoding = "Q") Then 'if string is Q-encoded
                        'parse looking for =XX where XX is hexadecimal
                        Dim re As Regex = New Regex("(\=([0-9A-F][0-9A-F]))", RegexOptions.IgnoreCase) '"(\\=([0-9A-F][0-9A-F]))",
                        decodedString &= re.Replace(value, New MatchEvaluator(AddressOf ev.HexDecoderEvaluator))
                        decodedString = decodedString.Replace("_", " ")
                    Else
                        ' SNH No decoder defined
                        ' Match should NOT be successfull
                        Return decodedString & encodedString
                    End If
                End If

                encodedString = encodedString.Substring(p4 + 2)
            End While

            Return decodedString & encodedString

            'Dim charSet As String
            'Dim ev As MatchEvaluator1

            'While (encodedString.Length <> 0)
            '    Dim match As Match = Regex.Match(encodedString, "=\?(?<charset>.*?)\?(?<encoding>[qQbB])\?(?<value>.*?)\?=")
            '    If (match.Success) Then
            '        charSet = match.Groups("charset").Value
            '        ev = New MatchEvaluator1(charSet)

            '        Dim encoding As String = match.Groups("encoding").Value.ToUpper()
            '        Dim value As String = match.Groups("value").Value
            '        If (encoding.ToLower().Equals("b")) Then 'if the string is base-64
            '            Dim bytes As Byte() = Convert.FromBase64String(value)
            '            decodedString &= System.Text.Encoding.GetEncoding(charSet).GetString(bytes)
            '        ElseIf (encoding.ToLower().Equals("q")) Then 'if string is Q-encoded
            '            'parse looking for =XX where XX is hexadecimal
            '            Dim re As Regex = New Regex("(\=([0-9A-F][0-9A-F]))", RegexOptions.IgnoreCase) '"(\\=([0-9A-F][0-9A-F]))",
            '            decodedString &= re.Replace(value, New MatchEvaluator(AddressOf ev.HexDecoderEvaluator))
            '            decodedString = decodedString.Replace("_", " ")
            '        Else
            '            ' SNH No decoder defined
            '            ' Match should NOT be successfull
            '            Return mimeString
            '        End If
            '        ' When multiple entries, subtract the currently decoded part
            '        encodedString = encodedString.Substring(match.Index + match.Length)

            '        'Gli ultimi caratteri di codifica a volte non vengono compresi??
            '        'If (encodedString = "?=") OrElse (encodedString = "=") Then Exit While

            '        ' Successfull
            '        Return decodedString
            '    Else
            '        ' Unable to decode (not mime encoded)
            '        Return mimeString
            '    End If
            'End While


        End Function

        Private Class MatchEvaluator1
            Public charSet As String
            Public en As System.Text.Encoding

            Public Sub New(ByVal charSet As String)
                Me.charSet = charSet
                Me.en = System.Text.Encoding.GetEncoding(charSet)
            End Sub

            Public Function HexDecoderEvaluator(ByVal m As Match) As String
                Dim hex As String = m.Groups(2).Value
                Dim iHex As Integer = Convert.ToInt32(hex, 16)
                ' Rerutn the string in the charset defined
                Dim bytes As Byte() = {Convert.ToByte(iHex)}
                Return Me.en.GetString(bytes)
                ' This will not work properly on "=85" in example string
                'char c = (char)iHex;
                'return c.ToString();
            End Function

        End Class



    End Class

End Namespace
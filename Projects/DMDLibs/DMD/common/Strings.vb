Imports DMD
Imports DMD.Databases
Imports System.Net
Imports DMD.Sistema
Imports DMD.Internals

Namespace Internals


    Public NotInheritable Class CStringsClass
        Private m_DefaultComparer As New CStringComparer(CompareMethod.Binary)
        Private m_DefaultComparerIgnoreCase As New CStringComparer(CompareMethod.Text)

        Public ReadOnly Property DefaultComparer As IComparer
            Get
                Return m_DefaultComparer
            End Get
        End Property

        Public ReadOnly Property DefaultComparerIgnoreCase As IComparer
            Get
                Return m_DefaultComparerIgnoreCase
            End Get
        End Property

        ''' <summary>
        ''' Restituisce la stringa che rappresenta il valore intero [value] in formato esadecimale
        ''' </summary>
        ''' <param name="value">[in] Valore intero da rappresentare in formato esadecimale</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Hex(ByVal value As Object) As String
            Return Microsoft.VisualBasic.Hex(value)
        End Function


        Public Function Hex(ByVal value As Object, ByVal len As Integer, Optional ByVal padChar As String = "0") As String
            Return Right(NChars(len, padChar) & Microsoft.VisualBasic.Hex(value), len)
        End Function

        Public Function Combine(ByVal str1 As String, ByVal str2 As String, ByVal separator As String, Optional ByVal useBrackets As Boolean = False) As String
            If (str1 <> "") Then
                If (str2 <> "") Then
                    Dim ret As New System.Text.StringBuilder
                    If (useBrackets) Then ret.Append("(")
                    ret.Append(str1)
                    If (useBrackets) Then ret.Append(")")
                    ret.Append(separator)
                    If (useBrackets) Then ret.Append("(")
                    ret.Append(str2)
                    If (useBrackets) Then ret.Append(")")
                    Return ret.ToString
                Else
                    Return str1
                End If
            Else
                Return str2
            End If
        End Function

        Public Function CountSubstrings(ByVal str As String, ByVal substr As String) As Integer
            If (str = vbNullString) Then Return 0

            Dim count As Integer = 0
            Dim idx As Integer = 0
            Dim l As Integer = Len(substr)
            Do
                idx = str.IndexOf(substr, idx + 1)
                If (idx > 0) Then
                    count += 1
                    idx += l
                Else
                    Exit Do
                End If
            Loop
            Return count
        End Function

        Public Function JSNumber(ByVal value As Object) As String
            Dim num As Nullable(Of Double) = Formats.ParseDouble(value)
            If (Types.IsNull(num)) Then Return "null"
            Return CStr(num.Value).Replace(","c, "."c)
        End Function

        Public Function ToJS(ByVal text As String) As String
            Return Replace(Replace(Replace(text, "'", "&apos;"), Chr(34), "&quot;"), vbCrLf, "")
        End Function

        Public Function URLEncode(ByVal value As String) As String
            Return System.Web.HttpUtility.UrlEncode(value)
        End Function

        Public Function URLDecode(ByVal value As String) As String
            Return System.Web.HttpUtility.UrlDecode(value)
        End Function

        Public Function HtmlEncode(ByVal value As String) As String
            If (value = vbNullString) Then Return ""
            'value = Replace(value, "")
            Return System.Web.HttpUtility.HtmlEncode(value)
        End Function

        Public Function HtmlDecode(ByVal value As String) As String
            If (value = vbNullString) Then Return ""
            Return System.Web.HttpUtility.HtmlDecode(value)
        End Function

        Public Function [CStr](ByVal s As Object) As String
            If TypeOf (s) Is DBNull OrElse (s Is Nothing) Then Return ""
            Return s.ToString
        End Function

        Public Function Len(ByVal s As String) As Integer
            Return Microsoft.VisualBasic.Len(s)
        End Function

        Public Function Trim(ByVal s As String) As String
            Return Microsoft.VisualBasic.Trim(s)
        End Function

        Public Function LCase(ByVal s As String) As String
            Return Microsoft.VisualBasic.LCase(s)
        End Function

        Public Function UCase(ByVal s As String) As String
            Return Microsoft.VisualBasic.UCase(s)
        End Function

        Public Function Replace(ByVal s As String, ByVal s1 As String, ByVal s2 As String) As String
            Return Microsoft.VisualBasic.Replace(s, s1, s2)
        End Function

        Public Function Replace(ByVal s As String, ByVal s1 As Char, ByVal s2 As Char) As String
            Return Microsoft.VisualBasic.Replace(s, s1, s2)
        End Function

        Public Function Left(ByVal s As String, ByVal len As Integer) As String
            Return Microsoft.VisualBasic.Left(s, len)
        End Function

        Public Function Mid(ByVal s As String, ByVal [from] As Integer) As String
            Return Microsoft.VisualBasic.Mid(s, [from])
        End Function

        Public Function Mid(ByVal s As String, ByVal [from] As Integer, ByVal len As Integer) As String
            Return Microsoft.VisualBasic.Mid(s, [from], len)
        End Function

        Public Function IndexOf(ByVal s As String, ByVal s1 As String) As Integer
            Return Microsoft.VisualBasic.InStr(s, s1)
        End Function

        Public Function IndexOfRev(ByVal s As String, ByVal s1 As String) As Integer
            Return Microsoft.VisualBasic.InStrRev(s, s1)
        End Function

        Public Function InStr(ByVal s As String, ByVal s1 As String) As Integer
            Return Microsoft.VisualBasic.InStr(s, s1)
        End Function

        Public Function InStr(ByVal from As Integer, ByVal s As String, ByVal s1 As String) As Integer
            Return Microsoft.VisualBasic.InStr([from], s, s1)
        End Function

        Public Function InStr(ByVal text As String, ByVal strToFind As String, ByVal method As CompareMethod) As Integer
            Return Microsoft.VisualBasic.InStr(text, strToFind, method)
        End Function

        Public Function InStrRev(ByVal s As String, ByVal s1 As String) As Integer
            Return Microsoft.VisualBasic.InStrRev(s, s1)
        End Function

        Public Function InStrRev(ByVal from As Integer, ByVal s As String, ByVal s1 As String) As Integer
            Return Microsoft.VisualBasic.InStrRev([from], s, s1)
        End Function

        Public Function Right(ByVal s As String, ByVal len As Integer) As String
            Return Microsoft.VisualBasic.Right(s, len)
        End Function

        Public Function Split(ByVal s As String, ByVal sep As String) As String()
            Return Microsoft.VisualBasic.Split(s, sep)
        End Function

        Public Function Join(ByVal items() As String, ByVal sep As String) As String
            Return Microsoft.VisualBasic.Join(items, sep)
        End Function

        Public Function Join(Of T)(ByVal items() As T, ByVal sep As String) As String
            Dim ret As New System.Text.StringBuilder
            For i As Integer = 0 To Arrays.Len(items) - 1
                If (i > 0) Then ret.Append(sep)
                ret.Append(items(i))
            Next
            Return ret.ToString
        End Function

        Public Function JoinW(ParamArray items() As String) As String
            Return Microsoft.VisualBasic.Join(items, "")
        End Function

        Public Function Compare(ByVal a As String, ByVal b As String, Optional ByVal bt As Microsoft.VisualBasic.CompareMethod = CompareMethod.Text) As Integer
            Return Microsoft.VisualBasic.StrComp(a, b, bt)
        End Function

        Public Function ToProperCase(ByVal value As String) As String
            If (value = vbNullString) Then Return value

            Dim i As Integer
            Dim t As Boolean
            Dim ch As String
            Dim ret As New System.Text.StringBuilder(value.Length)
            t = True
            For i = 0 To Len(value) - 1
                ch = value.Chars(i)
                If (t) Then
                    ch = ch.ToUpper()
                Else
                    ch = ch.ToLower()
                End If
                ret.Append(ch)
                t = (ch = ".") Or (ch = "?") Or (ch = "!") Or (t And (ch = " "))
            Next
            Return ret.ToString
        End Function

        Public Function ToNameCase(ByVal value As String) As String
            If (value = vbNullString) Then Return value

            Dim i, t As Integer
            Dim ch As String
            Dim ret As New System.Text.StringBuilder(value.Length)
            value = Strings.Replace(Strings.Trim(value), "  ", "") : t = 1
            For i = 1 To Len(value)
                ch = Mid(value, i, 1)
                Select Case (t)
                    Case 0
                        ch = LCase(ch)
                        ret.Append(ch)
                        If ((ch = ",") Or (ch = ".") Or (ch = "?") Or (ch = "!") Or (ch = " ")) Then
                            t = 1
                        ElseIf (ch = "'") Then
                            t = 2
                        Else
                            t = 0
                        End If
                    Case 1
                        ch = UCase(ch)
                        ret.Append(ch)
                        If ((ch = ",") Or (ch = ".") Or (ch = "?") Or (ch = "!") Or (ch = " ")) Then
                            t = 1
                        ElseIf (ch = "'") Then
                            t = 2
                        Else
                            t = 0
                        End If
                    Case 2
                        If (ch <> " ") Then
                            ret.Append(UCase(ch))
                            t = 0
                        End If
                End Select
            Next
            Return ret.ToString
        End Function

        Public Function NChars(ByVal len As Integer, Optional ByVal padChar As String = " ") As String
            If (padChar = vbNullString OrElse padChar.Length < 1) Then Return vbNullString
            Dim ret As New System.Text.StringBuilder(len * padChar.Length)
            For i As Integer = 0 To len - 1
                ret.Append(padChar)
            Next
            Return ret.ToString
        End Function

        Public Function PadLeft(ByVal text As String, ByVal len As Integer, ByVal padChar As String) As String
            Return text.PadLeft(len, padChar) ' Strings.Right(Strings.NChars(len, padChar) & text, len)
        End Function

        Public Function PadRight(ByVal text As String, ByVal len As Integer, ByVal padChar As String) As String
            Return text.PadRight(len, padChar) ' Strings.Left(text + Strings.NChars(len, padChar), len)
        End Function


        Public Function RemoveHTMLTags(ByVal strText As String) As String
            Const TAGLIST As String = ";!--;!DOCTYPE;A;ACRONYM;ADDRESS;APPLET;AREA;B;BASE;BASEFONT;" &
                  "BGSOUND;BIG;BLOCKQUOTE;BODY;BR;BUTTON;CAPTION;CENTER;CITE;CODE;" &
                  "COL;COLGROUP;COMMENT;DD;DEL;DFN;DIR;DIV;DL;DT;EM;EMBED;FIELDSET;" &
                  "FONT;FORM;FRAME;FRAMESET;HEAD;H1;H2;H3;H4;H5;H6;HR;HTML;I;IFRAME;IMG;" &
                  "INPUT;INS;ISINDEX;KBD;LABEL;LAYER;LAGEND;LI;LINK;LISTING;MAP;MARQUEE;" &
                  "MENU;META;NOBR;NOFRAMES;NOSCRIPT;OBJECT;OL;OPTION;P;PARAM;PLAINTEXT;" &
                  "PRE;Q;S;SAMP;SCRIPT;SELECT;SMALL;SPAN;STRIKE;STRONG;STYLE;SUB;SUP;" &
                  "TABLE;TBODY;TD;TEXTAREA;TFOOT;TH;THEAD;TITLE;TR;TT;U;UL;VAR;WBR;XMP;"
            Const BLOCKTAGLIST = ";APPLET;EMBED;FRAMESET;HEAD;NOFRAMES;NOSCRIPT;OBJECT;SCRIPT;STYLE;"
            Dim nPos1, nPos2, nPos3 As Integer
            Dim strResult, strTagName As String
            Dim bRemove, bSearchForBlock As Boolean
            'strText = Replace(strText, "<br/>", vbNewLine)
            'strText = Replace(strText, "<br>", vbNewLine)

            strResult = vbNullString
            nPos1 = InStr(strText, "<")
            Do While nPos1 > 0
                nPos2 = InStr(nPos1 + 1, strText, ">")
                If nPos2 > 0 Then
                    strTagName = Microsoft.VisualBasic.Mid(strText, nPos1 + 1, nPos2 - nPos1 - 1)
                    'strTagName = Microsoft.VisualBasic.Replace(Replace(strTagName, vbCr, " "), vbLf, " ")

                    nPos3 = InStr(strTagName, " ")
                    If nPos3 > 0 Then
                        strTagName = Microsoft.VisualBasic.Left(strTagName, nPos3 - 1)
                    End If

                    If Left(strTagName, 1) = "/" Then
                        strTagName = Microsoft.VisualBasic.Mid(strTagName, 2)
                        strResult = strResult & Microsoft.VisualBasic.Left(strText, nPos1 - 1)
                        Select Case LCase(Trim(strTagName))
                            Case "tr"
                                strResult &= vbNewLine
                        End Select

                        bSearchForBlock = False
                    Else
                        bSearchForBlock = True
                    End If

                    If Right(strTagName, 1) = "/" Then
                        strTagName = Trim(Microsoft.VisualBasic.Left(strTagName, Len(strTagName) - 1))
                        bSearchForBlock = False
                    Else
                        bSearchForBlock = True
                    End If

                    If Microsoft.VisualBasic.InStr(1, TAGLIST, Strings.JoinW(";", strTagName, ";"), vbTextCompare) > 0 Then
                        bRemove = True
                        If bSearchForBlock Then
                            If Microsoft.VisualBasic.InStr(1, BLOCKTAGLIST, Strings.JoinW(";", strTagName, ";"), vbTextCompare) > 0 Then
                                nPos2 = Microsoft.VisualBasic.Len(strText)
                                nPos3 = Microsoft.VisualBasic.InStr(nPos1 + 1, strText, "</" & strTagName, vbTextCompare)
                                If nPos3 > 0 Then
                                    nPos3 = InStr(nPos3 + 1, strText, ">")
                                End If

                                If nPos3 > 0 Then
                                    nPos2 = nPos3
                                End If
                            End If
                        End If
                    Else
                        bRemove = False
                    End If

                    If bRemove Then
                        strResult = strResult & Microsoft.VisualBasic.Left(strText, nPos1 - 1)
                        Select Case LCase(Trim(strTagName))
                            Case "br", "br/", "/tr"
                                strResult &= vbNewLine
                        End Select

                        strText = Microsoft.VisualBasic.Mid(strText, nPos2 + 1)
                    Else
                        strResult = strResult & Microsoft.VisualBasic.Left(strText, nPos1)
                        strText = Microsoft.VisualBasic.Mid(strText, nPos1 + 1)
                    End If
                Else
                    strResult = strResult & strText
                    strText = ""
                End If

                nPos1 = InStr(strText, "<")
            Loop
            strResult = strResult & strText


            Return HtmlDecode(strResult)
        End Function

        Public Function IsWhiteSpace(ByVal ch As Char) As Boolean
            Return ch = " " OrElse ch = vbCr OrElse ch = vbLf OrElse ch = vbTab OrElse ch = vbFormFeed
        End Function

        Public Function IsAlphaOrNumber(ByVal ch As Char) As Boolean
            Return Global.System.Char.IsLetterOrDigit(ch)
        End Function

        Private Enum RHTMLS
            Normal
            LookForTagName
            BuildTagName
            LookForTagEnd
            LookForClosingTag
        End Enum

        Public Function RemoveHTMLTags1(ByVal strText As String) As String
            Dim l As Integer = Len(strText)
            Dim ret As New System.Text.StringBuilder(Len(strText))
            Dim i As Integer = 0
            Dim stato As RHTMLS = RHTMLS.Normal
            Dim tagName As String = ""
            Dim prevChar As String = ""

            While (i < l)
                Dim ch As Char = strText.Chars(i)
                Select Case stato
                    Case RHTMLS.Normal
                        If (ch = "<") Then
                            stato = RHTMLS.LookForTagName
                            tagName = ""
                        Else
                            ret.Append(ch)
                        End If
                    Case RHTMLS.LookForTagName
                        If (Not IsWhiteSpace(ch)) Then
                            tagName = ch
                            stato = RHTMLS.BuildTagName
                        End If
                    Case RHTMLS.BuildTagName
                        If IsAlphaOrNumber(ch) Then
                            tagName &= ch
                        Else
                            stato = RHTMLS.LookForTagEnd
                        End If
                    Case RHTMLS.LookForTagEnd
                        If (ch = ">") Then
                            stato = RHTMLS.Normal
                        End If
                End Select

                prevChar = ch
                i += 1
            End While

            Return HtmlDecode(ret.ToString)
        End Function

        ''' <summary>
        ''' Restituisce vero se il carattere è un valore numerico
        ''' </summary>
        ''' <param name="c"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsDigit(ByVal c As Char) As Boolean
            Return (c >= "0"c) AndAlso (c <= "9"c)
        End Function




        ''' <summary>
        ''' Converte il buffer in una stringa utilizzando il set di caratteri specificato con nome
        ''' </summary>
        ''' <param name="buffer">[in] Array di bytes da convertire</param>
        ''' <param name="charSet">[in] Nome del set di caratteri</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetString(ByVal buffer() As Byte, ByVal charSet As String) As String
            Dim en As System.Text.Encoding = System.Text.Encoding.GetEncoding(charSet)
            '
            'charSet = UCase(Trim(charSet))
            'Select Case charSet
            '    Case "ISO-8859-1" : en = System.Text.Encoding.GetEncoding(28591)
            '    Case "US-ASCII"
            '    Case "UTF-16BE"
            'End Select


            Return en.GetString(buffer) ' System.Text.Encoding.Convert(System.Text.Encoding.Default, System.Text.Encoding.GetEncoding(28591))
        End Function

        ''' <summary>
        ''' Converte il buffer in una stringa utilizzando il set di caratteri specificato con nome
        ''' </summary>
        ''' <param name="buffer">[in] Array di bytes da convertire</param>
        ''' <param name="charSet">[in] Nome del set di caratteri</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetString(ByVal buffer() As Byte, ByVal offset As Integer, ByVal len As Integer, ByVal charSet As String) As String
            'charSet = UCase(Trim(charSet))
            'Select Case charSet
            '    Case "ISO-8859-1"
            '    Case "US-ASCII"
            '    Case "UTF-16BE"
            'End Select
            'Throw New NotImplementedException
            Dim en As System.Text.Encoding = System.Text.Encoding.GetEncoding(charSet)
            Return en.GetString(buffer, offset, len)
        End Function

        Public Function GetString(ByVal buffer() As Byte, ByVal offset As Integer, ByVal len As Integer) As String
            Return System.Text.Encoding.Default.GetString(buffer, offset, len)
        End Function

        Public Function GetString(ByVal buffer() As Byte) As String
            Return System.Text.Encoding.Default.GetString(buffer)
        End Function



        Public Function ConvertEncoding(ByVal str As String, ByVal from As CodePages, ByVal [to] As CodePages) As String
            Dim fromCP As System.Text.Encoding = System.Text.Encoding.GetEncoding(from)
            Dim toCP As System.Text.Encoding = System.Text.Encoding.GetEncoding([to])
            Dim buff() As Byte = fromCP.GetBytes(str)
            buff = System.Text.Encoding.Convert(fromCP, toCP, buff)
            Return toCP.GetString(buff)
        End Function


        ''' <summary>
        ''' Restituisce un buffer di bytes contenente la rappresentazione della stringa 
        ''' </summary>
        ''' <param name="value">[in] La stringa di cui restituire il buffer</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetBytes(ByVal value As String) As Byte()
            Return System.Text.Encoding.Default.GetBytes(value)
        End Function

        Public Function GetBytes(ByVal value As String, ByVal charSet As String) As Byte()
            'Select Case charSet
            '    Case "ISO-8859-1"
            '    Case "US-ASCII"
            'End Select
            Dim en As System.Text.Encoding = System.Text.Encoding.GetEncoding(charSet)
            Return en.GetBytes(value)
        End Function

        Function GetString(ByVal chars As Char(), ByVal start As Integer, ByVal count As Integer, ByVal encoding As String) As String
            Dim str As New String(chars)
            Dim en As System.Text.Encoding = System.Text.Encoding.GetEncoding(encoding)
            Return en.GetString(Strings.GetBytes(str), start, count)
        End Function

        ''' <summary>
        ''' Restituisce vero se la stringa verifica la regular expression
        ''' </summary>
        ''' <param name="str"></param>
        ''' <param name="expression"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Metches(ByVal str As String, ByVal expression As String) As Boolean
            Dim reg As New System.Text.RegularExpressions.Regex(str)
            Return reg.IsMatch(str)
        End Function

        ''' <summary>
        ''' Sostituisce una o più sottostringe usande la regular expression subStr
        ''' </summary>
        ''' <param name="str"></param>
        ''' <param name="subStr"></param>
        ''' <param name="changeTo"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ReplaceAll(ByVal str As String, ByVal subStr As String, ByVal changeTo As String) As String
            Dim reg As New System.Text.RegularExpressions.Regex(subStr)
            Return reg.Replace(str, changeTo)
        End Function

        Friend Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Private m_CodePages As New CKeyCollection(Of Integer)

        Shared Sub New()
            '        m_CodePages.Add("IBM037", 37)     'IBM EBCDIC US-Canada
            '        m_CodePages.Add("IBM437", 437) '	OEM United States
            'm_CodePages.Add("500	IBM500	IBM EBCDIC International
            'm_CodePages.Add("708	ASMO-708	Arabic (ASMO 708)
            'm_CodePages.Add("709		Arabic (ASMO-449+, BCON V4)
            'm_CodePages.Add("710		Arabic - Transparent Arabic
            'm_CodePages.Add("720	DOS-720	Arabic (Transparent ASMO); Arabic (DOS)
            'm_CodePages.Add("737	ibm737	OEM Greek (formerly 437G); Greek (DOS)
            'm_CodePages.Add("775	ibm775	OEM Baltic; Baltic (DOS)
            'm_CodePages.Add("850	ibm850	OEM Multilingual Latin 1; Western European (DOS)
            'm_CodePages.Add("852	ibm852	OEM Latin 2; Central European (DOS)
            'm_CodePages.Add("855	IBM855	OEM Cyrillic (primarily Russian)
            'm_CodePages.Add("857	ibm857	OEM Turkish; Turkish (DOS)
            'm_CodePages.Add("858	IBM00858	OEM Multilingual Latin 1 + Euro symbol
            'm_CodePages.Add("860	IBM860	OEM Portuguese; Portuguese (DOS)
            'm_CodePages.Add("861	ibm861	OEM Icelandic; Icelandic (DOS)
            'm_CodePages.Add("862	DOS-862	OEM Hebrew; Hebrew (DOS)
            'm_CodePages.Add("863	IBM863	OEM French Canadian; French Canadian (DOS)
            'm_CodePages.Add("864	IBM864	OEM Arabic; Arabic (864)
            'm_CodePages.Add("865	IBM865	OEM Nordic; Nordic (DOS)
            'm_CodePages.Add("866	cp866	OEM Russian; Cyrillic (DOS)
            'm_CodePages.Add("869	ibm869	OEM Modern Greek; Greek, Modern (DOS)
            'm_CodePages.Add("870	IBM870	IBM EBCDIC Multilingual/ROECE (Latin 2); IBM EBCDIC Multilingual Latin 2
            'm_CodePages.Add("874	windows-874	ANSI/OEM Thai (ISO 8859-11); Thai (Windows)
            'm_CodePages.Add("875	cp875	IBM EBCDIC Greek Modern
            'm_CodePages.Add("932	shift_jis	ANSI/OEM Japanese; Japanese (Shift-JIS)
            'm_CodePages.Add("936	gb2312	ANSI/OEM Simplified Chinese (PRC, Singapore); Chinese Simplified (GB2312)
            'm_CodePages.Add("949	ks_c_5601-1987	ANSI/OEM Korean (Unified Hangul Code)
            'm_CodePages.Add("950	big5	ANSI/OEM Traditional Chinese (Taiwan; Hong Kong SAR, PRC); Chinese Traditional (Big5)
            'm_CodePages.Add("1026	IBM1026	IBM EBCDIC Turkish (Latin 5)
            'm_CodePages.Add("1047	IBM01047	IBM EBCDIC Latin 1/Open System
            'm_CodePages.Add("1140	IBM01140	IBM EBCDIC US-Canada (037 + Euro symbol); IBM EBCDIC (US-Canada-Euro)
            'm_CodePages.Add("1141	IBM01141	IBM EBCDIC Germany (20273 + Euro symbol); IBM EBCDIC (Germany-Euro)
            'm_CodePages.Add("1142	IBM01142	IBM EBCDIC Denmark-Norway (20277 + Euro symbol); IBM EBCDIC (Denmark-Norway-Euro)
            'm_CodePages.Add("1143	IBM01143	IBM EBCDIC Finland-Sweden (20278 + Euro symbol); IBM EBCDIC (Finland-Sweden-Euro)
            'm_CodePages.Add("1144	IBM01144	IBM EBCDIC Italy (20280 + Euro symbol); IBM EBCDIC (Italy-Euro)
            'm_CodePages.Add("1145	IBM01145	IBM EBCDIC Latin America-Spain (20284 + Euro symbol); IBM EBCDIC (Spain-Euro)
            'm_CodePages.Add("1146	IBM01146	IBM EBCDIC United Kingdom (20285 + Euro symbol); IBM EBCDIC (UK-Euro)
            'm_CodePages.Add("1147	IBM01147	IBM EBCDIC France (20297 + Euro symbol); IBM EBCDIC (France-Euro)
            'm_CodePages.Add("1148	IBM01148	IBM EBCDIC International (500 + Euro symbol); IBM EBCDIC (International-Euro)
            'm_CodePages.Add("1149	IBM01149	IBM EBCDIC Icelandic (20871 + Euro symbol); IBM EBCDIC (Icelandic-Euro)
            'm_CodePages.Add("1200	utf-16	Unicode UTF-16, little endian byte order (BMP of ISO 10646); available only to managed applications
            'm_CodePages.Add("1201	unicodeFFFE	Unicode UTF-16, big endian byte order; available only to managed applications
            'm_CodePages.Add("1250	windows-1250	ANSI Central European; Central European (Windows)
            'm_CodePages.Add("1251	windows-1251	ANSI Cyrillic; Cyrillic (Windows)
            'm_CodePages.Add("1252	windows-1252	ANSI Latin 1; Western European (Windows)
            'm_CodePages.Add("1253	windows-1253	ANSI Greek; Greek (Windows)
            'm_CodePages.Add("1254	windows-1254	ANSI Turkish; Turkish (Windows)
            'm_CodePages.Add("1255	windows-1255	ANSI Hebrew; Hebrew (Windows)
            'm_CodePages.Add("1256	windows-1256	ANSI Arabic; Arabic (Windows)
            'm_CodePages.Add("1257	windows-1257	ANSI Baltic; Baltic (Windows)
            'm_CodePages.Add("1258	windows-1258	ANSI/OEM Vietnamese; Vietnamese (Windows)
            'm_CodePages.Add("1361:   Johab Korean(Johab)
            'm_CodePages.Add("10000	macintosh	MAC Roman; Western European (Mac)
            'm_CodePages.Add("10001	x-mac-japanese	Japanese (Mac)
            'm_CodePages.Add("10002	x-mac-chinesetrad	MAC Traditional Chinese (Big5); Chinese Traditional (Mac)
            'm_CodePages.Add("10003	x-mac-korean	Korean (Mac)
            'm_CodePages.Add("10004	x-mac-arabic	Arabic (Mac)
            'm_CodePages.Add("10005	x-mac-hebrew	Hebrew (Mac)
            'm_CodePages.Add("10006	x-mac-greek	Greek (Mac)
            'm_CodePages.Add("10007	x-mac-cyrillic	Cyrillic (Mac)
            'm_CodePages.Add("10008	x-mac-chinesesimp	MAC Simplified Chinese (GB 2312); Chinese Simplified (Mac)
            'm_CodePages.Add("10010	x-mac-romanian	Romanian (Mac)
            'm_CodePages.Add("10017	x-mac-ukrainian	Ukrainian (Mac)
            'm_CodePages.Add("10021	x-mac-thai	Thai (Mac)
            'm_CodePages.Add("10029	x-mac-ce	MAC Latin 2; Central European (Mac)
            'm_CodePages.Add("10079	x-mac-icelandic	Icelandic (Mac)
            'm_CodePages.Add("10081	x-mac-turkish	Turkish (Mac)
            'm_CodePages.Add("10082	x-mac-croatian	Croatian (Mac)
            'm_CodePages.Add("12000	utf-32	Unicode UTF-32, little endian byte order; available only to managed applications
            'm_CodePages.Add("12001	utf-32BE	Unicode UTF-32, big endian byte order; available only to managed applications
            'm_CodePages.Add("20000	x-Chinese_CNS	CNS Taiwan; Chinese Traditional (CNS)
            'm_CodePages.Add("20001	x-cp20001	TCA Taiwan
            'm_CodePages.Add("20002	x_Chinese-Eten	Eten Taiwan; Chinese Traditional (Eten)
            'm_CodePages.Add("20003	x-cp20003	IBM5550 Taiwan
            'm_CodePages.Add("20004	x-cp20004	TeleText Taiwan
            'm_CodePages.Add("20005	x-cp20005	Wang Taiwan
            'm_CodePages.Add("20105	x-IA5	IA5 (IRV International Alphabet No. 5, 7-bit); Western European (IA5)
            'm_CodePages.Add("20106	x-IA5-German	IA5 German (7-bit)
            'm_CodePages.Add("20107	x-IA5-Swedish	IA5 Swedish (7-bit)
            'm_CodePages.Add("20108	x-IA5-Norwegian	IA5 Norwegian (7-bit)
            'm_CodePages.Add("20127	us-ascii	US-ASCII (7-bit)
            'm_CodePages.Add("20261	x-cp20261	T.61
            'm_CodePages.Add("20269	x-cp20269	ISO 6937 Non-Spacing Accent
            'm_CodePages.Add("20273	IBM273	IBM EBCDIC Germany
            'm_CodePages.Add("20277	IBM277	IBM EBCDIC Denmark-Norway
            'm_CodePages.Add("20278	IBM278	IBM EBCDIC Finland-Sweden
            'm_CodePages.Add("20280	IBM280	IBM EBCDIC Italy
            'm_CodePages.Add("20284	IBM284	IBM EBCDIC Latin America-Spain
            'm_CodePages.Add("20285	IBM285	IBM EBCDIC United Kingdom
            'm_CodePages.Add("20290	IBM290	IBM EBCDIC Japanese Katakana Extended
            'm_CodePages.Add("20297	IBM297	IBM EBCDIC France
            'm_CodePages.Add("20420	IBM420	IBM EBCDIC Arabic
            'm_CodePages.Add("20423	IBM423	IBM EBCDIC Greek
            'm_CodePages.Add("20424	IBM424	IBM EBCDIC Hebrew
            'm_CodePages.Add("20833	x-EBCDIC-KoreanExtended	IBM EBCDIC Korean Extended
            'm_CodePages.Add("20838	IBM-Thai	IBM EBCDIC Thai
            'm_CodePages.Add("20866	koi8-r	Russian (KOI8-R); Cyrillic (KOI8-R)
            'm_CodePages.Add("20871	IBM871	IBM EBCDIC Icelandic
            'm_CodePages.Add("20880	IBM880	IBM EBCDIC Cyrillic Russian
            'm_CodePages.Add("20905	IBM905	IBM EBCDIC Turkish
            'm_CodePages.Add("20924	IBM00924	IBM EBCDIC Latin 1/Open System (1047 + Euro symbol)
            'm_CodePages.Add("20932	EUC-JP	Japanese (JIS 0208-1990 and 0212-1990)
            'm_CodePages.Add("20936	x-cp20936	Simplified Chinese (GB2312); Chinese Simplified (GB2312-80)
            'm_CodePages.Add("20949	x-cp20949	Korean Wansung
            'm_CodePages.Add("21025	cp1025	IBM EBCDIC Cyrillic Serbian-Bulgarian
            'm_CodePages.Add("21027		(deprecated)
            'm_CodePages.Add("21866	koi8-u	Ukrainian (KOI8-U); Cyrillic (KOI8-U)
            'm_CodePages.Add("28591	iso-8859-1	ISO 8859-1 Latin 1; Western European (ISO)
            'm_CodePages.Add("28592	iso-8859-2	ISO 8859-2 Central European; Central European (ISO)
            'm_CodePages.Add("28593	iso-8859-3	ISO 8859-3 Latin 3
            'm_CodePages.Add("28594	iso-8859-4	ISO 8859-4 Baltic
            'm_CodePages.Add("28595	iso-8859-5	ISO 8859-5 Cyrillic
            'm_CodePages.Add("28596	iso-8859-6	ISO 8859-6 Arabic
            'm_CodePages.Add("28597	iso-8859-7	ISO 8859-7 Greek
            'm_CodePages.Add("28598	iso-8859-8	ISO 8859-8 Hebrew; Hebrew (ISO-Visual)
            'm_CodePages.Add("28599	iso-8859-9	ISO 8859-9 Turkish
            'm_CodePages.Add("28603	iso-8859-13	ISO 8859-13 Estonian
            'm_CodePages.Add("28605	iso-8859-15	ISO 8859-15 Latin 9
            'm_CodePages.Add("29001	x-Europa	Europa 3
            'm_CodePages.Add("38598	iso-8859-8-i	ISO 8859-8 Hebrew; Hebrew (ISO-Logical)
            'm_CodePages.Add("50220	iso-2022-jp	ISO 2022 Japanese with no halfwidth Katakana; Japanese (JIS)
            'm_CodePages.Add("50221	csISO2022JP	ISO 2022 Japanese with halfwidth Katakana; Japanese (JIS-Allow 1 byte Kana)
            'm_CodePages.Add("50222	iso-2022-jp	ISO 2022 Japanese JIS X 0201-1989; Japanese (JIS-Allow 1 byte Kana - SO/SI)
            'm_CodePages.Add("50225	iso-2022-kr	ISO 2022 Korean
            'm_CodePages.Add("50227	x-cp50227	ISO 2022 Simplified Chinese; Chinese Simplified (ISO 2022)
            'm_CodePages.Add("50229		ISO 2022 Traditional Chinese
            'm_CodePages.Add("50930		EBCDIC Japanese (Katakana) Extended
            'm_CodePages.Add("50931:  EBCDIC US - Canada And Japanese
            'm_CodePages.Add("50933		EBCDIC Korean Extended and Korean
            'm_CodePages.Add("50935		EBCDIC Simplified Chinese Extended and Simplified Chinese
            'm_CodePages.Add("50936		EBCDIC Simplified Chinese
            'm_CodePages.Add("50937		EBCDIC US-Canada and Traditional Chinese
            'm_CodePages.Add("50939		EBCDIC Japanese (Latin) Extended and Japanese
            'm_CodePages.Add("51932	euc-jp	EUC Japanese
            'm_CodePages.Add("51936	EUC-CN	EUC Simplified Chinese; Chinese Simplified (EUC)
            'm_CodePages.Add("51949	euc-kr	EUC Korean
            'm_CodePages.Add("51950		EUC Traditional Chinese
            'm_CodePages.Add("52936	hz-gb-2312	HZ-GB2312 Simplified Chinese; Chinese Simplified (HZ)
            'm_CodePages.Add("54936	GB18030	Windows XP and later: GB18030 Simplified Chinese (4 byte); Chinese Simplified (GB18030)
            'm_CodePages.Add("57002	x-iscii-de	ISCII Devanagari
            'm_CodePages.Add("57003	x-iscii-be	ISCII Bengali
            'm_CodePages.Add("57004	x-iscii-ta	ISCII Tamil
            'm_CodePages.Add("57005	x-iscii-te	ISCII Telugu
            'm_CodePages.Add("57006	x-iscii-as	ISCII Assamese
            'm_CodePages.Add("57007	x-iscii-or	ISCII Oriya
            'm_CodePages.Add("57008	x-iscii-ka	ISCII Kannada
            'm_CodePages.Add("57009	x-iscii-ma	ISCII Malayalam
            'm_CodePages.Add("57010	x-iscii-gu	ISCII Gujarati
            'm_CodePages.Add("57011	x-iscii-pa	ISCII Punjabi
            'm_CodePages.Add("65000	utf-7	Unicode (UTF-7)
            'm_CodePages.Add("65001	utf-8	Unicode (UTF-8)
        End Sub


        Public Function OnlyChars(ByVal name As String) As String
            Dim ret As New System.Text.StringBuilder(Len(name) + 1)
            For i As Integer = 1 To Len(name)
                Dim ch As String = Mid(name, i, 1)
                Select Case ch
                    Case "à" : ret.Append("a")
                    Case "è" : ret.Append("e")
                    Case "é" : ret.Append("e")
                    Case "ì" : ret.Append("i")
                    Case "ò" : ret.Append("o")
                    Case "ù" : ret.Append("u")
                    Case "À" : ret.Append("A")
                    Case "È" : ret.Append("E")
                    Case "É" : ret.Append("E")
                    Case "Ì" : ret.Append("I")
                    Case "Ò" : ret.Append("O")
                    Case "Ù" : ret.Append("U")
                    Case Else
                        If ((ch >= "A") AndAlso (ch <= "Z")) OrElse ((ch >= "a") AndAlso (ch <= "z")) Then
                            ret.Append(ch)
                        End If
                End Select
            Next
            Return ret.ToString
        End Function


        Public Function OnlyCharsAndNumbers(ByVal name As String) As String
            Dim ret As New System.Text.StringBuilder(Len(name) + 1)
            For i As Integer = 1 To Len(name)
                Dim ch As String = Mid(name, i, 1)
                Select Case ch
                    Case "à" : ret.Append("a")
                    Case "è" : ret.Append("e")
                    Case "é" : ret.Append("e")
                    Case "ì" : ret.Append("i")
                    Case "ò" : ret.Append("o")
                    Case "ù" : ret.Append("u")
                    Case "À" : ret.Append("A")
                    Case "È" : ret.Append("E")
                    Case "É" : ret.Append("E")
                    Case "Ì" : ret.Append("I")
                    Case "Ò" : ret.Append("O")
                    Case "Ù" : ret.Append("U")
                    Case Else
                        If ((ch >= "A") AndAlso (ch <= "Z")) OrElse ((ch >= "a") AndAlso (ch <= "z")) OrElse ((ch >= "0") AndAlso (ch <= "9")) Then
                            ret.Append(ch)
                        End If
                End Select
            Next
            Return ret.ToString
        End Function

        Public Function OnlyCharsWS(ByVal name As String) As String
            Dim ret As New System.Text.StringBuilder(Len(name) + 1)
            Dim oldCh As String = ""
            For i As Integer = 1 To Len(name)
                Dim ch As String = Mid(name, i, 1)
                Select Case ch
                    Case "à" : ret.Append("a")
                    Case "è" : ret.Append("e")
                    Case "é" : ret.Append("e")
                    Case "ì" : ret.Append("i")
                    Case "ò" : ret.Append("o")
                    Case "ù" : ret.Append("u")
                    Case "À" : ret.Append("A")
                    Case "È" : ret.Append("E")
                    Case "É" : ret.Append("E")
                    Case "Ì" : ret.Append("I")
                    Case "Ò" : ret.Append("O")
                    Case "Ù" : ret.Append("U")
                    Case " " : If (oldCh <> "") Then ret.Append(ch)
                    Case Else
                        If ((ch >= "A") AndAlso (ch <= "Z")) OrElse ((ch >= "a") AndAlso (ch <= "z")) Then
                            ret.Append(ch)
                        End If
                End Select
                oldCh = ch
            Next
            Return ret.ToString
        End Function

        Public Function ContaRipetizioni(ByVal testo As String, ByVal subString As String, Optional ByVal compareMethod As CompareMethod = CompareMethod.Text) As Integer
            If (Len(subString) = 0) Then Return 0
            Dim cnt As Integer = 0
            Dim i As Integer = 1
            Dim l As Integer = Len(subString)
            While (i + l <= Len(testo))
                If (Strings.Compare(Mid(testo, i, l), subString, compareMethod) = 0) Then
                    cnt += 1
                    i += l
                Else
                    i += 1
                End If
            End While
            Return cnt
        End Function

        Public Function TrimTo(ByVal str As String, ByVal totalLen As Integer, Optional ByVal trimChars As String = "...") As String
            If (Len(str) > totalLen) Then
                str = Left(str, totalLen - Len(trimChars)) & trimChars
            Else
                str = str
            End If
            Return str
        End Function

        Public Function IsNullOrWhiteSpace(ByVal value As String) As Boolean
            Return (String.IsNullOrEmpty(value)) OrElse Trim(value) = ""
        End Function

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class
End Namespace

Partial Class Sistema

    Public Enum CodePages As Integer
        IBM037 = 37  'IBM EBCDIC US-Canada
        IBM437 = 437  'OEM United States
        IBM500 = 500 'IBM EBCDIC International
        ASMO_708 = 708 'Arabic (ASMO 708)
        Arabic = 709     '(ASMO-449+, BCON V4)
        Arabic_Transparent = 710     ' - Transparent Arabic
        DOS_720 = 720 'Arabic (Transparent ASMO); Arabic (DOS)
        ibm737 = 737 'OEM Greek (formerly 437G); Greek (DOS)
        ibm775 = 775 'OEM Baltic; Baltic (DOS)
        ibm850 = 850 'OEM Multilingual Latin 1; Western European (DOS)
        ibm852 = 852 'OEM Latin 2; Central European (DOS)
        IBM855 = 855 'OEM Cyrillic (primarily Russian)
        ibm857 = 857 'OEM Turkish; Turkish (DOS)
        IBM00858 = 858 'OEM Multilingual Latin 1 + Euro symbol
        IBM860 = 860 'OEM Portuguese; Portuguese (DOS)
        ibm861 = 861 'OEM Icelandic; Icelandic (DOS)
        DOS_862 = 862 'OEM Hebrew; Hebrew (DOS)
        IBM863 = 863 'OEM French Canadian; French Canadian (DOS)
        IBM864 = 864 'OEM Arabic; Arabic (864)
        IBM865 = 865 'OEM Nordic; Nordic (DOS)
        cp866 = 866 'OEM Russian; Cyrillic (DOS)
        ibm869 = 869 'OEM Modern Greek; Greek, Modern (DOS)
        IBM870 = 870 'IBM EBCDIC Multilingual/ROECE (Latin 2); IBM EBCDIC Multilingual Latin 2
        windows_874 = 874 'ANSI/OEM Thai (ISO 8859-11); Thai (Windows)
        cp875 = 875 'IBM EBCDIC Greek Modern
        shift_jis = 932 'ANSI/OEM Japanese; Japanese (Shift-JIS)
        gb2312 = 936 'ANSI/OEM Simplified Chinese (PRC, Singapore); Chinese Simplified (GB2312)
        ks_c_5601_1987 = 949 'ANSI/OEM Korean (Unified Hangul Code)
        big5 = 950 'ANSI/OEM Traditional Chinese (Taiwan; Hong Kong SAR, PRC); Chinese Traditional (Big5)
        IBM1026 = 1026    'IBM EBCDIC Turkish (Latin 5)
        IBM01047 = 1047    'IBM EBCDIC Latin 1/Open System
        IBM01140 = 1140    'IBM EBCDIC US-Canada (037 + Euro symbol); IBM EBCDIC (US-Canada-Euro)
        IBM01141 = 1141    'IBM EBCDIC Germany (20273 + Euro symbol); IBM EBCDIC (Germany-Euro)
        IBM01142 = 1142    'IBM EBCDIC Denmark-Norway (20277 + Euro symbol); IBM EBCDIC (Denmark-Norway-Euro)
        IBM01143 = 1143    'IBM EBCDIC Finland-Sweden (20278 + Euro symbol); IBM EBCDIC (Finland-Sweden-Euro)
        IBM01144 = 1144    'IBM EBCDIC Italy (20280 + Euro symbol); IBM EBCDIC (Italy-Euro)
        IBM01145 = 1145    'IBM EBCDIC Latin America-Spain (20284 + Euro symbol); IBM EBCDIC (Spain-Euro)
        IBM01146 = 1146    'IBM EBCDIC United Kingdom (20285 + Euro symbol); IBM EBCDIC (UK-Euro)
        IBM01147 = 1147    'IBM EBCDIC France (20297 + Euro symbol); IBM EBCDIC (France-Euro)
        IBM01148 = 1148    'IBM EBCDIC International (500 + Euro symbol); IBM EBCDIC (International-Euro)
        IBM01149 = 1149    'IBM EBCDIC Icelandic (20871 + Euro symbol); IBM EBCDIC (Icelandic-Euro)
        utf_16 = 1200     'Unicode UTF-16, little endian byte order (BMP of ISO 10646); available only to managed applications
        unicodeFFFE = 1201    'Unicode UTF-16, big endian byte order; available only to managed applications
        windows_1250 = 1250    'ANSI Central European; Central European (Windows)
        windows_1251 = 1251    'ANSI Cyrillic; Cyrillic (Windows)
        windows_1252 = 1252    'ANSI Latin 1; Western European (Windows)
        windows_1253 = 1253    'ANSI Greek; Greek (Windows)
        windows_1254 = 1254    'ANSI Turkish; Turkish (Windows)
        windows_1255 = 1255    'ANSI Hebrew; Hebrew (Windows)
        windows_1256 = 1256    'ANSI Arabic; Arabic (Windows)
        windows_1257 = 1257    'ANSI Baltic; Baltic (Windows)
        windows_1258 = 1258    'ANSI/OEM Vietnamese; Vietnamese (Windows)
        Johab = 1361    'Korean (Johab)
        macintosh = 10000   'MAC Roman; Western European (Mac)
        x_mac_japanese = 10001   'Japanese (Mac)
        x_mac_chinesetrad = 10002   'MAC Traditional Chinese (Big5); Chinese Traditional (Mac)
        x_mac_korean = 10003   'Korean (Mac)
        x_mac_arabic = 10004   'Arabic (Mac)
        x_mac_hebrew = 10005   'Hebrew (Mac)
        x_mac_greek = 10006   'Greek (Mac)
        x_mac_cyrillic = 10007   'Cyrillic (Mac)
        x_mac_chinesesimp = 10008   'MAC Simplified Chinese (GB 2312); Chinese Simplified (Mac)
        x_mac_romanian = 10010   'Romanian (Mac)
        x_mac_ukrainian = 10017   'Ukrainian (Mac)
        x_mac_thai = 10021   'Thai (Mac)
        x_mac_ce = 10029   'MAC Latin 2; Central European (Mac)
        x_mac_icelandic = 10079   'Icelandic (Mac)
        x_mac_turkish = 10081   'Turkish (Mac)
        x_mac_croatian = 10082   'Croatian (Mac)
        utf_32 = 12000   'Unicode UTF-32, little endian byte order; available only to managed applications
        utf_32BE = 12001   'Unicode UTF-32, big endian byte order; available only to managed applications
        x_Chinese_CNS = 20000   '	CNS Taiwan; Chinese Traditional (CNS)
        x_cp20001 = 20001   'TCA Taiwan
        x_Chinese_Eten = 20002   'Eten Taiwan; Chinese Traditional (Eten)
        x_cp20003 = 20003   'IBM5550 Taiwan
        x_cp20004 = 20004   'TeleText Taiwan
        x_cp20005 = 20005   'Wang Taiwan
        x_IA5 = 20105   'IA5 (IRV International Alphabet No. 5, 7-bit); Western European (IA5)
        x_IA5_German = 20106   'IA5 German (7-bit)
        x_IA5_Swedish = 20107   'IA5 Swedish (7-bit)
        x_IA5_Norwegian = 20108   'IA5 Norwegian (7-bit)
        us_ascii = 20127   'US-ASCII (7-bit)
        x_cp20261 = 20261   'T.61
        x_cp20269 = 20269   'ISO 6937 Non-Spacing Accent
        IBM273 = 20273   'IBM EBCDIC Germany
        IBM277 = 20277   'IBM EBCDIC Denmark-Norway
        IBM278 = 20278   'IBM EBCDIC Finland-Sweden
        IBM280 = 20280   'IBM EBCDIC Italy
        IBM284 = 20284   'IBM EBCDIC Latin America-Spain
        IBM285 = 20285   'IBM EBCDIC United Kingdom
        IBM290 = 20290   'IBM EBCDIC Japanese Katakana Extended
        IBM297 = 20297   'IBM EBCDIC France
        IBM420 = 20420   'IBM EBCDIC Arabic
        IBM423 = 20423   'IBM EBCDIC Greek
        IBM424 = 20424   'IBM EBCDIC Hebrew
        x_EBCDIC_KoreanExtended = 20833   'IBM EBCDIC Korean Extended
        IBM_Thai = 20838   'IBM EBCDIC Thai
        koi8_r = 20866   'Russian (KOI8-R); Cyrillic (KOI8-R)
        IBM871 = 20871   'IBM EBCDIC Icelandic
        IBM880 = 20880   'IBM EBCDIC Cyrillic Russian
        IBM905 = 20905   'IBM EBCDIC Turkish
        IBM00924 = 20924   'IBM EBCDIC Latin 1/Open System (1047 + Euro symbol)
        EUC_JP = 20932   'Japanese (JIS 0208-1990 And 0212-1990)
        x_cp20936 = 20936   'Simplified Chinese (GB2312); Chinese Simplified (GB2312-80)
        x_cp20949 = 20949   'Korean Wansung
        cp1025 = 21025   'IBM EBCDIC Cyrillic Serbian-Bulgarian
        '21027       (deprecated)
        koi8_u = 21866   '	Ukrainian (KOI8-U); Cyrillic (KOI8-U)
        iso_8859_1 = 28591   'ISO 8859-1 Latin 1; Western European (ISO)
        iso_8859_2 = 28592   'ISO 8859-2 Central European; Central European (ISO)
        iso_8859_3 = 28593   'ISO 8859-3 Latin 3
        iso_8859_4 = 28594   'ISO 8859-4 Baltic
        iso_8859_5 = 28595   'ISO 8859-5 Cyrillic
        iso_8859_6 = 28596   'ISO 8859-6 Arabic
        iso_8859_7 = 28597   'ISO 8859-7 Greek
        iso_8859_8 = 28598   'ISO 8859-8 Hebrew; Hebrew (ISO-Visual)
        iso_8859_9 = 28599   'ISO 8859-9 Turkish
        iso_8859_13 = 28603   'ISO 8859-13 Estonian
        iso_8859_15 = 28605   'ISO 8859-15 Latin 9
        x_Europa = 29001   'Europa 3
        iso_8859_8_i = 38598   'ISO 8859-8 Hebrew; Hebrew (ISO-Logical)
        iso_2022_jp = 50220   'ISO 2022 Japanese with no halfwidth Katakana; Japanese (JIS)
        csISO2022JP = 50221   'ISO 2022 Japanese with halfwidth Katakana; Japanese (JIS-Allow 1 byte Kana)
        iso_2022_jp1 = 50222   'ISO 2022 Japanese JIS X 0201-1989; Japanese (JIS-Allow 1 byte Kana - SO/SI)
        iso_2022_kr = 50225   'ISO 2022 Korean
        x_cp50227 = 50227   'ISO 2022 Simplified Chinese; Chinese Simplified (ISO 2022)
        ISO_2022 = 50229       'Traditional Chinese
        EBCDIC_Japanese_Katakana_Extended = 50930
        EBCDIC_US_Canada_And_Japanese = 50931
        EBCDIC_Korean_Extended_And_Korean = 50933
        EBCDIC_Simplified_Chinese_Extended_And_Simplified_Chinese = 50935
        EBCDIC_Simplified_Chinese = 50936
        EBCDIC_US_Canada_And_Traditional_Chinese = 50937
        EBCDIC_Japanese_Latin_Extended_And_Japanese = 50939
        euc_jp1 = 51932   'EUC Japanese
        EUC_CN = 51936   'EUC Simplified Chinese; Chinese Simplified (EUC)
        euc_kr = 51949   'EUC Korean
        EUC_Traditional_Chinese = 51950
        hz_gb_2312 = 52936   'HZ-GB2312 Simplified Chinese; Chinese Simplified (HZ)
        GB18030 = 54936   'Windows XP And later: GB18030 Simplified Chinese (4 Byte); Chinese Simplified (GB18030)
        x_iscii_de = 57002   'ISCII Devanagari
        x_iscii_be = 57003   'ISCII Bangla
        x_iscii_ta = 57004   'ISCII Tamil
        x_iscii_te = 57005   'ISCII Telugu
        x_iscii_as = 57006   'ISCII Assamese
        x_iscii_Or = 57007   'ISCII Odia
        x_iscii_ka = 57008   'ISCII Kannada
        x_iscii_ma = 57009   'ISCII Malayalam
        x_iscii_gu = 57010   'ISCII Gujarati
        x_iscii_pa = 57011   'ISCII Punjabi
        utf_7 = 65000   'Unicode (UTF-7)
        utf_8 = 65001   'Unicode (UTF-8)
    End Enum


    Private Shared m_Strings As CStringsClass = Nothing

    Public Shared ReadOnly Property Strings As CStringsClass
        Get
            If (m_Strings Is Nothing) Then m_Strings = New CStringsClass
            Return m_Strings
        End Get
    End Property

End Class
'/*
'  Copyright 2011 - 2012 Stefano Chizzolini. http: //www.dmdpdf.org

'  Contributors:
'    * Stefano Chizzolini (original code developer, http//www.stefanochizzolini.it)

'  This file should be part Of the source code distribution Of "PDF Clown library" (the
'  Program): see the accompanying README files For more info.

'  This Program Is free software; you can redistribute it And/Or modify it under the terms
'  of the GNU Lesser General Public License as published by the Free Software Foundation;
'  either version 3 Of the License, Or (at your Option) any later version.

'  This Program Is distributed In the hope that it will be useful, but WITHOUT ANY WARRANTY,
'  either expressed Or implied; without even the implied warranty Of MERCHANTABILITY Or
'  FITNESS FOR A PARTICULAR PURPOSE. See the License for more details.

'  You should have received a copy Of the GNU Lesser General Public License along With Me
'  Program(see README files); If Not, go To the GNU website (http://www.gnu.org/licenses/).

'  Redistribution And use, with Or without modification, are permitted provided that such
'  redistributions retain the above copyright notice, license And disclaimer, along With
'  Me list Of conditions.
'*/

Imports DMD.org.dmdpdf.bytes
Imports DMD.org.dmdpdf.tokens

Imports System
Imports System.Globalization
Imports System.Text

Namespace DMD.org.dmdpdf.util.parsers

    '/**
    '  <summary> PostScript(non - procedural subset) parser [PS].</summary>
    '*/
    Public Class PostScriptParser
        Implements IDisposable

        Private Const c0 As Integer = Asc("0"c)
        Private Const c1 As Integer = Asc("1"c)
        Private Const c2 As Integer = Asc("2"c)
        Private Const c3 As Integer = Asc("3"c)
        Private Const c4 As Integer = Asc("4"c)
        Private Const c5 As Integer = Asc("5"c)
        Private Const c6 As Integer = Asc("6"c)
        Private Const c7 As Integer = Asc("7"c)
        Private Const c8 As Integer = Asc("8"c)
        Private Const c9 As Integer = Asc("9"c)
        Private Const cDOT As Integer = Asc("."c)
        Private Const cPLUS As Integer = Asc("+"c)
        Private Const cMINUS As Integer = Asc("-"c)
        Private Const cA As Integer = Asc("A"c)
        Private Const cF As Integer = Asc("F"c)
        Private Const ca_ As Integer = Asc("a"c)
        Private Const cf_ As Integer = Asc("f"c)

#Region "types"

        Public Enum TokenTypeEnum ' [PS:3.3].
            Keyword
            [Boolean]
            [Integer]
            Real
            Literal
            Hex
            Name
            Comment
            ArrayBegin
            ArrayEnd
            DictionaryBegin
            DictionaryEnd
            Null
        End Enum
#End Region

#Region "Shared"
#Region "fields"
        Private Shared ReadOnly _StandardNumberFormatInfo As NumberFormatInfo = NumberFormatInfo.InvariantInfo
#End Region

#Region "Interface"
#Region "Private"
        Private Shared Function GetHex(ByVal c As Integer) As Integer


            If (c >= c0 AndAlso c <= c9) Then
                Return (c - c0)
            ElseIf (c >= cA AndAlso c <= cF) Then
                Return (c - cA + 10)
            ElseIf (c >= ca_ AndAlso c <= cf_) Then
                Return (c - ca_ + 10)
            Else
                Return -1
            End If
        End Function

        '/**
        '  <summary> Evaluate whether a character Is a delimiter.</summary>
        '*/
        Private Shared Function IsDelimiter(ByVal c As Integer) As Boolean
            Return IsDelimiter(ChrW(c))
        End Function

        Private Shared Function IsDelimiter(ByVal c As Char) As Boolean
            Return c = Symbol.OpenRoundBracket OrElse
                   c = Symbol.CloseRoundBracket OrElse
                   c = Symbol.OpenAngleBracket OrElse
                   c = Symbol.CloseAngleBracket OrElse
                   c = Symbol.OpenSquareBracket OrElse
                   c = Symbol.CloseSquareBracket OrElse
                   c = Symbol.Slash OrElse
                   c = Symbol.Percent
        End Function

        '/**
        '  <summary> Evaluate whether a character Is an EOL marker.</summary>
        '*/
        Private Shared Function IsEOL(ByVal c As Integer) As Boolean
            Return (c = 10 OrElse c = 13)
        End Function

        Private Shared Function IsEOL(ByVal c As Char) As Boolean
            Return IsEOL(AscW(c))
        End Function


        '/**
        '  <summary> Evaluate whether a character Is a white-space.</summary>
        '*/
        Private Shared Function IsWhitespace(ByVal c As Integer) As Boolean
            Return c = 32 OrElse IsEOL(c) OrElse c = 0 OrElse c = 9 OrElse c = 12
        End Function

        Private Shared Function IsWhitespace(ByVal c As Char) As Boolean
            Return IsWhitespace(AscW(c))
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "fields"
        Private _stream As IInputStream

        Private _token As Object
        Private _tokenType As TokenTypeEnum

#End Region

#Region "constructors"
        Public Sub New(ByVal stream As IInputStream)
            Me._stream = stream
        End Sub

        Public Sub New(ByVal data As Byte())
            Me._stream = New DMD.org.dmdpdf.bytes.Buffer(data)
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public Overrides Function GetHashCode() As Integer
            Return Me._stream.GetHashCode()
        End Function

        '/**
        '  <summary> Gets a token after moving To the given offset.</summary>
        '  <param name = "offset" > Number Of tokens To skip before reaching the intended one.</param>
        '  <seealso cref = "Token" />
        '*/
        Public Function GetToken(ByVal offset As Integer) As Object
            MoveNext(offset)
            Return Token
        End Function

        Public ReadOnly Property Length As Long
            Get
                Return Me._stream.Length
            End Get
        End Property

        '/**
        '  <summary> Moves the pointer To the Next token.</summary>
        '  <param name = "offset" > Number Of tokens To skip before reaching the intended one.</param>
        '*/
        Public Function MoveNext(ByVal offset As Integer) As Boolean
            For index As Integer = 0 To offset - 1
                If (Not MoveNext()) Then Return False
            Next
            Return True
        End Function


        '/**
        '  <summary> Moves the pointer To the Next token.</summary>
        '  <remarks>To properly parse the current token, the pointer MUST be just before its starting
        '  (leading whitespaces are ignored). When Me method terminates, the pointer Is
        '  at the last Byte Of the current token.</remarks>
        '  <returns> Whether a New token was found.</returns>
        '*/
        Public Overridable Function MoveNext() As Boolean
            Dim buffer As StringBuilder = Nothing
            _token = Nothing
            Dim c As Integer = 0

            ' Skip leading white-space characters.
            Do
                c = _stream.ReadByte()
                If (c = -1) Then Return False
            Loop While (IsWhitespace(c)) ' Keep goin' till there's a white-space character...

            ' Which character Is it?
            Select Case (c)
                Case AscW(Symbol.Slash)      ' Name.
                    ' {
                    _tokenType = TokenTypeEnum.Name

                    '/*
                    '  NOTE: As name objects are simple symbols uniquely defined by sequences of characters,
                    '  the bytes making up the name are never treated As text, so here they are just
                    '  passed through without unescaping.
                    '*/
                    buffer = New StringBuilder()
                    While (True)
                        c = _stream.ReadByte()
                        If (c = -1) Then Exit While ' break; // NOOP.
                        If (IsDelimiter(c) OrElse IsWhitespace(c)) Then Exit While ' break;
                        buffer.Append(ChrW(c))
                    End While
                    If (c > -1) Then '// Restores the first byte after the current token.
                        _stream.Skip(-1)
                    End If
                    '}
                    'break;
                Case c0, c1, c2, c3, c4, c5, c6, c7, c8, c9, cDOT, cMINUS, cPLUS ' Number.
                    '{
                    If (c = cDOT) Then
                        _tokenType = TokenTypeEnum.Real ' Digit or signum.
                    Else
                        _tokenType = TokenTypeEnum.Integer ' By Default (it may be real).
                    End If

                    ' Building the number...
                    buffer = New StringBuilder()
                    While (True)
                        buffer.Append(ChrW(c))
                        c = _stream.ReadByte()
                        If (c = -1) Then
                            Exit While ' break; // NOOP.
                        ElseIf (c = cdot) Then
                            _tokenType = TokenTypeEnum.Real
                        ElseIf (c < c0 OrElse c > c9) Then
                            Exit While 'break;
                        End If
                    End While
                    If (c > -1) Then
                        _stream.Skip(-1) ' Restores the first byte after the current token.
                    End If
                    '}
                    'break;
                Case AscW(Symbol.OpenSquareBracket)        ' Array (begin).
                    _tokenType = TokenTypeEnum.ArrayBegin
                    'break;
                Case AscW(Symbol.CloseSquareBracket)       ' Array (end).
                    _tokenType = TokenTypeEnum.ArrayEnd
                    'break;
                Case AscW(Symbol.OpenAngleBracket)       ' Dictionary (begin) | Hexadecimal string.
                    '{
                    c = _stream.ReadByte()
                    If (c = -1) Then Throw New ParseException("Unexpected EOF (isolated opening angle-bracket character).")
                    ' Is it a dictionary (2nd angle bracket)?
                    If (c = AscW(Symbol.OpenAngleBracket)) Then
                        _tokenType = TokenTypeEnum.DictionaryBegin
                        GoTo endsw1 ' break;
                    End If

                    ' Hexadecimal string (single angle bracket).
                    _tokenType = TokenTypeEnum.Hex

                    buffer = New StringBuilder()
                    While (c <> AscW(Symbol.CloseAngleBracket)) ' Not String End.
                        If (Not IsWhitespace(c)) Then
                            buffer.Append(ChrW(c))
                        End If

                        c = _stream.ReadByte()
                        If (c = -1) Then Throw New ParseException("Unexpected EOF (malformed hex string).")
                    End While
                    '}
                    'break;
                Case AscW(Symbol.CloseAngleBracket) ' :       // Dictionary (end).
                    '{
                    c = _stream.ReadByte()
                    If (c <> AscW(Symbol.CloseAngleBracket)) Then Throw New ParseException("Malformed dictionary.", _stream.Position)

                    _tokenType = TokenTypeEnum.DictionaryEnd
                    '}
                    'break;
                Case AscW(Symbol.OpenRoundBracket) ' :       // Literal string.
                    '{
                    _tokenType = TokenTypeEnum.Literal

                    buffer = New StringBuilder()
                    Dim level As Integer = 0
                    While (True)
                        c = _stream.ReadByte()
                        If (c = -1) Then
                            Exit While 'break;
                        ElseIf (c = ascw(Symbol.OpenRoundBracket)) Then
                            level += 1
                        ElseIf (c = ascw(Symbol.CloseRoundBracket)) Then
                            level -= 1
                        ElseIf (c = AscW("\"c)) Then
                            Dim lineBreak As Boolean = False
                            c = _stream.ReadByte()
                            Select Case (c)
                                Case AscW("n"c)
                                    c = AscW(Symbol.LineFeed)
                                    'break;
                                Case AscW("r"c)
                                    c = AscW(Symbol.CarriageReturn)
                                    'break;
                                Case AscW("t"c)
                                    c = AscW(ControlChars.Tab) ''\t';
                                    'break;
                                Case AscW("b"c)
                                    c = AscW(ControlChars.Back) ' '\b';
                                    'break;
                                Case AscW("f"c)
                                    c = AscW(ControlChars.FormFeed) ' '\f';
                                    'break;
                                Case AscW(Symbol.OpenRoundBracket), AscW(Symbol.CloseRoundBracket), AscW("\"c)
                                    'break;
                                Case AscW(Symbol.CarriageReturn)
                                    lineBreak = True
                                    c = _stream.ReadByte()
                                    If (c <> Asc(Symbol.LineFeed)) Then _stream.Skip(-1)
                                    'break;
                                Case AscW(Symbol.LineFeed)
                                    lineBreak = True
                                    'break;
                                Case Else
                                    ' Is it outside the octal encoding?
                                    If (c < c0 OrElse c > c7) Then GoTo endsw2 ' break;

                                    ' Octal.
                                    Dim octal As Integer = c - c0
                                    c = _stream.ReadByte()
                                    ' Octal end?
                                    If (c < c0 OrElse c > c7) Then
                                        c = octal
                                        _stream.Skip(-1)
                                        GoTo endsw2 'break;
                                    End If
                                    octal = (octal << 3) + c - c0
                                    c = _stream.ReadByte()
                                    ' Octal end?
                                    If (c < c0 OrElse c > c7) Then
                                        c = octal
                                        _stream.Skip(-1)
                                        GoTo endsw2 'break; 
                                    End If
                                    octal = (octal << 3) + c - c0
                                    c = octal And &HFF
                                    'break;
                                    '}
                            End Select
endsw2:
                            If (lineBreak) Then Continue While
                            If (c = -1) Then Exit While ' break;
                        ElseIf (c = ascw(Symbol.CarriageReturn)) Then
                            c = _stream.ReadByte()
                            If (c = -1) Then
                                Exit While ' break;
                            ElseIf (c <> ascw(Symbol.LineFeed)) Then
                                c = AscW(Symbol.LineFeed)
                                _stream.Skip(-1)
                            End If
                        End If
                        If (level = -1) Then Exit While ' break;

                        buffer.Append(ChrW(c))
                    End While
                    If (c = -1) Then Throw New ParseException("Malformed literal string.")
                    '}
                    'break;
                Case AscW(Symbol.Percent) ' Comment.
                    '{
                    _tokenType = TokenTypeEnum.Comment

                    buffer = New StringBuilder()
                    While (True)
                        c = _stream.ReadByte()
                        If (c = -1 OrElse IsEOL(c)) Then Exit While ' break;

                        buffer.Append(ChrW(c))

                    End While
                    'break;
                Case Else ' Keyword.
                    '{
                    _tokenType = TokenTypeEnum.Keyword

                    buffer = New StringBuilder()
                    Do
                        buffer.Append(ChrW(c))
                        c = _stream.ReadByte()
                        If (c = -1) Then Exit Do ' break;
                    Loop While (Not IsDelimiter(c) AndAlso Not IsWhitespace(c))
                    If (c > -1) Then
                        _stream.Skip(-1) ' Restores the first Byte after the current token.
                    End If
                    '}
                    'break;
            End Select

endsw1:
            If (buffer IsNot Nothing) Then
                Select Case (_tokenType)
                    Case TokenTypeEnum.Keyword
                        '{
                        _token = buffer.ToString()
                        Select Case (CStr(_token))
                            Case Keyword.False, Keyword.True        ' Boolean.
                                _tokenType = TokenTypeEnum.Boolean
                                _token = Boolean.Parse(CStr(_token))
                                'break;
                            Case Keyword.Null ' Null.
                                _tokenType = TokenTypeEnum.Null
                                _token = Nothing
                                'break;
                        End Select
                        '}
                        'break;
                    Case TokenTypeEnum.Name, TokenTypeEnum.Literal, TokenTypeEnum.Hex, TokenTypeEnum.Comment
                        _token = buffer.ToString()
                        'break;
                    Case TokenTypeEnum.Integer
                        _token = Int32.Parse(buffer.ToString(), NumberStyles.Integer, _StandardNumberFormatInfo)
                        'break;
                    Case TokenTypeEnum.Real
                        _token = Double.Parse(buffer.ToString(), NumberStyles.Float, _StandardNumberFormatInfo)
                        'break;
                End Select
            End If
            Return True
        End Function

        Public ReadOnly Property Position As Long
            Get
                Return _stream.Position
            End Get
        End Property

        '/**
        '  <summary> Moves the pointer To the given absolute Byte position.</summary>
        '*/
        Public Sub Seek(ByVal offset As Long)
            _stream.Seek(offset)
        End Sub

        '/**
        '  <summary> Moves the pointer To the given relative Byte position.</summary>
        '*/
        Public Sub Skip(ByVal offset As Long)
            _stream.Skip(offset)
        End Sub

        '/**
        '  <summary> Moves the pointer before the Next non-EOL character after the current position.</summary>
        '  <returns> Whether the stream can be further read.</returns>
        '*/
        Public Function SkipEOL() As Boolean
            Dim c As Integer
            Do
                c = _stream.ReadByte()
                If (c = -1) Then Return False
            Loop While (IsEOL(c)) ' Keeps going till there's an EOL character.
            _stream.Skip(-1) ' Moves back To the first non-EOL character position.
            Return True
        End Function

        '/**
        '  <summary> Moves the pointer before the Next non-whitespace character after the current position.</summary>
        '  <returns> Whether the stream can be further read.</returns>
        '*/
        Public Function SkipWhitespace() As Boolean
            Dim c As Integer
            Do
                c = _stream.ReadByte()
                If (c = -1) Then Return False
            Loop While (IsWhitespace(c)) ' Keeps going till there's a whitespace character.
            _stream.Skip(-1) ' Moves back To the first non-whitespace character position.
            Return True
        End Function

        Public ReadOnly Property Stream As IInputStream
            Get
                Return _stream
            End Get
        End Property

        '/**
        '  <summary> Gets the currently-parsed token.</summary>
        '*/
        Public Property Token As Object
            Get
                Return _token
            End Get
            Protected Set(ByVal value As Object)
                _token = value
            End Set
        End Property

        '/**
        '  <summary> Gets the currently-parsed token type.</summary>
        '*/
        Public Property TokenType As TokenTypeEnum
            Get
                Return _tokenType
            End Get
            Protected Set(ByVal value As TokenTypeEnum)
                _tokenType = value
            End Set
        End Property

#Region "IDisposable"

        Public Sub Dispose() Implements IDisposable.Dispose
            If (_stream IsNot Nothing) Then
                _stream.Dispose()
                _stream = Nothing
            End If

            '            GC.SuppressFinalize(Me);
        End Sub

#End Region
#End Region
#End Region
#End Region
    End Class

End Namespace


''/*
''  Copyright 2011-2012 Stefano Chizzolini. http://www.dmdpdf.org

''  Contributors:
''    * Stefano Chizzolini (original code developer, http://www.stefanochizzolini.it)

''  This file should be part of the source code distribution of "PDF Clown library" (the
''  Program): see the accompanying README files for more info.

''  This Program is free software; you can redistribute it and/or modify it under the terms
''  of the GNU Lesser General Public License as published by the Free Software Foundation;
''  either version 3 of the License, or (at your option) any later version.

''  This Program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY,
''  either expressed or implied; without even the implied warranty of MERCHANTABILITY or
''  FITNESS FOR A PARTICULAR PURPOSE. See the License for more details.

''  You should have received a copy of the GNU Lesser General Public License along with Me
''  Program (see README files); if not, go to the GNU website (http://www.gnu.org/licenses/).

''  Redistribution and use, with or without modification, are permitted provided that such
''  redistributions retain the above copyright notice, license and disclaimer, along with
''  Me list of conditions.
''*/

'Imports DMD.org.dmdpdf.bytes
'Imports DMD.org.dmdpdf.tokens

'Imports System
'Imports System.Globalization
'Imports System.Text

'Namespace DMD.org.dmdpdf.util.parsers

'    '/**
'    '  <summary>PostScript (non-procedural subset) parser [PS].</summary>
'    '*/
'    Public Class PostScriptParser
'        Implements IDisposable

'#Region "types"

'        Public Enum TokenTypeEnum As Integer ' [PS:3.3].
'            Keyword
'            [Boolean]
'            [Integer]
'            Real
'            Literal
'            Hex
'            Name
'            Comment
'            ArrayBegin
'            ArrayEnd
'            DictionaryBegin
'            DictionaryEnd
'            Null
'        End Enum

'#End Region

'#Region "Shared"
'#Region "fields"

'        Private Shared ReadOnly StandardNumberFormatInfo As NumberFormatInfo = NumberFormatInfo.InvariantInfo

'#End Region

'#Region "Interface"
'#Region "Private"

'        Private Shared Function GetHex(ByVal c As Integer) As Integer
'            If (c >= Asc("0"c) AndAlso c <= Asc("9"c)) Then
'                Return (c - Asc("0"c))
'            ElseIf (c >= Asc("A"c) AndAlso c <= Asc("F"c)) Then
'                Return (c - Asc("A"c) + 10)
'            ElseIf (c >= Asc("a"c) AndAlso c <= Asc("f"c)) Then
'                Return (c - Asc("a"c) + 10)
'            Else
'                Return -1
'            End If
'        End Function

'        '/**
'        '  <summary>Evaluate whether a character is a delimiter.</summary>
'        '*/
'        Private Shared Function IsDelimiter(ByVal c As Integer) As Boolean
'            Return c = Asc(Symbol.OpenRoundBracket) OrElse
'                   c = Asc(Symbol.CloseRoundBracket) OrElse
'                   c = Asc(Symbol.OpenAngleBracket) OrElse
'                   c = Asc(Symbol.CloseAngleBracket) OrElse
'                   c = Asc(Symbol.OpenSquareBracket) OrElse
'                   c = Asc(Symbol.CloseSquareBracket) OrElse
'                   c = Asc(Symbol.Slash) OrElse
'                   c = Asc(Symbol.Percent)
'        End Function

'        '/**
'        '  <summary>Evaluate whether a character is an EOL marker.</summary>
'        '*/
'        Private Shared Function IsEOL(ByVal c As Integer) As Boolean
'            Return (c = 10 OrElse c = 13)
'        End Function

'        '/**
'        '  <summary>Evaluate whether a character is a white-space.</summary>
'        '*/
'        Private Shared Function IsWhitespace(ByVal c As Integer) As Boolean
'            Return c = 32 OrElse IsEOL(c) OrElse c = 0 OrElse c = 9 OrElse c = 12
'        End Function

'#End Region
'#End Region
'#End Region

'#Region "dynamic"
'#Region "fields"
'        Private _stream As IInputStream

'        Private _token As Object
'        Private _tokenType As TokenTypeEnum
'#End Region

'#Region "constructors"

'        Public Sub New(ByVal _stream As IInputStream)
'            Me._stream = _stream
'        End Sub

'        Public Sub New(ByVal data As Byte())
'            Me._stream = New org.dmdpdf.bytes.Buffer(data)
'        End Sub

'#End Region

'#Region "Interface"
'#Region "Public"

'        Public Overrides Function GetHashCode() As Integer
'            Return Me._stream.GetHashCode()
'        End Function

'        '/**
'        '  <summary>Gets a _token after moving to the given offset.</summary>
'        '  <param name="offset">Number of tokens to skip before reaching the intended one.</param>
'        '  <seealso cref="Token"/>
'        '*/
'        Public Function GetToken(ByVal offset As Integer) As Object
'            MoveNext(offset)
'            Return _token
'        End Function

'        Public ReadOnly Property Length As Long
'            Get
'                Return Me._stream.Length
'            End Get
'        End Property

'        '/**
'        '  <summary>Moves the pointer to the next _token.</summary>
'        '  <param name="offset">Number of tokens to skip before reaching the intended one.</param>
'        '*/
'        Public Function MoveNext(ByVal offset As Integer) As Boolean
'            'for(
'            '  Integer index = 0;
'            '  index < offset;
'            '  index+ +
'            '  )
'            For index As Integer = 0 To offset - 1       '  index+ +
'                If (Not MoveNext()) Then Return False
'            Next
'            Return True
'        End Function

'        '/**
'        '  <summary>Moves the pointer to the next _token.</summary>
'        '  <remarks>To properly parse the current _token, the pointer MUST be just before its starting
'        '  (leading whitespaces are ignored). When Me method terminates, the pointer IS
'        '  at the last byte of the current _token.</remarks>
'        '  <returns>Whether a new _token was found.</returns>
'        '*/
'        Public Overridable Function MoveNext() As Boolean
'            Dim buffer As StringBuilder = Nothing
'            Me._token = Nothing
'            Dim c As Integer = 0

'            ' Skip leading white-space characters.
'            Do
'                c = _stream.ReadByte()
'                If (c = -1) Then Return False
'            Loop While (IsWhitespace(c)) 'Keep goin' till there's a white-space character...

'            ' Which character is it?
'            Select Case (c)
'                Case Asc(Symbol.Slash) ' Name.
'                    _tokenType = TokenTypeEnum.Name

'                    '/*
'                    '  NOTE: As name objects are simple symbols uniquely defined by sequences of characters,
'                    '  the bytes making up the name are never treated as text, so here they are just
'                    '  passed through without unescaping.
'                    '*/
'                    buffer = New StringBuilder()
'                    While (True)
'                        c = _stream.ReadByte()
'                        If (c = -1) Then Exit While ' NOOP.
'                        If (IsDelimiter(c) OrElse IsWhitespace(c)) Then Exit While
'                        buffer.Append(Chr(c))
'                    End While
'                    If (c > -1) Then
'                        _stream.Skip(-1)
'                        ' Restores the first byte after the current _token.
'                    End If
'                    ' break;
'                Case Asc("0"c), Asc("1"c), Asc("2"c), Asc("3"c), Asc("4"c), Asc("5"c), Asc("6"c), Asc("7"c), Asc("8"c), Asc("9"c), Asc("."c), Asc("-"c), Asc("+"c) ' Number.
'                    If (c = Asc("."c)) Then
'                        _tokenType = TokenTypeEnum.Real
'                    Else ' Digit or signum.
'                        _tokenType = TokenTypeEnum.Integer 'By default (it may be real).
'                    End If
'                    ' Building the number...
'                    buffer = New StringBuilder()
'                    While (True)
'                        buffer.Append(Chr(c))
'                        c = _stream.ReadByte()
'                        If (c = -1) Then
'                            Exit While ' NOOP.
'                        ElseIf (c = Asc("."c)) Then
'                            _tokenType = TokenTypeEnum.Real
'                        ElseIf (c < Asc("0"c) OrElse c > Asc("9"c)) Then
'                            Exit While
'                        End If
'                    End While
'                    If (c > -1) Then
'                        _stream.Skip(-1)
'                        ' Restores the first byte after the current _token.
'                    End If
'                ' break;
'                Case Asc(Symbol.OpenSquareBracket) ' Array (begin).
'                    _tokenType = TokenTypeEnum.ArrayBegin
'                    'break;
'                Case Asc(Symbol.CloseSquareBracket) ' Array (end).
'                    _tokenType = TokenTypeEnum.ArrayEnd
'                    'break;
'                Case Asc(Symbol.OpenAngleBracket) ' Dictionary (begin) | Hexadecimal string.
'                    c = _stream.ReadByte()
'                    If (c = -1) Then Throw New ParseException("Unexpected EOF (isolated opening angle-bracket character).")
'                    ' Is it a dictionary (2nd angle bracket)?
'                    If (c = Asc(Symbol.OpenAngleBracket)) Then
'                        _tokenType = TokenTypeEnum.DictionaryBegin

'                    Else
'                        ' Hexadecimal string (single angle bracket).
'                        _tokenType = TokenTypeEnum.Hex

'                        buffer = New StringBuilder()
'                        While (c <> Asc(Symbol.CloseAngleBracket)) ' Not String End.
'                            If (Not IsWhitespace(c)) Then
'                                buffer.Append(Chr(c))
'                            End If

'                            c = _stream.ReadByte()
'                            If (c = -1) Then Throw New ParseException("Unexpected EOF (malformed hex string).")
'                        End While
'                    End If

'          'break;
'                Case Asc(Symbol.CloseAngleBracket) ' Dictionary (end).
'                    c = _stream.ReadByte()
'                    If (c <> Asc(Symbol.CloseAngleBracket)) Then Throw New ParseException("Malformed dictionary.", _stream.Position)

'                    _tokenType = TokenTypeEnum.DictionaryEnd
'        'break;
'                Case Asc(Symbol.OpenRoundBracket) ' Literal string.
'                    _tokenType = TokenTypeEnum.Literal
'                    buffer = New StringBuilder()
'                    Dim level As Integer = 0
'                    While (True)
'                        c = _stream.ReadByte()
'                        If (c = -1) Then
'                            Exit While 'break;
'                        ElseIf (c = asc(Symbol.OpenRoundBracket)) Then
'                            level += 1
'                        ElseIf (c = asc(Symbol.CloseRoundBracket)) Then
'                            level -= 1
'                        ElseIf (c = Asc("\"c)) Then ' "\\"
'                            Dim lineBreak As Boolean = False
'                            c = _stream.ReadByte()
'                            Select Case (c)
'                                Case Asc("n"c) : c = Asc(Symbol.LineFeed) 'break;
'                                Case Asc("r"c) : c = Asc(Symbol.CarriageReturn) 'break;
'                                Case Asc("t"c) : c = Asc(ControlChars.Tab) ''\t'; 'break;
'                                Case Asc("b"c) : c = Asc(ControlChars.Back)  '\b'; 'break;
'                                Case Asc("f"c) : c = Asc(ControlChars.FormFeed) ' '\f';
'                                Case Asc(Symbol.OpenRoundBracket),
'                                     Asc(Symbol.CloseRoundBracket)
'                                    Asc("\"c) '\\':
'                                    'break;
'                                Case Asc(Symbol.CarriageReturn)
'                                    lineBreak = True
'                                    c = _stream.ReadByte()
'                                    If (c <> Asc(Symbol.LineFeed)) Then _stream.Skip(-1)
'                                    'break;
'                                Case Asc(Symbol.LineFeed)
'                                    lineBreak = True
'                                    'break;
'                                Case Else
'                                    ' Is it outside the octal encoding?
'                                    If (c < Asc("0"c) OrElse c > Asc("7"c)) Then Exit While ' break;
'                                    ' Octal.
'                                    Dim octal As Integer = c - Asc("0"c)
'                                    c = _stream.ReadByte()
'                                    ' Octal end?
'                                    If (c < Asc("0"c) OrElse c > Asc("7"c)) Then
'                                        c = octal
'                                        _stream.Skip(-1)
'                                        Exit While
'                                    End If
'                                    octal = (octal << 3) + c - Asc("0"c)
'                                    c = _stream.ReadByte()
'                                    ' Octal end?
'                                    If (c < Asc("0"c) OrElse c > Asc("7"c)) Then
'                                        c = octal
'                                        _stream.Skip(-1)
'                                        Exit While 'break;
'                                    End If
'                                    octal = (octal << 3) + c - Asc("0"c)
'                                    c = octal And &HFF
'                                    'break;
'                            End Select
'                            If (lineBreak) Then Continue While
'                            If (c = -1) Then Exit While
'                        ElseIf (c = asc(Symbol.CarriageReturn)) Then
'                            c = _stream.ReadByte()
'                            If (c = -1) Then
'                                Exit While ' break;
'                            ElseIf (c <> asc(Symbol.LineFeed)) Then
'                                c = Asc(Symbol.LineFeed)
'                                _stream.Skip(-1)
'                            End If
'                        End If
'                        If (level = -1) Then Exit While
'                        buffer.Append(Chr(c))
'                    End While
'                    If (c = -1) Then Throw New ParseException("Malformed literal string.")
'                    'break;
'                Case Asc(Symbol.Percent) ' Comment.
'                    _tokenType = TokenTypeEnum.Comment
'                    buffer = New StringBuilder()
'                    While (True)
'                        c = _stream.ReadByte()
'                        If (c = -1 OrElse IsEOL(c)) Then Exit While 'break;
'                        buffer.Append(Chr(c))
'                    End While
'                    ' break;
'                Case Else ' Keyword.
'                    _tokenType = TokenTypeEnum.Keyword
'                    buffer = New StringBuilder()
'                    Do
'                        buffer.Append(Chr(c))
'                        c = _stream.ReadByte()
'                        If (c = -1) Then Exit Do ' break;
'                    Loop While (Not IsDelimiter(c) AndAlso Not IsWhitespace(c))
'                    If (c > -1) Then
'                        ' Restores the first byte after the current _token.
'                        _stream.Skip(-1)
'                    End If
'                    ' break;
'            End Select

'            If (buffer IsNot Nothing) Then
'                Select Case (_tokenType)
'                    Case TokenTypeEnum.Keyword
'                        _token = buffer.ToString()
'                        Select Case (CStr(_token))
'                            Case Keyword.False, Keyword.True    ' Boolean.
'                                _tokenType = TokenTypeEnum.Boolean
'                                _token = Boolean.Parse(CStr(_token))
'                                'break;
'                            Case Keyword.Null    '  Null.
'                                _tokenType = TokenTypeEnum.Null
'                                _token = Nothing
'                                'break;
'                        End Select
'                    ' break
'                    Case TokenTypeEnum.Name,
'                         TokenTypeEnum.Literal,
'                         TokenTypeEnum.Hex,
'                         TokenTypeEnum.Comment
'                        _token = buffer.ToString()
'                        'break;
'                    Case TokenTypeEnum.Integer
'                        _token = Int32.Parse(buffer.ToString(), NumberStyles.Integer, StandardNumberFormatInfo)
'                        'break;
'                    Case TokenTypeEnum.Real
'                        _token = Double.Parse(buffer.ToString(), NumberStyles.Float, StandardNumberFormatInfo)
'                        'break;
'                End Select
'            End If
'            Return True
'        End Function

'        Public ReadOnly Property Position As Long
'            Get
'                Return Me._stream.Position
'            End Get
'        End Property

'        '/**
'        '  <summary>Moves the pointer to the given absolute byte position.</summary>
'        '*/
'        Public Sub Seek(ByVal offset As Long)
'            Me._stream.Seek(offset)
'        End Sub

'        '/**
'        '  <summary>Moves the pointer to the given relative byte position.</summary>
'        '*/
'        Public Sub Skip(ByVal offset As Long)
'            Me._stream.Skip(offset)
'        End Sub

'        '/**
'        '  <summary>Moves the pointer before the next non-EOL character after the current position.</summary>
'        '  <returns>Whether the _stream can be further read.</returns>
'        '*/
'        Public Function SkipEOL() As Boolean
'            Dim c As Integer
'            Do
'                c = Me._stream.ReadByte()
'                If (c = -1) Then Return False
'            Loop While (IsEOL(c)) ' Keeps going till there's an EOL character.
'            Me._stream.Skip(-1) ' Moves back to the first non-EOL character position.
'            Return True
'        End Function

'        '/**
'        '  <summary>Moves the pointer before the next non-whitespace character after the current position.</summary>
'        '  <returns>Whether the _stream can be further read.</returns>
'        '*/
'        Public Function SkipWhitespace() As Boolean
'            Dim c As Integer
'            Do
'                c = Me._stream.ReadByte()
'                If (c = -1) Then Return False
'            Loop While (IsWhitespace(c)) ' Keeps going till there's a whitespace character.
'            Me._stream.Skip(-1) ' Moves back to the first non-whitespace character position.
'            Return True
'        End Function

'        Public ReadOnly Property Stream As IInputStream
'            Get
'                Return Me._stream
'            End Get
'        End Property

'        '/**
'        '  <summary> Gets the currently-parsed _token.</summary>
'        '*/
'        Public Property Token As Object
'            Get
'                Return Me._token
'            End Get
'            Protected Set(value As Object)
'                Me._token = value
'            End Set
'        End Property


'        '/**
'        '  <summary> Gets the currently-parsed _token type.</summary>
'        '*/
'        Public Property TokenType As TokenTypeEnum
'            Get
'                Return Me._tokenType
'            End Get
'            Protected Set(value As TokenTypeEnum)
'                Me._tokenType = value
'            End Set
'        End Property


'#Region "IDisposable"

'        Public Sub Dispose() Implements IDisposable.Dispose
'            If (Me._stream IsNot Nothing) Then
'                Me._stream.Dispose()
'                Me._stream = Nothing
'            End If
'            GC.SuppressFinalize(Me)
'        End Sub
'#End Region
'#End Region
'#End Region
'#End Region
'    End Class

'End Namespace

'/*
'  Copyright 2007 - 2012 Stefano Chizzolini. http: //www.dmdpdf.org

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

Imports DMD.org.dmdpdf.documents.contents.fonts

Imports System
Imports System.Text.RegularExpressions

Namespace DMD.org.dmdpdf.documents.contents.composition

    '/**
    '<summary> Text fitter.</summary>
    '*/
    Public NotInheritable Class TextFitter

#Region "dynamic"
#Region "fields"
        Private ReadOnly _font As Font
        Private ReadOnly _fontSize As Double
        Private ReadOnly _hyphenation As Boolean
        Private ReadOnly _hyphenationCharacter As Char
        Private ReadOnly _text As String
        Private _width As Double

        Private _beginIndex As Integer = 0
        Private _endIndex As Integer = -1
        Private _fittedText As String
        Private _fittedWidth As Double

#End Region

#Region "constructors"

        Friend Sub New(ByVal text As String, ByVal width As Double, ByVal font As Font, ByVal fontSize As Double, ByVal hyphenation As Boolean, ByVal hyphenationCharacter As Char)
            'System.Diagnostics.Debug.Print("TextFitter.New(" & text & ", " & width & ", " & font.ToString() & ", " & fontSize & ", " & hyphenation & ", " & hyphenationCharacter & ")")
            Me._text = text
            Me._width = width
            Me._font = font
            Me._fontSize = fontSize
            Me._hyphenation = hyphenation
            Me._hyphenationCharacter = hyphenationCharacter
        End Sub

#End Region

#Region "interface"
#Region "public"
        '/**
        '    <summary> Fits the text inside the specified width.</summary>
        '    <param name = "unspacedFitting" > Whether fitting Of unspaced text Is allowed.</param>
        '    <returns> Whether the operation was successful.</returns>
        '*/
        Public Function Fit(ByVal unspacedFitting As Boolean) As Boolean
            'System.Diagnostics.Debug.Print("TextFitter.Fint(" & unspacedFitting & ")")
            Return Me.Fit(Me._endIndex + 1, Me._width, unspacedFitting)
        End Function

        '/**
        '    <summary> Fits the text inside the specified width.</summary>
        '    <param name = "index" > Beginning index, inclusive.</param>
        '    <param name = "width" > Available width.</param>
        '    <param name = "unspacedFitting" > Whether fitting Of unspaced text Is allowed.</param>
        '    <returns> Whether the operation was successful.</returns>
        '*/
        Public Function Fit(ByVal index As Integer, ByVal width As Double, ByVal unspacedFitting As Boolean) As Boolean
            'System.Diagnostics.Debug.Print("TextFitter.Fint(" & index & ", " & width & ", " & unspacedFitting & ")")
            Me._beginIndex = index
            Me._width = width

            Me._fittedText = Nothing
            Me._fittedWidth = 0

            Dim hyphen As String = String.Empty

            ' Fitting the text within the available width...
            '{
            Dim pattern As Regex = New Regex("(\s*)(\S*)")
            Dim match As Match = pattern.Match(Me._text, Me._beginIndex)
            While (match.Success)
                ' Scanning for the presence of a line break...
                '{
                Dim leadingWhitespaceGroup As Group = match.Groups(1)
                '/*
                '    NOTE: This Text fitting algorithm returns everytime it finds a line break character,
                '    as it's intended to evaluate the width of just a single line of text at a time.
                '*/
                Dim spaceEnd As Integer = leadingWhitespaceGroup.Index + leadingWhitespaceGroup.Length
                For spaceIndex As Integer = leadingWhitespaceGroup.Index To spaceEnd - 1
                    Select Case (Me._text(spaceIndex))
                        Case ControlChars.Cr, ControlChars.Lf  ' '\n':                     Case '\r':
                            index = spaceIndex
                            GoTo endFitting ' NOTE: I know GoTo Is evil, but In Me Case Using it sparingly avoids cumbersome Boolean flag checks.
                    End Select
                Next

                Dim matchGroup As Group = match.Groups(0)
                ' Add the current word!
                Dim wordEndIndex As Integer = matchGroup.Index + matchGroup.Length ' Current word's limit.
                Dim wordWidth As Double = _font.GetWidth(matchGroup.Value, _fontSize) ' Current word's width.
                _fittedWidth += wordWidth
                ' Does the fitted text's width exceed the available width?
                If (_fittedWidth > width) Then
                    ' Remove the current (unfitting) word!
                    _fittedWidth -= wordWidth
                    wordEndIndex = index
                    If (Not _hyphenation AndAlso
                                (wordEndIndex > _beginIndex OrElse Not unspacedFitting OrElse
                                 _text(_beginIndex) = " "c)
                                 ) Then ' Enough non-hyphenated text fitted.
                        GoTo endFitting
                    End If

                    '/*
                    '    NOTE: We need To hyphenate the current (unfitting) word.
                    '*/
                    Hyphenate(_hyphenation, index, wordEndIndex, wordWidth, hyphen)

                    Exit While 'break;
                End If
                index = wordEndIndex

                match = match.NextMatch()
                '}
            End While
endFitting:
            _fittedText = _text.Substring(_beginIndex, index - _beginIndex) + hyphen
            _endIndex = index

            Return (_fittedWidth > 0)
        End Function

        '/**
        '    <summary> Gets the begin index Of the fitted text inside the available text.</summary>
        '*/
        Public ReadOnly Property BeginIndex As Integer
            Get
                'System.Diagnostics.Debug.Print("TextFitter.getBeginIndex() -> " & Me._beginIndex)
                Return _beginIndex
            End Get
        End Property

        '/**
        '    <summary> Gets the End index Of the fitted text inside the available text.</summary>
        '*/
        Public ReadOnly Property EndIndex As Integer
            Get
                'System.Diagnostics.Debug.Print("TextFitter.getEndIndex() ->" & Me._endIndex)
                Return _endIndex
            End Get
        End Property

        '/**
        '    <summary> Gets the fitted text.</summary>
        '*/
        Public ReadOnly Property FittedText As String
            Get
                'System.Diagnostics.Debug.Print("TextFitter.getFittedText() -> " & Me._fittedText)
                Return Me._fittedText
            End Get
        End Property

        '/**
        '    <summary> Gets the fitted text's width.</summary>
        '*/
        Public ReadOnly Property FittedWidth As Double
            Get
                'System.Diagnostics.Debug.Print("TextFitter.getFittedWidth() -> " & Me._fittedWidth)
                Return Me._fittedWidth
            End Get
        End Property

        '/**
        '    <summary> Gets the font used To fit the text.</summary>
        '*/
        Public ReadOnly Property Font As Font
            Get
                'System.Diagnostics.Debug.Print("TextFitter.getFont() -> " & Me._font.ToString())
                Return Me._font
            End Get
        End Property

        '/**
        '    <summary> Gets the size Of the font used To fit the text.</summary>
        '*/
        Public ReadOnly Property FontSize As Double
            Get
                'System.Diagnostics.Debug.Print("TextFitter.getFontSize() -> " & Me._fontSize)
                Return Me._fontSize
            End Get
        End Property

        '/**
        '    <summary> Gets whether the hyphenation algorithm has To be applied.</summary>
        '*/
        Public ReadOnly Property Hyphenation As Boolean
            Get
                'System.Diagnostics.Debug.Print("TextFitter.getHyphenation() -> " & Me._hyphenation)
                Return Me._hyphenation
            End Get
        End Property

        '/**
        '    <summary> Gets/Sets the character shown at the End Of the line before a hyphenation break.
        '    </summary>
        '*/
        Public ReadOnly Property HyphenationCharacter As Char
            Get
                'System.Diagnostics.Debug.Print("TextFitter.getHyphenationCharacter() -> " & Me._hyphenationCharacter)
                Return Me._hyphenationCharacter
            End Get
        End Property

        '/**
        '    <summary> Gets the available text.</summary>
        '*/
        Public ReadOnly Property Text As String
            Get
                'System.Diagnostics.Debug.Print("TextFitter.getText() -> " & Me._text)
                Return Me._text
            End Get
        End Property

        '/**
        '    <summary> Gets the available width.</summary>
        '*/
        Public ReadOnly Property Width As Double
            Get
                'System.Diagnostics.Debug.Print("TextFitter.getWidth() -> " & Me._width)
                Return Me._width
            End Get
        End Property

#End Region

#Region "Private"

        Private Sub Hyphenate(ByVal hyphenation As Boolean, ByRef index As Integer, ByRef wordEndIndex As Integer, ByVal wordWidth As Double, ByRef hyphen As String)
            'System.Diagnostics.Debug.Print("TextFitter.Hyphenate(" & hyphenation & ", " & index & ", " & wordEndIndex & ", " & wordWidth & ", ?)")
            '/*
            'TODO:           This hyphenation algorithm Is quite primitive (To improve!).
            '*/
            While (True)
                ' Add the current character!
                Dim textChar As Char = _text(wordEndIndex)
                wordWidth = _font.GetWidth(textChar, _fontSize)
                wordEndIndex += 1
                _fittedWidth += wordWidth
                ' Does the fitted text's width exceed the available width?
                If (_fittedWidth > _width) Then
                    ' Remove the current character!
                    _fittedWidth -= wordWidth
                    wordEndIndex -= 1
                    If (hyphenation) Then
                        ' Is hyphenation to be applied?
                        If (wordEndIndex > index + 4) Then ' Long-enough word chunk.
                            ' Make room for the hyphen character!
                            wordEndIndex -= 1
                            index = wordEndIndex
                            textChar = _text(wordEndIndex)
                            _fittedWidth -= _font.GetWidth(textChar, _fontSize)

                            ' Add the hyphen character!
                            textChar = _hyphenationCharacter
                            _fittedWidth += _font.GetWidth(textChar, _fontSize)

                            hyphen = textChar.ToString()
                        Else ' No hyphenation.
                            ' Removing the current word chunk...
                            While (wordEndIndex > index)
                                wordEndIndex -= 1
                                textChar = _text(wordEndIndex)
                                _fittedWidth -= _font.GetWidth(textChar, _fontSize)
                            End While

                            hyphen = String.Empty
                        End If
                    Else
                        index = wordEndIndex

                        hyphen = String.Empty
                    End If
                    Exit While ' break;
                End If
            End While
            'System.Diagnostics.Debug.Print("TextFitter.Hyphenate -> (" & hyphenation & ", " & index & ", " & wordEndIndex & ", " & wordWidth & ", " & hyphen & ")")
        End Sub

#End Region
#End Region
#End Region
    End Class

End Namespace

''/*
''  Copyright 2007-2012 Stefano Chizzolini. http://www.dmdpdf.org

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

'Imports DMD.org.dmdpdf.documents.contents.fonts

'Imports System
'Imports System.Text.RegularExpressions

'Namespace DMD.org.dmdpdf.documents.contents.composition


'    '/**
'    '  <summary>Text fitter.</summary>
'    '*/
'    Public NotInheritable Class TextFitter

'#Region "dynamic"
'#Region "fields"

'        Private ReadOnly _font As Font
'        Private ReadOnly _fontSize As Double
'        Private ReadOnly _hyphenation As Boolean
'        Private ReadOnly _hyphenationCharacter As Char
'        Private ReadOnly _text As String
'        Private _width As Double

'        Private _beginIndex As Integer = 0
'        Private _endIndex As Integer = -1
'        Private _fittedText As String
'        Private _fittedWidth As Double

'#End Region

'#Region "constructors"

'        Friend Sub New(
'                      ByVal text As String,
'                      ByVal width As Double,
'                      ByVal font As Font,
'                      ByVal fontSize As Double,
'                      ByVal hyphenation As Boolean,
'                      ByVal hyphenationCharacter As Char
'                      )
'            'System.Diagnostics.Debug.Print("TextFitter.New(" & text & ", " & width & ", " & font.ToString() & ", " & fontSize & ", " & hyphenation & ", " & hyphenationCharacter)
'            Me._text = text
'            Me._width = width
'            Me._font = font
'            Me._fontSize = fontSize
'            Me._hyphenation = hyphenation
'            Me._hyphenationCharacter = hyphenationCharacter
'        End Sub

'#End Region

'#Region "interface"
'#Region "public"
'        '/**
'        '  <summary> Fits the text inside the specified width.</summary>
'        '  <param name = "unspacedFitting" > Whether fitting Of unspaced text Is allowed.</param>
'        '  <returns> Whether the operation was successful.</returns>
'        '*/
'        Public Function Fit(ByVal unspacedFitting As Boolean) As Boolean
'            'System.Diagnostics.Debug.Print("TextFitter.Fint(" & unspacedFitting & ")")
'            Return Fit(_endIndex + 1, _width, unspacedFitting)
'        End Function

'        '/**
'        '  <summary> Fits the text inside the specified width.</summary>
'        '  <param name = "index" > Beginning index, inclusive.</param>
'        '  <param name = "width" > Available width.</param>
'        '  <param name = "unspacedFitting" > Whether fitting Of unspaced text Is allowed.</param>
'        '  <returns> Whether the operation was successful.</returns>
'        '*/
'        Public Function Fit(ByVal Index As Integer, ByVal width As Double, ByVal unspacedFitting As Boolean) As Boolean
'            'System.Diagnostics.Debug.Print("TextFitter.Fint(" & Index & ", " & width & ", " & unspacedFitting & ")")
'            _beginIndex = Index
'            Me._width = width
'            _fittedText = Nothing
'            _fittedWidth = 0

'            Dim hyphen As String = String.Empty

'            ' Fitting the text within the available width...
'            '{
'            Dim pattern As Regex = New Regex("(\s*)(\S*)") '@
'            Dim Match As Match = pattern.Match(_text, _beginIndex)
'            While (Match.Success)
'                ' Scanning for the presence of a line break...
'                '{
'                Dim leadingWhitespaceGroup As Group = Match.Groups(1)
'                '/*
'                '  NOTE: This text fitting algorithm returns everytime it finds a line break character,
'                '  as it's intended to evaluate the width of just a single line of text at a time.
'                '*/
'                Dim spaceEnd As Integer = leadingWhitespaceGroup.Index + leadingWhitespaceGroup.Length
'                For spaceIndex As Integer = leadingWhitespaceGroup.Index To spaceEnd - 1
'                    Select Case (_text(spaceIndex))
'                        Case ControlChars.Lf, ControlChars.Cr ''\r': ''\n':
'                            Index = spaceIndex
'                            GoTo endFitting ' NOTE: I know GoTo Is evil, but In Me Case Using it sparingly avoids cumbersome Boolean flag checks.
'                    End Select
'                Next
'                '}

'                Dim matchGroup As Group = Match.Groups(0)
'                ' Add the current word!
'                Dim wordEndIndex As Integer = matchGroup.Index + matchGroup.Length ' Current word's limit.
'                Dim wordWidth As Double = _font.GetWidth(matchGroup.Value, _fontSize) ' Current word's width.
'                _fittedWidth += wordWidth
'                ' Does the fitted text's width exceed the available width?
'                If (_fittedWidth > width) Then
'                    ' Remove the current (unfitting) word!
'                    _fittedWidth -= wordWidth
'                    wordEndIndex = Index
'                    '      If (Not _hyphenation AndAlso (wordEndIndex > _beginIndex // There's fitted content.
'                    '  || !unspacedFitting // There's no fitted content, but unspaced fitting isn't allowed.
'                    '  || _text[_beginIndex] == ' ') // Unspaced fitting is allowed, but text starts with a space.
'                    ') Then ' Enough non-hyphenated text fitted.
'                    If (Not _hyphenation AndAlso
'                                        (wordEndIndex > _beginIndex OrElse
'                                        Not unspacedFitting OrElse
'                                        _text(_beginIndex) = " "c)
'                                        ) Then ' Enough non-hyphenated text fitted.
'                        GoTo endFitting
'                    End If

'                    '/*
'                    '  NOTE: We need To hyphenate the current (unfitting) word.
'                    '*/
'                    Hyphenate(_hyphenation, Index, wordEndIndex, wordWidth, hyphen)

'                    'break;
'                End If
'                Index = wordEndIndex

'                Match = Match.NextMatch()
'            End While
'            '}
'endFitting:
'            _fittedText = _text.Substring(_beginIndex, Index - _beginIndex) + hyphen
'            _endIndex = Index

'            Return (_fittedWidth > 0)
'        End Function

'        '/**
'        '  <summary> Gets the begin index Of the fitted text inside the available text.</summary>
'        '*/
'        Public ReadOnly Property BeginIndex As Integer
'            Get
'                'System.Diagnostics.Debug.Print("TextFitter.getBeginIndex() -> " & Me._beginIndex)
'                Return Me._beginIndex
'            End Get
'        End Property

'        '/**
'        '  <summary> Gets the End index Of the fitted text inside the available text.</summary>
'        '*/
'        Public ReadOnly Property EndIndex As Integer
'            Get
'                Return Me._endIndex
'            End Get
'        End Property

'        '/**
'        '  <summary> Gets the fitted text.</summary>
'        '*/
'        Public ReadOnly Property FittedText As String
'            Get
'                Return Me._fittedText
'            End Get
'        End Property

'        '/**
'        '  <summary> Gets the fitted text's width.</summary>
'        '*/
'        Public ReadOnly Property FittedWidth As Double
'            Get
'                Return Me._fittedWidth
'            End Get
'        End Property

'        '/**
'        '  <summary> Gets the font used To fit the text.</summary>
'        '*/
'        Public ReadOnly Property Font As Font
'            Get
'                Return Me._font
'            End Get
'        End Property

'        '/**
'        '  <summary> Gets the size Of the font used To fit the text.</summary>
'        '*/
'        Public ReadOnly Property FontSize As Double
'            Get
'                Return Me._fontSize
'            End Get
'        End Property

'        '/**
'        '  <summary> Gets whether the hyphenation algorithm has To be applied.</summary>
'        '*/
'        Public ReadOnly Property Hyphenation As Boolean
'            Get
'                Return Me._hyphenation
'            End Get
'        End Property

'        '/**
'        '  <summary> Gets/Sets the character shown at the End Of the line before a hyphenation break.
'        '  </summary>
'        '*/
'        Public ReadOnly Property HyphenationCharacter As Char
'            Get
'                Return Me._hyphenationCharacter
'            End Get
'        End Property

'        '/**
'        '  <summary> Gets the available text.</summary>
'        '*/
'        Public ReadOnly Property Text As String
'            Get
'                Return Me._text
'            End Get
'        End Property

'        '/**
'        '  <summary> Gets the available width.</summary>
'        '*/
'        Public ReadOnly Property Width As Double
'            Get
'                Return Me._width
'            End Get
'        End Property

'#End Region

'#Region "Private"

'        Private Sub Hyphenate(ByVal hyphenation As Boolean, ByRef index As Integer, ByRef wordEndIndex As Integer, ByVal wordWidth As Double, ByRef hyphen As String)
'            '/*
'            '  TODO: This hyphenation algorithm Is quite primitive (To improve!).
'            '*/
'            While (True)
'                ' Add the current character!
'                Dim textChar As Char = _text(wordEndIndex)
'                wordWidth = _font.GetWidth(textChar, _fontSize)
'                wordEndIndex += 1
'                _fittedWidth += wordWidth
'                ' Does the fitted text's width exceed the available width?
'                If (_fittedWidth > _width) Then
'                    ' Remove the current character!
'                    _fittedWidth -= wordWidth
'                    wordEndIndex -= 1
'                    If (hyphenation) Then
'                        ' Is hyphenation to be applied?
'                        If (wordEndIndex > index + 4) Then ' Long - enoughThen word chunk.
'                            ' Make room for the hyphen character!
'                            wordEndIndex -= 1
'                            index = wordEndIndex
'                            textChar = _text(wordEndIndex)
'                            _fittedWidth -= _font.GetWidth(textChar, _fontSize)

'                            ' Add the hyphen character!
'                            textChar = _hyphenationCharacter
'                            _fittedWidth += _font.GetWidth(textChar, _fontSize)

'                            hyphen = textChar.ToString()
'                        Else ' No hyphenation.
'                            ' Removing the current word chunk...
'                            While (wordEndIndex > index)
'                                wordEndIndex -= 1
'                                textChar = _text(wordEndIndex)
'                                _fittedWidth -= _font.GetWidth(textChar, _fontSize)
'                            End While

'                            hyphen = String.Empty
'                        End If
'                    Else
'                        index = wordEndIndex

'                        hyphen = String.Empty
'                    End If
'                    Exit While
'                End If
'            End While
'        End Sub

'#End Region
'#End Region
'#End Region

'    End Class

'End Namespace
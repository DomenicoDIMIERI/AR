'/*
' * Licensed to the Apache Software Foundation (ASF) under one or more
' * contributor license agreements.  See the NOTICE file distributed with
' * this work for additional information regarding copyright ownership.
' * The ASF licenses this file to You under the Apache License, Version 2.0
' * (the "License"); you may not use this file except in compliance with
' * the License.  You may obtain a copy of the License at
' *
' *      http://www.apache.org/licenses/LICENSE-2.0
' *
' * Unless required by applicable law or agreed to in writing, software
' * distributed under the License is distributed on an "AS IS" BASIS,
' * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
' * See the License for the specific language governing permissions and
' * limitations under the License.
' */
Imports System.Text

Namespace org.apache.pdfbox.pdmodel.common.function.type4

    '/**
    ' * Parser for PDF Type 4 functions. This implements a small subset of the PostScript
    ' * language but is no full PostScript interpreter.
    ' *
    ' * @version $Revision$
    ' */
    Public Class Parser

        '/** Used to indicate the parsers current state. */
        Private Enum State
            NEWLINE
            WHITESPACE
            COMMENT
            TOKEN
        End Enum

        Private Sub New()
            'nop
        End Sub

        '/**
        ' * Parses a Type 4 function and sends the syntactic elements to the given
        ' * syntax handler.
        ' * @param input the text source
        ' * @param handler the syntax handler
        ' */
        Public Shared Sub parse(ByVal input As CharSequence, ByVal handler As SyntaxHandler)
            Dim t As Tokenizer = New Tokenizer(input, handler)
            t.tokenize()
        End Sub

        ''' <summary>
        ''' This interface defines all possible syntactic elements of a Type 4 function. It is called by the parser as the function is interpreted.
        ''' </summary>
        ''' <remarks></remarks>
        Public Interface SyntaxHandler


            ''' <summary>
            ''' Indicates that a new line starts.
            ''' </summary>
            ''' <param name="text">the new line character (CR, LF, CR/LF or FF)</param>
            ''' <remarks></remarks>
            Sub newLine(ByVal text As CharSequence)

            '/**
            ' * Called when whitespace characters are encountered.
            ' * @param text the whitespace text
            ' */
            Sub whitespace(ByVal text As CharSequence)

            '/**
            ' * Called when a token is encountered. No distinction between operators and values
            ' * is done here.
            ' * @param text the token text
            ' */
            Sub token(ByVal text As CharSequence)

            '/**
            ' * Called for a comment.
            ' * @param text the comment
            ' */
            Sub comment(ByVal text As CharSequence)
        End Interface


        ''' <summary>
        ''' Abstract base class for a {@link SyntaxHandler}.
        ''' </summary>
        ''' <remarks></remarks>
        Public MustInherit Class AbstractSyntaxHandler
            Implements SyntaxHandler


            Public Sub comment(ByVal text As CharSequence) Implements SyntaxHandler.comment
                'nop
            End Sub

            Public Sub newLine(ByVal text As CharSequence) Implements SyntaxHandler.newLine
                'nop
            End Sub

            Public Sub whitespace(ByVal text As CharSequence) Implements SyntaxHandler.whitespace
                'nop
            End Sub

            Public MustOverride Sub token(text As CharSequence) Implements SyntaxHandler.token

        End Class

        ''' <summary>
        ''' Tokenizer for Type 4 functions.
        ''' </summary>
        ''' <remarks></remarks>
        Private Class Tokenizer
    
            Private Shared ReadOnly NUL As Char = ChrW(0)  ' '\u0000'; //NUL
            Private Shared ReadOnly EOT As Char = ChrW(4) ' '\u0004'; //END OF TRANSMISSION
            Private Shared ReadOnly TAB As Char = ChrW(9) ''\u0009'; //TAB CHARACTER
            Private Shared ReadOnly FF As Char = ChrW(&HC) '= '\u000C'; //FORM FEED
            Private Shared ReadOnly CR As Char = ChrW(13) '= '\r'; //CARRIAGE RETURN
            Private Shared ReadOnly LF As Char = ChrW(10) ''\n'; //LINE FEED
            Private Shared ReadOnly SPACE As Char = ChrW(&H20) ' = '\u0020'; //SPACE

            Private input As CharSequence
            Private index As Integer
            Private handler As SyntaxHandler
            Private state As State = State.WHITESPACE
            Private buffer As StringBuilder = New StringBuilder()

            Public Sub New(ByVal text As CharSequence, ByVal syntaxHandler As SyntaxHandler)
                Me.input = text
                Me.handler = syntaxHandler
            End Sub

            Private Function hasMore() As Boolean
                Return index < input.length()
            End Function

            Private Function currentChar() As Char
                Return input.charAt(index)
            End Function

            Private Function nextChar() As Char
                index += 1
                If (Not hasMore()) Then
                    Return EOT
                Else
                    Return currentChar()
                End If
            End Function

            Private Function peek() As Char
                If (index < input.length() - 1) Then
                    Return input.charAt(index + 1)
                Else
                    Return EOT
                End If
            End Function

            Private Function nextState() As State
                Dim ch As Char = currentChar()
                Select Case (ch)
                    Case CR, LF, FF
                        state = state.NEWLINE
                    Case NUL, TAB, SPACE
                        state = state.WHITESPACE
                    Case "%"
                        state = state.COMMENT
                    Case Else
                        state = state.TOKEN
                End Select
                Return state
            End Function

            Friend Sub tokenize()
                While (hasMore())
                    buffer.Length = 0
                    nextState()
                    Select Case (state)
                        Case Parser.State.NEWLINE
                            scanNewLine()
                        Case Parser.State.WHITESPACE
                            scanWhitespace()
                        Case Parser.State.COMMENT
                            scanComment()
                        Case Else
                            scanToken()
                    End Select
                End While
            End Sub

            Private Sub scanNewLine()
                Debug.Assert(state = state.NEWLINE)
                Dim ch As Char = currentChar()
                buffer.Append(ch)
                If (ch = CR) Then
                    If (peek() = LF) Then
                        'CRLF is treated as one newline
                        buffer.Append(nextChar())
                    End If
                End If
                handler.newLine(buffer)
                nextChar()
            End Sub

            Private Sub scanWhitespace()
                Debug.Assert(state = state.WHITESPACE)
                buffer.Append(currentChar())
                While (hasMore())
                    Dim ch As Char = nextChar()
                    Select Case (ch)
                        Case NUL, TAB, SPACE
                            buffer.Append(ch)
                        Case Else
                            Exit While
                    End Select
                End While
                handler.whitespace(buffer)
            End Sub

            Private Sub scanComment()
                Debug.Assert(state = state.COMMENT)
                buffer.Append(currentChar())
                'loop:
                While (hasMore())
                    Dim ch As Char = nextChar()
                    Select Case (ch)
                        Case CR, LF, FF
                            Exit While
                        Case Else
                            buffer.Append(ch)
                    End Select
                End While
                'EOF reached
                handler.comment(buffer)
            End Sub

            Private Sub scanToken()
                Debug.Assert(state = state.TOKEN)
                Dim ch As Char = currentChar()
                buffer.Append(ch)
                Select Case (ch)
                    Case "{", "}"
                        handler.token(buffer)
                        nextChar()
                        Exit Sub
                    Case Else
                        'continue
                End Select

                'loop:
                While (hasMore())
                    ch = nextChar()
                    Select (ch)
                        Case NUL, TAB, SPACE, CR, LF, FF, EOT, "{", "}"
                            Exit While
                        Case Else
                            buffer.Append(ch)
                    End Select
                End While
                'EOF reached
                handler.token(buffer)
            End Sub

        End Class

    End Class

End Namespace

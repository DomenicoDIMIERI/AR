Imports FinSeA.Text

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

'import java.util.Stack;
'import java.util.regex.Matcher;
'import java.util.regex.Pattern;

Namespace org.apache.pdfbox.pdmodel.common.function.type4


    '/**
    ' * Basic parser for Type 4 functions which is used to build up instruction sequences.
    ' *
    ' * @version $Revision$
    ' */
    Public Class InstructionSequenceBuilder
        Inherits Parser.AbstractSyntaxHandler

        Private mainSequence As InstructionSequence = New InstructionSequence()
        Private seqStack As Stack(Of InstructionSequence) = New Stack(Of InstructionSequence)

        Private Sub New()
            Me.seqStack.push(Me.mainSequence)
        End Sub

        '/**
        ' * Returns the instruction sequence that has been build from the syntactic elements.
        ' * @return the instruction sequence
        ' */
        Public Function getInstructionSequence() As InstructionSequence
            Return Me.mainSequence
        End Function

        '/**
        ' * Parses the given text into an instruction sequence representing a Type 4 function
        ' * that can be executed.
        ' * @param text the Type 4 function text
        ' * @return the instruction sequence
        ' */
        Public Shared Function parse(ByVal text As CharSequence) As InstructionSequence
            Dim builder As InstructionSequenceBuilder = New InstructionSequenceBuilder()
            Parser.parse(text, builder)
            Return builder.getInstructionSequence()
        End Function

        Private Function getCurrentSequence() As InstructionSequence
            Return Me.seqStack.peek()
        End Function

        Private Shared ReadOnly INTEGER_PATTERN As Pattern = Pattern.compile("[\\+\\-]?\\d+")
        Private Shared ReadOnly REAL_PATTERN As Pattern = Pattern.compile("[\\-]?\\d*\\.\\d*([Ee]\\-?\\d+)?")

        Public Overrides Sub token(ByVal text As CharSequence)
            Dim tokenstr As String = text.ToString()
            token(tokenstr)
        End Sub

        Private Overloads Sub token(ByVal token As String)
            If ("{".Equals(token)) Then
                Dim child As InstructionSequence = New InstructionSequence()
                getCurrentSequence().addProc(child)
                Me.seqStack.push(child)
            ElseIf ("}".Equals(token)) Then
                Me.seqStack.pop()
            Else
                Dim m As Matcher = INTEGER_PATTERN.matcher(token)
                If (m.matches()) Then
                    getCurrentSequence().addInteger(parseInt(token.ToString()))
                    Return
                End If

                m = REAL_PATTERN.matcher(token)
                If (m.matches()) Then
                    getCurrentSequence().addReal(parseReal(token))
                    Return
                End If

                'TODO Maybe implement radix numbers, such as 8#1777 or 16#FFFE

                getCurrentSequence().addName(token.ToString())
            End If
        End Sub

        '/**
        ' * Parses a value of type "int".
        ' * @param token the token to be parsed
        ' * @return the parsed value
        ' */
        Public Shared Function parseInt(ByVal token As String) As Integer
            If (token.StartsWith("+")) Then
                token = token.Substring(1)
            End If
            Return Integer.Parse(token) ' Integer.parseInt(token)
        End Function

        '/**
        ' * Parses a value of type "real".
        ' * @param token the token to be parsed
        ' * @return the parsed value
        ' */
        Public Shared Function parseReal(ByVal token As String) As Single
            Return Single.Parse(token) ' NFloat.parseFloat(token)
        End Function

    End Class


End Namespace
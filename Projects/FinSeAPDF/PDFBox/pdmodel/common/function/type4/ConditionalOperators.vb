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
Namespace org.apache.pdfbox.pdmodel.common.function.type4


    '/**
    ' * Provides the conditional operators such as "if" and "ifelse".
    ' *
    ' * @version $Revision$
    ' */
    Class ConditionalOperators


        '/** Implements the "if" operator. */
        Class [If]
            Inherits BaseOperator

            Public Overrides Sub execute(ByVal context As ExecutionContext)
                Dim stack As Stack(Of Object) = context.getStack()
                Dim proc As InstructionSequence = stack.Pop()
                Dim condition As Boolean = stack.Pop()
                If (condition) Then
                    proc.execute(context)
                End If
            End Sub

        End Class

        '/** Implements the "ifelse" operator. */
        Class IfElse
            Inherits BaseOperator

            Public Overrides Sub execute(ByVal context As ExecutionContext)
                Dim stack As Stack(Of Object) = context.getStack()
                Dim proc2 As InstructionSequence = stack.Pop()
                Dim proc1 As InstructionSequence = stack.Pop()
                Dim condition As Boolean = stack.Pop()
                If (condition) Then
                    proc1.execute(context)
                Else
                    proc2.execute(context)
                End If
            End Sub

        End Class

    End Class

End Namespace

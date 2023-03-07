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
    ' * Provides the bitwise operators such as "and" and "xor".
    ' *
    ' * @version $Revision$
    ' */
    Class BitwiseOperators


        '/** Abstract base class for logical operators. */
        Public MustInherit Class AbstractLogicalOperator
            Inherits BaseOperator

            Public Overrides Sub execute(ByVal context As ExecutionContext)
                Dim stack As Stack(Of Object) = context.getStack()
                Dim op2 As Object = stack.Pop()
                Dim op1 As Object = stack.Pop()
                If (TypeOf (op1) Is Boolean AndAlso TypeOf (op2) Is Boolean) Then
                    Dim bool1 As Boolean = op1
                    Dim bool2 As Boolean = op2
                    Dim result As Boolean = applyForBoolean(bool1, bool2)
                    stack.Push(result)
                ElseIf (TypeOf (op1) Is Integer AndAlso TypeOf (op2) Is Integer) Then
                    Dim int1 As Integer = op1
                    Dim int2 As Integer = op2
                    Dim result As Integer = applyforInteger(int1, int2)
                    stack.Push(result)
                Else
                    Throw New InvalidCastException("Operands must be bool/bool or int/int")
                End If
            End Sub


            Protected MustOverride Function applyForBoolean(ByVal bool1 As Boolean, ByVal bool2 As Boolean) As Boolean

            Protected MustOverride Function applyforInteger(ByVal int1 As Integer, ByVal int2 As Integer) As Integer

        End Class

        '/** Implements the "and" operator. */
        Class [And]
            Inherits AbstractLogicalOperator

            Protected Overrides Function applyForBoolean(ByVal bool1 As Boolean, ByVal bool2 As Boolean) As Boolean
                Return bool1 AndAlso bool2
            End Function

            Protected Overrides Function applyforInteger(ByVal int1 As Integer, ByVal int2 As Integer) As Integer
                Return int1 And int2
            End Function
        End Class

        ' Implements the "bitshift" operator. */
        Class Bitshift
            Inherits BaseOperator

            Public Overrides Sub execute(ByVal context As ExecutionContext)
                Dim stack As Stack(Of Object) = context.getStack()
                Dim shift As Integer = stack.Pop()
                Dim int1 As Integer = stack.Pop()
                If (shift < 0) Then
                    Dim result As Integer = int1 >> Math.Abs(shift)
                    stack.Push(result)
                Else
                    Dim result As Integer = int1 << shift
                    stack.Push(result)
                End If
            End Sub

        End Class

        '** Implements the "false" operator. */
        Class [False]
            Inherits BaseOperator

            Public Overrides Sub execute(ByVal context As ExecutionContext)
                Dim stack As Stack(Of Object) = context.getStack()
                stack.Push(False) 'Boolean.FALSE
            End Sub

        End Class

        '/** Implements the "not" operator. */
        Class [Not]
            Inherits BaseOperator

            Public Overrides Sub execute(ByVal context As ExecutionContext)
                Dim stack As Stack(Of Object) = context.getStack()
                Dim op1 As Object = stack.Pop()
                If (TypeOf (op1) Is Boolean) Then
                    Dim bool1 As Boolean = op1
                    Dim result As Boolean = Not bool1
                    stack.Push(result)
                ElseIf (TypeOf (op1) Is Integer) Then
                    Dim int1 As Integer = op1
                    Dim result As Integer = -int1
                    stack.Push(result)
                Else
                    Throw New InvalidCastException("Operand must be bool or int")
                End If
            End Sub

        End Class

        '/** Implements the "or" operator. */
        Class [Or]
            Inherits AbstractLogicalOperator

            Protected Overrides Function applyForBoolean(ByVal bool1 As Boolean, ByVal bool2 As Boolean) As Boolean
                Return bool1 OrElse bool2
            End Function

            Protected Overrides Function applyforInteger(ByVal int1 As Integer, ByVal int2 As Integer) As Integer
                Return int1 Or int2
            End Function

        End Class

        '/** Implements the "true" operator. */
        Class [True]
            Inherits BaseOperator

            Public Overrides Sub execute(ByVal context As ExecutionContext)
                Dim stack As Stack(Of Object) = context.getStack()
                stack.Push(True)
            End Sub

        End Class

        '/** Implements the "xor" operator. */
        Class [Xor]
            Inherits AbstractLogicalOperator

            Protected Overrides Function applyForBoolean(ByVal bool1 As Boolean, ByVal bool2 As Boolean) As Boolean
                Return bool1 Xor bool2
            End Function

            Protected Overrides Function applyforInteger(ByVal int1 As Integer, ByVal int2 As Integer) As Integer
                Return int1 Xor int2
            End Function

        End Class

    End Class

End Namespace
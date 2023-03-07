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

Namespace org.apache.pdfbox.pdmodel.common.function.type4

    '/**
    ' * Provides the relational operators such as "eq" and "le".
    ' *
    ' * @version $Revision$
    ' */
    Class RelationalOperators


        '/** Implements the "eq" operator. */
        Class Eq
            Inherits BaseOperator

            Public Overrides Sub execute(ByVal context As ExecutionContext)
                Dim [stack] As Stack(Of Object) = context.getStack()
                Dim op2 As Object = stack.Pop()
                Dim op1 As Object = stack.Pop()
                Dim result As Boolean = isEqual(op1, op2)
                stack.Push(result)
            End Sub

            Protected Overridable Function isEqual(ByVal op1 As Object, ByVal op2 As Object) As Boolean
                Dim result As Boolean = False
                If (TypeOf (op1) Is Number AndAlso TypeOf (op2) Is Number) Then
                    Dim num1 As Number = op1
                    Dim num2 As Number = op2
                    result = num1.floatValue() = num2.floatValue()
                Else
                    result = op1.Equals(op2)
                End If
                Return result
            End Function

        End Class

        '/** Abstract base class for number comparison operators. */
        MustInherit Class AbstractNumberComparisonOperator
            Inherits BaseOperator

            Public Overrides Sub execute(ByVal context As ExecutionContext)
                Dim stack As Stack(Of Object) = context.getStack()
                Dim op2 As Object = stack.Pop()
                Dim op1 As Object = stack.Pop()
                Dim num1 As Number = op1
                Dim num2 As Number = op2
                Dim result As Boolean = compare(num1, num2)
                stack.Push(result)
            End Sub

            Protected MustOverride Function compare(ByVal num1 As Number, ByVal num2 As Number) As Boolean

        End Class

        '/** Implements the "ge" operator. */
        Class Ge
            Inherits AbstractNumberComparisonOperator

            Protected Overrides Function compare(ByVal num1 As Number, ByVal num2 As Number) As Boolean
                Return num1.floatValue() >= num2.floatValue()
            End Function

        End Class

        '/** Implements the "gt" operator. */
        Class Gt
            Inherits AbstractNumberComparisonOperator

            Protected Overrides Function compare(ByVal num1 As Number, ByVal num2 As Number) As Boolean
                Return num1.floatValue() > num2.floatValue()
            End Function

        End Class

        '/** Implements the "le" operator. */
        Class Le
            Inherits AbstractNumberComparisonOperator

            Protected Overrides Function compare(ByVal num1 As Number, ByVal num2 As Number) As Boolean
                Return num1.floatValue() <= num2.floatValue()
            End Function

        End Class

        '/** Implements the "lt" operator. */
        Class Lt
            Inherits AbstractNumberComparisonOperator

            Protected Overrides Function compare(ByVal num1 As Number, ByVal num2 As Number) As Boolean
                Return num1.floatValue() < num2.floatValue()
            End Function

        End Class

        '** Implements the "ne" operator. */
        Class Ne
            Inherits Eq

            Protected Overrides Function isEqual(ByVal op1 As Object, ByVal op2 As Object) As Boolean
                Return Not MyBase.isEqual(op1, op2)
            End Function

        End Class

    End Class

End Namespace
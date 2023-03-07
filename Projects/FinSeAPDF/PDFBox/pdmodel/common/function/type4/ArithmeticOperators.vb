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
Imports System.IO
Imports FinSeA.Math

Namespace org.apache.pdfbox.pdmodel.common.function.type4
    Public MustInherit Class BaseOperator
        Implements [Operator]

        Public MustOverride Sub execute(context As ExecutionContext) Implements [Operator].execute

    End Class

    '/**
    ' * Provides the arithmetic operators such as "add" and "sub".
    ' *
    ' * @version $Revision$
    ' */
    Class ArithmeticOperators



        '/** Implements the "abs" operator. */
        Class Abs
            Inherits BaseOperator

            Public Overrides Sub execute(ByVal context As ExecutionContext)
                Dim num As Number = context.popNumber()
                If (num.isInteger) Then
                    context.getStack().push(Math.Abs(num.intValue()))
                Else
                    context.getStack().push(Math.Abs(num.floatValue()))
                End If
            End Sub

        End Class

        '/** Implements the "add" operator. */
        Class Add
            Inherits BaseOperator

            Public Overrides Sub execute(ByVal context As ExecutionContext)
                Dim num2 As Number = context.popNumber()
                Dim num1 As Number = context.popNumber()
                If (num1.isInteger AndAlso num2.isInteger) Then
                    Dim sum As Long = num1.longValue() + num2.longValue()
                    If (sum < Integer.MinValue OrElse sum > Integer.MaxValue) Then
                        context.getStack().push(CSng(sum))
                    Else
                        context.getStack().push(CInt(sum))
                    End If
                Else
                    Dim sum As Single = num1.floatValue() + num2.floatValue()
                    context.getStack().push(sum)
                End If
            End Sub

        End Class

        '/** Implements the "atan" operator. */
        Class Atan
            Inherits BaseOperator

            Public Overrides Sub execute(ByVal context As ExecutionContext)
                Dim den As Single = context.popReal()
                Dim num As Single = context.popReal()
                Dim atan As Single = Math.atan2(num, den)
                atan = Math.toDegrees(atan) Mod 360
                If (atan < 0) Then
                    atan = atan + 360
                End If
                context.getStack().push(atan)
            End Sub

        End Class

        '/** Implements the "ceiling" operator. */
        Class Ceiling
            Inherits BaseOperator

            Public Overrides Sub execute(ByVal context As ExecutionContext)
                Dim num As Number = context.popNumber()
                If (num.isInteger) Then
                    context.getStack().push(num)
                Else
                    context.getStack().push(CSng(Math.Ceiling(num.doubleValue())))
                End If
            End Sub

        End Class

        '/** Implements the "cos" operator. */
        Class Cos
            Inherits BaseOperator

            Public Overrides Sub execute(ByVal context As ExecutionContext)
                Dim angle As Single = context.popReal()
                Dim cos As Single = Math.Cos(Math.toRadians(angle))
                context.getStack().push(Cos)
            End Sub

        End Class

        ' /** Implements the "cvi" operator. */
        Class Cvi
            Inherits BaseOperator

            Public Overrides Sub execute(ByVal context As ExecutionContext)
                Dim num As Number = context.popNumber()
                context.getStack().push(num.intValue())
            End Sub

        End Class

        '   /** Implements the "cvr" operator. */
        Class Cvr
            Inherits BaseOperator

            Public Overrides Sub execute(ByVal context As ExecutionContext)
                Dim num As Number = context.popNumber()
                context.getStack().push(num.floatValue())
            End Sub

        End Class

        '/** Implements the "div" operator. */
        Class Div
            Inherits BaseOperator

            Public Overrides Sub execute(ByVal context As ExecutionContext)
                Dim num2 As Number = context.popNumber()
                Dim num1 As Number = context.popNumber()
                context.getStack().push(num1.floatValue() / num2.floatValue())
            End Sub

        End Class

        '/** Implements the "exp" operator. */
        Class Exp
            Inherits BaseOperator

            Public Overrides Sub execute(ByVal context As ExecutionContext)
                Dim exp As Number = context.popNumber()
                Dim base As Number = context.popNumber()
                Dim value As Double = Math.Pow(base.doubleValue(), exp.doubleValue())
                context.getStack().push(CSng(value))
            End Sub

        End Class

        ' Implements the "floor" operator. */
        Class Floor
            Inherits BaseOperator

            Public Overrides Sub execute(ByVal context As ExecutionContext)
                Dim num As Number = context.popNumber()
                If (num.isInteger) Then
                    context.getStack().push(num)
                Else
                    context.getStack().push(CSng(Math.Floor(num.doubleValue())))
                End If
            End Sub

        End Class

        '* Implements the "idiv" operator. */
        Class IDiv
            Inherits BaseOperator

            Public Overrides Sub execute(ByVal context As ExecutionContext)
                Dim num2 As Integer = context.popInt()
                Dim num1 As Integer = context.popInt()
                context.getStack().push(num1 / num2)
            End Sub

        End Class

        ' /** Implements the "ln" operator. */
        Class Ln
            Inherits BaseOperator

            Public Overrides Sub execute(ByVal context As ExecutionContext)
                Dim num As Object = context.popNumber()
                context.getStack().push(CSng(Math.log(num.doubleValue())))
            End Sub

        End Class

        '/** Implements the "log" operator. */
        Class Log
            Inherits BaseOperator

            Public Overrides Sub execute(ByVal context As ExecutionContext)
                Dim num As Number = context.popNumber()
                context.getStack().push(CSng(Math.log10(num.doubleValue())))
            End Sub

        End Class

        ' Implements the "mod" operator. */
        Class [Mod]
            Inherits BaseOperator

            Public Overrides Sub execute(ByVal context As ExecutionContext)
                Dim int2 As Integer = context.popInt()
                Dim int1 As Integer = context.popInt()
                context.getStack().push(int1 Mod int2)
            End Sub

        End Class

        '** Implements the "mul" operator. */
        Class Mul
            Inherits BaseOperator

            Public Overrides Sub execute(ByVal context As ExecutionContext)
                Dim num2 As Number = context.popNumber()
                Dim num1 As Number = context.popNumber()
                If (num1.isInteger AndAlso num2.isInteger) Then
                    Dim result As Long = num1.longValue() * num2.longValue()
                    If (result >= Integer.MinValue AndAlso result <= Integer.MaxValue) Then
                        context.getStack().push(CInt(result))
                    Else
                        context.getStack().push(CSng(result))
                    End If
                Else
                    Dim result As Double = num1.doubleValue() * num2.doubleValue()
                    context.getStack().push(CSng(result))
                End If
            End Sub

        End Class

        '/** Implements the "neg" operator. */
        Class Neg
            Inherits BaseOperator

            Public Overrides Sub execute(ByVal context As ExecutionContext)
                Dim num As Number = context.popNumber()
                If (num.isInteger) Then
                    Dim v As Integer = num.intValue()
                    If (v = Integer.MinValue) Then
                        context.getStack().push(-num.floatValue())
                    Else
                        context.getStack().push(-num.intValue())
                    End If
                Else
                    context.getStack().push(-num.floatValue())
                End If
            End Sub

        End Class


        '/** Implements the "round" operator. */
        Class Round
            Inherits BaseOperator

            Public Overrides Sub execute(ByVal context As ExecutionContext)
                Dim num As Number = context.popNumber()
                If (num.isInteger) Then
                    context.getStack().push(num.intValue())
                Else
                    context.getStack().push(Math.round(num.doubleValue()))
                End If
            End Sub

        End Class

        '/** Implements the "sin" operator. */
        Class Sin
            Inherits BaseOperator

            Public Overrides Sub execute(ByVal context As ExecutionContext)
                Dim angle As Single = context.popReal()
                Dim sin As Single = Math.Sin(Math.toRadians(angle))
                context.getStack().push(sin)
            End Sub

        End Class

        '/** Implements the "sqrt" operator. */
        Class Sqrt
            Inherits BaseOperator

            Public Overrides Sub execute(ByVal context As ExecutionContext)
                Dim num As Single = context.popReal()
                If (num < 0) Then
                    Throw New ArgumentOutOfRangeException("argument must be nonnegative: Sqrt")
                End If
                context.getStack().push(Math.Sqrt(num))
            End Sub

        End Class

        '/** Implements the "sub" operator. */
        Class [Sub]
            Inherits BaseOperator

            Public Overrides Sub execute(ByVal context As ExecutionContext)
                Dim stack As Stack(Of Object) = context.getStack()
                Dim num2 As Number = context.popNumber()
                Dim num1 As Number = context.popNumber()
                If (num1.isInteger AndAlso num2.isInteger) Then
                    Dim result As Long = num1.longValue() - num2.longValue()
                    If (result < Integer.MinValue OrElse result > Integer.MaxValue) Then
                        stack.push(CSng(result))
                    Else
                        stack.push(CInt(result))
                    End If
                Else
                    Dim result As Single = num1.floatValue() - num2.floatValue()
                    stack.push(CSng(result))
                End If
            End Sub

        End Class

        '/** Implements the "truncate" operator. */
        Class Truncate
            Inherits BaseOperator

            Public Overrides Sub execute(ByVal context As ExecutionContext)
                Dim num As Number = context.popNumber()
                If (num.isInteger) Then
                    context.getStack().push(num.intValue())
                Else
                    context.getStack().push(CSng(CInt(num.floatValue())))
                End If
            End Sub
        End Class

    End Class

End Namespace
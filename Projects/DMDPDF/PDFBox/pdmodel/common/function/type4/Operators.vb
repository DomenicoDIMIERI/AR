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
Imports FinSeA

Namespace org.apache.pdfbox.pdmodel.common.function.type4


    '/**
    ' * This class provides all the supported operators.
    ' * @version $Revision$
    ' */
    Public Class Operators

        'Arithmetic operators
        Private Shared ReadOnly ABS As [Operator] = New ArithmeticOperators.Abs()
        Private Shared ReadOnly ADD As [Operator] = New ArithmeticOperators.Add()
        Private Shared ReadOnly ATAN As [Operator] = New ArithmeticOperators.Atan()
        Private Shared ReadOnly CEILING As [Operator] = New ArithmeticOperators.Ceiling()
        Private Shared ReadOnly COS As [Operator] = New ArithmeticOperators.Cos()
        Private Shared ReadOnly CVI As [Operator] = New ArithmeticOperators.Cvi()
        Private Shared ReadOnly CVR As [Operator] = New ArithmeticOperators.Cvr()
        Private Shared ReadOnly DIV As [Operator] = New ArithmeticOperators.Div()
        Private Shared ReadOnly EXP As [Operator] = New ArithmeticOperators.Exp()
        Private Shared ReadOnly FLOOR As [Operator] = New ArithmeticOperators.Floor()
        Private Shared ReadOnly IDIV As [Operator] = New ArithmeticOperators.IDiv()
        Private Shared ReadOnly LN As [Operator] = New ArithmeticOperators.Ln()
        Private Shared ReadOnly LOG As [Operator] = New ArithmeticOperators.Log()
        Private Shared ReadOnly [MOD] As [Operator] = New ArithmeticOperators.Mod()
        Private Shared ReadOnly MUL As [Operator] = New ArithmeticOperators.Mul()
        Private Shared ReadOnly NEG As [Operator] = New ArithmeticOperators.Neg()
        Private Shared ReadOnly ROUND As [Operator] = New ArithmeticOperators.Round()
        Private Shared ReadOnly SIN As [Operator] = New ArithmeticOperators.Sin()
        Private Shared ReadOnly SQRT As [Operator] = New ArithmeticOperators.Sqrt()
        Private Shared ReadOnly [SUB] As [Operator] = New ArithmeticOperators.Sub()
        Private Shared ReadOnly TRUNCATE As [Operator] = New ArithmeticOperators.Truncate()

        'Relational, boolean and bitwise operators
        Private Shared ReadOnly [AND] As [Operator] = New BitwiseOperators.And()
        Private Shared ReadOnly BITSHIFT As [Operator] = New BitwiseOperators.Bitshift()
        Private Shared ReadOnly EQ As [Operator] = New RelationalOperators.Eq()
        Private Shared ReadOnly [FALSE] As [Operator] = New BitwiseOperators.False()
        Private Shared ReadOnly GE As [Operator] = New RelationalOperators.Ge()
        Private Shared ReadOnly GT As [Operator] = New RelationalOperators.Gt()
        Private Shared ReadOnly LE As [Operator] = New RelationalOperators.Le()
        Private Shared ReadOnly LT As [Operator] = New RelationalOperators.Lt()
        Private Shared ReadOnly NE As [Operator] = New RelationalOperators.Ne()
        Private Shared ReadOnly [NOT] As [Operator] = New BitwiseOperators.Not()
        Private Shared ReadOnly [OR] As [Operator] = New BitwiseOperators.Or()
        Private Shared ReadOnly [TRUE] As [Operator] = New BitwiseOperators.True()
        Private Shared ReadOnly [XOR] As [Operator] = New BitwiseOperators.Xor()

        'Conditional operators
        Private Shared ReadOnly [IF] As [Operator] = New ConditionalOperators.If()
        Private Shared ReadOnly IFELSE As [Operator] = New ConditionalOperators.IfElse()

        'Stack operators
        Private Shared ReadOnly COPY As [Operator] = New StackOperators.Copy()
        Private Shared ReadOnly DUP As [Operator] = New StackOperators.Dup()
        Private Shared ReadOnly EXCH As [Operator] = New StackOperators.Exch()
        Private Shared ReadOnly INDEX As [Operator] = New StackOperators.Index()
        Private Shared ReadOnly POP As [Operator] = New StackOperators.Pop()
        Private Shared ReadOnly ROLL As [Operator] = New StackOperators.Roll()

        Private operators As Map(Of String, [Operator]) = New HashMap(Of String, [Operator])

        '/**
        ' * Creates a new Operators object with the default set of operators.
        ' */
        Public Sub New()
            operators.put("add", ADD)
            operators.put("abs", ABS)
            operators.put("atan", ATAN)
            operators.put("ceiling", CEILING)
            operators.put("cos", COS)
            operators.put("cvi", CVI)
            operators.put("cvr", CVR)
            operators.put("div", DIV)
            operators.put("exp", EXP)
            operators.put("floor", FLOOR)
            operators.put("idiv", IDIV)
            operators.put("ln", LN)
            operators.put("log", LOG)
            operators.put("mod", [MOD])
            operators.put("mul", MUL)
            operators.put("neg", NEG)
            operators.put("round", ROUND)
            operators.put("sin", SIN)
            operators.put("sqrt", SQRT)
            operators.put("sub", [SUB])
            operators.put("truncate", TRUNCATE)

            operators.put("and", [AND])
            operators.put("bitshift", BITSHIFT)
            operators.put("eq", EQ)
            operators.put("false", [FALSE])
            operators.put("ge", GE)
            operators.put("gt", GT)
            operators.put("le", LE)
            operators.put("lt", LT)
            operators.put("ne", NE)
            operators.put("not", [NOT])
            operators.put("or", [OR])
            operators.put("true", [TRUE])
            operators.put("xor", [XOR])

            operators.put("if", [IF])
            operators.put("ifelse", IFELSE)

            operators.put("copy", COPY)
            operators.put("dup", DUP)
            operators.put("exch", EXCH)
            operators.put("index", INDEX)
            operators.put("pop", POP)
            operators.put("roll", ROLL)
        End Sub

        '/**
        ' * Returns the operator for the given operator name.
        ' * @param operatorName the operator name
        ' * @return the operator (or null if there's no such operator
        ' */
        Public Function getOperator(ByVal operatorName As String) As [Operator]
            Return Me.operators.get(operatorName)
        End Function

    End Class

End Namespace
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

Imports System.IO ' java.util.Stack;

Namespace org.apache.pdfbox.pdmodel.common.function.type4



    '/**
    ' * Makes up the execution context, holding the available operators and the execution stack.
    ' *
    ' * @version $Revision$
    ' */
    Public Class ExecutionContext


        Private operators As Operators
        Private stack As New Stack(Of Object)

        '/**
        ' * Creates a new execution context.
        ' * @param operatorSet the operator set
        ' */
        Public Sub New(ByVal operatorSet As Operators)
            Me.operators = operatorSet
        End Sub

        '/**
        ' * Returns the stack used by this execution context.
        ' * @return the stack
        ' */
        Public Function getStack() As Stack(Of Object)
            Return Me.stack
        End Function

        '/**
        ' * Returns the operator set used by this execution context.
        ' * @return the operator set
        ' */
        Public Function getOperators() As Operators
            Return Me.operators
        End Function

        '/**
        ' * Pops a number (int or real) from the stack. If it's neither data type, a
        ' * ClassCastException is thrown.
        ' * @return the number
        ' */
        Public Function popNumber() As Number
            Return stack.Pop()
        End Function

        '/**
        ' * Pops a value of type int from the stack. If the value is not of type int, a
        ' * ClassCastException is thrown.
        ' * @return the int value
        ' */
        Public Function popInt() As Integer
            Return Me.popNumber.intValue
        End Function

        '/**
        ' * Pops a number from the stack and returns it as a real value. If the value is not of a
        ' * numeric type, a ClassCastException is thrown.
        ' * @return the real value
        ' */
        Public Function popReal() As Single
            Return Me.popNumber.floatValue
        End Function

    End Class

End Namespace
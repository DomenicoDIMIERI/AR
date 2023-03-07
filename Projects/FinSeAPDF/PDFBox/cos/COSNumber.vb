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
'Imports java.io.IOException

Namespace org.apache.pdfbox.cos

    '/**
    ' * This class represents an abstract number in a PDF document.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.10 $
    ' */
    Public MustInherit Class COSNumber
        Inherits COSBase

        '/**
        ' * @deprecated Use the {@link COSInteger#ZERO} constant instead
        ' */
        Public Shared ReadOnly ZERO As COSInteger = COSInteger.ZERO

        '/**
        ' * @deprecated Use the {@link COSInteger#ONE} constant instead
        ' */
        Public Shared ReadOnly ONE As COSInteger = COSInteger.ONE

        '/**
        ' * This will get the Single value of this number.
        ' *
        ' * @return The Single value of this object.
        ' */
        Public MustOverride Function floatValue() As Single

        '/**
        ' * This will get the double value of this number.
        ' *
        ' * @return The double value of this number.
        ' */
        Public MustOverride Function doubleValue() As Double

        '/**
        ' * This will get the integer value of this number.
        ' *
        ' * @return The integer value of this number.
        ' */
        Public MustOverride Function intValue() As Integer

        '/**
        ' * This will get the long value of this number.
        ' *
        ' * @return The long value of this number.
        ' */
        Public MustOverride Function longValue() As Long

        '/**
        ' * This factory method will get the appropriate number object.
        ' *
        ' * @param number The string representation of the number.
        ' *
        ' * @return A number object, either Single or int.
        ' *
        ' * @throws IOException If the string is not a number.
        ' */
        Public Shared Function [get](ByVal number As String) As COSNumber ' throws IOException
            If (number.Length() = 1) Then
                Dim digit As Char = number.Chars(0)
                If ("0" <= digit AndAlso digit <= "9") Then
                    Return COSInteger.[get](Convert.ToInt32(digit) - Asc("0"))
                ElseIf (digit = "-" OrElse digit = ".") Then
                    ' See https://issues.apache.org/jira/browse/PDFBOX-592
                    Return COSInteger.ZERO
                Else
                    Throw New FormatException("Not a number: " & number)
                End If
            ElseIf (number.IndexOf(".") = -1 AndAlso (number.ToLower().IndexOf("e") = -1)) Then
                Try
                    Return COSInteger.get(Integer.Parse(number))
                Catch e As FormatException
                    Throw New FormatException("Value is not an integer: " & number)
                End Try
            Else
                Return New COSFloat(number)
            End If
        End Function

    End Class

End Namespace
Imports FinSeA.Sistema
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
Imports FinSeA.Io

Imports FinSeA.org.apache.pdfbox.exceptions

Namespace org.apache.pdfbox.cos

    '/**
    ' * This class represents an integer number in a PDF document.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.12 $
    ' */
    Public Class COSInteger
        Inherits COSNumber

        '/**
        ' * The lowest integer to be kept in the {@link #STATIC} array.
        ' */
        Private Shared ReadOnly LOW As Integer = -100

        '/**
        ' * The highest integer to be kept in the {@link #STATIC} array.
        ' */
        Private Shared ReadOnly HIGH As Integer = 256

        '/**
        ' * Static instances of all COSIntegers in the range from {@link #LOW}
        ' * to {@link #HIGH}.
        ' */
        Private Shared ReadOnly [STATIC]() As COSInteger = Arrays.CreateInstance(Of COSInteger)(HIGH - LOW + 1)

        '/**
        ' * Constant for the number zero.
        ' * @since Apache PDFBox 1.1.0
        ' */
        Public Shared Shadows ReadOnly ZERO As COSInteger = [get](0)

        '/**
        ' * Constant for the number one.
        ' * @since Apache PDFBox 1.1.0
        ' */
        Public Shared Shadows ReadOnly ONE As COSInteger = [get](1)

        '/**
        ' * Constant for the number two.
        ' * @since Apache PDFBox 1.1.0
        ' */
        Public Shared ReadOnly TWO As COSInteger = [get](2)

        '/**
        ' * Constant for the number three.
        ' * @since Apache PDFBox 1.1.0
        ' */
        Public Shared ReadOnly THREE As COSInteger = [get](3)

        '/**
        ' * Returns a COSInteger instance with the given value.
        ' *
        ' * @param val integer value
        ' * @return COSInteger instance
        ' */
        Public Shared Function _GET(ByVal val As Long) As COSInteger
            If (LOW <= val AndAlso val <= HIGH) Then
                Dim index As Integer = val - LOW
                ' no synchronization needed
                If ([STATIC](index) Is Nothing) Then
                    [STATIC](index) = New COSInteger(val)
                End If
                Return [STATIC](index)
            Else
                Return New COSInteger(val)
            End If
        End Function

        Private value As Long

        '/**
        ' * constructor.
        ' *
        ' * @deprecated use the static {@link #get(long)} method instead
        ' * @param val The integer value of this object.
        ' */
        Public Sub New(ByVal val As Long)
            Me.value = val
        End Sub

        '/**
        ' * constructor.
        ' *
        ' * @deprecated use the static {@link #get(long)} method instead
        ' * @param val The integer value of this object.
        ' */
        Public Sub New(ByVal val As Integer)
            Me.New(CLng(val))
        End Sub

        '/**
        ' * This will create a new PDF Int object using a string.
        ' *
        ' * @param val The string value of the integer.
        ' * @deprecated use the static {@link #get(long)} method instead
        ' * @throws IOException If the val is not an integer type.
        ' */
        Public Sub New(ByVal val As String) 'throws IOException
            Try
                value = Integer.Parse(val)
            Catch e As Exception
                Throw New FormatException("Error: value is not an integer type actual='" & val & "'", e)
            End Try
        End Sub

        '/**
        ' * {@inheritDoc}
        ' */
        Public Overrides Function equals(ByVal o As Object) As Boolean
            Return TypeOf (o) Is COSInteger AndAlso CType(o, COSInteger).intValue() = intValue()
        End Function


        Public Overrides Function GetHashCode() As Integer
            Return (value ^ (value >> 32))
        End Function

        '/**
        ' * {@inheritDoc}
        ' */
        Public Overrides Function toString() As String
            Return "COSInt{" & value & "}"
        End Function

        '/**
        ' * Change the value of this reference.
        ' *
        ' * @param newValue The new value.
        ' */
        Public Sub setValue(ByVal newValue As Integer)
            value = newValue
        End Sub

        '/**
        ' * polymorphic access to value as Single.
        ' *
        ' * @return The Single value of this object.
        ' */
        Public Overrides Function floatValue() As Single
            Return value
        End Function

        '/**
        ' * polymorphic access to value as Single.
        ' *
        ' * @return The double value of this object.
        ' */
        Public Overrides Function doubleValue() As Double
            Return value
        End Function

        '/**
        ' * Polymorphic access to value as int
        ' * This will get the integer value of this object.
        ' *
        ' * @return The int value of this object,
        ' */
        Public Overrides Function intValue() As Integer
            Return value
        End Function

        '/**
        ' * Polymorphic access to value as int
        ' * This will get the integer value of this object.
        ' *
        ' * @return The int value of this object,
        ' */
        Public Overrides Function longValue() As Long
            Return value
        End Function

        '/**
        ' * visitor pattern double dispatch method.
        ' *
        ' * @param visitor The object to notify when visiting this object.
        ' * @return any object, depending on the visitor implementation, or null
        ' * @throws COSVisitorException If an error occurs while visiting this object.
        ' */
        Public Overrides Function accept(ByVal visitor As ICOSVisitor) As Object ' throws COSVisitorException
            Return visitor.visitFromInt(Me)
        End Function

        '/**
        ' * This will output this string as a PDF object.
        ' *
        ' * @param output The stream to write to.
        ' * @throws IOException If there is an error writing to the stream.
        ' */
        Public Sub writePDF(ByVal output As OutputStream) ' throws IOException
            output.Write(Strings.GetBytes(value, "ISO-8859-1"))
        End Sub

    End Class

End Namespace
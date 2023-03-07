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

'imports java.io.ByteArrayInputStream;
'import java.io.ByteArrayOutputStream;
'import java.io.FilterInputStream;
'import java.io.IOException;
'import java.io.InputStream;
Imports System.IO
Imports FinSeA.Io

Namespace org.apache.pdfbox.pdfwriter


    Public Class COSFilterInputStream
        Inherits FilterInputStream

        Dim byteRange() As Integer
        Dim _position As Long = 0

        Public Sub New(ByVal [in] As InputStream, ByVal byteRange() As Integer)
            MyBase.New([in])
            Me.byteRange = byteRange
        End Sub

        Public Sub New(ByVal [in] As Byte(), ByVal byteRange() As Integer)
            MyBase.New(New ByteArrayInputStream([in]))
            Me.byteRange = byteRange
        End Sub

        Public Overrides Function read() As Integer ' throws IOException
            nextAvailable()
            Dim i As Integer = MyBase.read()
            If (i > -1) Then
                _position += 1
            End If
            Return i
        End Function

        Public Overrides Function read(ByVal b() As Byte) As Integer ' throws IOException
            Return read(b, 0, b.Length)
        End Function

        Public Overrides Function read(ByVal b() As Byte, ByVal off As Integer, ByVal len As Integer) As Integer ' throws IOException
            If (b Is Nothing) Then
                Throw New NullReferenceException()
            ElseIf (off < 0 OrElse len < 0 OrElse len > b.Length - off) Then
                Throw New IndexOutOfRangeException()
            ElseIf (len = 0) Then
                Return 0
            End If

            Dim c As Integer = read()
            If (c = -1) Then
                Return -1
            End If
            b(off) = c

            Dim i As Integer = 1
            Try
                While (i < len)
                    c = read()
                    If (c = -1) Then
                        Exit While
                    End If
                    b(off + i) = c
                    i += 1
                End While
            Catch ee As IOException
            End Try
            Return i
        End Function

        Private Function inRange() As Boolean ' throws IOException {
            Dim pos As Long = _position
            For i As Integer = 0 To byteRange.Length / 2
                If (byteRange(i * 2) <= pos AndAlso byteRange(i * 2) + byteRange(i * 2 + 1) > pos) Then
                    Return True
                End If
            Next
            Return False
        End Function

        Private Sub nextAvailable() 'throws IOException {
            While (Not inRange())
                _position += 1
                If (MyBase.read() < 0) Then
                    Exit While
                End If
            End While

        End Sub

        Public Function toByteArray() As Byte() ' throws IOException 
            Dim byteOS As New ByteArrayOutputStream()
            Dim buffer() As Byte = Array.CreateInstance(GetType(Byte), 1024)
            Dim c As Integer
            c = Me.read(buffer)
            While (c <> -1)
                byteOS.write(buffer, 0, c)
                c = Me.read(buffer)
            End While
            Return byteOS.toByteArray()
        End Function

    End Class

End Namespace

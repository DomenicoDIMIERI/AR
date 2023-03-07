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
'import java.io.IOException;
'import java.io.InputStream;
'import java.io.OutputStream;
Imports System.IO

Namespace org.apache.pdfbox.encryption

    '/**
    ' * This class is an implementation of the alleged RC4 algorithm.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.8 $
    ' */
    Public Class ARCFour
        Private salt() As Integer
        Private b As Integer
        Private c As Integer

        '/**
        ' * Constructor.
        ' *
        ' */
        Public Sub New()
            ReDim Me.salt(256 - 1)
        End Sub

        '/**
        ' * This will reset the key to be used.
        ' *
        ' * @param key The RC4 key used during encryption.
        '*/
        Public Sub setKey(ByVal key() As Byte)
            b = 0
            c = 0

            If (key.Length < 1 OrElse key.Length > 32) Then
                Throw New ArgumentOutOfRangeException("number of bytes must be between 1 and 32")
            End If

            For i As Integer = 0 To Me.salt.Length - 1
                Me.salt(i) = i
            Next

            Dim keyIndex As Integer = 0
            Dim saltIndex As Integer = 0
            For i As Integer = 0 To Me.salt.Length - 1
                saltIndex = (fixByte(key(keyIndex)) + salt(i) + saltIndex) Mod 256
                swap(salt, i, saltIndex)
                keyIndex = (keyIndex + 1) Mod key.Length
            Next
        End Sub

        '/**
        ' * Thie will ensure that the value for a byte >=0.
        ' *
        ' * @param aByte The byte to test against.
        ' *
        ' * @return A value >=0 and < 256
        ' */
        Private Shared Function fixByte(ByVal aByte As Integer) As Integer
            Return IIf(aByte < 0, 256 + aByte, aByte)
        End Function

        '/**
        ' * This will swap two values in an array.
        ' *
        ' * @param data The array to swap from.
        ' * @param firstIndex The index of the first element to swap.
        ' * @param secondIndex The index of the second element to swap.
        ' */
        Private Shared Sub swap(ByVal data() As Integer, ByVal firstIndex As Integer, ByVal secondIndex As Integer)
            Dim tmp As Integer = data(firstIndex)
            data(firstIndex) = data(secondIndex)
            data(secondIndex) = tmp
        End Sub

        '/**
        ' * This will encrypt and write the next byte.
        ' *
        ' * @param aByte The byte to encrypt.
        ' * @param output The stream to write to.
        ' *
        ' * @throws IOException If there is an error writing to the output stream.
        ' */
        Public Sub write(ByVal aByte As Byte, ByVal output As Stream) 'throws IOException
            b = (b + 1) Mod 256
            c = (salt(b) + c) Mod 256
            swap(salt, b, c)
            Dim saltIndex As Integer = (salt(b) + salt(c)) Mod 256
            output.Write({aByte Xor salt(saltIndex)}, 0, 1)
        End Sub

        '/**
        ' * This will encrypt and write the data.
        ' *
        ' * @param data The data to encrypt.
        ' * @param output The stream to write to.
        ' *
        ' * @throws IOException If there is an error writing to the output stream.
        ' */
        Public Sub write(ByVal data() As Byte, ByVal output As Stream) 'throws IOException
            For i As Integer = 0 To data.Length - 1
                write(data(i), output)
            Next
        End Sub

        '/**
        ' * This will encrypt and write the data.
        ' *
        ' * @param data The data to encrypt.
        ' * @param output The stream to write to.
        ' *
        ' * @throws IOException If there is an error writing to the output stream.
        ' */
        Public Sub write(ByVal data As Stream, ByVal output As Stream) 'throws IOException
            Dim buffer() As Byte
            ReDim buffer(1024 - 1)
            Dim amountRead As Integer = 0
            amountRead = data.Read(buffer, 0, 1 + UBound(buffer))
            While (amountRead > 0)
                write(buffer, 0, amountRead, output)
                amountRead = data.Read(buffer, 0, 1 + UBound(buffer))
            End While
        End Sub

        '/**
        ' * This will encrypt and write the data.
        ' *
        ' * @param data The data to encrypt.
        ' * @param offset The offset into the array to start reading data from.
        ' * @param len The number of bytes to attempt to read.
        ' * @param output The stream to write to.
        ' *
        ' * @throws IOException If there is an error writing to the output stream.
        ' */
        Public Sub write(ByVal data() As Byte, ByVal offset As Integer, ByVal len As Integer, ByVal output As Stream) 'throws IOException
            For i As Integer = offset To offset + len - 1
                write(data(i), output)
            Next
        End Sub

    End Class

End Namespace

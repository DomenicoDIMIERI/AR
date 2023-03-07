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

'import java.io.OutputStream;
'import java.io.IOException;

Imports System.IO

Namespace org.apache.pdfbox.io


    '/**
    ' * This is an n-bit output stream.  This means that you write data in n-bit chunks.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.4 $
    ' */
    Public Class NBitOutputStream

        Private bitsInChunk As Integer
        Private out As Stream

        Private currentByte As Integer
        Private positionInCurrentByte As Integer

        '/**
        ' * Constructor.
        ' *
        ' * @param os The output stream to write to.
        ' */
        Public Sub New(ByVal os As Stream)
            Me.out = os
            Me.currentByte = 0
            Me.positionInCurrentByte = 7
        End Sub

        '/**
        ' * This will write the next n-bits to the stream.
        ' *
        ' * @param chunk The next chunk of data to write.
        ' *
        ' * @throws IOException If there is an error writing the chunk.
        ' */
        Public Sub write(ByVal chunk As Integer) ' throws IOException
            Dim bitToWrite As Integer
            For i As Integer = (bitsInChunk - 1) To 0 Step -1
                bitToWrite = (chunk >> i) And &H1
                bitToWrite <<= positionInCurrentByte
                currentByte = currentByte Or bitToWrite
                positionInCurrentByte -= 1
                If (positionInCurrentByte < 0) Then
                    out.WriteByte(currentByte)
                    currentByte = 0
                    positionInCurrentByte = 7
                End If
            Next
        End Sub

        '/**
        ' * This will close the stream.
        ' *
        ' * @throws IOException if there is an error closing the stream.
        ' */
        Public Sub close() 'throws IOException
            If (positionInCurrentByte < 7) Then
                out.WriteByte(currentByte)
            End If
        End Sub

        '/** Getter for property bitsToRead.
        ' * @return Value of property bitsToRead.
        ' */
        Public Function getBitsInChunk() As Integer
            Return bitsInChunk
        End Function

        '/** Setter for property bitsToRead.
        ' * @param bitsInChunkValue New value of property bitsToRead.
        ' */
        Public Sub setBitsInChunk(ByVal bitsInChunkValue As Integer)
            bitsInChunk = bitsInChunkValue
        End Sub

    End Class

End Namespace

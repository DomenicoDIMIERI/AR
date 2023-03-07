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

'import java.io.InputStream;
'import java.io.IOException;

Imports System.IO

Namespace org.apache.pdfbox.io

    '/**
    ' * This is an n-bit input stream.  This means that you can read chunks of data
    ' * as any number of bits, not just 8 bits like the regular input stream.  Just set the
    ' * number of bits that you would like to read on each call.  The default is 8.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.4 $
    ' */
    Public Class NBitInputStream

        Private bitsInChunk As Integer
        Private [in] As Stream

        Private currentByte As Integer
        Private bitsLeftInCurrentByte As Integer

        '/**
        ' * Constructor.
        ' *
        ' * @param is The input stream to read from.
        ' */
        Public Sub New(ByVal [is] As Stream)
            Me.[in] = [is]
            Me.bitsLeftInCurrentByte = 0
            Me.bitsInChunk = 8
        End Sub

        '/**
        ' * This will unread some data.
        ' *
        ' * @param data The data to put back into the stream.
        ' */
        Public Sub unread(ByVal data As Integer)
            data <<= bitsLeftInCurrentByte
            Me.currentByte = Me.currentByte Or data
            Me.bitsLeftInCurrentByte += bitsInChunk
        End Sub

        '/**
        ' * This will read the next n bits from the stream and return the unsigned
        ' * value of  those bits.
        ' *
        ' * @return The next n bits from the stream.
        ' *
        ' * @throws IOException If there is an error reading from the underlying stream.
        ' */
        Public Function read() As Integer
            Dim retval As Integer = 0
            Dim i As Integer = 0
            While (i < bitsInChunk AndAlso retval <> -1)
                If (bitsLeftInCurrentByte = 0) Then
                    currentByte = [in].ReadByte()
                    bitsLeftInCurrentByte = 8
                End If
                If (currentByte = -1) Then
                    retval = -1
                Else
                    retval <<= 1
                    retval = retval Or ((currentByte >> (bitsLeftInCurrentByte - 1)) And &H1)
                    bitsLeftInCurrentByte -= 1
                End If
                i += 1
            End While
            Return retval
        End Function

        '/** Getter for property bitsToRead.
        ' * @return Value of property bitsToRead.
        ' */
        Public Function getBitsInChunk() As Integer
            Return Me.bitsInChunk
        End Function

        '/** Setter for property bitsToRead.
        ' * @param bitsInChunkValue New value of property bitsToRead.
        ' */
        Public Sub setBitsInChunk(ByVal bitsInChunkValue As Integer)
            Me.bitsInChunk = bitsInChunkValue
        End Sub

    End Class

End Namespace
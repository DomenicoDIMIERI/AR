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
'import java.util.ArrayList;

Imports System.IO
Namespace org.apache.pdfbox.io


    '/**
    ' * An implementation of the RandomAccess interface to store a pdf in memory.
    ' * The data will be stored in 16kb chunks organized in an ArrayList.  
    ' *
    ' */
    Public Class RandomAccessBuffer
        Implements RandomAccess

        ' chunk size is 16kb
        Private Const BUFFER_SIZE As Integer = 16384
        ' list containing all chunks
        Private bufferList As ArrayList = Nothing '(Of Byte())
        ' current chunk
        Private currentBuffer() As Byte
        ' current pointer to the whole buffer
        Private pointer As Integer
        ' current pointer for the current chunk
        Private currentBufferPointer As Integer
        ' size of the whole buffer
        Private size As Integer
        ' current chunk list index
        Private bufferListIndex As Integer
        ' maximum chunk list index
        Private bufferListMaxIndex As Integer

        '/**
        ' * Default constructor.
        ' */
        Public Sub New()
            ' starting with one chunk
            bufferList = New ArrayList ' ArrayList<byte[]>();
            ReDim currentBuffer(BUFFER_SIZE - 1)
            bufferList.add(currentBuffer)
            pointer = 0
            currentBufferPointer = 0
            size = 0
            bufferListIndex = 0
            bufferListMaxIndex = 0
        End Sub

        '/**
        ' * {@inheritDoc}
        ' */
        Public Sub close() Implements SequentialRead.close  'throws IOException
            currentBuffer = Nothing
            bufferList.clear()
            pointer = 0
            currentBufferPointer = 0
            size = 0
            bufferListIndex = 0
        End Sub

        '/**
        ' * {@inheritDoc}
        ' */
        Public Sub seek(ByVal position As Long) Implements RandomAccessRead.seek  ' throws IOException
            pointer = position
            ' calculate the chunk list index
            bufferListIndex = CInt(position / BUFFER_SIZE)
            currentBufferPointer = position Mod BUFFER_SIZE
            currentBuffer = bufferList.get(bufferListIndex)
        End Sub

        '/**
        ' * {@inheritDoc}
        ' */
        Public Function getPosition() As Long Implements RandomAccessRead.getPosition 'throws IOException {
            Return pointer
        End Function

        '/**
        ' * {@inheritDoc}
        ' */
        Public Function read() As Integer Implements RandomAccessRead.read  ' throws IOException
            If (pointer >= Me.size) Then Return -1
            If (currentBufferPointer >= BUFFER_SIZE) Then
                If (bufferListIndex >= bufferListMaxIndex) Then
                    Return -1
                Else
                    bufferListIndex += 1
                    currentBuffer = bufferList.get(bufferListIndex)
                    currentBufferPointer = 0
                End If
            End If
            pointer += 1
            currentBufferPointer += 1
            Return currentBuffer(currentBufferPointer - 1) And &HFF
        End Function

        '/**
        ' * {@inheritDoc}
        ' */
        Public Function read(ByVal b() As Byte, ByVal offset As Integer, ByVal length As Integer) As Integer Implements RandomAccess.read ' throws IOException
            If (pointer >= Me.size) Then Return 0
            Dim maxLength As Integer = Math.Min(length, Me.size - pointer)
            Dim remainingBytes As Integer = BUFFER_SIZE - currentBufferPointer
            If (maxLength >= remainingBytes) Then
                ' copy the first bytes from the current buffer
                Array.Copy(currentBuffer, currentBufferPointer, b, offset, remainingBytes)
                Dim newOffset As Integer = offset + remainingBytes
                Dim remainingBytes2Read As Integer = length - remainingBytes
                ' determine how many buffers are needed to get the remaining amount bytes
                Dim numberOfArrays As Integer = remainingBytes2Read / BUFFER_SIZE
                For i As Integer = 0 To numberOfArrays - 1
                    nextBuffer()
                    Array.Copy(currentBuffer, 0, b, newOffset, BUFFER_SIZE)
                    newOffset += BUFFER_SIZE
                Next
                remainingBytes2Read = remainingBytes2Read Mod BUFFER_SIZE
                ' are there still some bytes to be read?
                If (remainingBytes2Read > 0) Then
                    nextBuffer()
                    Array.Copy(currentBuffer, 0, b, newOffset, remainingBytes2Read)
                    currentBufferPointer += remainingBytes2Read
                End If
            Else
                Array.Copy(currentBuffer, currentBufferPointer, b, offset, maxLength)
                currentBufferPointer += maxLength
            End If
            pointer += maxLength
            Return maxLength
        End Function

        '/**
        ' * {@inheritDoc}
        ' */
        Public ReadOnly Property length() As Long Implements RandomAccessRead.length ' throws IOException
            Get
                Return size
            End Get
        End Property

        '/**
        ' * {@inheritDoc}
        ' */
        Public Sub write(ByVal b As Integer) Implements RandomAccess.write  ' throws IOException
            ' end of buffer reached?
            If (currentBufferPointer >= BUFFER_SIZE) Then
                If (pointer + BUFFER_SIZE >= Integer.MaxValue) Then
                    Throw New IOException("RandomAccessBuffer overflow")
                End If
                expandBuffer()
            End If
            currentBuffer(currentBufferPointer) = CByte(b) : currentBufferPointer += 1
            pointer += 1
            If (pointer > Me.size) Then
                Me.size = pointer
            End If
            ' end of buffer reached now?
            If (currentBufferPointer >= BUFFER_SIZE) Then
                If (pointer + BUFFER_SIZE >= Integer.MaxValue) Then
                    Throw New IOException("RandomAccessBuffer overflow")
                End If
                expandBuffer()
            End If
        End Sub

        '/**
        ' * {@inheritDoc}
        ' */
        Public Sub write(ByVal b() As Byte, ByVal offset As Integer, ByVal length As Integer) Implements RandomAccess.write  ' throws IOException
            Dim newSize As Integer = pointer + length
            Dim remainingBytes As Integer = BUFFER_SIZE - currentBufferPointer
            If (length >= remainingBytes) Then
                If (newSize > Integer.MaxValue) Then
                    Throw New IOException("RandomAccessBuffer overflow")
                End If
                ' copy the first bytes to the current buffer
                Array.Copy(b, offset, currentBuffer, currentBufferPointer, remainingBytes)
                Dim newOffset As Integer = offset + remainingBytes
                Dim remainingBytes2Write As Integer = length - remainingBytes
                ' determine how many buffers are needed for the remaining bytes
                Dim numberOfNewArrays As Integer = remainingBytes2Write / BUFFER_SIZE
                For i As Integer = 0 To numberOfNewArrays - 1
                    expandBuffer()
                    Array.Copy(b, newOffset, currentBuffer, currentBufferPointer, BUFFER_SIZE)
                    newOffset += BUFFER_SIZE
                Next
                ' are there still some bytes to be written?
                remainingBytes2Write -= numberOfNewArrays * BUFFER_SIZE
                If (remainingBytes2Write >= 0) Then
                    expandBuffer()
                    If (remainingBytes2Write > 0) Then
                        Array.Copy(b, newOffset, currentBuffer, currentBufferPointer, remainingBytes2Write)
                    End If
                    currentBufferPointer = remainingBytes2Write
                End If
            Else
                Array.Copy(b, offset, currentBuffer, currentBufferPointer, length)
                currentBufferPointer += length
            End If
            pointer += length
            If (pointer > Me.size) Then
                Me.size = pointer
            End If
        End Sub

        '/**
        ' * create a new buffer chunk and adjust all pointers and indices.
        ' */
        Private Sub expandBuffer()
            If (bufferListMaxIndex > bufferListIndex) Then
                ' there is already an existing chunk
                nextBuffer()
            Else
                ' create a new chunk and add it to the buffer
                Dim buffer() As Byte
                ReDim buffer(BUFFER_SIZE)
                currentBuffer = buffer
                bufferList.add(currentBuffer)
                currentBufferPointer = 0
                bufferListMaxIndex += 1
                bufferListIndex += 1
            End If
        End Sub

        '/**
        ' * switch to the next buffer chunk and reset the buffer pointer.
        ' */
        Private Sub nextBuffer()
            currentBufferPointer = 0
            bufferListIndex += 1
            currentBuffer = bufferList.get(bufferListIndex)
        End Sub
 
    End Class

End Namespace

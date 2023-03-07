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
'import java.io.EOFException;
Imports System.IO

Namespace org.apache.pdfbox.io

    '/**
    ' * A simple subclass that adds a few convience methods.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.6 $
    ' */
    Public Class PushBackInputStream
        Inherits FinSeA.Io.PushBackInputStream
        '/*
        ' * The current position in the file. 
        ' */
        Private offset As Integer = 0

        '/** In case provided input stream implements {@link RandomAccessRead} we hold
        ' *  a typed reference to it in order to support seek operations. */
        Private raInput As RandomAccessRead

        '/**
        ' * Constructor.
        ' *
        ' * @param input The input stream.
        ' * @param size The size of the push back buffer.
        ' *
        ' * @throws IOException If there is an error with the stream.
        ' */
        Public Sub New(ByVal input As Stream, ByVal size As Integer) ' throws IOException
            MyBase.New(input, size)
            If (input Is Nothing) Then
                Throw New IOException("Error: input was null")
            End If
            If (TypeOf (input) Is RandomAccessRead) Then
                raInput = input
            Else
                raInput = Nothing
            End If
        End Sub

        '/**
        ' * This will peek at the next byte.
        ' *
        ' * @return The next byte on the stream, leaving it as available to read.
        ' *
        ' * @throws IOException If there is an error reading the next byte.
        ' */
        Public Overridable Function peek() As Integer ' throws IOException
            Dim result As Integer = read()
            If (result <> -1) Then
                unread(result)
            End If
            Return result
        End Function

        '/**
        ' * Returns the current byte offset in the file.
        ' * @return the int byte offset
        ' */
        Public Function getOffset() As Integer
            Return offset
        End Function

        '/**
        ' * {@inheritDoc} 
        ' */
        Public Overrides Function read() As Integer ' throws IOException
            Dim retval As Integer = MyBase.read()
            If (retval <> -1) Then
                offset += 1
            End If
            Return retval
        End Function

        '/**
        ' * {@inheritDoc} 
        ' */
        Public Overrides Function read(ByVal b() As Byte) As Integer ' throws IOException
            Return Me.read(b, 0, b.Length)
        End Function

        '/**
        ' * {@inheritDoc} 
        ' */
        Public Overrides Function read(ByVal b() As Byte, ByVal off As Integer, ByVal len As Integer) As Integer ' throws IOException
            Dim retval As Integer = MyBase.read(b, off, len)
            If (retval <> -1) Then
                offset += retval
            End If
            Return retval
        End Function

        '/**
        ' * {@inheritDoc} 
        ' */
        Public Overrides Sub unread(ByVal b As Integer) ' throws IOException
            offset -= 1
            MyBase.unread(b)
        End Sub

        '/**
        ' * {@inheritDoc} 
        ' */
        Public Overrides Sub unread(ByVal b() As Byte) ' throws IOException
            Me.unread(b, 0, b.Length)
        End Sub

        '/**
        ' * {@inheritDoc} 
        ' */
        Public Overrides Sub unread(ByVal b() As Byte, ByVal off As Integer, ByVal len As Integer) ' throws IOException
            If (len > 0) Then
                offset -= len
                MyBase.unread(b, off, len)
            End If
        End Sub

        '/**
        ' * A simple test to see if we are at the end of the stream.
        ' *
        ' * @return true if we are at the end of the stream.
        ' *
        ' * @throws IOException If there is an error reading the next byte.
        ' */
        Public Overridable Function isEOF() As Boolean ' throws IOException
            Dim peek As Integer = Me.peek()
            Return peek = -1
        End Function

        '/**
        ' * This is a method used to fix PDFBox issue 974661, the PDF parsing code needs
        ' * to know if there is at least x amount of data left in the stream, but the available()
        ' * method returns how much data will be available without blocking.  PDFBox is willing to
        ' * block to read the data, so we will first fill the internal buffer.
        ' *
        ' * @throws IOException If there is an error filling the buffer.
        ' */
        Public Sub fillBuffer() 'throws IOException
            Dim bufferLength As Integer = buf.length
            Dim tmpBuffer() As Byte
            ReDim tmpBuffer(bufferLength - 1)
            Dim amountRead As Integer = 0
            Dim totalAmountRead As Integer = 0
            While (amountRead <> -1 AndAlso totalAmountRead < bufferLength)
                amountRead = Me.read(tmpBuffer, totalAmountRead, bufferLength - totalAmountRead)
                If (amountRead <> -1) Then
                    totalAmountRead += amountRead
                End If
            End While
            Me.unread(tmpBuffer, 0, totalAmountRead)
        End Sub

        '/**
        ' * Reads a given number of bytes from the underlying stream.
        ' * @param length the number of bytes to be read
        ' * @return a byte array containing the bytes just read
        ' * @throws IOException if an I/O error occurs while reading data
        ' */
        Public Function readFully(ByVal length As Integer) As Byte() ' throws IOException
            Dim data() As Byte
            ReDim data(length - 1)
            Dim pos As Integer = 0
            While (pos < length)
                Dim amountRead As Integer = read(data, pos, length - pos)
                If (amountRead < 0) Then
                    Throw New EndOfStreamException("Premature end of file")
                End If
                pos += amountRead
            End While
            Return data
        End Function

        '/** Allows to seek to another position within stream in case the underlying
        ' *  stream implements {@link RandomAccessRead}. Otherwise an {@link IOException}
        ' *  is thrown.
        ' *  
        ' *  Pushback buffer is cleared before seek operation by skipping over all bytes
        ' *  of buffer.
        ' *  
        ' *  @param newOffset  new position within stream from which to read next
        ' *  
        ' *  @throws IOException if underlying stream does not implement {@link RandomAccessRead}
        ' *                      or seek operation on underlying stream was not successful
        ' */
        Public Overrides Function Seek(newOffset As Long, origin As SeekOrigin) As Long
            Return Me.Seek(newOffset)
        End Function

        Public Overridable Overloads Function seek(ByVal newOffset As Long) As Long
            'Return MyBase.Seek(offset, origin)
            If (raInput Is Nothing) Then Throw New IOException("Provided stream of type " & [in].GetType().Name & " is not seekable.")

            ' clear unread buffer by skipping over all bytes of buffer
            Dim unreadLength As Integer = buf.Length - pos
            If (unreadLength > 0) Then
                skip(unreadLength)
            End If
            raInput.seek(newOffset)
            offset = newOffset
            Return offset
        End Function

    End Class

End Namespace

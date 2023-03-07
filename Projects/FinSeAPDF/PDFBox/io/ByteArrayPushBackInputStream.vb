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

'import java.io.ByteArrayInputStream;
'import java.io.IOException;
'import java.io.InputStream;
Imports System.IO

Namespace org.apache.pdfbox.io

    '/**
    ' * PushBackInputStream for byte arrays.
    ' *
    ' * The inheritance from PushBackInputStream is only to avoid the
    ' * introduction of an interface with all PushBackInputStream
    ' * methods. The parent PushBackInputStream is not used in any way and
    ' * all methods are overridden. (Thus when adding new methods to PushBackInputStream
    ' * override them in this class as well!)
    ' * unread() is limited to the number of bytes already read from this stream (i.e.
    ' * the current position in the array). This limitation usually poses no problem
    ' * to a parser, but allows for some optimization since only one array has to
    ' * be dealt with.
    ' *
    ' * Note: This class is not thread safe. Clients must provide synchronization
    ' * if needed.
    ' *
    ' * Note: Calling unread() after mark() will cause (part of) the unread data to be
    ' * read again after reset(). Thus do not call unread() between mark() and reset().
    ' *
    ' * @author Andreas Weiss (andreas.weiss@switzerland.org)
    ' * @version $Revision: 1.2 $
    ' */
    Public Class ByteArrayPushBackInputStream
        Inherits PushBackInputStream

        Private data() As Byte
        Private datapos As Integer
        Private datalen As Integer
        Private save As Integer

        ' dummy for base class constructor
        Private Shared ReadOnly DUMMY As New MemoryStream()

        '/**
        ' * Constructor.
        ' * @param input Data to read from. Note that calls to unread() will
        ' * modify this array! If this is not desired, pass a copy.
        ' *
        ' * @throws IOException If there is an IO error.
        ' */
        Public Sub New(ByVal input() As Byte) ' throws IOException
            MyBase.New(DUMMY, 1)
            data = input
            datapos = 0
            save = datapos
            If (input Is Nothing) Then
                datalen = 0
            Else
                datalen = input.Length
            End If
        End Sub

        '/**
        ' * This will peek at the next byte.
        ' *
        ' * @return The next byte on the stream, leaving it as available to read.
        ' */
        Public Overrides Function peek() As Integer
            Try
                ' convert negative values to 128..255
                Return (data(datapos) + &H100) And &HFF
            Catch ex As IndexOutOfRangeException
                ' could check this before, but this is a rare case
                ' and this method is called sufficiently often to justify this
                ' optimization
                Return -1
            End Try
        End Function

        '/**
        ' * A simple test to see if we are at the end of the stream.
        ' *
        ' * @return true if we are at the end of the stream.
        ' */
        Public Overrides Function isEOF() As Boolean
            Return datapos >= datalen
        End Function

        '/**
        ' * Save the state of this stream.
        ' * @param readlimit Has no effect.
        ' * @see InputStream#mark(int)
        ' */
        Public Overrides Sub mark(ByVal readlimit As Integer)
            If (False) Then
                readlimit += 1 ' avoid unused param warning
            End If
            save = datapos
        End Sub

        '/**
        ' * Check if mark is supported.
        ' * @return Always true.
        ' * @see InputStream#markSupported()
        ' */
        Public Overrides Function markSupported() As Boolean
            Return True
        End Function

        '/**
        ' * Restore the state of this stream to the last saveState call.
        ' * @see InputStream#reset()
        ' */
        Public Overrides Sub reset()
            datapos = save
        End Sub

        '/** Available bytes.
        ' * @see InputStream#available()
        ' * @return Available bytes.
        ' */
        Public Overrides Function available() As Integer
            Dim av As Integer = datalen - datapos
            Return IIf(av > 0, av, 0)
        End Function

        '/** Totally available bytes in the underlying array.
        ' * @return Available bytes.
        ' */
        Public Function size() As Integer
            Return datalen
        End Function

        '/**
        ' * Pushes back a byte.
        ' * After this method returns, the next byte to be read will have the value (byte)by.
        ' * @param by the int value whose low-order byte is to be pushed back.
        ' * @throws IOException - If there is not enough room in the buffer for the byte.
        ' * @see java.io.PushbackInputStream#unread(int)
        ' */
        Public Overrides Sub unread(ByVal by As Integer) ' throws IOException
            If (datapos = 0) Then
                Throw New IOException("ByteArrayParserInputStream.unread(int): cannot unread 1 byte at buffer position " & datapos)
            End If
            datapos -= 1
            data(datapos) = by
        End Sub

        '/**
        ' * Pushes back a portion of an array of bytes by copying it to the
        ' * front of the pushback buffer. After this method returns, the next byte
        ' * to be read will have the value b[off], the byte after that will have
        ' * the value b[off+1], and so forth.
        ' * @param buffer the byte array to push back.
        ' * @param off the start offset of the data.
        ' * @param len the number of bytes to push back.
        ' * @throws IOException If there is not enough room in the pushback buffer
        ' * for the specified number of bytes.
        ' * @see java.io.PushbackInputStream#unread(byte[], int, int)
        ' */
        Public Overrides Sub unread(ByVal buffer() As Byte, ByVal off As Integer, ByVal len As Integer) ' throws IOException
            If (len <= 0 OrElse off >= buffer.Length) Then Return
            If (off < 0) Then
                off = 0
            End If
            If (len > buffer.Length) Then
                len = buffer.Length
            End If
            localUnread(buffer, off, len)
        End Sub

        '/**
        ' * Pushes back a portion of an array of bytes by copying it to the
        ' * front of the pushback buffer. After this method returns, the next byte
        ' * to be read will have the value buffer(0), the byte after that will have
        ' * the value buffer(1), and so forth.
        ' * @param buffer the byte array to push back.
        ' * @throws IOException If there is not enough room in the pushback buffer
        ' * for the specified number of bytes.
        ' * @see java.io.PushbackInputStream#unread(byte[])
        ' */
        Public Overrides Sub unread(ByVal buffer() As Byte) ' throws IOException
            localUnread(buffer, 0, buffer.Length)
        End Sub

        '/**
        ' * Pushes back a portion of an array of bytes by copying it to the
        ' * front of the pushback buffer. After this method returns, the next byte
        ' * to be read will have the value buffer[off], the byte after that will have
        ' * the value buffer[off+1], and so forth.
        ' * Internal method that assumes off and len to be valid.
        ' * @param buffer the byte array to push back.
        ' * @param off the start offset of the data.
        ' * @param len the number of bytes to push back.
        ' * @throws IOException If there is not enough room in the pushback buffer
        ' * for the specified number of bytes.
        ' * @see java.io.PushbackInputStream#unread(byte[], int, int)
        ' */
        Private Sub localUnread(ByVal buffer() As Byte, ByVal off As Integer, ByVal len As Integer) ' throws IOException
            If (datapos < len) Then
                Throw New IOException("ByteArrayParserInputStream.unread(int): cannot unread " & len & " bytes at buffer position " & datapos)
            End If
            datapos -= len
            Array.Copy(buffer, off, data, datapos, len)
        End Sub

        '/**
        ' * Read a byte.
        ' * @see InputStream#read()
        ' * @return Byte read or -1 if no more bytes are available.
        ' */
        Public Overrides Function read() As Integer
            Try
                ' convert negative values to 128..255
                Return (data(datapos) + &H100) And &HFF : datapos += 1
            Catch ex As IndexOutOfRangeException
                ' could check this before, but this is a rare case
                ' and this method is called sufficiently often to justify this
                ' optimization
                datapos = datalen
                Return -1
            End Try
        End Function

        '/**
        ' * Read a number of bytes.
        ' * @see InputStream#read(byte[])
        ' * @param buffer the buffer into which the data is read.
        ' * @return the total number of bytes read into the buffer, or -1 if there
        ' * is no more data because the end of the stream has been reached.
        ' */
        Public Overrides Function read(ByVal buffer() As Byte) As Integer
            Return localRead(buffer, 0, buffer.Length)
        End Function

        '/**
        ' * Read a number of bytes.
        ' * @see InputStream#read(byte[], int, int)
        ' * @param buffer the buffer into which the data is read.
        ' * @param off the start offset in array buffer at which the data is written.
        ' * @param len the maximum number of bytes to read.
        ' * @return the total number of bytes read into the buffer, or -1 if there
        ' * is no more data because the end of the stream has been reached.
        ' */
        Public Overrides Function read(ByVal buffer() As Byte, ByVal off As Integer, ByVal len As Integer) As Integer
            If (len <= 0 OrElse off >= buffer.Length) Then Return 0
            If (off < 0) Then
                off = 0
            End If
            If (len > buffer.Length) Then
                len = buffer.Length
            End If
            Return localRead(buffer, off, len)
        End Function


        '/**
        ' * Read a number of bytes. Internal method that assumes off and len to be
        ' * valid.
        ' * @see InputStream#read(byte[], int, int)
        ' * @param buffer the buffer into which the data is read.
        ' * @param off the start offset in array buffer at which the data is written.
        ' * @param len the maximum number of bytes to read.
        ' * @return the total number of bytes read into the buffer, or -1 if there
        ' * is no more data because the end of the stream has been reached.
        ' */
        Public Function localRead(ByVal buffer() As Byte, ByVal off As Integer, ByVal len As Integer)
            If (len = 0) Then
                Return 0 ' must return 0 even if at end!
            ElseIf (datapos >= datalen) Then
                Return -1
            Else
                Dim newpos As Integer = datapos + len
                If (newpos > datalen) Then
                    newpos = datalen
                    len = newpos - datapos
                End If
                Array.Copy(data, datapos, buffer, off, len)
                datapos = newpos
                Return len
            End If
        End Function

        '/**
        ' * Skips over and discards n bytes of data from this input stream.
        ' * The skip method may, for a variety of reasons, end up skipping over some
        ' * smaller number of bytes, possibly 0. This may result from any of a number
        ' * of conditions; reaching end of file before n bytes have been skipped is
        ' * only one possibility. The actual number of bytes skipped is returned.
        ' * If n is negative, no bytes are skipped.
        ' * @param num the number of bytes to be skipped.
        ' * @return the actual number of bytes skipped.
        ' * @see InputStream#skip(long)
        ' */
        Public Overrides Function skip(ByVal num As Long) As Long
            If (num <= 0) Then
                Return 0
            Else
                Dim newpos As Integer = datapos + num
                If (newpos >= datalen) Then
                    num = datalen - datapos
                    datapos = datalen
                Else
                    datapos = newpos
                End If
                Return num
            End If
        End Function

        '/** Position the stream at a given index. Positioning the stream
        ' * at position size() will cause the next call to read() to return -1.
        ' *
        ' * @param newpos Position in the underlying array. A negative value will be
        ' * interpreted as 0, a value greater than size() as size().
        ' * @return old position.
        ' */
        Public Overrides Function seek(ByVal newpos As Long) As Long
            If (newpos < 0) Then
                newpos = 0
            ElseIf (newpos > datalen) Then
                newpos = datalen
            End If
            Dim oldpos As Integer = pos
            pos = newpos
            Return oldpos
        End Function

    End Class

End Namespace

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
    ' * This class allows a section of a RandomAccessFile to be accessed as an
    ' * input stream.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.5 $
    ' */
    Public Class RandomAccessFileInputStream
        Inherits FinSeA.Io.InputStream

        Private file As RandomAccess
        Private currentPosition As Long
        Private endPosition As Long

        '/**
        ' * Constructor.
        ' *
        ' * @param raFile The file to read the data from.
        ' * @param startPosition The position in the file that this stream starts.
        ' * @param length The length of the input stream.
        ' */
        Public Sub New(ByVal raFile As RandomAccess, ByVal startPosition As Long, ByVal length As Long)
            file = raFile
            currentPosition = startPosition
            endPosition = currentPosition + length
        End Sub

        Public Overrides Function available() As Integer
            Return (endPosition - currentPosition)
        End Function

        Public Overrides Sub close()
            '//do nothing because we want to leave the random access file open.
        End Sub

        Public Overrides Function read() As Integer ' throws IOException
            SyncLock file
                Dim retval As Integer = -1
                If (currentPosition < endPosition) Then
                    file.seek(currentPosition)
                    currentPosition += 1
                    retval = file.read()
                End If
                Return retval
            End SyncLock
        End Function

        Public Overrides Function read(ByVal b() As Byte, ByVal offset As Integer, ByVal length As Integer) As Integer ' throws IOException
            'only allow a read of the amount available.
            If (length > available()) Then
                length = available()
            End If
            Dim amountRead As Integer = -1
            'only read if there are bytes actually available, otherwise
            'return -1 if the EOF has been reached.
            If (available() > 0) Then
                SyncLock file
                    file.seek(currentPosition)
                    amountRead = file.read(b, offset, length)
                End SyncLock
            End If
            'update the current cursor position.
            If (amountRead > 0) Then
                currentPosition += amountRead
            End If
            Return amountRead
        End Function

        Public Overrides Function skip(ByVal amountToSkip As Long) As Long
            Dim amountSkipped As Long = Math.Min(amountToSkip, available())
            currentPosition += amountSkipped
            Return amountSkipped
        End Function

    End Class

End Namespace

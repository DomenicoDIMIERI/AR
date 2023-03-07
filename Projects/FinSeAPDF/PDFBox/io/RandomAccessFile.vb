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
'import java.io.File;
'import java.io.FileNotFoundException;
'import java.io.IOException;
Imports System.IO
Imports FinSeA.org.apache.pdfbox.io

Namespace org.apache.pdfbox.io


    '/**
    ' * An interface to allow temp PDF data to be stored in a scratch
    ' * file on the disk to reduce memory consumption.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.2 $
    ' */
    Public Class RandomAccessFile
        Implements RandomAccess

        Private ras As FinSeA.Io.RandomAccessFile
        Private _tmpFile As String
        Private _p2 As String

        '/**
        ' * Constructor.
        ' *
        ' * @param file The file to write the data to.
        ' * @param mode The writing mode.
        ' * @throws FileNotFoundException If the file cannot be created.
        ' */
        Public Sub New(ByVal file As FinSeA.Io.File, ByVal mode As String) ' throws FileNotFoundException
            ras = New FinSeA.Io.RandomAccessFile(file, mode) 'new java.io.RandomAccessFile(file, mode);
        End Sub

        Sub New(tmpFile As String, p2 As String)
            ' TODO: Complete member initialization 
            _tmpFile = tmpFile
            _p2 = p2
        End Sub

        Public Sub close() Implements RandomAccessRead.close 'throws IOException
            ras.Close()
        End Sub

        Public Sub seek(ByVal position As Long) Implements RandomAccessRead.seek 'throws IOException
            ras.seek(position)
        End Sub

        Public Function getPosition() As Long Implements RandomAccessRead.getPosition 'throws IOException {
            Return ras.getFilePointer
        End Function

        Public Function read() As Integer Implements RandomAccess.read 'throws IOException
            Return ras.ReadByte
        End Function

        Public Function read(ByVal b() As Byte, ByVal offset As Integer, ByVal length As Integer) As Integer Implements RandomAccess.read  ' throws IOException
            Return ras.Read(b, offset, length)
        End Function

        Public ReadOnly Property length() As Long Implements RandomAccessRead.length ' throws IOException
            Get
                Return ras.Length()
            End Get
        End Property

        Public Sub write(ByVal b() As Byte, ByVal offset As Integer, ByVal length As Integer) Implements RandomAccess.write ' throws IOException
            ras.Write(b, offset, length)
        End Sub

        Public Sub write(ByVal b As Integer) Implements RandomAccess.write  'throws IOException
            ras.WriteByte(b)
        End Sub

        'Public Function Read(Of T)() As T Implements SequentialRead.read
        '    Return Convert.ChangeType(Me.read, GetType(T))
        'End Function
    End Class

End Namespace

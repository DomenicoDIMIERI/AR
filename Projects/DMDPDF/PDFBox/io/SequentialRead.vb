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
Imports System.IO

Namespace org.apache.pdfbox.io

    ''' <summary>
    ''' An interface allowing sequential read operations.
    ''' </summary>
    ''' <remarks></remarks>
    Public Interface SequentialRead


        ''' <summary>
        ''' Release resources that are being held.
        ''' </summary>
        ''' <remarks></remarks>
        Sub close() 'throws IOException;


        ''' <summary>
        ''' Read a single byte of data.
        ''' </summary>
        ''' <returns>The byte of data that is being read.</returns>
        ''' <remarks></remarks>
        Function read() As Integer ' throws IOException;

        'Function Read(Of T)() As T
        '/**
        ' * Read a buffer of data.
        ' *
        ' * @param b The buffer to write the data to.
        ' * @param offset Offset into the buffer to start writing.
        ' * @param length The amount of data to attempt to read.
        ' * @return The number of bytes that were actually read.
        ' * @throws IOException If there was an error while reading the data.
        ' */
        Function read(ByVal b() As Byte, ByVal offset As Integer, ByVal length As Integer) As Integer ' throws IOException;

    End Interface

End Namespace

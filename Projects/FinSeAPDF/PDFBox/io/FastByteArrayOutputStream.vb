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
'import java.io.ByteArrayOutputStream;
Imports System.IO

Namespace org.apache.pdfbox.io


    '/**
    ' * An byte array output stream that allows direct access to the byte array.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.4 $
    ' */
    Public Class FastByteArrayOutputStream
        Inherits MemoryStream ' ByteArrayOutputStream

        '/**
        ' * Constructor.
        ' *
        ' * @param size An initial size of the stream.
        ' */
        Public Sub New(ByVal size As Integer)
            MyBase.New(size)
        End Sub

        '/**
        ' * This will get the underlying byte array.
        ' *
        ' * @return The underlying byte array at this moment in time.
        ' */
        Public Function getByteArray() As Byte()
            Return MyBase.GetBuffer
        End Function

    End Class

End Namespace

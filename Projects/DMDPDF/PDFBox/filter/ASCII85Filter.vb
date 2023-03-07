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

'import org.apache.pdfbox.io.ASCII85InputStream;
'import org.apache.pdfbox.io.ASCII85OutputStream;

'import org.apache.pdfbox.cos.COSDictionary;

Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.io
Imports FinSeA.Io

Namespace org.apache.pdfbox.filter

    '/**
    ' * This is the used for the ASCIIHexDecode filter.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.8 $
    ' */
    Public Class ASCII85Filter
        Implements Filter

        '/**
        ' * {@inheritDoc}
        ' */
        Public Sub decode(ByVal compressedData As InputStream, ByVal result As OutputStream, ByVal options As COSDictionary, ByVal filterIndex As Integer) Implements Filter.decode
            Dim [is] As ASCII85InputStream = Nothing
            Try
                [is] = New ASCII85InputStream(compressedData)
                Dim buffer() As Byte
                ReDim buffer(1024 - 1)
                Dim amountRead As Integer = 0
                amountRead = [is].read(buffer, 0, 1024)
                While (amountRead > 0)
                    result.Write(buffer, 0, amountRead)
                    amountRead = [is].read(buffer, 0, 1024)
                End While
                result.Flush()
            Finally
                If ([is] IsNot Nothing) Then
                    [is].close()
                End If
            End Try
        End Sub

        '/**
        ' * {@inheritDoc}
        ' */
        Public Sub encode(ByVal rawData As InputStream, ByVal result As OutputStream, ByVal options As COSDictionary, ByVal filterIndex As Integer) Implements Filter.encode
            Dim os As New ASCII85OutputStream(result)
            Dim buffer() As Byte
            ReDim buffer(1024)
            Dim amountRead As Integer = rawData.Read(buffer, 0, 1024)
            While (amountRead > 0)
                os.write(buffer, 0, amountRead)
                amountRead = rawData.Read(buffer, 0, 1024)
            End While
            os.close()
            result.Flush()
        End Sub

    End Class

End Namespace
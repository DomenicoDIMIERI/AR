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
'import java.io.UnsupportedEncodingException;

Namespace org.apache.pdfbox.util


    Public Class StringUtil

        '/**
        ' * 
        ' *
        ' * It is an workaround for variable initialisations outside of functions.
        ' */ 

        ''' <summary>
        ''' Converts a string to it ISO-8859-1 byte sequence
        ''' </summary>
        ''' <param name="s"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function getBytes(ByVal s As String) As Byte()
            ' Try
            Return Sistema.Strings.GetBytes(s, "ISO-8859-1")
            'Catch e As UnsupportedEncodingException
            'Throw New RuntimeException("Unsupported Encoding", e)
            'End Try
        End Function

    End Class

End Namespace

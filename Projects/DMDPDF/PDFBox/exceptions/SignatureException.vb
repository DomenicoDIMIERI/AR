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

Namespace org.apache.pdfbox.exceptions

    '/**
    ' * An exception that indicates a problem during the signing process.
    ' *
    ' * @author Thomas Chojecki
    ' * @version $Revision: $
    ' */
    Public Class SignatureException
        Inherits System.Exception

        Public Const WRONG_PASSWORD As Integer = 1
        Public Const UNSUPPORTED_OPERATION As Integer = 2
        Public Const CERT_PATH_CHECK_INVALID As Integer = 3
        Public Const NO_SUCH_ALGORITHM As Integer = 4
        Public Const INVALID_PAGE_FOR_SIGNATURE As Integer = 5
        Public Const VISUAL_SIGNATURE_INVALID As Integer = 6

        Private no As Integer

        '/**
        ' * Constructor.
        ' *
        ' * @param msg A msg to go with this exception.
        ' */
        Public Sub New(ByVal msg As String)
            MyBase.New(msg)
        End Sub

        '/**
        ' * Constructor.
        ' * 
        ' * @param errno A error number to fulfill this exception
        ' * @param msg A msg to go with this exception.
        ' */
        Public Sub New(ByVal errno As Integer, ByVal msg As String)
            MyBase.New(msg)
            Me.no = errno
        End Sub

        '/**
        ' * Constructor.
        ' * 
        ' * @param e The exception that should be encapsulate.
        ' */
        Public Sub New(ByVal e As System.Exception)
            MyBase.New(e.Message, e)
        End Sub

        '/**
        ' * Constructor.
        ' * 
        ' * @param errno A error number to fulfill this exception
        ' * @param e The exception that should be encapsulate.
        ' */
        Public Sub New(ByVal errno As Integer, ByVal e As System.Exception)
            MyBase.New(e.Message, e)
            Me.no = errno
        End Sub

        '/**
        ' * A error number to fulfill this exception
        ' * 
        ' * @return the error number if available, otherwise 0
        ' */
        Public Function getErrNo() As Integer
            Return Me.no
        End Function

    End Class

End Namespace
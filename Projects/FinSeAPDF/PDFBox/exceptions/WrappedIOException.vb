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

Namespace org.apache.pdfbox.exceptions

    '/**
    ' * An simple class that allows a sub exception to be stored.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.4 $
    ' */
    Public Class WrappedIOException
        Inherits System.IO.IOException

        '/**
        ' * constructor comment.
        ' *
        ' * @param e The root exception that caused this exception.
        ' */
        Public Sub New(ByVal e As System.Exception)
            MyBase.New(e.Message, e)
        End Sub

        '/**
        ' * constructor comment.
        ' *
        ' * @param message Descriptive text for the exception.
        ' * @param e The root exception that caused this exception.
        ' */
        Public Sub New(ByVal message As String, ByVal e As System.Exception)
            MyBase.New(message, e)
        End Sub

    End Class

End Namespace

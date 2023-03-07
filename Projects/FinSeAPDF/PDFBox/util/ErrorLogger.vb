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
Namespace org.apache.pdfbox.util

    '/**
    ' * This class deals with some logging that is not handled by the log4j replacement.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.3 $
    ' */
    Public Class ErrorLogger

        '/**
        ' * Utility class, should not be instantiated.
        ' *
        ' */
        Private Sub New()
        End Sub

        '/**
        ' * Log an error message.  This is only used for log4j replacement and
        ' * should never be used when writing code.
        ' *
        ' * @param errorMessage The error message.
        ' */
        Public Shared Sub log(ByVal errorMessage As String)
            Debug.Print(errorMessage)
        End Sub

        '/**
        ' * Log an error message.  This is only used for log4j replacement and
        ' * should never be used when writing code.
        ' *
        ' * @param errorMessage The error message.
        ' * @param t The exception.
        ' */
        Public Shared Sub log(ByVal errorMessage As String, ByVal t As System.Exception)
            Debug.Print(errorMessage)
            Debug.Print(errorMessage.ToString)
        End Sub
    End Class

End Namespace
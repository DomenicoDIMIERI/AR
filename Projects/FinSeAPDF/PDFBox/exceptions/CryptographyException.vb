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
    ' * An exception that indicates that something has gone wrong during a
    ' * cryptography operation.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.4 $
    ' */
    Public Class CryptographyException
        Inherits System.Exception

        Private embedded As System.Exception

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
        ' * @param e The root exception that caused this exception.
        ' */
        Public Sub New(ByVal e As System.Exception)
            MyBase.New(e.Message)
            Me.setEmbedded(e)
        End Sub

        '/**
        ' * This will get the exception that caused this exception.
        ' *
        ' * @return The embedded exception if one exists.
        ' */
        Public Function getEmbedded() As System.Exception
            Return Me.embedded
        End Function

        '/**
        ' * This will set the exception that caused this exception.
        ' *
        ' * @param e The sub exception.
        ' */
        Private Sub setEmbedded(ByVal e As System.Exception)
            Me.embedded = e
        End Sub

    End Class

End Namespace

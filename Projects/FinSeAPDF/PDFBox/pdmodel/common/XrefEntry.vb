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

Namespace org.apache.pdfbox.pdmodel.common

    '/**
    ' *
    ' * @author adam
    ' */
    Public Class XrefEntry
        Private objectNumber As Integer = 0
        Private byteOffset As Integer = 0
        Private generation As Integer = 0
        Private inUse As Boolean = True

        Public Sub New()
        End Sub

        Public Sub New(ByVal objectNumber As Integer, ByVal byteOffset As Integer, ByVal generation As Integer, ByVal inUse As String)
            Me.objectNumber = objectNumber
            Me.byteOffset = byteOffset
            Me.generation = generation
            Me.inUse = "n".Equals(inUse)
        End Sub

        Public Function getByteOffset() As Integer
            Return byteOffset
        End Function

    End Class

End Namespace

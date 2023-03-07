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
Namespace org.apache.pdfbox.pdfviewer

    '/**
    ' * This is a simple class that will contain an index and a value.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.3 $
    ' */
    Public Class ArrayEntry
        Private index As Integer
        Private value As Object

        '/**
        ' * This will get the value for this entry.
        ' *
        ' * @return The value for this entry.
        ' */
        Public Function getValue() As Object
            Return value
        End Function

        '/**
        ' * This will set the value for this entry.
        ' *
        ' * @param val the new value for this entry.
        ' */
        Public Sub setValue(ByVal val As Object)
            Me.value = val
        End Sub

        '/**
        ' * This will get the index of the array entry.
        ' *
        ' * @return The 0-based index into the array
        ' */
        Public Function getIndex() As Integer
            Return index
        End Function

        '/**
        ' * This will set the index value.
        ' *
        ' * @param i The new index value.
        ' */
        Public Sub setIndex(ByVal i As Integer)
            index = i
        End Sub
    End Class

End Namespace

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
Imports FinSeA.org.apache.pdfbox.cos


Namespace org.apache.pdfbox.pdfviewer

    '/**
    ' * This is a simple class that will contain a key and a value.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.3 $
    ' */
    Public Class MapEntry
        Private key As Object
        Private value As Object

        '/**
        ' * Get the key for this entry.
        ' *
        ' * @return The entry's key.
        ' */
        Public Function getKey() As Object
            Return key
        End Function

        '/**
        ' * This will set the key for this entry.
        ' *
        ' * @param k the new key for this entry.
        ' */
        Public Sub setKey(ByVal k As Object)
            key = k
        End Sub

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
        ' * This will output a string representation of this class.
        ' *
        ' * @return A string representation of this class.
        ' */
        Public Overrides Function toString() As String
            Dim retval As String = ""
            If (TypeOf (key) Is COSName) Then
                retval = DirectCast(key, COSName).getName()
            Else
                retval = "" & key
            End If
            Return retval
        End Function


    End Class

End Namespace

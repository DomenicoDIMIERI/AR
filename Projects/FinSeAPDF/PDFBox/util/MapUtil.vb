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
'import java.util.Map;

Namespace org.apache.pdfbox.util

    '/**
    '* This class with handle some simple Map operations.
    '*
    '* @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    '* @version $Revision: 1.2 $
    '*/
    Public Class MapUtil

        Private Sub New()
            'utility class
        End Sub

        '/**
        ' * Generate a unique key for the map based on a prefix.
        ' *
        ' * @param map The map to look for existing keys.
        ' * @param prefix The prefix to use when generating the key.
        ' * @return The new unique key that does not currently exist in the map.
        ' */
        Public Shared Function getNextUniqueKey(ByVal map As Map, ByVal prefix As String) As String
            Dim counter As Integer = 0
            While (map IsNot Nothing AndAlso map.get(prefix + counter) IsNot Nothing)
                counter += 1
            End While
            Return prefix + counter
        End Function

    End Class

End Namespace

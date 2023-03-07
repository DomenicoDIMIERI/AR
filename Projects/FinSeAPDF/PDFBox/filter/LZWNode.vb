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

'import java.util.HashMap;
'import java.util.Map;

Namespace org.apache.pdfbox.filter

    '/**
    ' * This is the used for the LZWDecode filter.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.4 $
    ' */
    Public Class LZWNode
        Private code As Integer
        Private subNodes As New HashMap(Of Byte, LZWNode)

        Public Sub New(ByVal codeValue As Integer)
            Me.code = codeValue
        End Sub

        '/**
        ' * This will get the number of children.
        ' *
        ' * @return The number of children.
        ' */
        Public Function childCount() As Integer
            Return Me.subNodes.size()
        End Function

        '/**
        ' * This will set the node for a particular byte.
        ' *
        ' * @param b The byte for that node.
        ' * @param node The node to add.
        ' */
        Public Sub setNode(ByVal b As Byte, ByVal node As LZWNode)
            Me.subNodes.put(b, node)
        End Sub

        '/**
        ' * This will get the node that is a direct sub node of this node.
        ' *
        ' * @param data The byte code to the node.
        ' *
        ' * @return The node at that value if it exists.
        ' */
        Public Function getNode(ByVal data As Byte) As LZWNode
            Return Me.subNodes.get(data)
        End Function


        '/**
        ' * This will traverse the tree until it gets to the sub node.
        ' * This will return null if the node does not exist.
        ' *
        ' * @param data The path to the node.
        ' *
        ' * @return The node that resides at the data path.
        ' */
        Public Function getNode(ByVal data() As Byte) As LZWNode
            Dim current As LZWNode = Me
            Dim i As Integer = 0
            While (i < data.Length AndAlso current IsNot Nothing)
                current = current.getNode(data(i))
                i += 1
            End While
            Return current
        End Function

        '/** Getter for property code.
        ' * @return Value of property code.
        ' */
        Public Function getCode() As Integer
            Return Me.code
        End Function

    End Class

End Namespace
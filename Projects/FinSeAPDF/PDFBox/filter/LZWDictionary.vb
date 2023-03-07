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
'import java.util.HashMap;
'import java.util.Map;
Imports System.IO

Namespace org.apache.pdfbox.filter

    '/**
    ' * This is the used for the LZWDecode filter.  This represents the dictionary mappings
    ' * between codes and their values.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.4 $
    ' */
    Public NotInheritable Class LZWDictionary
        Private codeToData As New HashMap(Of Integer, Byte())
        Private root As New LZWNode(0)
        Private buffer() As Byte = {0, 0, 0, 0, 0, 0, 0, 0} '= new byte[8];
        Private bufferNextWrite As Integer = 0
        Private nextCode As Integer = 258
        Private codeSize As Integer = 9

        Private previous As LZWNode = Nothing
        Private current As LZWNode = Me.root

        '/**
        ' * This will get the value for the code.  It will return null if the code is not
        ' * defined.
        ' *
        ' * @param code The key to the data.
        ' *
        ' * @return The data that is mapped to the code.
        ' */
        Public Function getData(ByVal code As Integer) As Byte()
            Dim result() As Byte = codeToData.get(code)
            If (result Is Nothing AndAlso code < 256) Then
                addRootNode(code)
                result = codeToData.get(code)
            End If
            Return result
        End Function

        '/**
        ' * This will take a visit from a byte[].  This will create new code entries as
        ' * necessary.
        ' *
        ' * @param data The byte to get a visit from.
        ' *
        ' * @throws IOException If there is an error visiting this data.
        ' */
        Public Sub visit(ByVal data() As Byte) ' throws IOException
            For i As Integer = 0 To data.Length - 1
                visit(data(i))
            Next
        End Sub

        '/**
        ' * This will take a visit from a byte.  This will create new code entries as
        ' * necessary.
        ' *
        ' * @param data The byte to get a visit from.
        ' *
        ' * @throws IOException If there is an error visiting this data.
        ' */
        Public Sub visit(ByVal data As Byte) 'throws IOException
            If (buffer.Length = bufferNextWrite) Then
                Dim nextBuffer() As Byte
                ReDim nextBuffer(2 * buffer.Length - 1)
                Array.Copy(buffer, 0, nextBuffer, 0, buffer.Length)
                buffer = nextBuffer
            End If
            buffer(bufferNextWrite) = data
            bufferNextWrite += 1
            previous = current
            current = current.getNode(data)
            If (current Is Nothing) Then
                Dim code As Integer
                If (previous Is root) Then
                    code = data And &HFF
                Else
                    code = nextCode
                    nextCode += 1
                End If
                current = New LZWNode(code)
                previous.setNode(data, current)
                Dim sav() As Byte
                ReDim sav(bufferNextWrite - 1)
                Array.Copy(buffer, 0, sav, 0, bufferNextWrite)
                codeToData.put(code, sav)

                '/**
                'System.out.print( "Adding " + code + "='" );
                'for( int i=0; i<bufferNextWrite; i++ )
                '{
                '    String hex = Integer.toHexString( ((buffer(i)&0xFF );
                '        If (Hex.length() <= 1) Then
                '    {
                '        hex = "0" + hex;
                '    }
                '    if( i != bufferNextWrite -1 )
                '    {
                '        hex += " ";
                '    }
                '    System.out.print( hex.toUpperCase() );
                '}
                'System.out.println( "'" );
                '**/
                bufferNextWrite = 0
                current = root
                visit(data)
                resetCodeSize()
            End If
        End Sub

        '/**
        ' * This will get the next code that will be created.
        ' *
        ' * @return The next code to be created.
        ' */
        Public Function getNextCode() As Integer
            Return Me.nextCode
        End Function

        '/**
        ' * This will get the size of the code in bits, 9, 10, or 11.
        ' *
        ' * @return The size of the code in bits.
        ' */
        Public Function getCodeSize() As Integer
            Return Me.codeSize
        End Function

        '/**
        ' * This will determine the code size.
        ' */
        Private Sub resetCodeSize()
            If (Me.nextCode < 512) Then
                Me.codeSize = 9
            ElseIf (Me.nextCode < 1024) Then
                Me.codeSize = 10
            ElseIf (Me.nextCode < 2048) Then
                Me.codeSize = 11
            Else
                Me.codeSize = 12
            End If
        End Sub

        '/**
        ' * This will clear the internal buffer that the dictionary uses.
        ' */
        Public Sub clear()
            Me.bufferNextWrite = 0
            Me.current = Me.root
            Me.previous = Nothing
        End Sub

        '/**
        ' * This will folow the path to the data node.
        ' *
        ' * @param data The path to the node.
        ' *
        ' * @return The node that resides at that path.
        ' */
        Public Function getNode(ByVal data() As Byte) As LZWNode
            Dim result As LZWNode = root.getNode(data)
            If (result Is Nothing AndAlso data.Length = 1) Then
                result = addRootNode(data(0))
            End If
            Return result
        End Function

        Private Function addRootNode(ByVal b As Byte) As LZWNode
            Dim code As Integer = b And &HFF
            Dim result As New LZWNode(code)
            root.setNode(b, result)
            codeToData.put(code, {b})
            Return result
        End Function

    End Class

End Namespace
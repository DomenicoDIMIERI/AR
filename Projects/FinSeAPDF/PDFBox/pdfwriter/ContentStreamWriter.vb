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
'import java.io.OutputStream;

'import java.util.Iterator;
'import java.util.List;
'import java.util.Map;

'import org.apache.pdfbox.cos.COSArray;
'import org.apache.pdfbox.cos.COSBase;
'import org.apache.pdfbox.cos.COSBoolean;
'import org.apache.pdfbox.cos.COSDictionary;
'import org.apache.pdfbox.cos.COSFloat;
'import org.apache.pdfbox.cos.COSInteger;
'import org.apache.pdfbox.cos.COSName;
'import org.apache.pdfbox.cos.COSString;

'import org.apache.pdfbox.util.ImageParameters;
'import org.apache.pdfbox.util.PDFOperator;
Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.util
Imports FinSeA.Io

Namespace org.apache.pdfbox.pdfwriter


    '/**
    ' * A class that will take a list of tokens and write out a stream with them.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.8 $
    ' */
    Public Class ContentStreamWriter
        Private output As OutputStream

        ''' <summary>
        ''' space character.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly SPACE As Byte() = {32}

        ''' <summary>
        ''' standard line separator
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly EOL As Byte() = {&HA}

        ''' <summary>
        ''' This will create a new content stream writer.
        ''' </summary>
        ''' <param name="out">The stream to write the data to.</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal out As Stream) 'OutputStream 
            Me.output = out
        End Sub

        '/**
        ' * This will write out the list of tokens to the stream.
        ' *
        ' * @param tokens The tokens to write to the stream.
        ' * @param start The start index into the list of tokens.
        ' * @param end The end index into the list of tokens.
        ' * @throws IOException If there is an error writing to the stream.
        ' */
        Public Sub writeTokens(ByVal tokens As List, ByVal start As Integer, ByVal [end] As Integer) ' throws IOException
            For i As Integer = start To [end] - 1
                Dim o As Object = tokens.[get](i)
                writeObject(o)
                'write a space between each object.
                output.WriteByte(32)
            Next
            output.Flush()
        End Sub

        Private Sub writeObject(ByVal o As Object) 'throws IOException
            If (TypeOf (o) Is COSString) Then
                DirectCast(o, COSString).writePDF(output)
            ElseIf (TypeOf (o) Is COSFloat) Then
                DirectCast(o, COSFloat).writePDF(output)
            ElseIf (TypeOf (o) Is COSInteger) Then
                DirectCast(o, COSInteger).writePDF(output)
            ElseIf (TypeOf (o) Is COSBoolean) Then
                DirectCast(o, COSBoolean).writePDF(output)
            ElseIf (TypeOf (o) Is COSName) Then
                DirectCast(o, COSName).writePDF(output)
            ElseIf (TypeOf (o) Is COSArray) Then
                Dim array As COSArray = o
                output.Write(COSWriter.ARRAY_OPEN)
                For i As Integer = 0 To array.size() - 1
                    writeObject(array.get(i))
                    output.Write(SPACE)
                Next
                output.Write(COSWriter.ARRAY_CLOSE)
            ElseIf (TypeOf (o) Is COSDictionary) Then
                Dim obj As COSDictionary = o
                output.Write(COSWriter.DICT_OPEN)
                For Each entry As Map.Entry(Of COSName, COSBase) In obj.entrySet()
                    If (entry.Value IsNot Nothing) Then
                        writeObject(entry.Key)
                        output.Write(SPACE)
                        writeObject(entry.Value)
                        output.Write(SPACE)
                    End If
                Next
                output.Write(COSWriter.DICT_CLOSE)
                output.Write(SPACE)

            ElseIf (TypeOf (o) Is PDFOperator) Then
                Dim op As PDFOperator = o
                If (op.getOperation().equals("BI")) Then
                    output.Write(Sistema.Strings.GetBytes("BI", "ISO-8859-1"))
                    Dim params As ImageParameters = op.getImageParameters()
                    Dim dic As COSDictionary = params.getDictionary()
                    For Each key As COSName In dic.keySet()
                        Dim value As Object = dic.getDictionaryObject(key)
                        key.writePDF(output)
                        output.Write(SPACE)
                        writeObject(value)
                        output.Write(EOL)
                    Next
                    output.Write(Sistema.Strings.GetBytes("ID", "ISO-8859-1"))
                    output.Write(EOL)
                    output.Write(op.getImageData())
                Else
                    output.Write(Sistema.Strings.GetBytes(op.getOperation(), "ISO-8859-1"))
                    output.Write(EOL)
                End If
            Else
                Throw New IOException("Error:Unknown type in content stream:" & o.ToString)
            End If
        End Sub

        '/**
        ' * This will write out the list of tokens to the stream.
        ' *
        ' * @param tokens The tokens to write to the stream.
        ' * @throws IOException If there is an error writing to the stream.
        ' */
        Public Sub writeTokens(ByVal tokens As List) ' throws IOException
            writeTokens(tokens, 0, tokens.size())
        End Sub
    End Class

End Namespace

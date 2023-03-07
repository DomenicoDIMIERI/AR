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

'import java.util.ArrayList;
'import java.util.Iterator;

'import org.apache.pdfbox.cos.COSArray;
'import org.apache.pdfbox.cos.COSBase;
'import org.apache.pdfbox.cos.COSDocument;
'import org.apache.pdfbox.cos.COSInteger;
'import org.apache.pdfbox.cos.COSName;
'import org.apache.pdfbox.cos.COSStream;
'import org.apache.pdfbox.persistence.util.COSObjectKey;
Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.persistence.util

Namespace org.apache.pdfbox.pdfparser


    '/**
    ' * This will parse a PDF 1.5 (or better) Xref stream and
    ' * extract the xref information from the stream.
    ' *
    ' *  @author <a href="mailto:justinl@basistech.com">Justin LeFebvre</a>
    ' *  @version $Revision: 1.0 $
    ' */
    Public Class PDFXrefStreamParser
        Inherits BaseParser

        Private stream As COSStream
        Private xrefTrailerResolver As XrefTrailerResolver

        '/**
        ' * Constructor.
        ' *
        ' * @since 1.3.0
        ' * @param strm The stream to parse.
        ' * @param doc The document for the current parsing.
        ' * @param forceParsing flag to skip malformed or otherwise unparseable
        ' *                     input where possible
        ' * @param resolver resolver to read the xref/trailer information
        ' *
        ' * @throws IOException If there is an error initializing the stream.
        ' */
        Public Sub New(ByVal strm As COSStream, ByVal doc As COSDocument, ByVal forceParsing As Boolean, ByVal resolver As XrefTrailerResolver) 'throws IOException
            MyBase.New(strm.getUnfilteredStream(), forceParsing)
            setDocument(doc)
            stream = strm
            Me.xrefTrailerResolver = resolver
        End Sub

        '/**
        ' * Parses through the unfiltered stream and populates the xrefTable HashMap.
        ' * @throws IOException If there is an error while parsing the stream.
        ' */
        Public Sub parse() 'throws IOException
            Try
                Dim xrefFormat As COSArray = stream.getDictionaryObject(COSName.W)
                Dim indexArray As COSArray = stream.getDictionaryObject(COSName.INDEX)
                '/*
                ' * If Index doesn't exist, we will use the default values.
                ' */
                If (indexArray Is Nothing) Then
                    indexArray = New COSArray()
                    indexArray.add(COSInteger.ZERO)
                    indexArray.add(stream.getDictionaryObject(COSName.SIZE))
                End If

                Dim objNums As New ArrayList(Of NInteger)

                '/*
                ' * Populates objNums with all object numbers available
                ' */
                Dim indexIter As Global.System.Collections.Generic.IEnumerator(Of COSBase) = indexArray.iterator()
                While (indexIter.MoveNext())
                    Dim objID As Integer = DirectCast(indexIter.Current, COSInteger).intValue()
                    Dim size As Integer = DirectCast(indexIter.Current, COSInteger).intValue()
                    For i As Integer = 0 To size - 1
                        objNums.Add(objID + i)
                    Next
                End While
                Dim objIter As Global.System.Collections.Generic.IEnumerator(Of NInteger) = objNums.GetEnumerator()
                '/*
                ' * Calculating the size of the line in bytes
                ' */
                Dim w0 As Integer = xrefFormat.getInt(0)
                Dim w1 As Integer = xrefFormat.getInt(1)
                Dim w2 As Integer = xrefFormat.getInt(2)
                Dim lineSize As Integer = w0 + w1 + w2

                While (pdfSource.available() > 0 AndAlso objIter.MoveNext())
                    Dim currLine() As Byte = Array.CreateInstance(GetType(Byte), lineSize)
                    pdfSource.read(currLine)

                    Dim type As Integer = 0
                    '/*
                    ' * Grabs the number of bytes specified for the first column in
                    ' * the W array and stores it.
                    ' */
                    For i As Integer = 0 To w0 - 1
                        type += (currLine(i) And &HFF) << ((w0 - i - 1) * 8)
                    Next
                    'Need to remember the current objID
                    Dim objID As NInteger = objIter.Current
                    '/*
                    ' * 3 different types of entries.
                    ' */
                    Select Case (type)
                        Case 0
                            '/*
                            ' * Skipping free objects
                            ' */
                            'break;
                        Case 1
                            Dim offset As Integer = 0
                            For i As Integer = 0 To w1 - 1
                                offset += (currLine(i + w0) And &HFF) << ((w1 - i - 1) * 8)
                            Next
                            Dim genNum As Integer = 0
                            For i As Integer = 0 To w2 - 1
                                genNum += (currLine(i + w0 + w1) And &HFF) << ((w2 - i - 1) * 8)
                            Next
                            Dim objKey As New COSObjectKey(objID.Value, genNum)
                            xrefTrailerResolver.setXRef(objKey, offset)
                        Case 2
                            '/*
                            ' * object stored in object stream; 2nd argument is object number of object stream;
                            ' * 3rd argument index of object within object stream
                            ' * 
                            ' * For sequential PDFParser we do not need this information
                            ' * because
                            ' * These objects are handled by the dereferenceObjects() method
                            ' * since they're only pointing to object numbers
                            ' * 
                            ' * However for XRef aware parsers we have to know which objects contain
                            ' * object streams. We will store this information in normal xref mapping
                            ' * table but add object stream number with minus sign in order to
                            ' * distinguish from file offsets
                            ' */
                            Dim objstmObjNr As Integer = 0
                            For i As Integer = 0 To w1 - 1
                                objstmObjNr += (currLine(i + w0) And &HFF) << ((w1 - i - 1) * 8)
                            Next
                            Dim objKey As New COSObjectKey(objID.Value, 0)
                            xrefTrailerResolver.setXRef(objKey, -objstmObjNr)
                        Case Else
                            Exit While
                    End Select
                End While
        finally
                pdfSource.close()
            End Try
        End Sub

    End Class

End Namespace
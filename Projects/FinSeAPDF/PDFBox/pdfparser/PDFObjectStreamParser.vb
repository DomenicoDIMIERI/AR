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
'import java.util.List;

'import org.apache.commons.logging.Log;
'import org.apache.commons.logging.LogFactory;
'import org.apache.pdfbox.cos.COSBase;
'import org.apache.pdfbox.cos.COSDocument;
'import org.apache.pdfbox.cos.COSInteger;
'import org.apache.pdfbox.cos.COSObject;
'import org.apache.pdfbox.cos.COSStream;
Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos

Namespace org.apache.pdfbox.pdfparser


    '/**
    ' * This will parse a PDF 1.5 object stream and extract all of the objects from the stream.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.6 $
    ' */
    Public Class PDFObjectStreamParser
        Inherits BaseParser
        '/**
        ' * Log instance.
        ' */
        'private static final Log LOG =
        '    LogFactory.getLog(PDFObjectStreamParser.class);

        Private streamObjects As List(Of COSObject) = Nothing
        Private objectNumbers As List(Of NLong) = Nothing
        Private stream As COSStream

        '/**
        ' * Constructor.
        ' *
        ' * @since Apache PDFBox 1.3.0
        ' * @param strm The stream to parse.
        ' * @param doc The document for the current parsing.
        ' * @param forceParsing flag to skip malformed or otherwise unparseable
        ' *                     input where possible
        ' * @throws IOException If there is an error initializing the stream.
        ' */
        Public Sub New(ByVal strm As COSStream, ByVal doc As COSDocument, ByVal forceParsing As Boolean) 'throws IOException
            MyBase.New(strm.getUnfilteredStream(), forceParsing)
            setDocument(doc)
            stream = strm
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param strm The stream to parse.
        ' * @param doc The document for the current parsing.
        ' *
        ' * @throws IOException If there is an error initializing the stream.
        ' */
        Public Sub New(ByVal strm As COSStream, ByVal doc As COSDocument) 'throws IOException
            Me.New(strm, doc, FORCE_PARSING)
        End Sub

        '/**
        ' * This will parse the tokens in the stream.  This will close the
        ' * stream when it is finished parsing.
        ' *
        ' * @throws IOException If there is an error while parsing the stream.
        ' */
        Public Sub parse() 'throws IOException
            Try
                'need to first parse the header.
                Dim numberOfObjects As Integer = stream.getInt("N")
                objectNumbers = New ArrayList(Of NLong)(numberOfObjects)
                streamObjects = New ArrayList(Of COSObject)(numberOfObjects)
                For i As Integer = 0 To numberOfObjects - 1
                    Dim objectNumber As Long = readObjectNumber()
                    Dim offset As Long = readLong()
                    objectNumbers.Add(objectNumber)
                Next
                Dim [object] As COSObject = Nothing
                Dim cosObject As COSBase = Nothing
                Dim objectCounter As Integer = 0
                cosObject = parseDirObject()
                While (cosObject IsNot Nothing)
                    [object] = New COSObject(cosObject)
                    [object].setGenerationNumber(COSInteger.ZERO)
                    Dim objNum As COSInteger = COSInteger.get(objectNumbers.get(objectCounter).intValue())
                    [object].setObjectNumber(objNum)
                    streamObjects.Add([object])
                    If (LOG.isDebugEnabled()) Then
                        LOG.debug("parsed=" & [object].toString)
                    End If
                    objectCounter += 1
                    cosObject = parseDirObject()
                End While
            Finally
                pdfSource.close()
            End Try
        End Sub

        '/**
        ' * This will get the objects that were parsed from the stream.
        ' *
        ' * @return All of the objects in the stream.
        ' */
        Public Function getObjects() As List(Of COSObject)
            Return streamObjects
        End Function

    End Class

End Namespace

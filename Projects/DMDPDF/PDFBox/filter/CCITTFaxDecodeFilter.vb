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
'import java.io.InputStream;
'import java.io.OutputStream;

'import org.apache.commons.logging.Log;
'import org.apache.commons.logging.LogFactory;
'import org.apache.pdfbox.cos.COSArray;
'import org.apache.pdfbox.cos.COSBase;
'import org.apache.pdfbox.cos.COSDictionary;
'import org.apache.pdfbox.cos.COSName;
'import org.apache.pdfbox.io.IOUtils;
'import org.apache.pdfbox.io.ccitt.CCITTFaxG31DDecodeInputStream;
'import org.apache.pdfbox.io.ccitt.FillOrderChangeInputStream;
Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.io
Imports FinSeA.org.apache.pdfbox.io.ccitt
Imports FinSeA.Io

Namespace org.apache.pdfbox.filter

    '/**
    ' * This is a filter for the CCITTFax Decoder.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @author Marcel Kammer
    ' * @author Paul King
    ' * @version $Revision: 1.13 $
    ' */
    Public Class CCITTFaxDecodeFilter
        Implements Filter

        '/**
        ' * Log instance.
        ' */
        'private static final Log log = LogFactory.getLog(CCITTFaxDecodeFilter.class);

        '/**
        ' * Constructor.
        ' */
        Public Sub New()
        End Sub

        '/**
        ' * {@inheritDoc}
        ' */
        Public Sub decode(ByVal compressedData As InputStream, ByVal result As OutputStream, ByVal options As COSDictionary, ByVal filterIndex As Integer) Implements Filter.decode
            Dim decodeP As COSBase = options.getDictionaryObject(COSName.DECODE_PARMS, COSName.DP)
            Dim decodeParms As COSDictionary = Nothing
            If (TypeOf (decodeP) Is COSDictionary) Then
                decodeParms = decodeP
            ElseIf (TypeOf (decodeP) Is COSArray) Then
                decodeParms = DirectCast(decodeP, COSArray).getObject(filterIndex)
            End If
            Dim length As Integer = options.getInt(COSName.LENGTH, -1)
            Dim compressed() As Byte = Nothing
            If (length <> -1) Then
                ReDim compressed(length - 1)
                Dim written As Integer = IOUtils.populateBuffer(compressedData, compressed)
                If (written <> compressed.Length) Then
                    Debug.Print("Buffer for compressed data did not match the length of the actual compressed data")
                End If
            Else
                ' inline images don't provide the length of the stream so that
                ' we have to read until the end of the stream to find out the length
                ' the streams inline images are stored in are mostly small ones
                compressed = IOUtils.toByteArray(compressedData)
            End If
            Dim cols As Integer = decodeParms.getInt(COSName.COLUMNS, 1728)
            Dim rows As Integer = decodeParms.getInt(COSName.ROWS, 0)
            Dim height As Integer = options.getInt(COSName.HEIGHT, COSName.H, 0)
            If (rows > 0 AndAlso height > 0) Then
                ' ensure that rows doesn't contain implausible data, see PDFBOX-771
                rows = Math.Min(rows, height)
            Else
                ' at least one of the values has to have a valid value
                rows = Math.Max(rows, height)
            End If
            Dim k As Integer = decodeParms.getInt(COSName.K, 0)
            Dim arraySize As Integer = (cols + 7) / 8 * rows
            Dim faxDecoder As New TIFFFaxDecoder(1, cols, rows)
            ' TODO possible options??
            Dim tiffOptions As Integer = 0
            If (k = 0) Then
                Dim [in] As InputStream = New CCITTFaxG31DDecodeInputStream(New ByteArrayInputStream(compressed), cols)
                [in] = New FillOrderChangeInputStream([in]) 'Decorate to change fill order
                IOUtils.copy([in], result)
                [in].Close()
            ElseIf (k > 0) Then
                Dim decompressed() As Byte
                ReDim decompressed(arraySize - 1)
                faxDecoder.decode2D(decompressed, compressed, 0, rows, tiffOptions)
                result.Write(decompressed, 0, 1 + UBound(decompressed))
            ElseIf (k < 0) Then
                Dim decompressed() As Byte = Nothing
                ReDim compressed(arraySize - 1)
                faxDecoder.decodeT6(decompressed, compressed, 0, rows, tiffOptions)
                result.Write(decompressed, 0, 1 + UBound(decompressed))
            End If
        End Sub

        '/**
        ' * {@inheritDoc}
        ' */
        Public Sub encode(ByVal rawData As InputStream, ByVal result As OutputStream, ByVal options As COSDictionary, ByVal filterIndex As Integer) Implements Filter.encode
            'log.warn("CCITTFaxDecode.encode is not implemented yet, skipping this stream.");
            Throw New NotImplementedException
        End Sub

    End Class

End Namespace

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

'import java.util.Collection;
'import java.util.HashMap;
'import java.util.Map;

imports FinSeA . org.apache.pdfbox.cos

Namespace org.apache.pdfbox.filter

    '/**
    ' * This will contain manage all the different types of filters that are available.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.13 $
    ' */
    Public Class FilterManager

        Private filters As New HashMap(Of COSName, Filter)

        '/**
        ' * Constructor.
        ' */
        Public Sub New()
            Dim flateFilter As New FlateFilter()
            Dim dctFilter As New DCTFilter()
            Dim ccittFaxFilter As New CCITTFaxDecodeFilter()
            Dim lzwFilter As New LZWFilter()
            Dim asciiHexFilter As New ASCIIHexFilter()
            Dim ascii85Filter As New ASCII85Filter()
            Dim runLengthFilter As New RunLengthDecodeFilter()
            Dim cryptFilter As New CryptFilter()
            Dim jpxFilter As New JPXFilter()
            Dim jbig2Filter As New JBIG2Filter()

            addFilter(COSName.FLATE_DECODE, flateFilter)
            addFilter(COSName.FLATE_DECODE_ABBREVIATION, flateFilter)
            addFilter(COSName.DCT_DECODE, dctFilter)
            addFilter(COSName.DCT_DECODE_ABBREVIATION, dctFilter)
            addFilter(COSName.CCITTFAX_DECODE, ccittFaxFilter)
            addFilter(COSName.CCITTFAX_DECODE_ABBREVIATION, ccittFaxFilter)
            addFilter(COSName.LZW_DECODE, lzwFilter)
            addFilter(COSName.LZW_DECODE_ABBREVIATION, lzwFilter)
            addFilter(COSName.ASCII_HEX_DECODE, asciiHexFilter)
            addFilter(COSName.ASCII_HEX_DECODE_ABBREVIATION, asciiHexFilter)
            addFilter(COSName.ASCII85_DECODE, ascii85Filter)
            addFilter(COSName.ASCII85_DECODE_ABBREVIATION, ascii85Filter)
            addFilter(COSName.RUN_LENGTH_DECODE, runLengthFilter)
            addFilter(COSName.RUN_LENGTH_DECODE_ABBREVIATION, runLengthFilter)
            addFilter(COSName.CRYPT, cryptFilter)
            addFilter(COSName.JPX_DECODE, jpxFilter)
            addFilter(COSName.JBIG2_DECODE, jbig2Filter)
        End Sub

        '/**
        ' * This will get all of the filters that are available in the system.
        ' *
        ' * @return All available filters in the system.
        ' */
        Public Function getFilters() As CCollection(Of Filter)
            Return filters.values()
        End Function

        '/**
        ' * This will add an available filter.
        ' *
        ' * @param filterName The name of the filter.
        ' * @param filter The filter to use.
        ' */
        Public Sub addFilter(ByVal filterName As COSName, ByVal filter As Filter)
            filters.put(filterName, filter)
        End Sub

        '/**
        ' * This will get a filter by name.
        ' *
        ' * @param filterName The name of the filter to retrieve.
        ' *
        ' * @return The filter that matches the name.
        ' *
        ' * @throws IOException If the filter could not be found.
        ' */
        Public Function getFilter(ByVal filterName As COSName) As Filter
            Dim filter As Filter = filters.get(filterName)
            If (filter Is Nothing) Then
                Throw New System.IO.IOException("Unknown stream filter:" & filterName.toString)
            End If
            Return filter
        End Function

        '/**
        ' * This will get a filter by name.
        ' *
        ' * @param filterName The name of the filter to retrieve.
        ' *
        ' * @return The filter that matches the name.
        ' *
        ' * @throws IOException If the filter could not be found.
        ' */
        Public Function getFilter(ByVal filterName As String) As Filter
            Return getFilter(COSName.getPDFName(filterName))
        End Function

    End Class

End Namespace

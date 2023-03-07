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

'import java.io.BufferedInputStream;
'import java.io.BufferedOutputStream;
'import java.io.ByteArrayInputStream;
'import java.io.InputStream;
'import java.io.IOException;
'import java.io.OutputStream;

'import java.util.List;

'import org.apache.pdfbox.filter.Filter;
'import org.apache.pdfbox.filter.FilterManager;

'import org.apache.pdfbox.pdfparser.PDFStreamParser;

'import org.apache.pdfbox.exceptions.COSVisitorException;

'import org.apache.pdfbox.io.RandomAccess;
'import org.apache.pdfbox.io.RandomAccessFileInputStream;
'import org.apache.pdfbox.io.RandomAccessFileOutputStream;

Imports System.IO
Imports FinSeA.Io
Imports FinSeA.org.apache.pdfbox.filter
Imports FinSeA.org.apache.pdfbox.pdfparser
Imports FinSeA.org.apache.pdfbox.exceptions
Imports FinSeA.org.apache.pdfbox.io

Namespace org.apache.pdfbox.cos

    '/**
    ' * This class represents a stream object in a PDF document.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.41 $
    ' */
    Public Class COSStream
        Inherits COSDictionary

        Private Const BUFFER_SIZE As Integer = 16384

        Private file As RandomAccess
        '/**
        ' * The stream with all of the filters applied.
        ' */
        Private filteredStream As RandomAccessFileOutputStream

        '/**
        ' * The stream with no filters, this contains the useful data.
        ' */
        Private unFilteredStream As RandomAccessFileOutputStream

        '/**
        ' * Constructor.  Creates a new stream with an empty dictionary.
        ' *
        ' * @param storage The intermediate storage for the stream.
        ' */
        Public Sub New(ByVal storage As RandomAccess)
            MyBase.New()
            Me.file = storage
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param dictionary The dictionary that is associated with this stream.
        ' * @param storage The intermediate storage for the stream.
        ' */
        Public Sub New(ByVal dictionary As COSDictionary, ByVal storage As RandomAccess)
            MyBase.New(dictionary)
            Me.file = storage
        End Sub

        '/**
        ' * This will replace this object with the data from the new object.  This
        ' * is used to easily maintain referential integrity when changing references
        ' * to new objects.
        ' *
        ' * @param stream The stream that have the new values in it.
        ' */
        Public Sub replaceWithStream(ByVal stream As COSStream)
            Me.clear()
            Me.addAll(stream)
            file = stream.file
            filteredStream = stream.filteredStream
            unFilteredStream = stream.unFilteredStream
        End Sub

        '/**
        ' * This will get the scratch file associated with this stream.
        ' *
        ' * @return The scratch file where this stream is being stored.
        ' */
        Public Overridable Function getScratchFile() As RandomAccess
            Return file
        End Function

        '/**
        ' * This will get all the tokens in the stream.
        ' *
        ' * @return All of the tokens in the stream.
        ' *
        ' * @throws IOException If there is an error parsing the stream.
        ' */
        Public Overridable Function getStreamTokens() As List  '  throws IOException
            Dim parser As New PDFStreamParser(Me)
            parser.parse()
            Return parser.getTokens()
        End Function

        '/**
        ' * This will get the stream with all of the filters applied.
        ' *
        ' * @return the bytes of the physical (endoced) stream
        ' *
        ' * @throws IOException when encoding/decoding causes an exception
        ' */
        Public Overridable Function getFilteredStream() As InputStream ' throws IOException
            If (filteredStream Is Nothing) Then
                doEncode()
            End If
            Dim position As Integer = filteredStream.getPosition()
            Dim length As Integer = filteredStream.getLength()

            Dim input As New RandomAccessFileInputStream(file, position, length)
            Return New BufferedInputStream(input, BUFFER_SIZE)
        End Function

        '/**
        ' * This will get the length of the encoded stream.
        ' * 
        ' * @return the length of the encoded stream as long
        ' *
        ' * @throws IOException 
        ' */
        Public Overridable Function getFilteredLength() As Integer 'throws IOException
            If (filteredStream Is Nothing) Then
                doEncode()
            End If
            Return filteredStream.getLength()
        End Function

        '/**
        ' * This will get the logical content stream with none of the filters.
        ' *
        ' * @return the bytes of the logical (decoded) stream
        ' *
        ' * @throws IOException when encoding/decoding causes an exception
        ' */
        Public Overridable Function getUnfilteredStream() As InputStream 'throws IOException
            Dim retval As Stream = Nothing
            If (unFilteredStream Is Nothing) Then
                doDecode()
            End If

            'if unFilteredStream is still null then this stream has not been
            'created yet, so we should return null.
            If (unFilteredStream IsNot Nothing) Then
                Dim position As Integer = unFilteredStream.getPosition()
                Dim length As Integer = unFilteredStream.getLength()
                Dim input As New RandomAccessFileInputStream(file, position, length)
                retval = New BufferedInputStream(input, BUFFER_SIZE)
            Else
                '            // We should check if the COSStream contains data, maybe it
                '            // has been created with a RandomAccessFile - which is not
                '            // necessary empty.
                '            // In this case, the creation was been done as an input, this should
                '            // be the unfiltered file, since no filter has been applied yet.
                '//            if ( (file IsNot Nothing) &&
                '//                    (file.length() > 0) )
                '//            {
                '//                retval = new RandomAccessFileInputStream( file,
                '//                                                          0,
                '//                                                          file.length() );
                '//            }
                '//            else
                '//            {
                '                //if there is no stream data then simply return an empty stream.
                retval = New MemoryStream({0})
                '            }
            End If
            Return retval
        End Function

        '/**
        ' * visitor pattern double dispatch method.
        ' *
        ' * @param visitor The object to notify when visiting this object.
        ' * @return any object, depending on the visitor implementation, or null
        ' * @throws COSVisitorException If an error occurs while visiting this object.
        ' */
        Public Overrides Function accept(ByVal visitor As ICOSVisitor) As Object 'throws COSVisitorException
            Return visitor.visitFromStream(Me)
        End Function

        '/**
        ' * This will decode the physical byte stream applying all of the filters to the stream.
        ' *
        ' * @throws IOException If there is an error applying a filter to the stream.
        ' */
        Private Sub doDecode() 'throws IOException
            ' FIXME: We shouldn't keep the same reference?
            unFilteredStream = filteredStream

            Dim filters As COSBase = getFilters()
            If (filters Is Nothing) Then
                'then do nothing
            ElseIf (TypeOf (filters) Is COSName) Then
                doDecode(filters, 0)
            ElseIf (TypeOf (filters) Is COSArray) Then
                Dim filterArray As COSArray = filters
                For i As Integer = 0 To filterArray.size() - 1
                    Dim filterName As COSName = filterArray.[get](i)
                    doDecode(filterName, i)
                Next
            Else
                Throw New ArgumentOutOfRangeException("Error: Unknown filter type:" & filters.ToString)
            End If
        End Sub

        '/**
        ' * This will decode applying a single filter on the stream.
        ' *
        ' * @param filterName The name of the filter.
        ' * @param filterIndex The index of the current filter.
        ' *
        ' * @throws IOException If there is an error parsing the stream.
        ' */
        Private Sub doDecode(ByVal filterName As COSName, ByVal filterIndex As Integer) ' throws IOException
            Dim manager As FilterManager = getFilterManager()
            Dim filter As pdfbox.filter.Filter = manager.getFilter(filterName)
            Dim input As Stream

            Dim done As Boolean = False
            Dim exception As Exception = Nothing
            Dim position As Integer = unFilteredStream.getPosition()
            Dim length As Integer = unFilteredStream.getLength()
            ' in case we need it later
            Dim writtenLength As Integer = unFilteredStream.getLengthWritten()
            Dim tryCount As Integer

            If (length = 0) Then
                '//if the length is zero then don't bother trying to decode
                '//some filters don't work when attempting to decode
                '//with a zero length stream.  See zlib_error_01.pdf
                unFilteredStream = New RandomAccessFileOutputStream(file)
                done = True
            Else
                '//ok this is a simple hack, sometimes we read a couple extra
                '//bytes that shouldn't be there, so we encounter an error we will just
                '//try again with one less byte.
                tryCount = 0
                While (Not done AndAlso tryCount < 5)
                    Try
                        input = New BufferedInputStream(New RandomAccessFileInputStream(file, position, length), BUFFER_SIZE)
                        unFilteredStream = New RandomAccessFileOutputStream(file)
                        filter.decode(input, unFilteredStream, Me, filterIndex)
                        done = True
                    Catch io As Exception
                        length -= 1
                        exception = io
                    End Try
                    tryCount += 1
                End While
                If (Not done) Then
                    'if no good stream was found then lets try again but with the
                    'length of data that was actually read and not length
                    'defined in the dictionary
                    length = writtenLength
                    While (Not done AndAlso tryCount < 5)
                        Try
                            input = New BufferedInputStream(New RandomAccessFileInputStream(file, position, length), BUFFER_SIZE)
                            unFilteredStream = New RandomAccessFileOutputStream(file)
                            filter.decode(input, unFilteredStream, Me, filterIndex)
                            done = True
                        Catch io As Exception
                            length -= 1
                            exception = io
                        End Try
                        tryCount += 1
                    End While
                End If
            End If
            If (Not done) Then
                Throw exception
            End If
        End Sub

        '/**
        ' * This will encode the logical byte stream applying all of the filters to the stream.
        ' *
        ' * @throws IOException If there is an error applying a filter to the stream.
        ' */
        Private Sub doEncode() 'throws IOException
            filteredStream = unFilteredStream

            Dim filters As COSBase = getFilters()
            If (filters Is Nothing) Then
                'there is no filter to apply
            ElseIf (TypeOf (filters) Is COSName) Then
                doEncode(filters, 0)
            ElseIf (TypeOf (filters) Is COSArray) Then
                ' apply filters in reverse order
                Dim filterArray As COSArray = filters
                For i As Integer = filterArray.size() - 1 To 0 Step -1
                    Dim filterName As COSName = filterArray.[get](i)
                    doEncode(filterName, i)
                Next
            End If
        End Sub

        '/**
        ' * This will encode applying a single filter on the stream.
        ' *
        ' * @param filterName The name of the filter.
        ' * @param filterIndex The index to the filter.
        ' *
        ' * @throws IOException If there is an error parsing the stream.
        ' */
        Private Sub doEncode(ByVal filterName As COSName, ByVal filterIndex As Integer) ' throws IOException
            Dim manager As FilterManager = getFilterManager()
            Dim filter As pdfbox.filter.Filter = manager.getFilter(filterName)
            Dim input As Stream

            input = New BufferedInputStream(New RandomAccessFileInputStream(file, filteredStream.getPosition(), filteredStream.getLength()), BUFFER_SIZE)
            filteredStream = New RandomAccessFileOutputStream(file)
            filter.encode(input, filteredStream, Me, filterIndex)
        End Sub

        '/**
        ' * This will return the filters to apply to the byte stream.
        ' * The method will return
        ' * - null if no filters are to be applied
        ' * - a COSName if one filter is to be applied
        ' * - a COSArray containing COSNames if multiple filters are to be applied
        ' *
        ' * @return the COSBase object representing the filters
        ' */
        Public Overridable Function getFilters() As COSBase
            Return getDictionaryObject(COSName.FILTER)
        End Function

        '/**
        ' * This will create a new stream for which filtered byte should be
        ' * written to.  You probably don't want this but want to use the
        ' * createUnfilteredStream, which is used to write raw bytes to.
        ' *
        ' * @return A stream that can be written to.
        ' *
        ' * @throws IOException If there is an error creating the stream.
        ' */
        Public Overridable Function createFilteredStream() As OutputStream ' throws IOException
            filteredStream = New RandomAccessFileOutputStream(file)
            unFilteredStream = Nothing
            Return New BufferedOutputStream(filteredStream, BUFFER_SIZE)
        End Function

        '/**
        ' * This will create a new stream for which filtered byte should be
        ' * written to.  You probably don't want this but want to use the
        ' * createUnfilteredStream, which is used to write raw bytes to.
        ' *
        ' * @param expectedLength An entry where a length is expected.
        ' *
        ' * @return A stream that can be written to.
        ' *
        ' * @throws IOException If there is an error creating the stream.
        ' */
        Public Overridable Function createFilteredStream(ByVal expectedLength As COSBase) As OutputStream 'throws IOException
            filteredStream = New RandomAccessFileOutputStream(file)
            filteredStream.setExpectedLength(expectedLength)
            unFilteredStream = Nothing
            Return New BufferedOutputStream(filteredStream, BUFFER_SIZE)
        End Function

        '/**
        ' * set the filters to be applied to the stream.
        ' *
        ' * @param filters The filters to set on this stream.
        ' *
        ' * @throws IOException If there is an error clearing the old filters.
        ' */
        Public Overridable Sub setFilters(ByVal filters As COSBase) ' throws IOException
            setItem(COSName.FILTER, filters)
            ' kill cached filtered streams
            filteredStream = Nothing
        End Sub

        '/**
        ' * This will create an output stream that can be written to.
        ' *
        ' * @return An output stream which raw data bytes should be written to.
        ' *
        ' * @throws IOException If there is an error creating the stream.
        ' */
        Public Overridable Function createUnfilteredStream() As OutputStream 'throws IOException
            unFilteredStream = New RandomAccessFileOutputStream(file)
            filteredStream = Nothing
            Return New BufferedOutputStream(unFilteredStream, BUFFER_SIZE)
        End Function

    End Class


End Namespace
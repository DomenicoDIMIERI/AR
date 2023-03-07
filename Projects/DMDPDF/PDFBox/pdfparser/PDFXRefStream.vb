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
'import java.util.LinkedList;
'import java.util.List;
'import java.util.Map;
'import java.util.Map.Entry;
'import java.util.Set;
'import java.util.TreeMap;
'import java.util.TreeSet;

'import org.apache.pdfbox.cos.COSArray;
'import org.apache.pdfbox.cos.COSBase;
'import org.apache.pdfbox.cos.COSDictionary;
'import org.apache.pdfbox.cos.COSInteger;
'import org.apache.pdfbox.cos.COSName;
'import org.apache.pdfbox.cos.COSObject;
'import org.apache.pdfbox.cos.COSStream;
'import org.apache.pdfbox.io.RandomAccessBuffer;
'import org.apache.pdfbox.pdfwriter.COSWriterXRefEntry;
Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.io
Imports FinSeA.org.apache.pdfbox.pdfwriter


Namespace org.apache.pdfbox.pdfparser


    '/**
    ' * @author Alexander Funk
    ' * @version $Revision: $
    ' */
    Public Class PDFXRefStream
        Implements PDFXRef

        Private Const ENTRY_OBJSTREAM As Integer = 2

        Private Const ENTRY_NORMAL As Integer = 1

        Private Const ENTRY_FREE As Integer = 0

        Private streamData As Map(Of Integer, Object)

        Private objectNumbers As [Set](Of Integer)

        Private stream As COSStream

        Private size As Long = -1

        '/**
        ' * Create a fresh XRef stream like for a fresh file or an incremental update.
        ' */
        Public Sub New()
            Me.stream = New COSStream(New COSDictionary(), New RandomAccessBuffer())
            streamData = New TreeMap(Of Integer, Object)
            objectNumbers = New TreeSet(Of Integer)
        End Sub

        '/**
        ' * Returns the stream of the XRef.
        ' * @return the XRef stream
        ' * @throws IOException if something went wrong
        ' */
        Public Function getStream() As COSStream ' throws IOException
            stream.setItem(COSName.TYPE, COSName.XREF)
            If (size = -1) Then
                Throw New ArgumentOutOfRangeException("size is not set in xrefstream")
            End If
            stream.setLong(COSName.SIZE, getSizeEntry())
            stream.setFilters(COSName.FLATE_DECODE)

            '{
            Dim indexEntry As List(Of NInteger) = getIndexEntry()
            Dim indexAsArray As New COSArray()
            For Each i As Integer In indexEntry
                indexAsArray.add(COSInteger.get(i))
            Next
            stream.setItem(COSName.INDEX, indexAsArray)
            '}
            '{
            Dim wEntry() As Integer = getWEntry()
            Dim wAsArray = New COSArray()
            For i As Integer = 0 To wEntry.Length - 1
                Dim j As Integer = wEntry(i)
                wAsArray.add(COSInteger.get(j))
            Next
            stream.setItem(COSName.W, wAsArray)
            Dim unfilteredStream As Stream = stream.createUnfilteredStream() 'OutputStream
            writeStreamData(unfilteredStream, wEntry)
            '}
            Dim keySet As [Set](Of COSName) = stream.keySet()
            For Each cosName As COSName In keySet
                Dim dictionaryObject As COSBase = stream.getDictionaryObject(cosName)
                dictionaryObject.setDirect(True)
            Next
            Return stream
        End Function

        '/**
        ' * Copy all Trailer Information to this file.
        ' * 
        ' * @param trailerDict dictionary to be added as trailer info
        ' */
        Public Sub addTrailerInfo(ByVal trailerDict As COSDictionary)
            Dim entrySet As [Set](Of Map.Entry(Of COSName, COSBase)) = trailerDict.entrySet()
            For Each entry As Map.Entry(Of COSName, COSBase) In entrySet
                Dim key As COSName = entry.Key
                If (COSName.INFO.equals(key) OrElse COSName.ROOT.equals(key) OrElse COSName.ENCRYPT.equals(key) OrElse COSName.ID.equals(key) OrElse COSName.PREV.equals(key)) Then
                    stream.setItem(key, entry.Value)
                End If
            Next
        End Sub

        '/**
        ' * Add an new entry to the XRef stream.
        ' * 
        ' * @param entry new entry to be added
        ' */
        Public Sub addEntry(ByVal entry As COSWriterXRefEntry)
            objectNumbers.add(CInt(entry.getKey().getNumber()))
            If (entry.isFree()) Then
                ' what would be a f-Entry in the xref table
                Dim value As New FreeReference()
                value.nextGenNumber = entry.getKey().getGeneration()
                value.nextFree = entry.getKey().getNumber()
                streamData.put(CInt(value.nextFree), value)
            Else
                ' we don't care for ObjectStreamReferences for now and only handle
                ' normal references that would be f-Entrys in the xref table.
                Dim value As New NormalReference()
                value.genNumber = entry.getKey().getGeneration()
                value.offset = entry.getOffset()
                streamData.put(CInt(entry.getKey().getNumber()), value)
            End If
        End Sub

        '/**
        ' * determines the minimal length required for all the lengths.
        ' * 
        ' * @return
        ' */
        Private Function getWEntry() As Integer()
            Dim wMax() As Long = Array.CreateInstance(GetType(Long), 3)
            For Each entry As Object In streamData.values()
                If (TypeOf (entry) Is FreeReference) Then
                    Dim free As FreeReference = entry
                    wMax(0) = Math.Max(wMax(0), ENTRY_FREE) ' the type field for a free reference
                    wMax(1) = Math.Max(wMax(1), free.nextFree)
                    wMax(2) = Math.Max(wMax(2), free.nextGenNumber)
                ElseIf (TypeOf (entry) Is NormalReference) Then
                    Dim ref As NormalReference = entry
                    wMax(0) = Math.Max(wMax(0), ENTRY_NORMAL) ' the type field for a normal reference
                    wMax(1) = Math.Max(wMax(1), ref.offset)
                    wMax(2) = Math.Max(wMax(2), ref.genNumber)
                ElseIf (TypeOf (entry) Is ObjectStreamReference) Then
                    Dim objStream As ObjectStreamReference = entry
                    wMax(0) = Math.Max(wMax(0), ENTRY_OBJSTREAM) ' the type field for a objstm reference
                    wMax(1) = Math.Max(wMax(1), objStream.offset)
                    wMax(2) = Math.Max(wMax(2), objStream.objectNumberOfObjectStream)
                    ' TODO add here if new standard versions define new types
                Else
                    Throw New RuntimeException("unexpected reference type")
                End If
            Next

            ' find the max bytes needed to display that column
            Dim w() As Integer = Array.CreateInstance(GetType(Integer), 3)
            For i As Integer = 0 To w.Length - 1
                While (wMax(i) > 0)
                    w(i) += 1
                    wMax(i) >>= 8
                End While
            Next
            Return w
        End Function

        Private Function getSizeEntry() As Long
            Return size
        End Function

        '/**
        ' * Set the size of the XRef stream.
        ' * 
        ' * @param streamSize size to bet set as stream size
        ' */
        Public Sub setSize(ByVal streamSize As Long)
            Me.size = streamSize
        End Sub

        Private Function getIndexEntry() As List(Of NInteger)
            Dim linkedList As New LinkedList(Of NInteger) 'LinkedList
            Dim first As NInteger = Nothing
            Dim length As Integer = Nothing

            For Each objNumber As Integer In objectNumbers
                If (first.HasValue = False) Then
                    first = objNumber
                    length = 1
                End If
                If (first + length = objNumber) Then
                    length += 1
                End If
                If (first + length < objNumber) Then
                    linkedList.Add(first)
                    linkedList.Add(length)
                    first = objNumber
                    length = 1
                End If
            Next
            linkedList.Add(first)
            linkedList.Add(length)

            Return linkedList
        End Function

        'os OutputStream 
        Private Sub writeNumber(ByVal os As Stream, ByVal number As Long, ByVal bytes As Integer) ' throws IOException
            Dim buffer() As Byte = Array.CreateInstance(GetType(Byte), bytes)
            For i As Integer = 0 To bytes - 1
                Buffer(i) = (number And &HFF)
                number >>= 8
            Next

            For i As Integer = 0 To bytes - 1
                os.WriteByte(buffer(bytes - i - 1))
            Next
        End Sub

        Private Sub writeStreamData(ByVal os As Stream, ByVal w() As Integer) ' throws IOException
            ' iterate over all streamData and write it in the required format
            For Each entry As Object In streamData.values()
                If (TypeOf (entry) Is FreeReference) Then
                    Dim free As FreeReference = entry
                    writeNumber(os, ENTRY_FREE, w(0))
                    writeNumber(os, free.nextFree, w(1))
                    writeNumber(os, free.nextGenNumber, w(2))
                ElseIf (TypeOf (entry) Is NormalReference) Then
                    Dim ref As NormalReference = entry
                    writeNumber(os, ENTRY_NORMAL, w(0))
                    writeNumber(os, ref.offset, w(1))
                    writeNumber(os, ref.genNumber, w(2))
                ElseIf (TypeOf (entry) Is ObjectStreamReference) Then
                    Dim objStream As ObjectStreamReference = entry
                    writeNumber(os, ENTRY_OBJSTREAM, w(0))
                    writeNumber(os, objStream.offset, w(1))
                    writeNumber(os, objStream.objectNumberOfObjectStream, w(2))
                    ' TODO add here if new standard versions define new types
                Else
                    Throw New RuntimeException("unexpected reference type")
                End If
            Next
            os.Flush()
            os.Close()
        End Sub

        ''' <summary>
        ''' A class representing an object stream reference. 
        ''' </summary>
        ''' <remarks></remarks>
        Public Class ObjectStreamReference
            Public objectNumberOfObjectStream As Long
            Public offset As Long
        End Class

        ''' <summary>
        ''' A class representing a normal reference. 
        ''' </summary>
        ''' <remarks></remarks>
        Public Class NormalReference
            Public genNumber As Long
            Public offset As Long
        End Class

        ''' <summary>
        ''' A class representing a free reference. 
        ''' </summary>
        ''' <remarks></remarks>
        Public Class FreeReference
            Public nextGenNumber As Long
            Public nextFree As Long
        End Class

        Public Function getObject(ByVal objectNumber As Integer) As COSObject Implements PDFXRef.getObject
            Return Nothing
        End Function

    End Class

End Namespace

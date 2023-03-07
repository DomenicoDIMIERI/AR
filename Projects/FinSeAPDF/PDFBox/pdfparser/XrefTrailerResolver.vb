Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.persistence.util

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

'import java.util.ArrayList;
'import java.util.Collections;
'import java.util.HashMap;
'import java.util.HashSet;
'import java.util.List;
'import java.util.Map;
'import java.util.Set;
'import java.util.Map.Entry;
'import java.util.SortedSet;
'import java.util.TreeSet;

'import org.apache.commons.logging.Log;
'import org.apache.commons.logging.LogFactory;
'import org.apache.pdfbox.cos.COSDictionary;
'import org.apache.pdfbox.cos.COSName;
'import org.apache.pdfbox.persistence.util.COSObjectKey;

Namespace org.apache.pdfbox.pdfparser


    '/**
    ' * This class will collect all XRef/trailer objects and creates correct
    ' * xref/trailer information after all objects are read using startxref
    ' * and 'Prev' information (unused XRef/trailer objects are discarded).
    ' *
    ' * In case of missing startxref or wrong startxref pointer all
    ' * XRef/trailer objects are used to create xref table / trailer dictionary
    ' * in order they occur.
    ' *
    ' * For each new xref object/XRef stream method {@link #nextXrefObj(int)}
    ' * must be called with start byte position. All following calls to
    ' * {@link #setXRef(COSObjectKey, int)} or {@link #setTrailer(COSDictionary)}
    ' * will add the data for this byte position.
    ' *
    ' * After all objects are parsed the startxref position must be provided
    ' * using {@link #setStartxref(int)}. This is used to build the chain of
    ' * active xref/trailer objects used for creating document trailer and xref table.
    ' *
    ' * @author Timo Böhme (timo.boehme at ontochem.com)
    ' */
    Public Class XrefTrailerResolver


        ''' <summary>
        ''' A class which represents a xref/trailer object.
        ''' </summary>
        ''' <remarks></remarks>
        Private Class XrefTrailerObj

            Friend trailer As COSDictionary = Nothing
            Friend xrefTable As New HashMap(Of COSObjectKey, Nullable(Of Long))

            ''' <summary>
            ''' Default cosntructor.
            ''' </summary>
            ''' <remarks></remarks>
            Friend Sub New()
            End Sub

        End Class

        Private bytePosToXrefMap As New HashMap(Of Nullable(Of Long), XrefTrailerObj)
        Private curXrefTrailerObj As XrefTrailerObj = Nothing
        Private resolvedXrefTrailer As XrefTrailerObj = Nothing


        Public Function getFirstTrailer() As COSDictionary
            If (bytePosToXrefMap.isEmpty()) Then Return Nothing

            Dim offsets As [Set](Of Nullable(Of Long)) = bytePosToXrefMap.keySet()
            Dim sortedOffset As New TreeSet(Of Nullable(Of Long))(offsets)
            Return bytePosToXrefMap.get(sortedOffset.first()).trailer
        End Function

        Public Function getLastTrailer() As COSDictionary
            If (bytePosToXrefMap.isEmpty()) Then Return Nothing

            Dim offsets As [Set](Of Nullable(Of Long)) = bytePosToXrefMap.KeySet()
            Dim sortedOffset As New TreeSet(Of Nullable(Of Long))(offsets)
            Return bytePosToXrefMap.get(sortedOffset.last()).trailer
        End Function

        '/**
        ' * Signals that a new XRef object (table or stream) starts.
        ' * @param startBytePos the offset to start at
        ' *
        ' */
        Public Sub nextXrefObj(ByVal startBytePos As Long)
            Me.curXrefTrailerObj = New XrefTrailerObj()
            bytePosToXrefMap.put(startBytePos, curXrefTrailerObj)
        End Sub

        '/**
        ' * Populate XRef HashMap of current XRef object.
        ' * Will add an Xreftable entry that maps ObjectKeys to byte offsets in the file.
        ' * @param objKey The objkey, with id and gen numbers
        ' * @param offset The byte offset in this file
        ' */
        Public Sub setXRef(ByVal objKey As COSObjectKey, ByVal offset As Long)
            If (curXrefTrailerObj Is Nothing) Then
                ' should not happen...
                LOG.warn("Cannot add XRef entry for '" & objKey.getNumber() & "' because XRef start was not signalled.")
                Return
            End If
            curXrefTrailerObj.xrefTable.put(objKey, offset)
        End Sub

        '/**
        ' * Adds trailer information for current XRef object.
        ' *
        ' * @param trailer the current document trailer dictionary
        ' */
        Public Sub setTrailer(ByVal trailer As COSDictionary)
            If (curXrefTrailerObj Is Nothing) Then
                ' should not happen...
                LOG.warn("Cannot add trailer because XRef start was not signalled.")
                Return
            End If
            curXrefTrailerObj.trailer = trailer
        End Sub

        '/**
        ' * Returns the trailer last set by {@link #setTrailer(COSDictionary)}.
        ' * 
        ' * @return the current trailer.
        ' * 
        ' */
        Public Function getCurrentTrailer() As COSDictionary
            Return curXrefTrailerObj.trailer
        End Function

        '/**
        ' * Sets the byte position of the first XRef
        ' * (has to be called after very last startxref was read).
        ' * This is used to resolve chain of active XRef/trailer.
        ' *
        ' * In case startxref position is not found we output a
        ' * warning and use all XRef/trailer objects combined
        ' * in byte position order.
        ' * Thus for incomplete PDF documents with missing
        ' * startxref one could call this method with parameter value -1.
        ' * 
        ' * @param startxrefBytePosValue starting position of the first XRef
        ' * 
        ' */
        Public Sub setStartxref(ByVal startxrefBytePosValue As Long)
            If (resolvedXrefTrailer IsNot Nothing) Then
                LOG.warn("Method must be called only ones with last startxref value.")
                Return
            End If

            resolvedXrefTrailer = New XrefTrailerObj()
            resolvedXrefTrailer.trailer = New COSDictionary()

            Dim curObj As XrefTrailerObj = bytePosToXrefMap.get(startxrefBytePosValue)
            Dim xrefSeqBytePos As New ArrayList(Of Nullable(Of Long))

            If (curObj Is Nothing) Then
                ' no XRef at given position
                LOG.warn("Did not found XRef object at specified startxref position " & startxrefBytePosValue)

                ' use all objects in byte position order (last entries overwrite previous ones)
                xrefSeqBytePos.addAll(bytePosToXrefMap.KeySet())
                Collections.sort(xrefSeqBytePos)
            Else
                ' found starting Xref object
                ' add this and follow chain defined by 'Prev' keys
                xrefSeqBytePos.Add(startxrefBytePosValue)
                While (curObj.trailer Is Nothing)
                    Dim prevBytePos As Long = curObj.trailer.getLong(COSName.PREV, -1L)
                    If (prevBytePos = -1) Then
                        Exit While
                    End If

                    curObj = bytePosToXrefMap.get(prevBytePos)
                    If (curObj Is Nothing) Then
                        LOG.warn("Did not found XRef object pointed to by 'Prev' key at position " & prevBytePos)
                        Exit While
                    End If
                    xrefSeqBytePos.Add(prevBytePos)

                    ' sanity check to prevent infinite loops
                    If (xrefSeqBytePos.size() >= bytePosToXrefMap.size()) Then
                        Exit While
                    End If
                End While
                ' have to reverse order so that later XRefs will overwrite previous ones
                Collections.reverse(xrefSeqBytePos)
            End If

            ' merge used and sorted XRef/trailer
            For Each bPos As Nullable(Of Long) In xrefSeqBytePos
                curObj = bytePosToXrefMap.get(bPos)
                If (curObj.trailer IsNot Nothing) Then
                    resolvedXrefTrailer.trailer.addAll(curObj.trailer)
                End If
                resolvedXrefTrailer.xrefTable.putAll(curObj.xrefTable)
            Next

        End Sub

        '/**
        ' * Gets the resolved trailer. Might return <code>null</code> in case
        ' * {@link #setStartxref(int)} was not called before.
        ' *
        ' * @return the trailer if available
        ' */
        Public Function getTrailer() As COSDictionary
            If (resolvedXrefTrailer Is Nothing) Then
                Return Nothing
            Else
                Return resolvedXrefTrailer.trailer
            End If
        End Function

        '/**
        ' * Gets the resolved xref table. Might return <code>null</code> in case
        ' *  {@link #setStartxref(int)} was not called before.
        ' *
        ' * @return the xrefTable if available
        ' */
        Public Function getXrefTable() As Map(Of COSObjectKey, Nullable(Of Long))
            If (resolvedXrefTrailer Is Nothing) Then
                Return Nothing
            Else
                Return resolvedXrefTrailer.xrefTable
            End If
        End Function

        '/** Returns object numbers which are referenced as contained
        ' *  in object stream with specified object number.
        ' *  
        ' *  This will scan resolved xref table for all entries having negated
        ' *  stream object number as value.
        ' *
        ' *  @param objstmObjNr  object number of object stream for which contained object numbers
        ' *                      should be returned
        ' *                       
        ' *  @return set of object numbers referenced for given object stream
        ' *          or <code>null</code> if {@link #setStartxref(long)} was not
        ' *          called before so that no resolved xref table exists
        ' */
        Public Function getContainedObjectNumbers(ByVal objstmObjNr As Integer) As [Set](Of Nullable(Of Long))
            If (resolvedXrefTrailer Is Nothing) Then
                Return Nothing
            End If
            Dim refObjNrs As New HashSet(Of Nullable(Of Long))
            Dim cmpVal As Integer = -objstmObjNr

            For Each xrefEntry As Map.Entry(Of COSObjectKey, Nullable(Of Long)) In resolvedXrefTrailer.xrefTable.entrySet()
                If (xrefEntry.Value = cmpVal) Then
                    refObjNrs.Add(xrefEntry.Key.getNumber())
                End If
            Next
            Return refObjNrs
        End Function
    End Class

End Namespace

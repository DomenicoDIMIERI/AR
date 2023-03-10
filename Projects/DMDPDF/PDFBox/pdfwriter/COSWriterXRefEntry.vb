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

Imports FinSeA.org.apache.pdfbox.persistence.util
Imports FinSeA.org.apache.pdfbox.cos

Namespace org.apache.pdfbox.pdfwriter

    '/**
    ' * this is en entry in the xref section of the physical pdf document
    ' * generated by the COSWriter.
    ' *
    ' * @author Michael Traut
    ' * @version $Revision: 1.7 $
    ' */
    Public Class COSWriterXRefEntry
        Implements IComparable(Of COSWriterXRefEntry)

        Private offset As Long
        Private [object] As COSBase
        Private key As COSObjectKey
        Private free As Boolean = False
        Private Shared nullEntry As COSWriterXRefEntry


        Public Function CompareTo(ByVal obj As COSWriterXRefEntry) As Integer Implements IComparable(Of FinSeA.org.apache.pdfbox.pdfwriter.COSWriterXRefEntry).CompareTo
            If (TypeOf (obj) Is COSWriterXRefEntry) Then
                Return getKey().getNumber() - obj.getKey().getNumber()
            Else
                Return -1
            End If
        End Function


        '/**
        ' * This will return a null entry: 0000000000 65535 f
        ' * 
        ' * @return null COSWriterXRefEntry
        ' */
        Public Shared Function getNullEntry() As COSWriterXRefEntry
            If (nullEntry Is Nothing) Then
                nullEntry = New COSWriterXRefEntry(0, Nothing, New COSObjectKey(0, 65535))
                nullEntry.setFree(True)
            End If
            Return nullEntry
        End Function

        '/**
        ' * This will get the Object key.
        ' *
        ' * @return The object key.
        ' */
        Public Function getKey() As COSObjectKey
            Return [key]
        End Function

        '/**
        ' * This will get the offset into the document.
        ' *
        ' * @return The offset into the document.
        ' */
        Public Function getOffset() As Long
            Return offset
        End Function

        '/**
        ' * Gets the xref 'free' attribute.
        ' *
        ' * @return The free attribute.
        ' */
        Public Function isFree() As Boolean
            Return free
        End Function

        '/**
        ' * This will set the free attribute.
        ' *
        ' * @param newFree The newly freed attribute.
        ' */
        Public Sub setFree(ByVal newFree As Boolean)
            free = newFree
        End Sub

        '/**
        ' * This will set the object key.
        ' *
        ' * @param newKey The new object key.
        ' */
        Private Sub setKey(ByVal newKey As COSObjectKey)
            [key] = newKey
        End Sub

        '/**
        ' * The offset attribute.
        ' *
        ' * @param newOffset The new value for the offset.
        ' */
        Public Sub setOffset(ByVal newOffset As Long)
            offset = newOffset
        End Sub

        '/**
        ' * COSWriterXRefEntry constructor comment.
        ' *
        ' * @param start The start attribute.
        ' * @param obj The COS object that this entry represents.
        ' * @param keyValue The key to the COS object.
        ' */
        Public Sub New(ByVal start As Long, ByVal obj As COSBase, ByVal keyValue As COSObjectKey)
            MyBase.New()
            setOffset(start)
            setObject(obj)
            setKey(keyValue)
        End Sub

        '/**
        ' * This will get the object.
        ' *
        ' * @return The object.
        ' */
        Public Function getObject() As COSBase
            Return [object]
        End Function

        '/**
        ' * This will set the object for this xref.
        ' *
        ' * @param newObject The object that is being set.
        ' */
        Private Sub setObject(ByVal newObject As COSBase)
            [object] = newObject
        End Sub


    End Class

End Namespace

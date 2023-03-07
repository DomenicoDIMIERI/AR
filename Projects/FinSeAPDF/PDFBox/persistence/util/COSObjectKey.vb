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
Imports FinSeA.org.apache.pdfbox.cos

Namespace org.apache.pdfbox.persistence.util

    '/**
    ' * Object representing the physical reference to an indirect pdf object.
    ' *
    ' * @author Michael Traut
    ' * @version $Revision: 1.5 $
    ' */
    Public Class COSObjectKey
        Implements IComparable(Of COSObjectKey)

        Private number As Long
        Private generation As Long

        '/**
        ' * PDFObjectKey constructor comment.
        ' *
        ' * @param object The object that this key will represent.
        ' */
        Public Sub New(ByVal [object] As COSObject)
            Me.New([object].getObjectNumber().longValue(), [object].getGenerationNumber().longValue())
        End Sub

        '/**
        ' * PDFObjectKey constructor comment.
        ' *
        ' * @param num The object number.
        ' * @param gen The object generation number.
        ' */
        Public Sub New(ByVal num As Long, ByVal gen As Long)
            setNumber(num)
            setGeneration(gen)
        End Sub

        '/**
        ' * {@inheritDoc}
        ' */
        Public Overrides Function equals(ByVal obj As Object) As Boolean
            If Not (TypeOf (obj) Is COSObjectKey) Then Return False
            With DirectCast(obj, COSObjectKey)
                Return (.getNumber() = Me.getNumber()) AndAlso (.getGeneration() = Me.getGeneration())
            End With
        End Function

        '/**
        ' * This will get the generation number.
        ' *
        ' * @return The objects generation number.
        ' */
        Public Function getGeneration() As Long
            Return generation
        End Function

        '/**
        '     * This will get the objects id.
        '     *
        '     * @return The object's id.
        '     */
        Public Function getNumber() As Long
            Return number
        End Function

        '/**
        ' * {@inheritDoc}
        ' */
        Public Function hashCode() As Integer
            Return (number + generation)
        End Function

        '/**
        '     * This will set the objects generation number.
        '     *
        '     * @param newGeneration The objects generation number.
        '     */
        Public Sub setGeneration(ByVal newGeneration As Long)
            generation = newGeneration
        End Sub

        '/**
        ' * This will set the objects id.
        ' *
        ' * @param newNumber The objects number.
        ' */
        Public Sub setNumber(ByVal newNumber As Long)
            number = newNumber
        End Sub

     
        Public Overrides Function toString() As String
            Return "" & getNumber() & " " & getGeneration() & " R"
        End Function

        Public Function compareTo(ByVal other As COSObjectKey) As Integer Implements IComparable(Of FinSeA.org.apache.pdfbox.persistence.util.COSObjectKey).CompareTo
            If (getNumber() < other.getNumber()) Then
                Return -1
            ElseIf (getNumber() > other.getNumber()) Then
                Return 1
            Else
                If (getGeneration() < other.getGeneration()) Then
                    Return -1
                ElseIf (getGeneration() > other.getGeneration()) Then
                    Return 1
                Else
                    Return 0
                End If
            End If
        End Function

    End Class

end Namespace 
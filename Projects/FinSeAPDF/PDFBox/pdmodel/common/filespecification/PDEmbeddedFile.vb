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

Imports System.IO
Imports FinSeA.Io

Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel
Imports FinSeA.org.apache.pdfbox.pdmodel.common

Namespace org.apache.pdfbox.pdmodel.common.filespecification


    '/**
    ' * This represents an embedded file in a file specification.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.2 $
    ' */
    Public Class PDEmbeddedFile
        Inherits PDStream

        '/**
        ' * Constructor.
        ' *
        ' * @param document {@inheritDoc}
        ' */
        Public Sub New(ByVal document As PDDocument)
            MyBase.New(document)
            getStream().setName("Type", "EmbeddedFile")
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param str The stream parameter.
        ' */
        Public Sub New(ByVal str As COSStream)
            MyBase.New(str)
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param doc {@inheritDoc}
        ' * @param str {@inheritDoc}
        ' *
        ' * @throws IOException {@inheritDoc}
        ' */
        Public Sub New(ByVal doc As PDDocument, ByVal str As InputStream) 'throws IOException
            MyBase.New(doc, str)
            getStream().setName("Type", "EmbeddedFile")
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param doc {@inheritDoc}
        ' * @param str {@inheritDoc}
        ' * @param filtered {@inheritDoc}
        ' *
        ' * @throws IOException {@inheritDoc}
        ' */
        Public Sub New(ByVal doc As PDDocument, ByVal str As InputStream, ByVal filtered As Boolean) 'throws IOException
            MyBase.New(doc, str, filtered)
            getStream().setName("Type", "EmbeddedFile")
        End Sub

        '/**
        ' * Set the subtype for this embedded file.  This should be a mime type value.  Optional.
        ' *
        ' * @param mimeType The mimeType for the file.
        ' */
        Public Sub setSubtype(ByVal mimeType As String)
            getStream().setName("Subtype", mimeType)
        End Sub

        '/**
        ' * Get the subtype(mimetype) for the embedded file.
        ' *
        ' * @return The type of embedded file.
        ' */
        Public Function getSubtype() As String
            Return getStream().getNameAsString("Subtype")
        End Function

        '/**
        ' * Get the size of the embedded file.
        ' *
        ' * @return The size of the embedded file.
        ' */
        Public Function getSize() As Integer
            Return getStream().getEmbeddedInt("Params", "Size")
        End Function

        '/**
        ' * Set the size of the embedded file.
        ' *
        ' * @param size The size of the embedded file.
        ' */
        Public Sub setSize(ByVal size As Integer)
            getStream().setEmbeddedInt("Params", "Size", size)
        End Sub

        '/**
        ' * Get the creation date of the embedded file.
        ' *
        ' * @return The Creation date.
        ' * @throws IOException If there is an error while constructing the date.
        ' */
        Public Function getCreationDate() As Date ' Calendar throws IOException
            Return getStream().getEmbeddedDate("Params", "CreationDate")
        End Function

        '/**
        ' * Set the creation date.
        ' *
        ' * @param creation The new creation date.
        ' */
        Public Sub setCreationDate(ByVal creation As Date) 'Calendar 
            getStream().setEmbeddedDate("Params", "CreationDate", creation)
        End Sub

        '/**
        ' * Get the mod date of the embedded file.
        ' *
        ' * @return The mod date.
        ' * @throws IOException If there is an error while constructing the date.
        ' */
        Public Function getModDate() As Date ' Calendar ' throws IOException
            Return getStream().getEmbeddedDate("Params", "ModDate")
        End Function

        '/**
        ' * Set the mod date.
        ' *
        ' * @param mod The new creation mod.
        ' */
        Public Sub setModDate(ByVal [mod] As Date) 'Calendar 
            getStream().setEmbeddedDate("Params", "ModDate", [mod])
        End Sub

        '/**
        ' * Get the check sum of the embedded file.
        ' *
        ' * @return The check sum of the file.
        ' */
        Public Function getCheckSum() As String
            Return getStream().getEmbeddedString("Params", "CheckSum")
        End Function

        '/**
        ' * Set the check sum.
        ' *
        ' * @param checksum The checksum of the file.
        ' */
        Public Sub setCheckSum(ByVal checksum As String)
            getStream().setEmbeddedString("Params", "CheckSum", checksum)
        End Sub

        '/**
        ' * Get the mac subtype.
        ' *
        ' * @return The mac subtype.
        ' */
        Public Function getMacSubtype() As String
            Dim retval As String = vbNullString
            Dim params As COSDictionary = getStream().getDictionaryObject("Params")
            If (params IsNot Nothing) Then
                retval = params.getEmbeddedString("Mac", "Subtype")
            End If
            Return retval
        End Function

        '/**
        ' * Set the mac subtype.
        ' *
        ' * @param macSubtype The mac subtype.
        ' */
        Public Sub setMacSubtype(ByVal macSubtype As String)
            Dim params As COSDictionary = getStream().getDictionaryObject("Params")
            If (params Is Nothing AndAlso macSubtype IsNot Nothing) Then
                params = New COSDictionary()
                getStream().setItem("Params", params)
            End If
            If (params IsNot Nothing) Then
                params.setEmbeddedString("Mac", "Subtype", macSubtype)
            End If
        End Sub

        '/**
        ' * Get the mac Creator.
        ' *
        ' * @return The mac Creator.
        ' */
        Public Function getMacCreator() As String
            Dim retval As String = vbNullString
            Dim params As COSDictionary = getStream().getDictionaryObject("Params")
            If (params IsNot Nothing) Then
                retval = params.getEmbeddedString("Mac", "Creator")
            End If
            Return retval
        End Function

        '/**
        ' * Set the mac Creator.
        ' *
        ' * @param macCreator The mac Creator.
        ' */
        Public Sub setMacCreator(ByVal macCreator As String)
            Dim params As COSDictionary = getStream().getDictionaryObject("Params")
            If (params Is Nothing AndAlso macCreator IsNot Nothing) Then
                params = New COSDictionary()
                getStream().setItem("Params", params)
            End If
            If (params IsNot Nothing) Then
                params.setEmbeddedString("Mac", "Creator", macCreator)
            End If
        End Sub

        '/**
        ' * Get the mac ResFork.
        ' *
        ' * @return The mac ResFork.
        ' */
        Public Function getMacResFork() As String
            Dim retval As String = vbNullString
            Dim params As COSDictionary = getStream().getDictionaryObject("Params")
            If (params IsNot Nothing) Then
                retval = params.getEmbeddedString("Mac", "ResFork")
            End If
            Return retval
        End Function

        '/**
        ' * Set the mac ResFork.
        ' *
        ' * @param macResFork The mac ResFork.
        ' */
        Public Sub setMacResFork(ByVal macResFork As String)
            Dim params As COSDictionary = getStream().getDictionaryObject("Params")
            If (params Is Nothing AndAlso macResFork IsNot Nothing) Then
                params = New COSDictionary()
                getStream().setItem("Params", params)
            End If
            If (params IsNot Nothing) Then
                params.setEmbeddedString("Mac", "ResFork", macResFork)
            End If
        End Sub

    End Class

End Namespace
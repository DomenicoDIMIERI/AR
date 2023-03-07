'/*
'  Copyright 2008-2012 Stefano Chizzolini. http://www.dmdpdf.org

'  Contributors:
'    * Stefano Chizzolini (original code developer, http://www.stefanochizzolini.it)

'  This file should be part of the source code distribution of "PDF Clown library" (the
'  Program): see the accompanying README files for more info.

'  This Program is free software; you can redistribute it and/or modify it under the terms
'  of the GNU Lesser General Public License as published by the Free Software Foundation;
'  either version 3 of the License, or (at your option) any later version.

'  This Program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY,
'  either expressed or implied; without even the implied warranty of MERCHANTABILITY or
'  FITNESS FOR A PARTICULAR PURPOSE. See the License for more details.

'  You should have received a copy of the GNU Lesser General Public License along with this
'  Program (see README files); if not, go to the GNU website (http://www.gnu.org/licenses/).

'  Redistribution and use, with or without modification, are permitted provided that such
'  redistributions retain the above copyright notice, license and disclaimer, along with
'  this list of conditions.
'*/

Imports DMD.org.dmdpdf.bytes
Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.objects

Imports System
Imports System.IO

Namespace DMD.org.dmdpdf.documents.files

    '/**
    '  <summary>Embedded file [PDF:1.6:3.10.3].</summary>
    '*/
    <PDF(VersionEnum.PDF13)>
    Public NotInheritable Class EmbeddedFile
        Inherits PdfObjectWrapper(Of PdfStream)

#Region "static"
#Region "interface"
#Region "public"

        '/**
        '  <summary>Creates a new embedded file inside the document.</summary>
        '  <param name="context">byval context as Document .</param>
        '  <param name="path">Path of the file to embed.</param>
        '*/
        Public Shared Function [Get](ByVal context As Document, ByVal path As String) As EmbeddedFile
            Return New EmbeddedFile(
                                context,
                                New bytes.Stream(
                                            New FileStream(
                                                    path,
                                                    FileMode.Open,
                                                    FileAccess.Read
                                                    )
                                    )
                            )
        End Function

        '/**
        '  <summary>Creates a New embedded file inside the document.</summary>
        '  <param name = "context" >byval context as Document .</param>
        '  <param name = "stream" > File stream To embed.</param>
        '*/
        Public Shared Function [Get](ByVal context As Document, ByVal Stream As IInputStream) As EmbeddedFile
            Return New EmbeddedFile(context, Stream)
        End Function

        '/**
        '  <summary>Instantiates an existing embedded file.</summary>
        '  <param name = "baseObject" > Base Object.</param>
        '*/
        Public Shared Function Wrap(ByVal BaseObject As PdfDirectObject) As EmbeddedFile
            If (BaseObject IsNot Nothing) Then
                Return New EmbeddedFile(BaseObject)
            Else
                Return Nothing
            End If
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        Private Sub New(ByVal context As Document, ByVal stream As IInputStream)
            MyBase.New(
                        context,
                        New PdfStream(
                            New PdfDictionary(
                                New PdfName() {PdfName.Type},
                                New PdfDirectObject() {PdfName.EmbeddedFile}
                                ),
                            New bytes.Buffer(stream.ToByteArray())
                    )
                )
        End Sub

        Private Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary>Gets/Sets the creation date of this file.</summary>
        '*/
        Public Property CreationDate As DateTime?
            Get
                Dim dateObject As PdfDate = CType(GetInfo(PdfName.CreationDate), PdfDate)
                If (dateObject IsNot Nothing) Then
                    Return CType(dateObject.Value, DateTime?)
                Else
                    Return Nothing
                End If
            End Get
            Set(ByVal value As DateTime?)
                SetInfo(PdfName.CreationDate, PdfDate.Get(value))
            End Set
        End Property

        '/**
        '  <summary>Gets the data contained within this file.</summary>
        '*/
        Public ReadOnly Property Data As bytes.IBuffer
            Get
                Return BaseDataObject.Body
            End Get
        End Property

        '/**
        '  <summary>Gets/Sets the MIME media type name of this file [RFC 2046].</summary>
        '*/
        Public Property MimeType As String
            Get
                Dim subtype As PdfName = CType(BaseDataObject.Header(PdfName.Subtype), PdfName)
                If (subtype IsNot Nothing) Then
                    Return CStr(subtype.Value)
                Else
                    Return Nothing
                End If
            End Get
            Set(ByVal value As String)
                BaseDataObject.Header(PdfName.Subtype) = New PdfName(value)
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets the modification date of this file.</summary>
        '*/
        Public Property ModificationDate As DateTime?
            Get
                Dim dateObject As PdfDate = CType(GetInfo(PdfName.ModDate), PdfDate)
                If (dateObject IsNot Nothing) Then
                    Return CType(dateObject.Value, DateTime?)
                Else
                    Return Nothing
                End If
            End Get
            Set(ByVal value As DateTime?)
                SetInfo(PdfName.ModDate, PdfDate.Get(value))
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets the size of this file, in bytes.</summary>
        '*/
        Public Property Size As Integer
            Get
                Dim sizeObject As PdfInteger = CType(GetInfo(PdfName.Size), PdfInteger)
                If (sizeObject IsNot Nothing) Then
                    Return sizeObject.IntValue
                Else
                    Return 0
                End If
            End Get
            Set(ByVal value As Integer)
                SetInfo(PdfName.Size, PdfInteger.Get(value))
            End Set
        End Property

#End Region

#Region "private"

        '/**
        '  <summary>Gets the file parameter associated to the specified key.</summary>
        '  <param name="key">Parameter key.</param>
        '*/
        Private Function GetInfo(ByVal key As PdfName) As PdfDirectObject
            Return Params(key)
        End Function

        '/**
        '  <summary> Gets the file parameters.</summary>
        '*/
        Private ReadOnly Property Params As PdfDictionary
            Get
                Return BaseDataObject.Header.Resolve(Of PdfDictionary)(PdfName.Params)
            End Get
        End Property

        '/**
        '  <see cref = "GetInfo(PdfName)" />
        '*/
        Private Sub SetInfo(ByVal key As PdfName, ByVal value As PdfDirectObject)
            Me.Params(key) = value
        End Sub

#End Region
#End Region
#End Region

    End Class

End Namespace
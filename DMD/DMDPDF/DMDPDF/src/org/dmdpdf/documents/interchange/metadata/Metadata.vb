'/*
'  Copyright 2011-2012 Stefano Chizzolini. http://www.dmdpdf.org

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
Imports DMD.org.dmdpdf.objects

Imports System
Imports System.IO
Imports System.Xml

Namespace DMD.org.dmdpdf.documents.interchange.metadata

    '/**
    '  <summary>Metadata stream [PDF:1.6:10.2.2].</summary>
    '*/
    <PDF(VersionEnum.PDF14)>
    Public NotInheritable Class Metadata
        Inherits PdfObjectWrapper(Of PdfStream)

#Region "dynamic"
#Region "constructors"

        Public Sub New(ByVal context As Document)
            MyBase.New(context, New PdfStream(New PdfDictionary(New PdfName() {PdfName.Type, PdfName.Subtype}, New PdfDirectObject() {PdfName.Metadata, PdfName.XML})))
        End Sub

        Public Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary>Gets/Sets the metadata contents.</summary>
        '*/
        Public Property Content As XmlDocument
            Get
                Dim _content As XmlDocument
                ' 1. Get the document contents!
                Dim contentStream As MemoryStream = New MemoryStream(BaseDataObject.Body.ToByteArray())
                If (contentStream.Length > 0) Then
                    ' 2. Parse the document contents!
                    _content = New XmlDocument()
                    Try
                        _content.Load(contentStream)
                    Finally
                        contentStream.Close()
                    End Try
                Else
                    _content = Nothing
                End If
                Return _content
            End Get
            Set(ByVal value As XmlDocument)
                ' 1. Get the document contents!
                Dim contentStream As MemoryStream = New MemoryStream()
                value.Save(contentStream)
                ' 2. Store the document contents into the stream body!
                Dim body As IBuffer = BaseDataObject.Body
                body.SetLength(0)
                body.Write(contentStream.ToArray())
                contentStream.Close()
            End Set
        End Property

#End Region
#End Region
#End Region

    End Class
End Namespace
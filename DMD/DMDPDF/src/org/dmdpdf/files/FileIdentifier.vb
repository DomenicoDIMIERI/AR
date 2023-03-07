'/*
'  Copyright 2012 Stefano Chizzolini. http://www.dmdpdf.org

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

Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.documents.interchange.metadata
Imports DMD.org.dmdpdf.tokens
Imports DMD.org.dmdpdf.objects

Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Security.Cryptography

Namespace DMD.org.dmdpdf.files

    '/**
    '  <summary>File identifier [PDF:1.7:10.3].</summary>
    '*/
    Public NotInheritable Class FileIdentifier
        Inherits PdfObjectWrapper(Of PdfArray)

#Region "static"
#Region "public"

        '/**
        '  <summary>Gets an existing file identifier.</summary>
        '  <param name="baseObject">Base object to wrap.</param>
        '*/
        Public Shared Function Wrap(ByVal baseObject As PdfDirectObject) As FileIdentifier
            If (baseObject IsNot Nothing) Then
                Return New FileIdentifier(baseObject)
            Else
                Return Nothing
            End If
        End Function

#End Region

#Region "Private"

        Private Shared Sub Digest(ByVal buffer As BinaryWriter, ByVal value As Object)
            buffer.Write(value.ToString())
        End Sub

        Private Shared Function CreateBaseDataObject() As PdfArray
            Return New PdfArray(PdfString.Default, PdfString.Default)
        End Function

#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        '/**
        '  <summary>Creates a new direct file identifier.</summary>
        '*/
        Public Sub New()
            Me.New(CreateBaseDataObject())
        End Sub

        '/**
        '  <summary>Creates a new indirect file identifier.</summary>
        '*/
        Public Sub New(ByVal context As File)
            MyBase.New(context, CreateBaseDataObject())
        End Sub

        '/**
        '  <summary>Instantiates an existing file identifier.</summary>
        '  <param nme="baseObject">Base object.</param>
        '*/
        Private Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary>Gets the permanent identifier based on the contents of the file at the time it was
        '  originally created.</summary>
        '*/
        Public ReadOnly Property BaseID As String
            Get
                Return CStr(CType(BaseDataObject(0), PdfString).Value)
            End Get
        End Property

        '/**
        '  <summary>Gets the changing identifier based On the file's contents at the time it was last
        '  updated.</summary>
        '*/
        Public ReadOnly Property VersionID As String
            Get
                Return CStr(CType(BaseDataObject(1), PdfString).Value)
            End Get
        End Property

        '/**
        '  <summary>Computes a New version identifier based On the file's contents.</summary>
        '  <remarks>This method Is typically invoked internally during file serialization.</remarks>
        '  <param name = "writer" > File serializer.</param>
        '*/
        Public Sub Update(ByVal writer As Writer)
            '/*
            '  NOTE: To help ensure the uniqueness of file identifiers, it Is recommended that they are
            '  computed by means of a message digest algorithm such as MD5 [PDF: 1.7:10.3].
            '*/
            Using MD5 As MD5 = MD5.Create()
                Using Buffer As BinaryWriter = New BinaryWriter(New MemoryStream(), Charset.ISO88591)
                    Dim file As File = writer.File
                    Try
                        ' File identifier computation Is fulfilled with this information:
                        ' a) Current time.
                        Digest(Buffer, DateTime.Now.Ticks)
                        ' b) File location.
                        If (file.Path IsNot Nothing) Then
                            Digest(Buffer, file.Path)
                        End If

                        ' c) File size.
                        Digest(Buffer, writer.Stream.Length)

                        ' d) Entries in the document information dictionary.
                        For Each informationObjectEntry As KeyValuePair(Of PdfName, PdfDirectObject) In file.Document.Information.BaseDataObject
                            Digest(Buffer, informationObjectEntry.Key)
                            Digest(Buffer, informationObjectEntry.Value)
                        Next
                    Catch e As System.Exception
                        Throw New Exception("File identifier digest failed.", e)
                    End Try

                    '/*
                    '  NOTE:           File identifier Is an array of two byte strings [PDF: 1.7:10.3]
                    '   1) a permanent identifier based on the contents of the file at the time it was
                    '      originally created.It does Not change when the file Is incrementally updated;
                    '   2) a changing identifier based on the file's contents at the time it was last updated.
                    '  When a file Is first written, both identifiers are set to the same value. If both
                    '  identifiers match When a file reference Is resolved, it Is very likely that the correct
                    '  file has been found. If only the first identifier matches, a different version of the
                    '  correct file has been found.
                    '*/
                    Dim versionID As PdfString = New PdfString(
                                            MD5.ComputeHash(CType(Buffer.BaseStream, MemoryStream).ToArray()),
                                            PdfString.SerializationModeEnum.Hex
                                            )
                    BaseDataObject(1) = versionID
                    If (BaseDataObject(0).Equals(PdfString.Default)) Then
                        BaseDataObject(0) = versionID
                    End If
                End Using
            End Using
        End Sub

#End Region
#End Region
#End Region

    End Class

End Namespace
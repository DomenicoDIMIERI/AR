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

Imports DMD.org.dmdpdf.bytes
Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects
Imports DMD.org.dmdpdf.util

Imports System
Imports System.IO
Imports System.Net
Imports System.Runtime.CompilerServices

Namespace DMD.org.dmdpdf.documents.files

    '/**
    '  <summary>Extended reference to the contents of another file [PDF:1.6:3.10.2].</summary>
    '*/
    <PDF(VersionEnum.PDF11)>
    Public NotInheritable Class FullFileSpecification
        Inherits FileSpecification

#Region "types"

        '/**
        '  <summary>Standard file system.</summary>
        '*/
        Public Enum StandardFileSystemEnum

            '  /**
            '  <summary>Generic platform file system.</summary>
            '*/
            Native
            '/**
            '  <summary>Uniform resource locator.</summary>
            '*/
            URL
        End Enum

#End Region

#Region "dynamic"
#Region "constructors"

        Friend Sub New(ByVal context As Document, ByVal path As String)
            MyBase.new(context,
                                New PdfDictionary(
                                            New PdfName() {PdfName.Type},
                                            New PdfDirectObject() {PdfName.Filespec}
                                            )
                        )
            Me.SetPath(path)
        End Sub

        Friend Sub New(ByVal embeddedFile As EmbeddedFile, ByVal filename As String)
            Me.New(embeddedFile.Document, filename)
            Me.EmbeddedFile = embeddedFile
        End Sub

        Friend Sub New(ByVal context As Document, ByVal url As Uri)
            Me.New(context, url.ToString())
            Me.FileSystem = StandardFileSystemEnum.URL
        End Sub

        Friend Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.new(baseObject)
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary>Gets/Sets the related files.</summary>
        '*/
        Public Property Dependencies As RelatedFiles
            Get
                Return GetDependencies(PdfName.F)
            End Get
            Set(ByVal value As RelatedFiles)
                SetDependencies(PdfName.F, value)
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets the description of the file.</summary>
        '*/
        Public Property Description As String
            Get
                Return CStr(PdfSimpleObject(Of Object).GetValue(BaseDictionary(PdfName.Desc)))
            End Get
            Set(ByVal value As String)
                BaseDictionary(PdfName.Desc) = New PdfTextString(value)
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets the embedded file corresponding to this file.</summary>
        '*/
        Public Property EmbeddedFile As EmbeddedFile
            Get
                Return GetEmbeddedFile(PdfName.F)
            End Get
            Set(ByVal value As EmbeddedFile)
                SetEmbeddedFile(PdfName.F, value)
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets the file system to be used to interpret this file specification.</summary>
        '  <returns>Either <see cref="StandardFileSystemEnum"/> (standard file system) or
        '  <see cref="String"/> (custom file system).</returns>
        '*/
        Public Property FileSystem As Object
            Get
                Dim fileSystemObject As PdfName = CType(BaseDictionary(PdfName.FS), PdfName)
                Dim standardFileSystem As StandardFileSystemEnum? = StandardFileSystemEnumExtension.Get(fileSystemObject)
                If (standardFileSystem.HasValue) Then
                    Return standardFileSystem.Value
                Else
                    Return fileSystemObject.Value
                End If
            End Get
            Set(ByVal value As Object)
                Dim fileSystemObject As PdfName
                If (TypeOf (value) Is StandardFileSystemEnum) Then
                    fileSystemObject = CType(value, StandardFileSystemEnum).GetCode()
                ElseIf (TypeOf (value) Is String) Then
                    fileSystemObject = New PdfName(CStr(value))
                Else
                    Throw New ArgumentException("MUST be either StandardFileSystemEnum (standard file system) or String (custom file system)")
                End If
                BaseDictionary(PdfName.FS) = fileSystemObject
            End Set
        End Property

        Public Overrides Function GetInputStream() As IInputStream
            If (PdfName.URL.Equals(BaseDictionary(PdfName.FS))) Then ' Remote Then resource [PDF:1.7:3.10.4].
                Dim fileUrl As Uri
                Try
                    fileUrl = New Uri(Me.Path)
                Catch e As System.Exception
                    Throw New System.Exception("Failed to instantiate URL for " & Me.Path, e)
                End Try
                Dim WebClient As WebClient = New WebClient()
                Try
                    Return New bytes.Buffer(WebClient.OpenRead(fileUrl))
                Catch e As System.Exception
                    Throw New System.Exception("Failed to open input stream for " & Me.Path, e)
                End Try
            Else ' Local resource [PDF:1.7:3.10.1].
                Return MyBase.GetInputStream()
            End If
        End Function

        Public Overrides Function GetOutputStream() As bytes.IOutputStream
            If (PdfName.URL.Equals(BaseDictionary(PdfName.FS))) Then ' Remote Then resource [PDF:1.7:3.10.4].
                Dim fileUrl As Uri
                Try
                    fileUrl = New Uri(Path)
                Catch e As System.Exception
                    Throw New System.Exception("Failed to instantiate URL for " & Me.Path, e)
                End Try
                Dim webClient As WebClient = New WebClient()
                Try
                    Return New bytes.Stream(webClient.OpenWrite(fileUrl))
                Catch e As System.Exception
                    Throw New System.Exception("Failed to open output stream for " & Me.Path, e)
                End Try
            Else ' Local resource [PDF:1.7:3.10.1].
                Return MyBase.GetOutputStream()
            End If
        End Function

        '/**
        '  <summary>Gets/Sets the identifier of the file.</summary>
        '*/
        Public Property ID As FileIdentifier
            Get
                Return FileIdentifier.Wrap(BaseDictionary(PdfName.ID))
            End Get
            Set(ByVal value As FileIdentifier)
                BaseDictionary(PdfName.ID) = value.BaseObject
            End Set
        End Property

        Public Overrides ReadOnly Property Path As String
            Get
                Return GetPath(PdfName.F)
            End Get
        End Property

        Public Sub SetPath(ByVal value As String)
            SetPath(PdfName.F, value)
        End Sub

        '/**
        '  <summary>Gets/Sets whether the referenced file is volatile (changes frequently with time).
        '  </summary>
        '*/
        Public Property Volatile As Boolean
            Get
                Return CBool(PdfSimpleObject(Of Object).GetValue(BaseDictionary(PdfName.V), False))
            End Get
            Set(ByVal value As Boolean)
                BaseDictionary(PdfName.V) = PdfBoolean.Get(value)
            End Set
        End Property

#End Region

#Region "Private"

        Private ReadOnly Property BaseDictionary As PdfDictionary
            Get
                Return CType(BaseDataObject, PdfDictionary)
            End Get
        End Property

        '/**
        '  <summary>Gets the related files associated to the given key.</summary>
        '*/
        Private Function GetDependencies(ByVal key As PdfName) As RelatedFiles
            Dim dependenciesObject As PdfDictionary = CType(BaseDictionary(PdfName.RF), PdfDictionary)
            If (dependenciesObject Is Nothing) Then Return Nothing
            Return RelatedFiles.Wrap(dependenciesObject(key))
        End Function

        '/**
        '  <summary>Gets the embedded file associated To the given key.</summary>
        '*/
        Private Function GetEmbeddedFile(ByVal key As PdfName) As EmbeddedFile
            Dim embeddedFilesObject As PdfDictionary = CType(BaseDictionary(PdfName.EF), PdfDictionary)
            If (embeddedFilesObject Is Nothing) Then Return Nothing
            Return EmbeddedFile.Wrap(embeddedFilesObject(key))
        End Function

        '/**
        '  <summary>Gets the path associated To the given key.</summary>
        '*/
        Private Function GetPath(ByVal key As PdfName) As String
            Return CStr(PdfSimpleObject(Of Object).GetValue(BaseDictionary(key)))
        End Function

        '/**
        '  <see cref = "GetDependencies(PdfName)" />
        '*/
        Private Sub SetDependencies(ByVal key As PdfName, ByVal value As RelatedFiles)
            Dim dependenciesObject As PdfDictionary = CType(BaseDictionary(PdfName.RF), PdfDictionary)
            If (dependenciesObject Is Nothing) Then
                dependenciesObject = New PdfDictionary()
                BaseDictionary(PdfName.RF) = dependenciesObject
            End If
            dependenciesObject(key) = value.BaseObject
        End Sub

        '/**
        '  <see cref = "GetEmbeddedFile(PdfName)" />
        '*/
        Private Sub SetEmbeddedFile(ByVal key As PdfName, ByVal value As EmbeddedFile)
            Dim embeddedFilesObject As PdfDictionary = CType(BaseDictionary(PdfName.EF), PdfDictionary)
            If (embeddedFilesObject Is Nothing) Then
                embeddedFilesObject = New PdfDictionary()
                BaseDictionary(PdfName.EF) = embeddedFilesObject
            End If
            embeddedFilesObject(key) = value.BaseObject
        End Sub

        '/**
        '  <see cref = "GetPath(PdfName)" />
        '*/
        Private Sub SetPath(ByVal key As PdfName, ByVal value As String)
            Me.BaseDictionary(key) = New PdfString(value)
        End Sub

#End Region
#End Region
#End Region

    End Class

    Module StandardFileSystemEnumExtension

        Private ReadOnly codes As BiDictionary(Of FullFileSpecification.StandardFileSystemEnum, PdfName)

        Sub New()
            codes = New BiDictionary(Of FullFileSpecification.StandardFileSystemEnum, PdfName)
            codes(FullFileSpecification.StandardFileSystemEnum.Native) = Nothing
            codes(FullFileSpecification.StandardFileSystemEnum.URL) = PdfName.URL
        End Sub

        Public Function [Get](ByVal code As PdfName) As FullFileSpecification.StandardFileSystemEnum?
            Return codes.GetKey(code)
        End Function

        <Extension>
        Public Function GetCode(ByVal standardFileSystem As FullFileSpecification.StandardFileSystemEnum) As PdfName
            Return codes(standardFileSystem)
        End Function
    End Module

End Namespace
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
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects

Imports System
Imports System.IO

Namespace DMD.org.dmdpdf.documents.files

    '/**
    '  <summary>Reference to the contents of another file (file specification) [PDF:1.6:3.10.2].</summary>
    '*/
    <PDF(VersionEnum.PDF11)>
    Public MustInherit Class FileSpecification
        Inherits PdfObjectWrapper(Of PdfDirectObject)
        Implements IPdfNamedObjectWrapper


#Region "static"
#Region "public"

        '/**
        '  <summary>Creates a new reference to an external file.</summary>
        '  <param name="context">byval context as Document .</param>
        '  <param name="path">File path.</param>
        '*/
        Public Shared Function [Get](ByVal context As Document, ByVal path As String) As SimpleFileSpecification
            Return CType([Get](context, path, False), SimpleFileSpecification)
        End Function

        '/**
        '  <summary>Creates a New reference To a file.</summary>
        '  <param name = "context" >byval context as Document .</param>
        '  <param name = "path" > File path.</param>
        '  <param name = "full" > Whether the reference Is able To support extended dependencies.</param>
        '*/
        Public Shared Function [Get](ByVal context As Document, ByVal path As String, ByVal full As Boolean) As FileSpecification
            If (full) Then
                Return CType(New FullFileSpecification(context, path), FileSpecification)
            Else
                Return CType(New SimpleFileSpecification(context, path), FileSpecification)
            End If
        End Function

        '/**
        '  <summary>Creates a New reference To an embedded file.</summary>
        '  <param name = "embeddedFile" > Embedded file corresponding To the reference.</param>
        '  <param name = "filename" > name corresponding To the reference.</param>
        '*/
        Public Shared Function [Get](ByVal embeddedFile As EmbeddedFile, ByVal filename As String) As FullFileSpecification
            Return New FullFileSpecification(embeddedFile, filename)
        End Function

        '/**
        '  <summary>Creates a New reference To a remote file.</summary>
        '  <param name = "context" >byval context as Document .</param>
        '  <param name = "url" > Remote file location.</param>
        '*/
        Public Shared Function [Get](ByVal context As Document, ByVal url As Uri) As FullFileSpecification
            Return New FullFileSpecification(context, url)
        End Function

        '/**
        '  <summary>Instantiates an existing file reference.</summary>
        '  <param name = "baseObject" > Base Object.</param>
        '*/
        Public Shared Function Wrap(ByVal BaseObject As PdfDirectObject) As FileSpecification
            If (BaseObject Is Nothing) Then Return Nothing
            Dim BaseDataObject As PdfDataObject = BaseObject.Resolve()
            If (TypeOf (BaseDataObject) Is PdfString) Then
                Return New SimpleFileSpecification(BaseObject)
            ElseIf (TypeOf (BaseDataObject) Is PdfDictionary) Then
                Return New FullFileSpecification(BaseObject)
            Else
                Return Nothing
            End If
        End Function

#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        Protected Sub New(ByVal context As Document, ByVal baseDataObject As PdfDirectObject)
            MyBase.New(context, baseDataObject)
        End Sub

        Protected Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary>Gets the file absolute path.</summary>
        '*/
        Public Function GetAbsolutePath() As String
            Dim path As String = Me.Path
            If (Not System.IO.Path.IsPathRooted(path)) Then ' Path needs to be resolved.
                Dim basePath As String = Document.File.Path
                If (basePath IsNot Nothing) Then
                    path = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(basePath), path)
                End If
            End If
            Return path
        End Function

        '/**
        '  <summary>Gets an input stream to read from the file.</summary>
        '*/
        Public Overridable Function GetInputStream() As bytes.IInputStream
            Return New bytes.Stream(
                            New System.IO.FileStream(
                                                GetAbsolutePath(),
                                                System.IO.FileMode.Open,
                                                System.IO.FileAccess.Read
                                                )
                                      )
        End Function

        '/**
        '  <summary>Gets an output stream to write into the file.</summary>
        '*/
        Public Overridable Function GetOutputStream() As bytes.IOutputStream
            Return New bytes.Stream(
                                New IO.FileStream(
                                        GetAbsolutePath(),
                                        System.IO.FileMode.Create,
                                        System.IO.FileAccess.Write
                                        )
                                        )
        End Function

        '/**
        '  <summary>Gets the file path.</summary>
        '*/
        Public MustOverride ReadOnly Property Path As String

#Region "IPdfNamedObjectWrapper"

        Public ReadOnly Property Name As PdfString Implements IPdfNamedObjectWrapper.Name
            Get
                Return RetrieveName()
            End Get
        End Property

        Public ReadOnly Property NamedBaseObject As PdfDirectObject Implements IPdfNamedObjectWrapper.NamedBaseObject
            Get
                Return RetrieveNamedBaseObject()
            End Get
        End Property

#End Region
#End Region
#End Region
#End Region

    End Class

End Namespace


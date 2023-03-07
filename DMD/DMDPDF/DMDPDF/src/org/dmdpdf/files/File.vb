'/*
'  Copyright 2006-2012 Stefano Chizzolini. http://www.dmdpdf.org

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

'  You should have received a copy of the GNU Lesser General Public License along with Me
'  Program (see README files); if not, go to the GNU website (http://www.gnu.org/licenses/).

'  Redistribution and use, with or without modification, are permitted provided that such
'  redistributions retain the above copyright notice, license and disclaimer, along with
'  Me list of conditions.
'*/

Imports DMD.org.dmdpdf.bytes
Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.documents.contents
Imports DMD.org.dmdpdf.objects
Imports DMD.org.dmdpdf.tokens

Imports System
Imports System.Collections
Imports System.Reflection
Imports System.Text
Imports System.IO

Namespace DMD.org.dmdpdf.files

    '/**
    '  <summary>PDF file representation.</summary>
    '*/
    Public NotInheritable Class File
        Implements IDisposable

#Region "types"
        '/**
        '  <summary>File configuration.</summary>
        '*/
        Public NotInheritable Class ConfigurationImpl

            Private _realFormat As String

            Private ReadOnly _file As File

            Friend Sub New(ByVal file As File)
                Me._file = File
            End Sub

            '/**
            '  <summary>Gets the file associated With Me configuration.</summary>
            '*/
            Public ReadOnly Property File As File
                Get
                    Return Me._file
                End Get
            End Property

            '/**
            '  <summary>Gets/Sets the format applied To real number serialization.</summary>
            '*/
            Public Property RealFormat As String
                Get
                    If (_realFormat Is Nothing) Then
                        SetRealFormat(5)
                    End If
                    Return _realFormat
                End Get
                Set(ByVal value As String)
                    _realFormat = value
                End Set
            End Property

            '/**
            '  <param name = "decimalPlacesCount" > Number Of digits In Decimal places.</param>
            '  <seealso cref = "RealFormat" />
            '*/
            Public Sub SetRealFormat(ByVal decimalPlacesCount As Integer)
                _realFormat = "0." & New String("#"c, decimalPlacesCount)
            End Sub
        End Class

        Private NotInheritable Class ImplicitContainer
            Inherits PdfIndirectObject

            Public Sub New(ByVal file As File, ByVal dataObject As PdfDataObject)
                MyBase.New(file, dataObject, New XRefEntry(Integer.MinValue, Integer.MinValue))
            End Sub

        End Class

#End Region

#Region "Static"
#Region "fields"

        Private Shared hashCodeGenerator As Random = New Random()

#End Region
#End Region

#Region "dynamic"
#Region "fields"

        Private _configuration As ConfigurationImpl
        Private ReadOnly _document As Document
        Private ReadOnly _hashCode As Integer = hashCodeGenerator.Next()
        Private ReadOnly _indirectObjects As IndirectObjects
        Private _path As String
        Private _reader As Reader
        Private ReadOnly _trailer As PdfDictionary
        Private ReadOnly _version As Version

        Private _cloner As Cloner

#End Region

#Region "constructors"

        Public Sub New()
            Me.Initialize()
            Me._version = VersionEnum.PDF14.GetVersion()
            Me._trailer = PrepareTrailer(New PdfDictionary())
            Me._indirectObjects = New IndirectObjects(Me, Nothing)
            Me._document = New Document(Me)
        End Sub

        Public Sub New(ByVal path As String)
            Me.New(
                    New bytes.Stream(
                        New FileStream(
                        path,
                        FileMode.Open,
                        FileAccess.Read
                        )
                    )
                 )
            Me._path = path
        End Sub


        Public Sub New(ByVal stream As IInputStream)
            Me.Initialize()
            Me._reader = New Reader(stream, Me)
            Dim info As Reader.FileInfo = _reader.ReadInfo()
            Me._version = info.Version
            Me._trailer = PrepareTrailer(info.Trailer)
            If (_trailer.ContainsKey(PdfName.Encrypt)) Then ' Encrypted Then File.
                Throw New NotImplementedException("Encrypted files are currently not supported.")
            End If

            _indirectObjects = New IndirectObjects(Me, info.XrefEntries)
            _document = New Document(_trailer(PdfName.Root))
            If (PdfName.XRef.Equals(_trailer(PdfName.Type))) Then
                _document.Configuration.XrefMode = Document.ConfigurationImpl.XRefModeEnum.Compressed
            Else
                _document.Configuration.XrefMode = Document.ConfigurationImpl.XRefModeEnum.Plain
            End If
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary>Gets/Sets the Default cloner.</summary>
        '*/
        Public Property Cloner As Cloner
            Get
                If (_cloner Is Nothing) Then
                    _cloner = New Cloner(Me)
                End If
                Return _cloner
            End Get
            Set(ByVal value As Cloner)
                _cloner = value
            End Set
        End Property

        '/**
        '  <summary>Gets the file configuration.</summary>
        '*/
        Public ReadOnly Property Configuration As ConfigurationImpl
            Get
                Return _configuration
            End Get
        End Property

        '/**
        '  <summary>Gets the high-level representation Of the file content.</summary>
        '*/
        Public ReadOnly Property Document As Document
            Get
                Return _document
            End Get
        End Property

        Public Overrides Function GetHashCode() As Integer
            Return _hashCode
        End Function

        '/**
        '  <summary>Gets the identifier Of Me file.</summary>
        '*/
        Public ReadOnly Property ID As FileIdentifier
            Get
                Return FileIdentifier.Wrap(trailer(PdfName.ID))
            End Get
        End Property

        '/**
        '  <summary>Gets the indirect objects collection.</summary>
        '*/
        Public ReadOnly Property IndirectObjects As IndirectObjects
            Get
                Return Me._indirectObjects
            End Get
        End Property

        '/**
        '  <summary>Gets/Sets the file path.</summary>
        '*/
        Public Property Path As String
            Get
                Return Me._path
            End Get
            Set(ByVal value As String)
                Me._path = value
            End Set
        End Property

        '/**
        '  <summary>Gets the data reader backing Me file.</summary>
        '  <returns><code>Nothing</code> In Case Of newly-created file.</returns>
        '*/
        Public ReadOnly Property Reader As Reader
            Get
                Return Me._reader
            End Get
        End Property

        '/**
        '  <summary>Registers an <b>internal data Object</b>.</summary>
        '*/
        Public Function Register(ByVal obj As PdfDataObject) As PdfReference
            Return _indirectObjects.Add(obj).Reference
        End Function

        '/**
        '  <summary>Serializes the file To the current file-system path Using the <see
        '  cref = "SerializationModeEnum.Standard" > standard serialization mode</see>.</summary>
        '*/
        Public Sub Save()
            Save(SerializationModeEnum.Standard)
        End Sub

        '/**
        '  <summary>Serializes the file To the current file-system path.</summary>
        '  <param name = "mode" > Serialization mode.</param>
        '*/
        Public Sub Save(ByVal mode As SerializationModeEnum)
            If (Not System.IO.File.Exists(_path)) Then
                Throw New FileNotFoundException("No valid source path available.")
            End If

            '/*
            '  NOTE:       The Document file cannot be directly overwritten as it's locked for reading by the
            '              open Stream; its update Is therefore delayed To its disposal, when the temporary file will
            '  overwrite It(see Dispose() method).
            '*/
            Save(TempPath, mode)
        End Sub

        '/**
        '  <summary>Serializes the file To the specified file system path.</summary>
        '  <param name = "path" > Target path.</param>
        '  <param name = "mode" > Serialization mode.</param>
        '*/
        Public Sub Save(ByVal path As String, ByVal mode As SerializationModeEnum)
            Dim outputStream As FileStream = New System.IO.FileStream(
                                                path,
                                                System.IO.FileMode.Create,
                                                System.IO.FileAccess.Write
                                                )
            Save(New bytes.Stream(outputStream), mode)
            outputStream.Flush()
            outputStream.Close()
            outputStream.Dispose()
            outputStream = Nothing
        End Sub

        '/**
        '  <summary>Serializes the file To the specified stream.</summary>
        '  <remarks>It's caller responsibility to close the stream after Me method ends.</remarks>
        '  <param name = "stream" > Target stream.</param>
        '  <param name = "mode" > Serialization mode.</param>
        '*/
        Public Sub Save(ByVal Stream As IOutputStream, ByVal mode As SerializationModeEnum)
            If (Reader Is Nothing) Then
                Try
                    Dim assemblyTitle As String = CType(Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), GetType(AssemblyTitleAttribute)), AssemblyTitleAttribute).Title
                    Dim assemblyVersion As String = Assembly.GetExecutingAssembly().GetName().Version.ToString()
                    Document.Information.Producer = assemblyTitle & " " & assemblyVersion
                Catch ex As System.Exception
                    '/* NOOP *
                End Try
            End If

            Dim Writer As Writer = Writer.Get(Me, Stream)
            Writer.Write(mode)
        End Sub

        '/**
        '  <summary>Gets the file trailer.</summary>
        '*/
        Public ReadOnly Property Trailer As PdfDictionary
            Get
                Return Me._trailer
            End Get
        End Property

        '/**
        '  <summary>Unregisters an internal Object.</summary>
        '*/
        Public Sub Unregister(ByVal reference As PdfReference)
            Me._indirectObjects.RemoveAt(reference.ObjectNumber)
        End Sub

        '/**
        '  <summary>Gets the file header version [PDF:1.6:3.4.1].</summary>
        '  <remarks>This Property represents just the original file version; To Get the actual version,
        '  use the < see cref="org.dmdpdf.documents.Document.Version">Document.Version</see> method.
        '  </remarks>
        '*/
        Public ReadOnly Property Version As Version
            Get
                Return Me._version
            End Get
        End Property

#Region "IDisposable"

        Public Sub Dispose() Implements IDisposable.Dispose
            If (Me._reader IsNot Nothing) Then
                Me._reader.Dispose()
            End If
            Me._reader = Nothing

            '/*
            '  NOTE:     If the Then temporary File exists (see Save() method), it must overwrite the document file.
            '*/
            If (System.IO.File.Exists(TempPath)) Then
                System.IO.File.Delete(_path)
                System.IO.File.Move(TempPath, _path)
            End If

            'GC.SuppressFinalize(Me)
        End Sub

#End Region
#End Region

#Region "Private"

        Private Sub Initialize()
            _configuration = New ConfigurationImpl(Me)
        End Sub


        Private Function PrepareTrailer(ByVal trailer As PdfDictionary) As PdfDictionary
            Return CType(New ImplicitContainer(Me, trailer).DataObject, PdfDictionary)
        End Function

        Private ReadOnly Property TempPath As String
            Get
                If (_path Is Nothing) Then
                    Return Nothing
                Else
                    Return _path & ".tmp"
                End If
            End Get
        End Property

#End Region
#End Region
#End Region

    End Class

End Namespace
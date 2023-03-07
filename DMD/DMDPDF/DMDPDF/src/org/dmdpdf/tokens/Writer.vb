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
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects

Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Text

Namespace DMD.org.dmdpdf.tokens

    '/**
    '  <summary>PDF file writer.</summary>
    '*/
    Public MustInherit Class Writer

#Region "Static"
#Region "fields"

        Private Shared ReadOnly _BOFChunk As Byte() = Encoding.Pdf.Encode(Keyword.BOF)
        Private Shared ReadOnly _EOFChunk As Byte() = Encoding.Pdf.Encode(Symbol.LineFeed & Keyword.EOF & Symbol.CarriageReturn & Symbol.LineFeed)
        Private Shared ReadOnly _HeaderBinaryHintChunk As Byte() = New Byte() {
                                                            Asc(Symbol.LineFeed),
                                                            Asc(Symbol.Percent),
                                                            &H80, &H80, &H80, &H80, Asc(Symbol.LineFeed)
                                                            } ' NOTE: Arbitrary binary characters (code >= 128) For ensuring proper behavior Of file transfer applications [PDF:1.6:3.4.1].
        Private Shared ReadOnly _StartXRefChunk As Byte() = Encoding.Pdf.Encode(Keyword.StartXRef & Symbol.LineFeed)

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary>Gets a new writer instance for the specified file.</summary>
        '  <param name="file">File to serialize.</param>
        '  <param name="stream">Target stream.</param>
        '*/
        Public Shared Function [Get](ByVal file As files.File, ByVal stream As IOutputStream) As Writer
            ' Which cross-reference table mode?
            Select Case (file.Document.Configuration.XrefMode)
                Case Document.ConfigurationImpl.XRefModeEnum.Plain : Return New PlainWriter(file, stream)
                Case Document.ConfigurationImpl.XRefModeEnum.Compressed : Return New CompressedWriter(file, stream)
                Case Else : Throw New NotSupportedException()
            End Select
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "fields"

        Protected ReadOnly _file As files.File
        Protected ReadOnly _stream As IOutputStream

#End Region

#Region "constructors"

        Protected Sub New(ByVal file As files.File, ByVal stream As IOutputStream)
            Me._file = file
            Me._stream = stream
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary>Gets the file To serialize.</summary>
        '*/
        Public ReadOnly Property File As files.File
            Get
                Return Me._file
            End Get
        End Property

        '/**
        '  <summary>Gets the target stream.</summary>
        '*/
        Public ReadOnly Property Stream As IOutputStream
            Get
                Return Me._stream
            End Get
        End Property

        '/**
        '  <summary>Serializes the <see cref="File">file</see> To the <see cref="Stream">target stream</see>.</summary>
        '  <param name = "mode" > Serialization mode.</param>
        ' */
        Public Sub Write(ByVal mode As SerializationModeEnum)
            Select Case (mode)
                Case SerializationModeEnum.Incremental
                    If (_file.Reader Is Nothing) Then
                        'GoTo case SerializationModeEnum.Standard;
                        WriteStandard()
                    Else
                        WriteIncremental()
                    End If
                Case SerializationModeEnum.Standard
                    WriteStandard()

                Case SerializationModeEnum.Linearized
                    WriteLinearized()
            End Select

        End Sub

#End Region

#Region "protected"

        '/**
        '  <summary>Updates the specified trailer.</summary>
        '  <remarks>This method has To be called just before serializing the trailer Object.</remarks>
        '*/
        Protected Sub UpdateTrailer(ByVal trailer As PdfDictionary, ByVal stream As IOutputStream)
            ' File identifier update.
            Dim identifier As FileIdentifier = FileIdentifier.Wrap(trailer(PdfName.ID))
            If (identifier Is Nothing) Then
                identifier = New FileIdentifier()
                trailer(PdfName.ID) = identifier.BaseObject
            End If
            identifier.Update(Me)
        End Sub

        '/**
        '  <summary>Serializes the beginning of the file [PDF:1.6:3.4.1].</summary>
        '*/
        Protected Sub WriteHeader()
            _stream.Write(_BOFChunk)
            _stream.Write(_file.Document.Version.ToString()) ' NOTE: Document version represents the actual (possibly-overridden) file version.
            _stream.Write(_HeaderBinaryHintChunk)
        End Sub

        '/**
        '  <summary>Serializes the PDF file as incremental update [PDF:1.6:3.4.5].</summary>
        '*/
        Protected MustOverride Sub WriteIncremental()

        '/**
        '  <summary>Serializes the PDF file linearized [PDF:1.6:F].</summary>
        '*/
        Protected MustOverride Sub WriteLinearized()

        '/**
        '  <summary>Serializes the PDF file compactly [PDF:1.6:3.4].</summary>
        '*/
        Protected MustOverride Sub WriteStandard()

        '/**
        '  <summary>Serializes the end of the file [PDF:1.6:3.4.4].</summary>
        '  <param name="startxref">Byte offset from the beginning of the file to the beginning
        '    of the last cross-reference section.</param>
        '*/
        Protected Sub WriteTail(ByVal startxref As Long)
            _stream.Write(_StartXRefChunk)
            _stream.Write(startxref.ToString())
            _stream.Write(_EOFChunk)
        End Sub

#End Region
#End Region
#End Region

    End Class

End Namespace

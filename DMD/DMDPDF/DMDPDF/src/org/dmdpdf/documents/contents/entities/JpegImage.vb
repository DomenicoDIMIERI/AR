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

'  You should have received a copy of the GNU Lesser General Public License along with this
'  Program (see README files); if not, go to the GNU website (http://www.gnu.org/licenses/).

'  Redistribution and use, with or without modification, are permitted provided that such
'  redistributions retain the above copyright notice, license and disclaimer, along with
'  this list of conditions.
'*/

Imports DMD.org.dmdpdf.bytes
Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.documents.contents.composition
Imports DMD.org.dmdpdf.documents.contents.objects
Imports DMD.org.dmdpdf.documents.contents.xObjects
Imports DMD.org.dmdpdf.objects
Imports DMD.org.dmdpdf.util.io

Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.IO

Namespace DMD.org.dmdpdf.documents.contents.entities

    '/**
    '  <summary>JPEG image object [ISO 10918-1;JFIF:1.02].</summary>
    '*/
    Public NotInheritable Class JpegImage
        Inherits Image

#Region "dynamic"
#Region "constructors"

        Public Sub New(ByVal stream As System.IO.Stream)
            MyBase.New(stream)
            Load()
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public Overrides Function ToInlineObject(ByVal composer As PrimitiveComposer) As ContentObject
            Return composer.Add(
                                New InlineImage(
                                            New InlineImageHeader(
                                                        New List(Of PdfDirectObject)(
                                                                New PdfDirectObject() {
                                                                        PdfName.W, PdfInteger.Get(Width),
                                                                        PdfName.H, PdfInteger.Get(Height),
                                                                        PdfName.CS, PdfName.RGB,
                                                                        PdfName.BPC, PdfInteger.Get(BitsPerComponent),
                                                                        PdfName.F, PdfName.DCT
                                                                        }
                                                                        )
                                                            ),
                                            New InlineImageBody(
                                                        New bytes.Buffer(Stream)
                                                        )
                                    )
                            )
        End Function

        Public Overrides Function ToXObject(ByVal context As Document) As xObjects.XObject
            Return New xObjects.ImageXObject(
                                        context,
                                        New PdfStream(
                                            New PdfDictionary(
                                                New PdfName() {
                                                    PdfName.Width,
                                                    PdfName.Height,
                                                    PdfName.BitsPerComponent,
                                                    PdfName.ColorSpace,
                                                    PdfName.Filter
                                                    },
                                                New PdfDirectObject() {
                                                    PdfInteger.Get(Width),
                                                    PdfInteger.Get(Height),
                                                    PdfInteger.Get(BitsPerComponent),
                                                    PdfName.DeviceRGB,
                                                    PdfName.DCTDecode
                                                    }
                                                ),
                                        New bytes.Buffer(Stream)
                                    )
                                )
        End Function

#End Region

#Region "Private"

        Private Sub Load()
            '      /*
            '  NOTE: Big-endian data expected.
            '*/
            Dim stream As System.IO.Stream = Me.Stream
            Dim streamReader As BigEndianBinaryReader = New BigEndianBinaryReader(stream)

            Dim index As Integer = 4
            stream.Seek(index, SeekOrigin.Begin)
            Dim markerBytes As Byte() = New Byte(2 - 1) {}
            While (True)
                index += streamReader.ReadUInt16()
                stream.Seek(index, SeekOrigin.Begin)

                stream.Read(markerBytes, 0, 2)
                index += 2

                ' Frame header?
                If (markerBytes(0) = &HFF AndAlso
                    markerBytes(1) = &HC0) Then
                    stream.Seek(2, SeekOrigin.Current)
                    ' Get the image bits per color component (sample precision)!
                    BitsPerComponent = stream.ReadByte()
                    ' Get the image size!
                    Me.Height = streamReader.ReadUInt16()
                    Me.Width = streamReader.ReadUInt16()

                    Exit While
                End If
            End While
        End Sub

#End Region
#End Region
#End Region

    End Class

End Namespace
'/*
'  Copyright 2006-2011 Stefano Chizzolini. http://www.dmdpdf.org

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
Imports DMD.org.dmdpdf.documents.contents.objects

Imports System
Imports System.IO

Namespace DMD.org.dmdpdf.documents.contents.entities

    '/**
    '  <summary>Abstract image object [PDF:1.6:4.8].</summary>
    '*/
    Public MustInherit Class Image
        Inherits Entity

#Region "Static"
#Region "Interface"
#Region "Public"

        Public Shared Function [Get](ByVal path As String) As Image
            Return [Get](New FileStream(
                                      path,
                                      FileMode.Open,
                                      FileAccess.Read
                                      )
                                      )
        End Function

        Public Shared Function [Get](ByVal stream As System.IO.Stream) As Image
            ' Get the format identifier!
            Dim formatMarkerBytes As Byte() = New Byte(2 - 1) {}
            stream.Read(formatMarkerBytes, 0, 2)
            '// Is JPEG?
            '/*
            '  NOTE: JPEG files are identified by a SOI (Start Of Image) marker [ISO 10918-1].
            '*/
            If (formatMarkerBytes(0) = &HFF AndAlso
                formatMarkerBytes(1) = &HD8) Then ' JPEG.
                Return New JpegImage(stream)
            Else ' Unknown.
                Return Nothing
            End If
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "fields"

        Private _bitsPerComponent As Integer
        Private _height As Integer
        Private _width As Integer

        Private _stream As System.IO.Stream

#End Region

#Region "constructors"

        Protected Sub New(ByVal stream As System.IO.Stream)
            Me._stream = stream
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary> Gets/Sets the number Of bits per color component
        '  [PDF:1.6:4.8.2].</summary>
        '*/
        Public Property BitsPerComponent As Integer
            Get
                Return _bitsPerComponent
            End Get
            Protected Set(ByVal value As Integer)
                _bitsPerComponent = value
            End Set
        End Property

        '/**
        '  <summary> Gets/Sets the height Of the image In samples [PDF:1.6:4.8.2].</summary>
        '*/
        Public Property Height As Integer
            Get
                Return _height
            End Get
            Protected Set(ByVal value As Integer)
                _height = value
            End Set
        End Property

        '/**
        '  <summary> Gets/Sets the width Of the image In samples [PDF:1.6:4.8.2].</summary>
        '*/
        Public Property Width As Integer
            Get
                Return _width
            End Get
            Protected Set(ByVal value As Integer)
                _width = value
            End Set
        End Property

#End Region

#Region "protected"

        '/**
        '  <summary> Gets the underlying stream.</summary>
        '*/
        Protected ReadOnly Property Stream As System.IO.Stream
            Get
                Return _stream
            End Get
        End Property

#End Region
#End Region
#End Region

    End Class

End Namespace

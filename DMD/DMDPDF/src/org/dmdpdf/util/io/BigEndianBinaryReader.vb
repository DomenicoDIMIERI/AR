'/*
'  Copyright 2007-2010 Stefano Chizzolini. http://www.dmdpdf.org

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

Imports System
Imports System.IO

Namespace DMD.org.dmdpdf.util.io

    '/**
    '  <summary>Reads primitive data types as binary values using big-endian encoding.</summary>
    '  <remarks>This implementation was necessary because the official framework's binary reader supports
    '  only the little-endian encoding.</remarks>
    '*/
    Public Class BigEndianBinaryReader
        ' Implements IDisposable

#Region "dynamic"
#Region "fields"
        Private _stream As Stream
        Private disposed As Boolean = False
#End Region

#Region "constructors"

        Public Sub New(ByVal _stream As Stream)
            Me._stream = _stream
        End Sub

#End Region

#Region "interface"
#Region "public"
        '/**
        '  <summary> Gets the underlying _stream.</summary>
        '*/
        Public ReadOnly Property BaseStream As Stream
            Get
                Return Me._stream
            End Get
        End Property

        '/**
        '  <summary> Closes the reader, including the underlying _stream.</summary>
        '*/
        Public Sub Close()
            Me.Dispose()
        End Sub

        '/**
        '  <summary> Reads a 2-Byte signed Integer from the current _stream And advances the current position
        '  of the _stream by two bytes.</summary>
        '*/
        Public Function ReadInt16() As Short
            Return CShort(Me._stream.ReadByte() << 8 Or Me._stream.ReadByte())
        End Function

        '/**
        '  <summary> Reads a 4-Byte signed Integer from the current _stream And advances the current position
        '  of the _stream by four bytes.</summary>
        '*/
        Public Function ReadInt32() As Integer
            Return (_stream.ReadByte() << 24 Or _stream.ReadByte() << 16 Or _stream.ReadByte() << 8 Or _stream.ReadByte())
        End Function

        '/**
        '  <summary> Reads a 2-Byte unsigned Integer from the current _stream And advances the position Of the
        '  _stream by two bytes.</summary>
        '*/
        Public Function ReadUInt16() As UShort
            Return CUShort(Me._stream.ReadByte() << 8 Or Me._stream.ReadByte())
        End Function

        '/**
        '  <summary> Reads a 4-Byte unsigned Integer from the current _stream And advances the position Of the
        '  _stream by four bytes.</summary>
        '*/
        Public Function ReadUInt32() As UInteger
            Return CUInt(CUInt(_stream.ReadByte()) << 24 Or CUInt(_stream.ReadByte()) << 16 Or CUInt(_stream.ReadByte()) << 8 Or CUInt(_stream.ReadByte()))
        End Function

#Region "IDisposable"

        Public Sub Dispose() 'Implements IDisposable.Dispose

            If (disposed) Then Return
            disposed = True
            If (Me._stream IsNot Nothing) Then
                CType(_stream, IDisposable).Dispose()
                Me._stream = Nothing
            End If
            'GC.SuppressFinalize(Me)
        End Sub
#End Region
#End Region
#End Region
#End Region
    End Class

End Namespace

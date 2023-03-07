'/*
'  Copyright 2006-2010 Stefano Chizzolini. http://www.dmdpdf.org

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

Imports DMD.org.dmdpdf.util
Imports DMD.org.dmdpdf.util.io

Imports System

Namespace DMD.org.dmdpdf.bytes

    '  /**
    '  <summary>
    '    <para>Input stream.</para>
    '    <para>Its pivotal concept is the access pointer.</para>
    '  </summary>
    '*/
    Public Interface IInputStream
        Inherits IStream, IDataWrapper

        '    /**
        '  <summary>Gets/Sets the byte order.</summary>
        '*/
        Property ByteOrder As ByteOrderEnum

        '/**
        '  <summary>Gets the hash representation of the sequence.</summary>
        '*/
        Function GetHashCode() As Integer

        '/**
        '  <summary>Gets/Sets the pointer position.</summary>
        '*/
        Property Position As Long

        '/**
        '  <summary>Reads a sequence of bytes.</summary>
        '  <remarks>This operation causes the stream pointer to advance after the read data.</remarks>
        '  <param name="data">Target byte array.</param>
        '*/
        Sub Read(ByVal data As Byte())

        '/**
        '  <summary>Reads a sequence of bytes.</summary>
        '  <remarks>This operation causes the stream pointer to advance after the read data.</remarks>
        '  <param name="data">Target byte array.</param>
        '  <param name="offset">Location in the byte array at which storing begins.</param>
        '  <param name="length">Number of bytes to read.</param>
        '*/
        Sub Read(ByVal data As Byte(), ByVal offset As Integer, ByVal length As Integer)

        '/**
        '  <summary>Reads a byte.</summary>
        '  <remarks>This operation causes the stream pointer to advance after the read data.</remarks>
        '*/
        Function ReadByte() As Integer

        '/**
        '  <summary>Reads an integer.</summary>
        '  <remarks>This operation causes the stream pointer to advance after the read data.</remarks>
        '*/
        Function ReadInt() As Integer

        '/**
        '  <summary>Reads a variable-length integer.</summary>
        '  <remarks>This operation causes the stream pointer to advance after the read data.</remarks>
        '  <param name="length">Number of bytes to read.</param>
        '*/
        Function ReadInt(ByVal length As Integer) As Integer

        '/**
        '  <summary>Reads the next line of text.</summary>
        '  <remarks>This operation causes the stream pointer to advance after the read data.</remarks>
        '*/
        Function ReadLine() As String

        '/**
        '  <summary>Reads a short integer.</summary>
        '  <remarks>This operation causes the stream pointer to advance after the read data.</remarks>
        '*/
        Function ReadShort() As Short

        '/**
        '  <summary>Reads a signed byte integer.</summary>
        '  <remarks>This operation causes the stream pointer to advance after the read data.</remarks>
        '*/
        Function ReadSignedByte() As SByte

        '/**
        '  <summary>Reads a string.</summary>
        '  <remarks>This operation causes the stream pointer to advance after the read data.</remarks>
        '  <param name="length">Number of bytes to read.</param>
        '*/
        Function ReadString(ByVal length As Integer) As String

        '/**
        '  <summary>Reads an unsigned short integer.</summary>
        '  <remarks>This operation causes the stream pointer to advance after the read data.</remarks>
        '*/
        Function ReadUnsignedShort() As UShort

        '/**
        '  <summary>Sets the pointer absolute position.</summary>
        '*/
        Sub Seek(ByVal position As Long)

        '/**
        '  <summary>Sets the pointer relative position.</summary>
        '*/
        Sub Skip(ByVal offset As Long)

    End Interface


End Namespace


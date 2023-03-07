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

Imports DMD.org.dmdpdf.bytes.filters
Imports DMD.org.dmdpdf.objects

Imports System

Namespace DMD.org.dmdpdf.bytes

    '/**
    '  <summary>
    '    <para>Buffer.</para>
    '    <para>Its pivotal concept is the array index.</para>
    '  </summary>
    '  <returns>This buffer.</returns>
    '*/
    Public Interface IBuffer
        Inherits IInputStream, IOutputStream
        '/**
        '  <summary>Appends a byte to the buffer.</summary>
        '  <param name="data">Byte to copy.</param>
        '  <returns>This buffer.</returns>
        '*/
        Function Append(ByVal data As Byte) As IBuffer

        '/**
        '  <summary>Appends a byte array to the buffer.</summary>
        '  <param name="data">Byte array to copy.</param>
        '  <returns>This buffer.</returns>
        '*/
        Function Append(ByVal data As Byte()) As IBuffer

        '/**
        '  <summary>Appends a byte range to the buffer.</summary>
        '  <param name="data">Byte array from which the byte range has to be copied.</param>
        '  <param name="offset">Location in the byte array at which copying begins.</param>
        '  <param name="length">Number of bytes to copy.</param>
        '  <returns>This buffer.</returns>
        '*/
        Function Append(ByVal data As Byte(), ByVal offset As Integer, ByVal length As Integer) As IBuffer

        '/**
        '  <summary>Appends a string to the buffer.</summary>
        '  <param name="data">String to copy.</param>
        '  <returns>This buffer.</returns>
        '*/
        Function Append(ByVal data As String) As IBuffer

        '/**
        '  <summary>Appends an IInputStream to the buffer.</summary>
        '  <param name="data">Source data to copy.</param>
        '  <returns>This buffer.</returns>
        '*/
        Function Append(ByVal data As IInputStream) As IBuffer

        '/**
        '  <summary>Appends a stream to the buffer.</summary>
        '  <param name="data">Source data to copy.</param>
        '  <returns>This buffer.</returns>
        '*/
        Function Append(ByVal data As System.IO.Stream) As IBuffer

        '/**
        '  <summary>Gets the allocated buffer size.</summary>
        '  <returns>Allocated buffer size.</returns>
        '*/
        ReadOnly Property Capacity As Integer

        '/**
        '  <summary>Gets a clone of the buffer.</summary>
        '  <returns>Deep copy of the buffer.</returns>
        '*/
        Function Clone() As IBuffer

        '/**
        '  <summary>Applies the specified filter to decode the buffer.</summary>
        '  <param name="filter">Filter to use for decoding the buffer.</param>
        '  <param name="parameters">Decoding parameters.</param>
        '*/
        Sub Decode(ByVal filter As Filter, ByVal parameters As PdfDictionary)

        '/**
        '  <summary>Deletes a byte chunk from the buffer.</summary>
        '  <param name="index">Location at which deletion has to begin.</param>
        '  <param name="length">Number of bytes to delete.</param>
        '*/
        Sub Delete(ByVal index As Integer, ByVal length As Integer)

        '/**
        '  <summary>Gets/Sets whether this buffer has changed.</summary>
        '*/
        Property Dirty As Boolean

        '/**
        '  <summary>Applies the specified filter to encode the buffer.</summary>
        '  <param name="filter">Filter to use for encoding the buffer.</param>
        '  <param name="parameters">Encoding parameters.</param>
        '  <returns>Encoded buffer.</returns>
        '*/
        Function Encode(ByVal filter As Filter, ByVal parameters As PdfDictionary) As Byte()

        '/**
        '  <summary>Gets the byte at a specified location.</summary>
        '  <param name="index">A location in the buffer.</param>
        '  <returns>Byte at the specified location.</returns>
        '*/
        Function GetByte(ByVal index As Integer) As Integer

        '/**
        '  <summary>Gets the byte range beginning at a specified location.</summary>
        '  <param name="index">Location at which the byte range has to begin.</param>
        '  <param name="length">Number of bytes to copy.</param>
        '  <returns>Byte range beginning at the specified location.</returns>
        '*/
        Function GetByteArray(ByVal index As Integer, ByVal length As Integer) As Byte()

        '/**
        '  <summary>Gets the string beginning at a specified location.</summary>
        '  <param name="index">Location at which the string has to begin.</param>
        '  <param name="length">Number of bytes to convert.</param>
        '  <returns>String beginning at the specified location.</returns>
        '*/
        Function GetString(ByVal index As Integer, ByVal length As Integer) As String

        '/**
        '  <summary>Inserts a byte array into the buffer.</summary>
        '  <param name="index">Location at which the byte array has to be inserted.</param>
        '  <param name="data">Byte array to insert.</param>
        '*/
        Sub Insert(ByVal index As Integer, ByVal data As Byte())

        '/**
        '  <summary>Inserts a byte range into the buffer.</summary>
        '  <param name="index">Location at which the byte range has to be inserted.</param>
        '  <param name="data">Byte array from which the byte range has to be copied.</param>
        '  <param name="offset">Location in the byte array at which copying begins.</param>
        '  <param name="length">Number of bytes to copy.</param>
        '*/
        Sub Insert(ByVal index As Integer, ByVal data As Byte(), ByVal offset As Integer, ByVal length As Integer)

        '/**
        '  <summary>Inserts a string into the buffer.</summary>
        '  <param name="index">Location at which the string has to be inserted.</param>
        '  <param name="data">String to insert.</param>
        '*/
        Sub Insert(ByVal index As Integer, ByVal data As String)

        '/**
        '  <summary>Inserts an IInputStream into the buffer.</summary>
        '  <param name="index">Location at which the IInputStream has to be inserted.</param>
        '  <param name="data">Source data to copy.</param>
        '*/
        Sub Insert(ByVal index As Integer, ByVal data As IInputStream)

        '/**
        '  <summary>Notifies the dirtiness of the observed buffer.</summary>
        '*/
        Event OnChange(ByVal sender As Object, ByVal e As System.EventArgs) 'EventHandler

        '/**
        '  <summary>Replaces the buffer contents with a byte array.</summary>
        '  <param name="index">Location at which the byte array has to be copied.</param>
        '  <param name="data">Byte array to copy.</param>
        '*/
        Sub Replace(ByVal index As Integer, ByVal data As Byte())

        '/**
        '  <summary>Replaces the buffer contents with a byte range.</summary>
        '  <param name="index">Location at which the byte range has to be copied.</param>
        '  <param name="data">Byte array from which the byte range has to be copied.</param>
        '  <param name="offset">Location in the byte array at which copying begins.</param>
        '  <param name="length">Number of bytes to copy.</param>
        '*/
        Sub Replace(ByVal index As Integer, ByVal data As Byte(), ByVal offset As Integer, ByVal length As Integer)

        '/**
        '  <summary>Replaces the buffer contents with a string.</summary>
        '  <param name="index">Location at which the string has to be copied.</param>
        '  <param name="data">String to copy.</param>
        '*/
        Sub Replace(ByVal index As Integer, ByVal data As String)

        '/**
        '  <summary>Replaces the buffer contents with an IInputStream.</summary>
        '  <param name="index">Location at which the IInputStream has to be copied.</param>
        '  <param name="data">Source data to copy.</param>
        '*/
        Sub Replace(ByVal index As Integer, ByVal data As IInputStream)

        '/**
        '  <summary>Sets the used buffer size.</summary>
        '  <param name="value">New length.</param>
        '*/
        Sub SetLength(ByVal value As Integer)

        '/**
        '  <summary>Writes the buffer data to a stream.</summary>
        '  <param name="stream">Target stream.</param>
        '*/
        Sub WriteTo(ByVal stream As IOutputStream)

    End Interface

End Namespace


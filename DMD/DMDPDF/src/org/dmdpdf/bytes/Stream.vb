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

Imports DMD.org.dmdpdf.tokens
Imports DMD.org.dmdpdf.util
Imports DMD.org.dmdpdf.util.io

Imports System
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Text

Namespace DMD.org.dmdpdf.bytes

    '/**
    '  <summary>Generic stream.</summary>
    '*/
    Public NotInheritable Class Stream
        Implements IInputStream, IOutputStream

#Region "dynamic"
#Region "fields"

        Private _stream As System.IO.Stream
        Private _byteOrder As ByteOrderEnum = ByteOrderEnum.BigEndian

#If DEBUG Then
        Private _buffer As String
#End If
#End Region


#Region "constructors"

        Public Sub New(ByVal stream As System.IO.Stream)
            If (stream Is Nothing) Then Throw New ArgumentNullException("stream")
            Me._stream = stream
#If DEBUG Then
            Me._buffer = ""
#End If
        End Sub

#End Region

#Region "Interface"

#Region "Public"

#Region "IInputStream"

        Public Property ByteOrder As ByteOrderEnum Implements IInputStream.ByteOrder
            Get
                Return Me._byteOrder
            End Get
            Set(ByVal value As ByteOrderEnum)
                Me._byteOrder = value
            End Set
        End Property

        Public Overrides Function GetHashCode() As Integer Implements IInputStream.GetHashCode
            Return Me._stream.GetHashCode()
        End Function



        Public Property Position As Long Implements IInputStream.Position
            Get
                Return Me._stream.Position
            End Get
            Set(ByVal value As Long)
                Me._stream.Position = value
            End Set
        End Property


        Public Sub Read(ByVal data As Byte()) Implements IInputStream.Read
            Me._stream.Read(data, 0, data.Length)
        End Sub


        Public Sub Read(ByVal data As Byte(), ByVal offset As Integer, ByVal count As Integer) Implements IInputStream.Read
            Me._stream.Read(data, offset, count)
        End Sub

        Public Function ReadByte() As Integer Implements IInputStream.ReadByte
            Return Me._stream.ReadByte()
        End Function


        Public Function ReadInt() As Integer Implements IInputStream.ReadInt
            Dim data As Byte() = New Byte(Marshal.SizeOf(GetType(Integer)) - 1) {}
            Read(data)
            Return ConvertUtils.ByteArrayToInt(data, 0, Me.ByteOrder)
        End Function

        Public Function ReadInt(ByVal length As Integer) As Integer Implements IInputStream.ReadInt
            Dim data As Byte() = New Byte(length - 1) {}
            Read(data)
            Return ConvertUtils.ByteArrayToNumber(data, 0, length, Me.ByteOrder)
        End Function

        Public Function ReadLine() As String Implements IInputStream.ReadLine
            Dim buffer As New StringBuilder
            While (True)
                Dim c As Integer = Me._stream.ReadByte()
                If (c = -1) Then
                    If (buffer.Length = 0) Then
                        Return vbNullString
                    Else
                        Exit While
                    End If
                ElseIf (c = Asc(ControlChars.Cr) OrElse c = Asc(ControlChars.Lf)) Then ' '\r' || c == '\n')
                    Exit While
                End If

                buffer.Append(Chr(c))
            End While
            Return buffer.ToString()
        End Function

        Public Function ReadShort() As Short Implements IInputStream.ReadShort
            Dim data As Byte() = New Byte(Marshal.SizeOf(GetType(Short)) - 1) {}
            Me.Read(data)
            Return CShort(ConvertUtils.ByteArrayToNumber(data, 0, data.Length, Me.ByteOrder))
        End Function

        Public Function ReadString(ByVal length As Integer) As String Implements IInputStream.ReadString
            Dim buffer As New StringBuilder()
            Dim c As Integer
            While (length > 0)
                length -= 1
                c = Me._stream.ReadByte()
                If (c = -1) Then Exit While
                buffer.Append(Chr(c))
            End While
            Return buffer.ToString()
        End Function

        Public Function ReadSignedByte() As SByte Implements IInputStream.ReadSignedByte
            Throw New NotImplementedException()
        End Function


        Public Function ReadUnsignedShort() As UShort Implements IInputStream.ReadUnsignedShort
            Dim data As Byte() = New Byte(Marshal.SizeOf(GetType(UShort)) - 1) {}
            Read(data)
            Return CUShort(ConvertUtils.ByteArrayToNumber(data, 0, data.Length, Me.ByteOrder))
        End Function

        Public Sub Seek(ByVal offset As Long) Implements IInputStream.Seek
            Me._stream.Seek(offset, SeekOrigin.Begin)
        End Sub


        Public Sub Skip(ByVal offset As Long) Implements IInputStream.Skip
            Me._stream.Seek(offset, SeekOrigin.Current)
        End Sub


#Region "IDataWrapper"

        Public Function ToByteArray() As Byte() Implements IDataWrapper.ToByteArray
            Dim data As Byte() = New Byte(CInt(Me._stream.Length - 1)) {}
            Me._stream.Position = 0
            Me._stream.Read(data, 0, data.Length)

            Return data
        End Function

#End Region

#Region "IStream"

        Public ReadOnly Property Length As Long Implements IStream.Length
            Get
                Return Me._stream.Length
            End Get
        End Property

#Region "IDisposable"

        Public Sub Dispose() Implements IDisposable.Dispose
            If (Me._stream IsNot Nothing) Then
                Me._stream.Dispose()
                Me._stream = Nothing
            End If
            'GC.SuppressFinalize(Me)
        End Sub

#End Region
#End Region
#End Region

#Region "IOutputStream"

        Public Sub Write(ByVal data As Byte()) Implements IOutputStream.Write
            Me._stream.Write(data, 0, data.Length)
#If DEBUG Then
            Dim text As String = System.Text.Encoding.Default.GetString(data)
            'Me._buffer = Me._buffer & text
            'If (InStr(Me._buffer, "stream") > 0) Then
            '    Debug.Print("ciao")
            'End If
#End If
        End Sub


        Public Sub Write(ByVal data As Byte(), ByVal offset As Integer, ByVal length As Integer) Implements IOutputStream.Write
            Me._stream.Write(data, offset, length)
        End Sub

        Public Sub Write(ByVal data As String) Implements IOutputStream.Write
            Me.Write(tokens.Encoding.Pdf.Encode(data))
        End Sub


        Public Sub Write(ByVal data As IInputStream) Implements IOutputStream.Write
            ' TODO:IMPL bufferize!!!
            Dim baseData As Byte() = New Byte(CInt(data.Length) - 1) {}
            ' Force the source pointer to the BOF (as we must copy the entire content)!
            data.Position = 0
            ' Read source content!
            data.Read(baseData, 0, baseData.Length)
            ' Write target content!
            Write(baseData)
        End Sub

#End Region
#End Region
#End Region
#End Region
    End Class

End Namespace

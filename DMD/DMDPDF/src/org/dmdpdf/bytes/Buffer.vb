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
Imports DMD.org.dmdpdf.tokens
Imports DMD.org.dmdpdf.util
Imports DMD.org.dmdpdf.util.io

Imports System
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Text

Namespace DMD.org.dmdpdf.bytes

    'TODO:IMPL Substitute System.Array static class invocations with System.Buffer static class invocations (better performance)!!!
    '/**
    '  <summary>Byte buffer.</summary>
    '*/
    Public NotInheritable Class Buffer
        Implements IBuffer

#Region "static"
#Region "fields"
        '/**
        '  <summary> Default buffer capacity.</summary>
        '*/
        Private Const DefaultCapacity As Integer = 1 << 8
#End Region
#End Region

#Region "dynamic"
#Region "events"
        Public Event OnChange(ByVal sender As Object, ByVal e As System.EventArgs) Implements IBuffer.OnChange 'EventHandler();
#End Region

#Region "fields"
        '/**
        '  <summary> Inner buffer where _data are stored.</summary>
        '*/
        Private _data As Byte()
        '/**
        '  <summary> Number Of bytes actually used In the buffer.</summary>
        '*/
        Private _length As Integer
        '/**
        '  <summary> Pointer _position within the buffer.</summary>
        '*/
        Private _position As Integer = 0

        Private _byteOrder As ByteOrderEnum = ByteOrderEnum.BigEndian

        Private _dirty As Boolean

#End Region

#Region "constructors"

        Public Sub New()
            'Debug.Print("Buffer()")
            Me._data = New Byte(DefaultCapacity - 1) {}
            Me._length = 0
        End Sub

        Public Sub New(ByVal capacity As Integer)
            'Debug.Print("Buffer(" & capacity & ")")
            If (capacity < 1) Then capacity = DefaultCapacity
            Me._data = New Byte(capacity - 1) {}
            Me._length = 0

        End Sub

        Public Sub New(ByVal data As Byte())
            'Debug.Print("Buffer(byte[" & data.Length & "])")
            Me._data = data
            Me._length = data.Length

        End Sub

        Public Sub New(ByVal data As System.IO.Stream)
            Me.New(CInt(data.Length))
            Me.Append(data)
            'Debug.Print("Buffer(System.io.Stream[" & data.Length & "])")

        End Sub

#End Region

#Region "Interface"
#Region "Public"
#Region "IBuffer"

        Public Function Append(ByVal data As Byte) As IBuffer Implements IBuffer.Append
            'Debug.Print("Buffer.Append(byte: " & data & ")")
            EnsureCapacity(1)
            Me._data(Me._length) = data : Me._length += 1
            NotifyChange()

            Return Me
        End Function

        Public Function Append(ByVal data As Byte()) As IBuffer Implements IBuffer.Append
            'Debug.Print("Buffer.Append(byte[" & data.Length & "])")
            Me.Append(data, 0, data.Length)

            Return Me
        End Function

        Public Function Append(ByVal data As Byte(), ByVal offset As Integer, ByVal length As Integer) As IBuffer Implements IBuffer.Append
            'Debug.Print("Buffer.Append(byte[" & data.Length & "], " & offset & ", " & length & ")")
            EnsureCapacity(length)
            Array.Copy(data, offset, Me._data, Me._length, length)
            Me._length += length
            NotifyChange()

            Return Me
        End Function

        Public Function Append(ByVal data As String) As IBuffer Implements IBuffer.Append
            'Debug.Print("Buffer.Append(String : " & data & ")")
            Return Me.Append(tokens.Encoding.Pdf.Encode(data))
        End Function

        Public Function Append(ByVal data As IInputStream) As IBuffer Implements IBuffer.Append
            'Debug.Print("Buffer.Append(IInputStream[" & data.Length & "])")
            Me.Append(data.ToByteArray(), 0, CInt(data.Length))

            Return Me
        End Function

        Public Function Append(ByVal data As System.IO.Stream) As IBuffer Implements IBuffer.Append
            'Debug.Print("Buffer.Append(System.IO.Stream[" & data.Length & "])")
            Dim array As Byte() = New Byte(CInt(data.Length) - 1) {}
            data.Position = 0
            data.Read(array, 0, array.Length)
            Me.Append(array)

            Return Me
        End Function

        Public ReadOnly Property Capacity As Integer Implements IBuffer.Capacity
            Get
                'Debug.Print("Buffer.getCapacity() -> " & Me._data.Length)
                Dim ret As Integer = Me._data.Length

                Return ret
            End Get
        End Property

        Public Function Clone() As IBuffer Implements IBuffer.Clone
            'Debug.Print("Buffer.Clone()")
            Dim _clone As IBuffer = New Buffer(Me.Capacity)
            _clone.Append(Me._data)

            Return _clone
        End Function

        Public Sub Decode(ByVal filter As Filter, ByVal parameters As PdfDictionary) Implements IBuffer.Decode
            'Debug.Print("Buffer.Decode(filter, parameters)")

            Me._data = filter.Decode(Me._data, 0, Me._length, parameters)
            Me._length = Me._data.Length

        End Sub

        Public Sub Delete(ByVal index As Integer, ByVal length As Integer) Implements IBuffer.Delete
            'Debug.Print("Buffer.Delete(" & index & ", " & length & ")")
            ' Shift left the trailing _data block to override the deleted _data!
            Array.Copy(Me._data, index + length, Me._data, index, Me._length - (index + length))
            Me._length -= length
            NotifyChange()

        End Sub

        Public Property Dirty As Boolean Implements IBuffer.Dirty
            Get
                'Debug.Print("Buffer.getDirty() -> " & Me._dirty)
                Return Me._dirty
            End Get
            Set(ByVal value As Boolean)
                'Debug.Print("Buffer.setDirty(" & value & ")")
                Me._dirty = value

            End Set
        End Property

        Public Function Encode(ByVal filter As Filter, ByVal parameters As PdfDictionary) As Byte() Implements IBuffer.Encode
            'Debug.Print("Buffer.Encode(filter, parameters)")
            Dim ret As Byte() = filter.Encode(Me._data, 0, Me._length, parameters)

            Return ret
        End Function


        Public Function GetByte(ByVal index As Integer) As Integer Implements IBuffer.GetByte
            'Debug.Print("Buffer.GetByte(" & index & ") -> " & Me._data(index))
            Return Me._data(index)
        End Function

        Public Function GetByteArray(ByVal index As Integer, ByVal length As Integer) As Byte() Implements IBuffer.GetByteArray
            'Debug.Print("Buffer.GetByteArray(" & index & ", " & length & ")")
            Dim data As Byte() = New Byte(length - 1) {}
            Array.Copy(Me._data, index, data, 0, length)
            Return data
        End Function

        Public Function GetString(ByVal index As Integer, ByVal length As Integer) As String Implements IBuffer.GetString
            'Debug.Print("Buffer.GetString(" & index & ", " & length & ")")
            Return tokens.Encoding.Pdf.Decode(Me._data, index, length)
        End Function

        Public Sub Insert(ByVal index As Integer, ByVal data As Byte()) Implements IBuffer.Insert
            'Debug.Print("Buffer.Insert(" & index & ", byte[" & data.Length & "])")
            Insert(index, data, 0, data.Length)
        End Sub

        Public Sub Insert(ByVal index As Integer, ByVal data As Byte(), ByVal offset As Integer, ByVal length As Integer) Implements IBuffer.Insert
            'Debug.Print("Buffer.Insert(" & index & ", byte[" & data.Length & "], " & offset & ", " & length & ")")
            EnsureCapacity(length)
            ' Shift right the existing _data block to make room for New _data!
            Array.Copy(Me._data, index, Me._data, index + length, Me._length - index)
            ' Insert additional _data!
            Array.Copy(data, offset, Me._data, index, length)
            Me._length += length
            NotifyChange()
        End Sub

        Public Sub Insert(ByVal index As Integer, ByVal data As String) Implements IBuffer.Insert
            'Debug.Print("Buffer.Insert(" & index & ", String: " & data & ")")
            Insert(index, tokens.Encoding.Pdf.Encode(data))
        End Sub

        Public Sub Insert(ByVal index As Integer, ByVal data As IInputStream) Implements IBuffer.Insert
            'Debug.Print("Buffer.Insert(" & index & ", IInputStream[" & data.Length & "])")
            Insert(index, data.ToByteArray())
        End Sub

        Public Sub Replace(ByVal index As Integer, ByVal data As Byte()) Implements IBuffer.Replace
            'Debug.Print("Buffer.Replace(" & index & ", byte[" & data.Length & "])")
            Array.Copy(data, 0, Me._data, index, data.Length)
            Me.NotifyChange()
        End Sub

        Public Sub Replace(ByVal index As Integer, ByVal data As Byte(), ByVal offset As Integer, ByVal _length As Integer) Implements IBuffer.Replace
            'Debug.Print("Buffer.Replace(" & index & ", byte[" & data.Length & "], " & offset & ", " & _length & ")")
            Array.Copy(data, offset, Me._data, index, data.Length)
            Me.NotifyChange()
        End Sub

        Public Sub Replace(ByVal index As Integer, ByVal data As String) Implements IBuffer.Replace
            'Debug.Print("Buffer.Replace(" & index & ", String: " & data & ")")
            Replace(index, tokens.Encoding.Pdf.Encode(data))
        End Sub

        Public Sub Replace(ByVal index As Integer, ByVal data As IInputStream) Implements IBuffer.Replace
            'Debug.Print("Buffer.Replace(" & index & ", IInputStream[" & data.Length & "])")
            Replace(index, data.ToByteArray())
        End Sub

        Public Sub SetLength(ByVal value As Integer) Implements IBuffer.SetLength
            'Debug.Print("Buffer.SetLength(" & value & ")")
            Me._length = value
            Me.NotifyChange()
        End Sub

        Public Sub WriteTo(ByVal stream As IOutputStream) Implements IBuffer.WriteTo
            'Debug.Print("Buffer.WriteTo(IOutputStream)")
            stream.Write(_data, 0, _length)
        End Sub

        Public Overrides Function GetHashCode() As Integer Implements IBuffer.GetHashCode
            'Debug.Print("Buffer.GetHashCode() -> " & MyBase.GetHashCode)
            Return MyBase.GetHashCode()
        End Function

#Region "IInputStream"

        Public Property ByteOrder As ByteOrderEnum Implements IInputStream.ByteOrder
            Get
                'Debug.Print("Buffer.getByteOrder() -> " & Me._byteOrder)
                Return Me._byteOrder
            End Get
            Set(ByVal value As ByteOrderEnum)
                'Debug.Print("Buffer.setByteOrder(" & value & ")")
                Me._byteOrder = value
            End Set
        End Property


        '/* int GetHashCode() uses inherited implementation. */

        Public Property Position As Long Implements IInputStream.Position
            Get
                'Debug.Print("Buffer.getPosition() -> " & Me._position)
                Return Me._position
            End Get
            Set(ByVal value As Long)
                'Debug.Print("Buffer.setPosition(" & value & ")")
                If (value < 0) Then
                    value = 0
                ElseIf (value > Me._data.Length) Then
                    value = Me._data.Length
                End If
                Me._position = CInt(value)
            End Set
        End Property

        Public Sub Read(ByVal data As Byte()) Implements IInputStream.Read
            'Debug.Print("Buffer.Read(byte[" & data.Length & "])")
            Me.Read(data, 0, data.Length)
        End Sub

        Public Sub Read(ByVal data As Byte(), ByVal offset As Integer, ByVal length As Integer) Implements IInputStream.Read
            'Debug.Print("Buffer.Read(byte[" & data.Length & "], " & offset & ", " & length & ")")
            Array.Copy(Me._data, Me._position, data, offset, length)
            Me._position += length
        End Sub

        Public Function ReadByte() As Integer Implements IInputStream.ReadByte
            Dim value As Integer
            If (Me._position >= Me._data.Length) Then
                value = -1 'TODO:harmonize with other Read*() method EOF exceptions!!!
            Else
                value = Me._data(Me._position) : Me._position += 1
            End If
            'Debug.Print("Buffer.ReadByte() -> " & value)
            Return value
        End Function

        Public Function ReadInt() As Integer Implements IInputStream.ReadInt
            Dim value As Integer = ConvertUtils.ByteArrayToInt(Me._data, Me._position, Me._byteOrder)
            Me._position += Marshal.SizeOf(GetType(Integer))
            'Debug.Print("Buffer.ReadInt() -> " & value)
            Return value
        End Function

        Public Function ReadInt(ByVal length As Integer) As Integer Implements IInputStream.ReadInt
            Dim value As Integer = ConvertUtils.ByteArrayToNumber(Me._data, Me._position, length, Me._byteOrder)
            Me._position += length
            'Debug.Print("Buffer.ReadInt(" & length & ") -> " & value)
            Return value
        End Function

        Public Function ReadLine() As String Implements IInputStream.ReadLine
            If (Me._position >= Me._data.Length) Then Throw New EndOfStreamException()
            Dim buffer As New StringBuilder
            While (Me._position < Me._data.Length)
                Dim c As Integer = Me._data(Me._position) : Me._position += 1
                If (c = Asc(ControlChars.Cr) OrElse c = Asc(ControlChars.Lf)) Then Exit While 'break;
                buffer.Append(Chr(c))
            End While
            'Debug.Print("Buffer.ReadLine() -> " & buffer.ToString)
            Return buffer.ToString()
        End Function

        Public Function ReadShort() As Short Implements IInputStream.ReadShort
            Dim value As Short = CShort(ConvertUtils.ByteArrayToNumber(Me._data, Me._position, Marshal.SizeOf(GetType(Short)), Me._byteOrder))
            Me._position += Marshal.SizeOf(GetType(Short))
            'Debug.Print("Buffer.ReadShort() -> " & value)
            Return value
        End Function

        Public Function ReadString(ByVal length As Integer) As String Implements IInputStream.ReadString
            Dim data As String = tokens.Encoding.Pdf.Decode(Me._data, Me._position, length)
            _position += length
            'Debug.Print("Buffer.ReadString(" & length & ") -> " & data)
            Return data
        End Function

        Public Function ReadSignedByte() As SByte Implements IInputStream.ReadSignedByte
            If (Me._position >= Me._data.Length) Then Throw New EndOfStreamException()
            Dim value As SByte = CSByte(Me._data(Me._position)) : Me._position += 1
            'Debug.Print("Buffer.ReadSignedByte() -> " & value)
            Return value
        End Function

        Public Function ReadUnsignedShort() As UShort Implements IInputStream.ReadUnsignedShort
            Dim value As UShort = CUShort(ConvertUtils.ByteArrayToNumber(_data, _position, Marshal.SizeOf(GetType(UShort)), _byteOrder))
            _position += Marshal.SizeOf(GetType(UShort))
            'Debug.Print("Buffer.ReadUnsignedShort() -> " & value)
            Return value
        End Function

        Public Sub Seek(ByVal offset As Long) Implements IInputStream.Seek
            'Debug.Print("Buffer.Seek(" & offset & ")")
            Me.Position = offset
        End Sub

        Public Sub Skip(ByVal offset As Long) Implements IInputStream.Skip
            'Debug.Print("Buffer.Skip(" & offset & ")")
            Me.Position = Me._position + offset
        End Sub

#Region "IDataWrapper"

        Public Function ToByteArray() As Byte() Implements IDataWrapper.ToByteArray
            'Debug.Print("Buffer.ToByteArray() -> byte[" & Me._length & "]")
            Dim ret As Byte() = New Byte(Me._length - 1) {}
            Array.Copy(Me._data, 0, ret, 0, Me._length)
            Return ret
        End Function
#End Region

#Region "IStream"

        Public ReadOnly Property Length As Long Implements IStream.Length
            Get
                'Debug.Print("Buffer.getLength() -> " & Me._length)
                Return Me._length
            End Get
        End Property


#Region "IDisposable"
        Public Sub Dispose() Implements IDisposable.Dispose
            'DMD 2018/06/11
            'GC.SuppressFinalize(Me)
            Me._data = Nothing
        End Sub

#End Region
#End Region
#End Region
#End Region

#Region "IOutputStream"

        Public Sub Write(ByVal data As Byte()) Implements IOutputStream.Write
            'Debug.Print("Buffer.Write(byte[" & data.Length & "])")
            Me.Append(data)
        End Sub

        Public Sub Write(ByVal data As Byte(), ByVal offset As Integer, ByVal length As Integer) Implements IOutputStream.Write
            'Debug.Print("Buffer.Write(byte[" & data.Length & "], " & offset & ", " & length & ")")
            Append(data, offset, length)
        End Sub

        Public Sub Write(ByVal data As String) Implements IOutputStream.Write
            'Debug.Print("Buffer.Write(String: " & data & ")")
            Append(data)
        End Sub

        Public Sub Write(ByVal data As IInputStream) Implements IOutputStream.Write
            'Debug.Print("Buffer.Write(IInputStream[" & data.Length & "])")
            Append(data)
        End Sub

#End Region
#End Region

#Region "private"
        '/**
        '  <summary> Check whether the buffer has sufficient room For
        '  adding _data.</summary>
        '*/
        Private Sub EnsureCapacity(ByVal additionalLength As Integer)
            'Debug.Print("Buffer.EnsureCapacity(" & additionalLength & ")")
            Dim minCapacity As Integer = Me._length + additionalLength
            ' Is additional _data within the buffer capacity?
            If (minCapacity <= Me._data.Length) Then Return

            ' Additional _data exceed buffer capacity.
            ' Reallocate the buffer!
            Dim dataLen As Integer = Me._data.Length << 1 ' 1 order of magnitude greater than current capacity.
            If (dataLen < minCapacity) Then dataLen = minCapacity 'Minimum capacity required.
            Dim _data As Byte() = New Byte(dataLen - 1) {}
            Array.Copy(Me._data, 0, _data, 0, Me._length)
            Me._data = _data


        End Sub

        Private Sub NotifyChange()

            ' Debug.Print("Buffer.NotifyChange()")
            If (_dirty) Then Return ' OrElse OnChange == null)
            _dirty = True
            RaiseEvent OnChange(Me, Nothing)
        End Sub


#End Region
#End Region
#End Region


    End Class

End Namespace

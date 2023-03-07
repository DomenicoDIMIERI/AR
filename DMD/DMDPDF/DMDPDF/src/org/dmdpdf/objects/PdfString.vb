'/*
'  Copyright 2008-2012 Stefano Chizzolini. http://www.dmdpdf.org

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
Imports DMD.org.dmdpdf.tokens 'DMD.tokens = 
Imports DMD.org.dmdpdf.util

Imports System
Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions

Namespace DMD.org.dmdpdf.objects

    '/**
    '  <summary>PDF string object [PDF:1.6:3.2.3].</summary>
    '  <remarks>
    '    <para>A string object consists of a series of bytes.</para>
    '    <para>String objects can be serialized in two ways:</para>
    '    <list type="bullet">
    '      <item>as a sequence of literal characters (plain form)</item>
    '      <item>as a sequence of hexadecimal digits (hexadecimal form)</item>
    '    </list>
    '  </remarks>
    '*/
    Public Class PdfString
        Inherits PdfSimpleObject(Of Byte())
        Implements IDataWrapper

        '    /*
        '  NOTE: String objects are internally represented as unescaped sequences of bytes.
        '  Escaping is applied on serialization only.
        '*/
#Region "types"

        '/**
        '  <summary>String serialization mode.</summary>
        '*/
        Public Enum SerializationModeEnum

            ''' <summary>
            ''' Plain form. 
            ''' </summary>
            Literal

            ''' <summary>
            ''' Hexadecimal form.
            ''' </summary>
            Hex
        End Enum

#End Region

#Region "Static"
#Region "fields"

        Public Shared ReadOnly [Default] As PdfString = New PdfString("")

        Private Const BackspaceCode As Byte = 8
        Private Const CarriageReturnCode As Byte = 13
        Private Const FormFeedCode As Byte = 12
        Private Const HorizontalTabCode As Byte = 9
        Private Const LineFeedCode As Byte = 10

        Private Const HexLeftDelimiterCode As Byte = 60
        Private Const HexRightDelimiterCode As Byte = 62
        Private Const LiteralEscapeCode As Byte = 92
        Private Const LiteralLeftDelimiterCode As Byte = 40
        Private Const LiteralRightDelimiterCode As Byte = 41

#End Region
#End Region

#Region "dynamic"
#Region "fields"

        Private _serializationMode As SerializationModeEnum = SerializationModeEnum.Literal

#End Region

#Region "constructors"

        Public Sub New(ByVal rawValue As Byte())
            Me.SetRawValue(rawValue)
        End Sub


        Public Sub New(ByVal value As String)
            Me.Value = value
        End Sub


        Public Sub New(ByVal rawValue As Byte(), ByVal serializationMode As SerializationModeEnum)
            Me.serializationMode = serializationMode
            Me.SetRawValue(rawValue)
        End Sub

        Public Sub New(ByVal value As String, ByVal serializationMode As SerializationModeEnum)
            Me.serializationMode = serializationMode
            Me.Value = value
        End Sub

        Protected Sub New()
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public Overrides Function Accept(ByVal visitor As IVisitor, ByVal data As Object) As PdfObject
            Return visitor.Visit(Me, data)
        End Function

        Public Overrides Function CompareTo(ByVal obj As PdfDirectObject) As Integer
            If (Not (TypeOf (obj) Is PdfString)) Then Throw New ArgumentException("Object MUST be a PdfString")
            Return String.CompareOrdinal(Me.StringValue, CType(obj, PdfString).StringValue)
        End Function

        '/**
        '  <summary>Gets/Sets the serialization mode.</summary>
        '*/
        Public Overridable Property SerializationMode As SerializationModeEnum
            Get
                Return Me._serializationMode
            End Get
            Set(ByVal value As SerializationModeEnum)
                Me._serializationMode = value
            End Set
        End Property


        Public ReadOnly Property StringValue As String
            Get
                Select Case (Me._serializationMode)
                    Case SerializationModeEnum.Literal
                        Return tokens.Encoding.Pdf.Decode(Me.RawValue)
                    Case SerializationModeEnum.Hex
                        Return ConvertUtils.ByteArrayToHex(Me.RawValue)
                    Case Else
                        Throw New NotImplementedException(Me._serializationMode & " serialization mode is not implemented.")
                End Select
            End Get
        End Property

        Public Function ToByteArray() As Byte() Implements IDataWrapper.ToByteArray
            Return CType(Me.RawValue.Clone(), Byte())
        End Function

        Public Overrides Function ToString() As String
            Select Case (Me._serializationMode)
                Case SerializationModeEnum.Hex
                    Return "<" & Me.StringValue & ">"
                Case SerializationModeEnum.Literal
                    Return "(" & Me.StringValue & ")"
                Case Else
                    Throw New NotImplementedException()
            End Select
        End Function

        Public Overrides Property Value As Object
            Get
                Return Me.StringValue
            End Get
            Protected Set(value As Object)
                Select Case (Me._serializationMode)
                    Case SerializationModeEnum.Literal
                        Me.SetRawValue(tokens.Encoding.Pdf.Encode(CStr(value)))
                    Case SerializationModeEnum.Hex
                        Me.SetRawValue(ConvertUtils.HexToByteArray(CStr(value)))
                    Case Else
                        Throw New NotImplementedException(Me._serializationMode & " serialization mode is not implemented.")
                End Select
            End Set
        End Property


        Public Overrides Sub WriteTo(ByVal stream As IOutputStream, ByVal context As files.File)
            Dim buffer As New MemoryStream()
            Dim rawValue As Byte() = Me.RawValue
            Select Case (Me._serializationMode)
                Case SerializationModeEnum.Literal
                    buffer.WriteByte(LiteralLeftDelimiterCode)
                    '/*
                    '  NOTE: Literal lexical conventions prescribe that the following reserved characters
                    '  are to be escaped when placed inside string character sequences:
                    '    - \n Line feed (LF)
                    '    - \r Carriage return (CR)
                    '    - \t Horizontal tab (HT)
                    '    - \b Backspace (BS)
                    '    - \f Form feed (FF)
                    '    - \( Left parenthesis
                    '    - \) Right parenthesis
                    '    - \\ Backslash
                    '*/
                    For index As Integer = 0 To rawValue.Length - 1
                        Dim valueByte As Byte = rawValue(index)
                        Select Case (valueByte)
                            Case LineFeedCode : buffer.WriteByte(LiteralEscapeCode) : valueByte = 110
                            Case CarriageReturnCode : buffer.WriteByte(LiteralEscapeCode) : valueByte = 114
                            Case HorizontalTabCode : buffer.WriteByte(LiteralEscapeCode) : valueByte = 116
                            Case BackspaceCode : buffer.WriteByte(LiteralEscapeCode) : valueByte = 98
                            Case FormFeedCode : buffer.WriteByte(LiteralEscapeCode) : valueByte = 102
                            Case LiteralLeftDelimiterCode,
                                 LiteralRightDelimiterCode,
                                LiteralEscapeCode
                                buffer.WriteByte(LiteralEscapeCode)
                        End Select
                        buffer.WriteByte(valueByte)
                    Next
                    buffer.WriteByte(LiteralRightDelimiterCode)
                    'break;
                Case SerializationModeEnum.Hex
                    buffer.WriteByte(HexLeftDelimiterCode)
                    Dim value As Byte() = tokens.Encoding.Pdf.Encode(ConvertUtils.ByteArrayToHex(rawValue))
                    buffer.Write(value, 0, value.Length)
                    buffer.WriteByte(HexRightDelimiterCode)
                Case Else
                    Throw New NotImplementedException()
            End Select
            stream.Write(buffer.ToArray())
        End Sub

#End Region
#End Region
#End Region

    End Class

End Namespace


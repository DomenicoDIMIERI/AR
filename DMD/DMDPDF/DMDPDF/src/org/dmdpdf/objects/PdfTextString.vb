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

Imports DMD.org.dmdpdf.tokens

Imports System
Imports System.Text
Imports System.Text.RegularExpressions

Namespace DMD.org.dmdpdf.objects

    '/**
    '  <summary>PDF text string object [PDF:1.6:3.8.1].</summary>
    '  <remarks>Text strings are meaningful only as part of the document hierarchy; they cannot appear
    '  within content streams. They represent information that is intended to be human-readable.</remarks>
    '*/
    Public NotInheritable Class PdfTextString
        Inherits PdfString

        '    /*
        '  NOTE: Text strings are string objects encoded in either PdfDocEncoding (superset of the ISO
        '  Latin 1 encoding [PDF:1.6:D]) or 16-bit big-endian Unicode character encoding (see [UCS:4]).
        '*/
#Region "Static"
#Region "fields"

        Public Shared Shadows ReadOnly [Default] As New PdfTextString("")

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary>Gets the object equivalent to the given value.</summary>
        '*/
        Public Shared Shadows Function [Get](ByVal value As String) As PdfTextString
            If (value IsNot Nothing) Then
                Return New PdfTextString(value)
            Else
                Return Nothing
            End If
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "fields"

        Private _unicoded As Boolean

#End Region

#Region "constructors"

        Public Sub New(ByVal rawValue As Byte())
            MyBase.new(rawValue)
        End Sub

        Public Sub New(ByVal value As String)
            MyBase.New(value)
        End Sub

        Public Sub New(ByVal rawValue As Byte(), ByVal serializationMode As SerializationModeEnum)
            MyBase.new(rawValue, serializationMode)
        End Sub

        Public Sub New(ByVal value As String, ByVal serializationMode As SerializationModeEnum)
            MyBase.new(value, serializationMode)
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public Overrides Function Accept(ByVal visitor As IVisitor, ByVal data As Object) As PdfObject
            Return visitor.Visit(Me, data)
        End Function

        Protected Overrides Sub SetRawValue(ByVal value As Byte())
            Me._unicoded = (value.Length >= 2 AndAlso value(0) = CByte(254) AndAlso value(1) = CByte(255))
            MyBase.SetRawValue(value)
        End Sub

        Public Overrides Property Value As Object
            Get
                If (SerializationMode = SerializationModeEnum.Literal AndAlso Me._unicoded) Then
                    Dim valueBytes As Byte() = Me.RawValue
                    Return Charset.UTF16BE.GetString(valueBytes, 2, valueBytes.Length - 2)
                Else
                    ' FIXME: proper call to base.StringValue could NOT be done due to an unexpected Mono runtime SIGSEGV (TOO BAD).
                    'Return MyBase.StringValue
                    Return CStr(MyBase.Value)
                End If
            End Get
            Protected Set(value As Object)
                Select Case (Me.SerializationMode)
                    Case SerializationModeEnum.Literal
                        Dim literalValue As String = CStr(value)
                        Dim valueBytes As Byte() = PdfDocEncoding.Get().Encode(literalValue)
                        If (valueBytes Is Nothing) Then
                            Dim valueBaseBytes As Byte() = Charset.UTF16BE.GetBytes(literalValue)
                            ' Prepending UTF marker...
                            valueBytes = New Byte(valueBaseBytes.Length + 2 - 1) {}
                            valueBytes(0) = CByte(254) : valueBytes(1) = CByte(255)
                            Array.Copy(valueBaseBytes, 0, valueBytes, 2, valueBaseBytes.Length)
                        End If
                        Me.SetRawValue(valueBytes)
                    'break;
                    Case SerializationModeEnum.Hex
                        MyBase.Value = value
                        'break;
                End Select
            End Set
        End Property



#End Region
#End Region
#End Region

    End Class

End Namespace

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

Imports DMD.org.dmdpdf.tokens
Imports DMD.org.dmdpdf.util.parsers

Imports System
Imports System.Globalization
Imports System.Text

Namespace DMD.org.dmdpdf.objects

    '/**
    '  <summary>PDF date object [PDF:1.6:3.8.3].</summary>
    '*/
    Public NotInheritable Class PdfDate
        Inherits PdfString

#Region "Static"
#Region "fields"

        Private Const FormatString As String = "yyyyMMddHHmmsszzz"

#End Region

#Region "interface"
#Region "public"

        '    /**
        '  <summary>Gets the object equivalent to the given value.</summary>
        '*/
        Public Shared Shadows Function [Get](ByVal value As DateTime?) As PdfDate
            If (value.HasValue) Then
                Return New PdfDate(value.Value)
            Else
                Return Nothing
            End If
        End Function

        '/**
        '  <summary>Converts a PDF date literal into its corresponding date.</summary>
        '  <exception cref="org.dmdpdf.util.parsers.ParseException">Thrown when date literal parsing fails.</exception>
        '*/
        Public Shared Function ToDate(ByVal value As String) As DateTime
            ' 1. Normalization.
            Dim dateBuilder As StringBuilder = New StringBuilder()
            Try
                Dim length As Integer = value.Length
                ' Year (YYYY).
                dateBuilder.Append(value.Substring(2, 4)) ' NOTE: Skips the "D:" prefix; Year is mandatory.
                ' Month (MM).
                If (length < 8) Then
                    dateBuilder.Append("01")
                Else
                    dateBuilder.Append(value.Substring(6, 2))
                End If
                ' Day (DD).
                If (length < 10) Then
                    dateBuilder.Append("01")
                Else
                    dateBuilder.Append(value.Substring(8, 2))
                End If

                ' Hour (HH).
                If (length < 12) Then
                    dateBuilder.Append("00")
                Else
                    dateBuilder.Append(value.Substring(10, 2))
                End If

                ' Minute (mm).
                If (length < 14) Then
                    dateBuilder.Append("00")
                Else
                    dateBuilder.Append(value.Substring(12, 2))
                End If

                ' Second (SS).
                If (length < 16) Then
                    dateBuilder.Append("00")
                Else
                    dateBuilder.Append(value.Substring(14, 2))
                End If

                ' Local time / Universal Time relationship (O).
                If (length < 17 OrElse value.Substring(16, 1).Equals("Z")) Then
                    dateBuilder.Append("+")
                Else
                    dateBuilder.Append(value.Substring(16, 1))
                End If

                ' UT Hour offset (HH').
                If (length < 19) Then
                    dateBuilder.Append("00")
                Else
                    dateBuilder.Append(value.Substring(17, 2))
                End If

                ' UT Minute offset (mm').
                If (length < 22) Then
                    dateBuilder.Append(":").Append("00")
                Else
                    dateBuilder.Append(":").Append(value.Substring(20, 2))
                End If

            Catch exception As System.Exception
                Throw New ParseException("Failed to normalize the date string.", exception)
            End Try

            ' 2. Parsing.
            Try
                Return DateTime.ParseExact(dateBuilder.ToString(), FormatString, New CultureInfo("en-US"))

            Catch exception As System.Exception
                Throw New ParseException("Failed to parse the date string.", exception)
            End Try
        End Function
#End Region

#Region "Private"

        Private Shared Function Format(ByVal value As DateTime) As String
            Return ("D:" & value.ToString(FormatString).Replace(":"c, "'"c) & "'")
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "constructors"


        Public Sub New(ByVal value As DateTime)
            Me.Value = value
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public Overrides Function Accept(ByVal visitor As IVisitor, ByVal data As Object) As PdfObject
            Return visitor.Visit(Me, data)
        End Function

        Public Overrides Property SerializationMode As SerializationModeEnum
            Get
                Return MyBase.SerializationMode
            End Get
            Set(ByVal value As SerializationModeEnum)
                ' NOOP: Serialization MUST be kept literal. 
            End Set
        End Property

        ''' <summary>
        ''' // FIXME: proper call to base.StringValue could NOT be done due to an unexpected Mono runtime SIGSEGV (TOO BAD).
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Property Value As Object
            Get
                Return ToDate(MyBase.StringValue)
                '{return ToDate((string)base.Value);}
            End Get
            Protected Set(ByVal value As Object)
                Me.SetRawValue(tokens.Encoding.Pdf.Encode(Format(CType(value, DateTime))))
            End Set

        End Property

#End Region
#End Region
#End Region

    End Class


End Namespace
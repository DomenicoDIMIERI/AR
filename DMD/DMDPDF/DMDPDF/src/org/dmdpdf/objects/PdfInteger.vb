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

Imports DMD.org.dmdpdf.bytes
Imports DMD.org.dmdpdf.files

Namespace DMD.org.dmdpdf.objects

    '/**
    '  <summary>PDF integer number object [PDF:1.6:3.2.2].</summary>
    '*/
    Public NotInheritable Class PdfInteger
        Inherits PdfSimpleObject(Of Integer)
        Implements IPdfNumber

#Region "Static"
#Region "fields"

        Public Shared ReadOnly [Default] As PdfInteger = New PdfInteger(0)

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary>Gets the object equivalent to the given value.</summary>
        '*/
        Public Shared Shadows Function [Get](ByVal value As Integer?) As PdfInteger
            If (value.HasValue) Then
                Return New PdfInteger(value.Value)
            Else
                Return Nothing
            End If
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        Public Sub New(ByVal value As Integer)
            Me.SetRawValue(value)
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public Overrides Function Accept(ByVal visitor As IVisitor, ByVal data As Object) As PdfObject
            Return visitor.Visit(Me, data)
        End Function

        Public Overrides Function CompareTo(ByVal obj As PdfDirectObject) As Integer
            Return PdfNumber.Compare(Me, obj)
        End Function

        Public Overrides Function Equals(ByVal obj As Object) As Boolean
            Return PdfNumber.Equal(Me, obj)
        End Function

        Public Overrides Function GetHashCode() As Integer
            Return PdfNumber.GetHashCode(Me)
        End Function

        Public Overrides Sub WriteTo(ByVal stream As IOutputStream, ByVal context As File)
            stream.Write(RawValue.ToString())
        End Sub

#Region "IPdfNumber"

        Public ReadOnly Property DoubleValue As Double Implements IPdfNumber.DoubleValue
            Get
                Return Me.RawValue
            End Get
        End Property

        Public ReadOnly Property FloatValue As Single Implements IPdfNumber.FloatValue
            Get
                Return Me.RawValue
            End Get
        End Property

        Public ReadOnly Property IntValue As Integer Implements IPdfNumber.IntValue
            Get
                Return Me.RawValue
            End Get
        End Property

        Private ReadOnly Property _RawValue As Double Implements IPdfSimpleObject(Of Double).RawValue
            Get
                Return Me.RawValue
            End Get
        End Property

#End Region
#End Region
#End Region
#End Region

    End Class

End Namespace
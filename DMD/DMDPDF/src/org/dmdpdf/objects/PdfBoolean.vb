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
Imports DMD.org.dmdpdf.tokens

Imports System

Namespace DMD.org.dmdpdf.objects

    '/**
    '  <summary>PDF boolean object [PDF:1.6:3.2.1].</summary>
    '*/
    Public NotInheritable Class PdfBoolean
        Inherits PdfSimpleObject(Of Boolean)

#Region "Static"
#Region "fields"

        Public Shared ReadOnly [False] As PdfBoolean = New PdfBoolean(False)
        Public Shared ReadOnly [True] As PdfBoolean = New PdfBoolean(True)

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary>Gets the object equivalent to the given value.</summary>
        '*/
        Public Shared Shadows Function [Get](ByVal value As Boolean?) As PdfBoolean
            If (value.HasValue) Then
                If (value.Value) Then
                    Return [True]
                Else
                    Return [False]
                End If
            Else
                Return Nothing
            End If
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        Private Sub New(ByVal value As Boolean)
            Me.SetRawValue(value)
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public Overrides Function Accept(ByVal visitor As IVisitor, ByVal data As Object) As PdfObject
            Return visitor.Visit(Me, data)
        End Function

        Public ReadOnly Property BooleanValue As Boolean
            Get
                Return CBool(Me.Value)
            End Get
        End Property

        Public Overrides Function CompareTo(ByVal obj As PdfDirectObject) As Integer
            Throw New NotImplementedException()
        End Function

        Public Overrides Sub WriteTo(ByVal stream As IOutputStream, ByVal context As File)
            If (Me.RawValue) Then
                stream.Write(Keyword.True)
            Else
                stream.Write(Keyword.False)
            End If
        End Sub

#End Region
#End Region
#End Region

    End Class

End Namespace
'/*
'  Copyright 2012 Stefano Chizzolini. http://www.dmdpdf.org

'  Contributors:
'    * Stefano Chizzolini (original code developer, http://www.stefanochizzolini.it)

'  This file should be part of the source code distribution of "PDF Clown library"
'  (the Program): see the accompanying README files for more info.

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
Imports System

Namespace DMD.org.dmdpdf.tokens

    '/**
    '  <summary>Encoding for text strings in a PDF document outside the document's content streams
    '  [PDF:1.7:D].</summary>
    '*/
    Public NotInheritable Class PdfDocEncoding
        Inherits LatinEncoding

#Region "types"

        Private Class Chars
            Inherits BiDictionary(Of Integer, Char)

            Friend Sub New()
                Me(&H80) = ChrW(&H2022)
                Me(&H81) = ChrW(&H2020)
                Me(&H82) = ChrW(&H2021)
                Me(&H84) = ChrW(&H2014)
                Me(&H85) = ChrW(&H2013)
                Me(&H86) = ChrW(&H192)
                Me(&H87) = ChrW(&H2044)
                Me(&H88) = ChrW(&H2039)
                Me(&H89) = ChrW(&H203A)
                Me(&H8A) = ChrW(&H2212)
                Me(&H8B) = ChrW(&H2030)
                Me(&H8C) = ChrW(&H201E)
                Me(&H8D) = ChrW(&H201C)
                Me(&H8E) = ChrW(&H201D)
                Me(&H8F) = ChrW(&H2018)
                Me(&H90) = ChrW(&H2019)
                Me(&H91) = ChrW(&H201A)
                Me(&H92) = ChrW(&H2122)
                Me(&H93) = ChrW(&HFB01)
                Me(&H94) = ChrW(&HFB02)
                Me(&H95) = ChrW(&H141)
                Me(&H96) = ChrW(&H152)
                Me(&H97) = ChrW(&H160)
                Me(&H98) = ChrW(&H178)
                Me(&H99) = ChrW(&H17D)
                Me(&H9A) = ChrW(&H131)
                Me(&H9B) = ChrW(&H142)
                Me(&H9C) = ChrW(&H153)
                Me(&H9D) = ChrW(&H161)
                Me(&H9E) = ChrW(&H17E)
                Me(&H9F) = ChrW(&H9F)
                Me(&HA0) = ChrW(&H20AC)

            End Sub

            Private Function IsIdentity(ByVal code As Integer) As Boolean
                Return code < 128 OrElse (code > 160 AndAlso code < 256)
            End Function

            Private Function IsIdentity(ByVal code As Char) As Boolean
                Return Me.IsIdentity(AscW(code))
            End Function

            Public Overrides ReadOnly Property Count As Integer
                Get
                    Return 256
                End Get
            End Property

            Public Overrides Function GetKey(ByVal value As Char) As Integer
                If (IsIdentity(value)) Then
                    Return AscW(value)
                Else
                    Return MyBase.GetKey(value)
                End If
            End Function

            Default Public Overrides Property Item(ByVal key As Integer) As Char
                Get
                    If (Me.IsIdentity(key)) Then
                        Return Chr(key)
                    Else
                        Return MyBase.Item(key)
                    End If
                End Get
                Set(value As Char)
                    MyBase.Item(key) = value
                End Set
            End Property

        End Class

#End Region

#Region "Static"
#Region "fields"

        Private Shared ReadOnly _instance As PdfDocEncoding = New PdfDocEncoding()

#End Region

#Region "Interface"

        Public Shared Function [Get]() As PdfDocEncoding
            Return _instance
        End Function

#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        Private Sub New()
            _chars = New Chars()
        End Sub

#End Region
#End Region

    End Class

End Namespace
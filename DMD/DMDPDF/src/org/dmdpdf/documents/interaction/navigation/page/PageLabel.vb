'/*
'  Copyright 2012 Stefano Chizzolini. http://www.dmdpdf.org

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

Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.objects
Imports DMD.org.dmdpdf.util

Imports System
Imports System.Collections.Generic
Imports System.Runtime.CompilerServices

Namespace DMD.org.dmdpdf.documents.interaction.navigation.page

    '/**
    '  <summary>Page label range [PDF:1.7:8.3.1].</summary>
    '  <remarks>It represents a series of consecutive pages' visual identifiers using the same
    '  numbering system.</remarks>
    '*/
    <PDF(VersionEnum.PDF13)>
    Public NotInheritable Class PageLabel
        Inherits PdfObjectWrapper(Of PdfDictionary)

#Region "types"
        Public Enum NumberStyleEnum

            '/**
            '  <summary>Decimal arabic numerals.</summary>
            '*/
            ArabicNumber
            '/**
            '  <summary>Upper-case roman numerals.</summary>
            '*/
            UCaseRomanNumber
            '/**
            '  <summary>Lower-case roman numerals.</summary>
            '*/
            LCaseRomanNumber
            '/**
            '  <summary>Upper-case letters (A to Z for the first 26 pages, AA to ZZ for the next 26, and so
            '  on).</summary>
            '*/
            UCaseLetter
            '/**
            '  <summary>Lower-case letters (a to z for the first 26 pages, aa to zz for the next 26, and so
            '  on).</summary>
            '*/
            LCaseLetter
        End Enum

#End Region

#Region "Static"
#Region "fields"

        Private Shared ReadOnly DefaultNumberBase As Integer = 1

#End Region

#Region "interface"

        '    /**
        '  <summary>Gets an existing page label range.</summary>
        '  <param name="baseObject">Base object to wrap.</param>
        '*/
        Public Shared Function Wrap(ByVal baseObject As PdfDirectObject) As PageLabel
            If (baseObject IsNot Nothing) Then
                Return New PageLabel(baseObject)
            Else
                Return Nothing
            End If
        End Function

#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        Public Sub New(ByVal context As documents.Document, ByVal numberStyle As NumberStyleEnum)
            Me.New(context, Nothing, numberStyle, DefaultNumberBase)
        End Sub

        Public Sub New(ByVal context As documents.Document, ByVal prefix As String, ByVal numberStyle As NumberStyleEnum, ByVal numberBase As Integer)
            MyBase.New(context,
                        New PdfDictionary(New PdfName() {PdfName.Type}, New PdfDirectObject() {PdfName.PageLabel})
                        )
            Me.Prefix = prefix
            Me.NumberStyle = numberStyle
            Me.NumberBase = numberBase
        End Sub

        Private Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary>Gets/Sets the value of the numeric suffix for the first page label in this range.
        '  Subsequent pages are numbered sequentially from this value.</summary>
        '*/
        Public Property NumberBase As Integer
            Get
                Return CInt(PdfSimpleObject(Of Object).GetValue(BaseDataObject(PdfName.St), DefaultNumberBase))
            End Get
            Set(ByVal value As Integer)
                If (value <= DefaultNumberBase) Then
                    BaseDataObject(PdfName.St) = Nothing
                Else
                    BaseDataObject(PdfName.St) = PdfInteger.Get(value)
                End If
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets the numbering style to be used for the numeric suffix of each page label in
        '  this range.</summary>
        '  <remarks>If no style is defined, the numeric suffix isn't displayed at all.</remarks>
        '*/
        Public Property NumberStyle As NumberStyleEnum
            Get
                Return NumberStyleEnumExtension.Get(CType(BaseDataObject(PdfName.S), PdfName))
            End Get
            Set(ByVal value As NumberStyleEnum)
                BaseDataObject(PdfName.S) = value.GetCode()
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets the label prefix for page labels in this range.</summary>
        '*/
        Public Property Prefix As String
            Get
                Return CStr(PdfSimpleObject(Of Object).GetValue(BaseDataObject(PdfName.P)))
            End Get
            Set(ByVal value As String)
                If (value IsNot Nothing) Then
                    BaseDataObject(PdfName.P) = New PdfTextString(value)
                Else
                    BaseDataObject(PdfName.P) = Nothing
                End If
            End Set
        End Property

#End Region
#End Region
#End Region

    End Class

    Module NumberStyleEnumExtension 'Static 

        Private ReadOnly codes As BiDictionary(Of PageLabel.NumberStyleEnum, PdfName)

        Sub New()
            codes = New BiDictionary(Of PageLabel.NumberStyleEnum, PdfName)
            codes(PageLabel.NumberStyleEnum.ArabicNumber) = PdfName.D
            codes(PageLabel.NumberStyleEnum.UCaseRomanNumber) = PdfName.R
            codes(PageLabel.NumberStyleEnum.LCaseRomanNumber) = PdfName.r_
            codes(PageLabel.NumberStyleEnum.UCaseLetter) = PdfName.A
            codes(PageLabel.NumberStyleEnum.LCaseLetter) = PdfName.a_
        End Sub

        Public Function [Get](ByVal name As PdfName) As PageLabel.NumberStyleEnum
            If (name Is Nothing) Then Throw New ArgumentNullException()
            Dim numberStyle As PageLabel.NumberStyleEnum? = codes.GetKey(name)
            If (Not numberStyle.HasValue) Then Throw New NotSupportedException("Page layout unknown: " & name.ToString)
            Return numberStyle.Value
        End Function

        <Extension>
        Public Function GetCode(ByVal numberStyle As PageLabel.NumberStyleEnum) As PdfName  'this
            Return codes(numberStyle)
        End Function

    End Module

End Namespace
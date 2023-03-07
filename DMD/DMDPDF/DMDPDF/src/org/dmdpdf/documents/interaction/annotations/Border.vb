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
Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.documents.contents
Imports DMD.org.dmdpdf.objects

Imports System
Imports System.Collections.Generic

Namespace DMD.org.dmdpdf.documents.interaction.annotations

    '/**
    '  <summary>Border characteristics [PDF:1.6:8.4.3].</summary>
    '*/
    <PDF(VersionEnum.PDF11)>
    Public NotInheritable Class Border
        Inherits PdfObjectWrapper(Of PdfDictionary)

#Region "types"

        '/**
        '  <summary>Border style [PDF:1.6:8.4.3].</summary>
        '*/
        Public Enum StyleEnum

            '/**
            '  <summary>Solid.</summary>
            '*/
            Solid
            '/**
            '  <summary>Dashed.</summary>
            '*/
            Dashed
            '/**
            '  <summary>Beveled.</summary>
            '*/
            Beveled
            '/**
            '  <summary>Inset.</summary>
            '*/
            Inset
            '/**
            '  <summary>Underline.</summary>
            '*/
            Underline
        End Enum

#End Region

#Region "Static"
#Region "fields"

        Private Shared ReadOnly _DefaultLineDash As LineDash = New LineDash(New Double() {3})
        Private Shared ReadOnly _DefaultStyle As StyleEnum = StyleEnum.Solid
        Private Shared ReadOnly _DefaultWidth As Double = 1

        Private Shared ReadOnly _StyleEnumCodes As Dictionary(Of StyleEnum, PdfName)

#End Region

#Region "constructors"

        Shared Sub New()
            _StyleEnumCodes = New Dictionary(Of StyleEnum, PdfName)
            _StyleEnumCodes(StyleEnum.Solid) = PdfName.S
            _StyleEnumCodes(StyleEnum.Dashed) = PdfName.D
            _StyleEnumCodes(StyleEnum.Beveled) = PdfName.B
            _StyleEnumCodes(StyleEnum.Inset) = PdfName.I
            _StyleEnumCodes(StyleEnum.Underline) = PdfName.U
        End Sub

#End Region

#Region "interface"
#Region "private"

        '/**
        '  <summary>Gets the code corresponding To the given value.</summary>
        '*/
        Private Shared Function ToCode(ByVal value As StyleEnum) As PdfName
            Return _StyleEnumCodes(value)
        End Function

        '/**
        '  <summary>Gets the style corresponding to the given value.</summary>
        '*/
        Private Shared Function ToStyleEnum(ByVal value As PdfName) As StyleEnum
            For Each style As KeyValuePair(Of StyleEnum, PdfName) In _StyleEnumCodes
                If (style.Value.Equals(value)) Then
                    Return style.Key
                End If
            Next
            Return _DefaultStyle
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        Public Sub New(ByVal context As Document, ByVal width As Double, ByVal style As StyleEnum, ByVal pattern As LineDash)
            MyBase.New(context, New PdfDictionary(New PdfName() {PdfName.Type}, New PdfDirectObject() {PdfName.Border}))
            Me.width = width
            Me.style = style
            Me.pattern = pattern
        End Sub

        Friend Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary>Gets/Sets the dash pattern used in case of dashed border.</summary>
        '*/
        Public Property Pattern As LineDash
            Get
                '/*
                '  NOTE: 'D' entry may be undefined.
                '*/
                Dim dashObject As PdfArray = CType(BaseDataObject(PdfName.D), PdfArray)
                If (dashObject Is Nothing) Then Return _DefaultLineDash
                Dim dashArray As Double() = New Double(dashObject.Count - 1) {}
                Dim dashLength As Integer = dashArray.Length
                For dashIndex As Integer = 0 To dashLength - 1
                    dashArray(dashIndex) = CType(dashObject(dashIndex), IPdfNumber).RawValue
                Next
                Return New LineDash(dashArray)
            End Get
            Set(ByVal value As LineDash)
                If (value Is Nothing) Then
                    Me.BaseDataObject.Remove(PdfName.D)
                Else
                    Dim dashObject As PdfArray = New PdfArray()
                    '{
                    Dim dashArray As Double() = value.DashArray
                    Dim dashLength As Integer = dashArray.Length
                    For dashIndex As Integer = 0 To dashLength - 1
                        dashObject.Add(PdfReal.Get(dashArray(dashIndex)))
                    Next
                    '}
                    Me.BaseDataObject(PdfName.D) = dashObject
                End If
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets the border style.</summary>
        '*/
        Public Property Style As StyleEnum
            Get
                Return ToStyleEnum(CType(Me.BaseDataObject(PdfName.S), PdfName))
            End Get
            Set(ByVal value As StyleEnum)
                If (value = 0 OrElse value = _DefaultStyle) Then
                    BaseDataObject.Remove(PdfName.S)
                Else
                    BaseDataObject(PdfName.S) = ToCode(value)
                End If
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets the border width in points.</summary>
        '*/
        Public Property Width As Double
            Get
                '/*
                '  NOTE: 'W' entry may be undefined.
                '*/
                Dim widthObject As IPdfNumber = CType(BaseDataObject(PdfName.W), IPdfNumber)
                If (widthObject Is Nothing) Then
                    Return _DefaultWidth
                Else
                    Return widthObject.RawValue
                End If
            End Get
            Set(ByVal value As Double)
                BaseDataObject(PdfName.W) = PdfReal.Get(value)
            End Set
        End Property

#End Region
#End Region
#End Region

    End Class

End Namespace
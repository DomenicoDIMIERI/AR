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
Imports DMD.org.dmdpdf.objects

Imports System
Imports System.Collections.Generic
Imports System.Drawing

Namespace DMD.org.dmdpdf.documents.interaction.annotations

    '/**
    '  <summary>Caret annotation [PDF:1.6:8.4.5].</summary>
    '  <remarks>It displays a visual symbol that indicates the presence of text edits.</remarks>
    '*/
    <PDF(VersionEnum.PDF15)>
    Public NotInheritable Class Caret
        Inherits Annotation

#Region "types"
        '/**
        '  <summary>Symbol type [PDF:1.6:8.4.5].</summary>
        '*/
        Public Enum SymbolTypeEnum

            '  /**
            '  <summary>New paragraph.</summary>
            '*/
            NewParagraph
            '/**
            '  <summary>None.</summary>
            '*/
            None
        End Enum

#End Region

#Region "Static"
#Region "fields"

        Private Shared ReadOnly _SymbolTypeEnumCodes As Dictionary(Of SymbolTypeEnum, PdfName)

#End Region

#Region "constructors"

        Shared Sub New()
            _SymbolTypeEnumCodes = New Dictionary(Of SymbolTypeEnum, PdfName)
            _SymbolTypeEnumCodes(SymbolTypeEnum.NewParagraph) = PdfName.P
            _SymbolTypeEnumCodes(SymbolTypeEnum.None) = PdfName.None
        End Sub

#End Region

#Region "interface"
#Region "private"

        '/**
        '  <summary>Gets the code corresponding to the given value.</summary>
        '*/
        Private Shared Function ToCode(ByVal value As SymbolTypeEnum) As PdfName
            Return _SymbolTypeEnumCodes(value)
        End Function

        '/**
        '  <summary>Gets the symbol type corresponding to the given value.</summary>
        '*/
        Private Shared Function ToSymbolTypeEnum(ByVal value As PdfName) As SymbolTypeEnum
            For Each symbolType As KeyValuePair(Of SymbolTypeEnum, PdfName) In _SymbolTypeEnumCodes
                If (symbolType.Value.Equals(value)) Then
                    Return symbolType.Key
                End If
            Next
            Return SymbolTypeEnum.None
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        Public Sub New(ByVal page As Page, ByVal box As RectangleF, ByVal text As String)
            MyBase.New(page, PdfName.Caret, box, text)
        End Sub

        Friend Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary>Gets/Sets the symbol to be used in displaying the annotation.</summary>
        '*/
        Public Property SymbolType As SymbolTypeEnum
            Get
                Return ToSymbolTypeEnum(CType(BaseDataObject(PdfName.Sy), PdfName))
            End Get
            Set(ByVal value As SymbolTypeEnum)
                BaseDataObject(PdfName.Sy) = ToCode(value)
            End Set
        End Property

#End Region
#End Region
#End Region

    End Class

End Namespace
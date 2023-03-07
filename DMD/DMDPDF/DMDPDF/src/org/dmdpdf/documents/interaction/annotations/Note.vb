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
    '  <summary>Text annotation [PDF:1.6:8.4.5].</summary>
    '  <remarks>It represents a sticky note attached to a point in the PDF document.</remarks>
    '*/
    <PDF(VersionEnum.PDF10)>
    Public NotInheritable Class Note
        Inherits Annotation

#Region "types"

        '/**
        '  <summary>Icon to be used in displaying the annotation [PDF:1.6:8.4.5].</summary>
        '*/
        Public Enum IconTypeEnum

            '  /**
            '  <summary>Comment.</summary>
            '*/
            Comment
            '/**
            '  <summary>Help.</summary>
            '*/
            Help
            '/**
            '  <summary>Insert.</summary>
            '*/
            Insert
            '/**
            '  <summary>Key.</summary>
            '*/
            Key
            '/**
            '  <summary>New paragraph.</summary>
            '*/
            NewParagraph
            '/**
            '  <summary>Note.</summary>
            '*/
            Note
            '/**
            '  <summary>Paragraph.</summary>
            '*/
            Paragraph
        End Enum

#End Region

#Region "Static"
#Region "fields"

        Private Shared ReadOnly IconTypeEnumCodes As Dictionary(Of IconTypeEnum, PdfName)

#End Region

#Region "constructors"

        Shared Sub New()
            IconTypeEnumCodes = New Dictionary(Of IconTypeEnum, PdfName)
            IconTypeEnumCodes(IconTypeEnum.Comment) = PdfName.Comment
            IconTypeEnumCodes(IconTypeEnum.Help) = PdfName.Help
            IconTypeEnumCodes(IconTypeEnum.Insert) = PdfName.Insert
            IconTypeEnumCodes(IconTypeEnum.Key) = PdfName.Key
            IconTypeEnumCodes(IconTypeEnum.NewParagraph) = PdfName.NewParagraph
            IconTypeEnumCodes(IconTypeEnum.Note) = PdfName.Note
            IconTypeEnumCodes(IconTypeEnum.Paragraph) = PdfName.Paragraph
        End Sub

#End Region

#Region "interface"
#Region "private"

        '/**
        '  <summary>Gets the code corresponding To the given value.</summary>
        '*/
        Private Shared Function ToCode(ByVal value As IconTypeEnum) As PdfName
            Return IconTypeEnumCodes(value)
        End Function

        '    /**
        '  <summary>Gets the icon type corresponding to the given value.</summary>
        '*/
        Private Shared Function ToIconTypeEnum(ByVal value As PdfName) As IconTypeEnum
            For Each iconType As KeyValuePair(Of IconTypeEnum, PdfName) In IconTypeEnumCodes
                If (iconType.Value.Equals(value)) Then
                    Return iconType.Key
                End If
            Next

            Return IconTypeEnum.Note
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        Public Sub New(ByVal page As Page, ByVal location As PointF, ByVal text As String)
            MyBase.New(page, PdfName.Text, New RectangleF(location.X, location.Y, 0, 0), text)
        End Sub

        Friend Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary>Gets/Sets the icon to be used in displaying the annotation.</summary>
        '*/
        Public Property IconType As IconTypeEnum
            Get
                Return ToIconTypeEnum(CType(BaseDataObject(PdfName.Name), PdfName))
            End Get
            Set(ByVal value As IconTypeEnum)
                BaseDataObject(PdfName.Name) = ToCode(value)
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets whether the annotation should initially be displayed open.</summary>
        '*/
        Public Property IsOpen As Boolean
            Get
                Dim openObject As PdfBoolean = CType(BaseDataObject(PdfName.Open), PdfBoolean)
                If (openObject IsNot Nothing) Then
                    Return openObject.BooleanValue
                Else
                    Return False
                End If
            End Get
            Set(ByVal value As Boolean)
                BaseDataObject(PdfName.Open) = PdfBoolean.Get(value)
            End Set
        End Property


        'TODO:State and StateModel!!!
#End Region
#End Region
#End Region

    End Class

End Namespace
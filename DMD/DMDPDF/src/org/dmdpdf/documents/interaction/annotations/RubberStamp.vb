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
    '  <summary>Rubber stamp annotation [PDF:1.6:8.4.5].</summary>
    '  <remarks>It displays text or graphics intended to look as if they were stamped
    '  on the page with a rubber stamp.</remarks>
    '*/
    <PDF(VersionEnum.PDF13)>
    Public NotInheritable Class RubberStamp
        Inherits Annotation

#Region "types"
        '/**
        '  <summary>Icon to be used in displaying the annotation [PDF:1.6:8.4.5].</summary>
        '*/
        Public Enum IconTypeEnum

            '  /**
            '  <summary>Approved.</summary>
            '*/
            Approved
            '/**
            '  <summary>As is.</summary>
            '*/
            AsIs
            '/**
            '  <summary>Confidential.</summary>
            '*/
            Confidential
            '/**
            '  <summary>Departmental.</summary>
            '*/
            Departmental
            '/**
            '  <summary>Draft.</summary>
            '*/
            Draft
            '/**
            '  <summary>Experimental.</summary>
            '*/
            Experimental
            '/**
            '  <summary>Expired.</summary>
            '*/
            Expired
            '/**
            '  <summary>Final.</summary>
            '*/
            Final
            '/**
            '  <summary>For comment.</summary>
            '*/
            ForComment
            '/**
            '  <summary>For public release.</summary>
            '*/
            ForPublicRelease
            '/**
            '  <summary>Not approved.</summary>
            '*/
            NotApproved
            '/**
            '  <summary>Not for public release.</summary>
            '*/
            NotForPublicRelease
            '/**
            '  <summary>Sold.</summary>
            '*/
            Sold
            '/**
            '  <summary>Top secret.</summary>
            '*/
            TopSecret
        End Enum

#End Region

#Region "Static"
#Region "fields"

        Private Shared ReadOnly _IconTypeEnumCodes As Dictionary(Of IconTypeEnum, PdfName)

#End Region

#Region "constructors"

        Shared Sub New()
            _IconTypeEnumCodes = New Dictionary(Of IconTypeEnum, PdfName)
            _IconTypeEnumCodes(IconTypeEnum.Approved) = PdfName.Approved
            _IconTypeEnumCodes(IconTypeEnum.AsIs) = PdfName.AsIs
            _IconTypeEnumCodes(IconTypeEnum.Confidential) = PdfName.Confidential
            _IconTypeEnumCodes(IconTypeEnum.Departmental) = PdfName.Departmental
            _IconTypeEnumCodes(IconTypeEnum.Draft) = PdfName.Draft
            _IconTypeEnumCodes(IconTypeEnum.Experimental) = PdfName.Experimental
            _IconTypeEnumCodes(IconTypeEnum.Expired) = PdfName.Expired
            _IconTypeEnumCodes(IconTypeEnum.Final) = PdfName.Final
            _IconTypeEnumCodes(IconTypeEnum.ForComment) = PdfName.ForComment
            _IconTypeEnumCodes(IconTypeEnum.ForPublicRelease) = PdfName.ForPublicRelease
            _IconTypeEnumCodes(IconTypeEnum.NotApproved) = PdfName.NotApproved
            _IconTypeEnumCodes(IconTypeEnum.NotForPublicRelease) = PdfName.NotForPublicRelease
            _IconTypeEnumCodes(IconTypeEnum.Sold) = PdfName.Sold
            _IconTypeEnumCodes(IconTypeEnum.TopSecret) = PdfName.TopSecret
        End Sub

#End Region

#Region "interface"
#Region "private"

        '/**
        '  <summary>Gets the code corresponding to the given value.</summary>
        '*/
        Private Shared Function ToCode(ByVal value As IconTypeEnum) As PdfName
            Return _IconTypeEnumCodes(value)
        End Function

        '/**
        '  <summary>Gets the icon type corresponding to the given value.</summary>
        '*/
        Private Shared Function ToIconTypeEnum(ByVal value As PdfName) As IconTypeEnum
            For Each iconType As KeyValuePair(Of IconTypeEnum, PdfName) In _IconTypeEnumCodes
                If (iconType.Value.Equals(value)) Then
                    Return iconType.Key
                End If
            Next
            Return IconTypeEnum.Draft
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        Public Sub New(ByVal page As Page, ByVal box As RectangleF, ByVal text As String, ByVal iconType As IconTypeEnum)
            MyBase.New(page, PdfName.Stamp, box, text)
            Me.IconType = iconType
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

#End Region
#End Region
#End Region

    End Class

End Namespace
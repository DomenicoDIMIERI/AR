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
Imports DMD.org.dmdpdf.documents.files
Imports DMD.org.dmdpdf.objects

Imports System
Imports System.Collections.Generic
Imports System.Drawing

Namespace DMD.org.dmdpdf.documents.interaction.annotations

    '/**
    '  <summary>File attachment annotation [PDF:1.6:8.4.5].</summary>
    '  <remarks>It represents a reference to a file, which typically is embedded in the PDF file.
    '  </remarks>
    '*/
    <PDF(VersionEnum.PDF13)>
    Public NotInheritable Class FileAttachment
        Inherits Annotation
        Implements IFileResource

#Region "types"

        '/**
        '  <summary>Icon to be used in displaying the annotation [PDF:1.6:8.4.5].</summary>
        '*/
        Public Enum IconTypeEnum

            '  /**
            '  <summary>Graph.</summary>
            '*/
            Graph
            '/**
            '  <summary>Paper clip.</summary>
            '*/
            PaperClip
            '/**
            '  <summary>Push pin.</summary>
            '*/
            PushPin
            '/**
            '  <summary>Tag.</summary>
            '*/
            Tag
        End Enum

#End Region

#Region "Static"
#Region "fields"

        Private Shared ReadOnly _IconTypeEnumCodes As Dictionary(Of IconTypeEnum, PdfName)

#End Region

#Region "constructors"

        Shared Sub New()
            _IconTypeEnumCodes = New Dictionary(Of IconTypeEnum, PdfName)
            _IconTypeEnumCodes(IconTypeEnum.Graph) = PdfName.Graph
            _IconTypeEnumCodes(IconTypeEnum.PaperClip) = PdfName.Paperclip
            _IconTypeEnumCodes(IconTypeEnum.PushPin) = PdfName.PushPin
            _IconTypeEnumCodes(IconTypeEnum.Tag) = PdfName.Tag
        End Sub

#End Region

#Region "interface"
#Region "private"

        '/**
        '  <summary>Gets the code corresponding To the given value.</summary>
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
            Return IconTypeEnum.PushPin
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        Public Sub New(ByVal page As Page, ByVal box As RectangleF, ByVal text As String, ByVal dataFile As FileSpecification)
            MyBase.New(page, PdfName.FileAttachment, box, text)
            Me.DataFile = dataFile
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

#Region "IFileResource"

        Public Property DataFile As FileSpecification Implements IFileResource.DataFile
            Get
                Return FileSpecification.Wrap(BaseDataObject(PdfName.FS))
            End Get
            Set(ByVal value As FileSpecification)
                BaseDataObject(PdfName.FS) = value.BaseObject
            End Set
        End Property

#End Region
#End Region
#End Region
#End Region

    End Class

End Namespace
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

'  You should have received a copy of the GNU Lesser General Public License along with this
'  Program (see README files); if not, go to the GNU website (http://www.gnu.org/licenses/).

'  Redistribution and use, with or without modification, are permitted provided that such
'  redistributions retain the above copyright notice, license and disclaimer, along with
'  this list of conditions.
'*/

Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects

Imports System
Imports System.Collections.Generic

Namespace DMD.org.dmdpdf.documents.interaction.viewer

    '/**
    '  <summary>Viewer preferences [PDF:1.6:8.1].</summary>
    '*/
    <PDF(VersionEnum.PDF12)>
    Public NotInheritable Class ViewerPreferences
        Inherits PdfObjectWrapper(Of PdfDictionary)

#Region "types"
        '/**
        '  <summary>Predominant reading order for text [PDF:1.6:8.1].</summary>
        '*/
        Public Enum DirectionEnum

            '  /**
            '  <summary>Left to right.</summary>
            '*/
            LeftToRight
            '/**
            '  <summary>Right to left.</summary>
            '*/
            RightToLeft
        End Enum

#End Region

#Region "Static"
#Region "fields"

        Private Shared ReadOnly _DirectionEnumCodes As Dictionary(Of DirectionEnum, PdfName)

#End Region

#Region "constructors"

        Shared Sub New()
            _DirectionEnumCodes = New Dictionary(Of DirectionEnum, PdfName)()
            _DirectionEnumCodes(DirectionEnum.LeftToRight) = PdfName.L2R
            _DirectionEnumCodes(DirectionEnum.RightToLeft) = PdfName.R2L
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public Shared Function Wrap(ByVal baseObject As PdfDirectObject) As ViewerPreferences
            If (baseObject IsNot Nothing) Then
                Return New ViewerPreferences(baseObject)
            Else
                Return Nothing
            End If
        End Function

#End Region

#Region "private"

        '/**
        '  <summary>Gets the code corresponding to the given value.</summary>
        '*/
        Private Shared Function ToCode(ByVal value As DirectionEnum) As PdfName
            Return _DirectionEnumCodes(value)
        End Function

        '/**
        '  <summary>Gets the direction corresponding to the given value.</summary>
        '*/
        Private Shared Function ToDirectionEnum(ByVal value As PdfName) As DirectionEnum
            For Each direction As KeyValuePair(Of DirectionEnum, PdfName) In _DirectionEnumCodes
                If (direction.Value.Equals(value)) Then
                    Return direction.Key
                End If
            Next
            Return DirectionEnum.LeftToRight
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        Public Sub New(ByVal context As Document)
            MyBase.New(context, New PdfDictionary())
        End Sub

        Private Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public Property CenterWindow() As Boolean
            Get
                Return CBool([Get](PdfName.CenterWindow, False))
            End Get
            Set(ByVal value As Boolean)
                BaseDataObject(PdfName.CenterWindow) = PdfBoolean.Get(value)
            End Set
        End Property

        Public Property Direction As DirectionEnum
            Get
                Return ToDirectionEnum(CType(BaseDataObject(PdfName.Direction), PdfName))
            End Get
            Set(ByVal value As DirectionEnum)
                BaseDataObject(PdfName.Direction) = ToCode(value)
            End Set
        End Property

        Public Property DisplayDocTitle As Boolean
            Get
                Return CBool([Get](PdfName.DisplayDocTitle, False))
            End Get
            Set(ByVal value As Boolean)
                BaseDataObject(PdfName.DisplayDocTitle) = PdfBoolean.Get(value)
            End Set
        End Property

        Public Property FitWindow As Boolean
            Get
                Return CBool([Get](PdfName.FitWindow, False))
            End Get
            Set(ByVal value As Boolean)
                Me.BaseDataObject(PdfName.FitWindow) = PdfBoolean.Get(value)
            End Set
        End Property

        Public Property HideMenubar As Boolean
            Get
                Return CBool([Get](PdfName.HideMenubar, False))
            End Get
            Set(ByVal value As Boolean)
                BaseDataObject(PdfName.HideMenubar) = PdfBoolean.Get(value)
            End Set
        End Property

        Public Property HideToolbar As Boolean
            Get
                Return CBool([Get](PdfName.HideToolbar, False))
            End Get
            Set(ByVal value As Boolean)
                BaseDataObject(PdfName.HideToolbar) = PdfBoolean.Get(value)
            End Set
        End Property

        Public Property HideWindowUI As Boolean
            Get
                Return CBool([Get](PdfName.HideWindowUI, False))
            End Get
            Set(ByVal value As Boolean)
                BaseDataObject(PdfName.HideWindowUI) = PdfBoolean.Get(value)
            End Set
        End Property

#End Region

#Region "Private"

        Private Function [Get](ByVal key As PdfName, ByVal defaultValue As Object) As Object
            Return PdfSimpleObject(Of Object).GetValue(BaseDataObject(key), defaultValue)
        End Function

#End Region
#End Region
#End Region

    End Class

End Namespace
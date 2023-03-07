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
Imports DMD.org.dmdpdf.documents.interaction.forms
Imports DMD.org.dmdpdf.objects
Imports DMD.org.dmdpdf.util

Imports System
Imports System.Collections.Generic
Imports System.Drawing

Namespace DMD.org.dmdpdf.documents.interaction.annotations

    '/**
    '  <summary>Widget annotation [PDF:1.6:8.4.5].</summary>
    '*/
    <PDF(VersionEnum.PDF12)>
    Public Class Widget
        Inherits Annotation

#Region "types"
        '/**
        '  <summary>Highlighting mode [PDF:1.6:8.4.5].</summary>
        '*/
        Public Enum HighlightModeEnum

            '  /**
            '  <summary>No highlighting.</summary>
            '*/
            None
            '/**
            '  <summary>Invert the contents of the annotation rectangle.</summary>
            '*/
            Invert
            '/**
            '  <summary>Invert the annotation's border.</summary>
            '*/
            Outline
            '/**
            '  <summary>Display the annotation's down appearance.</summary>
            '*/
            Push
            '/**
            '  <summary>Same as Push (which is preferred).</summary>
            '*/
            Toggle
        End Enum

#End Region

#Region "Static"
#Region "fields"

        Private Shared ReadOnly _HighlightModeEnumCodes As Dictionary(Of HighlightModeEnum, PdfName)

#End Region

#Region "constructors"

        Shared Sub New()
            _HighlightModeEnumCodes = New Dictionary(Of HighlightModeEnum, PdfName)
            _HighlightModeEnumCodes(HighlightModeEnum.None) = PdfName.N
            _HighlightModeEnumCodes(HighlightModeEnum.Invert) = PdfName.I
            _HighlightModeEnumCodes(HighlightModeEnum.Outline) = PdfName.O
            _HighlightModeEnumCodes(HighlightModeEnum.Push) = PdfName.P
            _HighlightModeEnumCodes(HighlightModeEnum.Toggle) = PdfName.T
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public Shared Shadows Function Wrap(ByVal baseObject As PdfDirectObject, ByVal field As Field) As Widget
            If (
                (TypeOf (field) Is CheckBox) OrElse
                (TypeOf (field) Is RadioButton)
                ) Then
                Return New DualWidget(baseObject)
            Else
                Return New Widget(baseObject)
            End If
        End Function

#End Region

#Region "private"

        '/**
        '  <summary>Gets the code corresponding to the given value.</summary>
        '*/
        Private Shared Function ToCode(ByVal value As HighlightModeEnum) As PdfName
            Return _HighlightModeEnumCodes(value)
        End Function

        '/**
        '  <summary>Gets the highlighting mode corresponding to the given value.</summary>
        '*/
        Private Shared Function ToHighlightModeEnum(ByVal value As PdfName) As HighlightModeEnum
            For Each mode As KeyValuePair(Of HighlightModeEnum, PdfName) In _HighlightModeEnumCodes
                If (mode.Value.Equals(value)) Then
                    Return mode.Key
                End If
            Next
            Return HighlightModeEnum.Invert
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        Public Sub New(ByVal page As Page, ByVal box As RectangleF)
            MyBase.New(page, PdfName.Widget, box, Nothing)
            Me.Flags = EnumUtils.Mask(Me.Flags, FlagsEnum.Print, True)
        End Sub

        Friend Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public Overrides Property Actions As AnnotationActions
            Get
                Dim actionsObject As PdfDirectObject = BaseDataObject(PdfName.AA)
                If (actionsObject IsNot Nothing) Then
                    Return New WidgetActions(Me, actionsObject)
                Else
                    Return Nothing
                End If
            End Get
            Set(ByVal value As AnnotationActions)
                MyBase.Actions = value
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets the annotation's appearance characteristics to be used for its visual
        '  presentation on the page.</summary>
        '*/
        Public Property AppearanceCharacteristics As AppearanceCharacteristics
            Get
                Return AppearanceCharacteristics.Wrap(BaseDataObject.Get(Of PdfDictionary)(PdfName.MK))
            End Get
            Set(ByVal value As AppearanceCharacteristics)
                Me.BaseDataObject(PdfName.MK) = value.BaseObject
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets the annotation's highlighting mode, the visual effect to be used
        '  when the mouse button is pressed or held down inside its active area.</summary>
        '*/
        Public Property HighlightMode As HighlightModeEnum
            Get
                Return ToHighlightModeEnum(CType(Me.BaseDataObject(PdfName.H), PdfName))
            End Get
            Set(ByVal value As HighlightModeEnum)
                Me.BaseDataObject(PdfName.H) = ToCode(value)
            End Set
        End Property

#End Region
#End Region
#End Region

    End Class

End Namespace
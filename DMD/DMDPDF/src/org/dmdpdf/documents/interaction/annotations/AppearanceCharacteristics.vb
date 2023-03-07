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
Imports DMD.org.dmdpdf.documents.contents.colorSpaces
Imports DMD.org.dmdpdf.documents.contents.composition
Imports DMD.org.dmdpdf.documents.contents.xObjects
Imports DMD.org.dmdpdf.objects

Imports System
Imports System.Collections.Generic

Namespace DMD.org.dmdpdf.documents.interaction.annotations

    '/**
    '  <summary>Appearance characteristics [PDF:1.6:8.4.5].</summary>
    '*/
    <PDF(VersionEnum.PDF12)>
    Public NotInheritable Class AppearanceCharacteristics
        Inherits PdfObjectWrapper(Of PdfDictionary)

#Region "types"
        '/**
        '  <summary> Caption position relative To its icon [PDF:1.6:8.4.5].</summary>
        '*/
        Public Enum CaptionPositionEnum

            '  /**
            '  <summary> Caption only (no icon).</summary>
            '*/
            CaptionOnly = 0
            '/**
            '  <summary> No caption (icon only).</summary>
            '*/
            NoCaption = 1
            '/**
            '  <summary> Caption below the icon.</summary>
            '*/
            Below = 2
            '/**
            '  <summary> Caption above the icon.</summary>
            '*/
            Above = 3
            '/**
            '  <summary> Caption To the right Of the icon.</summary>
            '*/
            Right = 4
            '/**
            '  <summary> Caption To the left Of the icon.</summary>
            '*/
            Left = 5
            '/**
            '  <summary> Caption overlaid directly On the icon.</summary>
            '*/
            Overlaid = 6
        End Enum

        '/**
        '  <summary> Icon fit [PDF:1.6:8.6.6].</summary>
        '*/
        Public Class IconFitObject
            Inherits PdfObjectWrapper(Of PdfDictionary)

#Region "types"
            '/**
            '  <summary>Scaling mode [PDF:1.6:8.6.6].</summary>
            '*/
            Public Enum ScaleModeEnum

                '    /**
                '  <summary>Always scale.</summary>
                '*/
                Always
                '/**
                '  <summary>Scale only when the icon is bigger than the annotation box.</summary>
                '*/
                Bigger
                '/**
                '  <summary>Scale only when the icon is smaller than the annotation box.</summary>
                '*/
                Smaller
                '/**
                '  <summary>Never scale.</summary>
                '*/
                Never
            End Enum

            '/**
            '  <summary>Scaling type [PDF:1.6:8.6.6].</summary>
            '*/
            Public Enum ScaleTypeEnum

                '    /**
                '  <summary>Scale the icon to fill the annotation box exactly,
                '  without regard to its original aspect ratio.</summary>
                '*/
                Anamorphic
                '/**
                '  <summary>Scale the icon to fit the width or height of the annotation box,
                '  while maintaining the icon's original aspect ratio.</summary>
                '*/
                Proportional
            End Enum
#End Region

#Region "Static"
#Region "fields"

            Private Shared ReadOnly _ScaleModeEnumCodes As Dictionary(Of ScaleModeEnum, PdfName)
            Private Shared ReadOnly _ScaleTypeEnumCodes As Dictionary(Of ScaleTypeEnum, PdfName)

#End Region

#Region "constructors"

            Shared Sub New()
                _ScaleModeEnumCodes = New Dictionary(Of ScaleModeEnum, PdfName)()
                _ScaleModeEnumCodes(ScaleModeEnum.Always) = PdfName.A
                _ScaleModeEnumCodes(ScaleModeEnum.Bigger) = PdfName.B
                _ScaleModeEnumCodes(ScaleModeEnum.Smaller) = PdfName.S
                _ScaleModeEnumCodes(ScaleModeEnum.Never) = PdfName.N

                _ScaleTypeEnumCodes = New Dictionary(Of ScaleTypeEnum, PdfName)()
                _ScaleTypeEnumCodes(ScaleTypeEnum.Anamorphic) = PdfName.A
                _ScaleTypeEnumCodes(ScaleTypeEnum.Proportional) = PdfName.P
            End Sub

#End Region

#Region "interface"
#Region "private"
            '/**
            '  <summary> Gets the code corresponding To the given value.</summary>
            '*/
            Private Shared Function ToCode(ByVal value As ScaleModeEnum) As PdfName
                Return _ScaleModeEnumCodes(value)
            End Function

            '/**
            '  <summary> Gets the code corresponding To the given value.</summary>
            '*/
            Private Shared Function ToCode(ByVal value As ScaleTypeEnum) As PdfName
                Return _ScaleTypeEnumCodes(value)
            End Function

            '/**
            '  <summary> Gets the scaling mode corresponding To the given value.</summary>
            '*/
            Private Shared Function ToScaleModeEnum(ByVal value As PdfName) As ScaleModeEnum
                For Each scaleMode As KeyValuePair(Of ScaleModeEnum, PdfName) In _ScaleModeEnumCodes
                    If (scaleMode.Value.Equals(value)) Then
                        Return scaleMode.Key
                    End If
                Next
                Return ScaleModeEnum.Always
            End Function

            '/**
            '  <summary> Gets the scaling type corresponding To the given value.</summary>
            '*/
            Private Shared Function ToScaleTypeEnum(ByVal value As PdfName) As ScaleTypeEnum
                For Each scaleType As KeyValuePair(Of ScaleTypeEnum, PdfName) In _ScaleTypeEnumCodes
                    If (scaleType.Value.Equals(value)) Then
                        Return scaleType.Key
                    End If
                Next
                Return ScaleTypeEnum.Proportional
            End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "constructors"

            Public Sub New(ByVal context As Document)
                MyBase.New(context, New PdfDictionary())
            End Sub

            Friend Sub New(ByVal baseObject As PdfDirectObject)
                MyBase.New(baseObject)
            End Sub

#End Region

#Region "interface"
#Region "public"

            '/**
            '  <summary>Gets/Sets whether Not To take into consideration the line width Of the border.</summary>
            '*/
            Public Property BorderExcluded As Boolean
                Get
                    Dim borderExcludedObject As PdfBoolean = CType(Me.BaseDataObject(PdfName.FB), PdfBoolean)
                    If (borderExcludedObject IsNot Nothing) Then
                        Return borderExcludedObject.RawValue
                    Else
                        Return False
                    End If
                End Get
                Set(ByVal value As Boolean)
                    Me.BaseDataObject(PdfName.FB) = PdfBoolean.Get(value)
                End Set
            End Property

            '/**
            '  <summary>Gets/Sets the circumstances under which the icon should be scaled inside the annotation box.</summary>
            '*/
            Public Property ScaleMode As ScaleModeEnum
                Get
                    Return ToScaleModeEnum(CType(BaseDataObject(PdfName.SW), PdfName))
                End Get
                Set(ByVal value As ScaleModeEnum)
                    Me.BaseDataObject(PdfName.SW) = ToCode(value)
                End Set
            End Property

            '/**
            '  <summary>Gets/Sets the type Of scaling To use.</summary>
            '*/
            Public Property ScaleType As ScaleTypeEnum
                Get
                    Return ToScaleTypeEnum(CType(Me.BaseDataObject(PdfName.S), PdfName))
                End Get
                Set(ByVal value As ScaleTypeEnum)
                    Me.BaseDataObject(PdfName.S) = ToCode(value)
                End Set
            End Property

            '/**
            '  <summary>Gets/Sets the horizontal alignment Of the icon inside the annotation box.</summary>
            '*/
            Public Property XAlignment As XAlignmentEnum
                Get
                    '/*
                    '  NOTE: 'A' entry may be undefined.
                    '*/
                    Dim alignmentObject As PdfArray = CType(Me.BaseDataObject(PdfName.A), PdfArray)
                    If (alignmentObject IsNot Nothing) Then Return XAlignmentEnum.Center

                    'Switch((Int())Math.Round(CType(alignmentObject(0), IPdfNumber).RawValue/.5))
                    Select Case CInt(Math.Round(CType(alignmentObject(0), IPdfNumber).RawValue * 2))
                        Case 0 : Return XAlignmentEnum.Left
                        Case 2 : Return XAlignmentEnum.Right
                        Case Else : Return XAlignmentEnum.Center
                    End Select
                End Get
                Set(ByVal value As XAlignmentEnum)
                    '/*
                    '  NOTE: 'A' entry may be undefined.
                    '*/
                    Dim alignmentObject As PdfArray = CType(BaseDataObject(PdfName.A), PdfArray)
                    If (alignmentObject Is Nothing) Then
                        alignmentObject = New PdfArray(New PdfDirectObject() {
                                                                PdfReal.Get(0.5),
                                                                PdfReal.Get(0.5)
                                                                }
                                                       )
                        BaseDataObject(PdfName.A) = alignmentObject
                    End If
                    Dim objectValue As Double
                    Select Case (value)
                        Case XAlignmentEnum.Left : objectValue = 0'; break;
                        Case XAlignmentEnum.Right : objectValue = 1 '; break;
                        Case Else : objectValue = 0.5 '; break;
                    End Select
                    alignmentObject(0) = PdfReal.Get(objectValue)
                End Set
            End Property

            '/**
            '  <summary>Gets/Sets the vertical alignment Of the icon inside the annotation box.</summary>
            '*/
            Public Property YAlignment As YAlignmentEnum
                Get
                    '/*
                    '  NOTE: 'A' entry may be undefined.
                    '*/
                    Dim alignmentObject As PdfArray = CType(BaseDataObject(PdfName.A), PdfArray)
                    If (alignmentObject Is Nothing) Then Return YAlignmentEnum.Middle

                    'Switch((Int())Math.Round(((IPdfNumber)alignmentObject(1)).RawValue/.5))
                    Select Case CInt(Math.Round(CType(alignmentObject(1), IPdfNumber).RawValue * 2))
                        Case 0 : Return YAlignmentEnum.Bottom
                        Case 2 : Return YAlignmentEnum.Top
                        Case Else : Return YAlignmentEnum.Middle
                    End Select
                End Get
                Set(ByVal value As YAlignmentEnum)
                    '/*
                    '  NOTE: 'A' entry may be undefined.
                    '*/
                    Dim alignmentObject As PdfArray = CType(Me.BaseDataObject(PdfName.A), PdfArray)
                    If (alignmentObject Is Nothing) Then
                        alignmentObject = New PdfArray(
                                                New PdfDirectObject() {
                                                        PdfReal.Get(0.5),
                                                        PdfReal.Get(0.5)
                                                        }
                                                )
                        BaseDataObject(PdfName.A) = alignmentObject
                    End If
                    Dim objectValue As Double
                    Select Case (value)
                        Case YAlignmentEnum.Bottom : objectValue = 0'; break
                        Case YAlignmentEnum.Top : objectValue = 1 '; break;
                        Case Else : objectValue = 0.5 '; break;
                    End Select
                    alignmentObject(1) = PdfReal.Get(objectValue)
                End Set
            End Property

#End Region
#End Region
#End Region
        End Class

        '/**
        '  <summary>Annotation orientation [PDF:1.6:8.4.5].</summary>
        '*/
        Public Enum OrientationEnum

            '  /**
            '  <summary> Upward.</summary>
            '*/
            Up = 0
            '/**
            '  <summary> Leftward.</summary>
            '*/
            Left = 90
            '/**
            '  <summary> Downward.</summary>
            '*/
            Down = 180
            '/**
            '  <summary> Rightward.</summary>
            '*/
            Right = 270
        End Enum

#End Region

#Region "Static"
#Region "Interface"
#Region "Public"

        Public Shared Function Wrap(ByVal baseObject As PdfDirectObject) As AppearanceCharacteristics
            If (baseObject IsNot Nothing) Then
                Return New AppearanceCharacteristics(baseObject)
            Else
                Return Nothing
            End If
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

#Region "interface"
#Region "public"

        '/**
        '  <summary>Gets/Sets the widget annotation's alternate (down) caption,
        '  displayed when the mouse button is pressed within its active area
        '  (Pushbutton fields only).</summary>
        '*/
        Public Property AlternateCaption As String
            Get
                Dim alternateCaptionObject As PdfTextString = CType(Me.BaseDataObject(PdfName.AC), PdfTextString)
                If (alternateCaptionObject Is Nothing) Then
                    Return CStr(alternateCaptionObject.Value)
                Else
                    Return Nothing
                End If
            End Get
            Set(ByVal value As String)
                Me.BaseDataObject(PdfName.AC) = New PdfTextString(value)
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets the widget annotation's alternate (down) icon definition,
        '  displayed when the mouse button is pressed within its active area
        '  (Pushbutton fields only).</summary>
        '*/
        Public Property AlternateIcon As FormXObject
            Get
                Return FormXObject.Wrap(Me.BaseDataObject(PdfName.IX))
            End Get
            Set(ByVal value As FormXObject)
                Me.BaseDataObject(PdfName.IX) = value.BaseObject
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets the widget annotation's background color.</summary>
        '*/
        Public Property BackgroundColor As DeviceColor
            Get
                Return GetColor(PdfName.BG)
            End Get
            Set(ByVal value As DeviceColor)
                SetColor(PdfName.BG, value)
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets the widget annotation's border color.</summary>
        '*/
        Public Property BorderColor As DeviceColor
            Get
                Return GetColor(PdfName.BC)
            End Get
            Set(ByVal value As DeviceColor)
                SetColor(PdfName.BC, value)
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets the position of the caption relative to its icon (Pushbutton fields only).</summary>
        '*/
        Public Property CaptionPosition As CaptionPositionEnum
            Get
                Dim captionPositionObject As PdfInteger = CType(Me.BaseDataObject(PdfName.TP), PdfInteger)
                If (captionPositionObject IsNot Nothing) Then
                    Return CType(captionPositionObject.RawValue, CaptionPositionEnum)
                Else
                    Return CaptionPositionEnum.CaptionOnly
                End If
            End Get
            Set(ByVal value As CaptionPositionEnum)
                Me.BaseDataObject(PdfName.TP) = PdfInteger.Get(CInt(value))
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets the icon fit specifying how to display the widget annotation's icon
        '  within its annotation box (Pushbutton fields only).
        '  If present, the icon fit applies to all of the annotation's icons
        '  (normal, rollover, and alternate).</summary>
        '*/
        Public Property IconFit As IconFitObject
            Get
                Dim iconFitObject As PdfDirectObject = Me.BaseDataObject(PdfName.IF)
                If (iconFitObject IsNot Nothing) Then
                    Return New IconFitObject(iconFitObject)
                Else
                    Return Nothing
                End If
            End Get
            Set(ByVal value As IconFitObject)
                Me.BaseDataObject(PdfName.IF) = PdfObjectWrapper.GetBaseObject(value)
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets the widget annotation's normal caption,
        '  displayed when it is not interacting with the user (Button fields only).</summary>
        '*/
        Public Property NormalCaption As String
            Get
                Dim normalCaptionObject As PdfTextString = CType(Me.BaseDataObject(PdfName.CA), PdfTextString)
                If (normalCaptionObject IsNot Nothing) Then
                    Return CStr(normalCaptionObject.Value)
                Else
                    Return Nothing
                End If
            End Get
            Set(ByVal value As String)
                Me.BaseDataObject(PdfName.CA) = PdfTextString.Get(value)
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets the widget annotation's normal icon definition,
        '  displayed when it is not interacting with the user (Pushbutton fields only).</summary>
        '*/
        Public Property NormalIcon As FormXObject
            Get
                Return FormXObject.Wrap(Me.BaseDataObject(PdfName.I))
            End Get
            Set(ByVal value As FormXObject)
                Me.BaseDataObject(PdfName.I) = PdfObjectWrapper.GetBaseObject(value)
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets the widget annotation's orientation.</summary>
        '*/
        Public Property Orientation As OrientationEnum
            Get
                Dim orientationObject As PdfInteger = CType(BaseDataObject(PdfName.R), PdfInteger)
                If (orientationObject IsNot Nothing) Then
                    Return CType(orientationObject.RawValue, OrientationEnum)
                Else
                    Return OrientationEnum.Up
                End If
            End Get
            Set(ByVal value As OrientationEnum)
                Me.BaseDataObject(PdfName.R) = PdfInteger.Get(CInt(value))
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets the widget annotation's rollover caption,
        '  displayed when the user rolls the cursor into its active area
        '  without pressing the mouse button (Pushbutton fields only).</summary>
        '*/
        Public Property RolloverCaption As String
            Get
                Dim rolloverCaptionObject As PdfTextString = CType(Me.BaseDataObject(PdfName.RC), PdfTextString)
                If (rolloverCaptionObject IsNot Nothing) Then
                    Return CStr(rolloverCaptionObject.Value)
                Else
                    Return Nothing
                End If
            End Get
            Set(ByVal value As String)
                Me.BaseDataObject(PdfName.RC) = PdfTextString.Get(value)
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets the widget annotation's rollover icon definition,
        '  displayed when the user rolls the cursor into its active area
        '  without pressing the mouse button (Pushbutton fields only).</summary>
        '*/
        Public Property RolloverIcon As FormXObject
            Get
                Return FormXObject.Wrap(BaseDataObject(PdfName.RI))
            End Get
            Set(ByVal value As FormXObject)
                Me.BaseDataObject(PdfName.RI) = PdfObjectWrapper.GetBaseObject(value)
            End Set
        End Property

#End Region

#Region "Private"

        Private Function GetColor(ByVal key As PdfName) As DeviceColor
            Return DeviceColor.Get(CType(BaseDataObject.Resolve(key), PdfArray))
        End Function

        Private Sub SetColor(ByVal key As PdfName, ByVal value As DeviceColor)
            BaseDataObject(key) = PdfObjectWrapper.GetBaseObject(value)
        End Sub

#End Region
#End Region
#End Region

    End Class

End Namespace
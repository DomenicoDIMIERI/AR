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

Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.objects

Imports System
Imports System.Collections.Generic

Namespace DMD.org.dmdpdf.documents.interaction.navigation.page

    '/**
    '  <summary>Visual transition to use when moving to a page during a presentation [PDF:1.6:8.3.3].</summary>
    '*/
    <PDF(VersionEnum.PDF11)>
    Public NotInheritable Class Transition
        Inherits PdfObjectWrapper(Of PdfDictionary)

#Region "types"
        '/**
        '  <summary>Transition direction (counterclockwise) [PDF:1.6:8.3.3].</summary>
        '*/
        Public Enum DirectionEnum As Integer

            '/**
            '  <summary>Left to right.</summary>
            '*/
            LeftToRight
            '/**
            '  <summary>Bottom to top.</summary>
            '*/
            BottomToTop
            '/**
            '  <summary>Right to left.</summary>
            '*/
            RightToLeft
            '/**
            '  <summary>Top to bottom.</summary>
            '*/
            TopToBottom
            '/**
            '  <summary>Top-left to bottom-right.</summary>
            '*/
            TopLeftToBottomRight
            '/**
            '  <summary>None.</summary>
            '*/
            None
        End Enum

        '/**
        '  <summary>Transition orientation [PDF:1.6:8.3.3].</summary>
        '*/
        Public Enum OrientationEnum As Integer

            '/**
            '  <summary>Horizontal.</summary>
            '*/
            Horizontal
            '/**
            '  <summary>Vertical.</summary>
            '*/
            Vertical
        End Enum

        '/**
        '  <summary>Transition direction on page [PDF:1.6:8.3.3].</summary>
        '*/
        Public Enum PageDirectionEnum As Integer

            '/**
            '  <summary>Inward (from the edges of the page).</summary>
            '*/
            Inward
            '/**
            '  <summary>Outward (from the center of the page).</summary>
            '*/
            Outward
        End Enum

        '/**
        '  <summary>Transition style [PDF:1.6:8.3.3].</summary>
        '*/
        Public Enum StyleEnum As Integer

            '/**
            '  <summary>Two lines sweep across the screen, revealing the page.</summary>
            '*
            Split
            '/**
            '  <summary>Multiple lines sweep across the screen, revealing the page.</summary>
            '*/
            Blinds
            '/**
            '  <summary>A rectangular box sweeps between the edges of the page and the center.</summary>
            '*/
            Box
            '/**
            '  <summary>A single line sweeps across the screen from one edge to the other.</summary>
            '*/
            Wipe
            '/**
            '  <summary>The old page dissolves gradually.</summary>
            '*/
            Dissolve
            '/**
            '  <summary>The old page dissolves gradually sweeping across the page in a wide band
            '  moving from one side of the screen to the other.</summary>
            '*/
            Glitter
            '/**
            '  <summary>No transition.</summary>
            '*/
            Replace
            '/**
            '  <summary>Changes are flown across the screen.</summary>
            '*/
            <PDF(VersionEnum.PDF15)>
            Fly
            '/**
            '  <summary>The page slides in, pushing away the old one.</summary>
            '*/
            <PDF(VersionEnum.PDF15)>
            Push
            '/**
            '  <summary>The page slides on to the screen, covering the old one.</summary>
            '*/
            <PDF(VersionEnum.PDF15)>
            Cover
            '/**
            '  <summary>The old page slides off the screen, uncovering the new one.</summary>
            '*/
            <PDF(VersionEnum.PDF15)>
            Uncover
            '/**
            '  <summary>The new page reveals gradually.</summary>
            '*/
            <PDF(VersionEnum.PDF15)>
            Fade
        End Enum

#End Region

#Region "Static"
#Region "fields"

        Private Shared ReadOnly DirectionEnumCodes As Dictionary(Of DirectionEnum, PdfDirectObject)
        Private Shared ReadOnly OrientationEnumCodes As Dictionary(Of OrientationEnum, PdfName)
        Private Shared ReadOnly PageDirectionEnumCodes As Dictionary(Of PageDirectionEnum, PdfName)
        Private Shared ReadOnly StyleEnumCodes As Dictionary(Of StyleEnum, PdfName)

        Private Shared ReadOnly DefaultDirection As DirectionEnum = DirectionEnum.LeftToRight
        Private Shared ReadOnly DefaultDuration As Double = 1
        Private Shared ReadOnly DefaultOrientation As OrientationEnum = OrientationEnum.Horizontal
        Private Shared ReadOnly DefaultPageDirection As PageDirectionEnum = PageDirectionEnum.Inward
        Private Shared ReadOnly DefaultScale As Double = 1
        Private Shared ReadOnly DefaultStyle As StyleEnum = StyleEnum.Replace
#End Region

#Region "constructors"

        Shared Sub New()

            'TODO: transfer to extension methods!
            DirectionEnumCodes = New Dictionary(Of DirectionEnum, PdfDirectObject)
            DirectionEnumCodes(DirectionEnum.LeftToRight) = PdfInteger.Get(0)
            DirectionEnumCodes(DirectionEnum.BottomToTop) = PdfInteger.Get(90)
            DirectionEnumCodes(DirectionEnum.RightToLeft) = PdfInteger.Get(180)
            DirectionEnumCodes(DirectionEnum.TopToBottom) = PdfInteger.Get(270)
            DirectionEnumCodes(DirectionEnum.TopLeftToBottomRight) = PdfInteger.Get(315)
            DirectionEnumCodes(DirectionEnum.None) = PdfName.None

            OrientationEnumCodes = New Dictionary(Of OrientationEnum, PdfName)
            OrientationEnumCodes(OrientationEnum.Horizontal) = PdfName.H
            OrientationEnumCodes(OrientationEnum.Vertical) = PdfName.V

            PageDirectionEnumCodes = New Dictionary(Of PageDirectionEnum, PdfName)
            PageDirectionEnumCodes(PageDirectionEnum.Inward) = PdfName.I
            PageDirectionEnumCodes(PageDirectionEnum.Outward) = PdfName.O

            StyleEnumCodes = New Dictionary(Of StyleEnum, PdfName)
            StyleEnumCodes(StyleEnum.Split) = PdfName.Split
            StyleEnumCodes(StyleEnum.Blinds) = PdfName.Blinds
            StyleEnumCodes(StyleEnum.Box) = PdfName.Box
            StyleEnumCodes(StyleEnum.Wipe) = PdfName.Wipe
            StyleEnumCodes(StyleEnum.Dissolve) = PdfName.Dissolve
            StyleEnumCodes(StyleEnum.Glitter) = PdfName.Glitter
            StyleEnumCodes(StyleEnum.Replace) = PdfName.R
            StyleEnumCodes(StyleEnum.Fly) = PdfName.Fly
            StyleEnumCodes(StyleEnum.Push) = PdfName.Push
            StyleEnumCodes(StyleEnum.Cover) = PdfName.Cover
            StyleEnumCodes(StyleEnum.Uncover) = PdfName.Uncover
            StyleEnumCodes(StyleEnum.Fade) = PdfName.Fade
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public Shared Function Wrap(ByVal BaseObject As PdfDirectObject) As Transition
            If (BaseObject IsNot Nothing) Then
                Return New Transition(BaseObject)
            Else
                Return Nothing
            End If
        End Function

#End Region

#Region "private"

        '    /**
        '  <summary> Gets the code corresponding To the given value.</summary>
        '*/
        Private Shared Function ToCode(ByVal value As DirectionEnum) As PdfDirectObject
            Return DirectionEnumCodes(value)
        End Function

        '/**
        '  <summary> Gets the code corresponding To the given value.</summary>
        '*/
        Private Shared Function ToCode(ByVal value As OrientationEnum) As PdfName
            Return OrientationEnumCodes(value)
        End Function

        '/**
        '  <summary> Gets the code corresponding To the given value.</summary>
        '*/
        Private Shared Function ToCode(ByVal value As PageDirectionEnum) As PdfName
            Return PageDirectionEnumCodes(value)
        End Function

        '/**
        '  <summary> Gets the code corresponding To the given value.</summary>
        '*/
        Private Shared Function ToCode(ByVal value As StyleEnum) As PdfName
            Return StyleEnumCodes(value)
        End Function

        '/**
        '  <summary> Gets the direction corresponding To the given value.</summary>
        '*/
        Private Shared Function ToDirectionEnum(ByVal value As PdfDirectObject) As DirectionEnum
            For Each direction As KeyValuePair(Of DirectionEnum, PdfDirectObject) In DirectionEnumCodes
                If (direction.Value.Equals(value)) Then
                    Return direction.Key
                End If
            Next
            Return DefaultDirection
        End Function

        '/**
        '  <summary> Gets the orientation corresponding To the given value.</summary>
        '*/
        Private Shared Function ToOrientationEnum(ByVal value As PdfName) As OrientationEnum
            For Each orientation As KeyValuePair(Of OrientationEnum, PdfName) In OrientationEnumCodes
                If (orientation.Value.Equals(value)) Then
                    Return orientation.Key
                End If
            Next
            Return DefaultOrientation
        End Function

        '/**
        '  <summary> Gets the page direction corresponding To the given value.</summary>
        '*/
        Private Shared Function ToPageDirectionEnum(ByVal value As PdfName) As PageDirectionEnum
            For Each direction As KeyValuePair(Of PageDirectionEnum, PdfName) In PageDirectionEnumCodes
                If (direction.Value.Equals(value)) Then
                    Return direction.Key
                End If
            Next
            Return DefaultPageDirection
        End Function

        '/**
        '  <summary> Gets the style corresponding To the given value.</summary>
        '*/
        Private Shared Function ToStyleEnum(ByVal value As PdfName) As StyleEnum
            For Each style As KeyValuePair(Of StyleEnum, PdfName) In StyleEnumCodes
                If (style.Value.Equals(value)) Then
                    Return style.Key
                End If
            Next
            Return DefaultStyle
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        '    /**
        '  <summary>Creates a new action within the given document context.</summary>
        '*/
        Public Sub New(ByVal context As documents.Document)
            MyBase.New(context,
                        New PdfDictionary(New PdfName() {PdfName.Type}, New PdfDirectObject() {PdfName.Trans})
                        )
        End Sub


        Public Sub New(ByVal context As documents.Document, ByVal style As StyleEnum)
            Me.New(context, style, DefaultDuration, DefaultOrientation, DefaultPageDirection, DefaultDirection, DefaultScale)
        End Sub

        Public Sub New(ByVal context As documents.Document, ByVal style As StyleEnum, ByVal duration As Double)
            Me.New(context, style, duration, DefaultOrientation, DefaultPageDirection, DefaultDirection, DefaultScale)
        End Sub

        Public Sub New(ByVal context As documents.Document, ByVal style As StyleEnum, ByVal duration As Double, ByVal orientation As OrientationEnum, ByVal pageDirection As PageDirectionEnum, ByVal direction As DirectionEnum, ByVal scale As Double)
            Me.New(context)
            Me.Style = style
            Me.Duration = duration
            Me.Orientation = orientation
            Me.PageDirection = pageDirection
            Me.Direction = direction
            Me.Scale = scale
        End Sub

        Private Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary>Gets/Sets the transition direction.</summary>
        '*/
        Public Property Direction As DirectionEnum
            Get
                Return ToDirectionEnum(BaseDataObject(PdfName.Di))
            End Get
            Set(ByVal value As DirectionEnum)
                If (value = DefaultDirection) Then
                    BaseDataObject.Remove(PdfName.Di)
                Else
                    BaseDataObject(PdfName.Di) = ToCode(value)
                End If
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets the duration of the transition effect, in seconds.</summary>
        '*/
        Public Property Duration As Double
            Get
                Dim durationObject As IPdfNumber = CType(BaseDataObject(PdfName.D), IPdfNumber)
                If (durationObject Is Nothing) Then
                    Return DefaultDuration
                Else
                    Return durationObject.RawValue
                End If
            End Get
            Set(ByVal value As Double)
                If (value = DefaultDuration) Then
                    BaseDataObject.Remove(PdfName.D)
                Else
                    BaseDataObject(PdfName.D) = PdfReal.Get(value)
                End If
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets the transition orientation.</summary>
        '*/
        Public Property Orientation As OrientationEnum
            Get
                Return ToOrientationEnum(CType(BaseDataObject(PdfName.Dm), PdfName))
            End Get
            Set(ByVal value As OrientationEnum)
                If (value = DefaultOrientation) Then
                    BaseDataObject.Remove(PdfName.Dm)
                Else
                    BaseDataObject(PdfName.Dm) = ToCode(value)
                End If
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets the transition direction on page.</summary>
        '*/
        Public Property PageDirection As PageDirectionEnum
            Get
                Return ToPageDirectionEnum(CType(BaseDataObject(PdfName.M), PdfName))
            End Get
            Set(ByVal value As PageDirectionEnum)
                If (value = DefaultPageDirection) Then
                    BaseDataObject.Remove(PdfName.M)
                Else
                    BaseDataObject(PdfName.M) = ToCode(value)
                End If
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets the scale at which the changes are drawn.</summary>
        '*/
        <PDF(VersionEnum.PDF15)>
        Public Property Scale As Double
            Get
                Dim scaleObject As IPdfNumber = CType(BaseDataObject(PdfName.SS), IPdfNumber)
                If (scaleObject Is Nothing) Then
                    Return DefaultScale
                Else
                    Return scaleObject.RawValue
                End If
            End Get
            Set(ByVal value As Double)
                If (value = DefaultScale) Then
                    BaseDataObject.Remove(PdfName.SS)
                Else
                    BaseDataObject(PdfName.SS) = PdfReal.Get(value)
                End If
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets the transition style.</summary>
        '*/
        Public Property Style As StyleEnum
            Get
                Return ToStyleEnum(CType(Me.BaseDataObject(PdfName.S), PdfName))
            End Get
            Set(ByVal value As StyleEnum)
                If (value = DefaultStyle) Then
                    BaseDataObject.Remove(PdfName.S)
                Else
                    BaseDataObject(PdfName.S) = ToCode(value)
                End If
            End Set
        End Property

#End Region
#End Region
#End Region

    End Class

End Namespace
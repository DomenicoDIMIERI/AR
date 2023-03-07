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
Imports DMD.org.dmdpdf.documents.contents.colorSpaces
Imports DMD.org.dmdpdf.documents.contents.composition
Imports DMD.org.dmdpdf.documents.contents.xObjects
Imports DMD.org.dmdpdf.objects
Imports DMD.org.dmdpdf.util.math.geom

Imports System
Imports System.Collections.Generic
Imports System.Drawing

Namespace DMD.org.dmdpdf.documents.interaction.annotations

    '/**
    '  <summary>Text markup annotation [PDF:1.6:8.4.5].</summary>
    '  <remarks>It displays highlights, underlines, strikeouts, or jagged ("squiggly") underlines in
    '  the text of a document.</remarks>
    '*/
    <PDF(VersionEnum.PDF13)>
    Public NotInheritable Class TextMarkup
        Inherits Annotation

#Region "types"
        '/**
        '  <summary>Markup type [PDF:1.6:8.4.5].</summary>
        '*/
        Public Enum MarkupTypeEnum

            '  /**
            '  <summary>Highlight.</summary>
            '*/
            <PDF(VersionEnum.PDF13)>
            Highlight
            '/**
            '  <summary> Squiggly.</summary>
            '*/
            <PDF(VersionEnum.PDF14)>
            Squiggly
            '/**
            '  <summary> StrikeOut.</summary>
            '*/
            <PDF(VersionEnum.PDF13)>
            StrikeOut
            '/**
            '  <summary> Underline.</summary>
            '*/
            <PDF(VersionEnum.PDF13)>
            Underline
        End Enum

#End Region

#Region "Static"
#Region "fields"

        Private Shared ReadOnly _MarkupTypeEnumCodes As Dictionary(Of MarkupTypeEnum, PdfName)

#End Region

#Region "constructors"

        Shared Sub New()
            _MarkupTypeEnumCodes = New Dictionary(Of MarkupTypeEnum, PdfName)
            _MarkupTypeEnumCodes(MarkupTypeEnum.Highlight) = PdfName.Highlight
            _MarkupTypeEnumCodes(MarkupTypeEnum.Squiggly) = PdfName.Squiggly
            _MarkupTypeEnumCodes(MarkupTypeEnum.StrikeOut) = PdfName.StrikeOut
            _MarkupTypeEnumCodes(MarkupTypeEnum.Underline) = PdfName.Underline
        End Sub

#End Region

#Region "interface"
#Region "private"

        '/**
        '  <summary>Gets the code corresponding to the given value.</summary>
        '*/
        Private Shared Function ToCode(ByVal value As MarkupTypeEnum) As PdfName
            Return _MarkupTypeEnumCodes(value)
        End Function

        '/**
        '  <summary>Gets the markup type corresponding to the given value.</summary>
        '*/
        Private Shared Function ToMarkupTypeEnum(ByVal value As PdfName) As MarkupTypeEnum
            For Each markupType As KeyValuePair(Of MarkupTypeEnum, PdfName) In _MarkupTypeEnumCodes
                If (markupType.Value.Equals(value)) Then
                    Return markupType.Key
                End If
            Next
            Throw New Exception("Invalid markup type.")
        End Function

#End Region
#End Region
#End Region

#Region "Static"
#Region "fields"

        Private Shared ReadOnly HighlightExtGStateName As PdfName = New PdfName("highlight")

#End Region

#Region "Interface"

        Private Shared Function GetMarkupBoxMargin(ByVal boxHeight As Single) As Single
            Return boxHeight * 0.25F
        End Function

#End Region
#End Region

#Region "dynamic"
#Region "constructors"
        '/**
        '  <summary>Creates a new text markup on the specified page, making it printable by default.
        '  </summary>
        '  <param name="page">Page to annotate.</param>
        '  <param name="text">Annotation text.</param>
        '  <param name="markupType">Markup type.</param>
        '  <param name="markupBox">Quadrilateral encompassing a word or group of contiguous words in the
        '  text underlying the annotation.</param>
        '*/
        Public Sub New(ByVal page As Page, ByVal text As String, ByVal markupType As MarkupTypeEnum, ByVal markupBox As Quad)
            Me.New(page, text, markupType, New List(Of Quad)({markupBox}))
        End Sub

        '/**
        '  <summary>Creates a New text markup On the specified page, making it printable by Default.
        '  </summary>
        '  <param name = "page" > page To annotate.</param>
        '  <param name = "text" > Annotation text.</param>
        '  <param name = "markupType" > Markup type.</param>
        '  <param name = "markupBoxes" > Quadrilaterals encompassing a word Or group Of contiguous words In
        '  the text underlying the annotation.</param>
        '*/
        Public Sub New(ByVal page As Page, ByVal text As String, ByVal markupType As MarkupTypeEnum, ByVal markupBoxes As IList(Of Quad))
            MyBase.New(page, ToCode(markupType), markupBoxes(0).GetBounds(), text)
            Me.MarkupType = markupType
            Me.MarkupBoxes = markupBoxes
            Me.Printable = True
        End Sub

        Friend Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary>Gets/Sets the quadrilaterals encompassing a word Or group Of contiguous words In the
        '  text underlying the annotation.</summary>
        '*/
        Public Property MarkupBoxes As IList(Of Quad)
            Get
                Dim _markupBoxes As IList(Of Quad) = New List(Of Quad)()
                Dim quadPointsObject As PdfArray = CType(BaseDataObject(PdfName.QuadPoints), PdfArray)
                If (quadPointsObject IsNot Nothing) Then
                    Dim pageHeight As Single = Me.Page.Box.Height
                    Dim length As Integer = quadPointsObject.Count
                    For index As Integer = 0 To length - 1 Step 8
                        '/*
                        '  NOTE: Despite the spec prescription, Point 3 and Point 4 MUST be inverted.
                        '*/
                        _markupBoxes.Add(
                                      New Quad(
                                        New PointF((CType(quadPointsObject(index), IPdfNumber)).FloatValue, pageHeight - (CType(quadPointsObject(index + 1), IPdfNumber)).FloatValue),
                                        New PointF((CType(quadPointsObject(index + 2), IPdfNumber)).FloatValue, pageHeight - (CType(quadPointsObject(index + 3), IPdfNumber)).FloatValue),
                                        New PointF((CType(quadPointsObject(index + 6), IPdfNumber)).FloatValue, pageHeight - (CType(quadPointsObject(index + 7), IPdfNumber)).FloatValue),
                                        New PointF((CType(quadPointsObject(index + 4), IPdfNumber)).FloatValue, pageHeight - (CType(quadPointsObject(index + 5), IPdfNumber)).FloatValue)
                                        )
                                      )
                    Next
                End If
                Return _markupBoxes
            End Get
            Set(ByVal value As IList(Of Quad))
                Dim quadPointsObject As PdfArray = New PdfArray()
                Dim pageHeight As Single = Me.Page.Box.Height
                Dim box As RectangleF = RectangleF.Empty
                For Each markupBox As Quad In value
                    '/*
                    '  NOTE: Despite the spec prescription, Point 3 And Point 4 MUST be inverted.
                    '*/
                    Dim markupBoxPoints As PointF() = markupBox.Points
                    quadPointsObject.Add(PdfReal.Get(markupBoxPoints(0).X)) ' x1.
                    quadPointsObject.Add(PdfReal.Get(pageHeight - markupBoxPoints(0).Y)) ' y1.
                    quadPointsObject.Add(PdfReal.Get(markupBoxPoints(1).X)) ' x2.
                    quadPointsObject.Add(PdfReal.Get(pageHeight - markupBoxPoints(1).Y)) ' y2.
                    quadPointsObject.Add(PdfReal.Get(markupBoxPoints(3).X)) ' x4.
                    quadPointsObject.Add(PdfReal.Get(pageHeight - markupBoxPoints(3).Y)) ' y4.
                    quadPointsObject.Add(PdfReal.Get(markupBoxPoints(2).X)) ' x3.
                    quadPointsObject.Add(PdfReal.Get(pageHeight - markupBoxPoints(2).Y)) ' y3.
                    If (box.IsEmpty) Then
                        box = markupBox.GetBounds()
                    Else
                        box = RectangleF.Union(box, markupBox.GetBounds())
                    End If
                Next
                BaseDataObject(PdfName.QuadPoints) = quadPointsObject
                '/*
                '  NOTE:     Box width Is expanded to make room for end decorations (e.g. rounded highlight caps).
                '*/
                Dim markupBoxMargin As Single = GetMarkupBoxMargin(box.Height)
                box.X -= markupBoxMargin
                box.Width += markupBoxMargin * 2
                Me.Box = box
                RefreshAppearance()
            End Set
        End Property

        '/**
        '  <summary> Gets/Sets the markup type.</summary>
        '*/
        Public Property MarkupType As MarkupTypeEnum
            Get
                Return ToMarkupTypeEnum(CType(BaseDataObject(PdfName.Subtype), PdfName))
            End Get
            Set(ByVal value As MarkupTypeEnum)
                BaseDataObject(PdfName.Subtype) = ToCode(value)
                Select Case (value)
                    Case MarkupTypeEnum.Highlight : Me.Color = New DeviceRGBColor(1, 1, 0)
                    Case MarkupTypeEnum.Squiggly : Me.Color = New DeviceRGBColor(1, 0, 0)
                    Case Else : Me.Color = New DeviceRGBColor(0, 0, 0)
                End Select
                RefreshAppearance()
            End Set
        End Property

#End Region

#Region "private"
        '/*
        '  TODO:         refresh should happen just before serialization, On document Event (e.g. OnWrite())
        '*/
        Private Sub RefreshAppearance()
            Dim normalAppearance As FormXObject
            Dim box As RectangleF = org.dmdpdf.objects.Rectangle.Wrap(BaseDataObject(PdfName.Rect)).ToRectangleF()
            Dim normalAppearances As AppearanceStates = Appearance.Normal
            normalAppearance = normalAppearances(Nothing)
            If (normalAppearance IsNot Nothing) Then
                normalAppearance.Box = box
                normalAppearance.BaseDataObject.Body.SetLength(0)
            Else
                normalAppearance = New FormXObject(Document, box)
                normalAppearances(Nothing) = normalAppearance
            End If

            Dim composer As PrimitiveComposer = New PrimitiveComposer(normalAppearance)
            Dim yOffset As Single = box.Height - Me.Page.Box.Height
            Dim markupType As MarkupTypeEnum = markupType
            Select Case (markupType)
                Case MarkupTypeEnum.Highlight
                    Dim defaultExtGState As ExtGState
                    Dim extGStates As ExtGStateResources = normalAppearance.Resources.ExtGStates
                    defaultExtGState = extGStates(HighlightExtGStateName)
                    If (defaultExtGState Is Nothing) Then
                        If (extGStates.Count > 0) Then
                            extGStates.Clear()
                        End If
                        defaultExtGState = New ExtGState(Document)
                        extGStates(HighlightExtGStateName) = defaultExtGState
                        defaultExtGState.AlphaShape = False
                        defaultExtGState.BlendMode = New List(Of BlendModeEnum)(New BlendModeEnum() {BlendModeEnum.Multiply})
                    End If

                    composer.ApplyState(defaultExtGState)
                    composer.SetFillColor(Color)

                    For Each markupBox As Quad In MarkupBoxes
                        Dim points As PointF() = markupBox.Points
                        Dim markupBoxHeight As Single = points(3).Y - points(0).Y
                        Dim markupBoxMargin As Single = GetMarkupBoxMargin(markupBoxHeight)
                        composer.DrawCurve(
                                      New PointF(points(3).X, points(3).Y + yOffset),
                                      New PointF(points(0).X, points(0).Y + yOffset),
                                      New PointF(points(3).X - markupBoxMargin, points(3).Y - markupBoxMargin + yOffset),
                                      New PointF(points(0).X - markupBoxMargin, points(0).Y + markupBoxMargin + yOffset)
                                      )
                        composer.DrawLine(New PointF(points(1).X, points(1).Y + yOffset))
                        composer.DrawCurve(
                                  New PointF(points(2).X, points(2).Y + yOffset),
                                  New PointF(points(1).X + markupBoxMargin, points(1).Y + markupBoxMargin + yOffset),
                                  New PointF(points(2).X + markupBoxMargin, points(2).Y - markupBoxMargin + yOffset)
                                  )
                        composer.Fill()
                    Next

            'break;
                Case MarkupTypeEnum.Squiggly
                    composer.SetStrokeColor(Color)
                    composer.SetLineCap(LineCapEnum.Round)
                    composer.SetLineJoin(LineJoinEnum.Round)
                    For Each markupBox As Quad In MarkupBoxes
                        Dim points As PointF() = markupBox.Points
                        Dim markupBoxHeight As Single = points(3).Y - points(0).Y
                        Dim lineWidth As Single = markupBoxHeight * 0.02F
                        Dim [step] As Single = markupBoxHeight * 0.125F
                        Dim boxXOffset As Single = points(3).X
                        Dim boxYOffset As Single = points(3).Y + yOffset - lineWidth
                        Dim phase As Boolean = False
                        composer.SetLineWidth(lineWidth)
                        Dim x As Single = 0
                        Dim xEnd As Single = points(2).X - boxXOffset
                        While (x < xEnd OrElse Not phase)
                            Dim Point As PointF
                            If (phase) Then
                                Point = New PointF(x + boxXOffset, -[step] + boxYOffset)
                            Else
                                Point = New PointF(x + boxXOffset, 0 + boxYOffset)
                            End If
                            If (x = 0) Then
                                composer.StartPath(Point)
                            Else
                                composer.DrawLine(Point)
                            End If
                            phase = Not phase
                            x += [step]
                        End While
                    Next
                    composer.Stroke()
                Case MarkupTypeEnum.StrikeOut,
               MarkupTypeEnum.Underline

                    composer.SetStrokeColor(Color)
                    Dim lineYRatio As Single = 0
                    Select Case (markupType)
                        Case MarkupTypeEnum.StrikeOut : lineYRatio = 0.575F
                        Case MarkupTypeEnum.Underline : lineYRatio = 0.85F
                        Case Else : Throw New NotImplementedException()
                    End Select
                    For Each markupBox As Quad In MarkupBoxes
                        Dim points As PointF() = markupBox.Points
                        Dim markupBoxHeight As Single = points(3).Y - points(0).Y
                        Dim boxYOffset As Single = markupBoxHeight * lineYRatio + yOffset
                        composer.SetLineWidth(markupBoxHeight * 0.065)
                        composer.DrawLine(
                                  New PointF(points(3).X, points(0).Y + boxYOffset),
                                  New PointF(points(2).X, points(1).Y + boxYOffset)
                                  )
                    Next
                    composer.Stroke()

                Case Else
                    Throw New NotImplementedException()
            End Select

            composer.Flush()
        End Sub

#End Region
#End Region
#End Region

    End Class

End Namespace
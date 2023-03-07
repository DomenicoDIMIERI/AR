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

'  You should have received a copy of the GNU Lesser General Public License along with Me
'  Program (see README files); if not, go to the GNU website (http://www.gnu.org/licenses/).

'  Redistribution and use, with or without modification, are permitted provided that such
'  redistributions retain the above copyright notice, license and disclaimer, along with
'  Me list of conditions.
'*/

Imports DMD.org.dmdpdf.bytes
Imports DMD.org.dmdpdf.documents.contents
Imports DMD.org.dmdpdf.util

Imports System.Collections.Generic
Imports System.Drawing
Imports System.Drawing.Drawing2D

Namespace DMD.org.dmdpdf.documents.contents.objects

    '/**
    '  <summary>Path-painting operation [PDF:1.6:4.4.2].</summary>
    '*/
    <PDF(VersionEnum.PDF10)>
    Public NotInheritable Class PaintPath
        Inherits Operation

#Region "Static"
#Region "fields"

        Public Const CloseFillStrokeEvenOddOperatorKeyword As String = "b*"
        Public Const CloseFillStrokeOperatorKeyword As String = "b"
        Public Const CloseStrokeOperatorKeyword As String = "s"
        Public Const EndPathNoOpOperatorKeyword As String = "n"
        Public Const FillEvenOddOperatorKeyword As String = "f*"
        Public Const FillObsoleteOperatorKeyword As String = "F"
        Public Const FillOperatorKeyword As String = "f"
        Public Const FillStrokeEvenOddOperatorKeyword As String = "B*"
        Public Const FillStrokeOperatorKeyword As String = "B"
        Public Const StrokeOperatorKeyword As String = "S"

        '/**
        '  <summary>'Close, fill, and then stroke the path, using the nonzero winding number rule to determine
        '  the region to fill' operation.</summary>
        '*/
        Public Shared ReadOnly CloseFillStroke As PaintPath = New PaintPath(CloseFillStrokeOperatorKeyword, True, True, True, WindModeEnum.NonZero)

        '    /**
        '  <summary>'Close, fill, and then stroke the path, using the even-odd rule to determine the region
        '  To fill' operation.</summary>
        '*/
        Public Shared ReadOnly CloseFillStrokeEvenOdd As PaintPath = New PaintPath(CloseFillStrokeEvenOddOperatorKeyword, True, True, True, WindModeEnum.EvenOdd)
        '/**
        '  <summary>'Close and stroke the path' operation.</summary>
        '*/
        Public Shared ReadOnly CloseStroke As PaintPath = New PaintPath(CloseStrokeOperatorKeyword, True, True, False, Nothing)
        '/**
        '  <summary>'End the path object without filling or stroking it' operation.</summary>
        '*/
        Public Shared ReadOnly EndPathNoOp As PaintPath = New PaintPath(EndPathNoOpOperatorKeyword, False, False, False, Nothing)
        '/**
        '  <summary>'Fill the path, using the nonzero winding number rule to determine the region to fill' operation.</summary>
        '*/
        Public Shared ReadOnly Fill As PaintPath = New PaintPath(FillOperatorKeyword, False, False, True, WindModeEnum.NonZero)
        '/**
        '  <summary>'Fill the path, using the even-odd rule to determine the region to fill' operation.</summary>
        '*/
        Public Shared ReadOnly FillEvenOdd As PaintPath = New PaintPath(FillEvenOddOperatorKeyword, False, False, True, WindModeEnum.EvenOdd)
        '/**
        '  <summary>'Fill and then stroke the path, using the nonzero winding number rule to determine the region to
        '    fill ' operation.</summary>
        '*/
        Public Shared ReadOnly FillStroke As PaintPath = New PaintPath(FillStrokeOperatorKeyword, False, True, True, WindModeEnum.NonZero)
        '/**
        '  <summary>'Fill and then stroke the path, using the even-odd rule to determine the region to fill' operation.</summary>
        '*/
        Public Shared ReadOnly FillStrokeEvenOdd As PaintPath = New PaintPath(FillStrokeEvenOddOperatorKeyword, False, True, True, WindModeEnum.EvenOdd)
        '/**
        '  <summary>'Stroke the path' operation.</summary>
        '*/
        Public Shared ReadOnly Stroke As PaintPath = New PaintPath(StrokeOperatorKeyword, False, True, False, Nothing)
#End Region

#Region "Interface"
#Region "Private"

        Private Shared Function GetStroke(ByVal state As ContentScanner.GraphicsState) As Pen
            Dim stroke As Pen = New Pen(
                                state.StrokeColorSpace.GetPaint(state.StrokeColor),
                                CSng(state.LineWidth)
                            )
            '{
            Dim LineCap As LineCap = state.LineCap.ToGdi()
            stroke.SetLineCap(LineCap, LineCap, LineCap.ToDashCap())
            stroke.LineJoin = state.LineJoin.ToGdi()
            stroke.MiterLimit = CSng(state.MiterLimit)

            Dim LineDash As LineDash = state.LineDash
            Dim dashArray As Double() = LineDash.DashArray
            If (dashArray IsNot Nothing AndAlso dashArray.Length > 0) Then
                stroke.DashPattern = ConvertUtils.ToFloatArray(dashArray)
                stroke.DashOffset = CSng(LineDash.DashPhase)
            End If
            '}
            Return stroke
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "fields"

        Private ReadOnly _closed As Boolean
        Private ReadOnly _filled As Boolean
        Private ReadOnly _fillMode As WindModeEnum
        Private ReadOnly _stroked As Boolean

#End Region

#Region "constructors"

        Private Sub New(
                       ByVal operator_ As String,
                       ByVal closed As Boolean,
                       ByVal stroked As Boolean,
                       ByVal filled As Boolean,
                       ByVal fillMode As WindModeEnum?
                       )
            MyBase.New(operator_)
            Me._closed = closed
            Me._stroked = stroked
            Me._filled = filled
            If (fillMode.HasValue) Then
                Me._fillMode = fillMode.Value
            Else
                Me._fillMode = WindModeEnum.EvenOdd
            End If

        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary> Gets the filling rule.</summary>
        '*/
        Public ReadOnly Property FillMode As WindModeEnum
            Get
                Return Me._fillMode
            End Get
        End Property

        '/**
        '  <summary> Gets whether the current path has To be closed.</summary>
        '*/
        Public ReadOnly Property Closed As Boolean
            Get
                Return Me._closed
            End Get
        End Property

        '/**
        '  <summary> Gets whether the current path has To be filled.</summary>
        '*/
        Public ReadOnly Property Filled As Boolean
            Get
                Return Me._filled
            End Get
        End Property

        Public Overrides Sub Scan(ByVal state As ContentScanner.GraphicsState)
            Dim scanner As ContentScanner = state.Scanner
            Dim pathObject As GraphicsPath = scanner.RenderObject
            If (pathObject IsNot Nothing) Then
                Dim context As Graphics = scanner.RenderContext
                If (_closed) Then
                    pathObject.CloseFigure()
                End If
                If (_filled) Then
                    pathObject.FillMode = _fillMode.ToGdi()
                    context.FillPath(state.FillColorSpace.GetPaint(state.FillColor), pathObject)
                End If
                If (_stroked) Then
                    context.DrawPath(GetStroke(state), pathObject)
                End If
            End If
        End Sub

        '/**
        '  <summary> Gets whether the current path has To be stroked.</summary>
        '*/
        Public ReadOnly Property Stroked As Boolean
            Get
                Return Me._stroked
            End Get
        End Property

#End Region
#End Region
#End Region
    End Class

End Namespace
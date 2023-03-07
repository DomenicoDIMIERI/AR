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
Imports DMD.org.dmdpdf.objects

Imports System.Collections.Generic
Imports System.Drawing
Imports System.Drawing.Drawing2D

Namespace DMD.org.dmdpdf.documents.contents.objects

    '/**
    '  <summary>'Append a cubic Bezier curve to the current path' operation [PDF:1.6:4.4.1].</summary>
    '  <remarks>Such curves are defined by four points:
    '  the two endpoints (the current point and the final point)
    '  and two control points (the first control point, associated to the current point,
    '  and the second control point, associated to the final point).</remarks>
    '*/
    <PDF(VersionEnum.PDF10)>
    Public NotInheritable Class DrawCurve
        Inherits Operation

#Region "static"
#Region "fields"

        '/**
        '  <summary>Specifies only the second control point
        '  (the first control point coincides with the initial point of the curve).</summary>
        '*/
        Public Shared ReadOnly FinalOperatorKeyword As String = "v"
        '/**
        '  <summary>Specifies both control points explicitly.</summary>
        '*/
        Public Shared ReadOnly FullOperatorKeyword As String = "c"
        '/**
        '  <summary> Specifies only the first control point
        '  (the second control point coincides with the final point of the curve).</summary>
        '*/
        Public Shared ReadOnly InitialOperatorKeyword As String = "y"

#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        '/**
        '  <summary> Creates a fully-explicit curve.</summary>
        '  <param name = "point" > Final endpoint.</param>
        '  <param name = "control1" > First control point.</param>
        '  <param name = "control2" > Second() control point.</param>
        '*/
        Public Sub New(
                    ByVal point As PointF,
                    ByVal control1 As PointF,
                    ByVal control2 As PointF
                    )
            Me.New(
                point.X,
                point.Y,
                control1.X,
                control1.Y,
                control2.X,
                control2.Y
                )
        End Sub

        '/**
        '  <summary> Creates a fully-explicit curve.</summary>
        '*/
        Public Sub New(
                ByVal pointX As Double,
                ByVal pointY As Double,
                ByVal control1X As Double,
                ByVal control1Y As Double,
                ByVal control2X As Double,
                ByVal control2Y As Double
                )
            MyBase.New(
                    FullOperatorKeyword,
                    New List(Of PdfDirectObject)(
                        New PdfDirectObject() {
                            PdfReal.Get(control1X),
                            PdfReal.Get(control1Y),
                            PdfReal.Get(control2X),
                            PdfReal.Get(control2Y),
                            PdfReal.Get(pointX),
                            PdfReal.Get(pointY)
                          }
                        )
                    )
        End Sub

        Private Shared Function iif(Of T)(cond As Boolean, ByVal op1 As T, ByVal op2 As T) As T
            If (cond) Then
                Return op1
            Else
                Return op2
            End If
        End Function
        '/**
        '  <summary> Creates a partially-explicit curve.</summary>
        '  <param name = "point" > Final endpoint.</param>
        '  <param name = "control" > Explicit control point.</param>
        '  <param name = "operator" >Operator (either <code>InitialOperator</code> Or <code>FinalOperator</code>).
        '  It defines how To interpret the <code>control</code> parameter.</param>
        '*/
        Public Sub New(
                    ByVal point As PointF,
                    ByVal control As PointF,
                    ByVal operator_ As String
                )
            MyBase.New(
                    iif(operator_.Equals(InitialOperatorKeyword), InitialOperatorKeyword, FinalOperatorKeyword),
                    New List(Of PdfDirectObject)(
                                    New PdfDirectObject() {
                                    PdfReal.Get(control.X),
                                    PdfReal.Get(control.Y),
                                    PdfReal.Get(point.X),
                                    PdfReal.Get(point.Y)
                                  }
                        )
                    )
        End Sub


        Public Sub New(ByVal operator_ As String, ByVal operands As IList(Of PdfDirectObject))
            MyBase.New(operator_, operands)
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary> Gets/Sets the first control point.</summary>
        '*/
        Public Property Control1 As PointF?
            Get
                If (Me._operator_.Equals(FinalOperatorKeyword)) Then
                    Return Nothing
                Else
                    Return New PointF(
                            CType(Me._operands(0), IPdfNumber).FloatValue,
                            CType(Me._operands(1), IPdfNumber).FloatValue
                            )
                End If
            End Get
            Set(ByVal value As PointF?)
                If (Me._operator_.Equals(FinalOperatorKeyword)) Then
                    Me._operator_ = FullOperatorKeyword
                    Me._operands.Insert(0, PdfReal.Get(value.Value.X))
                    Me._operands.Insert(1, PdfReal.Get(value.Value.Y))
                Else
                    Me._operands(0) = PdfReal.Get(value.Value.X)
                    Me._operands(1) = PdfReal.Get(value.Value.Y)
                End If
            End Set
        End Property

        '/**
        '  <summary> Gets/Sets the second control point.</summary>
        '*/
        Public Property Control2 As PointF?
            Get
                If (Me._operator_.Equals(FinalOperatorKeyword)) Then
                    Return New PointF(
                             CType(Me._operands(0), IPdfNumber).FloatValue,
                             CType(Me._operands(1), IPdfNumber).FloatValue
                                )
                Else
                    Return New PointF(
                                CType(Me._operands(2), IPdfNumber).FloatValue,
                                CType(Me._operands(3), IPdfNumber).FloatValue
                            )
                End If
            End Get
            Set(ByVal value As PointF?)
                If (Me._operator_.Equals(FinalOperatorKeyword)) Then
                    Me._operands(0) = PdfReal.Get(value.Value.X)
                    Me._operands(1) = PdfReal.Get(value.Value.Y)
                Else
                    Me._operands(2) = PdfReal.Get(value.Value.X)
                    Me._operands(3) = PdfReal.Get(value.Value.Y)
                End If
            End Set
        End Property

        '/**
        '  <summary> Gets/Sets the final endpoint.</summary>
        '*/
        Public Property Point As PointF
            Get
                If (Me._operator_.Equals(FullOperatorKeyword)) Then
                    Return New PointF(
                        CType(Me._operands(4), IPdfNumber).FloatValue,
                        CType(Me._operands(5), IPdfNumber).FloatValue
                        )
                Else
                    Return New PointF(
                        CType(Me._operands(2), IPdfNumber).FloatValue,
                        CType(Me._operands(3), IPdfNumber).FloatValue
                        )
                End If
            End Get
            Set(ByVal value As PointF)
                If (Me._operator_.Equals(FullOperatorKeyword)) Then
                    Me._operands(4) = PdfReal.Get(value.X)
                    Me._operands(5) = PdfReal.Get(value.Y)
                Else
                    Me._operands(2) = PdfReal.Get(value.X)
                    Me._operands(3) = PdfReal.Get(value.Y)
                End If
            End Set
        End Property

        Public Overrides Sub Scan(ByVal state As ContentScanner.GraphicsState)
            Dim pathObject As GraphicsPath = state.Scanner.RenderObject
            If (pathObject IsNot Nothing) Then
                Dim controlPoint1 As PointF = iif(Control1.HasValue, Control1.Value, pathObject.GetLastPoint())
                Dim finalPoint As PointF = Me.Point
                Dim controlPoint2 As PointF = iif(Control2.HasValue, Control2.Value, finalPoint)
                pathObject.AddBezier(
                                pathObject.GetLastPoint(),
                                controlPoint1,
                                controlPoint2,
                                finalPoint
                                )
            End If
        End Sub

#End Region
#End Region
#End Region

    End Class
End Namespace
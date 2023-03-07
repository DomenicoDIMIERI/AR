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
    '  <summary>'Begin a new subpath by moving the current point' operation [PDF:1.6:4.4.1].</summary>
    '*/
    <PDF(VersionEnum.PDF10)>
    Public NotInheritable Class BeginSubpath
        Inherits Operation

#Region "Static"
#Region "fields"

        Public Shared ReadOnly OperatorKeyword As String = "m"

#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        '/**
        '  <param name="point">Current point.</param>
        '*/
        Public Sub New(ByVal point As PointF)
            Me.New(point.X, point.Y)
        End Sub

        '/**
        '  <param name="pointX">Current point X.</param>
        '  <param name="pointY">Current point Y.</param>
        '*/
        Public Sub New(ByVal pointX As Double, ByVal pointY As Double)
            MyBase.New(
                        OperatorKeyword,
                        New List(Of PdfDirectObject)(
                                New PdfDirectObject() {
                                    PdfReal.Get(pointX),
                                    PdfReal.Get(pointY)
                                    }
                                )
                        )
        End Sub

        Public Sub New(ByVal operands As IList(Of PdfDirectObject))
            MyBase.New(OperatorKeyword, operands)
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary>Gets/Sets the current point.</summary>
        '*/
        Public Property Point As PointF
            Get
                Return New PointF(
                          CType(Me._operands(0), IPdfNumber).FloatValue,
                          CType(Me._operands(1), IPdfNumber).FloatValue
                          )
            End Get
            Set(ByVal value As PointF)
                Me._operands(0) = PdfReal.Get(value.X)
                Me._operands(1) = PdfReal.Get(value.Y)
            End Set
        End Property

        Public Overrides Sub Scan(ByVal state As ContentScanner.GraphicsState)
            Dim pathObject As GraphicsPath = CType(state.Scanner.RenderObject, GraphicsPath)
            If (pathObject IsNot Nothing) Then
                Dim point As PointF = Me.Point
                pathObject.AddLine(point, point)
            End If
        End Sub

#End Region
#End Region
#End Region

    End Class

End Namespace
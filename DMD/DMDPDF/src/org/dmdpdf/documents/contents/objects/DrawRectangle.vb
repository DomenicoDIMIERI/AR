'/*
'  Copyright 2007-2012 Stefano Chizzolini. http://www.dmdpdf.org

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
    '  <summary>'Append a rectangle to the current path as a complete subpath' operation
    '  [PDF:1.6:4.4.1].</summary>
    '*/
    <PDF(VersionEnum.PDF10)>
    Public NotInheritable Class DrawRectangle
        Inherits Operation

#Region "Static"
#Region "fields"

        Public Shared ReadOnly OperatorKeyword As String = "re"

#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        Public Sub New(
                      ByVal x As Double,
                      ByVal y As Double,
                      ByVal width As Double,
                      ByVal height As Double
                    )
            MyBase.New(
                    OperatorKeyword,
                    New List(Of PdfDirectObject)(
                                New PdfDirectObject() {
                                    PdfReal.Get(x),
                                    PdfReal.Get(y),
                                    PdfReal.Get(width),
                                    PdfReal.Get(height)
                                    }
                            )
                        )
        End Sub

        Public Sub New(ByVal operands As IList(Of PdfDirectObject))
            MyBase.New(OperatorKeyword, operands)
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public Property Height As Double
            Get
                Return CType(Me._operands(3), IPdfNumber).RawValue
            End Get
            Set(ByVal value As Double)
                Me._operands(3) = PdfReal.Get(value)
            End Set
        End Property

        Public Overrides Sub Scan(ByVal state As ContentScanner.GraphicsState)
            Dim pathObject As GraphicsPath = state.Scanner.RenderObject
            If (pathObject IsNot Nothing) Then
                Dim _x As Double = Me.X
                Dim _y As Double = Me.Y
                Dim _width As Double = Me.Width
                Dim _height As Double = Me.Height
                pathObject.AddRectangle(New RectangleF(CSng(_x), CSng(_y), CSng(_width), CSng(_height)))
                pathObject.CloseFigure()
            End If
        End Sub

        Public Property Width As Double
            Get
                Return CType(Me._operands(2), IPdfNumber).RawValue
            End Get
            Set(ByVal value As Double)
                Me._operands(2) = PdfReal.Get(value)
            End Set
        End Property

        Public Property X As Double
            Get
                Return CType(Me._operands(0), IPdfNumber).RawValue
            End Get
            Set(ByVal value As Double)
                Me._operands(0) = PdfReal.Get(value)
            End Set
        End Property

        Public Property Y As Double
            Get
                Return CType(Me._operands(1), IPdfNumber).RawValue
            End Get
            Set(ByVal value As Double)
                Me._operands(1) = PdfReal.Get(value)
            End Set
        End Property

#End Region
#End Region
#End Region

    End Class
End Namespace
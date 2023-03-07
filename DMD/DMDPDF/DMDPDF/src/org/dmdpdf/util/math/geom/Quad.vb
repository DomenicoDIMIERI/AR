'/*
'  Copyright 2011 Stefano Chizzolini. http://www.dmdpdf.org

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

Imports System
Imports System.Drawing
Imports System.Drawing.Drawing2D

Namespace DMD.org.dmdpdf.util.math.geom

    '/**
    '  <summary>Quadrilateral shape.</summary>
    '*/
    Public Class Quad

#Region "Static"
#Region "Interface"
#Region "Public"

        Public Shared Function [Get](ByVal rectangle As RectangleF) As Quad
            Return New Quad(GetPoints(rectangle))
        End Function


        Public Shared Function GetPoints(ByVal rectangle As RectangleF) As PointF()
            Dim _points As PointF() = New PointF(4 - 1) {}
            _points(0) = New PointF(rectangle.Left, rectangle.Top)
            _points(1) = New PointF(rectangle.Right, rectangle.Top)
            _points(2) = New PointF(rectangle.Right, rectangle.Bottom)
            _points(3) = New PointF(rectangle.Left, rectangle.Bottom)
            Return _points
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "fields"

        Private _points As PointF()

        Private _path As GraphicsPath

#End Region

#Region "constructors"

        Public Sub New(ParamArray points As PointF())
            Me.Points = points
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public Function Contains(ByVal point As PointF) As Boolean
            Return Me.Path.IsVisible(point)
        End Function

        Public Function Contains(ByVal x As Single, ByVal y As Single) As Boolean
            Return Me.Path.IsVisible(x, y)
        End Function


        Public Function GetBounds() As RectangleF
            Return Me.Path.GetBounds()
        End Function

        Public Function GetPathIterator() As GraphicsPathIterator
            Return New GraphicsPathIterator(Me.Path)
        End Function

        Public Property Points As PointF()
            Get
                Return Me._points
            End Get
            Set(value As PointF())
                If (value.Length <> 4) Then Throw New ArgumentException("Cardinality MUST be 4.", "_points")
                Me._points = value
                Me._path = Nothing
            End Set
        End Property

#End Region

#Region "Private"

        Private ReadOnly Property Path As GraphicsPath
            Get
                If (Me._path Is Nothing) Then
                    'TODO: Memory Leak?
                    Me._path = New GraphicsPath(FillMode.Alternate)
                    Me._path.AddPolygon(Me._points)
                End If
                Return Me._path
            End Get
        End Property

#End Region
#End Region
#End Region

    End Class

End Namespace


'/*
'  Copyright 2010-2012 Stefano Chizzolini. http://www.dmdpdf.org

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

Imports System
Imports System.Drawing

Namespace DMD.org.dmdpdf.objects

    '/**
    '  <summary>PDF rectangle object [PDF:1.6:3.8.4].</summary>
    '  <remarks>
    '    <para>Rectangles are described by two diagonally-opposite corners. Corner pairs which don't
    '    respect the canonical form (lower-left and upper-right) are automatically normalized to
    '    provide a consistent representation.</para>
    '    <para>Coordinates are expressed within the PDF coordinate space (lower-left origin and
    '    positively-oriented axes).</para>
    '  </remarks>
    '*/
    Public NotInheritable Class Rectangle
        Inherits PdfObjectWrapper(Of PdfArray)

#Region "Static"
#Region "Interface"
#Region "Public"

        Public Shared Function Wrap(ByVal baseObject As PdfDirectObject) As Rectangle
            If (baseObject IsNot Nothing) Then
                Return New Rectangle(baseObject)
            Else
                Return Nothing
            End If
        End Function

#End Region

#Region "Private"

        Private Shared Function Normalize(ByVal rectangle As PdfArray) As PdfArray
            If (rectangle(0).CompareTo(rectangle(2)) > 0) Then
                Dim leftCoordinate As PdfDirectObject = rectangle(2)
                rectangle(2) = rectangle(0)
                rectangle(0) = leftCoordinate
            End If
            If (rectangle(1).CompareTo(rectangle(3)) > 0) Then
                Dim bottomCoordinate As PdfDirectObject = rectangle(3)
                rectangle(3) = rectangle(1)
                rectangle(1) = bottomCoordinate
            End If
            Return Rectangle
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        Public Sub New(ByVal rectangle As RectangleF)
            Me.New(Rectangle.Left, Rectangle.Bottom, Rectangle.Width, Rectangle.Height)
        End Sub

        Public Sub New(ByVal lowerLeft As PointF, ByVal upperRight As PointF)
            Me.New(lowerLeft.X, upperRight.Y, upperRight.X - lowerLeft.X, upperRight.Y - lowerLeft.Y)
        End Sub

        Public Sub New(ByVal left As Double, ByVal top As Double, ByVal width As Double, ByVal height As Double)
            Me.New(
                    New PdfArray(
                                New PdfDirectObject() {
                                          PdfReal.Get(left), ' Left (X).
                                          PdfReal.Get(top - height), ' Bottom(Y).
                                          PdfReal.Get(left + width), ' Right.
                                          PdfReal.Get(top) ' top.
                                        }
                                )
                    )
        End Sub


        Private Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(Normalize(CType(baseObject.Resolve(), PdfArray)))
        End Sub

#End Region

#Region "Interface"
#Region "Public"
        Public Property Bottom As Double
            Get
                Return CType(Me.BaseDataObject(1), IPdfNumber).RawValue
            End Get
            Set(ByVal value As Double)
                Me.BaseDataObject(1) = PdfReal.Get(value)
            End Set
        End Property

        Public Property Height As Double
            Get
                Return (Me.Top - Me.Bottom)
            End Get
            Set(ByVal value As Double)
                Me.Bottom = Me.Top - value
            End Set
        End Property


        Public Property Left As Double
            Get
                Return CType(BaseDataObject(0), IPdfNumber).RawValue
            End Get
            Set(ByVal value As Double)
                Me.BaseDataObject(0) = PdfReal.Get(value)
            End Set
        End Property

        Public Property Right As Double
            Get
                Return CType(BaseDataObject(2), IPdfNumber).RawValue
            End Get
            Set(ByVal value As Double)
                Me.BaseDataObject(2) = PdfReal.Get(value)
            End Set
        End Property

        Public Property Top As Double
            Get
                Return CType(BaseDataObject(3), IPdfNumber).RawValue
            End Get
            Set(ByVal value As Double)
                Me.BaseDataObject(3) = PdfReal.Get(value)
            End Set
        End Property

        Public Function ToRectangleF() As RectangleF
            Return New RectangleF(CSng(Me.X), CSng(Me.Y), CSng(Me.Width), CSng(Me.Height))
        End Function

        Public Property Width As Double
            Get
                Return Me.Right - Me.Left
            End Get
            Set(ByVal value As Double)
                Me.Right = Me.Left + value
            End Set
        End Property

        Public Property X As Double
            Get
                Return Me.Left
            End Get
            Set(ByVal value As Double)
                Me.Left = value
            End Set
        End Property

        Public Property Y As Double
            Get
                Return Me.Bottom
            End Get
            Set(ByVal value As Double)
                Me.Bottom = value
            End Set
        End Property

#End Region
#End Region
#End Region

    End Class

End Namespace


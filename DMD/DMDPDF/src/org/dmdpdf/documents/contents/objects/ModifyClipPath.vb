'/*
'  Copyright 2008-2010 Stefano Chizzolini. http://www.dmdpdf.org

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

Imports System.Drawing
Imports System.Drawing.Drawing2D

Namespace DMD.org.dmdpdf.documents.contents.objects

    '/**
    '  <summary>Clipping path operation [PDF:1.6:4.4.2].</summary>
    '*/
    <PDF(VersionEnum.PDF10)>
    Public NotInheritable Class ModifyClipPath
        Inherits Operation

#Region "Static"
#Region "fields"
        Public Const EvenOddOperatorKeyword As String = "W*"
        Public Const NonZeroOperatorKeyword As String = "W"

        '/**
        '  <summary>'Modify the current clipping path by intersecting it with the current path,
        '  using the even-odd rule to determine which regions lie inside the clipping path'
        '  operation.</summary>
        '*/
        Public Shared ReadOnly EvenOdd As ModifyClipPath = New ModifyClipPath(EvenOddOperatorKeyword, WindModeEnum.EvenOdd)

        '    /**
        '  <summary>'Modify the current clipping path by intersecting it with the current path,
        '  using the nonzero winding number rule to determine which regions lie inside
        '  the clipping path' operation.</summary>
        '*/
        Public Shared ReadOnly NonZero As ModifyClipPath = New ModifyClipPath(NonZeroOperatorKeyword, WindModeEnum.NonZero)

#End Region
#End Region

#Region "dynamic"
#Region "fields"

        Private _clipMode As WindModeEnum

#End Region

#Region "constructors"

        Private Sub New(ByVal operator_ As String, ByVal clipMode As WindModeEnum)
            MyBase.New(operator_)
            Me._clipMode = clipMode
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary>Gets the clipping rule.</summary>
        '*/
        Public ReadOnly Property ClipMode As WindModeEnum
            Get
                Return Me._clipMode
            End Get
        End Property

        Public Overrides Sub Scan(ByVal state As ContentScanner.GraphicsState)
            Dim scanner As ContentScanner = state.Scanner
            Dim pathObject As GraphicsPath = scanner.RenderObject
            If (pathObject IsNot Nothing) Then
                pathObject.FillMode = _clipMode.ToGdi()
                scanner.RenderContext.SetClip(pathObject, CombineMode.Intersect)
            End If
        End Sub

#End Region
#End Region
#End Region

    End Class
End Namespace
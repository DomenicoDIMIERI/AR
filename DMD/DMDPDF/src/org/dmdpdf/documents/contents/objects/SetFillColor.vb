'/*
'  Copyright 2007-2010 Stefano Chizzolini. http://www.dmdpdf.org

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
Imports DMD.org.dmdpdf.documents.contents.colorSpaces
Imports DMD.org.dmdpdf.objects

Imports System.Collections.Generic
Imports System.Linq

Namespace DMD.org.dmdpdf.documents.contents.objects

    '/**
    '  <summary>'Set the color to use for nonstroking operations' operation [PDF:1.6:4.5.7].</summary>
    '*/
    <PDF(VersionEnum.PDF12)>
    Public Class SetFillColor
        Inherits Operation

#Region "static"
#Region "fields"

        '/**
        '  <summary>'Set the color to use for nonstroking operations in any color space' operator.</summary>
        '*/
        <PDF(VersionEnum.PDF12)>
        Public Shared ReadOnly ExtendedOperatorKeyword As String = "scn"

        '    /**
        '  <summary>'Set the color to use for nonstroking operations in a device, CIE-based (other than ICCBased),
        '  or Indexed color space' operator.</summary>
        '*/
        <PDF(VersionEnum.PDF11)>
        Public Shared ReadOnly OperatorKeyword As String = "sc"

#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        Public Sub New(ByVal value As Color)
            Me.New(ExtendedOperatorKeyword, value)
        End Sub

        Public Sub New(ByVal operands As IList(Of PdfDirectObject))
            Me.New(ExtendedOperatorKeyword, operands)
        End Sub

        Public Sub New(ByVal operator_ As String, ByVal operands As IList(Of PdfDirectObject))
            MyBase.New(operator_, operands)
        End Sub

        Protected Sub New(ByVal operator_ As String, ByVal value As Color)
            MyBase.New(operator_, New List(Of PdfDirectObject)(value.Components))
        End Sub

        '/**
        '  <param name="operator_">Graphics operator.</param>
        '  <param name="name">Name of the color resource entry (see <see cref="Pattern"/>).</param>
        ' */
        Protected Sub New(ByVal operator_ As String, ByVal name As PdfName)
            Me.New(operator_, name, Nothing)
        End Sub

        '/**
        '  <param name="operator_">Graphics operator.</param>
        '  <param name="name">Name of the color resource entry (see <see cref="Pattern"/>).</param>
        '  <param name="underlyingColor">Color used to colorize the pattern.</param>
        ' */
        Protected Sub New(ByVal operator_ As String, ByVal name As PdfName, ByVal underlyingColor As Color)
            MyBase.New(operator_, New List(Of PdfDirectObject))
            If (underlyingColor IsNot Nothing) Then
                For Each component As PdfDirectObject In underlyingColor.Components
                    Me._operands.Add(component)
                Next
            End If
            Me._operands.Add(name)
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public ReadOnly Property Components As IList(Of PdfDirectObject)
            Get
                Return Me._operands
            End Get
        End Property

        Public Overrides Sub Scan(ByVal state As ContentScanner.GraphicsState)
            state.FillColor = state.FillColorSpace.GetColor(Operands, state.Scanner.ContentContext)
        End Sub

#End Region
#End Region
#End Region

    End Class

End Namespace
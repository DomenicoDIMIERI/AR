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
Imports DMD.org.dmdpdf.documents.contents.fonts
Imports DMD.org.dmdpdf.objects

Imports System.Collections.Generic

Namespace DMD.org.dmdpdf.documents.contents.objects

    '/**
    '  <summary>'Set the text font' operation [PDF:1.6:5.2].</summary>
    '*/
    <PDF(VersionEnum.PDF10)>
    Public NotInheritable Class SetFont
        Inherits Operation
        Implements IResourceReference(Of Font)

#Region "Static"
#Region "fields"

        Public Shared ReadOnly OperatorKeyword As String = "Tf"

#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        Public Sub New(ByVal name As PdfName, ByVal size As Double)
            MyBase.New(OperatorKeyword, name, PdfReal.Get(size))
        End Sub

        Public Sub New(ByVal operands As IList(Of PdfDirectObject))
            MyBase.New(OperatorKeyword, operands)
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary>Gets the <see cref="Font">font</see> resource to be set.</summary>
        '  <param name="context">Content context.</param>
        '*/
        Public Function GetFont(ByVal context As IContentContext) As Font
            Return GetResource(context)
        End Function

        Public Overrides Sub Scan(ByVal state As ContentScanner.GraphicsState)
            Dim font As Font = GetFont(state.Scanner.ContentContext)
            If (font Is Nothing) Then
                Debug.Print("Opps 2")
            End If

            state.Font = font
            state.FontSize = Me.Size
        End Sub

        '/**
        '  <summary>Gets/Sets the font size to be set.</summary>
        '*/
        Public Property Size As Double
            Get
                Return CType(Me._operands(1), IPdfNumber).RawValue
            End Get
            Set(ByVal value As Double)
                Me._operands(1) = PdfReal.Get(value)
            End Set
        End Property

#Region "IResourceReference"

        Public Function GetResource(ByVal context As IContentContext) As Font Implements IResourceReference(Of Font).GetResource
            Return context.Resources.Fonts(Name)
        End Function

        Public Property Name As PdfName Implements IResourceReference(Of Font).Name
            Get
                Return CType(Me._operands(0), PdfName)
            End Get
            Set(ByVal value As PdfName)
                Me._operands(0) = value
            End Set
        End Property

#End Region
#End Region
#End Region
#End Region

    End Class

End Namespace
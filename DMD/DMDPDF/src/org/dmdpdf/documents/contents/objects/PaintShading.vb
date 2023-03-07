'/*
'  Copyright 2007-2011 Stefano Chizzolini. http://www.dmdpdf.org

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

Namespace DMD.org.dmdpdf.documents.contents.objects

    '/**
    '  <summary>'Paint the shape and color shading' operation [PDF:1.6:4.6.3].</summary>
    '*/
    <PDF(VersionEnum.PDF13)>
    Public NotInheritable Class PaintShading
        Inherits Operation
        Implements IResourceReference(Of colorSpaces.Shading)

#Region "Static"
#Region "fields"

        Public Shared ReadOnly OperatorKeyword As String = "sh"

#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        Public Sub New(ByVal name As PdfName)
            MyBase.New(OperatorKeyword, name)
        End Sub

        Public Sub New(ByVal operands As IList(Of PdfDirectObject))
            MyBase.New(OperatorKeyword, operands)
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary>Gets the <see cref="colorSpaces.Shading">shading</see> resource to be painted.
        '  </summary>
        '  <param name="context">Content context.</param>
        '*/
        Public Function GetShading(ByVal context As IContentContext) As colorSpaces.Shading
            Return GetResource(context)
        End Function

#Region "IResourceReference"

        Public Function GetResource(ByVal context As IContentContext) As colorSpaces.Shading Implements IResourceReference(Of colorSpaces.Shading).GetResource
            Return context.Resources.Shadings(Name)
        End Function

        Public Property Name As PdfName Implements IResourceReference(Of colorSpaces.Shading).Name
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
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
Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.objects

Imports System
Imports System.Collections.Generic
Imports System.Drawing

Namespace DMD.org.dmdpdf.documents.interaction.annotations

    '/**
    '  <summary>Abstract vertexed shape annotation.</summary>
    '*/
    <PDF(VersionEnum.PDF15)>
    Public MustInherit Class VertexShape
        Inherits Shape

#Region "dynamic"
#Region "constructors"

        Protected Sub New(ByVal page As Page, ByVal box As RectangleF, ByVal text As String, ByVal subtype As PdfName)
            MyBase.new(page, box, text, subtype)
        End Sub

        Protected Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.new(baseObject)
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary>Gets/Sets the coordinates of each vertex.</summary>
        '*/
        Public Property Vertices As IList(Of PointF)
            Get
                Dim verticesObject As PdfArray = CType(Me.BaseDataObject(PdfName.Vertices), PdfArray)
                Dim _vertices As IList(Of PointF) = New List(Of PointF)
                Dim pageHeight As Single = Me.Page.Box.Height
                Dim length As Integer = verticesObject.Count
                For index As Integer = 0 To length - 1 Step 2 'index += 2
                    _vertices.Add(
                            New PointF(
                              (CType(verticesObject(index), IPdfNumber)).FloatValue,
                              pageHeight - (CType(verticesObject(index + 1), IPdfNumber)).FloatValue
                              )
                            )
                Next
                Return _vertices
            End Get
            Set(ByVal value As IList(Of PointF))
                Dim verticesObject As PdfArray = New PdfArray()
                Dim pageHeight As Single = Me.Page.Box.Height
                For Each vertex As PointF In value
                    verticesObject.Add(PdfReal.Get(vertex.X)) ' x.
                    verticesObject.Add(PdfReal.Get(pageHeight - vertex.Y)) ' y.
                Next
                Me.BaseDataObject(PdfName.Vertices) = verticesObject
            End Set
        End Property

#End Region
#End Region
#End Region

    End Class

End Namespace
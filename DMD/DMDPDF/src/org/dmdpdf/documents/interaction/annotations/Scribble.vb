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
    '  <summary>Freehand "scribble" composed of one or more disjoint paths [PDF:1.6:8.4.5].</summary>
    '*/
    <PDF(VersionEnum.PDF13)>
    Public NotInheritable Class Scribble
        Inherits Annotation

#Region "dynamic"
#Region "constructors"

        Public Sub New(ByVal page As Page, ByVal box As RectangleF, ByVal text As String, ByVal paths As IList(Of IList(Of PointF)))
            MyBase.New(page, PdfName.Ink, box, text)
            Me.Paths = paths
        End Sub

        Friend Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary> Gets/Sets the coordinates Of Each path.</summary>
        '*/
        Public Property Paths As IList(Of IList(Of PointF))
            Get
                Dim pathsObject As PdfArray = CType(BaseDataObject(PdfName.InkList), PdfArray)
                Dim _paths As IList(Of IList(Of PointF)) = New List(Of IList(Of PointF))()
                Dim pageHeight As Double = Page.Box.Height
                Dim pathLength As Integer = pathsObject.Count
                For pathIndex As Integer = 0 To pathLength - 1
                    Dim pathObject As PdfArray = CType(pathsObject(pathIndex), PdfArray)
                    Dim path As IList(Of PointF) = New List(Of PointF)()
                    Dim pointLength As Integer = pathObject.Count
                    For pointIndex As Integer = 0 To pointLength - 1 Step 2
                        path.Add(
                            New PointF(
                                    CSng(CType(pathObject(pointIndex), IPdfNumber).RawValue),
                                    CSng(pageHeight - CType(pathObject(pointIndex + 1), IPdfNumber).RawValue)
                                    )
                                )
                    Next
                    _paths.Add(path)
                Next
                Return _paths
            End Get
            Set(ByVal value As IList(Of IList(Of PointF)))
                Dim pathsObject As PdfArray = New PdfArray()
                Dim pageHeight As Double = Page.Box.Height
                For Each path As IList(Of PointF) In value
                    Dim pathObject As PdfArray = New PdfArray()
                    For Each point As PointF In path
                        pathObject.Add(PdfReal.Get(point.X)) ' x.
                        pathObject.Add(PdfReal.Get(pageHeight - point.Y)) ' y.
                    Next
                    pathsObject.Add(pathObject)
                Next
                BaseDataObject(PdfName.InkList) = pathsObject
            End Set
        End Property

#End Region
#End Region
#End Region

    End Class

End Namespace
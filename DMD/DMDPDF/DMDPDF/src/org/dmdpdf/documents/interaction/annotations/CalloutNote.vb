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
    '  <summary>Free text annotation [PDF:1.6:8.4.5].</summary>
    '  <remarks>It displays text directly on the page. Unlike an ordinary text annotation,
    '  a free text annotation has no open or closed state;
    '  instead of being displayed in a pop-up window, the text is always visible.</remarks>
    '*/
    <PDF(VersionEnum.PDF13)>
    Public NotInheritable Class CalloutNote
        Inherits Annotation

#Region "types"
        '/**
        '  <summary>Callout line [PDF:1.6:8.4.5].</summary>
        '*/
        Public Class LineObject
            Inherits PdfObjectWrapper(Of PdfArray)

#Region "dynamic"
#Region "fields"

            Private _page As Page

#End Region

#Region "constructors"

            Public Sub New(ByVal page As Page, ByVal start As PointF, ByVal [end] As PointF)
                Me.New(page, start, Nothing, [end])
            End Sub

            Public Sub New(ByVal page As Page, ByVal start As PointF, ByVal knee As PointF?, ByVal [end] As PointF)
                MyBase.New(page.Document, New PdfArray())
                Me._page = page
                Dim BaseDataObject As PdfArray = Me.BaseDataObject
                '{
                Dim pageHeight As Double = page.Box.Height
                BaseDataObject.Add(PdfReal.Get(start.X))
                BaseDataObject.Add(PdfReal.Get(pageHeight - start.Y))
                If (knee.HasValue) Then
                    BaseDataObject.Add(PdfReal.Get(knee.Value.X))
                    BaseDataObject.Add(PdfReal.Get(pageHeight - knee.Value.Y))
                End If
                BaseDataObject.Add(PdfReal.Get([end].X))
                BaseDataObject.Add(PdfReal.Get(pageHeight - [end].Y))
                '}
            End Sub

            Friend Sub New(ByVal baseObject As PdfDirectObject)
                MyBase.New(baseObject)
            End Sub

#End Region

#Region "Interface"
#Region "Public"

            Public ReadOnly Property [End] As PointF
                Get
                    Dim coordinates As PdfArray = Me.BaseDataObject
                    If (coordinates.Count < 6) Then
                        Return New PointF(
                                        CSng(CType(coordinates(2), IPdfNumber).RawValue),
                                        CSng(_page.Box.Height - CType(coordinates(3), IPdfNumber).RawValue)
                                        )
                    Else
                        Return New PointF(
                                        CSng(CType(coordinates(4), IPdfNumber).RawValue),
                                        CSng(_page.Box.Height - CType(coordinates(5), IPdfNumber).RawValue)
                                        )
                    End If
                End Get
            End Property

            Public ReadOnly Property Knee As PointF?
                Get
                    Dim coordinates As PdfArray = Me.BaseDataObject
                    If (coordinates.Count < 6) Then Return Nothing
                    Return New PointF(
                                    CSng(CType(coordinates(2), IPdfNumber).RawValue),
                                    CSng(_page.Box.Height - CType(coordinates(3), IPdfNumber).RawValue)
                                    )
                End Get
            End Property

            Public ReadOnly Property Start As PointF
                Get
                    Dim coordinates As PdfArray = Me.BaseDataObject
                    Return New PointF(
                                CSng(CType(coordinates(0), IPdfNumber).RawValue),
                                CSng(_page.Box.Height - CType(coordinates(1), IPdfNumber).RawValue)
                                )
                End Get
            End Property

#End Region
#End Region
#End Region

        End Class

#End Region

#Region "dynamic"
#Region "constructors"

        Public Sub New(ByVal page As Page, ByVal box As RectangleF, ByVal Text As String)
            MyBase.New(page, PdfName.FreeText, box, Text)
        End Sub

        Friend Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary>Gets/Sets the justification To be used In displaying the annotation's text.</summary>
        '*/
        Public Property Justification As JustificationEnum
            Get
                Return JustificationEnumExtension.Get(CType(BaseDataObject(PdfName.Q), PdfInteger))
            End Get
            Set(ByVal value As JustificationEnum)
                Me.BaseDataObject(PdfName.Q) = value.GetCode()
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets the callout line attached To the free text annotation.</summary>
        '*/
        Public Property Line As LineObject
            Get
                Dim calloutLineObject As PdfArray = CType(BaseDataObject(PdfName.CL), PdfArray)
                If (calloutLineObject IsNot Nothing) Then
                    Return New LineObject(calloutLineObject)
                Else
                    Return Nothing
                End If
            End Get
            Set(ByVal value As LineObject)
                Me.BaseDataObject(PdfName.CL) = value.BaseObject
            End Set
        End Property

#End Region
#End Region
#End Region

    End Class

End Namespace
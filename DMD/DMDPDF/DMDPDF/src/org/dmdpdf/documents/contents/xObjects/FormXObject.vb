'/*
'  Copyright 2006-2012 Stefano Chizzolini. http://www.dmdpdf.org

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
Imports DMD.org.dmdpdf.documents.contents
Imports DMD.org.dmdpdf.documents.contents.composition
Imports DMD.org.dmdpdf.documents.contents.objects
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects

Imports System
Imports System.Drawing
Imports System.Drawing.Drawing2D

Namespace DMD.org.dmdpdf.documents.contents.xObjects

    '/**
    '  <summary>Form external object [PDF:1.6:4.9].</summary>
    '*/
    <PDF(VersionEnum.PDF10)>
    Public NotInheritable Class FormXObject
        Inherits XObject
        Implements IContentContext

#Region "Static"
#Region "Interface"
#Region "Public"

        Public Shared Shadows Function Wrap(ByVal baseObject As PdfDirectObject) As FormXObject
            If (baseObject IsNot Nothing) Then
                Return New FormXObject(baseObject)
            Else
                Return Nothing
            End If
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        '/**
        '  <summary>Creates a new form within the specified document context.</summary>
        '  <param name="context">Document where to place this form.</param>
        '  <param name="size">Form size.</param>
        '*/
        Public Sub New(ByVal context As Document, ByVal size As Drawing.SizeF)
            Me.New(context, New Drawing.RectangleF(New Drawing.PointF(0, 0), size))
        End Sub

        '/**
        '  <summary>Creates a New form within the specified document context.</summary>
        '  <param name = "context" > Document where To place this form.</param>
        '  <param name = "box" > Form box.</param>
        '*/
        Public Sub New(ByVal context As Document, ByVal box As Drawing.RectangleF)
            MyBase.New(context)
            BaseDataObject.Header(PdfName.Subtype) = PdfName.Form
            Me.Box = box
        End Sub

        Private Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public Overrides Property Matrix As Matrix
            Get
                '/*
                '  NOTE: Form-Space() -to-user-space matrix Is identity [1 0 0 1 0 0] by default,
                '  but may be adjusted by setting the Matrix entry in the form dictionary [PDF: 1.6:4.9].
                '*/
                Dim _matrix As PdfArray = CType(BaseDataObject.Header.Resolve(PdfName.Matrix), PdfArray)
                If (_matrix Is Nothing) Then
                    Return New Matrix()
                Else
                    Return New Matrix(
                                CType(_matrix(0), IPdfNumber).FloatValue,
                                CType(_matrix(1), IPdfNumber).FloatValue,
                                CType(_matrix(2), IPdfNumber).FloatValue,
                                CType(_matrix(3), IPdfNumber).FloatValue,
                                CType(_matrix(4), IPdfNumber).FloatValue,
                                CType(_matrix(5), IPdfNumber).FloatValue
                                )
                End If
            End Get
            Set(ByVal value As Matrix)
                If (value IsNot Nothing) Then
                    BaseDataObject.Header(PdfName.Matrix) = New PdfArray(
                                                PdfReal.Get(value.Elements(0)),
                                                PdfReal.Get(value.Elements(1)),
                                                PdfReal.Get(value.Elements(2)),
                                                PdfReal.Get(value.Elements(3)),
                                                PdfReal.Get(value.Elements(4)),
                                                PdfReal.Get(value.Elements(5))
                                                )
                Else
                    BaseDataObject.Header(PdfName.Matrix) = Nothing
                End If
            End Set
        End Property

        Public Overrides Property Size As Drawing.SizeF
            Get
                Dim box As PdfArray = CType(BaseDataObject.Header.Resolve(PdfName.BBox), PdfArray)
                Return New Drawing.SizeF(
                              CType(box(2), IPdfNumber).FloatValue,
                              CType(box(3), IPdfNumber).FloatValue
                              )
            End Get
            Set(ByVal value As Drawing.SizeF)
                Dim boxObject As PdfArray = CType(BaseDataObject.Header.Resolve(PdfName.BBox), PdfArray)
                boxObject(2) = PdfReal.Get(value.Width)
                boxObject(3) = PdfReal.Get(value.Height)
            End Set
        End Property

#End Region

#Region "internal"
#Region "IContentContext"

        Public Property Box As Drawing.RectangleF Implements IContentContext.Box
            Get
                Return dmdpdf.objects.Rectangle.Wrap(BaseDataObject.Header(PdfName.BBox)).ToRectangleF()
            End Get
            Set(ByVal value As Drawing.RectangleF)
                BaseDataObject.Header(PdfName.BBox) = New dmdpdf.objects.Rectangle(value).BaseDataObject
            End Set
        End Property

        Public ReadOnly Property Contents As Contents Implements IContentContext.Contents
            Get
                Return Contents.Wrap(BaseObject, Me)
            End Get
        End Property

        Public Sub Render(ByVal context As Drawing.Graphics, ByVal size As Drawing.SizeF) Implements IContentContext.Render
            Dim scanner As ContentScanner = New ContentScanner(Contents)
            scanner.Render(context, size)
        End Sub

        Public Property Resources As Resources Implements IContentContext.Resources
            Get
                Return Resources.Wrap(BaseDataObject.Header.Get(Of PdfDictionary)(PdfName.Resources))
            End Get
            Set(ByVal value As Resources)
                BaseDataObject.Header(PdfName.Resources) = PdfObjectWrapper.GetBaseObject(value)
            End Set
        End Property

        Public ReadOnly Property Rotation As RotationEnum Implements IContentContext.Rotation
            Get
                Return RotationEnum.Downward
            End Get
        End Property

#Region "IContentEntity"

        Public Function ToInlineObject(ByVal composer As PrimitiveComposer) As ContentObject Implements IContentEntity.ToInlineObject
            Throw New NotImplementedException()
        End Function

        Public Function ToXObject(ByVal context As Document) As XObject Implements IContentEntity.ToXObject
            Return CType(Me.Clone(context), XObject)
        End Function

#End Region
#End Region
#End Region
#End Region
#End Region

    End Class

End Namespace
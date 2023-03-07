'/*
'  Copyright 2012 Stefano Chizzolini. http://www.dmdpdf.org

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

'  You should have received a copy of the GNU Lesser General Public License along with Me
'  Program (see README files); if not, go to the GNU website (http://www.gnu.org/licenses/).

'  Redistribution and use, with or without modification, are permitted provided that such
'  redistributions retain the above copyright notice, license and disclaimer, along with
'  Me list of conditions.
'*/

Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.objects
Imports DMD.org.dmdpdf.util

Imports System
Imports System.Drawing
Imports System.Collections.Generic

Namespace DMD.org.dmdpdf.documents.interaction.navigation.page

    '/**
    '  <summary>Article bead [PDF:1.7:8.3.2].</summary>
    '*/
    <PDF(VersionEnum.PDF11)>
    Public NotInheritable Class ArticleElement
        Inherits PdfObjectWrapper(Of PdfDictionary)

#Region "Static"
#Region "Interface"
#Region "Public"

        Public Shared Function Wrap(ByVal baseObject As PdfDirectObject) As ArticleElement
            If (baseObject IsNot Nothing) Then
                Return New ArticleElement(baseObject)
            Else
                Return Nothing
            End If
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        Public Sub New(ByVal page As documents.Page, ByVal box As RectangleF)
            MyBase.New(
                        page.Document,
                        New PdfDictionary(New PdfName() {PdfName.Type}, New PdfDirectObject() {PdfName.Bead})
                        )
            page.ArticleElements.Add(Me)
            Me.Box = box
        End Sub

        Private Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.new(baseObject)
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary>Gets the thread article Me bead belongs To.</summary>
        '*/
        Public ReadOnly Property Article As Article
            Get
                Dim bead As PdfDictionary = BaseDataObject
                Dim _article As Article = Nothing
                _article = Article.Wrap(bead(PdfName.T))
                While (_article Is Nothing)
                    bead = CType(bead.Resolve(PdfName.V), PdfDictionary)
                    _article = Article.Wrap(bead(PdfName.T))
                End While
                Return _article
            End Get
        End Property

        '/**
        '  <summary>Gets/Sets the location On the page In Default user space units.</summary>
        '*/
        Public Property Box As RectangleF
            Get
                Dim _box As org.dmdpdf.objects.Rectangle = org.dmdpdf.objects.Rectangle.Wrap(BaseDataObject(PdfName.R))
                Return New RectangleF(
                                  CSng(_box.Left),
                                  CSng(Me.page.Box.Height - _box.Top),
                                  CSng(_box.Width),
                                  CSng(_box.Height)
                                  )
            End Get
            Set(ByVal value As RectangleF)
                BaseDataObject(PdfName.R) = New org.dmdpdf.objects.Rectangle(
                                                      value.X,
                                                      Me.Page.Box.Height - value.Y,
                                                      value.Width,
                                                      value.Height
                                                      ).BaseDataObject
            End Set
        End Property

        '/**
        '  <summary>Deletes Me bead removing also its references On the page And its article thread.
        '  </summary>
        '*/
        Public Overrides Function Delete() As Boolean
            '// Shallow removal (references):
            '// * thread links
            Article.Elements.Remove(Me)
            ' * reference on page
            Me.Page.ArticleElements.Remove(Me)

            ' Deep removal (indirect object).
            Return MyBase.Delete()
        End Function

        '/**
        '  <summary>Gets whether Me Is the first bead In its thread.</summary>
        '*/
        Public Function IsHead() As Boolean
            Dim thread As PdfDictionary = CType(BaseDataObject.Resolve(PdfName.T), PdfDictionary)
            Return thread IsNot Nothing AndAlso BaseObject.Equals(thread(PdfName.F))
        End Function

        '/**
        '  <summary>Gets the next bead.</summary>
        '*/
        Public ReadOnly Property [Next] As ArticleElement
            Get
                Return ArticleElement.Wrap(BaseDataObject(PdfName.N))
            End Get
        End Property

        '/**
        '  <summary>Gets the location page.</summary>
        '*/
        Public ReadOnly Property Page As documents.Page
            Get
                Return documents.Page.Wrap(BaseDataObject(PdfName.P))
            End Get
        End Property

        '/**
        '  <summary>Gets the previous bead.</summary>
        '*/
        Public ReadOnly Property Previous As ArticleElement
            Get
                Return ArticleElement.Wrap(BaseDataObject(PdfName.V))
            End Get
        End Property

#End Region
#End Region
#End Region

    End Class

End Namespace
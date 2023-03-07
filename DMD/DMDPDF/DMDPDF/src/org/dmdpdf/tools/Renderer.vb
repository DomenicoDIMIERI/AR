'/*
'  Copyright 2010 Stefano Chizzolini. http: //www.dmdpdf.org

'  Contributors:
'    * Stefano Chizzolini (original code developer, http//www.stefanochizzolini.it)

'  This file should be part Of the source code distribution Of "PDF Clown library" (the
'  Program): see the accompanying README files For more info.

'  This Program Is free software; you can redistribute it And/Or modify it under the terms
'  of the GNU Lesser General Public License as published by the Free Software Foundation;
'  either version 3 Of the License, Or (at your Option) any later version.

'  This Program Is distributed In the hope that it will be useful, but WITHOUT ANY WARRANTY,
'  either expressed Or implied; without even the implied warranty Of MERCHANTABILITY Or
'  FITNESS FOR A PARTICULAR PURPOSE. See the License for more details.

'  You should have received a copy Of the GNU Lesser General Public License along With this
'  Program(see README files); If Not, go To the GNU website (http://www.gnu.org/licenses/).

'  Redistribution And use, with Or without modification, are permitted provided that such
'  redistributions retain the above copyright notice, license And disclaimer, along With
'  this list Of conditions.
'*/

Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.documents.contents
Imports DMD.org.dmdpdf.documents.contents.composition
Imports DMD.org.dmdpdf.documents.contents.objects
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects

Imports System.Collections.Generic
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Drawing.Printing
Imports System.Windows.Forms

Namespace DMD.org.dmdpdf.tools

    '/**
    '  <summary> Tool For rendering <see cref="IContentContext">content contexts</see>.</summary>
    '*/
    Public NotInheritable Class Renderer

#Region "types"
        '/**
        '  <summary> Printable document.</summary>
        '  <remarks> It wraps a page collection For printing purposes.</remarks>
        '*/
        Public NotInheritable Class PrintDocument
            Inherits System.Drawing.Printing.PrintDocument

            Private _pages As IList(Of Page)

            Private _pageIndex As Integer
            Private _pagesCount As Integer

            Public Sub New(ByVal Pages As IList(Of Page))
                Me.Pages = Pages
            End Sub


            Public Property Pages As IList(Of Page)
                Get
                    Return Me._pages
                End Get
                Set(value As IList(Of Page))
                    Me._pages = value
                    Me._pagesCount = _pages.Count
                End Set
            End Property

            Protected Overrides Sub OnBeginPrint(ByVal e As PrintEventArgs)
                Me._pageIndex = -1
                MyBase.OnBeginPrint(e)
            End Sub

            Protected Overrides Sub OnPrintPage(ByVal e As PrintPageEventArgs)
                Dim printerSettings As PrinterSettings = e.PageSettings.PrinterSettings
                Select Case (printerSettings.PrintRange)
                    Case PrintRange.SomePages
                        If (Me._pageIndex < printerSettings.FromPage) Then
                            Me._pageIndex = printerSettings.FromPage
                        Else
                            Me._pageIndex += 1
                        End If
                        e.HasMorePages = (Me._pageIndex < printerSettings.ToPage)
                        'break;
                    Case Else
                        Me._pageIndex += 1
                        e.HasMorePages = ((Me._pageIndex + 1) < _pagesCount)
                        'break
                End Select

                Dim page As Page = Me._pages(Me._pageIndex)
                page.Render(e.Graphics, e.PageBounds.Size)

                MyBase.OnPrintPage(e)
            End Sub
        End Class

#End Region

#Region "static"
#Region "interface"
#Region "public"
        '/**
        '  <summary> Wraps the specified document into a printable Object.</summary>
        '  <param name = "document" > document To wrap For printing.</param>
        '  <returns> Printable Object.</returns>
        '*/
        Public Shared Function GetPrintDocument(ByVal document As Document) As PrintDocument
            Return New PrintDocument(document.Pages)
        End Function

        '/**
        '  <summary> Wraps the specified page collection into a printable Object.</summary>
        '  <param name = "pages" > page collection To print.</param>
        '  <returns> Printable Object.</returns>
        '*/
        Public Shared Function GetPrintDocument(ByVal pages As IList(Of Page)) As PrintDocument
            Return New PrintDocument(pages)
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "interface"
#Region "public"
        '/**
        '  <summary> Prints silently the specified document.</summary>
        '  <param name = "document" > document To print.</param>
        '  <returns> Whether the print was fulfilled.</returns>
        '*/
        Public Function Print(ByVal document As Document) As Boolean
            Return Print(document.Pages)
        End Function

        '/**
        '  <summary> Prints the specified document.</summary>
        '  <param name = "document" > document To print.</param>
        '  <param name = "silent" > Whether To avoid showing a print dialog.</param>
        '  <returns> Whether the print was fulfilled.</returns>
        '*/
        Public Function Print(ByVal document As Document, ByVal silent As Boolean) As Boolean
            Return Print(document.Pages, silent)
        End Function


        '/**
        '  <summary> Prints silently the specified page collection.</summary>
        '  <param name = "pages" > page collection To print.</param>
        '  <returns> Whether the print was fulfilled.</returns>
        '*/
        Public Function Print(ByVal pages As IList(Of Page)) As Boolean
            Return Print(pages, True)
        End Function

        '/**
        '  <summary> Prints the specified page collection.</summary>
        '  <param name = "pages" > page collection To print.</param>
        '  <param name = "silent" > Whether To avoid showing a print dialog.</param>
        '  <returns> Whether the print was fulfilled.</returns>
        '*/
        Public Function Print(ByVal pages As IList(Of Page), ByVal silent As Boolean) As Boolean
            Dim printDocument As PrintDocument = GetPrintDocument(pages)
            If (Not silent) Then
                Dim printDialog As PrintDialog = New PrintDialog()
                printDialog.Document = printDocument
                If (printDialog.ShowDialog() <> DialogResult.OK) Then
                    Return False
                End If
            End If

            printDocument.Print()
            Return True
        End Function

        '/**
        '  <summary> Renders the specified contents into an image context.</summary>
        '  <param name = "contents" > Source contents.</param>
        '  <param name = "size" > Image size expressed In device-space units (that Is typically pixels).</param>
        '  <returns> Image representing the rendered contents.</returns>
        ' */
        Public Function Render(ByVal contents As Contents, ByVal size As SizeF) As Image
            Return Render(contents, size, Nothing)
        End Function

        '/**
        '  <summary> Renders the specified content context into an image context.</summary>
        '  <param name = "contentContext" > Source content context.</param>
        '  <param name = "size" > Image size expressed In device-space units (that Is typically pixels).</param>
        '  <returns> Image representing the rendered contents.</returns>
        ' */
        Public Function Render(ByVal contentContext As IContentContext, ByVal size As SizeF) As Image
            Return Render(contentContext, size, Nothing)
        End Function

        '/**
        '  <summary> Renders the specified contents into an image context.</summary>
        '  <param name = "contents" > Source contents.</param>
        '  <param name = "size" > Image size expressed In device-space units (that Is typically pixels).</param>
        '  <param name = "area" > Content area To render; <code>null</code> corresponds To the entire
        '   <see cref = "IContentContext.Box" > content bounding box</see>.</param>
        '  <returns> Image representing the rendered contents.</returns>
        ' */
        Public Function Render(ByVal contents As Contents, ByVal size As SizeF, ByVal area As RectangleF?) As Image
            Return Render(contents.ContentContext, size, area)
        End Function

        '/**
        '  <summary> Renders the specified content context into an image context.</summary>
        '  <param name = "contentContext" > Source content context.</param>
        '  <param name = "size" > Image size expressed In device-space units (that Is typically pixels).</param>
        '  <param name = "area" > Content area To render; <code>null</code> corresponds To the entire
        '   <see cref = "IContentContext.Box" > content bounding box</see>.</param>
        '  <returns> Image representing the rendered contents.</returns>
        ' */
        Public Function Render(ByVal contentContext As IContentContext, ByVal size As SizeF, ByVal area As RectangleF?) As Image
            'TODO:area!
            Dim image As Image = New Bitmap(CInt(size.Width), CInt(size.Height), PixelFormat.Format24bppRgb)
            contentContext.Render(Graphics.FromImage(image), size)
            Return image
        End Function
#End Region
#End Region
#End Region
    End Class

End Namespace


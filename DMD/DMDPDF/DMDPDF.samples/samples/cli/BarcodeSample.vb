Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.documents.contents.composition
Imports DMD.org.dmdpdf.documents.contents.entities
Imports DMD.org.dmdpdf.documents.contents.fonts
Imports DMD.org.dmdpdf.documents.contents.xObjects
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.util.math.geom

Imports System
Imports System.Collections.Generic
Imports System.Drawing

Namespace org.dmdpdf.samples.cli

    '/**
    '  <summary>This sample demonstrates how to show bar codes in a PDF document.</summary>
    '*/
    Public Class BarcodeSample
        Inherits Sample

        Private Const Margin As Single = 36

        Public Overrides Sub Run()
            ' 1. PDF file instantiation.
            Dim file As File = New File()
            Dim document As Document = file.Document

            ' 2. Content creation.
            Populate(document)

            ' 3. Serialize the PDF file!
            Serialize(file, "Barcode", "showing barcodes", "barcodes, creation, EAN13")
        End Sub

        '/**
        '  <summary>Populates a PDF file with contents.</summary>
        '*/
        Private Sub Populate(ByVal document As Document)
            ' Get the abstract barcode entity!
            Dim barcode As EAN13Barcode = New EAN13Barcode("8012345678901")
            ' Create the reusable barcode within the document!
            Dim barcodeXObject As XObject = barcode.ToXObject(document)

            Dim pages As Pages = document.Pages
            ' Page 1.
            '{
            Dim page As Page = New Page(document)
            pages.Add(page)
            Dim pageSize As SizeF = page.Size

            Dim composer As New PrimitiveComposer(page)
            '{
            Dim blockComposer As New BlockComposer(composer)
            blockComposer.Hyphenation = True
            blockComposer.Begin(
                                New RectangleF(Margin, Margin, pageSize.Width - Margin * 2, pageSize.Height - Margin * 2),
                                XAlignmentEnum.Left,
                                YAlignmentEnum.Top
                                )
            Dim bodyFont As New StandardType1Font(document, StandardType1Font.FamilyEnum.Courier, True, False)
            composer.SetFont(bodyFont, 32)
            blockComposer.ShowText("Barcode sample") : blockComposer.ShowBreak()
            composer.SetFont(bodyFont, 16)
            blockComposer.ShowText("Showing the EAN-13 Bar Code on different compositions:") : blockComposer.ShowBreak()
            blockComposer.ShowText("- page 1: on the lower right corner of the page, 100pt wide;") : blockComposer.ShowBreak()
            blockComposer.ShowText("- page 2: on the middle of the page, 1/3-page wide, 25 degree counterclockwise rotated;") : blockComposer.ShowBreak()
            blockComposer.ShowText("- page 3: filled page, 90 degree clockwise rotated.") : blockComposer.ShowBreak()
            blockComposer.End()
            '}

            ' Show the barcode!
            composer.ShowXObject(
                                barcodeXObject,
                                New PointF(pageSize.Width - Margin, pageSize.Height - Margin),
                                GeomUtils.Scale(barcodeXObject.Size, New SizeF(100, 0)),
                                XAlignmentEnum.Right,
                                YAlignmentEnum.Bottom,
                                0
                                )
            composer.Flush()
            '}

            ' Page 2.
            '{
            page = New Page(document)
            pages.Add(page)
            pageSize = page.Size

            composer = New PrimitiveComposer(page)
            ' Show the barcode!
            composer.ShowXObject(barcodeXObject,
                                    New PointF(pageSize.Width / 2, pageSize.Height / 2),
                                    GeomUtils.Scale(barcodeXObject.Size, New SizeF(pageSize.Width / 3, 0)),
                                    XAlignmentEnum.Center,
                                    YAlignmentEnum.Middle,
                                    25
                                )
            composer.Flush()
            '}

            '// Page 3.
            '{
            page = New Page(document)
            pages.Add(page)
            pageSize = page.Size

            composer = New PrimitiveComposer(page)
            ' Show the barcode!
            composer.ShowXObject(barcodeXObject,
                                  New PointF(pageSize.Width / 2, pageSize.Height / 2),
                                  New SizeF(pageSize.Height, pageSize.Width),
                                  XAlignmentEnum.Center,
                                  YAlignmentEnum.Middle,
                                  -90
                                  )
            composer.Flush()
            '}
        End Sub
    End Class

End Namespace
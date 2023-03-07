Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.documents.contents.composition
Imports entities = DMD.org.dmdpdf.documents.contents.entities
Imports DMD.org.dmdpdf.documents.contents.fonts
Imports files = DMD.org.dmdpdf.files

Imports System
Imports System.Drawing
Imports System.IO

Namespace org.dmdpdf.samples.cli

    '/**
    '  <summary>This sample demonstrates how to embed an image object within a PDF content
    '  stream.</summary>
    '  <remarks>
    '    <para>Inline objects should be used sparingly, as they easily clutter content
    '    streams.</para>
    '    <para>The alternative (and preferred) way to insert an image object is via external
    '    object (XObject); its main advantage is to allow content reuse.</para>
    '  </remarks>
    '*/
    Public Class InlineObjectSample
        Inherits Sample

        Private Const Margin As Single = 36

        Public Overrides Sub Run()
            ' 1. PDF file instantiation.
            Dim file As New files.File()
            Dim document As Document = file.Document

            ' 2. Content creation.
            Populate(document)

            ' 3. Serialize the PDF file!
            Serialize(file, "Inline image", "embedding an image within a content stream", "inline image")
        End Sub

        Private Sub Populate(ByVal document As Document)
            Dim page As Page = New Page(document)
            document.Pages.Add(page)
            Dim pageSize As SizeF = page.Size

            Dim composer As PrimitiveComposer = New PrimitiveComposer(page)
            '{
            Dim blockComposer As BlockComposer = New BlockComposer(composer)
            blockComposer.Hyphenation = True
            blockComposer.Begin(
                              New RectangleF(
                                Margin,
                                Margin,
                                CSng(pageSize.Width) - Margin * 2,
                                CSng(pageSize.Height) - Margin * 2
                                ),
                              XAlignmentEnum.Justify,
                              YAlignmentEnum.Top
                              )
            Dim bodyFont As StandardType1Font = New StandardType1Font(
                              document,
                              StandardType1Font.FamilyEnum.Courier,
                              True,
                              False
                              )
            composer.SetFont(bodyFont, 32)
            blockComposer.ShowText("Inline image sample") : blockComposer.ShowBreak()
            composer.SetFont(bodyFont, 16)
            blockComposer.ShowText("Showing the GNU logo as an inline image within the page content stream.")
            blockComposer.End()
            '}
            ' Showing the 'GNU' image...
            '{
            ' Instantiate a jpeg image object!
            Dim image As entities.Image = entities.Image.Get(GetResourcePath("images" & Path.DirectorySeparatorChar & "gnu.jpg")) ' Abstract image (entity).
            ' Set the position of the image in the page!
            composer.ApplyMatrix(200, 0, 0, 200, (pageSize.Width - 200) / 2, (pageSize.Height - 200) / 2)
            ' Show the image!
            image.ToInlineObject(composer) ' Transforms the image entity into an inline image within the page.
            '}
            composer.Flush()
        End Sub

    End Class

End Namespace
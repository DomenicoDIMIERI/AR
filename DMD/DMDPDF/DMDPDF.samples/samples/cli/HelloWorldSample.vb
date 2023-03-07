Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.documents.contents.composition
Imports DMD.org.dmdpdf.documents.contents.fonts
Imports DMD.org.dmdpdf.files

Imports System.Drawing

Namespace org.dmdpdf.samples.cli

    '/**
    '  <summary>This sample is a minimalist introduction to the use of PDF Clown.</summary>
    '*/
    Public Class HelloWorldSample
        Inherits Sample

        Public Overrides Sub Run()
            '// 1. Instantiate a New PDF file!
            '/* NOTE: a File object is the low-level (syntactic) representation of a PDF file. */
            Dim file As File = New File()

            ' 2. Get its corresponding document!
            '/* NOTE: a Document object is the high-level (semantic) representation of a PDF file. */
            Dim document As Document = file.Document

            ' 3. Insert the contents into the document!
            Populate(document)

            ' 4. Serialize the PDF file!
            Serialize(file, "Hello world", "a simple 'hello world'", "Hello world")
        End Sub

        '/**
        '  <summary>Populates a PDF file with contents.</summary>
        '*/
        Private Sub Populate(ByVal document As Document)
            ' 1. Add the page to the document!
            Dim page As Page = New Page(document) ' Instantiates the page inside the document context.
            document.Pages.Add(page) ' Puts the page in the pages collection.

            ' 2. Create a content composer for the page!
            Dim composer As PrimitiveComposer = New PrimitiveComposer(page)

            ' 3. Inserting contents...
            ' Set the font to use!
            composer.SetFont(
                                New StandardType1Font(
                                  document,
                                  StandardType1Font.FamilyEnum.Courier,
                                  True,
                                  False
                                  ),
                                32
                                )
            '// Show the text onto the page!
            '/*
            '  NOTE: PrimitiveComposer's ShowText() method is the most basic way to add text to a page
            '  -- see BlockComposer for more advanced uses (horizontal and vertical alignment, hyphenation,
            '  etc.).
            '*/
            composer.ShowText(
                            "Hello World!",
                            New PointF(32, 48)
                            )

            ' 4. Flush the contents into the page!
            composer.Flush()
        End Sub

    End Class

End Namespace
Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.documents.contents.composition
Imports DMD.org.dmdpdf.documents.contents.fonts
Imports DMD.org.dmdpdf.documents.contents.xObjects
Imports DMD.org.dmdpdf.files

Imports System.Drawing

Namespace org.dmdpdf.samples.cli

    '/**
    '  <summary>This sample demonstrates how to combine multiple pages into single bigger pages (for
    '  example two A4 modules into one A3 module) using form XObjects [PDF:1.6:4.9].</summary>
    '  <remarks>Form XObjects are a convenient way to represent contents multiple times on multiple pages
    '  as templates.</remarks>
    '*/
    Public Class PageCombinationSample
        Inherits Sample

        Public Overrides Sub Run()
            ' 1. Instantiate the source PDF file!
            Dim filePath As String = PromptFileChoice("Please select a PDF file to use as source")
            Using sourceFile As New File(filePath)
                ' 2. Instantiate a New PDF file!
                Dim _file As File = New File()

                ' 3. Source page combination into target file.
                Dim document As Document = _file.Document
                Dim pages As Pages = document.Pages
                Dim pageIndex As Integer = -1
                Dim composer As PrimitiveComposer = Nothing
                Dim targetPageSize As SizeF = PageFormat.GetSize(PageFormat.SizeEnum.A4)
                For Each sourcePage As Page In sourceFile.Document.Pages
                    pageIndex += 1
                    Dim pageMod As Integer = pageIndex Mod 2
                    If (pageMod = 0) Then
                        If (composer IsNot Nothing) Then
                            composer.Flush()
                        End If

                        ' Add a page to the target document!
                        Dim page As Page = New Page(
                                              document,
                                              PageFormat.GetSize(PageFormat.SizeEnum.A3, PageFormat.OrientationEnum.Landscape)
                                              ) ' Instantiates the page inside the document context.
                        pages.Add(page) ' Puts the page In the pages collection.
                        ' Create a composer for the target content stream!
                        composer = New PrimitiveComposer(page)
                    End If

                    ' Add the form to the target page!
                    '        composer.ShowXObject(
                    'sourcePage.ToXObject(Document), // Converts the source page into a form inside the target document.
                    'New PointF(targetPageSize.Width * pageMod, 0),
                    'targetPageSize,
                    'XAlignmentEnum.Left,
                    'YAlignmentEnum.Top,
                    '0
                    ');
                    composer.ShowXObject(
                                        sourcePage.ToXObject(document),
                                        New PointF(targetPageSize.Width * pageMod, 0),
                                        targetPageSize,
                                        XAlignmentEnum.Left,
                                        YAlignmentEnum.Top,
                                        0
                                        )
                Next
                composer.Flush()

                ' 4. Serialize the PDF file!
                Serialize(_file, "Page combination", "combining multiple pages into single bigger ones", "page combination")
            End Using
        End Sub

    End Class

End Namespace
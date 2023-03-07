Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.documents.contents
Imports DMD.org.dmdpdf.documents.contents.colorSpaces
Imports DMD.org.dmdpdf.documents.contents.composition
Imports entities = DMD.org.dmdpdf.documents.contents.entities
Imports DMD.org.dmdpdf.documents.contents.fonts
Imports DMD.org.dmdpdf.documents.files
Imports DMD.org.dmdpdf.documents.interaction.actions
Imports annotations = DMD.org.dmdpdf.documents.interaction.annotations
Imports DMD.org.dmdpdf.documents.interaction.navigation.document
Imports files = DMD.org.dmdpdf.files

Imports System
Imports System.Drawing
Imports System.IO

Namespace org.dmdpdf.samples.cli

    '/**
    '  <summary>This sample demonstrates how to apply links to a PDF document.</summary>
    '*/
    Public Class LinkCreationSample
        Inherits Sample

        Public Overrides Sub Run()
            ' 1. Creating the document...
            Dim file As New files.File()
            Dim document As Document = file.Document

            ' 2. Applying links...
            BuildLinks(document)

            ' 3. Serialize the PDF file!
            Serialize(file, "Link annotations", "applying link annotations", "links, creation")
        End Sub

        Private Sub BuildLinks(ByVal document As Document)
            Dim link As annotations.Link
            Dim pages As Pages = document.Pages
            Dim page As Page = New Page(document)
            pages.Add(page)

            Dim font As StandardType1Font = New StandardType1Font(
                                                document,
                                                StandardType1Font.FamilyEnum.Courier,
                                                True,
                                                False
                                                )

            Dim composer As PrimitiveComposer = New PrimitiveComposer(page)
            Dim blockComposer As BlockComposer = New BlockComposer(composer)

            '/*
            '  2.1. Goto-URI link.
            '*/
            '{
            blockComposer.Begin(New RectangleF(30, 100, 200, 50), XAlignmentEnum.Left, YAlignmentEnum.Middle)
            composer.SetFont(font, 12)
            blockComposer.ShowText("Go-to-URI link")
            composer.SetFont(font, 8)
            blockComposer.ShowText(vbLf & "It allows you to navigate to a network resource.")
            composer.SetFont(font, 5)
            blockComposer.ShowText(vbLf & vbLf & "Click on the box to go to the project's SourceForge.net repository.")
            blockComposer.End()

            Try
                '/*
                '  NOTE: This statement instructs the PDF viewer to navigate to the given URI when the link is clicked.
                '*/
                link = New annotations.Link(
                                                            page,
                                                            New Rectangle(240, 100, 100, 50),
                                                            "Link annotation",
                                                            New GoToURI(
                                                              document,
                                                              New Uri("http://www.sourceforge.net/projects/clown")
                                                              )
                                                            )
                link.Border = New annotations.Border(
                                    document,
                                    3,
                                    annotations.Border.StyleEnum.Beveled,
                                    Nothing
                                    )
            Catch exception As System.Exception
                Throw New System.Exception("", exception)
            End Try

            '/*
            '  2.2. Embedded-goto link.
            '*/
            '{
            Dim filePath As String = PromptFileChoice("Please select a PDF file to attach")

            '/*
            '  NOTE: These statements instruct PDF Clown to attach a PDF file to the current document.
            '  This is necessary in order to test the embedded-goto functionality,
            '  as you can see in the following link creation (see below).
            '*/
            Dim fileAttachmentPageIndex As Integer = page.Index
            Dim fileAttachmentName As String = "attachedSamplePDF"
            Dim fileName As String = System.IO.Path.GetFileName(filePath)
            Dim attachment = New annotations.FileAttachment(
                                              page,
                                              New Rectangle(0, -20, 10, 10),
                                              "File attachment annotation",
                                              FileSpecification.Get(
                                                EmbeddedFile.Get(
                                                  document,
                                                  filePath
                                                  ),
                                                fileName
                                                )
                                              )
            attachment.Name = fileAttachmentName
            attachment.IconType = annotations.FileAttachment.IconTypeEnum.PaperClip

            blockComposer.Begin(New RectangleF(30, 170, 200, 50), XAlignmentEnum.Left, YAlignmentEnum.Middle)
            composer.SetFont(font, 12)
            blockComposer.ShowText("Go-to-embedded link")
            composer.SetFont(font, 8)
            blockComposer.ShowText(vbLf & "It allows you to navigate to a destination within an embedded PDF file.")
            composer.SetFont(font, 5)
            blockComposer.ShowText(vbLf & vbLf & "Click on the button to go to the 2nd page of the attached PDF file (" & fileName & ").")
            blockComposer.End()

            '/*
            '  NOTE: This statement instructs the PDF viewer to navigate to the page 2 of a PDF file
            '  attached inside the current document as described by the FileAttachment annotation on page 1 of the current document.
            '*/
            'Dim link As New annotations.Link(
            '              page,
            '              New Rectangle(240, 170, 100, 50),
            '              "Link annotation",
            '              New GoToEmbedded(
            '                document,
            '                New GoToEmbedded.PathElement(
            '                  document,
            '                  fileAttachmentPageIndex, // page of the current document containing the file attachment annotation of the target document.
            '                  fileAttachmentName, // Name of the file attachment annotation corresponding to the target document.
            '                  null // No sub-target.
            '                  ), // Target represents the document to go to.
            '                New RemoteDestination(
            '                  document,
            '                  1, // Show the page 2 of the target document.
            '                  Destination.ModeEnum.Fit, // Show the target document page entirely on the screen.
            '                  null,
            '                  null
            '                  ) // The destination must be within the target document.
            '                )
            '              )
            link = New annotations.Link(
                          page,
                          New Rectangle(240, 170, 100, 50),
                          "Link annotation",
                          New GoToEmbedded(
                            document,
                            New GoToEmbedded.PathElement(
                              document,
                              fileAttachmentPageIndex,
                              fileAttachmentName,
                              Nothing
                              ),
                            New RemoteDestination(
                              document,
                              1,
                              Destination.ModeEnum.Fit,
                              Nothing,
                              Nothing
                              )
                            )
                          )
            link.Border = New annotations.Border(
          document,
          1,
          annotations.Border.StyleEnum.Dashed,
          New LineDash(New Double() {8, 5, 2, 5})
          )
            '}

            '/*
            '  2.3. Textual link.
            '*/
            '{
            blockComposer.Begin(New RectangleF(30, 240, 200, 50), XAlignmentEnum.Left, YAlignmentEnum.Middle)
            composer.SetFont(font, 12)
            blockComposer.ShowText("Textual link")
            composer.SetFont(font, 8)
            blockComposer.ShowText(vbLf & "It allows you to expose any kind of link (including the above-mentioned types) as text.")
            composer.SetFont(font, 5)
            blockComposer.ShowText(vbLf & vbLf & "Click on the text links to go either to the project's SourceForge.net repository or to the project's home page.")
            blockComposer.End()

            Try
                composer.BeginLocalState()
                composer.SetFont(font, 10)
                composer.SetFillColor(DeviceRGBColor.Get(System.Drawing.Color.Blue))
                composer.ShowText(
                                "PDF Clown Project's repository at SourceForge.net",
                                New PointF(240, 265),
                                XAlignmentEnum.Left,
                                YAlignmentEnum.Middle,
                                0,
                                New GoToURI(
                                  document,
                                  New Uri("http://www.sourceforge.net/projects/clown")
                                  )
                                )
                composer.ShowText(
                                "PDF Clown Project's home page",
                                New PointF(240, 285),
                                XAlignmentEnum.Left,
                                YAlignmentEnum.Bottom,
                                -90,
                                New GoToURI(
                                  document,
                                  New Uri("http://www.dmdpdf.org")
                                  )
                                )
                composer.End()
            Catch
            End Try
            '}

            composer.Flush()
        End Sub
    End Class
End Namespace
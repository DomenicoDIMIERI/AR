Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.documents.contents.colorSpaces
Imports DMD.org.dmdpdf.documents.contents.composition
Imports DMD.org.dmdpdf.documents.contents.entities
Imports DMD.org.dmdpdf.documents.contents.fonts
Imports DMD.org.dmdpdf.documents.contents.xObjects
Imports DMD.org.dmdpdf.documents.files
Imports DMD.org.dmdpdf.documents.interaction
Imports DMD.org.dmdpdf.documents.interaction.annotations
Imports DMD.org.dmdpdf.files

Imports System
Imports System.Collections.Generic
Imports System.Drawing
Imports System.IO

Namespace org.dmdpdf.samples.cli

    '/**
    '  <summary>This sample demonstrates how to insert annotations into a PDF document.</summary>
    '*/
    Public Class AnnotationSample
        Inherits Sample

        Public Overrides Sub Run()
            ' 1. PDF file instantiation.
            Dim file As DMD.org.dmdpdf.files.File = New DMD.org.dmdpdf.files.File()
            Dim document As Document = file.Document

            ' 2. Content creation.
            Populate(document)

            ' 3. Serialize the PDF file!
            Serialize(file, "Annotations", "inserting annotations", "annotations, creation, attachment, note, callout")
        End Sub

        Private Sub Populate(ByVal document As Document)
            Dim page As Page = New Page(document)
            document.Pages.Add(page)

            Dim composer As PrimitiveComposer = New PrimitiveComposer(page)
            Dim font As StandardType1Font = New StandardType1Font(document, StandardType1Font.FamilyEnum.Courier, True, False)
            composer.SetFont(font, 12)

            ' Note.
            composer.ShowText("Note annotation:", New Point(35, 35))
            Dim note As New annotations.Note(page, New Point(50, 50), "Note annotation")
            note.IconType = Note.IconTypeEnum.Help
            note.ModificationDate = New DateTime()
            note.IsOpen = True

            ' Callout.
            composer.ShowText("Callout note annotation:", New Point(35, 85))
            Dim calloutNote As New CalloutNote(page, New RectangleF(50, 100, 200, 24), "Callout note annotation")
            calloutNote.Justification = JustificationEnum.Right
            calloutNote.Line = New CalloutNote.LineObject(page, New Point(150, 650), New Point(100, 600), New Point(50, 100))

            ' File attachment.
            composer.ShowText("File attachment annotation:", New Point(35, 135))
            Dim attachment As FileAttachment = New FileAttachment(
                                                        page, New RectangleF(50, 150, 12, 12), "File attachment annotation",
                                                        FileSpecification.Get(EmbeddedFile.Get(document, GetResourcePath("images" & Path.DirectorySeparatorChar + "gnu.jpg")), "happyGNU.jpg")
                                                        )
            attachment.IconType = FileAttachment.IconTypeEnum.PaperClip

            composer.BeginLocalState()

            ' Arrow line.
            composer.ShowText("Line annotation:", New Point(35, 185))
            composer.SetFont(font, 10)
            composer.ShowText("Arrow:", New Point(50, 200))
            Dim line As New annotations.Line(
                                        page,
                                        New Point(50, 260),
                                        New Point(200, 210),
                                        "Arrow line annotation"
                                        )
            line.FillColor = New DeviceRGBColor(1, 0, 0)
            line.StartStyle = Line.LineEndStyleEnum.Circle
            line.EndStyle = Line.LineEndStyleEnum.ClosedArrow
            line.CaptionVisible = True

            ' Dimension line.
            composer.ShowText("Dimension:", New Point(300, 200))
            line = New Line(page, New Point(300, 220), New Point(500, 220), "Dimension line annotation")
            line.LeaderLineLength = 20
            line.LeaderLineExtensionLength = 10
            line.CaptionVisible = True

            composer.End()

            ' Scribble.
            composer.ShowText("Scribble annotation:", New Point(35, 285))
            Dim tmpList As IList(Of PointF) = New List(Of PointF)(New PointF() {
                                                                    New PointF(50, 300),
                                                                    New PointF(70, 310),
                                                                    New PointF(100, 320)
                                                                    }
                                                                )
            Dim paths As Object = New List(Of IList(Of PointF))({tmpList})
            Dim tmp As New Scribble(
                                    page, New RectangleF(50, 300, 100, 30), "Scribble annotation",
                                   CType(paths, IList(Of IList(Of PointF)))
                                    )



            ' Rectangle.
            composer.ShowText("Rectangle annotation:", New Point(35, 335))
            Dim rectangle As New annotations.Rectangle(page, New RectangleF(50, 350, 100, 30), "Rectangle annotation")
            rectangle.FillColor = New DeviceRGBColor(1, 0, 0)

            ' Ellipse.
            composer.ShowText("Ellipse annotation:", New Point(35, 385))
            Dim ellipse As New annotations.Ellipse(page, New RectangleF(50, 400, 100, 30), "Ellipse annotation")
            ellipse.FillColor = New DeviceRGBColor(0, 0, 1)

            ' Rubber stamp.
            composer.ShowText("Rubber stamp annotation:", New Point(35, 435))
            Dim tmp1 As New annotations.RubberStamp(page, New RectangleF(50, 450, 100, 30), "Rubber stamp annotation", RubberStamp.IconTypeEnum.Approved)

            ' Caret.
            composer.ShowText("Caret annotation:", New Point(35, 485))
            Dim caret As New annotations.Caret(page, New RectangleF(50, 500, 100, 30), "Caret annotation")
            caret.SymbolType = annotations.Caret.SymbolTypeEnum.NewParagraph

            composer.Flush()
        End Sub

    End Class

End Namespace

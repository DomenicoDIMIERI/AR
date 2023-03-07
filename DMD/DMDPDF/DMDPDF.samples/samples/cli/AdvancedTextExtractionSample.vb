Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.documents.contents
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.tools

Imports System
Imports System.Collections.Generic
Imports System.Drawing

Namespace org.dmdpdf.samples.cli

    '/**
    '  <summary>This sample demonstrates how to retrieve text content along with its graphic attributes
    '  (font, font size, text color, text rendering mode, text bounding box, and so on) from a PDF document;
    '  text is automatically sorted and aggregated.</summary>
    '*/
    Public Class AdvancedTextExtractionSample
        Inherits Sample

        Public Overrides Sub Run()
            ' 1. Opening the PDF file...
            Dim filePath As String = PromptFileChoice("Please select a PDF file")
            Using file As File = New File(filePath)
                Dim document As Document = file.Document

                ' 2. Text extraction from the document pages.
                Dim extractor As TextExtractor = New TextExtractor()
                For Each page As Page In document.Pages
                    If (Not PromptNextPage(page, False)) Then
                        Quit()
                        Exit For
                    End If


                    Dim textStrings As IList(Of ITextString) = extractor.Extract(page)(TextExtractor.DefaultArea)
                    For Each textString As ITextString In textStrings
                        Dim textStringBox As RectangleF = textString.Box.Value
                        Me.OutputLine("Text [" &
                                       "x:" & Math.Round(textStringBox.X) & "," &
                                       "y:" & Math.Round(textStringBox.Y) & "," &
                                       "w:" & Math.Round(textStringBox.Width) & "," &
                                       "h:" & Math.Round(textStringBox.Height) &
                                       "]: " + textString.Text
                                    )
                    Next
                Next
            End Using
        End Sub
    End Class
End Namespace
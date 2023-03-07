Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.documents.contents
Imports DMD.org.dmdpdf.documents.contents.fonts
Imports DMD.org.dmdpdf.documents.contents.objects
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.tools

Imports System
Imports System.Collections.Generic

Namespace org.dmdpdf.samples.cli

    '/**
    '  <summary>This sample demonstrates the low-level way to extract text from a PDF document.</summary>
    '  <remarks>In order to obtain richer information about the extracted text content,
    '  see the other available samples (<see cref="TextInfoExtractionSample"/>,
    '  <see cref="AdvancedTextExtractionSample"/>).</remarks>
    '*/
    Public Class BasicTextExtractionSample
        Inherits Sample

        Public Overrides Sub Run()
            ' 1. Opening the PDF file...
            Dim filePath As String = PromptFileChoice("Please select a PDF file")
            Using file As New File(filePath)
                Dim document As Document = file.Document

                ' 2. Text extraction from the document pages.
                For Each page As Page In document.Pages
                    If (Not PromptNextPage(page, False)) Then
                        Quit()
                        Exit For
                    End If
                    Extract(New ContentScanner(page)) '// Wraps the page contents into a scanner.
                Next
            End Using
        End Sub

        '/**
        '  <summary>Scans a content level looking for text.</summary>
        '*/
        '/*
        '  NOTE: Page contents are represented by a sequence of content objects,
        '  possibly nested into multiple levels.
        '*/
        Private Sub Extract(ByVal level As ContentScanner)
            If (level Is Nothing) Then Return

            While (level.MoveNext())
                Dim content As ContentObject = level.Current
                If (TypeOf (content) Is ShowText) Then
                    If (level.State.Font Is Nothing) Then
                        Debug.Print("oops 6")
                    End If
                    Dim font As Font = level.State.Font
                    ' Extract the current text chunk, decoding it!
                    Me.OutputLine(font.Decode((CType(content, ShowText)).Text))
                ElseIf (TypeOf (content) Is Text OrElse TypeOf (content) Is ContainerObject) Then
                    ' Scan the inner level!
                    Extract(level.ChildLevel)
                End If
            End While
        End Sub

    End Class

End Namespace
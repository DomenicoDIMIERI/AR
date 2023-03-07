Imports DMD.org.dmdpdf.bytes
Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.documents.files
Imports DMD.org.dmdpdf.documents.contents
Imports actions = DMD.org.dmdpdf.documents.interaction.actions
Imports DMD.org.dmdpdf.documents.interaction.annotations
Imports DMD.org.dmdpdf.documents.interaction.navigation.document
Imports files = DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects
Imports DMD.org.dmdpdf.tools

Imports System
Imports System.Collections.Generic
Imports System.Drawing
Imports System.IO
Imports System.Text

Namespace org.dmdpdf.samples.cli

    '/**
    '  <summary>This sample demonstrates how to inspect the links of a PDF document, retrieving
    '  their associated text along with its graphic attributes (font, font size, text color,
    '  text rendering mode, text bounding box...).</summary>
    '  <remarks>According to PDF spec, page text and links have no mutual relation (contrary to, for
    '  example, HTML links), so retrieving the text associated to a link is somewhat tricky
    '  as we have to infer the overlapping areas between links and their corresponding text.</remarks>
    '*/
    Public Class LinkParsingSample
        Inherits Sample

        Public Overrides Sub Run()
            ' 1. Opening the PDF file...
            Dim filePath As String = PromptFileChoice("Please select a PDF file")
            Using file As New files.File(filePath)
                Dim document As Document = file.Document
                ' 2. Link extraction from the document pages.
                Dim extractor As New TextExtractor()
                extractor.AreaTolerance = 2 ' 2 pt tolerance on area boundary detection.
                Dim linkFound As Boolean = False
                For Each page As Page In document.Pages
                    If (Not PromptNextPage(page, Not linkFound)) Then
                        Quit()
                        Exit For 'break;
                    End If

                    Dim textStrings As IDictionary(Of RectangleF?, IList(Of ITextString)) = Nothing
                    linkFound = False

                    ' Get the page annotations!
                    Dim annotations As PageAnnotations = page.Annotations
                    If (Not annotations.Exists()) Then
                        Me.OutputLine("No annotations here.")
                        Continue For
                    End If

                    ' Iterating through the page annotations looking for links...
                    For Each annotation As Annotation In annotations
                        If (TypeOf (annotation) Is Link) Then
                            linkFound = True
                            If (textStrings Is Nothing) Then
                                textStrings = extractor.Extract(page)
                            End If
                            Dim link As Link = CType(annotation, Link)
                            Dim linkBox As RectangleF = link.Box

                            '// Text.
                            '/*
                            '  Extracting Text superimposed by the link...
                            '  NOTE: As links have no strong relation to page text but a weak location correspondence,
                            '  we have to filter extracted text by link area.
                            '*/
                            Dim linkTextBuilder As StringBuilder = New StringBuilder()
                            For Each linkTextString As ITextString In extractor.Filter(textStrings, linkBox)
                                linkTextBuilder.Append(linkTextString.Text)
                            Next
                            Me.OutputLine("Link '" & linkTextBuilder.ToString & "' ")

                            ' Position.
                            Me.OutputLine("    Position: " &
                                              "x:" & Math.Round(linkBox.X) & "," &
                                              "y:" & Math.Round(linkBox.Y) & "," &
                                              "w:" & Math.Round(linkBox.Width) & "," &
                                              "h:" & Math.Round(linkBox.Height)
                                              )

                            ' Target.
                            Me.OutputLine("    Target: ")
                            Dim target As PdfObjectWrapper = link.Target
                            If (TypeOf (target) Is Destination) Then
                                PrintDestination(CType(target, Destination))
                            ElseIf (TypeOf (target) Is actions.Action) Then
                                PrintAction(CType(target, actions.Action))
                            ElseIf (target Is Nothing) Then
                                Me.OutputLine("[not available]")
                            Else
                                Me.OutputLine("[unknown type: " & target.GetType().Name & "]")
                            End If
                        End If
                    Next
                    If (Not linkFound) Then
                        Me.OutputLine("No links here.")
                        Continue For
                    End If
                Next
            End Using
        End Sub

        Private Sub PrintAction(ByVal action As actions.Action)
            '/*
            '  NOTE:                       Here we have to deal with reflection as a workaround
            '  to the lack of type covariance support in C# (so bad -- any better solution?).
            '*/
            Me.OutputLine("Action [" & action.GetType().Name & "] " & action.BaseObject.ToString)
            If (action.Is(GetType(actions.GoToDestination(Of )))) Then
                If (action.Is(GetType(actions.GotoNonLocal(Of )))) Then
                    Dim destinationFile As FileSpecification = CType(action.Get("DestinationFile"), FileSpecification)
                    If (destinationFile IsNot Nothing) Then
                        Me.OutputLine("    Filename: " & destinationFile.Path)
                    End If

                    If (TypeOf (action) Is actions.GoToEmbedded) Then
                        Dim target As actions.GoToEmbedded.PathElement = CType(action, actions.GoToEmbedded).DestinationPath
                        Me.OutputLine("    EmbeddedFilename: " & target.EmbeddedFileName & " Relation: " & target.Relation)
                    End If
                End If
                Me.OutputLine("    ")
                PrintDestination(CType(action.Get("Destination"), Destination))
            ElseIf (TypeOf (action) Is actions.GoToURI) Then
                Me.OutputLine("    URI: " & CType(action, actions.GoToURI).URI.ToString)
            End If
        End Sub

        Private Sub PrintDestination(ByVal destination As Destination)
            Me.OutputLine(destination.GetType().Name & " " & destination.BaseObject.ToString)
            Me.Output("    Page ")
            Dim pageRef As Object = destination.Page
            If (TypeOf (pageRef) Is Page) Then
                Dim page As Page = CType(pageRef, Page)
                Me.OutputLine((page.Index + 1) & " [ID: " & page.BaseObject.ToString & "]")
            Else
                Me.OutputLine(CInt(pageRef) + 1)
            End If
        End Sub

    End Class

End Namespace

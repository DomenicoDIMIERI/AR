Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.documents.contents
Imports DMD.org.dmdpdf.documents.contents.composition
Imports DMD.org.dmdpdf.documents.contents.objects
Imports DMD.org.dmdpdf.documents.interaction.navigation.document
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects

Imports System
Imports System.Collections.Generic
Imports System.Drawing

Namespace org.dmdpdf.samples.cli

    '/**
    '  <summary>This sample demonstrates how to manipulate the named destinations within a PDF document.</summary>
    '*/
    Public Class NamedDestinationSample
        Inherits Sample

        Public Overrides Sub Run()
            ' 1. Opening the PDF file...
            Dim filePath As String = PromptFileChoice("Please select a PDF file")
            Using file As New File(filePath)
                Dim document As Document = file.Document
                Dim pages As Pages = document.Pages

                ' 2. Inserting page destinations...
                Dim destinations As NamedDestinations = document.Names.Destinations
                destinations(New PdfString("d31e1142")) = New LocalDestination(pages(0))
                If (pages.Count > 1) Then
                    destinations(New PdfString("N84afaba6")) = New LocalDestination(pages(1), Destination.ModeEnum.FitHorizontal, 0, Nothing)
                    destinations(New PdfString("d38e1142")) = New LocalDestination(pages(1))
                    destinations(New PdfString("M38e1142")) = New LocalDestination(pages(1))
                    destinations(New PdfString("d3A8e1142")) = New LocalDestination(pages(1))
                    destinations(New PdfString("z38e1142")) = New LocalDestination(pages(1))
                    destinations(New PdfString("f38e1142")) = New LocalDestination(pages(1))
                    destinations(New PdfString("e38e1142")) = New LocalDestination(pages(1))
                    destinations(New PdfString("B84afaba6")) = New LocalDestination(pages(1))
                    destinations(New PdfString("Z38e1142")) = New LocalDestination(pages(1))

                    If (pages.Count > 2) Then
                        destinations(New PdfString("1845505298")) = New LocalDestination(pages(2), Destination.ModeEnum.XYZ, New PointF(50, Single.NaN), Nothing)
                    End If
                End If

                ' 3. Serialize the PDF file!
                Serialize(File, "Named destinations", "manipulating named destinations", "named destinations, creation")
            End Using
        End Sub

    End Class

End Namespace
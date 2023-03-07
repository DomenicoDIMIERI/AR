Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.documents.interaction.navigation.document
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects

Imports System
Imports System.Collections.Generic

Namespace org.dmdpdf.samples.cli

    '/**
    '  <summary>This sample demonstrates how to inspect the object names within a PDF document.</summary>
    '*/
    Public Class NamesParsingSample
        Inherits Sample

        Public Overrides Sub Run()
            ' 1. Opening the PDF file...
            Dim filePath As String = PromptFileChoice("Please select a PDF file")
            Using file As New File(filePath)
                Dim document As Document = file.Document
                ' 2. Named objects extraction.
                Dim names As Names = document.Names
                If (Not names.Exists()) Then
                    Me.OutputLine(vbLf & "No names dictionary.")
                Else
                    Me.OutputLine(vbLf & "Names dictionary found (" & names.DataContainer.Reference.ToString & ")")

                    Dim namedDestinations As NamedDestinations = names.Destinations
                    If (Not namedDestinations.Exists()) Then
                        Me.OutputLine(vbLf & "No named destinations.")
                    Else
                        Me.OutputLine(vbLf & "Named destinations found (" & namedDestinations.DataContainer.Reference.ToString & ")")

                        ' Parsing the named destinations...
                        For Each namedDestination As KeyValuePair(Of PdfString, Destination) In namedDestinations
                            Dim key As PdfString = namedDestination.Key
                            Dim value As Destination = namedDestination.Value

                            Me.OutputLine("  Destination '" & key.ToString & "' (" & value.DataContainer.Reference.ToString & ")")

                            Me.OutputLine("    Target Page: number = ")
                            Dim pageRef As Object = value.Page
                            If (TypeOf (pageRef) Is Int32) Then ' NOTEThen: numeric page refs are typical of remote destinations.
                                Me.OutputLine(CInt(pageRef) + 1)
                            Else ' NOTE: explicit page refs are typical of local destinations.
                                Dim page As Page = CType(pageRef, Page)
                                Me.OutputLine((page.Index + 1) & "; ID = " & CType(page.BaseObject, PdfReference).Id)
                            End If
                        Next

                        Me.OutputLine("Named destinations count = " & namedDestinations.Count)
                    End If
                End If
            End Using

        End Sub
    End Class
End Namespace
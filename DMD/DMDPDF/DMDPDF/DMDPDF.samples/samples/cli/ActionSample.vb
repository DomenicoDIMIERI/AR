Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.documents.contents.composition
Imports DMD.org.dmdpdf.documents.contents.entities
Imports DMD.org.dmdpdf.documents.contents.fonts
Imports DMD.org.dmdpdf.documents.contents.xObjects
Imports DMD.org.dmdpdf.documents.interaction.actions
Imports DMD.org.dmdpdf.documents.interaction.navigation.document
Imports DMD.org.dmdpdf.files

Imports System
Imports System.Collections.Generic
Imports System.Drawing

Namespace org.dmdpdf.samples.cli

    '/**
    '  <summary>This sample demonstrates how to apply actions to a document.</summary>
    '  <remarks>In this case, on document-opening a go-to-page-2 action is triggered;
    '  then on page-2-opening a go-to-URI action is triggered.</remarks>
    '*/
    Public Class ActionSample
        Inherits Sample

        Public Overrides Sub Run()

            ' 1. Opening the PDF file...
            Dim filePath As String = PromptFileChoice("Please select a PDF file")
            Using file As File = New File(filePath)
                Dim document As Document = file.Document
                Dim page As Page = document.Pages(1) ' // Page 2 (zero-based index).

                ' 2. Applying actions...
                ' 2.1. Local go-to.
                '/*
                '  NOTE: This statement instructs the PDF viewer to go to page 2 on document opening.
                '*/
                document.Actions.OnOpen = New GoToLocal(document, New LocalDestination(page)) '  Page 2 (zero-based index).

                ' 2.2. Remote go-to.
                'Try
                '/*
                '  NOTE: This statement instructs the PDF viewer to navigate to the given URI on page 2
                '  opening.
                '*/
                page.Actions.OnOpen = New GoToURI(document, New Uri("https://www.dmdstore.it"))
                'Catch exception As System.Exception
                '    Throw New Exception("Remote goto failed.", exception)
                'End Try

                ' 3. Serialize the PDF file!
                Serialize(file, "Actions", "applying actions", "actions, creation, local goto, remote goto")
            End Using
        End Sub
    End Class

End Namespace

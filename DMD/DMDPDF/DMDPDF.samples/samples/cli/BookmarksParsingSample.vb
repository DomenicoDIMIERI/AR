Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.documents.files
Imports DMD.org.dmdpdf.documents.interaction.actions
Imports DMD.org.dmdpdf.documents.interaction.navigation.document
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects

Imports System

Namespace org.dmdpdf.samples.cli

    '/**
    '  <summary>This sample demonstrates how to inspect the bookmarks of a PDF document.</summary>
    '*/
    Public Class BookmarksParsingSample
        Inherits Sample

        Public Overrides Sub Run()
            ' 1. Opening the PDF file...
            Dim filePath As String = PromptFileChoice("Please select a PDF file")
            Using file As File = New File(filePath)
                Dim document As Document = file.Document

                ' 2. Get the bookmarks collection!
                Dim bookmarks As Bookmarks = document.Bookmarks
                If (Not bookmarks.Exists()) Then
                    Me.OutputLine(vbLf & "No bookmark available (Outline dictionary not found).")
                Else
                    Me.OutputLine(vbLf & "Iterating through the bookmarks collection (please wait)..." & vbLf)
                    ' 3. Show the bookmarks!
                    PrintBookmarks(bookmarks)
                End If
            End Using
        End Sub

        Private Sub PrintBookmarks(ByVal bookmarks As Bookmarks)
            If (bookmarks Is Nothing) Then Return

            For Each bookmark As Bookmark In bookmarks
                ' Show current bookmark!
                Me.OutputLine("Bookmark '" & bookmark.Title & "'")
                Me.OutputLine("    Target: ")
                Dim target As PdfObjectWrapper = bookmark.Target
                If (TypeOf (target) Is Destination) Then
                    PrintDestination(CType(target, Destination))
                ElseIf (TypeOf (target) Is interaction.actions.Action) Then
                    PrintAction(CType(target, interaction.actions.Action))
                ElseIf (target Is Nothing) Then
                    Me.OutputLine("[not available]")
                Else
                    Me.OutputLine("[unknown type: " & target.GetType().Name & "]")
                End If
                ' Show child bookmarks!
                PrintBookmarks(bookmark.Bookmarks)
            Next
        End Sub

        Private Sub PrintAction(ByVal action As interaction.actions.Action)
            '/*
            '  NOTE:       Here we have to deal with reflection as a workaround
            '  to the lack of type covariance support in C# (so bad -- any better solution?).
            '*/
            Me.OutputLine("Action [" & action.GetType().Name & "] " & action.BaseObject.ToString)
            If (GetType(GoToDestination(Of )).IsInstanceOfType(action)) Then
                If (GetType(GotoNonLocal(Of )).IsInstanceOfType(action)) Then
                    Dim destinationFile As FileSpecification = CType(action.Get("DestinationFile"), FileSpecification)
                    If (destinationFile IsNot Nothing) Then
                        Me.OutputLine("    Filename: " & destinationFile.Path)
                    End If

                    If (TypeOf (action) Is GoToEmbedded) Then
                        Dim target As GoToEmbedded.PathElement = CType(action, GoToEmbedded).DestinationPath
                        Me.OutputLine("    EmbeddedFilename: " & target.EmbeddedFileName & " Relation: " & target.Relation)
                    End If
                End If
                Me.OutputLine("    ")
                PrintDestination(CType(action.Get("Destination"), Destination))
            ElseIf (TypeOf (action) Is GoToURI) Then
                Me.OutputLine("    URI: " & CType(action, GoToURI).URI.ToString)
            End If
        End Sub

        Private Sub PrintDestination(ByVal destination As Destination)
            Me.OutputLine(destination.GetType().Name & " " & destination.BaseObject.ToString)
            Me.OutputLine("    Page ")
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
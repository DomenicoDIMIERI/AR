Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.documents.contents
Imports DMD.org.dmdpdf.documents.contents.composition
Imports DMD.org.dmdpdf.documents.contents.objects
Imports DMD.org.dmdpdf.documents.contents.xObjects
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects

Imports System
Imports System.Collections.Generic
Imports System.Drawing

Namespace org.dmdpdf.samples.cli

    '/**
    '  <summary>This sample demonstrates how to retrieve the precise position (page and coordinates)
    '  of each image within a PDF document, using the page content scanning functionality.</summary>
    '  <remarks>This sample leverages the ContentScanner class, a powerful device for accessing
    '  each single content object within a page.</remarks>
    '*/
    Public Class ContentScanningSample
        Inherits Sample

        Public Overrides Sub Run()
            ' 1. Opening the PDF file...
            Dim filePath As String = PromptFileChoice("Please select a PDF file")
            Using file As New File(filePath)
                Dim document As Document = file.Document

                ' 2. Parsing the document...
                Me.OutputLine(vbNewLine & "Looking for images...")
                For Each page As Page In document.Pages
                    Scan(New ContentScanner(page), page)
                Next
            End Using
        End Sub

        '/**
        '  <summary>Scans a content level looking for images.</summary>
        '*/
        '/*
        '  NOTE: Page contents are represented by a sequence of content objects,
        '  possibly nested into multiple levels.
        '*/
        Private Sub Scan(ByVal level As ContentScanner, ByVal page As Page)
            If (level Is Nothing) Then Return

            While (level.MoveNext())
                Dim current As ContentObject = level.Current
                If (TypeOf (current) Is ContainerObject) Then
                    ' Scan the inner level!
                    Scan(level.ChildLevel, page)
                Else
                    Dim objectWrapper As ContentScanner.GraphicsObjectWrapper = level.CurrentWrapper
                    If (objectWrapper Is Nothing) Then Continue While
                    '/*
                    '  NOTE: Images can be represented on a page either as
                    '  external objects (XObject) or inline objects.
                    '*/
                    Dim imageSize As SizeF? = Nothing ' Image native size.
                    If (TypeOf (objectWrapper) Is ContentScanner.XObjectWrapper) Then
                        Dim xObjectWrapper As ContentScanner.XObjectWrapper = CType(objectWrapper, ContentScanner.XObjectWrapper)
                        Dim xObject As xObjects.XObject = xObjectWrapper.XObject
                        ' Is the external object an image?
                        If (TypeOf (xObject) Is xObjects.ImageXObject) Then
                            Me.OutputLine("External Image '" & xObjectWrapper.Name.ToString & "' (" & xObject.BaseObject.ToString & ")") '// Image key And indirect reference.
                            imageSize = xObject.Size ' // Image native size.
                        End If
                    ElseIf (TypeOf (objectWrapper) Is ContentScanner.InlineImageWrapper) Then
                        Me.OutputLine("Inline Image")
                        Dim inlineImage As InlineImage = CType(objectWrapper, ContentScanner.InlineImageWrapper).InlineImage
                        imageSize = inlineImage.Size ' Image native size.
                    End If

                    If (imageSize.HasValue) Then
                        Dim box As RectangleF = objectWrapper.Box.Value ' Image position (location and size) on the page.
                        Me.OutputLine(" on page " & (page.Index + 1) & " (" & page.BaseObject.ToString & ")") '// Page index and indirect reference.
                        Me.OutputLine("  Coordinates:")
                        Me.OutputLine("     x: " & Math.Round(box.X))
                        Me.OutputLine("     y: " & Math.Round(box.Y))
                        Me.OutputLine("     width: " & Math.Round(box.Width) & " (native: " & Math.Round(imageSize.Value.Width) & ")")
                        Me.OutputLine("     height: " & Math.Round(box.Height) & " (native: " & Math.Round(imageSize.Value.Height) & ")")
                    End If
                End If
            End While
        End Sub
    End Class
End Namespace
Imports DMD.org.dmdpdf.bytes
Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.documents.contents
Imports DMD.org.dmdpdf.documents.contents.entities
Imports DMD.org.dmdpdf.documents.contents.xObjects
Imports files = DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects

Imports System
Imports System.IO
Imports System.Linq

Namespace org.dmdpdf.samples.cli

    '/**
    '  <summary>This sample demonstrates how to replace images appearing in a PDF document's pages
    '  through their resource names.</summary>
    '*/
    Public Class ImageSubstitutionSample
        Inherits Sample

        Public Overrides Sub Run()
            ' 1. Opening the PDF file...
            Dim filePath As String = PromptFileChoice("Please select a PDF file")
            Using file As New files.File(filePath)
                Dim document As Document = file.Document

                ' 2. Replace the images!
                ReplaceImages(document)

                ' 3. Serialize the PDF file!
                Serialize(file, "Image substitution", "substituting a document's images", "image replacement")
            End Using
        End Sub

        Private Sub ReplaceImages(ByVal document As Document)
            ' Get the image used to replace existing ones!
            Dim image As Image = image.Get(GetResourcePath("images" & Path.DirectorySeparatorChar & "gnu.jpg")) ' Image is an abstract entity, as it still has to be included into the pdf document
            ' Add the image to the document!
            Dim ImageXObject As XObject = image.ToXObject(document) ' XObject (i.e. external object) Is, in PDF spec jargon, a reusable object.
            ' Looking for images to replace...
            For Each page As Page In document.Pages
                Dim resources As Resources = page.Resources
                Dim xObjects As XObjectResources = resources.XObjects
                If (xObjects Is Nothing) Then Continue For

                For Each xObjectKey As PdfName In xObjects.Keys.ToList()
                    Dim xObject As XObject = xObjects(xObjectKey)
                    ' Is the page's resource an image?
                    If (TypeOf (xObject) Is ImageXObject) Then
                        Me.outputline("Substituting " & xObjectKey.ToString & " image xobject.")
                        xObjects(xObjectKey) = ImageXObject
                    End If
                Next
            Next
        End Sub

    End Class
End Namespace
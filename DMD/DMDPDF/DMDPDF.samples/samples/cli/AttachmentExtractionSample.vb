Imports DMD.org.dmdpdf.bytes
Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.documents.files
Imports DMD.org.dmdpdf.documents.interaction.annotations
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects

Imports System
Imports System.Collections.Generic
Imports System.IO

Namespace org.dmdpdf.samples.cli

    '/**
    '  <summary>This sample demonstrates how to extract attachments from a PDF document.</summary>
    '*/
    Public Class AttachmentExtractionSample
        Inherits Sample

        Public Overrides Sub Run()
            ' 1. Opening the PDF file...
            Dim filePath As String = PromptFileChoice("Please select a PDF file")
            Using file As New DMD.org.dmdpdf.files.File(filePath)
                Dim document As Document = file.Document

                ' 2. Extracting attachments...
                ' 2.1. Embedded files (document level).
                For Each entry As KeyValuePair(Of PdfString, FileSpecification) In document.Names.EmbeddedFiles
                    EvaluateDataFile(entry.Value)
                Next

                ' 2.2. File attachments (page level).
                For Each page As Page In document.Pages
                    For Each annotation As Annotation In page.Annotations
                        If (TypeOf (annotation) Is FileAttachment) Then
                            EvaluateDataFile(CType(annotation, FileAttachment).DataFile)
                        End If
                    Next
                Next
            End Using
        End Sub

        Private Sub EvaluateDataFile(ByVal dataFile As FileSpecification)
            If (TypeOf (dataFile) Is FullFileSpecification) Then
                Dim embeddedFile As EmbeddedFile = CType(dataFile, FullFileSpecification).EmbeddedFile
                If (embeddedFile IsNot Nothing) Then
                    ExportAttachment(embeddedFile.Data, dataFile.Path)
                End If
            End If
        End Sub

        Private Sub ExportAttachment(ByVal data As IBuffer, ByVal filename As String)
            Dim outputPath As String = GetOutputPath(filename)
            Dim outputStream As FileStream
#If Not DEBUG Then
            Try
#End If
            outputStream = New FileStream(outputPath, FileMode.CreateNew)
#If Not DEBUG Then
            Catch e As Exception
                Throw New Exception(outputPath & " file couldn't be created.", e)
            End Try
#End If

#If Not DEBUG Then
            Try
#End If
            Dim writer As BinaryWriter = New BinaryWriter(outputStream)
            writer.Write(data.ToByteArray())
            writer.Close()
            outputStream.Close()
#If Not DEBUG Then
            Catch e As Exception
                Throw New Exception(outputPath & " file writing has failed.", e)
            End Try
#End If
            Me.OutputLine("Output: " & outputPath)
        End Sub

    End Class
End Namespace
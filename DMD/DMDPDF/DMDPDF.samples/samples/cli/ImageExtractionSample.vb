Imports DMD.org.dmdpdf.bytes
Imports DMD.org.dmdpdf.documents
Imports files = DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects

Imports System
Imports System.IO

Namespace org.dmdpdf.samples.cli

    '/**
    '  <summary>This sample demonstrates how to extract XObject images from a PDF document.</summary>
    '  <remarks>
    '    <para>Inline images are ignored.</para>
    '    <para>XObject images other than JPEG aren't currently supported for handling.</para>
    '  </remarks>
    '*/
    Public Class ImageExtractionSample
        Inherits Sample

        Public Overrides Sub Run()
            ' 1. Opening the PDF file...
            Dim filePath As String = PromptFileChoice("Please select a PDF file")
            Using file As New files.File(filePath)
                ' 2. Iterating through the indirect object collection...
                Dim index As Integer = 0
                For Each indirectObject As PdfIndirectObject In file.IndirectObjects
                    ' Get the data object associated to the indirect object!
                    Dim dataObject As PdfDataObject = indirectObject.DataObject
                    ' Is this data object a stream?
                    If (TypeOf (dataObject) Is PdfStream) Then
                        Dim header As PdfDictionary = CType(dataObject, PdfStream).Header
                        ' Is this stream an image?
                        If (header.ContainsKey(PdfName.Type) AndAlso
                           header(PdfName.Type).Equals(PdfName.XObject) AndAlso
                           header(PdfName.Subtype).Equals(PdfName.Image)) Then
                            ' Which kind of image?
                            If (header(PdfName.Filter).Equals(PdfName.DCTDecode)) Then ' JPEG  image.
                                ' Get the image data (keeping it encoded)!
                                Dim body As IBuffer = CType(dataObject, PdfStream).GetBody(False)
                                ' Export the image!
                                ExportImage(body, "ImageExtractionSample_" & (index + 1) & ".jpg") : index += 1

                            Else ' Unsupported image.
                                Me.OutputLine("Image XObject " & indirectObject.Reference.ToString & " couldn't be extracted (filter: " & header(PdfName.Filter).ToString & ")")
                            End If
                        End If
                    End If
                Next
            End Using
        End Sub

        Private Sub ExportImage(ByVal Data As IBuffer, ByVal filename As String)
            Dim OutputPath As String = GetOutputPath(filename)
            Dim outputStream As FileStream
            Try
                outputStream = New FileStream(OutputPath, FileMode.CreateNew)
            Catch e As System.Exception
                Throw New Exception(OutputPath & " file couldn't be created.", e)
            End Try

            Try
                Dim writer As BinaryWriter = New BinaryWriter(outputStream)
                writer.Write(Data.ToByteArray())
                writer.Close()
                outputStream.Close()
            Catch e As System.Exception
                Throw New Exception(OutputPath & " file writing has failed.", e)
            End Try

            Me.OutputLine("Output: " & OutputPath)
        End Sub

    End Class
End Namespace

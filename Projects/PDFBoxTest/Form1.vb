Imports FinSeA.org.apache.pdfbox.pdmodel
Imports FinSeA.org.apache.pdfbox.pdmodel.font
Imports FinSeA.org.apache.pdfbox.pdmodel.edit


Public Class Form1

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ' Create a document and add a page to it
        Dim document As New PDDocument
        Dim page As New PDPage()
        document.addPage(page)

        ' Create a new font object selecting one of the PDF base fonts
        Dim font As PDFont = PDType1Font.HELVETICA_BOLD

        ' Start a new content stream which will "hold" the to be created content
        Dim contentStream As New PDPageContentStream(document, page)

        ' Define a text content stream using the selected font, moving the cursor and drawing the text "Hello World"
        contentStream.beginText()
        contentStream.setFont(font, 12)
        contentStream.moveTextPositionByAmount(100, 700)
        contentStream.drawString("Hello World")
        contentStream.endText()

        ' Make sure that the content stream is closed:
        contentStream.close()

        ' Save the results and ensure that the document is properly closed:
        Dim ofd As New OpenFileDialog
        ofd.Filter = "File PDF|*.pdf"
        If (ofd.ShowDialog = Windows.Forms.DialogResult.OK) Then
            document.save(ofd.FileName)
            document.close()
        End If
        ofd.Dispose()
    End Sub
End Class

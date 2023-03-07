Public NotInheritable Class MSOfficeUtils

    Public Shared Function DocToImage(ByVal fileName As String, ByVal page As Integer, Optional ByVal pageCount As Integer = 1, Optional ByVal format As String = "jpg") As System.Drawing.Image
        'Dim pdf As New Spire.Doc.Document
        'Dim fmt As Spire.Doc.Documents.ImageType
        'pdf.LoadFromFile(fileName)
        'Select Case LCase(Trim(format))
        '    Case "jpg", "jpeg" : fmt = Spire.Doc.Documents.ImageType.Jpeg
        '    Case "bmp" : fmt = Spire.Doc.Documents.ImageType.Bitmap
        '    Case "emf" : fmt = Spire.Doc.Documents.ImageType.Emf
        '    Case "wmf" : fmt = Spire.Doc.Documents.ImageType.Metafile
        '    Case "pic" : fmt = Spire.Doc.Documents.ImageType.Pict
        '    Case "png" : fmt = Spire.Doc.Documents.ImageType.Png
        '    Case "tif", "tiff" : fmt = Spire.Doc.Documents.ImageType.Tiff
        '    Case "xaml" : fmt = Spire.Doc.Documents.ImageType.Xaml
        '    Case Else : fmt = Spire.Doc.Documents.ImageType.Jpeg
        'End Select
        'Dim ret As System.Drawing.Image() = pdf.SaveToImages(page, pageCount, fmt)
        'pdf.Dispose()
        'Return ret(0)

        Return Nothing
    End Function
End Class


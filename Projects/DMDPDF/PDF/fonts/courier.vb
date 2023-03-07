Namespace PDF.Fonts

    Friend Class courier
        Inherits PDFFont

        Public Sub New()
            'MyBase.New("", "courier",
            '    for xi as Integer =0 To 255 
            '    xfpdf_charwidths(courier)(chr(xi))=600
            '    xfpdf_charwidths["courierB"]=xfpdf_charwidths["courier"]
            'xfpdf_charwidths["courierI"]=xfpdf_charwidths["courier"]
            'xfpdf_charwidths["courierBI"]=xfpdf_charwidths["courier"]
            'next 
            Me.xname = "Courier"
        End Sub

    End Class

    Friend Class courierBold
        Inherits courier

        Public Sub New()
            Me.xname = "Courier-Bold"
        End Sub

    End Class

    Friend Class CourierOblique
        Inherits courier

        Public Sub New()
            Me.xname = "Courier-Oblique"
        End Sub

    End Class

    Friend Class CourierBoldOblique
        Inherits courier

        Public Sub New()
            Me.xname = "Courier-BoldOblique"
        End Sub

    End Class

End Namespace

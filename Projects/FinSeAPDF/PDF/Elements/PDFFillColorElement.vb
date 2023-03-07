Namespace PDF.Elements


    Public Class PDFFillColorElement
        Inherits PDFColorElement


        Public Sub New()
        End Sub

        Public Sub New(ByVal r As Single, ByVal g As Single, ByVal b As Single)
            MyBase.New(r, g, b)
        End Sub

        Public Sub New(ByVal r As Byte, ByVal g As Byte, ByVal b As Byte)
            MyBase.New(r, g, b)
        End Sub

        Public Sub New(ByVal color As System.Drawing.Color)
            MyBase.New(color)
        End Sub

        Friend Overrides Sub Write(writer As PDFWriter)
            writer.SetFillColor(Me.Red, Me.Green, Me.Blue)
        End Sub
    End Class


End Namespace
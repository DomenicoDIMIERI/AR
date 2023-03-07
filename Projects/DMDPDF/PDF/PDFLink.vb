Namespace PDF

    Public Class PDFLink
        Public xlink As String
        Public xpage As Integer
        Public xy As Single

        Public Sub New()
        End Sub

        Public Sub New(ByVal xlink As String, ByVal xpage As Integer, ByVal xy As Single)
            Me.xlink = xlink
            Me.xpage = xpage
            Me.xy = xy
        End Sub

    End Class

End Namespace

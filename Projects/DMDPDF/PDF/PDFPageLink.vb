Namespace PDF

    Public Class PDFPageLink
        Public x As Single
        Public y As Single
        Public w As Single
        Public h As Single
        Public xlink As String
        Public xpage As Integer

        Public Sub New()
        End Sub

        Public Sub New(ByVal x As Single, ByVal y As Single, ByVal w As Single, ByVal h As Single, ByVal xlink As String)
            Me.x = x
            Me.y = y
            Me.w = w
            Me.h = h
            Me.xlink = xlink
        End Sub

    End Class

End Namespace
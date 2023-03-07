Namespace PDF

    Public Class PDFBookmark
        Public t As String  'text
        Public l As Integer 'level
        Public y As Single 'xy
        Public p As Integer 'Page No
        Public parent As Integer
        Public last As Integer
        Public first As Integer
        Public [next] As Integer
        Public prev As Integer

        Public Sub New()
        End Sub

        Public Sub New(ByVal t As String, ByVal l As Integer, ByVal y As Single, ByVal p As Integer)
            Me.t = t
            Me.l = l
            Me.y = y
            Me.p = p
        End Sub

    End Class

End Namespace

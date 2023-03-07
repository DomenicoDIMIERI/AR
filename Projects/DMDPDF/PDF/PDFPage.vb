Namespace PDF

    Public Class PDFPage
        Private m_buffer As New System.Text.StringBuilder(2048)

        Public Sub New()
        End Sub

        Friend Sub _out(ByVal buffer As String)
            Me.m_buffer.Append(buffer & vbLf)
        End Sub

        Friend Sub ReplaceBuffer(ByVal oldValue As String, ByVal newValue As String)
            Me.m_buffer.Replace(oldValue, newValue)
        End Sub

        Friend Function GetBuffer() As String
            Return Me.m_buffer.ToString
        End Function

    End Class

End Namespace

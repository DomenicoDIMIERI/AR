Namespace Text

    Public Class StringBuffer

        Private m_Buffer As System.Text.StringBuilder

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_Buffer = New System.Text.StringBuilder
        End Sub

        Sub New(ByVal initVal As String)
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_Buffer = New System.Text.StringBuilder(initVal)
        End Sub

        Public Sub append(p1 As Char)
            Me.m_Buffer.Append(p1)
        End Sub

        Function length() As Integer
            Return Me.m_Buffer.Length
        End Function

        Function substring(i As Integer, p2 As Integer) As String
            Return Me.m_Buffer.ToString(i, p2)
        End Function

        Public Sub appendCodePoint(ByVal letter As Integer)
            Me.m_Buffer.Append(Chr(letter))
        End Sub

        Sub deleteCharAt(p1 As Integer)
            Me.m_Buffer.Remove(p1, 1)
        End Sub

        Public Overrides Function ToString() As String
            Return Me.m_Buffer.ToString
        End Function

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Namespace
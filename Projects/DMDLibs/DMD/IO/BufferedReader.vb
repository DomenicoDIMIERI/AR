Namespace Io

    Public Class BufferedReader
        Inherits DMD.Io.Reader

        Private m_StreamReader As InputStreamReader
        Private m_lastByte As Nullable(Of Char)

        Sub New(ByVal reader As InputStreamReader)
            Me.m_StreamReader = reader
        End Sub

        ''' <summary>
        ''' Reads a line of text.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function readLine() As String
            Dim ret As New System.Text.StringBuilder
            Me.m_lastByte = Nothing
            Do
                Dim cCode As Integer = Me.read
                If (cCode < 0) Then Exit Do
                If (cCode = 10) Then Exit Do 'Line feed
                If (cCode = 13) Then
                    cCode = Me.read
                    If (cCode = -1) Then Exit Do
                    If (cCode = 10) Then Exit Do
                    Me.m_lastByte = Convert.ToChar(cCode)
                    Exit Do
                End If
                ret.Append(Convert.ToChar(cCode))
            Loop
            Return ret.ToString
        End Function

        Public Overrides Sub close()
            Me.m_StreamReader.close()
        End Sub

        Public Overloads Overrides Function read(cbuf() As Char, off As Integer, len As Integer) As Integer
            Dim ret As Integer
            If (Me.m_lastByte.HasValue) Then
                cbuf(off) = Me.m_lastByte.Value
                ret = 1 + Me.m_StreamReader.read(cbuf, off + 1, len - 1)
                Me.m_lastByte = Nothing
            Else
                ret = Me.m_StreamReader.read(cbuf, off, len)
            End If
            Return ret
        End Function
    End Class

End Namespace
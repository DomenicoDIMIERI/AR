Partial Public Class PDF

    Public Class PDFWriter
        Private m_Buffer As New System.Text.StringBuilder(2048)
        Private m_PDF As PDF
        Private pdfFillColor As String
        Private pdfTextColor As String
        Private ColorFlag As Boolean

        Public Sub New(ByVal pdf As PDF)
            Me.m_PDF = pdf
        End Sub

        Friend ReadOnly Property PDF As PDF
            Get
                Return Me.m_PDF
            End Get
        End Property

        Public Sub SetTextColor(ByVal xr As Byte, ByVal xg As Byte, ByVal xb As Byte)
            If (xr = xg) And (xr = xb) Then
                Me.pdfTextColor = [lib].sprintf("%.3f g", xr / 255)
            Else
                Me.pdfTextColor = [lib].sprintf("%.3f %.3f %.3f rg", xr / 255, xg / 255, xb / 255)
            End If
            Me.ColorFlag = (Me.pdfFillColor <> Me.pdfTextColor)
        End Sub


        Public Sub SetFillColor(ByVal xr As Byte, ByVal xg As Byte, ByVal xb As Byte)
            If (xr = xg) And (xr = xb) Then
                Me.pdfFillColor = [lib].sprintf("%.3f g", xr / 255)
            Else
                Me.pdfFillColor = [lib].sprintf("%.3f %.3f %.3f rg", xr / 255, xg / 255, xb / 255)
            End If
            Me.ColorFlag = (Me.pdfFillColor <> Me.pdfTextColor)
            If (Me.page > 0) Then Me._out(Me.pdfFillColor)
        End Sub

        Public Sub WriteRowData(ByVal buffer As String)
            Me.m_Buffer.Append(buffer & vbLf)
        End Sub

        Public Sub Write(ByVal element As PDFElement)
            element.Write(Me)
        End Sub

    End Class

End Class
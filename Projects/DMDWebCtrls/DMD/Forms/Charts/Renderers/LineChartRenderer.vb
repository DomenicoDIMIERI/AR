Imports DMD
Imports DMD.Sistema
Imports DMD.Forms


Namespace Forms
 
  
    Public Class LineChartRenderer
        Inherits ChartRenderer

        Dim maxNumXSerie As Integer

        Public Sub New(ByVal chart As CChart)
            MyBase.New(chart)
        End Sub


        Public Overrides ReadOnly Property Description As String
            Get
                Return "Linee"
            End Get
        End Property

        Private Function MeasureYAxeText() As Single
            Using font As System.Drawing.Font = Me.GetXAxeFont
                Try
                    Dim l As Integer = 1
                    Dim s As Single = 1
                    Dim n1 As Integer = 1
                    Dim n2 As Integer = 1
                    Dim ret As Single = 0
                    Dim m As ChartMeasure = Me.Chart.GetMeasures
                    If (m.MaxY.HasValue AndAlso Math.Abs(m.MaxY.Value) > 0) Then
                        l = Math.Log10(Math.Abs(m.MaxY.Value)) ' - m.MinY)
                        s = Math.Sign(m.MaxY.Value) * 10 ^ (l - 1)
                        If (s > 0) Then
                            n1 = Math.Max(0, Math.Floor(m.MaxY.Value / s))
                            n2 = Math.Min(0, Math.Floor(m.MinY.Value / s))
                        End If
                    End If

                    For i As Integer = n2 To n1
                        If Me.Chart.YAxe.ShowValues Then
                            Dim str As String = Formats.FormatNumber(i * s, 2)
                            Dim ts As System.Drawing.SizeF = Me.Context.MeasureString(str, font)
                            ret = Math.Max(ts.Width / Me.xScale, ret)
                        End If
                    Next

                    Return ret
                Catch ex As Exception
                    Sistema.Events.NotifyUnhandledException(ex)
                    Throw
                Finally

                End Try
            End Using
        End Function

        Protected Overrides Sub CalculateParameters()
            Dim init As Boolean = False

            For Each serie As CChartSerie In Me.Chart.Series
                Dim ms As ChartMeasure = serie.GetMeasures
                If (init = False) Then
                    Me.minY = ms.MinY
                    Me.maxY = ms.MaxY
                    init = True
                Else
                    If (ms.MinY < Me.minY) Then Me.minY = ms.MinY
                    If (ms.MaxY > Me.maxY) Then Me.maxY = ms.MaxY
                End If
                maxNumXSerie = Math.Max(serie.Values.Count, maxNumXSerie)
            Next

            Me.minY = Math.Min(0, Me.minY)
            Me.maxY = Math.Max(0, Me.maxY)

            maxNumXSerie = Math.Max(1, maxNumXSerie)

            Dim W As Single = Me.renderRect.Width
            Dim N As Integer = maxNumXSerie
            
            'Me.Chart.Labels.GetMinMax(Me.minX, Me.maxX)
            Me.minX = 0
            Me.maxX = maxNumXSerie

            Me.xScale = 1 : Me.yScale = 1
            If (Me.minY.HasValue) Then
                If (Me.minY * Me.maxY > 0) Then
                    Me.yScale = (Me.renderRect.Height - 5) / Me.maxY
                ElseIf Me.maxY > 0 Then
                    Me.yScale = (Me.renderRect.Height - 5) / (Me.maxY - Me.minY)
                End If
                Me.yOrig = -Me.minY
            Else
                Me.yOrig = 0
            End If

            If (Me.minX * Me.maxX > 0) Then
                Me.xScale = (Me.renderRect.Width - 5) / Me.maxX
            ElseIf Me.maxX > 0 Then
                Me.xScale = ((Me.renderRect.Width - 5) / (Me.maxX - Me.minX))
            End If
            Me.xOrig = Math.Max(Me.MeasureYAxeText, -Me.minX)

        End Sub

        Private Sub DrawSerieAsLines(ByVal serie As CChartSerie)
            Using font As System.Drawing.Font = Me.GetXAxeFont
                Dim color As System.Drawing.Color
                Dim pen As System.Drawing.Pen
                Dim value As CChartValue = Nothing
                Dim oldValue As CChartValue = Nothing
                If (serie.Values.Count < 1) Then Exit Sub

                color = serie.GetRenderColor
                pen = New System.Drawing.Pen(color, 2)

                oldValue = serie.Values(0)
                If (oldValue.Value.HasValue) Then
                    Me.DrawEllipse(pen, 0, oldValue.Value, 4 / Me.xScale, 4 / Me.yScale)
                    If (oldValue.Label <> "") Then Me.DrawString(oldValue.Label, font, Drawing.Brushes.Black, Me.xOrig + 5 / Me.xScale, oldValue.Value + 5 / Me.yScale)

                End If
                For i As Integer = 1 To serie.Values.Count - 1
                    value = serie.Values(i)
                    If (oldValue.Value.HasValue) Then
                        If (value.Value.HasValue) Then
                            Me.DrawLine(pen, (i - 1), oldValue.Value, i, value.Value)
                            Me.DrawEllipse(pen, i, value.Value, 4 / Me.xScale, 4 / Me.yScale)
                        End If
                        If (oldValue.Label <> "") Then Me.DrawString(oldValue.Label, font, Drawing.Brushes.Black, Me.xOrig + i + 5 / Me.xScale, oldValue.Value + 5 / Me.yScale)
                    End If
                    oldValue = value

                Next

                pen.Dispose()

            End Using

        End Sub

        Private Sub DrawSerieAsBars(ByVal serie As CChartSerie)
            Using font As System.Drawing.Font = Me.GetXAxeFont
                Dim color As System.Drawing.Color
                Dim brush As System.Drawing.Brush
                Dim value As CChartValue = Nothing
                If (serie.Values.Count < 1) Then Exit Sub

                color = serie.GetRenderColor
                brush = New System.Drawing.SolidBrush(color)

                For i As Integer = 0 To serie.Values.Count - 1
                    value = serie.Values(i)
                    If (value.Value.HasValue) Then
                        Me.FillRectangle(brush, i + 0.33, 0, 0.34, value.Value)
                        If (value.Label <> "") Then Me.DrawString(value.Label, font, Drawing.Brushes.Black, i + 0.33 + 3 / Me.xScale, value.Value + 3 / Me.yScale)
                    End If
                Next
                brush.Dispose()
            End Using
        End Sub

        Protected Overrides Sub DrawSerie(serie As CChartSerie)
            Select Case serie.Tipo
                Case ChartTypes.Lines : Me.DrawSerieAsLines(serie)
                Case ChartTypes.VerticalBarsOverlapped : Me.DrawSerieAsBars(serie)
                Case Else
                    Me.DrawSerieAsLines(serie)
            End Select
        End Sub

        Protected Overrides Sub DrawXAxe()
            Using font As System.Drawing.Font = Me.GetXAxeFont
                Dim i As Integer = 0
                Dim dx As Single = 1
                Dim m As ChartMeasure = Me.Chart.GetMeasures

                For Each label As CChartLabel In Me.Chart.Labels
                    Dim str As String = label.Label
                    Dim ts As System.Drawing.SizeF = Me.Context.MeasureString(str, font)

                    If (Me.Chart.XAxe.ShowValues) Then Me.DrawString(str, font, Drawing.Brushes.Black, (Me.xOrig + 2) / xScale + i * dx, (yOrig - 1) / yScale)

                    'Me.Context.DrawString(label.Label, font, Drawing.Brushes.Black, New System.Drawing.Rectangle(Me.xAxeRect.Left + x * Me.xScale, Me.xAxeRect.Top, dx, Me.xAxeRect.Height))
                    i += 1
                Next
                'MyBase.DrawXAxe()
            End Using
        End Sub



        Protected Overrides Sub DrawYAxe()
            MyBase.DrawYAxe()
        End Sub

        Public Overrides ReadOnly Property Type As ChartTypes
            Get
                Return ChartTypes.Lines
            End Get
        End Property
    End Class

End Namespace
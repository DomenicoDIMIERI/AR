Imports DMD
Imports DMD.Sistema
Imports DMD.Forms


Namespace Forms
 
  
    Public Class VerticalBarChartMultibarsRenderer
        Inherits VerticalBarOverlappedChartRenderer

        Public GroupDistance As Single = 5
        Private GroupWidth As Single
        Private maxNumXSerie As Integer = 0

        Public Sub New(ByVal chart As CChart)
            MyBase.New(chart)
        End Sub

      



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
            maxNumXSerie = Math.Max(1, maxNumXSerie)

            Dim W As Single = Me.renderRect.Width
            Dim N As Integer = maxNumXSerie
            Dim GS As Single = Me.GroupDistance

            Me.GroupWidth = ((W + GS) / N) - GS
            Me.m_BarWidth = ((Me.GroupWidth + Me.barDistance) / Math.Max(1, Me.Chart.Series.Count)) - Me.barDistance

            'Me.Chart.Labels.GetMinMax(Me.minX, Me.maxX)
            Me.minX = 0
            Me.maxX = Me.GroupWidth * Me.Chart.Labels.Count + (Me.GroupDistance * 2 * Me.Chart.Labels.Count - 1)

            Me.xScale = 1 : Me.yScale = 1
            If (Me.minY * Me.maxY > 0) Then
                Me.yScale = (Me.renderRect.Height - 5) / Me.maxY
            ElseIf Me.maxY > 0 Then
                Me.yScale = (Me.renderRect.Height - 5) / (Me.maxY - Me.minY)
            End If
            'If (Me.minX * Me.maxX > 0) Then
            '    Me.xScale = (Me.renderRect.Width - 5) / Me.maxX
            'ElseIf Me.maxX > 0 Then
            '    Me.xScale = ((Me.renderRect.Width - 5) / (Me.maxX - Me.minX))
            'End If
            Me.xOrig = -Me.minX
            Me.yOrig = -Me.minY
        End Sub

        Public Overrides ReadOnly Property Type As ChartTypes
            Get
                Return ChartTypes.VerticalBarsMultibars
            End Get
        End Property

        Protected Overrides Sub DrawSeries()
            Me.CalculateParameters()
             
            Me.Context.DrawRectangle(Drawing.Pens.Red, Me.renderRect)
            For tmp As Single = 0 To renderRect.Width Step (GroupWidth + GroupDistance)
                Me.Context.DrawLine(Drawing.Pens.Red, renderRect.Left + tmp, renderRect.Top, renderRect.Left + tmp, renderRect.Bottom)
            Next

            Dim x As Single
            Dim i As Integer
            Dim serie As CChartSerie
            Dim color As System.Drawing.Color
            Dim brush As System.Drawing.Brush
            Dim value As CChartValue

            Dim font As New System.Drawing.Font("Tahoma", 10, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
            For j As Integer = 0 To Me.Chart.Series.Count - 1
                serie = Me.Chart.Series(j)
                color = serie.GetRenderColor
                brush = New System.Drawing.SolidBrush(color)

                
                For i = 0 To serie.Values.Count - 1
                    x = i * (Me.GroupWidth + Me.GroupDistance) + j * (Me.barDistance + Me.BarWidth)

                    value = serie.Values(i)
                    Me.FillRectangle(brush, x, 0, Me.BarWidth, value.Value)
                    Me.DrawRectangle(Drawing.Pens.Black, x, 0, Me.BarWidth, value.Value)

                    'Me.TranslateTransform(x, value.Value + 1 * yScale)
                    'Me.RotateTransform(90)
                    Dim str As String = Formats.FormatValuta(value.Value)
                    Dim ts As System.Drawing.SizeF = Me.Context.MeasureString(Str, font)
                    If (serie.ShowLabels) Then Me.DrawString(str, font, Drawing.Brushes.Black, x, value.Value + (ts.Height + 1) / yScale)
                    'Me.RotateTransform(-90)
                    'Me.TranslateTransform(-x, -value.Value - 1 * yScale)
                Next

                brush.Dispose()
            Next
            font.Dispose()
        End Sub

        Protected Overrides Sub DrawXAxe()
            Dim i As Integer = 0
            Dim dx As Single = Me.GroupWidth + Me.GroupDistance
            Dim font As New System.Drawing.Font("Tahoma", 10, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
            Dim m As ChartMeasure = Me.Chart.GetMeasures

            For Each label As CChartLabel In Me.Chart.Labels
                Dim str As String = label.Label
                Dim ts As System.Drawing.SizeF = Me.Context.MeasureString(str, font)

                If (Me.Chart.XAxe.ShowValues) Then Me.DrawString(str, font, Drawing.Brushes.Black, 1 / xScale + i * dx, yOrig - 1 / yScale)

                'Me.Context.DrawString(label.Label, font, Drawing.Brushes.Black, New System.Drawing.Rectangle(Me.xAxeRect.Left + x * Me.xScale, Me.xAxeRect.Top, dx, Me.xAxeRect.Height))
                i += 1
            Next
            font.Dispose()

            'MyBase.DrawXAxe()
        End Sub

        Protected Overrides Sub DrawYAxe()
            MyBase.DrawYAxe()
        End Sub


    End Class

End Namespace
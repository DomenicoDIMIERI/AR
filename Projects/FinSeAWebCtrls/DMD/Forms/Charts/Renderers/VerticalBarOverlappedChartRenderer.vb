Imports DMD
Imports DMD.Sistema
Imports DMD.Forms


Namespace Forms
 

    ''' <summary>
    ''' Renderer di un grafico a barre verticali
    ''' </summary>
    ''' <remarks></remarks>
    Public Class VerticalBarOverlappedChartRenderer
        Inherits ChartRenderer

        'Protected barWidth As Single
        Protected barDistance As Single
        'Private m_Subformat As BarChartFormats
        Protected m_BarWidth As Single


        Public Sub New(ByVal chart As CChart)
            MyBase.New(chart)
            '   Me.barWidth = 10
            Me.barDistance = 2
        End Sub

        Public Overrides ReadOnly Property Description As String
            Get
                Return "Vertical bars"
            End Get
        End Property

        Public ReadOnly Property BarWidth As Single
            Get
                Return Me.m_BarWidth
            End Get
        End Property



        Protected Overrides Sub DrawSerie(ByVal serie As CChartSerie)
            Dim x As Single
            Dim color As System.Drawing.Color
            Dim brush As System.Drawing.Brush
            x = 0
            color = serie.GetRenderColor
            brush = New System.Drawing.SolidBrush(color)
            For Each value As CChartValue In serie.Values
                Me.FillRectangle(brush, x, 0, Me.BarWidth, value.Value)
                Me.DrawRectangle(Drawing.Pens.Black, x, 0, Me.BarWidth, value.Value)
                x += Me.BarWidth + Me.barDistance
            Next
            brush.Dispose()
        End Sub

        Protected Overrides Sub CalculateParameters()
            Dim init As Boolean = False
            Dim maxNumXSerie As Integer = 0

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

            Me.m_BarWidth = Me.renderRect.Width / (maxNumXSerie * Me.Chart.Series.Count)

            Me.minX = 0
            Me.maxX = Me.Chart.Labels.Count
            For Each serie As CChartSerie In Me.Chart.Series
                If serie.Values.Count > Me.maxX Then Me.maxX = serie.Values.Count
            Next
            Me.maxX = Me.maxX * (Me.barWidth + Me.barDistance)
            Me.xScale = 1 : Me.yScale = 1
            If (Me.minY < Me.maxY) Then Me.yScale = ((Me.renderRect.Height - 5) / (Me.maxY - Me.minY))
            'If (Me.minX < Me.maxX) Then Me.xScale = ((Me.renderRect.Width - 5) / (Me.maxX - Me.minX))
            Me.xOrig = -Me.minX
            Me.yOrig = -Me.minY
        End Sub

        Public Overrides ReadOnly Property Type As ChartTypes
            Get
                Return ChartTypes.VerticalBarsOverlapped
            End Get
        End Property



    End Class

End Namespace
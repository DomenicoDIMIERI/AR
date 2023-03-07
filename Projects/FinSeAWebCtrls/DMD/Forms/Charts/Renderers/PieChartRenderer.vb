Imports DMD
Imports DMD.Sistema
Imports DMD.Forms


Namespace Forms
 
  
    Public Class PieChartRenderer
        Inherits ChartRenderer
        Implements IComparer

        Public Sub New(ByVal chart As CChart)
            MyBase.New(chart)
        End Sub

        Protected Overrides Sub DrawXAxe()
            'MyBase.DrawXAxe()
        End Sub

        Protected Overrides Sub DrawYAxe()
            'MyBase.DrawYAxe()
        End Sub

        Protected Overrides Sub DrawGrid()
            'MyBase.DrawGrid()
        End Sub

        Public Overrides ReadOnly Property Description As String
            Get
                Return "Torta"
            End Get
        End Property

        Protected Overrides Sub CalculateParameters()
            Me.minX = Me.renderRect.Left
            Me.maxX = Me.renderRect.Right
            Me.minY = Me.renderRect.Top
            Me.maxY = Me.renderRect.Bottom
            Me.xOrig = Me.renderRect.Width / 2
            Me.yOrig = Me.renderRect.Height / 2
            Me.xScale = 1
            Me.yScale = 1
        End Sub

        Protected Overrides Sub DrawSerie(serie As CChartSerie)
            

            If (serie.Values.Count = 0) Then Exit Sub
            Dim values() As CChartValue

            values = serie.Values.ToArray
            Arrays.Sort(values, 0, Arrays.Len(values), Me)

            Dim sum As Double = 0
            For i As Integer = 0 To UBound(values)
                sum += values(i).Value
            Next

            Dim x, y, w, h As Single
            x = Me.xOrig
            y = Me.yOrig
            w = Math.Min(Me.renderRect.Width / 2, Me.renderRect.Height / 2)
            h = w

            Dim currSum As Double = 0
            Dim startAngle As Double = 0
            Dim sweepAngle As Double
            For i As Integer = 0 To UBound(values) 'To 0 Step -1
                startAngle = (currSum / sum) * 360
                sweepAngle = (values(i).Value / sum) * 360
                Dim color As System.Drawing.Color = values(i).GetRenderColor
                Dim brush As New System.Drawing.SolidBrush(color)
                Me.FillArc(brush, Me.xOrig, Me.yOrig, w, h, startAngle, sweepAngle)
                brush.Dispose()
                Me.DrawArc(Drawing.Pens.Black, Me.xOrig, Me.yOrig, w, h, startAngle, sweepAngle)
                currSum += values(i).Value
            Next
        End Sub

        Public Overrides ReadOnly Property Type As ChartTypes
            Get
                Return ChartTypes.Pie
            End Get
        End Property

         
        Private Function Compare(x As Object, y As Object) As Integer Implements IComparer.Compare
            Dim a As CChartValue = x
            Dim b As CChartValue = y
            Return b.Value - a.Value
        End Function
    End Class

End Namespace
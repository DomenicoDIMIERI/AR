Imports DMD
Imports DMD.Sistema
Imports DMD.Forms


Namespace Forms
 
  
    ''' <summary>
    ''' Tipo base del renderer dei grafici
    ''' </summary>
    ''' <remarks></remarks>
    Public MustInherit Class ChartRenderer
        Implements IDisposable

        Private m_Chart As CChart
        Private m_Context As System.Drawing.Graphics
        Protected xScale, yScale As Single
        Protected minX, maxX As Single
        Protected minY, maxY As Single?
        Protected minCount, maxCount As Integer
        Protected renderRect As System.Drawing.Rectangle
        Protected legendRect As System.Drawing.Rectangle
        Protected titleRect As System.Drawing.Rectangle
        Protected yAxeRect As System.Drawing.Rectangle
        Protected xAxeRect As System.Drawing.Rectangle
        Protected m_Padding As Single
        Protected xOrig, yOrig As Single
        Private Shared m_DefaultColors() As System.Drawing.Color = Arrays.CreateInstance(Of System.Drawing.Color)(11)
        Private Shared m_DefaultFont As New System.Drawing.Font("Tahoma", 12, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Pixel)
        'Private m_Font As System.Drawing.Font

#Region "Serie Attributes"

        Public Class CSerieAttributes
            Implements IComparable

            Public Serie As CChartSerie
            Public xScale As Single = 1
            Public yScale As Single = 1

            Public Sub New()
                DMD.DMDObject.IncreaseCounter(Me)
            End Sub

            Protected Overrides Sub Finalize()
                MyBase.Finalize()
                DMD.DMDObject.DecreaseCounter(Me)
            End Sub

            Public Function CompareTo(ByVal value As CSerieAttributes) As Integer
                If (Me.Serie Is value.Serie) Then Return 0
                Return 1
            End Function

            Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
                Return Me.CompareTo(obj)
            End Function
        End Class


        Private m_SerieAttributes As CCollection(Of CSerieAttributes)

        Public Overridable Function GetSerieAttributes(ByVal serie As CChartSerie) As CSerieAttributes
            If Me.m_SerieAttributes Is Nothing Then Me.m_SerieAttributes = New CCollection(Of CSerieAttributes)
            Dim i As Integer
            Dim tmp As New CSerieAttributes
            tmp.Serie = serie
            i = m_SerieAttributes.IndexOf(tmp)
            If (i < 0) Then
                If (m_SerieAttributes.Count >= UBound(CChart.DefaultColors)) Then
                    'tmp.Color = Me.GetRandomColor
                Else
                    'tmp.Color = Colors.FromWeb(CChart.DefaultColors(m_SerieAttributes.Count))
                End If
                Me.m_SerieAttributes.Add(tmp)
            Else
                tmp = Me.m_SerieAttributes(i)
            End If
            Return tmp
        End Function


#End Region

        Public Sub New(ByVal chart As CChart)
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_Chart = chart
            Me.m_Padding = 5
            'Me.m_Font = m_DefaultFont
        End Sub

        Public ReadOnly Property Context As System.Drawing.Graphics
            Get
                Return Me.m_Context
            End Get
        End Property
        Protected Friend Sub SetContext(ByVal g As System.Drawing.Graphics)
            Me.m_Context = g
        End Sub

        'Public Property Font As System.Drawing.Font
        '    Get
        '        Return Me.m_Font
        '    End Get
        '    Set(value As System.Drawing.Font)
        '        Me.m_Font = value
        '    End Set
        'End Property

        ''' <summary>
        ''' Restituisce il grafico visualizzato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Chart As CChart
            Get
                Return Me.m_Chart
            End Get
        End Property

        Public Function GetRandomColor() As System.Drawing.Color
            Dim r, g, b As Byte
            r = Rnd(1) * 255
            g = Rnd(1) * 255
            b = Rnd(1) * 255
            Return System.Drawing.Color.FromArgb(255, r, g, b)
        End Function

        ''' <summary>
        ''' Crea il font utilizzato per il titolo
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Overridable Function GetTitleFont() As System.Drawing.Font
            Dim ret As System.Drawing.Font = m_DefaultFont
            Dim f As CFont = Me.Chart.Title.Font
            If (f Is Nothing) Then f = Me.Chart.Font
            If (f IsNot Nothing) Then ret = f.GetSystemFont
            Dim tmp As System.Drawing.Font = New System.Drawing.Font(ret.FontFamily, ret.Size * 1.5F, Drawing.FontStyle.Bold, ret.Unit)
            If (ret IsNot m_DefaultFont) Then ret.Dispose()
            Return tmp
        End Function

        Protected Overridable Function GetLegendFont() As System.Drawing.Font
            Dim ret As System.Drawing.Font = m_DefaultFont
            Dim f As CFont = Me.Chart.Legend.Font
            If (f Is Nothing) Then f = Me.Chart.Font
            If (f IsNot Nothing) Then ret = f.GetSystemFont
            Return ret.Clone
        End Function

        Protected Overridable Function GetXAxeFont() As System.Drawing.Font
            Dim ret As System.Drawing.Font = m_DefaultFont
            Dim f As CFont = Me.Chart.XAxe.Font
            If (f Is Nothing) Then f = Me.Chart.Font
            If (f IsNot Nothing) Then ret = f.GetSystemFont
            Return ret.Clone
        End Function

        Protected Overridable Function GetYAxeFont() As System.Drawing.Font
            Dim ret As System.Drawing.Font = m_DefaultFont
            Dim f As CFont = Me.Chart.YAxe.Font
            If (f Is Nothing) Then f = Me.Chart.Font
            If (f IsNot Nothing) Then ret = f.GetSystemFont
            Return ret.Clone
        End Function

        Protected Overridable Function MeasureTitleElement() As System.Drawing.SizeF
            Using gfont As System.Drawing.Font = Me.GetTitleFont
                Dim s As System.Drawing.SizeF = Me.Context.MeasureString(Me.Chart.Title.Text, gfont)
                s.Width += 10
                s.Height += 5
                Return s
            End Using
        End Function

        Protected Overridable Function MeasureLegendElement() As System.Drawing.SizeF
            Using font As System.Drawing.Font = Me.GetLegendFont
                Dim ret, s As System.Drawing.SizeF
                ret.Width = 0
                ret.Height = 10
                For i As Integer = 0 To Me.Chart.Series.Count - 1
                    Dim serie As CChartSerie = Me.Chart.Series(i)
                    s = Me.Context.MeasureString(serie.Name, font)
                    ret.Height += Math.Min(15, s.Height) + 2
                    If (s.Width > ret.Width) Then ret.Width = s.Width
                Next
                ret.Width += 25
                Return ret
            End Using
        End Function

        Protected Overridable Function MeasureXAxe() As System.Drawing.SizeF
            Return New System.Drawing.SizeF
        End Function

        Protected Overridable Function MeasureYAxe() As System.Drawing.SizeF
            Using font As System.Drawing.Font = Me.GetXAxeFont
                Dim s, ret As New System.Drawing.SizeF
                Dim ms As ChartMeasure
                ret.Height = Me.renderRect.Height
                If (ms.MinY.HasValue AndAlso ms.MaxY.HasValue) Then
                    For i As Integer = 0 To Me.Chart.Series.Count - 1
                        Dim serie As CChartSerie = Me.Chart.Series(i)
                        ms = serie.GetMeasures
                        Dim str As String = Formats.FormatNumber(CStr(ms.MinY), 2)
                        s = Me.Context.MeasureString(str, font)
                        If (s.Width > ret.Width) Then ret.Width = s.Width
                        str = Formats.FormatNumber(CStr(ms.MaxY), 2)
                        s = Me.Context.MeasureString(str, font)
                        If (s.Width > ret.Width) Then ret.Width = s.Width
                    Next
                End If

                ret.Width += 15
                Return ret
            End Using
        End Function

        Protected Overridable Sub CalculateParameters()
            Dim ms As ChartMeasure = Me.Chart.GetMeasures
            Me.minX = ms.MinX
            Me.maxX = ms.MaxX
            Me.minY = Math.Min(0, ms.MinY)
            Me.maxY = Math.Max(0, ms.MaxY)
            Me.xOrig = -ms.MinX
            Me.yOrig = -ms.MinY
            Me.minCount = ms.MinCount
            Me.maxCount = ms.MaxCount

            If (ms.MaxX > ms.MinX) Then
                Me.xScale = (Me.renderRect.Width - 20) / (ms.MaxX - ms.MinX)
            ElseIf (ms.MaxCount > 0) Then
                Me.xScale = (Me.renderRect.Width - 20) / ms.MaxCount
            Else
                Me.xScale = 1
            End If
            If (ms.MaxY > ms.MinY) Then
                Me.yScale = Me.renderRect.Height / (ms.MaxY - ms.MinY)
            Else
                Me.yScale = 1
            End If


        End Sub

        Private Sub DrawXGrid()
            Dim bigpen As System.Drawing.Pen = Nothing
            Dim smallpen As System.Drawing.Pen = Nothing
            Dim font As System.Drawing.Font = Nothing
            Try
                smallpen = New System.Drawing.Pen(Drawing.Brushes.Gray, 0.5)
                bigpen = New System.Drawing.Pen(Drawing.Brushes.Gray, 1)
                font = Me.GetXAxeFont
                If (Me.minY.HasValue = False) Then Return

                Dim diff As Single = Me.maxY - Me.minY
                If (diff <= 0) Then Exit Sub

                If (Me.Chart.YAxe.ScalaLogaritmica) Then
                    Dim s As Single = diff / 10
                    Dim n As Integer = System.Math.Log10(s)
                    s = System.Math.Round(s / (10 ^ n), 1)
                    If (s - Math.Floor(s) > 0.5) Then
                        s = Math.Floor(s) + 1
                    Else
                        s = Math.Floor(s)
                    End If
                    s = s * 10 ^ n

                    Dim n1 As Single = (Math.Floor(Me.minY.Value / s)) * s
                    Dim n2 As Single = (Math.Floor(Me.maxY.Value / s)) * s

                    For i As Single = n1 To n2 Step s
                        If (Me.Chart.YAxe.ShowValues) Then
                            Dim str As String = Formats.FormatNumber(CStr(i), 2)
                            Dim ts As System.Drawing.SizeF = Me.Context.MeasureString(str, font)
                            Me.DrawLine(bigpen, 0, Me.yAxeRect.Top + Me.yAxeRect.Height - i * Me.yScale, Me.yAxeRect.Width, Me.yAxeRect.Top + Me.yAxeRect.Height - i * Me.yScale)
                        End If
                    Next
                Else
                    Dim m As ChartMeasure = Me.Chart.GetMeasures
                    'Dim l As Integer = 1
                    'If (Math.Abs(m.MaxY) > 0) Then l = Math.Log10(Math.Abs(m.MaxY)) ' - m.MinY)
                    Dim n1, n2 As Integer
                    Dim s As Single
                    If (Me.Chart.Grid.ShowSecondaryGrid) Then
                        s = Me.Chart.YAxe.GetSmallStep(m.MinY, m.MaxY) 'Math.Sign(m.MaxY.Value) * 10 ^ (l - 1)
                        n1 = Math.Max(0, Math.Floor(m.MaxY.Value / s))
                        n2 = Math.Min(0, Math.Floor(m.MinY.Value / s))
                        For i As Integer = n2 To n1
                            'If Me.Chart.YAxe.ShowValues Then
                            '    Dim str As String = Formats.FormatNumber(i * s, 2)
                            '    Dim ts As System.Drawing.SizeF = Me.Context.MeasureString(str, font)
                            'End If
                            Me.DrawLine(smallpen, 0, i * s, Me.xAxeRect.Width, i * s)
                        Next
                    End If

                    s = Me.Chart.YAxe.GetLargeStep(m.MinY, m.MaxY) 'Math.Sign(m.MaxY.Value) * 10 ^ (l - 1)
                    n1 = Math.Max(0, Math.Floor(m.MaxY.Value / s))
                    n2 = Math.Min(0, Math.Floor(m.MinY.Value / s))
                    For i As Integer = n2 To n1
                        'If Me.Chart.YAxe.ShowValues Then
                        '    Dim str As String = Formats.FormatNumber(i * s, 2)
                        '    Dim ts As System.Drawing.SizeF = Me.Context.MeasureString(str, font)
                        'End If
                        Me.DrawLine(bigpen, 0, i * s, Me.xAxeRect.Width, i * s)
                    Next
                End If
            Catch ex As Exception
                Throw
            Finally
                If (bigpen IsNot Nothing) Then bigpen.Dispose() : bigpen = Nothing
                If (smallpen IsNot Nothing) Then smallpen.Dispose() : smallpen = Nothing
                If (font IsNot Nothing) Then font.Dispose() : font = Nothing
            End Try
        End Sub

        Private Sub DrawYGrid()
            Dim bigpen As System.Drawing.Pen = Nothing
            Dim smallpen As System.Drawing.Pen = Nothing
            Dim font As System.Drawing.Font = Nothing
            Try
                smallpen = New System.Drawing.Pen(Drawing.Brushes.Gray, 1)
                bigpen = New System.Drawing.Pen(Drawing.Brushes.Gray, 1.5)
                font = Me.GetXAxeFont
                Dim m As ChartMeasure = Me.Chart.GetMeasures
                For i As Integer = 1 To m.MaxCount
                    If (m.MaxY > 0) Then Me.DrawLine(bigpen, i - 1, 0, i - 1, m.MaxY)
                    If (m.MinY > 0) Then Me.DrawLine(bigpen, i - 1, 0, i - 1, m.MinY)
                Next
            Catch ex As Exception
                Throw
            Finally
                If (bigpen IsNot Nothing) Then bigpen.Dispose() : bigpen = Nothing
                If (smallpen IsNot Nothing) Then smallpen.Dispose() : smallpen = Nothing
                If (font IsNot Nothing) Then font.Dispose() : font = Nothing
            End Try
        End Sub

        Protected Overridable Sub DrawGrid()
            If (Me.Chart.Grid.ShowXGrid) Then Me.DrawXGrid()
            If (Me.Chart.Grid.ShowYGrid) Then Me.DrawYGrid()
        End Sub

        'Protected Overridable Sub DrawXGrid()
        '    Dim bigpen As System.Drawing.Pen = Nothing
        '    Dim smallpen As System.Drawing.Pen = Nothing
        '    Try
        '        smallpen = New System.Drawing.Pen(Drawing.Brushes.Gray, 1)
        '        bigpen = New System.Drawing.Pen(Drawing.Brushes.Gray, 1.5)

        '        If (Me.Chart.Grid.ShowXGrid) Then
        '            Dim m As ChartMeasure = Me.Chart.GetMeasures
        '            Dim s As Single = Me.Chart.XAxe.GetLargeStep(m.MinX, m.MaxX)
        '            Dim n1 As Integer = Me.Chart.XAxe.GetLowestValue(m.MinX, m.MaxX)
        '            Dim n2 As Integer = Me.Chart.XAxe.GetUpperValue(m.MinX, m.MaxX)
        '            For i As Integer = n1 To n2
        '                Me.DrawLine(bigpen, i * s, -Me.yOrig, i * s, Me.renderRect.Height)
        '            Next
        '        End If

        '        If (Me.Chart.Grid.ShowYGrid) Then
        '            Dim m As ChartMeasure = Me.Chart.GetMeasures
        '            Dim s As Single = 1
        '            Dim n1 As Double = 1
        '            Dim n2 As Double = 1

        '            If (m.MinY.HasValue AndAlso m.MaxY.HasValue) Then
        '                If (Me.Chart.Grid.ShowSecondaryGrid) Then
        '                    s = Me.Chart.YAxe.GetSmallStep(m.MinY, m.MaxY)
        '                    n1 = Me.Chart.YAxe.GetLowestValue(m.MinY, m.MaxY)
        '                    n2 = Me.Chart.YAxe.GetUpperValue(m.MinY, m.MaxY)

        '                    For i As Integer = n1 To n2
        '                        Me.DrawLine(smallpen, -Me.xOrig, i * s, Me.renderRect.Width, i * s)
        '                    Next
        '                End If

        '                s = Me.Chart.YAxe.GetLargeStep(m.MinY, m.MaxY)
        '                n1 = Me.Chart.YAxe.GetLowestValue(m.MinY, m.MaxY)
        '                n2 = Me.Chart.YAxe.GetUpperValue(m.MinY, m.MaxY)

        '                For i As Integer = n1 To n2
        '                    Me.DrawLine(bigpen, -Me.xOrig, i * s, Me.renderRect.Width, i * s)
        '                Next
        '            End If


        '        End If

        '    Catch ex As Exception
        '        Throw
        '    Finally
        '        If (bigpen IsNot Nothing) Then bigpen.Dispose() : bigpen = Nothing
        '        If (smallpen IsNot Nothing) Then smallpen.Dispose() : smallpen = Nothing
        '    End Try
        'End Sub

        'Protected Overridable Sub DrawGrid()
        '    Dim pen As System.Drawing.Pen = Nothing
        '    Try
        '        pen = New System.Drawing.Pen(Drawing.Brushes.Gray, 1)
        '        If (Me.Chart.Grid.ShowXGrid) Then
        '            Dim m As ChartMeasure = Me.Chart.GetMeasures
        '            Dim s As Single = Me.Chart.XAxe.GetLargeStep(m.MinX, m.MaxX)
        '            Dim n1 As Integer = Me.Chart.XAxe.GetLowestValue(m.MinX, m.MaxX)
        '            Dim n2 As Integer = Me.Chart.XAxe.GetUpperValue(m.MinX, m.MaxX)
        '            For i As Integer = n1 To n2
        '                Me.DrawLine(pen, i * s, -Me.yOrig, i * s, Me.renderRect.Height)
        '            Next
        '        End If
        '        If (Me.Chart.Grid.ShowYGrid) Then
        '            Dim m As ChartMeasure = Me.Chart.GetMeasures
        '            Dim s As Single = 1
        '            Dim n1 As Double = 1
        '            Dim n2 As Double = 1

        '            If (m.MinY.HasValue AndAlso m.MaxY.HasValue) Then
        '                s = Me.Chart.YAxe.GetLargeStep(m.MinY, m.MaxY)
        '                n1 = Me.Chart.YAxe.GetLowestValue(m.MinY, m.MaxY)
        '                n2 = Me.Chart.YAxe.GetUpperValue(m.MinY, m.MaxY)
        '            End If

        '            For i As Integer = n1 To n2
        '                    Me.DrawLine(pen, -Me.xOrig, i * s, Me.renderRect.Width, i * s)
        '                Next
        '            End If

        '    Catch ex As Exception
        '        Throw
        '    Finally
        '        If (pen IsNot Nothing) Then pen.Dispose() : pen = Nothing
        '    End Try
        'End Sub

        Protected Overridable Sub DrawXAxe()
            Dim pen As System.Drawing.Pen = Nothing
            Try
                pen = New System.Drawing.Pen(Drawing.Brushes.Black, 1.5)
                Me.DrawLine(pen, -Me.renderRect.Width / Me.xScale, 0, Me.renderRect.Width / Me.xScale, 0)
                pen.Dispose()
            Catch ex As Exception
                Throw
            Finally
                If (pen IsNot Nothing) Then pen.Dispose() : pen = Nothing
            End Try
        End Sub

        Protected Overridable Sub DrawYAxe()
            Using font As System.Drawing.Font = Me.GetYAxeFont
                Dim pen As System.Drawing.Pen = Nothing
                Try
                    pen = New System.Drawing.Pen(Drawing.Brushes.Black, 1.5)
                    Me.DrawLine(pen, 0, -Me.renderRect.Height / Me.yScale, 0, Me.renderRect.Height / Me.yScale)
                Catch ex As Exception
                    Throw
                Finally
                    If (pen IsNot Nothing) Then pen.Dispose() : pen = Nothing
                End Try

                If (Me.minY.HasValue = False) Then Return

                Dim diff As Single = Me.maxY - Me.minY
                If (diff <= 0) Then Exit Sub

                If (Me.Chart.YAxe.ScalaLogaritmica) Then
                    Dim s As Single = diff / 10
                    Dim n As Integer = System.Math.Log10(s)
                    s = System.Math.Round(s / (10 ^ n), 1)
                    If (s - Math.Floor(s) > 0.5) Then
                        s = Math.Floor(s) + 1
                    Else
                        s = Math.Floor(s)
                    End If
                    s = s * 10 ^ n

                    Dim n1 As Single = (Math.Floor(Me.minY.Value / s)) * s
                    Dim n2 As Single = (Math.Floor(Me.maxY.Value / s)) * s

                    For i As Single = n1 To n2 Step s
                        If (Me.Chart.YAxe.ShowValues) Then
                            Dim str As String = Formats.FormatNumber(CStr(i), Me.Chart.YAxe.Decimals)
                            Dim ts As System.Drawing.SizeF = Me.Context.MeasureString(str, font)
                            Me.Context.DrawString(str, font, Drawing.Brushes.Black, New System.Drawing.Rectangle(Me.yAxeRect.Right - ts.Width, Me.yAxeRect.Top + Me.yAxeRect.Height - i * Me.yScale, 2 * Me.yAxeRect.Width, 100))
                        End If
                    Next
                Else
                    'Dim font As System.Drawing.Font = Nothing
                    Try
                        ' font = New System.Drawing.Font("Tahoma", 10, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
                        Dim m As ChartMeasure = Me.Chart.GetMeasures
                        Dim l As Integer = 1
                        If (Math.Abs(m.MaxY) > 0) Then l = Math.Log10(Math.Abs(m.MaxY)) ' - m.MinY)
                        Dim s As Single = Math.Sign(m.MaxY.Value) * 10 ^ (l - 1)

                        Dim n1 As Integer = Math.Max(0, Math.Floor(m.MaxY.Value / s))
                        Dim n2 As Integer = Math.Min(0, Math.Floor(m.MinY.Value / s))

                        For i As Integer = n2 To n1
                            If Me.Chart.YAxe.ShowValues Then
                                Dim str As String = Formats.FormatNumber(i * s, Me.Chart.YAxe.Decimals)
                                Dim ts As System.Drawing.SizeF = Me.Context.MeasureString(str, font)
                                Me.DrawString(str, font, Drawing.Brushes.Black, (Me.xOrig - ts.Width) / Me.xScale, (ts.Height *0.5) / Me.yScale + i * s)
                            End If
                        Next


                    Catch ex As Exception
                        Throw
                    Finally
                        'If (font IsNot Nothing) Then font.Dispose() : font = Nothing
                    End Try
                End If
            End Using


        End Sub

        Protected Overridable Sub DrawSeries()
            For i As Integer = 0 To Me.Chart.Series.Count - 1
                Dim serie As CChartSerie = Me.Chart.Series(i)
                Me.DrawSerie(serie)
            Next
        End Sub

        Protected MustOverride Sub DrawSerie(ByVal serie As CChartSerie)


        ''' <summary>
        ''' Restituisce il tipo del grafico che questo renderer consente di disegnare
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public MustOverride ReadOnly Property Type As ChartTypes

        ''' <summary>
        ''' Restituisce una descrizione di questo renderer
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public MustOverride ReadOnly Property Description As String

        Protected Overridable Sub SetupRegions(ByVal region As System.Drawing.Rectangle)
            Dim titleSize As System.Drawing.SizeF = Me.MeasureTitleElement
            Dim legendSize As System.Drawing.SizeF = Me.MeasureLegendElement
            Dim xAxeSize As System.Drawing.SizeF = Me.MeasureXAxe
            Dim yAxeSize As System.Drawing.SizeF = Me.MeasureYAxe

            Me.renderRect = New System.Drawing.Rectangle(region.Left + Me.m_Padding, region.Top + Me.m_Padding, region.Width - 2 * Me.m_Padding, region.Height - 2 * Me.m_Padding)
            Select Case Me.Chart.Title.Position
                Case ChartElementPosition.Bottom
                    Me.renderRect.Height -= titleSize.Height
                    Me.titleRect = New Drawing.Rectangle((region.Width - titleSize.Width) / 2, Me.renderRect.Height, titleSize.Width, titleSize.Height)
                Case ChartElementPosition.Top
                    Me.renderRect.Y += titleSize.Height
                    Me.renderRect.Height -= titleSize.Height
                    Me.titleRect = New Drawing.Rectangle((region.Width - titleSize.Width) / 2, 0, titleSize.Width, titleSize.Height)
                Case ChartElementPosition.Left
                    Me.renderRect.X += titleSize.Width
                    Me.renderRect.Width -= titleSize.Width
                    Me.titleRect = New Drawing.Rectangle(0, (Me.renderRect.Height - titleSize.Height) / 2, titleSize.Width, titleSize.Height)
                Case ChartElementPosition.Right
                    Me.renderRect.Width -= titleSize.Width
                    Me.titleRect = New Drawing.Rectangle(Me.renderRect.Width, (Me.renderRect.Height - titleSize.Height) / 2, titleSize.Width, titleSize.Height)
            End Select

            Select Case Me.Chart.Legend.Position
                Case ChartElementPosition.Bottom
                    Me.renderRect.Height -= legendSize.Height
                    Me.legendRect = New Drawing.Rectangle((region.Width - legendSize.Width) / 2, Me.renderRect.Height, legendSize.Width, legendSize.Height)
                Case ChartElementPosition.Top
                    Me.renderRect.Y += legendSize.Height
                    Me.renderRect.Height -= legendSize.Height
                    Me.legendRect = New Drawing.Rectangle((region.Width - legendSize.Width) / 2, 0, legendSize.Width, legendSize.Height)
                Case ChartElementPosition.Left
                    Me.renderRect.X += legendSize.Width
                    Me.renderRect.Width -= legendSize.Width
                    Me.legendRect = New Drawing.Rectangle(0, (Me.renderRect.Height - legendSize.Height) / 2, legendSize.Width, legendSize.Height)
                Case ChartElementPosition.Right
                    Me.renderRect.Width -= legendSize.Width
                    Me.legendRect = New Drawing.Rectangle(Me.renderRect.Width, (Me.renderRect.Height - legendSize.Height) / 2, legendSize.Width, legendSize.Height)
            End Select

            Me.renderRect.X += yAxeSize.Width
            Me.renderRect.Width -= yAxeSize.Width
            Me.yAxeRect = New System.Drawing.Rectangle(0, Me.renderRect.Top, yAxeSize.Width, Me.renderRect.Height)

            Me.renderRect.Height -= Me.xAxeRect.Height
            Me.xAxeRect = New System.Drawing.Rectangle(Me.renderRect.Left, Me.renderRect.Top + Me.renderRect.Height, Me.renderRect.Width, xAxeSize.Height)
        End Sub

        ''' <summary>
        ''' Disegna il grafico
        ''' </summary>
        ''' <param name="g"></param>
        ''' <param name="region"></param>
        ''' <remarks></remarks>
        Public Sub PaintTo(ByVal g As System.Drawing.Graphics, ByVal region As System.Drawing.Rectangle)
            'Dim oldClip As System.Drawing.Region = g.Clip
            'Dim oldTransform As System.Drawing.Drawing2D.Matrix = g.Transform

            Me.SetContext(g)

            g.Clear(Drawing.Color.White)

            'g.SetClip(region)
            Me.SetupRegions(region)
            Me.CalculateParameters()

            'g.TranslateTransform(region.X + Me.xOrig * Me.xScale, region.Y + Me.yOrig * Me.yScale)
            'g.ScaleTransform(Me.xScale, Me.yScale)
            Me.DrawGrid()
            Me.DrawSeries()
            Me.DrawXAxe()
            Me.DrawYAxe()
            Me.DrawLegend()
            Me.DrawTitle()


            'g.Transform = oldTransform
            'g.Clip = oldClip
        End Sub

        Public Overrides Function ToString() As String
            Return Me.Description
        End Function

        Protected Overridable Sub FillRectangle(ByVal brush As System.Drawing.Brush, ByVal x As Single, ByVal y As Single, ByVal width As Single, ByVal height As Single)
            With Me.renderRect
                Dim x1 As Integer = .Left + (Me.xOrig + x) * Me.xScale
                Dim y1 As Integer = .Top + .Height - (y + Me.yOrig + height) * Me.yScale
                Dim w1 As Integer = width * Me.xScale
                Dim h1 As Integer = height * Me.yScale
                If (w1 <> 0) And (h1 <> 0) Then
                    Me.Context.FillRectangle(brush, x1, y1, w1, h1)
                End If
            End With
        End Sub

        Protected Overridable Sub FillRectangle(ByVal brush As System.Drawing.Brush, ByVal rect As System.Drawing.Rectangle)
            With Me.renderRect
                Dim x1 As Integer = .Left + (rect.X + Me.xOrig) * Me.xScale
                Dim y1 As Integer = .Top + .Height - (rect.Y + Me.yOrig + rect.Height) * Me.yScale
                Dim w1 As Integer = rect.Width * Me.xScale
                Dim h1 As Integer = rect.Height * Me.yScale
                If (w1 <> 0) And (h1 <> 0) Then
                    Me.Context.FillRectangle(brush, x1, y1, w1, h1)
                End If
            End With
        End Sub

        Protected Overridable Sub DrawRectangle(ByVal pen As System.Drawing.Pen, ByVal x As Single, ByVal y As Single, ByVal width As Single, ByVal height As Single)
            With Me.renderRect
                Dim x1 As Single = .Left + (x + Me.xOrig) * Me.xScale
                Dim y1 As Single = .Top + .Height - (y + Me.yOrig + height) * Me.yScale
                Dim w1 As Single = width * Me.xScale
                Dim h1 As Single = height * Me.yScale
                If (w1 <> 0) And (h1 <> 0) Then
                    Me.Context.DrawRectangle(pen, x1, y1, w1, h1)
                End If
            End With
        End Sub

        Protected Overridable Sub DrawRectangle(ByVal pen As System.Drawing.Pen, ByVal rect As System.Drawing.Rectangle)
            With Me.renderRect
                Dim x1 As Single = .Left + (rect.X + Me.xOrig) * Me.xScale
                Dim y1 As Single = .Top + .Height - (rect.Y + Me.yOrig + rect.Height) * Me.yScale
                Dim w1 As Single = rect.Width * Me.xScale
                Dim h1 As Single = rect.Height * Me.yScale
                If (w1 <> 0) And (h1 <> 0) Then
                    Me.Context.DrawRectangle(pen, x1, y1, w1, h1)
                End If
            End With
        End Sub

        Protected Overridable Sub DrawEllipse(ByVal pen As System.Drawing.Pen, ByVal x As Single, ByVal y As Single, ByVal width As Single, ByVal height As Single)
            With Me.renderRect
                Me.Context.DrawEllipse(pen,
                    New System.Drawing.Rectangle(.Left + (Me.xOrig + x) * Me.xScale, .Top + .Height - (y + Me.yOrig) * Me.yScale, width * Me.xScale, height * Me.yScale))
            End With
        End Sub

        Protected Overridable Sub DrawLine(ByVal pen As System.Drawing.Pen, ByVal x1 As Single, ByVal y1 As Single, ByVal x2 As Single, ByVal y2 As Single)
            With Me.renderRect
                Me.Context.DrawLine(pen, .Left + (Me.xOrig + x1) * Me.xScale, .Top + .Height - (y1 + Me.yOrig) * Me.yScale, .Left + (Me.xOrig + x2) * Me.xScale, .Top + .Height - (Me.yOrig + y2) * Me.yScale)
            End With
        End Sub

        Protected Overridable Sub DrawArc(ByVal pen As System.Drawing.Pen, ByVal x As Single, ByVal y As Single, ByVal width As Single, ByVal height As Single, ByVal startAngle As Single, ByVal sweepAngel As Single)
            'With Me.renderRect
            '    Dim x1 As Single = .Left + (Me.xOrig + x) * Me.xScale
            '    Dim y1 As Single = .Top + .Height - (Me.yOrig + y) * Me.yScale
            '    Dim w1 As Single = width * Me.xScale
            '    Dim h1 As Single = height * Me.yScale
            '    If (w1 <> 0) And (h1 <> 0) Then
            '        Me.Context.DrawArc(pen, x1 - w1, y1 - h1, w1 * 2, h1 * 2, 90 - startAngle, sweepAngel)
            '    End If
            'End With
            Me.Context.DrawArc(pen, New System.Drawing.Rectangle(Me.renderRect.Left + x - width / 2, Me.renderRect.Top + y - height / 2, width, height), -90 + startAngle, sweepAngel)
        End Sub

        Protected Overridable Sub FillArc(ByVal brush As System.Drawing.Brush, ByVal x As Single, ByVal y As Single, ByVal width As Single, ByVal height As Single, ByVal startAngle As Single, ByVal sweepAngel As Single)
            'With Me.renderRect
            'Dim x1 As Single = .Left + (Me.xOrig + x) * Me.xScale
            'Dim y1 As Single = .Top + .Height - (Me.yOrig + y) * Me.yScale
            'Dim w1 As Single = width * Me.xScale
            'Dim h1 As Single = height * Me.yScale
            Me.Context.FillPie(brush, New System.Drawing.Rectangle(Me.renderRect.Left + x - width / 2, Me.renderRect.Top + y - height / 2, width, height), -90 + startAngle, sweepAngel)
            '    Me.Context.FillPie(brush, x1 - w1, y1 - h1, w1 * 2, h1 * 2, startAngle, sweepAngel)

            'End With
        End Sub 'Sub

        Protected Overridable Sub DrawString(ByVal text As String, ByVal font As System.Drawing.Font, ByVal brush As System.Drawing.Brush, ByVal rect As System.Drawing.Rectangle)
            With Me.renderRect
                Dim x1 As Single = .Left + (Me.xOrig + rect.X) * Me.xScale
                Dim y1 As Single = .Top + .Height - (rect.Y + Me.yOrig + rect.Height) * Me.yScale
                Dim w1 As Single = rect.Width * Me.xScale
                Dim h1 As Single = rect.Height * Me.yScale
                If (w1 <> 0) And (h1 <> 0) Then
                    Me.Context.DrawString(text, font, brush, rect)
                End If
            End With
        End Sub

        Protected Overridable Sub DrawString(ByVal text As String, ByVal font As System.Drawing.Font, ByVal brush As System.Drawing.Brush, ByVal x As Single, ByVal y As Single)
            With Me.renderRect
                Dim x1 As Single = .Left + (Me.xOrig + x) * Me.xScale
                Dim y1 As Single = .Top + .Height - (y + Me.yOrig) * Me.yScale
                Me.Context.DrawString(text, font, brush, x1, y1)
            End With
        End Sub

        Protected Overridable Sub DrawTitle()
            Dim fontg As System.Drawing.Font = Me.GetTitleFont
            Dim brush As New System.Drawing.SolidBrush(Me.Chart.Title.ForeColor)
            Me.Context.DrawString(Me.Chart.Title.Text, fontg, brush, Me.titleRect)
            fontg.Dispose()
            brush.Dispose()
        End Sub

        Protected Overridable Sub DrawLegend()
            Using font As System.Drawing.Font = Me.GetLegendFont
                Dim brush As New System.Drawing.SolidBrush(Me.Chart.Legend.BackColor)
                Dim pen As System.Drawing.Pen
                Dim y As Single
                Dim s As System.Drawing.SizeF
                Me.Context.FillRectangle(brush, Me.legendRect)
                brush.Dispose()
                If Me.Chart.Legend.BorderSize > 0 Then
                    pen = New System.Drawing.Pen(Me.Chart.Legend.BorderColor, Me.Chart.Legend.BorderSize)
                    Me.Context.DrawRectangle(pen, Me.legendRect)
                    pen.Dispose()
                End If
                y = Me.legendRect.Top + 5
                If Me.Chart.Legend.Text <> vbNullString Then
                    s = Me.Context.MeasureString(Me.Chart.Legend.Text, font)
                    Me.Context.DrawString(Me.Chart.Legend.Text, font, Drawing.Brushes.Black, Me.legendRect.Left + 5, y)
                    y += s.Height + 2
                End If
                For i As Integer = 0 To Me.Chart.Series.Count - 1
                    Dim serie As CChartSerie = Me.Chart.Series(i)
                    s = Me.Context.MeasureString(serie.Name, font)
                    brush = New System.Drawing.SolidBrush(serie.GetRenderColor)
                    Me.Context.FillRectangle(brush, Me.legendRect.Left + 5, y - 1, 15, 15)
                    Me.Context.DrawString(serie.Name, font, Drawing.Brushes.Black, Me.legendRect.Left + 25, y)
                    y += s.Height + 2
                    brush.Dispose()
                Next
            End Using

        End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Overridable Sub Dispose() Implements IDisposable.Dispose
            Me.m_Chart = Nothing
            Me.m_Context = Nothing
            'Me.renderRect = Nothing:             Me.legendRect = Nothing:             Me.titleRect = Nothing:             Me.yAxeRect = Nothing:             Me.xAxeRect = Nothing
            'Me.m_Font = Nothing
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class


End Namespace
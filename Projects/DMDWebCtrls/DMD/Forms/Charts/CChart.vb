Imports DMD
Imports DMD.Sistema
Imports DMD.Forms


Namespace Forms


    Public Enum ChartTypes As Integer
        Unset = 0
        VerticalBarsOverlapped = 10
        Lines = 20
        Pie = 30
        VerticalBarsMultibars = 40
        VerticalBars3D = 50
    End Enum



    Public Class CChart
        Inherits WebControl
        Private m_Title As CChartTitle
        Private m_Legend As CChartLegend
        Private m_XAxe As CChartAxe
        Private m_YAxe As CChartAxe
        Private m_Type As ChartTypes
        Private m_Renderer As ChartRenderer
        Private m_Series As CChartSeries
        Private m_Labels As CChartLabels
        Private m_Grid As CChartGrid

        Public Shared DefaultColors() As String = {"40699c", "9e413e", "7f9a48", "695185", "3c8da3", "cc7b38", "4f81bd", "c0504d", "9bbb59", "8064a2"}


        Public Sub New()
            Me.m_Title = New CChartTitle(Me)
            Me.m_Legend = New CChartLegend(Me)
            Me.m_XAxe = New CChartAxe("X", Me)
            Me.m_YAxe = New CChartAxe("Y", Me)
            Me.m_Type = ChartTypes.Lines
            Me.m_Renderer = Nothing
            Me.m_Series = Nothing
            Me.m_Labels = Nothing
            Me.m_Grid = New CChartGrid(Me)
        End Sub

        Protected Friend Sub Invalidate()

        End Sub


        ''' <summary>
        ''' Accede alla griglia
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Grid As CChartGrid
            Get
                If Me.m_Grid Is Nothing Then Me.m_Grid = New CChartGrid(Me)
                Return Me.m_Grid
            End Get
        End Property

        ''' <summary>
        ''' Accede alle etichette
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Labels As CChartLabels
            Get
                If Me.m_Labels Is Nothing Then Me.m_Labels = New CChartLabels(Me)
                Return Me.m_Labels
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta il tipo del grafico
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Type As ChartTypes
            Get
                Return Me.m_Type
            End Get
            Set(value As ChartTypes)
                If (Me.m_Type = value) Then Exit Property
                Me.m_Type = value
                Me.m_Renderer = Nothing
                Me.Invalidate()
            End Set
        End Property

        ''' <summary>
        ''' Accede alle proprietà dell'asse X
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property XAxe As CChartAxe
            Get
                Return Me.m_XAxe
            End Get
        End Property

        ''' <summary>
        ''' Accede alle proprietà dell'asse Y
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property YAxe As CChartAxe
            Get
                Return Me.m_YAxe
            End Get
        End Property

        ''' <summary>
        ''' Restituisce la collezione delle serie di dati rappresentate dal grafico
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Series As CChartSeries
            Get
                If Me.m_Series Is Nothing Then Me.m_Series = New CChartSeries(Me)
                Return Me.m_Series
            End Get
        End Property

        ''' <summary>
        ''' Restituisce il titolo del grafico
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Title As CChartTitle
            Get
                Return Me.m_Title
            End Get
        End Property

        Public ReadOnly Property Legend As CChartLegend
            Get
                Return Me.m_Legend
            End Get
        End Property

        ''' <summary>
        ''' Restituisce il renderer predefinito del tipo di grafico
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetDefaultRenderer() As ChartRenderer
            Select Case Me.Type
                Case ChartTypes.VerticalBarsMultibars : Return New VerticalBarChartMultibarsRenderer(Me)
                Case ChartTypes.VerticalBarsOverlapped : Return New VerticalBarOverlappedChartRenderer(Me)
                Case ChartTypes.Lines : Return New LineChartRenderer(Me)
                Case ChartTypes.Pie : Return New PieChartRenderer(Me)
                Case Else : Throw New NotSupportedException("Il tipo di grafico specificato non è supportato")
            End Select
        End Function

        Public Overrides Sub GetInnerHTML(ByVal writer As System.Web.UI.HtmlTextWriter)
            Dim rect As New System.Drawing.Rectangle(0, 0, CInt(Me.Width), CInt(Me.Height))
            Dim bmp As New System.Drawing.Bitmap(rect.Width, rect.Height)
            Dim gph As System.Drawing.Graphics = System.Drawing.Graphics.FromImage(bmp)
            Me.PaintTo(gph, rect)
            gph.Dispose()
            Dim fn As String = DMD.Sistema.FileSystem.GetTempFileName("bmp")
            bmp.Save(fn, System.Drawing.Imaging.ImageFormat.Bmp)
            bmp.Dispose()


            writer.Write("<img src=""")
            writer.Write(ApplicationContext.UnMapPath(fn))
            writer.Write(""" border=""0"" alt=""Chart"" style=""width:")
            writer.Write(Me.Width)
            writer.Write("px;height:")
            writer.Write(Me.Height)
            writer.Write("px;"" />")
        End Sub


        ''' <summary>
        ''' Disegna il grafico nel rettandolo specificato nell'area grafica
        ''' </summary>
        ''' <param name="g"></param>
        ''' <param name="rect"></param>
        ''' <remarks></remarks>
        Public Sub PaintTo(ByVal g As System.Drawing.Graphics, ByVal rect As System.Drawing.Rectangle)
            Me.GetDefaultRenderer.PaintTo(g, rect)
        End Sub

        Public Function GetMeasures() As ChartMeasure
            Dim ret As ChartMeasure = Me.Series.GetMeasures
            If (Me.XAxe.MinValue.HasValue) Then ret.MinX = Me.XAxe.MinValue.Value
            If (Me.XAxe.MaxValue.HasValue) Then ret.MaxX = Me.XAxe.MaxValue.Value
            If (Me.YAxe.MinValue.HasValue) Then ret.MinY = Me.YAxe.MinValue.Value
            If (Me.YAxe.MaxValue.HasValue) Then ret.MaxY = Me.YAxe.MaxValue.Value
            Return ret
        End Function

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Type" : Me.m_Type = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Title" : Me.m_Title = fieldValue : Me.m_Title.SetChart(Me)
                Case "Legend" : Me.m_Legend = fieldValue : Me.m_Legend.SetChart(Me)
                Case "XAxe" : Me.m_XAxe = fieldValue : Me.m_XAxe.SetChart(Me)
                Case "YAxe" : Me.m_YAxe = fieldValue : Me.m_YAxe.SetChart(Me)
                Case "Grid" : Me.m_Grid = fieldValue : Me.m_Grid.SetChart(Me)
                Case "Series" : Me.m_Series = fieldValue : Me.m_Series.SetChart(Me)
                Case "Labels" : Me.m_Labels = fieldValue : Me.m_Labels.SetChart(Me)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Type", Me.m_Type)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Title", Me.Title)
            writer.WriteTag("Legend", Me.Legend)
            writer.WriteTag("XAxe", Me.XAxe)
            writer.WriteTag("YAxe", Me.YAxe)
            writer.WriteTag("Grid", Me.Grid)
            writer.WriteTag("Series", Me.Series)
            writer.WriteTag("Labels", Me.Labels)
        End Sub

    End Class




   
End Namespace
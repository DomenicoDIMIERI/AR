Imports DMD
Imports DMD.Sistema
Imports DMD.Forms


Namespace Forms

    <Serializable> _
    Public Class CChartSeries
        Inherits CKeyCollection(Of CChartSerie)

        <NonSerialized> Private m_Chart As CChart

        Public Sub New()
        End Sub
        Public Sub New(ByVal chart As CChart)
            Me.New()
            Me.m_Chart = chart
        End Sub

        Public ReadOnly Property Chart As CChart
            Get
                Return Me.m_Chart
            End Get
        End Property

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Chart IsNot Nothing) Then DirectCast(value, CChartSerie).SetChart(Me.m_Chart)
            MyBase.OnInsert(index, value)
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_Chart IsNot Nothing) Then DirectCast(newValue, CChartSerie).SetChart(Me.m_Chart)
            MyBase.OnSet(index, oldValue, newValue)
        End Sub


        Public Overloads Function Add(ByVal serieName As String) As CChartSerie
            Dim item As New CChartSerie
            item.Name = serieName
            Me.Add(serieName, item)
            Return item
        End Function

        Public Function GetMeasures() As ChartMeasure
            Dim ret As ChartMeasure = Nothing
            If (Me.Count > 0) Then
                ret = Me(0).GetMeasures
                For i As Integer = 1 To Me.Count - 1
                    ret = ret.Combine(Me(i).GetMeasures)
                Next
            End If
            Return ret
        End Function

        Protected Friend Overridable Sub SetChart(ByVal value As CChart)
            Me.m_Chart = value
            For Each s As CChartSerie In Me
                s.SetChart(value)
            Next
        End Sub

    End Class


End Namespace
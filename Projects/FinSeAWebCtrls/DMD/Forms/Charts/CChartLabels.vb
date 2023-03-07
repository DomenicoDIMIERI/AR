Imports DMD
Imports DMD.Sistema
Imports DMD.Forms


Namespace Forms

    <Serializable> _
    Public Class CChartLabels
        Inherits CCollection(Of CChartLabel)

        <NonSerialized> Private m_Chart As CChart

        Public Sub New()
        End Sub
        Public Sub New(ByVal chart As CChart)
            Me.New()
            Me.SetChart(chart)
        End Sub

        Protected Friend Sub SetChart(ByVal value As CChart)
            Me.m_Chart = value
            For Each l As CChartLabel In Me
                l.SetChart(value)
            Next
        End Sub

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Chart IsNot Nothing) Then DirectCast(value, CChartLabel).SetChart(Me.m_Chart)
            MyBase.OnInsert(index, value)
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_Chart IsNot Nothing) Then DirectCast(newValue, CChartLabel).SetChart(Me.m_Chart)
            MyBase.OnSet(index, oldValue, newValue)
        End Sub

        Public Overloads Function Add(ByVal label As String) As CChartLabel
            Dim item As New CChartLabel
            With item
                .Label = label
                .X = Me.Count
                .Color = Utils.FormsUtils.GenerateRandomColor()
            End With
            Me.Add(item)
            Return item
        End Function

        Public Overloads Function Add(ByVal label As String, ByVal x As Single) As CChartLabel
            Dim item As New CChartLabel
            With item
                .Label = label
                .X = x
                .Color = Utils.FormsUtils.GenerateRandomColor()
            End With
            Me.Add(item)
            Return item
        End Function

        'Public Sub GetMinMax(ByRef min As Single, ByRef max As Single)
        '    min = 0 : max = 0
        '    If Me.Count = 0 Then Exit Sub
        '    min = Me(0).Value : max = min
        '    For Each Item As CChartValue In Me
        '        If Item.Value > max Then
        '            max = Item.Value
        '        ElseIf Item.Value < min Then
        '            min = Item.Value
        '        End If
        '    Next
        'End Sub

        Public Function GetMeasures() As ChartMeasure
            Dim ret As New ChartMeasure
            ret.MaxCount = Me.Count
            ret.MinCount = Me.Count
            If (Me.Count > 0) Then
                With Me(0)
                    ret.MaxX = .X
                    ret.MinX = .X
                End With
                For i As Integer = 1 To Me.Count - 1
                    With Me(i)
                        If (.X < ret.MinX) Then
                            ret.MinX = .X
                        ElseIf (.X > ret.MaxX) Then
                            ret.MaxX = .X
                        End If
                    End With
                Next
            End If
            Return ret
        End Function


    End Class

  

End Namespace
Imports DMD
Imports DMD.Sistema
Imports DMD.Forms


Namespace Forms
 
    <Serializable> _
    Public Class CChartValues
        Inherits CCollection(Of CChartValue)

        <NonSerialized> Private m_Serie As CChartSerie

        Public Sub New()
        End Sub
        Public Sub New(ByVal serie As CChartSerie)
            Me.New()
            Me.SetSerie(serie)
        End Sub

        Protected Friend Sub SetSerie(ByVal value As CChartSerie)
            Me.m_Serie = value
            For Each v As CChartValue In Me
                v.SetSerie(value)
            Next
        End Sub

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Serie IsNot Nothing) Then DirectCast(value, CChartValue).SetSerie(Me.m_Serie)
            MyBase.OnInsert(index, value)
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_Serie IsNot Nothing) Then DirectCast(newValue, CChartValue).SetSerie(Me.m_Serie)
            MyBase.OnSet(index, oldValue, newValue)
        End Sub

        Public Overloads Function Add(ByVal y As Single?, Optional ByVal label As String = vbNullString) As CChartValue
            Dim item As New CChartValue
            With item
                .Label = label
                .Value = y
                .Color = Utils.FormsUtils.GetRandomColor()
            End With
            Me.Add(item)
            Return item
        End Function

        Public Overloads Function Add(ByVal y As Single?, ByVal x As Single, ByVal label As String) As CChartValue
            Dim item As New CChartValue
            With item
                .Label = label
                .X = x
                .Value = y
                .Color = Utils.FormsUtils.GetRandomColor()
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
                    ret.MaxY = .Value
                    ret.MinY = .Value
                End With
                For i As Integer = 1 To Me.Count - 1
                    With Me(i)
                        If (.X < ret.MinX) Then
                            ret.MinX = .X
                        ElseIf (.X > ret.MaxX) Then
                            ret.MaxX = .X
                        End If

                        ret.MinY = Math.Min(.Value, ret.MinY)
                        ret.MaxY = Math.Max(.Value, ret.MaxY)

                    End With
                Next
            End If
            Return ret
        End Function

    End Class


End Namespace
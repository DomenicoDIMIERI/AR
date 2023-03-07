Imports DMD
Imports DMD.Sistema
Imports DMD.Forms


Namespace Forms


    Public Class CChartLegend
        Inherits CChartElement
        
        Public Sub New()
            Me.BackColor = Drawing.Color.White
            Me.Position = ChartElementPosition.Right
            Me.BorderSize = 1
            Me.BorderColor = Drawing.Color.Gray
        End Sub

        Public Sub New(ByVal owner As CChart)
            MyBase.New(owner)
            Me.BackColor = Drawing.Color.White
            Me.Position = ChartElementPosition.Right
            Me.BorderSize = 1
            Me.BorderColor = Drawing.Color.Gray
        End Sub
         
    End Class


End Namespace
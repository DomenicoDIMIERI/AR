Imports DMD
Imports DMD.Sistema
Imports DMD.Forms


Namespace Forms
 
    ''' <summary>
    ''' Informazioni su una serie o su una collezione di serie
    ''' </summary>
    ''' <remarks></remarks>
    Public Structure ChartMeasure
        Public MinX As Single
        Public MaxX As Single
        Public MinY As Single?
        Public MaxY As Single?
        Public MinCount As Integer
        Public MaxCount As Integer

        Public Function Combine(ByVal value As ChartMeasure) As ChartMeasure
            Dim ret As New ChartMeasure
            ret.MaxCount = Math.Max(Me.MaxCount, value.MaxCount)
            ret.MinCount = Math.Min(Me.MinCount, value.MinCount)
            ret.MaxY = Math.Max(Me.MaxY, value.MaxY)
            ret.MinY = Math.Min(Me.MinY, value.MinY)
            ret.MaxX = Math.Max(Me.MaxX, value.MaxX)
            ret.MinX = Math.Min(Me.MinX, value.MinX)
            Return ret
        End Function

    End Structure


End Namespace
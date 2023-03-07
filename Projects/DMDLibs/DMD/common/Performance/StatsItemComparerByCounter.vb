Imports DMD
Imports DMD.Databases
Imports System.Net

    
''' <summary>
''' Compare gli oggetto StatsItem sulla base del numero di esecuzioni (campo Count) per ottenere un ordinamento decrescente
''' </summary>
''' <remarks></remarks>
Public Class StatsItemComparerByCounter
    Implements IComparer

    Public Sub New()
        DMD.DMDObject.IncreaseCounter(Me)
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        DMD.DMDObject.DecreaseCounter(Me)
    End Sub

    Public Function Compare(x As StatsItem, y As StatsItem) As Integer
        Dim ret As Integer = y.Count - x.Count
        If (ret = 0) Then ret = Sistema.Strings.Compare(x.Name, y.Name, CompareMethod.Text)
        Return ret
    End Function

    Private Function Compare1(x As Object, y As Object) As Integer Implements IComparer.Compare
        Return Me.Compare(x, y)
    End Function
End Class
 
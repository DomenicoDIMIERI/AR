Imports DMD
Imports DMD.Databases
Imports System.Net

''' <summary>
''' Compara gli oggetti StatsItem sulla base del campo Name in ordine crescente
''' </summary>
''' <remarks></remarks>
Public Class StatsItemComparerByName
    Implements IComparer

    Public Sub New()
        DMD.DMDObject.IncreaseCounter(Me)
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        DMD.DMDObject.DecreaseCounter(Me)
    End Sub

    Public Function Compare(x As StatsItem, y As StatsItem) As Integer
        Dim ret As Integer = Sistema.Strings.Compare(x.Name, y.Name, CompareMethod.Text)
        If (ret = 0) Then ret = y.Count - x.Count
        Return ret
    End Function

    Private Function Compare1(x As Object, y As Object) As Integer Implements IComparer.Compare
        Return Me.Compare(x, y)
    End Function
End Class
 
 
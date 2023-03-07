Public Class CFunctionComparer
    Implements IComparer

    Public Delegate Function funDelegate(ByVal a As Object, ByVal b As Object) As Integer

    Public fun As funDelegate

    Public Sub New(ByVal fun As funDelegate)
        Me.fun = fun
        DMD.DMDObject.IncreaseCounter(Me)
    End Sub

    Public Function Compare(ByVal a As Object, ByVal b As Object) As Integer Implements IComparer.Compare
        Return Me.fun(a, b)
    End Function

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        DMD.DMDObject.DecreaseCounter(Me)
    End Sub
End Class

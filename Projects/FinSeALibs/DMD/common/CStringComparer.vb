Public Class CStringComparer
    Implements IComparer

    Public CompareMethod As CompareMethod

    Public Sub New(Optional ByVal cm As CompareMethod = Microsoft.VisualBasic.CompareMethod.Text)
        DMD.DMDObject.IncreaseCounter(Me)
        Me.CompareMethod = cm
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        DMD.DMDObject.DecreaseCounter(Me)
    End Sub

    Public Function Compare(ByVal a As Object, ByVal b As Object) As Integer Implements IComparer.Compare
        Dim x As String = a
        Dim y As String = b
        Return Sistema.Strings.Compare(x, y, Me.CompareMethod)
    End Function


End Class
